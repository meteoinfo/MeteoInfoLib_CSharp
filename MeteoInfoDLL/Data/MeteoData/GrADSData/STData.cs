using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GrADS station data
    /// </summary>
    public struct STData
    {
        /// <summary>
        /// Station data head
        /// </summary>
        public STDataHead STHead;
        /// <summary>
        /// Data list
        /// </summary>
        public List<STLevData> dataList;
    }
}
