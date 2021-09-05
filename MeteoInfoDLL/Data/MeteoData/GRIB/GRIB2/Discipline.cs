using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB 2 Discipline
    /// </summary>
    public class Discipline
    {
        #region Variables
        private int _Number;
        private string _Name;
        private Hashtable _CategoryTable;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Discipline()
        {
            _Number = -1;
            _Name = "Undefined";
            _CategoryTable = new Hashtable();
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
        public Hashtable CategoryTable
        {
            get { return _CategoryTable; }            
        }

        #endregion

        #region Methods
        /// <summary>
        /// Add a category
        /// </summary>
        /// <param name="aCat">category</param>
        public void SetCategory(Category aCat)
        {
            _CategoryTable[aCat.Number] = aCat;
        }

        /// <summary>
        /// Get a category
        /// </summary>
        /// <param name="catNum">category number</param>
        /// <returns>category</returns>
        public Category GetCategory(int catNum)
        {
            return (Category)_CategoryTable[catNum];
        }

        #endregion
    }
}
