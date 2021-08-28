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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:32:25 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// Not sure why we have all these county based Minnesota and Wisconsin projections
    /// </summary>
    public class Minnesota : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1983HARNAdjMNAitkinFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNAitkinMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNAnokaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNAnokaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeckerFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeckerMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeltramiNorthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeltramiNorthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeltramiSouthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeltramiSouthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBentonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBentonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBigStoneFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBigStoneMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBlueEarthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBlueEarthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNBrownFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNBrownMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCarltonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCarltonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCarverFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCarverMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCassNorthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCassNorthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCassSouthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCassSouthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNChippewaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNChippewaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNChisagoFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNChisagoMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNClayFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNClayMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNClearwaterFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNClearwaterMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCookNorthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCookNorthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCookSouthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCookSouthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCottonwoodFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCottonwoodMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNCrowWingFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNCrowWingMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNDakotaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNDakotaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNDodgeFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNDodgeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNDouglasFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNDouglasMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNFaribaultFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNFaribaultMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNFillmoreFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNFillmoreMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNFreebornFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNFreebornMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNGoodhueFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNGoodhueMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNGrantFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNGrantMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNHennepinFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNHennepinMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNHoustonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNHoustonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNHubbardFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNHubbardMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNIsantiFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNIsantiMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNItascaNorthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNItascaNorthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNItascaSouthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNItascaSouthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNJacksonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNJacksonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNKanabecFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNKanabecMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNKandiyohiFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNKandiyohiMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNKittsonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNKittsonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNKoochichingFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNKoochichingMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLacQuiParleFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLacQuiParleMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeoftheWoodsNorthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeoftheWoodsNorthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeoftheWoodsSouthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeoftheWoodsSouthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLeSueurFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLeSueurMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLincolnFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLincolnMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNLyonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNLyonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMahnomenFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMahnomenMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMarshallFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMarshallMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMartinFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMartinMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMcLeodFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMcLeodMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMeekerFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMeekerMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMilleLacsFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMilleLacsMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMorrisonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMorrisonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMowerFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMowerMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNMurrayFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNMurrayMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNNicolletFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNNicolletMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNNoblesFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNNoblesMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNNormanFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNNormanMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNOlmstedFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNOlmstedMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNOttertailFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNOttertailMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNPenningtonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNPenningtonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNPineFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNPineMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNPipestoneFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNPipestoneMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNPolkFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNPolkMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNPopeFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNPopeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRamseyFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRamseyMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRedLakeFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRedLakeMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRedwoodFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRedwoodMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRenvilleFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRenvilleMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRiceFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRiceMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRockFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRockMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNRoseauFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNRoseauMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNScottFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNScottMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNSherburneFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNSherburneMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNSibleyFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNSibleyMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNStearnsFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNStearnsMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNSteeleFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNSteeleMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNStevensFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNStevensMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisCentralFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisCentralMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisNorthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisNorthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisSouthFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisSouthMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNSwiftFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNSwiftMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNToddFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNToddMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNTraverseFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNTraverseMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWabashaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWabashaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWadenaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWadenaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWasecaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWasecaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWashingtonFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWashingtonMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWatonwanFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWatonwanMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWilkinFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWilkinMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWinonaFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWinonaMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNWrightFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNWrightMeters;
        public readonly ProjectionInfo NAD1983HARNAdjMNYellowMedicineFeet;
        public readonly ProjectionInfo NAD1983HARNAdjMNYellowMedicineMeters;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Minnesota
        /// </summary>
        public Minnesota()
        {
            NAD1983HARNAdjMNAitkinFeet = new ProjectionInfo("+proj=tmerc +lat_0=46.15 +lon_0=-93.41666666666667 +k=1.000059 +x_0=152409.319685395 +y_0=30481.86393707899 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNAitkinMeters = new ProjectionInfo("+proj=tmerc +lat_0=46.15 +lon_0=-93.41666666666667 +k=1.000059 +x_0=152409.319685395 +y_0=30481.86393707899 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNAdjMNAnokaFeet = new ProjectionInfo("+proj=lcc +lat_1=45.06666666666667 +lat_2=45.36666666666667 +lat_0=45.03333333333333 +lon_0=-93.26666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378418.941 +b=6357033.309845551 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNAnokaMeters = new ProjectionInfo("+proj=lcc +lat_1=45.06666666666667 +lat_2=45.36666666666667 +lat_0=45.03333333333333 +lon_0=-93.26666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378418.941 +b=6357033.309845551 +units=m +no_defs ");
            NAD1983HARNAdjMNBeckerFeet = new ProjectionInfo("+proj=lcc +lat_1=46.78333333333333 +lat_2=47.08333333333334 +lat_0=46.71666666666667 +lon_0=-95.68333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378586.581 +b=6357200.387780368 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNBeckerMeters = new ProjectionInfo("+proj=lcc +lat_1=46.78333333333333 +lat_2=47.08333333333334 +lat_0=46.71666666666667 +lon_0=-95.68333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378586.581 +b=6357200.387780368 +units=m +no_defs ");
            NAD1983HARNAdjMNBeltramiNorthFeet = new ProjectionInfo("+proj=lcc +lat_1=48.11666666666667 +lat_2=48.46666666666667 +lat_0=48.01666666666667 +lon_0=-95.01666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378505.809 +b=6357119.886593593 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNBeltramiNorthMeters = new ProjectionInfo("+proj=lcc +lat_1=48.11666666666667 +lat_2=48.46666666666667 +lat_0=48.01666666666667 +lon_0=-95.01666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378505.809 +b=6357119.886593593 +units=m +no_defs ");
            NAD1983HARNAdjMNBeltramiSouthFeet = new ProjectionInfo("+proj=lcc +lat_1=47.5 +lat_2=47.91666666666666 +lat_0=47.4 +lon_0=-94.84999999999999 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378544.823 +b=6357158.769787037 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNBeltramiSouthMeters = new ProjectionInfo("+proj=lcc +lat_1=47.5 +lat_2=47.91666666666666 +lat_0=47.4 +lon_0=-94.84999999999999 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378544.823 +b=6357158.769787037 +units=m +no_defs ");
            NAD1983HARNAdjMNBentonFeet = new ProjectionInfo("+proj=lcc +lat_1=45.58333333333334 +lat_2=45.78333333333333 +lat_0=45.55 +lon_0=-94.05 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378490.569 +b=6357104.697690427 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNBentonMeters = new ProjectionInfo("+proj=lcc +lat_1=45.58333333333334 +lat_2=45.78333333333333 +lat_0=45.55 +lon_0=-94.05 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378490.569 +b=6357104.697690427 +units=m +no_defs ");
            NAD1983HARNAdjMNBigStoneFeet = new ProjectionInfo("+proj=lcc +lat_1=45.21666666666667 +lat_2=45.53333333333333 +lat_0=45.15 +lon_0=-96.05 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378470.757 +b=6357084.952116313 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNBigStoneMeters = new ProjectionInfo("+proj=lcc +lat_1=45.21666666666667 +lat_2=45.53333333333333 +lat_0=45.15 +lon_0=-96.05 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378470.757 +b=6357084.952116313 +units=m +no_defs ");
            NAD1983HARNAdjMNBlueEarthFeet = new ProjectionInfo("+proj=lcc +lat_1=43.93333333333333 +lat_2=44.36666666666667 +lat_0=43.83333333333334 +lon_0=-94.26666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378403.701 +b=6357018.120942386 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNBlueEarthMeters = new ProjectionInfo("+proj=lcc +lat_1=43.93333333333333 +lat_2=44.36666666666667 +lat_0=43.83333333333334 +lon_0=-94.26666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378403.701 +b=6357018.120942386 +units=m +no_defs ");
            NAD1983HARNAdjMNBrownFeet = new ProjectionInfo("+proj=lcc +lat_1=44.16666666666666 +lat_2=44.46666666666667 +lat_0=44.1 +lon_0=-94.73333333333333 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378434.181 +b=6357048.498748716 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNBrownMeters = new ProjectionInfo("+proj=lcc +lat_1=44.16666666666666 +lat_2=44.46666666666667 +lat_0=44.1 +lon_0=-94.73333333333333 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378434.181 +b=6357048.498748716 +units=m +no_defs ");
            NAD1983HARNAdjMNCarltonFeet = new ProjectionInfo("+proj=lcc +lat_1=46.46666666666667 +lat_2=46.73333333333333 +lat_0=46.41666666666666 +lon_0=-92.68333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378454.907 +b=6357069.155258362 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNCarltonMeters = new ProjectionInfo("+proj=lcc +lat_1=46.46666666666667 +lat_2=46.73333333333333 +lat_0=46.41666666666666 +lon_0=-92.68333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378454.907 +b=6357069.155258362 +units=m +no_defs ");
            NAD1983HARNAdjMNCarverFeet = new ProjectionInfo("+proj=lcc +lat_1=44.68333333333333 +lat_2=44.9 +lat_0=44.63333333333333 +lon_0=-93.76666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378400.653 +b=6357015.083161753 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNCarverMeters = new ProjectionInfo("+proj=lcc +lat_1=44.68333333333333 +lat_2=44.9 +lat_0=44.63333333333333 +lon_0=-93.76666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378400.653 +b=6357015.083161753 +units=m +no_defs ");
            NAD1983HARNAdjMNCassNorthFeet = new ProjectionInfo("+proj=lcc +lat_1=46.91666666666666 +lat_2=47.31666666666667 +lat_0=46.8 +lon_0=-94.21666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378567.378 +b=6357181.249164391 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNCassNorthMeters = new ProjectionInfo("+proj=lcc +lat_1=46.91666666666666 +lat_2=47.31666666666667 +lat_0=46.8 +lon_0=-94.21666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378567.378 +b=6357181.249164391 +units=m +no_defs ");
            NAD1983HARNAdjMNCassSouthFeet = new ProjectionInfo("+proj=lcc +lat_1=46.26666666666667 +lat_2=46.73333333333333 +lat_0=46.15 +lon_0=-94.46666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378546.957 +b=6357160.89663214 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNCassSouthMeters = new ProjectionInfo("+proj=lcc +lat_1=46.26666666666667 +lat_2=46.73333333333333 +lat_0=46.15 +lon_0=-94.46666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378546.957 +b=6357160.89663214 +units=m +no_defs ");
            NAD1983HARNAdjMNChippewaFeet = new ProjectionInfo("+proj=lcc +lat_1=44.83333333333334 +lat_2=45.2 +lat_0=44.75 +lon_0=-95.84999999999999 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378476.853 +b=6357091.027677579 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNChippewaMeters = new ProjectionInfo("+proj=lcc +lat_1=44.83333333333334 +lat_2=45.2 +lat_0=44.75 +lon_0=-95.84999999999999 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378476.853 +b=6357091.027677579 +units=m +no_defs ");
            NAD1983HARNAdjMNChisagoFeet = new ProjectionInfo("+proj=lcc +lat_1=45.33333333333334 +lat_2=45.66666666666666 +lat_0=45.28333333333333 +lon_0=-93.08333333333333 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378411.321000001 +b=6357025.715393969 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNChisagoMeters = new ProjectionInfo("+proj=lcc +lat_1=45.33333333333334 +lat_2=45.66666666666666 +lat_0=45.28333333333333 +lon_0=-93.08333333333333 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378411.321000001 +b=6357025.715393969 +units=m +no_defs ");
            NAD1983HARNAdjMNClayFeet = new ProjectionInfo("+proj=tmerc +lat_0=46.61666666666667 +lon_0=-96.7 +k=1.000045 +x_0=152407.2112565913 +y_0=30481.44225131826 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNClayMeters = new ProjectionInfo("+proj=tmerc +lat_0=46.61666666666667 +lon_0=-96.7 +k=1.000045 +x_0=152407.2112565913 +y_0=30481.44225131827 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNAdjMNClearwaterFeet = new ProjectionInfo("+proj=tmerc +lat_0=47.15 +lon_0=-95.36666666666666 +k=1.000073 +x_0=152411.3546854458 +y_0=30482.27093708915 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNClearwaterMeters = new ProjectionInfo("+proj=tmerc +lat_0=47.15 +lon_0=-95.36666666666666 +k=1.000073 +x_0=152411.3546854458 +y_0=30482.27093708916 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNAdjMNCookNorthFeet = new ProjectionInfo("+proj=lcc +lat_1=47.93333333333333 +lat_2=48.16666666666666 +lat_0=47.88333333333333 +lon_0=-90.25 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378647.541 +b=6357261.14339303 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNCookNorthMeters = new ProjectionInfo("+proj=lcc +lat_1=47.93333333333333 +lat_2=48.16666666666666 +lat_0=47.88333333333333 +lon_0=-90.25 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378647.541 +b=6357261.14339303 +units=m +no_defs ");
            NAD1983HARNAdjMNCookSouthFeet = new ProjectionInfo("+proj=lcc +lat_1=47.55 +lat_2=47.81666666666667 +lat_0=47.43333333333333 +lon_0=-90.25 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378647.541 +b=6357261.14339303 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNCookSouthMeters = new ProjectionInfo("+proj=lcc +lat_1=47.55 +lat_2=47.81666666666667 +lat_0=47.43333333333333 +lon_0=-90.25 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378647.541 +b=6357261.14339303 +units=m +no_defs ");
            NAD1983HARNAdjMNCottonwoodFeet = new ProjectionInfo("+proj=lcc +lat_1=43.9 +lat_2=44.16666666666666 +lat_0=43.83333333333334 +lon_0=-94.91666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378514.953 +b=6357128.999935492 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNCottonwoodMeters = new ProjectionInfo("+proj=lcc +lat_1=43.9 +lat_2=44.16666666666666 +lat_0=43.83333333333334 +lon_0=-94.91666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378514.953 +b=6357128.999935492 +units=m +no_defs ");
            NAD1983HARNAdjMNCrowWingFeet = new ProjectionInfo("+proj=lcc +lat_1=46.26666666666667 +lat_2=46.73333333333333 +lat_0=46.15 +lon_0=-94.46666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378546.957 +b=6357160.89663214 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNCrowWingMeters = new ProjectionInfo("+proj=lcc +lat_1=46.26666666666667 +lat_2=46.73333333333333 +lat_0=46.15 +lon_0=-94.46666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378546.957 +b=6357160.89663214 +units=m +no_defs ");
            NAD1983HARNAdjMNDakotaFeet = new ProjectionInfo("+proj=lcc +lat_1=44.51666666666667 +lat_2=44.91666666666666 +lat_0=44.46666666666667 +lon_0=-93.31666666666666 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378421.989 +b=6357036.347626184 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNDakotaMeters = new ProjectionInfo("+proj=lcc +lat_1=44.51666666666667 +lat_2=44.91666666666666 +lat_0=44.46666666666667 +lon_0=-93.31666666666666 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378421.989 +b=6357036.347626184 +units=m +no_defs ");
            NAD1983HARNAdjMNDodgeFeet = new ProjectionInfo("+proj=lcc +lat_1=43.88333333333333 +lat_2=44.13333333333333 +lat_0=43.83333333333334 +lon_0=-92.91666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378481.425 +b=6357095.584348529 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNDodgeMeters = new ProjectionInfo("+proj=lcc +lat_1=43.88333333333333 +lat_2=44.13333333333333 +lat_0=43.83333333333334 +lon_0=-92.91666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378481.425 +b=6357095.584348529 +units=m +no_defs ");
            NAD1983HARNAdjMNDouglasFeet = new ProjectionInfo("+proj=lcc +lat_1=45.8 +lat_2=46.05 +lat_0=45.75 +lon_0=-96.05 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378518.001 +b=6357132.037716125 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNDouglasMeters = new ProjectionInfo("+proj=lcc +lat_1=45.8 +lat_2=46.05 +lat_0=45.75 +lon_0=-96.05 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378518.001 +b=6357132.037716125 +units=m +no_defs ");
            NAD1983HARNAdjMNFaribaultFeet = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-93.95 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378521.049 +b=6357135.075496757 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNFaribaultMeters = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-93.95 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378521.049 +b=6357135.075496757 +units=m +no_defs ");
            NAD1983HARNAdjMNFillmoreFeet = new ProjectionInfo("+proj=lcc +lat_1=43.55 +lat_2=43.8 +lat_0=43.5 +lon_0=-92.08333333333333 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378464.661 +b=6357078.876555047 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNFillmoreMeters = new ProjectionInfo("+proj=lcc +lat_1=43.55 +lat_2=43.8 +lat_0=43.5 +lon_0=-92.08333333333333 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378464.661 +b=6357078.876555047 +units=m +no_defs ");
            NAD1983HARNAdjMNFreebornFeet = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-93.95 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378521.049 +b=6357135.075496757 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNFreebornMeters = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-93.95 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378521.049 +b=6357135.075496757 +units=m +no_defs ");
            NAD1983HARNAdjMNGoodhueFeet = new ProjectionInfo("+proj=lcc +lat_1=44.3 +lat_2=44.66666666666666 +lat_0=44.18333333333333 +lon_0=-93.13333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378434.181 +b=6357048.498748716 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNGoodhueMeters = new ProjectionInfo("+proj=lcc +lat_1=44.3 +lat_2=44.66666666666666 +lat_0=44.18333333333333 +lon_0=-93.13333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378434.181 +b=6357048.498748716 +units=m +no_defs ");
            NAD1983HARNAdjMNGrantFeet = new ProjectionInfo("+proj=lcc +lat_1=45.8 +lat_2=46.05 +lat_0=45.75 +lon_0=-96.05 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378518.001 +b=6357132.037716125 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNGrantMeters = new ProjectionInfo("+proj=lcc +lat_1=45.8 +lat_2=46.05 +lat_0=45.75 +lon_0=-96.05 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378518.001 +b=6357132.037716125 +units=m +no_defs ");
            NAD1983HARNAdjMNHennepinFeet = new ProjectionInfo("+proj=lcc +lat_1=44.88333333333333 +lat_2=45.13333333333333 +lat_0=44.78333333333333 +lon_0=-93.38333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378418.941 +b=6357033.309845551 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNHennepinMeters = new ProjectionInfo("+proj=lcc +lat_1=44.88333333333333 +lat_2=45.13333333333333 +lat_0=44.78333333333333 +lon_0=-93.38333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378418.941 +b=6357033.309845551 +units=m +no_defs ");
            NAD1983HARNAdjMNHoustonFeet = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-91.46666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378436.619 +b=6357050.928574564 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNHoustonMeters = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-91.46666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378436.619 +b=6357050.928574564 +units=m +no_defs ");
            NAD1983HARNAdjMNHubbardFeet = new ProjectionInfo("+proj=tmerc +lat_0=46.8 +lon_0=-94.91666666666667 +k=1.000072 +x_0=152411.2096003556 +y_0=30482.24192007112 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNHubbardMeters = new ProjectionInfo("+proj=tmerc +lat_0=46.8 +lon_0=-94.91666666666667 +k=1.000072 +x_0=152411.2096003556 +y_0=30482.24192007113 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNAdjMNIsantiFeet = new ProjectionInfo("+proj=lcc +lat_1=45.33333333333334 +lat_2=45.66666666666666 +lat_0=45.28333333333333 +lon_0=-93.08333333333333 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378411.321000001 +b=6357025.715393969 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNIsantiMeters = new ProjectionInfo("+proj=lcc +lat_1=45.33333333333334 +lat_2=45.66666666666666 +lat_0=45.28333333333333 +lon_0=-93.08333333333333 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378411.321000001 +b=6357025.715393969 +units=m +no_defs ");
            NAD1983HARNAdjMNItascaNorthFeet = new ProjectionInfo("+proj=lcc +lat_1=47.56666666666667 +lat_2=47.81666666666667 +lat_0=47.5 +lon_0=-93.73333333333333 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378574.389 +b=6357188.236657837 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNItascaNorthMeters = new ProjectionInfo("+proj=lcc +lat_1=47.56666666666667 +lat_2=47.81666666666667 +lat_0=47.5 +lon_0=-93.73333333333333 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378574.389 +b=6357188.236657837 +units=m +no_defs ");
            NAD1983HARNAdjMNItascaSouthFeet = new ProjectionInfo("+proj=lcc +lat_1=47.08333333333334 +lat_2=47.41666666666666 +lat_0=47.01666666666667 +lon_0=-93.73333333333333 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378574.389 +b=6357188.236657837 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNItascaSouthMeters = new ProjectionInfo("+proj=lcc +lat_1=47.08333333333334 +lat_2=47.41666666666666 +lat_0=47.01666666666667 +lon_0=-93.73333333333333 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378574.389 +b=6357188.236657837 +units=m +no_defs ");
            NAD1983HARNAdjMNJacksonFeet = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-93.95 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378521.049 +b=6357135.075496757 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNJacksonMeters = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-93.95 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378521.049 +b=6357135.075496757 +units=m +no_defs ");
            NAD1983HARNAdjMNKanabecFeet = new ProjectionInfo("+proj=lcc +lat_1=45.81666666666667 +lat_2=46.33333333333334 +lat_0=45.71666666666667 +lon_0=-92.90000000000001 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378472.281 +b=6357086.47100663 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNKanabecMeters = new ProjectionInfo("+proj=lcc +lat_1=45.81666666666667 +lat_2=46.33333333333334 +lat_0=45.71666666666667 +lon_0=-92.90000000000001 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378472.281 +b=6357086.47100663 +units=m +no_defs ");
            NAD1983HARNAdjMNKandiyohiFeet = new ProjectionInfo("+proj=lcc +lat_1=44.96666666666667 +lat_2=45.33333333333334 +lat_0=44.88333333333333 +lon_0=-94.75 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378498.189 +b=6357112.29214201 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNKandiyohiMeters = new ProjectionInfo("+proj=lcc +lat_1=44.96666666666667 +lat_2=45.33333333333334 +lat_0=44.88333333333333 +lon_0=-94.75 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378498.189 +b=6357112.29214201 +units=m +no_defs ");
            NAD1983HARNAdjMNKittsonFeet = new ProjectionInfo("+proj=lcc +lat_1=48.6 +lat_2=48.93333333333333 +lat_0=48.53333333333333 +lon_0=-96.15000000000001 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378449.421 +b=6357063.687651882 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNKittsonMeters = new ProjectionInfo("+proj=lcc +lat_1=48.6 +lat_2=48.93333333333333 +lat_0=48.53333333333333 +lon_0=-96.15000000000001 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378449.421 +b=6357063.687651882 +units=m +no_defs ");
            NAD1983HARNAdjMNKoochichingFeet = new ProjectionInfo("+proj=lcc +lat_1=48 +lat_2=48.61666666666667 +lat_0=47.83333333333334 +lon_0=-93.75 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378525.621 +b=6357139.632167708 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNKoochichingMeters = new ProjectionInfo("+proj=lcc +lat_1=48 +lat_2=48.61666666666667 +lat_0=47.83333333333334 +lon_0=-93.75 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378525.621 +b=6357139.632167708 +units=m +no_defs ");
            NAD1983HARNAdjMNLacQuiParleFeet = new ProjectionInfo("+proj=lcc +lat_1=44.83333333333334 +lat_2=45.2 +lat_0=44.75 +lon_0=-95.84999999999999 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378476.853 +b=6357091.027677579 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNLacQuiParleMeters = new ProjectionInfo("+proj=lcc +lat_1=44.83333333333334 +lat_2=45.2 +lat_0=44.75 +lon_0=-95.84999999999999 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378476.853 +b=6357091.027677579 +units=m +no_defs ");
            NAD1983HARNAdjMNLakeFeet = new ProjectionInfo("+proj=tmerc +lat_0=47.06666666666667 +lon_0=-91.40000000000001 +k=1.000076 +x_0=152411.8635439675 +y_0=30482.3727087935 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNLakeMeters = new ProjectionInfo("+proj=tmerc +lat_0=47.06666666666667 +lon_0=-91.40000000000001 +k=1.000076 +x_0=152411.8635439675 +y_0=30482.3727087935 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNAdjMNLakeoftheWoodsNorthFeet = new ProjectionInfo("+proj=lcc +lat_1=49.18333333333333 +lat_2=49.33333333333334 +lat_0=49.15 +lon_0=-94.98333333333333 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378466.185 +b=6357080.395445363 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNLakeoftheWoodsNorthMeters = new ProjectionInfo("+proj=lcc +lat_1=49.18333333333333 +lat_2=49.33333333333334 +lat_0=49.15 +lon_0=-94.98333333333333 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378466.185 +b=6357080.395445363 +units=m +no_defs ");
            NAD1983HARNAdjMNLakeoftheWoodsSouthFeet = new ProjectionInfo("+proj=lcc +lat_1=48.45 +lat_2=48.88333333333333 +lat_0=48.35 +lon_0=-94.88333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378496.665 +b=6357110.773251694 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNLakeoftheWoodsSouthMeters = new ProjectionInfo("+proj=lcc +lat_1=48.45 +lat_2=48.88333333333333 +lat_0=48.35 +lon_0=-94.88333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378496.665 +b=6357110.773251694 +units=m +no_defs ");
            NAD1983HARNAdjMNLeSueurFeet = new ProjectionInfo("+proj=lcc +lat_1=44.3 +lat_2=44.66666666666666 +lat_0=44.18333333333333 +lon_0=-93.13333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378434.181 +b=6357048.498748716 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNLeSueurMeters = new ProjectionInfo("+proj=lcc +lat_1=44.3 +lat_2=44.66666666666666 +lat_0=44.18333333333333 +lon_0=-93.13333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378434.181 +b=6357048.498748716 +units=m +no_defs ");
            NAD1983HARNAdjMNLincolnFeet = new ProjectionInfo("+proj=lcc +lat_1=44.28333333333333 +lat_2=44.61666666666667 +lat_0=44.18333333333333 +lon_0=-96.26666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378643.579 +b=6357257.194676865 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNLincolnMeters = new ProjectionInfo("+proj=lcc +lat_1=44.28333333333333 +lat_2=44.61666666666667 +lat_0=44.18333333333333 +lon_0=-96.26666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378643.579 +b=6357257.194676865 +units=m +no_defs ");
            NAD1983HARNAdjMNLyonFeet = new ProjectionInfo("+proj=lcc +lat_1=44.25 +lat_2=44.58333333333334 +lat_0=44.18333333333333 +lon_0=-95.84999999999999 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378559.758 +b=6357173.65471281 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNLyonMeters = new ProjectionInfo("+proj=lcc +lat_1=44.25 +lat_2=44.58333333333334 +lat_0=44.18333333333333 +lon_0=-95.84999999999999 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378559.758 +b=6357173.65471281 +units=m +no_defs ");
            NAD1983HARNAdjMNMahnomenFeet = new ProjectionInfo("+proj=lcc +lat_1=47.2 +lat_2=47.45 +lat_0=47.15 +lon_0=-95.81666666666666 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378586.581 +b=6357200.387780368 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNMahnomenMeters = new ProjectionInfo("+proj=lcc +lat_1=47.2 +lat_2=47.45 +lat_0=47.15 +lon_0=-95.81666666666666 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378586.581 +b=6357200.387780368 +units=m +no_defs ");
            NAD1983HARNAdjMNMarshallFeet = new ProjectionInfo("+proj=lcc +lat_1=48.23333333333333 +lat_2=48.48333333333333 +lat_0=48.16666666666666 +lon_0=-96.38333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378441.801 +b=6357056.093200299 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNMarshallMeters = new ProjectionInfo("+proj=lcc +lat_1=48.23333333333333 +lat_2=48.48333333333333 +lat_0=48.16666666666666 +lon_0=-96.38333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378441.801 +b=6357056.093200299 +units=m +no_defs ");
            NAD1983HARNAdjMNMartinFeet = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-93.95 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378521.049 +b=6357135.075496757 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNMartinMeters = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-93.95 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378521.049 +b=6357135.075496757 +units=m +no_defs ");
            NAD1983HARNAdjMNMcLeodFeet = new ProjectionInfo("+proj=lcc +lat_1=44.53333333333333 +lat_2=44.91666666666666 +lat_0=44.45 +lon_0=-94.63333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378414.369 +b=6357028.753174601 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNMcLeodMeters = new ProjectionInfo("+proj=lcc +lat_1=44.53333333333333 +lat_2=44.91666666666666 +lat_0=44.45 +lon_0=-94.63333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378414.369 +b=6357028.753174601 +units=m +no_defs ");
            NAD1983HARNAdjMNMeekerFeet = new ProjectionInfo("+proj=lcc +lat_1=44.96666666666667 +lat_2=45.33333333333334 +lat_0=44.88333333333333 +lon_0=-94.75 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378498.189 +b=6357112.29214201 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNMeekerMeters = new ProjectionInfo("+proj=lcc +lat_1=44.96666666666667 +lat_2=45.33333333333334 +lat_0=44.88333333333333 +lon_0=-94.75 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378498.189 +b=6357112.29214201 +units=m +no_defs ");
            NAD1983HARNAdjMNMilleLacsFeet = new ProjectionInfo("+proj=tmerc +lat_0=45.55 +lon_0=-93.61666666666666 +k=1.000054 +x_0=152408.5566885446 +y_0=30481.71133770892 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNMilleLacsMeters = new ProjectionInfo("+proj=tmerc +lat_0=45.55 +lon_0=-93.61666666666666 +k=1.000054 +x_0=152408.5566885446 +y_0=30481.71133770892 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNAdjMNMorrisonFeet = new ProjectionInfo("+proj=lcc +lat_1=45.85 +lat_2=46.26666666666667 +lat_0=45.76666666666667 +lon_0=-94.2 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378502.761 +b=6357116.84881296 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNMorrisonMeters = new ProjectionInfo("+proj=lcc +lat_1=45.85 +lat_2=46.26666666666667 +lat_0=45.76666666666667 +lon_0=-94.2 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378502.761 +b=6357116.84881296 +units=m +no_defs ");
            NAD1983HARNAdjMNMowerFeet = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-93.95 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378521.049 +b=6357135.075496757 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNMowerMeters = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-93.95 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378521.049 +b=6357135.075496757 +units=m +no_defs ");
            NAD1983HARNAdjMNMurrayFeet = new ProjectionInfo("+proj=lcc +lat_1=43.91666666666666 +lat_2=44.16666666666666 +lat_0=43.83333333333334 +lon_0=-95.76666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378617.061 +b=6357230.765586698 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNMurrayMeters = new ProjectionInfo("+proj=lcc +lat_1=43.91666666666666 +lat_2=44.16666666666666 +lat_0=43.83333333333334 +lon_0=-95.76666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378617.061 +b=6357230.765586698 +units=m +no_defs ");
            NAD1983HARNAdjMNNicolletFeet = new ProjectionInfo("+proj=lcc +lat_1=43.93333333333333 +lat_2=44.36666666666667 +lat_0=43.83333333333334 +lon_0=-94.26666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378403.701 +b=6357018.120942386 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNNicolletMeters = new ProjectionInfo("+proj=lcc +lat_1=43.93333333333333 +lat_2=44.36666666666667 +lat_0=43.83333333333334 +lon_0=-94.26666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378403.701 +b=6357018.120942386 +units=m +no_defs ");
            NAD1983HARNAdjMNNoblesFeet = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-95.95 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378624.681 +b=6357238.360038281 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNNoblesMeters = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-95.95 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378624.681 +b=6357238.360038281 +units=m +no_defs ");
            NAD1983HARNAdjMNNormanFeet = new ProjectionInfo("+proj=lcc +lat_1=47.2 +lat_2=47.45 +lat_0=47.15 +lon_0=-96.45 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378468.623 +b=6357082.825271211 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNNormanMeters = new ProjectionInfo("+proj=lcc +lat_1=47.2 +lat_2=47.45 +lat_0=47.15 +lon_0=-96.45 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378468.623 +b=6357082.825271211 +units=m +no_defs ");
            NAD1983HARNAdjMNOlmstedFeet = new ProjectionInfo("+proj=lcc +lat_1=43.88333333333333 +lat_2=44.13333333333333 +lat_0=43.83333333333334 +lon_0=-92.91666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378481.425 +b=6357095.584348529 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNOlmstedMeters = new ProjectionInfo("+proj=lcc +lat_1=43.88333333333333 +lat_2=44.13333333333333 +lat_0=43.83333333333334 +lon_0=-92.91666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378481.425 +b=6357095.584348529 +units=m +no_defs ");
            NAD1983HARNAdjMNOttertailFeet = new ProjectionInfo("+proj=lcc +lat_1=46.18333333333333 +lat_2=46.65 +lat_0=46.1 +lon_0=-95.71666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378525.621 +b=6357139.632167708 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNOttertailMeters = new ProjectionInfo("+proj=lcc +lat_1=46.18333333333333 +lat_2=46.65 +lat_0=46.1 +lon_0=-95.71666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378525.621 +b=6357139.632167708 +units=m +no_defs ");
            NAD1983HARNAdjMNPenningtonFeet = new ProjectionInfo("+proj=lcc +lat_1=47.6 +lat_2=48.08333333333334 +lat_0=47.48333333333333 +lon_0=-96.36666666666666 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378445.763 +b=6357060.041916464 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNPenningtonMeters = new ProjectionInfo("+proj=lcc +lat_1=47.6 +lat_2=48.08333333333334 +lat_0=47.48333333333333 +lon_0=-96.36666666666666 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378445.763 +b=6357060.041916464 +units=m +no_defs ");
            NAD1983HARNAdjMNPineFeet = new ProjectionInfo("+proj=lcc +lat_1=45.81666666666667 +lat_2=46.33333333333334 +lat_0=45.71666666666667 +lon_0=-92.90000000000001 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378472.281 +b=6357086.47100663 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNPineMeters = new ProjectionInfo("+proj=lcc +lat_1=45.81666666666667 +lat_2=46.33333333333334 +lat_0=45.71666666666667 +lon_0=-92.90000000000001 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378472.281 +b=6357086.47100663 +units=m +no_defs ");
            NAD1983HARNAdjMNPipestoneFeet = new ProjectionInfo("+proj=lcc +lat_1=43.88333333333333 +lat_2=44.15 +lat_0=43.83333333333334 +lon_0=-96.25 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378670.401 +b=6357283.926747777 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNPipestoneMeters = new ProjectionInfo("+proj=lcc +lat_1=43.88333333333333 +lat_2=44.15 +lat_0=43.83333333333334 +lon_0=-96.25 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378670.401 +b=6357283.926747777 +units=m +no_defs ");
            NAD1983HARNAdjMNPolkFeet = new ProjectionInfo("+proj=lcc +lat_1=47.6 +lat_2=48.08333333333334 +lat_0=47.48333333333333 +lon_0=-96.36666666666666 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378445.763 +b=6357060.041916464 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNPolkMeters = new ProjectionInfo("+proj=lcc +lat_1=47.6 +lat_2=48.08333333333334 +lat_0=47.48333333333333 +lon_0=-96.36666666666666 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378445.763 +b=6357060.041916464 +units=m +no_defs ");
            NAD1983HARNAdjMNPopeFeet = new ProjectionInfo("+proj=lcc +lat_1=45.35 +lat_2=45.7 +lat_0=45.26666666666667 +lon_0=-95.15000000000001 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378502.761 +b=6357116.84881296 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNPopeMeters = new ProjectionInfo("+proj=lcc +lat_1=45.35 +lat_2=45.7 +lat_0=45.26666666666667 +lon_0=-95.15000000000001 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378502.761 +b=6357116.84881296 +units=m +no_defs ");
            NAD1983HARNAdjMNRamseyFeet = new ProjectionInfo("+proj=lcc +lat_1=44.88333333333333 +lat_2=45.13333333333333 +lat_0=44.78333333333333 +lon_0=-93.38333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378418.941 +b=6357033.309845551 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNRamseyMeters = new ProjectionInfo("+proj=lcc +lat_1=44.88333333333333 +lat_2=45.13333333333333 +lat_0=44.78333333333333 +lon_0=-93.38333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378418.941 +b=6357033.309845551 +units=m +no_defs ");
            NAD1983HARNAdjMNRedLakeFeet = new ProjectionInfo("+proj=lcc +lat_1=47.6 +lat_2=48.08333333333334 +lat_0=47.48333333333333 +lon_0=-96.36666666666666 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378445.763 +b=6357060.041916464 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNRedLakeMeters = new ProjectionInfo("+proj=lcc +lat_1=47.6 +lat_2=48.08333333333334 +lat_0=47.48333333333333 +lon_0=-96.36666666666666 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378445.763 +b=6357060.041916464 +units=m +no_defs ");
            NAD1983HARNAdjMNRedwoodFeet = new ProjectionInfo("+proj=lcc +lat_1=44.26666666666667 +lat_2=44.56666666666667 +lat_0=44.18333333333333 +lon_0=-95.23333333333333 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378438.753 +b=6357053.055419666 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNRedwoodMeters = new ProjectionInfo("+proj=lcc +lat_1=44.26666666666667 +lat_2=44.56666666666667 +lat_0=44.18333333333333 +lon_0=-95.23333333333333 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378438.753 +b=6357053.055419666 +units=m +no_defs ");
            NAD1983HARNAdjMNRenvilleFeet = new ProjectionInfo("+proj=lcc +lat_1=44.53333333333333 +lat_2=44.91666666666666 +lat_0=44.45 +lon_0=-94.63333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378414.369 +b=6357028.753174601 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNRenvilleMeters = new ProjectionInfo("+proj=lcc +lat_1=44.53333333333333 +lat_2=44.91666666666666 +lat_0=44.45 +lon_0=-94.63333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378414.369 +b=6357028.753174601 +units=m +no_defs ");
            NAD1983HARNAdjMNRiceFeet = new ProjectionInfo("+proj=lcc +lat_1=44.3 +lat_2=44.66666666666666 +lat_0=44.18333333333333 +lon_0=-93.13333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378434.181 +b=6357048.498748716 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNRiceMeters = new ProjectionInfo("+proj=lcc +lat_1=44.3 +lat_2=44.66666666666666 +lat_0=44.18333333333333 +lon_0=-93.13333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378434.181 +b=6357048.498748716 +units=m +no_defs ");
            NAD1983HARNAdjMNRockFeet = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-95.95 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378624.681 +b=6357238.360038281 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNRockMeters = new ProjectionInfo("+proj=lcc +lat_1=43.56666666666667 +lat_2=43.8 +lat_0=43.5 +lon_0=-95.95 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378624.681 +b=6357238.360038281 +units=m +no_defs ");
            NAD1983HARNAdjMNRoseauFeet = new ProjectionInfo("+proj=lcc +lat_1=48.6 +lat_2=48.93333333333333 +lat_0=48.53333333333333 +lon_0=-96.15000000000001 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378449.421 +b=6357063.687651882 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNRoseauMeters = new ProjectionInfo("+proj=lcc +lat_1=48.6 +lat_2=48.93333333333333 +lat_0=48.53333333333333 +lon_0=-96.15000000000001 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378449.421 +b=6357063.687651882 +units=m +no_defs ");
            NAD1983HARNAdjMNScottFeet = new ProjectionInfo("+proj=lcc +lat_1=44.51666666666667 +lat_2=44.91666666666666 +lat_0=44.46666666666667 +lon_0=-93.31666666666666 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378421.989 +b=6357036.347626184 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNScottMeters = new ProjectionInfo("+proj=lcc +lat_1=44.51666666666667 +lat_2=44.91666666666666 +lat_0=44.46666666666667 +lon_0=-93.31666666666666 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378421.989 +b=6357036.347626184 +units=m +no_defs ");
            NAD1983HARNAdjMNSherburneFeet = new ProjectionInfo("+proj=lcc +lat_1=45.03333333333333 +lat_2=45.46666666666667 +lat_0=44.96666666666667 +lon_0=-93.88333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378443.325 +b=6357057.612090616 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNSherburneMeters = new ProjectionInfo("+proj=lcc +lat_1=45.03333333333333 +lat_2=45.46666666666667 +lat_0=44.96666666666667 +lon_0=-93.88333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378443.325 +b=6357057.612090616 +units=m +no_defs ");
            NAD1983HARNAdjMNSibleyFeet = new ProjectionInfo("+proj=lcc +lat_1=44.53333333333333 +lat_2=44.91666666666666 +lat_0=44.45 +lon_0=-94.63333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378414.369 +b=6357028.753174601 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNSibleyMeters = new ProjectionInfo("+proj=lcc +lat_1=44.53333333333333 +lat_2=44.91666666666666 +lat_0=44.45 +lon_0=-94.63333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378414.369 +b=6357028.753174601 +units=m +no_defs ");
            NAD1983HARNAdjMNStearnsFeet = new ProjectionInfo("+proj=lcc +lat_1=45.35 +lat_2=45.7 +lat_0=45.26666666666667 +lon_0=-95.15000000000001 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378502.761 +b=6357116.84881296 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNStearnsMeters = new ProjectionInfo("+proj=lcc +lat_1=45.35 +lat_2=45.7 +lat_0=45.26666666666667 +lon_0=-95.15000000000001 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378502.761 +b=6357116.84881296 +units=m +no_defs ");
            NAD1983HARNAdjMNSteeleFeet = new ProjectionInfo("+proj=lcc +lat_1=43.88333333333333 +lat_2=44.13333333333333 +lat_0=43.83333333333334 +lon_0=-92.91666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378481.425 +b=6357095.584348529 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNSteeleMeters = new ProjectionInfo("+proj=lcc +lat_1=43.88333333333333 +lat_2=44.13333333333333 +lat_0=43.83333333333334 +lon_0=-92.91666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378481.425 +b=6357095.584348529 +units=m +no_defs ");
            NAD1983HARNAdjMNStevensFeet = new ProjectionInfo("+proj=lcc +lat_1=45.35 +lat_2=45.7 +lat_0=45.26666666666667 +lon_0=-95.15000000000001 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378502.761 +b=6357116.84881296 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNStevensMeters = new ProjectionInfo("+proj=lcc +lat_1=45.35 +lat_2=45.7 +lat_0=45.26666666666667 +lon_0=-95.15000000000001 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378502.761 +b=6357116.84881296 +units=m +no_defs ");
            NAD1983HARNAdjMNStLouisCentralFeet = new ProjectionInfo("+proj=lcc +lat_1=47.33333333333334 +lat_2=47.75 +lat_0=47.25 +lon_0=-92.45 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378605.783 +b=6357219.525399698 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNStLouisCentralMeters = new ProjectionInfo("+proj=lcc +lat_1=47.33333333333334 +lat_2=47.75 +lat_0=47.25 +lon_0=-92.45 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378605.783 +b=6357219.525399698 +units=m +no_defs ");
            NAD1983HARNAdjMNStLouisNorthFeet = new ProjectionInfo("+proj=lcc +lat_1=47.98333333333333 +lat_2=48.53333333333333 +lat_0=47.83333333333334 +lon_0=-92.45 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378543.909 +b=6357157.858851505 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNStLouisNorthMeters = new ProjectionInfo("+proj=lcc +lat_1=47.98333333333333 +lat_2=48.53333333333333 +lat_0=47.83333333333334 +lon_0=-92.45 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378543.909 +b=6357157.858851505 +units=m +no_defs ");
            NAD1983HARNAdjMNStLouisSouthFeet = new ProjectionInfo("+proj=lcc +lat_1=46.78333333333333 +lat_2=47.13333333333333 +lat_0=46.65 +lon_0=-92.45 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378540.861 +b=6357154.821070872 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNStLouisSouthMeters = new ProjectionInfo("+proj=lcc +lat_1=46.78333333333333 +lat_2=47.13333333333333 +lat_0=46.65 +lon_0=-92.45 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378540.861 +b=6357154.821070872 +units=m +no_defs ");
            NAD1983HARNAdjMNSwiftFeet = new ProjectionInfo("+proj=lcc +lat_1=45.21666666666667 +lat_2=45.53333333333333 +lat_0=45.15 +lon_0=-96.05 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378470.757 +b=6357084.952116313 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNSwiftMeters = new ProjectionInfo("+proj=lcc +lat_1=45.21666666666667 +lat_2=45.53333333333333 +lat_0=45.15 +lon_0=-96.05 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378470.757 +b=6357084.952116313 +units=m +no_defs ");
            NAD1983HARNAdjMNToddFeet = new ProjectionInfo("+proj=lcc +lat_1=45.86666666666667 +lat_2=46.28333333333333 +lat_0=45.76666666666667 +lon_0=-94.90000000000001 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378548.481 +b=6357162.415522455 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNToddMeters = new ProjectionInfo("+proj=lcc +lat_1=45.86666666666667 +lat_2=46.28333333333333 +lat_0=45.76666666666667 +lon_0=-94.90000000000001 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378548.481 +b=6357162.415522455 +units=m +no_defs ");
            NAD1983HARNAdjMNTraverseFeet = new ProjectionInfo("+proj=lcc +lat_1=45.63333333333333 +lat_2=45.96666666666667 +lat_0=45.58333333333334 +lon_0=-96.55 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378463.746 +b=6357077.964622869 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNTraverseMeters = new ProjectionInfo("+proj=lcc +lat_1=45.63333333333333 +lat_2=45.96666666666667 +lat_0=45.58333333333334 +lon_0=-96.55 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378463.746 +b=6357077.964622869 +units=m +no_defs ");
            NAD1983HARNAdjMNWabashaFeet = new ProjectionInfo("+proj=lcc +lat_1=44.15 +lat_2=44.41666666666666 +lat_0=44.1 +lon_0=-92.26666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378426.561 +b=6357040.904297134 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNWabashaMeters = new ProjectionInfo("+proj=lcc +lat_1=44.15 +lat_2=44.41666666666666 +lat_0=44.1 +lon_0=-92.26666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378426.561 +b=6357040.904297134 +units=m +no_defs ");
            NAD1983HARNAdjMNWadenaFeet = new ProjectionInfo("+proj=lcc +lat_1=46.26666666666667 +lat_2=46.73333333333333 +lat_0=46.15 +lon_0=-94.46666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378546.957 +b=6357160.89663214 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNWadenaMeters = new ProjectionInfo("+proj=lcc +lat_1=46.26666666666667 +lat_2=46.73333333333333 +lat_0=46.15 +lon_0=-94.46666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378546.957 +b=6357160.89663214 +units=m +no_defs ");
            NAD1983HARNAdjMNWasecaFeet = new ProjectionInfo("+proj=lcc +lat_1=43.88333333333333 +lat_2=44.13333333333333 +lat_0=43.83333333333334 +lon_0=-92.91666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378481.425 +b=6357095.584348529 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNWasecaMeters = new ProjectionInfo("+proj=lcc +lat_1=43.88333333333333 +lat_2=44.13333333333333 +lat_0=43.83333333333334 +lon_0=-92.91666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378481.425 +b=6357095.584348529 +units=m +no_defs ");
            NAD1983HARNAdjMNWashingtonFeet = new ProjectionInfo("+proj=tmerc +lat_0=44.73333333333333 +lon_0=-92.83333333333333 +k=1.000040 +x_0=152406.3759409195 +y_0=30481.2751881839 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNWashingtonMeters = new ProjectionInfo("+proj=tmerc +lat_0=44.73333333333333 +lon_0=-92.83333333333333 +k=1.000040 +x_0=152406.3759409195 +y_0=30481.2751881839 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNAdjMNWatonwanFeet = new ProjectionInfo("+proj=lcc +lat_1=43.9 +lat_2=44.16666666666666 +lat_0=43.83333333333334 +lon_0=-94.91666666666667 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378514.953 +b=6357128.999935492 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNWatonwanMeters = new ProjectionInfo("+proj=lcc +lat_1=43.9 +lat_2=44.16666666666666 +lat_0=43.83333333333334 +lon_0=-94.91666666666667 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378514.953 +b=6357128.999935492 +units=m +no_defs ");
            NAD1983HARNAdjMNWilkinFeet = new ProjectionInfo("+proj=tmerc +lat_0=46.01666666666667 +lon_0=-96.51666666666667 +k=1.000049 +x_0=152407.7573379731 +y_0=30481.55146759461 +ellps=GRS80 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNWilkinMeters = new ProjectionInfo("+proj=tmerc +lat_0=46.01666666666667 +lon_0=-96.51666666666667 +k=1.000049 +x_0=152407.7573379731 +y_0=30481.55146759462 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNAdjMNWinonaFeet = new ProjectionInfo("+proj=lcc +lat_1=43.9 +lat_2=44.13333333333333 +lat_0=43.83333333333334 +lon_0=-91.61666666666666 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378453.688 +b=6357067.940345438 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNWinonaMeters = new ProjectionInfo("+proj=lcc +lat_1=43.9 +lat_2=44.13333333333333 +lat_0=43.83333333333334 +lon_0=-91.61666666666666 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378453.688 +b=6357067.940345438 +units=m +no_defs ");
            NAD1983HARNAdjMNWrightFeet = new ProjectionInfo("+proj=lcc +lat_1=45.03333333333333 +lat_2=45.46666666666667 +lat_0=44.96666666666667 +lon_0=-93.88333333333334 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378443.325 +b=6357057.612090616 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNWrightMeters = new ProjectionInfo("+proj=lcc +lat_1=45.03333333333333 +lat_2=45.46666666666667 +lat_0=44.96666666666667 +lon_0=-93.88333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378443.325 +b=6357057.612090616 +units=m +no_defs ");
            NAD1983HARNAdjMNYellowMedicineFeet = new ProjectionInfo("+proj=lcc +lat_1=44.66666666666666 +lat_2=44.95 +lat_0=44.53333333333333 +lon_0=-95.90000000000001 +x_0=152400.3048006096 +y_0=30480.06096012192 +a=6378530.193 +b=6357144.188838657 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNAdjMNYellowMedicineMeters = new ProjectionInfo("+proj=lcc +lat_1=44.66666666666666 +lat_2=44.95 +lat_0=44.53333333333333 +lon_0=-95.90000000000001 +x_0=152400.3048006096 +y_0=30480.06096012193 +a=6378530.193 +b=6357144.188838657 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591