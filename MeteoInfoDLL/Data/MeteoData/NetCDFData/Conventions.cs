using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// NetCDF conventions enum
    /// </summary>
    public enum Conventions
    {
        /// <summary>
        /// Climate and Forecast (CF) Metadata Conventions
        /// </summary>
        CF,
        /// <summary>
        /// CMAQ Model 3 IOAPI Conventions
        /// </summary>
        IOAPI,
        /// <summary>
        /// WRF Out Coventions
        /// </summary>
        WRFOUT
    }
}
