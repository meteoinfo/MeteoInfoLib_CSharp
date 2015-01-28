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
    /// Layout north arrow
    /// </summary>
    public class LayoutNorthArrow:LayoutElement
    {
        #region Variables
        private LayoutMap _layoutMap;
        private SmoothingMode _smoothingMode;
        private bool _drawNeatLine;
        private Color _neatLineColor;
        private int _neatLineSize;
        private NorthArrowTypes _northArrowType;
        private float _angle;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="layoutMap">Layout map</param>
        public LayoutNorthArrow(LayoutMap layoutMap):base()
        {
            ElementType = ElementType.LayoutNorthArraw;
            ResizeAbility = ResizeAbility.ResizeAll;

            Width = 50;
            Height = 50;

            _layoutMap = layoutMap;
            _smoothingMode = SmoothingMode.AntiAlias;
            _drawNeatLine = false;
            _neatLineColor = Color.Black;
            _neatLineSize = 1;
            _northArrowType = NorthArrowTypes.NorthArrow1;
            _angle = 0;
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
        /// Get or set angle
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            set { _angle = value; }
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
            objAttr.Add("BackColor", "Back Color");
            objAttr.Add("ForeColor", "Fore Color");
            objAttr.Add("DrawNeatLine", "DrawNeatLine");
            objAttr.Add("NeatLineColor", "NeatLineColor");
            objAttr.Add("NeatLineSize", "NeatLineSize");
            objAttr.Add("Angle", "Angle");
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
        public void PaintGraphics(Graphics g)
        {
            PaintGraphics(g, new PointF(0, 0), 1.0f);
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
            //g.Transform.Scale(zoom, zoom);
            g.ScaleTransform(zoom, zoom);
            if (_angle != 0)
                g.RotateTransform(_angle);
            g.SmoothingMode = _smoothingMode;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            //Draw background color
            g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.Width * zoom, this.Height * zoom);

            DrawNorthArrow(g, zoom);

            //Draw neatline
            if (_drawNeatLine)
            {
                Rectangle mapRect = new Rectangle(_neatLineSize - 1, _neatLineSize - 1,
                    (int)((this.Width - _neatLineSize) * zoom), (int)((this.Height - _neatLineSize) * zoom));
                g.DrawRectangle(new Pen(_neatLineColor, _neatLineSize), mapRect);
            }

            g.Transform = oldMatrix;
        }

        private void DrawNorthArrow(Graphics g, float zoom)
        {
            switch (_northArrowType)
            {
                case NorthArrowTypes.NorthArrow1:
                    DrawNorthArrow1(g, zoom);
                    break;
            }
        }

        private void DrawNorthArrow1(Graphics g, float zoom)
        {
            Brush aBrush = new SolidBrush(ForeColor);
            Pen aPen = new Pen (aBrush);

            //Draw N symbol
            Point[] points = new Point[4];
            int x = Width / 2;
            int y = Height / 6;
            int w = Width / 6;
            int h = Height / 4;
            points[0] = new Point(x - w / 2, y + h / 2);
            points[1] = new Point(x - w / 2, y - h / 2);
            points[2] = new Point(x + w / 2, y + h / 2);
            points[3] = new Point(x + w / 2, y - h / 2);
            g.DrawLines(aPen, points);

            //Draw arrow
            w = Width / 2;
            h = Height * 2 / 3;
            points = new Point[3];
            points[0] = new Point(x - w / 2, Height);
            points[1] = new Point(x, Height - h / 2);
            points[2] = new Point(x, Height - h);
            g.FillPolygon(aBrush, points);

            points[0] = new Point(x + w / 2, Height);
            points[1] = new Point(x, Height - h / 2);
            points[2] = new Point(x, Height - h);
            g.DrawPolygon(aPen, points);
        }

        #endregion

    }
}
