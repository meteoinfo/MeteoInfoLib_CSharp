using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// METAR data info
    /// </summary>
    public class METARDataInfo : DataInfo,IStationDataInfo
    {
        #region Variables
        /// <summary>
        /// File name
        /// </summary>
        public string StFileName;        
        /// <summary>
        /// DateTime
        /// </summary>
        public DateTime dateTime;
        /// <summary>
        /// Station number
        /// </summary>
        public int stNum;
        /// <summary>
        /// Variable list
        /// </summary>
        public List<string> varList;
        /// <summary>
        /// Data list
        /// </summary>
        public List<List<string>> DataList;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public METARDataInfo()
        {
            string[] items = new string[] {"WindDirection","WindSpeed","Visibility","Weather",
                "CloudCover","Temperature","DewPoint","Altimeter"};
            varList = new List<string>(items.Length);
            varList.AddRange(items);
            DataList = new List<List<string>>();            
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read METAR data info
        /// </summary>
        /// <param name="dataFN">METAR data file name</param>      
        public override void ReadDataInfo(string dataFN)
        {            
            this.FileName = dataFN;

            //Read stations
            StreamReader sr = new StreamReader(StFileName);
            string aLine;
            string[] dataArray;
            List<string> dataList = new List<string>();
            List<string> stNameList = new List<string>();
            List<string[]> stPosList = new List<string[]>();
            int i, LastNonEmpty, dataNum;
            sr.ReadLine();
            dataNum = 0;
            while (true)
            {
                aLine = sr.ReadLine();
                if (aLine == null)
                {
                    break;
                }
                if (aLine == "")
                {
                    continue;
                }
                dataArray = aLine.Split(',');
                stNameList.Add(dataArray[1]);
                stPosList.Add(new string[2] { dataArray[2], dataArray[3] });
                dataNum += 1;
            }
            sr.Close();

            //Read METAR data
            sr = new StreamReader(dataFN);
            List<List<string>> disDataList = new List<List<string>>();
            int stIdx = 0;
            string stName;
            List<string> stList = new List<string>();
            int stIdx1 = 0;
            aLine = sr.ReadLine();
            if (aLine == "")
            {
                aLine = sr.ReadLine();
            }
            aLine = aLine.Trim();
            dateTime = DateTime.ParseExact(aLine, "yyyy/MM/dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            dateTime = dateTime.AddMinutes(29);
            dateTime = dateTime.AddMinutes(-dateTime.Minute);
            while (true)
            {
                aLine = sr.ReadLine();
                if (aLine == null)
                {
                    break;
                }
                if (aLine == "")
                {
                    continue;
                }
                aLine = aLine.Trim();
                if (aLine.Length == 16)
                {
                    continue;
                }

                dataArray = aLine.Split();
                LastNonEmpty = -1;
                dataList = new List<string>();
                for (i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] != string.Empty)
                    {
                        LastNonEmpty++;
                        dataList.Add(dataArray[i]);
                    }
                }
                stName = dataList[0];
                stIdx = stNameList.IndexOf(stName);
                stIdx1 = stList.IndexOf(stName);
                if (stIdx >= 0 && stIdx1 < 0)
                {
                    stList.Add(stName);
                    dataList.Insert(0, stPosList[stIdx][0]);
                    dataList.Insert(0, stPosList[stIdx][1]);
                    disDataList.Add(dataList);
                }
            }
            sr.Close();

            stNum = disDataList.Count;
            DataList = disDataList;

            Dimension tdim = new Dimension(DimensionType.T);
            tdim.DimValue.Add(DataConvert.ToDouble(dateTime));
            tdim.DimLength = 1;
            List<Variable> vars = new List<Variable>();
            foreach (string vName in varList)
            {
                Variable var = new Variable();
                var.Name = vName;
                var.SetDimension(tdim);
                var.IsStation = true;
                vars.Add(var);
            }
            this.Variables = vars;
        }

        /// <summary>
        /// Generate data info text
        /// </summary>        
        /// <returns>data information text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + this.FileName;
            dataInfo += Environment.NewLine + "Time: " + dateTime.ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "Station Number: " + stNum;

            return dataInfo;
        }

        /// <summary>
        /// Get discrete METAR data
        /// </summary>        
        /// <param name="vIdx">variable index</param>
        /// <param name="dataExtent">ref data extent</param>
        /// <returns>discrete data</returns>
        public double[,] GetDiscreteData(int vIdx, ref Global.Extent dataExtent)
        {
            string aStid;
            int i;
            Single lon, lat;
            double t;
            t = 0;
            string dataStr;
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
                aStid = dataList[2];
                lon = Convert.ToSingle(dataList[0]);
                lat = Convert.ToSingle(dataList[1]);
                //if (lon < 0)
                //{
                //    lon += 360;
                //}
                //Initialize data
                DiscreteData[0, i] = lon;
                DiscreteData[1, i] = lat;
                DiscreteData[2, i] = -9999;

                //Get extent
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

                int nVIdx = vIdx + 4;
                if (dataList[4].ToUpper() == "AUTO")    //Skip AUTO
                {
                    nVIdx += 1;
                }
                if (vIdx >= 1)    //Wind speed is in same string with wind direction
                {
                    nVIdx -= 1;
                }
                if (vIdx >= 2)    //Skip wind direction range
                {
                    if (dataList[nVIdx - (vIdx - 2)].Contains("V"))
                    {
                        nVIdx += 1;
                    }
                    dataStr = dataList[nVIdx - (vIdx - 2)];
                    //If no visibility data
                    if (!MIMath.IsNumeric(dataStr) &&
                        (dataStr.Length < 3 || (dataStr.Length >= 3 &&
                        dataStr.Substring(dataStr.Length - 2, 2) != "SM" &&
                        dataStr.Substring(dataStr.Length - 3, 3) != "NDV")))
                    {
                        if (vIdx == 2)
                        {
                            continue;
                        }
                        nVIdx -= 1;
                    }
                }
                if (vIdx >= 3)    //Skip runway visual range
                {
                    if (dataList.Count <= nVIdx)
                    {
                        continue;
                    }
                    while (true)
                    {
                        if (dataList[nVIdx - (vIdx - 3)].Substring(0, 1) == "R" &&
                            dataList[nVIdx - (vIdx - 3)].Contains("/"))
                        {
                            nVIdx += 1;
                            if (dataList.Count <= nVIdx)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    //If no weather data
                    dataStr = dataList[nVIdx - (vIdx - 3)];
                    if (dataStr.Substring(0, 1) != "+" && dataStr.Substring(0, 1) != "-" &&
                        dataStr.Substring(0, 2) != "VC" && dataStr.Length != 2 &&
                        dataStr.Length != 4)
                    {
                        if (vIdx == 3)
                        {
                            continue;
                        }
                        nVIdx -= 1;
                    }
                }
                if (vIdx >= 4)    //Skip second weather
                {
                    if (dataList[nVIdx - (vIdx - 4)].Length == 2)
                    {
                        nVIdx += 1;
                    }
                }
                if (vIdx >= 5)    //Skip other cloud
                {
                    while (true)
                    {
                        dataStr = dataList[nVIdx - (vIdx - 5)];
                        if ((dataStr.Length == 6 &&
                            MIMath.IsNumeric(dataStr.Substring(dataStr.Length - 3, 3))) ||
                            (dataStr.Length == 9 && dataStr.Substring(dataStr.Length - 3, 3) == "///") ||
                            dataStr.Substring(0, 1) == "/")
                        {
                            nVIdx += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (vIdx >= 6)    //Dew point is in same string with temprature
                {
                    nVIdx -= 1;
                }
                if (nVIdx >= dataList.Count)
                {
                    continue;
                }
                dataStr = dataList[nVIdx];
                switch (vIdx)
                {
                    case 0:    //WindDirection
                        if (dataStr.Length >= 7)
                        {
                            if (MIMath.IsNumeric(dataStr.Substring(0, 3)))
                            {
                                t = double.Parse(dataStr.Substring(0, 3));
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 1:    //WindSpeed                        
                        if (dataStr.Length >= 7)
                        {
                            if (MIMath.IsNumeric(dataStr.Substring(3, 2)))
                            {
                                t = double.Parse(dataStr.Substring(3, 2));
                                if (dataStr.Substring(5, 2).ToUpper() == "KT")
                                {
                                    t = t * 0.51444;    //Convert KT to MPS
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 2:    //Visibility                        
                        if (MIMath.IsNumeric(dataStr))    //Unit: m
                        {
                            t = double.Parse(dataStr);
                        }
                        else
                        {
                            if (dataStr.Length >= 3)
                            {
                                if (dataStr.Substring(dataStr.Length - 2, 2).ToUpper() == "SM")
                                {
                                    dataStr = dataStr.Substring(0, dataStr.Length - 2);
                                    if (dataStr.Contains("/"))
                                    {
                                        if (dataStr.Substring(0, 1).ToUpper() == "M")
                                        {
                                            dataStr = dataStr.Substring(1);
                                        }
                                        t = int.Parse(dataStr.Substring(0, dataStr.IndexOf("/"))) /
                                            int.Parse(dataStr.Substring(dataStr.IndexOf("/") + 1));
                                    }
                                    else
                                    {
                                        t = double.Parse(dataStr);
                                    }
                                    t = t * 1603.9;    //statute miles to meters
                                }
                                else if (dataStr.Substring(dataStr.Length - 3, 3).ToUpper() == "NDV")
                                {
                                    dataStr = dataStr.Substring(0, dataStr.Length - 3);
                                    t = double.Parse(dataStr);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;
                    case 3:    //Weather
                        int wIdx = 0;
                        if (dataStr.Length == 2)
                        {
                            if (GetWeatherIndex(dataStr, "", "", ref wIdx))
                            {
                                t = wIdx;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (dataStr.Length == 3)
                        {
                            if (dataStr.Substring(0, 1) == "+" || dataStr.Substring(0, 1) == "-")
                            {
                                if (GetWeatherIndex(dataStr.Substring(1, 2), dataStr.Substring(0, 1), "", ref wIdx))
                                {
                                    t = wIdx;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (dataStr.Length == 4)
                        {
                            if (GetWeatherIndex(dataStr.Substring(2, 2), "", dataStr.Substring(0, 2), ref wIdx))
                            {
                                t = wIdx;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (dataStr.Length == 5)
                        {
                            if (dataStr.Substring(0, 1) == "+" || dataStr.Substring(0, 1) == "-")
                            {
                                if (GetWeatherIndex(dataStr.Substring(3, 2), dataStr.Substring(0, 1),
                                    dataStr.Substring(1, 2), ref wIdx))
                                {
                                    t = wIdx;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 4:    //Cloud
                        int cCover = 0;
                        if (dataStr.Length >= 2)
                        {
                            if (dataStr.Substring(0, 2) == "VV")
                            {
                                t = 9;
                            }
                            else if (dataStr == "CAVOK")
                            {
                                t = 0;
                            }
                            else
                            {
                                if (dataStr.Length >= 3)
                                {
                                    dataStr = dataStr.Substring(0, 3);
                                    if (GetCloudCover(dataStr, ref cCover))
                                    {
                                        t = cCover;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 5:    //Temeprature
                        if (dataStr.Contains("/"))
                        {
                            dataStr = dataStr.Substring(0, dataStr.IndexOf("/"));
                            if (dataStr.Length == 0)
                            {
                                continue;
                            }
                            if (dataStr.Substring(0, 1) == "M")
                            {
                                dataStr = dataStr.Replace("M", "-");
                            }
                            if (MIMath.IsNumeric(dataStr))
                            {
                                t = double.Parse(dataStr);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 6:    //Dew point
                        if (dataStr.Contains("/"))
                        {
                            dataStr = dataStr.Substring(dataStr.IndexOf("/") + 1);
                            if (dataStr.Length == 0)
                            {
                                continue;
                            }
                            if (dataStr.Substring(0, 1) == "M")
                            {
                                dataStr = dataStr.Replace("M", "-");
                            }
                            if (MIMath.IsNumeric(dataStr))
                            {
                                t = double.Parse(dataStr);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 7:    //Altimeter
                        string altType = dataStr.Substring(0, 1);
                        if (dataStr.Length > 1 && (altType == "A" || altType == "Q"))
                        {
                            dataStr = dataStr.Substring(1);
                            if (MIMath.IsNumeric(dataStr))
                            {
                                t = double.Parse(dataStr);
                                if (altType == "A")
                                {
                                    t = t * 33.863 / 100;
                                }
                                if (t < 10)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                }

                DiscreteData[2, i] = t;
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
            StationData stationData = new StationData();
            List<string> stations = new List<string>();
            string aStid;
            int i;
            Single lon, lat;
            double t;
            t = 0;
            string dataStr;
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
                aStid = dataList[2];
                lon = Convert.ToSingle(dataList[0]);
                lat = Convert.ToSingle(dataList[1]);
                //if (lon < 0)
                //{
                //    lon += 360;
                //}
                //Initialize data
                DiscreteData[0, i] = lon;
                DiscreteData[1, i] = lat;
                DiscreteData[2, i] = -9999;
                stations.Add(aStid);

                //Get extent
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

                int nVIdx = varIdx + 4;
                if (dataList[4].ToUpper() == "AUTO")    //Skip AUTO
                {
                    nVIdx += 1;
                }
                if (varIdx >= 1)    //Wind speed is in same string with wind direction
                {
                    nVIdx -= 1;
                }
                if (varIdx >= 2)    //Skip wind direction range
                {
                    if (dataList[nVIdx - (varIdx - 2)].Contains("V"))
                    {
                        nVIdx += 1;
                    }
                    dataStr = dataList[nVIdx - (varIdx - 2)];
                    //If no visibility data
                    if (!MIMath.IsNumeric(dataStr) &&
                        (dataStr.Length < 3 || (dataStr.Length >= 3 &&
                        dataStr.Substring(dataStr.Length - 2, 2) != "SM" &&
                        dataStr.Substring(dataStr.Length - 3, 3) != "NDV")))
                    {
                        if (varIdx == 2)
                        {
                            continue;
                        }
                        nVIdx -= 1;
                    }
                }
                if (varIdx >= 3)    //Skip runway visual range
                {
                    if (dataList.Count <= nVIdx)
                    {
                        continue;
                    }
                    while (true)
                    {
                        if (dataList[nVIdx - (varIdx - 3)].Substring(0, 1) == "R" &&
                            dataList[nVIdx - (varIdx - 3)].Contains("/"))
                        {
                            nVIdx += 1;
                            if (dataList.Count <= nVIdx)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    //If no weather data
                    dataStr = dataList[nVIdx - (varIdx - 3)];
                    if (dataStr.Substring(0, 1) != "+" && dataStr.Substring(0, 1) != "-" &&
                        dataStr.Substring(0, 2) != "VC" && dataStr.Length != 2 &&
                        dataStr.Length != 4)
                    {
                        if (varIdx == 3)
                        {
                            continue;
                        }
                        nVIdx -= 1;
                    }
                }
                if (varIdx >= 4)    //Skip second weather
                {
                    if (dataList[nVIdx - (varIdx - 4)].Length == 2)
                    {
                        nVIdx += 1;
                    }
                }
                if (varIdx >= 5)    //Skip other cloud
                {
                    while (true)
                    {
                        dataStr = dataList[nVIdx - (varIdx - 5)];
                        if ((dataStr.Length == 6 &&
                            MIMath.IsNumeric(dataStr.Substring(dataStr.Length - 3, 3))) ||
                            (dataStr.Length == 9 && dataStr.Substring(dataStr.Length - 3, 3) == "///") ||
                            dataStr.Substring(0, 1) == "/")
                        {
                            nVIdx += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (varIdx >= 6)    //Dew point is in same string with temprature
                {
                    nVIdx -= 1;
                }
                if (nVIdx >= dataList.Count)
                {
                    continue;
                }
                dataStr = dataList[nVIdx];
                switch (varIdx)
                {
                    case 0:    //WindDirection
                        if (dataStr.Length >= 7)
                        {
                            if (MIMath.IsNumeric(dataStr.Substring(0, 3)))
                            {
                                t = double.Parse(dataStr.Substring(0, 3));
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 1:    //WindSpeed                        
                        if (dataStr.Length >= 7)
                        {
                            if (MIMath.IsNumeric(dataStr.Substring(3, 2)))
                            {
                                t = double.Parse(dataStr.Substring(3, 2));
                                if (dataStr.Substring(5, 2).ToUpper() == "KT")
                                {
                                    t = t * 0.51444;    //Convert KT to MPS
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 2:    //Visibility                        
                        if (MIMath.IsNumeric(dataStr))    //Unit: m
                        {
                            t = double.Parse(dataStr);
                        }
                        else
                        {
                            if (dataStr.Length >= 3)
                            {
                                if (dataStr.Substring(dataStr.Length - 2, 2).ToUpper() == "SM")
                                {
                                    dataStr = dataStr.Substring(0, dataStr.Length - 2);
                                    if (dataStr.Contains("/"))
                                    {
                                        if (dataStr.Substring(0, 1).ToUpper() == "M")
                                        {
                                            dataStr = dataStr.Substring(1);
                                        }
                                        t = int.Parse(dataStr.Substring(0, dataStr.IndexOf("/"))) /
                                            int.Parse(dataStr.Substring(dataStr.IndexOf("/") + 1));
                                    }
                                    else
                                    {
                                        t = double.Parse(dataStr);
                                    }
                                    t = t * 1603.9;    //statute miles to meters
                                }
                                else if (dataStr.Substring(dataStr.Length - 3, 3).ToUpper() == "NDV")
                                {
                                    dataStr = dataStr.Substring(0, dataStr.Length - 3);
                                    t = double.Parse(dataStr);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;
                    case 3:    //Weather
                        int wIdx = 0;
                        if (dataStr.Length == 2)
                        {
                            if (GetWeatherIndex(dataStr, "", "", ref wIdx))
                            {
                                t = wIdx;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (dataStr.Length == 3)
                        {
                            if (dataStr.Substring(0, 1) == "+" || dataStr.Substring(0, 1) == "-")
                            {
                                if (GetWeatherIndex(dataStr.Substring(1, 2), dataStr.Substring(0, 1), "", ref wIdx))
                                {
                                    t = wIdx;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (dataStr.Length == 4)
                        {
                            if (GetWeatherIndex(dataStr.Substring(2, 2), "", dataStr.Substring(0, 2), ref wIdx))
                            {
                                t = wIdx;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (dataStr.Length == 5)
                        {
                            if (dataStr.Substring(0, 1) == "+" || dataStr.Substring(0, 1) == "-")
                            {
                                if (GetWeatherIndex(dataStr.Substring(3, 2), dataStr.Substring(0, 1),
                                    dataStr.Substring(1, 2), ref wIdx))
                                {
                                    t = wIdx;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 4:    //Cloud
                        int cCover = 0;
                        if (dataStr.Length >= 2)
                        {
                            if (dataStr.Substring(0, 2) == "VV")
                            {
                                t = 9;
                            }
                            else if (dataStr == "CAVOK")
                            {
                                t = 0;
                            }
                            else
                            {
                                if (dataStr.Length >= 3)
                                {
                                    dataStr = dataStr.Substring(0, 3);
                                    if (GetCloudCover(dataStr, ref cCover))
                                    {
                                        t = cCover;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 5:    //Temeprature
                        if (dataStr.Contains("/"))
                        {
                            dataStr = dataStr.Substring(0, dataStr.IndexOf("/"));
                            if (dataStr.Length == 0)
                            {
                                continue;
                            }
                            if (dataStr.Substring(0, 1) == "M")
                            {
                                dataStr = dataStr.Replace("M", "-");
                            }
                            if (MIMath.IsNumeric(dataStr))
                            {
                                t = double.Parse(dataStr);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 6:    //Dew point
                        if (dataStr.Contains("/"))
                        {
                            dataStr = dataStr.Substring(dataStr.IndexOf("/") + 1);
                            if (dataStr.Length == 0)
                            {
                                continue;
                            }
                            if (dataStr.Substring(0, 1) == "M")
                            {
                                dataStr = dataStr.Replace("M", "-");
                            }
                            if (MIMath.IsNumeric(dataStr))
                            {
                                t = double.Parse(dataStr);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 7:    //Altimeter
                        string altType = dataStr.Substring(0, 1);
                        if (dataStr.Length > 1 && (altType == "A" || altType == "Q"))
                        {
                            dataStr = dataStr.Substring(1);
                            if (MIMath.IsNumeric(dataStr))
                            {
                                t = double.Parse(dataStr);
                                if (altType == "A")
                                {
                                    t = t * 33.863 / 100;
                                }
                                if (t < 10)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        break;
                }

                DiscreteData[2, i] = t;
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

        private bool GetWeatherIndex(string wStr, string intensity, string descriptor, ref int wIdx)
        {
            bool isTrue = true;
            switch (wStr.ToUpper())
            {
                //Precipitation
                case "DZ":    //Drizzle
                    if (intensity == "+")
                    {
                        switch (descriptor)
                        {
                            case "FZ":    //Freezing
                                wIdx = 57;
                                break;
                            default:
                                wIdx = 55;
                                break;
                        }
                    }
                    else if (intensity == "-")
                    {
                        switch (descriptor)
                        {
                            case "FZ":    //Freezing
                                wIdx = 56;
                                break;
                            default:
                                wIdx = 51;
                                break;
                        }
                    }
                    else
                    {
                        switch (descriptor)
                        {
                            case "FZ":    //Freezing
                                wIdx = 57;
                                break;
                            default:
                                wIdx = 53;
                                break;
                        }
                    }
                    break;
                case "RA":    //Rain
                    if (intensity == "+")
                    {
                        switch (descriptor)
                        {
                            case "FZ":    //Freezing
                                wIdx = 67;
                                break;
                            case "TS":    //Thunderstorm
                                wIdx = 92;
                                break;
                            case "SH":    //Shower
                                wIdx = 81;
                                break;
                            default:
                                wIdx = 65;
                                break;
                        }
                    }
                    else if (intensity == "-")
                    {
                        switch (descriptor)
                        {
                            case "FZ":    //Freezing
                                wIdx = 66;
                                break;
                            case "TS":    //Thunderstorm
                                wIdx = 91;
                                break;
                            case "SH":    //Shower
                                wIdx = 80;
                                break;
                            default:
                                wIdx = 61;
                                break;
                        }
                    }
                    else
                    {
                        switch (descriptor)
                        {
                            case "FZ":    //Freezing
                                wIdx = 67;
                                break;
                            case "TS":    //Thunderstorm
                                wIdx = 92;
                                break;
                            case "SH":    //Shower
                                wIdx = 81;
                                break;
                            default:
                                wIdx = 63;
                                break;
                        }
                    }
                    break;
                case "SN":    //Snow
                    if (intensity == "+")
                    {
                        switch (descriptor)
                        {
                            case "TS":    //Thunderstorm
                                wIdx = 94;
                                break;
                            case "SH":    //Shower
                                wIdx = 86;
                                break;
                            default:
                                wIdx = 75;
                                break;
                        }
                    }
                    else if (intensity == "-")
                    {
                        switch (descriptor)
                        {
                            case "TS":    //Thunderstorm
                                wIdx = 93;
                                break;
                            case "SH":    //Shower
                                wIdx = 85;
                                break;
                            default:
                                wIdx = 71;
                                break;
                        }
                    }
                    else
                    {
                        switch (descriptor)
                        {
                            case "TS":    //Thunderstorm
                                wIdx = 94;
                                break;
                            case "SH":    //Shower
                                wIdx = 86;
                                break;
                            default:
                                wIdx = 73;
                                break;
                        }
                    }
                    break;
                case "SG":    //Snow grains
                    wIdx = 77;
                    break;
                case "IC":    //Ice crystals
                    wIdx = 76;
                    break;
                case "PE":    //Ice pellets
                    switch (descriptor)
                    {
                        case "SH":    //Shower
                            if (intensity == "-")
                            {
                                wIdx = 87;
                            }
                            else
                            {
                                wIdx = 88;
                            }
                            break;
                        default:
                            wIdx = 79;
                            break;
                    }
                    break;
                case "GR":    //Hail
                    switch (descriptor)
                    {
                        case "TS":    //Thunderstorm
                            if (intensity == "+")
                            {
                                wIdx = 99;
                            }
                            else
                            {
                                wIdx = 96;
                            }
                            break;
                        default:
                            if (intensity == "-")
                            {
                                wIdx = 89;
                            }
                            else
                            {
                                wIdx = 90;
                            }
                            break;
                    }
                    break;
                case "GS":    //Small hail / snow pellets
                    wIdx = 89;
                    break;
                //case "UP":    //Unknow
                //wIdx = 89;
                //break;

                //Obscuration
                case "BR":    //Mist
                    wIdx = 10;
                    break;
                case "FG":    //Fog
                    wIdx = 45;
                    break;
                case "FU":    //Smoke
                    wIdx = 4;
                    break;
                //case "VA":    //Volcanic ash
                //    wIdx = 0;
                //    break;
                case "DU":    //Dust
                    wIdx = 6;
                    break;
                case "SA":    //Sand
                    wIdx = 7;
                    break;
                case "HZ":    //Haze
                    wIdx = 5;
                    break;
                //case "PY":    //Spray
                //    wIdx = 0;
                //    break;

                //Misc
                case "PO":    //Dust whirls
                    wIdx = 8;
                    break;
                case "SQ":    //Squalls
                    wIdx = 39;
                    break;
                case "FC":    //funnel cloud/tornado/waterspout
                    wIdx = 19;
                    break;
                case "SS":    //Dust storm
                    if (intensity == "+")
                    {
                        wIdx = 34;
                    }
                    else if (intensity == "-")
                    {
                        wIdx = 31;
                    }
                    else
                    {
                        wIdx = 31;
                    }
                    break;

                //Not weather
                default:
                    isTrue = false;
                    break;
            }

            return isTrue;
        }

        private bool GetCloudCover(string cStr, ref int cCover)
        {
            bool isTure = true;
            switch (cStr)
            {
                case "CLR":
                case "SKC":
                    cCover = 0;
                    break;
                case "FEW":
                    cCover = 1;
                    break;
                case "SCT":
                    cCover = 3;
                    break;
                case "BKN":
                    cCover = 6;
                    break;
                case "OVC":
                    cCover = 8;
                    break;
                default:
                    isTure = false;
                    break;
            }

            return isTure;
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
            double t;
            t = 0;
            string dataStr;
            List<string> dataList = new List<string>();
            List<StationModel> smList = new List<StationModel>();
            //double[,] DiscreteData = new double[10, DataList.Count];
            Single minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;

            for (i = 0; i < DataList.Count; i++)
            {
                dataList = DataList[i];
                aStid = dataList[2];
                lon = Convert.ToSingle(dataList[0]);
                lat = Convert.ToSingle(dataList[1]);
                if (lon < 0)
                {
                    lon += 360;
                }

                StationModel sm = new StationModel();

                //Initialize data
                sm.Longitude = lon;
                sm.Latitude = lat;

                //Get extent
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

                //WindDirection
                int nVIdx = 4;    //Wind group
                if (dataList[4].ToUpper() == "AUTO")    //Skip AUTO
                {
                    nVIdx += 1;
                }
                if (nVIdx >= dataList.Count)
                {
                    continue;
                }
                dataStr = dataList[nVIdx];
                if (dataStr.Length >= 7)
                {
                    if (MIMath.IsNumeric(dataStr.Substring(0, 3)))
                    {
                        t = double.Parse(dataStr.Substring(0, 3));
                        sm.WindDirection = t;
                    }
                }

                //WindSpeed                        
                if (dataStr.Length >= 7)
                {
                    if (MIMath.IsNumeric(dataStr.Substring(3, 2)))
                    {
                        t = double.Parse(dataStr.Substring(3, 2));
                        if (dataStr.Substring(5, 2).ToUpper() == "KT")
                        {
                            t = t * 0.51444;    //Convert KT to MPS
                        }
                        sm.WindSpeed = t;
                    }
                }

                //Visibility         
                nVIdx += 1;
                if (nVIdx >= dataList.Count)
                {
                    continue;
                }
                dataStr = dataList[nVIdx];
                //Skip wind direction range
                if (dataStr.Contains("V"))
                {
                    nVIdx += 1;
                }
                if (nVIdx >= dataList.Count)
                {
                    continue;
                }
                dataStr = dataList[nVIdx];
                if (MIMath.IsNumeric(dataStr))    //Unit: m
                {
                    t = double.Parse(dataStr);
                    sm.Visibility = t;
                }
                else
                {
                    if (dataStr.Length >= 3)
                    {
                        if (dataStr.Substring(dataStr.Length - 2, 2).ToUpper() == "SM")
                        {
                            dataStr = dataStr.Substring(0, dataStr.Length - 2);
                            if (dataStr.Contains("/"))
                            {
                                if (dataStr.Substring(0, 1).ToUpper() == "M")
                                {
                                    dataStr = dataStr.Substring(1);
                                }
                                t = int.Parse(dataStr.Substring(0, dataStr.IndexOf("/"))) /
                                    int.Parse(dataStr.Substring(dataStr.IndexOf("/") + 1));
                            }
                            else
                            {
                                t = double.Parse(dataStr);
                            }
                            t = t * 1603.9;    //statute miles to meters
                            sm.Visibility = t;
                        }
                        else if (dataStr.Substring(dataStr.Length - 3, 3).ToUpper() == "NDV")
                        {
                            dataStr = dataStr.Substring(0, dataStr.Length - 3);
                            t = double.Parse(dataStr);
                            sm.Visibility = t;
                        }
                        else
                        {
                            nVIdx -= 1;    //No visibility data
                        }
                    }
                    else
                    {
                        nVIdx -= 1;    //No visibility data
                    }
                }

                //Weather
                nVIdx += 1;
                if (nVIdx >= dataList.Count)
                {
                    continue;
                }
                dataStr = dataList[nVIdx];
                //Skip runway visual range                
                while (true)
                {
                    if (dataStr.Substring(0, 1) == "R" &&
                        dataStr.Contains("/"))
                    {
                        nVIdx += 1;
                        if (nVIdx >= dataList.Count)
                        {
                            continue;
                        }
                        dataStr = dataList[nVIdx];
                    }
                    else
                    {
                        break;
                    }
                }
                dataStr = dataList[nVIdx];
                int wIdx = 0;
                if (dataStr.Length == 2)
                {
                    if (GetWeatherIndex(dataStr, "", "", ref wIdx))
                    {
                        t = wIdx;
                        sm.Weather = t;
                    }
                    else
                    {
                        nVIdx -= 1;    //No weather data
                    }
                }
                else if (dataStr.Length == 3)
                {
                    if (dataStr.Substring(0, 1) == "+" || dataStr.Substring(0, 1) == "-")
                    {
                        if (GetWeatherIndex(dataStr.Substring(1, 2), dataStr.Substring(0, 1), "", ref wIdx))
                        {
                            t = wIdx;
                            sm.Weather = t;
                        }
                        else
                        {
                            nVIdx -= 1;    //No weather data
                        }
                    }
                    else
                    {
                        nVIdx -= 1;    //No weather data
                    }
                }
                else if (dataStr.Length == 4)
                {
                    if (GetWeatherIndex(dataStr.Substring(2, 2), "", dataStr.Substring(0, 2), ref wIdx))
                    {
                        t = wIdx;
                        sm.Weather = t;
                    }
                    else
                    {
                        nVIdx -= 1;    //No weather data
                    }
                }
                else if (dataStr.Length == 5)
                {
                    if (dataStr.Substring(0, 1) == "+" || dataStr.Substring(0, 1) == "-")
                    {
                        if (GetWeatherIndex(dataStr.Substring(3, 2), dataStr.Substring(0, 1),
                            dataStr.Substring(1, 2), ref wIdx))
                        {
                            t = wIdx;
                            sm.Weather = t;
                        }
                        else
                        {
                            nVIdx -= 1;    //No weather data
                        }
                    }
                    else
                    {
                        nVIdx -= 1;    //No weather data
                    }
                }
                else
                {
                    nVIdx -= 1;    //No weather data
                }

                //Cloud
                nVIdx += 1;
                if (nVIdx >= dataList.Count)
                {
                    continue;
                }
                dataStr = dataList[nVIdx];
                //Skip second weather
                if (dataStr.Length == 2)
                {
                    nVIdx += 1;
                }
                if (nVIdx >= dataList.Count)
                {
                    continue;
                }
                dataStr = dataList[nVIdx];
                int cCover = 0;
                if (dataStr.Length >= 2)
                {
                    if (dataStr.Substring(0, 2) == "VV")
                    {
                        t = 9;
                        sm.CloudCover = t;
                    }
                    else if (dataStr == "CAVOK")
                    {
                        t = 0;
                        sm.CloudCover = t;
                    }
                    else
                    {
                        if (dataStr.Length >= 3)
                        {
                            dataStr = dataStr.Substring(0, 3);
                            if (GetCloudCover(dataStr, ref cCover))
                            {
                                t = cCover;
                                sm.CloudCover = t;
                            }
                        }
                    }
                }

                //Temperature
                nVIdx += 1;
                if (nVIdx >= dataList.Count)
                {
                    continue;
                }
                dataStr = dataList[nVIdx];
                //Skip other cloud
                while (true)
                {
                    if ((dataStr.Length == 6 &&
                        MIMath.IsNumeric(dataStr.Substring(dataStr.Length - 3, 3))) ||
                        (dataStr.Length == 9 && dataStr.Substring(dataStr.Length - 3, 3) == "///") ||
                        dataStr.Substring(0, 1) == "/")
                    {
                        nVIdx += 1;
                        if (nVIdx >= dataList.Count)
                        {
                            continue;
                        }
                        dataStr = dataList[nVIdx];
                    }
                    else
                    {
                        break;
                    }
                }
                dataStr = dataList[nVIdx];
                if (dataStr.Contains("/"))
                {
                    dataStr = dataStr.Substring(0, dataStr.IndexOf("/"));
                    if (dataStr.Length > 0)
                    {
                        if (dataStr.Substring(0, 1) == "M")
                        {
                            dataStr = dataStr.Replace("M", "-");
                        }
                        if (MIMath.IsNumeric(dataStr))
                        {
                            t = double.Parse(dataStr);
                            sm.Temperature = t;
                        }
                    }
                }

                //Dew point
                dataStr = dataList[nVIdx];
                if (dataStr.Contains("/"))
                {
                    dataStr = dataStr.Substring(dataStr.IndexOf("/") + 1);
                    if (dataStr.Length > 0)
                    {
                        if (dataStr.Substring(0, 1) == "M")
                        {
                            dataStr = dataStr.Replace("M", "-");
                        }
                        if (MIMath.IsNumeric(dataStr))
                        {
                            t = double.Parse(dataStr);
                            sm.DewPoint = t;
                        }
                    }
                }

                //Altimeter
                nVIdx += 1;
                if (nVIdx >= dataList.Count)
                {
                    continue;
                }
                dataStr = dataList[nVIdx];
                string altType = dataStr.Substring(0, 1);
                if (dataStr.Length > 1 && (altType == "A" || altType == "Q"))
                {
                    dataStr = dataStr.Substring(1);
                    if (MIMath.IsNumeric(dataStr))
                    {
                        t = double.Parse(dataStr);
                        if (altType == "A")
                        {
                            t = t * 33.863 / 100;
                        }
                        if (t > 10)
                        {
                            sm.Pressure = t;
                        }
                    }
                }

                smList.Add(sm);
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
            return null;
        }

        #endregion
    }
}
