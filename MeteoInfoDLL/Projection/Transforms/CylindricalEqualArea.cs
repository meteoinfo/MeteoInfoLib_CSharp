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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 3:42:13 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// CylindricalEqualArea
    /// </summary>
    public class CylindricalEqualArea : EllipticalTransform
    {
        #region Private Variables

        private double _qp;
        private double[] _apa;
        private const double EPS = 1e-10;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CylindricalEqualArea
        /// </summary>
        public CylindricalEqualArea()
        {
            Name = "Cylindrical_Equal_Area";
            Proj4Name = "cea";
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
            xy[X] = K0 * lp[Lambda];
            xy[Y] = .5 * Proj.Qsfn(Math.Sin(lp[Phi]), E, OneEs) / K0;
        }

        /// <summary>
        /// The forward transform in the special case where there is no flattening of the spherical model of the earth.
        /// </summary>
        /// <param name="lp">The input lambda and phi geodetic values organized in an array</param>
        /// <param name="xy">The output x and y values organized in an array</param>
        protected override void SphericalForward(double[] lp, double[] xy)
        {
            xy[X] = K0 * lp[Lambda];
            xy[Y] = Math.Sin(lp[Phi]) / K0;
        }
        /// <summary>
        /// Performs the inverse transfrom from a single coordinate of linear units to the same coordinate in geodetic units    
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void EllipticalInverse(double[] xy, double[] lp)
        {
            lp[Phi] = Proj.AuthLat(Math.Asin( 2* xy[Y] * K0 / _qp), _apa);
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
            double t;

            if ((t = Math.Abs(xy[Y] *= K0)) - EPS <= 1)
            {
                if (t >= 1)
                    lp[Phi] = xy[Y] < 0 ? -HalfPi : HalfPi;
                else
                    lp[Phi] = Math.Asin(xy[Y]);
                lp[Lambda] = xy[X]/K0;
            }
            else throw new ProjectionException(20);
        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double t=0;
            if (projInfo.StandardParallel1 != null) t = projInfo.StandardParallel1.Value * Math.PI / 180;
            t = projInfo.ParamD("lat_ts")*Math.PI/180;
            if((K0 = Math.Cos(t)) < 0) throw new ProjectionException(-24);
            if (!IsElliptical) return;
            t = Math.Sin(t);
            K0 /= Math.Sqrt(1- Es * t * t);
            E = Math.Sqrt(Es);
            _apa = Proj.Authset(Es);
            _qp = Proj.Qsfn(1, E, OneEs);
        }

        #endregion

        #region Properties



        #endregion



    }
}
