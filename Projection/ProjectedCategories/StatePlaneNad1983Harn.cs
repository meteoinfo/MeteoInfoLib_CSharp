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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:01:59 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// StatePlaneNad1983Harn
    /// </summary>
    public class StatePlaneNad1983Harn : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1983HARNMaine2000CentralZone;
        public readonly ProjectionInfo NAD1983HARNMaine2000EastZone;
        public readonly ProjectionInfo NAD1983HARNMaine2000WestZone;
        public readonly ProjectionInfo NAD1983HARNStatePlaneAlabamaEastFIPS0101;
        public readonly ProjectionInfo NAD1983HARNStatePlaneAlabamaWestFIPS0102;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaCentralFIPS0202;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaEastFIPS0201;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaWestFIPS0203;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArkansasNorthFIPS0301;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArkansasSouthFIPS0302;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIFIPS0401;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIIFIPS0402;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIIIFIPS0403;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIVFIPS0404;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaVFIPS0405;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaVIFIPS0406;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoCentralFIPS0502;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoNorthFIPS0501;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoSouthFIPS0503;
        public readonly ProjectionInfo NAD1983HARNStatePlaneConnecticutFIPS0600;
        public readonly ProjectionInfo NAD1983HARNStatePlaneDelawareFIPS0700;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaEastFIPS0901;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaNorthFIPS0903;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaWestFIPS0902;
        public readonly ProjectionInfo NAD1983HARNStatePlaneGeorgiaEastFIPS1001;
        public readonly ProjectionInfo NAD1983HARNStatePlaneGeorgiaWestFIPS1002;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii1FIPS5101;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii2FIPS5102;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii3FIPS5103;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii4FIPS5104;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii5FIPS5105;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoCentralFIPS1102;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoEastFIPS1101;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoWestFIPS1103;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIllinoisEastFIPS1201;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIllinoisWestFIPS1202;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIndianaEastFIPS1301;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIndianaWestFIPS1302;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIowaNorthFIPS1401;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIowaSouthFIPS1402;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKansasNorthFIPS1501;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKansasSouthFIPS1502;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKentuckyNorthFIPS1601;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKentuckySouthFIPS1602;
        public readonly ProjectionInfo NAD1983HARNStatePlaneLouisianaNorthFIPS1701;
        public readonly ProjectionInfo NAD1983HARNStatePlaneLouisianaSouthFIPS1702;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMaineEastFIPS1801;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMaineWestFIPS1802;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMarylandFIPS1900;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMassachusettsIslandFIPS2002;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganCentralFIPS2112;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganNorthFIPS2111;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganSouthFIPS2113;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMinnesotaCentralFIPS2202;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMinnesotaNorthFIPS2201;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMinnesotaSouthFIPS2203;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMississippiEastFIPS2301;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMississippiWestFIPS2302;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMissouriCentralFIPS2402;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMissouriEastFIPS2401;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMissouriWestFIPS2403;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMontanaFIPS2500;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNebraskaFIPS2600;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNevadaCentralFIPS2702;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNevadaEastFIPS2701;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNevadaWestFIPS2703;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewHampshireFIPS2800;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewJerseyFIPS2900;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoCentralFIPS3002;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoEastFIPS3001;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoWestFIPS3003;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkCentralFIPS3102;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkEastFIPS3101;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkWestFIPS3103;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOhioNorthFIPS3401;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOhioSouthFIPS3402;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOklahomaNorthFIPS3501;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOklahomaSouthFIPS3502;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOregonNorthFIPS3601;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOregonSouthFIPS3602;
        public readonly ProjectionInfo NAD1983HARNStatePlanePRVirginIslandsFIPS5200;
        public readonly ProjectionInfo NAD1983HARNStatePlaneRhodeIslandFIPS3800;
        public readonly ProjectionInfo NAD1983HARNStatePlaneSouthDakotaNorthFIPS4001;
        public readonly ProjectionInfo NAD1983HARNStatePlaneSouthDakotaSouthFIPS4002;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTennesseeFIPS4100;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasCentralFIPS4203;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasNorthCentralFIPS4202;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasNorthFIPS4201;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasSouthCentralFIPS4204;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasSouthFIPS4205;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahCentralFIPS4302;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahNorthFIPS4301;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahSouthFIPS4303;
        public readonly ProjectionInfo NAD1983HARNStatePlaneVermontFIPS4400;
        public readonly ProjectionInfo NAD1983HARNStatePlaneVirginiaNorthFIPS4501;
        public readonly ProjectionInfo NAD1983HARNStatePlaneVirginiaSouthFIPS4502;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWashingtonNorthFIPS4601;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWashingtonSouthFIPS4602;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWestVirginiaNorthFIPS4701;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWestVirginiaSouthFIPS4702;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinCentralFIPS4802;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinNorthFIPS4801;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinSouthFIPS4803;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWyomingEastFIPS4901;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWyomingECFIPS4902;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWyomingWCFIPS4903;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWyomingWestFIPS4904;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StatePlaneNad1983Harn
        /// </summary>
        public StatePlaneNad1983Harn()
        {
            NAD1983HARNMaine2000CentralZone = new ProjectionInfo("+proj=tmerc +lat_0=43.5 +lon_0=-69.125 +k=0.999980 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNMaine2000EastZone = new ProjectionInfo("+proj=tmerc +lat_0=43.83333333333334 +lon_0=-67.875 +k=0.999980 +x_0=700000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNMaine2000WestZone = new ProjectionInfo("+proj=tmerc +lat_0=42.83333333333334 +lon_0=-70.375 +k=0.999980 +x_0=300000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneAlabamaEastFIPS0101 = new ProjectionInfo("+proj=tmerc +lat_0=30.5 +lon_0=-85.83333333333333 +k=0.999960 +x_0=200000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneAlabamaWestFIPS0102 = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=-87.5 +k=0.999933 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneArizonaCentralFIPS0202 = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-111.9166666666667 +k=0.999900 +x_0=213360 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneArizonaEastFIPS0201 = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-110.1666666666667 +k=0.999900 +x_0=213360 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneArizonaWestFIPS0203 = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-113.75 +k=0.999933 +x_0=213360 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneArkansasNorthFIPS0301 = new ProjectionInfo("+proj=lcc +lat_1=34.93333333333333 +lat_2=36.23333333333333 +lat_0=34.33333333333334 +lon_0=-92 +x_0=400000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneArkansasSouthFIPS0302 = new ProjectionInfo("+proj=lcc +lat_1=33.3 +lat_2=34.76666666666667 +lat_0=32.66666666666666 +lon_0=-92 +x_0=400000 +y_0=400000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneCaliforniaIFIPS0401 = new ProjectionInfo("+proj=lcc +lat_1=40 +lat_2=41.66666666666666 +lat_0=39.33333333333334 +lon_0=-122 +x_0=2000000 +y_0=500000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneCaliforniaIIFIPS0402 = new ProjectionInfo("+proj=lcc +lat_1=38.33333333333334 +lat_2=39.83333333333334 +lat_0=37.66666666666666 +lon_0=-122 +x_0=2000000 +y_0=500000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneCaliforniaIIIFIPS0403 = new ProjectionInfo("+proj=lcc +lat_1=37.06666666666667 +lat_2=38.43333333333333 +lat_0=36.5 +lon_0=-120.5 +x_0=2000000 +y_0=500000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneCaliforniaIVFIPS0404 = new ProjectionInfo("+proj=lcc +lat_1=36 +lat_2=37.25 +lat_0=35.33333333333334 +lon_0=-119 +x_0=2000000 +y_0=500000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneCaliforniaVFIPS0405 = new ProjectionInfo("+proj=lcc +lat_1=34.03333333333333 +lat_2=35.46666666666667 +lat_0=33.5 +lon_0=-118 +x_0=2000000 +y_0=500000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneCaliforniaVIFIPS0406 = new ProjectionInfo("+proj=lcc +lat_1=32.78333333333333 +lat_2=33.88333333333333 +lat_0=32.16666666666666 +lon_0=-116.25 +x_0=2000000 +y_0=500000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneColoradoCentralFIPS0502 = new ProjectionInfo("+proj=lcc +lat_1=38.45 +lat_2=39.75 +lat_0=37.83333333333334 +lon_0=-105.5 +x_0=914401.8289 +y_0=304800.6096 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneColoradoNorthFIPS0501 = new ProjectionInfo("+proj=lcc +lat_1=39.71666666666667 +lat_2=40.78333333333333 +lat_0=39.33333333333334 +lon_0=-105.5 +x_0=914401.8289 +y_0=304800.6096 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneColoradoSouthFIPS0503 = new ProjectionInfo("+proj=lcc +lat_1=37.23333333333333 +lat_2=38.43333333333333 +lat_0=36.66666666666666 +lon_0=-105.5 +x_0=914401.8289 +y_0=304800.6096 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneConnecticutFIPS0600 = new ProjectionInfo("+proj=lcc +lat_1=41.2 +lat_2=41.86666666666667 +lat_0=40.83333333333334 +lon_0=-72.75 +x_0=304800.6096 +y_0=152400.3048 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneDelawareFIPS0700 = new ProjectionInfo("+proj=tmerc +lat_0=38 +lon_0=-75.41666666666667 +k=0.999995 +x_0=200000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneFloridaEastFIPS0901 = new ProjectionInfo("+proj=tmerc +lat_0=24.33333333333333 +lon_0=-81 +k=0.999941 +x_0=200000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneFloridaNorthFIPS0903 = new ProjectionInfo("+proj=lcc +lat_1=29.58333333333333 +lat_2=30.75 +lat_0=29 +lon_0=-84.5 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneFloridaWestFIPS0902 = new ProjectionInfo("+proj=tmerc +lat_0=24.33333333333333 +lon_0=-82 +k=0.999941 +x_0=200000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneGeorgiaEastFIPS1001 = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=-82.16666666666667 +k=0.999900 +x_0=200000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneGeorgiaWestFIPS1002 = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=-84.16666666666667 +k=0.999900 +x_0=700000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneHawaii1FIPS5101 = new ProjectionInfo("+proj=tmerc +lat_0=18.83333333333333 +lon_0=-155.5 +k=0.999967 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneHawaii2FIPS5102 = new ProjectionInfo("+proj=tmerc +lat_0=20.33333333333333 +lon_0=-156.6666666666667 +k=0.999967 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneHawaii3FIPS5103 = new ProjectionInfo("+proj=tmerc +lat_0=21.16666666666667 +lon_0=-158 +k=0.999990 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneHawaii4FIPS5104 = new ProjectionInfo("+proj=tmerc +lat_0=21.83333333333333 +lon_0=-159.5 +k=0.999990 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneHawaii5FIPS5105 = new ProjectionInfo("+proj=tmerc +lat_0=21.66666666666667 +lon_0=-160.1666666666667 +k=1.000000 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneIdahoCentralFIPS1102 = new ProjectionInfo("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-114 +k=0.999947 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneIdahoEastFIPS1101 = new ProjectionInfo("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-112.1666666666667 +k=0.999947 +x_0=200000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneIdahoWestFIPS1103 = new ProjectionInfo("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-115.75 +k=0.999933 +x_0=800000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneIllinoisEastFIPS1201 = new ProjectionInfo("+proj=tmerc +lat_0=36.66666666666666 +lon_0=-88.33333333333333 +k=0.999975 +x_0=300000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneIllinoisWestFIPS1202 = new ProjectionInfo("+proj=tmerc +lat_0=36.66666666666666 +lon_0=-90.16666666666667 +k=0.999941 +x_0=700000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneIndianaEastFIPS1301 = new ProjectionInfo("+proj=tmerc +lat_0=37.5 +lon_0=-85.66666666666667 +k=0.999967 +x_0=100000 +y_0=250000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneIndianaWestFIPS1302 = new ProjectionInfo("+proj=tmerc +lat_0=37.5 +lon_0=-87.08333333333333 +k=0.999967 +x_0=900000 +y_0=250000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneIowaNorthFIPS1401 = new ProjectionInfo("+proj=lcc +lat_1=42.06666666666667 +lat_2=43.26666666666667 +lat_0=41.5 +lon_0=-93.5 +x_0=1500000 +y_0=1000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneIowaSouthFIPS1402 = new ProjectionInfo("+proj=lcc +lat_1=40.61666666666667 +lat_2=41.78333333333333 +lat_0=40 +lon_0=-93.5 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneKansasNorthFIPS1501 = new ProjectionInfo("+proj=lcc +lat_1=38.71666666666667 +lat_2=39.78333333333333 +lat_0=38.33333333333334 +lon_0=-98 +x_0=400000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneKansasSouthFIPS1502 = new ProjectionInfo("+proj=lcc +lat_1=37.26666666666667 +lat_2=38.56666666666667 +lat_0=36.66666666666666 +lon_0=-98.5 +x_0=400000 +y_0=400000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneKentuckyNorthFIPS1601 = new ProjectionInfo("+proj=lcc +lat_1=37.96666666666667 +lat_2=38.96666666666667 +lat_0=37.5 +lon_0=-84.25 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneKentuckySouthFIPS1602 = new ProjectionInfo("+proj=lcc +lat_1=36.73333333333333 +lat_2=37.93333333333333 +lat_0=36.33333333333334 +lon_0=-85.75 +x_0=500000 +y_0=500000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneLouisianaNorthFIPS1701 = new ProjectionInfo("+proj=lcc +lat_1=31.16666666666667 +lat_2=32.66666666666666 +lat_0=30.5 +lon_0=-92.5 +x_0=1000000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneLouisianaSouthFIPS1702 = new ProjectionInfo("+proj=lcc +lat_1=29.3 +lat_2=30.7 +lat_0=28.5 +lon_0=-91.33333333333333 +x_0=1000000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMaineEastFIPS1801 = new ProjectionInfo("+proj=tmerc +lat_0=43.66666666666666 +lon_0=-68.5 +k=0.999900 +x_0=300000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMaineWestFIPS1802 = new ProjectionInfo("+proj=tmerc +lat_0=42.83333333333334 +lon_0=-70.16666666666667 +k=0.999967 +x_0=900000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMarylandFIPS1900 = new ProjectionInfo("+proj=lcc +lat_1=38.3 +lat_2=39.45 +lat_0=37.66666666666666 +lon_0=-77 +x_0=400000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMassachusettsIslandFIPS2002 = new ProjectionInfo("+proj=lcc +lat_1=41.28333333333333 +lat_2=41.48333333333333 +lat_0=41 +lon_0=-70.5 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001 = new ProjectionInfo("+proj=lcc +lat_1=41.71666666666667 +lat_2=42.68333333333333 +lat_0=41 +lon_0=-71.5 +x_0=200000 +y_0=750000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMichiganCentralFIPS2112 = new ProjectionInfo("+proj=lcc +lat_1=44.18333333333333 +lat_2=45.7 +lat_0=43.31666666666667 +lon_0=-84.36666666666666 +x_0=6000000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMichiganNorthFIPS2111 = new ProjectionInfo("+proj=lcc +lat_1=45.48333333333333 +lat_2=47.08333333333334 +lat_0=44.78333333333333 +lon_0=-87 +x_0=8000000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMichiganSouthFIPS2113 = new ProjectionInfo("+proj=lcc +lat_1=42.1 +lat_2=43.66666666666666 +lat_0=41.5 +lon_0=-84.36666666666666 +x_0=4000000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMinnesotaCentralFIPS2202 = new ProjectionInfo("+proj=lcc +lat_1=45.61666666666667 +lat_2=47.05 +lat_0=45 +lon_0=-94.25 +x_0=800000 +y_0=100000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMinnesotaNorthFIPS2201 = new ProjectionInfo("+proj=lcc +lat_1=47.03333333333333 +lat_2=48.63333333333333 +lat_0=46.5 +lon_0=-93.09999999999999 +x_0=800000 +y_0=100000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMinnesotaSouthFIPS2203 = new ProjectionInfo("+proj=lcc +lat_1=43.78333333333333 +lat_2=45.21666666666667 +lat_0=43 +lon_0=-94 +x_0=800000 +y_0=100000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMississippiEastFIPS2301 = new ProjectionInfo("+proj=tmerc +lat_0=29.5 +lon_0=-88.83333333333333 +k=0.999950 +x_0=300000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMississippiWestFIPS2302 = new ProjectionInfo("+proj=tmerc +lat_0=29.5 +lon_0=-90.33333333333333 +k=0.999950 +x_0=700000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMissouriCentralFIPS2402 = new ProjectionInfo("+proj=tmerc +lat_0=35.83333333333334 +lon_0=-92.5 +k=0.999933 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMissouriEastFIPS2401 = new ProjectionInfo("+proj=tmerc +lat_0=35.83333333333334 +lon_0=-90.5 +k=0.999933 +x_0=250000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMissouriWestFIPS2403 = new ProjectionInfo("+proj=tmerc +lat_0=36.16666666666666 +lon_0=-94.5 +k=0.999941 +x_0=850000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneMontanaFIPS2500 = new ProjectionInfo("+proj=lcc +lat_1=45 +lat_2=49 +lat_0=44.25 +lon_0=-109.5 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNebraskaFIPS2600 = new ProjectionInfo("+proj=lcc +lat_1=40 +lat_2=43 +lat_0=39.83333333333334 +lon_0=-100 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNevadaCentralFIPS2702 = new ProjectionInfo("+proj=tmerc +lat_0=34.75 +lon_0=-116.6666666666667 +k=0.999900 +x_0=500000 +y_0=6000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNevadaEastFIPS2701 = new ProjectionInfo("+proj=tmerc +lat_0=34.75 +lon_0=-115.5833333333333 +k=0.999900 +x_0=200000 +y_0=8000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNevadaWestFIPS2703 = new ProjectionInfo("+proj=tmerc +lat_0=34.75 +lon_0=-118.5833333333333 +k=0.999900 +x_0=800000 +y_0=4000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNewHampshireFIPS2800 = new ProjectionInfo("+proj=tmerc +lat_0=42.5 +lon_0=-71.66666666666667 +k=0.999967 +x_0=300000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNewJerseyFIPS2900 = new ProjectionInfo("+proj=tmerc +lat_0=38.83333333333334 +lon_0=-74.5 +k=0.999900 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNewMexicoCentralFIPS3002 = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-106.25 +k=0.999900 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNewMexicoEastFIPS3001 = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-104.3333333333333 +k=0.999909 +x_0=165000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNewMexicoWestFIPS3003 = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-107.8333333333333 +k=0.999917 +x_0=830000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNewYorkCentralFIPS3102 = new ProjectionInfo("+proj=tmerc +lat_0=40 +lon_0=-76.58333333333333 +k=0.999938 +x_0=250000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNewYorkEastFIPS3101 = new ProjectionInfo("+proj=tmerc +lat_0=38.83333333333334 +lon_0=-74.5 +k=0.999900 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104 = new ProjectionInfo("+proj=lcc +lat_1=40.66666666666666 +lat_2=41.03333333333333 +lat_0=40.16666666666666 +lon_0=-74 +x_0=300000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNewYorkWestFIPS3103 = new ProjectionInfo("+proj=tmerc +lat_0=40 +lon_0=-78.58333333333333 +k=0.999938 +x_0=350000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301 = new ProjectionInfo("+proj=lcc +lat_1=47.43333333333333 +lat_2=48.73333333333333 +lat_0=47 +lon_0=-100.5 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302 = new ProjectionInfo("+proj=lcc +lat_1=46.18333333333333 +lat_2=47.48333333333333 +lat_0=45.66666666666666 +lon_0=-100.5 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneOhioNorthFIPS3401 = new ProjectionInfo("+proj=lcc +lat_1=40.43333333333333 +lat_2=41.7 +lat_0=39.66666666666666 +lon_0=-82.5 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneOhioSouthFIPS3402 = new ProjectionInfo("+proj=lcc +lat_1=38.73333333333333 +lat_2=40.03333333333333 +lat_0=38 +lon_0=-82.5 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneOklahomaNorthFIPS3501 = new ProjectionInfo("+proj=lcc +lat_1=35.56666666666667 +lat_2=36.76666666666667 +lat_0=35 +lon_0=-98 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneOklahomaSouthFIPS3502 = new ProjectionInfo("+proj=lcc +lat_1=33.93333333333333 +lat_2=35.23333333333333 +lat_0=33.33333333333334 +lon_0=-98 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneOregonNorthFIPS3601 = new ProjectionInfo("+proj=lcc +lat_1=44.33333333333334 +lat_2=46 +lat_0=43.66666666666666 +lon_0=-120.5 +x_0=2500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneOregonSouthFIPS3602 = new ProjectionInfo("+proj=lcc +lat_1=42.33333333333334 +lat_2=44 +lat_0=41.66666666666666 +lon_0=-120.5 +x_0=1500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlanePRVirginIslandsFIPS5200 = new ProjectionInfo("+proj=lcc +lat_1=18.03333333333333 +lat_2=18.43333333333333 +lat_0=17.83333333333333 +lon_0=-66.43333333333334 +x_0=200000 +y_0=200000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneRhodeIslandFIPS3800 = new ProjectionInfo("+proj=tmerc +lat_0=41.08333333333334 +lon_0=-71.5 +k=0.999994 +x_0=100000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneSouthDakotaNorthFIPS4001 = new ProjectionInfo("+proj=lcc +lat_1=44.41666666666666 +lat_2=45.68333333333333 +lat_0=43.83333333333334 +lon_0=-100 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneSouthDakotaSouthFIPS4002 = new ProjectionInfo("+proj=lcc +lat_1=42.83333333333334 +lat_2=44.4 +lat_0=42.33333333333334 +lon_0=-100.3333333333333 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneTennesseeFIPS4100 = new ProjectionInfo("+proj=lcc +lat_1=35.25 +lat_2=36.41666666666666 +lat_0=34.33333333333334 +lon_0=-86 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneTexasCentralFIPS4203 = new ProjectionInfo("+proj=lcc +lat_1=30.11666666666667 +lat_2=31.88333333333333 +lat_0=29.66666666666667 +lon_0=-100.3333333333333 +x_0=700000 +y_0=3000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneTexasNorthCentralFIPS4202 = new ProjectionInfo("+proj=lcc +lat_1=32.13333333333333 +lat_2=33.96666666666667 +lat_0=31.66666666666667 +lon_0=-98.5 +x_0=600000 +y_0=2000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneTexasNorthFIPS4201 = new ProjectionInfo("+proj=lcc +lat_1=34.65 +lat_2=36.18333333333333 +lat_0=34 +lon_0=-101.5 +x_0=200000 +y_0=1000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneTexasSouthCentralFIPS4204 = new ProjectionInfo("+proj=lcc +lat_1=28.38333333333333 +lat_2=30.28333333333333 +lat_0=27.83333333333333 +lon_0=-99 +x_0=600000 +y_0=4000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneTexasSouthFIPS4205 = new ProjectionInfo("+proj=lcc +lat_1=26.16666666666667 +lat_2=27.83333333333333 +lat_0=25.66666666666667 +lon_0=-98.5 +x_0=300000 +y_0=5000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneUtahCentralFIPS4302 = new ProjectionInfo("+proj=lcc +lat_1=39.01666666666667 +lat_2=40.65 +lat_0=38.33333333333334 +lon_0=-111.5 +x_0=500000 +y_0=2000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneUtahNorthFIPS4301 = new ProjectionInfo("+proj=lcc +lat_1=40.71666666666667 +lat_2=41.78333333333333 +lat_0=40.33333333333334 +lon_0=-111.5 +x_0=500000 +y_0=1000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneUtahSouthFIPS4303 = new ProjectionInfo("+proj=lcc +lat_1=37.21666666666667 +lat_2=38.35 +lat_0=36.66666666666666 +lon_0=-111.5 +x_0=500000 +y_0=3000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneVermontFIPS4400 = new ProjectionInfo("+proj=tmerc +lat_0=42.5 +lon_0=-72.5 +k=0.999964 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneVirginiaNorthFIPS4501 = new ProjectionInfo("+proj=lcc +lat_1=38.03333333333333 +lat_2=39.2 +lat_0=37.66666666666666 +lon_0=-78.5 +x_0=3500000 +y_0=2000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneVirginiaSouthFIPS4502 = new ProjectionInfo("+proj=lcc +lat_1=36.76666666666667 +lat_2=37.96666666666667 +lat_0=36.33333333333334 +lon_0=-78.5 +x_0=3500000 +y_0=1000000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneWashingtonNorthFIPS4601 = new ProjectionInfo("+proj=lcc +lat_1=47.5 +lat_2=48.73333333333333 +lat_0=47 +lon_0=-120.8333333333333 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneWashingtonSouthFIPS4602 = new ProjectionInfo("+proj=lcc +lat_1=45.83333333333334 +lat_2=47.33333333333334 +lat_0=45.33333333333334 +lon_0=-120.5 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneWestVirginiaNorthFIPS4701 = new ProjectionInfo("+proj=lcc +lat_1=39 +lat_2=40.25 +lat_0=38.5 +lon_0=-79.5 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneWestVirginiaSouthFIPS4702 = new ProjectionInfo("+proj=lcc +lat_1=37.48333333333333 +lat_2=38.88333333333333 +lat_0=37 +lon_0=-81 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneWisconsinCentralFIPS4802 = new ProjectionInfo("+proj=lcc +lat_1=44.25 +lat_2=45.5 +lat_0=43.83333333333334 +lon_0=-90 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneWisconsinNorthFIPS4801 = new ProjectionInfo("+proj=lcc +lat_1=45.56666666666667 +lat_2=46.76666666666667 +lat_0=45.16666666666666 +lon_0=-90 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneWisconsinSouthFIPS4803 = new ProjectionInfo("+proj=lcc +lat_1=42.73333333333333 +lat_2=44.06666666666667 +lat_0=42 +lon_0=-90 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneWyomingEastFIPS4901 = new ProjectionInfo("+proj=tmerc +lat_0=40.5 +lon_0=-105.1666666666667 +k=0.999938 +x_0=200000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneWyomingECFIPS4902 = new ProjectionInfo("+proj=tmerc +lat_0=40.5 +lon_0=-107.3333333333333 +k=0.999938 +x_0=400000 +y_0=100000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneWyomingWCFIPS4903 = new ProjectionInfo("+proj=tmerc +lat_0=40.5 +lon_0=-108.75 +k=0.999938 +x_0=600000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNStatePlaneWyomingWestFIPS4904 = new ProjectionInfo("+proj=tmerc +lat_0=40.5 +lon_0=-110.0833333333333 +k=0.999938 +x_0=800000 +y_0=100000 +ellps=GRS80 +units=m +no_defs ");


        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591