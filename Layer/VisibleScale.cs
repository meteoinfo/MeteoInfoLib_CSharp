using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// Visible scale setting
    /// </summary>
    public class VisibleScale
    {
        #region Variables
        private bool _enableMinVisScale = false;
        private bool _enableMaxVisScale = false;
        private double _minVisScale;
        private double _maxVisScale;
        #endregion

        #region Constructor

        #endregion

        #region Properties
        /// <summary>
        /// Get or set is enable minimum visible scale
        /// </summary>
        public bool EnableMinVisScale
        {
            get { return _enableMinVisScale; }
            set { _enableMinVisScale = value; }
        }

        /// <summary>
        /// Get or set if enable maximum visible scale
        /// </summary>
        public bool EnableMaxVisScale
        {
            get { return _enableMaxVisScale; }
            set { _enableMaxVisScale = value; }
        }

        /// <summary>
        /// Get or set minimum visible scale
        /// </summary>
        public double MinVisScale
        {
            get { return _minVisScale; }
            set { _minVisScale = value; }
        }

        /// <summary>
        /// Get or set maximum visible scale
        /// </summary>
        public double MaxVisScale
        {
            get { return _maxVisScale; }
            set { _maxVisScale = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get is visible scale enabled
        /// </summary>
        /// <returns>Is visible scale enabled</returns>
        public bool IsVisibleScaleEnabled()
        {
            if (this._enableMaxVisScale || this._enableMinVisScale)
                return true;
            else
                return false;
        }
        #endregion
    }
}
