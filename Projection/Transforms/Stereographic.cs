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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/11/2009 10:18:01 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Stereographic
    /// </summary>
    public class Stereographic : EllipticalTransform
    {
        #region Private Variables

        private const double TOL = 1E-8;
        private const int Niter = 8;
        private const double Conv = 1E-10;
        private Modes _mode;
        private double _phits;
        private double _sinX1;
        private double _cosX1;
        private double _akm1;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Stereographic
        /// </summary>
        public Stereographic()
        {
            Name = "Stereographic";
            Proj4Name = "stere";
            ProjectionName = ProjectionNames.Oblique_Stereographic;
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
            double sinX=0.0, cosX=0.0;

            double coslam = Math.Cos(lp[Lambda]);
	        double sinlam = Math.Sin(lp[Lambda]);
	        double sinphi = Math.Sin(lp[Phi]);
	        if (_mode == Modes.Oblique || _mode == Modes.Equitorial) 
            {
                double x;
                sinX = Math.Sin(x = 2* Math.Atan(Ssfn(lp[Phi], sinphi, E)) - HalfPi);
		        cosX = Math.Cos(x);
	        }
            if(_mode == Modes.Oblique || _mode == Modes.Equitorial)
            {
                double a;
                if(_mode == Modes.Oblique)
                {
                    a = _akm1 / (_cosX1 * (1+ _sinX1 * sinX +
		            _cosX1 * cosX * coslam));
		            xy[Y] = a * (_cosX1 * sinX - _sinX1 * cosX * coslam);

                }
                else
                {
                    a = 2* _akm1 / (1+ cosX * coslam);
		            xy[Y] = a * sinX;
                }
                xy[X] = a * cosX;
            }
            else
            {
                if(_mode == Modes.SouthPole)
                {
                    lp[Phi] = -lp[Phi];
		            coslam = - coslam;
		            sinphi = -sinphi;
                }
                xy[X] = _akm1 * Proj.Tsfn(lp[Phi], sinphi, E);
		        xy[Y] = - xy[X] * coslam;
            }
            xy[X] = xy[X] * sinlam;
        }

        /// <summary>
        /// The forward transform in the special case where there is no flattening of the spherical model of the earth.
        /// </summary>
        /// <param name="lp">The input lambda and phi geodetic values organized in an array</param>
        /// <param name="xy">The output x and y values organized in an array</param>
        protected override void SphericalForward(double[] lp, double[] xy)
        {
            double sinphi = Math.Sin(lp[Phi]);
	        double cosphi = Math.Cos(lp[Phi]);
	        double coslam = Math.Cos(lp[Lambda]);
	        double sinlam = Math.Sin(lp[Lambda]);
            if(_mode == Modes.Equitorial || _mode == Modes.Oblique)
            {
                if(_mode == Modes.Equitorial)
                {
                    xy[Y] = 1+ cosphi * coslam;
                }
                else
                {
                    xy[Y] = 1+ _sinX1 * sinphi + _cosX1 * cosphi * coslam;
                }
                if (xy[Y] <= EPS10)
                {
                    xy[X] = double.NaN;
                    xy[Y] = double.NaN;
                    //throw new ProjectionException(20);
                    return;
                }
		        xy[X] = (xy[Y] = _akm1 / xy[Y]) * cosphi * sinlam;
		        xy[Y] *= (_mode == Modes.Equitorial) ? sinphi :
		        _cosX1 * sinphi - _sinX1 * cosphi * coslam;
            }
            else
            {
                if (_mode == Modes.NorthPole)
                {
                    coslam = -coslam;
                    lp[Phi] = -lp[Phi];
                }
                if ( Math.Abs(lp[Phi] - HalfPi) < TOL)
                {
                    xy[X] = double.NaN;
                    xy[Y] = double.NaN;
                    //throw new ProjectionException(20);
                    return;
                }
		        xy[X] = sinlam * ( xy[Y] = _akm1 * Math.Tan(FortPi + .5 * lp[Phi]) );
		        xy[Y] *= coslam;
            }
        }

        /// <summary>
        /// Performs the inverse transfrom from a single coordinate of linear units to the same coordinate in geodetic units    
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void EllipticalInverse(double[] xy, double[] lp)
        {
            double sinphi, tp=0.0, phiL=0.0, halfe=0.0, halfpi=0.0;
            int i;

	        double rho = Proj.Hypot(xy[X], xy[Y]);
	        switch (_mode) 
            {
	            case Modes.Oblique:
	            case Modes.Equitorial:
		            double cosphi = Math.Cos( tp = 2* Math.Atan2(rho * _cosX1 , _akm1) );
		            sinphi = Math.Sin(tp);
                    if( rho == 0.0 )
		                phiL = Math.Asin(cosphi * _sinX1);
                    else
		                phiL = Math.Asin(cosphi * _sinX1 + (xy[Y] * sinphi * _cosX1 / rho));
		            tp = Math.Tan(.5 * (HalfPi + phiL));
		            xy[X] *= sinphi;
		            xy[Y] = rho * _cosX1 * cosphi - xy[Y] * _sinX1* sinphi;
		            halfpi = HalfPi;
		            halfe = .5 * E;
		            break;
	            case Modes.NorthPole:
		        case Modes.SouthPole:
                    if(_mode == Modes.NorthPole)xy[Y] = -xy[Y];
		            phiL = HalfPi - 2* Math.Atan(tp = - rho / _akm1);
		            halfpi = -HalfPi;
		            halfe = -.5 * E;
		            break;
	        }
	        for (i = Niter; i-- > 0; phiL = lp[Phi]) 
            {
		        sinphi = E * Math.Sin(phiL);
		        lp[Phi] = 2* Math.Atan(tp * Math.Pow((1+sinphi)/(1-sinphi), halfe)) - halfpi;
		        if ( Math.Abs(phiL - lp[Phi]) < Conv) 
                {
			        if (_mode == Modes.SouthPole) lp[Phi] = -lp[Phi];
			        lp[Lambda] = (xy[X] == 0&& xy[Y] == 0) ? 0: Math.Atan2(xy[X], xy[Y]);
			        return;
		        }
	        }
            throw new ProjectionException(20);
        }

        /// <summary>
        /// Performs the inverse transform from a single coordinate of linear units to the same coordinate in geodetic lambda and
        /// phi units in the special case where the shape of the earth is being approximated as a perfect sphere.
        /// </summary>
        /// <param name="xy">The double linear input x and y values organized into a 1 dimensional array</param>
        /// <param name="lp">The double geodetic output lambda and phi values organized into a 1 dimensional array</param>
        protected override void SphericalInverse(double[] xy, double[] lp)
        {
            double  c, rh;

	        double sinc = Math.Sin(c = 2* Math.Atan((rh = Proj.Hypot(xy[X], xy[Y])) / _akm1));
	        double cosc = Math.Cos(c);
	        lp[Lambda] = 0;
            switch (_mode)
            {
	            case Modes.Equitorial:
		            if ( Math.Abs(rh) <= EPS10) lp[Phi] = 0;
		            else lp[Phi] = Math.Asin(xy[Y] * sinc / rh);
		            if (cosc != 0|| xy[X] != 0) lp[Lambda] = Math.Atan2(xy[X] * sinc, cosc * rh);
		            break;
	            case Modes.Oblique:
		            if ( Math.Abs(rh) <= EPS10) lp[Phi] = Phi0;
		            else lp[Phi] = Math.Asin(cosc * _sinX1 + xy[Y] * sinc * _cosX1 / rh);
		            if ((c = cosc - _sinX1 * Math.Sin(lp[Phi])) != 0|| xy[X] != 0)
			        lp[Lambda] = Math.Atan2(xy[X] * sinc * _cosX1, c * rh);
		            break;
	            case Modes.NorthPole:
                case Modes.SouthPole:
		            if(_mode == Modes.NorthPole)xy[Y] = -xy[Y];
                    if (Math.Abs(rh) <= EPS10)
                    {
                        lp[Phi] = Phi0;
                    }
                    else
                    {
                        lp[Phi] = Math.Asin(_mode == Modes.SouthPole ? -cosc : cosc);
                    }
		            lp[Lambda] = (xy[X] == 0 && xy[Y] == 0) ? 0: Math.Atan2(xy[X], xy[Y]);
		            break;
	        }

        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (projInfo.StandardParallel1 != null)
            {
                _phits = projInfo.GetPhi1();                
            }
            else
            {
                _phits = HalfPi;
            }
            double t;
            if (Math.Abs((t = Math.Abs(Phi0)) - HalfPi) < EPS10)
            {
                _mode = Phi0 < 0 ? Modes.SouthPole : Modes.NorthPole;
            }
            else
            {
                _mode = t > EPS10 ? Modes.Oblique : Modes.Equitorial;
            }
	        _phits = Math.Abs(_phits);
	        if (Es != 0) 
            {
                switch (_mode) 
                {
		        case Modes.NorthPole:
		        case Modes.SouthPole:
			        if (Math.Abs(_phits - HalfPi) < EPS10)
                    {
				        _akm1 = 2* K0 / Math.Sqrt( Math.Pow(1+E,1+E)* Math.Pow(1-E,1-E));
                    }
			        else 
                    {
				        _akm1 =Math.Cos(_phits) / Proj.Tsfn(_phits, t =Math.Sin(_phits), E);
				        t *= E;
				        _akm1 /= Math.Sqrt(1- t * t);
			        }
			        break;
		        case Modes.Equitorial:
			        _akm1 = 2* K0;
			        break;
		        case Modes.Oblique:
			        t =Math.Sin(Phi0);
			        double x = 2* Math.Atan(Ssfn(Phi0, t, E)) - HalfPi;
			        t *= E;
			        _akm1 = 2* K0 *Math.Cos(Phi0) / Math.Sqrt(1- t * t);
			        _sinX1 =Math.Sin(x);
			        _cosX1 =Math.Cos(x);
			        break;
		        }
	        }
            else 
            {
                if(_mode == Modes.Oblique || _mode == Modes.Equitorial)
                {
                    if (_mode == Modes.Oblique)
                    {
                        _sinX1 = Math.Sin(Phi0);
                        _cosX1 = Math.Cos(Phi0);
                    }
                    _akm1 = 2 * K0;
                }
		        else
                {
					_akm1 = Math.Abs(_phits - HalfPi) >= EPS10 ?
			        Math.Cos(_phits) / Math.Tan(FortPi - .5 * _phits) :
			        2* K0 ;
			    }
		    }

            //Judge if north polar or south polar
            if (projInfo.LatitudeOfOrigin == 90)
                ProjectionName = ProjectionNames.North_Polar_Stereographic;
            else if (projInfo.LatitudeOfOrigin == -90)
                ProjectionName = ProjectionNames.South_Polar_Stereographic;

    	}

        #endregion

        
      
        private static double Ssfn(double phit, double sinphi, double eccen)
        {
            sinphi *= eccen;
            return (Math.Tan(.5 * (HalfPi + phit)) *
                Math.Pow((1 - sinphi) / (1 + sinphi), .5 * eccen));
        }        


    }
}
