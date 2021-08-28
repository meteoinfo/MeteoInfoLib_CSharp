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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:10:15 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// UtmWgs1972
    /// </summary>
    public class UtmWgs1972 : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo WGS1972UTMZone10N;
        public readonly ProjectionInfo WGS1972UTMZone10S;
        public readonly ProjectionInfo WGS1972UTMZone11N;
        public readonly ProjectionInfo WGS1972UTMZone11S;
        public readonly ProjectionInfo WGS1972UTMZone12N;
        public readonly ProjectionInfo WGS1972UTMZone12S;
        public readonly ProjectionInfo WGS1972UTMZone13N;
        public readonly ProjectionInfo WGS1972UTMZone13S;
        public readonly ProjectionInfo WGS1972UTMZone14N;
        public readonly ProjectionInfo WGS1972UTMZone14S;
        public readonly ProjectionInfo WGS1972UTMZone15N;
        public readonly ProjectionInfo WGS1972UTMZone15S;
        public readonly ProjectionInfo WGS1972UTMZone16N;
        public readonly ProjectionInfo WGS1972UTMZone16S;
        public readonly ProjectionInfo WGS1972UTMZone17N;
        public readonly ProjectionInfo WGS1972UTMZone17S;
        public readonly ProjectionInfo WGS1972UTMZone18N;
        public readonly ProjectionInfo WGS1972UTMZone18S;
        public readonly ProjectionInfo WGS1972UTMZone19N;
        public readonly ProjectionInfo WGS1972UTMZone19S;
        public readonly ProjectionInfo WGS1972UTMZone1N;
        public readonly ProjectionInfo WGS1972UTMZone1S;
        public readonly ProjectionInfo WGS1972UTMZone20N;
        public readonly ProjectionInfo WGS1972UTMZone20S;
        public readonly ProjectionInfo WGS1972UTMZone21N;
        public readonly ProjectionInfo WGS1972UTMZone21S;
        public readonly ProjectionInfo WGS1972UTMZone22N;
        public readonly ProjectionInfo WGS1972UTMZone22S;
        public readonly ProjectionInfo WGS1972UTMZone23N;
        public readonly ProjectionInfo WGS1972UTMZone23S;
        public readonly ProjectionInfo WGS1972UTMZone24N;
        public readonly ProjectionInfo WGS1972UTMZone24S;
        public readonly ProjectionInfo WGS1972UTMZone25N;
        public readonly ProjectionInfo WGS1972UTMZone25S;
        public readonly ProjectionInfo WGS1972UTMZone26N;
        public readonly ProjectionInfo WGS1972UTMZone26S;
        public readonly ProjectionInfo WGS1972UTMZone27N;
        public readonly ProjectionInfo WGS1972UTMZone27S;
        public readonly ProjectionInfo WGS1972UTMZone28N;
        public readonly ProjectionInfo WGS1972UTMZone28S;
        public readonly ProjectionInfo WGS1972UTMZone29N;
        public readonly ProjectionInfo WGS1972UTMZone29S;
        public readonly ProjectionInfo WGS1972UTMZone2N;
        public readonly ProjectionInfo WGS1972UTMZone2S;
        public readonly ProjectionInfo WGS1972UTMZone30N;
        public readonly ProjectionInfo WGS1972UTMZone30S;
        public readonly ProjectionInfo WGS1972UTMZone31N;
        public readonly ProjectionInfo WGS1972UTMZone31S;
        public readonly ProjectionInfo WGS1972UTMZone32N;
        public readonly ProjectionInfo WGS1972UTMZone32S;
        public readonly ProjectionInfo WGS1972UTMZone33N;
        public readonly ProjectionInfo WGS1972UTMZone33S;
        public readonly ProjectionInfo WGS1972UTMZone34N;
        public readonly ProjectionInfo WGS1972UTMZone34S;
        public readonly ProjectionInfo WGS1972UTMZone35N;
        public readonly ProjectionInfo WGS1972UTMZone35S;
        public readonly ProjectionInfo WGS1972UTMZone36N;
        public readonly ProjectionInfo WGS1972UTMZone36S;
        public readonly ProjectionInfo WGS1972UTMZone37N;
        public readonly ProjectionInfo WGS1972UTMZone37S;
        public readonly ProjectionInfo WGS1972UTMZone38N;
        public readonly ProjectionInfo WGS1972UTMZone38S;
        public readonly ProjectionInfo WGS1972UTMZone39N;
        public readonly ProjectionInfo WGS1972UTMZone39S;
        public readonly ProjectionInfo WGS1972UTMZone3N;
        public readonly ProjectionInfo WGS1972UTMZone3S;
        public readonly ProjectionInfo WGS1972UTMZone40N;
        public readonly ProjectionInfo WGS1972UTMZone40S;
        public readonly ProjectionInfo WGS1972UTMZone41N;
        public readonly ProjectionInfo WGS1972UTMZone41S;
        public readonly ProjectionInfo WGS1972UTMZone42N;
        public readonly ProjectionInfo WGS1972UTMZone42S;
        public readonly ProjectionInfo WGS1972UTMZone43N;
        public readonly ProjectionInfo WGS1972UTMZone43S;
        public readonly ProjectionInfo WGS1972UTMZone44N;
        public readonly ProjectionInfo WGS1972UTMZone44S;
        public readonly ProjectionInfo WGS1972UTMZone45N;
        public readonly ProjectionInfo WGS1972UTMZone45S;
        public readonly ProjectionInfo WGS1972UTMZone46N;
        public readonly ProjectionInfo WGS1972UTMZone46S;
        public readonly ProjectionInfo WGS1972UTMZone47N;
        public readonly ProjectionInfo WGS1972UTMZone47S;
        public readonly ProjectionInfo WGS1972UTMZone48N;
        public readonly ProjectionInfo WGS1972UTMZone48S;
        public readonly ProjectionInfo WGS1972UTMZone49N;
        public readonly ProjectionInfo WGS1972UTMZone49S;
        public readonly ProjectionInfo WGS1972UTMZone4N;
        public readonly ProjectionInfo WGS1972UTMZone4S;
        public readonly ProjectionInfo WGS1972UTMZone50N;
        public readonly ProjectionInfo WGS1972UTMZone50S;
        public readonly ProjectionInfo WGS1972UTMZone51N;
        public readonly ProjectionInfo WGS1972UTMZone51S;
        public readonly ProjectionInfo WGS1972UTMZone52N;
        public readonly ProjectionInfo WGS1972UTMZone52S;
        public readonly ProjectionInfo WGS1972UTMZone53N;
        public readonly ProjectionInfo WGS1972UTMZone53S;
        public readonly ProjectionInfo WGS1972UTMZone54N;
        public readonly ProjectionInfo WGS1972UTMZone54S;
        public readonly ProjectionInfo WGS1972UTMZone55N;
        public readonly ProjectionInfo WGS1972UTMZone55S;
        public readonly ProjectionInfo WGS1972UTMZone56N;
        public readonly ProjectionInfo WGS1972UTMZone56S;
        public readonly ProjectionInfo WGS1972UTMZone57N;
        public readonly ProjectionInfo WGS1972UTMZone57S;
        public readonly ProjectionInfo WGS1972UTMZone58N;
        public readonly ProjectionInfo WGS1972UTMZone58S;
        public readonly ProjectionInfo WGS1972UTMZone59N;
        public readonly ProjectionInfo WGS1972UTMZone59S;
        public readonly ProjectionInfo WGS1972UTMZone5N;
        public readonly ProjectionInfo WGS1972UTMZone5S;
        public readonly ProjectionInfo WGS1972UTMZone60N;
        public readonly ProjectionInfo WGS1972UTMZone60S;
        public readonly ProjectionInfo WGS1972UTMZone6N;
        public readonly ProjectionInfo WGS1972UTMZone6S;
        public readonly ProjectionInfo WGS1972UTMZone7N;
        public readonly ProjectionInfo WGS1972UTMZone7S;
        public readonly ProjectionInfo WGS1972UTMZone8N;
        public readonly ProjectionInfo WGS1972UTMZone8S;
        public readonly ProjectionInfo WGS1972UTMZone9N;
        public readonly ProjectionInfo WGS1972UTMZone9S;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of UtmWgs1972
        /// </summary>
        public UtmWgs1972()
        {
            WGS1972UTMZone10N = new ProjectionInfo("+proj=utm +zone=10 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone10S = new ProjectionInfo("+proj=utm +zone=10 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone11N = new ProjectionInfo("+proj=utm +zone=11 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone11S = new ProjectionInfo("+proj=utm +zone=11 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone12N = new ProjectionInfo("+proj=utm +zone=12 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone12S = new ProjectionInfo("+proj=utm +zone=12 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone13N = new ProjectionInfo("+proj=utm +zone=13 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone13S = new ProjectionInfo("+proj=utm +zone=13 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone14N = new ProjectionInfo("+proj=utm +zone=14 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone14S = new ProjectionInfo("+proj=utm +zone=14 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone15N = new ProjectionInfo("+proj=utm +zone=15 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone15S = new ProjectionInfo("+proj=utm +zone=15 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone16N = new ProjectionInfo("+proj=utm +zone=16 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone16S = new ProjectionInfo("+proj=utm +zone=16 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone17N = new ProjectionInfo("+proj=utm +zone=17 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone17S = new ProjectionInfo("+proj=utm +zone=17 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone18S = new ProjectionInfo("+proj=utm +zone=18 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone19S = new ProjectionInfo("+proj=utm +zone=19 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone1N = new ProjectionInfo("+proj=utm +zone=1 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone1S = new ProjectionInfo("+proj=utm +zone=1 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone20S = new ProjectionInfo("+proj=utm +zone=20 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone21S = new ProjectionInfo("+proj=utm +zone=21 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone22N = new ProjectionInfo("+proj=utm +zone=22 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone22S = new ProjectionInfo("+proj=utm +zone=22 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone23N = new ProjectionInfo("+proj=utm +zone=23 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone23S = new ProjectionInfo("+proj=utm +zone=23 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone24N = new ProjectionInfo("+proj=utm +zone=24 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone24S = new ProjectionInfo("+proj=utm +zone=24 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone25N = new ProjectionInfo("+proj=utm +zone=25 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone25S = new ProjectionInfo("+proj=utm +zone=25 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone26N = new ProjectionInfo("+proj=utm +zone=26 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone26S = new ProjectionInfo("+proj=utm +zone=26 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone27N = new ProjectionInfo("+proj=utm +zone=27 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone27S = new ProjectionInfo("+proj=utm +zone=27 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone28S = new ProjectionInfo("+proj=utm +zone=28 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone29S = new ProjectionInfo("+proj=utm +zone=29 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone2N = new ProjectionInfo("+proj=utm +zone=2 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone2S = new ProjectionInfo("+proj=utm +zone=2 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone30N = new ProjectionInfo("+proj=utm +zone=30 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone30S = new ProjectionInfo("+proj=utm +zone=30 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone31N = new ProjectionInfo("+proj=utm +zone=31 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone31S = new ProjectionInfo("+proj=utm +zone=31 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone32S = new ProjectionInfo("+proj=utm +zone=32 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone33N = new ProjectionInfo("+proj=utm +zone=33 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone33S = new ProjectionInfo("+proj=utm +zone=33 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone34N = new ProjectionInfo("+proj=utm +zone=34 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone34S = new ProjectionInfo("+proj=utm +zone=34 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone35N = new ProjectionInfo("+proj=utm +zone=35 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone35S = new ProjectionInfo("+proj=utm +zone=35 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone36N = new ProjectionInfo("+proj=utm +zone=36 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone36S = new ProjectionInfo("+proj=utm +zone=36 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone37N = new ProjectionInfo("+proj=utm +zone=37 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone37S = new ProjectionInfo("+proj=utm +zone=37 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone38S = new ProjectionInfo("+proj=utm +zone=38 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone39S = new ProjectionInfo("+proj=utm +zone=39 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone3N = new ProjectionInfo("+proj=utm +zone=3 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone3S = new ProjectionInfo("+proj=utm +zone=3 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone40N = new ProjectionInfo("+proj=utm +zone=40 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone40S = new ProjectionInfo("+proj=utm +zone=40 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone41N = new ProjectionInfo("+proj=utm +zone=41 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone41S = new ProjectionInfo("+proj=utm +zone=41 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone42N = new ProjectionInfo("+proj=utm +zone=42 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone42S = new ProjectionInfo("+proj=utm +zone=42 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone43N = new ProjectionInfo("+proj=utm +zone=43 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone43S = new ProjectionInfo("+proj=utm +zone=43 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone44N = new ProjectionInfo("+proj=utm +zone=44 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone44S = new ProjectionInfo("+proj=utm +zone=44 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone45N = new ProjectionInfo("+proj=utm +zone=45 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone45S = new ProjectionInfo("+proj=utm +zone=45 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone46N = new ProjectionInfo("+proj=utm +zone=46 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone46S = new ProjectionInfo("+proj=utm +zone=46 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone47N = new ProjectionInfo("+proj=utm +zone=47 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone47S = new ProjectionInfo("+proj=utm +zone=47 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone48N = new ProjectionInfo("+proj=utm +zone=48 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone48S = new ProjectionInfo("+proj=utm +zone=48 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone49N = new ProjectionInfo("+proj=utm +zone=49 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone49S = new ProjectionInfo("+proj=utm +zone=49 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone4N = new ProjectionInfo("+proj=utm +zone=4 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone4S = new ProjectionInfo("+proj=utm +zone=4 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone50N = new ProjectionInfo("+proj=utm +zone=50 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone50S = new ProjectionInfo("+proj=utm +zone=50 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone51N = new ProjectionInfo("+proj=utm +zone=51 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone51S = new ProjectionInfo("+proj=utm +zone=51 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone52N = new ProjectionInfo("+proj=utm +zone=52 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone52S = new ProjectionInfo("+proj=utm +zone=52 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone53N = new ProjectionInfo("+proj=utm +zone=53 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone53S = new ProjectionInfo("+proj=utm +zone=53 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone54N = new ProjectionInfo("+proj=utm +zone=54 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone54S = new ProjectionInfo("+proj=utm +zone=54 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone55N = new ProjectionInfo("+proj=utm +zone=55 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone55S = new ProjectionInfo("+proj=utm +zone=55 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone56N = new ProjectionInfo("+proj=utm +zone=56 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone56S = new ProjectionInfo("+proj=utm +zone=56 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone57N = new ProjectionInfo("+proj=utm +zone=57 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone57S = new ProjectionInfo("+proj=utm +zone=57 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone58N = new ProjectionInfo("+proj=utm +zone=58 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone59N = new ProjectionInfo("+proj=utm +zone=59 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone59S = new ProjectionInfo("+proj=utm +zone=59 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone5N = new ProjectionInfo("+proj=utm +zone=5 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone5S = new ProjectionInfo("+proj=utm +zone=5 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone60N = new ProjectionInfo("+proj=utm +zone=60 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone60S = new ProjectionInfo("+proj=utm +zone=60 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone6N = new ProjectionInfo("+proj=utm +zone=6 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone6S = new ProjectionInfo("+proj=utm +zone=6 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone7N = new ProjectionInfo("+proj=utm +zone=7 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone7S = new ProjectionInfo("+proj=utm +zone=7 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone8N = new ProjectionInfo("+proj=utm +zone=8 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone8S = new ProjectionInfo("+proj=utm +zone=8 +south +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone9N = new ProjectionInfo("+proj=utm +zone=9 +ellps=WGS72 +units=m +no_defs ");
            WGS1972UTMZone9S = new ProjectionInfo("+proj=utm +zone=9 +south +ellps=WGS72 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591