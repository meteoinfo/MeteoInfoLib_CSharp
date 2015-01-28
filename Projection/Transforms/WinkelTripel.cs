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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 2:26:13 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// WinkelTripel
    /// </summary>
    public class WinkelTripel : Transform
    {
        #region Private Variables

        private double _cosphi1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of WinkelTripel
        /// </summary>
        public WinkelTripel()
        {
            Name = "Winkel_Tripel";
            Proj4Name = "wintri";
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

            xy[X] = (xy[X] + lp[Lambda]*_cosphi1)*0.5;
            xy[Y] = (xy[Y] + lp[Phi])*0.5;
	
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (projInfo.StandardParallel1 != null)
            {
                _cosphi1 = Math.Cos(projInfo.GetPhi1());
                if (_cosphi1 == 0) throw new ProjectionException(22);
            }
            else
            {
                /* 50d28' or acos(2/pi) */
                _cosphi1 = 0.636619772367581343;
            }

        }

        #endregion

        #region Properties



        #endregion



    }
}
