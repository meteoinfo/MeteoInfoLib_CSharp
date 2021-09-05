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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:04:41 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591

namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// StatePlaneOther
    /// </summary>
    public class StatePlaneOther : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AmericanSamoa1962StatePlaneAmericanSamoaFIPS5300;
        public readonly ProjectionInfo NAD1983HARNGuamMapGrid;
        public readonly ProjectionInfo NAD1983HARNUTMZone2S;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganCentralFIPS2112;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganCentralOldFIPS2102;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganEastOldFIPS2101;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganNorthFIPS2111;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganSouthFIPS2113;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganWestOldFIPS2103;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii1FIPS5101;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii2FIPS5102;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii3FIPS5103;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii4FIPS5104;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii5FIPS5105;
        public readonly ProjectionInfo PuertoRicoStatePlanePuertoRicoFIPS5201;
        public readonly ProjectionInfo PuertoRicoStatePlaneVirginIslandsStCroixFIPS5202;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StatePlaneOther
        /// </summary>
        public StatePlaneOther()
        {
            AmericanSamoa1962StatePlaneAmericanSamoaFIPS5300 = new ProjectionInfo("+proj=lcc +lat_1=-14.26666666666667 +lat_0=-14.26666666666667 +lon_0=-170 +k_0=1 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNGuamMapGrid = new ProjectionInfo("+proj=tmerc +lat_0=13.5 +lon_0=144.75 +k=1.000000 +x_0=100000 +y_0=200000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNUTMZone2S = new ProjectionInfo("+proj=utm +zone=2 +south +ellps=GRS80 +units=m +no_defs ");
            NADMichiganStatePlaneMichiganCentralFIPS2112 = new ProjectionInfo("+proj=lcc +lat_1=44.18333333333333 +lat_2=45.7 +lat_0=43.31666666666667 +lon_0=-84.33333333333333 +x_0=609601.2192024385 +y_0=0 +a=6378450.047 +b=6356826.620025999 +to_meter=0.3048006096012192 +no_defs ");
            NADMichiganStatePlaneMichiganCentralOldFIPS2102 = new ProjectionInfo("+proj=tmerc +lat_0=41.5 +lon_0=-85.75 +k=0.999909 +x_0=152400.3048006096 +y_0=0 +a=6378450.047 +b=6356826.620025999 +to_meter=0.3048006096012192 +no_defs ");
            NADMichiganStatePlaneMichiganEastOldFIPS2101 = new ProjectionInfo("+proj=tmerc +lat_0=41.5 +lon_0=-83.66666666666667 +k=0.999943 +x_0=152400.3048006096 +y_0=0 +a=6378450.047 +b=6356826.620025999 +to_meter=0.3048006096012192 +no_defs ");
            NADMichiganStatePlaneMichiganNorthFIPS2111 = new ProjectionInfo("+proj=lcc +lat_1=45.48333333333333 +lat_2=47.08333333333334 +lat_0=44.78333333333333 +lon_0=-87 +x_0=609601.2192024385 +y_0=0 +a=6378450.047 +b=6356826.620025999 +to_meter=0.3048006096012192 +no_defs ");
            NADMichiganStatePlaneMichiganSouthFIPS2113 = new ProjectionInfo("+proj=lcc +lat_1=42.1 +lat_2=43.66666666666666 +lat_0=41.5 +lon_0=-84.33333333333333 +x_0=609601.2192024385 +y_0=0 +a=6378450.047 +b=6356826.620025999 +to_meter=0.3048006096012192 +no_defs ");
            NADMichiganStatePlaneMichiganWestOldFIPS2103 = new ProjectionInfo("+proj=tmerc +lat_0=41.5 +lon_0=-88.75 +k=0.999909 +x_0=152400.3048006096 +y_0=0 +a=6378450.047 +b=6356826.620025999 +to_meter=0.3048006096012192 +no_defs ");
            OldHawaiianStatePlaneHawaii1FIPS5101 = new ProjectionInfo("+proj=tmerc +lat_0=18.83333333333333 +lon_0=-155.5 +k=0.999967 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            OldHawaiianStatePlaneHawaii2FIPS5102 = new ProjectionInfo("+proj=tmerc +lat_0=20.33333333333333 +lon_0=-156.6666666666667 +k=0.999967 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            OldHawaiianStatePlaneHawaii3FIPS5103 = new ProjectionInfo("+proj=tmerc +lat_0=21.16666666666667 +lon_0=-158 +k=0.999990 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            OldHawaiianStatePlaneHawaii4FIPS5104 = new ProjectionInfo("+proj=tmerc +lat_0=21.83333333333333 +lon_0=-159.5 +k=0.999990 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            OldHawaiianStatePlaneHawaii5FIPS5105 = new ProjectionInfo("+proj=tmerc +lat_0=21.66666666666667 +lon_0=-160.1666666666667 +k=1.000000 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            PuertoRicoStatePlanePuertoRicoFIPS5201 = new ProjectionInfo("+proj=lcc +lat_1=18.03333333333333 +lat_2=18.43333333333333 +lat_0=17.83333333333333 +lon_0=-66.43333333333334 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            PuertoRicoStatePlaneVirginIslandsStCroixFIPS5202 = new ProjectionInfo("+proj=lcc +lat_1=18.03333333333333 +lat_2=18.43333333333333 +lat_0=17.83333333333333 +lon_0=-66.43333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591