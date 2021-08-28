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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 10:58:10 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// GoodeHomolosine
    /// </summary>
    public class GoodeHomolosine : Transform
    {
        #region Private Variables

        private double Y_COR = 0.05280;
        private double PHI_LIM = .71093078197902358062;
        private Sinusoidal _sinu;
        private Mollweide _moll;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GoodeHomolosine
        /// </summary>
        public GoodeHomolosine()
        {
            Proj4Name = "goode";
            Name = "Goode_Homolosine";
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
            if(Math.Abs(lp[Phi]) <= PHI_LIM)
            {
                xy = _sinu.Forward(lp);
            }
            else
            {
                xy = _moll.Forward(lp);
                xy[Y] -= lp[Phi] >= 0 ? Y_COR : -Y_COR;
            }
        }
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            if (Math.Abs(xy[Y]) <= PHI_LIM)
            {
                lp = _sinu.Inverse(xy);
            }
            else
            {
                xy[Y] += xy[Y] >= 0 ? Y_COR : -Y_COR;
                lp = _moll.Inverse(xy);
            }
        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            _moll = new Mollweide();
            _moll.Init(projInfo);
            _sinu = new Sinusoidal();
            _sinu.Init(projInfo);           
        }


        #endregion

        #region Properties



        #endregion



    }
}
