using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Point shape
    /// </summary>
    public class PointZShape:PointShape
    {
        private PointZ _point;

        /// <summary>
        /// Z
        /// </summary>
        public double Z;
        /// <summary>
        /// M
        /// </summary>
        public double M;  
      
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PointZShape()
        {
            ShapeType = ShapeTypes.PointZ;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set point
        /// </summary>
        public new PointZ Point
        {
            get { return _point; }
            set 
            {
                _point = value;
                Extent aExtent;
                aExtent.minX = _point.X;
                aExtent.maxX = _point.X;
                aExtent.minY = _point.Y;
                aExtent.maxY = _point.Y;
                Extent = aExtent;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clone PointShape
        /// </summary>
        /// <returns>PointShape</returns>
        public override object Clone()
        {
            PointZShape aPS = new PointZShape();
            //aPS = (PointZShape)base.Clone();
            aPS.Point = Point;
            aPS.Z = Z;
            aPS.M = M;
            aPS.Value = Value;
            aPS.LegendIndex = LegendIndex;

            return aPS;
        }

        #endregion
    }
}
