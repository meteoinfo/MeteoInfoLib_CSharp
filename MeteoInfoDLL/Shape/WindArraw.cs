using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Wind arraw
    /// </summary>
    public class WindArraw:PointShape
    {        
        /// <summary>
        /// size
        /// </summary>
        public Single size = 6;
        /// <summary>
        /// length
        /// </summary>
        public Single length = 20;
        /// <summary>
        /// angle
        /// </summary>
        public double angle = 270;

        #region Concstruction
        /// <summary>
        /// Constructor
        /// </summary>
        public WindArraw()
        {
            ShapeType = ShapeTypes.WindArraw;
            //ShapeType = ShapeTypes.Point;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Clone WindArraw
        /// </summary>
        /// <returns>WindArraw</returns>
        public override object Clone()
        {
            WindArraw aWA = new WindArraw();
            aWA.size = size;
            aWA.length = length;
            aWA.angle = angle;
            aWA.Point = Point;
            aWA.Value = Value;

            return aWA;
        }

        #endregion
    }
}
