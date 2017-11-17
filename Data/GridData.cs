using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MeteoInfoC.Layer;
using MeteoInfoC.Data.MapData;
using MeteoInfoC.Map;
using MeteoInfoC.Global;
using MeteoInfoC.Projections;
using MeteoInfoC.Shape;

namespace MeteoInfoC.Data
{
    /// <summary>
    /// Grid data
    /// </summary>
    public class GridData
    {
        #region Variable
        /// <summary>
        /// Grid data
        /// </summary>
        public double[,] Data;        
        /// <summary>
        /// x coordinate array
        /// </summary>
        public double[] X;
        /// <summary>
        /// y coordinate array
        /// </summary>
        public double[] Y;
        /// <summary>
        /// Undef data
        /// </summary>
        public double MissingValue;
        public ProjectionInfo ProjInfo = null;
        private bool _xStag = false;
        private bool _yStag = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>        
        public GridData()
        {
            MissingValue = -9999;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aGridData">grid data</param>
        public GridData(GridData aGridData)
        {
            X = aGridData.X;
            Y = aGridData.Y;            
            MissingValue = aGridData.MissingValue;
            Data = new double[YNum, XNum];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xStart">x start</param>
        /// <param name="xDelt">x delt</param>
        /// <param name="xNum">x number</param>
        /// <param name="yStart">y start</param>
        /// <param name="yDelt">y delt</param>
        /// <param name="yNum">y number</param>
        public GridData(double xStart, double xDelt, int xNum, double yStart, double yDelt, int yNum)
        {            
            X = new double[xNum];
            Y = new double[yNum];
            for (int i = 0; i < xNum; i++)
                X[i] = xStart + xDelt * i;
            for (int i = 0; i < yNum; i++)
                Y[i] = yStart + yDelt * i;

            MissingValue = -9999;
            Data = new double[yNum, xNum];
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get X number
        /// </summary>
        public int XNum { get { return X.Length; } }
        /// <summary>
        /// Get Y number
        /// </summary>
        public int YNum { get { return Y.Length; } }
        /// <summary>
        /// Get X delt
        /// </summary>
        public double XDelt { get { return X[1] - X[0]; } }
        /// <summary>
        /// Get Y delt
        /// </summary>
        public double YDelt { get { return Y[1] - Y[0]; } }

        /// <summary>
        /// Get if the data is global
        /// </summary>
        public bool IsGlobal
        {
            get
            {
                bool isGlobal = false;
                if (MIMath.DoubleEquals(X[XNum - 1] + XDelt - X[0], 360.0))
                    isGlobal = true;

                return isGlobal;
            }
        }

        /// <summary>
        /// Get or set if is X stag
        /// </summary>
        public bool XStag
        {
            get { return _xStag; }
            set { _xStag = value; }
        }

        /// <summary>
        /// Get or set if is Y stag
        /// </summary>
        public bool YStag
        {
            get { return _yStag; }
            set { _yStag = value; }
        }

        #endregion

        #region Methods

        #region Operator
        /// <summary>
        /// Override operator + for two GridData
        /// </summary>
        /// <param name="aGrid">a GridData</param>
        /// <param name="bGrid">a GridData</param>
        /// <returns>result GridData</returns>
        public static GridData operator +(GridData aGrid, GridData bGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;
            //if (bGrid.XNum != xNum || bGrid.YNum != yNum)
            //    return null;

            GridData cGrid = new GridData();
            cGrid.X = aGrid.X;
            cGrid.Y = aGrid.Y;            
            cGrid.Data = new double[yNum, xNum];
            cGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue) ||
                        MIMath.DoubleEquals(bGrid.Data[i, j], bGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        cGrid.Data[i, j] = aGrid.Data[i, j] + bGrid.Data[i, j];
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator + for a GridData and a double data
        /// </summary>
        /// <param name="aGrid">a GridData</param>
        /// <param name="aData">a double data</param>
        /// <returns>result GridData</returns>
        public static GridData operator +(GridData aGrid, double aData)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;            

            GridData cGrid = new GridData();
            cGrid.X = aGrid.X;
            cGrid.Y = aGrid.Y;            
            cGrid.Data = new double[yNum, xNum];
            cGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        cGrid.Data[i, j] = aGrid.Data[i, j] + aData;
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator + for a double data and a GridData
        /// </summary>
        /// /// <param name="aData">a double data</param>
        /// <param name="aGrid">a GridData</param>        
        /// <returns>result GridData</returns>
        public static GridData operator +(double aData, GridData aGrid)
        {
            return aGrid + aData;
        }

        /// <summary>
        /// Override operator - for GridData
        /// </summary>
        /// <param name="aGrid">a GridData</param>
        /// <param name="bGrid">a GridData</param>
        /// <returns>result GridData</returns>
        public static GridData operator -(GridData aGrid, GridData bGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;
            //if (bGrid.XNum != xNum || bGrid.YNum != yNum)
            //    return null;

            GridData cGrid = new GridData();
            cGrid.X = aGrid.X;
            cGrid.Y = aGrid.Y;            
            cGrid.Data = new double[yNum, xNum];
            cGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue) ||
                        MIMath.DoubleEquals(bGrid.Data[i, j], bGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        cGrid.Data[i, j] = aGrid.Data[i, j] - bGrid.Data[i, j];
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator - for a GridData and a double data
        /// </summary>
        /// <param name="aGrid">a GridData</param>
        /// <param name="aData">a double data</param>
        /// <returns>result GridData</returns>
        public static GridData operator -(GridData aGrid, double aData)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;            

            GridData cGrid = new GridData();
            cGrid.X = aGrid.X;
            cGrid.Y = aGrid.Y;            
            cGrid.Data = new double[yNum, xNum];
            cGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        cGrid.Data[i, j] = aGrid.Data[i, j] - aData;
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator - for a double data and a GridData
        /// </summary>
        /// /// <param name="aData">a double data</param>
        /// <param name="aGrid">a GridData</param>        
        /// <returns>result GridData</returns>
        public static GridData operator -(double aData, GridData aGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData cGrid = new GridData(aGrid);            
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        cGrid.Data[i, j] = aData - aGrid.Data[i, j];
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator * for GridData
        /// </summary>
        /// <param name="aGrid">a GridData</param>
        /// <param name="bGrid">a GridData</param>
        /// <returns>result GridData</returns>
        public static GridData operator *(GridData aGrid, GridData bGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;
            //if (bGrid.XNum != xNum || bGrid.YNum != yNum)
            //    return null;

            GridData cGrid = new GridData();
            cGrid.X = aGrid.X;
            cGrid.Y = aGrid.Y;            
            cGrid.Data = new double[yNum, xNum];
            cGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue) ||
                        MIMath.DoubleEquals(bGrid.Data[i, j], bGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        cGrid.Data[i, j] = aGrid.Data[i, j] * bGrid.Data[i, j];
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator * for a GridData and a double data
        /// </summary>
        /// <param name="aGrid">a GridData</param>
        /// <param name="aData">a double data</param>
        /// <returns>result GridData</returns>
        public static GridData operator *(GridData aGrid, double aData)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;            

            GridData cGrid = new GridData();
            cGrid.X = aGrid.X;
            cGrid.Y = aGrid.Y;            
            cGrid.Data = new double[yNum, xNum];
            cGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        cGrid.Data[i, j] = aGrid.Data[i, j] * aData;
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator * for a double data and a GridData
        /// </summary>
        /// <param name="aData">a double data</param>
        /// <param name="aGrid">a GridData</param>        
        /// <returns>result GridData</returns>
        public static GridData operator *(double aData, GridData aGrid)
        {
            return aGrid * aData;
        }

        /// <summary>
        /// Override operator / for GridData
        /// </summary>
        /// <param name="aGrid">a GridData</param>
        /// <param name="bGrid">a GridData</param>
        /// <returns>result GridData</returns>
        public static GridData operator /(GridData aGrid, GridData bGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;
            //if (bGrid.XNum != xNum || bGrid.YNum != yNum)
            //    return null;

            GridData cGrid = new GridData();
            cGrid.X = aGrid.X;
            cGrid.Y = aGrid.Y;           
            cGrid.Data = new double[yNum, xNum];
            cGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue) ||
                        MIMath.DoubleEquals(bGrid.Data[i, j], bGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                    {
                        if (bGrid.Data[i, j] == 0)
                            cGrid.Data[i, j] = cGrid.MissingValue;
                        else
                            cGrid.Data[i, j] = aGrid.Data[i, j] / bGrid.Data[i, j];
                    }
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator / for a GridData and a double data
        /// </summary>
        /// <param name="aGrid">a GridData</param>
        /// <param name="aData">a double data</param>
        /// <returns>result GridData</returns>
        public static GridData operator /(GridData aGrid, double aData)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData cGrid = new GridData();
            cGrid.X = aGrid.X;
            cGrid.Y = aGrid.Y;            
            cGrid.Data = new double[yNum, xNum];
            cGrid.MissingValue = aGrid.MissingValue;
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        cGrid.Data[i, j] = aGrid.Data[i, j] / aData;
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator / for a double data and a GridData
        /// </summary>
        /// <param name="aData">a double data</param>
        /// <param name="aGrid">a GridData</param>       
        /// <returns>result GridData</returns>
        public static GridData operator /(double aData, GridData aGrid)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData cGrid = new GridData(aGrid);            
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                        cGrid.Data[i, j] = aData / aGrid.Data[i, j];
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator > for a GridData and a double data
        /// </summary>
        /// <param name="aGrid">a GridData</param>
        /// <param name="aData">a double data</param>
        /// <returns>result GridData</returns>
        public static GridData operator >(GridData aGrid, double aData)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData cGrid = new GridData(aGrid);            
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                    {
                        if (aGrid.Data[i, j] > aData)
                            cGrid.Data[i, j] = 1;
                        else
                            cGrid.Data[i, j] = 0;
                    }
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator >= for a GridData and a double data
        /// </summary>
        /// <param name="aGrid">a GridData</param>
        /// <param name="aData">a double data</param>
        /// <returns>result GridData</returns>
        public static GridData operator >=(GridData aGrid, double aData)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData cGrid = new GridData(aGrid);
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                    {
                        if (aGrid.Data[i, j] >= aData)
                            cGrid.Data[i, j] = 1;
                        else
                            cGrid.Data[i, j] = 0;
                    }
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator less than for a GridData and a double data
        /// </summary>
        /// <param name="aGrid">a GridData</param>
        /// <param name="aData">a double data</param>
        /// <returns>result GridData</returns>
        public static GridData operator <(GridData aGrid, double aData)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData cGrid = new GridData(aGrid);
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                    {
                        if (aGrid.Data[i, j] < aData)
                            cGrid.Data[i, j] = 1;
                        else
                            cGrid.Data[i, j] = 0;
                    }
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Override operator less than or equal for a GridData and a double data
        /// </summary>
        /// <param name="aGrid">a GridData</param>
        /// <param name="aData">a double data</param>
        /// <returns>result GridData</returns>
        public static GridData operator <=(GridData aGrid, double aData)
        {
            int xNum = aGrid.XNum;
            int yNum = aGrid.YNum;

            GridData cGrid = new GridData(aGrid);
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if (MIMath.DoubleEquals(aGrid.Data[i, j], aGrid.MissingValue))
                        cGrid.Data[i, j] = aGrid.MissingValue;
                    else
                    {
                        if (aGrid.Data[i, j] <= aData)
                            cGrid.Data[i, j] = 1;
                        else
                            cGrid.Data[i, j] = 0;
                    }
                }
            }

            return cGrid;
        }

        #endregion

        #region Other
        /// <summary>
        /// Y reverse to the grid data
        /// </summary>
        public void YReverse()
        {
            double[, ] newData = new double[YNum, XNum];
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                {
                    newData[YNum - i - 1, j] = Data[i, j];
                }
            }
            Data = newData;
        }

        /// <summary>
        /// Get minimum value
        /// </summary>
        /// <returns>minimum value</returns>
        public double GetMinValue()
        {
            double min = 0;
            int vdNum = 0;
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                {
                    if (MIMath.DoubleEquals(Data[i, j], MissingValue))
                        continue;

                    if (vdNum == 0)
                        min = Data[i, j];
                    else
                    {
                        if (min > Data[i, j])
                            min = Data[i, j];
                    }
                    vdNum += 1;
                }
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
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                {
                    if (MIMath.DoubleEquals(Data[i, j], MissingValue))
                        continue;

                    if (vdNum == 0)
                        max = Data[i, j];
                    else
                    {
                        if (max < Data[i, j])
                            max = Data[i, j];
                    }
                    vdNum += 1;
                }
            }

            return max;
        }

        /// <summary>
        /// Calculate Summary value
        /// </summary>
        /// <returns>summary value</returns>
        public double Sum()
        {
            double sum = 0;
            int vdNum = 0;
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                {
                    if (MIMath.DoubleEquals(Data[i, j], MissingValue))
                        continue;

                    sum += Data[i, j];
                    vdNum += 1;
                }
            }
            
            return sum;
        }

        /// <summary>
        /// Calculate average value
        /// </summary>
        /// <returns>average value</returns>
        public double Average()
        {
            double ave = 0;
            int vdNum = 0;
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                {
                    if (MIMath.DoubleEquals(Data[i, j], MissingValue))
                        continue;

                    ave += Data[i, j];                    
                    vdNum += 1;
                }
            }

            ave = ave / vdNum;

            return ave;
        }

        /// <summary>
        /// Mask out grid data with a polygon shape
        /// </summary>
        /// <param name="maskGrid">Mask grid data</param>
        /// <returns>Maskouted grid data</returns>
        public GridData Maskout(GridData maskGrid)
        {
            GridData cGrid = new GridData();
            cGrid.X = X;
            cGrid.Y = Y;
            cGrid.Data = new double[YNum, XNum];
            cGrid.MissingValue = MissingValue;
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                {
                    if (maskGrid.Data[i, j] >= 0)
                        cGrid.Data[i, j] = Data[i, j];
                    else
                        cGrid.Data[i, j] = MissingValue;
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Mask out grid data with a polygon shape
        /// </summary>
        /// <param name="aPGS">Polygon shape</param>
        /// <returns>Maskouted grid data</returns>
        public GridData Maskout(PolygonShape aPGS)
        {
            GridData cGrid = new GridData();
            cGrid.X = X;
            cGrid.Y = Y;
            cGrid.Data = new double[YNum, XNum];
            cGrid.MissingValue = MissingValue;
            for (int i = 0; i < YNum; i++)
            {
                if (Y[i] >= aPGS.Extent.minY && Y[i] <= aPGS.Extent.maxY)
                {
                    for (int j = 0; j < XNum; j++)
                    {
                        if (X[j] >= aPGS.Extent.minX && X[j] <= aPGS.Extent.maxX)
                        {
                            if (Global.MIMath.PointInPolygon(aPGS, X[j], Y[i]))
                                cGrid.Data[i, j] = Data[i, j];
                            else
                                cGrid.Data[i, j] = MissingValue;
                        }
                        else
                            cGrid.Data[i, j] = MissingValue;
                    }
                }
                else
                {
                    for (int j = 0; j < XNum; j++)
                    {
                        cGrid.Data[i, j] = MissingValue;
                    }
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Mask out grid data with a polygon layer
        /// </summary>
        /// <param name="maskLayer">maskout layer</param>
        /// <returns>grid data</returns>
        public GridData Maskout(VectorLayer maskLayer)
        {
            if (maskLayer.ShapeType != MeteoInfoC.Shape.ShapeTypes.Polygon)
            {
                MessageBox.Show("The mask layer is not a polygon layer!" + Environment.NewLine + maskLayer.FileName, "Error");
                return this;
            }

            GridData cGrid = new GridData();
            cGrid.X = X;
            cGrid.Y = Y;            
            cGrid.Data = new double[YNum, XNum];
            cGrid.MissingValue = MissingValue;
            for (int i = 0; i < YNum; i++)
            {
                if (Y[i] >= maskLayer.Extent.minY && Y[i] <= maskLayer.Extent.maxY)
                {
                    for (int j = 0; j < XNum; j++)
                    {
                        if (X[j] >= maskLayer.Extent.minX && X[j] <= maskLayer.Extent.maxX)
                        {
                            if (Global.MIMath.PointInPolygonLayer(maskLayer, X[j], Y[i]))
                                cGrid.Data[i, j] = Data[i, j];
                            else
                                cGrid.Data[i, j] = MissingValue;
                        }
                        else
                            cGrid.Data[i, j] = MissingValue;
                    }
                }
                else
                {
                    for (int j = 0; j < XNum; j++)
                    {
                        cGrid.Data[i, j] = MissingValue;
                    }
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Maskout grid data by a polygon layer
        /// </summary>
        /// <param name="aMapView">MapView</param>
        /// <param name="layerName">layer name</param>
        /// <returns>grid data</returns>
        public GridData Maskout(MapView aMapView, string layerName)
        {
            int aHnd = aMapView.GetLayerHandleFromName(layerName);
            VectorLayer aLayer = (VectorLayer)aMapView.GetLayerFromHandle(aHnd);
            return Maskout(aLayer);
        }

        /// <summary>
        /// Save as Surfer ASCII data file
        /// </summary>
        /// <param name="aFile">file path</param>
        public void SaveAsSurferASCIIFile(string aFile)
        {
            StreamWriter sw = new StreamWriter(aFile);
            sw.WriteLine("DSAA");
            sw.WriteLine(XNum.ToString() + " " + YNum.ToString());
            sw.WriteLine(X[0].ToString() + " " + X[X.Length - 1].ToString());
            sw.WriteLine(Y[0].ToString() + " " + Y[Y.Length - 1].ToString());
            sw.WriteLine(GetMinValue().ToString() + " " + GetMaxValue().ToString());
            double value;
            string aLine = string.Empty;
            for (int i = 0; i < YNum; i++)
            {                
                for (int j = 0; j < XNum; j++)
                {
                    if (MIMath.DoubleEquals(Data[i, j], MissingValue))
                        value = -9999.0;
                    else
                        value = Data[i, j];

                    if (j == 0)
                        aLine = value.ToString();
                    else
                        aLine = aLine + " " + value.ToString();
                }
                sw.WriteLine(aLine);
            }

            sw.Close();
        }

        /// <summary>
        /// Save as MICAPS 4 data file
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <param name="description">description</param>
        /// <param name="aTime">data time</param>
        /// <param name="hours">hours</param>
        /// <param name="level">level</param>
        /// <param name="smooth">smooth coefficient</param>
        /// <param name="boldValue">bold value</param>
        public void SaveAsMICAPS4File(string aFile, string description, DateTime aTime, int hours, int level,
            float smooth, float boldValue)
        {
            double MinData = 0;
            double MaxData = 0;
            double undef = 9999;

            bool hasNoData = Drawing.ContourDraw.GetMaxMinValue(Data, undef, ref MinData, ref MaxData);
            int dNum = Global.MIMath.GetDecimalNum(MinData);

            SaveAsMICAPS4File(aFile, description, aTime, hours, level, smooth, boldValue, dNum);
        }

        /// <summary>
        /// Save as MICAPS 4 data file
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <param name="description">description</param>
        /// <param name="aTime">data time</param>
        /// <param name="hours">hours</param>
        /// <param name="level">level</param>
        /// <param name="smooth">smooth coefficient</param>
        /// <param name="boldValue">bold value</param>
        /// <param name="dNum">Decimal number of the data</param>
        public void SaveAsMICAPS4File(string aFile, string description, DateTime aTime, int hours, int level,
            float smooth, float boldValue, int dNum)
        {
            //Get contour values
            double[] CValues;            
            double MinData = 0;
            double MaxData = 0;
            double undef = 9999;

            bool hasNoData = Drawing.ContourDraw.GetMaxMinValue(Data, undef, ref MinData, ref MaxData);
            CValues = Legend.LegendManage.CreateContourValues(MinData, MaxData);
            double cDelt = CValues[1] - CValues[0];                        
            string dFormat = "f" + dNum.ToString();

            //Write file
            StreamWriter sw = new StreamWriter(aFile);
            sw.WriteLine("diamond 4 " + description);
            string aLine = aTime.ToString("yy MM dd HH") + " " + hours.ToString() + " " + level.ToString();
            sw.WriteLine(aLine);
            aLine = XDelt.ToString() + " " + YDelt.ToString() + " " + X[0].ToString() + " " + X[XNum - 1].ToString() +
                " " + Y[0].ToString() + " " + Y[YNum - 1].ToString();
            sw.WriteLine(aLine);
            aLine = XNum.ToString() + " " + YNum.ToString() + " " + cDelt.ToString() + " " + CValues[0].ToString() + " " +
                CValues[CValues.Length - 1].ToString() + " " + smooth.ToString() + " " + boldValue.ToString();
            sw.WriteLine(aLine);
            double value;
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                {
                    if (MIMath.DoubleEquals(Data[i, j], MissingValue))
                        value = undef;
                    else
                        value = Data[i, j];

                    if (j == 0)
                        aLine = value.ToString(dFormat);
                    else
                        aLine = aLine + " " + value.ToString(dFormat);
                }
                sw.WriteLine(aLine);
            }

            sw.Close();
        }

        /// <summary>
        /// Save as MICAPS 4 data file
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <param name="description">description</param>
        /// <param name="aTime">data time</param>
        /// <param name="hours">hours</param>
        /// <param name="level">level</param>
        /// <param name="smooth">smooth coefficient</param>
        /// <param name="boldValue">bold value</param>
        /// <param name="projInfo">projection information</param>
        public void SaveAsMICAPS4File(string aFile, string description, DateTime aTime, int hours, int level,
            float smooth, float boldValue, Projections.ProjectionInfo projInfo)
        {
            double MinData = 0;
            double MaxData = 0;
            double undef = 9999;

            bool hasNoData = Drawing.ContourDraw.GetMaxMinValue(Data, undef, ref MinData, ref MaxData);
            int dNum = Global.MIMath.GetDecimalNum(MinData);

            SaveAsMICAPS4File(aFile, description, aTime, hours, level, smooth, boldValue, projInfo, dNum);
        }

        /// <summary>
        /// Save as MICAPS 4 data file
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <param name="description">description</param>
        /// <param name="aTime">data time</param>
        /// <param name="hours">hours</param>
        /// <param name="level">level</param>
        /// <param name="smooth">smooth coefficient</param>
        /// <param name="boldValue">bold value</param>
        /// <param name="projInfo">projection information</param>
        /// <param name="dNum">Decimal number of the data</param>
        public void SaveAsMICAPS4File(string aFile, string description, DateTime aTime, int hours, int level,
            float smooth, float boldValue, Projections.ProjectionInfo projInfo, int dNum)
        {
            //Get contour values
            double[] CValues;
            double MinData = 0;
            double MaxData = 0;
            double undef = 9999;

            bool hasNoData = Drawing.ContourDraw.GetMaxMinValue(Data, undef, ref MinData, ref MaxData);
            CValues = Legend.LegendManage.CreateContourValues(MinData, MaxData);
            double cDelt = CValues[1] - CValues[0];            
            string dFormat = "f" + dNum.ToString();

            //Write file
            StreamWriter sw = new StreamWriter(aFile);
            sw.WriteLine("diamond 4 " + description);            
            string aLine = aTime.ToString("yy MM dd HH") + " " + hours.ToString() + " " + level.ToString();
            sw.WriteLine(aLine);

            double eCValue = CValues[CValues.Length - 1];
            double xdelt = XDelt;
            double ydelt = YDelt;
            double sLon = X[0];
            double eLon = X[XNum - 1];
            double sLat = Y[0];
            double eLat = Y[YNum - 1];
            if (!projInfo.IsLatLon)
            {
                switch (projInfo.Transform.ProjectionName)
                {
                    case MeteoInfoC.Projections.ProjectionNames.Lambert_Conformal:
                        eCValue = -1;
                        break;
                    case MeteoInfoC.Projections.ProjectionNames.Mercator:
                        eCValue = -2;
                        break;
                    case MeteoInfoC.Projections.ProjectionNames.North_Polar_Stereographic:
                        eCValue = -3;
                        break;
                }
                Projections.ProjectionInfo toProj = Projections.KnownCoordinateSystems.Geographic.World.WGS1984;                
                double[][] points = new double[2][];                
                points[0] = new double[] { sLon, sLat };
                points[1] = new double[] { eLon, eLat };
                Projections.Reproject.ReprojectPoints(points, projInfo, toProj, 0, 2);
                xdelt = points[1][0];
                ydelt = points[0][1];
                sLon = points[0][0];
                sLat = points[0][1];
                eLon = points[1][0];
                eLat = points[1][1];
            }
            aLine = xdelt.ToString() + " " + ydelt.ToString() + " " + sLon.ToString() + " " + eLon.ToString() +
                " " + sLat.ToString() + " " + eLat.ToString();
            sw.WriteLine(aLine);
            aLine = XNum.ToString() + " " + YNum.ToString() + " " + cDelt.ToString() + " " + CValues[0].ToString() + " " +
                eCValue.ToString() + " " + smooth.ToString() + " " + boldValue.ToString();
            sw.WriteLine(aLine);
            double value;
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                {
                    if (MIMath.DoubleEquals(Data[i, j], MissingValue))
                        value = undef;
                    else
                        value = Data[i, j];

                    if (j == 0)
                        aLine = value.ToString(dFormat);
                    else
                        aLine = aLine + " " + value.ToString(dFormat);
                }
                sw.WriteLine(aLine);
            }

            sw.Close();
        }

        /// <summary>
        /// Extract grid data by extent
        /// </summary>
        /// <param name="extent">extent</param>
        /// <returns>Extracted grid data</returns>
        public GridData Extract(Extent extent)
        {
            return Extract(extent.minX, extent.maxX, extent.minY, extent.maxY);
        }

        /// <summary>
        /// Extract grid data by extent
        /// </summary>
        /// <param name="sX">start X</param>
        /// <param name="eX">end X</param>
        /// <param name="sY">start Y</param>
        /// <param name="eY">end Y</param>
        /// <returns>grid data</returns>
        public GridData Extract(double sX, double eX, double sY, double eY)
        {
            if (eX <= sX || eY <= sY)
                return null;

            if (sX >= X[XNum - 2] || sY >= Y[YNum - 2])
                return null;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            int sXidx = 0, eXidx = XNum - 1, sYidx = 0, eYidx = YNum - 1;

            //Get start x
            int i;
            if (sX <= X[0])
                sXidx = 0;
            else
            {
                for (i = 0; i < XNum; i++)
                {
                    if (sX <= X[i])
                    {
                        sXidx = i;
                        break;
                    }
                }
            }

            //Get end x            
            if (eX >= X[XNum - 1])
                eXidx = XNum - 1;
            else
            {
                for (i = sXidx; i < XNum; i++)
                {
                    if (MIMath.DoubleEquals(eX, X[i]))
                    {
                        eXidx = i;
                        break;
                    }
                    else if (eX < X[i])
                    {
                        eXidx = i - 1;
                        break;
                    }
                }
            }

            //Get star y            
            if (sY <= Y[0])
                sYidx = 0;
            else
            {
                for (i = 0; i < YNum; i++)
                {
                    if (sY <= Y[i])
                    {
                        sYidx = i;
                        break;
                    }
                }
            }

            //Get end y            
            if (eY >= Y[YNum - 1])
                eYidx = YNum - 1;
            else
            {
                for (i = sYidx; i < YNum; i++)
                {
                    if (MIMath.DoubleEquals(eY, Y[i]))
                    {
                        eYidx = i;
                        break;
                    }
                    else if (eY < Y[i])
                    {
                        eYidx = i - 1;
                        break;
                    }
                }
            }

            int newXNum = eXidx - sXidx + 1;            
            double[] newX = new double[newXNum];
            for (i = sXidx; i <= eXidx; i++)
            {
                newX[i - sXidx] = X[i];
            }

            int newYNum = eYidx - sYidx + 1;
            double[] newY = new double[newYNum];
            for (i = sYidx; i <= eYidx; i++)
            {
                newY[i - sYidx] = Y[i];
            }

            aGridData.X = newX;
            aGridData.Y = newY;            

            double[,] newData = new double[newYNum, newXNum];
            for (i = sYidx; i <= eYidx; i++)
            {
                for (int j = sXidx; j <= eXidx; j++)
                {
                    newData[i - sYidx, j - sXidx] = Data[i, j];
                }
            }
            aGridData.Data = newData;

            return aGridData;
        }

        /// <summary>
        /// Extract grid by extent index
        /// </summary>
        /// <param name="sXIdx">start x index</param>
        /// <param name="sYIdx">start y index</param>
        /// <param name="xNum">x number</param>
        /// <param name="yNum">y number</param>
        /// <returns>Extracted grid data</returns>
        public GridData Extract(int sXIdx, int sYIdx, int xNum, int yNum)
        {
            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            int eXIdx = sXIdx + xNum - 1, eYIdx = sYIdx + yNum - 1;
            double[] newX = new double[xNum];
            int i;
            for (i = sXIdx; i <= eXIdx; i++)
            {
                newX[i - sXIdx] = X[i];
            }

            double[] newY = new double[yNum];
            for (i = sYIdx; i <= eYIdx; i++)
            {
                newY[i - sYIdx] = Y[i];
            }

            aGridData.X = newX;
            aGridData.Y = newY;

            double[,] newData = new double[yNum, xNum];
            for (i = sYIdx; i <= eYIdx; i++)
            {
                for (int j = sXIdx; j <= eXIdx; j++)
                {
                    newData[i - sYIdx, j - sXIdx] = Data[i, j];
                }
            }
            aGridData.Data = newData;

            return aGridData;
        }

        /// <summary>
        /// Skip the grid data by two dimension skip factor
        /// </summary>
        /// <param name="skipI">skip factor I</param>
        /// <param name="skipJ">skip factor J</param>
        /// <returns>result grid data</returns>
        public GridData Skip(int skipI, int skipJ)
        {
            int yNum = (YNum + skipI - 1) / skipI;
            int xNum = (XNum + skipJ - 1) / skipJ;
            int i, j, idxI, idxJ;
            
            GridData gData = new GridData();
            gData.MissingValue = MissingValue;
            gData.X = new double[xNum];
            gData.Y = new double[yNum];
            gData.Data = new double[yNum, xNum];

            for (i = 0; i < yNum; i++)
            {
                idxI = i * skipI;
                gData.Y[i] = Y[idxI];
            }
            for (j = 0; j < xNum; j++)
            {
                idxJ = j * skipJ;
                gData.X[j] = X[idxJ];
            }
            for (i = 0; i < yNum; i++)
            {
                idxI = i * skipI;
                for (j = 0; j < xNum; j++)
                {
                    idxJ = j * skipJ;
                    gData.Data[i, j] = Data[idxI, idxJ];
                }
            }

            return gData;
        }

        /// <summary>
        /// Get a cell value by X/Y coordinate - nearest cell
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>Cell value</returns>
        public double GetValue(double x, double y)
        {
            double iValue = MissingValue;
            if (x < X[0] || x > X[XNum - 1] || y < Y[0] || y > Y[YNum - 1])
                return iValue;

            //Get x/y index
            int xIdx = 0, yIdx = 0;
            xIdx = (int)((x - X[0]) / XDelt);
            yIdx = (int)((y - Y[0]) / YDelt);            
            if (xIdx == XNum - 1)
                xIdx = XNum - 2;

            if (yIdx == YNum - 1)
                yIdx = YNum - 2;

            int i1 = yIdx;
            int j1 = xIdx;
            int i2 = i1 + 1;
            int j2 = j1 + 1;            
            
            if (x - X[j1] < X[j2] - x)
            {
                xIdx = j1;
                if (y - Y[i1] < Y[i2] - y)
                {
                    yIdx = i1;
                }
                else
                {
                    yIdx = i2;
                }
            }
            else
            {
                xIdx = j2;
                if (y - Y[i1] < Y[i2] - y)
                {
                    yIdx = i1;
                }
                else
                {
                    yIdx = i2;
                }
            }

            iValue = Data[yIdx, xIdx];
            return iValue;
        }

        /// <summary>
        /// Interpolate grid data to a station point
        /// </summary>
        /// <param name="x">x coordinate of the station</param>
        /// <param name="y">y coordinate of the station</param>
        /// <returns>interpolated value</returns>
        public double ToStation(double x, double y)
        {
            double iValue = MissingValue;
            if (x < X[0] || x > X[XNum - 1] || y < Y[0] || y > Y[YNum - 1])
                return iValue;

            //Get x/y index
            int xIdx = 0, yIdx = 0;
            xIdx = (int)((x - X[0]) / XDelt);
            yIdx = (int)((y - Y[0]) / YDelt);
            //int i;
            //for (i = 0; i < XNum - 1; i++)
            //{
            //    if (x >= X[i] && x <= X[i + 1])
            //    {
            //        xIdx = i;
            //        break;
            //    }
            //}
            if (xIdx == XNum - 1)
                xIdx = XNum - 2;

            //for (i = 0; i < YNum - 1; i++)
            //{
            //    if (y >= Y[i] && y <= Y[i + 1])
            //    {
            //        yIdx = i;
            //        break;
            //    }
            //}
            if (yIdx == YNum - 1)
                yIdx = YNum - 2;

            int i1 = yIdx;
            int j1 = xIdx;
            int i2 = i1 + 1;
            int j2 = j1 + 1;
            double a = Data[i1, j1];
            double b = Data[i1, j2];
            double c = Data[i2, j1];
            double d = Data[i2, j2];
            double DX = X[1] - X[0];
            double DY = Y[1] - Y[0];
            List<double> dList = new List<double>();
            if (a != MissingValue)
                dList.Add(a);
            if (b != MissingValue)
                dList.Add(b);
            if (c != MissingValue)
                dList.Add(c);
            if (d != MissingValue)
                dList.Add(d);
            
            if (dList.Count == 0)
                return iValue;
            else if (dList.Count == 1)
                iValue = dList[0];
            else if (dList.Count <= 3)
            {
                double aSum = 0;
                foreach (double dd in dList)
                    aSum += dd;
                iValue = aSum / dList.Count;
            }
            else
            {
                double x1val = a + (c - a) * (y - Y[i1]) / DY;
                double x2val = b + (d - b) * (y - Y[i1]) / DY;
                iValue = x1val + (x2val - x1val) * (x - X[j1]) / DX;
            }

            return iValue;
        }

        /// <summary>
        /// Interpolate grid data to a station point
        /// </summary>
        /// <param name="x">x coordinate of the station</param>
        /// <param name="y">y coordinate of the station</param>
        /// <returns>interpolated value</returns>
        public double ToStation_Gaussian(double x, double y)
        {
            double iValue = MissingValue;
            if (x < X[0] || x > X[XNum - 1] || y < Y[0] || y > Y[YNum - 1])
                return iValue;

            //Get x/y index
            int xIdx = (int)((x - X[0]) / XDelt);
            int yIdx = 0;
            int i;
            //for (i = 0; i < XNum - 1; i++)
            //{
            //    if (x >= X[i] && x <= X[i + 1])
            //    {
            //        xIdx = i;
            //        break;
            //    }
            //}
            if (xIdx == XNum - 1)
                xIdx = XNum - 2;

            for (i = 0; i < YNum - 1; i++)
            {
                if (y >= Y[i] && y <= Y[i + 1])
                {
                    yIdx = i;
                    break;
                }
            }
            if (yIdx == YNum - 1)
                yIdx = YNum - 2;

            int i1 = yIdx;
            int j1 = xIdx;
            int i2 = i1 + 1;
            int j2 = j1 + 1;
            double a = Data[i1, j1];
            double b = Data[i1, j2];
            double c = Data[i2, j1];
            double d = Data[i2, j2];
            double DX = X[1] - X[0];
            double DY = Y[1] - Y[0];
            List<double> dList = new List<double>();
            if (a != MissingValue)
                dList.Add(a);
            if (b != MissingValue)
                dList.Add(b);
            if (c != MissingValue)
                dList.Add(c);
            if (d != MissingValue)
                dList.Add(d);

            if (dList.Count == 0)
                return iValue;
            else if (dList.Count == 1)
                iValue = dList[0];
            else if (dList.Count <= 3)
            {
                double aSum = 0;
                foreach (double dd in dList)
                    aSum += dd;
                iValue = aSum / dList.Count;
            }
            else
            {
                double x1val = a + (c - a) * (y - Y[i1]) / DY;
                double x2val = b + (d - b) * (y - Y[i1]) / DY;
                iValue = x1val + (x2val - x1val) * (x - X[j1]) / DX;
            }

            return iValue;
        }

        /// <summary>
        /// Interpolate grid data to station data
        /// </summary>
        /// <param name="inStData">input station data</param>
        /// <returns>interpolated station data</returns>
        public StationData ToStation(StationData inStData)
        {
            StationData outStData = new StationData(inStData);
            for (int i = 0; i < outStData.StNum; i++)
            {
                outStData.Data[2, i] = ToStation(outStData.Data[0, i], outStData.Data[1, i]);                
            }

            return outStData;
        }

        /// <summary>
        /// Interpolate grid data to stations imported from station file
        /// </summary>
        /// <param name="inFile">input station file</param>
        /// <param name="outFile">output station file</param>
        public void ToStation(string inFile, string outFile)
        {
            if (!File.Exists(inFile))
                return;

            StreamReader sr = new StreamReader(inFile, Encoding.Default);
            StreamWriter sw = new StreamWriter(outFile, false, Encoding.Default);

            //Header
            string aLine = sr.ReadLine();
            aLine = aLine + ",data";
            sw.WriteLine(aLine);

            //Data
            string[] dArray;
            string stId;
            double x, y;
            double value;
            while (sr.Peek() >= 0)
            {
                aLine = sr.ReadLine();
                dArray = aLine.Split(',');
                if (dArray.Length < 3)
                    continue;

                stId = dArray[0].Trim();
                x = double.Parse(dArray[1].Trim());
                y = double.Parse(dArray[2].Trim());
                value = ToStation(x, y);
                if (!double.IsNaN(value))
                {
                    aLine = aLine + "," + value.ToString();
                    sw.WriteLine(aLine);
                }
            }

            sr.Close();
            sw.Close();
        }

        /// <summary>
        /// Regrid data with double linear interpolation method
        /// </summary>
        /// <param name="gridData">output grid data</param>
        public void Regrid(GridData gridData)
        {
            for (int i = 0; i < gridData.YNum; i++)
            {
                for (int j = 0; j < gridData.XNum; j++)
                {
                    gridData.Data[i, j] = ToStation(gridData.X[j], gridData.Y[i]);
                }
            }
        }

        /// <summary>
        /// Set constant value
        /// </summary>
        /// <param name="aValue">value</param>
        public void SetValue(double aValue)
        {
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                {
                    Data[i, j] = aValue;
                }
            }
        }

        /// <summary>
        /// Set grid value by vectorlayer - the grid point was assigned a value of
        /// the index of the shape where the point is located in
        /// </summary>
        /// <param name="aLayer">Vector layer</param>
        /// <returns>New grid data</returns>
        public GridData SetValue(VectorLayer aLayer)
        {
            if (aLayer.ShapeType != MeteoInfoC.Shape.ShapeTypes.Polygon)
            {
                MessageBox.Show("The mask layer is not a polygon layer!" + Environment.NewLine + aLayer.FileName, "Error");
                return this;
            }

            GridData cGrid = new GridData();
            cGrid.X = X;
            cGrid.Y = Y;
            cGrid.Data = new double[YNum, XNum];
            cGrid.MissingValue = MissingValue;
            int idx;
            for (int i = 0; i < YNum; i++)
            {
                if (Y[i] >= aLayer.Extent.minY && Y[i] <= aLayer.Extent.maxY)
                {                    
                    for (int j = 0; j < XNum; j++)
                    {
                        cGrid.Data[i, j] = MissingValue;
                        if (X[j] >= aLayer.Extent.minX && X[j] <= aLayer.Extent.maxX)
                        {
                            for (idx = 0; idx < aLayer.ShapeNum; idx++)
                            {
                                PolygonShape aPGS = (PolygonShape)aLayer.ShapeList[idx];
                                if (Global.MIMath.PointInPolygon(aPGS, X[j], Y[i]))
                                {
                                    cGrid.Data[i, j] = idx;
                                    break;
                                }
                            }
                        }                            
                    }
                }
                else
                {
                    for (int j = 0; j < XNum; j++)
                    {
                        cGrid.Data[i, j] = MissingValue;
                    }
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Set grid value by vectorlayer - the grid point was assigned a value of
        /// the field value of the shape where the point is located in
        /// </summary>
        /// <param name="aLayer">Vector layer</param>
        /// <param name="fieldName">Field name - must be the double or int field</param>
        /// <returns>New grid data</returns>
        public GridData SetValue(VectorLayer aLayer, string fieldName)
        {
            if (aLayer.ShapeType != MeteoInfoC.Shape.ShapeTypes.Polygon)
            {
                MessageBox.Show("The mask layer is not a polygon layer!" + Environment.NewLine + aLayer.FileName, "Error");
                return this;
            }

            GridData cGrid = new GridData();
            cGrid.X = X;
            cGrid.Y = Y;
            cGrid.Data = new double[YNum, XNum];
            cGrid.MissingValue = MissingValue;
            int idx;
            for (int i = 0; i < YNum; i++)
            {
                if (Y[i] >= aLayer.Extent.minY && Y[i] <= aLayer.Extent.maxY)
                {
                    for (int j = 0; j < XNum; j++)
                    {
                        cGrid.Data[i, j] = MissingValue;
                        if (X[j] >= aLayer.Extent.minX && X[j] <= aLayer.Extent.maxX)
                        {
                            for (idx = 0; idx < aLayer.ShapeNum; idx++)
                            {
                                PolygonShape aPGS = (PolygonShape)aLayer.ShapeList[idx];
                                if (Global.MIMath.PointInPolygon(aPGS, X[j], Y[i]))
                                {
                                    cGrid.Data[i, j] = double.Parse(aLayer.GetCellValue(fieldName, idx).ToString());
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < XNum; j++)
                    {
                        cGrid.Data[i, j] = MissingValue;
                    }
                }
            }

            return cGrid;
        }

        /// <summary>
        /// Replace grid data value by a threshold - the values bigger/smaller than the threshold value
        /// will be replaced by the new value
        /// </summary>
        /// <param name="aValue">threshold value</param>
        /// <param name="bValue">new value</param>
        /// <param name="bigger">bigger or smaller</param>
        public void ReplaceValue(double aValue, double bValue, bool bigger)
        {
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                {
                    if (bigger)
                    {
                        if (Data[i, j] > aValue)
                            Data[i, j] = bValue;
                    }
                    else
                    {
                        if (Data[i, j] < aValue)
                            Data[i, j] = bValue;
                    }
                }
            }
        }

        /// <summary>
        /// Un stag grid data through x dimension
        /// </summary>
        /// <returns>Un stagged grid data</returns>
        public GridData UnStagger_X()
        {
            int xn = this.XNum;
            int yn = this.YNum;
            double dx = this.XDelt;
            int xn_us = xn - 1;
            int i, j;
            GridData usData = new GridData(this);
            usData.X = new double[xn_us];
            usData.Data = new double[yn, xn_us];
            for (i = 0; i < yn; i++)
            {
                for (j = 0; j < xn_us; j++)
                {
                    if (i == 0)
                        usData.X[j] = X[j] + dx;
                    usData.Data[i, j] = (this.Data[i, j] + this.Data[i, j + 1]) * 0.5;
                }
            }

            return usData;
        }

        /// <summary>
        /// Un stag grid data through y dimension
        /// </summary>
        /// <returns>Un stagged grid data</returns>
        public GridData UnStagger_Y()
        {
            int xn = this.XNum;
            int yn = this.YNum;
            double dy = this.YDelt;
            int yn_us = yn - 1;
            int i, j;
            GridData usData = new GridData(this);
            usData.Y = new double[yn_us];
            usData.Data = new double[yn_us, xn];
            for (i = 0; i < yn_us; i++)
            {
                for (j = 0; j < xn; j++)
                {
                    if (j == 0)
                        usData.Y[i] = Y[i] + dy;
                    usData.Data[i, j] = (this.Data[i, j] + this.Data[i + 1, j]) * 0.5;
                }
            }

            return usData;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>grid data</returns>
        public object Clone()
        {
            GridData newGridData = new GridData(this);
            newGridData.Data = Data;

            return newGridData;
        }

        #endregion

        #region Convert data
        /// <summary>
        /// Extend the grid data to global by add a new column data
        /// </summary>
        public void ExtendToGlobal()
        {
            double[,] newGridData = new double[YNum, XNum + 1];
            double[] newX = new double[XNum + 1];
            int i, j;
            for (i = 0; i < XNum; i++)
                newX[i] = X[i];

            newX[XNum] = newX[XNum - 1] + XDelt;
            for (i = 0; i < YNum; i++)
            {
                for (j = 0; j < XNum; j++)
                {
                    newGridData[i, j] = Data[i, j];
                }
                newGridData[i, XNum] = newGridData[i, 0];
            }

            Data = newGridData;
            X = newX;
        }

        /// <summary>
        /// Extend grid coordinates
        /// </summary>
        /// <param name="leftNum">Left extent number</param>
        /// <param name="rightNum">Right extent number</param>
        /// <param name="topNum">Top extent number</param>
        /// <param name="bottomNum">Bottom extent number</param>
        public void Extend(int leftNum, int rightNum, int topNum, int bottomNum)
        {
            int newXNum = XNum + leftNum + rightNum;
            int newYNum = YNum + topNum + bottomNum;
            double[,] newGridData = new double[newYNum, newXNum];
            double[] newX = new double[newXNum];
            double[] newY = new double[newYNum];
            int i, j;
            double sX = X[0] - leftNum * XDelt;
            double sY = Y[0] - bottomNum * YDelt;
            for (i = 0; i < newXNum; i++)
                newX[i] = sX + i * XDelt;

            for (i = 0; i < newYNum; i++)
                newY[i] = sY + i * YDelt;

            for (i = 0; i < newYNum; i++)
            {
                if (i < bottomNum || i >= bottomNum + YNum)
                {
                    for (j = 0; j < newXNum; j++)
                        newGridData[i, j] = MissingValue;
                }
                else
                {
                    for (j = 0; j < newXNum; j++)
                    {
                        if (j < leftNum || j >= leftNum + XNum)
                            newGridData[i, j] = MissingValue;
                        else
                            newGridData[i, j] = Data[i - bottomNum, j - leftNum];
                    }
                }
            }

            Data = newGridData;
            X = newX;
            Y = newY;
        }

        /// <summary>
        /// Shift global grid data from (0 - 360 degree) to (-180 - 180 degree)
        /// </summary>
        public void ShiftTo180()
        {
            if (IsGlobal)
            {
                if (X[XNum - 1] > 180)
                {
                    int i, idx = 0;
                    double[] newX = new double[XNum];
                    double[,] newGridData = new double[YNum, XNum];
                    for (i = 0; i < XNum; i++)
                    {
                        if (X[i] >= 180)
                        {
                            idx = i;
                            break;
                        }
                    }
                    for (i = idx; i < XNum; i++)
                        newX[i - idx] = X[i] - 360;
                    for (i = 0; i < idx; i++)
                        newX[XNum - idx + i] = X[i];

                    for (int j = 0; j < YNum; j++)
                    {
                        for (i = idx; i < XNum; i++)
                            newGridData[j, i - idx] = Data[j, i];
                        for (i = 0; i < idx; i++)
                            newGridData[j, XNum - idx + i] = Data[j, i];
                    }

                    Data = newGridData;
                    X = newX;
                }
            }
        }

        /// <summary>
        /// Union grid data - latter grid will overlap ahead grid, the latter undefine data will not overlap ahead data.
        /// All grid data should have same X/Y coordinate value
        /// </summary>
        /// <param name="gridData">grid data</param>
        public void Union(GridData gridData)
        {
            for (int m = 0; m < YNum; m++)
            {
                for (int n = 0; n < XNum; n++)
                {
                    if (!MIMath.DoubleEquals(gridData.Data[m, n], gridData.MissingValue))
                        Data[m, n] = gridData.Data[m, n];
                }
            }
        }

        /// <summary>
        /// Convert grid two dimension data to one dimension data
        /// </summary>
        /// <returns>one dimension data array</returns>
        public double[] ToOneDimData()
        {
            double[] data = new double[YNum * XNum];
            int ii;
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                {
                    ii = i * XNum + j;
                    data[ii] = Data[i, j];
                }
            }

            return data;
        }

        #endregion

        #region Projection
        /// <summary>
        /// Project grid data
        /// </summary>
        /// <param name="fromProj">from projection</param>
        /// <param name="toProj">to projection</param>
        /// <returns>projected grid data</returns>
        public GridData Project(ProjectionInfo fromProj, ProjectionInfo toProj)
        {
            GridData pGridData = new GridData(this);
            pGridData.Data = Data;
            Extent aExtent = new Extent();
            if (pGridData.IsGlobal || pGridData.X[pGridData.XNum -1] - pGridData.X[0] == 360)
                aExtent = ProjectionManage.GetProjectionGlobalExtent(toProj);
            else
                aExtent = ProjectionManage.GetProjectionExtent(fromProj, toProj, pGridData.X, pGridData.Y);

            double xDelt = (aExtent.maxX - aExtent.minX) / (pGridData.XNum - 1);
            double yDelt = (aExtent.maxY - aExtent.minY) / (pGridData.YNum - 1);
            double[] newX = new double[pGridData.XNum];
            double[] newY = new double[pGridData.YNum];
            for (int i = 0; i < newX.Length; i++)
                newX[i] = aExtent.minX + i * xDelt;

            for (int i = 0; i < newY.Length; i++)
                newY[i] = aExtent.minY + i * yDelt;

            pGridData.Project(fromProj, toProj, newX, newY);

            return pGridData;
        }

        /// <summary>
        /// Project grid data
        /// </summary>
        /// <param name="fromProj">from projection info</param>
        /// <param name="toProj">to projection info</param>
        /// <param name="newX">new X coordinates</param>
        /// <param name="newY">new Y coordinates</param>
        public void Project(ProjectionInfo fromProj, ProjectionInfo toProj, double[] newX, double[] newY)
        {
            PointD[,] pos = new PointD[newY.Length, newX.Length];
            double[,] newData = new double[newY.Length, newX.Length];
            int i, j, xIdx, yIdx;
            double x, y;
            //double[][] points = new double[newY.Length * newX.Length][];
            //for (i = 0; i < newY.Length; i++)
            //{
            //    for (j = 0; j < newX.Length; j++)
            //    {
            //        points[i * newY.Length + j] = new double[] { newX[j], newY[i] };                    
            //    }
            //}
            //try
            //{
            //    Reproject.ReprojectPoints(points, toProj, fromProj, 0, points.GetLength(0));
            //    for (i = 0; i < newY.Length; i++)
            //    {
            //        for (j = 0; j < newX.Length; j++)
            //        {
            //            x = points[i * newY.Length + j][0];
            //            y = points[i * newY.Length + j][1];
            //            pos[i, j] = new PointD(x, y);
            //            if (x < X[0] || x > X[X.Length - 1])
            //                newData[i, j] = MissingValue;
            //            else if (y < Y[0] || y > Y[Y.Length - 1])
            //                newData[i, j] = MissingValue;
            //            else
            //            {
            //                xIdx = (int)((x - X[0]) / XDelt);
            //                yIdx = (int)((y - Y[0]) / YDelt);
            //                newData[i, j] = Data[xIdx, yIdx];
            //            }
            //        }
            //    }
            //}
            //catch { }

            double[][] points = new double[1][];
            for (i = 0; i < newY.Length; i++)
            {
                for (j = 0; j < newX.Length; j++)
                {
                    points[0] = new double[] { newX[j], newY[i] };
                    try
                    {
                        Reproject.ReprojectPoints(points, toProj, fromProj, 0, 1);
                        x = points[0][0];
                        y = points[0][1];
                        if (x < X[0] || x > X[X.Length - 1])
                            newData[i, j] = MissingValue;
                        else if (y < Y[0] || y > Y[Y.Length - 1])
                            newData[i, j] = MissingValue;
                        else
                        {
                            xIdx = (int)((x - X[0]) / XDelt);
                            yIdx = (int)((y - Y[0]) / YDelt);
                            newData[i, j] = Data[yIdx, xIdx];
                        }
                    }
                    catch 
                    {
                        newData[i, j] = MissingValue;
                        j++;
                        continue;
                    }
                }
            }

            this.Data = newData;
            this.X = newX;
            this.Y = newY;
        }

        #endregion

        #region Gaussian grid
        /// <summary>
        /// Convert Gassian grid to lat/lon grid
        /// </summary>
        public void GassianToLatLon()
        {
            double ymin = Y[0];
            double ymax = Y[Y.Length - 1];
            double delta = (ymax - ymin) / (YNum - 1);
            double[] newY = new double[YNum];
            for (int i = 0; i < YNum; i++)
                newY[i] = ymin + i * delta;

            double[,] newData = new double[YNum, XNum];
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                {
                    newData[i, j] = ToStation_Gaussian(X[j], newY[i]);
                }
            }

            this.Y = newY;
            this.Data = newData;
        }

        /// <summary>
        /// Convert Gassian grid to lat/lon grid - only convert Y coordinate
        /// </summary>
        public void GassianToLatLon_Simple()
        {
            double ymin = Y[0];
            double ymax = Y[Y.Length - 1];
            double delta = (ymax - ymin) / (YNum - 1);
            double[] newY = new double[YNum];
            for (int i = 0; i < YNum; i++)
                newY[i] = ymin + i * delta;

            this.Y = newY;
        }

        #endregion

        #endregion
    }
}
