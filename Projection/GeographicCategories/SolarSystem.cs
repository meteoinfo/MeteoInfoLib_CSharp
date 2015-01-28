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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:16:32 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


#pragma warning disable 1591
namespace MeteoInfoC.Projections.GeographicCategories
{


    /// <summary>
    /// SolarSystem
    /// </summary>
    public class SolarSystem : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo Adrastea2000;
        public readonly ProjectionInfo Amalthea2000;
        public readonly ProjectionInfo Ananke2000;
        public readonly ProjectionInfo Ariel2000;
        public readonly ProjectionInfo Atlas2000;
        public readonly ProjectionInfo Belinda2000;
        public readonly ProjectionInfo Bianca2000;
        public readonly ProjectionInfo Callisto2000;
        public readonly ProjectionInfo Calypso2000;
        public readonly ProjectionInfo Carme2000;
        public readonly ProjectionInfo Charon2000;
        public readonly ProjectionInfo Cordelia2000;
        public readonly ProjectionInfo Cressida2000;
        public readonly ProjectionInfo Deimos2000;
        public readonly ProjectionInfo Desdemona2000;
        public readonly ProjectionInfo Despina2000;
        public readonly ProjectionInfo Dione2000;
        public readonly ProjectionInfo Elara2000;
        public readonly ProjectionInfo Enceladus2000;
        public readonly ProjectionInfo Epimetheus2000;
        public readonly ProjectionInfo Europa2000;
        public readonly ProjectionInfo Galatea2000;
        public readonly ProjectionInfo Ganymede2000;
        public readonly ProjectionInfo Helene2000;
        public readonly ProjectionInfo Himalia2000;
        public readonly ProjectionInfo Hyperion2000;
        public readonly ProjectionInfo Iapetus2000;
        public readonly ProjectionInfo Io2000;
        public readonly ProjectionInfo Janus2000;
        public readonly ProjectionInfo Juliet2000;
        public readonly ProjectionInfo Jupiter2000;
        public readonly ProjectionInfo Larissa2000;
        public readonly ProjectionInfo Leda2000;
        public readonly ProjectionInfo Lysithea2000;
        public readonly ProjectionInfo Mars1979;
        public readonly ProjectionInfo Mars2000;
        public readonly ProjectionInfo Mercury2000;
        public readonly ProjectionInfo Metis2000;
        public readonly ProjectionInfo Mimas2000;
        public readonly ProjectionInfo Miranda2000;
        public readonly ProjectionInfo Moon2000;
        public readonly ProjectionInfo Naiad2000;
        public readonly ProjectionInfo Neptune2000;
        public readonly ProjectionInfo Nereid2000;
        public readonly ProjectionInfo Oberon2000;
        public readonly ProjectionInfo Ophelia2000;
        public readonly ProjectionInfo Pan2000;
        public readonly ProjectionInfo Pandora2000;
        public readonly ProjectionInfo Pasiphae2000;
        public readonly ProjectionInfo Phobos2000;
        public readonly ProjectionInfo Phoebe2000;
        public readonly ProjectionInfo Pluto2000;
        public readonly ProjectionInfo Portia2000;
        public readonly ProjectionInfo Prometheus2000;
        public readonly ProjectionInfo Proteus2000;
        public readonly ProjectionInfo Puck2000;
        public readonly ProjectionInfo Rhea2000;
        public readonly ProjectionInfo Rosalind2000;
        public readonly ProjectionInfo Saturn2000;
        public readonly ProjectionInfo Sinope2000;
        public readonly ProjectionInfo Telesto2000;
        public readonly ProjectionInfo Tethys2000;
        public readonly ProjectionInfo Thalassa2000;
        public readonly ProjectionInfo Thebe2000;
        public readonly ProjectionInfo Titan2000;
        public readonly ProjectionInfo Titania2000;
        public readonly ProjectionInfo Triton2000;
        public readonly ProjectionInfo Umbriel2000;
        public readonly ProjectionInfo Uranus2000;
        public readonly ProjectionInfo Venus1985;
        public readonly ProjectionInfo Venus2000;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SolarSystem
        /// </summary>
        public SolarSystem()
        {
            Adrastea2000 = new ProjectionInfo("+proj=longlat +a=8200 +b=8200 +no_defs ");
            Amalthea2000 = new ProjectionInfo("+proj=longlat +a=83500 +b=83500 +no_defs ");
            Ananke2000 = new ProjectionInfo("+proj=longlat +a=10000 +b=10000 +no_defs ");
            Ariel2000 = new ProjectionInfo("+proj=longlat +a=578900 +b=578900 +no_defs ");
            Atlas2000 = new ProjectionInfo("+proj=longlat +a=16000 +b=16000 +no_defs ");
            Belinda2000 = new ProjectionInfo("+proj=longlat +a=33000 +b=33000 +no_defs ");
            Bianca2000 = new ProjectionInfo("+proj=longlat +a=21000 +b=21000 +no_defs ");
            Callisto2000 = new ProjectionInfo("+proj=longlat +a=2409300 +b=2409300 +no_defs ");
            Calypso2000 = new ProjectionInfo("+proj=longlat +a=9500 +b=9500 +no_defs ");
            Carme2000 = new ProjectionInfo("+proj=longlat +a=15000 +b=15000 +no_defs ");
            Charon2000 = new ProjectionInfo("+proj=longlat +a=593000 +b=593000 +no_defs ");
            Cordelia2000 = new ProjectionInfo("+proj=longlat +a=13000 +b=13000 +no_defs ");
            Cressida2000 = new ProjectionInfo("+proj=longlat +a=31000 +b=31000 +no_defs ");
            Deimos2000 = new ProjectionInfo("+proj=longlat +a=6200 +b=6200 +no_defs ");
            Desdemona2000 = new ProjectionInfo("+proj=longlat +a=27000 +b=27000 +no_defs ");
            Despina2000 = new ProjectionInfo("+proj=longlat +a=74000 +b=74000 +no_defs ");
            Dione2000 = new ProjectionInfo("+proj=longlat +a=560000 +b=560000 +no_defs ");
            Elara2000 = new ProjectionInfo("+proj=longlat +a=40000 +b=40000 +no_defs ");
            Enceladus2000 = new ProjectionInfo("+proj=longlat +a=249400 +b=249400 +no_defs ");
            Epimetheus2000 = new ProjectionInfo("+proj=longlat +a=59500 +b=59500 +no_defs ");
            Europa2000 = new ProjectionInfo("+proj=longlat +a=1562090 +b=1562090 +no_defs ");
            Galatea2000 = new ProjectionInfo("+proj=longlat +a=79000 +b=79000 +no_defs ");
            Ganymede2000 = new ProjectionInfo("+proj=longlat +a=2632345 +b=2632345 +no_defs ");
            Helene2000 = new ProjectionInfo("+proj=longlat +a=17500 +b=700.0000000000046 +no_defs ");
            Himalia2000 = new ProjectionInfo("+proj=longlat +a=85000 +b=85000 +no_defs ");
            Hyperion2000 = new ProjectionInfo("+proj=longlat +a=133000 +b=133000 +no_defs ");
            Iapetus2000 = new ProjectionInfo("+proj=longlat +a=718000 +b=718000 +no_defs ");
            Io2000 = new ProjectionInfo("+proj=longlat +a=1821460 +b=1821460 +no_defs ");
            Janus2000 = new ProjectionInfo("+proj=longlat +a=888000 +b=888000 +no_defs ");
            Juliet2000 = new ProjectionInfo("+proj=longlat +a=42000 +b=42000 +no_defs ");
            Jupiter2000 = new ProjectionInfo("+proj=longlat +a=71492000 +b=66853999.99999999 +no_defs ");
            Larissa2000 = new ProjectionInfo("+proj=longlat +a=104000 +b=89000 +no_defs ");
            Leda2000 = new ProjectionInfo("+proj=longlat +a=5000 +b=5000 +no_defs ");
            Lysithea2000 = new ProjectionInfo("+proj=longlat +a=12000 +b=12000 +no_defs ");
            Mars1979 = new ProjectionInfo("+proj=longlat +a=3393400 +b=3375730 +no_defs ");
            Mars2000 = new ProjectionInfo("+proj=longlat +a=3396190 +b=3376200 +no_defs ");
            Mercury2000 = new ProjectionInfo("+proj=longlat +a=2439700 +b=2439700 +no_defs ");
            Metis2000 = new ProjectionInfo("+proj=longlat +a=30000 +b=20000 +no_defs ");
            Mimas2000 = new ProjectionInfo("+proj=longlat +a=1986300 +b=1986300 +no_defs ");
            Miranda2000 = new ProjectionInfo("+proj=longlat +a=235800 +b=235800 +no_defs ");
            Moon2000 = new ProjectionInfo("+proj=longlat +a=1737400 +b=1737400 +no_defs ");
            Naiad2000 = new ProjectionInfo("+proj=longlat +a=29000 +b=29000 +no_defs ");
            Neptune2000 = new ProjectionInfo("+proj=longlat +a=24764000 +b=24341000 +no_defs ");
            Nereid2000 = new ProjectionInfo("+proj=longlat +a=170000 +b=170000 +no_defs ");
            Oberon2000 = new ProjectionInfo("+proj=longlat +a=761400 +b=761400 +no_defs ");
            Ophelia2000 = new ProjectionInfo("+proj=longlat +a=15000 +b=15000 +no_defs ");
            Pan2000 = new ProjectionInfo("+proj=longlat +a=10000 +b=10000 +no_defs ");
            Pandora2000 = new ProjectionInfo("+proj=longlat +a=41900 +b=41900 +no_defs ");
            Pasiphae2000 = new ProjectionInfo("+proj=longlat +a=18000 +b=18000 +no_defs ");
            Phobos2000 = new ProjectionInfo("+proj=longlat +a=11100 +b=11100 +no_defs ");
            Phoebe2000 = new ProjectionInfo("+proj=longlat +a=110000 +b=110000 +no_defs ");
            Pluto2000 = new ProjectionInfo("+proj=longlat +a=1195000 +b=1195000 +no_defs ");
            Portia2000 = new ProjectionInfo("+proj=longlat +a=54000 +b=54000 +no_defs ");
            Prometheus2000 = new ProjectionInfo("+proj=longlat +a=50100 +b=50100 +no_defs ");
            Proteus2000 = new ProjectionInfo("+proj=longlat +a=208000 +b=208000 +no_defs ");
            Puck2000 = new ProjectionInfo("+proj=longlat +a=77000 +b=77000 +no_defs ");
            Rhea2000 = new ProjectionInfo("+proj=longlat +a=764000 +b=764000 +no_defs ");
            Rosalind2000 = new ProjectionInfo("+proj=longlat +a=27000 +b=27000 +no_defs ");
            Saturn2000 = new ProjectionInfo("+proj=longlat +a=60268000 +b=54364000 +no_defs ");
            Sinope2000 = new ProjectionInfo("+proj=longlat +a=14000 +b=14000 +no_defs ");
            Telesto2000 = new ProjectionInfo("+proj=longlat +a=11000 +b=11000 +no_defs ");
            Tethys2000 = new ProjectionInfo("+proj=longlat +a=529800 +b=529800 +no_defs ");
            Thalassa2000 = new ProjectionInfo("+proj=longlat +a=40000 +b=40000 +no_defs ");
            Thebe2000 = new ProjectionInfo("+proj=longlat +a=49300 +b=49300 +no_defs ");
            Titan2000 = new ProjectionInfo("+proj=longlat +a=2575000 +b=2575000 +no_defs ");
            Titania2000 = new ProjectionInfo("+proj=longlat +a=788900 +b=788900 +no_defs ");
            Triton2000 = new ProjectionInfo("+proj=longlat +a=1352600 +b=1352600 +no_defs ");
            Umbriel2000 = new ProjectionInfo("+proj=longlat +a=584700 +b=584700 +no_defs ");
            Uranus2000 = new ProjectionInfo("+proj=longlat +a=25559000 +b=24973000 +no_defs ");
            Venus1985 = new ProjectionInfo("+proj=longlat +a=6051000 +b=6051000 +no_defs ");
            Venus2000 = new ProjectionInfo("+proj=longlat +a=6051800 +b=6051800 +no_defs ");

        }

        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
#pragma warning restore 1591