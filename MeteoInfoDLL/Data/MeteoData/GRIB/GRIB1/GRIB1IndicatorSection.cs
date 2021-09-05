using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 1 indicator section
    /// </summary>
    public class GRIB1IndicatorSection
    {
        #region Variables
        /// <summary>
        /// Length in bytes of GRIB record
        /// </summary>
        public int RecordLength;
        /// <summary>
        /// Length in bytes of this section
        /// </summary>
        public int Length;
        /// <summary>
        /// Edition of GRIB - 1
        /// </summary>
        public int Edition;
        /// <summary>
        /// Title of the message - GRIB
        /// </summary>
        public string Title;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        public GRIB1IndicatorSection(BinaryReader br)
        {
            Length = 8;
            Title = ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
            byte[] bytes = br.ReadBytes(3);
            RecordLength = Bytes2Number.Uint3(bytes[0], bytes[1], bytes[2]);
            Edition = Convert.ToInt32(br.ReadByte());
        }

        #endregion
    }
}
