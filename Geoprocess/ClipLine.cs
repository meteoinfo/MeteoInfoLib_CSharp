using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Geoprocess
{
    /// <summary>
    /// Clip line
    /// </summary>
    public class ClipLine
    {
        #region Variables
        private double _value;
        private bool _isLon;
        private bool _isLeftOrTop;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ClipLine()
        {
            _isLon = true;
            _isLeftOrTop = true;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set line value
        /// </summary>
        public double Value
        {
            get { return _value; }
            set { _value = value; }
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
        /// Get or set if is left (longitude) or top (latitude)
        /// </summary>
        public bool IsLeftOrTop
        {
            get { return _isLeftOrTop; }
            set { _isLeftOrTop = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Determine if a point is inside
        /// </summary>
        /// <param name="aPoint">a point</param>
        /// <returns>if is inside</returns>
        public bool IsInside(PointD aPoint)
        {
            bool isIn = false;
            if (_isLon)
            {
                if (_isLeftOrTop)
                    isIn = (aPoint.X <= _value);
                else
                    isIn = (aPoint.X >= _value);
            }
            else
            {
                if (_isLeftOrTop)
                    isIn = (aPoint.Y >= _value);
                else
                    isIn = (aPoint.Y <= _value);
            }

            return isIn;
        }

        /// <summary>
        /// Determine if an extent is cross
        /// </summary>
        /// <param name="aExtent">an extent</param>
        /// <returns>Is extent cross</returns>
        public bool IsExtentCross(Extent aExtent)
        {
            if (_isLeftOrTop)
            {
                PointD aPoint = new PointD(aExtent.minX, aExtent.maxY);
                return IsInside(aPoint);
            }
            else
            {
                PointD aPoint = new PointD(aExtent.maxX, aExtent.minY);
                return IsInside(aPoint);
            }
        }

        /// <summary>
        /// Determine if an extent is inside
        /// </summary>
        /// <param name="aExtent">an extent</param>
        /// <returns>Is extent inside</returns>
        public bool IsExtentInside(Extent aExtent)
        {
            if (_isLeftOrTop)
            {
                PointD aPoint = new PointD(aExtent.maxX, aExtent.minY);
                return IsInside(aPoint);
            }
            else
            {
                PointD aPoint = new PointD(aExtent.minX, aExtent.maxY);
                return IsInside(aPoint);
            }
        }

        #endregion
    }
}
