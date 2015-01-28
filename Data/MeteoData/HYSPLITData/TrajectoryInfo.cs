using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Trajectory info
    /// </summary>
    public class TrajectoryInfo
    {
        /// <summary>
        /// Trajectory name
        /// </summary>
        public string TrajName;
        /// <summary>
        /// Trajectory identifer
        /// </summary>
        public string TrajID;
        /// <summary>
        /// Trajectory center
        /// </summary>
        public string TrajCenter;
        /// <summary>
        /// Start time
        /// </summary>
        public DateTime StartTime;
        /// <summary>
        /// Start latitude
        /// </summary>
        public Single StartLat;
        /// <summary>
        /// Start longitude
        /// </summary>
        public Single StartLon;
        /// <summary>
        /// Start height
        /// </summary>
        public Single StartHeight;        
    }
}
