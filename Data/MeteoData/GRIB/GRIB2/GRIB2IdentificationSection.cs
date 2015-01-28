using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 2 identification section
    /// </summary>
    public class GRIB2IdentificationSection
    {
        #region Variables
        /// <summary>
        /// Section length in bytes
        /// </summary>
        public int Length;
        /// <summary>
        /// Number of section
        /// </summary>
        public int SectionNum;
        /// <summary>
        /// Parameter Table Version number
        /// </summary>
        public int MasterTableVersion;
        /// <summary>
        /// Local table version number
        /// </summary>
        public int LocalTableVersion;
        /// <summary>
        /// Significance of reference time
        /// </summary>
        public int SReferenceTime;
        /// <summary>
        /// Identification of center
        /// </summary>
        public int CenterID;              
        /// <summary>
        /// Base (analysis) time of the forecast
        /// </summary>
        public DateTime BaseTime;        
        /// <summary>
        /// Identification of sub-center (allocated by the originating center.
        /// </summary>
        public int SubCenterID;
        /// <summary>
        /// Production status of processed data in this GRIB message.
        /// </summary>
        public int ProductStatus;
        /// <summary>
        /// Type of processed data in this GRIB message.
        /// </summary>
        public int ProductType;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        public GRIB2IdentificationSection(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(4);
            Length = Bytes2Number.Int4(bytes[0], bytes[1], bytes[2], bytes[3]);

            br.BaseStream.Seek(-4, SeekOrigin.Current);
            bytes = br.ReadBytes(Length);

            SectionNum = bytes[4];
            CenterID = Bytes2Number.Int2(bytes[5], bytes[6]);
            SubCenterID = Bytes2Number.Int2(bytes[7], bytes[8]);
            MasterTableVersion = bytes[9];
            LocalTableVersion = bytes[10];
            SReferenceTime = bytes[11];
            int year = Bytes2Number.Int2(bytes[12], bytes[13]);
            int month = bytes[14];
            int day = bytes[15];
            int hour = bytes[16];
            int minute = bytes[17];
            int second = bytes[18];
            BaseTime = new DateTime(year, month, day, hour, minute, second);
            ProductStatus = bytes[19];
            ProductType = bytes[20];
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get center identification name
        /// </summary>
        /// <returns>center identification name</returns>
        public string GetCenterIDName()
        {

            switch (CenterID)
            {

                case 0:
                    return "WMO Secretariat";

                case 1:
                case 2:
                    return "Melbourne";

                case 4:
                case 5:
                    return "Moscow";

                case 7:
                    return "US National Weather Service (NCEP)";

                case 8:
                    return "US National Weather Service (NWSTG)";

                case 9:
                    return "US National Weather Service (other)";

                case 10:
                    return "Cairo (RSMC/RAFC)";

                case 12:
                    return "Dakar (RSMC/RAFC)";

                case 14:
                    return "Nairobi (RSMC/RAFC)";

                case 18:
                    return "Tunis Casablanca (RSMC)";

                case 20:
                    return "Las Palmas (RAFC)";

                case 21:
                    return "Algiers (RSMC)";

                case 24:
                    return "Pretoria (RSMC)";

                case 25:
                    return "La R?union (RSMC)";

                case 26:
                    return "Khabarovsk (RSMC)";

                case 28:
                    return "New Delhi (RSMC/RAFC)";

                case 30:
                    return "Novosibirsk (RSMC)";

                case 32:
                    return "Tashkent (RSMC)";

                case 33:
                    return "eddah (RSMC)";

                case 34:
                    return "Tokyo (RSMC), Japan Meteorological Agency";

                case 36:
                    return "Bangkok";

                case 37:
                    return "Ulan Bator";

                case 38:
                    return "Beijing (RSMC)";

                case 40:
                    return "Seoul";

                case 41:
                    return "Buenos Aires (RSMC/RAFC)";

                case 43:
                    return "Brasilia (RSMC/RAFC)";

                case 45:
                    return "Santiago";

                case 46:
                    return "Brazilian Space Agency ? INPE";

                case 51:
                    return "Miami (RSMC/RAFC)";

                case 52:
                    return "Miami RSMC, National Hurricane Center";

                case 53:
                    return "Montreal (RSMC)";

                case 55:
                    return "San Francisco";

                case 57:
                    return "Air Force Weather Agency";

                case 58:
                    return "Fleet Numerical Meteorology and Oceanography Center";

                case 59:
                    return "The NOAA Forecast Systems Laboratory";

                case 60:
                    return "United States National Centre for Atmospheric Research (NCAR)";

                case 64:
                    return "Honolulu";

                case 65:
                    return "Darwin (RSMC)";

                case 67:
                    return "Melbourne (RSMC)";

                case 69:
                    return "Wellington (RSMC/RAFC)";

                case 71:
                    return "Nadi (RSMC)";

                case 74:
                    return "UK Meteorological Office Bracknell (RSMC)";

                case 76:
                    return "Moscow (RSMC/RAFC)";

                case 78:
                    return "Offenbach (RSMC)";

                case 80:
                    return "Rome (RSMC)";

                case 82:
                    return "Norrk?ping";

                case 85:
                    return "Toulouse (RSMC)";

                case 86:
                    return "Helsinki";

                case 87:
                    return "Belgrade";

                case 88:
                    return "Oslo";

                case 89:
                    return "Prague";

                case 90:
                    return "Episkopi";

                case 91:
                    return "Ankara";

                case 92:
                    return "Frankfurt/Main (RAFC)";

                case 93:
                    return "London (WAFC)";

                case 94:
                    return "Copenhagen";

                case 95:
                    return "Rota";

                case 96:
                    return "Athens";

                case 97:
                    return "European Space Agency (ESA)";

                case 98:
                    return "ECMWF, RSMC";

                case 99:
                    return "De Bilt";

                case 110:
                    return "Hong-Kong";

                case 210:
                    return "Frascati (ESA/ESRIN)";

                case 211:
                    return "Lanion";

                case 212:
                    return "Lisboa";

                case 213:
                    return "Reykjavik";

                case 254:
                    return "EUMETSAT Operation Centre";

                default:
                    return "Unknown";
            }

        }

        /// <summary>
        /// Get significance reference time name
        /// </summary>
        /// <returns>significance reference time name</returns>
        public string GetSReferenceTimeName()
        {
            switch (SReferenceTime)
            {

                case 0:
                    return "Analysis";

                case 1:
                    return "Start of forecast";

                case 2:
                    return "Verifying time of forecast";

                case 3:
                    return "Observation time";

                default:
                    return "Unknown";
            }
        }

        /// <summary>
        /// Get product status name
        /// </summary>
        /// <returns>product status name</returns>
        public string GetProductStatusName()
        {
            switch (ProductStatus)
            {

                case 0:
                    return "Operational products";

                case 1:
                    return "Operational test products";

                case 2:
                    return "Research products";

                case 3:
                    return "Re-analysis products";

                case 5:
                    return "TIGGE products";

                default:
                    return "Unknown";
            }
        }

        /// <summary>
        /// Get product type name
        /// </summary>
        /// <returns>product type name</returns>
        public string GetProductTypeName()
        {
            switch (ProductType)
            {

                case 0:
                    return "Analysis products";

                case 1:
                    return "Forecast products";

                case 2:
                    return "Analysis and forecast products";

                case 3:
                    return "Control forecast products";

                case 4:
                    return "Perturbed forecast products";

                case 5:
                    return "Control and Perturbed forecast products";

                case 6:
                    return "Processed satellite observations";

                case 7:
                    return "Processed radar observations";

                default:
                    return "Unknown";
            }
        }

        #endregion
    }
}
