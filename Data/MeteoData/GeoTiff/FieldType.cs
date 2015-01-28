using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Field type
    /// </summary>
    public class FieldType
    {
        #region Variables
        private static FieldType[] types = new FieldType[20];
        /// <summary>
        /// BYTE field type
        /// </summary>
        public static FieldType BYTE = new FieldType("BYTE", 1, 1);
        /// <summary>
        /// ASCII field type
        /// </summary>
        public static FieldType ASCII = new FieldType("ASCII", 2, 1);
        /// <summary>
        /// SHORT field type
        /// </summary>
        public static FieldType SHORT = new FieldType("SHORT", 3, 2);
        /// <summary>
        /// LONG field type
        /// </summary>
        public static FieldType LONG = new FieldType("LONG", 4, 4);
        /// <summary>
        /// RATIONAL field type
        /// </summary>
        public static FieldType RATIONAL = new FieldType("RATIONAL", 5, 8);
        /// <summary>
        /// SBYTE field type
        /// </summary>
        public static FieldType SBYTE = new FieldType("SBYTE", 6, 1);
        /// <summary>
        /// UNDEFINED field type
        /// </summary>
        public static FieldType UNDEFINED = new FieldType("UNDEFINED", 7, 1);
        /// <summary>
        /// SSHORT field type
        /// </summary>
        public static FieldType SSHORT = new FieldType("SSHORT", 8, 2);
        /// <summary>
        /// SLONG field type
        /// </summary>
        public static FieldType SLONG = new FieldType("SLONG", 9, 4);
        /// <summary>
        /// SRATIONAL field type
        /// </summary>
        public static FieldType SRATIONAL = new FieldType("SRATIONAL", 10, 8);
        /// <summary>
        /// FLOAT field type
        /// </summary>
        public static FieldType FLOAT = new FieldType("FLOAT", 11, 4);
        /// <summary>
        /// DOUBLE field type
        /// </summary>
        public static FieldType DOUBLE = new FieldType("DOUBLE", 12, 8);
        /// <summary>
        /// Name
        /// </summary>
        public string Name;
        /// <summary>
        /// Code
        /// </summary>
        public int Code;
        /// <summary>
        /// Size
        /// </summary>
        public int Size;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="code">Code</param>
        /// <param name="size">Size</param>
        private FieldType(String name, int code, int size)
        {
            this.Name = name;
            this.Code = code;
            this.Size = size;
            types[code] = this;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get field type
        /// </summary>
        /// <param name="code">Field type code</param>
        /// <returns>Field type</returns>
        public static FieldType Get(int code)
        {
            return types[code];
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
