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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:52:35 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// NationalGridsNewZealand
    /// </summary>
    public class NationalGridsNewZealand : CoordinateSystemCategory
    {
        #region Private Variables
        public readonly ProjectionInfo ChathamIslands1979MapGrid;
        public readonly ProjectionInfo NewZealandMapGrid;
        public readonly ProjectionInfo NewZealandNorthIsland;
        public readonly ProjectionInfo NewZealandSouthIsland;
        public readonly ProjectionInfo NZGD1949AmuriCircuit;
        public readonly ProjectionInfo NZGD1949BayofPlentyCircuit;
        public readonly ProjectionInfo NZGD1949BluffCircuit;
        public readonly ProjectionInfo NZGD1949BullerCircuit;
        public readonly ProjectionInfo NZGD1949CollingwoodCircuit;
        public readonly ProjectionInfo NZGD1949GawlerCircuit;
        public readonly ProjectionInfo NZGD1949GreyCircuit;
        public readonly ProjectionInfo NZGD1949HawkesBayCircuit;
        public readonly ProjectionInfo NZGD1949HokitikaCircuit;
        public readonly ProjectionInfo NZGD1949JacksonsBayCircuit;
        public readonly ProjectionInfo NZGD1949KarameaCircuit;
        public readonly ProjectionInfo NZGD1949LindisPeakCircuit;
        public readonly ProjectionInfo NZGD1949MarlboroughCircuit;
        public readonly ProjectionInfo NZGD1949MountEdenCircuit;
        public readonly ProjectionInfo NZGD1949MountNicholasCircuit;
        public readonly ProjectionInfo NZGD1949MountPleasantCircuit;
        public readonly ProjectionInfo NZGD1949MountYorkCircuit;
        public readonly ProjectionInfo NZGD1949NelsonCircuit;
        public readonly ProjectionInfo NZGD1949NorthTaieriCircuit;
        public readonly ProjectionInfo NZGD1949ObservationPointCircuit;
        public readonly ProjectionInfo NZGD1949OkaritoCircuit;
        public readonly ProjectionInfo NZGD1949PovertyBayCircuit;
        public readonly ProjectionInfo NZGD1949TaranakiCircuit;
        public readonly ProjectionInfo NZGD1949TimaruCircuit;
        public readonly ProjectionInfo NZGD1949TuhirangiCircuit;
        public readonly ProjectionInfo NZGD1949UTMZone58S;
        public readonly ProjectionInfo NZGD1949UTMZone59S;
        public readonly ProjectionInfo NZGD1949UTMZone60S;
        public readonly ProjectionInfo NZGD1949WairarapaCircuit;
        public readonly ProjectionInfo NZGD1949WanganuiCircuit;
        public readonly ProjectionInfo NZGD1949WellingtonCircuit;
        public readonly ProjectionInfo NZGD2000AmuriCircuit;
        public readonly ProjectionInfo NZGD2000BayofPlentyCircuit;
        public readonly ProjectionInfo NZGD2000BluffCircuit;
        public readonly ProjectionInfo NZGD2000BullerCircuit;
        public readonly ProjectionInfo NZGD2000ChathamIslandCircuit;
        public readonly ProjectionInfo NZGD2000CollingwoodCircuit;
        public readonly ProjectionInfo NZGD2000GawlerCircuit;
        public readonly ProjectionInfo NZGD2000GreyCircuit;
        public readonly ProjectionInfo NZGD2000HawkesBayCircuit;
        public readonly ProjectionInfo NZGD2000HokitikaCircuit;
        public readonly ProjectionInfo NZGD2000JacksonsBayCircuit;
        public readonly ProjectionInfo NZGD2000KarameaCircuit;
        public readonly ProjectionInfo NZGD2000LindisPeakCircuit;
        public readonly ProjectionInfo NZGD2000MarlboroughCircuit;
        public readonly ProjectionInfo NZGD2000MountEdenCircuit;
        public readonly ProjectionInfo NZGD2000MountNicholasCircuit;
        public readonly ProjectionInfo NZGD2000MountPleasantCircuit;
        public readonly ProjectionInfo NZGD2000MountYorkCircuit;
        public readonly ProjectionInfo NZGD2000NelsonCircuit;
        public readonly ProjectionInfo NZGD2000NewZealandTransverseMercator;
        public readonly ProjectionInfo NZGD2000NorthTaieriCircuit;
        public readonly ProjectionInfo NZGD2000ObservationPointCircuit;
        public readonly ProjectionInfo NZGD2000OkaritoCircuit;
        public readonly ProjectionInfo NZGD2000PovertyBayCircuit;
        public readonly ProjectionInfo NZGD2000TaranakiCircuit;
        public readonly ProjectionInfo NZGD2000TimaruCircuit;
        public readonly ProjectionInfo NZGD2000TuhirangiCircuit;
        public readonly ProjectionInfo NZGD2000UTMZone58S;
        public readonly ProjectionInfo NZGD2000UTMZone59S;
        public readonly ProjectionInfo NZGD2000UTMZone60S;
        public readonly ProjectionInfo NZGD2000WairarapaCircuit;
        public readonly ProjectionInfo NZGD2000WanganuiCircuit;
        public readonly ProjectionInfo NZGD2000WellingtonCircuit;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NationalGridsNewZealand
        /// </summary>
        public NationalGridsNewZealand()
        {
            ChathamIslands1979MapGrid = new ProjectionInfo("+proj=tmerc +lat_0=-44 +lon_0=-176.5 +k=0.999600 +x_0=350000 +y_0=650000 +ellps=intl +units=m +no_defs ");
            NewZealandMapGrid = new ProjectionInfo("+proj=nzmg +lat_0=-41 +lon_0=173 +x_0=2510000 +y_0=6023150 +ellps=intl +units=m +no_defs ");
            NewZealandNorthIsland = new ProjectionInfo("+proj=tmerc +lat_0=-39 +lon_0=175.5 +k=1.000000 +x_0=274319.5243848086 +y_0=365759.3658464114 +ellps=intl +to_meter=0.9143984146160287 +no_defs ");
            NewZealandSouthIsland = new ProjectionInfo("+proj=tmerc +lat_0=-44 +lon_0=171.5 +k=1.000000 +x_0=457199.2073080143 +y_0=457199.2073080143 +ellps=intl +to_meter=0.9143984146160287 +no_defs ");
            NZGD1949AmuriCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-42.68911658333333 +lon_0=173.0101333888889 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949BayofPlentyCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-37.76124980555556 +lon_0=176.46619725 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949BluffCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-46.60000961111111 +lon_0=168.342872 +k=1.000000 +x_0=300002.66 +y_0=699999.58 +ellps=intl +units=m +no_defs ");
            NZGD1949BullerCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-41.81080286111111 +lon_0=171.5812600555556 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949CollingwoodCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-40.71475905555556 +lon_0=172.6720465 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949GawlerCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-43.74871155555556 +lon_0=171.3607484722222 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949GreyCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-42.33369427777778 +lon_0=171.5497713055556 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949HawkesBayCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-39.65092930555556 +lon_0=176.6736805277778 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949HokitikaCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-42.88632236111111 +lon_0=170.9799935 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949JacksonsBayCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-43.97780288888889 +lon_0=168.606267 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949KarameaCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-41.28991152777778 +lon_0=172.1090281944444 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949LindisPeakCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-44.73526797222222 +lon_0=169.4677550833333 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949MarlboroughCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-41.54448666666666 +lon_0=173.8020741111111 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949MountEdenCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-36.87986527777778 +lon_0=174.7643393611111 +k=0.999900 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949MountNicholasCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-45.13290258333333 +lon_0=168.3986411944444 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949MountPleasantCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-43.59063758333333 +lon_0=172.7271935833333 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949MountYorkCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-45.56372616666666 +lon_0=167.7388617777778 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949NelsonCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-41.27454472222222 +lon_0=173.2993168055555 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949NorthTaieriCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-45.86151336111111 +lon_0=170.2825891111111 +k=0.999960 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949ObservationPointCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-45.81619661111111 +lon_0=170.6285951666667 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949OkaritoCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-43.11012813888889 +lon_0=170.2609258333333 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949PovertyBayCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-38.62470277777778 +lon_0=177.8856362777778 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949TaranakiCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-39.13575830555556 +lon_0=174.22801175 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949TimaruCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-44.40222036111111 +lon_0=171.0572508333333 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949TuhirangiCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-39.51247038888889 +lon_0=175.6400368055556 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949UTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=intl +units=m +no_defs ");
            NZGD1949UTMZone59S = new ProjectionInfo("+proj=utm +zone=59 +south +ellps=intl +units=m +no_defs ");
            NZGD1949UTMZone60S = new ProjectionInfo("+proj=utm +zone=60 +south +ellps=intl +units=m +no_defs ");
            NZGD1949WairarapaCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-40.92553263888889 +lon_0=175.6473496666667 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949WanganuiCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-40.24194713888889 +lon_0=175.4880996111111 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD1949WellingtonCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-41.30131963888888 +lon_0=174.7766231111111 +k=1.000000 +x_0=300000 +y_0=700000 +ellps=intl +units=m +no_defs ");
            NZGD2000AmuriCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-42.68888888888888 +lon_0=173.01 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000BayofPlentyCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-37.76111111111111 +lon_0=176.4661111111111 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000BluffCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-46.6 +lon_0=168.3427777777778 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000BullerCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-41.81055555555555 +lon_0=171.5811111111111 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000ChathamIslandCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-44 +lon_0=-176.5 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000CollingwoodCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-40.71472222222223 +lon_0=172.6719444444444 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000GawlerCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-43.74861111111111 +lon_0=171.3605555555555 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000GreyCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-42.33361111111111 +lon_0=171.5497222222222 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000HawkesBayCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-39.65083333333333 +lon_0=176.6736111111111 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000HokitikaCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-42.88611111111111 +lon_0=170.9797222222222 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000JacksonsBayCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-43.97777777777778 +lon_0=168.6061111111111 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000KarameaCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-41.28972222222222 +lon_0=172.1088888888889 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000LindisPeakCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-44.735 +lon_0=169.4675 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000MarlboroughCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-41.54444444444444 +lon_0=173.8019444444444 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000MountEdenCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-36.87972222222222 +lon_0=174.7641666666667 +k=0.999900 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000MountNicholasCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-45.13277777777778 +lon_0=168.3986111111111 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000MountPleasantCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-43.59055555555556 +lon_0=172.7269444444445 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000MountYorkCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-45.56361111111111 +lon_0=167.7386111111111 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000NelsonCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-41.27444444444444 +lon_0=173.2991666666667 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000NewZealandTransverseMercator = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=173 +k=0.999600 +x_0=1600000 +y_0=10000000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000NorthTaieriCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-45.86138888888889 +lon_0=170.2825 +k=0.999960 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000ObservationPointCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-45.81611111111111 +lon_0=170.6283333333333 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000OkaritoCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-43.11 +lon_0=170.2608333333333 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000PovertyBayCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-38.62444444444444 +lon_0=177.8855555555556 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000TaranakiCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-39.13555555555556 +lon_0=174.2277777777778 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000TimaruCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-44.40194444444445 +lon_0=171.0572222222222 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000TuhirangiCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-39.51222222222222 +lon_0=175.64 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000UTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=GRS80 +units=m +no_defs ");
            NZGD2000UTMZone59S = new ProjectionInfo("+proj=utm +zone=59 +south +ellps=GRS80 +units=m +no_defs ");
            NZGD2000UTMZone60S = new ProjectionInfo("+proj=utm +zone=60 +south +ellps=GRS80 +units=m +no_defs ");
            NZGD2000WairarapaCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-40.92527777777777 +lon_0=175.6472222222222 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000WanganuiCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-40.24194444444444 +lon_0=175.4880555555555 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");
            NZGD2000WellingtonCircuit = new ProjectionInfo("+proj=tmerc +lat_0=-41.3011111111111 +lon_0=174.7763888888889 +k=1.000000 +x_0=400000 +y_0=800000 +ellps=GRS80 +units=m +no_defs ");


        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591