using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Projections;
using HDF5DotNet;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// HDF 5 data info
    /// </summary>
    public class HDF5DataInfo : DataInfo,IGridDataInfo,IStationDataInfo
    {
        #region Variables
        ///// <summary>
        ///// File name
        ///// </summary>
        //public string FileName;
        /// <summary>
        /// X coordinates
        /// </summary>
        public double[] X;
        /// <summary>
        /// Y coordinates
        /// </summary>
        public double[] Y;
        ///// <summary>
        ///// time list
        ///// </summary>
        //public List<DateTime> Times;
        /// <summary>
        /// level list
        /// </summary>
        public List<float> Levels = new List<float>();

        private H5FileId _fileID;
        private string _metaStr;
        private List<Dimension> _dimensions = new List<Dimension>();
        private List<Dimension> _swathDimensions = new List<Dimension>();
        private Variable _currentVariable = null;
        private bool _isSWATH = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public HDF5DataInfo()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or set current variable
        /// </summary>
        public Variable CurrentVariable
        {
            get { return _currentVariable; }
            set { _currentVariable = value; }
        }

        /// <summary>
        /// Get if is SWATH
        /// </summary>
        public bool IsSWATH
        {
            get { return _isSWATH; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read data info from a file
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>is ok</returns>
        public override void ReadDataInfo(string aFile)
        {
            if (!File.Exists(aFile))
                return;

            GetDataInfo(aFile);
        }

        private bool GetDataInfo(string aFile)
        {
            FileName = aFile;
            //For HDF EOS5 data
            //Open the file
            _fileID = H5F.open(aFile, H5F.OpenMode.ACC_RDONLY);
            
            //Read StructMetadata.X
            _metaStr = ReadStructMetaString();
            if (_metaStr == String.Empty)
                return false;

            string[] lines = _metaStr.Split('\n');
            List<string> mlines = new List<string>(lines);

            //Get SwathStructure
            MetaGroup ssGroup = new MetaGroup(mlines, "SwathStructure");
            if (ssGroup.Lines.Count >= 3)
            {
                //Get SWATH_1
                MetaGroup s1Group = ssGroup.GetGroup("SWATH_1");
                string swathName = s1Group.GetParaStr("SwathName").Trim('"');
                //Get dimensions
                MetaGroup sDimGroup = s1Group.GetGroup("Dimension");
                _swathDimensions = GetDimensions(sDimGroup);                
                //Get variables
                MetaGroup varGroup = s1Group.GetGroup("DataField");
                Variables.AddRange(GetSwathVariables(varGroup, swathName));
                _isSWATH = true;
            }

            //Get GridStructure
            MetaGroup gsGroup = new MetaGroup(mlines, "GridStructure");
            if (gsGroup.Lines.Count >= 3)
            {
                //Get GRID_1
                MetaGroup g1Group = gsGroup.GetGroup("GRID_1");
                //Get projection
                string gridName = g1Group.GetParaStr("GridName").Trim('"');
                if (!GetProjectionInfo(g1Group))
                    return false;

                //Get X/Y coordinate
                GetXYCoord(g1Group);
                //get dimensions
                MetaGroup dimGroup = g1Group.GetGroup("Dimension");
                _dimensions = new List<Dimension>();
                Dimension aDim = new Dimension(DimensionType.X);
                aDim.DimName = "XDim";
                aDim.SetValues(new List<double>(X));
                _dimensions.Add(aDim);
                aDim = new Dimension(DimensionType.Y);
                aDim.DimName = "YDim";
                aDim.SetValues(new List<double>(Y));
                _dimensions.Add(aDim);
                _dimensions.AddRange(GetDimensions(dimGroup));
                GetGridDimensionValues();
                //Get variables
                MetaGroup varGroup = g1Group.GetGroup("DataField");                
                Variables.AddRange(GetGridVariables(varGroup, gridName));
            }

            if (Variables.Count == 0)
                return false;
            
            H5F.close(_fileID);

            return true;
        }

        private string ReadStructMetaString()
        {
            H5GroupId infoGId = H5G.open(_fileID, "HDFEOS INFORMATION");
            string metaStr = String.Empty;
            var num_meta = H5G.getNumObjects(infoGId);
            for (int i = 0; i < (int)num_meta; i++)
            {
                string metaName = H5G.getObjectNameByIndex(infoGId, (ulong)i);
                if (metaName.Contains("StructMetadata"))
                {
                    ObjectInfo metaInfo = H5G.getObjectInfo(infoGId, metaName, false);
                    H5DataSetId dsId = H5D.open(infoGId, metaName);
                    H5DataSpaceId spaId = H5D.getSpace(dsId);
                    H5DataTypeId dtId = H5D.getType(dsId);
                    var size = H5T.getSize(dtId);
                    H5T.H5TClass tcls = H5T.getClass(dtId);
                    int rank = H5S.getSimpleExtentNDims(spaId);
                    var dims = H5S.getSimpleExtentDims(spaId);
                    //string[] outdata = new string[1];
                    H5DataTypeId dtypeId = H5T.copy(H5T.H5Type.C_S1);
                    H5T.setSize(dtypeId, size);
                    byte[] outdata = new byte[size];
                    H5D.read(dsId, dtypeId, new H5Array<byte>(outdata));
                    metaStr = System.Text.ASCIIEncoding.ASCII.GetString(outdata);

                    H5D.close(dsId);
                    H5S.close(spaId);
                    H5T.close(dtId);
                    
                    break;
                }
            }

            return metaStr;
        }

        private void GetXYCoord(MetaGroup aGroup)
        {
            int xNum = int.Parse(aGroup.GetParaStr("XDim"));
            int yNum = int.Parse(aGroup.GetParaStr("YDim"));
            string[] ul = aGroup.GetParaStr("UpperLeftPointMtrs").Split(',');
            string[] lr = aGroup.GetParaStr("LowerRightMtrs").Split(',');
            double minX, maxX, minY, maxY;
            if (this.ProjectionInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat)
            {
                minX = GetLonLatValue(ul[0].Substring(1), true);
                maxX = GetLonLatValue(lr[0].Substring(1), true);
                minY = GetLonLatValue(lr[1].Substring(0, lr[1].Length - 1), false);
                maxY = GetLonLatValue(ul[1].Substring(0, ul[1].Length - 1), false);
            }
            else
            {
                minX = double.Parse(ul[0].Substring(1));
                maxX = double.Parse(lr[0].Substring(1));
                minY = double.Parse(lr[1].Substring(0, lr[1].Length - 1));
                maxY = double.Parse(ul[1].Substring(0, ul[1].Length - 1));                
            }

            if (minY > maxY)
            {
                this.IsYReverse = true;
                double v = minY;
                minY = maxY;
                maxY = v;
            }

            double dX = (maxX - minX) / xNum;
            double dY = (maxY - minY) / yNum;
            X = new double[xNum];
            for (int i = 0; i < xNum; i++)
                X[i] = minX + dX * i;

            Y = new double[yNum];
            for (int i = 0; i < yNum; i++)
                Y[i] = minY + dY * i;
        }

        private double GetLonLatValue(string llStr, bool isLon)
        {
            int d, m;
            double s;
            int sIdx = 0;
            if (llStr.Substring(0, 1) == "-")
                sIdx = 1;

            int dNum = 3;
            if (!isLon)
                dNum = 2;

            d = int.Parse(llStr.Substring(sIdx, dNum));
            m = int.Parse(llStr.Substring(sIdx + dNum, 3));
            s = double.Parse(llStr.Substring(sIdx + dNum + 3));
            double v = d + (m + s / 60) / 60;
            if (sIdx == 1)
                v = -v;

            return v;
        }
        
        private bool GetProjectionInfo(MetaGroup aGroup)
        {
            string projType = aGroup.GetParaStr("Projection");
            switch (projType)
            {
                case "HE5_GCTP_GEO":

                    return true;
                default:

                    return false;
            }
        }

        private List<Dimension> GetDimensions(MetaGroup aGroup)
        {
            List<Dimension> dims = new List<Dimension>();
            Dimension aDim = new Dimension(DimensionType.Other);            

            for (int i = 1; i <= 10; i++)
            {
                string dimObjName = "Dimension_" + i.ToString();
                MetaObject dimObj = aGroup.GetObject(dimObjName);
                if (dimObj.ParaLines.Count > 0)
                {
                    string dimName = dimObj.GetParaStr("DimensionName");
                    int size = int.Parse(dimObj.GetParaStr("Size"));
                    dimName = dimName.Trim('"');
                    DimensionType dimType = DimensionType.Other;
                    switch (dimName.ToLower())
                    {
                        case "xdim":
                            continue;                            
                        case "ydim":
                            continue;  
                        case "ntimes":
                            dimType = DimensionType.T;
                            break;
                        case "nxtrack":
                            dimType = DimensionType .Xtrack ;
                            break ;
                        default:
                            dimType = DimensionType.Other;
                            break;
                    }
                    aDim = new Dimension(dimType);
                    aDim.DimName = dimName;
                    switch (dimType)
                    {              
                        case DimensionType.T:
                            for (int j = 0; j < size; j++)
                                aDim.DimValue.Add(8.0E-267);
                            break;
                        default:
                            for (int j = 0; j < size; j++)
                                aDim.DimValue.Add(j + 1);
                            break;
                    }
                    aDim.DimLength = aDim.DimValue.Count;
                    dims.Add(aDim);
                }
                else
                    break;
            }

            return dims;
        }

        private void GetGridDimensionValues()
        {
            //Open data set group
            H5GroupId dgroupId = H5G.open(_fileID, "HDFEOS");
            H5GroupId gridsGId = H5G.open(dgroupId, "GRIDS");
            string gName = H5G.getObjectNameByIndex(gridsGId, 0);
            H5GroupId dataGId = H5G.open(gridsGId, gName);
            H5GroupId groupId = H5G.open(dataGId, "Data Fields");
            string hdfPath = "/HDFEOS/GRIDS/" + gName + "/Data Fields";
            ulong num_objs = (ulong)H5G.getNumObjects(groupId);
            for (int i = 0; i < (int)num_objs; i++)
            {
                string objName = H5G.getObjectNameByIndex(groupId, (ulong)i);
                ObjectInfo oInfo = H5G.getObjectInfo(groupId, objName, false);
                if (oInfo.objectType == H5GType.DATASET)
                {
                    H5DataSetId dsId = H5D.open(groupId, objName);
                    H5DataSpaceId spaId = H5D.getSpace(dsId);
                    H5DataTypeId dtId = H5D.getType(dsId);
                    //H5T.H5TClass tcls = H5T.getClass(dtId);

                    int rank = H5S.getSimpleExtentNDims(spaId);
                    if (rank != 1)
                        continue;

                    var dims = H5S.getSimpleExtentDims(spaId);
                    int dimIdx = -1;
                    for (int d = 0; d < _dimensions.Count; d++)
                    {
                        if (_dimensions[d].DimLength == (int)dims[0])
                        {
                            dimIdx = d;
                            break;
                        }
                    }
                    if (dimIdx == -1)
                        continue;
                                       
                    H5DataTypeId dtypeId = H5T.copy(H5T.H5Type.NATIVE_FLOAT);
                    float[] outdata = new float[dims[0]];
                    H5D.read(dsId, dtypeId, new H5Array<float>(outdata));
                    for (int j = 0; j < outdata.Length; j++)
                        _dimensions[dimIdx].DimValue.Add(outdata[j]);

                    H5D.close(dsId);
                    H5S.close(spaId);
                    H5T.close(dtId);
                }
            }            

            //Close
            H5G.close(dgroupId);
            H5G.close(gridsGId);
            H5G.close(dataGId);
            H5G.close(groupId);
        }

        private List<Variable> GetGridVariables(MetaGroup aGroup, string gridName)
        {
            List<Variable> vars = new List<Variable>();
            string hdfPath = "/HDFEOS/GRIDS/" + gridName + "/Data Fields";
            for (int i = 1; i <= 100; i++)
            {
                string varObjName = "DataField_" + i.ToString();
                MetaObject varObj = aGroup.GetObject(varObjName);
                if (varObj.ParaLines.Count > 0)
                {
                    Variable aVar = new Variable();
                    aVar.Name = varObj.GetParaStr("DataFieldName").Trim('"');
                    string[] dimNames = varObj.GetParaStr("DimList").Trim(')').Substring(1).Split(',');
                    List<Dimension> dims = new List<Dimension>();
                    Dimension levelDim = null;
                    for (int j = 0; j < dimNames.Length; j++)
                    {
                        string dName = dimNames[j].Trim('"');
                        foreach (Dimension aDim in _dimensions)
                        {
                            if (aDim.DimName == dName)
                            {
                                dims.Add(aDim);
                                if (aDim.DimType != DimensionType.X && aDim.DimType != DimensionType.Y)
                                    levelDim = aDim;
                                break;
                            }
                        }
                    }
                    aVar.Dimensions = dims;
                    aVar.HDFPath = hdfPath;
                    if (aVar.DimNumber == 2 || aVar.DimNumber == 3)
                    {
                        if (aVar.DimNumber == 3)
                        {
                            aVar.Levels = levelDim.DimValue;
                        }
                        vars.Add(aVar);
                    }
                }
            }

            return vars;
        }

        private void GetSwathDimensionValue()
        {
            //Open data set group
            H5GroupId dgroupId = H5G.open(_fileID, "HDFEOS");
            H5GroupId swathsGId = H5G.open(dgroupId, "SWATHS");
            string gName = H5G.getObjectNameByIndex(swathsGId, 0);
            H5GroupId dataGId = H5G.open(swathsGId, gName);
            H5GroupId groupId = H5G.open(dataGId, "Geolocation Fields");            
            var num_objs = H5G.getNumObjects(groupId);
            for (int i = 0; i < (int)num_objs; i++)
            {
                string objName = H5G.getObjectNameByIndex(groupId, (ulong)i);
                ObjectInfo oInfo = H5G.getObjectInfo(groupId, objName, false);
                if (oInfo.objectType == H5GType.DATASET)
                {
                    H5DataSetId dsId = H5D.open(groupId, objName);
                    H5DataSpaceId spaId = H5D.getSpace(dsId);
                    H5DataTypeId dtId = H5D.getType(dsId);
                    //H5T.H5TClass tcls = H5T.getClass(dtId);

                    int rank = H5S.getSimpleExtentNDims(spaId);
                    if (rank != 1)
                        continue;

                    var dims = H5S.getSimpleExtentDims(spaId);
                    int dimIdx = -1;
                    for (int d = 0; d < _dimensions.Count; d++)
                    {
                        if (_dimensions[d].DimLength == (int)dims[0])
                        {
                            dimIdx = d;
                            break;
                        }
                    }
                    if (dimIdx == -1)
                        continue;

                    H5DataTypeId dtypeId = H5T.copy(H5T.H5Type.NATIVE_FLOAT);
                    float[] outdata = new float[dims[0]];
                    H5D.read(dsId, dtypeId, new H5Array<float>(outdata));
                    for (int j = 0; j < outdata.Length; j++)
                        _dimensions[dimIdx].DimValue.Add(outdata[j]);

                    H5D.close(dsId);
                    H5S.close(spaId);
                    H5T.close(dtId);
                }
            }

            //Close
            H5G.close(dgroupId);
            H5G.close(swathsGId);
            H5G.close(dataGId);
            H5G.close(groupId);
        }

        private List<Variable> GetSwathVariables(MetaGroup aGroup, string varName)
        {
            List<Variable> vars = new List<Variable>();
            string hdfPath = "/HDFEOS/SWATHS/" + varName + "/Data Fields";
            for (int i = 1; i <= 100; i++)
            {
                string varObjName = "DataField_" + i.ToString();
                MetaObject varObj = aGroup.GetObject(varObjName);
                if (varObj.ParaLines.Count > 0)
                {
                    Variable aVar = new Variable();
                    aVar.IsSwath = true;                    
                    aVar.Name = varObj.GetParaStr("DataFieldName").Trim('"');
                    string[] dimNames = varObj.GetParaStr("DimList").Trim(')').Substring(1).Split(',');
                    List<Dimension> dims = new List<Dimension>();
                    Dimension levelDim = null;
                    for (int j = 0; j < dimNames.Length; j++)
                    {
                        string dName = dimNames[j].Trim('"');
                        foreach (Dimension aDim in _swathDimensions)
                        {
                            if (aDim.DimName == dName)
                            {
                                dims.Add(aDim);
                                if (aDim.DimType != DimensionType.T && aDim.DimType != DimensionType.Xtrack)
                                    levelDim = aDim;
                                break;
                            }
                        }
                    }
                    aVar.Dimensions = dims;
                    aVar.HDFPath = hdfPath;
                    if (aVar.DimNumber <= 3)
                    {
                        if (levelDim != null)
                        {
                            aVar.Levels = levelDim.DimValue;
                        }
                        vars.Add(aVar);
                    }
                }
            }

            return vars;
        }

        //private void GetDataInfo_back(string aFile)
        //{
        //    FileName = aFile;
        //    //For HDF EOS5 data
        //    //Open the file
        //    _fileID = H5F.open(aFile, H5F.OpenMode.ACC_RDONLY);

        //    //Open data set group
        //    H5GroupId dgroupId = H5G.open(_fileID, "HDFEOS");
        //    H5GroupId gridsGId = H5G.open(dgroupId, "GRIDS");
        //    string gName = H5G.getObjectNameByIndex(gridsGId, 0);
        //    H5GroupId dataGId = H5G.open(gridsGId, gName);
        //    H5GroupId groupId = H5G.open(dataGId, "Data Fields");
        //    string hdfPath = "/HDFEOS/GRIDS/" + gName + "/Data Fields";
        //    ulong num_objs = H5G.getNumObjects(groupId);
        //    for (int i = 0; i < (int)num_objs; i++)
        //    {
        //        string objName = H5G.getObjectNameByIndex(groupId, i);
        //        ObjectInfo oInfo = H5G.getObjectInfo(groupId, objName, false);
        //        if (oInfo.objectType == H5GType.DATASET)
        //        {
        //            H5DataSetId dsId = H5D.open(groupId, objName);
        //            H5DataSpaceId spaId = H5D.getSpace(dsId);
        //            H5DataTypeId dtId = H5D.getType(dsId);
        //            H5T.H5TClass tcls = H5T.getClass(dtId);

        //            int rank = H5S.getSimpleExtentNDims(spaId);
        //            ulong[] dims = H5S.getSimpleExtentDims(spaId);
        //            //char[] outdata = new char[32000];
        //            H5DataTypeId dtypeId;
        //            //H5D.read(dsId, dtypeId, new H5Array<char>(outdata));
        //            switch (objName.ToLower())
        //            {
        //                case "time":

        //                    break;
        //                case "longitude":
        //                    dtypeId = H5T.copy(H5T.H5Type.NATIVE_FLOAT);
        //                    float[] outdata = new float[dims[0]];
        //                    H5D.read(dsId, dtypeId, new H5Array<float>(outdata));
        //                    X = new double[outdata.Length];
        //                    for (int j = 0; j < outdata.Length; j++)
        //                        X[j] = outdata[j];
        //                    break;
        //                case "latitude":
        //                    dtypeId = H5T.copy(H5T.H5Type.NATIVE_FLOAT);
        //                    outdata = new float[dims[0]];
        //                    H5D.read(dsId, dtypeId, new H5Array<float>(outdata));
        //                    Y = new double[outdata.Length];
        //                    for (int j = 0; j < outdata.Length; j++)
        //                        Y[j] = outdata[j];
        //                    break;
        //                case "pressure":
        //                    dtypeId = H5T.copy(H5T.H5Type.NATIVE_FLOAT);
        //                    outdata = new float[dims[0]];
        //                    H5D.read(dsId, dtypeId, new H5Array<float>(outdata));
        //                    Levels = new List<float>();
        //                    for (int j = 0; j < outdata.Length; j++)
        //                        Levels.Add(outdata[j]);
        //                    break;
        //                default:
        //                    Variable aVar = new Variable();
        //                    aVar.Name = objName;
        //                    aVar.Dimensions = new int[rank];
        //                    aVar.HDFPath = hdfPath;
        //                    for (int j = 0; j < rank; j++)
        //                        aVar.Dimensions[j] = (int)dims[j];
        //                    Variables.Add(aVar);
        //                    break;
        //            }

        //            H5D.close(dsId);
        //            H5S.close(spaId);
        //            H5T.close(dtId);
        //        }
        //    }
        //    if (Levels.Count > 1)
        //    {
        //        foreach (Variable aVar in Variables)
        //        {
        //            if (aVar.DimNumber >= 3)
        //                aVar.Levels = Levels;
        //        }
        //    }

        //    //Close
        //    H5G.close(dgroupId);
        //    H5G.close(gridsGId);
        //    H5G.close(dataGId);
        //    H5G.close(groupId);
        //    H5F.close(_fileID);
        //}

        private void Backup(string aFile)
        {
            //For HDF EOS5 data
            //Open the file
            _fileID = H5F.open(aFile, H5F.OpenMode.ACC_RDONLY);

            //Open information group
            H5GroupId groupId = H5G.open(_fileID, "HDFEOS INFORMATION");
            string objName = "StructMetadata.0";
            ObjectInfo oInfo = H5G.getObjectInfo(groupId, objName, false);
            //System.Windows.Forms.MessageBox.Show(oInfo.objectType.ToString());
            H5DataSetId dsId = H5D.open(groupId, objName);
            H5DataSpaceId spaId = H5D.getSpace(dsId);
            H5DataTypeId dtId = H5D.getType(dsId);
            H5T.H5TClass tcls = H5T.getClass(dtId);
            int rank = H5S.getSimpleExtentNDims(spaId);
            var dims = H5S.getSimpleExtentDims(spaId);
            string[] outdata = new string[1];
            H5DataTypeId dtypeId = H5T.copy(H5T.H5Type.C_S1);
            H5D.read(dsId, dtypeId, new H5Array<string>(outdata));
            //System.Windows.Forms.MessageBox.Show(outdata[0]);

            #region Backup
            ////Open the root group
            //H5GroupId groupId = H5G.open(_fileID, "/");

            //// Get number of objects in the root group
            //ulong num_objs = H5G.getNumObjects(groupId);

            //for (int i = 0; i < (int)num_objs; i++)
            //{
            //    string objName = H5G.getObjectNameByIndex(groupId, i);
            //    ObjectInfo oInfo = H5G.getObjectInfo(groupId, objName, false);
            //    if (oInfo.objectType == H5GType.DATASET)
            //    {
            //        H5DataSetId dsId = H5D.open(groupId, objName);
            //        H5DataSpaceId spaId = H5D.getSpace(dsId);
            //        int rank = H5S.getSimpleExtentNDims(spaId);
            //        ulong[] dims = H5S.getSimpleExtentDims(spaId);
            //        ulong[] offset = new ulong[rank];
            //        ulong[] count = new ulong[rank];

            //        offset[0] = 0;
            //        offset[1] = 0;
            //        offset[2] = 0;
            //        offset[3] = 0;

            //        count[0] = 1;
            //        count[1] = 1;
            //        count[2] = 1;
            //        count[3] = dims[3];
            //        switch (objName.ToLower())
            //        {
            //            case "time":

            //                break;
            //            case "longitude":

            //                break;
            //            case "latitude":

            //                break;
            //        }
            //    }
            //}
            #endregion

            //Close
            H5G.close(groupId);
            H5F.close(_fileID);
        }

        /// <summary>
        /// Generate data info text
        /// </summary>        
        /// <returns>data info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            int i;
            dataInfo = "File Name: " + FileName;            

            dataInfo += Environment.NewLine + "Variations: " + Variables.Count;
            for (i = 0; i < Variables.Count; i++)
            {
                dataInfo += Environment.NewLine + "\t" + Variables[i].Name;
            }

            dataInfo += Environment.NewLine;
            string[] lines = _metaStr.Split('\n');
            for (i = 0; i < lines.Length; i++)
                dataInfo += Environment.NewLine + lines[i];

            return dataInfo;
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
            Variable aVar = Variables[varIdx];

            //Open the file
            _fileID = H5F.open(FileName, H5F.OpenMode.ACC_RDONLY);

            //Open data set group
            H5GroupId groupId = H5G.open(_fileID, aVar.HDFPath);
            H5DataSetId dsId = H5D.open(groupId, aVar.Name);
            H5DataSpaceId spaId = H5D.getSpace(dsId);
            H5DataTypeId dtId = H5D.getType(dsId);
            H5T.H5TClass tcls = H5T.getClass(dtId);

            H5DataTypeId dtypeId;
            dtypeId = H5T.copy(H5T.H5Type.NATIVE_FLOAT);

            //Read Attribute
            H5ObjectInfo oinfo = H5O.getInfo(dsId);
            int attNum = (int)oinfo.nAttributes;
            double undef = -9999.0;
            double offset = 0.0;
            double scaleFactor = 1.0;
            for (int i = 0; i < attNum; i++)
            {
                H5AttributeId attId = H5A.openIndex(dsId, (uint)i);
                switch (H5A.getName(attId))
                {
                    case "_FillValue":
                        float[] mValue = new float[1];
                        H5A.read(attId, dtypeId, new H5Array<float>(mValue));
                        undef = mValue[0];
                        break;
                    case "Offset":
                        float[] osValue = new float[1];
                        H5A.read(attId, dtypeId, new H5Array<float>(osValue));
                        offset = osValue[0];
                        break;
                    case "ScaleFactor":
                        float[] sfValue = new float[1];
                        H5A.read(attId, dtypeId, new H5Array<float>(sfValue));
                        scaleFactor = sfValue[0];
                        break;
                }

                H5A.close(attId);
            }

            //int rank = H5S.getSimpleExtentNDims(spaId);
            //ulong[] dims = H5S.getSimpleExtentDims(spaId);                        

            float[,] outdata = new float[aVar.Dimensions[0].DimLength, aVar.Dimensions[1].DimLength];
            switch(aVar.DimNumber)
            {
                case 2:
                    outdata = new float[aVar.Dimensions[0].DimLength, aVar.Dimensions[1].DimLength];
                    H5D.read(dsId, dtypeId, new H5Array<float>(outdata));
                    break;
                case 3:
                    float[, ,] outdata3 = new float[aVar.Dimensions[0].DimLength, aVar.Dimensions[1].DimLength, aVar.Dimensions[2].DimLength];
                    H5D.read(dsId, dtypeId, new H5Array<float>(outdata3));                    
                    if (aVar.Dimensions[0].DimType != DimensionType.X && aVar.Dimensions[0].DimType != DimensionType.Y)
                    {
                        outdata = new float[aVar.Dimensions[1].DimLength, aVar.Dimensions[2].DimLength];
                        for (int i = 0; i < aVar.Dimensions[1].DimLength; i++)
                        {
                            for (int j = 0; j < aVar.Dimensions[2].DimLength; j++)
                                outdata[i, j] = outdata3[levelIdx, i, j];
                        }
                    }
                    else if (aVar.Dimensions[1].DimType != DimensionType.X && aVar.Dimensions[1].DimType != DimensionType.Y)
                    {
                        outdata = new float[aVar.Dimensions[0].DimLength, aVar.Dimensions[2].DimLength];
                        for (int i = 0; i < aVar.Dimensions[0].DimLength; i++)
                        {
                            for (int j = 0; j < aVar.Dimensions[2].DimLength; j++)
                                outdata[i, j] = outdata3[i, levelIdx, j];
                        }
                    }
                    else
                    {
                        outdata = new float[aVar.Dimensions[0].DimLength, aVar.Dimensions[1].DimLength];
                        for (int i = 0; i < aVar.Dimensions[0].DimLength; i++)
                        {
                            for (int j = 0; j < aVar.Dimensions[1].DimLength; j++)
                                outdata[i, j] = outdata3[i, j, levelIdx];
                        }
                    }
                    break;
            }


            H5D.close(dsId);
            H5S.close(spaId);
            H5T.close(dtId);

            H5G.close(groupId);
            H5F.close(_fileID);


            Dimension xDim = aVar.GetDimension(DimensionType.X);
            Dimension yDim = aVar.GetDimension(DimensionType.Y);
            int xDimIdx = aVar.GetDimIndex(xDim);
            int yDimIdx = aVar.GetDimIndex(yDim);

            GridData gridData = new GridData();
            gridData.X = X;
            gridData.Y = Y;
            gridData.Data = new double[yDim.DimLength, xDim.DimLength];
            gridData.MissingValue = undef;

            if (yDimIdx < xDimIdx)
            {                
                for (int i = 0; i < yDim.DimLength; i++)
                {
                    for (int j = 0; j < xDim.DimLength; j++)
                    {
                        gridData.Data[i, j] = outdata[i, j] * scaleFactor + offset;
                    }
                }
            }
            else
            {
                for (int i = 0; i < yDim.DimLength; i++)
                {
                    for (int j = 0; j < xDim.DimLength; j++)
                    {
                        gridData.Data[i, j] = outdata[j, i] * scaleFactor + offset;
                    }
                }
            }
            
            //if (ProjInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat)            
            //{
            //    if (gridData.X[gridData.XNum - 1] + (gridData.X[1] - gridData.X[0]) -
            //        gridData.X[0] == 360)
            //    {
            //        double[] newX = new double[xDim.DimLength + 1];
            //        for (int i = 0; i < X.Length; i++)
            //            newX[i] = X[i];
            //        newX[newX.Length - 1] = X[X.Length - 1] + X[1] - X[0];
            //        double[,] newData = new double[yDim.DimLength, xDim.DimLength + 1];
            //        for (int i = 0; i < yDim.DimLength; i++)
            //        {
            //            for (int j = 0; j < xDim.DimLength; j++)
            //            {
            //                newData[i, j] = gridData.Data[i, j];
            //            }
            //            newData[i, newX.Length - 1] = gridData.Data[i, 0];
            //        }

            //        gridData.X = newX;
            //        gridData.Data = newData;
            //    }
            //}

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
        /// Get station data
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="varIdx">variable index</param>
        /// <param name="levelIdx">level index</param>
        /// <returns>station data</returns>
        public StationData GetStationData(int timeIdx, int varIdx, int levelIdx)
        {
            StationData stData = new StationData();
            Variable aVar = Variables[varIdx];

            //Open the file
            _fileID = H5F.open(FileName, H5F.OpenMode.ACC_RDONLY);

            //Get data                  
            double[,] discretedData;
            double undef = -999.0, lonUndef = -999.0, latUndef = -999.0;
            int stNum;
            Global.Extent aExtent;
            if (aVar.HasXtrackDimension())
            {
                string varPath = aVar.HDFPath + "/" + aVar.Name;
                double[,] outdata = GetTwoDimData(varPath, levelIdx, ref undef);
                stNum = outdata.GetLength(0) * outdata.GetLength(1);
                discretedData = new double[3, stNum];
                for (int i = 0; i < outdata.GetLength(0); i++)
                {
                    for (int j = 0; j < outdata.GetLength(1); j++)
                        discretedData[2, i * outdata.GetLength(1) + j] = outdata[i, j];
                }

                //Get lon/lat               
                double minX, maxX;
                varPath = aVar.HDFPath;
                varPath = varPath.Replace("Data Fields", "Geolocation Fields") + "/Longitude";
                outdata = GetTwoDimData(varPath, levelIdx, ref lonUndef);
                minX = maxX = outdata[0, 0];
                for (int i = 0; i < outdata.GetLength(0); i++)
                {
                    for (int j = 0; j < outdata.GetLength(1); j++)
                    {
                        discretedData[0, i * outdata.GetLength(1) + j] = outdata[i, j];
                        if (!Global.MIMath.DoubleEquals(outdata[i, j], lonUndef))
                        {
                            if (minX > outdata[i, j])
                                minX = outdata[i, j];
                            else if (maxX < outdata[i, j])
                                maxX = outdata[i, j];
                        }
                    }
                }

                double minY, maxY;
                varPath = aVar.HDFPath;
                varPath = varPath.Replace("Data Fields", "Geolocation Fields") + "/Latitude";
                outdata = GetTwoDimData(varPath, levelIdx, ref latUndef);
                H5F.close(_fileID);
                minY = maxY = outdata[0, 0];
                for (int i = 0; i < outdata.GetLength(0); i++)
                {
                    for (int j = 0; j < outdata.GetLength(1); j++)
                    {
                        discretedData[1, i * outdata.GetLength(1) + j] = outdata[i, j];
                        if (!Global.MIMath.DoubleEquals(outdata[i, j], latUndef))
                        {
                            if (minY > outdata[i, j])
                                minY = outdata[i, j];
                            else if (maxY < outdata[i, j])
                                maxY = outdata[i, j];
                        }
                    }
                }
                aExtent = new MeteoInfoC.Global.Extent(minX, maxX, minY, maxY);
            }
            else
            {                
                string varPath = aVar.HDFPath + "/" + aVar.Name;
                double[] outdata = GetOneDimData(varPath, levelIdx, ref undef);
                stNum = outdata.Length;
                discretedData = new double[3, stNum];
                for (int i = 0; i < stNum; i++)
                    discretedData[2, i] = outdata[i];

                //Get lon/lat                
                double minX, maxX;
                varPath = aVar.HDFPath;
                varPath = varPath.Replace("Data Fields", "Geolocation Fields") + "/Longitude";
                outdata = GetOneDimData(varPath, levelIdx, ref lonUndef);
                minX = maxX = outdata[0];
                for (int i = 0; i < outdata.Length; i++)
                {
                    discretedData[0, i] = outdata[i];
                    if (!Global.MIMath.DoubleEquals(outdata[i], lonUndef))
                    {
                        if (minX > outdata[i])
                            minX = outdata[i];
                        else if (maxX < outdata[i])
                            maxX = outdata[i];
                    }
                }

                double minY, maxY;
                varPath = aVar.HDFPath;
                varPath = varPath.Replace("Data Fields", "Geolocation Fields") + "/Latitude";
                outdata = GetOneDimData(varPath, levelIdx, ref latUndef);
                H5F.close(_fileID);
                minY = maxY = outdata[0];
                for (int i = 0; i < outdata.Length; i++)
                {
                    discretedData[1, i] = outdata[i];
                    if (!Global.MIMath.DoubleEquals(outdata[i], latUndef))
                    {
                        if (minY > outdata[i])
                            minY = outdata[i];
                        else if (maxY < outdata[i])
                            maxY = outdata[i];
                    }
                }
                aExtent = new MeteoInfoC.Global.Extent(minX, maxX, minY, maxY);
            }

            List<double[]> disDataList = new List<double[]>();
            for (int i = 0; i < stNum; i++)
            {
                if (!Global.MIMath.DoubleEquals(discretedData[0, i], lonUndef) && !Global.MIMath.DoubleEquals(discretedData[1, i], latUndef))
                    disDataList.Add(new double[] { discretedData[0, i], discretedData[1, i], discretedData[2, i] });
            }
            stNum = disDataList.Count;
            discretedData = new double[3, stNum];
            for (int i = 0; i < stNum; i++)
            {
                discretedData[0, i] = disDataList[i][0];
                discretedData[1, i] = disDataList[i][1];
                discretedData[2, i] = disDataList[i][2];
            }

            //Set station data
            stData.Data = discretedData;
            stData.MissingValue = undef;
            stData.DataExtent = aExtent;
            stData.Stations = new List<string>();
            for (int i = 0; i < stNum; i++)
                stData.Stations.Add((i + 1).ToString());          

            return stData;
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

        private double[] GetOneDimData(string varPath, int levelIdx, ref double undef)
        {
            //Open data set group           
            H5DataSetId dsId = H5D.open(_fileID, varPath);
            H5DataSpaceId spaId = H5D.getSpace(dsId);
            H5DataTypeId dtId = H5D.getType(dsId);
            H5T.H5TClass tcls = H5T.getClass(dtId);

            H5DataTypeId dtypeId;
            dtypeId = H5T.copy(H5T.H5Type.NATIVE_FLOAT);

            //Read Attribute
            H5ObjectInfo oinfo = H5O.getInfo(dsId);
            int attNum = (int)oinfo.nAttributes;
            undef = -9999.0;
            double offset = 0.0;
            double scaleFactor = 1.0;
            for (int i = 0; i < attNum; i++)
            {
                H5AttributeId attId = H5A.openIndex(dsId, (uint)i);
                switch (H5A.getName(attId))
                {
                    case "_FillValue":
                        float[] mValue = new float[1];
                        H5A.read(attId, dtypeId, new H5Array<float>(mValue));
                        undef = mValue[0];
                        break;
                    case "Offset":
                        float[] osValue = new float[1];
                        H5A.read(attId, dtypeId, new H5Array<float>(osValue));
                        offset = osValue[0];
                        break;
                    case "ScaleFactor":
                        float[] sfValue = new float[1];
                        H5A.read(attId, dtypeId, new H5Array<float>(sfValue));
                        scaleFactor = sfValue[0];
                        break;
                }

                H5A.close(attId);
            }
            undef = undef * scaleFactor + offset;

            int rank = H5S.getSimpleExtentNDims(spaId);
            var dims = H5S.getSimpleExtentDims(spaId);                        

            float[] outdata = new float[dims[0]];
            switch (rank)
            {
                case 1:
                    outdata = new float[dims[0]];
                    H5D.read(dsId, dtypeId, new H5Array<float>(outdata));
                    break;
                case 2:
                    float[,] outdata2 = new float[dims[0], dims[1]];
                    H5D.read(dsId, dtypeId, new H5Array<float>(outdata2));
                    if (dims[0] > dims[1])
                    {
                        outdata = new float[dims[0]];
                        for (int i = 0; i < (int)dims[0]; i++)
                        {                        
                                outdata[i] = outdata2[i, levelIdx];
                        }
                    }                    
                    else
                    {
                        outdata = new float[dims[1]];
                        for (int i = 0; i < (int)dims[1]; i++)
                        {
                            outdata[i] = outdata2[levelIdx, i];
                        }
                    }
                    break;
            }

            H5D.close(dsId);
            H5S.close(spaId);
            H5T.close(dtId);            

            double[] odata = new double[outdata.Length];
            for (int i = 0; i < outdata.Length; i++)
                odata[i] = outdata[i] * scaleFactor + offset;

            return odata;
        }

        private double[,] GetTwoDimData(string varPath, int levelIdx, ref double undef)
        {
            //Open data set group           
            H5DataSetId dsId = H5D.open(_fileID, varPath);
            H5DataSpaceId spaId = H5D.getSpace(dsId);
            H5DataTypeId dtId = H5D.getType(dsId);
            H5T.H5TClass tcls = H5T.getClass(dtId);

            H5DataTypeId dtypeId;
            dtypeId = H5T.copy(H5T.H5Type.NATIVE_FLOAT);

            //Read Attribute
            H5ObjectInfo oinfo = H5O.getInfo(dsId);
            int attNum = (int)oinfo.nAttributes;
            undef = -9999.0;
            double offset = 0.0;
            double scaleFactor = 1.0;
            for (int i = 0; i < attNum; i++)
            {
                H5AttributeId attId = H5A.openIndex(dsId, (uint)i);
                switch (H5A.getName(attId))
                {
                    case "_FillValue":
                        float[] mValue = new float[1];
                        H5A.read(attId, dtypeId, new H5Array<float>(mValue));
                        undef = mValue[0];
                        break;
                    case "Offset":
                        float[] osValue = new float[1];
                        H5A.read(attId, dtypeId, new H5Array<float>(osValue));
                        offset = osValue[0];
                        break;
                    case "ScaleFactor":
                        float[] sfValue = new float[1];
                        H5A.read(attId, dtypeId, new H5Array<float>(sfValue));
                        scaleFactor = sfValue[0];
                        break;
                }

                H5A.close(attId);
            }
            undef = undef * scaleFactor + offset;

            int rank = H5S.getSimpleExtentNDims(spaId);
            var dims = H5S.getSimpleExtentDims(spaId);

            float[,] outdata = new float[dims[0], dims[1]];
            switch (rank)
            {
                case 2:
                    outdata = new float[dims[0], dims[1]];
                    H5D.read(dsId, dtypeId, new H5Array<float>(outdata));
                    break;
                case 3:
                    float[,,] outdata3 = new float[dims[0], dims[1], dims[3]];
                    H5D.read(dsId, dtypeId, new H5Array<float>(outdata3));
                    outdata = new float[dims[0], dims[1]];
                    for (int i = 0; i < (int)dims[0]; i++)
                    {
                        for (int j = 0; j < (int)dims[1]; j++)
                            outdata[i, j] = outdata3[i, j, levelIdx];
                    }
                    //if (dims[0] > dims[1])
                    //{
                    //    outdata = new float[dims[0]];
                    //    for (int i = 0; i < (int)dims[0]; i++)
                    //    {
                    //        outdata[i] = outdata2[i, levelIdx];
                    //    }
                    //}
                    //else
                    //{
                    //    outdata = new float[dims[1]];
                    //    for (int i = 0; i < (int)dims[1]; i++)
                    //    {
                    //        outdata[i] = outdata2[levelIdx, i];
                    //    }
                    //}
                    break;
            }

            H5D.close(dsId);
            H5S.close(spaId);
            H5T.close(dtId);

            double[,] odata = new double[outdata.GetLength(0), outdata.GetLength(1)];
            for (int i = 0; i < outdata.GetLength(0); i++)
            {
                for (int j = 0; j < outdata.GetLength(1); j++)
                    odata[i, j] = outdata[i, j] * scaleFactor + offset;
            }

            return odata;
        }

        #endregion
    }
}
