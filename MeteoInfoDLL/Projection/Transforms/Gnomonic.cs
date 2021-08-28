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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 10:40:14 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Gnomonic
    /// </summary>
    public class Gnomonic : Transform
    {
        #region Private Variables

        private double _sinph0;
        private double _cosph0;
        private Modes _mode;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Gnomonic
        /// </summary>
        public Gnomonic()
        {
            Proj4Name = "gnom";
            Name = "Gnomonic";
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
            double sinphi = Math.Sin(lp[Phi]);
            double cosphi = Math.Cos(lp[Phi]);
            double coslam = Math.Cos(lp[Lambda]);
            switch (_mode)
            {
                case Modes.Equitorial:
                    xy[Y] = cosphi*coslam;
                    break;
                case Modes.Oblique:
                    xy[Y] = _sinph0*sinphi + _cosph0*cosphi*coslam;
                    break;
                case Modes.SouthPole:
                    xy[Y] = - sinphi;
                    break;
                case Modes.NorthPole:
                    xy[Y] = sinphi;
                    break;
            }
            if (xy[Y] <= EPS10) throw new ProjectionException(20);
            xy[X] = (xy[Y] = 1/xy[Y])*cosphi*Math.Sin(lp[Lambda]);
            switch (_mode)
            {
                case Modes.Equitorial:
                    xy[Y] *= sinphi;
                    break;
                case Modes.Oblique:
                    xy[Y] *= _cosph0*sinphi - _sinph0*cosphi*coslam;
                    break;
                case Modes.NorthPole:
                case Modes.SouthPole:
                    if (_mode == Modes.NorthPole) coslam = -coslam;
                    xy[Y] *= cosphi*coslam;
                    break;
            }
        }
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            double rh = Proj.Hypot(xy[X], xy[Y]);
            double sinz = Math.Sin(lp[Phi] = Math.Atan(rh));
            double cosz = Math.Sqrt(1 - sinz*sinz);
            if (Math.Abs(rh) <= EPS10)
            {
                lp[Phi] = Phi0;
                lp[Lambda] = 0;
            }
            else
            {
                switch (_mode)
                {
                    case Modes.Oblique:
                        lp[Phi] = cosz*_sinph0 + xy[Y]*sinz*_cosph0/rh;
                        if (Math.Abs(lp[Phi]) >= 1)
                            lp[Phi] = lp[Phi] > 0 ? HalfPi : - HalfPi;
                        else
                            lp[Phi] = Math.Asin(lp[Phi]);
                        xy[Y] = (cosz - _sinph0*Math.Sin(lp[Phi]))*rh;
                        xy[X] *= sinz*_cosph0;
                        break;
                    case Modes.Equitorial:
                        lp[Phi] = xy[Y]*sinz/rh;
                        if (Math.Abs(lp[Phi]) >= 1)
                            lp[Phi] = lp[Phi] > 0 ? HalfPi : - HalfPi;
                        else
                            lp[Phi] = Math.Asin(lp[Phi]);
                        xy[Y] = cosz*rh;
                        xy[X] *= sinz;
                        break;
                    case Modes.SouthPole:
                        lp[Phi] -= HalfPi;
                        break;
                    case Modes.NorthPole:
                        lp[Phi] = HalfPi - lp[Phi];
                        xy[Y] = -xy[Y];
                        break;
                }
                lp[Lambda] = Math.Atan2(xy[X], xy[Y]);
            }
        }



        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (Math.Abs(Math.Abs(Phi0) - HalfPi) < EPS10)
		        _mode = Phi0 < 0? Modes.SouthPole : Modes.NorthPole;
	        else if (Math.Abs(Phi0) < EPS10)
		        _mode = Modes.Equitorial;
	        else {
		        _mode = Modes.Oblique;
		        _sinph0 = Math.Sin(Phi0);
		        _cosph0 = Math.Cos(Phi0);
	        }
        }


        #endregion

        #region Properties



        #endregion



    }
}
