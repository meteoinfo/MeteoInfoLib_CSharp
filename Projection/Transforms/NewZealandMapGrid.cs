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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 2:34:47 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// NewZealandMapGrid
    /// </summary>
    public class NewZealandMapGrid : Transform
    {
        #region Private Variables


        private const double Sec5ToRad = 0.4848136811095359935899141023;
        private const double RadToSec5 = 2.062648062470963551564733573;
        private const int Nbf = 5;
        private const int Ntpsi = 9;
        private const int Ntphi = 8;
        
        private readonly double[][] _bf;
        private readonly double[] _tphi;
        private readonly double[] _tpsi;
        

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NewZealandMapGrid
        /// </summary>
        public NewZealandMapGrid() 
        {
            Proj4Name = "nzmg";
            Name = "New_Zealand_Map_Grid";
            _bf = new double[6][];
            _bf[0] = new[] {.7557853228, 0.0};
            _bf[1] = new[] {.249204646, .003371507};
            _bf[2] = new[] {-.001541739, .041058560};
            _bf[3] = new[] { -.10162907, .01727609 };
            _bf[4] = new[] {-.26623489, -.36249218};
            _bf[5] = new[] { -.6870983, -1.1651967 };

            _tphi = new[] {1.5627014243, .5185406398, -.03333098, -.1052906, -.0368594, .007317, .01220, .00394, -.0013};
            _tpsi = new[]
                        {
                            .6399175073, -.1358797613, .063294409, -.02526853, .0117879, -.0055161, .0026906, -.001333,
                            .00067, -.00034
                        };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            Ra = 1/ (A = 6378388.0);
	        Lam0 = DegToRad * 173;
	        Phi0 = DegToRad * -41;
	        X0 = 2510000;
	        Y0 = 6023150;
        }

        /// <summary>
        /// The forward transform from geodetic units to linear units
        /// </summary>
        /// <param name="lp">The array of lambda, phi coordinates</param>
        /// <param name="xy">The array of x, y coordinates</param>
        protected override void OnForward(double[] lp, double[] xy)
        {
            double[] p = new double[2];          

            lp[Phi] = (lp[Phi] - Phi0) * RadToSec5;
            p[R] = _tpsi[Ntpsi];
            for (int i = Ntpsi-1; i >= 0; i--)
            {
                p[R] = _tpsi[i] + lp[Phi]*p[R];
            }
            p[R] *= lp[Phi];
            p[I] = lp[Lambda];
            p = Proj.Zpoly1(p, _bf, Nbf);
            xy[X] = p[I];
            xy[Y] = p[R];
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            int nn;
            double[] p = new double[2];
            double[] dp = new double[2];
            p[R] = xy[Y];
	        p[I] = xy[X];
	        for (nn = 20; nn > 0 ;--nn) 
            {
                double[] fp;
                double[] f = Proj.Zpolyd1(p, _bf, Nbf, out fp);
		        f[R] -= xy[Y];
		        f[I] -= xy[X];
		        double den = fp[R] * fp[R] + fp[I] * fp[I];
		        p[R] += dp[R] = -(f[R] * fp[R] + f[I] * fp[I]) / den;
		        p[I] += dp[I] = -(f[I] * fp[R] - f[R] * fp[I]) / den;
		        if ((Math.Abs(dp[R]) + Math.Abs(dp[I])) <= EPS10)
			    break;
	        }
	        if (nn > 0)
	        {
	            lp[Lambda] = p[I];
	            lp[Phi] = _tphi[Ntphi];
	            for (int i = Ntphi - 1; i > 0; i++)
	            {
	                lp[Phi] = _tphi[i] + p[R]*lp[Phi];
	            }
                lp[Phi] = Phi0 + p[R] * lp[Phi] * Sec5ToRad;
	        }
            else
	        {
                lp[Lambda] = lp[Phi] = double.MaxValue;
	        }

        }
        #endregion

        #region Properties



        #endregion



    }
}
