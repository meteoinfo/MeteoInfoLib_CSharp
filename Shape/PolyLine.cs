using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteoInfoC.Global;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Polyline
    /// </summary>
    public class PolyLine
    {
        #region Variables
        private Extent _extent;
        private List<PointD> _pointList;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PolyLine()
        {
            _extent = new Extent();
            _pointList = new List<PointD>();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set point list
        /// </summary>
        public List<PointD> PointList
        {
            get { return _pointList; }
            set
            {
                _pointList = value;
                _extent = MIMath.GetPointsExtent(_pointList);
            }
        }

        /// <summary>
        /// Get extent
        /// </summary>
        public Extent Extent
        {
            get { return _extent; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Set points
        /// </summary>
        /// <param name="points">points</param>
        public void SetPoints(PointF[] points)
        {
            _pointList = new List<PointD>();
            foreach (PointF aP in points)
                _pointList.Add(new PointD(aP.X, aP.Y));

            _extent = MIMath.GetPointsExtent(_pointList);
        }

        /// <summary>
        /// Determine if the polyline is closed
        /// </summary>
        /// <returns>if is closed</returns>
        public bool IsClosed()
        {
            PointD sPoint = _pointList[0];
            PointD ePoint = _pointList[_pointList.Count - 1];
            if (MIMath.DoubleEquals(sPoint.X, ePoint.X) && MIMath.DoubleEquals(sPoint.Y, ePoint.Y))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get length
        /// </summary>
        /// <returns>length</returns>
        public double GetLength()
        {
            double length = 0.0;
            double dx, dy;
            for (int i = 0; i < _pointList.Count - 1; i++)
            {
                dx = _pointList[i + 1].X - _pointList[i].X;
                dy = _pointList[i + 1].Y - _pointList[i].Y;
                length += Math.Sqrt(dx * dx + dy * dy);
            }

            return length;
        }

        /// <summary>
        /// Get lengths of each segment
        /// </summary>
        /// <returns>length array</returns>
        public double[] GetLengths()
        {
            double[] lengths = new double[_pointList.Count - 1];
            double dx, dy;
            for (int i = 0; i < _pointList.Count - 1; i++)
            {
                dx = _pointList[i + 1].X - _pointList[i].X;
                dy = _pointList[i + 1].Y - _pointList[i].Y;
                lengths[i] = Math.Sqrt(dx * dx + dy * dy);
            }

            return lengths;
        }

        /// <summary>
        /// Get position list: x, y, angle
        /// </summary>
        /// <param name="aLen">segment length</param>
        /// <returns>position list</returns>
        public List<double[]> GetPositions(double aLen)
        {
            double length = GetLength();
            int n = (int)(length / aLen);
            if (n <= 0)
                return null;

            List<double[]> pos = new List<double[]>();
            double x, y, angle;
            double[] lengths = GetLengths();
            int idx = 0;
            double sLen = lengths[0];
            for (int i = 0; i < n; i++)
            {
                double len = aLen * (i + 1);
                for (int j = idx; j < lengths.Length; j++)
                {                    
                    if (sLen > len)
                    {
                        idx = j;
                        break;
                    }
                    sLen += lengths[j + 1];
                }

                PointD aPoint = _pointList[idx];
                PointD bPoint = _pointList[idx + 1];
                x = aPoint.X + (bPoint.X - aPoint.X) * (lengths[idx] - (sLen - len)) / (lengths[idx]);
                y = aPoint.Y + (bPoint.Y - aPoint.Y) * (lengths[idx] - (sLen - len)) / (lengths[idx]);
                double U = bPoint.X - aPoint.X;
                double V = bPoint.Y - aPoint.Y;
                angle = Math.Atan((V) / (U)) * 180 / Math.PI;
                if (U < 0)
                    angle += 180;
                //if (angle < 0)
                //{
                //    if (bPoint.Y > aPoint.Y)
                //        angle += 180;
                //}
                //angle = MIMath.GetWindDirection(U, V);
                double[] data = new double[] { x, y, angle };
                pos.Add(data);
            }

            return pos;
        }

        #endregion
    }
}
