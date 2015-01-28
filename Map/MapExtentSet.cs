using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Map
{
    /// <summary>
    /// Map extent and X/y scale set
    /// </summary>
    public class MapExtentSet
    {
        #region Variables

        private double m_minX;
        private double m_maxX;
        private double m_minY;
        private double m_maxY;
        private double m_scaleX;
        private double m_scaleY;
        private double m_XYScaleFactor;
        #endregion

        #region Constructor       

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>       
        public MapExtentSet(double xMin, double xMax, double yMin, double yMax)
        {
            m_minX = xMin;
            m_maxX = xMax;
            m_minY = yMin;
            m_maxY = yMax;
            m_scaleX = 1;
            m_scaleY = 1;
            m_XYScaleFactor = 1;
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
        /// Get or set X/Y scale factor
        /// </summary>
        public double XYScaleFactor
        {
            get { return m_XYScaleFactor; }
            set { m_XYScaleFactor = value; }
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
            double lonRan, latRan, temp;
            
            m_scaleX = width / (m_maxX - m_minX);
            m_scaleY = height / (m_maxY - m_minY);
            if (isLonLatMap)
            {
                m_XYScaleFactor = 1.2;
            }
            else
            {
                m_XYScaleFactor = 1;
            }

            if (m_scaleX > m_scaleY)
            {
                m_scaleX = m_scaleY / m_XYScaleFactor;
                temp = m_minX;
                m_minX = m_maxX - width / m_scaleX;
                lonRan = (m_minX - temp) / 2;
                m_minX = m_minX - lonRan;
                m_maxX = m_maxX - lonRan;
            }
            else
            {
                m_scaleY = m_scaleX * m_XYScaleFactor;
                temp = m_minY;
                m_minY = m_maxY - height / m_scaleY;
                latRan = (m_minY - temp) / 2;
                m_minY = m_minY - latRan;
                m_maxY = m_maxY - latRan;
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

            m_scaleX = width / (m_maxX - m_minX);
            m_scaleY = height / (m_maxY - m_minY);
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
            double LonShift, MapExtentSet aLLSS)
        {            
            screenX = Convert.ToSingle((projX + LonShift - aLLSS.MinX) * aLLSS.ScaleX);
            screenY = Convert.ToSingle((aLLSS.MaxY - projY) * aLLSS.ScaleY);            
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
            MapExtentSet aLLSS)
        {
            projX = screenX / aLLSS.ScaleX + aLLSS.MinX;
            projY = aLLSS.MaxY - screenY / aLLSS.ScaleY;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>map extent set</returns>
        public MapExtentSet Clone()
        {
            MapExtentSet aMapES = new MapExtentSet(m_minX, m_maxX, m_minY, m_maxY);
            aMapES.MinX = m_minX;
            aMapES.MaxX = m_maxX;
            aMapES.MinY = m_minY;
            aMapES.MaxY = m_maxY;
            aMapES.ScaleX = m_scaleX;
            aMapES.m_scaleY = m_scaleY;

            return aMapES;
        }
        #endregion
    }
}
