using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// ARL Data label
    /// </summary>
    public class DataLabel
    {
        #region Variables
        /// <summary>
        /// Year
        /// </summary>
        public int Year;
        /// <summary>
        /// Month
        /// </summary>
        public Int16 Month;
        /// <summary>
        /// Day
        /// </summary>
        public Int16 Day;
        /// <summary>
        /// Hour
        /// </summary>
        public Int16 Hour;
        /// <summary>
        /// Forecast
        /// </summary>
        public Int16 Forecast;
        /// <summary>
        /// Level
        /// </summary>
        public Int16 Level;
        /// <summary>
        /// Grid
        /// </summary>
        public Int16 Grid;
        /// <summary>
        /// Variable
        /// </summary>
        public string Variable;
        /// <summary>
        /// Exponent
        /// </summary>
        public int Exponent;
        /// <summary>
        /// Precision
        /// </summary>
        public double Precision;
        /// <summary>
        /// Value
        /// </summary>
        public double Value;

        private DateTime _time = new DateTime();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public DataLabel()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="time">Time</param>
        public DataLabel(DateTime time)
        {
            _time = time;
            Year = int.Parse(time.ToString("yy"));
            Month = Int16.Parse(time.ToString("MM"));
            Day = Int16.Parse(time.ToString("dd"));
            Hour = Int16.Parse(time.ToString("HH"));
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set time
        /// </summary>
        public DateTime Time
        {
            get { return _time; }
            set 
            {
                _time = value;
                Year = int.Parse(_time.ToString("yy"));
                Month = Int16.Parse(_time.ToString("MM"));
                Day = Int16.Parse(_time.ToString("dd"));
                Hour = Int16.Parse(_time.ToString("HH"));
            }
        }

        #endregion
    }
}
