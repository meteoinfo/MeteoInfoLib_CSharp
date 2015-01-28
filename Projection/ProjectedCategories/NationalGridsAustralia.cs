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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:46:07 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// NatGridsAustralia
    /// </summary>
    public class NationalGridsAustralia : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AGD1966ACTGridAGCZone;
        public readonly ProjectionInfo AGD1966AMGZone48;
        public readonly ProjectionInfo AGD1966AMGZone49;
        public readonly ProjectionInfo AGD1966AMGZone50;
        public readonly ProjectionInfo AGD1966AMGZone51;
        public readonly ProjectionInfo AGD1966AMGZone52;
        public readonly ProjectionInfo AGD1966AMGZone53;
        public readonly ProjectionInfo AGD1966AMGZone54;
        public readonly ProjectionInfo AGD1966AMGZone55;
        public readonly ProjectionInfo AGD1966AMGZone56;
        public readonly ProjectionInfo AGD1966AMGZone57;
        public readonly ProjectionInfo AGD1966AMGZone58;
        public readonly ProjectionInfo AGD1966ISG542;
        public readonly ProjectionInfo AGD1966ISG543;
        public readonly ProjectionInfo AGD1966ISG551;
        public readonly ProjectionInfo AGD1966ISG552;
        public readonly ProjectionInfo AGD1966ISG553;
        public readonly ProjectionInfo AGD1966ISG561;
        public readonly ProjectionInfo AGD1966ISG562;
        public readonly ProjectionInfo AGD1966ISG563;
        public readonly ProjectionInfo AGD1966VICGRID;
        public readonly ProjectionInfo AGD1984AMGZone48;
        public readonly ProjectionInfo AGD1984AMGZone49;
        public readonly ProjectionInfo AGD1984AMGZone50;
        public readonly ProjectionInfo AGD1984AMGZone51;
        public readonly ProjectionInfo AGD1984AMGZone52;
        public readonly ProjectionInfo AGD1984AMGZone53;
        public readonly ProjectionInfo AGD1984AMGZone54;
        public readonly ProjectionInfo AGD1984AMGZone55;
        public readonly ProjectionInfo AGD1984AMGZone56;
        public readonly ProjectionInfo AGD1984AMGZone57;
        public readonly ProjectionInfo AGD1984AMGZone58;
        public readonly ProjectionInfo GDA1994MGAZone48;
        public readonly ProjectionInfo GDA1994MGAZone49;
        public readonly ProjectionInfo GDA1994MGAZone50;
        public readonly ProjectionInfo GDA1994MGAZone51;
        public readonly ProjectionInfo GDA1994MGAZone52;
        public readonly ProjectionInfo GDA1994MGAZone53;
        public readonly ProjectionInfo GDA1994MGAZone54;
        public readonly ProjectionInfo GDA1994MGAZone55;
        public readonly ProjectionInfo GDA1994MGAZone56;
        public readonly ProjectionInfo GDA1994MGAZone57;
        public readonly ProjectionInfo GDA1994MGAZone58;
        public readonly ProjectionInfo GDA1994SouthAustraliaLambert;
        public readonly ProjectionInfo GDA1994VICGRID94;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NatGridsAustralia
        /// </summary>
        public NationalGridsAustralia()
        {
            AGD1966ACTGridAGCZone = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=149.0092948333333 +k=1.000086 +x_0=200000 +y_0=4510193.4939 +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone48 = new ProjectionInfo("+proj=utm +zone=48 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone49 = new ProjectionInfo("+proj=utm +zone=49 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone50 = new ProjectionInfo("+proj=utm +zone=50 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone51 = new ProjectionInfo("+proj=utm +zone=51 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone52 = new ProjectionInfo("+proj=utm +zone=52 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone53 = new ProjectionInfo("+proj=utm +zone=53 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone54 = new ProjectionInfo("+proj=utm +zone=54 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone55 = new ProjectionInfo("+proj=utm +zone=55 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone56 = new ProjectionInfo("+proj=utm +zone=56 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone57 = new ProjectionInfo("+proj=utm +zone=57 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone58 = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG542 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=141 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG543 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=143 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG551 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=145 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG552 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=147 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG553 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=149 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG561 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=151 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG562 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=153 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG563 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=155 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966VICGRID = new ProjectionInfo("+proj=lcc +lat_1=-36 +lat_2=-38 +lat_0=-37 +lon_0=145 +x_0=2500000 +y_0=4500000 +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone48 = new ProjectionInfo("+proj=utm +zone=48 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone49 = new ProjectionInfo("+proj=utm +zone=49 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone50 = new ProjectionInfo("+proj=utm +zone=50 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone51 = new ProjectionInfo("+proj=utm +zone=51 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone52 = new ProjectionInfo("+proj=utm +zone=52 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone53 = new ProjectionInfo("+proj=utm +zone=53 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone54 = new ProjectionInfo("+proj=utm +zone=54 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone55 = new ProjectionInfo("+proj=utm +zone=55 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone56 = new ProjectionInfo("+proj=utm +zone=56 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone57 = new ProjectionInfo("+proj=utm +zone=57 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone58 = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=aust_SA +units=m +no_defs ");
            GDA1994MGAZone48 = new ProjectionInfo("+proj=utm +zone=48 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone49 = new ProjectionInfo("+proj=utm +zone=49 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone50 = new ProjectionInfo("+proj=utm +zone=50 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone51 = new ProjectionInfo("+proj=utm +zone=51 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone52 = new ProjectionInfo("+proj=utm +zone=52 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone53 = new ProjectionInfo("+proj=utm +zone=53 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone54 = new ProjectionInfo("+proj=utm +zone=54 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone55 = new ProjectionInfo("+proj=utm +zone=55 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone56 = new ProjectionInfo("+proj=utm +zone=56 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone57 = new ProjectionInfo("+proj=utm +zone=57 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone58 = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994SouthAustraliaLambert = new ProjectionInfo("+proj=lcc +lat_1=-28 +lat_2=-36 +lat_0=-32 +lon_0=135 +x_0=1000000 +y_0=2000000 +ellps=GRS80 +units=m +no_defs ");
            GDA1994VICGRID94 = new ProjectionInfo("+proj=lcc +lat_1=-36 +lat_2=-38 +lat_0=-37 +lon_0=145 +x_0=2500000 +y_0=2500000 +ellps=GRS80 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591