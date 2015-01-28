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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 4:52:13 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Robinson
    /// </summary>
    public class Robinson : Transform
    {
        #region Private Variables


        private double[] XX;
        private double[] YY;
        private const double FXC = 0.8487;
        private const double FYC = 1.3523;
        private const double C1 = 11.45915590261646417544;
        private const double RC1 = 0.08726646259971647884;
        private const int NODES = 18;
        private const double ONEEPS = 1.000001;
        private const double EPS = 1E-8;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Robinson
        /// </summary>
        public Robinson()
        {
            Proj4Name = "robin";
            Name = "Robinson";
            ProjectionName = ProjectionNames.Robinson;

            XX = new[]
                    {
                        1, -5.67239e-12, -7.15511e-05, 3.11028e-06,
                        0.9986, -0.000482241, -2.4897e-05, -1.33094e-06,
                        0.9954, -0.000831031, -4.4861e-05, -9.86588e-07,
                        0.99, -0.00135363, -5.96598e-05, 3.67749e-06,
                        0.9822, -0.00167442, -4.4975e-06, -5.72394e-06,
                        0.973, -0.00214869, -9.03565e-05, 1.88767e-08,
                        0.96, -0.00305084, -9.00732e-05, 1.64869e-06,
                        0.9427, -0.00382792, -6.53428e-05, -2.61493e-06,
                        0.9216, -0.00467747, -0.000104566, 4.8122e-06,
                        0.8962, -0.00536222, -3.23834e-05, -5.43445e-06,
                        0.8679, -0.00609364, -0.0001139, 3.32521e-06,
                        0.835, -0.00698325, -6.40219e-05, 9.34582e-07,
                        0.7986, -0.00755337, -5.00038e-05, 9.35532e-07,
                        0.7597, -0.00798325, -3.59716e-05, -2.27604e-06,
                        0.7186, -0.00851366, -7.0112e-05, -8.63072e-06,
                        0.6732, -0.00986209, -0.000199572, 1.91978e-05,
                        0.6213, -0.010418, 8.83948e-05, 6.24031e-06,
                        0.5722, -0.00906601, 0.000181999, 6.24033e-06,
                        0.5322, 0, 0, 0
                    };
            YY = new[]
                    {
                        0, 0.0124, 3.72529e-10, 1.15484e-09,
                        0.062, 0.0124001, 1.76951e-08, -5.92321e-09,
                        0.124, 0.0123998, -7.09668e-08, 2.25753e-08,
                        0.186, 0.0124008, 2.66917e-07, -8.44523e-08,
                        0.248, 0.0123971, -9.99682e-07, 3.15569e-07,
                        0.31, 0.0124108, 3.73349e-06, -1.1779e-06,
                        0.372, 0.0123598, -1.3935e-05, 4.39588e-06,
                        0.434, 0.0125501, 5.20034e-05, -1.00051e-05,
                        0.4968, 0.0123198, -9.80735e-05, 9.22397e-06,
                        0.5571, 0.0120308, 4.02857e-05, -5.2901e-06,
                        0.6176, 0.0120369, -3.90662e-05, 7.36117e-07,
                        0.6769, 0.0117015, -2.80246e-05, -8.54283e-07,
                        0.7346, 0.0113572, -4.08389e-05, -5.18524e-07,
                        0.7903, 0.0109099, -4.86169e-05, -1.0718e-06,
                        0.8435, 0.0103433, -6.46934e-05, 5.36384e-09,
                        0.8936, 0.00969679, -6.46129e-05, -8.54894e-06,
                        0.9394, 0.00840949, -0.000192847, -4.21023e-06,
                        0.9761, 0.00616525, -0.000256001, -4.21021e-06,
                        1, 0, 0, 0
                    };

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
            double dphi;

	        int i = (int)Math.Floor((dphi = Math.Abs(lp[Phi])) * C1);
	        if (i >= NODES) i = NODES - 1;
	        dphi = RadToDeg * (dphi - RC1 * i);
	        xy[X] = V(XX, i*4, dphi) * FXC * lp[Lambda];
	        xy[Y] = V(YY, i*4, dphi) * FYC;
	        if (lp[Phi] < 0) xy[Y] = -xy[Y];
        }        
        
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            double[] T = new double[4];

	        lp[Lambda] = xy[X] / FXC;
	        lp[Phi] = Math.Abs(xy[Y] / FYC);
	        if (lp[Phi] >= 1) 
            { /* simple pathologic cases */
                if (lp[Phi] > ONEEPS) throw new ProjectionException(20);
                
                    lp[Phi] = xy[Y] < 0 ? -HalfPi : HalfPi;
                    lp[Lambda] /= XX[NODES*4];
            } 
            else 
            { /* general problem */
		        /* in Y space, reduce to table interval */
                int i;
                for (i = (int)Math.Floor(lp[Phi] * NODES); i < 100000;i++ )
                {
                    if (YY[i*4] > lp[Phi]) --i;
                    else if (YY[(i + 1) * 4] <= lp[Phi]) ++i;
                    else break;
                }
		        Array.Copy(YY, i*4, T, 0, 4);
		        /* first guess, linear interp */
		        double t = 5* (lp[Phi] - T[0])/(YY[(i+1)*4] - T[0]);
		        /* make into root */
		        T[0] -= lp[Phi];
		        for (;;) 
                { /* Newton-Raphson reduction */
                    double t1;
                    t -= t1 = V(T, 0, t) / DV(T, 0, t);
			        if (Math.Abs(t1) < EPS)
				    break;
		        }
		        lp[Phi] = (5 * i + t) * DegToRad;
		        if (xy[Y] < 0) lp[Phi] = -lp[Phi];
		        lp[Lambda] /= V(XX, (i*4), t);
	        }
        }

        #endregion

        #region Properties



        #endregion

        private double V(double[] C, int iStart, double z)
        {
            return C[iStart] + z*(C[iStart+1] + z*(C[iStart+2] + z*C[iStart+3]));
        }
        private double DV(double[] C, int iStart, double z)
        {
            return C[iStart+1] + z*(C[iStart+2] + C[iStart+2] + z*3*C[iStart+3]);
        }

    }
}
