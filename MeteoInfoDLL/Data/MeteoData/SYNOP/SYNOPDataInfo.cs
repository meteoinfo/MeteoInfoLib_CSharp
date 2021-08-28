using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// SYNOP data info
    /// </summary>
    public class SYNOPDataInfo : DataInfo,IStationDataInfo
    {
        #region Variables
        /// <summary>
        /// File name
        /// </summary>
        public string StFileName;        
        /// <summary>
        /// DateTime
        /// </summary>
        public DateTime DateTime;
        /// <summary>
        /// Station number
        /// </summary>
        public int StationNum;   
        /// <summary>
        /// Variable list
        /// </summary>
        public List<string> VarList;
        /// <summary>
        /// Data list
        /// </summary>
        public List<List<string>> DataList;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public SYNOPDataInfo()
        {
            string[] items = new string[] {"Visibility","CloudCover","WindDirection","WindSpeed","Temperature","DewPoint",
                "Pressure","Precipitation","Weather"};
            VarList = new List<string>(items.Length);
            VarList.AddRange(items);
            DataList = new List<List<string>>();            
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read SYNOP data info
        /// </summary>
        /// <param name="dataFN">METAR data file name</param>       
        public override void ReadDataInfo(string dataFN)
        {            
            FileName = dataFN;

            //Read stations
            StreamReader sr = new StreamReader(StFileName);
            string aLine;
            string[] dataArray;
            List<string> dataList = new List<string>();
            List<string> stIDList = new List<string>();
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
                stIDList.Add(dataArray[1]);
                stPosList.Add(new string[2] { dataArray[5], dataArray[4] });
                dataNum += 1;
            }
            sr.Close();

            //Read data
            sr = new StreamReader(dataFN);
            List<List<string>> disDataList = new List<List<string>>();
            List<string> stList = new List<string>();
            string reportType = "AAXX", str, stID;
            DateTime toDay = DateTime.Now;
            DateTime aTime;
            string windSpeedIndicator = "/";
            int stIdx;
            bool isSetTime = true;
            while (true)
            {
                aLine = sr.ReadLine();
                if (aLine == null)
                    break;

                aLine = aLine.Trim();
                if (aLine == string.Empty)
                    continue;

                if (aLine.Length == 3 && MIMath.IsNumeric(aLine))    //Skip group number
                {
                    sr.ReadLine();    //Skip 090000 line                
                    continue;
                }

                //if (aLine.Substring(0, 2) == "SI" || aLine.Substring(0,2) == "SN")    //Skip "SI????" line
                //    continue;

                if (aLine.Length < 4)
                    continue;

                switch(aLine.Substring(0,4))
                {
                    case "AAXX":    //A SYNOP report from a fixed land station is identified by the symbolic letters MiMiMjMj = AAXX
                        reportType = "AAXX";
                        str = aLine.Substring(aLine.Length - 5, 5);
                        if (isSetTime)
                        {
                            aTime = new DateTime(toDay.Year, toDay.Month, int.Parse(str.Substring(0, 2)), int.Parse(str.Substring(2, 2)), 0, 0);
                            DateTime = aTime;
                            isSetTime = false;
                        }
                        windSpeedIndicator = str.Substring(str.Length - 1, 1);
                        break;
                    case "BBXX":    //A SHIP report from a sea station is identified by the symbolic letters MiMiMjMj = BBXX
                        reportType = "BBXX";
                        break;
                    case "OOXX":    //A SYNOP MOBIL report from a mobile land station is identified by the symbolic letters MiMiMjMj = OOXX
                        reportType = "OOXX";
                        break;
                    default:    //Data line
                        while (aLine.Substring(aLine.Length - 1, 1) != "=")
                        {
                            str = sr.ReadLine();
                            if (str == null)
                                break;
                            aLine = aLine + " " + sr.ReadLine();
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

                        stID = dataList[0];
                        switch (reportType)
                        {
                            case "AAXX":
                                if (dataList.Count > 2)
                                {
                                    stIdx = stIDList.IndexOf(stID);
                                    if (stIdx >= 0)
                                    {
                                        dataList.Insert(0, windSpeedIndicator);
                                        dataList.Insert(0, reportType);
                                        dataList.Insert(0, stPosList[stIdx][1]);
                                        dataList.Insert(0, stPosList[stIdx][0]);
                                        disDataList.Add(dataList);
                                    }
                                }
                                break;
                            case "BBXX":
                            case "OOXX":
                                if (dataList.Count > 5)
                                {
                                    if (dataList[2].Contains("/") || dataList[3].Contains("/"))
                                        continue;

                                    if (dataList[2].Substring(0, 2) != "99")
                                        continue;

                                    str = dataList[1];
                                    windSpeedIndicator = str.Substring(str.Length - 1, 1);
                                                                       
                                    float lat = float.Parse(dataList[2].Substring(2)) / 10;
                                    float lon = float.Parse(dataList[3].Substring(1)) / 10;
                                    if (lat > 90 || lon > 180)
                                        continue;

                                    switch (dataList[3].Substring(0, 1))
                                    {
                                        case "1":    //North east

                                            break;
                                        case "3":    //South east
                                            lat = -lat;
                                            break;
                                        case "5":    //South west
                                            lat = -lat;
                                            lon = -lon;
                                            break;
                                        case "7":    //North west
                                            lon = -lon;
                                            break;
                                    }

                                    dataList.Insert(0, windSpeedIndicator);
                                    dataList.Insert(0, reportType);
                                    dataList.Insert(0, lat.ToString());
                                    dataList.Insert(0, lon.ToString());
                                    disDataList.Add(dataList);
                                }
                                break;
                        }
                        break;

                }
            }
            sr.Close();

            StationNum = disDataList.Count;
            DataList = disDataList;

            Dimension tdim = new Dimension(DimensionType.T);
            tdim.DimValue.Add(DataConvert.ToDouble(DateTime));
            tdim.DimLength = 1;
            List<Variable> vars = new List<Variable>();
            foreach (string vName in VarList)
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
            dataInfo = "File Name: " + FileName;
            dataInfo += Environment.NewLine + "Time: " + DateTime.ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "Station Number: " + StationNum;

            return dataInfo;
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
            float lon, lat;
            List<string> dataList = new List<string>();
            //double[,] DiscreteData = new double[3, DataList.Count];
            List<double[]> discreteData = new List<double[]>();
            float minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            string windSpeedIndicator = "/";
            string reportType = "AAXX";
            //string precIndicator;

            for (i = 0; i < DataList.Count; i++)
            {
                dataList = DataList[i];
                reportType = dataList[2];
                windSpeedIndicator = dataList[3];
                aStid = dataList[4];
                lon = float.Parse(dataList[0]);
                lat = float.Parse(dataList[1]); 
                int sIdx = 5;
                switch (reportType)
                {
                    case "BBXX":
                    case "OOXX":
                        sIdx = 8;
                        break;
                }

                double[] disData = new double[3];
                disData[0] = lon;
                disData[1] = lat;
                disData[2] = GetDataValue(dataList, varIdx, sIdx, windSpeedIndicator);
                stations.Add(aStid);
                discreteData.Add(disData);

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
            }
            Extent dataExtent = new Extent();
            dataExtent.minX = minX;
            dataExtent.maxX = maxX;
            dataExtent.minY = minY;
            dataExtent.maxY = maxY;

            stationData.Data = new double[3, discreteData.Count];
            for (i = 0; i < discreteData.Count; i++)
            {
                stationData.Data[0, i] = discreteData[i][0];
                stationData.Data[1, i] = discreteData[i][1];
                stationData.Data[2, i] = discreteData[i][2];
            }
            stationData.DataExtent = dataExtent;
            stationData.Stations = stations;

            return stationData;
        }

        private double GetDataValue(List<string> dataList, int vIdx, int sIdx, string windSpeedIndicator)
        {
            double value = MissingValue;
            string str;
            int i;
            switch (vIdx)
            {
                case 0:    //Visibility
                    str = dataList[sIdx];
                    if (str.Length != 5)
                        break;

                    str = str.Substring(3);
                    if (str.Contains("/"))
                        break;

                    value = int.Parse(str);
                    if (value <= 50)
                        value = value / 10;
                    else if (value >= 56 && value <= 80)
                        value = value - 50;
                    else if (value >= 81 && value <= 89)
                        value = 30 + (value - 80) * 5;
                    else if (value >= 90 && value <= 99)
                    {
                        switch ((int)value)
                        {
                            case 90:
                                value = 0.04;
                                break;
                            case 91:
                                value = 0.05;
                                break;
                            case 92:
                                value = 0.2;
                                break;
                            case 93:
                                value = 0.5;
                                break;
                            case 94:
                                value = 1;
                                break;
                            case 95:
                                value = 2;
                                break;
                            case 96:
                                value = 4;
                                break;
                            case 97:
                                value = 10;
                                break;
                            case 98:
                                value = 20;
                                break;
                            case 99:
                                value = 50;
                                break;
                        }
                    }
                    break;
                case 1:   //Cloud cover
                    str = dataList[sIdx + 1];
                    if (str.Length != 5)
                        break;

                    str = str.Substring(0, 1);
                    if (str == "/")
                        break;

                    value = int.Parse(str);
                    break;
                case 2:    //Wind direction
                    str = dataList[sIdx + 1];
                    if (str.Length != 5)
                        break;

                    str = str.Substring(1, 2);
                    if (str == "//")
                        break;

                    value = int.Parse(str) * 10;
                    if (value > 360)
                        value = 0;
                    break;
                case 3:    //Wind speed
                    if (windSpeedIndicator == "/")
                        break;

                    str = dataList[sIdx + 1];
                    if (str.Length != 5)
                        break;

                    str = str.Substring(3);
                    if (str.Contains("/"))
                        break;

                    if (str == "99")
                    {
                        str = dataList[sIdx + 2].Substring(2);
                        if (str.Contains("/"))
                            break ;

                        value = int.Parse(str);
                    }
                    else
                        value = int.Parse(str);

                    if (windSpeedIndicator == "3" || windSpeedIndicator == "4")
                        value = value * 0.51444;    //Convert KT to MPS
                    break;
                case 4:    //Temperature
                    str = string.Empty;
                    for (i = sIdx + 2; i < dataList.Count; i++)
                    {
                        if (dataList[i].Length == 5 && dataList[i].Substring(0, 1) == "1")
                        {
                            str = dataList[i];
                            break;
                        }
                    }
                    if (str != string.Empty)
                    {
                        if (str.Contains("/"))
                            break;

                        string sign = str.Substring(1, 1);
                        value = double.Parse(str.Substring(2)) / 10;
                        if (sign == "1")
                            value = -value;
                    }
                    break;
                case 5:    //Dew point
                    str = string.Empty;
                    for (i = sIdx + 2; i < dataList.Count; i++)
                    {
                        if (dataList[i].Length == 5 && dataList[i].Substring(0, 1) == "2")
                        {
                            str = dataList[i];
                            break;
                        }
                    }
                    if (str != string.Empty)
                    {
                        if (str.Contains("/"))
                            break;

                        string sign = str.Substring(1, 1);
                        if (sign == "9")    //Relative humidity
                            break;

                        value = double.Parse(str.Substring(2)) / 10;
                        if (sign == "1")
                            value = -value;
                    }
                    break;
                case 6:    //Pressure
                    str = string.Empty;
                    for (i = sIdx + 2; i < dataList.Count; i++)
                    {
                        if (dataList[i].Length == 5 && dataList[i].Substring(0, 1) == "3")
                        {
                            str = dataList[i];
                            break;
                        }
                    }
                    if (str != string.Empty)
                    {
                        if (str.Contains("/"))
                            break;

                        if (!MIMath.IsNumeric(str.Substring(1)))
                            break;

                        value = double.Parse(str.Substring(1)) / 10;
                        value = value / 10;
                        if (value < 500)
                            value += 1000;
                    }
                    break;
                case 7:    //Precipitation
                    str = string.Empty;
                    for (i = sIdx + 2; i < dataList.Count; i++)
                    {
                        if (dataList[i].Length == 5 && dataList[i].Substring(0, 1) == "6")
                        {
                            str = dataList[i];
                            break;
                        }
                    }
                    if (str != string.Empty)
                    {
                        if (str.Contains("/"))
                            break;

                        value = double.Parse(str.Substring(1, 3));
                        if (value >= 990)
                            value = value - 990;
                    }
                    break;
                case 8:    //Weather
                    str = string.Empty;
                    for (i = sIdx + 2; i < dataList.Count; i++)
                    {
                        if (dataList[i].Length == 5 && dataList[i].Substring(0, 1) == "7")
                        {
                            str = dataList[i];
                            break;
                        }
                    }
                    if (str != string.Empty)
                    {
                        if (str.Substring(1, 2).Contains("/"))
                            break;

                        value = int.Parse(str.Substring(1, 2));
                    }
                    break;
            }

            return value;
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
            float lon, lat;
            List<string> dataList = new List<string>();
            List<StationModel> smList = new List<StationModel>();
            double[,] DiscreteData = new double[10, DataList.Count];
            float minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            string windSpeedIndicator = "/";
            string reportType = "AAXX";

            for (i = 0; i < DataList.Count; i++)
            {
                dataList = DataList[i];
                reportType = dataList[2];
                windSpeedIndicator = dataList[3];
                aStid = dataList[4];
                lon = float.Parse(dataList[0]);
                lat = float.Parse(dataList[1]);
                int sIdx = 5;
                switch (reportType)
                {
                    case "BBXX":
                    case "OOXX":
                        sIdx = 8;
                        break;
                }
                //Initialize data
                StationModel sm = new StationModel();
                sm.Longitude = lon;
                sm.Latitude = lat;
                sm.WindDirection = GetDataValue(dataList, 2, sIdx, windSpeedIndicator);    //Wind direction
                sm.WindSpeed = GetDataValue(dataList, 3, sIdx, windSpeedIndicator);    //Wind speed
                sm.Visibility = GetDataValue(dataList, 0, sIdx, windSpeedIndicator);    //Visibility
                sm.Weather = GetDataValue(dataList, 8, sIdx, windSpeedIndicator);    //Weather
                sm.CloudCover = GetDataValue(dataList, 1, sIdx, windSpeedIndicator);    //Cloud cover
                sm.Temperature = GetDataValue(dataList, 4, sIdx, windSpeedIndicator);    //Temperature
                sm.DewPoint = GetDataValue(dataList, 5, sIdx, windSpeedIndicator);    //Dew point 

                smList.Add(sm);

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
