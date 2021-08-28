using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GrADS station data head
    /// </summary>
    public struct STDataHead
    {
        /// <summary>
        /// Station identifer
        /// </summary>
        public string STID;
        /// <summary>
        /// Latitude
        /// </summary>
        public Single Lat;
        /// <summary>
        /// Longitude
        /// </summary>
        public Single Lon;
        /// <summary>
        /// Time
        /// </summary>
        public Single T;
        /// <summary>
        /// Level number
        /// </summary>
        public int NLev;
        /// <summary>
        /// Flag
        /// </summary>
        public int Flag;
    }
}
