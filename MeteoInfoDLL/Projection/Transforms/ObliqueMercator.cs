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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/31/2009 2:35:46 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// OrthogonalMercator
    /// </summary>
    public class ObliqueMercator : Transform
    {
        #region Private Variables

        private const double Tol = 1E-7;
        private double _alpha;
        private double _lamc;
        private double _lam1;
        private double _phi1;
        private double _lam2;
        private double _phi2;
        private double _gamma;
        private double _al;
        private double _bl;
        private double _el;
        private double _singam;
        private double _cosgam;
        private double _sinrot;
        private double _cosrot;
        private double _u0;
        private bool _ellips;
        private bool _rot;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of OrthogonalMercator
        /// </summary>
        public ObliqueMercator()
        {
            Proj4Name = "omerc";
            Name = "Hotine_Oblique_Mercator";
            ProjectionName = ProjectionNames.Hotine_Oblique_Mercator;
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
            double ul, us;
            double vl = Math.Sin(_bl*lp[Lambda]);
            if(Math.Abs(Math.Abs(lp[Phi]) - Math.PI/2) <= EPS10 )
            {
                ul = lp[Phi] < 0 ? -_singam : _singam;
                us = _al*lp[Phi]/_bl;
            }
            else
            {
                double q = _el/(_ellips ? Math.Pow(Proj.Tsfn(lp[Phi], Math.Sin(lp[Phi]), E), _bl) : TSFN0(lp[Phi]));
                double s = .5*(q - 1/q);
                ul = 2*(s*_singam - vl*_cosgam)/(q + 1/q);
                double con = Math.Cos(_bl*lp[Lambda]);
                if(Math.Abs(con) >= Tol)
                {
                    us = _al*Math.Atan((s*_cosgam + vl*_singam)/con)/_bl;
                    if(con < 0) us += Math.PI*_al/_bl;
                }
                else
                {
                    us = _al*_bl*lp[Lambda];
                }
            }
            if(Math.Abs(Math.Abs(ul) - 1) <= EPS10)throw new ProjectionException(20);
            double vs = .5*_al*Math.Log((1 - ul)/(1 + ul))/_bl;
            us -= _u0;
            if (!_rot)
            {
                xy[X] = us;
                xy[Y] = vs;
            }
            else
            {
                xy[X] = vs*_cosrot + us*_sinrot;
                xy[Y] = us*_cosrot - vs*_sinrot;
            }
        }
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            double us, vs;
            if(!_rot)
            {
                us = xy[X];
                vs = xy[Y];
            }
            else
            {
                vs = xy[X]*_cosrot - xy[Y]*_sinrot;
                us = xy[Y]*_cosrot + xy[X]*_sinrot;
            }
            us += _u0;
            double q = Math.Exp(-_bl*vs/_al);
            double s = .5*(q - 1/q);
            double vl = Math.Sin(_bl*us/_al);
            double ul = 2*(vl*_cosgam + s*_singam)/(q + 1/q);
            if (Math.Abs(Math.Abs(ul) - 1)<EPS10)
            {
                lp[Lambda] = 0;
                lp[Phi] = ul < 0 ? -HalfPi : HalfPi;
            }
            else
            {
                lp[Phi] = _el/Math.Sqrt((1 + ul)/(1 - ul));
                if (_ellips)
                {
                    if((lp[Phi] = Proj.Phi2(Math.Pow(lp[Phi], 1/_bl), E)) == double.MaxValue)
                    {
                        throw new ProjectionException(20);
                    }
                }
                else
                {
                    lp[Phi] = HalfPi - 2*Math.Atan(lp[Phi]);
                }
                lp[Lambda] = -Math.Atan2((s * _cosgam - vl * _singam), Math.Cos(_bl * us / _al)) / _bl;
            }
        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double con;
            double f;
            double d;
            double toRadians = projInfo.GeographicInfo.Unit.Radians;
            _rot = projInfo.ParamI("no_rot") == 0;
            bool azi = projInfo.ParamD("alpha") != 0.0;
            if (azi)
            {
                _lamc = projInfo.ParamD("lonc")*toRadians;
                _alpha = projInfo.ParamD("alpha") *toRadians;
                if (Math.Abs(_alpha) < Tol ||
                    Math.Abs(Math.Abs(Phi0) - HalfPi) <= Tol ||
                    Math.Abs(Math.Abs(_alpha) - HalfPi) <= Tol)
                    throw new ProjectionException(32);
            }
            else
            {
                _lam1 = projInfo.GetLam1();
                _phi1 = projInfo.GetPhi1();
                _lam2 = projInfo.GetLam2();
                _phi2 = projInfo.GetPhi2();
                if(Math.Abs(_phi1 - _phi2) <= Tol ||
                    (con = Math.Abs(_phi1)) <= Tol ||
                    Math.Abs(con - HalfPi) <= Tol ||
                    Math.Abs(Math.Abs(Phi0) - HalfPi) <= Tol ||
                    Math.Abs(Math.Abs(_phi2) - HalfPi) <= Tol)
                {
                    throw new ProjectionException(33);
                }
            }
            _ellips = Es > 0;
            double com = _ellips ? Math.Sqrt(OneEs) : 1;
            if (Math.Abs(Phi0) > EPS10)
            {
                double sinph0 = Math.Sin(Phi0);
                double cosph0 = Math.Cos(Phi0);
                if (_ellips)
                {
                    con = 1 - Es*sinph0*sinph0;
                    _bl = cosph0*cosph0;
                    _bl = Math.Sqrt(1 + Es*_bl*_bl/OneEs);
                    _al = _bl*K0*com/con;
                    d = _bl*com/(cosph0*Math.Sqrt(con));
                }
                else
                {
                    _bl = 1;
                    _al = K0;
                    d = 1/cosph0;
                }


                if ((f = d*d - 1) <= 0)
                {
                    f = 0;
                }
                else
                {
                    f = Math.Sqrt(f);
                    if (Phi0 < 0) f = -f;
                }
                _el = f += d;
                if (_ellips)
                {
                    _el *= Math.Pow(Proj.Tsfn(Phi0, sinph0, E), _bl);
                }
                else
                {
                    _el *= TSFN0(Phi0);
                }
            }
            else
            {
                _bl = 1/com;
                _al = K0;
                _el = d = f = 1;
            }
            if(azi)
            {
                _gamma = Math.Asin(Math.Sin(_alpha)/d);
                Lam0 = _lamc - Math.Asin((.5*(f - 1/f))*Math.Tan(_gamma))/_bl;
            }
            else
            {
                double h;
                double l;
                if(_ellips)
                {
                    h = Math.Pow(Proj.Tsfn(_phi1, Math.Sin(_phi1), E), _bl);
                    l = Math.Pow(Proj.Tsfn(_phi2, Math.Sin(_phi2), E), _bl);
                }
                else
                {
                    h = TSFN0(_phi1);
                    l = TSFN0(_phi2);
                }
                f = _el/h;
                double p = (l - h)/(l + h);
                double j = _el*_el;
                j = (j - l*h)/(j + l*h);
                if ((con = _lam1 - _lam2) < -Math.PI)
                {
                    _lam2 -= Math.PI*2;
                }
                else if (con > Math.PI)
                {
                    _lam2 += Math.PI*2;
                }
                Lam0 = Proj.Adjlon(.5*(_lam1 + _lam2) - Math.Atan(j*Math.Tan(.5*_bl*(_lam1 - _lam2))/p)/_bl);
                _gamma = Math.Atan(2*Math.Sin(_bl*Proj.Adjlon(_lam1 - Lam0))/(f - 1/f));
                _alpha = Math.Asin(d*Math.Sin(_gamma));
            }
            _singam = Math.Sin(_gamma);
            _cosgam = Math.Cos(_gamma);
            if(projInfo.ParamI("rot_conv") != 0)
            {
                f = _gamma;
            }
            else
            {
                f = _alpha;
            }
            _sinrot = Math.Sin(f);
            _cosrot = Math.Cos(f);
            if(projInfo.ParamI("no_uoff") != 0)
            {
                _u0 = 0;
            }
            else
            {
                _u0 = Math.Abs(_al*Math.Atan(Math.Sqrt(d*d - 1)/_cosrot)/_bl);
               
            }
            if (Phi0 < 0) _u0 = -_u0;
        }

        #endregion

        #region Properties



        #endregion

        #region private methods


        private double TSFN0(double x)
        {
            return Math.Tan(.5*(HalfPi - x));
        }

        #endregion

    }
}
