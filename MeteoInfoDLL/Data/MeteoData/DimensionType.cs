using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Dimension type enum
    /// </summary>
    public enum DimensionType
    {
        /// <summary>
        /// X dimension
        /// </summary>
        X,
        /// <summary>
        /// Y dimension
        /// </summary>
        Y,
        /// <summary>
        /// Z/Level dimension
        /// </summary>
        Z,
        /// <summary>
        /// Time dimension
        /// </summary>
        T,
        /// <summary>
        /// Xtrack dimension - for HDF EOS swath data
        /// </summary>
        Xtrack,
        /// <summary>
        /// Other dimension
        /// </summary>
        Other
    }
}
