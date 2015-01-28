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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 3:08:51 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Bonne
    /// </summary>
    public class Bonne : EllipticalTransform
    {
        #region Private Variables

        private double _phi1;
        private double _cphi1;
        private double _am1;
        private double _m1;
        private double[] _en;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Bonne
        /// </summary>
        public Bonne()
        {
            Name = "Bonne";
            Proj4Name = "bonne";
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
            double e, c;
        	double rh = _am1 + _m1 - Proj.Mlfn(lp[Phi], e = Math.Sin(lp[Phi]), c = Math.Cos(lp[Phi]), _en);
	        e = c * lp[Lambda] / (rh * Math.Sqrt(1- Es * e * e));
	        xy[X] = rh * Math.Sin(e);
	        xy[Y] = _am1 - rh * Math.Cos(e);
        }

        /// <summary>
        /// The forward transform in the special case where there is no flattening of the spherical model of the earth.
        /// </summary>
        /// <param name="lp">The input lambda and phi geodetic values organized in an array</param>
        /// <param name="xy">The output x and y values organized in an array</param>
        protected override void SphericalForward(double[] lp, double[] xy)
        {
            double rh = _cphi1 + _phi1 - lp[Phi];
            if (Math.Abs(rh) > EPS10)
            {
                double e;
                xy[X] = rh*Math.Sin(e = lp[Lambda]*Math.Cos(lp[Phi])/rh);
                xy[Y] = _cphi1 - rh*Math.Cos(e);
            }
            else
                xy[X] = xy[Y] = 0;
        }
        /// <summary>
        /// Performs the inverse transform from a single coordinate of linear units to the same coordinate in geodetic lambda and
        /// phi units in the special case where the shape of the earth is being approximated as a perfect sphere.
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void SphericalInverse(double[] xy, double[] lp)
        {
            double rh = Proj.Hypot(xy[X], xy[Y] = _cphi1 - xy[Y]);
            lp[Phi] = _cphi1 + _phi1 - rh;
            if (Math.Abs(lp[Phi]) > HalfPi) throw new ProjectionException(20);
            if (Math.Abs(Math.Abs(lp[Phi]) - HalfPi) <= EPS10)
                lp[Lambda] = 0;
            else
                lp[Lambda] = rh*Math.Atan2(xy[X], xy[Y])/Math.Cos(lp[Phi]);
        }

        /// <summary>
        /// Performs the inverse transfrom from a single coordinate of linear units to the same coordinate in geodetic units    
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void EllipticalInverse(double[] xy, double[] lp)
        {
            double s;

            double rh = Proj.Hypot(xy[X], xy[Y] = _am1 - xy[Y]);
            lp[Phi] = Proj.InvMlfn(_am1 + _m1 - rh, Es, _en);
            if ((s = Math.Abs(lp[Phi])) < HalfPi)
            {
                s = Math.Sin(lp[Phi]);
                lp[Lambda] = rh*Math.Atan2(xy[X], xy[Y])*
                             Math.Sqrt(1 - Es*s*s)/Math.Cos(lp[Phi]);
            }
            else if (Math.Abs(s - HalfPi) <= EPS10)
                lp[Lambda] = 0;
            else throw new ProjectionException(20);
        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            _phi1 = projInfo.GetPhi1();
            if (Math.Abs(_phi1) < EPS10) throw new ProjectionException(-23);
            if (Es > 0)
            {
                _en = Proj.Enfn(Es);
                double c;
                _m1 = Proj.Mlfn(_phi1, _am1 = Math.Sin(_phi1),
                              c = Math.Cos(_phi1), _en);
                _am1 = c / (Math.Sqrt(1 - Es * _am1 * _am1) * _am1);
            }
            else
            {
                if (Math.Abs(_phi1) + EPS10 >= HalfPi)
                    _cphi1 = 0;
                else
                    _cphi1 = 1/Math.Tan(_phi1);
            }
        }

        #endregion

        #region Properties



        #endregion



    }
}
