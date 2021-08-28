using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Geoprocess
{
    /// <summary>
    /// Spatial query type enum
    /// </summary>
    public enum SpatialQueryTypes
    {
        /// <summary>
        /// One feature is within another one
        /// </summary>
        Within,
        /// <summary>
        /// One feature contain another one
        /// </summary>
        Contain
    }
}
