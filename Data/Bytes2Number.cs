using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data
{
    /// <summary>
    /// Convert bytes to number
    /// </summary>
    public static class Bytes2Number
    {
        #region Variables
        /// <summary>
        /// Undefine data
        /// </summary>
        public static double UNDEF = -9999.0;

        #endregion

        #region Methods
        /// <summary>
        /// Convert two bytes into a signed integer.
        /// </summary>
        /// <param name="a">higher byte</param>
        /// <param name="b">lower byte</param>
        /// <returns>integer value</returns>
        public static int Int2(byte a, byte b)
        {

            return (1 - ((a & 128) >> 6)) * ((a & 127) << 8 | b);
        }

        /// <summary>
        /// Read signed integer of 2 bytes from binary reader
        /// </summary>
        /// <param name="br">binary reader</param>
        /// <returns>integer value</returns>
        public static int Int2(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(2);
            return Int2(bytes[0], bytes[1]);
        }

        /// <summary>
        /// Convert three bytes into a signed integer.
        /// </summary>
        /// <param name="a">higher byte</param>
        /// <param name="b">middle part byte</param>
        /// <param name="c">lower byte</param>
        /// <returns>integer value</returns>
        public static int Int3(byte a, byte b, byte c)
        {

            return (1 - ((a & 128) >> 6)) * ((a & 127) << 16 | b << 8 | c);
        }

        /// <summary>
        /// Convert four bytes into a signed integer.
        /// </summary>
        /// <param name="a">highest byte</param>
        /// <param name="b">higher middle byte</param>
        /// <param name="c">lower middle byte</param>
        /// <param name="d">lowest byte</param>
        /// <returns>integer value</returns>
        public static int Int4(byte a, byte b, byte c, byte d)
        {

            return (1 - ((a & 128) >> 6)) * ((a & 127) << 24 | b << 16 | c << 8 | d);
        }

        /// <summary>
        /// Read signed integer of 4 bytes from binary reader
        /// </summary>
        /// <param name="br">binary reader</param>
        /// <returns>integer value</returns>
        public static int Int4(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(4);
            return Int4(bytes[0], bytes[1], bytes[2], bytes[3]);
        }

        /// <summary>
        /// Convert two bytes into an unsigned integer.
        /// </summary>
        /// <param name="a">higher byte</param>
        /// <param name="b">lower byte</param>
        /// <returns>integer value</returns>
        public static int Uint2(byte a, byte b)
        {

            return a << 8 | b;
        }

        /// <summary>
        /// Read unsigned integer from binary reader
        /// </summary>
        /// <param name="br">binary reader</param>
        /// <returns>unsigned integer</returns>
        public static int Uint2(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(2);
            return Uint2(bytes[0], bytes[1]);
        }

        /// <summary>
        /// Convert three bytes into an unsigned integer.
        /// </summary>
        /// <param name="a">higher byte</param>
        /// <param name="b">middle byte</param>
        /// <param name="c">lower byte</param>
        /// <returns>integer value</returns>
        public static int Uint3(byte a, byte b, byte c)
        {

            return a << 16 | b << 8 | c;
        }

        /// <summary>
        /// Convert four bytes into an unsigned integer
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int Uint4(byte a, byte b, byte c, byte d)
        {
            return a << 24 | b << 16 | c << 8 | d;
        }

        /// <summary>
        /// Read a float value from binary reader
        /// </summary>
        /// <param name="br">binary reader</param>
        /// <returns>float value</returns>
        public static float Float(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(4);

            return Ieee2Float(bytes);

            ////int i = (bytes[3] & 0xff) << 24 | (bytes[2] & 0xff) << 16 | (bytes[1] & 0xff) << 8 | bytes[0] & 0xff;
            //int ch1 = bytes[0];
            //int ch2 = bytes[1];
            //int ch3 = bytes[2];
            //int ch4 = bytes[3];
            //int i = ((ch4 << 24) + (ch3 << 16) + (ch2 << 8) + (ch1));

            //return IntBitsToFloat(i);

            //return (float)BitConverter.Int64BitsToDouble(i);
        }

        private static float Ieee2Float(byte[] ieee)
        {
            double fmant;
            int exp;

            if ((ieee[0] & 127) == 0 && ieee[1] == 0 && ieee[2] == 0 && ieee[3] == 0)
                return (float)0.0;

            exp = ((ieee[0] & 127) << 1) + (ieee[1] >> 7);
            fmant = (double)((int)ieee[3] + (int)(ieee[2] << 8) +
                      (int)((ieee[1] | 128) << 16));
            if ((ieee[0] & 128) > 0) fmant = -fmant;
            return (float)(fmant * Math.Pow(2.0, (int)(exp - 128 - 22)));
        }

        private static float IntBitsToFloat(int bits)
        {
            int s = ((bits >> 31) == 0) ? 1 : -1;
            int e = ((bits >> 23) & 0xff);
            int m = (bits & 0x7fffff) | 0x800000;
            float f = float.NaN;
            if (e == 0)
            {
                f = (float)(s * m * Math.Pow(2.0, -149));
            }
            else
            {
                f = (float)(s * (1.0 + m * Math.Pow(2.0, -23)) * Math.Pow(2.0, e - 127));
            }
            
            return f;
        }

        //private static float IntBitsToFloat(int bits)
        //{
        //    int s = ((bits >> 31) == 0) ? 1 : -1;
        //    int e = ((bits >> 23) & 0xff);
        //    int m = (e == 0) ?
        //                    (bits & 0x7fffff) << 1 :
        //                    (bits & 0x7fffff) | 0x800000;

        //    float f = (float)(s * m * Math.Pow(2.0, e - 150));
        //    return f;
        //}

        /// <summary>
        /// Read a float value from binary reader
        /// </summary>
        /// <param name="br">binary reader</param>
        /// <returns>float value</returns>
        public static float Float4(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(4);
            return Float4(bytes[0], bytes[1], bytes[2], bytes[3]);
        }

        /// <summary>
        /// Convert four bytes into a float value.
        /// </summary>
        /// <param name="a">highest byte</param>
        /// <param name="b">higher byte</param>
        /// <param name="c">lower byte</param>
        /// <param name="d">lowest byte</param>
        /// <returns>float value</returns>
        public static float Float4(byte a, byte b, byte c, byte d)
        {

            int sgn, mant, exp;

            mant = b << 16 | c << 8 | d;
            if (mant == 0) return 0.0f;

            sgn = -(((a & 128) >> 6) - 1);
            exp = (a & 127) - 64;

            // TODO Validate, that this method works always
            return (float)(sgn * Math.Pow(16.0, exp - 6) * mant);
        }

        /// <summary>
        /// Convert bytes array into an unsigned integer
        /// </summary>
        /// <param name="bytes">bytes array</param>
        /// <returns>integer value</returns>
        public static int UInt(byte[] bytes)
        {
            int ivalue = bytes[0] << (8 * (bytes.Length - 1));
            for (int i = 1; i < bytes.Length; i++)
            {
                ivalue = ivalue | bytes[i] << (8 * (bytes.Length - i - 1));
            }

            return ivalue;
        }

        /// <summary>
        /// Convert 8 bytes into a signed long.
        /// </summary>
        /// <param name="bytes">8 bytes</param>
        /// <returns>signed long</returns>
        public static long Int8(byte[] bytes)
        {
            int a = bytes[0];
            int b = bytes[1];
            int c = bytes[2];
            int d = bytes[3];
            int e = bytes[4];
            int f = bytes[5];
            int g = bytes[6];
            int h = bytes[7];

            return (1 - ((a & 128) >> 6))
                   * ((a & 127) << 56 | b << 48 | c << 40 | d << 32 | e << 24
                      | f << 16 | g << 8 | h);

        }

        #endregion

    }
}
