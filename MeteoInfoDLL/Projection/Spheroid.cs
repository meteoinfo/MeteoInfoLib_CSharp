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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 10:09:16 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
//using MapWindow.Drawing;
namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Spheroid (Defaults to WGS84)
    /// </summary>
    public class Spheroid //: Descriptor, IEsriString
    {

        #region Private Variables

        private string _name;
        private double _equatorialRadius;
        private double _polarRadius;
        private Proj4Ellipsoids _knownEllipsoid;
        private Dictionary<Proj4Ellipsoids, string> _proj4Names;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Spheroid
        /// </summary>
        public Spheroid()
        {
            AddNames();
            AssignKnownEllipsoid(Proj4Ellipsoids.WGS_1984);
        }

        /// <summary>
        /// Creates a new spheroid using an the equatorial radius in meters and
        /// a flattening coefficient that is the inverse flattening factor.
        /// eg. for WGS84 (6378137.0, 298.257223563)
        /// </summary>
        /// <param name="equatorialRadius">The semi-major axis</param>
        /// <param name="inverseFlattening">The inverse of the flattening factor</param>
        public Spheroid(double equatorialRadius, double inverseFlattening)
        {
            _equatorialRadius = equatorialRadius;
            SetInverseFlattening(inverseFlattening);
            AddNames();
            AssignKnownEllipsoid(Proj4Ellipsoids.WGS_1984);
            
        }

       
        /// <summary>
        /// For perfect spheres, you just need to specify one radius, which will be 
        /// applied to both radii.  You can then directly change the polar or 
        /// equatorial radius if necessary using the properties.
        /// </summary>
        /// <param name="radius">The radius of the sphere</param>
        public Spheroid(double radius)
        {
            _polarRadius = radius;
            _equatorialRadius = radius;
            AddNames();
            AssignKnownEllipsoid(Proj4Ellipsoids.WGS_1984);
        }

        /// <summary>
        /// The ellps parameter in a proj4 string will only work with certain
        /// pre-defined spheroids, enumerated in the Proj4Ellipsoids enumeration.
        /// Custom spheroids can be specified but will use the a and b parameters
        /// when creating a proj4 parameter instead of using the ellps parameter.
        /// </summary>
        /// <param name="knownEllipse">Any of several predefined geographic ellipses</param>
        public Spheroid(Proj4Ellipsoids knownEllipse)
        {
            AddNames();
            AssignKnownEllipsoid(knownEllipse);
        }

        /// <summary>
        /// Given the proj4 code, this will set the radii correctly.
        /// </summary>
        /// <param name="proj4Ellips"></param>
        public Spheroid(string proj4Ellips)
        {
            AddNames();
            foreach (KeyValuePair<Proj4Ellipsoids, string> pair in _proj4Names)
            {
                if(pair.Value == proj4Ellips)
                {
                    AssignKnownEllipsoid(pair.Key);
                    return;
                }
            }
        }

        private void AssignKnownEllipsoid(Proj4Ellipsoids knownEllipse)
        {
            _name = knownEllipse.ToString();
            _knownEllipsoid = knownEllipse;
            switch (knownEllipse)
            {
                case Proj4Ellipsoids.Airy_1830:
                    _equatorialRadius = 6377563.396;
                    _polarRadius = 6356256.910;
                    break;
                case Proj4Ellipsoids.AiryModified:
                    _equatorialRadius = 6377340.189;
                    _polarRadius = 6356034.446;
                    break;
                case Proj4Ellipsoids.Andrae_1876:
                    _equatorialRadius = 6377104.43;
                    SetInverseFlattening(300);
                    break;
                case Proj4Ellipsoids.AppPhysics_1965:
                    _equatorialRadius = 6378137.0;
                    SetInverseFlattening(298.25);
                    break;
                case Proj4Ellipsoids.Austrailia_SouthAmerica:
                    _equatorialRadius = 6378160.0;
                    SetInverseFlattening(298.25);
                    break;
                case Proj4Ellipsoids.Bessel_1841:
                    _equatorialRadius = 6377397.155;
                    SetInverseFlattening(299.1528128);
                    break;
                case Proj4Ellipsoids.BesselNamibia:
                    _equatorialRadius = 6377483.865;
                    SetInverseFlattening(299.1528128);
                    break;
                case Proj4Ellipsoids.Clarke_1866:
                    _equatorialRadius = 6378206.4;
                    _polarRadius = 6356583.8;
                    break;
                case Proj4Ellipsoids.ClarkeModified_1880:
                    _equatorialRadius = 6378249.145;
                    SetInverseFlattening(293.4663);
                    break;
                case Proj4Ellipsoids.CPM_1799:
                    _equatorialRadius = 6375738.7;
                    SetInverseFlattening(334.29);
                    break;
                case Proj4Ellipsoids.Custom: // Default to WGS84
                    _equatorialRadius = 6378137.0;
                    SetInverseFlattening(298.257223563);
                    break;
                case Proj4Ellipsoids.Delambre_1810:
                    _equatorialRadius = 6376428;
                    SetInverseFlattening(311.5);
                    break;
                case Proj4Ellipsoids.Engelis_1985:
                    _equatorialRadius = 6378136.05;
                    SetInverseFlattening(298.2566);
                    break;
                case Proj4Ellipsoids.Everest_1830:
                    _equatorialRadius = 6377276.345;
                    SetInverseFlattening(300.8017);
                    break;
                case Proj4Ellipsoids.Everest_1948:
                    _equatorialRadius = 6377304.063;
                    SetInverseFlattening(300.8017);
                    break;
                case Proj4Ellipsoids.Everest_1956:
                    _equatorialRadius = 6377301.243;
                    SetInverseFlattening(300.8017);
                    break;
                case Proj4Ellipsoids.Everest_1969:
                    _equatorialRadius = 6377295.664;
                    SetInverseFlattening(300.8017);
                    break;
                case Proj4Ellipsoids.Everest_SS:
                    _equatorialRadius = 6377298.556;
                    SetInverseFlattening(300.8017);
                    break;
                case Proj4Ellipsoids.Fischer_1960:
                    _equatorialRadius = 6378166;
                    SetInverseFlattening(298.3);
                    break;
                case Proj4Ellipsoids.Fischer_1968:
                    _equatorialRadius = 6378150;
                    SetInverseFlattening(298.3);
                    break;
                case Proj4Ellipsoids.FischerModified_1960:
                    _equatorialRadius = 6378155;
                    SetInverseFlattening(298.3);
                    break;
                case Proj4Ellipsoids.GRS_1967:
                    _equatorialRadius = 6378160.0;
                    SetInverseFlattening(298.2471674270);
                    break;
                case Proj4Ellipsoids.GRS_1980:
                    _equatorialRadius = 6378137.0;
                    SetInverseFlattening(298.257222101);
                    break;
                case Proj4Ellipsoids.Helmert_1906:
                    _equatorialRadius = 6378200;
                    SetInverseFlattening(298.3);
                    break;
                case Proj4Ellipsoids.Hough:
                    _equatorialRadius = 6378270.0;
                    SetInverseFlattening(297);
                    break;
                case Proj4Ellipsoids.IAU_1976:
                    _equatorialRadius = 6378140.0;
                    SetInverseFlattening(298.257);
                    break;
                case Proj4Ellipsoids.International_1909:
                    _equatorialRadius = 6378388.0;
                    SetInverseFlattening(297);
                    break;
                case Proj4Ellipsoids.InternationalNew_1967:
                    _equatorialRadius = 6378157.5;
                    _polarRadius = 6356772.2;
                    break;
                case Proj4Ellipsoids.Krassovsky_1942:
                    _equatorialRadius = 6378245.0;
                    SetInverseFlattening(298.3);
                    break;
                case Proj4Ellipsoids.Kaula_1961:
                    _equatorialRadius = 6378163;
                    SetInverseFlattening(298.24);
                    break;
                case Proj4Ellipsoids.Lerch_1979:
                    _equatorialRadius = 6378139;
                    SetInverseFlattening(298.257);
                    break;
                case Proj4Ellipsoids.Maupertius_1738:
                    _equatorialRadius = 6397300;
                    SetInverseFlattening(191);
                    break;
                case Proj4Ellipsoids.Merit_1983:
                    _equatorialRadius = 6378137.0;
                    SetInverseFlattening(298.257);
                    break;
                case Proj4Ellipsoids.NavalWeaponsLab_1965:
                    _equatorialRadius = 6378145.0;
                    SetInverseFlattening(298.25);
                    break;
                case Proj4Ellipsoids.Plessis_1817:
                    _equatorialRadius = 6376523;
                    _polarRadius = 6355863;
                    break;
                case Proj4Ellipsoids.SoutheastAsia:
                    _equatorialRadius = 6378155.0;
                    _polarRadius = 6356773.3205;
                    break;
                case Proj4Ellipsoids.SovietGeodeticSystem_1985:
                    _equatorialRadius = 6378136.0;
                    SetInverseFlattening(298.257);
                    break;
                case Proj4Ellipsoids.Sphere:
                    _equatorialRadius = 6370997.0;
                    _polarRadius = 6370997.0;
                    break;
                case Proj4Ellipsoids.Walbeck:
                    _equatorialRadius = 6376896.0;
                    _polarRadius = 6355834.8467;
                    break;
                case Proj4Ellipsoids.WGS_1960:
                    _equatorialRadius = 6378165.0;
                    SetInverseFlattening(298.3);
                    break;
                case Proj4Ellipsoids.WGS_1966:
                    _equatorialRadius = 6378145.0;
                    SetInverseFlattening(298.25);
                    break;
                case Proj4Ellipsoids.WGS_1972:
                    _equatorialRadius = 6378135.0;
                    SetInverseFlattening(298.26);
                    break;
                case Proj4Ellipsoids.WGS_1984:
                    _equatorialRadius = 6378137.0;
                    SetInverseFlattening(298.257223563);
                    break;

            }

        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Calculates the flattening factor, (a - b) / a.  
        /// </summary>
        /// <returns></returns>
        public double FlatteningFactor()
        {
            return (_equatorialRadius - _polarRadius)/_equatorialRadius;
        }

        /// <summary>
        /// Uses the current known ellipsoid to return a code name for the proj4 string if possible.
        /// Otherwise, this returns the radial parameters a and b.
        /// </summary>
        /// <returns></returns>
        public string GetProj4String()
        {
            if(_knownEllipsoid != Proj4Ellipsoids.Custom)return _proj4Names[_knownEllipsoid];
            return "+a=" + _equatorialRadius + " +b=" + _polarRadius;
        }

        /// <summary>
        /// Calculates the eccentrity according to e = sqrt(2f - f^2) where f is the flattening factor.
        /// </summary>
        /// <returns></returns>
        public double Eccentricity()
        {
            double f = FlatteningFactor();
            return Math.Sqrt(2*f - f*f);
        }

        /// <summary>
        /// Calculates the square of eccentricity according to es = (2f - f^2) where f is the flattening factor.
        /// </summary>
        /// <returns></returns>
        public double EccentricitySquared()
        {
            double f = FlatteningFactor();
            return 2*f - f*f;
        }
       
        /// <summary>
        /// Calculates the inverse of the flattening factor, commonly saved to ESRI projections,
        /// or else provided as the "rf" parameter for Proj4 strings.  This is simply calculated
        /// as a / (a - b) where a is the semi-major axis and b is the semi-minor axis.
        /// </summary>
        /// <returns></returns>
        public double GetInverseFlattening()
        {
            if (_polarRadius == _equatorialRadius) return 0; // prevent divide by zero for spheres.
            return (_equatorialRadius)/(_equatorialRadius - _polarRadius);
        }

        /// <summary>
        /// Each of the enumerated known ellipsoids is encoded by an ellps parameter specified by 
        /// the corresponding string value.  Ellipsoids that are not found here or are specified
        /// as "Custom" in the enuemration will be replaced with an 'a' and a 'b' parameter instead.
        /// </summary>
        public Dictionary<Proj4Ellipsoids, string> Proj4Names
        {
            get { return _proj4Names; }
        }

        /// <summary>
        /// Gets a boolean that is true if the spheroid has been flattened.
        /// </summary>
        /// <returns>Boolean, true if the spheroid is oblate (or flattened)</returns>
        public bool IsOblate()
        {
            return (_polarRadius < _equatorialRadius);
        }

        private void AddNames()
        {
            _proj4Names = new Dictionary<Proj4Ellipsoids, string>();
            _proj4Names.Add(Proj4Ellipsoids.Airy_1830, "airy");
            _proj4Names.Add(Proj4Ellipsoids.AiryModified, "mod_airy");
            _proj4Names.Add(Proj4Ellipsoids.Andrae_1876, "andrae");
            _proj4Names.Add(Proj4Ellipsoids.AppPhysics_1965, "APL4.9");
            _proj4Names.Add(Proj4Ellipsoids.Austrailia_SouthAmerica, "aust_SA");
            _proj4Names.Add(Proj4Ellipsoids.Bessel_1841, "bessel");
            _proj4Names.Add(Proj4Ellipsoids.BesselNamibia, "Bess_nam");
            _proj4Names.Add(Proj4Ellipsoids.Clarke_1866, "clrk66");
            _proj4Names.Add(Proj4Ellipsoids.ClarkeModified_1880, "clrk80");
            _proj4Names.Add(Proj4Ellipsoids.CPM_1799, "CPM");
            _proj4Names.Add(Proj4Ellipsoids.Custom, "");
            _proj4Names.Add(Proj4Ellipsoids.Delambre_1810, "delmbr");
            _proj4Names.Add(Proj4Ellipsoids.Engelis_1985, "engelis");
            _proj4Names.Add(Proj4Ellipsoids.Everest_1830, "evrst30");
            _proj4Names.Add(Proj4Ellipsoids.Everest_1948, "evrst48");
            _proj4Names.Add(Proj4Ellipsoids.Everest_1956, "evrst56");
            _proj4Names.Add(Proj4Ellipsoids.Everest_1969, "evrst69");
            _proj4Names.Add(Proj4Ellipsoids.Everest_SS, "evrstSS");
            _proj4Names.Add(Proj4Ellipsoids.Fischer_1960, "fschr60");
            _proj4Names.Add(Proj4Ellipsoids.FischerModified_1960, "fschr60m");
            _proj4Names.Add(Proj4Ellipsoids.Fischer_1968, "fschr68");
            _proj4Names.Add(Proj4Ellipsoids.GRS_1967, "GRS67");
            _proj4Names.Add(Proj4Ellipsoids.GRS_1980, "GRS80");
            _proj4Names.Add(Proj4Ellipsoids.Helmert_1906, "helmert");
            _proj4Names.Add(Proj4Ellipsoids.Hough, "hough");
            _proj4Names.Add(Proj4Ellipsoids.IAU_1976, "IAU76");
            _proj4Names.Add(Proj4Ellipsoids.International_1909, "intl");
            _proj4Names.Add(Proj4Ellipsoids.InternationalNew_1967, "new_intl");
            _proj4Names.Add(Proj4Ellipsoids.Krassovsky_1942, "krass");
            _proj4Names.Add(Proj4Ellipsoids.Lerch_1979, "lerch");
            _proj4Names.Add(Proj4Ellipsoids.Maupertius_1738, "mprts");
            _proj4Names.Add(Proj4Ellipsoids.Merit_1983, "MERIT");
            _proj4Names.Add(Proj4Ellipsoids.NavalWeaponsLab_1965, "NWL9D");
            _proj4Names.Add(Proj4Ellipsoids.Plessis_1817, "plessis");
            _proj4Names.Add(Proj4Ellipsoids.SoutheastAsia, "SEasia");
            _proj4Names.Add(Proj4Ellipsoids.SovietGeodeticSystem_1985, "SGS85");
            _proj4Names.Add(Proj4Ellipsoids.Sphere, "sphere");
            _proj4Names.Add(Proj4Ellipsoids.Walbeck, "walbeck");
            _proj4Names.Add(Proj4Ellipsoids.WGS_1960, "WGS60");
            _proj4Names.Add(Proj4Ellipsoids.WGS_1966, "WGS66");
            _proj4Names.Add(Proj4Ellipsoids.WGS_1972, "WGS72");
            _proj4Names.Add(Proj4Ellipsoids.WGS_1984, "WGS84");
        }
      

        /// <summary>
        /// Sets the value by using the current semi-major axis (Equatorial Radius) in order to 
        /// calculate the semi-minor axis (Polar Radius).
        /// </summary>
        public void SetInverseFlattening(double value)
        {
            _polarRadius = _equatorialRadius - _equatorialRadius/value;
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string name of the spheroid.
        /// e.g.: WGS_1984
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// A spheroid is a pole flattened (oblate) sphere, with the radii of two axes being equal and longer
        /// than the third.  This is the radial measure of one of these major axes in meters.
        /// e.g. 6,378,137 for WGS 84
        /// </summary>
        public double EquatorialRadius
        {
            get { return _equatorialRadius; }
            set { _equatorialRadius = value; }
        }

        

        /// <summary>
        /// A spheroid is a pole flattened (oblate) sphere, with the radii of two axes being equal and longer
        /// than the third.  This is the radial measure of the smaller polar axis in meters.  One option is
        /// to specify this directly, but it can also be calculated using the major axis and the flattening factor.
        /// </summary>
        public double PolarRadius
        {
            get { return _polarRadius; }
            set { _polarRadius = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Proj4Ellipsoids KnownEllipsoid
        {
            get { return _knownEllipsoid; }
            set
            {
                AssignKnownEllipsoid(value);
            }
        }

        
        #endregion




        #region IEsriString Members

        /// <summary>
        /// Converts the spheroid parameters into a valid esri expression that uses the semi-major axis
        /// and the reciprocal flattening factor
        /// </summary>
        /// <returns></returns>
        public string ToEsriString()
        {
            return @"SPHEROID[""" + _name + @"""," + _equatorialRadius + "," + GetInverseFlattening() + "]";
        }

        /// <summary>
        /// Reads the ESRI string to define the spheroid, which controls how flattened the earth's radius is
        /// </summary>
        /// <param name="esriString"></param>
        public void ReadEsriString(string esriString)
        {
            if (esriString.Contains("SPHEROID") == false) return;
            int iStart = esriString.IndexOf("SPHEROID") + 9;
            int iEnd = esriString.IndexOf("]", iStart);
            if (iEnd < iStart) return;
            string extracted = esriString.Substring(iStart, iEnd - iStart);
            string[] terms = extracted.Split(',');
            _name = terms[0];
            _name = _name.Substring(1, _name.Length - 2);
            _equatorialRadius = double.Parse(terms[1], System.Globalization.CultureInfo.InvariantCulture);
            SetInverseFlattening(double.Parse(terms[2], System.Globalization.CultureInfo.InvariantCulture));
        }



        #endregion

        
    }
}
