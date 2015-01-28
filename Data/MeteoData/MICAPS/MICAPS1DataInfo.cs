using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// MICAPS 1 data info
    /// </summary>
    public class MICAPS1DataInfo:DataInfo,IStationDataInfo
    {
        #region Variables        
        /// <summary>
        /// Station number
        /// </summary>
        public int stNum;
        /// <summary>
        /// Variable number
        /// </summary>
        public int varNum;
        /// <summary>
        /// If is auto station
        /// </summary>
        public bool isAutoStation;
        /// <summary>
        /// If has all data columns
        /// </summary>
        public bool hasAllCols;
        /// <summary>
        /// Field list
        /// </summary>
        public List<string> FieldList;
        /// <summary>
        /// Variable list
        /// </summary>
        public List<string> VarList;
        /// <summary>
        /// Data list
        /// </summary>
        public List<List<string>> DataList;
        /// <summary>
        /// Description
        /// </summary>
        public string Description;
        /// <summary>
        /// DateTime
        /// </summary>
        public DateTime DateTime;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MICAPS1DataInfo()
        {
            string[] items = new string[] {"Altitude","Grade","CloudCover","WindDirection","WindSpeed","Pressure",
                "PressVar3h","WeatherPast1","WeatherPast2","Precipitation6h", "LowCloudShape",
                "LowCloudAmount","LowCloudHeight","DewPoint","Visibility","WeatherNow",
                "Temperature","MiddleCloudShape","HighCloudShape"};
            VarList = new List<string>(items.Length);
            VarList.AddRange(items);
            FieldList = new List<string>();
            FieldList.AddRange(new string[] { "Stid", "Longitude", "Latitude"});
            FieldList.AddRange(VarList);
            DataList = new List<List<string>>();
            isAutoStation = false;
            hasAllCols = false;
            MissingValue = 9999;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read MICAPS 1 data info
        /// </summary>
        /// <param name="aFile">file path</param>       
        public override void ReadDataInfo(string aFile)
        {
            StreamReader sr = new StreamReader(aFile, Encoding.Default);
            string aLine;
            string[] dataArray;
            int i, n, LastNonEmpty, dataNum;
            List<string> dataList = new List<string>();
            List<List<string>> disDataList = new List<List<string>>();

            //Read file head
            FileName = aFile;
            aLine = sr.ReadLine();
            Description = aLine;
            if (aLine.Contains("自动"))
            {
                isAutoStation = true;
            }
            aLine = sr.ReadLine();
            if (String.IsNullOrEmpty(aLine))
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
            DateTime = new DateTime(year, int.Parse(dataList[1]), int.Parse(dataList[2]), int.Parse(dataList[3]), 0, 0);

            Dimension tdim = new Dimension(DimensionType.T);
            tdim.DimValue.Add(DataConvert.ToDouble(DateTime));
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

            stNum = Convert.ToInt32(dataList[4]);

            //Read data
            dataNum = 0;
            do
            {
                aLine = sr.ReadLine();
                if (aLine == null)
                {
                    break;
                }
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

                for (n = 0; n <= 10; n++)
                {
                    if (dataList.Count < 24)
                    {
                        aLine = sr.ReadLine();
                        if (aLine == null)
                            break;
                        dataArray = aLine.Split();
                        LastNonEmpty = -1;
                        for (i = 0; i < dataArray.Length; i++)
                        {
                            if (dataArray[i] != string.Empty)
                            {
                                LastNonEmpty++;
                                dataList.Add(dataArray[i]);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (dataList.Count < 24)
                {
                    break;
                }
                else
                {
                    for (n = 0; n < 10; n++)
                    {
                        dataList.RemoveAt(dataList.Count - 1);
                        if (dataList.Count == 22)
                            break;
                    }
                }

                //if (dataList.Count < 26 && (string)dataList[22] == "1" && (string)dataList[23] == "2")
                //{
                //    aLine = sr.ReadLine();
                //    dataArray = aLine.Split();
                //    LastNonEmpty = -1;
                //    for (i = 0; i < dataArray.Length; i++)
                //    {
                //        if (dataArray[i] != string.Empty)
                //        {
                //            LastNonEmpty++;
                //            dataList.Add(dataArray[i]);
                //        }
                //    }
                //}

                if (dataNum == 0)
                {
                    if (dataList.Count == 26)
                    {
                        hasAllCols = true;
                    }
                    else
                    {
                        hasAllCols = false;
                    }
                }

                dataNum++;
                if (dataNum == 1)
                {
                    varNum = dataList.Count;
                }
                disDataList.Add(dataList);
                dataList = new List<string>();

            }
            while (aLine != null);

            sr.Close();

            DataList = disDataList;            
        }

        /// <summary>
        /// Generate data info text of MICAPS 1
        /// </summary>       
        /// <returns>info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + FileName;
            dataInfo += Environment.NewLine + "Description: " + Description;
            dataInfo += Environment.NewLine + "Time: " + DateTime.ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "Station Number: " + stNum;

            return dataInfo;
        }

        /// <summary>
        /// Get discrete data from MICAPS 1 data info
        /// </summary>        
        /// <param name="vIdx">variable index</param>
        /// <param name="dataExtent">ref data extent</param>
        /// <returns>discreted data</returns>
        public double[,] GetDiscreteData(int vIdx, ref Extent dataExtent)
        {
            vIdx += 3;
            //if (vIdx >= 22)
            //{
            //    vIdx += 2;
            //}

            string aStid;
            int i;
            Single lon, lat, t;
            List<string> dataList = new List<string>();
            double[,] DiscreteData = new double[3, DataList.Count];
            Single minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;

            for (i = 0; i < DataList.Count; i++)
            {
                dataList = DataList[i];
                aStid = (string)dataList[0];
                lon = Convert.ToSingle(dataList[1]);
                lat = Convert.ToSingle(dataList[2]);
                t = Convert.ToSingle(dataList[vIdx]);
                //if (lon < 0)
                //{
                //    lon += 360;
                //}
                DiscreteData[0, i] = lon;
                DiscreteData[1, i] = lat;
                DiscreteData[2, i] = t;

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
            dataExtent.minX = minX;
            dataExtent.maxX = maxX;
            dataExtent.minY = minY;
            dataExtent.maxY = maxY;

            return DiscreteData;
        }

        /// <summary>
        /// Get discrete data from MICAPS 1 data info
        /// </summary>        
        /// <param name="vIdx">variable index</param>
        /// <param name="stations">ref stations</param>
        /// <param name="dataExtent">ref data extent</param>
        /// <returns>discreted data</returns>
        public double[,] GetDiscreteData(int vIdx, ref List<string> stations, ref Extent dataExtent)
        {
            vIdx += 3;
            //if (vIdx >= 22)
            //{
            //    vIdx += 2;
            //}

            string aStid;
            int i;
            Single lon, lat, t;
            List<string> dataList = new List<string>();
            double[,] DiscreteData = new double[3, DataList.Count];
            Single minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            stations.Clear();

            for (i = 0; i < DataList.Count; i++)
            {
                dataList = DataList[i];
                aStid = (string)dataList[0];
                lon = Convert.ToSingle(dataList[1]);
                lat = Convert.ToSingle(dataList[2]);
                t = Convert.ToSingle(dataList[vIdx]);

                if (vIdx == 8)    //Pressure
                {                    
                    if (!MIMath.DoubleEquals(t, MissingValue))
                    {
                        if (t > 800)
                            t = t / 10 + 900;
                        else
                            t = t / 10 + 1000;
                    }
                }
                //if (lon < 0)
                //{
                //    lon += 360;
                //}
                stations.Add(aStid);
                DiscreteData[0, i] = lon;
                DiscreteData[1, i] = lat;
                DiscreteData[2, i] = t;

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
            dataExtent.minX = minX;
            dataExtent.maxX = maxX;
            dataExtent.minY = minY;
            dataExtent.maxY = maxY;

            return DiscreteData;
        }

        /// <summary>
        /// Read station data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Station data</returns>
        public StationData GetStationData(int timeIdx, int varIdx, int levelIdx)
        {
            StationData aStData = new StationData();            
            aStData.Data = GetDiscreteData(varIdx, ref aStData.Stations, ref aStData.DataExtent);
            aStData.MissingValue = this.MissingValue;

            return aStData;
        }

        /// <summary>
        /// Read station model data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Station data</returns>
        public StationModelData GetStationModelData(int timeIdx, int levelIdx)
        {
            StationModelData smData = new StationModelData();
            string aStid;
            int i;
            Single lon, lat;
            List<string> dataList = new List<string>();
            List<StationModel> smList = new List<StationModel>();
            double[,] DiscreteData = new double[10, DataList.Count];
            Single minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;

            for (i = 0; i < DataList.Count; i++)
            {
                dataList = DataList[i];
                aStid = (string)dataList[0];
                lon = Convert.ToSingle(dataList[1]);
                lat = Convert.ToSingle(dataList[2]);
                //if (lon < 0)
                //{
                //    lon += 360;
                //}
                StationModel sm = new StationModel();
                sm.StationIdentifer = aStid;
                sm.Longitude = lon;
                sm.Latitude = lat;
                sm.WindDirection = Convert.ToDouble(dataList[6]);    //Wind direction
                sm.WindSpeed = Convert.ToDouble(dataList[7]);    //Wind speed
                sm.Visibility = Convert.ToDouble(dataList[17]);    //Visibility
                sm.Weather = Convert.ToDouble(dataList[18]);    //Weather
                sm.CloudCover = Convert.ToDouble(dataList[5]);    //Cloud cover
                sm.Temperature = Convert.ToDouble(dataList[19]);    //Temperature
                sm.DewPoint = Convert.ToDouble(dataList[16]);    //Dew point
                //Pressure
                double press = Convert.ToDouble(dataList[8]);
                if (MIMath.DoubleEquals(press, this.MissingValue))
                {
                    sm.Pressure = press;
                }
                else
                {
                    if (press > 800)
                    {
                        sm.Pressure = press / 10 + 900;
                    }
                    else
                    {
                        sm.Pressure = press / 10 + 1000;
                    }
                }
                smList.Add(sm);

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

            smData.Data = smList;
            smData.DataExtent = dataExtent;
            smData.MissingValue = this.MissingValue;

            return smData;
        }

        /// <summary>
        /// Read station info data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>StationInfoData</returns>
        public StationInfoData GetStationInfoData(int timeIdx, int levelIdx)
        {
            StationInfoData stInfoData = new StationInfoData();
            stInfoData.DataList = this.DataList;
            stInfoData.Fields = this.FieldList;
            stInfoData.Variables = this.VarList;

            List<string> stations = new List<string>();
            int stNum = this.DataList.Count;
            for (int i = 0; i < stNum; i++)
            {
                stations.Add(this.DataList[i][0]);
            }
            stInfoData.Stations = stations;

            return stInfoData;
        }

        #endregion
    }

}
