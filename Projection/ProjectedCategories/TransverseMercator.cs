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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:06:30 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// TransverseMercator
    /// </summary>
    public class TransverseMercatorSystems : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo WGS1984lo16;
        public readonly ProjectionInfo WGS1984lo17;
        public readonly ProjectionInfo WGS1984lo18;
        public readonly ProjectionInfo WGS1984lo19;
        public readonly ProjectionInfo WGS1984lo20;
        public readonly ProjectionInfo WGS1984lo21;
        public readonly ProjectionInfo WGS1984lo22;
        public readonly ProjectionInfo WGS1984lo23;
        public readonly ProjectionInfo WGS1984lo24;
        public readonly ProjectionInfo WGS1984lo25;
        public readonly ProjectionInfo WGS1984lo26;
        public readonly ProjectionInfo WGS1984lo27;
        public readonly ProjectionInfo WGS1984lo28;
        public readonly ProjectionInfo WGS1984lo29;
        public readonly ProjectionInfo WGS1984lo30;
        public readonly ProjectionInfo WGS1984lo31;
        public readonly ProjectionInfo WGS1984lo32;
        public readonly ProjectionInfo WGS1984lo33;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TransverseMercator
        /// </summary>
        public TransverseMercatorSystems()
        {
            WGS1984lo16 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=16 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo17 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=17 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo18 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=18 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo19 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=19 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo20 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=20 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo21 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=21 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo22 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=22 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo23 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=23 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo24 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=24 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo25 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=25 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo26 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=26 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo27 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=27 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo28 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=28 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo29 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=29 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo30 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=30 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo31 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=31 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo32 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=32 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo33 = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=33 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");


        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591