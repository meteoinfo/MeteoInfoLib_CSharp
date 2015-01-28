using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data
{
    /// <summary>
    /// Station model data
    /// </summary>
    public class StationModel
    {
        #region Variables
        private string _stId;
        private string _stName;
        private double _lon;
        private double _lat;
        private double _cloudCover;
        private double _windDirection;
        private double _windSpeed;
        private double _pressure;
        private double _pressureChange;
        private double _weather;
        private double _previousWeather;
        private double _visibility;
        private double _temperature;
        private double _dewPoint;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public StationModel()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="initValue">initialize value</param>
        public StationModel(double initValue)
        {
            _cloudCover = initValue;
            _windDirection = initValue;
            _windSpeed = initValue;
            _pressure = initValue;
            _pressureChange = initValue;
            _weather = initValue;
            _previousWeather = initValue;
            _visibility = initValue;
            _temperature = initValue;
            _dewPoint = initValue;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set station identifer
        /// </summary>
        public string StationIdentifer
        {
            get { return _stId; }
            set { _stId = value; }
        }

        /// <summary>
        /// Get or set station name
        /// </summary>
        public string StationName
        {
            get { return _stName; }
            set { _stName = value; }
        }

        /// <summary>
        /// Get or set longitude
        /// </summary>
        public double Longitude
        {
            get { return _lon; }
            set { _lon = value; }
        }

        /// <summary>
        /// Get or set latitude
        /// </summary>
        public double Latitude
        {
            get { return _lat; }
            set { _lat = value; }
        }

        /// <summary>
        /// Get or set cloud cover
        /// </summary>
        public double CloudCover
        {
            get { return _cloudCover; }
            set { _cloudCover = value; }
        }

        /// <summary>
        /// Get or set wind direction
        /// </summary>
        public double WindDirection
        {
            get { return _windDirection; }
            set { _windDirection = value; }
        }

        /// <summary>
        /// Get or set wind speed
        /// </summary>
        public double WindSpeed
        {
            get { return _windSpeed; }
            set { _windSpeed = value; }
        }

        /// <summary>
        /// Get or set pressure
        /// </summary>
        public double Pressure
        {
            get { return _pressure; }
            set { _pressure = value; }
        }

        /// <summary>
        /// Get or set pressure change
        /// </summary>
        public double PressureChange
        {
            get { return _pressureChange; }
            set { _pressureChange = value; }
        }

        /// <summary>
        /// Get or set weather
        /// </summary>
        public double Weather
        {
            get { return _weather; }
            set { _weather = value; }
        }

        /// <summary>
        /// Get or set previous weather
        /// </summary>
        public double PreviousWeather
        {
            get { return _previousWeather; }
            set { _previousWeather = value; }
        }

        /// <summary>
        /// Get or set visibility
        /// </summary>
        public double Visibility
        {
            get { return _visibility; }
            set { _visibility = value; }
        }

        /// <summary>
        /// Get or set temperature
        /// </summary>
        public double Temperature
        {
            get { return _temperature; }
            set { _temperature = value; }
        }

        /// <summary>
        /// Get or set dew point
        /// </summary>
        public double DewPoint
        {
            get { return _dewPoint; }
            set { _dewPoint = value; }
        }

        #endregion
    }
}
