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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 11:39:09 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Sinusoidal
    /// </summary>
    public class Sinusoidal : EllipticalTransform
    {
        #region Private Variables

        private double[] _en;
        private double _m;
        private double _n;
        private double _cX;
        private double _cY;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Sinusoidal
        /// </summary>
        public Sinusoidal()
        {
            Proj4Name = "sinu";
            Name = "Sinusoidal";
            ProjectionName = ProjectionNames.Sinusoidal;
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
            double s, c;
	        xy[Y] = Proj.Mlfn(lp[Phi], s = Math.Sin(lp[Phi]), c = Math.Cos(lp[Phi]), _en);
	        xy[X] = lp[Lambda] * c / Math.Sqrt(1- Es * s * s);
        }

        /// <summary>
        /// Performs the inverse transform from a single coordinate of linear units to the same coordinate in geodetic lambda and
        /// phi units in the special case where the shape of the earth is being approximated as a perfect sphere.
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void SphericalInverse(double[] xy, double[] lp)
        {
            xy[Y] /= _cY;
            lp[Phi] = _m > 0
                          ? Proj.Aasin((_m*xy[Y] + Math.Sin(xy[Y]))/_n)
                          :
                              (_n != 1 ? Proj.Aasin(Math.Sin(xy[Y])/_n) : xy[Y]);
            lp[Lambda] = xy[X]/(_cX*(_m + Math.Cos(xy[Y])));
        }
        /// <summary>
        /// The forward transform where the spheroidal model of the earth has a flattening factor, 
        /// matching more closely with the oblique spheroid of the actual earth
        /// </summary>
        /// <param name="lp">The double values for geodetic lambda and phi organized into a one dimensional array</param>
        /// <param name="xy">The double values for linear x and y organized into a one dimensional array</param>
        protected override void EllipticalForward(double[] lp, double[] xy)
        {
           double s, c;
	        xy[Y] = Proj.Mlfn(lp[Phi], s = Math.Sin(lp[Phi]), c = Math.Cos(lp[Phi]), _en);
	        xy[X] = lp[Lambda] * c / Math.Sqrt(1- Es * s * s);

        }

        /// <summary>
        /// Performs the inverse transfrom from a single coordinate of linear units to the same coordinate in geodetic units    
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void EllipticalInverse(double[] xy, double[] lp)
        {
            double s;

            if ((s = Math.Abs(lp[Phi] = Proj.InvMlfn(xy[Y], Es, _en))) < HalfPi)
            {
                s = Math.Sin(lp[Phi]);
                lp[Lambda] = xy[X]*Math.Sqrt(1 - Es*s*s)/Math.Cos(lp[Phi]);
            }
            else if ((s - EPS10) < HalfPi)
                lp[Lambda] = 0;
            else throw new ProjectionException(20);
        }
        /// <summary>
        /// Handles the original configuration of sinusoidal transforms
        /// </summary>
        protected void Setup()
        {
            _cX = (_cY = Math.Sqrt((_m + 1) / _n)) / (_m + 1);
        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            _en = Proj.Enfn(Es);
            if(IsElliptical == false)
            {
                _n = 1;
                _m = 0;
                Setup();
            }
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the double M value
        /// </summary>
        protected double M
        {
            get { return _m; }
            set { _m = value; }
        }

        /// <summary>
        /// Gets or sets the N value
        /// </summary>
        protected double N
        {
            get { return _n; }
            set { _n = value; }
        }

        #endregion




    }
}
