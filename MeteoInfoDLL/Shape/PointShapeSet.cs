using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using MeteoInfoC.Legend;
using MeteoInfoC.Drawing;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Point shape set
    /// </summary>
    public class PointShapeSet
    {
        #region Variables

        private Color m_PointColor;
        private Single m_PointSize;
        private PointStyle m_PointStyle;
        private Color m_OutlineColor;
        private Boolean m_DrawOutline;
        private Boolean m_DrawFill;
        private Boolean m_DrawPoint;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aPB"></param>
        public PointShapeSet(PointBreak aPB)
        {
            m_PointColor = aPB.Color;
            m_PointSize = aPB.Size;
            m_PointStyle = aPB.Style;
            m_OutlineColor = aPB.OutlineColor;
            m_DrawOutline = aPB.DrawOutline;
            m_DrawFill = aPB.DrawFill;
            m_DrawPoint = aPB.DrawShape;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get or set point color
        /// </summary>
        [CategoryAttribute("Point Set"), DescriptionAttribute("Set point color")]
        public Color PointColor
        {
            get
            {
                return m_PointColor;
            }
            set
            {
                m_PointColor = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_Color(m_PointColor);
            }
        }

        /// <summary>
        /// Get or set point size
        /// </summary>
        [CategoryAttribute("Point Set"), DescriptionAttribute("Set point size")]
        public Single PointSize
        {
            get
            {
                return m_PointSize;
            }
            set
            {
                m_PointSize = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_Size(m_PointSize);
            }
        }

        /// <summary>
        /// Get or set point style
        /// </summary>
        [CategoryAttribute("Point Set"), DescriptionAttribute("Set point style")]
        public PointStyle PointStyle
        {
            get
            {
                return m_PointStyle;
            }
            set
            {
                m_PointStyle = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_PointStyle(m_PointStyle);
            }
        }

        /// <summary>
        /// Get or set point outline color
        /// </summary>
        [CategoryAttribute("Point Set"), DescriptionAttribute("Set point outline color")]
        public Color PointOutlineColor
        {
            get
            {
                return m_OutlineColor;
            }
            set
            {
                m_OutlineColor = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_OutlineColor(m_OutlineColor);
            }
        }

        /// <summary>
        /// Get or set if draw point outline
        /// </summary>
        [CategoryAttribute("Point Set"), DescriptionAttribute("Set point outline draw option")]
        public Boolean DrawOutline
        {
            get
            {
                return m_DrawOutline;
            }
            set
            {
                m_DrawOutline = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_DrawOutline(m_DrawOutline);
            }
        }

        /// <summary>
        /// Get or set if draw fill
        /// </summary>
        [CategoryAttribute("Point Set"), DescriptionAttribute("Set point fill draw option")]
        public Boolean DrawFill
        {
            get
            {
                return m_DrawFill;
            }
            set
            {
                m_DrawFill = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_DrawFill(m_DrawFill);
            }
        }

        /// <summary>
        /// Get or set if draw point
        /// </summary>
        [CategoryAttribute("Point Set"), DescriptionAttribute("Set point draw option")]
        public Boolean DrawPoint
        {
            get
            {
                return m_DrawPoint;
            }
            set
            {
                m_DrawPoint = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_DrawShape(m_DrawPoint);
            }
        }
        #endregion
    }
}
