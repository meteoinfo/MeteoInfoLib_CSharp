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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 8:52:12 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// TwoPointEquidistant
    /// </summary>
    public class TwoPointEquidistant : Transform
    {
        #region Private Variables

        private double _cp1;
        private double _sp1;
        private double _cp2;
        private double _sp2;
        private double _ccs;
        private double _cs;
        private double _sc;
        private double _r2z0;
        private double _z02;
        private double _dlam2;
        private double _hz0;
        private double _thz0;
        private double _rhshz0;
        private double _ca;
        private double _sa;
        private double _lp;
        private double _lamc;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TwoPointEquidistant
        /// </summary>
        public TwoPointEquidistant()
        {
            Proj4Name = "tpeqd";
            Name = "Two_Point_Equidistant";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double pp;

            /* get control point locations */
            double phi1 = projInfo.GetPhi1();
            double lam1 = projInfo.GetLam1();
            double phi2 = projInfo.GetPhi2();
            double lam2 = projInfo.GetLam2();
            if (phi1 == phi2 && lam1 == lam2) throw new ProjectionException(-25);
            Lam0 = Proj.Adjlon(0.5*(lam1 + lam2));
            _dlam2 = Proj.Adjlon(lam2 - lam1);
            _cp1 = Math.Cos(phi1);
            _cp2 = Math.Cos(phi2);
            _sp1 = Math.Sin(phi1);
            _sp2 = Math.Sin(phi2);
            _cs = _cp1*_sp2;
            _sc = _sp1*_cp2;
            _ccs = _cp1*_cp2*Math.Sin(_dlam2);
            _z02 = Proj.Aacos(_sp1*_sp2 + _cp1*_cp2*Math.Cos(_dlam2));
            _hz0 = .5*_z02;
            double A12 = Math.Atan2(_cp2*Math.Sin(_dlam2),
                                    _cp1*_sp2 - _sp1*_cp2*Math.Cos(_dlam2));
            _ca = Math.Cos(pp = Proj.Aasin(_cp1*Math.Sin(A12)));
            _sa = Math.Sin(pp);
            _lp = Proj.Adjlon(Math.Atan2(_cp1*Math.Cos(A12), _sp1) - _hz0);
            _dlam2 *= .5;
            _lamc = HalfPi - Math.Atan2(Math.Sin(A12)*_sp1, Math.Cos(A12)) - _dlam2;
            _thz0 = Math.Tan(_hz0);
            _rhshz0 = .5/Math.Sin(_hz0);
            _r2z0 = 0.5/_z02;
            _z02 *= _z02;
        }

        /// <summary>
        /// The forward transform from geodetic units to linear units
        /// </summary>
        /// <param name="lp">The array of lambda, phi coordinates</param>
        /// <param name="xy">The array of x, y coordinates</param>
        protected override void OnForward(double[] lp, double[] xy)
        {
            double t, dl1, dl2;
	        double sp = Math.Sin(lp[Phi]);
	        double cp = Math.Cos(lp[Phi]);
	        double z1 = Proj.Aacos(_sp1 * sp + _cp1 * cp * Math.Cos(dl1 = lp[Lambda] + _dlam2));
	        double z2 = Proj.Aacos(_sp2 * sp + _cp2 * cp * Math.Cos(dl2 = lp[Lambda] - _dlam2));
	        z1 *= z1;
	        z2 *= z2;
	        xy[X] = _r2z0 * (t = z1 - z2);
	        t = _z02 - t;
	        xy[Y] = _r2z0 * Proj.Asqrt(4* _z02 * z2 - t * t);
	        if ((_ccs * sp - cp * (_cs * Math.Sin(dl1) - _sc * Math.Sin(dl2))) < 0)
		    xy[Y] = -xy[Y];
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            double cz1 = Math.Cos(Proj.Hypot(xy[Y], xy[X] + _hz0));
	        double cz2 = Math.Cos(Proj.Hypot(xy[Y], xy[X] - _hz0));
	        double s = cz1 + cz2;
	        double d = cz1 - cz2;
	        lp[Lambda] = - Math.Atan2(d, (s * _thz0));
	        lp[Phi] = Proj.Aacos(Proj.Hypot(_thz0 * s, d) * _rhshz0);
	        if ( xy[Y] < 0)
		        lp[Phi] = - lp[Phi];
	        /* lam--phi now in system relative to P1--P2 base equator */
	        double sp = Math.Sin(lp[Phi]);
	        double cp = Math.Cos(lp[Phi]);
	        lp[Phi] = Proj.Aasin(_sa * sp + _ca * cp * (s = Math.Cos(lp[Lambda] -= _lp)));
	        lp[Lambda] = Math.Atan2(cp * Math.Sin(lp[Lambda]), _sa * cp * s - _ca * sp) + _lamc;
        }

        #endregion

       


    }
}
