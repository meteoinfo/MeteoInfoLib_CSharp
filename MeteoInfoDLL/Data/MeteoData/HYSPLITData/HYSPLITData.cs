using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using MeteoInfoC.Layer;
using MeteoInfoC.Shape;
using MeteoInfoC.Legend;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// HYSPLIT data operation
    /// </summary>
    public class HYSPLITData
    {
        /// <summary>
        /// Read HYSPLIT concentration data info
        /// </summary>
        /// <param name="aFile">File path</param>
        /// <returns></returns>
        public HYSPLITConcDataInfo ReadConcDataInfo(string aFile)
        {
            FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, j, hBytes;
            byte[] aBytes;
            HYSPLITConcDataInfo aDataInfo = new HYSPLITConcDataInfo();

            aDataInfo.FileName = aFile;

            //Record #1
            br.ReadBytes(4);
            aBytes = br.ReadBytes(4);
            aDataInfo.Ident = System.Text.ASCIIEncoding.ASCII.GetString(aBytes);
            aDataInfo.year = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            aDataInfo.month = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            aDataInfo.day = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            aDataInfo.hour = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            aDataInfo.forecast_hour = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            aDataInfo.loc_num = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            aDataInfo.pack_flag = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());

            //Record #2
            aDataInfo.locArray = new object[8, aDataInfo.loc_num];
            for (i = 0; i < aDataInfo.loc_num; i++)
            {
                br.ReadBytes(8);
                for (j = 0; j < 4; j++)
                {
                    aDataInfo.locArray[j, i] = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                }
                for (j = 4; j < 7; j++)
                {
                    aBytes = br.ReadBytes(4);
                    Array.Reverse(aBytes);
                    aDataInfo.locArray[j, i] = BitConverter.ToSingle(aBytes, 0);
                }
                aDataInfo.locArray[7, i] = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            }

            //Record #3
            string fName = Path.GetFileName(aFile).ToLower();
            if (fName.Contains("gemzint"))
            {
                br.ReadBytes(4);   //For vertical concentration file gemzint
            }
            else
            {
                br.ReadBytes(8); 
            }                    
            aDataInfo.lat_point_num = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            aDataInfo.lon_point_num = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            aBytes = br.ReadBytes(4);
            Array.Reverse(aBytes);
            aDataInfo.lat_delta = BitConverter.ToSingle(aBytes, 0);
            aBytes = br.ReadBytes(4);
            Array.Reverse(aBytes);
            aDataInfo.lon_delta = BitConverter.ToSingle(aBytes, 0);
            aBytes = br.ReadBytes(4);
            Array.Reverse(aBytes);
            aDataInfo.lat_LF = BitConverter.ToSingle(aBytes, 0);
            aBytes = br.ReadBytes(4);
            Array.Reverse(aBytes);
            aDataInfo.lon_LF = BitConverter.ToSingle(aBytes, 0);

            aDataInfo.X = new double[aDataInfo.lon_point_num];
            aDataInfo.Y = new double[aDataInfo.lat_point_num];
            for (i = 0; i < aDataInfo.lon_point_num; i++)
            {
                aDataInfo.X[i] = aDataInfo.lon_LF + i * aDataInfo.lon_delta;
            }
            if (aDataInfo.X[0] == 0 && aDataInfo.X[aDataInfo.X.Length - 1] +
                aDataInfo.lon_delta == 360)
            {
                aDataInfo.IsGlobal = true;
            }
            for (i = 0; i < aDataInfo.lat_point_num; i++)
            {
                aDataInfo.Y[i] = aDataInfo.lat_LF + i * aDataInfo.lat_delta;
            }

            //Record #4
            br.ReadBytes(8);
            aDataInfo.level_num = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            aDataInfo.heights = new int[aDataInfo.level_num];
            for (i = 0; i < aDataInfo.level_num; i++)
            {
                aDataInfo.heights[i] = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            }

            //Record #5
            br.ReadBytes(8);
            aDataInfo.pollutant_num = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            aDataInfo.pollutants = new string[aDataInfo.pollutant_num];
            for (i = 0; i < aDataInfo.pollutant_num; i++)
            {
                aBytes = br.ReadBytes(4);
                aDataInfo.pollutants[i] = System.Text.ASCIIEncoding.ASCII.GetString(aBytes);
            }

            hBytes = 36 + aDataInfo.lon_point_num * 40 + 32 + 12 + aDataInfo.level_num * 4 + 12 +
                aDataInfo.pollutant_num * 4;
            aDataInfo.hByte_num = hBytes;

            //Record Data
            int k, tNum;
            tNum = 0;
            int[] sampleTimes = new int[6];
            string dStr;
            DateTime aDateTime;
            aDataInfo.sample_start = new List<DateTime>();
            aDataInfo.sample_stop = new List<DateTime>();
            do
            {
                //Record #6
                br.ReadBytes(8);
                for (i = 0; i < 6; i++)
                {
                    sampleTimes[i] = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                }
                dStr = sampleTimes[0].ToString("00") + "-" + sampleTimes[1].ToString("00") +
                    "-" + sampleTimes[2].ToString("00") + " " + sampleTimes[3].ToString("00");
                aDateTime = DateTime.ParseExact(dStr, "yy-MM-dd HH", System.Globalization.CultureInfo.InvariantCulture);
                aDataInfo.sample_start.Add(aDateTime);

                //Record #7
                br.ReadBytes(8);
                for (i = 0; i < 6; i++)
                {
                    sampleTimes[i] = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                }
                dStr = sampleTimes[0].ToString("00") + "-" + sampleTimes[1].ToString("00") +
                    "-" + sampleTimes[2].ToString("00") + " " + sampleTimes[3].ToString("00");
                aDateTime = DateTime.ParseExact(dStr, "yy-MM-dd HH", System.Globalization.CultureInfo.InvariantCulture);
                aDataInfo.sample_stop.Add(aDateTime);

                //Record 8;
                int aLevel, aN, IP, JP;
                string aType;
                for (i = 0; i < aDataInfo.pollutant_num; i++)
                {
                    for (j = 0; j < aDataInfo.level_num; j++)
                    {
                        if (aDataInfo.pack_flag == 1)
                        {
                            br.ReadBytes(8);
                            aBytes = br.ReadBytes(4);
                            aType = System.Text.ASCIIEncoding.ASCII.GetString(aBytes);
                            aLevel = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                            aN = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                            for (k = 0; k < aN; k++)
                            {
                                if (br.BaseStream.Position + 8 > br.BaseStream.Length)
                                {
                                    break;
                                }
                                IP = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt16());
                                JP = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt16());
                                aBytes = br.ReadBytes(4);
                            }
                        }
                        else
                        {
                            br.ReadBytes(8);
                            aBytes = br.ReadBytes(4);
                            aType = System.Text.ASCIIEncoding.ASCII.GetString(aBytes);
                            aLevel = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                            for (JP = 0; JP < aDataInfo.lat_point_num; JP++)
                            {
                                for (IP = 0; IP < aDataInfo.lon_point_num; IP++)
                                {
                                    aBytes = br.ReadBytes(4);
                                }
                            }
                        }
                    }
                }

                tNum += 1;

                if (br.BaseStream.Position + 10 > br.BaseStream.Length)
                {
                    break;
                }
            } while (true);
            aDataInfo.time_num = tNum;

            br.Close();
            fs.Close();

            return aDataInfo;
        }

        /// <summary>
        /// Read HYSPLIT concentration data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <param name="aDataInfo">HYSPLIT concentration data info</param>
        /// <param name="aFactor">Data scale factor</param>
        /// <returns></returns>
        public double[,] ReadConcData(int timeIdx, int varIdx, int levelIdx, HYSPLITConcDataInfo aDataInfo, int aFactor)
        {
            FileStream fs = new FileStream(aDataInfo.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, j, nBytes;
            byte[] aBytes;
            double[,] dataArray = new double[aDataInfo.lon_point_num, aDataInfo.lat_point_num];
            double[,] newDataArray = new double[aDataInfo.lat_point_num, aDataInfo.lon_point_num];

            //Record #1            
            br.ReadBytes(36);

            //Record #2
            nBytes = (8 * 4 + 8) * aDataInfo.loc_num;
            br.ReadBytes(nBytes);

            //Record #3
            string fName = Path.GetFileName(aDataInfo.FileName).ToLower();
            if (fName.Contains("gemzint"))
            {
                br.ReadBytes(28);   //For vertical concentration file gemzint
            }
            else
            {
                br.ReadBytes(32);
            }                

            //Record #4
            nBytes = 12 + aDataInfo.level_num * 4;
            br.ReadBytes(nBytes);

            //Record #5
            nBytes = 12 + aDataInfo.pollutant_num * 4;
            br.ReadBytes(nBytes);

            //Record Data
            int t, k;
            int aLevel, aN, IP, JP;
            string aType;
            double aConc;
            for (t = 0; t < aDataInfo.time_num; t++)
            {
                br.ReadBytes(64);

                for (i = 0; i < aDataInfo.pollutant_num; i++)
                {
                    for (j = 0; j < aDataInfo.level_num; j++)
                    {
                        if (t == timeIdx && i == varIdx && j == levelIdx)
                        {
                            if (br.BaseStream.Position + 28 > br.BaseStream.Length)
                            {
                                break;
                            }
                            if (aDataInfo.pack_flag == 1)
                            {
                                br.ReadBytes(8);
                                aBytes = br.ReadBytes(4);
                                aType = System.Text.ASCIIEncoding.ASCII.GetString(aBytes);
                                aLevel = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                                aN = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                                for (k = 0; k < aN; k++)
                                {
                                    if (br.BaseStream.Position + 8 > br.BaseStream.Length)
                                    {
                                        break;
                                    }
                                    IP = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt16());
                                    JP = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt16());
                                    aBytes = br.ReadBytes(4);
                                    Array.Reverse(aBytes);
                                    aConc = BitConverter.ToSingle(aBytes, 0);
                                    if (IP >= 0 && IP < aDataInfo.lon_point_num && JP >= 0 && JP < aDataInfo.lat_point_num)
                                    {
                                        dataArray[IP, JP] = aConc * Math.Pow(10, aFactor);
                                    }
                                }
                            }
                            else
                            {
                                br.ReadBytes(8);
                                aBytes = br.ReadBytes(4);
                                aType = System.Text.ASCIIEncoding.ASCII.GetString(aBytes);
                                aLevel = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                                for (JP = 0; JP < aDataInfo.lat_point_num; JP++)
                                {
                                    for (IP = 0; IP < aDataInfo.lon_point_num; IP++)
                                    {
                                        aBytes = br.ReadBytes(4);
                                        Array.Reverse(aBytes);
                                        aConc = BitConverter.ToSingle(aBytes, 0);
                                        dataArray[IP, JP] = aConc * Math.Pow(10, aFactor);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (br.BaseStream.Position + 28 > br.BaseStream.Length)
                            {
                                break;
                            }
                            if (aDataInfo.pack_flag == 1)
                            {
                                br.ReadBytes(8);
                                aBytes = br.ReadBytes(4);
                                aType = System.Text.ASCIIEncoding.ASCII.GetString(aBytes);
                                aLevel = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                                aN = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                                for (k = 0; k < aN; k++)
                                {
                                    if (br.BaseStream.Position + 8 > br.BaseStream.Length)
                                    {
                                        break;
                                    }
                                    IP = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt16());
                                    JP = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt16());
                                    aBytes = br.ReadBytes(4);
                                }
                            }
                            else
                            {
                                br.ReadBytes(8);
                                aBytes = br.ReadBytes(4);
                                aType = System.Text.ASCIIEncoding.ASCII.GetString(aBytes);
                                aLevel = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                                for (JP = 0; JP < aDataInfo.lat_point_num; JP++)
                                {
                                    for (IP = 0; IP < aDataInfo.lon_point_num; IP++)
                                    {
                                        aBytes = br.ReadBytes(4);
                                    }
                                }
                            }
                        }
                    }
                }

                if (br.BaseStream.Position + 10 > br.BaseStream.Length)
                {
                    break;
                }
            }

            br.Close();
            fs.Close();

            if (aDataInfo.IsGlobal)
            {
                newDataArray = new double[aDataInfo.lat_point_num, aDataInfo.lon_point_num + 1];
            }
            for (i = 0; i < aDataInfo.lon_point_num; i++)
            {
                for (j = 0; j < aDataInfo.lat_point_num; j++)
                {
                    newDataArray[j, i] = dataArray[i, j];
                }
            }
            if (aDataInfo.IsGlobal)
            {
                for (i = 0; i < aDataInfo.lat_point_num; i++)
                {
                    newDataArray[i, aDataInfo.lon_point_num] = newDataArray[i, 0];
                }
            }

            return newDataArray;
        }

        /// <summary>
        /// Get HYSPLIT concentration data info text
        /// </summary>
        /// <param name="aDataInfo">HYSPLIT concentration data info</param>
        /// <returns></returns>
        public string GenerateInfoText(HYSPLITConcDataInfo aDataInfo)
        {
            string dataInfo;
            dataInfo = "File Name: " + aDataInfo.FileName;
            dataInfo += Environment.NewLine + "Pack Flag = " + aDataInfo.pack_flag.ToString();
            dataInfo += Environment.NewLine + "Xsize = " + aDataInfo.lon_point_num.ToString() +
                    "  Ysize = " + aDataInfo.lat_point_num.ToString() + "  Zsize = " + aDataInfo.level_num.ToString() +
                    "  Tsize = " + aDataInfo.time_num.ToString();
            dataInfo += Environment.NewLine + "Number of Variables = " + aDataInfo.pollutant_num.ToString();
            foreach (string v in aDataInfo.pollutants)
            {
                dataInfo += Environment.NewLine + v;
            }

            return dataInfo;
        }

        ///// <summary>
        ///// Read HYSPLIT particle dump data info
        ///// </summary>
        ///// <param name="aFile">Particle dump file name path</param>
        ///// <returns></returns>
        //public HYSPLITParticleInfo ReadParticleDataInfo(string aFile)
        //{
        //    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    int year, month, day, hour;            
        //    HYSPLITParticleInfo aDataInfo = new HYSPLITParticleInfo();

        //    aDataInfo.fileName = aFile;
        //    br.ReadBytes(4);
        //    aDataInfo.particleNum = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
        //    aDataInfo.pollutantNum = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
        //    year = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
        //    month = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
        //    day = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
        //    hour = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
        //    string dStr = year.ToString("00") + "-" + month.ToString("00") +
        //            "-" + day.ToString("00") + " " + hour.ToString("00");
        //    aDataInfo.dateTime = DateTime.ParseExact(dStr, "yy-MM-dd HH", System.Globalization.CultureInfo.InvariantCulture);

        //    br.Close();
        //    fs.Close();

        //    return aDataInfo;
        //}

        ///// <summary>
        ///// Read HYSPLIT particle data
        ///// </summary>
        ///// <param name="aDataInfo"></param>
        ///// <param name="dataExtent"></param>
        ///// <returns></returns>
        //public double[,] ReadParticleData(HYSPLITParticleInfo aDataInfo, ref Global.Extent dataExtent)
        //{
        //    double[,] DiscreteData = new double[3, aDataInfo.particleNum];

        //    FileStream fs = new FileStream(aDataInfo.fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] aBytes;
        //    int i, j;
        //    Single lon, lat, alt;
        //    Single minX, maxX, minY, maxY;
        //    minX = 0;
        //    maxX = 0;
        //    minY = 0;
        //    maxY = 0;

        //    br.ReadBytes(28);
        //    for (i = 0; i < aDataInfo.particleNum; i++)
        //    {
        //        br.ReadBytes(8);
        //        for (j = 0; j < aDataInfo.pollutantNum; j++)
        //        {
        //            br.ReadBytes(4);
        //        }
        //        br.ReadBytes(8);
        //        aBytes = br.ReadBytes(4);
        //        Array.Reverse(aBytes);
        //        lat = BitConverter.ToSingle(aBytes, 0);
        //        aBytes = br.ReadBytes(4);
        //        Array.Reverse(aBytes);
        //        lon = BitConverter.ToSingle(aBytes, 0);
        //        aBytes = br.ReadBytes(4);
        //        Array.Reverse(aBytes);
        //        alt = BitConverter.ToSingle(aBytes, 0);

        //        DiscreteData[0, i] = lon;
        //        DiscreteData[1, i] = lat;
        //        DiscreteData[2, i] = alt;

        //        br.ReadBytes(40);

        //        if (i == 0)
        //        {
        //            minX = lon;
        //            maxX = minX;
        //            minY = lat;
        //            maxY = minY;
        //        }
        //        else
        //        {
        //            if (minX > lon)
        //            {
        //                minX = lon;
        //            }
        //            else if (maxX < lon)
        //            {
        //                maxX = lon;
        //            }
        //            if (minY > lat)
        //            {
        //                minY = lat;
        //            }
        //            else if (maxY < lat)
        //            {
        //                maxY = lat;
        //            }
        //        }
        //    }

        //    dataExtent.minX = minX;
        //    dataExtent.maxX = maxX;
        //    dataExtent.minY = minY;
        //    dataExtent.maxY = maxY;

        //    br.Close();
        //    fs.Close();

        //    return DiscreteData;
        //}

        ///// <summary>
        ///// Get HYSPLIT particle dump data info text
        ///// </summary>
        ///// <param name="aDataInfo">HYSPLIT particle data info</param>
        ///// <returns></returns>
        //public string GenerateInfoText(HYSPLITParticleInfo aDataInfo)
        //{
        //    string dataInfo;
        //    dataInfo = "File Name: " + aDataInfo.fileName;            
        //    dataInfo += Environment.NewLine + "Time: " + aDataInfo.dateTime.ToString("yyyy-MM-dd HH:00");
        //    dataInfo += Environment.NewLine + "Particle Number: " + aDataInfo.particleNum;
        //    dataInfo += Environment.NewLine + "Pollutant Number: " + aDataInfo.pollutantNum;

        //    return dataInfo;
        //}        
    }
}
