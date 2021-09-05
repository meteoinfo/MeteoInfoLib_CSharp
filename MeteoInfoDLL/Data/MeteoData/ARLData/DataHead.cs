using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// ARL Data head
    /// </summary>
    public struct DataHead
    {
        /// <summary>
        /// MODEL - Dada source
        /// </summary>
        public string MODEL;
        /// <summary>
        /// Forecast hour (>99 the header forecast hr = 99)
        /// </summary>
        public int ICX;
        /// <summary>
        /// Minutes associated with data time
        /// </summary>
        public Int16 MN;
        /// <summary>
        /// Standard conformal projections are drawn around a central (polar) point. 
        /// A Normal Projection is one where this point is either the North Pole (latitude = +90.°) 
        /// or the South Pole (latitude = -90.°). 
        /// </summary>
        public Single POLE_LAT;
        /// <summary>
        /// Polar longitude
        /// </summary>
        public Single POLE_LON;
        /// <summary>
        /// Reference latitude is the angle at which the cone of a Lambert Conformal projection is tangent to the Earth
        /// </summary>
        public Single REF_LAT;
        /// <summary>
        /// Reference longitude is the longitude furthest from the cut
        /// </summary>
        public Single REF_LON;
        /// <summary>
        /// At reference longitude and latitude point,
        /// the scale (gridsize in km.) and orientation (degrees between local North and the y-axis) 
        /// have the specified values
        /// </summary>
        public Single SIZE;
        /// <summary>
        /// Orientation
        /// </summary>
        public Single ORIENT;
        /// <summary>
        /// TANG_LAT: Stereographic Projections are commonly drawn on a plane tangent to the North or South Pole,
        /// and Mercator projections on a cylinder parallel to the axis between the two poles
        /// </summary>
        public Single TANG_LAT;
        /// <summary>
        /// SYNC_XP
        /// </summary>
        public Single SYNC_XP;
        /// <summary>
        /// SYNC_YP
        /// </summary>
        public Single SYNC_YP;
        /// <summary>
        /// SYNC_LAT: To align (synchronize) the grid coordinates with the map, 
        /// we specify the latitude and longitude coordinates of a specific grid point, 
        /// which could be the origin or could be the midpoint of the grid, 
        /// or any other point in the grid
        /// </summary>
        public Single SYNC_LAT;
        /// <summary>
        /// SYNU_LON
        /// </summary>
        public Single SYNC_LON;
        /// <summary>
        /// DUMMY
        /// </summary>
        public Single DUMMY;
        /// <summary>
        /// X number
        /// </summary>
        public int NX;
        /// <summary>
        /// Y number
        /// </summary>
        public int NY;
        /// <summary>
        /// Z number - levels
        /// </summary>
        public int NZ;
        /// <summary>
        /// Vertical coordinate system flag
        /// </summary>
        public Int16 K_FLAG;
        /// <summary>
        /// Length in bytes of the index record, excluding the first 50 bytes
        /// </summary>
        public int LENH;
    }
}
