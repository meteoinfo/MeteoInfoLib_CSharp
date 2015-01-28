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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:38:49 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591

namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// GaussKrugerPulkovo1995
    /// </summary>
    public class GaussKrugerPulkovo1995 : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Pulkovo19953DegreeGKCM102E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM105E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM108E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM111E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM114E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM117E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM120E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM123E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM126E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM129E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM132E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM135E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM138E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM141E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM144E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM147E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM150E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM153E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM156E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM159E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM162E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM165E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM168E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM168W;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM171E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM171W;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM174E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM174W;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM177E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM177W;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM180E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM21E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM24E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM27E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM30E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM33E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM36E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM39E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM42E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM45E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM48E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM51E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM54E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM57E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM60E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM63E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM66E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM69E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM72E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM75E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM78E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM81E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM84E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM87E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM90E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM93E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM96E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKCM99E;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone10;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone11;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone12;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone13;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone14;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone15;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone16;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone17;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone18;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone19;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone20;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone21;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone22;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone23;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone24;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone25;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone26;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone27;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone28;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone29;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone30;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone31;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone32;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone33;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone34;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone35;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone36;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone37;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone38;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone39;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone40;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone41;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone42;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone43;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone44;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone45;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone46;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone47;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone48;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone49;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone50;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone51;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone52;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone53;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone54;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone55;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone56;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone57;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone58;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone59;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone60;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone61;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone62;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone63;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone64;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone7;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone8;
        public readonly ProjectionInfo Pulkovo19953DegreeGKZone9;
        public readonly ProjectionInfo Pulkovo1995GKZone10;
        public readonly ProjectionInfo Pulkovo1995GKZone10N;
        public readonly ProjectionInfo Pulkovo1995GKZone11;
        public readonly ProjectionInfo Pulkovo1995GKZone11N;
        public readonly ProjectionInfo Pulkovo1995GKZone12;
        public readonly ProjectionInfo Pulkovo1995GKZone12N;
        public readonly ProjectionInfo Pulkovo1995GKZone13;
        public readonly ProjectionInfo Pulkovo1995GKZone13N;
        public readonly ProjectionInfo Pulkovo1995GKZone14;
        public readonly ProjectionInfo Pulkovo1995GKZone14N;
        public readonly ProjectionInfo Pulkovo1995GKZone15;
        public readonly ProjectionInfo Pulkovo1995GKZone15N;
        public readonly ProjectionInfo Pulkovo1995GKZone16;
        public readonly ProjectionInfo Pulkovo1995GKZone16N;
        public readonly ProjectionInfo Pulkovo1995GKZone17;
        public readonly ProjectionInfo Pulkovo1995GKZone17N;
        public readonly ProjectionInfo Pulkovo1995GKZone18;
        public readonly ProjectionInfo Pulkovo1995GKZone18N;
        public readonly ProjectionInfo Pulkovo1995GKZone19;
        public readonly ProjectionInfo Pulkovo1995GKZone19N;
        public readonly ProjectionInfo Pulkovo1995GKZone2;
        public readonly ProjectionInfo Pulkovo1995GKZone20;
        public readonly ProjectionInfo Pulkovo1995GKZone20N;
        public readonly ProjectionInfo Pulkovo1995GKZone21;
        public readonly ProjectionInfo Pulkovo1995GKZone21N;
        public readonly ProjectionInfo Pulkovo1995GKZone22;
        public readonly ProjectionInfo Pulkovo1995GKZone22N;
        public readonly ProjectionInfo Pulkovo1995GKZone23;
        public readonly ProjectionInfo Pulkovo1995GKZone23N;
        public readonly ProjectionInfo Pulkovo1995GKZone24;
        public readonly ProjectionInfo Pulkovo1995GKZone24N;
        public readonly ProjectionInfo Pulkovo1995GKZone25;
        public readonly ProjectionInfo Pulkovo1995GKZone25N;
        public readonly ProjectionInfo Pulkovo1995GKZone26;
        public readonly ProjectionInfo Pulkovo1995GKZone26N;
        public readonly ProjectionInfo Pulkovo1995GKZone27;
        public readonly ProjectionInfo Pulkovo1995GKZone27N;
        public readonly ProjectionInfo Pulkovo1995GKZone28;
        public readonly ProjectionInfo Pulkovo1995GKZone28N;
        public readonly ProjectionInfo Pulkovo1995GKZone29;
        public readonly ProjectionInfo Pulkovo1995GKZone29N;
        public readonly ProjectionInfo Pulkovo1995GKZone2N;
        public readonly ProjectionInfo Pulkovo1995GKZone3;
        public readonly ProjectionInfo Pulkovo1995GKZone30;
        public readonly ProjectionInfo Pulkovo1995GKZone30N;
        public readonly ProjectionInfo Pulkovo1995GKZone31;
        public readonly ProjectionInfo Pulkovo1995GKZone31N;
        public readonly ProjectionInfo Pulkovo1995GKZone32;
        public readonly ProjectionInfo Pulkovo1995GKZone32N;
        public readonly ProjectionInfo Pulkovo1995GKZone3N;
        public readonly ProjectionInfo Pulkovo1995GKZone4;
        public readonly ProjectionInfo Pulkovo1995GKZone4N;
        public readonly ProjectionInfo Pulkovo1995GKZone5;
        public readonly ProjectionInfo Pulkovo1995GKZone5N;
        public readonly ProjectionInfo Pulkovo1995GKZone6;
        public readonly ProjectionInfo Pulkovo1995GKZone6N;
        public readonly ProjectionInfo Pulkovo1995GKZone7;
        public readonly ProjectionInfo Pulkovo1995GKZone7N;
        public readonly ProjectionInfo Pulkovo1995GKZone8;
        public readonly ProjectionInfo Pulkovo1995GKZone8N;
        public readonly ProjectionInfo Pulkovo1995GKZone9;
        public readonly ProjectionInfo Pulkovo1995GKZone9N;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GaussKrugerPulkovo1995
        /// </summary>
        public GaussKrugerPulkovo1995()
        {
            Pulkovo19953DegreeGKCM102E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=102 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM105E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM108E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=108 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM111E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM114E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=114 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM117E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=117 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM120E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=120 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM123E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=123 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM126E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=126 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM129E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=129 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM132E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=132 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM135E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=135 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM138E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=138 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM141E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=141 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM144E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=144 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM147E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=147 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM150E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=150 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM153E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=153 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM156E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=156 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM159E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=159 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM162E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=162 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM165E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=165 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM168E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=168 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM168W = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-168 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM171E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=171 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM171W = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-171 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM174E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=174 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM174W = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-174 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM177E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=177 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM177W = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-177 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM180E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=180 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM21E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM24E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM27E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=27 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM30E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=30 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM33E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=33 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM36E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=36 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM39E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=39 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM42E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=42 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM45E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=45 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM48E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=48 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM51E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=51 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM54E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=54 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM57E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=57 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM60E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=60 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM63E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=63 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM66E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=66 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM69E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=69 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM72E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=72 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM75E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=75 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM78E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=78 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM81E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=81 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM84E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=84 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM87E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=87 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM90E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=90 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM93E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=93 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM96E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=96 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKCM99E = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=99 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone10 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=30 +k=1.000000 +x_0=10500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone11 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=33 +k=1.000000 +x_0=11500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone12 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=36 +k=1.000000 +x_0=12500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone13 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=39 +k=1.000000 +x_0=13500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone14 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=42 +k=1.000000 +x_0=14500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone15 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=45 +k=1.000000 +x_0=15500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone16 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=48 +k=1.000000 +x_0=16500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone17 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=51 +k=1.000000 +x_0=17500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone18 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=54 +k=1.000000 +x_0=18500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone19 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=57 +k=1.000000 +x_0=19500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone20 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=60 +k=1.000000 +x_0=20500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone21 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=63 +k=1.000000 +x_0=21500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone22 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=66 +k=1.000000 +x_0=22500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone23 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=69 +k=1.000000 +x_0=23500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone24 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=72 +k=1.000000 +x_0=24500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone25 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=75 +k=1.000000 +x_0=25500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone26 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=78 +k=1.000000 +x_0=26500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone27 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=81 +k=1.000000 +x_0=27500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone28 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=84 +k=1.000000 +x_0=28500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone29 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=87 +k=1.000000 +x_0=29500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone30 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=90 +k=1.000000 +x_0=30500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone31 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=93 +k=1.000000 +x_0=31500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone32 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=96 +k=1.000000 +x_0=32500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone33 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=99 +k=1.000000 +x_0=33500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone34 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=102 +k=1.000000 +x_0=34500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone35 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=35500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone36 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=108 +k=1.000000 +x_0=36500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone37 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=37500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone38 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=114 +k=1.000000 +x_0=38500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone39 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=117 +k=1.000000 +x_0=39500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone40 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=120 +k=1.000000 +x_0=40500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone41 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=123 +k=1.000000 +x_0=41500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone42 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=126 +k=1.000000 +x_0=42500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone43 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=129 +k=1.000000 +x_0=43500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone44 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=132 +k=1.000000 +x_0=44500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone45 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=135 +k=1.000000 +x_0=45500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone46 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=138 +k=1.000000 +x_0=46500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone47 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=141 +k=1.000000 +x_0=47500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone48 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=144 +k=1.000000 +x_0=48500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone49 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=147 +k=1.000000 +x_0=49500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone50 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=150 +k=1.000000 +x_0=50500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone51 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=153 +k=1.000000 +x_0=51500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone52 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=156 +k=1.000000 +x_0=52500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone53 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=159 +k=1.000000 +x_0=53500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone54 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=162 +k=1.000000 +x_0=54500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone55 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=165 +k=1.000000 +x_0=55500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone56 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=168 +k=1.000000 +x_0=56500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone57 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=171 +k=1.000000 +x_0=57500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone58 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=174 +k=1.000000 +x_0=58500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone59 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=177 +k=1.000000 +x_0=59500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone60 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=180 +k=1.000000 +x_0=60500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone61 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-177 +k=1.000000 +x_0=61500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone62 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-174 +k=1.000000 +x_0=62500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone63 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-171 +k=1.000000 +x_0=63500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone64 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-168 +k=1.000000 +x_0=64500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone7 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21 +k=1.000000 +x_0=7500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone8 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=1.000000 +x_0=8500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo19953DegreeGKZone9 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=27 +k=1.000000 +x_0=9500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone10 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=57 +k=1.000000 +x_0=10500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone10N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=57 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone11 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=63 +k=1.000000 +x_0=11500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone11N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=63 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone12 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=69 +k=1.000000 +x_0=12500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone12N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=69 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone13 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=75 +k=1.000000 +x_0=13500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone13N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=75 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone14 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=81 +k=1.000000 +x_0=14500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone14N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=81 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone15 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=87 +k=1.000000 +x_0=15500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone15N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=87 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone16 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=93 +k=1.000000 +x_0=16500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone16N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=93 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone17 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=99 +k=1.000000 +x_0=17500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone17N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=99 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone18 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=18500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone18N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone19 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=19500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone19N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone2 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=9 +k=1.000000 +x_0=2500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone20 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=117 +k=1.000000 +x_0=20500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone20N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=117 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone21 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=123 +k=1.000000 +x_0=21500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone21N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=123 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone22 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=129 +k=1.000000 +x_0=22500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone22N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=129 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone23 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=135 +k=1.000000 +x_0=23500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone23N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=135 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone24 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=141 +k=1.000000 +x_0=24500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone24N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=141 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone25 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=147 +k=1.000000 +x_0=25500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone25N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=147 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone26 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=153 +k=1.000000 +x_0=26500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone26N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=153 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone27 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=159 +k=1.000000 +x_0=27500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone27N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=159 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone28 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=165 +k=1.000000 +x_0=28500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone28N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=165 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone29 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=171 +k=1.000000 +x_0=29500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone29N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=171 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone2N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=9 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone3 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=1.000000 +x_0=3500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone30 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=177 +k=1.000000 +x_0=30500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone30N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=177 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone31 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-177 +k=1.000000 +x_0=31500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone31N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-177 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone32 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-171 +k=1.000000 +x_0=32500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone32N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-171 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone3N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone4 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21 +k=1.000000 +x_0=4500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone4N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=27 +k=1.000000 +x_0=5500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone5N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=27 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone6 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=33 +k=1.000000 +x_0=6500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone6N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=33 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone7 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=39 +k=1.000000 +x_0=7500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone7N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=39 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone8 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=45 +k=1.000000 +x_0=8500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone8N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=45 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone9 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=51 +k=1.000000 +x_0=9500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1995GKZone9N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=51 +k=1.000000 +x_0=500000 +y_0=0 +ellps=krass +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591