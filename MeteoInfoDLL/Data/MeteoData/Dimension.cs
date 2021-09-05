using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Dimension
    /// </summary>
    public class Dimension
    {
        #region Variables
        private string _dimName;
        private DimensionType _dimType;
        private List<double> _dimValue;
        private int _dimId;
        private int _dimLength;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Dimension()
        {
            _dimType = DimensionType.Other;
            _dimValue = new List<double>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dimType">dimension type</param>
        public Dimension(DimensionType dimType)
        {
            _dimType = dimType;
            _dimValue = new List<double>();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get dimension length
        /// </summary>
        public int DimLength
        {
            get { return _dimLength; }
            set { _dimLength = value; }
        }

        /// <summary>
        /// Get or set dimension name
        /// </summary>
        public string DimName
        {
            get { return _dimName; }
            set { _dimName = value; }
        }

        /// <summary>
        /// Get or set dimension type
        /// </summary>
        public DimensionType DimType
        {
            get { return _dimType; }
            set { _dimType = value; }
        }

        /// <summary>
        /// Get dimension values
        /// </summary>
        public List<double> DimValue
        {
            get { return _dimValue; }
            //set 
            //{ 
            //    _dimValue = value;
            //    _dimLength = _dimValue.Count;
            //}
        }

        /// <summary>
        /// Get or set dim id
        /// </summary>
        public int DimId
        {
            get { return _dimId; }
            set { _dimId = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Jedge if two dimensions are equals
        /// </summary>
        /// <param name="aDim">a dimenstion</param>
        /// <returns>if equals</returns>
        public bool Equals(Dimension aDim)
        {
            if (_dimName != aDim.DimName) return false;
            if (_dimType != aDim.DimType) return false;
            if (DimLength != aDim.DimLength) return false;

            return true;
        }

        /// <summary>
        /// Set dimension values
        /// </summary>
        /// <param name="values">values</param>
        public void SetValues(List<double> values)
        {
            _dimValue = values;
            _dimLength = _dimValue.Count;
        }

        /// <summary>
        /// Set dimension values
        /// </summary>
        /// <param name="values">Values</param>
        public void SetValues(double[] values)
        {
            _dimValue = new List<double>();
            foreach (double v in values)
            {
                _dimValue.Add(v);
            }
            _dimLength = _dimValue.Count();
        }

        /// <summary>
        /// Get dimension values
        /// </summary>
        /// <returns>Values</returns>
        public double[] GetValues()
        {
            double[] values = new double[_dimLength];
            for (int i = 0; i < _dimLength; i++)
                values[i] = _dimValue[i];

            return values;
        }

        /// <summary>
        /// Add a dimension value
        /// </summary>
        /// <param name="value">a value</param>
        public void AddValue(double value)
        {
            _dimValue.Add(value);
            _dimLength = _dimValue.Count;
        }

        #endregion
    }
}
