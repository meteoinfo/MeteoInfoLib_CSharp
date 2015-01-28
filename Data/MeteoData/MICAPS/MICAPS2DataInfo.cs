using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// MICAPS 2 data info
    /// </summary>
    public class MICAPS2DataInfo : DataInfo,IStationDataInfo
    {
        #region Variables        
        /// <summary>
        /// Station number
        /// </summary>
        public int StNum;
        /// <summary>
        /// Level
        /// </summary>
        public int Level;
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
        public MICAPS2DataInfo()
        {
            string[] items = new string[] {"Height","Temperature","DepDewPoint","WindDirection","WindSpeed"};
            VarList = new List<string>(items.Length);
            VarList.AddRange(items);
            FieldList = new List<string>();
            FieldList.AddRange(new string[] { "Stid", "Longitude", "Latitude", "Altitude", "Grade" });
            FieldList.AddRange(VarList);
            DataList = new List<List<string>>();            
            MissingValue = 9999;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read MICAPS 2 data info
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
            aLine = sr.ReadLine();
            dataArray = aLine.Split();
            dataList.Clear();
            for (i = 0; i < dataArray.Length; i++)
            {
                if (dataArray[i] != string.Empty)
                {
                    dataList.Add(dataArray[i]);
                }
            }
            if (dataList.Count < 6)
            {
                aLine = sr.ReadLine();
                dataArray = aLine.Split();
                for (i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] != string.Empty)
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

            Level = int.Parse(dataList[4]);
            StNum = int.Parse(dataList[5]);

            Dimension tdim = new Dimension(DimensionType.T);
            tdim.DimValue.Add(DataConvert.ToDouble(DateTime));
            tdim.DimLength = 1;
            this.TimeDimension = tdim;
            Dimension zdim = new Dimension(DimensionType.Z);
            zdim.DimValue.Add(Level);
            zdim.DimLength = 1;
            List<Variable> variables = new List<Variable>();
            foreach (string vName in VarList)
            {
                Variable var = new Variable();
                var.Name = vName;
                var.IsStation = true;
                var.SetDimension(tdim);
                var.SetDimension(zdim);
                variables.Add(var);
            }
            this.Variables = variables;

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
                    if (dataList.Count < 10)
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

                if (dataList.Count < 10)
                {
                    break;
                }                                

                dataNum++;
                if (dataNum == 1)
                {
                    VarNum = dataList.Count;
                }
                disDataList.Add(dataList);
                dataList = new List<string>();

            }
            while (aLine != null);

            sr.Close();

            DataList = disDataList;            
        }

        /// <summary>
        /// Generate data info text of MICAPS 2
        /// </summary>       
        /// <returns>info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + FileName;
            dataInfo += Environment.NewLine + "Description: " + Description;
            dataInfo += Environment.NewLine + "Time: " + DateTime.ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "Station Number: " + StNum;

            return dataInfo;
        }        

        /// <summary>
        /// Get discrete data from MICAPS 2 data info
        /// </summary>        
        /// <param name="vIdx">variable index</param>
        /// <param name="stations">ref stations</param>
        /// <param name="dataExtent">ref data extent</param>
        /// <returns>discreted data</returns>
        public double[,] GetDiscreteData(int vIdx, ref List<string> stations, ref Extent dataExtent)
        {
            vIdx += 5;           

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
                StationModel sm = new StationModel();
                sm.Longitude = lon;
                sm.Latitude = lat;
                sm.WindDirection = Convert.ToDouble(dataList[8]);    //Wind direction
                sm.WindSpeed = Convert.ToDouble(dataList[9]);    //Wind speed
                sm.CloudCover = 1;    //Cloud cover
                sm.Temperature = Convert.ToDouble(dataList[6]);    //Temperature
                double ddp = double.Parse(dataList[7]);
                sm.DewPoint = DiscreteData[7, i] - ddp;    //Dew point
                sm.Pressure = double.Parse(dataList[5]);    //Pressure
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

            return stInfoData;
        }

        #endregion
    }
}
