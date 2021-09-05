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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:08:55 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591
namespace MeteoInfoC.Projections.GeographicCategories
{


    /// <summary>
    /// Europe
    /// </summary>
    public class Europe : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Albanian1987;
        public readonly ProjectionInfo ATFParis;
        public readonly ProjectionInfo Belge1950Brussels;
        public readonly ProjectionInfo Belge1972;
        public readonly ProjectionInfo Bern1898;
        public readonly ProjectionInfo Bern1898Bern;
        public readonly ProjectionInfo Bern1938;
        public readonly ProjectionInfo CH1903;
        public readonly ProjectionInfo Datum73;
        public readonly ProjectionInfo DatumLisboaBessel;
        public readonly ProjectionInfo DatumLisboaHayford;
        public readonly ProjectionInfo DealulPiscului1933Romania;
        public readonly ProjectionInfo DealulPiscului1970Romania;
        public readonly ProjectionInfo DeutscheHauptdreiecksnetz;
        public readonly ProjectionInfo DutchRD;
        public readonly ProjectionInfo Estonia1937;
        public readonly ProjectionInfo Estonia1992;
        public readonly ProjectionInfo Estonia1997;
        public readonly ProjectionInfo ETRF1989;
        public readonly ProjectionInfo ETRS1989;
        public readonly ProjectionInfo EUREFFIN;
        public readonly ProjectionInfo European1979;
        public readonly ProjectionInfo EuropeanDatum1950;
        public readonly ProjectionInfo EuropeanDatum1987;
        public readonly ProjectionInfo Greek;
        public readonly ProjectionInfo GreekAthens;
        public readonly ProjectionInfo GreekGeodeticRefSystem1987;
        public readonly ProjectionInfo Hermannskogel;
        public readonly ProjectionInfo Hjorsey1955;
        public readonly ProjectionInfo HungarianDatum1972;
        public readonly ProjectionInfo IRENET95;
        public readonly ProjectionInfo ISN1993;
        public readonly ProjectionInfo Kartastokoordinaattijarjestelma;
        public readonly ProjectionInfo Lisbon;
        public readonly ProjectionInfo LisbonLisbon;
        public readonly ProjectionInfo Lisbon1890;
        public readonly ProjectionInfo Lisbon1890Lisbon;
        public readonly ProjectionInfo LKS1992;
        public readonly ProjectionInfo LKS1994;
        public readonly ProjectionInfo Luxembourg1930;
        public readonly ProjectionInfo Madrid1870Madrid;
        public readonly ProjectionInfo MGIFerro;
        public readonly ProjectionInfo MilitarGeographischeInstitut;
        public readonly ProjectionInfo MonteMario;
        public readonly ProjectionInfo MonteMarioRome;
        public readonly ProjectionInfo NGO1948;
        public readonly ProjectionInfo NGO1948Oslo;
        public readonly ProjectionInfo NorddeGuerreParis;
        public readonly ProjectionInfo NouvelleTriangulationFrancaise;
        public readonly ProjectionInfo NTFParis;
        public readonly ProjectionInfo OSSN1980;
        public readonly ProjectionInfo OSGB1936;
        public readonly ProjectionInfo OSGB1970SN;
        public readonly ProjectionInfo OSNI1952;
        public readonly ProjectionInfo Pulkovo1942;
        public readonly ProjectionInfo Pulkovo1942Adj1958;
        public readonly ProjectionInfo Pulkovo1942Adj1983;
        public readonly ProjectionInfo Pulkovo1995;
        public readonly ProjectionInfo Qornoq;
        public readonly ProjectionInfo ReseauNationalBelge1950;
        public readonly ProjectionInfo ReseauNationalBelge1972;
        public readonly ProjectionInfo Reykjavik1900;
        public readonly ProjectionInfo RGF1993;
        public readonly ProjectionInfo Roma1940;
        public readonly ProjectionInfo RT1990;
        public readonly ProjectionInfo RT38;
        public readonly ProjectionInfo RT38Stockholm;
        public readonly ProjectionInfo S42Hungary;
        public readonly ProjectionInfo SJTSK;
        public readonly ProjectionInfo SWEREF99;
        public readonly ProjectionInfo SwissTRF1995;
        public readonly ProjectionInfo TM65;
        public readonly ProjectionInfo TM75;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Europe
        /// </summary>
        public Europe()
        {
            Albanian1987 = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            ATFParis = new ProjectionInfo("+proj=longlat +a=6376523 +b=6355862.933255573 +pm=2.337229166666667 +no_defs ");
            Belge1950Brussels = new ProjectionInfo("+proj=longlat +ellps=intl +pm=4.367975 +no_defs ");
            Belge1972 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Bern1898 = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            Bern1898Bern = new ProjectionInfo("+proj=longlat +ellps=bessel +pm=7.439583333333333 +no_defs ");
            Bern1938 = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            CH1903 = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            Datum73 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            DatumLisboaBessel = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            DatumLisboaHayford = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            DealulPiscului1933Romania = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            DealulPiscului1970Romania = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            DeutscheHauptdreiecksnetz = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            DutchRD = new ProjectionInfo("+proj=sterea +lat_0=52.15616055999986 +lon_0=5.387638888999871 +k=0.999908 +x_0=155000 +y_0=463000 +ellps=bessel +units=m +no_defs");
            DutchRD = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            Estonia1937 = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            Estonia1992 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            Estonia1997 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            ETRF1989 = new ProjectionInfo("+proj=longlat +ellps=WGS84 +no_defs ");
            ETRS1989 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            EUREFFIN = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            European1979 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            EuropeanDatum1950 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            EuropeanDatum1987 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Greek = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            GreekAthens = new ProjectionInfo("+proj=longlat +ellps=bessel +pm=23.7163375 +no_defs ");
            GreekGeodeticRefSystem1987 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            Hermannskogel = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            Hjorsey1955 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            HungarianDatum1972 = new ProjectionInfo("+proj=longlat +ellps=GRS67 +no_defs ");
            IRENET95 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            ISN1993 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            Kartastokoordinaattijarjestelma = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Lisbon = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            LisbonLisbon = new ProjectionInfo("+proj=longlat +ellps=intl +pm=-9.131906111111112 +no_defs ");
            Lisbon1890 = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            Lisbon1890Lisbon = new ProjectionInfo("+proj=longlat +ellps=bessel +pm=-9.131906111111112 +no_defs ");
            LKS1992 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            LKS1994 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            Luxembourg1930 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Madrid1870Madrid = new ProjectionInfo("+proj=longlat +a=6378298.3 +b=6356657.142669561 +pm=-3.687938888888889 +no_defs ");
            MGIFerro = new ProjectionInfo("+proj=longlat +ellps=bessel +pm=-17.66666666666667 +no_defs ");
            MilitarGeographischeInstitut = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            MonteMario = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            MonteMarioRome = new ProjectionInfo("+proj=longlat +ellps=intl +pm=12.45233333333333 +no_defs ");
            NGO1948 = new ProjectionInfo("+proj=longlat +a=6377492.018 +b=6356173.508712696 +no_defs ");
            NGO1948Oslo = new ProjectionInfo("+proj=longlat +a=6377492.018 +b=6356173.508712696 +pm=10.72291666666667 +no_defs ");
            NorddeGuerreParis = new ProjectionInfo("+proj=longlat +a=6376523 +b=6355862.933255573 +pm=2.337229166666667 +no_defs ");
            NouvelleTriangulationFrancaise = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            NTFParis = new ProjectionInfo("+proj=longlat +a=6378249.2 +b=6356514.999904194 +pm=2.337229166666667 +no_defs ");
            OSSN1980 = new ProjectionInfo("+proj=longlat +ellps=airy +no_defs ");
            OSGB1936 = new ProjectionInfo("+proj=longlat +ellps=airy +no_defs ");
            OSGB1970SN = new ProjectionInfo("+proj=longlat +ellps=airy +no_defs ");
            OSNI1952 = new ProjectionInfo("+proj=longlat +ellps=airy +no_defs ");
            Pulkovo1942 = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            Pulkovo1942Adj1958 = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            Pulkovo1942Adj1983 = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            Pulkovo1995 = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            Qornoq = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            ReseauNationalBelge1950 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            ReseauNationalBelge1972 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            Reykjavik1900 = new ProjectionInfo("+proj=longlat +a=6377019.27 +b=6355762.5391 +no_defs ");
            RGF1993 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            Roma1940 = new ProjectionInfo("+proj=longlat +ellps=intl +no_defs ");
            RT1990 = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            RT38 = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            RT38Stockholm = new ProjectionInfo("+proj=longlat +ellps=bessel +pm=18.05827777777778 +no_defs ");
            S42Hungary = new ProjectionInfo("+proj=longlat +ellps=krass +no_defs ");
            SJTSK = new ProjectionInfo("+proj=longlat +ellps=bessel +no_defs ");
            SWEREF99 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            SwissTRF1995 = new ProjectionInfo("+proj=longlat +ellps=GRS80 +no_defs ");
            TM65 = new ProjectionInfo("+proj=longlat +a=6377340.189 +b=6356034.447938534 +no_defs ");
            TM75 = new ProjectionInfo("+proj=longlat +a=6377340.189 +b=6356034.447938534 +no_defs ");


        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591