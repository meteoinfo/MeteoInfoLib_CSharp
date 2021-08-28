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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 2:42:59 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// BipolarObliqueConformalConic
    /// </summary>
    public class BipolarObliqueConformalConic : Transform
    {
        #region Private Variables

        private const double EPS = 1e-10;
        private const double ONEEPS = 1.000000001;
        private const int NITER = 10;
        private const double lamB = -.34894976726250681539;
        private const double n = .63055844881274687180;
        private const double F = 1.89724742567461030582;
        private const double Azab = .81650043674686363166;
        private const double Azba = 1.82261843856185925133;
        private const double T = 1.27246578267089012270;
        private const double rhoc = 1.20709121521568721927;
        private const double cAzc = .69691523038678375519;
        private const double sAzc = .71715351331143607555;
        private const double C45 = .70710678118654752469;
        private const double S45 = .70710678118654752410;
        private const double C20 = .93969262078590838411;
        private const double S20 = -.34202014332566873287;
        private const double R110 = 1.91986217719376253360;
        private const double R104 = 1.81514242207410275904;

        private bool _noskew;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of BipolarObliqueConformalConic
        /// </summary>
        public BipolarObliqueConformalConic()
        {
            Proj4Name = "bipc";
            Name = "Bipolar_Oblique_Conformal_Conic";
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
            double tphi, t, al, az, z, av, sdlam;

            double cphi = Math.Cos(lp[Phi]);
            double sphi = Math.Sin(lp[Phi]);
            double cdlam = Math.Cos(sdlam = lamB - lp[Lambda]);
            sdlam = Math.Sin(sdlam);
            if (Math.Abs(Math.Abs(lp[Phi]) - HalfPi) < EPS10)
            {
                az = lp[Phi] < 0 ? Math.PI : 0;
                tphi = double.MaxValue;
            }
            else
            {
                tphi = sphi/cphi;
                az = Math.Atan2(sdlam, C45*(tphi - cdlam));
            }
            bool tag = az > Azba;
            if (tag)
            {
                cdlam = Math.Cos(sdlam = lp[Lambda] + R110);
                sdlam = Math.Sin(sdlam);
                z = S20*sphi + C20*cphi*cdlam;
                if (Math.Abs(z) > 1)
                {
                    if (Math.Abs(z) > ONEEPS) throw new ProjectionException(20);
                    z = z < 0 ? -1 : 1;
                }
                else
                    z = Math.Acos(z);
                if (tphi != double.MaxValue)
                    az = Math.Atan2(sdlam, (C20*tphi - S20*cdlam));
                av = Azab;
                xy[Y] = rhoc;
            }
            else
            {
                z = S45*(sphi + cphi*cdlam);
                if (Math.Abs(z) > 1)
                {
                    if (Math.Abs(z) > ONEEPS) throw new ProjectionException(20);
                    z = z < 0 ? -1 : 1;
                }
                else
                    z = Math.Acos(z);
                av = Azba;
                xy[Y] = -rhoc;
            }
            if (z < 0) throw new ProjectionException(20);
            double r = F*(t = Math.Pow(Math.Tan(.5*z), n));
            if ((al = .5*(R104 - z)) < 0) throw new ProjectionException(20);
            al = (t + Math.Pow(al, n))/T;
            if (Math.Abs(al) > 1)
            {
                if (Math.Abs(al) > ONEEPS) throw new ProjectionException(20);
                al = al < 0 ? -1 : 1;
            }
            else
                al = Math.Acos(al);
            if (Math.Abs(t = n*(av - az)) < al)
                r /= Math.Cos(al + (tag ? t : -t));
            xy[X] = r*Math.Sin(t);
            xy[Y] += (tag ? -r : r)*Math.Cos(t);
            if (_noskew)
            {
                t = xy[X];
                xy[X] = -xy[X]*cAzc - xy[Y]*sAzc;
                xy[Y] = -xy[Y]*cAzc + t*sAzc;
            }
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            double r, rp;
            double z = 0, az, s, c, av;
            int i;

            if (_noskew)
            {
                double t = xy[X];
                xy[X] = -xy[X]*cAzc + xy[Y]*sAzc;
                xy[Y] = -xy[Y]*cAzc - t*sAzc;
            }
            bool neg = (xy[X] < 0);
            if (neg)
            {
                xy[Y] = rhoc - xy[Y];
                s = S20;
                c = C20;
                av = Azab;
            }
            else
            {
                xy[Y] += rhoc;
                s = S45;
                c = C45;
                av = Azba;
            }
            double rl = rp = r = Proj.Hypot(xy[X], xy[Y]);
            double fAz = Math.Abs(az = Math.Atan2(xy[X], xy[Y]));
            for (i = NITER; i > 0; --i)
            {
                z = 2*Math.Atan(Math.Pow(r/F, 1/n));
                double al = Math.Acos((Math.Pow(Math.Tan(.5*z), n) +
                                       Math.Pow(Math.Tan(.5*(R104 - z)), n))/T);
                if (fAz < al)
                    r = rp*Math.Cos(al + (neg ? az : -az));
                if (Math.Abs(rl - r) < EPS)
                    break;
                rl = r;
            }
            if (i == 0) throw new ProjectionException(20);
            az = av - az/n;
            lp[Phi] = Math.Asin(s*Math.Cos(z) + c*Math.Sin(z)*Math.Cos(az));
            lp[Lambda] = Math.Atan2(Math.Sin(az), c/Math.Tan(z) - s*Math.Cos(az));
            if (neg)
                lp[Lambda] -= R110;
            else
                lp[Lambda] = lamB - lp[Lambda];
        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (!projInfo.Parameters.ContainsKey("bns")) return;
            if (projInfo.ParamI("bns") != 0) _noskew = true;
        }

        #endregion

        #region Properties



        #endregion



    }
}
