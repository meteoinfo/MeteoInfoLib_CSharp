using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Data.MeteoData;
using MeteoInfoC.Global;
using MeteoInfoC.Data;

namespace MeteoInfoC.Drawing
{
    /// <summary>
    /// Contour draw
    /// </summary>
    public static class ContourDraw
    {
        //private static List<wContour.PointD> _cutPlist = new List<wContour.PointD>();

        /// <summary>
        /// Get if has undefine data
        /// </summary>
        /// <param name="S">data array</param>
        /// <param name="undefine">undefine data</param>        
        /// <returns>if has undefine data</returns>
        public static bool GetHasUndefineData(double[,] S, double undefine)
        {
            int i, j;
            bool isNodata = false;
            bool ifBreak = false;

            for (i = 0; i < S.GetLength(0); i++)
            {
                for (j = 0; j < S.GetLength(1); j++)
                {
                    if (Math.Abs(S[i, j] / undefine - 1) < 0.01)
                    {
                        isNodata = true;
                        ifBreak = true;
                        break;
                    }                    
                }
                if (ifBreak)
                    break;
            }

            return isNodata;
        }

        /// <summary>
        /// Get if has undefine data
        /// </summary>
        /// <param name="gridData">grid data</param>          
        /// <returns>if has undefine data</returns>
        public static bool GetHasUndefineData(GridData gridData)
        {
            int i, j;
            bool isNodata = false;
            bool ifBreak = false;

            for (i = 0; i < gridData.YNum; i++)
            {
                for (j = 0; j < gridData.XNum; j++)
                {
                    if (Math.Abs(gridData.Data[i, j] / gridData.MissingValue - 1) < 0.01)
                    {
                        isNodata = true;
                        ifBreak = true;
                        break;
                    }
                }
                if (ifBreak)
                    break;
            }

            return isNodata;
        }

        /// <summary>
        /// Get maximum and miminum value from grid data
        /// </summary>
        /// <param name="S"></param>
        /// <param name="noData"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Boolean GetMaxMinValue(double[,] S, double noData, ref double min, ref double max)
        {
            int i, j, validNum;
            Boolean isNodata = false;

            validNum = 0;
            for (i = 0; i < S.GetLength(0); i++)
            {
                for (j = 0; j < S.GetLength(1); j++)
                {
                    if (!(Math.Abs(S[i, j] / noData - 1) < 0.01))
                    {
                        validNum++;
                        if (validNum == 1)
                        {
                            min = S[i, j];
                            max = min;
                        }
                        else
                        {
                            if (S[i, j] < min)
                            {
                                min = S[i, j];
                            }
                            if (S[i, j] > max)
                            {
                                max = S[i, j];
                            }
                        }
                    }
                    else
                    {
                        isNodata = true;
                    }
                }
            }

            return isNodata;
        }

        /// <summary>
        /// Get maximum and minimum value from discrete data
        /// </summary>
        /// <param name="S"></param>
        /// <param name="noData"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Boolean GetMaxMinValueFDiscreteData(double[,] S, double noData, ref double min, ref double max)
        {
            int i, validNum;
            Boolean isNodata = false;

            validNum = 0;
            for (i = 0; i < S.GetLength(1); i++)
            {
                if (!(Math.Abs(S[2, i] / noData - 1) < 0.01))
                {
                    validNum++;
                    if (validNum == 1)
                    {
                        min = S[2, i];
                        max = min;
                    }
                    else
                    {
                        if (S[2, i] < min)
                        {
                            min = S[2, i];
                        }
                        if (S[2, i] > max)
                        {
                            max = S[2, i];
                        }
                    }
                }
                else
                {
                    isNodata = true;
                }

            }

            return isNodata;
        }

        /// <summary>
        /// Create grid X/y coordinate from discrete data
        /// </summary>
        /// <param name="aGDP"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public static void CreateGridXY(GridDataSetting aGDP, ref double[] X, ref double[] Y)
        {
            double XDelt, YDelt;

            X = new double[aGDP.XNum];
            Y = new double[aGDP.YNum];
            XDelt = (aGDP.DataExtent.maxX - aGDP.DataExtent.minX) / Convert.ToDouble(aGDP.XNum - 1);
            YDelt = (aGDP.DataExtent.maxY - aGDP.DataExtent.minY) / Convert.ToDouble(aGDP.YNum - 1);

            //m_Contour.CreateGridXY_Num(Xlb, Ylb, Xrt, Yrt, cols - 1, rows - 1, ref m_X, ref m_Y);
            wContour.Interpolate.CreateGridXY_Delt(aGDP.DataExtent.minX, aGDP.DataExtent.minY,
                aGDP.DataExtent.maxX, aGDP.DataExtent.maxY, XDelt, YDelt, ref X, ref Y);
        }

