using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// MICAPS 3 data info
    /// </summary>
    public class MICAPS3DataInfo:DataInfo,IStationDataInfo
    {
        #region Variables
        /// <summary>
        /// Station number
        /// </summary>
        public int StationNum;
        /// <summary>
        /// Height level
        /// </summary>
        public int Level;
        /// <summary>
        /// Contour number
        /// </summary>
        public int ContourNum;
        /// <summary>
        /// Contour values
        /// </summary>
        public List<float> Contours;
        /// <summary>
        /// Field list
        /// </summary>
        public List<string> FieldList;
        /// <summary>
        /// Variable number
        /// </summary>
        public int varNum;
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
        public MICAPS3DataInfo()
        {
            Contours = new List<float>();
            FieldList = new List<string>();
            VarList = new List<string>();            
            DataList = new List<List<string>>();            
            this.MissingValue = 9999;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read data info
        /// </summary>
        /// <param name="aFile">file path</param>                
        public override void ReadDataInfo(string aFile)
        {
            FileName = aFile;
            
            int i;
            StreamReader sr = new StreamReader(aFile, System.Text.Encoding.Default);
            string[] dataArray;
            List<string> dataList = new List<string>();            

            //Read file head
            string aLine = sr.ReadLine();
            Description = aLine;      
            //Read all lines
            aLine = sr.ReadLine();            
            string bLine;
            while ((bLine = sr.ReadLine()) != null)
            {
                aLine = aLine + " " + bLine;
            }
            sr.Close();

            dataArray = aLine.Split();            
            dataList.Clear();            
            for (i = 0; i < dataArray.Length; i++)
            {
                if (dataArray[i] != string.Empty)
                {                    
                    dataList.Add(dataArray[i]);
                }
            }

            //DateTime = Convert.ToDateTime(dataList[0] + "-" + dataList[1] + "-" + dataList[2] +
            //        " " + dataList[3] + ":00");
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
            ContourNum = int.Parse(dataList[5]);
            for (i = 0; i < ContourNum; i++)
            {
                Contours.Add(float.Parse(dataList[6 + i]));
            }

            int idx = 6 + ContourNum + 2;
            int pNum = int.Parse(dataList[idx]);
            idx += pNum * 2 + 1;

            varNum = int.Parse(dataList[idx]);
            idx += 1;
            StationNum = int.Parse(dataList[idx]);
            idx += 1;
            for (i = 0; i < varNum; i++)
            {
                VarList.Add("Var" + (i + 1).ToString());
            }
            FieldList.Add("Stid");
            FieldList.Add("Longitude");
            FieldList.Add("Latitude");
            FieldList.Add("Altitude");
            FieldList.AddRange(VarList);

            while(idx + 3 + varNum < dataList.Count)
            {
                List<string> aData = new List<string>();
                for (int j = 0; j < 4 + varNum; j++)
                {                    
                    aData.Add(dataList[idx]);
                    idx += 1;
                }
                DataList.Add(aData);
            }                     

            StationNum = DataList.Count;

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
        }

        /// <summary>
        /// Generate data info text
        /// </summary>        
        /// <returns>info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + FileName;
            dataInfo += Environment.NewLine + "Description: " + Description;
            dataInfo += Environment.NewLine + "Time: " + DateTime.ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "Station Number: " + StationNum;
            dataInfo += Environment.NewLine + "Fields: ";
            foreach (string aField in FieldList)
                dataInfo += Environment.NewLine + "  " + aField;

            return dataInfo;
        }

        /// <summary>
        /// Get discrete Lon/Lat station data
        /// </summary>        
        /// <param name="vIdx">varible index</param>
        /// <param name="dataExtent"></param>
        /// <returns></returns>
        public double[,] GetDiscreteData(int vIdx, ref Global.Extent dataExtent)
        {
            string stName;
            int i;
            double lon, lat;
            double t;
            t = 0;

            List<string> dataList = new List<string>();
            double[,] DiscreteData = new double[3, DataList.Count];
            double minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            
            //Get real variable index
            int varIdx = FieldList.IndexOf(VarList[vIdx]);

            for (i = 0; i < DataList.Count; i++)
            {
                dataList = DataList[i];
                stName = dataList[0];
                lon = double.Parse(dataList[1]);
                lat = double.Parse(dataList[2]);
                t = double.Parse(dataList[varIdx]);
                //if (lon < 0)
                //{
                //    lon += 360;
                //}
                //Initialize data
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
        /// Get discrete Lon/Lat station data
        /// </summary>        
        /// <param name="vIdx">varible index</param>
        /// <param name="stations">ref stations</param>
        /// <param name="dataExtent">ref data extent</param>
        /// <returns>discrete data</returns>
        public double[,] GetDiscreteData(int vIdx, ref List<string> stations, ref Global.Extent dataExtent)
        {
            string stName;
            int i;
            double lon, lat;
            double t;
            t = 0;

            List<string> dataList = new List<string>();
            double[,] DiscreteData = new double[3, DataList.Count];
            double minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            stations.Clear();
            
            //Get real variable index
            int varIdx = FieldList.IndexOf(VarList[vIdx]);

            for (i = 0; i < DataList.Count; i++)
            {
                dataList = DataList[i];
                stName = dataList[0];
                lon = double.Parse(dataList[1]);
                lat = double.Parse(dataList[2]);
                t = double.Parse(dataList[varIdx]);
                //if (lon < 0)
                //{
                //    lon += 360;
                //}
                //Initialize data
                stations.Add(stName);
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
