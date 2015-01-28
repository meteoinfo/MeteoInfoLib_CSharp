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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:36:17 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// GaussKrugerOther
    /// </summary>
    public class GaussKrugerOther : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Albanian1987GKZone4;
        public readonly ProjectionInfo ED19503DegreeGKZone10;
        public readonly ProjectionInfo ED19503DegreeGKZone11;
        public readonly ProjectionInfo ED19503DegreeGKZone12;
        public readonly ProjectionInfo ED19503DegreeGKZone13;
        public readonly ProjectionInfo ED19503DegreeGKZone14;
        public readonly ProjectionInfo ED19503DegreeGKZone15;
        public readonly ProjectionInfo ED19503DegreeGKZone9;
        public readonly ProjectionInfo Hanoi1972GKZone18;
        public readonly ProjectionInfo Hanoi1972GKZone19;
        public readonly ProjectionInfo Pulkovo1942Adj19833DegreeGKZone3;
        public readonly ProjectionInfo Pulkovo1942Adj19833DegreeGKZone4;
        public readonly ProjectionInfo Pulkovo1942Adj19833DegreeGKZone5;
        public readonly ProjectionInfo SouthYemenGKZone8;
        public readonly ProjectionInfo SouthYemenGKZone9;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GaussKrugerOther
        /// </summary>
        public GaussKrugerOther()
        {
            Albanian1987GKZone4 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21 +k=1.000000 +x_0=4500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            ED19503DegreeGKZone10 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=30 +k=1.000000 +x_0=10500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED19503DegreeGKZone11 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=33 +k=1.000000 +x_0=11500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED19503DegreeGKZone12 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=36 +k=1.000000 +x_0=12500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED19503DegreeGKZone13 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=39 +k=1.000000 +x_0=13500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED19503DegreeGKZone14 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=42 +k=1.000000 +x_0=14500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED19503DegreeGKZone15 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=45 +k=1.000000 +x_0=15500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            ED19503DegreeGKZone9 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=27 +k=1.000000 +x_0=9500000 +y_0=0 +ellps=intl +units=m +no_defs ");
            Hanoi1972GKZone18 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=105 +k=1.000000 +x_0=18500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Hanoi1972GKZone19 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=111 +k=1.000000 +x_0=19500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1942Adj19833DegreeGKZone3 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=9 +k=1.000000 +x_0=3500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1942Adj19833DegreeGKZone4 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=12 +k=1.000000 +x_0=4500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            Pulkovo1942Adj19833DegreeGKZone5 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=1.000000 +x_0=5500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            SouthYemenGKZone8 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=45 +k=1.000000 +x_0=8500000 +y_0=0 +ellps=krass +units=m +no_defs ");
            SouthYemenGKZone9 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=51 +k=1.000000 +x_0=9500000 +y_0=0 +ellps=krass +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591