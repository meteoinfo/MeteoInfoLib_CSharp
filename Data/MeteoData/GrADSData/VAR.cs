using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GrADS variable set
    /// </summary>
    public class VAR
    {
        #region Variables
        private string _vName;
        private int _levelNum;
        private string _units;
        private string _description;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public VAR()
        {

        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set Variable name
        /// </summary>
        public string VName
        {
            get { return _vName; }
            set { _vName = value; }
        }

        /// <summary>
        /// Get or set Level number
        /// </summary>
        public int LevelNum
        {
            get { return _levelNum; }
            set { _levelNum = value; }
        }

        /// <summary>
        /// Units
        /// </summary>
        public string Units
        {
            get { return _units; }
            set { _units = value; }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion
    }
}
