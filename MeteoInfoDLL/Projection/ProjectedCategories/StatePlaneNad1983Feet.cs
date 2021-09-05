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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:59:34 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// StatePlaneNad1983Feet
    /// </summary>
    public class StatePlaneNad1983Feet : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1983StatePlaneAlabamaEastFIPS0101Feet;
        public readonly ProjectionInfo NAD1983StatePlaneAlabamaWestFIPS0102Feet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska10FIPS5010Feet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska1FIPS5001Feet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska2FIPS5002Feet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska3FIPS5003Feet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska4FIPS5004Feet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska5FIPS5005Feet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska6FIPS5006Feet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska7FIPS5007Feet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska8FIPS5008Feet;
        public readonly ProjectionInfo NAD1983StatePlaneAlaska9FIPS5009Feet;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaCentralFIPS0202Feet;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaEastFIPS0201Feet;
        public readonly ProjectionInfo NAD1983StatePlaneArizonaWestFIPS0203Feet;
        public readonly ProjectionInfo NAD1983StatePlaneArkansasNorthFIPS0301Feet;
        public readonly ProjectionInfo NAD1983StatePlaneArkansasSouthFIPS0302Feet;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIFIPS0401Feet;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIIFIPS0402Feet;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIIIFIPS0403Feet;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaIVFIPS0404Feet;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaVFIPS0405Feet;
        public readonly ProjectionInfo NAD1983StatePlaneCaliforniaVIFIPS0406Feet;
        public readonly ProjectionInfo NAD1983StatePlaneColoradoCentralFIPS0502Feet;
        public readonly ProjectionInfo NAD1983StatePlaneColoradoNorthFIPS0501Feet;
        public readonly ProjectionInfo NAD1983StatePlaneColoradoSouthFIPS0503Feet;
        public readonly ProjectionInfo NAD1983StatePlaneConnecticutFIPS0600Feet;
        public readonly ProjectionInfo NAD1983StatePlaneDelawareFIPS0700Feet;
        public readonly ProjectionInfo NAD1983StatePlaneFloridaEastFIPS0901Feet;
        public readonly ProjectionInfo NAD1983StatePlaneFloridaNorthFIPS0903Feet;
        public readonly ProjectionInfo NAD1983StatePlaneFloridaWestFIPS0902Feet;
        public readonly ProjectionInfo NAD1983StatePlaneGeorgiaEastFIPS1001Feet;
        public readonly ProjectionInfo NAD1983StatePlaneGeorgiaWestFIPS1002Feet;
        public readonly ProjectionInfo NAD1983StatePlaneGuamFIPS5400Feet;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii1FIPS5101Feet;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii2FIPS5102Feet;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii3FIPS5103Feet;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii4FIPS5104Feet;
        public readonly ProjectionInfo NAD1983StatePlaneHawaii5FIPS5105Feet;
        public readonly ProjectionInfo NAD1983StatePlaneIdahoCentralFIPS1102Feet;
        public readonly ProjectionInfo NAD1983StatePlaneIdahoEastFIPS1101Feet;
        public readonly ProjectionInfo NAD1983StatePlaneIdahoWestFIPS1103Feet;
        public readonly ProjectionInfo NAD1983StatePlaneIllinoisEastFIPS1201Feet;
        public readonly ProjectionInfo NAD1983StatePlaneIllinoisWestFIPS1202Feet;
        public readonly ProjectionInfo NAD1983StatePlaneIndianaEastFIPS1301Feet;
        public readonly ProjectionInfo NAD1983StatePlaneIndianaWestFIPS1302Feet;
        public readonly ProjectionInfo NAD1983StatePlaneIowaNorthFIPS1401Feet;
        public readonly ProjectionInfo NAD1983StatePlaneIowaSouthFIPS1402Feet;
        public readonly ProjectionInfo NAD1983StatePlaneKansasNorthFIPS1501Feet;
        public readonly ProjectionInfo NAD1983StatePlaneKansasSouthFIPS1502Feet;
        public readonly ProjectionInfo NAD1983StatePlaneKentuckyFIPS1600Feet;
        public readonly ProjectionInfo NAD1983StatePlaneKentuckyNorthFIPS1601Feet;
        public readonly ProjectionInfo NAD1983StatePlaneKentuckySouthFIPS1602Feet;
        public readonly ProjectionInfo NAD1983StatePlaneLouisianaNorthFIPS1701Feet;
        public readonly ProjectionInfo NAD1983StatePlaneLouisianaSouthFIPS1702Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMaineEastFIPS1801Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMaineWestFIPS1802Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMarylandFIPS1900Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMassachusettsIslandFIPS2002Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMassachusettsMainlandFIPS2001Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganCentralFIPS2112Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganNorthFIPS2111Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMichiganSouthFIPS2113Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMinnesotaCentralFIPS2202Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMinnesotaNorthFIPS2201Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMinnesotaSouthFIPS2203Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMississippiEastFIPS2301Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMississippiWestFIPS2302Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMissouriCentralFIPS2402Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMissouriEastFIPS2401Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMissouriWestFIPS2403Feet;
        public readonly ProjectionInfo NAD1983StatePlaneMontanaFIPS2500Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNebraskaFIPS2600Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNevadaCentralFIPS2702Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNevadaEastFIPS2701Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNevadaWestFIPS2703Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNewHampshireFIPS2800Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNewJerseyFIPS2900Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNewMexicoCentralFIPS3002Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNewMexicoEastFIPS3001Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNewMexicoWestFIPS3003Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkCentralFIPS3102Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkEastFIPS3101Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkLongIslandFIPS3104Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNewYorkWestFIPS3103Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNorthCarolinaFIPS3200Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNorthDakotaNorthFIPS3301Feet;
        public readonly ProjectionInfo NAD1983StatePlaneNorthDakotaSouthFIPS3302Feet;
        public readonly ProjectionInfo NAD1983StatePlaneOhioNorthFIPS3401Feet;
        public readonly ProjectionInfo NAD1983StatePlaneOhioSouthFIPS3402Feet;
        public readonly ProjectionInfo NAD1983StatePlaneOklahomaNorthFIPS3501Feet;
        public readonly ProjectionInfo NAD1983StatePlaneOklahomaSouthFIPS3502Feet;
        public readonly ProjectionInfo NAD1983StatePlaneOregonNorthFIPS3601Feet;
        public readonly ProjectionInfo NAD1983StatePlaneOregonSouthFIPS3602Feet;
        public readonly ProjectionInfo NAD1983StatePlanePennsylvaniaNorthFIPS3701Feet;
        public readonly ProjectionInfo NAD1983StatePlanePennsylvaniaSouthFIPS3702Feet;
        public readonly ProjectionInfo NAD1983StatePlanePRVirginIslandsFIPS5200Feet;
        public readonly ProjectionInfo NAD1983StatePlaneRhodeIslandFIPS3800Feet;
        public readonly ProjectionInfo NAD1983StatePlaneSouthCarolinaFIPS3900Feet;
        public readonly ProjectionInfo NAD1983StatePlaneSouthDakotaNorthFIPS4001Feet;
        public readonly ProjectionInfo NAD1983StatePlaneSouthDakotaSouthFIPS4002Feet;
        public readonly ProjectionInfo NAD1983StatePlaneTennesseeFIPS4100Feet;
        public readonly ProjectionInfo NAD1983StatePlaneTexasCentralFIPS4203Feet;
        public readonly ProjectionInfo NAD1983StatePlaneTexasNorthCentralFIPS4202Feet;
        public readonly ProjectionInfo NAD1983StatePlaneTexasNorthFIPS4201Feet;
        public readonly ProjectionInfo NAD1983StatePlaneTexasSouthCentralFIPS4204Feet;
        public readonly ProjectionInfo NAD1983StatePlaneTexasSouthFIPS4205Feet;
        public readonly ProjectionInfo NAD1983StatePlaneUtahCentralFIPS4302Feet;
        public readonly ProjectionInfo NAD1983StatePlaneUtahNorthFIPS4301Feet;
        public readonly ProjectionInfo NAD1983StatePlaneUtahSouthFIPS4303Feet;
        public readonly ProjectionInfo NAD1983StatePlaneVermontFIPS4400Feet;
        public readonly ProjectionInfo NAD1983StatePlaneVirginiaNorthFIPS4501Feet;
        public readonly ProjectionInfo NAD1983StatePlaneVirginiaSouthFIPS4502Feet;
        public readonly ProjectionInfo NAD1983StatePlaneWashingtonNorthFIPS4601Feet;
        public readonly ProjectionInfo NAD1983StatePlaneWashingtonSouthFIPS4602Feet;
        public readonly ProjectionInfo NAD1983StatePlaneWestVirginiaNorthFIPS4701Feet;
        public readonly ProjectionInfo NAD1983StatePlaneWestVirginiaSouthFIPS4702Feet;
        public readonly ProjectionInfo NAD1983StatePlaneWisconsinCentralFIPS4802Feet;
        public readonly ProjectionInfo NAD1983StatePlaneWisconsinNorthFIPS4801Feet;
        public readonly ProjectionInfo NAD1983StatePlaneWisconsinSouthFIPS4803Feet;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingEastCentralFIPS4902Feet;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingEastFIPS4901Feet;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingWestCentralFIPS4903Feet;
        public readonly ProjectionInfo NAD1983StatePlaneWyomingWestFIPS4904Feet;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StatePlaneNad1983Feet
        /// </summary>
        public StatePlaneNad1983Feet()
        {
            NAD1983StatePlaneAlabamaEastFIPS0101Feet = new ProjectionInfo("+proj=tmerc +lat_0=30.5 +lon_0=-85.83333333333333 +k=0.999960 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneAlabamaWestFIPS0102Feet = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=-87.5 +k=0.999933 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneAlaska10FIPS5010Feet = new ProjectionInfo("+proj=lcc +lat_1=51.83333333333334 +lat_2=53.83333333333334 +lat_0=51 +lon_0=-176 +x_0=1000000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneAlaska1FIPS5001Feet = new ProjectionInfo("+proj=omerc +lat_0=57 +lonc=-133.6666666666667 +alpha=-36.86989764583333 +k=0.9999 +x_0=4999999.999999999 +y_0=-4999999.999999999 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneAlaska2FIPS5002Feet = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-142 +k=0.999900 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneAlaska3FIPS5003Feet = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-146 +k=0.999900 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneAlaska4FIPS5004Feet = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-150 +k=0.999900 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneAlaska5FIPS5005Feet = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-154 +k=0.999900 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneAlaska6FIPS5006Feet = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-158 +k=0.999900 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneAlaska7FIPS5007Feet = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-162 +k=0.999900 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneAlaska8FIPS5008Feet = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-166 +k=0.999900 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneAlaska9FIPS5009Feet = new ProjectionInfo("+proj=tmerc +lat_0=54 +lon_0=-170 +k=0.999900 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneArizonaCentralFIPS0202Feet = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-111.9166666666667 +k=0.999900 +x_0=213360 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneArizonaEastFIPS0201Feet = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-110.1666666666667 +k=0.999900 +x_0=213360 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneArizonaWestFIPS0203Feet = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-113.75 +k=0.999933 +x_0=213360 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneArkansasNorthFIPS0301Feet = new ProjectionInfo("+proj=lcc +lat_1=34.93333333333333 +lat_2=36.23333333333333 +lat_0=34.33333333333334 +lon_0=-92 +x_0=399999.9999999999 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneArkansasSouthFIPS0302Feet = new ProjectionInfo("+proj=lcc +lat_1=33.3 +lat_2=34.76666666666667 +lat_0=32.66666666666666 +lon_0=-92 +x_0=399999.9999999999 +y_0=399999.9999999999 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneCaliforniaIFIPS0401Feet = new ProjectionInfo("+proj=lcc +lat_1=40 +lat_2=41.66666666666666 +lat_0=39.33333333333334 +lon_0=-122 +x_0=2000000 +y_0=500000.0000000002 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneCaliforniaIIFIPS0402Feet = new ProjectionInfo("+proj=lcc +lat_1=38.33333333333334 +lat_2=39.83333333333334 +lat_0=37.66666666666666 +lon_0=-122 +x_0=2000000 +y_0=500000.0000000002 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneCaliforniaIIIFIPS0403Feet = new ProjectionInfo("+proj=lcc +lat_1=37.06666666666667 +lat_2=38.43333333333333 +lat_0=36.5 +lon_0=-120.5 +x_0=2000000 +y_0=500000.0000000002 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneCaliforniaIVFIPS0404Feet = new ProjectionInfo("+proj=lcc +lat_1=36 +lat_2=37.25 +lat_0=35.33333333333334 +lon_0=-119 +x_0=2000000 +y_0=500000.0000000002 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneCaliforniaVFIPS0405Feet = new ProjectionInfo("+proj=lcc +lat_1=34.03333333333333 +lat_2=35.46666666666667 +lat_0=33.5 +lon_0=-118 +x_0=2000000 +y_0=500000.0000000002 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneCaliforniaVIFIPS0406Feet = new ProjectionInfo("+proj=lcc +lat_1=32.78333333333333 +lat_2=33.88333333333333 +lat_0=32.16666666666666 +lon_0=-116.25 +x_0=2000000 +y_0=500000.0000000002 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneColoradoCentralFIPS0502Feet = new ProjectionInfo("+proj=lcc +lat_1=38.45 +lat_2=39.75 +lat_0=37.83333333333334 +lon_0=-105.5 +x_0=914401.8289 +y_0=304800.6096 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneColoradoNorthFIPS0501Feet = new ProjectionInfo("+proj=lcc +lat_1=39.71666666666667 +lat_2=40.78333333333333 +lat_0=39.33333333333334 +lon_0=-105.5 +x_0=914401.8289 +y_0=304800.6096 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneColoradoSouthFIPS0503Feet = new ProjectionInfo("+proj=lcc +lat_1=37.23333333333333 +lat_2=38.43333333333333 +lat_0=36.66666666666666 +lon_0=-105.5 +x_0=914401.8289 +y_0=304800.6096 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneConnecticutFIPS0600Feet = new ProjectionInfo("+proj=lcc +lat_1=41.2 +lat_2=41.86666666666667 +lat_0=40.83333333333334 +lon_0=-72.75 +x_0=304800.6096 +y_0=152400.3048 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneDelawareFIPS0700Feet = new ProjectionInfo("+proj=tmerc +lat_0=38 +lon_0=-75.41666666666667 +k=0.999995 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneFloridaEastFIPS0901Feet = new ProjectionInfo("+proj=tmerc +lat_0=24.33333333333333 +lon_0=-81 +k=0.999941 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneFloridaNorthFIPS0903Feet = new ProjectionInfo("+proj=lcc +lat_1=29.58333333333333 +lat_2=30.75 +lat_0=29 +lon_0=-84.5 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneFloridaWestFIPS0902Feet = new ProjectionInfo("+proj=tmerc +lat_0=24.33333333333333 +lon_0=-82 +k=0.999941 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneGeorgiaEastFIPS1001Feet = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=-82.16666666666667 +k=0.999900 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneGeorgiaWestFIPS1002Feet = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=-84.16666666666667 +k=0.999900 +x_0=700000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneGuamFIPS5400Feet = new ProjectionInfo("+proj=poly +lat_0=13.47246635277778 +lon_0=144.7487507055556 +x_0=49999.99999999999 +y_0=49999.99999999999 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneHawaii1FIPS5101Feet = new ProjectionInfo("+proj=tmerc +lat_0=18.83333333333333 +lon_0=-155.5 +k=0.999967 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneHawaii2FIPS5102Feet = new ProjectionInfo("+proj=tmerc +lat_0=20.33333333333333 +lon_0=-156.6666666666667 +k=0.999967 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneHawaii3FIPS5103Feet = new ProjectionInfo("+proj=tmerc +lat_0=21.16666666666667 +lon_0=-158 +k=0.999990 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneHawaii4FIPS5104Feet = new ProjectionInfo("+proj=tmerc +lat_0=21.83333333333333 +lon_0=-159.5 +k=0.999990 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneHawaii5FIPS5105Feet = new ProjectionInfo("+proj=tmerc +lat_0=21.66666666666667 +lon_0=-160.1666666666667 +k=1.000000 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneIdahoCentralFIPS1102Feet = new ProjectionInfo("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-114 +k=0.999947 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneIdahoEastFIPS1101Feet = new ProjectionInfo("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-112.1666666666667 +k=0.999947 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneIdahoWestFIPS1103Feet = new ProjectionInfo("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-115.75 +k=0.999933 +x_0=799999.9999999999 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneIllinoisEastFIPS1201Feet = new ProjectionInfo("+proj=tmerc +lat_0=36.66666666666666 +lon_0=-88.33333333333333 +k=0.999975 +x_0=300000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneIllinoisWestFIPS1202Feet = new ProjectionInfo("+proj=tmerc +lat_0=36.66666666666666 +lon_0=-90.16666666666667 +k=0.999941 +x_0=700000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneIndianaEastFIPS1301Feet = new ProjectionInfo("+proj=tmerc +lat_0=37.5 +lon_0=-85.66666666666667 +k=0.999967 +x_0=100000 +y_0=250000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneIndianaWestFIPS1302Feet = new ProjectionInfo("+proj=tmerc +lat_0=37.5 +lon_0=-87.08333333333333 +k=0.999967 +x_0=900000.0000000001 +y_0=250000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneIowaNorthFIPS1401Feet = new ProjectionInfo("+proj=lcc +lat_1=42.06666666666667 +lat_2=43.26666666666667 +lat_0=41.5 +lon_0=-93.5 +x_0=1500000 +y_0=1000000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneIowaSouthFIPS1402Feet = new ProjectionInfo("+proj=lcc +lat_1=40.61666666666667 +lat_2=41.78333333333333 +lat_0=40 +lon_0=-93.5 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneKansasNorthFIPS1501Feet = new ProjectionInfo("+proj=lcc +lat_1=38.71666666666667 +lat_2=39.78333333333333 +lat_0=38.33333333333334 +lon_0=-98 +x_0=399999.9999999999 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneKansasSouthFIPS1502Feet = new ProjectionInfo("+proj=lcc +lat_1=37.26666666666667 +lat_2=38.56666666666667 +lat_0=36.66666666666666 +lon_0=-98.5 +x_0=399999.9999999999 +y_0=399999.9999999999 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneKentuckyFIPS1600Feet = new ProjectionInfo("+proj=lcc +lat_1=37.08333333333334 +lat_2=38.66666666666666 +lat_0=36.33333333333334 +lon_0=-85.75 +x_0=1500000 +y_0=999999.9999999999 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneKentuckyNorthFIPS1601Feet = new ProjectionInfo("+proj=lcc +lat_1=37.96666666666667 +lat_2=38.96666666666667 +lat_0=37.5 +lon_0=-84.25 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneKentuckySouthFIPS1602Feet = new ProjectionInfo("+proj=lcc +lat_1=36.73333333333333 +lat_2=37.93333333333333 +lat_0=36.33333333333334 +lon_0=-85.75 +x_0=500000.0000000002 +y_0=500000.0000000002 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneLouisianaNorthFIPS1701Feet = new ProjectionInfo("+proj=lcc +lat_1=31.16666666666667 +lat_2=32.66666666666666 +lat_0=30.5 +lon_0=-92.5 +x_0=1000000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneLouisianaSouthFIPS1702Feet = new ProjectionInfo("+proj=lcc +lat_1=29.3 +lat_2=30.7 +lat_0=28.5 +lon_0=-91.33333333333333 +x_0=1000000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMaineEastFIPS1801Feet = new ProjectionInfo("+proj=tmerc +lat_0=43.66666666666666 +lon_0=-68.5 +k=0.999900 +x_0=300000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMaineWestFIPS1802Feet = new ProjectionInfo("+proj=tmerc +lat_0=42.83333333333334 +lon_0=-70.16666666666667 +k=0.999967 +x_0=900000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMarylandFIPS1900Feet = new ProjectionInfo("+proj=lcc +lat_1=38.3 +lat_2=39.45 +lat_0=37.66666666666666 +lon_0=-77 +x_0=399999.9999999999 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMassachusettsIslandFIPS2002Feet = new ProjectionInfo("+proj=lcc +lat_1=41.28333333333333 +lat_2=41.48333333333333 +lat_0=41 +lon_0=-70.5 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMassachusettsMainlandFIPS2001Feet = new ProjectionInfo("+proj=lcc +lat_1=41.71666666666667 +lat_2=42.68333333333333 +lat_0=41 +lon_0=-71.5 +x_0=200000 +y_0=750000.0000000001 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMichiganCentralFIPS2112Feet = new ProjectionInfo("+proj=lcc +lat_1=44.18333333333333 +lat_2=45.7 +lat_0=43.31666666666667 +lon_0=-84.36666666666666 +x_0=6000000.000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMichiganNorthFIPS2111Feet = new ProjectionInfo("+proj=lcc +lat_1=45.48333333333333 +lat_2=47.08333333333334 +lat_0=44.78333333333333 +lon_0=-87 +x_0=7999999.999999999 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMichiganSouthFIPS2113Feet = new ProjectionInfo("+proj=lcc +lat_1=42.1 +lat_2=43.66666666666666 +lat_0=41.5 +lon_0=-84.36666666666666 +x_0=4000000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMinnesotaCentralFIPS2202Feet = new ProjectionInfo("+proj=lcc +lat_1=45.61666666666667 +lat_2=47.05 +lat_0=45 +lon_0=-94.25 +x_0=799999.9999999999 +y_0=100000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMinnesotaNorthFIPS2201Feet = new ProjectionInfo("+proj=lcc +lat_1=47.03333333333333 +lat_2=48.63333333333333 +lat_0=46.5 +lon_0=-93.09999999999999 +x_0=799999.9999999999 +y_0=100000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMinnesotaSouthFIPS2203Feet = new ProjectionInfo("+proj=lcc +lat_1=43.78333333333333 +lat_2=45.21666666666667 +lat_0=43 +lon_0=-94 +x_0=799999.9999999999 +y_0=100000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMississippiEastFIPS2301Feet = new ProjectionInfo("+proj=tmerc +lat_0=29.5 +lon_0=-88.83333333333333 +k=0.999950 +x_0=300000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMississippiWestFIPS2302Feet = new ProjectionInfo("+proj=tmerc +lat_0=29.5 +lon_0=-90.33333333333333 +k=0.999950 +x_0=700000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMissouriCentralFIPS2402Feet = new ProjectionInfo("+proj=tmerc +lat_0=35.83333333333334 +lon_0=-92.5 +k=0.999933 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMissouriEastFIPS2401Feet = new ProjectionInfo("+proj=tmerc +lat_0=35.83333333333334 +lon_0=-90.5 +k=0.999933 +x_0=250000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMissouriWestFIPS2403Feet = new ProjectionInfo("+proj=tmerc +lat_0=36.16666666666666 +lon_0=-94.5 +k=0.999941 +x_0=850000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneMontanaFIPS2500Feet = new ProjectionInfo("+proj=lcc +lat_1=45 +lat_2=49 +lat_0=44.25 +lon_0=-109.5 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNebraskaFIPS2600Feet = new ProjectionInfo("+proj=lcc +lat_1=40 +lat_2=43 +lat_0=39.83333333333334 +lon_0=-100 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNevadaCentralFIPS2702Feet = new ProjectionInfo("+proj=tmerc +lat_0=34.75 +lon_0=-116.6666666666667 +k=0.999900 +x_0=500000.0000000002 +y_0=6000000.000000001 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNevadaEastFIPS2701Feet = new ProjectionInfo("+proj=tmerc +lat_0=34.75 +lon_0=-115.5833333333333 +k=0.999900 +x_0=200000 +y_0=7999999.999999999 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNevadaWestFIPS2703Feet = new ProjectionInfo("+proj=tmerc +lat_0=34.75 +lon_0=-118.5833333333333 +k=0.999900 +x_0=799999.9999999999 +y_0=4000000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNewHampshireFIPS2800Feet = new ProjectionInfo("+proj=tmerc +lat_0=42.5 +lon_0=-71.66666666666667 +k=0.999967 +x_0=300000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNewJerseyFIPS2900Feet = new ProjectionInfo("+proj=tmerc +lat_0=38.83333333333334 +lon_0=-74.5 +k=0.999900 +x_0=150000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNewMexicoCentralFIPS3002Feet = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-106.25 +k=0.999900 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNewMexicoEastFIPS3001Feet = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-104.3333333333333 +k=0.999909 +x_0=165000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNewMexicoWestFIPS3003Feet = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-107.8333333333333 +k=0.999917 +x_0=829999.9999999999 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNewYorkCentralFIPS3102Feet = new ProjectionInfo("+proj=tmerc +lat_0=40 +lon_0=-76.58333333333333 +k=0.999938 +x_0=250000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNewYorkEastFIPS3101Feet = new ProjectionInfo("+proj=tmerc +lat_0=38.83333333333334 +lon_0=-74.5 +k=0.999900 +x_0=150000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNewYorkLongIslandFIPS3104Feet = new ProjectionInfo("+proj=lcc +lat_1=40.66666666666666 +lat_2=41.03333333333333 +lat_0=40.16666666666666 +lon_0=-74 +x_0=300000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNewYorkWestFIPS3103Feet = new ProjectionInfo("+proj=tmerc +lat_0=40 +lon_0=-78.58333333333333 +k=0.999938 +x_0=350000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNorthCarolinaFIPS3200Feet = new ProjectionInfo("+proj=lcc +lat_1=34.33333333333334 +lat_2=36.16666666666666 +lat_0=33.75 +lon_0=-79 +x_0=609601.2199999999 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNorthDakotaNorthFIPS3301Feet = new ProjectionInfo("+proj=lcc +lat_1=47.43333333333333 +lat_2=48.73333333333333 +lat_0=47 +lon_0=-100.5 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneNorthDakotaSouthFIPS3302Feet = new ProjectionInfo("+proj=lcc +lat_1=46.18333333333333 +lat_2=47.48333333333333 +lat_0=45.66666666666666 +lon_0=-100.5 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneOhioNorthFIPS3401Feet = new ProjectionInfo("+proj=lcc +lat_1=40.43333333333333 +lat_2=41.7 +lat_0=39.66666666666666 +lon_0=-82.5 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneOhioSouthFIPS3402Feet = new ProjectionInfo("+proj=lcc +lat_1=38.73333333333333 +lat_2=40.03333333333333 +lat_0=38 +lon_0=-82.5 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneOklahomaNorthFIPS3501Feet = new ProjectionInfo("+proj=lcc +lat_1=35.56666666666667 +lat_2=36.76666666666667 +lat_0=35 +lon_0=-98 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneOklahomaSouthFIPS3502Feet = new ProjectionInfo("+proj=lcc +lat_1=33.93333333333333 +lat_2=35.23333333333333 +lat_0=33.33333333333334 +lon_0=-98 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneOregonNorthFIPS3601Feet = new ProjectionInfo("+proj=lcc +lat_1=44.33333333333334 +lat_2=46 +lat_0=43.66666666666666 +lon_0=-120.5 +x_0=2500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneOregonSouthFIPS3602Feet = new ProjectionInfo("+proj=lcc +lat_1=42.33333333333334 +lat_2=44 +lat_0=41.66666666666666 +lon_0=-120.5 +x_0=1500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlanePennsylvaniaNorthFIPS3701Feet = new ProjectionInfo("+proj=lcc +lat_1=40.88333333333333 +lat_2=41.95 +lat_0=40.16666666666666 +lon_0=-77.75 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlanePennsylvaniaSouthFIPS3702Feet = new ProjectionInfo("+proj=lcc +lat_1=39.93333333333333 +lat_2=40.96666666666667 +lat_0=39.33333333333334 +lon_0=-77.75 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlanePRVirginIslandsFIPS5200Feet = new ProjectionInfo("+proj=lcc +lat_1=18.03333333333333 +lat_2=18.43333333333333 +lat_0=17.83333333333333 +lon_0=-66.43333333333334 +x_0=199999.9999999999 +y_0=199999.9999999999 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneRhodeIslandFIPS3800Feet = new ProjectionInfo("+proj=tmerc +lat_0=41.08333333333334 +lon_0=-71.5 +k=0.999994 +x_0=100000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneSouthCarolinaFIPS3900Feet = new ProjectionInfo("+proj=lcc +lat_1=32.5 +lat_2=34.83333333333334 +lat_0=31.83333333333333 +lon_0=-81 +x_0=609600.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneSouthDakotaNorthFIPS4001Feet = new ProjectionInfo("+proj=lcc +lat_1=44.41666666666666 +lat_2=45.68333333333333 +lat_0=43.83333333333334 +lon_0=-100 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneSouthDakotaSouthFIPS4002Feet = new ProjectionInfo("+proj=lcc +lat_1=42.83333333333334 +lat_2=44.4 +lat_0=42.33333333333334 +lon_0=-100.3333333333333 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneTennesseeFIPS4100Feet = new ProjectionInfo("+proj=lcc +lat_1=35.25 +lat_2=36.41666666666666 +lat_0=34.33333333333334 +lon_0=-86 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneTexasCentralFIPS4203Feet = new ProjectionInfo("+proj=lcc +lat_1=30.11666666666667 +lat_2=31.88333333333333 +lat_0=29.66666666666667 +lon_0=-100.3333333333333 +x_0=700000 +y_0=3000000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneTexasNorthCentralFIPS4202Feet = new ProjectionInfo("+proj=lcc +lat_1=32.13333333333333 +lat_2=33.96666666666667 +lat_0=31.66666666666667 +lon_0=-98.5 +x_0=600000.0000000001 +y_0=2000000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneTexasNorthFIPS4201Feet = new ProjectionInfo("+proj=lcc +lat_1=34.65 +lat_2=36.18333333333333 +lat_0=34 +lon_0=-101.5 +x_0=200000 +y_0=1000000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneTexasSouthCentralFIPS4204Feet = new ProjectionInfo("+proj=lcc +lat_1=28.38333333333333 +lat_2=30.28333333333333 +lat_0=27.83333333333333 +lon_0=-99 +x_0=600000.0000000001 +y_0=4000000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneTexasSouthFIPS4205Feet = new ProjectionInfo("+proj=lcc +lat_1=26.16666666666667 +lat_2=27.83333333333333 +lat_0=25.66666666666667 +lon_0=-98.5 +x_0=300000 +y_0=4999999.999999999 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneUtahCentralFIPS4302Feet = new ProjectionInfo("+proj=lcc +lat_1=39.01666666666667 +lat_2=40.65 +lat_0=38.33333333333334 +lon_0=-111.5 +x_0=500000.0000000002 +y_0=2000000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneUtahNorthFIPS4301Feet = new ProjectionInfo("+proj=lcc +lat_1=40.71666666666667 +lat_2=41.78333333333333 +lat_0=40.33333333333334 +lon_0=-111.5 +x_0=500000.0000000002 +y_0=1000000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneUtahSouthFIPS4303Feet = new ProjectionInfo("+proj=lcc +lat_1=37.21666666666667 +lat_2=38.35 +lat_0=36.66666666666666 +lon_0=-111.5 +x_0=500000.0000000002 +y_0=3000000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneVermontFIPS4400Feet = new ProjectionInfo("+proj=tmerc +lat_0=42.5 +lon_0=-72.5 +k=0.999964 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneVirginiaNorthFIPS4501Feet = new ProjectionInfo("+proj=lcc +lat_1=38.03333333333333 +lat_2=39.2 +lat_0=37.66666666666666 +lon_0=-78.5 +x_0=3499999.999999999 +y_0=2000000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneVirginiaSouthFIPS4502Feet = new ProjectionInfo("+proj=lcc +lat_1=36.76666666666667 +lat_2=37.96666666666667 +lat_0=36.33333333333334 +lon_0=-78.5 +x_0=3499999.999999999 +y_0=1000000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneWashingtonNorthFIPS4601Feet = new ProjectionInfo("+proj=lcc +lat_1=47.5 +lat_2=48.73333333333333 +lat_0=47 +lon_0=-120.8333333333333 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneWashingtonSouthFIPS4602Feet = new ProjectionInfo("+proj=lcc +lat_1=45.83333333333334 +lat_2=47.33333333333334 +lat_0=45.33333333333334 +lon_0=-120.5 +x_0=500000.0000000002 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneWestVirginiaNorthFIPS4701Feet = new ProjectionInfo("+proj=lcc +lat_1=39 +lat_2=40.25 +lat_0=38.5 +lon_0=-79.5 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneWestVirginiaSouthFIPS4702Feet = new ProjectionInfo("+proj=lcc +lat_1=37.48333333333333 +lat_2=38.88333333333333 +lat_0=37 +lon_0=-81 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneWisconsinCentralFIPS4802Feet = new ProjectionInfo("+proj=lcc +lat_1=44.25 +lat_2=45.5 +lat_0=43.83333333333334 +lon_0=-90 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneWisconsinNorthFIPS4801Feet = new ProjectionInfo("+proj=lcc +lat_1=45.56666666666667 +lat_2=46.76666666666667 +lat_0=45.16666666666666 +lon_0=-90 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneWisconsinSouthFIPS4803Feet = new ProjectionInfo("+proj=lcc +lat_1=42.73333333333333 +lat_2=44.06666666666667 +lat_0=42 +lon_0=-90 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneWyomingEastCentralFIPS4902Feet = new ProjectionInfo("+proj=tmerc +lat_0=40.5 +lon_0=-107.3333333333333 +k=0.999938 +x_0=399999.9999999999 +y_0=100000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneWyomingEastFIPS4901Feet = new ProjectionInfo("+proj=tmerc +lat_0=40.5 +lon_0=-105.1666666666667 +k=0.999938 +x_0=200000 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneWyomingWestCentralFIPS4903Feet = new ProjectionInfo("+proj=tmerc +lat_0=40.5 +lon_0=-108.75 +k=0.999938 +x_0=600000.0000000001 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983StatePlaneWyomingWestFIPS4904Feet = new ProjectionInfo("+proj=tmerc +lat_0=40.5 +lon_0=-110.0833333333333 +k=0.999938 +x_0=799999.9999999999 +y_0=100000 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591