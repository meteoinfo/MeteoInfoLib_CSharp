using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using MeteoInfoC.Shape;
using MeteoInfoC.Legend;
using MeteoInfoC.Map;
using MeteoInfoC.Global;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// Legend scheme editor
    /// </summary>
    public class LegendSchemeEditor : UITypeEditor
    {
        /// <summary>
        /// Get edit style
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Edit value
        /// </summary>
        /// <param name="context">ITypeDescriptorContex</param>
        /// <param name="provider">IserviceProvider</param>
        /// <param name="value">object</param>
        /// <returns>object</returns>
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            LegendSchemeE aLSE = value as LegendSchemeE;
            if (svc != null)
            {
                //using (frmLegendSet frmLS = new frmLegendSet(false, aLSE.LayerHandle, aLSE.LayersTV))
                //{
                //    frmLS.SetLegendScheme(aLSE.LegendScheme);                    
                //    if (svc.ShowDialog(frmLS) == DialogResult.OK)
                //    {
                //        aLSE.LegendScheme = frmLS.GetLegendScheme();
                //        //frmMain.G_LayersTV.SetLayerLegendSchemeProj(aLSE.LayerHandle, aLSE.LegendScheme);
                //        //aLSE.LayersTV.SetLayerLegendSchemeProj(aLSE.LayerHandle, aLSE.LegendScheme);
                //    }
                //    else
                //    {
                //        if (frmLS.GetIsApplied())
                //        {
                //            //frmMain.G_LayersTV.SetLayerLegendScheme(aLSE.LayerHandle, aLSE.LegendScheme);
                //            //aLSE.LayersTV.SetLayerLegendScheme(aLSE.LayerHandle, aLSE.LegendScheme);
                //        }
                //    }
                //}
            }
            return value; // can also replace the wrapper object here    
        }
    }

    /// <summary>
    /// Legend scheme set
    /// </summary>
    [Editor(typeof(LegendSchemeEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(MyConverter))]
    public class LegendSchemeE
    {
        private int m_LayerHandle;
        private LegendScheme m_LegendScheme;
        private LayersLegend m_LayersTV;

        /// <summary>
        /// Layer handle
        /// </summary>
        public int LayerHandle
        {
            get
            {
                return m_LayerHandle;
            }

            set
            {
                m_LayerHandle = value;
            }
        }

        /// <summary>
        /// Legend scheme
        /// </summary>
        public LegendScheme LegendScheme
        {
            get
            {
                return m_LegendScheme;
            }
            set
            {
                m_LegendScheme = value;
            }
        }

        /// <summary>
        /// LayersLegend
        /// </summary>
        public LayersLegend LayersTV
        {
            get
            {
                return m_LayersTV;
            }
            set
            {
                m_LayersTV = value;
            }
        }
    }

    /// <summary>
    /// Convert to string
    /// </summary>
    public class MyConverter : StringConverter
    {
        /// <summary>
        /// Convert to
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return "Edit...";
        }
    }

    /// <summary>
    /// Vector layer set
    /// </summary>
    public class VectorLayerSet
    {
        #region Variable

        private string m_LayerName;
        private string m_FileName;
        private int m_Handle;
        private Extent m_Extent;
        private Boolean m_Visible;
        private bool m_IsMaskout;
        private LayerDrawType m_LayerDrawType;
        private LayerTypes m_LayerType;
        private ShapeTypes m_ShapeType;        
        private bool m_AviodCollision;
        private int m_TransparencyPerc;
        private int m_ShapeNum;
        private LegendSchemeE m_LegendSchemeE;
        private LayersLegend m_LayersTV;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aLayer"></param>
        /// <param name="aLayersTV"></param>
        public VectorLayerSet(VectorLayer aLayer, LayersLegend aLayersTV)
        {
            m_LayerName = aLayer.LayerName;
            m_FileName = aLayer.FileName;
            m_Handle = aLayer.Handle;
            m_Extent = aLayer.Extent;
            m_Visible = aLayer.Visible;
            m_LayerDrawType = aLayer.LayerDrawType;
            m_LayerType = aLayer.LayerType;
            m_ShapeType = aLayer.ShapeType;            
            m_AviodCollision = aLayer.AvoidCollision;
            m_TransparencyPerc = aLayer.TransparencyPerc;
            m_ShapeNum = aLayer.ShapeNum;
            m_LayersTV = aLayersTV;
            m_LegendSchemeE = new LegendSchemeE();
            m_LegendSchemeE.LayerHandle = aLayer.Handle;
            m_LegendSchemeE.LegendScheme = (LegendScheme)aLayer.LegendScheme.Clone();
            m_LegendSchemeE.LayersTV = aLayersTV;
            m_IsMaskout = aLayer.IsMaskout;
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
                //m_LayersTV.SetLayerName(m_Handle, m_LayerName);
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
                return m_TransparencyPerc;
            }
            set
            {
                m_TransparencyPerc = value;
                if (m_TransparencyPerc < 0)
                {
                    m_TransparencyPerc = 0;
                }
                if (m_TransparencyPerc > 100)
                {
                    m_TransparencyPerc = 100;
                }
                if (m_ShapeType == ShapeTypes.Polygon)
                {
                    //m_LayersTV.SetLayerTransparency(m_Handle, m_TransparencyPerc);
                }
            }
        }

        /// <summary>
        /// Get or set if layer visible
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
                //m_LayersTV.SetLayerVisible(m_Handle, m_Visible);
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
                //m_LayersTV.SetLayerIsMaskout(m_Handle, m_IsMaskout);
            }
        }        

        /// <summary>
        /// Get or set if enable avoid collision
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer if enable avoid collision")]
        public bool AvoidCollision
        {
            get
            {
                return m_AviodCollision;
            }
            set
            {
                m_AviodCollision = value;
                //m_LayersTV.SetLayerAvoidCollision(m_Handle, m_AviodCollision);
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
        
        /// <summary>
        /// Get layer file name
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
        public LayerTypes LayerType
        {
            get
            {
                return m_LayerType;
            }
        }

        /// <summary>
        /// Get layer draw type
        /// </summary>
        [CategoryAttribute("Layer Property"), DescriptionAttribute("layer draw type")]
        public LayerDrawType LayerDrawType
        {
            get
            {
                return m_LayerDrawType;
            }
        }

        /// <summary>
        /// Get layer shape type
        /// </summary>
        [CategoryAttribute("Layer Property"), DescriptionAttribute("layer shape type")]
        public ShapeTypes ShapeType
        {
            get
            {
                return m_ShapeType;
            }
        }

        /// <summary>
        /// Get layer shape number
        /// </summary>
        [CategoryAttribute("Layer Property"), DescriptionAttribute("layer shape number")]
        public int ShapeNum
        {
            get
            {
                return m_ShapeNum;
            }
        }


        //[CategoryAttribute("Layer Extent"), DescriptionAttribute("Layer left")]
        //public Single Left
        //{
        //    get
        //    {
        //        return (float)m_Extent.minX;
        //    }
        //}

        //[CategoryAttribute("Layer Extent"), DescriptionAttribute("Layer right")]
        //public Single Right
        //{
        //    get
        //    {
        //        return (float)m_Extent.maxX;
        //    }
        //}

        //[CategoryAttribute("Layer Extent"), DescriptionAttribute("Layer bottom")]
        //public Single Bottom
        //{
        //    get
        //    {
        //        return (float)m_Extent.minY;
        //    }
        //}

        //[CategoryAttribute("Layer Extent"), DescriptionAttribute("Layer top")]
        //public Single Top
        //{
        //    get
        //    {
        //        return (float)m_Extent.maxY;
        //    }
        //}

        #endregion
    }
}
