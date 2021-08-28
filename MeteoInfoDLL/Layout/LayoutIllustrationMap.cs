using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using MeteoInfoC.Global;
using MeteoInfoC.Map;
using MeteoInfoC.Legend;

namespace MeteoInfoC.Layout
{
    /// <summary>
    /// Layout illustration map
    /// </summary>
    public class LayoutIllustrationMap : LayoutMap
    {
        #region Variables
        private Extent _lonLatExtent = new Extent(106.5, 122.5, 1, 23);
        private MapView _linkedMapView = null;

        #endregion

        #region Constructor
        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public LayoutIllustrationMap()
        //    : base()
        //{
        //    ElementType = ElementType.LayoutIllustration;
        //}

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="aMapView">MapView</param>
        //public LayoutIllustrationMap(MapView aMapView)
        //    : base()
        //{
        //    MapView = new MapView();
        //    LinkedMapView = aMapView;            
        //    ElementType = ElementType.LayoutIllustration;
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aMapFrame"></param>
        public LayoutIllustrationMap(MapFrame aMapFrame):base(aMapFrame)
        {
            LinkedMapView = aMapFrame.MapView;
            ElementType = ElementType.LayoutIllustration;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set linded mapview
        /// </summary>
        public MapView LinkedMapView
        {
            get { return _linkedMapView; }
            set
            {
                _linkedMapView = value;
                //MapView.UpdateMapView(_linkedMapView);
            }
        }

        /// <summary>
        /// Get or set visible
        /// </summary>
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                //MapView.UpdateMapView(_linkedMapView);
            }
        }

        /// <summary>
        /// Get or set lon/lat extent
        /// </summary>
        public Extent LonLatExtent
        {
            get { return _lonLatExtent; }
            set { _lonLatExtent = value; }
        }

        /// <summary>
        /// Get or set minimum longitude
        /// </summary>
        public double MinLon
        {
            get { return _lonLatExtent.minX; }
            set { _lonLatExtent.minX = value; }
        }

        /// <summary>
        /// Get or set maximum longitude
        /// </summary>
        public double MaxLon
        {
            get { return _lonLatExtent.maxX; }
            set { _lonLatExtent.maxX = value; }
        }

        /// <summary>
        /// Get or set minimum latitude
        /// </summary>
        public double MinLat
        {
            get { return _lonLatExtent.minY; }
            set { _lonLatExtent.minY = value; }
        }

        /// <summary>
        /// Get or set maximum latitude
        /// </summary>
        public double MaxLat
        {
            get { return _lonLatExtent.maxY; }
            set { _lonLatExtent.maxY = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Override paint method
        /// </summary>
        /// <param name="g">graphics</param>
        public override void Paint(Graphics g)
        {
            if (this.MapFrame != null && this.Visible)
            {
                g.FillRectangle(new SolidBrush(this.MapFrame.BackColor), this.Bounds);
                //Extent orgExtent = this.MapView.ViewExtent;
                Extent aExtent = _lonLatExtent;
                if (!MapFrame.MapView.Projection.IsLonLatMap)
                    aExtent = MapFrame.MapView.Projection.GetProjectedExtentFromLonLat(aExtent);
                this.MapFrame.MapView.ViewExtent = aExtent;

                //Region oldRegion = g.Clip;
                //GraphicsPath path = new GraphicsPath();
                //Rectangle rect = this.Bounds;
                //path.AddRectangle(rect);
                //g.SetClip(path);
                //Matrix oldMatrix = g.Transform;
                //g.TranslateTransform(this.Left, this.Top);

                this.MapFrame.MapView.PaintGraphics(g, this.Bounds);

                //g.Transform = oldMatrix;
                //g.Clip = oldRegion;

                if (MapFrame.DrawNeatLine)
                {
                    Pen aPen = new Pen(MapFrame.NeatLineColor, MapFrame.NeatLineSize);
                    g.DrawRectangle(aPen, this.Bounds);
                }                

                //this.MapView.ViewExtent = orgExtent;               
            }
        }

        /// <summary>
        /// Override PaintOnLayout method
        /// </summary>
        /// <param name="g">graphics</param>
        /// <param name="pageLocation">page location</param>
        /// <param name="zoom">zoom</param>
        public override void PaintOnLayout(Graphics g, PointF pageLocation, float zoom)
        {
            if (this.MapFrame != null && this.Visible)
            {
                PointF aP = PageToScreen(this.Left, this.Top, pageLocation, zoom);
                Rectangle rect = new Rectangle((int)aP.X, (int)aP.Y, (int)(Width * zoom), (int)(Height * zoom));
                g.FillRectangle(new SolidBrush(this.MapFrame.MapView.BackColor), rect);
                //Extent orgExtent = this.MapView.ViewExtent;
                Extent aExtent = _lonLatExtent;
                if (!MapFrame.MapView.Projection.IsLonLatMap)
                    aExtent = MapFrame.MapView.Projection.GetProjectedExtentFromLonLat(aExtent);
                this.MapFrame.MapView.ViewExtent = aExtent;

                this.MapFrame.MapView.PaintGraphics(g, rect);

                if (MapFrame.DrawNeatLine)
                {
                    Pen aPen = new Pen(MapFrame.NeatLineColor, MapFrame.NeatLineSize);
                    g.DrawRectangle(aPen, rect);
                }
         
            }
        }

        /// <summary>
        /// Override get property object methods
        /// </summary>
        /// <returns>property object</returns>
        public override object GetPropertyObject()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("Visible", "Visible");
            objAttr.Add("MinLon", "MinLon");
            objAttr.Add("MaxLon", "MaxLon");
            objAttr.Add("MinLat", "MinLat");
            objAttr.Add("MaxLat", "MaxLat");
            objAttr.Add("DrawNeatLine", "DrawNeatLine");
            objAttr.Add("NeatLineColor", "NeatLineColor");
            objAttr.Add("NeatLineSize", "NeatLineSize");            
            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;
        }

        #endregion
    }
}