        /// <summary>
        /// Filter discrete data with radius
        /// </summary>
        /// <param name="S"></param>
        /// <param name="radius"></param>
        /// <param name="aExtent"></param>
        /// <param name="unDef"></param>
        /// <returns></returns>
        public static double[,] FilterDiscreteData_Radius(double[,] S, double radius, Extent aExtent, double unDef)
        {
            double[,] discretedData;
            ArrayList disDataList = new ArrayList();
            int i;
            for (i = 0; i < S.GetLength(1); i++)
            {
                if (Math.Abs(S[2, i] / unDef - 1) < 0.01)
                {
                    continue;
                }
                if (S[0, i] + radius < aExtent.minX || S[0, i] - radius > aExtent.maxX ||
                    S[1, i] + radius < aExtent.minY || S[1, i] - radius > aExtent.maxY)
                {
                    continue;
                }
                else
                {
                    disDataList.Add(new double[] { S[0, i], S[1, i], S[2, i] });
                }
            }

            discretedData = new double[3, disDataList.Count];
            i = 0;
            foreach (double[] disData in disDataList)
            {
                discretedData[0, i] = disData[0];
                discretedData[1, i] = disData[1];
                discretedData[2, i] = disData[2];
                i += 1;
            }

            return discretedData;
        }

        /// <summary>
        /// Interpolate discrete data by IDW neighous method
        /// </summary>
        /// <param name="S">discrete data</param>
        /// <param name="X">X array</param>
        /// <param name="Y">Y array</param>
        /// <param name="pNum">minimum point number</param>
        /// <param name="unDefData">Undefine data</param>
        /// <returns>grid array</returns>
        public static GridData InterpolateDiscreteData_Neighbor(double[,] S, double[] X, double[] Y, int pNum, double unDefData)
        {
            double[,] dataArray;
            int rows, cols;

            rows = Y.Length;
            cols = X.Length;
            dataArray = new double[rows, cols];
            dataArray = (double[,])wContour.Interpolate.Interpolation_IDW_Neighbor(S, X, Y, pNum, unDefData);            

            GridData gridData = new GridData();
            gridData.Data = dataArray;
            gridData.MissingValue = unDefData;
            gridData.X = X;
            gridData.Y = Y;            

            return gridData;
        }

        /// <summary>
        /// Interpolate discrete data by IDW radius method
        /// </summary>
        /// <param name="S">discrete data</param>
        /// <param name="X">X array</param>
        /// <param name="Y">Y array</param>
        /// <param name="minPNum">mininum point number</param>
        /// <param name="radius">radius</param>
        /// <param name="unDefData">undefine data</param>
        /// <returns>grid data</returns>
        public static GridData InterpolateDiscreteData_Radius(double[,] S, double[] X, double[] Y,
            int minPNum, double radius, double unDefData)
        {
            double[,] dataArray;
            int rows, cols;

            rows = Y.Length;
            cols = X.Length;
            dataArray = new double[rows, cols];
            dataArray = (double[,])wContour.Interpolate.Interpolation_IDW_Radius(S, X, Y, minPNum, radius, unDefData);

            GridData gridData = new GridData();
            gridData.Data = dataArray;
            gridData.MissingValue = unDefData;
            gridData.X = X;
            gridData.Y = Y;            

            return gridData;
        }

        /// <summary>
        /// Interpolate discrete data by cressman method
        /// </summary>
        /// <param name="S">discrete data</param>
        /// <param name="X">X array</param>
        /// <param name="Y">Y array</param>               
        /// <param name="unDefData">undefine data</param>
        /// <param name="radList">radius list</param>
        /// <returns>grid data</returns>
        public static GridData InterpolateDiscreteData_Cressman(double[,] S, double[] X, double[] Y,
            double unDefData, List<double> radList)
        {
            double[,] dataArray;
            int rows, cols;

            rows = Y.Length;
            cols = X.Length;
            dataArray = new double[rows, cols];
            dataArray = wContour.Interpolate.Cressman(S, X, Y, unDefData, radList);

            GridData gridData = new GridData();
            gridData.Data = dataArray;
            gridData.MissingValue = unDefData;
            gridData.X = X;
            gridData.Y = Y;            

            return gridData;
        }

