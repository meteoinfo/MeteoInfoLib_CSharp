using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MapData
{
    /// <summary>
    /// Shape file type
    /// </summary>
    public enum ShapeFileType
    {
        /// <summary>
        /// Null shape
        /// </summary>
        NullShape,
        /// <summary>
        /// Point
        /// </summary>
        Point,
        /// <summary>
        /// Polyline
        /// </summary>
        PolyLine,
        /// <summary>
        /// Polygon
        /// </summary>
        Polygon,
        /// <summary>
        /// MutiPoint
        /// </summary>
        MutiPoint,
        /// <summary>
        /// PointZ
        /// </summary>
        PointZ,
        /// <summary>
        /// PolyLineZ
        /// </summary>
        PolyLineZ,
        /// <summary>
        /// PolygonZ
        /// </summary>
        PolygonZ,
        /// <summary>
        /// MutiPointZ
        /// </summary>
        MutiPointZ,
        /// <summary>
        /// PointM
        /// </summary>
        PointM,
        /// <summary>
        /// PolyLineM
        /// </summary>
        PolyLineM,
        /// <summary>
        /// PolygonM
        /// </summary>
        PolygonM,
        /// <summary>
        /// MutiPointM
        /// </summary>
        MultiPointM,
        /// <summary>
        /// MutiPath
        /// </summary>
        MultiPatch
    }
}
