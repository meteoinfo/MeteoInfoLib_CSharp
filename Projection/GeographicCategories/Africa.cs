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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 3:22:06 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.GeographicCategories
{


    /// <summary>
    /// Africa
    /// </summary>
    public class Africa : CoordinateSystemCategory
    {
        #region Private Variables

        /// <summary> 
        ///Abidjan 1987
        /// </summary>
        public readonly ProjectionInfo Abidjan1987;
        public readonly ProjectionInfo Accra;
        public readonly ProjectionInfo Adindan;
        public readonly ProjectionInfo Afgooye;
        public readonly ProjectionInfo Agadez;
        public readonly ProjectionInfo AinelAbd1970;
        public readonly ProjectionInfo Arc1950;
        public readonly ProjectionInfo Arc1960;
        public readonly ProjectionInfo AyabelleLighthouse;
        public readonly ProjectionInfo Beduaram;
        public readonly ProjectionInfo Bissau;
        public readonly ProjectionInfo Camacupa;
        public readonly ProjectionInfo Cape;
        public readonly ProjectionInfo Carthage;
        public readonly ProjectionInfo Carthagedegrees;
        public readonly ProjectionInfo CarthageParis;
        public readonly ProjectionInfo Conakry1905;
        public readonly ProjectionInfo CotedIvoire;
        public readonly ProjectionInfo Dabola;
        public readonly ProjectionInfo Douala;
        public readonly ProjectionInfo Douala1948;
        public readonly ProjectionInfo Egypt1907;
        public readonly ProjectionInfo Egypt1930;
        public readonly ProjectionInfo EuropeanDatum1950;
        public readonly ProjectionInfo EuropeanLibyanDatum1979;
        public readonly ProjectionInfo Garoua;
        public readonly ProjectionInfo Hartebeesthoek1994;
        public readonly ProjectionInfo Kousseri;
        public readonly ProjectionInfo KuwaitOilCompany;
        public readonly ProjectionInfo KuwaitUtility;
        public readonly ProjectionInfo Leigon;
        public readonly ProjectionInfo Liberia1964;
        public readonly ProjectionInfo Locodjo1965;
        public readonly ProjectionInfo Lome;
        public readonly ProjectionInfo Madzansua;
        public readonly ProjectionInfo Mahe1971;
        public readonly ProjectionInfo Malongo1987;
        public readonly ProjectionInfo Manoca;
        public readonly ProjectionInfo Manoca1962;
        public readonly ProjectionInfo Massawa;
        public readonly ProjectionInfo Merchich;
        public readonly ProjectionInfo Merchichdegrees;
        public readonly ProjectionInfo Mhast;
        public readonly ProjectionInfo Minna;
        public readonly ProjectionInfo Moznet;
        public readonly ProjectionInfo Mporaloko;
        public readonly ProjectionInfo Nahrwan1967;
        public readonly ProjectionInfo NationalGeodeticNetworkKuwait;
        public readonly ProjectionInfo NordSahara1959;
        public readonly ProjectionInfo NordSahara1959Paris;
        public readonly ProjectionInfo Observatario;
        public readonly ProjectionInfo Oman;
        public readonly ProjectionInfo Palestine1923;
        public readonly ProjectionInfo PDO1993;
        public readonly ProjectionInfo Point58;
        public readonly ProjectionInfo PointeNoire;
        public readonly ProjectionInfo Qatar;
        public readonly ProjectionInfo Qatar1948;
        public readonly ProjectionInfo Schwarzeck;
        public readonly ProjectionInfo SierraLeone1924;
        public readonly ProjectionInfo SierraLeone1960;
        public readonly ProjectionInfo SierraLeone1968;
        public readonly ProjectionInfo SouthYemen;
        public readonly ProjectionInfo Sudan;
        public readonly ProjectionInfo Tananarive1925;
        public readonly ProjectionInfo Tananarive1925Paris;
        public readonly ProjectionInfo Tete;
        public readonly ProjectionInfo TrucialCoast1948;
        public readonly ProjectionInfo Voirol1875;
        public readonly ProjectionInfo Voirol1875degrees;
        public readonly ProjectionInfo Voirol1875Paris;
        public readonly ProjectionInfo VoirolUnifie1960;
        public readonly ProjectionInfo VoirolUnifie1960degrees;
        public readonly ProjectionInfo VoirolUnifie1960Paris;
        public readonly ProjectionInfo YemenNGN1996;
        public readonly ProjectionInfo Yoff;


        



        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Africa
        /// </summary>
        public Africa()
        {
            Abidjan1987 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Accra = new ProjectionInfo("+proj=longlat +a=6378300 +b=6356751.689189189 +no_defs ");
            Adindan = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Afgooye = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            Agadez = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            AinelAbd1970 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Arc1950 = new ProjectionInfo("+proj=longlat +a=6378249.145 +b=6356514.966395495 +no_defs ");
            Arc1960 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            AyabelleLighthouse = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Beduaram = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Bissau = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Camacupa = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Cape = new ProjectionInfo("+proj=longlat +a=6378249.145 +b=6356514.966395495 +no_defs ");
            Carthage = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Carthagedegrees = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            CarthageParis = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +no_defs ");
            Conakry1905 = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            CotedIvoire = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Dabola = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Douala = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Douala1948 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Egypt1907 = new ProjectionInfo("+proj=longlat +ellps=helmert +no_defs ");
            Egypt1930 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            EuropeanDatum1950 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            EuropeanLibyanDatum1979 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Garoua = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Hartebeesthoek1994 = new ProjectionInfo("+proj=longlat +ellps=WGS84 +no_defs ");
            Kousseri = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            KuwaitOilCompany = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            KuwaitUtility = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            Leigon = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Liberia1964 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Locodjo1965 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Lome = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Madzansua = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Mahe1971 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Malongo1987 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Manoca = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Manoca1962 = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Massawa = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            Merchich = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Merchichdegrees = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Mhast = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Minna = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Moznet = new ProjectionInfo("+proj=longlat +ellps=WGS84 +no_defs ");
            Mporaloko = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Nahrwan1967 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            NationalGeodeticNetworkKuwait = new ProjectionInfo("+proj=longlat +ellps=WGS84 +no_defs ");
            NordSahara1959 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            NordSahara1959Paris = new ProjectionInfo("+proj=longlat +ellps=clrk80 +pm=2.337229166666667 +no_defs ");
            Observatario = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Oman = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Palestine1923 = new ProjectionInfo("+proj=longlat +a=6378300.79 +b=6356566.430000036 +no_defs ");
            PDO1993 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Point58 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            PointeNoire = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Qatar = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Qatar1948 = new ProjectionInfo("+proj=longlat +ellps=helmert +no_defs ");
            Schwarzeck = new ProjectionInfo("+proj=longlat +ellps=bess_nam +no_defs ");
            SierraLeone1924 = new ProjectionInfo("+proj=longlat +a=6378300 +b=6356751.689189189 +no_defs ");
            SierraLeone1960 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            SierraLeone1968 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            SouthYemen = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            Sudan = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Tananarive1925 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Tananarive1925Paris = new ProjectionInfo("+proj=longlat +ellps=intl +pm=2.337229166666667 +no_defs ");
            Tete = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            TrucialCoast1948 = new ProjectionInfo("+proj=longlat +ellps=helmert +no_defs ");
            Voirol1875 = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Voirol1875degrees = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Voirol1875Paris = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +no_defs ");
            VoirolUnifie1960 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            VoirolUnifie1960degrees = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            VoirolUnifie1960Paris = new ProjectionInfo("+proj=longlat +ellps=clrk80 +pm=2.337229166666667 +no_defs ");
            YemenNGN1996 = new ProjectionInfo("+proj=longlat +ellps=WGS84 +no_defs ");
            Yoff = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
#pragma warning restore 1591
}
