using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Global
{
    /// <summary>
    /// Data convert static class
    /// </summary>
    public static class DataConvert
    {
        /// <summary>
        /// DateTime to double
        /// </summary>
        /// <param name="time">The DateTime</param>
        /// <returns>Double value</returns>
        public static double ToDouble(DateTime time)
        {
            return BitConverter.ToDouble(BitConverter.GetBytes(time.Ticks), 0);
        }

        /// <summary>
        /// Double to DateTime
        /// </summary>
        /// <param name="value">The double value</param>
        /// <returns>DateTime value</returns>
        public static DateTime ToDateTime(double value)
        {
            return new DateTime(BitConverter.ToInt64(BitConverter.GetBytes(value), 0));
        }

        /// <summary>
        /// Double to OADate
        /// </summary>
        /// <param name="value">The double value</param>
        /// <returns>OADate</returns>
        public static double ToOADate(double value)
        {
            return ToDateTime(value).ToOADate();
        }
    }
}
