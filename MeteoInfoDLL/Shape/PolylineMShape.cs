using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// PolylineM shape class
    /// </summary>
    public class PolylineMShape:PolylineShape
    {
        #region Variables   
        

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PolylineMShape():base()
        {
            ShapeType = ShapeTypes.PolylineM;            
        }
        #endregion

        #region Properties

        

        #endregion

        #region Methods
        

        /// <summary>
        /// Clone polylineM shape
        /// </summary>
        /// <returns>PolylineMShape</returns>
        public override object Clone()
        {
            PolylineMShape aPLS = new PolylineMShape();
            aPLS.value = value;
            aPLS.Extent = Extent;
            aPLS.PartNum = PartNum;
            aPLS.parts = (int[])parts.Clone();
            aPLS.Points = new List<PointD>(Points);
            aPLS.Visible = Visible;
            aPLS.Selected = Selected;
            aPLS.LegendIndex = LegendIndex;
            //aPLS.ZRange = (double[])ZRange.Clone();
            //aPLS.MRange = (double[])MRange.Clone();
            //aPLS.ZArray = (double[])ZArray.Clone();
            //aPLS.MArray = (double[])MArray.Clone();

            return aPLS;
        }

        /// <summary>
        /// Value clone
        /// </summary>
        /// <returns>new polyline Z shape</returns>
        public new PolylineZShape ValueClone()
        {
            PolylineZShape aPLS = new PolylineZShape();
            aPLS.value = value;
            aPLS.Visible = Visible;
            aPLS.Selected = Selected;
            aPLS.LegendIndex = LegendIndex;

            return aPLS;
        }

        #endregion
    }
}
