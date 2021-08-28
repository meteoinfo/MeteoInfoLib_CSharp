using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GrADS station level data
    /// </summary>
    public struct STLevData
    {
        /// <summary>
        /// Level
        /// </summary>
        public Single lev;
        /// <summary>
        /// Data array
        /// </summary>
        public Single[] data;
    }
}
