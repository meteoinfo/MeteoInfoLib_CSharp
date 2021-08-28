using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GrADS Y dimension set
    /// </summary>
    public class YDEFS
    {
        /// <summary>
        /// Type - linear or ...
        /// </summary>
        public string Type;
        /// <summary>
        /// Y number
        /// </summary>
        public int YNum;
        /// <summary>
        /// Y minimum
        /// </summary>
        public Single YMin;
        /// <summary>
        /// Y delt
        /// </summary>
        public Single YDelt;
        /// <summary>
        /// Y array
        /// </summary>
        public double[] Y;
    }
}
