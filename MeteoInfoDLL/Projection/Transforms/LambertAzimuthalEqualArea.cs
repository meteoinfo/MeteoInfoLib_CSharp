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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/10/2009 9:25:51 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// LambertAzimuthalEqualArea
    /// </summary>
    public class LambertAzimuthalEqualArea: EllipticalTransform
    {
       
        #region Private Variables

        private double _sinb1;
        private double _cosb1;
        private double _xmf;
        private double _ymf;
        private double _qp;
        private double _dd;
        private double _rq;
        private double[] _apa;
        private Modes _mode;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LambertAzimuthalEqualArea
        /// </summary>
        public LambertAzimuthalEqualArea()
        {
            Name = "Lambert_Azimuthal_Equal_Area";
            Proj4Name = "laea";
            ProjectionName = ProjectionNames.Lambert_Azimuthal_Equal_Area;
        }

        #endregion

        #region Methods


        /// <summary>
        /// The forward transform where the spheroidal model of the earth has a flattening factor, 
        /// matching more closely with the oblique spheroid of the actual earth
        /// </summary>
        /// <param name="lp">The double values for geodetic lambda and phi organized into a one dimensional array</param>
        /// <param name="xy">The double values for linear x and y organized into a one dimensional array</param>
        protected override void EllipticalForward(double[] lp, double[] xy)
        {
            double q, sinb = 0, cosb = 0, b = 0;
            double coslam = Math.Cos(lp[Lambda]);
            double sinlam = Math.Sin(lp[Lambda]);
            double sinphi = Math.Sin(lp[Phi]);
            q = Proj.Qsfn(sinphi, E, OneEs);
            if (_mode == Modes.Oblique || _mode == Modes.Equitorial)
            {
                sinb = q/_qp;
                cosb = Math.Sqrt(1 - sinb*sinb);
            }
            switch (_mode)
            {
                case Modes.Oblique:
                    b = 1 + _sinb1*sinb + _cosb1*cosb*coslam;
                    break;
                case Modes.Equitorial:
                    b = 1 + cosb*coslam;
                    break;
                case Modes.NorthPole:
                    b = HalfPi + lp[Phi];
                    q = _qp - q;
                    break;
                case Modes.SouthPole:
                    b = lp[Phi] - HalfPi;
                    q = _qp + q;
                    break;

            }
            if(Math.Abs(b) < EPS10) throw new ProjectionException(20);
            switch(_mode)
            {
                case Modes.Oblique:
                    xy[Y] = _ymf*(b = Math.Sqrt(2/b))*(_cosb1*sinb - _sinb1*cosb*coslam);
                    xy[X] = _xmf * b * cosb * sinlam;
                    break;
                case Modes.Equitorial:
                    xy[Y] = (b = Math.Sqrt(2/(1 + cosb*coslam)))*sinb*_ymf;
                    xy[X] = _xmf * b * cosb * sinlam;
                    break;
                case Modes.NorthPole:
                case Modes.SouthPole:
                    if (q >= 0)
                    {
                        xy[X] = (b = Math.Sqrt(q))*sinlam;
                        xy[Y] = coslam*(_mode == Modes.SouthPole ? b : -b);
                    }
                    else
                    {
                        xy[X] = xy[Y] = 0;
                    }
                    break;
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
            if (_mode == Modes.Equitorial || _mode == Modes.Oblique)
            {
                if (_mode == Modes.Equitorial) xy[X] = 1 + cosphi * coslam;
                if (_mode == Modes.Oblique) xy[Y] = 1 + _sinb1 * sinphi + _cosb1 * cosphi * coslam;
                if (xy[Y] <= EPS10) throw new ProjectionException(20);
                xy[X] = (xy[Y] = Math.Sqrt(2 / xy[Y])) * cosphi * Math.Sin(lp[Lambda]);
                xy[Y] *= _mode == Modes.Equitorial ? sinphi : _cosb1 * sinphi - _sinb1 * cosphi * coslam;
            }
            else
            {
                if (_mode == Modes.NorthPole) coslam = -coslam;
                if(Math.Abs(lp[Phi] + Phi0) < EPS10) throw new ProjectionException(20);
                xy[Y] = FortPi - lp[Phi]*.5;
                xy[Y] = 2*(_mode == Modes.SouthPole ? Math.Cos(xy[Y]) : Math.Sin(xy[Y]));
                xy[X] = xy[Y]*Math.Sin(lp[Lambda]);
                xy[Y] *= coslam;
            }
        }



        /// <summary>
        /// Performs the inverse transfrom from a single coordinate of linear units to the same coordinate in geodetic units    
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void EllipticalInverse(double[] xy, double[] lp)
        {
            double q;
            double ab;
            double x = xy[X];
            double y = xy[Y];
            if (_mode == Modes.Equitorial || _mode == Modes.Oblique)
            {
                double rho = Proj.Hypot(x /= _dd, y *= _dd);
                if(rho < EPS10)
                {
                    lp[Lambda] = 0;
                    lp[Phi] = Phi0;
                    return;
                }
                double sCe;
                double cCe = Math.Cos(sCe = 2*Math.Asin(.5*rho/_rq));
                x *= (sCe = Math.Sin(sCe));
                if(_mode == Modes.Oblique)
                {
                    q = _qp*(ab = cCe*_sinb1 + y*sCe*_cosb1/rho);
                    y = rho*_cosb1*cCe - y*_sinb1*sCe;
                }
                else
                {
                    q = _qp*(ab = y*sCe/rho);
                    y = rho*cCe;
                }
            }
            else
            {
                if (_mode == Modes.NorthPole) y = -y;
                if ((q = (x*x + y*y)) == 0)
                {
                    lp[Lambda] = 0;
                    lp[Phi] = Phi0;
                    return;
                }
                ab = 1 - q/_qp;
                if (_mode == Modes.SouthPole) ab = -ab;
            }
            lp[Lambda] = Math.Atan2(x, y);
            lp[Phi] = Proj.AuthLat(Math.Asin(ab), _apa);
        }

        /// <summary>
        /// Performs the inverse transform from a single coordinate of linear units to the same coordinate in geodetic lambda and
        /// phi units in the special case where the shape of the earth is being approximated as a perfect sphere.
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void SphericalInverse(double[] xy, double[] lp)
        {
            double cosz = 0, sinz = 0;
            double x = xy[X];
            double y = xy[Y];
            double rh = Proj.Hypot(x, y);
            if((lp[Phi] = rh * .5) > 1)throw new ProjectionException(20);
            lp[Phi] = 2*Math.Asin(lp[Phi]);
            if(_mode == Modes.Oblique || _mode == Modes.Equitorial)
            {
                sinz = Math.Sin(lp[Phi]);
                cosz = Math.Cos(lp[Phi]);
            }
            switch (_mode)
            {
                case Modes.Equitorial:
                    lp[Phi] = Math.Abs(rh) <= EPS10 ? 0 : Math.Asin(y*sinz/rh);
                    x *= sinz;
                    y = cosz*rh;
                    break;
                case Modes.Oblique:
                    lp[Phi] = Math.Abs(rh) <= EPS10 ? Phi0 : Math.Asin(cosz*_sinb1 + y*sinz*_sinb1/rh);
                    x *= sinz*_cosb1;
                    y = (cosz - Math.Sin(lp[Phi])*_sinb1)*rh;
                    break;
                case Modes.NorthPole:
                    y = -y;
                    lp[Phi] = HalfPi - lp[Phi];
                    break;
                case Modes.SouthPole:
                    lp[Phi] -= HalfPi;
                    break;
            }
            lp[Lambda] = (y == 0 && (_mode == Modes.Equitorial || _mode == Modes.Oblique)) ? 0 : Math.Atan2(x, y);
        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double t = Math.Abs(Phi0);
            if(Math.Abs(t - HalfPi) < EPS10)
            {
                _mode = Phi0 < 0 ? Modes.SouthPole : Modes.NorthPole;
            }
            else if(Math.Abs(t) < EPS10)
            {
                _mode = Modes.Equitorial;
            }
            else
            {
                _mode = Modes.Oblique;
            }
            if (Es == 0)
            {
                IsElliptical = false;
                _mode = Modes.Oblique;
                _sinb1 = Math.Sin(Phi0);
                _cosb1 = Math.Cos(Phi0);
                return;
            }
            IsElliptical = true;
            
            _qp = Proj.Qsfn(1, Es, OneEs);
           // _mmf = .5/(1 - Es);
            _apa = Proj.Authset(Es);
            switch(_mode)
            {
                case Modes.NorthPole:
                case Modes.SouthPole:
                    _dd = 1;
                    break;
                case Modes.Equitorial:
                    _dd = 1/(_rq = Math.Sqrt(.5*_qp));
                    _xmf = 1;
                    _ymf = .5*_qp;
                    break;
                case Modes.Oblique:
                    _rq = Math.Sqrt(.5*_qp);
                    double sinphi = Math.Sin(Phi0);
                    _sinb1 = Proj.Qsfn(sinphi, E, OneEs);
                    _cosb1 = Math.Sqrt(1 - _sinb1*_sinb1);
                    _dd = Math.Cos(Phi0)/(Math.Sqrt(1 - Es*sinphi*sinphi)*_rq*_cosb1);
                    _ymf = _xmf = _rq/_dd;
                    _xmf *= _dd;
                    break;     
            }

           
        }

        #endregion



    }
}
