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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:11:31 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// UtmWgs1984
    /// </summary>
    public class UtmWgs1984 : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo WGS1984ComplexUTMZone20N;
        public readonly ProjectionInfo WGS1984ComplexUTMZone21N;
        public readonly ProjectionInfo WGS1984ComplexUTMZone22N;
        public readonly ProjectionInfo WGS1984ComplexUTMZone23N;
        public readonly ProjectionInfo WGS1984ComplexUTMZone24N;
        public readonly ProjectionInfo WGS1984ComplexUTMZone25N;
        public readonly ProjectionInfo WGS1984ComplexUTMZone26N;
        public readonly ProjectionInfo WGS1984ComplexUTMZone27N;
        public readonly ProjectionInfo WGS1984ComplexUTMZone28N;
        public readonly ProjectionInfo WGS1984ComplexUTMZone29N;
        public readonly ProjectionInfo WGS1984ComplexUTMZone30N;
        public readonly ProjectionInfo WGS1984UTMZone10N;
        public readonly ProjectionInfo WGS1984UTMZone10S;
        public readonly ProjectionInfo WGS1984UTMZone11N;
        public readonly ProjectionInfo WGS1984UTMZone11S;
        public readonly ProjectionInfo WGS1984UTMZone12N;
        public readonly ProjectionInfo WGS1984UTMZone12S;
        public readonly ProjectionInfo WGS1984UTMZone13N;
        public readonly ProjectionInfo WGS1984UTMZone13S;
        public readonly ProjectionInfo WGS1984UTMZone14N;
        public readonly ProjectionInfo WGS1984UTMZone14S;
        public readonly ProjectionInfo WGS1984UTMZone15N;
        public readonly ProjectionInfo WGS1984UTMZone15S;
        public readonly ProjectionInfo WGS1984UTMZone16N;
        public readonly ProjectionInfo WGS1984UTMZone16S;
        public readonly ProjectionInfo WGS1984UTMZone17N;
        public readonly ProjectionInfo WGS1984UTMZone17S;
        public readonly ProjectionInfo WGS1984UTMZone18N;
        public readonly ProjectionInfo WGS1984UTMZone18S;
        public readonly ProjectionInfo WGS1984UTMZone19N;
        public readonly ProjectionInfo WGS1984UTMZone19S;
        public readonly ProjectionInfo WGS1984UTMZone1N;
        public readonly ProjectionInfo WGS1984UTMZone1S;
        public readonly ProjectionInfo WGS1984UTMZone20N;
        public readonly ProjectionInfo WGS1984UTMZone20S;
        public readonly ProjectionInfo WGS1984UTMZone21N;
        public readonly ProjectionInfo WGS1984UTMZone21S;
        public readonly ProjectionInfo WGS1984UTMZone22N;
        public readonly ProjectionInfo WGS1984UTMZone22S;
        public readonly ProjectionInfo WGS1984UTMZone23N;
        public readonly ProjectionInfo WGS1984UTMZone23S;
        public readonly ProjectionInfo WGS1984UTMZone24N;
        public readonly ProjectionInfo WGS1984UTMZone24S;
        public readonly ProjectionInfo WGS1984UTMZone25N;
        public readonly ProjectionInfo WGS1984UTMZone25S;
        public readonly ProjectionInfo WGS1984UTMZone26N;
        public readonly ProjectionInfo WGS1984UTMZone26S;
        public readonly ProjectionInfo WGS1984UTMZone27N;
        public readonly ProjectionInfo WGS1984UTMZone27S;
        public readonly ProjectionInfo WGS1984UTMZone28N;
        public readonly ProjectionInfo WGS1984UTMZone28S;
        public readonly ProjectionInfo WGS1984UTMZone29N;
        public readonly ProjectionInfo WGS1984UTMZone29S;
        public readonly ProjectionInfo WGS1984UTMZone2N;
        public readonly ProjectionInfo WGS1984UTMZone2S;
        public readonly ProjectionInfo WGS1984UTMZone30N;
        public readonly ProjectionInfo WGS1984UTMZone30S;
        public readonly ProjectionInfo WGS1984UTMZone31N;
        public readonly ProjectionInfo WGS1984UTMZone31S;
        public readonly ProjectionInfo WGS1984UTMZone32N;
        public readonly ProjectionInfo WGS1984UTMZone32S;
        public readonly ProjectionInfo WGS1984UTMZone33N;
        public readonly ProjectionInfo WGS1984UTMZone33S;
        public readonly ProjectionInfo WGS1984UTMZone34N;
        public readonly ProjectionInfo WGS1984UTMZone34S;
        public readonly ProjectionInfo WGS1984UTMZone35N;
        public readonly ProjectionInfo WGS1984UTMZone35S;
        public readonly ProjectionInfo WGS1984UTMZone36N;
        public readonly ProjectionInfo WGS1984UTMZone36S;
        public readonly ProjectionInfo WGS1984UTMZone37N;
        public readonly ProjectionInfo WGS1984UTMZone37S;
        public readonly ProjectionInfo WGS1984UTMZone38N;
        public readonly ProjectionInfo WGS1984UTMZone38S;
        public readonly ProjectionInfo WGS1984UTMZone39N;
        public readonly ProjectionInfo WGS1984UTMZone39S;
        public readonly ProjectionInfo WGS1984UTMZone3N;
        public readonly ProjectionInfo WGS1984UTMZone3S;
        public readonly ProjectionInfo WGS1984UTMZone40N;
        public readonly ProjectionInfo WGS1984UTMZone40S;
        public readonly ProjectionInfo WGS1984UTMZone41N;
        public readonly ProjectionInfo WGS1984UTMZone41S;
        public readonly ProjectionInfo WGS1984UTMZone42N;
        public readonly ProjectionInfo WGS1984UTMZone42S;
        public readonly ProjectionInfo WGS1984UTMZone43N;
        public readonly ProjectionInfo WGS1984UTMZone43S;
        public readonly ProjectionInfo WGS1984UTMZone44N;
        public readonly ProjectionInfo WGS1984UTMZone44S;
        public readonly ProjectionInfo WGS1984UTMZone45N;
        public readonly ProjectionInfo WGS1984UTMZone45S;
        public readonly ProjectionInfo WGS1984UTMZone46N;
        public readonly ProjectionInfo WGS1984UTMZone46S;
        public readonly ProjectionInfo WGS1984UTMZone47N;
        public readonly ProjectionInfo WGS1984UTMZone47S;
        public readonly ProjectionInfo WGS1984UTMZone48N;
        public readonly ProjectionInfo WGS1984UTMZone48S;
        public readonly ProjectionInfo WGS1984UTMZone49N;
        public readonly ProjectionInfo WGS1984UTMZone49S;
        public readonly ProjectionInfo WGS1984UTMZone4N;
        public readonly ProjectionInfo WGS1984UTMZone4S;
        public readonly ProjectionInfo WGS1984UTMZone50N;
        public readonly ProjectionInfo WGS1984UTMZone50S;
        public readonly ProjectionInfo WGS1984UTMZone51N;
        public readonly ProjectionInfo WGS1984UTMZone51S;
        public readonly ProjectionInfo WGS1984UTMZone52N;
        public readonly ProjectionInfo WGS1984UTMZone52S;
        public readonly ProjectionInfo WGS1984UTMZone53N;
        public readonly ProjectionInfo WGS1984UTMZone53S;
        public readonly ProjectionInfo WGS1984UTMZone54N;
        public readonly ProjectionInfo WGS1984UTMZone54S;
        public readonly ProjectionInfo WGS1984UTMZone55N;
        public readonly ProjectionInfo WGS1984UTMZone55S;
        public readonly ProjectionInfo WGS1984UTMZone56N;
        public readonly ProjectionInfo WGS1984UTMZone56S;
        public readonly ProjectionInfo WGS1984UTMZone57N;
        public readonly ProjectionInfo WGS1984UTMZone57S;
        public readonly ProjectionInfo WGS1984UTMZone58N;
        public readonly ProjectionInfo WGS1984UTMZone58S;
        public readonly ProjectionInfo WGS1984UTMZone59N;
        public readonly ProjectionInfo WGS1984UTMZone59S;
        public readonly ProjectionInfo WGS1984UTMZone5N;
        public readonly ProjectionInfo WGS1984UTMZone5S;
        public readonly ProjectionInfo WGS1984UTMZone60N;
        public readonly ProjectionInfo WGS1984UTMZone60S;
        public readonly ProjectionInfo WGS1984UTMZone6N;
        public readonly ProjectionInfo WGS1984UTMZone6S;
        public readonly ProjectionInfo WGS1984UTMZone7N;
        public readonly ProjectionInfo WGS1984UTMZone7S;
        public readonly ProjectionInfo WGS1984UTMZone8N;
        public readonly ProjectionInfo WGS1984UTMZone8S;
        public readonly ProjectionInfo WGS1984UTMZone9N;
        public readonly ProjectionInfo WGS1984UTMZone9S;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of UtmWgs1984
        /// </summary>
        public UtmWgs1984()
        {
            WGS1984ComplexUTMZone20N = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984ComplexUTMZone21N = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984ComplexUTMZone22N = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984ComplexUTMZone23N = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984ComplexUTMZone24N = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984ComplexUTMZone25N = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984ComplexUTMZone26N = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984ComplexUTMZone27N = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984ComplexUTMZone28N = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984ComplexUTMZone29N = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984ComplexUTMZone30N = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone10N = new ProjectionInfo("+proj=utm +zone=10 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone10S = new ProjectionInfo("+proj=utm +zone=10 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone11N = new ProjectionInfo("+proj=utm +zone=11 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone11S = new ProjectionInfo("+proj=utm +zone=11 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone12N = new ProjectionInfo("+proj=utm +zone=12 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone12S = new ProjectionInfo("+proj=utm +zone=12 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone13N = new ProjectionInfo("+proj=utm +zone=13 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone13S = new ProjectionInfo("+proj=utm +zone=13 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone14N = new ProjectionInfo("+proj=utm +zone=14 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone14S = new ProjectionInfo("+proj=utm +zone=14 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone15N = new ProjectionInfo("+proj=utm +zone=15 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone15S = new ProjectionInfo("+proj=utm +zone=15 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone16N = new ProjectionInfo("+proj=utm +zone=16 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone16S = new ProjectionInfo("+proj=utm +zone=16 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone17N = new ProjectionInfo("+proj=utm +zone=17 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone17S = new ProjectionInfo("+proj=utm +zone=17 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone18S = new ProjectionInfo("+proj=utm +zone=18 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone19S = new ProjectionInfo("+proj=utm +zone=19 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone1N = new ProjectionInfo("+proj=utm +zone=1 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone1S = new ProjectionInfo("+proj=utm +zone=1 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone20S = new ProjectionInfo("+proj=utm +zone=20 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone21S = new ProjectionInfo("+proj=utm +zone=21 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone22N = new ProjectionInfo("+proj=utm +zone=22 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone22S = new ProjectionInfo("+proj=utm +zone=22 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone23N = new ProjectionInfo("+proj=utm +zone=23 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone23S = new ProjectionInfo("+proj=utm +zone=23 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone24N = new ProjectionInfo("+proj=utm +zone=24 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone24S = new ProjectionInfo("+proj=utm +zone=24 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone25N = new ProjectionInfo("+proj=utm +zone=25 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone25S = new ProjectionInfo("+proj=utm +zone=25 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone26N = new ProjectionInfo("+proj=utm +zone=26 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone26S = new ProjectionInfo("+proj=utm +zone=26 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone27N = new ProjectionInfo("+proj=utm +zone=27 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone27S = new ProjectionInfo("+proj=utm +zone=27 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone28S = new ProjectionInfo("+proj=utm +zone=28 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone29S = new ProjectionInfo("+proj=utm +zone=29 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone2N = new ProjectionInfo("+proj=utm +zone=2 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone2S = new ProjectionInfo("+proj=utm +zone=2 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone30N = new ProjectionInfo("+proj=utm +zone=30 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone30S = new ProjectionInfo("+proj=utm +zone=30 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone31N = new ProjectionInfo("+proj=utm +zone=31 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone31S = new ProjectionInfo("+proj=utm +zone=31 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone32S = new ProjectionInfo("+proj=utm +zone=32 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone33N = new ProjectionInfo("+proj=utm +zone=33 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone33S = new ProjectionInfo("+proj=utm +zone=33 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone34N = new ProjectionInfo("+proj=utm +zone=34 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone34S = new ProjectionInfo("+proj=utm +zone=34 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone35N = new ProjectionInfo("+proj=utm +zone=35 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone35S = new ProjectionInfo("+proj=utm +zone=35 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone36N = new ProjectionInfo("+proj=utm +zone=36 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone36S = new ProjectionInfo("+proj=utm +zone=36 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone37N = new ProjectionInfo("+proj=utm +zone=37 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone37S = new ProjectionInfo("+proj=utm +zone=37 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone38S = new ProjectionInfo("+proj=utm +zone=38 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone39S = new ProjectionInfo("+proj=utm +zone=39 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone3N = new ProjectionInfo("+proj=utm +zone=3 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone3S = new ProjectionInfo("+proj=utm +zone=3 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone40N = new ProjectionInfo("+proj=utm +zone=40 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone40S = new ProjectionInfo("+proj=utm +zone=40 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone41N = new ProjectionInfo("+proj=utm +zone=41 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone41S = new ProjectionInfo("+proj=utm +zone=41 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone42N = new ProjectionInfo("+proj=utm +zone=42 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone42S = new ProjectionInfo("+proj=utm +zone=42 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone43N = new ProjectionInfo("+proj=utm +zone=43 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone43S = new ProjectionInfo("+proj=utm +zone=43 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone44N = new ProjectionInfo("+proj=utm +zone=44 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone44S = new ProjectionInfo("+proj=utm +zone=44 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone45N = new ProjectionInfo("+proj=utm +zone=45 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone45S = new ProjectionInfo("+proj=utm +zone=45 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone46N = new ProjectionInfo("+proj=utm +zone=46 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone46S = new ProjectionInfo("+proj=utm +zone=46 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone47N = new ProjectionInfo("+proj=utm +zone=47 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone47S = new ProjectionInfo("+proj=utm +zone=47 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone48N = new ProjectionInfo("+proj=utm +zone=48 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone48S = new ProjectionInfo("+proj=utm +zone=48 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone49N = new ProjectionInfo("+proj=utm +zone=49 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone49S = new ProjectionInfo("+proj=utm +zone=49 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone4N = new ProjectionInfo("+proj=utm +zone=4 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone4S = new ProjectionInfo("+proj=utm +zone=4 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone50N = new ProjectionInfo("+proj=utm +zone=50 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone50S = new ProjectionInfo("+proj=utm +zone=50 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone51N = new ProjectionInfo("+proj=utm +zone=51 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone51S = new ProjectionInfo("+proj=utm +zone=51 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone52N = new ProjectionInfo("+proj=utm +zone=52 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone52S = new ProjectionInfo("+proj=utm +zone=52 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone53N = new ProjectionInfo("+proj=utm +zone=53 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone53S = new ProjectionInfo("+proj=utm +zone=53 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone54N = new ProjectionInfo("+proj=utm +zone=54 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone54S = new ProjectionInfo("+proj=utm +zone=54 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone55N = new ProjectionInfo("+proj=utm +zone=55 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone55S = new ProjectionInfo("+proj=utm +zone=55 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone56N = new ProjectionInfo("+proj=utm +zone=56 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone56S = new ProjectionInfo("+proj=utm +zone=56 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone57N = new ProjectionInfo("+proj=utm +zone=57 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone57S = new ProjectionInfo("+proj=utm +zone=57 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone58N = new ProjectionInfo("+proj=utm +zone=58 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone59N = new ProjectionInfo("+proj=utm +zone=59 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone59S = new ProjectionInfo("+proj=utm +zone=59 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone5N = new ProjectionInfo("+proj=utm +zone=5 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone5S = new ProjectionInfo("+proj=utm +zone=5 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone60N = new ProjectionInfo("+proj=utm +zone=60 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone60S = new ProjectionInfo("+proj=utm +zone=60 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone6N = new ProjectionInfo("+proj=utm +zone=6 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone6S = new ProjectionInfo("+proj=utm +zone=6 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone7N = new ProjectionInfo("+proj=utm +zone=7 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone7S = new ProjectionInfo("+proj=utm +zone=7 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone8N = new ProjectionInfo("+proj=utm +zone=8 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone8S = new ProjectionInfo("+proj=utm +zone=8 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone9N = new ProjectionInfo("+proj=utm +zone=9 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984UTMZone9S = new ProjectionInfo("+proj=utm +zone=9 +south +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591