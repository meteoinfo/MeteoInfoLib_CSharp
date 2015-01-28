using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using MeteoInfoC.Layer;
using MeteoInfoC.Legend;
using MeteoInfoC.Shape;
using MeteoInfoC.Drawing;

namespace MeteoInfoC.Layout
{
    #region Structure
    /// <summary>
    /// Legend parameters
    /// </summary>
    public struct legendPara
    {
        /// <summary>
        /// Is vertical bar legend
        /// </summary>
        public bool isVertical;
        /// <summary>
        /// Legend start point
        /// </summary>
        public PointD startPoint;
        /// <summary>
        /// Legend length
        /// </summary>
        public double length;
        /// <summary>
        /// Legend width
        /// </summary>
        public double width;
        /// <summary>
        /// Legend contour values
        /// </summary>
        public double[] contourValues;
        /// <summary>
        /// If the first and last legend polygons are triangle
        /// </summary>
        public bool isTriangle;
    }

    /// <summary>
    /// Legend polygon
    /// </summary>
    public struct lPolygon
    {
        /// <summary>
        /// Value
        /// </summary>
        public double value;
        /// <summary>
        /// Is first polygon
        /// </summary>
        public bool isFirst;
        /// <summary>
        /// Point list
        /// </summary>
        public List<PointD> pointList;
    }

    #endregion

    /// <summary>
    /// Legend styles
    /// </summary>
    public enum LegendStyles
    {
        /// <summary>
        /// vertical bar
        /// </summary>
        Bar_Vertical,
        /// <summary>
        /// horizontal bar
        /// </summary>
        Bar_Horizontal,
        /// <summary>
        /// normal
        /// </summary>
        Normal
    }

    /// <summary>
    /// Layer update types
    /// </summary>
    public enum LayerUpdateTypes
    {
        /// <summary>
        /// Not update
        /// </summary>
        NotUpdate,
        /// <summary>
        /// First meteorological layer
        /// </summary>
        FirstMeteoLayer,
        /// <summary>
        /// Last added layer
        /// </summary>
        LastAddedLayer,
        /// <summary>
        /// First expanded layer
        /// </summary>
        FirstExpandedLayer
    }

    /// <summary>
    /// Layout legend
    /// </summary>
    public class LayoutLegend:LayoutElement
    {
        #region Internal classes
        internal class LayerNameConverter : StringConverter
        {
            /// <summary>
            /// Override GetStandardVaulesSupported method
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            /// <summary>
            /// Override GetStandardValues method
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                LayoutMap aLM = ((LayoutLegend)((CustomProperty)context.Instance).CurrentSelectObject).LayoutMap;
                List<string> layerNames = new List<string>();
                foreach (MapLayer aLayer in aLM.MapFrame.MapView.LayerSet.Layers)
                {
                    if (aLayer.LayerType != LayerTypes.ImageLayer)
                        layerNames.Add(aLayer.LayerName);
                }
                return new StandardValuesCollection(layerNames);
            }
        }

        #endregion

        #region Variables

        private MapLayout _mapLayout;
        private LayoutMap _layoutMap;

        //private bool _drawLegend;
        private LegendScheme _legendScheme = null;
        private ChartBreak _chartBreak = null;
        private MapLayer _legendLayer = null;
        private bool _drawChart = false;
        private SmoothingMode _smoothingMode;
        //private LegendSet m_LegendSet;
        private LayerUpdateTypes _layerUpdateType;
        private LegendStyles _legendStyle;        
        private string _title;
        private Font _font;
        private Font _titleFont;
        private bool _drawNeatLine;
        private Color _neatLineColor;
        private int _neatLineSize;
        private float _breakSpace;
        private float _topSpace;
        private float _leftSpace;
        private float _vBarWidth;
        private float _hBarHeight;
        private int _columnNum = 1;

