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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:15:48 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591
namespace MeteoInfoC.Projections.GeographicCategories
{


    /// <summary>
    /// Oceans
    /// </summary>
    public class Oceans : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AlaskanIslands;
        public readonly ProjectionInfo AmericanSamoa1962;
        public readonly ProjectionInfo Anguilla1957;
        public readonly ProjectionInfo Anna1Astro1965;
        public readonly ProjectionInfo Antigua1943;
        public readonly ProjectionInfo AscensionIsland1958;
        public readonly ProjectionInfo AstroBeaconE1945;
        public readonly ProjectionInfo AstroDOS714;
        public readonly ProjectionInfo AstronomicalStation1952;
        public readonly ProjectionInfo AzoresCentral1948;
        public readonly ProjectionInfo AzoresCentral1995;
        public readonly ProjectionInfo AzoresOccidental1939;
        public readonly ProjectionInfo AzoresOriental1940;
        public readonly ProjectionInfo AzoresOriental1995;
        public readonly ProjectionInfo BabSouth;
        public readonly ProjectionInfo Barbados;
        public readonly ProjectionInfo Barbados1938;
        public readonly ProjectionInfo BellevueIGN;
        public readonly ProjectionInfo Bermuda1957;
        public readonly ProjectionInfo Bermuda2000;
        public readonly ProjectionInfo CantonAstro1966;
        public readonly ProjectionInfo ChathamIslandAstro1971;
        public readonly ProjectionInfo Combani1950;
        public readonly ProjectionInfo CSG1967;
        public readonly ProjectionInfo Dominica1945;
        public readonly ProjectionInfo DOS1968;
        public readonly ProjectionInfo EasterIsland1967;
        public readonly ProjectionInfo FortDesaix;
        public readonly ProjectionInfo FortMarigot;
        public readonly ProjectionInfo FortThomas1955;
        public readonly ProjectionInfo Gan1970;
        public readonly ProjectionInfo GraciosaBaseSW1948;
        public readonly ProjectionInfo GrandComoros;
        public readonly ProjectionInfo Grenada1953;
        public readonly ProjectionInfo Guam1963;
        public readonly ProjectionInfo GUX1Astro;
        public readonly ProjectionInfo Hjorsey1955;
        public readonly ProjectionInfo IGN53Mare;
        public readonly ProjectionInfo IGN56Lifou;
        public readonly ProjectionInfo IGN72GrandeTerre;
        public readonly ProjectionInfo IGN72NukuHiva;
        public readonly ProjectionInfo ISTS061Astro1968;
        public readonly ProjectionInfo ISTS073Astro1969;
        public readonly ProjectionInfo Jamaica1875;
        public readonly ProjectionInfo Jamaica1969;
        public readonly ProjectionInfo JohnstonIsland1961;
        public readonly ProjectionInfo K01949;
        public readonly ProjectionInfo KerguelenIsland1949;
        public readonly ProjectionInfo KusaieAstro1951;
        public readonly ProjectionInfo LC5Astro1961;
        public readonly ProjectionInfo Madeira1936;
        public readonly ProjectionInfo Mahe1971;
        public readonly ProjectionInfo Majuro;
        public readonly ProjectionInfo MidwayAstro1961;
        public readonly ProjectionInfo Montserrat1958;
        public readonly ProjectionInfo MOP78;
        public readonly ProjectionInfo NEA74Noumea;
        public readonly ProjectionInfo ObservMeteorologico1939;
        public readonly ProjectionInfo OldHawaiian;
        public readonly ProjectionInfo PicodeLasNieves;
        public readonly ProjectionInfo PitcairnAstro1967;
        public readonly ProjectionInfo PitondesNeiges;
        public readonly ProjectionInfo Pohnpei;
        public readonly ProjectionInfo PortoSanto1936;
        public readonly ProjectionInfo PortoSanto1995;
        public readonly ProjectionInfo PuertoRico;
        public readonly ProjectionInfo Reunion;
        public readonly ProjectionInfo RGFG1995;
        public readonly ProjectionInfo RGNC1991;
        public readonly ProjectionInfo RGR1992;
        public readonly ProjectionInfo RRAF1991;
        public readonly ProjectionInfo SaintPierreetMiquelon1950;
        public readonly ProjectionInfo SainteAnne;
        public readonly ProjectionInfo SantoDOS1965;
        public readonly ProjectionInfo SaoBraz;
        public readonly ProjectionInfo SapperHill1943;
        public readonly ProjectionInfo SelvagemGrande1938;
        public readonly ProjectionInfo StKitts1955;
        public readonly ProjectionInfo StLucia1955;
        public readonly ProjectionInfo StVincent1945;
        public readonly ProjectionInfo ST71Belep;
        public readonly ProjectionInfo ST84IledesPins;
        public readonly ProjectionInfo ST87Ouvea;
        public readonly ProjectionInfo Tahaa;
        public readonly ProjectionInfo Tahiti;
        public readonly ProjectionInfo TernIslandAstro1961;
        public readonly ProjectionInfo TristanAstro1968;
        public readonly ProjectionInfo VitiLevu1916;
        public readonly ProjectionInfo WakeIslandAstro1952;
        public readonly ProjectionInfo WakeEniwetok1960;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Oceans
        /// </summary>
        public Oceans()
        {
            AlaskanIslands = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            AmericanSamoa1962 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Anguilla1957 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Anna1Astro1965 = new ProjectionInfo("+proj=longlat +ellps=aust_SA +no_defs ");
            Antigua1943 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            AscensionIsland1958 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            AstroBeaconE1945 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            AstroDOS714 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            AstronomicalStation1952 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            AzoresCentral1948 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            AzoresCentral1995 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            AzoresOccidental1939 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            AzoresOriental1940 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            AzoresOriental1995 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            BabSouth = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Barbados = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Barbados1938 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            BellevueIGN = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Bermuda1957 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Bermuda2000 = new ProjectionInfo("+proj=longlat +ellps=WGS84 +no_defs ");
            CantonAstro1966 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            ChathamIslandAstro1971 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Combani1950 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            CSG1967 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Dominica1945 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            DOS1968 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            EasterIsland1967 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            FortDesaix = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            FortMarigot = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            FortThomas1955 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Gan1970 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            GraciosaBaseSW1948 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            GrandComoros = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Grenada1953 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Guam1963 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            GUX1Astro = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Hjorsey1955 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            IGN53Mare = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            IGN56Lifou = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            IGN72GrandeTerre = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            IGN72NukuHiva = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            ISTS061Astro1968 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            ISTS073Astro1969 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Jamaica1875 = new ProjectionInfo("+proj=longlat +a=6378249.138 +b=6356514.959419348 +no_defs ");
            Jamaica1969 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            JohnstonIsland1961 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            K01949 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            KerguelenIsland1949 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            KusaieAstro1951 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            LC5Astro1961 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Madeira1936 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Mahe1971 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            Majuro = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            MidwayAstro1961 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Montserrat1958 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            MOP78 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            NEA74Noumea = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            ObservMeteorologico1939 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            OldHawaiian = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            PicodeLasNieves = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            PitcairnAstro1967 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            PitondesNeiges = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Pohnpei = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            PortoSanto1936 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            PortoSanto1995 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            PuertoRico = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            Reunion = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            RGFG1995 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            RGNC1991 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            RGR1992 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            RRAF1991 = new ProjectionInfo("+proj=longlat +ellps=WGS84 +no_defs ");
            SaintPierreetMiquelon1950 = new ProjectionInfo("+proj=longlat +ellps=clrk66 +no_defs ");
            SainteAnne = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            SantoDOS1965 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            SaoBraz = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            SapperHill1943 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            SelvagemGrande1938 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            StKitts1955 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            StLucia1955 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            StVincent1945 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            ST71Belep = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            ST84IledesPins = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            ST87Ouvea = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Tahaa = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Tahiti = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            TernIslandAstro1961 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            TristanAstro1968 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            VitiLevu1916 = new ProjectionInfo("+proj=longlat +ellps=clrk80 +no_defs ");
            WakeIslandAstro1952 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            WakeEniwetok1960 = new ProjectionInfo("+proj=longlat +a=6378270 +b=6356794.343434343 +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591