using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GeoTiff data info
    /// </summary>
    public class GeoTiffDataInfo : DataInfo,IGridDataInfo
    {
        #region Variables
        private string _byteOrder;
        private List<IFDEntry> _tags = new List<IFDEntry>();

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public GeoTiffDataInfo()
        {

        }

        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Read GeoTiff data info
        /// </summary>
        /// <param name="aFile">file path</param>
        public override void ReadDataInfo(string aFile)
        {
            this.FileName = aFile;
            FileStream sr = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(sr);

            //Image file header (IFH)
            _byteOrder = ASCIIEncoding.ASCII.GetString(br.ReadBytes(2));
            int version = BitConverter.ToUInt16(br.ReadBytes(2), 0);
            long offset = BitConverter.ToUInt32(br.ReadBytes(4), 0);

            //Read IFDs
            ReadIFD(br, offset);


            br.Close();
            sr.Close();

            IFDEntry widthIFD = this.FindTag(Tag.ImageWidth);
            IFDEntry heightIFD = this.FindTag(Tag.ImageLength);
            int width = (int)widthIFD.ValueOffset;
            int height = (int)heightIFD.ValueOffset;

            double[] X = new double[width];
            double[] Y = new double[height];
            IFDEntry modelTiePointTag = FindTag(Tag.ModelTiepointTag);
            IFDEntry modelPixelScaleTag = FindTag(Tag.ModelPixelScaleTag);
            double minLon = modelTiePointTag.ValueD[3];
            double maxLat = modelTiePointTag.ValueD[4];
            double xdelt = modelPixelScaleTag.ValueD[0];
            double ydelt = modelPixelScaleTag.ValueD[1];
            for (int i = 0; i < width; i++)
            {
                X[i] = minLon + xdelt * i;
            }
            for (int i = 0; i < height; i++)
            {
                Y[height - i - 1] = maxLat - ydelt * i;
            }
            Dimension xdim = new Dimension(DimensionType.X);
            xdim.SetValues(X);
            Dimension ydim = new Dimension(DimensionType.Y);
            ydim.SetValues(Y);
            Variable var = new Variable();
            var.Name = "var";
            var.SetDimension(ydim);
            var.SetDimension(xdim);
            List<Variable> vars = new List<Variable>();
            vars.Add(var);
            this.Variables = vars;
        }

        /// <summary>
        /// Generate data info text
        /// </summary>
        /// <returns>Data info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo;
            dataInfo = "File Name: " + FileName;
            //dataInfo += Environment.NewLine + "Data Type: ASCII Grid";
            //dataInfo += Environment.NewLine + "XNum = " + XNum.ToString() +
            //        "  YNum = " + YNum.ToString();
            //dataInfo += Environment.NewLine + "XMin = " + XMin.ToString() +
            //    "  YMin = " + YMin.ToString();
            //dataInfo += Environment.NewLine + "XSize = " + XDelt.ToString() +
            //    "  YSize = " + YDelt.ToString();
            //dataInfo += Environment.NewLine + "MissingValue = " + MissingValue.ToString();

            return dataInfo;
        }

        private void ReadIFD(BinaryReader br, long offset)
        {
            br.BaseStream.Seek(offset, SeekOrigin.Begin);
            int tagNum = BitConverter.ToUInt16(br.ReadBytes(2), 0);
            _tags = new List<IFDEntry>();
            int i;
            bool isGeoTiff = false;
            IFDEntry geoTiffTag = new IFDEntry();
            long position;
            for (i = 0; i < tagNum; i++)
            {
                IFDEntry aIFD = new IFDEntry();
                aIFD.Tag = new Tag(BitConverter.ToUInt16(br.ReadBytes(2), 0));
                aIFD.Type = FieldType.Get(BitConverter.ToUInt16(br.ReadBytes(2), 0));
                aIFD.Length = BitConverter.ToInt32(br.ReadBytes(4), 0);
                byte[] bytes = br.ReadBytes(4);                
                aIFD.ValueOffset = BitConverter.ToUInt32(bytes, 0);
                position = br.BaseStream.Position;

                if (aIFD.Length * aIFD.Type.Size <= 4)
                {
                    br.BaseStream.Seek(-4, SeekOrigin.Current);
                    readValues(br, aIFD);
                }
                else
                {
                    br.BaseStream.Seek(aIFD.ValueOffset, SeekOrigin.Begin);
                    readValues(br, aIFD);
                }
                br.BaseStream.Position = position;

                _tags.Add(aIFD);

                if (!isGeoTiff)
                {
                    if (aIFD.Tag.Code == 34735)
                    {
                        isGeoTiff = true;
                        geoTiffTag = aIFD;
                    }
                }
            }
            offset = BitConverter.ToUInt16(br.ReadBytes(2), 0);            

            if (isGeoTiff)
            {
                offset = geoTiffTag.ValueOffset;
                br.BaseStream.Seek(offset, SeekOrigin.Begin);
                GeoHeader geoHeader = new GeoHeader();
                geoHeader.KeyDirectoryVersion = BitConverter.ToUInt16(br.ReadBytes(2), 0);
                geoHeader.KeyRevision = BitConverter.ToUInt16(br.ReadBytes(2), 0);
                geoHeader.MinorRevision = BitConverter.ToUInt16(br.ReadBytes(2), 0);
                geoHeader.NumberOfKeys = BitConverter.ToUInt16(br.ReadBytes(2), 0);
                List<KeyEntry> keyEntries = new List<KeyEntry>();
                for (i = 0; i < geoHeader.NumberOfKeys; i++)
                {
                    KeyEntry keyEntry = new KeyEntry();
                    keyEntry.KeyID = BitConverter.ToUInt16(br.ReadBytes(2), 0);
                    keyEntry.TIFFTagLocation = BitConverter.ToUInt16(br.ReadBytes(2), 0);
                    keyEntry.Count = BitConverter.ToUInt16(br.ReadBytes(2), 0);
                    keyEntry.Value_Offset = BitConverter.ToUInt16(br.ReadBytes(2), 0);
                    keyEntries.Add(keyEntry);
                }
            }
        }

        private void readValues(BinaryReader br, IFDEntry ifd)
        {
            if (ifd.Type == FieldType.ASCII)
            {
                ifd.ValueS = readSValue(br, ifd);
            }
            else if (ifd.Type == FieldType.RATIONAL)
            {
                ifd.Value = new int[ifd.Length * 2];
                for (int i = 0; i < ifd.Length * 2; i++)
                {
                    ifd.Value[i] = readIntValue(br, ifd);
                }
            }
            else if (ifd.Type == FieldType.FLOAT)
            {
                ifd.ValueD = new double[ifd.Length];
                for (int i = 0; i < ifd.Length; i++)
                {
                    ifd.ValueD[i] = br.ReadSingle();
                }
            }
            else if (ifd.Type == FieldType.DOUBLE)
            {
                ifd.ValueD = new double[ifd.Length];
                for (int i = 0; i < ifd.Length; i++)
                {
                    ifd.ValueD[i] = br.ReadDouble();
                }
            }
            else
            {
                ifd.Value = new int[ifd.Length];
                for (int i = 0; i < ifd.Length; i++)
                {
                    ifd.Value[i] = readIntValue(br, ifd);
                }
            }
        }

        private String readSValue(BinaryReader br, IFDEntry ifd)
        {
            byte[] dst = br.ReadBytes(ifd.Length);
            return ASCIIEncoding.ASCII.GetString(dst);
        }

        private int readIntValue(BinaryReader br, IFDEntry ifd)
        {
            switch (ifd.Type.Code)
            {
                case 1:
                case 2:
                    return br.ReadByte();
                case 3:
                    return br.ReadUInt16();
                case 4:
                case 5:
                    return br.ReadInt32();
            }
            return 0;
        }

        private IFDEntry FindTag(Tag tag)
        {
            if (tag == null)
                return null;

            foreach (IFDEntry ifd in _tags)
            {
                if (ifd.Tag.Code == tag.Code)
                    return ifd;
            }

            return null;
        }

        private double[] ReadData(BinaryReader br, int width, int height)
        {
            double[] values = new double[width * height];
            IFDEntry tileOffsetTag = FindTag(Tag.TileOffsets);
            if (tileOffsetTag != null)
            {
                int tileOffset = tileOffsetTag.Value[0];
                IFDEntry tileSizeTag = FindTag(Tag.TileByteCounts);
                int tileSize = tileSizeTag.Value[0];
            }
            else
            {
                IFDEntry stripOffsetTag = FindTag(Tag.StripOffsets);
                if (stripOffsetTag != null)
                {
                    int stripNum = stripOffsetTag.Length;
                    int stripOffset;
                    IFDEntry stripSizeTag = FindTag(Tag.StripByteCounts);
                    int stripSize = stripSizeTag.Value[0];
                    IFDEntry rowsPerStripTag = FindTag(Tag.RowsPerStrip);
                    int rowNum = rowsPerStripTag.Value[0];
                    int n = 0;
                    for (int i = 0; i < stripNum; i++)
                    {
                        stripOffset = stripOffsetTag.Value[i];
                        br.BaseStream.Seek(stripOffset, SeekOrigin.Begin);
                        for (int j = 0; j < width * rowNum; j++)
                        {
                            values[n] = br.ReadUInt16();
                            n += 1;
                        }
                    }
                }
            }

            return values;
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
            FileStream sr = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(sr);

            Variable var = this.Variables[varIdx];
            int width = var.XDimension.DimLength;
            int height = var.YDimension.DimLength;

            double[] values1d = ReadData(br, width, height);
            double[,] values = new double[height, width];
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    values[height - i - 1, j] = values1d[i * width + j];
                }
            }

            GridData gData = new GridData();
            gData.Data = values;
            gData.X = var.XDimension.GetValues();
            gData.Y = var.YDimension.GetValues();

            br.Close();
            sr.Close();

            return gData;
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
