using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 2 local use section
    /// </summary>
    public class GRIB2LocalUseSection
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

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        public GRIB2LocalUseSection(BinaryReader br)
        {            
            Length = Bytes2Number.Int4(br);

            br.BaseStream.Seek(-4, SeekOrigin.Current);
            byte[] bytes = br.ReadBytes(Length);

            SectionNum = bytes[4];
        }

        #endregion
    }
}
