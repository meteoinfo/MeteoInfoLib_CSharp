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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/24/2009 1:28:49 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// UniversalTransverseMercator
    /// </summary>
    public class UniversalTransverseMercator : TransverseMercator
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of UniversalTransverseMercator.  The only difference
        /// for this one is that the proj4 name is a little different and when reading
        /// from proj4, we have to parse the zone and south parameters in order to
        /// create the transverse mercator projection.
        /// </summary>
        public UniversalTransverseMercator()
        {
            Proj4Name = "utm";
            ProjectionName = ProjectionNames.Universal_Transverse_Mercator;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            if (projInfo.IsSouth)
            {
                Y0 = 10000000;
                X0 = 500000;
                int zone;
                if (projInfo.Zone != null)
                {
                    zone = projInfo.Zone.Value;
                    if (zone <= 0 || zone > 60) throw new ProjectionException(35);
                    zone -= 1;
                }
                else
                {
                    zone = (int)Math.Floor((Proj.Adjlon(Lam0) + Math.PI) * 30 / Math.PI);
                    if (zone < 0) zone = 0;
                    if (zone >= 60) zone = 59;

                }
                Lam0 = (zone + .5) * Math.PI / 30 - Math.PI;
                K0 = 0.9996;
                Phi0 = 0;
            }

            base.OnInit(projInfo);
        }

        #endregion

        

    }
}
