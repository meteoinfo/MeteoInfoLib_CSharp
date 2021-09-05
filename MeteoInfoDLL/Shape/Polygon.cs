using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Geoprocess;
using MeteoInfoC.Global;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Polygon
    /// </summary>
    public class Polygon
    {
        #region Variables
        private List<PointD> _outLine;
        private List<List<PointD>> _holeLines;
        private Extent _extent;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Polygon()
        {
            _outLine = new List<PointD>();
            _holeLines = new List<List<PointD>>();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set outline
        /// </summary>
        public List<PointD> OutLine
        {
            get { return _outLine; }
            set 
            { 
                _outLine = value;
                _extent = MIMath.GetPointsExtent(_outLine);
            }
        }

        /// <summary>
        /// Get or set hole lines
        /// </summary>
        public List<List<PointD>> HoleLines
        {
            get { return _holeLines; }
            set { _holeLines = value; }
        }

        /// <summary>
        /// Get extent
        /// </summary>
        public Extent Extent
        {
            get { return _extent; }
        }

        /// <summary>
        /// Get rings
        /// </summary>
        public List<List<PointD>> Rings
        {
            get
            {
                List<List<PointD>> rings = new List<List<PointD>>();
                rings.Add(_outLine);
                if (HasHole)
                    rings.AddRange(_holeLines);

                return rings;
            }
        }

        /// <summary>
        /// Get if has hole
        /// </summary>
        public bool HasHole
        {
            get { return (_holeLines.Count > 0); }
        }

        /// <summary>
        /// Get ring number
        /// </summary>
        public int RingNumber
        {
            get { return HoleLines.Count + 1; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Add a hole line
        /// </summary>
        /// <param name="points">point list</param>
        public void AddHole(List<PointD> points)
        {
            if (GeoComputation.IsClockwise(points))
                points.Reverse();
            _holeLines.Add(points);
        }

        #endregion
    }
}
