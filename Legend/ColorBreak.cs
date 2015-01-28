using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Color break
    /// </summary>
    public class ColorBreak
    {
        #region Variables      
        private BreakTypes _breakType;
        private object _startValue;        
        private object _endValue;        
        private Color _color;                
        private string _caption;                
        private bool _isNoData;
        private bool _drawShape;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ColorBreak()
        {
            _breakType = BreakTypes.ColorBreak;
            _color = Color.Red;
            _isNoData = false;
            _drawShape = true;
            _startValue = 0;
            _endValue = 0;
            _caption = "";
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set break type
        /// </summary>
        public BreakTypes BreakType
        {
            get { return _breakType; }
            set { _breakType = value; }
        }

        /// <summary>
        /// Get or set start value
        /// </summary>
        public object StartValue
        {
            get { return _startValue; }
            set { _startValue = value; }
        }

        /// <summary>
        /// Get or set end value
        /// </summary>
        public object EndValue
        {
            get { return _endValue; }
            set { _endValue = value; }
        }

        /// <summary>
        /// Get or set color
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Get or set caption
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        /// <summary>
        /// Get or set if is undefine data
        /// </summary>
        public bool IsNoData
        {
            get { return _isNoData; }
            set { _isNoData = value; }
        }

        /// <summary>
        /// Get or set if draw shape
        /// </summary>
        public bool DrawShape
        {
            get { return _drawShape; }
            set { _drawShape = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get property object
        /// </summary>
        /// <returns>custom property object</returns>
        public virtual object GetPropertyObject()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("Color", "Color");            
            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>object</returns>
        public virtual object Clone()
        {
            //return MemberwiseClone();
            ColorBreak aCB = new ColorBreak();
            aCB.Caption = _caption;
            aCB.Color = _color;
            aCB.DrawShape = _drawShape;
            aCB.EndValue = _endValue;
            aCB.IsNoData = _isNoData;
            aCB.StartValue = _startValue;

            return aCB;
        }

        /// <summary>
        /// Export to XML document
        /// </summary>
        /// <param name="doc">xml document</param>
        /// <param name="parent">parent xml element</param>
        public virtual void ExportToXML(ref XmlDocument doc, XmlElement parent)
        {
            XmlElement brk = doc.CreateElement("Break");
            XmlAttribute caption = doc.CreateAttribute("Caption");
            XmlAttribute startValue = doc.CreateAttribute("StartValue");
            XmlAttribute endValue = doc.CreateAttribute("EndValue");
            XmlAttribute color = doc.CreateAttribute("Color");
            XmlAttribute isNoData = doc.CreateAttribute("IsNoData");

            caption.InnerText = _caption;
            startValue.InnerText = _startValue.ToString();
            endValue.InnerText = _endValue.ToString();
            color.InnerText = ColorTranslator.ToHtml(_color);
            isNoData.InnerText = _isNoData.ToString();

            brk.Attributes.Append(caption);
            brk.Attributes.Append(startValue);
            brk.Attributes.Append(endValue);
            brk.Attributes.Append(color);
            brk.Attributes.Append(isNoData);

            parent.AppendChild(brk);
        }

        /// <summary>
        /// Get value string
        /// </summary>
        /// <returns>value string</returns>
        public string GetValueString()
        {
            if (_startValue.ToString() == _endValue.ToString())
                return _startValue.ToString();
            else
                return _startValue.ToString() + " - " + _endValue.ToString();
        }

        #endregion
    }
}
