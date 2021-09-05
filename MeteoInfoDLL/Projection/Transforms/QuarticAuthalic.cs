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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 4:31:25 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// QuarticAuthalic
    /// </summary>
    public class QuarticAuthalic : Transform
    {
        #region Private Variables

        private double _cX;
        private double _cY;
        private double _cP;
        private bool _tanMode;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of QuarticAuthalic
        /// </summary>
        public QuarticAuthalic()
        {
            Proj4Name = "qua_aut";
            Name = "Quartic_Authalic";

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
            xy[X] = _cX * lp[Lambda] * Math.Cos(lp[Phi]);
            xy[Y] = _cY;
            lp[Phi] *= _cP;
            double c = Math.Cos(lp[Phi]);
            if (_tanMode)
            {
                xy[X] *= c * c;
                xy[Y] *= Math.Tan(lp[Phi]);
            }
            else
            {
                xy[X] /= c;
                xy[Y] *= Math.Sin(lp[Phi]);
            }
        }
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            xy[Y] /= _cY;
            double c = Math.Cos(lp[Phi] = _tanMode ? Math.Atan(xy[Y]) : Proj.Aasin(xy[Y]));
            lp[Phi] /= _cP;
            lp[Lambda] = xy[X] / (_cX * Math.Cos(lp[Phi] /= _cP));
            if (_tanMode)
                lp[Lambda] /= c * c;
            else
                lp[Lambda] *= c;
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            Setup(2, 2, false);
        }

        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="mode"></param>
        protected void Setup(double p, double q, bool mode)
        {
            _cX = q/p;
            _cY = p;
            _cP = 1/q;
            _tanMode = mode;
        }


        #endregion

        #region Properties



        #endregion



    }
}
