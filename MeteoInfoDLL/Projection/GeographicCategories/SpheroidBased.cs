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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 2:20:30 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.GeographicCategories
{


    /// <summary>
    /// SpheroidBased
    /// </summary>
    public class SpheroidBased : CoordinateSystemCategory
    {
        #region Fields

        public readonly ProjectionInfo Airy1830;
        public readonly ProjectionInfo Airymodified;
        public readonly ProjectionInfo AustralianNational;
        public readonly ProjectionInfo Authalicsphere;
        public readonly ProjectionInfo AuthalicsphereARCINFO;
        public readonly ProjectionInfo AverageTerrestrialSystem1977;
        public readonly ProjectionInfo Bessel1841;
        public readonly ProjectionInfo Besselmodified;
        public readonly ProjectionInfo BesselNamibia;
        public readonly ProjectionInfo Clarke1858;
        public readonly ProjectionInfo Clarke1866;
        public readonly ProjectionInfo Clarke1866Michigan;
        public readonly ProjectionInfo Clarke1880;
        public readonly ProjectionInfo Clarke1880Arc;
        public readonly ProjectionInfo Clarke1880Benoit;
        public readonly ProjectionInfo Clarke1880IGN;
        public readonly ProjectionInfo Clarke1880RGS;
        public readonly ProjectionInfo Clarke1880SGA;
        public readonly ProjectionInfo Everestdefinition1967;
        public readonly ProjectionInfo Everestdefinition1975;
        public readonly ProjectionInfo Everest1830;
        public readonly ProjectionInfo Everestmodified;
        public readonly ProjectionInfo Everestmodified1969;
        public readonly ProjectionInfo Fischer1960;
        public readonly ProjectionInfo Fischer1968;
        public readonly ProjectionInfo Fischermodified;
        public readonly ProjectionInfo GRS1967;
        public readonly ProjectionInfo GRS1980;
        public readonly ProjectionInfo Helmert1906;
        public readonly ProjectionInfo Hough1960;
        public readonly ProjectionInfo IndonesianNational;
        public readonly ProjectionInfo International1924;
        public readonly ProjectionInfo International1967;
        public readonly ProjectionInfo Krasovsky1940;
        public readonly ProjectionInfo OSU1986geoidalmodel;
        public readonly ProjectionInfo OSU1991geoidalmodel;
        public readonly ProjectionInfo Plessis1817;
        public readonly ProjectionInfo SphereEMEP;
        public readonly ProjectionInfo Struve1860;
        public readonly ProjectionInfo Transitpreciseephemeris;
        public readonly ProjectionInfo Walbeck;
        public readonly ProjectionInfo WarOffice;
        public readonly ProjectionInfo WGS1966;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SpheroidBased
        /// </summary>
        public SpheroidBased()
        {

            Airy1830 = new ProjectionInfo("+proj=longlat +ellps=airy +no_defs ");
            Airymodified = new ProjectionInfo("+proj=longlat +a=6377340.189 +b=6356034.447938534 +no_defs ");
            AustralianNational = new ProjectionInfo("+proj=longlat +ellps=aust_SA +no_defs ");
            Authalicsphere = new ProjectionInfo("+proj=longlat +a=6371000 +b=6371000 +no_defs ");
            AuthalicsphereARCINFO = new ProjectionInfo("+proj=longlat +a=6370997 +b=6370997 +no_defs ");
            AverageTerrestrialSystem1977 = new ProjectionInfo("+proj=longlat +a=6378135 +b=6356750.304921594 +no_defs ");
            Bessel1841 = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            Besselmodified = new ProjectionInfo("+proj=longlat +a=6377492.018 +b=6356173.508712696 +no_defs ");
            BesselNamibia = new ProjectionInfo("+proj=longlat +ellps=bess_nam +no_defs ");
            Clarke1858 = new ProjectionInfo("+proj=longlat +a=6378293.639 +b=6356617.98149216 +no_defs ");
            Clarke1866 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Clarke1866Michigan = new ProjectionInfo("+proj=longlat +a=6378450.047 +b=6356826.620025999 +no_defs ");
            Clarke1880 = new ProjectionInfo("+proj=longlat +a=6378249.138 +b=6356514.959419348 +no_defs ");
            Clarke1880Arc = new ProjectionInfo("+proj=longlat +a=6378249.145 +b=6356514.966395495 +no_defs ");
            Clarke1880Benoit = new ProjectionInfo("+proj=longlat +a=6378300.79 +b=6356566.430000036 +no_defs ");
            Clarke1880IGN = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Clarke1880RGS = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Clarke1880SGA = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.996941779 +no_defs ");
            Everestdefinition1967 = new ProjectionInfo("+proj=longlat +ellps=evrstSS +no_defs ");
            Everestdefinition1975 = new ProjectionInfo("+proj=longlat +a=6377301.243 +b=6356100.228368102 +no_defs ");
            Everest1830 = new ProjectionInfo("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            Everestmodified = new ProjectionInfo("+proj=longlat +a=6377304.063 +b=6356103.041812424 +no_defs ");
            Everestmodified1969 = new ProjectionInfo("+proj=longlat +a=6377295.664 +b=6356094.667915204 +no_defs ");
            Fischer1960 = new ProjectionInfo("+proj=longlat +a=6378166 +b=6356784.283607107 +no_defs ");
            Fischer1968 = new ProjectionInfo("+proj=longlat +a=6378150 +b=6356768.337244385 +no_defs ");
            Fischermodified = new ProjectionInfo("+proj=longlat +ellps=fschr60m +no_defs ");
            GRS1967 = new ProjectionInfo("+proj=longlat +ellps=GRS67 +no_defs ");
            GRS1980 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            Helmert1906 = new ProjectionInfo("+proj=longlat +ellps=helmert +no_defs ");
            Hough1960 = new ProjectionInfo("+proj=longlat +a=6378270 +b=6356794.343434343 +no_defs ");
            IndonesianNational = new ProjectionInfo("+proj=longlat +a=6378160 +b=6356774.50408554 +no_defs ");
            International1924 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            International1967 = new ProjectionInfo("+proj=longlat +ellps=aust_SA +no_defs ");
            Krasovsky1940 = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            OSU1986geoidalmodel = new ProjectionInfo("+proj=longlat +a=6378136.2 +b=6356751.516671965 +no_defs ");
            OSU1991geoidalmodel = new ProjectionInfo("+proj=longlat +a=6378136.3 +b=6356751.616336684 +no_defs ");
            Plessis1817 = new ProjectionInfo("+proj=longlat +a=6376523 +b=6355862.933255573 +no_defs ");
            SphereEMEP = new ProjectionInfo("+proj=longlat +a=6370000 +b=6370000 +no_defs ");
            Struve1860 = new ProjectionInfo("+proj=longlat +a=6378297 +b=6356655.847080379 +no_defs ");
            Transitpreciseephemeris = new ProjectionInfo("+proj=longlat +ellps=WGS66 +no_defs ");
            Walbeck = new ProjectionInfo("+proj=longlat +a=6376896 +b=6355834.846687364 +no_defs ");
            WarOffice = new ProjectionInfo("+proj=longlat +a=6378300.583 +b=6356752.270219594 +no_defs ");
            WGS1966 = new ProjectionInfo("+proj=longlat +ellps=WGS66 +no_defs ");


        }

        #endregion

     



    }
}
#pragma warning restore 1591