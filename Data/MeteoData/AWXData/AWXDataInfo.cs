using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Layer;
using MeteoInfoC.Projections;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// AWX data info - The data format of FY satellite products
    /// </summary>
    public class AWXDataInfo : DataInfo,IGridDataInfo,IStationDataInfo
    {
        #region Variables
        //private string _fileName;
        //First level head record - Part 1
        private string _dataFileName;
        private int _orderOfInt;
        private int _lenHeadP1;
        private int _lenHeadP2;
        private int _lenFillingData;
        private int _lenRecord;
        private int _numHeadRecord;
        private int _numDataRecord;
        private int _productType;
        private int _zipModel;
        private string _illumination;
        private int _qualityMark;
        ////First level head record - Part 2
        //private string _satelliteName;
        //private int _factorGridField;
        private int _byteGridData;
        //private int _refMarkGridData;
        //private int _scaleGridData;
        //private int _codeTimeFrame;
        private int _baseData = 0;
        private int _scaleFactor = 1;
        private int _startYear;
        private int _startMonth;
        private int _startDay;
        private int _startHour;
        private int _startMinute;
        private int _endYear;
        private int _endMonth;
        private int _endDay;
        private int _endHour;
        private int _endMinute;
        private double _ulLatitude;
        private double _ulLongitude;
        private double _lrLatitude;
        private double _lrLongitude;
        private int _unitGrid;
        private int _spaceLatGrid;
        private int _spaceLonGrid;
        private int _numLatGrid;
        private int _numLonGrid;

        private double _width;
        private double _height;
        private double _lonCenter;
        private double _latCenter;
        private double _xDelt;
        private double _yDelt;
        private double _xLB;
        private double _yLB;

        //private int _ifLandMask;
        //private int _valueLandMask;
        //private int _ifCloudMask;
        //private int _valueCloudMask;
        //private int _ifWaterMask;
        //private int _valueWaterMask;
        //private int _ifIceMask;
        //private int _valueIceMask;
        //private int _ifQCDone;
        //private int _upperLimitQC;
        //private int _lowerQC;
        //private int _standby;
        //private int _fillFlags;
        ////Second level head record
        //private string _fileIdSAT2004;
        //private string _formatVersion;
        //private string _producer;
        //private string _satelliteFlat;
        //private string _instrument;
        //private string _procVersion;
        //private string _reserved;
        //private string _copyRight;
        //private string _lenExtendedFD;
        //private int _sfillFlags;

        //Public
        /// <summary>
        /// start observation time
        /// </summary>
        public DateTime STime;
        /// <summary>
        /// end observation time
        /// </summary>
        public DateTime ETime;
        /// <summary>
        /// Image bytes
        /// </summary>
        public byte[] ImageBytes;
        /// <summary>
        /// World file parameter
        /// </summary>
        public WorldFilePara WorldFileP;
        /// <summary>
        /// Variable list
        /// </summary>
        public List<string> VarList;
        /// <summary>
        /// Field list
        /// </summary>
        public List<string> FieldList;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public AWXDataInfo()
        {
            //ProjInfo = KnownCoordinateSystems.Geographic.World.WGS1984;
        }

        #endregion

        #region Propertis

        /// <summary>
        /// Get x number
        /// </summary>
        public int XNum
        {
            get 
            {
                int xNum = 0;
                switch (_productType)
                {
                    case 1:
                        xNum = (int)_width;
                        break;
                    case 3:
                        xNum = _numLonGrid;
                        break;
                }
                return xNum;
            }
        }

        /// <summary>
        /// Get y number
        /// </summary>
        public int YNum
        {
            get
            {
                int yNum = 0;
                switch (_productType)
                {
                    case 1:
                        yNum = (int)_height;
                        break;
                    case 3:
                        yNum = _numLatGrid;
                        break;
                }
                return yNum;
            }
        }

        /// <summary>
        /// Get or set product type
        /// ＝1：静止气象卫星图象产品
        /// ＝2：极轨气象卫星图象产品
        /// ＝3：格点场定量产品
        /// ＝4：离散场定量产品
        /// ＝5：图形和分析产品
        /// </summary>
        public int ProductType
        {
            get { return _productType; }
            set { _productType = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read data info
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>if ok</returns>
        public override void ReadDataInfo(string aFile)
        {
            this.FileName = aFile;
            //_fileName = aFile;

            FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            //Read first level head record
            //Part 1
            _dataFileName = ASCIIEncoding.ASCII.GetString(br.ReadBytes(12));
            _orderOfInt = BitConverter.ToInt16(br.ReadBytes(2), 0);
            _lenHeadP1 = BitConverter.ToInt16(br.ReadBytes(2), 0);
            _lenHeadP2 = BitConverter.ToInt16(br.ReadBytes(2), 0);
            _lenFillingData = BitConverter.ToInt16(br.ReadBytes(2), 0);
            _lenRecord = BitConverter.ToInt16(br.ReadBytes(2), 0);
            _numHeadRecord = BitConverter.ToInt16(br.ReadBytes(2), 0);
            _numDataRecord = BitConverter.ToInt16(br.ReadBytes(2), 0);
            _productType = BitConverter.ToInt16(br.ReadBytes(2), 0);
            _zipModel = BitConverter.ToInt16(br.ReadBytes(2), 0);
            _illumination = ASCIIEncoding.ASCII.GetString(br.ReadBytes(8));
            _qualityMark = BitConverter.ToInt16(br.ReadBytes(2), 0);             

            //Part 2
            br.BaseStream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = br.ReadBytes(_lenHeadP1 + _lenHeadP2);
            byte[] tbytes = new byte[2];

            if (_productType == 3)
            {
                Array.Copy(bytes, 50, tbytes, 0, 2);
                _byteGridData = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 52, tbytes, 0, 2);
                _baseData = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 54, tbytes, 0, 2);
                _scaleFactor = BitConverter.ToInt16(tbytes, 0);
            }

            int yearIdx = 58;            
            switch (_productType)
            {
                case 1:
                    yearIdx = 48;                    
                    break;
                case 2:

                    break;
                case 3:
                    yearIdx = 58;                    
                    break;
                case 4:
                    yearIdx = 54;
                    break;
            }
            
            //Get start time
            Array.Copy(bytes, yearIdx, tbytes, 0, 2);
            _startYear = BitConverter.ToInt16(tbytes, 0);
            Array.Copy(bytes, yearIdx + 2, tbytes, 0, 2);
            _startMonth = BitConverter.ToInt16(tbytes, 0);
            Array.Copy(bytes, yearIdx + 4, tbytes, 0, 2);
            _startDay = BitConverter.ToInt16(tbytes, 0);
            Array.Copy(bytes, yearIdx + 6, tbytes, 0, 2);
            _startHour = BitConverter.ToInt16(tbytes, 0);
            Array.Copy(bytes, yearIdx + 8, tbytes, 0, 2);
            _startMinute = BitConverter.ToInt16(tbytes, 0);
            STime = new DateTime(_startYear, _startMonth, _startDay, _startHour, _startMinute, 0);

            if (_productType == 3 || _productType == 4)    //Get end time
            {
                Array.Copy(bytes, yearIdx + 10, tbytes, 0, 2);
                _endYear = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, yearIdx + 12, tbytes, 0, 2);
                _endMonth = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, yearIdx + 14, tbytes, 0, 2);
                _endDay = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, yearIdx + 16, tbytes, 0, 2);
                _endHour = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, yearIdx + 18, tbytes, 0, 2);
                _endMinute = BitConverter.ToInt16(tbytes, 0);
                if (_endYear > 0)
                    ETime = new DateTime(_endYear, _endMonth, _endDay, _endHour, _endMinute, 0);
            }

            if (_productType == 3)    //Get grid parameters
            {
                Array.Copy(bytes, yearIdx + 20, tbytes, 0, 2);
                _ulLatitude = BitConverter.ToInt16(tbytes, 0);
                _ulLatitude = _ulLatitude / 100;
                Array.Copy(bytes, yearIdx + 22, tbytes, 0, 2);
                _ulLongitude = BitConverter.ToInt16(tbytes, 0);
                _ulLongitude = _ulLongitude / 100;
                Array.Copy(bytes, yearIdx + 24, tbytes, 0, 2);
                _lrLatitude = BitConverter.ToInt16(tbytes, 0);
                _lrLatitude = _lrLatitude / 100;
                Array.Copy(bytes, yearIdx + 26, tbytes, 0, 2);
                _lrLongitude = BitConverter.ToInt16(tbytes, 0);
                _lrLongitude = _lrLongitude / 100;
                Array.Copy(bytes, yearIdx + 28, tbytes, 0, 2);
                _unitGrid = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, yearIdx + 30, tbytes, 0, 2);
                _spaceLonGrid = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, yearIdx + 32, tbytes, 0, 2);
                _spaceLatGrid = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, yearIdx + 34, tbytes, 0, 2);
                _numLonGrid = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, yearIdx + 36, tbytes, 0, 2);
                _numLatGrid = BitConverter.ToInt16(tbytes, 0);

                CalCoordinate_3();
            }

            if (_productType == 1)
                GetProjection(bytes);

            //Close file
            br.Close();
            fs.Close();  
          
            //Set variable list
            VarList = new List<string>();
            List<Variable> variables = new List<Variable>();
            Variable var;
            switch (_productType)
            {
                case 1:
                case 2:
                case 3:
                    VarList.Add("var");
                    var = new Variable();
                    var.Name = "var";
                    var.SetDimension(this.YDimension);
                    var.SetDimension(this.XDimension);
                    variables.Add(var);
                    break;
                case 4:
                    VarList.Add("Pressure");
                    VarList.Add("WindDirection");
                    VarList.Add("WindSpeed");
                    VarList.Add("Temperature");
                    VarList.Add("Slope");      
                    VarList.Add("Correlation");
                    VarList.Add("MiddleRow");
                    VarList.Add("MiddleCol");
                    VarList.Add("FirstRow");
                    VarList.Add("FirstCol");
                    VarList.Add("LastRow");
                    VarList.Add("LastCol");
                    VarList.Add("BrightTemp");
                    foreach (string vName in VarList)
                    {
                        var = new Variable();
                        var.Name = vName;
                        var.IsStation = true;
                        variables.Add(var);
                    }

                    FieldList = new List<string>();
                    FieldList.AddRange(new string[] { "Stid", "Longitude", "Latitude" });
                    FieldList.AddRange(VarList);
                    break;
            }
            this.Variables = variables;
        }

        private void GetProjection(byte[] bytes)
        {
            byte[] tbytes = new byte[2];
            if (_productType == 1)    //Get grid/projection parameters
            {
                Array.Copy(bytes, 58, tbytes, 0, 2);
                int channel = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 60, tbytes, 0, 2);
                int projType = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 62, tbytes, 0, 2);
                _width = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 64, tbytes, 0, 2);
                _height = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 66, tbytes, 0, 2);
                int ulLineNum = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 68, tbytes, 0, 2);
                int ulPixelNum = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 70, tbytes, 0, 2);
                int ratio = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 72, tbytes, 0, 2);
                _ulLatitude = BitConverter.ToInt16(tbytes, 0);
                _ulLatitude = _ulLatitude / 100;
                Array.Copy(bytes, 74, tbytes, 0, 2);
                _lrLatitude = BitConverter.ToInt16(tbytes, 0);
                _lrLatitude = _lrLatitude / 100;
                Array.Copy(bytes, 76, tbytes, 0, 2);
                _ulLongitude = BitConverter.ToInt16(tbytes, 0);
                _ulLongitude = _ulLongitude / 100;
                Array.Copy(bytes, 78, tbytes, 0, 2);
                _lrLongitude = BitConverter.ToInt16(tbytes, 0);
                _lrLongitude = _lrLongitude / 100;
                Array.Copy(bytes, 80, tbytes, 0, 2);
                _latCenter = BitConverter.ToInt16(tbytes, 0);
                _latCenter = _latCenter / 100;
                Array.Copy(bytes, 82, tbytes, 0, 2);
                _lonCenter = BitConverter.ToInt16(tbytes, 0);
                _lonCenter = _lonCenter / 100;
                Array.Copy(bytes, 84, tbytes, 0, 2);
                float lat1 = BitConverter.ToInt16(tbytes, 0);
                lat1 = lat1 / 100;
                Array.Copy(bytes, 86, tbytes, 0, 2);
                float lat2 = BitConverter.ToInt16(tbytes, 0);
                lat2 = lat2 / 100;
                Array.Copy(bytes, 88, tbytes, 0, 2);
                _xDelt = BitConverter.ToInt16(tbytes, 0);
                _xDelt = _xDelt / 100;
                Array.Copy(bytes, 90, tbytes, 0, 2);
                _yDelt = BitConverter.ToInt16(tbytes, 0);
                _yDelt = _yDelt / 100;
                Array.Copy(bytes, 92, tbytes, 0, 2);
                int hasGeoGrid = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 94, tbytes, 0, 2);
                int geoGridValue = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 96, tbytes, 0, 2);
                int lenPallate = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 98, tbytes, 0, 2);
                int lenVef = BitConverter.ToInt16(tbytes, 0);
                Array.Copy(bytes, 100, tbytes, 0, 2);
                int lenGeo = BitConverter.ToInt16(tbytes, 0);

                string projStr = this.ProjectionInfo.ToProj4String();
                switch (projType)
                {
                    case 0:    //未投影（卫星投影）

                        break;
                    case 1:    //兰勃托投影
                        projStr = "+proj=lcc" +
                                "+lon_0=" + _lonCenter.ToString() +
                                "+lat_0=" + _latCenter.ToString() +
                                "+lat_1=" + lat1.ToString() +
                                "+lat_2=" + lat2.ToString(); 
                        break;
                    case 2:    //麦卡托投影
                        projStr = "+proj=merc" +
                            "+lon_0=" + _lonCenter.ToString() +
                            "+lat_ts=" + lat1.ToString();
                        break;
                    case 3:    //极射投影
                        projStr = "+proj=stere" +
                                "+lat_0=" + _latCenter.ToString() +
                                "+lon_0=" + _lonCenter.ToString();                                                           
                        double k0 = Proj.CalScaleFactorFromStandardParallel(lat1);
                        projStr += "+k=" + k0.ToString(); 
                        break;
                    case 4:    //等经纬度投影

                        break;
                    case 5:    //等面积投影

                        break;
                }

                if (projStr != this.ProjectionInfo.ToProj4String())
                    this.ProjectionInfo = new ProjectionInfo(projStr);

                CalCoordinate_1(projType);
            }
        }

        /// <summary>
        /// Generate data info text
        /// </summary>       
        /// <returns>info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + FileName;
            dataInfo += Environment.NewLine + "Product Type: " + _productType.ToString();
            dataInfo += Environment.NewLine + "Start Time: " + STime.ToString("yyyy-MM-dd HH:mm");
            dataInfo += Environment.NewLine + "End Time: " + ETime.ToString("yyyy-MM-dd HH:mm");

            return dataInfo;
        }

        private void CalCoordinate_1(int projType)
        {
            int xNum = (int)_width;
            int yNum = (int)_height;
            switch (projType)
            {
                case 4:                    
                    _xDelt = (_lrLongitude - _ulLongitude) / xNum;
                    _yDelt = (_ulLatitude - _lrLatitude) / yNum;
                    _xLB = _ulLongitude + _xDelt;
                    _yLB = _lrLatitude + _yDelt;
                    break;
                default:
                    ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
                    double X_Center, Y_Center;
                    double[][] points = new double[1][];
                    points[0] = new double[] { _lonCenter, _latCenter };
                    Reproject.ReprojectPoints(points, fromProj, this.ProjectionInfo, 0, 1);
                    X_Center = points[0][0];
                    Y_Center = points[0][1];

                    _xDelt = _xDelt * 1000;
                    _yDelt = _yDelt * 1000;
                    _xLB = X_Center - (_xDelt * _width / 2);
                    _yLB = Y_Center - (_yDelt * _height / 2);                    
                    break;
            }
            
            double[] x = new double[xNum];
            double[] y = new double[yNum];
            int i;
            for (i = 0; i < xNum; i++)
            {
                x[i] = _xLB + _xDelt * i;
            }

            for (i = 0; i < yNum; i++)
            {
                y[i] = _yLB + _yDelt * i;
            }

            Dimension xdim = new Dimension(DimensionType.X);
            xdim.SetValues(x);
            this.XDimension = xdim;
            Dimension ydim = new Dimension(DimensionType.Y);
            ydim.SetValues(y);
            this.YDimension = ydim;           
        }

        private void CalCoordinate_3()
        {
            double width = _lrLongitude - _ulLongitude;
            double height = _ulLatitude - _lrLatitude;

            WorldFileP.XUL = _ulLongitude;
            WorldFileP.YUL = _ulLatitude;
            WorldFileP.XScale = width / _numLonGrid;
            WorldFileP.YScale = -height / _numLatGrid;

            double[] x = new double[_numLonGrid];
            double[] y = new double[_numLatGrid];
            double xDelt = (_lrLongitude - _ulLongitude) / _numLonGrid;
            double yDelt = (_ulLatitude - _lrLatitude) / _numLatGrid;
            int i;
            for (i = 0; i < _numLonGrid; i++)
            {
                x[i] = _ulLongitude + xDelt * i;
            }

            for (i = 0; i < _numLatGrid; i++)
            {
                y[i] = _lrLatitude + yDelt * i;
            }
            Dimension xdim = new Dimension(DimensionType.X);
            xdim.SetValues(x);
            this.XDimension = xdim;
            Dimension ydim = new Dimension(DimensionType.Y);
            ydim.SetValues(y);
            this.YDimension = ydim;            
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
            GridData gData = null;
            switch (_productType)
            {
                case 1:
                    gData = GetGridData_1();
                    break;
                case 3:
                    gData = GetGridData_3();
                    break;
            }

            return gData;
        }

        private GridData GetGridData_3()
        {
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            //Read byte data 
            br.BaseStream.Seek(_lenRecord * _numHeadRecord, SeekOrigin.Begin);
            int length = (int)(br.BaseStream.Length - br.BaseStream.Position);
            byte[] imageBytes = br.ReadBytes(length);

            br.Close();
            fs.Close();

            //Get grid data
            int i, j;
            GridData gridData = new GridData();
            double[] x = new double[_numLonGrid];
            double[] y = new double[_numLatGrid];
            double xDelt = (_lrLongitude - _ulLongitude) / _numLonGrid;
            double yDelt = (_ulLatitude - _lrLatitude) / _numLatGrid;
            for (i = 0; i < _numLonGrid; i++)
            {
                x[i] = _ulLongitude + xDelt * i;
            }

            for (i = 0; i < _numLatGrid; i++)
            {
                y[i] = _lrLatitude + yDelt * i;
            }
            gridData.X = x;
            gridData.Y = y;

            double[,] gData = new double[_numLatGrid, _numLonGrid];
            int bi = 0;
            int value = 0;
            byte[] vbytes = new byte[_byteGridData];

            for (i = 0; i < _numLatGrid; i++)
            {
                for (j = 0; j < _numLonGrid; j++)
                {
                    Array.Copy(imageBytes, bi, vbytes, 0, _byteGridData);
                    if (_byteGridData == 1)
                        value = vbytes[0];
                    else if (_byteGridData == 2)
                        value = BitConverter.ToInt16(vbytes, 0);
                    else if (_byteGridData == 4)
                        value = BitConverter.ToInt32(vbytes, 0);
                    gData[_numLatGrid - i - 1, j] = (double)(value + _baseData) / _scaleFactor;
                    bi += _byteGridData;
                }
            }
            gridData.Data = gData;

            return gridData;
        }

        private GridData GetGridData_1()
        {
            byte[] imageBytes = GetIamgeData();

            //Get grid data
            int i, j;
            GridData gridData = new GridData();
            int xNum = (int)_width;
            int yNum = (int)_height;
            double[] x = new double[xNum];
            double[] y = new double[yNum];            
            for (i = 0; i < xNum; i++)
            {
                x[i] = _xLB + _xDelt * i;
            }

            for (i = 0; i < yNum; i++)
            {
                y[i] = _yLB + _yDelt * i;
            }
            gridData.X = x;
            gridData.Y = y;

            double[,] gData = new double[yNum, xNum];
            for (i = 0; i < yNum; i++)
            {
                for (j = 0; j < xNum; j++)
                {
                    gData[yNum - i - 1, j] = imageBytes[i * xNum + j];
                }
            }
            gridData.Data = gData;

            return gridData;
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

        /// <summary>
        /// Get image data
        /// </summary>
        /// <returns>image data</returns>
        public byte[] GetIamgeData()
        {
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            byte[] bytes = br.ReadBytes(_lenHeadP1 + _lenHeadP2);
            byte[] tbytes = new byte[2];
            Array.Copy(bytes, 96, tbytes, 0, 2);
            int lenPallate = BitConverter.ToInt16(tbytes, 0);
            Array.Copy(bytes, 98, tbytes, 0, 2);
            int lenVef = BitConverter.ToInt16(tbytes, 0);
            Array.Copy(bytes, 100, tbytes, 0, 2);
            int lenGeo = BitConverter.ToInt16(tbytes, 0);

            //Read byte data 
            br.BaseStream.Seek(_lenRecord * _numHeadRecord, SeekOrigin.Begin);
            int length = (int)(br.BaseStream.Length - br.BaseStream.Position);
            byte[] imageBytes = br.ReadBytes(length);

            br.Close();
            fs.Close();

            return imageBytes;
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
            StationData stationData = new StationData();
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            Single lon, lat, t;
            List<string> stations = new List<string>();
            List<double[]> disDataList = new List<double[]>();            
            Single minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            br.BaseStream.Seek(_lenRecord * _numHeadRecord, SeekOrigin.Begin);
            long bP = br.BaseStream.Position;
            for (int i = 0; i < _numDataRecord; i++)
            {                
                br.BaseStream.Position = bP;
                if (br.BaseStream.Position + _lenRecord > br.BaseStream.Length)
                    break;
                
                stations.Add(i.ToString());
                lat = ((float)BitConverter.ToInt16(br.ReadBytes(2), 0)) / 100;
                lon = ((float)BitConverter.ToInt16(br.ReadBytes(2), 0)) / 100;
                if (varIdx <= 2)
                    br.ReadBytes(2 * varIdx);
                else
                    br.ReadBytes(2 * varIdx + 2);
                t = BitConverter.ToInt16(br.ReadBytes(2), 0);
                switch (varIdx)
                {
                    case 3:
                    case 12:
                        t = t / 100;
                        break;
                    case 4:
                    case 5:
                        t = t / 1000;
                        break;
                }

                disDataList.Add(new double[] { lon, lat, t });
                //discreteData[0, i] = lon;
                //discreteData[1, i] = lat;
                //discreteData[2, i] = t;

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

                bP = bP + _lenRecord;
            }

            Extent dataExtent = new Extent();
            dataExtent.minX = minX;
            dataExtent.maxX = maxX;
            dataExtent.minY = minY;
            dataExtent.maxY = maxY;

            br.Close();
            fs.Close();

            double[,] discreteData = new double[3, disDataList.Count];
            for (int i = 0; i < disDataList.Count; i++)
            {
                discreteData[0, i] = disDataList[i][0];
                discreteData[1, i] = disDataList[i][1];
                discreteData[2, i] = disDataList[i][2];
            }
            stationData.Data = discreteData;
            stationData.DataExtent = dataExtent;
            stationData.Stations = stations;

            return stationData;
        }

        /// <summary>
        /// Get station info data
        /// </summary>        
        /// <returns>station info data</returns>
        private List<List<string>> GetStationInfoDataList()
        {
            List<List<string>> stInfoData = new List<List<string>>();
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            Single lon, lat, t;
            List<string> stData = new List<string>();                       
            br.BaseStream.Seek(_lenRecord * 2, SeekOrigin.Begin);
            long bP = br.BaseStream.Position;
            for (int i = 0; i < _numDataRecord; i++)
            {
                br.BaseStream.Position = bP;
                if (br.BaseStream.Position + _lenRecord > br.BaseStream.Length)
                    break;

                stData = new List<string>();
                stData.Add(i.ToString());
                lat = ((float)BitConverter.ToInt16(br.ReadBytes(2), 0)) / 100;
                lon = ((float)BitConverter.ToInt16(br.ReadBytes(2), 0)) / 100;
                stData.Add(lon.ToString());
                stData.Add(lat.ToString());

                for (int j = 0; j < VarList.Count; j++)
                {
                    t = BitConverter.ToInt16(br.ReadBytes(2), 0);
                    switch (j)
                    {
                        case 2:
                            br.ReadBytes(2);
                            break;
                        case 3:
                        case 12:
                            t = t / 100;
                            break;
                        case 4:
                        case 5:
                            t = t / 1000;
                            break;
                    }
                    stData.Add(t.ToString());
                }

                stInfoData.Add(stData);

                bP = bP + _lenRecord;
            }
            
            br.Close();
            fs.Close();            

            return stInfoData;
        }

        /// <summary>
        /// Read station info data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>StationInfoData</returns>
        public StationInfoData GetStationInfoData(int timeIdx, int levelIdx)
        {
            StationInfoData stInfoData = new StationInfoData();
            stInfoData.DataList = GetStationInfoDataList();
            stInfoData.Fields = this.FieldList;
            stInfoData.Variables = this.VarList;

            return stInfoData;
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
