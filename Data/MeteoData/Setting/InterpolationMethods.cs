using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Grid interpolation methods
    /// </summary>
    public enum InterpolationMethods
    {
        /// <summary>
        /// IDW radius
        /// </summary>
        IDW_Radius,
        /// <summary>
        /// IDV neighbors
        /// </summary>
        IDW_Neighbors,
        /// <summary>
        /// Cressman analysis
        /// </summary>
        Cressman,
        /// <summary>
        /// Assign point to grid
        /// </summary>
        AssignPointToGrid
    }
}
