using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using MeteoInfoC.Drawing;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Legend scheme break of polyline
    /// </summary>
    public class PolyLineBreak:ColorBreak
    {
        #region Variables
        /// <summary>
        /// Size
        /// </summary>
        private Single _size;
        /// <summary>
        /// Style
        /// </summary>
        private LineStyles _style;
        /// <summary>
        /// If draw line
        /// </summary>
        private bool _drawPolyline;
        private bool _drawSymbol;
        private Single _symbolSize;
        private PointStyle _symbolStyle;
        private Color _symbolColor;
        private int _symbolInterval;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PolyLineBreak():base()
        {
            BreakType = BreakTypes.PolylineBreak;
            _size = 1.0f;
            _style = LineStyles.Solid;
            _drawPolyline = true;
            _drawSymbol = false;
            _symbolSize = 8.0f;
            _symbolStyle = PointStyle.UpTriangle;
            _symbolColor = Color;
            _symbolInterval = 1;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set polyline size
        /// </summary>
        public Single Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// Get or set line dash style
        /// </summary>
        public LineStyles Style
        {
            get { return _style; }
            set { _style = value; }
        }

        /// <summary>
        /// Get or set if draw polyline
        /// </summary>
        public bool DrawPolyline
        {
            get { return _drawPolyline; }
            set { _drawPolyline = value; }
        }

        /// <summary>
        /// Get or set if draw point symbol
        /// </summary>
        public bool DrawSymbol
        {
            get { return _drawSymbol; }
            set { _drawSymbol = value; }
        }

        /// <summary>
        /// Get or set symbol size
        /// </summary>
        public Single SymbolSize
        {
            get { return _symbolSize; }
            set { _symbolSize = value; }
        }

        /// <summary>
        /// Get or set symbol style
        /// </summary>
        public PointStyle SymbolStyle
        {
            get { return _symbolStyle; }
            set { _symbolStyle = value; }
        }

        /// <summary>
        /// Get or set symbol color
        /// </summary>
        public Color SymbolColor
        {
            get { return _symbolColor; }
            set { _symbolColor = value; }
        }

        /// <summary>
        /// Get or set symbol interval
        /// </summary>
        public int SymbolInterval
        {
            get { return _symbolInterval; }
            set { _symbolInterval = value; }
        }

        /// <summary>
        /// Get if is using DashStyle
        /// </summary>
        public bool IsUsingDashStyle
        {
            get
            {
                switch (_style)
                {
                    case LineStyles.Solid:
                    case LineStyles.Dash:
                    case LineStyles.Dot:
                    case LineStyles.DashDot:
                    case LineStyles.DashDotDot:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Get DashStyle
        /// </summary>
        public DashStyle DashStyle
        {
            get
            {
                DashStyle style = DashStyle.Solid;
                switch (_style)
                {
                    case LineStyles.Dash:
                        style = DashStyle.Dash;
                        break;
                    case LineStyles.Dot:
                        style = DashStyle.Dot;
                        break ;
                    case LineStyles.DashDot:
                        style = DashStyle.DashDot;
                        break ;
                    case LineStyles.DashDotDot:
                        style = DashStyle.DashDotDot;
                        break;
                }

                return style;
            }
            set
            {
                DashStyle style = value;
                switch (style)
                {
                    case DashStyle.Dash:
                        _style = LineStyles.Dash;
                        break;
                    case DashStyle.Dot:
                        _style = LineStyles.Dot;
                        break;
                    case DashStyle.DashDot:
                        _style = LineStyles.DashDot;
                        break ;
                    case DashStyle.DashDotDot:
                        _style = LineStyles.DashDotDot;
                        break;
                    default:
                        _style = LineStyles.Solid;
                        break;
                }
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get property object
        /// </summary>
        /// <returns>custom property object</returns>
        public override object GetPropertyObject()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("Color", "Color");
            objAttr.Add("Size", "Size");
            objAttr.Add("Style", "Style");
            objAttr.Add("DrawPolyline", "DrawPolyline");
            objAttr.Add("DrawSymbol", "DrawSymbol");
            objAttr.Add("SymbolSize", "SymbolSize");
            objAttr.Add("SymbolStyle", "SymbolStyle");
            objAttr.Add("SymbolColor", "SymbolColor");
            objAttr.Add("SymbolInterval", "SymbolInterval");
            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;
        }

        /// <summary>
        /// Override Clone method
        /// </summary>
        /// <returns>object</returns>
        public override object Clone()
        {
            PolyLineBreak aCB = new PolyLineBreak();
            aCB.Caption = this.Caption;
            aCB.Color = this.Color;
            aCB.DrawShape = this.DrawShape;
            aCB.EndValue = this.EndValue;
            aCB.IsNoData = this.IsNoData;
            aCB.StartValue = this.StartValue;            
            aCB.Size = _size;           
            aCB.Style = _style;
            aCB.DrawPolyline = _drawPolyline;
            aCB.DrawSymbol = _drawSymbol;
            aCB.SymbolSize = _symbolSize;
            aCB.SymbolColor = _symbolColor;
            aCB.SymbolStyle = _symbolStyle;
            aCB.SymbolInterval = _symbolInterval;

            return aCB;
        }

        #endregion
    }
}
