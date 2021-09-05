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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:06:28 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591
namespace MeteoInfoC.Projections.GeographicCategories
{


    /// <summary>
    /// Australia
    /// </summary>
    public class Australia : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AustralianGeodeticDatum1966;
        public readonly ProjectionInfo AustralianGeodeticDatum1984;
        public readonly ProjectionInfo ChathamIslands1979;
        public readonly ProjectionInfo GeocentricDatumofAustralia1994;
        public readonly ProjectionInfo NewZealandGeodeticDatum1949;
        public readonly ProjectionInfo NZGD2000;



        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Australia
        /// </summary>
        public Australia()
        {
            AustralianGeodeticDatum1966 = new ProjectionInfo("+proj=longlat +ellps=aust_SA +no_defs ");
            AustralianGeodeticDatum1984 = new ProjectionInfo("+proj=longlat +ellps=aust_SA +no_defs ");
            ChathamIslands1979 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            GeocentricDatumofAustralia1994 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            NewZealandGeodeticDatum1949 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            NZGD2000 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591
