using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 1 parameter table
    /// </summary>
    public class GRIBParameterTable
    {
        #region Variables
        /// <summary>
        /// Table file name
        /// </summary>
        public string FileName;
        /// <summary>
        /// Center id
        /// </summary>
        public int CenterID;
        /// <summary>
        /// Sub center id
        /// </summary>
        public int SubCenterID;
        /// <summary>
        /// Table edition number
        /// </summary>
        public int TableNum;
        /// <summary>
        /// Parameters
        /// </summary>
        public Parameter[] Parameters;        

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">file name</param>
        /// <param name="cen">center id</param>
        /// <param name="sub">subcenter id</param>
        /// <param name="tab">table number</param>
        /// <param name="par">parameter tables</param>
        public GRIBParameterTable(String name, int cen, int sub, int tab, Parameter[] par)
        {
            FileName = name;
            CenterID = cen;
            SubCenterID = sub;
            TableNum = tab;
            Parameters = par;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get default parameter
        /// </summary>
        /// <param name="paramNum">parameter number</param>
        /// <returns>parameter</returns>
        public static Variable GetDefaultParameter(int paramNum)
        {
            string[][] defaulttable_ncep_reanal2 = new string[][]
            {
                           /*   0 */   new string[] {"var0", "undefined", "undefined"},
                           /*   1 */   new string[] {"pres", "Pressure", "Pa"},
                           /*   2 */   new string[] {"prmsl", "Pressure reduced to MSL", "Pa"},
                           /*   3 */   new string[] {"ptend", "Pressure tendency", "Pa/s"},
                           /*   4 */   new string[] {"var4", "undefined", "undefined"},
                           /*   5 */   new string[] {"var5", "undefined", "undefined"},
                           /*   6 */   new string[] {"gp", "Geopotential", "m^2/s^2"},
                           /*   7 */   new string[] {"hgt", "Geopotential height", "gpm"},
                           /*   8 */   new string[] {"dist", "Geometric height", "m"},
                           /*   9 */   new string[] {"hstdv", "Std dev of height", "m"},
                           /*  10 */   new string[] {"hvar", "Varianance of height", "m^2"},
                           /*  11 */   new string[] {"tmp", "Temperature", "K"},
                           /*  12 */   new string[] {"vtmp", "Virtual temperature", "K"},
                           /*  13 */   new string[] {"pot", "Potential temperature", "K"},
                           /*  14 */   new string[] {"epot", "Pseudo-adiabatic pot. temperature", "K"},
                           /*  15 */   new string[] {"tmax", "Max. temperature", "K"},
                           /*  16 */   new string[] {"tmin", "Min. temperature", "K"},
                           /*  17 */   new string[] {"dpt", "Dew point temperature", "K"},
                           /*  18 */   new string[] {"depr", "Dew point depression", "K"},
                           /*  19 */   new string[] {"lapr", "Lapse rate", "K/m"},
                           /*  20 */   new string[] {"visib", "Visibility", "m"},
                           /*  21 */   new string[] {"rdsp1", "Radar spectra (1)", ""},
                           /*  22 */   new string[] {"rdsp2", "Radar spectra (2)", ""},
                           /*  23 */   new string[] {"rdsp3", "Radar spectra (3)", ""},
                           /*  24 */   new string[] {"var24", "undefined", "undefined"},
                           /*  25 */   new string[] {"tmpa", "Temperature anomaly", "K"},
                           /*  26 */   new string[] {"presa", "Pressure anomaly", "Pa"},
                           /*  27 */   new string[] {"gpa", "Geopotential height anomaly", "gpm"},
                           /*  28 */   new string[] {"wvsp1", "Wave spectra (1)", ""},
                           /*  29 */   new string[] {"wvsp2", "Wave spectra (2)", ""},
                           /*  30 */   new string[] {"wvsp3", "Wave spectra (3)", ""},
                           /*  31 */   new string[] {"wdir", "Wind direction", "deg"},
                           /*  32 */   new string[] {"wind", "Wind speed", "m/s"},
                           /*  33 */   new string[] {"ugrd", "u wind", "m/s"},
                           /*  34 */   new string[] {"vgrd", "v wind", "m/s"},
                           /*  35 */   new string[] {"strm", "Stream function", "m^2/s"},
                           /*  36 */   new string[] {"vpot", "Velocity potential", "m^2/s"},
                           /*  37 */   new string[] {"mntsf", "Montgomery stream function", "m^2/s^2"},
                           /*  38 */   new string[] {"sgcvv", "Sigma coord. vertical velocity", "/s"},
                           /*  39 */   new string[] {"vvel", "Pressure vertical velocity", "Pa/s"},
                           /*  40 */   new string[] {"dzdt", "Geometric vertical velocity", "m/s"},
                           /*  41 */   new string[] {"absv", "Absolute vorticity", "/s"},
                           /*  42 */   new string[] {"absd", "Absolute divergence", "/s"},
                           /*  43 */   new string[] {"relv", "Relative vorticity", "/s"},
                           /*  44 */   new string[] {"reld", "Relative divergence", "/s"},
                           /*  45 */   new string[] {"vucsh", "Vertical u shear", "/s"},
                           /*  46 */   new string[] {"vvcsh", "Vertical v shear", "/s"},
                           /*  47 */   new string[] {"dirc", "Direction of current", "deg"},
                           /*  48 */   new string[] {"spc", "Speed of current", "m/s"},
                           /*  49 */   new string[] {"uogrd", "u of current", "m/s"},
                           /*  50 */   new string[] {"vogrd", "v of current", "m/s"},
                           /*  51 */   new string[] {"spfh", "Specific humidity", "kg/kg"},
                           /*  52 */   new string[] {"rh", "Relative humidity", "%"},
                           /*  53 */   new string[] {"mixr", "Humidity mixing ratio", "kg/kg"},
                           /*  54 */   new string[] {"pwat", "Precipitable water", "kg/m^2"},
                           /*  55 */   new string[] {"vapp", "Vapor pressure", "Pa"},
                           /*  56 */   new string[] {"satd", "Saturation deficit", "Pa"},
                           /*  57 */   new string[] {"evp", "Evaporation", "kg/m^2"},
                           /*  58 */   new string[] {"cice", "Cloud Ice", "kg/m^2"},
                           /*  59 */   new string[] {"prate", "Precipitation rate", "kg/m^2/s"},
                           /*  60 */   new string[] {"tstm", "Thunderstorm probability", "%"},
                           /*  61 */   new string[] {"apcp", "Total precipitation", "kg/m^2"},
                           /*  62 */   new string[] {"ncpcp", "Large scale precipitation", "kg/m^2"},
                           /*  63 */   new string[] {"acpcp", "Convective precipitation", "kg/m^2"},
                           /*  64 */   new string[] {"srweq", "Snowfall rate water equiv.", "kg/m^2/s"},
                           /*  65 */   new string[] {"weasd", "Accum. snow", "kg/m^2"},
                           /*  66 */   new string[] {"snod", "Snow depth", "m"},
                           /*  67 */   new string[] {"mixht", "Mixed layer depth", "m"},
                           /*  68 */   new string[] {"tthdp", "Transient thermocline depth", "m"},
                           /*  69 */   new string[] {"mthd", "Main thermocline depth", "m"},
                           /*  70 */   new string[] {"mtha", "Main thermocline anomaly", "m"},
                           /*  71 */   new string[] {"tcdc", "Total cloud cover", "%"},
                           /*  72 */   new string[] {"cdcon", "Convective cloud cover", "%"},
                           /*  73 */   new string[] {"lcdc", "Low level cloud cover", "%"},
                           /*  74 */   new string[] {"mcdc", "Mid level cloud cover", "%"},
                           /*  75 */   new string[] {"hcdc", "High level cloud cover", "%"},
                           /*  76 */   new string[] {"cwat", "Cloud water", "kg/m^2"},
                           /*  77 */   new string[] {"var77", "undefined", "undefined"},
                           /*  78 */   new string[] {"snoc", "Convective snow", "kg/m^2"},
                           /*  79 */   new string[] {"snol", "Large scale snow", "kg/m^2"},
                           /*  80 */   new string[] {"wtmp", "Water temperature", "K"},
                           /*  81 */   new string[] {"land", "Land cover (land=1;sea=0)", "fraction"},
                           /*  82 */   new string[] {"dslm", "Deviation of sea level from mean", "m"},
                           /*  83 */   new string[] {"sfcr", "Surface roughness", "m"},
                           /*  84 */   new string[] {"albdo", "Albedo", "%"},
                           /*  85 */   new string[] {"tsoil", "Soil temperature", "K"},
                           /*  86 */   new string[] {"soilm", "Soil moisture content", "kg/m^2"},
                           /*  87 */   new string[] {"veg", "Vegetation", "%"},
                           /*  88 */   new string[] {"salty", "Salinity", "kg/kg"},
                           /*  89 */   new string[] {"den", "Density", "kg/m^3"},
                           /*  90 */   new string[] {"runof", "Runoff", "kg/m^2"},
                           /*  91 */   new string[] {"icec", "Ice concentration (ice=1;no ice=0)", "fraction"},
                           /*  92 */   new string[] {"icetk", "Ice thickness", "m"},
                           /*  93 */   new string[] {"diced", "Direction of ice drift", "deg"},
                           /*  94 */   new string[] {"siced", "Speed of ice drift", "m/s"},
                           /*  95 */   new string[] {"uice", "u of ice drift", "m/s"},
                           /*  96 */   new string[] {"vice", "v of ice drift", "m/s"},
                           /*  97 */   new string[] {"iceg", "Ice growth rate", "m/s"},
                           /*  98 */   new string[] {"iced", "Ice divergence", "/s"},
                           /*  99 */   new string[] {"snom", "Snow melt", "kg/m^2"},
                           /* 100 */   new string[] {"htsgw", "Sig height of wind waves and swell", "m"},
                           /* 101 */   new string[] {"wvdir", "Direction of wind waves", "deg"},
                           /* 102 */   new string[] {"wvhgt", "Sig height of wind waves", "m"},
                           /* 103 */   new string[] {"wvper", "Mean period of wind waves", "s"},
                           /* 104 */   new string[] {"swdir", "Direction of swell waves", "deg"},
                           /* 105 */   new string[] {"swell", "Sig height of swell waves", "m"},
                           /* 106 */   new string[] {"swper", "Mean period of swell waves", "s"},
                           /* 107 */   new string[] {"dirpw", "Primary wave direction", "deg"},
                           /* 108 */   new string[] {"perpw", "Primary wave mean period", "s"},
                           /* 109 */   new string[] {"dirsw", "Secondary wave direction", "deg"},
                           /* 110 */   new string[] {"persw", "Secondary wave mean period", "s"},
                           /* 111 */   new string[] {"nswrs", "Net short wave (surface)", "W/m^2"},
                           /* 112 */   new string[] {"nlwrs", "Net long wave (surface)", "W/m^2"},
                           /* 113 */   new string[] {"nswrt", "Net short wave (top)", "W/m^2"},
                           /* 114 */   new string[] {"nlwrt", "Net long wave (top)", "W/m^2"},
                           /* 115 */   new string[] {"lwavr", "Long wave", "W/m^2"},
                           /* 116 */   new string[] {"swavr", "Short wave", "W/m^2"},
                           /* 117 */   new string[] {"grad", "Global radiation", "W/m^2"},
                           /* 118 */   new string[] {"var118", "undefined", "undefined"},
                           /* 119 */   new string[] {"var119", "undefined", "undefined"},
                           /* 120 */   new string[] {"var120", "undefined", "undefined"},
                           /* 121 */   new string[] {"lhtfl", "Latent heat flux", "W/m^2"},
                           /* 122 */   new string[] {"shtfl", "Sensible heat flux", "W/m^2"},
                           /* 123 */   new string[] {"blydp", "Boundary layer dissipation", "W/m^2"},
                           /* 124 */   new string[] {"uflx", "Zonal momentum flux", "N/m^2"},
                           /* 125 */   new string[] {"vflx", "Meridional momentum flux", "N/m^2"},
                           /* 126 */   new string[] {"wmixe", "Wind mixing energy", "J"},
                           /* 127 */   new string[] {"imgd", "Image data", ""},
                           /* 128 */   new string[] {"mslsa", "Mean sea level pressure (Std Atm)", "Pa"},
                           /* 129 */   new string[] {"mslma", "Mean sea level pressure (MAPS)", "Pa"},
                           /* 130 */   new string[] {"mslet", "Mean sea level pressure (ETA model)", "Pa"},
                           /* 131 */   new string[] {"lftx", "Surface lifted index", "K"},
                           /* 132 */   new string[] {"4lftx", "Best (4-layer) lifted index", "K"},
                           /* 133 */   new string[] {"kx", "K index", "K"},
                           /* 134 */   new string[] {"sx", "Sweat index", "K"},
                           /* 135 */   new string[] {"mconv", "Horizontal moisture divergence", "kg/kg/s"},
                           /* 136 */   new string[] {"vssh", "Vertical speed shear", "1/s"},
                           /* 137 */   new string[] {"tslsa", "3-hr pressure tendency (Std Atmos Red)", "Pa/s"},
                           /* 138 */   new string[] {"bvf2", "Brunt-Vaisala frequency^2", "1/s^2"},
                           /* 139 */   new string[] {"pvmw", "Potential vorticity (mass-weighted)", "1/s/m"},
                           /* 140 */   new string[] {"crain", "Categorical rain", "yes=1;no=0"},
                           /* 141 */   new string[] {"cfrzr", "Categorical freezing rain", "yes=1;no=0"},
                           /* 142 */   new string[] {"cicep", "Categorical ice pellets", "yes=1;no=0"},
                           /* 143 */   new string[] {"csnow", "Categorical snow", "yes=1;no=0"},
                           /* 144 */   new string[] {"soilw", "Volumetric soil moisture", "fraction"},
                           /* 145 */   new string[] {"pevpr", "Potential evaporation rate", "W/m^2"},
                           /* 146 */   new string[] {"cwork", "Cloud work function", "J/kg"},
                           /* 147 */   new string[] {"u-gwd", "Zonal gravity wave stress", "N/m^2"},
                           /* 148 */   new string[] {"v-gwd", "Meridional gravity wave stress", "N/m^2"},
                           /* 149 */   new string[] {"pvort", "Potential vorticity", "m^2/s/kg"},
                           /* 150 */   new string[] {"var150", "undefined", "undefined"},
                           /* 151 */   new string[] {"var151", "undefined", "undefined"},
                           /* 152 */   new string[] {"var152", "undefined", "undefined"},
                           /* 153 */   new string[] {"mfxdv", "Moisture flux divergence", "gr/gr*m/s/m"},
                           /* 154 */   new string[] {"vqr154", "undefined", "undefined"},
                           /* 155 */   new string[] {"gflux", "Ground heat flux", "W/m^2"},
                           /* 156 */   new string[] {"cin", "Convective inhibition", "J/kg"},
                           /* 157 */   new string[] {"cape", "Convective Avail. Pot. Energy", "J/kg"},
                           /* 158 */   new string[] {"tke", "Turbulent kinetic energy", "J/kg"},
                           /* 159 */   new string[] {"condp", "Lifted parcel condensation pressure", "Pa"},
                           /* 160 */   new string[] {"csusf", "Clear sky upward solar flux", "W/m^2"},
                           /* 161 */   new string[] {"csdsf", "Clear sky downward solar flux", "W/m^2"},
                           /* 162 */   new string[] {"csulf", "Clear sky upward long wave flux", "W/m^2"},
                           /* 163 */   new string[] {"csdlf", "Clear sky downward long wave flux", "W/m^2"},
                           /* 164 */   new string[] {"cfnsf", "Cloud forcing net solar flux", "W/m^2"},
                           /* 165 */   new string[] {"cfnlf", "Cloud forcing net long wave flux", "W/m^2"},
                           /* 166 */   new string[] {"vbdsf", "Visible beam downward solar flux", "W/m^2"},
                           /* 167 */   new string[] {"vddsf", "Visible diffuse downward solar flux", "W/m^2"},
                           /* 168 */   new string[] {"nbdsf", "Near IR beam downward solar flux", "W/m^2"},
                           /* 169 */   new string[] {"nddsf", "Near IR diffuse downward solar flux", "W/m^2"},
                           /* 170 */   new string[] {"ustr", "U wind stress", "N/m^2"},
                           /* 171 */   new string[] {"vstr", "V wind stress", "N/m^2"},
                           /* 172 */   new string[] {"mflx", "Momentum flux", "N/m^2"},
                           /* 173 */   new string[] {"lmh", "Mass point model surface", ""},
                           /* 174 */   new string[] {"lmv", "Velocity point model surface", ""},
                           /* 175 */   new string[] {"sglyr", "Neraby model level", ""},
                           /* 176 */   new string[] {"nlat", "Latitude", "deg"},
                           /* 177 */   new string[] {"nlon", "Longitude", "deg"},
                           /* 178 */   new string[] {"umas", "Mass weighted u", "gm/m*K*s"},
                           /* 179 */   new string[] {"vmas", "Mass weigtted v", "gm/m*K*s"},
                           /* 180 */   new string[] {"var180", "undefined", "undefined"},
                           /* 181 */   new string[] {"lpsx", "x-gradient of log pressure", "1/m"},
                           /* 182 */   new string[] {"lpsy", "y-gradient of log pressure", "1/m"},
                           /* 183 */   new string[] {"hgtx", "x-gradient of height", "m/m"},
                           /* 184 */   new string[] {"hgty", "y-gradient of height", "m/m"},
                           /* 185 */   new string[] {"stdz", "Standard deviation of Geop. hgt.", "m"},
                           /* 186 */   new string[] {"stdu", "Standard deviation of zonal wind", "m/s"},
                           /* 187 */   new string[] {"stdv", "Standard deviation of meridional wind", "m/s"},
                           /* 188 */   new string[] {"stdq", "Standard deviation of spec. hum.", "gm/gm"},
                           /* 189 */   new string[] {"stdt", "Standard deviation of temperature", "K"},
                           /* 190 */   new string[] {"cbuw", "Covariance between u and omega", "m/s*Pa/s"},
                           /* 191 */   new string[] {"cbvw", "Covariance between v and omega", "m/s*Pa/s"},
                           /* 192 */   new string[] {"cbuq", "Covariance between u and specific hum", "m/s*gm/gm"},
                           /* 193 */   new string[] {"cbvq", "Covariance between v and specific hum", "m/s*gm/gm"},
                           /* 194 */   new string[] {"cbtw", "Covariance between T and omega", "K*Pa/s"},
                           /* 195 */   new string[] {"cbqw", "Covariance between spec. hum and omeg", "gm/gm*Pa/s"},
                           /* 196 */   new string[] {"cbmzw", "Covariance between v and u", "m^2/si^2"},
                           /* 197 */   new string[] {"cbtzw", "Covariance between u and T", "K*m/s"},
                           /* 198 */   new string[] {"cbtmw", "Covariance between v and T", "K*m/s"},
                           /* 199 */   new string[] {"stdrh", "Standard deviation of Rel. Hum.", "%"},
                           /* 200 */   new string[] {"sdtz", "Std dev of time tend of geop. hgt", "m"},
                           /* 201 */   new string[] {"icwat", "Ice-free water surface", "%"},
                           /* 202 */   new string[] {"sdtu", "Std dev of time tend of zonal wind", "m/s"},
                           /* 203 */   new string[] {"sdtv", "Std dev of time tend of merid wind", "m/s"},
                           /* 204 */   new string[] {"dswrf", "Downward solar radiation flux", "W/m^2"},
                           /* 205 */   new string[] {"dlwrf", "Downward long wave radiation flux", "W/m^2"},
                           /* 206 */   new string[] {"sdtq", "Std dev of time tend of spec. hum", "gm/gm"},
                           /* 207 */   new string[] {"mstav", "Moisture availability", "%"},
                           /* 208 */   new string[] {"sfexc", "Exchange coefficient", "(kg/m^3)(m/s)"},
                           /* 209 */   new string[] {"mixly", "No. of mixed layers next to surface", "integer"},
                           /* 210 */   new string[] {"sdtt", "Std dev of time tend of temperature", "K"},
                           /* 211 */   new string[] {"uswrf", "Upward short wave flux", "W/m^2"},
                           /* 212 */   new string[] {"ulwrf", "Upward long wave flux", "W/m^2"},
                           /* 213 */   new string[] {"cdlyr", "Non-convective cloud", "%"},
                           /* 214 */   new string[] {"cprat", "Convective precip. rate", "kg/m^2/s"},
                           /* 215 */   new string[] {"ttdia", "Temperature tendency by all physics", "K/s"},
                           /* 216 */   new string[] {"ttrad", "Temperature tendency by all radiation", "K/s"},
                           /* 217 */   new string[] {"ttphy", "Temperature tendency by non-radiation physics", "K/s"},
                           /* 218 */   new string[] {"preix", "Precip index (0.0-1.00)", "fraction"},
                           /* 219 */   new string[] {"tsd1d", "Std. dev. of IR T over 1x1 deg area", "K"},
                           /* 220 */   new string[] {"nlgsp", "Natural log of surface pressure", "ln(kPa)"},
                           /* 221 */   new string[] {"sdtrh", "Std dev of time tend of rel humt", "%"},
                           /* 222 */   new string[] {"5wavh", "5-wave geopotential height", "gpm"},
                           /* 223 */   new string[] {"cwat", "Plant canopy surface water", "kg/m^2"},
                           /* 224 */   new string[] {"pltrs", "Maximum stomato plant resistance", "s/m"},
                           /* 225 */   new string[] {"rhcld", "RH-type cloud cover", "%"},
                           /* 226 */   new string[] {"bmixl", "Blackadar's mixing length scale", "m"},
                           /* 227 */   new string[] {"amixl", "Asymptotic mixing length scale", "m"},
                           /* 228 */   new string[] {"pevap", "Potential evaporation", "kg^2"},
                           /* 229 */   new string[] {"snohf", "Snow melt heat flux", "W/m^2"},
                           /* 230 */   new string[] {"snoev", "Snow sublimation heat flux", "W/m^2"},
                           /* 231 */   new string[] {"mflux", "Convective cloud mass flux", "Pa/s"},
                           /* 232 */   new string[] {"dtrf", "Downward total radiation flux", "W/m^2"},
                           /* 233 */   new string[] {"utrf", "Upward total radiation flux", "W/m^2"},
                           /* 234 */   new string[] {"bgrun", "Baseflow-groundwater runoff", "kg/m^2"},
                           /* 235 */   new string[] {"ssrun", "Storm surface runoff", "kg/m^2"},
                           /* 236 */   new string[] {"var236", "undefined", "undefined"},
                           /* 237 */   new string[] {"ozone", "Total column ozone concentration", "Dobson"},
                           /* 238 */   new string[] {"snoc", "Snow cover", "%"},
                           /* 239 */   new string[] {"snot", "Snow temperature", "K"},
                           /* 240 */   new string[] {"glcr", "Permanent snow points", "mask"},
                           /* 241 */   new string[] {"lrghr", "Large scale condensation heating rate", "K/s"},
                           /* 242 */   new string[] {"cnvhr", "Deep convective heating rate", "K/s"},
                           /* 243 */   new string[] {"cnvmr", "Deep convective moistening rate", "kg/kg/s"},
                           /* 244 */   new string[] {"shahr", "Shallow convective heating rate", "K/s"},
                           /* 245 */   new string[] {"shamr", "Shallow convective moistening rate", "kg/kg/s"},
                           /* 246 */   new string[] {"vdfhr", "Vertical diffusion heating rate", "K/s"},
                           /* 247 */   new string[] {"vdfua", "Vertical diffusion zonal accel", "m/s/s"},
                           /* 248 */   new string[] {"vdfva", "Vertical diffusion meridional accel", "m/s/s"},
                           /* 249 */   new string[] {"vdfmr", "Vertical diffusion moistening rate", "kg/kg/s"},
                           /* 250 */   new string[] {"swhr", "Solar radiative heating rate", "K/s"},
                           /* 251 */   new string[] {"lwhr", "Longwave radiative heating rate", "K/s"},
                           /* 252 */   new string[] {"cd", "Drag coefficient", ""},
                           /* 253 */   new string[] {"fricv", "Friction velocity", "m/s"},
                           /* 254 */   new string[] {"ri", "Richardson number", ""},
                           /* 255 */   new string[] {"var255", "undefined", "undefined"}
            };

            String pname = defaulttable_ncep_reanal2[paramNum][0];
            String pdesc = defaulttable_ncep_reanal2[paramNum][1];
            String punit = defaulttable_ncep_reanal2[paramNum][2];
            Variable aVar = new Variable(paramNum, pname, pdesc, punit);

            return aVar;
        }

        private static void initDefaultTableEntries(List<GRIBParameterTable> aTables)
        {
            string[][] defaulttable_ncep_reanal2 = new string[][]
            {
                           /*   0 */   new string[] {"var0", "undefined", "undefined"},
                           /*   1 */   new string[] {"pres", "Pressure", "Pa"},
                           /*   2 */   new string[] {"prmsl", "Pressure reduced to MSL", "Pa"},
                           /*   3 */   new string[] {"ptend", "Pressure tendency", "Pa/s"},
                           /*   4 */   new string[] {"var4", "undefined", "undefined"},
                           /*   5 */   new string[] {"var5", "undefined", "undefined"},
                           /*   6 */   new string[] {"gp", "Geopotential", "m^2/s^2"},
                           /*   7 */   new string[] {"hgt", "Geopotential height", "gpm"},
                           /*   8 */   new string[] {"dist", "Geometric height", "m"},
                           /*   9 */   new string[] {"hstdv", "Std dev of height", "m"},
                           /*  10 */   new string[] {"hvar", "Varianance of height", "m^2"},
                           /*  11 */   new string[] {"tmp", "Temperature", "K"},
                           /*  12 */   new string[] {"vtmp", "Virtual temperature", "K"},
                           /*  13 */   new string[] {"pot", "Potential temperature", "K"},
                           /*  14 */   new string[] {"epot", "Pseudo-adiabatic pot. temperature", "K"},
                           /*  15 */   new string[] {"tmax", "Max. temperature", "K"},
                           /*  16 */   new string[] {"tmin", "Min. temperature", "K"},
                           /*  17 */   new string[] {"dpt", "Dew point temperature", "K"},
                           /*  18 */   new string[] {"depr", "Dew point depression", "K"},
                           /*  19 */   new string[] {"lapr", "Lapse rate", "K/m"},
                           /*  20 */   new string[] {"visib", "Visibility", "m"},
                           /*  21 */   new string[] {"rdsp1", "Radar spectra (1)", ""},
                           /*  22 */   new string[] {"rdsp2", "Radar spectra (2)", ""},
                           /*  23 */   new string[] {"rdsp3", "Radar spectra (3)", ""},
                           /*  24 */   new string[] {"var24", "undefined", "undefined"},
                           /*  25 */   new string[] {"tmpa", "Temperature anomaly", "K"},
                           /*  26 */   new string[] {"presa", "Pressure anomaly", "Pa"},
                           /*  27 */   new string[] {"gpa", "Geopotential height anomaly", "gpm"},
                           /*  28 */   new string[] {"wvsp1", "Wave spectra (1)", ""},
                           /*  29 */   new string[] {"wvsp2", "Wave spectra (2)", ""},
                           /*  30 */   new string[] {"wvsp3", "Wave spectra (3)", ""},
                           /*  31 */   new string[] {"wdir", "Wind direction", "deg"},
                           /*  32 */   new string[] {"wind", "Wind speed", "m/s"},
                           /*  33 */   new string[] {"ugrd", "u wind", "m/s"},
                           /*  34 */   new string[] {"vgrd", "v wind", "m/s"},
                           /*  35 */   new string[] {"strm", "Stream function", "m^2/s"},
                           /*  36 */   new string[] {"vpot", "Velocity potential", "m^2/s"},
                           /*  37 */   new string[] {"mntsf", "Montgomery stream function", "m^2/s^2"},
                           /*  38 */   new string[] {"sgcvv", "Sigma coord. vertical velocity", "/s"},
                           /*  39 */   new string[] {"vvel", "Pressure vertical velocity", "Pa/s"},
                           /*  40 */   new string[] {"dzdt", "Geometric vertical velocity", "m/s"},
                           /*  41 */   new string[] {"absv", "Absolute vorticity", "/s"},
                           /*  42 */   new string[] {"absd", "Absolute divergence", "/s"},
                           /*  43 */   new string[] {"relv", "Relative vorticity", "/s"},
                           /*  44 */   new string[] {"reld", "Relative divergence", "/s"},
                           /*  45 */   new string[] {"vucsh", "Vertical u shear", "/s"},
                           /*  46 */   new string[] {"vvcsh", "Vertical v shear", "/s"},
                           /*  47 */   new string[] {"dirc", "Direction of current", "deg"},
                           /*  48 */   new string[] {"spc", "Speed of current", "m/s"},
                           /*  49 */   new string[] {"uogrd", "u of current", "m/s"},
                           /*  50 */   new string[] {"vogrd", "v of current", "m/s"},
                           /*  51 */   new string[] {"spfh", "Specific humidity", "kg/kg"},
                           /*  52 */   new string[] {"rh", "Relative humidity", "%"},
                           /*  53 */   new string[] {"mixr", "Humidity mixing ratio", "kg/kg"},
                           /*  54 */   new string[] {"pwat", "Precipitable water", "kg/m^2"},
                           /*  55 */   new string[] {"vapp", "Vapor pressure", "Pa"},
                           /*  56 */   new string[] {"satd", "Saturation deficit", "Pa"},
                           /*  57 */   new string[] {"evp", "Evaporation", "kg/m^2"},
                           /*  58 */   new string[] {"cice", "Cloud Ice", "kg/m^2"},
                           /*  59 */   new string[] {"prate", "Precipitation rate", "kg/m^2/s"},
                           /*  60 */   new string[] {"tstm", "Thunderstorm probability", "%"},
                           /*  61 */   new string[] {"apcp", "Total precipitation", "kg/m^2"},
                           /*  62 */   new string[] {"ncpcp", "Large scale precipitation", "kg/m^2"},
                           /*  63 */   new string[] {"acpcp", "Convective precipitation", "kg/m^2"},
                           /*  64 */   new string[] {"srweq", "Snowfall rate water equiv.", "kg/m^2/s"},
                           /*  65 */   new string[] {"weasd", "Accum. snow", "kg/m^2"},
                           /*  66 */   new string[] {"snod", "Snow depth", "m"},
                           /*  67 */   new string[] {"mixht", "Mixed layer depth", "m"},
                           /*  68 */   new string[] {"tthdp", "Transient thermocline depth", "m"},
                           /*  69 */   new string[] {"mthd", "Main thermocline depth", "m"},
                           /*  70 */   new string[] {"mtha", "Main thermocline anomaly", "m"},
                           /*  71 */   new string[] {"tcdc", "Total cloud cover", "%"},
                           /*  72 */   new string[] {"cdcon", "Convective cloud cover", "%"},
                           /*  73 */   new string[] {"lcdc", "Low level cloud cover", "%"},
                           /*  74 */   new string[] {"mcdc", "Mid level cloud cover", "%"},
                           /*  75 */   new string[] {"hcdc", "High level cloud cover", "%"},
                           /*  76 */   new string[] {"cwat", "Cloud water", "kg/m^2"},
                           /*  77 */   new string[] {"var77", "undefined", "undefined"},
                           /*  78 */   new string[] {"snoc", "Convective snow", "kg/m^2"},
                           /*  79 */   new string[] {"snol", "Large scale snow", "kg/m^2"},
                           /*  80 */   new string[] {"wtmp", "Water temperature", "K"},
                           /*  81 */   new string[] {"land", "Land cover (land=1;sea=0)", "fraction"},
                           /*  82 */   new string[] {"dslm", "Deviation of sea level from mean", "m"},
                           /*  83 */   new string[] {"sfcr", "Surface roughness", "m"},
                           /*  84 */   new string[] {"albdo", "Albedo", "%"},
                           /*  85 */   new string[] {"tsoil", "Soil temperature", "K"},
                           /*  86 */   new string[] {"soilm", "Soil moisture content", "kg/m^2"},
                           /*  87 */   new string[] {"veg", "Vegetation", "%"},
                           /*  88 */   new string[] {"salty", "Salinity", "kg/kg"},
                           /*  89 */   new string[] {"den", "Density", "kg/m^3"},
                           /*  90 */   new string[] {"runof", "Runoff", "kg/m^2"},
                           /*  91 */   new string[] {"icec", "Ice concentration (ice=1;no ice=0)", "fraction"},
                           /*  92 */   new string[] {"icetk", "Ice thickness", "m"},
                           /*  93 */   new string[] {"diced", "Direction of ice drift", "deg"},
                           /*  94 */   new string[] {"siced", "Speed of ice drift", "m/s"},
                           /*  95 */   new string[] {"uice", "u of ice drift", "m/s"},
                           /*  96 */   new string[] {"vice", "v of ice drift", "m/s"},
                           /*  97 */   new string[] {"iceg", "Ice growth rate", "m/s"},
                           /*  98 */   new string[] {"iced", "Ice divergence", "/s"},
                           /*  99 */   new string[] {"snom", "Snow melt", "kg/m^2"},
                           /* 100 */   new string[] {"htsgw", "Sig height of wind waves and swell", "m"},
                           /* 101 */   new string[] {"wvdir", "Direction of wind waves", "deg"},
                           /* 102 */   new string[] {"wvhgt", "Sig height of wind waves", "m"},
                           /* 103 */   new string[] {"wvper", "Mean period of wind waves", "s"},
                           /* 104 */   new string[] {"swdir", "Direction of swell waves", "deg"},
                           /* 105 */   new string[] {"swell", "Sig height of swell waves", "m"},
                           /* 106 */   new string[] {"swper", "Mean period of swell waves", "s"},
                           /* 107 */   new string[] {"dirpw", "Primary wave direction", "deg"},
                           /* 108 */   new string[] {"perpw", "Primary wave mean period", "s"},
                           /* 109 */   new string[] {"dirsw", "Secondary wave direction", "deg"},
                           /* 110 */   new string[] {"persw", "Secondary wave mean period", "s"},
                           /* 111 */   new string[] {"nswrs", "Net short wave (surface)", "W/m^2"},
                           /* 112 */   new string[] {"nlwrs", "Net long wave (surface)", "W/m^2"},
                           /* 113 */   new string[] {"nswrt", "Net short wave (top)", "W/m^2"},
                           /* 114 */   new string[] {"nlwrt", "Net long wave (top)", "W/m^2"},
                           /* 115 */   new string[] {"lwavr", "Long wave", "W/m^2"},
                           /* 116 */   new string[] {"swavr", "Short wave", "W/m^2"},
                           /* 117 */   new string[] {"grad", "Global radiation", "W/m^2"},
                           /* 118 */   new string[] {"var118", "undefined", "undefined"},
                           /* 119 */   new string[] {"var119", "undefined", "undefined"},
                           /* 120 */   new string[] {"var120", "undefined", "undefined"},
                           /* 121 */   new string[] {"lhtfl", "Latent heat flux", "W/m^2"},
                           /* 122 */   new string[] {"shtfl", "Sensible heat flux", "W/m^2"},
                           /* 123 */   new string[] {"blydp", "Boundary layer dissipation", "W/m^2"},
                           /* 124 */   new string[] {"uflx", "Zonal momentum flux", "N/m^2"},
                           /* 125 */   new string[] {"vflx", "Meridional momentum flux", "N/m^2"},
                           /* 126 */   new string[] {"wmixe", "Wind mixing energy", "J"},
                           /* 127 */   new string[] {"imgd", "Image data", ""},
                           /* 128 */   new string[] {"mslsa", "Mean sea level pressure (Std Atm)", "Pa"},
                           /* 129 */   new string[] {"mslma", "Mean sea level pressure (MAPS)", "Pa"},
                           /* 130 */   new string[] {"mslet", "Mean sea level pressure (ETA model)", "Pa"},
                           /* 131 */   new string[] {"lftx", "Surface lifted index", "K"},
                           /* 132 */   new string[] {"4lftx", "Best (4-layer) lifted index", "K"},
                           /* 133 */   new string[] {"kx", "K index", "K"},
                           /* 134 */   new string[] {"sx", "Sweat index", "K"},
                           /* 135 */   new string[] {"mconv", "Horizontal moisture divergence", "kg/kg/s"},
                           /* 136 */   new string[] {"vssh", "Vertical speed shear", "1/s"},
                           /* 137 */   new string[] {"tslsa", "3-hr pressure tendency (Std Atmos Red)", "Pa/s"},
                           /* 138 */   new string[] {"bvf2", "Brunt-Vaisala frequency^2", "1/s^2"},
                           /* 139 */   new string[] {"pvmw", "Potential vorticity (mass-weighted)", "1/s/m"},
                           /* 140 */   new string[] {"crain", "Categorical rain", "yes=1;no=0"},
                           /* 141 */   new string[] {"cfrzr", "Categorical freezing rain", "yes=1;no=0"},
                           /* 142 */   new string[] {"cicep", "Categorical ice pellets", "yes=1;no=0"},
                           /* 143 */   new string[] {"csnow", "Categorical snow", "yes=1;no=0"},
                           /* 144 */   new string[] {"soilw", "Volumetric soil moisture", "fraction"},
                           /* 145 */   new string[] {"pevpr", "Potential evaporation rate", "W/m^2"},
                           /* 146 */   new string[] {"cwork", "Cloud work function", "J/kg"},
                           /* 147 */   new string[] {"u-gwd", "Zonal gravity wave stress", "N/m^2"},
                           /* 148 */   new string[] {"v-gwd", "Meridional gravity wave stress", "N/m^2"},
                           /* 149 */   new string[] {"pvort", "Potential vorticity", "m^2/s/kg"},
                           /* 150 */   new string[] {"var150", "undefined", "undefined"},
                           /* 151 */   new string[] {"var151", "undefined", "undefined"},
                           /* 152 */   new string[] {"var152", "undefined", "undefined"},
                           /* 153 */   new string[] {"mfxdv", "Moisture flux divergence", "gr/gr*m/s/m"},
                           /* 154 */   new string[] {"vqr154", "undefined", "undefined"},
                           /* 155 */   new string[] {"gflux", "Ground heat flux", "W/m^2"},
                           /* 156 */   new string[] {"cin", "Convective inhibition", "J/kg"},
                           /* 157 */   new string[] {"cape", "Convective Avail. Pot. Energy", "J/kg"},
                           /* 158 */   new string[] {"tke", "Turbulent kinetic energy", "J/kg"},
                           /* 159 */   new string[] {"condp", "Lifted parcel condensation pressure", "Pa"},
                           /* 160 */   new string[] {"csusf", "Clear sky upward solar flux", "W/m^2"},
                           /* 161 */   new string[] {"csdsf", "Clear sky downward solar flux", "W/m^2"},
                           /* 162 */   new string[] {"csulf", "Clear sky upward long wave flux", "W/m^2"},
                           /* 163 */   new string[] {"csdlf", "Clear sky downward long wave flux", "W/m^2"},
                           /* 164 */   new string[] {"cfnsf", "Cloud forcing net solar flux", "W/m^2"},
                           /* 165 */   new string[] {"cfnlf", "Cloud forcing net long wave flux", "W/m^2"},
                           /* 166 */   new string[] {"vbdsf", "Visible beam downward solar flux", "W/m^2"},
                           /* 167 */   new string[] {"vddsf", "Visible diffuse downward solar flux", "W/m^2"},
                           /* 168 */   new string[] {"nbdsf", "Near IR beam downward solar flux", "W/m^2"},
                           /* 169 */   new string[] {"nddsf", "Near IR diffuse downward solar flux", "W/m^2"},
                           /* 170 */   new string[] {"ustr", "U wind stress", "N/m^2"},
                           /* 171 */   new string[] {"vstr", "V wind stress", "N/m^2"},
                           /* 172 */   new string[] {"mflx", "Momentum flux", "N/m^2"},
                           /* 173 */   new string[] {"lmh", "Mass point model surface", ""},
                           /* 174 */   new string[] {"lmv", "Velocity point model surface", ""},
                           /* 175 */   new string[] {"sglyr", "Neraby model level", ""},
                           /* 176 */   new string[] {"nlat", "Latitude", "deg"},
                           /* 177 */   new string[] {"nlon", "Longitude", "deg"},
                           /* 178 */   new string[] {"umas", "Mass weighted u", "gm/m*K*s"},
                           /* 179 */   new string[] {"vmas", "Mass weigtted v", "gm/m*K*s"},
                           /* 180 */   new string[] {"var180", "undefined", "undefined"},
                           /* 181 */   new string[] {"lpsx", "x-gradient of log pressure", "1/m"},
                           /* 182 */   new string[] {"lpsy", "y-gradient of log pressure", "1/m"},
                           /* 183 */   new string[] {"hgtx", "x-gradient of height", "m/m"},
                           /* 184 */   new string[] {"hgty", "y-gradient of height", "m/m"},
                           /* 185 */   new string[] {"stdz", "Standard deviation of Geop. hgt.", "m"},
                           /* 186 */   new string[] {"stdu", "Standard deviation of zonal wind", "m/s"},
                           /* 187 */   new string[] {"stdv", "Standard deviation of meridional wind", "m/s"},
                           /* 188 */   new string[] {"stdq", "Standard deviation of spec. hum.", "gm/gm"},
                           /* 189 */   new string[] {"stdt", "Standard deviation of temperature", "K"},
                           /* 190 */   new string[] {"cbuw", "Covariance between u and omega", "m/s*Pa/s"},
                           /* 191 */   new string[] {"cbvw", "Covariance between v and omega", "m/s*Pa/s"},
                           /* 192 */   new string[] {"cbuq", "Covariance between u and specific hum", "m/s*gm/gm"},
                           /* 193 */   new string[] {"cbvq", "Covariance between v and specific hum", "m/s*gm/gm"},
                           /* 194 */   new string[] {"cbtw", "Covariance between T and omega", "K*Pa/s"},
                           /* 195 */   new string[] {"cbqw", "Covariance between spec. hum and omeg", "gm/gm*Pa/s"},
                           /* 196 */   new string[] {"cbmzw", "Covariance between v and u", "m^2/si^2"},
                           /* 197 */   new string[] {"cbtzw", "Covariance between u and T", "K*m/s"},
                           /* 198 */   new string[] {"cbtmw", "Covariance between v and T", "K*m/s"},
                           /* 199 */   new string[] {"stdrh", "Standard deviation of Rel. Hum.", "%"},
                           /* 200 */   new string[] {"sdtz", "Std dev of time tend of geop. hgt", "m"},
                           /* 201 */   new string[] {"icwat", "Ice-free water surface", "%"},
                           /* 202 */   new string[] {"sdtu", "Std dev of time tend of zonal wind", "m/s"},
                           /* 203 */   new string[] {"sdtv", "Std dev of time tend of merid wind", "m/s"},
                           /* 204 */   new string[] {"dswrf", "Downward solar radiation flux", "W/m^2"},
                           /* 205 */   new string[] {"dlwrf", "Downward long wave radiation flux", "W/m^2"},
                           /* 206 */   new string[] {"sdtq", "Std dev of time tend of spec. hum", "gm/gm"},
                           /* 207 */   new string[] {"mstav", "Moisture availability", "%"},
                           /* 208 */   new string[] {"sfexc", "Exchange coefficient", "(kg/m^3)(m/s)"},
                           /* 209 */   new string[] {"mixly", "No. of mixed layers next to surface", "integer"},
                           /* 210 */   new string[] {"sdtt", "Std dev of time tend of temperature", "K"},
                           /* 211 */   new string[] {"uswrf", "Upward short wave flux", "W/m^2"},
                           /* 212 */   new string[] {"ulwrf", "Upward long wave flux", "W/m^2"},
                           /* 213 */   new string[] {"cdlyr", "Non-convective cloud", "%"},
                           /* 214 */   new string[] {"cprat", "Convective precip. rate", "kg/m^2/s"},
                           /* 215 */   new string[] {"ttdia", "Temperature tendency by all physics", "K/s"},
                           /* 216 */   new string[] {"ttrad", "Temperature tendency by all radiation", "K/s"},
                           /* 217 */   new string[] {"ttphy", "Temperature tendency by non-radiation physics", "K/s"},
                           /* 218 */   new string[] {"preix", "Precip index (0.0-1.00)", "fraction"},
                           /* 219 */   new string[] {"tsd1d", "Std. dev. of IR T over 1x1 deg area", "K"},
                           /* 220 */   new string[] {"nlgsp", "Natural log of surface pressure", "ln(kPa)"},
                           /* 221 */   new string[] {"sdtrh", "Std dev of time tend of rel humt", "%"},
                           /* 222 */   new string[] {"5wavh", "5-wave geopotential height", "gpm"},
                           /* 223 */   new string[] {"cwat", "Plant canopy surface water", "kg/m^2"},
                           /* 224 */   new string[] {"pltrs", "Maximum stomato plant resistance", "s/m"},
                           /* 225 */   new string[] {"rhcld", "RH-type cloud cover", "%"},
                           /* 226 */   new string[] {"bmixl", "Blackadar's mixing length scale", "m"},
                           /* 227 */   new string[] {"amixl", "Asymptotic mixing length scale", "m"},
                           /* 228 */   new string[] {"pevap", "Potential evaporation", "kg^2"},
                           /* 229 */   new string[] {"snohf", "Snow melt heat flux", "W/m^2"},
                           /* 230 */   new string[] {"snoev", "Snow sublimation heat flux", "W/m^2"},
                           /* 231 */   new string[] {"mflux", "Convective cloud mass flux", "Pa/s"},
                           /* 232 */   new string[] {"dtrf", "Downward total radiation flux", "W/m^2"},
                           /* 233 */   new string[] {"utrf", "Upward total radiation flux", "W/m^2"},
                           /* 234 */   new string[] {"bgrun", "Baseflow-groundwater runoff", "kg/m^2"},
                           /* 235 */   new string[] {"ssrun", "Storm surface runoff", "kg/m^2"},
                           /* 236 */   new string[] {"var236", "undefined", "undefined"},
                           /* 237 */   new string[] {"ozone", "Total column ozone concentration", "Dobson"},
                           /* 238 */   new string[] {"snoc", "Snow cover", "%"},
                           /* 239 */   new string[] {"snot", "Snow temperature", "K"},
                           /* 240 */   new string[] {"glcr", "Permanent snow points", "mask"},
                           /* 241 */   new string[] {"lrghr", "Large scale condensation heating rate", "K/s"},
                           /* 242 */   new string[] {"cnvhr", "Deep convective heating rate", "K/s"},
                           /* 243 */   new string[] {"cnvmr", "Deep convective moistening rate", "kg/kg/s"},
                           /* 244 */   new string[] {"shahr", "Shallow convective heating rate", "K/s"},
                           /* 245 */   new string[] {"shamr", "Shallow convective moistening rate", "kg/kg/s"},
                           /* 246 */   new string[] {"vdfhr", "Vertical diffusion heating rate", "K/s"},
                           /* 247 */   new string[] {"vdfua", "Vertical diffusion zonal accel", "m/s/s"},
                           /* 248 */   new string[] {"vdfva", "Vertical diffusion meridional accel", "m/s/s"},
                           /* 249 */   new string[] {"vdfmr", "Vertical diffusion moistening rate", "kg/kg/s"},
                           /* 250 */   new string[] {"swhr", "Solar radiative heating rate", "K/s"},
                           /* 251 */   new string[] {"lwhr", "Longwave radiative heating rate", "K/s"},
                           /* 252 */   new string[] {"cd", "Drag coefficient", ""},
                           /* 253 */   new string[] {"fricv", "Friction velocity", "m/s"},
                           /* 254 */   new string[] {"ri", "Richardson number", ""},
                           /* 255 */   new string[] {"var255", "undefined", "undefined"}
            };
            int npar = defaulttable_ncep_reanal2.Length;
            //assert npar <= NPARAMETERS;
            Parameter[] parameters = new Parameter[npar];
            for (int n = 0; n < npar; ++n)
            {
                String pname = defaulttable_ncep_reanal2[n][0];
                String pdesc = defaulttable_ncep_reanal2[n][1];
                String punit = defaulttable_ncep_reanal2[n][2];
                parameters[n] = new Parameter(n, pname, pdesc, punit);
            }

            aTables.Add(new GRIBParameterTable("ncep_reanal2.1", 7, -1, 1, parameters));
            aTables.Add(new GRIBParameterTable("ncep_reanal2.2", 7, -1, 2, parameters));
            aTables.Add(new GRIBParameterTable("ncep_reanal2.3", 7, -1, 3, parameters));
            aTables.Add(new GRIBParameterTable("ncep_reanal2.4", 81, -1, 3, parameters));
            aTables.Add(new GRIBParameterTable("ncep_reanal2.5", 88, -1, 2, parameters));
            aTables.Add(new GRIBParameterTable("ncep_reanal2.6", 88, -1, 128, parameters));
        }

        /**
   * type of vertical coordinate: short Name
   * derived from code table 4.5.
   *
   * @param id surfaceType
   * @return SurfaceNameShort
   */       
        static public String getTypeSurfaceNameShort(int id)
        {

            switch (id)
            {

                case 0:
                    return "";

                case 1:
                    return "surface";

                case 2:
                    return "cloud_base";

                case 3:
                    return "cloud_tops";

                case 4:
                    return "zeroDegC_isotherm";

                case 5:
                    return "adiabatic_condensation_lifted";

                case 6:
                    return "maximum_wind";

                case 7:
                    return "tropopause";

                case 8:
                    return "atmosphere_top";

                case 9:
                    return "sea_bottom";

                case 20:
                    return "isotherm";

                case 100:
                    return "isobaric";

                case 101:
                    return "two_isobaric";

                case 102:
                    return "msl";

                case 103:
                    return "above_msl";

                case 104:
                    return "two_above_msl";

                case 105:
                    return "above_ground";

                case 106:
                    return "two_above_ground";

                case 107:
                    return "sigmal";

                case 108:
                    return "two_sigma";

                case 109:
                    return "hybrid";

                case 110:
                    return "two_hybrid";

                case 111:
                    return "below_surface";

                case 112:
                    return "two_below_surface";

                case 113:
                    return "isentropic";

                case 114:
                    return "two_isentropic";

                case 115:
                    return "pressure_difference";

                case 116:
                    return "two_pressure_difference";

                case 117:
                    return "potential_vorticity";

                case 119:
                    return "ETA";

                case 120:
                    return "two_ETA";

                case 121:
                    return "two_isobaric_high_prec";

                case 125:
                    return "above_ground_high_prec";

                case 128:
                    return "two_sigma_high_prec";

                case 141:
                    return "two_isobaric_high_prec";

                case 160:
                    return "depth_below_sea";

                case 200:
                    return "entire_atmosphere";

                case 201:
                    return "entire_ocean";                

                case 255:
                    return "missing";

                default:
                    return "Unknown" + id;
            }

        }  // end getTypeSurfaceNameShort

        #endregion
    }
}
