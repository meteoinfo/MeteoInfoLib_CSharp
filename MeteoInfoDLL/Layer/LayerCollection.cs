using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MeteoInfoC.Layer
{   
    /// <summary>
    /// Layers
    /// </summary>
    public class LayerCollection
    {
        #region Private Variables

        //private int _layerNum;
        private int _selectedLayer;
        private List<MapLayer> _layers;
        //private List<MapLayer> _geoLayers;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LayerCollection()
        {
            //_layerNum = 0;
            _selectedLayer = -1;
            _layers = new List<MapLayer>();
            //_geoLayers = new List<MapLayer>();
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get the number of layers
        /// </summary>
        public int LayerNum
        {
            get { return Layers.Count; }
        }

        /// <summary>
        /// Get or set selected layer of the layers
        /// </summary>
        public int SelectedLayer
        {
            get { return _selectedLayer; }
            set { _selectedLayer = value; }
        }

        /// <summary>
        /// Get or set layers
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] 
        public List<MapLayer> Layers
        {
            get { return _layers; }
            set { _layers = value; }
        }

        ///// <summary>
        ///// Get or set geographic layers with lat/lon to
        ///// save lat/lon layers when a projection is applied
        ///// </summary>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] 
        //public List<MapLayer> GeoLayers
        //{
        //    get { return _geoLayers; }
        //    set { _geoLayers = value; }
        //}

        #endregion
    }

}
