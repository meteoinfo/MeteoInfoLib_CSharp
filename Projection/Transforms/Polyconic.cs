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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 4:15:20 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Polygconic
    /// </summary>
    public class Polyconic : EllipticalTransform
    {
        #region Private Variables

        private double _ml0;
        private double[] _en;
        private const double TOL = 1E-10;
        private const double CONV = 1E-10;
        private const int N_ITER = 10;
        private const int I_ITER = 20;
        private const double ITOL = 1E-12;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Polygconic
        /// </summary>
        public Polyconic()
        {
            Proj4Name = "poly";
            Name = "Polyconic";
        }

        #endregion

        #region Methods
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (IsElliptical)
            {
                _en = Proj.Enfn(Es);
                _ml0 = Proj.Mlfn(Phi0, Math.Sin(Phi0), Math.Cos(Phi0), _en);
            }
            else
            {
                _ml0 = -Phi0;
            }
        }
        /// <summary>
        /// The forward transform where the spheroidal model of the earth has a flattening factor, 
        /// matching more closely with the oblique spheroid of the actual earth
        /// </summary>
        /// <param name="lp">The double values for geodetic lambda and phi organized into a one dimensional array</param>
        /// <param name="xy">The double values for linear x and y organized into a one dimensional array</param>
        protected override void EllipticalForward(double[] lp, double[] xy)
        {
            if (Math.Abs(lp[Phi]) <= TOL)
            {
                xy[X] = lp[Lambda]; xy[Y] = -_ml0;
            }
	        else 
            {
		        double  sp = Math.Sin(lp[Phi]);
	            double  cp;
	            double  ms = Math.Abs(cp = Math.Cos(lp[Phi])) > TOL ? Proj.Msfn(sp, cp, Es) / sp : 0;
		        xy[X] = ms * Math.Sin(lp[Lambda] *= sp);
		        xy[Y] = (Proj.Mlfn(lp[Phi], sp, cp, _en) - _ml0) + ms * (1- Math.Cos(lp[Lambda]));
	        }
        }

        /// <summary>
        /// The forward transform in the special case where there is no flattening of the spherical model of the earth.
        /// </summary>
        /// <param name="lp">The input lambda and phi geodetic values organized in an array</param>
        /// <param name="xy">The output x and y values organized in an array</param>
        protected override void SphericalForward(double[] lp, double[] xy)
        {
            if (Math.Abs(lp[Phi]) <= TOL)
            {
                xy[X] = lp[Lambda]; xy[Y] = _ml0;
            }
	        else
            {
		        double  cot = 1/ Math.Tan(lp[Phi]);
	            double  e;
	            xy[X] = Math.Sin(e = lp[Lambda] * Math.Sin(lp[Phi])) * cot;
		        xy[Y] = lp[Phi] - Phi0 + cot * (1- Math.Cos(e));
	        }
        }

        /// <summary>
        /// Performs the inverse transfrom from a single coordinate of linear units to the same coordinate in geodetic units    
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void EllipticalInverse(double[] xy, double[] lp)
        {
            xy[Y] += _ml0;
            if (Math.Abs(xy[Y]) <= TOL)
            {
                lp[Lambda] = xy[X];
                lp[Phi] = 0;
            }
            else
            {
                double c;
                int i;
                double r = xy[Y]*xy[Y] + xy[X]*xy[X];
                for (lp[Phi] = xy[Y], i = I_ITER; i > 0; --i)
                {
                    double sp = Math.Sin(lp[Phi]);
                    double cp;
                    double s2ph = sp*(cp = Math.Cos(lp[Phi]));
                    if (Math.Abs(cp) < ITOL)
                        throw new ProjectionException(20);
                    double mlp;
                    c = sp*(mlp = Math.Sqrt(1 - Es*sp*sp))/cp;
                    double ml = Proj.Mlfn(lp[Phi], sp, cp, _en);
                    double mlb = ml*ml + r;
                    mlp = OneEs/(mlp*mlp*mlp);
                    double dPhi;
                    lp[Phi] += (dPhi =
                                (ml + ml + c*mlb - 2*xy[Y]*(c*ml + 1))/( Es*s2ph*(mlb - 2*xy[Y]*ml)/c +
                                                                           2*(xy[Y] - ml)*(c*mlp - 1/s2ph) - mlp - mlp));
                    if (Math.Abs(dPhi) <= ITOL)
                        break;
                }
                if (i == 0) throw new ProjectionException(20);
                c = Math.Sin(lp[Phi]);
                lp[Lambda] = Math.Asin(xy[X]*Math.Tan(lp[Phi])*Math.Sqrt(1 - Es*c*c))/Math.Sin(lp[Phi]);
            }
        }

        /// <summary>
        /// Performs the inverse transform from a single coordinate of linear units to the same coordinate in geodetic lambda and
        /// phi units in the special case where the shape of the earth is being approximated as a perfect sphere.
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void SphericalInverse(double[] xy, double[] lp)
        {
            if (Math.Abs(xy[Y] = Phi0 + xy[Y]) <= TOL)
            {
                lp[Lambda] = xy[X];
                lp[Phi] = 0;
            }
            else
            {
                lp[Phi] = xy[Y];
                double b = xy[X]*xy[X] + xy[Y]*xy[Y];
                int i = N_ITER;
                double dphi;
                do
                {
                    double tp = Math.Tan(lp[Phi]);
                    lp[Phi] -= (dphi = (xy[Y]*(lp[Phi]*tp + 1) - lp[Phi] -
                                        .5*(lp[Phi]*lp[Phi] + b)*tp)/
                                       ((lp[Phi] - xy[Y])/tp - 1));
                } while (Math.Abs(dphi) > CONV && --i > 0);
                if (i == 0) throw new ProjectionException(20);
                lp[Lambda] = Math.Asin(xy[X]*Math.Tan(lp[Phi]))/Math.Sin(lp[Phi]);
            }
        }

        #endregion

  


    }
}
