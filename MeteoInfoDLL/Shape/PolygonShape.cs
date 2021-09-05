using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
//using System.Drawing;
using MeteoInfoC.Global;
using MeteoInfoC.Geoprocess;
using MeteoInfoC.Projections;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Polygon shape
    /// </summary>
    public class PolygonShape:Shape
    {
        #region Variables
        private List<PointD> _points;
        private List<Polygon> _polygons;
        /// <summary>
        /// Start value
        /// </summary>
        public double lowValue;
        /// <summary>
        /// End value
        /// </summary>
        public double highValue;
        /// <summary>
        /// Part number
        /// </summary>
        public int _numParts;
        /// <summary>
        /// Part array
        /// </summary>
        public int[] parts;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PolygonShape()
        {
            ShapeType = ShapeTypes.Polygon;
            _points = new List<PointD>();
            _numParts = 1;
            parts = new int[1];
            parts[0] = 0;
            _polygons = new List<Polygon>();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set points
        /// </summary>
        public List<PointD> Points
        {
            get { return _points; }
            set 
            {
                _points = value;
                Extent = MIMath.GetPointsExtent(_points);
                UpdatePolygons();
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
        /// Get point number
        /// </summary>
        public int PointNum
        {
            get { return _points.Count; }
        }

        /// <summary>
        /// Get or set polygons
        /// </summary>
        public List<Polygon> Polygons
        {
            get{ return _polygons; }
            set
            {
                _polygons = value;
                UpdatePartsPoints();
            }
        }

        /// <summary>
        /// Get area
        /// </summary>
        public double Area
        {
            get
            {
                double area = 0.0;
                foreach (Polygon aPG in _polygons)
                {
                    area += GeoComputation.GetArea(aPG.OutLine);
                    foreach (List<PointD> hole in aPG.HoleLines)
                        area -= GeoComputation.GetArea(hole);
                }

                return area;
            }
        }

        /// <summary>
        /// Get spherical area
        /// </summary>
        public double SphericalArea
        {
            get
            {
                double area = 0.0;
                foreach (Polygon aPG in _polygons)
                {
                    area += GeoComputation.SphericalPolygonArea(aPG.OutLine);
                    foreach (List<PointD> hole in aPG.HoleLines)
                        area -= GeoComputation.SphericalPolygonArea(hole);
                }

                return area;
            }
        }

        #endregion

        #region Methods
       
        private void UpdatePolygons()
        {
            _polygons = new List<Polygon>();
            if (_numParts == 1)
            {
                Polygon aPolygon = new Polygon();
                aPolygon.OutLine = Points;
                _polygons.Add(aPolygon);
            }
            else
            {
                PointD[] Pointps;
                Polygon aPolygon = null;
                for (int p = 0; p < _numParts; p++)
                {
                    if (p == _numParts - 1)
                    {
                        Pointps = new PointD[PointNum - parts[p]];
                        for (int pp = parts[p]; pp < PointNum; pp++)
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

                    if (GeoComputation.IsClockwise(Pointps))
                    {
                        if (p > 0)
                            _polygons.Add(aPolygon);

                        aPolygon = new Polygon();
                        aPolygon.OutLine = new List<PointD>(Pointps);
                    }
                    else
                    {
                        if (aPolygon == null)
                        {
                            Pointps.Reverse();
                            aPolygon = new Polygon();
                            aPolygon.OutLine = new List<PointD>(Pointps);
                        }
                        else
                            aPolygon.AddHole(new List<PointD>(Pointps));
                    }
                }
                _polygons.Add(aPolygon);
            }
        }

        private void UpdatePartsPoints()
        {
            _numParts = 0;
            _points = new List<PointD>();
            List<int> partList = new List<int>();
            for (int i = 0; i < _polygons.Count; i++)
            {
                _numParts += _polygons[i].RingNumber;
                for (int j = 0; j < _polygons[i].RingNumber; j++)
                {
                    partList.Add(_points.Count);
                    _points.AddRange(_polygons[i].Rings[j]);
                }
            }
            parts = partList.ToArray();            
            Extent = MIMath.GetPointsExtent(_points);
        }

        /// <summary>
        /// Add a hole line
        /// </summary>
        /// <param name="points">point list</param>
        /// <param name="polygonIdx">polygon index</param>
        public void AddHole(List<PointD> points, int polygonIdx)
        {
            Polygon aPolygon = _polygons[polygonIdx];
            aPolygon.AddHole(points);
            _polygons[polygonIdx] = aPolygon;

            UpdatePartsPoints();
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
        /// Clone PolygonShape
        /// </summary>
        /// <returns>PolygonShape</returns>
        public override object Clone()
        {
            PolygonShape aPGS = new PolygonShape();
            aPGS.Extent = Extent;
            aPGS.highValue = highValue;
            aPGS.lowValue = lowValue;
            aPGS._numParts = _numParts;
            aPGS.parts = (int[])parts.Clone();
            aPGS.Points = new List<PointD>(Points);
            aPGS.Visible = Visible;
            aPGS.Selected = Selected;
            aPGS.LegendIndex = LegendIndex;

            return aPGS;
        }

        /// <summary>
        /// Clone PolygonShape with values
        /// </summary>
        /// <returns>new polygon shape</returns>
        public PolygonShape ValueClone()
        {
            PolygonShape aPGS = new PolygonShape();
            aPGS.highValue = highValue;
            aPGS.lowValue = lowValue;
            aPGS.Visible = Visible;
            aPGS.Selected = Selected;
            aPGS.LegendIndex = LegendIndex;

            return aPGS;
        }

        /// <summary>
        /// Determine if a point is inside this circle shape
        /// </summary>
        /// <param name="point">The point</param>
        /// <returns>Is inside or not</returns>
        public bool IsPointInside(PointD point)
        {
            bool isIn = false;

            for (int i = 0; i < _polygons.Count; i++)
            {
                Polygon aPRing = _polygons[i];
                isIn = GeoComputation.PointInPolygon(aPRing.OutLine, point);
                if (isIn)
                {
                    if (aPRing.HasHole)
                    {
                        foreach (List<PointD> aLine in aPRing.HoleLines)
                        {
                            if (GeoComputation.PointInPolygon(aLine, point))
                            {
                                isIn = false;
                                break;
                            }
                        }
                    }
                }

                if (isIn)
                    return isIn;
            }

            return isIn;
        }

        #endregion
    }
}
