using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB message index
    /// </summary>
    public class GRIB1MessageIndex
    {
        #region Variables
        /// <summary>
        /// Message start position bytes
        /// </summary>
        public long Position;
        /// <summary>
        /// DateTime
        /// </summary>
        public DateTime DateTime;
        /// <summary>
        /// Level
        /// </summary>
        public double Level;
        /// <summary>
        /// Parameter
        /// </summary>
        public Variable Parameter;

        #endregion

        #region Concstructor
        /// <summary>
        /// Constructor
        /// </summary>
        public GRIB1MessageIndex()
        {
            DateTime = new DateTime();
            Parameter = new Variable();
        }

        #endregion

        #region Medthods        
        /// <summary>
        /// Judge if two message index are equal
        /// </summary>
        /// <param name="aMessageIdx">a message index</param>
        /// <returns>if equal</returns>
        public bool Equals(GRIB1MessageIndex aMessageIdx)
        {
            if (!Parameter.Equals(aMessageIdx.Parameter)) return false;
            if (DateTime != aMessageIdx.DateTime) return false;
            if (Level != aMessageIdx.Level) return false;            

            return true;
        }

        #endregion
    }
}
