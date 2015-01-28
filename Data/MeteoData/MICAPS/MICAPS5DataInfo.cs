using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// MICAPS 5 data info
    /// </summary>
    public class MICAPS5DataInfo:DataInfo
    {
        #region Variables        
        /// <summary>
        /// Station number
        /// </summary>
        public int StNum;
        /// <summary>
        /// Variable number
        /// </summary>
        public int VarNum;
        /// <summary>
        /// Field list
        /// </summary>
        public List<string> FieldList;
        /// <summary>
        /// Variable list
        /// </summary>
        public List<string> VarList;
        private string Description;
        private DateTime time;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MICAPS5DataInfo()
        {
            string[] items = new string[] {"Height","Temperature","DepDewPoint","WindDirection","WindSpeed"};
            VarList = new List<string>(items.Length);
            VarList.AddRange(items);
            FieldList = new List<string>();
            FieldList.AddRange(new string[] { "Stid", "Longitude", "Latitude", "Altitude", "Grade" });
            FieldList.AddRange(VarList);           
            this.MissingValue = 9999;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read MICAPS 5 data info
        /// </summary>
        /// <param name="aFile">file path</param>       
        public override void ReadDataInfo(string aFile)
        {
            StreamReader sr = new StreamReader(aFile, Encoding.Default);
            string aLine;
            string[] dataArray;
            int i, LastNonEmpty;
            List<string> dataList = new List<string>();            

            //Read file head
            FileName = aFile;
            aLine = sr.ReadLine();
            Description = aLine;            
            aLine = sr.ReadLine();
            dataArray = aLine.Split();
            LastNonEmpty = -1;
            dataList.Clear();
            for (i = 0; i < dataArray.Length; i++)
            {
                if (dataArray[i] != string.Empty)
                {
                    LastNonEmpty++;
                    dataList.Add(dataArray[i]);
                }
            }
            //DateTime = DateTime.ParseExact(dataList[0] + "-" + dataList[1] + "-" + dataList[2] +
            //    " " + dataList[3] + ":00", "yy-MM-dd HH:mm", null);
            int year = int.Parse(dataList[0]);
            if (year < 100)
            {
                if (year < 50)
                    year = 2000 + year;
                else
                    year = 1900 + year;
            }
            time = new DateTime(year, int.Parse(dataList[1]), int.Parse(dataList[2]), int.Parse(dataList[3]), 0, 0);
            
            StNum = int.Parse(dataList[4]);            

            sr.Close();

            Dimension tdim = new Dimension(DimensionType.T);
            tdim.DimValue.Add(DataConvert.ToDouble(time));
            tdim.DimLength = 1;
            this.TimeDimension = tdim;
            List<Variable> variables = new List<Variable>();
            foreach (string vName in VarList)
            {
                Variable var = new Variable();
                var.Name = vName;
                var.IsStation = true;
                var.SetDimension(tdim);
                variables.Add(var);
            }
            this.Variables = variables;
        }

        /// <summary>
        /// Generate data info text of MICAPS 5
        /// </summary>       
        /// <returns>info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + FileName;
            dataInfo += Environment.NewLine + "Description: " + Description;
            dataInfo += Environment.NewLine + "Time: " + time.ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "Station Number: " + StNum;

            return dataInfo;
        }

        /// <summary>
        /// Get one station vertical data
        /// </summary>
        /// <param name="stId">station identifer</param>
        /// <returns>station vertical data</returns>
        public List<double[]> GetOneStationData(string stId)
        {
            List<double[]> vData = new List<double[]>();
            StreamReader sr = new StreamReader(FileName, Encoding.Default);
            string aLine;
            string[] dataArray;
            int i, LastNonEmpty;
            List<string> dataList = new List<string>();
            int lNum;

            sr.ReadLine();
            sr.ReadLine();
            aLine = sr.ReadLine();
            while (aLine != null)
            {
                if (aLine.Substring(0, 3) == "   ")
                {
                    aLine = sr.ReadLine();
                    continue;
                }

                if (aLine.Length > 5 && aLine.Substring(0, 5) == stId)
                {
                    dataArray = aLine.Split();
                    LastNonEmpty = -1;
                    dataList.Clear();
                    for (i = 0; i < dataArray.Length; i++)
                    {
                        if (dataArray[i] != string.Empty)
                        {
                            LastNonEmpty++;
                            dataList.Add(dataArray[i]);
                        }
                    }
                    lNum = int.Parse(dataList[4]) / 6;

                    for (int l = 0; l < lNum; l++)
                    {
                        aLine = sr.ReadLine();
                        LastNonEmpty = -1;
                        dataList.Clear();
                        for (i = 0; i < dataArray.Length; i++)
                        {
                            if (dataArray[i] != string.Empty)
                            {
                                LastNonEmpty++;
                                dataList.Add(dataArray[i]);
                            }
                        }
                        double[] theData = new double[6];
                        for (i = 0; i < 6; i++)
                            theData[i] = double.Parse(dataList[i]);

                        vData.Add(theData);
                    }
                    break;
                }

                aLine = sr.ReadLine();
            }

            sr.Close();

            return vData;
        }

        #endregion
    }
}
