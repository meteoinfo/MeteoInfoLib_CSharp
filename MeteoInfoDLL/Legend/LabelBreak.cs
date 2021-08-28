using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Label break
    /// </summary>
    public class LabelBreak:ColorBreak
    {
        #region Events definition
        /// <summary>
        /// Occurs after font, text changed
        /// </summary>
        public event EventHandler SizeChanged;

        #endregion

        #region Private variables
        private string _text;
        private float _angle;        
        private Font _font;
        private AlignType _alignType;
        private float _xShift;
        private float _yShift;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LabelBreak():base()
        {
            BreakType = BreakTypes.LabelBreak;
            _text = "";
            _angle = 0;
            Color = Color.Black;
            _font = new Font("Arial", 7);
            _alignType = AlignType.Center;
            _yShift = 0;
        }

        #endregion

        #region Properties        
        /// <summary>
        /// Get or set label text
        /// </summary>
        public string Text
        {
            get { return _text; }
            set 
            { 
                _text = value;
                OnSizeChanged();
            }
        }

        /// <summary>
        /// Get or set label angle
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }        

        /// <summary>
        /// Get or set label font
        /// </summary>
        public Font Font
        {
            get { return _font; }
            set 
            { 
                _font = value;
                OnSizeChanged();
            }
        }

        /// <summary>
        /// Get or set label align type
        /// </summary>
        public AlignType AlignType
        {
            get { return _alignType; }
            set { _alignType = value; }
        }

        /// <summary>
        /// Get or set label y shift
        /// </summary>
        public float YShift
        {
            get { return _yShift; }
            set { _yShift = value; }
        }

        /// <summary>
        /// Get or set label x shift
        /// </summary>
        public float XShift
        {
            get { return _xShift; }
            set { _xShift = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get property object
        /// </summary>
        /// <returns>property object</returns>
        public override object GetPropertyObject()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("Text", "Text");
            objAttr.Add("Angle", "Angle");
            objAttr.Add("Color", "Color");
            objAttr.Add("Font", "Font");
            //objAttr.Add("AlignType", "AlignType");
            //objAttr.Add("YShift", "YShift");
            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;
        }

        /// <summary>
        /// Override Clone method
        /// </summary>
        /// <returns>object</returns>
        public override object Clone()
        {
            LabelBreak aCB = new LabelBreak();
            aCB.Caption = this.Caption;
            aCB.Color = this.Color;
            aCB.DrawShape = this.DrawShape;
            aCB.EndValue = this.EndValue;
            aCB.IsNoData = this.IsNoData;
            aCB.StartValue = this.StartValue;            
            aCB.Angle = _angle;
            aCB.Text = _text;
            aCB.Font = _font;
            aCB.AlignType = _alignType;
            aCB.YShift = _yShift;
            aCB.XShift = _xShift;

            return aCB;
        }

        #endregion

        #region Events
        /// <summary>
        /// Fire the size changed event
        /// </summary>
        protected virtual void OnSizeChanged()
        {
            if (SizeChanged != null) SizeChanged(this, new EventArgs());
        }

        #endregion
    }
}
