using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GrADS Time dimension set
    /// </summary>
    public class TDEFS
    {
        /// <summary>
        /// Type - linear or ...
        /// </summary>
        public string Type;
        /// <summary>
        /// Time number
        /// </summary>
        public int TNum;
        /// <summary>
        /// Start time
        /// </summary>
        public DateTime STime;
        /// <summary>
        /// Time delt
        /// </summary>
        public string TDelt;
        /// <summary>
        /// Time delta value
        /// </summary>
        public int DeltaValue;
        /// <summary>
        /// Time unit
        /// </summary>
        public String Unit;
        /// <summary>
        /// Time array
        /// </summary>
        public DateTime[] times;
    }
}
