using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// Layer type
    /// </summary>
    public enum LayerDrawType
    {
        /// <summary>
        /// Map layer
        /// </summary>
        Map,
        /// <summary>
        /// Shaded layer
        /// </summary>
        Shaded,
        /// <summary>
        /// Contour layer
        /// </summary>
        Contour,
        /// <summary>
        /// Grid fill layer
        /// </summary>
        GridFill,
        /// <summary>
        /// Grid point layer
        /// </summary>
        GridPoint,
        /// <summary>
        /// Wind vector layer
        /// </summary>
        Vector,
        /// <summary>
        /// Station point layer
        /// </summary>
        StationPoint,
        /// <summary>
        /// Wind barb layer
        /// </summary>
        Barb,
        /// <summary>
        /// Weather symbol layer
        /// </summary>
        WeatherSymbol,
        /// <summary>
        /// Station model layer
        /// </summary>
        StationModel,
        /// <summary>
        /// Image layer
        /// </summary>
        Image,
        /// <summary>
        /// Raster Layer
        /// </summary>
        Raster,
        /// <summary>
        /// Trajectory line layer
        /// </summary>
        TrajLine,
        /// <summary>
        /// Trajectory point layer
        /// </summary>
        TrajPoint,
        /// <summary>
        /// Streamline
        /// </summary>
        Streamline
    }     
}
