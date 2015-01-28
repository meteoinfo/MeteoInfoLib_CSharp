using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using System.IO;
using System.Drawing.Imaging;
using MeteoInfoC.Shape;
using MeteoInfoC.Layer;
using MeteoInfoC.Global;
using MeteoInfoC.Legend;
using MeteoInfoC.Drawing;
using MeteoInfoC.Data.MapData;
using MeteoInfoC.Geoprocess;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Draw meteo data
    /// </summary>
    public static class DrawMeteoData
    {        
        #region Methods

        /// <summary>
        /// Interpolate station data to grid data
        /// </summary>
        /// <param name="aStData">The station data</param>
        /// <param name="interpSet">Interpolation setting</param>
        /// <returns>Interpolated grid data</returns>
        public static GridData InterpolateData(StationData aStData, InterpolationSetting interpSet)
        {
            GridData aGridData = new GridData();
            double[] X = new double[1];
            double[] Y = new double[1];
            //GridDataSetting aGDP = interpSet.GridDataSet;
            //aGDP.DataExtent = aStData.DataExtent;
            //interpSet.GridDataSet = aGDP;
            //interpSet.Radius = Convert.ToSingle((interpSet.GridDataSet.DataExtent.maxX -
                //interpSet.GridDataSet.DataExtent.minX) / interpSet.GridDataSet.XNum * 2);
            ContourDraw.CreateGridXY(interpSet.GridDataSet, ref X, ref Y);

            double[,] S = aStData.Data;
            S = ContourDraw.FilterDiscreteData_Radius(S, interpSet.Radius,
                interpSet.GridDataSet.DataExtent, aStData.MissingValue);
            switch (interpSet.InterpolationMethod)
            {
                case InterpolationMethods.IDW_Radius:
                    aGridData = ContourDraw.InterpolateDiscreteData_Radius(S,
                        X, Y, interpSet.MinPointNum, interpSet.Radius, aStData.MissingValue);
                    break;
                case InterpolationMethods.IDW_Neighbors:
                    aGridData = ContourDraw.InterpolateDiscreteData_Neighbor(S, X, Y,
                        interpSet.MinPointNum, aStData.MissingValue);
                    break;
                case InterpolationMethods.Cressman:
                    aGridData = ContourDraw.InterpolateDiscreteData_Cressman(S, X, Y,
                        aStData.MissingValue, interpSet.RadList);
                    break;
            }

            return aGridData;
        }

        /// <summary>
        /// Create a layer from grid data
        /// </summary>
        /// <param name="gridData">The grid data</param>
        /// <param name="drawType">Draw type</param>
        /// <param name="aLS">Legend scheme</param>
        /// <param name="layerName">Layer name</param>
        /// <param name="fieldName">Field name</param>
        /// <returns>Created layer</returns>
        public static MapLayer CreateLayer(GridData gridData, DrawType2D drawType, LegendScheme aLS, string layerName, string fieldName)
        {
            MapLayer aLayer = null;
            switch (drawType)
            {
                case DrawType2D.Contour:
                    aLayer = DrawMeteoData.CreateContourLayer(gridData, aLS, layerName, fieldName);
                    break;
                case DrawType2D.Shaded:
                    aLayer = DrawMeteoData.CreateShadedLayer(gridData, aLS, layerName, fieldName);
                    break;
                case DrawType2D.Grid_Point:
                    aLayer = DrawMeteoData.CreateGridPointLayer(gridData, aLS, layerName, fieldName);
                    break;
                case DrawType2D.Grid_Fill:
                    aLayer = DrawMeteoData.CreateGridFillLayer(gridData, aLS, layerName, fieldName);
                    break;
                case DrawType2D.Raster:
                    aLayer = DrawMeteoData.CreateRasterLayer(gridData, layerName, aLS);
                    break;
            }            

            return aLayer;
        }

        /// <summary>
        /// Create a layer from two grid data
        /// </summary>
        /// <param name="uData">U/WindDirection grid data</param>
        /// <param name="vData">V/WindSpeed grid data</param>
        /// <param name="drawType">Draw type</param>
        /// <param name="aLS">Legend scheme</param>
        /// <param name="LayerName">Layer name</param>
        /// <param name="isUV">If is U/V</param>
        /// <returns>Created layer</returns>
        public static MapLayer CreateLayer(GridData uData, GridData vData, DrawType2D drawType, LegendScheme aLS, string LayerName, bool isUV)
        {
            MapLayer aLayer = null;
            switch (drawType)
            {
                case DrawType2D.Vector:
                    aLayer = DrawMeteoData.CreateGridVectorLayer(uData, vData, uData, aLS, false, LayerName, isUV);
                    break;
                case DrawType2D.Barb:
                    aLayer = DrawMeteoData.CreateGridBarbLayer(uData, vData, uData, aLS, false, LayerName, isUV);
                    break;
                case DrawType2D.Streamline:
                    aLayer = DrawMeteoData.CreateStreamlineLayer(uData, vData, 4, aLS, LayerName, isUV);
                    break;
            }

            return aLayer;
        }

        /// <summary>
        /// Create a streamline layer from two grid data
        /// </summary>
        /// <param name="uData">U/WindDirection grid data</param>
        /// <param name="vData">V/WindSpeed grid data</param>
        /// <param name="drawType">Draw type</param>
        /// <param name="aLS">Legend scheme</param>
        /// <param name="LayerName">Layer name</param>
        /// <param name="isUV">If is U/V</param>
        /// <param name="density">Density</param>
        /// <returns>Created layer</returns>
        public static MapLayer CreateLayer(GridData uData, GridData vData, DrawType2D drawType, LegendScheme aLS, string LayerName,
            bool isUV, int density)
        {
            MapLayer aLayer = null;
            switch (drawType)
            {
                case DrawType2D.Streamline:
                    aLayer = DrawMeteoData.CreateStreamlineLayer(uData, vData, density, aLS, LayerName, isUV);
                    break;
            }

            return aLayer;
        }

        /// <summary>
        /// Create a layer from three grid data
        /// </summary>
        /// <param name="uData">U/WindDirection grid data</param>
        /// <param name="vData">V/WindSpeed grid data</param>
        /// <param name="gridData">Grid data</param>
        /// <param name="drawType">Draw type</param>
        /// <param name="aLS">Legend scheme</param>
        /// <param name="LayerName">Layer name</param>
        /// <param name="isUV">If is U/V</param>
        /// <returns>Created layer</returns>
        public static MapLayer CreateLayer(GridData uData, GridData vData, GridData gridData, DrawType2D drawType, LegendScheme aLS,
            string LayerName, bool isUV)
        {
            MapLayer aLayer = null;
            switch (drawType)
            {
                case DrawType2D.Vector:
                    aLayer = DrawMeteoData.CreateGridVectorLayer(uData, vData, gridData, aLS, true, LayerName, isUV);
                    break;
                case DrawType2D.Barb:
                    aLayer = DrawMeteoData.CreateGridBarbLayer(uData, vData, gridData, aLS, true, LayerName, isUV);
                    break;
            }

            return aLayer;
        }

        /// <summary>
        /// Create a layer from station data
        /// </summary>
        /// <param name="stationData">Station data</param>
        /// <param name="drawType">Draw type</param>
        /// <param name="aLS">Legend scheme</param>
        /// <param name="layerName">Layer name</param>
        /// <param name="fieldName">Field name (for station point layer) or weather type (for weather symbol layer)</param>
        /// <returns>Created layer</returns>
        public static MapLayer CreateLayer(StationData stationData, DrawType2D drawType, LegendScheme aLS, string layerName,
            string fieldName)
        {
            MapLayer aLayer = null;
            switch (drawType)
            {
                case DrawType2D.Station_Point:
                    aLayer = DrawMeteoData.CreateSTPointLayer(stationData, aLS, layerName, fieldName);
                    break;
                case DrawType2D.Weather_Symbol:
                    aLayer = DrawMeteoData.CreateWeatherSymbolLayer(stationData, fieldName, aLS, layerName);
                    break;
            }

            return aLayer;
        }

        /// <summary>
        /// Create a layer from station data
        /// </summary>
        /// <param name="stationData">Station data</param>
        /// <param name="drawType">Draw type</param>
        /// <param name="aLS">Legend scheme</param>
        /// <param name="layerName">Layer name</param>        
        /// <returns>Created layer</returns>
        public static MapLayer CreateLayer(StationData stationData, DrawType2D drawType, LegendScheme aLS, string layerName)
        {
            MapLayer aLayer = null;
            switch (drawType)
            {
                case DrawType2D.Weather_Symbol:
                    aLayer = DrawMeteoData.CreateWeatherSymbolLayer(stationData, aLS, layerName);
                    break;
            }

            return aLayer;
        }

        /// <summary>
        /// Create a layer with station data
        /// </summary>
        /// <param name="stationData">Station data</param>
        /// <param name="drawType">Draw type</param>
        /// <param name="aLS">Legend scheme</param>
        /// <param name="layerName">Layer name</param>
        /// <param name="weathers">Weathers</param>
        /// <returns>Created layer</returns>
        public static MapLayer CreateLaeyr(StationData stationData, DrawType2D drawType, LegendScheme aLS, string layerName, List<int> weathers)
        {
            MapLayer aLayer = null;
            switch (drawType)
            {
                case DrawType2D.Weather_Symbol:
                    aLayer = DrawMeteoData.CreateWeatherSymbolLayer(stationData, weathers, aLS, layerName);
                    break;
            }

            return aLayer;
        }

        /// <summary>
        /// Create point layer from text file with lon/lat column - comma dilimit
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="lonIdx">Longitude column index</param>
        /// <param name="latIdx">Latitude column index</param>
        /// <param name="aLS">Legend Scheme</param>
        /// <param name="lName">Layer Name</param>
        /// <returns></returns>
        public static VectorLayer CreateXYLayer(string filePath, int lonIdx, int latIdx, LegendScheme aLS, string lName)
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            aLayer.LayerDrawType = LayerDrawType.Map;
            aLayer.LayerName = lName;
            aLayer.LegendScheme = aLS;
            aLayer.Visible = true;

            double lon, lat;

            StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Default);
            string[] dataArray;
            string aLine = sr.ReadLine();    //First line - title
            //Get field list
            List<string> fieldList = new List<string>();
            dataArray = aLine.Split(',');
            if (dataArray.Length < 3)
            {
                return null;
            }
            fieldList = new List<string>(dataArray.Length);
            fieldList.AddRange(dataArray);

            //Judge field type
            List<string> varList = new List<string>();
            aLine = sr.ReadLine();    //First data line
            dataArray = aLine.Split(',');
            for (int i = 3; i < dataArray.Length; i++)
            {
                if (MIMath.IsNumeric(dataArray[i]))
                    varList.Add(fieldList[i]);
            }


            //Add fields
            for (int i = 0; i < fieldList.Count; i++)
            {
                DataColumn aDC = new DataColumn();
                aDC.ColumnName = fieldList[i];
                if (varList.Contains(fieldList[i]))
                    aDC.DataType = typeof(double);
                else
                    aDC.DataType = typeof(string);
                aLayer.EditAddField(aDC);
            }

            //Read data
            //aLine = sr.ReadLine();
            while (aLine != null)
            {
                dataArray = aLine.Split(',');
                if (dataArray.Length < 2)
                {
                    aLine = sr.ReadLine();
                    continue;
                }

                PointD aPoint = new PointD();
                lon = double.Parse(dataArray[lonIdx]);
                lat = double.Parse(dataArray[latIdx]);
                aPoint.X = lon;
                aPoint.Y = lat;

                //Add shape
                PointShape aPS = new PointShape();
                aPS.Point = aPoint;
                int shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPS, shapeNum))
                {
                    //Edit record value
                    for (int j = 0; j < fieldList.Count; j++)
                    {
                        if (varList.Contains(fieldList[j]))
                            aLayer.EditCellValue(fieldList[j], shapeNum, double.Parse(dataArray[j]));
                        else
                            aLayer.EditCellValue(fieldList[j], shapeNum, dataArray[j]);
                    }
                }

                aLine = sr.ReadLine();
            }

            return aLayer;
        }

        /// <summary>
        /// Create station point layer
        /// </summary>
        /// <param name="DiscreteData"></param>
        /// <param name="aLS"></param>
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateSTPointLayer(double[,] DiscreteData, LegendScheme aLS, string LName)
        {
            ArrayList stPoints = new ArrayList();
            
            int i;
            PointD aPoint;
            
            ArrayList PList = new ArrayList();
            string columnName = LName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = columnName;
            aDC.DataType = typeof(double);
            aLayer.EditAddField(aDC);
            
            for (i = 0; i < DiscreteData.GetLength(1); i++)
            {
                aPoint = new PointD();
                aPoint.X = DiscreteData[0, i];
                aPoint.Y = DiscreteData[1, i];
                PointShape aPointShape = new PointShape();
                aPointShape.Point = aPoint;
                aPointShape.Value = DiscreteData[2, i];
                
                int shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPointShape, shapeNum))
                    aLayer.EditCellValue(columnName, shapeNum, DiscreteData[2, i]);                
            }
            
            aLayer.LayerName = LName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.StationPoint;                        
            
            return aLayer;
        }

        /// <summary>
        /// Create station point layer
        /// </summary>
        /// <param name="stationData">station data</param>
        /// <param name="aLS">legend scheme</param>
        /// <param name="LName">layer name</param>
        /// <param name="fieldName">field name</param>
        /// <returns>vector layer</returns>
        public static VectorLayer CreateSTPointLayer(StationData stationData, LegendScheme aLS, string LName, string fieldName)
        {
            ArrayList stPoints = new ArrayList();

            int i;
            PointD aPoint;

            ArrayList PList = new ArrayList();
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            aLayer.EditAddField("Stid", typeof(string));
            aLayer.EditAddField(fieldName, typeof(double));

            for (i = 0; i < stationData.Data.GetLength(1); i++)
            {
                aPoint = new PointD();
                aPoint.X = stationData.Data[0, i];
                aPoint.Y = stationData.Data[1, i];
                PointShape aPointShape = new PointShape();
                aPointShape.Point = aPoint;
                aPointShape.Value = stationData.Data[2, i];

                int shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPointShape, shapeNum))
                {
                    aLayer.EditCellValue("Stid", shapeNum, stationData.Stations[i]);
                    aLayer.EditCellValue(fieldName, shapeNum, stationData.Data[2, i]);
                }
            }

            aLayer.LayerName = LName;
            aLS.FieldName = fieldName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.StationPoint;

            return aLayer;
        }

        /// <summary>
        /// Create station info layer
        /// </summary>
        /// <param name="stInfoData">Station Info data</param>
        /// <param name="aLS">Legend scheme</param>
        /// <param name="LName">Layer nmae</param>
        /// <returns>Station info vector layre</returns>
        public static VectorLayer CreateSTInfoLayer(StationInfoData stInfoData, LegendScheme aLS, string LName)
        {
            return CreateSTInfoLayer(stInfoData.Fields, stInfoData.Variables, stInfoData.DataList, aLS, LName);
        }

        /// <summary>
        /// Create station info layer
        /// </summary>
        /// <param name="fieldList">field list</param>
        /// <param name="varList">variable list</param>
        /// <param name="DiscreteData">data list</param>
        /// <param name="aLS">legend scheme</param>
        /// <param name="LName">layer name</param>
        /// <returns>vector layer</returns>
        public static VectorLayer CreateSTInfoLayer(List<string> fieldList, List<string> varList, 
            List<List<string>> DiscreteData, LegendScheme aLS, string LName)
        {
            ArrayList stPoints = new ArrayList();

            int i, j;
            PointD aPoint;

            ArrayList PList = new ArrayList();            
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            for (i = 0; i < fieldList.Count; i++)
            {                
                DataColumn aDC = new DataColumn();
                aDC.ColumnName = fieldList[i];
                if (varList.Contains(fieldList[i]))
                    aDC.DataType = typeof(double);
                else
                    aDC.DataType = typeof(string);
                aLayer.EditAddField(aDC);
            }

            for (i = 0; i < DiscreteData.Count; i++)
            {
                List<string> dataList = DiscreteData[i];
                aPoint = new PointD();
                aPoint.X = double.Parse(dataList[1]);
                aPoint.Y = double.Parse(dataList[2]);
                PointShape aPointShape = new PointShape();
                aPointShape.Point = aPoint;                

                int shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPointShape, shapeNum))
                {
                    for (j = 0; j < fieldList.Count; j++)
                    {
                        if (varList.Contains(fieldList[j]))
                            aLayer.EditCellValue(fieldList[j], shapeNum, double.Parse(dataList[j]));
                        else
                            aLayer.EditCellValue(fieldList[j], shapeNum, dataList[j]);
                    }
                }
            }

            aLayer.LayerName = LName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.StationPoint;

            return aLayer;
        }

        /// <summary>
        /// Create grid point layer
        /// </summary>
        /// <param name="GridData"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="aLS"></param>
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateGridPointLayer(double[,] GridData, double[] X, double[] Y,
            LegendScheme aLS, string LName)
        {            
            //generate grid points
            int i, j;
            PointD aPoint;
                       
            string columnName = LName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = columnName;
            aDC.DataType = typeof(double);
            aLayer.EditAddField(aDC);
            
            for (i = 0; i < GridData.GetLength(0); i++)
            {
                for (j = 0; j < GridData.GetLength(1); j++)
                {
                    aPoint = new PointD();
                    aPoint.X = X[j];
                    aPoint.Y = Y[i];
                    PointShape aPointShape = new PointShape(); 
                    aPointShape.Point = aPoint;
                    aPointShape.Value = GridData[i, j];

                    int shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aPointShape, shapeNum))
                        aLayer.EditCellValue(columnName, shapeNum, GridData[i, j]);                    
                }
            }              
                       
            aLayer.LayerName = LName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.GridPoint;                        

            return aLayer;
        }

        /// <summary>
        /// Create grid point layer
        /// </summary>
        /// <param name="gridData">grid data</param>        
        /// <param name="aLS">legend scheme</param>
        /// <param name="LName">layer name</param>
        /// <param name="fieldName">field name</param>
        /// <returns>VectorLayer</returns>
        public static VectorLayer CreateGridPointLayer(GridData gridData, LegendScheme aLS, string LName, string fieldName)
        {
            //generate grid points
            int i, j;
            PointD aPoint;

            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = fieldName;
            aDC.DataType = typeof(double);
            aLayer.EditAddField(aDC);

            for (i = 0; i < gridData.YNum; i++)
            {
                for (j = 0; j < gridData.XNum; j++)
                {
                    aPoint = new PointD();
                    aPoint.X = gridData.X[j];
                    aPoint.Y = gridData.Y[i];
                    PointShape aPointShape = new PointShape();
                    aPointShape.Point = aPoint;
                    aPointShape.Value = gridData.Data[i, j];

                    int shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aPointShape, shapeNum))
                        aLayer.EditCellValue(fieldName, shapeNum, gridData.Data[i, j]);
                }
            }

            aLayer.LayerName = LName;
            aLS.FieldName = fieldName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.GridPoint;

            return aLayer;
        }

        /// <summary>
        /// Create grid fill layer
        /// </summary>
        /// <param name="GridData"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="XDelt"></param>
        /// <param name="YDelt"></param>
        /// <param name="aLS"></param>
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateGridFillLayer(double[,] GridData, double[] X, double[] Y,
            double XDelt, double YDelt, LegendScheme aLS, string LName)
        {            
            //generate grid points
            int i, j;
            PointD aPoint;

            List<PointD> PList = new List<PointD>();                         
            string columnName = LName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polygon);
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = columnName;
            aDC.DataType = typeof(double);
            aLayer.EditAddField(aDC);
            
            for (i = 0; i < GridData.GetLength(0); i++)
            {
                for (j = 0; j < GridData.GetLength(1); j++)
                {
                    PList = new List<PointD>();
                    aPoint = new PointD();
                    aPoint.X = X[j] - XDelt / 2;
                    aPoint.Y = Y[i] - YDelt / 2;
                    PList.Add(aPoint);
                    aPoint = new PointD();
                    aPoint.X = X[j] - XDelt / 2;
                    aPoint.Y = Y[i] + YDelt / 2;
                    PList.Add(aPoint);
                    aPoint = new PointD();
                    aPoint.X = X[j] + XDelt / 2;
                    aPoint.Y = Y[i] + YDelt / 2;
                    PList.Add(aPoint);
                    aPoint = new PointD();
                    aPoint.X = X[j] + XDelt / 2;
                    aPoint.Y = Y[i] - YDelt / 2;
                    PList.Add(aPoint);
                    PList.Add(PList[0]);

                    PolygonShape aPGS = new PolygonShape();
                    aPGS.lowValue = GridData[i, j];
                    aPGS.highValue = aPGS.lowValue;
                    aPGS.Points = PList;
                    aPGS.Extent = MIMath.GetPointsExtent(PList);                                        

                    int shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aPGS, shapeNum))
                        aLayer.EditCellValue(columnName, shapeNum, GridData[i, j]);                    
                }
            }            
                                    
            aLayer.LayerName = LName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.GridFill;                        

            return aLayer;
        }

        /// <summary>
        /// Create grid fill layer
        /// </summary>
        /// <param name="gridData">grid data</param>        
        /// <param name="aLS">legend scheme</param>
        /// <param name="LName">layer name</param>
        /// <param name="fieldName">field name</param>
        /// <returns>VectorLayer</returns>
        public static VectorLayer CreateGridFillLayer(GridData gridData, LegendScheme aLS, string LName, string fieldName)
        {
            //generate grid points
            int i, j;
            PointD aPoint;

            List<PointD> PList = new List<PointD>();
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polygon);
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = fieldName;
            aDC.DataType = typeof(double);
            aLayer.EditAddField(aDC);

            double XDelt = gridData.X[1] - gridData.X[0];
            double YDelt = gridData.Y[1] - gridData.Y[0];
            for (i = 0; i < gridData.YNum; i++)
            {
                for (j = 0; j < gridData.XNum; j++)
                {
                    PList = new List<PointD>();
                    aPoint = new PointD();
                    aPoint.X = gridData.X[j] - XDelt / 2;
                    aPoint.Y = gridData.Y[i] - YDelt / 2;
                    PList.Add(aPoint);
                    aPoint = new PointD();
                    aPoint.X = gridData.X[j] - XDelt / 2;
                    aPoint.Y = gridData.Y[i] + YDelt / 2;
                    PList.Add(aPoint);
                    aPoint = new PointD();
                    aPoint.X = gridData.X[j] + XDelt / 2;
                    aPoint.Y = gridData.Y[i] + YDelt / 2;
                    PList.Add(aPoint);
                    aPoint = new PointD();
                    aPoint.X = gridData.X[j] + XDelt / 2;
                    aPoint.Y = gridData.Y[i] - YDelt / 2;
                    PList.Add(aPoint);
                    PList.Add(PList[0]);

                    PolygonShape aPGS = new PolygonShape();
                    aPGS.lowValue = gridData.Data[i, j];
                    aPGS.highValue = aPGS.lowValue;
                    aPGS.Points = PList;

                    int shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aPGS, shapeNum))
                        aLayer.EditCellValue(fieldName, shapeNum, gridData.Data[i, j]);
                }
            }

            aLayer.LayerName = LName;
            aLS.FieldName = fieldName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.GridFill;

            return aLayer;
        }

        /// <summary>
        /// Create contour layer
        /// </summary>
        /// <param name="GridData"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="CValues"></param>
        /// <param name="hasNoData"></param>
        /// <param name="MissingValue"></param>
        /// <param name="aLS"></param>
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateContourLayer(double[,] GridData, double[] X, double[] Y, 
            double[] CValues, Boolean hasNoData, double MissingValue, LegendScheme aLS, string LName)
        {
            List<wContour.PolyLine> ContourLines = new List<wContour.PolyLine>();

            List<wContour.Border> Borders = new List<wContour.Border>();
            ContourLines = ContourDraw.TracingContourLines(GridData,
                CValues, X, Y, MissingValue, ref Borders);
            ContourLines = wContour.Contour.SmoothLines(ContourLines);
                        
            wContour.PolyLine aLine;            
            double aValue;
            string columnName = LName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
            DataColumn aDC = new DataColumn(columnName, typeof(double));                     
            aLayer.EditAddField(aDC);
            
            for (int i = 0; i < ContourLines.Count; i++)
            {
                aLine = (wContour.PolyLine)ContourLines[i];
                aValue = aLine.Value;

                PolylineShape aPolyline = new PolylineShape();
                PointD aPoint = new PointD();
                List<PointD> pList = new List<PointD>();
                for (int j = 0; j < aLine.PointList.Count; j++)
                {
                    aPoint = new PointD();
                    aPoint.X = ((wContour.PointD)aLine.PointList[j]).X;
                    aPoint.Y = ((wContour.PointD)aLine.PointList[j]).Y;
                    pList.Add(aPoint);
                }
                aPolyline.Points = pList;
                aPolyline.value = aValue;
                aPolyline.Extent = MIMath.GetPointsExtent(pList);                                               

                int shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPolyline, shapeNum))
                {                    
                    aLayer.EditCellValue(columnName, shapeNum, aValue);
                }
            }            
                        
            aLayer.LayerName = LName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();           
            aLayer.LayerDrawType = LayerDrawType.Contour;                        

            aLayer.LabelSet.DrawLabels = true;
            aLayer.LabelSet.DrawShadow = true;
            aLayer.LabelSet.ShadowColor = Color.White;
            aLayer.LabelSet.YOffset = 3;
            aLayer.LabelSet.FieldName = columnName;
            aLayer.LabelSet.ColorByLegend = true;
            aLayer.LabelSet.DynamicContourLabel = true;              
            //aLayer.AddLabels();

            return aLayer;
        }

        /// <summary>
        /// Create contour layer
        /// </summary>
        /// <param name="gridData">grid data</param>        
        /// <param name="aLS">legend scheme</param>
        /// <param name="LName">layer name</param>
        /// <param name="fieldName">field name</param>
        /// <returns>VectorLayer</returns>
        public static VectorLayer CreateContourLayer(GridData gridData, LegendScheme aLS, string LName, string fieldName)
        {
            List<wContour.PolyLine> ContourLines = new List<wContour.PolyLine>();

            double[] cValues = new double[1];
            Color[] colors = new Color[1];
            LegendManage.SetContoursAndColors(aLS, ref cValues, ref colors);

            double minData = 0;
            double maxData = 0;
            bool hasNoData = ContourDraw.GetMaxMinValue(gridData.Data, gridData.MissingValue, ref minData, ref maxData);

            List<wContour.Border> Borders = new List<wContour.Border>();
            ContourLines = ContourDraw.TracingContourLines(gridData.Data,
                cValues, gridData.X, gridData.Y, gridData.MissingValue, ref Borders); 

            if (ContourLines.Count == 0)
                return null;

            ContourLines = wContour.Contour.SmoothLines(ContourLines);

            wContour.PolyLine aLine;
            double aValue;
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
            DataColumn aDC = new DataColumn(fieldName, typeof(double));
            aLayer.EditAddField(aDC);

            for (int i = 0; i < ContourLines.Count; i++)
            {
                aLine = (wContour.PolyLine)ContourLines[i];
                aValue = aLine.Value;

                PolylineShape aPolyline = new PolylineShape();
                PointD aPoint = new PointD();
                List<PointD> pList = new List<PointD>();
                for (int j = 0; j < aLine.PointList.Count; j++)
                {
                    aPoint = new PointD();
                    aPoint.X = ((wContour.PointD)aLine.PointList[j]).X;
                    aPoint.Y = ((wContour.PointD)aLine.PointList[j]).Y;
                    pList.Add(aPoint);
                }
                aPolyline.Points = pList;
                aPolyline.value = aValue;
                aPolyline.Extent = MIMath.GetPointsExtent(pList);

                int shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPolyline, shapeNum))
                {
                    aLayer.EditCellValue(fieldName, shapeNum, aValue);
                }
            }

            aLayer.LayerName = LName;
            aLS.FieldName = fieldName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Contour;

            aLayer.LabelSet.DrawLabels = true;
            aLayer.LabelSet.DrawShadow = true;
            aLayer.LabelSet.ShadowColor = Color.White;
            aLayer.LabelSet.YOffset = 3;
            aLayer.LabelSet.FieldName = fieldName;
            aLayer.LabelSet.ColorByLegend = true;
            aLayer.LabelSet.DynamicContourLabel = true;
            //aLayer.AddLabels();

            return aLayer;
        }        

        /// <summary>
        /// Create shaded layer
        /// </summary>
        /// <param name="GridData"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="CValues"></param>
        /// <param name="Colors"></param>
        /// <param name="MaxData"></param>
        /// <param name="MinData"></param>
        /// <param name="hasNoData"></param>
        /// <param name="MissingValue"></param>
        /// <param name="aLS"></param>
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateShadedLayer(double[,] GridData, double[] X, double[] Y,
            double[] CValues, Color[] Colors, double MaxData, double MinData,
            Boolean hasNoData, double MissingValue, LegendScheme aLS, string LName)
        {
            List<wContour.PolyLine> ContourLines = new List<wContour.PolyLine>();
            List<wContour.Polygon> ContourPolygons = new List<wContour.Polygon>();

            List<wContour.Border> Borders = new List<wContour.Border>();
            ContourLines = ContourDraw.TracingContourLines(GridData,
                CValues, X, Y, MissingValue, ref Borders);
            ContourLines = wContour.Contour.SmoothLines(ContourLines);
            ContourPolygons = ContourDraw.TracingPolygons(GridData, ContourLines, Borders,
                X, Y, CValues);
                        
            wContour.Polygon aPolygon;            
            Color aColor;
            double aValue;
            int valueIdx;
            string columnName = LName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polygon);
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = columnName + "_Low";
            aDC.DataType = typeof(double);
            aLayer.EditAddField(aDC);
            aDC = new DataColumn();
            aDC.ColumnName = columnName + "_High";
            aDC.DataType = typeof(double);
            aLayer.EditAddField(aDC);
            
            for (int i = 0; i < ContourPolygons.Count; i++)
            {
                aPolygon = (wContour.Polygon)ContourPolygons[i];
                aValue = aPolygon.LowValue;

                PointD aPoint = new PointD();
                List<PointD> pList = new List<PointD>();
                for (int j = 0; j < aPolygon.OutLine.PointList.Count; j++)
                {
                    aPoint = new PointD();
                    aPoint.X = ((wContour.PointD)aPolygon.OutLine.PointList[j]).X;
                    aPoint.Y = ((wContour.PointD)aPolygon.OutLine.PointList[j]).Y;
                    pList.Add(aPoint);
                }

                PolygonShape aPolygonShape = new PolygonShape();
                aPolygonShape.Points = pList;
                aPolygonShape.lowValue = aValue;                
                
                valueIdx = Array.IndexOf(CValues, aValue);
                if (valueIdx == CValues.Length - 1)
                {
                    aPolygonShape.highValue = MaxData;
                }
                else
                {
                    aPolygonShape.highValue = CValues[valueIdx + 1];
                }
                aColor = Colors[Array.IndexOf(CValues, aValue) + 1];
                if (!aPolygon.IsHighCenter)
                {
                    for (int j = 0; j < Colors.Length; j++)
                    {
                        if (aColor == Colors[j] && j > 0)
                        {
                            aColor = Colors[j - 1];
                            break;
                        }
                    }
                    aPolygonShape.highValue = aValue;
                    if (valueIdx == 0)
                    {
                        aPolygonShape.lowValue = MinData;
                    }
                    else
                    {
                        aPolygonShape.lowValue = CValues[valueIdx - 1];
                    }
                }

                int shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPolygonShape, shapeNum))
                {
                    aLayer.EditCellValue(columnName + "_Low", shapeNum, aPolygonShape.lowValue);
                    aLayer.EditCellValue(columnName + "_High", shapeNum, aPolygonShape.highValue);
                }
            }
            
            aLayer.LayerName = LName;
            aLS.FieldName = columnName + "_Low";
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Shaded;                        

            return aLayer;
        }

        /// <summary>
        /// Create shaded layer
        /// </summary>
        /// <param name="gridData">grid data</param>        
        /// <param name="aLS">legend scheme</param>
        /// <param name="LName">layer name</param>
        /// <param name="fieldName">field name</param>
        /// <returns>vector layer</returns>
        public static VectorLayer CreateShadedLayer(GridData gridData, LegendScheme aLS, string LName, string fieldName)
        {
            List<wContour.PolyLine> ContourLines = new List<wContour.PolyLine>();
            List<wContour.Polygon> ContourPolygons = new List<wContour.Polygon>();

            double[] cValues = new double[1];
            Color[] colors = new Color[1];
            LegendManage.SetContoursAndColors(aLS, ref cValues, ref colors);

            double minData = 0;
            double maxData = 0;
            bool hasNoData = ContourDraw.GetMaxMinValue(gridData.Data, gridData.MissingValue, ref minData, ref maxData);

            List<wContour.Border> Borders = new List<wContour.Border>();
            ContourLines = ContourDraw.TracingContourLines(gridData.Data,
                cValues, gridData.X, gridData.Y, gridData.MissingValue, ref Borders);      
       

            ContourLines = wContour.Contour.SmoothLines(ContourLines);
            ContourPolygons = ContourDraw.TracingPolygons(gridData.Data, ContourLines, Borders,
                gridData.X, gridData.Y, cValues);

            //if (hasNoData)
            //{
            //    List<wContour.Border> Borders = new List<wContour.Border>();
            //    ContourLines = ContourDraw.TracingContourLines_Obstacle(gridData.Data,
            //        cValues, gridData.X, gridData.Y, gridData.MissingValue, ref Borders);
            //    ContourLines = wContour.Contour.SmoothLines(ContourLines);
            //    ContourPolygons = ContourDraw.TracingPolygons_Obstacle(gridData.Data, ContourLines, Borders,
            //        gridData.X, gridData.Y, cValues);
            //}
            //else
            //{
            //    ContourLines = ContourDraw.TracingContourLines(gridData.Data, cValues, gridData.X, gridData.Y);
            //    ContourLines = wContour.Contour.SmoothLines(ContourLines);
            //    //m_ContourLines = (ArrayList)m_Contour.CutIsolineWithPolygon(m_ContourLines, m_CutPList);
            //    //m_IsCutted = true;
            //    ContourPolygons = ContourDraw.TracingPolygons(ContourLines, false, gridData.X, gridData.Y, cValues);
            //}

            wContour.Polygon aPolygon;
            //Color aColor;
            double aValue;
            int valueIdx;
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polygon);
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = fieldName + "_Low";
            aDC.DataType = typeof(double);
            aLayer.EditAddField(aDC);
            aDC = new DataColumn();
            aDC.ColumnName = fieldName + "_High";
            aDC.DataType = typeof(double);
            aLayer.EditAddField(aDC);

            for (int i = 0; i < ContourPolygons.Count; i++)
            {
                aPolygon = (wContour.Polygon)ContourPolygons[i];
                aValue = aPolygon.LowValue;

                PointD aPoint = new PointD();
                List<PointD> pList = new List<PointD>();
                for (int j = 0; j < aPolygon.OutLine.PointList.Count; j++)
                {
                    aPoint = new PointD();
                    aPoint.X = ((wContour.PointD)aPolygon.OutLine.PointList[j]).X;
                    aPoint.Y = ((wContour.PointD)aPolygon.OutLine.PointList[j]).Y;
                    pList.Add(aPoint);
                }
                if (!GeoComputation.IsClockwise(pList))
                    pList.Reverse();

                PolygonShape aPolygonShape = new PolygonShape();
                aPolygonShape.Points = pList;
                aPolygonShape.lowValue = aValue;
                if (aPolygon.HasHoles)
                {
                    for (int h = 0; h < aPolygon.HoleLines.Count; h++)
                    {
                        pList = new List<PointD>();
                        for (int j = 0; j < aPolygon.HoleLines[h].PointList.Count; j++)
                        {
                            aPoint = new PointD();
                            aPoint.X = ((wContour.PointD)aPolygon.HoleLines[h].PointList[j]).X;
                            aPoint.Y = ((wContour.PointD)aPolygon.HoleLines[h].PointList[j]).Y;
                            pList.Add(aPoint);
                        }
                        aPolygonShape.AddHole(pList, 0);
                    }
                }

                valueIdx = Array.IndexOf(cValues, aValue);
                if (valueIdx == cValues.Length - 1)
                {
                    aPolygonShape.highValue = maxData;
                }
                else
                {
                    aPolygonShape.highValue = cValues[valueIdx + 1];
                }
                //aColor = colors[Array.IndexOf(cValues, aValue) + 1];
                if (!aPolygon.IsHighCenter)
                {
                    //for (int j = 0; j < colors.Length; j++)
                    //{
                    //    if (aColor == colors[j] && j > 0)
                    //    {
                    //        aColor = colors[j - 1];
                    //        break;
                    //    }
                    //}
                    aPolygonShape.highValue = aValue;
                    if (valueIdx == 0)
                    {
                        aPolygonShape.lowValue = minData;
                    }
                    else
                    {
                        aPolygonShape.lowValue = cValues[valueIdx - 1];
                    }
                }

                int shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPolygonShape, shapeNum))
                {
                    aLayer.EditCellValue(fieldName + "_Low", shapeNum, aPolygonShape.lowValue);
                    aLayer.EditCellValue(fieldName + "_High", shapeNum, aPolygonShape.highValue);
                }
            }

            aLayer.LayerName = LName;
            aLS.FieldName = fieldName + "_Low";
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Shaded;

            return aLayer;
        }

        /// <summary>
        /// Create grid wind vector layer from U/V or direction/speed grid data
        /// </summary>
        /// <param name="uData"></param>
        /// <param name="vData"></param>
        /// <param name="gridData"></param>
        /// <param name="aLS"></param>
        /// <param name="ifColor"></param>
        /// <param name="lName"></param>
        /// <param name="isUV"></param>
        /// <returns></returns>
        public static VectorLayer CreateGridVectorLayer(GridData uData, GridData vData, GridData gridData,
            LegendScheme aLS, bool ifColor, string lName, bool isUV)
        {
            GridData windDirData = new GridData();
            GridData windSpeedData = new GridData();
            if (isUV)
            {
                DataMath.GetDSFromUV(uData, vData, ref windDirData, ref windSpeedData);
            }
            else
            {
                windDirData = uData;
                windSpeedData = vData;
            }

            int i, j;
            double windDir, windSpeed;
            Single size = 6;
            PointD aPoint = new PointD();
            int XNum = uData.X.Length;
            int YNum = uData.Y.Length;

            string columnName = lName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindArraw);
            //Add data column   
            if (isUV)
            {
                aLayer.EditAddField("U", typeof(Single));
                aLayer.EditAddField("V", typeof(Single));
            }
            aLayer.EditAddField("WindDirection", typeof(Single));
            aLayer.EditAddField("WindSpeed", typeof(Single));
            bool ifAdd = true;
            if (aLayer.GetFieldNameList().Contains(columnName))
            {
                ifAdd = false;
            }
            if (ifColor && ifAdd)
            {
                aLayer.EditAddField(columnName, typeof(Single));
            }

            for (i = 0; i < YNum; i++)
            {
                for (j = 0; j < XNum; j++)
                {
                    windDir = windDirData.Data[i, j];
                    windSpeed = windSpeedData.Data[i, j];
                    if (!MIMath.DoubleEquals(windDir, uData.MissingValue))
                    {
                        if (!MIMath.DoubleEquals(windSpeed, uData.MissingValue))
                        {
                            aPoint = new PointD();
                            aPoint.X = uData.X[j];
                            aPoint.Y = uData.Y[i];
                            WindArraw aArraw = new WindArraw();
                            aArraw.angle = windDir;
                            aArraw.length = (float)windSpeed;
                            aArraw.size = size;
                            aArraw.Point = aPoint;

                            if (ifColor)
                            {
                                aArraw.Value = gridData.Data[i, j];
                            }

                            int shapeNum = aLayer.ShapeNum;
                            if (aLayer.EditInsertShape(aArraw, shapeNum))
                            {
                                if (isUV)
                                {
                                    aLayer.EditCellValue("U", shapeNum, uData.Data[i, j]);
                                    aLayer.EditCellValue("V", shapeNum, vData.Data[i, j]);
                                }
                                aLayer.EditCellValue("WindDirection", shapeNum, aArraw.angle);
                                aLayer.EditCellValue("WindSpeed", shapeNum, aArraw.length);
                                if (ifColor && ifAdd)
                                    aLayer.EditCellValue(columnName, shapeNum, gridData.Data[i, j]);
                            }
                        }
                    }

                }
            }

            aLayer.LayerName = lName;
            if (ifColor && ifAdd)
                aLS.FieldName = columnName;
            else
                aLS.FieldName = "WindSpeed";
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Vector;

            return aLayer;
        }
        
        /// <summary>
        /// Create wind vector layer from grid data
        /// </summary>
        /// <param name="Udata"></param>
        /// <param name="Vdata"></param>
        /// <param name="GridData"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="MissingValue"></param>
        /// <param name="aLS"></param>
        /// <param name="IfColor"></param>
        /// <param name="LName"></param>
        /// <param name="isUV"></param>
        /// <returns></returns>
        public static VectorLayer CreateGridVectorLayer(double[,] Udata, double[,] Vdata, double[,] GridData,
            double[] X, double[] Y, double MissingValue, LegendScheme aLS, bool IfColor, string LName, bool isUV)
        {
            if (isUV)
                return CreateGridVectorLayer_UV(Udata, Vdata, GridData, X, Y, MissingValue, aLS, IfColor, LName);
            else
                return CreateGridVectorLayer_DS(Udata, Vdata, GridData, X, Y, MissingValue, aLS, IfColor, LName);
        }

        /// <summary>
        /// Create vector layer
        /// </summary>
        /// <param name="Udata"></param>
        /// <param name="Vdata"></param>
        /// <param name="GridData"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="MissingValue"></param>
        /// <param name="aLS"></param>
        /// <param name="IfColor"></param>
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateGridVectorLayer_UV(double[,] Udata, double[,] Vdata, double[,] GridData,
            double[] X, double[] Y, double MissingValue, LegendScheme aLS, bool IfColor, string LName)
        {            
            int i;             
            int j;
            
            double U, V;
            Single size = 6;
            PointD aPoint = new PointD();          
            int XNum = X.Length;
            int YNum = Y.Length;

            string columnName = LName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindArraw);
            //Add data column            
            aLayer.EditAddField("U",typeof(Single));            
            aLayer.EditAddField("V",typeof(Single));
            aLayer.EditAddField("Angle", typeof(Single));
            aLayer.EditAddField("Speed", typeof(Single));
            bool ifAdd = true;
            if (columnName == "U" || columnName == "V")
            {
                ifAdd = false;
            }
            if (IfColor && ifAdd)
            {                
                aLayer.EditAddField(columnName,typeof(Single));
            }
            
            for (i = 0; i < YNum; i++)
            {
                for (j = 0; j < XNum; j++)
                {
                    U = Udata[i, j];
                    V = Vdata[i, j];
                    if (!(Math.Abs(U / MissingValue - 1) < 0.01))
                    {
                        if (!(Math.Abs(V / MissingValue - 1) < 0.01))
                        {
                            aPoint = new PointD();
                            aPoint.X = X[j];
                            aPoint.Y = Y[i];
                            WindArraw aArraw = new WindArraw();
                            aArraw = Draw.CalArraw(U, V, 0, size, aPoint);
                           
                            if (IfColor)
                            {
                                aArraw.Value = GridData[i, j];                                
                            }

                            int shapeNum = aLayer.ShapeNum;
                            if (aLayer.EditInsertShape(aArraw, shapeNum))
                            {
                                aLayer.EditCellValue("U", shapeNum, U);
                                aLayer.EditCellValue("V", shapeNum, V);
                                aLayer.EditCellValue("Angle", shapeNum, aArraw.angle);
                                aLayer.EditCellValue("Speed", shapeNum, aArraw.length);
                                if (IfColor && ifAdd)
                                    aLayer.EditCellValue(columnName, shapeNum, GridData[i, j]);
                            }                            
                        }
                    }

                }
            }
            
            aLayer.LayerName = LName;
            if (IfColor && ifAdd)
                aLS.FieldName = columnName;
            else
                aLS.FieldName = "Speed";
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Vector;                        

            return aLayer;    
        }

        /// <summary>
        /// Create vector layer from grid data (direction/speed)
        /// </summary>
        /// <param name="windDirData"></param>
        /// <param name="windSpeedData"></param>
        /// <param name="GridData"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="MissingValue"></param>
        /// <param name="aLS"></param>
        /// <param name="IfColor"></param>
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateGridVectorLayer_DS(double[,] windDirData, double[,] windSpeedData, double[,] GridData,
            double[] X, double[] Y, double MissingValue, LegendScheme aLS, bool IfColor, string LName)
        {
            int i;
            int j;

            double windDir, windSpeed;
            Single size = 6;
            PointD aPoint = new PointD();
            int XNum = X.Length;
            int YNum = Y.Length;

            string columnName = LName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindArraw);
            //Add data column                        
            aLayer.EditAddField("WindDirection", typeof(Single));
            aLayer.EditAddField("WindSpeed", typeof(Single));
            bool ifAdd = true;
            if (columnName == "WindDirection" || columnName == "WindSpeed")
            {
                ifAdd = false;
            }
            if (IfColor && ifAdd)
            {
                aLayer.EditAddField(columnName, typeof(Single));
            }

            for (i = 0; i < YNum; i++)
            {
                for (j = 0; j < XNum; j++)
                {
                    windDir = windDirData[i, j];
                    windSpeed = windSpeedData[i, j];
                    if (!MIMath.DoubleEquals(windDir, MissingValue))
                    {
                        if (!MIMath.DoubleEquals(windSpeed, MissingValue))
                        {
                            aPoint = new PointD();
                            aPoint.X = X[j];
                            aPoint.Y = Y[i];
                            WindArraw aArraw = new WindArraw();
                            aArraw.angle = windDir;
                            aArraw.length = (float)windSpeed;
                            aArraw.size = size;
                            aArraw.Point = aPoint;
                            
                            if (IfColor)
                            {
                                aArraw.Value = GridData[i, j];
                            }

                            int shapeNum = aLayer.ShapeNum;
                            if (aLayer.EditInsertShape(aArraw, shapeNum))
                            {                                
                                aLayer.EditCellValue("WindDirection", shapeNum, aArraw.angle);
                                aLayer.EditCellValue("WindSpeed", shapeNum, aArraw.length);
                                if (IfColor && ifAdd)
                                    aLayer.EditCellValue(columnName, shapeNum, GridData[i, j]);
                            }
                        }
                    }

                }
            }

            aLayer.LayerName = LName;
            if (IfColor && ifAdd)
                aLS.FieldName = columnName;
            else
                aLS.FieldName = "WindSpeed";
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Vector;

            return aLayer;
        }

        /// <summary>
        /// Create streamline layer by U/V or wind direction/speed grid data
        /// </summary>
        /// <param name="uData"></param>
        /// <param name="vData"></param>
        /// <param name="density"></param>
        /// <param name="aLS"></param>
        /// <param name="lName"></param>
        /// <param name="isUV"></param>
        /// <returns></returns>
        public static VectorLayer CreateStreamlineLayer(GridData uData, GridData vData, int density, LegendScheme aLS,
            string lName, bool isUV)
        {
            GridData uGridData = new GridData();
            GridData vGridData = new GridData();
            if (isUV)
            {
                uGridData = uData;
                vGridData = vData;
            }
            else
            {
                DataMath.GetUVFromDS(uData, vData, ref uGridData, ref vGridData);
            }

            List<wContour.PolyLine> streamlines = new List<wContour.PolyLine>();
            streamlines = wContour.Contour.TracingStreamline(uGridData.Data, vGridData.Data, 
                uGridData.X, vGridData.Y, uGridData.MissingValue, density);

            wContour.PolyLine aLine;
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
            DataColumn aDC = new DataColumn("ID", typeof(int));
            aLayer.EditAddField(aDC);

            for (int i = 0; i < streamlines.Count - 1; i++)
            {
                aLine = streamlines[i];

                PolylineShape aPolyline = new PolylineShape();
                PointD aPoint = new PointD();
                List<PointD> pList = new List<PointD>();
                for (int j = 0; j < aLine.PointList.Count; j++)
                {
                    aPoint = new PointD();
                    aPoint.X = (aLine.PointList[j]).X;
                    aPoint.Y = (aLine.PointList[j]).Y;
                    pList.Add(aPoint);
                }
                aPolyline.Points = pList;
                aPolyline.value = density;
                aPolyline.Extent = MIMath.GetPointsExtent(pList);

                int shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPolyline, shapeNum))
                {
                    aLayer.EditCellValue("ID", shapeNum, shapeNum + 1);
                }
            }

            aLayer.LayerName = lName;
            aLS.FieldName = "ID";
            aLayer.LegendScheme = aLS;
            aLayer.LayerDrawType = LayerDrawType.Streamline;

            return aLayer;
        }

        /// <summary>
        /// Create Streamline layer
        /// </summary>
        /// <param name="Udata">U component array</param>
        /// <param name="Vdata">V component array</param>       
        /// <param name="X">X coordinate array</param>
        /// <param name="Y">Y coordinate array</param>
        /// <param name="MissingValue">undefine data</param>
        /// <param name="density">streamline density</param>
        /// <param name="aLS">legend scheme</param>        
        /// <param name="LName">layer name</param>
        /// <returns>retult layer</returns>
        public static VectorLayer CreateStreamlineLayer(double[,] Udata, double[,] Vdata,
            double[] X, double[] Y, double MissingValue, int density, LegendScheme aLS, string LName)
        {
            List<wContour.PolyLine> streamlines = new List<wContour.PolyLine>();
            streamlines = wContour.Contour.TracingStreamline(Udata, Vdata, X, Y, MissingValue, density);            

            wContour.PolyLine aLine;                  
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
            DataColumn aDC = new DataColumn("ID", typeof(int));
            aLayer.EditAddField(aDC);

            for (int i = 0; i < streamlines.Count - 1; i++)
            {
                aLine = streamlines[i];                

                PolylineShape aPolyline = new PolylineShape();
                PointD aPoint = new PointD();
                List<PointD> pList = new List<PointD>();
                for (int j = 0; j < aLine.PointList.Count; j++)
                {
                    aPoint = new PointD();
                    aPoint.X = (aLine.PointList[j]).X;
                    aPoint.Y = (aLine.PointList[j]).Y;
                    pList.Add(aPoint);
                }
                aPolyline.Points = pList;
                aPolyline.value = density;
                aPolyline.Extent = MIMath.GetPointsExtent(pList);

                int shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPolyline, shapeNum))
                {
                    aLayer.EditCellValue("ID", shapeNum, shapeNum + 1);
                }
            }

            aLayer.LayerName = LName;
            aLS.FieldName = "ID";
            aLayer.LegendScheme = aLS;
            aLayer.LayerDrawType = LayerDrawType.Streamline;            

            return aLayer;
        }

        /// <summary>
        /// Create grid barb layer from U/V or wind direction/speed grid data
        /// </summary>
        /// <param name="uData"></param>
        /// <param name="vData"></param>
        /// <param name="gridData"></param>
        /// <param name="aLS"></param>
        /// <param name="ifColor"></param>
        /// <param name="lName"></param>
        /// <param name="isUV"></param>
        /// <returns></returns>
        public static VectorLayer CreateGridBarbLayer(GridData uData, GridData vData, GridData gridData,
            LegendScheme aLS, bool ifColor, string lName, bool isUV)
        {
            GridData windDirData = new GridData();
            GridData windSpeedData = new GridData();
            if (isUV)
            {
                DataMath.GetDSFromUV(uData, vData, ref windDirData, ref windSpeedData);
            }
            else
            {
                windDirData = uData;
                windSpeedData = vData;
            }
            
            int i, j;
            WindBarb aWB = new WindBarb();
            double windDir, windSpeed;
            //Single size = 6;
            PointD aPoint = new PointD();
            int XNum = windDirData.X.Length;
            int YNum = windDirData.Y.Length;
            string columnName = lName.Split('_')[0];

            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindBarb);
            //Add data column  
            if (isUV)
            {
                aLayer.EditAddField("U", typeof(Single));
                aLayer.EditAddField("V", typeof(Single));
            }
            aLayer.EditAddField("WindDirection", typeof(Single));
            aLayer.EditAddField("WindSpeed", typeof(Single));
            bool ifAdd = true;
            if (aLayer.GetFieldNameList().Contains(columnName))
            {
                ifAdd = false;
            }
            if (ifColor && ifAdd)
            {
                aLayer.EditAddField(columnName, typeof(Single));
            }

            for (i = 0; i < YNum; i++)
            {
                for (j = 0; j < XNum; j++)
                {
                    windDir = windDirData.Data[i, j];
                    windSpeed = windSpeedData.Data[i, j];
                    if (!MIMath.DoubleEquals(windDir, windDirData.MissingValue))
                    {
                        if (!MIMath.DoubleEquals(windSpeed, windSpeedData.MissingValue))
                        {
                            aPoint = new PointD();
                            aPoint.X = (Single)windDirData.X[j];
                            aPoint.Y = (Single)windDirData.Y[i];
                            aWB = Draw.CalWindBarb((float)windDir, (float)windSpeed, 0, 10, aPoint);
                            if (ifColor)
                            {
                                aWB.Value = gridData.Data[i, j];
                            }

                            int shapeNum = aLayer.ShapeNum;
                            if (aLayer.EditInsertShape(aWB, shapeNum))
                            {
                                if (isUV)
                                {
                                    aLayer.EditCellValue("U", shapeNum, uData.Data[i, j]);
                                    aLayer.EditCellValue("V", shapeNum, vData.Data[i, j]);
                                }
                                aLayer.EditCellValue("WindDirection", shapeNum, aWB.angle);
                                aLayer.EditCellValue("WindSpeed", shapeNum, aWB.windSpeed);
                                if (ifColor && ifAdd)
                                    aLayer.EditCellValue(columnName, shapeNum, gridData.Data[i, j]);
                            }
                        }
                    }

                }
            }

            aLayer.LayerName = lName;
            if (ifColor && ifAdd)
                aLS.FieldName = columnName;
            else
                aLS.FieldName = "WindSpeed";
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Barb;

            return aLayer;
        }

        /// <summary>
        /// Create grid barb layer
        /// </summary>
        /// <param name="Udata"></param>
        /// <param name="Vdata"></param>
        /// <param name="GridData"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="MissingValue"></param>
        /// <param name="aLS"></param>
        /// <param name="IfColor"></param>
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateGridBarbLayer(double[,] Udata, double[,] Vdata, double[,] GridData,
            double[] X, double[] Y, double MissingValue, LegendScheme aLS, bool IfColor, string LName)
        {            
            int i;            
            int j;
            WindBarb aWB = new WindBarb();
            double U, V;
            //Single size = 6;
            PointD aPoint = new PointD();
            int XNum = X.Length;
            int YNum = Y.Length;
            string columnName = LName.Split('_')[0];

            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindBarb);
            //Add data column            
            aLayer.EditAddField("U", typeof(Single));
            aLayer.EditAddField("V", typeof(Single));
            aLayer.EditAddField("Angle", typeof(Single));
            aLayer.EditAddField("Speed", typeof(Single));
            bool ifAdd = true;
            if (columnName == "U" || columnName == "V")
            {
                ifAdd = false;
            }
            if (IfColor && ifAdd)
            {
                aLayer.EditAddField(columnName, typeof(Single));
            }
            
            for (i = 0; i < YNum; i++)
            {
                for (j = 0; j < XNum; j++)
                {
                    U = Udata[i, j];
                    V = Vdata[i, j];
                    if (!(Math.Abs(U / MissingValue - 1) < 0.01))
                    {
                        if (!(Math.Abs(V / MissingValue - 1) < 0.01))
                        {
                            aPoint = new PointD();
                            aPoint.X = (Single)X[j];
                            aPoint.Y = (Single)Y[i];
                            aWB = Draw.CalWindBarbUV(U, V, 0, 10, aPoint);
                            if (IfColor)
                            {
                                aWB.Value = GridData[i, j];
                            }

                            int shapeNum = aLayer.ShapeNum;
                            if (aLayer.EditInsertShape(aWB, shapeNum))
                            {
                                aLayer.EditCellValue("U", shapeNum, U);
                                aLayer.EditCellValue("V", shapeNum, V);
                                aLayer.EditCellValue("Angle", shapeNum, aWB.angle);
                                aLayer.EditCellValue("Speed", shapeNum, aWB.windSpeed);
                                if (IfColor && ifAdd)
                                    aLayer.EditCellValue(columnName, shapeNum, GridData[i, j]);
                            }                              
                        }
                    }

                }
            }
            
            aLayer.LayerName = LName;
            if (IfColor && ifAdd)
                aLS.FieldName = columnName;
            else
                aLS.FieldName = "Speed";
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Barb;                        

            return aLayer;
        }

        /// <summary>
        /// Create station vector layer
        /// </summary>
        /// <param name="uData">U station data</param>
        /// <param name="vData">V station data</param>
        /// <param name="stData">station data</param>
        /// <param name="aLS">legend scheme</param>
        /// <param name="ifColor">if color</param>
        /// <param name="lName">layer name</param>
        /// <param name="isUV">if is U/V</param>
        /// <returns>vector layer</returns>
        public static VectorLayer CreateSTVectorLayer_old(StationData uData, StationData vData, StationData stData,
            LegendScheme aLS, bool ifColor, string lName, bool isUV)
        {
            StationData windDirData = new StationData();
            StationData windSpeedData = new StationData();
            if (isUV)
            {
                DataMath.GetDSFromUV(uData, vData, ref windDirData, ref windSpeedData);
            }
            else
            {
                windDirData = uData;
                windSpeedData = vData;
            }

            int i;
            Single windDir, windSpeed;
            PointD aPoint = new PointD();

            string columnName = lName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindArraw);
            //Add data column         
            if (isUV)
            {
                aLayer.EditAddField("U", typeof(Single));
                aLayer.EditAddField("V", typeof(Single));
            }
            aLayer.EditAddField("WindDirection", typeof(Single));
            aLayer.EditAddField("WindSpeed", typeof(Single));
            bool ifAdd = true;
            if (aLayer.GetFieldNameList().Contains(columnName))
            {
                ifAdd = false;
            }
            if (ifColor && ifAdd)
            {
                aLayer.EditAddField(columnName, typeof(Single));
            }

            aLayer.AttributeTable.Table.BeginLoadData();
            for (i = 0; i < windDirData.StNum; i++)
            {
                windDir = (Single)windDirData.Data[2, i];
                windSpeed = (Single)windSpeedData.Data[2, i];
                if (!MIMath.DoubleEquals(windDir, windDirData.MissingValue))
                {
                    if (!MIMath.DoubleEquals(windSpeed, windSpeedData.MissingValue))
                    {
                        aPoint = new PointD();
                        aPoint.X = windDirData.Data[0, i];
                        aPoint.Y = windDirData.Data[1, i];

                        WindArraw aArraw = new WindArraw();
                        aArraw.angle = windDir;
                        aArraw.length = windSpeed;
                        aArraw.size = 6;
                        aArraw.Point = aPoint;
                        if (ifColor)
                        {
                            aArraw.Value = stData.Data[2, i];
                        }

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aArraw, shapeNum))
                        {
                            if (isUV)
                            {
                                aLayer.EditCellValue("U", shapeNum, uData.Data[2, i]);
                                aLayer.EditCellValue("V", shapeNum, vData.Data[2, i]);
                            }
                            aLayer.EditCellValue("WindDirection", shapeNum, windDir);
                            aLayer.EditCellValue("WindSpeed", shapeNum, windSpeed);
                            if (ifColor && ifAdd)
                                aLayer.EditCellValue(columnName, shapeNum, stData.Data[2, i]);
                        }
                    }
                }
            }

            aLayer.LayerName = lName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Vector;

            return aLayer;
        }

        /// <summary>
        /// Create station vector layer
        /// </summary>
        /// <param name="uData">U station data</param>
        /// <param name="vData">V station data</param>
        /// <param name="stData">station data</param>
        /// <param name="aLS">legend scheme</param>
        /// <param name="lName">layer name</param>
        /// <param name="isUV">if is U/V</param>
        /// <returns>vector layer</returns>
        public static VectorLayer CreateSTVectorLayer(StationData uData, StationData vData, StationData stData,
            LegendScheme aLS, string lName, bool isUV)
        {
            StationData windDirData = new StationData();
            StationData windSpeedData = new StationData();
            if (isUV)
            {
                DataMath.GetDSFromUV(uData, vData, ref windDirData, ref windSpeedData);
            }
            else
            {
                windDirData = uData;
                windSpeedData = vData;
            }

            int i;
            Single windDir, windSpeed;
            PointD aPoint = new PointD();

            string columnName = lName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindArraw);
            //Add data column         
            if (isUV)
            {
                aLayer.EditAddField("U", typeof(Single));
                aLayer.EditAddField("V", typeof(Single));
            }
            aLayer.EditAddField("WindDirection", typeof(Single));
            aLayer.EditAddField("WindSpeed", typeof(Single));
            bool ifAdd = true;
            if (aLayer.GetFieldNameList().Contains(columnName))
            {
                ifAdd = false;
            }
            if (ifAdd)
            {
                aLayer.EditAddField(columnName, typeof(Single));
            }

            aLayer.AttributeTable.Table.BeginLoadData();
            for (i = 0; i < windDirData.StNum; i++)
            {
                windDir = (Single)windDirData.Data[2, i];
                windSpeed = (Single)windSpeedData.Data[2, i];
                if (!MIMath.DoubleEquals(windDir, windDirData.MissingValue))
                {
                    if (!MIMath.DoubleEquals(windSpeed, windSpeedData.MissingValue))
                    {
                        aPoint = new PointD();
                        aPoint.X = windDirData.Data[0, i];
                        aPoint.Y = windDirData.Data[1, i];

                        WindArraw aArraw = new WindArraw();
                        aArraw.angle = windDir;
                        aArraw.length = windSpeed;
                        aArraw.size = 6;
                        aArraw.Point = aPoint;
                        aArraw.Value = stData.Data[2, i];

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aArraw, shapeNum))
                        {
                            if (isUV)
                            {
                                aLayer.EditCellValue("U", shapeNum, uData.Data[2, i]);
                                aLayer.EditCellValue("V", shapeNum, vData.Data[2, i]);
                            }
                            aLayer.EditCellValue("WindDirection", shapeNum, windDir);
                            aLayer.EditCellValue("WindSpeed", shapeNum, windSpeed);
                            if (ifAdd)
                                aLayer.EditCellValue(columnName, shapeNum, stData.Data[2, i]);
                        }
                    }
                }
            }

            aLayer.LayerName = lName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Vector;

            return aLayer;
        }

        /// <summary>
        /// Create station vector layer
        /// </summary>
        /// <param name="uData">U station data</param>
        /// <param name="vData">V station data</param>
        /// <param name="aLS">legend scheme</param>
        /// <param name="lName">layer name</param>
        /// <param name="isUV">if is U/V</param>
        /// <returns>vector layer</returns>
        public static VectorLayer CreateSTVectorLayer(StationData uData, StationData vData,
            LegendScheme aLS, string lName, bool isUV)
        {
            StationData windDirData = new StationData();
            StationData windSpeedData = new StationData();
            if (isUV)
            {
                DataMath.GetDSFromUV(uData, vData, ref windDirData, ref windSpeedData);
            }
            else
            {
                windDirData = uData;
                windSpeedData = vData;
            }

            int i;
            Single windDir, windSpeed;
            PointD aPoint = new PointD();

            string columnName = lName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindArraw);
            //Add data column         
            if (isUV)
            {
                aLayer.EditAddField("U", typeof(Single));
                aLayer.EditAddField("V", typeof(Single));
            }
            aLayer.EditAddField("WindDirection", typeof(Single));
            aLayer.EditAddField("WindSpeed", typeof(Single));

            aLayer.AttributeTable.Table.BeginLoadData();
            for (i = 0; i < windDirData.StNum; i++)
            {
                windDir = (Single)windDirData.Data[2, i];
                windSpeed = (Single)windSpeedData.Data[2, i];
                if (!MIMath.DoubleEquals(windDir, windDirData.MissingValue))
                {
                    if (!MIMath.DoubleEquals(windSpeed, windSpeedData.MissingValue))
                    {
                        aPoint = new PointD();
                        aPoint.X = windDirData.Data[0, i];
                        aPoint.Y = windDirData.Data[1, i];

                        WindArraw aArraw = new WindArraw();
                        aArraw.angle = windDir;
                        aArraw.length = windSpeed;
                        aArraw.size = 6;
                        aArraw.Point = aPoint;

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aArraw, shapeNum))
                        {
                            if (isUV)
                            {
                                aLayer.EditCellValue("U", shapeNum, uData.Data[2, i]);
                                aLayer.EditCellValue("V", shapeNum, vData.Data[2, i]);
                            }
                            aLayer.EditCellValue("WindDirection", shapeNum, windDir);
                            aLayer.EditCellValue("WindSpeed", shapeNum, windSpeed);
                        }
                    }
                }
            }

            aLayer.LayerName = lName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Vector;

            return aLayer;
        }

        /// <summary>
        /// Create station vector layer
        /// </summary>
        /// <param name="uData"></param>
        /// <param name="vData"></param>
        /// <param name="DiscreteData"></param>
        /// <param name="MissingValue"></param>
        /// <param name="aLS"></param>
        /// <param name="IfColor"></param>
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateSTVectorLayer_UV(double[,] uData, double[,] vData, double[,] DiscreteData,
            double MissingValue, LegendScheme aLS, bool IfColor, string LName)
        {
            int i;
            double U, V;
            PointD aPoint = new PointD();

            string columnName = LName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindArraw);
            //Add data column         
            aLayer.EditAddField("U", typeof(Single));
            aLayer.EditAddField("V", typeof(Single));
            aLayer.EditAddField("WindDirection", typeof(Single));
            aLayer.EditAddField("WindSpeed", typeof(Single));
            bool ifAdd = true;
            if (columnName == "U" || columnName == "V")
            {
                ifAdd = false;
            }
            if (IfColor && ifAdd)
            {
                aLayer.EditAddField(columnName, typeof(Single));
            }

            aLayer.AttributeTable.Table.BeginLoadData();
            for (i = 0; i < uData.GetLength(1); i++)
            {
                U = uData[2, i];
                V = vData[2, i];
                if (!MIMath.DoubleEquals(U, MissingValue))
                {
                    if (!MIMath.DoubleEquals(V, MissingValue))
                    {
                        aPoint = new PointD();
                        aPoint.X = uData[0, i];
                        aPoint.Y = uData[1, i];

                        WindArraw aArraw = new WindArraw();
                        aArraw = Draw.CalArraw(U, V, 0, 6, aPoint);
                        
                        if (IfColor)
                        {
                            aArraw.Value = DiscreteData[2, i];
                        }

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aArraw, shapeNum))
                        {
                            aLayer.EditCellValue("U", shapeNum, U);
                            aLayer.EditCellValue("V", shapeNum, V);
                            aLayer.EditCellValue("WindDirection", shapeNum, aArraw.angle);
                            aLayer.EditCellValue("WindSpeed", shapeNum, aArraw.length);
                            if (IfColor && ifAdd)
                                aLayer.EditCellValue(columnName, shapeNum, DiscreteData[2, i]);
                        }
                    }
                }
            }

            aLayer.LayerName = LName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Vector;

            return aLayer;
        }

        /// <summary>
        /// Create station vector layer
        /// </summary>
        /// <param name="windDirData"></param>
        /// <param name="windSpeedData"></param>
        /// <param name="DiscreteData"></param>
        /// <param name="MissingValue"></param>
        /// <param name="aLS"></param>
        /// <param name="IfColor"></param>
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateSTVectorLayer_DS(double[,] windDirData, double[,] windSpeedData, double[,] DiscreteData,
            double MissingValue, LegendScheme aLS, bool IfColor, string LName)
        {
            int i;
            Single windDir, windSpeed;
            PointD aPoint = new PointD();

            string columnName = LName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindArraw);
            //Add data column            
            aLayer.EditAddField("WindDirection", typeof(Single));
            aLayer.EditAddField("WindSpeed", typeof(Single));
            bool ifAdd = true;
            if (columnName == "WindDirection" || columnName == "WindSpeed")
            {
                ifAdd = false;
            }
            if (IfColor && ifAdd)
            {
                aLayer.EditAddField(columnName, typeof(Single));
            }

            aLayer.AttributeTable.Table.BeginLoadData();
            for (i = 0; i < windDirData.GetLength(1); i++)
            {
                windDir = (Single)windDirData[2, i];
                windSpeed = (Single)windSpeedData[2, i];
                if (!(Math.Abs(windDir / MissingValue - 1) < 0.01))
                {
                    if (!(Math.Abs(windSpeed / MissingValue - 1) < 0.01))
                    {
                        aPoint = new PointD();
                        aPoint.X = windDirData[0, i];
                        aPoint.Y = windDirData[1, i];                        

                        WindArraw aArraw = new WindArraw();
                        aArraw.angle = windDir;
                        aArraw.length = windSpeed;
                        aArraw.size = 6;
                        aArraw.Point = aPoint;                        
                        if (IfColor)
                        {
                            aArraw.Value = DiscreteData[2, i];
                        }

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aArraw, shapeNum))
                        {
                            aLayer.EditCellValue("WindDirection", shapeNum, windDir);
                            aLayer.EditCellValue("WindSpeed", shapeNum, windSpeed);
                            if (IfColor && ifAdd)
                                aLayer.EditCellValue(columnName, shapeNum, DiscreteData[2, i]);
                        }
                    }
                }
            }

            aLayer.LayerName = LName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Vector;

            return aLayer;
        }

        /// <summary>
        /// Create station barb layer from U/V or direction/speed station data
        /// </summary>
        /// <param name="uData"></param>
        /// <param name="vData"></param>
        /// <param name="stData"></param>
        /// <param name="aLS"></param>
        /// <param name="ifColor"></param>
        /// <param name="lName"></param>
        /// <param name="isUV"></param>
        /// <returns></returns>
        public static VectorLayer CreateSTBarbLayer_old(StationData uData, StationData vData, StationData stData,
            LegendScheme aLS, bool ifColor, string lName, bool isUV)
        {
            StationData windDirData = new StationData();
            StationData windSpeedData = new StationData();
            if (isUV)
            {
                DataMath.GetDSFromUV(uData, vData, ref windDirData, ref windSpeedData);
            }
            else
            {
                windDirData = uData;
                windSpeedData = vData;
            }

            int i;
            Single windDir, windSpeed;
            PointD aPoint = new PointD();

            string columnName = lName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindBarb);
            //Add data column        
            if (isUV)
            {
                aLayer.EditAddField("U", typeof(Single));
                aLayer.EditAddField("V", typeof(Single));
            }
            aLayer.EditAddField("WindDirection", typeof(Single));
            aLayer.EditAddField("WindSpeed", typeof(Single));
            bool ifAdd = true;
            if (aLayer.GetFieldNameList().Contains(columnName))
            {
                ifAdd = false;
            }
            if (ifColor && ifAdd)
            {
                aLayer.EditAddField(columnName, typeof(Single));
            }

            aLayer.AttributeTable.Table.BeginLoadData();
            for (i = 0; i < windDirData.StNum; i++)
            {
                windDir = (Single)windDirData.Data[2, i];
                windSpeed = (Single)windSpeedData.Data[2, i];
                if (windSpeed == 0)
                    continue;

                if (!MIMath.DoubleEquals(windDir, windDirData.MissingValue))
                {
                    if (!MIMath.DoubleEquals(windSpeed, windSpeedData.MissingValue))
                    {
                        aPoint = new PointD();
                        aPoint.X = windDirData.Data[0, i];
                        aPoint.Y = windDirData.Data[1, i];
                        WindBarb aWB = new WindBarb();
                        aWB = Draw.CalWindBarb(windDir, windSpeed, 0, 10, aPoint);
                        if (ifColor)
                        {
                            aWB.Value = stData.Data[2, i];
                        }

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aWB, shapeNum))
                        {
                            if (isUV)
                            {
                                aLayer.EditCellValue("U", shapeNum, uData.Data[2, i]);
                                aLayer.EditCellValue("V", shapeNum, vData.Data[2, i]);
                            }
                            aLayer.EditCellValue("WindDirection", shapeNum, windDir);
                            aLayer.EditCellValue("WindSpeed", shapeNum, windSpeed);
                            if (ifColor && ifAdd)
                                aLayer.EditCellValue(columnName, shapeNum, stData.Data[2, i]);
                        }
                    }
                }
            }

            aLayer.LayerName = lName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Barb;

            return aLayer;
        }

        /// <summary>
        /// Create station barb layer from U/V or direction/speed station data
        /// </summary>
        /// <param name="uData"></param>
        /// <param name="vData"></param>
        /// <param name="stData"></param>
        /// <param name="aLS"></param>
        /// <param name="lName"></param>
        /// <param name="isUV"></param>
        /// <returns></returns>
        public static VectorLayer CreateSTBarbLayer(StationData uData, StationData vData, StationData stData,
            LegendScheme aLS, string lName, bool isUV)
        {
            StationData windDirData = new StationData();
            StationData windSpeedData = new StationData();
            if (isUV)
            {
                DataMath.GetDSFromUV(uData, vData, ref windDirData, ref windSpeedData);
            }
            else
            {
                windDirData = uData;
                windSpeedData = vData;
            }

            int i;
            Single windDir, windSpeed;
            PointD aPoint = new PointD();

            string columnName = lName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindBarb);
            //Add data column        
            if (isUV)
            {
                aLayer.EditAddField("U", typeof(Single));
                aLayer.EditAddField("V", typeof(Single));
            }
            aLayer.EditAddField("WindDirection", typeof(Single));
            aLayer.EditAddField("WindSpeed", typeof(Single));
            bool ifAdd = true;
            if (aLayer.GetFieldNameList().Contains(columnName))
            {
                ifAdd = false;
            }
            if (ifAdd)
            {
                aLayer.EditAddField(columnName, typeof(Single));
            }

            aLayer.AttributeTable.Table.BeginLoadData();
            for (i = 0; i < windDirData.StNum; i++)
            {
                windDir = (Single)windDirData.Data[2, i];
                windSpeed = (Single)windSpeedData.Data[2, i];
                if (windSpeed == 0)
                    continue;

                if (!MIMath.DoubleEquals(windDir, windDirData.MissingValue))
                {
                    if (!MIMath.DoubleEquals(windSpeed, windSpeedData.MissingValue))
                    {
                        aPoint = new PointD();
                        aPoint.X = windDirData.Data[0, i];
                        aPoint.Y = windDirData.Data[1, i];
                        WindBarb aWB = new WindBarb();
                        aWB = Draw.CalWindBarb(windDir, windSpeed, 0, 10, aPoint);
                        aWB.Value = stData.Data[2, i];

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aWB, shapeNum))
                        {
                            if (isUV)
                            {
                                aLayer.EditCellValue("U", shapeNum, uData.Data[2, i]);
                                aLayer.EditCellValue("V", shapeNum, vData.Data[2, i]);
                            }
                            aLayer.EditCellValue("WindDirection", shapeNum, windDir);
                            aLayer.EditCellValue("WindSpeed", shapeNum, windSpeed);
                            if (ifAdd)
                                aLayer.EditCellValue(columnName, shapeNum, stData.Data[2, i]);
                        }
                    }
                }
            }

            aLayer.LayerName = lName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Barb;

            return aLayer;
        }

        /// <summary>
        /// Create station barb layer from U/V or direction/speed station data
        /// </summary>
        /// <param name="uData"></param>
        /// <param name="vData"></param>
        /// <param name="aLS"></param>
        /// <param name="lName"></param>
        /// <param name="isUV"></param>
        /// <returns></returns>
        public static VectorLayer CreateSTBarbLayer(StationData uData, StationData vData,
            LegendScheme aLS, string lName, bool isUV)
        {
            StationData windDirData = new StationData();
            StationData windSpeedData = new StationData();
            if (isUV)
            {
                DataMath.GetDSFromUV(uData, vData, ref windDirData, ref windSpeedData);
            }
            else
            {
                windDirData = uData;
                windSpeedData = vData;
            }

            int i;
            Single windDir, windSpeed;
            PointD aPoint = new PointD();

            string columnName = lName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindBarb);
            //Add data column        
            if (isUV)
            {
                aLayer.EditAddField("U", typeof(Single));
                aLayer.EditAddField("V", typeof(Single));
            }
            aLayer.EditAddField("WindDirection", typeof(Single));
            aLayer.EditAddField("WindSpeed", typeof(Single));

            aLayer.AttributeTable.Table.BeginLoadData();
            for (i = 0; i < windDirData.StNum; i++)
            {
                windDir = (Single)windDirData.Data[2, i];
                windSpeed = (Single)windSpeedData.Data[2, i];
                if (windSpeed == 0)
                    continue;

                if (!MIMath.DoubleEquals(windDir, windDirData.MissingValue))
                {
                    if (!MIMath.DoubleEquals(windSpeed, windSpeedData.MissingValue))
                    {
                        aPoint = new PointD();
                        aPoint.X = windDirData.Data[0, i];
                        aPoint.Y = windDirData.Data[1, i];
                        WindBarb aWB = new WindBarb();
                        aWB = Draw.CalWindBarb(windDir, windSpeed, 0, 10, aPoint);

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aWB, shapeNum))
                        {
                            if (isUV)
                            {
                                aLayer.EditCellValue("U", shapeNum, uData.Data[2, i]);
                                aLayer.EditCellValue("V", shapeNum, vData.Data[2, i]);
                            }
                            aLayer.EditCellValue("WindDirection", shapeNum, windDir);
                            aLayer.EditCellValue("WindSpeed", shapeNum, windSpeed);
                        }
                    }
                }
            }

            aLayer.LayerName = lName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Barb;

            return aLayer;
        }

        /// <summary>
        /// Create station barb layer
        /// </summary>
        /// <param name="windDirData"></param>
        /// <param name="windSpeedData"></param>
        /// <param name="DiscreteData"></param>
        /// <param name="MissingValue"></param>
        /// <param name="aLS"></param>
        /// <param name="IfColor"></param>
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateSTBarbLayer(double[,] windDirData, double[,] windSpeedData, double[,] DiscreteData,
            double MissingValue, LegendScheme aLS, bool IfColor, string LName)
        {
            int i;                                              
            Single windDir, windSpeed;
            PointD aPoint;

            string columnName = LName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WindBarb);
            //Add data column            
            aLayer.EditAddField("WindDirection", typeof(Single));
            aLayer.EditAddField("WindSpeed", typeof(Single));
            bool ifAdd = true;
            if (columnName == "WindDirection" || columnName == "WindSpeed")
            {
                ifAdd = false;
            }
            if (IfColor && ifAdd)
            {
                aLayer.EditAddField(columnName, typeof(Single));
            }

            aLayer.AttributeTable.Table.BeginLoadData();
            for (i = 0; i < windDirData.GetLength(1); i++)
            {
                windDir = (Single)windDirData[2, i];
                windSpeed = (Single)windSpeedData[2, i];
                if (!(Math.Abs(windDir / MissingValue - 1) < 0.01))
                {
                    if (!(Math.Abs(windSpeed / MissingValue - 1) < 0.01))
                    {
                        aPoint = new PointD();
                        aPoint.X = windDirData[0, i];
                        aPoint.Y = windDirData[1, i];
                        WindBarb aWB = new WindBarb();
                        aWB = Draw.CalWindBarb(windDir, windSpeed, 0, 10, aPoint);
                        if (IfColor)
                        {
                            aWB.Value = DiscreteData[2, i];
                        }

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aWB, shapeNum))
                        {
                            aLayer.EditCellValue("WindDirection", shapeNum, windDir);
                            aLayer.EditCellValue("WindSpeed", shapeNum, windSpeed);
                            if (IfColor && ifAdd)
                                aLayer.EditCellValue(columnName, shapeNum, DiscreteData[2, i]);
                        }                            
                    }
                }
            }
            
            aLayer.LayerName = LName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.Barb;            
            
            return aLayer;
        }        

        /// <summary>
        /// Create station weather symbol layer
        /// </summary>
        /// <param name="weatherData">weather data</param>
        /// <param name="aLS">legend scheme</param>
        /// <param name="LName">layer name</param>
        /// <returns>vector weather symbol layer</returns>
        public static VectorLayer CreateWeatherSymbolLayer(StationData weatherData, LegendScheme aLS, string LName)
        {
            int i;
            WeatherSymbol aWS = new WeatherSymbol();
            int weather;
            PointD aPoint;           

            string columnName = LName.Split('_')[0];
            VectorLayer aLayer = new VectorLayer(ShapeTypes.WeatherSymbol);
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = columnName;
            aDC.DataType = typeof(double);
            aLayer.EditAddField(aDC);

            aLayer.AttributeTable.Table.BeginLoadData();
            for (i = 0; i < weatherData.Data.GetLength(1); i++)
            {
                weather = (int)weatherData.Data[2, i];
                if (!(Math.Abs(weather / weatherData.MissingValue - 1) < 0.01))
                {
                    aPoint = new PointD();
                    aPoint.X = (Single)weatherData.Data[0, i];
                    aPoint.Y = (Single)weatherData.Data[1, i];
                    aWS = Draw.CalWeatherSymbol(0, weather, aPoint);

                    int shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aWS, shapeNum))
                        aLayer.EditCellValue(columnName, shapeNum, weather);
                }
            }

            aLayer.LayerName = LName;
            aLS.FieldName = columnName;
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.WeatherSymbol;

            return aLayer;
        }

        /// <summary>
        /// Create station weather symbol layer
        /// </summary>
        /// <param name="weatherData">Weather data</param>
        /// <param name="WeatherType">Weather type</param>        
        /// <param name="aLS">Legend scheme</param>
        /// <param name="LName">Layer name</param>
        /// <returns>Weather symbol layer</returns>
        public static VectorLayer CreateWeatherSymbolLayer(StationData weatherData, string WeatherType,
            LegendScheme aLS, string LName)
        {                      
            List<int> wList = GetWeatherTypes(WeatherType);

            return CreateWeatherSymbolLayer(weatherData, wList, aLS, LName);
        }

        /// <summary>
        /// Create station weather symbol layer
        /// </summary>
        /// <param name="weatherData">Weather data</param>
        /// <param name="wList">Weather index list</param>        
        /// <param name="aLS">Legend scheme</param>
        /// <param name="LName">Layer name</param>
        /// <returns>Weather symbol layer</returns>
        public static VectorLayer CreateWeatherSymbolLayer(StationData weatherData, List<int> wList,
            LegendScheme aLS, string LName)
        {
            int i;
            int weather;
            PointD aPoint;

            string columnName = "Weather";
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = columnName;
            aDC.DataType = typeof(double);
            aLayer.EditAddField(aDC);

            aLayer.AttributeTable.Table.BeginLoadData();
            for (i = 0; i < weatherData.Data.GetLength(1); i++)
            {
                weather = (int)weatherData.Data[2, i];
                if (!(Math.Abs(weather / weatherData.MissingValue - 1) < 0.01))
                {
                    if (wList.Contains(weather))
                    {
                        aPoint = new PointD();
                        aPoint.X = weatherData.Data[0, i];
                        aPoint.Y = weatherData.Data[1, i];
                        PointShape aPS = new PointShape();
                        aPS.Point = aPoint;
                        aPS.Value = weather;

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aPS, shapeNum))
                            aLayer.EditCellValue(columnName, shapeNum, weather);
                    }
                }
            }


            aLayer.LayerName = LName;            
            aLS.FieldName = columnName;
            aLayer.LegendScheme = aLS;
            aLayer.LayerDrawType = LayerDrawType.StationPoint;

            return aLayer;
        }

        /// <summary>
        /// Create station model layer
        /// </summary>
        /// <param name="stationModelData">station model data</param>
        /// <param name="MissingValue">undefine data</param>
        /// <param name="aLS">legend scheme</param>
        /// <param name="LName">layer name</param>
        /// <param name="isSurface">if is surface</param>
        /// <returns>vector layer</returns>
        public static VectorLayer CreateStationModelLayer(StationModelData stationModelData,
            double MissingValue, LegendScheme aLS, string LName, bool isSurface)
        {
            int i;            
            StationModelShape aSM = new StationModelShape();           
            Single windDir, windSpeed;
            int weather, cCover, temp, dewPoint, pressure;
            PointD aPoint;
            
            VectorLayer aLayer = new VectorLayer(ShapeTypes.StationModel);
            aLayer.EditAddField(new DataColumn("Station", typeof(string)));
            aLayer.EditAddField(new DataColumn("Stid", typeof(string)));
            aLayer.EditAddField(new DataColumn("WindDirection",typeof(Single)));
            aLayer.EditAddField(new DataColumn("WindSpeed", typeof(Single)));
            aLayer.EditAddField(new DataColumn("Weather", typeof(int)));
            aLayer.EditAddField(new DataColumn("Temperature", typeof(int)));
            aLayer.EditAddField(new DataColumn("DewPoint", typeof(int)));
            aLayer.EditAddField(new DataColumn("Pressure", typeof(int)));
            aLayer.EditAddField(new DataColumn("CloudCoverage", typeof(int))); 

            aLayer.AttributeTable.Table.BeginLoadData();
            for (i = 0; i < stationModelData.DataNum; i++)
            {
                StationModel sm = stationModelData.Data[i];
                windDir = (Single)sm.WindDirection;
                windSpeed = (Single)sm.WindSpeed;
                if (!(MIMath.DoubleEquals(windDir, MissingValue)))
                {
                    if (!(Math.Abs(windSpeed / MissingValue - 1) < 0.01))
                    {
                        aPoint = new PointD();
                        aPoint.X = (Single)sm.Longitude;
                        aPoint.Y = (Single)sm.Latitude;
                        weather = (int)sm.Weather;
                        cCover = (int)sm.CloudCover;
                        temp = (int)sm.Temperature;
                        dewPoint = (int)sm.DewPoint;
                        pressure = (int)sm.Pressure;
                        if (isSurface)
                        {
                            if (!(MIMath.DoubleEquals(sm.Pressure, MissingValue)))                       
                            {
                                //pressure = (int)((stationModelData[9, i] - 1000) * 10);
                                string pStr = ((int)(sm.Pressure * 10)).ToString();
                                if (pStr.Length < 3)
                                    pressure = (int)MissingValue;
                                else
                                {
                                    pStr = pStr.Substring(pStr.Length - 3);
                                    pressure = int.Parse(pStr);
                                }
                            }
                        }
                        
                        aSM = Draw.CalStationModel(windDir, windSpeed, 0, 12, aPoint, weather,
                            temp, dewPoint, pressure, cCover);

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aSM, shapeNum))
                        {
                            aLayer.EditCellValue("Station", shapeNum, sm.StationName);
                            aLayer.EditCellValue("Stid", shapeNum, sm.StationIdentifer);
                            aLayer.EditCellValue("WindDirection", shapeNum, windDir);
                            aLayer.EditCellValue("WindSpeed", shapeNum, windSpeed);
                            aLayer.EditCellValue("Weather", shapeNum, weather);
                            aLayer.EditCellValue("Temperature", shapeNum, temp);
                            aLayer.EditCellValue("DewPoint", shapeNum, dewPoint);
                            aLayer.EditCellValue("Pressure", shapeNum, pressure);
                            aLayer.EditCellValue("CloudCoverage", shapeNum, cCover);
                        }                        
                    }
                }
            }
            
            aLayer.LayerName = LName;
            aLS.FieldName = "";
            aLayer.LegendScheme = (LegendScheme)aLS.Clone();
            aLayer.LayerDrawType = LayerDrawType.StationModel;                                             

            return aLayer;
        }

        /// <summary>
        /// Create station weather symbol layer
        /// </summary>
        /// <param name="weatherData"></param>
        /// <param name="WeatherType"></param>        
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateWeatherSymbolLayer(StationData weatherData, string WeatherType,
            string LName)
        {
            List<int> wList = GetWeatherTypes(WeatherType);

            return CreateWeatherSymbolLayer(weatherData, wList, LName);
        }

        /// <summary>
        /// Create station weather symbol layer
        /// </summary>
        /// <param name="weatherData"></param>
        /// <param name="wList"></param>        
        /// <param name="LName"></param>
        /// <returns></returns>
        public static VectorLayer CreateWeatherSymbolLayer(StationData weatherData, List<int> wList,
            string LName)
        {
            LegendScheme aLS = CreateWeatherLegendScheme(wList);
            return CreateWeatherSymbolLayer(weatherData, wList, aLS, LName);
        }

        private static LegendScheme CreateWeatherLegendScheme(List<int> wList)
        {
            LegendScheme aLS = new LegendScheme(ShapeTypes.Point);
            aLS.LegendType = LegendType.UniqueValue;
            foreach (int w in wList)
            {
                PointBreak aPB = new PointBreak();
                aPB.MarkerType = MarkerType.Character;
                aPB.Size = 12;
                aPB.Color = Color.Blue;
                aPB.FontName = "Weather";
                aPB.StartValue = w;
                aPB.EndValue = w;
                int charIdx = w + 28;
                if (w == 99)
                    charIdx = w + 97;
                aPB.CharIndex = charIdx;
                aPB.Caption = w.ToString();

                aLS.LegendBreaks.Add(aPB);
            }

            return aLS;
        }

        /// <summary>
        /// Create raster layer
        /// </summary>
        /// <param name="GridData">grid data</param>        
        ///<param name="LName">layer name</param>
        ///<param name="paletteFile">palette file</param>
        /// <returns>raster layer</returns>
        public static RasterLayer CreateRasterLayer(GridData GridData, string LName, string paletteFile)
        {
            RasterLayer aRLayer = new RasterLayer();
            aRLayer.GridData = GridData;
            aRLayer.SetImageByGridData();
            aRLayer.SetPalette(paletteFile);
            aRLayer.LayerName = LName;
            aRLayer.Visible = true;
            aRLayer.LayerDrawType = LayerDrawType.Raster;
            aRLayer.IsMaskout = true;

            return aRLayer;
        }

        /// <summary>
        /// Create raster layer
        /// </summary>
        /// <param name="GridData">grid data</param>        
        ///<param name="LName">layer name</param>
        ///<param name="aLS">legend scheme</param>
        /// <returns>raster layer</returns>
        public static RasterLayer CreateRasterLayer(GridData GridData, string LName, LegendScheme aLS)
        {           
            RasterLayer aRLayer = new RasterLayer();
            aRLayer.GridData = GridData;
            aRLayer.LegendScheme = aLS;
            aRLayer.LayerName = LName;
            aRLayer.Visible = true;            
            aRLayer.LayerDrawType = LayerDrawType.Raster;
            aRLayer.IsMaskout = true;

            return aRLayer;
        }

        /// <summary>
        /// Create image layer
        /// </summary>
        /// <param name="aImage">image</param>
        /// <param name="aWFP">world file paramters</param>
        /// <param name="LName">layer name</param>
        /// <returns>image layer</returns>
        public static ImageLayer CreateImageLayer(Image aImage, WorldFilePara aWFP, string LName)
        {
            ImageLayer aImageLayer = new ImageLayer();
            aImageLayer.Image = aImage;
            aImageLayer.LayerName = LName;
            aImageLayer.Visible = true;            
            aImageLayer.WorldFileParaV = aWFP;

            double XBR, YBR;
            XBR = aImageLayer.Image.Width * aImageLayer.WorldFileParaV.XScale + aImageLayer.WorldFileParaV.XUL;
            YBR = aImageLayer.Image.Height * aImageLayer.WorldFileParaV.YScale + aImageLayer.WorldFileParaV.YUL;
            MeteoInfoC.Global.Extent aExtent = new MeteoInfoC.Global.Extent();
            aExtent.minX = aImageLayer.WorldFileParaV.XUL;
            aExtent.minY = YBR;
            aExtent.maxX = XBR;
            aExtent.maxY = aImageLayer.WorldFileParaV.YUL;
            aImageLayer.Extent = aExtent;
            aImageLayer.LayerDrawType = LayerDrawType.Image;
            aImageLayer.IsMaskout = true;

            return aImageLayer;
        }

        /// <summary>
        /// Create image layer
        /// </summary>
        /// <param name="GridData">grid data</param>
        ///<param name="X">X array</param>
        ///<param name="Y">Y array</param>
        ///<param name="LName">layer name</param>
        /// <returns>image layer</returns>
        public static ImageLayer CreateImageLayer(double[,] GridData, double[] X, double[] Y,
            string LName)
        {
            int xNum = X.Length;
            int yNum = Y.Length;
            byte[] imageBytes = new byte[xNum * yNum];
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    imageBytes[i * xNum + j] =(byte)(int)GridData[i, j];
                }
            }
            Image aImage = CreateBitmap(imageBytes, xNum, yNum);

            WorldFilePara aWFP = new WorldFilePara();
            aWFP.XUL = X[0];
            aWFP.YUL = Y[yNum - 1];
            aWFP.XScale = (X[xNum - 1] - X[0]) / aImage.Width;
            aWFP.YScale = (Y[0] - Y[yNum - 1]) / aImage.Height;
            aWFP.XRotate = 0;
            aWFP.YRotate = 0;

            ImageLayer aImageLayer = CreateImageLayer(aImage, aWFP, LName);

            return aImageLayer;
        }

        /// <summary>
        /// Create image layer
        /// </summary>
        /// <param name="imageBytes">byte data array</param>
        /// <param name="xNum">x number</param>
        /// <param name="yNum">y number</param>
        ///<param name="aWFP">world file parameter</param>
        ///<param name="LName">layer name</param>
        /// <returns>image layer</returns>
        public static ImageLayer CreateImageLayer(byte[] imageBytes, int xNum, int yNum,
            WorldFilePara aWFP, string LName)
        {                       
            Image aImage = CreateBitmap(imageBytes, xNum, yNum);
            ImageLayer aImageLayer = CreateImageLayer(aImage, aWFP, LName);

            return aImageLayer;
        }

        /// <summary>
        /// Create image layer
        /// </summary>
        /// <param name="pFile">palette file</param>
        /// <param name="imageBytes">byte data array</param>
        /// <param name="xNum">x number</param>
        /// <param name="yNum">y number</param>
        ///<param name="aWFP">world file parameter</param>
        ///<param name="LName">layer name</param>
        /// <returns>image layer</returns>
        public static ImageLayer CreateImageLayer(string pFile, byte[] imageBytes, int xNum, int yNum,
            WorldFilePara aWFP, string LName)
        {
            Image aImage = CreateBitmap(imageBytes, xNum, yNum);
            ImageLayer aImageLayer = CreateImageLayer(aImage, aWFP, LName);

            return aImageLayer;
        }

        /// <summary>
        /// Get bitmap from bytes
        /// </summary>
        /// <param name="originalImageData">original bytes data</param>
        /// <param name="originalWidth">width</param>
        /// <param name="originalHeight">heigth</param>
        /// <returns>bitmap</returns>
        public static Bitmap CreateBitmap(byte[] originalImageData, int originalWidth, int originalHeight)
        {
            //指定8位格式，即256色
            Bitmap resultBitmap = new Bitmap(originalWidth, originalHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            //将该位图存入内存中
            MemoryStream curImageStream = new MemoryStream();
            resultBitmap.Save(curImageStream, System.Drawing.Imaging.ImageFormat.Bmp);
            curImageStream.Flush();

            //由于位图数据需要DWORD对齐（4byte倍数），计算需要补位的个数
            int curPadNum = ((originalWidth * 8 + 31) / 32 * 4) - originalWidth;

            //最终生成的位图数据大小
            int bitmapDataSize = ((originalWidth * 8 + 31) / 32 * 4) * originalHeight;

            //数据部分相对文件开始偏移，具体可以参考位图文件格式
            int dataOffset = ReadData(curImageStream, 10, 4);


            //改变调色板，因为默认的调色板是32位彩色的，需要修改为256色的调色板
            int paletteStart = 54;
            int paletteEnd = dataOffset;
            List<Color> colors = new List<Color>();
            for (int i = 0; i < 256; i++)
            {
                colors.Add(Color.FromArgb(255, i, i, i));
            }
            SetPalette(colors, paletteStart, paletteEnd, curImageStream);

            //最终生成的位图数据，以及大小，高度没有变，宽度需要调整
            byte[] destImageData = new byte[bitmapDataSize];
            int destWidth = originalWidth + curPadNum;

            //Not inverse
            for (int originalRowIndex = 0; originalRowIndex < originalHeight; originalRowIndex++)
            {
                int destRowIndex = originalRowIndex;

                for (int dataIndex = 0; dataIndex < originalWidth; dataIndex++)
                {
                    //同时还要注意，新的位图数据的宽度已经变化destWidth，否则会产生错位
                    destImageData[destRowIndex * destWidth + dataIndex] = originalImageData[originalRowIndex * originalWidth + dataIndex];
                }
            }


            //将流的Position移到数据段   
            curImageStream.Position = dataOffset;

            //将新位图数据写入内存中
            curImageStream.Write(destImageData, 0, bitmapDataSize);

            curImageStream.Flush();

            //将内存中的位图写入Bitmap对象
            resultBitmap = new Bitmap(curImageStream);

            curImageStream.Dispose();
            curImageStream.Close();

            return resultBitmap;
        }

        /// <summary>
        /// Set color palette to a image from a palette file
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <param name="aImage">image</param>
        public static void SetPalette(string aFile, Image aImage)
        {
            List<Color> colors = GetColorsFromPaletteFile(aFile);

            SetPalette(colors, aImage);
        }

        private static List<Color> GetColorsFromPaletteFile(string pFile)
        {
            StreamReader sr = new StreamReader(pFile, Encoding.Default);
            sr.ReadLine();
            string aLine = sr.ReadLine();
            string[] dataArray;
            List<Color> colors = new List<Color>();
            while (aLine != null)
            {
                if (aLine == String.Empty)
                {
                    aLine = sr.ReadLine();
                    continue;
                }

                dataArray = aLine.Split();
                int LastNonEmpty = -1;
                List<int> dataList = new List<int>();
                for (int i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] != string.Empty)
                    {
                        LastNonEmpty++;
                        dataList.Add(int.Parse(dataArray[i]));
                    }
                }                
                colors.Add(Color.FromArgb(255, dataList[3], dataList[2], dataList[1]));

                aLine = sr.ReadLine();
            }            
            sr.Close();

            return colors;
        }

        /// <summary>
        /// Set color palette to a image
        /// </summary>
        /// <param name="colors">color array</param>
        /// <param name="aImage">image</param>
        public static void SetPalette(List<Color> colors, Image aImage)
        {
            ColorPalette pal = aImage.Palette;
            int count = colors.Count;
            if (count > 256)
                count = 256;

            for (int i = 0; i < count; i++)
                pal.Entries[i] = colors[i];

            aImage.Palette = pal;
        }

        /// <summary>
        /// Set a transparency color
        /// </summary>
        /// <param name="tColor">transparency color</param>
        /// <param name="aImage">image</param>
        public static void SetTransparencyColor(Color tColor, Image aImage)
        {
            ColorPalette pal = aImage.Palette;
            for (int i = 0; i < pal.Entries.Length; i++)
            {
                Color aColor = pal.Entries[i];
                if (aColor.R == tColor.R && aColor.G == tColor.G && aColor.B == tColor.B)
                {
                    pal.Entries[i] = aColor = Color.FromArgb(0, aColor.R, aColor.G, aColor.B);
                }
            }

            aImage.Palette = pal;
        }

        /// <summary>
        /// Set palette with grey colors
        /// </summary>
        /// <param name="colors">color list</param>
        /// <param name="paletteStart">start index</param>
        /// <param name="paletteEnd">end index</param>
        /// <param name="curImageStream">memory stream</param>
        public static void SetPalette(List<Color> colors, int paletteStart, int paletteEnd, MemoryStream curImageStream)
        {
            int c = 0;

            for (int i = paletteStart; i < paletteEnd; i += 4)
            {
                byte[] tempColor = new byte[4];
                tempColor[0] = colors[c].B;
                tempColor[1] = colors[c].G;
                tempColor[2] = colors[c].R;
                tempColor[3] = colors[c].A;
                c++;

                curImageStream.Position = i;
                curImageStream.Write(tempColor, 0, 4);
            }
        }

        /// <summary>
        /// 从内存流中指定位置，读取数据
        /// </summary>
        /// <param name="curStream"></param>
        /// <param name="startPosition"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int ReadData(MemoryStream curStream, int startPosition, int length)
        {
            int result = -1;

            byte[] tempData = new byte[length];
            curStream.Position = startPosition;
            curStream.Read(tempData, 0, length);
            result = BitConverter.ToInt32(tempData, 0);

            return result;
        }

        /// <summary>
        /// 向内存流中指定位置，写入数据
        /// </summary>
        /// <param name="curStream"></param>
        /// <param name="startPosition"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        private static void WriteData(MemoryStream curStream, int startPosition, int length, int value)
        {
            curStream.Position = startPosition;
            curStream.Write(BitConverter.GetBytes(value), 0, length);
        }

        /// <summary>
        /// Get weather list
        /// </summary>
        /// <param name="weatherType"></param>
        /// <returns></returns>
        public static List<int> GetWeatherTypes(string weatherType)
        {
            List<int> weatherList = new List<int>();
            int i;
            switch (weatherType)
            {
                case "All Weather":
                    for (i = 4; i < 100; i++)
                    {
                        weatherList.Add(i);
                    }
                    break;
                case "SDS":
                    weatherList.AddRange(new int[] { 6, 7, 8, 9, 30, 31, 32, 33, 34, 35 });
                    break;
                case "SDS, Haze":
                    weatherList.AddRange(new int[] { 5, 6, 7, 8, 9, 30, 31, 32, 33, 34, 35 });
                    break;
                case "Smoke, Haze, Mist":
                    weatherList.AddRange(new int[] { 4, 5, 10 });
                    break;
                case "Smoke":
                    weatherList.Add(4);
                    break;
                case "Haze":
                    weatherList.Add(5);
                    break;
                case "Mist":
                    weatherList.Add(10);
                    break;
                case "Fog":
                    for (i = 40; i < 50; i++)
                    {
                        weatherList.Add(i);
                    }
                    break;
            }

            return weatherList;
        }
        
        #endregion
    }
}
