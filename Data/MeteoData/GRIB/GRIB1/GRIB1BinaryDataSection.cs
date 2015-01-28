using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 1 binary data section
    /// </summary>
    public class GRIB1BinaryDataSection
    {
        #region Variables
        /// <summary>
        /// Undefine data
        /// </summary>
        public double UNDEF = -9999.0;
        /// <summary>
        /// Length in bytes of this BDS
        /// </summary>
        public int Length;
        /// <summary>
        /// Binary scale factor
        /// </summary>
        public int BinScale;
        /// <summary>
        /// Reference value, the base for all parameter values
        /// </summary>
        public float RefValue;
        /// <summary>
        /// Number of bits per value
        /// </summary>
        public int NumBits;
        /// <summary>
        /// Array of parameter values
        /// </summary>
        public double[] Data;
        /// <summary>
        /// Minimal parameter value in grid.
        /// </summary>
        public double MinValue = float.MaxValue;
        /// <summary>
        /// Maximal parameter value in grid.
        /// </summary>
        public double MaxValue = -float.MaxValue;
        /// <summary>
        /// rdg - added this to prevent a divide by zero error if variable data empty       
        /// Indicates whether the BMS is represented by a single value
        ///  -  Octet 12 is empty, and the data is represented by the reference value.
        /// </summary>
        public bool IsConstant = false;

        private int bitBuf = 0;
        private int bitPos = 0;        

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        /// <param name="decimalscale">decimal scale</param>
        /// <param name="bms">GRIB 1 BMS</param>
        /// <param name="scanMode">scan mode</param>
        /// <param name="Xlength">X coordinate number</param>
        /// <param name="Ylength">Y coordinate number</param>
        public GRIB1BinaryDataSection(BinaryReader br, int decimalscale, GRIB1BitMapSection bms, int scanMode, 
            int Xlength, int Ylength)
        {
            byte[] bytes = br.ReadBytes(3);
            Length = Bytes2Number.Uint3(bytes[0], bytes[1], bytes[2]);

            br.BaseStream.Seek(-3, SeekOrigin.Current);
            bytes = br.ReadBytes(11);    //Read before the data array.

            // octet 4, 1st half (packing flag)
            int unusedbits = Convert.ToInt32(bytes[3]);
            if ((unusedbits & 192) != 0)
            {
                throw new NotSupportedException(
                    "Grib1BinaryDataSection: (octet 4, 1st half) not grid point data and simple packing ");
            }

            // octet 4, 2nd half (number of unused bits at end of this section)
            unusedbits = unusedbits & 15;            
            BinScale = Bytes2Number.Int2(bytes[4], bytes[5]);            
            RefValue = Bytes2Number.Float4(bytes[6], bytes[7], bytes[8], bytes[9]);
            NumBits = Convert.ToInt32(bytes[10]);
            if (NumBits == 0)
                IsConstant = true;

            // *** read values ************************************************************

            float refd = (float)(Math.Pow(10.0, -decimalscale) * RefValue);
            float scale = (float)(Math.Pow(10.0, -decimalscale) * Math.Pow(2.0, BinScale));

            if (bms != null)
            {
                bool[] bitmap = bms.Bitmap;

                Data = new double[bitmap.Length];
                for (int i = 0; i < bitmap.Length; i++)
                {
                    if (bitmap[i])
                    {
                        if (!IsConstant)
                        {
                            Data[i] = refd + scale * Bits2UInt(NumBits, br);
                            if (Data[i] > MaxValue)
                                MaxValue = Data[i];
                            if (Data[i] < MinValue)
                                MinValue = Data[i];
                        }
                        else
                        {// rdg - added this to handle a constant valued parameter
                            Data[i] = refd;
                        }
                    }
                    else
                        Data[i] = UNDEF;
                }
                ScanningModeCheck(scanMode, Xlength);
            }
            else
            {
                if (!IsConstant)
                {
                    Data = new double[((Length - 11) * 8 - unusedbits) / NumBits];

                    for (int i = 0; i < Data.Length; i++)
                    {
                        Data[i] = refd + scale * Bits2UInt(NumBits, br);                        

                        if (Data[i] > MaxValue)
                            MaxValue = this.Data[i];
                        if (Data[i] < MinValue)
                            MinValue = this.Data[i];
                    }
                    ScanningModeCheck(scanMode, Xlength);
                }
                else
                { // constant valued - same min and max
                    Data = new double[Xlength * Ylength];
                    MaxValue = refd;
                    MinValue = refd;
                    for (int i = 0; i < Data.Length; i++)
                        Data[i] = refd;
                }
            }
        }

        #endregion

        #region Methods
        private int Bits2UInt(int nb, BinaryReader br)
        {
            int bitsLeft = nb;
            int result = 0;

            if (bitPos == 0)
            {
                //bitBuf = br.Read();
                bitBuf = br.ReadByte();
                bitPos = 8;
            }

            while (true)
            {
                int shift = bitsLeft - bitPos;
                if (shift > 0)
                {
                    // Consume the entire buffer
                    result |= bitBuf << shift;                    
                    bitsLeft -= bitPos;

                    // Get the next byte from the RandomAccessFile
                    //bitBuf = br.Read();
                    bitBuf = br.ReadByte();
                    bitPos = 8;
                }
                else
                {
                    // Consume a portion of the buffer
                    result |= bitBuf >> -shift;
                    bitPos -= bitsLeft;
                    bitBuf &= 0xff >> (8 - bitPos);  // mask off consumed bits

                    return result;
                }
            }
        }

        private void ScanningModeCheck(int scanMode, int Xlength)
        {
            double tmp;
            int mid = (int)Xlength / 2;

            // Mode  0 +x, -y, adjacent x, adjacent rows same dir
            // Mode  64 +x, +y, adjacent x, adjacent rows same dir
            if ((scanMode == 0) || (scanMode == 64))
            {
                return;
            }
            // Mode  128 -x, -y, adjacent x, adjacent rows same dir
            // Mode  192 -x, +y, adjacent x, adjacent rows same dir
            // change -x to +x ie east to west -> west to east
            else if ((scanMode == 128) || (scanMode == 192))
            {
                mid = (int)Xlength / 2;
                //System.out.println( "Xlength =" +Xlength +" mid ="+ mid );
                for (int index = 0; index < Data.Length; index += Xlength)
                {
                    for (int idx = 0; idx < mid; idx++)
                    {
                        tmp = Data[index + idx];
                        Data[index + idx] = Data[index + Xlength - idx - 1];
                        Data[index + Xlength - idx - 1] = tmp;
                        //System.out.println( "switch " + (index + idx) + " " +
                        //(index + Xlength -idx -1) );
                    }
                }
                return;
            }            
        }

        #endregion
    }
}
