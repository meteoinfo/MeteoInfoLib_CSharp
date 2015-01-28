using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.ComponentModel;
using MeteoInfoC.Global;
using MeteoInfoC.Legend;
using MeteoInfoC.Data;
using MeteoInfoC.Data.MeteoData;
using MeteoInfoC.Shape;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// Raster layer
    /// </summary>
    public class RasterLayer:ImageLayer
    {
        #region Variables
        private LegendScheme _legendScheme;
        private GridData _gridData;
        private GridData _originGridData = null;
        private bool _isProjected = false;
        private InterpolationMode _interpMode = InterpolationMode.NearestNeighbor;

        #endregion               

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public RasterLayer()
        {
            //m_handle = -1;
            //m_layerName = "";
            //m_fileName = "";
            //m_visible = true;
            //m_IsMaskout = false;
            LayerType = LayerTypes.RasterLayer;
            ShapeType = ShapeTypes.Image;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get or set legend scheme
        /// </summary>
        public override LegendScheme LegendScheme
        {
            get { return _legendScheme; }
            set
            {
                _legendScheme = value;
                if (_legendScheme.BreakNum < 50)
                {
                    //UpdateImage();
                    UpdateImage(_legendScheme);
                }
                else
                    SetPaletteByLegend();
            }
        }

        /// <summary>
        /// Get or set grid data array
        /// </summary>
        public GridData GridData
        {
            get { return _gridData; }
            set 
            {
                _gridData = value;
                UpdateGridData();
            }
        }

        /// <summary>
        /// Get or set if is projected
        /// </summary>
        public bool IsProjected
        {
            get { return _isProjected; }
            set { _isProjected = value; }
        }

        /// <summary>
        /// Get or set interpolation mode
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set image interpolation mode")]
        public InterpolationMode InterpMode
        {
            get { return _interpMode; }
            set 
            { 
                _interpMode = value;
                if (_interpMode == InterpolationMode.Invalid)
                    _interpMode = InterpolationMode.NearestNeighbor;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update origin data
        /// </summary>
        public void UpdateOriginData()
        {
            _originGridData = (GridData)_gridData.Clone();
            _isProjected = true;
        }

        /// <summary>
        /// Get origin data
        /// </summary>
        public void GetOriginData()
        {
            _gridData = (GridData)_originGridData.Clone();
        }

        /// <summary>
        /// Get cell value by a point
        /// </summary>
        /// <param name="iIdx">i index</param>
        /// <param name="jIdx">j index</param>
        /// <returns>cell value</returns>
        public double GetCellValue(int iIdx, int jIdx)
        {
            return GridData.Data[iIdx, jIdx];
        }

        /// <summary>
        /// Set color palette to a image from a palette file
        /// </summary>
        /// <param name="aFile">file path</param>        
        public new void SetPalette(string aFile)
        {
            base.SetPalette(aFile);
            _legendScheme = new LegendScheme(ShapeTypes.Image);
            _legendScheme.ImportFromPaletteFile_Unique(aFile);
        }

        /// <summary>
        /// Set color palette by legend scheme
        /// </summary>
        public void SetPaletteByLegend()
        {
            List<Color> colors = _legendScheme.GetColors();
            SetPalette(colors);
        }

        /// <summary>
        /// Update image
        /// </summary>
        public void UpdateImage()
        {
            int xNum = GridData.XNum;
            int yNum = GridData.YNum;
            byte[] imageBytes = new byte[xNum * yNum];
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    int value = -1;
                    int b;
                    for (b = 0; b < LegendScheme.LegendBreaks.Count - 1; b++)
                    {
                        ColorBreak aCB = LegendScheme.LegendBreaks[b];
                        if (aCB.StartValue.ToString() == aCB.EndValue.ToString())
                        {
                            if (GridData.Data[i, j] == double.Parse(aCB.StartValue.ToString()))
                            {
                                value = b;
                                break;
                            }
                        }
                        else
                        {
                            if (GridData.Data[i, j] >= double.Parse(aCB.StartValue.ToString()) && GridData.Data[i, j] < double.Parse(aCB.EndValue.ToString()))
                            {
                                value = b;
                                break;
                            }
                        }
                    }
                    if (value == -1)
                    {
                        value = b;
                        if (LegendScheme.LegendBreaks[LegendScheme.BreakNum - 1].IsNoData)
                        {
                            if (!MIMath.DoubleEquals(GridData.Data[i, j], LegendScheme.MissingValue))
                                value = b - 1;
                        }
                    }
                    imageBytes[i * xNum + j] = (byte)value;
                }
            }

            Image = DrawMeteoData.CreateBitmap(imageBytes, xNum, yNum);
            List<Color> colors = LegendScheme.GetColors();
            DrawMeteoData.SetPalette(colors, Image);
        }

        /// <summary>
        /// Update image by legend scheme
        /// </summary>
        /// <param name="als">The legend scheme</param>
        public void UpdateImage(LegendScheme als)
        {
            Image image = getImageFromGridData(_gridData, als);
            this.Image = image;
        }

        private Image getImageFromGridData(GridData gdata, LegendScheme als)
        {
            int width, height, breakNum;
            width = gdata.XNum;
            height = gdata.YNum;
            breakNum = als.BreakNum;
            double[] breakValue = new double[breakNum];
            Color[] breakColor = new Color[breakNum];
            Color undefColor = Color.White;
            for (int i = 0; i < breakNum; i++)
            {
                breakValue[i] = Convert.ToDouble(als.LegendBreaks[i].EndValue);
                breakColor[i] = als.LegendBreaks[i].Color;
                if (als.LegendBreaks[i].IsNoData)
                    undefColor = als.LegendBreaks[i].Color;
            }
            Color defaultColor = breakColor[breakNum - 1];    //Set default as last color
            Bitmap aBitmap = new Bitmap(width, height);
            aBitmap.MakeTransparent();
            double oneValue;
            Color oneColor = defaultColor;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    oneValue = gdata.Data[i, j];
                    if (MIMath.DoubleEquals(oneValue, gdata.MissingValue))
                    {
                        oneColor = undefColor;
                    }
                    else
                    {
                        oneColor = defaultColor;                        
                        for (int k = 0; k < breakNum - 1; k++)
                        {
                            if (oneValue < breakValue[k])
                            {
                                oneColor = breakColor[k];
                                break;
                            }
                        }
                    }
                    aBitmap.SetPixel(j, height - i - 1, oneColor);
                }
            }
            Image aImage = (Image)aBitmap;
            return aImage;
        }

        private Image getImageFromGridData_Bak1(GridData gdata, LegendScheme als)
        {
            int width, height, breakNum;
            width = gdata.XNum;
            height = gdata.YNum;
            breakNum = als.BreakNum;            
            Color defaultColor = als.LegendBreaks[breakNum - 1].Color;    //默认颜色为最后一个颜色
            Bitmap aBitmap = new Bitmap(width, height);
            aBitmap.MakeTransparent();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    double oneValue = gdata.Data[i, j];
                    Color oneColor = defaultColor;
                    for (int k = 0; k < breakNum; k++)
                    {
                        if (als.LegendBreaks[k].StartValue.ToString() == als.LegendBreaks[k].EndValue.ToString())
                        {
                            if (MIMath.DoubleEquals(oneValue, Double.Parse(als.LegendBreaks[k].EndValue.ToString())))
                            {
                                oneColor = als.LegendBreaks[k].Color;
                                break;
                            }
                        }
                        else
                        {
                            if (oneValue >= Double.Parse(als.LegendBreaks[k].StartValue.ToString()) && oneValue < Double.Parse(als.LegendBreaks[k].EndValue.ToString()))
                            {
                                oneColor = als.LegendBreaks[k].Color;
                                break;
                            }
                        }
                    }
                    aBitmap.SetPixel(j, height - i - 1, oneColor);
                }
            }
            Image aImage = (Image)aBitmap;
            return aImage;
        }

        private Image getImageFromGridData_Bak(GridData gdata, LegendScheme als)
        {
            int width,height,breakNum;
            width = gdata.XNum;
            height = gdata.YNum;
            breakNum = als.BreakNum;
            double[] breakValue = new double[breakNum];
            Color[] breakColor = new Color[breakNum];
            for (int i = 0; i < breakNum; i++)
            {
                breakValue[i] = Convert.ToDouble(als.LegendBreaks[i].EndValue);
                breakColor[i] = als.LegendBreaks[i].Color;
            }
            Color defaultColor = breakColor[breakNum-1];    //默认颜色为最后一个颜色
            Bitmap aBitmap = new Bitmap(width, height);
            aBitmap.MakeTransparent();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    double oneValue = gdata.Data[i, j];
                    Color oneColor = defaultColor;
                    //循环只到breakNum-1 是因为最后一个LegendBreaks的EndValue和StartValue是一样的
                    for (int k = 0; k < breakNum - 1; k++)    
                    {
                        if (oneValue < breakValue[k])
                        {
                            oneColor = breakColor[k];
                            break;
                        }
                    }
                    aBitmap.SetPixel(j, height-i-1, oneColor);
                }
            }
            Image aImage = (Image)aBitmap;
            return aImage;
        }

        /// <summary>
        /// Set image by grid data
        /// </summary>
        public void SetImageByGridData()
        {
            ColorPalette cPal = null;
            if (Image != null)
                cPal = Image.Palette;

            int xNum = GridData.XNum;
            int yNum = GridData.YNum;
            byte[] imageBytes = new byte[xNum * yNum];
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    imageBytes[i * xNum + j] = (byte)GridData.Data[i, j];
                }
            }

            Image = DrawMeteoData.CreateBitmap(imageBytes, xNum, yNum);
            if (cPal != null)
                Image.Palette = cPal;
        }

        private void UpdateExtent()
        {
            double XBR, YBR;
            XBR = _gridData.XNum * WorldFileParaV.XScale + WorldFileParaV.XUL;
            YBR = _gridData.YNum * WorldFileParaV.YScale + WorldFileParaV.YUL;
            Extent aExtent = new Extent();
            aExtent.minX = WorldFileParaV.XUL;
            aExtent.minY = YBR;
            aExtent.maxX = XBR;
            aExtent.maxY = WorldFileParaV.YUL;
            this.Extent = aExtent;
        }

        /// <summary>
        /// Update grid data
        /// </summary>
        public void UpdateGridData()
        {
            WorldFilePara aWFP = new WorldFilePara();

            //aWFP.XUL = _gridData.X[0];
            //aWFP.YUL = _gridData.Y[GridData.YNum - 1];
            aWFP.XUL = _gridData.X[0] - _gridData.XDelt / 2;
            aWFP.YUL = _gridData.Y[_gridData.YNum - 1] + _gridData.YDelt / 2;
            aWFP.XScale = _gridData.XDelt;
            aWFP.YScale = -_gridData.YDelt;

            aWFP.XRotate = 0;
            aWFP.YRotate = 0;

            WorldFileParaV = aWFP;

            UpdateExtent();
        }

        /// <summary>
        /// Override get custom property method
        /// </summary>
        /// <returns>property object</returns>
        public override object GetPropertyObject()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("LayerType", "LayerType");
            //objAttr.Add("ShapeType", "ShapeType");
            objAttr.Add("Handle", "Handle");
            objAttr.Add("LayerName", "LayerName");
            objAttr.Add("FileName", "FileName");
            objAttr.Add("Visible", "Visible");
            //objAttr.Add("LayerDrawType", "LayerDrawType");
            objAttr.Add("IsMaskout", "IsMaskout");
            //objAttr.Add("TransparencyPerc", "TransparencyPerc");
            objAttr.Add("InterpMode", "InterpMode");
            CustomProperty cp = new CustomProperty(this, objAttr);

            return cp;
        }

        #endregion
    }
}
