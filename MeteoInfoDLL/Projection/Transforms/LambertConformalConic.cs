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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/29/2009 2:57:42 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// LambertConformalConic
    /// </summary>
    public class LambertConformalConic : Transform
    {
        #region Private Variables

        private double _phi1;
        private double _phi2;
        private double _n;
        private double _rho;
        private double _rho0;
        private double _c;
        private bool _ellipse;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Lambert Conformal Conic projection
        /// </summary>
        public LambertConformalConic()
        {
            Name = "Lambert_Conformal_Conic";
            Proj4Name = "lcc";
            ProjectionName = ProjectionNames.Lambert_Conformal;
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
            if(Math.Abs(Math.Abs(lp[Phi])- HalfPi) < EPS10)
            {
                if (lp[Phi] * _n <= 0)
                {
                    xy[X] = double.NaN;
                    xy[Y] = double.NaN;
                    //throw new ProjectionException(20);
                    return;
                }
                _rho = 0;
            }
            else
            {
                double x;
                if(_ellipse)
                {
                    x = Math.Pow(Proj.Tsfn(lp[Phi], Math.Sin(lp[Phi]), E), _n);
                }
                else
                {
                    x = Math.Pow(Math.Tan(Math.PI/4 + .5*lp[Phi]), -_n);
                }
                _rho = _c*x;
            }
            double nLam = lp[Lambda]*_n;
            xy[X] = K0*(_rho*Math.Sin(nLam));
            xy[Y] = K0*(_rho0 - _rho*Math.Cos(nLam));
        }
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            double x = xy[X]/K0;
            double y = _rho0 - xy[Y]/K0;

            _rho = Math.Sqrt(x*x + y*y);

            if (_rho != 0.0)
            {
                if (_n < 0)
                {
                    _rho = -_rho;
                    x = -x;
                    y = -y;
                }
                if (_ellipse)
                {
                    lp[Phi] = Proj.Phi2(Math.Pow(_rho / _c, 1 / _n), E);
                    if (lp[Phi] == double.MaxValue) throw new ProjectionException(20);
                }
                else
                {
                    lp[Phi] = 2 * Math.Atan(Math.Pow(_c / _rho, 1 / _n)) - HalfPi;
                }
                lp[Lambda] = Math.Atan2(x, y) / _n;
            }
            else
            {
                lp[Lambda] = 0;
                lp[Phi] = _n > 0 ? HalfPi : -HalfPi;
            }
        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double sinphi;
            double degreesToRadians = projInfo.GeographicInfo.Unit.Radians;
            _phi1 = projInfo.GetPhi1();
            if(projInfo.StandardParallel2 != null)
            {
                _phi2 = projInfo.GetPhi2();
            }
            else
            {
                _phi2 = _phi1;
                _phi1 = projInfo.GetPhi0();
            }
            if(Math.Abs(_phi1 + _phi2) < EPS10)throw new ProjectionException(21);
            _n = sinphi = Math.Sin(_phi1);
            double cosphi = Math.Cos(_phi1);
            bool secant = Math.Abs(_phi1 - _phi2) >= EPS10;
            _ellipse = projInfo.GeographicInfo.Datum.Spheroid.IsOblate();
            if(_ellipse)
            {
                double m1 = Proj.Msfn(sinphi, cosphi, Es);
                double ml1 = Proj.Tsfn(_phi1, sinphi, E);
                if(secant)
                {
                    sinphi = Math.Sin(_phi2);
                    _n = Math.Log(m1/ Proj.Msfn(sinphi, Math.Cos(_phi2), Es));
                    _n = _n/Math.Log(ml1/Proj.Tsfn(_phi2, sinphi, E));
                }
                _rho0 = m1*Math.Pow(ml1, -_n)/_n;
                _c = _rho0;
                if(Math.Abs(Math.Abs(Phi0) - HalfPi) < EPS10)
                {
                    _rho0 = 0;
                }
                else
                {
                    _rho0 *= Math.Pow(Proj.Tsfn(Phi0, Math.Sin(Phi0), E), _n);
                }
            }
            else
            {
                if(secant)
                {
                    _n = Math.Log(cosphi/Math.Cos(_phi2))/
                         Math.Log(Math.Tan(Math.PI/4 + .5*_phi2)/
                                  Math.Tan(Math.PI/4 + .5*_phi1));
                    _c = cosphi*Math.Pow(Math.Tan(Math.PI/4 + .5*_phi1), _n)/_n;
                }
                if (Math.Abs(Math.Abs(Phi0) - HalfPi) < EPS10)
                {
                    _rho0 = 0;
                }
                else
                {
                    _rho0 = _c*Math.Pow(Math.Tan(Math.PI/4 + .5*Phi0), -_n);
                }
            }

        }


        /// <summary>
        /// Special factor calculations for a factors calculation
        /// </summary>
        /// <param name="lp">lambda-phi</param>
        /// <param name="p">The projection</param>
        /// <param name="fac">The Factors</param>
        protected override void OnSpecial(double[] lp, ProjectionInfo p, Factors fac)
        {
            if (Math.Abs(Math.Abs(lp[Phi]) - HalfPi) < EPS10)
            {
                if ((lp[Phi]*_n) <= 0) return;
                _rho = 0;
            }
            else
            {
                _rho = _c *(_ellipse ? Math.Pow(Proj.Tsfn(lp[Phi], Math.Sin(lp[Phi]),
                                                 p.GeographicInfo.Datum.Spheroid.Eccentricity()), _n)
                                   : Math.Pow(Math.Tan(Math.PI/4 + .5*lp[Phi]), -_n));
            }
            fac.code = AnalyticModes.IsAnalHK | AnalyticModes.IsAnalConv;
	        fac.k = fac.h = p.ScaleFactor * _n * _rho / Proj.Msfn(Math.Sin(lp[Phi]), Math.Cos(lp[Phi]), p.GeographicInfo.Datum.Spheroid.EccentricitySquared());
	        fac.conv = - _n * lp[Lambda];
        }
        #endregion

        #region Properties



        #endregion

        
    }
}
