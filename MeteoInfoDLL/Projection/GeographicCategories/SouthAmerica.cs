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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:18:17 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.GeographicCategories
{


    /// <summary>
    /// SouthAmerica
    /// </summary>
    public class SouthAmerica : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Aratu;
        public readonly ProjectionInfo Bogota;
        public readonly ProjectionInfo BogotaBogota;
        public readonly ProjectionInfo CampoInchauspe;
        public readonly ProjectionInfo ChosMalal1914;
        public readonly ProjectionInfo Chua;
        public readonly ProjectionInfo CorregoAlegre;
        public readonly ProjectionInfo GuyaneFrancaise;
        public readonly ProjectionInfo HitoXVIII1963;
        public readonly ProjectionInfo LaCanoa;
        public readonly ProjectionInfo Lake;
        public readonly ProjectionInfo LomaQuintana;
        public readonly ProjectionInfo MountDillon;
        public readonly ProjectionInfo Naparima1955;
        public readonly ProjectionInfo Naparima1972;
        public readonly ProjectionInfo PampadelCastillo;
        public readonly ProjectionInfo POSGAR;
        public readonly ProjectionInfo POSGAR1998;
        public readonly ProjectionInfo ProvisionalSouthAmer;
        public readonly ProjectionInfo REGVEN;
        public readonly ProjectionInfo SapperHill1943;
        public readonly ProjectionInfo SIRGAS;
        public readonly ProjectionInfo SouthAmericanDatum1969;
        public readonly ProjectionInfo Trinidad1903;
        public readonly ProjectionInfo Yacare;
        public readonly ProjectionInfo Zanderij;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SouthAmerica
        /// </summary>
        public SouthAmerica()
        {
            Aratu = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Bogota = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            BogotaBogota = new ProjectionInfo("+proj=longlat +ellps=intl +pm=-74.08091666666667 +no_defs ");
            CampoInchauspe = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            ChosMalal1914 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Chua = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            CorregoAlegre = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            GuyaneFrancaise = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            HitoXVIII1963 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            LaCanoa = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Lake = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            LomaQuintana = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            MountDillon = new ProjectionInfo("+proj=longlat +a=6378293.639 +b=6356617.98149216 +no_defs ");
            Naparima1955 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Naparima1972 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            PampadelCastillo = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            POSGAR = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            POSGAR1998 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            ProvisionalSouthAmer = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            REGVEN = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            SapperHill1943 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            SIRGAS = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            SouthAmericanDatum1969 = new ProjectionInfo("+proj=longlat +ellps=aust_SA +no_defs ");
            Trinidad1903 = new ProjectionInfo("+proj=longlat +a=6378293.639 +b=6356617.98149216 +no_defs ");
            Yacare = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Zanderij = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591