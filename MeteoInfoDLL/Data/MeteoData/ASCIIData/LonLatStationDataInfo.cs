using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Lon/Lat stations data info
    /// </summary>
    public class LonLatStationDataInfo : DataInfo,IStationDataInfo
    {
        #region Variables
        /// <summary>
        /// Field list
        /// </summary>
        public List<string> FieldList;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LonLatStationDataInfo()
        {
            FieldList = new List<string>();
            //VarList = new List<string>();            
            //DataList = new List<List<string>>();            
            //MissingValue = -9999;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read Lon/Lat station data info
        /// </summary>
        /// <param name="aFile">data file name</param>                
        public override void ReadDataInfo(string aFile)
        {
            FileName = aFile;

            //Read data
            int i;
            StreamReader sr = new StreamReader(aFile, System.Text.Encoding.UTF8);
            string[] dataArray, fieldArray;            
            string aLine = sr.ReadLine();    //Title
            fieldArray = aLine.Split(',');
            if (fieldArray.Length < 3)
            {
                MessageBox.Show("The data should have at least four fields!", "Error");
                return;
            }
            FieldList = new List<string>(fieldArray.Length);
            FieldList.AddRange(fieldArray);
            //Judge field type
            aLine = sr.ReadLine();    //First line
            dataArray = aLine.Split(',');
            List<Variable> variables = new List<Variable>();
            for (i = 3; i < dataArray.Length; i++)
            {
                if (MIMath.IsNumeric_1(dataArray[i]))
                {
                    Variable var = new Variable();
                    var.Name = fieldArray[i];
                    var.IsStation = true;
                    variables.Add(var);
                }
            }
            this.Variables = variables;

            sr.Close();
        }

        /// <summary>
        /// Generate data info text
        /// </summary>        
        /// <returns>info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + FileName;
            //dataInfo += Environment.NewLine + "Time: " + aDataInfo.dateTime.ToString("yyyy-MM-dd HH:00");
            //dataInfo += Environment.NewLine + "Station Number: " + StationNum;
            dataInfo += Environment.NewLine + "Fields: ";
            foreach (string aField in FieldList)
                dataInfo += Environment.NewLine + "  " + aField;

            return dataInfo;
        }

        ///// <summary>
        ///// Get discrete Lon/Lat station data
        ///// </summary>        
        ///// <param name="vIdx">varible index</param>
        ///// <param name="dataExtent"></param>
        ///// <returns></returns>
        //public double[,] GetDiscreteData(int vIdx, ref Global.Extent dataExtent)
        //{
        //    string stName;
        //    int i;
        //    double lon, lat;
        //    double t;
        //    t = 0;

        //    List<string> dataList = new List<string>();
        //    double[,] DiscreteData = new double[3, DataList.Count];
        //    double minX, maxX, minY, maxY;
        //    minX = 0;
        //    maxX = 0;
        //    minY = 0;
        //    maxY = 0;            

        //    //Get real variable index
        //    int varIdx = FieldList.IndexOf(VarList[vIdx]);

        //    for (i = 0; i < DataList.Count; i++)
        //    {
        //        dataList = DataList[i];
        //        stName = dataList[0];
        //        lon = double.Parse(dataList[1]);
        //        lat = double.Parse(dataList[2]);
        //        t = double.Parse(dataList[varIdx]);
        //        //if (lon < 0)
        //        //{
        //        //    lon += 360;
        //        //}
        //        //Initialize data
        //        DiscreteData[0, i] = lon;
        //        DiscreteData[1, i] = lat;
        //        DiscreteData[2, i] = t;

        //        if (i == 0)
        //        {
        //            minX = lon;
        //            maxX = minX;
        //            minY = lat;
        //            maxY = minY;
        //        }
        //        else
        //        {
        //            if (minX > lon)
        //            {
        //                minX = lon;
        //            }
        //            else if (maxX < lon)
        //            {
        //                maxX = lon;
        //            }
        //            if (minY > lat)
        //            {
        //                minY = lat;
        //            }
        //            else if (maxY < lat)
        //            {
        //                maxY = lat;
        //            }
        //        }
        //    }
        //    dataExtent.minX = minX;
        //    dataExtent.maxX = maxX;
        //    dataExtent.minY = minY;
        //    dataExtent.maxY = maxY;

        //    return DiscreteData;
        //}

        /// <summary>
        /// Read station data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Station data</returns>
        public StationData GetStationData(int timeIdx, int varIdx, int levelIdx)
        {
            StreamReader sr = new StreamReader(this.FileName, System.Text.Encoding.UTF8);
            List<string[]> dataList = new List<string[]>();
            sr.ReadLine();
            string line = sr.ReadLine();
            while (line != null)
            {
                if (line == String.Empty)
                {
                    line = sr.ReadLine();
                    continue;
                }
                dataList.Add(line.Split(','));
                line = sr.ReadLine();
            }
            sr.Close();


            StationData stationData = new StationData();
            List<string> stations = new List<string>();
            string stName;
            int i;
            double lon, lat;
            double t;
            t = 0;

            string[] dataArray;
            double[,] DiscreteData = new double[3, dataList.Count];
            double minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;            

            //Get real variable index
            int varIdx1 = FieldList.IndexOf(this.Variables[varIdx].Name);
            String vstr;
            for (i = 0; i < dataList.Count; i++)
            {
                dataArray = dataList[i];
                stName = dataArray[0];
                lon = double.Parse(dataArray[1]);
                lat = double.Parse(dataArray[2]);
                vstr = dataArray[varIdx1];
                if (String.IsNullOrEmpty(vstr))
                    t = stationData.MissingValue;
                else
                    t = double.Parse(dataArray[varIdx1]);
                //if (lon < 0)
                //{
                //    lon += 360;
                //}
                //Initialize data
                DiscreteData[0, i] = lon;
                DiscreteData[1, i] = lat;
                DiscreteData[2, i] = t;
                stations.Add(stName);

                if (i == 0)
                {
                    minX = lon;
                    maxX = minX;
                    minY = lat;
                    maxY = minY;
                }
                else
                {
                    if (minX > lon)
                    {
                        minX = lon;
                    }
                    else if (maxX < lon)
                    {
                        maxX = lon;
                    }
                    if (minY > lat)
                    {
                        minY = lat;
                    }
                    else if (maxY < lat)
                    {
                        maxY = lat;
                    }
                }
            }
            Extent dataExtent = new Extent();
            dataExtent.minX = minX;
            dataExtent.maxX = maxX;
            dataExtent.minY = minY;
            dataExtent.maxY = maxY;

            stationData.Data = DiscreteData;
            stationData.DataExtent = dataExtent;
            stationData.Stations = stations;

            return stationData;
        }

        /// <summary>
        /// Get Lon/Lat station data - all data are missing data
        /// </summary>               
        /// <returns>null station data</returns>
        public StationData GetNullStationData()
        {
            StreamReader sr = new StreamReader(this.FileName, System.Text.Encoding.UTF8);
            List<string[]> dataList = new List<string[]>();
            sr.ReadLine();
            string line = sr.ReadLine();
            while (line != null)
            {
                if (line == String.Empty)
                {
                    line = sr.ReadLine();
                    continue;
                }
                dataList.Add(line.Split(','));
                line = sr.ReadLine();
            }
            sr.Close();

            StationData stationData = new StationData();
            List<string> stations = new List<string>();
            string stName;
            int i;
            double lon, lat;
            double t;
            t = 0;

            string[] dataArray;
            double[,] DiscreteData = new double[3, dataList.Count];
            double minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;

            for (i = 0; i < dataList.Count; i++)
            {
                dataArray = dataList[i];
                stName = dataArray[0];
                lon = double.Parse(dataArray[1]);
                lat = double.Parse(dataArray[2]);
                t = stationData.MissingValue;
                DiscreteData[0, i] = lon;
                DiscreteData[1, i] = lat;
                DiscreteData[2, i] = t;
                stations.Add(stName);

                if (i == 0)
                {
                    minX = lon;
                    maxX = minX;
                    minY = lat;
                    maxY = minY;
                }
                else
                {
                    if (minX > lon)
                    {
                        minX = lon;
                    }
                    else if (maxX < lon)
                    {
                        maxX = lon;
                    }
                    if (minY > lat)
                    {
                        minY = lat;
                    }
                    else if (maxY < lat)
                    {
                        maxY = lat;
                    }
                }
            }
            Extent dataExtent = new Extent();
            dataExtent.minX = minX;
            dataExtent.maxX = maxX;
            dataExtent.minY = minY;
            dataExtent.maxY = maxY;

            stationData.Data = DiscreteData;
            stationData.DataExtent = dataExtent;
            stationData.Stations = stations;

            return stationData;
        }

        /// <summary>
        /// Read station info data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>StationInfoData</returns>
        public StationInfoData GetStationInfoData(int timeIdx, int levelIdx)
        {
            StreamReader sr = new StreamReader(this.FileName, System.Text.Encoding.UTF8);
            List<List<string>> dataList = new List<List<string>>();
            sr.ReadLine();
            string line = sr.ReadLine();
            while (line != null)
            {
                if (line == String.Empty)
                {
                    line = sr.ReadLine();
                    continue;
                }
                List<string> aList = new List<string>(line.Split(','));
                dataList.Add(aList);
                line = sr.ReadLine();
            }
            sr.Close();

            StationInfoData stInfoData = new StationInfoData();
            stInfoData.DataList = dataList;
            stInfoData.Fields = this.FieldList;
            stInfoData.Variables = this.VariableNames;

            return stInfoData;
        }

        /// <summary>
        /// Read station model data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Station data</returns>
        public StationModelData GetStationModelData(int timeIdx, int levelIdx)
        {
            return null;
        }

        #endregion
    }
}
