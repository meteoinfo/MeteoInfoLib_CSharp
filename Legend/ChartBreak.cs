using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteoInfoC.Shape;
using MeteoInfoC.Global;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// legend scheme break of chart
    /// </summary>
    public class ChartBreak:ColorBreak
    {
        #region Variables
        private ChartTypes _chartType;
        private List<float> _chartData;
        private int _xShift;
        private int _yShift;
        private LegendScheme _legendScheme;
        private int _maxSize;
        private int _minSize;
        private float _maxValue;
        private float _minValue;
        private int _barWidth;
        private AlignType _alignType;
        private bool _view3D;
        private int _thickness;
        private int _shpIdx;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ChartBreak(ChartTypes chartType):base()
        {
            BreakType = BreakTypes.ChartBreak;
            _chartType = chartType;
            _chartData = new List<float>();
            _xShift = 0;
            _yShift = 0;
            _legendScheme = new LegendScheme(ShapeTypes.Polygon);
            _maxSize = 50;
            _minSize = 10;
            _barWidth = 10;
            _alignType = AlignType.Center;
            _view3D = false;
            _thickness = 5;
            _shpIdx = 0;
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
        /// Get or set chart data
        /// </summary>
        public List<float> ChartData
        {
            get { return _chartData; }
            set { _chartData = value; }
        }

        /// <summary>
        /// Get chart item number
        /// </summary>
        public int ItemNum
        {
            get { return _chartData.Count; }
        }

        /// <summary>
        /// Get data sum
        /// </summary>
        public float DataSum
        {
            get
            {
                float sum = 0;
                foreach (float d in _chartData)
                    sum += d;
                return sum;
            }
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

        /// <summary>
        /// Get or set shape index
        /// </summary>
        public int ShapeIndex
        {
            get { return _shpIdx; }
            set { _shpIdx = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get bar heights
        /// </summary>
        /// <returns>bar heights</returns>
        public List<int> GetBarHeights()
        {
            List<int> heights = new List<int>();
            int i, h;
            for (i = 0; i < _chartData.Count; i++)
            {
                if (_minSize == 0)
                    h = (int)(_chartData[i] / _maxValue * _maxSize);
                else
                    h = (int)((_chartData[i] - _minValue) / (_maxValue - _minValue) *
                        (_maxSize - _minSize) + _minSize);
                if (h == 0)
                    h = 1;
                heights.Add(h);
            }

            return heights;
        }

        /// <summary>
        /// Get chart width
        /// </summary>
        /// <returns>chart width</returns>
        public int GetWidth()
        {
            int width = 0;
            switch (_chartType)
            {
                case ChartTypes.BarChart:
                    width = _barWidth * _chartData.Count;
                    if (_view3D)
                        width += _thickness;
                    break;
                case ChartTypes.PieChart:
                    if (_minSize == _maxSize)
                        width = _maxSize;
                    else
                        if (_minSize == 0)
                            width = (int)(DataSum / _maxValue * _maxSize);
                        else
                            width = (int)((DataSum - _minValue) / (_maxValue - _minValue) *
                                (_maxSize - _minSize) + _minSize);
                    break;
            }

            return width;
        }

        /// <summary>
        /// Get chart height
        /// </summary>
        /// <returns>chart height</returns>
        public int GetHeight()
        {
            int height = 0;
            switch (_chartType)
            {
                case ChartTypes.BarChart:
                    height = GetBarHeights().Max();
                    break;
                case ChartTypes.PieChart:
                    if (_minSize == _maxSize)
                        height = _maxSize;
                    else
                        if (_minSize == 0)
                            height = (int)(DataSum / _maxValue * _maxSize);
                        else
                            height = (int)((DataSum - _minValue) / (_maxValue - _minValue) *
                                (_maxSize - _minSize) + _minSize);

                    if (_view3D)
                        height = height * 2 / 3;
                    break;
            }

            if (_view3D)
                height += _thickness;

            return height;
        }

        /// <summary>
        /// Get pie angles
        /// </summary>
        /// <returns>pie angle list</returns>
        public List<List<float>> GetPieAngles()
        {
            List<List<float>> angles = new List<List<float>>();
            float sum = DataSum;
            float startAngle = 0;
            float sweepAngle;
            for (int i = 0; i < _chartData.Count; i++)
            {
                sweepAngle = _chartData[i] / sum * 360;
                List<float> ssa = new List<float>();
                ssa.Add(startAngle);
                ssa.Add(sweepAngle);
                angles.Add(ssa);
                startAngle += sweepAngle;
                if (startAngle > 360)
                    startAngle = startAngle - 360;
            }

            return angles;
        }

        /// <summary>
        /// Override Clone method
        /// </summary>
        /// <returns>object</returns>
        public override object Clone()
        {
            ChartBreak aCB = new ChartBreak(_chartType);
            aCB.Caption = this.Caption;
            aCB.AlignType = _alignType;
            aCB.BarWidth = _barWidth;
            aCB.ChartData = new List<float>(_chartData);
            aCB.Color = Color;
            aCB.DrawShape = DrawShape;
            aCB.LegendScheme = _legendScheme;
            aCB.MaxSize = _maxSize;
            aCB.MaxValue = _maxValue;
            aCB.MinSize = _minSize;
            aCB.MinValue = _minValue;
            aCB.Thickness = _thickness;
            aCB.View3D = _view3D;
            aCB.XShift = _xShift;
            aCB.YShift = _yShift;

            return aCB;
        }

        /// <summary>
        /// Get sample chart break
        /// </summary>
        /// <returns>sample chart break</returns>
        public ChartBreak GetSampleChartBreak()
        {
            ChartBreak aCB = (ChartBreak)Clone();
            int i;
            switch (aCB.ChartType)
            {
                case ChartTypes.BarChart:
                    float min = aCB.MaxValue / aCB.ItemNum;
                    float dv = (aCB.MaxValue - min) / aCB.ItemNum;
                    for (i = 0; i < aCB.ItemNum; i++)
                    {
                        int v = (int)(min + dv * i);
                        double n = Math.Pow(10, v.ToString().Length - 1);
                        v = Convert.ToInt32(Math.Floor(v / n) * n);
                        aCB.ChartData[i] = v;
                    }
                    break;
                case ChartTypes.PieChart:
                    float sum = (aCB.MaxValue - aCB.MinValue) * 2 / 3;
                    float data = sum / aCB.ItemNum;
                    for (i = 0; i < aCB.ItemNum; i++)
                        aCB.ChartData[i] = data;
                    break;
            }

            return aCB;
        }

        /// <summary>
        /// Get draw extent
        /// </summary>
        /// <param name="aPoint">start point</param>
        /// <returns>draw extent</returns>
        public Extent GetDrawExtent(PointF aPoint)
        {
            int width = GetWidth();
            int height = GetHeight();
            switch (_alignType)
            {
                case AlignType.Center:
                    aPoint.X -= width / 2;
                    aPoint.Y += height / 2;
                    break;
                case AlignType.Left:
                    aPoint.X -= width;
                    aPoint.Y += height / 2;
                    break;
                case AlignType.Right:
                    aPoint.Y += height / 2;
                    break;
            }
            aPoint.X += _xShift;
            aPoint.Y -= _yShift;

            Extent aExtent = new Extent();
            aExtent.minX = aPoint.X;
            aExtent.maxX = aPoint.X + width;
            aExtent.minY = aPoint.Y - height;
            aExtent.maxY = aPoint.Y;

            return aExtent;
        }

        #endregion
    }
}
