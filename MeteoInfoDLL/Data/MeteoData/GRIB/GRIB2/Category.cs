using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB 2 category
    /// </summary>
    public class Category
    {
         #region Variables
        private int _Number;
        private string _Name;
        private Hashtable _ParameterTable;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Category()
        {
            _Number = -1;
            _Name = "Undefined";
            _ParameterTable = new Hashtable();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set discipline number
        /// </summary>
        public int Number
        {
            get { return _Number; }
            set { _Number = value; }
        }

        /// <summary>
        /// Get or set discipline name
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// Get category table
        /// </summary>
        public Hashtable ParameterTable
        {
            get { return _ParameterTable; }            
        }

        #endregion

        #region Methods
        /// <summary>
        /// Add a parameter
        /// </summary>
        /// <param name="aPar">parameter</param>
        public void SetParameter(Variable aPar)
        {
            _ParameterTable[aPar.Number] = aPar;
        }

        /// <summary>
        /// Get a parameter
        /// </summary>
        /// <param name="parNum">parameter number</param>
        /// <returns>parameter</returns>
        public Variable GetParameter(int parNum)
        {
            return (Variable)_ParameterTable[parNum];
        }

        #endregion
    }
}
