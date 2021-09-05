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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 4:39:45 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************



namespace MeteoInfoC.Projections
{


    /// <summary>
    /// PutinsP1
    /// </summary>
    public class PutinsP1 : Transform
    {
        #region Private Variables

        private const double Cx = 1.89490;
        private const double Cy = 0.94745;
        private const double Ca = -0.5;
        private const double Cb = 0.30396355092701331433;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PutinsP1
        /// </summary>
        public PutinsP1()
        {
            Proj4Name = "putp1";
            Name = "Putins_P1";
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
            xy[Y] = Cy * lp[Phi];
            xy[X] = Cx * lp[Lambda] * (Ca + Proj.Asqrt(1 - Cb * lp[Phi] * lp[Phi]));
        }
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            lp[Phi] = xy[Y] / Cy;
            lp[Lambda] = xy[X] / (Cx * (Ca + Proj.Asqrt(1 - Cb * lp[Phi] * lp[Phi])));
        }

        #endregion



    }
}
