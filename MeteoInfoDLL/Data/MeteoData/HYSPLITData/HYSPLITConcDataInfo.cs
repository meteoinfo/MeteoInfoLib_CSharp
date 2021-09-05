using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// HYSPLIT concentration data info
    /// </summary>
    public class HYSPLITConcDataInfo : DataInfo,IGridDataInfo
    {
        #region Variables
        /// <summary>
        /// Specie identifer
        /// </summary>
        public string Ident;
        /// <summary>
        /// Year
        /// </summary>
        public int year;
        /// <summary>
        /// Month
        /// </summary>
        public int month;
        /// <summary>
        /// Day
        /// </summary>
        public int day;
        /// <summary>
        /// Hour
        /// </summary>
        public int hour;
        /// <summary>
        /// Forecast hour
        /// </summary>
        public int forecast_hour;
        /// <summary>
        /// Location number
        /// </summary>
        public int loc_num;
        /// <summary>
        /// Pack flag
        /// </summary>
        public int pack_flag;
        /// <summary>
        /// Location array
        /// </summary>
        public object[,] locArray;
        /// <summary>
        /// Latitude point number
        /// </summary>
        public int lat_point_num;
        /// <summary>
        /// Longitude point number
        /// </summary>
        public int lon_point_num;
        /// <summary>
        /// Latitude delta
        /// </summary>
        public Single lat_delta;
        /// <summary>
        /// Longitude delta
        /// </summary>
        public Single lon_delta;
        /// <summary>
        /// Longitude start
        /// </summary>
        public Single lon_LF;
        /// <summary>
        /// Latitude start
        /// </summary>
        public Single lat_LF;
        /// <summary>
        /// X array
        /// </summary>
        public double[] X;
        /// <summary>
        /// Y array
        /// </summary>
        public double[] Y;
        /// <summary>
        /// Time number
        /// </summary>
        public int time_num;
        /// <summary>
        /// Level number
        /// </summary>
        public int level_num;
        /// <summary>
        /// Height array
        /// </summary>
        public int[] heights;
        /// <summary>
        /// Pollutant number
        /// </summary>
        public int pollutant_num;
        /// <summary>
        /// Pollutant name array
        /// </summary>
        public string[] pollutants;
        /// <summary>
        /// hByte number
        /// </summary>
        public int hByte_num;
        /// <summary>
        /// Sample start time list
        /// </summary>
        public List<DateTime> sample_start;
        /// <summary>
        /// Sample stop time list
        /// </summary>
        public List<DateTime> sample_stop;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public HYSPLITConcDataInfo()
        {

        }

        #endregion

        #region Methods
        /// <summary>
        /// Read HYSPLIT concentration data info
        /// </summary>
        /// <param name="aFile">File path</param>       
        public override void ReadDataInfo(string aFile)
        {
            FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, j, hBytes;
            byte[] aBytes;            

            this.FileName = aFile;

            //Record #1
            br.ReadBytes(4);
            aBytes = br.ReadBytes(4);
            Ident = System.Text.ASCIIEncoding.ASCII.GetString(aBytes);
            year = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            month = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            day = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            hour = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            forecast_hour = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            loc_num = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            pack_flag = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());

            //Record #2
            locArray = new object[8, loc_num];
            for (i = 0; i < loc_num; i++)
            {
                br.ReadBytes(8);
                for (j = 0; j < 4; j++)
                {
                    locArray[j, i] = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                }
                for (j = 4; j < 7; j++)
                {
                    aBytes = br.ReadBytes(4);
                    Array.Reverse(aBytes);
                    locArray[j, i] = BitConverter.ToSingle(aBytes, 0);
                }
                locArray[7, i] = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
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
            lat_point_num = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            lon_point_num = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            aBytes = br.ReadBytes(4);
            Array.Reverse(aBytes);
            lat_delta = BitConverter.ToSingle(aBytes, 0);
            aBytes = br.ReadBytes(4);
            Array.Reverse(aBytes);
            lon_delta = BitConverter.ToSingle(aBytes, 0);
            aBytes = br.ReadBytes(4);
            Array.Reverse(aBytes);
            lat_LF = BitConverter.ToSingle(aBytes, 0);
            aBytes = br.ReadBytes(4);
            Array.Reverse(aBytes);
            lon_LF = BitConverter.ToSingle(aBytes, 0);

            X = new double[lon_point_num];
            Y = new double[lat_point_num];
            for (i = 0; i < lon_point_num; i++)
            {
                X[i] = lon_LF + i * lon_delta;
            }
            if (X[0] == 0 && X[X.Length - 1] +
                lon_delta == 360)
            {
                this.IsGlobal = true;
            }
            for (i = 0; i < lat_point_num; i++)
            {
                Y[i] = lat_LF + i * lat_delta;
            }

            Dimension xdim = new Dimension(DimensionType.X);
            xdim.SetValues(X);
            Dimension ydim = new Dimension(DimensionType.Y);
            ydim.SetValues(Y);

            //Record #4
            br.ReadBytes(8);
            level_num = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            heights = new int[level_num];
            double[] values = new double[level_num];
            for (i = 0; i < level_num; i++)
            {
                heights[i] = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                values[i] = heights[i];
            }
            Dimension zdim = new Dimension(DimensionType.Z);
            zdim.SetValues(values);

            //Record #5
            br.ReadBytes(8);
            pollutant_num = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
            pollutants = new string[pollutant_num];
            for (i = 0; i < pollutant_num; i++)
            {
                aBytes = br.ReadBytes(4);
                pollutants[i] = System.Text.ASCIIEncoding.ASCII.GetString(aBytes);
            }

            hBytes = 36 + lon_point_num * 40 + 32 + 12 + level_num * 4 + 12 +
                pollutant_num * 4;
            hByte_num = hBytes;

            //Record Data
            int k, tNum;
            tNum = 0;
            int[] sampleTimes = new int[6];
            string dStr;
            DateTime aDateTime;
            sample_start = new List<DateTime>();
            sample_stop = new List<DateTime>();
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
                sample_start.Add(aDateTime);

                //Record #7
                br.ReadBytes(8);
                for (i = 0; i < 6; i++)
                {
                    sampleTimes[i] = System.Net.IPAddress.NetworkToHostOrder(br.ReadInt32());
                }
                dStr = sampleTimes[0].ToString("00") + "-" + sampleTimes[1].ToString("00") +
                    "-" + sampleTimes[2].ToString("00") + " " + sampleTimes[3].ToString("00");
                aDateTime = DateTime.ParseExact(dStr, "yy-MM-dd HH", System.Globalization.CultureInfo.InvariantCulture);
                sample_stop.Add(aDateTime);

                //Record 8;
                int aLevel, aN, IP, JP;
                string aType;
                for (i = 0; i < pollutant_num; i++)
                {
                    for (j = 0; j < level_num; j++)
                    {
                        if (pack_flag == 1)
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
                            for (JP = 0; JP < lat_point_num; JP++)
                            {
                                for (IP = 0; IP < lon_point_num; IP++)
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
            time_num = tNum;
            double[] tvalues = new double[time_num];
            for (i = 0; i < time_num; i++)
            {
                tvalues[i] = DataConvert.ToDouble(sample_start[i]);
            }
            Dimension tdim = new Dimension(DimensionType.T);
            tdim.SetValues(tvalues);

            List<Variable> variables = new List<Variable>();
            for (i = 0; i < pollutant_num; i++)
            {
                Variable var = new Variable();
                var.Name = pollutants[i];
                var.SetDimension(tdim);
                var.SetDimension(zdim);
                var.SetDimension(ydim);
                var.SetDimension(xdim);
                variables.Add(var);
            }
            this.Variables = variables;

            br.Close();
            fs.Close();           
        }

        /// <summary>
        /// Read HYSPLIT concentration data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>        
        /// <param name="aFactor">Data scale factor</param>
        /// <returns></returns>
        public double[,] ReadConcData(int timeIdx, int varIdx, int levelIdx, int aFactor)
        {
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, j, nBytes;
            byte[] aBytes;
            double[,] dataArray = new double[lon_point_num, lat_point_num];
            double[,] newDataArray = new double[lat_point_num, lon_point_num];

            //Record #1            
            br.ReadBytes(36);

            //Record #2
            nBytes = (8 * 4 + 8) * loc_num;
            br.ReadBytes(nBytes);

            //Record #3
            string fName = Path.GetFileName(this.FileName).ToLower();
            if (fName.Contains("gemzint"))
            {
                br.ReadBytes(28);   //For vertical concentration file gemzint
            }
            else
            {
                br.ReadBytes(32);
            }

            //Record #4
            nBytes = 12 + level_num * 4;
            br.ReadBytes(nBytes);

            //Record #5
            nBytes = 12 + pollutant_num * 4;
            br.ReadBytes(nBytes);

            //Record Data
            int t, k;
            int aLevel, aN, IP, JP;
            string aType;
            double aConc;
            for (t = 0; t < time_num; t++)
            {
                br.ReadBytes(64);

                for (i = 0; i < pollutant_num; i++)
                {
                    for (j = 0; j < level_num; j++)
                    {
                        if (t == timeIdx && i == varIdx && j == levelIdx)
                        {
                            if (br.BaseStream.Position + 28 > br.BaseStream.Length)
                            {
                                break;
                            }
                            if (pack_flag == 1)
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
                                    if (IP >= 0 && IP < lon_point_num && JP >= 0 && JP < lat_point_num)
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
                                for (JP = 0; JP < lat_point_num; JP++)
                                {
                                    for (IP = 0; IP < lon_point_num; IP++)
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
                            if (pack_flag == 1)
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
                                for (JP = 0; JP < lat_point_num; JP++)
                                {
                                    for (IP = 0; IP < lon_point_num; IP++)
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

            if (this.IsGlobal)
            {
                newDataArray = new double[lat_point_num, lon_point_num + 1];
            }
            for (i = 0; i < lon_point_num; i++)
            {
                for (j = 0; j < lat_point_num; j++)
                {
                    newDataArray[j, i] = dataArray[i, j];
                }
            }
            if (this.IsGlobal)
            {
                for (i = 0; i < lat_point_num; i++)
                {
                    newDataArray[i, lon_point_num] = newDataArray[i, 0];
                }
            }

            return newDataArray;
        }

        /// <summary>
        /// Read HYSPLIT concentration data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>        
        /// <param name="aFactor">Data scale factor</param>
        /// <returns>grid data</returns>
        public GridData GetGridData(int timeIdx, int varIdx, int levelIdx, int aFactor)
        {
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, j, nBytes;
            byte[] aBytes;
            double[,] dataArray = new double[lon_point_num, lat_point_num];
            double[,] newDataArray = new double[lat_point_num, lon_point_num];

            //Record #1            
            br.ReadBytes(36);

            //Record #2
            nBytes = (8 * 4 + 8) * loc_num;
            br.ReadBytes(nBytes);

            //Record #3
            string fName = Path.GetFileName(this.FileName).ToLower();
            if (fName.Contains("gemzint"))
            {
                br.ReadBytes(28);   //For vertical concentration file gemzint
            }
            else
            {
                br.ReadBytes(32);
            }

            //Record #4
            nBytes = 12 + level_num * 4;
            br.ReadBytes(nBytes);

            //Record #5
            nBytes = 12 + pollutant_num * 4;
            br.ReadBytes(nBytes);

            //Record Data
            int t, k;
            int aLevel, aN, IP, JP;
            string aType;
            double aConc;
            for (t = 0; t < time_num; t++)
            {
                br.ReadBytes(64);

                for (i = 0; i < pollutant_num; i++)
                {
                    for (j = 0; j < level_num; j++)
                    {
                        if (t == timeIdx && i == varIdx && j == levelIdx)
                        {
                            if (br.BaseStream.Position + 28 > br.BaseStream.Length)
                            {
                                break;
                            }
                            if (pack_flag == 1)
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
                                    if (IP >= 0 && IP < lon_point_num && JP >= 0 && JP < lat_point_num)
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
                                for (JP = 0; JP < lat_point_num; JP++)
                                {
                                    for (IP = 0; IP < lon_point_num; IP++)
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
                            if (pack_flag == 1)
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
                                for (JP = 0; JP < lat_point_num; JP++)
                                {
                                    for (IP = 0; IP < lon_point_num; IP++)
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

            double[] newX = X;
            for (i = 0; i < lon_point_num; i++)
            {
                for (j = 0; j < lat_point_num; j++)
                {
                    newDataArray[j, i] = dataArray[i, j];
                }
            }

            GridData gridData = new GridData();
            gridData.Data = newDataArray;
            gridData.X = newX;            
            gridData.Y = Y;            
            gridData.MissingValue = MissingValue;

            return gridData;
        }

        /// <summary>
        /// Read grid data - LonLat
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_LonLat(int timeIdx, int varIdx, int levelIdx)
        {
            return GetGridData(timeIdx, varIdx, levelIdx, 0);
        }

        /// <summary>
        /// Get HYSPLIT concentration data info text
        /// </summary>        
        /// <returns>data info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + this.FileName;
            dataInfo += Environment.NewLine + "Pack Flag = " + pack_flag.ToString();
            dataInfo += Environment.NewLine + "Xsize = " + lon_point_num.ToString() +
                    "  Ysize = " + lat_point_num.ToString() + "  Zsize = " + level_num.ToString() +
                    "  Tsize = " + time_num.ToString();
            dataInfo += Environment.NewLine + "Number of Variables = " + pollutant_num.ToString();
            foreach (string v in pollutants)
            {
                dataInfo += Environment.NewLine + v;
            }

            return dataInfo;
        }

        /// <summary>
        /// Read grid data - TimeLat
        /// </summary>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_TimeLat(int lonIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - TimeLon
        /// </summary>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_TimeLon(int latIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - LevelLat
        /// </summary>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="timeIdx">Time index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_LevelLat(int lonIdx, int varIdx, int timeIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - LevelLon
        /// </summary>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="timeIdx">Time index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_LevelLon(int latIdx, int varIdx, int timeIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - LevelTime
        /// </summary>
        /// <param name="latIdx">Laititude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="lonIdx">Longitude index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_LevelTime(int latIdx, int varIdx, int lonIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - Time
        /// </summary>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_Time(int lonIdx, int latIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - Level
        /// </summary>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="timeIdx">Time index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_Level(int lonIdx, int latIdx, int varIdx, int timeIdx)
        {
            return null;
        }

        /// <summary>
        /// Get grid data - Lon
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level Index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_Lon(int timeIdx, int latIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Get grid data - Lat
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_Lat(int timeIdx, int lonIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        #endregion
    }
}
