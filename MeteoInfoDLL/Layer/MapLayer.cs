using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using MeteoInfoC.Projections;
using MeteoInfoC.Global;
using MeteoInfoC.Legend;
using MeteoInfoC.Shape;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// Layer
    /// </summary>
    public class MapLayer
    {
        #region Variables
        private LayerTypes _layerType;
        private ShapeTypes _shapeType;
        private int _handle;
        private string _layerName;
        private string _fileName;
        private ProjectionInfo _projInfo;
        private Extent _extent;
        private Boolean _visible;
        private LayerDrawType _layerDrawType;
        private bool _isMaskout;
        private LegendScheme _legendScheme;
        private bool _Expended;        
        private int _TransparencyPerc;
        private string _tag;
        private VisibleScale _visibleScale;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public MapLayer()
        {            
            _fileName = "";
            _projInfo = KnownCoordinateSystems.Geographic.World.WGS1984;
            _handle = -1;            
            _visible = true;            
            _isMaskout = false;
            _Expended = false;
            _tag = string.Empty;
            _visibleScale = new VisibleScale();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set layer type
        /// </summary>
        [ReadOnly(true)]
        [CategoryAttribute("Layer Property"), DescriptionAttribute("layer type")]
        public LayerTypes LayerType
        {
            get { return _layerType; }
            set { _layerType = value; }
        }

        /// <summary>
        /// Get or set shape type
        /// </summary>
        [ReadOnly(true)]
        [CategoryAttribute("Layer Property"), DescriptionAttribute("layer shape type")]
        public ShapeTypes ShapeType
        {
            get { return _shapeType; }
            set { _shapeType = value; }
        }   

        /// <summary>
        /// Get or set layer handle
        /// </summary>
        [ReadOnly(true)]
        [CategoryAttribute("Layer Property"), DescriptionAttribute("Layer handle")]
        public int Handle
        {
            get { return _handle; }
            set { _handle = value; }
        }

        /// <summary>
        /// Get or set layer name
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer name")]
        public string LayerName
        {
            get { return _layerName; }
            set { _layerName = value; }
        }

        /// <summary>
        /// Get or set file name of the layer
        /// </summary>
        [ReadOnly(true)]
        [CategoryAttribute("Layer Property"), DescriptionAttribute("File name of the layer")]
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// Get or set projection info
        /// </summary>
        public ProjectionInfo ProjInfo
        {
            get { return _projInfo; }
            set { _projInfo = value; }
        }

        /// <summary>
        /// Get or set layer extent
        /// </summary>
        public Extent Extent
        {
            get { return _extent; }
            set { _extent = value; }
        }

        /// <summary>
        /// Get or set layer visible
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer if visible")]
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// Get or set layer draw type
        /// </summary>
        [ReadOnly(true)]
        [CategoryAttribute("Layer Property"), DescriptionAttribute("layer draw type")]
        public LayerDrawType LayerDrawType
        {
            get { return _layerDrawType; }
            set { _layerDrawType = value; }
        }        

        /// <summary>
        /// Get or set if layer will be maskout
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer if maskout")]
        public bool IsMaskout
        {
            get { return _isMaskout; }
            set { _isMaskout = value; }
        }

        /// <summary>
        /// Get or set legend scheme
        /// </summary>
        public virtual LegendScheme LegendScheme
        {
            get { return _legendScheme; }
            set { _legendScheme = value; }
        }

        /// <summary>
        /// Get or set if layer legend is expanded
        /// </summary>
        public bool Expanded
        {
            get { return _Expended; }
            set { _Expended = value; }
        }

        /// <summary>
        /// Get or set transparency percent
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer color transparency percent (0 - 100)")]
        public virtual int TransparencyPerc
        {
            get { return _TransparencyPerc; }
            set { _TransparencyPerc = value; }
        }

        /// <summary>
        /// Get or set Tag
        /// </summary>
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        /// Get or set visible scale
        /// </summary>
        public VisibleScale VisibleScale
        {
            get { return _visibleScale; }
            set { _visibleScale = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Set properties
        /// </summary>
        /// <returns>property object</returns>
        public virtual object GetPropertyObject()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("LayerType", "LayerType");
            objAttr.Add("ShapeType", "ShapeType");
            objAttr.Add("Handle", "Handle");
            objAttr.Add("LayerName", "LayerName");
            objAttr.Add("FileName", "FileName");
            objAttr.Add("Visible", "Visible");
            objAttr.Add("LayerDrawType", "LayerDrawType");
            objAttr.Add("IsMaskout", "IsMaskout");
            objAttr.Add("TransparencyPerc", "TransparencyPerc");
            CustomProperty cp = new CustomProperty(this, objAttr);

            return cp;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>object</returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
