using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC
{
    /// <summary>
    /// double point class
    /// </summary>
    public class PointD
    {
        #region Varables
        /// <summary>
        /// X
        /// </summary>
        public double X;
        /// <summary>
        /// Y
        /// </summary>
        public double Y;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PointD()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>PointD object</returns>
        public object Clone()
        {
            PointD aP = new PointD();
            aP.X = X;
            aP.Y = Y;
            return aP;
        }

        #endregion
    }
}
