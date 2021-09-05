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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:07:32 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
#pragma warning disable 1591
namespace MeteoInfoC.Projections.GeographicCategories
{


    /// <summary>
    /// CountySystems
    /// </summary>
    public class CountySystems : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1983HARNAdjMNAnoka;
        public readonly ProjectionInfo NAD1983HARNAdjMNBecker;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeltramiNorth;
        public readonly ProjectionInfo NAD1983HARNAdjMNBeltramiSouth;
        public readonly ProjectionInfo NAD1983HARNAdjMNBenton;
        public readonly ProjectionInfo NAD1983HARNAdjMNBigStone;
        public readonly ProjectionInfo NAD1983HARNAdjMNBlueEarth;
        public readonly ProjectionInfo NAD1983HARNAdjMNBrown;
        public readonly ProjectionInfo NAD1983HARNAdjMNCarlton;
        public readonly ProjectionInfo NAD1983HARNAdjMNCarver;
        public readonly ProjectionInfo NAD1983HARNAdjMNCassNorth;
        public readonly ProjectionInfo NAD1983HARNAdjMNCassSouth;
        public readonly ProjectionInfo NAD1983HARNAdjMNChippewa;
        public readonly ProjectionInfo NAD1983HARNAdjMNChisago;
        public readonly ProjectionInfo NAD1983HARNAdjMNCookNorth;
        public readonly ProjectionInfo NAD1983HARNAdjMNCookSouth;
        public readonly ProjectionInfo NAD1983HARNAdjMNCottonwood;
        public readonly ProjectionInfo NAD1983HARNAdjMNCrowWing;
        public readonly ProjectionInfo NAD1983HARNAdjMNDakota;
        public readonly ProjectionInfo NAD1983HARNAdjMNDodge;
        public readonly ProjectionInfo NAD1983HARNAdjMNDouglas;
        public readonly ProjectionInfo NAD1983HARNAdjMNFaribault;
        public readonly ProjectionInfo NAD1983HARNAdjMNFillmore;
        public readonly ProjectionInfo NAD1983HARNAdjMNFreeborn;
        public readonly ProjectionInfo NAD1983HARNAdjMNGoodhue;
        public readonly ProjectionInfo NAD1983HARNAdjMNGrant;
        public readonly ProjectionInfo NAD1983HARNAdjMNHennepin;
        public readonly ProjectionInfo NAD1983HARNAdjMNHouston;
        public readonly ProjectionInfo NAD1983HARNAdjMNIsanti;
        public readonly ProjectionInfo NAD1983HARNAdjMNItascaNorth;
        public readonly ProjectionInfo NAD1983HARNAdjMNItascaSouth;
        public readonly ProjectionInfo NAD1983HARNAdjMNJackson;
        public readonly ProjectionInfo NAD1983HARNAdjMNKanabec;
        public readonly ProjectionInfo NAD1983HARNAdjMNKandiyohi;
        public readonly ProjectionInfo NAD1983HARNAdjMNKittson;
        public readonly ProjectionInfo NAD1983HARNAdjMNKoochiching;
        public readonly ProjectionInfo NAD1983HARNAdjMNLacQuiParle;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeoftheWoodsNorth;
        public readonly ProjectionInfo NAD1983HARNAdjMNLakeoftheWoodsSouth;
        public readonly ProjectionInfo NAD1983HARNAdjMNLeSueur;
        public readonly ProjectionInfo NAD1983HARNAdjMNLincoln;
        public readonly ProjectionInfo NAD1983HARNAdjMNLyon;
        public readonly ProjectionInfo NAD1983HARNAdjMNMahnomen;
        public readonly ProjectionInfo NAD1983HARNAdjMNMarshall;
        public readonly ProjectionInfo NAD1983HARNAdjMNMartin;
        public readonly ProjectionInfo NAD1983HARNAdjMNMcLeod;
        public readonly ProjectionInfo NAD1983HARNAdjMNMeeker;
        public readonly ProjectionInfo NAD1983HARNAdjMNMorrison;
        public readonly ProjectionInfo NAD1983HARNAdjMNMower;
        public readonly ProjectionInfo NAD1983HARNAdjMNMurray;
        public readonly ProjectionInfo NAD1983HARNAdjMNNicollet;
        public readonly ProjectionInfo NAD1983HARNAdjMNNobles;
        public readonly ProjectionInfo NAD1983HARNAdjMNNorman;
        public readonly ProjectionInfo NAD1983HARNAdjMNOlmsted;
        public readonly ProjectionInfo NAD1983HARNAdjMNOttertail;
        public readonly ProjectionInfo NAD1983HARNAdjMNPennington;
        public readonly ProjectionInfo NAD1983HARNAdjMNPine;
        public readonly ProjectionInfo NAD1983HARNAdjMNPipestone;
        public readonly ProjectionInfo NAD1983HARNAdjMNPolk;
        public readonly ProjectionInfo NAD1983HARNAdjMNPope;
        public readonly ProjectionInfo NAD1983HARNAdjMNRamsey;
        public readonly ProjectionInfo NAD1983HARNAdjMNRedLake;
        public readonly ProjectionInfo NAD1983HARNAdjMNRedwood;
        public readonly ProjectionInfo NAD1983HARNAdjMNRenville;
        public readonly ProjectionInfo NAD1983HARNAdjMNRice;
        public readonly ProjectionInfo NAD1983HARNAdjMNRock;
        public readonly ProjectionInfo NAD1983HARNAdjMNRoseau;
        public readonly ProjectionInfo NAD1983HARNAdjMNScott;
        public readonly ProjectionInfo NAD1983HARNAdjMNSherburne;
        public readonly ProjectionInfo NAD1983HARNAdjMNSibley;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisCentral;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisNorth;
        public readonly ProjectionInfo NAD1983HARNAdjMNStLouisSouth;
        public readonly ProjectionInfo NAD1983HARNAdjMNStearns;
        public readonly ProjectionInfo NAD1983HARNAdjMNSteele;
        public readonly ProjectionInfo NAD1983HARNAdjMNStevens;
        public readonly ProjectionInfo NAD1983HARNAdjMNSwift;
        public readonly ProjectionInfo NAD1983HARNAdjMNTodd;
        public readonly ProjectionInfo NAD1983HARNAdjMNTraverse;
        public readonly ProjectionInfo NAD1983HARNAdjMNWabasha;
        public readonly ProjectionInfo NAD1983HARNAdjMNWadena;
        public readonly ProjectionInfo NAD1983HARNAdjMNWaseca;
        public readonly ProjectionInfo NAD1983HARNAdjMNWatonwan;
        public readonly ProjectionInfo NAD1983HARNAdjMNWinona;
        public readonly ProjectionInfo NAD1983HARNAdjMNWright;
        public readonly ProjectionInfo NAD1983HARNAdjMNYellowMedicine;
        public readonly ProjectionInfo NAD1983HARNAdjWIAdams;
        public readonly ProjectionInfo NAD1983HARNAdjWIAshland;
        public readonly ProjectionInfo NAD1983HARNAdjWIBarron;
        public readonly ProjectionInfo NAD1983HARNAdjWIBayfield;
        public readonly ProjectionInfo NAD1983HARNAdjWIBrown;
        public readonly ProjectionInfo NAD1983HARNAdjWIBuffalo;
        public readonly ProjectionInfo NAD1983HARNAdjWIBurnett;
        public readonly ProjectionInfo NAD1983HARNAdjWICalumet;
        public readonly ProjectionInfo NAD1983HARNAdjWIChippewa;
        public readonly ProjectionInfo NAD1983HARNAdjWIClark;
        public readonly ProjectionInfo NAD1983HARNAdjWIColumbia;
        public readonly ProjectionInfo NAD1983HARNAdjWICrawford;
        public readonly ProjectionInfo NAD1983HARNAdjWIDane;
        public readonly ProjectionInfo NAD1983HARNAdjWIDodge;
        public readonly ProjectionInfo NAD1983HARNAdjWIDoor;
        public readonly ProjectionInfo NAD1983HARNAdjWIDouglas;
        public readonly ProjectionInfo NAD1983HARNAdjWIDunn;
        public readonly ProjectionInfo NAD1983HARNAdjWIEauClaire;
        public readonly ProjectionInfo NAD1983HARNAdjWIFlorence;
        public readonly ProjectionInfo NAD1983HARNAdjWIFondduLac;
        public readonly ProjectionInfo NAD1983HARNAdjWIForest;
        public readonly ProjectionInfo NAD1983HARNAdjWIGrant;
        public readonly ProjectionInfo NAD1983HARNAdjWIGreen;
        public readonly ProjectionInfo NAD1983HARNAdjWIGreenLake;
        public readonly ProjectionInfo NAD1983HARNAdjWIIowa;
        public readonly ProjectionInfo NAD1983HARNAdjWIIron;
        public readonly ProjectionInfo NAD1983HARNAdjWIJackson;
        public readonly ProjectionInfo NAD1983HARNAdjWIJefferson;
        public readonly ProjectionInfo NAD1983HARNAdjWIJuneau;
        public readonly ProjectionInfo NAD1983HARNAdjWIKenosha;
        public readonly ProjectionInfo NAD1983HARNAdjWIKewaunee;
        public readonly ProjectionInfo NAD1983HARNAdjWILaCrosse;
        public readonly ProjectionInfo NAD1983HARNAdjWILafayette;
        public readonly ProjectionInfo NAD1983HARNAdjWILanglade;
        public readonly ProjectionInfo NAD1983HARNAdjWILincoln;
        public readonly ProjectionInfo NAD1983HARNAdjWIManitowoc;
        public readonly ProjectionInfo NAD1983HARNAdjWIMarathon;
        public readonly ProjectionInfo NAD1983HARNAdjWIMarinette;
        public readonly ProjectionInfo NAD1983HARNAdjWIMarquette;
        public readonly ProjectionInfo NAD1983HARNAdjWIMenominee;
        public readonly ProjectionInfo NAD1983HARNAdjWIMilwaukee;
        public readonly ProjectionInfo NAD1983HARNAdjWIMonroe;
        public readonly ProjectionInfo NAD1983HARNAdjWIOconto;
        public readonly ProjectionInfo NAD1983HARNAdjWIOneida;
        public readonly ProjectionInfo NAD1983HARNAdjWIOutagamie;
        public readonly ProjectionInfo NAD1983HARNAdjWIOzaukee;
        public readonly ProjectionInfo NAD1983HARNAdjWIPepin;
        public readonly ProjectionInfo NAD1983HARNAdjWIPierce;
        public readonly ProjectionInfo NAD1983HARNAdjWIPolk;
        public readonly ProjectionInfo NAD1983HARNAdjWIPortage;
        public readonly ProjectionInfo NAD1983HARNAdjWIPrice;
        public readonly ProjectionInfo NAD1983HARNAdjWIRacine;
        public readonly ProjectionInfo NAD1983HARNAdjWIRichland;
        public readonly ProjectionInfo NAD1983HARNAdjWIRock;
        public readonly ProjectionInfo NAD1983HARNAdjWIRusk;
        public readonly ProjectionInfo NAD1983HARNAdjWISauk;
        public readonly ProjectionInfo NAD1983HARNAdjWISawyer;
        public readonly ProjectionInfo NAD1983HARNAdjWIShawano;
        public readonly ProjectionInfo NAD1983HARNAdjWISheboygan;
        public readonly ProjectionInfo NAD1983HARNAdjWIStCroix;
        public readonly ProjectionInfo NAD1983HARNAdjWITaylor;
        public readonly ProjectionInfo NAD1983HARNAdjWITrempealeau;
        public readonly ProjectionInfo NAD1983HARNAdjWIVernon;
        public readonly ProjectionInfo NAD1983HARNAdjWIVilas;
        public readonly ProjectionInfo NAD1983HARNAdjWIWalworth;
        public readonly ProjectionInfo NAD1983HARNAdjWIWashburn;
        public readonly ProjectionInfo NAD1983HARNAdjWIWashington;
        public readonly ProjectionInfo NAD1983HARNAdjWIWaukesha;
        public readonly ProjectionInfo NAD1983HARNAdjWIWaupaca;
        public readonly ProjectionInfo NAD1983HARNAdjWIWaushara;
        public readonly ProjectionInfo NAD1983HARNAdjWIWinnebago;
        public readonly ProjectionInfo NAD1983HARNAdjWIWood;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CountySystems
        /// </summary>
        public CountySystems()
        {
            NAD1983HARNAdjMNAnoka = new ProjectionInfo("+proj=longlat +a=6378418.941 +b=6357033.309845551 +no_defs ");
            NAD1983HARNAdjMNBecker = new ProjectionInfo("+proj=longlat +a=6378586.581 +b=6357200.387780368 +no_defs ");
            NAD1983HARNAdjMNBeltramiNorth = new ProjectionInfo("+proj=longlat +a=6378505.809 +b=6357119.886593593 +no_defs ");
            NAD1983HARNAdjMNBeltramiSouth = new ProjectionInfo("+proj=longlat +a=6378544.823 +b=6357158.769787037 +no_defs ");
            NAD1983HARNAdjMNBenton = new ProjectionInfo("+proj=longlat +a=6378490.569 +b=6357104.697690427 +no_defs ");
            NAD1983HARNAdjMNBigStone = new ProjectionInfo("+proj=longlat +a=6378470.757 +b=6357084.952116313 +no_defs ");
            NAD1983HARNAdjMNBlueEarth = new ProjectionInfo("+proj=longlat +a=6378403.701 +b=6357018.120942386 +no_defs ");
            NAD1983HARNAdjMNBrown = new ProjectionInfo("+proj=longlat +a=6378434.181 +b=6357048.498748716 +no_defs ");
            NAD1983HARNAdjMNCarlton = new ProjectionInfo("+proj=longlat +a=6378454.907 +b=6357069.155258362 +no_defs ");
            NAD1983HARNAdjMNCarver = new ProjectionInfo("+proj=longlat +a=6378400.653 +b=6357015.083161753 +no_defs ");
            NAD1983HARNAdjMNCassNorth = new ProjectionInfo("+proj=longlat +a=6378567.378 +b=6357181.249164391 +no_defs ");
            NAD1983HARNAdjMNCassSouth = new ProjectionInfo("+proj=longlat +a=6378546.957 +b=6357160.89663214 +no_defs ");
            NAD1983HARNAdjMNChippewa = new ProjectionInfo("+proj=longlat +a=6378476.853 +b=6357091.027677579 +no_defs ");
            NAD1983HARNAdjMNChisago = new ProjectionInfo("+proj=longlat +a=6378411.321000001 +b=6357025.715393969 +no_defs ");
            NAD1983HARNAdjMNCookNorth = new ProjectionInfo("+proj=longlat +a=6378647.541 +b=6357261.14339303 +no_defs ");
            NAD1983HARNAdjMNCookSouth = new ProjectionInfo("+proj=longlat +a=6378647.541 +b=6357261.14339303 +no_defs ");
            NAD1983HARNAdjMNCottonwood = new ProjectionInfo("+proj=longlat +a=6378514.953 +b=6357128.999935492 +no_defs ");
            NAD1983HARNAdjMNCrowWing = new ProjectionInfo("+proj=longlat +a=6378546.957 +b=6357160.89663214 +no_defs ");
            NAD1983HARNAdjMNDakota = new ProjectionInfo("+proj=longlat +a=6378421.989 +b=6357036.347626184 +no_defs ");
            NAD1983HARNAdjMNDodge = new ProjectionInfo("+proj=longlat +a=6378481.425 +b=6357095.584348529 +no_defs ");
            NAD1983HARNAdjMNDouglas = new ProjectionInfo("+proj=longlat +a=6378518.001 +b=6357132.037716125 +no_defs ");
            NAD1983HARNAdjMNFaribault = new ProjectionInfo("+proj=longlat +a=6378521.049 +b=6357135.075496757 +no_defs ");
            NAD1983HARNAdjMNFillmore = new ProjectionInfo("+proj=longlat +a=6378464.661 +b=6357078.876555047 +no_defs ");
            NAD1983HARNAdjMNFreeborn = new ProjectionInfo("+proj=longlat +a=6378521.049 +b=6357135.075496757 +no_defs ");
            NAD1983HARNAdjMNGoodhue = new ProjectionInfo("+proj=longlat +a=6378434.181 +b=6357048.498748716 +no_defs ");
            NAD1983HARNAdjMNGrant = new ProjectionInfo("+proj=longlat +a=6378518.001 +b=6357132.037716125 +no_defs ");
            NAD1983HARNAdjMNHennepin = new ProjectionInfo("+proj=longlat +a=6378418.941 +b=6357033.309845551 +no_defs ");
            NAD1983HARNAdjMNHouston = new ProjectionInfo("+proj=longlat +a=6378436.619 +b=6357050.928574564 +no_defs ");
            NAD1983HARNAdjMNIsanti = new ProjectionInfo("+proj=longlat +a=6378411.321000001 +b=6357025.715393969 +no_defs ");
            NAD1983HARNAdjMNItascaNorth = new ProjectionInfo("+proj=longlat +a=6378574.389 +b=6357188.236657837 +no_defs ");
            NAD1983HARNAdjMNItascaSouth = new ProjectionInfo("+proj=longlat +a=6378574.389 +b=6357188.236657837 +no_defs ");
            NAD1983HARNAdjMNJackson = new ProjectionInfo("+proj=longlat +a=6378521.049 +b=6357135.075496757 +no_defs ");
            NAD1983HARNAdjMNKanabec = new ProjectionInfo("+proj=longlat +a=6378472.281 +b=6357086.47100663 +no_defs ");
            NAD1983HARNAdjMNKandiyohi = new ProjectionInfo("+proj=longlat +a=6378498.189 +b=6357112.29214201 +no_defs ");
            NAD1983HARNAdjMNKittson = new ProjectionInfo("+proj=longlat +a=6378449.421 +b=6357063.687651882 +no_defs ");
            NAD1983HARNAdjMNKoochiching = new ProjectionInfo("+proj=longlat +a=6378525.621 +b=6357139.632167708 +no_defs ");
            NAD1983HARNAdjMNLacQuiParle = new ProjectionInfo("+proj=longlat +a=6378476.853 +b=6357091.027677579 +no_defs ");
            NAD1983HARNAdjMNLakeoftheWoodsNorth = new ProjectionInfo("+proj=longlat +a=6378466.185 +b=6357080.395445363 +no_defs ");
            NAD1983HARNAdjMNLakeoftheWoodsSouth = new ProjectionInfo("+proj=longlat +a=6378496.665 +b=6357110.773251694 +no_defs ");
            NAD1983HARNAdjMNLeSueur = new ProjectionInfo("+proj=longlat +a=6378434.181 +b=6357048.498748716 +no_defs ");
            NAD1983HARNAdjMNLincoln = new ProjectionInfo("+proj=longlat +a=6378643.579 +b=6357257.194676865 +no_defs ");
            NAD1983HARNAdjMNLyon = new ProjectionInfo("+proj=longlat +a=6378559.758 +b=6357173.65471281 +no_defs ");
            NAD1983HARNAdjMNMahnomen = new ProjectionInfo("+proj=longlat +a=6378586.581 +b=6357200.387780368 +no_defs ");
            NAD1983HARNAdjMNMarshall = new ProjectionInfo("+proj=longlat +a=6378441.801 +b=6357056.093200299 +no_defs ");
            NAD1983HARNAdjMNMartin = new ProjectionInfo("+proj=longlat +a=6378521.049 +b=6357135.075496757 +no_defs ");
            NAD1983HARNAdjMNMcLeod = new ProjectionInfo("+proj=longlat +a=6378414.369 +b=6357028.753174601 +no_defs ");
            NAD1983HARNAdjMNMeeker = new ProjectionInfo("+proj=longlat +a=6378498.189 +b=6357112.29214201 +no_defs ");
            NAD1983HARNAdjMNMorrison = new ProjectionInfo("+proj=longlat +a=6378502.761 +b=6357116.84881296 +no_defs ");
            NAD1983HARNAdjMNMower = new ProjectionInfo("+proj=longlat +a=6378521.049 +b=6357135.075496757 +no_defs ");
            NAD1983HARNAdjMNMurray = new ProjectionInfo("+proj=longlat +a=6378617.061 +b=6357230.765586698 +no_defs ");
            NAD1983HARNAdjMNNicollet = new ProjectionInfo("+proj=longlat +a=6378403.701 +b=6357018.120942386 +no_defs ");
            NAD1983HARNAdjMNNobles = new ProjectionInfo("+proj=longlat +a=6378624.681 +b=6357238.360038281 +no_defs ");
            NAD1983HARNAdjMNNorman = new ProjectionInfo("+proj=longlat +a=6378468.623 +b=6357082.825271211 +no_defs ");
            NAD1983HARNAdjMNOlmsted = new ProjectionInfo("+proj=longlat +a=6378481.425 +b=6357095.584348529 +no_defs ");
            NAD1983HARNAdjMNOttertail = new ProjectionInfo("+proj=longlat +a=6378525.621 +b=6357139.632167708 +no_defs ");
            NAD1983HARNAdjMNPennington = new ProjectionInfo("+proj=longlat +a=6378445.763 +b=6357060.041916464 +no_defs ");
            NAD1983HARNAdjMNPine = new ProjectionInfo("+proj=longlat +a=6378472.281 +b=6357086.47100663 +no_defs ");
            NAD1983HARNAdjMNPipestone = new ProjectionInfo("+proj=longlat +a=6378670.401 +b=6357283.926747777 +no_defs ");
            NAD1983HARNAdjMNPolk = new ProjectionInfo("+proj=longlat +a=6378445.763 +b=6357060.041916464 +no_defs ");
            NAD1983HARNAdjMNPope = new ProjectionInfo("+proj=longlat +a=6378502.761 +b=6357116.84881296 +no_defs ");
            NAD1983HARNAdjMNRamsey = new ProjectionInfo("+proj=longlat +a=6378418.941 +b=6357033.309845551 +no_defs ");
            NAD1983HARNAdjMNRedLake = new ProjectionInfo("+proj=longlat +a=6378445.763 +b=6357060.041916464 +no_defs ");
            NAD1983HARNAdjMNRedwood = new ProjectionInfo("+proj=longlat +a=6378438.753 +b=6357053.055419666 +no_defs ");
            NAD1983HARNAdjMNRenville = new ProjectionInfo("+proj=longlat +a=6378414.369 +b=6357028.753174601 +no_defs ");
            NAD1983HARNAdjMNRice = new ProjectionInfo("+proj=longlat +a=6378434.181 +b=6357048.498748716 +no_defs ");
            NAD1983HARNAdjMNRock = new ProjectionInfo("+proj=longlat +a=6378624.681 +b=6357238.360038281 +no_defs ");
            NAD1983HARNAdjMNRoseau = new ProjectionInfo("+proj=longlat +a=6378449.421 +b=6357063.687651882 +no_defs ");
            NAD1983HARNAdjMNScott = new ProjectionInfo("+proj=longlat +a=6378421.989 +b=6357036.347626184 +no_defs ");
            NAD1983HARNAdjMNSherburne = new ProjectionInfo("+proj=longlat +a=6378443.325 +b=6357057.612090616 +no_defs ");
            NAD1983HARNAdjMNSibley = new ProjectionInfo("+proj=longlat +a=6378414.369 +b=6357028.753174601 +no_defs ");
            NAD1983HARNAdjMNStLouisCentral = new ProjectionInfo("+proj=longlat +a=6378605.783 +b=6357219.525399698 +no_defs ");
            NAD1983HARNAdjMNStLouisNorth = new ProjectionInfo("+proj=longlat +a=6378543.909 +b=6357157.858851505 +no_defs ");
            NAD1983HARNAdjMNStLouisSouth = new ProjectionInfo("+proj=longlat +a=6378540.861 +b=6357154.821070872 +no_defs ");
            NAD1983HARNAdjMNStearns = new ProjectionInfo("+proj=longlat +a=6378502.761 +b=6357116.84881296 +no_defs ");
            NAD1983HARNAdjMNSteele = new ProjectionInfo("+proj=longlat +a=6378481.425 +b=6357095.584348529 +no_defs ");
            NAD1983HARNAdjMNStevens = new ProjectionInfo("+proj=longlat +a=6378502.761 +b=6357116.84881296 +no_defs ");
            NAD1983HARNAdjMNSwift = new ProjectionInfo("+proj=longlat +a=6378470.757 +b=6357084.952116313 +no_defs ");
            NAD1983HARNAdjMNTodd = new ProjectionInfo("+proj=longlat +a=6378548.481 +b=6357162.415522455 +no_defs ");
            NAD1983HARNAdjMNTraverse = new ProjectionInfo("+proj=longlat +a=6378463.746 +b=6357077.964622869 +no_defs ");
            NAD1983HARNAdjMNWabasha = new ProjectionInfo("+proj=longlat +a=6378426.561 +b=6357040.904297134 +no_defs ");
            NAD1983HARNAdjMNWadena = new ProjectionInfo("+proj=longlat +a=6378546.957 +b=6357160.89663214 +no_defs ");
            NAD1983HARNAdjMNWaseca = new ProjectionInfo("+proj=longlat +a=6378481.425 +b=6357095.584348529 +no_defs ");
            NAD1983HARNAdjMNWatonwan = new ProjectionInfo("+proj=longlat +a=6378514.953 +b=6357128.999935492 +no_defs ");
            NAD1983HARNAdjMNWinona = new ProjectionInfo("+proj=longlat +a=6378453.688 +b=6357067.940345438 +no_defs ");
            NAD1983HARNAdjMNWright = new ProjectionInfo("+proj=longlat +a=6378443.325 +b=6357057.612090616 +no_defs ");
            NAD1983HARNAdjMNYellowMedicine = new ProjectionInfo("+proj=longlat +a=6378530.193 +b=6357144.188838657 +no_defs ");
            NAD1983HARNAdjWIAdams = new ProjectionInfo("+proj=longlat +a=6378376.271 +b=6356991.5851403 +no_defs ");
            NAD1983HARNAdjWIAshland = new ProjectionInfo("+proj=longlat +a=6378471.92 +b=6357087.2341403 +no_defs ");
            NAD1983HARNAdjWIBarron = new ProjectionInfo("+proj=longlat +a=6378472.931 +b=6357088.2451403 +no_defs ");
            NAD1983HARNAdjWIBayfield = new ProjectionInfo("+proj=longlat +a=6378411.351 +b=6357026.6651403 +no_defs ");
            NAD1983HARNAdjWIBrown = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            NAD1983HARNAdjWIBuffalo = new ProjectionInfo("+proj=longlat +a=6378380.991 +b=6356996.305140301 +no_defs ");
            NAD1983HARNAdjWIBurnett = new ProjectionInfo("+proj=longlat +a=6378414.96 +b=6357030.2741403 +no_defs ");
            NAD1983HARNAdjWICalumet = new ProjectionInfo("+proj=longlat +a=6378345.09 +b=6356960.4041403 +no_defs ");
            NAD1983HARNAdjWIChippewa = new ProjectionInfo("+proj=longlat +a=6378412.542 +b=6357027.856140301 +no_defs ");
            NAD1983HARNAdjWIClark = new ProjectionInfo("+proj=longlat +a=6378470.401 +b=6357085.7151403 +no_defs ");
            NAD1983HARNAdjWIColumbia = new ProjectionInfo("+proj=longlat +a=6378376.331 +b=6356991.645140301 +no_defs ");
            NAD1983HARNAdjWICrawford = new ProjectionInfo("+proj=longlat +a=6378379.031 +b=6356994.345140301 +no_defs ");
            NAD1983HARNAdjWIDane = new ProjectionInfo("+proj=longlat +a=6378407.621 +b=6357022.935140301 +no_defs ");
            NAD1983HARNAdjWIDodge = new ProjectionInfo("+proj=longlat +a=6378376.811 +b=6356992.1251403 +no_defs ");
            NAD1983HARNAdjWIDoor = new ProjectionInfo("+proj=longlat +a=6378313.92 +b=6356929.2341403 +no_defs ");
            NAD1983HARNAdjWIDouglas = new ProjectionInfo("+proj=longlat +a=6378414.93 +b=6357030.2441403 +no_defs ");
            NAD1983HARNAdjWIDunn = new ProjectionInfo("+proj=longlat +a=6378413.021 +b=6357028.3351403 +no_defs ");
            NAD1983HARNAdjWIEauClaire = new ProjectionInfo("+proj=longlat +a=6378380.381 +b=6356995.6951403 +no_defs ");
            NAD1983HARNAdjWIFlorence = new ProjectionInfo("+proj=longlat +a=6378530.851 +b=6357146.1651403 +no_defs ");
            NAD1983HARNAdjWIFondduLac = new ProjectionInfo("+proj=longlat +a=6378345.09 +b=6356960.4041403 +no_defs ");
            NAD1983HARNAdjWIForest = new ProjectionInfo("+proj=longlat +a=6378591.521 +b=6357206.8351403 +no_defs ");
            NAD1983HARNAdjWIGrant = new ProjectionInfo("+proj=longlat +a=6378378.881 +b=6356994.1951403 +no_defs ");
            NAD1983HARNAdjWIGreen = new ProjectionInfo("+proj=longlat +a=6378408.481 +b=6357023.7951403 +no_defs ");
            NAD1983HARNAdjWIGreenLake = new ProjectionInfo("+proj=longlat +a=6378375.601 +b=6356990.9151403 +no_defs ");
            NAD1983HARNAdjWIIowa = new ProjectionInfo("+proj=longlat +a=6378408.041 +b=6357023.355140301 +no_defs ");
            NAD1983HARNAdjWIIron = new ProjectionInfo("+proj=longlat +a=6378655.071000001 +b=6357270.385140301 +no_defs ");
            NAD1983HARNAdjWIJackson = new ProjectionInfo("+proj=longlat +a=6378409.151 +b=6357024.4651403 +no_defs ");
            NAD1983HARNAdjWIJefferson = new ProjectionInfo("+proj=longlat +a=6378376.811 +b=6356992.1251403 +no_defs ");
            NAD1983HARNAdjWIJuneau = new ProjectionInfo("+proj=longlat +a=6378376.271 +b=6356991.5851403 +no_defs ");
            NAD1983HARNAdjWIKenosha = new ProjectionInfo("+proj=longlat +a=6378315.7 +b=6356931.014140301 +no_defs ");
            NAD1983HARNAdjWIKewaunee = new ProjectionInfo("+proj=longlat +a=6378285.86 +b=6356901.174140301 +no_defs ");
            NAD1983HARNAdjWILaCrosse = new ProjectionInfo("+proj=longlat +a=6378379.301 +b=6356994.6151403 +no_defs ");
            NAD1983HARNAdjWILafayette = new ProjectionInfo("+proj=longlat +a=6378408.481 +b=6357023.7951403 +no_defs ");
            NAD1983HARNAdjWILanglade = new ProjectionInfo("+proj=longlat +a=6378560.121 +b=6357175.435140301 +no_defs ");
            NAD1983HARNAdjWILincoln = new ProjectionInfo("+proj=longlat +a=6378531.821000001 +b=6357147.135140301 +no_defs ");
            NAD1983HARNAdjWIManitowoc = new ProjectionInfo("+proj=longlat +a=6378285.86 +b=6356901.174140301 +no_defs ");
            NAD1983HARNAdjWIMarathon = new ProjectionInfo("+proj=longlat +a=6378500.6 +b=6357115.9141403 +no_defs ");
            NAD1983HARNAdjWIMarinette = new ProjectionInfo("+proj=longlat +a=6378376.041 +b=6356991.355140301 +no_defs ");
            NAD1983HARNAdjWIMarquette = new ProjectionInfo("+proj=longlat +a=6378375.601 +b=6356990.9151403 +no_defs ");
            NAD1983HARNAdjWIMenominee = new ProjectionInfo("+proj=longlat +a=6378406.601 +b=6357021.9151403 +no_defs ");
            NAD1983HARNAdjWIMilwaukee = new ProjectionInfo("+proj=longlat +a=6378315.7 +b=6356931.014140301 +no_defs ");
            NAD1983HARNAdjWIMonroe = new ProjectionInfo("+proj=longlat +a=6378438.991 +b=6357054.305140301 +no_defs ");
            NAD1983HARNAdjWIOconto = new ProjectionInfo("+proj=longlat +a=6378345.42 +b=6356960.7341403 +no_defs ");
            NAD1983HARNAdjWIOneida = new ProjectionInfo("+proj=longlat +a=6378593.86 +b=6357209.174140301 +no_defs ");
            NAD1983HARNAdjWIOutagamie = new ProjectionInfo("+proj=longlat +a=6378345.09 +b=6356960.4041403 +no_defs ");
            NAD1983HARNAdjWIOzaukee = new ProjectionInfo("+proj=longlat +a=6378315.7 +b=6356931.014140301 +no_defs ");
            NAD1983HARNAdjWIPepin = new ProjectionInfo("+proj=longlat +a=6378381.271 +b=6356996.5851403 +no_defs ");
            NAD1983HARNAdjWIPierce = new ProjectionInfo("+proj=longlat +a=6378381.271 +b=6356996.5851403 +no_defs ");
            NAD1983HARNAdjWIPolk = new ProjectionInfo("+proj=longlat +a=6378413.671 +b=6357028.9851403 +no_defs ");
            NAD1983HARNAdjWIPortage = new ProjectionInfo("+proj=longlat +a=6378344.377 +b=6356959.691139228 +no_defs ");
            NAD1983HARNAdjWIPrice = new ProjectionInfo("+proj=longlat +a=6378563.891 +b=6357179.2051403 +no_defs ");
            NAD1983HARNAdjWIRacine = new ProjectionInfo("+proj=longlat +a=6378315.7 +b=6356931.014140301 +no_defs ");
            NAD1983HARNAdjWIRichland = new ProjectionInfo("+proj=longlat +a=6378408.091 +b=6357023.4051403 +no_defs ");
            NAD1983HARNAdjWIRock = new ProjectionInfo("+proj=longlat +a=6378377.671 +b=6356992.9851403 +no_defs ");
            NAD1983HARNAdjWIRusk = new ProjectionInfo("+proj=longlat +a=6378472.751 +b=6357088.0651403 +no_defs ");
            NAD1983HARNAdjWISauk = new ProjectionInfo("+proj=longlat +a=6378407.281 +b=6357022.595140301 +no_defs ");
            NAD1983HARNAdjWISawyer = new ProjectionInfo("+proj=longlat +a=6378534.451 +b=6357149.765140301 +no_defs ");
            NAD1983HARNAdjWIShawano = new ProjectionInfo("+proj=longlat +a=6378406.051 +b=6357021.3651403 +no_defs ");
            NAD1983HARNAdjWISheboygan = new ProjectionInfo("+proj=longlat +a=6378285.86 +b=6356901.174140301 +no_defs ");
            NAD1983HARNAdjWIStCroix = new ProjectionInfo("+proj=longlat +a=6378412.511 +b=6357027.8251403 +no_defs ");
            NAD1983HARNAdjWITaylor = new ProjectionInfo("+proj=longlat +a=6378532.921 +b=6357148.2351403 +no_defs ");
            NAD1983HARNAdjWITrempealeau = new ProjectionInfo("+proj=longlat +a=6378380.091 +b=6356995.4051403 +no_defs ");
            NAD1983HARNAdjWIVernon = new ProjectionInfo("+proj=longlat +a=6378408.941 +b=6357024.2551403 +no_defs ");
            NAD1983HARNAdjWIVilas = new ProjectionInfo("+proj=longlat +a=6378624.171 +b=6357239.4851403 +no_defs ");
            NAD1983HARNAdjWIWalworth = new ProjectionInfo("+proj=longlat +a=6378377.411 +b=6356992.725140301 +no_defs ");
            NAD1983HARNAdjWIWashburn = new ProjectionInfo("+proj=longlat +a=6378474.591 +b=6357089.9051403 +no_defs ");
            NAD1983HARNAdjWIWashington = new ProjectionInfo("+proj=longlat +a=6378407.141 +b=6357022.4551403 +no_defs ");
            NAD1983HARNAdjWIWaukesha = new ProjectionInfo("+proj=longlat +a=6378376.871 +b=6356992.185140301 +no_defs ");
            NAD1983HARNAdjWIWaupaca = new ProjectionInfo("+proj=longlat +a=6378375.251 +b=6356990.5651403 +no_defs ");
            NAD1983HARNAdjWIWaushara = new ProjectionInfo("+proj=longlat +a=6378405.971 +b=6357021.2851403 +no_defs ");
            NAD1983HARNAdjWIWinnebago = new ProjectionInfo("+proj=longlat +a=6378345.09 +b=6356960.4041403 +no_defs ");
            NAD1983HARNAdjWIWood = new ProjectionInfo("+proj=longlat +a=6378437.651 +b=6357052.9651403 +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591