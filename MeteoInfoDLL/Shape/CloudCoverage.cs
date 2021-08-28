using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Cloud coverage
    /// </summary>
    public struct CloudCoverage
    {
        /// <summary>
        /// Start point
        /// </summary>
        public PointD sPoint;
        /// <summary>
        /// Size
        /// </summary>
        public Single size;
        /// <summary>
        /// Cloud cover
        /// </summary>
        public int cloudCover;
        /// <summary>
        /// Value
        /// </summary>
        public Single value;
    }
}
