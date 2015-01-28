using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MeteoInfoC.Shape;
using MeteoInfoC.Global;
using MeteoInfoC.Layer;
using MeteoInfoC.Map;
using MeteoInfoC.Projections;

namespace MeteoInfoC.Geoprocess
{
    /// <summary>
    /// Computational geometry
    /// </summary>
    public static class GeoComputation
    {
        private const double EARTH_RADIUS = 6378.137;

        #region General

        /// <summary>
        /// Determine if a point list is clockwise
        /// </summary>
        /// <param name="pointList">point list</param>
        /// <returns>is or not clockwise</returns>
        public static bool IsClockwise(List<PointD> pointList)
        {
            int i;
            PointD aPoint = new PointD();
            double yMax = 0;
            int yMaxIdx = 0;
            for (i = 0; i < pointList.Count - 1; i++)
            {
                aPoint = (PointD)pointList[i];
                if (i == 0)
                {
                    yMax = aPoint.Y;
                    yMaxIdx = 0;
                }
                else
                {
                    if (yMax < aPoint.Y)
                    {
                        yMax = aPoint.Y;
                        yMaxIdx = i;
                    }
                }
            }
            PointD p1, p2, p3;
            int p1Idx, p2Idx, p3Idx;
            p1Idx = yMaxIdx - 1;
            p2Idx = yMaxIdx;
            p3Idx = yMaxIdx + 1;
            if (yMaxIdx == 0)
                p1Idx = pointList.Count - 2;

            p1 = (PointD)pointList[p1Idx];
            p2 = (PointD)pointList[p2Idx];
            p3 = (PointD)pointList[p3Idx];
            if ((p3.X - p1.X) * (p2.Y - p1.Y) - (p2.X - p1.X) * (p3.Y - p1.Y) > 0)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Determine if a point list is clockwise
        /// </summary>
        /// <param name="pointList">point list</param>
        /// <returns>is or not clockwise</returns>
        public static bool IsClockwise(List<PointM> pointList)
        {
            int i;
            PointM aPoint = new PointM();
            double yMax = 0;
            int yMaxIdx = 0;
            for (i = 0; i < pointList.Count - 1; i++)
            {
                aPoint = pointList[i];
                if (i == 0)
                {
                    yMax = aPoint.Y;
                    yMaxIdx = 0;
                }
                else
                {
                    if (yMax < aPoint.Y)
                    {
                        yMax = aPoint.Y;
                        yMaxIdx = i;
                    }
                }
            }
            PointD p1, p2, p3;
            int p1Idx, p2Idx, p3Idx;
            p1Idx = yMaxIdx - 1;
            p2Idx = yMaxIdx;
            p3Idx = yMaxIdx + 1;
            if (yMaxIdx == 0)
                p1Idx = pointList.Count - 2;

            p1 = pointList[p1Idx];
            p2 = pointList[p2Idx];
            p3 = pointList[p3Idx];
            if ((p3.X - p1.X) * (p2.Y - p1.Y) - (p2.X - p1.X) * (p3.Y - p1.Y) > 0)
                return true;
            else
                return false;

        }        

        /// <summary>
        /// Determine if a point array is clockwise
        /// </summary>
        /// <param name="points">point array</param>
        /// <returns>is or not clockwise</returns>
        public static bool IsClockwise(PointD[] points)
        {            
            List<PointD> pointList = new List<PointD>(points);
            return IsClockwise(pointList);
        }

        /// <summary>
        /// Judge if a point is in a polygon
        /// </summary>
        /// <param name="poly">polygon border</param>
        /// <param name="aPoint">point</param>
        /// <returns>If the point is in the polygon</returns>
        public static bool PointInPolygon(List<PointD> poly, PointD aPoint)
        {
            double xNew, yNew, xOld, yOld;
            double x1, y1, x2, y2;
            int i;
            bool inside = false;
            int nPoints = poly.Count;

            if (nPoints < 3)
                return false;

            xOld = (poly[nPoints - 1]).X;
            yOld = (poly[nPoints - 1]).Y;
            for (i = 0; i < nPoints; i++)
            {
                xNew = (poly[i]).X;
                yNew = (poly[i]).Y;
                if (xNew > xOld)
                {
                    x1 = xOld;
                    x2 = xNew;
                    y1 = yOld;
                    y2 = yNew;
                }
                else
                {
                    x1 = xNew;
                    x2 = xOld;
                    y1 = yNew;
                    y2 = yOld;
                }

                //---- edge "open" at left end
                if ((xNew < aPoint.X) == (aPoint.X <= xOld) &&
                   (aPoint.Y - y1) * (x2 - x1) < (y2 - y1) * (aPoint.X - x1))
                    inside = !inside;

                xOld = xNew;
                yOld = yNew;
            }

            return inside;
        }

        /// <summary>
        /// Judge if a point is in a polygon
        /// </summary>
        /// <param name="aPolygon">polygon</param>
        /// <param name="aPoint">point</param>
        /// <returns>is in</returns>
        public static bool PointInPolygon(Polygon aPolygon, PointD aPoint)
        {
            if (aPolygon.HasHole)
            {
                bool isIn = PointInPolygon(aPolygon.OutLine, aPoint);
                if (isIn)
                {
                    foreach (List<PointD> aLine in aPolygon.HoleLines)
                    {
                        if (PointInPolygon(aLine, aPoint))
                        {
                            isIn = false;
                            break;
                        }
                    }
                }

                return isIn;
            }
            else
                return PointInPolygon(aPolygon.OutLine, aPoint);
        }

        /// <summary>
        /// Judge if a point is in a polygon
        /// </summary>
        /// <param name="aPolygon">polygon</param>
        /// <param name="aPoint">point</param>
        /// <returns>is in</returns>
        public static bool PointInPolygon(PolygonShape aPolygon, PointD aPoint)
        {
            bool isIn = false;
            if (aPolygon.GetType() == typeof(CircleShape))
            {
                return ((CircleShape)aPolygon).IsPointInside(aPoint);
            }
            else
            {
                for (int i = 0; i < aPolygon.Polygons.Count; i++)
                {
                    Polygon aPRing = aPolygon.Polygons[i];
                    isIn = PointInPolygon(aPRing.OutLine, aPoint);
                    if (isIn)
                    {
                        if (aPRing.HasHole)
                        {
                            foreach (List<PointD> aLine in aPRing.HoleLines)
                            {
                                if (PointInPolygon(aLine, aPoint))
                                {
                                    isIn = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (isIn)
                        return isIn;
                }
            }

            return isIn;
        }

        /// <summary>
        /// Judge if a point is in polygons
        /// </summary>
        /// <param name="polygons">polygons</param>
        /// <param name="aPoint">point</param>
        /// <returns>is in</returns>
        public static bool PointInPolygons(List<PolygonShape> polygons, PointD aPoint)
        {
            bool isIn = false;
            foreach (PolygonShape aPGS in polygons)
            {
                if (PointInPolygon(aPGS, aPoint))
                {
                    isIn = true;
                    break;
                }
            }

            return isIn;
        }

        /// <summary>
        /// Judge if a point is in a polygonLayer
        /// </summary>
        /// <param name="aLayer">a polygon layer</param>
        /// <param name="aPoint">a point</param>
        /// <param name="onlySel">if only use selected shapes</param>
        /// <returns>is in</returns>
        public static bool PointInPolygonLayer(VectorLayer aLayer, PointD aPoint, bool onlySel)
        {
            List<PolygonShape> polygons = new List<PolygonShape>();
            if (onlySel)
            {
                foreach (Shape.Shape aShape in aLayer.ShapeList)
                {
                    if (aShape.Selected)
                        polygons.Add((PolygonShape)aShape);
                }
            }
            else
            {
                foreach (Shape.Shape aShape in aLayer.ShapeList)
                    polygons.Add((PolygonShape)aShape);
            }

            return PointInPolygons(polygons, aPoint);
        }

        /// <summary>
        /// Calculate the distance between point and a line segment
        /// </summary>
        /// <param name="point">The point</param>
        /// <param name="pt1">End point of the line segment</param>
        /// <param name="pt2">End point of the line segment</param>
        /// <returns>Distance</returns>
        public static double dis_PointToLine(PointD point, PointD pt1, PointD pt2)
        {
            double dis;
            if (MIMath.DoubleEquals(pt2.X, pt1.X))
                dis = Math.Abs(point.X - pt1.X);
            else if (MIMath.DoubleEquals(pt2.Y, pt1.Y))
                dis = Math.Abs(point.Y - pt1.Y);
            else
            {
                double k = (pt2.Y - pt1.Y) / (pt2.X - pt1.X);
                double x = (k * k * pt1.X + k * (point.Y - pt1.Y) + point.X) / (k * k + 1);
                double y = k * (x - pt1.X) + pt1.Y;
                dis = distance(point, new PointD(x, y));
            }
            return dis;
        }

        /// <summary>
        /// Get distance between two point
        /// </summary>
        /// <param name="pt1">Point one</param>
        /// <param name="pt2">Point two</param>
        /// <returns>Distance</returns>
        public static double distance(PointD pt1, PointD pt2)
        {
            return Math.Sqrt((pt2.Y - pt1.Y) * (pt2.Y - pt1.Y) + (pt2.X - pt1.X) * (pt2.X - pt1.X));
        }

        /// <summary>
        /// Select polyline shape by a point 
        /// </summary>
        /// <param name="sp">The point</param>
        /// <param name="aPLS">The polyline shape</param>
        /// <param name="buffer">Buffer</param>
        /// <returns>Is the polyline shape selected</returns>
        public static bool SelectPolylineShape(PointD sp, PolylineShape aPLS, double buffer)
        {
            Extent aExtent = new Extent();
            aExtent.minX = sp.X - buffer;
            aExtent.maxX = sp.X + buffer;
            aExtent.minY = sp.Y - buffer;
            aExtent.maxY = sp.Y + buffer;
            bool isSelected = false;
            if (MIMath.IsExtentCross(aExtent, aPLS.Extent))
            {
                for (int j = 0; j < aPLS.PointNum; j++)
                {
                    PointD aPoint = aPLS.Points[j];
                    if (MIMath.PointInExtent(aPoint, aExtent))
                    {
                        isSelected = true;
                        break;
                    }
                    if (j < aPLS.PointNum - 1)
                    {
                        PointD bPoint = aPLS.Points[j + 1];
                        if (Math.Abs(sp.Y - aPoint.Y) <= Math.Abs(bPoint.Y - aPoint.Y)
                                || Math.Abs(sp.X - aPoint.X) <= Math.Abs(bPoint.X - aPoint.X))
                        {
                            if (GeoComputation.dis_PointToLine(sp, aPoint, bPoint) < aExtent.Width)
                            {
                                isSelected = true;
                                break;
                            }
                        }
                    }
                }
            }

            return isSelected;
        }

        #endregion

        #region Clipping

        /// <summary>
        /// Clip a layer
        /// </summary>
        /// <param name="oLayer">a layer</param>
        /// <param name="clipObj">clipping object</param>
        /// <returns>clipped layer</returns>
        public static VectorLayer ClipLayer(VectorLayer oLayer, object clipObj)
        {
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

            int i;
            List<Shape.Shape> newShapeList = new List<MeteoInfoC.Shape.Shape>();
            for (i = 0; i < aLayer.ShapeList.Count; i++)
            {
                Shape.Shape aShape = aLayer.ShapeList[i];
                aShape = ClipShape(aShape, clipObj);
                if (aShape != null)
                {
                    newShapeList.Add(aShape);
                    DataRow aDR = oLayer.AttributeTable.Table.Rows[i];
                    aLayer.AttributeTable.Table.ImportRow(aDR);
                }
            }

            aLayer.ShapeList = newShapeList;
            aLayer.UpdateExtent();

            return aLayer;
        }

        /// <summary>
        /// Clip a shape
        /// </summary>
        /// <param name="aShape">a shape</param>
        /// <param name="clipObj">clipping object</param>
        /// <returns>clipped shape</returns>
        public static Shape.Shape ClipShape(Shape.Shape aShape, object clipObj)
        {
            switch (aShape.ShapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes.WeatherSymbol:
                case ShapeTypes.WindArraw:
                case ShapeTypes.WindBarb:
                case ShapeTypes.StationModel:
                    return ClipPointShape((PointShape)aShape, clipObj);
                case ShapeTypes.Polyline:
                    return ClipPolylineShape((PolylineShape)aShape, clipObj);
                case ShapeTypes.Polygon:
                case ShapeTypes.PolygonM:
                    return ClipPolygonShape((PolygonShape)aShape, clipObj);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Clip point shape with a clipping object
        /// </summary>
        /// <param name="aPS">point shape</param>
        /// <param name="clipObj">clipping object</param>
        /// <returns>clipped point shape</returns>
        public static PointShape ClipPointShape(PointShape aPS, object clipObj)
        {
            if (PointInClipObj(clipObj, aPS.Point))
                return aPS;
            else
                return null;
        }

        /// <summary>
        /// Clip polygon shape with a clipping object
        /// </summary>
        /// <param name="aPGS">polygon shape</param>
        /// <param name="clipObj">clipping object</param>
        /// <returns>clipped polygon shape</returns>
        public static PolygonShape ClipPolygonShape(PolygonShape aPGS, object clipObj)
        {
            List<Polygon> polygons = ClipPolygons(aPGS.Polygons, clipObj);
            if (polygons.Count == 0)
                return null;
            else
            {
                PolygonShape bPGS = aPGS.ValueClone();
                bPGS.Polygons = polygons;

                return bPGS;
            }
        }

        /// <summary>
        /// Clip polygon shape with a longitude
        /// </summary>
        /// <param name="aPGS">polygon shape</param>
        /// <param name="lon">longitude</param>
        /// <returns>clipped polygon shape</returns>
        public static PolygonShape ClipPolygonShape_Lon(PolygonShape aPGS, double lon)
        {
            List<Polygon> polygons = new List<Polygon>();
            ClipLine clipLine = new ClipLine();
            clipLine.IsLon = true;
            clipLine.Value = lon - 0.0001;
            clipLine.IsLeftOrTop = true;
            polygons.AddRange(ClipPolygons(aPGS.Polygons, clipLine));

            clipLine.Value = lon + 0.0001;
            clipLine.IsLeftOrTop = false;
            polygons.AddRange(ClipPolygons(aPGS.Polygons, clipLine));

            PolygonShape bPGS = aPGS.ValueClone();
            bPGS.Polygons = polygons;

            return bPGS;
        }

        /// <summary>
        /// Clip polygon shape with a latitude
        /// </summary>
        /// <param name="aPGS">polygon shape</param>
        /// <param name="lat">latitude</param>
        /// <returns>clipped polygon shape</returns>
        public static PolygonShape ClipPolygonShape_Lat(PolygonShape aPGS, double lat)
        {
            List<Polygon> polygons = new List<Polygon>();
            ClipLine clipLine = new ClipLine();
            clipLine.IsLon = false;
            clipLine.Value = lat + 0.0001;
            clipLine.IsLeftOrTop = true;
            polygons.AddRange(ClipPolygons(aPGS.Polygons, clipLine));

            clipLine.Value = lat - 0.0001;
            clipLine.IsLeftOrTop = false;
            polygons.AddRange(ClipPolygons(aPGS.Polygons, clipLine));

            PolygonShape bPGS = aPGS.ValueClone();
            bPGS.Polygons = polygons;

            return bPGS;
        }

        /// <summary>
        /// Clip polygon shape with a latitude
        /// </summary>
        /// <param name="aPGS">polygon shape</param>
        /// <param name="lat">latitude</param>
        /// <param name="isTop">if is top</param>
        /// <returns>clipped polygon shape</returns>
        public static PolygonShape ClipPolygonShape_Lat(PolygonShape aPGS, double lat, bool isTop)
        {
            List<Polygon> polygons = new List<Polygon>();
            ClipLine clipLine = new ClipLine();
            clipLine.IsLon = false;
            clipLine.Value = lat;
            clipLine.IsLeftOrTop = isTop;
            polygons.AddRange(ClipPolygons(aPGS.Polygons, clipLine));            

            PolygonShape bPGS = aPGS.ValueClone();
            bPGS.Polygons = polygons;

            return bPGS;
        }

        /// <summary>
        /// Clip polyline shape with a clipping object
        /// </summary>
        /// <param name="aPLS">polyline shape</param>
        /// <param name="clipObj">clipping object</param>
        /// <returns>clipped polyline shape</returns>
        public static PolylineShape ClipPolylineShape(PolylineShape aPLS, object clipObj)
        {
            List<PolyLine> polyLines = ClipPolyLines(aPLS.PolyLines, clipObj);
            if (polyLines.Count == 0)
                return null;
            else
            {
                PolylineShape bPLS = aPLS.ValueClone();
                bPLS.PolyLines = polyLines;

                return bPLS;
            }
        }

        /// <summary>
        /// Clip polyline shape with a longitude
        /// </summary>
        /// <param name="aPLS">polyline shape</param>
        /// <param name="lon">longitude</param>
        /// <returns>clipped polyline shape</returns>
        public static PolylineShape ClipPolylineShape_Lon(PolylineShape aPLS, double lon)
        {
            List<PolyLine> polylines = new List<PolyLine>();
            ClipLine clipLine = new ClipLine();
            clipLine.IsLon = true;
            clipLine.Value = lon - 0.0001;
            clipLine.IsLeftOrTop = true;
            polylines.AddRange(ClipPolyLines(aPLS.PolyLines, clipLine));

            clipLine.Value = lon + 0.0001;
            clipLine.IsLeftOrTop = false;
            polylines.AddRange(ClipPolyLines(aPLS.PolyLines, clipLine));            

            PolylineShape bPLS = aPLS.ValueClone();
            bPLS.PolyLines = polylines;

            return bPLS;
        }

        ///// <summary>
        ///// Clip polyline Z shape with a longitude
        ///// </summary>
        ///// <param name="aPLS">polyline Z shape</param>
        ///// <param name="lon">longitude</param>
        ///// <returns>clipped polyline Z shape</returns>
        //public static PolylineZShape ClipPolylineShape_Lon(PolylineZShape aPLS, double lon)
        //{
        //    List<PolylineZ> polylines = new List<PolylineZ>();
        //    ClipLine clipLine = new ClipLine();
        //    clipLine.IsLon = true;
        //    clipLine.Value = lon - 0.0001;
        //    clipLine.IsLeftOrTop = true;
        //    polylines.AddRange(ClipPolyLines(aPLS.Polylines, clipLine));

        //    clipLine.Value = lon + 0.0001;
        //    clipLine.IsLeftOrTop = false;
        //    polylines.AddRange(ClipPolyLines(aPLS.Polylines, clipLine));

        //    PolylineZShape bPLS = aPLS.ValueClone();
        //    bPLS.Polylines = polylines;

        //    return bPLS;
        //}

        /// <summary>
        /// Clip polyline shape with a longitude
        /// </summary>
        /// <param name="aPLS">polyline shape</param>
        /// <param name="lat">latitude</param>
        /// <returns>clipped polyline shape</returns>
        public static PolylineShape ClipPolylineShape_Lat(PolylineShape aPLS, double lat)
        {
            List<PolyLine> polylines = new List<PolyLine>();
            ClipLine clipLine = new ClipLine();
            clipLine.IsLon = false;
            clipLine.Value = lat + 0.0001;
            clipLine.IsLeftOrTop = true;
            polylines.AddRange(ClipPolyLines(aPLS.PolyLines, clipLine));

            clipLine.Value = lat - 0.0001;
            clipLine.IsLeftOrTop = false;
            polylines.AddRange(ClipPolyLines(aPLS.PolyLines, clipLine));

            PolylineShape bPLS = aPLS.ValueClone();
            bPLS.PolyLines = polylines;

            return bPLS;
        }

        /// <summary>
        /// Clip polyline shape with a longitude
        /// </summary>
        /// <param name="aPLS">polyline shape</param>
        /// <param name="lat">latitude</param>
        /// <param name="isTop">if is top</param>
        /// <returns>clipped polyline shape</returns>
        public static PolylineShape ClipPolylineShape_Lat(PolylineShape aPLS, double lat, bool isTop)
        {
            List<PolyLine> polylines = new List<PolyLine>();
            ClipLine clipLine = new ClipLine();
            clipLine.IsLon = false;
            clipLine.Value = lat;
            clipLine.IsLeftOrTop = isTop;
            polylines.AddRange(ClipPolyLines(aPLS.PolyLines, clipLine));            

            PolylineShape bPLS = aPLS.ValueClone();
            bPLS.PolyLines = polylines;

            return bPLS;
        }

        ///// <summary>
        ///// Clip polylineZ shape with a longitude
        ///// </summary>
        ///// <param name="aPLS">polylineZ shape</param>
        ///// <param name="lat">latitude</param>
        ///// <param name="isTop">if is top</param>
        ///// <returns>clipped polyline shape</returns>
        //public static PolylineZShape ClipPolylineShape_Lat(PolylineZShape aPLS, double lat, bool isTop)
        //{
        //    List<PolylineZ> polylines = new List<PolylineZ>();
        //    ClipLine clipLine = new ClipLine();
        //    clipLine.IsLon = false;
        //    clipLine.Value = lat;
        //    clipLine.IsLeftOrTop = isTop;
        //    polylines.AddRange(ClipPolyLines(aPLS.Polylines, clipLine));

        //    PolylineZShape bPLS = aPLS.ValueClone();
        //    bPLS.Polylines = polylines;

        //    return bPLS;
        //}

        /// <summary>
        /// Get grid labels of a polyline
        /// </summary>
        /// <param name="inPolyLine">polyline</param>
        /// <param name="clipExtent">clipping object</param>
        /// <param name="isVertical">if is vertical</param>
        /// <returns>clip points</returns>
        public static List<GridLabel> GetGridLabels(PolyLine inPolyLine, Extent clipExtent, bool isVertical)
        {
            List<GridLabel> gridLabels = new List<GridLabel>();
            List<PointD> aPList = inPolyLine.PointList;

            if (!IsExtentCross(inPolyLine.Extent, clipExtent))
                return gridLabels;

            int i, j;            
            //Judge if all points of the polyline are in the cut polygon - outline   
            List<List<PointD>> newLines = new List<List<PointD>>();
            bool isReversed = false;
            if (PointInClipObj(clipExtent, aPList[0]))
            {
                bool isAllIn = true;
                int notInIdx = 0;
                for (i = 0; i < aPList.Count; i++)
                {
                    if (!PointInClipObj(clipExtent, aPList[i]))
                    {
                        notInIdx = i;
                        isAllIn = false;
                        break;
                    }
                }
                if (!isAllIn)   //Put start point outside of the cut polygon
                {
                    if (inPolyLine.IsClosed())
                    {
                        List<PointD> bPList = new List<PointD>();
                        bPList.AddRange(aPList.GetRange(notInIdx, aPList.Count - notInIdx));
                        bPList.AddRange(aPList.GetRange(1, notInIdx - 1));
                        //for (i = notInIdx; i < aPList.Count; i++)
                        //    bPList.Add(aPList[i]);

                        //for (i = 1; i < notInIdx; i++)
                        //    bPList.Add(aPList[i]);

                        bPList.Add(bPList[0]);
                        //if (!IsClockwise(bPList))
                        //    bPList.Reverse();
                        newLines.Add(bPList);
                    }
                    else
                    {
                        aPList.Reverse();
                        newLines.Add(aPList);
                        isReversed = true;
                    }
                }
                else    //the input polygon is inside the cut polygon
                {
                    GridLabel aGL = new GridLabel();
                    aGL.IsLon = isVertical;
                    aGL.IsBorder = false;
                    aGL.LabPoint = aPList[0];
                    if (isVertical)
                        aGL.LabDirection = Direction.South;
                    else
                        aGL.LabDirection = Direction.Weast;
                    gridLabels.Add(aGL);

                    aGL = new GridLabel();
                    aGL.IsLon = isVertical;
                    aGL.IsBorder = false;
                    aGL.LabPoint = aPList[aPList.Count - 1];
                    if (isVertical)
                        aGL.LabDirection = Direction.North;
                    else
                        aGL.LabDirection = Direction.East;
                    gridLabels.Add(aGL);

                    return gridLabels;
                }
            }
            else
            {
                newLines.Add(aPList);
            }

            //Prepare border point list
            List<BorderPoint> borderList = new List<BorderPoint>();
            BorderPoint aBP = new BorderPoint();
            List<PointD> clipPList = GetClipPointList(clipExtent);
            foreach (PointD aP in clipPList)
            {
                aBP = new BorderPoint();
                aBP.Point = aP;
                aBP.Id = -1;
                borderList.Add(aBP);
            }

            //Cutting                     
            for (int l = 0; l < newLines.Count; l++)
            {
                aPList = newLines[l];
                bool isInPolygon = PointInClipObj(clipExtent, aPList[0]);
                PointD q1, q2, p1, p2, IPoint = new PointD();
                Line lineA, lineB;
                List<PointD> newPlist = new List<PointD>();
                PolyLine bLine = new PolyLine();
                p1 = aPList[0];
                int inIdx = -1, outIdx = -1;
                //bool newLine = true;
                int a1 = 0;
                for (i = 1; i < aPList.Count; i++)
                {
                    p2 = aPList[i];
                    if (PointInClipObj(clipExtent, p2))
                    {
                        if (!isInPolygon)
                        {
                            lineA.P1 = p1;
                            lineA.P2 = p2;
                            //q1 = borderList[borderList.Count - 1].Point;
                            q1 = borderList[0].Point;
                            for (j = 1; j < borderList.Count; j++)
                            {
                                q2 = borderList[j].Point;
                                lineB.P1 = q1;
                                lineB.P2 = q2;
                                if (IsLineSegmentCross(lineA, lineB))
                                {
                                    IPoint = GetCrossPoint(lineA, lineB);                                    
                                    inIdx = j;
                                    break;
                                }
                                q1 = q2;
                            }
                            GridLabel aGL = new GridLabel();
                            aGL.IsLon = isVertical;
                            aGL.IsBorder = true;
                            aGL.LabPoint = IPoint;
                            if (MIMath.DoubleEquals(q1.X, borderList[j].Point.X))
                            {
                                if (MIMath.DoubleEquals(q1.X, clipExtent.minX))
                                    aGL.LabDirection = Direction.Weast;
                                else
                                    aGL.LabDirection = Direction.East;
                            }
                            else
                            {
                                if (MIMath.DoubleEquals(q1.Y, clipExtent.minY))
                                    aGL.LabDirection = Direction.South;
                                else
                                    aGL.LabDirection = Direction.North;
                            }

                            if (isVertical)
                            {
                                if (aGL.LabDirection == Direction.South || aGL.LabDirection == Direction.North)
                                    gridLabels.Add(aGL);
                            }
                            else
                            {
                                if (aGL.LabDirection == Direction.East || aGL.LabDirection == Direction.Weast)
                                    gridLabels.Add(aGL);
                            }

                        }
                        newPlist.Add(aPList[i]);
                        isInPolygon = true;
                    }
                    else
                    {
                        if (isInPolygon)
                        {
                            lineA.P1 = p1;
                            lineA.P2 = p2;
                            //q1 = borderList[borderList.Count - 1].Point;
                            q1 = borderList[0].Point;
                            for (j = 1; j < borderList.Count; j++)
                            {
                                q2 = borderList[j].Point;
                                lineB.P1 = q1;
                                lineB.P2 = q2;
                                if (IsLineSegmentCross(lineA, lineB))
                                {                                    
                                    IPoint = GetCrossPoint(lineA, lineB);                                                                        
                                    outIdx = j;
                                    a1 = inIdx;

                                    //newLine = false;
                                    break;
                                }
                                q1 = q2;
                            }
                            GridLabel aGL = new GridLabel();
                            aGL.IsBorder = true;
                            aGL.IsLon = isVertical;
                            aGL.LabPoint = IPoint;
                            if (MIMath.DoubleEquals(q1.X, borderList[j].Point.X))
                            {
                                if (MIMath.DoubleEquals(q1.X, clipExtent.minX))
                                    aGL.LabDirection = Direction.Weast;
                                else
                                    aGL.LabDirection = Direction.East;
                            }
                            else
                            {
                                if (MIMath.DoubleEquals(q1.Y, clipExtent.minY))
                                    aGL.LabDirection = Direction.South;
                                else
                                    aGL.LabDirection = Direction.North;
                            }

                            if (isVertical)
                            {
                                if (aGL.LabDirection == Direction.South || aGL.LabDirection == Direction.North)
                                    gridLabels.Add(aGL);
                            }
                            else
                            {
                                if (aGL.LabDirection == Direction.East || aGL.LabDirection == Direction.Weast)
                                    gridLabels.Add(aGL);
                            }

                            isInPolygon = false;
                            newPlist = new List<PointD>();
                        }
                    }
                    p1 = p2;
                }

                if (isInPolygon && newPlist.Count > 1)
                {
                    GridLabel aGL = new GridLabel();
                    aGL.IsLon = isVertical;
                    aGL.IsBorder = false;
                    aGL.LabPoint = newPlist[newPlist.Count - 1];
                    if (isVertical)
                    {
                        if (isReversed)
                            aGL.LabDirection = Direction.South;
                        else
                            aGL.LabDirection = Direction.North;
                    }
                    else
                    {
                        if (isReversed)
                            aGL.LabDirection = Direction.Weast;
                        else
                            aGL.LabDirection = Direction.East;
                    }
                    
                    gridLabels.Add(aGL);                    
                }
            }

            return gridLabels;
        }

        /// <summary>
        /// Get grid labels of a straight line
        /// </summary>
        /// <param name="inPolyLine">polyline</param>
        /// <param name="clipExtent">clipping object</param>
        /// <param name="isVertical">if is vertical</param>
        /// <returns>clip points</returns>
        public static List<GridLabel> GetGridLabels_StraightLine(PolyLine inPolyLine, Extent clipExtent, bool isVertical)
        {
            List<GridLabel> gridLabels = new List<GridLabel>();
            List<PointD> aPList = inPolyLine.PointList;

            PointD aPoint = inPolyLine.PointList[0];
            if (isVertical)
            {
                if (aPoint.X < clipExtent.minX || aPoint.X > clipExtent.maxX)
                    return gridLabels;

                GridLabel aGL = new GridLabel();
                aGL.LabDirection = Direction.South;
                aGL.LabPoint = new PointD(aPoint.X, clipExtent.minY);
                gridLabels.Add(aGL);
            }
            else
            {
                if (aPoint.Y < clipExtent.minY || aPoint.Y > clipExtent.maxY)
                    return gridLabels;

                GridLabel aGL = new GridLabel();
                aGL.LabDirection = Direction.Weast;
                aGL.LabPoint = new PointD(clipExtent.minX, aPoint.Y);
                gridLabels.Add(aGL);                
            }

            aPoint = inPolyLine.PointList[inPolyLine.PointList.Count - 1];
            if (isVertical)
            {
                if (aPoint.X < clipExtent.minX || aPoint.X > clipExtent.maxX)
                    return gridLabels;

                GridLabel aGL = new GridLabel();
                aGL.LabDirection = Direction.North;
                aGL.LabPoint = new PointD(aPoint.X, clipExtent.maxY);
                gridLabels.Add(aGL);
            }
            else
            {
                if (aPoint.Y < clipExtent.minY || aPoint.Y > clipExtent.maxY)
                    return gridLabels;

                GridLabel aGL = new GridLabel();
                aGL.LabDirection = Direction.East;
                aGL.LabPoint = new PointD(clipExtent.maxX, aPoint.Y);
                gridLabels.Add(aGL);
            }

            return gridLabels;
        }        

        /// <summary>
        /// Clip polygons with a clipping object
        /// </summary>
        /// <param name="polygons">polygon list</param>
        /// <param name="clipObj">clipping object</param>
        /// <returns>clipped polygons</returns>
        private static List<Polygon> ClipPolygons(List<Polygon> polygons, object clipObj)
        {
            List<Polygon> newPolygons = new List<Polygon>();
            for (int i = 0; i < polygons.Count; i++)
            {
                Polygon aPolygon = polygons[i];
                newPolygons.AddRange(ClipPolygon(aPolygon, clipObj));                
            }
            
            return newPolygons;
        }

        /// <summary>
        /// Clip polylines with a clipping object
        /// </summary>
        /// <param name="polyLines">polyline list</param>
        /// <param name="clipObj">clipping object</param>
        /// <returns>clipped polylines</returns>
        private static List<PolyLine> ClipPolyLines(List<PolyLine> polyLines, object clipObj)
        {
            List<PolyLine> newPolyLines = new List<PolyLine>();
            for (int i = 0; i < polyLines.Count; i++)
            {
                PolyLine aPolyLine = polyLines[i];
                newPolyLines.AddRange(ClipPolyLine(aPolyLine, clipObj));
            }

            return newPolyLines;
        }

        ///// <summary>
        ///// Clip polylines with a clipping object
        ///// </summary>
        ///// <param name="polyLines">polyline list</param>
        ///// <param name="clipObj">clipping object</param>
        ///// <returns>clipped polylines</returns>
        //private static List<PolylineZ> ClipPolyLines(List<PolylineZ> polyLines, object clipObj)
        //{
        //    List<PolylineZ> newPolyLines = new List<PolylineZ>();
        //    for (int i = 0; i < polyLines.Count; i++)
        //    {
        //        PolylineZ aPolyLine = polyLines[i];
        //        newPolyLines.AddRange(ClipPolyLine(aPolyLine, clipObj));
        //    }

        //    return newPolyLines;
        //}

        //private static List<Polygon> ClipPolygon_Hole(Polygon inPolygon, List<PointD> clipPList)
        //{
        //    List<Polygon> newPolygons = new List<Polygon>();
        //    List<PolyLine> newPolylines = new List<PolyLine>();
        //    List<PointD> aPList = inPolygon.OutLine;
        //    Extent plExtent = MIMath.GetPointsExtent(aPList);
        //    Extent cutExtent = MIMath.GetPointsExtent(clipPList);

        //    if (!MIMath.IsExtentCross(plExtent, cutExtent))
        //        return newPolygons;

        //    int i, j;

        //    if (!IsClockwise(clipPList))    //---- Make cut polygon clockwise
        //        clipPList.Reverse();

        //    //Judge if all points of the polyline are in the cut polygon - outline   
        //    List<List<PointD>> newLines = new List<List<PointD>>();
        //    if (PointInPolygon(clipPList, aPList[0]))
        //    {
        //        bool isAllIn = true;
        //        int notInIdx = 0;
        //        for (i = 0; i < aPList.Count; i++)
        //        {
        //            if (!PointInPolygon(clipPList, aPList[i]))
        //            {
        //                notInIdx = i;
        //                isAllIn = false;
        //                break;
        //            }
        //        }
        //        if (!isAllIn)   //Put start point outside of the cut polygon
        //        {
        //            List<PointD> bPList = new List<PointD>();
        //            bPList.AddRange(aPList.GetRange(notInIdx, aPList.Count - notInIdx));
        //            bPList.AddRange(aPList.GetRange(1, notInIdx - 1));
        //            //for (i = notInIdx; i < aPList.Count; i++)
        //            //    bPList.Add(aPList[i]);

        //            //for (i = 1; i < notInIdx; i++)
        //            //    bPList.Add(aPList[i]);

        //            bPList.Add(bPList[0]);
        //            //if (!IsClockwise(bPList))
        //            //    bPList.Reverse();
        //            newLines.Add(bPList);
        //        }
        //        else    //the input polygon is inside the cut polygon
        //        {
        //            newPolygons.Add(inPolygon);
        //            return newPolygons;
        //        }
        //    }
        //    else
        //    {
        //        newLines.Add(aPList);
        //    }

        //    //Holes
        //    List<List<PointD>> holeLines = new List<List<PointD>>();
        //    for (int h = 0; h < inPolygon.HoleLines.Count; h++)
        //    {
        //        List<PointD> holePList = inPolygon.HoleLines[h];
        //        plExtent = MIMath.GetPointsExtent(holePList);
        //        if (!MIMath.IsExtentCross(plExtent, cutExtent))
        //            continue;

        //        if (PointInPolygon(clipPList, holePList[0]))
        //        {
        //            bool isAllIn = true;
        //            int notInIdx = 0;
        //            for (i = 0; i < holePList.Count; i++)
        //            {
        //                if (!PointInPolygon(clipPList, holePList[i]))
        //                {
        //                    notInIdx = i;
        //                    isAllIn = false;
        //                    break;
        //                }
        //            }
        //            if (!isAllIn)   //Put start point outside of the cut polygon
        //            {
        //                List<PointD> bPList = new List<PointD>();
        //                bPList.AddRange(holePList.GetRange(notInIdx, holePList.Count - notInIdx));
        //                bPList.AddRange(holePList.GetRange(1, notInIdx - 1));
        //                //for (i = notInIdx; i < aPList.Count; i++)
        //                //    bPList.Add(aPList[i]);

        //                //for (i = 1; i < notInIdx; i++)
        //                //    bPList.Add(aPList[i]);

        //                bPList.Add(bPList[0]);
        //                newLines.Add(bPList);
        //            }
        //            else    //the hole is inside the cut polygon
        //            {
        //                holeLines.Add(inPolygon.HoleLines[h]);
        //            }
        //        }
        //        else
        //            newLines.Add(holePList);
        //    }

        //    //Prepare border point list
        //    List<BorderPoint> borderList = new List<BorderPoint>();
        //    BorderPoint aBP = new BorderPoint();
        //    foreach (PointD aP in clipPList)
        //    {
        //        aBP = new BorderPoint();
        //        aBP.Point = aP;
        //        aBP.Id = -1;
        //        borderList.Add(aBP);
        //    }

        //    //Cutting                     
        //    for (int l = 0; l < newLines.Count; l++)
        //    {
        //        aPList = newLines[l];
        //        bool isInPolygon = false;
        //        PointD q1, q2, p1, p2, IPoint = new PointD();
        //        Line lineA, lineB;
        //        List<PointD> newPlist = new List<PointD>();
        //        PolyLine bLine = new PolyLine();
        //        p1 = aPList[0];
        //        int inIdx = -1, outIdx = -1;
        //        bool newLine = true;
        //        int a1 = 0;
        //        for (i = 1; i < aPList.Count; i++)
        //        {
        //            p2 = aPList[i];
        //            if (PointInPolygon(clipPList, p2))
        //            {
        //                if (!isInPolygon)
        //                {
        //                    lineA.P1 = p1;
        //                    lineA.P2 = p2;
        //                    q1 = borderList[borderList.Count - 1].Point;
        //                    for (j = 0; j < borderList.Count; j++)
        //                    {
        //                        q2 = borderList[j].Point;
        //                        lineB.P1 = q1;
        //                        lineB.P2 = q2;
        //                        if (IsLineSegmentCross(lineA, lineB))
        //                        {
        //                            IPoint = GetCrossPoint(lineA, lineB);
        //                            aBP = new BorderPoint();
        //                            aBP.Id = newPolylines.Count;
        //                            aBP.Point = IPoint;
        //                            borderList.Insert(j, aBP);
        //                            inIdx = j;
        //                            break;
        //                        }
        //                        q1 = q2;
        //                    }
        //                    newPlist.Add(IPoint);
        //                }
        //                newPlist.Add(aPList[i]);
        //                isInPolygon = true;
        //            }
        //            else
        //            {
        //                if (isInPolygon)
        //                {
        //                    lineA.P1 = p1;
        //                    lineA.P2 = p2;
        //                    q1 = borderList[borderList.Count - 1].Point;
        //                    for (j = 0; j < borderList.Count; j++)
        //                    {
        //                        q2 = borderList[j].Point;
        //                        lineB.P1 = q1;
        //                        lineB.P2 = q2;
        //                        if (IsLineSegmentCross(lineA, lineB))
        //                        {
        //                            if (!newLine)
        //                            {
        //                                if (inIdx - outIdx >= 1 && inIdx - outIdx <= 10)
        //                                {
        //                                    if (!TwoPointsInside(a1, outIdx, inIdx, j))
        //                                    {
        //                                        borderList.RemoveAt(inIdx);
        //                                        borderList.Insert(outIdx, aBP);
        //                                    }
        //                                }
        //                                else if (inIdx - outIdx <= -1 && inIdx - outIdx >= -10)
        //                                {
        //                                    if (!TwoPointsInside(a1, outIdx, inIdx, j))
        //                                    {
        //                                        borderList.RemoveAt(inIdx);
        //                                        borderList.Insert(outIdx + 1, aBP);
        //                                    }
        //                                }
        //                                else if (inIdx == outIdx)
        //                                {
        //                                    if (!TwoPointsInside(a1, outIdx, inIdx, j))
        //                                    {
        //                                        borderList.RemoveAt(inIdx);
        //                                        borderList.Insert(inIdx + 1, aBP);
        //                                    }
        //                                }
        //                            }
        //                            IPoint = GetCrossPoint(lineA, lineB);
        //                            aBP = new BorderPoint();
        //                            aBP.Id = newPolylines.Count;
        //                            aBP.Point = IPoint;
        //                            borderList.Insert(j, aBP);
        //                            outIdx = j;
        //                            a1 = inIdx;

        //                            newLine = false;
        //                            break;
        //                        }
        //                        q1 = q2;
        //                    }
        //                    newPlist.Add(IPoint);

        //                    bLine = new PolyLine();
        //                    //bLine.Value = inPolygon.OutLine.Value;
        //                    //bLine.Type = inPolygon.OutLine.Type;
        //                    bLine.PointList = new List<PointD>(newPlist);
        //                    newPolylines.Add(bLine);

        //                    isInPolygon = false;
        //                    newPlist = new List<PointD>();
        //                }
        //            }
        //            p1 = p2;
        //        }
        //    }

        //    if (newPolylines.Count > 0)
        //    {
        //        //Tracing polygons
        //        newPolygons = TracingClipPolygons(inPolygon, newPolylines, borderList);
        //    }
        //    else
        //    {
        //        if (PointInPolygon(aPList, clipPList[0]))
        //        {
        //            Extent aBound = new Extent();
        //            Polygon aPolygon = new Polygon();                    
        //            aPolygon.OutLine = new List<PointD>(clipPList);                    
        //            aPolygon.HoleLines = new List<List<PointD>>();

        //            newPolygons.Add(aPolygon);
        //        }
        //    }

        //    if (holeLines.Count > 0)
        //    {
        //        AddHoles_Ring(ref newPolygons, holeLines);
        //    }

        //    return newPolygons;
        //}

        private static List<Polygon> ClipPolygon(Polygon inPolygon, List<PointD> clipPList)
        {
            List<Polygon> newPolygons = new List<Polygon>();
            List<PolyLine> newPolylines = new List<PolyLine>();
            List<PointD> aPList = inPolygon.OutLine;
            Extent plExtent = MIMath.GetPointsExtent(aPList);
            Extent cutExtent = MIMath.GetPointsExtent(clipPList);

            if (!MIMath.IsExtentCross(plExtent, cutExtent))
                return newPolygons;

            int i, j;

            if (!IsClockwise(clipPList))    //---- Make cut polygon clockwise
                clipPList.Reverse();

            //Judge if all points of the polyline are in the cut polygon - outline   
            List<List<PointD>> newLines = new List<List<PointD>>();
            if (PointInPolygon(clipPList, aPList[0]))
            {
                bool isAllIn = true;
                int notInIdx = 0;
                for (i = 0; i < aPList.Count; i++)
                {
                    if (!PointInPolygon(clipPList, aPList[i]))
                    {
                        notInIdx = i;
                        isAllIn = false;
                        break;
                    }
                }
                if (!isAllIn)   //Put start point outside of the cut polygon
                {
                    List<PointD> bPList = new List<PointD>();
                    bPList.AddRange(aPList.GetRange(notInIdx, aPList.Count - notInIdx));
                    bPList.AddRange(aPList.GetRange(1, notInIdx - 1));
                    //for (i = notInIdx; i < aPList.Count; i++)
                    //    bPList.Add(aPList[i]);

                    //for (i = 1; i < notInIdx; i++)
                    //    bPList.Add(aPList[i]);

                    bPList.Add(bPList[0]);
                    //if (!IsClockwise(bPList))
                    //    bPList.Reverse();
                    newLines.Add(bPList);
                }
                else    //the input polygon is inside the cut polygon
                {
                    newPolygons.Add(inPolygon);
                    return newPolygons;
                }
            }
            else
            {
                newLines.Add(aPList);
            }

            //Holes
            List<List<PointD>> holeLines = new List<List<PointD>>();
            if (inPolygon.HasHole)
            {                
                for (int h = 0; h < inPolygon.HoleLines.Count; h++)
                {
                    List<PointD> holePList = inPolygon.HoleLines[h];
                    plExtent = MIMath.GetPointsExtent(holePList);
                    if (!MIMath.IsExtentCross(plExtent, cutExtent))
                        continue;

                    if (PointInPolygon(clipPList, holePList[0]))
                    {
                        bool isAllIn = true;
                        int notInIdx = 0;
                        for (i = 0; i < holePList.Count; i++)
                        {
                            if (!PointInPolygon(clipPList, holePList[i]))
                            {
                                notInIdx = i;
                                isAllIn = false;
                                break;
                            }
                        }
                        if (!isAllIn)   //Put start point outside of the cut polygon
                        {
                            List<PointD> bPList = new List<PointD>();
                            bPList.AddRange(holePList.GetRange(notInIdx, holePList.Count - notInIdx));
                            bPList.AddRange(holePList.GetRange(1, notInIdx - 1));
                            //for (i = notInIdx; i < aPList.Count; i++)
                            //    bPList.Add(aPList[i]);

                            //for (i = 1; i < notInIdx; i++)
                            //    bPList.Add(aPList[i]);

                            bPList.Add(bPList[0]);
                            newLines.Add(bPList);
                        }
                        else    //the hole is inside the cut polygon
                        {
                            holeLines.Add(inPolygon.HoleLines[h]);
                        }
                    }
                    else
                        newLines.Add(holePList);
                }
            }

            //Prepare border point list
            List<BorderPoint> borderList = new List<BorderPoint>();
            BorderPoint aBP = new BorderPoint();
            foreach (PointD aP in clipPList)
            {
                aBP = new BorderPoint();
                aBP.Point = aP;
                aBP.Id = -1;
                borderList.Add(aBP);
            }

            //Cutting                     
            for (int l = 0; l < newLines.Count; l++)
            {
                aPList = newLines[l];
                bool isInPolygon = false;
                PointD q1, q2, p1, p2, IPoint = new PointD();
                Line lineA, lineB;
                List<PointD> newPlist = new List<PointD>();
                PolyLine bLine = new PolyLine();
                p1 = aPList[0];
                int inIdx = -1, outIdx = -1;
                bool newLine = true;
                int a1 = 0;
                for (i = 1; i < aPList.Count; i++)
                {
                    p2 = aPList[i];
                    if (PointInPolygon(clipPList, p2))
                    {
                        if (!isInPolygon)
                        {
                            lineA.P1 = p1;
                            lineA.P2 = p2;
                            q1 = borderList[borderList.Count - 1].Point;
                            for (j = 0; j < borderList.Count; j++)
                            {
                                q2 = borderList[j].Point;
                                lineB.P1 = q1;
                                lineB.P2 = q2;
                                if (IsLineSegmentCross(lineA, lineB))
                                {
                                    IPoint = GetCrossPoint(lineA, lineB);
                                    aBP = new BorderPoint();
                                    aBP.Id = newPolylines.Count;
                                    aBP.Point = IPoint;
                                    borderList.Insert(j, aBP);
                                    inIdx = j;
                                    break;
                                }
                                q1 = q2;
                            }
                            newPlist.Add(IPoint);
                        }
                        newPlist.Add(aPList[i]);
                        isInPolygon = true;
                    }
                    else
                    {
                        if (isInPolygon)
                        {
                            lineA.P1 = p1;
                            lineA.P2 = p2;
                            q1 = borderList[borderList.Count - 1].Point;
                            for (j = 0; j < borderList.Count; j++)
                            {
                                q2 = borderList[j].Point;
                                lineB.P1 = q1;
                                lineB.P2 = q2;
                                if (IsLineSegmentCross(lineA, lineB))
                                {
                                    if (!newLine)
                                    {
                                        if (inIdx - outIdx >= 1 && inIdx - outIdx <= 10)
                                        {
                                            if (!TwoPointsInside(a1, outIdx, inIdx, j))
                                            {
                                                borderList.RemoveAt(inIdx);
                                                borderList.Insert(outIdx, aBP);
                                            }
                                        }
                                        else if (inIdx - outIdx <= -1 && inIdx - outIdx >= -10)
                                        {
                                            if (!TwoPointsInside(a1, outIdx, inIdx, j))
                                            {
                                                borderList.RemoveAt(inIdx);
                                                borderList.Insert(outIdx + 1, aBP);
                                            }
                                        }
                                        else if (inIdx == outIdx)
                                        {
                                            if (!TwoPointsInside(a1, outIdx, inIdx, j))
                                            {
                                                borderList.RemoveAt(inIdx);
                                                borderList.Insert(inIdx + 1, aBP);
                                            }
                                        }
                                    }
                                    IPoint = GetCrossPoint(lineA, lineB);
                                    aBP = new BorderPoint();
                                    aBP.Id = newPolylines.Count;
                                    aBP.Point = IPoint;
                                    borderList.Insert(j, aBP);
                                    outIdx = j;
                                    a1 = inIdx;

                                    newLine = false;
                                    break;
                                }
                                q1 = q2;
                            }
                            newPlist.Add(IPoint);

                            bLine = new PolyLine();
                            //bLine.Value = inPolygon.OutLine.Value;
                            //bLine.Type = inPolygon.OutLine.Type;
                            bLine.PointList = new List<PointD>(newPlist);
                            newPolylines.Add(bLine);

                            isInPolygon = false;
                            newPlist = new List<PointD>();
                        }
                    }
                    p1 = p2;
                }
            }

            if (newPolylines.Count > 0)
            {
                //Tracing polygons
                newPolygons = TracingClipPolygons(inPolygon, newPolylines, borderList);                
            }
            else
            {
                if (PointInPolygon(aPList, clipPList[0]))
                {                    
                    Polygon aPolygon = new Polygon();
                    aPolygon.OutLine = new List<PointD>(clipPList);
                    aPolygon.HoleLines = new List<List<PointD>>();

                    newPolygons.Add(aPolygon);
                }
            }

            if (holeLines.Count > 0)
            {
                AddHoles_Ring(ref newPolygons, holeLines);
            }

            return newPolygons;
        }        

        private static List<Polygon> ClipPolygon_Back(Polygon inPolygon, List<PointD> clipPList)
        {
            List<Polygon> newPolygons = new List<Polygon>();
            List<PolyLine> newPolylines = new List<PolyLine>();
            List<PointD> aPList = inPolygon.OutLine;
            Extent plExtent = MIMath.GetPointsExtent(aPList);
            Extent cutExtent = MIMath.GetPointsExtent(clipPList);

            if (!MIMath.IsExtentCross(plExtent, cutExtent))
                return newPolygons;

            int i, j;

            if (!IsClockwise(clipPList))    //---- Make cut polygon clockwise
                clipPList.Reverse();

            //Judge if all points of the polyline are in the cut polygon            
            if (PointInPolygon(clipPList, aPList[0]))
            {
                bool isAllIn = true;
                int notInIdx = 0;
                for (i = 0; i < aPList.Count; i++)
                {
                    if (!PointInPolygon(clipPList, aPList[i]))
                    {
                        notInIdx = i;
                        isAllIn = false;
                        break;
                    }
                }
                if (!isAllIn)   //Put start point outside of the cut polygon
                {
                    List<PointD> bPList = new List<PointD>();
                    bPList.AddRange(aPList.GetRange(notInIdx, aPList.Count - notInIdx));
                    bPList.AddRange(aPList.GetRange(1, notInIdx - 1));
                    //for (i = notInIdx; i < aPList.Count; i++)
                    //    bPList.Add(aPList[i]);

                    //for (i = 1; i < notInIdx; i++)
                    //    bPList.Add(aPList[i]);

                    bPList.Add(bPList[0]);
                    aPList = new List<PointD>(bPList);
                }
                else    //the input polygon is inside the cut polygon
                {
                    newPolygons.Add(inPolygon);
                    return newPolygons;
                }
            }

            //Prepare border point list
            List<BorderPoint> borderList = new List<BorderPoint>();
            BorderPoint aBP = new BorderPoint();
            foreach (PointD aP in clipPList)
            {
                aBP = new BorderPoint();
                aBP.Point = aP;
                aBP.Id = -1;
                borderList.Add(aBP);
            }

            //Cutting            
            bool isInPolygon = false;
            PointD q1, q2, p1, p2, IPoint = new PointD();
            Line lineA, lineB;
            List<PointD> newPlist = new List<PointD>();
            PolyLine bLine = new PolyLine();
            p1 = aPList[0];
            int inIdx = -1, outIdx = -1;
            int a1 = 0;
            bool isNewLine = true;
            for (i = 1; i < aPList.Count; i++)
            {
                p2 = (PointD)aPList[i];
                if (PointInPolygon(clipPList, p2))
                {
                    if (!isInPolygon)
                    {
                        lineA.P1 = p1;
                        lineA.P2 = p2;
                        q1 = borderList[borderList.Count - 1].Point;
                        for (j = 0; j < borderList.Count; j++)
                        {
                            q2 = borderList[j].Point;
                            lineB.P1 = q1;
                            lineB.P2 = q2;
                            if (IsLineSegmentCross(lineA, lineB))
                            {
                                IPoint = GetCrossPoint(lineA, lineB);
                                aBP = new BorderPoint();
                                aBP.Id = newPolylines.Count;
                                aBP.Point = IPoint;
                                borderList.Insert(j, aBP);
                                inIdx = j;
                                break;
                            }
                            q1 = q2;
                        }
                        newPlist.Add(IPoint);
                    }
                    newPlist.Add(aPList[i]);
                    isInPolygon = true;
                }
                else
                {
                    if (isInPolygon)
                    {
                        lineA.P1 = p1;
                        lineA.P2 = p2;
                        q1 = borderList[borderList.Count - 1].Point;
                        for (j = 0; j < borderList.Count; j++)
                        {
                            q2 = borderList[j].Point;
                            lineB.P1 = q1;
                            lineB.P2 = q2;
                            if (IsLineSegmentCross(lineA, lineB))
                            {
                                if (!isNewLine)
                                {
                                    if (inIdx - outIdx >= 1 && inIdx - outIdx <= 10)
                                    {
                                        if (!TwoPointsInside(a1, outIdx, inIdx, j))
                                        {
                                            borderList.RemoveAt(inIdx);
                                            borderList.Insert(outIdx, aBP);
                                        }
                                    }
                                    else if (inIdx - outIdx <= -1 && inIdx - outIdx >= -10)
                                    {
                                        if (!TwoPointsInside(a1, outIdx, inIdx, j))
                                        {
                                            borderList.RemoveAt(inIdx);
                                            borderList.Insert(outIdx + 1, aBP);
                                        }
                                    }
                                    else if (inIdx == outIdx)
                                    {
                                        if (!TwoPointsInside(a1, outIdx, inIdx, j))
                                        {
                                            borderList.RemoveAt(inIdx);
                                            borderList.Insert(inIdx + 1, aBP);
                                        }
                                    }
                                }
                                IPoint = GetCrossPoint(lineA, lineB);
                                aBP = new BorderPoint();
                                aBP.Id = newPolylines.Count;
                                aBP.Point = IPoint;
                                borderList.Insert(j, aBP);
                                outIdx = j;
                                a1 = inIdx;
                                isNewLine = false;
                                break;
                            }
                            q1 = q2;
                        }
                        newPlist.Add(IPoint);

                        bLine = new PolyLine();
                        //bLine.Value = inPolygon.OutLine.Value;
                        //bLine.Type = inPolygon.OutLine.Type;
                        bLine.PointList = new List<PointD>(newPlist);
                        newPolylines.Add(bLine);

                        isInPolygon = false;
                        newPlist = new List<PointD>();
                    }
                }
                p1 = p2;
            }

            if (newPolylines.Count > 0)
            {
                //Tracing polygons
                newPolygons = TracingClipPolygons(inPolygon, newPolylines, borderList);
            }
            else
            {
                if (PointInPolygon(aPList, clipPList[0]))
                {                    
                    Polygon aPolygon = new Polygon();                    
                    aPolygon.OutLine = new List<PointD>(clipPList);
                    aPolygon.HoleLines = new List<List<PointD>>();

                    newPolygons.Add(aPolygon);
                }
            }

            return newPolygons;
        }

        private static bool IsExtentCross(Extent aExtent, object clipObj)
        {
            if (clipObj.GetType() == typeof(List<PointD>))
            {
                Extent bExtent = MIMath.GetPointsExtent((List<PointD>)clipObj);
                return MIMath.IsExtentCross(aExtent, bExtent);
            }
            if (clipObj.GetType() == typeof(ClipLine))
            {
                return ((ClipLine)clipObj).IsExtentCross(aExtent);                
            }
            if (clipObj.GetType() == typeof(Extent))
            {
                return MIMath.IsExtentCross(aExtent, (Extent)clipObj);
            }

            return false;
        }

        private static bool PointInClipObj(object clipObj, PointD aPoint)
        {
            if (clipObj.GetType() == typeof(List<PointD>))
            {
                return PointInPolygon((List<PointD>)clipObj, aPoint);
            }
            if (clipObj.GetType() == typeof(ClipLine))
            {
                return ((ClipLine)clipObj).IsInside(aPoint);
            }
            if (clipObj.GetType() == typeof(Extent))
            {
                return MIMath.PointInExtent(aPoint, (Extent)clipObj);
            }

            return false;
        }

        private static List<PointD> GetClipPointList(object clipObj)
        {
            List<PointD> clipPList = new List<PointD>();
            if (clipObj.GetType() == typeof(List<PointD>))
            {
                clipPList = (List<PointD>)clipObj;
            }
            if (clipObj.GetType() == typeof(ClipLine))
            {
                ClipLine clipLine = (ClipLine)clipObj;
                if (clipLine.IsLon)
                {
                    for (int i = -100; i <= 100; i++)
                        clipPList.Add(new PointD(clipLine.Value, i));                  
                }
                else
                {
                    for (int i = -370; i <= 370; i++)
                        clipPList.Add(new PointD(i, clipLine.Value));               
                }
            }
            if (clipObj.GetType() == typeof(Extent))
            {
                Extent aExtent = (Extent)clipObj;
                clipPList.Add(new PointD(aExtent.minX, aExtent.minY));
                clipPList.Add(new PointD(aExtent.minX, aExtent.maxY));
                clipPList.Add(new PointD(aExtent.maxX, aExtent.maxY));
                clipPList.Add(new PointD(aExtent.maxX, aExtent.minY));
                clipPList.Add(clipPList[0]);
            }

            return clipPList;
        }

        private static List<Polygon> ClipPolygon(Polygon inPolygon, object clipObj)
        {
            List<Polygon> newPolygons = new List<Polygon>();
            List<PolyLine> newPolylines = new List<PolyLine>();
            List<PointD> aPList = inPolygon.OutLine;                  

            if (!IsExtentCross(inPolygon.Extent, clipObj))
                return newPolygons;

            int i, j;

            if (clipObj.GetType() == typeof(List<PointD>))
            {
                if (!IsClockwise((List<PointD>)clipObj))    //---- Make cut polygon clockwise
                    ((List<PointD>)clipObj).Reverse();
            }
            else if (clipObj.GetType() == typeof(ClipLine))
            {
                if (((ClipLine)clipObj).IsExtentInside(inPolygon.Extent))
                {
                    newPolygons.Add(inPolygon);
                    return newPolygons;
                }
            }

            //Judge if all points of the polyline are in the cut polygon - outline   
            List<List<PointD>> newLines = new List<List<PointD>>();
            if (PointInClipObj(clipObj, aPList[0]))
            {
                bool isAllIn = true;
                int notInIdx = 0;
                for (i = 0; i < aPList.Count; i++)
                {
                    if (!PointInClipObj(clipObj, aPList[i]))
                    {
                        notInIdx = i;
                        isAllIn = false;
                        break;
                    }
                }
                if (!isAllIn)   //Put start point outside of the cut polygon
                {
                    List<PointD> bPList = new List<PointD>();
                    bPList.AddRange(aPList.GetRange(notInIdx, aPList.Count - notInIdx));
                    bPList.AddRange(aPList.GetRange(1, notInIdx - 1));
                    //for (i = notInIdx; i < aPList.Count; i++)
                    //    bPList.Add(aPList[i]);

                    //for (i = 1; i < notInIdx; i++)
                    //    bPList.Add(aPList[i]);

                    bPList.Add(bPList[0]);
                    //if (!IsClockwise(bPList))
                    //    bPList.Reverse();
                    newLines.Add(bPList);
                }
                else    //the input polygon is inside the cut polygon
                {
                    newPolygons.Add(inPolygon);
                    return newPolygons;
                }
            }
            else
            {
                newLines.Add(aPList);
            }

            //Holes
            List<List<PointD>> holeLines = new List<List<PointD>>();
            if (inPolygon.HasHole)
            {
                for (int h = 0; h < inPolygon.HoleLines.Count; h++)
                {
                    List<PointD> holePList = inPolygon.HoleLines[h];
                    Extent plExtent = MIMath.GetPointsExtent(holePList);
                    if (!IsExtentCross(plExtent, clipObj))
                        continue;

                    if (PointInClipObj(clipObj, holePList[0]))
                    {
                        bool isAllIn = true;
                        int notInIdx = 0;
                        for (i = 0; i < holePList.Count; i++)
                        {
                            if (!PointInClipObj(clipObj, holePList[i]))
                            {
                                notInIdx = i;
                                isAllIn = false;
                                break;
                            }
                        }
                        if (!isAllIn)   //Put start point outside of the cut polygon
                        {
                            List<PointD> bPList = new List<PointD>();
                            bPList.AddRange(holePList.GetRange(notInIdx, holePList.Count - notInIdx));
                            bPList.AddRange(holePList.GetRange(1, notInIdx - 1));
                            //for (i = notInIdx; i < aPList.Count; i++)
                            //    bPList.Add(aPList[i]);

                            //for (i = 1; i < notInIdx; i++)
                            //    bPList.Add(aPList[i]);

                            bPList.Add(bPList[0]);
                            newLines.Add(bPList);
                        }
                        else    //the hole is inside the cut polygon
                        {
                            holeLines.Add(inPolygon.HoleLines[h]);
                        }
                    }
                    else
                        newLines.Add(holePList);
                }
            }

            //Prepare border point list
            List<BorderPoint> borderList = new List<BorderPoint>();
            BorderPoint aBP = new BorderPoint();
            List<PointD> clipPList = GetClipPointList(clipObj);
            foreach (PointD aP in clipPList)
            {
                aBP = new BorderPoint();
                aBP.Point = aP;
                aBP.Id = -1;
                borderList.Add(aBP);
            }

            //Cutting                     
            for (int l = 0; l < newLines.Count; l++)
            {
                aPList = newLines[l];
                bool isInPolygon = false;
                PointD q1, q2, p1, p2, IPoint = new PointD();
                Line lineA, lineB;
                List<PointD> newPlist = new List<PointD>();
                PolyLine bLine = new PolyLine();
                p1 = aPList[0];
                int inIdx = -1, outIdx = -1;
                bool newLine = true;
                int a1 = 0;
                for (i = 1; i < aPList.Count; i++)
                {
                    p2 = aPList[i];
                    if (PointInClipObj(clipObj, p2))
                    {
                        if (!isInPolygon)
                        {
                            lineA.P1 = p1;
                            lineA.P2 = p2;
                            //q1 = borderList[borderList.Count - 1].Point;
                            q1 = borderList[0].Point;
                            for (j = 1; j < borderList.Count; j++)
                            {
                                q2 = borderList[j].Point;
                                lineB.P1 = q1;
                                lineB.P2 = q2;
                                if (IsLineSegmentCross(lineA, lineB))
                                {
                                    IPoint = GetCrossPoint(lineA, lineB);
                                    aBP = new BorderPoint();
                                    aBP.Id = newPolylines.Count;
                                    aBP.Point = IPoint;
                                    borderList.Insert(j, aBP);
                                    inIdx = j;
                                    break;
                                }
                                q1 = q2;
                            }
                            newPlist.Add(IPoint);
                        }
                        newPlist.Add(aPList[i]);
                        isInPolygon = true;
                    }
                    else
                    {
                        if (isInPolygon)
                        {
                            lineA.P1 = p1;
                            lineA.P2 = p2;
                            //q1 = borderList[borderList.Count - 1].Point;
                            q1 = borderList[0].Point;
                            for (j = 1; j < borderList.Count; j++)
                            {
                                q2 = borderList[j].Point;
                                lineB.P1 = q1;
                                lineB.P2 = q2;
                                if (IsLineSegmentCross(lineA, lineB))
                                {
                                    if (!newLine)
                                    {
                                        if (inIdx - outIdx >= 1 && inIdx - outIdx <= 10)
                                        {
                                            if (!TwoPointsInside(a1, outIdx, inIdx, j))
                                            {
                                                borderList.RemoveAt(inIdx);
                                                borderList.Insert(outIdx, aBP);
                                            }
                                        }
                                        else if (inIdx - outIdx <= -1 && inIdx - outIdx >= -10)
                                        {
                                            if (!TwoPointsInside(a1, outIdx, inIdx, j))
                                            {
                                                borderList.RemoveAt(inIdx);
                                                borderList.Insert(outIdx + 1, aBP);
                                            }
                                        }
                                        else if (inIdx == outIdx)
                                        {
                                            if (!TwoPointsInside(a1, outIdx, inIdx, j))
                                            {
                                                borderList.RemoveAt(inIdx);
                                                borderList.Insert(inIdx + 1, aBP);
                                            }
                                        }
                                    }
                                    IPoint = GetCrossPoint(lineA, lineB);
                                    aBP = new BorderPoint();
                                    aBP.Id = newPolylines.Count;
                                    aBP.Point = IPoint;
                                    borderList.Insert(j, aBP);
                                    outIdx = j;
                                    a1 = inIdx;

                                    newLine = false;
                                    break;
                                }
                                q1 = q2;
                            }
                            newPlist.Add(IPoint);

                            bLine = new PolyLine();
                            //bLine.Value = inPolygon.OutLine.Value;
                            //bLine.Type = inPolygon.OutLine.Type;
                            bLine.PointList = new List<PointD>(newPlist);
                            newPolylines.Add(bLine);

                            isInPolygon = false;
                            newPlist = new List<PointD>();
                        }
                    }
                    p1 = p2;
                }
            }

            if (newPolylines.Count > 0)
            {
                if (aBP.Id >= newPolylines.Count)
                    return newPolygons;

                //Tracing polygons
                newPolygons = TracingClipPolygons(inPolygon, newPolylines, borderList);                
            }
            else
            {
                if (clipObj.GetType() != typeof(ClipLine))
                {
                    if (PointInPolygon(aPList, clipPList[0]))
                    {
                        if (!IsClockwise(clipPList))
                            clipPList.Reverse();

                        Polygon aPolygon = new Polygon();
                        aPolygon.OutLine = new List<PointD>(clipPList);
                        aPolygon.HoleLines = new List<List<PointD>>();

                        newPolygons.Add(aPolygon);
                    }
                }
            }            

            if (holeLines.Count > 0)
            {
                AddHoles_Ring(ref newPolygons, holeLines);
            }

            return newPolygons;
        }

        private static List<PolyLine> ClipPolyLine(PolyLine inPolyLine, object clipObj)
        {            
            List<PolyLine> newPolylines = new List<PolyLine>();
            List<PointD> aPList = inPolyLine.PointList;

            if (!IsExtentCross(inPolyLine.Extent, clipObj))
                return newPolylines;

            int i, j;

            if (clipObj.GetType() == typeof(List<PointD>))
            {
                if (!IsClockwise((List<PointD>)clipObj))    //---- Make cut polygon clockwise
                    ((List<PointD>)clipObj).Reverse();
            }
            else if (clipObj.GetType() == typeof(ClipLine))
            {
                if (((ClipLine)clipObj).IsExtentInside(inPolyLine.Extent))
                {
                    newPolylines.Add(inPolyLine);
                    return newPolylines;
                }
            }

            //Judge if all points of the polyline are in the cut polygon - outline   
            List<List<PointD>> newLines = new List<List<PointD>>();
            if (PointInClipObj(clipObj, aPList[0]))
            {
                bool isAllIn = true;
                int notInIdx = 0;
                for (i = 0; i < aPList.Count; i++)
                {
                    if (!PointInClipObj(clipObj, aPList[i]))
                    {
                        notInIdx = i;
                        isAllIn = false;
                        break;
                    }
                }
                if (!isAllIn)   //Put start point outside of the cut polygon
                {
                    if (inPolyLine.IsClosed())
                    {
                        List<PointD> bPList = new List<PointD>();
                        bPList.AddRange(aPList.GetRange(notInIdx, aPList.Count - notInIdx));
                        bPList.AddRange(aPList.GetRange(1, notInIdx - 1));
                        //for (i = notInIdx; i < aPList.Count; i++)
                        //    bPList.Add(aPList[i]);

                        //for (i = 1; i < notInIdx; i++)
                        //    bPList.Add(aPList[i]);

                        bPList.Add(bPList[0]);
                        //if (!IsClockwise(bPList))
                        //    bPList.Reverse();
                        newLines.Add(bPList);
                    }
                    else
                    {
                        aPList.Reverse();
                        newLines.Add(aPList);
                    }
                }
                else    //the input polygon is inside the cut polygon
                {
                    newPolylines.Add(inPolyLine);
                    return newPolylines;
                }
            }
            else
            {
                newLines.Add(aPList);
            }            

            //Prepare border point list
            List<BorderPoint> borderList = new List<BorderPoint>();
            BorderPoint aBP = new BorderPoint();
            List<PointD> clipPList = GetClipPointList(clipObj);
            foreach (PointD aP in clipPList)
            {
                aBP = new BorderPoint();
                aBP.Point = aP;
                aBP.Id = -1;
                borderList.Add(aBP);
            }

            //Cutting                     
            for (int l = 0; l < newLines.Count; l++)
            {
                aPList = newLines[l];
                bool isInPolygon = PointInClipObj(clipObj, aPList[0]);
                PointD q1, q2, p1, p2, IPoint = new PointD();
                Line lineA, lineB;
                List<PointD> newPlist = new List<PointD>();
                PolyLine bLine = new PolyLine();
                p1 = aPList[0];
                int inIdx = -1, outIdx = -1;
                bool newLine = true;
                int a1 = 0;
                for (i = 1; i < aPList.Count; i++)
                {
                    p2 = aPList[i];
                    if (PointInClipObj(clipObj, p2))
                    {
                        if (!isInPolygon)
                        {
                            lineA.P1 = p1;
                            lineA.P2 = p2;
                            //q1 = borderList[borderList.Count - 1].Point;
                            q1 = borderList[0].Point;
                            for (j = 1; j < borderList.Count; j++)
                            {
                                q2 = borderList[j].Point;
                                lineB.P1 = q1;
                                lineB.P2 = q2;
                                if (IsLineSegmentCross(lineA, lineB))
                                {
                                    IPoint = GetCrossPoint(lineA, lineB);
                                    aBP = new BorderPoint();
                                    aBP.Id = newPolylines.Count;
                                    aBP.Point = IPoint;
                                    borderList.Insert(j, aBP);
                                    inIdx = j;
                                    break;
                                }
                                q1 = q2;
                            }
                            newPlist.Add(IPoint);
                        }
                        newPlist.Add(aPList[i]);
                        isInPolygon = true;
                    }
                    else
                    {
                        if (isInPolygon)
                        {
                            lineA.P1 = p1;
                            lineA.P2 = p2;
                            //q1 = borderList[borderList.Count - 1].Point;
                            q1 = borderList[0].Point;
                            for (j = 1; j < borderList.Count; j++)
                            {
                                q2 = borderList[j].Point;
                                lineB.P1 = q1;
                                lineB.P2 = q2;
                                if (IsLineSegmentCross(lineA, lineB))
                                {
                                    if (!newLine)
                                    {
                                        if (inIdx - outIdx >= 1 && inIdx - outIdx <= 10)
                                        {
                                            if (!TwoPointsInside(a1, outIdx, inIdx, j))
                                            {
                                                borderList.RemoveAt(inIdx);
                                                borderList.Insert(outIdx, aBP);
                                            }
                                        }
                                        else if (inIdx - outIdx <= -1 && inIdx - outIdx >= -10)
                                        {
                                            if (!TwoPointsInside(a1, outIdx, inIdx, j))
                                            {
                                                borderList.RemoveAt(inIdx);
                                                borderList.Insert(outIdx + 1, aBP);
                                            }
                                        }
                                        else if (inIdx == outIdx)
                                        {
                                            if (!TwoPointsInside(a1, outIdx, inIdx, j))
                                            {
                                                borderList.RemoveAt(inIdx);
                                                borderList.Insert(inIdx + 1, aBP);
                                            }
                                        }
                                    }
                                    IPoint = GetCrossPoint(lineA, lineB);
                                    aBP = new BorderPoint();
                                    aBP.Id = newPolylines.Count;
                                    aBP.Point = IPoint;
                                    borderList.Insert(j, aBP);
                                    outIdx = j;
                                    a1 = inIdx;

                                    newLine = false;
                                    break;
                                }
                                q1 = q2;
                            }
                            newPlist.Add(IPoint);

                            bLine = new PolyLine();
                            //bLine.Value = inPolygon.OutLine.Value;
                            //bLine.Type = inPolygon.OutLine.Type;
                            bLine.PointList = new List<PointD>(newPlist);
                            newPolylines.Add(bLine);

                            isInPolygon = false;
                            newPlist = new List<PointD>();
                        }
                    }
                    p1 = p2;
                }

                if (isInPolygon && newPlist.Count > 1)
                {
                    bLine = new PolyLine();
                    //bLine.Value = inPolyline.Value;
                    //bLine.Type = inPolyline.Type;
                    bLine.PointList = new List<PointD>(newPlist);
                    newPolylines.Add(bLine);
                }
            }

            return newPolylines;
        }

        //private static List<PolylineZ> ClipPolyLine(PolylineZ inPolyLine, object clipObj)
        //{
        //    List<PolylineZ> newPolylines = new List<PolylineZ>();
        //    List<PointZ> aPList = inPolyLine.PointList;

        //    if (!IsExtentCross(inPolyLine.Extent, clipObj))
        //        return newPolylines;

        //    int i, j;

        //    if (clipObj.GetType() == typeof(List<PointD>))
        //    {
        //        if (!IsClockwise((List<PointD>)clipObj))    //---- Make cut polygon clockwise
        //            ((List<PointD>)clipObj).Reverse();
        //    }
        //    else if (clipObj.GetType() == typeof(ClipLine))
        //    {
        //        if (((ClipLine)clipObj).IsExtentInside(inPolyLine.Extent))
        //        {
        //            newPolylines.Add(inPolyLine);
        //            return newPolylines;
        //        }
        //    }

        //    //Judge if all points of the polyline are in the cut polygon - outline   
        //    List<List<PointZ>> newLines = new List<List<PointZ>>();
        //    if (PointInClipObj(clipObj, aPList[0].ToPointD()))
        //    {
        //        bool isAllIn = true;
        //        int notInIdx = 0;
        //        for (i = 0; i < aPList.Count; i++)
        //        {
        //            if (!PointInClipObj(clipObj, aPList[i].ToPointD()))
        //            {
        //                notInIdx = i;
        //                isAllIn = false;
        //                break;
        //            }
        //        }
        //        if (!isAllIn)   //Put start point outside of the cut polygon
        //        {
        //            if (inPolyLine.IsClosed())
        //            {
        //                List<PointZ> bPList = new List<PointZ>();
        //                bPList.AddRange(aPList.GetRange(notInIdx, aPList.Count - notInIdx));
        //                bPList.AddRange(aPList.GetRange(1, notInIdx - 1));
        //                //for (i = notInIdx; i < aPList.Count; i++)
        //                //    bPList.Add(aPList[i]);

        //                //for (i = 1; i < notInIdx; i++)
        //                //    bPList.Add(aPList[i]);

        //                bPList.Add(bPList[0]);
        //                //if (!IsClockwise(bPList))
        //                //    bPList.Reverse();
        //                newLines.Add(bPList);
        //            }
        //            else
        //            {
        //                aPList.Reverse();
        //                newLines.Add(aPList);
        //            }
        //        }
        //        else    //the input polygon is inside the cut polygon
        //        {
        //            newPolylines.Add(inPolyLine);
        //            return newPolylines;
        //        }
        //    }
        //    else
        //    {
        //        newLines.Add(aPList);
        //    }

        //    //Prepare border point list
        //    List<BorderPoint> borderList = new List<BorderPoint>();
        //    BorderPoint aBP = new BorderPoint();
        //    List<PointD> clipPList = GetClipPointList(clipObj);
        //    foreach (PointD aP in clipPList)
        //    {
        //        aBP = new BorderPoint();
        //        aBP.Point = aP;
        //        aBP.Id = -1;
        //        borderList.Add(aBP);
        //    }

        //    //Cutting                     
        //    for (int l = 0; l < newLines.Count; l++)
        //    {
        //        aPList = newLines[l];
        //        bool isInPolygon = PointInClipObj(clipObj, aPList[0].ToPointD());
        //        PointD q1, q2 = new PointD();
        //        PointZ p1, p2 = new PointZ();
        //        PointZ IPoint = new PointZ();
        //        Line lineA, lineB;
        //        List<PointZ> newPlist = new List<PointZ>();
        //        PolylineZ bLine = new PolylineZ();
        //        p1 = aPList[0];
        //        int inIdx = -1, outIdx = -1;
        //        bool newLine = true;
        //        int a1 = 0;
        //        for (i = 1; i < aPList.Count; i++)
        //        {
        //            p2 = aPList[i];
        //            if (PointInClipObj(clipObj, p2.ToPointD()))
        //            {
        //                if (!isInPolygon)
        //                {
        //                    lineA.P1 = p1.ToPointD();
        //                    lineA.P2 = p2.ToPointD();
        //                    //q1 = borderList[borderList.Count - 1].Point;
        //                    q1 = borderList[0].Point;
        //                    for (j = 1; j < borderList.Count; j++)
        //                    {
        //                        q2 = borderList[j].Point;
        //                        lineB.P1 = q1;
        //                        lineB.P2 = q2;
        //                        if (IsLineSegmentCross(lineA, lineB))
        //                        {
        //                            PointD crossP = GetCrossPoint(lineA, lineB);
        //                            IPoint = new PointZ();
        //                            IPoint.X = crossP.X;
        //                            IPoint.Y = crossP.Y;
        //                            IPoint.Z = (p1.Z + p2.Z) / 2;
        //                            aBP = new BorderPoint();
        //                            aBP.Id = newPolylines.Count;
        //                            aBP.Point = IPoint.ToPointD();
        //                            borderList.Insert(j, aBP);
        //                            inIdx = j;
        //                            break;
        //                        }
        //                        q1 = q2;
        //                    }
        //                    newPlist.Add(IPoint);
        //                }
        //                newPlist.Add(aPList[i]);
        //                isInPolygon = true;
        //            }
        //            else
        //            {
        //                if (isInPolygon)
        //                {
        //                    lineA.P1 = p1.ToPointD();
        //                    lineA.P2 = p2.ToPointD();
        //                    //q1 = borderList[borderList.Count - 1].Point;
        //                    q1 = borderList[0].Point;
        //                    for (j = 1; j < borderList.Count; j++)
        //                    {
        //                        q2 = borderList[j].Point;
        //                        lineB.P1 = q1;
        //                        lineB.P2 = q2;
        //                        if (IsLineSegmentCross(lineA, lineB))
        //                        {
        //                            if (!newLine)
        //                            {
        //                                if (inIdx - outIdx >= 1 && inIdx - outIdx <= 10)
        //                                {
        //                                    if (!TwoPointsInside(a1, outIdx, inIdx, j))
        //                                    {
        //                                        borderList.RemoveAt(inIdx);
        //                                        borderList.Insert(outIdx, aBP);
        //                                    }
        //                                }
        //                                else if (inIdx - outIdx <= -1 && inIdx - outIdx >= -10)
        //                                {
        //                                    if (!TwoPointsInside(a1, outIdx, inIdx, j))
        //                                    {
        //                                        borderList.RemoveAt(inIdx);
        //                                        borderList.Insert(outIdx + 1, aBP);
        //                                    }
        //                                }
        //                                else if (inIdx == outIdx)
        //                                {
        //                                    if (!TwoPointsInside(a1, outIdx, inIdx, j))
        //                                    {
        //                                        borderList.RemoveAt(inIdx);
        //                                        borderList.Insert(inIdx + 1, aBP);
        //                                    }
        //                                }
        //                            }
        //                            PointD crossP = GetCrossPoint(lineA, lineB);
        //                            IPoint = new PointZ();
        //                            IPoint.X = crossP.X;
        //                            IPoint.Y = crossP.Y;
        //                            IPoint.Z = (p1.Z + p2.Z) / 2;
        //                            aBP = new BorderPoint();
        //                            aBP.Id = newPolylines.Count;
        //                            aBP.Point = IPoint.ToPointD();
        //                            borderList.Insert(j, aBP);
        //                            outIdx = j;
        //                            a1 = inIdx;

        //                            newLine = false;
        //                            break;
        //                        }
        //                        q1 = q2;
        //                    }
        //                    newPlist.Add(IPoint);

        //                    bLine = new PolylineZ();
        //                    //bLine.Value = inPolygon.OutLine.Value;
        //                    //bLine.Type = inPolygon.OutLine.Type;
        //                    bLine.PointList = new List<PointZ>(newPlist);
        //                    newPolylines.Add(bLine);

        //                    isInPolygon = false;
        //                    newPlist = new List<PointZ>();
        //                }
        //            }
        //            p1 = p2;
        //        }

        //        if (isInPolygon && newPlist.Count > 1)
        //        {
        //            bLine = new PolylineZ();
        //            //bLine.Value = inPolyline.Value;
        //            //bLine.Type = inPolyline.Type;
        //            bLine.PointList = new List<PointZ>(newPlist);
        //            newPolylines.Add(bLine);
        //        }
        //    }

        //    return newPolylines;
        //}        

        private static List<Polygon> TracingClipPolygons(Polygon inPolygon, List<PolyLine> LineList, List<BorderPoint> borderList)
        {
            if (LineList.Count == 0)
                return new List<Polygon>();

            List<Polygon> aPolygonList = new List<Polygon>(), newPolygonlist = new List<Polygon>();
            List<PolyLine> aLineList = new List<PolyLine>();
            PolyLine aLine = new PolyLine();
            PointD aPoint;
            Polygon aPolygon = new Polygon();
            //Extent aBound = new Extent();
            int i, j;

            aLineList = new List<PolyLine>(LineList);

            //---- Tracing border polygon
            List<PointD> aPList = new List<PointD>();
            List<PointD> newPList = new List<PointD>();
            BorderPoint bP;
            int[] timesArray = new int[borderList.Count - 1];
            for (i = 0; i < timesArray.Length; i++)
                timesArray[i] = 0;

            int pIdx, pNum;
            List<BorderPoint> lineBorderList = new List<BorderPoint>();

            pNum = borderList.Count - 1;
            PointD bPoint, b1Point;
            for (i = 0; i < pNum; i++)
            {
                if ((borderList[i]).Id == -1)
                    continue;

                pIdx = i;
                aPList.Clear();
                lineBorderList.Add(borderList[i]);
                bP = borderList[pIdx];
                b1Point = borderList[pIdx].Point;

                //---- Clockwise traceing
                if (timesArray[pIdx] < 1)
                {
                    aPList.Add((borderList[pIdx]).Point);
                    pIdx += 1;
                    if (pIdx == pNum)
                        pIdx = 0;

                    bPoint = borderList[pIdx].Point;
                    if (borderList[pIdx].Id > -1)
                    {
                        bPoint.X = (bPoint.X + b1Point.X) / 2;
                        bPoint.Y = (bPoint.Y + b1Point.Y) / 2;
                    }
                    if (PointInPolygon(inPolygon, bPoint))
                    {
                        while (true)
                        {
                            bP = borderList[pIdx];
                            if (bP.Id == -1)    //---- Not endpoint of contour
                            {
                                if (timesArray[pIdx] == 1)
                                    break;

                                aPList.Add(bP.Point);
                                timesArray[pIdx] += +1;
                            }
                            else    //---- endpoint of contour
                            {
                                if (timesArray[pIdx] == 1)
                                    break;

                                timesArray[pIdx] += +1;
                                aLine = aLineList[bP.Id];

                                newPList = new List<PointD>(aLine.PointList);
                                aPoint = (PointD)newPList[0];

                                if (!(MIMath.DoubleEquals(bP.Point.X, aPoint.X) && MIMath.DoubleEquals(bP.Point.Y, aPoint.Y)))    //---- Start point
                                    //if (!IsClockwise(newPList))
                                    newPList.Reverse();

                                aPList.AddRange(newPList);
                                for (j = 0; j < borderList.Count - 1; j++)
                                {
                                    if (j != pIdx)
                                    {
                                        if ((borderList[j]).Id == bP.Id)
                                        {
                                            pIdx = j;
                                            timesArray[pIdx] += +1;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (pIdx == i)
                            {
                                if (aPList.Count > 0)
                                {
                                    aPolygon = new Polygon();
                                    aPolygon.OutLine = new List<PointD>(aPList);
                                    aPolygon.HoleLines = new List<List<PointD>>();
                                    aPolygonList.Add(aPolygon);
                                }
                                break;
                            }
                            pIdx += 1;
                            if (pIdx == pNum)
                                pIdx = 0;
                        }
                    }
                }

                //---- Anticlockwise traceing
                pIdx = i;
                if (timesArray[pIdx] < 1)
                {
                    aPList.Clear();
                    aPList.Add((borderList[pIdx]).Point);
                    pIdx += -1;
                    if (pIdx == -1)
                        pIdx = pNum - 1;

                    bPoint = borderList[pIdx].Point;
                    if (borderList[pIdx].Id > -1)
                    {
                        bPoint.X = (bPoint.X + b1Point.X) / 2;
                        bPoint.Y = (bPoint.Y + b1Point.Y) / 2;
                    }
                    if (PointInPolygon(inPolygon, bPoint))
                    {
                        while (true)
                        {
                            bP = borderList[pIdx];
                            if (bP.Id == -1)    //---- Not endpoint of contour
                            {
                                if (timesArray[pIdx] == 1)
                                    break;

                                aPList.Add(bP.Point);
                                timesArray[pIdx] += +1;
                            }
                            else    //---- endpoint of contour
                            {
                                if (timesArray[pIdx] == 1)
                                    break;

                                timesArray[pIdx] += +1;
                                aLine = aLineList[bP.Id];

                                newPList = new List<PointD>(aLine.PointList);
                                aPoint = (PointD)newPList[0];

                                if (!(MIMath.DoubleEquals(bP.Point.X, aPoint.X) && MIMath.DoubleEquals(bP.Point.Y, aPoint.Y)))    //---- Start point
                                    //if (IsClockwise(newPList))
                                    newPList.Reverse();

                                aPList.AddRange(newPList);
                                for (j = 0; j < borderList.Count - 1; j++)
                                {
                                    if (j != pIdx)
                                    {
                                        if ((borderList[j]).Id == bP.Id)
                                        {
                                            pIdx = j;
                                            timesArray[pIdx] += +1;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (pIdx == i)
                            {
                                if (aPList.Count > 0)
                                {
                                    aPolygon = new Polygon();
                                    aPList.Reverse();
                                    aPolygon.OutLine = new List<PointD>(aPList);
                                    aPolygon.HoleLines = new List<List<PointD>>();
                                    aPolygonList.Add(aPolygon);
                                }
                                break;
                            }
                            pIdx += -1;
                            if (pIdx == -1)
                                pIdx = pNum - 1;

                        }
                    }
                }
            }

            newPolygonlist = new List<Polygon>(aPolygonList);

            return newPolygonlist;
        }

        private static bool IsLineSegmentCross(Line lineA, Line lineB)
        {
            Extent boundA = new Extent(), boundB = new Extent();
            List<PointD> PListA = new List<PointD>(), PListB = new List<PointD>();
            PListA.Add(lineA.P1);
            PListA.Add(lineA.P2);
            PListB.Add(lineB.P1);
            PListB.Add(lineB.P2);
            boundA = MIMath.GetPointsExtent(PListA);
            boundB = MIMath.GetPointsExtent(PListB);            

            if (!MIMath.IsExtentCross(boundA, boundB))
                return false;
            else
            {
                double XP1 = (lineB.P1.X - lineA.P1.X) * (lineA.P2.Y - lineA.P1.Y) -
                  (lineA.P2.X - lineA.P1.X) * (lineB.P1.Y - lineA.P1.Y);
                double XP2 = (lineB.P2.X - lineA.P1.X) * (lineA.P2.Y - lineA.P1.Y) -
                  (lineA.P2.X - lineA.P1.X) * (lineB.P2.Y - lineA.P1.Y);
                if (XP1 * XP2 > 0)
                    return false;
                else
                    return true;
            }
        }

        private static PointD GetCrossPoint(Line lineA, Line lineB)
        {
            PointD IPoint = new PointD();
            PointD p1, p2, q1, q2;
            double tempLeft, tempRight;

            double XP1 = (lineB.P1.X - lineA.P1.X) * (lineA.P2.Y - lineA.P1.Y) -
                  (lineA.P2.X - lineA.P1.X) * (lineB.P1.Y - lineA.P1.Y);
            double XP2 = (lineB.P2.X - lineA.P1.X) * (lineA.P2.Y - lineA.P1.Y) -
              (lineA.P2.X - lineA.P1.X) * (lineB.P2.Y - lineA.P1.Y);
            if (XP1 == 0)
                IPoint = lineB.P1;
            else if (XP2 == 0)
                IPoint = lineB.P2;
            else
            {
                p1 = lineA.P1;
                p2 = lineA.P2;
                q1 = lineB.P1;
                q2 = lineB.P2;

                tempLeft = (q2.X - q1.X) * (p1.Y - p2.Y) - (p2.X - p1.X) * (q1.Y - q2.Y);
                tempRight = (p1.Y - q1.Y) * (p2.X - p1.X) * (q2.X - q1.X) + q1.X * (q2.Y - q1.Y) * (p2.X - p1.X) - p1.X * (p2.Y - p1.Y) * (q2.X - q1.X);
                IPoint.X = tempRight / tempLeft;

                tempLeft = (p1.X - p2.X) * (q2.Y - q1.Y) - (p2.Y - p1.Y) * (q1.X - q2.X);
                tempRight = p2.Y * (p1.X - p2.X) * (q2.Y - q1.Y) + (q2.X - p2.X) * (q2.Y - q1.Y) * (p1.Y - p2.Y) - q2.Y * (q1.X - q2.X) * (p2.Y - p1.Y);
                IPoint.Y = tempRight / tempLeft;
            }

            return IPoint;
        }

        private static bool TwoPointsInside(int a1, int a2, int b1, int b2)
        {
            if (a2 < a1)
            {
                int c = a1;
                a1 = a2;
                a2 = c;
            }

            if (b1 >= a1 && b1 <= a2)
            {
                if (b2 >= a1 && b2 <= a2)
                    return true;
                else
                    return false;
            }
            else
            {
                if (!(b2 >= a1 && b2 <= a2))
                    return true;
                else
                    return false;
            }
        }

        private static bool TwoPointsInside_Back(int a1, int a2, int b1, int b2)
        {
            if (a2 < a1)
                a1 += 1;
            if (b1 < a1)
                a1 += 1;
            if (b1 < a2)
                a2 += 1;


            if (a2 < a1)
            {
                int c = a1;
                a1 = a2;
                a2 = c;
            }

            if (b1 > a1 && b1 <= a2)
            {
                if (b2 > a1 && b2 <= a2)
                    return true;
                else
                    return false;
            }
            else
            {
                if (!(b2 > a1 && b2 <= a2))
                    return true;
                else
                    return false;
            }
        }

        private static void AddHoles_Ring(ref List<Polygon> polygonList, List<List<PointD>> holeList)
        {
            int i, j;
            for (i = 0; i < holeList.Count; i++)
            {
                List<PointD> holePs = holeList[i];
                Extent aExtent = MIMath.GetPointsExtent(holePs);
                for (j = 0; j < polygonList.Count; j++)
                {
                    Polygon aPolygon = polygonList[j];
                    if (aPolygon.Extent.Include(aExtent))
                    {
                        bool isHole = true;
                        foreach (PointD aP in holePs)
                        {
                            if (!PointInPolygon(aPolygon.OutLine, aP))
                            {
                                isHole = false;
                                break;
                            }
                        }
                        if (isHole)
                        {
                            aPolygon.AddHole(holePs);
                            polygonList[j] = aPolygon;
                            break;
                        }
                    }
                }
            }
        }        

        #endregion

        #region Earth
        private static double Rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        /// <summary>
        /// Get distance between two points on the earth
        /// </summary>
        /// <param name="lat1">latitude of point 1</param>
        /// <param name="lon1">longitude of point 1</param>
        /// <param name="lat2">latitude of point 2</param>
        /// <param name="lon2">longitude of point 2</param>
        /// <returns>distance - km</returns>
        public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double radLat1 = Rad(lat1);
            double radLat2 = Rad(lat2);
            double a = radLat1 - radLat2;
            double b = Rad(lon1) - Rad(lon2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
                Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;

            return s;
        }

        /// <summary>
        /// Get distance
        /// </summary>
        /// <param name="points">point list</param>
        /// <param name="isLonLat">if is lon_lat</param>
        /// <returns>distance</returns>
        public static double GetDistance(List<PointD> points, bool isLonLat)
        {
            double tdis = 0.0;
            for (int i = 0; i < points.Count - 1; i++)
            {
                double ax = points[i].X;
                double ay = points[i].Y;
                double bx = points[i + 1].X;
                double by = points[i + 1].Y;
                double dx = Math.Abs(bx - ax);
                double dy = Math.Abs(by - ay);
                double dist;
                if (isLonLat)
                {
                    double y = (by + ay) / 2;
                    double factor = Math.Cos(y * Math.PI / 180);
                    dx *= factor;
                    dist = Math.Sqrt(dx * dx + dy * dy);
                    dist = dist * 111319.5;
                }
                else
                    dist = Math.Sqrt(dx * dx + dy * dy);

                tdis += dist;
            }

            return tdis;
        }

        ///// <summary>
        ///// Get polygon area on earth surface
        ///// </summary>
        ///// <param name="points">point list</param>
        ///// <param name="isLonLat">if is lon/lat</param>
        ///// <returns>area</returns>
        //public static double GetArea(List<PointD> points, bool isLonLat)
        //{

        //    int Count = points.Count;
        //    if (Count > 2)
        //    {
        //        double mtotalArea = 0;


        //        if (isLonLat)
        //        {
        //            double LowX = 0.0;
        //            double LowY = 0.0;
        //            double MiddleX = 0.0;
        //            double MiddleY = 0.0;
        //            double HighX = 0.0;
        //            double HighY = 0.0;

        //            double AM = 0.0;
        //            double BM = 0.0;
        //            double CM = 0.0;

        //            double AL = 0.0;
        //            double BL = 0.0;
        //            double CL = 0.0;

        //            double AH = 0.0;
        //            double BH = 0.0;
        //            double CH = 0.0;

        //            double CoefficientL = 0.0;
        //            double CoefficientH = 0.0;

        //            double ALtangent = 0.0;
        //            double BLtangent = 0.0;
        //            double CLtangent = 0.0;

        //            double AHtangent = 0.0;
        //            double BHtangent = 0.0;
        //            double CHtangent = 0.0;

        //            double ANormalLine = 0.0;
        //            double BNormalLine = 0.0;
        //            double CNormalLine = 0.0;

        //            double OrientationValue = 0.0;

        //            double AngleCos = 0.0;

        //            double Sum1 = 0.0;
        //            double Sum2 = 0.0;
        //            int Count2 = 0;
        //            int Count1 = 0;


        //            double Sum = 0.0;                    

        //            for (int i = 0; i < Count; i++)
        //            {
        //                if (i == 0)
        //                {
        //                    LowX = points[Count - 1].X * Math.PI / 180;
        //                    LowY = points[Count - 1].Y * Math.PI / 180;
        //                    MiddleX = points[0].X * Math.PI / 180;
        //                    MiddleY = points[0].Y * Math.PI / 180;
        //                    HighX = points[1].X * Math.PI / 180;
        //                    HighY = points[1].Y * Math.PI / 180;
        //                }
        //                else if (i == Count - 1)
        //                {
        //                    LowX = points[Count - 2].X * Math.PI / 180;
        //                    LowY = points[Count - 2].Y * Math.PI / 180;
        //                    MiddleX = points[Count - 1].X * Math.PI / 180;
        //                    MiddleY = points[Count - 1].Y * Math.PI / 180;
        //                    HighX = points[0].X * Math.PI / 180;
        //                    HighY = points[0].Y * Math.PI / 180;
        //                }
        //                else
        //                {
        //                    LowX = points[i - 1].X * Math.PI / 180;
        //                    LowY = points[i - 1].Y * Math.PI / 180;
        //                    MiddleX = points[i].X * Math.PI / 180;
        //                    MiddleY = points[i].Y * Math.PI / 180;
        //                    HighX = points[i + 1].X * Math.PI / 180;
        //                    HighY = points[i + 1].Y * Math.PI / 180;
        //                }

        //                AM = Math.Cos(MiddleY) * Math.Cos(MiddleX);
        //                BM = Math.Cos(MiddleY) * Math.Sin(MiddleX);
        //                CM = Math.Sin(MiddleY);
        //                AL = Math.Cos(LowY) * Math.Cos(LowX);
        //                BL = Math.Cos(LowY) * Math.Sin(LowX);
        //                CL = Math.Sin(LowY);
        //                AH = Math.Cos(HighY) * Math.Cos(HighX);
        //                BH = Math.Cos(HighY) * Math.Sin(HighX);
        //                CH = Math.Sin(HighY);


        //                CoefficientL = (AM * AM + BM * BM + CM * CM) / (AM * AL + BM * BL + CM * CL);
        //                CoefficientH = (AM * AM + BM * BM + CM * CM) / (AM * AH + BM * BH + CM * CH);

        //                ALtangent = CoefficientL * AL - AM;
        //                BLtangent = CoefficientL * BL - BM;
        //                CLtangent = CoefficientL * CL - CM;
        //                AHtangent = CoefficientH * AH - AM;
        //                BHtangent = CoefficientH * BH - BM;
        //                CHtangent = CoefficientH * CH - CM;

        //                if (AHtangent * ALtangent + BHtangent * BLtangent + CHtangent * CLtangent == 0)
        //                    AngleCos = 0;
        //                else
        //                    AngleCos = (AHtangent * ALtangent + BHtangent * BLtangent + CHtangent * CLtangent)
        //                        / (Math.Sqrt(AHtangent * AHtangent + BHtangent * BHtangent + CHtangent * CHtangent)
        //                        * Math.Sqrt(ALtangent * ALtangent + BLtangent * BLtangent + CLtangent * CLtangent));

        //                AngleCos = Math.Acos(AngleCos);

        //                ANormalLine = BHtangent * CLtangent - CHtangent * BLtangent;
        //                BNormalLine = 0 - (AHtangent * CLtangent - CHtangent * ALtangent);
        //                CNormalLine = AHtangent * BLtangent - BHtangent * ALtangent;

        //                if (AM != 0)
        //                    OrientationValue = ANormalLine / AM;
        //                else if (BM != 0)
        //                    OrientationValue = BNormalLine / BM;
        //                else
        //                    OrientationValue = CNormalLine / CM;

        //                if (OrientationValue > 0)
        //                {
        //                    Sum1 += AngleCos;
        //                    Count1++;

        //                }
        //                else
        //                {
        //                    Sum2 += AngleCos;
        //                    Count2++;
        //                    //Sum +=2*Math.PI-AngleCos;
        //                }

        //            }

        //            if (Sum1 > Sum2)
        //            {
        //                Sum = Sum1 + (2 * Math.PI * Count2 - Sum2);
        //            }
        //            else
        //            {
        //                Sum = (2 * Math.PI * Count1 - Sum1) + Sum2;
        //            }
                    
        //            mtotalArea = (Sum - (Count - 2) * Math.PI) * EARTH_RADIUS * EARTH_RADIUS;
        //        }
        //        else
        //        {

        //            int i, j;
        //            double p1x, p1y;
        //            double p2x, p2y;
        //            for (i = Count - 1, j = 0; j < Count; i = j, j++)
        //            {

        //                p1x = points[i].X;
        //                p1y = points[i].Y;

        //                p2x = points[j].X;
        //                p2y = points[j].Y;

        //                mtotalArea += p1x * p2y - p2x * p1y;
        //            }
        //            mtotalArea /= 2.0;
        //        }
        //        return mtotalArea;
        //    }
        //    return -1;
        //}

        /// <summary>
        /// Get polygon area on earth surface
        /// </summary>
        /// <param name="points">point list</param>
        /// <param name="isLonLat">if is lon/lat</param>
        /// <returns>area</returns>
        public static double GetArea(List<PointD> points, bool isLonLat)
        {

            int Count = points.Count;
            if (Count > 2)
            {
                double mtotalArea = 0;


                if (isLonLat)
                {
                    return SphericalPolygonArea(points, EARTH_RADIUS);
                }
                else
                {
                    int i, j;
                    double p1x, p1y;
                    double p2x, p2y;
                    for (i = Count - 1, j = 0; j < Count; i = j, j++)
                    {

                        p1x = points[i].X;
                        p1y = points[i].Y;

                        p2x = points[j].X;
                        p2y = points[j].Y;

                        mtotalArea += p1x * p2y - p2x * p1y;
                    }
                    mtotalArea /= 2.0;

                    if (mtotalArea < 0)
                        mtotalArea = -mtotalArea;

                    return mtotalArea;
                }
            }
            return 0;
        }

        /// <summary>
        /// Get polygon area on earth surface
        /// </summary>
        /// <param name="points">point list</param>
        /// <returns>area</returns>
        public static double GetArea(List<PointD> points)
        {
            return GetArea(points, false);            
        }

        /// <summary>
        /// Get polygon area on earth surface
        /// </summary>
        /// <param name="points">point list</param>
        /// <returns>area</returns>
        public static double GetArea(List<PointM> points)
        {
            List<PointD> newPoints = new List<PointD>();
            foreach (PointM aP in points)
                newPoints.Add(new PointD(aP.X, aP.Y));

            return GetArea(newPoints, false);
        } 

        /// <summary>
        /// Get polygon area on earth surface
        /// </summary>
        /// <param name="points">point list</param>
        /// <returns>area</returns>
        public static double CalArea(List<PointD> points)
        {
            if (points.Count < 3)
                return 0.0;

            double sum = 0.0;
            for (int i = 0; i < points.Count - 1; i++)
            {
                double bx = points[i].X;
                double by = points[i].Y;
                double cx = points[i + 1].X;
                double cy = points[i + 1].Y;
                sum += (bx + cx) * (cy - by);
            }
            return -sum / 2.0;
        }

        /// <summary>
        /// Compute the Area of a Spherical Polygon
        /// </summary>
        /// <param name="points">lon/lat point list</param>
        /// <returns>area</returns>
        public static double SphericalPolygonArea(List<PointD> points)
        {
            return SphericalPolygonArea(points, EARTH_RADIUS * 1000);
        }

        /// <summary>
        /// Compute the Area of a Spherical Polygon
        /// </summary>
        /// <param name="points">lon/lat point list</param>
        /// <returns>area</returns>
        public static double SphericalPolygonArea(List<PointM> points)
        {
            List<PointD> newPoints = new List<PointD>();
            foreach (PointM aP in points)
                newPoints.Add(new PointD(aP.X, aP.Y));

            return SphericalPolygonArea(newPoints, EARTH_RADIUS * 1000);
        }

        /// <summary>
        /// Compute the Area of a Spherical Polygon
        /// </summary>
        /// <param name="points">lon/lat point list</param>
        /// <param name="r">shperical radius</param>
        /// <returns>area</returns>
        public static double SphericalPolygonArea(List<PointD> points, double r)
        {
            double[] lat = new double[points.Count];
            double[] lon = new double[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                lon[i] = Rad(points[i].X);
                lat[i] = Rad(points[i].Y);
            }

            return SphericalPolygonArea(lat, lon, r);
        }

        /// <summary>
        /// Haversine function : hav(x) = (1-cos(x))/2
        /// </summary>
        /// <param name="x"></param>
        /// <returns>Returns the value of Haversine function</returns>
        public static double Haversine(double x)
        {
            return (1.0 - Math.Cos(x)) / 2.0;
        }

        /// <summary>
        /// Compute the Area of a Spherical Polygon
        /// </summary>
        /// <param name="lat">the latitudes of all vertices(in radian)</param>
        /// <param name="lon">the longitudes of all vertices(in radian)</param>
        /// <param name="r">spherical radius</param>
        /// <returns>Returns the area of a spherical polygon</returns>
        public static double SphericalPolygonArea(double[] lat, double[] lon, double r)
        {
            double lam1 = 0, lam2 = 0, beta1 = 0, beta2 = 0, cosB1 = 0, cosB2 = 0;
            double hav = 0;
            double sum = 0;

            for (int j = 0; j < lat.Length; j++)
            {
                int k = j + 1;
                if (j == 0)
                {
                    lam1 = lon[j];
                    beta1 = lat[j];
                    lam2 = lon[j + 1];
                    beta2 = lat[j + 1];
                    cosB1 = Math.Cos(beta1);
                    cosB2 = Math.Cos(beta2);
                }
                else
                {
                    k = (j + 1) % lat.Length;
                    lam1 = lam2;
                    beta1 = beta2;
                    lam2 = lon[k];
                    beta2 = lat[k];
                    cosB1 = cosB2;
                    cosB2 = Math.Cos(beta2);
                }
                if (lam1 != lam2)
                {
                    hav = Haversine(beta2 - beta1) +
                                     cosB1 * cosB2 * Haversine(lam2 - lam1);
                    double a = 2 * Math.Asin(Math.Sqrt(hav));
                    double b = Math.PI / 2 - beta2;
                    double c = Math.PI / 2 - beta1;
                    double s = 0.5 * (a + b + c);
                    double t = Math.Tan(s / 2) * Math.Tan((s - a) / 2) *
                                           Math.Tan((s - b) / 2) * Math.Tan((s - c) / 2);

                    double excess = Math.Abs(4 * Math.Atan(Math.Sqrt(
                                                   Math.Abs(t))));

                    if (lam2 < lam1)
                    {
                        excess = -excess;
                    }

                    sum += excess;
                }
            }
            return Math.Abs(sum) * r * r;
        }

        #endregion

    }
}
