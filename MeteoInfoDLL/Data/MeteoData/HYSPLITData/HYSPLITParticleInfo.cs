using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// HYSPLIT particle dump data info
    /// </summary>
    public class HYSPLITParticleInfo : DataInfo,IStationDataInfo
    {
        #region Variables
        /// <summary>
        /// list of particle number, pollutant number and stream start position
        /// </summary>
        public List<List<int>> Parameters;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public HYSPLITParticleInfo()
        {
            Parameters = new List<List<int>>();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read HYSPLIT particle dump data info
        /// </summary>
        /// <param name="aFile">Particle dump file name path</param>
        /// <returns>HYSPLIT particle data info</returns>
        public override void ReadDataInfo(string aFile)
        {
            FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int year, month, day, hour;
            Times = new List<DateTime>();
            Parameters = new List<List<int>>();

            this.FileName = aFile;
            List<DateTime> times = new List<DateTime>();
            List<double> values = new List<double>();
            while (br.BaseStream.Position < br.BaseStream.Length - 28)
            {
                //Read head
                int pos = (int)br.BaseStream.Position;
                br.ReadBytes(4);
                int particleNum = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                int pollutantNum = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                year = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                month = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                day = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                hour = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                string dStr = year.ToString("00") + "-" + month.ToString("00") +
                        "-" + day.ToString("00") + " " + hour.ToString("00");
                DateTime time = DateTime.ParseExact(dStr, "yy-MM-dd HH", System.Globalization.CultureInfo.InvariantCulture);
                times.Add(time);
                values.Add(DataConvert.ToDouble(time));
                Parameters.Add(new List<int>(new int[] { particleNum, pollutantNum, pos }));

                //Skip data
                int len = (8 + pollutantNum * 4 + 60) * particleNum + 4;
                br.ReadBytes(len);
            }
            Dimension tdim = new Dimension(DimensionType.T);
            tdim.SetValues(values);
            Variable var = new Variable();
            var.Name = "Particle";
            var.IsStation = true;
            var.SetDimension(tdim);
            List<Variable> variables = new List<Variable>();
            variables.Add(var);
            this.Variables = variables;

            br.Close();
            fs.Close();            
        }

        ///// <summary>
        ///// Read HYSPLIT particle data
        ///// </summary>        
        ///// <param name="dataExtent">ref data extent</param>
        ///// <returns>discrete data</returns>
        //public double[,] ReadParticleData(ref Global.Extent dataExtent)
        //{
        //    double[,] DiscreteData = new double[3, particleNum];

        //    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
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
        //    for (i = 0; i < particleNum; i++)
        //    {
        //        br.ReadBytes(8);
        //        for (j = 0; j < pollutantNum; j++)
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

        /// <summary>
        /// Read station data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Station data</returns>
        public StationData GetStationData(int timeIdx, int varIdx, int levelIdx)
        {
            StationData stationData = new StationData();
            List<string> stations = new List<string>();
            int particleNum = Parameters[timeIdx][0];
            int pollutantNum = Parameters[timeIdx][1];
            int pos = Parameters[timeIdx][2];
            double[,] DiscreteData = new double[3, particleNum];

            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] aBytes;
            int i, j;
            Single lon, lat, alt;
            Single minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;

            br.BaseStream.Seek(pos, SeekOrigin.Begin);
            br.ReadBytes(28);
            for (i = 0; i < particleNum; i++)
            {
                br.ReadBytes(8);
                for (j = 0; j < pollutantNum; j++)
                {
                    br.ReadBytes(4);
                }
                br.ReadBytes(8);
                aBytes = br.ReadBytes(4);
                Array.Reverse(aBytes);
                lat = BitConverter.ToSingle(aBytes, 0);
                aBytes = br.ReadBytes(4);
                Array.Reverse(aBytes);
                lon = BitConverter.ToSingle(aBytes, 0);
                aBytes = br.ReadBytes(4);
                Array.Reverse(aBytes);
                alt = BitConverter.ToSingle(aBytes, 0);

                DiscreteData[0, i] = lon;
                DiscreteData[1, i] = lat;
                DiscreteData[2, i] = alt;
                stations.Add("P" + (i + 1).ToString());

                br.ReadBytes(40);

                if (i == 0)
                {
                    minX = lon;
                    maxX = minX;
                    minY = lat;
                    maxY = minY;
                }
                else
                {
                    if (minX > lon)
                    {
                        minX = lon;
                    }
                    else if (maxX < lon)
                    {
                        maxX = lon;
                    }
                    if (minY > lat)
                    {
                        minY = lat;
                    }
                    else if (maxY < lat)
                    {
                        maxY = lat;
                    }
                }
            }
            MeteoInfoC.Global.Extent dataExtent = new MeteoInfoC.Global.Extent();
            dataExtent.minX = minX;
            dataExtent.maxX = maxX;
            dataExtent.minY = minY;
            dataExtent.maxY = maxY;

            br.Close();
            fs.Close();

            stationData.Data = DiscreteData;
            stationData.DataExtent = dataExtent;
            stationData.Stations = stations;

            return stationData;
        }

        /// <summary>
        /// Get HYSPLIT particle dump data info text
        /// </summary>       
        /// <returns>data information text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + this.FileName;
            for (int i = 0; i < Times.Count; i++)
            {
                dataInfo += Environment.NewLine + "Time: " + Times[i].ToString("yyyy-MM-dd HH:00");
                dataInfo += Environment.NewLine + "\tParticle Number: " + Parameters[i][0];
                dataInfo += Environment.NewLine + "\tPollutant Number: " + Parameters[i][1];
            }

            return dataInfo;
        }

        /// <summary>
        /// Read station info data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>StationInfoData</returns>
        public StationInfoData GetStationInfoData(int timeIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Read station model data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Station data</returns>
        public StationModelData GetStationModelData(int timeIdx, int levelIdx)
        {
            return null;
        }

        #endregion
    }
}
