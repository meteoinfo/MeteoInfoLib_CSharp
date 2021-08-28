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
    /// GRIB 1 data info
    /// </summary>
    public class GRIB1DataInfo : DataInfo,IGridDataInfo
    {
        #region Variables
        /// <summary>
        /// Is Lat/Lon
        /// </summary>
        public bool IsLatLon;
        /// <summary>
        /// List of lengths of all records
        /// </summary>
        public List<long> TimeStartPos;        
        /// <summary>
        /// X array
        /// </summary>
        public double[] X;
        /// <summary>
        /// Y array
        /// </summary>
        public double[] Y;
        /// <summary>
        /// Total message number
        /// </summary>
        public int MessageNumber;
        /// <summary>
        /// Message index list
        /// </summary>
        public List<GRIB1MessageIndex> MessageIdxList;
        /// <summary>
        /// Header length
        /// </summary>
        public int HeaderLength = 0;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public GRIB1DataInfo()
        {
            IsLatLon = true;                  
            TimeStartPos = new List<long>();            
            Variables = new List<Variable>();
            MessageIdxList = new List<GRIB1MessageIndex>();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Read GRIB data info
        /// </summary>
        /// <param name="aFile">file path</param>
        public override void ReadDataInfo(string aFile)
        {
            FileName = aFile;
            FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            //bool isNewTime = false;
            int recordNum = 0;
            bool setGrid = false;
            int gridNum = 0;
            List<DateTime> times = new List<DateTime>();
            List<Variable> variables = new List<Variable>();
            while (br.BaseStream.Position < br.BaseStream.Length - 30)
            {
                br.BaseStream.Seek(HeaderLength, SeekOrigin.Current);
                string title = ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
                br.BaseStream.Seek(-4, SeekOrigin.Current);
                if (title != "GRIB")
                    br.BaseStream.Seek(2, SeekOrigin.Current);

                GRIB1MessageIndex messageIdx = new GRIB1MessageIndex();
                messageIdx.Position = br.BaseStream.Position;

                //Read message setting
                GRIB1IndicatorSection rIS = new GRIB1IndicatorSection(br);
                if (rIS.Edition != 1)
                    break;

                GRIB1ProductDefineSection rPDS = new GRIB1ProductDefineSection(br);
                if (!setGrid)
                {
                    if (rPDS.GDSExist)
                    {
                        if (gridNum == 0)
                        {
                            GRIB1GridDefineSection rGDS = new GRIB1GridDefineSection(br);
                            ProjectionInfo = rGDS.GetProjectionInfo();
                            if (ProjectionInfo.Transform.Proj4Name != "lonlat")
                                IsLatLon = false;
                            X = new double[1];
                            Y = new double[1];
                            rGDS.GetXYArray(ref X, ref Y);
                            if (ProjectionInfo.Transform.Proj4Name == "lonlat")
                            {
                                if (X[X.Length - 1] + (X[1] - X[0]) - X[0] == 360)
                                    IsGlobal = true;
                            }

                            Dimension xDim = new Dimension(DimensionType.X);
                            xDim.SetValues(X);
                            this.XDimension = xDim;
                            Dimension yDim = new Dimension(DimensionType.Y);
                            yDim.SetValues(Y);
                            this.YDimension = yDim;

                            br.BaseStream.Seek(rIS.RecordLength - rIS.Length - rPDS.Length - rGDS.Length, SeekOrigin.Current);
                            gridNum += 1;
                        }
                        else
                            br.BaseStream.Seek(rIS.RecordLength - rIS.Length - rPDS.Length, SeekOrigin.Current);
                    }
                    else
                    {
                        br.BaseStream.Seek(rIS.RecordLength - rIS.Length - rPDS.Length, SeekOrigin.Current);
                        if (gridNum == 0)
                        {
                            if (rPDS.CenterID == 7)    //NCEP
                            {
                                GetStorageGrid_NCEP(rPDS.GridID);
                                gridNum += 1;

                                Dimension xDim = new Dimension(DimensionType.X);
                                xDim.SetValues(X);
                                this.XDimension = xDim;
                                Dimension yDim = new Dimension(DimensionType.Y);
                                yDim.SetValues(Y);
                                this.YDimension = yDim;
                            }
                        }
                    }
                }
                else
                    br.BaseStream.Seek(rIS.RecordLength - rIS.Length - rPDS.Length, SeekOrigin.Current);
                recordNum += 1;
                br.BaseStream.Seek(HeaderLength, SeekOrigin.Current);

                //Get validate time
                DateTime theTime = rPDS.GetForecastTime();

                //Get parameter
                Variable bPar = (Variable)rPDS.Parameter.Clone();
                bPar.LevelType = rPDS.LevelType;
                bPar.Name = bPar.Name + "@" + GRIBParameterTable.getTypeSurfaceNameShort(bPar.LevelType);

                //Set dimensions
                bool isAdd = true;
                bool isEqual = false;
                foreach (Variable aPar in variables)
                {
                    if (aPar.TEquals(bPar))
                    {
                        if (rPDS.LevelType != 1)
                        {
                            //if (!aPar.TDimension.DimValue.Contains(DataConvert.ToDouble(theTime)))
                            //    aPar.TDimension.AddValue(DataConvert.ToDouble(theTime));
                            //if (!aPar.ZDimension.DimValue.Contains(rPDS.LevelValue))
                            //    aPar.ZDimension.AddValue(rPDS.LevelValue);
                            aPar.AddLevel(rPDS.LevelValue);
                        }
                        isAdd = false;
                        break;
                    }
                    if (aPar.Equals(bPar))
                        isEqual = true;
                }
                if (isAdd && isEqual)
                {
                    bPar.Name = bPar.Name + "_" + bPar.LevelType;
                    foreach (Variable aPar in variables)
                    {
                        if (aPar.TEquals(bPar))
                        {
                            if (rPDS.LevelType != 1)
                            {
                                //if (!aPar.TDimension.DimValue.Contains(DataConvert.ToDouble(theTime)))
                                //    aPar.TDimension.AddValue(DataConvert.ToDouble(theTime));
                                //if (!aPar.ZDimension.DimValue.Contains(rPDS.LevelValue))
                                //    aPar.ZDimension.AddValue(rPDS.LevelValue);
                                aPar.AddLevel(rPDS.LevelValue);
                            }
                            isAdd = false;
                            break;
                        }
                    }
                }
                if (isAdd)
                {                    
                    if (rPDS.LevelType != 1)
                        bPar.AddLevel(rPDS.LevelValue);

                    variables.Add(bPar);
                }

                if (!times.Contains(theTime))
                {
                    TimeStartPos.Add(br.BaseStream.Position - rIS.RecordLength);
                    times.Add(theTime);
                    //if (Times.Count > 1)
                    //    isNewTime = true;
                }

                //Set message index                
                messageIdx.DateTime = theTime;
                messageIdx.Level = rPDS.LevelValue;
                messageIdx.Parameter = bPar;
                MessageIdxList.Add(messageIdx);
            }
            MessageNumber = MessageIdxList.Count;

            this.Times = times;
            List<double> values = new List<double>();
            foreach (DateTime t in times)
            {
                values.Add(DataConvert.ToDouble(t));
            }
            Dimension tDim = new Dimension(DimensionType.T);
            tDim.SetValues(values);
            this.TimeDimension = tDim;

            foreach (Variable var in variables)
            {
                var.SetDimension(tDim);
                Dimension zDim = new Dimension(DimensionType.Z);
                zDim.SetValues(var.Levels);
                this.ZDimension = zDim;
                var.SetDimension(zDim);
                var.SetDimension(YDimension);
                var.SetDimension(XDimension);
            }
            this.Variables = variables;

            br.Close();
            fs.Close();
        }

        ///// <summary>
        ///// Read GRIB data info
        ///// </summary>
        ///// <param name="aFile">file path</param>
        //public void ReadDataInfo(string aFile)
        //{
        //    FileName = aFile;
        //    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);

        //    bool isNewTime = false;
        //    int recordNum = 0;
        //    bool setGrid = false;
        //    int gridNum = 0;
        //    while (br.BaseStream.Position < br.BaseStream.Length - 30)
        //    {
        //        br.BaseStream.Seek(HeaderLength, SeekOrigin.Current);
        //        string title = ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //        br.BaseStream.Seek(-4, SeekOrigin.Current);
        //        if (title != "GRIB")
        //            br.BaseStream.Seek(2, SeekOrigin.Current);

        //        GRIB1MessageIndex messageIdx = new GRIB1MessageIndex();
        //        messageIdx.Position = br.BaseStream.Position;

        //        //Read message setting
        //        GRIB1IndicatorSection rIS = new GRIB1IndicatorSection(br);
        //        if (rIS.Edition != 1)
        //            break;

        //        GRIB1ProductDefineSection rPDS = new GRIB1ProductDefineSection(br);
        //        if (!setGrid)
        //        {
        //            if (rPDS.GDSExist && gridNum == 0)
        //            {
        //                GRIB1GridDefineSection rGDS = new GRIB1GridDefineSection(br);
        //                ProjInfo = rGDS.GetProjectionInfo();
        //                if (ProjInfo.Transform.Proj4Name != "lonlat")
        //                    IsLatLon = false;
        //                X = new double[1];
        //                Y = new double[1];
        //                rGDS.GetXYArray(ref X, ref Y);
        //                if (ProjInfo.Transform.Proj4Name == "lonlat")
        //                {
        //                    if (X[X.Length - 1] + (X[1] - X[0]) - X[0] == 360)
        //                        IsGlobal = true;
        //                }
        //                br.BaseStream.Seek(rIS.RecordLength - rIS.Length - rPDS.Length - rGDS.Length, SeekOrigin.Current);
        //                gridNum += 1;
        //            }
        //            else
        //                br.BaseStream.Seek(rIS.RecordLength - rIS.Length - rPDS.Length, SeekOrigin.Current);
        //        }
        //        else
        //            br.BaseStream.Seek(rIS.RecordLength - rIS.Length - rPDS.Length, SeekOrigin.Current);
        //        recordNum += 1;
        //        br.BaseStream.Seek(HeaderLength, SeekOrigin.Current);

        //        //Get validate time
        //        DateTime theTime = rPDS.GetForecastTime();

        //        //Get parameter
        //        Variable bPar = (Variable)rPDS.Parameter.Clone();
        //        bPar.LevelType = rPDS.LevelType;

        //        //Set dimensions
        //        if (!Times.Contains(theTime))
        //        {
        //            TimeStartPos.Add(br.BaseStream.Position - rIS.RecordLength);
        //            Times.Add(theTime);
        //            if (Times.Count > 1)
        //                isNewTime = true;
        //        }

        //        if (!isNewTime)
        //        {
        //            bool isAdd = true;
        //            bool isEqual = false;
        //            foreach (Variable aPar in Variables)
        //            {
        //                if (aPar.TEquals(bPar))
        //                {
        //                    if (rPDS.LevelType != 1)
        //                    {
        //                        aPar.AddLevel(rPDS.LevelValue);
        //                    }
        //                    isAdd = false;
        //                    break;
        //                }
        //                if (aPar.Equals(bPar))
        //                    isEqual = true;
        //            }
        //            if (isAdd && isEqual)
        //            {
        //                bPar.Name = bPar.Name + "_" + bPar.LevelType;
        //                foreach (Variable aPar in Variables)
        //                {
        //                    if (aPar.TEquals(bPar))
        //                    {
        //                        if (rPDS.LevelType != 1)
        //                        {
        //                            aPar.AddLevel(rPDS.LevelValue);
        //                        }
        //                        isAdd = false;
        //                        break;
        //                    }
        //                }
        //            }
        //            if (isAdd)
        //            {
        //                if (rPDS.LevelType != 1)
        //                    bPar.AddLevel(rPDS.LevelValue);

        //                Variables.Add(bPar);
        //            }
        //        }

        //        //Set message index                
        //        messageIdx.DateTime = theTime;
        //        messageIdx.Level = rPDS.LevelValue;
        //        messageIdx.Parameter = bPar;
        //        MessageIdxList.Add(messageIdx);
        //    }
        //    MessageNumber = MessageIdxList.Count;

        //    br.Close();
        //    fs.Close();
        //}

        private void GetStorageGrid_NCEP(int gridID)
        {
            bool isTrue = true;
            string projStr = KnownCoordinateSystems.Geographic.World.WGS1984.ToProj4String();
            int i, nx = 37, ny = 36;
            double dx = 5, dy = 2.5, sx = 0, sy = 0;
            switch (gridID)
            {
                case 21:
                    nx = 37;
                    ny = 36;
                    dx = 5;
                    dy = 2.5;
                    sx = 0;
                    sy = 0;                    
                    break;
                case 22:
                    nx = 37;
                    ny = 36;
                    dx = 5;
                    dy = 2.5;
                    sx = -180;
                    sy = 0;                     
                    break;
                case 23:
                    nx = 37;
                    ny = 36;
                    dx = 5;
                    dy = 2.5;
                    sx = 0;
                    sy = -90;                     
                    break;
                case 24:
                    nx = 37;
                    ny = 36;
                    dx = 5;
                    dy = 2.5;
                    sx = -180;
                    sy = -90;                     
                    break;
                case 25:
                    nx = 72;
                    ny = 18;
                    dx = 5;
                    dy = 5;
                    sx = 0;
                    sy = 0;                     
                    break;
                case 26:
                    nx = 72;
                    ny = 18;
                    dx = 5;
                    dy = 5;
                    sx = 0;
                    sy = -90;                    
                    break;
                case 61:
                    nx = 91;
                    ny = 45;
                    dx = 2;
                    dy = 2;
                    sx = 0;
                    sy = 0; 
                    break;
                case 62:
                    nx = 91;
                    ny = 45;
                    dx = 2;
                    dy = 2;
                    sx = -180;
                    sy = 0; 
                    break;
                case 63:
                    nx = 91;
                    ny = 45;
                    dx = 2;
                    dy = 2;
                    sx = 0;
                    sy = -90; 
                    break;
                case 64:
                    nx = 91;
                    ny = 45;
                    dx = 2;
                    dy = 2;
                    sx = -180;
                    sy = -90; 
                    break;
                default:
                    isTrue = false;
                    break;
            }

            if (isTrue)
            {
                X = new double[nx];
                Y = new double[ny];
                for (i = 0; i < nx; i++)
                    X[i] = sx + i * dx;
                for (i = 0; i < ny; i++)
                    Y[i] = sy + i * dy;

                ProjectionInfo = new ProjectionInfo(projStr);
            }
        }

        /// <summary>
        /// Generate info text
        /// </summary>
        /// <returns>info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            int i;

            dataInfo = "File Name: " + FileName;

            dataInfo += Environment.NewLine + "Xsize = " + X.Length.ToString() +
                    "  Ysize = " + Y.Length.ToString() + "  Zsize = " + GetLevelNumber().ToString() +
                    "  Tsize = " + Times.Count.ToString();

            dataInfo += Environment.NewLine + "Number of Variables = " + Variables.Count.ToString();
            foreach (Variable v in Variables)
                dataInfo += Environment.NewLine + "\t" + v.Name + " " + v.LevelNum.ToString() + " " +
                        v.Units + " " + v.Description;

            dataInfo += Environment.NewLine + "Tsize = " + Times.Count.ToString();
            for (i = 0; i < Times.Count; i++)
                dataInfo += Environment.NewLine + "\t" + Times[i].ToString("yyyy-MM-dd HH:mm:ss");

            dataInfo += Environment.NewLine + "Xsize = " + X.Length.ToString();
            for (i = 0; i < X.Length; i++)
                dataInfo += Environment.NewLine + "\t" + X[i].ToString();

            dataInfo += Environment.NewLine + "Ysize = " + Y.Length.ToString();
            for (i = 0; i < Y.Length; i++)
                dataInfo += Environment.NewLine + "\t" + Y[i].ToString();            

            return dataInfo;
        }

        private int GetLevelNumber()
        {
            int lNum = 0;
            foreach (Variable aPar in Variables)
            {
                if (aPar.LevelNum > lNum)
                    lNum = aPar.LevelNum;
            }

            return lNum;
        }

        /// <summary>
        /// Get GRIB 1 grid data - Lon/Lat
        /// </summary>
        /// <param name="timeIdx">time index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>       
        /// <returns>grid data</returns>
        public GridData GetGridData_LonLat_Old(int timeIdx, int varIdx, int levelIdx)
        {
            GridData gridData = new GridData();
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, j;

            //Seek times
            br.BaseStream.Seek(TimeStartPos[timeIdx], SeekOrigin.Current);

            //Seek Variables
            for (i = 0; i < varIdx; i++)
            {
                for (j = 0; j < Variables[i].GetTrueLevelNumber(); j++)
                {
                    SeekNextRecord(br);
                }
            }

            //Seek Levels
            Variable aVar = Variables[varIdx];
            for (i = 0; i < levelIdx; i++)
            {
                SeekNextRecord(br);
            }

            GRIB1Message aRecord = new GRIB1Message(br, this);
            gridData.MissingValue = MissingValue;            
            gridData.X = X;
            gridData.Y = Y;
            gridData.Data = GetDataArray(aRecord);

            return gridData;
        }

        private double[,] GetDataArray(BinaryReader br, long position, int timeIdx, int varIdx, int levelIdx)
        {
            br.BaseStream.Position = position;
            GRIB1Message aMessage = new GRIB1Message(br, this);
            double[,] theData = GetDataArray(aMessage);

            return theData;
        }

        /// <summary>
        /// Get GRIB 1 grid data - Lon/Lat
        /// </summary>
        /// <param name="timeIdx">time index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>       
        /// <returns>grid data</returns>
        public GridData GetGridData_LonLat(int timeIdx, int varIdx, int levelIdx)
        {
            GridData gridData = new GridData();
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i;

            //Get time, parameter and level
            GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();            
            aMessageIdx.Parameter = Variables[varIdx];
            //aMessageIdx.DateTime = Times[timeIdx];
            //aMessageIdx.DateTime = DateTime.FromOADate(aMessageIdx.Parameter.TDimension.DimValue[timeIdx]);
            aMessageIdx.DateTime = DataConvert.ToDateTime(aMessageIdx.Parameter.TDimension.DimValue[timeIdx]);
            if (aMessageIdx.Parameter.Levels.Count == 0)
                aMessageIdx.Level = 0;
            else
                aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];           

            //Get the message position
            long aPosition = 0;
            for (i = 0; i < MessageNumber; i++)
            {
                GRIB1MessageIndex mIdx = MessageIdxList[i];
                if (mIdx.Equals(aMessageIdx))
                {
                    aPosition = mIdx.Position;
                    break;
                }
            }

            //Read data
            br.BaseStream.Position = aPosition;
            GRIB1Message aMessage = new GRIB1Message(br, this);
            gridData.MissingValue = MissingValue;            
            gridData.X = X;
            gridData.Y = Y;
            gridData.Data = GetDataArray(aMessage);

            br.Close();
            fs.Close();

            //Gaussian grid
            switch (aMessage.RecordGDS.GridType)
            {
                case 4:    // Gaussian latitude/longitude
                    //gridData.GassianToLatLon();
                    gridData.GassianToLatLon_Simple();
                    break;
            }

            return gridData;
        }

        private void SeekNextRecord(BinaryReader br)
        {
            GRIB1IndicatorSection rIS = new GRIB1IndicatorSection(br);
            br.BaseStream.Seek(rIS.RecordLength - rIS.Length, SeekOrigin.Current);
        }

        private double[,] GetDataArray(GRIB1Message aMessage)
        {
            if (aMessage.RecordGDS != null)
            {
                if (aMessage.RecordGDS.ThinnedGrid)
                {
                    return GetDataArray_Thinned(aMessage);
                }
                else
                {
                    return GetDataArray(aMessage.RecordBDS, aMessage.RecordGDS.YReverse);
                }
            }
            else
            {
                return GetDataArray(aMessage.RecordBDS);
            }
        }

        private double[,] GetDataArray(GRIB1BinaryDataSection rBDS)
        {
            return GetDataArray(rBDS, false);
        }
        
        private double[,] GetDataArray(GRIB1BinaryDataSection rBDS, bool yReverse)
        {
            double[,] data = new double[Y.Length, X.Length];
            for (int i = 0; i < Y.Length; i++)
            {
                for (int j = 0; j < X.Length; j++)
                {
                    if (yReverse)    //-y
                        data[i, j] = rBDS.Data[(Y.Length - i - 1) * X.Length + j];
                    else
                        data[i, j] = rBDS.Data[i * X.Length + j];
                }
            }

            return data;
        }

        private double[,] GetDataArray_Thinned(GRIB1Message aMessage)
        {
            GRIB1BinaryDataSection rBDS = aMessage.RecordBDS;
            bool yReverse = aMessage.RecordGDS.YReverse;
            double[,] data = new double[Y.Length, X.Length];
            double xrange = X[X.Length - 1] - X[0];
            int dIdx = 0;
            for (int i = 0; i < Y.Length; i++)
            {
                int nx = aMessage.RecordGDS.ThinnedXNums[i];
                for (int j = 0; j < X.Length; j++)
                {
                    //Calulate the x index (mj) of thinned array (real value)
                    double mj = (double)j * (nx - 1.0) / (double)(X.Length - 1);
                    if (Math.Abs(mj - (int)mj) < 1.0E-10)
                        data[i, j] = rBDS.Data[dIdx + (int)mj];
                    else
                    {
                        // Get the 2 closest values from thinned array
                        double Vb = rBDS.Data[dIdx + (int)mj];
                        double Vc = rBDS.Data[dIdx + (int)mj + 1];
                        // Get the next two closest, if available:
                        double Va = -999999.0;
                        double Vd = -999999.0;
                        if (mj > 1.0)
                            Va = rBDS.Data[dIdx + (int)mj - 1];
                        if (mj < nx - 2)
                            Vd = rBDS.Data[dIdx + (int)mj + 2];
                        if ((Va < -999998.0) || (Vd < -999998.0))
                        {
                            // Use 2-point linear interpolation.
                            data[i, j] = Vb * ((int)mj + 1.0 - mj) + Vc * (mj - (int)mj);
                        }
                        else
                        {
                            // Use 4-point overlapping parabolic interpolation.
                            double xmj = mj - (float)((int)mj);
                            data[i, j] = oned(xmj, Va, Vb, Vc, Vd);
                        }

                    }
                }
                dIdx += nx;
            }

            return data;
        }

        private double oned(double X, double A, double B, double C, double D)
        {

            double Answer;
            if (Math.Abs(X) < 1.0E-10)
            {
                Answer = B;
                return Answer;
            }

            if (Math.Abs(X - 1.0) < 1.0E-10)
            {
                Answer = C;
                return Answer;
            }

            Answer = (1.0 - X) * (B + X * (0.5 * (C - A) + X * (0.5 * (C + A) - B))) + X * (C + (1.0 - X) * (0.5
            * (B - D) + (1.0 - X) * (0.5 * (B + D) - C)));

            return Answer;
        }

        /// <summary>
        /// Get GRIB1 data - Time/Lon
        /// </summary>
        /// <param name="latIdx">latitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>        
        /// <returns>grid data</returns>
        public GridData GetGridData_TimeLon(int latIdx, int varIdx, int levelIdx)
        {
            GridData gridData = new GridData();
            int xNum, yNum, tNum, t;
            xNum = X.Length;
            yNum = Y.Length;
            tNum = Times.Count;
            double[,] theData = new double[yNum, xNum];
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);            
            double[,] newGridData = new double[tNum, xNum];
            int i, j;

            for (t = 0; t < Times.Count; t++)
            {                
                //Get time, parameter and level
                GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
                aMessageIdx.DateTime = Times[t];
                aMessageIdx.Parameter = Variables[varIdx];
                if (aMessageIdx.Parameter.Levels.Count == 0)
                    aMessageIdx.Level = 0;
                else
                    aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

                //Get the message position
                long aPosition = 0;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB1MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aPosition = mIdx.Position;
                        break;
                    }
                }

                //Read data
                br.BaseStream.Position = aPosition;
                GRIB1Message aMessage = new GRIB1Message(br, this);                
                theData = GetDataArray(aMessage);
                for ( j = 0; j < xNum; j++)
                {
                    newGridData[t, j] = theData[latIdx, j];
                }
            }

            br.Close();
            fs.Close();

            double[] newY = new double[Times.Count];
            for (i = 0; i < Times.Count; i++)
                newY[i] = DataConvert.ToDouble(Times[i]);

            gridData.MissingValue = MissingValue;            
            gridData.X = X;
            gridData.Y = newY;
            gridData.Data = newGridData;

            return gridData;
        }

        /// <summary>
        /// Get GRIB1 data - Time/Lat
        /// </summary>
        /// <param name="lonIdx">longitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>        
        /// <returns>grid data</returns>
        public GridData GetGridData_TimeLat(int lonIdx, int varIdx, int levelIdx)
        {
            GridData gridData = new GridData();
            int xNum, yNum, tNum, t;
            xNum = X.Length;
            yNum = Y.Length;
            tNum = Times.Count;
            double[,] theData = new double[yNum, xNum];
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            double[,] newGridData = new double[tNum, yNum];
            int i;

            for (t = 0; t < Times.Count; t++)
            {
                //Get time, parameter and level
                GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
                aMessageIdx.DateTime = Times[t];
                aMessageIdx.Parameter = Variables[varIdx];
                if (aMessageIdx.Parameter.Levels.Count == 0)
                    aMessageIdx.Level = 0;
                else
                    aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

                //Get the message position
                long aPosition = 0;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB1MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aPosition = mIdx.Position;
                        break;
                    }
                }

                //Read data
                br.BaseStream.Position = aPosition;
                GRIB1Message aMessage = new GRIB1Message(br, this);
                theData = GetDataArray(aMessage);
                for (i = 0; i < yNum; i++)
                {
                    newGridData[t, i] = theData[i, lonIdx];
                }
            }

            br.Close();
            fs.Close();

            double[] newY = new double[Times.Count];
            for (i = 0; i < Times.Count; i++)
                newY[i] = DataConvert.ToDouble(Times[i]);

            gridData.MissingValue = MissingValue;           
            gridData.X = Y;
            gridData.Y = newY;
            gridData.Data = newGridData;

            return gridData;
        }

        /// <summary>
        /// Get GRIB1 data - Level/Lat
        /// </summary>
        /// <param name="lonIdx">longitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="tIdx">time index</param>        
        /// <returns>grid data</returns>
        public GridData GetGridData_LevelLat(int lonIdx, int varIdx, int tIdx)
        {
            GridData gridData = new GridData();
            Variable aPar = Variables[varIdx];
            int xNum, yNum, lNum;
            xNum = X.Length;
            yNum = Y.Length;            
            double[,] theData = new double[yNum, xNum];
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            lNum = aPar.Levels.Count;
            double[,] newGridData = new double[lNum, yNum];
            int i;            

            for (int l = 0; l < aPar.Levels.Count; l++)
            {
                //Get time, parameter and level
                GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
                aMessageIdx.DateTime = Times[tIdx];
                aMessageIdx.Parameter = Variables[varIdx];
                aMessageIdx.Level = aPar.Levels[l];
                    
                //Get the message position
                long aPosition = 0;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB1MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aPosition = mIdx.Position;
                        break;
                    }
                }

                //Read data
                br.BaseStream.Position = aPosition;
                GRIB1Message aMessage = new GRIB1Message(br, this);
                theData = GetDataArray(aMessage);
                for (i = 0; i < yNum; i++)
                {
                    newGridData[l, i] = theData[i, lonIdx];
                }
            }

            br.Close();
            fs.Close();

            double[] newY = new double[lNum];
            for (i = 0; i < lNum; i++)
                newY[i] = aPar.Levels[i];

            gridData.MissingValue = MissingValue;            
            gridData.X = Y;
            gridData.Y = newY;
            gridData.Data = newGridData;

            return gridData;
        }

        /// <summary>
        /// Get GRIB1 data - Level/Lon
        /// </summary>
        /// <param name="latIdx">latitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="tIdx">time index</param>        
        /// <returns>grid data</returns>
        public GridData GetGridData_LevelLon(int latIdx, int varIdx, int tIdx)
        {
            GridData gridData = new GridData();
            Variable aPar = Variables[varIdx];
            int xNum, yNum, lNum;
            xNum = X.Length;
            yNum = Y.Length;
            double[,] theData = new double[yNum, xNum];
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            lNum = aPar.Levels.Count;
            double[,] newGridData = new double[lNum, xNum];
            int i;

            for (int l = 0; l < aPar.Levels.Count; l++)
            {
                //Get time, parameter and level
                GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
                aMessageIdx.DateTime = Times[tIdx];
                aMessageIdx.Parameter = Variables[varIdx];
                aMessageIdx.Level = aPar.Levels[l];

                //Get the message position
                long aPosition = 0;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB1MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aPosition = mIdx.Position;
                        break;
                    }
                }

                //Read data
                br.BaseStream.Position = aPosition;
                GRIB1Message aMessage = new GRIB1Message(br, this);
                theData = GetDataArray(aMessage);
                for (i = 0; i < xNum; i++)
                {
                    newGridData[l, i] = theData[latIdx, i];
                }
            }

            br.Close();
            fs.Close();

            double[] newY = new double[lNum];
            for (i = 0; i < lNum; i++)
                newY[i] = aPar.Levels[i];

            gridData.MissingValue = MissingValue;            
            gridData.X = X;
            gridData.Y = newY;
            gridData.Data = newGridData;

            return gridData;
        }

        /// <summary>
        /// Get GRIB1 data - Level/Time
        /// </summary>
        /// <param name="latIdx">latitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="lonIdx">longitude index</param>        
        /// <returns>grid data</returns>
        public GridData GetGridData_LevelTime(int latIdx, int varIdx, int lonIdx)
        {
            GridData gridData = new GridData();
            Variable aPar = Variables[varIdx];
            int xNum, yNum, lNum, tNum;
            xNum = X.Length;
            yNum = Y.Length;
            tNum = Times.Count;
            double[,] theData = new double[yNum, xNum];
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            lNum = aPar.Levels.Count;
            double[,] newGridData = new double[lNum, tNum];
            int i;

            for (int l = 0; l < aPar.Levels.Count; l++)
            {
                for (int t = 0; t < Times.Count; t++)
                {
                    //Get time, parameter and level
                    GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
                    aMessageIdx.DateTime = Times[t];
                    aMessageIdx.Parameter = Variables[varIdx];
                    aMessageIdx.Level = aPar.Levels[l];

                    //Get the message position
                    long aPosition = 0;
                    for (i = 0; i < MessageNumber; i++)
                    {
                        GRIB1MessageIndex mIdx = MessageIdxList[i];
                        if (mIdx.Equals(aMessageIdx))
                        {
                            aPosition = mIdx.Position;
                            break;
                        }
                    }

                    //Read data
                    br.BaseStream.Position = aPosition;
                    GRIB1Message aMessage = new GRIB1Message(br, this);
                    theData = GetDataArray(aMessage);
                    newGridData[l, t] = theData[latIdx, lonIdx];                    
                }
            }

            br.Close();
            fs.Close();

            double[] newX = new double[Times.Count];
            for (i = 0; i < Times.Count; i++)
                newX[i] = DataConvert.ToDouble(Times[i]);

            double[] newY = new double[lNum];
            for (i = 0; i < lNum; i++)
                newY[i] = aPar.Levels[i];

            gridData.MissingValue = MissingValue;            
            gridData.X = newX;
            gridData.Y = newY;
            gridData.Data = newGridData;

            return gridData;
        }

        /// <summary>
        /// Get grid data - Time
        /// </summary>
        /// <param name="lonIdx">longitude index</param>
        /// <param name="latIdx">latitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>       
        /// <returns>grid data</returns>
        public GridData GetGridData_Time(int lonIdx, int latIdx, int varIdx, int levelIdx)
        {            
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            double[,] theData;
            int i;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = new double[Times.Count];
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, Times.Count];

            for (int t = 0; t < Times.Count; t++)
            {
                //Get time, parameter and level
                GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
                aMessageIdx.DateTime = Times[t];
                aMessageIdx.Parameter = Variables[varIdx];
                if (aMessageIdx.Parameter.Levels.Count == 0)
                    aMessageIdx.Level = 0;
                else
                    aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

                //Get the message position
                long aPosition = 0;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB1MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aPosition = mIdx.Position;
                        break;
                    }
                }

                //Read data
                br.BaseStream.Position = aPosition;
                GRIB1Message aMessage = new GRIB1Message(br, this);
                theData = GetDataArray(aMessage);
                double aValue = theData[latIdx, lonIdx];
                aGridData.X[t] = DataConvert.ToDouble(Times[t]);
                aGridData.Data[0, t] = aValue;                
            }

            br.Close();
            fs.Close();

            return aGridData;
        }

        /// <summary>
        /// Get grid data - Time
        /// </summary>
        /// <param name="lonIdx">longitude index</param>
        /// <param name="latIdx">latitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>       
        /// <returns>one dimension data</returns>
        public List<PointD> GetOneDimData_Time(int lonIdx, int latIdx, int varIdx, int levelIdx)
        {
            PointD aPoint = new PointD();
            List<PointD> pointList = new List<PointD>();
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            double[,] theData;
            int i;

            for (int t = 0; t < Times.Count; t++)
            {
                //Get time, parameter and level
                GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
                aMessageIdx.DateTime = Times[t];
                aMessageIdx.Parameter = Variables[varIdx];
                if (aMessageIdx.Parameter.Levels.Count == 0)
                    aMessageIdx.Level = 0;
                else
                    aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

                //Get the message position
                long aPosition = 0;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB1MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aPosition = mIdx.Position;
                        break;
                    }
                }

                //Read data
                br.BaseStream.Position = aPosition;
                GRIB1Message aMessage = new GRIB1Message(br, this);
                theData = GetDataArray(aMessage);
                double aValue = theData[latIdx, lonIdx];
                if (!(Math.Abs(aValue / MissingValue - 1) < 0.01))
                {
                    aPoint.X = Times[t].ToBinary();
                    aPoint.Y = aValue;
                    pointList.Add(aPoint);
                }
            }

            br.Close();
            fs.Close();

            return pointList;
        }

        /// <summary>
        /// Get ARL data - level
        /// </summary>
        /// <param name="lonIdx">longitude index</param>
        /// <param name="latIdx">latitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="timeIdx">level index</param>       
        /// <returns>one dimension data</returns>
        public GridData GetGridData_Level(int lonIdx, int latIdx, int varIdx, int timeIdx)
        {            
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            double[,] theData;
            Variable aPar = Variables[varIdx];
            int lNum = aPar.Levels.Count;
            int i;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = new double[lNum];
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, lNum];

            for (int l = 0; l < lNum; l++)
            {
                //Get time, parameter and level
                GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
                aMessageIdx.DateTime = Times[timeIdx];
                aMessageIdx.Parameter = Variables[varIdx];
                aMessageIdx.Level = aMessageIdx.Parameter.Levels[l];

                //Get the message position
                long aPosition = 0;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB1MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aPosition = mIdx.Position;
                        break;
                    }
                }

                //Read data
                br.BaseStream.Position = aPosition;
                GRIB1Message aMessage = new GRIB1Message(br, this);
                theData = GetDataArray(aMessage);
                double aValue = theData[latIdx, lonIdx];
                aGridData.X[l] = aPar.Levels[l];
                aGridData.Data[0, l] = aValue;                
            }

            br.Close();
            fs.Close();

            return aGridData;
        }

        /// <summary>
        /// Get ARL data - level
        /// </summary>
        /// <param name="lonIdx">longitude index</param>
        /// <param name="latIdx">latitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="timeIdx">level index</param>       
        /// <returns>one dimension data</returns>
        public List<PointD> GetOneDimData_Level(int lonIdx, int latIdx, int varIdx, int timeIdx)
        {
            PointD aPoint = new PointD();
            List<PointD> pointList = new List<PointD>();
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            double[,] theData;
            Variable aPar = Variables[varIdx];
            int lNum = aPar.Levels.Count;
            int i;

            for (int l = 0; l < lNum; l++)
            {
                //Get time, parameter and level
                GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
                aMessageIdx.DateTime = Times[timeIdx];
                aMessageIdx.Parameter = Variables[varIdx];
                aMessageIdx.Level = aMessageIdx.Parameter.Levels[l];
                    
                //Get the message position
                long aPosition = 0;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB1MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aPosition = mIdx.Position;
                        break;
                    }
                }

                //Read data
                br.BaseStream.Position = aPosition;
                GRIB1Message aMessage = new GRIB1Message(br, this);
                theData = GetDataArray(aMessage);
                double aValue = theData[latIdx, lonIdx];
                if (!(Math.Abs(aValue / MissingValue - 1) < 0.01))
                {
                    aPoint.X = aValue;
                    aPoint.Y = aPar.Levels[l];
                    pointList.Add(aPoint);
                }
            }

            br.Close();
            fs.Close();

            return pointList;
        }

        /// <summary>
        /// Get ARL data - longitude
        /// </summary>
        /// <param name="timeIdx">time index</param>
        /// <param name="latIdx">latitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>       
        /// <returns>one dimension data</returns>
        public GridData GetGridData_Lon(int timeIdx, int latIdx, int varIdx, int levelIdx)
        {            
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            double[,] theData;
            int i;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = X;
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, X.Length];

            //Get time, parameter and level
            GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
            aMessageIdx.DateTime = Times[timeIdx];
            aMessageIdx.Parameter = Variables[varIdx];
            if (aMessageIdx.Parameter.Levels.Count == 0)
                aMessageIdx.Level = 0;
            else
                aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

            //Get the message position
            long aPosition = 0;
            for (i = 0; i < MessageNumber; i++)
            {
                GRIB1MessageIndex mIdx = MessageIdxList[i];
                if (mIdx.Equals(aMessageIdx))
                {
                    aPosition = mIdx.Position;
                    break;
                }
            }

            //Read data
            br.BaseStream.Position = aPosition;
            GRIB1Message aMessage = new GRIB1Message(br, this);
            theData = GetDataArray(aMessage);
            for (i = 0; i < X.Length; i++)
            {
                double aValue = theData[latIdx, i];
                aGridData.Data[0, i] = aValue;                
            }

            br.Close();
            fs.Close();

            return aGridData;
        }

        /// <summary>
        /// Get ARL data - longitude
        /// </summary>
        /// <param name="timeIdx">time index</param>
        /// <param name="latIdx">latitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>       
        /// <returns>one dimension data</returns>
        public List<PointD> GetOneDimData_Lon(int timeIdx, int latIdx, int varIdx, int levelIdx)
        {
            PointD aPoint = new PointD();
            List<PointD> pointList = new List<PointD>();
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            double[,] theData;
            int i;

            //Get time, parameter and level
            GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
            aMessageIdx.DateTime = Times[timeIdx];
            aMessageIdx.Parameter = Variables[varIdx];
            if (aMessageIdx.Parameter.Levels.Count == 0)
                aMessageIdx.Level = 0;
            else
                aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

            //Get the message position
            long aPosition = 0;
            for (i = 0; i < MessageNumber; i++)
            {
                GRIB1MessageIndex mIdx = MessageIdxList[i];
                if (mIdx.Equals(aMessageIdx))
                {
                    aPosition = mIdx.Position;
                    break;
                }
            }

            //Read data
            br.BaseStream.Position = aPosition;
            GRIB1Message aMessage = new GRIB1Message(br, this);
            theData = GetDataArray(aMessage);
            for (i = 0; i < X.Length; i++)
            {
                double aValue = theData[latIdx, i];
                if (!(Math.Abs(aValue / MissingValue - 1) < 0.01))
                {
                    aPoint.X = X[i];
                    aPoint.Y = aValue;
                    pointList.Add(aPoint);
                }
            }

            br.Close();
            fs.Close();

            return pointList;
        }

        /// <summary>
        /// Get ARL data - latitude
        /// </summary>
        /// <param name="timeIdx">time index</param>
        /// <param name="lonIdx">longitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>       
        /// <returns>one dimension data</returns>
        public GridData GetGridData_Lat(int timeIdx, int lonIdx, int varIdx, int levelIdx)
        {            
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            double[,] theData;
            int i;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = Y;
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, Y.Length];

            //Get time, parameter and level
            GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
            aMessageIdx.DateTime = Times[timeIdx];
            aMessageIdx.Parameter = Variables[varIdx];
            if (aMessageIdx.Parameter.Levels.Count == 0)
                aMessageIdx.Level = 0;
            else
                aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

            //Get the message position
            long aPosition = 0;
            for (i = 0; i < MessageNumber; i++)
            {
                GRIB1MessageIndex mIdx = MessageIdxList[i];
                if (mIdx.Equals(aMessageIdx))
                {
                    aPosition = mIdx.Position;
                    break;
                }
            }

            //Read data
            br.BaseStream.Position = aPosition;
            GRIB1Message aMessage = new GRIB1Message(br, this);
            theData = GetDataArray(aMessage);
            for (i = 0; i < Y.Length; i++)
            {
                double aValue = theData[i, lonIdx];
                aGridData.Data[0, i] = aValue;                
            }

            br.Close();
            fs.Close();

            return aGridData;
        }

        /// <summary>
        /// Get ARL data - latitude
        /// </summary>
        /// <param name="timeIdx">time index</param>
        /// <param name="lonIdx">longitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>       
        /// <returns>one dimension data</returns>
        public List<PointD> GetOneDimData_Lat(int timeIdx, int lonIdx, int varIdx, int levelIdx)
        {
            PointD aPoint = new PointD();
            List<PointD> pointList = new List<PointD>();
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            double[,] theData;
            int i;

            //Get time, parameter and level
            GRIB1MessageIndex aMessageIdx = new GRIB1MessageIndex();
            aMessageIdx.DateTime = Times[timeIdx];
            aMessageIdx.Parameter = Variables[varIdx];
            if (aMessageIdx.Parameter.Levels.Count == 0)
                aMessageIdx.Level = 0;
            else
                aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

            //Get the message position
            long aPosition = 0;
            for (i = 0; i < MessageNumber; i++)
            {
                GRIB1MessageIndex mIdx = MessageIdxList[i];
                if (mIdx.Equals(aMessageIdx))
                {
                    aPosition = mIdx.Position;
                    break;
                }
            }

            //Read data
            br.BaseStream.Position = aPosition;
            GRIB1Message aMessage = new GRIB1Message(br, this);
            theData = GetDataArray(aMessage);
            for (i = 0; i < Y.Length; i++)
            {
                double aValue = theData[i, lonIdx];
                if (!(Math.Abs(aValue / MissingValue - 1) < 0.01))
                {
                    aPoint.X = Y[i];
                    aPoint.Y = aValue;
                    pointList.Add(aPoint);
                }
            }

            br.Close();
            fs.Close();

            return pointList;
        }

        #endregion
    }
}
