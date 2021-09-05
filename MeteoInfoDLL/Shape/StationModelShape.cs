using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Station model symbol
    /// </summary>
    public class StationModelShape:PointShape
    {
        #region Variables
        /// <summary>
        /// Wind barb
        /// </summary>
        public WindBarb windBarb;
        /// <summary>
        /// Weather symbol
        /// </summary>
        public WeatherSymbol weatherSymbol;
        /// <summary>
        /// Cloud coverage
        /// </summary>
        public CloudCoverage cloudCoverage;
        /// <summary>
        /// Temperature
        /// </summary>
        public int temperature;
        /// <summary>
        /// Dew point
        /// </summary>
        public int dewPoint;
        /// <summary>
        /// Pressure
        /// </summary>
        public int pressure;        
        /// <summary>
        /// Size
        /// </summary>
        public Single size;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public StationModelShape()
        {
            ShapeType = ShapeTypes.StationModel;
            weatherSymbol = new WeatherSymbol();
            windBarb = new WindBarb();
            cloudCoverage = new CloudCoverage();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Clone StationModel
        /// </summary>
        /// <returns>StationModel</returns>
        public override object Clone()
        {
            StationModelShape aSM = new StationModelShape();
            //aSM = (StationModel)base.Clone();
            aSM.size = size;
            aSM.pressure = pressure;
            aSM.dewPoint = dewPoint;
            aSM.temperature = temperature;
            aSM.cloudCoverage = cloudCoverage;
            //aSM.weatherSymbol = new WeatherSymbol();
            aSM.weatherSymbol =(WeatherSymbol)weatherSymbol.Clone();
            //aSM.windBarb = new WindBarb();
            aSM.windBarb = (WindBarb)windBarb.Clone();
            aSM.Point = Point;
            aSM.Value = Value;

            return aSM;
        }

        #endregion
    }
}
