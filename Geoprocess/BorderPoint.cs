using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Geoprocess
{
    /// <summary>
    /// Border point
    /// </summary>
    public struct BorderPoint
    {
        /// <summary>
        /// Identifer
        /// </summary>
        public int Id;
        /// <summary>
        /// Border index
        /// </summary>
        public int BorderIdx;
        /// <summary>
        /// Border inner index
        /// </summary>
        public int BInnerIdx;
        /// <summary>
        /// Point
        /// </summary>
        public PointD Point;
        /// <summary>
        /// Value
        /// </summary>
        public double Value;
    }
}
