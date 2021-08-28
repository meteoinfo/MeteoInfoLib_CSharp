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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 5:02:20 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Eckert5
    /// </summary>
    public class Eckert5 : Transform
    {
        #region Private Variables

        private const double Xf = 0.44101277172455148219;
        private const double Rxf = 2.26750802723822639137;
        private const double Yf = 0.88202554344910296438;
        private const double Ryf = 1.13375401361911319568;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Eckert5
        /// </summary>
        public Eckert5()
        {
            Proj4Name = "eck5";
            Name = "Eckert_V";
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
            xy[X] = Xf * (1+ Math.Cos(lp[Phi])) * lp[Lambda];
	        xy[Y] = Yf * lp[Phi];
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            lp[Lambda] = Rxf * xy[X] / (1+ Math.Cos( lp[Phi] = Ryf * xy[Y]));
        }
        

        #endregion

      


    }
}
