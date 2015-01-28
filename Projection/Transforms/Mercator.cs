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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 2:09:02 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Mercator
    /// </summary>
    public class Mercator : EllipticalTransform
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Mercator
        /// </summary>
        public Mercator()
        {
            Proj4Name = "merc";
            Name = "Mercator";
            ProjectionName = ProjectionNames.Mercator;
        }

        #endregion

        #region Methods
        /// <summary>
        /// The forward transform where the spheroidal model of the earth has a flattening factor, 
        /// matching more closely with the oblique spheroid of the actual earth
        /// </summary>
        /// <param name="lp">The double values for geodetic lambda and phi organized into a one dimensional array</param>
        /// <param name="xy">The double values for linear x and y organized into a one dimensional array</param>
        protected override void EllipticalForward(double[] lp, double[] xy)
        {
            if (Math.Abs(Math.Abs(lp[Phi]) - HalfPi) <= EPS10)
            {
                xy[X] = double.NaN;
                xy[Y] = double.NaN;
                //throw new ProjectionException(20);
                return;
            }
            xy[X] = K0 * lp[Lambda];
            xy[Y] = -K0 * Math.Log(Proj.Tsfn(lp[Phi], Math.Sin(lp[Phi]), E));
        }


        /// <summary>
        /// The forward transform in the special case where there is no flattening of the spherical model of the earth.
        /// </summary>
        /// <param name="lp">The input lambda and phi geodetic values organized in an array</param>
        /// <param name="xy">The output x and y values organized in an array</param>
        protected override void SphericalForward(double[] lp, double[] xy)
        {
            if (Math.Abs(Math.Abs(lp[Phi]) - HalfPi) <= EPS10)
            {
                xy[X] = double.NaN;
                xy[Y] = double.NaN;
                //throw new ProjectionException(20);
                return;
            }
            xy[X] = K0 * lp[Lambda];
            xy[Y] = K0 * Math.Log(Math.Tan(FortPi + .5 * lp[Phi]));
        }

        /// <summary>
        /// Performs the inverse transfrom from a single coordinate of linear units to the same coordinate in geodetic units    
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void EllipticalInverse(double[] xy, double[] lp)
        {
            if ((lp[Phi] = Proj.Phi2(Math.Exp(-xy[Y] / K0), E)) == double.MaxValue) throw new ProjectionException(20);
            lp[Lambda] = xy[X] / K0;
        }

        /// <summary>
        /// Performs the inverse transform from a single coordinate of linear units to the same coordinate in geodetic lambda and
        /// phi units in the special case where the shape of the earth is being approximated as a perfect sphere.
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void SphericalInverse(double[] xy, double[] lp)
        {
            lp[Phi] = HalfPi - 2* Math.Atan(Math.Exp(-xy[Y] / K0));
	        lp[Lambda] = xy[X] / K0;
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double phits=0.0;
	        bool isPhits = false;
            if(projInfo.StandardParallel1 != null)
            {
                isPhits = true;
                phits = projInfo.GetPhi1();
                if (phits >= HalfPi) throw new ProjectionException(-24);
            }
         
            
	        if (IsElliptical) 
            { /* ellipsoid */
		        if (isPhits) K0 = Proj.Msfn(Math.Sin(phits), Math.Cos(phits), Es);
	        } 
            else
            { /* sphere */
		        if (isPhits) K0 = Math.Cos(phits);
	        }
        }

        #endregion

        #region Properties



        #endregion



    }
}
