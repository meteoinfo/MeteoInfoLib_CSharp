using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Rectangle shape
    /// </summary>
    public class RectangleShape:PolygonShape
    {
        #region Variables                            
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public RectangleShape()
        {
            ShapeType = ShapeTypes.Rectangle;                  
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
            RectangleShape aPGS = new RectangleShape();
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
            return Global.MIMath.PointInExtent(point, this.Extent);
        }

        #endregion
    }
}
