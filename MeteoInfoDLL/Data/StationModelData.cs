using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data
{
    /// <summary>
    /// Station model data
    /// </summary>
    public class StationModelData
    {
        #region Variables
        private List<StationModel> _data = new List<StationModel>();
        private Extent _dataExtent = new Extent();
        private double _unDef = -9999.0;

        #endregion

        #region Properties
        /// <summary>
        /// Get or set data
        /// </summary>
        public List<StationModel> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// Get or set data extent
        /// </summary>
        public Extent DataExtent
        {
            get { return _dataExtent; }
            set { _dataExtent = value; }
        }

        /// <summary>
        /// Get or set undefine value
        /// </summary>
        public double MissingValue
        {
            get { return _unDef; }
            set { _unDef = value; }
        }

        /// <summary>
        /// Get data number
        /// </summary>
        public int DataNum
        {
            get { return _data.Count; }
        }

        #endregion
    }
}
