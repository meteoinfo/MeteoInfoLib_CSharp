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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:14:40 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591

namespace MeteoInfoC.Projections.GeographicCategories
{


    /// <summary>
    /// NorthAmerica
    /// </summary>
    public class NorthAmerica : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AlaskanIslands;
        public readonly ProjectionInfo AmericanSamoa1962;
        public readonly ProjectionInfo Ammassalik1958;
        public readonly ProjectionInfo ATS1977;
        public readonly ProjectionInfo Barbados;
        public readonly ProjectionInfo Bermuda1957;
        public readonly ProjectionInfo Bermuda2000;
        public readonly ProjectionInfo CapeCanaveral;
        public readonly ProjectionInfo Guam1963;
        public readonly ProjectionInfo Helle1954;
        public readonly ProjectionInfo Jamaica1875;
        public readonly ProjectionInfo Jamaica1969;
        public readonly ProjectionInfo NAD1927CGQ77;
        public readonly ProjectionInfo NAD1927Definition1976;
        public readonly ProjectionInfo NADMichigan;
        public readonly ProjectionInfo NorthAmerican1983CSRS98;
        public readonly ProjectionInfo NorthAmerican1983HARN;
        public readonly ProjectionInfo NorthAmericanDatum1927;
        public readonly ProjectionInfo NorthAmericanDatum1983;
        public readonly ProjectionInfo OldHawaiian;
        public readonly ProjectionInfo PuertoRico;
        public readonly ProjectionInfo Qornoq;
        public readonly ProjectionInfo Qornoq1927;
        public readonly ProjectionInfo Scoresbysund1952;
        public readonly ProjectionInfo StGeorgeIsland;
        public readonly ProjectionInfo StLawrenceIsland;
        public readonly ProjectionInfo StPaulIsland;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NorthAmerica
        /// </summary>
        public NorthAmerica()
        {
            AlaskanIslands = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            AmericanSamoa1962 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Ammassalik1958 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            ATS1977 = new ProjectionInfo("+proj=longlat +a=6378135 +b=6356750.304921594 +no_defs ");
            Barbados = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Bermuda1957 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Bermuda2000 = new ProjectionInfo("+proj=longlat +ellps=WGS84 +no_defs ");
            CapeCanaveral = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Guam1963 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Helle1954 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Jamaica1875 = new ProjectionInfo("+proj=longlat +a=6378249.138 +b=6356514.959419348 +no_defs ");
            Jamaica1969 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            NAD1927CGQ77 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            NAD1927Definition1976 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            NADMichigan = new ProjectionInfo("+proj=longlat +a=6378450.047 +b=6356826.620025999 +no_defs ");
            NorthAmerican1983CSRS98 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            NorthAmerican1983HARN = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            NorthAmericanDatum1927 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +datum=NAD27 +no_defs ");
            NorthAmericanDatum1983 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +datum=NAD83 +no_defs ");
            OldHawaiian = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            PuertoRico = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Qornoq = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Qornoq1927 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Scoresbysund1952 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            StGeorgeIsland = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            StLawrenceIsland = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            StPaulIsland = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");


        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591