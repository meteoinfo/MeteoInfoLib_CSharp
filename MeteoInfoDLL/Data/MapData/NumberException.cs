using System;

namespace MeteoInfoC.Data.MapData
{


    /// <summary>
    /// NumberException
    /// </summary>
    public class NumberException : Exception
    {
        /// <summary>
        /// An exception that is specifically fo the NumberConverter class
        /// </summary>
        /// <param name="message">The message for the exception</param>
        public NumberException(string message)
            : base(message)
        {

        }
    }
}
