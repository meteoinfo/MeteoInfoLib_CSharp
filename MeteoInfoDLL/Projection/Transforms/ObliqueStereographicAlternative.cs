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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/17/2009 9:06:15 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// ObliqueStereographicAlternative
    /// </summary>
    public class ObliqueStereographicAlternative : Transform
    {
        #region Private Variables

        private double _phic0;
        private double _sinc0;
        private double _cosc0;
        private double _r2;
        private Gauss _gauss;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ObliqueStereographicAlternative
        /// </summary>
        public ObliqueStereographicAlternative()
        {
            Proj4Name = "sterea";
            Name = "Oblique_Stereographic_Alternative";
            ProjectionName = ProjectionNames.Oblique_Stereographic;
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
            lp = _gauss.Forward(lp);
	        double sinc = Math.Sin(lp[Phi]);
	        double cosc = Math.Cos(lp[Phi]);
	        double cosl = Math.Cos(lp[Lambda]);
	        double k = K0 * _r2 / (1+ _sinc0 * sinc + _cosc0 * cosc * cosl);
	        xy[X] = k * cosc * Math.Sin(lp[Lambda]);
	        xy[Y] = k * (_cosc0 * sinc - _sinc0 * cosc * cosl);
        }
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            double rho;

            xy[X] /= K0;
	        xy[Y] /= K0;
	        if((rho = Proj.Hypot(xy[X], xy[Y])) > 0) {
		        double c = 2* Math.Atan2(rho, _r2);
		        double sinc = Math.Sin(c);
		        double cosc = Math.Cos(c);
		        lp[Phi] = Math.Asin(cosc * _sinc0 + xy[Y] * sinc * _cosc0 / rho);
		        lp[Lambda] = Math.Atan2(xy[X] * sinc, rho * _cosc0 * cosc -
			        xy[Y] * _sinc0 * sinc);
	        } else {
		        lp[Phi] = _phic0;
		        lp[Lambda] = 0;
	        }
            double[] temp = _gauss.Inverse(lp);
            lp[Phi] = temp[Phi];
            lp[Lambda] = temp[Lambda];
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double r = 0;
            _gauss = new Gauss(E, Phi0, ref _phic0, ref r);
	        _sinc0 = Math.Sin(_phic0);
	        _cosc0 = Math.Cos(_phic0);
	        _r2 = 2 * r;
        }

        #endregion

        #region Properties



        #endregion



    }
}
