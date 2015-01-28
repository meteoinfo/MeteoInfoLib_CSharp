using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB data info
    /// </summary>
    public static class GRIBData
    {
        /// <summary>
        /// Get GRIB edition number
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <param name="headerLen">header length</param>
        /// <returns>GRIB edition number</returns>
        public static int GetGRIBEdition(string aFile, ref int headerLen)
        {
            FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            headerLen = 0;
            string title = ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
            if (title != "GRIB")
            {
                headerLen = 1;
                br.BaseStream.Seek(1, SeekOrigin.Begin);
                for (int i = 0; i < 100; i++)
                {
                    title = ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
                    if (title == "GRIB")
                        break;

                    headerLen += 1;
                    br.BaseStream.Seek(headerLen, SeekOrigin.Begin);
                }
            }
            br.ReadBytes(3);

            int edition = Convert.ToInt32(br.ReadByte());

            br.Close();
            fs.Close();

            return edition;
        }

        /// <summary>
        /// Read section number - GRIB 2
        /// </summary>
        /// <param name="br">binary reader</param>
        /// <returns>section number</returns>
        public static int ReadSectionNumber(BinaryReader br)
        {
            if (br.BaseStream.Position + 16 >= br.BaseStream.Length)
                return -1;

            br.ReadBytes(4);
            int sectionNum = br.ReadByte();
            br.BaseStream.Seek(-5, SeekOrigin.Current);

            return sectionNum;
        }

        /// <summary>
        /// Get mean longitude between tow longitude
        /// </summary>
        /// <param name="lon1">longitude 1</param>
        /// <param name="lon2">longitude 2</param>
        /// <returns>mean longitude</returns>
        public static double GetMeanLongitude(double lon1, double lon2)
        {
            double meanLon;
            if (lon1 < lon2)
                meanLon = (lon1 + lon2) / 2;
            else
            {
                meanLon = (lon1 + lon2 + 360) / 2 ;
                if (meanLon > 360)
                    meanLon -= 360;
            }

            return meanLon;
        }
    }
}
