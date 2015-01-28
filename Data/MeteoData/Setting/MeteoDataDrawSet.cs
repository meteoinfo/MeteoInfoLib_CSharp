using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Meteo data drawing set
    /// </summary>
    public class MeteoDataDrawSet
    {
        #region Variables

        private string m_weatherType;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MeteoDataDrawSet()
        {
            m_weatherType = "All Weather";
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get or set weather type
        /// </summary>
        public string WeatherType
        {
            get { return m_weatherType; }
            set { m_weatherType = value; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get weather type
        /// </summary>
        /// <param name="weatherType">Weather type string</param>
        /// <returns>ArrayList of weather codes</returns>
        public ArrayList GetWeatherTypes(string weatherType)
        {
            ArrayList weatherList = new ArrayList();
            int i;
            switch (weatherType)
            {
                case "All Weather":
                    for (i = 4; i < 100; i++)
                    {
                        weatherList.Add(i);
                    }
                    break;
                case "SDS":
                    weatherList = ArrayList.Adapter(new int[] { 6, 7, 8, 9, 30, 31, 32, 33, 34, 35 });
                    break;
                case "SDS, Haze":
                    weatherList = ArrayList.Adapter(new int[] { 5, 6, 7, 8, 9, 30, 31, 32, 33, 34, 35 });
                    break;
                case "Smoke, Haze,Mist":
                    weatherList = ArrayList.Adapter(new int[] { 4, 5, 10 });
                    break;
                case "Smoke":
                    weatherList.Add(4);
                    break;
                case "Haze":
                    weatherList.Add(5);
                    break;
                case "Mist":
                    weatherList.Add(10);
                    break;
                case "Fog":
                    for (i = 40; i < 50; i++)
                    {
                        weatherList.Add(i);
                    }
                    break;
            }

            return weatherList;
        }
        #endregion
    }

}