        #endregion        

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapLayout">map layout</param>
        /// <param name="layoutMap">layout map</param>
        public LayoutLegend(MapLayout mapLayout, LayoutMap layoutMap):base()
        {
            ElementType = ElementType.LayoutLegend;
            ResizeAbility = ResizeAbility.ResizeAll;

            _mapLayout = mapLayout;          
            _layoutMap = layoutMap;
            _layoutMap.MapViewUpdated += new EventHandler(LayoutMap_MapViewUpdated);
            _smoothingMode = SmoothingMode.AntiAlias;
            _layerUpdateType = LayerUpdateTypes.FirstMeteoLayer;
            _legendStyle = LegendStyles.Normal;            
            _title = "";
            _drawNeatLine = false;
            _neatLineColor = Color.Black;
            _neatLineSize = 1;
            _breakSpace = 3;
            _topSpace = 5;
            _leftSpace = 5;
            _vBarWidth = 10;
            _hBarHeight = 10;
            _font = new Font("Arial", 7);
            _titleFont = new Font("Arial", 8);            
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get layout map
        /// </summary>
        public LayoutMap LayoutMap
        {
            get { return _layoutMap; }
        }

        ///// <summary>
        ///// Get or set if draw legend
        ///// </summary>
        //[CategoryAttribute("Property"), DescriptionAttribute("If draw legend")]
        //public bool DrawLegend
        //{
        //    get { return _drawLegend; }
        //    set { _drawLegend = value; }
        //}

        /// <summary>
        /// Get or set legend scheme
        /// </summary>
        [CategoryAttribute("Property"), DescriptionAttribute("Legend scheme")]
        public LegendScheme LegendScheme
        {
            get { return _legendScheme; }
            set 
            { 
                _legendScheme = value;
                UpdateLegendSize();
            }
        }

        /// <summary>
        /// Get or set legend layer
        /// </summary>
        [CategoryAttribute("Property"), DescriptionAttribute("Legend layer")]
        public MapLayer LegendLayer
        {
            get { return _legendLayer; }
            set 
            { 
                _legendLayer = value;
                _legendScheme = _legendLayer.LegendScheme;
                if (_legendLayer.LayerType == LayerTypes.VectorLayer)
                {
                    _drawChart = ((VectorLayer)_legendLayer).ChartSet.DrawCharts;
                    if (((VectorLayer)_legendLayer).ChartPoints.Count > 0)
                        _chartBreak = ((ChartBreak)((VectorLayer)_legendLayer).ChartPoints[0].Legend).GetSampleChartBreak();
                }
                string aStr = _legendLayer.LayerName;
                if (aStr.Contains("_"))
                    aStr = aStr.Split('_')[1];
                _title = aStr;
                UpdateLegendSize();
            }
        }

        /// <summary>
        /// Get or set legend layer name
        /// </summary>
        [CategoryAttribute("Property"), DescriptionAttribute("Legend layer name")]
        [TypeConverter(typeof(LayerNameConverter))]
        public string LayerName
        {
            get { return _legendLayer.LayerName; }
            set
            {
                string lName = value;
                LegendLayer = _layoutMap.MapFrame.MapView.GetLayerFromName(lName); 
            }
        }

        /// <summary>
        /// Get or set SmoothingMode
        /// </summary>
        [CategoryAttribute("Property"), DescriptionAttribute("Smoothing mode")]
        public SmoothingMode SmoothingMode
        {
            get { return _smoothingMode; }
            set { _smoothingMode = value; }
        }

        /// <summary>
        /// Get or set layer update type
        /// </summary>
        [CategoryAttribute("Property"), DescriptionAttribute("Layer update type")]
        public LayerUpdateTypes LayerUpdateType
        {
            get { return _layerUpdateType; }
            set { _layerUpdateType = value; }
        }

        /// <summary>
        /// Get or set legend style
        /// </summary>
        [CategoryAttribute("Property"), DescriptionAttribute("Legend style")]
        public LegendStyles LegendStyle
        {
            get { return _legendStyle; }
            set
            {
                _legendStyle = value;
                //switch (_legendStyle)
                //{
                //    case LegendStyles.Bar_Horizontal:
                //    case LegendStyles.Bar_Vertical:
                //        BackColor = Color.White;
                //        break;
                //}
                if (Visible)
                {
                    UpdateLegendSize();                    
                    //m_MapControl.UpdateControlSize();                    
                }
            }
        }        

        /// <summary>
        /// Get or set legend title
        /// </summary>
        [CategoryAttribute("Property"), DescriptionAttribute("Legend title")]
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                UpdateLegendSize();
                //Refresh();
            }
        }        

        /// <summary>
        /// Get or set if draw neat line
        /// </summary>
        [CategoryAttribute("NeatLine"), DescriptionAttribute("If draw neatline")]
        public bool DrawNeatLine
        {
            get { return _drawNeatLine; }
            set
            {
                _drawNeatLine = value;
                //Refresh();
            }
        }

        /// <summary>
        /// Get or set neat line color
        /// </summary>
        [CategoryAttribute("NeatLine"), DescriptionAttribute("Neatline color")]
        public Color NeatLineColor
        {
            get { return _neatLineColor; }
            set
            {
                _neatLineColor = value;
                //Refresh();
            }
        }

        /// <summary>
        /// Get or set neat line size
        /// </summary>
        [CategoryAttribute("NeatLine"), DescriptionAttribute("Neatline size")]
        public int NeatLineSize
        {
            get { return _neatLineSize; }
            set
            {
                _neatLineSize = value;
                //Refresh();
            }
        }

        /// <summary>
        /// Override Font property
        /// </summary>
        [CategoryAttribute("Property"), DescriptionAttribute("Legend font")]
        public Font Font
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
                _titleFont = new Font(Font.Name, Font.Size + 2);
                UpdateLegendSize();
            }
        }

        /// <summary>
        /// Get or set column number
        /// </summary>
        [CategoryAttribute("Property"), DescriptionAttribute("Column number")]
        public int ColumnNumber
        {
            get { return _columnNum; }
            set 
            { 
                _columnNum = value;
                UpdateLegendSize();
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Paint graphics
        /// </summary>
        /// <param name="g">graphics</param>
        public void PaintGraphics(Graphics g)
        {
            if (_legendScheme == null)
                return;

            Matrix oldMatrix = g.Transform;
            g.TranslateTransform(this.Left, this.Top);
            g.SmoothingMode = _smoothingMode;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;  

            //Draw background color
            g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.Width, this.Height);           

            switch (_legendStyle)
            {
                case LegendStyles.Bar_Horizontal:
                    DrawHorizontalBarLegend(g, 1);
                    break;
                case LegendStyles.Bar_Vertical:
                    DrawVerticalBarLegend(g, 1);
                    break;
                case LegendStyles.Normal:
                    DrawNormalLegend(g, 1);                    
                    break;
            }

            //Draw neatline
            if (_drawNeatLine)
            {
                Rectangle mapRect = new Rectangle(_neatLineSize - 1, _neatLineSize - 1,
                    this.Width - _neatLineSize, this.Height - _neatLineSize);
                g.DrawRectangle(new Pen(_neatLineColor, _neatLineSize), mapRect);
            }

            g.Transform = oldMatrix;
        }

        /// <summary>
        /// Paint graphics
        /// </summary>
        /// <param name="g">graphics</param>
        /// <param name="pageLocation">page location</param>
        /// <param name="zoom">zoom</param>
        public void PaintGraphics(Graphics g, PointF pageLocation, float zoom)
        {
            if (_legendScheme == null)
                return;

            //if (_legendLayer.LayerType == LayerTypes.ImageLayer)
            //    return;

            Matrix oldMatrix = g.Transform;
            PointF aP = PageToScreen(this.Left, this.Top, pageLocation, zoom);
            g.TranslateTransform(aP.X, aP.Y);
            g.Transform.Scale(zoom, zoom);
            g.SmoothingMode = _smoothingMode;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            //Draw background color
            g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.Width * zoom, this.Height * zoom);

            switch (_legendStyle)
            {
                case LegendStyles.Bar_Horizontal:
                    DrawHorizontalBarLegend(g, zoom);
                    break;
                case LegendStyles.Bar_Vertical:
                    DrawVerticalBarLegend(g, zoom);
                    break;
                case LegendStyles.Normal:
                    DrawNormalLegend(g, zoom);
                    break;
            }

            //Draw neatline
            if (_drawNeatLine)
            {
                Rectangle mapRect = new Rectangle(_neatLineSize - 1, _neatLineSize - 1,
                    (int)((this.Width - _neatLineSize) * zoom), (int)((this.Height - _neatLineSize) * zoom));
                g.DrawRectangle(new Pen(_neatLineColor, _neatLineSize), mapRect);
            }

            g.Transform = oldMatrix;
        }

        private void DrawChartLegend(Graphics g, float zoom, PointF aPoint)
        {
            //VectorLayer aLayer = (VectorLayer)_legendLayer;
            //ChartBreak aCB = ((ChartBreak)aLayer.ChartPoints[0].Legend).GetSampleChartBreak();

            //Draw chart symbol
            aPoint.X = 5;
            aPoint.Y += _chartBreak.GetHeight();
            switch (_chartBreak.ChartType)
            {
                case ChartTypes.BarChart:
                    Font lFont = new Font(Font.Name, Font.Size * zoom, Font.Style);
                    Draw.DrawBarChartSymbol(aPoint, _chartBreak, g, true, lFont);
                    break;
                case ChartTypes.PieChart:
                    Draw.DrawPieChartSymbol(aPoint, _chartBreak, g);
                    break;
            }
            aPoint.Y += _breakSpace;

            //Draw breaks
            LegendScheme aLS = _chartBreak.LegendScheme;
            DrawNormalLegend(g, zoom, aLS, ref aPoint, false);
        }

        private void DrawNormalLegend(Graphics g, float zoom)
        {
            //LegendScheme aLS = _legendLayer.LegendScheme;
            PointF aP = new PointF(0, 0);
            DrawNormalLegend(g, zoom, _legendScheme, ref aP, true);
            float height = GetBreakHeight(g) * zoom;
            aP.Y += height + _breakSpace;

            //Draw chart legend
            if (_drawChart)
                DrawChartLegend(g, zoom, aP);            
        }

        private void DrawNormalLegend(Graphics g, float zoom, LegendScheme aLS, ref PointF aP, bool drawTitle)
        {                                                          
            SolidBrush labBrush = new SolidBrush(this.ForeColor);            
            string caption = "";
            SizeF aSF;
            float leftSpace = _leftSpace * zoom;
            float topSpace = _topSpace * zoom;
            float breakSpace = _breakSpace * zoom;
            float height = GetBreakHeight(g) * zoom;
            float width = height * 2;
            float colWidth = GetBreakHeight(g) * 2 + GetLabelWidth(g) + 10;

            //Draw title
            if (drawTitle)
            {
                Font tFont = new Font(_titleFont.Name, _titleFont.Size * zoom, _titleFont.Style);
                string Title = _title;
                aP.X = leftSpace;
                aP.Y = leftSpace;
                aSF = g.MeasureString(Title, tFont);
                Single titleHeight = aSF.Height;
                g.DrawString(_title, tFont, labBrush, aP);
                aP.Y += titleHeight + breakSpace - height / 2;
                tFont.Dispose();
            }
            
            //Set columns
            int[] rowNums = new int[_columnNum];
            int ave = aLS.VisibleBreakNum / _columnNum;
            int num = 0;
            int i;
            for (i = 1; i < _columnNum; i++)
            {
                rowNums[i] = ave;
                num += ave;
            }
            rowNums[0] = aLS.VisibleBreakNum - num;

            //Draw legend                        
            Font lFont = new Font(Font.Name, Font.Size * zoom, Font.Style);
            float sX = aP.X;
            float sY = aP.Y;            
            i = 0;
            for (int col = 0; col < _columnNum; col++)
            {
                aP.X = width / 2 + leftSpace + col * colWidth;
                aP.Y = sY;
                for (int row = 0; row < rowNums[col]; row++)
                {                    
                    if (!aLS.LegendBreaks[i].DrawShape)
                    {
                        i += 1;
                        continue;
                    }
                    aP.Y += height + breakSpace;
                    
                    //bool isVisible = true;
                    switch (aLS.ShapeType)
                    {
                        case ShapeTypes.Point:
                            PointBreak aPB = (PointBreak)((PointBreak)aLS.LegendBreaks[i]).Clone();
                            caption = aPB.Caption;
                            aPB.Size = aPB.Size * zoom;
                            Draw.DrawPoint(aP, aPB, g);
                            break;
                        case ShapeTypes.Polyline:
                        case ShapeTypes.PolylineZ:
                            PolyLineBreak aPLB = (PolyLineBreak)aLS.LegendBreaks[i];
                            caption = aPLB.Caption;
                            Draw.DrawPolyLineSymbol(aP, width, height, aPLB, g);
                            break;
                        case ShapeTypes.Polygon:
                            PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[i];
                            caption = aPGB.Caption;
                            Draw.DrawPolygonSymbol(aP, width, height, aPGB, _legendLayer.TransparencyPerc, g);
                            break;
                        case ShapeTypes.Image:
                            ColorBreak aCB = aLS.LegendBreaks[i];
                            caption = aCB.Caption;
                            Draw.DrawPolygonSymbol(aP, aCB.Color, Color.Black, width,
                                    height, true, true, g);
                            break;
                    }

                    PointF sP = new PointF(0, 0);
                    sP.X = aP.X + width / 2;
                    sP.Y = aP.Y;
                    aSF = g.MeasureString(caption, lFont);
                    g.DrawString(caption, lFont, labBrush, sP.X, sP.Y - aSF.Height / 2);

                    i += 1;
                }
            }

            lFont.Dispose();
            labBrush.Dispose();
        }

        private void DrawVerticalBarLegend(Graphics g, float zoom)
        {
            LegendScheme aLS = _legendScheme;
            PointF aP = new PointF(0, 0);
            PointF sP = new PointF(0, 0);                                                 
            SolidBrush labBrush = new SolidBrush(this.ForeColor);
            bool DrawShape = true, DrawFill = true, DrawOutline = true;
            Color FillColor = Color.Red, OutlineColor = this.ForeColor;
            string caption = "";
            SizeF aSF;

            int bNum = aLS.BreakNum;
            if (aLS.LegendBreaks[bNum - 1].IsNoData)
                bNum -= 1;

            float width = _vBarWidth * zoom;
            float height = (this.Height - 5) * zoom / bNum;
            Font lFont = new Font(Font.Name, Font.Size * zoom, Font.Style);

            for (int i = 0; i < bNum; i++)
            {
                switch (aLS.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)aLS.LegendBreaks[i];
                        //DrawShape = aPB.DrawShape;
                        DrawFill = aPB.DrawFill;
                        //DrawOutline = aPB.drawOutline;
                        FillColor = aPB.Color;
                        //OutlineColor = aPB.outlineColor;
                        if (aLS.LegendType == LegendType.UniqueValue)
                            caption = aPB.Caption;
                        else
                            caption = aPB.EndValue.ToString();
                        break;
                    case ShapeTypes.Polyline:
                        PolyLineBreak aPLB = (PolyLineBreak)aLS.LegendBreaks[i];
                        //DrawShape = aPLB.DrawPolyline;                        
                        FillColor = aPLB.Color;
                        if (aLS.LegendType == LegendType.UniqueValue)
                            caption = aPLB.Caption;
                        else
                            caption = aPLB.EndValue.ToString();
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[i];
                        //DrawShape = aPGB.DrawShape;
                        DrawFill = aPGB.DrawFill;
                        //DrawOutline = aPGB.drawOutline;
                        FillColor = aPGB.Color;
                        //OutlineColor = aPGB.outlineColor;
                        if (aLS.LegendType == LegendType.UniqueValue)
                            caption = aPGB.Caption;
                        else
                            caption = aPGB.EndValue.ToString();
                        break;
                    case ShapeTypes.Image:
                        ColorBreak aCB = aLS.LegendBreaks[i];
                        //DrawShape = true;
                        DrawFill = true;
                        FillColor = aCB.Color;
                        if (aLS.LegendType == LegendType.UniqueValue)
                            caption = aCB.Caption;
                        else
                            caption = aCB.EndValue.ToString();
                        break;                        
                }
                                
                aP.X = width / 2;
                aP.Y = i * height + height / 2;

                if (aLS.LegendType == LegendType.UniqueValue)
                {
                    if (DrawShape)
                    {
                        if (aLS.ShapeType == ShapeTypes.Polygon)
                        {
                            PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[i].Clone();
                            aPGB.DrawOutline = true;
                            aPGB.OutlineColor = Color.Black;
                            if (!aPGB.DrawShape)
                            {
                                aPGB.Color = Color.Transparent;
                            }
                            Draw.DrawPolygonSymbol(aP, width, height, aPGB, _legendLayer.TransparencyPerc, g);
                        }
                        else
                            Draw.DrawPolygonSymbol(aP, FillColor, OutlineColor, width,
                                height, DrawFill, DrawOutline, g);
                    }

                    sP.X = aP.X + width / 2;
                    sP.Y = aP.Y;
                    aSF = g.MeasureString(caption, lFont);
                    g.DrawString(caption, lFont, labBrush, sP.X, sP.Y - aSF.Height / 2);
                }
                else
                {
                    if (DrawShape)
                    {
                        if (i == 0)
                        {
                            PointF[] Points = new PointF[3];
                            Points[0].X = aP.X;
                            Points[0].Y = 0;
                            Points[1].X = 0;
                            Points[1].Y = height;
                            Points[2].X = width;
                            Points[2].Y = height;
                            if (aLS.ShapeType == ShapeTypes.Polygon)
                            {
                                PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[i].Clone();
                                aPGB.DrawOutline = true;
                                aPGB.OutlineColor = Color.Black;
                                if (!aPGB.DrawShape)
                                {
                                    aPGB.Color = Color.Transparent;
                                }
                                Draw.DrawPolygon(Points, aPGB, g);
                            }
                            else
                                Draw.DrawPolygon(Points, FillColor, OutlineColor, width,
                                    height, DrawFill, DrawOutline, g);                         
                        }
                        else if (i == bNum - 1)
                        {
                            PointF[] Points = new PointF[3];
                            Points[0].X = 0;
                            Points[0].Y = i * height;
                            Points[1].X = width;
                            Points[1].Y = i * height;
                            Points[2].X = aP.X;
                            Points[2].Y = i * height + height;
                            if (aLS.ShapeType == ShapeTypes.Polygon)
                            {
                                PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[i].Clone();
                                aPGB.DrawOutline = true;
                                aPGB.OutlineColor = Color.Black;
                                if (!aPGB.DrawShape)
                                {
                                    aPGB.Color = Color.Transparent;
                                }
                                Draw.DrawPolygon(Points, aPGB, g);
                            }
                            else
                                Draw.DrawPolygon(Points, FillColor, OutlineColor, width,
                                    height, DrawFill, DrawOutline, g);
                        }
                        else
                        {
                            if (aLS.ShapeType == ShapeTypes.Polygon)
                            {
                                PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[i].Clone();
                                aPGB.DrawOutline = true;
                                aPGB.OutlineColor = Color.Black;
                                if (!aPGB.DrawShape)
                                {
                                    aPGB.Color = Color.Transparent;
                                }
                                Draw.DrawPolygonSymbol(aP, width, height, aPGB, _legendLayer.TransparencyPerc, g);
                            }
                            else
                                Draw.DrawPolygonSymbol(aP, FillColor, OutlineColor, width,
                                     height, DrawFill, DrawOutline, g);
                        }
                    }

                    sP.X = aP.X + width / 2;
                    sP.Y = aP.Y + height / 2;
                    if (i < bNum - 1)
                    {
                        aSF = g.MeasureString(caption, lFont);
                        g.DrawString(caption, lFont, labBrush, sP.X, sP.Y - aSF.Height / 2);
                    }
                }
            }

            lFont.Dispose();
            labBrush.Dispose();
        }

        private void DrawHorizontalBarLegend(Graphics g, float zoom)
        {
            LegendScheme aLS = _legendScheme;            
            PointF aP = new PointF(0, 0);
            PointF sP = new PointF(0, 0);
            Single width, height;                    
            SolidBrush labBrush = new SolidBrush(this.ForeColor);
            bool DrawShape = true, DrawFill = true, DrawOutline = true;
            Color FillColor = Color.Red, OutlineColor = this.ForeColor;
            string caption = "";
            SizeF aSF;

            int bNum = aLS.BreakNum;
            if (aLS.LegendBreaks[bNum - 1].IsNoData)
                bNum -= 1;

            width = (this.Width - 5) * zoom / bNum;
            height = _hBarHeight * zoom;
            Font lFont = new Font(Font.Name, Font.Size * zoom, Font.Style);

            for (int i = 0; i < bNum; i++)
            {
                switch (aLS.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)aLS.LegendBreaks[i];
                        //DrawShape = aPB.DrawShape;
                        DrawFill = aPB.DrawFill;
                        //DrawOutline = aPB.drawOutline;
                        FillColor = aPB.Color;
                        //OutlineColor = aPB.outlineColor;
                        if (aLS.LegendType == LegendType.UniqueValue)
                            caption = aPB.Caption;
                        else
                            caption = aPB.EndValue.ToString();
                        break;
                    case ShapeTypes.Polyline:
                        PolyLineBreak aPLB = (PolyLineBreak)aLS.LegendBreaks[i];
                        //DrawShape = aPLB.DrawPolyline;
                        FillColor = aPLB.Color;
                        if (aLS.LegendType == LegendType.UniqueValue)
                            caption = aPLB.Caption;
                        else
                            caption = aPLB.EndValue.ToString();
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[i];
                        //DrawShape = aPGB.DrawShape;
                        DrawFill = aPGB.DrawFill;
                        //DrawOutline = aPGB.drawOutline;
                        FillColor = aPGB.Color;
                        //OutlineColor = aPGB.outlineColor;
                        if (aLS.LegendType == LegendType.UniqueValue)
                            caption = aPGB.Caption;
                        else
                            caption = aPGB.EndValue.ToString();
                        break;
                    case ShapeTypes.Image:
                        ColorBreak aCB = aLS.LegendBreaks[i];
                        //DrawShape = true;
                        DrawFill = true;
                        FillColor = aCB.Color;
                        if (aLS.LegendType == LegendType.UniqueValue)
                            caption = aCB.Caption;
                        else
                            caption = aCB.EndValue.ToString();
                        break; 
                }

                aP.X = i * width + width / 2;
                aP.Y = height / 2;
                
                if (aLS.LegendType == LegendType.UniqueValue)
                {
                    if (DrawShape)
                    {
                        if (aLS.ShapeType == ShapeTypes.Polygon)
                        {
                            PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[i].Clone();
                            aPGB.DrawOutline = true;
                            aPGB.OutlineColor = Color.Black;
                            if (!aPGB.DrawShape)
                            {
                                aPGB.Color = Color.Transparent;
                            }
                            Draw.DrawPolygonSymbol(aP, width, height, aPGB, _legendLayer.TransparencyPerc, g);
                        }
                        else
                            Draw.DrawPolygonSymbol(aP, FillColor, OutlineColor, width,
                                height, DrawFill, DrawOutline, g);
                    }

                    sP.X = aP.X;
                    sP.Y = aP.Y + height / 2;
                    aSF = g.MeasureString(caption, lFont);
                    g.DrawString(caption, lFont, labBrush, sP.X - aSF.Width / 2, sP.Y);
                }
                else
                {
                    if (DrawShape)
                    {
                        if (i == 0)
                        {
                            PointF[] Points = new PointF[3];
                            Points[0].X = 0;
                            Points[0].Y = aP.Y;
                            Points[1].X = width;
                            Points[1].Y = 0;
                            Points[2].X = width;
                            Points[2].Y = height;
                            if (aLS.ShapeType == ShapeTypes.Polygon)
                            {
                                PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[i].Clone();
                                aPGB.DrawOutline = true;
                                aPGB.OutlineColor = Color.Black;
                                if (!aPGB.DrawShape)
                                {
                                    aPGB.Color = Color.Transparent;
                                }
                                Draw.DrawPolygon(Points, aPGB, g);
                            }
                            else
                                Draw.DrawPolygon(Points, FillColor, OutlineColor, width,
                                    height, DrawFill, DrawOutline, g);
                        }
                        else if (i == bNum - 1)
                        {
                            PointF[] Points = new PointF[3];
                            Points[0].X = i * width;
                            Points[0].Y = height;
                            Points[1].X = i * width;
                            Points[1].Y = 0;
                            Points[2].X = i * width + width;
                            Points[2].Y = aP.Y;
                            if (aLS.ShapeType == ShapeTypes.Polygon)
                            {
                                PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[i].Clone();
                                aPGB.DrawOutline = true;
                                aPGB.OutlineColor = Color.Black;
                                if (!aPGB.DrawShape)
                                {
                                    aPGB.Color = Color.Transparent;
                                }
                                Draw.DrawPolygon(Points, aPGB, g);
                            }
                            else
                                Draw.DrawPolygon(Points, FillColor, OutlineColor, width,
                                    height, DrawFill, DrawOutline, g);
                        }
                        else
                        {
                            if (aLS.ShapeType == ShapeTypes.Polygon)
                            {
                                PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[i].Clone();
                                aPGB.DrawOutline = true;
                                aPGB.OutlineColor = Color.Black;
                                if (!aPGB.DrawShape)
                                {
                                    aPGB.Color = Color.Transparent;
                                }
                                Draw.DrawPolygonSymbol(aP, width, height, aPGB, _legendLayer.TransparencyPerc, g);
                            }
                            else
                                Draw.DrawPolygonSymbol(aP, FillColor, OutlineColor, width,
                                    height, DrawFill, DrawOutline, g);
                        }
                    }                    

                    sP.X = aP.X + width / 2;
                    sP.Y = aP.Y + height / 2;
                    if (i < bNum - 1)
                    {
                        aSF = g.MeasureString(caption, lFont);
                        g.DrawString(caption, lFont, labBrush, sP.X - aSF.Width / 2, sP.Y);
                    }
                }
            }

            lFont.Dispose();
            labBrush.Dispose();
        }

        private int GetLabelWidth(Graphics g)
        {
            LegendScheme aLS = _legendScheme;            
            Single width = 0;                               
            string caption = "";
            SizeF aSF;
            int bNum = aLS.BreakNum;

            if (_legendStyle == LegendStyles.Normal)
            {
                aSF = g.MeasureString(_title, Font);
                width = aSF.Width;
            }
            else
            {
                if (aLS.LegendBreaks[bNum - 1].IsNoData)
                    bNum -= 1;
            }
            for (int i = 0; i < bNum; i++)
            {
                switch (aLS.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)aLS.LegendBreaks[i];
                        if (aLS.LegendType == LegendType.GraduatedColor && _legendStyle != LegendStyles.Normal)
                            caption = aPB.EndValue.ToString();
                        else
                            caption = aPB.Caption;
                        break;
                    case ShapeTypes.Polyline:
                        PolyLineBreak aPLB = (PolyLineBreak)aLS.LegendBreaks[i];
                        if (aLS.LegendType == LegendType.GraduatedColor && _legendStyle != LegendStyles.Normal)
                            caption = aPLB.EndValue.ToString();
                        else
                            caption = aPLB.Caption;
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[i];
                        if (aLS.LegendType == LegendType.GraduatedColor && _legendStyle != LegendStyles.Normal)
                            caption = aPGB.EndValue.ToString();
                        else
                            caption = aPGB.Caption;
                        break;
                    case ShapeTypes.Image:
                        ColorBreak aCB = aLS.LegendBreaks[i];
                        if (aLS.LegendType == LegendType.GraduatedColor && _legendStyle != LegendStyles.Normal)
                            caption = aCB.EndValue.ToString();
                        else
                            caption = aCB.Caption;
                        break;
                }

                bool isValid = true;
                switch (aLS.LegendType)
                {
                    case LegendType.GraduatedColor:
                        if (_legendStyle != LegendStyles.Normal)
                        {
                            if (i == bNum - 1)
                                isValid = false;
                        }
                        break;
                }
                if (isValid)
                {
                    aSF = g.MeasureString(caption, Font);
                    if (width < aSF.Width)
                        width = aSF.Width;
                }
            }

            return (int)width;
        }

        private int GetBreakHeight(Graphics g)
        {
            string title = _title;
            if (title.Trim() == "")
                title = "Temp";

            SizeF aSF = g.MeasureString(title, Font);
            float height = aSF.Height;

            return (int)Math.Ceiling(height);
        }

        private int GetTitleHeight(Graphics g)
        {
            SizeF aSF = g.MeasureString(_title, _titleFont);
            float height = aSF.Height;

            return (int)Math.Ceiling(height);
        }

        /// <summary>
        /// Update legend control size
        /// </summary>
        public void UpdateLegendSize()
        {
            if (_legendScheme == null)
                return;

            Graphics g = _mapLayout.CreateGraphics();
            int bNum = _legendScheme.BreakNum;
            if (_legendScheme.LegendBreaks[bNum - 1].IsNoData)
                bNum -= 1;

            switch (_legendStyle)
            {
                case LegendStyles.Bar_Vertical:
                    this.Width = 10 + GetLabelWidth(g) + 5;
                    this.Height = _layoutMap.Height * 2 / 3;
                    break;
                case LegendStyles.Bar_Horizontal:
                    this.Width = _layoutMap.Width * 2 / 3;
                    this.Height = 30;
                    break;
                case LegendStyles.Normal:
                    int aHeight = GetBreakHeight(g);
                    int colWidth = aHeight * 2 + GetLabelWidth(g) + 10;
                    this.Width = colWidth * _columnNum;

                    //Set columns
                    int[] rowNums = new int[_columnNum];
                    int ave = _legendScheme.VisibleBreakNum / _columnNum;
                    int num = 0;
                    int i;
                    for (i = 1; i < _columnNum; i++)
                    {
                        rowNums[i] = ave;
                        num += ave;
                    }
                    rowNums[0] = _legendScheme.VisibleBreakNum - num;

                    this.Height = (int)(rowNums[0] * (aHeight + _breakSpace) +
                        GetTitleHeight(g) + _breakSpace * 2 + aHeight / 2 + 5);
                    if (_drawChart)
                    {
                        //ChartBreak aCB = ((ChartBreak)aLayer.ChartPoints[0].Legend).GetSampleChartBreak();
                        this.Height += (int)(_breakSpace * 2 + _chartBreak.GetHeight() +
                            _chartBreak.LegendScheme.BreakNum * (aHeight + _breakSpace) + aHeight / 2 + 5);
                    }
                    break;
            }
        }

        ///// <summary>
        ///// Update legend control size
        ///// </summary>
        //public void UpdateLegendSize()
        //{
        //    if (_legendLayer != null)
        //    {
        //        if (_legendLayer.LegendScheme == null)
        //            return;

        //        Graphics g = _mapLayout.CreateGraphics();
        //        int bNum = _legendLayer.LegendScheme.BreakNum;
        //        if (_legendLayer.LegendScheme.LegendBreaks[bNum - 1].IsNoData)
        //            bNum -= 1;

        //        switch (_legendStyle)
        //        {
        //            case LegendStyles.Bar_Vertical:
        //                this.Width = 10 + GetLabelWidth(g) + 5;
        //                this.Height = _layoutMap.Height * 2 / 3;
        //                break;
        //            case LegendStyles.Bar_Horizontal:
        //                this.Width = _layoutMap.Width * 2 / 3;
        //                this.Height = 30;
        //                break;
        //            case LegendStyles.Normal:
        //                int aHeight = GetBreakHeight(g);
        //                int colWidth = aHeight * 2 + GetLabelWidth(g) + 10;
        //                this.Width = colWidth * _columnNum;

        //                //Set columns
        //                int[] rowNums = new int[_columnNum];
        //                int ave = _legendLayer.LegendScheme.VisibleBreakNum / _columnNum;
        //                int num = 0;
        //                int i;
        //                for (i = 1; i < _columnNum; i++)
        //                {
        //                    rowNums[i] = ave;
        //                    num += ave;
        //                }
        //                rowNums[0] = _legendLayer.LegendScheme.VisibleBreakNum - num;

        //                this.Height = (int)(rowNums[0] * (aHeight + _breakSpace) +
        //                    GetTitleHeight(g) + _breakSpace * 2 + aHeight / 2 + 5);
        //                if (_legendLayer.LayerType == LayerTypes.VectorLayer)
        //                {
        //                    VectorLayer aLayer = (VectorLayer)_legendLayer;
        //                    if (aLayer.ChartSet.DrawCharts)
        //                    {
        //                        ChartBreak aCB = ((ChartBreak)aLayer.ChartPoints[0].Legend).GetSampleChartBreak();
        //                        this.Height += (int)(_breakSpace * 2 + aCB.GetHeight() +
        //                            aCB.LegendScheme.BreakNum * (aHeight + _breakSpace) + aHeight / 2 + 5);
        //                    }
        //                }
        //                break;
        //        }
        //    }
        //}

        private List<lPolygon> CreateLegend(double[] CValues)
        {            
            legendPara legendPara;

            PointD aPoint = new PointD();
            aPoint.X = 0;
            aPoint.Y = 0;
            legendPara.isTriangle = true;
            legendPara.isVertical = false;
            legendPara.startPoint = aPoint;
            legendPara.length = this.Width;
            legendPara.width = 10;
            legendPara.contourValues = CValues;

            return CreateBarLegend(legendPara);
        }

        /// <summary>
        /// Create bar legend
        /// </summary>
        /// <param name="aLegendPara">legend parameter</param>
        /// <returns>legend polygons</returns>
        public List<lPolygon> CreateBarLegend(legendPara aLegendPara)
        {
            List<lPolygon> polygonList = new List<lPolygon>();
            List<PointD> pList = new List<PointD>();
            lPolygon aLPolygon;
            //PointD aPoint;
            int i, pNum;
            double aLength;
            bool ifRectangle;

            pNum = aLegendPara.contourValues.Length + 1;
            aLength = aLegendPara.length / pNum;
            if (aLegendPara.isVertical)
            {
                for (i = 0; i < pNum; i++)
                {
                    pList = new List<PointD>();
                    ifRectangle = true;
                    if (i == 0)
                    {
                        aLPolygon.value = aLegendPara.contourValues[0];
                        aLPolygon.isFirst = true;
                        if (aLegendPara.isTriangle)
                        {
                            PointD aPoint = new PointD();
                            aPoint.X = aLegendPara.startPoint.X + aLegendPara.width / 2;
                            aPoint.Y = aLegendPara.startPoint.Y;
                            pList.Add(aPoint);
                            aPoint.X = aLegendPara.startPoint.X + aLegendPara.width;
                            aPoint.Y = aLegendPara.startPoint.Y + aLength;
                            pList.Add(aPoint);
                            aPoint.X = aLegendPara.startPoint.X;
                            pList.Add(aPoint);
                            ifRectangle = false;
                        }
                    }
                    else
                    {
                        aLPolygon.value = aLegendPara.contourValues[i - 1];
                        aLPolygon.isFirst = false;
                        if (i == pNum - 1)
                        {
                            if (aLegendPara.isTriangle)
                            {
                                PointD aPoint = new PointD();
                                aPoint.X = aLegendPara.startPoint.X;
                                aPoint.Y = aLegendPara.startPoint.Y + i * aLength;
                                pList.Add(aPoint);
                                aPoint.X = aLegendPara.startPoint.X + aLegendPara.width;
                                aPoint.Y = aLegendPara.startPoint.Y + i * aLength;
                                pList.Add(aPoint);
                                aPoint.X = aLegendPara.startPoint.X + aLegendPara.width / 2;
                                aPoint.Y = aLegendPara.startPoint.Y + (i + 1) * aLength;
                                pList.Add(aPoint);
                                ifRectangle = false;
                            }
                        }
                    }

                    if (ifRectangle)
                    {
                        PointD aPoint = new PointD();
                        aPoint.X = aLegendPara.startPoint.X;
                        aPoint.Y = aLegendPara.startPoint.Y + i * aLength;
                        pList.Add(aPoint);
                        aPoint.X = aLegendPara.startPoint.X + aLegendPara.width;
                        pList.Add(aPoint);
                        aPoint.Y = aLegendPara.startPoint.Y + (i + 1) * aLength;
                        pList.Add(aPoint);
                        aPoint.X = aLegendPara.startPoint.X;
                        pList.Add(aPoint);
                    }

                    pList.Add(pList[0]);
                    aLPolygon.pointList = pList;

                    polygonList.Add(aLPolygon);
                }
            }
            else
            {
                for (i = 0; i < pNum; i++)
                {
                    pList = new List<PointD>();
                    ifRectangle = true;
                    if (i == 0)
                    {
                        aLPolygon.value = aLegendPara.contourValues[0];
                        aLPolygon.isFirst = true;
                        if (aLegendPara.isTriangle)
                        {
                            PointD aPoint = new PointD();
                            aPoint.X = aLegendPara.startPoint.X;
                            aPoint.Y = aLegendPara.startPoint.Y + aLegendPara.width / 2;
                            pList.Add(aPoint);
                            aPoint.X = aLegendPara.startPoint.X + aLength;
                            aPoint.Y = aLegendPara.startPoint.Y;
                            pList.Add(aPoint);
                            aPoint.Y = aLegendPara.startPoint.Y + aLegendPara.width;
                            pList.Add(aPoint);
                            ifRectangle = false;
                        }
                    }
                    else
                    {
                        aLPolygon.value = aLegendPara.contourValues[i - 1];
                        aLPolygon.isFirst = false;
                        if (i == pNum - 1)
                        {
                            if (aLegendPara.isTriangle)
                            {
                                PointD aPoint = new PointD();
                                aPoint.X = aLegendPara.startPoint.X + i * aLength;
                                aPoint.Y = aLegendPara.startPoint.Y;
                                pList.Add(aPoint);
                                aPoint.X = aLegendPara.startPoint.X + (i + 1) * aLength;
                                aPoint.Y = aLegendPara.startPoint.Y + aLegendPara.width / 2;
                                pList.Add(aPoint);
                                aPoint.X = aLegendPara.startPoint.X + i * aLength;
                                aPoint.Y = aLegendPara.startPoint.Y + aLegendPara.width;
                                pList.Add(aPoint);
                                ifRectangle = false;
                            }
                        }
                    }

                    if (ifRectangle)
                    {
                        PointD aPoint = new PointD();
                        aPoint.X = aLegendPara.startPoint.X + i * aLength;
                        aPoint.Y = aLegendPara.startPoint.Y;
                        pList.Add(aPoint);
                        aPoint.X = aLegendPara.startPoint.X + (i + 1) * aLength;
                        pList.Add(aPoint);
                        aPoint.Y = aLegendPara.startPoint.Y + aLegendPara.width;
                        pList.Add(aPoint);
                        aPoint.X = aLegendPara.startPoint.X + i * aLength;
                        pList.Add(aPoint);
                    }

                    pList.Add(pList[0]);
                    aLPolygon.pointList = pList;

                    polygonList.Add(aLPolygon);
                }
            }

            return polygonList;
        }

        #endregion

        #region Events
        /// <summary>
        /// Override paint method
        /// </summary>
        /// <param name="g">graphics</param>
        public override void Paint(Graphics g)
        {
            if (Visible)
                PaintGraphics(g);     
        }

        /// <summary>
        /// Override PaintOnLayout method
        /// </summary>
        /// <param name="g">graphics</param>
        /// <param name="pageLocation">page location</param>
        /// <param name="zoom">zoom</param>
        public override void PaintOnLayout(Graphics g, PointF pageLocation, float zoom)
        {
            if (Visible)
                PaintGraphics(g, pageLocation, zoom);
        }

        ///// <summary>
        ///// OnPaint
        ///// </summary>
        ///// <param name="e">PaintEventArgs</param>
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    //base.OnPaint(e);

        //    if (_drawLegend)
        //    {
        //        Graphics g = e.Graphics;
                             
        //        PaintGraphics(g);

        //        //Draws the selection rectangle around each selected item
        //        Pen selectionPen = new Pen(Color.Red, 1F);
        //        selectionPen.DashPattern = new[] { 2.0F, 1.0F };
        //        selectionPen.DashCap = DashCap.Round;
        //        if (m_IsSelectedInLayout)
        //        {
        //            Rectangle aRect = this.Bounds;
        //            g.DrawRectangle(selectionPen, 0, 0, this.Width - 1, this.Height - 1);
        //        }
        //    }            
        //}       
       

        ///// <summary>
        ///// Override OnMouseDoubleClick event
        ///// </summary>
        ///// <param name="e">MouseEventArgs</param>
        //protected override void OnMouseDoubleClick(MouseEventArgs e)
        //{
        //    //base.OnMouseDoubleClick(e);

        //    SetProperties();
        //}

        ///// <summary>
        ///// Set properties
        ///// </summary>
        //public void SetProperties()
        //{
        //    Dictionary<string, string> objAttr = new Dictionary<string, string>();
        //    objAttr.Add("FirstMeteoLayer", "FirstMeteoLayer");
        //    objAttr.Add("LegendStyle", "LegendStyle");
        //    objAttr.Add("Title", "Title");
        //    objAttr.Add("DrawNeatLine", "DrawNeatLine");
        //    objAttr.Add("NeatLineColor", "NeatLineColor");
        //    objAttr.Add("NeatLineSize", "NeatLineSize");
        //    objAttr.Add("Font", "Font");
        //    objAttr.Add("BackColor", "Back Color");
        //    objAttr.Add("ForeColor", "Fore Color");
        //    CustomProperty cp = new CustomProperty(this, objAttr);
        //    frmProperty aFrmP = new frmProperty();
        //    aFrmP.SetObject(cp);
        //    aFrmP.ShowDialog();
        //}

        /// <summary>
        /// Override get property object methods
        /// </summary>
        /// <returns>property object</returns>
        public override object GetPropertyObject()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("LayerName", "LayerName");
            objAttr.Add("LayerUpdateType", "LayerUpdateType");
            objAttr.Add("LegendStyle", "LegendStyle");
            objAttr.Add("Title", "Title");
            objAttr.Add("DrawNeatLine", "DrawNeatLine");
            objAttr.Add("NeatLineColor", "NeatLineColor");
            objAttr.Add("NeatLineSize", "NeatLineSize");
            objAttr.Add("Font", "Font");
            objAttr.Add("BackColor", "Back Color");
            objAttr.Add("ForeColor", "Fore Color");
            objAttr.Add("Left", "Left");
            objAttr.Add("Top", "Top");
            objAttr.Add("ColumnNumber", "ColumnNumber");
            objAttr.Add("Width", "Width");
            objAttr.Add("Height", "Height");
            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;
        }

        private void LayoutMap_MapViewUpdated(object sender, EventArgs e)
        {
            switch (_layerUpdateType)
            {
                case LayerUpdateTypes.FirstExpandedLayer:
                    for (int i = 0; i < _layoutMap.MapFrame.MapView.LayerSet.Layers.Count; i++)
                    {
                        MapLayer aLayer = _layoutMap.MapFrame.MapView.LayerSet.Layers[_layoutMap.MapFrame.MapView.LayerSet.Layers.Count - 1 - i];
                        if (aLayer.LayerType != LayerTypes.ImageLayer)
                        {
                            if (aLayer.Visible && aLayer.Expanded && aLayer.LegendScheme.LegendType != LegendType.SingleSymbol)
                            {
                                this.Visible = true;
                                this.LegendLayer = aLayer;
                                break;
                            }
                        }
                    }
                    break;
                case LayerUpdateTypes.FirstMeteoLayer:
                    for (int i = 0; i < _layoutMap.MapFrame.MapView.LayerSet.Layers.Count; i++)
                    {
                        MapLayer aLayer = _layoutMap.MapFrame.MapView.LayerSet.Layers[_layoutMap.MapFrame.MapView.LayerSet.Layers.Count - 1 - i];
                        if (aLayer.LayerType != LayerTypes.ImageLayer)
                        {
                            if (aLayer.Visible && aLayer.LayerDrawType != LayerDrawType.Map &&
                                aLayer.LegendScheme.LegendType != LegendType.SingleSymbol)
                            {
                                this.Visible = true;
                                this.LegendLayer = aLayer;
                                break;
                            }
                        }
                    }
                    break;
                case LayerUpdateTypes.LastAddedLayer:
                    if (_layoutMap.MapFrame.MapView.LastAddedLayer == null)
                        break;

                    if (_layoutMap.MapFrame.MapView.LastAddedLayer.LayerType != LayerTypes.ImageLayer)
                    {
                        this.Visible = true;
                        this.LegendLayer = _layoutMap.MapFrame.MapView.LastAddedLayer;
                    }
                    break;
            }

            this.UpdateLegendSize();
        }
       
        #endregion
    }
}
