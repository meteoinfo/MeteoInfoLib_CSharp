using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Map
{
    /// <summary>
    /// Set map screen parameters
    /// </summary>
    public class XYScreenSet
    {
        #region Variables

        private double m_minX;
        private double m_maxX;
        private double m_minY;
        private double m_maxY;
        private double m_scaleX;
        private double m_scaleY;
        private int m_XLBorderSpace;
        private int m_XRBorderSpace;
        private int m_YTBorderSpace;
        private int m_YBBorderSpace;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public XYScreenSet()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        /// <param name="xlbs"></param>
        /// <param name="xrbs"></param>
        /// <param name="ytbs"></param>
        /// <param name="ybbs"></param>
        public XYScreenSet(double xMin, double xMax, double yMin, double yMax,
            int xlbs, int xrbs, int ytbs, int ybbs)
        {
            m_minX = xMin;
            m_maxX = xMax;
            m_minY = yMin;
            m_maxY = yMax;
            m_XLBorderSpace = xlbs;
            m_XRBorderSpace = xrbs;
            m_YTBorderSpace = ytbs;
            m_YBBorderSpace = ybbs;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get or set minimum X
        /// </summary>
        public double MinX
        {
            get { return m_minX; }
            set { m_minX = value; }
        }

        /// <summary>
        /// Get or set maximum X
        /// </summary>
        public double MaxX
        {
            get { return m_maxX; }
            set { m_maxX = value; }
        }

        /// <summary>
        /// Get or set minimum y
        /// </summary>
        public double MinY
        {
            get { return m_minY; }
            set { m_minY = value; }
        }

        /// <summary>
        /// Get or set maximum y
        /// </summary>
        public double MaxY
        {
            get { return m_maxY; }
            set { m_maxY = value; }
        }

        /// <summary>
        /// Get or set X scale
        /// </summary>
        public double ScaleX
        {
            get { return m_scaleX; }
            set { m_scaleX = value; }
        }

        /// <summary>
        /// Get or set y scale
        /// </summary>
        public double ScaleY
        {
            get { return m_scaleY; }
            set { m_scaleY = value; }
        }

        /// <summary>
        /// Get or set left border space
        /// </summary>
        public int XLBorderSpace
        {
            get { return m_XLBorderSpace; }
            set { m_XLBorderSpace = value; }
        }

        /// <summary>
        /// Get or set right border space
        /// </summary>
        public int XRBorderSpace
        {
            get { return m_XRBorderSpace; }
            set { m_XRBorderSpace = value; }
        }

        /// <summary>
        /// Get or set top border space
        /// </summary>
        public int YTBorderSpace
        {
            get { return m_YTBorderSpace; }
            set { m_YTBorderSpace = value; }
        }

        /// <summary>
        /// Get or set bottom border space
        /// </summary>
        public int YBBorderSpace
        {
            get { return m_YBBorderSpace; }
            set { m_YBBorderSpace = value; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Set map screen parameters for geological map
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isLonLatMap"></param>
        public void SetCoordinateGeoMap(double xMin, double xMax, double yMin, double yMax,
            int width, int height, bool isLonLatMap)
        {
            m_minX = xMin;
            m_maxX = xMax;
            m_minY = yMin;
            m_maxY = yMax;
            double lonRan, latRan;

            if (width <= m_XLBorderSpace + m_XRBorderSpace)
            {
                width = m_XLBorderSpace + m_XRBorderSpace + 100;
            }
            if (height <= m_YBBorderSpace + m_YTBorderSpace)
            {
                height = m_YBBorderSpace + m_YTBorderSpace + 100;
            }
            m_scaleX = (width - m_XLBorderSpace - m_XRBorderSpace) / (m_maxX - m_minX);
            m_scaleY = (height - m_YTBorderSpace - m_YBBorderSpace) / (m_maxY - m_minY);
            if (isLonLatMap)
            {
                if (m_scaleX > m_scaleY)
                {
                    m_scaleX = m_scaleY / 1.2;
                    m_minX = m_maxX - (width - m_XLBorderSpace - m_XRBorderSpace) / m_scaleX;
                }
                else
                {
                    m_scaleY = m_scaleX * 1.2;
                    m_minY = m_maxY - (height - m_YTBorderSpace - m_YBBorderSpace) / m_scaleY;
                }
            }
            else
            {
                if (m_scaleX > m_scaleY)
                {
                    m_scaleX = m_scaleY;
                    lonRan = m_maxX - m_minX;
                    m_minX = m_maxX - (width - m_XLBorderSpace - m_XRBorderSpace) / m_scaleX;
                    lonRan = (m_maxX - m_minX - lonRan) / 2;
                    m_minX = m_minX + lonRan;
                    m_maxX = m_maxX + lonRan;
                }
                else
                {
                    m_scaleY = m_scaleX;
                    latRan = m_maxY - m_minY;
                    m_minY = m_maxY - (height - m_YTBorderSpace - m_YBBorderSpace) / m_scaleY;
                    latRan = (m_maxY - m_minY - latRan) / 2;
                    m_minY = m_minY + latRan;
                    m_maxY = m_maxY + latRan;
                }
            }

        }

        /// <summary>
        /// Set map screen parameters for normal map
        /// </summary>
        /// <param name="lonMin"></param>
        /// <param name="lonMax"></param>
        /// <param name="latMin"></param>
        /// <param name="latMax"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetCoordinateMap(double lonMin, double lonMax, double latMin, double latMax,
            int width, int height)
        {
            if (lonMax <= lonMin || latMax <= latMin)
            {
                return;
            }
            m_minX = lonMin;
            m_maxX = lonMax;
            m_minY = latMin;
            m_maxY = latMax;

            m_scaleX = (width - m_XLBorderSpace - m_XRBorderSpace) / (m_maxX - m_minX);
            m_scaleY = (height - m_YTBorderSpace - m_YBBorderSpace) / (m_maxY - m_minY);
        }

        /// <summary>
        /// Convert coordinate from map to screen
        /// </summary>
        /// <param name="projX"></param>
        /// <param name="projY"></param>
        /// <param name="screenX"></param>
        /// <param name="screenY"></param>
        /// <param name="LonShift"></param>
        /// <param name="aLLSS"></param>
        public void ProjToScreen(double projX, double projY, ref Single screenX, ref Single screenY,
            double LonShift, XYScreenSet aLLSS)
        {            
            screenX = Convert.ToSingle((projX + LonShift - aLLSS.MinX) * aLLSS.ScaleX + aLLSS.XLBorderSpace);
            screenY = Convert.ToSingle((aLLSS.MaxY - projY) * aLLSS.ScaleY + aLLSS.YTBorderSpace);            
        }

        /// <summary>
        /// convert coordinate from screen to map
        /// </summary>
        /// <param name="projX"></param>
        /// <param name="projY"></param>
        /// <param name="screenX"></param>
        /// <param name="screenY"></param>
        /// <param name="aLLSS"></param>
        public void ScreenToProj(ref double projX, ref double projY, double screenX, double screenY,
            XYScreenSet aLLSS)
        {
            projX = (screenX - aLLSS.XLBorderSpace) / aLLSS.ScaleX + aLLSS.MinX;
            projY = aLLSS.MaxY - (screenY - aLLSS.YTBorderSpace) / aLLSS.ScaleY;
        }
        #endregion
    }
}
