using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 1 bit map section
    /// </summary>
    public class GRIB1BitMapSection
    {
        #region Variables
        /// <summary>
        /// Length in octets of Bit Map Section
        /// </summary>
        public int Length;       
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
        public GRIB1BitMapSection(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(3);
            Length = Bytes2Number.Uint3(bytes[0], bytes[1], bytes[2]);

            int unUsed = Convert.ToInt32(br.ReadByte());
            bytes = br.ReadBytes(2);
            int bm = Bytes2Number.Int2(bytes[0], bytes[1]);
            if (bm != 0)
            {
                if ((Length - 6) == 0)
                    return;

                br.ReadBytes(Length - 6);
                return;
            }

            byte[] data = br.ReadBytes(Length - 6);
            Bitmap = new bool[(Length - 6) * 8 - unUsed];
            int[] bitmask = {128, 64, 32, 16, 8, 4, 2, 1};
            for (int i = 0; i < Bitmap.Length; i++)
            {
                Bitmap[i] = (data[i / 8] & bitmask[i % 8]) != 0;
            }
        }

        #endregion
    }
}
