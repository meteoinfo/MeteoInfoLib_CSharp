using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace MeteoInfoC.Map
{
    /// <summary>
    /// Layer name converter
    /// </summary>
    public class LayerNameConverter : StringConverter
    {
        /// <summary>
        /// Get standard values supported
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Get standard values
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> list = (context.Instance as MaskOut).PolygonMapLayerList;
            StandardValuesCollection cols = new StandardValuesCollection(list);
            return cols;
        }

    }

    /// <summary>
    /// Mask out the certain region
    /// </summary>
    public class MaskOut
    {
        #region Variables

        /// <summary>
        /// MapView object
        /// </summary>
        public MapView m_MapControl;

        private Boolean m_SetMaskLayer;
        private String m_MaskLayer;
        private List<string> m_list;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aMapView"></param>
        public MaskOut(MapView aMapView)
        {
            m_MapControl = aMapView;
            m_SetMaskLayer = false;
            m_MaskLayer = "";
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get or set polygon map layer list
        /// </summary>
        [Browsable(false)]
        public List<string> PolygonMapLayerList
        {
            get
            {
                return m_list;
            }
            set
            {
                m_list = value;
            }
        }

        /// <summary>
        /// Get or set if mask out
        /// </summary>
        [CategoryAttribute("Mask Layer"), DescriptionAttribute("If set mask layer")]
        public Boolean SetMaskLayer
        {
            get
            {
                return m_SetMaskLayer;
            }
            set
            {
                m_SetMaskLayer = value;

                //// Create a PropertyDescriptor for "SpouseName" by calling the static GetProperties on TypeDescriptor.
                //PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())["MaskLayer"];

                //// Fetch the ReadOnlyAttribute from the descriptor. 
                //ReadOnlyAttribute attrib = (ReadOnlyAttribute)descriptor.Attributes[typeof(ReadOnlyAttribute)];

                //// Get the internal isReadOnly field from the ReadOnlyAttribute using reflection.  
                //FieldInfo isReadOnly = attrib.GetType().GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);

                //// Using Reflection, set the internal isReadOnly field.     
                //isReadOnly.SetValue(attrib, !value);

                m_MapControl.PaintLayers();
            }
        }

        /// <summary>
        /// Get or set mask layer name
        /// </summary>
        [CategoryAttribute("Mask Layer"), DescriptionAttribute("Set mask layer")]
        [TypeConverter(typeof(LayerNameConverter))]
        //[ReadOnly(true)]
        public string MaskLayer
        {
            get
            {
                return m_MaskLayer;
            }
            set
            {
                m_MaskLayer = value;
                m_MapControl.PaintLayers();
            }
        }
        #endregion
    }
}
