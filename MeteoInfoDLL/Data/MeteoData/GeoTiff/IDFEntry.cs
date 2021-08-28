using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Image file directory entry
    /// </summary>
    class IFDEntry
    {
        #region Variables
        private Tag _tag;
        private FieldType _type;
        private int _length;
        private long _valueOffset;
        public int[] Value;
        public double[] ValueD;
        public String ValueS;
        //protected ArrayList geokeys = null;

        #endregion

        #region Constructor
        public IFDEntry()
        {

        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set tag
        /// </summary>
        public Tag Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        /// Get or set type
        /// </summary>
        public FieldType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Get or set length
        /// </summary>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// Get or set value/offset
        /// </summary>
        public long ValueOffset
        {
            get { return _valueOffset; }
            set { _valueOffset = value; }
        }

        #endregion
    }
}
