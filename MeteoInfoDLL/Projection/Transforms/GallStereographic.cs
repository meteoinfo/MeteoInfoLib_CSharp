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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 10:09:22 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// GallStereographic
    /// </summary>
    public class GallStereographic : Transform
    {
        #region Private Variables

        private const double Yf = 1.70710678118654752440;
        private const double Xf = 0.70710678118654752440;
        private const double Ryf = 0.58578643762690495119;
        private const double Rxf = 1.41421356237309504880;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GallStereographic
        /// </summary>
        public GallStereographic()
        {
            Proj4Name = "gall";
            Name = "Gall_Stereographic";
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
            xy[X] = Xf * lp[Lambda];
            xy[Y] = Yf * Math.Tan(.5 * lp[Phi]);
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            lp[Lambda] = Rxf * xy[X];
	        lp[Phi] = 2* Math.Atan(xy[Y] * Ryf);
        }

        #endregion

        


    }
}
