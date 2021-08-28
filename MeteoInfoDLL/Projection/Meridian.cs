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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 12:39:34 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;
using System.Collections.Generic;
//using MapWindow.Drawing;
using System.Linq;
namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Meridian
    /// </summary>
    public class Meridian //: Descriptor, IEsriString
    {
        #region Private Variables

        private string _name;
        private double _longitude;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Meridian
        /// </summary>
        public Meridian()
        {

        }

        /// <summary>
        /// Generates a custom meridian given a name and a longitude
        /// </summary>
        /// <param name="longitude">The longitude to use</param>
        /// <param name="name">The string name for this meridian</param>
        public Meridian(double longitude, string name)
        {
            _longitude = longitude;
            _name = name;
        }

        /// <summary>
        /// Creates a new meridian from one of the known, proj4 meridian locations.
        /// Presumably the longitudes here correspond to various standard meridians
        /// rather than some arbitrary longitudes of capital cities.
        /// </summary>
        /// <param name="standardMeridian">One of the enumerations listed</param>
        public Meridian(Proj4Meridians standardMeridian)
        {
            AssignMeridian(standardMeridian);
        }

        /// <summary>
        /// Creates a new meridian from one of the known, proj4 meridian locations.
        /// </summary>
        /// <param name="standardMeridianName">The string name of the meridian to use</param>
        public Meridian(string standardMeridianName)
        {
            Proj4Meridians[] m = Enum.GetValues(typeof (Proj4Meridians)) as Proj4Meridians[];
            foreach (Proj4Meridians meridian in m)
            {
                if (m.ToString().ToLower() == standardMeridianName.ToLower())
                {
                    AssignMeridian(meridian);
                    break;
                }

            }
        }

        private void AssignMeridian(Proj4Meridians standardMeridian)
        {
            _name = standardMeridian.ToString();
            switch (standardMeridian)
            {
                case Proj4Meridians.Greenwich:
                    _longitude = 0;
                    break;
                case Proj4Meridians.Lisbon:
                    _longitude = -9.131906111;
                    break;
                case Proj4Meridians.Paris:
                    _longitude = 2.337229167;
                    break;
                case Proj4Meridians.Bogota:
                    _longitude = -74.08091667;
                    break;
                case Proj4Meridians.Madrid:
                    _longitude = -3.687938889;
                    break;
                case Proj4Meridians.Rome:
                    _longitude = 12.45233333;
                    break;
                case Proj4Meridians.Bern:
                    _longitude = 7.439583333;
                    break;
                case Proj4Meridians.Jakarta:
                    _longitude = 106.8077194;
                    break;
                case Proj4Meridians.Ferro:
                    _longitude = -17.66666667;
                    break;
                case Proj4Meridians.Brussels:
                    _longitude = 4.367975;
                    break;
                case Proj4Meridians.Stockholm:
                    _longitude = 18.05827778;
                    break;
                case Proj4Meridians.Athens:
                    _longitude = 23.7163375;
                    break;
                case Proj4Meridians.Oslo:
                    _longitude = 10.72291667;
                    break;

            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attempts to parse the parameters in order to read the meridian
        /// </summary>
        /// <param name="parameters"></param>
        public void ReadProj4Parameters(Dictionary<string, string> parameters)
        {
            if(parameters.ContainsKey("pm"))
            {
                string pm = parameters["pm"];
                Proj4Meridians[] meridians = Enum.GetValues(typeof(Proj4Meridians)) as Proj4Meridians[];
                if (meridians != null)
                {
                    foreach (Proj4Meridians meridian in meridians)
                    {
                        if (meridian.ToString().ToLower() == pm.ToLower())
                        {
                            AssignMeridian(meridian);
                        }
                    }
                }
            }
        }

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the string name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the longitude where the prime meridian for this geographic coordinate occurs.
        /// </summary>
        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        #endregion




        #region IEsriString Members

        /// <summary>
        /// Writes the longitude and prime meridian content to the esri string
        /// </summary>
        /// <returns>A string that is formatted for an esri prj file</returns>
        public string ToEsriString()
        {
            return @"PRIMEM[""" + _name + @"""," + _longitude + "]";
        }

        /// <summary>
        /// Reads content from an esri string, learning information about the prime meridian
        /// </summary>
        /// <param name="esriString"></param>
        public void ReadEsriString(string esriString)
        {
            if (esriString.Contains("PRIMEM") == false) return;
            int iStart = esriString.IndexOf("PRIMEM") + 7;
            int iEnd = esriString.IndexOf("]", iStart);
            if (iEnd < iStart) return;
            string extracted = esriString.Substring(iStart, iEnd - iStart);
            string[] terms = extracted.Split(',');
            _name = terms[0];
            _name = _name.Substring(1, _name.Length - 2);
            _longitude = double.Parse(terms[1]);
        }

        #endregion
    }
}
