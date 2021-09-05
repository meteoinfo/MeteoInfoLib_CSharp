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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:07:15 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// UtmNad1927
    /// </summary>
    public class UtmNad1927 : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1927UTMZone10N;
        public readonly ProjectionInfo NAD1927UTMZone11N;
        public readonly ProjectionInfo NAD1927UTMZone12N;
        public readonly ProjectionInfo NAD1927UTMZone13N;
        public readonly ProjectionInfo NAD1927UTMZone14N;
        public readonly ProjectionInfo NAD1927UTMZone15N;
        public readonly ProjectionInfo NAD1927UTMZone16N;
        public readonly ProjectionInfo NAD1927UTMZone17N;
        public readonly ProjectionInfo NAD1927UTMZone18N;
        public readonly ProjectionInfo NAD1927UTMZone19N;
        public readonly ProjectionInfo NAD1927UTMZone1N;
        public readonly ProjectionInfo NAD1927UTMZone20N;
        public readonly ProjectionInfo NAD1927UTMZone21N;
        public readonly ProjectionInfo NAD1927UTMZone22N;
        public readonly ProjectionInfo NAD1927UTMZone2N;
        public readonly ProjectionInfo NAD1927UTMZone3N;
        public readonly ProjectionInfo NAD1927UTMZone4N;
        public readonly ProjectionInfo NAD1927UTMZone59N;
        public readonly ProjectionInfo NAD1927UTMZone5N;
        public readonly ProjectionInfo NAD1927UTMZone60N;
        public readonly ProjectionInfo NAD1927UTMZone6N;
        public readonly ProjectionInfo NAD1927UTMZone7N;
        public readonly ProjectionInfo NAD1927UTMZone8N;
        public readonly ProjectionInfo NAD1927UTMZone9N;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of UtmNad1927
        /// </summary>
        public UtmNad1927()
        {
            NAD1927UTMZone10N = new ProjectionInfo("+proj=utm +zone=10 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone11N = new ProjectionInfo("+proj=utm +zone=11 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone12N = new ProjectionInfo("+proj=utm +zone=12 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone13N = new ProjectionInfo("+proj=utm +zone=13 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone14N = new ProjectionInfo("+proj=utm +zone=14 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone15N = new ProjectionInfo("+proj=utm +zone=15 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone16N = new ProjectionInfo("+proj=utm +zone=16 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone17N = new ProjectionInfo("+proj=utm +zone=17 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone1N = new ProjectionInfo("+proj=utm +zone=1 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone22N = new ProjectionInfo("+proj=utm +zone=22 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone2N = new ProjectionInfo("+proj=utm +zone=2 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone3N = new ProjectionInfo("+proj=utm +zone=3 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone4N = new ProjectionInfo("+proj=utm +zone=4 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone59N = new ProjectionInfo("+proj=utm +zone=59 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone5N = new ProjectionInfo("+proj=utm +zone=5 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone60N = new ProjectionInfo("+proj=utm +zone=60 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone6N = new ProjectionInfo("+proj=utm +zone=6 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone7N = new ProjectionInfo("+proj=utm +zone=7 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone8N = new ProjectionInfo("+proj=utm +zone=8 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927UTMZone9N = new ProjectionInfo("+proj=utm +zone=9 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591