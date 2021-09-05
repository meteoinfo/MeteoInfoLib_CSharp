using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Base MICAPS data info class
    /// </summary>
    public class MICAPSDataInfo
    {
        #region Variables
        /// <summary>
        /// File name
        /// </summary>
        public string FileName;
        /// <summary>
        /// Description
        /// </summary>
        public string Description;
        /// <summary>
        /// DateTime
        /// </summary>
        public DateTime DateTime;
        /// <summary>
        /// Undefine data
        /// </summary>
        public double UNDEF;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MICAPSDataInfo()
        {
            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Read MICAPS data head
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public string ReadMICAPSHead(string aFile)
        {
            string dataType;
            StreamReader sr = new StreamReader(aFile);
            string aLine;
            string[] dataArray;
            List<string> dataList = new List<string>();

            aLine = sr.ReadLine();
            dataArray = aLine.Split();
            int LastNonEmpty = -1;
            dataList.Clear();
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (dataArray[i] != string.Empty)
                {
                    LastNonEmpty++;
                    dataList.Add(dataArray[i]);
                }
            }
            dataType = dataList[0] + " " + dataList[1];
            sr.Close();

            dataType = dataType.Trim().ToLower();
            return dataType;
        }

        #endregion
    }
}
