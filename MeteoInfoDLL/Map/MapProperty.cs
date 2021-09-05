using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace MeteoInfoC.Map
{
    /// <summary>
    /// Set map property
    /// </summary>
    public class MapProperty
    {
        #region Variables

        /// <summary>
        /// MapView object
        /// </summary>
        public MapView MapControl;

        private SmoothingMode m_SmoothingMode;               
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aMapView"></param>
        public MapProperty(MapView aMapView)
        {
            MapControl = aMapView;             
            m_SmoothingMode = SmoothingMode.Default;            
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get or set smoothing mode of GDI+
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set graph smoothing mode")]
        public SmoothingMode SmoothingMode
        {
            get
            {
                return m_SmoothingMode;
            }
            set
            {
                if (value != SmoothingMode.Invalid)
                {
                    m_SmoothingMode = value;
                    MapControl.PaintLayers();
                }
            }
        }

        /// <summary>
        /// Get or set fore color of map view
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set map fore color")]
        public Color ForeColor
        {
            get
            {
                return MapControl.ForeColor;
            }
            set
            {
                MapControl.ForeColor = value;
                MapControl.PaintLayers();
            }
        }

        /// <summary>
        /// Get or set back color of map region
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set map back color")]
        public Color BackColor
        {
            get
            {                
                return MapControl.BackColor;
            }
            set
            {                
                MapControl.BackColor = value;
                if (MapControl.BackColor == Color.Black)
                {
                    MapControl.ForeColor = Color.White;
                    MapControl.NeatLineColor = Color.White;
                    MapControl.GridLineColor = Color.White;
                }
                else if (MapControl.BackColor == Color.White)
                {
                    MapControl.ForeColor = Color.Black;
                    MapControl.NeatLineColor = Color.Black;
                    MapControl.GridLineColor = Color.Black;
                }
                MapControl.PaintLayers();
            }
        }        
        
        #endregion

        #region Methods
        

        #endregion
    }
}
