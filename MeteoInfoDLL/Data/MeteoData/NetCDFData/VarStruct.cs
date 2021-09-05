using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// NetCDF variable struct
    /// </summary>
    public class VarStruct
    {
        #region Variables
        /// <summary>
        /// Variable name
        /// </summary>
        public string varName;
        /// <summary>
        /// Variable identifer
        /// </summary>
        public int varid;
        /// <summary>
        /// NetCDF data type
        /// </summary>
        public NetCDF4.NcType ncType;
        ///// <summary>
        ///// Dimension number
        ///// </summary>
        //public int nDims;
        /// <summary>
        /// Dimension identifer array
        /// </summary>
        public int[] dimids;
        /// <summary>
        /// Dimension list
        /// </summary>
        public List<Dimension> Dimensions = new List<Dimension>();
        /// <summary>
        /// Attribute number
        /// </summary>
        public int nAtts;
        /// <summary>
        /// Attribute list
        /// </summary>
        public List<AttStruct> attList;
        /// <summary>
        /// If is data variable
        /// </summary>
        public bool isDataVar;
        /// <summary>
        /// Level list
        /// </summary>
        public List<double> Levels;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public VarStruct()
        {
            attList = new List<AttStruct>();
            Levels = new List<double>();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get dimention number
        /// </summary>
        public int DimNumber
        {
            get { return Dimensions.Count; }
        }

        /// <summary>
        /// Get level number
        /// </summary>
        public int LevelNum
        {
            get { return Levels.Count; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get attribute index by name, return -1 if the name not exist.
        /// </summary>
        /// <param name="attName">attribute name</param>
        /// <returns>attribute index</returns>
        public int GetAttributeIndex(string attName)
        {
            int idx = -1;
            for (int i = 0; i < attList.Count; i++)
            {
                if (attList[i].attName.ToLower() == attName.ToLower())
                {
                    idx = i;
                    break;
                }
            }

            return idx;
        }

        /// <summary>
        /// Get attribute value string by name
        /// </summary>
        /// <param name="attName">attribute name</param>
        /// <returns>attribute value string</returns>
        public string GetAttributeString(string attName)
        {
            string attStr = String.Empty;
            foreach (AttStruct aAtt in attList)
            {
                if (aAtt.attName.ToLower() == attName.ToLower())
                {
                    attStr = aAtt.ToString();
                }
            }

            return attStr;
        }

        /// <summary>
        /// Add attribute
        /// </summary>
        /// <param name="attName">attribute name</param>
        /// <param name="attValue">attribute value</param>
        public void AddAttribute(string attName, string attValue)
        {
            AttStruct aAtt = new AttStruct();
            aAtt.NCType = NetCDF4.NcType.NC_CHAR;
            aAtt.attName = attName;
            aAtt.attValue = attValue;
            aAtt.attLen = attValue.Length;

            attList.Add(aAtt);
            nAtts = attList.Count;
        }

        /// <summary>
        /// Add attribute
        /// </summary>
        /// <param name="attName">attribute name</param>
        /// <param name="attValue">attribute value</param>
        public void AddAttribute(string attName, double attValue)
        {
            AttStruct aAtt = new AttStruct();
            aAtt.NCType = NetCDF4.NcType.NC_DOUBLE;
            aAtt.attName = attName;
            aAtt.attValue = attValue;
            aAtt.attLen = 1;

            attList.Add(aAtt);
            nAtts = attList.Count;
        }

        /// <summary>
        /// Judge if the variable has a dimension
        /// </summary>
        /// <param name="dimId">dimension id</param>
        /// <returns>result</returns>
        public bool HasDimension(int dimId)
        {
            return ((IList<int>)dimids).Contains(dimId);
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>object</returns>
        public object Clone()
        {
            VarStruct aVarS = new VarStruct();
            aVarS.attList.AddRange(attList);
            aVarS.dimids = dimids;
            aVarS.Dimensions.AddRange(Dimensions);
            aVarS.isDataVar = isDataVar;
            aVarS.Levels.AddRange(Levels);
            aVarS.nAtts = nAtts;
            aVarS.ncType = ncType;
            //aVarS.nDims = nDims;
            aVarS.varid = varid;
            aVarS.varName = varName;

            return aVarS;
        }

        #endregion
    }
}
