using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using MeteoInfoC.Projections;
using MeteoInfoC.Global;
using MeteoInfoC.Layer;
using MeteoInfoC.Shape;
using MeteoInfoC.Legend;
using MeteoInfoC.Drawing;
using MeteoInfoC.Geoprocess;

namespace MeteoInfoC.Map
{
    /// <summary>
    /// Projection set
    /// </summary>
    public class ProjectionSet
    {
        #region Events definition
        /// <summary>
        /// Occurs after projection changed
        /// </summary>
        public event EventHandler ProjectionChanged;

        #endregion

        #region Variables

        //const double PI = 3.1415926535;

        //private bool _isLonLatMap;        
        private ProjectionInfo _projInfo;
        private string _projStr;
        private double _refLon;
        private double _refCutLon;
        #endregion

        #region Construction
        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectionSet()
        {
            //_isLonLatMap = true;            
            _projInfo = KnownCoordinateSystems.Geographic.World.WGS1984;
            _projStr = _projInfo.ToProj4String();
            _refLon = 0;
        }
        #endregion

        #region Property

        /// <summary>
        /// Get if the map is lon/lat
        /// </summary>
        public bool IsLonLatMap
        {
            get { return _projInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat; }        
        }        

        /// <summary>
        /// Get or set projection info
        /// </summary>
        public ProjectionInfo ProjInfo
        {
            get { return _projInfo; }
            set { _projInfo = value; }
        }

        /// <summary>
        /// Get or set projection string
        /// </summary>
        public string ProjStr
        {
            get { return _projStr; }
            set { _projStr = value; }
        }

        /// <summary>
        /// Get or set reference longitude as central meridian
        /// </summary>
        public double RefLon
        {
            get { return _refLon; }
            set { _refLon = value; }
        }

        /// <summary>
        /// Get or set reference cut longitude which is used in projection
        /// </summary>
        public double RefCutLon
        {
            get { return _refCutLon; }
            set { _refCutLon = value; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get projected extent from Lon/Lat
        /// </summary>
        /// <param name="aLLSS"></param>
        /// <returns></returns>
        public Extent GetProjectedExtentFromLonLat(MapExtentSet aLLSS)
        {
            Extent aExtent = new Extent();
            //Define coordinate system
            //ICoordinateSystem fromCS, toCS;
            //toCS = mapCoordinateSystem;
            //fromCS = GeographicCoordinateSystem.WGS84;
            //CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
            //ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(fromCS, toCS);
            ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
            ProjectionInfo toProj = _projInfo;

            //Get the border of longitude and latitude
            //List<double[]> fromList = new List<double[]>();
            //List<double[]> toList = new List<double[]>();
            //fromList.Add(new double[] { aLLSS.minLon, aLLSS.minLat });
            //fromList.Add(new double[] { aLLSS.minLon, aLLSS.maxLat });
            //fromList.Add(new double[] { aLLSS.maxLon, aLLSS.maxLat });
            //fromList.Add(new double[] { aLLSS.maxLon, aLLSS.minLat });
            //toList = trans.MathTransform.TransformList(fromList);
            double[][] points = new double[4][];
            points[0] = new double[] { aLLSS.MinX, aLLSS.MinY };
            points[1] = new double[] { aLLSS.MinX, aLLSS.MaxY };
            points[2] = new double[] { aLLSS.MaxX, aLLSS.MaxY };
            points[3] = new double[] { aLLSS.MaxX, aLLSS.MinY };
            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);

            //Get lon lat extent
            aExtent.minX = Math.Min(points[0][0], points[1][0]);
            aExtent.minY = Math.Min(points[0][1], points[3][1]);
            aExtent.maxX = Math.Max(points[2][0], points[3][0]);
            aExtent.maxY = Math.Max(points[1][1], points[2][1]);

            return aExtent;
        }

        /// <summary>
        /// Get projected extent from Lon/Lat
        /// </summary>
        /// <param name="sExtent">lon/lat extent</param>
        /// <returns>projected extent</returns>
        public Extent GetProjectedExtentFromLonLat(Extent sExtent)
        {
            Extent aExtent = new Extent();            
            ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
            ProjectionInfo toProj = _projInfo;

            //Get the border of longitude and latitude            
            double[][] points = new double[4][];
            points[0] = new double[] { sExtent.minX, sExtent.minY };
            points[1] = new double[] { sExtent.minX, sExtent.maxY };
            points[2] = new double[] { sExtent.maxX, sExtent.maxY };
            points[3] = new double[] { sExtent.maxX, sExtent.minY };
            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);

            //Get lon lat extent
            aExtent.minX = Math.Min(points[0][0], points[1][0]);
            aExtent.minY = Math.Min(points[0][1], points[3][1]);
            aExtent.maxX = Math.Max(points[2][0], points[3][0]);
            aExtent.maxY = Math.Max(points[1][1], points[2][1]);

            return aExtent;
        }

        /// <summary>
        /// Get longitude/latitude extent from a projected extent
        /// </summary>
        /// <param name="sExtent">projected extent</param>
        /// <returns>longitude/latitude extent</returns>
        public Extent GetLonLatExtent(Extent sExtent)
        {
            if (_projInfo.IsLatLon)
                return sExtent;

            Extent aExtent = new Extent();
            ProjectionInfo fromProj = _projInfo;
            ProjectionInfo toProj = KnownCoordinateSystems.Geographic.World.WGS1984;
            double[][] points = new double[4][];
            points[0] = new double[] { sExtent.minX, sExtent.minY };
            points[1] = new double[] { sExtent.minX, sExtent.maxY };
            points[2] = new double[] { sExtent.maxX, sExtent.maxY };
            points[3] = new double[] { sExtent.maxX, sExtent.minY };
            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);

            //Get lon lat extent
            aExtent.minX = Math.Min(points[0][0], points[1][0]);
            aExtent.minY = Math.Min(points[0][1], points[3][1]);
            aExtent.maxX = Math.Max(points[2][0], points[3][0]);
            aExtent.maxY = Math.Max(points[1][1], points[2][1]);

            return aExtent;
        }

        /// <summary>
        /// Project angle
        /// </summary>
        /// <param name="oAngle"></param>
        /// <param name="fromP1"></param>
        /// <param name="toP1"></param>
        /// <param name="fromProj"></param>
        /// <param name="toProj"></param>
        /// <returns></returns>
        public double ProjectAngle_Proj4(double oAngle, double[] fromP1, double[] toP1, ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            double pAngle = oAngle;
            double[] fromP2;
            double[] toP2;
            double[][] points = new double[1][];

            if (fromP1[1] == 90)
            {
                fromP2 = new double[] { fromP1[0], fromP1[1] - 10 };
                points[0] = (double[])fromP2.Clone();
                try
                {
                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                    toP2 = points[0];
                    double x, y;
                    x = toP2[0] - toP1[0];
                    y = toP2[1] - toP1[1];
                    double aLen = Math.Sqrt(x * x + y * y);
                    double angle = Math.Asin(x / aLen) * 180 / Math.PI;
                    if (x < 0 && y < 0)
                    {
                        angle = 180.0 - angle;
                    }
                    else if (x > 0 && y < 0)
                    {
                        angle = 180.0 - angle;
                    }
                    else if (x < 0 && y > 0)
                    {
                        angle = 360.0 + angle;
                    }
                    if (aLen == 0)
                    {
                        Console.WriteLine("Error");
                    }
                    pAngle = oAngle + (angle - 180);
                    if (pAngle > 360)
                    {
                        pAngle = pAngle - 360;
                    }
                    else if (pAngle < 0)
                    {
                        pAngle = pAngle + 360;
                    }
                }
                catch
                {

                }
            }
            else
            {
                fromP2 = new double[] { fromP1[0] + 10, fromP1[1] };
                points[0] = (double[])fromP2.Clone();
                try
                {
                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                    toP2 = points[0];

                    double x, y;
                    x = toP2[0] - toP1[0];
                    y = toP2[1] - toP1[1];
                    double aLen = Math.Sqrt(x * x + y * y);
                    if (aLen == 0)
                        return pAngle;

                    double angle = Math.Asin(x / aLen) * 180 / Math.PI;
                    if (Double.IsNaN(angle))
                        return pAngle;

                    if (x < 0 && y < 0)
                    {
                        angle = 180.0 - angle;
                    }
                    else if (x > 0 && y < 0)
                    {
                        angle = 180.0 - angle;
                    }
                    else if (x < 0 && y > 0)
                    {
                        angle = 360.0 + angle;
                    }
                    
                    pAngle = oAngle + (angle - 90);
                    if (pAngle > 360)
                    {
                        pAngle = pAngle - 360;
                    }
                    else if (pAngle < 0)
                    {
                        pAngle = pAngle + 360;
                    }
                }
                catch
                {

                }
            }

            return pAngle;
        }

        /// <summary>
        /// Project layer angle
        /// </summary>
        /// <param name="oLayer"></param>
        /// <param name="fromProj"></param>
        /// <param name="toProj"></param>
        /// <returns></returns>
        public VectorLayer ProjectLayerAngle(VectorLayer oLayer, ProjectionInfo fromProj, ProjectionInfo toProj)
        {            
            //coordinate transform process            
            ArrayList newPoints = new ArrayList();                        

            VectorLayer aLayer = new VectorLayer(oLayer.ShapeType);
            aLayer = (VectorLayer)oLayer.Clone();

            //aLayer.AttributeTable.Table = oLayer.AttributeTable.Table.Clone();
            aLayer.AttributeTable.Table = new DataTable();
            foreach (DataColumn aDC in oLayer.AttributeTable.Table.Columns)
            {
                DataColumn bDC = new DataColumn();
                bDC.ColumnName = aDC.ColumnName;
                bDC.DataType = aDC.DataType;
                aLayer.EditAddField(bDC);
            }

            //aLayer.AttributeTable.Table.Rows.Clear();
            int s;
            switch (aLayer.LayerDrawType)
            {                
                case LayerDrawType.Vector:
                    List<Shape.Shape> vectors = new List<Shape.Shape>();
                    newPoints.Clear();
                    for (s = 0; s < aLayer.ShapeList.Count; s++)
                    {
                        WindArraw aArraw = (WindArraw)aLayer.ShapeList[s];
                        if (fromProj.Transform.Proj4Name == "lonlat")
                        {
                            switch (toProj.Transform.ProjectionName)
                            {
                                case ProjectionNames.Lambert_Conformal:
                                case ProjectionNames.North_Polar_Stereographic:
                                    if (aArraw.Point.X < -89)
                                    {
                                        continue;
                                    }
                                    break;
                                case ProjectionNames.South_Polar_Stereographic:
                                    if (aArraw.Point.Y > 89)
                                    {
                                        continue;
                                    }
                                    break;
                            }
                        }
                        double[] fromP = new double[] { aArraw.Point.X, aArraw.Point.Y };
                        double[] toP;
                        double[][] points = new double[1][];
                        points[0] = (double[])fromP.Clone();
                        try
                        {
                            //Reproject point back to fromProj
                            Reproject.ReprojectPoints(points, toProj, fromProj, 0, points.Length);
                            toP = points[0];                            
                            aArraw.angle = ProjectAngle_Proj4(aArraw.angle, toP, fromP, fromProj, toProj);
                            newPoints.Add(aArraw.Point);
                            //aLayer.shapeList[s] = aArraw;
                            vectors.Add(aArraw);

                            DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                            aLayer.AttributeTable.Table.ImportRow(aDR);
                        }
                        catch
                        {

                        }
                    }
                    aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(vectors);
                    aLayer.Extent = MIMath.GetPointsExtent(newPoints);

                    break;
                case LayerDrawType.Barb:
                    List<Shape.Shape> windBarbs = new List<MeteoInfoC.Shape.Shape>();
                    newPoints.Clear();
                    for (s = 0; s < aLayer.ShapeList.Count; s++)
                    {
                        WindBarb aWB = (WindBarb)aLayer.ShapeList[s];
                        if (fromProj.Transform.Proj4Name == "lonlat")
                        {
                            switch (toProj.Transform.ProjectionName)
                            {
                                case ProjectionNames.Lambert_Conformal:
                                case ProjectionNames.North_Polar_Stereographic:
                                    if (aWB.Point.Y < -89)
                                    {
                                        continue;
                                    }
                                    break;
                                case ProjectionNames.South_Polar_Stereographic:
                                    if (aWB.Point.Y > 89)
                                    {
                                        continue;
                                    }
                                    break;
                            }
                        }
                        double[] fromP = new double[] { aWB.Point.X, aWB.Point.Y };
                        double[][] points = new double[1][];
                        points[0] = (double[])fromP.Clone();
                        try
                        {
                            Reproject.ReprojectPoints(points, toProj, fromProj, 0, points.Length);
                            double[] toP = points[0];                            
                            aWB.angle = ProjectAngle_Proj4(aWB.angle, toP, fromP, fromProj, toProj);
                            newPoints.Add(aWB.Point);
                            windBarbs.Add(aWB);
                            //aLayer.shapeList[s] = aWB;

                            DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                            aLayer.AttributeTable.Table.ImportRow(aDR);
                        }
                        catch
                        {

                        }
                    }
                    aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(windBarbs);
                    aLayer.Extent = MIMath.GetPointsExtent(newPoints);

                    break;
                case LayerDrawType.StationModel:
                    List<Shape.Shape> stationModels = new List<MeteoInfoC.Shape.Shape>();
                    newPoints.Clear();
                    for (s = 0; s < aLayer.ShapeList.Count; s++)
                    {
                        StationModelShape aSM = (StationModelShape)aLayer.ShapeList[s];
                        if (fromProj.Transform.Proj4Name == "lonlat")
                        {
                            switch (toProj.Transform.ProjectionName)
                            {
                                case ProjectionNames.Lambert_Conformal:
                                case ProjectionNames.North_Polar_Stereographic:
                                    if (aSM.Point.Y < -89)
                                    {
                                        continue;
                                    }
                                    break;
                                case ProjectionNames.South_Polar_Stereographic:
                                    if (aSM.Point.Y > 89)
                                    {
                                        continue;
                                    }
                                    break;
                            }
                        }
                        double[] fromP = new double[] { aSM.Point.X, aSM.Point.Y };
                        double[][] points = new double[1][];
                        points[0] = (double[])fromP.Clone();
                        try
                        {
                            Reproject.ReprojectPoints(points, toProj, fromProj, 0, points.Length);
                            double[] toP = points[0];                            
                            aSM.windBarb.angle = ProjectAngle_Proj4(aSM.windBarb.angle, toP, fromP, fromProj, toProj);
                            newPoints.Add(aSM.Point);
                            stationModels.Add(aSM);
                            //aLayer.shapeList[s] = aWB;

                            DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                            aLayer.AttributeTable.Table.ImportRow(aDR);
                        }
                        catch
                        {

                        }
                    }
                    aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(stationModels);
                    aLayer.Extent = MIMath.GetPointsExtent(newPoints);

                    break;                
            }

            //if (aLayer.LabelSetV.DrawLabels)
            //{
            //    aLayer.AddLabels();
            //}

            return aLayer;
        }

        ///// <summary>
        ///// Project layer
        ///// </summary>
        ///// <param name="oLayer"></param>
        ///// <param name="fromProj"></param>
        ///// <param name="toProj"></param>
        ///// <returns></returns>
        //public VectorLayer ProjectLayer_Proj4_Old(VectorLayer oLayer, ProjectionInfo fromProj, ProjectionInfo toProj)
        //{

        //    double refLon = _refCutLon;
        //    if (oLayer.Extent.maxX > 180 && oLayer.Extent.minX > refLon)
        //    {
        //        refLon += 360;
        //    }

        //    //coordinate transform process
        //    int i, s;
        //    PointD wPoint = new PointD();
        //    PointD aPoint = new PointD();            
        //    ArrayList newPoints = new ArrayList();            
        //    Extent lExtent = new Extent();            

        //    VectorLayer aLayer = new VectorLayer(oLayer.ShapeType);
        //    aLayer = oLayer.Clone();

        //    //aLayer.AttributeTable.Table = oLayer.AttributeTable.Table.Clone();
        //    aLayer.AttributeTable.Table = new DataTable();
        //    foreach (DataColumn aDC in oLayer.AttributeTable.Table.Columns)
        //    {
        //        DataColumn bDC = new DataColumn();
        //        bDC.ColumnName = aDC.ColumnName;
        //        bDC.DataType = aDC.DataType;
        //        aLayer.EditAddField(bDC);
        //    }

        //    //aLayer.AttributeTable.Table.Rows.Clear();

        //    switch (aLayer.LayerDrawType)
        //    {
        //        case LayerDrawType.WeatherSymbol:
        //            ArrayList weatherSymbols = new ArrayList();
        //            newPoints.Clear();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                WeatherSymbol aWS = (WeatherSymbol)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aWS.Point.Y < -89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aWS.Point.Y > 89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }
        //                double[] fromP = new double[] { aWS.Point.X, aWS.Point.Y };
        //                double[] toP;
        //                double[][] points = new double[1][];
        //                points[0] = (double[])fromP.Clone();
        //                try
        //                {
        //                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                    toP = points[0];
        //                    aPoint.X = (Single)toP[0];
        //                    aPoint.Y = (Single)toP[1];
        //                    aWS.Point = aPoint;
        //                    newPoints.Add(aPoint);
        //                    //aLayer.shapeList[s] = aWS;
        //                    weatherSymbols.Add(aWS);

