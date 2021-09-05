using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;
using MeteoInfoC.Geoprocess;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Polyline shape
    /// </summary>
    public class PolylineShape:Shape
    {
        #region Variables
        /// <summary>
        /// Points list
        /// </summary>
        private List<PointD> _points;
        private List<PolyLine> _polyLines;

        /// <summary>
        /// Value
        /// </summary>
        public double value;
        /// <summary>
        /// Part number
        /// </summary>
        private int _numParts;
        /// <summary>
        /// Part array
        /// </summary>
        public int[] parts;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PolylineShape()
        {
            ShapeType = ShapeTypes.Polyline;
            _points = new List<PointD>();
            _numParts = 1;
            parts = new int[1];
            parts[0] = 0;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get or set Points
        /// </summary>
        public List<PointD> Points
        {
            get { return _points; }
            set 
            { 
                _points = value;
                Extent = MIMath.GetPointsExtent(_points);
                UpdatePolyLines();
            }
        }

        /// <summary>
        /// Get or set part number
        /// </summary>
        public int PartNum
        {
            get { return _numParts; }
            set { _numParts = value; }
        }

        /// <summary>
        /// Get Point number
        /// </summary>
        public int PointNum
        {
            get { return _points.Count; }
        }

        /// <summary>
        /// Get or set polylines
        /// </summary>
        public List<PolyLine> PolyLines
        {
            get { return _polyLines; }
            set
            {
                _polyLines = value;
                UpdatePartsPoints();
            }
        }

        /// <summary>
        /// Get if is closed
        /// </summary>
        public bool IsClosed
        {
            get
            {
                bool isClosed = false;
                if (MIMath.DoubleEquals(_points[0].X, _points[_points.Count - 1].X) &&
                    MIMath.DoubleEquals(_points[0].Y, _points[_points.Count - 1].Y))
                    isClosed = true;

                return isClosed;
            }
        }

        /// <summary>
        /// Get length
        /// </summary>
        public double Length
        {
            get
            {
                double length = 0.0;
                double dx, dy;
                foreach (PolyLine aPL in _polyLines)
                {
                    for (int i = 0; i < aPL.PointList.Count - 1; i++)
                    {
                        dx = aPL.PointList[i + 1].X - aPL.PointList[i].X;
                        dy = aPL.PointList[i + 1].Y - aPL.PointList[i].Y;
                        length += Math.Sqrt(dx * dx + dy * dy);
                    }
                }

                return length;
            }
        }

        #endregion

        #region Methods  
        private void UpdatePolyLines()
        {
            _polyLines = new List<PolyLine>();
            if (_numParts == 1)
            {
                PolyLine aPolyLine = new PolyLine();
                aPolyLine.PointList = Points;
                _polyLines.Add(aPolyLine);
            }
            else
            {
                PointD[] Pointps;
                PolyLine aPolyLine = null;
                int numPoints = this._points.Count;
                for (int p = 0; p < _numParts; p++)
                {
                    if (p == _numParts - 1)
                    {
                        Pointps = new PointD[numPoints - parts[p]];
                        for (int pp = parts[p]; pp < numPoints; pp++)
                        {
                            Pointps[pp - parts[p]] = Points[pp];
                        }
                    }
                    else
                    {
                        Pointps = new PointD[parts[p + 1] - parts[p]];
                        for (int pp = parts[p]; pp < parts[p + 1]; pp++)
                        {
                            Pointps[pp - parts[p]] = Points[pp];
                        }
                    }

                    aPolyLine = new PolyLine();
                    aPolyLine.PointList = new List<PointD>(Pointps);
                    _polyLines.Add(aPolyLine);
                }                
            }
        }

        private void UpdatePartsPoints()
        {
            _numParts = 0;
            _points = new List<PointD>();
            List<int> partList = new List<int>();
            for (int i = 0; i < _polyLines.Count; i++)
            {
                _numParts += 1;
                partList.Add(_points.Count);
                _points.AddRange(_polyLines[i].PointList);
            }
            parts = partList.ToArray();
            Extent = MIMath.GetPointsExtent(_points);
        }

        /// <summary>
        /// Override get points method
        /// </summary>
        /// <returns>points</returns>
        public override List<PointD> GetPoints()
        {
            return _points;
        }

        /// <summary>
        /// Override set points
        /// </summary>
        /// <param name="points">points</param>
        public override void SetPoints(List<PointD> points)
        {
            Points = points;
        }

        /// <summary>
        /// Clone polylineshape
        /// </summary>
        /// <returns>PolylineShape</returns>
        public override object Clone()
        {
            PolylineShape aPLS = new PolylineShape();
            aPLS.value = value;
            aPLS.Extent = Extent;
            aPLS._numParts = _numParts;
            aPLS.parts = (int[])parts.Clone();
            aPLS.Points = new List<PointD>(Points);
            aPLS.Visible = Visible;
            aPLS.Selected = Selected;
            aPLS.LegendIndex = LegendIndex;

            return aPLS;
        }

        /// <summary>
        /// Value clone
        /// </summary>
        /// <returns>new polyline shape</returns>
        public PolylineShape ValueClone()
        {
            PolylineShape aPLS = new PolylineShape();
            aPLS.value = value;            
            aPLS.Visible = Visible;
            aPLS.Selected = Selected;
            aPLS.LegendIndex = LegendIndex;

            return aPLS;
        }

        #endregion
    }
}
