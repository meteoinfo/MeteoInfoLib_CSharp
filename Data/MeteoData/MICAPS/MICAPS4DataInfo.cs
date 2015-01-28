using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// MICAPS 4 data info
    /// </summary>
    public class MICAPS4DataInfo:DataInfo,IGridDataInfo
    {
        #region Variables        
        /// <summary>
        /// Forecast hours
        /// </summary>
        public int hours;
        /// <summary>
        /// Level
        /// </summary>
        public int level;
        /// <summary>
        /// X delt
        /// </summary>
        public Single XDelt;
        /// <summary>
        /// Y delt
        /// </summary>
        public Single YDelt;
        /// <summary>
        /// X minimum
        /// </summary>
        public Single XMin;
        /// <summary>
        /// X maximum
        /// </summary>
        public Single XMax;
        /// <summary>
        /// Y minimum
        /// </summary>
        public Single YMin;
        /// <summary>
        /// Y maximum
        /// </summary>
        public Single YMax;
        /// <summary>
        /// Y number
        /// </summary>
        public int YNum;
        /// <summary>
        /// X number
        /// </summary>
        public int XNum;
        /// <summary>
        /// Contour delt
        /// </summary>
        public Single contourDelt;
        /// <summary>
        /// Contour start value
        /// </summary>
        public Single contourSValue;
        /// <summary>
        /// Contour end value
        /// </summary>
        public Single contourEValue;
        /// <summary>
        /// Smooth coefficient
        /// </summary>
        public Single smoothCo;
        /// <summary>
        /// Bold contour value
        /// </summary>
        public Single boldValue;
        /// <summary>
        /// Is Lat/Lon
        /// </summary>
        public Boolean isLonLat;
        /// <summary>
        /// Data array
        /// </summary>
        public double[,] GridData;       
        /// <summary>
        /// X array
        /// </summary>
        public double[] X;
        /// <summary>
        /// Y array
        /// </summary>
        public double[] Y;
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
        public MICAPS4DataInfo()
        {
            this.MissingValue = 9999;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Read MICAPS 4 data info
        /// </summary>
        /// <param name="aFile"></param>    
        public override void ReadDataInfo(string aFile)
        {
            StreamReader sr = new StreamReader(aFile, Encoding.Default);
            string aLine;
            string[] dataArray;
            int i, j, n, LastNonEmpty;
            List<string> dataList = new List<string>();
            List<string> disDataList = new List<string>();            

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
            //DateTime = Convert.ToDateTime(dataList[0] + "-" + dataList[1] + "-" + dataList[2] +
            //    " " + dataList[3] + ":00");
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

            hours = Convert.ToInt32(dataList[4]);
            level = Convert.ToInt32(dataList[5]);
            XDelt = Convert.ToSingle(dataList[6]);
            YDelt = Convert.ToSingle(dataList[7]);
            XMin = Convert.ToSingle(dataList[8]);
            XMax = Convert.ToSingle(dataList[9]);
            YMin = Convert.ToSingle(dataList[10]);
            YMax = Convert.ToSingle(dataList[11]);
            XNum = Convert.ToInt32(dataList[12]);
            YNum = Convert.ToInt32(dataList[13]);
            contourDelt = Convert.ToSingle(dataList[14]);
            contourSValue = Convert.ToSingle(dataList[15]);
            contourEValue = Convert.ToSingle(dataList[16]);
            smoothCo = Convert.ToSingle(dataList[17]);
            boldValue = Convert.ToSingle(dataList[18]);
            if ((string)dataList[16] == "-1" || (string)dataList[16] == "-2" || (string)dataList[16] == "-3")
            {
                isLonLat = false;
            }
            else
            {
                isLonLat = true;
            }
            X = new double[XNum];
            for (i = 0; i < XNum; i++)
            {
                X[i] = XMin + i * XDelt;
            }
            Y = new double[YNum];

            bool yReversed = false;
            if (YDelt < 0)
            {
                yReversed = true;
                YDelt = -YDelt;
            }
            if (YMin > YMax)
            {
                float temp = YMin;
                YMin = YMax;
                YMax = temp;
            }
            for (i = 0; i < YNum; i++)
            {
                Y[i] = YMin + i * YDelt;
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

            double[,] gridData = new double[YNum, XNum];
            for (i = 0; i < YNum; i++)
            {
                for (j = 0; j < XNum; j++)
                {
                    gridData[i, j] = Convert.ToDouble(dataList[i * XNum + j]);
                }
            }

            if (!yReversed)
                GridData = gridData;
            else
            {
                GridData = new double[YNum, XNum];
                for (i = 0; i < YNum; i++)
                {
                    for (j = 0; j < XNum; j++)
                    {
                        GridData[i, j] = gridData[YNum - i - 1, j];
                    }
                }
            }

            Dimension tdim = new Dimension(DimensionType.T);
            tdim.DimValue.Add(DataConvert.ToDouble(DateTime));
            tdim.DimLength = 1;
            this.TimeDimension = tdim;
            Dimension zdim = new Dimension(DimensionType.Z);
            zdim.DimValue.Add(level);
            zdim.DimLength = 1;
            Dimension xdim = new Dimension(DimensionType.X);
            xdim.SetValues(X);
            Dimension ydim = new Dimension(DimensionType.Y);
            ydim.SetValues(Y);

            List<Variable> variables = new List<Variable>();
            Variable var = new Variable();
            var.Name = "var";
            var.IsStation = true;
            var.SetDimension(tdim);
            var.SetDimension(zdim);
            var.SetDimension(ydim);
            var.SetDimension(xdim);
            variables.Add(var);
            this.Variables = variables;
        }

        /// <summary>
        /// Generate data info text of MICAPS 4
        /// </summary>        
        /// <returns></returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + FileName;
            dataInfo += Environment.NewLine + "Description: " + Description;
            dataInfo += Environment.NewLine + "Time: " + DateTime.ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "Forecast Hours = " + hours.ToString() +
                "  Level = " + level.ToString();
            dataInfo += Environment.NewLine + "Xsize = " + XNum.ToString() +
                    "  Ysize = " + YNum.ToString();

            return dataInfo;
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
            GridData gridData = new GridData();
            gridData.Data = GridData;
            gridData.X = X;
            gridData.Y = Y;           
            gridData.MissingValue = MissingValue;

            return gridData;
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
