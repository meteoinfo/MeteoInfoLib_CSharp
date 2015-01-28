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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:33:59 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// Wisconsin
    /// </summary>
    public class Wisconsin : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1983HARNAdjWIAdamsFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIAdamsMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIAshlandFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIAshlandMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIBarronFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIBarronMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIBayfieldFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIBayfieldMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIBrownFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIBrownMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIBuffaloFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIBuffaloMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIBurnettFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIBurnettMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWICalumetFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWICalumetMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIChippewaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIChippewaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIClarkFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIClarkMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIColumbiaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIColumbiaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWICrawfordFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWICrawfordMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIDaneFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIDaneMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIDodgeFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIDodgeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIDoorFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIDoorMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIDouglasFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIDouglasMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIDunnFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIDunnMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIEauClaireFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIEauClaireMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIFlorenceFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIFlorenceMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIFondduLacFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIFondduLacMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIForestFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIForestMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIGrantFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIGrantMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIGreenFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIGreenLakeFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIGreenLakeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIGreenMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIIowaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIIowaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIIronFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIIronMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIJacksonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIJacksonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIJeffersonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIJeffersonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIJuneauFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIJuneauMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIKenoshaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIKenoshaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIKewauneeFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIKewauneeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWILaCrosseFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWILaCrosseMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWILafayetteFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWILafayetteMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWILangladeFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWILangladeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWILincolnFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWILincolnMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIManitowocFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIManitowocMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIMarathonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIMarathonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIMarinetteFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIMarinetteMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIMarquetteFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIMarquetteMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIMenomineeFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIMenomineeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIMilwaukeeFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIMilwaukeeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIMonroeFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIMonroeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIOcontoFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIOcontoMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIOneidaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIOneidaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIOutagamieFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIOutagamieMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIOzaukeeFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIOzaukeeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIPepinFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIPepinMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIPierceFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIPierceMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIPolkFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIPolkMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIPortageFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIPortageMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIPriceFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIPriceMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIRacineFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIRacineMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIRichlandFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIRichlandMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIRockFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIRockMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIRuskFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIRuskMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWISaukFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWISaukMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWISawyerFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWISawyerMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIShawanoFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIShawanoMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWISheboyganFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWISheboyganMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIStCroixFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIStCroixMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWITaylorFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWITaylorMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWITrempealeauFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWITrempealeauMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIVernonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIVernonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIVilasFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIVilasMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIWalworthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIWalworthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIWashburnFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIWashburnMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIWashingtonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIWashingtonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIWaukeshaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIWaukeshaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIWaupacaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIWaupacaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIWausharaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIWausharaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIWinnebagoFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIWinnebagoMeters;
        public readonly ProjectionInfo NAD1983HARNAdjWIWoodFeet;
        public readonly ProjectionInfo NAD1983HARNAdjWIWoodMeters;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Wisconsin
        /// </summary>
        public Wisconsin()
        {
            NAD1983HARNAdjWIAdamsFeet = new ProjectionInfo("+proj=tmerc +lat_0=43.36666666666667 +lon_0=-90 +k=0.999999 +x_0=147218.6944373889 +y_0=0 +a=6378376.271 +b=6356991.5851403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIAdamsMeters = new ProjectionInfo("+proj=tmerc +lat_0=43.36666666666667 +lon_0=-90 +k=0.999999 +x_0=147218.6944373889 +y_0=0 +a=6378376.271 +b=6356991.5851403 +units=m +no_defs ");
            NAD1983HARNAdjWIAshlandFeet = new ProjectionInfo("+proj=tmerc +lat_0=45.70611111111111 +lon_0=-90.62222222222222 +k=0.999997 +x_0=172821.9456438913 +y_0=0 +a=6378471.92 +b=6357087.2341403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIAshlandMeters = new ProjectionInfo("+proj=tmerc +lat_0=45.70611111111111 +lon_0=-90.62222222222222 +k=0.999997 +x_0=172821.9456438913 +y_0=0 +a=6378471.92 +b=6357087.2341403 +units=m +no_defs ");
            NAD1983HARNAdjWIBarronFeet = new ProjectionInfo("+proj=tmerc +lat_0=45.13333333333333 +lon_0=-91.84999999999999 +k=0.999996 +x_0=93150 +y_0=0 +a=6378472.931 +b=6357088.2451403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIBarronMeters = new ProjectionInfo("+proj=tmerc +lat_0=45.13333333333333 +lon_0=-91.84999999999999 +k=0.999996 +x_0=93150 +y_0=0 +a=6378472.931 +b=6357088.2451403 +units=m +no_defs ");
            NAD1983HARNAdjWIBayfieldFeet = new ProjectionInfo("+proj=lcc +lat_1=46.41388888888888 +lat_2=46.925 +lat_0=45.33333333333334 +lon_0=-91.15277777777779 +x_0=228600.4572009144 +y_0=0 +a=6378411.351 +b=6357026.6651403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIBayfieldMeters = new ProjectionInfo("+proj=lcc +lat_1=46.41388888888888 +lat_2=46.925 +lat_0=45.33333333333334 +lon_0=-91.15277777777779 +x_0=228600.4572009144 +y_0=0 +a=6378411.351 +b=6357026.6651403 +units=m +no_defs ");
            NAD1983HARNAdjWIBrownFeet = new ProjectionInfo("+proj=tmerc +lat_0=43 +lon_0=-88 +k=1.000020 +x_0=31599.99998983998 +y_0=4599.989839979679 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIBrownMeters = new ProjectionInfo("+proj=tmerc +lat_0=43 +lon_0=-88 +k=1.000020 +x_0=31599.99998984 +y_0=4599.98983997968 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNAdjWIBuffaloFeet = new ProjectionInfo("+proj=tmerc +lat_0=43.48138888888889 +lon_0=-91.79722222222222 +k=1.000000 +x_0=175260.350520701 +y_0=0 +a=6378380.991 +b=6356996.305140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIBuffaloMeters = new ProjectionInfo("+proj=tmerc +lat_0=43.48138888888889 +lon_0=-91.79722222222222 +k=1.000000 +x_0=175260.3505207011 +y_0=0 +a=6378380.991 +b=6356996.305140301 +units=m +no_defs ");
            NAD1983HARNAdjWIBurnettFeet = new ProjectionInfo("+proj=lcc +lat_1=45.71388888888889 +lat_2=46.08333333333334 +lat_0=45.36388888888889 +lon_0=-92.45777777777778 +x_0=64008.12801625603 +y_0=0 +a=6378414.96 +b=6357030.2741403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIBurnettMeters = new ProjectionInfo("+proj=lcc +lat_1=45.71388888888889 +lat_2=46.08333333333334 +lat_0=45.36388888888889 +lon_0=-92.45777777777778 +x_0=64008.12801625604 +y_0=0 +a=6378414.96 +b=6357030.2741403 +units=m +no_defs ");
            NAD1983HARNAdjWICalumetFeet = new ProjectionInfo("+proj=tmerc +lat_0=42.71944444444445 +lon_0=-88.5 +k=0.999996 +x_0=244754.889509779 +y_0=0 +a=6378345.09 +b=6356960.4041403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWICalumetMeters = new ProjectionInfo("+proj=tmerc +lat_0=42.71944444444445 +lon_0=-88.5 +k=0.999996 +x_0=244754.8895097791 +y_0=0 +a=6378345.09 +b=6356960.4041403 +units=m +no_defs ");
            NAD1983HARNAdjWIChippewaFeet = new ProjectionInfo("+proj=lcc +lat_1=44.81388888888888 +lat_2=45.14166666666667 +lat_0=44.58111111111111 +lon_0=-91.29444444444444 +x_0=60045.72009144018 +y_0=0 +a=6378412.542 +b=6357027.856140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIChippewaMeters = new ProjectionInfo("+proj=lcc +lat_1=44.81388888888888 +lat_2=45.14166666666667 +lat_0=44.58111111111111 +lon_0=-91.29444444444444 +x_0=60045.72009144019 +y_0=0 +a=6378412.542 +b=6357027.856140301 +units=m +no_defs ");
            NAD1983HARNAdjWIClarkFeet = new ProjectionInfo("+proj=tmerc +lat_0=43.6 +lon_0=-90.70833333333334 +k=0.999994 +x_0=199949.1998983998 +y_0=0 +a=6378470.401 +b=6357085.7151403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIClarkMeters = new ProjectionInfo("+proj=tmerc +lat_0=43.6 +lon_0=-90.70833333333334 +k=0.999994 +x_0=199949.1998984 +y_0=0 +a=6378470.401 +b=6357085.7151403 +units=m +no_defs ");
            NAD1983HARNAdjWIColumbiaFeet = new ProjectionInfo("+proj=lcc +lat_1=43.33333333333334 +lat_2=43.59166666666667 +lat_0=42.45833333333334 +lon_0=-89.39444444444445 +x_0=169164.3383286767 +y_0=0 +a=6378376.331 +b=6356991.645140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIColumbiaMeters = new ProjectionInfo("+proj=lcc +lat_1=43.33333333333334 +lat_2=43.59166666666667 +lat_0=42.45833333333334 +lon_0=-89.39444444444445 +x_0=169164.3383286767 +y_0=0 +a=6378376.331 +b=6356991.645140301 +units=m +no_defs ");
            NAD1983HARNAdjWICrawfordFeet = new ProjectionInfo("+proj=lcc +lat_1=43.05833333333333 +lat_2=43.34166666666667 +lat_0=42.71666666666667 +lon_0=-90.9388888888889 +x_0=113690.6273812548 +y_0=0 +a=6378379.031 +b=6356994.345140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWICrawfordMeters = new ProjectionInfo("+proj=lcc +lat_1=43.05833333333333 +lat_2=43.34166666666667 +lat_0=42.71666666666667 +lon_0=-90.9388888888889 +x_0=113690.6273812548 +y_0=0 +a=6378379.031 +b=6356994.345140301 +units=m +no_defs ");
            NAD1983HARNAdjWIDaneFeet = new ProjectionInfo("+proj=lcc +lat_1=42.90833333333333 +lat_2=43.23055555555555 +lat_0=41.75 +lon_0=-89.42222222222223 +x_0=247193.2943865888 +y_0=0 +a=6378407.621 +b=6357022.935140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIDaneMeters = new ProjectionInfo("+proj=lcc +lat_1=42.90833333333333 +lat_2=43.23055555555555 +lat_0=41.75 +lon_0=-89.42222222222223 +x_0=247193.2943865888 +y_0=0 +a=6378407.621 +b=6357022.935140301 +units=m +no_defs ");
            NAD1983HARNAdjWIDodgeFeet = new ProjectionInfo("+proj=tmerc +lat_0=41.47222222222222 +lon_0=-88.77500000000001 +k=0.999997 +x_0=263347.7266954534 +y_0=0 +a=6378376.811 +b=6356992.1251403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIDodgeMeters = new ProjectionInfo("+proj=tmerc +lat_0=41.47222222222222 +lon_0=-88.77500000000001 +k=0.999997 +x_0=263347.7266954534 +y_0=0 +a=6378376.811 +b=6356992.1251403 +units=m +no_defs ");
            NAD1983HARNAdjWIDoorFeet = new ProjectionInfo("+proj=tmerc +lat_0=44.4 +lon_0=-87.27222222222223 +k=0.999991 +x_0=158801.1176022352 +y_0=0 +a=6378313.92 +b=6356929.2341403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIDoorMeters = new ProjectionInfo("+proj=tmerc +lat_0=44.4 +lon_0=-87.27222222222223 +k=0.999991 +x_0=158801.1176022352 +y_0=0 +a=6378313.92 +b=6356929.2341403 +units=m +no_defs ");
            NAD1983HARNAdjWIDouglasFeet = new ProjectionInfo("+proj=tmerc +lat_0=45.88333333333333 +lon_0=-91.91666666666667 +k=0.999995 +x_0=59131.31826263652 +y_0=0 +a=6378414.93 +b=6357030.2441403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIDouglasMeters = new ProjectionInfo("+proj=tmerc +lat_0=45.88333333333333 +lon_0=-91.91666666666667 +k=0.999995 +x_0=59131.31826263653 +y_0=0 +a=6378414.93 +b=6357030.2441403 +units=m +no_defs ");
            NAD1983HARNAdjWIDunnFeet = new ProjectionInfo("+proj=tmerc +lat_0=44.40833333333333 +lon_0=-91.89444444444445 +k=0.999998 +x_0=51816.10363220726 +y_0=0 +a=6378413.021 +b=6357028.3351403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIDunnMeters = new ProjectionInfo("+proj=tmerc +lat_0=44.40833333333333 +lon_0=-91.89444444444445 +k=0.999998 +x_0=51816.10363220727 +y_0=0 +a=6378413.021 +b=6357028.3351403 +units=m +no_defs ");
            NAD1983HARNAdjWIEauClaireFeet = new ProjectionInfo("+proj=lcc +lat_1=44.73055555555555 +lat_2=45.01388888888889 +lat_0=44.04722222222222 +lon_0=-91.28888888888889 +x_0=120091.4401828804 +y_0=0 +a=6378380.381 +b=6356995.6951403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIEauClaireMeters = new ProjectionInfo("+proj=lcc +lat_1=44.73055555555555 +lat_2=45.01388888888889 +lat_0=44.04722222222222 +lon_0=-91.28888888888889 +x_0=120091.4401828804 +y_0=0 +a=6378380.381 +b=6356995.6951403 +units=m +no_defs ");
            NAD1983HARNAdjWIFlorenceFeet = new ProjectionInfo("+proj=tmerc +lat_0=45.43888888888888 +lon_0=-88.14166666666668 +k=0.999993 +x_0=133502.667005334 +y_0=0 +a=6378530.851 +b=6357146.1651403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIFlorenceMeters = new ProjectionInfo("+proj=tmerc +lat_0=45.43888888888888 +lon_0=-88.14166666666668 +k=0.999993 +x_0=133502.667005334 +y_0=0 +a=6378530.851 +b=6357146.1651403 +units=m +no_defs ");
            NAD1983HARNAdjWIFondduLacFeet = new ProjectionInfo("+proj=tmerc +lat_0=42.71944444444445 +lon_0=-88.5 +k=0.999996 +x_0=244754.889509779 +y_0=0 +a=6378345.09 +b=6356960.4041403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIFondduLacMeters = new ProjectionInfo("+proj=tmerc +lat_0=42.71944444444445 +lon_0=-88.5 +k=0.999996 +x_0=244754.8895097791 +y_0=0 +a=6378345.09 +b=6356960.4041403 +units=m +no_defs ");
            NAD1983HARNAdjWIForestFeet = new ProjectionInfo("+proj=tmerc +lat_0=44.00555555555555 +lon_0=-88.63333333333334 +k=0.999996 +x_0=275844.5516891034 +y_0=0 +a=6378591.521 +b=6357206.8351403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIForestMeters = new ProjectionInfo("+proj=tmerc +lat_0=44.00555555555555 +lon_0=-88.63333333333334 +k=0.999996 +x_0=275844.5516891034 +y_0=0 +a=6378591.521 +b=6357206.8351403 +units=m +no_defs ");
            NAD1983HARNAdjWIGrantFeet = new ProjectionInfo("+proj=tmerc +lat_0=41.41111111111111 +lon_0=-90.8 +k=0.999997 +x_0=242316.4846329693 +y_0=0 +a=6378378.881 +b=6356994.1951403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIGrantMeters = new ProjectionInfo("+proj=tmerc +lat_0=41.41111111111111 +lon_0=-90.8 +k=0.999997 +x_0=242316.4846329693 +y_0=0 +a=6378378.881 +b=6356994.1951403 +units=m +no_defs ");
            NAD1983HARNAdjWIGreenFeet = new ProjectionInfo("+proj=lcc +lat_1=42.48611111111111 +lat_2=42.78888888888888 +lat_0=42.225 +lon_0=-89.83888888888889 +x_0=170078.7401574803 +y_0=0 +a=6378408.481 +b=6357023.7951403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIGreenLakeFeet = new ProjectionInfo("+proj=lcc +lat_1=43.66666666666666 +lat_2=43.94722222222222 +lat_0=43.09444444444445 +lon_0=-89.24166666666667 +x_0=150876.3017526035 +y_0=0 +a=6378375.601 +b=6356990.9151403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIGreenLakeMeters = new ProjectionInfo("+proj=lcc +lat_1=43.66666666666666 +lat_2=43.94722222222222 +lat_0=43.09444444444445 +lon_0=-89.24166666666667 +x_0=150876.3017526035 +y_0=0 +a=6378375.601 +b=6356990.9151403 +units=m +no_defs ");
            NAD1983HARNAdjWIGreenMeters = new ProjectionInfo("+proj=lcc +lat_1=42.48611111111111 +lat_2=42.78888888888888 +lat_0=42.225 +lon_0=-89.83888888888889 +x_0=170078.7401574803 +y_0=0 +a=6378408.481 +b=6357023.7951403 +units=m +no_defs ");
            NAD1983HARNAdjWIIowaFeet = new ProjectionInfo("+proj=tmerc +lat_0=42.53888888888888 +lon_0=-90.16111111111111 +k=0.999997 +x_0=113081.0261620523 +y_0=0 +a=6378408.041 +b=6357023.355140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIIowaMeters = new ProjectionInfo("+proj=tmerc +lat_0=42.53888888888888 +lon_0=-90.16111111111111 +k=0.999997 +x_0=113081.0261620523 +y_0=0 +a=6378408.041 +b=6357023.355140301 +units=m +no_defs ");
            NAD1983HARNAdjWIIronFeet = new ProjectionInfo("+proj=tmerc +lat_0=45.43333333333333 +lon_0=-90.25555555555556 +k=0.999996 +x_0=220980.4419608839 +y_0=0 +a=6378655.071000001 +b=6357270.385140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIIronMeters = new ProjectionInfo("+proj=tmerc +lat_0=45.43333333333333 +lon_0=-90.25555555555556 +k=0.999996 +x_0=220980.4419608839 +y_0=0 +a=6378655.071000001 +b=6357270.385140301 +units=m +no_defs ");
            NAD1983HARNAdjWIJacksonFeet = new ProjectionInfo("+proj=lcc +lat_1=44.16388888888888 +lat_2=44.41944444444444 +lat_0=43.79444444444444 +lon_0=-90.73888888888889 +x_0=125882.6517653035 +y_0=0 +a=6378409.151 +b=6357024.4651403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIJacksonMeters = new ProjectionInfo("+proj=lcc +lat_1=44.16388888888888 +lat_2=44.41944444444444 +lat_0=43.79444444444444 +lon_0=-90.73888888888889 +x_0=125882.6517653035 +y_0=0 +a=6378409.151 +b=6357024.4651403 +units=m +no_defs ");
            NAD1983HARNAdjWIJeffersonFeet = new ProjectionInfo("+proj=tmerc +lat_0=41.47222222222222 +lon_0=-88.77500000000001 +k=0.999997 +x_0=263347.7266954534 +y_0=0 +a=6378376.811 +b=6356992.1251403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIJeffersonMeters = new ProjectionInfo("+proj=tmerc +lat_0=41.47222222222222 +lon_0=-88.77500000000001 +k=0.999997 +x_0=263347.7266954534 +y_0=0 +a=6378376.811 +b=6356992.1251403 +units=m +no_defs ");
            NAD1983HARNAdjWIJuneauFeet = new ProjectionInfo("+proj=tmerc +lat_0=43.36666666666667 +lon_0=-90 +k=0.999999 +x_0=147218.6944373889 +y_0=0 +a=6378376.271 +b=6356991.5851403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIJuneauMeters = new ProjectionInfo("+proj=tmerc +lat_0=43.36666666666667 +lon_0=-90 +k=0.999999 +x_0=147218.6944373889 +y_0=0 +a=6378376.271 +b=6356991.5851403 +units=m +no_defs ");
            NAD1983HARNAdjWIKenoshaFeet = new ProjectionInfo("+proj=tmerc +lat_0=42.21666666666667 +lon_0=-87.89444444444445 +k=0.999998 +x_0=185928.3718567437 +y_0=0 +a=6378315.7 +b=6356931.014140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIKenoshaMeters = new ProjectionInfo("+proj=tmerc +lat_0=42.21666666666667 +lon_0=-87.89444444444445 +k=0.999998 +x_0=185928.3718567437 +y_0=0 +a=6378315.7 +b=6356931.014140301 +units=m +no_defs ");
            NAD1983HARNAdjWIKewauneeFeet = new ProjectionInfo("+proj=tmerc +lat_0=43.26666666666667 +lon_0=-87.55 +k=1.000000 +x_0=79857.75971551942 +y_0=0 +a=6378285.86 +b=6356901.174140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIKewauneeMeters = new ProjectionInfo("+proj=tmerc +lat_0=43.26666666666667 +lon_0=-87.55 +k=1.000000 +x_0=79857.75971551944 +y_0=0 +a=6378285.86 +b=6356901.174140301 +units=m +no_defs ");
            NAD1983HARNAdjWILaCrosseFeet = new ProjectionInfo("+proj=tmerc +lat_0=43.45111111111111 +lon_0=-91.31666666666666 +k=0.999994 +x_0=130454.6609093218 +y_0=0 +a=6378379.301 +b=6356994.6151403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWILaCrosseMeters = new ProjectionInfo("+proj=tmerc +lat_0=43.45111111111111 +lon_0=-91.31666666666666 +k=0.999994 +x_0=130454.6609093218 +y_0=0 +a=6378379.301 +b=6356994.6151403 +units=m +no_defs ");
            NAD1983HARNAdjWILafayetteFeet = new ProjectionInfo("+proj=lcc +lat_1=42.48611111111111 +lat_2=42.78888888888888 +lat_0=42.225 +lon_0=-89.83888888888889 +x_0=170078.7401574803 +y_0=0 +a=6378408.481 +b=6357023.7951403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWILafayetteMeters = new ProjectionInfo("+proj=lcc +lat_1=42.48611111111111 +lat_2=42.78888888888888 +lat_0=42.225 +lon_0=-89.83888888888889 +x_0=170078.7401574803 +y_0=0 +a=6378408.481 +b=6357023.7951403 +units=m +no_defs ");
            NAD1983HARNAdjWILangladeFeet = new ProjectionInfo("+proj=lcc +lat_1=45 +lat_2=45.30833333333333 +lat_0=44.20694444444445 +lon_0=-89.03333333333333 +x_0=198425.1968503937 +y_0=0 +a=6378560.121 +b=6357175.435140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWILangladeMeters = new ProjectionInfo("+proj=lcc +lat_1=45 +lat_2=45.30833333333333 +lat_0=44.20694444444445 +lon_0=-89.03333333333333 +x_0=198425.1968503937 +y_0=0 +a=6378560.121 +b=6357175.435140301 +units=m +no_defs ");
            NAD1983HARNAdjWILincolnFeet = new ProjectionInfo("+proj=tmerc +lat_0=44.84444444444445 +lon_0=-89.73333333333333 +k=0.999998 +x_0=116129.0322580645 +y_0=0 +a=6378531.821000001 +b=6357147.135140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWILincolnMeters = new ProjectionInfo("+proj=tmerc +lat_0=44.84444444444445 +lon_0=-89.73333333333333 +k=0.999998 +x_0=116129.0322580645 +y_0=0 +a=6378531.821000001 +b=6357147.135140301 +units=m +no_defs ");
            NAD1983HARNAdjWIManitowocFeet = new ProjectionInfo("+proj=tmerc +lat_0=43.26666666666667 +lon_0=-87.55 +k=1.000000 +x_0=79857.75971551942 +y_0=0 +a=6378285.86 +b=6356901.174140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIManitowocMeters = new ProjectionInfo("+proj=tmerc +lat_0=43.26666666666667 +lon_0=-87.55 +k=1.000000 +x_0=79857.75971551944 +y_0=0 +a=6378285.86 +b=6356901.174140301 +units=m +no_defs ");
            NAD1983HARNAdjWIMarathonFeet = new ProjectionInfo("+proj=lcc +lat_1=44.74527777777778 +lat_2=45.05638888888888 +lat_0=44.40555555555555 +lon_0=-89.77 +x_0=74676.1493522987 +y_0=0 +a=6378500.6 +b=6357115.9141403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIMarathonMeters = new ProjectionInfo("+proj=lcc +lat_1=44.74527777777778 +lat_2=45.05638888888888 +lat_0=44.40555555555555 +lon_0=-89.77 +x_0=74676.14935229872 +y_0=0 +a=6378500.6 +b=6357115.9141403 +units=m +no_defs ");
            NAD1983HARNAdjWIMarinetteFeet = new ProjectionInfo("+proj=tmerc +lat_0=44.69166666666666 +lon_0=-87.71111111111111 +k=0.999986 +x_0=238658.8773177546 +y_0=0 +a=6378376.041 +b=6356991.355140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIMarinetteMeters = new ProjectionInfo("+proj=tmerc +lat_0=44.69166666666666 +lon_0=-87.71111111111111 +k=0.999986 +x_0=238658.8773177547 +y_0=0 +a=6378376.041 +b=6356991.355140301 +units=m +no_defs ");
            NAD1983HARNAdjWIMarquetteFeet = new ProjectionInfo("+proj=lcc +lat_1=43.66666666666666 +lat_2=43.94722222222222 +lat_0=43.09444444444445 +lon_0=-89.24166666666667 +x_0=150876.3017526035 +y_0=0 +a=6378375.601 +b=6356990.9151403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIMarquetteMeters = new ProjectionInfo("+proj=lcc +lat_1=43.66666666666666 +lat_2=43.94722222222222 +lat_0=43.09444444444445 +lon_0=-89.24166666666667 +x_0=150876.3017526035 +y_0=0 +a=6378375.601 +b=6356990.9151403 +units=m +no_defs ");
            NAD1983HARNAdjWIMenomineeFeet = new ProjectionInfo("+proj=tmerc +lat_0=44.71666666666667 +lon_0=-88.41666666666667 +k=0.999994 +x_0=105461.0109220218 +y_0=0 +a=6378406.601 +b=6357021.9151403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIMenomineeMeters = new ProjectionInfo("+proj=tmerc +lat_0=44.71666666666667 +lon_0=-88.41666666666667 +k=0.999994 +x_0=105461.0109220219 +y_0=0 +a=6378406.601 +b=6357021.9151403 +units=m +no_defs ");
            NAD1983HARNAdjWIMilwaukeeFeet = new ProjectionInfo("+proj=tmerc +lat_0=42.21666666666667 +lon_0=-87.89444444444445 +k=0.999998 +x_0=185928.3718567437 +y_0=0 +a=6378315.7 +b=6356931.014140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIMilwaukeeMeters = new ProjectionInfo("+proj=tmerc +lat_0=42.21666666666667 +lon_0=-87.89444444444445 +k=0.999998 +x_0=185928.3718567437 +y_0=0 +a=6378315.7 +b=6356931.014140301 +units=m +no_defs ");
            NAD1983HARNAdjWIMonroeFeet = new ProjectionInfo("+proj=lcc +lat_1=43.83888888888889 +lat_2=44.16111111111111 +lat_0=42.90277777777778 +lon_0=-90.64166666666668 +x_0=204521.2090424181 +y_0=0 +a=6378438.991 +b=6357054.305140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIMonroeMeters = new ProjectionInfo("+proj=lcc +lat_1=43.83888888888889 +lat_2=44.16111111111111 +lat_0=42.90277777777778 +lon_0=-90.64166666666668 +x_0=204521.2090424181 +y_0=0 +a=6378438.991 +b=6357054.305140301 +units=m +no_defs ");
            NAD1983HARNAdjWIOcontoFeet = new ProjectionInfo("+proj=tmerc +lat_0=44.39722222222222 +lon_0=-87.90833333333335 +k=0.999991 +x_0=182880.3657607315 +y_0=0 +a=6378345.42 +b=6356960.7341403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIOcontoMeters = new ProjectionInfo("+proj=tmerc +lat_0=44.39722222222222 +lon_0=-87.90833333333335 +k=0.999991 +x_0=182880.3657607315 +y_0=0 +a=6378345.42 +b=6356960.7341403 +units=m +no_defs ");
            NAD1983HARNAdjWIOneidaFeet = new ProjectionInfo("+proj=lcc +lat_1=45.56666666666667 +lat_2=45.84166666666667 +lat_0=45.18611111111111 +lon_0=-89.54444444444444 +x_0=70104.14020828041 +y_0=0 +a=6378593.86 +b=6357209.174140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIOneidaMeters = new ProjectionInfo("+proj=lcc +lat_1=45.56666666666667 +lat_2=45.84166666666667 +lat_0=45.18611111111111 +lon_0=-89.54444444444444 +x_0=70104.14020828043 +y_0=0 +a=6378593.86 +b=6357209.174140301 +units=m +no_defs ");
            NAD1983HARNAdjWIOutagamieFeet = new ProjectionInfo("+proj=tmerc +lat_0=42.71944444444445 +lon_0=-88.5 +k=0.999996 +x_0=244754.889509779 +y_0=0 +a=6378345.09 +b=6356960.4041403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIOutagamieMeters = new ProjectionInfo("+proj=tmerc +lat_0=42.71944444444445 +lon_0=-88.5 +k=0.999996 +x_0=244754.8895097791 +y_0=0 +a=6378345.09 +b=6356960.4041403 +units=m +no_defs ");
            NAD1983HARNAdjWIOzaukeeFeet = new ProjectionInfo("+proj=tmerc +lat_0=42.21666666666667 +lon_0=-87.89444444444445 +k=0.999998 +x_0=185928.3718567437 +y_0=0 +a=6378315.7 +b=6356931.014140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIOzaukeeMeters = new ProjectionInfo("+proj=tmerc +lat_0=42.21666666666667 +lon_0=-87.89444444444445 +k=0.999998 +x_0=185928.3718567437 +y_0=0 +a=6378315.7 +b=6356931.014140301 +units=m +no_defs ");
            NAD1983HARNAdjWIPepinFeet = new ProjectionInfo("+proj=lcc +lat_1=44.52222222222222 +lat_2=44.75 +lat_0=43.86194444444445 +lon_0=-92.22777777777777 +x_0=167640.3352806706 +y_0=0 +a=6378381.271 +b=6356996.5851403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIPepinMeters = new ProjectionInfo("+proj=lcc +lat_1=44.52222222222222 +lat_2=44.75 +lat_0=43.86194444444445 +lon_0=-92.22777777777777 +x_0=167640.3352806706 +y_0=0 +a=6378381.271 +b=6356996.5851403 +units=m +no_defs ");
            NAD1983HARNAdjWIPierceFeet = new ProjectionInfo("+proj=lcc +lat_1=44.52222222222222 +lat_2=44.75 +lat_0=43.86194444444445 +lon_0=-92.22777777777777 +x_0=167640.3352806706 +y_0=0 +a=6378381.271 +b=6356996.5851403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIPierceMeters = new ProjectionInfo("+proj=lcc +lat_1=44.52222222222222 +lat_2=44.75 +lat_0=43.86194444444445 +lon_0=-92.22777777777777 +x_0=167640.3352806706 +y_0=0 +a=6378381.271 +b=6356996.5851403 +units=m +no_defs ");
            NAD1983HARNAdjWIPolkFeet = new ProjectionInfo("+proj=tmerc +lat_0=44.66111111111111 +lon_0=-92.63333333333334 +k=1.000000 +x_0=141732.2834645669 +y_0=0 +a=6378413.671 +b=6357028.9851403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIPolkMeters = new ProjectionInfo("+proj=tmerc +lat_0=44.66111111111111 +lon_0=-92.63333333333334 +k=1.000000 +x_0=141732.2834645669 +y_0=0 +a=6378413.671 +b=6357028.9851403 +units=m +no_defs ");
            NAD1983HARNAdjWIPortageFeet = new ProjectionInfo("+proj=lcc +lat_1=44.18333333333333 +lat_2=44.65 +lat_0=43.96666666666667 +lon_0=-89.5 +x_0=56388.11277622555 +y_0=0 +a=6378344.377 +b=6356959.691139228 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIPortageMeters = new ProjectionInfo("+proj=lcc +lat_1=44.18333333333333 +lat_2=44.65 +lat_0=43.96666666666667 +lon_0=-89.5 +x_0=56388.11277622556 +y_0=0 +a=6378344.377 +b=6356959.691139228 +units=m +no_defs ");
            NAD1983HARNAdjWIPriceFeet = new ProjectionInfo("+proj=tmerc +lat_0=44.55555555555555 +lon_0=-90.48888888888889 +k=0.999998 +x_0=227990.8559817119 +y_0=0 +a=6378563.891 +b=6357179.2051403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIPriceMeters = new ProjectionInfo("+proj=tmerc +lat_0=44.55555555555555 +lon_0=-90.48888888888889 +k=0.999998 +x_0=227990.855981712 +y_0=0 +a=6378563.891 +b=6357179.2051403 +units=m +no_defs ");
            NAD1983HARNAdjWIRacineFeet = new ProjectionInfo("+proj=tmerc +lat_0=42.21666666666667 +lon_0=-87.89444444444445 +k=0.999998 +x_0=185928.3718567437 +y_0=0 +a=6378315.7 +b=6356931.014140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIRacineMeters = new ProjectionInfo("+proj=tmerc +lat_0=42.21666666666667 +lon_0=-87.89444444444445 +k=0.999998 +x_0=185928.3718567437 +y_0=0 +a=6378315.7 +b=6356931.014140301 +units=m +no_defs ");
            NAD1983HARNAdjWIRichlandFeet = new ProjectionInfo("+proj=lcc +lat_1=43.14166666666667 +lat_2=43.50277777777778 +lat_0=42.11388888888889 +lon_0=-90.43055555555556 +x_0=202387.6047752095 +y_0=0 +a=6378408.091 +b=6357023.4051403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIRichlandMeters = new ProjectionInfo("+proj=lcc +lat_1=43.14166666666667 +lat_2=43.50277777777778 +lat_0=42.11388888888889 +lon_0=-90.43055555555556 +x_0=202387.6047752096 +y_0=0 +a=6378408.091 +b=6357023.4051403 +units=m +no_defs ");
            NAD1983HARNAdjWIRockFeet = new ProjectionInfo("+proj=tmerc +lat_0=41.94444444444444 +lon_0=-89.07222222222222 +k=0.999996 +x_0=146304.2926085852 +y_0=0 +a=6378377.671 +b=6356992.9851403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIRockMeters = new ProjectionInfo("+proj=tmerc +lat_0=41.94444444444444 +lon_0=-89.07222222222222 +k=0.999996 +x_0=146304.2926085852 +y_0=0 +a=6378377.671 +b=6356992.9851403 +units=m +no_defs ");
            NAD1983HARNAdjWIRuskFeet = new ProjectionInfo("+proj=tmerc +lat_0=43.91944444444444 +lon_0=-91.06666666666666 +k=0.999997 +x_0=250546.1010922022 +y_0=0 +a=6378472.751 +b=6357088.0651403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIRuskMeters = new ProjectionInfo("+proj=tmerc +lat_0=43.91944444444444 +lon_0=-91.06666666666666 +k=0.999997 +x_0=250546.1010922022 +y_0=0 +a=6378472.751 +b=6357088.0651403 +units=m +no_defs ");
            NAD1983HARNAdjWISaukFeet = new ProjectionInfo("+proj=tmerc +lat_0=42.81944444444445 +lon_0=-89.90000000000001 +k=0.999995 +x_0=185623.5712471425 +y_0=0 +a=6378407.281 +b=6357022.595140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWISaukMeters = new ProjectionInfo("+proj=tmerc +lat_0=42.81944444444445 +lon_0=-89.90000000000001 +k=0.999995 +x_0=185623.5712471425 +y_0=0 +a=6378407.281 +b=6357022.595140301 +units=m +no_defs ");
            NAD1983HARNAdjWISawyerFeet = new ProjectionInfo("+proj=lcc +lat_1=45.71944444444445 +lat_2=46.08055555555556 +lat_0=44.81388888888888 +lon_0=-91.11666666666666 +x_0=216713.2334264669 +y_0=0 +a=6378534.451 +b=6357149.765140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWISawyerMeters = new ProjectionInfo("+proj=lcc +lat_1=45.71944444444445 +lat_2=46.08055555555556 +lat_0=44.81388888888888 +lon_0=-91.11666666666666 +x_0=216713.2334264669 +y_0=0 +a=6378534.451 +b=6357149.765140301 +units=m +no_defs ");
            NAD1983HARNAdjWIShawanoFeet = new ProjectionInfo("+proj=tmerc +lat_0=44.03611111111111 +lon_0=-88.60555555555555 +k=0.999990 +x_0=262433.3248666497 +y_0=0 +a=6378406.051 +b=6357021.3651403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIShawanoMeters = new ProjectionInfo("+proj=tmerc +lat_0=44.03611111111111 +lon_0=-88.60555555555555 +k=0.999990 +x_0=262433.3248666498 +y_0=0 +a=6378406.051 +b=6357021.3651403 +units=m +no_defs ");
            NAD1983HARNAdjWISheboyganFeet = new ProjectionInfo("+proj=tmerc +lat_0=43.26666666666667 +lon_0=-87.55 +k=1.000000 +x_0=79857.75971551942 +y_0=0 +a=6378285.86 +b=6356901.174140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWISheboyganMeters = new ProjectionInfo("+proj=tmerc +lat_0=43.26666666666667 +lon_0=-87.55 +k=1.000000 +x_0=79857.75971551944 +y_0=0 +a=6378285.86 +b=6356901.174140301 +units=m +no_defs ");
            NAD1983HARNAdjWIStCroixFeet = new ProjectionInfo("+proj=tmerc +lat_0=44.03611111111111 +lon_0=-92.63333333333334 +k=0.999995 +x_0=165506.731013462 +y_0=0 +a=6378412.511 +b=6357027.8251403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIStCroixMeters = new ProjectionInfo("+proj=tmerc +lat_0=44.03611111111111 +lon_0=-92.63333333333334 +k=0.999995 +x_0=165506.731013462 +y_0=0 +a=6378412.511 +b=6357027.8251403 +units=m +no_defs ");
            NAD1983HARNAdjWITaylorFeet = new ProjectionInfo("+proj=lcc +lat_1=45.05555555555555 +lat_2=45.3 +lat_0=44.20833333333334 +lon_0=-90.48333333333333 +x_0=187147.5742951486 +y_0=0 +a=6378532.921 +b=6357148.2351403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWITaylorMeters = new ProjectionInfo("+proj=lcc +lat_1=45.05555555555555 +lat_2=45.3 +lat_0=44.20833333333334 +lon_0=-90.48333333333333 +x_0=187147.5742951486 +y_0=0 +a=6378532.921 +b=6357148.2351403 +units=m +no_defs ");
            NAD1983HARNAdjWITrempealeauFeet = new ProjectionInfo("+proj=tmerc +lat_0=43.16111111111111 +lon_0=-91.36666666666666 +k=0.999998 +x_0=256946.9138938278 +y_0=0 +a=6378380.091 +b=6356995.4051403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWITrempealeauMeters = new ProjectionInfo("+proj=tmerc +lat_0=43.16111111111111 +lon_0=-91.36666666666666 +k=0.999998 +x_0=256946.9138938278 +y_0=0 +a=6378380.091 +b=6356995.4051403 +units=m +no_defs ");
            NAD1983HARNAdjWIVernonFeet = new ProjectionInfo("+proj=lcc +lat_1=43.46666666666667 +lat_2=43.68333333333333 +lat_0=43.14722222222222 +lon_0=-90.78333333333333 +x_0=222504.44500889 +y_0=0 +a=6378408.941 +b=6357024.2551403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIVernonMeters = new ProjectionInfo("+proj=lcc +lat_1=43.46666666666667 +lat_2=43.68333333333333 +lat_0=43.14722222222222 +lon_0=-90.78333333333333 +x_0=222504.44500889 +y_0=0 +a=6378408.941 +b=6357024.2551403 +units=m +no_defs ");
            NAD1983HARNAdjWIVilasFeet = new ProjectionInfo("+proj=lcc +lat_1=45.93055555555555 +lat_2=46.225 +lat_0=45.625 +lon_0=-89.48888888888889 +x_0=134417.0688341377 +y_0=0 +a=6378624.171 +b=6357239.4851403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIVilasMeters = new ProjectionInfo("+proj=lcc +lat_1=45.93055555555555 +lat_2=46.225 +lat_0=45.625 +lon_0=-89.48888888888889 +x_0=134417.0688341377 +y_0=0 +a=6378624.171 +b=6357239.4851403 +units=m +no_defs ");
            NAD1983HARNAdjWIWalworthFeet = new ProjectionInfo("+proj=lcc +lat_1=42.58888888888889 +lat_2=42.75 +lat_0=41.66944444444444 +lon_0=-88.54166666666667 +x_0=232562.8651257302 +y_0=0 +a=6378377.411 +b=6356992.725140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIWalworthMeters = new ProjectionInfo("+proj=lcc +lat_1=42.58888888888889 +lat_2=42.75 +lat_0=41.66944444444444 +lon_0=-88.54166666666667 +x_0=232562.8651257303 +y_0=0 +a=6378377.411 +b=6356992.725140301 +units=m +no_defs ");
            NAD1983HARNAdjWIWashburnFeet = new ProjectionInfo("+proj=lcc +lat_1=45.77222222222222 +lat_2=46.15 +lat_0=44.26666666666667 +lon_0=-91.78333333333333 +x_0=234086.8681737363 +y_0=0 +a=6378474.591 +b=6357089.9051403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIWashburnMeters = new ProjectionInfo("+proj=lcc +lat_1=45.77222222222222 +lat_2=46.15 +lat_0=44.26666666666667 +lon_0=-91.78333333333333 +x_0=234086.8681737364 +y_0=0 +a=6378474.591 +b=6357089.9051403 +units=m +no_defs ");
            NAD1983HARNAdjWIWashingtonFeet = new ProjectionInfo("+proj=tmerc +lat_0=42.91805555555555 +lon_0=-88.06388888888888 +k=0.999995 +x_0=120091.4401828804 +y_0=0 +a=6378407.141 +b=6357022.4551403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIWashingtonMeters = new ProjectionInfo("+proj=tmerc +lat_0=42.91805555555555 +lon_0=-88.06388888888888 +k=0.999995 +x_0=120091.4401828804 +y_0=0 +a=6378407.141 +b=6357022.4551403 +units=m +no_defs ");
            NAD1983HARNAdjWIWaukeshaFeet = new ProjectionInfo("+proj=tmerc +lat_0=42.56944444444445 +lon_0=-88.22499999999999 +k=0.999997 +x_0=208788.4175768351 +y_0=0 +a=6378376.871 +b=6356992.185140301 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIWaukeshaMeters = new ProjectionInfo("+proj=tmerc +lat_0=42.56944444444445 +lon_0=-88.22499999999999 +k=0.999997 +x_0=208788.4175768352 +y_0=0 +a=6378376.871 +b=6356992.185140301 +units=m +no_defs ");
            NAD1983HARNAdjWIWaupacaFeet = new ProjectionInfo("+proj=tmerc +lat_0=43.42027777777778 +lon_0=-88.81666666666666 +k=0.999996 +x_0=185013.9700279401 +y_0=0 +a=6378375.251 +b=6356990.5651403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIWaupacaMeters = new ProjectionInfo("+proj=tmerc +lat_0=43.42027777777778 +lon_0=-88.81666666666666 +k=0.999996 +x_0=185013.9700279401 +y_0=0 +a=6378375.251 +b=6356990.5651403 +units=m +no_defs ");
            NAD1983HARNAdjWIWausharaFeet = new ProjectionInfo("+proj=lcc +lat_1=43.975 +lat_2=44.25277777777778 +lat_0=43.70833333333334 +lon_0=-89.24166666666667 +x_0=120091.4401828804 +y_0=0 +a=6378405.971 +b=6357021.2851403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIWausharaMeters = new ProjectionInfo("+proj=lcc +lat_1=43.975 +lat_2=44.25277777777778 +lat_0=43.70833333333334 +lon_0=-89.24166666666667 +x_0=120091.4401828804 +y_0=0 +a=6378405.971 +b=6357021.2851403 +units=m +no_defs ");
            NAD1983HARNAdjWIWinnebagoFeet = new ProjectionInfo("+proj=tmerc +lat_0=42.71944444444445 +lon_0=-88.5 +k=0.999996 +x_0=244754.889509779 +y_0=0 +a=6378345.09 +b=6356960.4041403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIWinnebagoMeters = new ProjectionInfo("+proj=tmerc +lat_0=42.71944444444445 +lon_0=-88.5 +k=0.999996 +x_0=244754.8895097791 +y_0=0 +a=6378345.09 +b=6356960.4041403 +units=m +no_defs ");
            NAD1983HARNAdjWIWoodFeet = new ProjectionInfo("+proj=lcc +lat_1=44.18055555555555 +lat_2=44.54444444444444 +lat_0=43.15138888888889 +lon_0=-90 +x_0=208483.6169672339 +y_0=0 +a=6378437.651 +b=6357052.9651403 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjWIWoodMeters = new ProjectionInfo("+proj=lcc +lat_1=44.18055555555555 +lat_2=44.54444444444444 +lat_0=43.15138888888889 +lon_0=-90 +x_0=208483.616967234 +y_0=0 +a=6378437.651 +b=6357052.9651403 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591