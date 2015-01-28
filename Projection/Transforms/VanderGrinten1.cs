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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 9:07:59 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// VanderGrinten1
    /// </summary>
    public class VanderGrinten1 : Transform
    {
        #region Private Variables

        private const double TOL = 1E-10;
        private const double THIRD = .33333333333333333333;
        private const double C227 = .07407407407407407407;
        private const double Pi43 = 4.18879020478639098458;
        private const double Pisq = 9.86960440108935861869;
        private const double Tpisq = 19.73920880217871723738;
        private const double Hpisq = 4.93480220054467930934;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of VanderGrinten1
        /// </summary>
        public VanderGrinten1()
        {
            Proj4Name = "vandg";
            Name = "Van_der_Grinten_I";
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
            double p2 = Math.Abs(lp[Phi]/HalfPi);
            if ((p2 - TOL) > 1) throw new ProjectionException(20);
            if (p2 > 1)
                p2 = 1;
            if (Math.Abs(lp[Phi]) <= TOL)
            {
                xy[X] = lp[Lambda];
                xy[Y] = 0;
            }
            else if (Math.Abs(lp[Lambda]) <= TOL || Math.Abs(p2 - 1) < TOL)
            {
                xy[X] = 0;
                xy[Y] = Math.PI*Math.Tan(.5*Math.Asin(p2));
                if (lp[Phi] < 0) xy[Y] = -xy[Y];
            }
            else
            {
                double al = .5*Math.Abs(Math.PI/lp[Lambda] - lp[Lambda]/Math.PI);
                double al2 = al*al;
                double g = Math.Sqrt(1 - p2*p2);
                g = g/(p2 + g - 1);
                double g2 = g*g;
                p2 = g*(2/p2 - 1);
                p2 = p2*p2;
                xy[X] = g - p2;
                g = p2 + al2;
                xy[X] = Math.PI*(al*xy[X] + Math.Sqrt(al2*xy[X]*xy[X] - g*(g2 - p2)))/g;
                if (lp[Lambda] < 0) xy[X] = -xy[X];
                xy[Y] = Math.Abs(xy[X]/Math.PI);
                xy[Y] = 1 - xy[Y]*(xy[Y] + 2*al);
                if (xy[Y] < -TOL) throw new ProjectionException(20);
                if (xy[Y] < 0) xy[Y] = 0;
                else xy[Y] = Math.Sqrt(xy[Y])*(lp[Phi] < 0 ? -Math.PI : Math.PI);
            }
        }
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            double t, ay;

            double x2 = xy[X]*xy[X];
            if ((ay = Math.Abs(xy[Y])) < TOL)
            {
                lp[Phi] = 0;
                t = x2*x2 + Tpisq*(x2 + Hpisq);
                lp[Lambda] = Math.Abs(xy[X]) <= TOL ? 0 : .5*(x2 - Pisq + Math.Sqrt(t))/xy[X];
                return;
            }
            double y2 = xy[Y]*xy[Y];
            double r = x2 + y2;
            double r2 = r*r;
            double c1 = - Math.PI*ay*(r + Pisq);
            double c3 = r2 + TwoPi*(ay*r + Math.PI*(y2 + Math.PI*(ay + HalfPi)));
            double c2 = c1 + Pisq*(r - 3*y2);
            double c0 = Math.PI*ay;
            c2 /= c3;
            double al = c1/c3 - THIRD*c2*c2;
            double m = 2*Math.Sqrt(-THIRD*al);
            double d = C227*c2*c2*c2 + (c0*c0 - THIRD*c2*c1)/c3;
            if (((t = Math.Abs(d = 3*d/(al*m))) - TOL) > 1)
                throw new ProjectionException(20);
            d = t > 1 ? (d > 0 ? 0 : Math.PI) : Math.Acos(d);
            lp[Phi] = Math.PI*(m*Math.Cos(d*THIRD + Pi43) - THIRD*c2);
            if (xy[Y] < 0) lp[Phi] = -lp[Phi];
            t = r2 + Pisq*(x2 - y2 + Hpisq);
            lp[Lambda] = Math.Abs(xy[X]) <= TOL ? 0 : .5*(r - Pisq + (t <= 0 ? 0 : Math.Sqrt(t)))/xy[X];
        }

        #endregion

       


    }
}
