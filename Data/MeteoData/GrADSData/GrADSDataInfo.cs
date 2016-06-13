using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using MeteoInfoC.Global;
using MeteoInfoC.Projections;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GrADS data info
    /// </summary>
    public class GrADSDataInfo : DataInfo,IGridDataInfo,IStationDataInfo
    {
        #region Variables

        private FileStream _fs = null;
        private BinaryWriter _bw = null;

        ///// <summary>
        ///// Control file name
        ///// </summary>
        //public string FileName;
        /// <summary>
        /// Descriptor
        /// </summary>
        public string DESCRIPTOR;
        /// <summary>
        /// Data file name
        /// </summary>
        public string DSET;
        /// <summary>
        /// Is Lat/Lon
        /// </summary>
        public Boolean isLatLon;        
        ///// <summary>
        ///// Projection info
        ///// </summary>
        //public ProjectionInfo ProjInfo;
        /// <summary>
        /// If rotate vector
        /// </summary>
        public bool EarthWind;
        /// <summary>
        /// Data type
        /// </summary>
        public string DTYPE;
        /// <summary>
        /// Options
        /// </summary>
        public Options OPTIONS;
        /// <summary>
        /// Title
        /// </summary>
        public string TITLE;
        /// <summary>
        /// Projection set
        /// </summary>
        public PDEFS PDEF;
        /// <summary>
        /// X set
        /// </summary>
        public XDEFS XDEF = new XDEFS();
        /// <summary>
        /// Y set
        /// </summary>
        public YDEFS YDEF = new YDEFS();
        /// <summary>
        /// Level set
        /// </summary>
        public ZDEFS ZDEF = new ZDEFS();
        /// <summary>
        /// Time set
        /// </summary>
        public TDEFS TDEF = new TDEFS();
        /// <summary>
        /// Variable set
        /// </summary>
        public VARDEFS VARDEF = new VARDEFS();
        /// <summary>
        /// A header record of length bytes that precedes the data
        /// </summary>
        public int FILEHEADER;
        /// <summary>
        /// A header record of length bytes preceding each time block of binary data
        /// </summary>
        public int THEADER;
        /// <summary>
        /// A header record of length bytes preceding each horizontal grid (XY block) of binary data
        /// </summary>
        public int XYHEADER;
        /// <summary>
        /// Is global
        /// </summary>
        public bool isGlobal;
        private int _byteNum = 4;
        /// <summary>
        /// Record length in bytes for x/y varying grid
        /// </summary>
        private int _recordLen;
        /// <summary>
        /// Record length per time
        /// </summary>
        private int _recLenPerTime;
        /// <summary>
        /// X coordinate
        /// </summary>
        public double[] X;
        /// <summary>
        /// Y coordinate
        /// </summary>
        public double[] Y;
        /// <summary>
        /// X coordinate number
        /// </summary>
        public int XNum;
        /// <summary>
        /// Y coordinate number
        /// </summary>
        public int YNum;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public GrADSDataInfo()
        {
            DTYPE = "Gridded";
            MissingValue = -9999.0;
            TITLE = "";
            OPTIONS = new Options();
            FILEHEADER = 0;
            THEADER = 0;
            XYHEADER = 0;
            isGlobal = false;
            isLatLon = true;            
            PDEF = new PDEFS();
            //ProjInfo = KnownCoordinateSystems.Geographic.World.WGS1984;
            EarthWind = true;
        }

        #endregion

        #region Properties
        ///// <summary>
        ///// Get variable list
        ///// </summary>
        //public List<string> Variables
        //{
        //    get 
        //    {
        //        List<string> varList = new List<string>();
        //        foreach (VAR aVar in VARDEF.Vars)
        //        {
        //            varList.Add(aVar.VName);
        //        }

        //        return varList;
        //    }
        //}

        /// <summary>
        /// Get variable list they have upper levels
        /// </summary>
        public List<Variable> UpperVariables
        {
            get
            {
                List<Variable> uVarList = new List<Variable>();
                foreach (Variable aVar in VARDEF.Vars)
                {
                    if (aVar.LevelNum > 1)
                        uVarList.Add(aVar);
                }

                return uVarList;
            }
        }

        /// <summary>
        /// Get variable name list they have upper levels
        /// </summary>
        public List<string> UpperVariableNames
        {
            get
            {
                List<string> uVarList = new List<string>();
                foreach (Variable aVar in VARDEF.Vars)
                {
                    if (aVar.LevelNum > 1)
                        uVarList.Add(aVar.Name);
                }

                return uVarList;
            }
        }

        ///// <summary>
        ///// Get time list
        ///// </summary>
        //public List<DateTime> Times
        //{
        //    get
        //    {
        //        List<DateTime> times = new List<DateTime>();
        //        for (int i = 0; i < TDEF.TNum; i++)
        //            times.Add(TDEF.times[i]);

        //        return times;
        //    }
        //}

        #endregion

        #region Methods
        #region Read and write data
        /// <summary>
        /// Read GrADS data info
        /// </summary>
        /// <param name="aFile">ctl file path</param>
        /// <returns>is correct</returns>
        public override void ReadDataInfo(string aFile)
        {
            string eStr = string.Empty;
            ReadDataInfo(aFile, ref eStr);
            List<DateTime> times = new List<DateTime>();
            int i;
            for (i = 0; i < TDEF.TNum; i++)
            {
                times.Add(TDEF.times[i]);
            }
            this.Times = times;
            this.Variables = VARDEF.Vars;
        }

        /// <summary>
        /// Read GrADS ctl file
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="errorStr"></param>
        /// <returns></returns>
        public bool ReadDataInfo(string aFile, ref string errorStr)
        {
            FileName = aFile;
            StreamReader sr = new StreamReader(aFile, Encoding.Default);
            string aLine;
            string[] dataArray;
            int i, j, LastNonEmpty;
            //ArrayList dataList = new ArrayList();
            List<string> dataList = new List<string>();
            Boolean isEnd = false;

            //Set dufault value
            DESCRIPTOR = aFile;

            do
            {
                aLine = sr.ReadLine().Trim();
                if (aLine == string.Empty)
                {
                    continue;
                }
                dataArray = aLine.Split();
                LastNonEmpty = -1;
                dataList.Clear();
                for (i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] != string.Empty)
                    {
                        LastNonEmpty++;
                        dataList.Add(dataArray[i]);
                    }
                }
                switch ((dataList[0]).ToUpper())
                {
                    case "DSET":
                        DSET = (string)dataList[1];
                        bool isNotPath = false;
                        if (!DSET.Contains('/') && !DSET.Contains('\\'))
                            isNotPath = true;

                        if (DSET.Contains('%'))
                        {
                            OPTIONS.template = true;
                        }

                        string aDir;
                        if (Path.GetDirectoryName(aFile) != "")
                            aDir = Path.GetDirectoryName(aFile);
                        else
                            aDir = Environment.CurrentDirectory;

                        if (isNotPath)
                        {                            
                            if (DSET.Substring(0, 1) == "^")
                                DSET = Path.Combine(aDir, DSET.Substring(1));
                            else
                                DSET = Path.Combine(aDir, DSET);

                            //if (!File.Exists(DSET))
                            //{
                            //    DSET = (string)dataList[1];
                            //    DSET = Path.GetDirectoryName(aFile) + "/" +
                            //        DSET.Substring(1, DSET.Length - 1);
                            //}
                        }

                        if (!OPTIONS.template)
                        {
                            if (!File.Exists(DSET))
                            {
                                if (DSET.Substring(0, 2) == "./" || DSET.Substring(0, 2) == ".\\")
                                    DSET = Path.Combine(aDir, DSET.Substring(2));
                                else
                                    errorStr = "The data file is not exist!" + Environment.NewLine + DSET;
                                //goto ERROR;
                            }
                        }
                        break;
                    case "DTYPE":
                        DTYPE = (string)dataList[1];
                        if (DTYPE.ToUpper() != "GRIDDED" && DTYPE.ToUpper() != "STATION")
                        {
                            errorStr = "The data type is not supported at present!" + Environment.NewLine
                                + DTYPE;
                            goto ERROR;
                        }
                        break;
                    case "OPTIONS":
                        for (i = 1; i < dataList.Count; i++)
                        {
                            switch (dataList[i].ToLower())
                            {
                                case "big_endian":
                                    OPTIONS.big_endian = true;
                                    break;
                                case "byteswapped":
                                    OPTIONS.byteswapped = true;
                                    break;
                                case "365_day_calendar":
                                    OPTIONS.calendar_365_day = true;
                                    break;
                                case "cray_32bit_ieee":
                                    OPTIONS.cray_32bit_ieee = true;
                                    break;
                                case "little_endian":
                                    OPTIONS.little_endian = true;
                                    break;
                                case "pascals":
                                    OPTIONS.pascals = true;
                                    break;
                                case "sequential":
                                    OPTIONS.sequential = true;
                                    break;
                                case "template":
                                    OPTIONS.template = true;
                                    break;
                                case "yrev":
                                    OPTIONS.yrev = true;
                                    break;
                                case "zrev":
                                    OPTIONS.zrev = true;
                                    break;
                            }
                        }
                        break;
                    case "UNDEF":
                        MissingValue = Convert.ToDouble(dataList[1]);
                        break;
                    case "TITLE":
                        TITLE = aLine.Substring(5, aLine.Length - 5).Trim();
                        break;
                    case "FILEHEADER":
                        FILEHEADER = int.Parse(dataList[1]);
                        break;
                    case "THEADER":
                        THEADER = int.Parse(dataList[1]);
                        break;
                    case "XYHEADER":
                        XYHEADER = int.Parse(dataList[1]);
                        break;
                    case "PDEF":
                        PDEF.PDEF_Type = dataList[3].ToUpper();
                        string ProjStr;
                        ProjectionInfo theProj = new ProjectionInfo();
                        switch (PDEF.PDEF_Type)
                        {
                            case "LCC":
                            case "LCCR":
                                PDEF_LCC aPLCC = new PDEF_LCC();
                                aPLCC.isize = int.Parse(dataList[1]);
                                aPLCC.jsize = int.Parse(dataList[2]);
                                aPLCC.latref = Single.Parse(dataList[4]);
                                aPLCC.lonref = Single.Parse(dataList[5]);
                                aPLCC.iref = Single.Parse(dataList[6]);
                                aPLCC.jref = Single.Parse(dataList[7]);
                                aPLCC.Struelat = Single.Parse(dataList[8]);
                                aPLCC.Ntruelat = Single.Parse(dataList[9]);
                                aPLCC.slon = Single.Parse(dataList[10]);
                                aPLCC.dx = Single.Parse(dataList[11]);
                                aPLCC.dy = Single.Parse(dataList[12]);
                                PDEF.PDEF_Content = aPLCC;

                                isLatLon = false;                                

                                ProjStr = "+proj=lcc" +
                                        "+lat_1=" + aPLCC.Struelat.ToString() +
                                        "+lat_2=" + aPLCC.Ntruelat.ToString() +
                                        "+lat_0=" + aPLCC.latref.ToString() +
                                        "+lon_0=" + aPLCC.slon.ToString();


                                theProj = new ProjectionInfo(ProjStr);
                                ProjectionInfo = theProj;
                                if (PDEF.PDEF_Type == "LCCR")
                                    EarthWind = false;

                                //Set X Y
                                XNum = aPLCC.isize;
                                YNum = aPLCC.jsize;
                                X = new double[aPLCC.isize];
                                Y = new double[aPLCC.jsize];
                                GetProjectedXY(theProj, aPLCC.dx, aPLCC.dy, aPLCC.iref, aPLCC.jref, aPLCC.lonref,
                                    aPLCC.latref, ref X, ref Y);
                                break;
                            case "NPS":
                            case "SPS":
                                int iSize = int.Parse(dataList[1]);
                                int jSize = int.Parse(dataList[2]);
                                float iPole = float.Parse(dataList[3]);
                                float jPole = float.Parse(dataList[4]);
                                float lonRef = float.Parse(dataList[5]);
                                float dx = float.Parse(dataList[6]) * 1000;
                                float dy = dx;

                                string lat0 = "90";
                                if (PDEF.PDEF_Type == "SPS")
                                    lat0 = "-90";

                                isLatLon = false;

                                ProjStr = "+proj=stere+lon_0=" + lonRef.ToString() +
                                          "+lat_0=" + lat0;

                                this.ProjectionInfo = new ProjectionInfo(ProjStr);

                                //Set X Y
                                XNum = iSize;
                                YNum = jSize;
                                X = new double[iSize];
                                Y = new double[jSize];
                                GetProjectedXY_NPS(dx, dy, iPole, jPole, ref X, ref Y);
                                break;
                            default:
                                errorStr = "The PDEF type is not supported at present!" + Environment.NewLine +
                                    "Please send your data to the author to improve MeteoInfo!";
                                goto ERROR;
                        }
                        break;
                    case "XDEF":
                        XDEF.XNum = Convert.ToInt32(dataList[1]);
                        XDEF.X = new double[XDEF.XNum];
                        XDEF.Type = (string)dataList[2];
                        List<double> values = new List<double>();
                        if (XDEF.Type.ToUpper() == "LINEAR")
                        {
                            XDEF.XMin = Convert.ToSingle(dataList[3]);
                            XDEF.XDelt = Convert.ToSingle(dataList[4]);                            
                        }
                        else
                        {
                            if (dataList.Count < XDEF.XNum + 3)
                            {
                                while (true)
                                {
                                    aLine = aLine + " " + sr.ReadLine().Trim();
                                    if (aLine == string.Empty)
                                    {
                                        continue;
                                    }
                                    dataArray = aLine.Split();
                                    LastNonEmpty = -1;
                                    dataList.Clear();
                                    for (i = 0; i < dataArray.Length; i++)
                                    {
                                        if (dataArray[i] != string.Empty)
                                        {
                                            LastNonEmpty++;
                                            dataList.Add(dataArray[i]);
                                        }
                                    }
                                    if (dataList.Count >= XDEF.XNum + 3)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (dataList.Count > XDEF.XNum + 3)
                            {
                                errorStr = "XDEF is wrong! Please check the ctl file!";
                                goto ERROR;
                            }
                            XDEF.XMin = Convert.ToSingle(dataList[3]);
                            float xmax = float.Parse(dataList[dataList.Count - 1]);
                            XDEF.XDelt = (xmax - XDEF.XMin) / (XDEF.XNum - 1);
                        }
                        for (i = 0; i < XDEF.XNum; i++)
                        {
                            XDEF.X[i] = XDEF.XMin + i * XDEF.XDelt;
                            values.Add((double)XDEF.XMin + i * XDEF.XDelt);
                        }
                        if (XDEF.XMin == 0 && XDEF.X[XDEF.XNum - 1]
                            + XDEF.XDelt == 360)
                        {
                            isGlobal = true;
                        }
                        Dimension xDim = new Dimension(DimensionType.X);
                        xDim.SetValues(values);
                        this.XDimension = xDim;
                        break;
                    case "YDEF":
                        YDEF.YNum = Convert.ToInt32(dataList[1]);
                        YDEF.Y = new double[YDEF.YNum];
                        YDEF.Type = (string)dataList[2];
                        values = new List<double>();
                        if (YDEF.Type.ToUpper() == "LINEAR")
                        {
                            YDEF.YMin = Convert.ToSingle(dataList[3]);
                            YDEF.YDelt = Convert.ToSingle(dataList[4]);                            
                        }
                        else
                        {
                            if (dataList.Count < YDEF.YNum + 3)
                            {
                                while (true)
                                {
                                    aLine = aLine + " " + sr.ReadLine().Trim();
                                    if (aLine == string.Empty)
                                    {
                                        continue;
                                    }
                                    dataArray = aLine.Split();
                                    LastNonEmpty = -1;
                                    dataList.Clear();
                                    for (i = 0; i < dataArray.Length; i++)
                                    {
                                        if (dataArray[i] != string.Empty)
                                        {
                                            LastNonEmpty++;
                                            dataList.Add(dataArray[i]);
                                        }
                                    }
                                    if (dataList.Count >= YDEF.YNum + 3)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (dataList.Count > YDEF.YNum + 3)
                            {
                                errorStr = "YDEF is wrong! Please check the ctl file!";
                                goto ERROR;
                            }
                            YDEF.YMin = Convert.ToSingle(dataList[3]);
                            float ymax = float.Parse(dataList[dataList.Count - 1]);
                            YDEF.YDelt = (ymax - YDEF.YMin) / (YDEF.YNum - 1);
                        }
                        for (i = 0; i < YDEF.YNum; i++)
                        {
                            YDEF.Y[i] = YDEF.YMin + i * YDEF.YDelt;
                            values.Add(YDEF.Y[i]);
                        }
                        Dimension yDim = new Dimension(DimensionType.Y);
                        yDim.SetValues(values);
                        this.YDimension = yDim;
                        break;
                    case "ZDEF":
                        ZDEF.ZNum = Convert.ToInt32(dataList[1]);
                        ZDEF.Type = (string)dataList[2];
                        ZDEF.ZLevels = new Single[ZDEF.ZNum];
                        values = new List<double>();
                        if (ZDEF.Type.ToUpper() == "LINEAR")
                        {
                            ZDEF.SLevel = Convert.ToSingle(dataList[3]);
                            ZDEF.ZDelt = Convert.ToSingle(dataList[4]);
                            for (i = 0; i < ZDEF.ZNum; i++)
                            {
                                ZDEF.ZLevels[i] = ZDEF.SLevel + i * ZDEF.ZDelt;
                                values.Add(ZDEF.ZLevels[i]);
                            }
                        }
                        else
                        {
                            if (dataList.Count < ZDEF.ZNum + 3)
                            {
                                while (true)
                                {
                                    aLine = aLine + " " + sr.ReadLine().Trim();
                                    if (aLine == string.Empty)
                                    {
                                        continue;
                                    }
                                    dataArray = aLine.Split();
                                    LastNonEmpty = -1;
                                    dataList.Clear();
                                    for (i = 0; i < dataArray.Length; i++)
                                    {
                                        if (dataArray[i] != string.Empty)
                                        {
                                            LastNonEmpty++;
                                            dataList.Add(dataArray[i]);
                                        }
                                    }
                                    if (dataList.Count >= ZDEF.ZNum + 3)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (dataList.Count > ZDEF.ZNum + 3)
                            {
                                errorStr = "ZDEF is wrong! Please check the ctl file!";
                                goto ERROR;
                            }
                            for (i = 0; i < ZDEF.ZNum; i++)
                            {
                                ZDEF.ZLevels[i] = Convert.ToSingle(dataList[3 + i]);
                                values.Add(ZDEF.ZLevels[i]);
                            }
                            Dimension zDim = new Dimension(DimensionType.Z);
                            zDim.SetValues(values);
                            this.ZDimension = zDim;
                        }
                        break;
                    case "TDEF":
                        TDEF.TNum = Convert.ToInt32(dataList[1]);
                        TDEF.Type = (string)dataList[2];
                        TDEF.times = new DateTime[TDEF.TNum];
                        if (TDEF.Type.ToUpper() == "LINEAR")
                        {
                            string dStr = (string)dataList[3];
                            dStr = dStr.ToUpper();
                            i = dStr.IndexOf("Z");
                            if (i == -1)
                            {
                                if (char.IsNumber(dStr.ToCharArray()[0]))
                                {
                                    dStr = "00:00Z" + dStr;
                                }
                                else
                                {
                                    dStr = "00:00Z01" + dStr;
                                }
                            }
                            else if (i == 1)
                            {
                                dStr = "0" + dStr.Substring(0, 1) + ":00" + dStr.Substring(1);
                            }
                            else if (i == 2)
                            {
                                dStr = dStr.Substring(0, 2) + ":00" + dStr.Substring(2);
                            }
                            if (!(char.IsNumber(dStr.ToCharArray()[dStr.Length - 3])))
                            {
                                int aY = Convert.ToInt32(dStr.Substring(dStr.Length - 2, 2));
                                if (aY > 50)
                                {
                                    aY = 1900 + aY;
                                }
                                else
                                {
                                    aY = 2000 + aY;
                                }
                                dStr = dStr.Substring(0, dStr.Length - 2) + Convert.ToString(aY);
                            }
                            if (dStr.Length == 14)
                            {
                                dStr = dStr.Insert(6, "0");
                            }
                            string mn = dStr.Substring(8, 3);
                            string Nmn = mn.Substring(0, 1).ToUpper() + mn.Substring(1, 2).ToLower();
                            dStr = dStr.Replace(mn, Nmn);
                            dStr = dStr.Replace("Z", " ");
                            TDEF.STime = DateTime.ParseExact(dStr, "HH:mm ddMMMyyyy", System.Globalization.CultureInfo.InvariantCulture);

                            //Read time interval
                            TDEF.TDelt = dataList[4];
                            char[] tChar = dataList[4].ToCharArray();
                            int aPos = 0;    //Position between number and string
                            for (i = 0; i < tChar.Length; i++)
                            {
                                if (!Char.IsNumber(tChar[i]))
                                {
                                    aPos = i;
                                    break;
                                }
                            }
                            if (aPos == 0)
                            {
                                errorStr = "TDEF is wrong! Please check the ctl file!";
                                goto ERROR;
                            }
                            int iNum = int.Parse(TDEF.TDelt.Substring(0, aPos));
                            TDEF.DeltaValue = iNum;
                            string tStr = TDEF.TDelt.Substring(aPos);
                            TDEF.Unit = tStr;
                            switch (tStr.ToLower())
                            {
                                case "mn":
                                    for (i = 0; i < TDEF.TNum; i++)
                                    {
                                        TDEF.times[i] = TDEF.STime.AddMinutes(i * iNum);
                                    }
                                    break;
                                case "hr":
                                    for (i = 0; i < TDEF.TNum; i++)
                                    {
                                        TDEF.times[i] = TDEF.STime.AddHours(i * iNum);
                                    }
                                    break;
                                case "dy":
                                    if (this.OPTIONS.calendar_365_day)
                                    {
                                        DateTime stime = TDEF.STime.AddDays(-1);
                                        for (i = 0; i < TDEF.TNum; i++)
                                        {
                                            stime = stime.AddDays(1);
                                            if (stime.DayOfYear == 366)
                                                stime = stime.AddDays(1);

                                            TDEF.times[i] = stime;
                                        }
                                    }
                                    else
                                    {
                                        for (i = 0; i < TDEF.TNum; i++)
                                        {
                                            TDEF.times[i] = TDEF.STime.AddDays(i * iNum);
                                        }
                                    }
                                    break;
                                case "mo":
                                case "mon":
                                    for (i = 0; i < TDEF.TNum; i++)
                                    {
                                        TDEF.times[i] = TDEF.STime.AddMonths(i * iNum);
                                    }
                                    break;
                                case "yr":
                                    for (i = 0; i < TDEF.TNum; i++)
                                    {
                                        TDEF.times[i] = TDEF.STime.AddYears(i * iNum);
                                    }
                                    break;
                            }

                            values = new List<double>();
                            foreach (DateTime t in TDEF.times)
                            {
                                values.Add(DataConvert.ToDouble(t));
                            }
                            Dimension tDim = new Dimension(DimensionType.T);
                            tDim.SetValues(values);
                            this.TimeDimension = tDim;
                        }
                        else
                        {
                            errorStr = "Only linear TDEF data is supported at prsent! " + Environment.NewLine +
                                   "Please send your data to the author to improve MeteoInfo!";
                            goto ERROR;
                        }
                        break;
                    case "VARS":
                        int vNum = Convert.ToInt32(dataList[1]);
                        for (i = 0; i < vNum; i++)
                        {
                            aLine = sr.ReadLine();
                            dataArray = aLine.Split();
                            LastNonEmpty = -1;
                            dataList.Clear();
                            for (j = 0; j < dataArray.Length; j++)
                            {
                                if (dataArray[j] != string.Empty)
                                {
                                    LastNonEmpty++;
                                    dataList.Add(dataArray[j]);
                                }
                            }
                            Variable aVar = new Variable();
                            aVar.Name = (string)dataList[0];
                            int lNum = Convert.ToInt32(dataList[1]);
                            List<double> levs = new List<double>();
                            for (j = 0; j < lNum; j++)
                            {
                                if (ZDEF.ZNum > j)
                                {
                                    aVar.AddLevel(ZDEF.ZLevels[j]);
                                    levs.Add((double)ZDEF.ZLevels[j]);
                                }
                            }
                            aVar.Units = dataList[2];
                            if (aVar.Units.Contains("-1,40,"))
                            {
                                if (aVar.Units == "-1,40,1")
                                    _byteNum = 1;
                            }
                            
                            if (dataList.Count > 3)
                            {
                                aVar.Description = (string)dataList[3];
                            }
                            aVar.SetDimension(this.XDimension);
                            aVar.SetDimension(this.YDimension);
                            aVar.SetDimension(this.TimeDimension);
                            if (lNum > 1)
                            {
                                Dimension zDim = new Dimension(DimensionType.Z);
                                zDim.SetValues(levs);
                                aVar.SetDimension(zDim);
                            }

                            VARDEF.AddVar(aVar);
                        }
                        break;
                    case "ENDVARS":
                        isEnd = true;
                        break;
                }

                if (isEnd)
                {
                    break;
                }

            } while (aLine != null);

            sr.Close();

            //Set X/Y coordinate
            if (isLatLon)
            {
                X = XDEF.X;
                Y = YDEF.Y;
                XNum = XDEF.XNum;
                YNum = YDEF.YNum;
            }

            //Calculate record length
            _recordLen = XNum * YNum * 4;
            if (OPTIONS.sequential)
                _recordLen += 8;

            //Calculate data length of each time
            _recLenPerTime = 0;
            int llNum;
            for (i = 0; i < VARDEF.VNum; i++)
            {
                llNum = VARDEF.Vars[i].LevelNum;
                if (llNum == 0)
                {
                    llNum = 1;
                }
                _recLenPerTime += llNum * _recordLen;
            }

            goto FINISH;

        ERROR:
            sr.Close();
            return false;
        FINISH:
            return true;
        }

        ///// <summary>
        ///// Read GrADS ctl file
        ///// </summary>
        ///// <param name="aFile"></param>
        ///// <param name="aGDataInfo"></param>
        ///// <param name="errorStr"></param>
        ///// <returns></returns>
        //public Boolean ReadGrADSCTLFile(string aFile, ref GrADSDataInfo aGDataInfo, ref string errorStr)
        //{
        //    StreamReader sr = new StreamReader(aFile, Encoding.Default);
        //    string aLine;
        //    string[] dataArray;
        //    int i, j, LastNonEmpty;
        //    //ArrayList dataList = new ArrayList();
        //    List<string> dataList = new List<string>();
        //    Boolean isEnd = false;

        //    //Set dufault value
        //    aGDataInfo.DESCRIPTOR = aFile;

        //    do
        //    {
        //        aLine = sr.ReadLine().Trim();
        //        if (aLine == string.Empty)
        //        {
        //            continue;
        //        }
        //        dataArray = aLine.Split();
        //        LastNonEmpty = -1;
        //        dataList.Clear();
        //        for (i = 0; i < dataArray.Length; i++)
        //        {
        //            if (dataArray[i] != string.Empty)
        //            {
        //                LastNonEmpty++;
        //                dataList.Add(dataArray[i]);
        //            }
        //        }
        //        switch (((string)dataList[0]).ToUpper())
        //        {
        //            case "DSET":
        //                aGDataInfo.DSET = (string)dataList[1];
        //                if (aGDataInfo.DSET.Substring(0, 1) == "^")
        //                {
        //                    aGDataInfo.DSET = Path.Combine(Path.GetDirectoryName(aFile),
        //                        aGDataInfo.DSET.Substring(1));                            
        //                }

        //                if (!File.Exists(aGDataInfo.DSET))
        //                {
        //                    aGDataInfo.DSET = Path.Combine(Path.GetDirectoryName(aFile),
        //                        aGDataInfo.DSET);
        //                    if (!File.Exists(aGDataInfo.DSET))
        //                    {
        //                        errorStr = "The data file is not exist!" + Environment.NewLine + aGDataInfo.DSET;
        //                        goto ERROR;
        //                    }
        //                }
        //                break;
        //            case "DTYPE":
        //                aGDataInfo.DTYPE = (string)dataList[1];
        //                if (aGDataInfo.DTYPE.ToUpper() != "GRIDDED" && aGDataInfo.DTYPE.ToUpper() != "STATION")
        //                {
        //                    errorStr = "The data type is not supported at present!" + Environment.NewLine
        //                        + aGDataInfo.DTYPE;
        //                    goto ERROR;
        //                }
        //                break;
        //            case "OPTIONS":
        //                for (i = 1; i < dataList.Count; i++)
        //                {
        //                    switch (dataList[i].ToLower())
        //                    {
        //                        case "big_endian":
        //                            aGDataInfo.OPTIONS.big_endian = true;
        //                            break;
        //                        case "byteswapped":
        //                            aGDataInfo.OPTIONS.byteswapped = true;
        //                            break;
        //                        case "365_day_calendar":
        //                            aGDataInfo.OPTIONS.calendar_365_day = true;
        //                            break;
        //                        case "cray_32bit_ieee":
        //                            aGDataInfo.OPTIONS.cray_32bit_ieee = true;
        //                            break;
        //                        case "little_endian":
        //                            aGDataInfo.OPTIONS.little_endian = true;
        //                            break;
        //                        case "pascals":
        //                            aGDataInfo.OPTIONS.pascals = true;
        //                            break;
        //                        case "sequential":
        //                            aGDataInfo.OPTIONS.sequential = true;
        //                            break;
        //                        case "template":
        //                            aGDataInfo.OPTIONS.template = true;
        //                            break;
        //                        case "yrev":
        //                            aGDataInfo.OPTIONS.yrev = true;
        //                            break;
        //                        case "zrev":
        //                            aGDataInfo.OPTIONS.zrev = true;
        //                            break;
        //                    }
        //                }
        //                break;
        //            case "MissingValue":
        //                aGDataInfo.MissingValue = Convert.ToDouble(dataList[1]);
        //                break;
        //            case "TITLE":
        //                aGDataInfo.TITLE = aLine.Substring(5, aLine.Length - 5).Trim();
        //                break;
        //            case "FILEHEADER":
        //                aGDataInfo.FILEHEADER = int.Parse(dataList[1]);
        //                break;
        //            case "THEADER":
        //                aGDataInfo.THEADER = int.Parse(dataList[1]);
        //                break;
        //            case "XYHEADER":
        //                aGDataInfo.XYHEADER = int.Parse(dataList[1]);
        //                break;
        //            case "PDEF":
        //                aGDataInfo.PDEF.PDEF_Type = dataList[3].ToUpper();
        //                string ProjStr;
        //                ProjectionInfo theProj = new ProjectionInfo();
        //                switch (aGDataInfo.PDEF.PDEF_Type)
        //                {
        //                    case "LCC":
        //                    case "LCCR":
        //                        PDEF_LCC aPLCC = new PDEF_LCC();
        //                        aPLCC.isize = int.Parse(dataList[1]);
        //                        aPLCC.jsize = int.Parse(dataList[2]);
        //                        aPLCC.latref = Single.Parse(dataList[4]);
        //                        aPLCC.lonref = Single.Parse(dataList[5]);
        //                        aPLCC.iref = Single.Parse(dataList[6]);
        //                        aPLCC.jref = Single.Parse(dataList[7]);
        //                        aPLCC.Struelat = Single.Parse(dataList[8]);
        //                        aPLCC.Ntruelat = Single.Parse(dataList[9]);
        //                        aPLCC.slon = Single.Parse(dataList[10]);
        //                        aPLCC.dx = Single.Parse(dataList[11]);
        //                        aPLCC.dy = Single.Parse(dataList[12]);
        //                        aGDataInfo.PDEF.PDEF_Content = aPLCC;

        //                        aGDataInfo.isLatLon = false;                                                              

        //                        ProjStr = "+proj=lcc" +
        //                                "+lat_1=" + aPLCC.Struelat.ToString() +
        //                                "+lat_2=" + aPLCC.Ntruelat.ToString() +
        //                                "+lat_0=" + aPLCC.latref.ToString() +
        //                                "+lon_0=" + aPLCC.slon.ToString();


        //                        theProj = new ProjectionInfo(ProjStr);
        //                        aGDataInfo.ProjInfo = theProj;
        //                        if (aGDataInfo.PDEF.PDEF_Type == "LCCR")
        //                            aGDataInfo.EarthWind = false;

        //                        //Set X Y
        //                        aGDataInfo.XNum = aPLCC.isize;
        //                        aGDataInfo.YNum = aPLCC.jsize;
        //                        aGDataInfo.X = new double[aPLCC.isize];
        //                        aGDataInfo.Y = new double[aPLCC.jsize];
        //                        GetProjectedXY(theProj, aPLCC.dx, aPLCC.dy, aPLCC.iref, aPLCC.jref, aPLCC.lonref,
        //                            aPLCC.latref, ref aGDataInfo.X, ref aGDataInfo.Y);
        //                        break;
        //                    default:
        //                        errorStr = "The PDEF type is not supported at present!" + Environment.NewLine +
        //                            "Please send your data to the author to improve MeteoInfo!";
        //                        goto ERROR;
        //                }
        //                break;
        //            case "XDEF":
        //                aGDataInfo.XDEF.XNum = Convert.ToInt32(dataList[1]);
        //                aGDataInfo.XDEF.X = new double[aGDataInfo.XDEF.XNum];
        //                aGDataInfo.XDEF.Type = (string)dataList[2];
        //                if (aGDataInfo.XDEF.Type.ToUpper() == "LINEAR")
        //                {
        //                    aGDataInfo.XDEF.XMin = Convert.ToSingle(dataList[3]);
        //                    aGDataInfo.XDEF.XDelt = Convert.ToSingle(dataList[4]);
        //                    for (i = 0; i < aGDataInfo.XDEF.XNum; i++)
        //                    {
        //                        aGDataInfo.XDEF.X[i] = aGDataInfo.XDEF.XMin + i * aGDataInfo.XDEF.XDelt;
        //                    }
        //                    if (aGDataInfo.XDEF.XMin == 0 && aGDataInfo.XDEF.X[aGDataInfo.XDEF.XNum - 1]
        //                        + aGDataInfo.XDEF.XDelt == 360)
        //                    {
        //                        aGDataInfo.isGlobal = true;
        //                    }
        //                }
        //                else
        //                {
        //                    errorStr = "Only linear XDEF data is supported at prsent! " + Environment.NewLine +
        //                        "Please send your data to the author to improve MeteoInfo!";
        //                    goto ERROR;
        //                }
        //                break;
        //            case "YDEF":
        //                aGDataInfo.YDEF.YNum = Convert.ToInt32(dataList[1]);
        //                aGDataInfo.YDEF.Y = new double[aGDataInfo.YDEF.YNum];
        //                aGDataInfo.YDEF.Type = (string)dataList[2];
        //                if (aGDataInfo.YDEF.Type.ToUpper() == "LINEAR")
        //                {
        //                    aGDataInfo.YDEF.YMin = Convert.ToSingle(dataList[3]);
        //                    aGDataInfo.YDEF.YDelt = Convert.ToSingle(dataList[4]);
        //                    for (i = 0; i < aGDataInfo.YDEF.YNum; i++)
        //                    {
        //                        aGDataInfo.YDEF.Y[i] = aGDataInfo.YDEF.YMin + i * aGDataInfo.YDEF.YDelt;
        //                    }
        //                }
        //                else
        //                {
        //                    errorStr = "Only linear YDEF data is supported at prsent! " + Environment.NewLine +
        //                        "Please send your data to the author to improve MeteoInfo!";
        //                    goto ERROR;
        //                }
        //                break;
        //            case "ZDEF":
        //                aGDataInfo.ZDEF.ZNum = Convert.ToInt32(dataList[1]);
        //                aGDataInfo.ZDEF.Type = (string)dataList[2];
        //                aGDataInfo.ZDEF.ZLevels = new Single[aGDataInfo.ZDEF.ZNum];
        //                if (aGDataInfo.ZDEF.Type.ToUpper() == "LINEAR")
        //                {
        //                    aGDataInfo.ZDEF.SLevel = Convert.ToSingle(dataList[3]);
        //                    aGDataInfo.ZDEF.ZDelt = Convert.ToSingle(dataList[4]);
        //                    for (i = 0; i < aGDataInfo.ZDEF.ZNum; i++)
        //                    {
        //                        aGDataInfo.ZDEF.ZLevels[i] = aGDataInfo.ZDEF.SLevel + i * aGDataInfo.ZDEF.ZDelt;
        //                    }
        //                }
        //                else
        //                {
        //                    if (dataList.Count < aGDataInfo.ZDEF.ZNum + 3)
        //                    {
        //                        while (true)
        //                        {
        //                            aLine = aLine + " " + sr.ReadLine().Trim();
        //                            if (aLine == string.Empty)
        //                            {
        //                                continue;
        //                            }
        //                            dataArray = aLine.Split();
        //                            LastNonEmpty = -1;
        //                            dataList.Clear();
        //                            for (i = 0; i < dataArray.Length; i++)
        //                            {
        //                                if (dataArray[i] != string.Empty)
        //                                {
        //                                    LastNonEmpty++;
        //                                    dataList.Add(dataArray[i]);
        //                                }
        //                            }
        //                            if (dataList.Count >= aGDataInfo.ZDEF.ZNum + 3)
        //                            {
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    if (dataList.Count > aGDataInfo.ZDEF.ZNum + 3)
        //                    {
        //                        errorStr = "ZDEF is wrong! Please check the ctl file!";
        //                        goto ERROR;
        //                    }
        //                    for (i = 0; i < aGDataInfo.ZDEF.ZNum; i++)
        //                    {
        //                        aGDataInfo.ZDEF.ZLevels[i] = Convert.ToSingle(dataList[3 + i]);
        //                    }
        //                }
        //                break;
        //            case "TDEF":
        //                aGDataInfo.TDEF.TNum = Convert.ToInt32(dataList[1]);
        //                aGDataInfo.TDEF.Type = (string)dataList[2];
        //                aGDataInfo.TDEF.times = new DateTime[aGDataInfo.TDEF.TNum];
        //                if (aGDataInfo.TDEF.Type.ToUpper() == "LINEAR")
        //                {
        //                    string dStr = (string)dataList[3];
        //                    dStr = dStr.ToUpper();
        //                    i = dStr.IndexOf("Z");
        //                    if (i == -1)
        //                    {
        //                        if (char.IsNumber(dStr.ToCharArray()[0]))
        //                        {
        //                            dStr = "00:00Z" + dStr;
        //                        }
        //                        else
        //                        {
        //                            dStr = "00:00Z01" + dStr;
        //                        }
        //                    }
        //                    else if (i == 1)
        //                    {
        //                        dStr = "0" + dStr.Substring(0, 1) + ":00" + dStr.Substring(1);
        //                    }
        //                    else if (i == 2)
        //                    {
        //                        dStr = dStr.Substring(0, 2) + ":00" + dStr.Substring(2);
        //                    }
        //                    if (!(char.IsNumber(dStr.ToCharArray()[dStr.Length - 3])))
        //                    {
        //                        int aY = Convert.ToInt32(dStr.Substring(dStr.Length - 2, 2));
        //                        if (aY > 50)
        //                        {
        //                            aY = 1900 + aY;
        //                        }
        //                        else
        //                        {
        //                            aY = 2000 + aY;
        //                        }
        //                        dStr = dStr.Substring(0, dStr.Length - 2) + Convert.ToString(aY);
        //                    }
        //                    if (dStr.Length == 14)
        //                    {
        //                        dStr = dStr.Insert(6, "0");
        //                    }
        //                    string mn = dStr.Substring(8, 3);
        //                    string Nmn = mn.Substring(0, 1).ToUpper() + mn.Substring(1, 2).ToLower();
        //                    dStr = dStr.Replace(mn, Nmn);
        //                    dStr = dStr.Replace("Z", " ");
        //                    aGDataInfo.TDEF.STime = DateTime.ParseExact(dStr, "HH:mm ddMMMyyyy", System.Globalization.CultureInfo.InvariantCulture);

        //                    //Read time interval
        //                    aGDataInfo.TDEF.TDelt = dataList[4];
        //                    char[] tChar = dataList[4].ToCharArray();
        //                    int aPos = 0;    //Position between number and string
        //                    for (i = 0; i < tChar.Length; i++)
        //                    {
        //                        if (!Char.IsNumber(tChar[i]))
        //                        {
        //                            aPos = i;
        //                            break;
        //                        }
        //                    }
        //                    if (aPos == 0)
        //                    {
        //                        errorStr = "TDEF is wrong! Please check the ctl file!";
        //                        goto ERROR;
        //                    }
        //                    int iNum = int.Parse(aGDataInfo.TDEF.TDelt.Substring(0, aPos));
        //                    string tStr = aGDataInfo.TDEF.TDelt.Substring(aPos);
        //                    switch (tStr.ToLower())
        //                    {
        //                        case "mn":
        //                            for (i = 0; i < aGDataInfo.TDEF.TNum; i++)
        //                            {
        //                                aGDataInfo.TDEF.times[i] = aGDataInfo.TDEF.STime.AddMinutes(i * iNum);
        //                            }
        //                            break;
        //                        case "hr":
        //                            for (i = 0; i < aGDataInfo.TDEF.TNum; i++)
        //                            {
        //                                aGDataInfo.TDEF.times[i] = aGDataInfo.TDEF.STime.AddHours(i * iNum);
        //                            }
        //                            break;
        //                        case "dy":
        //                            for (i = 0; i < aGDataInfo.TDEF.TNum; i++)
        //                            {
        //                                aGDataInfo.TDEF.times[i] = aGDataInfo.TDEF.STime.AddDays(i * iNum);
        //                            }
        //                            break;
        //                        case "mo":
        //                        case "mon":
        //                            for (i = 0; i < aGDataInfo.TDEF.TNum; i++)
        //                            {
        //                                aGDataInfo.TDEF.times[i] = aGDataInfo.TDEF.STime.AddMonths(i * iNum);
        //                            }
        //                            break;
        //                        case "yr":
        //                            for (i = 0; i < aGDataInfo.TDEF.TNum; i++)
        //                            {
        //                                aGDataInfo.TDEF.times[i] = aGDataInfo.TDEF.STime.AddYears(i * iNum);
        //                            }
        //                            break;
        //                    }
        //                }
        //                else
        //                {
        //                    errorStr = "Only linear TDEF data is supported at prsent! " + Environment.NewLine +
        //                           "Please send your data to the author to improve MeteoInfo!";
        //                    goto ERROR;
        //                }
        //                break;
        //            case "VARS":
        //                aGDataInfo.VARDEF.VNum = Convert.ToInt32(dataList[1]);
        //                aGDataInfo.VARDEF.Vars = new VAR[aGDataInfo.VARDEF.VNum];
        //                for (i = 0; i < aGDataInfo.VARDEF.VNum; i++)
        //                {
        //                    aLine = sr.ReadLine();
        //                    dataArray = aLine.Split();
        //                    LastNonEmpty = -1;
        //                    dataList.Clear();
        //                    for (j = 0; j < dataArray.Length; j++)
        //                    {
        //                        if (dataArray[j] != string.Empty)
        //                        {
        //                            LastNonEmpty++;
        //                            dataList.Add(dataArray[j]);
        //                        }
        //                    }
        //                    aGDataInfo.VARDEF.Vars[i].VName = (string)dataList[0];
        //                    aGDataInfo.VARDEF.Vars[i].LevelNum = Convert.ToInt32(dataList[1]);
        //                    aGDataInfo.VARDEF.Vars[i].Units = (string)dataList[2];
        //                    if (dataList.Count > 3)
        //                    {
        //                        aGDataInfo.VARDEF.Vars[i].Description = (string)dataList[3];
        //                    }
        //                }
        //                break;
        //            case "ENDVARS":
        //                isEnd = true;
        //                break;
        //        }

        //        if (isEnd)
        //        {
        //            break;
        //        }

        //    } while (aLine != null);

        //    sr.Close();

        //    //Set X/Y coordinate
        //    if (aGDataInfo.isLatLon)
        //    {
        //        aGDataInfo.X = aGDataInfo.XDEF.X;
        //        aGDataInfo.Y = aGDataInfo.YDEF.Y;
        //        aGDataInfo.XNum = aGDataInfo.XDEF.XNum;
        //        aGDataInfo.YNum = aGDataInfo.YDEF.YNum;
        //    }

        //    //Calculate record length
        //    aGDataInfo._recordLen = aGDataInfo.XNum * aGDataInfo.YNum * 4;
        //    if (aGDataInfo.OPTIONS.sequential)
        //        aGDataInfo._recordLen += 8;

        //    //Calculate data length of each time
        //    aGDataInfo._recLenPerTime = 0;
        //    int lNum;
        //    for (i = 0; i < aGDataInfo.VARDEF.VNum; i++)
        //    {
        //        lNum = aGDataInfo.VARDEF.Vars[i].LevelNum;
        //        if (lNum == 0)
        //        {
        //            lNum = 1;
        //        }
        //        aGDataInfo._recLenPerTime += lNum * aGDataInfo._recordLen;
        //    }

        //    goto FINISH;

        //ERROR:
        //    sr.Close();
        //    return false;
        //FINISH:
        //    return true;
        //}

        /// <summary>
        /// Write GrADS control file
        /// </summary>
        public void WriteGrADSCTLFile()
        {
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            string aLine;
            int i;

            string fn = Path.GetFileName(DSET);
            sw.WriteLine("DSET ^" + fn);
            if (DTYPE != "GRIDDED")
            {
                sw.WriteLine("DTYPE " + DTYPE);
            }
            sw.WriteLine("TITLE " + TITLE);
            sw.WriteLine("UNDEF " + MissingValue.ToString());

            if (DTYPE == "GRIDDED")
            {
                aLine = "XDEF " + XDEF.XNum.ToString() + " " + XDEF.Type;
                if (XDEF.Type.ToUpper() == "LINEAR")
                {
                    aLine = aLine + " " + XDEF.XMin.ToString() + " " + XDEF.XDelt.ToString();
                }
                else
                {
                    for (i = 0; i < XDEF.XNum; i++)
                    {
                        aLine = aLine + " " + XDEF.X[i].ToString();
                    }
                }
                sw.WriteLine(aLine);
                aLine = "YDEF " + YDEF.YNum.ToString() + " " + YDEF.Type;
                if (YDEF.Type.ToUpper() == "LINEAR")
                {
                    aLine = aLine + " " + YDEF.YMin.ToString() + " " + YDEF.YDelt.ToString();
                }
                else
                {
                    for (i = 0; i < YDEF.YNum; i++)
                    {
                        aLine = aLine + " " + YDEF.Y[i].ToString();
                    }
                }
                sw.WriteLine(aLine);
                aLine = "ZDEF " + ZDEF.ZNum.ToString() + " " + ZDEF.Type;
                if (ZDEF.Type.ToUpper() == "LINEAR")
                {
                    aLine = aLine + " " + ZDEF.SLevel.ToString() + " " + ZDEF.ZDelt.ToString();
                }
                else
                {
                    for (i = 0; i < ZDEF.ZNum; i++)
                    {
                        aLine = aLine + " " + ZDEF.ZLevels[i].ToString();
                    }
                }
                sw.WriteLine(aLine);
            }

            aLine = "TDEF " + TDEF.TNum.ToString() + " " + TDEF.Type;
            if (TDEF.Type.ToUpper() == "LINEAR")
            {
                aLine = aLine + " " + TDEF.STime.ToString("HH:mmZddMMMyyyy", DateTimeFormatInfo.InvariantInfo) + " " + TDEF.TDelt;
            }
            else
            {
                for (i = 0; i < TDEF.TNum; i++)
                {
                    aLine = aLine + " " + TDEF.times[i].ToString("HH:mmZddMMMyyyy", DateTimeFormatInfo.InvariantInfo);
                }
            }
            sw.WriteLine(aLine);

            sw.WriteLine("VARS " + VARDEF.VNum.ToString());
            for (i = 0; i < VARDEF.VNum; i++)
            {
                sw.WriteLine(VARDEF.Vars[i].Name + " " + VARDEF.Vars[i].LevelNum + " " +
                    VARDEF.Vars[i].Units + "  " + VARDEF.Vars[i].Description);
            }
            sw.WriteLine("ENDVARS");

            sw.Close();
            fs.Close();
        }        

        ///// <summary>
        ///// Write GrADS control file
        ///// </summary>
        ///// <param name="aFile">File name</param>
        ///// <param name="aDataInfo">GrADS data info</param>
        //public void WriteGrADSCTLFile(string aFile, GrADSDataInfo aDataInfo)
        //{
        //    FileStream fs = new FileStream(aFile, FileMode.Create, FileAccess.Write);
        //    StreamWriter sw = new StreamWriter(fs);
        //    string aLine;
        //    int i;
            
        //    sw.WriteLine("DSET " + aDataInfo.DSET);
        //    if (aDataInfo.DTYPE != "GRIDDED")
        //    {
        //        sw.WriteLine("DTYPE " + aDataInfo.DTYPE);
        //    }
        //    sw.WriteLine("TITLE " + aDataInfo.TITLE);
        //    sw.WriteLine("MissingValue " + aDataInfo.MissingValue.ToString());

        //    aLine = "XDEF " + aDataInfo.XDEF.XNum.ToString() + " " + aDataInfo.XDEF.Type;
        //    if (aDataInfo.XDEF.Type.ToUpper() == "LINEAR")
        //    {
        //        aLine = aLine + " " + aDataInfo.XDEF.XMin.ToString() + " " + aDataInfo.XDEF.XDelt.ToString();
        //    }
        //    else
        //    {
        //        for (i = 0; i < aDataInfo.XDEF.XNum; i++)
        //        {
        //            aLine = aLine + " " + aDataInfo.XDEF.X[i].ToString();
        //        }
        //    }
        //    sw.WriteLine(aLine);
        //    aLine = "YDEF " + aDataInfo.YDEF.YNum.ToString() + " " + aDataInfo.YDEF.Type;
        //    if (aDataInfo.YDEF.Type.ToUpper() == "LINEAR")
        //    {
        //        aLine = aLine + " " + aDataInfo.YDEF.YMin.ToString() + " " + aDataInfo.YDEF.YDelt.ToString();
        //    }
        //    else
        //    {
        //        for (i = 0; i < aDataInfo.YDEF.YNum; i++)
        //        {
        //            aLine = aLine + " " + aDataInfo.YDEF.Y[i].ToString();
        //        }
        //    }
        //    sw.WriteLine(aLine);
        //    aLine = "ZDEF " + aDataInfo.ZDEF.ZNum.ToString() + " " + aDataInfo.ZDEF.Type;
        //    if (aDataInfo.ZDEF.Type.ToUpper() == "LINEAR")
        //    {
        //        aLine = aLine + " " + aDataInfo.ZDEF.SLevel.ToString() + " " + aDataInfo.ZDEF.ZDelt.ToString();
        //    }
        //    else
        //    {
        //        for (i = 0; i < aDataInfo.ZDEF.ZNum; i++)
        //        {
        //            aLine = aLine + " " + aDataInfo.ZDEF.ZLevels[i].ToString();
        //        }
        //    }
        //    sw.WriteLine(aLine);
        //    aLine = "TDEF " + aDataInfo.TDEF.TNum.ToString() + " " + aDataInfo.TDEF.Type;
        //    if (aDataInfo.TDEF.Type.ToUpper() == "LINEAR")
        //    {
        //        aLine = aLine + " " + aDataInfo.TDEF.STime.ToString("HH:mmZddMMMyyyy", DateTimeFormatInfo.InvariantInfo) + " " + aDataInfo.TDEF.TDelt;
        //    }
        //    else
        //    {
        //        for (i = 0; i < aDataInfo.TDEF.TNum; i++)
        //        {
        //            aLine = aLine + " " + aDataInfo.TDEF.times[i].ToString("HH:mmZddMMMyyyy", DateTimeFormatInfo.InvariantInfo);
        //        }
        //    }
        //    sw.WriteLine(aLine);

        //    sw.WriteLine("VARS " + aDataInfo.VARDEF.VNum.ToString());
        //    for (i = 0; i < aDataInfo.VARDEF.VNum; i++)
        //    {
        //        sw.WriteLine(aDataInfo.VARDEF.Vars[i].VName + " " + aDataInfo.VARDEF.Vars[i].LevelNum + " " +
        //            aDataInfo.VARDEF.Vars[i].Units + "  " + aDataInfo.VARDEF.Vars[i].Description);
        //    }
        //    sw.WriteLine("ENDVARS");

        //    sw.Close();
        //    fs.Close();
        //}

        /// <summary>
        /// Write GrADS ctl file - station
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="aM1DataInfo"></param>
        public void WriteGrADSCtlFile_Station(string aFile, MICAPS1DataInfo aM1DataInfo)
        {
            string ctlFile = Path.Combine(Path.GetDirectoryName(aFile),
                Path.GetFileNameWithoutExtension(aFile) + ".ctl");
            string stnmapFile = Path.Combine(Path.GetDirectoryName(aFile),
                Path.GetFileNameWithoutExtension(aFile) + ".map");
            FileStream fs = new FileStream(ctlFile, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            int varNum = aM1DataInfo.varNum - 5;
            List<string> varNames = new List<string>();
            int i;
            string[] items = new string[] {"TotalCloud","WindDirection","WindSpeed","Pressure",
                "PressVar3h","WeatherPast1","WeatherPast2","Precipitation6h", "LowCloudShape",
                "LowCloudAmount","LowCloudHeight","DewPoint","Visibility","WeatherNow",
                "Temprature","MiddleCloudShape","HighCloudShape","TempVar24h","PressVar24h"};
            if (varNum > 19)
            {
                varNum = varNum - 2;
            }
            for (i = 0; i < varNum; i++)
            {
                varNames.Add(items[i]);
            }

            sw.WriteLine("DSET " + aFile);
            sw.WriteLine("DTYPE station");
            sw.WriteLine("STNMAP " + stnmapFile);
            sw.WriteLine("MissingValue " + aM1DataInfo.MissingValue);
            sw.WriteLine("TITLE Surface station observation");
            string timeAdd;
            if (aM1DataInfo.isAutoStation)
            {
                timeAdd = "1hr";
            }
            else
            {
                timeAdd = "3hr";
            }
            sw.WriteLine("TDEF 1 LINEAR " + aM1DataInfo.DateTime.ToString("HHZddMMMyyyy", DateTimeFormatInfo.InvariantInfo) + " " + timeAdd);
            sw.WriteLine("VARS " + varNum.ToString());
            for (i = 0; i < varNum; i++)
            {
                sw.WriteLine(varNames[i] + " 0 99 Variable");
            }
            sw.WriteLine("ENDVARS");

            //Close
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// Write GrADS ctl file -station
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="varNames"></param>
        /// <param name="MissingValue"></param>
        /// <param name="tIncrement"></param>
        /// <param name="sTime"></param>
        /// <param name="timeNum"></param>
        public void WriteGrADSCtlFile_Station(string aFile, List<string> varNames, double MissingValue,
            string tIncrement, DateTime sTime, int timeNum)
        {
            string ctlFile = Path.Combine(Path.GetDirectoryName(aFile),
                Path.GetFileNameWithoutExtension(aFile) + ".ctl");
            string stnmapFile = Path.Combine(Path.GetDirectoryName(aFile),
                Path.GetFileNameWithoutExtension(aFile) + ".map");
            FileStream fs = new FileStream(ctlFile, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            int varNum = varNames.Count;
            int i;

            sw.WriteLine("DSET " + aFile);
            sw.WriteLine("DTYPE station");
            sw.WriteLine("STNMAP " + stnmapFile);
            sw.WriteLine("UNDEF " + MissingValue);
            sw.WriteLine("TITLE Surface station observation");
            sw.WriteLine("TDEF " + timeNum.ToString() + " LINEAR " +
                sTime.ToString("HHZddMMMyyyy", DateTimeFormatInfo.InvariantInfo) + " " + tIncrement);
            sw.WriteLine("VARS " + varNum.ToString());
            for (i = 0; i < varNum; i++)
            {
                sw.WriteLine(varNames[i] + " 0 99 Variable");
            }
            sw.WriteLine("ENDVARS");

            //Close
            sw.Close();
            fs.Close();
        }

        private object[] getFilePath_Template(int timeIdx)
        {
            string filePath;
            string path = Path.GetDirectoryName(DSET);
            string fn = Path.GetFileName(DSET);
            DateTime time = this.Times[timeIdx];
            String tStr = "none";
            if (fn.Contains("%y4"))
            {
                fn = fn.Replace("%y4", time.ToString("yyyy"));
                tStr = "year";
            }
            if (fn.Contains("%y2"))
            {
                fn = fn.Replace("%y2", time.ToString("yy"));
                tStr = "year";
            }
            if (fn.Contains("%m1"))
            {
                fn = fn.Replace("%m1", time.ToString("M"));
                tStr = "month";
            }
            if (fn.Contains("%m2"))
            {
                fn = fn.Replace("%m2", time.ToString("MM"));
                tStr = "month";
            }
            if (fn.Contains("%mc"))
            {
                fn = fn.Replace("%mc", time.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture));
                tStr = "month";
            }
            if (fn.Contains("%d1"))
            {
                fn = fn.Replace("%d1", time.ToString("d"));
                tStr = "day";
            }
            if (fn.Contains("%d2"))
            {
                fn = fn.Replace("%d2", time.ToString("dd"));
                tStr = "day";
            }
            if (fn.Contains("%h1"))
            {
                fn = fn.Replace("%h1", time.Hour.ToString());
                tStr = "hour";
            }
            if (fn.Contains("%h2"))
            {
                fn = fn.Replace("%h2", time.ToString("HH"));
                tStr = "hour";
            }
            if (fn.Contains("%n2"))
            {
                fn = fn.Replace("%n2", time.ToString("mm"));
                tStr = "minute";
            }

            filePath = Path.Combine(path, fn);

            if (tStr == "none")
            {
                return new object[] { filePath, timeIdx };
            }

            int tIdx = 0;
            int month = time.Month;
            int day = time.Day;
            switch (tStr.ToLower())
            {
                case "year":
                    switch (TDEF.Unit)
                    {
                        case "mn":
                            tIdx = ((time.DayOfYear - 1) * 24 * 60 + time.Minute) / TDEF.DeltaValue;
                            break;
                        case "hr":
                            tIdx = ((time.DayOfYear - 1) * 24 + time.Hour) / TDEF.DeltaValue;
                            break;
                        case "dy":
                            tIdx = time.DayOfYear - 1;
                            break;
                        case "mo":
                        case "mon":
                            tIdx = time.Month - 1;
                            break;
                    }
                    break;
                case "month":
                    switch (TDEF.Unit)
                    {
                        case "mn":
                            tIdx = ((time.Day - 1) * 24 * 60 + time.Minute) / TDEF.DeltaValue;
                            break;
                        case "hr":
                            tIdx = ((time.Day - 1) * 24 + time.Hour) / TDEF.DeltaValue;
                            break;
                        case "dy":
                            tIdx = time.Day - 1;
                            break;
                    }
                    break;
                case "day":
                    switch (TDEF.Unit)
                    {
                        case "mn":
                            tIdx = ((time.Hour - 1) * 60 + time.Minute) / TDEF.DeltaValue;
                            break;
                        case "hr":
                            tIdx = time.Hour - 1;
                            break;
                    }
                    break;
            }

            return new object[] { filePath, tIdx};
        }

        /// <summary>
        /// Read GrADS grid data - Lat/Lon
        /// </summary>        
        /// <param name="timeIdx">time index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>        
        /// <returns>grid data</returns>
        public GridData GetGridData_LonLat(int timeIdx, int varIdx, int levelIdx)
        {
            GridData gridData = new GridData();
            gridData.Data = ReadGrADSData_Grid_LonLat(timeIdx, varIdx, levelIdx);
            gridData.X = X;
            gridData.Y = Y;                     
            gridData.MissingValue = MissingValue;

            if (OPTIONS.yrev)
                gridData.YReverse();

            return gridData;
        }

        /// <summary>
        /// Read GrADS grid data - Lat/Lon
        /// </summary>        
        /// <param name="timeIdx">time index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>        
        /// <returns>grid data</returns>
        public double[,] ReadGrADSData_Grid_LonLat(int timeIdx, int varIdx, int levelIdx)
        {
            int xNum, yNum;
            xNum = XNum;
            yNum = YNum;
            double[,] gridData = new double[yNum, xNum];            

            string filePath = DSET;
            int tIdx = timeIdx;
            if (OPTIONS.template)
            {
                object[] result = getFilePath_Template(timeIdx);
                filePath = (string)result[0];
                tIdx = (int)result[1];
            }

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, j, lNum;
            byte[] aBytes;

            br.BaseStream.Position = FILEHEADER;
            br.BaseStream.Position += (long)tIdx * (long)_recLenPerTime;
            
            for (i = 0; i < varIdx; i++)
            {
                lNum = VARDEF.Vars[i].LevelNum;
                if (lNum == 0)
                {
                    lNum = 1;
                }
                br.BaseStream.Position += lNum * _recordLen;
            }
            br.BaseStream.Position += levelIdx * _recordLen;

            if (OPTIONS.sequential)
                br.BaseStream.Position += 4;

            //Read X/Y data
            byte[] byteData = br.ReadBytes(xNum * yNum * _byteNum);
            int start = 0;
            for (i = 0; i < yNum; i++)
            {
                for (j = 0; j < xNum; j++)
                {
                    aBytes = new byte[4];
                    Array.Copy(byteData, start, aBytes, 0, _byteNum);
                    start += _byteNum;
                    //aBytes = br.ReadBytes(4);                    
                    //if (aGDataInfo.OPTIONS.cray_32bit_ieee)
                    //    gridData[i, j] = VaxSingleFromBytes(aBytes);
                    //else
                    switch (_byteNum)
                    {
                        case 1:
                            gridData[i, j] = aBytes[0];
                            break;
                        default:
                            if (OPTIONS.big_endian || OPTIONS.byteswapped)
                                Array.Reverse(aBytes);
                            gridData[i, j] = BitConverter.ToSingle(aBytes, 0);
                            break;
                    }                    
                }
            }

            br.Close();
            fs.Close();

            return gridData;
        }

        /// <summary>
        /// Read GrADS grid data - Time/Lon
        /// </summary>        
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_TimeLon(int latIdx, int varIdx, int levelIdx)
        {
            int xNum, yNum;
            xNum = XNum;
            yNum = TDEF.TNum;
            int i, j, lNum, t;
            double[,] gridData = new double[yNum, xNum];

            if (OPTIONS.template)
            {
                byte[] aBytes;
                long aTPosition;

                for (t = 0; t < TDEF.TNum; t++)
                {
                    object[] result = getFilePath_Template(t);
                    String filePath = (string)result[0];
                    int tIdx = (int)result[1];
                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    br.BaseStream.Position = (long)FILEHEADER + (long)tIdx * (long)_recLenPerTime;
                    aTPosition = br.BaseStream.Position;

                    for (i = 0; i < varIdx; i++)
                    {
                        lNum = VARDEF.Vars[i].LevelNum;
                        if (lNum == 0)
                        {
                            lNum = 1;
                        }
                        br.BaseStream.Position += lNum * _recordLen;
                    }
                    br.BaseStream.Position += levelIdx * _recordLen;
                    if (OPTIONS.sequential)
                        br.BaseStream.Position += 4;
                    br.BaseStream.Position += latIdx * xNum * 4;

                    if (br.BaseStream.Position >= br.BaseStream.Length)
                    {
                        Console.WriteLine("Erro");
                    }

                    for (j = 0; j < xNum; j++)
                    {
                        aBytes = br.ReadBytes(4);
                        if (OPTIONS.big_endian || OPTIONS.byteswapped)
                        {
                            Array.Reverse(aBytes);
                        }
                        gridData[t, j] = BitConverter.ToSingle(aBytes, 0);
                    }
                    //br.BaseStream.Position += (yNum - latIdx - 1) * xNum * 4;   
                    br.BaseStream.Position = aTPosition;
                    br.Close();
                    fs.Close();
                }                
            }
            else
            {
                FileStream fs = new FileStream(DSET, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);                
                byte[] aBytes;
                long aTPosition;

                for (t = 0; t < TDEF.TNum; t++)
                {
                    br.BaseStream.Position = (long)FILEHEADER + (long)t * (long)_recLenPerTime;
                    aTPosition = br.BaseStream.Position;

                    for (i = 0; i < varIdx; i++)
                    {
                        lNum = VARDEF.Vars[i].LevelNum;
                        if (lNum == 0)
                        {
                            lNum = 1;
                        }
                        br.BaseStream.Position += lNum * _recordLen;
                    }
                    br.BaseStream.Position += levelIdx * _recordLen;
                    if (OPTIONS.sequential)
                        br.BaseStream.Position += 4;
                    br.BaseStream.Position += latIdx * xNum * 4;

                    if (br.BaseStream.Position >= br.BaseStream.Length)
                    {
                        Console.WriteLine("Erro");
                    }

                    for (j = 0; j < xNum; j++)
                    {
                        aBytes = br.ReadBytes(4);
                        if (OPTIONS.big_endian || OPTIONS.byteswapped)
                        {
                            Array.Reverse(aBytes);
                        }
                        gridData[t, j] = BitConverter.ToSingle(aBytes, 0);
                    }
                    //br.BaseStream.Position += (yNum - latIdx - 1) * xNum * 4;   
                    br.BaseStream.Position = aTPosition;
                }

                br.Close();
                fs.Close();
            }

            GridData aGridData = new GridData();
            aGridData.Data = gridData;
            aGridData.MissingValue = MissingValue;
            aGridData.X = X;
            aGridData.Y = new double[Times.Count];
            for (i = 0; i < Times.Count; i++)
                aGridData.Y[i] = DataConvert.ToDouble(Times[i]);

            return aGridData;
        }

        ///// <summary>
        ///// Read GrADS grid data - Time/Lon
        ///// </summary>
        ///// <param name="aFile"></param>
        ///// <param name="latIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="levelIdx"></param>
        ///// <param name="aGDataInfo"></param>
        ///// <returns></returns>
        //public double[,] ReadGrADSData_Grid_TimeLon(string aFile, int latIdx, int varIdx, int levelIdx,
        //    GrADSDataInfo aGDataInfo)
        //{
        //    int xNum, yNum;
        //    xNum = aGDataInfo.XNum;
        //    yNum = aGDataInfo.TDEF.TNum;
        //    double[,] gridData = new double[yNum, xNum];

        //    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    int i, j, lNum, t;
        //    byte[] aBytes;
        //    long aTPosition;

        //    for (t = 0; t < aGDataInfo.TDEF.TNum; t++)
        //    {
        //        br.BaseStream.Position = aGDataInfo.FILEHEADER + t * aGDataInfo._recLenPerTime;
        //        aTPosition = br.BaseStream.Position;

        //        for (i = 0; i < varIdx; i++)
        //        {
        //            lNum = aGDataInfo.VARDEF.Vars[i].LevelNum;
        //            if (lNum == 0)
        //            {
        //                lNum = 1;
        //            }
        //            br.BaseStream.Position += lNum * aGDataInfo._recordLen;
        //        }
        //        br.BaseStream.Position += levelIdx * aGDataInfo._recordLen;
        //        if (aGDataInfo.OPTIONS.sequential)
        //            br.BaseStream.Position += 4;
        //        br.BaseStream.Position += latIdx * xNum * 4;

        //        if (br.BaseStream.Position >= br.BaseStream.Length)
        //        {
        //            Console.WriteLine("Erro");
        //        }

        //        for (j = 0; j < xNum; j++)
        //        {
        //            aBytes = br.ReadBytes(4);
        //            if (aGDataInfo.OPTIONS.big_endian || aGDataInfo.OPTIONS.byteswapped)
        //            {
        //                Array.Reverse(aBytes);
        //            }
        //            gridData[t, j] = BitConverter.ToSingle(aBytes, 0);
        //        }
        //        //br.BaseStream.Position += (yNum - latIdx - 1) * xNum * 4;   
        //        br.BaseStream.Position = aTPosition;
        //    }

        //    br.Close();

        //    return gridData;
        //}

        /// <summary>
        /// Read GrADS grid data - Time/Lat
        /// </summary>        
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>       
        /// <returns></returns>
        public GridData GetGridData_TimeLat(int lonIdx, int varIdx, int levelIdx)
        {
            int xNum, yNum;
            xNum = YNum;
            yNum = TDEF.TNum;
            int i, j, lNum, t;
            double[,] gridData = new double[yNum, xNum];

            if (OPTIONS.template)
            {
                byte[] aBytes;
                long aTPosition;

                for (t = 0; t < TDEF.TNum; t++)
                {
                    object[] result = getFilePath_Template(t);
                    String filePath = (string)result[0];
                    int tIdx = (int)result[1];
                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    br.BaseStream.Position = (long)FILEHEADER + (long)tIdx * (long)_recLenPerTime;
                    aTPosition = br.BaseStream.Position;

                    for (i = 0; i < varIdx; i++)
                    {
                        lNum = VARDEF.Vars[i].LevelNum;
                        if (lNum == 0)
                        {
                            lNum = 1;
                        }
                        br.BaseStream.Position += lNum * _recordLen;
                    }
                    br.BaseStream.Position += levelIdx * _recordLen;
                    //br.BaseStream.Position += latIdx * xNum * 4;
                    if (OPTIONS.sequential)
                        br.BaseStream.Position += 4;

                    if (br.BaseStream.Position >= br.BaseStream.Length)
                    {
                        Console.WriteLine("Erro");
                    }

                    for (i = 0; i < YNum; i++)
                    {
                        for (j = 0; j < XNum; j++)
                        {
                            aBytes = br.ReadBytes(4);
                            if (j == lonIdx)
                            {
                                if (OPTIONS.big_endian || OPTIONS.byteswapped)
                                {
                                    Array.Reverse(aBytes);
                                }
                                gridData[t, i] = BitConverter.ToSingle(aBytes, 0);
                            }
                        }
                    }
                    //br.BaseStream.Position += (yNum - latIdx - 1) * xNum * 4;   
                    br.BaseStream.Position = aTPosition;
                    br.Close();
                    fs.Close();
                }
            }
            else
            {
                FileStream fs = new FileStream(DSET, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);                
                byte[] aBytes;
                long aTPosition;

                for (t = 0; t < TDEF.TNum; t++)
                {
                    br.BaseStream.Position = (long)FILEHEADER + (long)t * (long)_recLenPerTime;
                    aTPosition = br.BaseStream.Position;

                    for (i = 0; i < varIdx; i++)
                    {
                        lNum = VARDEF.Vars[i].LevelNum;
                        if (lNum == 0)
                        {
                            lNum = 1;
                        }
                        br.BaseStream.Position += lNum * _recordLen;
                    }
                    br.BaseStream.Position += levelIdx * _recordLen;
                    //br.BaseStream.Position += latIdx * xNum * 4;
                    if (OPTIONS.sequential)
                        br.BaseStream.Position += 4;

                    if (br.BaseStream.Position >= br.BaseStream.Length)
                    {
                        Console.WriteLine("Erro");
                    }

                    for (i = 0; i < YNum; i++)
                    {
                        for (j = 0; j < XNum; j++)
                        {
                            aBytes = br.ReadBytes(4);
                            if (j == lonIdx)
                            {
                                if (OPTIONS.big_endian || OPTIONS.byteswapped)
                                {
                                    Array.Reverse(aBytes);
                                }
                                gridData[t, i] = BitConverter.ToSingle(aBytes, 0);
                            }
                        }
                    }
                    //br.BaseStream.Position += (yNum - latIdx - 1) * xNum * 4;   
                    br.BaseStream.Position = aTPosition;
                }

                br.Close();
                fs.Close();
            }

            GridData aGridData = new GridData();
            aGridData.Data = gridData;
            aGridData.MissingValue = MissingValue;
            aGridData.X = Y;
            aGridData.Y = new double[Times.Count];
            for (i = 0; i < Times.Count; i++)
                aGridData.Y[i] = DataConvert.ToDouble(Times[i]);

            return aGridData;
        }

        ///// <summary>
        ///// Read GrADS grid data - Time/Lat
        ///// </summary>
        ///// <param name="aFile"></param>
        ///// <param name="lonIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="levelIdx"></param>
        ///// <param name="aGDataInfo"></param>
        ///// <returns></returns>
        //public double[,] ReadGrADSData_Grid_TimeLat(string aFile, int lonIdx, int varIdx, int levelIdx,
        //    GrADSDataInfo aGDataInfo)
        //{
        //    int xNum, yNum;
        //    xNum = aGDataInfo.YNum;
        //    yNum = aGDataInfo.TDEF.TNum;
        //    double[,] gridData = new double[yNum, xNum];

        //    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    int i, j, lNum, t;
        //    byte[] aBytes;
        //    long aTPosition;

        //    for (t = 0; t < aGDataInfo.TDEF.TNum; t++)
        //    {
        //        br.BaseStream.Position = aGDataInfo.FILEHEADER + t * aGDataInfo._recLenPerTime;
        //        aTPosition = br.BaseStream.Position;

        //        for (i = 0; i < varIdx; i++)
        //        {
        //            lNum = aGDataInfo.VARDEF.Vars[i].LevelNum;
        //            if (lNum == 0)
        //            {
        //                lNum = 1;
        //            }
        //            br.BaseStream.Position += lNum * aGDataInfo._recordLen;
        //        }
        //        br.BaseStream.Position += levelIdx * aGDataInfo._recordLen;
        //        //br.BaseStream.Position += latIdx * xNum * 4;
        //        if (aGDataInfo.OPTIONS.sequential)
        //            br.BaseStream.Position += 4;

        //        if (br.BaseStream.Position >= br.BaseStream.Length)
        //        {
        //            Console.WriteLine("Erro");
        //        }

        //        for (i = 0; i < aGDataInfo.YNum; i++)
        //        {
        //            for (j = 0; j < aGDataInfo.XNum; j++)
        //            {
        //                aBytes = br.ReadBytes(4);
        //                if (j == lonIdx)
        //                {
        //                    if (aGDataInfo.OPTIONS.big_endian || aGDataInfo.OPTIONS.byteswapped)
        //                    {
        //                        Array.Reverse(aBytes);
        //                    }
        //                    gridData[t, i] = BitConverter.ToSingle(aBytes, 0);
        //                }
        //            }
        //        }
        //        //br.BaseStream.Position += (yNum - latIdx - 1) * xNum * 4;   
        //        br.BaseStream.Position = aTPosition;
        //    }

        //    br.Close();

        //    return gridData;
        //}

        /// <summary>
        /// Read GrADS grid data - Level/Lat
        /// </summary>        
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="timeIdx"></param>       
        /// <returns></returns>
        public GridData GetGridData_LevelLat(int lonIdx, int varIdx, int timeIdx)
        {
            int xNum, yNum;
            xNum = YNum;
            yNum = VARDEF.Vars[varIdx].LevelNum;
            double[,] gridData = new double[yNum, xNum];

            string filePath = DSET;
            int tIdx = timeIdx;
            if (OPTIONS.template)
            {
                object[] result = getFilePath_Template(timeIdx);
                filePath = (string)result[0];
                tIdx = (int)result[1];
            }

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, j, lNum;
            byte[] aBytes;

            br.BaseStream.Position = FILEHEADER;
            br.BaseStream.Position += (long)tIdx * (long)_recLenPerTime;

            for (i = 0; i < varIdx; i++)
            {
                lNum = VARDEF.Vars[i].LevelNum;
                if (lNum == 0)
                {
                    lNum = 1;
                }
                br.BaseStream.Position += lNum * _recordLen;
            }
            //br.BaseStream.Position += levelIdx * aGDataInfo._recordLen * 4;                

            if (br.BaseStream.Position >= br.BaseStream.Length)
            {
                Console.WriteLine("Erro");
            }

            for (i = 0; i < yNum; i++)    //Levels
            {
                if (OPTIONS.sequential)
                    br.BaseStream.Position += 4;

                for (j = 0; j < YNum; j++)
                {
                    br.ReadBytes(lonIdx * 4);

                    aBytes = br.ReadBytes(4);
                    if (OPTIONS.big_endian || OPTIONS.byteswapped)
                    {
                        Array.Reverse(aBytes);
                    }
                    gridData[i, j] = BitConverter.ToSingle(aBytes, 0);

                    br.ReadBytes((XNum - lonIdx - 1) * 4);
                }
            }

            br.Close();
            fs.Close();

            GridData aGridData = new GridData();
            aGridData.Data = gridData;
            aGridData.MissingValue = MissingValue;
            aGridData.X = Y;
            double[] levels = new double[VARDEF.Vars[varIdx].LevelNum];
            for (i = 0; i < levels.Length; i++)
                levels[i] = ZDEF.ZLevels[i];
                 
            aGridData.Y = levels;

            return aGridData;
        }

        ///// <summary>
        ///// Read GrADS grid data - Level/Lat
        ///// </summary>
        ///// <param name="aFile"></param>
        ///// <param name="lonIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="tIdx"></param>
        ///// <param name="aGDataInfo"></param>
        ///// <returns></returns>
        //public double[,] ReadGrADSData_Grid_LevelLat(string aFile, int lonIdx, int varIdx, int tIdx,
        //    GrADSDataInfo aGDataInfo)
        //{
        //    int xNum, yNum;
        //    xNum = aGDataInfo.YNum;
        //    yNum = aGDataInfo.VARDEF.Vars[varIdx].LevelNum;
        //    double[,] gridData = new double[yNum, xNum];

        //    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    int i, j, lNum;
        //    byte[] aBytes;

        //    br.BaseStream.Position = aGDataInfo.FILEHEADER + tIdx * aGDataInfo._recLenPerTime;

        //    for (i = 0; i < varIdx; i++)
        //    {
        //        lNum = aGDataInfo.VARDEF.Vars[i].LevelNum;
        //        if (lNum == 0)
        //        {
        //            lNum = 1;
        //        }
        //        br.BaseStream.Position += lNum * aGDataInfo._recordLen;
        //    }
        //    //br.BaseStream.Position += levelIdx * aGDataInfo._recordLen * 4;                

        //    if (br.BaseStream.Position >= br.BaseStream.Length)
        //    {
        //        Console.WriteLine("Erro");
        //    }

        //    for (i = 0; i < yNum; i++)    //Levels
        //    {
        //        if (aGDataInfo.OPTIONS.sequential)
        //            br.BaseStream.Position += 4;

        //        for (j = 0; j < aGDataInfo.YNum; j++)
        //        {
        //            br.ReadBytes(lonIdx * 4);

        //            aBytes = br.ReadBytes(4);
        //            if (aGDataInfo.OPTIONS.big_endian || aGDataInfo.OPTIONS.byteswapped)
        //            {
        //                Array.Reverse(aBytes);
        //            }
        //            gridData[i, j] = BitConverter.ToSingle(aBytes, 0);

        //            br.ReadBytes((aGDataInfo.XNum - lonIdx - 1) * 4);
        //        }
        //    }

        //    br.Close();

        //    return gridData;
        //}

        /// <summary>
        /// Read GrADS grid data - Level/Lon
        /// </summary>        
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="timeIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_LevelLon(int latIdx, int varIdx, int timeIdx)
        {
            int xNum, yNum;
            xNum = XNum;
            yNum = VARDEF.Vars[varIdx].LevelNum;
            double[,] gridData = new double[yNum, xNum];

            string filePath = DSET;
            int tIdx = timeIdx;
            if (OPTIONS.template)
            {
                object[] result = getFilePath_Template(timeIdx);
                filePath = (string)result[0];
                tIdx = (int)result[1];
            }

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, j, lNum;
            byte[] aBytes;

            br.BaseStream.Position = FILEHEADER;
            br.BaseStream.Position += (long)tIdx * (long)_recLenPerTime;

            for (i = 0; i < varIdx; i++)
            {
                lNum = VARDEF.Vars[i].LevelNum;
                if (lNum == 0)
                {
                    lNum = 1;
                }
                br.BaseStream.Position += (long)lNum * (long)_recordLen;
            }

            if (br.BaseStream.Position >= br.BaseStream.Length)
            {
                Console.WriteLine("Erro");
            }

            for (i = 0; i < yNum; i++)    //Levels
            {
                if (OPTIONS.sequential)
                    br.BaseStream.Position += 4;
                br.BaseStream.Position += (long)latIdx * (long)xNum * 4;
                for (j = 0; j < xNum; j++)
                {
                    aBytes = br.ReadBytes(4);
                    if (OPTIONS.big_endian || OPTIONS.byteswapped)
                    {
                        Array.Reverse(aBytes);
                    }
                    gridData[i, j] = BitConverter.ToSingle(aBytes, 0);
                }
                br.BaseStream.Position += (YNum - latIdx - 1) * xNum * 4;
            }

            br.Close();
            fs.Close();

            GridData aGridData = new GridData();
            aGridData.Data = gridData;
            aGridData.MissingValue = MissingValue;
            aGridData.X = X;
            double[] levels = new double[VARDEF.Vars[varIdx].LevelNum];
            for (i = 0; i < levels.Length; i++)
                levels[i] = ZDEF.ZLevels[i];

            aGridData.Y = levels;

            return aGridData;
        }

        ///// <summary>
        ///// Read GrADS grid data - Level/Lon
        ///// </summary>
        ///// <param name="aFile"></param>
        ///// <param name="latIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="tIdx"></param>
        ///// <param name="aGDataInfo"></param>
        ///// <returns></returns>
        //public double[,] ReadGrADSData_Grid_LevelLon(string aFile, int latIdx, int varIdx, int tIdx,
        //    GrADSDataInfo aGDataInfo)
        //{
        //    int xNum, yNum;
        //    xNum = aGDataInfo.XNum;
        //    yNum = aGDataInfo.VARDEF.Vars[varIdx].LevelNum;
        //    double[,] gridData = new double[yNum, xNum];

        //    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    int i, j, lNum;
        //    byte[] aBytes;

        //    br.BaseStream.Position = aGDataInfo.FILEHEADER + tIdx * aGDataInfo._recLenPerTime;

        //    for (i = 0; i < varIdx; i++)
        //    {
        //        lNum = aGDataInfo.VARDEF.Vars[i].LevelNum;
        //        if (lNum == 0)
        //        {
        //            lNum = 1;
        //        }
        //        br.BaseStream.Position += lNum * aGDataInfo._recordLen;
        //    }

        //    if (br.BaseStream.Position >= br.BaseStream.Length)
        //    {
        //        Console.WriteLine("Erro");
        //    }

        //    for (i = 0; i < yNum; i++)    //Levels
        //    {
        //        if (aGDataInfo.OPTIONS.sequential)
        //            br.BaseStream.Position += 4;
        //        br.BaseStream.Position += latIdx * xNum * 4;
        //        for (j = 0; j < xNum; j++)
        //        {
        //            aBytes = br.ReadBytes(4);
        //            if (aGDataInfo.OPTIONS.big_endian || aGDataInfo.OPTIONS.byteswapped)
        //            {
        //                Array.Reverse(aBytes);
        //            }
        //            gridData[i, j] = BitConverter.ToSingle(aBytes, 0);
        //        }
        //        br.BaseStream.Position += (aGDataInfo.YNum - latIdx - 1) * xNum * 4;
        //    }

        //    br.Close();

        //    return gridData;
        //}

        /// <summary>
        /// Read GrADS grid data - Level/Time
        /// </summary>       
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="lonIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_LevelTime(int latIdx, int varIdx, int lonIdx)
        {
            int xNum, yNum;
            xNum = TDEF.TNum;
            yNum = VARDEF.Vars[varIdx].LevelNum;
            int i, lNum, t;
            double[,] gridData = new double[yNum, xNum];

            if (OPTIONS.template)
            {                
                byte[] aBytes;
                long aTPosition;

                for (t = 0; t < TDEF.TNum; t++)
                {
                    object[] result = getFilePath_Template(t);
                    String filePath = (string)result[0];
                    int tIdx = (int)result[1];
                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    br.BaseStream.Position = (long)FILEHEADER + (long)tIdx * (long)_recLenPerTime;
                    aTPosition = br.BaseStream.Position;

                    for (i = 0; i < varIdx; i++)
                    {
                        lNum = VARDEF.Vars[i].LevelNum;
                        if (lNum == 0)
                        {
                            lNum = 1;
                        }
                        br.BaseStream.Position += (long)lNum * (long)_recordLen;
                    }

                    if (br.BaseStream.Position >= br.BaseStream.Length)
                    {
                        Console.WriteLine("Erro");
                    }

                    for (i = 0; i < yNum; i++)    //Levels
                    {
                        if (OPTIONS.sequential)
                            br.BaseStream.Position += 4;
                        br.BaseStream.Position += latIdx * xNum * 4;
                        br.BaseStream.Position += lonIdx * 4;

                        aBytes = br.ReadBytes(4);
                        if (OPTIONS.big_endian || OPTIONS.byteswapped)
                        {
                            Array.Reverse(aBytes);
                        }
                        gridData[i, t] = BitConverter.ToSingle(aBytes, 0);

                        br.BaseStream.Position += (XNum - lonIdx - 1) * 4;
                        br.BaseStream.Position += (YNum - latIdx - 1) * xNum * 4;
                    }

                    br.Close();
                    fs.Close();
                }
            }
            else
            {
                FileStream fs = new FileStream(DSET, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);                
                byte[] aBytes;
                long aTPosition;

                for (t = 0; t < TDEF.TNum; t++)
                {
                    br.BaseStream.Position = (long)FILEHEADER + (long)t * (long)_recLenPerTime;
                    aTPosition = br.BaseStream.Position;

                    for (i = 0; i < varIdx; i++)
                    {
                        lNum = VARDEF.Vars[i].LevelNum;
                        if (lNum == 0)
                        {
                            lNum = 1;
                        }
                        br.BaseStream.Position += (long)lNum * (long)_recordLen;
                    }

                    if (br.BaseStream.Position >= br.BaseStream.Length)
                    {
                        Console.WriteLine("Erro");
                    }

                    for (i = 0; i < yNum; i++)    //Levels
                    {
                        if (OPTIONS.sequential)
                            br.BaseStream.Position += 4;
                        br.BaseStream.Position += latIdx * xNum * 4;
                        br.BaseStream.Position += lonIdx * 4;

                        aBytes = br.ReadBytes(4);
                        if (OPTIONS.big_endian || OPTIONS.byteswapped)
                        {
                            Array.Reverse(aBytes);
                        }
                        gridData[i, t] = BitConverter.ToSingle(aBytes, 0);

                        br.BaseStream.Position += (XNum - lonIdx - 1) * 4;
                        br.BaseStream.Position += (YNum - latIdx - 1) * xNum * 4;
                    }

                    br.BaseStream.Position = aTPosition;
                }

                br.Close();
                fs.Close();
            }

            GridData aGridData = new GridData();
            aGridData.Data = gridData;
            aGridData.MissingValue = MissingValue;
            aGridData.X = new double[Times.Count];            
            for (i = 0; i < Times.Count; i++)
                aGridData.X[i] = DataConvert.ToDouble(Times[i]);
            double[] levels = new double[VARDEF.Vars[varIdx].LevelNum];
            for (i = 0; i < levels.Length; i++)
                levels[i] = ZDEF.ZLevels[i];
            aGridData.Y = levels;

            return aGridData;
        }

        ///// <summary>
        ///// Read GrADS grid data - Level/Time
        ///// </summary>
        ///// <param name="aFile"></param>
        ///// <param name="latIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="lonIdx"></param>
        ///// <param name="aGDataInfo"></param>
        ///// <returns></returns>
        //public double[,] ReadGrADSData_Grid_LevelTime(string aFile, int latIdx, int varIdx, int lonIdx,
        //    GrADSDataInfo aGDataInfo)
        //{
        //    int xNum, yNum;
        //    xNum = aGDataInfo.TDEF.TNum;
        //    yNum = aGDataInfo.VARDEF.Vars[varIdx].LevelNum;
        //    double[,] gridData = new double[yNum, xNum];

        //    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    int i, lNum, t;
        //    byte[] aBytes;
        //    long aTPosition;

        //    for (t = 0; t < aGDataInfo.TDEF.TNum; t++)
        //    {
        //        br.BaseStream.Position = aGDataInfo.FILEHEADER + t * aGDataInfo._recLenPerTime;
        //        aTPosition = br.BaseStream.Position;

        //        for (i = 0; i < varIdx; i++)
        //        {
        //            lNum = aGDataInfo.VARDEF.Vars[i].LevelNum;
        //            if (lNum == 0)
        //            {
        //                lNum = 1;
        //            }
        //            br.BaseStream.Position += lNum * aGDataInfo._recordLen;
        //        }

        //        if (br.BaseStream.Position >= br.BaseStream.Length)
        //        {
        //            Console.WriteLine("Erro");
        //        }

        //        for (i = 0; i < yNum; i++)    //Levels
        //        {
        //            if (aGDataInfo.OPTIONS.sequential)
        //                br.BaseStream.Position += 4;
        //            br.BaseStream.Position += latIdx * xNum * 4;
        //            br.BaseStream.Position += lonIdx * 4;

        //            aBytes = br.ReadBytes(4);
        //            if (aGDataInfo.OPTIONS.big_endian || aGDataInfo.OPTIONS.byteswapped)
        //            {
        //                Array.Reverse(aBytes);
        //            }
        //            gridData[i, t] = BitConverter.ToSingle(aBytes, 0);

        //            br.BaseStream.Position += (aGDataInfo.XNum - lonIdx - 1) * 4;
        //            br.BaseStream.Position += (aGDataInfo.YNum - latIdx - 1) * xNum * 4;
        //        }

        //        br.BaseStream.Position = aTPosition;
        //    }

        //    br.Close();

        //    return gridData;
        //}

        /// <summary>
        /// Read GrADS grid data - Time
        /// </summary>        
        /// <param name="latIdx"></param>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_Time(int lonIdx, int latIdx, int varIdx, int levelIdx)
        {                       
            int i, lNum, t;
            byte[] aBytes;
            Single aValue;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = new double[TDEF.TNum];
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, TDEF.TNum];

            if (OPTIONS.template)
            {
                for (t = 0; t < TDEF.TNum; t++)
                {
                    object[] result = getFilePath_Template(t);
                    String filePath = (string)result[0];
                    int tIdx = (int)result[1];
                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    br.BaseStream.Position = (long)FILEHEADER + (long)tIdx * (long)_recLenPerTime;

                    for (i = 0; i < varIdx; i++)
                    {
                        lNum = VARDEF.Vars[i].LevelNum;
                        if (lNum == 0)
                        {
                            lNum = 1;
                        }
                        br.BaseStream.Position += (long)lNum * (long)_recordLen;
                    }
                    br.BaseStream.Position += (long)levelIdx * (long)_recordLen;
                    if (OPTIONS.sequential)
                        br.BaseStream.Position += 4;
                    br.BaseStream.Position += latIdx * XNum * 4;

                    if (br.BaseStream.Position >= br.BaseStream.Length)
                    {
                        Console.WriteLine("Erro");
                    }

                    br.BaseStream.Position += lonIdx * 4;

                    aBytes = br.ReadBytes(4);
                    if (OPTIONS.big_endian || OPTIONS.byteswapped)
                    {
                        Array.Reverse(aBytes);
                    }
                    aValue = BitConverter.ToSingle(aBytes, 0);
                    aGridData.X[t] = DataConvert.ToDouble(TDEF.times[t]);
                    aGridData.Data[0, t] = aValue;

                    br.Close();
                    fs.Close();
                }
            }
            else
            {
                FileStream fs = new FileStream(DSET, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                for (t = 0; t < TDEF.TNum; t++)
                {
                    br.BaseStream.Position = (long)FILEHEADER + (long)t * (long)_recLenPerTime;

                    for (i = 0; i < varIdx; i++)
                    {
                        lNum = VARDEF.Vars[i].LevelNum;
                        if (lNum == 0)
                        {
                            lNum = 1;
                        }
                        br.BaseStream.Position += (long)lNum * (long)_recordLen;
                    }
                    br.BaseStream.Position += (long)levelIdx * (long)_recordLen;
                    if (OPTIONS.sequential)
                        br.BaseStream.Position += 4;
                    br.BaseStream.Position += latIdx * XNum * 4;

                    if (br.BaseStream.Position >= br.BaseStream.Length)
                    {
                        Console.WriteLine("Erro");
                    }

                    br.BaseStream.Position += lonIdx * 4;

                    aBytes = br.ReadBytes(4);
                    if (OPTIONS.big_endian || OPTIONS.byteswapped)
                    {
                        Array.Reverse(aBytes);
                    }
                    aValue = BitConverter.ToSingle(aBytes, 0);
                    aGridData.X[t] = DataConvert.ToDouble(TDEF.times[t]);
                    aGridData.Data[0, t] = aValue;
                }

                br.Close();
                fs.Close();
            }

            return aGridData;
        }

        ///// <summary>
        ///// Read GrADS grid data - Time
        ///// </summary>
        ///// <param name="aFile"></param>
        ///// <param name="latIdx"></param>
        ///// <param name="lonIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="levelIdx"></param>
        ///// <param name="aGDataInfo"></param>
        ///// <returns></returns>
        //public List<PointD> ReadGrADSData_Grid_Time(string aFile, int latIdx, int lonIdx, int varIdx, int levelIdx,
        //    GrADSDataInfo aGDataInfo)
        //{
        //    PointD aPoint = new PointD();
        //    List<PointD> pointList = new List<PointD>();

        //    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    int i, lNum, t;
        //    byte[] aBytes;
        //    Single aValue;

        //    for (t = 0; t < aGDataInfo.TDEF.TNum; t++)
        //    {
        //        br.BaseStream.Position = aGDataInfo.FILEHEADER + t * aGDataInfo._recLenPerTime;

        //        for (i = 0; i < varIdx; i++)
        //        {
        //            lNum = aGDataInfo.VARDEF.Vars[i].LevelNum;
        //            if (lNum == 0)
        //            {
        //                lNum = 1;
        //            }
        //            br.BaseStream.Position += lNum * aGDataInfo._recordLen;
        //        }
        //        br.BaseStream.Position += levelIdx * aGDataInfo._recordLen;
        //        if (aGDataInfo.OPTIONS.sequential)
        //            br.BaseStream.Position += 4;
        //        br.BaseStream.Position += latIdx * aGDataInfo.XNum * 4;

        //        if (br.BaseStream.Position >= br.BaseStream.Length)
        //        {
        //            Console.WriteLine("Erro");
        //        }

        //        br.BaseStream.Position += lonIdx * 4;

        //        aBytes = br.ReadBytes(4);
        //        if (aGDataInfo.OPTIONS.big_endian || aGDataInfo.OPTIONS.byteswapped)
        //        {
        //            Array.Reverse(aBytes);
        //        }
        //        aValue = BitConverter.ToSingle(aBytes, 0);
        //        if (!(Math.Abs(aValue / aGDataInfo.MissingValue - 1) < 0.01))
        //        {
        //            aPoint.X = aGDataInfo.TDEF.times[t].ToBinary();
        //            aPoint.Y = aValue;
        //            pointList.Add(aPoint);
        //        }
        //    }

        //    br.Close();

        //    return pointList;
        //}

        /// <summary>
        /// Read GrADS grid data - Level
        /// </summary>        
        /// <param name="latIdx"></param>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="timeIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_Level( int lonIdx, int latIdx,int varIdx, int timeIdx)
        {
            string filePath = DSET;
            int tIdx = timeIdx;
            if (OPTIONS.template)
            {
                object[] result = getFilePath_Template(timeIdx);
                filePath = (string)result[0];
                tIdx = (int)result[1];
            }

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, lNum;
            byte[] aBytes;
            Single aValue;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = new double[ZDEF.ZNum];
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, ZDEF.ZNum];

            br.BaseStream.Position = FILEHEADER;
            br.BaseStream.Position += (long)tIdx * (long)_recLenPerTime;            

            for (i = 0; i < varIdx; i++)
            {
                lNum = VARDEF.Vars[i].LevelNum;
                if (lNum == 0)
                {
                    lNum = 1;
                }
                br.BaseStream.Position += (long)lNum * (long)_recordLen;
            }

            long aPosition = br.BaseStream.Position;

            for (i = 0; i < ZDEF.ZNum; i++)
            {
                br.BaseStream.Position = aPosition + (long)i * (long)_recordLen;
                if (OPTIONS.sequential)
                    br.BaseStream.Position += 4;
                br.BaseStream.Position += latIdx * XNum * 4;
                br.BaseStream.Position += lonIdx * 4;

                aBytes = br.ReadBytes(4);
                if (OPTIONS.big_endian || OPTIONS.byteswapped)
                {
                    Array.Reverse(aBytes);
                }
                aValue = BitConverter.ToSingle(aBytes, 0);
                aGridData.X[i] = ZDEF.ZLevels[i];
                aGridData.Data[0, i] = aValue;                
            }

            br.Close();
            fs.Close();

            return aGridData;
        }

        ///// <summary>
        ///// Read GrADS grid data - Level
        ///// </summary>
        ///// <param name="aFile"></param>
        ///// <param name="latIdx"></param>
        ///// <param name="lonIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="tIdx"></param>
        ///// <param name="aGDataInfo"></param>
        ///// <returns></returns>
        //public List<PointD> ReadGrADSData_Grid_Level(string aFile, int latIdx, int lonIdx, int varIdx, int tIdx,
        //    GrADSDataInfo aGDataInfo)
        //{
        //    PointD aPoint = new PointD();
        //    List<PointD> pointList = new List<PointD>();

        //    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    int i, lNum;
        //    byte[] aBytes;
        //    Single aValue;

        //    br.BaseStream.Position = aGDataInfo.FILEHEADER + tIdx * aGDataInfo._recLenPerTime;

        //    for (i = 0; i < varIdx; i++)
        //    {
        //        lNum = aGDataInfo.VARDEF.Vars[i].LevelNum;
        //        if (lNum == 0)
        //        {
        //            lNum = 1;
        //        }
        //        br.BaseStream.Position += lNum * aGDataInfo._recordLen;
        //    }

        //    long aPosition = br.BaseStream.Position;

        //    for (i = 0; i < aGDataInfo.ZDEF.ZNum; i++)
        //    {
        //        br.BaseStream.Position = aPosition + i * aGDataInfo._recordLen;
        //        if (aGDataInfo.OPTIONS.sequential)
        //            br.BaseStream.Position += 4;
        //        br.BaseStream.Position += latIdx * aGDataInfo.XNum * 4;
        //        br.BaseStream.Position += lonIdx * 4;

        //        aBytes = br.ReadBytes(4);
        //        if (aGDataInfo.OPTIONS.big_endian || aGDataInfo.OPTIONS.byteswapped)
        //        {
        //            Array.Reverse(aBytes);
        //        }
        //        aValue = BitConverter.ToSingle(aBytes, 0);
        //        if (!(Math.Abs(aValue / aGDataInfo.MissingValue - 1) < 0.01))
        //        {
        //            aPoint.Y = aGDataInfo.ZDEF.ZLevels[i];
        //            aPoint.X = aValue;
        //            pointList.Add(aPoint);
        //        }
        //    }

        //    br.Close();

        //    return pointList;
        //}

        /// <summary>
        /// Read GrADS grid data - Longitude
        /// </summary>        
        /// <param name="latIdx"></param>
        /// <param name="levelIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="timeIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_Lon(int timeIdx, int latIdx, int varIdx, int levelIdx)
        {
            string filePath = DSET;
            int tIdx = timeIdx;
            if (OPTIONS.template)
            {
                object[] result = getFilePath_Template(timeIdx);
                filePath = (string)result[0];
                tIdx = (int)result[1];
            }

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, lNum;
            byte[] aBytes;
            Single aValue;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = X;
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, X.Length];

            br.BaseStream.Position = FILEHEADER;
            br.BaseStream.Position += (long)tIdx * (long)_recLenPerTime;            

            for (i = 0; i < varIdx; i++)
            {
                lNum = VARDEF.Vars[i].LevelNum;
                if (lNum == 0)
                {
                    lNum = 1;
                }
                br.BaseStream.Position += (long)lNum * (long)_recordLen;
            }
            br.BaseStream.Position += (long)levelIdx * (long)_recordLen;
            if (OPTIONS.sequential)
                br.BaseStream.Position += 4;
            br.BaseStream.Position += latIdx * XNum * 4;

            for (i = 0; i < XNum; i++)
            {
                aBytes = br.ReadBytes(4);
                if (OPTIONS.big_endian || OPTIONS.byteswapped)
                {
                    Array.Reverse(aBytes);
                }
                aValue = BitConverter.ToSingle(aBytes, 0);
                aGridData.Data[0, i] = aValue;                
            }

            br.Close();
            fs.Close();

            return aGridData;
        }

        ///// <summary>
        ///// Read GrADS grid data - Longitude
        ///// </summary>
        ///// <param name="aFile"></param>
        ///// <param name="latIdx"></param>
        ///// <param name="levelIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="tIdx"></param>
        ///// <param name="aGDataInfo"></param>
        ///// <returns></returns>
        //public List<PointD> ReadGrADSData_Grid_Lon(string aFile, int latIdx, int levelIdx, int varIdx, int tIdx,
        //    GrADSDataInfo aGDataInfo)
        //{
        //    PointD aPoint = new PointD();
        //    List<PointD> pointList = new List<PointD>();

        //    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    int i, lNum;
        //    byte[] aBytes;
        //    Single aValue;

        //    br.BaseStream.Position = aGDataInfo.FILEHEADER + tIdx * aGDataInfo._recLenPerTime;
        //    for (i = 0; i < varIdx; i++)
        //    {
        //        lNum = aGDataInfo.VARDEF.Vars[i].LevelNum;
        //        if (lNum == 0)
        //        {
        //            lNum = 1;
        //        }
        //        br.BaseStream.Position += lNum * aGDataInfo._recordLen;
        //    }
        //    br.BaseStream.Position += levelIdx * aGDataInfo._recordLen;
        //    if (aGDataInfo.OPTIONS.sequential)
        //        br.BaseStream.Position += 4;
        //    br.BaseStream.Position += latIdx * aGDataInfo.XNum * 4;

        //    for (i = 0; i < aGDataInfo.XNum; i++)
        //    {
        //        aBytes = br.ReadBytes(4);
        //        if (aGDataInfo.OPTIONS.big_endian || aGDataInfo.OPTIONS.byteswapped)
        //        {
        //            Array.Reverse(aBytes);
        //        }
        //        aValue = BitConverter.ToSingle(aBytes, 0);
        //        if (!(Math.Abs(aValue / aGDataInfo.MissingValue - 1) < 0.01))
        //        {
        //            aPoint.X = aGDataInfo.XDEF.X[i];
        //            aPoint.Y = aValue;
        //            pointList.Add(aPoint);
        //        }
        //    }

        //    br.Close();

        //    return pointList;
        //}

        /// <summary>
        /// Read GrADS data - Latitude
        /// </summary>        
        /// <param name="lonIdx"></param>
        /// <param name="levelIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="timeIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_Lat(int timeIdx, int lonIdx, int varIdx, int levelIdx)
        {
            string filePath = DSET;
            int tIdx = timeIdx;
            if (OPTIONS.template)
            {
                object[] result = getFilePath_Template(timeIdx);
                filePath = (string)result[0];
                tIdx = (int)result[1];
            }

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, lNum;
            byte[] aBytes;
            Single aValue;

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = Y;
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, Y.Length];

            br.BaseStream.Position = FILEHEADER;
            br.BaseStream.Position += (long)tIdx * (long)_recLenPerTime;            

            for (i = 0; i < varIdx; i++)
            {
                lNum = VARDEF.Vars[i].LevelNum;
                if (lNum == 0)
                {
                    lNum = 1;
                }
                br.BaseStream.Position += (long)lNum * (long)_recordLen;
            }
            br.BaseStream.Position += (long)levelIdx * (long)_recordLen;
            if (OPTIONS.sequential)
                br.BaseStream.Position += 4;
            long aPosition = br.BaseStream.Position;

            for (i = 0; i < YNum; i++)
            {
                br.BaseStream.Position = aPosition + (long)i * (long)XNum * 4;
                br.BaseStream.Position += lonIdx * 4;
                aBytes = br.ReadBytes(4);
                if (OPTIONS.big_endian || OPTIONS.byteswapped)
                {
                    Array.Reverse(aBytes);
                }
                aValue = BitConverter.ToSingle(aBytes, 0);
                aGridData.Data[0, i] = aValue;                
            }

            br.Close();
            fs.Close();

            return aGridData;
        }

        ///// <summary>
        ///// Read GrADS data - Latitude
        ///// </summary>
        ///// <param name="aFile"></param>
        ///// <param name="lonIdx"></param>
        ///// <param name="levelIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="tIdx"></param>
        ///// <param name="aGDataInfo"></param>
        ///// <returns></returns>
        //public List<PointD> ReadGrADSData_Grid_Lat(string aFile, int lonIdx, int levelIdx, int varIdx, int tIdx,
        //    GrADSDataInfo aGDataInfo)
        //{
        //    PointD aPoint = new PointD();
        //    List<PointD> pointList = new List<PointD>();

        //    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    int i, lNum;
        //    byte[] aBytes;
        //    Single aValue;

        //    br.BaseStream.Position = aGDataInfo.FILEHEADER + tIdx * aGDataInfo._recLenPerTime;
        //    for (i = 0; i < varIdx; i++)
        //    {
        //        lNum = aGDataInfo.VARDEF.Vars[i].LevelNum;
        //        if (lNum == 0)
        //        {
        //            lNum = 1;
        //        }
        //        br.BaseStream.Position += lNum * aGDataInfo._recordLen;
        //    }
        //    br.BaseStream.Position += levelIdx * aGDataInfo._recordLen;
        //    if (aGDataInfo.OPTIONS.sequential)
        //        br.BaseStream.Position += 4;
        //    long aPosition = br.BaseStream.Position;

        //    for (i = 0; i < aGDataInfo.YNum; i++)
        //    {
        //        br.BaseStream.Position = aPosition + i * aGDataInfo.XNum * 4;
        //        br.BaseStream.Position += lonIdx * 4;
        //        aBytes = br.ReadBytes(4);
        //        if (aGDataInfo.OPTIONS.big_endian || aGDataInfo.OPTIONS.byteswapped)
        //        {
        //            Array.Reverse(aBytes);
        //        }
        //        aValue = BitConverter.ToSingle(aBytes, 0);
        //        if (!(Math.Abs(aValue / aGDataInfo.MissingValue - 1) < 0.01))
        //        {
        //            aPoint.X = aGDataInfo.YDEF.Y[i];
        //            aPoint.Y = aValue;
        //            pointList.Add(aPoint);
        //        }
        //    }

        //    br.Close();

        //    return pointList;
        //}

        /// <summary>
        /// Read GrADS station data
        /// </summary>       
        /// <param name="timeIdx">time index</param>            
        /// <returns>station data list</returns>
        public List<STData> ReadGrADSData_Station(int timeIdx)
        {
            List<STData> stDataList = new List<STData>();

            string filePath = DSET;
            int tIdx = timeIdx;
            if (OPTIONS.template)
            {
                object[] result = getFilePath_Template(timeIdx);
                filePath = (string)result[0];
                tIdx = (int)result[1];
            }

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, j, stNum, tNum;
            STDataHead aSTDH = new STDataHead();
            STLevData aSTLevData = new STLevData();
            STData aSTData = new STData();
            int varNum = VARDEF.VNum;
            int uVarNum = UpperVariables.Count;
            if (uVarNum > 0)
                varNum = varNum - uVarNum;
            byte[] aBytes;

            bool isBigEndian = false;
            if (OPTIONS.big_endian || OPTIONS.byteswapped)
                isBigEndian = true;

            stNum = 0;
            tNum = 0;
            if (OPTIONS.template)
            {
                timeIdx = 0;
            }
            do
            {
                aBytes = GetByteArray(br, 8, isBigEndian);
                aSTDH.STID = System.Text.Encoding.Default.GetString(aBytes);

                aBytes = GetByteArray(br, 4, isBigEndian);
                aSTDH.Lat = BitConverter.ToSingle(aBytes, 0);

                aBytes = GetByteArray(br, 4, isBigEndian);
                aSTDH.Lon = BitConverter.ToSingle(aBytes, 0);

                aBytes = GetByteArray(br, 4, isBigEndian);
                aSTDH.T = BitConverter.ToSingle(aBytes, 0);

                aBytes = GetByteArray(br, 4, isBigEndian);
                aSTDH.NLev = BitConverter.ToInt32(aBytes, 0);

                aBytes = GetByteArray(br, 4, isBigEndian);
                aSTDH.Flag = BitConverter.ToInt32(aBytes, 0);
                if (aSTDH.NLev > 0)
                {
                    stNum += 1;
                    aSTData.STHead = aSTDH;
                    aSTData.dataList = new List<STLevData>();
                    if (aSTDH.Flag == 1)    //Has ground level
                    {
                        aSTLevData.data = new Single[varNum];
                        for (i = 0; i < varNum; i++)
                        {
                            aBytes = GetByteArray(br, 4, isBigEndian);
                            aSTLevData.data[i] = BitConverter.ToSingle(aBytes, 0);
                        }
                        aSTLevData.lev = 0;
                        aSTData.dataList.Add(aSTLevData);
                    }
                    if (aSTDH.NLev - aSTDH.Flag > 0)    //Has upper level
                    {
                        for (i = 0; i < aSTDH.NLev - aSTDH.Flag; i++)
                        {
                            aBytes = GetByteArray(br, 4, isBigEndian);
                            aSTLevData.lev = BitConverter.ToSingle(aBytes, 0);
                            aSTLevData.data = new Single[uVarNum];
                            for (j = 0; j < uVarNum; j++)
                            {
                                aBytes = GetByteArray(br, 4, isBigEndian);
                                aSTLevData.data[j] = BitConverter.ToSingle(aBytes, 0);
                            }
                            aSTData.dataList.Add(aSTLevData);
                        }
                    }

                    if (tNum == timeIdx)
                    {
                        stDataList.Add(aSTData);
                    }
                }
                else    //End of time seriel
                {
                    //if (stNum > 0)    //Not end of the file
                    //{
                    stNum = 0;
                    if (tNum == timeIdx)
                    {
                        break;
                    }
                    tNum += 1;
                    if (br.BaseStream.Position + 28 >= br.BaseStream.Length)
                    {
                        break;
                    }
                    //}
                    //else       //End of the file
                    //{
                    //    break;
                    //}
                }
            }
            while (true);

            br.Close();
            fs.Close();

            return stDataList;
        }

        /// <summary>
        /// Read GrADS station data
        /// </summary>       
        /// <param name="vIdx">variable index</param>
        /// <param name="stID">statin identifer</param>   
        /// <returns>station data list</returns>
        public GridData GetGridData_Station(int vIdx, string stID)
        {
            GridData gData = new GridData();
            gData.MissingValue = MissingValue;            

            FileStream fs = new FileStream(DSET, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, stNum, tNum;
            STDataHead aSTDH = new STDataHead();
            STLevData aSTLevData = new STLevData();
            STData aSTData = new STData();
            Variable aVar = UpperVariables[vIdx];
            int varNum = VARDEF.VNum;
            int uVarNum = UpperVariables.Count;
            if (uVarNum > 0)
                varNum = varNum - uVarNum;
            byte[] aBytes;

            bool isBigEndian = false;
            if (OPTIONS.big_endian || OPTIONS.byteswapped)
                isBigEndian = true;

            gData.X = new double[Times.Count];
            for (i = 0; i < Times.Count; i++)
                gData.X[i] = DataConvert.ToDouble(Times[i]);

            gData.Y = new double[aVar.LevelNum];
            for (i = 0; i < aVar.LevelNum; i++)
                gData.Y[i] = i + 1;

            gData.Data = new double[aVar.LevelNum, Times.Count];

            stNum = 0;
            tNum = 0;
            do
            {
                aBytes = GetByteArray(br, 8, isBigEndian);
                aSTDH.STID = System.Text.Encoding.Default.GetString(aBytes);

                aBytes = GetByteArray(br, 4, isBigEndian);
                aSTDH.Lat = BitConverter.ToSingle(aBytes, 0);

                aBytes = GetByteArray(br, 4, isBigEndian);
                aSTDH.Lon = BitConverter.ToSingle(aBytes, 0);

                aBytes = GetByteArray(br, 4, isBigEndian);
                aSTDH.T = BitConverter.ToSingle(aBytes, 0);

                aBytes = GetByteArray(br, 4, isBigEndian);
                aSTDH.NLev = BitConverter.ToInt32(aBytes, 0);

                aBytes = GetByteArray(br, 4, isBigEndian);
                aSTDH.Flag = BitConverter.ToInt32(aBytes, 0);

                if (aSTDH.NLev > 0)
                {
                    stNum += 1;
                    aSTData.STHead = aSTDH;
                    aSTData.dataList = new List<STLevData>();
                    if (aSTDH.Flag == 1)    //Has ground level
                    {
                        if (aSTDH.STID == stID)
                        {
                            aSTLevData.data = new Single[varNum];
                            for (i = 0; i < varNum; i++)
                            {
                                aBytes = GetByteArray(br, 4, isBigEndian);
                                aSTLevData.data[i] = BitConverter.ToSingle(aBytes, 0);
                            }
                            aSTLevData.lev = 0;
                            aSTData.dataList.Add(aSTLevData);
                        }
                        else
                            br.ReadBytes(varNum * 4);
                    }
                    if (aSTDH.NLev - aSTDH.Flag > 0)    //Has upper level
                    {
                        if (aSTDH.STID == stID)
                        {
                            for (i = 0; i < aSTDH.NLev - aSTDH.Flag; i++)
                            {
                                br.ReadBytes(4 + vIdx * 4);
                                aBytes = GetByteArray(br, 4, isBigEndian);
                                gData.Data[i, tNum] = BitConverter.ToSingle(aBytes, 0);
                                br.ReadBytes((uVarNum - vIdx - 1) * 4);
                            }
                        }
                        else
                            br.ReadBytes((aSTDH.NLev - aSTDH.Flag) * (uVarNum + 1) * 4);
                    }
                }
                else    //End of time seriel
                {
                    stNum = 0;
                    if (tNum == Times.Count - 1)
                    {
                        break;
                    }
                    tNum += 1;
                    if (br.BaseStream.Position + 28 >= br.BaseStream.Length)
                    {
                        break;
                    }
                }
            }
            while (true);

            br.Close();
            fs.Close();

            return gData;
        }

        private byte[] GetByteArray(BinaryReader br, int n, bool isBigEndian)
        {
            byte[] aBytes = br.ReadBytes(n);
            if (isBigEndian)
                Array.Reverse(aBytes);

            return aBytes;
        }

        /// <summary>
        /// Get Upper Level stations
        /// </summary>
        /// <param name="timeIdx">time index</param>
        /// <returns>upper level stations</returns>
        public List<string> GetUpperLevelStations(int timeIdx)
        {
            List<STData> stDataList = ReadGrADSData_Station(timeIdx);
            List<string> stations = new List<string>();
            foreach (STData aSTData in stDataList)
            {
                if (aSTData.STHead.NLev - aSTData.STHead.Flag > 0)
                    stations.Add(aSTData.STHead.STID);
            }

            return stations;
        }

        /// <summary>
        /// Write grid data to a GrADS binary data file
        /// </summary>
        /// <param name="gridData">grid data</param>
        public void WriteGridData(GridData gridData)
        {
            WriteGrADSData_Grid(_bw, gridData.Data);
        }

        /// <summary>
        /// Write grid data to a GrADS binary data file
        /// </summary>
        /// <param name="gridData">grid data</param>
        public void WriteGridData(double[,] gridData)
        {
            WriteGrADSData_Grid(_bw, gridData);
        }

        ///// <summary>
        ///// Write GrADS grid data
        ///// </summary>        
        ///// <param name="gridData">Grid data</param>
        //public void WriteGrADSData_Grid(double[,] gridData)
        //{
        //    FileStream fs = new FileStream(DSET, FileMode.Create, FileAccess.Write);
        //    BinaryWriter bw = new BinaryWriter(fs);

        //    WriteGrADSData_Grid(bw, gridData);

        //    bw.Close();
        //    fs.Close();
        //}

        /// <summary>
        /// Write GrADS grid data
        /// </summary>
        /// <param name="aFile">File name</param>
        /// <param name="gridData">Grid data</param>
        public void WriteGrADSData_Grid(string aFile, double[,] gridData)
        {
            FileStream fs = new FileStream(aFile, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            WriteGrADSData_Grid(bw, gridData);

            bw.Close();
            fs.Close();
        }

        /// <summary>
        /// Write GrADS grid data
        /// </summary>
        /// <param name="bw">File name</param>
        /// <param name="gridData">Grid data</param>
        public void WriteGrADSData_Grid(BinaryWriter bw, double[,] gridData)
        {            
            int i, j;
            Single aData;

            for (i = 0; i < gridData.GetLength(0); i++)
            {
                for (j = 0; j < gridData.GetLength(1); j++)
                {
                    aData = (Single)gridData[i, j];
                    bw.Write(aData);
                }
            }                     
        }

        /// <summary>
        /// Write undefine grid data GrADS data file
        /// </summary>
        public void WriteGridData_Null()
        {
            WriteGrADSData_Grid_Null(_bw);
        }

        /// <summary>
        /// Write undefine grid data to GrADS file
        /// </summary>
        /// <param name="bw">binary writer</param>
        public void WriteGrADSData_Grid_Null(BinaryWriter bw)
        {
            double[,] gridData = new double[YDEF.YNum, XDEF.XNum];
            for (int i = 0; i < YDEF.YNum; i++)
            {
                for (int j = 0; j < XDEF.XNum; j++)
                {
                    gridData[i, j] = MissingValue;
                }
            }

            WriteGrADSData_Grid(bw, gridData);
        }

        /// <summary>
        /// Write GrADS station data
        /// </summary>
        /// <param name="aFile">Output GrADS station binary data file</param>
        /// <param name="aM1DataInfo">The MICAPS 1 data info</param>
        public void WriteGrADSStationData(String aFile, MICAPS1DataInfo aM1DataInfo)
        {
            List<int> varIdxList = new List<int>();
            int varNum = aM1DataInfo.varNum - 5;
            for (int i = 0; i < varNum; i++)
            {
                varIdxList.Add(i);
            }

            this.WriteGrADSStationData(aFile, aM1DataInfo, varIdxList);
        }

        /// <summary>
        /// Write GrADS station data
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <param name="aM1DataInfo">MICAPS 1 data info</param>
        /// <param name="varIdxList">variable index list</param>
        public void WriteGrADSStationData(string aFile, MICAPS1DataInfo aM1DataInfo, List<int> varIdxList)
        {
            FileStream fs = new FileStream(aFile, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            WriteGrADSStationData(bw, aM1DataInfo, varIdxList);

            //Close
            bw.Close();
            fs.Close();
        }

        /// <summary>
        /// Write GrADS station data
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="aM1DataInfo"></param>
        /// <param name="varIdxList"></param>
        public void WriteGrADSStationData(BinaryWriter bw, MICAPS1DataInfo aM1DataInfo,
            List<int> varIdxList)
        {
            int i, j;
            string aStid = "11111";
            Single lon, lat, t, value;
            int nLev, flag;
            lon = 0;
            lat = 0;
            t = 0;
            nLev = 1;
            flag = 1;    //Has ground level
            List<string> dataList = new List<string>();
            int varNum = varIdxList.Count;
            int varIdx;
            char[] st = new char[8];
            ASCIItoBin CASCIItoBin = new ASCIItoBin();
            byte[] stBytes = new byte[8];

            for (i = 0; i < aM1DataInfo.DataList.Count; i++)
            {
                dataList = aM1DataInfo.DataList[i];
                aStid = (string)dataList[0];
                lon = Convert.ToSingle(dataList[1]);
                lat = Convert.ToSingle(dataList[2]);

                //Write head  
                aStid = aStid.PadRight(8);
                st = aStid.ToCharArray();
                bw.Write(st);
                //stBytes = CASCIItoBin.ConvertToByte(aStid, 8);
                //bw.Write(stBytes);
                bw.Write(lat);
                bw.Write(lon);
                bw.Write(t);
                bw.Write(nLev);
                bw.Write(flag);

                //Write data
                for (j = 0; j < varNum; j++)
                {
                    varIdx = varIdxList[j];
                    if (varIdx != 20 && varIdx != 21)
                    {
                        value = Convert.ToSingle(dataList[varIdx + 5]);
                        bw.Write(value);
                    }
                }
            }
            nLev = 0;    //End of a time
            //Write time end head
            aStid = aStid.PadRight(8);
            st = aStid.ToCharArray();
            bw.Write(st);
            //stBytes = CASCIItoBin.ConvertToByte(aStid, 8);
            //bw.Write(stBytes);
            bw.Write(lat);
            bw.Write(lon);
            bw.Write(t);
            bw.Write(nLev);
            bw.Write(flag);
        }

        /// <summary>
        /// Write GrADS station data
        /// </summary>
        /// <param name="stInfoData">Station info data</param>
        public void WriteStationData(StationInfoData stInfoData)
        {
            WriteStationData(_bw, stInfoData);
        }

        /// <summary>
        /// Write GrADS station data
        /// </summary>
        /// <param name="bw">Binary writer</param>
        /// <param name="stInfoData">Station info data</param>
        private void WriteStationData(BinaryWriter bw, StationInfoData stInfoData)
        {
            int i, j;
            string aStid = "11111";
            Single lon, lat, t, value;
            int nLev, flag;
            lon = 0;
            lat = 0;
            t = 0;
            nLev = 1;
            flag = 1;    //Has ground level
            List<string> dataList = new List<string>();
            int varNum = stInfoData.Variables.Count;
            char[] st = new char[8];
            byte[] stBytes = new byte[8];

            for (i = 0; i < stInfoData.DataList.Count; i++)
            {
                dataList = stInfoData.DataList[i];
                aStid = dataList[0];
                lon = float.Parse(dataList[1]);
                lat = float.Parse(dataList[2]);

                //Write head  
                aStid = aStid.PadRight(8);
                st = aStid.ToCharArray();
                bw.Write(st);
                bw.Write(lat);
                bw.Write(lon);
                bw.Write(t);
                bw.Write(nLev);
                bw.Write(flag);

                //Write data
                for (j = 0; j < varNum; j++)
                {
                    value = Convert.ToSingle(dataList[j + 3]);
                    bw.Write(value);
                }
            }
            nLev = 0;    //End of a time
            //Write time end head
            aStid = aStid.PadRight(8);
            st = aStid.ToCharArray();
            bw.Write(st);
            bw.Write(lat);
            bw.Write(lon);
            bw.Write(t);
            bw.Write(nLev);
            bw.Write(flag);
        }

        /// <summary>
        /// Write GrADS station data null (no data at that time)
        /// </summary>
        /// <param name="bw"></param>
        public void WriteGrADSStationDataNull(BinaryWriter bw)
        {
            string aStid = "11111";
            Single lon, lat, t;
            int nLev, flag;
            lon = 0;
            lat = 0;
            t = 0;
            nLev = 1;
            flag = 1;    //Has ground level
            ArrayList dataList = new ArrayList();
            char[] st = new char[8];

            nLev = 0;    //End of a time
            //Write time end head
            aStid = aStid.PadRight(8);
            st = aStid.ToCharArray();
            bw.Write(st);
            bw.Write(lat);
            bw.Write(lon);
            bw.Write(t);
            bw.Write(nLev);
            bw.Write(flag);
        }

        /// <summary>
        /// Read station data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Station data</returns>
        public StationData GetStationData(int timeIdx, int varIdx, int levelIdx)
        {
            if (levelIdx == 0)
            {
                List<STData> stationData = ReadGrADSData_Station(timeIdx);
                return GetGroundStationData(stationData, varIdx);
            }
            else
            {
                return null;
            }
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
        /// <returns>Station model data</returns>
        public StationModelData GetStationModelData(int timeIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Generate ground discrete data
        /// </summary>
        /// <param name="stDataList"></param>
        /// <param name="varIdx"></param>
        /// <param name="dataExtent"></param>
        /// <returns></returns>
        public double[,] GetGroundDiscretedData(List<STData> stDataList, int varIdx, ref Extent dataExtent)
        {
            double[,] discretedData;
            ArrayList disDataList = new ArrayList();
            STLevData aSTLevData = new STLevData();
            Single lon, lat, aValue;
            Single minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;

            foreach (STData aSTData in stDataList)
            {
                if (aSTData.STHead.Flag == 1)
                {
                    lon = aSTData.STHead.Lon;
                    lat = aSTData.STHead.Lat;
                    aSTLevData = (STLevData)aSTData.dataList[0];
                    aValue = aSTLevData.data[varIdx];
                    disDataList.Add(new Single[] { lon, lat, aValue });
                }
            }

            discretedData = new double[3, disDataList.Count];
            int i = 0;
            foreach (Single[] disData in disDataList)
            {
                discretedData[0, i] = disData[0];
                discretedData[1, i] = disData[1];
                discretedData[2, i] = disData[2];
                if (i == 0)
                {
                    minX = disData[0];
                    maxX = minX;
                    minY = disData[1];
                    maxY = minY;
                }
                else
                {
                    if (minX > disData[0])
                    {
                        minX = disData[0];
                    }
                    else if (maxX < disData[0])
                    {
                        maxX = disData[0];
                    }
                    if (minY > disData[1])
                    {
                        minY = disData[1];
                    }
                    else if (maxY < disData[1])
                    {
                        maxY = disData[1];
                    }
                }
                i++;
            }
            dataExtent.minX = minX;
            dataExtent.maxX = maxX;
            dataExtent.minY = minY;
            dataExtent.maxY = maxY;

            return discretedData;
        }

        /// <summary>
        /// Get ground station data
        /// </summary>
        /// <param name="stDataList">station data list</param>
        /// <param name="varIdx">variable index</param>        
        /// <returns>station data</returns>
        public StationData GetGroundStationData(List<STData> stDataList, int varIdx)
        {
            StationData stationData = new StationData();
            double[,] discretedData;
            List<float[]> disDataList = new List<float[]>();
            STLevData aSTLevData = new STLevData();
            Single lon, lat, aValue;
            Single minX, maxX, minY, maxY;
            string stid;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            List<string> stations = new List<string>();

            foreach (STData aSTData in stDataList)
            {
                if (aSTData.STHead.Flag == 1)
                {
                    stid = aSTData.STHead.STID;
                    lon = aSTData.STHead.Lon;
                    lat = aSTData.STHead.Lat;
                    aSTLevData = (STLevData)aSTData.dataList[0];
                    aValue = aSTLevData.data[varIdx];
                    stations.Add(stid);
                    disDataList.Add(new float[] { lon, lat, aValue });
                }
            }

            discretedData = new double[3, disDataList.Count];
            int i = 0;
            foreach (float[] disData in disDataList)
            {
                discretedData[0, i] = disData[0];
                discretedData[1, i] = disData[1];
                discretedData[2, i] = disData[2];
                if (i == 0)
                {
                    minX = disData[0];
                    maxX = minX;
                    minY = disData[1];
                    maxY = minY;
                }
                else
                {
                    if (minX > disData[0])
                    {
                        minX = disData[0];
                    }
                    else if (maxX < disData[0])
                    {
                        maxX = disData[0];
                    }
                    if (minY > disData[1])
                    {
                        minY = disData[1];
                    }
                    else if (maxY < disData[1])
                    {
                        maxY = disData[1];
                    }
                }
                i++;
            }
            Extent dataExtent = new Extent();
            dataExtent.minX = minX;
            dataExtent.maxX = maxX;
            dataExtent.minY = minY;
            dataExtent.maxY = maxY;

            stationData.Data = discretedData;
            stationData.DataExtent = dataExtent;
            stationData.Stations = stations;
            stationData.MissingValue = MissingValue;

            return stationData;
        }

        /// <summary>
        /// Get ground station info data
        /// </summary>
        /// <param name="timeIdx">Time index</param>        
        /// <returns>Station info data</returns>
        public StationInfoData GetGroundStationInfoData(int timeIdx)
        {
            List<STData> stDataList = ReadGrADSData_Station(timeIdx);
            return GetGroundStationInfoData(stDataList);
        }

        /// <summary>
        /// Get ground station info data
        /// </summary>
        /// <param name="stDataList">station data list</param>        
        /// <returns>station info data</returns>
        public StationInfoData GetGroundStationInfoData(List<STData> stDataList)
        {
            StationInfoData stInfoData = new StationInfoData();
            stInfoData.Variables = this.VariableNames;
            List<string> fields = new List<string>();
            fields.Add("Stid");
            fields.Add("Lon");
            fields.Add("Lat");
            foreach (string f in this.VariableNames)
                fields.Add(f);

            stInfoData.Fields = fields;

            STLevData aSTLevData = new STLevData();
            float lon, lat;
            string stid;
            List<string> dataList;

            foreach (STData aSTData in stDataList)
            {
                if (aSTData.STHead.Flag == 1)
                {                    
                    stid = aSTData.STHead.STID;
                    lon = aSTData.STHead.Lon;
                    lat = aSTData.STHead.Lat;
                    aSTLevData = (STLevData)aSTData.dataList[0];
                    dataList = new List<string>();
                    dataList.Add(stid);
                    dataList.Add(lon.ToString());
                    dataList.Add(lat.ToString());
                    foreach (float value in aSTLevData.data)
                        dataList.Add(value.ToString());

                    stInfoData.DataList.Add(dataList);
                }
            }            

            return stInfoData;
        }        

        /// <summary>
        /// Generate data info text
        /// </summary>
        /// <returns>Data info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;

            dataInfo = "Title: " + TITLE;
            dataInfo += Environment.NewLine + "Descriptor: " + DESCRIPTOR;
            dataInfo += Environment.NewLine + "Binary: " + DSET;
            dataInfo += Environment.NewLine + "Type = " + DTYPE;
            if (DTYPE.ToUpper() == "STATION")
            {
                dataInfo += Environment.NewLine + "Tsize = " + TDEF.TNum.ToString();
            }
            else
            {
                dataInfo += Environment.NewLine + "Xsize = " + XDEF.XNum.ToString() +
                    "  Ysize = " + YDEF.YNum.ToString() + "  Zsize = " + ZDEF.ZNum.ToString() +
                    "  Tsize = " + TDEF.TNum.ToString();
            }
            dataInfo += Environment.NewLine + "Number of Variables = " + VARDEF.VNum.ToString();
            foreach (Variable v in VARDEF.Vars)
            {
                dataInfo += Environment.NewLine + v.Name + " " + v.LevelNum.ToString() + " " +
                    v.Units + " " + v.Description;
            }

            return dataInfo;
        }

        /// <summary>
        /// Create a GrADS binary data file
        /// </summary>
        /// <param name="aFile">file path</param>
        public void CreateDataFile(string aFile)
        {
            _fs = new FileStream(aFile, FileMode.Create, FileAccess.Write);
            _bw = new BinaryWriter(_fs);
        }

        /// <summary>
        /// Close the data file created by prevoid step
        /// </summary>
        public void CloseDataFile()
        {
            _bw.Close();
            _fs.Close();
        }

        #endregion

        #region Others
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

        private void GetProjectedXY(ProjectionInfo projInfo, Single XSize, Single YSize,
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
                i_X = sync_X - (sync_XP - i_XP) * XSize;
            }
            i_YP = (int)sync_YP;
            if (sync_YP == i_YP)
            {
                i_Y = sync_Y;
            }
            else
            {
                i_Y = sync_Y - (sync_YP - i_YP) * YSize;
            }

            //Get left bottom X/Y
            int nx, ny;
            nx = X.Length;
            ny = Y.Length;
            double xlb, ylb;
            xlb = i_X - (i_XP - 1) * XSize;
            ylb = i_Y - (i_YP - 1) * YSize;

            //Get X Y with orient 0
            int i;
            for (i = 0; i < nx; i++)
            {
                X[i] = xlb + i * XSize;
            }
            for (i = 0; i < ny; i++)
            {
                Y[i] = ylb + i * YSize;
            }
        }

        private void GetProjectedXY_NPS(float XSize, float YSize,
            float sync_XP, float sync_YP,
            ref double[] X, ref double[] Y)
        {
            //Get sync X/Y
            double sync_X = 0, sync_Y = 0;            

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
                i_X = sync_X - (sync_XP - i_XP) * XSize;
            }
            i_YP = (int)sync_YP;
            if (sync_YP == i_YP)
            {
                i_Y = sync_Y;
            }
            else
            {
                i_Y = sync_Y - (sync_YP - i_YP) * YSize;
            }

            //Get left bottom X/Y
            int nx, ny;
            nx = X.Length;
            ny = Y.Length;
            double xlb, ylb;
            xlb = i_X - (i_XP - 1) * XSize;
            ylb = i_Y - (i_YP - 1) * YSize;

            //Get X Y with orient 0
            int i;
            for (i = 0; i < nx; i++)
            {
                X[i] = xlb + i * XSize;
            }
            for (i = 0; i < ny; i++)
            {
                Y[i] = ylb + i * YSize;
            }
        }

        ///<summary>
        ///Swaps the byte order of an Single
        ///</summary>
        /// <param name="d">Single to swap</param>
        /// <returns>Byte Order swapped Single</returns>
        private Single SwapByteOrder(Single d)
        {
            byte[] buffer = BitConverter.GetBytes(d);
            Array.Reverse(buffer, 0, buffer.Length);
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Reading some binary files created under OpenVMS (Alpha)
        /// </summary>
        /// <param name="bytes">byte array</param>
        /// <returns>Float value</returns>
        private Single VaxSingleFromBytes(byte[] bytes)
        {
            uint S;
            uint E;
            ulong F;
            uint b1 = bytes[1];
            uint b2 = bytes[0];
            uint b3 = bytes[3];
            uint b4 = bytes[2];

            S = (b1 & 0x80) >> 7;
            E = ((b1 & 0x7f) << 1) + ((b2 & 0x80) >> 7);
            F = ((b2 & 0x7f) << 16) + (b3 << 8) + b4;

            Single rval = 0;
            double M, F1, A, B, C, e24;
            A = 2.0;
            B = 128.0;
            C = 0.5;
            e24 = 16777216.0; // 2^24

            M = (double)F / e24;

            if (S == 0) F1 = 1.0;
            else F1 = -1.0;

            if (0 < E) rval = (float)(F1 * (C + M) * Math.Pow(A, E - B));
            else if (E == 0 && S == 0)
                rval = 0;
            else if (E == 0 && S == 1)
                throw new ArgumentOutOfRangeException();
            //return -1; // reserved

            return rval;
        }

        #endregion

        #region Convert
        /// <summary>
        /// Convert to netCDF data
        /// </summary>
        /// <param name="ncFilePath">output netCDF data file path</param>
        public void ConvertToNCData(string ncFilePath)
        {
            int i, j;

            //Create netCDF data info object
            NetCDFDataInfo outDataInfo = new NetCDFDataInfo();
            outDataInfo.FileName = ncFilePath;
            outDataInfo.MissingValue = MissingValue;
            outDataInfo.unlimdimid = 3;

            //Add dimensions: lon, lat, level, time
            Dimension lonDim = outDataInfo.AddDimension("lon", XDEF.XNum);
            Dimension latDim = outDataInfo.AddDimension("lat", YDEF.YNum);
            Dimension levelDim = outDataInfo.AddDimension("level", ZDEF.ZNum);
            Dimension timeDim = outDataInfo.AddDimension("time", -1);

            //Add variables
            outDataInfo.AddVariable("lon", NetCDF4.NcType.NC_DOUBLE, new Dimension[] { lonDim });
            outDataInfo.AddVariable("lat", NetCDF4.NcType.NC_DOUBLE, new Dimension[] { latDim });
            outDataInfo.AddVariable("level", NetCDF4.NcType.NC_DOUBLE, new Dimension[] { levelDim });
            outDataInfo.AddVariable("time", NetCDF4.NcType.NC_DOUBLE, new Dimension[] { timeDim });
            foreach (Variable aVar in VARDEF.Vars)
            {
                if (aVar.LevelNum > 1)
                    outDataInfo.AddVariable(aVar.Name, NetCDF4.NcType.NC_DOUBLE,
                        new Dimension[] { timeDim, levelDim, latDim, lonDim });
                else
                    outDataInfo.AddVariable(aVar.Name, NetCDF4.NcType.NC_DOUBLE,
                        new Dimension[] { timeDim, latDim, lonDim });
            }

            //Add variable attributes
            outDataInfo.AddVariableAttribute("lon", "units", "degrees_east");
            outDataInfo.AddVariableAttribute("lon", "long_name", "longitude");
            outDataInfo.AddVariableAttribute("lon", "axis", "x");
            outDataInfo.AddVariableAttribute("lat", "units", "degrees_north");
            outDataInfo.AddVariableAttribute("lat", "long_name", "latitude");
            outDataInfo.AddVariableAttribute("lat", "axis", "y");
            outDataInfo.AddVariableAttribute("level", "units", "level");
            outDataInfo.AddVariableAttribute("level", "long_name", "level");
            outDataInfo.AddVariableAttribute("level", "axis", "z");
            outDataInfo.AddVariableAttribute("time", "units", "hours since 1800-1-1 00:00:00");
            outDataInfo.AddVariableAttribute("time", "long_name", "time");
            outDataInfo.AddVariableAttribute("time", "axis", "t");
            foreach (Variable aVar in VARDEF.Vars)
            {
                outDataInfo.AddVariableAttribute(aVar.Name, "units", aVar.Units);
                outDataInfo.AddVariableAttribute(aVar.Name, "long_name", aVar.Name);
                outDataInfo.AddVariableAttribute(aVar.Name, "missing_value", MissingValue);
                outDataInfo.AddVariableAttribute(aVar.Name, "description", aVar.Description);
            }

            //Add global attributes
            outDataInfo.AddGlobalAttribute("title", "Pressure data");
            outDataInfo.AddGlobalAttribute("description", "script sample data");

            //Create netCDF file
            outDataInfo.CreateNCFile(ncFilePath);

            //Write lon, lat, level, time data
            outDataInfo.WriteVar("lon", X);
            outDataInfo.WriteVar("lat", Y);

            double[] levels = new double[ZDEF.ZNum];
            for (i = 0; i < levels.Length; i++)
                levels[i] = ZDEF.ZLevels[i];

            outDataInfo.WriteVar("level", levels);

            double[] timeArray = new double[Times.Count];
            for (i = 0; i < Times.Count; i++)
            {
                DateTime st = new DateTime(1800, 1, 1);
                double hours = (Times[i] - st).TotalHours;
                timeArray[i] = hours;
            }
            int[] startArray = new int[1];
            int[] countArray = new int[1];
            startArray[0] = 0;
            countArray[0] = Times.Count;
            outDataInfo.WriteVara("time", startArray, countArray, timeArray);

            //Write data
            for (i = 0; i < VARDEF.VNum; i++)
            {
                Variable aVar = VARDEF.Vars[i];
                for (j = 0; j < Times.Count; j++)
                {
                    if (aVar.LevelNum > 1)
                    {
                        double[] dataArray;
                        startArray = new int[4];
                        countArray = new int[4];
                        for (int l = 0; l < aVar.LevelNum; l++)
                        {
                            startArray[0] = j;
                            startArray[1] = l;
                            startArray[2] = 0;
                            startArray[3] = 0;
                            countArray[0] = 1;
                            countArray[1] = 1;
                            countArray[2] = latDim.DimLength;
                            countArray[3] = lonDim.DimLength;
                            dataArray = GetGridData_LonLat(j, i, l).ToOneDimData();
                            outDataInfo.WriteVara(aVar.Name, startArray, countArray, dataArray);
                        }
                    }
                    else
                    {
                        double[] dataArray = new double[1];
                        startArray = new int[3];
                        countArray = new int[3];
                        startArray[0] = j;
                        startArray[1] = 0;
                        startArray[2] = 0;
                        countArray[0] = 1;
                        countArray[1] = latDim.DimLength;
                        countArray[2] = lonDim.DimLength;
                        dataArray = GetGridData_LonLat(j, i, 0).ToOneDimData();
                        outDataInfo.WriteVara(aVar.Name, startArray, countArray, dataArray);
                    }
                }
            }

            //Close netCDF file
            outDataInfo.CloseNCFile();
        }

        #endregion

        #endregion
    }   
}
