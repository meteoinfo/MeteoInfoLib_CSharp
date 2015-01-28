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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:03:57 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.GeographicCategories
{


    /// <summary>
    /// Antarctica
    /// </summary>
    public class Antarctica : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AustralianAntarctic1998;
        public readonly ProjectionInfo CampAreaAstro;
        public readonly ProjectionInfo DeceptionIsland;
        public readonly ProjectionInfo Petrels1972;
        public readonly ProjectionInfo PointeGeologiePerroud1950;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Antarctica
        /// </summary>
        public Antarctica()
        {
            AustralianAntarctic1998 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            CampAreaAstro = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            DeceptionIsland = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Petrels1972 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            PointeGeologiePerroud1950 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");


        }

        #endregion




    }

}
#pragma warning restore 1591
