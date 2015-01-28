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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:35:25 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// GaussKrugerBeijing1954
    /// </summary>
    public class GaussKrugerBeijing1954 : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Beijing19543DegreeGKCM102E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM105E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM108E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM111E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM114E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM117E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM120E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM123E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM126E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM129E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM132E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM135E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM75E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM78E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM81E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM84E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM87E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM90E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM93E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM96E;
        public readonly ProjectionInfo Beijing19543DegreeGKCM99E;
        public readonly ProjectionInfo Beijing19543DegreeGKZone25;
        public readonly ProjectionInfo Beijing19543DegreeGKZone26;
        public readonly ProjectionInfo Beijing19543DegreeGKZone27;
        public readonly ProjectionInfo Beijing19543DegreeGKZone28;
        public readonly ProjectionInfo Beijing19543DegreeGKZone29;
        public readonly ProjectionInfo Beijing19543DegreeGKZone30;
        public readonly ProjectionInfo Beijing19543DegreeGKZone31;
        public readonly ProjectionInfo Beijing19543DegreeGKZone32;
        public readonly ProjectionInfo Beijing19543DegreeGKZone33;
        public readonly ProjectionInfo Beijing19543DegreeGKZone34;
        public readonly ProjectionInfo Beijing19543DegreeGKZone35;
        public readonly ProjectionInfo Beijing19543DegreeGKZone36;
        public readonly ProjectionInfo Beijing19543DegreeGKZone37;
        public readonly ProjectionInfo Beijing19543DegreeGKZone38;
        public readonly ProjectionInfo Beijing19543DegreeGKZone39;
        public readonly ProjectionInfo Beijing19543DegreeGKZone40;
        public readonly ProjectionInfo Beijing19543DegreeGKZone41;
        public readonly ProjectionInfo Beijing19543DegreeGKZone42;
        public readonly ProjectionInfo Beijing19543DegreeGKZone43;
        public readonly ProjectionInfo Beijing19543DegreeGKZone44;
        public readonly ProjectionInfo Beijing19543DegreeGKZone45;
        public readonly ProjectionInfo Beijing1954GKZone13;
        public readonly ProjectionInfo Beijing1954GKZone13N;
        public readonly ProjectionInfo Beijing1954GKZone14;
        public readonly ProjectionInfo Beijing1954GKZone14N;
        public readonly ProjectionInfo Beijing1954GKZone15;
        public readonly ProjectionInfo Beijing1954GKZone15N;
        public readonly ProjectionInfo Beijing1954GKZone16;
        public readonly ProjectionInfo Beijing1954GKZone16N;
        public readonly ProjectionInfo Beijing1954GKZone17;
        public readonly ProjectionInfo Beijing1954GKZone17N;
        public readonly ProjectionInfo Beijing1954GKZone18;
        public readonly ProjectionInfo Beijing1954GKZone18N;
        public readonly ProjectionInfo Beijing1954GKZone19;
        public readonly ProjectionInfo Beijing1954GKZone19N;
        public readonly ProjectionInfo Beijing1954GKZone20;
        public readonly ProjectionInfo Beijing1954GKZone20N;
        public readonly ProjectionInfo Beijing1954GKZone21;
        public readonly ProjectionInfo Beijing1954GKZone21N;
        public readonly ProjectionInfo Beijing1954GKZone22;
        public readonly ProjectionInfo Beijing1954GKZone22N;
        public readonly ProjectionInfo Beijing1954GKZone23;
        public readonly ProjectionInfo Beijing1954GKZone23N;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GaussKrugerBeijing1954
        /// </summary>
        public GaussKrugerBeijing1954()
        {
            Beijing19543DegreeGKCM102E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=102 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM105E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM108E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=108 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM111E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM114E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=114 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM117E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=117 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM120E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=120 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM123E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=123 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM126E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=126 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM129E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=129 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM132E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=132 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM135E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=135 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM75E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=75 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM78E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=78 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM81E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=81 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM84E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=84 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM87E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=87 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM90E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=90 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM93E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=93 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM96E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=96 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKCM99E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=99 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone25 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=75 +k=1.000000 +x_0=25500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone26 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=78 +k=1.000000 +x_0=26500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone27 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=81 +k=1.000000 +x_0=27500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone28 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=84 +k=1.000000 +x_0=28500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone29 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=87 +k=1.000000 +x_0=29500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone30 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=90 +k=1.000000 +x_0=30500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone31 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=93 +k=1.000000 +x_0=31500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone32 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=96 +k=1.000000 +x_0=32500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone33 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=99 +k=1.000000 +x_0=33500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone34 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=102 +k=1.000000 +x_0=34500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone35 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=35500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone36 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=108 +k=1.000000 +x_0=36500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone37 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=37500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone38 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=114 +k=1.000000 +x_0=38500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone39 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=117 +k=1.000000 +x_0=39500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone40 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=120 +k=1.000000 +x_0=40500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone41 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=123 +k=1.000000 +x_0=41500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone42 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=126 +k=1.000000 +x_0=42500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone43 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=129 +k=1.000000 +x_0=43500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone44 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=132 +k=1.000000 +x_0=44500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing19543DegreeGKZone45 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=135 +k=1.000000 +x_0=45500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone13 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=75 +k=1.000000 +x_0=13500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone13N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=75 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone14 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=81 +k=1.000000 +x_0=14500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone14N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=81 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone15 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=87 +k=1.000000 +x_0=15500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone15N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=87 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone16 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=93 +k=1.000000 +x_0=16500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone16N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=93 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone17 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=99 +k=1.000000 +x_0=17500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone17N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=99 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone18 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=18500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone18N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone19 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=19500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone19N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone20 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=117 +k=1.000000 +x_0=20500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone20N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=117 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone21 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=123 +k=1.000000 +x_0=21500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone21N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=123 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone22 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=129 +k=1.000000 +x_0=22500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone22N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=129 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone23 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=135 +k=1.000000 +x_0=23500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Beijing1954GKZone23N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=135 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591