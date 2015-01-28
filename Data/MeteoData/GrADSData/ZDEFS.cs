using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Z dimension set
    /// </summary>
    public class ZDEFS
    {
        /// <summary>
        /// Type - linear or ...
        /// </summary>
        public string Type;
        /// <summary>
        /// Level number
        /// </summary>
        public int ZNum;
        /// <summary>
        /// Start level
        /// </summary>
        public Single SLevel;
        /// <summary>
        /// Level delta
        /// </summary>
        public Single ZDelt;
        /// <summary>
        /// Level array
        /// </summary>
        public Single[] ZLevels;
    }
}
