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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:04:52 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.GeographicCategories
{


    /// <summary>
    /// Asia
    /// </summary>
    public class Asia : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AinelAbd1970;
        public readonly ProjectionInfo Batavia;
        public readonly ProjectionInfo BataviaJakarta;
        public readonly ProjectionInfo Beijing1954;
        public readonly ProjectionInfo BukitRimpah;
        public readonly ProjectionInfo DeirezZor;
        public readonly ProjectionInfo European1950ED77;
        public readonly ProjectionInfo EuropeanDatum1950;
        public readonly ProjectionInfo EverestBangladesh;
        public readonly ProjectionInfo EverestIndiaandNepal;
        public readonly ProjectionInfo Everestdef1962;
        public readonly ProjectionInfo Everestdef1967;
        public readonly ProjectionInfo Everestdef1975;
        public readonly ProjectionInfo Everest1830;
        public readonly ProjectionInfo EverestModified;
        public readonly ProjectionInfo Fahud;
        public readonly ProjectionInfo FD1958;
        public readonly ProjectionInfo Gandajika1970;
        public readonly ProjectionInfo GunungSegara;
        public readonly ProjectionInfo GunungSegaraJakarta;
        public readonly ProjectionInfo Hanoi1972;
        public readonly ProjectionInfo HeratNorth;
        public readonly ProjectionInfo HongKong1963;
        public readonly ProjectionInfo HongKong1980;
        public readonly ProjectionInfo HuTzuShan;
        public readonly ProjectionInfo IGM1995;
        public readonly ProjectionInfo IKBD1992;
        public readonly ProjectionInfo Indian1954;
        public readonly ProjectionInfo Indian1960;
        public readonly ProjectionInfo Indian1975;
        public readonly ProjectionInfo IndonesianDatum1974;
        public readonly ProjectionInfo Israel;
        public readonly ProjectionInfo JGD2000;
        public readonly ProjectionInfo Jordan;
        public readonly ProjectionInfo Kalianpur1880;
        public readonly ProjectionInfo Kalianpur1937;
        public readonly ProjectionInfo Kalianpur1962;
        public readonly ProjectionInfo Kalianpur1975;
        public readonly ProjectionInfo Kandawala;
        public readonly ProjectionInfo Kertau;
        public readonly ProjectionInfo KoreanDatum1985;
        public readonly ProjectionInfo KoreanDatum1995;
        public readonly ProjectionInfo KuwaitOilCompany;
        public readonly ProjectionInfo KuwaitUtility;
        public readonly ProjectionInfo Luzon1911;
        public readonly ProjectionInfo Makassar;
        public readonly ProjectionInfo MakassarJakarta;
        public readonly ProjectionInfo Nahrwan1967;
        public readonly ProjectionInfo NationalGeodeticNetworkKuwait;
        public readonly ProjectionInfo ObservatorioMeteorologico1965;
        public readonly ProjectionInfo Oman;
        public readonly ProjectionInfo Padang1884;
        public readonly ProjectionInfo Padang1884Jakarta;
        public readonly ProjectionInfo Palestine1923;
        public readonly ProjectionInfo Pulkovo1942;
        public readonly ProjectionInfo Pulkovo1995;
        public readonly ProjectionInfo Qatar;
        public readonly ProjectionInfo Qatar1948;
        public readonly ProjectionInfo QND1995;
        public readonly ProjectionInfo Rassadiran;
        public readonly ProjectionInfo Samboja;
        public readonly ProjectionInfo Segora;
        public readonly ProjectionInfo Serindung;
        public readonly ProjectionInfo SouthAsiaSingapore;
        public readonly ProjectionInfo Timbalai1948;
        public readonly ProjectionInfo Tokyo;
        public readonly ProjectionInfo TrucialCoast1948;
        public readonly ProjectionInfo Xian1980;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Asia
        /// </summary>
        public Asia()
        {
            AinelAbd1970 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Batavia = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            BataviaJakarta = new ProjectionInfo("+proj=longlat +ellps=bessel +pm=106.8077194444444 +no_defs ");
            Beijing1954 = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            BukitRimpah = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            DeirezZor = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            European1950ED77 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            EuropeanDatum1950 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            EverestBangladesh = new ProjectionInfo("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            EverestIndiaandNepal = new ProjectionInfo("+proj=longlat +a=6377301.243 +b=6356100.230165384 +no_defs ");
            Everestdef1962 = new ProjectionInfo("+proj=longlat +a=6377301.243 +b=6356100.230165384 +no_defs ");
            Everestdef1967 = new ProjectionInfo("+proj=longlat +ellps=evrstSS +no_defs ");
            Everestdef1975 = new ProjectionInfo("+proj=longlat +a=6377299.151 +b=6356098.145120132 +no_defs ");
            Everest1830 = new ProjectionInfo("+proj=longlat +a=6377299.36 +b=6356098.35162804 +no_defs ");
            EverestModified = new ProjectionInfo("+proj=longlat +a=6377304.063 +b=6356103.041812424 +no_defs ");
            Fahud = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            FD1958 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Gandajika1970 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            GunungSegara = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            GunungSegaraJakarta = new ProjectionInfo("+proj=longlat +ellps=bessel +pm=106.8077194444444 +no_defs ");
            Hanoi1972 = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            HeratNorth = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            HongKong1963 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            HongKong1980 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            HuTzuShan = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            IGM1995 = new ProjectionInfo("+proj=longlat +ellps=WGS84 +no_defs ");
            IKBD1992 = new ProjectionInfo("+proj=longlat +ellps=WGS84 +no_defs ");
            Indian1954 = new ProjectionInfo("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            Indian1960 = new ProjectionInfo("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            Indian1975 = new ProjectionInfo("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            IndonesianDatum1974 = new ProjectionInfo("+proj=longlat +a=6378160 +b=6356774.50408554 +no_defs ");
            Israel = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            JGD2000 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            Jordan = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Kalianpur1880 = new ProjectionInfo("+proj=longlat +a=6377299.36 +b=6356098.35162804 +no_defs ");
            Kalianpur1937 = new ProjectionInfo("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            Kalianpur1962 = new ProjectionInfo("+proj=longlat +a=6377301.243 +b=6356100.230165384 +no_defs ");
            Kalianpur1975 = new ProjectionInfo("+proj=longlat +a=6377299.151 +b=6356098.145120132 +no_defs ");
            Kandawala = new ProjectionInfo("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            Kertau = new ProjectionInfo("+proj=longlat +a=6377304.063 +b=6356103.038993155 +no_defs ");
            KoreanDatum1985 = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            KoreanDatum1995 = new ProjectionInfo("+proj=longlat +ellps=WGS84 +no_defs ");
            KuwaitOilCompany = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            KuwaitUtility = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            Luzon1911 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Makassar = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            MakassarJakarta = new ProjectionInfo("+proj=longlat +ellps=bessel +pm=106.8077194444444 +no_defs ");
            Nahrwan1967 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            NationalGeodeticNetworkKuwait = new ProjectionInfo("+proj=longlat +ellps=WGS84 +no_defs ");
            ObservatorioMeteorologico1965 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Oman = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Padang1884 = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            Padang1884Jakarta = new ProjectionInfo("+proj=longlat +ellps=bessel +pm=106.8077194444444 +no_defs ");
            Palestine1923 = new ProjectionInfo("+proj=longlat +a=6378300.79 +b=6356566.430000036 +no_defs ");
            Pulkovo1942 = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            Pulkovo1995 = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            Qatar = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Qatar1948 = new ProjectionInfo("+proj=longlat +ellps=helmert +no_defs ");
            QND1995 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Rassadiran = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Samboja = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            Segora = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            Serindung = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            SouthAsiaSingapore = new ProjectionInfo("+proj=longlat +ellps=fschr60m +no_defs ");
            Timbalai1948 = new ProjectionInfo("+proj=longlat +ellps=evrstSS +no_defs ");
            Tokyo = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            TrucialCoast1948 = new ProjectionInfo("+proj=longlat +ellps=helmert +no_defs ");
            Xian1980 = new ProjectionInfo("+proj=longlat +a=6378140 +b=6356755.288157528 +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591