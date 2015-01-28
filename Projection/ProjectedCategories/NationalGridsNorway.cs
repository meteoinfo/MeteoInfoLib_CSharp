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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:53:34 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// NationalGridsNorway
    /// </summary>
    public class NationalGridsNorway : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NGO1948BaerumKommune;
        public readonly ProjectionInfo NGO1948Bergenhalvoen;
        public readonly ProjectionInfo NGO1948NorwayZone1;
        public readonly ProjectionInfo NGO1948NorwayZone2;
        public readonly ProjectionInfo NGO1948NorwayZone3;
        public readonly ProjectionInfo NGO1948NorwayZone4;
        public readonly ProjectionInfo NGO1948NorwayZone5;
        public readonly ProjectionInfo NGO1948NorwayZone6;
        public readonly ProjectionInfo NGO1948NorwayZone7;
        public readonly ProjectionInfo NGO1948NorwayZone8;
        public readonly ProjectionInfo NGO1948OsloKommune;
        public readonly ProjectionInfo NGO1948OsloNorwayZone1;
        public readonly ProjectionInfo NGO1948OsloNorwayZone2;
        public readonly ProjectionInfo NGO1948OsloNorwayZone3;
        public readonly ProjectionInfo NGO1948OsloNorwayZone4;
        public readonly ProjectionInfo NGO1948OsloNorwayZone5;
        public readonly ProjectionInfo NGO1948OsloNorwayZone6;
        public readonly ProjectionInfo NGO1948OsloNorwayZone7;
        public readonly ProjectionInfo NGO1948OsloNorwayZone8;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NationalGridsNorway
        /// </summary>
        public NationalGridsNorway()
        {
            NGO1948BaerumKommune = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=10.72291666666667 +k=1.000000 +x_0=19999.32 +y_0=-202977.79 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948Bergenhalvoen = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=6.05625 +k=1.000000 +x_0=100000 +y_0=-200000 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone1 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=6.056249999999999 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone2 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=8.389583333333333 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone3 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=10.72291666666667 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone4 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=13.22291666666667 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone5 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=16.88958333333333 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone6 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=20.88958333333333 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone7 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=24.88958333333333 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948NorwayZone8 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=29.05625 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948OsloKommune = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=10.72291666666667 +k=1.000000 +x_0=0 +y_0=-212979.18 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948OsloNorwayZone1 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=-15.38958333333334 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone2 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=-13.05625 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone3 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=-10.72291666666667 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone4 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=-8.22291666666667 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone5 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=-4.556250000000003 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone6 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=-0.5562500000000004 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone7 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=3.44375 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");
            NGO1948OsloNorwayZone8 = new ProjectionInfo("+proj=tmerc +lat_0=58 +lon_0=7.610416666666659 +k=1.000000 +x_0=0 +y_0=0 +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +units=m +no_defs ");


        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591