using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Circle shape
    /// </summary>
    public class CircleShape:PolygonShape
    {
        #region Variables   
        private PointD _center = new PointD();
        private double _radius;         
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public CircleShape()
        {
            ShapeType = ShapeTypes.Circle;                  
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cPoint">Center point</param>
        /// <param name="radius">Radius</param>
        public CircleShape(PointD cPoint, double radius)
        {
            ShapeType = ShapeTypes.Circle;
            _center = cPoint;
            _radius = radius;

            List<PointD> points = new List<PointD>();
            points.Add(new PointD(cPoint.X - radius, cPoint.Y));
            points.Add(new PointD(cPoint.X, cPoint.Y - radius));
            points.Add(new PointD(cPoint.X + radius, cPoint.Y));
            points.Add(new PointD(cPoint.X, cPoint.Y + radius));

            this.Points = points;
        }

        #endregion

        #region Properties    

        #endregion

        #region Methods   
        /// <summary>
        /// Override SetPoints method
        /// </summary>
        /// <param name="points">The points</param>
        public override void SetPoints(List<PointD> points)
        {
            base.SetPoints(points);
            this._center = new PointD(points[1].X, points[0].Y);
            this._radius = points[1].X - points[0].X;
        }

        /// <summary>
        /// Clone PlygonShape
        /// </summary>
        /// <returns>PolygonShape</returns>
        public override object Clone()
        {
            CircleShape aPGS = new CircleShape();
            aPGS.Extent = Extent;            
            aPGS.Points = new List<PointD>(Points);
            aPGS.Visible = Visible;
            aPGS.Selected = Selected;

            return aPGS;
        }

        /// <summary>
        /// Determine if a point is inside this circle shape
        /// </summary>
        /// <param name="point">The point</param>
        /// <returns>Is inside or not</returns>
        public new bool IsPointInside(PointD point)
        {
            double dist = Math.Sqrt((point.X - _center.X) * (point.X - _center.X) + (point.Y - _center.Y) *
                (point.Y - _center.Y));
            return dist <= _radius;
        }

        #endregion
    }
}
