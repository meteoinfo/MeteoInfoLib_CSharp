using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Legend scheme break of polygon
    /// </summary>
    public class PolygonBreak:ColorBreak
    {
        #region Variables
        /// <summary>
        /// Outline color
        /// </summary>
        private Color _outlineColor;
        /// <summary>
        /// Outline size
        /// </summary>
        private Single _outlineSize;        
        /// <summary>
        /// If draw outline
        /// </summary>
        private bool _drawOutline;
        /// <summary>
        /// If draw fill
        /// </summary>
        private bool _drawFill;
        /// <summary>
        /// If draw polygon
        /// </summary>        
        private bool _usingHatchStyle;
        private HatchStyle _style;
        private Color _backColor;
        //private int _transparencyPerc;
        private bool _isMaskout;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PolygonBreak():base()
        {
            BreakType = BreakTypes.PolygonBreak;
            _outlineColor = Color.Black;
            _outlineSize = 1.0f;
            _drawOutline = true;
            _drawFill = true;            
            _usingHatchStyle = false;
            _style = HatchStyle.Horizontal;
            _backColor = Color.Transparent;
            //_transparencyPerc = 0;
            _isMaskout = false;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set polygon outline color
        /// </summary>
        public Color OutlineColor
        {
            get { return _outlineColor; }
            set { _outlineColor = value; }
        }

        /// <summary>
        /// Get or set outline size
        /// </summary>
        public Single OutlineSize
        {
            get { return _outlineSize; }
            set { _outlineSize = value; }
        }

        /// <summary>
        /// Get or set if draw outline
        /// </summary>
        public bool DrawOutline
        {
            get { return _drawOutline; }
            set { _drawOutline = value; }
        }

        /// <summary>
        /// Get or set if draw fill
        /// </summary>
        public bool DrawFill
        {
            get { return _drawFill; }
            set { _drawFill = value; }
        }        

        /// <summary>
        /// Get or set if using hatch style
        /// </summary>
        public bool UsingHatchStyle
        {
            get { return _usingHatchStyle; }
            set { _usingHatchStyle = value; }
        }

        /// <summary>
        /// Get or set hatch style
        /// </summary>
        public HatchStyle Style
        {
            get { return _style; }
            set { _style = value; }
        }

        /// <summary>
        /// Get or set back color
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        ///// <summary>
        ///// Get or set fill color transparency percent
        ///// </summary>
        //public int TransparencyPercent
        //{
        //    get { return _transparencyPerc; }
        //    set { _transparencyPerc = value; }
        //}

        /// <summary>
        /// Get or set if maskout
        /// </summary>
        public bool IsMaskout
        {
            get { return _isMaskout; }
            set { _isMaskout = value; }
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
            objAttr.Add("OutlineColor", "OutlineColor");
            objAttr.Add("OutlineSize", "OutlineSize");
            objAttr.Add("DrawOutline", "DrawOutline");
            objAttr.Add("DrawFill", "DrawFill");
            objAttr.Add("DrawPolygon", "DrawPolygon");
            objAttr.Add("UsingHatchStyle", "UsingHatchStyle");
            objAttr.Add("Style", "Style");
            objAttr.Add("BackColor", "BackColor");
            objAttr.Add("TransparencyPercent", "TransparencyPercent");
            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;
        }

        /// <summary>
        /// Override Clone method
        /// </summary>
        /// <returns>object</returns>
        public override object Clone()
        {
            PolygonBreak aCB = new PolygonBreak();
            aCB.Caption = this.Caption;
            aCB.Color = this.Color;
            aCB.DrawShape = this.DrawShape;
            aCB.EndValue = this.EndValue;
            aCB.IsNoData = this.IsNoData;
            aCB.StartValue = this.StartValue;            
            aCB.OutlineColor = _outlineColor;
            aCB.OutlineSize = _outlineSize;
            aCB.DrawOutline = _drawOutline;
            aCB.DrawFill = _drawFill;
            aCB.UsingHatchStyle = _usingHatchStyle;
            aCB.Style = _style;
            aCB.BackColor = _backColor;
            //aCB.TransparencyPercent = _transparencyPerc;
            aCB.IsMaskout = _isMaskout;

            return aCB;
        }

        #endregion
    }
}
