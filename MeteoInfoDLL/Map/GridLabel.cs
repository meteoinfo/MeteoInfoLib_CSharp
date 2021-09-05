using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Map
{
    /// <summary>
    /// Grid line label
    /// </summary>
    public class GridLabel
    {
        #region Variables
        private Direction _labDirection;
        private string _labString;
        private PointD _labPoint;
        private bool _isLon;
        private bool _isBorder;
        private float _value;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public GridLabel()
        {
            _labDirection = Direction.East;
            _isLon = true;
            _isBorder = true;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set label direction
        /// </summary>
        public Direction LabDirection
        {
            get { return _labDirection; }
            set { _labDirection = value; }
        }

        /// <summary>
        /// Get or set label string
        /// </summary>
        public string LabString
        {
            get { return _labString; }
            set { _labString = value; }
        }

        /// <summary>
        /// Get or set label point
        /// </summary>
        public PointD LabPoint
        {
            get { return _labPoint; }
            set { _labPoint = value; }
        }

        /// <summary>
        /// Get or set if is longitude
        /// </summary>
        public bool IsLon
        {
            get { return _isLon; }
            set { _isLon = value; }
        }

        /// <summary>
        /// Get or set if is border
        /// </summary>
        public bool IsBorder
        {
            get { return _isBorder; }
            set { _isBorder = value; }
        }

        /// <summary>
        /// Get or set value
        /// </summary>
        public float Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #endregion
    }
}
