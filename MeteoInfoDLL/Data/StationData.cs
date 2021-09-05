using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Global;
using MeteoInfoC.Layer;
using MeteoInfoC.Map;
using MeteoInfoC.Projections;
using MeteoInfoC.Shape;

namespace MeteoInfoC.Data
{
    /// <summary>
    /// Station data
    /// </summary>
    public class StationData
    {
        #region Variables
        /// <summary>
        /// station data: longitude, latitude, value
        /// </summary>
        public double[,] Data;
        /// <summary>
        /// Station identifer list
        /// </summary>
        public List<string> Stations;
        /// <summary>
        /// Data extent
        /// </summary>
        public Extent DataExtent;
        /// <summary>
        /// Undef data
        /// </summary>
        public double MissingValue;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public StationData()
        {
            Stations = new List<string>();
            DataExtent = new Extent();
            MissingValue = -9999;
        }

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="aStData">station data</param>
        public StationData(StationData aStData)
        {
            Stations = aStData.Stations;
            DataExtent = aStData.DataExtent;
            MissingValue = aStData.MissingValue;
            Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < StNum; i++)
            {
                Data[0, i] = aStData.Data[0, i];
                Data[1, i] = aStData.Data[1, i];
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get station number
        /// </summary>
        public int StNum
        {
            get { return Data.GetLength(1); }
        }

        /// <summary>
        /// Get X coodinates array
        /// </summary>
        public double[] X
        {
            get
            {
                double[] x = new double[StNum];
                for (int i = 0; i < StNum; i++)
                    x[i] = Data[0, i];

                return x;
            }
        }

        /// <summary>
        /// Get Y coodinates array
        /// </summary>
        public double[] Y
        {
            get
            {
                double[] y = new double[StNum];
                for (int i = 0; i < StNum; i++)
                    y[i] = Data[1, i];

                return y;
            }
        }

        #endregion

        #region Methods

        #region Operator
        /// <summary>
        /// Override operator + for two station data
        /// </summary>
        /// <param name="aStData">a station data</param>
        /// <param name="bStData">a station data</param>
        /// <returns>result station data</returns>
        public static StationData operator +(StationData aStData, StationData bStData)
        {
            if (!MIMath.IsExtentCross(aStData.DataExtent, bStData.DataExtent))
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
            for (int i = 0; i < aStData.Stations.Count; i++)
            {                
                aStid = aStData.Stations[i];
                if (aStid == "99999")
                    continue;

                double aValue = aStData.Data[2, i];
                if (aValue == aStData.MissingValue)
                {
                    //aValue = 0;
                    continue;
                }

                stIdx = bStData.Stations.IndexOf(aStid);                
                if (stIdx >= 0)
                {
                    double bValue = bStData.Data[2, stIdx];
                    if (bValue == bStData.MissingValue)
                    {
                        //bValue = 0;
                        continue;
                    }

                    cStData.Stations.Add(aStid);
                    double[] theData = new double[3];
                    theData[0] = aStData.Data[0, i];
                    theData[1] = aStData.Data[1, i];
                    theData[2] = aValue + bValue;
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
        /// Override operator + between a station data and a double data
        /// </summary>
        /// <param name="aStData">a station data</param>
        /// <param name="aData">a double data</param>
        /// <returns>result station data</returns>
        public static StationData operator +(StationData aStData, double aData)
        {            
            StationData cStData = new StationData();
            cStData.Stations = aStData.Stations;
            cStData.DataExtent = aStData.DataExtent;
            cStData.MissingValue = aStData.MissingValue;
            cStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];            
            for (int i = 0; i < aStData.Stations.Count; i++)
            {
                cStData.Data[0, i] = aStData.Data[0, i];
                cStData.Data[1, i] = aStData.Data[1, i];
                double aValue = aStData.Data[2, i];
                if (aValue == aStData.MissingValue)
                    cStData.Data[2, i] = aValue;
                else
                    cStData.Data[2, i] = aValue + aData;
            }            

            return cStData;
        }

        /// <summary>
        /// Override operator + between a double data and a station data
        /// </summary>
        /// <param name="aData">a double data</param>
        /// <param name="aStData">a station data</param>        
        /// <returns>result station data</returns>
        public static StationData operator +(double aData, StationData aStData)
        {
            return aStData + aData;
        }

        /// <summary>
        /// Override operator - for two station data
        /// </summary>
        /// <param name="aStData">a station data</param>
        /// <param name="bStData">a station data</param>
        /// <returns>result station data</returns>
        public static StationData operator -(StationData aStData, StationData bStData)
        {
            if (!MIMath.IsExtentCross(aStData.DataExtent, bStData.DataExtent))
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
            for (int i = 0; i < aStData.Stations.Count; i++)
            {
                aStid = aStData.Stations[i];
                if (aStid == "99999")
                    continue;

                double aValue = aStData.Data[2, i];
                if (aValue == aStData.MissingValue)
                {
                    //aValue = 0;
                    continue;
                }

                stIdx = bStData.Stations.IndexOf(aStid);
                if (stIdx >= 0)
                {
                    double bValue = bStData.Data[2, stIdx];
                    if (bValue == bStData.MissingValue)
                    {
                        //bValue = 0;
                        continue;
                    }

                    cStData.Stations.Add(aStid);
                    double[] theData = new double[3];
                    theData[0] = aStData.Data[0, i];
                    theData[1] = aStData.Data[1, i];
                    theData[2] = aValue - bValue;
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
        /// Override operator - between a station data and a double data
        /// </summary>
        /// <param name="aStData">a station data</param>
        /// <param name="aData">a double data</param>
        /// <returns>result station data</returns>
        public static StationData operator -(StationData aStData, double aData)
        {
            StationData cStData = new StationData(aStData);            
            for (int i = 0; i < aStData.Stations.Count; i++)
            {                
                double aValue = aStData.Data[2, i];
                if (MIMath.DoubleEquals(aValue, aStData.MissingValue))
                    cStData.Data[2, i] = aValue;
                else
                    cStData.Data[2, i] = aValue - aData;
            }

            return cStData;
        }

        /// <summary>
        /// Override operator - between a double data and a station data
        /// </summary>
        /// <param name="aData">a double data</param>
        /// <param name="aStData">a station data</param>        
        /// <returns>result station data</returns>
        public static StationData operator -(double aData, StationData aStData)
        {
            StationData cStData = new StationData(aStData);            
            for (int i = 0; i < aStData.Stations.Count; i++)
            {                
                double aValue = aStData.Data[2, i];
                if (MIMath.DoubleEquals(aValue, aStData.MissingValue))
                    cStData.Data[2, i] = aStData.MissingValue;
                else
                    cStData.Data[2, i] = aData - aValue;
            }

            return cStData;
        }

        /// <summary>
        /// Override operator * for two station data
        /// </summary>
        /// <param name="aStData">a station data</param>
        /// <param name="bStData">a station data</param>
        /// <returns>result station data</returns>
        public static StationData operator *(StationData aStData, StationData bStData)
        {
            if (!MIMath.IsExtentCross(aStData.DataExtent, bStData.DataExtent))
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
            for (int i = 0; i < aStData.Stations.Count; i++)
            {
                aStid = aStData.Stations[i];
                if (aStid == "99999")
                    continue;

                double aValue = aStData.Data[2, i];
                if (aValue == aStData.MissingValue)
                {
                    //aValue = 0;
                    continue;
                }

                stIdx = bStData.Stations.IndexOf(aStid);
                if (stIdx >= 0)
                {
                    double bValue = bStData.Data[2, stIdx];
                    if (bValue == bStData.MissingValue)
                    {
                        //bValue = 0;
                        continue;
                    }

                    cStData.Stations.Add(aStid);
                    double[] theData = new double[3];
                    theData[0] = aStData.Data[0, i];
                    theData[1] = aStData.Data[1, i];
                    theData[2] = aValue * bValue;
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
        /// Override operator * between a station data and a double data
        /// </summary>
        /// <param name="aStData">a station data</param>
        /// <param name="aData">a double data</param>
        /// <returns>result station data</returns>
        public static StationData operator *(StationData aStData, double aData)
        {
            StationData cStData = new StationData();
            cStData.Stations = aStData.Stations;
            cStData.DataExtent = aStData.DataExtent;
            cStData.MissingValue = aStData.MissingValue;
            cStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Stations.Count; i++)
            {
                cStData.Data[0, i] = aStData.Data[0, i];
                cStData.Data[1, i] = aStData.Data[1, i];
                double aValue = aStData.Data[2, i];
                if (aValue == aStData.MissingValue)
                    cStData.Data[2, i] = aValue;
                else
                    cStData.Data[2, i] = aValue * aData;
            }

            return cStData;
        }

        /// <summary>
        /// Override operator * between a double data and a station data
        /// </summary>
        /// <param name="aData">a double data</param>
        /// <param name="aStData">a station data</param>        
        /// <returns>result station data</returns>
        public static StationData operator *(double aData, StationData aStData)
        {
            return aStData * aData;
        }

        /// <summary>
        /// Override operator / for two station data
        /// </summary>
        /// <param name="aStData">a station data</param>
        /// <param name="bStData">a station data</param>
        /// <returns>result station data</returns>
        public static StationData operator /(StationData aStData, StationData bStData)
        {
            if (!MIMath.IsExtentCross(aStData.DataExtent, bStData.DataExtent))
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
            for (int i = 0; i < aStData.Stations.Count; i++)
            {
                aStid = aStData.Stations[i];
                if (aStid == "99999")
                    continue;

                double aValue = aStData.Data[2, i];
                if (aValue == aStData.MissingValue)
                {
                    //aValue = 0;
                    continue;
                }

                stIdx = bStData.Stations.IndexOf(aStid);
                if (stIdx >= 0)
                {
                    double bValue = bStData.Data[2, stIdx];
                    if (bValue == bStData.MissingValue)
                    {
                        //bValue = 0;
                        continue;
                    }

                    cStData.Stations.Add(aStid);
                    double[] theData = new double[3];
                    theData[0] = aStData.Data[0, i];
                    theData[1] = aStData.Data[1, i];
                    theData[2] = aValue / bValue;
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
        /// Override operator / between a station data and a double data
        /// </summary>
        /// <param name="aStData">a station data</param>
        /// <param name="aData">a double data</param>
        /// <returns>result station data</returns>
        public static StationData operator /(StationData aStData, double aData)
        {
            StationData cStData = new StationData();
            cStData.Stations = aStData.Stations;
            cStData.DataExtent = aStData.DataExtent;
            cStData.MissingValue = aStData.MissingValue;
            cStData.Data = new double[aStData.Data.GetLength(0), aStData.Data.GetLength(1)];
            for (int i = 0; i < aStData.Stations.Count; i++)
            {
                cStData.Data[0, i] = aStData.Data[0, i];
                cStData.Data[1, i] = aStData.Data[1, i];
                double aValue = aStData.Data[2, i];
                if (aValue == aStData.MissingValue)
                    cStData.Data[2, i] = aValue;
                else
                    cStData.Data[2, i] = aValue / aData;
            }

            return cStData;
        }

        /// <summary>
        /// Override operator / between a double data and a station data
        /// </summary>
        /// <param name="aData">a double data</param>
        /// <param name="aStData">a station data</param>        
        /// <returns>result station data</returns>
        public static StationData operator /(double aData, StationData aStData)
        {
            StationData cStData = new StationData(aStData);            
            for (int i = 0; i < aStData.Stations.Count; i++)
            {                
                double aValue = aStData.Data[2, i];
                if (MIMath.DoubleEquals(aValue, aStData.MissingValue))
                    cStData.Data[2, i] = aStData.MissingValue;
                else
                    cStData.Data[2, i] = aData / aValue;
            }

            return cStData;
        }

        /// <summary>
        /// Override operator > between a station data and a double data
        /// </summary>
        /// <param name="aStData">a station data</param>
        /// <param name="aData">a double data</param>        
        /// <returns>result station data</returns>
        public static StationData operator >(StationData aStData, double aData)
        {
            StationData cStData = new StationData(aStData);
            for (int i = 0; i < aStData.Stations.Count; i++)
            {
                double aValue = aStData.Data[2, i];
                if (MIMath.DoubleEquals(aValue, aStData.MissingValue))
                    cStData.Data[2, i] = aStData.MissingValue;
                else
                {
                    if (aValue > aData)
                        cStData.Data[2, i] = 1;
                    else
                        cStData.Data[2, i] = 0;
                }
            }

            return cStData;
        }

        /// <summary>
        /// Override operator >= between a station data and a double data
        /// </summary>
        /// <param name="aStData">a station data</param>
        /// <param name="aData">a double data</param>        
        /// <returns>result station data</returns>
        public static StationData operator >=(StationData aStData, double aData)
        {
            StationData cStData = new StationData(aStData);
            for (int i = 0; i < aStData.Stations.Count; i++)
            {
                double aValue = aStData.Data[2, i];
                if (MIMath.DoubleEquals(aValue, aStData.MissingValue))
                    cStData.Data[2, i] = aStData.MissingValue;
                else
                {
                    if (aValue >= aData)
                        cStData.Data[2, i] = 1;
                    else
                        cStData.Data[2, i] = 0;
                }
            }

            return cStData;
        }

        /// <summary>
        /// Override operator less than between a station data and a double data
        /// </summary>
        /// <param name="aStData">a station data</param>
        /// <param name="aData">a double data</param>        
        /// <returns>result station data</returns>
        public static StationData operator <(StationData aStData, double aData)
        {
            StationData cStData = new StationData(aStData);
            for (int i = 0; i < aStData.Stations.Count; i++)
            {
                double aValue = aStData.Data[2, i];
                if (MIMath.DoubleEquals(aValue, aStData.MissingValue))
                    cStData.Data[2, i] = aStData.MissingValue;
                else
                {
                    if (aValue < aData)
                        cStData.Data[2, i] = 1;
                    else
                        cStData.Data[2, i] = 0;
                }
            }

            return cStData;
        }

        /// <summary>
        /// Override operator less than or equal between a station data and a double data
        /// </summary>
        /// <param name="aStData">a station data</param>
        /// <param name="aData">a double data</param>        
        /// <returns>result station data</returns>
        public static StationData operator <=(StationData aStData, double aData)
        {
            StationData cStData = new StationData(aStData);
            for (int i = 0; i < aStData.Stations.Count; i++)
            {
                double aValue = aStData.Data[2, i];
                if (MIMath.DoubleEquals(aValue, aStData.MissingValue))
                    cStData.Data[2, i] = aStData.MissingValue;
                else
                {
                    if (aValue <= aData)
                        cStData.Data[2, i] = 1;
                    else
                        cStData.Data[2, i] = 0;
                }
            }

            return cStData;
        }

        #endregion

        #region Other
        /// <summary>
        /// Get minimum value
        /// </summary>
        /// <returns>minimum value</returns>
        public double GetMinValue()
        {
            double min = 0;
            int vdNum = 0;
            for (int i = 0; i < Data.GetLength(1); i++)
            {
                if (Math.Abs(Data[2, i] / MissingValue - 1) < 0.01)
                    continue;

                if (vdNum == 0)
                    min = Data[2, i];
                else
                {
                    if (min > Data[2, i])
                        min = Data[2, i];
                }
                vdNum += 1;
            }

            return min;
        }

        /// <summary>
        /// Get maximum value
        /// </summary>
        /// <returns>maximum value</returns>
        public double GetMaxValue()
        {
            double max = 0;
            int vdNum = 0;
            for (int i = 0; i < Data.GetLength(1); i++)
            {
                if (Math.Abs(Data[2, i] / MissingValue - 1) < 0.01)
                    continue;

                if (vdNum == 0)
                    max = Data[2, i];
                else
                {
                    if (max < Data[2, i])
                        max = Data[2, i];
                }
                vdNum += 1;
            }

            return max;
        }

        /// <summary>
        /// Get summary value
        /// </summary>
        /// <returns>summary value</returns>
        public double Sum()
        {
            double sum = 0;
            int vdNum = 0;
            for (int i = 0; i < Data.GetLength(1); i++)
            {
                if (Math.Abs(Data[2, i] / MissingValue - 1) < 0.01)
                    continue;

                sum += Data[2, i];                
                vdNum += 1;
            }

            return sum;
        }

        /// <summary>
        /// Get average value
        /// </summary>
        /// <returns>average value</returns>
        public double Average()
        {
            double sum = 0;
            int vdNum = 0;
            for (int i = 0; i < Data.GetLength(1); i++)
            {
                if (Math.Abs(Data[2, i] / MissingValue - 1) < 0.01)
                    continue;

                sum += Data[2, i];
                vdNum += 1;
            }

            return sum / vdNum;
        }

        /// <summary>
        /// Mask out station data with a polygon layer
        /// </summary>
        /// <param name="aPGS">maskout layer</param>
        /// <returns>station data</returns>
        public StationData Maskout(PolygonShape aPGS)
        {
            StationData cStData = new StationData();
            cStData.Stations = new List<string>();
            cStData.MissingValue = MissingValue;
            List<List<double>> dataList = new List<List<double>>();
            for (int i = 0; i < Stations.Count; i++)
            {
                if (Data[0, i] >= aPGS.Extent.minX && Data[0, i] <= aPGS.Extent.maxX &&
                    Data[1, i] >= aPGS.Extent.minY && Data[1, i] <= aPGS.Extent.maxY)
                {
                    if (Global.MIMath.PointInPolygon(aPGS, Data[0, i], Data[1, i]))
                    {
                        dataList.Add(new List<double> { Data[0, i], Data[1, i], Data[2, i] });
                        cStData.Stations.Add(Stations[i]);
                    }
                }
            }

            cStData.Data = new double[3, dataList.Count];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < dataList.Count; j++)
                {
                    cStData.Data[i, j] = dataList[j][i];
                }
            }
            cStData.DataExtent = Global.MIMath.GetExtent(cStData.Data);

            return cStData;
        }


        /// <summary>
        /// Mask out station data with a polygon layer
        /// </summary>
        /// <param name="maskLayer">maskout layer</param>
        /// <returns>station data</returns>
        public StationData Maskout(VectorLayer maskLayer)
        {
            StationData cStData = new StationData();
            cStData.Stations = new List<string>();            
            cStData.MissingValue = MissingValue;            
            List<List<double>> dataList = new List<List<double>>();
            for (int i = 0; i < Stations.Count; i++)
            {                
                if (Data[0, i] >= maskLayer.Extent.minX && Data[0, i] <= maskLayer.Extent.maxX &&
                    Data[1, i] >= maskLayer.Extent.minY && Data[1, i] <= maskLayer.Extent.maxY)
                {
                    if (Global.MIMath.PointInPolygonLayer(maskLayer, Data[0, i], Data[1, i]))
                    {
                        dataList.Add(new List<double> { Data[0, i], Data[1, i], Data[2, i] });
                        cStData.Stations.Add(Stations[i]);
                    }                               
                }
            }

            cStData.Data = new double[3, dataList.Count];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < dataList.Count; j++)
                {
                    cStData.Data[i, j] = dataList[j][i];
                }
            }
            cStData.DataExtent = Global.MIMath.GetExtent(cStData.Data);            

            return cStData;
        }

        /// <summary>
        /// Maskout station data by a polygon layer
        /// </summary>
        /// <param name="aMapView">MapView</param>
        /// <param name="layerName">layer name</param>
        /// <returns>station data</returns>
        public StationData Maskout(MapView aMapView, string layerName)
        {
            int aHnd = aMapView.GetLayerHandleFromName(layerName);
            VectorLayer aLayer = (VectorLayer)aMapView.GetLayerFromHandle(aHnd);
            return Maskout(aLayer);
        }

        /// <summary>
        /// Extract station data by extent
        /// </summary>
        /// <param name="sX">start X</param>
        /// <param name="eX">end X</param>
        /// <param name="sY">start Y</param>
        /// <param name="eY">end Y</param>
        /// <returns>station data</returns>
        public StationData Extract(double sX, double eX, double sY, double eY)
        {
            StationData cStData = new StationData();
            cStData.Stations = new List<string>();
            cStData.MissingValue = MissingValue;
            List<List<double>> dataList = new List<List<double>>();
            for (int i = 0; i < Stations.Count; i++)
            {
                if (Data[0, i] >= sX && Data[0, i] <= eX &&
                    Data[1, i] >= sY && Data[1, i] <= eY)
                {
                    dataList.Add(new List<double> { Data[0, i], Data[1, i], Data[2, i] });
                    cStData.Stations.Add(Stations[i]);
                }
            }

            cStData.Data = new double[3, dataList.Count];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < dataList.Count; j++)
                {
                    cStData.Data[i, j] = dataList[j][i];
                }
            }
            cStData.DataExtent = Global.MIMath.GetExtent(cStData.Data);

            return cStData;
        }

        /// <summary>
        /// Save as CSV data file
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <param name="fieldName">data field name</param>
        public void SaveAsCSVFile(string aFile, string fieldName)
        {
            SaveAsCSVFile(aFile, fieldName, false);
        }

        /// <summary>
        /// Save as CSV data file
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <param name="fieldName">data field name</param>
        /// <param name="saveUndefData">if save undefine data</param>
        public void SaveAsCSVFile(string aFile, string fieldName, bool saveUndefData)
        {
            StreamWriter sw = new StreamWriter(aFile, false, Encoding.Default);

            string aStr = "Stid,Latitude,Lontitude," + fieldName;                        
            sw.WriteLine(aStr);
            for (int i = 0; i < Data.GetLength(1); i++)
            {
                if (!saveUndefData)
                {
                    if (MIMath.DoubleEquals(Data[2, i], MissingValue))
                        continue;
                }

                aStr = Stations[i] + "," + Data[0, i].ToString() + "," + Data[1, i].ToString() + "," + Data[2, i].ToString();                
                sw.WriteLine(aStr);
            }

            sw.Close();
        }

        /// <summary>
        /// Project station data
        /// </summary>
        /// <param name="fromProj">from projection info</param>
        /// <param name="toProj">to projection info</param>
        /// <returns>projected station data</returns>
        public StationData Projection(ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            StationData pStData = new StationData();
            pStData.MissingValue = this.MissingValue;
            List<double[]> pointsXY = new List<double[]>();
            for (int i = 0; i < StNum; i++)
            {
                double[][] points = new double[1][];
                points[0] = new double[] { Data[0, i], Data[1, i] };
                try
                {
                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, points.Length);
                    pointsXY.Add(new double[] { points[0][0], points[0][1], Data[2, i] });
                    pStData.Stations.Add(this.Stations[i]);
                }
                catch { }
            }

            pStData.Data = new double[3, pointsXY.Count];
            for (int i = 0; i < pointsXY.Count; i++)
            {
                pStData.Data[0, i] = pointsXY[i][0];
                pStData.Data[1, i] = pointsXY[i][1];
                pStData.Data[2, i] = pointsXY[i][2];
            }
            pStData.UpdateExtent();

            return pStData;
        }

        /// <summary>
        /// Update data extent
        /// </summary>
        public void UpdateExtent()
        {
            double minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            double lon, lat;

            for (int i = 0; i < StNum; i++)
            {
                lon = Data[0, i];
                lat = Data[1, i];
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
            this.DataExtent = dataExtent;
        }

        #endregion

        #endregion
    }
}
