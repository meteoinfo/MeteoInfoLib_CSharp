using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Point M class
    /// </summary>
    public class PointM:PointD
    {
        #region Varables
        /// <summary>
        /// Measure
        /// </summary>
        public double M;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PointM()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="m">m</param>
        public PointM(double x, double y, double m)
        {
            X = x;
            Y = y;
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
