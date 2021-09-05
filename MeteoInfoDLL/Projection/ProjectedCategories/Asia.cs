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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:23:38 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591

namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// Asia
    /// </summary>
    public class Asia : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AsiaLambertConformalConic;
        public readonly ProjectionInfo AsiaNorthAlbersEqualAreaConic;
        public readonly ProjectionInfo AsiaNorthEquidistantConic;
        public readonly ProjectionInfo AsiaNorthLambertConformalConic;
        public readonly ProjectionInfo AsiaSouthAlbersEqualAreaConic;
        public readonly ProjectionInfo AsiaSouthEquidistantConic;
        public readonly ProjectionInfo AsiaSouthLambertConformalConic;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Asia
        /// </summary>
        public Asia()
        {
            AsiaLambertConformalConic = new ProjectionInfo("+proj=lcc +lat_1=30 +lat_2=62 +lat_0=0 +lon_0=105 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AsiaNorthAlbersEqualAreaConic = new ProjectionInfo("+proj=aea +lat_1=15 +lat_2=65 +lat_0=30 +lon_0=95 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AsiaNorthEquidistantConic = new ProjectionInfo("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=15 +lat_2=65 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AsiaNorthLambertConformalConic = new ProjectionInfo("+proj=lcc +lat_1=15 +lat_2=65 +lat_0=30 +lon_0=95 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AsiaSouthAlbersEqualAreaConic = new ProjectionInfo("+proj=aea +lat_1=7 +lat_2=-32 +lat_0=-15 +lon_0=125 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AsiaSouthEquidistantConic = new ProjectionInfo("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=7 +lat_2=-32 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            AsiaSouthLambertConformalConic = new ProjectionInfo("+proj=lcc +lat_1=7 +lat_2=-32 +lat_0=-15 +lon_0=125 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591