using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data
{
    /// <summary>
    /// Data math
    /// </summary>
    public static class DataMath
    {
        #region Operator
        /// <summary>
        /// Take add operator of two objects
        /// </summary>
        /// <param name="a">object a</param>
        /// <param name="b">object b</param>
        /// <returns>object result</returns>
        public static object Add(object a, object b)
        {
            if (a.GetType() == typeof(GridData))
            {
                if (b.GetType() == typeof(GridData))
                    return (GridData)a + (GridData)b;
                else
                    return (GridData)a + (double)b;
            }
            else if (a.GetType() == typeof(StationData))
            {
                if (b.GetType() == typeof(StationData))
                    return (StationData)a + (StationData)b;
                else
                    return (StationData)a + (double)b;
            }
            else
            {
                if (b.GetType() == typeof(GridData))
                    return (double)a + (GridData)b;
                else if (b.GetType() == typeof(StationData))
                    return (double)a + (StationData)b;
                else
                    return (double)a + (double)b;
            }
        }

        /// <summary>
        /// Take subtract operator of two objects
        /// </summary>
        /// <param name="a">object a</param>
        /// <param name="b">object b</param>
        /// <returns>object result</returns>
        public static object Subtract(object a, object b)
        {
            if (a.GetType() == typeof(GridData))
            {
                if (b.GetType() == typeof(GridData))
                    return (GridData)a - (GridData)b;
                else
                    return (GridData)a - (double)b;
            }
            else if (a.GetType() == typeof(StationData))
            {
                if (b.GetType() == typeof(StationData))
                    return (StationData)a - (StationData)b;
                else
                    return (StationData)a - (double)b;
            }
            else
            {
                if (b.GetType() == typeof(GridData))
                    return (double)a - (GridData)b;
                else if (b.GetType() == typeof(StationData))
                    return (double)a - (StationData)b;
                else
                    return (double)a - (double)b;
            }
        }

        /// <summary>
        /// Take multiple operator of two objects
        /// </summary>
        /// <param name="a">object a</param>
        /// <param name="b">object b</param>
        /// <returns>object result</returns>
        public static object Multiple(object a, object b)
        {
            if (a.GetType() == typeof(GridData))
            {
                if (b.GetType() == typeof(GridData))
                    return (GridData)a * (GridData)b;
                else
                    return (GridData)a * (double)b;
            }
            else if (a.GetType() == typeof(StationData))
            {
                if (b.GetType() == typeof(StationData))
                    return (StationData)a * (StationData)b;
                else
                    return (StationData)a * (double)b;
            }
            else
            {
                if (b.GetType() == typeof(GridData))
                    return (double)a * (GridData)b;
                else if (b.GetType() == typeof(StationData))
                    return (double)a * (StationData)b;
                else
                    return (double)a * (double)b;
            }
        }

        /// <summary>
        /// Take divide operator of two objects
        /// </summary>
        /// <param name="a">object a</param>
        /// <param name="b">object b</param>
        /// <returns>object result</returns>
        public static object Divide(object a, object b)
        {
            if (a.GetType() == typeof(GridData))
            {
                if (b.GetType() == typeof(GridData))
                    return (GridData)a / (GridData)b;
                else
                    return (GridData)a / (double)b;
            }
            else if (a.GetType() == typeof(StationData))
            {
                if (b.GetType() == typeof(StationData))
                    return (StationData)a / (StationData)b;
                else
                    return (StationData)a / (double)b;
            }
            else
            {
                if (b.GetType() == typeof(GridData))
                    return (double)a / (GridData)b;
                else if (b.GetType() == typeof(StationData))
                    return (double)a / (StationData)b;
                else
                    return (double)a / (double)b;
            }
        }

        #endregion

        #region Function
        /// <summary>
        /// Take absolute value of a grid data
        /// </summary>
        /// <param name="aGrid">a grid data</param>
        /// <returns>result grid data</returns>
        public static GridData Abs(GridData aGrid)
        {            
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;            

            GridData bGrid = new GridData();
            bGrid.X = aGrid.X;
            bGrid.Y = aGrid.Y;            
            bGrid.Data = new double[yNum, xNum];
            bGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(aGrid.Data[i, j] / aGrid.MissingValue - 1) < 0.01)
                        bGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        bGrid.Data[i, j] = Math.Abs(aGrid.Data[i, j]);
                }
            }

            return bGrid;
        }

        /// <summary>
        /// Get absolute value of a station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <returns>result station data</returns>
        public static StationData Abs(StationData aStData)
        {
            StationData bStData = new StationData();
            bStData.DataExtent = aStData.DataExtent;
            bStData.Stations = aStData.Stations;
            bStData.MissingValue = aStData.MissingValue;
            bStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Data.GetLength(1); i++)
            {
                bStData.Data[0, i] = aStData.Data[0, i];
                bStData.Data[1, i] = aStData.Data[1, i];
                if (aStData.Data[2, i] == aStData.MissingValue)
                    bStData.Data[2, i] = aStData.MissingValue;
                else
                    bStData.Data[2, i] = Math.Abs(aStData.Data[2, i]);
            }

            return bStData;
        }

        /// <summary>
        /// Take abstract value
        /// </summary>
        /// <param name="a">object a</param>        
        /// <returns>object</returns>
        public static object Abs(object a)
        {
            if (a.GetType() == typeof(GridData))
                return Abs((GridData)a);
            if (a.GetType() == typeof(StationData))
                return Abs((StationData)a);
            else
                return Math.Abs((double)a);
        }

        /// <summary>
        /// Take anti-Cosine value of a grid data
        /// </summary>
        /// <param name="aGrid">a grid data</param>
        /// <returns>result grid data</returns>
        public static GridData Acos(GridData aGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData bGrid = new GridData();
            bGrid.X = aGrid.X;
            bGrid.Y = aGrid.Y;           
            bGrid.Data = new double[yNum, xNum];
            bGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(aGrid.Data[i, j] / aGrid.MissingValue - 1) < 0.01)
                        bGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        bGrid.Data[i, j] = Math.Acos(aGrid.Data[i, j]);
                }
            }

            return bGrid;
        }

        /// <summary>
        /// Get anti-Cosine value of a station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <returns>result station data</returns>
        public static StationData Acos(StationData aStData)
        {
            StationData bStData = new StationData();
            bStData.DataExtent = aStData.DataExtent;
            bStData.Stations = aStData.Stations;
            bStData.MissingValue = aStData.MissingValue;
            bStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Data.GetLength(1); i++)
            {
                bStData.Data[0, i] = aStData.Data[0, i];
                bStData.Data[1, i] = aStData.Data[1, i];
                if (aStData.Data[2, i] == aStData.MissingValue)
                    bStData.Data[2, i] = aStData.MissingValue;
                else
                    bStData.Data[2, i] = Math.Acos(aStData.Data[2, i]);
            }

            return bStData;
        }

        /// <summary>
        /// Take anti-Cosine value
        /// </summary>
        /// <param name="a">object a</param>        
        /// <returns>object</returns>
        public static object Acos(object a)
        {
            if (a.GetType() == typeof(GridData))
                return Acos((GridData)a);
            if (a.GetType() == typeof(StationData))
                return Acos((StationData)a);
            else
                return Math.Acos((double)a);
        }

        /// <summary>
        /// Take Anti-Sine value of a grid data
        /// </summary>
        /// <param name="aGrid">a grid data</param>
        /// <returns>result grid data</returns>
        public static GridData Asin(GridData aGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData bGrid = new GridData();
            bGrid.X = aGrid.X;
            bGrid.Y = aGrid.Y;            
            bGrid.Data = new double[yNum, xNum];
            bGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(aGrid.Data[i, j] / aGrid.MissingValue - 1) < 0.01)
                        bGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        bGrid.Data[i, j] = Math.Asin(aGrid.Data[i, j]);
                }
            }

            return bGrid;
        }

        /// <summary>
        /// Get anti-Sine value of a station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <returns>result station data</returns>
        public static StationData Asin(StationData aStData)
        {
            StationData bStData = new StationData();
            bStData.DataExtent = aStData.DataExtent;
            bStData.Stations = aStData.Stations;
            bStData.MissingValue = aStData.MissingValue;
            bStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Data.GetLength(1); i++)
            {
                bStData.Data[0, i] = aStData.Data[0, i];
                bStData.Data[1, i] = aStData.Data[1, i];
                if (aStData.Data[2, i] == aStData.MissingValue)
                    bStData.Data[2, i] = aStData.MissingValue;
                else
                    bStData.Data[2, i] = Math.Asin(aStData.Data[2, i]);
            }

            return bStData;
        }

        /// <summary>
        /// Take anti-Sine value
        /// </summary>
        /// <param name="a">object a</param>        
        /// <returns>object</returns>
        public static object Asin(object a)
        {
            if (a.GetType() == typeof(GridData))
                return Asin((GridData)a);
            if (a.GetType() == typeof(StationData))
                return Asin((StationData)a);
            else
                return Math.Asin((double)a);
        }

        /// <summary>
        /// Take Anti-Tangent value of a grid data
        /// </summary>
        /// <param name="aGrid">a grid data</param>
        /// <returns>result grid data</returns>
        public static GridData Atan(GridData aGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData bGrid = new GridData();
            bGrid.X = aGrid.X;
            bGrid.Y = aGrid.Y;           
            bGrid.Data = new double[yNum, xNum];
            bGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(aGrid.Data[i, j] / aGrid.MissingValue - 1) < 0.01)
                        bGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        bGrid.Data[i, j] = Math.Atan(aGrid.Data[i, j]);
                }
            }

            return bGrid;
        }

        /// <summary>
        /// Get anti-Tangent value of a station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <returns>result station data</returns>
        public static StationData Atan(StationData aStData)
        {
            StationData bStData = new StationData();
            bStData.DataExtent = aStData.DataExtent;
            bStData.Stations = aStData.Stations;
            bStData.MissingValue = aStData.MissingValue;
            bStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Data.GetLength(1); i++)
            {
                bStData.Data[0, i] = aStData.Data[0, i];
                bStData.Data[1, i] = aStData.Data[1, i];
                if (aStData.Data[2, i] == aStData.MissingValue)
                    bStData.Data[2, i] = aStData.MissingValue;
                else
                    bStData.Data[2, i] = Math.Atan(aStData.Data[2, i]);
            }

            return bStData;
        }

        /// <summary>
        /// Take anti-Tangent value
        /// </summary>
        /// <param name="a">object a</param>        
        /// <returns>object</returns>
        public static object Atan(object a)
        {
            if (a.GetType() == typeof(GridData))
                return Atan((GridData)a);
            if (a.GetType() == typeof(StationData))
                return Atan((StationData)a);
            else
                return Math.Atan((double)a);
        }

        /// <summary>
        /// Take cosine value of a grid data
        /// </summary>
        /// <param name="aGrid">a grid data</param>
        /// <returns>result grid data</returns>
        public static GridData Cos(GridData aGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData bGrid = new GridData();
            bGrid.X = aGrid.X;
            bGrid.Y = aGrid.Y;            
            bGrid.Data = new double[yNum, xNum];
            bGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(aGrid.Data[i, j] / aGrid.MissingValue - 1) < 0.01)
                        bGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        bGrid.Data[i, j] = Math.Cos(aGrid.Data[i, j]);
                }
            }

            return bGrid;
        }

        /// <summary>
        /// Get Cosine value of a station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <returns>result station data</returns>
        public static StationData Cos(StationData aStData)
        {
            StationData bStData = new StationData();
            bStData.DataExtent = aStData.DataExtent;
            bStData.Stations = aStData.Stations;
            bStData.MissingValue = aStData.MissingValue;
            bStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Data.GetLength(1); i++)
            {
                bStData.Data[0, i] = aStData.Data[0, i];
                bStData.Data[1, i] = aStData.Data[1, i];
                if (aStData.Data[2, i] == aStData.MissingValue)
                    bStData.Data[2, i] = aStData.MissingValue;
                else
                    bStData.Data[2, i] = Math.Cos(aStData.Data[2, i]);
            }

            return bStData;
        }

        /// <summary>
        /// Take Cosine value
        /// </summary>
        /// <param name="a">object a</param>        
        /// <returns>object</returns>
        public static object Cos(object a)
        {
            if (a.GetType() == typeof(GridData))
                return Cos((GridData)a);
            if (a.GetType() == typeof(StationData))
                return Cos((StationData)a);
            else
                return Math.Cos((double)a);
        }

        /// <summary>
        /// Take sine value of a grid data
        /// </summary>
        /// <param name="aGrid">a grid data</param>
        /// <returns>result grid data</returns>
        public static GridData Sin(GridData aGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData bGrid = new GridData();
            bGrid.X = aGrid.X;
            bGrid.Y = aGrid.Y;            
            bGrid.Data = new double[yNum, xNum];
            bGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(aGrid.Data[i, j] / aGrid.MissingValue - 1) < 0.01)
                        bGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        bGrid.Data[i, j] = Math.Sin(aGrid.Data[i, j]);
                }
            }

            return bGrid;
        }

        /// <summary>
        /// Get Sine value of a station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <returns>result station data</returns>
        public static StationData Sin(StationData aStData)
        {
            StationData bStData = new StationData();
            bStData.DataExtent = aStData.DataExtent;
            bStData.Stations = aStData.Stations;
            bStData.MissingValue = aStData.MissingValue;
            bStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Data.GetLength(1); i++)
            {
                bStData.Data[0, i] = aStData.Data[0, i];
                bStData.Data[1, i] = aStData.Data[1, i];
                if (aStData.Data[2, i] == aStData.MissingValue)
                    bStData.Data[2, i] = aStData.MissingValue;
                else
                    bStData.Data[2, i] = Math.Sin(aStData.Data[2, i]);
            }

            return bStData;
        }

        /// <summary>
        /// Take Sine value
        /// </summary>
        /// <param name="a">object a</param>        
        /// <returns>object</returns>
        public static object Sin(object a)
        {
            if (a.GetType() == typeof(GridData))
                return Sin((GridData)a);
            if (a.GetType() == typeof(StationData))
                return Sin((StationData)a);
            else
                return Math.Sin((double)a);
        }

        /// <summary>
        /// Take tangent value of a grid data
        /// </summary>
        /// <param name="aGrid">a grid data</param>
        /// <returns>result grid data</returns>
        public static GridData Tan(GridData aGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData bGrid = new GridData();
            bGrid.X = aGrid.X;
            bGrid.Y = aGrid.Y;            
            bGrid.Data = new double[yNum, xNum];
            bGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(aGrid.Data[i, j] / aGrid.MissingValue - 1) < 0.01)
                        bGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        bGrid.Data[i, j] = Math.Tan(aGrid.Data[i, j]);
                }
            }

            return bGrid;
        }

        /// <summary>
        /// Get Tangent value of a station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <returns>result station data</returns>
        public static StationData Tan(StationData aStData)
        {
            StationData bStData = new StationData();
            bStData.DataExtent = aStData.DataExtent;
            bStData.Stations = aStData.Stations;
            bStData.MissingValue = aStData.MissingValue;
            bStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Data.GetLength(1); i++)
            {
                bStData.Data[0, i] = aStData.Data[0, i];
                bStData.Data[1, i] = aStData.Data[1, i];
                if (aStData.Data[2, i] == aStData.MissingValue)
                    bStData.Data[2, i] = aStData.MissingValue;
                else
                    bStData.Data[2, i] = Math.Tan(aStData.Data[2, i]);
            }

            return bStData;
        }

        /// <summary>
        /// Take Tangent value
        /// </summary>
        /// <param name="a">object a</param>        
        /// <returns>object</returns>
        public static object Tan(object a)
        {
            if (a.GetType() == typeof(GridData))
                return Tan((GridData)a);
            if (a.GetType() == typeof(StationData))
                return Tan((StationData)a);
            else
                return Math.Tan((double)a);
        }

        /// <summary>
        /// Take e raised specific power value of a grid data
        /// </summary>
        /// <param name="aGrid">a grid data</param>
        /// <returns>result grid data</returns>
        public static GridData Exp(GridData aGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData bGrid = new GridData();
            bGrid.X = aGrid.X;
            bGrid.Y = aGrid.Y;            
            bGrid.Data = new double[yNum, xNum];
            bGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(aGrid.Data[i, j] / aGrid.MissingValue - 1) < 0.01)
                        bGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        bGrid.Data[i, j] = Math.Exp(aGrid.Data[i, j]);
                }
            }

            return bGrid;
        }

        /// <summary>
        /// Take e raised specific power value of a station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <returns>result station data</returns>
        public static StationData Exp(StationData aStData)
        {
            StationData bStData = new StationData();
            bStData.DataExtent = aStData.DataExtent;
            bStData.Stations = aStData.Stations;
            bStData.MissingValue = aStData.MissingValue;
            bStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Data.GetLength(1); i++)
            {
                bStData.Data[0, i] = aStData.Data[0, i];
                bStData.Data[1, i] = aStData.Data[1, i];
                if (aStData.Data[2, i] == aStData.MissingValue)
                    bStData.Data[2, i] = aStData.MissingValue;
                else
                    bStData.Data[2, i] = Math.Exp(aStData.Data[2, i]);
            }

            return bStData;
        }

        /// <summary>
        /// Take Exponent value
        /// </summary>
        /// <param name="a">object a</param>        
        /// <returns>object</returns>
        public static object Exp(object a)
        {
            if (a.GetType() == typeof(GridData))
                return Exp((GridData)a);
            if (a.GetType() == typeof(StationData))
                return Exp((StationData)a);
            else
                return Math.Exp((double)a);
        }

        /// <summary>
        /// Take power value of a grid data
        /// </summary>
        /// <param name="aGrid">a grid data</param>
        /// <param name="p">power value</param>
        /// <returns>result grid data</returns>
        public static GridData Pow(GridData aGrid, double p)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData bGrid = new GridData();
            bGrid.X = aGrid.X;
            bGrid.Y = aGrid.Y;            
            bGrid.Data = new double[yNum, xNum];
            bGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(aGrid.Data[i, j] / aGrid.MissingValue - 1) < 0.01)
                        bGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        bGrid.Data[i, j] = Math.Pow(aGrid.Data[i, j], p);
                }
            }

            return bGrid;
        }

        /// <summary>
        /// Take power value of a station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <param name="p">power</param>
        /// <returns>result station data</returns>
        public static StationData Pow(StationData aStData, double p)
        {
            StationData bStData = new StationData();
            bStData.DataExtent = aStData.DataExtent;
            bStData.Stations = aStData.Stations;
            bStData.MissingValue = aStData.MissingValue;
            bStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Data.GetLength(1); i++)
            {
                bStData.Data[0, i] = aStData.Data[0, i];
                bStData.Data[1, i] = aStData.Data[1, i];
                if (aStData.Data[2, i] == aStData.MissingValue)
                    bStData.Data[2, i] = aStData.MissingValue;
                else
                    bStData.Data[2, i] = Math.Pow(aStData.Data[2, i], p);
            }

            return bStData;
        }

        /// <summary>
        /// Take power value
        /// </summary>
        /// <param name="a">object a</param>
        /// <param name="b">double b</param>
        /// <returns>object</returns>
        public static object Pow(object a, double b)
        {
            if (a.GetType() == typeof(GridData))
                return Pow((GridData)a, b);
            if (a.GetType() == typeof(StationData))
                return Pow((StationData)a, b);
            else
                return Math.Pow((double)a, b);
        }

        /// <summary>
        /// Take square root value of a grid data
        /// </summary>
        /// <param name="aGrid">a grid data</param>
        /// <returns>result grid data</returns>
        public static GridData Sqrt(GridData aGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData bGrid = new GridData();
            bGrid.X = aGrid.X;
            bGrid.Y = aGrid.Y;            
            bGrid.Data = new double[yNum, xNum];
            bGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(aGrid.Data[i, j] / aGrid.MissingValue - 1) < 0.01)
                        bGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        bGrid.Data[i, j] = Math.Sqrt(aGrid.Data[i, j]);
                }
            }

            return bGrid;
        }

        /// <summary>
        /// Take square root value of a station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <returns>result station data</returns>
        public static StationData Sqrt(StationData aStData)
        {
            StationData bStData = new StationData();
            bStData.DataExtent = aStData.DataExtent;
            bStData.Stations = aStData.Stations;
            bStData.MissingValue = aStData.MissingValue;
            bStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Data.GetLength(1); i++)
            {
                bStData.Data[0, i] = aStData.Data[0, i];
                bStData.Data[1, i] = aStData.Data[1, i];
                if (aStData.Data[2, i] == aStData.MissingValue)
                    bStData.Data[2, i] = aStData.MissingValue;
                else
                    bStData.Data[2, i] = Math.Sqrt(aStData.Data[2, i]);
            }

            return bStData;
        }

        /// <summary>
        /// Take square root value
        /// </summary>
        /// <param name="a">object a</param>        
        /// <returns>object</returns>
        public static object Sqrt(object a)
        {
            if (a.GetType() == typeof(GridData))
                return Sqrt((GridData)a);
            if (a.GetType() == typeof(StationData))
                return Sqrt((StationData)a);
            else
                return Math.Sqrt((double)a);
        }

        /// <summary>
        /// Take natural logarithm value of a grid data
        /// </summary>
        /// <param name="aGrid">a grid data</param>
        /// <returns>result grid data</returns>
        public static GridData Log(GridData aGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData bGrid = new GridData();
            bGrid.X = aGrid.X;
            bGrid.Y = aGrid.Y;            
            bGrid.Data = new double[yNum, xNum];
            bGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(aGrid.Data[i, j] / aGrid.MissingValue - 1) < 0.01)
                        bGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        bGrid.Data[i, j] = Math.Log(aGrid.Data[i, j]);
                }
            }

            return bGrid;
        }

        /// <summary>
        /// Take natural logarithm value of a station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <returns>result station data</returns>
        public static StationData Log(StationData aStData)
        {
            StationData bStData = new StationData();
            bStData.DataExtent = aStData.DataExtent;
            bStData.Stations = aStData.Stations;
            bStData.MissingValue = aStData.MissingValue;
            bStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Data.GetLength(1); i++)
            {
                bStData.Data[0, i] = aStData.Data[0, i];
                bStData.Data[1, i] = aStData.Data[1, i];
                if (aStData.Data[2, i] == aStData.MissingValue)
                    bStData.Data[2, i] = aStData.MissingValue;
                else
                    bStData.Data[2, i] = Math.Log(aStData.Data[2, i]);
            }

            return bStData;
        }

        /// <summary>
        /// Take natural logarithm value
        /// </summary>
        /// <param name="a">object a</param>        
        /// <returns>object</returns>
        public static object Log(object a)
        {
            if (a.GetType() == typeof(GridData))
                return Log((GridData)a);
            if (a.GetType() == typeof(StationData))
                return Log((StationData)a);
            else
                return Math.Log((double)a);
        }

        /// <summary>
        /// Take base 10 logarithm value of a grid data
        /// </summary>
        /// <param name="aGrid">a grid data</param>
        /// <returns>result grid data</returns>
        public static GridData Log10(GridData aGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData bGrid = new GridData();
            bGrid.X = aGrid.X;
            bGrid.Y = aGrid.Y;            
            bGrid.Data = new double[yNum, xNum];
            bGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(aGrid.Data[i, j] / aGrid.MissingValue - 1) < 0.01)
                        bGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        bGrid.Data[i, j] = Math.Log10(aGrid.Data[i, j]);
                }
            }

            return bGrid;
        }

        /// <summary>
        /// Take base 10 logarithm value of a station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <returns>result station data</returns>
        public static StationData Log10(StationData aStData)
        {
            StationData bStData = new StationData();
            bStData.DataExtent = aStData.DataExtent;
            bStData.Stations = aStData.Stations;
            bStData.MissingValue = aStData.MissingValue;
            bStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Data.GetLength(1); i++)
            {
                bStData.Data[0, i] = aStData.Data[0, i];
                bStData.Data[1, i] = aStData.Data[1, i];
                if (aStData.Data[2, i] == aStData.MissingValue)
                    bStData.Data[2, i] = aStData.MissingValue;
                else
                    bStData.Data[2, i] = Math.Log10(aStData.Data[2, i]);
            }

            return bStData;
        }

        /// <summary>
        /// Take base 10 logrithm value
        /// </summary>
        /// <param name="a">object a</param>        
        /// <returns>object</returns>
        public static object Log10(object a)
        {
            if (a.GetType() == typeof(GridData))
                return Log10((GridData)a);
            if (a.GetType() == typeof(StationData))
                return Log10((StationData)a);
            else
                return Math.Log10((double)a);
        }

        /// <summary>
        /// Take maximum grid data from two grid data
        /// </summary>
        /// <param name="aGrid">grid data</param>
        /// <param name="bGrid">grid data</param>
        /// <returns>maximum grid data</returns>
        public static GridData Maximum(GridData aGrid, GridData bGrid)
        {
            GridData cGrid = new GridData(aGrid);
            for (int i = 0; i < cGrid.YNum; i++)
            {
                for (int j = 0; j < cGrid.XNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue) ||
                        MIMath.DoubleEquals(bGrid.Data[i, j], bGrid.MissingValue))
                        cGrid.Data[i, j] = cGrid.MissingValue;
                    else
                    {
                        if (aGrid.Data[i, j] >= bGrid.Data[i, j])
                            cGrid.Data[i, j] = aGrid.Data[i, j];
                        else
                            cGrid.Data[i, j] = bGrid.Data[i, j];
                    }
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Take minimum grid data from two grid data
        /// </summary>
        /// <param name="aGrid">grid data</param>
        /// <param name="bGrid">grid data</param>
        /// <returns>minimum grid data</returns>
        public static GridData Minimum(GridData aGrid, GridData bGrid)
        {
            GridData cGrid = new GridData(aGrid);
            for (int i = 0; i < cGrid.YNum; i++)
            {
                for (int j = 0; j < cGrid.XNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue) ||
                        MIMath.DoubleEquals(bGrid.Data[i, j], bGrid.MissingValue))
                        cGrid.Data[i, j] = cGrid.MissingValue;
                    else
                    {
                        if (aGrid.Data[i, j] <= bGrid.Data[i, j])
                            cGrid.Data[i, j] = aGrid.Data[i, j];
                        else
                            cGrid.Data[i, j] = bGrid.Data[i, j];
                    }
                }
            }

            return cGrid;
        }

        #endregion

        #region more
        /// <summary>
        /// Take magnitude value from U/V data
        /// </summary>
        /// <param name="UData">U data</param>
        /// <param name="VData">V data</param>
        /// <returns>magnitude data</returns>
        public static GridData Magnitude(GridData UData, GridData VData)
        {
            int xNum = UData.XNum;
            int yNum = UData.YNum;
            //if (bGrid.XNum != xNum || bGrid.YNum != yNum)
            //    return null;

            GridData cGrid = new GridData();
            cGrid.X = UData.X;
            cGrid.Y = UData.Y;            
            cGrid.Data = new double[yNum, xNum];
            cGrid.MissingValue = UData.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (Math.Abs(UData.Data[i, j] / UData.MissingValue - 1) < 0.01 || 
                        Math.Abs(VData.Data[i, j] / VData.MissingValue - 1) < 0.01)
                        cGrid.Data[i, j] = UData.MissingValue;
                    else
                        cGrid.Data[i, j] = Math.Sqrt(UData.Data[i, j] * UData.Data[i, j] + VData.Data[i, j] * VData.Data[i, j]);
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Take magnitude value from U/V data
        /// </summary>
        /// <param name="UData">U data</param>
        /// <param name="VData">V data</param>
        /// <returns>magnitude data</returns>
        public static StationData Magnitude(StationData UData, StationData VData)
        {
            if (!MIMath.IsExtentCross(UData.DataExtent, VData.DataExtent))
                return null;

            StationData cStData = new StationData();
            List<double[]> cData = new List<double[]>();
            string aStid;
            int stIdx = -1;
            double minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            for (int i = 0; i < UData.Stations.Count; i++)
            {
                aStid = UData.Stations[i];
                if (aStid == "99999")
                    continue;

                double aValue = UData.Data[2, i];
                if (aValue == UData.MissingValue)
                {
                    //aValue = 0;
                    continue;
                }

                stIdx = VData.Stations.IndexOf(aStid);
                if (stIdx >= 0)
                {
                    double bValue = VData.Data[2, stIdx];
                    if (bValue == VData.MissingValue)
                    {
                        //bValue = 0;
                        continue;
                    }

                    cStData.Stations.Add(aStid);
                    double[] theData = new double[3];
                    theData[0] = UData.Data[0, i];
                    theData[1] = UData.Data[1, i];
                    theData[2] = Math.Sqrt(aValue * aValue + bValue * bValue);
                    cData.Add(theData);

                    if (cStData.Stations.Count == 1)
                    {
                        minX = theData[0];
                        maxX = minX;
                        minY = theData[1];
                        maxY = minY;
                    }
                    else
                    {
                        if (minX > theData[0])
                        {
                            minX = theData[0];
                        }
                        else if (maxX < theData[0])
                        {
                            maxX = theData[0];
                        }
                        if (minY > theData[1])
                        {
                            minY = theData[1];
                        }
                        else if (maxY < theData[1])
                        {
                            maxY = theData[1];
                        }
                    }
                }
            }
            cStData.DataExtent.minX = minX;
            cStData.DataExtent.maxX = maxX;
            cStData.DataExtent.minY = minY;
            cStData.DataExtent.maxY = maxY;
            cStData.Data = new double[3, cData.Count];
            for (int i = 0; i < cData.Count; i++)
            {
                cStData.Data[0, i] = cData[i][0];
                cStData.Data[1, i] = cData[i][1];
                cStData.Data[2, i] = cData[i][2];
            }

            return cStData;
        }

        /// <summary>
        /// Performs a centered difference operation on a grid data in the x or y direction
        /// </summary>
        /// <param name="aData">grid data</param>
        /// <param name="isX">if is x direction</param>
        /// <returns>result grid data</returns>
        public static GridData Cdiff(GridData aData, bool isX)
        {
            GridData bData = new GridData();
            bData.X = aData.X;
            bData.Y = aData.Y;            
            bData.MissingValue = aData.MissingValue;
            bData.Data = new double[aData.YNum, aData.XNum];
            for (int i = 0; i < aData.YNum; i++)
            {                
                for (int j = 0; j < aData.XNum; j++)
                {
                    if (i == 0 || i == aData.YNum - 1 || j == 0 || j == aData.XNum - 1)
                        bData.Data[i, j] = aData.MissingValue;
                    else
                    {
                        double a, b;
                        if (isX)
                        {
                            a = aData.Data[i, j + 1];
                            b = aData.Data[i, j - 1];
                            if (Math.Abs(a / aData.MissingValue - 1) < 0.01 || Math.Abs(b / aData.MissingValue - 1) < 0.01)
                                bData.Data[i, j] = aData.MissingValue;
                            else
                                bData.Data[i, j] = a - b;
                        }
                        else
                        {
                            a = aData.Data[i + 1, j];
                            b = aData.Data[i - 1, j];
                            if (Math.Abs(a / aData.MissingValue - 1) < 0.01 || Math.Abs(b / aData.MissingValue - 1) < 0.01)
                                bData.Data[i, j] = aData.MissingValue;
                            else
                                bData.Data[i, j] = a - b;
                        }
                    }
                }
            }

            return bData;
        }

        /// <summary>
        /// Calculates the vertical component of the curl (ie, vorticity) 
        /// </summary>
        /// <param name="UData">U component</param>
        /// <param name="VData">V component</param>
        /// <returns>curl</returns>
        public static GridData Hcurl(GridData UData, GridData VData)
        {            
            GridData lonData = new GridData(UData);
            GridData latData = new GridData(UData);
            int i, j;
            for (i = 0; i < UData.YNum; i++)
            {
                for (j = 0; j < UData.XNum; j++)
                {
                    lonData.Data[i, j] = UData.X[j];
                    latData.Data[i, j] = UData.Y[i];
                }
            }
            GridData dv = Cdiff(VData, true);
            GridData dx = Cdiff(lonData, true) * Math.PI / 180;
            GridData du = Cdiff(UData * Cos(latData * Math.PI / 180), false);
            GridData dy = Cdiff(latData, false) * Math.PI / 180;
            GridData gData = (dv / dx - du / dy) / (Cos(latData * Math.PI / 180) * 6.37e6);

            return gData;
        }

        /// <summary>
        /// Calculates the horizontal divergence using finite differencing 
        /// </summary>
        /// <param name="UData">U component</param>
        /// <param name="VData">V component</param>
        /// <returns>divergence</returns>
        public static GridData Hdivg(GridData UData, GridData VData)
        {
            GridData lonData = new GridData(UData);
            GridData latData = new GridData(UData);
            int i, j;
            for (i = 0; i < UData.YNum; i++)
            {
                for (j = 0; j < UData.XNum; j++)
                {
                    lonData.Data[i, j] = UData.X[j];
                    latData.Data[i, j] = UData.Y[i];
                }
            }
            GridData du = Cdiff(UData, true);
            GridData dx = Cdiff(lonData, true) * Math.PI / 180;
            GridData dv = Cdiff(VData * Cos(latData * Math.PI / 180), false);
            GridData dy = Cdiff(latData, false) * Math.PI / 180;
            GridData gData = (du / dx + dv / dy) / (Cos(latData * Math.PI / 180) * 6.37e6);

            return gData;
        }

        /// <summary>
        /// Calculate average grid data from grid data list
        /// </summary>
        /// <param name="gDataList">grid data list</param>
        /// <returns>average grid data</returns>
        public static GridData Average(List<GridData> gDataList)
        {
            GridData rData = new GridData();
            for (int i = 0; i < gDataList.Count; i++)
            {
                if (i == 0)
                    rData = gDataList[i];
                else
                    rData = rData + gDataList[i];
            }
            rData = rData / gDataList.Count;
            
            return rData;
        }

        /// <summary>
        /// Calculate average grid data from grid data list
        /// </summary>
        /// <param name="gDataList">grid data list</param>
        /// <param name="ignoreUndef">if ignore undefine data</param>
        /// <returns>average grid data</returns>
        public static GridData Average(List<GridData> gDataList, bool ignoreUndef)
        {
            if (ignoreUndef)
                return Average(gDataList);
            else
            {
                GridData rData = new GridData(gDataList[0]);
                GridData numData = new GridData(gDataList[0]);
                rData.SetValue(0);
                numData.SetValue(0);
                for (int d = 0; d < gDataList.Count; d++)
                {
                    GridData aGrid = gDataList[d];
                    for (int i = 0; i < rData.YNum; i++)
                    {
                        for (int j = 0; j < rData.XNum; j++)
                        {
                            if (!MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue))
                            {
                                rData.Data[i, j] = aGrid.Data[i, j] + rData.Data[i, j];
                                numData.Data[i, j] += 1;
                            }
                        }
                    }
                }

                for (int i = 0; i < rData.YNum; i++)
                {
                    for (int j = 0; j < rData.XNum; j++)
                    {
                        if (rData.Data[i, j] == 0)
                            rData.Data[i, j] = rData.MissingValue;
                    }
                }                
                rData = rData / numData;

                return rData;
            }
        }

        /// <summary>
        /// Calculate average grid data from grid data list
        /// </summary>
        /// <param name="gDataList">grid data list</param>
        /// <param name="ignoreUndef">if ignore undefine data</param>
        /// <param name="validNum">valid number</param>
        /// <returns>average grid data</returns>
        public static GridData Average(List<GridData> gDataList, bool ignoreUndef, int validNum)
        {
            if (ignoreUndef)
                return Average(gDataList);
            else
            {
                GridData rData = new GridData(gDataList[0]);
                GridData numData = new GridData(gDataList[0]);
                rData.SetValue(0);
                numData.SetValue(0);
                for (int d = 0; d < gDataList.Count; d++)
                {
                    GridData aGrid = gDataList[d];
                    for (int i = 0; i < rData.YNum; i++)
                    {
                        for (int j = 0; j < rData.XNum; j++)
                        {
                            if (!MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue))
                            {
                                rData.Data[i, j] = aGrid.Data[i, j] + rData.Data[i, j];
                                numData.Data[i, j] += 1;
                            }                            
                        }
                    }
                }

                for (int i = 0; i < rData.YNum; i++)
                {
                    for (int j = 0; j < rData.XNum; j++)
                    {
                        if (rData.Data[i, j] == 0)
                            rData.Data[i, j] = rData.MissingValue;
                    }
                }
                numData.ReplaceValue(validNum, 0, false);
                rData = rData / numData;

                return rData;
            }
        }

        /// <summary>
        /// Union grid data - latter grid will overlap ahead grid, the latter undefine data will not overlap ahead data.
        /// All grid data should have same X/Y coordinate value
        /// </summary>
        /// <param name="gDataList">grid data list</param>
        /// <returns>overlaped grid data</returns>
        public static GridData Union(List<GridData> gDataList)
        {
            GridData rData = new GridData();
            for (int i = 0; i < gDataList.Count; i++)
            {
                if (i == 0)
                    rData = gDataList[i];
                else
                    for (int m = 0; m < rData.YNum; m++)
                    {
                        for (int n = 0; n < rData.XNum; n++)
                        {
                            if (!MIMath.DoubleEquals(gDataList[i].Data[m, n], rData.MissingValue))
                                rData.Data[m, n] = gDataList[i].Data[m, n];
                        }
                    }
            }

            return rData;
        }

        #endregion

        #region Gaussian
        /// <summary>
        /// This function provides latitudes on a Gaussian grid from the
        /// number of latitude lines.
        /// </summary>
        /// <param name="nlat">the number of latitude lines</param>
        /// <returns>the latitudes of hemisphere</returns>
        public static object[] Gauss2Lats(int nlat)
        {
            double acon = 180.0 / Math.PI;

            // convergence criterion for iteration of cos latitude
            double xlim = 1.0e-7;

            // initialise arrays
            int i;
            //int iNum = 720;
            int iNum = nlat;
            double[] cosc = new double[iNum];
            double[] gwt = new double[iNum];
            double[] sinc = new double[iNum];
            double[] colat = new double[iNum];
            double[] wos2 = new double[iNum];
            for (i = 0; i < iNum; i++)
            {
                cosc[i] = 0.0;
                gwt[i] = 0.0;
                sinc[i] = 0.0;
                colat[i] = 0.0;
                wos2[i] = 0.0;
            }

            // the number of zeros between pole and equator
            int nzero = nlat / 2;

            // set first guess for cos(colat)
            for (i = 1; i <= nzero; i++)
                cosc[i -1] = Math.Sin((i - 0.5) * Math.PI / nlat + Math.PI * 0.5);

            // constants for determining the derivative of the polynomial
            int fi = nlat;
            double fi1 = fi + 1.0;
            double a = fi * fi1 / Math.Sqrt(4.0 * fi1 * fi1 - 1.0);
            double b = fi1 * fi / Math.Sqrt(4.0 * fi * fi - 1.0);

            //loop over latitudes, iterating the search for each root
            double c, d;
            for (i = 0; i < nzero; i++)
            {
                // determine the value of the ordinary Legendre polynomial for the current guess root
                double g = Gord(nlat, cosc[i]);
                // determine the derivative of the polynomial at this point
                double gm = Gord(nlat - 1, cosc[i]);
                double gp = Gord(nlat + 1, cosc[i]);
                double gt = (cosc[i] * cosc[i] - 1.0) / (a * gp - b * gm);
                // update the estimate of the root
                double delta = g * gt;
                cosc[i] = cosc[i] - delta;

                // if convergence criterion has not been met, keep trying
                while (Math.Abs(delta) > xlim)
                {
                    g = Gord(nlat, cosc[i]);
                    gm = Gord(nlat - 1, cosc[i]);
                    gp = Gord(nlat + 1, cosc[i]);
                    gt = (cosc[i] * cosc[i] - 1.0) / (a * gp - b * gm);
                    delta = g * gt;
                    cosc[i] = cosc[i] - delta;
                }
                // determine the Gaussian weights
                c = 2.0 * (1.0 - cosc[i] * cosc[i]);
                d = Gord(nlat - 1, cosc[i]);
                d = d * d * fi * fi;
                gwt[i] = c * (fi - 0.5) / d;
            }

            // determine the colatitudes and sin(colat) and weights over sin**2
            for (i = 0; i < nzero; i++)
            {
                colat[i] = Math.Acos(cosc[i]);
                sinc[i] = Math.Sin(colat[i]);
                wos2[i] = gwt[i] / (sinc[i] * sinc[i]);
            }

            // if nlat is odd, set values at the equator
            if (nlat % 2 != 0)
            {
                i = nzero;
                cosc[i] = 0.0;
                c = 2.0;
                d = Gord(nlat - 1, cosc[i]);
                d = d * d * fi * fi;
                gwt[i] = c * (fi - 0.5) / d;
                colat[i] = Math.PI * 0.5;
                sinc[i] = 1.0;
                wos2[i] = gwt[i];
            }

            // determine the southern hemisphere values by symmetry
            for (i = nlat - nzero; i < nlat; i++)
            {
                int j = nlat - i - 1;
                cosc[i] = -cosc[j];
                gwt[i] = gwt[j];
                colat[i] = Math.PI - colat[j];
                sinc[i] = sinc[j];
                wos2[i] = wos2[j];
            }

            double ylat = - 90.0;

            // calculate latitudes and latitude spacing
            double[] xlat = new double[nlat];
            double[] dlat = new double[nlat];
            for (i = 0; i < nzero; i++)
            {
                xlat[i] = - Math.Acos(sinc[i]) * acon;
                dlat[i] = xlat[i] - ylat;
                ylat = xlat[i];
            }

            if (nlat % 2 != 0)
            {
                i = nzero;
                xlat[i] = 0;
                dlat[i] = xlat[i] - ylat;
            }

            for (i = nlat - nzero; i < nlat; i++)
            {
                xlat[i] = Math.Acos(sinc[i]) * acon;
                dlat[i] = xlat[i] - ylat;
                ylat = xlat[i];
            }

            //// calculate latitudes and latitude spacing
            //double[] xlat = new double[nlat];
            //double[] dlat = new double[nlat];
            //for (i = 0; i < nlat; i++)
            //{
            //    xlat[i] = Math.Acos(sinc[i]) * acon;
            //    dlat[i] = xlat[i] - ylat;
            //    ylat = xlat[i];
            //}

            return new object[] { xlat, dlat };
        }

        /// <summary>
        /// Calculates the value of an ordinary Legendre polynomial at a latitude
        /// </summary>
        /// <param name="n">the degree of the polynomial</param>
        /// <param name="x">cos(colatitude)</param>
        /// <returns>the value of the Legendre polynomial of degree n at  latitude asin(x)</returns>
        private static double Gord(int n, double x)
        {
            //determine the colatitude
            double colat = Math.Acos(x);
            double c1 = Math.Sqrt(2.0);

            for (int i = 1; i <= n; i++)
            {
                c1 = c1 * Math.Sqrt(1.0 - 1.0 / (4 * i * i));
            }

            int fn = n;
            double ang = fn * colat;
            double s1 = 0.0;
            double c4 = 1.0;
            double a = -1.0;
            double b = 0.0;

            for (int k = 0; k <= n; k = k + 2)
            {
                if (k == n)
                    c4 = 0.5 * c4;

                s1 = s1 + c4 * Math.Cos(ang);
                a = a + 2.0;
                b = b + 1.0;
                int fk = k;
                ang = colat * (fn - fk - 2.0);
                c4 = (a * (fn - b + 1.0) / (b * (fn + fn - a))) * c4;
            }
            return s1 * c1;
        }

        #endregion

        #region Wind UV
        private static void GetUVFromDS(double windDir, double windSpeed, ref double U, ref double V)
        {
            double dir = windDir + 180;
            if (dir > 360)
                dir = dir - 360;

            dir = dir * Math.PI / 180;
            U = windSpeed * Math.Sin(dir);
            V = windSpeed * Math.Cos(dir);
        }

        private static void GetDSFromUV(double U, double V, ref double windDir, ref double windSpeed)
        {            
            windSpeed = Math.Sqrt(U * U + V * V);
            if (windSpeed == 0)
                windDir = 0;
            else
            {
                windDir = Math.Asin(U / windSpeed) * 180 / Math.PI;
                if (U < 0 && V < 0)
                {
                    windDir = 180.0 - windDir;
                }
                else if (U > 0 && V < 0)
                {
                    windDir = 180.0 - windDir;
                }
                else if (U < 0 && V > 0)
                {
                    windDir = 360.0 + windDir;
                }
                windDir += 180;
                if (windDir >= 360)
                    windDir -= 360;
            }
        }

        /// <summary>
        /// Get wind U/V grid data from wind direction/speed grid data
        /// </summary>
        /// <param name="windDirData">wind direction data</param>
        /// <param name="windSpeedData">wind speed data</param>
        /// <param name="uData">ref U data</param>
        /// <param name="vData">ref V data</param>
        public static void GetUVFromDS(GridData windDirData, GridData windSpeedData, ref GridData uData, ref GridData vData)
        {
            uData = new GridData(windDirData);
            vData = new GridData(windDirData);
            for (int i = 0; i < windDirData.YNum; i++)
            {
                for (int j = 0; j < windDirData.XNum; j++)
                {
                    if (MIMath.DoubleEquals(windDirData.Data[i, j], windDirData.MissingValue) ||
                        MIMath.DoubleEquals(windSpeedData.Data[i, j], windSpeedData.MissingValue))
                    {
                        uData.Data[i, j] = uData.MissingValue;
                        vData.Data[i, j] = vData.MissingValue;
                    }
                    else
                        GetUVFromDS(windDirData.Data[i, j], windSpeedData.Data[i, j], ref uData.Data[i, j],
                                ref vData.Data[i, j]);
                }
            }
        }

        /// <summary>
        /// Get wind U/V station data from wind direction/speed station data
        /// </summary>
        /// <param name="windDirData">wind direction data</param>
        /// <param name="windSpeedData">wind speed data</param>
        /// <param name="uData">ref U data</param>
        /// <param name="vData">ref V data</param>
        public static void GetUVFromDS(StationData windDirData, StationData windSpeedData, 
            ref StationData uData, ref StationData vData)
        {
            uData = new StationData(windDirData);
            vData = new StationData(windSpeedData);
            for (int i = 0; i < windDirData.StNum; i++)
            {
                if (MIMath.DoubleEquals(windDirData.Data[2, i], windDirData.MissingValue) ||
                    MIMath.DoubleEquals(windSpeedData.Data[2, i], windSpeedData.MissingValue))
                {
                    uData.Data[2, i] = uData.MissingValue;
                    vData.Data[2, i] = vData.MissingValue;
                }
                else
                    GetUVFromDS(windDirData.Data[2, i], windSpeedData.Data[2, i], ref uData.Data[2, i],
                        ref vData.Data[2, i]);
            }
        }

        /// <summary>
        /// Get wind direction/speed grid data from wind U/V grid data
        /// </summary>
        /// <param name="uData">U data</param>
        /// <param name="vData">V data</param>
        /// <param name="windDirData">ref wind direction data</param>
        /// <param name="windSpeedData">ref wind speed data</param>
        public static void GetDSFromUV(GridData uData, GridData vData, ref GridData windDirData, ref GridData windSpeedData)
        {
            windDirData = new GridData(uData);
            windSpeedData = new GridData(uData);
            for (int i = 0; i < uData.YNum; i++)
            {
                for (int j = 0; j < uData.XNum; j++)
                {
                    if (MIMath.DoubleEquals(uData.Data[i, j], uData.MissingValue) ||
                        MIMath.DoubleEquals(vData.Data[i, j], vData.MissingValue))
                    {
                        windDirData.Data[i, j] = windDirData.MissingValue;
                        windSpeedData.Data[i, j] = windSpeedData.MissingValue;
                    }
                    else
                        GetDSFromUV(uData.Data[i, j], vData.Data[i, j], ref windDirData.Data[i, j],
                                ref windSpeedData.Data[i, j]);
                }
            }
        }

        /// <summary>
        /// Get wind direction/speed station data from wind U/V station data
        /// </summary>
        /// <param name="uData">U data</param>
        /// <param name="vData">V data</param>
        /// <param name="windDirData">ref wind direciton data</param>
        /// <param name="windSpeedData">ref wind speed data</param>
        public static void GetDSFromUV(StationData uData, StationData vData,
            ref StationData windDirData, ref StationData windSpeedData)
        {
            windDirData = new StationData(uData);
            windSpeedData = new StationData(vData);
            for (int i = 0; i < windDirData.StNum; i++)
            {
                if (MIMath.DoubleEquals(uData.Data[2, i], uData.MissingValue) ||
                    MIMath.DoubleEquals(vData.Data[2, i], vData.MissingValue))
                {
                    windDirData.Data[2, i] = windDirData.MissingValue;
                    windSpeedData.Data[2, i] = windSpeedData.MissingValue;
                }
                else
                    GetDSFromUV(uData.Data[2, i], vData.Data[2, i], ref windDirData.Data[2, i],
                        ref windSpeedData.Data[2, i]);
            }
        }

        #endregion

        #region Statistics
       
        /// <summary>
        /// Determine the least square trend equation - linear fitting
        /// </summary>
        /// <param name="xData"> X data array</param>
        /// <param name="yData">Y data array</param>
        /// <returns>array - y intercept and slope</returns>
        public static double[] leastSquareTrend(double[] xData, double[] yData)
        {
            int n = xData.Length;
            double sumX = 0.0;
            double sumY = 0.0;
            double sumSquareX = 0.0;
            double sumXY = 0.0;
            for (int i = 0; i < n; i++)
            {
                sumX += xData[i];
                sumY += yData[i];
                sumSquareX += xData[i] * xData[i];
                sumXY += xData[i] * yData[i];
            }

            double a = (sumSquareX * sumY - sumX * sumXY) / (n * sumSquareX - sumX * sumX);
            double b = (n * sumXY - sumX * sumY) / (n * sumSquareX - sumX * sumX);

            return new double[] { a, b };
        }

        /// <summary>
        /// Determine the least square trend equation - linear fitting
        /// </summary>
        /// <param name="dataList">Grid data list</param>
        /// <param name="xData">X data array</param>
        /// <returns>Result grid data - slop</returns>
        public static GridData leastSquareTrend(List<GridData> dataList, double[] xData)
        {
            int n = dataList.Count;
            double[] yData = new double[n];
            double missingValue = dataList[0].MissingValue;
            GridData rData = new GridData(dataList[0]);
            rData.SetValue(missingValue);
            double value;
            bool ifcal;
            for (int i = 0; i < dataList[0].YNum; i++)
            {
                for (int j = 0; j < dataList[0].XNum; j++)
                {
                    ifcal = true;
                    for (int d = 0; d < n; d++)
                    {
                        value = dataList[d].Data[i,j];
                        if (MIMath.DoubleEquals(value, dataList[d].MissingValue))
                        {
                            ifcal = false;
                            break;
                        }
                        yData[d] = dataList[d].Data[i, j];
                    }
                    if (ifcal)
                    {
                        rData.Data[i, j] = leastSquareTrend(xData, yData)[1];
                    }
                }
            }

            return rData;
        }
        
        /// <summary>
        /// Mann-Kendall trend statistics
        /// </summary>
        /// <param name="ts">Input data array</param>
        /// <returns>Result array - z (trend)/beta (change value per unit time)</returns>
        public static double[] mann_Kendall_Trend(double[] ts)
        {
            int i, j, s = 0, k = 0;
            int n = ts.Length;
            double[] differ = new double[n * (n - 1) / 2];
            double z, beta;

            //Calculate z
            for (i = 0; i < n - 1; i++)
            {
                for (j = i + 1; j < n; j++)
                {
                    if (ts[j] > ts[i])
                    {
                        s = s + 1;
                    }
                    else if (ts[j] < ts[i])
                    {
                        s = s - 1;
                    }
                    differ[k] = (ts[j] - ts[i]) / (j - i);
                    k += 1;
                }
            }

            double var = n * (n - 1) * (2 * n + 5) / 18;

            if (s > 0)
            {
                z = (double)(s - 1) / Math.Sqrt(var);
            }
            else if (s == 0)
            {
                z = 0;
            }
            else
            {
                z = (double)(s + 1) / Math.Sqrt(var);
            }

            //Calculate beta
            Array.Sort(differ);
            if (k % 2 == 0)
            {
                beta = (differ[k / 2] + differ[k / 2 + 1]) / 2;
            }
            else
            {
                beta = differ[k / 2 + 1];
            }

            return new double[] { z, beta };
        }

        /// <summary>
        /// Mann-Kendall trend statistics
        /// </summary>
        /// <param name="ts">Input data array</param>
        /// <returns>Result array - z (trend)/beta (change value per unit time)</returns>
        public static double[] mann_Kendall_Trend_1(double[] ts)
        {
            int i, j, s = 0, k = 0;
            int n = ts.Length;
            int[] p = new int[n - 1];
            int psum = 0;
            double[] differ = new double[n * (n - 1) / 2];
            double beta;

            //Calculate z
            for (i = 0; i < n - 1; i++)
            {
                s = 0;
                for (j = i + 1; j < n; j++)
                {
                    if (ts[j] > ts[i])
                    {
                        s = s + 1;
                    }
                    differ[k] = (ts[j] - ts[i]) / (j - i);
                    k += 1;
                }
                p[i] = s;
                psum += s;
            }

            double t = 4 * psum / (n * (n - 1)) - 1;
            double var = 2 * (2 * n + 5) / (9 * n * (n - 1));

            double u = t / Math.Sqrt(var);

            //Calculate beta
            Array.Sort(differ);
            if (k % 2 == 0)
            {
                beta = (differ[k / 2] + differ[k / 2 + 1]) / 2;
            }
            else
            {
                beta = differ[k / 2 + 1];
            }

            return new double[] { u, beta };
        }

        #endregion

        #region Others
        ///// <summary>
        ///// Judge if a data is undefine
        ///// </summary>
        ///// <param name="aData">data</param>
        ///// <param name="MissingValue">undefine value</param>
        ///// <returns>Is or not undefine data</returns>
        //public static bool IsUndefineData(double aData, double MissingValue)
        //{
        //    if (Math.Abs(aData / MissingValue - 1) < 0.01)
        //        return true;
        //    else
        //        return false;
        //}

        #endregion
    }
}
