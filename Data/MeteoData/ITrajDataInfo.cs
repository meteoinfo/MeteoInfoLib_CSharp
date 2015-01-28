using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Layer;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Trajectory data info interface
    /// </summary>
    public interface ITrajDataInfo
    {
        /// <summary>
        /// Create trajectory line layer
        /// </summary>
        /// <returns>Map layer</returns>
        VectorLayer CreateTrajLineLayer();

        /// <summary>
        /// Create trajectory point layer
        /// </summary>
        /// <returns>Map layer</returns>
        VectorLayer CreateTrajPointLayer();

        /// <summary>
        /// Create trajectory start point layer
        /// </summary>
        /// <returns>Map layer</returns>
        VectorLayer CreateTrajStartPointLayer();
    }
}
