using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// MICAPS data methods
    /// </summary>
    public class MICAPSData
    {
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
            ArrayList dataList = new ArrayList();

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

        /// <summary>
        /// Read MICAPS 1 data info
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public MICAPS1DataInfo ReadMicaps1(string aFile)
        {
            StreamReader sr = new StreamReader(aFile, Encoding.Default);
            string aLine;
            string[] dataArray;
            int i, n, LastNonEmpty, dataNum;
            List<string> dataList = new List<string>();
            List<List<string>> disDataList = new List<List<string>>();
            MICAPS1DataInfo aM1DataInfo = new MICAPS1DataInfo();

            aM1DataInfo.FileName = aFile;
            aLine = sr.ReadLine();
            aM1DataInfo.Description = aLine;
            if (aLine.Contains("自动"))
            {
                aM1DataInfo.isAutoStation = true;
            }
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
            aM1DataInfo.DateTime = Convert.ToDateTime(dataList[0] + "-" + dataList[1] + "-" + dataList[2] +
                " " + dataList[3] + ":00");
            aM1DataInfo.stNum = Convert.ToInt32(dataList[4]);

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

                if (dataList.Count < 26 && (string)dataList[22] == "1" && (string)dataList[23] == "2")
                {
                    aLine = sr.ReadLine();
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

                if (dataNum == 0)
                {
                    if (dataList.Count == 26)
                    {
                        aM1DataInfo.hasAllCols = true;
                    }
                    else
                    {
                        aM1DataInfo.hasAllCols = false;
                    }
                }

                dataNum++;
                if (dataNum == 1)
                {
                    aM1DataInfo.varNum = dataList.Count;
                }
                disDataList.Add(dataList);
                dataList = new List<string>();

            }
            while (aLine != null);

            sr.Close();

            aM1DataInfo.DataList = disDataList;
            return aM1DataInfo;
        }

        /// <summary>
        /// Read MICAPS 4 data info
        /// </summary>
        /// <param name="aFile"></param>
        /// <returns></returns>
        public MICAPS4DataInfo ReadMicaps4(string aFile)
        {
            StreamReader sr = new StreamReader(aFile, Encoding.Default);
            string aLine;
            string[] dataArray;
            int i, j, n, LastNonEmpty;
            ArrayList dataList = new ArrayList();
            ArrayList disDataList = new ArrayList();
            MICAPS4DataInfo aM4DataInfo = new MICAPS4DataInfo();

            aM4DataInfo.FileName = aFile;
            aLine = sr.ReadLine();
            aM4DataInfo.Description = aLine;
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
            for (n = 0; n <= 10; n++)
            {
                if (dataList.Count < 19)
                {
                    aLine = sr.ReadLine();
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
            aM4DataInfo.DateTime = Convert.ToDateTime(dataList[0] + "-" + dataList[1] + "-" + dataList[2] +
                " " + dataList[3] + ":00");
            aM4DataInfo.hours = Convert.ToInt32(dataList[4]);
            aM4DataInfo.level = Convert.ToInt32(dataList[5]);
            aM4DataInfo.XDelt = Convert.ToSingle(dataList[6]);
            aM4DataInfo.YDelt = Convert.ToSingle(dataList[7]);
            aM4DataInfo.XMin = Convert.ToSingle(dataList[8]);
            aM4DataInfo.XMax = Convert.ToSingle(dataList[9]);
            aM4DataInfo.YMin = Convert.ToSingle(dataList[10]);
            aM4DataInfo.YMax = Convert.ToSingle(dataList[11]);
            aM4DataInfo.XNum = Convert.ToInt32(dataList[12]);
            aM4DataInfo.YNum = Convert.ToInt32(dataList[13]);
            aM4DataInfo.contourDelt = Convert.ToSingle(dataList[14]);
            aM4DataInfo.contourSValue = Convert.ToSingle(dataList[15]);
            aM4DataInfo.contourEValue = Convert.ToSingle(dataList[16]);
            aM4DataInfo.smoothCo = Convert.ToSingle(dataList[17]);
            aM4DataInfo.boldValue = Convert.ToSingle(dataList[18]);
            if ((string)dataList[16] == "-1" || (string)dataList[16] == "-2" || (string)dataList[16] == "-3")
            {
                aM4DataInfo.isLonLat = false;
            }
            else
            {
                aM4DataInfo.isLonLat = true;
            }
            aM4DataInfo.X = new double[aM4DataInfo.XNum];
            for (i = 0; i < aM4DataInfo.XNum; i++)
            {
                aM4DataInfo.X[i] = aM4DataInfo.XMin + i * aM4DataInfo.XDelt;
            }
            aM4DataInfo.Y = new double[aM4DataInfo.YNum];
            for (i = 0; i < aM4DataInfo.YNum; i++)
            {
                aM4DataInfo.Y[i] = aM4DataInfo.YMin + i * aM4DataInfo.YDelt;
            }

            string dataStr = sr.ReadToEnd();
            sr.Close();
            dataArray = dataStr.Split();
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

            double[,] gridData = new double[aM4DataInfo.YNum, aM4DataInfo.XNum];
            for (i = 0; i < aM4DataInfo.YNum; i++)
            {
                for (j = 0; j < aM4DataInfo.XNum; j++)
                {
                    gridData[i, j] = Convert.ToDouble(dataList[i * aM4DataInfo.XNum + j]);
                }
            }

            aM4DataInfo.GridData = new double[aM4DataInfo.YNum, aM4DataInfo.XNum];
            for (i = 0; i < aM4DataInfo.YNum; i++)
            {
                for (j = 0; j < aM4DataInfo.XNum; j++)
                {
                    aM4DataInfo.GridData[i, j] = gridData[i, j];
                }
            }

            return aM4DataInfo;
        }

        /// <summary>
        /// Read MICAPS 11 data info
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>MICAPS 11 data info</returns>
        public MICAPS11DataInfo ReadMicaps11(string aFile)
        {
            StreamReader sr = new StreamReader(aFile, Encoding.Default);
            string aLine;
            string[] dataArray;
            int i, j, n, LastNonEmpty;
            List<string> dataList = new List<string>();            
            MICAPS11DataInfo aM11DataInfo = new MICAPS11DataInfo();

            aM11DataInfo.FileName = aFile;
            aLine = sr.ReadLine();
            aM11DataInfo.Description = aLine;
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
            for (n = 0; n <= 10; n++)
            {
                if (dataList.Count < 14)
                {
                    aLine = sr.ReadLine();
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
            aM11DataInfo.DateTime = Convert.ToDateTime(dataList[0] + "-" + dataList[1] + "-" + dataList[2] +
                " " + dataList[3] + ":00");
            aM11DataInfo.hours = Convert.ToInt32(dataList[4]);
            aM11DataInfo.level = Convert.ToInt32(dataList[5]);
            aM11DataInfo.XDelt = Convert.ToSingle(dataList[6]);
            aM11DataInfo.YDelt = Convert.ToSingle(dataList[7]);
            aM11DataInfo.XMin = Convert.ToSingle(dataList[8]);
            aM11DataInfo.XMax = Convert.ToSingle(dataList[9]);
            aM11DataInfo.YMin = Convert.ToSingle(dataList[10]);
            aM11DataInfo.YMax = Convert.ToSingle(dataList[11]);
            aM11DataInfo.XNum = Convert.ToInt32(dataList[12]);
            aM11DataInfo.YNum = Convert.ToInt32(dataList[13]);
            
            if (aM11DataInfo.XMax > 1000)
            {
                aM11DataInfo.isLonLat = false;
            }
            else
            {
                aM11DataInfo.isLonLat = true;
            }
            aM11DataInfo.X = new double[aM11DataInfo.XNum];
            for (i = 0; i < aM11DataInfo.XNum; i++)
            {
                aM11DataInfo.X[i] = aM11DataInfo.XMin + i * aM11DataInfo.XDelt;
            }
            aM11DataInfo.Y = new double[aM11DataInfo.YNum];
            for (i = 0; i < aM11DataInfo.YNum; i++)
            {
                aM11DataInfo.Y[i] = aM11DataInfo.YMin + i * aM11DataInfo.YDelt;
            }

            string dataStr = sr.ReadToEnd();
            sr.Close();
            dataArray = dataStr.Split();
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

            aM11DataInfo.UGridData = new double[aM11DataInfo.YNum, aM11DataInfo.XNum];
            aM11DataInfo.VGridData = new double[aM11DataInfo.YNum, aM11DataInfo.XNum];
            int dataNum=aM11DataInfo .YNum * aM11DataInfo .XNum;
            for (i = 0; i < aM11DataInfo.YNum; i++)
            {
                for (j = 0; j < aM11DataInfo.XNum; j++)
                {
                    aM11DataInfo.UGridData[i, j] = double.Parse(dataList[i * aM11DataInfo.XNum + j]);
                    aM11DataInfo.VGridData[i, j] = double.Parse(dataList[dataNum + i * aM11DataInfo.XNum + j]);
                }
            }            

            return aM11DataInfo;
        }

        /// <summary>
        /// Get discrete data from MICAPS 1 data info
        /// </summary>
        /// <param name="aMDInfo"></param>
        /// <param name="vIdx"></param>
        /// <param name="dataExtent"></param>
        /// <returns></returns>
        public double[,] GetDiscreteData_M1(MICAPS1DataInfo aMDInfo, int vIdx, ref Global.Extent dataExtent)
        {
            string aStid;
            int i;
            Single lon, lat, t;
            List<string> dataList = new List<string>();
            double[,] DiscreteData = new double[3, aMDInfo.DataList.Count];
            Single minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;

            for (i = 0; i < aMDInfo.DataList.Count; i++)
            {
                dataList = aMDInfo.DataList[i];
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

        ///// <summary>
        ///// Get station model data of MICAPS1
        ///// </summary>
        ///// <param name="aMDInfo"></param>
        ///// <param name="dataExtent"></param>
        ///// <returns></returns>
        //public double[,] GetStationModelData_M1(MICAPS1DataInfo aMDInfo, ref Global.Extent dataExtent)
        //{
        //    string aStid;
        //    int i;
        //    Single lon, lat;
        //    List<string> dataList = new List<string>();
        //    double[,] DiscreteData = new double[10, aMDInfo.DataList.Count];
        //    Single minX, maxX, minY, maxY;
        //    minX = 0;
        //    maxX = 0;
        //    minY = 0;
        //    maxY = 0;

        //    for (i = 0; i < aMDInfo.DataList.Count; i++)
        //    {
        //        dataList = aMDInfo.DataList[i];
        //        aStid = (string)dataList[0];
        //        lon = Convert.ToSingle(dataList[1]);
        //        lat = Convert.ToSingle(dataList[2]);                               
        //        //if (lon < 0)
        //        //{
        //        //    lon += 360;
        //        //}
        //        DiscreteData[0, i] = lon;
        //        DiscreteData[1, i] = lat;
        //        DiscreteData[2, i] = Convert.ToDouble(dataList[6]);    //Wind direction
        //        DiscreteData[3, i] = Convert.ToDouble(dataList[7]);    //Wind speed
        //        DiscreteData[4, i] = Convert.ToDouble(dataList[17]);    //Visibility
        //        DiscreteData[5, i] = Convert.ToDouble(dataList[18]);    //Weather
        //        DiscreteData[6, i] = Convert.ToDouble(dataList[5]);    //Cloud cover
        //        DiscreteData[7, i] = Convert.ToDouble(dataList[19]);    //Temperature
        //        DiscreteData[8, i] = Convert.ToDouble(dataList[16]);    //Dew point
        //        //Pressure
        //        double press = Convert.ToDouble(dataList[8]);
        //        if (Math.Abs(press / aMDInfo.UNDEF - 1) < 0.01)
        //        {
        //            DiscreteData[9, i] = press;
        //        }
        //        else
        //        {
        //            if (press > 800)
        //            {
        //                DiscreteData[9, i] = press / 10 + 1000;
        //            }
        //            else
        //            {
        //                DiscreteData[9, i] = press / 10 + 900;
        //            }
        //        }

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
        /// Generate data info text of MICAPS 1
        /// </summary>
        /// <param name="aDataInfo"></param>
        /// <returns></returns>
        public string GenerateInfoText_M1(MICAPS1DataInfo aDataInfo)
        {
            string dataInfo;
            dataInfo = "File Name: " + aDataInfo.FileName;
            dataInfo += Environment.NewLine + "Description: " + aDataInfo.Description;
            dataInfo += Environment.NewLine + "Time: " + aDataInfo.DateTime.ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "Station Number: " + aDataInfo.stNum;

            return dataInfo;
        }

        /// <summary>
        /// Generate data info text of MICAPS 4
        /// </summary>
        /// <param name="aDataInfo"></param>
        /// <returns></returns>
        public string GenerateInfoText_M4(MICAPS4DataInfo aDataInfo)
        {
            string dataInfo;
            dataInfo = "File Name: " + aDataInfo.FileName;
            dataInfo += Environment.NewLine + "Description: " + aDataInfo.Description;
            dataInfo += Environment.NewLine + "Time: " + aDataInfo.DateTime.ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "Forecast Hours = " + aDataInfo.hours.ToString() +
                "  Level = " + aDataInfo.level.ToString();
            dataInfo += Environment.NewLine + "Xsize = " + aDataInfo.XNum.ToString() +
                    "  Ysize = " + aDataInfo.YNum.ToString();

            return dataInfo;
        }

        /// <summary>
        /// Generate data info text of MICAPS 11
        /// </summary>
        /// <param name="aDataInfo"></param>
        /// <returns></returns>
        public string GenerateInfoText_M11(MICAPS11DataInfo aDataInfo)
        {
            string dataInfo;
            dataInfo = "File Name: " + aDataInfo.FileName;
            dataInfo += Environment.NewLine + "Description: " + aDataInfo.Description;
            dataInfo += Environment.NewLine + "Time: " + aDataInfo.DateTime.ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "Forecast Hours = " + aDataInfo.hours.ToString() +
                "  Level = " + aDataInfo.level.ToString();
            dataInfo += Environment.NewLine + "Xsize = " + aDataInfo.XNum.ToString() +
                    "  Ysize = " + aDataInfo.YNum.ToString();

            return dataInfo;
        }

        /// <summary>
        /// Get weather list
        /// </summary>
        /// <param name="weatherType"></param>
        /// <returns></returns>
        public ArrayList GetWeatherTypes(string weatherType)
        {
            ArrayList weatherList = new ArrayList();
            int i;
            switch (weatherType)
            {
                case "All Weather":
                    for (i = 4; i < 100; i++)
                    {
                        weatherList.Add(i);
                    }
                    break;
                case "SDS":
                    weatherList = ArrayList.Adapter(new int[] { 6, 7, 8, 9, 30, 31, 32, 33, 34, 35 });
                    break;
                case "SDS, Haze":
                    weatherList = ArrayList.Adapter(new int[] { 5, 6, 7, 8, 9, 30, 31, 32, 33, 34, 35 });
                    break;
                case "Smoke, Haze,Mist":
                    weatherList = ArrayList.Adapter(new int[] { 4, 5, 10 });
                    break;
                case "Smoke":
                    weatherList.Add(4);
                    break;
                case "Haze":
                    weatherList.Add(5);
                    break;
                case "Mist":
                    weatherList.Add(10);
                    break;
                case "Fog":
                    for (i = 40; i < 50; i++)
                    {
                        weatherList.Add(i);
                    }
                    break;
            }

            return weatherList;
        }

        /// <summary>
        /// Convert MICAPS1 data to GrADS station data
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        public void MICAPS1ToGrADS(string inFile, string outFile)
        {
            MICAPS1DataInfo aDataInfo = new MICAPS1DataInfo();
            GrADSDataInfo CGrADSData = new GrADSDataInfo();

            aDataInfo = ReadMicaps1(inFile);
            CGrADSData.WriteGrADSStationData(outFile, aDataInfo);
            CGrADSData.WriteGrADSCtlFile_Station(outFile, aDataInfo);
        }
    }
}
