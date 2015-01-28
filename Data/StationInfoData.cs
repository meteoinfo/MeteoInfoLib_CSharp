using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data
{
    /// <summary>
    /// Station Info data - multi variables
    /// </summary>
    public class StationInfoData
    {
        #region Variables
        private List<string> _fields = new List<string>();
        private List<string> _variables = new List<string>();
        private List<List<string>> _dataList = new List<List<string>>();
        private List<string> _stations = new List<string>();

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public StationInfoData()
        {
            //_fields.Add("Stid");
            //_fields.Add("X");
            //_fields.Add("Y");
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set variables
        /// </summary>
        public List<string> Variables
        {
            get { return _variables; }
            set { _variables = value; }
        }

        /// <summary>
        /// Get or set fields
        /// </summary>
        public List<string> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        /// <summary>
        /// Get or set station identifer list
        /// </summary>
        public List<string> Stations
        {
            get { return _stations; }
            set { _stations = value; }
        }

        /// <summary>
        /// Get or set data list - the first three columns are stid, lon and lat
        /// </summary>
        public List<List<string>> DataList
        {
            get { return _dataList; }
            set { _dataList = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Save the station info data to CSV file
        /// </summary>
        /// <param name="aFile">File path</param>
        public void SaveAsCSVFile(string aFile)
        {
            StreamWriter sw = new StreamWriter(aFile, false, Encoding.Default);

            string aStr = "";
            for (int i = 0; i < _fields.Count; i++)
            {
                if (i == 0)
                    aStr = _fields[i];
                else
                    aStr += "," + _fields[i];
            }
            sw.WriteLine(aStr);
            for (int i = 0; i < _dataList.Count; i++)
            {
                List<string> dList = _dataList[i];
                for (int j = 0; j < dList.Count; j++)
                {
                    if (j == 0)
                        aStr = dList[j];
                    else
                        aStr += "," + dList[j];
                }
                sw.WriteLine(aStr);
            }

            sw.Close();
        }

        #endregion
    }
}
