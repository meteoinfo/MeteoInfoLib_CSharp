using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using MeteoInfoC.Layout;
using MeteoInfoC.Data;
using MeteoInfoC.Data.MapData;
using MeteoInfoC.Data.MeteoData;
using MeteoInfoC.Map;
using MeteoInfoC.Layer;
using MeteoInfoC.Legend;
using MeteoInfoC.Shape;
using MeteoInfoC.Drawing;
using MeteoInfoC.Projections;
using MeteoInfoC.Global;

namespace MeteoInfoC
{
    /// <summary>
    /// Command class
    /// </summary>
    public class MIApp:Form
    {
        #region Variables
        private MapLayout _mapLayout;
        private MeteoDataInfo _meteoDataInfo;
        private GridData _gridData;
        //private double[,] _gridData;
        private StationData _stationData;
        //private double[,] _DiscreteData;
        private DrawType2D _2DDrawType;
        private bool _hasNoData;        
        //private double _MinData, _MaxData;
        private double[] _X, _Y;
        //private double _XDelt, _YDelt;
        //private int _XNum, _YNum;
        //private double _NoData;
        private double[] _CValues;
        private Color[] _Colors;
        private LegendScheme _legendScheme = null;

        //private int _TimeIdx;
        //private int _VarIdx;
        //private int _LevelIdx;
        private int _StrmDensity = 4;

        //private bool _IfInterpolateGrid = false;
        
        private int _lastAddedLayerHandle;

        private bool _useSameLegendScheme = false;
        private bool _useSameGridInterSet = false;
        private InterpolationSetting _GridInterp = new InterpolationSetting();

