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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/23/2009 11:18:39 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// TransverseMercator
    /// </summary>
    public class TransverseMercator : EllipticalTransform
    {
        #region Private Variables

        private const double FC1 = 1.0;
        private const double FC2 = .5;
        private const double FC3 = .16666666666666666666;
        private const double FC4 = .08333333333333333333;
        private const double FC5 = .05;
        private const double FC6 = .03333333333333333333;
        private const double FC7 = .02380952380952380952;
        private const double FC8 = .01785714285714285714;

       

        private double _esp;
        private double _ml0;
        private double[] _en;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TransverseMercator
        /// </summary>
        public TransverseMercator()
        {
            Name = "Transverse_Mercator";
            Proj4Name = "tmerc";
            ProjectionName = ProjectionNames.Transverse_Mercator;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if(Es != 0)
            {
                _en = MeridionalDistance.GetEN(Es);
                if(_en == null)throw new ProjectionException(0);
                
                _ml0 = MeridionalDistance.MeridionalLength(Phi0, Math.Sin(Phi0), Math.Cos(Phi0), _en);
                _esp = Es/(1 - Es);
            }
            else
            {
                _esp = K0;
                _ml0 = .5*_esp;
            }
        }

       

       

        #endregion

    

        #region Private Methods

        /// <summary>
        /// The forward transform in the special case where there is no flattening of the spherical model of the earth.
        /// </summary>
        /// <param name="lp">The input lambda and phi geodetic values organized in an array</param>
        /// <param name="xy">The output x and y values organized in an array</param>
        protected override void SphericalForward(double[] lp, double[] xy)
        {
            double cosphi;

            /*
             * Fail if our longitude is more than 90 degrees from the 
             * central meridian since the results are essentially garbage. 
             * Is error -20 really an appropriate return value?
             * 
             *  http://trac.osgeo.org/proj/ticket/5
             */
            if( lp[Lambda] < -HalfPi || lp[Lambda] > HalfPi )
            {
                xy[X] = double.PositiveInfinity;
                xy[Y] = double.PositiveInfinity;
                throw new ProjectionException(14);
            }

	        double b = (cosphi = Math.Cos(lp[Phi])) * Math.Sin(lp[Lambda]);
	        if (Math.Abs(Math.Abs(b) - 1) <= EPS10) throw new ProjectionException(20);
	        xy[X] = _ml0 * Math.Log((1 + b) / (1 - b));
            if ((b = Math.Abs(xy[Y] = cosphi * Math.Cos(lp[Lambda]) / Math.Sqrt(1 - b * b))) >= 1)
            {
                if ((b - 1) > EPS10) throw new ProjectionException(20);
                xy[Y] = 0;
            }
            else
            {
                xy[Y] = Math.Acos(xy[Y]);
            }
            if (lp[Phi] < 0) xy[Y] = -xy[Y];
            xy[Y] = _esp *(xy[Y] - Phi0);
        }
        /// <summary>
        /// The forward transform where the spheroidal model of the earth has a flattening factor, 
        /// matching more closely with the oblique spheroid of the actual earth
        /// </summary>
        /// <param name="lp">The double values for geodetic lambda and phi organized into a one dimensional array</param>
        /// <param name="xy">The double values for linear x and y organized into a one dimensional array</param>
        protected override void EllipticalForward(double[] lp, double[] xy)
        {
            /*
             * Fail if our longitude is more than 90 degrees from the 
             * central meridian since the results are essentially garbage. 
             * Is error -20 really an appropriate return value?
             * 
             *  http://trac.osgeo.org/proj/ticket/5
             */
            if (lp[Lambda] < -HalfPi || lp[Lambda] > HalfPi)
            {
                xy[X] = double.NaN;
                xy[Y] = double.NaN;
                //xy[X] = double.PositiveInfinity;
                //xy[Y] = double.PositiveInfinity;
                //throw new ProjectionException(14);
            }
            else
            {
                double sinphi = Math.Sin(lp[Phi]);
                double cosphi = Math.Cos(lp[Phi]);
                double t = Math.Abs(cosphi) > 1E-10 ? sinphi / cosphi : 0;
                t *= t;
                double al = cosphi * lp[Lambda];
                double als = al * al;
                al /= Math.Sqrt(1 - Es * sinphi * sinphi);
                double n = _esp * cosphi * cosphi;
                xy[X] = K0 * al * (FC1 +
                        FC3 * als * (1 - t + n +
                        FC5 * als * (5 + t * (t - 18) + n * (14 - 58 * t) +
                        FC7 * als * (61 + t * (t * (179 - t) - 479)))));
                xy[Y] = K0 * (MeridionalDistance.MeridionalLength(lp[Phi], sinphi, cosphi, _en) - _ml0 +
                        sinphi * al * lp[Lambda] * FC2 * (1 +
                        FC4 * als * (5 - t + n * (9 + 4 * n) +
                        FC6 * als * (61 + t * (t - 58) + n * (270 - 330 * t) +
                        FC8 * als * (1385 + t * (t * (543 - t) - 3111))))));
            }
        }

        /// <summary>
        /// Performs the inverse transform from a single coordinate of linear units to the same coordinate in geodetic lambda and
        /// phi units in the special case where the shape of the earth is being approximated as a perfect sphere.
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void SphericalInverse(double[] xy, double[] lp)
        {
            double h, g;
            h = Math.Exp(xy[X] / _esp);
	        g = .5 * (h - 1 / h);
	        h = Math.Cos(Phi0 + xy[Y] / _esp);
	        lp[Phi] = Math.Asin(Math.Sqrt((1 - h * h) / (1 + g * g)));
	        if (xy[Y] < 0) lp[Phi] = -lp[Phi]; 
	        lp[Lambda] = ( g != 0 || h != 0) ? Math.Atan2(g, h) : 0;
        }
        /// <summary>
        /// Performs the inverse transfrom from a single coordinate of linear units to the same coordinate in geodetic units    
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void EllipticalInverse(double[] xy, double[] lp)
        {

            double arg = _ml0 + xy[Y] / K0;
            lp[Phi] = MeridionalDistance.AngularDistance(_ml0 + xy[Y] / K0, Es, _en);
	        if (Math.Abs(lp[Phi]) >= HalfPi) 
            {
		        lp[Phi] = xy[Y] < 0 ? -HalfPi : HalfPi;
		        lp[Lambda] = 0;
	        }
            else
            {
		        double sinphi = Math.Sin(lp[Phi]);
		        double cosphi = Math.Cos(lp[Phi]);
		        double t = Math.Abs(cosphi) > 1e-10 ? sinphi/cosphi : 0;
		        double n = _esp * cosphi * cosphi;
                double con;
                double d = xy[X] * Math.Sqrt(con = 1 - Es * sinphi * sinphi) / K0;
		        con *= t;
		        t *= t;
		        double ds = d * d;
		        lp[Phi] -= (con * ds / (1-Es)) * FC2 * (1 -
			        ds * FC4 * (5 + t * (3 - 9 *  n) + n * (1 - 4 * n) -
			        ds * FC6 * (61 + t * (90 - 252 * n +
				    45 * t) + 46 * n
		            - ds * FC8 * (1385 + t * (3633 + t * (4095 + 1574 * t))))));
		        lp[Lambda] = d * (FC1 -
			                 ds * FC3*( 1 + 2*t + n -
			                 ds * FC5*(5 + t*(28 + 24*t + 8*n) + 6*n
		                   - ds * FC7 * (61 + t * (662 + t * (1320 + 720 * t)))))) / cosphi;
	        }
        }

        
        #endregion

    }
}
