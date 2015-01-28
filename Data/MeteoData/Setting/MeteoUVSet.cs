using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Wind U V variable names
    /// </summary>
    public class MeteoUVSet
    {
        #region Variable

        private string _uStr;
        private string _vStr;
        private bool _isFixUVStr;
        private bool _isUV;
        private int _skipX;
        private int _skipY;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MeteoUVSet()
        {
            _uStr = "U";
            _vStr = "V";
            _isFixUVStr = false;
            _isUV = true;
            _skipX = 1;
            _skipY = 1;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get or set U variable name
        /// </summary>
        public string UStr
        {
            get { return _uStr; }
            set { _uStr = value; }
        }

        /// <summary>
        /// Get or set V variable name
        /// </summary>
        public string VStr
        {
            get { return _vStr; }
            set { _vStr = value; }
        }

        /// <summary>
        /// Get or set if fix U/V variable names
        /// </summary>
        public bool IsFixUVStr
        {
            get { return _isFixUVStr; }
            set { _isFixUVStr = value; }
        }

        /// <summary>
        /// Get or set if is U/V or Direction/Speed
        /// </summary>
        public bool IsUV
        {
            get { return _isUV; }
            set { _isUV = value; }
        }

        /// <summary>
        /// Get or set skip X
        /// </summary>
        public int SkipX
        {
            get { return _skipX; }
            set { _skipX = value; }
        }

        /// <summary>
        /// Get or set skip Y
        /// </summary>
        public int SkipY
        {
            get { return _skipY; }
            set { _skipY = value; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Try to set U/V variable names automatic
        /// </summary>
        /// <param name="vList">Variables list</param>
        /// <returns>Boolean result if can find U/V variables</returns>
        public Boolean AutoSetUVStr(List<string> vList)
        {
            Boolean isOK = false;
            if (vList.Contains("U") && vList.Contains("V"))
            {
                UStr = "U";
                VStr = "V";
                isOK = true;
            }

            if (vList.Contains("u") && vList.Contains("v"))
            {
                UStr = "u";
                VStr = "v";
                isOK = true;
            }

            if (vList.Contains("U10M") && vList.Contains("V10M"))
            {
                UStr = "U10M";
                VStr = "V10M";
                isOK = true;
            }

            if (vList.Contains("UWND") && vList.Contains("VWND"))
            {
                UStr = "UWND";
                VStr = "VWND";
                isOK = true;
            }

            if (vList.Contains("uwnd") && vList.Contains("vwnd"))
            {
                UStr = "uwnd";
                VStr = "vwnd";
                isOK = true;
            }

            if (vList.Contains("ugrd") && vList.Contains("vgrd"))
            {
                UStr = "ugrd";
                VStr = "vgrd";
                isOK = true;
            }

            return isOK;
        }
        #endregion
    }
}
