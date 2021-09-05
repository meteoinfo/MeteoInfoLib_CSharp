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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/17/2009 10:09:09 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// SwissObliqueMercator
    /// </summary>
    public class SwissObliqueMercator : Transform
    {
        #region Private Variables

        private double _k;
        private double _c;
        private double _hlfE;
        private double _kR;
        private double _cosp0;
        private double _sinp0;

        private const double EPS = 1E-10;
        private const int Niter = 6;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SwissObliqueMercator
        /// </summary>
        public SwissObliqueMercator()
        {
            Proj4Name = "somerc";
            Name = "Swiss_Oblique_Mercator";
           
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double phip0;

	        _hlfE = 0.5 * E;
	        double cp = Math.Cos(Phi0);
	        cp *= cp;
	        _c = Math.Sqrt(1 + Es * cp * cp * ROneEs);
	        double sp = Math.Sin(Phi0);
	        _cosp0 = Math.Cos( phip0 = Proj.Aasin(_sinp0 = sp / _c) );
	        sp *= E;
	        _k = Math.Log(Math.Tan(FortPi + 0.5 * phip0)) - _c * (
		        Math.Log(Math.Tan(FortPi + 0.5 * Phi0)) - _hlfE *
		        Math.Log((1+ sp) / (1- sp)));
	        _kR = K0 * Math.Sqrt(OneEs) / (1- sp * sp);
        }
        /// <summary>
        /// The forward transform from geodetic units to linear units
        /// </summary>
        /// <param name="lp">The array of lambda, phi coordinates</param>
        /// <param name="xy">The array of x, y coordinates</param>
        protected override void OnForward(double[] lp, double[] xy)
        {
            double sp = E * Math.Sin(lp[Phi]);
	        double phip = 2 * Math.Atan( Math.Exp( _c * (
	                                                        Math.Log(Math.Tan(FortPi + 0.5 * lp[Phi])) - _hlfE * Math.Log((1+ sp)/(1- sp)))
	                                               + _k)) - HalfPi;
	        double lamp = _c * lp[Lambda];
	        double cp = Math.Cos(phip);
	        double phipp = Proj.Aasin(_cosp0 * Math.Sin(phip) - _sinp0 * cp * Math.Cos(lamp));
	        double lampp = Proj.Aasin(cp * Math.Sin(lamp) / Math.Cos(phipp));
	        xy[X] = _kR * lampp;
	        xy[Y] = _kR * Math.Log(Math.Tan(FortPi + 0.5 * phipp));
        }
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            int i;

	        double phipp = 2* (Math.Atan(Math.Exp(xy[Y] / _kR)) - FortPi);
	        double lampp = xy[X] / _kR;
	        double cp = Math.Cos(phipp);
	        double phip = Proj.Aasin(_cosp0 * Math.Sin(phipp) + _sinp0 * cp * Math.Cos(lampp));
	        double lamp = Proj.Aasin(cp * Math.Sin(lampp) / Math.Cos(phip));
	        double con = (_k - Math.Log(Math.Tan(FortPi + 0.5 * phip)))/_c;
	        for (i = Niter; i > 0 ; --i) {
		        double esp = E * Math.Sin(phip);
		        double delp = (con + Math.Log(Math.Tan(FortPi + 0.5 * phip)) - _hlfE *
		                                                                       Math.Log((1+ esp)/(1- esp)) ) *
		                      (1- esp * esp) * Math.Cos(phip) * ROneEs;
		        phip -= delp;
		        if (Math.Abs(delp) < EPS)
			        break;
	        }
            if (i <= 0) throw new ProjectionException(20);
            lp[Phi] = phip;
            lp[Lambda] = lamp/_c;
            
        }

        #endregion

       



    }
}
