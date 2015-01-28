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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:03:19 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591

namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// StatePlaneNad1983HarnFeet
    /// </summary>
    public class StatePlaneNad1983HarnFeet : CoordinateSystemCategory
    {
        #region Private Variables


        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaCentralFIPS0202FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaEastFIPS0201FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneArizonaWestFIPS0203FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIFIPS0401Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIIFIPS0402Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIIIFIPS0403Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaIVFIPS0404Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaVFIPS0405Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneCaliforniaVIFIPS0406Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoCentralFIPS0502Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoNorthFIPS0501Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneColoradoSouthFIPS0503Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneConnecticutFIPS0600Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneDelawareFIPS0700Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaEastFIPS0901Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaNorthFIPS0903Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneFloridaWestFIPS0902Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneGeorgiaEastFIPS1001Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneGeorgiaWestFIPS1002Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii1FIPS5101Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii2FIPS5102Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii3FIPS5103Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii4FIPS5104Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneHawaii5FIPS5105Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoCentralFIPS1102Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoEastFIPS1101Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIdahoWestFIPS1103Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIndianaEastFIPS1301Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneIndianaWestFIPS1302Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKentuckyNorthFIPS1601Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneKentuckySouthFIPS1602Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMarylandFIPS1900Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMassachusettsIslandFIPS2002Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganCentralFIPS2112FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganNorthFIPS2111FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMichiganSouthFIPS2113FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMississippiEastFIPS2301Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMississippiWestFIPS2302Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneMontanaFIPS2500FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoCentralFIPS3002Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoEastFIPS3001Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewMexicoWestFIPS3003Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkCentralFIPS3102Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkEastFIPS3101Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNewYorkWestFIPS3103Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOklahomaNorthFIPS3501Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOklahomaSouthFIPS3502Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOregonNorthFIPS3601FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneOregonSouthFIPS3602FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTennesseeFIPS4100Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasCentralFIPS4203Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasNorthCentralFIPS4202Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasNorthFIPS4201Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasSouthCentralFIPS4204Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneTexasSouthFIPS4205Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahCentralFIPS4302FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahNorthFIPS4301FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneUtahSouthFIPS4303FeetIntl;
        public readonly ProjectionInfo NAD1983HARNStatePlaneVirginiaNorthFIPS4501Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneVirginiaSouthFIPS4502Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWashingtonNorthFIPS4601Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWashingtonSouthFIPS4602Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinCentralFIPS4802Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinNorthFIPS4801Feet;
        public readonly ProjectionInfo NAD1983HARNStatePlaneWisconsinSouthFIPS4803Feet;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StatePlaneNad1983HarnFeet
        /// </summary>
        public StatePlaneNad1983HarnFeet()
        {
            NAD1983HARNStatePlaneArizonaCentralFIPS0202FeetIntl = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-111.9166666666667 +k=0.999900 +x_0=213360 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneArizonaEastFIPS0201FeetIntl = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-110.1666666666667 +k=0.999900 +x_0=213360 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneArizonaWestFIPS0203FeetIntl = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-113.75 +k=0.999933 +x_0=213360 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneCaliforniaIFIPS0401Feet = new ProjectionInfo("+proj=lcc +lat_1=40 +lat_2=41.66666666666666 +lat_0=39.33333333333334 +lon_0=-122 +x_0=2000000 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneCaliforniaIIFIPS0402Feet = new ProjectionInfo("+proj=lcc +lat_1=38.33333333333334 +lat_2=39.83333333333334 +lat_0=37.66666666666666 +lon_0=-122 +x_0=2000000 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneCaliforniaIIIFIPS0403Feet = new ProjectionInfo("+proj=lcc +lat_1=37.06666666666667 +lat_2=38.43333333333333 +lat_0=36.5 +lon_0=-120.5 +x_0=2000000 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneCaliforniaIVFIPS0404Feet = new ProjectionInfo("+proj=lcc +lat_1=36 +lat_2=37.25 +lat_0=35.33333333333334 +lon_0=-119 +x_0=2000000 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneCaliforniaVFIPS0405Feet = new ProjectionInfo("+proj=lcc +lat_1=34.03333333333333 +lat_2=35.46666666666667 +lat_0=33.5 +lon_0=-118 +x_0=2000000 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneCaliforniaVIFIPS0406Feet = new ProjectionInfo("+proj=lcc +lat_1=32.78333333333333 +lat_2=33.88333333333333 +lat_0=32.16666666666666 +lon_0=-116.25 +x_0=2000000 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneColoradoCentralFIPS0502Feet = new ProjectionInfo("+proj=lcc +lat_1=38.45 +lat_2=39.75 +lat_0=37.83333333333334 +lon_0=-105.5 +x_0=914401.8288999999 +y_0=304800.6096 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneColoradoNorthFIPS0501Feet = new ProjectionInfo("+proj=lcc +lat_1=39.71666666666667 +lat_2=40.78333333333333 +lat_0=39.33333333333334 +lon_0=-105.5 +x_0=914401.8288999999 +y_0=304800.6096 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneColoradoSouthFIPS0503Feet = new ProjectionInfo("+proj=lcc +lat_1=37.23333333333333 +lat_2=38.43333333333333 +lat_0=36.66666666666666 +lon_0=-105.5 +x_0=914401.8288999999 +y_0=304800.6096 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneConnecticutFIPS0600Feet = new ProjectionInfo("+proj=lcc +lat_1=41.2 +lat_2=41.86666666666667 +lat_0=40.83333333333334 +lon_0=-72.75 +x_0=304800.6096 +y_0=152400.3048 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneDelawareFIPS0700Feet = new ProjectionInfo("+proj=tmerc +lat_0=38 +lon_0=-75.41666666666667 +k=0.999995 +x_0=199999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneFloridaEastFIPS0901Feet = new ProjectionInfo("+proj=tmerc +lat_0=24.33333333333333 +lon_0=-81 +k=0.999941 +x_0=199999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneFloridaNorthFIPS0903Feet = new ProjectionInfo("+proj=lcc +lat_1=29.58333333333333 +lat_2=30.75 +lat_0=29 +lon_0=-84.5 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneFloridaWestFIPS0902Feet = new ProjectionInfo("+proj=tmerc +lat_0=24.33333333333333 +lon_0=-82 +k=0.999941 +x_0=199999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneGeorgiaEastFIPS1001Feet = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=-82.16666666666667 +k=0.999900 +x_0=199999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneGeorgiaWestFIPS1002Feet = new ProjectionInfo("+proj=tmerc +lat_0=30 +lon_0=-84.16666666666667 +k=0.999900 +x_0=699999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneHawaii1FIPS5101Feet = new ProjectionInfo("+proj=tmerc +lat_0=18.83333333333333 +lon_0=-155.5 +k=0.999967 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneHawaii2FIPS5102Feet = new ProjectionInfo("+proj=tmerc +lat_0=20.33333333333333 +lon_0=-156.6666666666667 +k=0.999967 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneHawaii3FIPS5103Feet = new ProjectionInfo("+proj=tmerc +lat_0=21.16666666666667 +lon_0=-158 +k=0.999990 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneHawaii4FIPS5104Feet = new ProjectionInfo("+proj=tmerc +lat_0=21.83333333333333 +lon_0=-159.5 +k=0.999990 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneHawaii5FIPS5105Feet = new ProjectionInfo("+proj=tmerc +lat_0=21.66666666666667 +lon_0=-160.1666666666667 +k=1.000000 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneIdahoCentralFIPS1102Feet = new ProjectionInfo("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-114 +k=0.999947 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneIdahoEastFIPS1101Feet = new ProjectionInfo("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-112.1666666666667 +k=0.999947 +x_0=199999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneIdahoWestFIPS1103Feet = new ProjectionInfo("+proj=tmerc +lat_0=41.66666666666666 +lon_0=-115.75 +k=0.999933 +x_0=799999.9999999998 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneIndianaEastFIPS1301Feet = new ProjectionInfo("+proj=tmerc +lat_0=37.5 +lon_0=-85.66666666666667 +k=0.999967 +x_0=99999.99999999999 +y_0=250000 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneIndianaWestFIPS1302Feet = new ProjectionInfo("+proj=tmerc +lat_0=37.5 +lon_0=-87.08333333333333 +k=0.999967 +x_0=900000 +y_0=250000 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneKentuckyNorthFIPS1601Feet = new ProjectionInfo("+proj=lcc +lat_1=37.96666666666667 +lat_2=38.96666666666667 +lat_0=37.5 +lon_0=-84.25 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneKentuckySouthFIPS1602Feet = new ProjectionInfo("+proj=lcc +lat_1=36.73333333333333 +lat_2=37.93333333333333 +lat_0=36.33333333333334 +lon_0=-85.75 +x_0=500000.0000000001 +y_0=500000.0000000001 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneMarylandFIPS1900Feet = new ProjectionInfo("+proj=lcc +lat_1=38.3 +lat_2=39.45 +lat_0=37.66666666666666 +lon_0=-77 +x_0=399999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneMassachusettsIslandFIPS2002Feet = new ProjectionInfo("+proj=lcc +lat_1=41.28333333333333 +lat_2=41.48333333333333 +lat_0=41 +lon_0=-70.5 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001Feet = new ProjectionInfo("+proj=lcc +lat_1=41.71666666666667 +lat_2=42.68333333333333 +lat_0=41 +lon_0=-71.5 +x_0=199999.9999999999 +y_0=750000 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneMichiganCentralFIPS2112FeetIntl = new ProjectionInfo("+proj=lcc +lat_1=44.18333333333333 +lat_2=45.7 +lat_0=43.31666666666667 +lon_0=-84.36666666666666 +x_0=6000000 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneMichiganNorthFIPS2111FeetIntl = new ProjectionInfo("+proj=lcc +lat_1=45.48333333333333 +lat_2=47.08333333333334 +lat_0=44.78333333333333 +lon_0=-87 +x_0=7999999.999999998 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneMichiganSouthFIPS2113FeetIntl = new ProjectionInfo("+proj=lcc +lat_1=42.1 +lat_2=43.66666666666666 +lat_0=41.5 +lon_0=-84.36666666666666 +x_0=3999999.999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneMississippiEastFIPS2301Feet = new ProjectionInfo("+proj=tmerc +lat_0=29.5 +lon_0=-88.83333333333333 +k=0.999950 +x_0=300000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneMississippiWestFIPS2302Feet = new ProjectionInfo("+proj=tmerc +lat_0=29.5 +lon_0=-90.33333333333333 +k=0.999950 +x_0=699999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneMontanaFIPS2500FeetIntl = new ProjectionInfo("+proj=lcc +lat_1=45 +lat_2=49 +lat_0=44.25 +lon_0=-109.5 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneNewMexicoCentralFIPS3002Feet = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-106.25 +k=0.999900 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNewMexicoEastFIPS3001Feet = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-104.3333333333333 +k=0.999909 +x_0=165000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNewMexicoWestFIPS3003Feet = new ProjectionInfo("+proj=tmerc +lat_0=31 +lon_0=-107.8333333333333 +k=0.999917 +x_0=829999.9999999998 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNewYorkCentralFIPS3102Feet = new ProjectionInfo("+proj=tmerc +lat_0=40 +lon_0=-76.58333333333333 +k=0.999938 +x_0=250000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNewYorkEastFIPS3101Feet = new ProjectionInfo("+proj=tmerc +lat_0=38.83333333333334 +lon_0=-74.5 +k=0.999900 +x_0=150000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104Feet = new ProjectionInfo("+proj=lcc +lat_1=40.66666666666666 +lat_2=41.03333333333333 +lat_0=40.16666666666666 +lon_0=-74 +x_0=300000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNewYorkWestFIPS3103Feet = new ProjectionInfo("+proj=tmerc +lat_0=40 +lon_0=-78.58333333333333 +k=0.999938 +x_0=350000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301FeetIntl = new ProjectionInfo("+proj=lcc +lat_1=47.43333333333333 +lat_2=48.73333333333333 +lat_0=47 +lon_0=-100.5 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302FeetIntl = new ProjectionInfo("+proj=lcc +lat_1=46.18333333333333 +lat_2=47.48333333333333 +lat_0=45.66666666666666 +lon_0=-100.5 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneOklahomaNorthFIPS3501Feet = new ProjectionInfo("+proj=lcc +lat_1=35.56666666666667 +lat_2=36.76666666666667 +lat_0=35 +lon_0=-98 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneOklahomaSouthFIPS3502Feet = new ProjectionInfo("+proj=lcc +lat_1=33.93333333333333 +lat_2=35.23333333333333 +lat_0=33.33333333333334 +lon_0=-98 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneOregonNorthFIPS3601FeetIntl = new ProjectionInfo("+proj=lcc +lat_1=44.33333333333334 +lat_2=46 +lat_0=43.66666666666666 +lon_0=-120.5 +x_0=2500000 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneOregonSouthFIPS3602FeetIntl = new ProjectionInfo("+proj=lcc +lat_1=42.33333333333334 +lat_2=44 +lat_0=41.66666666666666 +lon_0=-120.5 +x_0=1500000 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneTennesseeFIPS4100Feet = new ProjectionInfo("+proj=lcc +lat_1=35.25 +lat_2=36.41666666666666 +lat_0=34.33333333333334 +lon_0=-86 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneTexasCentralFIPS4203Feet = new ProjectionInfo("+proj=lcc +lat_1=30.11666666666667 +lat_2=31.88333333333333 +lat_0=29.66666666666667 +lon_0=-100.3333333333333 +x_0=699999.9999999999 +y_0=3000000 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneTexasNorthCentralFIPS4202Feet = new ProjectionInfo("+proj=lcc +lat_1=32.13333333333333 +lat_2=33.96666666666667 +lat_0=31.66666666666667 +lon_0=-98.5 +x_0=600000 +y_0=2000000 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneTexasNorthFIPS4201Feet = new ProjectionInfo("+proj=lcc +lat_1=34.65 +lat_2=36.18333333333333 +lat_0=34 +lon_0=-101.5 +x_0=199999.9999999999 +y_0=999999.9999999999 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneTexasSouthCentralFIPS4204Feet = new ProjectionInfo("+proj=lcc +lat_1=28.38333333333333 +lat_2=30.28333333333333 +lat_0=27.83333333333333 +lon_0=-99 +x_0=600000 +y_0=3999999.999999999 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneTexasSouthFIPS4205Feet = new ProjectionInfo("+proj=lcc +lat_1=26.16666666666667 +lat_2=27.83333333333333 +lat_0=25.66666666666667 +lon_0=-98.5 +x_0=300000 +y_0=4999999.999999998 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneUtahCentralFIPS4302FeetIntl = new ProjectionInfo("+proj=lcc +lat_1=39.01666666666667 +lat_2=40.65 +lat_0=38.33333333333334 +lon_0=-111.5 +x_0=499999.9999999998 +y_0=2000000 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneUtahNorthFIPS4301FeetIntl = new ProjectionInfo("+proj=lcc +lat_1=40.71666666666667 +lat_2=41.78333333333333 +lat_0=40.33333333333334 +lon_0=-111.5 +x_0=499999.9999999998 +y_0=999999.9999999999 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneUtahSouthFIPS4303FeetIntl = new ProjectionInfo("+proj=lcc +lat_1=37.21666666666667 +lat_2=38.35 +lat_0=36.66666666666666 +lon_0=-111.5 +x_0=499999.9999999998 +y_0=3000000 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983HARNStatePlaneVirginiaNorthFIPS4501Feet = new ProjectionInfo("+proj=lcc +lat_1=38.03333333333333 +lat_2=39.2 +lat_0=37.66666666666666 +lon_0=-78.5 +x_0=3499999.999999998 +y_0=2000000 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneVirginiaSouthFIPS4502Feet = new ProjectionInfo("+proj=lcc +lat_1=36.76666666666667 +lat_2=37.96666666666667 +lat_0=36.33333333333334 +lon_0=-78.5 +x_0=3499999.999999998 +y_0=999999.9999999999 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneWashingtonNorthFIPS4601Feet = new ProjectionInfo("+proj=lcc +lat_1=47.5 +lat_2=48.73333333333333 +lat_0=47 +lon_0=-120.8333333333333 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneWashingtonSouthFIPS4602Feet = new ProjectionInfo("+proj=lcc +lat_1=45.83333333333334 +lat_2=47.33333333333334 +lat_0=45.33333333333334 +lon_0=-120.5 +x_0=500000.0000000001 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneWisconsinCentralFIPS4802Feet = new ProjectionInfo("+proj=lcc +lat_1=44.25 +lat_2=45.5 +lat_0=43.83333333333334 +lon_0=-90 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneWisconsinNorthFIPS4801Feet = new ProjectionInfo("+proj=lcc +lat_1=45.56666666666667 +lat_2=46.76666666666667 +lat_0=45.16666666666666 +lon_0=-90 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNStatePlaneWisconsinSouthFIPS4803Feet = new ProjectionInfo("+proj=lcc +lat_1=42.73333333333333 +lat_2=44.06666666666667 +lat_0=42 +lon_0=-90 +x_0=600000 +y_0=0 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591