using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 2 indicator section
    /// </summary>
    public class GRIB2IndicatorSection
    {
        #region Variables
        /// <summary>
        /// Length in bytes of GRIB record
        /// </summary>
        public long RecordLength;
        /// <summary>
        /// Length in bytes of this section
        /// </summary>
        public int Length;
        /// <summary>
        /// Edition of GRIB - 2
        /// </summary>
        public int Edition;
        /// <summary>
        /// Discipline - GRIB Master Table Number
        /// </summary>
        public int Discipline;
        /// <summary>
        /// Title - GRIB
        /// </summary>
        public string Title;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        public GRIB2IndicatorSection(BinaryReader br)
        {
            Length = 16;
            Title = ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
            br.ReadBytes(2);
            Discipline = br.ReadByte();
            Edition = br.ReadByte();
            byte[] bytes = br.ReadBytes(8);
            RecordLength = Bytes2Number.Int8(bytes);        
        }

        #endregion
    }
}
