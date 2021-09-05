using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// ASCII GRID data info
    /// </summary>
    public class ASCIIGRIDDataInfo : DataInfo,IGridDataInfo
    {
        #region Variables        
        /// <summary>
        /// if data set is lon/lat
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
        /// x minimum, y minimum
        /// </summary>
        public double XMin, YMin;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ASCIIGRIDDataInfo()
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
        /// Read ASCII GRID data info
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>ASCII GRID data info</returns>
        public override void ReadDataInfo(string aFile)
        {            
            FileName = aFile;

            //double[,] dataArray,newDataArray;
            StreamReader sr = new StreamReader(aFile);
            double xllCorner, yllCorner, cellSize, nodata_value;
            int ncols, nrows, LastNonEmpty, i;
            string aLine;
            string[] dataArray;
            List<string> dataList = new List<string>();

            aLine = sr.ReadLine();
            for (i = 1; i <= 5; i++)
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

            ncols = int.Parse(dataList[1]);
            nrows = int.Parse(dataList[3]);
            xllCorner = double.Parse(dataList[5]);
            yllCorner = double.Parse(dataList[7]);
            cellSize = double.Parse(dataList[9]);
            nodata_value = double.Parse(dataList[11]);

            XNum = ncols;
            YNum = nrows;
            XMin = xllCorner;
            YMin = yllCorner;
            XDelt = cellSize;
            YDelt = cellSize;
            MissingValue = nodata_value;
            X = new double[XNum];
            for (i = 0; i < XNum; i++)
            {
                X[i] = XMin + i * XDelt;
            }
            if (X[XNum - 1] + XDelt - X[0] == 360)
                this.IsGlobal = true;

            Y = new double[YNum];
            for (i = 0; i < YNum; i++)
            {
                Y[i] = YMin + i * YDelt;
            }

            Dimension xDim = new Dimension(DimensionType.X);
            xDim.SetValues(X);
            this.XDimension = xDim;
            Dimension yDim = new Dimension(DimensionType.Y);
            yDim.SetValues(Y);
            this.YDimension = yDim;

            List<Variable> variables = new List<Variable>();
            Variable aVar = new Variable();
            aVar.Name = "var";
            aVar.SetDimension(xDim);
            aVar.SetDimension(yDim);
            variables.Add(aVar);
            this.Variables = variables;

            sr.Close();            
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
            GridData aGridData = new GridData();
            aGridData.Data = ReadData();
            aGridData.X = X;
            aGridData.Y = Y;           
            aGridData.MissingValue = MissingValue;

            return aGridData;
        }

        /// <summary>
        /// Read grid data
        /// </summary>
        /// <returns>grid data</returns>
        public double[,] ReadData()
        {
            double[,] gridData = new double[YNum, XNum];            
            StreamReader sr = new StreamReader(FileName);
            string[] dataArray;
            List<string> dataList = new List<string>();
            int LastNonEmpty, i, j;
            string aLine, wholeStr;

            for (i = 0; i < 6; i++)
                sr.ReadLine();

            aLine = sr.ReadLine().Trim();
            string aStr = aLine.Split()[0];

            wholeStr = sr.ReadToEnd();
            sr.Close();

            if (MIMath.IsNumeric(aStr))
                wholeStr = aLine + " " + wholeStr;

            dataArray = wholeStr.Split();
            LastNonEmpty = -1;
            for (i = 0; i < dataArray.Length; i++)
            {
                if (dataArray[i] != string.Empty)
                {
                    LastNonEmpty++;
                    dataList.Add(dataArray[i]);
                }
            }

            for (i = 0; i < YNum; i++)
            {
                for (j = 0; j < XNum; j++)
                    gridData[i, j] = double.Parse(dataList[i * XNum + j]);
            }

            double[,] newGridData = new double[YNum, XNum];
            for (i = 0; i < YNum; i++)
            {
                for (j = 0; j < XNum; j++)
                    newGridData[i, j] = gridData[YNum - 1 - i, j];
            }

            return newGridData;
        }

        /// <summary>
        /// Generate data info text
        /// </summary>        
        /// <returns>info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + FileName;
            dataInfo += Environment.NewLine + "Data Type: ASCII Grid";            
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
