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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 9:26:57 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Winkel2
    /// </summary>
    public class Winkel2 : Transform
    {
        #region Private Variables

        private const int MaxIter = 10;
        private const double LoopTOL = 1e-7;
        private const double TwoDPi = 0.636619772367581343;
        private double _cosphi1;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Winkel2
        /// </summary>
        public Winkel2()
        {
            Proj4Name = "wink2";
            Name = "Winkel_II";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            _cosphi1 = Math.Cos(projInfo.GetPhi1());
        }

        /// <summary>
        /// The forward transform from geodetic units to linear units
        /// </summary>
        /// <param name="lp">The array of lambda, phi coordinates</param>
        /// <param name="xy">The array of x, y coordinates</param>
        protected override void OnForward(double[] lp, double[] xy)
        {
            int i;

            xy[Y] = lp[Phi]*TwoDPi;
            double k = Math.PI*Math.Sin(lp[Phi]);
            lp[Phi] *= 1.8;
            for (i = MaxIter; i > 0; --i)
            {
                double v;
                lp[Phi] -= v = (lp[Phi] + Math.Sin(lp[Phi]) - k)/
                               (1 + Math.Cos(lp[Phi]));
                if (Math.Abs(v) < LoopTOL)
                    break;
            }
            if (i == 0)
                lp[Phi] = (lp[Phi] < 0) ? -HalfPi : HalfPi;
            else
                lp[Phi] *= 0.5;
            xy[X] = 0.5*lp[Lambda]*(Math.Cos(lp[Phi]) + _cosphi1);
            xy[Y] = FortPi*(Math.Sin(lp[Phi]) + xy[Y]);
        }

        #endregion

     



    }
}
