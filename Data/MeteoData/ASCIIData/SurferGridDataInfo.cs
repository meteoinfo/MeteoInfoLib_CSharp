using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Surfer grid data info
    /// </summary>
    public class SurferGridDataInfo : DataInfo,IGridDataInfo
    {
        #region variables        
        /// <summary>
        /// if data set is lat/lon
        /// </summary>
        public bool isLonLat;              
        /// <summary>
        /// levels array
        /// </summary>
        public double[] levels;
        /// <summary>
        /// x coordinate array
        /// </summary>
        public double[] X;
        /// <summary>
        /// y coordinate array
        /// </summary>
        public double[] Y;
        /// <summary>
        /// x delt, y delt
        /// </summary>
        public double XDelt, YDelt;
        /// <summary>
        /// x number, y number
        /// </summary>
        public int XNum, YNum;
        /// <summary>
        /// x/y minimum, x/y maximum
        /// </summary>
        public double XMin, XMax, YMin, YMax;
        /// <summary>
        /// z minimum, z maximum
        /// </summary>
        public double ZMin, ZMax;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        ///<param name="aFile">file path</param>
        public SurferGridDataInfo(string aFile):this()
        {                        
            ReadDataInfo(aFile);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SurferGridDataInfo()
        {
            isLonLat = true;
            //times = new List<DateTime>();
            MissingValue = -9999;
            //isGlobal = false;
        }

        #endregion

        #region Properties


        #endregion

        #region Methods
        /// <summary>
        /// Read data info
        /// </summary>
        /// <param name="aFile">file path</param>        
        public override void ReadDataInfo(string aFile)
        {
            FileName = aFile;

            //Read file
            StreamReader sr = new StreamReader(aFile);            
            int LastNonEmpty, i;
            string aLine;
            string[] dataArray;
            List<string> dataList = new List<string>();

            aLine = sr.ReadLine();
            for (i = 1; i <= 4; i++)
                aLine = aLine + " " + sr.ReadLine();

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

            XNum = int.Parse(dataList[1]);
            YNum = int.Parse(dataList[2]);
            XMin = double.Parse(dataList[3]);
            XMax = double.Parse(dataList[4]);
            YMin = double.Parse(dataList[5]);
            YMax = double.Parse(dataList[6]);
            ZMin = double.Parse(dataList[7]);
            ZMax = double.Parse(dataList[8]);
            
            XDelt = (XMax - XMin) / (XNum - 1);
            YDelt = (YMax - YMin) / (YNum - 1);            
            X = new double[XNum];
            for (i = 0; i < XNum; i++)
            {
                X[i] = XMin + i * XDelt;
            }
            Y = new double[YNum];
            for (i = 0; i < YNum; i++)
            {
                Y[i] = YMin + i * YDelt;
            }

            sr.Close();

            Dimension xDim = new Dimension(DimensionType.X);
            xDim.SetValues(X);
            Dimension yDim = new Dimension(DimensionType.Y);
            yDim.SetValues(Y);
            List<Variable> variables = new List<Variable>();
            Variable var = new Variable();
            var.Name = "var";
            var.SetDimension(xDim);
            var.SetDimension(yDim);
            variables.Add(var);
            this.Variables = variables;
        }

        /// <summary>
        /// Read grid data
        /// </summary>
        /// <returns>grid data</returns>
        public double[,] ReadData()
        {            
            double[,] gridData = new double[YNum, XNum];

            //Read file
            StreamReader sr = new StreamReader(FileName);
            int LastNonEmpty, i;
            string aLine;
            string[] dataArray;
            List<string> dataList = new List<string>();
            
            for (i = 0; i < 5; i++)
                sr.ReadLine();

            aLine = sr.ReadLine();
            int ii, jj;
            int d = 0;
            while (aLine != null)
            {
                if (aLine.Trim() == "")
                {
                    aLine = sr.ReadLine();
                    continue;
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

                for (i = 0; i < dataList.Count; i++)
                {
                    ii = d / XNum;
                    jj = d % XNum;
                    gridData[ii, jj] = double.Parse(dataList[i]);
                    d += 1;
                }

                aLine = sr.ReadLine();
            }
            
            sr.Close();            

            return gridData;
        }

        /// <summary>
        /// Read grid data - LonLat
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_LonLat(int timeIdx, int varIdx, int levelIdx)
        {            
            double[,] gridData = new double[YNum, XNum];

            //Read file
            StreamReader sr = new StreamReader(FileName);
            int LastNonEmpty, i;
            string aLine;
            string[] dataArray;
            List<string> dataList = new List<string>();

            for (i = 0; i < 5; i++)
                sr.ReadLine();

            aLine = sr.ReadLine();
            int ii, jj;
            int d = 0;
            while (aLine != null)
            {
                if (aLine.Trim() == "")
                {
                    aLine = sr.ReadLine();
                    continue;
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

                for (i = 0; i < dataList.Count; i++)
                {
                    ii = d / XNum;
                    jj = d % XNum;
                    gridData[ii, jj] = double.Parse(dataList[i]);
                    d += 1;
                }

                aLine = sr.ReadLine();
            }

            sr.Close();

            GridData aGridData = new GridData();
            aGridData.Data = gridData;
            aGridData.MissingValue = MissingValue;
            aGridData.X = X;
            aGridData.Y = Y;           

            return aGridData;
        }

        /// <summary>
        /// Generate data info text
        /// </summary>        
        /// <returns>info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + FileName;
            dataInfo += Environment.NewLine + "Data Type: Surfer Grid";
            dataInfo += Environment.NewLine + "XNum = " + XNum.ToString() +
                    "  YNum = " + YNum.ToString();
            dataInfo += Environment.NewLine + "XMin = " + XMin.ToString() +
                "  YMin = " + YMin.ToString();
            dataInfo += Environment.NewLine + "XSize = " + XDelt.ToString() +
                "  YSize = " + YDelt.ToString();
            dataInfo += Environment.NewLine + "MissingValue = " + MissingValue.ToString();

            return dataInfo;
        }

        /// <summary>
        /// Read grid data - TimeLat
        /// </summary>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_TimeLat(int lonIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - TimeLon
        /// </summary>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_TimeLon(int latIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - LevelLat
        /// </summary>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="timeIdx">Time index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_LevelLat(int lonIdx, int varIdx, int timeIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - LevelLon
        /// </summary>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="timeIdx">Time index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_LevelLon(int latIdx, int varIdx, int timeIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - LevelTime
        /// </summary>
        /// <param name="latIdx">Laititude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="lonIdx">Longitude index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_LevelTime(int latIdx, int varIdx, int lonIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - Time
        /// </summary>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_Time(int lonIdx, int latIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - Level
        /// </summary>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="timeIdx">Time index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_Level(int lonIdx, int latIdx, int varIdx, int timeIdx)
        {
            return null;
        }

        /// <summary>
        /// Get grid data - Lon
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level Index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_Lon(int timeIdx, int latIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Get grid data - Lat
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_Lat(int timeIdx, int lonIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        #endregion
    }
}
