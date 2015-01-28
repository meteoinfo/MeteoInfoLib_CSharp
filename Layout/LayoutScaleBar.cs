using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace MeteoInfoC.Layout
{
    /// <summary>
    /// Layout scale bar
    /// </summary>
    public class LayoutScaleBar:LayoutElement
    {
        #region Variables
        private LayoutMap _layoutMap;
        private SmoothingMode _smoothingMode;
        private Font _font;
        private ScaleBarTypes _scaleBarType;
        private ScaleBarUnits _unit;
        private string _unitText;
        private int _numBreaks;
        private bool _drawNeatLine;
        private Color _neatLineColor;
        private int _neatLineSize;
        private bool _drawScaleText;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="layoutMap">Layout map</param>
        public LayoutScaleBar(LayoutMap layoutMap):base()
        {
            ElementType = ElementType.LayoutScaleBar;
            ResizeAbility = ResizeAbility.ResizeAll;

            Width = 200;
            Height = 50;
            //BackColor = Color.YellowGreen;
            _layoutMap = layoutMap;
            _smoothingMode = SmoothingMode.AntiAlias;
            _scaleBarType = ScaleBarTypes.ScaleLine1;
            _drawNeatLine = false;
            _neatLineColor = Color.Black;
            _neatLineSize = 1;
            _font = new Font("Arial", 8);
            _unit = ScaleBarUnits.Kilometers;
            _unitText = "km";
            _numBreaks = 4;
            _drawScaleText = false;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set layout map
        /// </summary>
        public LayoutMap LayoutMap
        {
            get { return _layoutMap; }
        }

        /// <summary>
        /// Get or set scale bar type
        /// </summary>
        public ScaleBarTypes ScaleBarType
        {
            get { return _scaleBarType; }
            set { _scaleBarType = value; }
        }

        /// <summary>
        /// Get or set if draw neat line
        /// </summary>
        public bool DrawNeatLine
        {
            get { return _drawNeatLine; }
            set { _drawNeatLine = value; }
        }

        /// <summary>
        /// Get or set neat line color
        /// </summary>
        public Color NeatLineColor
        {
            get { return _neatLineColor; }
            set { _neatLineColor = value; }
        }

        /// <summary>
        /// Get or set neat line size
        /// </summary>
        public int NeatLineSize
        {
            get { return _neatLineSize; }
            set { _neatLineSize = value; }
        }

        /// <summary>
        /// Get or set label text font
        /// </summary>
        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        /// <summary>
        /// Get or set break number
        /// </summary>
        public int BreakNumber
        {
            get { return _numBreaks; }
            set { _numBreaks = value; }
        }

        /// <summary>
        /// Get or set if draw scale text
        /// </summary>
        public bool DrawScaleText
        {
            get { return _drawScaleText; }
            set { _drawScaleText = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Override get property object methods
        /// </summary>
        /// <returns>property object</returns>
        public override object GetPropertyObject()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("ScaleBarType", "ScaleBarType");
            objAttr.Add("DrawNeatLine", "DrawNeatLine");
            objAttr.Add("NeatLineColor", "NeatLineColor");
            objAttr.Add("NeatLineSize", "NeatLineSize");
            objAttr.Add("Font", "Font");
            objAttr.Add("BackColor", "Back Color");
            objAttr.Add("ForeColor", "Fore Color");
            objAttr.Add("DrawScaleText", "DrawScaleText");
            objAttr.Add("Left", "Left");
            objAttr.Add("Top", "Top");
            objAttr.Add("Width", "Width");
            objAttr.Add("Height", "Height");
            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;
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

        /// <summary>
        /// Paint graphics
        /// </summary>
        /// <param name="g">graphics</param>
        /// <param name="pageLocation">page location</param>
        /// <param name="zoom">zoom</param>
        public void PaintGraphics(Graphics g, PointF pageLocation, float zoom)
        {
            Matrix oldMatrix = g.Transform;
            PointF aP = PageToScreen(this.Left, this.Top, pageLocation, zoom);
            g.TranslateTransform(aP.X, aP.Y);
            g.Transform.Scale(zoom, zoom);
            g.SmoothingMode = _smoothingMode;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            //Draw background color
            g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.Width * zoom, this.Height * zoom);

            DrawScaleBar(g, zoom);            

            //Draw neatline
            if (_drawNeatLine)
            {
                Rectangle mapRect = new Rectangle(_neatLineSize - 1, _neatLineSize - 1,
                    (int)((this.Width - _neatLineSize) * zoom), (int)((this.Height - _neatLineSize) * zoom));
                g.DrawRectangle(new Pen(_neatLineColor, _neatLineSize), mapRect);
            }

            g.Transform = oldMatrix;
        }

        /// <summary>
        /// Paint graphics
        /// </summary>
        /// <param name="g">graphics</param>
        public void PaintGraphics(Graphics g)
        {
            PaintGraphics(g, new PointF(0, 0), 1.0f);
        }

        private void DrawScaleBar(Graphics g, float zoom)
        {
            //Sets up the pens and brushes
            Brush scaleBrush = new SolidBrush(ForeColor);
            Pen scalePen = new Pen(scaleBrush);

            Font aFont = new Font(_font.Name, _font.Size * zoom, _font.Style);
            //Calculates the width of one break in greographic units
            float unitLegnth = g.MeasureString(_unitText, aFont).Width * 2;
            float widthNoUnit = (this.Width * zoom - unitLegnth);
            Int64 geoBreakWidth = (Int64)(GetGeoWidth(widthNoUnit / _numBreaks));

            //If the geobreakWidth is less than 1 we return and don't draw anything
            if (geoBreakWidth < 1)
                return;

            double n = Math.Pow(10, geoBreakWidth.ToString().Length - 1);
            geoBreakWidth = Convert.ToInt64(Math.Floor(geoBreakWidth / n) * n);

            Int64 breakWidth = (Int64)(GetWidth(geoBreakWidth));
            float fontHeight = g.MeasureString(geoBreakWidth.ToString(), _font).Height * zoom;
            float leftStart = g.MeasureString(Math.Abs(geoBreakWidth).ToString(), _font).Width / 2F;            

            //Draw scale text
            double scale = geoBreakWidth * GetConversionFactor(_unit) * 100 / (breakWidth / g.DpiX * 2.539999918) * zoom;
            if (_drawScaleText)
                g.DrawString("1 : " + String.Format("{0:0,0}", scale),
                    aFont, scaleBrush, leftStart - (g.MeasureString(Math.Abs(0).ToString(), aFont).Width / 2), fontHeight * 2.5F);

            //Draw scale bar
            switch (_scaleBarType)
            {
                case ScaleBarTypes.ScaleLine1:
                    DrawScaleLine1(g, zoom, aFont, breakWidth, geoBreakWidth);
                    break;
                case ScaleBarTypes.ScaleLine2:
                    DrawScaleLine2(g, zoom, aFont, breakWidth, geoBreakWidth);
                    break;
                case ScaleBarTypes.AlternatingBar:
                    DrawAlternatingBar(g, zoom, aFont, breakWidth, geoBreakWidth);
                    break;
            }
        }

        private double GetConversionFactor(ScaleBarUnits unit)
        {
            switch (unit)
            {
                case ScaleBarUnits.Kilometers:
                    return 1000;
                default:
                    return 1;
            }
        }

        private double GetGeoWidth(double width)
        {
            double geoWidth = width / _layoutMap.MapFrame.MapView.XScale / GetConversionFactor(_unit);
            if (_layoutMap.MapFrame.MapView.Projection.IsLonLatMap)
                geoWidth = geoWidth * GetLonDistScale();

            return geoWidth;
        }

        private double GetWidth(double geoWidth)
        {
            double width = geoWidth * _layoutMap.MapFrame.MapView.XScale * GetConversionFactor(_unit);
            if (_layoutMap.MapFrame.MapView.Projection.IsLonLatMap)
                width = width / GetLonDistScale();

            return width;
        }

        private double GetLonDistScale()
        {
            //Get meters of one longitude degree
            double pY = (_layoutMap.MapFrame.MapView.ViewExtent.maxY + _layoutMap.MapFrame.MapView.ViewExtent.minY) / 2;
            double ProjX = 0, ProjY = pY, pProjX = 1, pProjY = pY;
            double dx = Math.Abs(ProjX - pProjX);
            double dy = Math.Abs(ProjY - pProjY);
            double dist;
            double y = (ProjY + pProjY) / 2;
            double factor = Math.Cos(y * Math.PI / 180);
            dx *= factor;
            dist = Math.Sqrt(dx * dx + dy * dy);
            dist = dist * 111319.5;

            return dist;
        }

        private void DrawScaleLine1(Graphics g, float zoom, Font aFont, long breakWidth, long geoBreakWidth)
        {
            Brush scaleBrush = new SolidBrush(ForeColor);
            Pen scalePen = new Pen(scaleBrush);
            float fontHeight = g.MeasureString(geoBreakWidth.ToString(), _font).Height * zoom;
            float leftStart = g.MeasureString(Math.Abs(geoBreakWidth).ToString(), _font).Width / 2F;
            int yShift = 10;

            g.DrawLine(scalePen, leftStart, fontHeight * 1.6f + yShift, leftStart + (breakWidth * _numBreaks), fontHeight * 1.6f + yShift);
            for (int i = 0; i <= _numBreaks; i++)
            {
                g.DrawLine(scalePen, leftStart, fontHeight * 1.1f + yShift, leftStart, fontHeight * 1.6f + yShift);
                g.DrawString(Math.Abs(geoBreakWidth * i).ToString(), aFont, scaleBrush, leftStart - (g.MeasureString(Math.Abs(geoBreakWidth * i).ToString(), aFont).Width / 2), yShift);
                leftStart = leftStart + breakWidth;
            }
            g.DrawString(_unitText, aFont, scaleBrush, leftStart - breakWidth + (fontHeight / 2), fontHeight * 1.1f + yShift);
        }

        private void DrawScaleLine2(Graphics g, float zoom, Font aFont, long breakWidth, long geoBreakWidth)
        {
            Brush scaleBrush = new SolidBrush(ForeColor);
            Pen scalePen = new Pen(scaleBrush);
            float fontHeight = g.MeasureString(geoBreakWidth.ToString(), _font).Height * zoom;
            float leftStart = g.MeasureString(Math.Abs(geoBreakWidth).ToString(), _font).Width / 2F;
            int yShift = 5;

            g.DrawLine(scalePen, leftStart, fontHeight * 1.6f + yShift, leftStart + (breakWidth * _numBreaks), fontHeight * 1.6f + yShift);
            for (int i = 0; i <= _numBreaks; i++)
            {
                g.DrawLine(scalePen, leftStart, fontHeight * 1.1f + yShift, leftStart, fontHeight + (fontHeight * 1.1f) + yShift);
                g.DrawString(Math.Abs(geoBreakWidth * i).ToString(), aFont, scaleBrush, leftStart - (g.MeasureString(Math.Abs(geoBreakWidth * i).ToString(), aFont).Width / 2), yShift);
                leftStart = leftStart + breakWidth;
            }
            g.DrawString(_unitText, aFont, scaleBrush, leftStart - breakWidth + (fontHeight / 2), fontHeight * 1.1f + yShift);
        }

        private void DrawAlternatingBar(Graphics g, float zoom, Font aFont, long breakWidth, long geoBreakWidth)
        {
            Brush scaleBrush = new SolidBrush(ForeColor);
            Pen scalePen = new Pen(scaleBrush);
            float fontHeight = g.MeasureString(geoBreakWidth.ToString(), _font).Height * zoom;
            float leftStart = g.MeasureString(Math.Abs(geoBreakWidth).ToString(), _font).Width / 2F;
            int yShift = 5;
            float rHeight = fontHeight / 2;
            
            bool isFill = false;
            for (int i = 0; i <= _numBreaks; i++)
            {
                if (i < _numBreaks)
                {
                    if (isFill)
                        g.FillRectangle(scaleBrush, leftStart, fontHeight * 1.1f + yShift, breakWidth, rHeight);
                    else
                        g.DrawRectangle(scalePen, leftStart, fontHeight * 1.1f + yShift, breakWidth, rHeight);
                }
         
                g.DrawString(Math.Abs(geoBreakWidth * i).ToString(), aFont, scaleBrush, leftStart - (g.MeasureString(Math.Abs(geoBreakWidth * i).ToString(), aFont).Width / 2), yShift);
                leftStart = leftStart + breakWidth;
                isFill = !isFill;
            }
            g.DrawString(_unitText, aFont, scaleBrush, leftStart - breakWidth + (fontHeight / 2), fontHeight * 1.1f + yShift);
        }

        #endregion
        
    }
}
