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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 10:18:14 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
namespace MeteoInfoC.Projections
{


    /// <summary>
    /// GeostationarySatellite
    /// </summary>
    public class GeostationarySatellite : EllipticalTransform
    {
        #region Private Variables

        private double _h;
        private double _radiusP;
        private double _radiusP2;
        private double _radiusPInv2;
        private double _radiusG;
        private double _radiusG1;
        private double _c;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeostationarySatellite
        /// </summary>
        public GeostationarySatellite()
        {
            Name = "Geostationary_Satellite";
            Proj4Name = "geos";
            ProjectionName = ProjectionNames.Geostationary;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The forward transform in the special case where there is no flattening of the spherical model of the earth.
        /// </summary>
        /// <param name="lp">The input lambda and phi geodetic values organized in an array</param>
        /// <param name="xy">The output x and y values organized in an array</param>
        protected override void SphericalForward(double[] lp, double[] xy)
        {
            /* Calculation of the three components of the vector from satellite to
                ** position on earth surface (lon,lat).*/
            double tmp = Math.Cos(lp[Phi]);
            double vx = Math.Cos(lp[Lambda])*tmp;
            double vy = Math.Sin(lp[Lambda])*tmp;
            double vz = Math.Sin(lp[Phi]);
            
            /* Check visibility.*/
            if (((_radiusG - vx) * vx - vy * vy - vz * vz) < 0)
            {
                xy[X] = double.NaN;
                xy[Y] = double.NaN;
                //throw new ProjectionException(20);
                return;
            }
            
            /* Calculation based on view angles from satellite.*/
            tmp = _radiusG - vx;
            xy[X] = _radiusG1*Math.Atan(vy/tmp);
            xy[Y] = _radiusG1*Math.Atan(vz/Proj.Hypot(vy, tmp));
        }
        /// <summary>
        /// The forward transform where the spheroidal model of the earth has a flattening factor, 
        /// matching more closely with the oblique spheroid of the actual earth
        /// </summary>
        /// <param name="lp">The double values for geodetic lambda and phi organized into a one dimensional array</param>
        /// <param name="xy">The double values for linear x and y organized into a one dimensional array</param>
        protected override void EllipticalForward(double[] lp, double[] xy)
        {
            /* Calculation of geocentric latitude. */
            lp[Phi] = Math.Atan(_radiusP2*Math.Tan(lp[Phi]));

            /* Calculation of the three components of the vector from satellite to
                ** position on earth surface (lon,lat).*/
            double r = (_radiusP)/Proj.Hypot(_radiusP*Math.Cos(lp[Phi]), Math.Sin(lp[Phi]));
            double vx = r*Math.Cos(lp[Lambda])*Math.Cos(lp[Phi]);
            double vy = r*Math.Sin(lp[Lambda])*Math.Cos(lp[Phi]);
            double vz = r*Math.Sin(lp[Phi]);

             /* Check visibility. */
            if (((_radiusG - vx) * vx - vy * vy - vz * vz * _radiusPInv2) < 0)
            {
                xy[X] = double.NaN;
                xy[Y] = double.NaN;
                //throw new ProjectionException(20);
                return;
            }

            /* Calculation based on view angles from satellite. */
            double tmp = _radiusG - vx;
            xy[X] = _radiusG1*Math.Atan(vy/tmp);
            xy[Y] = _radiusG1*Math.Atan(vz/Proj.Hypot(vy, tmp));
        }

        /// <summary>
        /// Performs the inverse transform from a single coordinate of linear units to the same coordinate in geodetic lambda and
        /// phi units in the special case where the shape of the earth is being approximated as a perfect sphere.
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void SphericalInverse(double[] xy, double[] lp)
        {
            double det;

            /* Setting three components of vector from satellite to position.*/
            double vx = -1.0;
            double vy = Math.Tan(xy[X]/(_radiusG - 1.0));
            double vz = Math.Tan(xy[Y]/(_radiusG - 1.0))*Math.Sqrt(1.0 + vy*vy);
            
            /* Calculation of terms in cubic equation and determinant.*/
            double a = vy*vy + vz*vz + vx*vx;
            double b = 2*_radiusG*vx;
            if ((det = (b*b) - 4*a*_c) < 0) throw new ProjectionException(20);
            
            /* Calculation of three components of vector from satellite to position.*/
            double k = (-b - Math.Sqrt(det))/(2*a);
            vx = _radiusG + k*vx;
            vy *= k;
            vz *= k;

            /* Calculation of longitude and latitude.*/
            lp[Lambda] = Math.Atan2(vy, vx);
            lp[Phi] = Math.Atan(vz*Math.Cos(lp[Lambda])/vx);

        }
        /// <summary>
        /// Performs the inverse transfrom from a single coordinate of linear units to the same coordinate in geodetic units    
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void EllipticalInverse(double[] xy, double[] lp)
        {
            double det;

            /* Setting three components of vector from satellite to position.*/
            double vx = -1.0;
            double vy = Math.Tan(xy[X] / _radiusG1);
            double vz = Math.Tan(xy[Y] / _radiusG1) * Proj.Hypot(1.0, vy);

            /* Calculation of terms in cubic equation and determinant.*/
            double a = vz / _radiusP;
            a   = vy * vy + a * a + vx * vx;
            double b = 2 * _radiusG * vx;
            if ((det = (b * b) - 4 * a * _c) < 0) throw new ProjectionException(20);

            /* Calculation of three components of vector from satellite to position.*/
            double k = (-b - Math.Sqrt(det)) / (2* a);
            vx = _radiusG + k * vx;
            vy *= k;
            vz *= k;

            /* Calculation of longitude and latitude.*/
            lp[Lambda]  = Math.Atan2(vy, vx);
            lp[Phi] = Math.Atan(vz * Math.Cos(lp[Lambda]) / vx);
            lp[Phi] = Math.Atan(_radiusPInv2 * Math.Tan(lp[Phi]));
        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if ((_h = projInfo.ParamD("h")) <= 0) throw new ProjectionException(-30);
	        //if (Phi0 == 0) throw new ProjectionException(-46);
            _radiusG = 1 + (_radiusG1 = _h / A);
            _c = _radiusG * _radiusG - 1.0;
	        if (IsElliptical) {
                _radiusP = Math.Sqrt(OneEs);
                _radiusP2 = OneEs;
                _radiusPInv2 = ROneEs;

	        } else {
                _radiusP = _radiusP2 = _radiusPInv2 = 1.0;

	        }
        }

        #endregion

        #region Properties



        #endregion



    }
}