        /// <summary>
        /// Interpolate discrete data to Grid data
        /// </summary>
        /// <param name="S">discrete data</param>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <param name="unDefData">undefined value</param>
        /// <returns>grid data</returns>
        public static GridData AssignPointToGrid(double[,] S, double[] X, double[] Y, double unDefData)
        {
            double[,] dataArray = wContour.Interpolate.AssignPointToGrid(S, X, Y, unDefData);

            GridData gridData = new GridData();
            gridData.Data = dataArray;
            gridData.MissingValue = unDefData;
            gridData.X = X;
            gridData.Y = Y;

            return gridData;
        }

        /// <summary>
        /// Interpolate grid data
        /// </summary>
        /// <param name="aGridData">origin grid data</param>
        /// <returns>result grid data</returns>
        public static GridData Interpolate_Grid(GridData aGridData)
        {
            GridData bGridData = new GridData();
            bGridData.Data = wContour.Interpolate.Interpolation_Grid(aGridData.Data, aGridData.X, aGridData.Y,
                aGridData.MissingValue, ref bGridData.X, ref bGridData.Y);

            return bGridData;
        }

        ///// <summary>
        ///// Tracing contour lines
        ///// </summary>
        ///// <param name="gridData"></param>
        ///// <param name="cValues"></param>
        ///// <param name="X"></param>
        ///// <param name="Y"></param>
        ///// <returns></returns>
        //public static List<wContour.PolyLine> TracingContourLines(double[,] gridData, double[] cValues, double[] X, double[] Y)
        //{
        //    int nc;
        //    nc = cValues.Length;

        //    Double XDelt, YDelt;
        //    XDelt = X[1] - X[0];
        //    YDelt = Y[1] - Y[0];
        //    return wContour.Contour.CreateContourLines(gridData, X, Y, nc, cValues, XDelt, YDelt);
        //}

        /// <summary>
        /// Tracing contour lines with undefine data
        /// </summary>
        /// <param name="gridData"></param>
        /// <param name="cValues"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="noData"></param>
        /// <param name="borders"></param>
        /// <returns></returns>
        public static List<wContour.PolyLine> TracingContourLines(double[,] gridData, double[] cValues, double[] X,
            double[] Y, double noData, ref List<wContour.Border> borders)
        {
            int nc;
            nc = cValues.Length;

            Double XDelt, YDelt;
            XDelt = X[1] - X[0];
            YDelt = Y[1] - Y[0];
            int[,] S1 = new int[gridData.GetLength(0), gridData.GetLength(1)];
            borders = wContour.Contour.TracingBorders(gridData, X, Y, ref S1, noData);
            //return wContour.Contour.CreateContourLines_UndefData(gridData, X, Y, nc, cValues, XDelt, YDelt, S1, noData, borders);
            return wContour.Contour.TracingContourLines(gridData, X, Y, nc, cValues, noData, borders, S1);
        }

        ///// <summary>
        ///// Tracing shaded polygons
        ///// </summary>
        ///// <param name="contourLines"></param>
        ///// <param name="isCutted"></param>
        ///// <param name="X"></param>
        ///// <param name="Y"></param>
        ///// <param name="cValues"></param>
        ///// <returns></returns>
        //public static List<wContour.Polygon> TracingPolygons(List<wContour.PolyLine> contourLines, Boolean isCutted, double[] X, double[] Y, double[] cValues)
        //{
        //    wContour.Extent aBound;
        //    aBound.xMin = X[0];
        //    aBound.xMax = X[X.Length - 1];
        //    aBound.yMin = Y[0];
        //    aBound.yMax = Y[Y.Length - 1];

        //    if (isCutted)
        //    {
        //        return wContour.Contour.CreateCutContourPolygons(contourLines, _cutPlist, aBound, cValues);
        //    }
        //    else
        //    {
        //        return wContour.Contour.CreateContourPolygons(contourLines, aBound, cValues);
        //    }
        //}

        /// <summary>
        /// Tracing shaded polygons with undefine data
        /// </summary>
        /// <param name="gridData"></param>
        /// <param name="contourLines"></param>
        /// <param name="borders"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="cValues"></param>
        /// <returns></returns>
        public static List<wContour.Polygon> TracingPolygons(double[,] gridData, List<wContour.PolyLine> contourLines, List<wContour.Border> borders, double[] X, double[] Y, double[] cValues)
        {
            wContour.Extent aBound = new wContour.Extent();
            aBound.xMin = X[0];
            aBound.xMax = X[X.Length - 1];
            aBound.yMin = Y[0];
            aBound.yMax = Y[Y.Length - 1];

            //return wContour.Contour.CreateBorderContourPolygons(gridData, contourLines, borders, aBound, cValues);
            return wContour.Contour.TracingPolygons(gridData, contourLines, borders, cValues);
        }
    }
}
