using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteoInfoC.Global;

namespace MeteoInfoC.Geoprocess
{
    /// <summary>
    /// Spline class
    /// </summary>
    public static class Spline
    {
        private class Vec2
        {
            public double X, Y;
            public Vec2(double x, double y) { this.X = x; this.Y = y; }
            public static implicit operator PointD(Vec2 v) { return new PointD(v.X, v.Y); }
            public static implicit operator Vec2(PointD p) { return new Vec2(p.X, p.Y); }
            public static Vec2 operator +(Vec2 v1, Vec2 v2) { return new Vec2(v1.X + v2.X, v1.Y + v2.Y); }
            public static Vec2 operator -(Vec2 v1, Vec2 v2) { return new Vec2(v1.X - v2.X, v1.Y - v2.Y); }
            public static Vec2 operator *(Vec2 v, float f) { return new Vec2(v.X * f, v.Y * f); }
            public static Vec2 operator /(Vec2 v, float f) { return new Vec2(v.X / f, v.Y / f); }
        }

        /// <summary>
        /// '贝塞尔'内插。结果不包括头尾点
        /// </summary>
        private static PointD[] InterpolateBezier(PointD p0, PointD p1, PointD p2, PointD p3, int samples)
        {
            PointD[] result = new PointD[samples];
            for (int i = 0; i < samples; i++)
            {
                float t = (i + 1) / (samples + 1.0f);
                result[i] =
                    (Vec2)p0 * (1 - t) * (1 - t) * (1 - t) +
                    (Vec2)p1 * (3 * (1 - t) * (1 - t) * t) +
                    (Vec2)p2 * (3 * (1 - t) * t * t) +
                    (Vec2)p3 * (t * t * t);
            }
            return result;
        }

        /// <summary>
        /// Interpolate Cardinal Spline
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="samples"></param>
        /// <returns></returns>
        private static PointD[] InterpolateCardinalSpline(PointD p0, PointD p1, PointD p2, PointD p3, int samples)
        {
            const float tension = 0.5f;
            Vec2 u = ((Vec2)p2 - (Vec2)p0) * (tension / 3) + p1;
            Vec2 v = ((Vec2)p1 - (Vec2)p3) * (tension / 3) + p2;
            return InterpolateBezier(p1, u, v, p2, samples);
        }

        /// <summary>
        /// '基数样条'内插法。 points为通过点，samplesInSegment为两个样本点之间的内插数量。
        /// </summary>
        /// <param name="points">points</param>
        /// <param name="samplesInSegment">samples in segment</param>
        /// <returns>result points</returns>
        public static PointD[] CardinalSpline(PointD[] points, int samplesInSegment)
        {
            List<PointD> result = new List<PointD>();
            for (int i = 0; i < points.Length - 1; i++)
            {
                result.Add(points[i]);
                result.AddRange(InterpolateCardinalSpline(
                    points[Math.Max(i - 1, 0)],
                    points[i],
                    points[i + 1],
                    points[Math.Min(i + 2, points.Length - 1)],
                    samplesInSegment
                    ));
            }
            result.Add(points[points.Length - 1]);
            return result.ToArray();
        }

        #region Calculation of Spline

        private static void CalcCurve(PointD[] pts, double tenstion, out PointD p1, out PointD p2)
        {
            double deltaX, deltaY;
            deltaX = pts[2].X - pts[0].X;
            deltaY = pts[2].Y - pts[0].Y;
            p1 = new PointD((pts[1].X - tenstion * deltaX), (pts[1].Y - tenstion * deltaY));
            p2 = new PointD((pts[1].X + tenstion * deltaX), (pts[1].Y + tenstion * deltaY));
        }

        private static void CalcCurveEnd(PointD end, PointD adj, double tension, out PointD p1)
        {
            p1 = new PointD(((tension * (adj.X - end.X) + end.X)), ((tension * (adj.Y - end.Y) + end.Y)));
        }

        /// <summary>
        /// Cardinal spline calculation
        /// </summary>
        /// <param name="pts">point list</param>
        /// <param name="t">tension</param>
        /// <param name="closed">is closed</param>
        /// <returns>result points</returns>
        public static PointD[] CardinalSpline(List<PointD> pts, double t, bool closed)
        {
            int i, nrRetPts;
            PointD p1, p2;
            double tension = t * (1d / 3d); //we are calculating contolpoints.

            if (closed)
                nrRetPts = (pts.Count + 1) * 3 - 2;
            else
                nrRetPts = pts.Count * 3 - 2;

            PointD[] retPnt = new PointD[nrRetPts];
            for (i = 0; i < nrRetPts; i++)
                retPnt[i] = new PointD();

            if (!closed)
            {
                CalcCurveEnd(pts[0], pts[1], tension, out p1);
                retPnt[0] = pts[0];
                retPnt[1] = p1;
            }
            for (i = 0; i < pts.Count - (closed ? 1 : 2); i++)
            {
                CalcCurve(new PointD[] { pts[i], pts[i + 1], pts[(i + 2) % pts.Count] }, tension, out  p1, out p2);
                retPnt[3 * i + 2] = p1;
                retPnt[3 * i + 3] = pts[i + 1];
                retPnt[3 * i + 4] = p2;
            }
            if (closed)
            {
                CalcCurve(new PointD[] { pts[pts.Count - 1], pts[0], pts[1] }, tension, out p1, out p2);
                retPnt[nrRetPts - 2] = p1;
                retPnt[0] = pts[0];
                retPnt[1] = p2;
                retPnt[nrRetPts - 1] = retPnt[0];
            }
            else
            {
                CalcCurveEnd(pts[pts.Count - 1], pts[pts.Count - 2], tension, out p1);
                retPnt[nrRetPts - 2] = p1;
                retPnt[nrRetPts - 1] = pts[pts.Count - 1];
            }
            return retPnt;
        }

        #endregion
    }
}
