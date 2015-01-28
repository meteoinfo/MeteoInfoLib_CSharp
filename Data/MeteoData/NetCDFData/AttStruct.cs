using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// NetCDF attribute struct
    /// </summary>
    public struct AttStruct
    {
        #region Variables
        /// <summary>
        /// Attribute name
        /// </summary>
        public string attName;
        /// <summary>
        /// NetCDF data type
        /// </summary>
        public NetCDF4.NcType NCType;
        /// <summary>
        /// Attribute length
        /// </summary>
        public int attLen;
        /// <summary>
        /// Attribute value
        /// </summary>
        public object attValue;

        #endregion

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public AttStruct()
        //{
        //    attValue = null;
        //}

        #region Methods    
        /// <summary>
        /// To string
        /// </summary>
        /// <returns>string</returns>
        public new string ToString()
        {
            string outStr = "";           
            int j;
            switch (NCType)
            {
                case NetCDF4.NcType.NC_CHAR:
                    outStr = attValue.ToString();
                    break;
                case NetCDF4.NcType.NC_INT:
                    for (j = 0; j < ((int[])attValue).Length; j++)
                    {
                        outStr += ((int[])attValue)[j].ToString() + ", ";
                    }
                    break;
                case NetCDF4.NcType.NC_SHORT:
                    for (j = 0; j < ((Int16[])attValue).Length; j++)
                    {
                        outStr += ((Int16[])attValue)[j].ToString() + ", ";
                    }
                    break;
                case NetCDF4.NcType.NC_BYTE:
                    for (j = 0; j < ((byte[])attValue).Length; j++)
                    {
                        outStr += ((byte[])attValue)[j].ToString() + ", ";
                    }
                    break;
                case NetCDF4.NcType.NC_FLOAT:
                    for (j = 0; j < ((Single[])attValue).Length; j++)
                    {
                        outStr += ((Single[])attValue)[j].ToString() + ", ";
                    }
                    break;
                case NetCDF4.NcType.NC_DOUBLE:
                    for (j = 0; j < ((double[])attValue).Length; j++)
                    {
                        outStr += ((double[])attValue)[j].ToString() + ", ";
                    }
                    break;
            }
            outStr = outStr.TrimEnd();
            outStr = outStr.TrimEnd(new char[] { ',' });            

            return outStr;
        }

        #endregion
    }
}
