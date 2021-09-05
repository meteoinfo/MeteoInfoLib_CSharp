using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Weathe symbol
    /// </summary>
    public class WeatherSymbol:PointShape
    {        
        /// <summary>
        /// Size
        /// </summary>
        public Single size;
        /// <summary>
        /// Weather
        /// </summary>
        public int weather;

        #region Concstruction
        /// <summary>
        /// Constructor
        /// </summary>
        public WeatherSymbol()
        {
            ShapeType = ShapeTypes.WeatherSymbol;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Clone WeatherSymbol
        /// </summary>
        /// <returns>WeatherSymbol</returns>
        public override object Clone()
        {
            WeatherSymbol aWS = new WeatherSymbol();
            aWS.size = size;
            aWS.weather = weather;
            aWS.Point = Point;
            aWS.Value = Value;

            return aWS;
        }

        #endregion
    }
}
