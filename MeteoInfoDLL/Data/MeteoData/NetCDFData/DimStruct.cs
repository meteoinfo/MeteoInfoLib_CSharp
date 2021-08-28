using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// NetCDF dimension struct
    /// </summary>
    public struct DimStruct
    {
        /// <summary>
        /// Dimension name
        /// </summary>
        public string dimName;
        /// <summary>
        /// Dimension length
        /// </summary>
        public int dimLen;
        /// <summary>
        /// Dimension identifer
        /// </summary>
        public int dimid;
        /// <summary>
        /// If is unlimited
        /// </summary>
        public bool IsUnlimited;
    }
}
