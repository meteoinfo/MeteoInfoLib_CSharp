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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:25:11 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// NorthAmerica
    /// </summary>
    public class NorthAmerica : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AlaskaAlbersEqualAreaConic;
        public readonly ProjectionInfo CanadaAlbersEqualAreaConic;
        public readonly ProjectionInfo CanadaLambertConformalConic;
        public readonly ProjectionInfo HawaiiAlbersEqualAreaConic;
        public readonly ProjectionInfo NorthAmericaAlbersEqualAreaConic;
        public readonly ProjectionInfo NorthAmericaEquidistantConic;
        public readonly ProjectionInfo NorthAmericaLambertConformalConic;
        public readonly ProjectionInfo USAContiguousAlbersEqualAreaConic;
        public readonly ProjectionInfo USAContiguousAlbersEqualAreaConicUSGS;
        public readonly ProjectionInfo USAContiguousEquidistantConic;
        public readonly ProjectionInfo USAContiguousLambertConformalConic;
       

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NorthAmerica
        /// </summary>
        public NorthAmerica()
        {
            AlaskaAlbersEqualAreaConic = new ProjectionInfo("+proj=aea +lat_1=55 +lat_2=65 +lat_0=50 +lon_0=-154 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            CanadaAlbersEqualAreaConic = new ProjectionInfo("+proj=aea +lat_1=50 +lat_2=70 +lat_0=40 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            CanadaLambertConformalConic = new ProjectionInfo("+proj=lcc +lat_1=50 +lat_2=70 +lat_0=40 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            HawaiiAlbersEqualAreaConic = new ProjectionInfo("+proj=aea +lat_1=8 +lat_2=18 +lat_0=13 +lon_0=-157 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NorthAmericaAlbersEqualAreaConic = new ProjectionInfo("+proj=aea +lat_1=20 +lat_2=60 +lat_0=40 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NorthAmericaEquidistantConic = new ProjectionInfo("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=20 +lat_2=60 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NorthAmericaLambertConformalConic = new ProjectionInfo("+proj=lcc +lat_1=20 +lat_2=60 +lat_0=40 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            USAContiguousAlbersEqualAreaConic = new ProjectionInfo("+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=37.5 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            USAContiguousAlbersEqualAreaConicUSGS = new ProjectionInfo("+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            USAContiguousEquidistantConic = new ProjectionInfo("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=33 +lat_2=45 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            USAContiguousLambertConformalConic = new ProjectionInfo("+proj=lcc +lat_1=33 +lat_2=45 +lat_0=39 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591