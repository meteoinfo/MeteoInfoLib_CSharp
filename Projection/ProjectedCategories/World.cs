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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:12:33 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// World
    /// </summary>
    public class World : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Aitoffworld;
        public readonly ProjectionInfo Behrmannworld;
        public readonly ProjectionInfo Bonneworld;
        public readonly ProjectionInfo CrasterParabolicworld;
        public readonly ProjectionInfo Cubeworld;
        public readonly ProjectionInfo CylindricalEqualAreaworld;
        public readonly ProjectionInfo EckertIIIworld;
        public readonly ProjectionInfo EckertIIworld;
        public readonly ProjectionInfo EckertIVworld;
        public readonly ProjectionInfo EckertIworld;
        public readonly ProjectionInfo EckertVIworld;
        public readonly ProjectionInfo EckertVworld;
        public readonly ProjectionInfo EquidistantConicworld;
        public readonly ProjectionInfo EquidistantCylindricalworld;
        public readonly ProjectionInfo FlatPolarQuarticworld;
        public readonly ProjectionInfo Fullerworld;
        public readonly ProjectionInfo GallStereographicworld;
        public readonly ProjectionInfo HammerAitoffworld;
        public readonly ProjectionInfo Loximuthalworld;
        public readonly ProjectionInfo Mercatorworld;
        public readonly ProjectionInfo MillerCylindricalworld;
        public readonly ProjectionInfo Mollweideworld;
        public readonly ProjectionInfo PlateCarreeworld;
        public readonly ProjectionInfo Polyconicworld;
        public readonly ProjectionInfo QuarticAuthalicworld;
        public readonly ProjectionInfo Robinsonworld;
        public readonly ProjectionInfo Sinusoidalworld;
        public readonly ProjectionInfo TheWorldfromSpace;
        public readonly ProjectionInfo Timesworld;
        public readonly ProjectionInfo VanderGrintenIworld;
        public readonly ProjectionInfo VerticalPerspectiveworld;
        public readonly ProjectionInfo WinkelIIworld;
        public readonly ProjectionInfo WinkelIworld;
        public readonly ProjectionInfo WinkelTripelNGSworld;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of World
        /// </summary>
        public World()
        {
            Aitoffworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Behrmannworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Bonneworld = new ProjectionInfo("+proj=bonne +lon_0=0 +lat_1=60 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            CrasterParabolicworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Cubeworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            CylindricalEqualAreaworld = new ProjectionInfo("+proj=cea +lon_0=0 +lat_ts=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EckertIIIworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EckertIIworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EckertIVworld = new ProjectionInfo("+proj=eck4 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EckertIworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EckertVIworld = new ProjectionInfo("+proj=eck6 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EckertVworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EquidistantConicworld = new ProjectionInfo("+proj=eqdc +lat_0=0 +lon_0=0 +lat_1=60 +lat_2=60 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            EquidistantCylindricalworld = new ProjectionInfo("+proj=eqc +lat_ts=0 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            FlatPolarQuarticworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Fullerworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            GallStereographicworld = new ProjectionInfo("+proj=gall +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            HammerAitoffworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Loximuthalworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Mercatorworld = new ProjectionInfo("+proj=merc +lat_ts=0 +lon_0=0 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            MillerCylindricalworld = new ProjectionInfo("+proj=mill +lat_0=0 +lon_0=0 +x_0=0 +y_0=0 +R_A +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Mollweideworld = new ProjectionInfo("+proj=moll +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            PlateCarreeworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Polyconicworld = new ProjectionInfo("+proj=poly +lat_0=0 +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            QuarticAuthalicworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Robinsonworld = new ProjectionInfo("+proj=robin +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            Sinusoidalworld = new ProjectionInfo("+proj=sinu +lon_0=0 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            TheWorldfromSpace = new ProjectionInfo("+proj=ortho +lat_0=42.5333333333 +lon_0=-72.5333333334 +x_0=0 +y_0=0 +a=6370997 +b=6370997 +units=m +no_defs ");
            Timesworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            VanderGrintenIworld = new ProjectionInfo("+proj=vandg +lon_0=0 +x_0=0 +y_0=0 +R_A +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            VerticalPerspectiveworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WinkelIIworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WinkelIworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WinkelTripelNGSworld = new ProjectionInfo("+ellps=WGS84 +datum=WGS84 +units=m +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591