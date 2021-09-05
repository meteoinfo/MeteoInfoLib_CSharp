using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GrADS X dimension set
    /// </summary>
    public class XDEFS
    {
        /// <summary>
        /// Type - linear or ...
        /// </summary>
        public string Type;
        /// <summary>
        /// X number
        /// </summary>
        public int XNum;
        /// <summary>
        /// X minimum
        /// </summary>
        public Single XMin;
        /// <summary>
        /// X delta
        /// </summary>
        public Single XDelt;
        /// <summary>
        /// X array
        /// </summary>
        public double[] X;
    }
}
