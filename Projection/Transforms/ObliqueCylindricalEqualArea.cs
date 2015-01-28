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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 4:01:37 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// ObliqueCylindricalEqualArea
    /// </summary>
    public class ObliqueCylindricalEqualArea : Transform
    {
        #region Private Variables

        private double _rok;
        private double _rtk;
        private double _sinphi;
        private double _cosphi;
        private double _singam;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ObliqueCylindricalEqualArea
        /// </summary>
        public ObliqueCylindricalEqualArea()
        {
            Proj4Name = "ocea";
            Name = "Oblique_Cylindrical_Equal_Area";
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
            xy[Y] = Math.Sin(lp[Lambda]);
	        double t = Math.Cos(lp[Lambda]);
	        xy[X] = Math.Atan((Math.Tan(lp[Phi]) * _cosphi + _sinphi * xy[Y]) / t);
	        if (t < 0) xy[X] += Math.PI;
	        xy[X] *= _rtk;
	        xy[Y] = _rok * (_sinphi * Math.Sin(lp[Phi]) - _cosphi * Math.Cos(lp[Phi]) * xy[Y]);
        }
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            double s;
	        xy[Y] /= _rok;
	        xy[X] /= _rtk;
	        double t = Math.Sqrt(1- xy[Y] * xy[Y]);
	        lp[Phi] = Math.Asin(xy[Y] * _sinphi + t * _cosphi * (s = Math.Sin(xy[X])));
	        lp[Lambda] = Math.Atan2(t * _sinphi * s - xy[Y] * _cosphi,
		        t * Math.Cos(xy[X]));
        }

        #endregion

        #region Properties
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            const double phi_0 = 0.0;

            _rok = A/K0;
            _rtk = A*K0;
            if (projInfo.Parameters.ContainsKey("alpha"))
            {
                double alpha = projInfo.ParamR("alpha");
                double lonz = projInfo.ParamR("lonc");
                _singam = Math.Atan(-Math.Cos(alpha)/(-Math.Sin(phi_0)*Math.Sin(alpha))) + lonz;
                _sinphi = Math.Asin(Math.Cos(phi_0)*Math.Sin(alpha));
            }
            else
            {
                double phi_1 = projInfo.GetPhi1();
                double phi_2 = projInfo.GetPhi2();
                double lam_1 = projInfo.ParamR("lon_1");
                double lam_2 = projInfo.ParamR("lon_2");
                _singam = Math.Atan2(Math.Cos(phi_1)*Math.Sin(phi_2)*Math.Cos(lam_1) -
                                     Math.Sin(phi_1)*Math.Cos(phi_2)*Math.Cos(lam_2),
                                     Math.Sin(phi_1)*Math.Cos(phi_2)*Math.Sin(lam_2) -
                                     Math.Cos(phi_1)*Math.Sin(phi_2)*Math.Sin(lam_1));
                _sinphi = Math.Atan(-Math.Cos(_singam - lam_1)/Math.Tan(phi_1));
            }
            Lam0 = _singam + HalfPi;
            _cosphi = Math.Cos(_sinphi);
            _sinphi = Math.Sin(_sinphi);
            _singam = Math.Sin(_singam);
        }

        #endregion



    }
}
