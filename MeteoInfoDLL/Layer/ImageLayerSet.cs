using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Reflection;
using MeteoInfoC.Global;
using MeteoInfoC.Legend;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// Image layer set
    /// </summary>
    public class ImageLayerSet
    {
        #region Variables

        private LayersLegend m_LayersLegend;
        private string m_LayerName;
        private string m_FileName;
        private string m_WorldFileName;
        private int m_Handle;
        private LayerDrawType m_LayerType;
        private Extent m_Extent;
        private Boolean m_Visible;
        private bool m_IsMaskout;
        private int _TransparencyPerc;
        private bool _SetTransparencyColor;
        private Color _TransparencyColor;
        private WorldFilePara m_WFP;
        private double m_XUL;
        private double m_YUL;
        private double m_XScale;
        private double m_YScale;
        private double m_Width;
        private double m_Height;
        private double m_XNum;
        private double m_YNum;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aILayer"></param>
        /// <param name="aLayersLegend"></param>
        public ImageLayerSet(ImageLayer aILayer, LayersLegend aLayersLegend)
        {
            m_LayersLegend = aLayersLegend;
            m_LayerName = aILayer.LayerName;
            m_FileName = aILayer.FileName;
            m_WorldFileName = aILayer.WorldFileName;
            m_Handle = aILayer.Handle;
            m_LayerType = aILayer.LayerDrawType;
            m_Visible = aILayer.Visible;
            m_WFP = aILayer.WorldFileParaV;
            m_XUL = aILayer.WorldFileParaV.XUL;
            m_YUL = aILayer.WorldFileParaV.YUL;
            m_XScale = aILayer.WorldFileParaV.XScale;
            m_YScale = aILayer.WorldFileParaV.YScale;
            m_Extent = aILayer.Extent;
            m_Width = m_Extent.maxX - m_Extent.minX;
            m_Height = m_Extent.minY - m_Extent.maxY;
            m_XNum = aILayer.Image.Width;
            m_YNum = aILayer.Image.Height;
            m_IsMaskout = aILayer.IsMaskout;
            _TransparencyPerc = aILayer.TransparencyPerc;
            _SetTransparencyColor = aILayer.SetTransColor;
            _TransparencyColor = aILayer.TransparencyColor;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get or set layer name
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer name")]
        public string LayerName
        {
            get
            {
                return m_LayerName;
            }
            set
            {
                m_LayerName = value;
                //m_LayersLegend.SetLayerName(m_Handle, m_LayerName);
            }
        }

        /// <summary>
        /// Get or set if visible
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer if visible")]
        public Boolean Visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                m_Visible = value;
                //m_LayersLegend.SetLayerVisible(m_Handle, m_Visible);
            }
        }

        /// <summary>
        /// Get or set if layer will be maskout
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer if maskout")]
        public bool IsMaskout
        {
            get { return m_IsMaskout; }
            set 
            {
                m_IsMaskout = value;
                //m_LayersLegend.SetLayerIsMaskout(m_Handle, m_IsMaskout);
            }
        }

        /// <summary>
        /// Get or set layer color transparency percent
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer color transparency percent (0 - 100)")]
        public int TransparencyPercent
        {
            get
            {
                return _TransparencyPerc;
            }
            set
            {
                _TransparencyPerc = value;
                if (_TransparencyPerc < 0)
                {
                    _TransparencyPerc = 0;
                }
                if (_TransparencyPerc > 100)
                {
                    _TransparencyPerc = 100;
                }
                //m_LayersLegend.SetLayerTransparency(m_Handle, _TransparencyPerc);
            }
        }

        /// <summary>
        /// Get or set if set transparency color
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("If set layer transparency color")]
        public bool SetTransparencyColor
        {
            get { return _SetTransparencyColor; }
            set
            {
                _SetTransparencyColor = value;

                // Create a PropertyDescriptor for "SpouseName" by calling the static GetProperties on TypeDescriptor.
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())["TransparencyColor"];

                // Fetch the ReadOnlyAttribute from the descriptor. 
                ReadOnlyAttribute attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];

                // Get the internal isReadOnly field from the ReadOnlyAttribute using reflection.  
                FieldInfo isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);

                // Using Reflection, set the internal isReadOnly field.     
                isReadOnly.SetValue(attrib, !value);

                //m_LayersLegend.SetImageLayerSetTransparencyColor(m_Handle, _SetTransparencyColor);
            }
        }

        /// <summary>
        /// Get or set transparecny color
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer transparency color")]
        [ReadOnly(true)]
        public Color TransparencyColor
        {
            get { return _TransparencyColor; }
            set
            {
                _TransparencyColor = value;
                //m_LayersLegend.SetImageLayerTransparencyColor(m_Handle, _TransparencyColor);
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
                return m_XUL;
            }
            set
            {
                m_XUL = value;
                m_WFP.XUL = m_XUL;
                m_Extent.minX = m_XUL;
                m_Extent.maxX = m_XUL + m_Width;
                //m_LayersLegend.SetImageLayerExtent(m_Handle, m_Extent, m_WFP);
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
                return m_YUL;
            }
            set
            {
                m_YUL = value;
                m_WFP.YUL = m_YUL;
                m_Extent.maxY = m_YUL;
                m_Extent.minY = m_YUL + m_Height;
                //m_LayersLegend.SetImageLayerExtent(m_Handle, m_Extent, m_WFP);
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
                return m_XScale;
            }
            set
            {
                m_XScale = value;
                m_WFP.XScale = m_XScale;
                m_Width = m_XNum * m_XScale;
                m_Extent.maxX = m_XUL + m_Width;
                //m_LayersLegend.SetImageLayerExtent(m_Handle, m_Extent, m_WFP);
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
                return m_YScale;
            }
            set
            {
                m_YScale = value;
                m_WFP.YScale = m_YScale;
                m_Height = m_YNum * m_YScale;
                m_Extent.minY = m_YUL + m_Height;
                //m_LayersLegend.SetImageLayerExtent(m_Handle, m_Extent, m_WFP);
            }
        }

        /// <summary>
        /// Get file name
        /// </summary>
        [CategoryAttribute("Layer Property"), DescriptionAttribute("File name of the layer")]
        public string FileName
        {
            get
            {
                return m_FileName;
            }
        }

        /// <summary>
        /// Get world file name
        /// </summary>
        [CategoryAttribute("Layer Property"), DescriptionAttribute("World file name of the layer")]
        public string WorldFileName
        {
            get
            {
                return m_WorldFileName;
            }
        }

        /// <summary>
        /// Get layer handle
        /// </summary>
        [CategoryAttribute("Layer Property"), DescriptionAttribute("Layer handle")]
        public int Handle
        {
            get
            {
                return m_Handle;
            }
        }

        /// <summary>
        /// Get layer type
        /// </summary>
        [CategoryAttribute("Layer Property"), DescriptionAttribute("layer type")]
        public LayerDrawType LayerType
        {
            get
            {
                return m_LayerType;
            }
        }
        #endregion
    }
}
