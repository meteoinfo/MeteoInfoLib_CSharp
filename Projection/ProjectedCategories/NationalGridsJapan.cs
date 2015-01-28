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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:51:42 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// NationalGridsJapan
    /// </summary>
    public class NationalGridsJapan : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo JapanZone1;
        public readonly ProjectionInfo JapanZone10;
        public readonly ProjectionInfo JapanZone11;
        public readonly ProjectionInfo JapanZone12;
        public readonly ProjectionInfo JapanZone13;
        public readonly ProjectionInfo JapanZone14;
        public readonly ProjectionInfo JapanZone15;
        public readonly ProjectionInfo JapanZone16;
        public readonly ProjectionInfo JapanZone17;
        public readonly ProjectionInfo JapanZone18;
        public readonly ProjectionInfo JapanZone19;
        public readonly ProjectionInfo JapanZone2;
        public readonly ProjectionInfo JapanZone3;
        public readonly ProjectionInfo JapanZone4;
        public readonly ProjectionInfo JapanZone5;
        public readonly ProjectionInfo JapanZone6;
        public readonly ProjectionInfo JapanZone7;
        public readonly ProjectionInfo JapanZone8;
        public readonly ProjectionInfo JapanZone9;
        public readonly ProjectionInfo JGD2000JapanZone1;
        public readonly ProjectionInfo JGD2000JapanZone10;
        public readonly ProjectionInfo JGD2000JapanZone11;
        public readonly ProjectionInfo JGD2000JapanZone12;
        public readonly ProjectionInfo JGD2000JapanZone13;
        public readonly ProjectionInfo JGD2000JapanZone14;
        public readonly ProjectionInfo JGD2000JapanZone15;
        public readonly ProjectionInfo JGD2000JapanZone16;
        public readonly ProjectionInfo JGD2000JapanZone17;
        public readonly ProjectionInfo JGD2000JapanZone18;
        public readonly ProjectionInfo JGD2000JapanZone19;
        public readonly ProjectionInfo JGD2000JapanZone2;
        public readonly ProjectionInfo JGD2000JapanZone3;
        public readonly ProjectionInfo JGD2000JapanZone4;
        public readonly ProjectionInfo JGD2000JapanZone5;
        public readonly ProjectionInfo JGD2000JapanZone6;
        public readonly ProjectionInfo JGD2000JapanZone7;
        public readonly ProjectionInfo JGD2000JapanZone8;
        public readonly ProjectionInfo JGD2000JapanZone9;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NationalGridsJapan
        /// </summary>
        public NationalGridsJapan()
        {
            JapanZone1 = new ProjectionInfo("+proj=tmerc +lat_0=33 +lon_0=129.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone10 = new ProjectionInfo("+proj=tmerc +lat_0=40 +lon_0=140.8333333333333 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone11 = new ProjectionInfo("+proj=tmerc +lat_0=44 +lon_0=140.25 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone12 = new ProjectionInfo("+proj=tmerc +lat_0=44 +lon_0=142.25 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone13 = new ProjectionInfo("+proj=tmerc +lat_0=44 +lon_0=144.25 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone14 = new ProjectionInfo("+proj=tmerc +lat_0=26 +lon_0=142 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone15 = new ProjectionInfo("+proj=tmerc +lat_0=26 +lon_0=127.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone16 = new ProjectionInfo("+proj=tmerc +lat_0=26 +lon_0=124 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone17 = new ProjectionInfo("+proj=tmerc +lat_0=26 +lon_0=131 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone18 = new ProjectionInfo("+proj=tmerc +lat_0=20 +lon_0=136 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone19 = new ProjectionInfo("+proj=tmerc +lat_0=26 +lon_0=154 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone2 = new ProjectionInfo("+proj=tmerc +lat_0=33 +lon_0=131 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone3 = new ProjectionInfo("+proj=tmerc +lat_0=36 +lon_0=132.1666666666667 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone4 = new ProjectionInfo("+proj=tmerc +lat_0=33 +lon_0=133.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone5 = new ProjectionInfo("+proj=tmerc +lat_0=36 +lon_0=134.3333333333333 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone6 = new ProjectionInfo("+proj=tmerc +lat_0=36 +lon_0=136 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone7 = new ProjectionInfo("+proj=tmerc +lat_0=36 +lon_0=137.1666666666667 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone8 = new ProjectionInfo("+proj=tmerc +lat_0=36 +lon_0=138.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JapanZone9 = new ProjectionInfo("+proj=tmerc +lat_0=36 +lon_0=139.8333333333333 +k=0.999900 +x_0=0 +y_0=0 +ellps=bessel +units=m +no_defs ");
            JGD2000JapanZone1 = new ProjectionInfo("+proj=tmerc +lat_0=33 +lon_0=129.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone10 = new ProjectionInfo("+proj=tmerc +lat_0=40 +lon_0=140.8333333333333 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone11 = new ProjectionInfo("+proj=tmerc +lat_0=44 +lon_0=140.25 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone12 = new ProjectionInfo("+proj=tmerc +lat_0=44 +lon_0=142.25 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone13 = new ProjectionInfo("+proj=tmerc +lat_0=44 +lon_0=144.25 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone14 = new ProjectionInfo("+proj=tmerc +lat_0=26 +lon_0=142 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone15 = new ProjectionInfo("+proj=tmerc +lat_0=26 +lon_0=127.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone16 = new ProjectionInfo("+proj=tmerc +lat_0=26 +lon_0=124 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone17 = new ProjectionInfo("+proj=tmerc +lat_0=26 +lon_0=131 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone18 = new ProjectionInfo("+proj=tmerc +lat_0=20 +lon_0=136 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone19 = new ProjectionInfo("+proj=tmerc +lat_0=26 +lon_0=154 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone2 = new ProjectionInfo("+proj=tmerc +lat_0=33 +lon_0=131 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone3 = new ProjectionInfo("+proj=tmerc +lat_0=36 +lon_0=132.1666666666667 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone4 = new ProjectionInfo("+proj=tmerc +lat_0=33 +lon_0=133.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone5 = new ProjectionInfo("+proj=tmerc +lat_0=36 +lon_0=134.3333333333333 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone6 = new ProjectionInfo("+proj=tmerc +lat_0=36 +lon_0=136 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone7 = new ProjectionInfo("+proj=tmerc +lat_0=36 +lon_0=137.1666666666667 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone8 = new ProjectionInfo("+proj=tmerc +lat_0=36 +lon_0=138.5 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            JGD2000JapanZone9 = new ProjectionInfo("+proj=tmerc +lat_0=36 +lon_0=139.8333333333333 +k=0.999900 +x_0=0 +y_0=0 +ellps=GRS80 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591