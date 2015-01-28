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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/10/2009 4:24:11 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Orthographic
    /// </summary>
    public class Orthographic : Transform
    {
        #region Private Variables

      

        private double _sinph0;
        private double _cosph0;
        private Modes _mode;




        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Orthographic
        /// </summary>
        public Orthographic()
        {
            Name = "Orthographic";
            Proj4Name = "ortho";
            ProjectionName = ProjectionNames.Orthographic;
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
            double cosphi = Math.Cos(lp[Phi]);
            double coslam = Math.Cos(lp[Lambda]);
            switch (_mode)
            {
                case Modes.Equitorial:
                    if (cosphi * coslam < -EPS10)
                    {
                        xy[X] = double.NaN;
                        xy[Y] = double.NaN;                        
                        //ProjectionException(20);
                    }
                    else
                        xy[Y] = Math.Sin(lp[Phi]);
                    break;
                case Modes.Oblique:
                    double sinphi;
                    if (_sinph0 * (sinphi = Math.Sin(lp[Phi])) + _cosph0 * cosphi * coslam < -EPS10)
                    {
                        xy[X] = double.NaN;
                        xy[Y] = double.NaN;
                        //throw new ProjectionException(20);
                    }
                    else
                        xy[Y] = _cosph0 * sinphi - _sinph0 * cosphi * coslam;
                    break;
                default:
                    if(_mode == Modes.NorthPole)coslam = -coslam;
                    if (Math.Abs(lp[Phi] - Phi0) - EPS10 > HalfPi)
                    {
                        xy[X] = double.NaN;
                        xy[Y] = double.NaN;
                        //throw new ProjectionException(20);
                    }
                    else
                        xy[Y] = cosphi * coslam;
                    break;
            }
            if (xy[X] != double.NaN)
                xy[X] = cosphi * Math.Sin(lp[Lambda]);
        
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            double rh, sinc;
            double x = xy[X];
            double y = xy[Y];
            if((sinc = (rh = Proj.Hypot(x, y))) > 1)
            {
                if((sinc - 1) > EPS10) throw new ProjectionException(20);
                sinc = 1;
            }
            double cosc = Math.Sqrt(1 - sinc*sinc);
            if(Math.Abs(rh) <= EPS10)
            {
                lp[Phi] = Phi0;
                lp[Lambda] = 0;
            }
            else
            {
                switch (_mode)
                {
                    case Modes.NorthPole:
                        y = -y;
                        lp[Phi] = Math.Acos(sinc);
                        break;
                    case Modes.SouthPole:
                        lp[Phi] = -Math.Acos(sinc);
                        break;
                    case Modes.Equitorial:
                        lp[Phi] = y*sinc/rh;
                        x *= sinc;
                        y = cosc*rh;
                        if(Math.Abs(lp[Phi]) >= 1)
                        {
                            lp[Phi] = lp[Phi] < 0 ? -HalfPi : HalfPi;
                        }
                        else
                        {
                            lp[Phi] = Math.Asin(lp[Phi]);
                        }
                        break;
                    case Modes.Oblique:
                        lp[Phi] = cosc*_sinph0 + y*sinc*_cosph0/rh;
                        y = (cosc - _sinph0*lp[Phi])*rh;
                        x *= sinc*_cosph0;
                        if (Math.Abs(lp[Phi]) >= 1)
                        {
                            lp[Phi] = lp[Phi] < 0 ? -HalfPi : HalfPi;
                        }
                        else
                        {
                            lp[Phi] = Math.Asin(lp[Phi]);
                        }
                        break;

                }
                lp[Lambda] = (y == 0 && (_mode == Modes.Oblique || _mode == Modes.Equitorial))
                                 ? (x == 0 ? 0 : x < 0 ? -HalfPi : HalfPi)
                                 : Math.Atan2(x, y);
            }
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if(Math.Abs(Math.Abs(Phi0) - HalfPi) <= EPS10)
            {
                _mode = Phi0 < 0 ? Modes.SouthPole : Modes.NorthPole;
            }
            else if (Math.Abs(Phi0) > EPS10)
            {
                _mode = Modes.Oblique;
                _sinph0 = Math.Sin(Phi0);
                _cosph0 = Math.Cos(Phi0);
            }
            else
            {
                _mode = Modes.Equitorial;
            }
        }

        #endregion

        #region Properties



        #endregion



    }
}
