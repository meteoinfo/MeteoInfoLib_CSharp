using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Projections;
using MeteoInfoC.Global;
using MeteoInfoC.Layer;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// LRIT/HRIT data info - for meteorological satellite
    /// </summary>
    public class HRITDataInfo : DataInfo,IGridDataInfo
    {
        #region Stucts
        struct PrimaryHeader
        {
            /// <summary>
            /// Header type
            /// </summary>
            public int Header_Type;
            /// <summary>
            /// Header record length
            /// </summary>
            public int Header_Record_Length;
            /// <summary>
            /// File type code
            /// </summary>
            public int File_Type_Code;
            /// <summary>
            /// Total header length
            /// </summary>
            public int Total_Header_Length;
            /// <summary>
            /// Data field length
            /// </summary>
            public int Data_Field_Length;
        }

        struct ImageStructure
        {
            /// <summary>
            /// Header type
            /// </summary>
            public int Header_Type;
            /// <summary>
            /// Header record length
            /// </summary>
            public int Header_Record_Length;
            /// <summary>
            /// Number of bits per pixel - 16: for image data; 1: for overlay data
            /// </summary>
            public int NB;
            /// <summary>
            /// Number of columns
            /// </summary>
            public int NC;
            /// <summary>
            /// Number of lines
            /// </summary>
            public int NL;
            /// <summary>
            /// Compression flag - 0: no compression; 1: lossless compression; 2: lossy compression
            /// </summary>
            public int Compression_Flag;
        }

        struct ImageNavigation
        {
            /// <summary>
            /// Header type
            /// </summary>
            public int Header_Type;
            /// <summary>
            /// Header record length
            /// </summary>
            public int Header_Record_Length;
            /// <summary>
            /// Projection name
            /// </summary>
            public string Projection_Name;
            /// <summary>
            /// Column scaling factor
            /// </summary>
            public int CFAC;
            /// <summary>
            /// Line scaling factor
            /// </summary>
            public int LFAC;
            /// <summary>
            /// Column offset
            /// </summary>
            public int COFF;
            /// <summary>
            /// Line offset
            /// </summary>
            public int LOFF;
        }

        struct ImageDataFunction
        {
            /// <summary>
            /// Header type
            /// </summary>
            public int Header_Type;
            /// <summary>
            /// Header record length
            /// </summary>
            public int Header_Record_Length;
            /// <summary>
            /// Data definition block
            /// </summary>
            public string Data_Definition_Block;
        }

        struct Annotation
        {
            /// <summary>
            /// Header type
            /// </summary>
            public int Header_Type;
            /// <summary>
            /// Header record length
            /// </summary>
            public int Header_Record_Length;
            /// <summary>
            /// Annotation text
            /// </summary>
            public string Annotation_Text;
        }

        struct TimeStamp
        {
            /// <summary>
            /// Header type
            /// </summary>
            public int Header_Type;
            /// <summary>
            /// Header record length
            /// </summary>
            public int Header_Record_Length;
            /// <summary>
            /// P-Field fixed vlue according to CCSDS
            /// </summary>
            public int CDS_P_Field;
            /// <summary>
            /// T-Field according to CCSDS
            /// </summary>
            public int CDS_T_Field;
        }

        struct ImageSegmentIdentification
        {
            /// <summary>
            /// Header type
            /// </summary>
            public int Header_Type;
            /// <summary>
            /// Header record length
            /// </summary>
            public int Header_Record_Length;
            /// <summary>
            /// Image segment sequence number
            /// </summary>
            public int Image_Segm_Seq_No;
            /// <summary>
            /// Total number of image segments
            /// </summary>
            public int Total_No_Image_Segm;
            /// <summary>
            /// Line number of the image segment
            /// </summary>
            public int Line_No_Image_Segm;
        }

        struct ImageCompensationInformation
        {
            /// <summary>
            /// Header type
            /// </summary>
            public int Header_Type;
            /// <summary>
            /// Header record length
            /// </summary>
            public int Header_Record_Length;
            /// <summary>
            /// Image compensation information
            /// </summary>
            public string Image_Compensation_Information;
        }

        struct ImageObservationTime
        {
            /// <summary>
            /// Header type
            /// </summary>
            public int Header_Type;
            /// <summary>
            /// Header record length
            /// </summary>
            public int Header_Record_Length;
            /// <summary>
            /// Image observation time
            /// </summary>
            public string Image_Observation_Time;
        }

        struct ImageQualityInformation
        {
            /// <summary>
            /// Header type
            /// </summary>
            public int Header_Type;
            /// <summary>
            /// Header record length
            /// </summary>
            public int Header_Record_Length;
            /// <summary>
            /// Image quality information
            /// </summary>
            public string Image_Quality_Information;
        }

        #endregion

        #region Variables
        private PrimaryHeader _primaryHeader;
        private ImageStructure _imageStructure;
        private ImageNavigation _imageNavigation;
        private ImageDataFunction _imageDataFunction;
        private Annotation _annotation;
        private TimeStamp _timeStamp;
        private ImageSegmentIdentification _imageSegmentID;
        private ImageCompensationInformation _imageCompInfo;
        private ImageObservationTime _imageObsTime;
        private ImageQualityInformation _imageQualityInfo;
        private int _dataLength;

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
        public List<string> varList;
        /// <summary>
        /// Field list
        /// </summary>
        public List<string> FieldList;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public HRITDataInfo()
        {

        }

        #endregion

        #region Propertis

        /// <summary>
        /// Get x number
        /// </summary>
        public int XNum
        {
            get { return _imageStructure.NC;}
        }

        /// <summary>
        /// Get y number
        /// </summary>
        public int YNum
        {
            get
            {
                return _imageStructure.NL;
            }
        }

        /// <summary>
        /// Get product type
        /// </summary>
        public int ProductType
        {
            get { return _primaryHeader.File_Type_Code; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read data info
        /// </summary>
        /// <param name="aFile">file path</param>
        public override void ReadDataInfo(string aFile)
        {
            this.FileName = aFile;

            FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            int headerType;
            int headerRecordLength;
            //Read primary header
            _primaryHeader.Header_Type = br.ReadByte();
            _primaryHeader.Header_Record_Length = Bytes2Number.Uint2(br);
            _primaryHeader.File_Type_Code = br.ReadByte();
            if (_primaryHeader.File_Type_Code != 0)
                return;

            _primaryHeader.Total_Header_Length = Bytes2Number.UInt(br.ReadBytes(4)); 
            _primaryHeader.Data_Field_Length = Bytes2Number.UInt(br.ReadBytes(8));
            _dataLength = (int)(br.BaseStream.Length - br.BaseStream.Position);

            //Read secondary headers
            while (true)
            {
                headerType = br.ReadByte();
                if (br.BaseStream.Position > _primaryHeader.Total_Header_Length)
                    break;

                if (headerType > 132)
                    break;

                switch (headerType)
                {
                    case 1:
                        //Read header type#1 - Image Structure
                        _imageStructure.Header_Type = headerType;
                        _imageStructure.Header_Record_Length = Bytes2Number.UInt(br.ReadBytes(2));
                        _imageStructure.NB = br.ReadByte();
                        _imageStructure.NC = Bytes2Number.UInt(br.ReadBytes(2));
                        _imageStructure.NL = Bytes2Number.UInt(br.ReadBytes(2));
                        _imageStructure.Compression_Flag = br.ReadByte();
                        break;
                    case 2:
                        //Read header type#2 - Image Navigation
                        _imageNavigation.Header_Type = headerType;
                        _imageNavigation.Header_Record_Length = Bytes2Number.UInt(br.ReadBytes(2));
                        _imageNavigation.Projection_Name = ASCIIEncoding.ASCII.GetString(br.ReadBytes(32)).Trim();
                        _imageNavigation.CFAC = Bytes2Number.Int4(br);
                        _imageNavigation.LFAC = Bytes2Number.Int4(br);
                        _imageNavigation.COFF = Bytes2Number.Int4(br);
                        _imageNavigation.LOFF = Bytes2Number.Int4(br);
                        //_imageNavigation.CFAC = BitConverter.ToInt32(br.ReadBytes(4), 0);
                        //_imageNavigation.LFAC = BitConverter.ToInt32(br.ReadBytes(4), 0);
                        //_imageNavigation.COFF = BitConverter.ToInt32(br.ReadBytes(4), 0);
                        //_imageNavigation.LOFF = BitConverter.ToInt32(br.ReadBytes(4), 0);
                        break;
                    case 3:
                        //Read header type#3 - Image Data Function
                        _imageDataFunction.Header_Type = headerType;
                        _imageDataFunction.Header_Record_Length = Bytes2Number.UInt(br.ReadBytes(2));
                        _imageDataFunction.Data_Definition_Block = ASCIIEncoding.ASCII.GetString(br.ReadBytes(_imageDataFunction.Header_Record_Length - 3));
                        break;
                    case 4:
                        //Read header type#4 - Annotation
                        _annotation.Header_Type = headerType;
                        _annotation.Header_Record_Length = Bytes2Number.UInt(br.ReadBytes(2));
                        _annotation.Annotation_Text = ASCIIEncoding.ASCII.GetString(br.ReadBytes(_annotation.Header_Record_Length - 3));
                        break;
                    case 5:
                        //Read header type#5 - Time Stamp
                        _timeStamp.Header_Type = headerType;
                        _timeStamp.Header_Record_Length = Bytes2Number.UInt(br.ReadBytes(2));
                        _timeStamp.CDS_P_Field = br.ReadByte();
                        _timeStamp.CDS_T_Field = Bytes2Number.UInt(br.ReadBytes(6));
                        break;
                    case 128:
                        //Read header type#128 - Image Segment Identification
                        _imageSegmentID.Header_Type = headerType;
                        _imageSegmentID.Header_Record_Length = Bytes2Number.UInt(br.ReadBytes(2));
                        _imageSegmentID.Image_Segm_Seq_No = br.ReadByte();
                        _imageSegmentID.Total_No_Image_Segm = br.ReadByte();
                        _imageSegmentID.Line_No_Image_Segm = Bytes2Number.UInt(br.ReadBytes(2));
                        break;
                    case 130:    //Read header type#130 - Image compensation information header
                        _imageCompInfo.Header_Type = headerType;
                        _imageCompInfo.Header_Record_Length = Bytes2Number.UInt(br.ReadBytes(2));
                        _imageCompInfo.Image_Compensation_Information = ASCIIEncoding.ASCII.GetString(br.ReadBytes(_imageCompInfo.Header_Record_Length - 3));
                        break;
                    case 131:    //Read header type#131 - Image observation time
                        _imageObsTime.Header_Type = headerType;
                        _imageObsTime.Header_Record_Length = Bytes2Number.UInt(br.ReadBytes(2));
                        _imageObsTime.Image_Observation_Time = ASCIIEncoding.ASCII.GetString(br.ReadBytes(_imageObsTime.Header_Record_Length - 3));
                        break;
                    case 132:    //Read header type#132 - Image quality information
                        _imageQualityInfo.Header_Type = headerType;
                        _imageQualityInfo.Header_Record_Length = Bytes2Number.UInt(br.ReadBytes(2));
                        _imageQualityInfo.Image_Quality_Information = ASCIIEncoding.ASCII.GetString(br.ReadBytes(_imageQualityInfo.Header_Record_Length - 3));
                        break;
                    default:
                        headerRecordLength = Bytes2Number.UInt(br.ReadBytes(2));
                        br.ReadBytes(headerRecordLength - 3);
                        break;
                }
            }
                                  
            //Close file
            br.Close();
            fs.Close();  

            //Get projection
            GetProjection(_imageNavigation.Projection_Name);

            int tNL = _imageStructure.NL * _imageSegmentID.Total_No_Image_Segm;
            double[] x = new double[XNum];
            double[] y = new double[tNL];
            double cof = Math.Pow(2, -16);
            double CFAC_d = _imageNavigation.CFAC * cof;
            double LFAC_d = _imageNavigation.LFAC * cof;
            double radius = 6378137.0;
            int i;
            for (i = 0; i < XNum; i++)
            {
                x[i] = (double)(i + 1 - _imageNavigation.COFF) / CFAC_d * radius / 10;
            }

            for (i = 0; i < tNL; i++)
            {
                y[i] = (double)(i + 1 - _imageNavigation.LOFF) / LFAC_d * radius / 10;
            }

            Dimension xdim = new Dimension(DimensionType.X);
            xdim.SetValues(x);
            this.XDimension = xdim;
            Dimension ydim = new Dimension(DimensionType.Y);
            ydim.SetValues(y);
            this.YDimension = ydim;
          
            //Set variable list
            varList = new List<string>();
            varList.Add("var");
            Variable var = new Variable();
            var.Name = "var";
            var.SetDimension(ydim);
            var.SetDimension(xdim);
            List<Variable> variables = new List<Variable>();
            variables.Add(var);
            this.Variables = variables;
        }

        private void GetProjection(string projNameStr)
        {
            if (projNameStr.Substring(0, 4) == "GEOS")
            {
                string lonStr = projNameStr.Split('(')[1].TrimEnd(')');
                string projStr = "+proj=geos +lon_0=" + lonStr + " +h=35785831";
                this.ProjectionInfo = new ProjectionInfo(projStr);
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
            dataInfo += Environment.NewLine + "Product Type: " + ProductType.ToString();
            dataInfo += Environment.NewLine + "Start Time: " + STime.ToString("yyyy-MM-dd HH:mm");
            dataInfo += Environment.NewLine + "End Time: " + ETime.ToString("yyyy-MM-dd HH:mm");

            return dataInfo;
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
            FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            //Read byte data 
            br.BaseStream.Seek(_primaryHeader.Total_Header_Length, SeekOrigin.Begin);
            int length = (int)(br.BaseStream.Length - br.BaseStream.Position);
            byte[] imageBytes = br.ReadBytes(length);

            br.Close();
            fs.Close();

            //Get grid data
            int i, j;
            int tNL = _imageStructure.NL * _imageSegmentID.Total_No_Image_Segm;
            GridData gridData = new GridData();
            double[] x = new double[XNum];
            double[] y = new double[YNum];
            double cof = Math.Pow(2, -16);
            double CFAC_d = _imageNavigation.CFAC * cof;
            double LFAC_d = _imageNavigation.LFAC * cof;
            double radius = 6378137.0;
            for (i = 0; i < XNum; i++)
            {
                x[i] = (double)(i + 1 - _imageNavigation.COFF) / CFAC_d * radius / 10;
            }

            int stNL = tNL - _imageSegmentID.Line_No_Image_Segm + 1 - YNum;
            for (i = 0; i < YNum; i++)
            {
                y[i] = (double)(stNL + i - _imageNavigation.LOFF) / LFAC_d * radius / 10;
            }
            gridData.X = x;
            gridData.Y = y;

            double[,] gData = new double[YNum, XNum];
            for (i = 0; i < YNum; i++)
            {
                for (j = 0; j < XNum; j++)
                {
                    gData[YNum - i - 1, j] = Bytes2Number.Uint2(imageBytes[i * XNum * 2 + j * 2], imageBytes[1 * XNum * 2 + j * 2 + 1]);
                }
            }
            gridData.Data = gData;

            return gridData;
        }

        /// <summary>
        /// Get all grid data
        /// </summary>
        /// <returns>grid data</returns>
        public GridData GetGridData_All()
        {
            int i, j;
            int tNL = _imageStructure.NL * _imageSegmentID.Total_No_Image_Segm;
            GridData gridData = new GridData();
            double[] x = new double[XNum];
            double[] y = new double[tNL];
            double cof = Math.Pow(2, -16);
            double CFAC_d = _imageNavigation.CFAC * cof;
            double LFAC_d = _imageNavigation.LFAC * cof;
            double radius = 6378137.0;
            for (i = 0; i < XNum; i++)
            {
                x[i] = (double)(i + 1 - _imageNavigation.COFF) / CFAC_d * radius / 10;
            }

            for (i = 0; i < tNL; i++)
            {
                y[i] = (double)(i + 1 - _imageNavigation.LOFF) / LFAC_d * radius / 10;
            }
            gridData.X = x;
            gridData.Y = y;
            double[,] gData = new double[tNL, XNum];

            //Get file list
            string baseFN = this.FileName.Substring(0, this.FileName.Length - 3);
            for (int f = 0; f < _imageSegmentID.Total_No_Image_Segm; f++)
            {
                string fileName = baseFN + (f + 1).ToString("000");
                int stNL = tNL - YNum * (f + 1);

                if (File.Exists(fileName))
                {
                    HRITDataInfo aDataInfo = new HRITDataInfo();
                    aDataInfo.ReadDataInfo(fileName);
                    GridData aGridData = aDataInfo.GetGridData_LonLat(0, 0, 0);

                    for (i = 0; i < YNum; i++)
                    {
                        for (j = 0; j < XNum; j++)
                        {
                            gData[stNL + i, j] = aGridData.Data[i, j];
                        }
                    }
                }
                else
                {
                    for (i = 0; i < YNum; i++)
                    {
                        for (j = 0; j < XNum; j++)
                        {
                            gData[stNL + i, j] = this.MissingValue;
                        }
                    }
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

        #endregion
    }
}
