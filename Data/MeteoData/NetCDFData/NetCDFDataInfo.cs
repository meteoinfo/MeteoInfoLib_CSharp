using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using MeteoInfoC.Projections;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// NetCDF data info
    /// </summary>
    public class NetCDFDataInfo: DataInfo,IGridDataInfo,ICloneable
    {
        #region Variables
        /// <summary>
        /// ncid of the file
        /// </summary>
        public int ncid;
        /// <summary>
        /// number of dimensions
        /// </summary>
        public int ndims;
        /// <summary>
        /// number of variables
        /// </summary>
        public int nvars;
        /// <summary>
        /// number of global attributes
        /// </summary>
        public int natts;
        /// <summary>
        /// id of unlimited dimension
        /// </summary>
        public int unlimdimid = -1;
        /// <summary>
        /// if data set is lat/lon
        /// </summary>
        public bool isLatLon;
        /// <summary>
        /// dimension list
        /// </summary>
        private List<Dimension> _dimList;
        /// <summary>
        /// global attribute list
        /// </summary>
        public List<AttStruct> gAttList;
        ///// <summary>
        ///// variable list
        ///// </summary>
        //public List<Variable> varList;        
        /// <summary>
        /// levels array
        /// </summary>
        public double[] levels;
        /// <summary>
        /// x coordinate array
        /// </summary>
        public double[] X;
        /// <summary>
        /// y coordinate array
        /// </summary>
        public double[] Y;
        /// <summary>
        /// x delt, y delt
        /// </summary>
        public double XDelt, YDelt;
        /// <summary>
        /// Data convention, CF or others
        /// </summary>
        public Conventions Convention;

        private Variable _xVar = null;
        private Variable _yVar = null;
        private Variable _levelVar = null;
        private Variable _timeVar = null;
        /// <summary>
        /// Times
        /// </summary>
        public List<DateTime> times = new List<DateTime>();

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public NetCDFDataInfo()
        {
            isLatLon = true;
            //varList = new List<List<string>>();
            //levelNum = 0;            
            _dimList = new List<Dimension>();
            gAttList = new List<AttStruct>();
            //varList = new List<Variable>();            
            Convention = Conventions.CF;
        }

        #endregion

        #region Properties        

        /// <summary>
        /// Get dimension name list
        /// </summary>
        public List<string> DimensionNames
        {
            get
            {
                List<string> nameList = new List<string>();
                foreach (Dimension aDim in _dimList)
                {
                    nameList.Add(aDim.DimName);
                }

                return nameList;
            }
        }

        /// <summary>
        /// Get dimensions
        /// </summary>
        public List<Dimension> Dimensions
        {
            get { return _dimList; }
        }

        #endregion

        #region Methods

        #region Read data
        /// <summary>
        /// Read NetCDF data info
        /// </summary>
        /// <param name="aFile">file path</param>       
        /// <returns>is correct</returns>
        public override void ReadDataInfo(string aFile)
        {
            int res;

            res = NetCDF4.nc_open(aFile, (int)NetCDF4.CreateMode.NC_NOWRITE, out ncid);
            if (res != 0) { goto ERROR; }
            res = NetCDF4.nc_inq(ncid, out ndims, out nvars, out natts, out unlimdimid);
            if (res != 0) { goto ERROR; }
            this.FileName = aFile;            

            //Read dimensions
            StringBuilder dimName = new StringBuilder("", (int)NetCDF4.NetCDF_limits.NC_MAX_NAME);
            int i, dimLen;            
            List<string> dimNames = new List<string>();
            for (i = 0; i < ndims; i++)
            {
                dimName = new StringBuilder("", (int)NetCDF4.NetCDF_limits.NC_MAX_NAME);
                res = NetCDF4.nc_inq_dim(ncid, i, dimName, out dimLen);
                if (res != 0) { goto ERROR; }

                Dimension aDimS = new Dimension();
                aDimS.DimName = dimName.ToString();
                aDimS.DimLength = dimLen;
                aDimS.DimId = i;
                _dimList.Add(aDimS);
                dimNames.Add(aDimS.DimName);
            }

            //Read global attribute                                       
            for (i = 0; i < natts; i++)
            {
                AttStruct aAtts = new AttStruct();
                if (ReadAtt(ncid, NetCDF4.NC_GLOBAL, i, ref aAtts))
                {
                    gAttList.Add(aAtts);
                }
            }

            //Get convention
            Convention = GetConvention();

            //Read variables  
            List<Variable> varList = new List<Variable>();      
            for (i = 0; i < nvars; i++)
            {
                Variable aVarS = new Variable();
                if (ReadVar(ncid, i, ref aVarS))
                {
                    //aVarS.isDataVar = true;
                    //if (dimNames.Contains(aVarS.Name))
                    //    aVarS.isDataVar = false;
                    //else
                    //{
                        //List<string> dims = new List<string>();
                        //switch (Convention)
                        //{
                        //    case Conventions.IOAPI:                                
                        //        for (int j = 0; j < aVarS.DimNumber; j++)
                        //            dims.Add(_dimList[aVarS.dimids[j]].DimName);

                        //        if (!dims.Contains("ROW") || !dims.Contains("COL"))
                        //            aVarS.isDataVar = false;
                        //        break;
                        //    case Conventions.WRFOUT:                                
                        //        for (int j = 0; j < aVarS.DimNumber; j++)
                        //            dims.Add(_dimList[aVarS.dimids[j]].DimName);

                        //        if (!dims.Contains("west_east") || !dims.Contains("south_north"))
                        //            aVarS.isDataVar = false;
                        //        if (dims.Contains("west_east_stag") || dims.Contains("south_north_stag"))
                        //            aVarS.isDataVar = true;
                        //        break;
                        //}                        
                    //}

                    varList.Add(aVarS);
                }
            }
            this.Variables = varList;

            //Get projection
            GetProjection();

            //Get x(longitude)/y(latitude)z/(level)/time dimension values
            bool isSupported = GetDimensionValues();
            
            if (!isSupported)
            {
                //Close file
                NetCDF4.nc_close(ncid);
                MessageBox.Show("The NetCDF data is not supported at present!", "Error");
                return;
            }

            GetVariableLevels();            

            return;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            NetCDF4.nc_close(ncid);
            return;
        }

        private bool ReadAtt(int ncid, int varid, int attnum, ref AttStruct aAttS)
        {
            int res;
            StringBuilder attName = new StringBuilder("", (int)NetCDF4.NetCDF_limits.NC_MAX_NAME);
            res = NetCDF4.nc_inq_attname(ncid, varid, attnum, attName);
            if (res != 0) { goto ERROR; }
            aAttS.attName = attName.ToString();
            int attLen, aType;
            res = NetCDF4.nc_inq_att(ncid, varid, attName, out aType, out attLen);
            if (res != 0) { goto ERROR; }
            //res = NetCDF4.nc_inq_atttype(ncid, varid, attName, out aType);
            //if (res != 0) { goto ERROR; }
            
            aAttS.NCType = (NetCDF4.NcType)aType;
            aAttS.attLen = attLen;
            switch (aAttS.NCType)
            {
                case NetCDF4.NcType.NC_CHAR:
                    StringBuilder atttext = new StringBuilder("", attLen);
                    res = NetCDF4.nc_get_att_text(ncid, varid, attName.ToString(), atttext);
                    if (res != 0) { goto ERROR; }
                    aAttS.attValue = atttext.ToString();
                    if (((string)aAttS.attValue).Length > attLen)
                    {
                        aAttS.attValue = ((string)aAttS.attValue).Substring(0, attLen);
                    }
                    break;
                case NetCDF4.NcType.NC_BYTE:
                    byte[] attbyte = new byte[attLen];
                    res = NetCDF4.nc_get_att_uchar(ncid, varid, attName.ToString(), attbyte);
                    if (res != 0) { goto ERROR; }
                    aAttS.attValue = attbyte;
                    break;
                case NetCDF4.NcType.NC_INT:
                    int[] attint = new int[attLen];
                    res = NetCDF4.nc_get_att_int(ncid, varid, attName.ToString(), attint);
                    if (res != 0) { goto ERROR; }
                    aAttS.attValue = attint;
                    break;
                case NetCDF4.NcType.NC_SHORT:
                    Int16[] attshort = new Int16[attLen];
                    res = NetCDF4.nc_get_att_short(ncid, varid, attName.ToString(), attshort);
                    if (res != 0) { goto ERROR; }
                    aAttS.attValue = attshort;
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    Single[] attfloat = new Single[attLen];
                    res = NetCDF4.nc_get_att_float(ncid, varid, attName.ToString(), attfloat);
                    if (res != 0) { goto ERROR; }
                    aAttS.attValue = attfloat;
                    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    double[] attdouble = new double[attLen];
                    res = NetCDF4.nc_get_att_double(ncid, varid, attName.ToString(), attdouble);
                    if (res != 0) { goto ERROR; }
                    aAttS.attValue = attdouble;
                    break;
            }

            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }

        private bool ReadVar(int ncid, int varid, ref Variable aVarS)
        {
            int res, i;
            int ndims, natts, aType;
            int[] dimids;
            StringBuilder varName = new StringBuilder("", (int)NetCDF4.NetCDF_limits.NC_MAX_NAME);
            res = NetCDF4.nc_inq_varndims(ncid, varid, out ndims);
            if (res != 0) { goto ERROR; }
            dimids = new int[ndims];
            res = NetCDF4.nc_inq_var(ncid, varid, varName, out aType, out ndims, dimids, out natts);
            if (res != 0) { goto ERROR; }            

            aVarS.Name = varName.ToString();
            aVarS.VarId = varid;
            //aVarS.nDims = ndims;
            aVarS.NCType = (NetCDF4.NcType)aType;
            aVarS.AttNumber = natts;
            //aVarS.dimids = dimids;
            aVarS.Dimensions = new List<Dimension>();
            for (i = 0; i < dimids.Length; i++)
            {
                for (int j = 0; j < _dimList.Count; j++)
                {
                    if (_dimList[j].DimId == dimids[i])
                    {
                        aVarS.Dimensions.Add(_dimList[j]);
                        break;
                    }
                }
            }
            aVarS.Attributes = new List<AttStruct>();

            //Read variation attribute
            for (i = 0; i < natts; i++)
            {
                AttStruct aAtts = new AttStruct();
                ReadAtt(ncid, varid, i, ref aAtts);
                aVarS.Attributes.Add(aAtts);
            }            

            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }

        private Dimension findDimension(string dimName)
        {
            foreach (Dimension dim in _dimList)
            {
                if (dim.DimName.Equals(dimName, StringComparison.OrdinalIgnoreCase))
                    return dim;
            }

            return null;
        }
     
        /// <summary>
        /// Get start time and time unit
        /// </summary>
        /// <returns>Start time and time unit</returns>
        public object[] GetStartTimeInfo()
        {
            int unitsId = 0;
            string unitsStr;
            Variable aVar = GetTimeVariable();
            for (int i = 0; i < aVar.AttNumber; i++)
            {
                if (aVar.Attributes[i].attName == "units")
                {
                    unitsId = i;
                    break;
                }
            }

            unitsStr = (string)aVar.Attributes[unitsId].attValue;

            DateTime sTime = new DateTime();
            TimeUnit aTU = new TimeUnit();
            GetPSDTimeInfo(unitsStr, ref sTime, ref aTU);

            return new object[] { sTime, aTU };
        }

        /// <summary>
        /// Get start time and time unit from time string
        /// </summary>
        /// <param name="tStr">Time string</param>
        /// <param name="sTime">Start time</param>
        /// <param name="aTU">Time unit</param>
        private void GetPSDTimeInfo(string tStr, ref DateTime sTime, ref TimeUnit aTU)
        {
            aTU = TimeUnit.Second;
            tStr = tStr.Trim();
            string tu;
            string[] dataArray;
            List<string> dataList = new List<string>();
            int i;

            dataArray = tStr.Split();
            int LastNonEmpty = -1;
            for (i = 0; i < dataArray.Length; i++)
            {
                if (dataArray[i] != string.Empty)
                {
                    LastNonEmpty++;
                    dataList.Add(dataArray[i]);
                }
            }

            if (dataList.Count < 2)
                return;

            //Get time unit
            tu = dataList[0];
            if (tu.ToLower().Contains("second"))
                aTU = TimeUnit.Second;
            else
            {
                if (tu.Length == 1)
                {
                    switch (tu.ToLower())
                    {
                        case "y":
                            aTU = TimeUnit.Year;
                            break;
                        case "d":
                            aTU = TimeUnit.Day;
                            break;
                        case "h":
                            aTU = TimeUnit.Hour;
                            break;
                        case "s":
                            aTU = TimeUnit.Second;
                            break;
                    }
                }
                else
                {
                    switch (tu.ToLower().Substring(0, 2))
                    {
                        case "yr":
                        case "ye":
                            aTU = TimeUnit.Year;
                            break;
                        case "mo":
                            aTU = TimeUnit.Month;
                            break;
                        case "da":
                            aTU = TimeUnit.Day;
                            break;
                        case "hr":
                        case "ho":
                            aTU = TimeUnit.Hour;
                            break;
                        case "mi":
                            aTU = TimeUnit.Minute;
                            break;
                        case "se":
                            aTU = TimeUnit.Second;
                            break;
                    }
                }
            }

            //Get start time
            string ST;
            ST = dataList[2];
            if (ST.Contains("T"))
            {
                dataList.Add(ST.Split('T')[1]);
                ST = ST.Split('T')[0];
            }
            int year = int.Parse(ST.Split('-')[0]);
            int month = int.Parse(ST.Split('-')[1]);
            int day = int.Parse(ST.Split('-')[2]);
            if (day > 1000)
            {
                int temp = year;
                year = day;
                day = temp;
            }
            int hour = 0;
            int min = 0;
            int sec = 0;
            if (dataList.Count >= 4)
            {
                string hmsStr = dataList[3];
                hmsStr = hmsStr.Replace("0.0", "00");
                try
                {
                    hour = int.Parse(hmsStr.Split(':')[0]);
                    min = int.Parse(hmsStr.Split(':')[1]);
                    sec = int.Parse(hmsStr.Split(':')[2]);
                }
                catch { }
            }

            if (year == 0)
                year = 1;
            //else if (year < 100)
            //    year += 2000;

            sTime = new DateTime(year, month, day, hour, min, sec);

            //if (ST.Split('-')[0] == "1")
            //{
            //    ST = "200" + ST;
            //}
            //switch (dataList.Count)
            //{                
            //    case 4:
            //        ST += " " + dataList[3];
            //        ST = ST.Replace("0.0", "00");                    
            //        break;
            //    case 5:
            //        ST += " " + dataList[3];
            //        ST = ST.Replace("0.0", "00");                    
            //        break;
            //}
            //if (ST.Substring(0, 4) == "0000")
            //    ST = "0001" + ST.Substring(4);
            //sTime = DateTime.Parse(ST);
        }

        private double GetValidVarAtt(AttStruct aAttS)
        {
            double aValue = 0;
            switch (aAttS.NCType)
            {
                case NetCDF4.NcType.NC_INT:
                    aValue = ((int[])aAttS.attValue)[0];
                    break;
                case NetCDF4.NcType.NC_SHORT:
                    aValue = ((Int16[])aAttS.attValue)[0];
                    break;
                case NetCDF4.NcType.NC_BYTE:
                    aValue = ((byte[])aAttS.attValue)[0];
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    aValue = ((Single[])aAttS.attValue)[0];
                    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    aValue = ((double[])aAttS.attValue)[0];
                    break;
            }

            return aValue;
        }

        private Conventions GetConvention()
        {
            Conventions convention = Convention;
            bool isIOAPI = false;
            bool isWRFOOUT = false;
            List<string> WRFStrings = new List<string>();

            foreach (AttStruct aAtts in gAttList)
            {
                if (aAtts.attName.ToLower() == "ioapi_version")
                {
                    isIOAPI = true;
                    break;
                }
                if (aAtts.attName.ToUpper() == "TITLE")
                {
                    string title = GetAttStr(aAtts);
                    if (title.ToUpper().Contains("OUTPUT FROM WRF") || title.ToUpper().Contains("OUTPUT FROM GEOGRID")
                        || title.ToUpper().Contains("OUTPUT FROM GRIDGEN") || title.ToUpper().Contains("OUTPUT FROM METGRID"))
                        isWRFOOUT = true;
                    break;
                }
            }

            if (isIOAPI)
                convention = Conventions.IOAPI;

            if (isWRFOOUT)
                convention = Conventions.WRFOUT;

            return convention;
        }

        private void GetProjection()
        {
            string projStr = this.ProjectionInfo.ToProj4String();
            switch (Convention)
            {
                case Conventions.CF:
                    Variable pVar = null;
                    int pvIdx = -1;
                    foreach (Variable aVarS in Variables)
                    {
                        pvIdx = aVarS.GetAttributeIndex("grid_mapping_name");
                        if (pvIdx > -1)
                        {
                            pVar = aVarS;
                            break;
                        }
                    }

                    if (pVar != null)
                    {
                        AttStruct pAtt = pVar.Attributes[pvIdx];
                        switch (pAtt.attValue.ToString())
                        {
                            case "albers_conical_equal_area":
                                //Two standard parallels condition need to be considered  
                                string spStr = pVar.GetAttributeString("standard_parallel");
                                string sp1;
                                string sp2 = String.Empty;
                                if (spStr.Contains(','))
                                {
                                    sp1 = spStr.Split(',')[0].Trim();
                                    sp2 = spStr.Split(',')[1].Trim();
                                }
                                else
                                    sp1 = spStr;
                                projStr = "+proj=aea" +
                                    "+lat_1=" + sp1;
                                if (!String.IsNullOrEmpty(sp2))
                                    projStr += "+lat_2=" + sp2;

                                projStr += "+lon_0=" + pVar.GetAttributeString("longitude_of_central_meridian") +
                                    "+lat_0=" + pVar.GetAttributeString("latitude_of_projection_origin") +
                                    "+x_0=" + pVar.GetAttributeString("false_easting") +
                                    "+y_0=" + pVar.GetAttributeString("false_northing");
                                break;
                            case "azimuthal_equidistant":
                                projStr = "+proj=aeqd" +
                                    "+lon_0=" + pVar.GetAttributeString("longitude_of_projection_origin") +
                                    "+lat_0=" + pVar.GetAttributeString("latitude_of_projection_origin") +
                                    "+x_0=" + pVar.GetAttributeString("false_easting") +
                                    "+y_0=" + pVar.GetAttributeString("false_northing");
                                break;
                            case "lambert_azimuthal_equal_area":
                                projStr = "+proj=laea" +
                                    "+lon_0=" + pVar.GetAttributeString("longitude_of_projection_origin") +
                                    "+lat_0=" + pVar.GetAttributeString("latitude_of_projection_origin") +
                                    "+x_0=" + pVar.GetAttributeString("false_easting") +
                                    "+y_0=" + pVar.GetAttributeString("false_northing");
                                break;
                            case "lambert_conformal_conic":
                                //Two standard parallels condition need to be considered
                                spStr = pVar.GetAttributeString("standard_parallel");                                
                                sp2 = String.Empty;
                                if (spStr.Contains(','))
                                {
                                    sp1 = spStr.Split(',')[0].Trim();
                                    sp2 = spStr.Split(',')[1].Trim();
                                }
                                else
                                    sp1 = spStr;
                                projStr = "+proj=lcc" +
                                    "+lat_1=" + sp1;
                                if (!String.IsNullOrEmpty(sp2))
                                    projStr += "+lat_2=" + sp2;
                                
                                projStr += "+lon_0=" + pVar.GetAttributeString("longitude_of_central_meridian") +
                                    "+lat_0=" + pVar.GetAttributeString("latitude_of_projection_origin") +
                                    "+x_0=" + pVar.GetAttributeString("false_easting") +
                                    "+y_0=" + pVar.GetAttributeString("false_northing");
                                break;
                            case "lambert_cylindrical_equal_area":
                                projStr = "+proj=cea" +
                                    "+lon_0=" + pVar.GetAttributeString("longitude_of_central_meridian");

                                if (pVar.GetAttributeIndex("standard_parallel") > -1)
                                    projStr += "+lat_ts=" + pVar.GetAttributeString("standard_parallel");
                                else if (pVar.GetAttributeIndex("scale_factor_at_projection_origin") > -1)
                                    projStr += "+k_0=" + pVar.GetAttributeString("scale_factor_at_projection_origin"); 

                                projStr += "+x_0=" + pVar.GetAttributeString("false_easting") +
                                    "+y_0=" + pVar.GetAttributeString("false_northing");
                                break;
                            case "latitude_longitude":

                                break;
                            case "mercator":
                                projStr = "+proj=merc" +
                                    "+lon_0=" + pVar.GetAttributeString("longitude_of_projection_origin");

                                if (pVar.GetAttributeIndex("standard_parallel") > -1)
                                    projStr += "+lat_ts=" + pVar.GetAttributeString("standard_parallel");
                                else if (pVar.GetAttributeIndex("scale_factor_at_projection_origin") > -1)
                                    projStr += "+k_0=" + pVar.GetAttributeString("scale_factor_at_projection_origin");

                                projStr += "+x_0=" + pVar.GetAttributeString("false_easting") +
                                    "+y_0=" + pVar.GetAttributeString("false_northing");                               
                                break;
                            case "orthographic":
                                projStr = "+proj=ortho" +
                                    "+lon_0=" + pVar.GetAttributeString("longitude_of_projection_origin") +
                                    "+lat_0=" + pVar.GetAttributeString("latitude_of_projection_origin") +
                                    "+x_0=" + pVar.GetAttributeString("false_easting") +
                                    "+y_0=" + pVar.GetAttributeString("false_northing");
                                break;
                            case "polar_stereographic":
                                projStr = "+proj=stere" +
                                    "+lon_0=" + pVar.GetAttributeString("straight_vertical_longitude_from_pole") +
                                    "+lat_0=" + pVar.GetAttributeString("latitude_of_projection_origin");

                                if (pVar.GetAttributeIndex("standard_parallel") > -1)
                                {                    
                                    string stPs = pVar.GetAttributeString("standard_parallel");
                                    //projStr += "+lat_ts=" + stPs;                                    
                                    double k0 = Proj.CalScaleFactorFromStandardParallel(double.Parse(stPs));
                                    projStr += "+k=" + k0.ToString("0.00");
                                }
                                else if (pVar.GetAttributeIndex("scale_factor_at_projection_origin") > -1)
                                    projStr += "+k_0=" + pVar.GetAttributeString("scale_factor_at_projection_origin");                                

                                projStr += "+x_0=" + pVar.GetAttributeString("false_easting");
                                projStr += "+y_0=" + pVar.GetAttributeString("false_northing");                                                           
                                break;
                            case "rotated_latitude_longitude":

                                break;
                            case "stereographic":
                                projStr = "+proj=stere" +
                                    "+lon_0=" + pVar.GetAttributeString("longitude_of_projection_origin") +
                                    "+lat_0=" + pVar.GetAttributeString("latitude_of_projection_origin") +
                                    "+k_0=" + pVar.GetAttributeString("scale_factor_at_projection_origin") +
                                    "+x_0=" + pVar.GetAttributeString("false_easting") +
                                    "+y_0=" + pVar.GetAttributeString("false_northing");
                                break;
                            case "transverse_mercator":
                                projStr = "+proj=tmerc" +
                                    "+lon_0=" + pVar.GetAttributeString("longitude_of_central_meridian") +
                                    "+lat_0=" + pVar.GetAttributeString("latitude_of_projection_origin") +
                                    "+k_0=" + pVar.GetAttributeString("scale_factor_at_central_meridian") +
                                    "+x_0=" + pVar.GetAttributeString("false_easting") +
                                    "+y_0=" + pVar.GetAttributeString("false_northing");
                                break;
                            case "vertical_perspective":
                                projStr = "+proj=geos" +
                                    "+lon_0=" + pVar.GetAttributeString("longitude_of_projection_origin") +
                                    "+lat_0=" + pVar.GetAttributeString("latitude_of_projection_origin") +
                                    "+h=" + pVar.GetAttributeString("perspective_point_height") +
                                    "+x_0=" + pVar.GetAttributeString("false_easting") +
                                    "+y_0=" + pVar.GetAttributeString("false_northing");
                                break;

                        }
                    }
                    break;
                case Conventions.IOAPI:
                    int gridType = int.Parse(GetGlobalAttStr("GDTYP"));
                    switch (gridType)
                    {
                        case 1:    //Lat-Lon
                            break;
                        case 2:    //Lambert conformal conic
                            projStr = "+proj=lcc" +
                                "+lon_0=" + GetGlobalAttStr("P_GAM") +
                                "+lat_0=" + GetGlobalAttStr("YCENT") +
                                "+lat_1=" + GetGlobalAttStr("P_ALP") +
                                "+lat_2=" + GetGlobalAttStr("P_BET");                            
                            break;
                        case 3:    //General Mercator (hotine oblique mercator)
                            projStr = "+proj=omerc" +
                                "+lat_0=" + GetGlobalAttStr("P_ALP") +
                                "+lonc=" + GetGlobalAttStr("P_BET") +
                                "+alpha=" + GetGlobalAttStr("P_GAM");                                
                            break;
                        case 4:    //tangent stereographic
                            projStr = "+proj=stere" +
                                "+lon_0=" + GetGlobalAttStr("P_BET") +
                                "+lat_0=" + GetGlobalAttStr("P_ALP");
                            break;
                        case 5:    //UTM
                            projStr = "+proj=utm" +
                                "+zone=" + GetGlobalAttStr("P_ALP");
                            break;
                        case 6:    //polar secant stereographic
                            string lat0 = "90";
                            if (GetGlobalAttStr("P_ALP").Substring(0, 1) == "-")
                                lat0 = "-90";
                            projStr = "+proj=stere" +
                                "+lon_0=" + GetGlobalAttStr("P_GAM") +
                                "+lat_0=" + lat0;                                
                            string stPs = GetGlobalAttStr("P_BET");
                            //projStr += "+lat_ts=" + stPs;                                    
                            double k0 = Proj.CalScaleFactorFromStandardParallel(double.Parse(stPs));
                            projStr += "+k=" + k0.ToString();
                            break;
                        case 7:    //Equatorial Mercator
                            projStr = "+proj=merc" +
                                "+lat_ts=" + GetGlobalAttStr("P_ALP") +
                                "+lon_0=" + GetGlobalAttStr("P_GAM");
                            break;
                        case 8:    //Transverse Mercator 
                            projStr = "+proj=tmerc" +
                                "+lon_0=" + GetGlobalAttStr("P_GAM") +
                                "+lat_0=" + GetGlobalAttStr("P_ALP");
                            break;
                        case 9:    //Albers Equal-Area Conic
                            projStr = "+proj=aea" +
                                "+lat_1=" + GetGlobalAttStr("P_ALP") +
                                "+lat_2=" + GetGlobalAttStr("P_BET") +
                                "+lat_0=" + GetGlobalAttStr("YCENT") +
                                "+lon_0=" + GetGlobalAttStr("P_GAM");
                            break;
                        case 10:    //Lambert Azimuthal Equal-Area
                            projStr = "+proj=laea" +
                                "+lat_0=" + GetGlobalAttStr("P_ALP") +
                                "+lon_0=" + GetGlobalAttStr("P_GAM");
                            break;
                    }
                    break;
                case Conventions.WRFOUT:
                    int mapProj = int.Parse(GetGlobalAttStr("MAP_PROJ"));
                    switch (mapProj)
                    {
                        case 1:    //Lambert conformal
                            projStr = "+proj=lcc" +
                                "+lon_0=" + GetGlobalAttStr("STAND_LON") +
                                "+lat_0=" + GetGlobalAttStr("CEN_LAT") +
                                "+lat_1=" + GetGlobalAttStr("TRUELAT1") +
                                "+lat_2=" + GetGlobalAttStr("TRUELAT2");                            
                            break;
                        case 2:    //Polar Stereographic
                            string lat0 = "90";
                            if (GetGlobalAttStr("POLE_LAT").Substring(0, 1) == "-")
                                lat0 = "-90";
                            projStr = "+proj=stere" +
                                "+lon_0=" + GetGlobalAttStr("STAND_LON") +
                                "+lat_0=" + lat0;
                            string stPs = GetGlobalAttStr("CEN_LAT");
                            //projStr += "+lat_ts=" + stPs;                                    
                            double k0 = Proj.CalScaleFactorFromStandardParallel(double.Parse(stPs));
                            projStr += "+k=" + k0.ToString();                               
                            break;                            
                        case 3:    //Mercator
                            projStr = "+proj=merc" +
                                "+lat_ts=" + GetGlobalAttStr("CEN_LAT") +
                                "+lon_0=" + GetGlobalAttStr("STAND_LON");
                            break;
                    }
                    break;
            }
            if (projStr != this.ProjectionInfo.ToProj4String())
                this.ProjectionInfo = new ProjectionInfo(projStr);
        }

        private int GetVarLength(Variable aVarS)
        {
            int dataLen = 1;
            for (int i = 0; i < aVarS.DimNumber; i++)
            {
                dataLen = dataLen * _dimList[aVarS.Dimensions[i].DimId].DimLength;
            }

            return dataLen;
        }

        //private void GetDimValues_old()
        //{
        //    foreach (Variable aVar in varList)
        //    {
        //        if (aVar.DimNumber != 1)
        //            continue;

        //        foreach (Dimension aDim in _dimList)
        //        {
        //            if (aVar.Name == aDim.DimName)
        //            {
        //                aVar.IsCoorVar = true;
        //                aDim.DimType = GetDimType(aVar);
        //                double[] values = new double[aDim.DimLength];
        //                GetVarData(ncid, aVar.VarId, aDim.DimLength, aVar.NCType, ref values);
        //                aDim.SetValues(new List<double>(values));
        //                switch (aDim.DimType)
        //                {
        //                    case DimensionType.X:
        //                        X = values;
        //                        if (X[0] > X[1])
        //                        {
        //                            Array.Reverse(X);
        //                            this.IsXReverse = true;
        //                        }
        //                        XDelt = X[1] - X[0];
        //                        if (this.ProjectionInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat)
        //                        {
        //                            if (X[aDim.DimLength - 1] + XDelt -
        //                                X[0] == 360)
        //                            {
        //                                this.IsGlobal = true;
        //                            }
        //                        }
        //                        break;
        //                    case DimensionType.Y:
        //                        Y = values;
        //                        if (Y[0] > Y[1])
        //                        {
        //                            Array.Reverse(Y);
        //                            this.IsYReverse = true;
        //                        }
        //                        YDelt = Y[1] - Y[0];
        //                        break;
        //                    case DimensionType.Z:
        //                        levels = values;
        //                        break;
        //                    case DimensionType.T:
        //                        //Get start time
        //                        int unitsId = 0;
        //                        string unitsStr;
        //                        int i;
        //                        for (i = 0; i < aVar.AttNumber; i++)
        //                        {
        //                            if (aVar.Attributes[i].attName == "units")
        //                            {
        //                                unitsId = i;
        //                                break;
        //                            }
        //                        }

        //                        unitsStr = (string)aVar.Attributes[unitsId].attValue;
        //                        if (unitsStr.Contains("as"))
        //                        {
        //                            //Get data time
        //                            double[] DTimes = values;
        //                            for (i = 0; i < DTimes.Length; i++)
        //                            {
        //                                string md = ((int)DTimes[i]).ToString();
        //                                if (md.Length <= 3)
        //                                    md = "0" + md;
        //                                times.Add(DateTime.ParseExact("2001" + md, "yyyyMMdd", null));
        //                            }
        //                        }
        //                        else
        //                        {
        //                            DateTime sTime = new DateTime();
        //                            TimeUnit aTU = new TimeUnit();
        //                            GetPSDTimeInfo(unitsStr, ref sTime, ref aTU);

        //                            //Get data time
        //                            double[] DTimes = values;
        //                            for (i = 0; i < aDim.DimLength; i++)
        //                            {
        //                                switch (aTU)
        //                                {
        //                                    case TimeUnit.Year:
        //                                        times.Add(sTime.AddYears((int)DTimes[i]));
        //                                        break;
        //                                    case TimeUnit.Month:
        //                                        times.Add(sTime.AddMonths((int)DTimes[i]));
        //                                        break;
        //                                    case TimeUnit.Day:
        //                                        times.Add(sTime.AddDays(DTimes[i]));
        //                                        break;
        //                                    case TimeUnit.Hour:
        //                                        times.Add(sTime.AddHours(DTimes[i]));
        //                                        break;
        //                                    case TimeUnit.Minute:
        //                                        times.Add(sTime.AddMinutes(DTimes[i]));
        //                                        break;
        //                                    case TimeUnit.Second:
        //                                        times.Add(sTime.AddSeconds(DTimes[i]));
        //                                        break;
        //                                }
        //                            }
        //                        }
        //                        break;
        //                }
        //                break;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Get time coordinate variable
        /// </summary>
        /// <returns>Time variable</returns>
        public Variable GetTimeVariable()
        {
            Variable tVar = null;
            foreach (Variable var in Variables)
            {
                if (var.DimNumber == 1)
                {
                    if (GetDimType(var) == DimensionType.T)
                    {
                        tVar = var;
                        break;
                    }
                }
            }

            return tVar;
        }

        private void GetDimValues()
        {
            List<Variable> oneDimVars = new List<Variable>();
            foreach (Variable aVar in Variables)
            {
                if (aVar.DimNumber == 1)
                    oneDimVars.Add(aVar);
            }

            foreach (Dimension aDim in _dimList)
            {
                bool isFind = false;
                Variable aVar = null;
                foreach (Variable var in oneDimVars)
                {
                    if (aDim.DimName == var.Name)
                    {
                        isFind = true;
                        aVar = var;
                        break;
                    }
                }

                if (!isFind)
                {                    
                    foreach (Variable var in oneDimVars)
                    {
                        if (aDim.DimName == var.Dimensions[0].DimName)
                        {
                            isFind = true;
                            aVar = var;
                            break;
                        }
                    }
                }

                if (isFind)
                {
                    aVar.IsCoorVar = true;
                    aDim.DimType = GetDimType(aVar);
                    double[] values = new double[aDim.DimLength];
                    GetVarData(ncid, aVar.VarId, aDim.DimLength, aVar.NCType, ref values);
                    //aDim.SetValues(new List<double>(values));
                    switch (aDim.DimType)
                    {
                        case DimensionType.X:
                            X = values;
                            if (X[0] > X[1])
                            {
                                Array.Reverse(X);
                                this.IsXReverse = true;
                            }
                            XDelt = X[1] - X[0];
                            if (this.ProjectionInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat)
                            {
                                if (X[aDim.DimLength - 1] + XDelt -
                                    X[0] == 360)
                                {
                                    this.IsGlobal = true;
                                }
                            }
                            aDim.SetValues(X);
                            this.XDimension = aDim;
                            break;
                        case DimensionType.Y:
                            Y = values;
                            if (Y[0] > Y[1])
                            {
                                Array.Reverse(Y);
                                this.IsYReverse = true;
                            }
                            YDelt = Y[1] - Y[0];
                            aDim.SetValues(Y);
                            this.YDimension = aDim;
                            break;
                        case DimensionType.Z:
                            levels = values;
                            aDim.SetValues(levels);
                            this.ZDimension = aDim;
                            break;
                        case DimensionType.T:
                            //Get start time
                            int unitsId = 0;
                            string unitsStr;
                            int i;
                            for (i = 0; i < aVar.AttNumber; i++)
                            {
                                if (aVar.Attributes[i].attName == "units")
                                {
                                    unitsId = i;
                                    break;
                                }
                            }

                            unitsStr = (string)aVar.Attributes[unitsId].attValue;
                            if (unitsStr.Contains("as"))
                            {
                                //Get data time
                                double[] DTimes = values;
                                for (i = 0; i < DTimes.Length; i++)
                                {
                                    string md = ((int)DTimes[i]).ToString();
                                    if (md.Length <= 3)
                                        md = "0" + md;
                                    //times.Add(DateTime.ParseExact("2001" + md, "yyyyMMdd", null));
                                    times.Add(DateTime.ParseExact(md, "yyyyMMdd", null));
                                }
                            }
                            else
                            {
                                DateTime sTime = new DateTime();
                                TimeUnit aTU = new TimeUnit();
                                GetPSDTimeInfo(unitsStr, ref sTime, ref aTU);

                                //Get data time
                                double[] DTimes = values;
                                for (i = 0; i < aDim.DimLength; i++)
                                {
                                    switch (aTU)
                                    {
                                        case TimeUnit.Year:
                                            times.Add(sTime.AddYears((int)DTimes[i]));
                                            break;
                                        case TimeUnit.Month:
                                            times.Add(sTime.AddMonths((int)DTimes[i]));
                                            break;
                                        case TimeUnit.Day:
                                            times.Add(sTime.AddDays(DTimes[i]));
                                            break;
                                        case TimeUnit.Hour:
                                            if (sTime.Year <= 1582 && sTime.Month <= 10 && sTime.Day <= 4 && DTimes[i] > 48)
                                                times.Add(sTime.AddHours(DTimes[i] - 48));
                                            else
                                                try
                                                {
                                                    times.Add(sTime.AddHours(DTimes[i]));
                                                }
                                                catch 
                                                {
                                                    for (int n = 0; n < DTimes.Length; n++)
                                                        DTimes[n] += 48;
                                                    times.Add(sTime.AddHours(DTimes[i]));
                                                }                                                
                                            break;
                                        case TimeUnit.Minute:
                                            times.Add(sTime.AddMinutes(DTimes[i]));
                                            break;
                                        case TimeUnit.Second:
                                            times.Add(sTime.AddSeconds(DTimes[i]));
                                            break;
                                    }
                                }
                            }
                            double[] tvalues = new double[times.Count];
                            for (i = 0; i < times.Count; i++)
                            {
                                tvalues[i] = DataConvert.ToDouble(times[i]);
                            }

                            aDim.SetValues(tvalues);
                            this.TimeDimension = aDim;
                            break;
                    }                    
                }
            }
        }

        private DimensionType GetDimType(Variable aVar)
        {
            string sName;
            DimensionType dimType = DimensionType.Other;
            if (aVar.GetAttributeIndex("axis") > -1)
            {
                sName = aVar.GetAttributeString("axis").Trim();
                switch (sName.ToLower())
                {
                    case "x":
                        dimType = DimensionType.X;
                        break;
                    case "y":
                        dimType = DimensionType.Y;
                        break;
                    case "z":
                        dimType = DimensionType.Z;
                        break;
                    case "t":
                        dimType = DimensionType.T;
                        break;
                }
            }
            if (dimType == DimensionType.Other)
            {
                if (aVar.GetAttributeIndex("standard_name") > -1)
                {
                    sName = aVar.GetAttributeString("standard_name").Trim();
                    switch (sName.ToLower())
                    {
                        case "longitude":
                        case "projection_x_coordinate":
                        case "longitude_east":
                            dimType = DimensionType.X;
                            break;
                        case "latitude":
                        case "projection_y_coordinate":
                        case "latitude_north":
                            dimType = DimensionType.Y;
                            break;
                        case "time":
                            dimType = DimensionType.T;
                            break;
                        case "level":
                            dimType = DimensionType.Z;
                            break;
                    }
                }
            }
            if (dimType == DimensionType.Other)
            {
                if (aVar.GetAttributeIndex("long_name") > -1)
                {
                    sName = aVar.GetAttributeString("long_name").Trim();
                    switch (sName.ToLower())
                    {
                        case "longitude":
                        case "coordinate longitude":
                        case "x":
                            dimType = DimensionType.X;
                            break;
                        case "latitude":
                        case "coordinate latitude":
                        case "y":
                            dimType = DimensionType.Y;
                            break;
                        case "time":
                        case "initial time":
                            dimType = DimensionType.T;
                            break;
                        case "level":
                        case "pressure_level":
                            dimType = DimensionType.Z;
                            break;
                        default:
                            if (sName.ToLower().Contains("level") || sName.ToLower().Contains("depths"))
                                dimType = DimensionType.Z;
                            break;
                    }
                }
            }
            if (dimType == DimensionType.Other)            
            {
                switch (aVar.Name.ToLower())
                {
                    case "lon":
                    case "longitude":
                    case "x":
                        dimType = DimensionType.X;
                        break;
                    case "lat":
                    case "latitude":
                    case "y":
                        dimType = DimensionType.Y;
                        break;
                    case "time":
                        dimType = DimensionType.T;
                        break;
                    case "level":
                    case "lev":
                    case "height":
                        dimType = DimensionType.Z;
                        break;
                }
            }

            return dimType;
        }

        private void GetDimValues_CF()
        {
            int res = 0, dimLen, i;
            DateTime sTime = new DateTime();

            GetDimensionVariables();
            if (_xVar != null)
            {
                res = NetCDF4.nc_inq_dimlen(ncid, _xVar.Dimensions[0].DimId, out dimLen);
                if (res != 0) { goto ERROR; }

                X = new double[dimLen];
                GetVarData(ncid, _xVar.VarId, dimLen, _xVar.NCType, ref X);
                if (X[0] > X[1])
                {
                    Array.Reverse(X);
                    this.IsXReverse = true;
                }
                XDelt = X[1] - X[0];
                if (this.ProjectionInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat)
                //if (_xVar.GetAttributeString("standard_name") == "longitude")
                {
                    if (X[dimLen - 1] + XDelt -
                        X[0] == 360)
                    {
                        this.IsGlobal = true;
                    }
                }
            }
            if (_yVar != null)
            {
                res = NetCDF4.nc_inq_dimlen(ncid, _yVar.Dimensions[0].DimId, out dimLen);
                if (res != 0) { goto ERROR; }

                Y = new double[dimLen];
                GetVarData(ncid, _yVar.VarId, dimLen, _yVar.NCType, ref Y);
                if (Y[0] > Y[1])
                {
                    Array.Reverse(Y);
                    this.IsYReverse = true;
                }
                YDelt = Y[1] - Y[0];
            }
            if (_levelVar != null)
            {
                res = NetCDF4.nc_inq_dimlen(ncid, _levelVar.Dimensions[0].DimId, out dimLen);
                if (res != 0) { goto ERROR; }

                levels = new double[dimLen];
                GetVarData(ncid, _levelVar.VarId, dimLen, _levelVar.NCType, ref levels);
            }
            if (_timeVar != null)
            {
                res = NetCDF4.nc_inq_dimlen(ncid, _timeVar.Dimensions[0].DimId, out dimLen);
                if (res != 0) { goto ERROR; }

                //Get start time
                int unitsId = 0;
                string unitsStr;
                for (i = 0; i < _timeVar.AttNumber; i++)
                {
                    if (_timeVar.Attributes[i].attName == "units")
                    {
                        unitsId = i;
                        break;
                    }
                }
                
                unitsStr = (string)_timeVar.Attributes[unitsId].attValue;
                if (unitsStr.Contains("as"))
                {
                    //Get data time
                    double[] DTimes = new double[dimLen];
                    GetVarData(ncid, _timeVar.VarId, dimLen, _timeVar.NCType, ref DTimes);
                    for (i = 0; i < DTimes.Length; i++)
                    {
                        string md = ((int)DTimes[i]).ToString();
                        if (md.Length <= 3)
                            md = "0" + md;
                        times.Add(DateTime.ParseExact("2001" + md, "yyyyMMdd", null));
                    }
                }
                else
                {
                    sTime = new DateTime();
                    TimeUnit aTU = new TimeUnit();
                    GetPSDTimeInfo(unitsStr, ref sTime, ref aTU);

                    //Get data time
                    double[] DTimes = new double[dimLen];
                    GetVarData(ncid, _timeVar.VarId, dimLen, _timeVar.NCType, ref DTimes);
                    for (i = 0; i < dimLen; i++)
                    {
                        switch (aTU)
                        {
                            case TimeUnit.Year:
                                times.Add(sTime.AddYears((int)DTimes[i]));
                                break;
                            case TimeUnit.Month:
                                times.Add(sTime.AddMonths((int)DTimes[i]));
                                break;
                            case TimeUnit.Day:
                                times.Add(sTime.AddDays(DTimes[i]));
                                break;
                            case TimeUnit.Hour:
                                //aDataInfo.times.Add(sTime.AddHours(DTimes[i] - 48));
                                times.Add(sTime.AddHours(DTimes[i]));
                                break;
                            case TimeUnit.Minute:
                                times.Add(sTime.AddMinutes(DTimes[i]));
                                break;
                            case TimeUnit.Second:
                                times.Add(sTime.AddSeconds(DTimes[i]));
                                break;
                        }
                    }
                }
            }

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
        }

        private void GetDimensionVariables()
        {           
            string sName;
            foreach (Variable aVarS in Variables)
            {
                if (aVarS.DimNumber != 1)
                    continue;

                if (aVarS.GetAttributeIndex("axis") > -1)
                {
                    sName = aVarS.GetAttributeString("axis").Trim();
                    switch (sName.ToLower())
                    {
                        case "x":
                            _xVar = aVarS;                            
                            break;
                        case "y":
                            _yVar = aVarS;
                            break;
                        case "z":
                            _levelVar = aVarS;
                            break;
                        case "t":
                            _timeVar = aVarS;
                            break;
                    }
                }
                else if (aVarS.GetAttributeIndex("standard_name") > -1)
                {
                    sName = aVarS.GetAttributeString("standard_name").Trim();
                    switch (sName.ToLower())
                    {
                        case "longitude":
                        case "projection_x_coordinate":
                        case "longitude_east":
                            _xVar = aVarS;
                            break;
                        case "latitude":
                        case "projection_y_coordinate":
                        case "latitude_north":
                            _yVar = aVarS;
                            break;
                        case "time":
                            _timeVar = aVarS;
                            break;
                        case "level":
                            _levelVar = aVarS;
                            break;
                    }                   
                }
                else if (aVarS.GetAttributeIndex("long_name") > -1)
                {
                    sName = aVarS.GetAttributeString("long_name").Trim();
                    switch (sName.ToLower())
                    {
                        case "longitude":
                        case "coordinate longitude":
                            _xVar = aVarS;
                            break;
                        case "latitude":
                        case "coordinate latitude":
                            _yVar = aVarS;
                            break;
                        case "time":
                        case "initial time":
                            _timeVar = aVarS;
                            break;
                        case "level":
                        case "pressure_level":
                            _levelVar = aVarS;
                            break;
                    }
                }
                else if (DimensionNames.Contains(aVarS.Name))
                {
                    switch (aVarS.Name.ToLower())
                    {
                        case "lon":
                        case "longitude":
                            _xVar = aVarS;
                            break;
                        case "lat":
                        case "latitude":
                            _yVar = aVarS;
                            break;
                        case "time":
                            _timeVar = aVarS;
                            break;
                        case "level":
                        case "lev":
                        case "height":
                            _levelVar = aVarS;
                            break;
                    }
                }
            }
            
            ////Check 2 dimension variables if X/Y coordinates can't be found in one dimension variables
            //if (_xVar == null)
            //{
            //    foreach (Variable aVarS in varList)
            //    {
            //        if (aVarS.DimNumber != 2)
            //            continue;

            //        switch (aVarS.Name.ToLower())
            //        {
            //            case "longitude":
            //            case "lon":
            //                _xVar = aVarS;
            //                break;
            //            case "latitude":
            //            case "lat":
            //                _yVar = aVarS;
            //                break;
            //        }
            //    }
            //}

            if (_levelVar == null)
            {
                foreach (Variable aVarS in Variables)
                {
                    if (DimensionNames.Contains(aVarS.Name))
                    {
                        switch (aVarS.Name.ToLower())
                        {                            
                            case "level":
                            case "lev":
                            case "height":
                                _levelVar = aVarS;
                                break;
                        }
                    }
                }
            }
        }

        private bool GetDimensionValues()
        {
            int res, dimLen, i;
            DateTime sTime = new DateTime();
            switch (Convention)
            {
                case Conventions.CF:
                    //GetDimValues_CF();
                    GetDimValues();
                    break;
                case Conventions.IOAPI:
                    //Get times
                    string sDateStr = GetGlobalAttStr("SDATE");
                    string sTimeStr = GetGlobalAttStr("STIME");
                    sTime = new DateTime(int.Parse(sDateStr.Substring(0, 4)), 1, 1).AddDays(int.Parse(sDateStr.Substring(4)));
                    if (sTimeStr.Length <= 2)
                        sTime = sTime.AddSeconds(int.Parse(sTimeStr));
                    else if (sTimeStr.Length <= 4)
                    {
                        sTime = sTime.AddMinutes(int.Parse(sTimeStr.Substring(0, sTimeStr.Length - 2)));
                        sTime = sTime.AddSeconds(int.Parse(sTimeStr.Substring(sTimeStr.Length - 2)));
                    }
                    else
                    {
                        sTime = sTime.AddHours(int.Parse(sTimeStr.Substring(0, sTimeStr.Length - 4)));
                        sTime = sTime.AddMinutes(int.Parse(sTimeStr.Substring(sTimeStr.Length - 4, 2)));
                        sTime = sTime.AddSeconds(int.Parse(sTimeStr.Substring(sTimeStr.Length - 2)));
                    }
                    int tNum = GetDimensionLength("TSTEP");
                    sTimeStr = GetGlobalAttStr("TSTEP");
                    times.Add(sTime);
                    for (i = 1; i < tNum; i++)
                    {
                        if (sTimeStr.Length <= 2)
                            sTime = sTime.AddSeconds(int.Parse(sTimeStr));
                        else if (sTimeStr.Length <= 4)
                        {
                            sTime = sTime.AddMinutes(int.Parse(sTimeStr.Substring(0, sTimeStr.Length - 2)));
                            sTime = sTime.AddSeconds(int.Parse(sTimeStr.Substring(sTimeStr.Length - 2)));
                        }
                        else
                        {
                            sTime = sTime.AddHours(int.Parse(sTimeStr.Substring(0, sTimeStr.Length - 4)));
                            sTime = sTime.AddMinutes(int.Parse(sTimeStr.Substring(sTimeStr.Length - 4, 2)));
                            sTime = sTime.AddSeconds(int.Parse(sTimeStr.Substring(sTimeStr.Length - 2)));
                        }
                        times.Add(sTime);
                    }

                    Dimension tdim = findDimension("TSTEP");
                    if (tdim != null)
                    {
                        tdim.DimType = DimensionType.T;
                        double[] tvalues = new double[times.Count];
                        for (i = 0; i < times.Count; i++)
                        {
                            tvalues[i] = DataConvert.ToDouble(times[i]);
                        }

                        tdim.SetValues(tvalues);
                        this.TimeDimension = tdim;
                    }

                    //Get levels
                    string levStr = GetGlobalAttStr("VGLVLS");
                    string[] levStrs = levStr.Split(',');
                    levels = new double[levStrs.Length - 1];
                    for (i = 0; i < levStrs.Length - 1; i++)
                    {
                        levels[i] = (double.Parse(levStrs[i].Trim()) + double.Parse(levStrs[i + 1].Trim())) / 2;
                    }
                    Dimension zdim = findDimension("LAY");
                    if (zdim != null)
                    {
                        zdim.DimType = DimensionType.Z;
                        zdim.SetValues(levels);
                        this.ZDimension = zdim;
                    }

                    //Get X array
                    int xNum = int.Parse(GetGlobalAttStr("NCOLS"));
                    X = new double[xNum];
                    double sx = double.Parse(GetGlobalAttStr("XORIG"));
                    XDelt = double.Parse(GetGlobalAttStr("XCELL"));
                    for (i = 0; i < xNum; i++)
                        X[i] = sx + XDelt * i;

                    Dimension xdim = findDimension("COL");
                    if (xdim != null)
                    {
                        xdim.DimType = DimensionType.X;
                        xdim.SetValues(X);
                        this.XDimension = xdim;
                    }

                    //Get Y array
                    int yNum = int.Parse(GetGlobalAttStr("NROWS"));
                    Y = new double[yNum];
                    double sy = double.Parse(GetGlobalAttStr("YORIG"));
                    YDelt = double.Parse(GetGlobalAttStr("YCELL"));
                    for (i = 0; i < yNum; i++)
                        Y[i] = sy + YDelt * i;

                    Dimension ydim = findDimension("ROW");
                    if (ydim != null)
                    {
                        ydim.DimType = DimensionType.Y;
                        ydim.SetValues(Y);
                        this.YDimension = ydim;
                    }
                    break;
                case Conventions.WRFOUT:
                    double orgLon = 0, orgLat = 0, orgX, orgY;
                    xdim = findDimension("west_east");
                    ydim = findDimension("south_north");
                    xNum = xdim.DimLength;
                    yNum = ydim.DimLength;
                    xdim.DimType = DimensionType.X;
                    ydim.DimType = DimensionType.Y;
                    
                    List<string> varNameList = new List<string>();
                    foreach (Variable aVarS in Variables)
                    {
                        varNameList.Add(aVarS.Name);
                    }
                    if (varNameList.Contains("XLAT"))
                        _yVar = Variables[varNameList.IndexOf("XLAT")];
                    else if (varNameList.Contains("XLAT_M"))
                        _yVar = Variables[varNameList.IndexOf("XLAT_M")];

                    if (varNameList.Contains("XLONG"))
                        _xVar = Variables[varNameList.IndexOf("XLONG")];
                    else if (varNameList.Contains("XLONG_M"))
                        _xVar = Variables[varNameList.IndexOf("XLONG_M")];

                    if (varNameList.Contains("ZNU"))
                        _levelVar = Variables[varNameList.IndexOf("ZNU")];

                    if (_yVar != null)
                    {
                        dimLen = GetVarLength(_yVar);
                        double[] xlat = new double[dimLen];
                        GetVarData(ncid, _yVar.VarId, dimLen, _yVar.NCType, ref xlat);
                        orgLat = xlat[0];
                    }
                    if (_xVar != null)
                    {
                        dimLen = GetVarLength(_xVar);
                        double[] xlon = new double[dimLen];
                        GetVarData(ncid, _xVar.VarId, dimLen, _xVar.NCType, ref xlon);
                        orgLon = xlon[0];
                    }

                    //Get levels
                    zdim = findDimension("bottom_up");
                    if (zdim == null)
                        zdim = findDimension("bottom_top");
                    if (zdim != null)
                    {
                        zdim.DimType = DimensionType.Z;
                        int lNum = zdim.DimLength;
                        if (_levelVar != null)
                        {
                            dimLen = GetVarLength(_levelVar);
                            levels = new double[dimLen];
                            GetVarData(ncid, _levelVar.VarId, dimLen, _levelVar.NCType, ref levels);
                            Array.Resize(ref levels, lNum);
                            zdim.SetValues(levels);
                            this.ZDimension = zdim;
                        }
                    }

                    if (varNameList.Contains("ZNW"))
                    {
                        Variable levelVar = Variables[varNameList.IndexOf("ZNW")];
                        zdim = findDimension("bottom_up_stag");
                        if (zdim == null)
                            zdim = findDimension("bottom_top_stag");
                        if (zdim != null)
                        {
                            zdim.DimType = DimensionType.Z;
                            int lNum = zdim.DimLength;
                            if (levelVar != null)
                            {
                                dimLen = GetVarLength(_levelVar);
                                levels = new double[dimLen];
                                GetVarData(ncid, levelVar.VarId, dimLen, levelVar.NCType, ref levels);
                                Array.Resize(ref levels, lNum);
                                zdim.SetValues(levels);
                                //this.ZDimension = zdim;
                            }
                        }
                    }

                    if (varNameList.Contains("ZS"))
                    {
                        Variable levelVar = Variables[varNameList.IndexOf("ZS")];
                        zdim = findDimension("soil_layers_stag");
                        if (zdim != null)
                        {
                            zdim.DimType = DimensionType.Z;
                            int lNum = zdim.DimLength;
                            if (levelVar != null)
                            {
                                dimLen = GetVarLength(_levelVar);
                                levels = new double[dimLen];
                                GetVarData(ncid, levelVar.VarId, dimLen, levelVar.NCType, ref levels);
                                Array.Resize(ref levels, lNum);
                                zdim.SetValues(levels);
                                //this.ZDimension = zdim;
                            }
                        }
                    }

                    //Get time dimension
                    foreach (Variable aVarS in Variables)
                    {
                        dimLen = GetVarLength(aVarS);
                        //Get times
                        if (aVarS.Name.ToLower() == "times" && aVarS.DimNumber == 2)
                        {
                            tdim = findDimension("Time");
                            tdim.DimType = DimensionType.T;
                            tNum = tdim.DimLength;
                            int strLen = GetDimensionLength("DateStrLen");
                            byte[] charData = new byte[tNum * strLen];
                            res = NetCDF4.nc_get_var_text(ncid, aVarS.VarId, charData);
                            string tStr;
                            double[] tvalues = new double[tNum];
                            for (i = 0; i < tNum; i++)
                            {
                                StringBuilder timeStr = new StringBuilder();
                                for (int j = 0; j < strLen; j++)
                                {
                                    timeStr.Append((char)charData[i * strLen + j]);
                                }
                                tStr = timeStr.ToString();
                                if (tStr.Contains("0000-00-00"))
                                    tStr = "0001-01-01_00:00:00";
                                times.Add(DateTime.ParseExact(tStr, "yyyy-MM-dd_HH:mm:ss", null));
                                tvalues[i] = DataConvert.ToDouble(times[i]);
                            }

                            tdim.SetValues(tvalues);
                            this.TimeDimension = tdim;
                        }

                        ////Get levels
                        //int lNum = GetDimensionLength("bottom_top");
                        //if (aVarS.Name == "ZNU")
                        //{
                        //    levels = new double[dimLen];
                        //    GetOneDimVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref levels);
                        //    Array.Resize(ref levels, lNum);
                        //}

                        ////Get X/Y array                                               
                        //if (aVarS.Name == "XLAT" || aVarS.Name == "XLAT_M")
                        //{
                        //    double[] xlat = new double[dimLen];
                        //    GetOneDimVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref xlat);
                        //    orgLat = xlat[0];
                        //}                        
                        //if (aVarS.Name == "XLONG" || aVarS.Name == "XLONG_M")
                        //{
                        //    double[] xlon = new double[dimLen];
                        //    GetOneDimVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref xlon);
                        //    orgLon = xlon[0];
                        //}                        
                    }
                    ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
                    double[][] points = new double[1][];
                    points[0] = new double[] { orgLon, orgLat };
                    Reproject.ReprojectPoints(points, fromProj, this.ProjectionInfo, 0, 1);
                    orgX = points[0][0];
                    orgY = points[0][1];
                    double dx = double.Parse(GetGlobalAttStr("DX"));
                    double dy = double.Parse(GetGlobalAttStr("DY"));
                    X = new double[xNum];
                    for (i = 0; i < xNum; i++)
                        X[i] = orgX + dx * i;
                    Y = new double[yNum];
                    for (i = 0; i < yNum; i++)
                        Y[i] = orgY + dy * i;
                    xdim.SetValues(X);
                    ydim.SetValues(Y);
                    this.XDimension = xdim;
                    this.YDimension = ydim;
                    Dimension xsdim = findDimension("west_east_stag");
                    Dimension ysdim = findDimension("south_north_stag");
                    if (xsdim != null && ysdim != null)
                    {
                        xsdim.DimType = DimensionType.X;
                        double[] nX = new double[xNum + 1];
                        double norgX = orgX - dx * 0.5;
                        for (i = 0; i <= xNum; i++)
                            nX[i] = norgX + dx * i;
                        xsdim.SetValues(nX);

                        ysdim.DimType = DimensionType.Y;
                        double[] nY = new double[yNum + 1];
                        double norgY = orgY - dx * 0.5;
                        for (i = 0; i <= yNum; i++)
                            nY[i] = norgY + dy * i;
                        ysdim.SetValues(nY);
                    }
                    break;
                default:
                    return false;
            }

            return true;

        //ERROR:
        //    MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
        //    return false;
        }

        //private bool GetDimensionValues_back()
        //{
        //    int res, dimLen, i;
        //    DateTime sTime = new DateTime();
        //    switch (Convention)
        //    {
        //        case Conventions.CF:
        //            foreach (Variable aVarS in Variables)
        //            {
        //                if (aVarS.DimNumber == 1)
        //                {
        //                    res = NetCDF4.nc_inq_dimlen(ncid, aVarS.Dimensions[0].DimId, out dimLen);
        //                    if (res != 0) { goto ERROR; }

        //                    if (aVarS.Name.Length == 1)
        //                    {
        //                        if (aVarS.Name.ToLower() == "x")
        //                        {
        //                            X = new double[dimLen];
        //                            GetVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref X);
        //                            if (X[0] > X[1])
        //                            {
        //                                Array.Reverse(X);
        //                                XReverse = true;
        //                            }
        //                            XDelt = X[1] - X[0];
        //                            if (X[dimLen - 1] + XDelt -
        //                                X[0] == 360)
        //                            {
        //                                isGlobal = true;
        //                            }
        //                        }
        //                        if (aVarS.Name.ToLower() == "y")
        //                        {
        //                            Y = new double[dimLen];
        //                            GetVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref Y);
        //                            if (Y[0] > Y[1])
        //                            {
        //                                Array.Reverse(Y);
        //                                YReverse = true;
        //                            }
        //                            YDelt = Y[1] - Y[0];
        //                        }
        //                    }
        //                    if (aVarS.Name.Length == 2)
        //                    {
        //                        if (aVarS.Name.Substring(0, 1).ToLower() == "x")
        //                        {
        //                            X = new double[dimLen];
        //                            GetVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref X);
        //                            if (X[0] > X[1])
        //                            {
        //                                Array.Reverse(X);
        //                                XReverse = true;
        //                            }
        //                            XDelt = X[1] - X[0];
        //                            if (X[dimLen - 1] + XDelt -
        //                                X[0] == 360)
        //                            {
        //                                isGlobal = true;
        //                            }
        //                        }
        //                        if (aVarS.Name.Substring(0, 1).ToLower() == "y")
        //                        {
        //                            Y = new double[dimLen];
        //                            GetVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref Y);
        //                            if (Y[0] > Y[1])
        //                            {
        //                                Array.Reverse(Y);
        //                                YReverse = true;
        //                            }
        //                            YDelt = Y[1] - Y[0];
        //                        }
        //                    }
        //                    if (aVarS.Name.Length == 3)
        //                    {
        //                        if (aVarS.Name.ToLower().Substring(0, 3) == "lon")
        //                        {
        //                            X = new double[dimLen];
        //                            GetVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref X);
        //                            if (X[0] > X[1])
        //                            {
        //                                Array.Reverse(X);
        //                                XReverse = true;
        //                            }
        //                            XDelt = X[1] - X[0];
        //                            if (X[dimLen - 1] + XDelt -
        //                                X[0] == 360)
        //                            {
        //                                isGlobal = true;
        //                            }
        //                        }
        //                        if (aVarS.Name.ToLower().Substring(0, 3) == "lat")
        //                        {
        //                            Y = new double[dimLen];
        //                            GetVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref Y);
        //                            if (Y[0] > Y[1])
        //                            {
        //                                Array.Reverse(Y);
        //                                YReverse = true;
        //                            }
        //                            YDelt = Y[1] - Y[0];
        //                        }
        //                    }
        //                    if (aVarS.Name.Length >= 5)
        //                    {
        //                        if (aVarS.Name.ToLower().Substring(0, 5) == "level" ||
        //                            aVarS.Name.ToLower() == "height")
        //                        {
        //                            levels = new double[dimLen];
        //                            GetVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref levels);
        //                        }
        //                    }
        //                    if (aVarS.Name.Length >= 4)
        //                    {
        //                        if (aVarS.Name.ToLower() == "time")
        //                        {
        //                            //Get start time
        //                            int unitsId = 0;
        //                            string unitsStr;
        //                            for (i = 0; i < aVarS.AttNumber; i++)
        //                            {
        //                                if (aVarS.Attributes[i].attName == "units")
        //                                {
        //                                    unitsId = i;
        //                                    break;
        //                                }
        //                            }

        //                            unitsStr = (string)aVarS.Attributes[unitsId].attValue;
        //                            if (unitsStr.Contains("as"))
        //                            {
        //                                //Get data time
        //                                double[] DTimes = new double[dimLen];
        //                                GetVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref DTimes);                                        
        //                                for (i = 0; i < DTimes.Length; i++)
        //                                {
        //                                    string md = ((int)DTimes[i]).ToString();
        //                                    if (md.Length <= 3)
        //                                        md = "0" + md;
        //                                    times.Add(DateTime.ParseExact("2001" + md, "yyyyMMdd", null));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                sTime = new DateTime();
        //                                TimeUnit aTU = new TimeUnit();
        //                                GetPSDTimeInfo(unitsStr, ref sTime, ref aTU);

        //                                //Get data time
        //                                double[] DTimes = new double[dimLen];
        //                                GetVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref DTimes);
        //                                for (i = 0; i < dimLen; i++)
        //                                {
        //                                    switch (aTU)
        //                                    {
        //                                        case TimeUnit.Year:
        //                                            times.Add(sTime.AddYears((int)DTimes[i]));
        //                                            break;
        //                                        case TimeUnit.Month:
        //                                            times.Add(sTime.AddMonths((int)DTimes[i]));
        //                                            break;
        //                                        case TimeUnit.Day:
        //                                            times.Add(sTime.AddDays(DTimes[i]));
        //                                            break;
        //                                        case TimeUnit.Hour:
        //                                            //aDataInfo.times.Add(sTime.AddHours(DTimes[i] - 48));
        //                                            times.Add(sTime.AddHours(DTimes[i]));
        //                                            break;
        //                                        case TimeUnit.Minute:
        //                                            times.Add(sTime.AddMinutes(DTimes[i]));
        //                                            break;
        //                                        case TimeUnit.Second:
        //                                            times.Add(sTime.AddSeconds(DTimes[i]));
        //                                            break;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            break;
        //        case Conventions.IOAPI:
        //            //Get times
        //            string sDateStr = GetGlobalAttStr("SDATE");
        //            string sTimeStr = GetGlobalAttStr("STIME");
        //            sTime = new DateTime(int.Parse(sDateStr.Substring(0, 4)), 1, 1).AddDays(int.Parse(sDateStr.Substring(4)));
        //            if (sTimeStr.Length <= 2)
        //                sTime = sTime.AddSeconds(int.Parse(sTimeStr));
        //            else if (sTimeStr.Length <= 4)
        //            {
        //                sTime = sTime.AddMinutes(int.Parse(sTimeStr.Substring(0, sTimeStr.Length - 2)));
        //                sTime = sTime.AddSeconds(int.Parse(sTimeStr.Substring(sTimeStr.Length - 2)));
        //            }
        //            else
        //            {
        //                sTime = sTime.AddHours(int.Parse(sTimeStr.Substring(0, sTimeStr.Length - 4)));
        //                sTime = sTime.AddMinutes(int.Parse(sTimeStr.Substring(sTimeStr.Length - 4, 2)));
        //                sTime = sTime.AddSeconds(int.Parse(sTimeStr.Substring(sTimeStr.Length - 2)));
        //            }
        //            int tNum = GetDimensionLength("TSTEP");                    
        //            sTimeStr = GetGlobalAttStr("TSTEP");
        //            times.Add(sTime);
        //            for (i = 1; i < tNum; i++)
        //            {
        //                if (sTimeStr.Length <= 2)
        //                    sTime = sTime.AddSeconds(int.Parse(sTimeStr));
        //                else if (sTimeStr.Length <= 4)
        //                {
        //                    sTime = sTime.AddMinutes(int.Parse(sTimeStr.Substring(0, sTimeStr.Length - 2)));
        //                    sTime = sTime.AddSeconds(int.Parse(sTimeStr.Substring(sTimeStr.Length - 2)));
        //                }
        //                else
        //                {
        //                    sTime = sTime.AddHours(int.Parse(sTimeStr.Substring(0, sTimeStr.Length - 4)));
        //                    sTime = sTime.AddMinutes(int.Parse(sTimeStr.Substring(sTimeStr.Length - 4, 2)));
        //                    sTime = sTime.AddSeconds(int.Parse(sTimeStr.Substring(sTimeStr.Length - 2)));
        //                }
        //                times.Add(sTime);
        //            }

        //            //Get levels
        //            string levStr = GetGlobalAttStr("VGLVLS");
        //            string[] levStrs = levStr.Split(',');
        //            levels = new double[levStrs.Length];
        //            for (i = 0; i < levStrs.Length; i++)
        //            {
        //                levels[i] = double.Parse(levStrs[i].Trim());
        //            }

        //            //Get X array
        //            int xNum = int.Parse(GetGlobalAttStr("NCOLS"));
        //            X = new double[xNum];
        //            double sx = double.Parse(GetGlobalAttStr("XORIG"));
        //            XDelt = double.Parse(GetGlobalAttStr("XCELL"));
        //            for (i = 0; i < xNum; i++)
        //                X[i] = sx + XDelt * i;

        //            //Get Y array
        //            int yNum = int.Parse(GetGlobalAttStr("NROWS"));
        //            Y = new double[yNum];
        //            double sy = double.Parse(GetGlobalAttStr("YORIG"));
        //            YDelt = double.Parse(GetGlobalAttStr("YCELL"));
        //            for (i = 0; i < yNum; i++)
        //                Y[i] = sy + YDelt * i;
        //            break;
        //        case Conventions.WRFOUT:
        //            double orgLon = 0, orgLat = 0, orgX, orgY;
        //            xNum = GetDimensionLength("west_east");
        //            yNum = GetDimensionLength("south_north");
        //            Variable lonVar = null;
        //            Variable latVar = null;
        //            List<string> varNameList = new List<string>();
        //            foreach (Variable aVarS in Variables)
        //            {
        //                varNameList.Add(aVarS.Name);
        //            }
        //            if (varNameList.Contains("XLAT"))
        //                latVar = Variables[varNameList.IndexOf("XLAT")];
        //            else if (varNameList.Contains("XLAT_M"))
        //                latVar = Variables[varNameList.IndexOf("XLAT_M")];

        //            if (varNameList.Contains("XLONG"))
        //                lonVar = Variables[varNameList.IndexOf("XLONG")];
        //            else if (varNameList.Contains("XLONG_M"))
        //                lonVar = Variables[varNameList.IndexOf("XLONG_M")];

        //            if (latVar != null)
        //            {
        //                dimLen = GetVarLength(latVar);
        //                double[] xlat = new double[dimLen];
        //                GetVarData(ncid, latVar.VarId, dimLen, latVar.NCType, ref xlat);
        //                orgLat = xlat[0];
        //            }
        //            if (lonVar != null)
        //            {
        //                dimLen = GetVarLength(lonVar);
        //                double[] xlon = new double[dimLen];
        //                GetVarData(ncid, lonVar.VarId, dimLen, lonVar.NCType, ref xlon);
        //                orgLon = xlon[0];
        //            }

        //            foreach (Variable aVarS in Variables)
        //            {
        //                dimLen = GetVarLength(aVarS);
        //                //Get times
        //                if (aVarS.Name.ToLower() == "times" && aVarS.DimNumber == 2)
        //                {                                                        
        //                    tNum = GetDimensionLength("Time");
        //                    int strLen = GetDimensionLength("DateStrLen");
        //                    byte[] charData = new byte[tNum * strLen];
        //                    res = NetCDF4.nc_get_var_text(ncid, aVarS.VarId, charData);
        //                    string tStr;
        //                    for (i = 0; i < tNum; i++)
        //                    {
        //                        StringBuilder timeStr = new StringBuilder();
        //                        for (int j = 0; j < strLen; j++)
        //                        {
        //                            timeStr.Append((char)charData[i * strLen + j]);
        //                        }
        //                        tStr = timeStr.ToString();
        //                        if (tStr.Contains("0000-00-00"))
        //                            tStr = "0001-01-01_00:00:00";
        //                        times.Add(DateTime.ParseExact(tStr, "yyyy-MM-dd_HH:mm:ss", null));
        //                    }                            
        //                }

        //                //Get levels
        //                int lNum = GetDimensionLength("bottom_top");
        //                if (aVarS.Name == "ZNU")
        //                {                            
        //                    levels = new double[dimLen];                           
        //                    GetVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref levels);                            
        //                    Array.Resize(ref levels, lNum);
        //                }

        //                ////Get X/Y array                                               
        //                //if (aVarS.Name == "XLAT" || aVarS.Name == "XLAT_M")
        //                //{
        //                //    double[] xlat = new double[dimLen];
        //                //    GetOneDimVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref xlat);
        //                //    orgLat = xlat[0];
        //                //}                        
        //                //if (aVarS.Name == "XLONG" || aVarS.Name == "XLONG_M")
        //                //{
        //                //    double[] xlon = new double[dimLen];
        //                //    GetOneDimVarData(ncid, aVarS.VarId, dimLen, aVarS.NCType, ref xlon);
        //                //    orgLon = xlon[0];
        //                //}                        
        //            }
        //            ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
        //            double[][] points = new double[1][];
        //            points[0] = new double[] { orgLon, orgLat };
        //            Reproject.ReprojectPoints(points, fromProj, ProjInfo, 0, 1);
        //            orgX = points[0][0];
        //            orgY = points[0][1];
        //            double dx = double.Parse(GetGlobalAttStr("DX"));
        //            double dy = double.Parse(GetGlobalAttStr("DY"));
        //            X = new double[xNum];
        //            for (i = 0; i < xNum; i++)
        //                X[i] = orgX + dx * i;
        //            Y = new double[yNum];
        //            for (i = 0; i < yNum; i++)
        //                Y[i] = orgY + dy * i;
        //            break;
        //        default:                    
        //            return false;
        //    }

        //    return true;

        //ERROR:
        //    MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
        //    return false;
        //}

        private void GetVariableLevels()
        {
            switch (Convention)
            {
                case Conventions.CF:                
                    //if (_levelVar != null)
                    //{
                    //    foreach (Variable aVarS in Variables)
                    //    {
                    //        if (aVarS.DimNumber >= 3 && aVarS.HasDimension(_levelVar.Dimensions[0].DimId))
                    //        {
                    //            aVarS.Levels = new List<double>(levels);
                    //        }
                    //    }
                    //}

                    foreach (Variable aVar in Variables)
                    {
                        if (aVar.ZDimension != null)
                        {
                            aVar.Levels = aVar.ZDimension.DimValue;
                        }
                    }
                    break;
                case Conventions.IOAPI:
                    if (levels.Length > 1)
                    {
                        int dIdx = -1;
                        if (DimensionNames.Contains("LAY"))
                            dIdx = _dimList[DimensionNames.IndexOf("LAY")].DimId;
                        else if (DimensionNames.Contains ("Lay"))
                            dIdx = _dimList[DimensionNames.IndexOf("Lay")].DimId;

                        if (dIdx > -1)
                        {
                            foreach (Variable aVarS in Variables)
                            {
                                if (aVarS.DimNumber >= 3 && aVarS.HasDimension(dIdx))
                                {
                                    aVarS.Levels = new List<double>(levels);
                                }
                            }
                        }
                    }
                    break;
                case Conventions.WRFOUT:
                    if (_levelVar != null)
                    {
                        foreach (Variable aVarS in Variables)
                        {
                            if (_levelVar.DimNumber == 1)
                            {
                                if (aVarS.DimNumber >= 3 && aVarS.HasDimension(_levelVar.Dimensions[0].DimId))
                                {
                                    aVarS.Levels = new List<double>(levels);
                                }
                            }
                            if (_levelVar.DimNumber == 2)
                            {
                                if (aVarS.DimNumber >= 3 && aVarS.HasDimension(_levelVar.Dimensions[1].DimId))
                                {
                                    aVarS.Levels = new List<double>(levels);
                                }
                            }
                        }
                    }
                    break;
            }
        }

        private string GetGlobalAttStr(string attName)
        {
            string attStr = string.Empty;
            foreach (AttStruct aAttS in gAttList)
            {
                if (aAttS.attName == attName)
                {
                    attStr = aAttS.ToString();
                    break;
                }
            }

            return attStr;
        }

        private int GetDimensionLength(string dimName)
        {
            foreach (Dimension aDimS in _dimList)
            {
                if (aDimS.DimName == dimName)
                    return aDimS.DimLength;
            }

            return -1;
        }

        /// <summary>
        /// Generate data info text
        /// </summary>        
        /// <returns>data info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            int i, j;
            AttStruct aAttS = new AttStruct();
            dataInfo = "File Name: " + this.FileName;
            dataInfo += Environment.NewLine + "Dimensions: " + ndims;
            for (i = 0; i < _dimList.Count; i++)
            {
                dataInfo += Environment.NewLine + "\t" + _dimList[i].DimName + " = " +
                    _dimList[i].DimLength.ToString() + ";";
            }

            dataInfo += Environment.NewLine + "Global Attributes: " + natts;
            for (i = 0; i < gAttList.Count; i++)
            {
                aAttS = gAttList[i];
                dataInfo += Environment.NewLine + "\t: " + GetAttStr(aAttS);
            }

            dataInfo += Environment.NewLine + "Variations: " + nvars;
            for (i = 0; i < Variables.Count; i++)
            {
                dataInfo += Environment.NewLine + "\t" + Variables[i].NCType.ToString() +
                    " " + Variables[i].Name + "(";
                for (j = 0; j < Variables[i].DimNumber; j++)
                {
                    dataInfo += _dimList[Variables[i].Dimensions[j].DimId].DimName + ",";
                }
                dataInfo = dataInfo.TrimEnd(new char[] { ',' });
                dataInfo += ");";
                for (j = 0; j < Variables[i].AttNumber; j++)
                {
                    aAttS = Variables[i].Attributes[j];
                    dataInfo += Environment.NewLine + "\t" + "\t" + Variables[i].Name +
                        ": " + GetAttStr(aAttS);
                }
            }

            dataInfo += Environment.NewLine + "Unlimited dimension: " + unlimdimid;

            return dataInfo;
        }

        /// <summary>
        /// Get attribute structure string
        /// </summary>
        /// <param name="aAttS"></param>
        /// <returns></returns>
        private string GetAttStr(AttStruct aAttS)
        {
            string outStr;
            outStr = aAttS.attName + " = ";
            int j;
            switch (aAttS.NCType)
            {
                case NetCDF4.NcType.NC_CHAR:
                    outStr += "\"" + aAttS.attValue.ToString() + "\"";
                    break;
                case NetCDF4.NcType.NC_INT:
                    for (j = 0; j < ((int[])aAttS.attValue).Length; j++)
                    {
                        outStr += ((int[])aAttS.attValue)[j].ToString() + ", ";
                    }
                    break;
                case NetCDF4.NcType.NC_SHORT:
                    for (j = 0; j < ((Int16[])aAttS.attValue).Length; j++)
                    {
                        outStr += ((Int16[])aAttS.attValue)[j].ToString() + ", ";
                    }
                    break;
                case NetCDF4.NcType.NC_BYTE:
                    for (j = 0; j < ((byte[])aAttS.attValue).Length; j++)
                    {
                        outStr += ((byte[])aAttS.attValue)[j].ToString() + ", ";
                    }
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    for (j = 0; j < ((Single[])aAttS.attValue).Length; j++)
                    {
                        outStr += ((Single[])aAttS.attValue)[j].ToString() + ", ";
                    }
                    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    for (j = 0; j < ((double[])aAttS.attValue).Length; j++)
                    {
                        outStr += ((double[])aAttS.attValue)[j].ToString() + ", ";
                    }
                    break;
            }
            outStr = outStr.TrimEnd();
            outStr = outStr.TrimEnd(new char[] { ',' });
            outStr += ";";

            return outStr;
        }      

        /// <summary>
        /// Get NetCDF grid data - Lon/Lat
        /// </summary>
        /// <param name="timeIdx">time index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>   
        /// <returns>grid data</returns>
        public GridData GetGridData_LonLat(int timeIdx, int varIdx, int levelIdx)
        {            
            int res, i, j;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < nvars; i++)
            {
                //if (Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = Variables[dVarIdx];

            int xNum, yNum;
            xNum = aVarS.XDimension.DimLength;
            yNum = aVarS.YDimension.DimLength;
            double[,] gridData = new double[yNum, xNum];

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.AttNumber; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }

                //MODIS NetCDF data
                if (aVarS.Attributes[i].attName == "_FillValue")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Adjust undefine data
            MissingValue = MissingValue * scale_factor + add_offset;

            //Get grid data
            int varid;           
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            if (aVarS.DimNumber == 4)
            {
                start[0] = timeIdx;
                start[1] = levelIdx;
                count[2] = yNum;
                count[3] = xNum;
            }
            else if (aVarS.DimNumber == 3)
            {
                start[0] = timeIdx;
                count[1] = yNum;
                count[2] = xNum;
            }
            else if (aVarS.DimNumber == 2)
            {
                count[0] = yNum;
                count[1] = xNum;
            }
            else
            {
                MessageBox.Show("The variation of " + aVarS.Name + " can't be used!" +
                    Environment.NewLine + "The dimension number is: " + aVarS.DimNumber, "Error");
            }
            double[] aData = new double[yNum * xNum];

            switch (aVarS.NCType)
            {
                case NetCDF4.NcType.NC_CHAR:
                    byte[] charData = new byte[yNum * xNum];
                    res = NetCDF4.nc_get_vara_text(ncid, varid, start, count, charData);
                    for (i = 0; i < yNum * xNum; i++)
                        aData[i] = charData[i];
                    break;
                default:
                    res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);
                    if (res != 0)
                        MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
                    break;
            }                        

            for (i = 0; i < yNum; i++)
            {
                for (j = 0; j < xNum; j++)
                {
                    gridData[i, j] = aData[i * xNum + j] * scale_factor + add_offset;
                    //gridData[i, j] = aData[i * xNum + j] + add_offset;
                }
            }                  

            GridData aGridData = new GridData();
            aGridData.Data = gridData;
            aGridData.X = aVarS.XDimension.GetValues();
            aGridData.Y = aVarS.YDimension.GetValues();
            aGridData.MissingValue = MissingValue;  

            if (this.IsYReverse)
            {
                double[,] nGridData = new double[aGridData.Data.GetLength(0), aGridData.Data.GetLength(1)];
                for (i = 0; i < aGridData.Data.GetLength(0); i++)
                {
                    for (j = 0; j < aGridData.Data.GetLength(1); j++)
                    {
                        nGridData[i, j] = aGridData.Data[aGridData.Data.GetLength(0) - i - 1, j];
                    }
                }
                aGridData.Data = nGridData;
            }

            if (this.Convention == Conventions.WRFOUT)
            {
                if (aVarS.Name == "U")
                    aGridData.XStag = true;

                if (aVarS.Name == "V")
                    aGridData.YStag = true;
            }

            return aGridData;   
        }

        /// <summary>
        /// Get NetCDF grid data - Lon/Lat
        /// </summary>
        /// <param name="timeIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>        
        /// <returns></returns>
        public double[,] GetNetCDFGridData(int timeIdx, int varIdx, int levelIdx)
        {
            int xNum, yNum;
            xNum = X.Length;
            yNum = Y.Length;
            double[,] gridData = new double[yNum, xNum];

            int res, i, j;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < nvars; i++)
            {
                //if (Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = Variables[dVarIdx];

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }

                //MODIS NetCDF data
                if (aVarS.Attributes[i].attName == "_FillValue")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Adjust undefine data
            MissingValue = MissingValue * scale_factor + add_offset;

            //Get grid data
            int varid;
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            if (aVarS.DimNumber == 4)
            {
                start[0] = timeIdx;
                start[1] = levelIdx;
                count[2] = yNum;
                count[3] = xNum;
            }
            else if (aVarS.DimNumber == 3)
            {
                start[0] = timeIdx;
                count[1] = yNum;
                count[2] = xNum;
            }
            else if (aVarS.DimNumber == 2)
            {
                count[0] = yNum;
                count[1] = xNum;
            }
            else
            {
                MessageBox.Show("The variation of " + aVarS.Name + " can't be used!" +
                    Environment.NewLine + "The dimension number is: " + aVarS.DimNumber, "Error");
            }
            double[] aData = new double[yNum * xNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < yNum; i++)
            {
                for (j = 0; j < xNum; j++)
                {
                    gridData[i, j] = aData[i * xNum + j] * scale_factor + add_offset;
                    //gridData[i, j] = aData[i * xNum + j] + add_offset;
                }
            }
            
            if (this.IsGlobal)
            {
                double[,] newGridData;
                newGridData = new double[yNum, xNum + 1];
                double[] newX = new double[xNum + 1];
                for (i = 0; i < xNum; i++)
                    newX[i] = X[i];

                newX[xNum] = newX[xNum - 1] + XDelt;
                for (i = 0; i < yNum; i++)
                {
                    for (j = 0; j < xNum; j++)
                    {
                        newGridData[i, j] = gridData[i, j];
                    }
                    newGridData[i, xNum] = newGridData[i, 0];
                }

                gridData= newGridData;                
            }            

            if (this.IsYReverse)
            {
                double[,] nGridData = new double[gridData.GetLength(0), gridData.GetLength(1)];
                for (i = 0; i < gridData.GetLength(0); i++)
                {
                    for (j = 0; j < gridData.GetLength(1); j++)
                    {
                        nGridData[i, j] = gridData[gridData.GetLength(0) - i - 1, j];
                    }
                }
                gridData = nGridData;
            }

            return gridData;
        }

        /// <summary>
        /// Get NetCDF grid data - Time/Lon
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_TimeLon(int latIdx, int varIdx, int levelIdx)
        {
            int xNum, yNum;
            xNum = X.Length;
            yNum = times.Count;
            double[,] gridData = new double[yNum, xNum];

            int res, i, j;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < nvars; i++)
            {
                //if (Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = Variables[dVarIdx];

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int varid;            
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            if (aVarS.DimNumber == 4)
            {
                start[2] = latIdx;
                start[1] = levelIdx;
                count[0] = yNum;
                count[3] = xNum;
            }
            else if (aVarS.DimNumber == 3)
            {
                start[1] = latIdx;
                count[0] = yNum;
                count[2] = xNum;
            }
            else
            {
                MessageBox.Show("The variation of " + aVarS.Name + " can't be used!" +
                    Environment.NewLine + "The dimension number is: " + aVarS.DimNumber, "Error");
            }

            double[] aData = new double[yNum * xNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < yNum; i++)
            {
                for (j = 0; j < xNum; j++)
                {
                    gridData[i, j] = aData[i * xNum + j] * scale_factor + add_offset;
                }
            }

            GridData aGridData = new GridData();
            aGridData.Data = gridData;
            aGridData.MissingValue = MissingValue;
            aGridData.X = X;
            aGridData.Y = new double[times.Count];
            for (i = 0; i < times.Count; i++)
                aGridData.Y[i] = DataConvert.ToDouble(times[i]);

            return aGridData;

        }

        /// <summary>
        /// Get NetCDF grid data - Time/Lon
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>
        /// <param name="aDataInfo"></param>
        /// <returns></returns>
        public double[,] GetNetCDFGridData_TimeLon(int latIdx, int varIdx, int levelIdx, NetCDFDataInfo aDataInfo)
        {
            int xNum, yNum;
            xNum = aDataInfo.X.Length;
            yNum = aDataInfo.times.Count;
            double[,] gridData = new double[yNum, xNum];

            int res, i, j;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < aDataInfo.nvars; i++)
            {
                //if (aDataInfo.Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = aDataInfo.Variables[dVarIdx];

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    aDataInfo.MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int ncid, varid;
            ncid = aDataInfo.ncid;
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            if (aVarS.DimNumber == 4)
            {
                start[2] = latIdx;
                start[1] = levelIdx;
                count[0] = yNum;
                count[3] = xNum;
            }
            else if (aVarS.DimNumber == 3)
            {
                start[1] = latIdx;
                count[0] = yNum;
                count[2] = xNum;
            }
            else
            {
                MessageBox.Show("The variation of " + aVarS.Name + " can't be used!" +
                    Environment.NewLine + "The dimension number is: " + aVarS.DimNumber, "Error");
            }

            double[] aData = new double[yNum * xNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < yNum; i++)
            {
                for (j = 0; j < xNum; j++)
                {
                    gridData[i, j] = aData[i * xNum + j] * scale_factor + add_offset;
                }
            }

            return gridData;

        }

        /// <summary>
        /// Get NetCDF grid data - Time/Lat
        /// </summary>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_TimeLat(int lonIdx, int varIdx, int levelIdx)
        {
            int xNum, yNum;
            xNum = Y.Length;
            yNum = times.Count;
            double[,] gridData = new double[yNum, xNum];

            int res, i, j;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < nvars; i++)
            {
                //if (Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = Variables[dVarIdx];

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int varid;            
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            if (aVarS.DimNumber == 4)
            {
                start[3] = lonIdx;
                start[1] = levelIdx;
                count[0] = yNum;
                count[2] = xNum;
            }
            else if (aVarS.DimNumber == 3)
            {
                start[2] = lonIdx;
                count[0] = yNum;
                count[1] = xNum;
            }
            else
            {
                MessageBox.Show("The variation of " + aVarS.Name + " can't be used!" +
                    Environment.NewLine + "The dimension number is: " + aVarS.DimNumber, "Error");
            }

            double[] aData = new double[yNum * xNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            if (this.IsYReverse)
            {
                for (i = 0; i < yNum; i++)
                {
                    for (j = 0; j < xNum; j++)
                    {
                        gridData[i, xNum - j - 1] = aData[i * xNum + j] * scale_factor + add_offset;
                    }
                }
            }
            else
            {
                for (i = 0; i < yNum; i++)
                {
                    for (j = 0; j < xNum; j++)
                    {
                        gridData[i, j] = aData[i * xNum + j] * scale_factor + add_offset;
                    }
                }
            }

            GridData aGridData = new GridData();
            aGridData.Data = gridData;
            aGridData.MissingValue = MissingValue;
            aGridData.X = Y;
            aGridData.Y = new double[times.Count];
            for (i = 0; i < times.Count; i++)
                aGridData.Y[i] = DataConvert.ToDouble(times[i]);

            return aGridData;
        }

        /// <summary>
        /// Get NetCDF grid data - Time/Lat
        /// </summary>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>
        /// <param name="aDataInfo"></param>
        /// <returns></returns>
        public double[,] GetNetCDFGridData_TimeLat(int lonIdx, int varIdx, int levelIdx, NetCDFDataInfo aDataInfo)
        {
            int xNum, yNum;
            xNum = aDataInfo.Y.Length;
            yNum = aDataInfo.times.Count;
            double[,] gridData = new double[yNum, xNum];

            int res, i, j;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < aDataInfo.nvars; i++)
            {
                //if (aDataInfo.Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = aDataInfo.Variables[dVarIdx];

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    aDataInfo.MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int ncid, varid;
            ncid = aDataInfo.ncid;
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            if (aVarS.DimNumber == 4)
            {
                start[3] = lonIdx;
                start[1] = levelIdx;
                count[0] = yNum;
                count[2] = xNum;
            }
            else if (aVarS.DimNumber == 3)
            {
                start[2] = lonIdx;
                count[0] = yNum;
                count[1] = xNum;
            }
            else
            {
                MessageBox.Show("The variation of " + aVarS.Name + " can't be used!" +
                    Environment.NewLine + "The dimension number is: " + aVarS.DimNumber, "Error");
            }

            double[] aData = new double[yNum * xNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < yNum; i++)
            {
                for (j = 0; j < xNum; j++)
                {
                    gridData[i, j] = aData[i * xNum + j] * scale_factor + add_offset;
                }
            }

            return gridData;

        }

        /// <summary>
        /// Get NetCDF grid data - Level/Lon
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="timeIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_LevelLon(int latIdx, int varIdx, int timeIdx)
        {
            int xNum, yNum;
            xNum = X.Length;
            yNum = levels.Length;
            double[,] gridData = new double[yNum, xNum];

            int res, i, j;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < nvars; i++)
            {
                //if (Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = Variables[dVarIdx];

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int varid;            
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            start[2] = latIdx;
            start[0] = timeIdx;
            count[1] = yNum;
            count[3] = xNum;
            double[] aData = new double[yNum * xNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < yNum; i++)
            {
                for (j = 0; j < xNum; j++)
                {
                    gridData[i, j] = aData[i * xNum + j] * scale_factor + add_offset;
                }
            }

            GridData aGridData = new GridData();
            aGridData.Data = gridData;
            aGridData.MissingValue = MissingValue;
            aGridData.X = X;
            aGridData.Y = levels;            

            return aGridData;

        }

        /// <summary>
        /// Get NetCDF grid data - Level/Lon
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="timeIdx"></param>
        /// <param name="aDataInfo"></param>
        /// <returns></returns>
        public double[,] GetNetCDFGridData_LevelLon(int latIdx, int varIdx, int timeIdx, NetCDFDataInfo aDataInfo)
        {
            int xNum, yNum;
            xNum = aDataInfo.X.Length;
            yNum = aDataInfo.levels.Length;
            double[,] gridData = new double[yNum, xNum];

            int res, i, j;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < aDataInfo.nvars; i++)
            {
                //if (aDataInfo.Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = aDataInfo.Variables[dVarIdx];

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    aDataInfo.MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int ncid, varid;
            ncid = aDataInfo.ncid;
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            start[2] = latIdx;
            start[0] = timeIdx;
            count[1] = yNum;
            count[3] = xNum;
            double[] aData = new double[yNum * xNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < yNum; i++)
            {
                for (j = 0; j < xNum; j++)
                {
                    gridData[i, j] = aData[i * xNum + j] * scale_factor + add_offset;
                }
            }

            return gridData;

        }

        /// <summary>
        /// Get NetCDF grid data - Level/Lat
        /// </summary>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="timeIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_LevelLat(int lonIdx, int varIdx, int timeIdx)
        {
            int xNum, yNum;
            xNum = Y.Length;
            yNum = levels.Length;
            double[,] gridData = new double[yNum, xNum];

            int res, i, j;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < nvars; i++)
            {
                //if (Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = Variables[dVarIdx];

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int varid;            
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            start[3] = lonIdx;
            start[0] = timeIdx;
            count[1] = yNum;
            count[2] = xNum;
            double[] aData = new double[yNum * xNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            if (this.IsYReverse)
            {
                for (i = 0; i < yNum; i++)
                {
                    for (j = 0; j < xNum; j++)
                    {
                        gridData[i, xNum - j - 1] = aData[i * xNum + j] * scale_factor + add_offset;
                    }
                }
            }
            else
            {
                for (i = 0; i < yNum; i++)
                {
                    for (j = 0; j < xNum; j++)
                    {
                        gridData[i, j] = aData[i * xNum + j] * scale_factor + add_offset;
                    }
                }
            }

            GridData aGridData = new GridData();
            aGridData.Data = gridData;
            aGridData.MissingValue = MissingValue;
            aGridData.X = Y;
            aGridData.Y = levels;

            return aGridData;

        }

        /// <summary>
        /// Get NetCDF grid data - Level/Lat
        /// </summary>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="timeIdx"></param>
        /// <param name="aDataInfo"></param>
        /// <returns></returns>
        public double[,] GetNetCDFGridData_LevelLat(int lonIdx, int varIdx, int timeIdx, NetCDFDataInfo aDataInfo)
        {
            int xNum, yNum;
            xNum = aDataInfo.Y.Length;
            yNum = aDataInfo.levels.Length;
            double[,] gridData = new double[yNum, xNum];

            int res, i, j;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < aDataInfo.nvars; i++)
            {
                //if (aDataInfo.Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = aDataInfo.Variables[dVarIdx];

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    aDataInfo.MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int ncid, varid;
            ncid = aDataInfo.ncid;
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            start[3] = lonIdx;
            start[0] = timeIdx;
            count[1] = yNum;
            count[2] = xNum;
            double[] aData = new double[yNum * xNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < yNum; i++)
            {
                for (j = 0; j < xNum; j++)
                {
                    gridData[i, j] = aData[i * xNum + j] * scale_factor + add_offset;
                }
            }

            return gridData;

        }

        /// <summary>
        /// Get NetCDF grid data - Level/Time
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="lonIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_LevelTime(int latIdx, int varIdx, int lonIdx)
        {
            int xNum, yNum;
            xNum = times.Count;
            yNum = levels.Length;
            double[,] gridData = new double[yNum, xNum];

            int res, i, j;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < nvars; i++)
            {
                //if (Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = Variables[dVarIdx];

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int varid;            
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            start[2] = latIdx;
            start[3] = lonIdx;
            count[1] = yNum;
            count[0] = xNum;
            double[] aData = new double[yNum * xNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < yNum; i++)
            {
                for (j = 0; j < xNum; j++)
                {
                    gridData[i, j] = aData[i * xNum + j] * scale_factor + add_offset;
                }
            }

            GridData aGridData = new GridData();
            aGridData.Data = gridData;
            aGridData.MissingValue = MissingValue;
            aGridData.X = new double[times.Count];
            for (i = 0; i < times.Count; i++)
                aGridData.X[i] = DataConvert.ToDouble(times[i]);
            aGridData.Y = levels;

            return aGridData;

        }

        /// <summary>
        /// Get NetCDF grid data - Level/Time
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="lonIdx"></param>
        /// <param name="aDataInfo"></param>
        /// <returns></returns>
        public double[,] GetNetCDFGridData_LevelTime(int latIdx, int varIdx, int lonIdx, NetCDFDataInfo aDataInfo)
        {
            int xNum, yNum;
            xNum = aDataInfo.times.Count;
            yNum = aDataInfo.levels.Length;
            double[,] gridData = new double[yNum, xNum];

            int res, i, j;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < aDataInfo.nvars; i++)
            {
                //if (aDataInfo.Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = aDataInfo.Variables[dVarIdx];

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    aDataInfo.MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int ncid, varid;
            ncid = aDataInfo.ncid;
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            start[2] = latIdx;
            start[3] = lonIdx;
            count[1] = yNum;
            count[0] = xNum;
            double[] aData = new double[yNum * xNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < yNum; i++)
            {
                for (j = 0; j < xNum; j++)
                {
                    gridData[i, j] = aData[i * xNum + j] * scale_factor + add_offset;
                }
            }

            return gridData;

        }

        /// <summary>
        /// Get NetCDF grid data - Time
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>       
        /// <returns></returns>
        public GridData GetGridData_Time(int lonIdx, int latIdx, int varIdx, int levelIdx)
        {
            int dNum;
            dNum = times.Count;            

            int res, i;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < nvars; i++)
            {
                //if (Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = Variables[dVarIdx];
            double aValue;

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int varid;            
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            if (aVarS.DimNumber == 4)
            {
                start[2] = latIdx;
                start[1] = levelIdx;
                start[3] = lonIdx;
                count[0] = dNum;
            }
            else if (aVarS.DimNumber == 3)
            {
                start[1] = latIdx;
                start[2] = lonIdx;
                count[0] = dNum;
            }
            else
            {
                MessageBox.Show("The variation of " + aVarS.Name + " can't be used!" +
                    Environment.NewLine + "The dimension number is: " + aVarS.DimNumber, "Error");
            }

            double[] aData = new double[dNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = new double[times.Count];
            aGridData.Y = new double[1];
            aGridData.Data = new double[1, times.Count];
            aGridData.Y[0] = 0;
            for (i = 0; i < dNum; i++)
            {
                aValue = aData[i] * scale_factor + add_offset;
                aGridData.Data[0, i] = aValue;
                aGridData.X[i] = DataConvert.ToDouble(times[i]);
            }

            return aGridData;

        }

        /// <summary>
        /// Get NetCDF grid data - Time
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>
        /// <param name="aDataInfo"></param>
        /// <returns></returns>
        public List<PointD> GetNetCDFGridData_Time(int latIdx, int lonIdx, int varIdx, int levelIdx, NetCDFDataInfo aDataInfo)
        {
            int dNum;
            dNum = aDataInfo.times.Count;

            PointD aPoint = new PointD();
            List<PointD> pointList = new List<PointD>();

            int res, i;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < aDataInfo.nvars; i++)
            {
                //if (aDataInfo.Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = aDataInfo.Variables[dVarIdx];
            double aValue;

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    aDataInfo.MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int ncid, varid;
            ncid = aDataInfo.ncid;
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            if (aVarS.DimNumber == 4)
            {
                start[2] = latIdx;
                start[1] = levelIdx;
                start[3] = lonIdx;
                count[0] = dNum;
            }
            else if (aVarS.DimNumber == 3)
            {
                start[1] = latIdx;
                start[2] = lonIdx;
                count[0] = dNum;
            }
            else
            {
                MessageBox.Show("The variation of " + aVarS.Name + " can't be used!" +
                    Environment.NewLine + "The dimension number is: " + aVarS.DimNumber, "Error");
            }

            double[] aData = new double[dNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < dNum; i++)
            {
                aValue = aData[i] * scale_factor + add_offset;
                if (!(MIMath.DoubleEquals(aValue, aDataInfo.MissingValue)))
                {
                    aPoint.X = aDataInfo.times[i].ToBinary();
                    aPoint.Y = aValue;
                    pointList.Add(aPoint);
                }
            }

            return pointList;

        }

        /// <summary>
        /// Get NetCDF data - Level
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="timeIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_Level(int lonIdx, int latIdx, int varIdx, int timeIdx)
        {
            int dNum;
            dNum = levels.Length;
            
            int res, i;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < nvars; i++)
            {
                //if (Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = Variables[dVarIdx];
            double aValue;

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int varid;           
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            start[2] = latIdx;
            start[0] = timeIdx;
            start[3] = lonIdx;
            count[1] = dNum;
            double[] aData = new double[dNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);
           
            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = new double[levels.Length];
            aGridData.Y = new double[1];
            aGridData.Data = new double[1, levels.Length];
            aGridData.Y[0] = 0;
            for (i = 0; i < dNum; i++)
            {
                aValue = aData[i] * scale_factor + add_offset;
                aGridData.Data[0, i] = aValue;
                aGridData.X[i] = levels[i];
            }

            return aGridData;

        }

        /// <summary>
        /// Get NetCDF data - Level
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="timeIdx"></param>
        /// <param name="aDataInfo"></param>
        /// <returns></returns>
        public List<PointD> GetNetCDFGridData_Level(int latIdx, int lonIdx, int varIdx, int timeIdx, NetCDFDataInfo aDataInfo)
        {
            int dNum;
            dNum = aDataInfo.levels.Length;

            PointD aPoint = new PointD();
            List<PointD> pointList = new List<PointD>();

            int res, i;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < aDataInfo.nvars; i++)
            {
                //if (aDataInfo.Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = aDataInfo.Variables[dVarIdx];
            double aValue;

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    aDataInfo.MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int ncid, varid;
            ncid = aDataInfo.ncid;
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            start[2] = latIdx;
            start[0] = timeIdx;
            start[3] = lonIdx;
            count[1] = dNum;
            double[] aData = new double[dNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < dNum; i++)
            {
                aValue = aData[i] * scale_factor + add_offset;
                if (!(MIMath.DoubleEquals(aValue, aDataInfo.MissingValue)))
                {
                    aPoint.X = aValue;
                    aPoint.Y = aDataInfo.levels[i];
                    pointList.Add(aPoint);
                }
            }

            return pointList;

        }

        /// <summary>
        /// Get NetCDF data - Longitude
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="timeIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_Lon(int timeIdx, int latIdx, int varIdx, int levelIdx)
        {
            int dNum;
            dNum = X.Length;            

            int res, i;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < nvars; i++)
            {
                //if (Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = Variables[dVarIdx];
            double aValue;

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int varid;           
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            if (aVarS.DimNumber == 4)
            {
                start[2] = latIdx;
                start[1] = levelIdx;
                start[0] = timeIdx;
                count[3] = dNum;
            }
            else if (aVarS.DimNumber == 3)
            {
                start[1] = latIdx;
                start[0] = timeIdx;
                count[2] = dNum;
            }
            else
            {
                MessageBox.Show("The variation of " + aVarS.Name + " can't be used!" +
                    Environment.NewLine + "The dimension number is: " + aVarS.DimNumber, "Error");
            }

            double[] aData = new double[dNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = X;
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, X.Length];
            for (i = 0; i < dNum; i++)
            {
                aValue = aData[i] * scale_factor + add_offset;
                aGridData.Data[0, i] = aValue;                
            }

            return aGridData;

        }

        /// <summary>
        /// Get NetCDF data - Longitude
        /// </summary>
        /// <param name="latIdx"></param>
        /// <param name="timeIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>
        /// <param name="aDataInfo"></param>
        /// <returns></returns>
        public List<PointD> GetNetCDFGridData_Lon(int latIdx, int timeIdx, int varIdx, int levelIdx, NetCDFDataInfo aDataInfo)
        {
            int dNum;
            dNum = aDataInfo.X.Length;

            PointD aPoint = new PointD();
            List<PointD> pointList = new List<PointD>();

            int res, i;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < aDataInfo.nvars; i++)
            {
                //if (aDataInfo.Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = aDataInfo.Variables[dVarIdx];
            double aValue;

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    aDataInfo.MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int ncid, varid;
            ncid = aDataInfo.ncid;
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            if (aVarS.DimNumber == 4)
            {
                start[2] = latIdx;
                start[1] = levelIdx;
                start[0] = timeIdx;
                count[3] = dNum;
            }
            else if (aVarS.DimNumber == 3)
            {
                start[1] = latIdx;
                start[0] = timeIdx;
                count[2] = dNum;
            }
            else
            {
                MessageBox.Show("The variation of " + aVarS.Name + " can't be used!" +
                    Environment.NewLine + "The dimension number is: " + aVarS.DimNumber, "Error");
            }

            double[] aData = new double[dNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < dNum; i++)
            {
                aValue = aData[i] * scale_factor + add_offset;
                if (!(MIMath.DoubleEquals(aValue, aDataInfo.MissingValue)))
                {
                    aPoint.X = aDataInfo.X[i];
                    aPoint.Y = aValue;
                    pointList.Add(aPoint);
                }
            }

            return pointList;

        }

        /// <summary>
        /// Get NetCDF data - Latitude
        /// </summary>
        /// <param name="timeIdx"></param>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>        
        /// <returns></returns>
        public GridData GetGridData_Lat(int timeIdx, int lonIdx, int varIdx, int levelIdx)
        {
            int dNum;
            dNum = Y.Length;            

            int res, i;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < nvars; i++)
            {
                //if (Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = Variables[dVarIdx];
            double aValue;

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int varid;            
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            if (aVarS.DimNumber == 4)
            {
                start[0] = timeIdx;
                start[1] = levelIdx;
                start[3] = lonIdx;
                count[2] = dNum;
            }
            else if (aVarS.DimNumber == 3)
            {
                start[2] = lonIdx;
                start[0] = timeIdx;
                count[1] = dNum;
            }
            else
            {
                MessageBox.Show("The variation of " + aVarS.Name + " can't be used!" +
                    Environment.NewLine + "The dimension number is: " + aVarS.DimNumber, "Error");
            }

            double[] aData = new double[dNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            GridData aGridData = new GridData();
            aGridData.MissingValue = MissingValue;
            aGridData.X = Y;
            aGridData.Y = new double[1];
            aGridData.Y[0] = 0;
            aGridData.Data = new double[1, Y.Length];
            for (i = 0; i < dNum; i++)
            {
                aValue = aData[i] * scale_factor + add_offset;
                aGridData.Data[0, i] = aValue;
            }

            return aGridData;            
        }

        /// <summary>
        /// Get NetCDF data - Latitude
        /// </summary>
        /// <param name="timeIdx"></param>
        /// <param name="lonIdx"></param>
        /// <param name="varIdx"></param>
        /// <param name="levelIdx"></param>
        /// <param name="aDataInfo"></param>
        /// <returns></returns>
        public List<PointD> GetNetCDFGridData_Lat(int timeIdx, int lonIdx, int varIdx, int levelIdx, NetCDFDataInfo aDataInfo)
        {
            int dNum;
            dNum = aDataInfo.Y.Length;

            PointD aPoint = new PointD();
            List<PointD> pointList = new List<PointD>();

            int res, i;
            Variable aVarS = new Variable();
            int dVarIdx = varIdx;
            int dVarNum = -1;
            for (i = 0; i < aDataInfo.nvars; i++)
            {
                //if (aDataInfo.Variables[i].isDataVar)
                //    dVarNum += 1;
                if (dVarNum == varIdx)
                {
                    dVarIdx = i;
                    break;
                }
            }
            aVarS = aDataInfo.Variables[dVarIdx];
            double aValue;

            //Get pack info
            double add_offset, scale_factor;
            add_offset = 0;
            scale_factor = 1;
            for (i = 0; i < aVarS.Attributes.Count; i++)
            {
                if (aVarS.Attributes[i].attName == "add_offset")
                {
                    add_offset = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "scale_factor")
                {
                    scale_factor = GetValidVarAtt(aVarS.Attributes[i]);
                }

                if (aVarS.Attributes[i].attName == "missing_value")
                {
                    aDataInfo.MissingValue = GetValidVarAtt(aVarS.Attributes[i]);
                }
            }

            //Get grid data
            int ncid, varid;
            ncid = aDataInfo.ncid;
            varid = aVarS.VarId;
            int[] start = new int[aVarS.DimNumber];
            int[] count = new int[aVarS.DimNumber];
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                start[i] = 0;
                count[i] = 1;
            }
            if (aVarS.DimNumber == 4)
            {
                start[0] = timeIdx;
                start[1] = levelIdx;
                start[3] = lonIdx;
                count[2] = dNum;
            }
            else if (aVarS.DimNumber == 3)
            {
                start[2] = lonIdx;
                start[0] = timeIdx;
                count[1] = dNum;
            }
            else
            {
                MessageBox.Show("The variation of " + aVarS.Name + " can't be used!" +
                    Environment.NewLine + "The dimension number is: " + aVarS.DimNumber, "Error");
            }

            double[] aData = new double[dNum];

            res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, aData);

            for (i = 0; i < dNum; i++)
            {
                aValue = aData[i] * scale_factor + add_offset;
                if (!(MIMath.DoubleEquals(aValue, aDataInfo.MissingValue)))
                {
                    aPoint.X = aDataInfo.Y[i];
                    aPoint.Y = aValue;
                    pointList.Add(aPoint);
                }
            }

            return pointList;

        }

        #endregion

        #region Others

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>NetCDF data info</returns>
        public object Clone()
        {
            NetCDFDataInfo aDataInfo = new NetCDFDataInfo();
            aDataInfo._dimList.AddRange(_dimList);
            aDataInfo.FileName = FileName;
            aDataInfo.gAttList.AddRange(gAttList);
            aDataInfo.IsGlobal = IsGlobal;
            aDataInfo.isLatLon = isLatLon;
            aDataInfo.levels = levels;
            aDataInfo.natts = natts;
            aDataInfo.ncid = ncid;
            aDataInfo.ndims = ndims;
            aDataInfo.nvars = nvars;
            aDataInfo.times.AddRange(times);
            aDataInfo.MissingValue = this.MissingValue;
            aDataInfo.unlimdimid = unlimdimid;
            foreach (Variable aVarS in Variables)
            {
                Variable bVarS = (Variable)aVarS.Clone();
                aDataInfo.Variables.Add(bVarS);
            }
            //aDataInfo.Variables.AddRange(Variables);
            aDataInfo.X = X;
            aDataInfo.Y = Y;
            aDataInfo.XDelt = XDelt;
            aDataInfo.YDelt = YDelt;
            aDataInfo.IsXReverse = IsXReverse;
            aDataInfo.IsYReverse = IsYReverse;

            return aDataInfo;
        }       

        /// <summary>
        /// Add dimension
        /// </summary>
        /// <param name="aDimS">a dimension</param>
        public void AddDimension(Dimension aDimS)
        {
            _dimList.Add(aDimS);
            ndims += 1;
        }

        private bool GetVaraData(int ncid, int varid, int[] start, int[] count, NetCDF4.NcType ncType,
            ref double[] data)
        {
            int res, i;
            int dimLen = 0;
            for (i = 0; i < count.Length; i++)
                dimLen += count[i];

            data = new double[dimLen];
            switch (ncType)
            {
                case NetCDF4.NcType.NC_CHAR:
                    byte[] varbyte = new byte[dimLen];
                    res = NetCDF4.nc_get_vara_text(ncid, varid, start, count, varbyte);
                    for (i = 0; i < dimLen; i++)
                        data[i] = (double)varbyte[i];

                    break;
                case NetCDF4.NcType.NC_SHORT:
                    Int16[] varshort = new Int16[dimLen];
                    res = NetCDF4.nc_get_vara_short(ncid, varid, start, count, varshort);
                    if (res != 0) { goto ERROR; }
                    for (i = 0; i < dimLen; i++)
                    {
                        data[i] = (double)varshort[i];
                    }
                    break;
                case NetCDF4.NcType.NC_INT:
                    int[] varint = new int[dimLen];
                    res = NetCDF4.nc_get_vara_int(ncid, varid, start, count, varint);
                    if (res != 0) { goto ERROR; }
                    for (i = 0; i < dimLen; i++)
                    {
                        data[i] = (double)varint[i];
                    }
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    Single[] varfloat = new Single[dimLen];
                    res = NetCDF4.nc_get_vara_float(ncid, varid, start, count, varfloat);
                    if (res != 0) { goto ERROR; }
                    for (i = 0; i < dimLen; i++)
                    {
                        data[i] = (double)varfloat[i];
                    }
                    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    double[] vardouble = new double[dimLen];
                    res = NetCDF4.nc_get_vara_double(ncid, varid, start, count, vardouble);
                    if (res != 0) { goto ERROR; }
                    for (i = 0; i < dimLen; i++)
                    {
                        data[i] = (double)vardouble[i];
                    }
                    break;
            }

            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }             

        private bool GetVarData(int ncid, int varid, int dimLen, NetCDF4.NcType ncType,
            ref double[] data)
        {
            int res, i;
            switch (ncType)
            {
                case NetCDF4.NcType.NC_SHORT:
                    Int16[] varshort = new Int16[dimLen];
                    res = NetCDF4.nc_get_var_short(ncid, varid, varshort);
                    if (res != 0) { goto ERROR; }
                    for (i = 0; i < dimLen; i++)
                    {
                        data[i] = (double)varshort[i];
                    }
                    break;
                case NetCDF4.NcType.NC_INT:
                    int[] varint = new int[dimLen];
                    res = NetCDF4.nc_get_var_int(ncid, varid, varint);
                    if (res != 0) { goto ERROR; }
                    for (i = 0; i < dimLen; i++)
                    {
                        data[i] = (double)varint[i];
                    }
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    Single[] varfloat = new Single[dimLen];
                    res = NetCDF4.nc_get_var_float(ncid, varid, varfloat);
                    if (res != 0) { goto ERROR; }
                    for (i = 0; i < dimLen; i++)
                    {
                        data[i] = (double)varfloat[i];
                    }
                    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    double[] vardouble = new double[dimLen];
                    res = NetCDF4.nc_get_var_double(ncid, varid, vardouble);
                    if (res != 0) { goto ERROR; }
                    for (i = 0; i < dimLen; i++)
                    {
                        data[i] = (double)vardouble[i];
                    }
                    break;
            }

            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }             

        /// <summary>
        /// Get variable data
        /// </summary>
        /// <param name="aVarS">a variable</param>
        /// <param name="data">data</param>
        /// <returns>is correct</returns>
        public bool GetVarData(Variable aVarS, ref object[] data)
        {
            int res, i;
            int dataLen = 1;
            for (i = 0; i < aVarS.DimNumber; i++)
            {
                dataLen = dataLen * _dimList[aVarS.Dimensions[i].DimId].DimLength;
            }
            data = new object[dataLen];
            switch (aVarS.NCType)
            {
                case NetCDF4.NcType.NC_CHAR:
                    //string[] varchar = new string[dataLen];
                    //res = NetCDF4.nc_get_var_string(ncid, aVarS.VarId, varchar);
                    //Array.Copy(varchar, data, dataLen);

                    byte[] varchar = new byte[dataLen];
                    res = NetCDF4.nc_get_var_text(ncid, aVarS.VarId, varchar);
                    Array.Copy(varchar, data, dataLen);
                    
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_BYTE:
                    byte[] varbyte = new byte[dataLen];                    
                    res = NetCDF4.nc_get_var_ubyte(ncid, aVarS.VarId, varbyte);
                    Array.Copy(varbyte, data, dataLen);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_SHORT:
                    Int16[] varshort = new Int16[dataLen];                    
                    res = NetCDF4.nc_get_var_short(ncid, aVarS.VarId, varshort);
                    Array.Copy(varshort, data, dataLen);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_INT:
                    int[] varint = new int[dataLen];                    
                    res = NetCDF4.nc_get_var_int(ncid, aVarS.VarId, varint);
                    Array.Copy(varint, data, dataLen);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    Single[] varfloat = new Single[dataLen];                    
                    res = NetCDF4.nc_get_var_float(ncid, aVarS.VarId, varfloat);
                    Array.Copy(varfloat, data, dataLen);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    double[] vardouble = new double[dataLen];                    
                    res = NetCDF4.nc_get_var_double(ncid, aVarS.VarId, vardouble);
                    Array.Copy(vardouble, data, dataLen);
                    if (res != 0) { goto ERROR; }
                    break;
            }

            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }

        /// <summary>
        /// Get part of variable data
        /// </summary>
        /// <param name="aVarS">a variable</param>
        /// <param name="start">start array</param>
        /// <param name="count">count array</param>
        /// <param name="data">ref data</param>
        /// <returns>is correct</returns>
        public bool GetVaraData(Variable aVarS, int[] start, int[] count, ref object[] data)
        {
            int res, i;
            int dataLen = 1;
            for (i = 0; i < count.Length; i++)
            {
                dataLen = dataLen * count[i];
            }
            data = new object[dataLen];
            switch (aVarS.NCType)
            {
                case NetCDF4.NcType.NC_CHAR:
                    string[] varchar = new string[dataLen];
                    res = NetCDF4.nc_get_vara_string(ncid, aVarS.VarId, start, count, varchar);
                    Array.Copy(varchar, data, dataLen);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_BYTE:
                    byte[] varbyte = new byte[dataLen];
                    res = NetCDF4.nc_get_vara_ubyte(ncid, aVarS.VarId, start, count, varbyte);
                    Array.Copy(varbyte, data, dataLen);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_SHORT:
                    Int16[] varshort = new Int16[dataLen];
                    res = NetCDF4.nc_get_vara_short(ncid, aVarS.VarId, start, count, varshort);
                    Array.Copy(varshort, data, dataLen);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_INT:
                    int[] varint = new int[dataLen];
                    res = NetCDF4.nc_get_vara_int(ncid, aVarS.VarId, start, count, varint);
                    Array.Copy(varint, data, dataLen);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    Single[] varfloat = new Single[dataLen];
                    res = NetCDF4.nc_get_vara_float(ncid, aVarS.VarId, start, count, varfloat);
                    Array.Copy(varfloat, data, dataLen);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    double[] vardouble = new double[dataLen];
                    res = NetCDF4.nc_get_vara_double(ncid, aVarS.VarId, start, count, vardouble);
                    Array.Copy(vardouble, data, dataLen);
                    if (res != 0) { goto ERROR; }
                    break;
            }

            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }        

        /// <summary>
        /// Write NetCDF data file
        /// </summary>
        /// <param name="aFile">file path</param>        
        /// <param name="data">data array</param>
        /// <returns>is correct</returns>
        public bool WriteNetCDFData(string aFile, object[] data)
        {
            int res, i, j;
            //int ncid;

            //Create NetCDF file
            res = NetCDF4.nc_create(aFile, (int)NetCDF4.CreateMode.NC_CLOBBER, out ncid);
            if (res != 0) { goto ERROR; }

            //Write data info
            WriteNetCDFDataInfo(ncid);

            //Write variable data         
            int sDataIdx = 0;
            for (i = 0; i < nvars; i++)
            {
                Variable aVarS = Variables[i];
                int dataLen = 1;
                for (j = 0; j < aVarS.DimNumber; j++)
                {
                    dataLen = dataLen * _dimList[aVarS.Dimensions[j].DimId].DimLength;
                }
                object[] aData = new object[dataLen];
                Array.Copy(data, sDataIdx, aData, 0, dataLen);
                if (aVarS.Name == "time")
                {
                    double[] tData = new double[aData.Length];
                    for (j = 0; j < aData.Length; j++)
                        tData[j] = double.Parse(aData[j].ToString());
                    NetCDF4.nc_put_vara_double(ncid, aVarS.VarId, new int[1] { 0 }, new int[1] { aData.Length }, tData);
                }
                else
                {
                    if (aVarS.DimNumber > 1)
                    {
                        int[] start = new int[aVarS.DimNumber];
                        int[] count = new int[aVarS.DimNumber];
                        for (j = 0; j < aVarS.DimNumber; j++)
                        {
                            start[j] = 0;
                            count[j] = _dimList[aVarS.Dimensions[j].DimId].DimLength;
                        }
                        WriteVaraData(aVarS.VarId, aVarS.NCType, start, count, aData);
                    }
                    else
                        WriteVarData(aVarS.VarId, aVarS.NCType, aData);
                }
                sDataIdx += dataLen;
            }

            //Close data file
            res = NetCDF4.nc_close(ncid);
            if (res != 0) { goto ERROR; }            
            
            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }        

        /// <summary>
        /// Write attribute
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="aAttS"></param>
        /// <returns></returns>
        public bool WriteAtt(int ncid, int varid, AttStruct aAttS)
        {
            int res;
            string attName = aAttS.attName;
            int attLen = aAttS.attLen;
            switch (aAttS.NCType)
            {
                case NetCDF4.NcType.NC_CHAR:
                    res = NetCDF4.nc_put_att_text(ncid, varid, attName, attLen, (string)aAttS.attValue);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_BYTE:
                    res = NetCDF4.nc_put_att_byte(ncid, varid, attName, (int)NetCDF4.NcType.NC_BYTE,
                        attLen, (sbyte[])aAttS.attValue);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_INT:
                    res = NetCDF4.nc_put_att_int(ncid, varid, attName, (int)NetCDF4.NcType.NC_INT,
                        attLen, (int[])aAttS.attValue);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_SHORT:
                    res = NetCDF4.nc_put_att_short(ncid, varid, attName, (int)NetCDF4.NcType.NC_SHORT,
                        attLen, (Int16[])aAttS.attValue);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    res = NetCDF4.nc_put_att_float(ncid, varid, attName, (int)NetCDF4.NcType.NC_FLOAT,
                        attLen, (Single[])aAttS.attValue);
                    if (res != 0) { goto ERROR; }
                    break;
                //case NetCDF4.NcType.NC_DOUBLE:
                //    res = NetCDF4.nc_put_att_double(ncid, varid, attName, (int)NetCDF4.NcType.NC_DOUBLE,
                //        attLen, (double[])aAttS.attValue);
                //    if (res != 0) { goto ERROR; }
                //    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    double[] data;
                    if (aAttS.attValue is Array)                        
                        data = (double[])aAttS.attValue;
                    else
                        data = new double[] { (double)aAttS.attValue };

                    res = NetCDF4.nc_put_att_double(ncid, varid, attName, (int)NetCDF4.NcType.NC_DOUBLE,
                        attLen, data);
                    if (res != 0) { goto ERROR; }
                    break;
            }

            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }

        #endregion

        #region Write
        /// <summary>
        /// Add a dimension
        /// </summary>
        /// <param name="dimName">dimension name</param>
        /// <param name="dimLen">dimension length</param>
        /// <returns>dimension</returns>
        public Dimension AddDimension(string dimName, int dimLen)
        {
            Dimension aDim = new Dimension();
            aDim.DimName = dimName;
            aDim.DimLength = dimLen;
            aDim.DimId = _dimList.Count;

            _dimList.Add(aDim);
            ndims = _dimList.Count;

            return aDim;
        }

        /// <summary>
        /// Add a variable
        /// </summary>
        /// <param name="aVar">variable</param>
        public void AddVariable(Variable aVar)
        {
            aVar.VarId = Variables.Count;
            Variables.Add(aVar);
            nvars = Variables.Count;
        }

        /// <summary>
        /// Add a new variable in cn file
        /// </summary>
        /// <param name="aVarS">variable</param>
        /// <param name="ncid">nc file identifer</param>
        /// <returns>is ok</returns>
        public bool AddNewVariable(Variable aVarS, int ncid)
        {
            int res = NetCDF4.nc_redef(ncid);
            if (res != 0) { goto ERROR; }

            int varid = aVarS.VarId;
            res = NetCDF4.nc_def_var(ncid, aVarS.Name, (int)aVarS.NCType,
                aVarS.DimNumber, aVarS.DimIds, out varid);
            if (res != 0) { goto ERROR; }

            foreach (AttStruct aAttS in aVarS.Attributes)
            {
                WriteAtt(ncid, varid, aAttS);
            }

            res = NetCDF4.nc_enddef(ncid);
            if (res != 0) { goto ERROR; }

            aVarS.VarId = varid;
            Variables.Add(aVarS);
            nvars = Variables.Count;
            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }

        /// <summary>
        /// Add a variable
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <param name="ncType">NcType</param>
        /// <param name="dims">dimension list</param>
        public void AddVariable(string varName, NetCDF4.NcType ncType, List<Dimension> dims)
        {
            Variable aVar = new Variable();
            aVar.VarId = Variables.Count;
            aVar.Name = varName;
            aVar.NCType = ncType;
            //aVar.nDims = dims.Length;
            aVar.Dimensions = dims;

            Variables.Add(aVar);
            nvars = Variables.Count;            
        }

        /// <summary>
        /// Add a variable
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <param name="ncType">NcType</param>
        /// <param name="dims">dimension list</param>
        public void AddVariable(string varName, NetCDF4.NcType ncType, Dimension[] dims)
        {
            List<Dimension> _dimList = new List<Dimension>(dims);
            AddVariable(varName, ncType, _dimList);
        }        

        private int GetVariableID(string varName)
        {
            int varID = -1;
            foreach (Variable aVar in Variables)
            {
                if (aVar.Name == varName)
                {
                    varID = aVar.VarId;
                    break;
                }
            }

            return varID;
        }

        /// <summary>
        /// Add attribute to the variable
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <param name="attName">attribute name</param>
        /// <param name="attValue">attribute value</param>
        public void AddVariableAttribute(string varName, string attName, string attValue)
        {            
            Variable aVar = GetVariable(varName);
            aVar.AddAttribute(attName, attValue);
        }

        /// <summary>
        /// Add attribute to the variable
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <param name="attName">attribute name</param>
        /// <param name="attValue">attribute value</param>
        public void AddVariableAttribute(string varName, string attName, double attValue)
        {
            Variable aVar = GetVariable(varName);
            aVar.AddAttribute(attName, attValue);
        }

        /// <summary>
        /// Add global attribute
        /// </summary>
        /// <param name="gAttName">attribute name</param>
        /// <param name="gAttValue">attribute value</param>
        public void AddGlobalAttribute(string gAttName, string gAttValue)
        {
            AttStruct aAtt = new AttStruct();
            aAtt.attName = gAttName;
            aAtt.attValue = gAttValue;
            aAtt.NCType = NetCDF4.NcType.NC_CHAR;
            aAtt.attLen = gAttValue.Length;

            gAttList.Add(aAtt);
            natts = gAttList.Count;
        }

        /// <summary>
        /// Create NetCDF file
        /// </summary>
        /// <param name="aFile">file path</param>
        public void CreateNCFile(string aFile)
        {
            int res = NetCDF4.nc_create(aFile, (int)NetCDF4.CreateMode.NC_CLOBBER, out ncid);

            //Write data info
            WriteNetCDFDataInfo(ncid);
        }

        /// <summary>
        /// Write NetCDF data info
        /// </summary>
        /// <param name="ncid">data file identifer</param>        
        /// <returns>is correct</returns>
        private bool WriteNetCDFDataInfo(int ncid)
        {
            int res, i;

            //Define dimensions
            int dimid = 0;
            for (i = 0; i < ndims; i++)
            {
                Dimension aDimS = _dimList[i];

                if (aDimS.DimId == unlimdimid)
                {
                    res = NetCDF4.nc_def_dim(ncid, aDimS.DimName,
                        NetCDF4.NC_UNLIMITED, out dimid);
                    if (res != 0) { goto ERROR; }
                }
                else
                {
                    res = NetCDF4.nc_def_dim(ncid, aDimS.DimName,
                        aDimS.DimLength, out dimid);
                    if (res != 0) { goto ERROR; }
                }

                aDimS.DimId = dimid;
                _dimList[i] = aDimS;
            }

            //Define variables            
            int varid = 0;
            for (i = 0; i < Variables.Count; i++)
            {
                Variable aVarS = Variables[i];

                res = NetCDF4.nc_def_var(ncid, aVarS.Name, (int)aVarS.NCType,
                    aVarS.DimNumber, aVarS.DimIds, out varid);
                if (res != 0) { goto ERROR; }

                aVarS.VarId = varid;
                Variables[i] = aVarS;
            }

            //Write attribute data                
            foreach (AttStruct aAttS in gAttList)
            {
                WriteAtt(ncid, NetCDF4.NC_GLOBAL, aAttS);
            }
            for (i = 0; i < nvars; i++)
            {
                foreach (AttStruct aAttS in Variables[i].Attributes)
                {
                    WriteAtt(ncid, Variables[i].VarId, aAttS);
                }
            }

            //End define
            res = NetCDF4.nc_enddef(ncid);
            if (res != 0) { goto ERROR; }

            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }

        /// <summary>
        /// Write variable data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <param name="data">data array</param>
        public void WriteVar(string varName, object[] data)
        {
            Variable aVar = GetVariable(varName);
            WriteVar(aVar, data);            
        }

        /// <summary>
        /// Write variable data
        /// </summary>
        /// <param name="aVar">variable</param>
        /// <param name="data">data array</param>
        public void WriteVar(Variable aVar, object[] data)
        {
            WriteVarData(aVar.VarId, aVar.NCType, data);
        }

        /// <summary>
        /// Write variable data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <param name="data">data array</param>
        public void WriteVar(string varName, double[] data)
        {
            Variable aVar = GetVariable(varName);
            double[] vardouble = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                vardouble[i] = double.Parse(data[i].ToString());
            }
            int res = NetCDF4.nc_put_var_double(ncid, aVar.VarId, vardouble);
        }

        /// <summary>
        /// Write variable data
        /// </summary>
        /// <param name="aVar">variable</param>
        /// <param name="start">start position array</param>
        /// <param name="count">count array</param>
        /// <param name="data">data array</param>
        public void WriteVara(Variable aVar, int[] start, int[] count, object[] data)
        {
            WriteVaraData(aVar.VarId, aVar.NCType, start, count, data);
        }

        /// <summary>
        /// Write variable data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <param name="start">start position array</param>
        /// <param name="count">count array</param>
        /// <param name="data">data array</param>
        public void WriteVara(string varName, int[] start, int[] count, object[] data)
        {
            Variable aVar = GetVariable(varName);
            WriteVara(aVar, start, count, data);
        }

        /// <summary>
        /// Write variable data
        /// </summary>
        /// <param name="varName">variable name</param>
        /// <param name="start">start position array</param>
        /// <param name="count">count array</param>
        /// <param name="data">data array</param>
        public void WriteVara(string varName, int[] start, int[] count, double[] data)
        {
            Variable aVar = GetVariable(varName);
            double[] vardouble = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                vardouble[i] = double.Parse(data[i].ToString());
            }
            int res = NetCDF4.nc_put_vara_double(ncid, aVar.VarId, start, count, vardouble);
        }

        /// <summary>
        /// Write data of a variable
        /// </summary>       
        /// <param name="varid">variable identifer</param>        
        /// <param name="ncType">NetCDF data type</param>
        /// <param name="data">data array</param>
        /// <returns>is correct</returns>
        public bool WriteVarData(int varid, NetCDF4.NcType ncType,
            object[] data)
        {
            int res, i;
            switch (ncType)
            {
                case NetCDF4.NcType.NC_CHAR:
                    string[] varchar = new string[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varchar[i] = (string)data[i];
                    }
                    res = NetCDF4.nc_put_var_string(ncid, varid, varchar);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_BYTE:
                    byte[] varbyte = new byte[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varbyte[i] = (byte)data[i];
                    }
                    res = NetCDF4.nc_put_var_ubyte(ncid, varid, varbyte);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_SHORT:
                    Int16[] varshort = new Int16[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varshort[i] = (Int16)data[i];
                    }
                    res = NetCDF4.nc_put_var_short(ncid, varid, varshort);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_INT:
                    int[] varint = new int[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varint[i] = (int)data[i];
                    }
                    res = NetCDF4.nc_put_var_int(ncid, varid, varint);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    Single[] varfloat = new Single[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varfloat[i] = (Single)data[i];
                    }
                    res = NetCDF4.nc_put_var_float(ncid, varid, varfloat);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    double[] vardouble = new double[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        vardouble[i] = double.Parse(data[i].ToString());
                    }
                    res = NetCDF4.nc_put_var_double(ncid, varid, vardouble);
                    if (res != 0) { goto ERROR; }
                    break;
            }

            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }

        /// <summary>
        /// Write data of a variable
        /// </summary>      
        /// <param name="ncid">nc file identifer</param>
        /// <param name="varid">variable identifer</param>        
        /// <param name="ncType">NetCDF data type</param>
        /// <param name="data">data array</param>
        /// <returns>is correct</returns>
        public bool WriteVarData(int ncid, int varid, NetCDF4.NcType ncType,
            object[] data)
        {
            int res, i;
            switch (ncType)
            {
                case NetCDF4.NcType.NC_CHAR:
                    string[] varchar = new string[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varchar[i] = (string)data[i];
                    }
                    res = NetCDF4.nc_put_var_string(ncid, varid, varchar);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_BYTE:
                    byte[] varbyte = new byte[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varbyte[i] = (byte)data[i];
                    }
                    res = NetCDF4.nc_put_var_ubyte(ncid, varid, varbyte);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_SHORT:
                    Int16[] varshort = new Int16[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varshort[i] = (Int16)data[i];
                    }
                    res = NetCDF4.nc_put_var_short(ncid, varid, varshort);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_INT:
                    int[] varint = new int[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varint[i] = (int)data[i];
                    }
                    res = NetCDF4.nc_put_var_int(ncid, varid, varint);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    Single[] varfloat = new Single[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varfloat[i] = (Single)data[i];
                    }
                    res = NetCDF4.nc_put_var_float(ncid, varid, varfloat);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    double[] vardouble = new double[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        vardouble[i] = double.Parse(data[i].ToString());
                    }
                    res = NetCDF4.nc_put_var_double(ncid, varid, vardouble);
                    if (res != 0) { goto ERROR; }
                    break;
            }

            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }

        /// <summary>
        /// Write data of a variable
        /// </summary>       
        /// <param name="varid">variable identifer</param>        
        /// <param name="ncType">NetCDF data type</param>
        /// <param name="start">start position array</param>
        /// <param name="count">count array</param>
        /// <param name="data">data array</param>
        /// <returns>is correct</returns>
        public bool WriteVaraData(int varid, NetCDF4.NcType ncType, int[] start, int[] count,
            object[] data)
        {
            int res, i;
            switch (ncType)
            {
                case NetCDF4.NcType.NC_CHAR:
                    //string[] varchar = new string[data.Length];
                    //for (i = 0; i < data.Length; i++)
                    //{
                    //    varchar[i] = (string)data[i];
                    //}
                    //res = NetCDF4.nc_put_vara_string(ncid, varid, start, count, varchar);

                    byte[] varchar = new byte[data.Length];
                    for (i = 0; i < data.Length; i++)
                        varchar[i] = (byte)data[i];
                    res = NetCDF4.nc_put_vara_text(ncid, varid, start, count, varchar);

                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_BYTE:
                    byte[] varbyte = new byte[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varbyte[i] = (byte)data[i];
                    }
                    res = NetCDF4.nc_put_vara_ubyte(ncid, varid, start, count, varbyte);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_SHORT:
                    Int16[] varshort = new Int16[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varshort[i] = (Int16)data[i];
                    }
                    res = NetCDF4.nc_put_vara_short(ncid, varid, start, count, varshort);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_INT:
                    int[] varint = new int[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varint[i] = (int)data[i];
                    }
                    res = NetCDF4.nc_put_vara_int(ncid, varid, start, count, varint);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    Single[] varfloat = new Single[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varfloat[i] = (Single)data[i];
                    }
                    res = NetCDF4.nc_put_vara_float(ncid, varid, start, count, varfloat);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    double[] vardouble = new double[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        vardouble[i] = double.Parse(data[i].ToString());
                    }
                    res = NetCDF4.nc_put_vara_double(ncid, varid, start, count, vardouble);
                    if (res != 0) { goto ERROR; }
                    break;
            }

            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }

        /// <summary>
        /// Write data of a variable
        /// </summary>       
        /// <param name="ncid">nc identifer</param>
        /// <param name="varid">variable identifer</param>        
        /// <param name="ncType">NetCDF data type</param>
        /// <param name="start">start position array</param>
        /// <param name="count">count array</param>
        /// <param name="data">data array</param>
        /// <returns>is correct</returns>
        public bool WriteVaraData(int ncid, int varid, NetCDF4.NcType ncType, int[] start, int[] count,
            object[] data)
        {
            int res, i;
            switch (ncType)
            {
                case NetCDF4.NcType.NC_CHAR:
                    //string[] varchar = new string[data.Length];
                    //for (i = 0; i < data.Length; i++)
                    //{
                    //    varchar[i] = (string)data[i];
                    //}
                    //res = NetCDF4.nc_put_vara_string(ncid, varid, start, count, varchar);

                    byte[] varchar = new byte[data.Length];
                    for (i = 0; i < data.Length; i++)
                        varchar[i] = (byte)data[i];
                    res = NetCDF4.nc_put_vara_text(ncid, varid, start, count, varchar);

                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_BYTE:
                    byte[] varbyte = new byte[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varbyte[i] = (byte)data[i];
                    }
                    res = NetCDF4.nc_put_vara_ubyte(ncid, varid, start, count, varbyte);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_SHORT:
                    Int16[] varshort = new Int16[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varshort[i] = (Int16)data[i];
                    }
                    res = NetCDF4.nc_put_vara_short(ncid, varid, start, count, varshort);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_INT:
                    int[] varint = new int[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varint[i] = (int)data[i];
                    }
                    res = NetCDF4.nc_put_vara_int(ncid, varid, start, count, varint);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    Single[] varfloat = new Single[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        varfloat[i] = (Single)data[i];
                    }
                    res = NetCDF4.nc_put_vara_float(ncid, varid, start, count, varfloat);
                    if (res != 0) { goto ERROR; }
                    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    double[] vardouble = new double[data.Length];
                    for (i = 0; i < data.Length; i++)
                    {
                        vardouble[i] = double.Parse(data[i].ToString());
                    }
                    res = NetCDF4.nc_put_vara_double(ncid, varid, start, count, vardouble);
                    if (res != 0) { goto ERROR; }
                    break;
            }

            return true;

        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");
            return false;
        }

        /// <summary>
        /// Close NetCDF file
        /// </summary>
        public void CloseNCFile()
        {
            NetCDF4.nc_close(ncid);
        }

        #endregion

        #endregion
    }
}
