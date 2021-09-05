using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MapData
{
    /// <summary>
    /// Shape file
    /// </summary>
    public struct ShapeFile
    {
        /// <summary>
        /// Shape file type
        /// </summary>
        public ShapeFileType shapeFileType;
        /// <summary>
        /// Shape number
        /// </summary>
        public int shapeNum;
        /// <summary>
        /// Extent
        /// </summary>
        public Extent extent;
        /// <summary>
        /// Shape list
        /// </summary>
        public ArrayList shapes;
        /// <summary>
        /// Data table
        /// </summary>
        public DataTable dataTable;
    }
}
