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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:55:21 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591

namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// Polar
    /// </summary>
    public class Polar : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NorthPoleAzimuthalEquidistant;
        public readonly ProjectionInfo NorthPoleGnomonic;
        public readonly ProjectionInfo NorthPoleLambertAzimuthalEqualArea;
        public readonly ProjectionInfo NorthPoleOrthographic;
        public readonly ProjectionInfo NorthPoleStereographic;
        public readonly ProjectionInfo Perroud1950TerreAdeliePolarStereographic;
        public readonly ProjectionInfo Petrels1972TerreAdeliePolarStereographic;
        public readonly ProjectionInfo SouthPoleAzimuthalEquidistant;
        public readonly ProjectionInfo SouthPoleGnomonic;
        public readonly ProjectionInfo SouthPoleLambertAzimuthalEqualArea;
        public readonly ProjectionInfo SouthPoleOrthographic;
        public readonly ProjectionInfo SouthPoleStereographic;
        public readonly ProjectionInfo UPSNorth;
        public readonly ProjectionInfo UPSSouth;
        public readonly ProjectionInfo WGS1984AntarcticPolarStereographic;
        public readonly ProjectionInfo WGS1984AustralianAntarcticLambert;
        public readonly ProjectionInfo WGS1984AustralianAntarcticPolarStereographic;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Polar
        /// </summary>
        public Polar()
        {
            NorthPoleAzimuthalEquidistant = new ProjectionInfo("+proj=aeqd +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            NorthPoleGnomonic = new ProjectionInfo("+proj=gnom +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            NorthPoleLambertAzimuthalEqualArea = new ProjectionInfo("+proj=laea +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            NorthPoleOrthographic = new ProjectionInfo("+proj=ortho +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            NorthPoleStereographic = new ProjectionInfo("+proj=stere +lat_0=90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Perroud1950TerreAdeliePolarStereographic = new ProjectionInfo("+ellps=intl +units=m +no_defs ");
            Petrels1972TerreAdeliePolarStereographic = new ProjectionInfo("+ellps=intl +units=m +no_defs ");
            SouthPoleAzimuthalEquidistant = new ProjectionInfo("+proj=aeqd +lat_0=-90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            SouthPoleGnomonic = new ProjectionInfo("+proj=gnom +lat_0=-90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            SouthPoleLambertAzimuthalEqualArea = new ProjectionInfo("+proj=laea +lat_0=-90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            SouthPoleOrthographic = new ProjectionInfo("+proj=ortho +lat_0=-90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            SouthPoleStereographic = new ProjectionInfo("+proj=stere +lat_0=-90 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            UPSNorth = new ProjectionInfo("+proj=stere +lat_0=90 +lon_0=0 +x_0=2000000 +y_0=2000000 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            UPSSouth = new ProjectionInfo("+proj=stere +lat_0=-90 +lon_0=0 +x_0=2000000 +y_0=2000000 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984AntarcticPolarStereographic = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984AustralianAntarcticLambert = new ProjectionInfo("+proj=lcc +lat_1=-68.5 +lat_2=-74.5 +lat_0=-50 +lon_0=70 +x_0=6000000 +y_0=6000000 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984AustralianAntarcticPolarStereographic = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591