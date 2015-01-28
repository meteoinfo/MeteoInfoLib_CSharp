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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:05:31 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// StateSystems
    /// </summary>
    public class StateSystems : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1927AlaskaAlbersFeet;
        public readonly ProjectionInfo NAD1927AlaskaAlbersMeters;
        public readonly ProjectionInfo NAD1927CaliforniaTealeAlbers;
        public readonly ProjectionInfo NAD1927GeorgiaStatewideAlbers;
        public readonly ProjectionInfo NAD1927TexasStatewideMappingSystem;
        public readonly ProjectionInfo NAD1983CaliforniaTealeAlbers;
        public readonly ProjectionInfo NAD1983GeorgiaStatewideLambert;
        public readonly ProjectionInfo NAD1983HARNOregonStatewideLambert;
        public readonly ProjectionInfo NAD1983HARNOregonStatewideLambertFeetIntl;
        public readonly ProjectionInfo NAD1983IdahoTM;
        public readonly ProjectionInfo NAD1983OregonStatewideLambert;
        public readonly ProjectionInfo NAD1983OregonStatewideLambertFeetIntl;
        public readonly ProjectionInfo NAD1983TexasCentricMappingSystemAlbers;
        public readonly ProjectionInfo NAD1983TexasCentricMappingSystemLambert;
        public readonly ProjectionInfo NAD1983TexasStatewideMappingSystem;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StateSystems
        /// </summary>
        public StateSystems()
        {
            NAD1927AlaskaAlbersFeet = new ProjectionInfo("+proj=aea +lat_1=55 +lat_2=65 +lat_0=50 +lon_0=-154 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927AlaskaAlbersMeters = new ProjectionInfo("+proj=aea +lat_1=55 +lat_2=65 +lat_0=50 +lon_0=-154 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927CaliforniaTealeAlbers = new ProjectionInfo("+proj=aea +lat_1=34 +lat_2=40.5 +lat_0=0 +lon_0=-120 +x_0=0 +y_0=-4000000 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927GeorgiaStatewideAlbers = new ProjectionInfo("+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-83.5 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927TexasStatewideMappingSystem = new ProjectionInfo("+proj=lcc +lat_1=27.41666666666667 +lat_2=34.91666666666666 +lat_0=31.16666666666667 +lon_0=-100 +x_0=914400 +y_0=914400 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048 +no_defs ");
            NAD1983CaliforniaTealeAlbers = new ProjectionInfo("+proj=aea +lat_1=34 +lat_2=40.5 +lat_0=0 +lon_0=-120 +x_0=0 +y_0=-4000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983GeorgiaStatewideLambert = new ProjectionInfo("+proj=lcc +lat_1=31.41666666666667 +lat_2=34.28333333333333 +lat_0=0 +lon_0=-83.5 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNOregonStatewideLambert = new ProjectionInfo("+proj=lcc +lat_1=43 +lat_2=45.5 +lat_0=41.75 +lon_0=-120.5 +x_0=400000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNOregonStatewideLambertFeetIntl = new ProjectionInfo("+proj=lcc +lat_1=43 +lat_2=45.5 +lat_0=41.75 +lon_0=-120.5 +x_0=399999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983IdahoTM = new ProjectionInfo("+proj=tmerc +lat_0=42 +lon_0=-114 +k=0.999600 +x_0=2000000 +y_0=3000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983OregonStatewideLambert = new ProjectionInfo("+proj=lcc +lat_1=43 +lat_2=45.5 +lat_0=41.75 +lon_0=-120.5 +x_0=400000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983OregonStatewideLambertFeetIntl = new ProjectionInfo("+proj=lcc +lat_1=43 +lat_2=45.5 +lat_0=41.75 +lon_0=-120.5 +x_0=399999.9999999999 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048 +no_defs ");
            NAD1983TexasCentricMappingSystemAlbers = new ProjectionInfo("+proj=aea +lat_1=27.5 +lat_2=35 +lat_0=18 +lon_0=-100 +x_0=1500000 +y_0=6000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983TexasCentricMappingSystemLambert = new ProjectionInfo("+proj=lcc +lat_1=27.5 +lat_2=35 +lat_0=18 +lon_0=-100 +x_0=1500000 +y_0=5000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983TexasStatewideMappingSystem = new ProjectionInfo("+proj=lcc +lat_1=27.41666666666667 +lat_2=34.91666666666666 +lat_0=31.16666666666667 +lon_0=-100 +x_0=1000000 +y_0=1000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591