        //private GrADSDataInfo _GrADSDataInfo = new GrADSDataInfo();

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MIApp()
        {
            System.Globalization.CultureInfo myCI = new System.Globalization.CultureInfo("en-US", false);
            System.Threading.Thread.CurrentThread.CurrentCulture = myCI;
            
            _mapLayout = new MapLayout();
            _mapLayout.Dock = DockStyle.Fill;
            this.Controls.Add(_mapLayout);
            this.Width = 800;
            this.Height = 600;
            this.Text = "MeteoInfo Script";
            //_mapLayout.DefaultLegend.LegendStyle = LegendStyleEnum.Bar_Vertical;
            _mapLayout.MouseMode = MouseMode.Select;

            MapFrame aMF = _mapLayout.MapFrames[0];
            aMF.LayoutBounds = new Rectangle(40, 36, 606, 420);
            aMF.IsFireMapViewUpdate = true;
            _mapLayout.AddElement(new LayoutMap(aMF));

            _2DDrawType = DrawType2D.Contour;
            _lastAddedLayerHandle = -1;

            _meteoDataInfo = new MeteoDataInfo();

            //_TimeIdx = 0;
            //_VarIdx = 0;
            //_LevelIdx = 0;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or Set MapLayout
        /// </summary>
        public MapLayout MapLayout
        {
            get { return _mapLayout; }
            set { _mapLayout = value; }
        }

        /// <summary>
        /// Meteological data info
        /// </summary>
        public MeteoDataInfo MeteoDataInfo
        {
            get { return _meteoDataInfo; }
            set { _meteoDataInfo = value; }
        }

        /// <summary>
        /// Get or set plot dimension
        /// </summary>
        public PlotDimension PlotDimension
        {
            get { return _meteoDataInfo.DimensionSet; }
            set 
            {
                PlotDimension pDim = value;
                if (_meteoDataInfo.DimensionSet != pDim)
                {
                    _meteoDataInfo.DimensionSet = pDim;
                    switch (pDim)
                    {
                        case PlotDimension.Lat_Lon:
                            _mapLayout.ActiveMapFrame.MapView.RemoveAllLayers();
                            _mapLayout.ActiveMapFrame.MapView.IsGeoMap = true;
                            break;
                        case PlotDimension.Level_Lat:
                        case PlotDimension.Level_Lon:
                        case PlotDimension.Level_Time:
                        case PlotDimension.Time_Lat:
                        case PlotDimension.Time_Lon:
                            _mapLayout.ActiveMapFrame.MapView.RemoveAllLayers();
                            _mapLayout.ActiveMapFrame.MapView.IsGeoMap = false;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Get or set draw type
        /// </summary>
        public DrawType2D DrawType2D
        {
            get { return _2DDrawType; }
            set { _2DDrawType = value; }
        }

        /// <summary>
        /// Get or set default legend scheme
        /// </summary>
        public LegendScheme LegendScheme
        {
            get { return _legendScheme; }
            set { _legendScheme = value; }
        }

        /// <summary>
        /// Get or set if use default legend scheme
        /// </summary>
        public bool UseDefaultLegendScheme
        {
            get { return _useSameLegendScheme; }
            set { _useSameLegendScheme = value; }
        }

        /// <summary>
        /// Get or set time index
        /// </summary>
        public int TimeIndex
        {
            get { return _meteoDataInfo.TimeIndex; }
            set { _meteoDataInfo.TimeIndex = value; }
        }

        /// <summary>
        /// Get or set level index
        /// </summary>
        public int LevelIndex
        {
            get { return _meteoDataInfo.LevelIndex; }
            set { _meteoDataInfo.LevelIndex = value; }
        }

        /// <summary>
        /// Get or set longitude index
        /// </summary>
        public int LonIndex
        {
            get { return _meteoDataInfo.LonIndex; }
            set { _meteoDataInfo.LonIndex = value; }
        }

        /// <summary>
        /// Get or set latitude index
        /// </summary>
        public int LatIndex
        {
            get { return _meteoDataInfo.LatIndex; }
            set { _meteoDataInfo.LatIndex = value; }
        }

        /// <summary>
        /// Get or set variable index
        /// </summary>
        public int VariableIndex
        {
            get { return _meteoDataInfo.VariableIndex; }
            set { _meteoDataInfo.VariableIndex = value; }
        }

        /// <summary>
        /// Get last added layer
        /// </summary>
        public MapLayer LastLayer
        {
            get { return _mapLayout.ActiveMapFrame.MapView.GetLayerFromHandle(_lastAddedLayerHandle); }
        }

        #endregion

        #region Methods

        #region Load/Save XML project file
        /// <summary>
        /// Load project file
        /// </summary>
        /// <param name="fileName">Project file name</param>
        public void LoadProjectFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlElement root = doc.DocumentElement;

            Environment.CurrentDirectory = Path.GetDirectoryName(fileName);

            //Load elements
            this._mapLayout.ActiveLayoutMap.MapFrame.MapView.LockViewUpdate = true;

            //Load map frames content
            List<MapFrame> mfs = new List<MapFrame>();
            XmlNode mapFrames = root.GetElementsByTagName("MapFrames")[0];
            if (mapFrames == null)
            {
                MapFrame mf = new MapFrame();
                mf.ImportProjectXML(root);
                //AddMapFrame(mf);
                //mf.LayersUpdated += MapFrameLayerUpdated;
                mf.Active = true;
                mfs.Add(mf);
            }
            else
            {
                foreach (XmlNode mapFrame in mapFrames.ChildNodes)
                {
                    MapFrame mf = new MapFrame();
                    mf.ImportProjectXML((XmlElement)mapFrame);
                    //AddMapFrame(mf);
                    //mf.LayersUpdated += MapFrameLayerUpdated;
                    mfs.Add(mf);
                }
            }

            _mapLayout.MapFrames = mfs;

            //Load MapLayout content
            _mapLayout.ImportProjectXML(root);
            this._mapLayout.ActiveLayoutMap.MapFrame.MapView.LockViewUpdate = false;  
        }

        #endregion

        #region Open data
        /// <summary>
        /// Open layer
        /// </summary>
        /// <param name="aFile">layer file</param>
        ///<returns>map layer</returns>
        public MapLayer OpenLayer(string aFile)
        {
            aFile = Path.GetFullPath(aFile);
            MapLayer aLayer = MapDataManage.OpenLayer(aFile);            

            if (aLayer != null)
                _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.AddLayer(aLayer);

            return aLayer;
        }

        /// <summary>
        /// Open GrADS data
        /// </summary>
        /// <param name="aFile">data file</param>
        public void OpenGrADSData(string aFile)
        {
            _meteoDataInfo.OpenGrADSData(aFile);            
        }

        /// <summary>
        /// Open MICAPS data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenMICAPSData(string aFile)
        {
            _meteoDataInfo.OpenMICAPSData(aFile);                       
        }

        /// <summary>
        /// Open ARL packed meteorological data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenARLData(string aFile)
        {
            _meteoDataInfo.OpenARLData(aFile);            
        }

        /// <summary>
        /// Open GRIB meteorological data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenGRIBData(string aFile)
        {
            _meteoDataInfo.OpenGRIBData(aFile);
        }

        /// <summary>
        /// Open HDF data file
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenHDFData(string aFile)
        {
            _meteoDataInfo.OpenHDFData(aFile);
        }

        /// <summary>
        /// Open NetCDF data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenNCData(string aFile)
        {
            _meteoDataInfo.OpenNCData(aFile);            
        }

        /// <summary>
        /// Open lon/lat station data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenLonLatData(string aFile)
        {
            _meteoDataInfo.OpenLonLatData(aFile);

            _GridInterp.UnDefData = _meteoDataInfo.MissingValue;
        }

        /// <summary>
        /// Open HYSPLIT concentration data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenHYSPLITConc(string aFile)
        {
            _meteoDataInfo.OpenHYSPLITConc(aFile);            
        }

        /// <summary>
        /// Open HYSPLIT trajectory data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenHYSPLITTraj(string aFile)
        {
            _meteoDataInfo.OpenHYSPLITTraj(aFile);
        }

        /// <summary>
        /// Get grid data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>grid data</returns>
        public GridData GetGridData(string varName)
        {
            return _meteoDataInfo.GetGridData(varName);            
        }        

        /// <summary>
        /// Get station data
        /// </summary>
        /// <param name="varName">variable index</param>
        /// <returns>station data</returns>
        public StationData GetStationData(string varName)
        {
            return _meteoDataInfo.GetStationData(varName);            
        }

        //private bool GetStationModelData(ref double[,] stationModelData)
        //{
        //    bool ifTrue = false;
        //    switch (_meteoDataInfo.DataType)
        //    {
        //        case MeteoDataType.MICAPS_1:
        //            MICAPSData aMICAPSData = new MICAPSData();
        //            Extent aExtent = new Extent();
        //            stationModelData = aMICAPSData.GetStationModelData_M1((MICAPS1DataInfo)_meteoDataInfo.DataInfo,
        //                ref aExtent);
        //            ifTrue = true;
        //            break;                
        //        case MeteoDataType.METAR:
        //            METARData CMETARData = new METARData();
        //            aExtent = new Extent();
        //            stationModelData = CMETARData.GetStationModelData((METARDataInfo)_meteoDataInfo.DataInfo,
        //                ref aExtent);
        //            ifTrue = true;
        //            break;
        //        case MeteoDataType.ISH:
        //            ISHDataInfo aISHDF = (ISHDataInfo)_meteoDataInfo.DataInfo;
        //            aExtent = new Extent();
        //            List<string> stIDList = new List<string>();
        //            stationModelData = aISHDF.GetStationModelData(ref stIDList, ref aExtent);
        //            ifTrue = true;
        //            break;
        //    }

        //    return ifTrue;
        //}

        //private bool GetStationInfoData(ref List<List<string>> stationInfoData, ref List<string> fieldList,
        //    ref List<string> varList)
        //{
        //    bool ifTrue = false;
        //    switch (_meteoDataInfo.DataType)
        //    {
        //        case MeteoDataType.LonLatStation:
        //            LonLatStationDataInfo aLLSDataInfo = (LonLatStationDataInfo)_meteoDataInfo.DataInfo;
        //            stationInfoData = aLLSDataInfo.DataList;
        //            fieldList = aLLSDataInfo.FieldList;
        //            varList = aLLSDataInfo.VarList;
        //            ifTrue = true;
        //            break;
        //        case MeteoDataType.MICAPS_3:
        //            MICAPS3DataInfo aM3DataInfo = (MICAPS3DataInfo)_meteoDataInfo.DataInfo;
        //            stationInfoData = aM3DataInfo.DataList;
        //            fieldList = aM3DataInfo.FieldList;
        //            varList = aM3DataInfo.VarList;
        //            ifTrue = true;
        //            break;
        //        case MeteoDataType.MICAPS_1:
        //            MICAPS1DataInfo aM1DataInfo = (MICAPS1DataInfo)_meteoDataInfo.DataInfo;
        //            stationInfoData = aM1DataInfo.DataList;
        //            fieldList = aM1DataInfo.FieldList;
        //            ifTrue = true;
        //            break;
        //    }

        //    return ifTrue;
        //}

        /// <summary>
        /// Get time number
        /// </summary>
        /// <returns>time number</returns>
        public int GetTimeNumber()
        {
            return _meteoDataInfo.GetTimeNumber();
        }

        /// <summary>
        /// Get DateTime by time index
        /// </summary>
        /// <param name="timeIdx">time index</param>
        /// <returns>DateTime</returns>
        public DateTime GetTime(int timeIdx)
        {
            return _meteoDataInfo.GetTime(timeIdx);
        }

        /// <summary>
        /// Get level number
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>level number</returns>
        public int GetLevelNumber(string varName)
        {
            return _meteoDataInfo.GetLevelNumber(varName);
        }

        /// <summary>
        /// Get level number by first variable
        /// </summary>
        /// <returns>level number</returns>
        public int GetLevelNumber()
        {
            return _meteoDataInfo.GetLevelNumber();
        }

        #endregion

        #region Display
        /// <summary>
        /// Display
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>Map layer</returns>
        public MapLayer Display(string varName)
        {
            if (_meteoDataInfo.IsGridData)
            {
                _gridData = GetGridData(varName);
                return Display(_gridData);
            }
            else if (_meteoDataInfo.IsStationData)
            {
                _stationData = GetStationData(varName);
                return Display(_stationData);
            }

            return null;        
        }

        /// <summary>
        /// Display gird data
        /// </summary>
        /// <param name="aGridData">grid data</param>
        /// <returns>Map layer</returns>
        public MapLayer Display(GridData aGridData)
        {
            _gridData = aGridData;
            LegendScheme aLS;
            if (_useSameLegendScheme && _legendScheme.IsConsistent(_2DDrawType))
                aLS = _legendScheme;
            else
                aLS = CreateLegendScheme();
            //Create meteo data layer
            return DrawMeteoMap_Grid(true, aLS);
        }

        /// <summary>
        /// Display U/V grid data
        /// </summary>
        /// <param name="UGridData">U grid data</param>
        /// <param name="VGridData">V grid data</param>
        /// <returns>Map layer</returns>
        public MapLayer Display(GridData UGridData, GridData VGridData)
        {
            _gridData = UGridData;
            //Create legend scheme
            LegendScheme aLS = null;
            switch (_2DDrawType)
            {
                case DrawType2D.Streamline:
                    aLS = CreateLegendScheme();
                    break;
                default:
                    aLS = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Blue, 10);
                    break;
            }  

            //Create meteo data layer
            if (aLS != null)
                return DrawMeteoMap_Grid(UGridData, VGridData, false, aLS);
            else
                return null;
        }

        /// <summary>
        /// Display color U/V grid data
        /// </summary>
        /// <param name="UGridData">U grid data</param>
        /// <param name="VGridData">V grid data</param>
        /// <param name="XGridData">Color grid data</param>
        /// <returns>Map layer</returns>
        public MapLayer Display(GridData UGridData, GridData VGridData, GridData XGridData)
        {
            _gridData = XGridData;
            //Create legend scheme
            LegendScheme aLS = LegendManage.CreateLegendSchemeFromGridData(_gridData,
                LegendType.GraduatedColor, ShapeTypes.Point, ref _hasNoData);
            PointBreak aPB = new PointBreak();
            for (int i = 0; i < aLS.LegendBreaks.Count; i++)
            {
                aPB = (PointBreak)aLS.LegendBreaks[i];
                aPB.Size = 10;
                aLS.LegendBreaks[i] = aPB;
            }

            //Create meteo data layer
            return DrawMeteoMap_Grid(UGridData, VGridData, true, aLS);
        }

        /// <summary>
        /// Display station data
        /// </summary>
        /// <param name="aStData">station data</param>
        /// <returns>Map layer</returns>
        public MapLayer Display(StationData aStData)
        {
            _stationData = aStData;
            InterpolateData();
            LegendScheme aLS = null;
            if (_useSameLegendScheme && _legendScheme.IsConsistent(_2DDrawType))
                aLS = _legendScheme;
            else
                aLS = CreateLegendScheme_Station();

            return DrawMeteoMap_Station(true, aLS);
        }

        /// <summary>
        /// Display station vector/barb/streamline data
        /// </summary>
        /// <param name="UStData">U/WindDirection station data</param>
        /// <param name="VStData">V/WindSpeed station data</param>
        /// <returns>Map layer</returns>
        public MapLayer Display(StationData UStData, StationData VStData)
        {
            _stationData = UStData;
            return Display(UStData, VStData, true);
        }

        /// <summary>
        /// Display station vector/barb/streamline data
        /// </summary>
        /// <param name="UStData">U/WindDirection station data</param>
        /// <param name="VStData">V/WindSpeed station data</param>
        /// <param name="isUV">if is U/V</param>
        /// <returns>Map layer</returns>
        public MapLayer Display(StationData UStData, StationData VStData, bool isUV)
        {
            //Get legend scheme
            LegendScheme aLS = null;
            aLS = CreateLegendScheme_Station();        

            //Create meteo data layer
            MapLayer aLayer = null;
            string LName = VariableIndex.ToString() + "_" + LevelIndex.ToString() + "_" + TimeIndex.ToString();
            switch (_2DDrawType)
            {
                case DrawType2D.Vector:
                case DrawType2D.Barb:
                case DrawType2D.Streamline:
                    switch (_2DDrawType)
                    {
                        case DrawType2D.Vector:
                        case DrawType2D.Barb:                            
                            if (_2DDrawType == DrawType2D.Vector)
                            {
                                LName = "Vector_" + LName;

                                aLayer = DrawMeteoData.CreateSTVectorLayer(UStData, VStData,
                                    aLS, LName, _meteoDataInfo.MeteoUVSet.IsUV);
                            }
                            else
                            {
                                LName = "Barb_" + LName;
                                aLayer = DrawMeteoData.CreateSTBarbLayer(UStData, VStData,
                                    aLS, LName, _meteoDataInfo.MeteoUVSet.IsUV);
                            }
                            break;
                        case DrawType2D.Streamline:
                            StationData nstUData = new StationData();
                            StationData nstVData = new StationData();
                            if (_meteoDataInfo.MeteoUVSet.IsUV)
                            {
                                nstUData = UStData;
                                nstVData = VStData;
                            }
                            else
                                DataMath.GetUVFromDS(UStData, VStData, ref nstUData, ref nstVData);

                            GridData UData = InterpolateData(nstUData);
                            GridData VData = InterpolateData(nstVData);
                            LName = "Streamline_" + LName;
                            aLayer = DrawMeteoData.CreateStreamlineLayer(UData, VData, _StrmDensity, aLS, LName,
                                true);
                            break;

                    }
                    break;
            }

            if (aLayer != null)
            {
                aLayer.IsMaskout = true;
                aLayer.ProjInfo = _meteoDataInfo.ProjInfo;
                _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.AddLayer(aLayer);
            }

            return aLayer;
        }

        /// <summary>
        /// Display station vector/barb/streamline data
        /// </summary>
        /// <param name="UStData">U/WindDirection station data</param>
        /// <param name="VStData">V/WindSpeed station data</param>
        /// <param name="stData">Station data for color</param>
        /// <returns>Map layer</returns>
        public MapLayer Display(StationData UStData, StationData VStData, StationData stData)
        {
            return Display(UStData, VStData, stData, true);
        }

        /// <summary>
        /// Display station vector/barb/streamline data
        /// </summary>
        /// <param name="UStData">U/WindDirection station data</param>
        /// <param name="VStData">V/WindSpeed station data</param>
        /// <param name="stData">Station data for color</param>
        /// <param name="isUV">if is U/V</param>
        /// <returns>Map layer</returns>
        public MapLayer Display(StationData UStData, StationData VStData, StationData stData, bool isUV)
        {
            //Get legend scheme
            LegendScheme aLS = null;
            aLS = LegendManage.CreateLegendSchemeFromStationData(_stationData,
                    LegendType.GraduatedColor, ShapeTypes.Point, ref _hasNoData);

            //Create meteo data layer
            MapLayer aLayer = null;
            string LName = VariableIndex.ToString() + "_" + LevelIndex.ToString() + "_" + TimeIndex.ToString();
            switch (_2DDrawType)
            {
                case DrawType2D.Vector:
                case DrawType2D.Barb:
                case DrawType2D.Streamline:
                    switch (_2DDrawType)
                    {
                        case DrawType2D.Vector:
                        case DrawType2D.Barb:
                            if (_2DDrawType == DrawType2D.Vector)
                            {
                                LName = "Vector_" + LName;

                                aLayer = DrawMeteoData.CreateSTVectorLayer(UStData, VStData, stData,
                                    aLS, LName, _meteoDataInfo.MeteoUVSet.IsUV);
                            }
                            else
                            {
                                LName = "Barb_" + LName;
                                aLayer = DrawMeteoData.CreateSTBarbLayer(UStData, VStData, stData,
                                    aLS, LName, _meteoDataInfo.MeteoUVSet.IsUV);
                            }
                            break;
                        case DrawType2D.Streamline:
                            StationData nstUData = new StationData();
                            StationData nstVData = new StationData();
                            if (_meteoDataInfo.MeteoUVSet.IsUV)
                            {
                                nstUData = UStData;
                                nstVData = VStData;
                            }
                            else
                                DataMath.GetUVFromDS(UStData, VStData, ref nstUData, ref nstVData);

                            GridData UData = InterpolateData(nstUData);
                            GridData VData = InterpolateData(nstVData);
                            LName = "Streamline_" + LName;
                            aLayer = DrawMeteoData.CreateStreamlineLayer(UData, VData, _StrmDensity, aLS, LName,
                                true);
                            break;

                    }
                    break;
            }

            if (aLayer != null)
            {
                aLayer.IsMaskout = true;
                aLayer.ProjInfo = _meteoDataInfo.ProjInfo;
                _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.AddLayer(aLayer);
            }

            return aLayer;
        }

        /// <summary>
        /// Display trajectory
        /// </summary>
        /// <returns>Map layer</returns>
        public MapLayer DisplayTraj()
        {
            switch (_meteoDataInfo.DataType)
            {
                case MeteoDataType.HYSPLIT_Traj:
                    HYSPLITTrajectoryInfo aDataInfo = (HYSPLITTrajectoryInfo)_meteoDataInfo.DataInfo;
                    VectorLayer aLayer = null;
                    switch (_2DDrawType)
                    {
                        case DrawType2D.Traj_Line:
                            aLayer = aDataInfo.CreateTrajLineLayer();
                            if (_useSameLegendScheme)
                                aLayer.LegendScheme = _legendScheme;
                            else
                            {
                                PolyLineBreak aPLB = new PolyLineBreak();
                                for (int i = 0; i < aLayer.LegendScheme.BreakNum; i++)
                                {
                                    aPLB = (PolyLineBreak)aLayer.LegendScheme.LegendBreaks[i];
                                    aPLB.Size = 2;
                                    aLayer.LegendScheme.LegendBreaks[i] = aPLB;
                                }
                            }
                            _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.AddLayer(aLayer);
                            break;
                        case DrawType2D.Traj_StartPoint:
                            aLayer = aDataInfo.CreateTrajStartPointLayer();
                            PointBreak aPB = (PointBreak)aLayer.LegendScheme.LegendBreaks[0];
                            aPB.Style = PointStyle.UpTriangle;
                            aLayer.LegendScheme.LegendBreaks[0] = aPB;
                            _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.AddLayer(aLayer);
                            break;
                        case DrawType2D.Traj_Point:
                            aLayer = aDataInfo.CreateTrajPointLayer();
                            _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.AddLayer(aLayer);
                            break;
                    }
                    return aLayer;
            }
            return null;
        }       

        private void InterpolateData()
        {
            _gridData = InterpolateData(_stationData);            
        }

        private GridData InterpolateData(StationData aStData)
        {
            GridData aGridData = new GridData();
            switch (_2DDrawType)
            {
                case DrawType2D.Contour:
                case DrawType2D.Shaded:
                    if (!_useSameGridInterSet)
                    {
                        GridDataSetting aGDP = _GridInterp.GridDataSet;
                        aGDP.DataExtent = GetDiscretedDataExtent(aStData.Data);
                        _GridInterp.GridDataSet = aGDP;
                        _GridInterp.Radius = Convert.ToSingle((_GridInterp.GridDataSet.DataExtent.maxX -
                            _GridInterp.GridDataSet.DataExtent.minX) / _GridInterp.GridDataSet.XNum * 2);
                        ContourDraw.CreateGridXY(_GridInterp.GridDataSet, ref _X, ref _Y);
                        _useSameGridInterSet = true;
                    }

                    double[,] S = aStData.Data;
                    S = ContourDraw.FilterDiscreteData_Radius(S, _GridInterp.Radius,
                        _GridInterp.GridDataSet.DataExtent, _meteoDataInfo.MissingValue);
                    switch (_GridInterp.InterpolationMethod)
                    {
                        case InterpolationMethods.IDW_Radius:
                            aGridData = ContourDraw.InterpolateDiscreteData_Radius(S,
                                _X, _Y, _GridInterp.MinPointNum, _GridInterp.Radius, _meteoDataInfo.MissingValue);
                            break;
                        case InterpolationMethods.IDW_Neighbors:
                            aGridData = ContourDraw.InterpolateDiscreteData_Neighbor(S, _X, _Y,
                                _GridInterp.MinPointNum, _meteoDataInfo.MissingValue);
                            break;
                        case InterpolationMethods.Cressman:
                            aGridData = ContourDraw.InterpolateDiscreteData_Cressman(S, _X, _Y,
                                _meteoDataInfo.MissingValue, _GridInterp.RadList);
                            break;
                    }
                    break;
            }

            return aGridData;
        }        

        /// <summary>
        /// Display
        /// </summary>
        /// <param name="U">U name</param>
        /// <param name="V">V name</param>
        /// <returns>Map layer</returns>
        public MapLayer Display(string U, string V)
        {
            if (_meteoDataInfo.IsGridData)
            {
                GridData UGridData = GetGridData(U);
                GridData VGridData = GetGridData(V);

                if (UGridData != null && VGridData != null)
                {
                    return Display(UGridData, VGridData);
                }
                else
                    return null;
            }
            else if (_meteoDataInfo.IsStationData)
            {
                StationData UStData = GetStationData(U);
                StationData VStData = GetStationData(V);

                if (UStData != null && VStData != null)
                    return Display(UStData, VStData);
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// Display
        /// </summary>
        /// <param name="U">U name</param>
        /// <param name="V">V name</param>
        /// <param name="varName">varible name</param>
        /// <returns>Map layer</returns>
        public MapLayer Display(string U, string V, string varName)
        {
            if (_meteoDataInfo.IsGridData)
            {
                GridData UGridData = GetGridData(U);
                GridData VGridData = GetGridData(V);
                _gridData = GetGridData(varName);

                if (UGridData != null && VGridData != null && _gridData != null)
                {
                    return Display(UGridData, VGridData, _gridData);
                }
                else
                    return null;
            }
            else if (_meteoDataInfo.IsStationData)
            {
                StationData UStData = GetStationData(U);
                StationData VStData = GetStationData(V);
                _stationData = GetStationData(varName);

                if (UStData != null && VStData != null && _stationData != null)
                    return Display(UStData, VStData, true);
                else
                    return null;
            }
            else
                return null;
        }

        ///// <summary>
        ///// Display wind with wind directory and wind speed
        ///// </summary>
        ///// <param name="windDir">wind directory</param>
        ///// <param name="windSpeed">wind speed</param>
        //public void DiaplayWind(string windDir, string windSpeed)
        //{                        
        //    StationData windDirData = null;
        //    StationData windSpeedData = null;            

        //    windDirData = _meteoDataInfo.GetStationData(windDir);
        //    windSpeedData = _meteoDataInfo.GetStationData(windSpeed);            

        //    if (windDirData != null && windSpeedData != null)
        //    {
        //        _stationData.Data = windDirData.Data;
        //        //Create legend scheme
        //        LegendScheme aLS = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Blue, 10);

        //        //Create meteo data layer
        //        DrawMeteoMap_Station(windDirData, windSpeedData, false, aLS);
        //    }
        //}

        private void GetXYGridStrs(ref List<string> xGridStrs, ref List<string> yGridStrs)
        {
            xGridStrs.Clear();
            yGridStrs.Clear();
            int i;

            switch (_meteoDataInfo.DimensionSet)
            {
                case PlotDimension.Time_Lat:
                    xGridStrs = GetLatGridStr();
                    yGridStrs = GetTimeGridStr();
                    break;
                case PlotDimension.Time_Lon:
                    xGridStrs = GetLonGridStr();
                    yGridStrs = GetTimeGridStr();
                    break;
                case PlotDimension.Level_Lat:
                    xGridStrs = GetLatGridStr();
                    List<double> levels = _meteoDataInfo.GetLevels();
                    for (i = 0; i < levels.Count; i++)
                    {
                        yGridStrs.Add(levels[i].ToString());
                    }
                    break;
                case PlotDimension.Level_Lon:
                    xGridStrs = GetLonGridStr();
                    levels = _meteoDataInfo.GetLevels();
                    for (i = 0; i < levels.Count; i++)
                    {
                        yGridStrs.Add(levels[i].ToString());
                    }
                    break;
                case PlotDimension.Level_Time:
                    xGridStrs = GetTimeGridStr();
                    levels = _meteoDataInfo.GetLevels();
                    for (i = 0; i < levels.Count; i++)
                    {
                        yGridStrs.Add(levels[i].ToString());
                    }
                    break;
            }
        }

        private List<string> GetLonGridStr()
        {
            List<string> GStrList = new List<string>();
            string drawStr;
            int i;
            double[] X = _meteoDataInfo.GetX();

            if (_meteoDataInfo.IsLonLat)
            {
                Single lon;
                for (i = 0; i < X.Length; i++)
                {
                    lon = Single.Parse(X[i].ToString());
                    if (lon < 0)
                    {
                        lon = lon + 360;
                    }
                    if (lon > 0 && lon <= 180)
                    {
                        drawStr = lon.ToString() + "E";
                    }
                    else if (lon == 0 || lon == 360)
                    {
                        drawStr = "0";
                    }
                    else if (lon == 180)
                    {
                        drawStr = "180";
                    }
                    else
                    {
                        drawStr = (360 - lon).ToString() + "W";
                    }
                    GStrList.Add(drawStr);
                }
            }
            else
            {
                for (i = 0; i < X.Length; i++)
                {
                    drawStr = X[i].ToString();
                    GStrList.Add(drawStr);
                }
            }

            return GStrList;
        }

        private List<string> GetLatGridStr()
        {
            List<string> GStrList = new List<string>();
            string drawStr;
            int i;
            double[] Y = _meteoDataInfo.GetY();

            if (_meteoDataInfo.IsLonLat)
            {
                Single lat;
                for (i = 0; i < Y.Length; i++)
                {
                    lat = Single.Parse(Y[i].ToString());
                    if (lat > 0)
                    {
                        drawStr = lat.ToString() + "N";
                    }
                    else if (lat < 0)
                    {
                        drawStr = (-lat).ToString() + "S";
                    }
                    else
                    {
                        drawStr = "EQ";
                    }
                    GStrList.Add(drawStr);
                }
            }
            else
            {
                for (i = 0; i < Y.Length; i++)
                {
                    drawStr = Y[i].ToString();
                    GStrList.Add(drawStr);
                }
            }

            return GStrList;
        }

        private List<string> GetTimeGridStr()
        {
            List<string> GStrList = new List<string>();
            int i;
            List<DateTime> DTList = _meteoDataInfo.GetTimes();           

            string timeFormat;
            if ((DTList[1] - DTList[0]).Duration().Days >= 1)
            {
                timeFormat = "yyyy-MM-dd";
            }
            else if ((DTList[1] - DTList[0]).Duration().Hours >= 1)
            {
                timeFormat = "yyyy-MM-dd HH";
            }
            else
            {
                timeFormat = "yyyy-MM-dd HH:mm";
            }

            if (DTList[0].Year == DTList[DTList.Count - 1].Year)
            {
                timeFormat = timeFormat.Substring(5);
                if (DTList[0].Month == DTList[DTList.Count - 1].Month)
                {
                    timeFormat = timeFormat.Substring(3);
                }
            }

            for (i = 0; i < DTList.Count; i++)
            {
                GStrList.Add(DTList[i].ToString(timeFormat));
            }

            return GStrList;
        }

        private MapLayer DrawMeteoMap_Grid(Boolean isNew, LegendScheme aLS)
        {
            List<string> xGridStrs = new List<string>();
            List<string> yGridStrs = new List<string>();
            GetXYGridStrs(ref xGridStrs, ref yGridStrs);
            _mapLayout.ActiveMapFrame.MapView.XGridStrs = new List<string>(xGridStrs);
            _mapLayout.ActiveMapFrame.MapView.YGridStrs = new List<string>(yGridStrs);

            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);            
            string LName = VariableIndex.ToString() + "_" + LevelIndex.ToString() + 
                "_" + TimeIndex.ToString();
            string fieldName = "Data";
            if (_meteoDataInfo.DataInfo != null)
                fieldName = _meteoDataInfo.GetVariableNames()[VariableIndex];
            bool ifAddLayer = true;                       

            //Create layer
            switch (_2DDrawType)
            {
                case DrawType2D.Contour:
                    LegendManage.SetContoursAndColors(aLS, ref _CValues, ref _Colors);
                    aLayer = DrawMeteoData.CreateContourLayer(_gridData, aLS, LName, fieldName);
                    aLayer.LabelSet.ShadowColor = _mapLayout.ActiveMapFrame.MapView.BackColor;
                    aLayer.AddLabelsContourDynamic(_mapLayout.ActiveMapFrame.MapView.ViewExtent);
                    break;
                case DrawType2D.Shaded:
                    LegendManage.SetContoursAndColors(aLS, ref _CValues, ref _Colors);
                    aLayer = DrawMeteoData.CreateShadedLayer(_gridData, aLS, LName, fieldName);
                    break;
                case DrawType2D.Grid_Fill:
                    LegendManage.SetContoursAndColors(aLS, ref _CValues, ref _Colors);
                    aLayer = DrawMeteoData.CreateGridFillLayer(_gridData, aLS, LName, fieldName);
                    break;
                case DrawType2D.Grid_Point:
                    LegendManage.SetContoursAndColors(aLS, ref _CValues, ref _Colors);
                    aLayer = DrawMeteoData.CreateGridPointLayer(_gridData, aLS, LName, fieldName);
                    break;                
            }

            aLayer.IsMaskout = true;
            aLayer.ProjInfo = _meteoDataInfo.ProjInfo;
            if (ifAddLayer)
            {
                if (aLayer.ShapeType == ShapeTypes.Polygon)
                {
                    _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.InsertPolygonLayer(aLayer);
                    //_mapLayout.UpdateLegendByLayers();
                }
                else
                {
                    _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.AddLayer(aLayer);
                }
            }

            return aLayer;
        }

        private MapLayer DrawMeteoMap_Grid(GridData UGridData, GridData VGridData, bool ifColor, LegendScheme aLS)
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            string LName = "UV_" + LevelIndex.ToString() + "_" + TimeIndex.ToString();
            bool ifAddLayer = true;
            
            //if (UGridData.YNum != _Y.Length)
            //{
            //    _X = _meteoDataInfo.X;
            //    _Y = _meteoDataInfo.Y;
            //}

            //Create layer
            switch (_2DDrawType)
            {                
                case DrawType2D.Barb:                   
                    aLayer = DrawMeteoData.CreateGridBarbLayer(UGridData, VGridData, _gridData, aLS, ifColor, LName, true);
                    break;
                case DrawType2D.Streamline:
                    aLayer = DrawMeteoData.CreateStreamlineLayer(UGridData, VGridData, _StrmDensity, aLS, LName, true);
                    break;
                default:                    
                    aLayer = DrawMeteoData.CreateGridVectorLayer(UGridData, VGridData, _gridData, aLS, ifColor, LName, true);
                    break;
            }

            aLayer.IsMaskout = true;
            aLayer.ProjInfo = _meteoDataInfo.ProjInfo;
            if (ifAddLayer)
            {
                _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.AddLayer(aLayer);
            }

            return aLayer;
        }

        private MapLayer DrawMeteoMap_Station(Boolean isNew, LegendScheme aLS)
        {
            return DrawMeteoMap_Station(_stationData, isNew, aLS);
        }

        private MapLayer DrawMeteoMap_Station(StationData aStData, bool isNew, LegendScheme aLS)
        {
            bool hasNoData = _hasNoData;
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            string LName = "Station";
            string fieldName = "Data";
            if (_meteoDataInfo.DataInfo != null)
                fieldName = _meteoDataInfo.GetVariableNames()[VariableIndex];

            switch (_2DDrawType)
            {
                case DrawType2D.Contour:
                case DrawType2D.Shaded:
                    if (_GridInterp.InterpolationMethod == InterpolationMethods.IDW_Neighbors)
                        hasNoData = false;
                    else
                        hasNoData = ContourDraw.GetHasUndefineData(_gridData);
                    break;
            }

            bool ifAddLayer = true;

            switch (_2DDrawType)
            {
                case DrawType2D.Station_Point:
                    LegendManage.SetContoursAndColors(aLS, ref _CValues, ref _Colors);
                    aLayer = DrawMeteoData.CreateSTPointLayer(_stationData, aLS, LName, fieldName);
                    break;
                case DrawType2D.Contour:
                    LegendManage.SetContoursAndColors(aLS, ref _CValues, ref _Colors);
                    aLayer = DrawMeteoData.CreateContourLayer(_gridData, aLS, LName, fieldName);
                    break;
                case DrawType2D.Shaded:
                    LegendManage.SetContoursAndColors(aLS, ref _CValues, ref _Colors);
                    aLayer = DrawMeteoData.CreateShadedLayer(_gridData, aLS, LName, fieldName);
                    break;
                case DrawType2D.Weather_Symbol:
                    aLayer = DrawMeteoData.CreateWeatherSymbolLayer(_stationData,
                            "All Weather", LName);
                    break;
                case DrawType2D.Station_Model:
                    //Extent aExtent = new Extent();
                    StationModelData stationModelData = _meteoDataInfo.GetStationModelData();
                    if (stationModelData != null)
                    {
                        bool isSurface = true;
                        if (_meteoDataInfo.DataType == MeteoDataType.MICAPS_2)
                            isSurface = false;
                        aLayer = DrawMeteoData.CreateStationModelLayer(stationModelData,
                            _meteoDataInfo.MissingValue, aLS, LName, isSurface);
                    }
                    else
                    {
                        ifAddLayer = false;
                    }
                    break;
                case DrawType2D.Station_Info:
                    StationInfoData stInfoData = _meteoDataInfo.GetStationInfoData();
                    if (stInfoData != null)
                        aLayer = DrawMeteoData.CreateSTInfoLayer(stInfoData, aLS, LName);
                    else
                        ifAddLayer = false;
                    break;
            }

            aLayer.IsMaskout = true;
            aLayer.ProjInfo = _meteoDataInfo.ProjInfo;
            if (ifAddLayer)
            {
                if (aLayer.ShapeType == ShapeTypes.Polygon)
                {
                    _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.InsertPolygonLayer(aLayer);
                    //_mapLayout.UpdateLegendByLayers();
                }
                else
                {
                    //if (m_2DDrawType == DrawType2D.Vector || m_2DDrawType == DrawType2D.Barb)
                    //{
                    //    m_lastAddedLayerHandle = frmMain.G_LayerLegend.AddWindLayer(aLayer, true, aProjInfo,
                    //        m_MeteoDataInfo.EarthWind);
                    //}
                    //else
                    //{
                    _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.AddLayer(aLayer);
                    //}
                }
            }

            return aLayer;
        }

        private MapLayer DrawMeteoMap_Station(StationData windDirData, StationData windSpeedData, bool ifColor, LegendScheme aLS)
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            string LName = VariableIndex.ToString() + "_" + LevelIndex.ToString() + "_" + TimeIndex.ToString();

            switch (_2DDrawType)
            {
                case DrawType2D.Barb:
                    if (ifColor)
                        aLayer = DrawMeteoData.CreateSTBarbLayer(windDirData, windSpeedData, _stationData,
                            aLS, LName, false);
                    else
                        aLayer = DrawMeteoData.CreateSTBarbLayer(windDirData, windSpeedData, aLS, LName, false);
                    break;
                default:
                    if (ifColor)
                        aLayer = DrawMeteoData.CreateSTVectorLayer(windDirData, windSpeedData, _stationData,
                            aLS, LName, false);
                    else
                        aLayer = DrawMeteoData.CreateSTVectorLayer(windDirData, windSpeedData, aLS, LName, false);
                    break;
            }

            aLayer.IsMaskout = true;
            aLayer.ProjInfo = _meteoDataInfo.ProjInfo;
            _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.AddLayer(aLayer);

            return aLayer;
        }

        private LegendScheme CreateLegendScheme()
        {
            LegendScheme aLegendScheme = null;
            switch (_2DDrawType)
            {
                case DrawType2D.Contour:
                    aLegendScheme = LegendManage.CreateLegendSchemeFromGridData(_gridData,
                        LegendType.UniqueValue, ShapeTypes.Polyline, ref _hasNoData);
                    break;
                case DrawType2D.Shaded:
                    aLegendScheme = LegendManage.CreateLegendSchemeFromGridData(_gridData,
                        LegendType.GraduatedColor, ShapeTypes.Polygon, ref _hasNoData);
                    break;
                case DrawType2D.Grid_Fill:
                    aLegendScheme = LegendManage.CreateLegendSchemeFromGridData(_gridData,
                        LegendType.GraduatedColor, ShapeTypes.Polygon, ref _hasNoData);
                    break;
                case DrawType2D.Grid_Point:
                    aLegendScheme = LegendManage.CreateLegendSchemeFromGridData(_gridData,
                        LegendType.GraduatedColor, ShapeTypes.Point, ref _hasNoData);
                    break;
                case DrawType2D.Vector:                    
                case DrawType2D.Barb:                    
                    aLegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Blue, 10);                    
                    break;
                case DrawType2D.Streamline:
                    aLegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.Blue, 1);
                    break;
                case DrawType2D.Raster:
                    aLegendScheme = LegendManage.CreateLegendSchemeFromGridData(_gridData,
                        LegendType.GraduatedColor, ShapeTypes.Image, ref _hasNoData);
                    break;
            }

            return aLegendScheme;
        }        

