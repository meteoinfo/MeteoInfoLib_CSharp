//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 2:54:46 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

//using MapWindow.Drawing;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Unit
    /// </summary>
    public class LinearUnit //: Descriptor, IEsriString
    {
        #region Private Variables

        private string _name;
        private double _meters;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Unit
        /// </summary>
        public LinearUnit()
        {
            _meters = 1;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of this 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the constant to multiply with maps distances to get the distances in meters
        /// </summary>
        public double Meters
        {
            get { return _meters; }
            set { _meters = value; }

        }

        #endregion




        #region IEsriString Members

        /// <summary>
        /// Generates the part of the ESRI well known text for this linear unit
        /// </summary>
        /// <returns>A string that contains the name and conversion factor to meters </returns>
        public string ToEsriString()
        {
            return @"UNIT[""" + _name + @"""," + _meters + "]";
        }

        /// <summary>
        /// Parses the UNIT member of ESRI well known text into a linear unit
        /// </summary>
        /// <param name="esriString"></param>
        public void ReadEsriString(string esriString)
        {
            if (esriString.Contains("UNIT") == false) return;
            int iStart = esriString.IndexOf("UNIT") + 5;
            int iEnd = esriString.IndexOf("]", iStart);
            if (iEnd < iStart) return;
            string extracted = esriString.Substring(iStart, iEnd - iStart);
            string[] terms = extracted.Split(',');
            _name = terms[0];
            _name = _name.Substring(1, _name.Length - 2);
            _meters = double.Parse(terms[1]);
        }

        #endregion
    }
}
