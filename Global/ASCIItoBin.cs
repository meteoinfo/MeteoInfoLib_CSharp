using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Global
{
    /// <summary>
    /// String to binary array
    /// </summary>
    class ASCIItoBin
    {
        /// <summary>
        /// Convert string to desired length byte array
        /// </summary>
        /// <param name="theString">The string need to be converted</param>
        /// <param name="desiredLength">Desired length of byte array</param>
        /// <returns>Byte array</returns>
        public byte[] ConvertToByte(string theString, int desiredLength)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] byteStream = new byte[(uint)desiredLength];
            encoder.GetBytes(theString, 0, (theString.Length <= desiredLength) ? theString.Length : desiredLength, byteStream, 0);
            return byteStream;
        }
    } 
}
