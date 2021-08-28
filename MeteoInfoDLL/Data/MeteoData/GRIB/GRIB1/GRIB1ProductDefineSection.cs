using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 1 product define section
    /// </summary>
    public class GRIB1ProductDefineSection
    {
        #region Variables
        /// <summary>
        /// PDS length in bytes
        /// </summary>
        public int Length;
        /// <summary>
        /// Parameter Table Version number, currently 3 for internatioanl exchange.
        /// Parameter table version mubers 128-254 are reserved for local use.
        /// </summary>
        public int TableVersion;
        /// <summary>
        /// Identification of center
        /// </summary>
        public int CenterID;
        /// <summary>
        /// Generating process ID number, allocated by the originating center.
        /// </summary>
        public int TypeGenProcess;
        /// <summary>
        /// Grid Identification (geographical location and area, defined by the originating center.
        /// </summary>
        public int GridID;
        /// <summary>
        /// If GDS exist
        /// </summary>
        public bool GDSExist;
        /// <summary>
        /// If BMS exist
        /// </summary>
        public bool BMSExist;
        /// <summary>
        /// Indicator of parameter and units
        /// </summary>
        public int ParameterIndicator;
        /// <summary>
        /// Variable
        /// </summary>
        public Variable Parameter;
        /// <summary>
        /// Indicator of type of level or layer
        /// </summary>
        public int LevelType;
        /// <summary>
        /// Height of the level or layer
        /// </summary>
        public int LevelValue;       
        /// <summary>
        /// Base (analysis) time of the forecast
        /// </summary>
        public DateTime BaseTime;
        /// <summary>
        /// Forecast time unit
        /// </summary>
        public int ForecastTimeUnit;
        /// <summary>
        /// Period of time (Number of time units)
        /// 0 for analysis or initialize analysis.       
        /// </summary>
        public int P1;
        /// <summary>
        /// Period of time (Number of time units)
        /// or time interval between successive analyses
        /// or successive initialized analyses, or forecasts
        /// or undergoing averaging or accumulation        
        /// </summary>
        public int P2;
        /// <summary>
        /// Time range indicator
        /// </summary>
        public int TimeRangeIndicator;
        /// <summary>
        /// Number included in average, when 'Time range indicator' indicates an average of
        /// accumulation; otherwise set to zero.
        /// </summary>
        public int AveInclude;
        /// <summary>
        /// Number Missing from averages or accumulations
        /// </summary>
        public int NumMissing;
        /// <summary>
        /// Century of Initial (Reference) time (=20 until Jan. 1, 2001)
        /// </summary>
        public int InitialCentral;
        /// <summary>
        /// Identification of sub-center (allocated by the originating center.
        /// </summary>
        public int SubCenterID;
        /// <summary>
        /// The decimal scale factor D
        /// A negative value is indicated by setting the high order bit(bit No.1)
        /// in octet 27 to 1 (on)
        /// </summary>
        public int DecimalScale;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        public GRIB1ProductDefineSection(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(3);
            Length = Bytes2Number.Uint3(bytes[0], bytes[1], bytes[2]);

            br.BaseStream.Seek(-3, SeekOrigin.Current);
            bytes = br.ReadBytes(Length);

            TableVersion = Convert.ToInt32(bytes[3]);
            CenterID = Convert.ToInt32(bytes[4]);
            TypeGenProcess = Convert.ToInt32(bytes[5]);
            GridID = Convert.ToInt32(bytes[6]);
            int ifExists = Convert.ToInt32(bytes[7]);
            GDSExist = (ifExists & 128) == 128;
            BMSExist = (ifExists & 64) == 64;
            ParameterIndicator = Convert.ToInt32(bytes[8]);
            Parameter = GRIBParameterTable.GetDefaultParameter(ParameterIndicator);
            LevelType = Convert.ToInt32(bytes[9]);
            LevelValue = Bytes2Number.Uint2(bytes[10], bytes[11]);            
            int year = Convert.ToInt32(bytes[12]);
            int month = Convert.ToInt32(bytes[13]);
            int day = Convert.ToInt32(bytes[14]);
            int hour = Convert.ToInt32(bytes[15]);
            int minute = Convert.ToInt32(bytes[16]);            
            ForecastTimeUnit = Convert.ToInt32(bytes[17]);
            P1 = Convert.ToInt32(bytes[18]);
            P2 = Convert.ToInt32(bytes[19]);
            TimeRangeIndicator = Convert.ToInt32(bytes[20]);            
            AveInclude =Bytes2Number.Int2(bytes[21], bytes[22]);
            NumMissing = Convert.ToInt32(bytes[23]);
            InitialCentral = Convert.ToInt32(bytes[24]);
            BaseTime = new DateTime((InitialCentral - 1) * 100 + year, month, day, hour, minute, 0);
            SubCenterID = Convert.ToInt32(bytes[25]);
            DecimalScale = Bytes2Number.Int2(bytes[26], bytes[27]);    //octets 27-28            
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get forecast time
        /// </summary>
        /// <returns>forecast time</returns>
        public DateTime GetForecastTime()
        {
            DateTime fTime = BaseTime;
            int forecastTime = 0;            

            //Get time increment
            switch (TimeRangeIndicator)
            {

                case 0:
                    //product valid at RT + P1
                    forecastTime = P1;
                    break;

                case 1:
                    //product valid for RT, P1=0
                    forecastTime = 0;
                    break;

                case 2:
                    //product valid from (RT + P1) to (RT + P2)
                    forecastTime = P2;
                    break;

                case 3:
                    //product is an average between (RT + P1) to (RT + P2)
                    forecastTime = P2;
                    break;

                case 4:
                    //product is an accumulation between (RT + P1) to (RT + P2)
                    forecastTime = P2;
                    break;

                case 5:
                    //product is the difference (RT + P2) - (RT + P1)
                    forecastTime = P2;
                    break;

                case 6:
                    //product is an average from (RT - P1) to (RT - P2)
                    forecastTime = -P2;
                    break;

                case 7:
                    //product is an average from (RT - P1) to (RT + P2)
                    forecastTime = P2;
                    break;

                case 10:
                    //product valid at RT + P1
                    // p1 really consists of 2 bytes p1 and p2
                    forecastTime = P1 = Bytes2Number.Int2((byte)P1, (byte)P2);
                    P2 = 0;
                    break;

                case 51:
                   //mean value from RT to (RT + P2)
                    forecastTime = P2;
                    break;

                case 113:
                    //Average of N forecasts, forecast period of P1, reference intervals of P2
                    forecastTime = P1;
                    break;                
            }
            forecastTime = forecastTime * CalculateIncrement(ForecastTimeUnit, 1);

            //Get forecast time
            switch (ForecastTimeUnit)
            {
                case 0:    //Minute
                    fTime = BaseTime.AddMinutes(forecastTime);
                    break;
                case 1:    //Hour
                case 10:
                case 11:
                case 12:
                    fTime = BaseTime.AddHours(forecastTime);
                    break;
                case 2:    //Day
                    fTime = BaseTime.AddDays(forecastTime);
                    break;
                case 3:    //Month
                    fTime = BaseTime.AddMonths(forecastTime);
                    break;
                case 4:    //Year
                case 5:
                case 6:
                case 7:
                    fTime = BaseTime.AddYears(forecastTime);
                    break;
                case 13:
                    fTime = BaseTime.AddSeconds(forecastTime);
                    break;
            }

            //Return
            return fTime;
        }

        /// <summary>
        /// calculates the increment between time intervals 
        /// </summary>
        /// <param name="tui">tui time unit indicator</param>
        /// <param name="length">length of interval</param>
        /// <returns>increment</returns>
        private int CalculateIncrement(int tui, int length)
        {
            switch (tui)
            {

                case 1:
                    return length;
                case 10:
                    return 3 * length;
                case 11:
                    return 6 * length;
                case 12:
                    return 12 * length;
                case 0:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 13:
                    return length;     
                default:
                    return 0;
            }

        }

        #endregion
    }
}
