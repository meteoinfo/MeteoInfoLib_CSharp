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
    /// GRIB edition 2 data info
    /// </summary>
    public class GRIB2DataInfo : DataInfo,IGridDataInfo
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
        /// Message index list
        /// </summary>
        public List<GRIB2MessageIndex> MessageIdxList;
        /// <summary>
        /// Total message number
        /// </summary>
        public int MessageNumber;
        /// <summary>
        /// Header length
        /// </summary>
        public int HeaderLength = 0;

        private GRIB2ParameterTable _ParameterTable = new GRIB2ParameterTable();

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public GRIB2DataInfo()
        {
            IsLatLon = true;
            IsGlobal = false;
            MissingValue = -9999.0;
            TimeStartPos = new List<long>();
            //TimeList = new List<DateTime>();
            //ParameterList = new List<Variable>();
            MessageIdxList = new List<GRIB2MessageIndex>();
        }

        #endregion

        #region Properties
        

        #endregion

        #region Methods
        /// <summary>
        /// Read data info
        /// </summary>
        /// <param name="aFile">file path</param>
        public override void ReadDataInfo(string aFile)
        {
            FileName = aFile;
            FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            //Shift header
            br.BaseStream.Seek(HeaderLength, SeekOrigin.Begin);

            bool isNewTime = false;
            int recordNum = 0;
            int gridNum = 0;
            List<DateTime> times = new List<DateTime>();
            List<Variable> variables = new List<Variable>();
            while (br.BaseStream.Position < br.BaseStream.Length - 30)
            {                
                long messageStart = br.BaseStream.Position;
                GRIB2IndicatorSection rINS = new GRIB2IndicatorSection(br);
                messageStart += rINS.Shift;
                long messageEnd = messageStart + rINS.RecordLength;
                GRIB2IdentificationSection rIDS = new GRIB2IdentificationSection(br);

                while (ReadSectionNumber(br) != 8)
                {
                    int sectionNum = ReadSectionNumber(br);
                    GRIB2MessageIndex messageIdx = new GRIB2MessageIndex();
                    messageIdx.MessagePos = messageStart;
                    messageIdx.DataPos = br.BaseStream.Position;
                    messageIdx.StartSection = sectionNum;
                    if (sectionNum == 2)
                    {
                        GRIB2LocalUseSection rLUS = new GRIB2LocalUseSection(br);
                        GRIB2GridDefinitionSection rGDS = new GRIB2GridDefinitionSection(br);
                        if (gridNum == 0)
                        {
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
                        }
                        else
                        {
                            X = new double[1];
                            Y = new double[1];
                            rGDS.GetXYArray(ref X, ref Y);
                        }
                        gridNum += 1;
                    }
                    if (sectionNum == 3)
                    {
                        GRIB2GridDefinitionSection rGDS = new GRIB2GridDefinitionSection(br);
                        if (gridNum == 0)
                        {
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
                        }
                        else
                        {
                            X = new double[1];
                            Y = new double[1];
                            rGDS.GetXYArray(ref X, ref Y);
                        }
                        gridNum += 1;
                    }
                    GRIB2ProductDefinitionSection rPDS = new GRIB2ProductDefinitionSection(br);
                    SeekNextSction(br);    //Skip Data representation section
                    SeekNextSction(br);    //Skip Bitmap section
                    SeekNextSction(br);    //Skip Data section

                    recordNum += 1;

                    //Get parameter  
                    Variable bPar = rPDS.GetParameter(_ParameterTable, rINS.Discipline);
                    bPar.LevelType = rPDS.TypeFirstFixedSurface;
                    bPar.Name = bPar.Name + "@" + GRIB2Tables.getTypeSurfaceNameShort(bPar.LevelType);

                    //Set dimensions                    
                    DateTime theTime = rPDS.GetForecastTime(rIDS.BaseTime);
                    if (!times.Contains(theTime))
                    {
                        TimeStartPos.Add(br.BaseStream.Position - rINS.RecordLength);
                        times.Add(theTime);
                        if (times.Count > 1)
                            isNewTime = true;
                    }
                    
                    if (!isNewTime)
                    {
                        bool isAdd = true;
                        bool isEqual = false;
                        foreach (Variable aPar in variables)
                        {
                            if (aPar.TEquals(bPar))
                            {
                                //if (rPDS.TypeFirstFixedSurface != 1)
                                //{
                                //    aPar.AddLevel(rPDS.FirstFixedSurfaceValue);
                                //}
                                aPar.AddLevel(rPDS.FirstFixedSurfaceValue);
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
                                    //if (rPDS.TypeFirstFixedSurface != 1)
                                    //{
                                    //    aPar.AddLevel(rPDS.FirstFixedSurfaceValue);
                                    //}
                                    aPar.AddLevel(rPDS.FirstFixedSurfaceValue);
                                    isAdd = false;
                                    break;
                                }
                            }
                        }
                        if (isAdd)
                        {
                            //if (rPDS.TypeFirstFixedSurface != 1)
                            //    bPar.AddLevel(rPDS.FirstFixedSurfaceValue);
                            bPar.AddLevel(rPDS.FirstFixedSurfaceValue);
                            Dimension xDim = new Dimension(DimensionType.X);
                            xDim.SetValues(X);
                            Dimension yDim = new Dimension(DimensionType.Y);
                            yDim.SetValues(Y);
                            bPar.XDimension = xDim;
                            bPar.YDimension = yDim;
                            variables.Add(bPar);
                        }
                    }

                    //Set message index                                     
                    messageIdx.DateTime = theTime;
                    messageIdx.Level = rPDS.FirstFixedSurfaceValue;
                    messageIdx.Parameter = bPar;
                    MessageIdxList.Add(messageIdx);
                }

                br.BaseStream.Seek(messageEnd, SeekOrigin.Begin); 
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
                //var.SetDimension(YDimension);
                //var.SetDimension(XDimension);  
            }
            this.Variables = variables;

            br.Close();
            fs.Close();
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
                    "  Tsize = " + this.Times.Count.ToString();

            dataInfo += Environment.NewLine + "Number of Variables = " + this.Variables.Count.ToString();
            foreach (Variable v in this.Variables)
                dataInfo += Environment.NewLine + "\t" + v.Name + " " + v.LevelNum.ToString() + " " +
                        v.Units + " " + v.Description;

            dataInfo += Environment.NewLine + "Tsize = " + this.Times.Count.ToString();
            for (i = 0; i < this.Times.Count; i++)
                dataInfo += Environment.NewLine + "\t" + this.Times[i].ToString("yyyy-MM-dd HH:mm:ss");

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
            foreach (Variable aPar in this.Variables)
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
        public GridData GetGridData_LonLat(int timeIdx, int varIdx, int levelIdx)
        {
            GridData gridData = new GridData();
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i;

            //Get time, parameter and level
            GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
            aMessageIdx.DateTime = this.Times[timeIdx];
            Variable var = this.Variables[varIdx];
            aMessageIdx.Parameter = var;
            if (aMessageIdx.Parameter.Levels.Count == 0)
                aMessageIdx.Level = 0;
            else
                aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

            //Get the message position    
            bool isMatch = false;
            for (i = 0; i < MessageNumber; i++)
            {
                GRIB2MessageIndex mIdx = MessageIdxList[i];
                if (mIdx.Equals(aMessageIdx))
                {
                    aMessageIdx.MessagePos = mIdx.MessagePos;
                    aMessageIdx.StartSection = mIdx.StartSection;
                    aMessageIdx.DataPos = mIdx.DataPos;
                    isMatch = true;
                    break;
                }
            }

            if (!isMatch)
                return null;

            //Read data
            br.BaseStream.Position = aMessageIdx.MessagePos;
            GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
            gridData.MissingValue = MissingValue;            
            gridData.X = var.XDimension.GetValues();
            gridData.Y = var.YDimension.GetValues();            

            br.Close();
            fs.Close();

            if (aMessage.GribDS.Data == null)
                return null;

            gridData.Data = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, var.XDimension.DimLength, var.YDimension.DimLength);
            MessageNumber = MessageIdxList.Count;

            //Gaussian grid
            switch (aMessage.GribGDS.TemplateNum)
            {
                case 40:
                case 41:
                case 42:
                case 43:  // Gaussian latitude/longitude
                    //gridData.GassianToLatLon();
                    gridData.GassianToLatLon_Simple();
                    break;
            }

            return gridData;
        }

        private void SeekNextMessage(BinaryReader br)
        {
            GRIB2IndicatorSection rIS = new GRIB2IndicatorSection(br);
            br.BaseStream.Seek(rIS.RecordLength - rIS.Length, SeekOrigin.Current);
        }

        private int ReadSectionNumber(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(4);            
            if (ASCIIEncoding.ASCII.GetString(bytes) == "GRIB")
            {
                br.BaseStream.Seek(-4, SeekOrigin.Current);
                return 0;
            }
            else if (bytes[0] == '7' && bytes[1] == '7' && bytes[2] == '7' && bytes[3] == '7')
            {
                br.BaseStream.Seek(-4, SeekOrigin.Current);
                return 8;
            }
            else
            {
                int sectionNum = br.ReadByte();
                br.BaseStream.Seek(-5, SeekOrigin.Current);
                return sectionNum;
            }
        }

        private void SeekNextSction(BinaryReader br)
        {
            int length = Bytes2Number.Int4(br);
            br.BaseStream.Seek(length - 4, SeekOrigin.Current);
        }

        private double[,] GetDataArray(GRIB2DataSection gribDS, int scanMode, int xNum, int yNum)
        {
            double[,] data = new double[yNum, xNum];
            for (int i = 0; i < yNum; i++)
            {
                for (int j = 0; j < xNum; j++)
                {
                    if ((scanMode & 64) == 0)    //-y
                        data[i, j] = gribDS.Data[(yNum - i - 1) * xNum + j];
                    else
                        data[i, j] = gribDS.Data[i * xNum + j];

                    if (double.IsNaN(data[i, j]))
                        data[i, j] = MissingValue;
                }
            }

            return data;
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
            Variable var = this.Variables[varIdx];
            int xNum, yNum, tNum, t;
            xNum = var.XDimension.DimLength;
            yNum = var.YDimension.DimLength;
            tNum = this.Times.Count;
            double[,] theData = new double[yNum, xNum];
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            double[,] newGridData = new double[tNum, xNum];
            int i, j;            

            for (t = 0; t < tNum; t++)
            {
                //Get time, parameter and level
                GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
                aMessageIdx.DateTime = this.Times[t];
                aMessageIdx.Parameter = var;
                if (aMessageIdx.Parameter.Levels.Count == 0)
                    aMessageIdx.Level = 0;
                else
                    aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

                //Get the message position    
                bool isMatch = false;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB2MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aMessageIdx.MessagePos = mIdx.MessagePos;
                        aMessageIdx.StartSection = mIdx.StartSection;
                        aMessageIdx.DataPos = mIdx.DataPos;
                        isMatch = true;
                        break;
                    }
                }

                if (!isMatch)
                    return null;

                //Read data
                br.BaseStream.Position = aMessageIdx.MessagePos;
                GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
                theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, xNum, yNum);
                for (j = 0; j < xNum; j++)
                {
                    newGridData[t, j] = theData[latIdx, j];
                }
            }

            br.Close();
            fs.Close();

            double[] newY = new double[this.Times.Count];
            for (i = 0; i < this.Times.Count; i++)
                newY[i] = DataConvert.ToDouble(this.Times[i]);

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
            Variable var = this.Variables[varIdx];
            int xNum, yNum, tNum, t;
            xNum = var.XDimension.DimLength;
            yNum = var.YDimension.DimLength;
            tNum = this.Times.Count;
            double[,] theData = new double[yNum, xNum];
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            double[,] newGridData = new double[tNum, yNum];
            int i;

            for (t = 0; t < tNum; t++)
            {
                //Get time, parameter and level
                GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
                aMessageIdx.DateTime = this.Times[t];
                aMessageIdx.Parameter = var;
                if (aMessageIdx.Parameter.Levels.Count == 0)
                    aMessageIdx.Level = 0;
                else
                    aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

                //Get the message position    
                bool isMatch = false;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB2MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aMessageIdx.MessagePos = mIdx.MessagePos;
                        aMessageIdx.StartSection = mIdx.StartSection;
                        aMessageIdx.DataPos = mIdx.DataPos;
                        isMatch = true;
                        break;
                    }
                }

                if (!isMatch)
                {
                    br.Close();
                    fs.Close();
                    return null;
                }

                //Read data
                br.BaseStream.Position = aMessageIdx.MessagePos;
                GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
                theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, xNum, yNum);
                for (i = 0; i < yNum; i++)
                {
                    newGridData[t, i] = theData[i, lonIdx];
                }
            }

            br.Close();
            fs.Close();

            double[] newY = new double[this.Times.Count];
            for (i = 0; i < this.Times.Count; i++)
                newY[i] = DataConvert.ToDouble(this.Times[i]);

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
            Variable var = this.Variables[varIdx];
            int xNum, yNum, lNum;
            xNum = var.XDimension.DimLength;
            yNum = var.YDimension.DimLength;
            double[,] theData = new double[yNum, xNum];
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            lNum = var.Levels.Count;
            double[,] newGridData = new double[lNum, yNum];
            int i;

            for (int l = 0; l < var.Levels.Count; l++)
            {
                //Get time, parameter and level
                GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
                aMessageIdx.DateTime = this.Times[tIdx];
                aMessageIdx.Parameter = var;
                if (aMessageIdx.Parameter.Levels.Count == 0)
                    aMessageIdx.Level = 0;
                else
                    aMessageIdx.Level = aMessageIdx.Parameter.Levels[l];

                //Get the message position    
                bool isMatch = false;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB2MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aMessageIdx.MessagePos = mIdx.MessagePos;
                        aMessageIdx.StartSection = mIdx.StartSection;
                        aMessageIdx.DataPos = mIdx.DataPos;
                        isMatch = true;
                        break;
                    }
                }

                if (!isMatch)
                {
                    br.Close();
                    fs.Close();
                    return null;
                }

                //Read data
                br.BaseStream.Position = aMessageIdx.MessagePos;
                GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
                theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, xNum, yNum);
                for (i = 0; i < yNum; i++)
                {
                    newGridData[l, i] = theData[i, lonIdx];
                }
            }

            br.Close();
            fs.Close();

            double[] newY = new double[lNum];
            for (i = 0; i < lNum; i++)
                newY[i] = var.Levels[i];

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
            Variable var = this.Variables[varIdx];
            int xNum, yNum, lNum;
            xNum = var.XDimension.DimLength;
            yNum = var.YDimension.DimLength;
            double[,] theData = new double[yNum, xNum];
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            lNum = var.Levels.Count;
            double[,] newGridData = new double[lNum, xNum];
            int i;

            for (int l = 0; l < var.Levels.Count; l++)
            {
                //Get time, parameter and level
                GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
                aMessageIdx.DateTime = this.Times[tIdx];
                aMessageIdx.Parameter = var;
                if (aMessageIdx.Parameter.Levels.Count == 0)
                    aMessageIdx.Level = 0;
                else
                    aMessageIdx.Level = aMessageIdx.Parameter.Levels[l];

                //Get the message position    
                bool isMatch = false;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB2MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aMessageIdx.MessagePos = mIdx.MessagePos;
                        aMessageIdx.StartSection = mIdx.StartSection;
                        aMessageIdx.DataPos = mIdx.DataPos;
                        isMatch = true;
                        break;
                    }
                }

                if (!isMatch)
                {
                    br.Close();
                    fs.Close();
                    return null;
                }

                //Read data
                br.BaseStream.Position = aMessageIdx.MessagePos;
                GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
                theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode,xNum, yNum);
                for (i = 0; i < xNum; i++)
                {
                    newGridData[l, i] = theData[latIdx, i];
                }
            }

            br.Close();
            fs.Close();

            double[] newY = new double[lNum];
            for (i = 0; i < lNum; i++)
                newY[i] = var.Levels[i];

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
            Variable var = this.Variables[varIdx];
            int xNum, yNum, lNum, tNum;
            xNum = var.XDimension.DimLength;
            yNum = var.YDimension.DimLength;
            tNum = this.Times.Count;
            double[,] theData = new double[yNum, xNum];
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            lNum = var.Levels.Count;
            double[,] newGridData = new double[lNum, tNum];
            int i;

            for (int l = 0; l < var.Levels.Count; l++)
            {
                for (int t = 0; t < this.Times.Count; t++)
                {
                    //Get time, parameter and level
                    GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
                    aMessageIdx.DateTime = this.Times[t];
                    aMessageIdx.Parameter = var;
                    if (aMessageIdx.Parameter.Levels.Count == 0)
                        aMessageIdx.Level = 0;
                    else
                        aMessageIdx.Level = aMessageIdx.Parameter.Levels[l];

                    //Get the message position    
                    bool isMatch = false;
                    for (i = 0; i < MessageNumber; i++)
                    {
                        GRIB2MessageIndex mIdx = MessageIdxList[i];
                        if (mIdx.Equals(aMessageIdx))
                        {
                            aMessageIdx.MessagePos = mIdx.MessagePos;
                            aMessageIdx.StartSection = mIdx.StartSection;
                            aMessageIdx.DataPos = mIdx.DataPos;
                            isMatch = true;
                            break;
                        }
                    }

                    if (!isMatch)
                    {
                        br.Close();
                        fs.Close();
                        return null;
                    }

                    //Read data
                    br.BaseStream.Position = aMessageIdx.MessagePos;
                    GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
                    theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, xNum, yNum);
                    newGridData[l, t] = theData[latIdx, lonIdx];
                }
            }

            br.Close();
            fs.Close();

            double[] newX = new double[this.Times.Count];
            for (i = 0; i < this.Times.Count; i++)
                newX[i] = DataConvert.ToDouble(this.Times[i]);

            double[] newY = new double[lNum];
            for (i = 0; i < lNum; i++)
                newY[i] = var.Levels[i];

            gridData.MissingValue = MissingValue;            
            gridData.X = newX;
            gridData.Y = newY;
            gridData.Data = newGridData;

            return gridData;
        }

        /// <summary>
        /// Get ARL data - Time
        /// </summary>
        /// <param name="lonIdx">longitude index</param>
        /// <param name="latIdx">latitude index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>       
        /// <returns>one dimension data</returns>
        public GridData GetGridData_Time(int lonIdx, int latIdx, int varIdx, int levelIdx)
        {            
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            double[,] theData;
            int i;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = new double[this.Times.Count];
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, this.Times.Count];

            Variable var = this.Variables[varIdx];
            int xNum = var.XDimension.DimLength;
            int yNum = var.YDimension.DimLength;
            for (int t = 0; t < this.Times.Count; t++)
            {
                //Get time, parameter and level
                GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
                aMessageIdx.DateTime = this.Times[t];
                aMessageIdx.Parameter = var;
                if (aMessageIdx.Parameter.Levels.Count == 0)
                    aMessageIdx.Level = 0;
                else
                    aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

                //Get the message position    
                bool isMatch = false;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB2MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aMessageIdx.MessagePos = mIdx.MessagePos;
                        aMessageIdx.StartSection = mIdx.StartSection;
                        aMessageIdx.DataPos = mIdx.DataPos;
                        isMatch = true;
                        break;
                    }
                }

                if (!isMatch)
                {
                    br.Close();
                    fs.Close();
                    return null;
                }

                //Read data
                br.BaseStream.Position = aMessageIdx.MessagePos;
                GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
                theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, xNum, yNum);
                double aValue = theData[latIdx, lonIdx];
                aGridData.X[t] = DataConvert.ToDouble(this.Times[t]);
                aGridData.Data[0, t] = aValue;                
            }

            br.Close();
            fs.Close();

            return aGridData;
        }

        /// <summary>
        /// Get ARL data - Time
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

            Variable var = this.Variables[varIdx];
            int xNum = var.XDimension.DimLength;
            int yNum = var.YDimension.DimLength;
            for (int t = 0; t < this.Times.Count; t++)
            {
                //Get time, parameter and level
                GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
                aMessageIdx.DateTime = this.Times[t];
                aMessageIdx.Parameter = var;
                if (aMessageIdx.Parameter.Levels.Count == 0)
                    aMessageIdx.Level = 0;
                else
                    aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

                //Get the message position    
                bool isMatch = false;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB2MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aMessageIdx.MessagePos = mIdx.MessagePos;
                        aMessageIdx.StartSection = mIdx.StartSection;
                        aMessageIdx.DataPos = mIdx.DataPos;
                        isMatch = true;
                        break;
                    }
                }

                if (!isMatch)
                {
                    br.Close();
                    fs.Close();
                    return null;
                }

                //Read data
                br.BaseStream.Position = aMessageIdx.MessagePos;
                GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
                theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, xNum, yNum);
                double aValue = theData[latIdx, lonIdx];
                if (!(Math.Abs(aValue / MissingValue - 1) < 0.01))
                {
                    aPoint.X = this.Times[t].ToBinary();
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
            Variable aPar = this.Variables[varIdx];
            int lNum = aPar.Levels.Count;
            int i;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = new double[lNum];
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, lNum];

            Variable var = this.Variables[varIdx];
            int xNum = var.XDimension.DimLength;
            int yNum = var.YDimension.DimLength;
            for (int l = 0; l < lNum; l++)
            {
                //Get time, parameter and level
                GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
                aMessageIdx.DateTime = this.Times[timeIdx];
                aMessageIdx.Parameter = var;
                if (aMessageIdx.Parameter.Levels.Count == 0)
                    aMessageIdx.Level = 0;
                else
                    aMessageIdx.Level = aMessageIdx.Parameter.Levels[l];

                //Get the message position    
                bool isMatch = false;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB2MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aMessageIdx.MessagePos = mIdx.MessagePos;
                        aMessageIdx.StartSection = mIdx.StartSection;
                        aMessageIdx.DataPos = mIdx.DataPos;
                        isMatch = true;
                        break;
                    }
                }

                if (!isMatch)
                {
                    br.Close();
                    fs.Close();
                    return null;
                }

                //Read data
                br.BaseStream.Position = aMessageIdx.MessagePos;
                GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
                theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, xNum, yNum);
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
            Variable var = this.Variables[varIdx];
            int xNum = var.XDimension.DimLength;
            int yNum = var.YDimension.DimLength;
            int lNum = var.Levels.Count;
            int i;

            for (int l = 0; l < lNum; l++)
            {
                //Get time, parameter and level
                GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
                aMessageIdx.DateTime = this.Times[timeIdx];
                aMessageIdx.Parameter = var;
                if (aMessageIdx.Parameter.Levels.Count == 0)
                    aMessageIdx.Level = 0;
                else
                    aMessageIdx.Level = aMessageIdx.Parameter.Levels[l];

                //Get the message position    
                bool isMatch = false;
                for (i = 0; i < MessageNumber; i++)
                {
                    GRIB2MessageIndex mIdx = MessageIdxList[i];
                    if (mIdx.Equals(aMessageIdx))
                    {
                        aMessageIdx.MessagePos = mIdx.MessagePos;
                        aMessageIdx.StartSection = mIdx.StartSection;
                        aMessageIdx.DataPos = mIdx.DataPos;
                        isMatch = true;
                        break;
                    }
                }

                if (!isMatch)
                {
                    br.Close();
                    fs.Close();
                    return null;
                }

                //Read data
                br.BaseStream.Position = aMessageIdx.MessagePos;
                GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
                theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, xNum, yNum);
                double aValue = theData[latIdx, lonIdx];
                if (!(Math.Abs(aValue / MissingValue - 1) < 0.01))
                {
                    aPoint.X = aValue;
                    aPoint.Y = var.Levels[l];
                    pointList.Add(aPoint);
                }
            }

            br.Close();
            fs.Close();

            return pointList;
        }

        /// <summary>
        /// Get grid data - longitude
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

            Variable var = this.Variables[varIdx];
            int xNum = var.XDimension.DimLength;
            int yNum = var.YDimension.DimLength;
            //Get time, parameter and level
            GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
            aMessageIdx.DateTime = this.Times[timeIdx];
            aMessageIdx.Parameter = var;
            if (aMessageIdx.Parameter.Levels.Count == 0)
                aMessageIdx.Level = 0;
            else
                aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

            //Get the message position    
            bool isMatch = false;
            for (i = 0; i < MessageNumber; i++)
            {
                GRIB2MessageIndex mIdx = MessageIdxList[i];
                if (mIdx.Equals(aMessageIdx))
                {
                    aMessageIdx.MessagePos = mIdx.MessagePos;
                    aMessageIdx.StartSection = mIdx.StartSection;
                    aMessageIdx.DataPos = mIdx.DataPos;
                    isMatch = true;
                    break;
                }
            }

            if (!isMatch)
            {
                br.Close();
                fs.Close();                    
                return null;
            }

            //Read data
            br.BaseStream.Position = aMessageIdx.MessagePos;
            GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
            theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, xNum, yNum);
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

            Variable var = this.Variables[varIdx];
            int xNum = var.XDimension.DimLength;
            int yNum = var.YDimension.DimLength;

            //Get time, parameter and level
            GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
            aMessageIdx.DateTime = this.Times[timeIdx];
            aMessageIdx.Parameter = var;
            if (aMessageIdx.Parameter.Levels.Count == 0)
                aMessageIdx.Level = 0;
            else
                aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

            //Get the message position    
            bool isMatch = false;
            for (i = 0; i < MessageNumber; i++)
            {
                GRIB2MessageIndex mIdx = MessageIdxList[i];
                if (mIdx.Equals(aMessageIdx))
                {
                    aMessageIdx.MessagePos = mIdx.MessagePos;
                    aMessageIdx.StartSection = mIdx.StartSection;
                    aMessageIdx.DataPos = mIdx.DataPos;
                    isMatch = true;
                    break;
                }
            }

            if (!isMatch)
            {
                br.Close();
                fs.Close();
                return null;
            }

            //Read data
            br.BaseStream.Position = aMessageIdx.MessagePos;
            GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
            theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, xNum, yNum);
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
        /// Get grid data - latitude
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

            Variable var = this.Variables[varIdx];
            int xNum = var.XDimension.DimLength;
            int yNum = var.YDimension.DimLength;

            //Get time, parameter and level
            GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
            aMessageIdx.DateTime = this.Times[timeIdx];
            aMessageIdx.Parameter = var;
            if (aMessageIdx.Parameter.Levels.Count == 0)
                aMessageIdx.Level = 0;
            else
                aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

            //Get the message position    
            bool isMatch = false;
            for (i = 0; i < MessageNumber; i++)
            {
                GRIB2MessageIndex mIdx = MessageIdxList[i];
                if (mIdx.Equals(aMessageIdx))
                {
                    aMessageIdx.MessagePos = mIdx.MessagePos;
                    aMessageIdx.StartSection = mIdx.StartSection;
                    aMessageIdx.DataPos = mIdx.DataPos;
                    isMatch = true;
                    break;
                }
            }

            if (!isMatch)
            {
                br.Close();
                fs.Close();
                return null;
            }

            //Read data
            br.BaseStream.Position = aMessageIdx.MessagePos;
            GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
            theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, xNum, yNum);
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

            Variable var = this.Variables[varIdx];
            int xNum = var.XDimension.DimLength;
            int yNum = var.YDimension.DimLength;

            //Get time, parameter and level
            GRIB2MessageIndex aMessageIdx = new GRIB2MessageIndex();
            aMessageIdx.DateTime = this.Times[timeIdx];
            aMessageIdx.Parameter = var;
            if (aMessageIdx.Parameter.Levels.Count == 0)
                aMessageIdx.Level = 0;
            else
                aMessageIdx.Level = aMessageIdx.Parameter.Levels[levelIdx];

            //Get the message position    
            bool isMatch = false;
            for (i = 0; i < MessageNumber; i++)
            {
                GRIB2MessageIndex mIdx = MessageIdxList[i];
                if (mIdx.Equals(aMessageIdx))
                {
                    aMessageIdx.MessagePos = mIdx.MessagePos;
                    aMessageIdx.StartSection = mIdx.StartSection;
                    aMessageIdx.DataPos = mIdx.DataPos;
                    isMatch = true;
                    break;
                }
            }

            if (!isMatch)
            {
                br.Close();
                fs.Close();
                return null;
            }

            //Read data
            br.BaseStream.Position = aMessageIdx.MessagePos;
            GRIB2Message aMessage = new GRIB2Message(br, aMessageIdx.StartSection, aMessageIdx.DataPos);
            theData = GetDataArray(aMessage.GribDS, aMessage.GribGDS.ScanMode, xNum, yNum);
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
