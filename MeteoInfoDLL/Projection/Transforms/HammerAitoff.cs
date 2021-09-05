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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 1:16:49 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// HammerAitoff
    /// </summary>
    public class HammerAitoff : Transform
    {
        #region Private Variables

        private double _w;
        private double _m;
        private double _rm;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of HammerAitoff
        /// </summary>
        public HammerAitoff()
        {
            Proj4Name = "hammer";
            Name = "Hammer_Aitoff";

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
            double cosphi;
	        double d = Math.Sqrt(2/(1+ (cosphi = Math.Cos(lp[Phi])) * Math.Cos(lp[Lambda] *= _w)));
	        xy[X] = _m * d * cosphi * Math.Sin(lp[Lambda]);
	        xy[Y] = _rm * d * Math.Sin(lp[Phi]);
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (projInfo.Parameters.ContainsKey("W"))
            {
                _w = projInfo.ParamD("W");
                if (_w <= 0) throw new ProjectionException(27);
            }
            else
            {
                _w = .5;
            }
            if(projInfo.Parameters.ContainsKey("M"))
            {
                _m = projInfo.ParamD("M");
                if(_m <= 0)throw new ProjectionException(27);
            }
            else
            {
                _m = 1;
            }
            _rm = 1/_m;
            _m /= _w;


        }

        #endregion

       


    }
}
