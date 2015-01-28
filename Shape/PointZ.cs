using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Point Z class
    /// </summary>
    public class PointZ:PointD
    {
        #region Varables
        /// <summary>
        /// Z coordinate
        /// </summary>
        public double Z;
        /// <summary>
        /// Measure
        /// </summary>
        public double M;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PointZ()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="m">m</param>
        /// <param name="z">z</param>
        public PointZ(double x, double y, double z, double m)
        {
            X = x;
            Y = y;
            Z = z;
            M = m;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Convert to PointD
        /// </summary>
        /// <returns>PointD</returns>
        public PointD ToPointD()
        {
            return new PointD(X, Y);
        }

        #endregion
    }
}
