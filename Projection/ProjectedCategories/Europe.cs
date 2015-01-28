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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:24:27 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591

namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// Europe
    /// </summary>
    public class Europe : CoordinateSystemCategory
    {
        #region Private Variables
        public readonly ProjectionInfo EMEP150KilometerGrid;
        public readonly ProjectionInfo EMEP50KilometerGrid;
        public readonly ProjectionInfo ETRS1989LAEA;
        public readonly ProjectionInfo ETRS1989LCC;
        public readonly ProjectionInfo EuropeAlbersEqualAreaConic;
        public readonly ProjectionInfo EuropeEquidistantConic;
        public readonly ProjectionInfo EuropeLambertConformalConic;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Europe
        /// </summary>
        public Europe()
        {
            EMEP150KilometerGrid = new ProjectionInfo("+a=6370000 +b=6370000 +to_meter=150000 +no_defs ");
            EMEP50KilometerGrid = new ProjectionInfo("+a=6370000 +b=6370000 +to_meter=50000 +no_defs ");
            ETRS1989LAEA = new ProjectionInfo("+proj=laea +lat_0=52 +lon_0=10 +x_0=4321000 +y_0=3210000 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989LCC = new ProjectionInfo("+proj=lcc +lat_1=35 +lat_2=65 +lat_0=52 +lon_0=10 +x_0=4000000 +y_0=2800000 +ellps=GRS80 +units=m +no_defs ");
            EuropeAlbersEqualAreaConic = new ProjectionInfo("+proj=aea +lat_1=43 +lat_2=62 +lat_0=30 +lon_0=10 +x_0=0 +y_0=0 +ellps=intl +units=m +no_defs ");
            EuropeEquidistantConic = new ProjectionInfo("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=43 +lat_2=62 +x_0=0 +y_0=0 +ellps=intl +units=m +no_defs ");
            EuropeLambertConformalConic = new ProjectionInfo("+proj=lcc +lat_1=43 +lat_2=62 +lat_0=30 +lon_0=10 +x_0=0 +y_0=0 +ellps=intl +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591