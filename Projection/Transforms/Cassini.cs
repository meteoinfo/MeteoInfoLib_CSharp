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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 3:20:34 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Cassini
    /// </summary>
    public class Cassini : EllipticalTransform
    {
        #region Private Variables

        private double _m0;
        private double _n;
        private double _t;
        private double _a1;
        private double _c;
        private double _r;
        private double _dd;
        private double _d2;
        private double _a2;
        private double _tn;
        private double[] _en;

        private const double C1 = .16666666666666666666;
        private const double C2 = .00833333333333333333;
        private const double C3 = .04166666666666666666;
        private const double C4 = .33333333333333333333;
        private const double C5 = .06666666666666666666;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Cassini
        /// </summary>
        public Cassini()
        {
            Proj4Name = "cass";
            Name = "Cassini";
        }

        #endregion

        #region Methods

        /// <summary>
        /// The forward transform in the special case where there is no flattening of the spherical model of the earth.
        /// </summary>
        /// <param name="lp">The input lambda and phi geodetic values organized in an array</param>
        /// <param name="xy">The output x and y values organized in an array</param>
        protected override void SphericalForward(double[] lp, double[] xy)
        {
            xy[X] = Math.Asin(Math.Cos(lp[Phi]) * Math.Sin(lp[Lambda]));
            xy[Y] = Math.Atan2(Math.Tan(lp[Phi]), Math.Cos(lp[Lambda])) - Phi0;
        }
        /// <summary>
        /// The forward transform where the spheroidal model of the earth has a flattening factor, 
        /// matching more closely with the oblique spheroid of the actual earth
        /// </summary>
        /// <param name="lp">The double values for geodetic lambda and phi organized into a one dimensional array</param>
        /// <param name="xy">The double values for linear x and y organized into a one dimensional array</param>
        protected override void EllipticalForward(double[] lp, double[] xy)
        {
            xy[Y] = Proj.Mlfn(lp[Phi], _n = Math.Sin(lp[Phi]), _c = Math.Cos(lp[Phi]), _en);
	        _n = 1/Math.Sqrt(1- Es * _n * _n);
	        _tn = Math.Tan(lp[Phi]); _t = _tn * _tn;
	        _a1 = lp[Lambda] * _c;
	        _c *= Es * _c / (1 - Es);
	        _a2 = _a1 * _a1;
	        xy[X] = _n * _a1 * (1- _a2 * _t *
		        (C1 - (8- _t + 8* _c) * _a2 * C2));
	        xy[Y] -= _m0 - _n * _tn * _a2 *
		        (.5 + (5- _t + 6* _c) * _a2 * C3);
        }

        /// <summary>
        /// Performs the inverse transfrom from a single coordinate of linear units to the same coordinate in geodetic units    
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void EllipticalInverse(double[] xy, double[] lp)
        {
            double ph1 = Proj.InvMlfn(_m0 + xy[Y], Es, _en);
	        _tn = Math.Tan(ph1); _t = _tn * _tn;
	        _n = Math.Sin(ph1);
	        _r = 1/ (1- Es * _n * _n);
	        _n = Math.Sqrt(_r);
	        _r *= (1- Es) * _n;
	        _dd = xy[X] / _n;
	        _d2 = _dd * _dd;
	        lp[Phi] = ph1 - (_n * _tn / _r) * _d2 *
		        (.5 - (1+ 3* _t) * _d2 * C3);
	        lp[Lambda] = _dd * (1+ _t * _d2 *
		        (-C4 + (1+ 3* _t) * _d2 * C5)) / Math.Cos(ph1);
        }
        /// <summary>
        /// Performs the inverse transform from a single coordinate of linear units to the same coordinate in geodetic lambda and
        /// phi units in the special case where the shape of the earth is being approximated as a perfect sphere.
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void SphericalInverse(double[] xy, double[] lp)
        {
            lp[Phi] = Math.Asin(Math.Sin(_dd = xy[Y] + Phi0) * Math.Cos(xy[X]));
            lp[Lambda] = Math.Atan2(Math.Tan(xy[X]), Math.Cos(_dd));
        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (IsElliptical)
            {
                _en = Proj.Enfn(Es);
                _m0 = Proj.Mlfn(Phi0, Math.Sin(Phi0), Math.Cos(Phi0), _en);
            }
           
        }

        #endregion




    }
}
