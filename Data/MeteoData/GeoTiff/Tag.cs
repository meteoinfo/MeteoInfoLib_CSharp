using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Tag
    /// </summary>
    class Tag
    {
        #region Variables
        private string _name;
        private int _code;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="code">code</param>
        public Tag(string name, int code)
        {
            _name = name;
            _code = code;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">code</param>
        public Tag(int code)
        {
            _code = code;
            _name = GetName(code);
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Get or set code
        /// </summary>
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }

        #endregion

        #region Methods
        private string GetName(int code)
        {
            string name = String.Empty;
            switch (code)
            {
                case 254:
                    name = "NewSubfileType";
                    break;
                case 256:
                    name = "ImageWidth";
                    break;
                case 257:
                    name = "ImageHeight";
                    break;
                case 258:
                    name = "BitsPerSample";
                    break;
                case 259:
                    name = "Compression";
                    break;
                case 262:
                    name = "PhotometricInterpretation";
                    break;
                case 266:
                    name = "FillOrder";
                    break ;
                case 269:
                    name = "DocumentName";
                    break;
                case 270:
                    name = "ImageDescription";
                    break;
                case 273:
                    name = "StripOffsets";
                    break;
                case 274:
                    name = "Orientation";
                    break;
                case 277:
                    name = "SamplesPerPixel";
                    break;
                case 278:
                    name = "RowsPerStrip";
                    break;
                case 279:
                    name = "StripByteCounts";
                    break;
                case 282:
                    name = "XResolution";
                    break;
                case 283:
                    name = "YResolution";
                    break;
                case 284:
                    name = "PlanarConfiguration";
                    break;
                case 296:
                    name = "ResolutionUnit";
                    break;
                case 297:
                    name = "PageNumber";
                    break;
                case 305:
                    name = "Software";
                    break;
                case 320:
                    name = "ColorMap";
                    break;
                case 322:
                    name = "TileWidth";
                    break;
                case 323:
                    name = "TileHeight";
                    break;
                case 324:
                    name = "TileOffset";
                    break;
                case 325:
                    name = "TileByteCounts";
                    break;
                case 339:
                    name = "SampleFormat";
                    break;
                case 340:
                    name = "SMinSampleValue";
                    break;
                case 341:
                    name = "SMaxSampleValue";
                    break;
                case 33550:
                    name = "ModelPixelScaleTag";
                    break;
                case 33920:
                    name = "IntergraphMatrixTag";
                    break;
                case 33922:
                    name = "ModelTiepointTag";
                    break;
                case 34264:
                    name = "ModelTransformationTag";
                    break;
                case 34735:
                    name = "GeoKeyDirectoryTag";
                    break;
                case 34736:
                    name = "GeoDoubleParamsTag";
                    break;
                case 34737:
                    name = "GeoAsciiParamsTag";
                    break;
                case 42113:
                    name = "GDALNoDataTag";
                    break;
            }

            return name;
        }

        #endregion

        #region Tag List
        // tiff tags used for geotiff
        static public Tag ModelPixelScaleTag = new Tag("ModelPixelScaleTag", 33550);
        static public Tag IntergraphMatrixTag = new Tag("IntergraphMatrixTag", 33920);
        static public Tag ModelTiepointTag = new Tag("ModelTiepointTag", 33922);
        static public Tag ModelTransformationTag = new Tag("ModelTransformationTag", 34264);
        static public Tag GeoKeyDirectoryTag = new Tag("GeoKeyDirectoryTag", 34735);
        static public Tag GeoDoubleParamsTag = new Tag("GeoDoubleParamsTag", 34736);
        static public Tag GeoAsciiParamsTag = new Tag("GeoAsciiParamsTag", 34737);

        // Gdal tiff tags
        static public Tag GDALNoData = new Tag("GDALNoDataTag", 42113);

        public static Tag NewSubfileType = new Tag("NewSubfileType", 254);
        static public Tag ImageWidth = new Tag("ImageWidth", 256);
        static public Tag ImageLength = new Tag("ImageLength", 257);
        static public Tag BitsPerSample = new Tag("BitsPerSample", 258);
        static public Tag Compression = new Tag("Compression", 259);
        static public Tag PhotometricInterpretation = new Tag("PhotometricInterpretation", 262);
        static public Tag FillOrder = new Tag("FillOrder", 266);
        static public Tag DocumentName = new Tag("DocumentName", 269);
        static public Tag ImageDescription = new Tag("ImageDescription", 270);
        static public Tag StripOffsets = new Tag("StripOffsets", 273);
        static public Tag Orientation = new Tag("Orientation", 274);
        static public Tag SamplesPerPixel = new Tag("SamplesPerPixel", 277);
        static public Tag RowsPerStrip = new Tag("RowsPerStrip", 278);
        static public Tag StripByteCounts = new Tag("StripByteCounts", 279);
        static public Tag XResolution = new Tag("XResolution", 282);
        static public Tag YResolution = new Tag("YResolution", 283);
        static public Tag PlanarConfiguration = new Tag("PlanarConfiguration", 284);
        static public Tag ResolutionUnit = new Tag("ResolutionUnit", 296);
        static public Tag PageNumber = new Tag("PageNumber", 297);
        static public Tag Software = new Tag("Software", 305);
        static public Tag ColorMap = new Tag("ColorMap", 320);
        static public Tag TileWidth = new Tag("TileWidth", 322);
        static public Tag TileLength = new Tag("TileLength", 323);
        static public Tag TileOffsets = new Tag("TileOffsets", 324);
        static public Tag TileByteCounts = new Tag("TileByteCounts", 325);
        static public Tag SampleFormat = new Tag("SampleFormat", 339);
        static public Tag SMinSampleValue = new Tag("SMinSampleValue", 340);
        static public Tag SMaxSampleValue = new Tag("SMaxSampleValue", 341);

        
        #endregion
    }
}
