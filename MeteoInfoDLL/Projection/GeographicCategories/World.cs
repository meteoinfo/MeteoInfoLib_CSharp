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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:21:38 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.GeographicCategories
{


    /// <summary>
    /// World
    /// </summary>
    public class World : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo GRS1980;
        public readonly ProjectionInfo ITRF1988;
        public readonly ProjectionInfo ITRF1989;
        public readonly ProjectionInfo ITRF1990;
        public readonly ProjectionInfo ITRF1991;
        public readonly ProjectionInfo ITRF1992;
        public readonly ProjectionInfo ITRF1993;
        public readonly ProjectionInfo ITRF1994;
        public readonly ProjectionInfo ITRF1996;
        public readonly ProjectionInfo ITRF1997;
        public readonly ProjectionInfo ITRF2000;
        public readonly ProjectionInfo NSWC9Z2;
        public readonly ProjectionInfo WGS1966;
        public readonly ProjectionInfo WGS1972;
        public readonly ProjectionInfo WGS1972TBE;
        public readonly ProjectionInfo WGS1984;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of World
        /// </summary>
        public World()
        {
            GRS1980 = new ProjectionInfo("+proj=longlat +ellps=WGS84 +datum=GRS80 +no_defs ");
            ITRF1988 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1989 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1990 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1991 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1992 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1993 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1994 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1996 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF1997 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            ITRF2000 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            NSWC9Z2 = new ProjectionInfo("+proj=longlat +ellps=WGS66 +no_defs ");
            WGS1966 = new ProjectionInfo("+proj=longlat +ellps=WGS66 +no_defs ");
            WGS1972 = new ProjectionInfo("+proj=longlat +ellps=WGS72 +no_defs ");
            WGS1972TBE = new ProjectionInfo("+proj=longlat +ellps=WGS72 +no_defs ");
            WGS1984 = new ProjectionInfo("+proj=longlat +ellps=WGS84 +datum=WGS84 +no_defs ");            

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591