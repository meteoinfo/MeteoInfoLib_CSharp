using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MeteoInfoC.Projections;
using MeteoInfoC.Layer;
using MeteoInfoC.Legend;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Meteo data info
    /// </summary>
    public class MeteoDataInfo
    {
        #region Variables
        private PlotDimension _dimensionSet = PlotDimension.Lat_Lon;
        private int _varIdx;
        private int _timeIdx;
        private int _levelIdx;
        private int _latIdx;
        private int _lonIdx;

        /// <summary>
        /// Meteological data type
        /// </summary>
        public MeteoDataType DataType;
        /// <summary>
        /// Is Lont/Lat
        /// </summary>
        public bool IsLonLat;
        /// <summary>
        /// ProjectionInfo
        /// </summary>
        public ProjectionInfo ProjInfo;
        /// <summary>
        /// If the U/V of the wind are along latitude/longitude.
        /// </summary>
        public bool EarthWind;
        /// <summary>
        /// Meteorological data info
        /// </summary>
        private DataInfo _dataInfo;
        /// <summary>
        /// Data information text
        /// </summary>
        public string InfoText;
        /// <summary>
        /// Undefine data
        /// </summary>
        public double MissingValue;
        /// <summary>
        /// Wind U/V variable name
        /// </summary>
        public MeteoUVSet MeteoUVSet;
        ///// <summary>
        ///// X coordinate array
        ///// </summary>
        //public double[] X;
        ///// <summary>
        ///// Y coordinate array
        ///// </summary>
        //public double[] Y;
        /// <summary>
        /// If X reserved
        /// </summary>
        public bool XReserve;
        /// <summary>
        /// If Y reserved
        /// </summary>
        public bool YReserve;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MeteoDataInfo()
        {
            _dataInfo = null;
            IsLonLat = true;
            ProjInfo = KnownCoordinateSystems.Geographic.World.WGS1984;
            EarthWind = true;
            InfoText = "";
            MissingValue = -9999;
            MeteoUVSet = new MeteoUVSet();
            XReserve = false;
            YReserve = false;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set DataInfo
        /// </summary>
        public DataInfo DataInfo
        {
            get { return _dataInfo; }
            set { _dataInfo = value; }
        }

        /// <summary>
        /// Get or set plot dimension
        /// </summary>
        public PlotDimension DimensionSet
        {
            get { return _dimensionSet; }
            set { _dimensionSet = value; }
        }

        /// <summary>
        /// Get or set time index
        /// </summary>
        public int TimeIndex
        {
            get { return _timeIdx; }
            set { _timeIdx = value; }
        }

        /// <summary>
        /// Get or set level index
        /// </summary>
        public int LevelIndex
        {
            get { return _levelIdx; }
            set { _levelIdx = value; }
        }

        /// <summary>
        /// Get or set variable index
        /// </summary>
        public int VariableIndex
        {
            get { return _varIdx; }
            set { _varIdx = value; }
        }

        /// <summary>
        /// Get or set longitude index
        /// </summary>
        public int LonIndex
        {
            get { return _lonIdx; }
            set { _lonIdx = value; }
        }

        /// <summary>
        /// Get or set latitude index
        /// </summary>
        public int LatIndex
        {
            get { return _latIdx; }
            set { _latIdx = value; }
        }

        /// <summary>
        /// Get if is grid data
        /// </summary>
        public bool IsGridData
        {
            get
            {
                switch (DataType)
                {
                    case MeteoDataType.ARL_Grid:
                    case MeteoDataType.ASCII_Grid:
                    case MeteoDataType.GrADS_Grid:
                    case MeteoDataType.GRIB1:
                    case MeteoDataType.GRIB2:
                    case MeteoDataType.HYSPLIT_Conc:
                    case MeteoDataType.MICAPS_11:
                    case MeteoDataType.MICAPS_13:
                    case MeteoDataType.MICAPS_4:
                    case MeteoDataType.NetCDF:
                    case MeteoDataType.Sufer_Grid:
                    case MeteoDataType.GeoTiff:
                        return true;
                    case MeteoDataType.AWX:
                        switch (((AWXDataInfo)DataInfo).ProductType)
                        {
                            case 1:
                            case 2:
                            case 3:
                                return true;
                            default:
                                return false;
                        }                        
                    case MeteoDataType.HDF:
                        if (((HDF5DataInfo)DataInfo).IsSWATH)
                            return false;
                        else
                            return true;

                        //if (((HDF5DataInfo)DataInfo).CurrentVariable.IsSwath)
                        //    return false;
                        //else
                        //    return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Get if is station data
        /// </summary>
        public bool IsStationData
        {
            get
            {
                switch (DataType)
                {
                    case MeteoDataType.GrADS_Station:
                    case MeteoDataType.ISH:
                    case MeteoDataType.METAR:
                    case MeteoDataType.MICAPS_1:
                    case MeteoDataType.MICAPS_2:
                    case MeteoDataType.MICAPS_3:
                    case MeteoDataType.LonLatStation:
                    case MeteoDataType.SYNOP:
                    case MeteoDataType.HYSPLIT_Particle:
                        return true;
                    case MeteoDataType.AWX:
                        if (((AWXDataInfo)DataInfo).ProductType == 4)
                            return true;
                        else
                            return false;
                    case MeteoDataType.HDF:
                        if (((HDF5DataInfo)DataInfo).IsSWATH)
                            return false;
                        else
                            return true;

                        //if (((HDF5DataInfo)DataInfo).CurrentVariable.IsSwath)
                        //    return true;
                        //else
                        //    return false;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Get if is trajectory data
        /// </summary>
        public bool IsTrajData
        {
            get
            {
                switch (DataType)
                {
                    case MeteoDataType.HYSPLIT_Traj:
                    case MeteoDataType.MICAPS_7:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Get if is SWATH data
        /// </summary>
        public bool IsSWATHData
        {
            get
            {
                switch (DataType)
                {
                    case MeteoDataType.HDF:
                        if (((HDF5DataInfo)DataInfo).IsSWATH)
                            return true;
                        else
                            return false;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Get variable dimension number
        /// </summary>
        public int DimensionNumber
        {
            get
            {
                int dn = 2;
                switch (_dimensionSet)
                {
                    case PlotDimension.Lat_Lon:
                    case PlotDimension.Level_Lat:
                    case PlotDimension.Level_Lon:
                    case PlotDimension.Level_Time:
                    case PlotDimension.Time_Lat:
                    case PlotDimension.Time_Lon:
                        dn = 2;
                        break;
                    case PlotDimension.Level:
                    case PlotDimension.Lon:
                    case PlotDimension.Time:
                    case PlotDimension.Lat:
                        dn = 1;
                        break;
                }

                return dn;
            }
        }

        #endregion

        #region Methods
        #region Open data
        /// <summary>
        /// Open GrADS data
        /// </summary>
        /// <param name="aFile">data file</param>
        public void OpenGrADSData(string aFile)
        {            
            GrADSDataInfo aDataInfo = new GrADSDataInfo();
            //string ErrorStr = "";

            aDataInfo.ReadDataInfo(aFile);
            DataInfo = aDataInfo;
            ProjInfo = aDataInfo.ProjectionInfo;
            MissingValue = aDataInfo.MissingValue;
            InfoText = aDataInfo.GenerateInfoText();

            if (aDataInfo.DTYPE == "Gridded")
            {
                DataType = MeteoDataType.GrADS_Grid;
                YReserve = aDataInfo.OPTIONS.yrev;

                if (!aDataInfo.isLatLon)
                {
                    IsLonLat = false;
                    //ProjInfo = aDataInfo.ProjInfo;
                    EarthWind = aDataInfo.EarthWind;
                }
            }
            else
            {
                DataType = MeteoDataType.GrADS_Station;
            }         
        }

        /// <summary>
        /// Open MICAPS data
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>Is data opened correctly</returns>
        public bool OpenMICAPSData(string aFile)
        {            
            MICAPSDataInfo aMDataInfo = new MICAPSDataInfo();
            string micapsDataType = aMDataInfo.ReadMICAPSHead(aFile);
            switch (micapsDataType)
            {
                case "diamond 1":
                    MICAPS1DataInfo aM1DataInfo = new MICAPS1DataInfo();
                    aM1DataInfo.ReadDataInfo(aFile);
                    DataType = MeteoDataType.MICAPS_1;
                    DataInfo = aM1DataInfo;
                    MissingValue = aM1DataInfo.MissingValue;
                    InfoText = aM1DataInfo.GenerateInfoText();
                    MeteoUVSet.IsUV = false;
                    MeteoUVSet.IsFixUVStr = true;
                    MeteoUVSet.UStr = "WindDirection";
                    MeteoUVSet.VStr = "WindSpeed";
                    return true;
                case "diamond 2":
                    MICAPS2DataInfo aM2DataInfo = new MICAPS2DataInfo();
                    aM2DataInfo.ReadDataInfo(aFile);
                    DataType = MeteoDataType.MICAPS_2;
                    DataInfo = aM2DataInfo;
                    MissingValue = aM2DataInfo.MissingValue;
                    InfoText = aM2DataInfo.GenerateInfoText();
                    MeteoUVSet.IsUV = false;
                    MeteoUVSet.IsFixUVStr = true;
                    MeteoUVSet.UStr = "WindDirection";
                    MeteoUVSet.VStr = "WindSpeed";
                    return true;
                case "diamond 3":
                    MICAPS3DataInfo aM3DataInfo = new MICAPS3DataInfo();
                    aM3DataInfo.ReadDataInfo(aFile);
                    DataType = MeteoDataType.MICAPS_3;
                    DataInfo = aM3DataInfo;
                    MissingValue = aM3DataInfo.MissingValue;
                    InfoText = aM3DataInfo.GenerateInfoText();
                    MeteoUVSet.IsUV = false;
                    MeteoUVSet.IsFixUVStr = true;
                    MeteoUVSet.UStr = "WindDirection";
                    MeteoUVSet.VStr = "WindSpeed";
                    return true;
                case "diamond 4":
                    MICAPS4DataInfo aM4DataInfo = new MICAPS4DataInfo();
                    aM4DataInfo.ReadDataInfo(aFile);
                    if (aM4DataInfo.isLonLat)
                    {
                        DataType = MeteoDataType.MICAPS_4;
                        DataInfo = aM4DataInfo;
                        MissingValue = aM4DataInfo.MissingValue;
                        InfoText = aM4DataInfo.GenerateInfoText();
                        return true;                
                    }
                    else
                    {
                        //MessageBox.Show("Only longitude and latitude data were supported at present!", "Error");
                        return false;
                    }
                case "diamond 7":
                    MICAPS7DataInfo aM7DataInfo = new MICAPS7DataInfo();
                    string[] trajFiles = new string[1];
                    trajFiles[0] = aFile;
                    aM7DataInfo.ReadDataInfo(trajFiles);
                    DataType = MeteoDataType.MICAPS_7;
                    DataInfo = aM7DataInfo;                    
                    InfoText = aM7DataInfo.GenerateInfoText();
                    return true;
                case "diamond 11":
                    MICAPS11DataInfo aM11DataInfo = new MICAPS11DataInfo();
                    aM11DataInfo.ReadDataInfo(aFile);
                    if (aM11DataInfo.isLonLat)
                    {
                        DataType = MeteoDataType.MICAPS_11;
                        DataInfo = aM11DataInfo;
                        MissingValue = aM11DataInfo.MissingValue;
                        InfoText = aM11DataInfo.GenerateInfoText();
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Only longitude and latitude data were supported at present!", "Error");
                        return false;
                    }
                case "diamond 13":
                    MICAPS13DataInfo aM13DataInfo = new MICAPS13DataInfo();
                    aM13DataInfo.ReadDataInfo(aFile);
                    DataType = MeteoDataType.MICAPS_13;
                    DataInfo = aM13DataInfo;
                    ProjInfo = aM13DataInfo.ProjectionInfo;
                    InfoText = aM13DataInfo.GenerateInfoText();
                    return true;
                default:
                    MessageBox.Show("The data were supported at present! " + micapsDataType, "Error");
                    return false;
            }
        }

        /// <summary>
        /// Open ARL packed meteorological data
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>Is data opened correctly</returns>
        public bool OpenARLData(string aFile)
        {            
            ARLDataInfo aDataInfo = new ARLDataInfo();
            aDataInfo.ReadDataInfo(aFile);
            DataType = MeteoDataType.ARL_Grid;
            DataInfo = aDataInfo;
            ProjInfo = aDataInfo.ProjectionInfo;
            IsLonLat = aDataInfo.isLatLon;
            MissingValue = aDataInfo.MissingValue;

            //Get data info text
            InfoText = aDataInfo.GenerateInfoText();

            return true;
        }

        /// <summary>
        /// Open GRIB meteorological data
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>Is data opened correctly</returns>
        public bool OpenGRIBData(string aFile)
        {                                    
            int headerLen = 0;
            int edition = GRIBData.GetGRIBEdition(aFile, ref headerLen);
            if (edition == 1)
            {
                GRIB1DataInfo aDataInfo = new GRIB1DataInfo();
                aDataInfo.HeaderLength = headerLen;
                aDataInfo.ReadDataInfo(aFile);
                DataType = MeteoDataType.GRIB1;
                DataInfo = aDataInfo;
                ProjInfo = aDataInfo.ProjectionInfo;
                MissingValue = aDataInfo.MissingValue;
                InfoText = aDataInfo.GenerateInfoText();
                return true;
            }
            else if (edition == 2)
            {                
                GRIB2DataInfo aDataInfo = new GRIB2DataInfo();
                aDataInfo.ReadDataInfo(aFile);
                DataType = MeteoDataType.GRIB2;
                DataInfo = aDataInfo;
                ProjInfo = aDataInfo.ProjectionInfo;
                MissingValue = aDataInfo.MissingValue;
                InfoText = aDataInfo.GenerateInfoText();
                return true;
            }
            else
            {                
                //MessageBox.Show("The data is not GRIB format!", "Error");
                return false;
            }
        }

        /// <summary>
        /// Open NetCDF data
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>if success</returns>
        public bool OpenNCData(string aFile)
        {
            NetCDFDataInfo aDataInfo = new NetCDFDataInfo();
            string errorStr = "Can not open data file: " + aFile;

            aDataInfo.ReadDataInfo(aFile);
            DataType = MeteoDataType.NetCDF;
            DataInfo = aDataInfo;
            ProjInfo = aDataInfo.ProjectionInfo;
            IsLonLat = aDataInfo.isLatLon;
            MissingValue = aDataInfo.MissingValue;
            YReserve = aDataInfo.IsYReverse;
            InfoText = aDataInfo.GenerateInfoText();

            return true;
        }

        /// <summary>
        /// Open HDF data
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>if ok</returns>
        public bool OpenHDFData(string aFile)
        {
            HDF5DataInfo aDataInfo = new HDF5DataInfo();
            aDataInfo.ReadDataInfo(aFile);

            DataType = MeteoDataType.HDF;
            DataInfo = aDataInfo;
            MissingValue = aDataInfo.MissingValue;
            ProjInfo = aDataInfo.ProjectionInfo;
            InfoText = aDataInfo.GenerateInfoText();

            return true;
        }

        /// <summary>
        /// Open AWX data
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>Is data opened correctly</returns>
        public bool OpenAWXData(string aFile)
        {
            AWXDataInfo aDataInfo = new AWXDataInfo();
            aDataInfo.ReadDataInfo(aFile);
            DataInfo = aDataInfo;
            ProjInfo = aDataInfo.ProjectionInfo;
            DataType = MeteoDataType.AWX;
            InfoText = aDataInfo.GenerateInfoText();
            return true;
        }

        /// <summary>
        /// Open GeoTiff data
        /// </summary>
        /// <param name="fileName">File name</param>
        public void OpenGeoTiffData(string fileName)
        {
            GeoTiffDataInfo aDataInfo = new GeoTiffDataInfo();
            aDataInfo.ReadDataInfo(fileName);
            DataInfo = aDataInfo;
            ProjInfo = aDataInfo.ProjectionInfo;
            DataType = MeteoDataType.GeoTiff;
            InfoText = aDataInfo.GenerateInfoText();            
        }

        /// <summary>
        /// Open HRIT data
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>Is data opened correctly</returns>
        public bool OpenHRITData(string aFile)
        {
            HRITDataInfo aDataInfo = new HRITDataInfo();
            aDataInfo.ReadDataInfo(aFile);
            DataInfo = aDataInfo;
            ProjInfo = aDataInfo.ProjectionInfo;
            DataType = MeteoDataType.HRIT;
            InfoText = aDataInfo.GenerateInfoText();
            return true;
        }

        /// <summary>
        /// Open lon/lat station data
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>Is data opened correctly</returns>
        public bool OpenLonLatData(string aFile)
        {           
            LonLatStationDataInfo aDataInfo = new LonLatStationDataInfo();
            aDataInfo.ReadDataInfo(aFile);
            DataType = MeteoDataType.LonLatStation;
            DataInfo = aDataInfo;
            MissingValue = aDataInfo.MissingValue;
            InfoText = aDataInfo.GenerateInfoText();
            //MeteoUVSet.IsUV = false;
            //MeteoUVSet.IsFixUVStr = true;
            //MeteoUVSet.UStr = "WindDirection";
            //MeteoUVSet.VStr = "WindSpeed";

            return true;
        }

        /// <summary>
        /// Open HYSPLIT concentration data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenHYSPLITConc(string aFile)
        {            
            HYSPLITConcDataInfo aDataInfo = new HYSPLITConcDataInfo();
            aDataInfo.ReadDataInfo(aFile);
            DataType = MeteoDataType.HYSPLIT_Conc;
            DataInfo = aDataInfo;
            MissingValue = aDataInfo.MissingValue;
            InfoText = aDataInfo.GenerateInfoText();
        }

        /// <summary>
        /// Open HYSPLIT trajectory data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenHYSPLITTraj(string aFile)
        {           
            //Read data info                            
            HYSPLITTrajectoryInfo aDataInfo = new HYSPLITTrajectoryInfo();
            aDataInfo.ReadDataInfo(aFile);
            DataType = MeteoDataType.HYSPLIT_Traj;
            DataInfo = aDataInfo;
            InfoText = aDataInfo.GenerateInfoText();
        }

        /// <summary>
        /// Open HYSPLIT trajectory data
        /// </summary>
        /// <param name="trajFiles">file paths</param>
        public void OpenHYSPLITTraj(string[] trajFiles)
        {
            //Read data info                            
            HYSPLITTrajectoryInfo aDataInfo = new HYSPLITTrajectoryInfo();
            aDataInfo.ReadDataInfo(trajFiles);
            DataType = MeteoDataType.HYSPLIT_Traj;
            DataInfo = aDataInfo;
            InfoText = aDataInfo.GenerateInfoText();
        }

        /// <summary>
        /// Open HYSPLIT particle data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenHYSPLITParticle(string aFile)
        {
            HYSPLITParticleInfo aDataInfo = new HYSPLITParticleInfo();
            aDataInfo.ReadDataInfo(aFile);
            DataType = MeteoDataType.HYSPLIT_Particle;
            DataInfo = aDataInfo;
            MissingValue = aDataInfo.MissingValue;
            InfoText = aDataInfo.GenerateInfoText();
        }

        /// <summary>
        /// Open METAR data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenMETARData(string aFile)
        {
            METARDataInfo aDataInfo = new METARDataInfo();
            string stFile = Application.StartupPath + "\\Station\\METAR_Stations.csv";
            aDataInfo.StFileName = stFile;
            aDataInfo.ReadDataInfo(aFile);
            DataType = MeteoDataType.METAR;
            DataInfo = aDataInfo;
            InfoText = aDataInfo.GenerateInfoText();
            MeteoUVSet.IsUV = false;
            MeteoUVSet.IsFixUVStr = true;
            MeteoUVSet.UStr = "WindDirection";
            MeteoUVSet.VStr = "WindSpeed";
        }

        /// <summary>
        /// Open SYNOP data
        /// </summary>
        /// <param name="aFile">data file path</param>
        /// <param name="stFile">station file path</param>
        public void OpenSYNOPData(string aFile, string stFile)
        {
            SYNOPDataInfo aDataInfo = new SYNOPDataInfo();
            aDataInfo.StFileName = stFile;
            aDataInfo.ReadDataInfo(aFile);
            DataType = MeteoDataType.SYNOP;
            DataInfo = aDataInfo;
            InfoText = aDataInfo.GenerateInfoText();
            MeteoUVSet.IsUV = false;
            MeteoUVSet.IsFixUVStr = true;
            MeteoUVSet.UStr = "WindDirection";
            MeteoUVSet.VStr = "WindSpeed";
        }

        /// <summary>
        /// Open ISH data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenISHData(string aFile)
        {
            ISHDataInfo aDataInfo = new ISHDataInfo();
            aDataInfo.ReadDataInfo(aFile);
            DataType = MeteoDataType.ISH;
            DataInfo = aDataInfo;
            MissingValue = aDataInfo.MissingValue;
            InfoText = aDataInfo.GenerateInfoText();
            MeteoUVSet.IsUV = false;
            MeteoUVSet.IsFixUVStr = true;
            MeteoUVSet.UStr = "WindDirection";
            MeteoUVSet.VStr = "WindSpeed";
        }

        /// <summary>
        /// Open Surfer grid data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenSuferGridData(string aFile)
        {
            SurferGridDataInfo aDataInfo = new SurferGridDataInfo();
            aDataInfo.ReadDataInfo(aFile);
            DataType = MeteoDataType.Sufer_Grid;
            DataInfo = aDataInfo;
            MissingValue = aDataInfo.MissingValue;
            InfoText = aDataInfo.GenerateInfoText();
        }

        /// <summary>
        /// Open ESRI ASCII grid data
        /// </summary>
        /// <param name="aFile">file path</param>
        public void OpenASCIIGridData(string aFile)
        {
            ASCIIGRIDDataInfo aDataInfo = new ASCIIGRIDDataInfo();
            aDataInfo.ReadDataInfo(aFile);
            DataType = MeteoDataType.ASCII_Grid;
            DataInfo = aDataInfo;
            MissingValue = aDataInfo.MissingValue;
            InfoText = aDataInfo.GenerateInfoText();
        }

        #endregion

        #region Get data
        /// <summary>
        /// Get grid data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>grid data</returns>
        public GridData GetGridData(string varName)
        {
            _varIdx = GetVariableIndex(varName);
            if (_varIdx < 0)
            {
                MathParser mathParser = new MathParser(this);
                GridData gridData = (GridData)mathParser.Evaluate(varName);
                gridData.ProjInfo = this.ProjInfo;
                return gridData;
            }
            else
            {
                GridData gridData = this.getGridData();
                gridData.ProjInfo = this.ProjInfo;
                return gridData;
            }
        }

        /// <summary>
        /// Get grid data
        /// </summary>
        /// <returns>The grid data</returns>
        public GridData getGridData()
        {
            if (_varIdx < 0)
            {
                return null;
            }

            switch (_dimensionSet)
            {
                case PlotDimension.Lat_Lon:
                    return ((IGridDataInfo)_dataInfo).GetGridData_LonLat(_timeIdx, _varIdx, _levelIdx);
                case PlotDimension.Time_Lon:
                    return ((IGridDataInfo)_dataInfo).GetGridData_TimeLon(_latIdx, _varIdx, _levelIdx);
                case PlotDimension.Time_Lat:
                    return ((IGridDataInfo)_dataInfo).GetGridData_TimeLat(_lonIdx, _varIdx, _levelIdx);
                case PlotDimension.Level_Lon:
                    return ((IGridDataInfo)_dataInfo).GetGridData_LevelLon(_latIdx, _varIdx, _timeIdx);
                case PlotDimension.Level_Lat:
                    return ((IGridDataInfo)_dataInfo).GetGridData_LevelLat(_lonIdx, _varIdx, _timeIdx);
                case PlotDimension.Level_Time:
                    return ((IGridDataInfo)_dataInfo).GetGridData_LevelTime(_latIdx, _varIdx, _lonIdx);
                case PlotDimension.Lat:
                    return ((IGridDataInfo)_dataInfo).GetGridData_Lat(_timeIdx, _lonIdx, _varIdx, _levelIdx);
                case PlotDimension.Level:
                    return ((IGridDataInfo)_dataInfo).GetGridData_Level(_lonIdx, _latIdx, _varIdx, _timeIdx);
                case PlotDimension.Lon:
                    return ((IGridDataInfo)_dataInfo).GetGridData_Lon(_timeIdx, _latIdx, _varIdx, _levelIdx);
                case PlotDimension.Time:
                    return ((IGridDataInfo)_dataInfo).GetGridData_Time(_lonIdx, _latIdx, _varIdx, _levelIdx);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Get Lon_Lat grid data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>grid data</returns>
        public GridData GetGridData_LonLat(string varName)
        {
            GridData aGridData = null;
            _varIdx = GetVariableIndex(varName);
            if (_varIdx == -1)
                return null;

            switch (DataType)
            {               
                case MeteoDataType.HRIT:
                    HRITDataInfo aHRITDataInfo = (HRITDataInfo)DataInfo;
                    aGridData = aHRITDataInfo.GetGridData_All();
                    break;
                default:
                    aGridData = ((IGridDataInfo)DataInfo).GetGridData_LonLat(_timeIdx, _varIdx, _levelIdx);
                    break;
            }

            return aGridData;
        }

        /// <summary>
        /// Get Time_Lon grid data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>grid data</returns>
        public GridData GetGridData_TimeLon(string varName)
        {
            GridData aGridData = null;
            _varIdx = GetVariableIndex(varName);
            switch (DataType)
            {
                case MeteoDataType.GrADS_Grid:
                    aGridData = ((GrADSDataInfo)DataInfo).GetGridData_TimeLon(_latIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.GRIB1:
                    aGridData = ((GRIB1DataInfo)DataInfo).GetGridData_TimeLon(_latIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.GRIB2:
                    aGridData = ((GRIB2DataInfo)DataInfo).GetGridData_TimeLon(_latIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.ARL_Grid:
                    aGridData = ((ARLDataInfo)DataInfo).GetGridData_TimeLon(_latIdx, _varIdx, _levelIdx);
                    break;                                                               
                case MeteoDataType.NetCDF:                    
                    aGridData = ((NetCDFDataInfo)DataInfo).GetGridData_TimeLon(_latIdx, _varIdx, _levelIdx);
                    break;               
            }

            return aGridData;
        }

        /// <summary>
        /// Get Time_Lat grid data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>grid data</returns>
        public GridData GetGridData_TimeLat(string varName)
        {
            GridData aGridData = null;
            _varIdx = GetVariableIndex(varName);
            switch (DataType)
            {
                case MeteoDataType.GrADS_Grid:
                    aGridData = ((GrADSDataInfo)DataInfo).GetGridData_TimeLat(_lonIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.GRIB1:
                    aGridData = ((GRIB1DataInfo)DataInfo).GetGridData_TimeLat(_lonIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.GRIB2:
                    aGridData = ((GRIB2DataInfo)DataInfo).GetGridData_TimeLat(_lonIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.ARL_Grid:
                    aGridData = ((ARLDataInfo)DataInfo).GetGridData_TimeLat(_lonIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.NetCDF:
                    aGridData = ((NetCDFDataInfo)DataInfo).GetGridData_TimeLat(_lonIdx, _varIdx, _levelIdx);
                    break;
            }

            return aGridData;
        }

        /// <summary>
        /// Get Level_Lon grid data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>grid data</returns>
        public GridData GetGridData_LevelLon(string varName)
        {
            GridData aGridData = null;
            _varIdx = GetVariableIndex(varName);
            switch (DataType)
            {
                case MeteoDataType.GrADS_Grid:
                    aGridData = ((GrADSDataInfo)DataInfo).GetGridData_LevelLon(_latIdx, _varIdx, _timeIdx);
                    break;
                case MeteoDataType.GRIB1:
                    aGridData = ((GRIB1DataInfo)DataInfo).GetGridData_LevelLon(_latIdx, _varIdx, _timeIdx);
                    break;
                case MeteoDataType.GRIB2:
                    aGridData = ((GRIB2DataInfo)DataInfo).GetGridData_LevelLon(_latIdx, _varIdx, _timeIdx);
                    break;
                case MeteoDataType.ARL_Grid:
                    aGridData = ((ARLDataInfo)DataInfo).GetGridData_LevelLon(_latIdx, _varIdx, _timeIdx);
                    break;
                case MeteoDataType.NetCDF:
                    aGridData = ((NetCDFDataInfo)DataInfo).GetGridData_LevelLon(_latIdx, _varIdx, _timeIdx);
                    break;
            }

            return aGridData;
        }

        /// <summary>
        /// Get Level_Lat grid data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>grid data</returns>
        public GridData GetGridData_LevelLat(string varName)
        {
            GridData aGridData = null;
            _varIdx = GetVariableIndex(varName);
            switch (DataType)
            {
                case MeteoDataType.GrADS_Grid:
                    aGridData = ((GrADSDataInfo)DataInfo).GetGridData_LevelLat(_lonIdx, _varIdx, _timeIdx);
                    break;
                case MeteoDataType.GRIB1:
                    aGridData = ((GRIB1DataInfo)DataInfo).GetGridData_LevelLat(_lonIdx, _varIdx, _timeIdx);
                    break;
                case MeteoDataType.GRIB2:
                    aGridData = ((GRIB2DataInfo)DataInfo).GetGridData_LevelLat(_lonIdx, _varIdx, _timeIdx);
                    break;
                case MeteoDataType.ARL_Grid:
                    aGridData = ((ARLDataInfo)DataInfo).GetGridData_LevelLat(_lonIdx, _varIdx, _timeIdx);
                    break;
                case MeteoDataType.NetCDF:
                    aGridData = ((NetCDFDataInfo)DataInfo).GetGridData_LevelLat(_lonIdx, _varIdx, _timeIdx);
                    break;
            }

            return aGridData;
        }

        /// <summary>
        /// Get Level_Time grid data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>grid data</returns>
        public GridData GetGridData_LevelTime(string varName)
        {
            GridData aGridData = null;
            _varIdx = GetVariableIndex(varName);
            switch (DataType)
            {
                case MeteoDataType.GrADS_Grid:
                    aGridData = ((GrADSDataInfo)DataInfo).GetGridData_LevelTime(_latIdx, _varIdx, _lonIdx);
                    break;
                case MeteoDataType.GRIB1:
                    aGridData = ((GRIB1DataInfo)DataInfo).GetGridData_LevelTime(_latIdx, _varIdx, _lonIdx);
                    break;
                case MeteoDataType.GRIB2:
                    aGridData = ((GRIB2DataInfo)DataInfo).GetGridData_LevelTime(_latIdx, _varIdx, _lonIdx);
                    break;
                case MeteoDataType.ARL_Grid:
                    aGridData = ((ARLDataInfo)DataInfo).GetGridData_LevelTime(_latIdx, _varIdx, _lonIdx);
                    break;
                case MeteoDataType.NetCDF:
                    aGridData = ((NetCDFDataInfo)DataInfo).GetGridData_LevelTime(_latIdx, _varIdx, _lonIdx);
                    break;
            }

            return aGridData;
        }

        /// <summary>
        /// Get latitude grid data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>grid data</returns>
        public GridData GetGridData_Lat(string varName)
        {
            GridData aGridData = null;
            _varIdx = GetVariableIndex(varName);
            switch (DataType)
            {
                case MeteoDataType.GrADS_Grid:
                    aGridData = ((GrADSDataInfo)DataInfo).GetGridData_Lat(_timeIdx, _lonIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.GRIB1:
                    aGridData = ((GRIB1DataInfo)DataInfo).GetGridData_Lat(_timeIdx, _lonIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.GRIB2:
                    aGridData = ((GRIB2DataInfo)DataInfo).GetGridData_Lat(_timeIdx, _lonIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.ARL_Grid:
                    aGridData = ((ARLDataInfo)DataInfo).GetGridData_Lat(_timeIdx, _lonIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.NetCDF:
                    aGridData = ((NetCDFDataInfo)DataInfo).GetGridData_Lat(_timeIdx, _lonIdx, _varIdx, _levelIdx);
                    break;
            }

            return aGridData;
        }

        /// <summary>
        /// Get longitude grid data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>grid data</returns>
        public GridData GetGridData_Lon(string varName)
        {
            GridData aGridData = null;
            _varIdx = GetVariableIndex(varName);
            switch (DataType)
            {
                case MeteoDataType.GrADS_Grid:
                    aGridData = ((GrADSDataInfo)DataInfo).GetGridData_Lon(_timeIdx, _latIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.GRIB1:
                    aGridData = ((GRIB1DataInfo)DataInfo).GetGridData_Lon(_timeIdx, _latIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.GRIB2:
                    aGridData = ((GRIB2DataInfo)DataInfo).GetGridData_Lon(_timeIdx, _latIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.ARL_Grid:
                    aGridData = ((ARLDataInfo)DataInfo).GetGridData_Lon(_timeIdx, _latIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.NetCDF:
                    aGridData = ((NetCDFDataInfo)DataInfo).GetGridData_Lon(_timeIdx, _lonIdx, _varIdx, _levelIdx);
                    break;
            }

            return aGridData;
        }

        /// <summary>
        /// Get level grid data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>grid data</returns>
        public GridData GetGridData_Level(string varName)
        {
            GridData aGridData = null;
            _varIdx = GetVariableIndex(varName);
            switch (DataType)
            {
                case MeteoDataType.GrADS_Grid:
                    aGridData = ((GrADSDataInfo)DataInfo).GetGridData_Level(_lonIdx, _latIdx, _varIdx, _timeIdx);
                    break;
                case MeteoDataType.GRIB1:
                    aGridData = ((GRIB1DataInfo)DataInfo).GetGridData_Level(_lonIdx, _latIdx, _varIdx, _timeIdx);
                    break;
                case MeteoDataType.GRIB2:
                    aGridData = ((GRIB2DataInfo)DataInfo).GetGridData_Level(_lonIdx, _latIdx, _varIdx, _timeIdx);
                    break;
                case MeteoDataType.ARL_Grid:
                    aGridData = ((ARLDataInfo)DataInfo).GetGridData_Level(_lonIdx, _latIdx, _varIdx, _timeIdx);
                    break;
                case MeteoDataType.NetCDF:
                    aGridData = ((NetCDFDataInfo)DataInfo).GetGridData_Level(_lonIdx, _latIdx, _varIdx, _timeIdx);
                    break;
            }

            return aGridData;
        }

        /// <summary>
        /// Get time grid data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>grid data</returns>
        public GridData GetGridData_Time(string varName)
        {
            GridData aGridData = null;
            _varIdx = GetVariableIndex(varName);
            switch (DataType)
            {
                case MeteoDataType.GrADS_Grid:
                    aGridData = ((GrADSDataInfo)DataInfo).GetGridData_Time(_lonIdx, _latIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.GRIB1:
                    aGridData = ((GRIB1DataInfo)DataInfo).GetGridData_Time(_lonIdx, _latIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.GRIB2:
                    aGridData = ((GRIB2DataInfo)DataInfo).GetGridData_Time(_lonIdx, _latIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.ARL_Grid:
                    aGridData = ((ARLDataInfo)DataInfo).GetGridData_Time(_lonIdx, _latIdx, _varIdx, _levelIdx);
                    break;
                case MeteoDataType.NetCDF:
                    aGridData = ((NetCDFDataInfo)DataInfo).GetGridData_Time(_lonIdx, _latIdx, _varIdx, _levelIdx);
                    break;
            }

            return aGridData;
        }

        ///// <summary>
        ///// Get grid data
        ///// </summary>
        ///// <param name="timeIdx">time index</param>
        ///// <param name="varIdx">variable index</param>
        ///// <param name="levelIdx">level index</param>
        ///// <returns>grid data</returns>
        //public GridData GetGridData(int timeIdx, int varIdx, int levelIdx)
        //{
        //    GridData gridData = null;
        //    switch (DataType)
        //    {
        //        case MeteoDataType.GrADS_Grid:
        //            gridData = ((GrADSDataInfo)DataInfo).GetGridData_LonLat(timeIdx, varIdx, levelIdx);
        //            break;
        //        case MeteoDataType.MICAPS_11:
        //            gridData = ((MICAPS11DataInfo)DataInfo).GetGridData(varIdx);
        //            break;
        //        case MeteoDataType.Sufer_Grid:
        //            gridData = ((SurferGridDataInfo)DataInfo).GetGridData();
        //            break;
        //    }

        //    return gridData;
        //}

        /// <summary>
        /// Get station data
        /// </summary>
        /// <param name="varName">Variable name</param>
        /// <returns>The station data</returns>
        public StationData GetStationData(string varName)
        {
            _varIdx = GetVariableIndex(varName);
            if (_varIdx >= 0)
            {
                return this.GetStationData();
            }
            else
            {
                MathParser mathParser = new MathParser(this);
                StationData stationData = (StationData)mathParser.Evaluate(varName);
                return stationData;
            }            
        }

        /// <summary>
        /// Get station data
        /// </summary>
        /// <returns>The station data</returns>
        public StationData GetStationData()
        {
            if (_varIdx >= 0)
            {
                StationData stData = ((IStationDataInfo)_dataInfo).GetStationData(_timeIdx, _varIdx, _levelIdx);
                return stData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get station model data
        /// </summary>
        /// <returns>station model data array</returns>
        public StationModelData GetStationModelData()
        {
            return ((IStationDataInfo)DataInfo).GetStationModelData(_timeIdx, _levelIdx);            
        }

        /// <summary>
        /// Get station info data
        /// </summary>
        /// <returns>The station info data</returns>
        public StationInfoData GetStationInfoData()
        {
            return ((IStationDataInfo)DataInfo).GetStationInfoData(_timeIdx, _levelIdx);            
        }

        /// <summary>
        /// Get station info data
        /// </summary>
        /// <param name="timeIndex">Time index</param>
        /// <returns>The station info data</returns>
        public StationInfoData GetStationInfoData(int timeIndex)
        {
            return ((IStationDataInfo)DataInfo).GetStationInfoData(timeIndex, _levelIdx);            
        }

        /// <summary>
        /// Get file name
        /// </summary>
        /// <returns>file name</returns>
        public string GetFileName()
        {
            return DataInfo.FileName;
        }

        /// <summary>
        /// Get time number
        /// </summary>
        /// <returns>time number</returns>
        public int GetTimeNumber()
        {
            return DataInfo.TimeNum;            
        }

        /// <summary>
        /// Get DateTime
        /// </summary>
        /// <returns>DateTime</returns>
        public DateTime GetTime()
        {
            return GetTime(_timeIdx);
        }

        /// <summary>
        /// Get DateTime by time index
        /// </summary>
        /// <param name="timeIdx">time index</param>
        /// <returns>DateTime</returns>
        public DateTime GetTime(int timeIdx)
        {
            return DataInfo.Times[timeIdx];
        }

        /// <summary>
        /// Get time index
        /// </summary>
        /// <param name="aTime">a DateTime</param>
        /// <returns>time index</returns>
        public int GetTimeIndex(DateTime aTime)
        {
            int tNum = GetTimeNumber();
            if (aTime < GetTime(0))
                return -1;
            else if (aTime > GetTime(tNum - 1))
                return -1;
            else
            {
                int tIdx = 0;
                for (int i = 0; i < tNum - 1; i++)
                {
                    if (aTime >= GetTime(i) && aTime < GetTime(i + 1))
                    {
                        if (aTime.Subtract(GetTime(i)) > GetTime(i + 1).Subtract(aTime))
                            tIdx = i + 1;
                        else
                            tIdx = i;
                        break;
                    }
                }
                return tIdx;
            }
        }

        /// <summary>
        /// Get level number
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <returns>level number</returns>
        public int GetLevelNumber(string varName)
        {
            int varIdx = GetVariableIndex(varName);
            return GetLevelNumber(varIdx);
        }

        /// <summary>
        /// Get level number
        /// </summary>
        /// <param name="varIdx">variable index</param>
        /// <returns>level number</returns>
        public int GetLevelNumber(int varIdx)
        {
            return DataInfo.Variables[varIdx].LevelNum;            
        }

        /// <summary>
        /// Get level number by first variable
        /// </summary>
        /// <returns>level number</returns>
        public int GetLevelNumber()
        {            
            return GetLevelNumber(0);
        }

        #endregion

        #region Create Layer
        

        #endregion

        #region Others
        /// <summary>
        /// Get variable list
        /// </summary>
        /// <returns>variable list</returns>
        public List<Variable> GetVariables()
        {
            return DataInfo.Variables;            
        }

        /// <summary>
        /// Get variable by name
        /// </summary>
        /// <param name="varName">Variable name</param>
        /// <returns>The variable</returns>
        public Variable GetVariable(string varName)
        {
            int vIdx = GetVariableIndex(varName);
            if (vIdx >= 0)
            {
                return GetVariables()[vIdx];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get variable list
        /// </summary>
        /// <returns>variable name list</returns>
        public List<string> GetVariableNames()
        {
            return DataInfo.VariableNames;
        }

        /// <summary>
        /// Get variable index
        /// </summary>
        /// <param name="varName">Variable name</param>
        /// <returns>Variable index</returns>
        public int GetVariableIndex(string varName)
        {
            List<string> varList = GetVariableNames();
            int idx = varList.IndexOf(varName);            

            return idx;
        }

        /// <summary>
        /// Get time list
        /// </summary>
        /// <returns>time list</returns>
        public List<DateTime> GetTimes()
        {
            return DataInfo.Times;            
        }

        /// <summary>
        /// Get levels
        /// </summary>
        /// <returns>levels</returns>
        public List<double> GetLevels()
        {
            return DataInfo.Variables[_varIdx].Levels;            
        }

        /// <summary>
        /// Get X coordinates
        /// </summary>
        /// <returns>X</returns>
        public double[] GetX()
        {
            double[] X = new double[0];
            switch (DataType)
            {
                case MeteoDataType.ARL_Grid:
                    X = ((ARLDataInfo)DataInfo).X;
                    break;
                case MeteoDataType.ASCII_Grid:
                    X = ((ASCIIGRIDDataInfo)DataInfo).X;
                    break;
                case MeteoDataType.GrADS_Grid:
                    X = ((GrADSDataInfo)DataInfo).X;
                    break;
                case MeteoDataType.GRIB1:
                    X = ((GRIB1DataInfo)DataInfo).X;
                    break;
                case MeteoDataType.GRIB2:
                    X = ((GRIB2DataInfo)DataInfo).X;
                    break;
                case MeteoDataType.HYSPLIT_Conc:
                    X = ((HYSPLITConcDataInfo)DataInfo).X;
                    break;
                case MeteoDataType.NetCDF:
                    X = ((NetCDFDataInfo)DataInfo).X;
                    break;
                case MeteoDataType.MICAPS_4:
                    X = ((MICAPS4DataInfo)DataInfo).X;
                    break;
            }

            return X;
        }

        /// <summary>
        /// Get Y coordinates
        /// </summary>
        /// <returns>Y</returns>
        public double[] GetY()
        {
            double[] Y = new double[0];
            switch (DataType)
            {
                case MeteoDataType.ARL_Grid:
                    Y = ((ARLDataInfo)DataInfo).Y;
                    break;
                case MeteoDataType.ASCII_Grid:
                    Y = ((ASCIIGRIDDataInfo)DataInfo).Y;
                    break;
                case MeteoDataType.GrADS_Grid:
                    Y = ((GrADSDataInfo)DataInfo).Y;
                    break;
                case MeteoDataType.GRIB1:
                    Y = ((GRIB1DataInfo)DataInfo).Y;
                    break;
                case MeteoDataType.GRIB2:
                    Y = ((GRIB2DataInfo)DataInfo).Y;
                    break;
                case MeteoDataType.HYSPLIT_Conc:
                    Y = ((HYSPLITConcDataInfo)DataInfo).Y;
                    break;
                case MeteoDataType.NetCDF:
                    Y = ((NetCDFDataInfo)DataInfo).Y;
                    break;
                case MeteoDataType.MICAPS_4:
                    Y = ((MICAPS4DataInfo)DataInfo).Y;
                    break;
            }

            return Y;
        }

        #endregion

        #region Math parse

        /// <summary>
        /// Get function grid data
        /// </summary>
        /// <param name="functionString">function string</param>
        /// <returns>grid data</returns>
        public double[,] GetFunctionGridData(string functionString)
        {
            double[,] gridData = new double[1, 1];


            return gridData;
        }

        private void GridMathParse(string mathStr)
        {
            int NPart = 0;
            string SubExpr = "";

            mathStr = mathStr.Trim();
            for (int i = 0; i < mathStr.Length; i++)
            {
                string chr = mathStr.Substring(i, 1);
                switch (chr)
                {
                    case " ":    //Skip spaces

                        break;
                    case "(":
                        NPart += 1;
                        if (SubExpr != "")    //eval preceding text
                        {

                        }
                        break;
                    case "+":

                        break;
                    default:

                        break;
                }
            }
        }

        /// <summary>
        /// Parse expression
        /// </summary>
        /// <param name="expr">ref expression string</param>
        /// <returns>return int</returns>
        public int ParseExpr(ref string expr)
        {
            int op, op1;
            op = ParseFactor(ref expr);
            if (expr.Length != 0)
            {
                if (expr[0] == '+')
                {
                    expr = expr.Substring(1);
                    op1 = ParseExpr(ref expr);
                    op += op1;
                }
                else if (expr[0] == '-')
                {
                    expr = expr.Substring(1);
                    op1 = ParseExpr(ref expr);
                    op -= op1;
                }
            }
            return op;
        }

        private int ParseFactor(ref string expr)
        {
            int op, op1;
            op = ParseTerm(ref expr);
            if (expr.Length != 0)
            {
                if (expr[0] == '*')
                {
                    expr = expr.Substring(1);
                    op1 = ParseFactor(ref expr);
                    op *= op1;
                }
                else if (expr[0] == '/')
                {
                    expr = expr.Substring(1);
                    op1 = ParseFactor(ref expr);
                    op /= op1;
                }
            }
            return op;
        }

        private int ParseTerm(ref string expr)
        {
            int returnValue = 0;
            if (expr.Length != 0)
            {
                if (char.IsDigit(expr[0]))
                {
                    returnValue = ParseNumber(ref expr);
                    return returnValue;
                }
                else if (expr[0] == '(')
                {
                    expr = expr.Substring(1);
                    returnValue = ParseExpr(ref expr);
                    return returnValue;
                }
                else if (expr[0] == ')')
                    expr = expr.Substring(1);
            }
            return returnValue;
        }

        private int ParseNumber(ref string expr)
        {
            string numberTemp = "";
            for (int i = 0; i < expr.Length && char.IsDigit(expr[i]); i++)
            {
                if (char.IsDigit(expr[0]))
                {
                    numberTemp += expr[0];
                    expr = expr.Substring(1);
                }
            }
            return int.Parse(numberTemp);
        }

        

        #endregion

        #endregion
    }

}
