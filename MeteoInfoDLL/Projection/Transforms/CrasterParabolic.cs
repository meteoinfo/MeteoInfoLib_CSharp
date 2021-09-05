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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 4:01:45 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// CrasterParabolic
    /// </summary>
    public class CrasterParabolic : Transform
    {
        #region Private Variables

        private const double Xm = 0.97720502380583984317;
        private const double Rxm = 1.02332670794648848847;
        private const double Ym = 3.06998012383946546542;
        private const double Rym = 0.32573500793527994772;
        private const double Third = 0.333333333333333333;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CrasterParabolic
        /// </summary>
        public CrasterParabolic()
        {
            Proj4Name = "crast";
            Name = "Craster_Parabolic";
           
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
            lp[Phi] *= Third;
	        xy[X] = Xm * lp[Lambda] * (2* Math.Cos(lp[Phi] + lp[Phi]) - 1);
	        xy[Y] = Ym * Math.Sin(lp[Phi]);
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            lp[Phi] = 3* Math.Asin(xy[Y] * Rym);
	        lp[Lambda] = xy[X] * Rxm / (2* Math.Cos((lp[Phi] + lp[Phi]) * Third) - 1);
        }

        #endregion

       


    }
}