        private LegendScheme CreateLegendScheme_Station()
        {
            LegendScheme aLegendScheme = null;
            switch (_2DDrawType)
            {
                case DrawType2D.Station_Point:
                    aLegendScheme = LegendManage.CreateLegendSchemeFromStationData(_stationData,
                        LegendType.GraduatedColor, ShapeTypes.Point, ref _hasNoData);
                    break;
                case DrawType2D.Contour:
                    aLegendScheme = LegendManage.CreateLegendSchemeFromGridData(_gridData,
                        LegendType.UniqueValue, ShapeTypes.Polyline, ref _hasNoData);
                    break;
                case DrawType2D.Shaded:
                    aLegendScheme = LegendManage.CreateLegendSchemeFromGridData(_gridData,
                        LegendType.GraduatedColor, ShapeTypes.Polygon, ref _hasNoData);
                    break;
                case DrawType2D.Vector:
                case DrawType2D.Barb:                    
                    aLegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Blue, 10);
                    break;
                case DrawType2D.Streamline:
                    aLegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.Blue, 1);
                    break;
                case DrawType2D.Weather_Symbol:
                    aLegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Blue, 12);
                    break;
                case DrawType2D.Station_Model:
                    aLegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Blue, 12);
                    break;
                case DrawType2D.Station_Info:
                    aLegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Red, 10);
                    break;
            }

            return aLegendScheme;
        }

        //private LegendScheme CreateLegendSchemeFromDiscreteData(double[,] discreteData,
        //    ShapeTypes aST, ref Boolean hasNoData)
        //{
        //    LegendScheme aLS = null;
        //    double[] CValues;
        //    Color[] colors;

        //    hasNoData = ContourDraw.GetMaxMinValueFDiscreteData(discreteData, _meteoDataInfo.MissingValue, ref _MinData, ref _MaxData);
        //    CValues = LegendManage.CreateContourValues(_MinData, _MaxData);
        //    colors = LegendManage.CreateRainBowColors(CValues.Length + 1);

        //    //Generate lengendscheme            
        //    aLS = LegendManage.CreateGraduatedLegendScheme(CValues, colors,
        //        aST, _MinData, _MaxData, hasNoData, _meteoDataInfo.MissingValue);

        //    return aLS;
        //}

        //private LegendScheme CreateLegendSchemeFromData(double[,] GridData,
        //    LegendType aLT, ShapeTypes aST, ref Boolean hasNoData)
        //{
        //    LegendScheme aLS = null;
        //    double[] CValues;
        //    Color[] colors;

        //    hasNoData = ContourDraw.GetMaxMinValue(GridData, _meteoDataInfo.MissingValue, ref _MinData, ref _MaxData);
        //    CValues = LegendManage.CreateContourValues(_MinData, _MaxData);
        //    colors = LegendManage.CreateRainBowColors(CValues.Length + 1);

        //    //Generate lengendscheme  
        //    if (aLT == LegendType.UniqueValue)
        //    {
        //        aLS = LegendManage.CreateUniqValueLegendScheme(CValues, colors,
        //            aST, _MinData, _MaxData, hasNoData, _meteoDataInfo.MissingValue);
        //    }
        //    else
        //    {
        //        aLS = LegendManage.CreateGraduatedLegendScheme(CValues, colors,
        //            aST, _MinData, _MaxData, hasNoData, _meteoDataInfo.MissingValue);
        //    }

        //    return aLS;
        //}

        /// <summary>
        /// Get the extent of discredted data array
        /// </summary>
        /// <param name="discretedData">discreted data array</param>
        /// <returns>extent</returns>
        private Extent GetDiscretedDataExtent(double[,] discretedData)
        {
            Extent dataExtent = new Extent();
            double minX, maxX, minY, maxY;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            for (int i = 0; i < discretedData.GetLength(1); i++)
            {
                if (i == 0)
                {
                    minX = discretedData[0, i];
                    maxX = minX;
                    minY = discretedData[1, i];
                    maxY = minY;
                }
                else
                {
                    if (minX > discretedData[0, i])
                    {
                        minX = discretedData[0, i];
                    }
                    else if (maxX < discretedData[0, i])
                    {
                        maxX = discretedData[0, i];
                    }
                    if (minY > discretedData[1, i])
                    {
                        minY = discretedData[1, i];
                    }
                    else if (maxY < discretedData[1, i])
                    {
                        maxY = discretedData[1, i];
                    }
                }
            }
            dataExtent.minX = minX;
            dataExtent.maxX = maxX;
            dataExtent.minY = minY;
            dataExtent.maxY = maxY;

            return dataExtent;
        }

        /// <summary>
        /// ReDraw
        /// </summary>
        public void ReDraw()
        {
            _mapLayout.PaintGraphics();
        }

        #endregion

        #region Get mothods
        /// <summary>
        /// Get layer by name
        /// </summary>
        /// <param name="layerName">layer name</param>
        /// <returns>map layer</returns>
        public MapLayer GetLayer(string layerName)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
                return null;
            }

            return _mapLayout.ActiveMapFrame.MapView.GetLayerFromHandle(aHnd);
        }

        /// <summary>
        /// Get vector layer by name
        /// </summary>
        /// <param name="layerName">layer name</param>
        /// <returns>vector layer</returns>
        public VectorLayer GetVectorLayer(string layerName)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
                return null;
            }

            return (VectorLayer)_mapLayout.ActiveMapFrame.MapView.GetLayerFromHandle(aHnd);
        }

        /// <summary>
        /// Get image layer by name
        /// </summary>
        /// <param name="layerName">layer name</param>
        /// <returns>image layer</returns>
        public MapLayer GetImageLayer(string layerName)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
                return null;
            }

            return (ImageLayer)_mapLayout.ActiveMapFrame.MapView.GetLayerFromHandle(aHnd);
        }

        #endregion

        #region Set methods
        /// <summary>
        /// Set if the map layout is antialis
        /// </summary>
        /// <param name="isAntiAlias">is AntiAlias</param>
        public void SetAntiAlias(bool isAntiAlias)
        {
            if (isAntiAlias)
                _mapLayout.SmoothingMode = SmoothingMode.AntiAlias;
            else
                _mapLayout.SmoothingMode = SmoothingMode.Default;
        }

        /// <summary>
        /// Set draw type
        /// </summary>
        /// <param name="drawType">draw type</param>
        public void SetDrawType(string drawType)
        {
            switch (drawType.ToLower())
            {
                case "contour":
                    _2DDrawType = DrawType2D.Contour;
                    break;
                case "shaded":
                    _2DDrawType = DrawType2D.Shaded;
                    break;   
                case "grid_fill":
                    _2DDrawType = DrawType2D.Grid_Fill;
                    break;
                case "grid_point":
                    _2DDrawType = DrawType2D.Grid_Point;
                    break;
                case "vector":
                    _2DDrawType = DrawType2D.Vector;
                    break;
                case "barb":
                    _2DDrawType = DrawType2D.Barb;
                    break;
                case "streamline":
                    _2DDrawType = DrawType2D.Streamline;
                    break; 
                case "station_point":
                    _2DDrawType = DrawType2D.Station_Point;
                    break;
                case "weather_symbol":
                    _2DDrawType = DrawType2D.Weather_Symbol;
                    break;
                case "station_model":
                    _2DDrawType = DrawType2D.Station_Model;
                    break;
                case "station_info":
                    _2DDrawType = DrawType2D.Station_Info;
                    break;
                case "traj_line":
                    _2DDrawType = DrawType2D.Traj_Line;
                    break;
                case "traj_point":
                    _2DDrawType = DrawType2D.Traj_Point;
                    break;
                case "traj_startpoint":
                    _2DDrawType = DrawType2D.Traj_StartPoint;
                    break;
                case "image":
                    _2DDrawType = DrawType2D.Image;
                    break;
                case "raster":
                    _2DDrawType = DrawType2D.Raster;
                    break;
            }
        }

        /// <summary>
        /// Set time index
        /// </summary>
        /// <param name="timeIdx">time index</param>
        public void SetTime(int timeIdx)
        {
            TimeIndex = timeIdx;
        }

        /// <summary>
        /// Set level index
        /// </summary>
        /// <param name="levelIdx">level index</param>
        public void SetLevel(int levelIdx)
        {
            LevelIndex = levelIdx;
        }

        ///// <summary>
        ///// Set main tile
        ///// </summary>
        ///// <param name="aTitle">title string</param>
        //public void SetTitle(string aTitle)
        //{
        //    LabelBreak aLB = (LabelBreak)((LayoutGraphic)_mapLayout.DefaultTitle).Graphic.Legend;
        //    aLB.Text = aTitle;
        //}

        /// <summary>
        /// Set maskout layer
        /// </summary>
        /// <param name="layerName">layer name</param>
        public void SetMaskout(string layerName)
        {
            _mapLayout.ActiveMapFrame.MapView.MaskOut.SetMaskLayer = true;
            _mapLayout.ActiveMapFrame.MapView.MaskOut.MaskLayer = layerName;
        }

        /// <summary>
        /// Set paper size
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        public void SetPaperSize(int width, int height)
        {
            PaperSize aPS = new PaperSize("Custom", width, height);
            _mapLayout.PaperSize = aPS;
        }

        ///// <summary>
        ///// Set illustration - if paint
        ///// </summary>
        ///// <param name="isPaint">if paint</param>
        //public void SetIllustration(bool isPaint)
        //{
        //    _mapLayout.DefaultIllustration.Visible = isPaint;
        //}

        ///// <summary>
        ///// Set illustration - extent
        ///// </summary>
        ///// <param name="minLon">mininum longitude</param>
        ///// <param name="maxLon">maxinum longitude</param>
        ///// <param name="minLat">mininum latitude</param>
        ///// <param name="maxLat">maxinum latitude</param>
        //public void SetIllustration(float minLon, float maxLon, float minLat, float maxLat)
        //{
        //    _mapLayout.DefaultIllustration.MinLon = minLon;
        //    _mapLayout.DefaultIllustration.MaxLon = maxLon;
        //    _mapLayout.DefaultIllustration.MinLat = minLat;
        //    _mapLayout.DefaultIllustration.MaxLat = maxLat;
        //}

        ///// <summary>
        ///// Set illustration position
        ///// </summary>
        ///// <param name="left">left</param>
        ///// <param name="top">top</param>
        ///// <param name="width">width</param>
        ///// <param name="height">height</param>
        //public void SetIllusPos(int left, int top, int width, int height)
        //{
        //    _mapLayout.DefaultIllustration.Left = left;
        //    _mapLayout.DefaultIllustration.Top = top;
        //    _mapLayout.DefaultIllustration.Width = width;
        //    _mapLayout.DefaultIllustration.Height = height;
        //}

        ///// <summary>
        ///// Set illustration position
        ///// </summary>
        ///// <param name="left">left</param>
        ///// <param name="top">top</param>        
        //public void SetIllusPos(int left, int top)
        //{
        //    _mapLayout.DefaultIllustration.Left = left;
        //    _mapLayout.DefaultIllustration.Top = top;            
        //}

        /// <summary>
        /// Clear maskout layer
        /// </summary>
        public void ClearMaskout()
        {
            _mapLayout.ActiveMapFrame.MapView.MaskOut.SetMaskLayer = false;
        }

        /// <summary>
        /// Set grid interpolation parameters
        /// </summary>
        /// <param name="minX">mininum x</param>
        /// <param name="maxX">maxinum x</param>
        /// <param name="minY">mininum y</param>
        /// <param name="maxY">maxinum y</param>
        /// <param name="xNum">x number</param>
        /// <param name="yNum">y nunmber</param>
        /// <param name="aInterMethod">interpolation method</param>
        /// <param name="radius">radius</param>
        /// <param name="minNum">mininum number</param>
        public void SetInterpolation(double minX, double maxX, double minY, double maxY, int xNum, int yNum,
            string aInterMethod, float radius, int minNum)
        {
            GridDataSetting aGDP = new GridDataSetting();
            aGDP.DataExtent.minX = minX;
            aGDP.DataExtent.maxX = maxX;
            aGDP.DataExtent.minY = minY;
            aGDP.DataExtent.maxY = maxY;
            aGDP.XNum = xNum;
            aGDP.YNum = yNum;
            _GridInterp.GridDataSet = aGDP;

            _GridInterp.InterpolationMethod = (InterpolationMethods)Enum.Parse(typeof(InterpolationMethods), aInterMethod, true);
            _GridInterp.Radius = radius;
            _GridInterp.MinPointNum = minNum;

            ContourDraw.CreateGridXY(_GridInterp.GridDataSet, ref _X, ref _Y);
            _useSameGridInterSet = true;
        }

        /// <summary>
        /// Set grid interpolation parameters
        /// </summary>
        /// <param name="minX">mininum x</param>
        /// <param name="maxX">maxinum x</param>
        /// <param name="minY">mininum y</param>
        /// <param name="maxY">maxinum y</param>
        /// <param name="xNum">x number</param>
        /// <param name="yNum">y nunmber</param>
        /// <param name="aInterMethod">interpolation method</param>
        /// <param name="radList">radius</param>        
        public void SetInterpolation(double minX, double maxX, double minY, double maxY, int xNum, int yNum,
            string aInterMethod, List<double> radList)
        {
            GridDataSetting aGDP = new GridDataSetting();
            aGDP.DataExtent.minX = minX;
            aGDP.DataExtent.maxX = maxX;
            aGDP.DataExtent.minY = minY;
            aGDP.DataExtent.maxY = maxY;
            aGDP.XNum = xNum;
            aGDP.YNum = yNum;
            _GridInterp.GridDataSet = aGDP;

            _GridInterp.InterpolationMethod = (InterpolationMethods)Enum.Parse(typeof(InterpolationMethods), aInterMethod, true);
            _GridInterp.RadList = radList;
            _GridInterp.MinPointNum = 1;

            ContourDraw.CreateGridXY(_GridInterp.GridDataSet, ref _X, ref _Y);
            _useSameGridInterSet = true;
        }

        #endregion

        #region Legend scheme
        /// <summary>
        /// Set legend break: color
        /// </summary>
        /// <param name="layerName">layer name</param>
        /// <param name="brkIdx">break index</param>
        /// <param name="aColor">color</param>
        public void SetLegendBreak(string layerName, int brkIdx, Color aColor)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
                return;
            }

            MapLayer aLayer = _mapLayout.ActiveMapFrame.MapView.GetLayerFromHandle(aHnd);
            ColorBreak aCB = aLayer.LegendScheme.LegendBreaks[brkIdx];
            aCB.Color = aColor;
        }

        /// <summary>
        /// Set legend break: color
        /// </summary>
        /// <param name="layerName">layer name</param>
        /// <param name="brkIdx">break index</param>
        /// <param name="aColor">fill color</param>
        /// <param name="outlineColor">outline color</param>
        /// <param name="outlineSize">outline size</param>
        /// <param name="drawOutline">draw outline</param>
        /// <param name="drawFill">draw fill</param>
        /// <param name="drawShape">draw shape</param>
        public void SetLegendBreak(string layerName, int brkIdx, Color aColor, Color outlineColor, float outlineSize,
            bool drawOutline, bool drawFill, bool drawShape)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
                return;
            }

            MapLayer aLayer = _mapLayout.ActiveMapFrame.MapView.GetLayerFromHandle(aHnd);
            PolygonBreak aPB = (PolygonBreak)aLayer.LegendScheme.LegendBreaks[brkIdx];
            aPB.Color = aColor;
            aPB.OutlineColor = outlineColor;
            aPB.OutlineSize = outlineSize;
            aPB.DrawOutline = drawOutline;
            aPB.DrawFill = drawFill;
            aPB.DrawShape = drawShape;
        }

        /// <summary>
        /// Set legend break
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="brkIdx"></param>
        /// <param name="size"></param>
        /// <param name="aColor"></param>
        /// <param name="outlineColor"></param>
        /// <param name="drawOutline"></param>
        /// <param name="drawFill"></param>
        /// <param name="drawShape"></param>
        public void SetLegendBreak(string layerName, int brkIdx, float size, Color aColor, Color outlineColor,
            bool drawOutline, bool drawFill, bool drawShape)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
                return;
            }

            MapLayer aLayer = _mapLayout.ActiveMapFrame.MapView.GetLayerFromHandle(aHnd);
            PointBreak aPB = (PointBreak)aLayer.LegendScheme.LegendBreaks[brkIdx];
            aPB.Size = size;
            aPB.Color = aColor;
            aPB.OutlineColor = outlineColor;
            aPB.DrawFill = drawFill;
            aPB.DrawOutline = drawOutline;
            aPB.DrawShape = drawShape;
        }

        /// <summary>
        /// Set layer transparency percent
        /// </summary>
        /// <param name="layerName">layer name</param>
        /// <param name="trans">transparency percent</param>
        public void SetTransparency(string layerName, int trans)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
                return;
            }

            MapLayer aLayer = _mapLayout.ActiveMapFrame.MapView.GetLayerFromHandle(aHnd);
            aLayer.TransparencyPerc = trans;
        }

        /// <summary>
        /// Set default legend scheme
        /// </summary>
        /// <param name="legendFile">file path</param>
        public void SetLegendScheme(string legendFile)
        {
            switch (_2DDrawType)
            {
                case DrawType2D.Contour:
                case DrawType2D.Streamline:
                    _legendScheme = new LegendScheme(ShapeTypes.Polyline);
                    break;
                case DrawType2D.Shaded:
                case DrawType2D.Grid_Fill:
                    _legendScheme = new LegendScheme(ShapeTypes.Polygon);
                    break;
                case DrawType2D.Grid_Point:
                case DrawType2D.Barb:
                case DrawType2D.Vector:
                case DrawType2D.Station_Info:
                case DrawType2D.Station_Model:
                case DrawType2D.Station_Point:
                case DrawType2D.Weather_Symbol:
                    _legendScheme = new LegendScheme(ShapeTypes.Point);
                    break;                    
            }
            //LegendManage.ImportLegendScheme(legendFile, ref _legendScheme);
            _legendScheme.ImportFromXMLFile(legendFile, false);
            _legendScheme.MissingValue = _meteoDataInfo.MissingValue;
            _useSameLegendScheme = true;
        }

        #endregion

        #region Label
        /// <summary>
        /// Add labels to a layer
        /// </summary>
        /// <param name="layerName">layer name</param>
        /// <param name="fieldName">field name</param>
        public void AddLabels(string layerName, string fieldName)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
                return;
            }

            VectorLayer aLayer = (VectorLayer)_mapLayout.ActiveMapFrame.MapView.GetLayerFromHandle(aHnd);
            aLayer.LabelSet.FieldName = fieldName;
            aLayer.AddLabels();
        }

        #endregion

        #region Projection
        /// <summary>
        /// project layers
        /// </summary>
        /// <param name="projStr">projection string</param>
        public void ProjectLayers(string projStr)
        {
            ProjectionInfo aProjInfo = new ProjectionInfo(projStr);
            _mapLayout.ActiveMapFrame.MapView.ProjectLayers( aProjInfo);
        }

        #endregion

        #region Output

        /// <summary>
        /// Save as a figure
        /// </summary>
        /// <param name="aFile">file path</param>
        public void SaveFigure(string aFile)
        {            
            _mapLayout.ExportToPicture(aFile);            
        }

        #endregion

        #region Zoom
        /// <summary>
        /// Zoom to extent
        /// </summary>
        /// <param name="minX">mininum x</param>
        /// <param name="maxX">maxinum x</param>
        /// <param name="minY">mininum y</param>
        /// <param name="maxY">maxinum y</param>
        public void Zoom(double minX, double maxX, double minY, double maxY)
        {
            _mapLayout.ActiveMapFrame.MapView.ZoomToExtent(minX, maxX, minY, maxY);
        }

        /// <summary>
        /// Zoom to lon/lat extent
        /// </summary>
        /// <param name="minX">mininum x</param>
        /// <param name="maxX">maxinum x</param>
        /// <param name="minY">mininum y</param>
        /// <param name="maxY">maxinum y</param>
        public void ZoomLonLat(double minX, double maxX, double minY, double maxY)
        {
            _mapLayout.ActiveMapFrame.MapView.ZoomToExtentLonLat(minX, maxX, minY, maxY);
        }

        /// <summary>
        /// Zoom to exactly extent
        /// </summary>
        /// <param name="minX">mininum x</param>
        /// <param name="maxX">maxinum x</param>
        /// <param name="minY">mininum y</param>
        /// <param name="maxY">maxinum y</param>
        public void ZoomEx(double minX, double maxX, double minY, double maxY)
        {
            Extent aExtent = new Extent();
            aExtent.minX = minX;
            aExtent.maxX = maxX;
            aExtent.minY = minY;
            aExtent.maxY = maxY;

            _mapLayout.ActiveMapFrame.MapView.ZoomToExtentEx(aExtent);
        }

        /// <summary>
        /// Zoom to exactly lon/lat extent
        /// </summary>
        /// <param name="minX">mininum x</param>
        /// <param name="maxX">maxinum x</param>
        /// <param name="minY">mininum y</param>
        /// <param name="maxY">maxinum y</param>
        public void ZoomLonLatEx(double minX, double maxX, double minY, double maxY)
        {
            Extent aExtent = new Extent();
            aExtent.minX = minX;
            aExtent.maxX = maxX;
            aExtent.minY = minY;
            aExtent.maxY = maxY;

            _mapLayout.ActiveLayoutMap.ZoomToExtentLonLatEx(aExtent);
        }

        /// <summary>
        /// Zoom to layer extent
        /// </summary>
        /// <param name="layerName">layer name</param>
        public void ZoomToLayer(string layerName)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
            else
            {
                MapLayer aLayer = _mapLayout.ActiveMapFrame.MapView.GetLayerFromHandle(aHnd);
                _mapLayout.ActiveMapFrame.MapView.ZoomToExtent(aLayer.Extent);
            }
        }

        /// <summary>
        /// Zoom to last added layer extent
        /// </summary>
        public void ZoomToLastLayer()
        {
            MapLayer aLayer = _mapLayout.ActiveMapFrame.MapView.GetLayerFromHandle(_lastAddedLayerHandle);
            if (aLayer != null)
                _mapLayout.ActiveMapFrame.MapView.ZoomToExtent(aLayer.Extent);
            else
                Console.WriteLine("Error: There is no last added layer or it has been removed!");
        }

        #endregion

        #region layer
        /// <summary>
        /// Add a layer
        /// </summary>
        /// <param name="layer">The layer</param>
        public void AddLayer(MapLayer layer)
        {
            _lastAddedLayerHandle = _mapLayout.ActiveMapFrame.MapView.AddLayer(layer);
        }

        /// <summary>
        /// Set layer if visible
        /// </summary>
        /// <param name="layerName">layer name</param>
        /// <param name="visible">visible</param>
        public void SetLayerVisible(string layerName, bool visible)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
                return;
            }

            MapLayer aLayer = _mapLayout.ActiveMapFrame.MapView.GetLayerFromHandle(aHnd);
            aLayer.Visible = visible;
        }

        /// <summary>
        /// Move layer to top
        /// </summary>
        /// <param name="layerName">layer name</param>
        public void MoveLayerToTop(string layerName)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
                return;
            }
            int fromIdx = _mapLayout.ActiveMapFrame.MapView.GetLayerIdxFromHandle(aHnd);
            _mapLayout.ActiveMapFrame.MapView.MoveLayer(fromIdx, _mapLayout.ActiveMapFrame.MapView.LayerSet.LayerNum - 1);
            //_mapLayout.ActiveMapFrame.MapView.PaintLayers();
        }

        /// <summary>
        /// Move layer to bottom
        /// </summary>
        /// <param name="layerName">layer name</param>
        public void MoveLayerToBottom(string layerName)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
                return;
            }
            int fromIdx = _mapLayout.ActiveMapFrame.MapView.GetLayerIdxFromHandle(aHnd);
            _mapLayout.ActiveMapFrame.MapView.MoveLayer(fromIdx, 0);
            //_mapLayout.ActiveMapFrame.MapView.PaintLayers();
        }

        /// <summary>
        /// Move layer to toLayer
        /// </summary>
        /// <param name="movedLayer">moved layer name</param>
        /// <param name="toLayer">to layer name</param>        
        public void MoveLayer(string movedLayer, string toLayer)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(movedLayer);
            if (aHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + movedLayer + "!");
                return;
            }
            int bHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(toLayer);
            if (bHnd == -1)
            {
                Console.WriteLine("Error: There is no layer :" + toLayer + "!");
                return;
            }

            int fromIdx = _mapLayout.ActiveMapFrame.MapView.GetLayerIdxFromHandle(aHnd);
            int toIdx = _mapLayout.ActiveMapFrame.MapView.GetLayerIdxFromHandle(bHnd);
            _mapLayout.ActiveMapFrame.MapView.MoveLayer(fromIdx, toIdx);
            //_mapLayout.ActiveMapFrame.MapView.PaintLayers();
        }

        /// <summary>
        /// Remove a layer by layer name
        /// </summary>
        /// <param name="layerName">layer name</param>
        public void RemoveLayer(string layerName)
        {
            int aHnd = _mapLayout.ActiveMapFrame.MapView.GetLayerHandleFromName(layerName);
            if (aHnd == -1)
                Console.WriteLine("Error: There is no layer :" + layerName + "!");
            else
                _mapLayout.ActiveMapFrame.MapView.RemoveLayerHandle(aHnd);
        }

        /// <summary>
        /// Remove last added layer
        /// </summary>
        public void RemoveLastLayer()
        {
            _mapLayout.ActiveMapFrame.MapView.RemoveLayerHandle(_lastAddedLayerHandle);
        }

        /// <summary>
        /// Remove all layers
        /// </summary>
        public void RemoveAllLayers()
        {
            _mapLayout.ActiveMapFrame.MapView.RemoveAllLayers();
        }

        /// <summary>
        /// Remove data layers
        /// </summary>
        public void RemoveDataLayers()
        {
            _mapLayout.ActiveMapFrame.MapView.RemoveMeteoLayers();
        }

        #endregion

        #region Data format convertion
        /// <summary>
        /// Convert MICAPS4 data to NetCDF format: dust
        /// Can convert muti-files in one forecast cycle
        /// </summary>
        /// <param name="inFile">one of the input MICAPS4 files</param>
        /// <param name="outFile">output NetCDF file</param>
        /// <param name="varName">variable name</param>
        /// <param name="varLongName">variable long name</param>
        /// <param name="varUnit">variable unit</param>
        public void MICAPS4ToNetCDF_Dust(string inFile, string outFile, string varName, string varLongName, string varUnit)
        {           
            //Get input files
            string inDir = Path.GetDirectoryName(inFile);
            string aFilter = Path.GetFileNameWithoutExtension(inFile) + ".*";
            string[] m4Files = Directory.GetFiles(inDir, aFilter);
            if (m4Files.Length == 0)
            {
                Console.WriteLine("Error: No input file!");
            }

            //Define            
            int i;
            NetCDFData CNetCDFData = new NetCDFData();
            NetCDFDataInfo outDataInfo = new NetCDFDataInfo();
            List<string> varList = new List<string>();
            MICAPS4DataInfo inDataInfo = new MICAPS4DataInfo();
            inDataInfo.ReadDataInfo(m4Files[0]);

            //Set data info    
            outDataInfo.FileName = outFile;
            outDataInfo.IsGlobal = false;
            outDataInfo.isLatLon = true;
            outDataInfo.MissingValue = inDataInfo.MissingValue;
            outDataInfo.unlimdimid = 2;

            //Add dimensions: lon, lat, time
            Dimension lonDim = outDataInfo.AddDimension("lon", inDataInfo.XNum);
            Dimension latDim = outDataInfo.AddDimension("lat", inDataInfo.YNum);
            Dimension timeDim = outDataInfo.AddDimension("time", -1);

            //Add variables
            outDataInfo.AddVariable("lon", NetCDF4.NcType.NC_DOUBLE, new Dimension[] { lonDim });
            outDataInfo.AddVariableAttribute("lon", "units", "degrees_east");
            outDataInfo.AddVariableAttribute("lon", "long_name", "longitude");
            outDataInfo.AddVariable("lat", NetCDF4.NcType.NC_DOUBLE, new Dimension[] { latDim });
            outDataInfo.AddVariableAttribute("lat", "units", "degrees_north");
            outDataInfo.AddVariableAttribute("lat", "long_name", "latitude");
            outDataInfo.AddVariable("time", NetCDF4.NcType.NC_DOUBLE, new Dimension[] { timeDim });
            outDataInfo.AddVariableAttribute("time", "units", "days since 1-1-1 00:00:00");
            outDataInfo.AddVariableAttribute("time", "long_name", "time");
            //outDataInfo.AddVariableAttribute("time", "delta_t", "0000-00-01 00:00:00");
            outDataInfo.AddVariable(varName, NetCDF4.NcType.NC_DOUBLE, new Dimension[] { timeDim, latDim, lonDim });
            if (varUnit != string.Empty)
                outDataInfo.AddVariableAttribute(varName, "units", varUnit);
            outDataInfo.AddVariableAttribute(varName, "long_name", varLongName);
            outDataInfo.AddVariableAttribute(varName, "missing_value", inDataInfo.MissingValue);

            //Add global attributes
            outDataInfo.AddGlobalAttribute("title", "Asian dust storm forecast");
            outDataInfo.AddGlobalAttribute("model", "CUACE/Dust");
            outDataInfo.AddGlobalAttribute("institute", "Chinese Academy of Meteological Sciences");

            //Creat NetCDF file
            outDataInfo.CreateNCFile(outFile);

            DateTime sTime = DateTime.Parse("0001-1-1 00:00:00");
            int fNum = m4Files.Length;
            for (i = 0; i < fNum; i++)
            {
                string aFile = m4Files[i];
                if (i > 0)
                {
                    inDataInfo = new MICAPS4DataInfo();
                    inDataInfo.ReadDataInfo(aFile);
                }
                else
                {
                    //Write lon,lat data                              
                    outDataInfo.WriteVar("lon", inDataInfo.X);
                    outDataInfo.WriteVar("lat", inDataInfo.Y);
                }

                //Write time data
                object[] tData = new object[1];
                tData[0] = inDataInfo.DateTime.Subtract(sTime).TotalDays;
                int[] start = new int[1];
                int[] count = new int[1];
                start[0] = i;
                count[0] = 1;
                outDataInfo.WriteVara("time", start, count, tData);

                //Write dust data
                object[] dustData = new object[inDataInfo.XNum * inDataInfo.YNum];
                for (int m = 0; m < inDataInfo.YNum; m++)
                {
                    for (int n = 0; n < inDataInfo.XNum; n++)
                    {
                        dustData[m * inDataInfo.XNum + n] = inDataInfo.GridData[m, n];
                    }
                }
                start = new int[3];
                start[0] = i;
                start[1] = 0;
                start[2] = 0;
                count = new int[3];
                count[0] = 1;
                count[1] = latDim.DimLength;
                count[2] = lonDim.DimLength;
                outDataInfo.WriteVara(varName, start, count, dustData);      
          
                //Show progress message
                Console.WriteLine(m4Files[i]);
            }
            outDataInfo.CloseNCFile();
        }

        #endregion

        #endregion
    }
}
