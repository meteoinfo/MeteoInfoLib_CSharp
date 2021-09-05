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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 11:39:36 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// AzimuthalEquidistant
    /// </summary>
    public class AzimuthalEquidistant : EllipticalTransform
    {


        #region Private Variables


        private double _sinph0;
        private double _cosph0;
        private double[] _en;
        private double _M1;
        private double _N1;
        private double _Mp;
        private double _He;
        private double _g;
        private Modes _mode;
        private bool _isGuam;

        private const double TOL = 1E-14;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AzimuthalEquidistant
        /// </summary>
        public AzimuthalEquidistant() 
        {
            Proj4Name = "aeqd";
            Name = "Azimuthal_Equidistant";
        }

        #endregion

        #region Methods
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            Phi0 = projInfo.GetPhi0();
	        if (Math.Abs(Math.Abs(Phi0) - HalfPi) < EPS10) 
            {
		        _mode = Phi0 < 0 ? Modes.SouthPole : Modes.NorthPole;
		        _sinph0 = Phi0 < 0 ? -1: 1;
		        _cosph0 = 0;
	        } 
            else if (Math.Abs(Phi0) < EPS10)
            {
		        _mode = Modes.Equitorial;
		        _sinph0 = 0;
		        _cosph0 = 1;
	        } 
            else 
            {
		        _mode = Modes.Oblique;
		        _sinph0 = Math.Sin(Phi0);
		        _cosph0 = Math.Cos(Phi0);
	        }
	        if (Es == 0)return;
            _en = Proj.Enfn(Es);
            if(projInfo.Parameters.ContainsKey("guam"))
            {
			    _M1 = Proj.Mlfn(Phi0, _sinph0, _cosph0, _en);
			    _isGuam = true;
    		} 
            else 
            {
			    switch (_mode)
                {
			        case Modes.NorthPole:
				        _Mp = Proj.Mlfn(HalfPi, 1, 0, _en);
				        break;
			        case Modes.SouthPole:
				        _Mp = Proj.Mlfn(-HalfPi, -1, 0, _en);
				        break;
			        case Modes.Equitorial:
			        case Modes.Oblique:
				        _N1 = 1/ Math.Sqrt(1- Es * _sinph0 * _sinph0);
				        _g = _sinph0 * (_He = E / Math.Sqrt(OneEs));
				        _He *= _cosph0;
				        break;
			    }
	        }
        }

        /// <summary>
        /// The forward transform in the special case where there is no flattening of the spherical model of the earth.
        /// </summary>
        /// <param name="lp">The input lambda and phi geodetic values organized in an array</param>
        /// <param name="xy">The output x and y values organized in an array</param>
        protected override void SphericalForward(double[] lp, double[] xy)
        {
            double sinphi = Math.Sin(lp[Phi]);
	        double cosphi = Math.Cos(lp[Phi]);
	        double coslam = Math.Cos(lp[Lambda]);
	        switch (_mode) 
            {
	            case Modes.Equitorial:
                case Modes.Oblique:
                    if(_mode == Modes.Equitorial)
                    {
                        xy[Y] = cosphi * coslam;
                    }
                    else
                    {
                        xy[Y] = _sinph0 * sinphi + _cosph0 * cosphi * coslam;
                    }
		            if (Math.Abs(Math.Abs(xy[Y]) - 1) < TOL)
		            {
		                if (xy[Y] < 0)throw new ProjectionException(20);
		                xy[X] = xy[Y] = 0;
		            }
		            else 
                    {
			            xy[Y] = Math.Acos(xy[Y]);
			            xy[Y] /= Math.Sin(xy[Y]);
			            xy[X] = xy[Y] * cosphi * Math.Sin(lp[Lambda]);
			            xy[Y] *= (_mode == Modes.Equitorial) ? sinphi :
		   		        _cosph0 * sinphi - _sinph0 * cosphi * coslam;
		            }
		            break;
	            case Modes.NorthPole:
	            case Modes.SouthPole:
                    if(_mode == Modes.NorthPole)
                    {
                        lp[Phi] = -lp[Phi];
                        coslam = -coslam;
                    }
		        if (Math.Abs(lp[Phi] - HalfPi) < EPS10) throw new ProjectionException(20);
		        xy[X] = (xy[Y] = (HalfPi + lp[Phi])) * Math.Sin(lp[Lambda]);
		        xy[Y] *= coslam;
		        break;
	        }
        }
        
        /// <summary>
        /// The forward transform where the spheroidal model of the earth has a flattening factor, 
        /// matching more closely with the oblique spheroid of the actual earth
        /// </summary>
        /// <param name="lp">The double values for geodetic lambda and phi organized into a one dimensional array</param>
        /// <param name="xy">The double values for linear x and y organized into a one dimensional array</param>
        protected override void EllipticalForward(double[] lp, double[] xy)
        {
            if (_isGuam)
            {
                GuamForward(lp, xy);
                return;
            }

            double coslam = Math.Cos(lp[Lambda]);
	        double cosphi = Math.Cos(lp[Phi]);
	        double sinphi = Math.Sin(lp[Phi]);
	        switch (_mode) 
            {
                case Modes.NorthPole:
	            case Modes.SouthPole:
                    if (_mode == Modes.NorthPole) coslam = -coslam;
                    double rho;
                    xy[X] = (rho = Math.Abs(_Mp - Proj.Mlfn(lp[Phi], sinphi, cosphi, _en))) * Math.Sin(lp[Lambda]);
		            xy[Y] = rho * coslam;
		            break;
	            case Modes.Equitorial:
	            case Modes.Oblique:
		            if (Math.Abs(lp[Lambda]) < EPS10 && Math.Abs(lp[Phi] - Phi0) < EPS10) 
                    {
			            xy[X] = xy[Y] = 0;
			            break;
		            }   
		            double t = Math.Atan2(OneEs * sinphi + Es * _N1 * _sinph0 *
		                                                   Math.Sqrt(1- Es * sinphi * sinphi), cosphi);
		            double ct = Math.Cos(t); double st = Math.Sin(t);
		            double az = Math.Atan2(Math.Sin(lp[Lambda]) * ct, _cosph0 * st - _sinph0 * coslam * ct);
		            double cA = Math.Cos(az); double sA = Math.Sin(az);
		            double s = Math.Asin( Math.Abs(sA) < TOL ?
		                                                         (_cosph0 * st - _sinph0 * coslam * ct) / cA :
		                                                                                                         Math.Sin(lp[Lambda]) * ct / sA );
		            double h = _He * cA;
		            double h2 = h * h;
		            double c = _N1 * s * (1+ s * s * (- h2 * (1- h2)/6+
		                                              s * ( _g * h * (1- 2* h2 * h2) / 8+
		                                                    s * ((h2 * (4- 7* h2) - 3* _g * _g * (1- 7* h2)) /
		                                                         120- s * _g * h / 48))));
		            xy[X] = c * sA;
		            xy[Y] = c * cA;
		            break;
	        } 
        }

        private void GuamForward(double[] lp, double[] xy)
        {
            double cosphi = Math.Cos(lp[Phi]);
	        double sinphi = Math.Sin(lp[Phi]);
	        double t = 1/ Math.Sqrt(1- Es * sinphi * sinphi);
	        xy[X] = lp[Lambda] * cosphi * t;
	        xy[Y] = Proj.Mlfn(lp[Phi], sinphi, cosphi, _en) - _M1 +
		        .5 * lp[Lambda] * lp[Lambda] * cosphi * sinphi * t;
        }


        /// <summary>
        /// Performs the inverse transform from a single coordinate of linear units to the same coordinate in geodetic lambda and
        /// phi units in the special case where the shape of the earth is being approximated as a perfect sphere.
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void SphericalInverse(double[] xy, double[] lp)
        {
            double cRh;

            if ((cRh = Proj.Hypot(xy[X], xy[Y])) > Math.PI)
            {
                if (cRh - EPS10 > Math.PI) throw new ProjectionException(20);
                cRh = Math.PI;
            }
            else if (cRh < EPS10)
            {
                lp[Phi] = Phi0;
                lp[Lambda] = 0;
                return;
            }
            if (_mode == Modes.Oblique || _mode == Modes.Equitorial)
            {
                double sinc = Math.Sin(cRh);
                double cosc = Math.Cos(cRh);
                if (_mode == Modes.Equitorial)
                {
                    lp[Phi] = Proj.Aasin(xy[Y]*sinc/cRh);
                    xy[X] *= sinc;
                    xy[Y] = cosc*cRh;
                }
                else
                {
                    lp[Phi] = Proj.Aasin(cosc*_sinph0 + xy[Y]*sinc*_cosph0/
                                                      cRh);
                    xy[Y] = (cosc - _sinph0*Math.Sin(lp[Phi]))*cRh;
                    xy[X] *= sinc*_cosph0;
                }
                lp[Lambda] = xy[Y] == 0 ? 0 : Math.Atan2(xy[X], xy[Y]);
            }
            else if (_mode == Modes.NorthPole)
            {
                lp[Phi] = HalfPi - cRh;
                lp[Lambda] = Math.Atan2(xy[X], -xy[Y]);
            }
            else
            {
                lp[Phi] = cRh - HalfPi;
                lp[Lambda] = Math.Atan2(xy[X], xy[Y]);
            }
        }

        /// <summary>
        /// Performs the inverse transfrom from a single coordinate of linear units to the same coordinate in geodetic units    
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void EllipticalInverse(double[] xy, double[] lp)
        {
            if(_isGuam)
            {
                GuamInverse(xy, lp);
            }

            double c;

            if ((c = Proj.Hypot(xy[X], xy[Y])) < EPS10)
            {
                lp[Phi] = Phi0;
                lp[Lambda] = 0;
                return;
            }
            if (_mode == Modes.Oblique || _mode == Modes.Equitorial)
            {
                double az;
                double cosAz = Math.Cos(az = Math.Atan2(xy[X], xy[Y]));
                double t = _cosph0*cosAz;
                double b = Es*t/OneEs;
                double a = - b*t;
                b *= 3*(1 - a)*_sinph0;
                double d = c/_N1;
                double e = d*(1 - d*d*(a*(1 + a)/6 + b*(1 + 3*a)*d/24));
                double f = 1 - e*e*(a/2 + b*e/6);
                double psi = Proj.Aasin(_sinph0*Math.Cos(e) + t*Math.Sin(e));
                lp[Lambda] = Proj.Aasin(Math.Sin(az)*Math.Sin(e)/Math.Cos(psi));
                if ((t = Math.Abs(psi)) < EPS10)
                    lp[Phi] = 0;
                else if (Math.Abs(t - HalfPi) < 0)
                    lp[Phi] = HalfPi;
                else
                {
                    lp[Phi] = Math.Atan((1 - Es*f*_sinph0/Math.Sin(psi))*Math.Tan(psi)/
                                        OneEs);
                }
            }
            else
            {
                /* Polar */
                lp[Phi] = Proj.InvMlfn(_mode == Modes.NorthPole ? _Mp - c : _Mp + c,
                                     Es, _en);
                lp[Lambda] = Math.Atan2(xy[X], _mode == Modes.NorthPole ? -xy[Y] : xy[Y]);
            }

        }

        private void GuamInverse(double[] xy, double[] lp)
        {
            double t = 0;
            int i;

            double x2 = 0.5*xy[X]*xy[X];
            lp[Phi] = Phi0;
            for (i = 0; i < 3; ++i)
            {
                t = E*Math.Sin(lp[Phi]);
                lp[Phi] = Proj.InvMlfn(_M1 + xy[Y] -
                                     x2*Math.Tan(lp[Phi])*(t = Math.Sqrt(1 - t*t)), Es, _en);
            }
            lp[Lambda] = xy[X]*t/Math.Cos(lp[Phi]);
        }

        #endregion

        #region Properties



        #endregion



    }
}
