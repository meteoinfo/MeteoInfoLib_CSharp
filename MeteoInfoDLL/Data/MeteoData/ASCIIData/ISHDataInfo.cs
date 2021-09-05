using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// ISH NOAA data info
    /// </summary>
    public class ISHDataInfo : DataInfo,IStationDataInfo
    {
        #region Variables
              
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
        public List<string> dataList;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ISHDataInfo()
        {            
            string[] items = new string[] {"WindDirection","WindSpeed","Visibility","Weather",
                "CloudCover","Temperature","DewPoint","Altimeter"};
            varList = new List<string>(items.Length);
            varList.AddRange(items);
            dataList = new List<string>();            
            MissingValue = -9999;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read ISH data info
        /// </summary>
        /// <param name="dataFN">ISH data file name</param>        
        /// <returns>void</returns>
        public override void ReadDataInfo(string dataFN)
        {            
            FileName = dataFN;
            
            //Read ISH data
            StreamReader sr = new StreamReader(dataFN);                                              
            string aLine = sr.ReadLine();
            string tStr = aLine.Substring(15, 10);          
            dateTime = DateTime.ParseExact(aLine.Substring(15, 10), "yyyyMMddHH",
                System.Globalization.CultureInfo.InvariantCulture);
            List<double> tvalues = new List<double>();
            tvalues.Add(DataConvert.ToDouble(dateTime));
            Dimension tDim = new Dimension(DimensionType.T);
            tDim.SetValues(tvalues);
            
            dataList = new List<string>();
            stNum = 0;
            while (aLine != null)
            {                
                if (aLine == "")
                {
                    aLine = sr.ReadLine();
                    continue;
                }
                string ctStr = aLine.Substring(15, 10);
                if (ctStr != tStr)
                    break;

                dataList.Add(aLine);
                stNum += 1;
                aLine = sr.ReadLine();                
            }
            sr.Close();

            List<Variable> variables = new List<Variable>();
            foreach (string vName in varList)
            {
                Variable var = new Variable();
                var.Name = vName;
                var.IsStation = true;
                var.SetDimension(tDim);
                variables.Add(var);
            }
            this.Variables = variables;
        }

        /// <summary>
        /// Generate data info text
        /// </summary>        
        /// <returns>Info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + FileName;
            dataInfo += Environment.NewLine + "Time: " + dateTime.ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "Station Number: " + stNum;

            return dataInfo;
        }

        /// <summary>
        /// Get discreted data
        /// </summary>
        /// <param name="varIdx">Variable index</param>
        /// <param name="stIDList">ref station identifer list</param>
        /// <param name="dataExtent">ref data extent</param>
        /// <returns>discreted data</returns>
        public double[,] GetDiscreteData(int varIdx, ref List<string> stIDList, ref Global.Extent dataExtent)
        {
            string aStid;
            int i;
            double lon, lat, var;
            string aLine;
            List<double[]> disDataList = new List<double[]>();
            double[,] DiscreteData;
            double minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            string lonStr, latStr, varStr;
            stIDList = new List<string>();

            //Get variable position in the record
            int startIdx = 0, vLen = 1, scaleFactor = 1;
            string undefStr = "";
            switch (varIdx)
            {
                case 0:    //Wind direction
                    startIdx = 60;
                    vLen = 3;
                    undefStr = "999";
                    break;
                case 1:    //Wind speed
                    startIdx = 65;
                    vLen = 4;
                    undefStr = "9999";
                    scaleFactor = 10;
                    break;
                case 2:    //visibility
                    startIdx = 78;
                    vLen = 6;
                    undefStr = "999999";
                    break;
                case 5:    //Temperature
                    startIdx = 87;
                    vLen = 5;
                    undefStr = "+9999";
                    scaleFactor = 10;
                    break;
                case 6:    //Dew point temperature
                    startIdx = 93;
                    vLen = 5;
                    undefStr = "+9999";
                    scaleFactor = 10;
                    break;
                case 7:    //Sea level pressure
                    startIdx = 99;
                    vLen = 5;
                    undefStr = "99999";
                    scaleFactor = 10;
                    break;
            }

            //Loop
            for (i = 0; i < dataList.Count; i++)
            {
                aLine = dataList[i];
                aStid = aLine.Substring(4, 6);
                latStr = aLine.Substring(28, 6);
                if (latStr == "+99999")
                    continue;
                lat = double.Parse(latStr) / 1000;
                lonStr = aLine.Substring(34, 7);
                if (lonStr == "+999999")
                    continue;
                lon = double.Parse(lonStr) / 1000;

                var = MissingValue;
                switch (varIdx)
                {
                    case 3:    //Present weather
                        if (aLine.Contains("MW1"))    //Manual report
                        {
                            startIdx = aLine.IndexOf("MW1") + 3;
                            vLen = 2;
                            varStr = aLine.Substring(startIdx, vLen);
                            var = double.Parse(varStr);
                        }
                        else if (aLine.Contains("AW1"))    //Automatic report
                        {
                            startIdx = aLine.IndexOf("AW1") + 3;
                            vLen = 2;
                            varStr = aLine.Substring(startIdx, vLen);
                            try { var = double.Parse(varStr); }
                            catch { break; }
                        }
                        break;
                    case 4:    //Cloud cover
                        if (aLine.Contains("GF1"))   
                        {
                            startIdx = aLine.IndexOf("GF1") + 3;
                            vLen = 2;
                            undefStr = "99";
                            varStr = aLine.Substring(startIdx, vLen);
                            if (varStr != undefStr)
                                var = double.Parse(varStr);
                        }
                        break;
                    default:
                        varStr = aLine.Substring(startIdx, vLen);
                        if (varStr != undefStr)                            
                            var = double.Parse(varStr) / scaleFactor;

                        break;
                }
                
                //DiscreteData[0, i] = lon;
                //DiscreteData[1, i] = lat;
                //DiscreteData[2, i] = var;
                disDataList.Add(new double[] { lon, lat, var });
                stIDList.Add(aStid);

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

            //Return
            DiscreteData = new double[3, disDataList.Count];
            for (i = 0; i < disDataList.Count; i++)
            {
                DiscreteData[0, i] = disDataList[i][0];
                DiscreteData[1, i] = disDataList[i][1];
                DiscreteData[2, i] = disDataList[i][2];
            }
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
            string aStid;
            int i;
            double lon, lat, var;
            string aLine;
            List<double[]> disDataList = new List<double[]>();
            double[,] DiscreteData;
            double minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            string lonStr, latStr, varStr;
            List<string> stIDList = new List<string>();

            //Get variable position in the record
            int startIdx = 0, vLen = 1, scaleFactor = 1;
            string undefStr = "";
            switch (varIdx)
            {
                case 0:    //Wind direction
                    startIdx = 60;
                    vLen = 3;
                    undefStr = "999";
                    break;
                case 1:    //Wind speed
                    startIdx = 65;
                    vLen = 4;
                    undefStr = "9999";
                    scaleFactor = 10;
                    break;
                case 2:    //visibility
                    startIdx = 78;
                    vLen = 6;
                    undefStr = "999999";
                    break;
                case 5:    //Temperature
                    startIdx = 87;
                    vLen = 5;
                    undefStr = "+9999";
                    scaleFactor = 10;
                    break;
                case 6:    //Dew point temperature
                    startIdx = 93;
                    vLen = 5;
                    undefStr = "+9999";
                    scaleFactor = 10;
                    break;
                case 7:    //Sea level pressure
                    startIdx = 99;
                    vLen = 5;
                    undefStr = "99999";
                    scaleFactor = 10;
                    break;
            }

            //Loop
            for (i = 0; i < dataList.Count; i++)
            {
                aLine = dataList[i];
                aStid = aLine.Substring(4, 6);
                latStr = aLine.Substring(28, 6);
                if (latStr == "+99999")
                    continue;
                lat = double.Parse(latStr) / 1000;
                lonStr = aLine.Substring(34, 7);
                if (lonStr == "+999999")
                    continue;
                lon = double.Parse(lonStr) / 1000;

                var = MissingValue;
                switch (varIdx)
                {
                    case 3:    //Present weather
                        if (aLine.Contains("MW1"))    //Manual report
                        {
                            startIdx = aLine.IndexOf("MW1") + 3;
                            vLen = 2;
                            varStr = aLine.Substring(startIdx, vLen);
                            var = double.Parse(varStr);
                        }
                        else if (aLine.Contains("AW1"))    //Automatic report
                        {
                            startIdx = aLine.IndexOf("AW1") + 3;
                            vLen = 2;
                            varStr = aLine.Substring(startIdx, vLen);
                            try { var = double.Parse(varStr); }
                            catch { break; }
                        }
                        break;
                    case 4:    //Cloud cover
                        if (aLine.Contains("GF1"))
                        {
                            startIdx = aLine.IndexOf("GF1") + 3;
                            vLen = 2;
                            undefStr = "99";
                            varStr = aLine.Substring(startIdx, vLen);
                            if (varStr != undefStr)
                                var = double.Parse(varStr);
                        }
                        break;
                    default:
                        varStr = aLine.Substring(startIdx, vLen);
                        if (varStr != undefStr)
                            var = double.Parse(varStr) / scaleFactor;

                        break;
                }

                //DiscreteData[0, i] = lon;
                //DiscreteData[1, i] = lat;
                //DiscreteData[2, i] = var;
                disDataList.Add(new double[] { lon, lat, var });
                stIDList.Add(aStid);

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
            MeteoInfoC.Global.Extent dataExtent = new MeteoInfoC.Global.Extent();
            dataExtent.minX = minX;
            dataExtent.maxX = maxX;
            dataExtent.minY = minY;
            dataExtent.maxY = maxY;

            //Return
            DiscreteData = new double[3, disDataList.Count];
            for (i = 0; i < disDataList.Count; i++)
            {
                DiscreteData[0, i] = disDataList[i][0];
                DiscreteData[1, i] = disDataList[i][1];
                DiscreteData[2, i] = disDataList[i][2];
            }

            stationData.Data = DiscreteData;
            stationData.DataExtent = dataExtent;
            stationData.Stations = stIDList;

            return stationData;
        }

        /// <summary>
        /// Get station model data
        /// </summary>        
        /// <param name="stIDList">ref station identifer list</param>
        /// <param name="dataExtent">ref data extent</param>
        /// <returns>station model data</returns>
        public double[,] GetStationModelData(ref List<string> stIDList, ref Global.Extent dataExtent)
        {
            string aStid;
            int i;
            double lon, lat, var;
            string aLine;
            double[,] DiscreteData = new double[10, dataList.Count];
            double minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            string lonStr, latStr, varStr;
            stIDList = new List<string>();

            //Loop                      
            for (i = 0; i < dataList.Count; i++)
            {
                aLine = dataList[i];
                aStid = aLine.Substring(4, 6);
                latStr = aLine.Substring(28, 6);
                if (latStr == "+99999")
                    continue;
                lat = double.Parse(latStr) / 1000;
                lonStr = aLine.Substring(34, 7);
                if (lonStr == "+999999")
                    continue;
                lon = double.Parse(lonStr) / 1000;
                DiscreteData[0, i] = lon;
                DiscreteData[1, i] = lat;
                stIDList.Add(aStid);

                for (int j = 0; j < 8; j++)
                {
                    int startIdx = 0, vLen = 1, scaleFactor = 1;
                    string undefStr = "";
                    var = MissingValue;
                    bool hasData = true;
                    switch (j)
                    {
                        case 0:    //Wind direction
                            startIdx = 60;
                            vLen = 3;
                            undefStr = "999";
                            break;
                        case 1:    //Wind speed
                            startIdx = 65;
                            vLen = 4;
                            undefStr = "9999";
                            scaleFactor = 10;
                            break;
                        case 2:    //visibility
                            startIdx = 78;
                            vLen = 6;
                            undefStr = "999999";
                            break;
                        case 5:    //Temperature
                            startIdx = 87;
                            vLen = 5;
                            undefStr = "+9999";
                            scaleFactor = 10;
                            break;
                        case 6:    //Dew point temperature
                            startIdx = 93;
                            vLen = 5;
                            undefStr = "+9999";
                            scaleFactor = 10;
                            break;
                        case 7:    //Sea level pressure
                            startIdx = 99;
                            vLen = 5;
                            undefStr = "99999";
                            scaleFactor = 10;
                            break;
                        case 3:    //Present weather
                            if (aLine.Contains("MW1"))    //Manual report
                            {
                                startIdx = aLine.IndexOf("MW1") + 3;
                                vLen = 2;                                
                            }
                            else if (aLine.Contains("AW1"))    //Automatic report
                            {
                                startIdx = aLine.IndexOf("AW1") + 3;
                                vLen = 2;                                
                            }
                            else
                                hasData = false;
                            break;
                        case 4:    //Cloud cover
                            if (aLine.Contains("GF1"))
                            {
                                startIdx = aLine.IndexOf("GF1") + 3;
                                vLen = 2;
                                undefStr = "99";                                
                            }
                            else
                                hasData = false;
                            break;                        
                    }
                    if (hasData)
                    {
                        varStr = aLine.Substring(startIdx, vLen);
                        if (varStr != undefStr)
                        {
                            try { var = double.Parse(varStr) / scaleFactor; }
                            catch { }
                        }
                    }                    
                    
                    DiscreteData[j + 2, i] = var;                    
                }

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

            //Return
            return DiscreteData;
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
            double lon, lat, var;
            string aLine;
            //double[,] DiscreteData = new double[10, dataList.Count];
            List<StationModel> smList = new List<StationModel>();
            double minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            string lonStr, latStr, varStr;
           
            //Loop                      
            for (i = 0; i < dataList.Count; i++)
            {
                aLine = dataList[i];
                aStid = aLine.Substring(4, 6);
                latStr = aLine.Substring(28, 6);
                if (latStr == "+99999")
                    continue;
                lat = double.Parse(latStr) / 1000;
                lonStr = aLine.Substring(34, 7);
                if (lonStr == "+999999")
                    continue;
                lon = double.Parse(lonStr) / 1000;

                StationModel sm = new StationModel();
                sm.Longitude = lon;
                sm.Latitude = lat;                

                for (int j = 0; j < 8; j++)
                {
                    int startIdx = 0, vLen = 1, scaleFactor = 1;
                    string undefStr = "";
                    var = MissingValue;
                    bool hasData = true;
                    switch (j)
                    {
                        case 0:    //Wind direction
                            startIdx = 60;
                            vLen = 3;
                            undefStr = "999";
                            break;
                        case 1:    //Wind speed
                            startIdx = 65;
                            vLen = 4;
                            undefStr = "9999";
                            scaleFactor = 10;
                            break;
                        case 2:    //visibility
                            startIdx = 78;
                            vLen = 6;
                            undefStr = "999999";
                            break;
                        case 5:    //Temperature
                            startIdx = 87;
                            vLen = 5;
                            undefStr = "+9999";
                            scaleFactor = 10;
                            break;
                        case 6:    //Dew point temperature
                            startIdx = 93;
                            vLen = 5;
                            undefStr = "+9999";
                            scaleFactor = 10;
                            break;
                        case 7:    //Sea level pressure
                            startIdx = 99;
                            vLen = 5;
                            undefStr = "99999";
                            scaleFactor = 10;
                            break;
                        case 3:    //Present weather
                            if (aLine.Contains("MW1"))    //Manual report
                            {
                                startIdx = aLine.IndexOf("MW1") + 3;
                                vLen = 2;
                            }
                            else if (aLine.Contains("AW1"))    //Automatic report
                            {
                                startIdx = aLine.IndexOf("AW1") + 3;
                                vLen = 2;
                            }
                            else
                                hasData = false;
                            break;
                        case 4:    //Cloud cover
                            if (aLine.Contains("GF1"))
                            {
                                startIdx = aLine.IndexOf("GF1") + 3;
                                vLen = 2;
                                undefStr = "99";
                            }
                            else
                                hasData = false;
                            break;
                    }
                    if (hasData)
                    {
                        varStr = aLine.Substring(startIdx, vLen);
                        if (varStr != undefStr)
                        {
                            try { var = double.Parse(varStr) / scaleFactor; }
                            catch { }
                        }
                    }

                    switch (j)
                    {
                        case 0:    //Wind direction
                            sm.WindDirection = var;
                            break;
                        case 1:    //Wind speed
                            sm.WindSpeed = var;
                            break;
                        case 2:    //visibility
                            sm.Visibility = var;
                            break;
                        case 5:    //Temperature
                            sm.Temperature = var;
                            break;
                        case 6:    //Dew point temperature
                            sm.DewPoint = var;
                            break;
                        case 7:    //Sea level pressure
                            sm.Pressure = var;
                            break;
                        case 3:    //Present weather
                            sm.Weather = var;
                            break;
                        case 4:    //Cloud cover
                            sm.CloudCover = var;
                            break;
                    }
                }

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
