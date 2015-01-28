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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:47:45 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// NationalGridsCanada
    /// </summary>
    public class NationalGridsCanada : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo ATS1977MTM4NovaScotia;
        public readonly ProjectionInfo ATS1977MTM5NovaScotia;
        public readonly ProjectionInfo ATS1977NewBrunswickStereographic;
        public readonly ProjectionInfo NAD192710TMAEPForest;
        public readonly ProjectionInfo NAD192710TMAEPResource;
        public readonly ProjectionInfo NAD19273TM111;
        public readonly ProjectionInfo NAD19273TM114;
        public readonly ProjectionInfo NAD19273TM117;
        public readonly ProjectionInfo NAD19273TM120;
        public readonly ProjectionInfo NAD1927CGQ77MTM10SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM2SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM3SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM4SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM5SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM6SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM7SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM8SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77MTM9SCoPQ;
        public readonly ProjectionInfo NAD1927CGQ77QuebecLambert;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone17N;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone18N;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone19N;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone20N;
        public readonly ProjectionInfo NAD1927CGQ77UTMZone21N;
        public readonly ProjectionInfo NAD1927DEF1976MTM10;
        public readonly ProjectionInfo NAD1927DEF1976MTM11;
        public readonly ProjectionInfo NAD1927DEF1976MTM12;
        public readonly ProjectionInfo NAD1927DEF1976MTM13;
        public readonly ProjectionInfo NAD1927DEF1976MTM14;
        public readonly ProjectionInfo NAD1927DEF1976MTM15;
        public readonly ProjectionInfo NAD1927DEF1976MTM16;
        public readonly ProjectionInfo NAD1927DEF1976MTM17;
        public readonly ProjectionInfo NAD1927DEF1976MTM8;
        public readonly ProjectionInfo NAD1927DEF1976MTM9;
        public readonly ProjectionInfo NAD1927DEF1976UTMZone15N;
        public readonly ProjectionInfo NAD1927DEF1976UTMZone16N;
        public readonly ProjectionInfo NAD1927DEF1976UTMZone17N;
        public readonly ProjectionInfo NAD1927DEF1976UTMZone18N;
        public readonly ProjectionInfo NAD1927MTM1;
        public readonly ProjectionInfo NAD1927MTM2;
        public readonly ProjectionInfo NAD1927MTM3;
        public readonly ProjectionInfo NAD1927MTM4;
        public readonly ProjectionInfo NAD1927MTM5;
        public readonly ProjectionInfo NAD1927MTM6;
        public readonly ProjectionInfo NAD1927QuebecLambert;
        public readonly ProjectionInfo NAD198310TMAEPForest;
        public readonly ProjectionInfo NAD198310TMAEPResource;
        public readonly ProjectionInfo NAD19833TM111;
        public readonly ProjectionInfo NAD19833TM114;
        public readonly ProjectionInfo NAD19833TM117;
        public readonly ProjectionInfo NAD19833TM120;
        public readonly ProjectionInfo NAD1983BCEnvironmentAlbers;
        public readonly ProjectionInfo NAD1983CSRS98MTM10;
        public readonly ProjectionInfo NAD1983CSRS98MTM2SCoPQ;
        public readonly ProjectionInfo NAD1983CSRS98MTM3;
        public readonly ProjectionInfo NAD1983CSRS98MTM4;
        public readonly ProjectionInfo NAD1983CSRS98MTM5;
        public readonly ProjectionInfo NAD1983CSRS98MTM6;
        public readonly ProjectionInfo NAD1983CSRS98MTM7;
        public readonly ProjectionInfo NAD1983CSRS98MTM8;
        public readonly ProjectionInfo NAD1983CSRS98MTM9;
        public readonly ProjectionInfo NAD1983CSRS98NewBrunswickStereographic;
        public readonly ProjectionInfo NAD1983CSRS98PrinceEdwardIsland;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone11N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone12N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone13N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone17N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone18N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone19N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone20N;
        public readonly ProjectionInfo NAD1983CSRS98UTMZone21N;
        public readonly ProjectionInfo NAD1983MTM1;
        public readonly ProjectionInfo NAD1983MTM10;
        public readonly ProjectionInfo NAD1983MTM11;
        public readonly ProjectionInfo NAD1983MTM12;
        public readonly ProjectionInfo NAD1983MTM13;
        public readonly ProjectionInfo NAD1983MTM14;
        public readonly ProjectionInfo NAD1983MTM15;
        public readonly ProjectionInfo NAD1983MTM16;
        public readonly ProjectionInfo NAD1983MTM17;
        public readonly ProjectionInfo NAD1983MTM2;
        public readonly ProjectionInfo NAD1983MTM2SCoPQ;
        public readonly ProjectionInfo NAD1983MTM3;
        public readonly ProjectionInfo NAD1983MTM4;
        public readonly ProjectionInfo NAD1983MTM5;
        public readonly ProjectionInfo NAD1983MTM6;
        public readonly ProjectionInfo NAD1983MTM7;
        public readonly ProjectionInfo NAD1983MTM8;
        public readonly ProjectionInfo NAD1983MTM9;
        public readonly ProjectionInfo NAD1983QuebecLambert;
        public readonly ProjectionInfo PrinceEdwardIslandStereographic;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NationalGridsCanada
        /// </summary>
        public NationalGridsCanada()
        {
            ATS1977MTM4NovaScotia = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-61.5 +k=0.999900 +x_0=4500000 +y_0=0 +a=6378135 +b=6356750.304921594 +units=m +no_defs ");
            ATS1977MTM5NovaScotia = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-64.5 +k=0.999900 +x_0=5500000 +y_0=0 +a=6378135 +b=6356750.304921594 +units=m +no_defs ");
            ATS1977NewBrunswickStereographic = new ProjectionInfo("+a=6378135 +b=6356750.304921594 +units=m +no_defs ");
            NAD192710TMAEPForest = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-115 +k=0.999200 +x_0=500000 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD192710TMAEPResource = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-115 +k=0.999200 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD19273TM111 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-111 +k=0.999900 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD19273TM114 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-114 +k=0.999900 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD19273TM117 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-117 +k=0.999900 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD19273TM120 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-120 +k=0.999900 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927CGQ77MTM10SCoPQ = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-79.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM2SCoPQ = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-55.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM3SCoPQ = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-58.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM4SCoPQ = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-61.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM5SCoPQ = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-64.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM6SCoPQ = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-67.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM7SCoPQ = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-70.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM8SCoPQ = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-73.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77MTM9SCoPQ = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-76.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77QuebecLambert = new ProjectionInfo("+proj=lcc +lat_1=46 +lat_2=60 +lat_0=44 +lon_0=-68.5 +x_0=0 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77UTMZone17N = new ProjectionInfo("+proj=utm +zone=17 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77UTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77UTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77UTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=clrk66 +units=m +no_defs ");
            NAD1927CGQ77UTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM10 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-79.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM11 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-82.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM12 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-81 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM13 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-84 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM14 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-87 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM15 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-90 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM16 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-93 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM17 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-96 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM8 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-73.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976MTM9 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-76.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976UTMZone15N = new ProjectionInfo("+proj=utm +zone=15 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976UTMZone16N = new ProjectionInfo("+proj=utm +zone=16 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976UTMZone17N = new ProjectionInfo("+proj=utm +zone=17 +ellps=clrk66 +units=m +no_defs ");
            NAD1927DEF1976UTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=clrk66 +units=m +no_defs ");
            NAD1927MTM1 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-53 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927MTM2 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-56 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927MTM3 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-58.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927MTM4 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-61.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927MTM5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-64.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927MTM6 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-67.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927QuebecLambert = new ProjectionInfo("+proj=lcc +lat_1=46 +lat_2=60 +lat_0=44 +lon_0=-68.5 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD198310TMAEPForest = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-115 +k=0.999200 +x_0=500000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD198310TMAEPResource = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-115 +k=0.999200 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD19833TM111 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-111 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD19833TM114 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-114 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD19833TM117 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-117 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD19833TM120 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-120 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983BCEnvironmentAlbers = new ProjectionInfo("+proj=aea +lat_1=50 +lat_2=58.5 +lat_0=45 +lon_0=-126 +x_0=1000000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983CSRS98MTM10 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-79.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM2SCoPQ = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-55.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM3 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-58.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM4 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-61.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-64.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM6 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-67.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM7 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-70.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM8 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-73.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98MTM9 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-76.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98NewBrunswickStereographic = new ProjectionInfo("+ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98PrinceEdwardIsland = new ProjectionInfo("+ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone11N = new ProjectionInfo("+proj=utm +zone=11 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone12N = new ProjectionInfo("+proj=utm +zone=12 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone13N = new ProjectionInfo("+proj=utm +zone=13 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone17N = new ProjectionInfo("+proj=utm +zone=17 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=GRS80 +units=m +no_defs ");
            NAD1983CSRS98UTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=GRS80 +units=m +no_defs ");
            NAD1983MTM1 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-53 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM10 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-79.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM11 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-82.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM12 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-81 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM13 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-84 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM14 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-87 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM15 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-90 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM16 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-93 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM17 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-96 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM2 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-56 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM2SCoPQ = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-55.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM3 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-58.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM4 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-61.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-64.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM6 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-67.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM7 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-70.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM8 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-73.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983MTM9 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-76.5 +k=0.999900 +x_0=304800 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983QuebecLambert = new ProjectionInfo("+proj=lcc +lat_1=46 +lat_2=60 +lat_0=44 +lon_0=-68.5 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            PrinceEdwardIslandStereographic = new ProjectionInfo("+a=6378135 +b=6356750.304921594 +units=m +no_defs ");


        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591