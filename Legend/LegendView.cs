using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Shape;
using MeteoInfoC.Drawing;
using MeteoInfoC.Global;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Legend view control
    /// </summary>
    public partial class LegendView : UserControl
    {
        #region Variables
        private LegendScheme _legendScheme;
        private int _breakHeight;
        private int _symbolWidth;
        private int _valueWidth;
        private int _labelWidth;

        private frmPointSymbolSet _frmPointSymbolSet;
        private frmPolylineSymbolSet _frmPolylineSymbolSet;
        private frmPolygonSymbolSet _frmPolygonSymbolSet;
        private frmColorSymbolSet _frmColorSymbolSet;

        private List<int> _selectedRows = new List<int>();
        private int _startRow = -1;
        private ColorBreak _curBreak = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>        
        public LegendView()
        {
            InitializeComponent();

            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
            this.BackColor = Color.White;
            TB_Editor.Visible = false;

            _frmPointSymbolSet = new frmPointSymbolSet(this);
            _frmPolylineSymbolSet = new frmPolylineSymbolSet(this);
            _frmPolygonSymbolSet = new frmPolygonSymbolSet(this);
            _frmColorSymbolSet = new frmColorSymbolSet(this);
            _legendScheme = null;
            _breakHeight = 20;
            _symbolWidth = 60;
            _valueWidth = (this.Width - _symbolWidth) / 2;
            _labelWidth = _valueWidth;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set legend scheme
        /// </summary>
        public LegendScheme LegendScheme
        {
            get { return _legendScheme; }
            set { _legendScheme = value; }
        }

        /// <summary>
        /// Get selected rows
        /// </summary>
        public List<int> SelectedRows
        {
            get { return _selectedRows; }            
        }

        #endregion

        #region Methods
        /// <summary>
        /// Update legend scheme
        /// </summary>
        /// <param name="aLS">legend scheme</param>
        public void Update(LegendScheme aLS)
        {
            _legendScheme = aLS;
            this.Invalidate();
        }

        #region Edit legend break
        /// <summary>
        /// Set legend break color
        /// </summary>
        /// <param name="aColor"></param>
        public void SetLegendBreak_Color(Color aColor)
        {                   
            foreach(int rowIdx in _selectedRows)
            {                
                _legendScheme.LegendBreaks[rowIdx].Color = aColor;                
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break color alpha
        /// </summary>
        /// <param name="alpha">Alpha value</param>
        public void SetLegendBreak_Alpha(int alpha)
        {
            foreach (int rowIdx in _selectedRows)
                _legendScheme.LegendBreaks[rowIdx].Color = Color.FromArgb(alpha, _legendScheme.LegendBreaks[rowIdx].Color);
            this.Invalidate();
        }

        /// <summary>
        /// Set legend break outline color
        /// </summary>
        /// <param name="aColor">color</param>
        public void SetLegendBreak_OutlineColor(Color aColor)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                switch (_legendScheme.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPB.OutlineColor = aColor;
                        _legendScheme.LegendBreaks[rowIdx] = aPB;
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPGB.OutlineColor = aColor;
                        _legendScheme.LegendBreaks[rowIdx] = aPGB;
                        break;
                }
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break outline size
        /// </summary>
        /// <param name="outlineSize">color</param>
        public void SetLegendBreak_OutlineSize(float outlineSize)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                switch (_legendScheme.ShapeType)
                {
                    //case ShapeTypes.Point:
                    //    PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                    //    aPB.OutlineColor = aColor;
                    //    _legendScheme.LegendBreaks[rowIdx] = aPB;
                    //    break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPGB.OutlineSize = outlineSize;
                        _legendScheme.LegendBreaks[rowIdx] = aPGB;
                        break;
                }
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break angle
        /// </summary>
        /// <param name="angle">angle</param>
        public void SetLegendBreak_Angle(float angle)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                aPB.Angle = angle;
                _legendScheme.LegendBreaks[rowIdx] = aPB;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break size
        /// </summary>
        /// <param name="aSize">size</param>
        public void SetLegendBreak_Size(Single aSize)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                switch (_legendScheme.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPB.Size = aSize;
                        _legendScheme.LegendBreaks[rowIdx] = aPB;
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineZ:
                        PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPLB.Size = aSize;
                        _legendScheme.LegendBreaks[rowIdx] = aPLB;
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPGB.OutlineSize = aSize;
                        _legendScheme.LegendBreaks[rowIdx] = aPGB;
                        break;
                }
            }
            
            this.Invalidate();
        }

        /// <summary>
        /// Set legend break point style
        /// </summary>
        /// <param name="aPS"></param>
        public void SetLegendBreak_PointStyle(PointStyle aPS)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                aPB.Style = aPS;
                _legendScheme.LegendBreaks[rowIdx] = aPB;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break polyline style
        /// </summary>
        /// <param name="style"></param>
        public void SetLegendBreak_PolylineStyle(LineStyles style)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                aPLB.Style = style;
                _legendScheme.LegendBreaks[rowIdx] = aPLB;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break if draw outline
        /// </summary>
        /// <param name="drawOutLine"></param>
        public void SetLegendBreak_DrawOutline(Boolean drawOutLine)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                switch (_legendScheme.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPB.DrawOutline = drawOutLine;
                        _legendScheme.LegendBreaks[rowIdx] = aPB;
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPGB.DrawOutline = drawOutLine;
                        _legendScheme.LegendBreaks[rowIdx] = aPGB;
                        break;
                }
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break if draw fill
        /// </summary>
        /// <param name="drawFill"></param>
        public void SetLegendBreak_DrawFill(Boolean drawFill)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                switch (_legendScheme.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPB.DrawFill = drawFill;
                        _legendScheme.LegendBreaks[rowIdx] = aPB;
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPGB.DrawFill = drawFill;
                        _legendScheme.LegendBreaks[rowIdx] = aPGB;
                        break;
                }
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break if draw shape
        /// </summary>
        /// <param name="drawShape"></param>
        public void SetLegendBreak_DrawShape(Boolean drawShape)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                switch (_legendScheme.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPB.DrawShape = drawShape;
                        _legendScheme.LegendBreaks[rowIdx] = aPB;
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineZ:
                        PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPLB.DrawPolyline = drawShape;
                        _legendScheme.LegendBreaks[rowIdx] = aPLB;
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPGB.DrawShape = drawShape;
                        _legendScheme.LegendBreaks[rowIdx] = aPGB;
                        break;
                }
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break if using hatch style
        /// </summary>
        /// <param name="usginHatchStyle"></param>
        public void SetLegendBreak_UsingHatchStyle(Boolean usginHatchStyle)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                aPGB.UsingHatchStyle = usginHatchStyle;
                _legendScheme.LegendBreaks[rowIdx] = aPGB;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break hatch style
        /// </summary>
        /// <param name="hatchStyle"></param>
        public void SetLegendBreak_HatchStyle(HatchStyle hatchStyle)
        {
            foreach (int rowIdx in _selectedRows)
            {               
                PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                aPGB.Style = hatchStyle;
                _legendScheme.LegendBreaks[rowIdx] = aPGB;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break marder type
        /// </summary>
        /// <param name="markerType">marker type</param>
        public void SetLegendBreak_MarkerType(MarkerType markerType)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                aPB.MarkerType = markerType;
                _legendScheme.LegendBreaks[rowIdx] = aPB;
            }
        }

        /// <summary>
        /// Set legend break font name
        /// </summary>
        /// <param name="fontName">font name</param>
        public void SetLegendBreak_FontName(string fontName)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                aPB.FontName = fontName;
                _legendScheme.LegendBreaks[rowIdx] = aPB;
            }
        }

        /// <summary>
        /// Set legend break image path
        /// </summary>
        /// <param name="imagePath">image path</param>
        public void SetLegendBreak_Image(string imagePath)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                aPB.ImagePath = imagePath;
                _legendScheme.LegendBreaks[rowIdx] = aPB;
            }
        }

        /// <summary>
        /// Set legend break marker index
        /// </summary>
        /// <param name="markerIdx">marker index</param>
        public void SetLegendBreak_MarkerIndex(int markerIdx)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                switch (aPB.MarkerType)
                {
                    case MarkerType.Character:
                        aPB.CharIndex = markerIdx;
                        break;
                    case MarkerType.Simple:
                        aPB.Style = (PointStyle)markerIdx;
                        break;
                    case MarkerType.Image:

                        break;
                }
                _legendScheme.LegendBreaks[rowIdx] = aPB;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break if draw fill
        /// </summary>
        /// <param name="backColor"></param>
        public void SetLegendBreak_BackColor(Color backColor)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                aPGB.BackColor = backColor;
                _legendScheme.LegendBreaks[rowIdx] = aPGB;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break alpha if draw fill
        /// </summary>
        /// <param name="alpha">Alpha value</param>
        public void SetLegendBreak_BackColorAlpha(int alpha)
        {
            foreach (int rowIdx in _selectedRows)
            {
                PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                aPGB.BackColor = Color.FromArgb(alpha, aPGB.BackColor);
                _legendScheme.LegendBreaks[rowIdx] = aPGB;
            }
        }

        /// <summary>
        /// Set legend break if draw symbol
        /// </summary>
        /// <param name="drawSymbol"></param>
        public void SetLegendBreak_DrawSymbol(bool drawSymbol)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                aPLB.DrawSymbol = drawSymbol;
                _legendScheme.LegendBreaks[rowIdx] = aPLB;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break symbol size
        /// </summary>
        /// <param name="symbolSize"></param>
        public void SetLegendBreak_SymbolSize(Single symbolSize)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                aPLB.SymbolSize = symbolSize;
                _legendScheme.LegendBreaks[rowIdx] = aPLB;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break symbol size
        /// </summary>
        /// <param name="symbolStyle"></param>
        public void SetLegendBreak_SymbolStyle(PointStyle symbolStyle)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                aPLB.SymbolStyle = symbolStyle;
                _legendScheme.LegendBreaks[rowIdx] = aPLB;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break symbol color
        /// </summary>
        /// <param name="symbolColor">symbol color</param>
        public void SetLegendBreak_SymbolColor(Color symbolColor)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                aPLB.SymbolColor = symbolColor;
                _legendScheme.LegendBreaks[rowIdx] = aPLB;
            }

            this.Invalidate();
        }

        /// <summary>
        /// Set legend break symbol interval
        /// </summary>
        /// <param name="symbolInterval">symbol interval</param>
        public void SetLegendBreak_SymbolInterval(int symbolInterval)
        {
            foreach (int rowIdx in _selectedRows)
            {                
                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                aPLB.SymbolInterval = symbolInterval;
                _legendScheme.LegendBreaks[rowIdx] = aPLB;
            }

            this.Invalidate();
        }

        #endregion

        #endregion

        #region Events
        /// <summary>
        /// Override OnPaint event
        /// </summary>
        /// <param name="e">PaintEventArgs</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_legendScheme != null)
            {
                Graphics g = e.Graphics;
                PaintLegendScheme(g);
            }
        }

        private void PaintLegendScheme(Graphics g)
        {
            int TotalHeight = CalcTotalDrawHeight();
            Rectangle rect;
            if (TotalHeight > this.Height)
            {
                _vScrollBar.Minimum = 0;
                _vScrollBar.SmallChange = _breakHeight;
                _vScrollBar.LargeChange = this.Height;
                _vScrollBar.Maximum = TotalHeight;

                if (_vScrollBar.Visible == false)
                {
                    _vScrollBar.Value = 0;
                    _vScrollBar.Visible = true;
                }
                rect = new Rectangle(0, -_vScrollBar.Value, this.Width - _vScrollBar.Width, TotalHeight);

                _valueWidth = (this.Width - _vScrollBar.Width - _symbolWidth) / 2;
                _labelWidth = _valueWidth;
            }
            else
            {
                _vScrollBar.Visible = false;
                rect = new Rectangle(0, 0, this.Width, this.Height);

                _valueWidth = (this.Width - _symbolWidth) / 2;
                _labelWidth = _valueWidth;
            }            

            //Draw breaks
            DrawBreaks(g, rect.Y);

            //Draw title            
            DrawTitle(g);
        }

        private void DrawTitle(Graphics g)
        {            
            //Symbol
            int sX = 0;
            SolidBrush aBrush = new SolidBrush(Color.WhiteSmoke);
            SolidBrush sBrush = new SolidBrush(Color.Black);
            Pen aPen = new Pen(Color.Black);
            Font aFont = new Font("Arial", 8);
            g.FillRectangle(aBrush, new Rectangle(sX, 0, _symbolWidth, _breakHeight));
            g.DrawRectangle(aPen, new Rectangle(0, 0, _symbolWidth, _breakHeight));
            string str = "Symbol";
            SizeF size = g.MeasureString(str, aFont);
            int cx = _symbolWidth / 2;
            int cy = _breakHeight / 2;
            PointF aPoint = new PointF(cx - size.Width / 2, cy - size.Height / 2);
            g.DrawString(str, aFont, sBrush, aPoint);

            if (_legendScheme.LegendType != LegendType.SingleSymbol)
            {
                //Value
                sX = _symbolWidth;
                g.FillRectangle(aBrush, new Rectangle(sX, 0, _valueWidth, _breakHeight));
                g.DrawRectangle(aPen, new Rectangle(sX, 0, _valueWidth, _breakHeight));
                str = "Value";
                size = g.MeasureString(str, aFont);
                cx = sX + _valueWidth / 2;
                aPoint = new PointF(cx - size.Width / 2, cy - size.Height / 2);
                g.DrawString(str, aFont, sBrush, aPoint);

                //Label
                sX = _symbolWidth + _valueWidth;
            }
            else
            {
                sX = _symbolWidth;
            }

            //Label            
            g.FillRectangle(aBrush, new Rectangle(sX, 0, _labelWidth, _breakHeight));
            g.DrawRectangle(aPen, new Rectangle(sX, 0, _labelWidth, _breakHeight));
            str = "Label";
            size = g.MeasureString(str, aFont);
            cx = sX + _labelWidth / 2;
            aPoint = new PointF(cx - size.Width / 2, cy - size.Height / 2);
            g.DrawString(str, aFont, sBrush, aPoint);
        }

        private void DrawBreaks(Graphics g, int sHeight)
        {
            Point sP = new Point(0, sHeight + _breakHeight);
            for (int i = 0; i < _legendScheme.BreakNum; i++)
            {
                if (sP.Y + _breakHeight > _breakHeight)
                {
                    ColorBreak aCB = _legendScheme.LegendBreaks[i];
                    Rectangle rect = new Rectangle(sP.X, sP.Y, _symbolWidth, _breakHeight);
                    bool selected = _selectedRows.Contains(i);
                    DrawBreakSymbol(aCB, _legendScheme.ShapeType, rect, selected, g);
                    sP.Y += _breakHeight;
                }
                else if (sP.Y > this.Height)
                    break;
                else
                {
                    sP.Y += _breakHeight;
                    continue;
                }
            }
        }

        private void DrawBreakSymbol(ColorBreak aCB, ShapeTypes shapeType, Rectangle rect, bool selected, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Single aSize;
            PointF aP = new Point(0, 0);
            Single width, height;            
            aP.X = rect.Left + rect.Width / 2;
            aP.Y = rect.Top + rect.Height / 2;

            //Draw selected back color
            if (selected)
            {
                g.FillRectangle(Brushes.LightGray, new Rectangle(_symbolWidth, rect.Top, _valueWidth + _labelWidth, rect.Height));
            }

            //Draw symbol
            switch (shapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes.PointZ:
                case ShapeTypes.WindArraw:
                case ShapeTypes.WindBarb:
                case ShapeTypes.WeatherSymbol:
                case ShapeTypes.StationModel:
                    PointBreak aPB = (PointBreak)aCB;                                 
                    aSize = aPB.Size;
                    if (aPB.DrawShape)
                    {
                        //Draw.DrawPoint(aPB.Style, aP, aPB.Color, aPB.OutlineColor,
                        //    aPB.Size, aPB.DrawOutline, aPB.DrawFill, g);
                        if (aPB.MarkerType == MarkerType.Character)
                        {
                            TextRenderingHint aTextRendering = g.TextRenderingHint;
                            g.TextRenderingHint = TextRenderingHint.AntiAlias;
                            Draw.DrawPoint(aP, aPB, g);
                            g.TextRenderingHint = aTextRendering;
                        }
                        else
                            Draw.DrawPoint(aP, aPB, g);
                    }
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineZ:
                    PolyLineBreak aPLB = (PolyLineBreak)aCB;                                       
                    aSize = aPLB.Size;
                    width = rect.Width / 3 * 2;
                    height = rect.Height / 3 * 2;
                    Draw.DrawPolyLineSymbol(aP, width, height, aPLB, g);
                    break;
                case ShapeTypes.Polygon:
                    PolygonBreak aPGB = (PolygonBreak)aCB;                                  
                    width = rect.Width / 3 * 2;
                    height = rect.Height / 5 * 4;
                    if (aPGB.DrawShape)
                    {
                        //Draw.DrawPolygonSymbol(aP, aPGB.Color, aPGB.OutlineColor, width,
                        //    height, aPGB.DrawFill, aPGB.DrawOutline, g);
                        Draw.DrawPolygonSymbol(aP, width, height, aPGB, 0, g);
                    }
                    break;
                case ShapeTypes.Image:                                                         
                    width = rect.Width / 3 * 2;
                    height = rect.Height / 3 * 2;
                    Draw.DrawPolygonSymbol(aP, aCB.Color, Color.Black, width,
                            height, true, true, g);
                    break;
            }  
          
            //Draw value and label
            int sX = _symbolWidth;
            Font aFont = new Font("Arial", 8);
            string str = aCB.Caption;
            SizeF size = g.MeasureString(str, aFont);
            aP.X = sX;
            aP.Y -= size.Height / 2;
            //if (size.Width > _labelWidth)
            //    str = str.Substring(0, 16) + "...";

            if (_legendScheme.LegendType == LegendType.SingleSymbol)
            {
                //Label
                //g.DrawString(str, aFont, Brushes.Black, aP);
                g.DrawString(str, aFont, Brushes.Black, new RectangleF(aP, new SizeF(_valueWidth, size.Height)));
            }
            else
            {
                //Label
                aP.X += _valueWidth;
                g.DrawString(str, aFont, Brushes.Black, aP);

                //Value
                if (aCB.StartValue.ToString() == aCB.EndValue.ToString())
                    str = aCB.StartValue.ToString();
                else
                    str = aCB.StartValue.ToString() + " - " + aCB.EndValue.ToString();

                size = g.MeasureString(str, aFont);
                aP.X = sX;                
                //if (size.Width > _labelWidth)
                //    str = str.Substring(0, 16) + "...";

                //g.DrawString(str, aFont, Brushes.Black, aP);
                g.DrawString(str, aFont, Brushes.Black, new RectangleF(aP, new SizeF(_valueWidth, size.Height)));
            }
        }

        private int CalcTotalDrawHeight()
        {
            return _breakHeight * (_legendScheme.BreakNum + 1);
        } 

        /// <summary>
        /// Override OnResize event
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.Width > 0 && this.Height > 0)
            {
                _valueWidth = (this.Width - _symbolWidth) / 2;
                _labelWidth = _valueWidth;

                _vScrollBar.Top = 0;
                _vScrollBar.Height = this.Height;
                _vScrollBar.Left = this.Width - _vScrollBar.Width;
            }

            this.Invalidate();
        }

        private void _vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            VScrollBar sbar = (VScrollBar)sender;
            sbar.Value = e.NewValue;
            this.Invalidate();;
        }        

        /// <summary>
        /// Override OnMouseClick event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Left)
            {
                if (TB_Editor.Visible)
                    AfterCellEdit();

                int curTop = 0;
                int bIdx = GetBreakIndexByPosition(e.X, e.Y, ref curTop);
                if (bIdx >= 0)
                {
                    if ((ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        if (_selectedRows.Contains(bIdx))
                            _selectedRows.Remove(bIdx);
                        else
                        {
                            _selectedRows.Add(bIdx);
                            _startRow = bIdx;
                        }
                    }
                    else if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        _selectedRows.Clear();
                        if (_startRow == -1)
                            _selectedRows.Add(bIdx);
                        else
                        {
                            if (bIdx > _startRow)
                            {
                                for (int i = _startRow; i <= bIdx; i++)
                                    _selectedRows.Add(i);
                            }
                            else
                            {
                                for (int i = bIdx; i <= _startRow; i++)
                                    _selectedRows.Add(i);
                            }
                        }
                    }
                    else
                    {
                        _selectedRows.Clear();
                        _selectedRows.Add(bIdx);
                        _startRow = bIdx;
                    }

                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Override OnMouseDoubleClick event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (e.Button == MouseButtons.Left)
            {
                if (TB_Editor.Visible)
                    AfterCellEdit();

                int curTop = 0;
                int bIdx = GetBreakIndexByPosition(e.X, e.Y, ref curTop);
                if (bIdx >= 0)
                {
                    if ((ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        if (_selectedRows.Contains(bIdx))
                            _selectedRows.Remove(bIdx);
                        else
                        {
                            _selectedRows.Add(bIdx);
                            _startRow = bIdx;
                        }
                    }
                    else if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        _selectedRows.Clear();
                        if (_startRow == -1)
                            _selectedRows.Add(bIdx);
                        else
                        {
                            if (bIdx > _startRow)
                            {
                                for (int i = _startRow; i <= bIdx; i++)
                                    _selectedRows.Add(i);
                            }
                            else
                            {
                                for (int i = bIdx; i <= _startRow; i++)
                                    _selectedRows.Add(i);
                            }
                        }
                    }
                    else
                    {
                        _selectedRows.Clear();
                        _selectedRows.Add(bIdx);
                        _startRow = bIdx;
                    }

                    ColorBreak aCB = _legendScheme.LegendBreaks[bIdx];
                    _curBreak = aCB;
                    if (IsInSymbol(e.X))
                    {                        
                        ShowSymbolSetForm(aCB);
                    }
                    else if (IsInValue(e.X))
                    {
                        TB_Editor.Left = _symbolWidth;
                        TB_Editor.Top = curTop;
                        TB_Editor.Width = _valueWidth;
                        TB_Editor.Height = _breakHeight;
                        TB_Editor.Text = aCB.GetValueString();
                        TB_Editor.Tag = "Value";
                        TB_Editor.Visible = true;
                    }
                    else if (IsInLabel(e.X))
                    {
                        if (_legendScheme.LegendType == LegendType.SingleSymbol)
                            TB_Editor.Left = _symbolWidth;
                        else
                            TB_Editor.Left = _symbolWidth + _valueWidth;
                        TB_Editor.Top = curTop;
                        TB_Editor.Width = _valueWidth;
                        TB_Editor.Height = _breakHeight;
                        TB_Editor.Text = aCB.Caption;
                        TB_Editor.Tag = "Label";
                        TB_Editor.Visible = true;
                    }
                }                
            }
        }

        private ColorBreak GetBreakByPosition(int x, int y, ref int curTop)
        {
            ColorBreak aCB = null;            
            int idx = GetBreakIndexByPosition(x, y, ref curTop);
            if (idx >= 0)
                aCB = _legendScheme.LegendBreaks[idx];
                        
            return aCB;
        }

        private int GetBreakIndexByPosition(int x, int y, ref int curTop)
        {
            int idx = -1;
            curTop = 0;
            if (_vScrollBar.Visible == true)
                curTop = curTop - _vScrollBar.Value;

            for (int i = 0; i < _legendScheme.BreakNum; i++)
            {
                curTop += _breakHeight;
                if (y > curTop && y < curTop + _breakHeight)
                {
                    idx = i;
                    break;
                }
            }

            return idx;
        }

        private bool IsInSymbol(int x)
        {
            if (x > 0 && x < _symbolWidth)
                return true;
            else
                return false;
        }

        private bool IsInValue(int x)
        {
            if (_legendScheme.LegendType == LegendType.SingleSymbol)
                return false;
            else
            {
                if (x > _symbolWidth && x < _symbolWidth + _valueWidth)
                    return true;
                else
                    return false;
            }
        }

        private bool IsInLabel(int x)
        {
            if (_legendScheme.LegendType == LegendType.SingleSymbol)
            {
                if (x > _symbolWidth && x < _symbolWidth + _labelWidth)
                    return true;
                else
                    return false;
            }
            else
            {
                if (x > _symbolWidth + _valueWidth && x < _symbolWidth + _valueWidth + _labelWidth)
                    return true;
                else
                    return false;
            }
        }

        private void ShowSymbolSetForm(ColorBreak aCB)
        {
            switch (_legendScheme.ShapeType)
            {
                case ShapeTypes.Point:
                    PointBreak aPB = (PointBreak)aCB;
                    
                    if (!_frmPointSymbolSet.Visible)
                    {
                        _frmPointSymbolSet = new frmPointSymbolSet(this);
                        _frmPointSymbolSet.Show(this);
                    }
                    _frmPointSymbolSet.PointBreak = aPB;
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineZ:
                    PolyLineBreak aPLB = (PolyLineBreak)aCB;
                    
                    if (!_frmPolylineSymbolSet.Visible)
                    {
                        _frmPolylineSymbolSet = new frmPolylineSymbolSet(this);
                        _frmPolylineSymbolSet.Show(this);
                    }
                    _frmPolylineSymbolSet.PolylineBreak = aPLB;
                    break;
                case ShapeTypes.Polygon:
                    PolygonBreak aPGB = (PolygonBreak)aCB;
                    
                    if (!_frmPolygonSymbolSet.Visible)
                    {
                        _frmPolygonSymbolSet = new frmPolygonSymbolSet(this);
                        _frmPolygonSymbolSet.Show(this);
                    }
                    _frmPolygonSymbolSet.PolygonBreak = aPGB;
                    break;
                case ShapeTypes.Image:                    
                    if (!_frmColorSymbolSet.Visible)
                    {
                        _frmColorSymbolSet = new frmColorSymbolSet(this);
                        _frmColorSymbolSet.Show(this);
                    }
                    _frmColorSymbolSet.ColorBreak = aCB;
                    break;
            }
        }                

        private void TB_Editor_Leave(object sender, EventArgs e)
        {
            AfterCellEdit();
        }

        private void AfterCellEdit()
        {
            if (TB_Editor.Tag.ToString() == "Value")
            {
                string aValue = TB_Editor.Text.Trim();
                double sValue, eValue;
                int aIdx;

                aIdx = aValue.IndexOf("-");
                if (aIdx > 0)
                {
                    if (aValue.Substring(aIdx - 1, 1) == "E")
                        aIdx = aValue.IndexOf("-", aIdx + 1);
                    sValue = Convert.ToDouble(aValue.Substring(0, aIdx).Trim());
                    eValue = Convert.ToDouble(aValue.Substring(aIdx + 1).Trim());
                    aValue = aValue.Substring(0, aIdx).Trim() + " - " + aValue.Substring(aIdx + 1).Trim();
                    _curBreak.StartValue = sValue;
                    _curBreak.EndValue = eValue;
                }
                else if (aIdx == 0)
                {
                    aIdx = aValue.Substring(1).IndexOf("-");
                    if (aIdx > 0)
                    {
                        aIdx += 1;
                        sValue = Convert.ToDouble(aValue.Substring(0, aIdx).Trim());
                        eValue = Convert.ToDouble(aValue.Substring(aIdx + 1).Trim());
                        aValue = aValue.Substring(0, aIdx).Trim() + " - " + aValue.Substring(aIdx + 1).Trim();
                    }
                    else
                    {
                        sValue = Convert.ToDouble(aValue);
                        eValue = sValue;
                    }
                    _curBreak.StartValue = sValue;
                    _curBreak.EndValue = eValue;
                }
                else
                {
                    if (MIMath.IsNumeric(aValue))
                    {
                        sValue = Convert.ToDouble(aValue);
                        eValue = sValue;
                    }
                    _curBreak.StartValue = aValue;
                    _curBreak.EndValue = aValue;
                }

                _curBreak.Caption = aValue;                
            }
            else if (TB_Editor.Tag.ToString() == "Label")
            {
                string caption = TB_Editor.Text.Trim();
                _curBreak.Caption = caption;
            }

            TB_Editor.Visible = false;
            this.Validate();
        }

        #endregion
    }
}
