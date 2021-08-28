using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteoInfoC.Drawing;
using System.Xml;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Legend scheme break of point
    /// </summary>
    public class PointBreak:ColorBreak
    {
        #region Variables
        private MarkerType _markerType;
        /// <summary>
        /// Outline color
        /// </summary>
        private Color _outlineColor;        
        /// <summary>
        /// Size
        /// </summary>
        private Single _size;
        /// <summary>
        /// Style
        /// </summary>
        private PointStyle _style;        
        /// <summary>
        /// If draw outline
        /// </summary>
        private Boolean _drawOutline;
        /// <summary>
        /// If draw fill
        /// </summary>
        private bool _drawFill;
        private string _fontName;
        private int _charIndex;
        private string _imagePath;
        private float _angle;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PointBreak():base()
        {
            BreakType = BreakTypes.PointBreak;
            _markerType = MarkerType.Simple;
            _fontName = "Arial";
            _charIndex = 0;
            _outlineColor = Color.Black;
            _size = 1.0f;
            _style = PointStyle.Circle;
            _drawOutline = true;
            _drawFill = true;
            _angle = 0;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set marker type
        /// </summary>
        public MarkerType MarkerType
        {
            get { return _markerType; }
            set { _markerType = value; }
        }

        /// <summary>
        /// Get or set font name for character marker
        /// </summary>
        public string FontName
        {
            get { return _fontName; }
            set { _fontName = value; }
        }

        /// <summary>
        /// Get or set character index
        /// </summary>
        public int CharIndex
        {
            get { return _charIndex; }
            set { _charIndex = value; }
        }

        /// <summary>
        /// Get or set image path
        /// </summary>
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }

        /// <summary>
        /// Get or set outline color
        /// </summary>
        public Color OutlineColor
        {
            get { return _outlineColor; }
            set { _outlineColor = value; }
        }

        /// <summary>
        /// Get or set point size
        /// </summary>
        public Single Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// Get or set point style
        /// </summary>
        public PointStyle Style
        {
            get { return _style; }
            set { _style = value; }
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
        /// Get property object
        /// </summary>
        /// <returns>custom property object</returns>
        public override object GetPropertyObject()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("Color", "Color");
            objAttr.Add("OutlineColor", "OutlineColor");
            objAttr.Add("Size", "Size");
            objAttr.Add("Style", "Style");
            objAttr.Add("DrawOutline", "DrawOutline");
            objAttr.Add("DrawFill", "DrawFill");
            objAttr.Add("DrawPoint", "DrawPoint");
            objAttr.Add("Angle", "Angle");
            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;
        }

        /// <summary>
        /// Override Clone method
        /// </summary>
        /// <returns>object</returns>
        public override object Clone()
        {
            PointBreak aCB = new PointBreak();
            aCB.Caption = this.Caption;
            aCB.Color = this.Color;
            aCB.DrawShape = this.DrawShape;
            aCB.EndValue = this.EndValue;
            aCB.IsNoData = this.IsNoData;
            aCB.StartValue = this.StartValue;
            aCB.MarkerType = _markerType;
            aCB.FontName = _fontName;
            aCB.CharIndex = _charIndex;
            aCB.ImagePath = _imagePath;
            aCB.OutlineColor = _outlineColor;
            aCB.Size = _size;
            aCB.DrawOutline = _drawOutline;
            aCB.DrawFill = _drawFill;
            aCB.Style = _style;
            aCB.Angle = _angle;

            return aCB;
        }

        /// <summary>
        /// Export to XML document
        /// </summary>
        /// <param name="doc">xml document</param>
        /// <param name="parent">parent xml element</param>
        public override void ExportToXML(ref XmlDocument doc, XmlElement parent)
        {
            XmlElement brk = doc.CreateElement("Break");
            XmlAttribute caption = doc.CreateAttribute("Caption");
            XmlAttribute startValue = doc.CreateAttribute("StartValue");
            XmlAttribute endValue = doc.CreateAttribute("EndValue");
            XmlAttribute color = doc.CreateAttribute("Color");
            XmlAttribute isNoData = doc.CreateAttribute("IsNoData");

            caption.InnerText = Caption;
            startValue.InnerText = StartValue.ToString();
            endValue.InnerText = EndValue.ToString();
            color.InnerText = ColorTranslator.ToHtml(Color);
            isNoData.InnerText = IsNoData.ToString();

            brk.Attributes.Append(caption);
            brk.Attributes.Append(startValue);
            brk.Attributes.Append(endValue);
            brk.Attributes.Append(color);
            brk.Attributes.Append(isNoData);

            parent.AppendChild(brk);
        }

        #endregion
    }
}
