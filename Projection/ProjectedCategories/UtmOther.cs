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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:08:45 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.ProjectedCategories
{


    /// <summary>
    /// UtmOther
    /// </summary>
    public class UtmOther : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Abidjan1987UTMZone29N;
        public readonly ProjectionInfo Abidjan1987UTMZone30N;
        public readonly ProjectionInfo AdindanUTMZone37N;
        public readonly ProjectionInfo AdindanUTMZone38N;
        public readonly ProjectionInfo AfgooyeUTMZone38N;
        public readonly ProjectionInfo AfgooyeUTMZone39N;
        public readonly ProjectionInfo AinelAbd1970UTMZone37N;
        public readonly ProjectionInfo AinelAbd1970UTMZone38N;
        public readonly ProjectionInfo AinelAbd1970UTMZone39N;
        public readonly ProjectionInfo AmericanSamoa1962UTMZone2S;
        public readonly ProjectionInfo AratuUTMZone22S;
        public readonly ProjectionInfo AratuUTMZone23S;
        public readonly ProjectionInfo AratuUTMZone24S;
        public readonly ProjectionInfo Arc1950UTMZone34S;
        public readonly ProjectionInfo Arc1950UTMZone35S;
        public readonly ProjectionInfo Arc1950UTMZone36S;
        public readonly ProjectionInfo Arc1960UTMZone35N;
        public readonly ProjectionInfo Arc1960UTMZone35S;
        public readonly ProjectionInfo Arc1960UTMZone36N;
        public readonly ProjectionInfo Arc1960UTMZone36S;
        public readonly ProjectionInfo Arc1960UTMZone37N;
        public readonly ProjectionInfo Arc1960UTMZone37S;
        public readonly ProjectionInfo ATS1977UTMZone19N;
        public readonly ProjectionInfo ATS1977UTMZone20N;
        public readonly ProjectionInfo AzoresCentral1995UTMZone26N;
        public readonly ProjectionInfo AzoresOriental1995UTMZone26N;
        public readonly ProjectionInfo BataviaUTMZone48S;
        public readonly ProjectionInfo BataviaUTMZone49S;
        public readonly ProjectionInfo BataviaUTMZone50S;
        public readonly ProjectionInfo BissauUTMZone28N;
        public readonly ProjectionInfo BogotaUTMZone17N;
        public readonly ProjectionInfo BogotaUTMZone18N;
        public readonly ProjectionInfo CamacupaUTMZone32S;
        public readonly ProjectionInfo CamacupaUTMZone33S;
        public readonly ProjectionInfo CampoInchauspeUTM19S;
        public readonly ProjectionInfo CampoInchauspeUTM20S;
        public readonly ProjectionInfo CapeUTMZone34S;
        public readonly ProjectionInfo CapeUTMZone35S;
        public readonly ProjectionInfo CapeUTMZone36S;
        public readonly ProjectionInfo CarthageUTMZone32N;
        public readonly ProjectionInfo Combani1950UTMZone38S;
        public readonly ProjectionInfo Conakry1905UTMZone28N;
        public readonly ProjectionInfo Conakry1905UTMZone29N;
        public readonly ProjectionInfo CorregoAlegreUTMZone23S;
        public readonly ProjectionInfo CorregoAlegreUTMZone24S;
        public readonly ProjectionInfo CSG1967UTMZone22N;
        public readonly ProjectionInfo DabolaUTMZone28N;
        public readonly ProjectionInfo DabolaUTMZone29N;
        public readonly ProjectionInfo Datum73UTMZone29N;
        public readonly ProjectionInfo DoualaUTMZone32N;
        public readonly ProjectionInfo ED1950ED77UTMZone38N;
        public readonly ProjectionInfo ED1950ED77UTMZone39N;
        public readonly ProjectionInfo ED1950ED77UTMZone40N;
        public readonly ProjectionInfo ED1950ED77UTMZone41N;
        public readonly ProjectionInfo ELD1979UTMZone32N;
        public readonly ProjectionInfo ELD1979UTMZone33N;
        public readonly ProjectionInfo ELD1979UTMZone34N;
        public readonly ProjectionInfo ELD1979UTMZone35N;
        public readonly ProjectionInfo ETRF1989UTMZone28N;
        public readonly ProjectionInfo ETRF1989UTMZone29N;
        public readonly ProjectionInfo ETRF1989UTMZone30N;
        public readonly ProjectionInfo ETRF1989UTMZone31N;
        public readonly ProjectionInfo ETRF1989UTMZone32N;
        public readonly ProjectionInfo ETRF1989UTMZone33N;
        public readonly ProjectionInfo ETRF1989UTMZone34N;
        public readonly ProjectionInfo ETRF1989UTMZone35N;
        public readonly ProjectionInfo ETRF1989UTMZone36N;
        public readonly ProjectionInfo ETRF1989UTMZone37N;
        public readonly ProjectionInfo ETRF1989UTMZone38N;
        public readonly ProjectionInfo ETRS1989UTMZone26N;
        public readonly ProjectionInfo ETRS1989UTMZone27N;
        public readonly ProjectionInfo ETRS1989UTMZone28N;
        public readonly ProjectionInfo ETRS1989UTMZone29N;
        public readonly ProjectionInfo ETRS1989UTMZone30N;
        public readonly ProjectionInfo ETRS1989UTMZone31N;
        public readonly ProjectionInfo ETRS1989UTMZone32N;
        public readonly ProjectionInfo ETRS1989UTMZone33N;
        public readonly ProjectionInfo ETRS1989UTMZone34N;
        public readonly ProjectionInfo ETRS1989UTMZone35N;
        public readonly ProjectionInfo ETRS1989UTMZone36N;
        public readonly ProjectionInfo ETRS1989UTMZone37N;
        public readonly ProjectionInfo ETRS1989UTMZone38N;
        public readonly ProjectionInfo ETRS1989UTMZone39N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone28N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone29N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone30N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone31N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone32N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone33N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone34N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone35N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone36N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone37N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone38N;
        public readonly ProjectionInfo FahudUTMZone39N;
        public readonly ProjectionInfo FahudUTMZone40N;
        public readonly ProjectionInfo FortDesaixUTMZone20N;
        public readonly ProjectionInfo FortMarigotUTMZone20N;
        public readonly ProjectionInfo GarouaUTMZone33N;
        public readonly ProjectionInfo GraciosaBaseSW1948UTMZone26N;
        public readonly ProjectionInfo GrandComorosUTMZone38S;
        public readonly ProjectionInfo HitoXVIII1963UTMZone19S;
        public readonly ProjectionInfo Hjorsey1955UTMZone26N;
        public readonly ProjectionInfo Hjorsey1955UTMZone27N;
        public readonly ProjectionInfo Hjorsey1955UTMZone28N;
        public readonly ProjectionInfo HongKong1980UTMZone49N;
        public readonly ProjectionInfo HongKong1980UTMZone50N;
        public readonly ProjectionInfo IGM1995UTMZone32N;
        public readonly ProjectionInfo IGM1995UTMZone33N;
        public readonly ProjectionInfo IGN53MareUTMZone58S;
        public readonly ProjectionInfo IGN56LifouUTMZone58S;
        public readonly ProjectionInfo IGN72GrandeTerreUTMZone58S;
        public readonly ProjectionInfo IGN72NukuHivaUTMZone7S;
        public readonly ProjectionInfo Indian1954UTMZone46N;
        public readonly ProjectionInfo Indian1954UTMZone47N;
        public readonly ProjectionInfo Indian1954UTMZone48N;
        public readonly ProjectionInfo Indian1960UTMZone48N;
        public readonly ProjectionInfo Indian1960UTMZone49N;
        public readonly ProjectionInfo Indian1975UTMZone47N;
        public readonly ProjectionInfo Indian1975UTMZone48N;
        public readonly ProjectionInfo Indonesia1974UTMZone46N;
        public readonly ProjectionInfo Indonesia1974UTMZone46S;
        public readonly ProjectionInfo Indonesia1974UTMZone47N;
        public readonly ProjectionInfo Indonesia1974UTMZone47S;
        public readonly ProjectionInfo Indonesia1974UTMZone48N;
        public readonly ProjectionInfo Indonesia1974UTMZone48S;
        public readonly ProjectionInfo Indonesia1974UTMZone49N;
        public readonly ProjectionInfo Indonesia1974UTMZone49S;
        public readonly ProjectionInfo Indonesia1974UTMZone50N;
        public readonly ProjectionInfo Indonesia1974UTMZone50S;
        public readonly ProjectionInfo Indonesia1974UTMZone51N;
        public readonly ProjectionInfo Indonesia1974UTMZone51S;
        public readonly ProjectionInfo Indonesia1974UTMZone52N;
        public readonly ProjectionInfo Indonesia1974UTMZone52S;
        public readonly ProjectionInfo Indonesia1974UTMZone53N;
        public readonly ProjectionInfo Indonesia1974UTMZone53S;
        public readonly ProjectionInfo Indonesia1974UTMZone54S;
        public readonly ProjectionInfo IRENET95UTMZone29N;
        public readonly ProjectionInfo JGD2000UTMZone51N;
        public readonly ProjectionInfo JGD2000UTMZone52N;
        public readonly ProjectionInfo JGD2000UTMZone53N;
        public readonly ProjectionInfo JGD2000UTMZone54N;
        public readonly ProjectionInfo JGD2000UTMZone55N;
        public readonly ProjectionInfo JGD2000UTMZone56N;
        public readonly ProjectionInfo K01949UTMZone42S;
        public readonly ProjectionInfo KertauUTMZone47N;
        public readonly ProjectionInfo KertauUTMZone48N;
        public readonly ProjectionInfo KousseriUTMZone33N;
        public readonly ProjectionInfo LaCanoaUTMZone18N;
        public readonly ProjectionInfo LaCanoaUTMZone19N;
        public readonly ProjectionInfo LaCanoaUTMZone20N;
        public readonly ProjectionInfo LaCanoaUTMZone21N;
        public readonly ProjectionInfo Locodjo1965UTMZone29N;
        public readonly ProjectionInfo Locodjo1965UTMZone30N;
        public readonly ProjectionInfo LomeUTMZone31N;
        public readonly ProjectionInfo Malongo1987UTMZone32S;
        public readonly ProjectionInfo Manoca1962UTMZone32N;
        public readonly ProjectionInfo MassawaUTMZone37N;
        public readonly ProjectionInfo MhastUTMZone32S;
        public readonly ProjectionInfo MinnaUTMZone31N;
        public readonly ProjectionInfo MinnaUTMZone32N;
        public readonly ProjectionInfo MOP78UTMZone1S;
        public readonly ProjectionInfo MoznetUTMZone36S;
        public readonly ProjectionInfo MoznetUTMZone37S;
        public readonly ProjectionInfo MporalokoUTMZone32N;
        public readonly ProjectionInfo MporalokoUTMZone32S;
        public readonly ProjectionInfo NAD1927BLMZone14N;
        public readonly ProjectionInfo NAD1927BLMZone15N;
        public readonly ProjectionInfo NAD1927BLMZone16N;
        public readonly ProjectionInfo NAD1927BLMZone17N;
        public readonly ProjectionInfo NAD1983HARNUTMZone11N;
        public readonly ProjectionInfo NAD1983HARNUTMZone12N;
        public readonly ProjectionInfo NAD1983HARNUTMZone13N;
        public readonly ProjectionInfo NAD1983HARNUTMZone18N;
        public readonly ProjectionInfo NAD1983HARNUTMZone2S;
        public readonly ProjectionInfo NAD1983HARNUTMZone4N;
        public readonly ProjectionInfo NAD1983HARNUTMZone5N;
        public readonly ProjectionInfo Nahrwan1967UTMZone38N;
        public readonly ProjectionInfo Nahrwan1967UTMZone39N;
        public readonly ProjectionInfo Nahrwan1967UTMZone40N;
        public readonly ProjectionInfo Naparima1955UTMZone20N;
        public readonly ProjectionInfo Naparima1972UTMZone20N;
        public readonly ProjectionInfo NEA74NoumeaUTMZone58S;
        public readonly ProjectionInfo NGNUTMZone38N;
        public readonly ProjectionInfo NGNUTMZone39N;
        public readonly ProjectionInfo NGO1948UTMZone32N;
        public readonly ProjectionInfo NGO1948UTMZone33N;
        public readonly ProjectionInfo NGO1948UTMZone34N;
        public readonly ProjectionInfo NGO1948UTMZone35N;
        public readonly ProjectionInfo NordSahara1959UTMZone29N;
        public readonly ProjectionInfo NordSahara1959UTMZone30N;
        public readonly ProjectionInfo NordSahara1959UTMZone31N;
        public readonly ProjectionInfo NordSahara1959UTMZone32N;
        public readonly ProjectionInfo NZGD1949UTMZone58S;
        public readonly ProjectionInfo NZGD1949UTMZone59S;
        public readonly ProjectionInfo NZGD1949UTMZone60S;
        public readonly ProjectionInfo NZGD2000UTMZone58S;
        public readonly ProjectionInfo NZGD2000UTMZone59S;
        public readonly ProjectionInfo NZGD2000UTMZone60S;
        public readonly ProjectionInfo ObservMeteorologico1939UTMZone25N;
        public readonly ProjectionInfo OldHawaiianUTMZone4N;
        public readonly ProjectionInfo OldHawaiianUTMZone5N;
        public readonly ProjectionInfo PDO1993UTMZone39N;
        public readonly ProjectionInfo PDO1993UTMZone40N;
        public readonly ProjectionInfo PointeNoireUTMZone32S;
        public readonly ProjectionInfo PortoSanto1936UTMZone28N;
        public readonly ProjectionInfo PortoSanto1995UTMZone28N;
        public readonly ProjectionInfo ProvSAmerDatumUTMZone17s;
        public readonly ProjectionInfo ProvSAmerDatumUTMZone18N;
        public readonly ProjectionInfo ProvSAmerDatumUTMZone18S;
        public readonly ProjectionInfo ProvSAmerDatumUTMZone19N;
        public readonly ProjectionInfo ProvSAmerDatumUTMZone19S;
        public readonly ProjectionInfo ProvSAmerDatumUTMZone20N;
        public readonly ProjectionInfo ProvSAmerDatumUTMZone20S;
        public readonly ProjectionInfo ProvSAmerDatumUTMZone21N;
        public readonly ProjectionInfo ProvSAmerDatumUTMZone22S;
        public readonly ProjectionInfo PuertoRicoUTMZone20N;
        public readonly ProjectionInfo Qornoq1927UTMZone22N;
        public readonly ProjectionInfo Qornoq1927UTMZone23N;
        public readonly ProjectionInfo REGVENUTMZone18N;
        public readonly ProjectionInfo REGVENUTMZone19N;
        public readonly ProjectionInfo REGVENUTMZone20N;
        public readonly ProjectionInfo RGFG1995UTMZone22N;
        public readonly ProjectionInfo RGR1992UTMZone40S;
        public readonly ProjectionInfo RRAF1991UTMZone20N;
        public readonly ProjectionInfo SainteAnneUTMZone20N;
        public readonly ProjectionInfo SaintPierreetMiquelon1950UTMZone21N;
        public readonly ProjectionInfo SambojaUTMZone50S;
        public readonly ProjectionInfo SaoBrazUTMZone26N;
        public readonly ProjectionInfo SapperHill1943UTMZone20S;
        public readonly ProjectionInfo SapperHill1943UTMZone21S;
        public readonly ProjectionInfo SchwarzeckUTMZone33S;
        public readonly ProjectionInfo SelvagemGrande1938UTMZone28N;
        public readonly ProjectionInfo SierraLeone1968UTMZone28N;
        public readonly ProjectionInfo SierraLeone1968UTMZone29N;
        public readonly ProjectionInfo SIRGASUTMZone17N;
        public readonly ProjectionInfo SIRGASUTMZone17S;
        public readonly ProjectionInfo SIRGASUTMZone18N;
        public readonly ProjectionInfo SIRGASUTMZone18S;
        public readonly ProjectionInfo SIRGASUTMZone19N;
        public readonly ProjectionInfo SIRGASUTMZone19S;
        public readonly ProjectionInfo SIRGASUTMZone20N;
        public readonly ProjectionInfo SIRGASUTMZone20S;
        public readonly ProjectionInfo SIRGASUTMZone21N;
        public readonly ProjectionInfo SIRGASUTMZone21S;
        public readonly ProjectionInfo SIRGASUTMZone22N;
        public readonly ProjectionInfo SIRGASUTMZone22S;
        public readonly ProjectionInfo SIRGASUTMZone23S;
        public readonly ProjectionInfo SIRGASUTMZone24S;
        public readonly ProjectionInfo SIRGASUTMZone25S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone17S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone18N;
        public readonly ProjectionInfo SouthAmerican1969UTMZone18S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone19N;
        public readonly ProjectionInfo SouthAmerican1969UTMZone19S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone20N;
        public readonly ProjectionInfo SouthAmerican1969UTMZone20S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone21N;
        public readonly ProjectionInfo SouthAmerican1969UTMZone21S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone22N;
        public readonly ProjectionInfo SouthAmerican1969UTMZone22S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone23S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone24S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone25S;
        public readonly ProjectionInfo ST71BelepUTMZone58S;
        public readonly ProjectionInfo ST84IledesPinsUTMZone58S;
        public readonly ProjectionInfo ST87OuveaUTMZone58S;
        public readonly ProjectionInfo SudanUTMZone35N;
        public readonly ProjectionInfo SudanUTMZone36N;
        public readonly ProjectionInfo TahaaUTMZone5S;
        public readonly ProjectionInfo TahitiUTMZone6S;
        public readonly ProjectionInfo Tananarive1925UTMZone38S;
        public readonly ProjectionInfo Tananarive1925UTMZone39S;
        public readonly ProjectionInfo TeteUTMZone36S;
        public readonly ProjectionInfo TeteUTMZone37S;
        public readonly ProjectionInfo Timbalai1948UTMZone49N;
        public readonly ProjectionInfo Timbalai1948UTMZone50N;
        public readonly ProjectionInfo TokyoUTMZone51N;
        public readonly ProjectionInfo TokyoUTMZone52N;
        public readonly ProjectionInfo TokyoUTMZone53N;
        public readonly ProjectionInfo TokyoUTMZone54N;
        public readonly ProjectionInfo TokyoUTMZone55N;
        public readonly ProjectionInfo TokyoUTMZone56N;
        public readonly ProjectionInfo TrucialCoast1948UTMZone39N;
        public readonly ProjectionInfo TrucialCoast1948UTMZone40N;
        public readonly ProjectionInfo YemenNGN1996UTMZone38N;
        public readonly ProjectionInfo YemenNGN1996UTMZone39N;
        public readonly ProjectionInfo Yoff1972UTMZone28N;
        public readonly ProjectionInfo Zanderij1972UTMZone21N;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of UtmOther
        /// </summary>
        public UtmOther()
        {
            Abidjan1987UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=clrk80 +units=m +no_defs ");
            Abidjan1987UTMZone30N = new ProjectionInfo("+proj=utm +zone=30 +ellps=clrk80 +units=m +no_defs ");
            AdindanUTMZone37N = new ProjectionInfo("+proj=utm +zone=37 +ellps=clrk80 +units=m +no_defs ");
            AdindanUTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=clrk80 +units=m +no_defs ");
            AfgooyeUTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=krass +units=m +no_defs ");
            AfgooyeUTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=krass +units=m +no_defs ");
            AinelAbd1970UTMZone37N = new ProjectionInfo("+proj=utm +zone=37 +ellps=intl +units=m +no_defs ");
            AinelAbd1970UTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=intl +units=m +no_defs ");
            AinelAbd1970UTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=intl +units=m +no_defs ");
            AmericanSamoa1962UTMZone2S = new ProjectionInfo("+proj=utm +zone=2 +south +ellps=clrk66 +units=m +no_defs ");
            AratuUTMZone22S = new ProjectionInfo("+proj=utm +zone=22 +south +ellps=intl +units=m +no_defs ");
            AratuUTMZone23S = new ProjectionInfo("+proj=utm +zone=23 +south +ellps=intl +units=m +no_defs ");
            AratuUTMZone24S = new ProjectionInfo("+proj=utm +zone=24 +south +ellps=intl +units=m +no_defs ");
            Arc1950UTMZone34S = new ProjectionInfo("+proj=utm +zone=34 +south +a=6378249.145 +b=6356514.966395495 +units=m +no_defs ");
            Arc1950UTMZone35S = new ProjectionInfo("+proj=utm +zone=35 +south +a=6378249.145 +b=6356514.966395495 +units=m +no_defs ");
            Arc1950UTMZone36S = new ProjectionInfo("+proj=utm +zone=36 +south +a=6378249.145 +b=6356514.966395495 +units=m +no_defs ");
            Arc1960UTMZone35N = new ProjectionInfo("+proj=utm +zone=35 +ellps=clrk80 +units=m +no_defs ");
            Arc1960UTMZone35S = new ProjectionInfo("+proj=utm +zone=35 +south +ellps=clrk80 +units=m +no_defs ");
            Arc1960UTMZone36N = new ProjectionInfo("+proj=utm +zone=36 +ellps=clrk80 +units=m +no_defs ");
            Arc1960UTMZone36S = new ProjectionInfo("+proj=utm +zone=36 +south +ellps=clrk80 +units=m +no_defs ");
            Arc1960UTMZone37N = new ProjectionInfo("+proj=utm +zone=37 +ellps=clrk80 +units=m +no_defs ");
            Arc1960UTMZone37S = new ProjectionInfo("+proj=utm +zone=37 +south +ellps=clrk80 +units=m +no_defs ");
            ATS1977UTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +a=6378135 +b=6356750.304921594 +units=m +no_defs ");
            ATS1977UTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +a=6378135 +b=6356750.304921594 +units=m +no_defs ");
            AzoresCentral1995UTMZone26N = new ProjectionInfo("+proj=utm +zone=26 +ellps=intl +units=m +no_defs ");
            AzoresOriental1995UTMZone26N = new ProjectionInfo("+proj=utm +zone=26 +ellps=intl +units=m +no_defs ");
            BataviaUTMZone48S = new ProjectionInfo("+proj=utm +zone=48 +south +ellps=bessel +units=m +no_defs ");
            BataviaUTMZone49S = new ProjectionInfo("+proj=utm +zone=49 +south +ellps=bessel +units=m +no_defs ");
            BataviaUTMZone50S = new ProjectionInfo("+proj=utm +zone=50 +south +ellps=bessel +units=m +no_defs ");
            BissauUTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=intl +units=m +no_defs ");
            BogotaUTMZone17N = new ProjectionInfo("+proj=utm +zone=17 +ellps=intl +units=m +no_defs ");
            BogotaUTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=intl +units=m +no_defs ");
            CamacupaUTMZone32S = new ProjectionInfo("+proj=utm +zone=32 +south +ellps=clrk80 +units=m +no_defs ");
            CamacupaUTMZone33S = new ProjectionInfo("+proj=utm +zone=33 +south +ellps=clrk80 +units=m +no_defs ");
            CampoInchauspeUTM19S = new ProjectionInfo("+proj=utm +zone=19 +south +ellps=intl +units=m +no_defs ");
            CampoInchauspeUTM20S = new ProjectionInfo("+proj=utm +zone=20 +south +ellps=intl +units=m +no_defs ");
            CapeUTMZone34S = new ProjectionInfo("+proj=utm +zone=34 +south +a=6378249.145 +b=6356514.966395495 +units=m +no_defs ");
            CapeUTMZone35S = new ProjectionInfo("+proj=utm +zone=35 +south +a=6378249.145 +b=6356514.966395495 +units=m +no_defs ");
            CapeUTMZone36S = new ProjectionInfo("+proj=utm +zone=36 +south +a=6378249.145 +b=6356514.966395495 +units=m +no_defs ");
            CarthageUTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            Combani1950UTMZone38S = new ProjectionInfo("+proj=utm +zone=38 +south +ellps=intl +units=m +no_defs ");
            Conakry1905UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            Conakry1905UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            CorregoAlegreUTMZone23S = new ProjectionInfo("+proj=utm +zone=23 +south +ellps=intl +units=m +no_defs ");
            CorregoAlegreUTMZone24S = new ProjectionInfo("+proj=utm +zone=24 +south +ellps=intl +units=m +no_defs ");
            CSG1967UTMZone22N = new ProjectionInfo("+proj=utm +zone=22 +ellps=intl +units=m +no_defs ");
            DabolaUTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=clrk80 +units=m +no_defs ");
            DabolaUTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=clrk80 +units=m +no_defs ");
            Datum73UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=intl +units=m +no_defs ");
            DoualaUTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            ED1950ED77UTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=intl +units=m +no_defs ");
            ED1950ED77UTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=intl +units=m +no_defs ");
            ED1950ED77UTMZone40N = new ProjectionInfo("+proj=utm +zone=40 +ellps=intl +units=m +no_defs ");
            ED1950ED77UTMZone41N = new ProjectionInfo("+proj=utm +zone=41 +ellps=intl +units=m +no_defs ");
            ELD1979UTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +ellps=intl +units=m +no_defs ");
            ELD1979UTMZone33N = new ProjectionInfo("+proj=utm +zone=33 +ellps=intl +units=m +no_defs ");
            ELD1979UTMZone34N = new ProjectionInfo("+proj=utm +zone=34 +ellps=intl +units=m +no_defs ");
            ELD1979UTMZone35N = new ProjectionInfo("+proj=utm +zone=35 +ellps=intl +units=m +no_defs ");
            ETRF1989UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=WGS84 +units=m +no_defs ");
            ETRF1989UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=WGS84 +units=m +no_defs ");
            ETRF1989UTMZone30N = new ProjectionInfo("+proj=utm +zone=30 +ellps=WGS84 +units=m +no_defs ");
            ETRF1989UTMZone31N = new ProjectionInfo("+proj=utm +zone=31 +ellps=WGS84 +units=m +no_defs ");
            ETRF1989UTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +ellps=WGS84 +units=m +no_defs ");
            ETRF1989UTMZone33N = new ProjectionInfo("+proj=utm +zone=33 +ellps=WGS84 +units=m +no_defs ");
            ETRF1989UTMZone34N = new ProjectionInfo("+proj=utm +zone=34 +ellps=WGS84 +units=m +no_defs ");
            ETRF1989UTMZone35N = new ProjectionInfo("+proj=utm +zone=35 +ellps=WGS84 +units=m +no_defs ");
            ETRF1989UTMZone36N = new ProjectionInfo("+proj=utm +zone=36 +ellps=WGS84 +units=m +no_defs ");
            ETRF1989UTMZone37N = new ProjectionInfo("+proj=utm +zone=37 +ellps=WGS84 +units=m +no_defs ");
            ETRF1989UTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=WGS84 +units=m +no_defs ");
            ETRS1989UTMZone26N = new ProjectionInfo("+proj=utm +zone=26 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone27N = new ProjectionInfo("+proj=utm +zone=27 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone30N = new ProjectionInfo("+proj=utm +zone=30 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone31N = new ProjectionInfo("+proj=utm +zone=31 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone33N = new ProjectionInfo("+proj=utm +zone=33 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone34N = new ProjectionInfo("+proj=utm +zone=34 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone35N = new ProjectionInfo("+proj=utm +zone=35 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone36N = new ProjectionInfo("+proj=utm +zone=36 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone37N = new ProjectionInfo("+proj=utm +zone=37 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=GRS80 +units=m +no_defs ");
            ETRS1989UTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=GRS80 +units=m +no_defs ");
            EuropeanDatum1950UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=intl +units=m +no_defs ");
            EuropeanDatum1950UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=intl +units=m +no_defs ");
            EuropeanDatum1950UTMZone30N = new ProjectionInfo("+proj=utm +zone=30 +ellps=intl +units=m +no_defs ");
            EuropeanDatum1950UTMZone31N = new ProjectionInfo("+proj=utm +zone=31 +ellps=intl +units=m +no_defs ");
            EuropeanDatum1950UTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +ellps=intl +units=m +no_defs ");
            EuropeanDatum1950UTMZone33N = new ProjectionInfo("+proj=utm +zone=33 +ellps=intl +units=m +no_defs ");
            EuropeanDatum1950UTMZone34N = new ProjectionInfo("+proj=utm +zone=34 +ellps=intl +units=m +no_defs ");
            EuropeanDatum1950UTMZone35N = new ProjectionInfo("+proj=utm +zone=35 +ellps=intl +units=m +no_defs ");
            EuropeanDatum1950UTMZone36N = new ProjectionInfo("+proj=utm +zone=36 +ellps=intl +units=m +no_defs ");
            EuropeanDatum1950UTMZone37N = new ProjectionInfo("+proj=utm +zone=37 +ellps=intl +units=m +no_defs ");
            EuropeanDatum1950UTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=intl +units=m +no_defs ");
            FahudUTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=clrk80 +units=m +no_defs ");
            FahudUTMZone40N = new ProjectionInfo("+proj=utm +zone=40 +ellps=clrk80 +units=m +no_defs ");
            FortDesaixUTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=intl +units=m +no_defs ");
            FortMarigotUTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=intl +units=m +no_defs ");
            GarouaUTMZone33N = new ProjectionInfo("+proj=utm +zone=33 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            GraciosaBaseSW1948UTMZone26N = new ProjectionInfo("+proj=utm +zone=26 +ellps=intl +units=m +no_defs ");
            GrandComorosUTMZone38S = new ProjectionInfo("+proj=utm +zone=38 +south +ellps=intl +units=m +no_defs ");
            HitoXVIII1963UTMZone19S = new ProjectionInfo("+proj=utm +zone=19 +south +ellps=intl +units=m +no_defs ");
            Hjorsey1955UTMZone26N = new ProjectionInfo("+proj=utm +zone=26 +ellps=intl +units=m +no_defs ");
            Hjorsey1955UTMZone27N = new ProjectionInfo("+proj=utm +zone=27 +ellps=intl +units=m +no_defs ");
            Hjorsey1955UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=intl +units=m +no_defs ");
            HongKong1980UTMZone49N = new ProjectionInfo("+proj=utm +zone=49 +ellps=intl +units=m +no_defs ");
            HongKong1980UTMZone50N = new ProjectionInfo("+proj=utm +zone=50 +ellps=intl +units=m +no_defs ");
            IGM1995UTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +ellps=WGS84 +units=m +no_defs ");
            IGM1995UTMZone33N = new ProjectionInfo("+proj=utm +zone=33 +ellps=WGS84 +units=m +no_defs ");
            IGN53MareUTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=intl +units=m +no_defs ");
            IGN56LifouUTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=intl +units=m +no_defs ");
            IGN72GrandeTerreUTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=intl +units=m +no_defs ");
            IGN72NukuHivaUTMZone7S = new ProjectionInfo("+proj=utm +zone=7 +south +ellps=intl +units=m +no_defs ");
            Indian1954UTMZone46N = new ProjectionInfo("+proj=utm +zone=46 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Indian1954UTMZone47N = new ProjectionInfo("+proj=utm +zone=47 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Indian1954UTMZone48N = new ProjectionInfo("+proj=utm +zone=48 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Indian1960UTMZone48N = new ProjectionInfo("+proj=utm +zone=48 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Indian1960UTMZone49N = new ProjectionInfo("+proj=utm +zone=49 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Indian1975UTMZone47N = new ProjectionInfo("+proj=utm +zone=47 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Indian1975UTMZone48N = new ProjectionInfo("+proj=utm +zone=48 +a=6377276.345 +b=6356075.41314024 +units=m +no_defs ");
            Indonesia1974UTMZone46N = new ProjectionInfo("+proj=utm +zone=46 +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone46S = new ProjectionInfo("+proj=utm +zone=46 +south +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone47N = new ProjectionInfo("+proj=utm +zone=47 +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone47S = new ProjectionInfo("+proj=utm +zone=47 +south +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone48N = new ProjectionInfo("+proj=utm +zone=48 +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone48S = new ProjectionInfo("+proj=utm +zone=48 +south +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone49N = new ProjectionInfo("+proj=utm +zone=49 +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone49S = new ProjectionInfo("+proj=utm +zone=49 +south +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone50N = new ProjectionInfo("+proj=utm +zone=50 +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone50S = new ProjectionInfo("+proj=utm +zone=50 +south +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone51N = new ProjectionInfo("+proj=utm +zone=51 +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone51S = new ProjectionInfo("+proj=utm +zone=51 +south +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone52N = new ProjectionInfo("+proj=utm +zone=52 +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone52S = new ProjectionInfo("+proj=utm +zone=52 +south +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone53N = new ProjectionInfo("+proj=utm +zone=53 +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone53S = new ProjectionInfo("+proj=utm +zone=53 +south +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            Indonesia1974UTMZone54S = new ProjectionInfo("+proj=utm +zone=54 +south +a=6378160 +b=6356774.50408554 +units=m +no_defs ");
            IRENET95UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=GRS80 +units=m +no_defs ");
            JGD2000UTMZone51N = new ProjectionInfo("+proj=utm +zone=51 +ellps=GRS80 +units=m +no_defs ");
            JGD2000UTMZone52N = new ProjectionInfo("+proj=utm +zone=52 +ellps=GRS80 +units=m +no_defs ");
            JGD2000UTMZone53N = new ProjectionInfo("+proj=utm +zone=53 +ellps=GRS80 +units=m +no_defs ");
            JGD2000UTMZone54N = new ProjectionInfo("+proj=utm +zone=54 +ellps=GRS80 +units=m +no_defs ");
            JGD2000UTMZone55N = new ProjectionInfo("+proj=utm +zone=55 +ellps=GRS80 +units=m +no_defs ");
            JGD2000UTMZone56N = new ProjectionInfo("+proj=utm +zone=56 +ellps=GRS80 +units=m +no_defs ");
            K01949UTMZone42S = new ProjectionInfo("+proj=utm +zone=42 +south +ellps=intl +units=m +no_defs ");
            KertauUTMZone47N = new ProjectionInfo("+proj=utm +zone=47 +a=6377304.063 +b=6356103.038993155 +units=m +no_defs ");
            KertauUTMZone48N = new ProjectionInfo("+proj=utm +zone=48 +a=6377304.063 +b=6356103.038993155 +units=m +no_defs ");
            KousseriUTMZone33N = new ProjectionInfo("+proj=utm +zone=33 +ellps=clrk80 +units=m +no_defs ");
            LaCanoaUTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=intl +units=m +no_defs ");
            LaCanoaUTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +ellps=intl +units=m +no_defs ");
            LaCanoaUTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=intl +units=m +no_defs ");
            LaCanoaUTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=intl +units=m +no_defs ");
            Locodjo1965UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=clrk80 +units=m +no_defs ");
            Locodjo1965UTMZone30N = new ProjectionInfo("+proj=utm +zone=30 +ellps=clrk80 +units=m +no_defs ");
            LomeUTMZone31N = new ProjectionInfo("+proj=utm +zone=31 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            Malongo1987UTMZone32S = new ProjectionInfo("+proj=utm +zone=32 +south +ellps=intl +units=m +no_defs ");
            Manoca1962UTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            MassawaUTMZone37N = new ProjectionInfo("+proj=utm +zone=37 +ellps=bessel +units=m +no_defs ");
            MhastUTMZone32S = new ProjectionInfo("+proj=utm +zone=32 +south +ellps=intl +units=m +no_defs ");
            MinnaUTMZone31N = new ProjectionInfo("+proj=utm +zone=31 +ellps=clrk80 +units=m +no_defs ");
            MinnaUTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +ellps=clrk80 +units=m +no_defs ");
            MOP78UTMZone1S = new ProjectionInfo("+proj=utm +zone=1 +south +ellps=intl +units=m +no_defs ");
            MoznetUTMZone36S = new ProjectionInfo("+proj=utm +zone=36 +south +ellps=WGS84 +units=m +no_defs ");
            MoznetUTMZone37S = new ProjectionInfo("+proj=utm +zone=37 +south +ellps=WGS84 +units=m +no_defs ");
            MporalokoUTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            MporalokoUTMZone32S = new ProjectionInfo("+proj=utm +zone=32 +south +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            NAD1927BLMZone14N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-99 +k=0.999600 +x_0=500000.0000000002 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927BLMZone15N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-93 +k=0.999600 +x_0=500000.0000000002 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927BLMZone16N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-87 +k=0.999600 +x_0=500000.0000000002 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927BLMZone17N = new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=-81 +k=0.999600 +x_0=500000.0000000002 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNUTMZone11N = new ProjectionInfo("+proj=utm +zone=11 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNUTMZone12N = new ProjectionInfo("+proj=utm +zone=12 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNUTMZone13N = new ProjectionInfo("+proj=utm +zone=13 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNUTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNUTMZone2S = new ProjectionInfo("+proj=utm +zone=2 +south +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNUTMZone4N = new ProjectionInfo("+proj=utm +zone=4 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNUTMZone5N = new ProjectionInfo("+proj=utm +zone=5 +ellps=GRS80 +units=m +no_defs ");
            Nahrwan1967UTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=clrk80 +units=m +no_defs ");
            Nahrwan1967UTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=clrk80 +units=m +no_defs ");
            Nahrwan1967UTMZone40N = new ProjectionInfo("+proj=utm +zone=40 +ellps=clrk80 +units=m +no_defs ");
            Naparima1955UTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=intl +units=m +no_defs ");
            Naparima1972UTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=intl +units=m +no_defs ");
            NEA74NoumeaUTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=intl +units=m +no_defs ");
            NGNUTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=WGS84 +units=m +no_defs ");
            NGNUTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=WGS84 +units=m +no_defs ");
            NGO1948UTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948UTMZone33N = new ProjectionInfo("+proj=utm +zone=33 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948UTMZone34N = new ProjectionInfo("+proj=utm +zone=34 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NGO1948UTMZone35N = new ProjectionInfo("+proj=utm +zone=35 +a=6377492.018 +b=6356173.508712696 +units=m +no_defs ");
            NordSahara1959UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=clrk80 +units=m +no_defs ");
            NordSahara1959UTMZone30N = new ProjectionInfo("+proj=utm +zone=30 +ellps=clrk80 +units=m +no_defs ");
            NordSahara1959UTMZone31N = new ProjectionInfo("+proj=utm +zone=31 +ellps=clrk80 +units=m +no_defs ");
            NordSahara1959UTMZone32N = new ProjectionInfo("+proj=utm +zone=32 +ellps=clrk80 +units=m +no_defs ");
            NZGD1949UTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=intl +units=m +no_defs ");
            NZGD1949UTMZone59S = new ProjectionInfo("+proj=utm +zone=59 +south +ellps=intl +units=m +no_defs ");
            NZGD1949UTMZone60S = new ProjectionInfo("+proj=utm +zone=60 +south +ellps=intl +units=m +no_defs ");
            NZGD2000UTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=GRS80 +units=m +no_defs ");
            NZGD2000UTMZone59S = new ProjectionInfo("+proj=utm +zone=59 +south +ellps=GRS80 +units=m +no_defs ");
            NZGD2000UTMZone60S = new ProjectionInfo("+proj=utm +zone=60 +south +ellps=GRS80 +units=m +no_defs ");
            ObservMeteorologico1939UTMZone25N = new ProjectionInfo("+proj=utm +zone=25 +ellps=intl +units=m +no_defs ");
            OldHawaiianUTMZone4N = new ProjectionInfo("+proj=utm +zone=4 +ellps=clrk66 +units=m +no_defs ");
            OldHawaiianUTMZone5N = new ProjectionInfo("+proj=utm +zone=5 +ellps=clrk66 +units=m +no_defs ");
            PDO1993UTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=clrk80 +units=m +no_defs ");
            PDO1993UTMZone40N = new ProjectionInfo("+proj=utm +zone=40 +ellps=clrk80 +units=m +no_defs ");
            PointeNoireUTMZone32S = new ProjectionInfo("+proj=utm +zone=32 +south +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            PortoSanto1936UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=intl +units=m +no_defs ");
            PortoSanto1995UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=intl +units=m +no_defs ");
            ProvSAmerDatumUTMZone17s = new ProjectionInfo("+proj=utm +zone=17 +south +ellps=intl +units=m +no_defs ");
            ProvSAmerDatumUTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=intl +units=m +no_defs ");
            ProvSAmerDatumUTMZone18S = new ProjectionInfo("+proj=utm +zone=18 +south +ellps=intl +units=m +no_defs ");
            ProvSAmerDatumUTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +ellps=intl +units=m +no_defs ");
            ProvSAmerDatumUTMZone19S = new ProjectionInfo("+proj=utm +zone=19 +south +ellps=intl +units=m +no_defs ");
            ProvSAmerDatumUTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=intl +units=m +no_defs ");
            ProvSAmerDatumUTMZone20S = new ProjectionInfo("+proj=utm +zone=20 +south +ellps=intl +units=m +no_defs ");
            ProvSAmerDatumUTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=intl +units=m +no_defs ");
            ProvSAmerDatumUTMZone22S = new ProjectionInfo("+proj=utm +zone=22 +south +ellps=intl +units=m +no_defs ");
            PuertoRicoUTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=clrk66 +units=m +no_defs ");
            Qornoq1927UTMZone22N = new ProjectionInfo("+proj=utm +zone=22 +ellps=intl +units=m +no_defs ");
            Qornoq1927UTMZone23N = new ProjectionInfo("+proj=utm +zone=23 +ellps=intl +units=m +no_defs ");
            REGVENUTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=GRS80 +units=m +no_defs ");
            REGVENUTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +ellps=GRS80 +units=m +no_defs ");
            REGVENUTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=GRS80 +units=m +no_defs ");
            RGFG1995UTMZone22N = new ProjectionInfo("+proj=utm +zone=22 +ellps=GRS80 +units=m +no_defs ");
            RGR1992UTMZone40S = new ProjectionInfo("+proj=utm +zone=40 +south +ellps=GRS80 +units=m +no_defs ");
            RRAF1991UTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=WGS84 +units=m +no_defs ");
            SainteAnneUTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=intl +units=m +no_defs ");
            SaintPierreetMiquelon1950UTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=clrk66 +units=m +no_defs ");
            SambojaUTMZone50S = new ProjectionInfo("+proj=utm +zone=50 +south +ellps=bessel +units=m +no_defs ");
            SaoBrazUTMZone26N = new ProjectionInfo("+proj=utm +zone=26 +ellps=intl +units=m +no_defs ");
            SapperHill1943UTMZone20S = new ProjectionInfo("+proj=utm +zone=20 +south +ellps=intl +units=m +no_defs ");
            SapperHill1943UTMZone21S = new ProjectionInfo("+proj=utm +zone=21 +south +ellps=intl +units=m +no_defs ");
            SchwarzeckUTMZone33S = new ProjectionInfo("+proj=utm +zone=33 +south +ellps=bess_nam +units=m +no_defs ");
            SelvagemGrande1938UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=intl +units=m +no_defs ");
            SierraLeone1968UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +ellps=clrk80 +units=m +no_defs ");
            SierraLeone1968UTMZone29N = new ProjectionInfo("+proj=utm +zone=29 +ellps=clrk80 +units=m +no_defs ");
            SIRGASUTMZone17N = new ProjectionInfo("+proj=utm +zone=17 +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone17S = new ProjectionInfo("+proj=utm +zone=17 +south +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone18S = new ProjectionInfo("+proj=utm +zone=18 +south +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone19S = new ProjectionInfo("+proj=utm +zone=19 +south +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone20S = new ProjectionInfo("+proj=utm +zone=20 +south +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone21S = new ProjectionInfo("+proj=utm +zone=21 +south +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone22N = new ProjectionInfo("+proj=utm +zone=22 +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone22S = new ProjectionInfo("+proj=utm +zone=22 +south +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone23S = new ProjectionInfo("+proj=utm +zone=23 +south +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone24S = new ProjectionInfo("+proj=utm +zone=24 +south +ellps=GRS80 +units=m +no_defs ");
            SIRGASUTMZone25S = new ProjectionInfo("+proj=utm +zone=25 +south +ellps=GRS80 +units=m +no_defs ");
            SouthAmerican1969UTMZone17S = new ProjectionInfo("+proj=utm +zone=17 +south +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone18N = new ProjectionInfo("+proj=utm +zone=18 +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone18S = new ProjectionInfo("+proj=utm +zone=18 +south +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone19N = new ProjectionInfo("+proj=utm +zone=19 +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone19S = new ProjectionInfo("+proj=utm +zone=19 +south +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone20N = new ProjectionInfo("+proj=utm +zone=20 +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone20S = new ProjectionInfo("+proj=utm +zone=20 +south +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone21S = new ProjectionInfo("+proj=utm +zone=21 +south +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone22N = new ProjectionInfo("+proj=utm +zone=22 +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone22S = new ProjectionInfo("+proj=utm +zone=22 +south +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone23S = new ProjectionInfo("+proj=utm +zone=23 +south +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone24S = new ProjectionInfo("+proj=utm +zone=24 +south +ellps=aust_SA +units=m +no_defs ");
            SouthAmerican1969UTMZone25S = new ProjectionInfo("+proj=utm +zone=25 +south +ellps=aust_SA +units=m +no_defs ");
            ST71BelepUTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=intl +units=m +no_defs ");
            ST84IledesPinsUTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=intl +units=m +no_defs ");
            ST87OuveaUTMZone58S = new ProjectionInfo("+proj=utm +zone=58 +south +ellps=intl +units=m +no_defs ");
            SudanUTMZone35N = new ProjectionInfo("+proj=utm +zone=35 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            SudanUTMZone36N = new ProjectionInfo("+proj=utm +zone=36 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            TahaaUTMZone5S = new ProjectionInfo("+proj=utm +zone=5 +south +ellps=intl +units=m +no_defs ");
            TahitiUTMZone6S = new ProjectionInfo("+proj=utm +zone=6 +south +ellps=intl +units=m +no_defs ");
            Tananarive1925UTMZone38S = new ProjectionInfo("+proj=utm +zone=38 +south +ellps=intl +units=m +no_defs ");
            Tananarive1925UTMZone39S = new ProjectionInfo("+proj=utm +zone=39 +south +ellps=intl +units=m +no_defs ");
            TeteUTMZone36S = new ProjectionInfo("+proj=utm +zone=36 +south +ellps=clrk66 +units=m +no_defs ");
            TeteUTMZone37S = new ProjectionInfo("+proj=utm +zone=37 +south +ellps=clrk66 +units=m +no_defs ");
            Timbalai1948UTMZone49N = new ProjectionInfo("+proj=utm +zone=49 +ellps=evrstSS +units=m +no_defs ");
            Timbalai1948UTMZone50N = new ProjectionInfo("+proj=utm +zone=50 +ellps=evrstSS +units=m +no_defs ");
            TokyoUTMZone51N = new ProjectionInfo("+proj=utm +zone=51 +ellps=bessel +units=m +no_defs ");
            TokyoUTMZone52N = new ProjectionInfo("+proj=utm +zone=52 +ellps=bessel +units=m +no_defs ");
            TokyoUTMZone53N = new ProjectionInfo("+proj=utm +zone=53 +ellps=bessel +units=m +no_defs ");
            TokyoUTMZone54N = new ProjectionInfo("+proj=utm +zone=54 +ellps=bessel +units=m +no_defs ");
            TokyoUTMZone55N = new ProjectionInfo("+proj=utm +zone=55 +ellps=bessel +units=m +no_defs ");
            TokyoUTMZone56N = new ProjectionInfo("+proj=utm +zone=56 +ellps=bessel +units=m +no_defs ");
            TrucialCoast1948UTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=helmert +units=m +no_defs ");
            TrucialCoast1948UTMZone40N = new ProjectionInfo("+proj=utm +zone=40 +ellps=helmert +units=m +no_defs ");
            YemenNGN1996UTMZone38N = new ProjectionInfo("+proj=utm +zone=38 +ellps=WGS84 +units=m +no_defs ");
            YemenNGN1996UTMZone39N = new ProjectionInfo("+proj=utm +zone=39 +ellps=WGS84 +units=m +no_defs ");
            Yoff1972UTMZone28N = new ProjectionInfo("+proj=utm +zone=28 +a=6378249.2 +b=6356514.999904194 +units=m +no_defs ");
            Zanderij1972UTMZone21N = new ProjectionInfo("+proj=utm +zone=21 +ellps=intl +units=m +no_defs ");


        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591