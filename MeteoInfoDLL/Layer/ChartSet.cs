using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Legend;
using MeteoInfoC.Shape;
using System.Xml;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// Chart setting class
    /// </summary>
    public class ChartSet
    {
        #region Variables
        private ChartTypes _chartType;
        private bool _drawCharts;
        private List<string> _fieldNames;
        private int _xShift;
        private int _yShift;
        private LegendScheme _legendScheme;
        private int _maxSize;
        private int _minSize;
        private float _maxValue;
        private float _minValue;
        private int _barWidth;
        private bool _avoidCollision;
        private AlignType _alignType;
        private bool _view3D;
        private int _thickness;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ChartSet()
        {
            _chartType = ChartTypes.BarChart;
            _drawCharts = false;
            _fieldNames = new List<string>();
            _xShift = 0;
            _yShift = 0;
            _legendScheme = new LegendScheme(ShapeTypes.Polygon);
            _maxSize = 50;
            _minSize = 0;
            _barWidth = 8;
            _avoidCollision = true;
            _alignType = AlignType.Center;
            _view3D = false;
            _thickness = 5;

        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set chart type
        /// </summary>
        public ChartTypes ChartType
        {
            get { return _chartType; }
            set { _chartType = value; }
        }

        /// <summary>
        /// Get or set if draw charts
        /// </summary>
        public bool DrawCharts
        {
            get { return _drawCharts; }
            set { _drawCharts = value; }
        }

        /// <summary>
        /// Get or set field names
        /// </summary>
        public List<string> FieldNames
        {
            get { return _fieldNames; }
            set { _fieldNames = value; }
        }

        /// <summary>
        /// Get or set x shift
        /// </summary>
        public int XShift
        {
            get { return _xShift; }
            set { _xShift = value; }
        }

        /// <summary>
        /// Get or set y shift
        /// </summary>
        public int YShift
        {
            get { return _yShift; }
            set { _yShift = value; }
        }

        /// <summary>
        /// Get or set legend scheme
        /// </summary>
        public LegendScheme LegendScheme
        {
            get { return _legendScheme; }
            set { _legendScheme = value; }
        }

        /// <summary>
        /// Get or set maximum size
        /// </summary>
        public int MaxSize
        {
            get { return _maxSize; }
            set { _maxSize = value; }
        }

        /// <summary>
        /// Get or set minimum size
        /// </summary>
        public int MinSize
        {
            get { return _minSize; }
            set { _minSize = value; }
        }

        /// <summary>
        /// Get or set maximum value
        /// </summary>
        public float MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        /// <summary>
        /// Get or set minimum value
        /// </summary>
        public float MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        /// <summary>
        /// Get or set bar width
        /// </summary>
        public int BarWidth
        {
            get { return _barWidth; }
            set { _barWidth = value; }
        }

        /// <summary>
        /// Get or set if avoid collision
        /// </summary>
        public bool AvoidCollision
        {
            get { return _avoidCollision; }
            set { _avoidCollision = value; }
        }

        /// <summary>
        /// Get or set align type
        /// </summary>
        public AlignType AlignType
        {
            get { return _alignType; }
            set { _alignType = value; }
        }

        /// <summary>
        /// Get or set if view 3D
        /// </summary>
        public bool View3D
        {
            get { return _view3D; }
            set { _view3D = value; }
        }

        /// <summary>
        /// Get or set 3D thickness
        /// </summary>
        public int Thickness
        {
            get { return _thickness; }
            set { _thickness = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Export to XML document
        /// </summary>
        /// <param name="doc">xml document</param>
        /// <param name="parent">parent xml element</param>
        public void ExportToXML(ref XmlDocument doc, XmlElement parent)
        {

        }

        #endregion
    }
}
