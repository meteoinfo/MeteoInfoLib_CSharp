using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MapData
{
    /// <summary>
    /// Polygon shape of shapefile
    /// </summary>
    public struct Shape_Polygon
    {
        /// <summary>
        /// Extent
        /// </summary>
        public Extent extent;
        /// <summary>
        /// Part number
        /// </summary>
        public int numParts;
        /// <summary>
        /// Point number
        /// </summary>
        public int numPoints;
        /// <summary>
        /// Part array
        /// </summary>
        public int[] parts;
        /// <summary>
        /// Point array
        /// </summary>
        public PointF[] points;
    }
}
