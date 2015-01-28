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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:13:38 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// WorldSpheroid
    /// </summary>
    public class WorldSpheroid : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Aitoffsphere;
        public readonly ProjectionInfo Behrmannsphere;
        public readonly ProjectionInfo Bonnesphere;
        public readonly ProjectionInfo CrasterParabolicsphere;
        public readonly ProjectionInfo CylindricalEqualAreasphere;
        public readonly ProjectionInfo EckertIIIsphere;
        public readonly ProjectionInfo EckertIIsphere;
        public readonly ProjectionInfo EckertIsphere;
        public readonly ProjectionInfo EckertIVsphere;
        public readonly ProjectionInfo EckertVIsphere;
        public readonly ProjectionInfo EckertVsphere;
        public readonly ProjectionInfo EquidistantConicsphere;
        public readonly ProjectionInfo EquidistantCylindricalsphere;
        public readonly ProjectionInfo FlatPolarQuarticsphere;
        public readonly ProjectionInfo GallStereographicsphere;
        public readonly ProjectionInfo HammerAitoffsphere;
        public readonly ProjectionInfo Loximuthalsphere;
        public readonly ProjectionInfo Mercatorsphere;
        public readonly ProjectionInfo MillerCylindricalsphere;
        public readonly ProjectionInfo Mollweidesphere;
        public readonly ProjectionInfo PlateCarreesphere;
        public readonly ProjectionInfo Polyconicsphere;
        public readonly ProjectionInfo QuarticAuthalicsphere;
        public readonly ProjectionInfo Robinsonsphere;
        public readonly ProjectionInfo Sinusoidalsphere;
        public readonly ProjectionInfo Timessphere;
        public readonly ProjectionInfo VanderGrintenIsphere;
        public readonly ProjectionInfo VerticalPerspectivesphere;
        public readonly ProjectionInfo WinkelIIsphere;
        public readonly ProjectionInfo WinkelIsphere;
        public readonly ProjectionInfo WinkelTripelNGSsphere;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of WorldSpheroid
        /// </summary>
        public WorldSpheroid()
        {
            Aitoffsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            Behrmannsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            Bonnesphere = new ProjectionInfo("+proj=bonne +lon_0=0 +lat_1=60 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            CrasterParabolicsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            CylindricalEqualAreasphere = new ProjectionInfo("+proj=cea +lon_0=0 +lat_ts=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            EckertIIIsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            EckertIIsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            EckertIsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            EckertIVsphere = new ProjectionInfo("+proj=eck4 +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            EckertVIsphere = new ProjectionInfo("+proj=eck6 +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            EckertVsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            EquidistantConicsphere = new ProjectionInfo("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=60 +lat_2=60 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            EquidistantCylindricalsphere = new ProjectionInfo("+proj=eqc +lat_ts=0 +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            FlatPolarQuarticsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            GallStereographicsphere = new ProjectionInfo("+proj=gall +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            HammerAitoffsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            Loximuthalsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            Mercatorsphere = new ProjectionInfo("+proj=merc +lat_ts=0 +lon_0=0 +k=1.000000 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            MillerCylindricalsphere = new ProjectionInfo("+proj=mill +lat_0=0 +lon_0=0 +x_0=0 +y_0=0 +R_A +a=6371000 +b=6371000 +units=m +no_defs ");
            Mollweidesphere = new ProjectionInfo("+proj=moll +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            PlateCarreesphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            Polyconicsphere = new ProjectionInfo("+proj=poly +lat_0=0 +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            QuarticAuthalicsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            Robinsonsphere = new ProjectionInfo("+proj=robin +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            Sinusoidalsphere = new ProjectionInfo("+proj=sinu +lon_0=0 +x_0=0 +y_0=0 +a=6371000 +b=6371000 +units=m +no_defs ");
            Timessphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            VanderGrintenIsphere = new ProjectionInfo("+proj=vandg +lon_0=0 +x_0=0 +y_0=0 +R_A +a=6371000 +b=6371000 +units=m +no_defs ");
            VerticalPerspectivesphere = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WinkelIIsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            WinkelIsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");
            WinkelTripelNGSsphere = new ProjectionInfo("+a=6371000 +b=6371000 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591