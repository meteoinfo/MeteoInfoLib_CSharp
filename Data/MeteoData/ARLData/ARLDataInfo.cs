using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Projections;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// ARL meteo data info
    /// </summary>
    public class ARLDataInfo : DataInfo,IGridDataInfo
    {
        #region Variables

        private FileStream _fs = null;
        private BinaryWriter _bw = null;

        /// <summary>
        /// Is Lat/Lon
        /// </summary>
        public Boolean isLatLon;        
        /// <summary>
        /// Data head
        /// </summary>
        public DataHead dataHead;
        /// <summary>
        /// Record length
        /// </summary>
        public int recLen;
        /// <summary>
        /// Index length
        /// </summary>
        public int indexLen;
        /// <summary>
        /// Record number per time
        /// </summary>
        public int recsPerTime;
        /// <summary>
        /// Variable list
        /// </summary>
        public List<List<string>> LevelVarList;
        /// <summary>
        /// Level number
        /// </summary>
        public int levelNum;
        /// <summary>
        /// Level list
        /// </summary>
        public List<double> levels;
        /// <summary>
        /// X array
        /// </summary>
        public double[] X;
        /// <summary>
        /// Y array
        /// </summary>
        public double[] Y;
        private List<DateTime> times;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ARLDataInfo()
        {
            isLatLon = false;
            LevelVarList = new List<List<string>>();
            levelNum = 0;
            levels = new List<double>();
        }

        #endregion

        #region Properties
 

        #endregion

        #region Methods
        #region Read

        /// <summary>
        /// Read ARL data info
        /// </summary>
        /// <param name="aFile"></param>                
        /// <returns></returns>
        public override void ReadDataInfo(string aFile)
        {
            this.FileName = aFile;
            FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            DataLabel aDL = new DataLabel();
            DataHead aDH = new DataHead();
            int i, j, vNum;
            string vName;
            List<string> vList = new List<string>();

            //open file to decode the standard label (50) plus the 
            //fixed portion (108) of the extended header   
            aDL = ReadDataLabel(br);

            aDH.MODEL = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
            aDH.ICX = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(3)));
            aDH.MN = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDH.POLE_LAT = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
            aDH.POLE_LON = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
            aDH.REF_LAT = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
            aDH.REF_LON = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
            aDH.SIZE = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
            aDH.ORIENT = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
            aDH.TANG_LAT = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
            aDH.SYNC_XP = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
            aDH.SYNC_YP = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
            aDH.SYNC_LAT = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
            aDH.SYNC_LON = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
            aDH.DUMMY = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
            aDH.NX = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(3)));
            aDH.NY = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(3)));
            aDH.NZ = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(3)));
            aDH.K_FLAG = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDH.LENH = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));

            int NXY = aDH.NX * aDH.NY;
            int LEN = NXY + 50;
            recLen = LEN;
            int indexRecNum = 1;
            indexLen = recLen;

            if (aDH.LENH > NXY)
            {
                byte[] bytes;
                bytes = br.ReadBytes(NXY - 108);
                List<byte> byteList = new List<byte>();
                byteList.AddRange(bytes);
                for (i = 0; i < 100; i++)
                {
                    aDL = ReadDataLabel(br);
                    if (aDL.Variable != "INDX")
                        break;

                    byteList.AddRange(br.ReadBytes(NXY));
                }
                bytes = byteList.ToArray();
                indexRecNum = i + 1;
                indexLen += i * recLen;

                byte[] nbytes;
                int idx = 0;
                int n;
                for (i = 0; i < aDH.NZ; i++)
                {
                    n = 6;
                    nbytes = new byte[n];
                    Array.Copy(bytes, idx, nbytes, 0, n);
                    idx += n;
                    string lstr = System.Text.ASCIIEncoding.ASCII.GetString(nbytes);
                    if (MIMath.IsNumeric(lstr))
                        levels.Add(float.Parse(lstr));
                    else
                        levels.Add(0.0f);
                    //levels.Add(float.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(6))));
                    n = 2;
                    nbytes = new byte[n];
                    Array.Copy(bytes, idx, nbytes, 0, n);
                    idx += n;
                    vNum = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(nbytes));
                    for (j = 0; j < vNum; j++)
                    {
                        n = 4;
                        nbytes = new byte[n];
                        Array.Copy(bytes, idx, nbytes, 0, n);
                        idx += n;
                        vName = System.Text.ASCIIEncoding.ASCII.GetString(nbytes);
                        vList.Add(vName);
                        idx += 4;
                    }
                    LevelVarList.Add(new List<string>(vList));
                    vList.Clear();
                }
            }
            else
            {
                for (i = 0; i < aDH.NZ; i++)
                {
                    string lstr = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(6));
                    if (MIMath.IsNumeric(lstr))
                        levels.Add(float.Parse(lstr));
                    else
                        levels.Add(0.0f);
                    //levels.Add(float.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(6))));
                    vNum = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                    for (j = 0; j < vNum; j++)
                    {
                        vName = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
                        vList.Add(vName);
                        br.ReadBytes(4);
                    }
                    LevelVarList.Add(new List<string>(vList));
                    vList.Clear();
                }
            }
            levelNum = aDH.NZ;

            //Close file reader
            br.Close();
            fs.Close();

            //if (aDL.Variable != "INDX")
            //{
            //    string ErrorStr = "WARNING Old format meteo data grid!" + Environment.NewLine + aDL.Variable;
            //}

            //Decide projection
            dataHead = aDH;
            if (aDH.SIZE == 0)
            {
                isLatLon = true;                
                X = new double[aDH.NX];
                Y = new double[aDH.NY];
                for (i = 0; i < aDH.NX; i++)
                {
                    X[i] = aDH.SYNC_LON + i * aDH.REF_LON;
                }
                if (X[aDH.NX - 1] + aDH.REF_LON - X[0] == 360)
                {
                    this.IsGlobal = true;
                }
                for (i = 0; i < aDH.NY; i++)
                {
                    Y[i] = aDH.SYNC_LAT + i * aDH.REF_LAT;
                }
            }
            else
            {
                //Identify projection
                isLatLon = false;
                string ProjStr;
                ProjectionInfo theProj = new ProjectionInfo();
                if (aDH.POLE_LAT == 90 || aDH.POLE_LAT == -90)
                {
                    if (aDH.TANG_LAT == 90 || aDH.TANG_LAT == -90)
                    {                        
                        ProjStr = "+proj=stere" +
                                "+lat_0=" + aDH.TANG_LAT.ToString() +
                                "+lon_0=" + (aDH.REF_LON + aDH.ORIENT).ToString();
                    }
                    else if (aDH.TANG_LAT == 0)
                    {                        
                        ProjStr = "+proj=merc" +
                            "+lon_0=" + (aDH.REF_LON + aDH.ORIENT).ToString();
                    }
                    else
                    {                        
                        ProjStr = "+proj=lcc" +
                                "+lat_0=" + aDH.TANG_LAT.ToString() +
                                "+lat_1=" + aDH.REF_LAT.ToString() +
                                "+lon_0=" + (aDH.REF_LON + aDH.ORIENT).ToString();
                    }
                }
                else
                {
                    if (aDH.TANG_LAT == 0)
                    {                        
                        ProjStr = "+proj=tmerc" +
                            "+lat_0=" + aDH.POLE_LAT.ToString() +
                            "+lon_0=" + (aDH.REF_LON + aDH.ORIENT).ToString();
                    }
                    else
                    {                        
                        ProjStr = "+proj=stere" +
                                    "+lat_0=" + aDH.POLE_LAT.ToString() +
                                    "+lon_0=" + (aDH.REF_LON + aDH.ORIENT).ToString();
                    }
                }

                theProj = new ProjectionInfo(ProjStr);
                this.ProjectionInfo = theProj;

                //Set X Y
                X = new double[aDH.NX];
                Y = new double[aDH.NY];
                GetProjectedXY(theProj, aDH.SIZE * 1000, aDH.SYNC_XP, aDH.SYNC_YP, aDH.SYNC_LON,
                    aDH.SYNC_LAT, ref X, ref Y);
            }

            //Reopen            
            byte[] dataBytes = new byte[NXY];
            DateTime aTime, oldTime;
            int recNum, timeNum;
            fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            br = new BinaryReader(fs);
            recNum = 0;
            timeNum = 0;
            string dStr;
            dStr = aDL.Year.ToString("00") + "-" + aDL.Month.ToString("00") + "-" +
                aDL.Day.ToString("00") + " " + aDL.Hour.ToString("00");
            oldTime = DateTime.ParseExact(dStr, "yy-MM-dd HH", System.Globalization.CultureInfo.InvariantCulture);
            times = new List<DateTime>();
            times.Add(oldTime);

            do
            {
                if (br.BaseStream.Position >= br.BaseStream.Length)
                {
                    break;
                }

                //Read label
                aDL = ReadDataLabel(br);

                //Read Data
                dataBytes = br.ReadBytes(NXY);

                if (aDL.Variable != "INDX")
                {
                    dStr = aDL.Year.ToString("00") + "-" + aDL.Month.ToString("00") + "-" +
                        aDL.Day.ToString("00") + " " + aDL.Hour.ToString("00");
                    aTime = DateTime.ParseExact(dStr, "yy-MM-dd HH", System.Globalization.CultureInfo.InvariantCulture);
                    if (aTime != oldTime)
                    {
                        times.Add(aTime);
                        oldTime = aTime;
                        timeNum += 1;
                    }
                    if (timeNum == 0)
                    {
                        recNum += 1;
                    }
                }

            } while (true);

            br.Close();
            fs.Close();

            //this.Times = times;
            List<double> tvalues = new List<double>();
            foreach (DateTime t in times)
                tvalues.Add(DataConvert.ToDouble(t));

            Dimension tdim = new Dimension(DimensionType.T);
            tdim.SetValues(tvalues);
            this.TimeDimension = tdim;
            Dimension ydim = new Dimension(DimensionType.Y);
            ydim.SetValues(Y);
            this.YDimension = ydim;
            Dimension xdim = new Dimension(DimensionType.X);
            xdim.SetValues(X);
            this.XDimension = xdim;

            recsPerTime = recNum + indexRecNum;
            Variable aVar = new Variable();
            vList.Clear();
            int varIdx;
            List<Variable> vars = new List<Variable>();
            for (i = 0; i < LevelVarList.Count; i++)
            {
                for (j = 0; j < LevelVarList[i].Count; j++)
                {
                    vName = LevelVarList[i][j];
                    if (!vList.Contains(vName))
                    {
                        vList.Add(vName);
                        aVar = new Variable();
                        aVar.Name = vName;
                        aVar.Levels.Add(levels[i]);
                        aVar.LevelIdxs.Add(i);
                        aVar.VarInLevelIdxs.Add(j);                        
                        vars.Add(aVar);
                    }
                    else
                    {
                        varIdx = vList.IndexOf(vName);
                        aVar = vars[varIdx];
                        aVar.Levels.Add(levels[i]);
                        aVar.LevelIdxs.Add(i);
                        aVar.VarInLevelIdxs.Add(j);
                        //aVar.LevelIdxs.Add(i);
                        //aVar.VarInLevelIdxs.Add(j);
                        vars[varIdx] = aVar;
                    }
                }
            }

            foreach (Variable var in vars)
            {
                Dimension zdim = new Dimension(DimensionType.Z);
                zdim.SetValues(var.Levels);
                var.SetDimension(tdim);
                var.SetDimension(zdim);
                var.SetDimension(ydim);
                var.SetDimension(xdim);
            }
            this.Variables = vars;
        }

        private DataLabel ReadDataLabel(BinaryReader br)
        {
            DataLabel aDL = new DataLabel();
            aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
            aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
            aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
            aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

            return aDL;
        }

        private void GetProjectedXY(ProjectionInfo projInfo, Single size,
            Single sync_XP, Single sync_YP, Single sync_Lon, Single sync_Lat,
            ref double[] X, ref double[] Y)
        {
            //Get sync X/Y
            ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
            double sync_X, sync_Y;
            double[][] points = new double[1][];
            points[0] = new double[] { sync_Lon, sync_Lat };
            Reproject.ReprojectPoints(points, fromProj, projInfo, 0, 1);
            sync_X = points[0][0];
            sync_Y = points[0][1];

            //Get integer sync X/Y            
            int i_XP, i_YP;
            double i_X, i_Y;
            i_XP = (int)sync_XP;
            if (sync_XP == i_XP)
            {
                i_X = sync_X;
            }
            else
            {
                i_X = sync_X - (sync_XP - i_XP) * size;
            }
            i_YP = (int)sync_YP;
            if (sync_YP == i_YP)
            {
                i_Y = sync_Y;
            }
            else
            {
                i_Y = sync_Y - (sync_YP - i_YP) * size;
            }

            //Get left bottom X/Y
            int nx, ny;
            nx = X.Length;
            ny = Y.Length;
            double xlb, ylb;
            xlb = i_X - (i_XP - 1) * size;
            ylb = i_Y - (i_YP - 1) * size;

            //Get X Y with orient 0
            int i;
            for (i = 0; i < nx; i++)
            {
                X[i] = xlb + i * size;
            }
            for (i = 0; i < ny; i++)
            {
                Y[i] = ylb + i * size;
            }
        }

        private double Eqvlat(double lat1, double lat2)
        {
            double RADPDEG = Math.PI / 180.0;
            double DEGPRAD = 180.0 / Math.PI;
            double slat1, slat2, al1, al2;
            slat1 = Math.Sin(RADPDEG * lat1);
            slat2 = Math.Sin(RADPDEG * lat2);
            /* reorder, slat1 larger */
            if (slat1 < slat2)
            {
                double temp = slat1;
                slat1 = slat2;
                slat2 = temp;
            }
            /*  Take care of special cases first */
            if (slat1 == slat2) return Math.Asin(slat1) * DEGPRAD;
            if (slat1 == -slat2) return 0.0;
            if (slat1 >= 1.0) return 90.0;
            if (slat2 <= -1.0) return -90.0;
            /********************************************************/
            double FSM = 1.0e-3;
            /* Compute al1 = log((1. - slat1)/(1. - slat2))/(slat1 - slat2) */
            {
                double tau = (slat1 - slat2) / (2.0 - slat1 - slat2);
                if (tau > FSM)
                {
                    al1 = Math.Log((1.0 - slat1) / (1.0 - slat2)) / (slat1 - slat2);
                }
                else
                {
                    tau *= tau;
                    al1 = -2.0 / (2.0 - slat1 - slat2) *
                               (1.0 + tau *
                               (1.0 / 3.0 + tau *
                               (1.0 / 5.0 + tau *
                               (1.0 / 7.0 + tau))));
                }
            }
            /* Compute al2 = log((1. + slat1)/(1. + slat2))/(slat1 - slat2) */
            {
                double tau = (slat1 - slat2) / (2.0 + slat1 + slat2);
                if (tau > FSM)
                {
                    al2 = Math.Log((1.0 + slat1) / (1.0 + slat2)) / (slat1 - slat2);
                }
                else
                {
                    tau *= tau;
                    al2 = 2.0 / (2.0 + slat1 + slat2) *
                               (1.0 + tau *
                               (1.0 / 3.0 + tau *
                               (1.0 / 5.0 + tau *
                               (1.0 / 7.0 + tau))));
                }
            }
            return Math.Asin((al1 + al2) / (al1 - al2)) * DEGPRAD;
        }

        /// <summary>
        /// Get ARL grid data - Lon/Lat
        /// </summary>
        /// <param name="timeIdx">time index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>       
        /// <returns>grid data</returns>
        public GridData GetGridData_LonLat(int timeIdx, int varIdx, int levelIdx)
        {
            GridData gridData = new GridData();
            gridData.Data = GetARLGridData_LonLat(timeIdx, varIdx, levelIdx);
            gridData.MissingValue = MissingValue;           
            gridData.X = X;
            //if (isGlobal)
            //{
            //    Array.Resize(ref gridData.X, X.Length + 1);
            //    gridData.X[X.Length] = X[X.Length - 1] + (X[1] - X[0]);
            //}
            gridData.Y = Y;

            //if (isGlobal)
            //    gridData.ExtendToGlobal();

            return gridData;
        }

        /// <summary>
        /// Get ARL data - Lon/Lat
        /// </summary>
        /// <param name="timeIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>       
        /// <returns></returns>
        public double[,] GetARLGridData_LonLat(int timeIdx, int varIdx, int levelIdx)
        {
            int xNum, yNum;
            xNum = dataHead.NX;
            yNum = dataHead.NY;
            double[,] gridData = new double[yNum, xNum];
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] dataBytes;
            DataLabel aDL = new DataLabel();

            //Update level and variable index
            Variable aVar = Variables[varIdx];
            if (aVar.LevelNum > 1)
                levelIdx += 1;

            varIdx = LevelVarList[levelIdx].IndexOf(aVar.Name);

            br.BaseStream.Position = timeIdx * recsPerTime * recLen;
            br.BaseStream.Position += indexLen;
            for (int i = 0; i < levelIdx; i++)
            {
                br.BaseStream.Position += LevelVarList[i].Count * recLen;
            }
            br.BaseStream.Position += varIdx * recLen;

            //Read label
            aDL = ReadDataLabel(br);
            
            //Read Data
            dataBytes = br.ReadBytes(recLen - 50);

            br.Close();
            fs.Close();

            gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

            return gridData;
        }

        /// <summary>
        /// Get ARL data - Time/Lon
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_TimeLon(int latIdx, int varIdx, int levelIdx)
        {
            GridData gridData = new GridData();
            gridData.Data = GetARLGridData_TimeLon(latIdx, varIdx, levelIdx);
            gridData.MissingValue = MissingValue;
            gridData.X = X;
            gridData.Y = new double[this.TimeNum];
            for (int i = 0; i < this.TimeNum; i++)
                gridData.Y[i] = DataConvert.ToDouble(this.Times[i]);

            return gridData;
        }

        /// <summary>
        /// Get ARL data - Time/Lon
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>        
        /// <returns></returns>
        public double[,] GetARLGridData_TimeLon(int latIdx, int varIdx, int levelIdx)
        {
            int xNum, yNum, tNum, t;
            xNum = dataHead.NX;
            yNum = dataHead.NY;
            tNum = this.Times.Count;
            double[,] gridData = new double[yNum, xNum];
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] dataBytes;
            DataLabel aDL = new DataLabel();
            double[,] newGridData = new double[tNum, xNum];

            //Update level and variable index
            Variable aVar = Variables[varIdx];
            if (aVar.LevelNum > 1)
                levelIdx += 1;

            varIdx = LevelVarList[levelIdx].IndexOf(aVar.Name);

            for (t = 0; t < tNum; t++)
            {
                br.BaseStream.Position = t * recsPerTime * recLen;
                br.BaseStream.Position += indexLen;
                for (int i = 0; i < levelIdx; i++)
                {
                    br.BaseStream.Position += LevelVarList[i].Count * recLen;
                }
                br.BaseStream.Position += varIdx * recLen;

                //Read label
                aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
                aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
                aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
                aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

                //Read Data
                dataBytes = br.ReadBytes(recLen - 50);
                gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);
                for (int j = 0; j < xNum; j++)
                {
                    newGridData[t, j] = gridData[latIdx, j];
                }
            }

            br.Close();
            fs.Close();

            return newGridData;
        }

        /// <summary>
        /// Get ARL data - Time/Lat
        /// </summary>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_TimeLat(int lonIdx, int varIdx, int levelIdx)
        {
            GridData gridData = new GridData();
            gridData.Data = GetARLGridData_TimeLat(lonIdx, varIdx, levelIdx);
            gridData.MissingValue = MissingValue;
            gridData.X = Y;
            gridData.Y = new double[this.TimeNum];
            for (int i = 0; i < this.TimeNum; i++)
                gridData.Y[i] = DataConvert.ToDouble(this.Times[i]);

            return gridData;
        }

        /// <summary>
        /// Get ARL data - Time/Lat
        /// </summary>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>       
        /// <returns></returns>
        public double[,] GetARLGridData_TimeLat(int lonIdx, int varIdx, int levelIdx)
        {
            int xNum, yNum, tNum, t;
            xNum = dataHead.NX;
            yNum = dataHead.NY;
            tNum = this.TimeNum;
            double[,] gridData = new double[yNum, xNum];
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] dataBytes;
            DataLabel aDL = new DataLabel();
            double[,] newGridData = new double[tNum, yNum];

            //Update level and variable index
            Variable aVar = Variables[varIdx];
            if (aVar.LevelNum > 1)
                levelIdx += 1;

            varIdx = LevelVarList[levelIdx].IndexOf(aVar.Name);

            for (t = 0; t < tNum; t++)
            {
                br.BaseStream.Position = t * recsPerTime * recLen;
                br.BaseStream.Position += indexLen;
                for (int i = 0; i < levelIdx; i++)
                {
                    br.BaseStream.Position += LevelVarList[i].Count * recLen;
                }
                br.BaseStream.Position += varIdx * recLen;

                //Read label
                aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
                aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
                aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
                aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

                //Read Data
                dataBytes = br.ReadBytes(recLen - 50);
                gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);
                for (int i = 0; i < yNum; i++)
                {
                    newGridData[t, i] = gridData[i, lonIdx];
                }
            }

            br.Close();
            fs.Close();

            return newGridData;
        }

        /// <summary>
        /// Get ARL data - Level/Lon
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="tIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_LevelLon(int latIdx, int varIdx, int tIdx)
        {
            GridData gridData = new GridData();
            gridData.Data = GetARLGridData_LevelLon(latIdx, varIdx, tIdx);
            gridData.MissingValue = MissingValue;
            gridData.X = X;
            gridData.Y = Variables[varIdx].Levels.ToArray();

            return gridData;
        }

        /// <summary>
        /// Get ARL data - Level/Lon
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="cvarIdx"></param>
        /// <param name="tIdx"></param>        
        /// <returns></returns>
        public double[,] GetARLGridData_LevelLon(int latIdx, int cvarIdx, int tIdx)
        {
            int xNum, yNum, lNum, varIdx, levIdx;
            xNum = dataHead.NX;
            yNum = dataHead.NY;
            lNum = Variables[cvarIdx].LevelNum;
            double[,] gridData = new double[yNum, xNum];
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] dataBytes;
            DataLabel aDL = new DataLabel();
            double[,] newGridData = new double[lNum, xNum];
            long aLevPosition;

            br.BaseStream.Position = tIdx * recsPerTime * recLen;
            br.BaseStream.Position += indexLen;
            aLevPosition = br.BaseStream.Position;
            levIdx = Variables[cvarIdx].LevelIdxs[0];
            for (int i = 0; i < lNum; i++)
            {
                varIdx = Variables[cvarIdx].VarInLevelIdxs[i];
                levIdx = Variables[cvarIdx].LevelIdxs[i];
                br.BaseStream.Position = aLevPosition;
                for (int j = 0; j < levIdx; j++)
                {
                    br.BaseStream.Position += LevelVarList[j].Count * recLen;
                }
                br.BaseStream.Position += varIdx * recLen;

                //Read label
                aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
                aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
                aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
                aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

                //Read Data
                dataBytes = br.ReadBytes(recLen - 50);
                gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);
                for (int j = 0; j < xNum; j++)
                {
                    newGridData[i, j] = gridData[latIdx, j];
                }
            }

            br.Close();
            fs.Close();

            return newGridData;
        }

        /// <summary>
        /// Get ARL data - Level/Lat
        /// </summary>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="tIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_LevelLat(int lonIdx, int varIdx, int tIdx)
        {
            GridData gridData = new GridData();
            gridData.Data = GetARLGridData_LevelLat(lonIdx, varIdx, tIdx);
            gridData.MissingValue = MissingValue;
            gridData.X = Y;
            gridData.Y = Variables[varIdx].Levels.ToArray();
            
            return gridData;
        }

        /// <summary>
        /// Get ARL data - Level/Lat
        /// </summary>
        /// <param name="lonIdx"></param>
        /// <param name="cvarIdx"></param>
        /// <param name="tIdx"></param>       
        /// <returns></returns>
        public double[,] GetARLGridData_LevelLat(int lonIdx, int cvarIdx, int tIdx)
        {
            int xNum, yNum, lNum, varIdx, levIdx;
            xNum = dataHead.NX;
            yNum = dataHead.NY;
            lNum = Variables[cvarIdx].LevelNum;
            double[,] gridData = new double[yNum, xNum];
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] dataBytes;
            DataLabel aDL = new DataLabel();
            double[,] newGridData = new double[lNum, yNum];
            long aLevPosition;

            br.BaseStream.Position = tIdx * recsPerTime * recLen;
            br.BaseStream.Position += indexLen;
            aLevPosition = br.BaseStream.Position;
            levIdx = Variables[cvarIdx].LevelIdxs[0];
            for (int i = 0; i < lNum; i++)
            {
                varIdx = Variables[cvarIdx].VarInLevelIdxs[i];
                levIdx = Variables[cvarIdx].LevelIdxs[i];
                br.BaseStream.Position = aLevPosition;
                for (int j = 0; j < levIdx; j++)
                {
                    br.BaseStream.Position += LevelVarList[j].Count * recLen;
                }
                br.BaseStream.Position += varIdx * recLen;

                //Read label
                aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
                aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
                aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
                aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

                //Read Data
                dataBytes = br.ReadBytes(recLen - 50);
                gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);
                for (int j = 0; j < yNum; j++)
                {
                    newGridData[i, j] = gridData[j, lonIdx];
                }
            }

            br.Close();
            fs.Close();

            return newGridData;
        }

        /// <summary>
        /// Get ARL data - Level/Time
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="lonIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_LevelTime(int latIdx, int varIdx, int lonIdx)
        {
            GridData gridData = new GridData();
            gridData.Data = GetARLGridData_LevelTime(latIdx, varIdx, lonIdx);
            gridData.MissingValue = MissingValue;
            gridData.X = new double[this.TimeNum];
            for (int i = 0; i < this.TimeNum; i++)
                gridData.X[i] = DataConvert.ToDouble(Times[i]);
            gridData.Y = Variables[varIdx].Levels.ToArray();

            return gridData;
        }

        /// <summary>
        /// Get ARL data - LevelTime
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="cvarIdx"></param>
        /// <param name="lonIdx"></param>        
        /// <returns></returns>
        public double[,] GetARLGridData_LevelTime(int latIdx, int cvarIdx, int lonIdx)
        {
            int xNum, yNum, lNum, varIdx, levIdx, t, tNum;
            xNum = dataHead.NX;
            yNum = dataHead.NY;
            lNum = Variables[cvarIdx].LevelNum;
            tNum = this.TimeNum;
            double[,] gridData = new double[yNum, xNum];
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] dataBytes;
            DataLabel aDL = new DataLabel();
            double[,] newGridData = new double[lNum, tNum];
            long aLevPosition;

            for (t = 0; t < tNum; t++)
            {
                br.BaseStream.Position = t * recsPerTime * recLen;
                br.BaseStream.Position += indexLen;
                aLevPosition = br.BaseStream.Position;
                levIdx = Variables[cvarIdx].LevelIdxs[0];
                for (int i = 0; i < lNum; i++)
                {
                    varIdx = Variables[cvarIdx].VarInLevelIdxs[i];
                    levIdx = Variables[cvarIdx].LevelIdxs[i];
                    br.BaseStream.Position = aLevPosition;
                    for (int j = 0; j < levIdx; j++)
                    {
                        br.BaseStream.Position += LevelVarList[j].Count * recLen;
                    }
                    br.BaseStream.Position += varIdx * recLen;

                    //Read label
                    aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                    aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                    aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                    aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                    aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                    aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                    aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                    aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
                    aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
                    aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
                    aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

                    //Read Data
                    dataBytes = br.ReadBytes(recLen - 50);
                    gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

                    newGridData[i, t] = gridData[latIdx, lonIdx];

                }
            }

            br.Close();
            fs.Close();

            return newGridData;
        }

        private double[,] UnpackARLGridData(byte[] dataBytes, int xNum, int yNum, DataLabel aDL)
        {
            double[,] gridData = new double[yNum, xNum];
            double SCALE = Math.Pow(2.0, (7 - aDL.Exponent));
            double VOLD = aDL.Value;
            int INDX = 0;
            int i, j;
            for (j = 0; j < yNum; j++)
            {
                for (i = 0; i < xNum; i++)
                {
                    gridData[j, i] = ((int)(dataBytes[INDX]) - 127) / SCALE + VOLD;
                    INDX += 1;
                    VOLD = gridData[j, i];
                }
                VOLD = gridData[j, 0];
            }


            return gridData;
        }

        /// <summary>
        /// Get ARL data - Time
        /// </summary>
        /// <param name="lonIdx"></param>
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>       
        /// <returns></returns>
        public GridData GetGridData_Time(int lonIdx, int latIdx, int varIdx, int levelIdx)
        {            
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] dataBytes;
            DataLabel aDL = new DataLabel();
            int xNum, yNum, t;
            xNum = dataHead.NX;
            yNum = dataHead.NY;
            double[,] gridData = new double[yNum, xNum];
            double aValue;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = new double[this.TimeNum];
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, this.TimeNum];

            //Update level and variable index
            Variable aVar = Variables[varIdx];
            if (aVar.LevelNum > 1)
                levelIdx += 1;

            varIdx = LevelVarList[levelIdx].IndexOf(aVar.Name);

            for (t = 0; t < this.TimeNum; t++)
            {
                br.BaseStream.Position = t * recsPerTime * recLen;
                br.BaseStream.Position += indexLen;
                for (int i = 0; i < levelIdx; i++)
                {
                    br.BaseStream.Position += LevelVarList[i].Count * recLen;
                }
                br.BaseStream.Position += varIdx * recLen;

                //Read label
                aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
                aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
                aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
                aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

                //Read Data
                dataBytes = br.ReadBytes(recLen - 50);
                gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

                aValue = gridData[latIdx, lonIdx];
                aGridData.X[t] = DataConvert.ToDouble(Times[t]);
                aGridData.Data[0, t] = aValue;                
            }

            br.Close();
            fs.Close();

            return aGridData;
        }

        ///// <summary>
        ///// Get ARL data - Time
        ///// </summary>
        ///// <param name="lonIdx"></param>
        ///// <param name="latIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="levelIdx"></param>       
        ///// <returns></returns>
        //public List<PointD> GetARLGridData_Time(int lonIdx, int latIdx, int varIdx, int levelIdx)
        //{
        //    PointD aPoint = new PointD();
        //    List<PointD> pointList = new List<PointD>();

        //    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    int xNum, yNum, t;
        //    xNum = dataHead.NX;
        //    yNum = dataHead.NY;
        //    double[,] gridData = new double[yNum, xNum];
        //    double aValue;

        //    for (t = 0; t < times.Count; t++)
        //    {
        //        br.BaseStream.Position = t * recsPerTime * recLen;
        //        br.BaseStream.Position += recLen;
        //        for (int i = 0; i < levelIdx; i++)
        //        {
        //            br.BaseStream.Position += LevelVarList[i].Count * recLen;
        //        }
        //        br.BaseStream.Position += varIdx * recLen;

        //        //Read label
        //        aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //        aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //        aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //        aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //        //Read Data
        //        dataBytes = br.ReadBytes(recLen - 50);
        //        gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

        //        aValue = gridData[latIdx, lonIdx];
        //        if (!(Math.Abs(aValue / MissingValue - 1) < 0.01))
        //        {
        //            aPoint.X = times[t].ToBinary();
        //            aPoint.Y = aValue;
        //            pointList.Add(aPoint);
        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return pointList;
        //}

        /// <summary>
        /// Get ARL data - Level
        /// </summary>
        /// <param name="lonIdx"></param>
        /// <param name="latIdx"></param>
        /// <param name="cvarIdx"></param>
        /// <param name="timeIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_Level(int lonIdx, int latIdx, int cvarIdx, int timeIdx)
        {            
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] dataBytes;
            DataLabel aDL = new DataLabel();
            int xNum, yNum, varIdx, levIdx, lNum;
            xNum = dataHead.NX;
            yNum = dataHead.NY;
            lNum = Variables[cvarIdx].LevelNum;
            double[,] gridData = new double[yNum, xNum];
            double aValue;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = new double[lNum];
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, lNum];

            br.BaseStream.Position = timeIdx * recsPerTime * recLen;
            br.BaseStream.Position += indexLen;
            long aLevPosition = br.BaseStream.Position;
            levIdx = Variables[cvarIdx].LevelIdxs[0];
            for (int i = 0; i < lNum; i++)
            {
                varIdx = Variables[cvarIdx].VarInLevelIdxs[i];
                levIdx = Variables[cvarIdx].LevelIdxs[i];
                br.BaseStream.Position = aLevPosition;
                for (int j = 0; j < levIdx; j++)
                {
                    br.BaseStream.Position += LevelVarList[j].Count * recLen;
                }
                br.BaseStream.Position += varIdx * recLen;

                //Read label
                aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
                aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
                aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
                aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
                aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

                //Read Data
                dataBytes = br.ReadBytes(recLen - 50);
                gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);
                aValue = gridData[latIdx, lonIdx];
                aGridData.X[i] = levels[levIdx];
                aGridData.Data[0, i] = aValue;                
            }

            br.Close();
            fs.Close();

            return aGridData;
        }

        ///// <summary>
        ///// Get ARL data - Level
        ///// </summary>
        ///// <param name="lonIdx"></param>
        ///// <param name="latIdx"></param>
        ///// <param name="cvarIdx"></param>
        ///// <param name="timeIdx"></param>        
        ///// <returns></returns>
        //public List<PointD> GetARLGridData_Level(int lonIdx, int latIdx, int cvarIdx, int timeIdx)
        //{
        //    PointD aPoint = new PointD();
        //    List<PointD> pointList = new List<PointD>();

        //    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    int xNum, yNum, varIdx, levIdx, lNum;
        //    xNum = dataHead.NX;
        //    yNum = dataHead.NY;
        //    lNum = Variables[cvarIdx].LevelNum;
        //    double[,] gridData = new double[yNum, xNum];
        //    double aValue;

        //    br.BaseStream.Position += recLen;
        //    long aLevPosition = br.BaseStream.Position;
        //    levIdx = Variables[cvarIdx].LevelIdxs[0];
        //    for (int i = 0; i < lNum; i++)
        //    {
        //        varIdx = Variables[cvarIdx].VarInLevelIdxs[i];
        //        levIdx = Variables[cvarIdx].LevelIdxs[i];
        //        br.BaseStream.Position = aLevPosition;
        //        for (int j = 0; j < levIdx; j++)
        //        {
        //            br.BaseStream.Position += LevelVarList[j].Count * recLen;
        //        }
        //        br.BaseStream.Position += varIdx * recLen;

        //        //Read label
        //        aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //        aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //        aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //        aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //        //Read Data
        //        dataBytes = br.ReadBytes(recLen - 50);
        //        gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);
        //        aValue = gridData[latIdx, lonIdx];
        //        if (!(Math.Abs(aValue / MissingValue - 1) < 0.01))
        //        {
        //            aPoint.X = aValue;
        //            aPoint.Y = levels[levIdx];
        //            pointList.Add(aPoint);
        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return pointList;
        //}

        /// <summary>
        /// Get ARL data - Longitude
        /// </summary>
        /// <param name="timeIdx"></param>
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_Lon(int timeIdx, int latIdx, int varIdx, int levelIdx)
        {            
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] dataBytes;
            DataLabel aDL = new DataLabel();
            int xNum, yNum, i;
            xNum = dataHead.NX;
            yNum = dataHead.NY;
            double[,] gridData = new double[yNum, xNum];
            double aValue;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = X;
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, X.Length];

            //Update level and variable index
            Variable aVar = Variables[varIdx];
            if (aVar.LevelNum > 1)
                levelIdx += 1;

            varIdx = LevelVarList[levelIdx].IndexOf(aVar.Name);

            br.BaseStream.Position = timeIdx * recsPerTime * recLen;
            br.BaseStream.Position += indexLen;
            for (i = 0; i < levelIdx; i++)
            {
                br.BaseStream.Position += LevelVarList[i].Count * recLen;
            }
            br.BaseStream.Position += varIdx * recLen;

            //Read label
            aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
            aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
            aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
            aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

            //Read Data
            dataBytes = br.ReadBytes(recLen - 50);
            gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

            for (i = 0; i < xNum; i++)
            {
                aValue = gridData[latIdx, i];
                aGridData.Data[0, i] = aValue;                
            }

            br.Close();
            fs.Close();

            return aGridData;
        }

        ///// <summary>
        ///// Get ARL data - Longitude
        ///// </summary>
        ///// <param name="timeIdx"></param>
        ///// <param name="latIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="levelIdx"></param>        
        ///// <returns></returns>
        //public List<PointD> GetARLGridData_Lon(int timeIdx, int latIdx, int varIdx, int levelIdx)
        //{
        //    PointD aPoint = new PointD();
        //    List<PointD> pointList = new List<PointD>();

        //    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    int xNum, yNum, i;
        //    xNum = dataHead.NX;
        //    yNum = dataHead.NY;
        //    double[,] gridData = new double[yNum, xNum];
        //    double aValue;

        //    br.BaseStream.Position = timeIdx * recsPerTime * recLen;
        //    br.BaseStream.Position += recLen;
        //    for (i = 0; i < levelIdx; i++)
        //    {
        //        br.BaseStream.Position += LevelVarList[i].Count * recLen;
        //    }
        //    br.BaseStream.Position += varIdx * recLen;

        //    //Read label
        //    aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //    aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //    aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //    aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //    //Read Data
        //    dataBytes = br.ReadBytes(recLen - 50);
        //    gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

        //    for (i = 0; i < xNum; i++)
        //    {
        //        aValue = gridData[latIdx, i];
        //        if (!(Math.Abs(aValue / MissingValue - 1) < 0.01))
        //        {
        //            aPoint.X = X[i];
        //            aPoint.Y = aValue;
        //            pointList.Add(aPoint);
        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return pointList;
        //}

        /// <summary>
        /// Get ARL data - Latitude
        /// </summary>
        /// <param name="timeIdx"></param>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>       
        /// <returns></returns>
        public GridData GetGridData_Lat(int timeIdx, int lonIdx, int varIdx, int levelIdx)
        {
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] dataBytes;
            DataLabel aDL = new DataLabel();
            int xNum, yNum, i;
            xNum = dataHead.NX;
            yNum = dataHead.NY;
            double[,] gridData = new double[yNum, xNum];
            double aValue;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = Y;
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, Y.Length];

            //Update level and variable index
            Variable aVar = Variables[varIdx];
            if (aVar.LevelNum > 1)
                levelIdx += 1;

            varIdx = LevelVarList[levelIdx].IndexOf(aVar.Name);

            br.BaseStream.Position = timeIdx * recsPerTime * recLen;
            br.BaseStream.Position += indexLen;
            for (i = 0; i < levelIdx; i++)
            {
                br.BaseStream.Position += LevelVarList[i].Count * recLen;
            }
            br.BaseStream.Position += varIdx * recLen;

            //Read label
            aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
            aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
            aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
            aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
            aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

            //Read Data
            dataBytes = br.ReadBytes(recLen - 50);
            gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

            for (i = 0; i < yNum; i++)
            {
                aValue = gridData[i, lonIdx];
                aGridData.Data[0, i] = aValue;                
            }

            br.Close();
            fs.Close();

            return aGridData;
        }

        ///// <summary>
        ///// Get ARL data - Latitude
        ///// </summary>
        ///// <param name="timeIdx"></param>
        ///// <param name="lonIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="levelIdx"></param>       
        ///// <returns></returns>
        //public List<PointD> GetARLGridData_Lat(int timeIdx, int lonIdx, int varIdx, int levelIdx)
        //{
        //    PointD aPoint = new PointD();
        //    List<PointD> pointList = new List<PointD>();

        //    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    int xNum, yNum, i;
        //    xNum = dataHead.NX;
        //    yNum = dataHead.NY;
        //    double[,] gridData = new double[yNum, xNum];
        //    double aValue;

        //    br.BaseStream.Position = timeIdx * recsPerTime * recLen;
        //    br.BaseStream.Position += recLen;
        //    for (i = 0; i < levelIdx; i++)
        //    {
        //        br.BaseStream.Position += LevelVarList[i].Count * recLen;
        //    }
        //    br.BaseStream.Position += varIdx * recLen;

        //    //Read label
        //    aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //    aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //    aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //    aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //    //Read Data
        //    dataBytes = br.ReadBytes(recLen - 50);
        //    gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

        //    for (i = 0; i < yNum; i++)
        //    {
        //        aValue = gridData[i, lonIdx];
        //        if (!(Math.Abs(aValue / MissingValue - 1) < 0.01))
        //        {
        //            aPoint.X = Y[i];
        //            aPoint.Y = aValue;
        //            pointList.Add(aPoint);
        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return pointList;
        //}

        /// <summary>
        /// Generate data info text
        /// </summary>        
        /// <returns></returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + this.FileName;
            dataInfo += Environment.NewLine + "File Start Time: " + times[0].ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "File End Time: " + times[times.Count - 1].ToString("yyyy-MM-dd HH:00");
            dataInfo += Environment.NewLine + "Record Length Bytes: " + recLen.ToString();
            dataInfo += Environment.NewLine + "Meteo Data Model: " + dataHead.MODEL;
            dataInfo += Environment.NewLine + "Xsize = " + dataHead.NX.ToString() +
                    "  Ysize = " + dataHead.NY.ToString() + "  Zsize = " + dataHead.NZ.ToString() +
                    "  Tsize = " + Times.Count.ToString();
            dataInfo += Environment.NewLine + "Record Per Time: " + recsPerTime.ToString();
            dataInfo += Environment.NewLine + "Number of Surface Variables = " + LevelVarList[0].Count.ToString();
            foreach (string v in LevelVarList[0])
            {
                dataInfo += Environment.NewLine + "  " + v;
            }
            if (LevelVarList.Count > 1)
            {
                dataInfo += Environment.NewLine + "Number of Upper Variables = " + LevelVarList[1].Count.ToString();
                foreach (string v in LevelVarList[1])
                {
                    dataInfo += Environment.NewLine + "  " + v;
                }
            }
            dataInfo += Environment.NewLine + "Pole pnt lat/lon: " +
                dataHead.POLE_LAT.ToString() + "  " + dataHead.POLE_LON.ToString();
            dataInfo += Environment.NewLine + "Reference pnt lat/lon: " +
                            dataHead.REF_LAT.ToString() + "  " + dataHead.REF_LON.ToString();
            dataInfo += Environment.NewLine + "Grid Size: " + dataHead.SIZE.ToString();
            dataInfo += Environment.NewLine + "Orientation: " + dataHead.ORIENT.ToString();
            dataInfo += Environment.NewLine + "Tan lat/cone: " + dataHead.TANG_LAT.ToString();
            dataInfo += Environment.NewLine + "Syn pnt x/y: " + dataHead.SYNC_XP.ToString() +
                "  " + dataHead.SYNC_YP.ToString();
            dataInfo += Environment.NewLine + "Syn pnt lat/lon: " + dataHead.SYNC_LAT.ToString() +
                "  " + dataHead.SYNC_LON.ToString();

            return dataInfo;
        }

        #endregion

        #region Write
        /// <summary>
        /// Create a ARL binary data file
        /// </summary>
        /// <param name="aFile">file path</param>
        public void CreateDataFile(string aFile)
        {
            _fs = new FileStream(aFile, FileMode.Create, FileAccess.Write);
            _bw = new BinaryWriter(_fs);
        }

        /// <summary>
        /// Close the data file created by previos step
        /// </summary>
        public void CloseDataFile()
        {
            _bw.Close();
            _fs.Close();
        }

        /// <summary>
        /// Get data header of index record
        /// </summary>
        /// <param name="projInfo">Projection info</param>
        /// <param name="model">Data source</param>
        /// <param name="kFlag">Level flag</param>
        /// <returns>The data header</returns>
        public DataHead GetDataHead(ProjectionInfo projInfo, string model, int kFlag)
        {
            int i;
            DataHead aDH = new DataHead();
            aDH.MODEL = model;
            aDH.ICX = 0;
            aDH.MN = 0;
            aDH.K_FLAG = (short)kFlag;
            aDH.LENH = 108;
            for (i = 0; i < levels.Count; i++)
            {
                aDH.LENH += this.LevelVarList[i].Count * 8 + 8;
            }

            if (projInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat)
            {                
                aDH.POLE_LAT = 90;
                aDH.POLE_LON = 0;
                aDH.REF_LAT = (float)(Y[1] - Y[0]);
                aDH.REF_LON = (float)(X[1] - X[0]);
                aDH.SIZE = 0;
                aDH.ORIENT = 0;
                aDH.TANG_LAT = 0;
                aDH.SYNC_XP = 1;
                aDH.SYNC_YP = 1;
                aDH.SYNC_LAT = (float)Y[0];
                aDH.SYNC_LON = (float)X[0];
                aDH.DUMMY = 0;
                aDH.NX = X.Length;
                aDH.NY = Y.Length;
                aDH.NZ = levels.Count;
            }
            else
            {
                ProjectionInfo toProj = KnownCoordinateSystems.Geographic.World.WGS1984;
                double sync_lon, sync_lat;
                double[][] points = new double[1][];
                points[0] = new double[] { X[0], Y[0] };
                Reproject.ReprojectPoints(points, projInfo, toProj, 0, 1);
                sync_lon = points[0][0];
                sync_lat = points[0][1];                
                switch (projInfo.Transform.ProjectionName)
                {
                    case ProjectionNames.Lambert_Conformal:
                        double tanLat = this.Eqvlat((double)projInfo.StandardParallel1, (double)projInfo.StandardParallel2);
                        aDH.POLE_LAT = 90;
                        aDH.POLE_LON = 0;
                        aDH.REF_LAT = (float)tanLat;
                        aDH.REF_LON = (float)projInfo.CentralMeridian;
                        aDH.SIZE = (float)(X[1] - X[0]) / 1000;
                        aDH.ORIENT = 0;
                        aDH.TANG_LAT = (float)tanLat;
                        aDH.SYNC_XP = 1;
                        aDH.SYNC_YP = 1;
                        aDH.SYNC_LAT = (float)sync_lat;
                        aDH.SYNC_LON = (float)sync_lon;
                        aDH.DUMMY = 0;
                        aDH.NX = X.Length;
                        aDH.NY = Y.Length;
                        aDH.NZ = levels.Count;
                        break;
                    case ProjectionNames.Mercator:
                        aDH.POLE_LAT = 90;
                        aDH.POLE_LON = 0;
                        aDH.REF_LAT = 0;
                        aDH.REF_LON = (float)projInfo.CentralMeridian;
                        aDH.SIZE = (float)(X[1] - X[0]) / 1000;
                        aDH.ORIENT = 0;
                        aDH.TANG_LAT = aDH.REF_LAT;
                        aDH.SYNC_XP = 1;
                        aDH.SYNC_YP = 1;
                        aDH.SYNC_LAT = (float)sync_lat;
                        aDH.SYNC_LON = (float)sync_lon;
                        aDH.DUMMY = 0;
                        aDH.NX = X.Length;
                        aDH.NY = Y.Length;
                        aDH.NZ = levels.Count;
                        break;
                    case ProjectionNames.North_Polar_Stereographic:
                        aDH.POLE_LAT = 90;
                        aDH.POLE_LON = 0;
                        aDH.REF_LAT = 90;
                        aDH.REF_LON = (float)projInfo.CentralMeridian;
                        aDH.SIZE = (float)(X[1] - X[0]) / 1000;
                        aDH.ORIENT = 0;
                        aDH.TANG_LAT = aDH.REF_LAT;
                        aDH.SYNC_XP = 1;
                        aDH.SYNC_YP = 1;
                        aDH.SYNC_LAT = (float)sync_lat;
                        aDH.SYNC_LON = (float)sync_lon;
                        aDH.DUMMY = 0;
                        aDH.NX = X.Length;
                        aDH.NY = Y.Length;
                        aDH.NZ = levels.Count;
                        break;
                    case ProjectionNames.South_Polar_Stereographic:
                        aDH.POLE_LAT = -90;
                        aDH.POLE_LON = 0;
                        aDH.REF_LAT = -90;
                        aDH.REF_LON = (float)projInfo.CentralMeridian;
                        aDH.SIZE = (float)(X[1] - X[0]) / 1000;
                        aDH.ORIENT = 0;
                        aDH.TANG_LAT = aDH.REF_LAT;
                        aDH.SYNC_XP = 1;
                        aDH.SYNC_YP = 1;
                        aDH.SYNC_LAT = (float)sync_lat;
                        aDH.SYNC_LON = (float)sync_lon;
                        aDH.DUMMY = 0;
                        aDH.NX = X.Length;
                        aDH.NY = Y.Length;
                        aDH.NZ = levels.Count;
                        break;
                }
            }

            return aDH;
        }

        private string PadNumStr(string str, int n)
        {
            string nstr = str;
            if (nstr.IndexOf('.') < 0)
            {
                nstr = nstr + ".";
            }
            if (nstr.Length > 6)
            {
                nstr = nstr.Substring(0, 6);
            }
            return nstr.PadRight(n, '0');
        }

        /// <summary>
        /// Write index record
        /// </summary>
        /// <param name="time">The time</param>
        /// <param name="aDH">The data header</param>
        public void WriteIndexRecord(DateTime time, DataHead aDH)
        {
            //write the standard label (50) plus the 
            //fixed portion (108) of the extended header   
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(time.ToString("yy")));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(time.ToString("MM")));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(time.ToString("dd")));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(time.ToString("HH")));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes("00"));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes("00"));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes("11"));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes("INDX"));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes("   0"));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(" 0.0000000E+00"));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(" 0.0000000E+00"));

            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDH.MODEL.PadRight(4, '1')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDH.ICX.ToString().PadLeft(3, ' ')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDH.MN.ToString().PadLeft(2, ' ')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(PadNumStr(aDH.POLE_LAT.ToString(), 7)));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(PadNumStr(aDH.POLE_LON.ToString(), 7)));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(PadNumStr(aDH.REF_LAT.ToString(), 7)));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(PadNumStr(aDH.REF_LON.ToString(), 7)));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(PadNumStr(aDH.SIZE.ToString(), 7)));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(PadNumStr(aDH.ORIENT.ToString(), 7)));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(PadNumStr(aDH.TANG_LAT.ToString(), 7)));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(PadNumStr(aDH.SYNC_XP.ToString(), 7)));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(PadNumStr(aDH.SYNC_YP.ToString(), 7)));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(PadNumStr(aDH.SYNC_LAT.ToString(), 7)));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(PadNumStr(aDH.SYNC_LON.ToString(), 7)));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(PadNumStr(aDH.DUMMY.ToString(), 7)));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDH.NX.ToString().PadLeft(3, ' ')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDH.NY.ToString().PadLeft(3, ' ')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDH.NZ.ToString().PadLeft(3, ' ')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDH.K_FLAG.ToString().PadLeft(2, ' ')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDH.LENH.ToString().PadLeft(4, ' ')));
            string levStr;
            for (int i = 0; i < aDH.NZ; i++)
            {
                levStr = PadNumStr(((float)levels[i]).ToString(), 6);
                _bw.Write(System.Text.Encoding.ASCII.GetBytes(levStr));
                int vNum = LevelVarList[i].Count;
                _bw.Write(System.Text.Encoding.ASCII.GetBytes(vNum.ToString().PadLeft(2, ' ')));
                for (int j = 0; j < vNum; j++)
                {
                    _bw.Write(System.Text.Encoding.ASCII.GetBytes(LevelVarList[i][j].PadRight(4, '1')));
                    _bw.Write(System.Text.Encoding.ASCII.GetBytes("226"));
                    _bw.Write(System.Text.Encoding.ASCII.GetBytes(" "));
                }
            }
            _bw.Write(new byte[aDH.NY * aDH.NX - aDH.LENH]);
        }

        /// <summary>
        /// Write grid data
        /// </summary>
        /// <param name="aDL">The data label</param>
        /// <param name="gridData">The grid data</param>
        public void WriteGridData(DataLabel aDL, GridData gridData)
        {
            byte[] dataBytes = PackARLGridData(gridData, aDL);

            //write data label
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDL.Time.ToString("yy")));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDL.Time.ToString("MM")));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDL.Time.ToString("dd")));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDL.Time.ToString("HH")));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDL.Forecast.ToString().PadLeft(2, ' ')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDL.Level.ToString().PadLeft(2, ' ')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDL.Grid.ToString().PadLeft(2, ' ')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDL.Variable.PadRight(4, '1')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDL.Exponent.ToString().PadLeft(4, ' ')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDL.Precision.ToString("0.0000000E+00").PadLeft(14, ' ')));
            _bw.Write(System.Text.Encoding.ASCII.GetBytes(aDL.Value.ToString("0.0000000E+00").PadLeft(14, ' ')));

            //Write data
            _bw.Write(dataBytes);
        }

        /// <summary>
        /// Write grid data
        /// </summary>
        /// <param name="time">The time</param>
        /// <param name="levelIdx">The level index</param>
        /// <param name="varName">The variable</param>
        /// <param name="forecast">The forecast hour</param>
        /// <param name="grid">The grid id</param>
        /// <param name="gridData">The grid data</param>
        public void WriteGridData(DateTime time, int levelIdx, string varName, int forecast, int grid, GridData gridData)
        {
            DataLabel aDL = new DataLabel(time);
            aDL.Level = (short)levelIdx;
            aDL.Variable = varName;
            aDL.Grid = (short)grid;
            aDL.Forecast = (short)forecast;
            WriteGridData(aDL, gridData);
        }

        private byte[] PackARLGridData(GridData gridData, DataLabel aDL)
        {
            int nx = gridData.XNum;
            int ny = gridData.YNum;
            double var1 = gridData.Data[0, 0];
            double rold = var1;
            double rmax = 0.0;
            int i, j;
            //Find the maximum difference between adjacent elements
            for (i = 0; i < ny; i++)
            {
                for (j = 0; j < nx; j++)
                {
                    //Compute max difference between elements along row
                    rmax = Math.Max(Math.Abs(gridData.Data[i, j] - rold), rmax);
                    rold = gridData.Data[i, j];
                }
                rold = gridData.Data[i, 0];
            }

            double sexp = 0.0;
            //Compute the required scaling exponent
            if (rmax != 0.0)
            {
                sexp = Math.Log(rmax) / Math.Log(2.0);
            }
            int nexp = (int)sexp;
            //Positive or whole number scaling round up for lower precision
            if (sexp >= 0.0 || sexp % 1.0 == 0.0)
            {
                nexp += 1;
            }
            //Precision range is -127 to 127 or 254
            double prec = Math.Pow(2.0, nexp) / 254.0;

            byte[] dataBytes = new byte[ny * nx];
            double SCALE = Math.Pow(2.0, (7 - nexp));
            double VOLD = var1;
            double rcol = var1;
            int ksum = 0;
            int INDX = 0; 
            int ival;
            for (j = 0; j < ny; j++)
            {
                VOLD = rcol;
                for (i = 0; i < nx; i++)
                {
                    ival = (int)((gridData.Data[j, i] - VOLD) * SCALE + 127.5);
                    dataBytes[INDX] = (byte)ival;
                    VOLD = (float)(ival - 127) /SCALE + VOLD;
                    if (i == 0)
                    {
                        rcol = VOLD;
                    }
                    //maintain fotatin checksum
                    ksum += ival;
                    //if sum carries over the eight bit add one
                    if (ksum >= 256)
                    {
                        ksum = ksum - 255;
                    }
                    INDX += 1;
                    //VOLD = gridData.Data[j, i];                                        
                }
                //VOLD = gridData.Data[j, 0];
            }

            aDL.Exponent = nexp;
            aDL.Precision = prec;
            aDL.Value = var1;

            return dataBytes;
        }

        #endregion

        #endregion
    }
}
