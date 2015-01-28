using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Curve polygon shape
    /// </summary>
    public class CurvePolygonShape:PolygonShape
    {
        #region Variables
        

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public CurvePolygonShape()
        {
            ShapeType = ShapeTypes.CurvePolygon;            
        }
        #endregion

        #region Properties
        

        #endregion

        #region Methods 

        /// <summary>
        /// Clone CurvePolygonShape
        /// </summary>
        /// <returns>CurvePolygonShape</returns>
        public override object Clone()
        {
            CurvePolygonShape aPGS = new CurvePolygonShape();
            aPGS.Extent = Extent;
            aPGS.highValue = highValue;
            aPGS.lowValue = lowValue;
            aPGS.PartNum = PartNum;
            aPGS.parts = (int[])parts.Clone();
            aPGS.Points = new List<PointD>(Points);
            aPGS.Visible = Visible;
            aPGS.Selected = Selected;

            return aPGS;
        }

        /// <summary>
        /// Clone CurvePolygonShape with values
        /// </summary>
        /// <returns>new curve polygon shape</returns>
        public new CurvePolygonShape ValueClone()
        {
            CurvePolygonShape aPGS = new CurvePolygonShape();
            aPGS.highValue = highValue;
            aPGS.lowValue = lowValue;
            aPGS.Visible = Visible;
            aPGS.Selected = Selected;

            return aPGS;
        }

        #endregion
    }
}
