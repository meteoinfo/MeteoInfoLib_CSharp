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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:56:40 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// StatePlaneNad1927
    /// </summary>
    public class StatePlaneNad1927 : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1927StatePlaneAlabamaEastFIPS0101;
        public readonly ProjectionInfo NAD1927StatePlaneAlabamaWestFIPS0102;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska10FIPS5010;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska1FIPS5001;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska2FIPS5002;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska3FIPS5003;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska4FIPS5004;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska5FIPS5005;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska6FIPS5006;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska7FIPS5007;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska8FIPS5008;
        public readonly ProjectionInfo NAD1927StatePlaneAlaska9FIPS5009;
        public readonly ProjectionInfo NAD1927StatePlaneArizonaCentralFIPS0202;
        public readonly ProjectionInfo NAD1927StatePlaneArizonaEastFIPS0201;
        public readonly ProjectionInfo NAD1927StatePlaneArizonaWestFIPS0203;
        public readonly ProjectionInfo NAD1927StatePlaneArkansasNorthFIPS0301;
        public readonly ProjectionInfo NAD1927StatePlaneArkansasSouthFIPS0302;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaIFIPS0401;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaIIFIPS0402;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaIIIFIPS0403;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaIVFIPS0404;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaVFIPS0405;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaVIFIPS0406;
        public readonly ProjectionInfo NAD1927StatePlaneCaliforniaVIIFIPS0407;
        public readonly ProjectionInfo NAD1927StatePlaneColoradoCentralFIPS0502;
        public readonly ProjectionInfo NAD1927StatePlaneColoradoNorthFIPS0501;
        public readonly ProjectionInfo NAD1927StatePlaneColoradoSouthFIPS0503;
        public readonly ProjectionInfo NAD1927StatePlaneConnecticutFIPS0600;
        public readonly ProjectionInfo NAD1927StatePlaneDelawareFIPS0700;
        public readonly ProjectionInfo NAD1927StatePlaneFloridaEastFIPS0901;
        public readonly ProjectionInfo NAD1927StatePlaneFloridaNorthFIPS0903;
        public readonly ProjectionInfo NAD1927StatePlaneFloridaWestFIPS0902;
        public readonly ProjectionInfo NAD1927StatePlaneGeorgiaEastFIPS1001;
        public readonly ProjectionInfo NAD1927StatePlaneGeorgiaWestFIPS1002;
        public readonly ProjectionInfo NAD1927StatePlaneGuamFIPS5400;
        public readonly ProjectionInfo NAD1927StatePlaneIdahoCentralFIPS1102;
        public readonly ProjectionInfo NAD1927StatePlaneIdahoEastFIPS1101;
        public readonly ProjectionInfo NAD1927StatePlaneIdahoWestFIPS1103;
        public readonly ProjectionInfo NAD1927StatePlaneIllinoisEastFIPS1201;
        public readonly ProjectionInfo NAD1927StatePlaneIllinoisWestFIPS1202;
        public readonly ProjectionInfo NAD1927StatePlaneIndianaEastFIPS1301;
        public readonly ProjectionInfo NAD1927StatePlaneIndianaWestFIPS1302;
        public readonly ProjectionInfo NAD1927StatePlaneIowaNorthFIPS1401;
        public readonly ProjectionInfo NAD1927StatePlaneIowaSouthFIPS1402;
        public readonly ProjectionInfo NAD1927StatePlaneKansasNorthFIPS1501;
        public readonly ProjectionInfo NAD1927StatePlaneKansasSouthFIPS1502;
        public readonly ProjectionInfo NAD1927StatePlaneKentuckyNorthFIPS1601;
        public readonly ProjectionInfo NAD1927StatePlaneKentuckySouthFIPS1602;
        public readonly ProjectionInfo NAD1927StatePlaneLouisianaNorthFIPS1701;
        public readonly ProjectionInfo NAD1927StatePlaneLouisianaSouthFIPS1702;
        public readonly ProjectionInfo NAD1927StatePlaneMaineEastFIPS1801;
        public readonly ProjectionInfo NAD1927StatePlaneMaineWestFIPS1802;
        public readonly ProjectionInfo NAD1927StatePlaneMarylandFIPS1900;
        public readonly ProjectionInfo NAD1927StatePlaneMassachusettsIslandFIPS2002;
        public readonly ProjectionInfo NAD1927StatePlaneMassachusettsMainlandFIPS2001;
        public readonly ProjectionInfo NAD1927StatePlaneMichiganCentralFIPS2112;
        public readonly ProjectionInfo NAD1927StatePlaneMichiganNorthFIPS2111;
        public readonly ProjectionInfo NAD1927StatePlaneMichiganSouthFIPS2113;
        public readonly ProjectionInfo NAD1927StatePlaneMinnesotaCentralFIPS2202;
        public readonly ProjectionInfo NAD1927StatePlaneMinnesotaNorthFIPS2201;
        public readonly ProjectionInfo NAD1927StatePlaneMinnesotaSouthFIPS2203;
        public readonly ProjectionInfo NAD1927StatePlaneMississippiEastFIPS2301;
        public readonly ProjectionInfo NAD1927StatePlaneMississippiWestFIPS2302;
        public readonly ProjectionInfo NAD1927StatePlaneMissouriCentralFIPS2402;
        public readonly ProjectionInfo NAD1927StatePlaneMissouriEastFIPS2401;
        public readonly ProjectionInfo NAD1927StatePlaneMissouriWestFIPS2403;
        public readonly ProjectionInfo NAD1927StatePlaneMontanaCentralFIPS2502;
        public readonly ProjectionInfo NAD1927StatePlaneMontanaNorthFIPS2501;
        public readonly ProjectionInfo NAD1927StatePlaneMontanaSouthFIPS2503;
        public readonly ProjectionInfo NAD1927StatePlaneNebraskaNorthFIPS2601;
        public readonly ProjectionInfo NAD1927StatePlaneNebraskaSouthFIPS2602;
        public readonly ProjectionInfo NAD1927StatePlaneNevadaCentralFIPS2702;
        public readonly ProjectionInfo NAD1927StatePlaneNevadaEastFIPS2701;
        public readonly ProjectionInfo NAD1927StatePlaneNevadaWestFIPS2703;
        public readonly ProjectionInfo NAD1927StatePlaneNewHampshireFIPS2800;
        public readonly ProjectionInfo NAD1927StatePlaneNewJerseyFIPS2900;
        public readonly ProjectionInfo NAD1927StatePlaneNewMexicoCentralFIPS3002;
        public readonly ProjectionInfo NAD1927StatePlaneNewMexicoEastFIPS3001;
        public readonly ProjectionInfo NAD1927StatePlaneNewMexicoWestFIPS3003;
        public readonly ProjectionInfo NAD1927StatePlaneNewYorkCentralFIPS3102;
        public readonly ProjectionInfo NAD1927StatePlaneNewYorkEastFIPS3101;
        public readonly ProjectionInfo NAD1927StatePlaneNewYorkLongIslandFIPS3104;
        public readonly ProjectionInfo NAD1927StatePlaneNewYorkWestFIPS3103;
        public readonly ProjectionInfo NAD1927StatePlaneNorthCarolinaFIPS3200;
        public readonly ProjectionInfo NAD1927StatePlaneNorthDakotaNorthFIPS3301;
        public readonly ProjectionInfo NAD1927StatePlaneNorthDakotaSouthFIPS3302;
        public readonly ProjectionInfo NAD1927StatePlaneOhioNorthFIPS3401;
        public readonly ProjectionInfo NAD1927StatePlaneOhioSouthFIPS3402;
        public readonly ProjectionInfo NAD1927StatePlaneOklahomaNorthFIPS3501;
        public readonly ProjectionInfo NAD1927StatePlaneOklahomaSouthFIPS3502;
        public readonly ProjectionInfo NAD1927StatePlaneOregonNorthFIPS3601;
        public readonly ProjectionInfo NAD1927StatePlaneOregonSouthFIPS3602;
        public readonly ProjectionInfo NAD1927StatePlanePennsylvaniaNorthFIPS3701;
        public readonly ProjectionInfo NAD1927StatePlanePennsylvaniaSouthFIPS3702;
        public readonly ProjectionInfo NAD1927StatePlanePuertoRicoFIPS5201;
        public readonly ProjectionInfo NAD1927StatePlaneRhodeIslandFIPS3800;
        public readonly ProjectionInfo NAD1927StatePlaneSouthCarolinaNorthFIPS3901;
        public readonly ProjectionInfo NAD1927StatePlaneSouthCarolinaSouthFIPS3902;
        public readonly ProjectionInfo NAD1927StatePlaneSouthDakotaNorthFIPS4001;
        public readonly ProjectionInfo NAD1927StatePlaneSouthDakotaSouthFIPS4002;
        public readonly ProjectionInfo NAD1927StatePlaneTennesseeFIPS4100;
        public readonly ProjectionInfo NAD1927StatePlaneTexasCentralFIPS4203;
        public readonly ProjectionInfo NAD1927StatePlaneTexasNorthCentralFIPS4202;
        public readonly ProjectionInfo NAD1927StatePlaneTexasNorthFIPS4201;
        public readonly ProjectionInfo NAD1927StatePlaneTexasSouthCentralFIPS4204;
        public readonly ProjectionInfo NAD1927StatePlaneTexasSouthFIPS4205;
        public readonly ProjectionInfo NAD1927StatePlaneUtahCentralFIPS4302;
        public readonly ProjectionInfo NAD1927StatePlaneUtahNorthFIPS4301;
        public readonly ProjectionInfo NAD1927StatePlaneUtahSouthFIPS4303;
        public readonly ProjectionInfo NAD1927StatePlaneVermontFIPS3400;
        public readonly ProjectionInfo NAD1927StatePlaneVirginiaNorthFIPS4501;
        public readonly ProjectionInfo NAD1927StatePlaneVirginiaSouthFIPS4502;
        public readonly ProjectionInfo NAD1927StatePlaneWashingtonNorthFIPS4601;
        public readonly ProjectionInfo NAD1927StatePlaneWashingtonSouthFIPS4602;
        public readonly ProjectionInfo NAD1927StatePlaneWestVirginiaNorthFIPS4701;
        public readonly ProjectionInfo NAD1927StatePlaneWestVirginiaSouthFIPS4702;
        public readonly ProjectionInfo NAD1927StatePlaneWisconsinCentralFIPS4802;
        public readonly ProjectionInfo NAD1927StatePlaneWisconsinNorthFIPS4801;
        public readonly ProjectionInfo NAD1927StatePlaneWisconsinSouthFIPS4803;
        public readonly ProjectionInfo NAD1927StatePlaneWyomingEastCentralFIPS4902;
        public readonly ProjectionInfo NAD1927StatePlaneWyomingEastFIPS4901;
        public readonly ProjectionInfo NAD1927StatePlaneWyomingWestCentralFIPS4903;
        public readonly ProjectionInfo NAD1927StatePlaneWyomingWestFIPS4904;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StatePlaneNad1927
        /// </summary>
        public StatePlaneNad1927()
        {
            NAD1927StatePlaneAlabamaEastFIPS0101 = new ProjectionInfo("+proj=tmerc +lat_0=30.5 +lon_0=-85.83333333333333 +k=0.999960 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneAlabamaWestFIPS0102 = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=-87.5 +k=0.999933 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneAlaska10FIPS5010 = new ProjectionInfo("+proj=lcc +lat_1=51.83333333333334 +lat_2=53.83333333333334 +lat_0=51 +lon_0=-176 +x_0=914401.8288036577 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneAlaska1FIPS5001 = new ProjectionInfo("+proj=omerc +lat_0=57 +lonc=-133.6666666666667 +alpha=-36.86989764583333 +k=0.9999 +x_0=5000000.000000102 +y_0=-5000000.000000102 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneAlaska2FIPS5002 = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-142 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneAlaska3FIPS5003 = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-146 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneAlaska4FIPS5004 = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-150 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneAlaska5FIPS5005 = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-154 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneAlaska6FIPS5006 = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-158 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneAlaska7FIPS5007 = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-162 +k=0.999900 +x_0=213360.4267208535 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneAlaska8FIPS5008 = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-166 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneAlaska9FIPS5009 = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-170 +k=0.999900 +x_0=182880.3657607315 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneArizonaCentralFIPS0202 = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-111.9166666666667 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneArizonaEastFIPS0201 = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-110.1666666666667 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneArizonaWestFIPS0203 = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-113.75 +k=0.999933 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneArkansasNorthFIPS0301 = new ProjectionInfo("+proj=lcc +lat_1=34.93333333333333 +lat_2=36.23333333333333 +lat_0=34.33333333333334 +lon_0=-92 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneArkansasSouthFIPS0302 = new ProjectionInfo("+proj=lcc +lat_1=33.3 +lat_2=34.76666666666667 +lat_0=32.66666666666666 +lon_0=-92 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneCaliforniaIFIPS0401 = new ProjectionInfo("+proj=lcc +lat_1=40 +lat_2=41.66666666666666 +lat_0=39.33333333333334 +lon_0=-122 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneCaliforniaIIFIPS0402 = new ProjectionInfo("+proj=lcc +lat_1=38.33333333333334 +lat_2=39.83333333333334 +lat_0=37.66666666666666 +lon_0=-122 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneCaliforniaIIIFIPS0403 = new ProjectionInfo("+proj=lcc +lat_1=37.06666666666667 +lat_2=38.43333333333333 +lat_0=36.5 +lon_0=-120.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneCaliforniaIVFIPS0404 = new ProjectionInfo("+proj=lcc +lat_1=36 +lat_2=37.25 +lat_0=35.33333333333334 +lon_0=-119 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneCaliforniaVFIPS0405 = new ProjectionInfo("+proj=lcc +lat_1=34.03333333333333 +lat_2=35.46666666666667 +lat_0=33.5 +lon_0=-118 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneCaliforniaVIFIPS0406 = new ProjectionInfo("+proj=lcc +lat_1=32.78333333333333 +lat_2=33.88333333333333 +lat_0=32.16666666666666 +lon_0=-116.25 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneCaliforniaVIIFIPS0407 = new ProjectionInfo("+proj=lcc +lat_1=33.86666666666667 +lat_2=34.41666666666666 +lat_0=34.13333333333333 +lon_0=-118.3333333333333 +x_0=1276106.450596901 +y_0=1268253.006858014 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneColoradoCentralFIPS0502 = new ProjectionInfo("+proj=lcc +lat_1=38.45 +lat_2=39.75 +lat_0=37.83333333333334 +lon_0=-105.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneColoradoNorthFIPS0501 = new ProjectionInfo("+proj=lcc +lat_1=39.71666666666667 +lat_2=40.78333333333333 +lat_0=39.33333333333334 +lon_0=-105.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneColoradoSouthFIPS0503 = new ProjectionInfo("+proj=lcc +lat_1=37.23333333333333 +lat_2=38.43333333333333 +lat_0=36.66666666666666 +lon_0=-105.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneConnecticutFIPS0600 = new ProjectionInfo("+proj=lcc +lat_1=41.2 +lat_2=41.86666666666667 +lat_0=40.83333333333334 +lon_0=-72.75 +x_0=182880.3657607315 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneDelawareFIPS0700 = new ProjectionInfo("+proj=tmerc +lat_0=38 +lon_0=-75.41666666666667 +k=0.999995 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneFloridaEastFIPS0901 = new ProjectionInfo("+proj=tmerc +lat_0=24.33333333333333 +lon_0=-81 +k=0.999941 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneFloridaNorthFIPS0903 = new ProjectionInfo("+proj=lcc +lat_1=29.58333333333333 +lat_2=30.75 +lat_0=29 +lon_0=-84.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneFloridaWestFIPS0902 = new ProjectionInfo("+proj=tmerc +lat_0=24.33333333333333 +lon_0=-82 +k=0.999941 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneGeorgiaEastFIPS1001 = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=-82.16666666666667 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneGeorgiaWestFIPS1002 = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=-84.16666666666667 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneGuamFIPS5400 = new ProjectionInfo("+proj=poly +lat_0=13.47246635277778 +lon_0=144.7487507055556 +x_0=50000.00000000002 +y_0=50000.00000000002 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneIdahoCentralFIPS1102 = new ProjectionInfo("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-114 +k=0.999947 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneIdahoEastFIPS1101 = new ProjectionInfo("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-112.1666666666667 +k=0.999947 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneIdahoWestFIPS1103 = new ProjectionInfo("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-115.75 +k=0.999933 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneIllinoisEastFIPS1201 = new ProjectionInfo("+proj=tmerc +lat_0=36.66666666666666 +lon_0=-88.33333333333333 +k=0.999975 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneIllinoisWestFIPS1202 = new ProjectionInfo("+proj=tmerc +lat_0=36.66666666666666 +lon_0=-90.16666666666667 +k=0.999941 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneIndianaEastFIPS1301 = new ProjectionInfo("+proj=tmerc +lat_0=37.5 +lon_0=-85.66666666666667 +k=0.999967 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneIndianaWestFIPS1302 = new ProjectionInfo("+proj=tmerc +lat_0=37.5 +lon_0=-87.08333333333333 +k=0.999967 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneIowaNorthFIPS1401 = new ProjectionInfo("+proj=lcc +lat_1=42.06666666666667 +lat_2=43.26666666666667 +lat_0=41.5 +lon_0=-93.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneIowaSouthFIPS1402 = new ProjectionInfo("+proj=lcc +lat_1=40.61666666666667 +lat_2=41.78333333333333 +lat_0=40 +lon_0=-93.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneKansasNorthFIPS1501 = new ProjectionInfo("+proj=lcc +lat_1=38.71666666666667 +lat_2=39.78333333333333 +lat_0=38.33333333333334 +lon_0=-98 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneKansasSouthFIPS1502 = new ProjectionInfo("+proj=lcc +lat_1=37.26666666666667 +lat_2=38.56666666666667 +lat_0=36.66666666666666 +lon_0=-98.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneKentuckyNorthFIPS1601 = new ProjectionInfo("+proj=lcc +lat_1=37.96666666666667 +lat_2=38.96666666666667 +lat_0=37.5 +lon_0=-84.25 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneKentuckySouthFIPS1602 = new ProjectionInfo("+proj=lcc +lat_1=36.73333333333333 +lat_2=37.93333333333333 +lat_0=36.33333333333334 +lon_0=-85.75 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneLouisianaNorthFIPS1701 = new ProjectionInfo("+proj=lcc +lat_1=31.16666666666667 +lat_2=32.66666666666666 +lat_0=30.66666666666667 +lon_0=-92.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneLouisianaSouthFIPS1702 = new ProjectionInfo("+proj=lcc +lat_1=29.3 +lat_2=30.7 +lat_0=28.66666666666667 +lon_0=-91.33333333333333 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMaineEastFIPS1801 = new ProjectionInfo("+proj=tmerc +lat_0=43.83333333333334 +lon_0=-68.5 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMaineWestFIPS1802 = new ProjectionInfo("+proj=tmerc +lat_0=42.83333333333334 +lon_0=-70.16666666666667 +k=0.999967 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMarylandFIPS1900 = new ProjectionInfo("+proj=lcc +lat_1=38.3 +lat_2=39.45 +lat_0=37.83333333333334 +lon_0=-77 +x_0=243840.4876809754 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMassachusettsIslandFIPS2002 = new ProjectionInfo("+proj=lcc +lat_1=41.28333333333333 +lat_2=41.48333333333333 +lat_0=41 +lon_0=-70.5 +x_0=60960.12192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMassachusettsMainlandFIPS2001 = new ProjectionInfo("+proj=lcc +lat_1=41.71666666666667 +lat_2=42.68333333333333 +lat_0=41 +lon_0=-71.5 +x_0=182880.3657607315 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMichiganCentralFIPS2112 = new ProjectionInfo("+proj=lcc +lat_1=44.18333333333333 +lat_2=45.7 +lat_0=43.31666666666667 +lon_0=-84.33333333333333 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMichiganNorthFIPS2111 = new ProjectionInfo("+proj=lcc +lat_1=45.48333333333333 +lat_2=47.08333333333334 +lat_0=44.78333333333333 +lon_0=-87 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMichiganSouthFIPS2113 = new ProjectionInfo("+proj=lcc +lat_1=42.1 +lat_2=43.66666666666666 +lat_0=41.5 +lon_0=-84.33333333333333 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMinnesotaCentralFIPS2202 = new ProjectionInfo("+proj=lcc +lat_1=45.61666666666667 +lat_2=47.05 +lat_0=45 +lon_0=-94.25 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMinnesotaNorthFIPS2201 = new ProjectionInfo("+proj=lcc +lat_1=47.03333333333333 +lat_2=48.63333333333333 +lat_0=46.5 +lon_0=-93.09999999999999 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMinnesotaSouthFIPS2203 = new ProjectionInfo("+proj=lcc +lat_1=43.78333333333333 +lat_2=45.21666666666667 +lat_0=43 +lon_0=-94 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMississippiEastFIPS2301 = new ProjectionInfo("+proj=tmerc +lat_0=29.66666666666667 +lon_0=-88.83333333333333 +k=0.999960 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMississippiWestFIPS2302 = new ProjectionInfo("+proj=tmerc +lat_0=30.5 +lon_0=-90.33333333333333 +k=0.999941 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMissouriCentralFIPS2402 = new ProjectionInfo("+proj=tmerc +lat_0=35.83333333333334 +lon_0=-92.5 +k=0.999933 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMissouriEastFIPS2401 = new ProjectionInfo("+proj=tmerc +lat_0=35.83333333333334 +lon_0=-90.5 +k=0.999933 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMissouriWestFIPS2403 = new ProjectionInfo("+proj=tmerc +lat_0=36.16666666666666 +lon_0=-94.5 +k=0.999941 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMontanaCentralFIPS2502 = new ProjectionInfo("+proj=lcc +lat_1=46.45 +lat_2=47.88333333333333 +lat_0=45.83333333333334 +lon_0=-109.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMontanaNorthFIPS2501 = new ProjectionInfo("+proj=lcc +lat_1=47.85 +lat_2=48.71666666666667 +lat_0=47 +lon_0=-109.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneMontanaSouthFIPS2503 = new ProjectionInfo("+proj=lcc +lat_1=44.86666666666667 +lat_2=46.4 +lat_0=44 +lon_0=-109.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNebraskaNorthFIPS2601 = new ProjectionInfo("+proj=lcc +lat_1=41.85 +lat_2=42.81666666666667 +lat_0=41.33333333333334 +lon_0=-100 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNebraskaSouthFIPS2602 = new ProjectionInfo("+proj=lcc +lat_1=40.28333333333333 +lat_2=41.71666666666667 +lat_0=39.66666666666666 +lon_0=-99.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNevadaCentralFIPS2702 = new ProjectionInfo("+proj=tmerc +lat_0=34.75 +lon_0=-116.6666666666667 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNevadaEastFIPS2701 = new ProjectionInfo("+proj=tmerc +lat_0=34.75 +lon_0=-115.5833333333333 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNevadaWestFIPS2703 = new ProjectionInfo("+proj=tmerc +lat_0=34.75 +lon_0=-118.5833333333333 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNewHampshireFIPS2800 = new ProjectionInfo("+proj=tmerc +lat_0=42.5 +lon_0=-71.66666666666667 +k=0.999967 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNewJerseyFIPS2900 = new ProjectionInfo("+proj=tmerc +lat_0=38.83333333333334 +lon_0=-74.66666666666667 +k=0.999975 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNewMexicoCentralFIPS3002 = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-106.25 +k=0.999900 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNewMexicoEastFIPS3001 = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-104.3333333333333 +k=0.999909 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNewMexicoWestFIPS3003 = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-107.8333333333333 +k=0.999917 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNewYorkCentralFIPS3102 = new ProjectionInfo("+proj=tmerc +lat_0=40 +lon_0=-76.58333333333333 +k=0.999938 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNewYorkEastFIPS3101 = new ProjectionInfo("+proj=tmerc +lat_0=40 +lon_0=-74.33333333333333 +k=0.999967 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNewYorkLongIslandFIPS3104 = new ProjectionInfo("+proj=lcc +lat_1=40.66666666666666 +lat_2=41.03333333333333 +lat_0=40.5 +lon_0=-74 +x_0=609601.2192024385 +y_0=30480.06096012193 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNewYorkWestFIPS3103 = new ProjectionInfo("+proj=tmerc +lat_0=40 +lon_0=-78.58333333333333 +k=0.999938 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNorthCarolinaFIPS3200 = new ProjectionInfo("+proj=lcc +lat_1=34.33333333333334 +lat_2=36.16666666666666 +lat_0=33.75 +lon_0=-79 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNorthDakotaNorthFIPS3301 = new ProjectionInfo("+proj=lcc +lat_1=47.43333333333333 +lat_2=48.73333333333333 +lat_0=47 +lon_0=-100.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneNorthDakotaSouthFIPS3302 = new ProjectionInfo("+proj=lcc +lat_1=46.18333333333333 +lat_2=47.48333333333333 +lat_0=45.66666666666666 +lon_0=-100.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneOhioNorthFIPS3401 = new ProjectionInfo("+proj=lcc +lat_1=40.43333333333333 +lat_2=41.7 +lat_0=39.66666666666666 +lon_0=-82.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneOhioSouthFIPS3402 = new ProjectionInfo("+proj=lcc +lat_1=38.73333333333333 +lat_2=40.03333333333333 +lat_0=38 +lon_0=-82.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneOklahomaNorthFIPS3501 = new ProjectionInfo("+proj=lcc +lat_1=35.56666666666667 +lat_2=36.76666666666667 +lat_0=35 +lon_0=-98 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneOklahomaSouthFIPS3502 = new ProjectionInfo("+proj=lcc +lat_1=33.93333333333333 +lat_2=35.23333333333333 +lat_0=33.33333333333334 +lon_0=-98 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneOregonNorthFIPS3601 = new ProjectionInfo("+proj=lcc +lat_1=44.33333333333334 +lat_2=46 +lat_0=43.66666666666666 +lon_0=-120.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneOregonSouthFIPS3602 = new ProjectionInfo("+proj=lcc +lat_1=42.33333333333334 +lat_2=44 +lat_0=41.66666666666666 +lon_0=-120.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlanePennsylvaniaNorthFIPS3701 = new ProjectionInfo("+proj=lcc +lat_1=40.88333333333333 +lat_2=41.95 +lat_0=40.16666666666666 +lon_0=-77.75 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlanePennsylvaniaSouthFIPS3702 = new ProjectionInfo("+proj=lcc +lat_1=39.93333333333333 +lat_2=40.96666666666667 +lat_0=39.33333333333334 +lon_0=-77.75 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlanePuertoRicoFIPS5201 = new ProjectionInfo("+proj=lcc +lat_1=18.03333333333333 +lat_2=18.43333333333333 +lat_0=17.83333333333333 +lon_0=-66.43333333333334 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneRhodeIslandFIPS3800 = new ProjectionInfo("+proj=tmerc +lat_0=41.08333333333334 +lon_0=-71.5 +k=0.999994 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneSouthCarolinaNorthFIPS3901 = new ProjectionInfo("+proj=lcc +lat_1=33.76666666666667 +lat_2=34.96666666666667 +lat_0=33 +lon_0=-81 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneSouthCarolinaSouthFIPS3902 = new ProjectionInfo("+proj=lcc +lat_1=32.33333333333334 +lat_2=33.66666666666666 +lat_0=31.83333333333333 +lon_0=-81 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneSouthDakotaNorthFIPS4001 = new ProjectionInfo("+proj=lcc +lat_1=44.41666666666666 +lat_2=45.68333333333333 +lat_0=43.83333333333334 +lon_0=-100 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneSouthDakotaSouthFIPS4002 = new ProjectionInfo("+proj=lcc +lat_1=42.83333333333334 +lat_2=44.4 +lat_0=42.33333333333334 +lon_0=-100.3333333333333 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneTennesseeFIPS4100 = new ProjectionInfo("+proj=lcc +lat_1=35.25 +lat_2=36.41666666666666 +lat_0=34.66666666666666 +lon_0=-86 +x_0=609601.2192024385 +y_0=30480.06096012193 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneTexasCentralFIPS4203 = new ProjectionInfo("+proj=lcc +lat_1=30.11666666666667 +lat_2=31.88333333333333 +lat_0=29.66666666666667 +lon_0=-100.3333333333333 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneTexasNorthCentralFIPS4202 = new ProjectionInfo("+proj=lcc +lat_1=32.13333333333333 +lat_2=33.96666666666667 +lat_0=31.66666666666667 +lon_0=-97.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneTexasNorthFIPS4201 = new ProjectionInfo("+proj=lcc +lat_1=34.65 +lat_2=36.18333333333333 +lat_0=34 +lon_0=-101.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneTexasSouthCentralFIPS4204 = new ProjectionInfo("+proj=lcc +lat_1=28.38333333333333 +lat_2=30.28333333333333 +lat_0=27.83333333333333 +lon_0=-99 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneTexasSouthFIPS4205 = new ProjectionInfo("+proj=lcc +lat_1=26.16666666666667 +lat_2=27.83333333333333 +lat_0=25.66666666666667 +lon_0=-98.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneUtahCentralFIPS4302 = new ProjectionInfo("+proj=lcc +lat_1=39.01666666666667 +lat_2=40.65 +lat_0=38.33333333333334 +lon_0=-111.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneUtahNorthFIPS4301 = new ProjectionInfo("+proj=lcc +lat_1=40.71666666666667 +lat_2=41.78333333333333 +lat_0=40.33333333333334 +lon_0=-111.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneUtahSouthFIPS4303 = new ProjectionInfo("+proj=lcc +lat_1=37.21666666666667 +lat_2=38.35 +lat_0=36.66666666666666 +lon_0=-111.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneVermontFIPS3400 = new ProjectionInfo("+proj=tmerc +lat_0=42.5 +lon_0=-72.5 +k=0.999964 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneVirginiaNorthFIPS4501 = new ProjectionInfo("+proj=lcc +lat_1=38.03333333333333 +lat_2=39.2 +lat_0=37.66666666666666 +lon_0=-78.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneVirginiaSouthFIPS4502 = new ProjectionInfo("+proj=lcc +lat_1=36.76666666666667 +lat_2=37.96666666666667 +lat_0=36.33333333333334 +lon_0=-78.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneWashingtonNorthFIPS4601 = new ProjectionInfo("+proj=lcc +lat_1=47.5 +lat_2=48.73333333333333 +lat_0=47 +lon_0=-120.8333333333333 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneWashingtonSouthFIPS4602 = new ProjectionInfo("+proj=lcc +lat_1=45.83333333333334 +lat_2=47.33333333333334 +lat_0=45.33333333333334 +lon_0=-120.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneWestVirginiaNorthFIPS4701 = new ProjectionInfo("+proj=lcc +lat_1=39 +lat_2=40.25 +lat_0=38.5 +lon_0=-79.5 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneWestVirginiaSouthFIPS4702 = new ProjectionInfo("+proj=lcc +lat_1=37.48333333333333 +lat_2=38.88333333333333 +lat_0=37 +lon_0=-81 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneWisconsinCentralFIPS4802 = new ProjectionInfo("+proj=lcc +lat_1=44.25 +lat_2=45.5 +lat_0=43.83333333333334 +lon_0=-90 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneWisconsinNorthFIPS4801 = new ProjectionInfo("+proj=lcc +lat_1=45.56666666666667 +lat_2=46.76666666666667 +lat_0=45.16666666666666 +lon_0=-90 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneWisconsinSouthFIPS4803 = new ProjectionInfo("+proj=lcc +lat_1=42.73333333333333 +lat_2=44.06666666666667 +lat_0=42 +lon_0=-90 +x_0=609601.2192024385 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneWyomingEastCentralFIPS4902 = new ProjectionInfo("+proj=tmerc +lat_0=40.66666666666666 +lon_0=-107.3333333333333 +k=0.999941 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneWyomingEastFIPS4901 = new ProjectionInfo("+proj=tmerc +lat_0=40.66666666666666 +lon_0=-105.1666666666667 +k=0.999941 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneWyomingWestCentralFIPS4903 = new ProjectionInfo("+proj=tmerc +lat_0=40.66666666666666 +lon_0=-108.75 +k=0.999941 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927StatePlaneWyomingWestFIPS4904 = new ProjectionInfo("+proj=tmerc +lat_0=40.66666666666666 +lon_0=-110.0833333333333 +k=0.999941 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591