        //                    DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                    aLayer.AttributeTable.Table.ImportRow(aDR);
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            aLayer.ShapeList = (ArrayList)weatherSymbols.Clone();
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);                    

        //            break;
        //        case LayerDrawType.Vector:
        //            ArrayList vectors = new ArrayList();
        //            newPoints.Clear();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                WindArraw aArraw = (WindArraw)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aArraw.Point.X < -89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aArraw.Point.Y > 89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }
        //                double[] fromP = new double[] { aArraw.Point.X, aArraw.Point.Y };
        //                double[] toP;
        //                double[][] points = new double[1][];
        //                points[0] = (double[])fromP.Clone();
        //                try
        //                {
        //                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                    toP = points[0];
        //                    wPoint.X = toP[0];
        //                    wPoint.Y = toP[1];
        //                    aArraw.Point = wPoint;
        //                    aArraw.angle = ProjectAngle_Proj4(aArraw.angle, fromP, toP, fromProj, toProj);
        //                    newPoints.Add(wPoint);
        //                    //aLayer.shapeList[s] = aArraw;
        //                    vectors.Add(aArraw);

        //                    DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                    aLayer.AttributeTable.Table.ImportRow(aDR);
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            aLayer.ShapeList = (ArrayList)vectors.Clone();
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //            break;
        //        case LayerDrawType.Barb:
        //            ArrayList windBarbs = new ArrayList();
        //            newPoints.Clear();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                WindBarb aWB = (WindBarb)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aWB.Point.Y < -89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aWB.Point.Y > 89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }                        
        //                double[] fromP = new double[] { aWB.Point.X, aWB.Point.Y };
        //                double[][] points = new double[1][];
        //                points[0] = (double[])fromP.Clone();
        //                try
        //                {
        //                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                    double[] toP = points[0];
        //                    aPoint.X = (Single)toP[0];
        //                    aPoint.Y = (Single)toP[1];
        //                    aWB.Point = aPoint;
        //                    aWB.angle = ProjectAngle_Proj4(aWB.angle, fromP, toP, fromProj, toProj);
        //                    newPoints.Add(aPoint);
        //                    windBarbs.Add(aWB);
        //                    //aLayer.shapeList[s] = aWB;

        //                    DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                    aLayer.AttributeTable.Table.ImportRow(aDR);
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            aLayer.ShapeList = (ArrayList)windBarbs.Clone();
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //            break;
        //        case LayerDrawType.StationModel:
        //            ArrayList stationModels = new ArrayList();
        //            newPoints.Clear();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {                        
        //                StationModel aSM = (StationModel)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aSM.Point.Y < -89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aSM.Point.Y > 89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }
        //                double[] fromP = new double[] { aSM.Point.X, aSM.Point.Y };
        //                double[][] points = new double[1][];
        //                points[0] = (double[])fromP.Clone();
        //                try
        //                {
        //                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                    double[] toP = points[0];
        //                    aPoint.X = (Single)toP[0];
        //                    aPoint.Y = (Single)toP[1];
        //                    aSM.Point = aPoint;
        //                    aSM.windBarb.angle = ProjectAngle_Proj4(aSM.windBarb.angle, fromP, toP, fromProj, toProj);
        //                    newPoints.Add(aPoint);
        //                    stationModels.Add(aSM);
        //                    //aLayer.shapeList[s] = aWB;

        //                    DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                    aLayer.AttributeTable.Table.ImportRow(aDR);
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            aLayer.ShapeList = (ArrayList)stationModels.Clone();
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //            break;
        //        default:
        //            switch (aLayer.ShapeType)
        //            {
        //                case ShapeTypes.Point:
        //                    ArrayList shapePoints = new ArrayList();
        //                    newPoints.Clear();
        //                    for (s = 0; s < aLayer.ShapeList.Count; s++)
        //                    {
        //                        PointShape aPS = (PointShape)aLayer.ShapeList[s];
        //                        if (fromProj.Transform.Proj4Name == "lonlat")
        //                        {
        //                            switch (toProj.Transform.ProjectionName)
        //                            {
        //                                case ProjectionNames.Lambert_Conformal:
        //                                case ProjectionNames.North_Polar_Stereographic:
        //                                    if (aPS.Point.Y < -89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                                case ProjectionNames.South_Polar_Stereographic:
        //                                    if (aPS.Point.Y > 89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                            }
        //                        }
        //                        aPS = ProjectPointShape(aPS, fromProj, toProj);
        //                        if (aPS != null)
        //                        {
        //                            shapePoints.Add(aPS);
        //                            newPoints.Add(aPS.Point);

        //                            DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                            aLayer.AttributeTable.Table.ImportRow(aDR);
        //                        }                                
        //                    }
        //                    aLayer.ShapeList = (ArrayList)shapePoints.Clone();
        //                    aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //                    break;
        //                case ShapeTypes.Polyline:
        //                case ShapeTypes.PolylineZ:
        //                    ArrayList newPolylines = new ArrayList();
        //                    for (s = 0; s < aLayer.ShapeList.Count; s++)
        //                    {
        //                        PolylineShape aPLS = (PolylineShape)aLayer.ShapeList[s];
        //                        List<PolylineShape> plsList = new List<PolylineShape>();
        //                        if (fromProj.Transform.Proj4Name == "lonlat")
        //                        {                                    
        //                            plsList = CutPolyLineShapeLon(refLon, aPLS);
        //                        }
        //                        else
        //                        {
        //                            plsList.Add(aPLS);
        //                        }
        //                        for (i = 0; i < plsList.Count; i++)
        //                        {
        //                            aPLS = plsList[i];
        //                            aPLS = ProjectPolylineShape(aPLS, fromProj, toProj);                                    
        //                            if (aPLS != null)
        //                            {                                        
        //                                newPolylines.Add(aPLS);

        //                                DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                                aLayer.AttributeTable.Table.ImportRow(aDR);

        //                                if (s == 0 && i == 0)
        //                                {
        //                                    lExtent = aPLS.Extent;
        //                                }
        //                                else
        //                                {
        //                                    lExtent = MIMath.GetLagerExtent(lExtent, aPLS.Extent);
        //                                }
        //                            }                                   
        //                        }
        //                    }
        //                    aLayer.ShapeList = (ArrayList)newPolylines.Clone();
        //                    newPolylines.Clear();
        //                    aLayer.ShapeNum = aLayer.ShapeList.Count;
        //                    aLayer.Extent = lExtent;

        //                    break;
        //                case ShapeTypes.Polygon:
        //                    ArrayList newPolygons = new ArrayList();
        //                    for (s = 0; s < aLayer.ShapeList.Count; s++)
        //                    {
        //                        DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                        PolygonShape aPGS = (PolygonShape)aLayer.ShapeList[s];
        //                        List<PolygonShape> pgsList = new List<PolygonShape>();
        //                        if (fromProj.Transform.Proj4Name == "lonlat")
        //                        {
        //                            switch (toProj.Transform.ProjectionName)
        //                            {
        //                                case ProjectionNames.Lambert_Conformal:
        //                                case ProjectionNames.North_Polar_Stereographic:
        //                                    if (aPGS.Extent.minY < -89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                                case ProjectionNames.South_Polar_Stereographic:
        //                                    if (aPGS.Extent.maxY > 89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                            }
        //                            pgsList = CutPolygonShapeLon(refLon, aPGS);
        //                        }
        //                        else
        //                        {
        //                            pgsList.Add(aPGS);
        //                        }
        //                        for (i = 0; i < pgsList.Count; i++)
        //                        {
        //                            aPGS = pgsList[i];                                                                        
        //                            aPGS = ProjectPolygonShape(aPGS, fromProj, toProj);
        //                            if (aPGS != null)
        //                            {                                    
        //                                newPolygons.Add(aPGS);
                                        
        //                                //aLayer.AttributeTable.Table.ImportRow(aDR);
        //                                aLayer.AttributeTable.Table.Rows.Add(aDR.ItemArray);

        //                                if (s == 0)
        //                                {
        //                                    lExtent = aPGS.Extent;
        //                                }
        //                                else
        //                                {
        //                                    lExtent = MIMath.GetLagerExtent(lExtent, aPGS.Extent);
        //                                }                                                                                   
        //                            }
        //                        }
        //                    }
        //                    aLayer.ShapeList = (ArrayList)newPolygons.Clone();
        //                    newPolygons.Clear();
        //                    aLayer.ShapeNum = aLayer.ShapeList.Count;
        //                    aLayer.Extent = lExtent;
        //                    break;
        //            }
        //            break;
        //    }

        //    if (aLayer.LabelSetV.DrawLabels)
        //    {
        //        aLayer.AddLabels();
        //    }

        //    return aLayer;
        //}

        ///// <summary>
        ///// Project vector layer
        ///// </summary>
        ///// <param name="oLayer">the layer</param>
        ///// <param name="fromProj">from projection</param>
        ///// <param name="toProj">to projection</param>
        ///// <returns>projected vector layer</returns>
        //public VectorLayer ProjectLayer(VectorLayer oLayer, ProjectionInfo fromProj, ProjectionInfo toProj)
        //{
        //    return ProjectLayer(oLayer, fromProj, toProj, true);
        //}

        /// <summary>
        /// Project vector layer
        /// </summary>
        /// <param name="oLayer">the layer</param>
        /// <param name="toProj">to projection</param>
        /// <returns>projected vector layer</returns>
        public void ProjectLayer(VectorLayer oLayer, ProjectionInfo toProj)
        {
            ProjectLayer(oLayer, toProj, true);
        }

        ///// <summary>
        ///// Project vector layer
        ///// </summary>
        ///// <param name="oLayer">the layer</param>
        ///// <param name="fromProj">from projection</param>
        ///// <param name="toProj">to projection</param>
        ///// <param name="projectLabels">if project labels</param>
        ///// <returns>projected vector layer</returns>
        //public VectorLayer ProjectLayer(VectorLayer oLayer, ProjectionInfo fromProj, ProjectionInfo toProj, bool projectLabels)
        //{

        //    double refLon = _refCutLon;
        //    if (oLayer.Extent.maxX > 180 && oLayer.Extent.minX > refLon)
        //    {
        //        refLon += 360;
        //    }

        //    //coordinate transform process
        //    int i, s;
        //    //PointD wPoint = new PointD();
        //    //PointD aPoint = new PointD();
        //    ArrayList newPoints = new ArrayList();
        //    Extent lExtent = new Extent();

        //    VectorLayer aLayer = new VectorLayer(oLayer.ShapeType);
        //    aLayer = (VectorLayer)oLayer.Clone();

        //    //aLayer.AttributeTable.Table = oLayer.AttributeTable.Table.Clone();
        //    aLayer.AttributeTable.Table = new DataTable();
        //    foreach (DataColumn aDC in oLayer.AttributeTable.Table.Columns)
        //    {
        //        DataColumn bDC = new DataColumn();
        //        bDC.ColumnName = aDC.ColumnName;
        //        bDC.DataType = aDC.DataType;
        //        aLayer.EditAddField(bDC);
        //    }

        //    //aLayer.AttributeTable.Table.Rows.Clear();

        //    switch (aLayer.ShapeType)
        //    {
        //        case ShapeTypes.Point:
        //        case ShapeTypes.WeatherSymbol:
        //        case ShapeTypes.WindArraw:
        //        case ShapeTypes.WindBarb:
        //        case ShapeTypes.StationModel:
        //            List<Shape.Shape> shapePoints = new List<MeteoInfoC.Shape.Shape>();
        //            newPoints.Clear();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                PointShape aPS = (PointShape)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                            if (aPS.Point.Y < -80)
        //                                continue;
        //                            break;
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aPS.Point.Y < 0)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aPS.Point.Y > 0)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }
        //                aPS = ProjectPointShape(aPS, fromProj, toProj);
        //                if (aPS != null)
        //                {
        //                    shapePoints.Add(aPS);
        //                    newPoints.Add(aPS.Point);

        //                    DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                    aLayer.AttributeTable.Table.ImportRow(aDR);
        //                }
        //            }
        //            aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(shapePoints);                    
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //            break;
        //        case ShapeTypes.Polyline:                
        //            List<Shape.Shape> newPolylines = new List<MeteoInfoC.Shape.Shape>();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                PolylineShape aPLS = (PolylineShape)aLayer.ShapeList[s];
        //                List<PolylineShape> plsList = new List<PolylineShape>();
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                            if (aPLS.Extent.minY < -80)
        //                                aPLS = GeoComputation.ClipPolylineShape_Lat(aPLS, -80, true);
        //                            break;
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aPLS.Extent.minY < 0)
        //                            {
        //                                //continue;
        //                                aPLS = GeoComputation.ClipPolylineShape_Lat(aPLS, 0, true);
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aPLS.Extent.maxY > 0)
        //                            {
        //                                //continue;
        //                                aPLS = GeoComputation.ClipPolylineShape_Lat(aPLS, 0, false);
        //                            }
        //                            break;
        //                    }
        //                    if (aPLS == null)
        //                        continue;

        //                    aPLS = GeoComputation.ClipPolylineShape_Lon(aPLS, refLon);
        //                    //plsList = CutPolyLineShapeLon(refLon, aPLS);
        //                    if (aPLS == null)
        //                        continue;

        //                    plsList.Add(GeoComputation.ClipPolylineShape_Lon(aPLS, refLon));
        //                }
        //                else
        //                {
        //                    plsList.Add(aPLS);
        //                }
        //                for (i = 0; i < plsList.Count; i++)
        //                {
        //                    aPLS = plsList[i];
        //                    aPLS = ProjectPolylineShape(aPLS, fromProj, toProj);
        //                    if (aPLS != null)
        //                    {
        //                        newPolylines.Add(aPLS);

        //                        DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                        aLayer.AttributeTable.Table.ImportRow(aDR);

        //                        if (s == 0 && i == 0)
        //                        {
        //                            lExtent = aPLS.Extent;
        //                        }
        //                        else
        //                        {
        //                            lExtent = MIMath.GetLagerExtent(lExtent, aPLS.Extent);
        //                        }
        //                    }
        //                }
        //            }
        //            aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(newPolylines);                    
        //            newPolylines.Clear();                    
        //            aLayer.Extent = lExtent;

        //            break;
        //        case ShapeTypes.PolylineZ:
        //            newPolylines = new List<MeteoInfoC.Shape.Shape>();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                PolylineZShape aPLS = (PolylineZShape)aLayer.ShapeList[s];
        //                List<PolylineZShape> plsList = new List<PolylineZShape>();
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                            if (aPLS.Extent.minY < -80)
        //                                aPLS = GeoComputation.ClipPolylineShape_Lat(aPLS, -80, true);
        //                            break;
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aPLS.Extent.minY < 0)
        //                            {
        //                                //continue;
        //                                aPLS = GeoComputation.ClipPolylineShape_Lat(aPLS, 0, true);
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aPLS.Extent.maxY > 0)
        //                            {
        //                                //continue;
        //                                aPLS = GeoComputation.ClipPolylineShape_Lat(aPLS, 0, false);
        //                            }
        //                            break;
        //                    }
        //                    if (aPLS == null)
        //                        continue;

        //                    aPLS = GeoComputation.ClipPolylineShape_Lon(aPLS, refLon);
        //                    //plsList = CutPolyLineShapeLon(refLon, aPLS);
        //                    if (aPLS == null)
        //                        continue;

        //                    plsList.Add(GeoComputation.ClipPolylineShape_Lon(aPLS, refLon));
        //                }
        //                else
        //                {
        //                    plsList.Add(aPLS);
        //                }
        //                for (i = 0; i < plsList.Count; i++)
        //                {
        //                    aPLS = plsList[i];
        //                    aPLS = ProjectPolylineShape(aPLS, fromProj, toProj);
        //                    if (aPLS != null)
        //                    {
        //                        newPolylines.Add(aPLS);

        //                        DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                        aLayer.AttributeTable.Table.ImportRow(aDR);

        //                        if (s == 0 && i == 0)
        //                        {
        //                            lExtent = aPLS.Extent;
        //                        }
        //                        else
        //                        {
        //                            lExtent = MIMath.GetLagerExtent(lExtent, aPLS.Extent);
        //                        }
        //                    }
        //                }
        //            }
        //            aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(newPolylines);
        //            newPolylines.Clear();
        //            aLayer.Extent = lExtent;
        //            break;
        //        case ShapeTypes.Polygon:
        //            List<Shape.Shape> newPolygons = new List<MeteoInfoC.Shape.Shape>();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                PolygonShape aPGS = (PolygonShape)aLayer.ShapeList[s];
        //                List<PolygonShape> pgsList = new List<PolygonShape>();
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                            if (aPGS.Extent.minY < -80)
        //                                aPGS = GeoComputation.ClipPolygonShape_Lat(aPGS, -80, true);
        //                            break;
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aPGS.Extent.minY < 0)
        //                            {
        //                                //continue;
        //                                aPGS = GeoComputation.ClipPolygonShape_Lat(aPGS, 0, true);
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aPGS.Extent.maxY > 0)
        //                            {
        //                                //continue;
        //                                aPGS = GeoComputation.ClipPolygonShape_Lat(aPGS, 0, false);
        //                            }
        //                            break;
        //                    }
        //                    if (aPGS == null)
        //                        continue;

                            
        //                    //aPGS = GeoComputation.ClipPolygonShape_Lon(aPGS, refLon);
        //                    //if (aPGS == null)
        //                    //    continue;

        //                    pgsList.Add(GeoComputation.ClipPolygonShape_Lon(aPGS, refLon));
        //                }
        //                else
        //                {
        //                    pgsList.Add(aPGS);
        //                }
        //                for (i = 0; i < pgsList.Count; i++)
        //                {
        //                    aPGS = pgsList[i];
        //                    aPGS = ProjectPolygonShape(aPGS, fromProj, toProj);
        //                    if (aPGS != null)
        //                    {
        //                        newPolygons.Add(aPGS);

        //                        //aLayer.AttributeTable.Table.ImportRow(aDR);
        //                        aLayer.AttributeTable.Table.Rows.Add(aDR.ItemArray);

        //                        if (s == 0)
        //                        {
        //                            lExtent = aPGS.Extent;
        //                        }
        //                        else
        //                        {
        //                            lExtent = MIMath.GetLagerExtent(lExtent, aPGS.Extent);
        //                        }
        //                    }
        //                }
        //            }
        //            aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(newPolygons);
        //            newPolygons.Clear();                    
        //            aLayer.Extent = lExtent;
        //            break;
        //    }

        //    if (oLayer.LabelPoints.Count > 0)
        //    {
        //        if (projectLabels)
        //            aLayer.LabelPoints = ProjectGraphics(oLayer.LabelPoints, fromProj, toProj);
        //        else
        //            aLayer.LabelPoints = new List<Graphic>(oLayer.LabelPoints);
        //    }

        //    aLayer.ProjInfo = toProj;

        //    return aLayer;
        //}

        /// <summary>
        /// Project vector layer
        /// </summary>
        /// <param name="oLayer">the layer</param>
        /// <param name="toProj">to projection</param>
        /// <param name="projectLabels">if project labels</param>
        public void ProjectLayer(VectorLayer oLayer, ProjectionInfo toProj, bool projectLabels)
        {
            ProjectionInfo fromProj = oLayer.ProjInfo;
            if (fromProj.ToProj4String() == toProj.ToProj4String())
            {
                if (oLayer.IsProjected)
                {
                    oLayer.GetOriginData();
                }

                return;
            }

            if (oLayer.IsProjected)
                oLayer.GetOriginData();
            else
                oLayer.UpdateOriginData();

            double refLon = _refCutLon;
            if (oLayer.Extent.maxX > 180 && oLayer.Extent.minX > refLon)
            {
                refLon += 360;
            }

            //coordinate transform process
            int i, s;
            //PointD wPoint = new PointD();
            //PointD aPoint = new PointD();
            ArrayList newPoints = new ArrayList();
            Extent lExtent = new Extent();

            DataTable aTable = new DataTable();
            foreach (DataColumn aDC in oLayer.AttributeTable.Table.Columns)
            {
                DataColumn bDC = new DataColumn();
                bDC.ColumnName = aDC.ColumnName;
                bDC.DataType = aDC.DataType;
                aTable.Columns.Add(bDC);
            }

            //aLayer.AttributeTable.Table.Rows.Clear();

            switch (oLayer.ShapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes.PointM:
                case ShapeTypes.PointZ:
                case ShapeTypes.WeatherSymbol:
                case ShapeTypes.WindArraw:
                case ShapeTypes.WindBarb:
                case ShapeTypes.StationModel:
                    List<Shape.Shape> shapePoints = new List<MeteoInfoC.Shape.Shape>();
                    newPoints.Clear();
                    for (s = 0; s < oLayer.ShapeList.Count; s++)
                    {
                        PointShape aPS = (PointShape)oLayer.ShapeList[s];
                        if (fromProj.Transform.Proj4Name == "lonlat")
                        {
                            switch (toProj.Transform.ProjectionName)
                            {
                                case ProjectionNames.Lambert_Conformal:
                                    if (aPS.Point.Y < -80)
                                        continue;
                                    break;
                                case ProjectionNames.North_Polar_Stereographic:
                                    if (aPS.Point.Y < 0)
                                    {
                                        continue;
                                    }
                                    break;
                                case ProjectionNames.South_Polar_Stereographic:
                                    if (aPS.Point.Y > 0)
                                    {
                                        continue;
                                    }
                                    break;
                            }
                        }
                        aPS = ProjectPointShape(aPS, fromProj, toProj);
                        if (aPS != null)
                        {
                            shapePoints.Add(aPS);
                            newPoints.Add(aPS.Point);

                            DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                            aTable.ImportRow(aDR);
                        }
                    }
                    oLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(shapePoints);
                    oLayer.Extent = MIMath.GetPointsExtent(newPoints);

                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineM:
                case ShapeTypes.PolylineZ:
                    List<Shape.Shape> newPolylines = new List<MeteoInfoC.Shape.Shape>();
                    for (s = 0; s < oLayer.ShapeList.Count; s++)
                    {
                        PolylineShape aPLS = (PolylineShape)oLayer.ShapeList[s];
                        List<PolylineShape> plsList = new List<PolylineShape>();
                        if (fromProj.Transform.Proj4Name == "lonlat")
                        {
                            switch (toProj.Transform.ProjectionName)
                            {
                                case ProjectionNames.Lambert_Conformal:
                                    if (aPLS.Extent.minY < -80)
                                        aPLS = GeoComputation.ClipPolylineShape_Lat(aPLS, -80, true);
                                    break;
                                case ProjectionNames.North_Polar_Stereographic:
                                    if (aPLS.Extent.minY < 0)
                                    {
                                        //continue;
                                        aPLS = GeoComputation.ClipPolylineShape_Lat(aPLS, 0, true);
                                    }
                                    break;
                                case ProjectionNames.South_Polar_Stereographic:
                                    if (aPLS.Extent.maxY > 0)
                                    {
                                        //continue;
                                        aPLS = GeoComputation.ClipPolylineShape_Lat(aPLS, 0, false);
                                    }
                                    break;
                            }
                            if (aPLS == null)
                                continue;

                            aPLS = GeoComputation.ClipPolylineShape_Lon(aPLS, refLon);
                            //plsList = CutPolyLineShapeLon(refLon, aPLS);
                            if (aPLS == null)
                                continue;

                            plsList.Add(GeoComputation.ClipPolylineShape_Lon(aPLS, refLon));
                        }
                        else
                        {
                            plsList.Add(aPLS);
                        }
                        for (i = 0; i < plsList.Count; i++)
                        {
                            aPLS = plsList[i];
                            aPLS = ProjectPolylineShape(aPLS, fromProj, toProj);
                            if (aPLS != null)
                            {
                                newPolylines.Add(aPLS);

                                DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                                aTable.ImportRow(aDR);

                                if (s == 0 && i == 0)
                                {
                                    lExtent = aPLS.Extent;
                                }
                                else
                                {
                                    lExtent = MIMath.GetLagerExtent(lExtent, aPLS.Extent);
                                }
                            }
                        }
                    }
                    oLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(newPolylines);
                    newPolylines.Clear();
                    oLayer.Extent = lExtent;

                    break;                                    
                case ShapeTypes.Polygon:
                case ShapeTypes.PolygonM:
                    List<Shape.Shape> newPolygons = new List<MeteoInfoC.Shape.Shape>();
                    for (s = 0; s < oLayer.ShapeList.Count; s++)
                    {
                        DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                        PolygonShape aPGS = (PolygonShape)oLayer.ShapeList[s];
                        List<PolygonShape> pgsList = new List<PolygonShape>();
                        if (fromProj.Transform.Proj4Name == "lonlat")
                        {
                            switch (toProj.Transform.ProjectionName)
                            {
                                case ProjectionNames.Lambert_Conformal:
                                    if (aPGS.Extent.minY < -80)
                                        aPGS = GeoComputation.ClipPolygonShape_Lat(aPGS, -80, true);
                                    break;
                                case ProjectionNames.North_Polar_Stereographic:
                                    if (aPGS.Extent.minY < 0)
                                    {
                                        //continue;
                                        aPGS = GeoComputation.ClipPolygonShape_Lat(aPGS, 0, true);
                                    }
                                    break;
                                case ProjectionNames.South_Polar_Stereographic:
                                    if (aPGS.Extent.maxY > 0)
                                    {
                                        //continue;
                                        aPGS = GeoComputation.ClipPolygonShape_Lat(aPGS, 0, false);
                                    }
                                    break;
                            }
                            if (aPGS == null)
                                continue;


                            //aPGS = GeoComputation.ClipPolygonShape_Lon(aPGS, refLon);
                            //if (aPGS == null)
                            //    continue;

                            pgsList.Add(GeoComputation.ClipPolygonShape_Lon(aPGS, refLon));
                        }
                        else
                        {
                            pgsList.Add(aPGS);
                        }
                        for (i = 0; i < pgsList.Count; i++)
                        {
                            aPGS = pgsList[i];
                            aPGS = ProjectPolygonShape(aPGS, fromProj, toProj);
                            if (aPGS != null)
                            {
                                newPolygons.Add(aPGS);

                                //aLayer.AttributeTable.Table.ImportRow(aDR);
                                aTable.Rows.Add(aDR.ItemArray);

                                if (s == 0)
                                {
                                    lExtent = aPGS.Extent;
                                }
                                else
                                {
                                    lExtent = MIMath.GetLagerExtent(lExtent, aPGS.Extent);
                                }
                            }
                        }
                    }
                    oLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(newPolygons);
                    newPolygons.Clear();
                    oLayer.Extent = lExtent;
                    break;
            }
            oLayer.AttributeTable.Table = aTable;

            if (oLayer.LabelPoints.Count > 0)
            {
                if (projectLabels)
                    oLayer.LabelPoints = ProjectGraphics(oLayer.LabelPoints, fromProj, toProj);
                else
                    oLayer.LabelPoints = new List<Graphic>(oLayer.LabelPoints);
            }
        }

        /// <summary>
        /// Project raster layer
        /// </summary>
        /// <param name="oLayer">the layer</param>
        /// <param name="toProj">to projection</param>
        public void ProjectLayer(RasterLayer oLayer, ProjectionInfo toProj)
        {
            if (toProj.Transform.ProjectionName == ProjectionNames.Robinson)
                return;

            if (oLayer.ProjInfo.ToProj4String() == toProj.ToProj4String())
            {
                if (oLayer.IsProjected)
                {
                    oLayer.GetOriginData();
                    oLayer.UpdateGridData();
                    if (oLayer.LegendScheme.BreakNum < 50)
                        oLayer.UpdateImage();
                    else
                        oLayer.SetImageByGridData();
                }
                return;
            }

            if (!oLayer.IsProjected)
                oLayer.UpdateOriginData();
            else
                oLayer.GetOriginData();

            oLayer.GridData = oLayer.GridData.Project(oLayer.ProjInfo, toProj);
            //oLayer.UpdateImage();
            if (oLayer.LegendScheme.BreakNum < 50)
                oLayer.UpdateImage();
            else
                oLayer.SetImageByGridData();
        }

        private PointShape ProjectPointShape(PointShape aPS, ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            PointShape newPS = (PointShape)aPS.Clone();
            double[][] points = new double[1][];
            points[0] = new double[] { newPS.Point.X, newPS.Point.Y };
            double[] fromP = new double[] { newPS.Point.X, newPS.Point.Y };
            try
            {
                Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                if (!double.IsNaN(points[0][0]) && !double.IsNaN(points[0][1]))
                {
                    double[] toP = points[0];                                        
                    newPS.Point = new PointD(points[0][0], points[0][1]);                    
                    switch (aPS.ShapeType)
                    {
                        case ShapeTypes.WindBarb:
                            ((WindBarb)newPS).angle = ProjectAngle_Proj4(((WindBarb)newPS).angle, fromP, toP, fromProj, toProj);
                            break;
                        case ShapeTypes.WindArraw:
                            ((WindArraw)newPS).angle = ProjectAngle_Proj4(((WindArraw)newPS).angle, fromP, toP, fromProj, toProj);
                            break;
                        case ShapeTypes.StationModel:
                            ((StationModelShape)newPS).windBarb.angle = ProjectAngle_Proj4(((StationModelShape)newPS).windBarb.angle, fromP, toP, fromProj, toProj);
                            break;
                    }
                    return newPS;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        private PolylineShape ProjectPolylineShape(PolylineShape aPLS, ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            List<PolyLine> polyLines = new List<PolyLine>();
            for (int i = 0; i < aPLS.PolyLines.Count; i++)
            {
                List<PointD> newPoints = new List<PointD>();
                PolyLine aPL = aPLS.PolyLines[i];
                PolyLine bPL = null;
                for (int j = 0; j < aPL.PointList.Count; j++)
                {
                    double[][] points = new double[1][];
                    PointD wPoint = aPL.PointList[j];
                    points[0] = new double[] { wPoint.X, wPoint.Y };
                    try
                    {
                        Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                        if (!double.IsNaN(points[0][0]) && !double.IsNaN(points[0][1]))
                        {
                            wPoint = new PointD();
                            wPoint.X = points[0][0];
                            wPoint.Y = points[0][1];
                            newPoints.Add(wPoint);
                        }
                    }
                    catch
                    {
                        break;
                    }
                }

                if (newPoints.Count > 1)
                {
                    bPL = new PolyLine();
                    bPL.PointList = newPoints;
                    polyLines.Add(bPL);
                }
            }

            
            if (polyLines.Count > 0)
            {
                aPLS.PolyLines = polyLines;

                return aPLS;
            }
            else
                return null;
        }

        //private PolylineZShape ProjectPolylineShape(PolylineZShape aPLS, ProjectionInfo fromProj, ProjectionInfo toProj)
        //{
        //    List<PolylineZ> polyLines = new List<PolylineZ>();
        //    for (int i = 0; i < aPLS.Polylines.Count; i++)
        //    {
        //        List<PointZ> newPoints = new List<PointZ>();
        //        PolylineZ aPL = aPLS.Polylines[i];
        //        PolylineZ bPL = null;
        //        for (int j = 0; j < aPL.PointList.Count; j++)
        //        {
        //            double[][] points = new double[1][];
        //            PointZ wPoint = aPL.PointList[j];
        //            points[0] = new double[] { wPoint.X, wPoint.Y };
        //            try
        //            {
        //                Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                if (!double.IsNaN(points[0][0]) && !double.IsNaN(points[0][1]))
        //                {
        //                    wPoint = new PointZ();
        //                    wPoint.X = points[0][0];
        //                    wPoint.Y = points[0][1];
        //                    newPoints.Add(wPoint);
        //                }
        //            }
        //            catch
        //            {
        //                break;
        //            }
        //        }

        //        if (newPoints.Count > 1)
        //        {
        //            bPL = new PolylineZ();
        //            bPL.PointList = newPoints;
        //            polyLines.Add(bPL);
        //        }
        //    }


        //    if (polyLines.Count > 0)
        //    {
        //        aPLS.Polylines = polyLines;

        //        return aPLS;
        //    }
        //    else
        //        return null;
        //}

        private CurveLineShape ProjectCurvelineShape(CurveLineShape aPLS, ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            List<PolyLine> polyLines = new List<PolyLine>();
            for (int i = 0; i < aPLS.PolyLines.Count; i++)
            {
                List<PointD> newPoints = new List<PointD>();
                PolyLine aPL = aPLS.PolyLines[i];
                PolyLine bPL = null;
                for (int j = 0; j < aPL.PointList.Count; j++)
                {
                    double[][] points = new double[1][];
                    PointD wPoint = aPL.PointList[j];
                    points[0] = new double[] { wPoint.X, wPoint.Y };
                    try
                    {
                        Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                        if (!double.IsNaN(points[0][0]) && !double.IsNaN(points[0][1]))
                        {
                            wPoint = new PointD();
                            wPoint.X = points[0][0];
                            wPoint.Y = points[0][1];
                            newPoints.Add(wPoint);
                        }
                    }
                    catch
                    {
                        break;
                    }
                }

                if (newPoints.Count > 1)
                {
                    bPL = new PolyLine();
                    bPL.PointList = newPoints;
                    polyLines.Add(bPL);
                }
            }


            if (polyLines.Count > 0)
            {
                aPLS.PolyLines = polyLines;

                return aPLS;
            }
            else
                return null;
        }

        //private PolylineShape ProjectPolylineShape_Back(PolylineShape aPLS, ProjectionInfo fromProj, ProjectionInfo toProj)
        //{
        //    List<PointD> newPoints = new List<PointD>();

        //    for (int j = 0; j < aPLS.Points.Count; j++)
        //    {
        //        double[][] points = new double[1][];
        //        PointD wPoint = (PointD)aPLS.Points[j];
        //        points[0] = new double[] { wPoint.X, wPoint.Y };
        //        try
        //        {
        //            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //            if (!double.IsNaN(points[0][0]) && !double.IsNaN(points[0][1]))
        //            {
        //                wPoint.X = points[0][0];
        //                wPoint.Y = points[0][1];
        //                newPoints.Add(wPoint);
        //            }
        //        }
        //        catch
        //        {
        //            break;
        //        }
        //    }
        //    if (newPoints.Count > 1)
        //    {
        //        if (aPLS.numPoints != newPoints.Count)
        //        {
        //            aPLS.numPoints = newPoints.Count;
        //            int partNum = 1;
        //            for (int i = 0; i < aPLS.PartNum; i++)
        //            {
        //                if (aPLS.parts[i] >= aPLS.numPoints)
        //                {
        //                    partNum = i;
        //                    break;
        //                }
        //            }
        //            aPLS.PartNum = partNum;
        //            Array.Resize(ref aPLS.parts, partNum);
        //        }
        //        aPLS.Extent = MIMath.GetPointsExtent(newPoints);
        //        aPLS.Points = newPoints;

        //        return aPLS;
        //    }
        //    else
        //        return null;
        //}

        /// <summary>
        /// Porject polygon shape
        /// </summary>
        /// <param name="aPGS">a polygon shape</param>
        /// <param name="fromProj">from projection</param>
        /// <param name="toProj">to projection</param>
        /// <returns>projected polygon shape</returns>
        public PolygonShape ProjectPolygonShape(PolygonShape aPGS, ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            List<Polygon> polygons = new List<Polygon>();
            for (int i = 0; i < aPGS.Polygons.Count; i++)
            {
                Polygon aPG = aPGS.Polygons[i];
                Polygon bPG = null;
                for (int r = 0; r < aPG.RingNumber; r++)
                {
                    List<PointD> pList = aPG.Rings[r];
                    List<PointD> newPoints = new List<PointD>();
                    for (int j = 0; j < pList.Count; j++)
                    {
                        double[][] points = new double[1][];
                        PointD wPoint = pList[j];
                        points[0] = new double[] { wPoint.X, wPoint.Y };
                        try
                        {
                            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                            if (!double.IsNaN(points[0][0]) && !double.IsNaN(points[0][1]))
                            {
                                wPoint = new PointD();
                                wPoint.X = points[0][0];
                                wPoint.Y = points[0][1];
                                newPoints.Add(wPoint);
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }

                    if (r == 0)
                    {
                        if (newPoints.Count > 2)
                        {
                            bPG = new Polygon();
                            bPG.OutLine = newPoints;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if (newPoints.Count > 2)
                            bPG.AddHole(newPoints);
                    }
                }

                if (bPG != null)
                    polygons.Add(bPG);
            }

            if (polygons.Count > 0)
            {
                aPGS.Polygons = polygons;

                return aPGS;
            }
            else
                return null;
        }

        private CurvePolygonShape ProjectCurvePolygonShape(CurvePolygonShape aPGS, ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            List<Polygon> polygons = new List<Polygon>();
            for (int i = 0; i < aPGS.Polygons.Count; i++)
            {
                Polygon aPG = aPGS.Polygons[i];
                Polygon bPG = null;
                for (int r = 0; r < aPG.RingNumber; r++)
                {
                    List<PointD> pList = aPG.Rings[r];
                    List<PointD> newPoints = new List<PointD>();
                    for (int j = 0; j < pList.Count; j++)
                    {
                        double[][] points = new double[1][];
                        PointD wPoint = pList[j];
                        points[0] = new double[] { wPoint.X, wPoint.Y };
                        try
                        {
                            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                            if (!double.IsNaN(points[0][0]) && !double.IsNaN(points[0][1]))
                            {
                                wPoint = new PointD();
                                wPoint.X = points[0][0];
                                wPoint.Y = points[0][1];
                                newPoints.Add(wPoint);
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }

                    if (r == 0)
                    {
                        if (newPoints.Count > 2)
                        {
                            bPG = new Polygon();
                            bPG.OutLine = newPoints;
                        }
                        else
                            break;
                    }
                    else
                    {
                        if (newPoints.Count > 2)
                            bPG.AddHole(newPoints);
                    }
                }

                if (bPG != null)
                    polygons.Add(bPG);
            }

            if (polygons.Count > 0)
            {
                aPGS.Polygons = polygons;

                return aPGS;
            }
            else
                return null;
        }

        private CircleShape ProjectCircleShape(CircleShape aCS, ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            double radius = Math.Abs(aCS.Points[1].X - aCS.Points[0].X);
            double[][] points = new double[1][];
            PointD centerPoint = new PointD(aCS.Points[0].X + radius, aCS.Points[0].Y);
            points[0] = new double[] { centerPoint.X, centerPoint.Y };
            try
            {
                Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                if (!double.IsNaN(points[0][0]) && !double.IsNaN(points[0][1]))
                {
                    centerPoint.X = points[0][0];
                    centerPoint.Y = points[0][1];
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }

            points = new double[1][];
            PointD leftPoint = aCS.Points[0];
            points[0] = new double[] { leftPoint.X, leftPoint.Y };
            try
            {
                Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                if (!double.IsNaN(points[0][0]) && !double.IsNaN(points[0][1]))
                {
                    leftPoint.X = points[0][0];
                    leftPoint.Y = points[0][1];
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }

            radius = Math.Abs(centerPoint.X - leftPoint.X);
            List<PointD> newPoints = new List<PointD>();
            newPoints.Add(new PointD(centerPoint.X - radius, centerPoint.Y));
            newPoints.Add(new PointD(centerPoint.X, centerPoint.Y - radius));
            newPoints.Add(new PointD(centerPoint.X + radius, centerPoint.Y));
            newPoints.Add(new PointD(centerPoint.X, centerPoint.Y + radius));
            CircleShape newCS = new CircleShape();
            newCS.Points = newPoints;

            return newCS;
        }

        private EllipseShape ProjectEllipseShape(EllipseShape aES, ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            double xRadius = Math.Abs(aES.Points[2].X - aES.Points[0].X) / 2;
            double yRadius = Math.Abs(aES.Points[2].Y - aES.Points[0].Y) / 2;
            double[][] points = new double[1][];
            PointD centerPoint = new PointD(aES.Extent.minX + xRadius, aES.Extent.minY + yRadius);
            points[0] = new double[] { centerPoint.X, centerPoint.Y };
            try
            {
                Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                if (!double.IsNaN(points[0][0]) && !double.IsNaN(points[0][1]))
                {
                    centerPoint.X = points[0][0];
                    centerPoint.Y = points[0][1];
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }

            points = new double[1][];
            PointD lbPoint = new PointD(aES.Extent.minX, aES.Extent.minY);
            points[0] = new double[] { lbPoint.X, lbPoint.Y };
            try
            {
                Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                if (!double.IsNaN(points[0][0]) && !double.IsNaN(points[0][1]))
                {
                    lbPoint.X = points[0][0];
                    lbPoint.Y = points[0][1];
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }

            xRadius = Math.Abs(centerPoint.X - lbPoint.X);
            yRadius = Math.Abs(centerPoint.Y - lbPoint.Y);
            List<PointD> newPoints = new List<PointD>();
            newPoints.Add(new PointD(centerPoint.X - xRadius, centerPoint.Y - yRadius));
            newPoints.Add(new PointD(centerPoint.X - xRadius, centerPoint.Y + yRadius));
            newPoints.Add(new PointD(centerPoint.X + xRadius, centerPoint.Y + yRadius));
            newPoints.Add(new PointD(centerPoint.X + xRadius, centerPoint.Y - yRadius));
            EllipseShape newES = new EllipseShape();
            newES.Points = newPoints;

            return newES;
        }

        //private PolygonShape ProjectPolygonShape_Back(PolygonShape aPGS, ProjectionInfo fromProj, ProjectionInfo toProj)
        //{
        //    List<PointD> newPoints = new List<PointD>();

        //    for (int j = 0; j < aPGS.Points.Count; j++)
        //    {
        //        double[][] points = new double[1][];
        //        PointD wPoint = (PointD)aPGS.Points[j];
        //        points[0] = new double[] { wPoint.X, wPoint.Y };
        //        try
        //        {
        //            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //            if (!double.IsNaN(points[0][0]) && !double.IsNaN(points[0][1]))
        //            {
        //                wPoint.X = points[0][0];
        //                wPoint.Y = points[0][1];
        //                newPoints.Add(wPoint);
        //            }
        //        }
        //        catch
        //        {
        //            break;
        //        }
        //    }
        //    if (newPoints.Count > 2)
        //    {
        //        if (aPGS.numPoints != newPoints.Count)
        //        {
        //            aPGS.numPoints = newPoints.Count;
        //            int partNum = 1;
        //            for (int i = 1; i < aPGS.PartNum; i++)
        //            {
        //                if (aPGS.parts[i] >= aPGS.numPoints)
        //                {
        //                    partNum = i;
        //                    break;
        //                }
        //            }
        //            aPGS.PartNum = partNum;
        //            Array.Resize(ref aPGS.parts, partNum);
        //        }
        //        aPGS.Extent = MIMath.GetPointsExtent(newPoints);
        //        aPGS.Points = newPoints;

        //        return aPGS;
        //    }
        //    else
        //        return null;           
        //}

        private Shape.Shape ProjectShape(Shape.Shape aShape, ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            Shape.Shape newShape = new MeteoInfoC.Shape.Shape();
            switch (aShape.ShapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes.PointM:
                    newShape = ProjectPointShape((PointShape)aShape, fromProj, toProj);
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineM:
                    newShape = ProjectPolylineShape((PolylineShape)aShape, fromProj, toProj);
                    break;
                case ShapeTypes.CurveLine:
                    newShape = ProjectCurvelineShape((CurveLineShape)aShape, fromProj, toProj);
                    break;
                case ShapeTypes.Polygon:
                case ShapeTypes.PolygonM:
                case ShapeTypes.Rectangle:
                    newShape = ProjectPolygonShape((PolygonShape)aShape, fromProj, toProj);
                    break;
                case ShapeTypes.CurvePolygon:
                    newShape = ProjectCurvePolygonShape((CurvePolygonShape)aShape, fromProj, toProj);
                    break;
                case ShapeTypes.Circle:
                    newShape = ProjectCircleShape((CircleShape)aShape, fromProj, toProj);
                    break;
                case ShapeTypes.Ellipse:
                    newShape = ProjectEllipseShape((EllipseShape)aShape, fromProj, toProj);
                    break;
                default:
                    newShape = null ;
                    break;
            }

            return newShape;
        }

        private GraphicCollection ProjectGraphics(GraphicCollection aGCollection, ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            GraphicCollection newGCollection = new GraphicCollection();
            foreach (Graphic aGraphic in aGCollection.GraphicList)
            {
                aGraphic.Shape = ProjectShape(aGraphic.Shape, fromProj, toProj);
                if (aGraphic.Shape != null)
                    newGCollection.Add(aGraphic);
            }

            return newGCollection;
        }

        private List<Graphic> ProjectGraphics(List<Graphic> graphics, ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            List<Graphic> newGraphics = new List<Graphic>();
            foreach (Graphic aGraphic in graphics)
            {
                Shape.Shape aShape = ProjectShape(aGraphic.Shape, fromProj, toProj);
                if (aShape != null)
                {
                    newGraphics.Add(new Graphic(aShape, aGraphic.Legend));
                }
            }

            return newGraphics;
        }

        ///// <summary>
        ///// Project layer
        ///// </summary>
        ///// <param name="oLayer">Old layer</param>
        ///// <param name="fromProj">from projection info</param>
        ///// <param name="toProj">to projection info</param>
        ///// <param name="IfReprojectAngle">if reproject wind angle</param>
        ///// <returns>vector layer after reprojection</returns>
        //public VectorLayer ProjectWindLayer(VectorLayer oLayer, ProjectionInfo fromProj, ProjectionInfo toProj,
        //    bool IfReprojectAngle)
        //{                 
        //    //Set reference longitude
        //    double refLon = _refCutLon;
        //    if (oLayer.Extent.maxX > 180 && oLayer.Extent.minX > refLon)
        //    {
        //        refLon += 360;
        //    }

        //    //coordinate transform process
        //    int i, j, s;
        //    PointD wPoint = new PointD();
        //    PointD aPoint = new PointD();
        //    List<PointD> newPoints = new List<PointD>();
        //    Extent lExtent = new Extent();            

        //    VectorLayer aLayer = new VectorLayer(oLayer.ShapeType);
        //    aLayer = (VectorLayer)oLayer.Clone();

        //    //aLayer.AttributeTable.Table = oLayer.AttributeTable.Table.Clone();
        //    aLayer.AttributeTable.Table = new DataTable();
        //    foreach (DataColumn aDC in oLayer.AttributeTable.Table.Columns)
        //    {
        //        DataColumn bDC = new DataColumn();
        //        bDC.ColumnName = aDC.ColumnName;
        //        bDC.DataType = aDC.DataType;
        //        aLayer.EditAddField(bDC);
        //    }

        //    //aLayer.AttributeTable.Table.Rows.Clear();

        //    switch (aLayer.LayerDrawType)
        //    {
        //        case LayerDrawType.WeatherSymbol:
        //            List<Shape.Shape> weatherSymbols = new List<MeteoInfoC.Shape.Shape>();
        //            newPoints.Clear();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                WeatherSymbol aWS = (WeatherSymbol)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aWS.Point.Y < -89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aWS.Point.Y > 89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }
        //                double[] fromP = new double[] { aWS.Point.X, aWS.Point.Y };
        //                double[] toP;
        //                double[][] points = new double[1][];
        //                points[0] = (double[])fromP.Clone();
        //                try
        //                {
        //                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                    toP = points[0];
        //                    aPoint.X = (Single)toP[0];
        //                    aPoint.Y = (Single)toP[1];
        //                    aWS.Point = aPoint;
        //                    newPoints.Add(aPoint);
        //                    //aLayer.shapeList[s] = aWS;
        //                    weatherSymbols.Add(aWS);

        //                    DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                    aLayer.AttributeTable.Table.ImportRow(aDR);
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(weatherSymbols);
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //            break;
        //        case LayerDrawType.Vector:
        //            List<Shape.Shape> vectors = new List<MeteoInfoC.Shape.Shape>();
        //            newPoints.Clear();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                WindArraw aArraw = (WindArraw)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aArraw.Point.X < -89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aArraw.Point.Y > 89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }
        //                double[] fromP = new double[] { aArraw.Point.X, aArraw.Point.Y };
        //                double[] toP;
        //                double[][] points = new double[1][];
        //                points[0] = (double[])fromP.Clone();
        //                try
        //                {
        //                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                    toP = points[0];
        //                    wPoint.X = toP[0];
        //                    wPoint.Y = toP[1];
        //                    aArraw.Point = wPoint;
        //                    if (IfReprojectAngle)
        //                        aArraw.angle = ProjectAngle_Proj4(aArraw.angle, fromP, toP, fromProj, toProj);
        //                    newPoints.Add(wPoint);
        //                    //aLayer.shapeList[s] = aArraw;
        //                    vectors.Add(aArraw);

        //                    DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                    aLayer.AttributeTable.Table.ImportRow(aDR);
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(vectors);
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //            break;
        //        case LayerDrawType.Barb:
        //            List<Shape.Shape> windBarbs = new List<MeteoInfoC.Shape.Shape>();
        //            newPoints.Clear();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                WindBarb aWB = (WindBarb)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aWB.Point.Y < -89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aWB.Point.Y > 89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }
        //                double[] fromP = new double[] { aWB.Point.X, aWB.Point.Y };
        //                double[][] points = new double[1][];
        //                points[0] = (double[])fromP.Clone();
        //                try
        //                {
        //                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                    double[] toP = points[0];
        //                    aPoint.X = (Single)toP[0];
        //                    aPoint.Y = (Single)toP[1];
        //                    aWB.Point = aPoint;
        //                    if (IfReprojectAngle)
        //                        aWB.angle = ProjectAngle_Proj4(aWB.angle, fromP, toP, fromProj, toProj);
        //                    newPoints.Add(aPoint);
        //                    windBarbs.Add(aWB);
        //                    //aLayer.shapeList[s] = aWB;

        //                    DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                    aLayer.AttributeTable.Table.ImportRow(aDR);
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(windBarbs);
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //            break;
        //        case LayerDrawType.StationModel:
        //            List<Shape.Shape> stationModels = new List<MeteoInfoC.Shape.Shape>();
        //            newPoints.Clear();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                StationModel aSM = (StationModel)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aSM.Point.Y < -89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aSM.Point.Y > 89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }
        //                double[] fromP = new double[] { aSM.Point.X, aSM.Point.Y };
        //                double[][] points = new double[1][];
        //                points[0] = (double[])fromP.Clone();
        //                try
        //                {
        //                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                    double[] toP = points[0];
        //                    aPoint.X = (Single)toP[0];
        //                    aPoint.Y = (Single)toP[1];
        //                    aSM.Point = aPoint;
        //                    if (IfReprojectAngle)
        //                        aSM.windBarb.angle = ProjectAngle_Proj4(aSM.windBarb.angle, fromP, toP, fromProj, toProj);
        //                    newPoints.Add(aPoint);
        //                    stationModels.Add(aSM);
        //                    //aLayer.shapeList[s] = aWB;

        //                    DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                    aLayer.AttributeTable.Table.ImportRow(aDR);
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(stationModels);
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //            break;
        //        default:
        //            switch (aLayer.ShapeType)
        //            {
        //                case ShapeTypes.Point:
        //                    List<Shape.Shape> shapePoints = new List<MeteoInfoC.Shape.Shape>();
        //                    newPoints = new List<PointD>();
        //                    for (s = 0; s < aLayer.ShapeList.Count; s++)
        //                    {
        //                        PointShape aPS = (PointShape)aLayer.ShapeList[s];
        //                        if (fromProj.Transform.Proj4Name == "lonlat")
        //                        {
        //                            switch (toProj.Transform.ProjectionName)
        //                            {
        //                                case ProjectionNames.Lambert_Conformal:
        //                                case ProjectionNames.North_Polar_Stereographic:
        //                                    if (aPS.Point.Y < -89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                                case ProjectionNames.South_Polar_Stereographic:
        //                                    if (aPS.Point.Y > 89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                            }
        //                        }
        //                        double[][] points = new double[1][];
        //                        points[0] = new double[] { aPS.Point.X, aPS.Point.Y };
        //                        try
        //                        {
        //                            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                            wPoint.X = points[0][0];
        //                            wPoint.Y = points[0][1];
        //                            aPS.Point = wPoint;
        //                            newPoints.Add(wPoint);
        //                            shapePoints.Add(aPS);
        //                            //aLayer.shapeList[s] = aPS;

        //                            DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                            aLayer.AttributeTable.Table.ImportRow(aDR);
        //                        }
        //                        catch
        //                        {

        //                        }
        //                    }
        //                    aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(shapePoints);
        //                    aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //                    break;
        //                case ShapeTypes.Polyline:
        //                case ShapeTypes.PolylineZ:
        //                    List<Shape.Shape> newPolylines = new List<MeteoInfoC.Shape.Shape>();
        //                    for (s = 0; s < aLayer.ShapeList.Count; s++)
        //                    {
        //                        PolylineShape aPLS = (PolylineShape)aLayer.ShapeList[s];
        //                        List<PolylineShape> plsList = new List<PolylineShape>();
        //                        if (fromProj.Transform.Proj4Name == "lonlat")
        //                        {                                    
        //                            plsList = CutPolyLineShapeLon(refLon, aPLS);
        //                        }
        //                        else
        //                        {
        //                            plsList.Add(aPLS);
        //                        }
        //                        for (i = 0; i < plsList.Count; i++)
        //                        {
        //                            aPLS = plsList[i];
        //                            newPoints = new List<PointD>();
        //                            for (j = 0; j < aPLS.Points.Count; j++)
        //                            {
        //                                double[][] points = new double[1][];
        //                                wPoint = (PointD)aPLS.Points[j];
        //                                points[0] = new double[] { wPoint.X, wPoint.Y };
        //                                try
        //                                {
        //                                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                                    wPoint.X = points[0][0];
        //                                    wPoint.Y = points[0][1];
        //                                    newPoints.Add(wPoint);
        //                                }
        //                                catch
        //                                {
        //                                    break;
        //                                }
        //                            }
        //                            if (newPoints.Count > 1)
        //                            {
        //                                aPLS.Extent = MIMath.GetPointsExtent(newPoints);
        //                                aPLS.Points = newPoints;
        //                                newPolylines.Add(aPLS);

        //                                DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                                aLayer.AttributeTable.Table.ImportRow(aDR);

        //                                if (s == 0 && i == 0)
        //                                {
        //                                    lExtent = aPLS.Extent;
        //                                }
        //                                else
        //                                {
        //                                    lExtent = MIMath.GetLagerExtent(lExtent, aPLS.Extent);
        //                                }

        //                            }
        //                        }
        //                    }
        //                    aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(newPolylines);
        //                    newPolylines.Clear();                            
        //                    aLayer.Extent = lExtent;

        //                    break;
        //                case ShapeTypes.Polygon:
        //                    List<Shape.Shape> newPolygons = new List<MeteoInfoC.Shape.Shape>();
        //                    for (s = 0; s < aLayer.ShapeList.Count; s++)
        //                    {
        //                        DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
        //                        PolygonShape aPGS = (PolygonShape)aLayer.ShapeList[s];
        //                        List<PolygonShape> pgsList = new List<PolygonShape>();
        //                        if (fromProj.Transform.Proj4Name == "lonlat")
        //                        {
        //                            switch (toProj.Transform.ProjectionName)
        //                            {
        //                                case ProjectionNames.Lambert_Conformal:
        //                                case ProjectionNames.North_Polar_Stereographic:
        //                                    if (aPGS.Extent.minY < -89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                                case ProjectionNames.South_Polar_Stereographic:
        //                                    if (aPGS.Extent.maxY > 89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                            }
        //                            switch (toProj.Transform.ProjectionName)
        //                            {
        //                                //case ProjectionName.Orthographic:
        //                                //    //double newRefLon;
        //                                //    //List<PolygonShape> newPSList = new List<PolygonShape>();
        //                                //    //newPSList.Add(aPGS);
        //                                //    //for (int l = 0; l < 35; l++)
        //                                //    //{
        //                                //    //    newRefLon = -170 + l * 10;
        //                                //    //    if (oLayer.Extent.maxX > 180)
        //                                //    //    {
        //                                //    //        newRefLon += 180;
        //                                //    //    }
        //                                //    //    pgsList.Clear();
        //                                //    //    foreach (PolygonShape newPS in newPSList)
        //                                //    //    {
        //                                //    //        pgsList.AddRange(CutPolygonLon(newRefLon, newPS));
        //                                //    //    }
        //                                //    //    newPSList.Clear();
        //                                //    //    newPSList.AddRange(pgsList);                                                
        //                                //    //}
        //                                //    break;
        //                                default:                                            
        //                                    //pgsList = CutPolygonShapeLon(refLon, aPGS);
        //                                    pgsList.Add(GeoComputation.ClipPolygonShape_Lon(aPGS, refLon));
        //                                    break;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            pgsList.Add(aPGS);
        //                        }
        //                        for (i = 0; i < pgsList.Count; i++)
        //                        {
        //                            aPGS = pgsList[i];
        //                            newPoints = new List<PointD>();
        //                            for (j = 0; j < aPGS.Points.Count; j++)
        //                            {
        //                                double[][] points = new double[1][];
        //                                wPoint = (PointD)aPGS.Points[j];
        //                                points[0] = new double[] { wPoint.X, wPoint.Y };
        //                                try
        //                                {
        //                                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                                    wPoint.X = points[0][0];
        //                                    wPoint.Y = points[0][1];
        //                                    newPoints.Add(wPoint);
        //                                }
        //                                catch
        //                                {
        //                                    break;
        //                                }
        //                            }
        //                            if (newPoints.Count > 2)
        //                            {
        //                                aPGS.Extent = MIMath.GetPointsExtent(newPoints);
        //                                aPGS.Points = newPoints;
        //                                newPolygons.Add(aPGS);

        //                                //aLayer.AttributeTable.Table.ImportRow(aDR);
        //                                aLayer.AttributeTable.Table.Rows.Add(aDR.ItemArray);

        //                                if (s == 0)
        //                                {
        //                                    lExtent = aPGS.Extent;
        //                                }
        //                                else
        //                                {
        //                                    lExtent = MIMath.GetLagerExtent(lExtent, aPGS.Extent);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    aLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(newPolygons);
        //                    newPolygons.Clear();                           
        //                    aLayer.Extent = lExtent;
        //                    break;
        //            }
        //            break;
        //    }

        //    if (aLayer.LabelSet.DrawLabels)
        //    {
        //        aLayer.AddLabels();
        //    }

        //    return aLayer;
        //}


        /// <summary>
        /// Project layer
        /// </summary>
        /// <param name="oLayer">Old layer</param>
        /// <param name="toProj">to projection info</param>
        /// <param name="IfReprojectAngle">if reproject wind angle</param>
        public void ProjectWindLayer(VectorLayer oLayer, ProjectionInfo toProj, bool IfReprojectAngle)
        {
            ProjectionInfo fromProj = oLayer.ProjInfo;
            if (fromProj.ToProj4String() == toProj.ToProj4String())
            {
                if (oLayer.IsProjected)
                {
                    oLayer.GetOriginData();
                }

                return;
            }

            if (oLayer.IsProjected)
                oLayer.GetOriginData();
            else
                oLayer.UpdateOriginData();

            //Set reference longitude
            double refLon = _refCutLon;
            if (oLayer.Extent.maxX > 180 && oLayer.Extent.minX > refLon)
            {
                refLon += 360;
            }

            //coordinate transform process
            int i, j, s;
            PointD wPoint = new PointD();
            PointD aPoint = new PointD();
            List<PointD> newPoints = new List<PointD>();
            Extent lExtent = new Extent();

            DataTable aTable = new DataTable();
            foreach (DataColumn aDC in oLayer.AttributeTable.Table.Columns)
            {
                DataColumn bDC = new DataColumn();
                bDC.ColumnName = aDC.ColumnName;
                bDC.DataType = aDC.DataType;
                aTable.Columns.Add(bDC);
            }

            //aLayer.AttributeTable.Table.Rows.Clear();

            switch (oLayer.LayerDrawType)
            {
                case LayerDrawType.WeatherSymbol:
                    List<Shape.Shape> weatherSymbols = new List<MeteoInfoC.Shape.Shape>();
                    newPoints.Clear();
                    for (s = 0; s < oLayer.ShapeList.Count; s++)
                    {
                        WeatherSymbol aWS = (WeatherSymbol)oLayer.ShapeList[s];
                        if (fromProj.Transform.Proj4Name == "lonlat")
                        {
                            switch (toProj.Transform.ProjectionName)
                            {
                                case ProjectionNames.Lambert_Conformal:
                                case ProjectionNames.North_Polar_Stereographic:
                                    if (aWS.Point.Y < -89)
                                    {
                                        continue;
                                    }
                                    break;
                                case ProjectionNames.South_Polar_Stereographic:
                                    if (aWS.Point.Y > 89)
                                    {
                                        continue;
                                    }
                                    break;
                            }
                        }
                        double[] fromP = new double[] { aWS.Point.X, aWS.Point.Y };
                        double[] toP;
                        double[][] points = new double[1][];
                        points[0] = (double[])fromP.Clone();
                        try
                        {
                            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                            toP = points[0];
                            aPoint = new PointD();
                            aPoint.X = (Single)toP[0];
                            aPoint.Y = (Single)toP[1];
                            aWS.Point = aPoint;
                            newPoints.Add(aPoint);
                            //aLayer.shapeList[s] = aWS;
                            weatherSymbols.Add(aWS);

                            DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                            aTable.ImportRow(aDR);
                        }
                        catch
                        {

                        }
                    }
                    oLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(weatherSymbols);
                    oLayer.Extent = MIMath.GetPointsExtent(newPoints);

                    break;
                case LayerDrawType.Vector:
                    List<Shape.Shape> vectors = new List<MeteoInfoC.Shape.Shape>();
                    newPoints.Clear();
                    for (s = 0; s < oLayer.ShapeList.Count; s++)
                    {
                        WindArraw aArraw = (WindArraw)oLayer.ShapeList[s];
                        if (fromProj.Transform.Proj4Name == "lonlat")
                        {
                            switch (toProj.Transform.ProjectionName)
                            {
                                case ProjectionNames.Lambert_Conformal:
                                case ProjectionNames.North_Polar_Stereographic:
                                    if (aArraw.Point.X < -89)
                                    {
                                        continue;
                                    }
                                    break;
                                case ProjectionNames.South_Polar_Stereographic:
                                    if (aArraw.Point.Y > 89)
                                    {
                                        continue;
                                    }
                                    break;
                            }
                        }
                        double[] fromP = new double[] { aArraw.Point.X, aArraw.Point.Y };
                        double[] toP;
                        double[][] points = new double[1][];
                        points[0] = (double[])fromP.Clone();
                        try
                        {
                            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                            toP = points[0];
                            wPoint = new PointD();
                            wPoint.X = toP[0];
                            wPoint.Y = toP[1];
                            aArraw.Point = wPoint;
                            if (IfReprojectAngle)
                                aArraw.angle = ProjectAngle_Proj4(aArraw.angle, fromP, toP, fromProj, toProj);
                            newPoints.Add(wPoint);
                            //aLayer.shapeList[s] = aArraw;
                            vectors.Add(aArraw);

                            DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                            aTable.ImportRow(aDR);
                        }
                        catch
                        {

                        }
                    }
                    oLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(vectors);
                    oLayer.Extent = MIMath.GetPointsExtent(newPoints);

                    break;
                case LayerDrawType.Barb:
                    List<Shape.Shape> windBarbs = new List<MeteoInfoC.Shape.Shape>();
                    newPoints.Clear();
                    for (s = 0; s < oLayer.ShapeList.Count; s++)
                    {
                        WindBarb aWB = (WindBarb)oLayer.ShapeList[s];
                        if (fromProj.Transform.Proj4Name == "lonlat")
                        {
                            switch (toProj.Transform.ProjectionName)
                            {
                                case ProjectionNames.Lambert_Conformal:
                                case ProjectionNames.North_Polar_Stereographic:
                                    if (aWB.Point.Y < -89)
                                    {
                                        continue;
                                    }
                                    break;
                                case ProjectionNames.South_Polar_Stereographic:
                                    if (aWB.Point.Y > 89)
                                    {
                                        continue;
                                    }
                                    break;
                            }
                        }
                        double[] fromP = new double[] { aWB.Point.X, aWB.Point.Y };
                        double[][] points = new double[1][];
                        points[0] = (double[])fromP.Clone();
                        try
                        {
                            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                            double[] toP = points[0];
                            aPoint = new PointD();
                            aPoint.X = (Single)toP[0];
                            aPoint.Y = (Single)toP[1];
                            aWB.Point = aPoint;
                            if (IfReprojectAngle)
                                aWB.angle = ProjectAngle_Proj4(aWB.angle, fromP, toP, fromProj, toProj);
                            newPoints.Add(aPoint);
                            windBarbs.Add(aWB);
                            //aLayer.shapeList[s] = aWB;

                            DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                            aTable.ImportRow(aDR);
                        }
                        catch
                        {

                        }
                    }
                    oLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(windBarbs);
                    oLayer.Extent = MIMath.GetPointsExtent(newPoints);

                    break;
                case LayerDrawType.StationModel:
                    List<Shape.Shape> stationModels = new List<MeteoInfoC.Shape.Shape>();
                    newPoints.Clear();
                    for (s = 0; s < oLayer.ShapeList.Count; s++)
                    {
                        StationModelShape aSM = (StationModelShape)oLayer.ShapeList[s];
                        if (fromProj.Transform.Proj4Name == "lonlat")
                        {
                            switch (toProj.Transform.ProjectionName)
                            {
                                case ProjectionNames.Lambert_Conformal:
                                case ProjectionNames.North_Polar_Stereographic:
                                    if (aSM.Point.Y < -89)
                                    {
                                        continue;
                                    }
                                    break;
                                case ProjectionNames.South_Polar_Stereographic:
                                    if (aSM.Point.Y > 89)
                                    {
                                        continue;
                                    }
                                    break;
                            }
                        }
                        double[] fromP = new double[] { aSM.Point.X, aSM.Point.Y };
                        double[][] points = new double[1][];
                        points[0] = (double[])fromP.Clone();
                        try
                        {
                            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                            double[] toP = points[0];
                            aPoint = new PointD();
                            aPoint.X = (Single)toP[0];
                            aPoint.Y = (Single)toP[1];
                            aSM.Point = aPoint;
                            if (IfReprojectAngle)
                                aSM.windBarb.angle = ProjectAngle_Proj4(aSM.windBarb.angle, fromP, toP, fromProj, toProj);
                            newPoints.Add(aPoint);
                            stationModels.Add(aSM);
                            //aLayer.shapeList[s] = aWB;

                            DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                            aTable.ImportRow(aDR);
                        }
                        catch
                        {

                        }
                    }
                    oLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(stationModels);
                    oLayer.Extent = MIMath.GetPointsExtent(newPoints);

                    break;
                default:
                    switch (oLayer.ShapeType)
                    {
                        case ShapeTypes.Point:
                        case ShapeTypes.PointM:
                            List<Shape.Shape> shapePoints = new List<MeteoInfoC.Shape.Shape>();
                            newPoints = new List<PointD>();
                            for (s = 0; s < oLayer.ShapeList.Count; s++)
                            {
                                PointShape aPS = (PointShape)oLayer.ShapeList[s];
                                if (fromProj.Transform.Proj4Name == "lonlat")
                                {
                                    switch (toProj.Transform.ProjectionName)
                                    {
                                        case ProjectionNames.Lambert_Conformal:
                                        case ProjectionNames.North_Polar_Stereographic:
                                            if (aPS.Point.Y < -89)
                                            {
                                                continue;
                                            }
                                            break;
                                        case ProjectionNames.South_Polar_Stereographic:
                                            if (aPS.Point.Y > 89)
                                            {
                                                continue;
                                            }
                                            break;
                                    }
                                }
                                double[][] points = new double[1][];
                                points[0] = new double[] { aPS.Point.X, aPS.Point.Y };
                                try
                                {
                                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                                    wPoint = new PointD();
                                    wPoint.X = points[0][0];
                                    wPoint.Y = points[0][1];
                                    aPS.Point = wPoint;
                                    newPoints.Add(wPoint);
                                    shapePoints.Add(aPS);
                                    //aLayer.shapeList[s] = aPS;

                                    DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                                    aTable.ImportRow(aDR);
                                }
                                catch
                                {

                                }
                            }
                            oLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(shapePoints);
                            oLayer.Extent = MIMath.GetPointsExtent(newPoints);

                            break;
                        case ShapeTypes.Polyline:
                        case ShapeTypes.PolylineM:
                        case ShapeTypes.PolylineZ:
                            List<Shape.Shape> newPolylines = new List<MeteoInfoC.Shape.Shape>();
                            for (s = 0; s < oLayer.ShapeList.Count; s++)
                            {
                                PolylineShape aPLS = (PolylineShape)oLayer.ShapeList[s];
                                List<PolylineShape> plsList = new List<PolylineShape>();
                                if (fromProj.Transform.Proj4Name == "lonlat")
                                {
                                    plsList = CutPolyLineShapeLon(refLon, aPLS);
                                }
                                else
                                {
                                    plsList.Add(aPLS);
                                }
                                for (i = 0; i < plsList.Count; i++)
                                {
                                    aPLS = plsList[i];
                                    newPoints = new List<PointD>();
                                    for (j = 0; j < aPLS.Points.Count; j++)
                                    {
                                        double[][] points = new double[1][];
                                        wPoint = aPLS.Points[j];
                                        points[0] = new double[] { wPoint.X, wPoint.Y };
                                        try
                                        {
                                            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                                            wPoint = new PointD();
                                            wPoint.X = points[0][0];
                                            wPoint.Y = points[0][1];
                                            newPoints.Add(wPoint);
                                        }
                                        catch
                                        {
                                            break;
                                        }
                                    }
                                    if (newPoints.Count > 1)
                                    {
                                        aPLS.Extent = MIMath.GetPointsExtent(newPoints);
                                        aPLS.Points = newPoints;
                                        newPolylines.Add(aPLS);

                                        DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                                        aTable.ImportRow(aDR);

                                        if (s == 0 && i == 0)
                                        {
                                            lExtent = aPLS.Extent;
                                        }
                                        else
                                        {
                                            lExtent = MIMath.GetLagerExtent(lExtent, aPLS.Extent);
                                        }

                                    }
                                }
                            }
                            oLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(newPolylines);
                            newPolylines.Clear();
                            oLayer.Extent = lExtent;

                            break;
                        case ShapeTypes.Polygon:
                        case ShapeTypes.PolygonM:
                            List<Shape.Shape> newPolygons = new List<MeteoInfoC.Shape.Shape>();
                            for (s = 0; s < oLayer.ShapeList.Count; s++)
                            {
                                DataRow aDR = oLayer.AttributeTable.Table.Rows[s];
                                PolygonShape aPGS = (PolygonShape)oLayer.ShapeList[s];
                                List<PolygonShape> pgsList = new List<PolygonShape>();
                                if (fromProj.Transform.Proj4Name == "lonlat")
                                {
                                    switch (toProj.Transform.ProjectionName)
                                    {
                                        case ProjectionNames.Lambert_Conformal:
                                        case ProjectionNames.North_Polar_Stereographic:
                                            if (aPGS.Extent.minY < -89)
                                            {
                                                continue;
                                            }
                                            break;
                                        case ProjectionNames.South_Polar_Stereographic:
                                            if (aPGS.Extent.maxY > 89)
                                            {
                                                continue;
                                            }
                                            break;
                                    }
                                    switch (toProj.Transform.ProjectionName)
                                    {
                                        //case ProjectionName.Orthographic:
                                        //    //double newRefLon;
                                        //    //List<PolygonShape> newPSList = new List<PolygonShape>();
                                        //    //newPSList.Add(aPGS);
                                        //    //for (int l = 0; l < 35; l++)
                                        //    //{
                                        //    //    newRefLon = -170 + l * 10;
                                        //    //    if (oLayer.Extent.maxX > 180)
                                        //    //    {
                                        //    //        newRefLon += 180;
                                        //    //    }
                                        //    //    pgsList.Clear();
                                        //    //    foreach (PolygonShape newPS in newPSList)
                                        //    //    {
                                        //    //        pgsList.AddRange(CutPolygonLon(newRefLon, newPS));
                                        //    //    }
                                        //    //    newPSList.Clear();
                                        //    //    newPSList.AddRange(pgsList);                                                
                                        //    //}
                                        //    break;
                                        default:
                                            //pgsList = CutPolygonShapeLon(refLon, aPGS);
                                            pgsList.Add(GeoComputation.ClipPolygonShape_Lon(aPGS, refLon));
                                            break;
                                    }
                                }
                                else
                                {
                                    pgsList.Add(aPGS);
                                }
                                for (i = 0; i < pgsList.Count; i++)
                                {
                                    aPGS = pgsList[i];
                                    newPoints = new List<PointD>();
                                    for (j = 0; j < aPGS.Points.Count; j++)
                                    {
                                        double[][] points = new double[1][];
                                        wPoint = aPGS.Points[j];
                                        points[0] = new double[] { wPoint.X, wPoint.Y };
                                        try
                                        {
                                            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                                            wPoint = new PointD();
                                            wPoint.X = points[0][0];
                                            wPoint.Y = points[0][1];
                                            newPoints.Add(wPoint);
                                        }
                                        catch
                                        {
                                            break;
                                        }
                                    }
                                    if (newPoints.Count > 2)
                                    {
                                        aPGS.Extent = MIMath.GetPointsExtent(newPoints);
                                        aPGS.Points = newPoints;
                                        newPolygons.Add(aPGS);

                                        //aLayer.AttributeTable.Table.ImportRow(aDR);
                                        aTable.Rows.Add(aDR.ItemArray);

                                        if (s == 0)
                                        {
                                            lExtent = aPGS.Extent;
                                        }
                                        else
                                        {
                                            lExtent = MIMath.GetLagerExtent(lExtent, aPGS.Extent);
                                        }
                                    }
                                }
                            }
                            oLayer.ShapeList = new List<MeteoInfoC.Shape.Shape>(newPolygons);
                            newPolygons.Clear();
                            oLayer.Extent = lExtent;
                            break;
                    }
                    break;
            }

            oLayer.AttributeTable.Table = aTable;

            if (oLayer.LabelPoints.Count > 0)
            {
                oLayer.LabelPoints = ProjectGraphics(oLayer.LabelPoints, fromProj, toProj);
            }
        }

        ///// <summary>
        ///// Project layer
        ///// </summary>
        ///// <param name="oLayer"></param>
        ///// <param name="fromProj"></param>
        ///// <param name="toProj"></param>
        ///// <returns></returns>
        //public VectorLayer ProjectLayer_Proj4_Back(VectorLayer oLayer, ProjectionInfo fromProj, ProjectionInfo toProj)
        //{

        //    double refLon = _refCutLon;
        //    if (oLayer.Extent.maxX > 180 && oLayer.Extent.minX > refLon)
        //    {
        //        refLon += 360;
        //    }

        //    //coordinate transform process
        //    int i, j, s;
        //    PointD wPoint = new PointD();
        //    PointD aPoint = new PointD();
        //    List<PointD> newPoints = new List<PointD>();
        //    Extent lExtent = new Extent();            

        //    VectorLayer aLayer = new VectorLayer(oLayer.ShapeType);
        //    aLayer.Extent = oLayer.Extent;
        //    aLayer.FileName = oLayer.FileName;
        //    aLayer.Handle = oLayer.Handle;
        //    aLayer.LayerName = oLayer.LayerName;
        //    aLayer.LegendScheme = oLayer.LegendScheme;
        //    aLayer.ShapeList = (ArrayList)oLayer.ShapeList.Clone();
        //    aLayer.ShapeNum = oLayer.ShapeNum;                        
        //    aLayer.TransparencyPerc = oLayer.TransparencyPerc;
        //    aLayer.LayerDrawType = oLayer.LayerDrawType;
        //    aLayer.Visible = oLayer.Visible;
        //    aLayer.AttributeTable.Table = oLayer.AttributeTable.Table;
        //    aLayer.LabelSetV = oLayer.LabelSetV;

        //    switch (aLayer.LayerDrawType)
        //    {
        //        case LayerDrawType.WeatherSymbol:
        //            ArrayList weatherSymbols = new ArrayList();
        //            newPoints = new List<PointD>();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                WeatherSymbol aWS = (WeatherSymbol)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aWS.Point.Y < -89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aWS.Point.Y > 89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }
        //                double[] fromP = new double[] { aWS.Point.X, aWS.Point.Y };
        //                double[] toP;
        //                double[][] points = new double[1][];
        //                points[0] = (double[])fromP.Clone();
        //                try
        //                {
        //                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                    toP = points[0];
        //                    aPoint.X = (Single)toP[0];
        //                    aPoint.Y = (Single)toP[1];
        //                    aWS.Point = aPoint;
        //                    newPoints.Add(aPoint);
        //                    //aLayer.shapeList[s] = aWS;
        //                    weatherSymbols.Add(aWS);
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            aLayer.ShapeList = (ArrayList)weatherSymbols.Clone();
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //            break;
        //        case LayerDrawType.Vector:
        //            ArrayList vectors = new ArrayList();
        //            newPoints = new List<PointD>();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                WindArraw aArraw = (WindArraw)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aArraw.Point.Y < -89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aArraw.Point.Y > 89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }
        //                double[] fromP = new double[] { aArraw.Point.X, aArraw.Point.Y };
        //                double[] toP;
        //                double[][] points = new double[1][];
        //                points[0] = (double[])fromP.Clone();
        //                try
        //                {
        //                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                    toP = points[0];
        //                    wPoint.X = toP[0];
        //                    wPoint.Y = toP[1];
        //                    aArraw.Point = wPoint;
        //                    aArraw.angle = ProjectAngle_Proj4(aArraw.angle, fromP, toP, fromProj, toProj);
        //                    newPoints.Add(aPoint);
        //                    //aLayer.shapeList[s] = aArraw;
        //                    vectors.Add(aArraw);
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            aLayer.ShapeList = (ArrayList)vectors.Clone();
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //            break;
        //        case LayerDrawType.Barb:
        //            ArrayList windBarbs = new ArrayList();
        //            newPoints = new List<PointD>();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                WindBarb aWB = (WindBarb)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aWB.Point.Y < -89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aWB.Point.Y > 89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }
        //                double[] fromP = new double[] { aWB.Point.X, aWB.Point.Y };
        //                double[][] points = new double[1][];
        //                points[0] = (double[])fromP.Clone();
        //                try
        //                {
        //                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                    double[] toP = points[0];
        //                    aPoint.X = (Single)toP[0];
        //                    aPoint.Y = (Single)toP[1];
        //                    aWB.Point = aPoint;
        //                    aWB.angle = ProjectAngle_Proj4(aWB.angle, fromP, toP, fromProj, toProj);
        //                    newPoints.Add(aPoint);
        //                    windBarbs.Add(aWB);
        //                    //aLayer.shapeList[s] = aWB;
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            aLayer.ShapeList = (ArrayList)windBarbs.Clone();
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //            break;
        //        case LayerDrawType.StationModel:
        //            ArrayList stationModels = new ArrayList();
        //            newPoints = new List<PointD>();
        //            for (s = 0; s < aLayer.ShapeList.Count; s++)
        //            {
        //                StationModel aSM = (StationModel)aLayer.ShapeList[s];
        //                if (fromProj.Transform.Proj4Name == "lonlat")
        //                {
        //                    switch (toProj.Transform.ProjectionName)
        //                    {
        //                        case ProjectionNames.Lambert_Conformal:
        //                        case ProjectionNames.North_Polar_Stereographic:
        //                            if (aSM.Point.Y < -89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                        case ProjectionNames.South_Polar_Stereographic:
        //                            if (aSM.Point.Y > 89)
        //                            {
        //                                continue;
        //                            }
        //                            break;
        //                    }
        //                }
        //                double[] fromP = new double[] { aSM.Point.X, aSM.Point.Y };
        //                double[][] points = new double[1][];
        //                points[0] = (double[])fromP.Clone();
        //                try
        //                {
        //                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                    double[] toP = points[0];
        //                    aPoint.X = (Single)toP[0];
        //                    aPoint.Y = (Single)toP[1];
        //                    aSM.Point = aPoint;
        //                    aSM.windBarb.angle = ProjectAngle_Proj4(aSM.windBarb.angle, fromP, toP, fromProj, toProj);
        //                    newPoints.Add(aPoint);
        //                    stationModels.Add(aSM);
        //                    //aLayer.shapeList[s] = aWB;
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            aLayer.ShapeList = (ArrayList)stationModels.Clone();
        //            aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //            break;
        //        default:
        //            switch (aLayer.ShapeType)
        //            {
        //                case ShapeTypes.Point:
        //                    ArrayList shapePoints = new ArrayList();
        //                    newPoints = new List<PointD>();
        //                    for (s = 0; s < aLayer.ShapeList.Count; s++)
        //                    {
        //                        PointShape aPS = (PointShape)aLayer.ShapeList[s];
        //                        if (fromProj.Transform.Proj4Name == "lonlat")
        //                        {
        //                            switch (toProj.Transform.ProjectionName)
        //                            {
        //                                case ProjectionNames.Lambert_Conformal:
        //                                case ProjectionNames.North_Polar_Stereographic:
        //                                    if (aPS.Point.Y < -89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                                case ProjectionNames.South_Polar_Stereographic:
        //                                    if (aPS.Point.Y > 89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                            }
        //                        }
        //                        double[][] points = new double[1][];
        //                        points[0] = new double[] { aPS.Point.X, aPS.Point.Y };
        //                        try
        //                        {
        //                            Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                            wPoint.X = points[0][0];
        //                            wPoint.Y = points[0][1];
        //                            aPS.Point = wPoint;
        //                            newPoints.Add(wPoint);
        //                            shapePoints.Add(aPS);
        //                            //aLayer.shapeList[s] = aPS;
        //                        }
        //                        catch
        //                        {

        //                        }
        //                    }
        //                    aLayer.ShapeList = (ArrayList)shapePoints.Clone();
        //                    aLayer.Extent = MIMath.GetPointsExtent(newPoints);

        //                    break;
        //                case ShapeTypes.Polyline:
        //                    ArrayList newPolylines = new ArrayList();
        //                    for (s = 0; s < aLayer.ShapeList.Count; s++)
        //                    {
        //                        PolylineShape aPLS = (PolylineShape)aLayer.ShapeList[s];
        //                        List<PolylineShape> plsList = new List<PolylineShape>();
        //                        if (fromProj.Transform.Proj4Name == "lonlat")
        //                        {
        //                            switch (toProj.Transform.ProjectionName)
        //                            {
        //                                case ProjectionNames.Lambert_Conformal:
        //                                case ProjectionNames.North_Polar_Stereographic:
        //                                    if (aPLS.Extent.minY < -89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                                case ProjectionNames.South_Polar_Stereographic:
        //                                    if (aPLS.Extent.maxY > 89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                            }

        //                            if ((aPLS.Extent.minX < refLon && aPLS.Extent.maxX > refLon)
        //                                || aPLS.Extent.minX == refLon || aPLS.Extent.maxX == refLon)
        //                            {
        //                                plsList = CutPolyLineLon(refLon, aPLS);
        //                            }
        //                            else
        //                            {
        //                                plsList.Add(aPLS);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            plsList.Add(aPLS);
        //                        }
        //                        for (i = 0; i < plsList.Count; i++)
        //                        {
        //                            aPLS = plsList[i];
        //                            double[][] points = new double[aPLS.Points.Count][];
        //                            newPoints = new List<PointD>();
        //                            for (j = 0; j < aPLS.Points.Count; j++)
        //                            {
        //                                wPoint = (PointD)aPLS.Points[j];
        //                                points[j] = new double[] { wPoint.X, wPoint.Y };
        //                            }
        //                            if (points.Length > 1)
        //                            {
        //                                try
        //                                {
        //                                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                                    for (j = 0; j < points.Length; j++)
        //                                    {
        //                                        wPoint.X = points[j][0];
        //                                        wPoint.Y = points[j][1];
        //                                        newPoints.Add(wPoint);
        //                                    }
        //                                    aPLS.Extent = MIMath.GetPointsExtent(newPoints);
        //                                    aPLS.Points = newPoints;
        //                                    newPolylines.Add(aPLS);

        //                                    if (s == 0 && i == 0)
        //                                    {
        //                                        lExtent = aPLS.Extent;
        //                                    }
        //                                    else
        //                                    {
        //                                        lExtent = MIMath.GetLagerExtent(lExtent, aPLS.Extent);
        //                                    }
        //                                }
        //                                catch
        //                                {

        //                                }
        //                            }
        //                        }
        //                    }
        //                    aLayer.ShapeList = (ArrayList)newPolylines.Clone();
        //                    newPolylines.Clear();
        //                    aLayer.ShapeNum = aLayer.ShapeList.Count;
        //                    aLayer.Extent = lExtent;

        //                    break;
        //                case ShapeTypes.Polygon:
        //                    ArrayList newPolygons = new ArrayList();
        //                    for (s = 0; s < aLayer.ShapeList.Count; s++)
        //                    {
        //                        PolygonShape aPGS = (PolygonShape)aLayer.ShapeList[s];
        //                        List<PolygonShape> pgsList = new List<PolygonShape>();
        //                        if (fromProj.Transform.Proj4Name == "lonlat")
        //                        {
        //                            switch (toProj.Transform.ProjectionName)
        //                            {
        //                                case ProjectionNames.Lambert_Conformal:
        //                                case ProjectionNames.North_Polar_Stereographic:
        //                                    if (aPGS.Extent.minY < -89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                                case ProjectionNames.South_Polar_Stereographic:
        //                                    if (aPGS.Extent.maxY > 89)
        //                                    {
        //                                        continue;
        //                                    }
        //                                    break;
        //                            }
        //                            if ((aPGS.Extent.minX < refLon && aPGS.Extent.maxX > refLon)
        //                                || aPGS.Extent.minX == refLon || aPGS.Extent.maxX == refLon)
        //                            //if (aPGS.Extent.minX < refLon && aPGS.Extent.maxX > refLon)
        //                            {
        //                                pgsList = CutPolygonLon(refLon, aPGS);
        //                            }
        //                            else
        //                            {
        //                                pgsList.Add(aPGS);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            pgsList.Add(aPGS);
        //                        }
        //                        for (i = 0; i < pgsList.Count; i++)
        //                        {
        //                            aPGS = pgsList[i];
        //                            double[][] points = new double[aPGS.Points.Count][];
        //                            newPoints = new List<PointD>();
        //                            for (j = 0; j < aPGS.Points.Count; j++)
        //                            {
        //                                wPoint = (PointD)aPGS.Points[j];
        //                                points[j] = new double[] { wPoint.X, wPoint.Y };
        //                            }
        //                            if (points.Length > 1)
        //                            {
        //                                try
        //                                {
        //                                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
        //                                    for (j = 0; j < points.Length; j++)
        //                                    {
        //                                        wPoint.X = points[j][0];
        //                                        wPoint.Y = points[j][1];
        //                                        newPoints.Add(wPoint);
        //                                    }
        //                                    aPGS.Extent = MIMath.GetPointsExtent(newPoints);
        //                                    aPGS.Points = newPoints;
        //                                    newPolygons.Add(aPGS);

        //                                    if (s == 0)
        //                                    {
        //                                        lExtent = aPGS.Extent;
        //                                    }
        //                                    else
        //                                    {
        //                                        lExtent = MIMath.GetLagerExtent(lExtent, aPGS.Extent);
        //                                    }
        //                                }
        //                                catch
        //                                {

        //                                }
        //                            }
        //                        }
        //                    }
        //                    aLayer.ShapeList = (ArrayList)newPolygons.Clone();
        //                    newPolygons.Clear();
        //                    aLayer.ShapeNum = aLayer.ShapeList.Count;
        //                    aLayer.Extent = lExtent;
        //                    break;
        //            }
        //            break;
        //    }

        //    if (aLayer.LabelSetV.DrawLabels)
        //    {
        //        aLayer.AddLabels();
        //    }

        //    return aLayer;
        //}

        ///// <summary>
        ///// Project layers
        ///// </summary>
        ///// <param name="aMapView"></param>
        ///// <param name="fromProj"></param>
        ///// <param name="toProj"></param>
        //public void ProjectLayers_Proj4(MapView aMapView, ProjectionInfo fromProj, ProjectionInfo toProj)
        //{
        //    if (toProj.Transform.ProjectionName == ProjectionNames.Oblique_Stereographic)
        //    {
        //        if (toProj.LatitudeOfOrigin == 90)
        //            toProj.Transform.ProjectionName = ProjectionNames.North_Polar_Stereographic;
        //        if (toProj.LatitudeOfOrigin == -90)
        //            toProj.Transform.ProjectionName = ProjectionNames.South_Polar_Stereographic;
        //    }

        //    //coordinate transform process
        //    int i;
        //    for (i = 0; i < aMapView.LayerSet.Layers.Count; i++)
        //    {
        //        if ((aMapView.LayerSet.Layers[i]).GetType() == typeof(VectorLayer))
        //        {
        //            VectorLayer oLayer = (VectorLayer)aMapView.LayerSet.Layers[i];
        //            VectorLayer aLayer = new VectorLayer(oLayer.ShapeType);
        //            aLayer = ProjectLayer_Proj4(oLayer, fromProj, toProj);
        //            aMapView.LayerSet.Layers[i] = aLayer;
        //        }
        //    }
        //    aMapView.Extent = aMapView.GetLayersWholeExtent();
        //    Extent aExten = aMapView.Extent;
        //    aMapView.ZoomToExtent(aExten);
        //    //aMapView.MapExtentSetV.SetCoordinateGeoMap(aExten.minX, aExten.maxX,
        //    //    aExten.minY, aExten.maxY, aMapView.Width, aMapView.Height,
        //    //    aMapView.Projection.IsLonLatMap);
        //}

        ///// <summary>
        ///// Project layers
        ///// </summary>
        ///// <param name="aMapView">map view</param>
        ///// <param name="toProj">To projection</param>
        //public void ProjectLayers_old(MapView aMapView, ProjectionInfo toProj)
        //{
        //    if (aMapView.Projection.ProjInfo.ToProj4String() == toProj.ToProj4String())
        //        return;

        //    aMapView.LockViewUpdate = true;

        //    ProjectionInfo originProj = aMapView.Projection.ProjInfo;
        //    ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
      
        //    if (fromProj.ToProj4String() == toProj.ToProj4String())
        //    {
        //        if (aMapView.Projection.ProjInfo.ToProj4String() == fromProj.ToProj4String())
        //        {
        //            aMapView.LockViewUpdate = false;
        //            if (aMapView.Projection.ProjInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat)
        //            {
        //                aMapView.LayerSet.Layers = new List<MeteoInfoC.Layer.MapLayer>(aMapView.LayerSet.GeoLayers);
        //            }
        //            return;
        //        }
        //        else
        //        {
        //            ProjectionSet aProjSet = aMapView.Projection;
        //            aProjSet.ProjInfo = fromProj;                    
        //            aMapView.Projection = aProjSet;
        //            //frmMain.G_LayerLegend.MapView.Layers.Layers = (ArrayList)frmMain.G_LayerLegend.MapView.Layers.GeoLayers.Clone();
        //            aMapView.LayerSet.Layers = new List<MapLayer>(aMapView.LayerSet.GeoLayers);
        //            for (int i = 0; i < aMapView.LayerSet.LayerNum; i++)
        //            {
        //                if (aMapView.LayerSet.Layers[i].LayerType == LayerTypes.RasterLayer)
        //                    ProjectLayer((RasterLayer)aMapView.LayerSet.Layers[i], toProj);
        //            }

        //            //Project graphics
        //            if (aMapView.GraphicCollection.Count > 0)
        //            {
        //                GraphicCollection newGCollection = ProjectGraphics(aMapView.GraphicCollection, originProj, toProj);
        //                aMapView.SetGraphics(newGCollection);
        //            }

        //            aMapView.LockViewUpdate = false;
        //            aMapView.Extent = aMapView.GetLayersWholeExtent();
        //            Extent aExtent = aMapView.Extent;
        //            aMapView.ZoomToExtent(aExtent);
        //            aMapView.RaiseViewExtentChangedEvent();
        //            return;
        //        }
        //    }

        //    if (aMapView.Projection.ProjInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat)
        //    {
        //        //aMapView.Layers.GeoLayers = (ArrayList)aMapView.Layers.Layers.Clone();
        //        aMapView.LayerSet.GeoLayers = new List<MeteoInfoC.Layer.MapLayer>(aMapView.LayerSet.Layers);
        //    }
        //    else
        //    {
        //        //aMapView.Layers.Layers = (ArrayList)aMapView.Layers.GeoLayers.Clone();
        //        aMapView.LayerSet.Layers = new List<MeteoInfoC.Layer.MapLayer>(aMapView.LayerSet.GeoLayers);
        //    }

        //    //if (toProj.Transform.ProjectionName == ProjectionNames.Oblique_Stereographic)
        //    //{
        //    //    if (toProj.LatitudeOfOrigin == 90)
        //    //        toProj.Transform.ProjectionName = ProjectionNames.North_Polar_Stereographic;
        //    //    if (toProj.LatitudeOfOrigin == -90)
        //    //        toProj.Transform.ProjectionName = ProjectionNames.South_Polar_Stereographic;
        //    //}
        //    aMapView.Projection.ProjInfo = toProj;            
        //    double refLon = Convert.ToDouble(toProj.CentralMeridian);
        //    refLon += 180;
        //    if (refLon > 180)
        //    {
        //        refLon = refLon - 360;
        //    }
        //    else if (refLon < -180)
        //    {
        //        refLon = refLon + 360;
        //    }
        //    aMapView.Projection.RefCutLon = refLon;

        //    for (int i = 0; i < aMapView.LayerSet.Layers.Count; i++)
        //    {
        //        switch (aMapView.LayerSet.Layers[i].LayerType)
        //        {
        //            case LayerTypes.VectorLayer:
        //                VectorLayer oLayer = (VectorLayer)aMapView.LayerSet.Layers[i];
        //                VectorLayer aLayer = ProjectLayer(oLayer, fromProj, toProj);
        //                aMapView.LayerSet.Layers[i] = aLayer;
        //                break;
        //            case LayerTypes.RasterLayer:
        //                RasterLayer oRLayer = (RasterLayer)aMapView.LayerSet.Layers[i];
        //                ProjectLayer(oRLayer, toProj);
        //                break;
        //        }                
        //    } 
           
        //    //Project graphics
        //    if (aMapView.GraphicCollection.Count > 0)
        //    {
        //        GraphicCollection newGCollection = ProjectGraphics(aMapView.GraphicCollection, originProj, toProj);
        //        aMapView.SetGraphics(newGCollection);
        //    }

        //    aMapView.Extent = aMapView.GetLayersWholeExtent();
        //    Extent aExten = aMapView.Extent;            
        //    //aMapView.LonLatLayer = aMapView.Projection.GenerateLonLatLayer();
        //    //if (aMapView.LonLatLayer == null)
        //    aMapView.LonLatLayer = aMapView.GenerateLonLatLayer();
        //    aMapView.LonLatProjLayer = ProjectLayer(aMapView.LonLatLayer, fromProj, toProj);
        //    for (int i = 0; i < aMapView.LonLatProjLayer.ShapeNum; i++)
        //    {
        //        PolylineShape aPLS = (PolylineShape)aMapView.LonLatProjLayer.ShapeList[i];
        //        if (aPLS.PolyLines.Count == 2)
        //        {
        //            PointD aP = aPLS.PolyLines[0].PointList[aPLS.PolyLines[0].PointList.Count - 1];
        //            PointD bP = aPLS.PolyLines[1].PointList[aPLS.PolyLines[1].PointList.Count - 1];
        //            bool isJoin = false;
        //            if (refLon == 0)
        //            {
        //                if (Math.Abs(aP.X) < 0.1 && Math.Abs(bP.X) < 0.1 && MIMath.DoubleEquals(aP.Y, bP.Y))
        //                    isJoin = true;
        //            }
        //            else
        //            {
        //                if (MIMath.DoubleEquals(aP.X, bP.X) && MIMath.DoubleEquals(aP.Y, bP.Y))
        //                    isJoin = true;
        //            }

        //            if (isJoin)
        //            {
        //                List<PolyLine> polyLines = new List<PolyLine>();
        //                PolyLine aPL = new PolyLine();
        //                List<PointD> pList = new List<PointD>(aPLS.PolyLines[1].PointList);
        //                List<PointD> bPList = new List<PointD>(aPLS.PolyLines[0].PointList);
        //                bPList.Reverse();
        //                pList.AddRange(bPList);
        //                aPL.PointList = pList;
        //                polyLines.Add(aPL);
        //                aPLS.PolyLines = polyLines;
        //                aMapView.LonLatProjLayer.ShapeList[i] = aPLS;
        //            }
        //        }
        //    }

        //    aMapView.LockViewUpdate = false;
        //    aMapView.ZoomToExtent(aExten);

        //    OnProjectionChanged();
        //}

        /// <summary>
        /// Project layers
        /// </summary>
        /// <param name="aMapView">map view</param>
        /// <param name="toProj">To projection</param>
        public void ProjectLayers(MapView aMapView, ProjectionInfo toProj)
        {
            ProjectLayers(aMapView, toProj, true);
        }

        /// <summary>
        /// Project layers
        /// </summary>
        /// <param name="aMapView">map view</param>
        /// <param name="toProj">To projection</param>
        /// <param name="isUpdateView">if paint mapview</param>
        public void ProjectLayers(MapView aMapView, ProjectionInfo toProj, Boolean isUpdateView)
        {
            if (aMapView.Projection.ProjInfo.ToProj4String() == toProj.ToProj4String())
                return;

            aMapView.LockViewUpdate = true;

            ProjectionInfo fromProj = aMapView.Projection.ProjInfo;            

            aMapView.Projection.ProjInfo = toProj;
            double refLon = Convert.ToDouble(toProj.CentralMeridian);
            refLon += 180;
            if (refLon > 180)
            {
                refLon = refLon - 360;
            }
            else if (refLon < -180)
            {
                refLon = refLon + 360;
            }
            aMapView.Projection.RefCutLon = refLon;

            for (int i = 0; i < aMapView.LayerSet.Layers.Count; i++)
            {
                switch (aMapView.LayerSet.Layers[i].LayerType)
                {
                    case LayerTypes.VectorLayer:
                        VectorLayer oLayer = (VectorLayer)aMapView.LayerSet.Layers[i];
                        ProjectLayer(oLayer, toProj);
                        break;
                    case LayerTypes.RasterLayer:
                        RasterLayer oRLayer = (RasterLayer)aMapView.LayerSet.Layers[i];
                        ProjectLayer(oRLayer, toProj);
                        break;
                }
            }

            //Project graphics
            if (aMapView.GraphicCollection.Count > 0)
            {
                GraphicCollection newGCollection = ProjectGraphics(aMapView.GraphicCollection, fromProj, toProj);
                aMapView.SetGraphics(newGCollection);
            }

            aMapView.Extent = aMapView.GetLayersWholeExtent();
            Extent aExten = aMapView.Extent;
            //aMapView.LonLatLayer = aMapView.Projection.GenerateLonLatLayer();
            //if (aMapView.LonLatLayer == null)
            aMapView.LonLatLayer = aMapView.GenerateLonLatLayer();
            //aMapView.LonLatProjLayer = ProjectLayer(aMapView.LonLatLayer, fromProj, toProj);
            ProjectLayer(aMapView.LonLatLayer, toProj);
            aMapView.LonLatProjLayer = aMapView.LonLatLayer;
            for (int i = 0; i < aMapView.LonLatProjLayer.ShapeNum; i++)
            {
                PolylineShape aPLS = (PolylineShape)aMapView.LonLatProjLayer.ShapeList[i];
                if (aPLS.PolyLines.Count == 2)
                {
                    PointD aP = aPLS.PolyLines[0].PointList[aPLS.PolyLines[0].PointList.Count - 1];
                    PointD bP = aPLS.PolyLines[1].PointList[aPLS.PolyLines[1].PointList.Count - 1];
                    bool isJoin = false;
                    if (refLon == 0)
                    {
                        if (Math.Abs(aP.X) < 0.1 && Math.Abs(bP.X) < 0.1 && MIMath.DoubleEquals(aP.Y, bP.Y))
                            isJoin = true;
                    }
                    else
                    {
                        if (MIMath.DoubleEquals(aP.X, bP.X) && MIMath.DoubleEquals(aP.Y, bP.Y))
                            isJoin = true;
                    }

                    if (isJoin)
                    {
                        List<PolyLine> polyLines = new List<PolyLine>();
                        PolyLine aPL = new PolyLine();
                        List<PointD> pList = new List<PointD>(aPLS.PolyLines[1].PointList);
                        List<PointD> bPList = new List<PointD>(aPLS.PolyLines[0].PointList);
                        bPList.Reverse();
                        pList.AddRange(bPList);
                        aPL.PointList = pList;
                        polyLines.Add(aPL);
                        aPLS.PolyLines = polyLines;
                        aMapView.LonLatProjLayer.ShapeList[i] = aPLS;
                    }
                }
            }

            if (isUpdateView)
                aMapView.LockViewUpdate = false;
            aMapView.ZoomToExtent(aExten);

            OnProjectionChanged();
        }

        /// <summary>
        /// Cut polyline shape by longitude
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="aPLS"></param>
        /// <returns></returns>
        public List<PolylineShape> CutPolyLineShapeLon(double lon, PolylineShape aPLS)
        {
            List<PolylineShape> plsList = new List<PolylineShape>();
            if (!((aPLS.Extent.minX < lon && aPLS.Extent.maxX > lon)
                || aPLS.Extent.minX == lon || aPLS.Extent.maxX == lon))
            {
                plsList.Add(aPLS);
                return plsList;
            }

            if (!(aPLS.PartNum > 1))
            {
                plsList = CutPolyLineLon(lon, aPLS);
            }
            else
            {
                List<PointD> pointList = aPLS.Points;
                for (int i = 0; i < aPLS.PartNum; i++)
                {
                    List<PointD> pList = new List<PointD>();
                    if (i == aPLS.PartNum - 1)
                    {
                        for (int j = aPLS.parts[i]; j < aPLS.PointNum; j++)
                        {
                            pList.Add(pointList[j]);
                        }
                    }
                    else
                    {
                        for (int j = aPLS.parts[i]; j < aPLS.parts[i + 1]; j++)
                        {
                            pList.Add(pointList[j]);
                        }
                    }
                    plsList.AddRange(CutPolyLineLon(lon, aPLS, pList));
                }
            }

            return plsList;
        }

        private List<PolylineShape> CutPolyLineLon(double lon, PolylineShape bPLS, List<PointD> pList)
        {
            List<PolylineShape> plsList = new List<PolylineShape>();

            PolylineShape aPLS = new PolylineShape();
            aPLS = (PolylineShape)bPLS.Clone();
            aPLS.PartNum = 1;
            aPLS.parts = new int[1];
            aPLS.parts[0] = 0;
            aPLS.Points = pList;
            
            Extent aExtent = MIMath.GetPointsExtent(pList);
            if (!((aExtent.minX < lon && aExtent.maxX > lon)
                || aExtent.minX == lon || aExtent.maxX == lon))
            {
                plsList.Add(aPLS);
                return plsList;
            }

            List<PointD> newPList = new List<PointD>();
            List<PointD> tempPList = new List<PointD>();
            PointD wP = new PointD();
            PointD wP1 = new PointD();
            PointD wP2 = new PointD();
            int i;
            List<PointD> crossPoints = new List<PointD>();
            List<int> crossPIdxs = new List<int>();

            //Calculate cross points
            for (i = 0; i < pList.Count - 1; i++)
            {
                wP1 = pList[i];
                wP2 = pList[i + 1];
                if ((wP1.X - lon) * (wP2.X - lon) <= 0)
                {
                    wP = new PointD();
                    wP.X = lon;
                    wP.Y = wP1.Y + (wP2.Y - wP1.Y) / (wP2.X - wP1.X) * (wP.X - wP1.X);
                    crossPoints.Add(wP);
                    crossPIdxs.Add(i + 1);
                }
            }
            if (crossPoints.Count == 0)
            {
                plsList.Add(aPLS);
                return plsList;
            }

            //DataRow

            PolylineShape newPLS;
            int aCount, sIdx;
            wP1 = pList[0];
            wP2 = pList[pList.Count - 1];
            bool isClose = false;
            if (wP1.X == wP2.X && wP1.Y == wP2.Y && !(wP1.X == lon))
            {
                isClose = true;
            }
            if (wP1.X == lon)
            {
                aCount = crossPIdxs[0] - 1;
            }
            else
            {
                aCount = crossPIdxs[0];
            }
            if (aCount > 0)
            {
                tempPList = pList.GetRange(0, aCount);
                wP = crossPoints[0];
                if ((tempPList[0]).X < lon)
                {
                    wP.X = lon - 0.0001;
                    //wP.X = lon - 0.01;
                }
                else
                {
                    wP.X = lon + 0.0001;
                }
                tempPList.Add(wP);
            }

            for (i = 0; i < crossPoints.Count; i++)
            {
                wP1 = crossPoints[i];
                if (i == crossPoints.Count - 1)
                {
                    sIdx = crossPIdxs[i];
                    aCount = pList.Count - crossPIdxs[i];
                    if (((PointD)pList[crossPIdxs[i]]).X == lon)
                    {
                        sIdx += 1;
                        aCount -= 1;
                    }
                    if (((PointD)pList[pList.Count - 1]).X == lon)
                    {
                        aCount = aCount - 1;
                    }
                    if (aCount > 0)
                    {
                        newPList = pList.GetRange(sIdx, aCount);
                        if (((PointD)newPList[0]).X < lon)
                        {
                            wP1.X = lon - 0.0001;
                        }
                        else
                        {
                            wP1.X = lon + 0.0001;
                        }
                        newPList.Insert(0, wP1);
                    }
                    if (isClose)
                    {
                        //newPList.AddRange(tempPList);    //Order is reverse in Mono
                        foreach (PointD aPD in tempPList)
                        {
                            newPList.Add(aPD);
                        }
                    }
                    else
                    {
                        if (tempPList.Count > 2)
                        {
                            newPLS = new PolylineShape();
                            newPLS = (PolylineShape)aPLS.Clone();
                            newPLS.Points = tempPList;
                            plsList.Insert(0, newPLS);
                        }
                    }
                }
                else
                {
                    wP2 = crossPoints[i + 1];
                    sIdx = crossPIdxs[i];
                    aCount = crossPIdxs[i + 1] - crossPIdxs[i];
                    if (((PointD)pList[crossPIdxs[i + 1]]).X == lon)
                    {
                        aCount -= 1;
                    }
                    if (((PointD)pList[crossPIdxs[i]]).X == lon)
                    {
                        sIdx += 1;
                        aCount -= 1;
                    }
                    if (aCount > 1)
                    {
                        newPList = pList.GetRange(sIdx, aCount);
                        if ((newPList[0]).X < lon)
                        {
                            wP1.X = lon - 0.0001;
                            wP2.X = lon - 0.0001;
                        }
                        else
                        {
                            wP1.X = lon + 0.0001;
                            wP2.X = lon + 0.0001;
                        }
                        newPList.Insert(0, wP1);
                        newPList.Add(wP2);
                    }
                }
                if (newPList.Count > 3)
                {
                    newPLS = new PolylineShape();
                    newPLS = (PolylineShape)aPLS.Clone();
                    newPLS.Points = newPList;
                    plsList.Add(newPLS);
                    newPList = new List<PointD>();
                }
            }

            return plsList;
        }

        /// <summary>
        /// Cut polyline by a longitude
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="aPLS"></param>
        /// <returns></returns>
        private List<PolylineShape> CutPolyLineLon(double lon, PolylineShape aPLS)
        {
            List<PolylineShape> plsList = new List<PolylineShape>();
            List<PointD> pList = aPLS.Points;
            List<PointD> newPList = new List<PointD>();
            List<PointD> tempPList = new List<PointD>();
            PointD wP = new PointD();
            PointD wP1 = new PointD();
            PointD wP2 = new PointD();
            int i;
            List<PointD> crossPoints = new List<PointD>();
            List<int> crossPIdxs = new List<int>();

            //Calculate cross points
            for (i = 0; i < pList.Count - 1; i++)
            {
                wP1 = pList[i];
                wP2 = pList[i + 1];
                if ((wP1.X - lon) * (wP2.X - lon) <= 0)
                {
                    wP = new PointD();
                    wP.X = lon;
                    wP.Y = wP1.Y + (wP2.Y - wP1.Y) / (wP2.X - wP1.X) * (wP.X - wP1.X);
                    crossPoints.Add(wP);
                    crossPIdxs.Add(i + 1);
                }
            }
            if (crossPoints.Count == 0)
            {
                plsList.Add(aPLS);
                return plsList;
            }

            //DataRow

            PolylineShape newPLS;
            int aCount, sIdx;
            wP1 = pList[0];
            wP2 = pList[pList.Count - 1];
            bool isClose = false;
            if (wP1.X == wP2.X && wP1.Y == wP2.Y && !(wP1.X == lon))
            {
                isClose = true;
            }
            if (wP1.X == lon)
            {
                aCount = crossPIdxs[0] - 1;
            }
            else
            {
                aCount = crossPIdxs[0];
            }
            if (aCount > 0)
            {
                tempPList = pList.GetRange(0, aCount);
                wP = crossPoints[0];
                if ((tempPList[0]).X < lon)
                {
                    wP.X = lon - 0.0001;
                    //wP.X = lon - 0.01;
                }
                else
                {
                    wP.X = lon + 0.0001;
                }
                tempPList.Add(wP);
            }

            for (i = 0; i < crossPoints.Count; i++)
            {
                wP1 = crossPoints[i];
                if (i == crossPoints.Count - 1)
                {
                    sIdx = crossPIdxs[i];
                    aCount = pList.Count - crossPIdxs[i];
                    if ((pList[crossPIdxs[i]]).X == lon)
                    {
                        sIdx += 1;
                        aCount -= 1;
                    }
                    if ((pList[pList.Count - 1]).X == lon)
                    {
                        aCount = aCount - 1;
                    }
                    if (aCount > 0)
                    {
                        newPList = pList.GetRange(sIdx, aCount);
                        if ((newPList[0]).X < lon)
                        {
                            wP1.X = lon - 0.0001;
                        }
                        else
                        {
                            wP1.X = lon + 0.0001;
                        }
                        newPList.Insert(0, wP1);
                    }
                    if (isClose)
                    {
                        //newPList.AddRange(tempPList);
                        foreach (PointD aPD in tempPList)
                        {
                            newPList.Add(aPD);
                        }
                    }
                    else
                    {
                        if (tempPList.Count > 2)
                        {
                            newPLS = new PolylineShape();
                            newPLS = (PolylineShape)aPLS.Clone();
                            newPLS.Points = tempPList;
                            plsList.Insert(0, newPLS);
                        }
                    }
                }
                else
                {
                    wP2 = crossPoints[i + 1];
                    sIdx = crossPIdxs[i];
                    aCount = crossPIdxs[i + 1] - crossPIdxs[i];
                    if ((pList[crossPIdxs[i + 1]]).X == lon)
                    {
                        aCount -= 1;
                    }
                    if ((pList[crossPIdxs[i]]).X == lon)
                    {
                        sIdx += 1;
                        aCount -= 1;
                    }
                    if (aCount > 1)
                    {
                        newPList = pList.GetRange(sIdx, aCount);
                        if ((newPList[0]).X < lon)
                        {
                            wP1.X = lon - 0.0001;
                            wP2.X = lon - 0.0001;
                        }
                        else
                        {
                            wP1.X = lon + 0.0001;
                            wP2.X = lon + 0.0001;
                        }
                        newPList.Insert(0, wP1);
                        newPList.Add(wP2);
                    }
                }
                if (newPList.Count > 3)
                {
                    newPLS = new PolylineShape();
                    newPLS = (PolylineShape)aPLS.Clone();
                    newPLS.Points = newPList;
                    plsList.Add(newPLS);
                    newPList = new List<PointD>();
                }
            }

            return plsList;
        }

        /// <summary>
        /// Cut polygon shape by longitude
        /// </summary>
        /// <param name="lon">longitude</param>
        /// <param name="aPGS">polygon shape</param>
        /// <returns>polygon shape list</returns>
        public List<PolygonShape> CutPolygonShapeLon(double lon, PolygonShape aPGS)
        {
            List<PolygonShape> pgsList = new List<PolygonShape>();
            if (!((aPGS.Extent.minX < lon && aPGS.Extent.maxX > lon)
                || aPGS.Extent.minX == lon || aPGS.Extent.maxX == lon))
            {
                pgsList.Add(aPGS);
                return pgsList;
            }

            if (!(aPGS.PartNum > 1))
            {
                pgsList = CutPolygonLon(lon, aPGS);
            }
            else
            {
                List<PointD> pointList = aPGS.Points;
                for (int i = 0; i < aPGS.PartNum; i++)
                {
                    List<PointD> pList = new List<PointD>();
                    if (i == aPGS.PartNum - 1)
                    {
                        for (int j = aPGS.parts[i]; j < aPGS.PointNum; j++)
                        {
                            pList.Add(pointList[j]);
                        }
                    }
                    else
                    {
                        for (int j = aPGS.parts[i]; j < aPGS.parts[i + 1]; j++)
                        {
                            pList.Add(pointList[j]);
                        }
                    }
                    pgsList.AddRange(CutPolygonLon(lon, aPGS, pList));
                }
            }

            return pgsList;
        }

        private List<PolygonShape> CutPolygonLon(double lon, PolygonShape bPGS, List<PointD> pList)
        {
            List<PolygonShape> pgsList = new List<PolygonShape>();

            PolygonShape aPGS = new PolygonShape();
            aPGS = (PolygonShape)bPGS.Clone();
            aPGS.PartNum = 1;
            aPGS.parts = new int[1];
            aPGS.parts[0] = 0;
            aPGS.Points = pList;
            
            Extent aExtent = MIMath.GetPointsExtent(pList);
            if (!((aExtent.minX < lon && aExtent.maxX > lon)
                || aExtent.minX == lon || aExtent.maxX == lon))
            {
                pgsList.Add(aPGS);
                return pgsList;
            }

            List<PointD> newPList = new List<PointD>();
            List<PointD> tempPList = new List<PointD>();
            PointD wP = new PointD();
            PointD wP1 = new PointD();
            PointD wP2 = new PointD();
            int i;
            List<PointD> crossPoints = new List<PointD>();
            List<int> crossPIdxs = new List<int>();

            //Calculate cross points
            for (i = 0; i < pList.Count - 1; i++)
            {
                wP1 = pList[i];
                wP2 = pList[i + 1];
                if ((wP1.X - lon) * (wP2.X - lon) <= 0)
                {
                    wP = new PointD();
                    wP.X = lon;
                    wP.Y = wP1.Y + (wP2.Y - wP1.Y) / (wP2.X - wP1.X) * (wP.X - wP1.X);
                    crossPoints.Add(wP);
                    crossPIdxs.Add(i + 1);
                }
            }
            if (crossPoints.Count == 0)
            {
                pgsList.Add(aPGS);
                return pgsList;
            }

            int aCount, sIdx;
            wP1 = pList[0];
            wP2 = pList[pList.Count - 1];
            sIdx = 0;
            aCount = crossPIdxs[0];
            if (wP1.X == lon)
            {
                sIdx += 1;
                aCount = crossPIdxs[0] - 1;
            }
            if (aCount > 0)
            {
                tempPList = pList.GetRange(sIdx, aCount);
                wP = crossPoints[0];
                if ((tempPList[0]).X < lon)
                {
                    wP.X = lon - 0.0001;
                }
                else if ((tempPList[0]).X > lon)
                {
                    wP.X = lon + 0.0001;
                }
                else
                {
                    wP.X = lon;
                }
                tempPList.Add(wP);
            }

            for (i = 0; i < crossPoints.Count; i++)
            {
                wP1 = crossPoints[i];
                if (i == crossPoints.Count - 1)
                {
                    sIdx = crossPIdxs[i];
                    aCount = pList.Count - crossPIdxs[i];
                    if ((pList[crossPIdxs[i]]).X == lon)
                    {
                        sIdx += 1;
                        aCount -= 1;
                    }
                    if ((pList[pList.Count - 1]).X == lon)
                    {
                        aCount = aCount - 1;
                    }
                    if (aCount > 0)
                    {
                        newPList = pList.GetRange(sIdx, aCount);

                        if (((PointD)newPList[0]).X < lon)
                        {
                            wP1.X = lon - 0.0001;
                        }
                        else if (((PointD)newPList[0]).X > lon)
                        {
                            wP1.X = lon + 0.0001;
                        }
                        else
                        {
                            wP1.X = lon;
                        }

                        newPList.Insert(0, wP1);

                        //newPList.AddRange(tempPList);
                        foreach (PointD aPD in tempPList)
                        {
                            newPList.Add(aPD);
                        }

                        wP2 = newPList[newPList.Count - 1];
                        if (Math.Abs(wP2.Y - wP1.Y) > 1)
                        {
                            PointD tempP = new PointD();
                            tempP.X = wP1.X;
                            for (int j = 0; j < (int)Math.Abs(wP2.Y - wP1.Y); j++)
                            {
                                if (wP1.Y < wP2.Y)
                                {
                                    tempP.Y = wP2.Y - j;
                                }
                                else
                                {
                                    tempP.Y = wP2.Y + j;
                                }
                                newPList.Add(tempP);
                            }
                        }
                        newPList.Add(wP1);
                    }

                }
                else
                {
                    wP2 = crossPoints[i + 1];
                    sIdx = crossPIdxs[i];
                    aCount = crossPIdxs[i + 1] - crossPIdxs[i];
                    if ((pList[crossPIdxs[i + 1]]).X == lon)
                    {
                        aCount -= 1;
                    }
                    if ((pList[crossPIdxs[i]]).X == lon)
                    {
                        sIdx += 1;
                        aCount -= 1;
                    }
                    if (aCount > 1)
                    {
                        newPList = pList.GetRange(sIdx, aCount);
                        if ((newPList[0]).X < lon)
                        {
                            wP1.X = lon - 0.0001;
                            wP2.X = lon - 0.0001;
                        }
                        else if (((PointD)newPList[0]).X > lon)
                        {
                            wP1.X = lon + 0.0001;
                            wP2.X = lon + 0.0001;
                        }
                        else
                        {
                            wP1.X = lon;
                            wP2.X = lon;
                        }

                        newPList.Insert(0, wP1);
                        newPList.Add(wP2);
                        if (Math.Abs(wP2.Y - wP1.Y) > 1)
                        {                            
                            for (int j = 0; j < (int)Math.Abs(wP2.Y - wP1.Y); j++)
                            {
                                PointD tempP = new PointD();
                                tempP.X = wP1.X;
                                if (wP1.Y < wP2.Y)
                                {
                                    tempP.Y = wP2.Y - j;
                                }
                                else
                                {
                                    tempP.Y = wP2.Y + j;
                                }
                                newPList.Add(tempP);
                            }
                        }
                        newPList.Add(wP1);
                    }
                }
                if (newPList.Count > 4)
                {
                    PolygonShape newPGS = new PolygonShape();
                    newPGS = (PolygonShape)aPGS.Clone();
                    newPGS.Points = newPList;
                    pgsList.Add(newPGS);
                    newPList = new List<PointD>();
                }
            }

            return pgsList;
        }

        /// <summary>
        /// Cut polygon by a longitude
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="aPGS"></param>
        /// <returns></returns>
        private List<PolygonShape> CutPolygonLon(double lon, PolygonShape aPGS)
        {
            List<PolygonShape> pgsList = new List<PolygonShape>();

            //if (!((aPGS.Extent.minX < lon && aPGS.Extent.maxX > lon)
            //    || aPGS.Extent.minX == lon || aPGS.Extent.maxX == lon))
            //{
            //    pgsList.Add(aPGS);
            //    return pgsList;
            //}

            List<PointD> pList = aPGS.Points;
            List<PointD> newPList = new List<PointD>();
            List<PointD> tempPList = new List<PointD>();
            PointD wP = new PointD();
            PointD wP1 = new PointD();
            PointD wP2 = new PointD();
            int i;
            List<PointD> crossPoints = new List<PointD>();
            List<int> crossPIdxs = new List<int>();

            //Calculate cross points
            for (i = 0; i < pList.Count - 1; i++)
            {
                wP1 = pList[i];
                wP2 = pList[i + 1];
                if ((wP1.X - lon) * (wP2.X - lon) <= 0)
                {
                    wP = new PointD();
                    wP.X = lon;
                    wP.Y = wP1.Y + (wP2.Y - wP1.Y) / (wP2.X - wP1.X) * (wP.X - wP1.X);
                    crossPoints.Add(wP);
                    crossPIdxs.Add(i + 1);
                }
            }
            if (crossPoints.Count == 0)
            {
                pgsList.Add(aPGS);
                return pgsList;
            }

            int aCount, sIdx;
            wP1 = pList[0];
            wP2 = pList[pList.Count - 1];
            sIdx = 0;
            aCount = crossPIdxs[0];
            if (wP1.X == lon)
            {
                sIdx += 1;
                aCount = crossPIdxs[0] - 1;
            }
            if (aCount > 0)
            {
                tempPList = pList.GetRange(sIdx, aCount);
                wP = crossPoints[0];
                if ((tempPList[0]).X < lon)
                {
                    wP.X = lon - 0.0001;
                }
                else if ((tempPList[0]).X > lon)
                {
                    wP.X = lon + 0.0001;
                }
                else
                {
                    wP.X = lon;
                }
                tempPList.Add(wP);
            }

            for (i = 0; i < crossPoints.Count; i++)
            {
                wP1 = crossPoints[i];
                if (i == crossPoints.Count - 1)
                {
                    sIdx = crossPIdxs[i];
                    aCount = pList.Count - crossPIdxs[i];
                    if ((pList[crossPIdxs[i]]).X == lon)
                    {
                        sIdx += 1;
                        aCount -= 1;
                    }
                    if ((pList[pList.Count - 1]).X == lon)
                    {
                        aCount = aCount - 1;
                    }
                    if (aCount > 0)
                    {
                        newPList = pList.GetRange(sIdx, aCount);

                        if ((newPList[0]).X < lon)
                        {
                            wP1.X = lon - 0.0001;
                        }
                        else if ((newPList[0]).X > lon)
                        {
                            wP1.X = lon + 0.0001;
                        }
                        else
                        {
                            wP1.X = lon;
                        }

                        newPList.Insert(0, wP1);

                        //newPList.AddRange(tempPList);
                        foreach (PointD aPD in tempPList)
                        {
                            newPList.Add(aPD);
                        }

                        wP2 = newPList[newPList.Count - 1];
                        if (Math.Abs(wP2.Y - wP1.Y) > 1)
                        {                            
                            for (int j = 0; j < (int)Math.Abs(wP2.Y - wP1.Y); j++)
                            {
                                PointD tempP = new PointD();
                                tempP.X = wP1.X;
                                if (wP1.Y < wP2.Y)
                                {
                                    tempP.Y = wP2.Y - j;
                                }
                                else
                                {
                                    tempP.Y = wP2.Y + j;
                                }
                                newPList.Add(tempP);
                            }
                        }
                        newPList.Add(wP1);
                    }

                }
                else
                {
                    wP2 = crossPoints[i + 1];
                    sIdx = crossPIdxs[i];
                    aCount = crossPIdxs[i + 1] - crossPIdxs[i];
                    if ((pList[crossPIdxs[i + 1]]).X == lon)
                    {
                        aCount -= 1;
                    }
                    if ((pList[crossPIdxs[i]]).X == lon)
                    {
                        sIdx += 1;
                        aCount -= 1;
                    }
                    if (aCount > 1)
                    {
                        newPList = pList.GetRange(sIdx, aCount);
                        if ((newPList[0]).X < lon)
                        {
                            wP1.X = lon - 0.0001;
                            wP2.X = lon - 0.0001;
                        }
                        else if ((newPList[0]).X > lon)
                        {
                            wP1.X = lon + 0.0001;
                            wP2.X = lon + 0.0001;
                        }
                        else
                        {
                            wP1.X = lon;
                            wP2.X = lon;
                        }

                        newPList.Insert(0, wP1);
                        newPList.Add(wP2);
                        if (Math.Abs(wP2.Y - wP1.Y) > 1)
                        {                            
                            for (int j = 0; j < (int)Math.Abs(wP2.Y - wP1.Y); j++)
                            {
                                PointD tempP = new PointD();
                                tempP.X = wP1.X;
                                if (wP1.Y < wP2.Y)
                                {
                                    tempP.Y = wP2.Y - j;
                                }
                                else
                                {
                                    tempP.Y = wP2.Y + j;
                                }
                                newPList.Add(tempP);
                            }
                        }
                        newPList.Add(wP1);
                    }
                }
                if (newPList.Count > 4)
                {
                    PolygonShape newPGS = new PolygonShape();
                    newPGS = (PolygonShape)aPGS.Clone();
                    newPGS.Points = newPList;
                    pgsList.Add(newPGS );
                    newPList = new List<PointD>();
                }
            }

            return pgsList;
        }

        ///// <summary>
        ///// Generate Lon/Lat layer
        ///// </summary>
        ///// <returns>vector layer</returns>
        //public VectorLayer GenerateLonLatLayer_Bak()
        //{
        //    //Create lon/lat layer                        
        //    PolylineShape aPLS = new PolylineShape();    
        //    int i, j, lineNum;
        //    double lon, lat;
        //    PointD wPoint = new PointD();
        //    List<PointD> PList = new List<PointD>();
        //    double refLon;
        //    int refIntLon;

        //    //Define coordinate transform            
        //    ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
        //    ProjectionInfo toProj = _projInfo;

        //    refLon = _refCutLon;
        //    refIntLon = (int)refLon;

        //    VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
        //    string columnName = "Value";            
        //    DataColumn aDC = new DataColumn();
        //    aDC.ColumnName = columnName;
        //    aDC.DataType = typeof(Single);
        //    aLayer.EditAddField(aDC);                        
        //    int shapeNum;

        //    //Longitude
        //    lineNum = 0;
        //    Extent extent;
        //    List<double> lonList = new List<double>();
        //    for (i = 0; i <= 36; i++)
        //    {
        //        lon = -180 + i * 10;
        //        lonList.Add(lon);
        //    }
        //    lonList.Add(refLon - 0.0001);
        //    lonList.Add(refLon + 0.0001);

        //    for (i = 0; i <lonList.Count; i++)
        //    {
        //        lon = lonList[i];
        //        if (lon == refIntLon)
        //        {
        //            continue;
        //        }
        //        aPLS = new PolylineShape();
        //        aPLS.value = lon;                
        //        extent.minX = lon;
        //        extent.maxX = lon;
        //        extent.minY = -90;
        //        extent.maxY = 90;
        //        aPLS.Extent = extent;
        //        PList = new List<PointD>();
        //        for (j = -90; j <= 90; j++)
        //        {
        //            wPoint.X = lon;
        //            wPoint.Y = j;
        //            PList.Add(wPoint);
        //        }
        //        switch (toProj.Transform.ProjectionName)
        //        {
        //            case ProjectionNames.Lambert_Conformal:
        //            case ProjectionNames.North_Polar_Stereographic:
        //                PList.RemoveRange(0, 10);
        //                extent.minY = -80;
        //                aPLS.Extent = extent;
        //                break;
        //            case ProjectionNames.South_Polar_Stereographic:
        //                PList.RemoveRange(PList.Count - 10, 10);
        //                extent.maxY = 80;
        //                aPLS.Extent = extent;
        //                break;
        //            case ProjectionNames.Oblique_Stereographic:
        //            case ProjectionNames.Transverse_Mercator:
        //                PList.RemoveRange(0, 10);
        //                PList.RemoveRange(PList.Count - 10, 10);
        //                extent.minY = -80;
        //                extent.maxY = 80;
        //                aPLS.Extent = extent;
        //                break;
        //        }
        //        aPLS.Points = PList;
        //        aPLS.numPoints = PList.Count;

        //        shapeNum = aLayer.ShapeNum;
        //        if (aLayer.EditInsertShape(aPLS, shapeNum))
        //            aLayer.EditCellValue(columnName, shapeNum, lon);
                
        //        lineNum += 1;
        //    }

        //    ////Add longitudes around reference longitude
        //    //lon = refLon - 0.0001;
        //    //aPLS = new PolylineShape();
        //    //aPLS.value = lon;            
        //    //extent.minX = lon;
        //    //extent.maxX = lon;
        //    //extent.minY = -80;
        //    //extent.maxY = 90;
        //    //aPLS.Extent = extent;
        //    //PList = new List<PointD>();
        //    //for (j = -80; j <= 90; j++)
        //    //{
        //    //    wPoint.X = lon;
        //    //    wPoint.Y = j;
        //    //    PList.Add(wPoint);
        //    //}
        //    //aPLS.Points = PList;
        //    //aPLS.numPoints = PList.Count;
        //    //shapeNum = aLayer.ShapeNum;
        //    //if (aLayer.EditInsertShape(aPLS, shapeNum))
        //    //    aLayer.EditCellValue(columnName, shapeNum, lon);
            
        //    //lineNum += 1;

        //    //lon = refLon + 0.0001;
        //    //aPLS = new PolylineShape();
        //    //aPLS.value = lon;
        //    //extent.minX = lon;
        //    //extent.maxX = lon;
        //    //extent.minY = -80;
        //    //extent.maxY = 90;
        //    //aPLS.Extent = extent;
        //    //PList = new List<PointD>();
        //    //for (j = -80; j <= 90; j++)
        //    //{
        //    //    wPoint.X = lon;
        //    //    wPoint.Y = j;
        //    //    PList.Add(wPoint);
        //    //}
        //    //aPLS.Points = PList;
        //    //aPLS.numPoints = PList.Count;

        //    //shapeNum = aLayer.ShapeNum;
        //    //if (aLayer.EditInsertShape(aPLS, shapeNum))
        //    //    aLayer.EditCellValue(columnName, shapeNum, lon);

        //    //lineNum += 1;            

        //    //Latitue
        //    for (i = 0; i <= 18; i++)
        //    {
        //        lat = -90 + i * 10;
        //        aPLS = new PolylineShape();
        //        aPLS.value = lat;
        //        extent.minX = -180;
        //        extent.maxX = 180;
        //        extent.minY = lat;
        //        extent.maxY = lat;
        //        aPLS.Extent = extent;
        //        PList = new List<PointD>();
        //        switch (toProj.Transform.ProjectionName)
        //        {
        //            case ProjectionNames.Lambert_Conformal:
        //                if (i == 0)
        //                {
        //                    continue;
        //                }
        //                extent.minX = -180;
        //                extent.maxX = refIntLon;
        //                extent.minY = lat;
        //                extent.maxY = lat;
        //                aPLS.Extent = extent;
        //                PList = new List<PointD>();
        //                for (j = -180; j < refIntLon; j++)
        //                {
        //                    wPoint.X = j;
        //                    wPoint.Y = lat;
        //                    PList.Add(wPoint);
        //                }
        //                wPoint.X = refLon - 0.0001;
        //                wPoint.Y = lat;
        //                PList.Add(wPoint);
        //                if (PList.Count > 1)
        //                {
        //                    aPLS.Points = PList;
        //                    aPLS.numPoints = PList.Count;

        //                    shapeNum = aLayer.ShapeNum;
        //                    if (aLayer.EditInsertShape(aPLS, shapeNum))
        //                        aLayer.EditCellValue(columnName, shapeNum, lat);

        //                    lineNum += 1;
        //                }

        //                aPLS = new PolylineShape();
        //                extent.minX = refIntLon;
        //                extent.maxX = 180;
        //                extent.minY = lat;
        //                extent.maxY = lat;
        //                aPLS.Extent = extent;
        //                PList = new List<PointD>();
        //                wPoint.X = refLon + 0.0001;
        //                wPoint.Y = lat;
        //                PList.Add(wPoint);
        //                for (j = refIntLon + 1; j <= 180; j++)
        //                {
        //                    wPoint.X = j;
        //                    wPoint.Y = lat;
        //                    PList.Add(wPoint);
        //                }
        //                if (PList.Count > 1)
        //                {
        //                    aPLS.Points = PList;
        //                    aPLS.numPoints = PList.Count;

        //                    shapeNum = aLayer.ShapeNum;
        //                    if (aLayer.EditInsertShape(aPLS, shapeNum))
        //                        aLayer.EditCellValue(columnName, shapeNum, lat);

        //                    lineNum += 1;
        //                }
        //                break;
        //            case ProjectionNames.North_Polar_Stereographic:
        //                if (i == 0)
        //                {
        //                    continue;
        //                }
        //                for (j = -180; j <= 180; j++)
        //                {
        //                    wPoint.X = j;
        //                    wPoint.Y = lat;
        //                    PList.Add(wPoint);
        //                }
        //                break;
        //            case ProjectionNames.South_Polar_Stereographic:
        //                if (i == 18)
        //                {
        //                    continue;
        //                }
        //                for (j = -180; j <= 180; j++)
        //                {
        //                    wPoint.X = j;
        //                    wPoint.Y = lat;
        //                    PList.Add(wPoint);
        //                }
        //                break;  
        //            //case ProjectionNames.Oblique_Stereographic:
        //            //case ProjectionNames.Transverse_Mercator:

        //            //    break;
        //            default:
        //                for (j = -180; j <= 180; j++)
        //                {
        //                    wPoint.X = j;
        //                    wPoint.Y = lat;
        //                    PList.Add(wPoint);
        //                }
        //                break;
        //        }

        //        if (PList.Count > 1)
        //        {
        //            aPLS.Points = PList;
        //            aPLS.numPoints = PList.Count;

        //            shapeNum = aLayer.ShapeNum;
        //            if (aLayer.EditInsertShape(aPLS, shapeNum))
        //                aLayer.EditCellValue(columnName, shapeNum, lat);

        //            lineNum += 1;
        //        }
        //    }

        //    //if (m_projectionName == ProjectionName.Orthographic ||
        //    //    m_projectionName == ProjectionName.Transverse_Mercator)
        //    //{                
        //    //    ArrayList NPolylines = new ArrayList();
        //    //    NPolylines = (ArrayList)polylines.Clone();
        //    //    polylines.Clear();
        //    //    for (i = 0; i < NPolylines.Count; i++)
        //    //    {
        //    //        aPLS = (PolylineShape)NPolylines[i];
        //    //        PList = (ArrayList)aPLS.points.Clone();
        //    //        ArrayList NPList = new ArrayList();
        //    //        for (j = 0; j < PList.Count - 10; j += 10)
        //    //        {
        //    //            NPList.Clear();
        //    //            for (int k = 0; k < 11; k++)
        //    //            {
        //    //                NPList.Add(PList[j + k]);
        //    //            }
        //    //            aPLS.points = (ArrayList)NPList.Clone();
        //    //            polylines.Add(aPLS);

        //    //            aDR = aLayer.AttributeTable.Table.NewRow();
        //    //            aDR[aDC] = aPLS.value;
        //    //            aLayer.AttributeTable.Table.Rows.Add(aDR);
        //    //            lineNum += 1;
        //    //        }
        //    //    }
        //    //}            

        //    //Generate layer
        //    Extent lExt = new Extent();
        //    lExt.minX = -180;
        //    lExt.maxX = 180;
        //    lExt.minY = -90;
        //    lExt.maxY = 90;
            
        //    aLayer.Extent = lExt;
        //    aLayer.LayerName = "Map_LonLat";
        //    aLayer.FileName = "";
        //    aLayer.LayerDrawType = LayerDrawType.Map;                        
        //    aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
        //    PolyLineBreak aPLB = (PolyLineBreak)aLayer.LegendScheme.LegendBreaks[0];
        //    aPLB.Style = LineStyles.Dash;
        //    aLayer.LegendScheme.LegendBreaks[0] = aPLB;
        //    aLayer.Visible = true;                

        //    //Get projected lon/lat layer      
        //    VectorLayer lonLatLayer = ProjectLayer(aLayer, fromProj, toProj);
        //    return lonLatLayer;
        //}

        /// <summary>
        /// Generate Lon/Lat layer
        /// </summary>
        /// <returns>vector layer</returns>
        public VectorLayer GenerateLonLatLayer()
        {
            //Create lon/lat layer                        
            PolylineShape aPLS = new PolylineShape();
            int i, j, lineNum;
            double lon, lat;            
            List<PointD> PList = new List<PointD>();
            double refLon;
            int refIntLon;

            //Define coordinate transform            
            ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
            ProjectionInfo toProj = _projInfo;

            refLon = _refCutLon;
            refIntLon = (int)refLon;

            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
            string columnName = "Value";
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = columnName;
            aDC.DataType = typeof(Single);
            aLayer.EditAddField(aDC);
            int shapeNum;

            //Longitude
            lineNum = 0;
            Extent extent;
            List<double> lonList = new List<double>();
            for (i = 0; i <= 36; i++)
            {
                lon = -180 + i * 10;
                lonList.Add(lon);
            }
            lonList.Add(refLon - 0.0001);
            lonList.Add(refLon + 0.0001);

            for (i = 0; i < lonList.Count; i++)
            {
                lon = lonList[i];
                if (lon == refIntLon)
                {
                    continue;
                }
                int minLat = -90;
                int maxLat = 90;
                switch (toProj.Transform.ProjectionName)
                {
                    case ProjectionNames.Lambert_Conformal:
                    case ProjectionNames.North_Polar_Stereographic:                        
                        minLat = -80;                        
                        break;
                    case ProjectionNames.South_Polar_Stereographic:
                        maxLat = 80;                        
                        break;
                    case ProjectionNames.Oblique_Stereographic:
                    case ProjectionNames.Transverse_Mercator:
                        minLat = -80;
                        maxLat = 80;                        
                        break;
                }

                for (j = minLat; j < maxLat; j++)
                {
                    aPLS = new PolylineShape();
                    aPLS.value = lon;
                    extent.minX = lon;
                    extent.maxX = lon;
                    extent.minY = minLat;
                    extent.maxY = maxLat;
                    aPLS.Extent = extent;
                    PList = new List<PointD>();
                    PList.Add(new PointD(lon, j));
                    PList.Add(new PointD(lon, j + 1));

                    aPLS.Points = PList;

                    shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aPLS, shapeNum))
                        aLayer.EditCellValue(columnName, shapeNum, lon);

                    lineNum += 1;
                }                               
            }              

            //Latitue
            for (i = 0; i <= 18; i++)
            {
                switch (toProj.Transform.ProjectionName)
                {
                    case ProjectionNames.Lambert_Conformal:
                    case ProjectionNames.North_Polar_Stereographic:
                        if (i == 0)
                            continue;
                        break;
                    case ProjectionNames.South_Polar_Stereographic:
                        if (i == 18)
                            continue;
                        break;
                    case ProjectionNames.Oblique_Stereographic:
                    case ProjectionNames.Transverse_Mercator:
                        if (i == 0 || i == 18)
                            continue;
                        break;
                }

                lat = -90 + i * 10;

                for (j = -180; j < 180; j++)
                {
                    aPLS = new PolylineShape();
                    aPLS.value = lat;
                    extent.minX = -180;
                    extent.maxX = 180;
                    extent.minY = lat;
                    extent.maxY = lat;
                    aPLS.Extent = extent;
                    PList = new List<PointD>();
                    PList.Add(new PointD(j, lat));
                    PList.Add(new PointD(j + 1, lat));
               
                    aPLS.Points = PList;

                    shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aPLS, shapeNum))
                        aLayer.EditCellValue(columnName, shapeNum, lat);

                    lineNum += 1;
                }
            }                 

            //Generate layer
            Extent lExt = new Extent();
            lExt.minX = -180;
            lExt.maxX = 180;
            lExt.minY = -90;
            lExt.maxY = 90;

            aLayer.Extent = lExt;
            aLayer.LayerName = "Map_LonLat";
            aLayer.FileName = "";
            aLayer.LayerDrawType = LayerDrawType.Map;
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
            PolyLineBreak aPLB = (PolyLineBreak)aLayer.LegendScheme.LegendBreaks[0];
            aPLB.Style = LineStyles.Dash;
            aLayer.LegendScheme.LegendBreaks[0] = aPLB;
            aLayer.Visible = true;

            //Get projected lon/lat layer      
            //VectorLayer lonLatLayer = ProjectLayer(aLayer, fromProj, toProj);
            ProjectLayer(aLayer, toProj);
            return aLayer;
            //return lonLatLayer;
        }

        #endregion

        #region Events
        /// <summary>
        /// Fires ProjectionChangded event
        /// </summary>
        protected virtual void OnProjectionChanged()
        {
            if (ProjectionChanged != null) ProjectionChanged(this, new EventArgs());
        }

        #endregion
    }
}
