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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 10:00:36 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System.Collections.Generic;
//using MapWindow.Drawing;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// GeographicInfo
    /// </summary>
    public class GeographicInfo //: Descriptor, IEsriString
    {
        #region Private Variables

        private string _name;
        private Datum _datum;
        private Meridian _meridian;
        private AngularUnit _unit;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeographicInfo
        /// </summary>
        public GeographicInfo()
        {
            _datum = new Datum();
            _meridian = new Meridian();
            _unit = new AngularUnit();
        }

        #endregion

        #region Methods

        

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the datum
        /// eg: D_WGS_1984
        /// </summary>
        public Datum Datum
        {
            get { return _datum; }
            set { _datum = value; }
        }

        /// <summary>
        /// Gets or sets the prime meridian longitude of the 0 mark, relative to Greenwitch 
        /// </summary>
        public Meridian Meridian
        {
            get { return _meridian; }
            set { _meridian = value; }
        }

        /// <summary>
        /// Gets or sets the geographic coordinate system name
        /// eg: GCS_WGS_1984
        /// </summary>
        public string Name
        {
            get { return _name;  }
            set { _name = value; }
        }


        /// <summary>
        /// Gets or sets the units
        /// </summary>
        public AngularUnit Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }
      

        #endregion




        #region IEsriString Members

        /// <summary>
        /// Generates an esri string from the information in this geographic info class, including the name, datum, meridian, and unit.
        /// </summary>
        /// <returns></returns>
        public string ToEsriString()
        {
            return @"GEOGCS[""" + _name + @"""," + Datum.ToEsriString() + "," + Meridian.ToEsriString() + "," + Unit.ToEsriString() + "]";
        }

        /// <summary>
        /// Reads an esri string in order to parse the datum, meridian and unit as well as the name.
        /// </summary>
        /// <param name="esriString">The string to parse</param>
        public void ReadEsriString(string esriString)
        {
            if (esriString.Contains("GEOGCS") == false) return;
            int iStart = esriString.IndexOf("GEOGCS") + 8;
            int iEnd = esriString.IndexOf(@""",", iStart) - 1;
            if (iEnd < iStart) return;
            _name = esriString.Substring(iStart, iEnd - iStart);
            _datum.ReadEsriString(esriString);
            _meridian.ReadEsriString(esriString);
            _unit.ReadEsriString(esriString);
        }

        /// <summary>
        /// Reads in parameters from the proj4 string that control the datum and prime meridian
        /// </summary>
        /// <param name="parameters">The dictionary of all the parameter names and values in string form</param>
        public void ReadProj4Parameters(Dictionary<string, string> parameters)
        {
            _meridian.ReadProj4Parameters(parameters);
            _datum.ReadProj4Params(parameters);
            switch (_datum.Spheroid.KnownEllipsoid)
            {
                case Proj4Ellipsoids.WGS_1984:
                    _name = "GCS_WGS_1984";
                    break;
            }
        }

        #endregion
    }
}
