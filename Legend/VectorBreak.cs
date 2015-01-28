using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Vector break for wind arraw
    /// </summary>
    public class VectorBreak:ColorBreak
    {
        #region Variables
        private float _zoom = 1.0f;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public VectorBreak():base()
        {
            BreakType = BreakTypes.VectorBreak;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set drawing zoom value
        /// </summary>
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        #endregion
    }
}
