using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Shape type
    /// </summary>
    public enum ShapeTypes
    {
        /// <summary>
        /// Point
        /// </summary>
        Point = 1,        
        /// <summary>
        /// Polyline
        /// </summary>
        Polyline = 3,
        /// <summary>
        /// Polygon
        /// </summary>
        Polygon = 5,
        /// <summary>
        /// PointZ
        /// </summary>
        PointZ = 11,
        /// <summary>
        /// PolyLineZ
        /// </summary>
        PolylineZ = 13,
        /// <summary>
        /// Measured Point
        /// </summary>
        PointM = 21,
        /// <summary>
        /// Measured Polyline
        /// </summary>
        PolylineM = 23,
        /// <summary>
        /// Measured Polygon
        /// </summary>
        PolygonM = 25,
        /// <summary>
        /// Wind Arraw
        /// </summary>
        WindArraw = 41,
        /// <summary>
        /// WindBarb
        /// </summary>
        WindBarb = 42,
        /// <summary>
        /// WeatherSymbol
        /// </summary>
        WeatherSymbol = 43,
        /// <summary>
        /// StationModel
        /// </summary>
        StationModel = 44,
        /// <summary>
        /// Image
        /// </summary>
        Image = 99,
        /// <summary>
        /// Rectangle
        /// </summary>
        Rectangle = 51,
        /// <summary>
        /// Curve line
        /// </summary>
        CurveLine = 52,
        /// <summary>
        /// Curve polygon
        /// </summary>
        CurvePolygon = 53,
        /// <summary>
        /// Ellipse
        /// </summary>
        Ellipse = 54,
        /// <summary>
        /// Circle
        /// </summary>
        Circle = 55
    }
}
