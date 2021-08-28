using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Layer;

namespace MeteoInfoC.Map
{
    /// <summary>
    /// MapView interface
    /// </summary>
    public interface IMapView
    {
        #region Properties
        /// <summary>
        /// Layers
        /// </summary>
        LayerCollection LayerSet
        {
            get;
            set;
        }

        #endregion

        #region Methods


        #endregion
    }
}
