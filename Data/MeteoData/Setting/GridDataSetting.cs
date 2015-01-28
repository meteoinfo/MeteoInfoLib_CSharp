using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Grid data parameter set
    /// </summary>
    public class GridDataSetting
    {
        /// <summary>
        /// Data extent
        /// </summary>
        public Extent DataExtent = new Extent();
        /// <summary>
        /// X number
        /// </summary>
        public int XNum;
        /// <summary>
        /// Y number
        /// </summary>
        public int YNum;
    }
}
