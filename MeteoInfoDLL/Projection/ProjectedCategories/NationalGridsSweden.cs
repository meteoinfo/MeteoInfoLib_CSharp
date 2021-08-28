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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:54:34 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// NationalGridsSweden
    /// </summary>
    public class NationalGridsSweden : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo RT380gon;
        public readonly ProjectionInfo RT3825gonO;
        public readonly ProjectionInfo RT3825gonV;
        public readonly ProjectionInfo RT385gonO;
        public readonly ProjectionInfo RT385gonV;
        public readonly ProjectionInfo RT3875gonV;
        public readonly ProjectionInfo RT900gon;
        public readonly ProjectionInfo RT9025gonO;
        public readonly ProjectionInfo RT9025gonV;
        public readonly ProjectionInfo RT905gonO;
        public readonly ProjectionInfo RT905gonV;
        public readonly ProjectionInfo RT9075gonV;
        public readonly ProjectionInfo SWEREF991200;
        public readonly ProjectionInfo SWEREF991330;
        public readonly ProjectionInfo SWEREF991415;
        public readonly ProjectionInfo SWEREF991500;
        public readonly ProjectionInfo SWEREF991545;
        public readonly ProjectionInfo SWEREF991630;
        public readonly ProjectionInfo SWEREF991715;
        public readonly ProjectionInfo SWEREF991800;
        public readonly ProjectionInfo SWEREF991845;
        public readonly ProjectionInfo SWEREF992015;
        public readonly ProjectionInfo SWEREF992145;
        public readonly ProjectionInfo SWEREF992315;
        public readonly ProjectionInfo SWEREF99TM;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NationalGridsSweden
        /// </summary>
        public NationalGridsSweden()
        {
            RT380gon = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=18.05827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT3825gonO = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=20.30827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT3825gonV = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15.80827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT385gonO = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=22.55827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT385gonV = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=13.55827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT3875gonV = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=11.30827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT900gon = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=18.05827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT9025gonO = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=20.30827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT9025gonV = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15.80827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT905gonO = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=22.55827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT905gonV = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=13.55827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            RT9075gonV = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=11.30827777777778 +k=1.000000 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs ");
            SWEREF991200 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=12 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991330 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=13.5 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991415 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=14.25 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991500 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991545 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=15.75 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991630 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=16.5 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991715 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=17.25 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991800 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=18 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF991845 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=18.75 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF992015 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=20.25 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF992145 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21.75 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF992315 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=23.25 +k=1.000000 +x_0=150000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            SWEREF99TM = new ProjectionInfo("+proj=utm +zone=33 +ellps=GRS80 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591