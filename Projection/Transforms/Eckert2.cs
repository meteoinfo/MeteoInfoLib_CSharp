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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 4:23:39 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Eckert2
    /// </summary>
    public class Eckert2 : Transform
    {
        #region Private Variables

        private const double FXC = 0.46065886596178063902;
        private const double FYC = 1.44720250911653531871;
        private const double C13 = 0.33333333333333333333;
        private const double ONEEPS = 1.0000001;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Eckert2
        /// </summary>
        public Eckert2()
        {
            Proj4Name = "eck2";
            Name = "Eckert_II";
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
            xy[X] = FXC * lp[Lambda] * (xy[Y] = Math.Sqrt(4- 3* Math.Sin(Math.Abs(lp[Phi]))));
	        xy[Y] = FYC * (2- xy[Y]);
	        if ( lp[Phi] < 0) xy[Y] = -xy[Y];
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            lp[Lambda] = xy[X]/(FXC*(lp[Phi] = 2 - Math.Abs(xy[Y])/FYC));
            lp[Phi] = (4 - lp[Phi]*lp[Phi])*C13;
            if (Math.Abs(lp[Phi]) >= 1)
            {
                if (Math.Abs(lp[Phi]) > ONEEPS) throw new ProjectionException(20);
                lp[Phi] = lp[Phi] < 0 ? -HalfPi : HalfPi;
            }
            else
                lp[Phi] = Math.Asin(lp[Phi]);
            if (xy[Y] < 0)
                lp[Phi] = -lp[Phi];
        }
        #endregion

       



    }
}
