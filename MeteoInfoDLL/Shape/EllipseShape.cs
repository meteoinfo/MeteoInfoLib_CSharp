using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Ellipse shape
    /// </summary>
    public class EllipseShape:PolygonShape
    {
        #region Variables                            
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EllipseShape()
        {
            ShapeType = ShapeTypes.Ellipse;                  
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cPoint">Center point</param>
        /// <param name="a">Radius of x direction</param>
        /// <param name="b">Radius of y direction</param>
        public EllipseShape(PointD cPoint, double a, double b)
        {
            ShapeType = ShapeTypes.Ellipse;

            List<PointD> points = new List<PointD>();
            points.Add(new PointD(cPoint.X - a, cPoint.Y - b));
            points.Add(new PointD(cPoint.X - a, cPoint.Y + b));
            points.Add(new PointD(cPoint.X + a, cPoint.Y + b));
            points.Add(new PointD(cPoint.X + a, cPoint.Y - b));

            this.Points = points;
        }

        #endregion

        #region Properties        

        #endregion

        #region Methods        
        /// <summary>
        /// Clone PlygonShape
        /// </summary>
        /// <returns>PolygonShape</returns>
        public override object Clone()
        {
            EllipseShape aPGS = new EllipseShape();
            aPGS.Extent = Extent;            
            aPGS.Points = new List<PointD>(Points);
            aPGS.Visible = Visible;
            aPGS.Selected = Selected;

            return aPGS;
        }

        #endregion
    }
}
