using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Projections;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Data info class
    /// </summary>
    public abstract class DataInfo
    {
        #region Variables
        private String _fileName;
        private List<Variable> _variables = new List<Variable>();
        private Dimension _tDim = new Dimension(DimensionType.T);
        private Dimension _xDim = new Dimension(DimensionType.X);
        private Dimension _yDim = new Dimension(DimensionType.Y);
        private Dimension _zDim = new Dimension(DimensionType.Z);
        private bool _xReverse = false;
        private bool _yReverse = false;
        private bool _isGlobal = false;
        private double _missingValue = -9999.0;
        private ProjectionInfo _projInfo = KnownCoordinateSystems.Geographic.World.WGS1984;

        #endregion

        #region Properties
        /// <summary>
        /// Get or set file name
        /// </summary>
        public String FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// Get or set variables
        /// </summary>
        public List<Variable> Variables
        {
            get { return _variables; }
            set { _variables = value; }
        }

        /// <summary>
        /// Get variable number
        /// </summary>
        public int VariableNum
        {
            get { return _variables.Count; }
        }

        /// <summary>
        /// Get variable names
        /// </summary>
        public List<string> VariableNames
        {
            get
            {
                List<String> names = new List<String>();
                foreach (Variable var in _variables)
                {
                    names.Add(var.Name);
                }

                return names;
            }
        }

        /// <summary>
        /// Get or set times
        /// </summary>
        public List<DateTime> Times
        {
            get
            {
                List<double> values = _tDim.DimValue;
                List<DateTime> times = new List<DateTime>();
                foreach (double v in values)
                {
                    times.Add(DataConvert.ToDateTime(v));
                }

                return times;
            }
            set
            {
                List<double> values = new List<double>();
                List<DateTime> times = value;
                foreach (DateTime v in times)
                {
                    values.Add(DataConvert.ToDouble(v));
                }

                _tDim.SetValues(values);
            }
        }

        /// <summary>
        /// Get time number
        /// </summary>
        public int TimeNum
        {
            get { return _tDim.DimLength; }
        }

        /// <summary>
        /// Get or set time dimension
        /// </summary>
        public Dimension TimeDimension
        {
            get { return _tDim; }
            set { _tDim = value; }
        }

        /// <summary>
        /// Get or set X dimension
        /// </summary>
        public Dimension XDimension
        {
            get { return _xDim; }
            set { _xDim = value; }
        }

        /// <summary>
        /// Get or set Y dimension
        /// </summary>
        public Dimension YDimension
        {
            get { return _yDim; }
            set { _yDim = value; }
        }

        /// <summary>
        /// Get or set Z dimension
        /// </summary>
        public Dimension ZDimension
        {
            get { return _zDim; }
            set { _zDim = value; }
        }

        /// <summary>
        /// Get or set if x reversed
        /// </summary>
        public bool IsXReverse
        {
            get { return _xReverse; }
            set { _xReverse = value; }
        }

        /// <summary>
        /// Get or set if y reversed
        /// </summary>
        public bool IsYReverse
        {
            get { return _yReverse; }
            set { _yReverse = value; }
        }

        /// <summary>
        /// Get or set if is global data
        /// </summary>
        public bool IsGlobal
        {
            get { return _isGlobal; }
            set { _isGlobal = value; }
        }

        /// <summary>
        /// Get or set missing value
        /// </summary>
        public double MissingValue
        {
            get { return _missingValue; }
            set { _missingValue = value; }
        }

        /// <summary>
        /// Get or set projectin info
        /// </summary>
        public ProjectionInfo ProjectionInfo
        {
            get { return _projInfo; }
            set { _projInfo = value; }
        }

        #endregion

        #region Methods
       /// <summary>
       /// Read data info
       /// </summary>
       /// <param name="fileName">File name</param>
        public abstract void ReadDataInfo(String fileName);

        /// <summary>
        /// Generate data info text
        /// </summary>
        /// <returns>Data info text</returns>
        public abstract string GenerateInfoText();

        /// <summary>
        /// Get variable by name
        /// </summary>
        /// <param name="varName">Variable name</param>
        /// <returns>Variable</returns>
        public Variable GetVariable(String varName)
        {
            foreach (Variable var in _variables)
            {
                if (var.Name == varName)
                    return var;
            }

            return null;
        }

        #endregion
    }
}
