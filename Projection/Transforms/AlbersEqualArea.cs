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
// The original content was ported from the C language from the 4.6 version of Proj4 libraries. 
// Frank Warmerdam has released the full content of that version under the MIT license which is
// recognized as being approximately equivalent to public domain.  The original work was done 
// mostly by Gerald Evenden.  The latest versions of the C libraries can be obtained here:
// http://trac.osgeo.org/proj/
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 10:15:38 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// AlbersEqualArea
    /// </summary>
    public class AlbersEqualArea : Transform
    {
        #region Private Variables

        private const int NIter = 15;
        private const double EPSILON = 1.0E-7;
        private const double TOL = 1E-10;
        private const double TOL7 = 1E-7;

        private double _ec;
        private double _n;
        private double _c;
        private double _dd;
        private double _n2;
        private double _rho0;
        private double _rho;
        private double _phi1;
        private double _phi2;

      

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AlbersEqualArea
        /// </summary>
        public AlbersEqualArea()
        {
            Name = "Albers";
            Proj4Name = "aea";
            ProjectionName = ProjectionNames.Albers_Conic_Equal_Area;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The forward transform from geodetic units to linear units
        /// </summary>
        /// <param name="lp">The array of lambda, phi coordinates</param>
        /// <param name="xy">The array of x, y coordinates</param>
        protected override void OnForward(double[] lp, double[] xy)
        {
            if ((_rho = _c - (IsElliptical ? _n * Proj.Qsfn(Math.Sin(lp[Phi]),
            E, OneEs) : _n2 * Math.Sin(lp[Phi]))) < 0)
            {
                xy[X] = double.NaN;
                xy[Y] = double.NaN;
                //throw new ProjectionException(20);
                return;
            }
	        _rho = _dd * Math.Sqrt(_rho);
	        xy[X] = _rho * Math.Sin( lp[Lambda] *= _n );
	        xy[Y] = _rho0 - _rho * Math.Cos(lp[Lambda]);
        }

        
        private static double PhiFn(double qs, double te, double toneEs) 
        {
            double dphi;
	        double myPhi = Math.Asin(.5 * qs);
	        if (te < EPSILON) return( myPhi );
	        int i = NIter;
	        do 
            {
		        double sinpi = Math.Sin(myPhi);
		        double cospi = Math.Cos(myPhi);
		        double con = te * sinpi;
		        double com = 1- con * con;
		        dphi = .5 * com * com / cospi * (qs / toneEs -
		        sinpi / com + .5 / te * Math.Log((1- con) / (1+ con)));
		        myPhi += dphi;
	        } 
            while (Math.Abs(dphi) > TOL && --i > 0);
	        return( i != 0 ? myPhi : double.MaxValue );
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            if( (_rho = Proj.Hypot(xy[X], xy[Y] = _rho0 - xy[Y])) != 0.0 ) 
            {   
                if (_n < 0) 
                {
			        _rho = -_rho;
			        xy[X] = -xy[X];
			        xy[Y] = -xy[Y];
		        }
		        lp[Phi] =  _rho / _dd;
		        if (IsElliptical) 
                {
			        lp[Phi] = (_c - lp[Phi] * lp[Phi]) / _n;
			        if (Math.Abs(_ec - Math.Abs(lp[Phi])) > TOL7)
                    {
                        if ((lp[Phi] = PhiFn(lp[Phi], E, OneEs)) == double.MaxValue) throw new ProjectionException(20);
			        }
                    else
			        {
			            lp[Phi] = lp[Phi] < 0 ? -HalfPi : HalfPi;
			        }
                } else if (Math.Abs(lp[Phi] = (_c - lp[Phi] * lp[Phi]) / _n2) <= 1)
                {
                    lp[Phi] = Math.Asin(lp[Phi]);
                }
                else
                {
                    lp[Phi] = lp[Phi] < 0 ? -HalfPi : HalfPi;
                }
                lp[Lambda] = Math.Atan2(xy[X], xy[Y]) / _n;
	        }
            else
            {
		        lp[Lambda] = 0;
		        lp[Phi] = _n > 0? HalfPi : - HalfPi;
	        }
        }

   
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            _phi1 = projInfo.GetPhi1();
            _phi2 = projInfo.GetPhi2();
            Setup();
        }
        
        /// <summary>
        /// Internal code handling the setup operations for the transform
        /// </summary>
        protected void Setup()
        {
            double sinphi;
            if (Math.Abs(_phi1 + _phi2) < EPS10)
            {
                throw new ProjectionException(-21);
            }
	        _n = sinphi = Math.Sin(_phi1);
	        double cosphi = Math.Cos(_phi1);
	        bool secant = Math.Abs(_phi1 - _phi2) >= EPS10;
	        if(IsElliptical) 
            {
		        double m1 = Proj.Msfn(sinphi, cosphi, Es);
		        double ml1 = Proj.Qsfn(sinphi, E, OneEs);
		        if (secant) 
                { /* secant cone */
                    sinphi = Math.Sin(_phi2);
			        cosphi = Math.Cos(_phi2);
			        double m2 = Proj.Msfn(sinphi, cosphi, Es);
			        double ml2 = Proj.Qsfn(sinphi, E, OneEs);
			        _n = (m1 * m1 - m2 * m2) / (ml2 - ml1);
		        }
		        _ec = 1- .5 * OneEs * Math.Log((1- E) / (1+ E)) / E;
		        _c = m1 * m1 + _n * ml1;
		        _dd = 1/ _n;
		        _rho0 = _dd * Math.Sqrt(_c - _n * Proj.Qsfn(Math.Sin(Phi0), E, OneEs));
	        } 
            else
            {
		        if (secant) _n = .5 * (_n + Math.Sin(_phi2));
		        _n2 = _n + _n;
		        _c = cosphi * cosphi + _n2 * sinphi;
		        _dd = 1/ _n;
		        _rho0 = _dd * Math.Sqrt(_c - _n2 * Math.Sin(Phi0));
	        }

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Phi1 parameter
        /// </summary>
        protected double Phi1
        {
            get { return _phi1; }
            set { _phi1 = value; }
        }

        /// <summary>
        /// Gets or sets the Phi2 parameter
        /// </summary>
        protected double Phi2
        {
            get { return _phi2; }
            set { _phi2 = value; }
        }

        #endregion



    }
}
