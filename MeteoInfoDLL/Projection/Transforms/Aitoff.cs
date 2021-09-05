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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 2:17:40 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Aitoff
    /// </summary>
    public class Aitoff : Transform
    {
        #region Private Variables

        
       

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Aitoff
        /// </summary>
        public Aitoff()
        {
            Proj4Name = "aitoff";
            Name = "Aitoff";
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
            double c, d;

            if ((d = Math.Acos(Math.Cos(lp[Phi])*Math.Cos(c = 0.5*lp[Lambda]))) != 0)
            {
                xy[X] = 2*d*Math.Cos(lp[Phi])*Math.Sin(c)*(xy[Y] = 1/Math.Sin(d));
                xy[Y] *= d*Math.Sin(lp[Phi]);
            }
            else
                xy[X] = xy[Y] = 0;
        }

       


        #endregion

      
       



    }
}
