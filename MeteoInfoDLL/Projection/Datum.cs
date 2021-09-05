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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 3:27:36 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System.Collections.Generic;
//using MapWindow.Drawing;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Datum
    /// </summary>
    public class Datum //: Descriptor, IEsriString
    {
        private const double SecToRad = 4.84813681109535993589914102357e-6;

        #region Private Variables

        

        private string _name;
        private Spheroid _spheroid;
        private double[] _toWGS84;
        private DatumTypes _datumtype;
        private string _description;
        private string[] _nadGrids;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Datum
        /// </summary>
        public Datum()
        {
            _spheroid = new Spheroid();
        }

        /// <summary>
        /// uses a string name of a standard datum to create a new instance of the Datum class
        /// </summary>
        /// <param name="standardDatum">The string name of the datum to use</param>
        public Datum(string standardDatum)
        {
            _spheroid = new Spheroid();
            AssignStandardDatum(standardDatum);
        }

        /// <summary>
        /// Uses a Proj4Datums enumeration in order to specify a known datum to
        /// define the spheroid and to WGS calculation method and parameters
        /// </summary>
        /// <param name="standardDatum">The Proj4Datums enumeration specifying the known datum</param>
        public Datum(Proj4Datums standardDatum)
        {
            _spheroid = new Spheroid();
            AssignStandardDatum(standardDatum.ToString());
            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compares two datums to see if they are actually describing the same thing and
        /// therefore don't need to be transformed.
        /// </summary>
        /// <param name="otherDatum">The other datum to compare against</param>
        /// <returns>The boolean result of the operation.</returns>
        public bool Matches(Datum otherDatum)
        {
            if (_datumtype != otherDatum.DatumType) return false;
            if (_spheroid.EquatorialRadius != otherDatum.Spheroid.EquatorialRadius) return false;
            if (_spheroid.PolarRadius != otherDatum.Spheroid.PolarRadius) return false;
            if(_datumtype == DatumTypes.Param3)
            {
                if (_toWGS84[0] != otherDatum.ToWGS84[0]) return false;
                if (_toWGS84[1] != otherDatum.ToWGS84[1]) return false;
                if (_toWGS84[2] != otherDatum.ToWGS84[2]) return false;
                return true;
            }
            if(_datumtype == DatumTypes.Param7)
            {
                if (_toWGS84[0] != otherDatum.ToWGS84[0]) return false;
                if (_toWGS84[1] != otherDatum.ToWGS84[1]) return false;
                if (_toWGS84[2] != otherDatum.ToWGS84[2]) return false;
                if (_toWGS84[3] != otherDatum.ToWGS84[3]) return false;
                if (_toWGS84[4] != otherDatum.ToWGS84[4]) return false;
                if (_toWGS84[5] != otherDatum.ToWGS84[5]) return false;
                if (_toWGS84[6] != otherDatum.ToWGS84[6]) return false;
                return true;
            }
            if (_datumtype == DatumTypes.GridShift)
            {
                if (_nadGrids.Length != otherDatum.NadGrids.Length) return false;
                for(int i = 0; i < _nadGrids.Length; i++)
                {
                    if(_nadGrids[i] != otherDatum.NadGrids[i]) return false;
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the datum defining the spherical characteristics of the model of the earth
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The spheroid of the earth, defining the maximal radius and the flattening factor
        /// </summary>
        public Spheroid Spheroid
        {
            get { return _spheroid; }
            set { _spheroid = value; }
        }

        /// <summary>
        /// Gets or sets the set of double parameters, (this can either be 3 or 7 parameters)
        /// used to transform this 
        /// </summary>
        public double[] ToWGS84
        {
            get { return _toWGS84; }
            set { _toWGS84 = value; }
        }

        /// <summary>
        /// Gets or sets the datum type, which clarifies how to perform transforms to WGS84
        /// </summary>
        public DatumTypes DatumType
        {
            get { return _datumtype; }
            set { _datumtype = value; }
        }

        /// <summary>
        /// Gets or sets an english description for this datum
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the array of string nadGrid
        /// </summary>
        public string[] NadGrids
        {
            get { return _nadGrids; }
            set { _nadGrids = value; }
        }

        #endregion




        #region IEsriString Members

        /// <summary>
        /// Creates an esri well known text string for the datum part of the string
        /// </summary>
        /// <returns>The datum portion of the esri well known text</returns>
        public string ToEsriString()
        {
            return @"DATUM[""" + _name + @"""," + Spheroid.ToEsriString() + "]";
        }

        /// <summary>
        /// parses the datum from the esri string
        /// </summary>
        /// <param name="esriString">The string to parse values from</param>
        public void ReadEsriString(string esriString)
        {
            if (esriString.Contains("DATUM") == false) return;
            int iStart = esriString.IndexOf("DATUM") + 7;
            int iEnd = esriString.IndexOf(@""",", iStart) - 1;
            if (iEnd < iStart) return;
            _name = esriString.Substring(iStart, iEnd - iStart);
            _spheroid.ReadEsriString(esriString);
        }

        private void AssignStandardDatum(string datumName)
        {
            string id = datumName.ToLower();
            switch (id)
            {
                case "wgs84":
                    _toWGS84 = new double[] { 0, 0, 0 };
                    _spheroid = new Spheroid(Proj4Ellipsoids.WGS_1984);
                    _description = "WGS 1984";
                    _datumtype = DatumTypes.WGS84;
                    _name = "D_WGS_1984";
                    break;
                case "ggrs87":
                    _toWGS84 = new[] { -199.87, 74.79, 246.62 };
                    _spheroid = new Spheroid(Proj4Ellipsoids.GRS_1980);
                    _description = "Greek Geodetic Reference System 1987";
                    _datumtype = DatumTypes.Param3;
                    break;
                case "nad83":
                    _toWGS84 = new double[] { 0, 0, 0 };
                    _spheroid = new Spheroid(Proj4Ellipsoids.GRS_1980);
                    _description = "North American Datum 1983";
                    _datumtype = DatumTypes.WGS84;
                    break;
                case "nad27":
                    _nadGrids = new[] { "@conus", "@alaska", "@ntv2_0.gsb", "@ntv1_can.dat" };
                    _spheroid = new Spheroid(Proj4Ellipsoids.Clarke_1866);
                    _description = "North American Datum 1927";
                    _datumtype = DatumTypes.GridShift;
                    break;
                case "potsdam":
                    _toWGS84 = new[] { 606.0, 23.0, 413.0 };
                    _spheroid = new Spheroid(Proj4Ellipsoids.Bessel_1841);
                    _description = "Potsdam Rauenberg 1950 DHDN";
                    _datumtype = DatumTypes.Param3;
                    break;
                case "carthage":
                    _toWGS84 = new[] { -263.0, 6, 413 };
                    _spheroid = new Spheroid(Proj4Ellipsoids.ClarkeModified_1880);
                    _description = "Carthage 1934 Tunisia";
                    _datumtype = DatumTypes.Param3;
                    break;
                case "hermannskogel":
                    _toWGS84 = new[] { 653.0, -212.0, 449 };
                    _spheroid = new Spheroid(Proj4Ellipsoids.Bessel_1841);
                    _description = "Hermannskogel";
                    _datumtype = DatumTypes.Param3;
                    break;
                case "ire65":
                    _toWGS84 = new[] { 482.530, -130.569, 564.557, -1.042, -.214, -.631, 8.15 };
                    _spheroid = new Spheroid(Proj4Ellipsoids.AiryModified);
                    _description = "Ireland 1965";
                    _datumtype = DatumTypes.Param7;
                    break;
                case "nzgd49":
                    _toWGS84 = new[] { 59.47, -5.04, 187.44, 0.47, -0.1, 1.024, -4.5993 };
                    _spheroid = new Spheroid(Proj4Ellipsoids.International_1909);
                    _description = "New Zealand";
                    _datumtype = DatumTypes.Param7;
                    break;
                case "osgb36":
                    _toWGS84 = new[] { 446.448, -125.157, 542.060, 0.1502, 0.2470, 0.8421, -20.4894 };
                    _spheroid = new Spheroid(Proj4Ellipsoids.Airy_1830);
                    _description = "Airy 1830";
                    _datumtype = DatumTypes.Param7;
                    break;
            }
        }

        /// <summary>
        /// Reads the proj4 parameters and parses out the ones that control the 
        /// datum.
        /// </summary>
        /// <param name="parameters"></param>
        /// <remarks>Originally ported from pj_datum_set.c</remarks>
        public void ReadProj4Params(Dictionary<string, string> parameters)
        {
            DatumType = DatumTypes.Unknown;

            /* -------------------------------------------------------------------- */
            /*      Is there a datum definition in the parameters list?  If so,     */
            /*      add the defining values to the parameter list.  Notice that       */
            /*      this will append the ellipse definition as well as the          */
            /*      towgs84= and related parameters.  It should also be pointed     */
            /*      out that the addition is permanent rather than temporary        */
            /*      like most other keyword expansion so that the ellipse           */
            /*      definition will last into the pj_ell_set() function called      */
            /*      after this one.                                                 */
            /* -------------------------------------------------------------------- */

            if (parameters.ContainsKey("datum"))
            {
                AssignStandardDatum(parameters["datum"]);
                // Even though th ellipsoid is set by its known definition, we permit overriding it with a specifically defined ellps parameter
            }

            // ELLIPSOID PARAMETER
            if (parameters.ContainsKey("ellps"))
            {
                Spheroid = new Spheroid(parameters["ellps"]);
            }
            else
            {
                if (parameters.ContainsKey("a"))
                {
                    Spheroid.EquatorialRadius = double.Parse(parameters["a"]);
                }
                if (parameters.ContainsKey("b"))
                {
                    Spheroid.PolarRadius = double.Parse(parameters["b"]);
                }
                if (parameters.ContainsKey("rf"))
                {
                    Spheroid.SetInverseFlattening(double.Parse(parameters["rf"]));
                }
                if (parameters.ContainsKey("R"))
                {
                    Spheroid.EquatorialRadius = double.Parse(parameters["R"]);
                }

            }

            // DATUM PARAMETER
            if (parameters.ContainsKey("nadgrids"))
            {
                _nadGrids = parameters["nadgrids"].Split(',');
                _datumtype = DatumTypes.GridShift;
            }
            else if (parameters.ContainsKey("towgs84"))
            {
                string[] rawVals = parameters["towgs84"].Split(',');
                _toWGS84 = new double[rawVals.Length];
                for(int i = 0; i < rawVals.Length; i++)
                {
                    _toWGS84[i] = double.Parse(rawVals[i]);
                }
                _datumtype = DatumTypes.Param7;
                if (_toWGS84.Length < 7) _datumtype = DatumTypes.Param3;
                if (_toWGS84[3] == 0.0 && _toWGS84[4] == 0.0 &&
                   _toWGS84[5] == 0.0 && _toWGS84[6] == 0.0) _datumtype = DatumTypes.Param3;
                if(_datumtype == DatumTypes.Param7)
                {
                    // Trnasform from arc seconds to radians
                    _toWGS84[3] *= SecToRad;
                    _toWGS84[4] *= SecToRad;
                    _toWGS84[5] *= SecToRad;
                    // transform from parts per millon to scaling factor
                    _toWGS84[6] = (_toWGS84[6]/1000000.0) + 1;
                }
            }

        }

        #endregion
    }
}
