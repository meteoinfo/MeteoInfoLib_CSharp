using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Point M shape class
    /// </summary>
    public class PointMShape:PointShape
    {
        #region Variables
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PointMShape():base()
        {
            ShapeType = ShapeTypes.PointM;
        }

        #endregion

        #region Properties
        

        #endregion

        #region Methods
        
        /// <summary>
        /// Clone PointMShape
        /// </summary>
        /// <returns>PointMShape</returns>
        public override object Clone()
        {
            PointMShape aPS = new PointMShape();
            aPS.Point = Point;
            aPS.Value = Value;
            aPS.Visible = Visible;
            aPS.Selected = Selected;
            aPS.LegendIndex = LegendIndex;

            return aPS;
        }

        #endregion
    }
}
