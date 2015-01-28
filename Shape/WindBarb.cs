using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Wind barb
    /// </summary>
    public class WindBarb:PointShape
    {        
        /// <summary>
        /// size
        /// </summary>
        public Single size;
        /// <summary>
        /// angle
        /// </summary>
        public double angle;
        /// <summary>
        /// wind speed
        /// </summary>
        public Single windSpeed;
        /// <summary>
        /// wind speed line
        /// </summary>
        public WindSpeedLine windSpeesLine;

        #region Concstruction
        /// <summary>
        /// Constructor
        /// </summary>
        public WindBarb()
        {
            ShapeType = ShapeTypes.WindBarb;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Clone WindBarb
        /// </summary>
        /// <returns>WindBarb</returns>
        public override object Clone()
        {
            WindBarb aWB = new WindBarb();
            aWB.size = size;
            aWB.windSpeed = windSpeed;
            aWB.angle = angle;
            aWB.windSpeesLine = windSpeesLine;
            aWB.Point = Point;
            aWB.Value = Value;

            return aWB;
        }

        #endregion
    }
}
