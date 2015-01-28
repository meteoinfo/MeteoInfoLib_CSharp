using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Data;
using MeteoInfoC.Shape;
using MeteoInfoC.Layer;

namespace MeteoInfoC.Global
{
    /// <summary>
    /// Operators in MeteoInfoC
    /// </summary>
    public static class MIMath
    {
        #region Methods

        /// <summary>
        /// Split a string by space
        /// </summary>
        /// <param name="aLine">a string</param>
        /// <returns>split result</returns>
        public static string[] SplitBySpace(string aLine)
        {
            string[] dataArray = aLine.Split();
            int LastNonEmpty = -1;
            List<string> dataList = new List<string>();
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (dataArray[i] != string.Empty)
                {
                    LastNonEmpty++;
                    dataList.Add(dataArray[i]);
                }
            }

            dataArray = dataList.ToArray();
            return dataArray;
        }

        /// <summary>
        /// Judge if a rectangle include another
        /// </summary>
        /// <param name="aRect">a rectangle</param>
        /// <param name="bRect">b rectangle</param>
        /// <returns>is included</returns>
        public static bool IsInclude(Rectangle aRect, Rectangle bRect)
        {
            if (aRect.Width >= bRect.Width && aRect.Height >= bRect.Height)
            {
                if (aRect.Left <= bRect.Left && aRect.Right >= bRect.Right && aRect.Top <= bRect.Top && aRect.Bottom >= bRect.Bottom)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Get maximum extent from two extent
        /// </summary>
        /// <param name="aET"></param>
        /// <param name="bET"></param>
        /// <returns></returns>
        public static Extent GetLagerExtent(Extent aET, Extent bET)
        {
            Extent cET = new Extent();
            cET.minX = Math.Min(aET.minX, bET.minX);
            cET.minY = Math.Min(aET.minY, bET.minY);
            cET.maxX = Math.Max(aET.maxX, bET.maxX);
            cET.maxY = Math.Max(aET.maxY, bET.maxY);

            return cET;
        }

        /// <summary>
        /// Get maximum extent from two extent
        /// </summary>
        /// <param name="aET"></param>
        /// <param name="bET"></param>
        /// <returns></returns>
        public static Extent GetSmallerExtent(Extent aET, Extent bET)
        {
            Extent cET = new Extent();
            cET.minX = Math.Max(aET.minX, bET.minX);
            cET.minY = Math.Max(aET.minY, bET.minY);
            cET.maxX = Math.Min(aET.maxX, bET.maxX);
            cET.maxY = Math.Min(aET.maxY, bET.maxY);

            return cET;
        }

        /// <summary>
        /// Shift extent with longitude
        /// </summary>
        /// <param name="aET"></param>
        /// <param name="lonShift"></param>
        /// <returns></returns>
        public static Extent ShiftExtentLon(Extent aET, double lonShift)
        {
            Extent cET = new Extent();
            cET.minX = aET.minX + lonShift;
            cET.maxX = aET.maxX + lonShift;
            cET.minY = aET.minY;
            cET.maxY = aET.maxY;

            return cET;
        }

        /// <summary>
        /// Judge if two extent cross
        /// </summary>
        /// <param name="aET">a extent</param>
        /// <param name="bET">a extent</param>
        /// <returns>If two extent cross</returns>
        public static Boolean IsExtentCross(Extent aET, Extent bET)
        {
            if (aET.maxX < bET.minX || aET.maxY < bET.minY || bET.maxX < aET.minX || bET.maxY < aET.minY)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Get intersection extent
        /// </summary>
        /// <param name="aET">a extent</param>
        /// <param name="bET">a extent</param>
        /// <returns>Intersection extent</returns>
        public static Extent ExtentInterSection(Extent aET, Extent bET)
        {
            Extent cET = new Extent();
            cET.minX = Math.Max(aET.minX, bET.minX);
            cET.maxX = Math.Min(aET.maxX, bET.maxX);
            cET.minY = Math.Max(aET.minY, bET.minY);
            cET.maxY = Math.Min(aET.maxY, bET.maxY);

            return cET;
        }

        /// <summary>
        /// Get extent from point list
        /// </summary>
        /// <param name="PList"></param>
        /// <returns></returns>
        public static Extent GetPointsExtent(ArrayList PList)
        {
            Extent cET = new Extent();
            for (int i = 0; i < PList.Count; i++)
            {
                PointD aP = (PointD)PList[i];
                if (i == 0)
                {
                    cET.minX = aP.X;
                    cET.maxX = aP.X;
                    cET.minY = aP.Y;
                    cET.maxY = aP.Y;
                }
                else
                {
                    if (cET.minX > aP.X)
                    {
                        cET.minX = aP.X;
                    }
                    else if (cET.maxX < aP.X)
                    {
                        cET.maxX = aP.X;
                    }

                    if (cET.minY > aP.Y)
                    {
                        cET.minY = aP.Y;
                    }
                    else if (cET.maxY < aP.Y)
                    {
                        cET.maxY = aP.Y;
                    }
                }
            }

            return cET;
        }

        /// <summary>
        /// Get extent from point list
        /// </summary>
        /// <param name="PList"></param>
        /// <returns></returns>
        public static Extent GetPointsExtent(List<PointZ> PList)
        {
            List<PointD> pList = new List<PointD>();
            foreach (PointZ aP in PList)
                pList.Add(new PointD(aP.X, aP.Y));

            return GetPointsExtent(pList);
        }

        /// <summary>
        /// Get extent from point list
        /// </summary>
        /// <param name="PList"></param>
        /// <returns></returns>
        public static Extent GetPointsExtent(List<PointM> PList)
        {
            List<PointD> pList = new List<PointD>();
            foreach (PointM aP in PList)
                pList.Add(new PointD(aP.X, aP.Y));

            return GetPointsExtent(pList);
        }

        /// <summary>
        /// Get extent from point list
        /// </summary>
        /// <param name="PList"></param>
        /// <returns></returns>
        public static Extent GetPointsExtent(List<PointD> PList)
        {
            Extent cET = new Extent();
            for (int i = 0; i < PList.Count; i++)
            {
                PointD aP = PList[i];
                if (i == 0)
                {
                    cET.minX = aP.X;
                    cET.maxX = aP.X;
                    cET.minY = aP.Y;
                    cET.maxY = aP.Y;
                }
                else
                {
                    if (cET.minX > aP.X)
                    {
                        cET.minX = aP.X;
                    }
                    else if (cET.maxX < aP.X)
                    {
                        cET.maxX = aP.X;
                    }

                    if (cET.minY > aP.Y)
                    {
                        cET.minY = aP.Y;
                    }
                    else if (cET.maxY < aP.Y)
                    {
                        cET.maxY = aP.Y;
                    }
                }
            }

            return cET;
        }

        /// <summary>
        /// Get extent from PointF list
        /// </summary>
        /// <param name="PList"></param>
        /// <returns></returns>
        public static Extent GetPointFsExtent(ArrayList PList)
        {
            Extent cET = new Extent();
            for (int i = 0; i < PList.Count; i++)
            {
                PointF aP = (PointF)PList[i];
                if (i == 0)
                {
                    cET.minX = aP.X;
                    cET.maxX = aP.X;
                    cET.minY = aP.Y;
                    cET.maxY = aP.Y;
                }
                else
                {
                    if (cET.minX > aP.X)
                    {
                        cET.minX = aP.X;
                    }
                    else if (cET.maxX < aP.X)
                    {
                        cET.maxX = aP.X;
                    }

                    if (cET.minY > aP.Y)
                    {
                        cET.minY = aP.Y;
                    }
                    else if (cET.maxY < aP.Y)
                    {
                        cET.maxY = aP.Y;
                    }
                }
            }

            return cET;
        }

        /// <summary>
        /// Get extent from PointF list
        /// </summary>
        /// <param name="PList"></param>
        /// <returns></returns>
        public static Extent GetPointFsExtent(PointF[] PList)
        {
            Extent cET = new Extent();
            for (int i = 0; i < PList.Length; i++)
            {
                PointF aP = (PointF)PList[i];
                if (i == 0)
                {
                    cET.minX = aP.X;
                    cET.maxX = aP.X;
                    cET.minY = aP.Y;
                    cET.maxY = aP.Y;
                }
                else
                {
                    if (cET.minX > aP.X)
                    {
                        cET.minX = aP.X;
                    }
                    else if (cET.maxX < aP.X)
                    {
                        cET.maxX = aP.X;
                    }

                    if (cET.minY > aP.Y)
                    {
                        cET.minY = aP.Y;
                    }
                    else if (cET.maxY < aP.Y)
                    {
                        cET.maxY = aP.Y;
                    }
                }
            }

            return cET;
        }

        /// <summary>
        /// Get extent from PointF list
        /// </summary>
        /// <param name="PList"></param>
        /// <returns></returns>
        public static Extent GetPointFsExtent(List<PointF> PList)
        {
            Extent cET = new Extent();
            for (int i = 0; i < PList.Count; i++)
            {
                PointF aP = (PointF)PList[i];
                if (i == 0)
                {
                    cET.minX = aP.X;
                    cET.maxX = aP.X;
                    cET.minY = aP.Y;
                    cET.maxY = aP.Y;
                }
                else
                {
                    if (cET.minX > aP.X)
                    {
                        cET.minX = aP.X;
                    }
                    else if (cET.maxX < aP.X)
                    {
                        cET.maxX = aP.X;
                    }

                    if (cET.minY > aP.Y)
                    {
                        cET.minY = aP.Y;
                    }
                    else if (cET.maxY < aP.Y)
                    {
                        cET.maxY = aP.Y;
                    }
                }
            }

            return cET;
        }

        /// <summary>
        /// Get extent from discrete data
        /// </summary>
        /// <param name="discreteData">discrete data</param>
        /// <returns>extent</returns>
        public static Extent GetExtent(double[,] discreteData)
        {
            Extent extent = new Extent();
            double minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            for (int i = 0; i < discreteData.GetLength(1); i++)
            {
                double x = discreteData[0, i];
                double y = discreteData[1, i];
                if (i == 0)
                {
                    minX = x;
                    maxX = minX;
                    minY = y;
                    maxY = minY;
                }
                else
                {
                    if (minX > x)
                    {
                        minX = x;
                    }
                    else if (maxX < x)
                    {
                        maxX = x;
                    }
                    if (minY > y)
                    {
                        minY = y;
                    }
                    else if (maxY < y)
                    {
                        maxY = y;
                    }
                }
            }

            extent.minX = minX;
            extent.maxX = maxX;
            extent.minY = minY;
            extent.maxY = maxY;

            return extent;
        }

        /// <summary>
        /// Judge if a point is in a rectangle
        /// </summary>
        /// <param name="aP"></param>
        /// <param name="aET"></param>
        /// <returns></returns>
        public static bool PointInExtent(PointD aP, Extent aET)
        {
            if (aP.X > aET.minX && aP.X < aET.maxX && aP.Y > aET.minY && aP.Y < aET.maxY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Judge if a point is in an extent
        /// </summary>
        /// <param name="aP"></param>
        /// <param name="aET"></param>
        /// <returns></returns>
        public static bool PointInExtent(PointF aP, Extent aET)
        {
            if (aP.X > aET.minX && aP.X < aET.maxX && aP.Y > aET.minY && aP.Y < aET.maxY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Judge if a pointF is in a rectangle
        /// </summary>
        /// <param name="aP">PointF</param>
        /// <param name="aRect">Rectangle</param>
        /// <returns>if the point is inside</returns>
        public static bool PointInRectangle(PointF aP, Rectangle aRect)
        {
            if (aP.X > aRect.X && aP.X < aRect.X + aRect.Width && aP.Y > aRect.Y && aP.Y < aRect.Y + aRect.Height)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Judge if a point is in a rectangle
        /// </summary>
        /// <param name="aP">Point</param>
        /// <param name="aRect">Rectangle</param>
        /// <returns>if the point is inside</returns>
        public static bool PointInRectangle(Point aP, Rectangle aRect)
        {
            if (aP.X > aRect.X && aP.X < aRect.X + aRect.Width && aP.Y > aRect.Y && aP.Y < aRect.Y + aRect.Height)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Judge if a PointD is in a rectangle
        /// </summary>
        /// <param name="aP">Point</param>
        /// <param name="aRect">Rectangle</param>
        /// <returns>if the point is inside</returns>
        public static bool PointInRectangle(PointD aP, Rectangle aRect)
        {
            if (aP.X > aRect.X && aP.X < aRect.X + aRect.Width && aP.Y > aRect.Y && aP.Y < aRect.Y + aRect.Height)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Judge if a point is in a polygon
        /// </summary>
        /// <param name="poly">polygon coordinate list</param>
        /// <param name="aPoint">point</param>
        /// <returns>if inside</returns>
        public static bool PointInPolygon(List<PointD> poly, PointD aPoint)
        {
            double xNew, yNew, xOld, yOld;
            double x1, y1, x2, y2;
            int i;
            bool inside = false;
            int nPoints = poly.Count;

            if (nPoints < 3)
                return false;

            xOld = poly[nPoints - 1].X;
            yOld = poly[nPoints - 1].Y;
            for (i = 0; i < nPoints; i++)
            {
                xNew = poly[i].X;
                yNew = poly[i].Y;
                if (xNew > xOld)
                {
                    x1 = xOld;
                    x2 = xNew;
                    y1 = yOld;
                    y2 = yNew;
                }
                else
                {
                    x1 = xNew;
                    x2 = xOld;
                    y1 = yNew;
                    y2 = yOld;
                }

                //---- edge "open" at left end
                if ((xNew < aPoint.X) == (aPoint.X <= xOld) &&
                   (aPoint.Y - y1) * (x2 - x1) < (y2 - y1) * (aPoint.X - x1))
                    inside = !inside;

                xOld = xNew;
                yOld = yNew;
            }

            return inside;
        }

        /// <summary>
        /// Judge if a point is in a polygon
        /// </summary>
        /// <param name="poly">polygon coordinate list</param>
        /// <param name="aPoint">point</param>
        /// <returns>if inside</returns>
        public static bool PointInPolygon(ArrayList poly, PointD aPoint)
        {
            double xNew, yNew, xOld, yOld;
            double x1, y1, x2, y2;
            int i;
            bool inside = false;
            int nPoints = poly.Count;

            if (nPoints < 3)
                return false;

            xOld = ((PointD)poly[nPoints - 1]).X;
            yOld = ((PointD)poly[nPoints - 1]).Y;
            for (i = 0; i < nPoints; i++)
            {
                xNew = ((PointD)poly[i]).X;
                yNew = ((PointD)poly[i]).Y;
                if (xNew > xOld)
                {
                    x1 = xOld;
                    x2 = xNew;
                    y1 = yOld;
                    y2 = yNew;
                }
                else
                {
                    x1 = xNew;
                    x2 = xOld;
                    y1 = yNew;
                    y2 = yOld;
                }

                //---- edge "open" at left end
                if ((xNew < aPoint.X) == (aPoint.X <= xOld) &&
                   (aPoint.Y - y1) * (x2 - x1) < (y2 - y1) * (aPoint.X - x1))
                    inside = !inside;

                xOld = xNew;
                yOld = yNew;
            }

            return inside;
        }

        /// <summary>
        /// Judge if a point is in a polygon shape
        /// </summary>
        /// <param name="aPGS">PolygonShape</param>
        /// <param name="aPoint">point</param>
        /// <returns>if inside</returns>
        public static bool PointInPolygon(PolygonShape aPGS, PointD aPoint)
        {
            if (aPoint.X < aPGS.Extent.minX || aPoint.X > aPGS.Extent.maxX ||
                aPoint.Y < aPGS.Extent.minY || aPoint.Y > aPGS.Extent.maxY)
                return false;

            bool inside = false;
            for (int p = 0; p < aPGS.PartNum; p++)
            {
                ArrayList pList = new ArrayList();
                if (p == aPGS.PartNum - 1)
                {
                    for (int pp = aPGS.parts[p]; pp < aPGS.PointNum; pp++)
                    {
                        pList.Add(aPGS.Points[pp]);
                    }
                }
                else
                {
                    for (int pp = aPGS.parts[p]; pp < aPGS.parts[p + 1]; pp++)
                    {
                        pList.Add(aPGS.Points[pp]);
                    }
                }
                if (PointInPolygon(pList, aPoint))
                {
                    inside = true;
                    break;
                }
            }

            return inside;
        }

        /// <summary>
        /// Judge if a point is in a polygon
        /// </summary>
        /// <param name="aPGS">PolygonShape</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>if inside</returns>
        public static bool PointInPolygon(PolygonShape aPGS, double x, double y)
        {
            PointD aPoint = new PointD(x, y);
            return PointInPolygon(aPGS, aPoint);
        }

        /// <summary>
        /// Judge if a point is in a polygon layer
        /// </summary>
        /// <param name="aLayer">polygon layer</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>if inside</returns>
        public static bool PointInPolygonLayer(VectorLayer aLayer, double x, double y)
        {
            bool isIn = false;
            foreach (PolygonShape aPGS in aLayer.ShapeList)
            {
                if (PointInPolygon(aPGS, x, y))
                {
                    isIn = true;
                    break;
                }
            }

            return isIn;
        }

        /// <summary>
        /// Judge if a string is number
        /// </summary>
        /// <param name="strNumber">String</param>
        /// <returns>bool</returns>
        public static bool IsNumeric(String strNumber)
        {
            strNumber = strNumber.Trim();
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
            !objTwoDotPattern.IsMatch(strNumber) &&
            !objTwoMinusPattern.IsMatch(strNumber) &&
            objNumberPattern.IsMatch(strNumber);
        }

        /// <summary>
        /// Judge if a string is number
        /// </summary>
        /// <param name="strNumber">String</param>
        /// <returns>bool</returns>
        public static bool IsNumeric_1(String strNumber)
        {
            strNumber = strNumber.Trim();
            double Num;
            bool isNum = double.TryParse(strNumber, out Num);

            return isNum;
        }

        /// <summary>
        /// Judge if a DataColumn is numeric
        /// </summary>
        /// <param name="col">DataColumn</param>
        /// <returns>Is numeric</returns>
        public static bool IsNumeric(DataColumn col)
        {
            if (col == null)
                return false;
            // Make this const 
            var numericTypes = new[] { typeof(Byte), typeof(Decimal), typeof(Double), 
                typeof(Int16), typeof(Int32), typeof(Int64), typeof(SByte), 
                typeof(Single), typeof(UInt16), typeof(UInt32), typeof(UInt64)};
            return numericTypes.Contains(col.DataType);
        }

        /// <summary>
        /// Get max min value of data array
        /// </summary>
        /// <param name="S">data array</param>
        /// <param name="unDef">Undefine data</param>
        /// <param name="min">ref mininum</param>
        /// <param name="max">ref maximum</param>
        /// <returns>If has undefine data</returns>
        public static bool GetMaxMinValue(double[] S, double unDef, ref double min, ref double max)
        {
            int i, validNum;
            bool isNodata = false;

            validNum = 0;
            for (i = 0; i < S.Length; i++)
            {
                if (!(Math.Abs(S[i] / unDef - 1) < 0.01))
                {
                    validNum++;
                    if (validNum == 1)
                    {
                        min = S[i];
                        max = min;
                    }
                    else
                    {
                        if (S[i] < min)
                        {
                            min = S[i];
                        }
                        if (S[i] > max)
                        {
                            max = S[i];
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
        /// Get decimal number of a double data for ToString() format
        /// </summary>
        /// <param name="aData">data</param>
        /// <returns>decimal number</returns>
        public static int GetDecimalNum(double aData)
        {
            if (aData - (int)aData == 0)
                return 0;

            int dNum = 0;
            string eStr;
            int aE;
            
            eStr = aData.ToString("e1");            
            aE = int.Parse(eStr.Substring(eStr.IndexOf("e") + 1));

            if (aE >= 0)
                dNum = 2;
            else
                dNum = Math.Abs(aE) + 1;

            return dNum;
        }

        ///// <summary>
        ///// Judge if a data is undefine
        ///// </summary>
        ///// <param name="aData">data</param>
        ///// <param name="UNDEF">undefine value</param>
        ///// <returns>Is or not undefine data</returns>
        //public static bool IsUndefineData(double aData, double UNDEF)
        //{
        //    if (Math.Abs(aData / UNDEF - 1) < 0.01)
        //        return true;
        //    else
        //        return false;
        //}

        ///// <summary>
        ///// Determine if two double data equal
        ///// </summary>
        ///// <param name="a">double a</param>
        ///// <param name="b">double b</param>
        ///// <returns>is or not equal</returns>
        //public static bool DoubleEquals(double a, double b)
        //{
        //    if (b == 0)
        //        return DoubleEquals_Abs(a, b);
        //    else
        //    {
        //        if (DoubleEquals_Abs(a, b))
        //        {
        //            if (Math.Abs(a / b - 1) < 0.00001)
        //                return true;
        //            else
        //                return false;
        //        }
        //        else
        //            return false;
        //    }
        //}

        /// <summary>
        /// Determine if two double data equal
        /// </summary>
        /// <param name="a">double a</param>
        /// <param name="b">double b</param>
        /// <returns>is or not equal</returns>
        public static bool DoubleEquals(double a, double b)
        {
            double difference = Math.Abs(a * 0.00001);
            if (Math.Abs(a - b) <= difference)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determine if two double data equal
        /// </summary>
        /// <param name="a">double a</param>
        /// <param name="b">double b</param>
        /// <returns>is or not equal</returns>
        public static bool DoubleEquals_Abs(double a, double b)
        {
            if (Math.Abs(a - b) < 0.0000001)
                return true;
            else
                return false;
        }

        /// <summary>
        /// longitude distance
        /// </summary>
        /// <param name="lon1">longitude 1</param>
        /// <param name="lon2">longitude 2</param>
        /// <returns>longitude distance</returns>
        public static float LonDistance(float lon1, float lon2)
        {
            if (Math.Abs(lon1 - lon2) > 180)
            {
                if (lon1 > lon2)
                    lon2 += 360;
                else
                    lon1 += 360;
            }

            return Math.Abs(lon1 - lon2);
        }

        /// <summary>
        /// Add longitude
        /// </summary>
        /// <param name="lon1">longitude 1</param>
        /// <param name="delta">delta</param>
        /// <returns>longitude</returns>
        public static float LonAdd(float lon1, float delta)
        {
            float lon = lon1 + delta;
            if (lon > 180)
                lon -= 360;
            if (lon < -180)
                lon += 360;

            return lon;
        }

        /// <summary>
        /// Calculate ellipse coordinate by angle
        /// </summary>
        /// <param name="x0">center x</param>
        /// <param name="y0">center y</param>
        /// <param name="a">major semi axis</param>
        /// <param name="b">minor semi axis</param>
        /// <param name="angle">angle</param>
        /// <returns>coordinate</returns>
        public static PointF CalEllipseCoordByAngle(double x0, double y0, double a, double b, double angle)
        {
            double dx, dy;
            dx = Math.Sqrt((a * a * b * b) / (b * b + a * a * Math.Tan(angle) * Math.Tan(angle)));
            dy = dx * Math.Tan(angle);

            double x, y;
            if (angle <= Math.PI / 2)
            {
                x = x0 + dx;
                y = y0 + dy;
            }
            else if (angle <= Math.PI)
            {
                x = x0 - dx;
                y = y0 - dy;
            }
            else if (angle <= Math.PI * 1.5)
            {
                x = x0 - dx;
                y = y0 - dy;
            }
            else
            {
                x = x0 + dx;
                y = y0 + dy;
            }

            PointF aP = new PointF((float)x, (float)y);
            return aP;
        }

        /// <summary>
        /// Get wind direction from U/V
        /// </summary>
        /// <param name="U">U</param>
        /// <param name="V">V</param>
        /// <returns>wind direction</returns>
        public static double GetWindDirection(double U, double V)
        {
            double windSpeed = (Single)Math.Sqrt(U * U + V * V);
            double windDir = Math.Asin(U / windSpeed) * 180 / Math.PI;
            if (U < 0 && V < 0)
            {
                windDir = 180 - windDir;
            }
            else if (U > 0 && V < 0)
            {
                windDir = 180 - windDir;
            }
            else if (U < 0 && V > 0)
            {
                windDir = 360 + windDir;
            }
            windDir += 180;
            if (windDir >= 360)
                windDir -= 360;

            return windDir;
        }

        #endregion
    }
}
