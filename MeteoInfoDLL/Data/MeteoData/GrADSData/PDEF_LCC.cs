using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Lambert projection define of GrADS data
    /// </summary>
    public struct PDEF_LCC
    {
        /// <summary>
        /// Projection type
        /// </summary>
        public string PType;
        /// <summary>
        /// The size of the native grid in the x direction
        /// </summary>
        public int isize;
        /// <summary>
        /// The size of the native grid in the y direction
        /// </summary>
        public int jsize;
        /// <summary>
        /// reference latitude
        /// </summary>
        public Single latref;
        /// <summary>
        /// reference longitude (in degrees, E is positive, W is negative)
        /// </summary>
        public Single lonref;
        /// <summary>
        /// i of ref point
        /// </summary>
        public Single iref;
        /// <summary>
        /// j of ref point 
        /// </summary>
        public Single jref;
        /// <summary>
        /// S true lat
        /// </summary>
        public Single Struelat;
        /// <summary>
        /// N true lat
        /// </summary>
        public Single Ntruelat;
        /// <summary>
        /// Standard longitude
        /// </summary>
        public Single slon;
        /// <summary>
        /// grid X increment in meters
        /// </summary>
        public Single dx;
        /// <summary>
        /// grid y increment in meters
        /// </summary>
        public Single dy;
    }
}
