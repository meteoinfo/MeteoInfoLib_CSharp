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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/12/2009 5:14:06 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// EquidistantConic
    /// </summary>
    public class EquidistantConic : Transform
    {
        #region Private Variables

        private double _phi1;
        private double _phi2;
        private double _n;
        private double _rho;
        private double _rho0;
        private double _c;
        private double[] _en;
       
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of EquidistantConic
        /// </summary>
        public EquidistantConic()
        {
            Proj4Name = "eqdc";
            Name = "Equidistant_Conic";
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
            _rho = _c - (IsElliptical ? Proj.Mlfn(lp[Phi], Math.Sin(lp[Phi]),
            Math.Cos(lp[Phi]), _en) : lp[Phi]);
            xy[X] = _rho * Math.Sin(lp[Lambda] *= _n);
            xy[Y] = _rho0 - _rho * Math.Cos(lp[Lambda]);
        }
        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            if ((_rho = Proj.Hypot(xy[X], xy[Y] = _rho0 - xy[Y])) != 0.0)
            {
                if (_n < 0)
                {
                    _rho = -_rho;
                    xy[X] = -xy[X];
                    xy[Y] = -xy[Y];
                }
                lp[Phi] = _c - _rho;
                if (IsElliptical)
                    lp[Phi] = Proj.InvMlfn(lp[Phi], Es, _en);
                lp[Lambda] = Math.Atan2(xy[X], xy[Y])/_n;
            }
            else
            {
                lp[Lambda] = 0;
                lp[Phi] = _n > 0 ? HalfPi : - HalfPi;
            }
        }

        /// <summary>
        /// This exists in the case that we ever develop code to perform the special proj4 calculations
        /// </summary>
        /// <param name="lp"></param>
        /// <param name="p"></param>
        /// <param name="fac"></param>
        protected override void OnSpecial(double[] lp, ProjectionInfo p, Factors fac)
        {
            double sinphi = Math.Sin(lp[Phi]);
            double cosphi = Math.Cos(lp[Phi]);
            fac.code = fac.code | AnalyticModes.IsAnalHK;
            fac.h = 1;
            fac.k = _n*(_c - (IsElliptical
                                  ? Proj.Mlfn(lp[Phi], sinphi,
                                            cosphi, _en)
                                  : lp[Phi]))/Proj.Msfn(sinphi, cosphi, Es);
        }
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            double sinphi;

            if (projInfo.StandardParallel1 != null) _phi1 = projInfo.StandardParallel1.Value * Math.PI / 180;
            if (projInfo.StandardParallel2 != null) _phi2 = projInfo.StandardParallel2.Value * Math.PI / 180;

            if (Math.Abs(_phi1 + _phi2) < EPS10) throw new ProjectionException(-21);
            _en = Proj.Enfn(Es);
            _n = sinphi = Math.Sin(_phi1);
            double cosphi = Math.Cos(_phi1);
            bool secant = Math.Abs(_phi1 - _phi2) >= EPS10;
            if (IsElliptical)
            {
                double m1 = Proj.Msfn(sinphi, cosphi, Es);
                double ml1 = Proj.Mlfn(_phi1, sinphi, cosphi, _en);
                if (secant)
                {
                    /* secant cone */
                    sinphi = Math.Sin(_phi2);
                    cosphi = Math.Cos(_phi2);
                    _n = (m1 - Proj.Msfn(sinphi, cosphi, Es))/
                         (Proj.Mlfn(_phi2, sinphi, cosphi, _en) - ml1);
                }
                _c = ml1 + m1/_n;
                _rho0 = _c - Proj.Mlfn(Phi0, Math.Sin(Phi0),
                                     Math.Cos(Phi0), _en);
            }
            else
            {
                if (secant)
                    _n = (cosphi - Math.Cos(_phi2))/(_phi2 - _phi1);
                _c = _phi1 + Math.Cos(_phi1)/_n;
                _rho0 = _c - Phi0;
            }
        }




        #endregion

    

    }
}
