//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// The original content was ported from the C language from the 4.6 version of Proj4 libraries. 
// Frank Warmerdam has released the full content of that version under the MIT license which is
// recognized as being approximately equivalent to public domain.  The original work was done 
// mostly by Gerald Evenden.  The latest versions of the C libraries can be obtained here:
// http://trac.osgeo.org/proj/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/23/2009 9:33:11 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// TransverseMercator is a class allowing the transverse mercator transform as transcribed
    /// from the 4.6 version of the Proj4 library (pj_tmerc.c)
    /// </summary>
    public class Transform : ITransform
    {

        #region Private Variables

        /// <summary>
        /// For transforms that distinguish between polar, oblique and equitorial modes
        /// </summary>
        protected enum Modes
        {
            /// <summary>
            /// North Pole
            /// </summary>
            NorthPole = 0,
            /// <summary>
            /// South Pole
            /// </summary>
            SouthPole = 1,
            /// <summary>
            /// Equitorial
            /// </summary>
            Equitorial = 2,
            /// <summary>
            /// Oblique
            /// </summary>
            Oblique = 3
        }


        /// <summary>
        /// Pi/2
        /// </summary>
        protected const double HalfPi = 1.5707963267948966; //= Math.PI/2;
        /// <summary>
        /// Math.Pi / 4
        /// </summary>
        protected const double FortPi = 0.78539816339744833; //= Math.PI/4;
        /// <summary>
        /// 2 * Math.Pi
        /// </summary>
        protected const double TwoPi = Math.PI*2;
        /// <summary>
        /// 1E-10
        /// </summary>
        protected const double EPS10 = 1E-10;
        /// <summary>
        /// Analytic Hk
        /// </summary>
        protected const int IsAnalHk = 4;
        /// <summary>
        /// Analytic Conv
        /// </summary>
        protected const int IsAnalConv = 10;
        /// <summary>
        /// Analytic Xl Yl
        /// </summary>
        protected const int IsAnalXlYl = 1;
        /// <summary>
        /// Analytic Xp Yp 
        /// </summary>
        protected const int IsAnalXpYp = 2;
        /// <summary>
        /// Radians to Degrees
        /// </summary>
        protected const double RadToDeg = 57.29577951308232;
        /// <summary>
        /// Degrees to Radians
        /// </summary>
        protected const double DegToRad = .0174532925199432958;

        /// <summary>
        /// The major axis
        /// </summary>
        protected double A;

        /// <summary>
        /// 1/a
        /// </summary>
        protected double Ra;

        /// <summary>
        /// 1 - e^2;
        /// </summary>
        protected double OneEs;

        /// <summary>
        /// 1/OneEs
        /// </summary>
        protected double ROneEs;

        /// <summary>
        /// Eccentricity
        /// </summary>
        protected double E;

        /// <summary>
        /// True if the spheroid is flattened
        /// </summary>
        protected bool IsElliptical;
        
        /// <summary>
        /// Eccentricity Squared
        /// </summary>
        protected double Es; // eccentricity squared
        
        /// <summary>
        /// Central Latitude
        /// </summary>
        protected double Phi0; // central latitude
        
        /// <summary>
        /// Central Longitude
        /// </summary>
        protected double Lam0; // central longitude
        
        /// <summary>
        /// False Easting
        /// </summary>
        protected double X0; // easting
        
        /// <summary>
        /// False Northing
        /// </summary>
        protected double Y0; // northing
        
        /// <summary>
        /// Scaling Factor
        /// </summary>
        protected double K0; // scaling factor
        
        /// <summary>
        /// Scaling to meter
        /// </summary>
        protected double ToMeter; // cartesian scaling
        
        /// <summary>
        /// Scaling from meter
        /// </summary>
        protected double FromMeter; // cartesian scaling from meter

        /// <summary>
        /// The integer index representing lambda values in arrays representing geodetic coordinates
        /// </summary>
        protected const int Lambda = 0;
        /// <summary>
        /// The integer index representing phi values in arrays representing geodetic coordinates
        /// </summary>
        protected const int Phi = 1;
        /// <summary>
        /// The integer index representing X values in arrays representing linear coordinates
        /// </summary>
        protected const int X = 0;
        /// <summary>
        /// The integer index representing Y values in arrays representing linear coordinates
        /// </summary>
        protected const int Y = 1;

        /// <summary>
        /// The integer index representing real values in arrays representing complex numbers
        /// </summary>
        protected const int R = 0;
        /// <summary>
        /// The integer index representing immaginary values in arrays representing complex numbers
        /// </summary>
        protected const int I = 1;

        private string _esriName;
        private string _proj4Name;
        private string[] _proj4Aliases;
        private ProjectionNames _ProjectionName;
        #endregion

        #region Constructors

       

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the parameters from the projection info
        /// </summary>
        /// <param name="proj">The projection information used to control this transform</param>
        public void Init(ProjectionInfo proj)
        {
            // Setup protected values common to all the projections that inherit from this projection
            Es = proj.GeographicInfo.Datum.Spheroid.EccentricitySquared();
            if (proj.LatitudeOfOrigin != null) Phi0 = proj.GetPhi0();
            if(proj.CentralMeridian != null) Lam0 = proj.GetLam0();
            if(proj.FalseEasting != null) X0 = proj.FalseEasting.Value;
            if(proj.FalseNorthing != null) Y0 = proj.FalseNorthing.Value;
            K0 = proj.ScaleFactor;
            A = proj.GeographicInfo.Datum.Spheroid.EquatorialRadius;
            E = proj.GeographicInfo.Datum.Spheroid.Eccentricity();
            Ra = 1/A;
            OneEs = 1 - Es;
            ROneEs = 1/OneEs;
            ToMeter = 1;
            FromMeter = 1;
            //_datumParams = proj.GeographicInfo.Datum.ToWGS84;
            if(proj.Unit != null)
            {
                ToMeter = proj.Unit.Meters;
                FromMeter = 1/proj.Unit.Meters;
            }


            if (Es != 0)
            {
                IsElliptical = true;
            }
            OnInit(proj);
        }

        /// <summary>
        /// Uses an enumeration to generate a new instance of a known transform class
        /// </summary>
        /// <param name="value">The member of the KnownTransforms to instantiate</param>
        /// <returns>A new ITransform interface representing the specific transform</returns>
        public static ITransform FromKnownTransform(KnownTransforms value)
        {
            string name = value.ToString();
            foreach (ITransform transform in TransformManager.DefaultTransformManager.Transforms)
            {
                //if (transform.Name == name) return transform.Copy();
                if (transform.Name == name) return (ITransform)transform.Clone();
            }
            return null;
        }

        /// <summary>
        /// Transforms all the coordinates by cycling through them in a loop, 
        /// transforming each one.  Only the 0 and 1 values of each coordinate will be 
        /// transformed, higher dimensional values will be copied.
        /// </summary>
        public void Forward(double[][] points, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < numPoints; i++)
            {
                points[i] = Forward(points[i]);
            }
        }

        /// <summary>
        /// Transforms all the coordinates by cycling through them in a loop, 
        /// transforming each one.  Only the 0 and 1 values of each coordinate will be 
        /// transformed, higher dimensional values will be copied.
        /// </summary>
        public void Inverse(double[][] points, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < numPoints; i++)
            {
                points[i] = Inverse(points[i]);
            }

        }
        
        /// <summary>
        /// Transforms a single coordinate, so that specific coordinates can be chosen
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        public double[] Forward(double[] lp)
        {
            double[] xy = new double[lp.Length];
            for (int i = 2; i < lp.Length; i++)
            {
                // Copy any other values in the vertices like z values or whatever.
                xy[i] = lp[i];
            }

            OnForward(lp, xy);

            return xy;
        }

       
        /// <summary>
        /// Transforms a single coordinate, so that specific coordinates can be chosen
        /// </summary>
        /// <param name="xy"></param>
        /// <returns></returns>
        public double[] Inverse(double[] xy)
        {
            double[] lp = new double[xy.Length];
            for (int i = 2; i < xy.Length; i++)
            {
                // Copy any other values in the vertices like z values or whatever.
                lp[i] = xy[i];
            }

            OnInverse(xy, lp);

            return lp;
        }

        /// <summary>
        /// Special factor calculations for a factors calculation
        /// </summary>
        /// <param name="lp">lambda-phi</param>
        /// <param name="p">The projection</param>
        /// <param name="fac">The Factors</param>
        public void Special(double[] lp, ProjectionInfo p, Factors fac)
        {
            OnSpecial(lp, p, fac);
        }

        /// <summary>
        /// Allows for some custom code during a process method
        /// </summary>
        /// <param name="lp">lambda-phi</param>
        /// <param name="p">The projection coordinate system information</param>
        /// <param name="fac">The Factors</param>
        protected  virtual void OnSpecial(double[] lp, ProjectionInfo p, Factors fac)
        {
            // some projections support this as part of a process routine,
            // this will not affect forward or inverse transforms

        }

        #endregion


        #region Properties

        

        /// <summary>
        /// Gets or sets the string name of this projection as it appears in .prj files and
        /// ESRI wkt.  This can also be several names separated by a semicolon.
        /// </summary>
        public string Name
        {
            get { return _esriName; }
            protected set { _esriName = value; }
        }

        /// <summary>
        /// Gets or sets the string name of this projection as it should appear in proj4 strings.
        /// This can also be several names deliminated by a semicolon.
        /// </summary>
        public string Proj4Name
        {
            get { return _proj4Name; }
            protected set { _proj4Name = value;}
        }

        /// <summary>
        /// Gets or sets a list of optional aliases that can be used in the place of the Proj4Name.
        /// This will only be used during reading, and will not be used during writing.
        /// </summary>
        public string[] Proj4Aliases
        {
            get { return _proj4Aliases; }
            protected set { _proj4Aliases = value; }
        }

        /// <summary>
        /// Get or set projection name enum
        /// </summary>
        public ProjectionNames ProjectionName
        {
            get { return _ProjectionName; }
            set { _ProjectionName = value; }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected virtual void OnInit(ProjectionInfo projInfo)
        {
            
        }

        /// <summary>
        /// Transforms cartesian x, y to geodetic lambda, phi
        /// </summary>
        /// <param name="lp">in -> the lambda, phi coordinates</param>
        /// <param name="xy">out -> the cartesian x, y</param>
        protected virtual void OnForward(double[] lp, double[] xy)
        {
            
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected virtual void OnInverse(double[] xy, double[] lp)
        {
            
        }

        

        #endregion


        #region ICloneable Members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Transform copy = MemberwiseClone() as Transform;
            return copy;
        }

        /// <summary>
        /// This allows for custom behavior to override the base behavior
        /// </summary>
        /// <param name="duplicate"></param>
        protected virtual void OnCopy(Transform duplicate)
        {
            
        }

        #endregion
    }
}
