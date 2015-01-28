using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Variable class
    /// </summary>
    public class Variable
    {
        #region Variables
        /// <summary>
        /// Parameter number
        /// </summary>
        public int Number;
        /// <summary>
        /// Variable name
        /// </summary>
        public string Name;
        /// <summary>
        /// Level type
        /// </summary>
        public int LevelType;
        /// <summary>
        /// Level list
        /// </summary>
        public List<double> Levels;
        /// <summary>
        /// Units
        /// </summary>
        public string Units;
        /// <summary>
        /// Description
        /// </summary>
        public string Description;

        private List<Dimension> _dimensions = new List<Dimension>();
        private string _hdfPath;
        private bool _isStation = false;
        private bool _isSwath = false;
        private NetCDF4.NcType _ncType;
        private List<AttStruct> _attributes = new List<AttStruct>();
        private int _attNumber;
        private int _varId;
        private bool _isCoordVar = false;
        private List<int> _levelIdxs = new List<int>();
        private List<int> _varInLevelIdxs = new List<int>();

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Variable()
        {            
            Name = "Undef";            
            Levels = new List<double>();
            Units = "Undef";
            Description = "Undef";            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aNum">Parameter number</param>
        /// <param name="aName">name</param>
        /// <param name="aDesc">description</param>
        /// <param name="aUnit">units</param>
        public Variable(int aNum, String aName, String aDesc, String aUnit)
        {
            Number = aNum;
            Name = aName;
            Description = aDesc;
            Units = aUnit;            
            Levels = new List<double>();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get dimention number
        /// </summary>
        public int DimNumber
        {
            get { return _dimensions.Count; }
        }

        /// <summary>
        /// Get level number
        /// </summary>
        public int LevelNum
        {
            get { return Levels.Count; }
        }

        /// <summary>
        /// Get or set HDF path
        /// </summary>
        public string HDFPath
        {
            get { return _hdfPath; }
            set { _hdfPath = value; }
        }

        /// <summary>
        /// Get or set dimensions
        /// </summary>
        public List<Dimension> Dimensions
        {
            get { return _dimensions; }
            set { _dimensions = value; }
        }

        /// <summary>
        /// Get or set X dimension
        /// </summary>
        public Dimension XDimension
        {
            get { return GetDimension(DimensionType.X); }
            set { SetDimension(value, DimensionType.X); }
        }

        /// <summary>
        /// Get or set Y dimension
        /// </summary>
        public Dimension YDimension
        {
            get { return GetDimension(DimensionType.Y); }
            set { SetDimension(value, DimensionType.Y); }
        }

        /// <summary>
        /// Get or set Z dimension
        /// </summary>
        public Dimension ZDimension
        {
            get { return GetDimension(DimensionType.Z); }
            set { SetDimension(value, DimensionType.Z); }
        }

        /// <summary>
        /// Get or set T dimension
        /// </summary>
        public Dimension TDimension
        {
            get { return GetDimension(DimensionType.T); }
            set { SetDimension(value, DimensionType.T); }
        }

        /// <summary>
        /// Get dimension identifers
        /// </summary>
        public int[] DimIds
        {
            get
            {
                int[] dimids = new int[_dimensions.Count];
                for (int i = 0; i < _dimensions.Count; i++)
                    dimids[i] = _dimensions[i].DimId;

                return dimids;
            }
        }

        /// <summary>
        /// Get or set if the variable is station data
        /// </summary>
        public bool IsStation
        {
            get { return _isStation; }
            set { _isStation = value; }
        }

        /// <summary>
        /// Get or set if the variable is swath data set
        /// </summary>
        public bool IsSwath
        {
            get { return _isSwath; }
            set { _isSwath = value; }
        }

        /// <summary>
        /// Get of set NC type
        /// </summary>
        public NetCDF4.NcType NCType
        {
            get { return _ncType; }
            set { _ncType = value; }
        }

        /// <summary>
        /// Get or set attributes
        /// </summary>
        public List<AttStruct> Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        /// <summary>
        /// Get or set attribute number
        /// </summary>
        public int AttNumber
        {
            get { return _attNumber; }
            set { _attNumber = value; }
        }

        /// <summary>
        /// Get or set variable id
        /// </summary>
        public int VarId
        {
            get { return _varId; }
            set { _varId = value; }
        }

        /// <summary>
        /// Get or set if the variable is coordinate variable
        /// </summary>
        public bool IsCoorVar
        {
            get { return _isCoordVar; }
            set { _isCoordVar = value; }
        }

        /// <summary>
        /// Get or set level index list - for ARL data
        /// </summary>
        public List<int> LevelIdxs
        {
            get { return _levelIdxs; }
            set { _levelIdxs = value; }
        }

        /// <summary>
        /// Get or set Variable index in level index list - for ARL data
        /// </summary>
        public List<int> VarInLevelIdxs
        {
            get { return _varInLevelIdxs; }
            set { _varInLevelIdxs = value; }
        }

        /// <summary>
        /// Get if the variable is plottable - has both X and Y dimension for grid data,
        /// or is station data
        /// </summary>
        public bool IsPlottable
        {
            get
            {
                if (_isStation || _isSwath)
                    return true;
                if (this.XDimension == null)
                    return false;
                if (this.YDimension == null)
                    return false;

                return true;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>parameter object</returns>
        public object Clone()
        {
            Variable aPar = new Variable();
            aPar.Number = Number;
            aPar.Name = Name;
            aPar.Units = Units;
            aPar.Description = Description;
            //aPar.LevelNum = LevelNum;
            aPar.LevelType = LevelType;
            //aPar.Levels = Levels;

            aPar.Attributes.AddRange(_attributes);
            aPar.Dimensions.AddRange(Dimensions);
            aPar.IsCoorVar = _isCoordVar;
            aPar.Levels.AddRange(Levels);
            aPar.AttNumber = _attNumber;
            aPar.NCType = _ncType;
            aPar.VarId = _varId;

            return aPar;
        }

        /// <summary>
        /// Judge if two parameter are equal
        /// </summary>
        /// <param name="aVar">variable</param>
        /// <returns>if equal</returns>
        public bool Equals(Variable aVar)
        {
            if (Name != aVar.Name) return false;
            if (Number != aVar.Number) return false;
            if (Description != aVar.Description) return false;
            if (Units != aVar.Units) return false;

            return true;
        }

        /// <summary>
        /// Judge if two parameter are totally equal
        /// </summary>
        /// <param name="aVar">variable</param>
        /// <returns>if equal</returns>
        public bool TEquals(Variable aVar)
        {
            if (Name != aVar.Name) return false;
            if (Number != aVar.Number) return false;
            if (Description != aVar.Description) return false;
            if (Units != aVar.Units) return false;
            if (LevelType != aVar.LevelType) return false;

            return true;
        }

        /// <summary>
        /// Add a level
        /// </summary>
        /// <param name="levelValue">level value</param>
        public void AddLevel(float levelValue)
        {
            if (!Levels.Contains(levelValue))
            {
                Levels.Add(levelValue);                
            }
        }

        /// <summary>
        /// Get true level number
        /// </summary>
        /// <returns>true level number</returns>
        public int GetTrueLevelNumber()
        {
            if (LevelNum == 0)
                return 1;
            else
                return LevelNum;
        }

        /// <summary>
        /// Get dimension by type
        /// </summary>
        /// <param name="dimType">dimension type</param>
        /// <returns>dimensioin</returns>
        public Dimension GetDimension(DimensionType dimType)
        {
            Dimension aDim = null;
            for (int i = 0; i < DimNumber; i++)
            {
                if (_dimensions[i].DimType == dimType)
                {
                    aDim = _dimensions[i];
                    break;
                }
            }

            return aDim;
        }

        /// <summary>
        /// Set dimension
        /// </summary>
        ///<param name="aDim">a dimension</param>
        public void SetDimension(Dimension aDim)
        {
            bool hasDim = false; 
            for (int i = 0; i < DimNumber; i++)
            {
                if (_dimensions[i].DimType == aDim.DimType)
                {
                    _dimensions[i] = aDim;
                    hasDim = true;
                    break;
                }
            }

            if (!hasDim)
                _dimensions.Add(aDim);
        }

        /// <summary>
        /// Set dimension by dimension type
        /// </summary>
        /// <param name="aDim">a dimension</param>
        /// <param name="dimType">dimension type</param>
        public void SetDimension(Dimension aDim, DimensionType dimType)
        {
            if (aDim.DimType != dimType)
                return;
            else
                SetDimension(aDim);
        }

        /// <summary>
        /// Get index of a dimension
        /// </summary>
        /// <param name="aDim">a dimension</param>
        /// <returns>index</returns>
        public int GetDimIndex(Dimension aDim)
        {
            int idx = -1;
            for (int i = 0; i < DimNumber; i++)
            {
                if (aDim.Equals(_dimensions[i]))
                {
                    idx = i;
                    break;
                }
            }

            return idx;
        }

        /// <summary>
        /// Judge is has Xtrack dimension
        /// </summary>
        /// <returns>if has Xtrack dimension</returns>
        public bool HasXtrackDimension()
        {
            bool has = false;
            for (int i = 0; i < DimNumber; i++)
            {
                if (_dimensions[i].DimType == DimensionType.Xtrack)
                {
                    has = true;
                    break;
                }
            }

            return has;
        }

        /// <summary>
        /// Judge if the variable has a dimension
        /// </summary>
        /// <param name="dimId">dimension id</param>
        /// <returns>result</returns>
        public bool HasDimension(int dimId)
        {
            foreach (Dimension aDim in _dimensions)
            {
                if (aDim.DimId == dimId)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Get attribute index by name, return -1 if the name not exist.
        /// </summary>
        /// <param name="attName">attribute name</param>
        /// <returns>attribute index</returns>
        public int GetAttributeIndex(string attName)
        {
            int idx = -1;
            for (int i = 0; i < _attributes.Count; i++)
            {
                if (_attributes[i].attName.ToLower() == attName.ToLower())
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
            foreach (AttStruct aAtt in _attributes)
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

            _attributes.Add(aAtt);
            _attNumber = _attributes.Count;
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

            _attributes.Add(aAtt);
            _attNumber = _attributes.Count;
        }

        /// <summary>
        /// Get time list
        /// </summary>
        /// <returns>Time list</returns>
        public List<DateTime> getTimes()
        {
            Dimension tDim = this.TDimension;
            if (tDim == null)
            {
                return null;
            }

            List<Double> values = tDim.DimValue;
            List<DateTime> times = new List<DateTime>();
            foreach (Double v in values)
            {
                //times.Add(DateTime.FromOADate(v));
                times.Add(DataConvert.ToDateTime(v));
            }

            return times;
        }

        #endregion
    }
}
