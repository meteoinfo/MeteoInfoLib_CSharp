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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:49:00 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// IndianSubcontinent
    /// </summary>
    public class NationalGridsIndia : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Kalianpur1880IndiaZone0;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneI;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneIIa;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneIIb;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneIII;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneIV;
        public readonly ProjectionInfo Kalianpur1937IndiaZoneIIb;
        public readonly ProjectionInfo Kalianpur1937UTMZone45N;
        public readonly ProjectionInfo Kalianpur1937UTMZone46N;
        public readonly ProjectionInfo Kalianpur1962IndiaZoneI;
        public readonly ProjectionInfo Kalianpur1962IndiaZoneIIa;
        public readonly ProjectionInfo Kalianpur1962UTMZone41N;
        public readonly ProjectionInfo Kalianpur1962UTMZone42N;
        public readonly ProjectionInfo Kalianpur1962UTMZone43N;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneI;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneIIa;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneIIb;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneIII;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneIV;
        public readonly ProjectionInfo Kalianpur1975UTMZone42N;
        public readonly ProjectionInfo Kalianpur1975UTMZone43N;
        public readonly ProjectionInfo Kalianpur1975UTMZone44N;
        public readonly ProjectionInfo Kalianpur1975UTMZone45N;
        public readonly ProjectionInfo Kalianpur1975UTMZone46N;
        public readonly ProjectionInfo Kalianpur1975UTMZone47N;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of IndianSubcontinent
        /// </summary>
        public NationalGridsIndia()
        {
            Kalianpur1880IndiaZone0 = new ProjectionInfo("+proj=lcc +lat_1=39.5 +lat_0=39.5 +lon_0=68 +k_0=0.99846154 +x_0=2153865.73916853 +y_0=2368292.194628102 +a=6377299.36 +b=6356098.35162804 +to_meter=0.9143985307444408 +no_defs ");
            Kalianpur1880IndiaZoneI = new ProjectionInfo("+proj=lcc +lat_1=32.5 +lat_0=32.5 +lon_0=68 +k_0=0.99878641 +x_0=2743195.592233322 +y_0=914398.5307444407 +a=6377299.36 +b=6356098.35162804 +to_meter=0.9143985307444408 +no_defs ");
            Kalianpur1880IndiaZoneIIa = new ProjectionInfo("+proj=lcc +lat_1=26 +lat_0=26 +lon_0=74 +k_0=0.99878641 +x_0=2743195.592233322 +y_0=914398.5307444407 +a=6377299.36 +b=6356098.35162804 +to_meter=0.9143985307444408 +no_defs ");
            Kalianpur1880IndiaZoneIIb = new ProjectionInfo("+proj=lcc +lat_1=26 +lat_0=26 +lon_0=90 +k_0=0.99878641 +x_0=2743195.592233322 +y_0=914398.5307444407 +a=6377299.36 +b=6356098.35162804 +to_meter=0.9143985307444408 +no_defs ");
            Kalianpur1880IndiaZoneIII = new ProjectionInfo("+proj=lcc +lat_1=19 +lat_0=19 +lon_0=80 +k_0=0.99878641 +x_0=2743195.592233322 +y_0=914398.5307444407 +a=6377299.36 +b=6356098.35162804 +to_meter=0.9143985307444408 +no_defs ");
            Kalianpur1880IndiaZoneIV = new ProjectionInfo("+proj=lcc +lat_1=12 +lat_0=12 +lon_0=80 +k_0=0.99878641 +x_0=2743195.592233322 +y_0=914398.5307444407 +a=6377299.36 +b=6356098.35162804 +to_meter=0.9143985307444408 +no_defs ");
            Kalianpur1937IndiaZoneIIb = new ProjectionInfo("+proj=lcc +lat_1=26 +lat_0=26 +lon_0=90 +k_0=0.99878641 +x_0=2743195.5 +y_0=914398.5 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Kalianpur1937UTMZone45N = new ProjectionInfo("+proj=utm +zone=45 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Kalianpur1937UTMZone46N = new ProjectionInfo("+proj=utm +zone=46 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Kalianpur1962IndiaZoneI = new ProjectionInfo("+proj=lcc +lat_1=32.5 +lat_0=32.5 +lon_0=68 +k_0=0.99878641 +x_0=2743196.4 +y_0=914398.8000000001 +a=6377301.243 +b=6356100.230165384 +units=m +no_defs ");
            Kalianpur1962IndiaZoneIIa = new ProjectionInfo("+proj=lcc +lat_1=26 +lat_0=26 +lon_0=74 +k_0=0.99878641 +x_0=2743196.4 +y_0=914398.8000000001 +a=6377301.243 +b=6356100.230165384 +units=m +no_defs ");
            Kalianpur1962UTMZone41N = new ProjectionInfo("+proj=utm +zone=41 +a=6377301.243 +b=6356100.230165384 +units=m +no_defs ");
            Kalianpur1962UTMZone42N = new ProjectionInfo("+proj=utm +zone=42 +a=6377301.243 +b=6356100.230165384 +units=m +no_defs ");
            Kalianpur1962UTMZone43N = new ProjectionInfo("+proj=utm +zone=43 +a=6377301.243 +b=6356100.230165384 +units=m +no_defs ");
            Kalianpur1975IndiaZoneI = new ProjectionInfo("+proj=lcc +lat_1=32.5 +lat_0=32.5 +lon_0=68 +k_0=0.99878641 +x_0=2743185.69 +y_0=914395.23 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975IndiaZoneIIa = new ProjectionInfo("+proj=lcc +lat_1=26 +lat_0=26 +lon_0=74 +k_0=0.99878641 +x_0=2743185.69 +y_0=914395.23 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975IndiaZoneIIb = new ProjectionInfo("+proj=lcc +lat_1=26 +lat_0=26 +lon_0=90 +k_0=0.99878641 +x_0=2743185.69 +y_0=914395.23 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975IndiaZoneIII = new ProjectionInfo("+proj=lcc +lat_1=19 +lat_0=19 +lon_0=80 +k_0=0.99878641 +x_0=2743185.69 +y_0=914395.23 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975IndiaZoneIV = new ProjectionInfo("+proj=lcc +lat_1=12 +lat_0=12 +lon_0=80 +k_0=0.99878641 +x_0=2743185.69 +y_0=914395.23 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975UTMZone42N = new ProjectionInfo("+proj=utm +zone=42 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975UTMZone43N = new ProjectionInfo("+proj=utm +zone=43 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975UTMZone44N = new ProjectionInfo("+proj=utm +zone=44 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975UTMZone45N = new ProjectionInfo("+proj=utm +zone=45 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975UTMZone46N = new ProjectionInfo("+proj=utm +zone=46 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");
            Kalianpur1975UTMZone47N = new ProjectionInfo("+proj=utm +zone=47 +a=6377299.151 +b=6356098.145120132 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591