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
    public class PointShape:Shape,ICloneable
    {
        #region Variables
        private PointD _point = new PointD();      
        private double _value;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PointShape()
        {
            ShapeType = ShapeTypes.Point;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set point coordinate
        /// </summary>
        public PointD Point
        {
            get { return _point; }
            set 
            { 
                _point = value;
                updateExtent();
            }
        }

        /// <summary>
        /// Get or set value
        /// </summary>
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Override get points
        /// </summary>
        /// <returns>points</returns>
        public override List<PointD> GetPoints()
        {
            List<PointD> pList = new List<PointD>();
            pList.Add(_point);

            return pList;
        }

        /// <summary>
        /// Override set points
        /// </summary>
        /// <param name="points">points</param>
        public override void SetPoints(List<PointD> points)
        {
            Point = points[0];
        }

        /// <summary>
        /// Update point extent
        /// </summary>
        public void updateExtent()
        {
            Extent aExtent;
            aExtent.minX = _point.X;
            aExtent.maxX = _point.X;
            aExtent.minY = _point.Y;
            aExtent.maxY = _point.Y;
            Extent = aExtent;
        }

        /// <summary>
        /// Clone PointShape
        /// </summary>
        /// <returns>PointShape</returns>
        public override object Clone()
        {
            PointShape aPS = new PointShape();
            aPS.Point = _point;
            aPS.Value = _value;
            aPS.Visible = Visible;
            aPS.Selected = Selected;
            aPS.LegendIndex = LegendIndex;

            return aPS;
        }

        #endregion
    }
}
