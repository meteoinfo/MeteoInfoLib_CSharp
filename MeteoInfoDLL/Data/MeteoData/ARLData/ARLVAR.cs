using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// ARL variable set
    /// </summary>
    public class ARLVAR
    {
        /// <summary>
        /// Variable name
        /// </summary>
        public string VName;
        /// <summary>
        /// Level number
        /// </summary>
        public int LevelNum;
        /// <summary>
        /// Level idex list
        /// </summary>
        public List<int> LevelIdxs;
        /// <summary>
        /// Variable in level index list
        /// </summary>
        public List<int> VarInLevelIdxs;

        /// <summary>
        /// Constructor
        /// </summary>
        public ARLVAR()
        {
            LevelIdxs = new List<int>();
            VarInLevelIdxs = new List<int>();
        }
    }
}
