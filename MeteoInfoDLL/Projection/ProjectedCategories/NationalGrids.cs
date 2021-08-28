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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:43:09 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591

namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// NationalGrids
    /// </summary>
    public class NationalGrids : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Abidjan1987TM5NW;
        public readonly ProjectionInfo AccraGhanaGrid;
        public readonly ProjectionInfo AccraTM1NW;
        public readonly ProjectionInfo AinelAbdAramcoLambert;
        public readonly ProjectionInfo AmericanSamoa1962SamoaLambert;
        public readonly ProjectionInfo Anguilla1957BritishWestIndiesGrid;
        public readonly ProjectionInfo Antigua1943BritishWestIndiesGrid;
        public readonly ProjectionInfo ArgentinaZone1;
        public readonly ProjectionInfo ArgentinaZone2;
        public readonly ProjectionInfo ArgentinaZone3;
        public readonly ProjectionInfo ArgentinaZone4;
        public readonly ProjectionInfo ArgentinaZone5;
        public readonly ProjectionInfo ArgentinaZone6;
        public readonly ProjectionInfo ArgentinaZone7;
        public readonly ProjectionInfo AustriaFerroCentralZone;
        public readonly ProjectionInfo AustriaFerroEastZone;
        public readonly ProjectionInfo AustriaFerroWestZone;
        public readonly ProjectionInfo BahrainStateGrid;
        public readonly ProjectionInfo Barbados1938BarbadosGrid;
        public readonly ProjectionInfo Barbados1938BritishWestIndiesGrid;
        public readonly ProjectionInfo BataviaNEIEZ;
        public readonly ProjectionInfo BataviaTM109SE;
        public readonly ProjectionInfo BelgeLambert1950;
        public readonly ProjectionInfo BelgeLambert1972;
        public readonly ProjectionInfo Bermuda2000NationalGrid;
        public readonly ProjectionInfo Bern1898BernLV03C;
        public readonly ProjectionInfo BritishNationalGridOSGB36;
        public readonly ProjectionInfo CamacupaTM1130SE;
        public readonly ProjectionInfo CamacupaTM12SE;
        public readonly ProjectionInfo CarthageTM11NE;
        public readonly ProjectionInfo CentreFrance;
        public readonly ProjectionInfo CH1903LV03;
        public readonly ProjectionInfo CH1903LV95;
        public readonly ProjectionInfo ChosMalal1914Argentina2;
        public readonly ProjectionInfo ColombiaBogotaZone;
        public readonly ProjectionInfo ColombiaEastZone;
        public readonly ProjectionInfo ColombiaECentralZone;
        public readonly ProjectionInfo ColombiaWestZone;
        public readonly ProjectionInfo Corse;
        public readonly ProjectionInfo Datum73HayfordGaussIGeoE;
        public readonly ProjectionInfo Datum73HayfordGaussIPCC;
        public readonly ProjectionInfo DeirezZorLevantStereographic;
        public readonly ProjectionInfo DeirezZorLevantZone;
        public readonly ProjectionInfo DeirezZorSyriaLambert;
        public readonly ProjectionInfo DHDN3DegreeGaussZone1;
        public readonly ProjectionInfo DHDN3DegreeGaussZone2;
        public readonly ProjectionInfo DHDN3DegreeGaussZone3;
        public readonly ProjectionInfo DHDN3DegreeGaussZone4;
        public readonly ProjectionInfo DHDN3DegreeGaussZone5;
        public readonly ProjectionInfo Dominica1945BritishWestIndiesGrid;
        public readonly ProjectionInfo Douala1948AOFWest;
        public readonly ProjectionInfo DutchRD;
        public readonly ProjectionInfo ED1950FranceEuroLambert;
        public readonly ProjectionInfo ED1950TM0N;
        public readonly ProjectionInfo ED1950TM27;
        public readonly ProjectionInfo ED1950TM30;
        public readonly ProjectionInfo ED1950TM33;
        public readonly ProjectionInfo ED1950TM36;
        public readonly ProjectionInfo ED1950TM39;
        public readonly ProjectionInfo ED1950TM42;
        public readonly ProjectionInfo ED1950TM45;
        public readonly ProjectionInfo ED1950TM5NE;
        public readonly ProjectionInfo ED1950Turkey10;
        public readonly ProjectionInfo ED1950Turkey11;
        public readonly ProjectionInfo ED1950Turkey12;
        public readonly ProjectionInfo ED1950Turkey13;
        public readonly ProjectionInfo ED1950Turkey14;
        public readonly ProjectionInfo ED1950Turkey15;
        public readonly ProjectionInfo ED1950Turkey9;
        public readonly ProjectionInfo EgyptBlueBelt;
        public readonly ProjectionInfo EgyptExtendedPurpleBelt;
        public readonly ProjectionInfo EgyptPurpleBelt;
        public readonly ProjectionInfo EgyptRedBelt;
        public readonly ProjectionInfo ELD1979Libya10;
        public readonly ProjectionInfo ELD1979Libya11;
        public readonly ProjectionInfo ELD1979Libya12;
        public readonly ProjectionInfo ELD1979Libya13;
        public readonly ProjectionInfo ELD1979Libya5;
        public readonly ProjectionInfo ELD1979Libya6;
        public readonly ProjectionInfo ELD1979Libya7;
        public readonly ProjectionInfo ELD1979Libya8;
        public readonly ProjectionInfo ELD1979Libya9;
        public readonly ProjectionInfo ELD1979TM12NE;
        public readonly ProjectionInfo Estonia1997EstoniaNationalGrid;
        public readonly ProjectionInfo EstonianCoordinateSystemof1992;
        public readonly ProjectionInfo ETRF1989TMBaltic1993;
        public readonly ProjectionInfo ETRS1989Kp2000Bornholm;
        public readonly ProjectionInfo ETRS1989Kp2000Jutland;
        public readonly ProjectionInfo ETRS1989Kp2000Zealand;
        public readonly ProjectionInfo ETRS1989PolandCS2000Zone5;
        public readonly ProjectionInfo ETRS1989PolandCS2000Zone6;
        public readonly ProjectionInfo ETRS1989PolandCS2000Zone7;
        public readonly ProjectionInfo ETRS1989PolandCS2000Zone8;
        public readonly ProjectionInfo ETRS1989PolandCS92;
        public readonly ProjectionInfo ETRS1989TM30NE;
        public readonly ProjectionInfo ETRS1989TMBaltic1993;
        public readonly ProjectionInfo ETRS1989UWPP1992;
        public readonly ProjectionInfo ETRS1989UWPP2000PAS5;
        public readonly ProjectionInfo ETRS1989UWPP2000PAS6;
        public readonly ProjectionInfo ETRS1989UWPP2000PAS7;
        public readonly ProjectionInfo ETRS1989UWPP2000PAS8;
        public readonly ProjectionInfo EUREFFINTM35FIN;
        public readonly ProjectionInfo EverestModified1969RSOMalayaMeters;
        public readonly ProjectionInfo FD1958Iraq;
        public readonly ProjectionInfo FinlandZone1;
        public readonly ProjectionInfo FinlandZone2;
        public readonly ProjectionInfo FinlandZone3;
        public readonly ProjectionInfo FinlandZone4;
        public readonly ProjectionInfo FranceI;
        public readonly ProjectionInfo FranceII;
        public readonly ProjectionInfo FranceIII;
        public readonly ProjectionInfo FranceIV;
        public readonly ProjectionInfo GermanyZone1;
        public readonly ProjectionInfo GermanyZone2;
        public readonly ProjectionInfo GermanyZone3;
        public readonly ProjectionInfo GermanyZone4;
        public readonly ProjectionInfo GermanyZone5;
        public readonly ProjectionInfo GhanaMetreGrid;
        public readonly ProjectionInfo GreekGrid;
        public readonly ProjectionInfo Grenada1953BritishWestIndiesGrid;
        public readonly ProjectionInfo GuernseyGrid;
        public readonly ProjectionInfo GunungSegaraNEIEZ;
        public readonly ProjectionInfo Hanoi1972GK106NE;
        public readonly ProjectionInfo HD1972EgysegesOrszagosVetuleti;
        public readonly ProjectionInfo Helle1954JanMayenGrid;
        public readonly ProjectionInfo HitoXVIII1963Argentina2;
        public readonly ProjectionInfo HongKong1980Grid;
        public readonly ProjectionInfo Indian1960TM106NE;
        public readonly ProjectionInfo IRENET95IrishTranverseMercator;
        public readonly ProjectionInfo IrishNationalGrid;
        public readonly ProjectionInfo ISN1993Lambert1993;
        public readonly ProjectionInfo IsraelTMGrid;
        public readonly ProjectionInfo Jamaica1875OldGrid;
        public readonly ProjectionInfo JamaicaGrid;
        public readonly ProjectionInfo JordanJTM;
        public readonly ProjectionInfo KandawalaCeylonBeltIndianYards1937;
        public readonly ProjectionInfo KandawalaCeylonBeltMeters;
        public readonly ProjectionInfo KertauRSOMalayaChains;
        public readonly ProjectionInfo KertauRSOMalayaMeters;
        public readonly ProjectionInfo KertauSingaporeGrid;
        public readonly ProjectionInfo KOCLambert;
        public readonly ProjectionInfo Korean1985KoreaCentralBelt;
        public readonly ProjectionInfo Korean1985KoreaEastBelt;
        public readonly ProjectionInfo Korean1985KoreaWestBelt;
        public readonly ProjectionInfo KUDAMSKTM;
        public readonly ProjectionInfo KuwaitOilCoLambert;
        public readonly ProjectionInfo KuwaitUtilityKTM;
        public readonly ProjectionInfo LakeMaracaiboGrid;
        public readonly ProjectionInfo LakeMaracaiboGridM1;
        public readonly ProjectionInfo LakeMaracaiboGridM3;
        public readonly ProjectionInfo LakeMaracaiboLaRosaGrid;
        public readonly ProjectionInfo LietuvosKoordinaciuSistema;
        public readonly ProjectionInfo LisboaBesselBonne;
        public readonly ProjectionInfo LisboaHayfordGaussIGeoE;
        public readonly ProjectionInfo LisboaHayfordGaussIPCC;
        public readonly ProjectionInfo Locodjo1965TM5NW;
        public readonly ProjectionInfo Luxembourg1930Gauss;
        public readonly ProjectionInfo Madrid1870MadridSpain;
        public readonly ProjectionInfo MakassarNEIEZ;
        public readonly ProjectionInfo MGI3DegreeGaussZone5;
        public readonly ProjectionInfo MGI3DegreeGaussZone6;
        public readonly ProjectionInfo MGI3DegreeGaussZone7;
        public readonly ProjectionInfo MGI3DegreeGaussZone8;
        public readonly ProjectionInfo MGIAustriaLambert;
        public readonly ProjectionInfo MGIBalkans5;
        public readonly ProjectionInfo MGIBalkans6;
        public readonly ProjectionInfo MGIBalkans7;
        public readonly ProjectionInfo MGIBalkans8;
        public readonly ProjectionInfo MGIM28;
        public readonly ProjectionInfo MGIM31;
        public readonly ProjectionInfo MGIM34;
        public readonly ProjectionInfo MGISloveniaGrid;
        public readonly ProjectionInfo MonteMarioItaly1;
        public readonly ProjectionInfo MonteMarioItaly2;
        public readonly ProjectionInfo MonteMarioRomeItaly1;
        public readonly ProjectionInfo MonteMarioRomeItaly2;
        public readonly ProjectionInfo Montserrat1958BritishWestIndiesGrid;
        public readonly ProjectionInfo MountDillonTobagoGrid;
        public readonly ProjectionInfo NAD1927CubaNorte;
        public readonly ProjectionInfo NAD1927CubaSur;
        public readonly ProjectionInfo NAD1927GuatemalaNorte;
        public readonly ProjectionInfo NAD1927GuatemalaSur;
        public readonly ProjectionInfo NAD1927MichiganGeoRefMeters;
        public readonly ProjectionInfo NAD1927MichiganGeoRefUSfeet;
        public readonly ProjectionInfo NAD1983HARNGuamMapGrid;
        public readonly ProjectionInfo NAD1983MichiganGeoReferencedMeters;
        public readonly ProjectionInfo NAD1983MichiganGeoRefMeters;
        public readonly ProjectionInfo NAD1983MichiganGeoRefUSfeet;
        public readonly ProjectionInfo NewZealandMapGrid;
        public readonly ProjectionInfo NewZealandNorthIsland;
        public readonly ProjectionInfo NewZealandSouthIsland;
        public readonly ProjectionInfo NigeriaEastBelt;
        public readonly ProjectionInfo NigeriaMidBelt;
        public readonly ProjectionInfo NigeriaWestBelt;
        public readonly ProjectionInfo NordAlgerie;
        public readonly ProjectionInfo NordAlgerieancienne;
        public readonly ProjectionInfo NordAlgerieAnciennedegrees;
        public readonly ProjectionInfo NordAlgeriedegrees;
        public readonly ProjectionInfo NorddeGuerre;
        public readonly ProjectionInfo NordFrance;
        public readonly ProjectionInfo NordMaroc;
        public readonly ProjectionInfo NordMarocdegrees;
        public readonly ProjectionInfo NordTunisie;
        public readonly ProjectionInfo NTFFranceIdegrees;
        public readonly ProjectionInfo NTFFranceIIdegrees;
        public readonly ProjectionInfo NTFFranceIIIdegrees;
        public readonly ProjectionInfo NTFFranceIVdegrees;
        public readonly ProjectionInfo ObservatorioMeteorologico1965MacauGrid;
        public readonly ProjectionInfo OSNI1952IrishNationalGrid;
        public readonly ProjectionInfo Palestine1923IsraelCSGrid;
        public readonly ProjectionInfo Palestine1923PalestineBelt;
        public readonly ProjectionInfo Palestine1923PalestineGrid;
        public readonly ProjectionInfo PampadelCastilloArgentina2;
        public readonly ProjectionInfo PeruCentralZone;
        public readonly ProjectionInfo PeruEastZone;
        public readonly ProjectionInfo PeruWestZone;
        public readonly ProjectionInfo PhilippinesZoneI;
        public readonly ProjectionInfo PhilippinesZoneII;
        public readonly ProjectionInfo PhilippinesZoneIII;
        public readonly ProjectionInfo PhilippinesZoneIV;
        public readonly ProjectionInfo PhilippinesZoneV;
        public readonly ProjectionInfo PitondesNeigesTMReunion;
        public readonly ProjectionInfo PortugueseNationalGrid;
        public readonly ProjectionInfo PSAD1956ICNRegional;
        public readonly ProjectionInfo Qatar1948QatarGrid;
        public readonly ProjectionInfo QatarNationalGrid;
        public readonly ProjectionInfo RassadiranNakhleTaqi;
        public readonly ProjectionInfo RGF1993Lambert93;
        public readonly ProjectionInfo RGNC1991LambertNewCaledonia;
        public readonly ProjectionInfo Rijksdriehoekstelsel;
        public readonly ProjectionInfo Roma1940GaussBoagaEst;
        public readonly ProjectionInfo Roma1940GaussBoagaOvest;
        public readonly ProjectionInfo RT9025gonWest;
        public readonly ProjectionInfo SAD1969BrazilPolyconic;
        public readonly ProjectionInfo Sahara;
        public readonly ProjectionInfo Saharadegrees;
        public readonly ProjectionInfo SierraLeone1924NewColonyGrid;
        public readonly ProjectionInfo SierraLeone1924NewWarOfficeGrid;
        public readonly ProjectionInfo SJTSKFerroKrovak;
        public readonly ProjectionInfo SJTSKFerroKrovakEastNorth;
        public readonly ProjectionInfo SJTSKKrovak;
        public readonly ProjectionInfo SJTSKKrovakEastNorth;
        public readonly ProjectionInfo Stereo1933;
        public readonly ProjectionInfo Stereo1970;
        public readonly ProjectionInfo StKitts1955BritishWestIndiesGrid;
        public readonly ProjectionInfo StLucia1955BritishWestIndiesGrid;
        public readonly ProjectionInfo StVincent1945BritishWestIndiesGrid;
        public readonly ProjectionInfo SudAlgerie;
        public readonly ProjectionInfo SudAlgerieAncienneDegree;
        public readonly ProjectionInfo SudAlgeriedegrees;
        public readonly ProjectionInfo SudFrance;
        public readonly ProjectionInfo SudMaroc;
        public readonly ProjectionInfo SudMarocdegrees;
        public readonly ProjectionInfo SudTunisie;
        public readonly ProjectionInfo SwedishNationalGrid;
        public readonly ProjectionInfo Timbalai1948RSOBorneoChains;
        public readonly ProjectionInfo Timbalai1948RSOBorneoFeet;
        public readonly ProjectionInfo Timbalai1948RSOBorneoMeters;
        public readonly ProjectionInfo TM75IrishGrid;
        public readonly ProjectionInfo Trinidad1903TrinidadGrid;
        public readonly ProjectionInfo Trinidad1903TrinidadGridFeetClarke;
        public readonly ProjectionInfo UWPP1992;
        public readonly ProjectionInfo UWPP2000pas5;
        public readonly ProjectionInfo UWPP2000pas6;
        public readonly ProjectionInfo UWPP2000pas7;
        public readonly ProjectionInfo UWPP2000pas8;
        public readonly ProjectionInfo WGS1972TM106NE;
        public readonly ProjectionInfo WGS1984TM116SE;
        public readonly ProjectionInfo WGS1984TM132SE;
        public readonly ProjectionInfo WGS1984TM36SE;
        public readonly ProjectionInfo WGS1984TM6NE;
        public readonly ProjectionInfo ZanderijSurinameOldTM;
        public readonly ProjectionInfo ZanderijSurinameTM;
        public readonly ProjectionInfo ZanderijTM54NW;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NationalGrids
        /// </summary>
        public NationalGrids()
        {
            Abidjan1987TM5NW = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-5 +k=0.999600 +x_0=500000 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            AccraGhanaGrid = new ProjectionInfo("+proj=tmerc +lat_0=4.666666666666667 +lon_0=-1 +k=0.999750 +x_0=274319.7391633579 +y_0=0 +a=6378300 +b=6356751.689189189 +to_meter=0.3047997101815088 +no_defs ");
            AccraTM1NW = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-1 +k=0.999600 +x_0=500000 +y_0=0 +a=6378300 +b=6356751.689189189 +units=m +no_defs ");
            AinelAbdAramcoLambert = new ProjectionInfo("+proj=lcc +lat_1=17 +lat_2=33 +lat_0=25.08951 +lon_0=48 +x_0=0 +y_0=0 +ellps=intl +units=m +no_defs ");
            AmericanSamoa1962SamoaLambert = new ProjectionInfo("+proj=lcc +lat_1=-14.26666666666667 +lat_0=-14.26666666666667 +lon_0=-170 +k_0=1 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            Anguilla1957BritishWestIndiesGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-62 +k=0.999500 +x_0=400000 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            Antigua1943BritishWestIndiesGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-62 +k=0.999500 +x_0=400000 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            ArgentinaZone1 = new ProjectionInfo("+proj=tmerc +lat_0=-90 +lon_0=-72 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ArgentinaZone2 = new ProjectionInfo("+proj=tmerc +lat_0=-90 +lon_0=-69 +k=1.000000 +x_0=2500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ArgentinaZone3 = new ProjectionInfo("+proj=tmerc +lat_0=-90 +lon_0=-66 +k=1.000000 +x_0=3500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ArgentinaZone4 = new ProjectionInfo("+proj=tmerc +lat_0=-90 +lon_0=-63 +k=1.000000 +x_0=4500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ArgentinaZone5 = new ProjectionInfo("+proj=tmerc +lat_0=-90 +lon_0=-60 +k=1.000000 +x_0=5500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ArgentinaZone6 = new ProjectionInfo("+proj=tmerc +lat_0=-90 +lon_0=-57 +k=1.000000 +x_0=6500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ArgentinaZone7 = new ProjectionInfo("+proj=tmerc +lat_0=-90 +lon_0=-54 +k=1.000000 +x_0=7500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            AustriaFerroCentralZone = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=48.66666666666667 +k=1.000000 +x_0=0 +y_0=0 +ellps=bessel +pm=-17.66666666666667 +units=m +no_defs ");
            AustriaFerroEastZone = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=51.66666666666667 +k=1.000000 +x_0=0 +y_0=0 +ellps=bessel +pm=-17.66666666666667 +units=m +no_defs ");
            AustriaFerroWestZone = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=45.66666666666667 +k=1.000000 +x_0=0 +y_0=0 +ellps=bessel +pm=-17.66666666666667 +units=m +no_defs ");
            BahrainStateGrid = new ProjectionInfo("+proj=utm +zone=39 +ellps=intl +units=m +no_defs ");
            Barbados1938BarbadosGrid = new ProjectionInfo("+proj=tmerc +lat_0=13.17638888888889 +lon_0=-59.55972222222222 +k=0.999999 +x_0=30000 +y_0=75000 +ellps=clrk80 +units=m +no_defs ");
            Barbados1938BritishWestIndiesGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-62 +k=0.999500 +x_0=400000 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            BataviaNEIEZ = new ProjectionInfo("+proj=merc +lat_ts=4.45405154589751 +lon_0=110 +k=1.000000 +x_0=3900000 +y_0=900000 +ellps=bessel +units=m +no_defs ");
            BataviaTM109SE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=109 +k=0.999600 +x_0=500000 +y_0=10000000 +ellps=bessel +units=m +no_defs ");
            BelgeLambert1950 = new ProjectionInfo("+proj=lcc +lat_1=49.83333333333334 +lat_2=51.16666666666666 +lat_0=90 +lon_0=-4.367975 +x_0=150000 +y_0=5400000 +ellps=intl +pm=4.367975 +units=m +no_defs ");
            BelgeLambert1972 = new ProjectionInfo("+proj=lcc +lat_1=49.8333339 +lat_2=51.16666733333333 +lat_0=90 +lon_0=4.367486666666666 +x_0=150000.01256 +y_0=5400088.4378 +ellps=intl +units=m +no_defs ");
            Bermuda2000NationalGrid = new ProjectionInfo("+proj=tmerc +lat_0=32 +lon_0=-64.75 +k=1.000000 +x_0=550000 +y_0=100000 +ellps=WGS84 +units=m +no_defs ");
            Bern1898BernLV03C = new ProjectionInfo("+proj=somerc +lat_0=46.95240555555556 +lon_0=-7.439583333333333 +x_0=0 +y_0=0 +ellps=bessel +pm=7.439583333333333 +units=m +no_defs ");
            BritishNationalGridOSGB36 = new ProjectionInfo("+proj=tmerc +lat_0=49 +lon_0=-2 +k=0.9996012717 +x_0=400000 +y_0=-100000 +ellps=airy +datum=OSGB36 +units=m +no_defs ");
            CamacupaTM1130SE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=11.5 +k=0.999600 +x_0=500000 +y_0=10000000 +ellps=clrk80 +units=m +no_defs ");
            CamacupaTM12SE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=12 +k=0.999600 +x_0=500000 +y_0=10000000 +ellps=clrk80 +units=m +no_defs ");
            CarthageTM11NE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=9.900000000000002 +k=0.999600 +x_0=500000 +y_0=0 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            CentreFrance = new ProjectionInfo("+proj=lcc +lat_1=46.8 +lat_0=46.8 +lon_0=-2.337229166666667 +k_0=0.99987742 +x_0=600000 +y_0=200000 +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +units=m +no_defs ");
            CH1903LV03 = new ProjectionInfo("+proj=somerc +lat_0=46.95240555555556 +lon_0=7.439583333333333 +x_0=600000 +y_0=200000 +ellps=bessel +units=m +no_defs ");
            CH1903LV95 = new ProjectionInfo("+proj=somerc +lat_0=46.95240555555556 +lon_0=7.439583333333333 +x_0=2600000 +y_0=1200000 +ellps=bessel +units=m +no_defs ");
            ChosMalal1914Argentina2 = new ProjectionInfo("+proj=tmerc +lat_0=-90 +lon_0=-69 +k=1.000000 +x_0=2500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ColombiaBogotaZone = new ProjectionInfo("+proj=tmerc +lat_0=4.599047222222222 +lon_0=-74.08091666666667 +k=1.000000 +x_0=1000000 +y_0=1000000 +ellps=intl +units=m +no_defs ");
            ColombiaEastZone = new ProjectionInfo("+proj=tmerc +lat_0=4.599047222222222 +lon_0=-68.08091666666667 +k=1.000000 +x_0=1000000 +y_0=1000000 +ellps=intl +units=m +no_defs ");
            ColombiaECentralZone = new ProjectionInfo("+proj=tmerc +lat_0=4.599047222222222 +lon_0=-71.08091666666667 +k=1.000000 +x_0=1000000 +y_0=1000000 +ellps=intl +units=m +no_defs ");
            ColombiaWestZone = new ProjectionInfo("+proj=tmerc +lat_0=4.599047222222222 +lon_0=-77.08091666666667 +k=1.000000 +x_0=1000000 +y_0=1000000 +ellps=intl +units=m +no_defs ");
            Corse = new ProjectionInfo("+proj=lcc +lat_1=42.165 +lat_0=42.165 +lon_0=-2.337229166666667 +k_0=0.99994471 +x_0=234.358 +y_0=185861.369 +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +units=m +no_defs ");
            Datum73HayfordGaussIGeoE = new ProjectionInfo("+proj=tmerc +lat_0=39.66666666666666 +lon_0=-8.131906111111112 +k=1.000000 +x_0=200180.598 +y_0=299913.01 +ellps=intl +units=m +no_defs ");
            Datum73HayfordGaussIPCC = new ProjectionInfo("+proj=tmerc +lat_0=39.66666666666666 +lon_0=-8.131906111111112 +k=1.000000 +x_0=180.598 +y_0=-86.99 +ellps=intl +units=m +no_defs ");
            DeirezZorLevantStereographic = new ProjectionInfo("+a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            DeirezZorLevantZone = new ProjectionInfo("+a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            DeirezZorSyriaLambert = new ProjectionInfo("+proj=lcc +lat_1=34.65 +lat_0=34.65 +lon_0=37.35 +k_0=0.9996256 +x_0=300000 +y_0=300000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            DHDN3DegreeGaussZone1 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=3 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            DHDN3DegreeGaussZone2 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=6 +k=1.000000 +x_0=2500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            DHDN3DegreeGaussZone3 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=9 +k=1.000000 +x_0=3500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            DHDN3DegreeGaussZone4 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=12 +k=1.000000 +x_0=4500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            DHDN3DegreeGaussZone5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=1.000000 +x_0=5500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            Dominica1945BritishWestIndiesGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-62 +k=0.999500 +x_0=400000 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            Douala1948AOFWest = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=10.5 +k=0.999000 +x_0=1000000 +y_0=1000000 +ellps=intl +units=m +no_defs ");
            DutchRD = new ProjectionInfo("+proj=sterea +lat_0=52.15616055555555 +lon_0=5.38763888888889 +k=0.999908 +x_0=155000 +y_0=463000 +ellps=bessel +units=m +towgs84=565.2369,50.0087,465.658,-0.406857330322398,0.350732676542563,-1.8703473836068,4.0812 +no_defs +to +proj=latlong +datum=WGS84 ");
            ED1950FranceEuroLambert = new ProjectionInfo("+proj=lcc +lat_1=46.8 +lat_0=46.8 +lon_0=2.337229166666667 +k_0=0.99987742 +x_0=600000 +y_0=2200000 +ellps=intl +units=m +no_defs ");
            ED1950TM0N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=0 +k=0.999600 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950TM27 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=27 +k=1.000000 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950TM30 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=30 +k=1.000000 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950TM33 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=33 +k=1.000000 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950TM36 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=36 +k=1.000000 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950TM39 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=39 +k=1.000000 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950TM42 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=42 +k=1.000000 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950TM45 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=45 +k=1.000000 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950TM5NE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=5 +k=0.999600 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950Turkey10 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=30 +k=0.999600 +x_0=10500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950Turkey11 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=33 +k=0.999600 +x_0=11500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950Turkey12 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=36 +k=0.999600 +x_0=12500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950Turkey13 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=39 +k=0.999600 +x_0=13500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950Turkey14 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=42 +k=0.999600 +x_0=14500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950Turkey15 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=45 +k=0.999600 +x_0=15500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED1950Turkey9 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=27 +k=0.999600 +x_0=9500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            EgyptBlueBelt = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=35 +k=1.000000 +x_0=300000 +y_0=1100000 +ellps=helmert +units=m +no_defs ");
            EgyptExtendedPurpleBelt = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=27 +k=1.000000 +x_0=700000 +y_0=1200000 +ellps=helmert +units=m +no_defs ");
            EgyptPurpleBelt = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=27 +k=1.000000 +x_0=700000 +y_0=200000 +ellps=helmert +units=m +no_defs ");
            EgyptRedBelt = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=31 +k=1.000000 +x_0=615000 +y_0=810000 +ellps=helmert +units=m +no_defs ");
            ELD1979Libya10 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=19 +k=0.999900 +x_0=200000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ELD1979Libya11 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21 +k=0.999900 +x_0=200000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ELD1979Libya12 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=23 +k=0.999900 +x_0=200000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ELD1979Libya13 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=25 +k=0.999900 +x_0=200000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ELD1979Libya5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=9 +k=0.999900 +x_0=200000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ELD1979Libya6 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=11 +k=0.999900 +x_0=200000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ELD1979Libya7 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=13 +k=0.999900 +x_0=200000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ELD1979Libya8 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=0.999900 +x_0=200000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ELD1979Libya9 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=17 +k=0.999900 +x_0=200000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ELD1979TM12NE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=12 +k=0.999600 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            Estonia1997EstoniaNationalGrid = new ProjectionInfo("+proj=lcc +lat_1=58 +lat_2=59.33333333333334 +lat_0=57.51755393055556 +lon_0=24 +x_0=500000 +y_0=6375000 +ellps=GRS80 +units=m +no_defs ");
            EstonianCoordinateSystemof1992 = new ProjectionInfo("+proj=lcc +lat_1=58 +lat_2=59.33333333333334 +lat_0=57.51755393055556 +lon_0=24 +x_0=500000 +y_0=6375000 +ellps=GRS80 +units=m +no_defs ");
            ETRF1989TMBaltic1993 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=0.999600 +x_0=500000 +y_0=0 +ellps=WGS84 +units=m +no_defs ");
            ETRS1989Kp2000Bornholm = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=1.000000 +x_0=900000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989Kp2000Jutland = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=9.5 +k=0.999950 +x_0=200000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989Kp2000Zealand = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=12 +k=0.999950 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989PolandCS2000Zone5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=0.999923 +x_0=5500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989PolandCS2000Zone6 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=18 +k=0.999923 +x_0=6500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989PolandCS2000Zone7 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21 +k=0.999923 +x_0=7500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989PolandCS2000Zone8 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=0.999923 +x_0=8500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989PolandCS92 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=19 +k=0.999300 +x_0=500000 +y_0=-5300000 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989TM30NE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=30 +k=0.999600 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989TMBaltic1993 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=0.999600 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UWPP1992 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=19 +k=0.999300 +x_0=500000 +y_0=-5300000 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UWPP2000PAS5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=0.999923 +x_0=5500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UWPP2000PAS6 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=18 +k=0.999923 +x_0=6500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UWPP2000PAS7 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21 +k=0.999923 +x_0=7500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UWPP2000PAS8 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=0.999923 +x_0=8500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            EUREFFINTM35FIN = new ProjectionInfo("+proj=utm +zone=35 +ellps=GRS80 +units=m +no_defs ");
            EverestModified1969RSOMalayaMeters = new ProjectionInfo("+a=6377295.664 +b=6356094.667915204 +units=m +no_defs ");
            FD1958Iraq = new ProjectionInfo("+proj=lcc +lat_1=32.5 +lat_0=32.5 +lon_0=45 +k_0=0.9987864077700001 +x_0=1500000 +y_0=1166200 +ellps=clrk80 +units=m +no_defs ");
            FinlandZone1 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            FinlandZone2 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=1.000000 +x_0=2500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            FinlandZone3 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=27 +k=1.000000 +x_0=3500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            FinlandZone4 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=30 +k=1.000000 +x_0=4500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            FranceI = new ProjectionInfo("+proj=lcc +lat_1=49.49999999999999 +lat_0=49.49999999999999 +lon_0=-2.337229166666667 +k_0=0.999877341 +x_0=600000 +y_0=1200000 +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +units=m +no_defs ");
            FranceII = new ProjectionInfo("+proj=lcc +lat_1=46.8 +lat_0=46.8 +lon_0=-2.337229166666667 +k_0=0.99987742 +x_0=600000 +y_0=2200000 +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +units=m +no_defs ");
            FranceIII = new ProjectionInfo("+proj=lcc +lat_1=44.09999999999999 +lat_0=44.09999999999999 +lon_0=-2.337229166666667 +k_0=0.999877499 +x_0=600000 +y_0=3200000 +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +units=m +no_defs ");
            FranceIV = new ProjectionInfo("+proj=lcc +lat_1=42.165 +lat_0=42.165 +lon_0=-2.337229166666667 +k_0=0.99994471 +x_0=234.358 +y_0=185861.369 +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +units=m +no_defs ");
            GermanyZone1 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=3 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            GermanyZone2 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=6 +k=1.000000 +x_0=2500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            GermanyZone3 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=9 +k=1.000000 +x_0=3500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            GermanyZone4 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=12 +k=1.000000 +x_0=4500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            GermanyZone5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=1.000000 +x_0=5500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            GhanaMetreGrid = new ProjectionInfo("+proj=tmerc +lat_0=4.666666666666667 +lon_0=-1 +k=0.999750 +x_0=274319.51 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            GreekGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=0.999600 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            Grenada1953BritishWestIndiesGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-62 +k=0.999500 +x_0=400000 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            GuernseyGrid = new ProjectionInfo("+proj=tmerc +lat_0=49.5 +lon_0=-2.416666666666667 +k=0.999997 +x_0=47000 +y_0=50000 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            GunungSegaraNEIEZ = new ProjectionInfo("+proj=merc +lat_ts=4.45405154589751 +lon_0=110 +k=1.000000 +x_0=3900000 +y_0=900000 +ellps=bessel +units=m +no_defs ");
            Hanoi1972GK106NE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=106 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            HD1972EgysegesOrszagosVetuleti = new ProjectionInfo("+proj=somerc +lat_0=47.14439372222 +lon_0=19.048571778 +x_0=650000 +y_0=200000 +ellps=GRS67 +units=m +no_defs ");
            Helle1954JanMayenGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-8.5 +k=1.000000 +x_0=50000 +y_0=-7800000 +ellps=intl +units=m +no_defs ");
            HitoXVIII1963Argentina2 = new ProjectionInfo("+proj=tmerc +lat_0=-90 +lon_0=-69 +k=1.000000 +x_0=2500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            HongKong1980Grid = new ProjectionInfo("+proj=tmerc +lat_0=22.31213333333334 +lon_0=114.1785555555556 +k=1.000000 +x_0=836694.0500000001 +y_0=819069.8000000001 +ellps=intl +units=m +no_defs ");
            Indian1960TM106NE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=106 +k=0.999600 +x_0=500000 +y_0=0 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            IRENET95IrishTranverseMercator = new ProjectionInfo("+proj=tmerc +lat_0=53.5 +lon_0=-8 +k=0.999820 +x_0=600000 +y_0=750000 +ellps=GRS80 +units=m +no_defs ");
            IrishNationalGrid = new ProjectionInfo("+proj=tmerc +lat_0=53.5 +lon_0=-8 +k=1.000035 +x_0=200000 +y_0=250000 +a=6377340.189 +b=6356034.447938534 +units=m +no_defs ");
            ISN1993Lambert1993 = new ProjectionInfo("+proj=lcc +lat_1=64.25 +lat_2=65.75 +lat_0=65 +lon_0=-19 +x_0=500000 +y_0=500000 +ellps=GRS80 +units=m +no_defs ");
            IsraelTMGrid = new ProjectionInfo("+proj=tmerc +lat_0=31.73439361111111 +lon_0=35.20451694444445 +k=1.000007 +x_0=219529.584 +y_0=626907.39 +ellps=GRS80 +units=m +no_defs ");
            Jamaica1875OldGrid = new ProjectionInfo("+proj=lcc +lat_1=18 +lat_0=18 +lon_0=-77 +k_0=1 +x_0=550000 +y_0=400000 +a=6378249.138 +b=6356514.959419348 +units=m +no_defs ");
            JamaicaGrid = new ProjectionInfo("+proj=lcc +lat_1=18 +lat_0=18 +lon_0=-77 +k_0=1 +x_0=250000 +y_0=150000 +ellps=clrk66 +units=m +no_defs ");
            JordanJTM = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=37 +k=0.999800 +x_0=500000 +y_0=-3000000 +ellps=intl +units=m +no_defs ");
            KandawalaCeylonBeltIndianYards1937 = new ProjectionInfo("+proj=tmerc +lat_0=7.000480277777778 +lon_0=80.77171111111112 +k=1.000000 +x_0=160933.56048 +y_0=160933.56048 +a=6377276.345 +b=6356075.41314024 +to_meter=0.91439523 +no_defs ");
            KandawalaCeylonBeltMeters = new ProjectionInfo("+proj=tmerc +lat_0=7.000480277777778 +lon_0=80.77171111111112 +k=1.000000 +x_0=160933.56048 +y_0=160933.56048 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            KertauRSOMalayaChains = new ProjectionInfo("+a=6377304.063 +b=6356103.038993155 +to_meter=20.11678249437587 +no_defs ");
            KertauRSOMalayaMeters = new ProjectionInfo("+a=6377304.063 +b=6356103.038993155 +units=m +no_defs ");
            KertauSingaporeGrid = new ProjectionInfo("+proj=cass +lat_0=1.287646666666667 +lon_0=103.8530022222222 +x_0=30000 +y_0=30000 +a=6377304.063 +b=6356103.038993155 +units=m +no_defs ");
            KOCLambert = new ProjectionInfo("+proj=lcc +lat_1=32.5 +lat_0=32.5 +lon_0=45 +k_0=0.998786407767 +x_0=1500000 +y_0=1166200 +ellps=clrk80 +units=m +no_defs ");
            Korean1985KoreaCentralBelt = new ProjectionInfo("+proj=tmerc +lat_0=38 +lon_0=127 +k=1.000000 +x_0=200000 +y_0=500000 +ellps=bessel +units=m +no_defs ");
            Korean1985KoreaEastBelt = new ProjectionInfo("+proj=tmerc +lat_0=38 +lon_0=129 +k=1.000000 +x_0=200000 +y_0=500000 +ellps=bessel +units=m +no_defs ");
            Korean1985KoreaWestBelt = new ProjectionInfo("+proj=tmerc +lat_0=38 +lon_0=125 +k=1.000000 +x_0=200000 +y_0=500000 +ellps=bessel +units=m +no_defs ");
            KUDAMSKTM = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=48 +k=0.999600 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            KuwaitOilCoLambert = new ProjectionInfo("+proj=lcc +lat_1=32.5 +lat_0=32.5 +lon_0=45 +k_0=0.998786407767 +x_0=1500000 +y_0=1166200 +ellps=clrk80 +units=m +no_defs ");
            KuwaitUtilityKTM = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=48 +k=0.999600 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            LakeMaracaiboGrid = new ProjectionInfo("+proj=lcc +lat_1=10.16666666666667 +lat_0=10.16666666666667 +lon_0=-71.60561777777777 +k_0=1 +x_0=200000 +y_0=147315.028 +ellps=intl +units=m +no_defs ");
            LakeMaracaiboGridM1 = new ProjectionInfo("+proj=lcc +lat_1=10.16666666666667 +lat_0=10.16666666666667 +lon_0=-71.60561777777777 +k_0=1 +x_0=0 +y_0=-52684.972 +ellps=intl +units=m +no_defs ");
            LakeMaracaiboGridM3 = new ProjectionInfo("+proj=lcc +lat_1=10.16666666666667 +lat_0=10.16666666666667 +lon_0=-71.60561777777777 +k_0=1 +x_0=500000 +y_0=447315.028 +ellps=intl +units=m +no_defs ");
            LakeMaracaiboLaRosaGrid = new ProjectionInfo("+proj=lcc +lat_1=10.16666666666667 +lat_0=10.16666666666667 +lon_0=-71.60561777777777 +k_0=1 +x_0=-17044 +y_0=-23139.97 +ellps=intl +units=m +no_defs ");
            LietuvosKoordinaciuSistema = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=0.999800 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            LisboaBesselBonne = new ProjectionInfo("+proj=bonne +lon_0=-8.131906111111112 +lat_1=39.66666666666666 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            LisboaHayfordGaussIGeoE = new ProjectionInfo("+proj=tmerc +lat_0=39.66666666666666 +lon_0=-8.131906111111112 +k=1.000000 +x_0=200000 +y_0=300000 +ellps=intl +units=m +no_defs ");
            LisboaHayfordGaussIPCC = new ProjectionInfo("+proj=tmerc +lat_0=39.66666666666666 +lon_0=-8.131906111111112 +k=1.000000 +x_0=0 +y_0=0 +ellps=intl +units=m +no_defs ");
            Locodjo1965TM5NW = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-5 +k=0.999600 +x_0=500000 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            Luxembourg1930Gauss = new ProjectionInfo("+proj=tmerc +lat_0=49.83333333333334 +lon_0=6.166666666666667 +k=1.000000 +x_0=80000 +y_0=100000 +ellps=intl +units=m +no_defs ");
            Madrid1870MadridSpain = new ProjectionInfo("+proj=lcc +lat_1=40 +lat_0=40 +lon_0=3.687938888888889 +k_0=0.9988085293 +x_0=600000 +y_0=600000 +a=6378298.3 +b=6356657.142669561 +pm=-3.687938888888889 +units=m +no_defs ");
            MakassarNEIEZ = new ProjectionInfo("+proj=merc +lat_ts=4.45405154589751 +lon_0=110 +k=1.000000 +x_0=3900000 +y_0=900000 +ellps=bessel +units=m +no_defs ");
            MGI3DegreeGaussZone5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=1.000000 +x_0=5500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            MGI3DegreeGaussZone6 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=18 +k=1.000000 +x_0=6500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            MGI3DegreeGaussZone7 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21 +k=1.000000 +x_0=7500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            MGI3DegreeGaussZone8 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=1.000000 +x_0=8500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            MGIAustriaLambert = new ProjectionInfo("+proj=lcc +lat_1=46 +lat_2=49 +lat_0=47.5 +lon_0=13.33333333333333 +x_0=400000 +y_0=400000 +ellps=bessel +units=m +no_defs ");
            MGIBalkans5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=0.999900 +x_0=5500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            MGIBalkans6 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=18 +k=0.999900 +x_0=6500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            MGIBalkans7 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=0.999900 +x_0=8500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            MGIBalkans8 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=0.999900 +x_0=8500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            MGIM28 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=10.33333333333333 +k=1.000000 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            MGIM31 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=13.33333333333333 +k=1.000000 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            MGIM34 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=16.33333333333334 +k=1.000000 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            MGISloveniaGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=0.999900 +x_0=500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            MonteMarioItaly1 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=9 +k=0.999600 +x_0=1500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            MonteMarioItaly2 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=0.999600 +x_0=2520000 +y_0=0 +ellps=intl +units=m +no_defs ");
            MonteMarioRomeItaly1 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-15.90466666333333 +k=0.999600 +x_0=1500000 +y_0=0 +ellps=intl +pm=12.45233333333333 +units=m +no_defs ");
            MonteMarioRomeItaly2 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-9.90466666333333 +k=0.999600 +x_0=2520000 +y_0=0 +ellps=intl +pm=12.45233333333333 +units=m +no_defs ");
            Montserrat1958BritishWestIndiesGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-62 +k=0.999500 +x_0=400000 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            MountDillonTobagoGrid = new ProjectionInfo("+proj=cass +lat_0=11.25217861111111 +lon_0=-59.68600888888889 +x_0=37718.66154375 +y_0=36209.915082 +a=6378293.639 +b=6356617.98149216 +to_meter=0.2011661949 +no_defs ");
            NAD1927CubaNorte = new ProjectionInfo("+proj=lcc +lat_1=22.35 +lat_0=22.35 +lon_0=-81 +k_0=0.99993602 +x_0=500000 +y_0=280296.016 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927CubaSur = new ProjectionInfo("+proj=lcc +lat_1=20.71666666666667 +lat_0=20.71666666666667 +lon_0=-76.83333333333333 +k_0=0.99994848 +x_0=500000 +y_0=229126.939 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927GuatemalaNorte = new ProjectionInfo("+proj=lcc +lat_1=16.81666666666667 +lat_0=16.81666666666667 +lon_0=-90.33333333333333 +k_0=0.99992226 +x_0=500000 +y_0=292209.579 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927GuatemalaSur = new ProjectionInfo("+proj=lcc +lat_1=14.9 +lat_0=14.9 +lon_0=-90.33333333333333 +k_0=0.99989906 +x_0=500000 +y_0=325992.681 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927MichiganGeoRefMeters = new ProjectionInfo("+proj=omerc +lat_0=45.30916666666666 +lonc=-86 +alpha=337.255555555556 +k=0.9996 +x_0=2546731.496 +y_0=-4354009.816 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927MichiganGeoRefUSfeet = new ProjectionInfo("+proj=omerc +lat_0=45.30916666666666 +lonc=-86 +alpha=337.255555555556 +k=0.9996 +x_0=2546731.495961392 +y_0=-4354009.816002033 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNGuamMapGrid = new ProjectionInfo("+proj=tmerc +lat_0=13.5 +lon_0=144.75 +k=1.000000 +x_0=100000 +y_0=200000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983MichiganGeoReferencedMeters = new ProjectionInfo("+proj=omerc +lat_0=45.30916666666666 +lonc=-86 +alpha=337.25556 +k=0.9996 +x_0=2546731.496 +y_0=-4354009.816 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MichiganGeoRefMeters = new ProjectionInfo("+proj=omerc +lat_0=45.30916666666666 +lonc=-86 +alpha=337.255555555556 +k=0.9996 +x_0=2546731.496 +y_0=-4354009.816 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MichiganGeoRefUSfeet = new ProjectionInfo("+proj=omerc +lat_0=45.30916666666666 +lonc=-86 +alpha=337.255555555556 +k=0.9996 +x_0=2546731.495961392 +y_0=-4354009.816002033 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NewZealandMapGrid = new ProjectionInfo("+proj=nzmg +lat_0=-41 +lon_0=173 +x_0=2510000 +y_0=6023150 +ellps=intl +units=m +no_defs ");
            NewZealandNorthIsland = new ProjectionInfo("+proj=tmerc +lat_0=-39 +lon_0=175.5 +k=1.000000 +x_0=274319.5243848086 +y_0=365759.3658464114 +ellps=intl +to_meter=0.9143984146160287 +no_defs ");
            NewZealandSouthIsland = new ProjectionInfo("+proj=tmerc +lat_0=-44 +lon_0=171.5 +k=1.000000 +x_0=457199.2073080143 +y_0=457199.2073080143 +ellps=intl +to_meter=0.9143984146160287 +no_defs ");
            NigeriaEastBelt = new ProjectionInfo("+proj=tmerc +lat_0=4 +lon_0=12.5 +k=0.999750 +x_0=1110369.7 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            NigeriaMidBelt = new ProjectionInfo("+proj=tmerc +lat_0=4 +lon_0=8.5 +k=0.999750 +x_0=670553.98 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            NigeriaWestBelt = new ProjectionInfo("+proj=tmerc +lat_0=4 +lon_0=4.5 +k=0.999750 +x_0=230738.26 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            NordAlgerie = new ProjectionInfo("+proj=lcc +lat_1=36 +lat_0=36 +lon_0=2.7 +k_0=0.999625544 +x_0=500135 +y_0=300090 +ellps=clrk80 +units=m +no_defs ");
            NordAlgerieancienne = new ProjectionInfo("+proj=lcc +lat_1=37 +lat_0=37 +lon_0=3 +k_0=0.999625769 +x_0=500000 +y_0=300000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            NordAlgerieAnciennedegrees = new ProjectionInfo("+proj=lcc +lat_1=36 +lat_0=36 +lon_0=2.7 +k_0=0.999625544 +x_0=500000 +y_0=300000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            NordAlgeriedegrees = new ProjectionInfo("+proj=lcc +lat_1=36 +lat_0=36 +lon_0=2.7 +k_0=0.999625544 +x_0=500135 +y_0=300090 +ellps=clrk80 +units=m +no_defs ");
            NorddeGuerre = new ProjectionInfo("+proj=lcc +lat_1=49.49999999999999 +lat_0=49.49999999999999 +lon_0=3.062770833333333 +k_0=0.9995090800000001 +x_0=500000 +y_0=300000 +a=6376523 +b=6355862.933255573 +pm=2.337229166666667 +units=m +no_defs ");
            NordFrance = new ProjectionInfo("+proj=lcc +lat_1=49.49999999999999 +lat_0=49.49999999999999 +lon_0=-2.337229166666667 +k_0=0.999877341 +x_0=600000 +y_0=200000 +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +units=m +no_defs ");
            NordMaroc = new ProjectionInfo("+proj=lcc +lat_1=37 +lat_0=37 +lon_0=-6 +k_0=0.999625769 +x_0=500000 +y_0=300000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            NordMarocdegrees = new ProjectionInfo("+proj=lcc +lat_1=33.3 +lat_0=33.3 +lon_0=-5.4 +k_0=0.999625769 +x_0=500000 +y_0=300000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            NordTunisie = new ProjectionInfo("+proj=lcc +lat_1=36 +lat_0=36 +lon_0=9.899999999999999 +k_0=0.999625544 +x_0=500000 +y_0=300000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            NTFFranceIdegrees = new ProjectionInfo("+proj=lcc +lat_1=49.5 +lat_0=49.5 +lon_0=2.337229166666667 +k_0=0.999877341 +x_0=600000 +y_0=1200000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            NTFFranceIIdegrees = new ProjectionInfo("+proj=lcc +lat_1=46.8 +lat_0=46.8 +lon_0=2.337229166666667 +k_0=0.99987742 +x_0=600000 +y_0=2200000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            NTFFranceIIIdegrees = new ProjectionInfo("+proj=lcc +lat_1=44.1 +lat_0=44.1 +lon_0=2.337229166666667 +k_0=0.999877499 +x_0=600000 +y_0=3200000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            NTFFranceIVdegrees = new ProjectionInfo("+proj=lcc +lat_1=42.165 +lat_0=42.165 +lon_0=2.337229166666667 +k_0=0.99994471 +x_0=234.358 +y_0=185861.369 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            ObservatorioMeteorologico1965MacauGrid = new ProjectionInfo("+proj=tmerc +lat_0=22.21239722222222 +lon_0=113.5364694444444 +k=1.000000 +x_0=20000 +y_0=20000 +ellps=intl +units=m +no_defs ");
            OSNI1952IrishNationalGrid = new ProjectionInfo("+proj=tmerc +lat_0=53.5 +lon_0=-8 +k=1.000035 +x_0=200000 +y_0=250000 +ellps=airy +units=m +no_defs ");
            Palestine1923IsraelCSGrid = new ProjectionInfo("+proj=cass +lat_0=31.73409694444445 +lon_0=35.21208055555556 +x_0=170251.555 +y_0=1126867.909 +a=6378300.79 +b=6356566.430000036 +units=m +no_defs ");
            Palestine1923PalestineBelt = new ProjectionInfo("+proj=tmerc +lat_0=31.73409694444445 +lon_0=35.21208055555556 +k=1.000000 +x_0=170251.555 +y_0=1126867.909 +a=6378300.79 +b=6356566.430000036 +units=m +no_defs ");
            Palestine1923PalestineGrid = new ProjectionInfo("+proj=cass +lat_0=31.73409694444445 +lon_0=35.21208055555556 +x_0=170251.555 +y_0=126867.909 +a=6378300.79 +b=6356566.430000036 +units=m +no_defs ");
            PampadelCastilloArgentina2 = new ProjectionInfo("+proj=tmerc +lat_0=-90 +lon_0=-69 +k=1.000000 +x_0=2500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            PeruCentralZone = new ProjectionInfo("+proj=tmerc +lat_0=-9.5 +lon_0=-76 +k=0.999330 +x_0=720000 +y_0=1039979.159 +ellps=intl +units=m +no_defs ");
            PeruEastZone = new ProjectionInfo("+proj=tmerc +lat_0=-9.5 +lon_0=-70.5 +k=0.999530 +x_0=1324000 +y_0=1040084.558 +ellps=intl +units=m +no_defs ");
            PeruWestZone = new ProjectionInfo("+proj=tmerc +lat_0=-6 +lon_0=-80.5 +k=0.999830 +x_0=222000 +y_0=1426834.743 +ellps=intl +units=m +no_defs ");
            PhilippinesZoneI = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=117 +k=0.999950 +x_0=500000 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            PhilippinesZoneII = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=119 +k=0.999950 +x_0=500000 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            PhilippinesZoneIII = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=121 +k=0.999950 +x_0=500000 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            PhilippinesZoneIV = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=123 +k=0.999950 +x_0=500000 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            PhilippinesZoneV = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=125 +k=0.999950 +x_0=500000 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            PitondesNeigesTMReunion = new ProjectionInfo("+proj=tmerc +lat_0=-21.11666666666667 +lon_0=55.53333333333333 +k=1.000000 +x_0=50000 +y_0=160000 +ellps=intl +units=m +no_defs ");
            PortugueseNationalGrid = new ProjectionInfo("+proj=tmerc +lat_0=39.66666666666666 +lon_0=10.13190611111111 +k=1.000000 +x_0=200000 +y_0=300000 +ellps=intl +pm=-9.131906111111112 +units=m +no_defs ");
            PSAD1956ICNRegional = new ProjectionInfo("+proj=lcc +lat_1=3 +lat_2=9 +lat_0=6 +lon_0=-66 +x_0=1000000 +y_0=1000000 +ellps=intl +units=m +no_defs ");
            Qatar1948QatarGrid = new ProjectionInfo("+proj=cass +lat_0=25.38236111111111 +lon_0=50.76138888888889 +x_0=100000 +y_0=100000 +ellps=helmert +units=m +no_defs ");
            QatarNationalGrid = new ProjectionInfo("+proj=tmerc +lat_0=24.45 +lon_0=51.21666666666667 +k=0.999990 +x_0=200000 +y_0=300000 +ellps=intl +units=m +no_defs ");
            RassadiranNakhleTaqi = new ProjectionInfo("+proj=omerc +lat_0=27.56882880555555 +lonc=52.60353916666667 +alpha=0.5716611944444444 +k=0.999895934 +x_0=658377.437 +y_0=3044969.194 +ellps=intl +units=m +no_defs ");
            RGF1993Lambert93 = new ProjectionInfo("+proj=lcc +lat_1=44 +lat_2=49 +lat_0=46.5 +lon_0=3 +x_0=700000 +y_0=6600000 +ellps=GRS80 +units=m +no_defs ");
            RGNC1991LambertNewCaledonia = new ProjectionInfo("+proj=lcc +lat_1=-20.66666666666667 +lat_2=-22.33333333333333 +lat_0=-21.5 +lon_0=166 +x_0=400000 +y_0=300000 +ellps=intl +units=m +no_defs ");
            Rijksdriehoekstelsel = new ProjectionInfo("+ellps=bessel +units=m +no_defs ");
            Roma1940GaussBoagaEst = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=0.999600 +x_0=2520000 +y_0=0 +ellps=intl +units=m +no_defs ");
            Roma1940GaussBoagaOvest = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=9 +k=0.999600 +x_0=1500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            RT9025gonWest = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15.80827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            SAD1969BrazilPolyconic = new ProjectionInfo("+proj=poly +lat_0=0 +lon_0=-54 +x_0=5000000 +y_0=10000000 +ellps=aust_SA +units=m +no_defs ");
            Sahara = new ProjectionInfo("+proj=lcc +lat_1=29 +lat_0=29 +lon_0=-6 +k_0=0.9996 +x_0=1200000 +y_0=400000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            Saharadegrees = new ProjectionInfo("+proj=lcc +lat_1=26.1 +lat_0=26.1 +lon_0=-5.4 +k_0=0.9996 +x_0=1200000 +y_0=400000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            SierraLeone1924NewColonyGrid = new ProjectionInfo("+proj=tmerc +lat_0=6.666666666666667 +lon_0=-12 +k=1.000000 +x_0=152399.8550907544 +y_0=0 +a=6378300 +b=6356751.689189189 +to_meter=0.3047997101815088 +no_defs ");
            SierraLeone1924NewWarOfficeGrid = new ProjectionInfo("+proj=tmerc +lat_0=6.666666666666667 +lon_0=-12 +k=1.000000 +x_0=243839.7681452071 +y_0=182879.8261089053 +a=6378300 +b=6356751.689189189 +to_meter=0.3047997101815088 +no_defs ");
            SJTSKFerroKrovak = new ProjectionInfo("+proj=krovak +lat_0=49.5 +lon_0=60.16666666666667 +alpha=30.28813975277778 +k=0.9999 +x_0=0 +y_0=0 +ellps=bessel +pm=-17.66666666666667 +units=m +no_defs ");
            SJTSKFerroKrovakEastNorth = new ProjectionInfo("+proj=krovak +lat_0=49.5 +lon_0=60.16666666666667 +alpha=30.28813975277778 +k=0.9999 +x_0=0 +y_0=0 +ellps=bessel +pm=-17.66666666666667 +units=m +no_defs ");
            SJTSKKrovak = new ProjectionInfo("+proj=krovak +lat_0=49.5 +lon_0=24.83333333333333 +alpha=30.28813975277778 +k=0.9999 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            SJTSKKrovakEastNorth = new ProjectionInfo("+proj=krovak +lat_0=49.5 +lon_0=24.83333333333333 +alpha=30.28813975277778 +k=0.9999 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            Stereo1933 = new ProjectionInfo("+ellps=intl +units=m +no_defs ");
            Stereo1970 = new ProjectionInfo("+ellps=krass +units=m +no_defs ");
            StKitts1955BritishWestIndiesGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-62 +k=0.999500 +x_0=400000 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            StLucia1955BritishWestIndiesGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-62 +k=0.999500 +x_0=400000 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            StVincent1945BritishWestIndiesGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-62 +k=0.999500 +x_0=400000 +y_0=0 +ellps=clrk80 +units=m +no_defs ");
            SudAlgerie = new ProjectionInfo("+proj=lcc +lat_1=33.3 +lat_0=33.3 +lon_0=2.7 +k_0=0.999625769 +x_0=500135 +y_0=300090 +ellps=clrk80 +units=m +no_defs ");
            SudAlgerieAncienneDegree = new ProjectionInfo("+proj=lcc +lat_1=33.3 +lat_0=33.3 +lon_0=2.7 +k_0=0.999625769 +x_0=500000 +y_0=300000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            SudAlgeriedegrees = new ProjectionInfo("+proj=lcc +lat_1=33.3 +lat_0=33.3 +lon_0=2.7 +k_0=0.999625769 +x_0=500135 +y_0=300090 +ellps=clrk80 +units=m +no_defs ");
            SudFrance = new ProjectionInfo("+proj=lcc +lat_1=44.09999999999999 +lat_0=44.09999999999999 +lon_0=-2.337229166666667 +k_0=0.999877499 +x_0=600000 +y_0=200000 +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +units=m +no_defs ");
            SudMaroc = new ProjectionInfo("+proj=lcc +lat_1=33 +lat_0=33 +lon_0=-6 +k_0=0.9996155960000001 +x_0=500000 +y_0=300000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            SudMarocdegrees = new ProjectionInfo("+proj=lcc +lat_1=29.7 +lat_0=29.7 +lon_0=-5.4 +k_0=0.9996155960000001 +x_0=500000 +y_0=300000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            SudTunisie = new ProjectionInfo("+proj=lcc +lat_1=33.3 +lat_0=33.3 +lon_0=9.899999999999999 +k_0=0.999625769 +x_0=500000 +y_0=300000 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            SwedishNationalGrid = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-20.30827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +pm=18.05827777777778 +units=m +no_defs ");
            Timbalai1948RSOBorneoChains = new ProjectionInfo("+ellps=evrstSS +to_meter=20.11676512155263 +no_defs ");
            Timbalai1948RSOBorneoFeet = new ProjectionInfo("+ellps=evrstSS +to_meter=0.3047994715386762 +no_defs ");
            Timbalai1948RSOBorneoMeters = new ProjectionInfo("+ellps=evrstSS +units=m +no_defs ");
            TM75IrishGrid = new ProjectionInfo("+proj=tmerc +lat_0=53.5 +lon_0=-8 +k=1.000035 +x_0=200000 +y_0=250000 +a=6377340.189 +b=6356034.447938534 +units=m +no_defs ");
            Trinidad1903TrinidadGrid = new ProjectionInfo("+proj=cass +lat_0=10.44166666666667 +lon_0=-61.33333333333334 +x_0=86501.46380700001 +y_0=65379.0133425 +a=6378293.639 +b=6356617.98149216 +to_meter=0.2011661949 +no_defs ");
            Trinidad1903TrinidadGridFeetClarke = new ProjectionInfo("+proj=cass +lat_0=10.44166666666667 +lon_0=-61.33333333333334 +x_0=86501.46380699999 +y_0=65379.0133425 +a=6378293.639 +b=6356617.98149216 +to_meter=0.304797265 +no_defs ");
            UWPP1992 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=19 +k=0.999300 +x_0=500000 +y_0=-5300000 +ellps=WGS84 +units=m +no_defs ");
            UWPP2000pas5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=0.999923 +x_0=5500000 +y_0=0 +ellps=WGS84 +units=m +no_defs ");
            UWPP2000pas6 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=18 +k=0.999923 +x_0=6500000 +y_0=0 +ellps=WGS84 +units=m +no_defs ");
            UWPP2000pas7 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21 +k=0.999923 +x_0=7500000 +y_0=0 +ellps=WGS84 +units=m +no_defs ");
            UWPP2000pas8 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=0.999923 +x_0=8500000 +y_0=0 +ellps=WGS84 +units=m +no_defs ");
            WGS1972TM106NE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=106 +k=0.999600 +x_0=500000 +y_0=0 +ellps=WGS72 +units=m +no_defs ");
            WGS1984TM116SE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=116 +k=0.999600 +x_0=500000 +y_0=10000000 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984TM132SE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=132 +k=0.999600 +x_0=500000 +y_0=10000000 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984TM36SE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=36 +k=0.999600 +x_0=500000 +y_0=10000000 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984TM6NE = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=6 +k=0.999600 +x_0=500000 +y_0=10000000 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            ZanderijSurinameOldTM = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-55.68333333333333 +k=0.999600 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ZanderijSurinameTM = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-55.68333333333333 +k=0.999900 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ZanderijTM54NW = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-54 +k=0.999600 +x_0=500000 +y_0=0 +ellps=intl +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591