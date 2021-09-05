using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using MeteoInfoC.Global;
using MeteoInfoC.Shape;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// Image layer
    /// </summary>
    public class ImageLayer:MapLayer
    {
        #region Variables
        private Image _Image;        
        private WorldFilePara _worldFilePara;       
        private string _worldFileName;
        //private int _TransparencyPerc;
        private bool _isSetTransColor;
        private Color _TransparencyColor;
       
        #endregion               

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ImageLayer()
        {
            LayerType = LayerTypes.ImageLayer;
            ShapeType = ShapeTypes.Image;
            //m_handle = -1;
            //m_layerName = "";
            //m_fileName = "";
            //m_visible = true;
            //m_IsMaskout = false;
            _isSetTransColor = false;
            _TransparencyColor = Color.Black;
        }
        #endregion

        #region Properties     
        /// <summary>
        /// Get or set image
        /// </summary>
        public Image Image
        {
            get { return _Image; }
            set 
            { 
                _Image = value;
                _TransparencyColor = ((Bitmap)_Image).GetPixel(1, 1);
            }
        }        

        /// <summary>
        /// Get or set world file name of the layer
        /// </summary>
        [ReadOnly(true)]
        [CategoryAttribute("Layer Property"), DescriptionAttribute("World file name of the layer")]
        public string WorldFileName
        {
            get { return _worldFileName; }
            set { _worldFileName = value; }
        }

        /// <summary>
        /// Get or set world file parameters
        /// </summary>
        public WorldFilePara WorldFileParaV
        {
            get { return _worldFilePara; }
            set { _worldFilePara = value; }
        }

        /// <summary>
        /// Override transparency percent property
        /// </summary>
        public override int TransparencyPerc
        {
            get { return base.TransparencyPerc; }
            set
            {
                base.TransparencyPerc = value;
                //UpdateTransparency();
            }
        }

        /// <summary>
        /// Get or set if set transparency color
        /// </summary>
        public bool SetTransColor
        {
            get { return _isSetTransColor; }
            set
            { 
                _isSetTransColor = value;
                if (_isSetTransColor)
                    ((Bitmap)_Image).MakeTransparent(_TransparencyColor);

                //if (_isSetTransColor)
                //    SetTransparencyColor(_TransparencyColor);
                //else
                //    UpdateTransparency();
            }
        }

        /// <summary>
        /// Get or set transparency color
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer transparency color")]
        [ReadOnly(true)]
        public Color TransparencyColor
        {
            get { return _TransparencyColor; }
            set 
            {
                _TransparencyColor = value;
                if (_isSetTransColor)
                {
                    ((Bitmap)_Image).MakeTransparent(_TransparencyColor);
                    //((Bitmap)_Image).MakeTransparent();
                    //for (int i = 0; i < _Image.Width; i++)
                    //{
                    //    for (int j = 0; j < _Image.Height; j++)
                    //    {
                    //        if (((Bitmap)_Image).GetPixel(i, j) == _TransparencyColor)
                    //            ((Bitmap)_Image).SetPixel(i, j, Color.FromArgb(0, _TransparencyColor));
                    //    }
                    //}
                }
                //if (_isSetTransColor)
                //{
                //    UpdateTransparency();
                //    SetTransparencyColor(_TransparencyColor);
                //}
            }
        }

        /// <summary>
        /// Get or set if set transparency color
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("If set layer transparency color")]
        public bool IsSetTransColor
        {
            get { return _isSetTransColor; }
            set
            {
                _isSetTransColor = value;

                // Create a PropertyDescriptor for "SpouseName" by calling the static GetProperties on TypeDescriptor.
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())["TransparencyColor"];

                // Fetch the ReadOnlyAttribute from the descriptor. 
                ReadOnlyAttribute attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];

                // Get the internal isReadOnly field from the ReadOnlyAttribute using reflection.  
                FieldInfo isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);

                // Using Reflection, set the internal isReadOnly field.     
                isReadOnly.SetValue(attrib, !value);
            }
        }

        /// <summary>
        /// Get or set X upper-left
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set image X coordinate of the center of the upper-left pixel")]
        public double XUL
        {
            get
            {
                return _worldFilePara.XUL;
            }
            set
            {
                _worldFilePara.XUL = value;
                Extent aExtent = this.Extent;
                aExtent.minX = _worldFilePara.XUL;
                aExtent.maxX = _worldFilePara.XUL + this.Extent.Width;
                this.Extent = aExtent;
                if (File.Exists(_worldFileName))
                    WriteImageWorldFile(_worldFileName, _worldFilePara);
            }
        }

        /// <summary>
        /// Get or set y upper-left
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set image y coordinate of the center of the upper-left pixel")]
        public double YUL
        {
            get
            {
                return _worldFilePara.YUL;
            }
            set
            {
                _worldFilePara.YUL = value;
                Extent aExtent = this.Extent;
                aExtent.maxY = _worldFilePara.YUL;
                aExtent.minY = _worldFilePara.YUL - this.Extent.Height;
                this.Extent = aExtent;
                if (File.Exists(_worldFileName))
                    WriteImageWorldFile(_worldFileName, _worldFilePara);
            }
        }

        /// <summary>
        /// Get or set X scale
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set dimension of a pixel in map units in X direction")]
        public double XScale
        {
            get
            {
                return _worldFilePara.XScale;
            }
            set
            {
                _worldFilePara.XScale = value;
                Extent aExtent = this.Extent;
                double width = _Image.Width * _worldFilePara.XScale;
                aExtent.maxX = _worldFilePara.XUL + width;
                this.Extent = aExtent;
                if (File.Exists(_worldFileName))
                    WriteImageWorldFile(_worldFileName, _worldFilePara);
            }
        }

        /// <summary>
        /// Get or set y scale
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set dimension of a pixel in map units in y direction")]
        public double YScale
        {
            get
            {
                return _worldFilePara.YScale;
            }
            set
            {
                _worldFilePara.YScale = value;
                Extent aExtent = this.Extent;
                double height = _Image.Height * _worldFilePara.YScale;
                aExtent.minY = _worldFilePara.YUL + height;
                this.Extent = aExtent;
                if (File.Exists(_worldFileName))
                    WriteImageWorldFile(_worldFileName, _worldFilePara);                
            }
        }

        /// <summary>
        /// Get or set x rotate
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set rotation/shear of the image in x direction")]
        public double XRotate
        {
            get { return _worldFilePara.XRotate; }
            set
            {
                _worldFilePara.XRotate = value;
                if (File.Exists(_worldFileName))
                    WriteImageWorldFile(_worldFileName, _worldFilePara);
            }
        }

        /// <summary>
        /// Get or set y rotate
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set rotation/shear of the image in y direction")]
        public double YRotate
        {
            get { return _worldFilePara.YRotate; }
            set
            {
                _worldFilePara.YRotate = value;
                if (File.Exists(_worldFileName))
                    WriteImageWorldFile(_worldFileName, _worldFilePara);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Read image world file
        /// </summary>
        /// <param name="aIFile"></param>        
        public void ReadImageWorldFile(string aIFile)
        {
            StreamReader sr = new StreamReader(aIFile);           
            
            _worldFilePara.XScale = Convert.ToDouble(sr.ReadLine());
            _worldFilePara.YRotate = Convert.ToDouble(sr.ReadLine());
            _worldFilePara.XRotate = Convert.ToDouble(sr.ReadLine());
            _worldFilePara.YScale = Convert.ToDouble(sr.ReadLine());
            _worldFilePara.XUL = Convert.ToDouble(sr.ReadLine());
            _worldFilePara.YUL = Convert.ToDouble(sr.ReadLine());
            sr.Close();
        }

        /// <summary>
        /// Write image world file
        /// </summary>
        /// <param name="aFile"></param>
        /// <param name="aWFP"></param>
        public void WriteImageWorldFile(string aFile, WorldFilePara aWFP)
        {
            StreamWriter sw = new StreamWriter(aFile);
            sw.WriteLine(aWFP.XScale.ToString());
            sw.WriteLine(aWFP.YRotate.ToString());
            sw.WriteLine(aWFP.XRotate.ToString());
            sw.WriteLine(aWFP.YScale.ToString());
            sw.WriteLine(aWFP.XUL.ToString());
            sw.WriteLine(aWFP.YUL.ToString());
            sw.Close();
        }

        /// <summary>
        /// Set color palette to a image from a palette file
        /// </summary>
        /// <param name="aFile">file path</param>        
        public void SetPalette(string aFile)
        {
            List<Color> colors = GetColorsFromPaletteFile(aFile);

            SetPalette(colors);
        }

        private List<Color> GetColorsFromPaletteFile(string pFile)
        {
            StreamReader sr = new StreamReader(pFile, Encoding.Default);
            sr.ReadLine();
            string aLine = sr.ReadLine();
            string[] dataArray;
            List<Color> colors = new List<Color>();
            while (aLine != null)
            {
                if (aLine == String.Empty)
                {
                    aLine = sr.ReadLine();
                    continue;
                }

                dataArray = aLine.Split();
                int LastNonEmpty = -1;
                List<int> dataList = new List<int>();
                for (int i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] != string.Empty)
                    {
                        LastNonEmpty++;
                        dataList.Add(int.Parse(dataArray[i]));
                    }
                }
                colors.Add(Color.FromArgb(255, dataList[3], dataList[2], dataList[1]));

                aLine = sr.ReadLine();
            }
            sr.Close();

            return colors;
        }

        /// <summary>
        /// Set color palette to a image
        /// </summary>
        /// <param name="colors">color array</param>        
        public void SetPalette(List<Color> colors)
        {
            ColorPalette pal = _Image.Palette;
            int count = colors.Count;
            if (count > 256)
                count = 256;

            for (int i = 0; i < count; i++)
                pal.Entries[i] = colors[i];

            _Image.Palette = pal;
        }

        /// <summary>
        /// Set color palette to a image
        /// </summary>            
        public void UpdateTransparency()
        {            
            int alpha = (int)((1 - (double)TransparencyPerc / 100.0) * 255);
            ColorPalette pal = _Image.Palette;

            for (int i = 0; i < pal.Entries.Length; i++)
                pal.Entries[i] = Color.FromArgb(alpha, pal.Entries[i].R, pal.Entries[i].G, pal.Entries[i].B);

            _Image.Palette = pal;

            if (_isSetTransColor)
                SetTransparencyColor(_TransparencyColor);
        }

        /// <summary>
        /// Set a transparency color
        /// </summary>
        /// <param name="tColor">transparency color</param>        
        public void SetTransparencyColor(Color tColor)
        {
            ColorPalette pal = _Image.Palette;
            for (int i = 0; i < pal.Entries.Length; i++)
            {
                Color aColor = pal.Entries[i];
                if (aColor.R == tColor.R && aColor.G == tColor.G && aColor.B == tColor.B)
                {
                    pal.Entries[i] = aColor = Color.FromArgb(0, aColor.R, aColor.G, aColor.B);
                }
            }

            _Image.Palette = pal;
        }

        /// <summary>
        /// Override get custom property method
        /// </summary>
        /// <returns>property object</returns>
        public override object GetPropertyObject()
        {
            CustomProperty cp = (CustomProperty)base.GetPropertyObject();

            Dictionary<string, string> objAtt = cp.ObjectAttribs;
            objAtt.Add("WorldFileName", "WorldFileName");
            objAtt.Add("SetTransparencyColor", "SetTransparencyColor");
            objAtt.Add("TransparencyColor", "TransparencyColor");
            objAtt.Add("XUL", "XUL");
            objAtt.Add("YUL", "YUL");
            objAtt.Add("XScale", "XScale");
            objAtt.Add("YScale", "YScale");
            objAtt.Add("XRotate", "XRotate");
            objAtt.Add("YRotate", "YRotate");

            return new CustomProperty(this, objAtt);
        }

        #endregion
    }
}
