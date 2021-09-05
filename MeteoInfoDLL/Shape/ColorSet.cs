using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using MeteoInfoC.Legend;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Color set
    /// </summary>
    public class ColorSet
    {
        #region Variables
        private Color _LColor;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aCB">color break</param>
        public ColorSet(ColorBreak aCB)
        {
            _LColor = aCB.Color;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or set point color
        /// </summary>
        [CategoryAttribute("Point Set"), DescriptionAttribute("Set point color")]
        public Color LColor
        {
            get
            {
                return _LColor;
            }
            set
            {
                _LColor = value;
                frmLegendSet.pCurrenWin.SetLegendBreak_Color(_LColor);
            }
        }

        #endregion
    }
}
