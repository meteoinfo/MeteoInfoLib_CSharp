using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteoInfoC.Global
{
    /// <summary>
    /// Extent
    /// </summary>
    public struct Extent
    {
        #region Variable
        /// <summary>
        /// minimun x
        /// </summary>
        public double minX;
        /// <summary>
        /// maximum x
        /// </summary>
        public double maxX;
        /// <summary>
        /// minimum y
        /// </summary>
        public double minY;
        /// <summary>
        /// maximum y
        /// </summary>
        public double maxY;

        #endregion
       
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        public Extent(double xMin, double xMax, double yMin, double yMax)
        {
            minX = xMin;
            maxX = xMax;
            minY = yMin;
            maxY = yMax;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get width
        /// </summary>
        public double Width
        {
            get { return maxX - minX; }
        }

        /// <summary>
        /// Get height
        /// </summary>
        public double Height
        {
            get { return maxY - minY; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Judge if this extent include another extent
        /// </summary>
        /// <param name="bExtent">extent</param>
        /// <returns>is included</returns>
        public bool Include(Extent bExtent)
        {
            if (minX <= bExtent.minX && maxX >= bExtent.maxX && minY <= bExtent.minY && maxY >= bExtent.maxY)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Convert to rectangle
        /// </summary>
        /// <returns>rectangle</returns>
        public Rectangle ConvertToRectangle()
        {
            return new Rectangle((int)minX, (int)minY, (int)Width, (int)Height);
        }

        /// <summary>
        /// Get center point
        /// </summary>
        /// <returns>Center point</returns>
        public PointD GetCenterPoint()
        {
            return new PointD((maxX - minX) / 2 + minX, (maxY - minY) / 2 + minY);
        }

        /// <summary>
        /// Shift extent
        /// </summary>
        /// <param name="dx">X shift value</param>
        /// <param name="dy">Y shift value</param>
        /// <returns>Shifted extent</returns>
        public Extent Shift(double dx, double dy)
        {
            return new Extent(minX + dx, maxX + dx, minY + dy, maxY + dy);
        }

        #endregion

    }
}
