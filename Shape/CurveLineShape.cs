using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Curve line shape
    /// </summary>
    public class CurveLineShape : PolylineShape
    {
        #region Variables        
        

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public CurveLineShape()
        {
            ShapeType = ShapeTypes.CurveLine;            
        }
        #endregion

        #region Properties
        

        #endregion

        #region Methods          

        /// <summary>
        /// Clone polylineshape
        /// </summary>
        /// <returns>PolylineShape</returns>
        public override object Clone()
        {
            CurveLineShape aPLS = new CurveLineShape();
            aPLS.value = value;
            aPLS.Extent = Extent;
            aPLS.PartNum = PartNum;
            aPLS.parts = (int[])parts.Clone();
            aPLS.Points = new List<PointD>(Points);
            aPLS.Visible = Visible;
            aPLS.Selected = Selected;

            return aPLS;
        }

        /// <summary>
        /// Value clone
        /// </summary>
        /// <returns>new polyline shape</returns>
        public new PolylineShape ValueClone()
        {
            CurveLineShape aPLS = new CurveLineShape();
            aPLS.value = value;            
            aPLS.Visible = Visible;
            aPLS.Selected = Selected;

            return aPLS;
        }

        #endregion
    }
}
