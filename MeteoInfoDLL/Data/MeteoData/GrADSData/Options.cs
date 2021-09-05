using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GrADS data options
    /// </summary>
    public class Options
    {
        /// <summary>
        /// (GrADS version 2.0) (For DTYPE grib2 only) Indicates that pressure values that appear in 
        /// the descriptor file (in the ZDEF entry and in the GRIB2 codes in the variable declarations) 
        /// are given in units of Pascals. 
        /// </summary>
        public bool pascals;
        /// <summary>
        /// Indicates that the Y dimension (latitude) in the data file has been written in the reverse 
        /// order from what GrADS assumes. 
        /// </summary>
        public bool yrev;
        /// <summary>
        /// Indicates that the Z dimension (pressure) in the data file has been written from top to bottom,
        /// rather than from bottom to top as GrADS assumes. 
        /// </summary>
        public bool zrev;
        /// <summary>
        /// Indicates that a template for multiple data files is in use.
        /// </summary>
        public bool template;
        /// <summary>
        /// Indicates that the file was written in sequential unformatted I/O. 
        /// This keyword may be used with either station or gridded data. 
        /// If your gridded data is written in sequential format, 
        /// then each record must be an X-Y varying grid. 
        /// </summary>
        public bool sequential;
        /// <summary>
        /// Indicates the data file was created with perpetual 365-day years,
        /// with no leap years. This is used for some types of model ouput.
        /// </summary>
        public bool calendar_365_day;
        /// <summary>
        /// Indicates the binary data file is in reverse byte order from the normal byte order of your machine.
        /// </summary>
        public bool byteswapped;
        /// <summary>
        /// Indicates the data file contains 32-bit IEEE floats created on a big endian platform (e.g., sun, sgi)
        /// </summary>
        public bool big_endian;
        /// <summary>
        /// Indicates the data file contains 32-bit IEEE floats created on a little endian platform (e.g., iX86, and dec)
        /// </summary>
        public bool little_endian;
        /// <summary>
        /// Indicates the data file contains 32-bit IEEE floats created on a cray.
        /// </summary>
        public bool cray_32bit_ieee;

        /// <summary>
        /// Constructor
        /// </summary>
        public Options()
        {
            pascals = false;
            yrev = false;
            zrev = false;
            template = false;
            sequential = false;
            calendar_365_day = false;
            byteswapped = false;
            big_endian = false;
            little_endian = true;
            cray_32bit_ieee = false;
        }
    }
}
