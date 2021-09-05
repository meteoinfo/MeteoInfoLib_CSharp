using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MeteoInfoC.Global;
using MeteoInfoC.Legend;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// Raset layer set
    /// </summary>
    public class RasterLayerSet
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
        private WorldFilePara m_WFP;
        private double m_XUL;
        private double m_YUL;
        private double m_XScale;
        private double m_YScale;
        private double m_Width;
        private double m_Height;
        private double m_XNum;
        private double m_YNum;
        private LegendSchemeE m_LegendSchemeE;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aILayer">raster layer</param>
        /// <param name="aLayersLegend">layersLegend</param>
        public RasterLayerSet(RasterLayer aILayer, LayersLegend aLayersLegend)
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
            m_LegendSchemeE = new LegendSchemeE();
            m_LegendSchemeE.LayerHandle = aILayer.Handle;
            m_LegendSchemeE.LegendScheme = aILayer.LegendScheme;
            m_LegendSchemeE.LayersTV = aLayersLegend;
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

        ///// <summary>
        ///// Get or set X upper-left
        ///// </summary>
        //[CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set image X coordinate of the center of the upper-left pixel")]
        //public double XUL
        //{
        //    get
        //    {
        //        return m_XUL;
        //    }
        //    set
        //    {
        //        m_XUL = value;
        //        m_WFP.XUL = m_XUL;
        //        m_Extent.minX = m_XUL;
        //        m_Extent.maxX = m_XUL + m_Width;
        //        m_LayersLegend.SetImageLayerExtent(m_Handle, m_Extent, m_WFP);
        //    }
        //}

        ///// <summary>
        ///// Get or set y upper-left
        ///// </summary>
        //[CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set image y coordinate of the center of the upper-left pixel")]
        //public double YUL
        //{
        //    get
        //    {
        //        return m_YUL;
        //    }
        //    set
        //    {
        //        m_YUL = value;
        //        m_WFP.YUL = m_YUL;
        //        m_Extent.maxY = m_YUL;
        //        m_Extent.minY = m_YUL + m_Height;
        //        m_LayersLegend.SetImageLayerExtent(m_Handle, m_Extent, m_WFP);
        //    }
        //}

        ///// <summary>
        ///// Get or set X scale
        ///// </summary>
        //[CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set dimension of a pixel in map units in X direction")]
        //public double XScale
        //{
        //    get
        //    {
        //        return m_XScale;
        //    }
        //    set
        //    {
        //        m_XScale = value;
        //        m_WFP.XScale = m_XScale;
        //        m_Width = m_XNum * m_XScale;
        //        m_Extent.maxX = m_XUL + m_Width;
        //        m_LayersLegend.SetImageLayerExtent(m_Handle, m_Extent, m_WFP);
        //    }
        //}

        ///// <summary>
        ///// Get or set y scale
        ///// </summary>
        //[CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set dimension of a pixel in map units in y direction")]
        //public double YScale
        //{
        //    get
        //    {
        //        return m_YScale;
        //    }
        //    set
        //    {
        //        m_YScale = value;
        //        m_WFP.YScale = m_YScale;
        //        m_Height = m_YNum * m_YScale;
        //        m_Extent.minY = m_YUL + m_Height;
        //        m_LayersLegend.SetImageLayerExtent(m_Handle, m_Extent, m_WFP);
        //    }
        //}

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

        /// <summary>
        /// Get or set legend scheme
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer legend scheme")]
        public LegendSchemeE LegendScheme
        {
            get
            {
                return m_LegendSchemeE;
            }
            set
            {
                m_LegendSchemeE = value;
                //m_LayersTV.SetLayerLegendScheme(m_LegendSchemeE.LayerHandle, m_LegendSchemeE.LegendScheme);
            }
        }

        #endregion
    }
}
