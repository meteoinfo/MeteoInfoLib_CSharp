using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Line graph break
    /// </summary>
    public class BarGraphBreak:ColorBreak
    {
        #region Variables                  
        private Single _size;
        private List<string> _fields;   

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public BarGraphBreak(): base()
        {            
            _size = 1.0f;
            _fields = new List<string>();
        }

        #endregion

        #region Properties        

        /// <summary>
        /// Get or set point size
        /// </summary>
        public Single Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// Get fields
        /// </summary>
        public List<string> Fields
        {
            get { return _fields; }
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
            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;
        }
        
        #endregion
    }
}
