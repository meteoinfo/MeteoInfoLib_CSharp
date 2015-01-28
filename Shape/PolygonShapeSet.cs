using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using MeteoInfoC.Legend;
using MeteoInfoC.Drawing;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Polygon shape set
    /// </summary>
    public class PolygonShapeSet
    {
        #region Variables

        private Color m_Color;
        private Color m_OutlineColor;
        private Boolean m_DrawOutline;
        private Boolean m_DrawFill;
        private Boolean m_DrawPolygon;
        private Single m_OutlineSize;
        private bool _usingHatchStyle;
        private HatchStyle _style;
        private Color _backColor;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aPGB"></param>
        public PolygonShapeSet(PolygonBreak aPGB)
        {
            m_Color = aPGB.Color;
            m_OutlineColor = aPGB.OutlineColor;
            m_DrawOutline = aPGB.DrawOutline;
            m_DrawFill = aPGB.DrawFill;
            m_DrawPolygon = aPGB.DrawShape;
            m_OutlineSize = aPGB.OutlineSize;
            _usingHatchStyle = aPGB.UsingHatchStyle;
            _style = aPGB.Style;
            _backColor = aPGB.BackColor;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get or set polygon color
        /// </summary>
        [CategoryAttribute("Polygon Set"), DescriptionAttribute("Set polygon color")]
        public Color PolygonColor
        {
            get
            {
                return m_Color;
            }
            set
            {
                m_Color = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_Color(m_Color);
            }
        }

        /// <summary>
        /// Get or set polygon outline color
        /// </summary>
        [CategoryAttribute("Polygon Set"), DescriptionAttribute("Set polygon outline color")]
        public Color OutlineColor
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
        /// Get or set if draw outline
        /// </summary>
        [CategoryAttribute("Polygon Set"), DescriptionAttribute("Set polygon outline draw option")]
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
        [CategoryAttribute("Polygon Set"), DescriptionAttribute("Set polygon fill draw option")]
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
        /// Get or set if draw polygon
        /// </summary>
        [CategoryAttribute("Polygon Set"), DescriptionAttribute("Set polygon draw option")]
        public Boolean DrawPolygon
        {
            get
            {
                return m_DrawPolygon;
            }
            set
            {
                m_DrawPolygon = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_DrawShape(m_DrawPolygon);
            }
        }

        /// <summary>
        /// Get or set outline size
        /// </summary>
        [CategoryAttribute("Polygon Set"), DescriptionAttribute("Set polygon outline size")]
        public Single OutlineSize
        {
            get
            {
                return m_OutlineSize;
            }
            set
            {
                m_OutlineSize = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_Size(m_OutlineSize);
            }
        }

        /// <summary>
        /// Get or set if using hatch style
        /// </summary>
        [CategoryAttribute("Polygon Set"), DescriptionAttribute("Set polygon if using hatch style")]
        public bool UsingHatchStyle
        {
            get
            {
                return _usingHatchStyle;
            }
            set
            {
                _usingHatchStyle = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_UsingHatchStyle(_usingHatchStyle);
            }
        }

        /// <summary>
        /// Get or set hatch style
        /// </summary>
        [CategoryAttribute("Polygon Set"), DescriptionAttribute("Set polygon hatch style")]
        public HatchStyle Style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_HatchStyle(_style);
            }
        }

        /// <summary>
        /// Get or set hatch style
        /// </summary>
        [CategoryAttribute("Polygon Set"), DescriptionAttribute("Set polygon hatch style")]
        public Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_BackColor(_backColor);
            }
        }

        #endregion
    }
}
