using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteoInfoC.Shape;
using MeteoInfoC.Legend;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// Label set
    /// </summary>
    public class LabelSet
    {
        #region Private Variables  
        private bool _drawLabels;
        private string _fieldName;        
        private Font _labelFont;
        private Color _labelColor;
        private bool _drawShadow;
        private Color _ShadowColor;
        private AlignType _labelAlignType;
        private int _xOffset;
        private int _yOffset;
        private bool _avoidCollision;
        private bool _colorByLegend;
        private bool _dynamicContourLabel;
        private bool _autoDecimal;
        private int _decimalDigits;
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LabelSet()
        {
            _drawLabels = false;
            _fieldName = "";
            _labelFont = new Font("Arial", 7);
            _labelColor = Color.Black;
            _drawShadow = false;
            _ShadowColor = Color.White;
            _labelAlignType = AlignType.Center;
            _xOffset = 0;
            _yOffset = 0;
            _avoidCollision = true;
            _colorByLegend = false;
            _dynamicContourLabel = false;
            _autoDecimal = true;
            _decimalDigits = 2;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get or set if draw labels
        /// </summary>
        public bool DrawLabels
        {
            get { return _drawLabels; }
            set { _drawLabels = value; }
        }

        /// <summary>
        /// Get or set label field name
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }             

        /// <summary>
        /// Get or set label font
        /// </summary>
        public Font LabelFont
        {
            get { return _labelFont; }
            set { _labelFont = value; }
        }

        /// <summary>
        /// Get or set label color
        /// </summary>
        public Color LabelColor
        {
            get { return _labelColor; }
            set { _labelColor = value; }
        }

        /// <summary>
        /// Get or set if show shadow
        /// </summary>
        public bool DrawShadow
        {
            get { return _drawShadow; }
            set { _drawShadow = value; }
        }

        /// <summary>
        /// Get or set shadow color
        /// </summary>
        public Color ShadowColor
        {
            get { return _ShadowColor; }
            set { _ShadowColor = value; }
        }

        /// <summary>
        /// Get or set label align type
        /// </summary>
        public AlignType LabelAlignType
        {
            get { return _labelAlignType; }
            set { _labelAlignType = value; }
        }

        /// <summary>
        /// Get or set x offset
        /// </summary>
        public int XOffset
        {
            get { return _xOffset; }
            set { _xOffset = value; }
        }

        /// <summary>
        /// Get or set y offset
        /// </summary>
        public int YOffset
        {
            get { return _yOffset; }
            set { _yOffset = value; }
        }

        /// <summary>
        /// Get or set if avoid collision
        /// </summary>
        public bool AvoidCollision
        {
            get { return _avoidCollision; }
            set { _avoidCollision = value; }
        }

        /// <summary>
        /// Get or set if set color by legend
        /// </summary>
        public bool ColorByLegend
        {
            get { return _colorByLegend; }
            set { _colorByLegend = value; }
        }

        /// <summary>
        /// Get or set if using dynamic contour label
        /// </summary>
        public bool DynamicContourLabel
        {
            get { return _dynamicContourLabel; }
            set { _dynamicContourLabel = value; }
        }

        /// <summary>
        /// Get or set if automatic set decimal digits
        /// </summary>
        public bool AutoDecimal
        {
            get { return _autoDecimal; }
            set { _autoDecimal = value; }
        }

        /// <summary>
        /// Get or set decimal digits
        /// </summary>
        public int DecimalDigits
        {
            get { return _decimalDigits; }
            set { _decimalDigits = value; }
        }
        
        #endregion

        #region Methods
        

        #endregion
    }
}
