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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 1:59:49 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Loximuthal
    /// </summary>
    public class Loximuthal : Transform
    {
        #region Private Variables

        private double _phi1;
        private double _cosphi1;
        private double _tanphi1;
        private const double EPS = 1E-8;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Loximuthal
        /// </summary>
        public Loximuthal()
        {
            Proj4Name = "loxim";
            Name = "Loximuthal";

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
            xy[Y] = lp[Phi] - _phi1;
            if (Math.Abs(xy[Y]) < EPS)
                xy[X] = lp[Lambda]*_cosphi1;
            else
            {
                xy[X] = FortPi + 0.5*lp[Phi];
                if (Math.Abs(xy[X]) < EPS || Math.Abs(Math.Abs(xy[X]) - HalfPi) < EPS)
                    xy[X] = 0;
                else
                    xy[X] = lp[Lambda]*xy[Y]/Math.Log(Math.Tan(xy[X])/_tanphi1);
            }
        }
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            lp[Phi] = xy[Y] + _phi1;
            if (Math.Abs(xy[Y]) < EPS)
                lp[Lambda] = xy[X]/_cosphi1;
            else if (Math.Abs(lp[Lambda] = FortPi + 0.5*lp[Phi]) < EPS ||
                     Math.Abs(Math.Abs(lp[Lambda]) - HalfPi) < EPS)
                lp[Lambda] = 0;
            else
                lp[Lambda] = xy[X]*Math.Log(Math.Tan(lp[Lambda])/_tanphi1)/xy[Y];
        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if(projInfo.StandardParallel1 != null)
            {
                _phi1 = projInfo.GetPhi1();
                _cosphi1 = Math.Cos(_phi1);
                if(_cosphi1 < EPS) throw new ProjectionException(22);

            }
            _tanphi1 = Math.Tan(FortPi + 0.5 * _phi1);
        }

        #endregion

        #region Properties



        #endregion



    }
}
