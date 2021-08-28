using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 2 bit map section
    /// </summary>
    public class GRIB2BitMapSection
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
        /// Bit map indicator
        /// </summary>
        public int BitMapIndicator;
        /// <summary>
        /// The bit map
        /// </summary>
        public bool[] Bitmap;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        public GRIB2BitMapSection(BinaryReader br)
        {
            long sectionEnd = br.BaseStream.Position;

            Length = Bytes2Number.Int4(br);
            sectionEnd += Length;

            SectionNum = br.ReadByte();
            BitMapIndicator = br.ReadByte();

            //No bit map
            if (BitMapIndicator != 0)
                return;

            //Get bit map data
            byte[] data = br.ReadBytes(Length - 6);
            Bitmap = new bool[(Length - 6) * 8];
            int[] bitmask = { 128, 64, 32, 16, 8, 4, 2, 1 };
            for (int i = 0; i < Bitmap.Length; i++)
            {
                Bitmap[i] = (data[i / 8] & bitmask[i % 8]) != 0;
            }

            //Set stream position to the section end
            br.BaseStream.Position = sectionEnd;
        }

        #endregion
    }
}
