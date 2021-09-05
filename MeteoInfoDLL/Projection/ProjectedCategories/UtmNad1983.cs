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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:08:05 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// UtmNad1983
    /// </summary>
    public class UtmNad1983 : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1983UTMZone10N;
        public readonly ProjectionInfo NAD1983UTMZone11N;
        public readonly ProjectionInfo NAD1983UTMZone12N;
        public readonly ProjectionInfo NAD1983UTMZone13N;
        public readonly ProjectionInfo NAD1983UTMZone14N;
        public readonly ProjectionInfo NAD1983UTMZone15N;
        public readonly ProjectionInfo NAD1983UTMZone16N;
        public readonly ProjectionInfo NAD1983UTMZone17N;
        public readonly ProjectionInfo NAD1983UTMZone18N;
        public readonly ProjectionInfo NAD1983UTMZone19N;
        public readonly ProjectionInfo NAD1983UTMZone1N;
        public readonly ProjectionInfo NAD1983UTMZone20N;
        public readonly ProjectionInfo NAD1983UTMZone21N;
        public readonly ProjectionInfo NAD1983UTMZone22N;
        public readonly ProjectionInfo NAD1983UTMZone23N;
        public readonly ProjectionInfo NAD1983UTMZone2N;
        public readonly ProjectionInfo NAD1983UTMZone3N;
        public readonly ProjectionInfo NAD1983UTMZone4N;
        public readonly ProjectionInfo NAD1983UTMZone59N;
        public readonly ProjectionInfo NAD1983UTMZone5N;
        public readonly ProjectionInfo NAD1983UTMZone60N;
        public readonly ProjectionInfo NAD1983UTMZone6N;
        public readonly ProjectionInfo NAD1983UTMZone7N;
        public readonly ProjectionInfo NAD1983UTMZone8N;
        public readonly ProjectionInfo NAD1983UTMZone9N;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of UtmNad1983
        /// </summary>
        public UtmNad1983()
        {
            NAD1983UTMZone10N = new ProjectionInfo("+proj=utm +zone=10 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone11N = new ProjectionInfo("+proj=utm +zone=11 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone12N = new ProjectionInfo("+proj=utm +zone=12 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone13N = new ProjectionInfo("+proj=utm +zone=13 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone14N = new ProjectionInfo("+proj=utm +zone=14 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone15N = new ProjectionInfo("+proj=utm +zone=15 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone16N = new ProjectionInfo("+proj=utm +zone=16 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone17N = new ProjectionInfo("+proj=utm +zone=17 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone1N = new ProjectionInfo("+proj=utm +zone=1 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone22N = new ProjectionInfo("+proj=utm +zone=22 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone23N = new ProjectionInfo("+proj=utm +zone=23 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone2N = new ProjectionInfo("+proj=utm +zone=2 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone3N = new ProjectionInfo("+proj=utm +zone=3 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone4N = new ProjectionInfo("+proj=utm +zone=4 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone59N = new ProjectionInfo("+proj=utm +zone=59 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone5N = new ProjectionInfo("+proj=utm +zone=5 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone60N = new ProjectionInfo("+proj=utm +zone=60 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone6N = new ProjectionInfo("+proj=utm +zone=6 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone7N = new ProjectionInfo("+proj=utm +zone=7 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone8N = new ProjectionInfo("+proj=utm +zone=8 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983UTMZone9N = new ProjectionInfo("+proj=utm +zone=9 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591