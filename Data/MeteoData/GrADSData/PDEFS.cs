using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Projection define of GrADS data
    /// </summary>
    public class PDEFS
    {
        /// <summary>
        /// PDEF type
        /// </summary>
        public string PDEF_Type;
        /// <summary>
        /// PDEF content
        /// </summary>
        public object PDEF_Content;

        /// <summary>
        /// Constructor
        /// </summary>
        public PDEFS()
        {
            PDEF_Type = "LATLON";
        }
    }
}
