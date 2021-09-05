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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 4:56:10 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Eckert4
    /// </summary>
    public class Eckert4 : Transform
    {
        #region Private Variables

        private const double Cx = .42223820031577120149;
        private const double Cy = 1.32650042817700232218;
        private const double CP = 3.57079632679489661922;
        private const double EPS = 1E-7;
        private const int Niter = 6;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Eckert4
        /// </summary>
        public Eckert4()
        {
            Proj4Name = "eck4";
            Name = "Eckert_IV";
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
            int i;

            double p = CP*Math.Sin(lp[Phi]);
            double V = lp[Phi]*lp[Phi];
            lp[Phi] *= 0.895168 + V*(0.0218849 + V*0.00826809);
            for (i = Niter; i > 0; --i)
            {
                double c = Math.Cos(lp[Phi]);
                double s = Math.Sin(lp[Phi]);
                lp[Phi] -= V = (lp[Phi] + s*(c + 2) - p)/
                               (1 + c*(c + 2) - s*s);
                if (Math.Abs(V) < EPS)
                    break;
            }
            if (i == 0)
            {
                xy[X] = Cx*lp[Lambda];
                xy[Y] = lp[Phi] < 0 ? -Cy : Cy;
            }
            else
            {
                xy[X] = Cx*lp[Lambda]*(1 + Math.Cos(lp[Phi]));
                xy[Y] = Cy*Math.Sin(lp[Phi]);
            }
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            double c;
	        lp[Phi] = Proj.Aasin(xy[Y] / Cy);
	        lp[Lambda] = xy[X] / (Cx * (1+ (c = Math.Cos(lp[Phi]))));
	        lp[Phi] = Proj.Aasin((lp[Phi] + Math.Sin(lp[Phi]) * (c + 2)) / CP);
        }

        #endregion

    }
}
