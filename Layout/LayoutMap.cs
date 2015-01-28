using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using MeteoInfoC.Map;
using MeteoInfoC.Global;
using MeteoInfoC.Legend;

namespace MeteoInfoC.Layout
{
    /// <summary>
    /// Layout map
    /// </summary>
    public class LayoutMap : LayoutElement
    {
        #region Events definitation
        /// <summary>
        /// Occurs after map view updated
        /// </summary>
        public event EventHandler MapViewUpdated;

        #endregion

        #region Variables
        //private MapView _mapView = null;
        private MapFrame _mapFrame = null;

        //private bool _drawNeatLine = true;        
        //private Color _neatLineColor = Color.Black;
        //private int _neatLineSize = 1;

        //private bool _drawGridLabel = true;
        //private Font _gridFont = new Font("Arial", 8);

        #endregion

        #region Constructor
        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public LayoutMap():base()
        //{
        //    ElementType = ElementType.LayoutMap;
        //    ResizeAbility = ResizeAbility.ResizeAll;
        //}

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="mapView">map view</param>
        //public LayoutMap(MapView mapView):base()
        //{
        //    ElementType = ElementType.LayoutMap;
        //    ResizeAbility = ResizeAbility.ResizeAll;
        //    _mapView = mapView;
        //}
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapFrame">map frame</param>
        public LayoutMap(MapFrame mapFrame):base()
        {
            ElementType = ElementType.LayoutMap;
            ResizeAbility = ResizeAbility.ResizeAll;
            this.MapFrame = mapFrame;            
        }

        #endregion

        #region Properties
        ///// <summary>
        ///// Get or set map view
        ///// </summary>
        //public MapView MapView
        //{
        //    get { return _mapView; }
        //    set { _mapView = value; }
        //}

        /// <summary>
        /// Get or set map frame
        /// </summary>
        public MapFrame MapFrame
        {
            get { return _mapFrame; }
            set 
            { 
                _mapFrame = value;
                _mapFrame.MapViewUpdated += new EventHandler(MapFrame_MapViewUpdated);
                //_mapFrame.LayoutBoundsChanged += new EventHandler(MapFrame_BoundsChanged);
                //_mapFrame.RaiseLayoutBoundsChangedEvent();
            }
        }

        //#region Setting     
        /// <summary>
        /// Get or set map view back color
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set map view back color")]
        public new Color BackColor
        {
            get { return _mapFrame.BackColor; }
            set { _mapFrame.BackColor = value; }
        }

        /// <summary>
        /// Get or set map view fore color
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set map view fore color")]
        public new Color ForeColor
        {
            get { return _mapFrame.ForeColor; }
            set { _mapFrame.ForeColor = value; }
        }

        /// <summary>
        /// Get or set left
        /// </summary>
        public override int Left
        {
            get { return _mapFrame.LayoutBounds.Left; }
            set
            {
                int left = value;
                _mapFrame.LayoutBounds = new Rectangle(left, _mapFrame.LayoutBounds.Y, _mapFrame.LayoutBounds.Width,
                    _mapFrame.LayoutBounds.Height);
            }
        }

        /// <summary>
        /// Get or set top
        /// </summary>
        public override int Top
        {
            get { return _mapFrame.LayoutBounds.Top; }
            set
            {
                int top = value;
                _mapFrame.LayoutBounds = new Rectangle(_mapFrame.LayoutBounds.X, top, _mapFrame.LayoutBounds.Width,
                    _mapFrame.LayoutBounds.Height);
            }
        }

        /// <summary>
        /// Get or set width
        /// </summary>
        public override int Width
        {
            get { return _mapFrame.LayoutBounds.Width; }
            set
            {
                int width = value;
                _mapFrame.LayoutBounds = new Rectangle(_mapFrame.LayoutBounds.X, _mapFrame.LayoutBounds.Y, width,
                    _mapFrame.LayoutBounds.Height);
            }
        }

        /// <summary>
        /// Get or set height
        /// </summary>
        public override int Height
        {
            get { return _mapFrame.LayoutBounds.Height; }
            set
            {
                int height = value;
                _mapFrame.LayoutBounds = new Rectangle(_mapFrame.LayoutBounds.X, _mapFrame.LayoutBounds.Y, _mapFrame.LayoutBounds.Width,
                    height);
            }
        }

        /// <summary>
        /// Get or set bounds
        /// </summary>
        public override Rectangle Bounds
        {
            get { return _mapFrame.LayoutBounds; }
            set { _mapFrame.LayoutBounds = value; }
        }

        /// <summary>
        /// Get or set map view neat line color
        /// </summary>
        [CategoryAttribute("NeatLine"), DescriptionAttribute("Set if draw map view neat line")]
        public bool DrawNeatLine
        {
            get
            {
                return MapFrame.DrawNeatLine;
            }
            set
            {
                MapFrame.DrawNeatLine = value;
            }
        }

        /// <summary>
        /// Get or set map view neat line color
        /// </summary>
        [CategoryAttribute("NeatLine"), DescriptionAttribute("Set map view neat line color")]
        public Color NeatLineColor
        {
            get
            {
                return MapFrame.NeatLineColor;
            }
            set
            {
                MapFrame.NeatLineColor = value;
            }
        }

        /// <summary>
        /// Get or set map view neat line size
        /// </summary>
        [CategoryAttribute("NeatLine"), DescriptionAttribute("Set map view neat line size")]
        public int NeatLineSize
        {
            get
            {
                return MapFrame.NeatLineSize;
            }
            set
            {
                MapFrame.NeatLineSize = value;
            }
        }

        /// <summary>
        /// Get or set gird line color
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid line color")]
        public Color GridLineColor
        {
            get
            {
                return MapFrame.GridLineColor;
            }
            set
            {
                MapFrame.GridLineColor = value;
            }
        }

        /// <summary>
        /// Get or set grid line size
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid line size")]
        public int GridLineSize
        {
            get
            {
                return MapFrame.GridLineSize;
            }
            set
            {
                MapFrame.GridLineSize = value;
            }
        }

        /// <summary>
        /// Get or set grid line style
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid line style")]
        public DashStyle GridLineStyle
        {
            get
            {
                return MapFrame.GridLineStyle;
            }
            set
            {
                MapFrame.GridLineStyle = value;
            }
        }

        /// <summary>
        /// Get or set if draw grid labels
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set if draw grid labels")]
        public bool DrawGridLabel
        {
            get { return MapFrame.DrawGridLabel; }
            set { MapFrame.DrawGridLabel = value; }
        }

        /// <summary>
        /// Get or set if draw grid tick line inside
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set if draw grid tick line inside")]
        public bool InsideTickLine
        {
            get { return _mapFrame.InsideTickLine; }
            set { _mapFrame.InsideTickLine = value; }
        }

        /// <summary>
        /// Get or set grid tick line length
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid tick line length")]
        public int TickLineLength
        {
            get { return _mapFrame.TickLineLength; }
            set { _mapFrame.TickLineLength = value; }
        }

        /// <summary>
        /// Get or set grid label shift
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid label shift")]
        public int GridLabelShift
        {
            get { return _mapFrame.GridLabelShift; }
            set { _mapFrame.GridLabelShift = value; }
        }

        /// <summary>
        /// Get or set grid label position
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid label position")]
        public GridLabelPosition GridLabelPosition
        {
            get { return MapFrame.GridLabelPosition; }
            set { MapFrame.GridLabelPosition = value; }
        }

        /// <summary>
        /// Get or set if draw grid line
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set if draw grid line")]
        public bool DrawGridLine
        {
            get
            {
                return MapFrame.DrawGridLine;
            }
            set
            {
                MapFrame.DrawGridLine = value;
            }
        }

        /// <summary>
        /// Get or set if draw grid line
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set if draw grid tick line")]
        public bool DrawGridTickLine
        {
            get
            {
                return MapFrame.DrawGridTickLine;
            }
            set
            {
                MapFrame.DrawGridTickLine = value;
            }
        }

        /// <summary>
        /// Get or set if draw degree symbol
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set if draw degree symbol")]
        public bool DrawDegreeSymbol
        {
            get { return _mapFrame.DrawDegreeSymbol; }
            set { _mapFrame.DrawDegreeSymbol = value; }
        }

        /// <summary>
        /// Get or set grid lable font
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid lable font")]
        public Font GridFont
        {
            get { return MapFrame.GridFont; }
            set { MapFrame.GridFont = value; }
        }

        /// <summary>
        /// Get or set grid x/longitude delt
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid x/longitude delt")]
        public double GridXDelt
        {
            get { return MapFrame.GridXDelt; }
            set { MapFrame.GridXDelt = value; }
        }

        /// <summary>
        /// Get or set grid y/latitude delt
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid y/latitude delt")]
        public double GridYDelt
        {
            get { return MapFrame.GridYDelt; }
            set { MapFrame.GridYDelt = value; }
        }

        /// <summary>
        /// Get or set grid x/longitude delt
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid x/longitude origin")]
        public double GridXOrigin
        {
            get { return MapFrame.GridXOrigin; }
            set { MapFrame.GridXOrigin = value; }
        }

        /// <summary>
        /// Get or set grid y/latitude delt
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid y/latitude origin")]
        public double GridYOrigin
        {
            get { return MapFrame.GridYOrigin; }
            set { MapFrame.GridYOrigin = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Override paint method
        /// </summary>
        /// <param name="g">graphics</param>
        public override void Paint(Graphics g)
        {
            if (_mapFrame != null)
            {
                g.FillRectangle(new SolidBrush(_mapFrame.MapView.BackColor), _mapFrame.LayoutBounds);

                //Region oldRegion = g.Clip;
                //GraphicsPath path = new GraphicsPath();
                //Rectangle rect = this.Bounds;                
                //path.AddRectangle(rect);
                //g.SetClip(path);
                //Matrix oldMatrix = g.Transform;
                //g.TranslateTransform(this.Left, this.Top);                

                _mapFrame.MapView.PaintGraphics(g, _mapFrame.LayoutBounds);

                //Draw lon/lat grid labels
                if (_mapFrame.DrawGridLabel)
                {
                    List<Extent> extentList = new List<Extent>();
                    Extent maxExtent = new Extent();
                    Extent aExtent = new Extent();
                    SizeF aSF = new SizeF();
                    SolidBrush aBrush = new SolidBrush(this.ForeColor);
                    Pen aPen = new Pen(_mapFrame.GridLineColor);
                    aPen.Width = _mapFrame.GridLineSize;
                    string drawStr;
                    PointF sP = new PointF(0, 0);
                    PointF eP = new PointF(0, 0);
                    Font font = new Font(_mapFrame.GridFont.Name, _mapFrame.GridFont.Size, _mapFrame.GridFont.Style);
                    float labX, labY;
                    int len = 5;
                    int space = len + 2;
                    for (int i = 0; i < _mapFrame.MapView.GridLabels.Count; i++)
                    {
                        GridLabel aGL = _mapFrame.MapView.GridLabels[i];
                        switch (_mapFrame.GridLabelPosition)
                        {
                            case GridLabelPosition.LeftBottom:
                                switch (aGL.LabDirection)
                                {
                                    case Direction.East:
                                    case Direction.North:
                                        continue;
                                }
                                break;
                            case GridLabelPosition.LeftUp:
                                switch (aGL.LabDirection)
                                {
                                    case Direction.East:
                                    case Direction.South:
                                        continue;
                                }
                                break;
                            case GridLabelPosition.RightBottom:
                                switch (aGL.LabDirection)
                                {
                                    case Direction.Weast:
                                    case Direction.North:
                                        continue;
                                }
                                break;
                            case GridLabelPosition.RightUp:
                                switch (aGL.LabDirection)
                                {
                                    case Direction.Weast:
                                    case Direction.South:
                                        continue;
                                }
                                break;
                        }                        

                        labX = (float)aGL.LabPoint.X;
                        labY = (float)aGL.LabPoint.Y;
                        labX = labX + this.Left;
                        labY = labY + this.Top;
                        sP.X = labX;
                        sP.Y = labY;

                        drawStr = aGL.LabString;
                        aSF = g.MeasureString(drawStr, font);
                        switch (aGL.LabDirection)
                        {
                            case Direction.South:                                
                                labX = labX - aSF.Width / 2;
                                labY = labY + space;
                                eP.X = sP.X;
                                eP.Y = sP.Y + len;
                                break;
                            case Direction.Weast:
                                labX = labX - aSF.Width - space;
                                labY = labY - aSF.Height / 2;
                                eP.X = sP.X - len;
                                eP.Y = sP.Y;
                                break;
                            case Direction.North:
                                labX = labX - aSF.Width / 2;
                                labY = labY - aSF.Height - space;
                                eP.X = sP.X;
                                eP.Y = sP.Y - len;
                                break;
                            case Direction.East:
                                labX = labX + space;
                                labY = labY - aSF.Height / 2;
                                eP.X = sP.X + len;
                                eP.Y = sP.Y;
                                break;
                        }

                        bool ifDraw = true;
                        float aSize = aSF.Width / 2;
                        float bSize = aSF.Height / 2;
                        aExtent.minX = labX;
                        aExtent.maxX = labX + aSF.Width;
                        aExtent.minY = labY - aSF.Height;
                        aExtent.maxY = labY;

                        //Judge extent                                        
                        if (extentList.Count == 0)
                        {
                            maxExtent = aExtent;
                            extentList.Add(aExtent);
                        }
                        else
                        {
                            if (!MIMath.IsExtentCross(aExtent, maxExtent))
                            {
                                extentList.Add(aExtent);
                                maxExtent = MIMath.GetLagerExtent(maxExtent, aExtent);
                            }
                            else
                            {
                                for (int j = 0; j < extentList.Count; j++)
                                {
                                    if (MIMath.IsExtentCross(aExtent, extentList[j]))
                                    {
                                        ifDraw = false;
                                        break;
                                    }
                                }
                                if (ifDraw)
                                {
                                    extentList.Add(aExtent);
                                    maxExtent = MIMath.GetLagerExtent(maxExtent, aExtent);
                                }
                            }
                        }

                        if (ifDraw)
                        {
                            g.DrawLine(aPen, sP, eP);
                            g.DrawString(drawStr, font, aBrush, labX, labY);
                        }
                    }

                    aPen.Dispose();
                    aBrush.Dispose();
                }

                //g.Transform = oldMatrix;
                //g.Clip = oldRegion;

                if (_mapFrame.DrawNeatLine)
                {
                    Pen aPen = new Pen(_mapFrame.NeatLineColor, _mapFrame.NeatLineSize);
                    g.DrawRectangle(aPen, _mapFrame.LayoutBounds);
                    aPen.Dispose();
                }               
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
            if (_mapFrame != null)
            {
                PointF aP = PageToScreen(this.Left, this.Top, pageLocation, zoom);
                Rectangle rect = new Rectangle((int)aP.X, (int)aP.Y, (int)(Width * zoom), (int)(Height * zoom));
                g.FillRectangle(new SolidBrush(_mapFrame.MapView.BackColor), rect);                             

                _mapFrame.MapView.PaintGraphics(g, rect);

                //Draw lon/lat grid labels
                if (_mapFrame.DrawGridLabel || _mapFrame.DrawGridTickLine)
                {
                    List<Extent> extentList = new List<Extent>();
                    Extent maxExtent = new Extent();
                    Extent aExtent = new Extent();
                    SizeF aSF = new SizeF();
                    SolidBrush aBrush = new SolidBrush(this.ForeColor);
                    Pen aPen = new Pen(_mapFrame.GridLineColor);
                    aPen.Width = _mapFrame.GridLineSize;
                    string drawStr;
                    PointF sP = new PointF(0, 0);
                    PointF eP = new PointF(0, 0);
                    Font font = new Font(_mapFrame.GridFont.Name, _mapFrame.GridFont.Size * zoom, _mapFrame.GridFont.Style);
                    float labX, labY;
                    int len = _mapFrame.TickLineLength;
                    int space = len + _mapFrame.GridLabelShift;
                    if (_mapFrame.InsideTickLine)
                        space = _mapFrame.GridLabelShift;

                    for (int i = 0; i < _mapFrame.MapView.GridLabels.Count; i++)
                    {
                        GridLabel aGL = _mapFrame.MapView.GridLabels[i];
                        switch (_mapFrame.GridLabelPosition)
                        {
                            case GridLabelPosition.LeftBottom:
                                switch (aGL.LabDirection)
                                {
                                    case Direction.East:
                                    case Direction.North:
                                        continue;
                                }
                                break;
                            case GridLabelPosition.LeftUp:
                                switch (aGL.LabDirection)
                                {
                                    case Direction.East:
                                    case Direction.South:
                                        continue;
                                }
                                break;
                            case GridLabelPosition.RightBottom:
                                switch (aGL.LabDirection)
                                {
                                    case Direction.Weast:
                                    case Direction.North:
                                        continue;
                                }
                                break;
                            case GridLabelPosition.RightUp:
                                switch (aGL.LabDirection)
                                {
                                    case Direction.Weast:
                                    case Direction.South:
                                        continue;
                                }
                                break;
                        } 

                        labX = (float)aGL.LabPoint.X;
                        labY = (float)aGL.LabPoint.Y;
                        labX = labX + this.Left * zoom + pageLocation.X;
                        labY = labY + this.Top * zoom + pageLocation.Y;
                        sP.X = labX;
                        sP.Y = labY;

                        drawStr = aGL.LabString;
                        if (_mapFrame.DrawDegreeSymbol)
                        {
                            if (drawStr.EndsWith("E") || drawStr.EndsWith("W") || drawStr.EndsWith("N") || drawStr.EndsWith("S"))
                                drawStr = drawStr.Substring(0, drawStr.Length - 1) + ((char)186).ToString() + drawStr.Substring(drawStr.Length - 1);
                            else
                                drawStr = drawStr + ((char)186).ToString();
                        }
                        aSF = g.MeasureString(drawStr, font);
                        switch (aGL.LabDirection)
                        {
                            case Direction.South:
                                labX = labX - aSF.Width / 2;
                                labY = labY + space;
                                eP.X = sP.X;
                                if (_mapFrame.InsideTickLine)
                                    eP.Y = sP.Y - len;
                                else
                                    eP.Y = sP.Y + len;
                                break;
                            case Direction.Weast:
                                labX = labX - aSF.Width - space;
                                labY = labY - aSF.Height / 2;
                                eP.Y = sP.Y;
                                if (_mapFrame.InsideTickLine)
                                    eP.X = sP.X + len;
                                else
                                    eP.X = sP.X - len;                                
                                break;
                            case Direction.North:
                                labX = labX - aSF.Width / 2;
                                labY = labY - aSF.Height - space;
                                eP.X = sP.X;
                                if (_mapFrame.InsideTickLine)
                                    eP.Y = sP.Y + len;
                                else
                                    eP.Y = sP.Y - len;
                                break;
                            case Direction.East:
                                labX = labX + space;
                                labY = labY - aSF.Height / 2;
                                eP.Y = sP.Y;
                                if (_mapFrame.InsideTickLine)
                                    eP.X = sP.X - len;
                                else
                                    eP.X = sP.X + len;                                
                                break;
                        }

                        bool ifDraw = true;
                        float aSize = aSF.Width / 2;
                        float bSize = aSF.Height / 2;
                        aExtent.minX = labX;
                        aExtent.maxX = labX + aSF.Width;
                        aExtent.minY = labY - aSF.Height;
                        aExtent.maxY = labY;

                        //Judge extent                                        
                        if (extentList.Count == 0)
                        {
                            maxExtent = aExtent;
                            extentList.Add(aExtent);
                        }
                        else
                        {
                            if (!MIMath.IsExtentCross(aExtent, maxExtent))
                            {
                                extentList.Add(aExtent);
                                maxExtent = MIMath.GetLagerExtent(maxExtent, aExtent);
                            }
                            else
                            {
                                for (int j = 0; j < extentList.Count; j++)
                                {
                                    if (MIMath.IsExtentCross(aExtent, extentList[j]))
                                    {
                                        ifDraw = false;
                                        break;
                                    }
                                }
                                if (ifDraw)
                                {
                                    extentList.Add(aExtent);
                                    maxExtent = MIMath.GetLagerExtent(maxExtent, aExtent);
                                }                                
                            }
                        }
                        
                        if (ifDraw)
                        {
                            if (_mapFrame.DrawGridTickLine)
                                g.DrawLine(aPen, sP, eP);
                            if (_mapFrame.DrawGridLabel)
                                g.DrawString(drawStr, font, aBrush, labX, labY);
                        }
                    }

                    aPen.Dispose();
                    aBrush.Dispose();
                }

                //Draw neat line
                if (_mapFrame.DrawNeatLine)
                {
                    Pen aPen = new Pen(_mapFrame.NeatLineColor, _mapFrame.NeatLineSize);
                    g.DrawRectangle(aPen, rect);
                    aPen.Dispose();
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
            objAttr.Add("BackColor", "BackColor");
            objAttr.Add("ForeColor", "ForeColor");
            objAttr.Add("DrawNeatLine", "DrawNeatLine");
            objAttr.Add("NeatLineColor", "NeatLineColor");
            objAttr.Add("NeatLineSize", "NeatLineSize");
            objAttr.Add("DrawGridLine", "DrawGridLine");
            objAttr.Add("DrawGridTickLine", "DrawGridTickLine");
            objAttr.Add("DrawGridLabel", "DrawGridLabel");
            objAttr.Add("InsideTickLine", "InsideTickLine");
            objAttr.Add("TickLineLength", "TickLineLength");
            objAttr.Add("GridLabelShift", "GridLabelShift");
            objAttr.Add("GridLabelPosition", "GridLabelPosition");
            objAttr.Add("GridLineColor", "GridLineColor");
            objAttr.Add("GridLineSize", "GridLineSize");
            objAttr.Add("GridLineStyle", "GridLineStyle");
            objAttr.Add("GridFont", "GridFont");
            objAttr.Add("Left", "Left");
            objAttr.Add("Top", "Top");
            objAttr.Add("Width", "Width");
            objAttr.Add("Height", "Height");
            if (_mapFrame.MapView.IsGeoMap)
            {
                objAttr.Add("DrawDegreeSymbol", "DrawDegreeSymbol");
                objAttr.Add("GridXDelt", "GridXDelt");
                objAttr.Add("GridYDelt", "GridYDelt");
                objAttr.Add("GridXOrigin", "GridXOrigin");
                objAttr.Add("GridYOrigin", "GridYOrigin");
            }
            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;

            //CustomProperty cp = (CustomProperty)_mapFrame.GetPropertyObject();            
            //cp.ObjectAttribs.Add("Left", "Left");
            //cp.ObjectAttribs.Add("Top", "Top");
            //cp.ObjectAttribs.Add("Width", "Width");
            //cp.ObjectAttribs.Add("Height", "Height");

            //return cp;
        }

        /// <summary>
        /// Zoom to exactly lon/lat extent
        /// </summary>
        /// <param name="aExtent">extent</param>
        public void ZoomToExtentLonLatEx(Extent aExtent)
        {
            if (!_mapFrame.MapView.Projection.IsLonLatMap)
            {
                aExtent = _mapFrame.MapView.Projection.GetProjectedExtentFromLonLat(aExtent);
            }

            SetSizeByExtent(aExtent);
            _mapFrame.MapView.ViewExtent = aExtent;
        }

        /// <summary>
        /// Zoom to exactly extent
        /// </summary>
        /// <param name="aExtent">Extent</param>
        public void ZoomToExtnetEx(Extent aExtent)
        {
            SetSizeByExtent(aExtent);
            _mapFrame.MapView.ViewExtent = aExtent;
        }

        private void SetSizeByExtent(Extent aExtent)
        {
            double scaleFactor;

            double scaleX = Width / (aExtent.maxX - aExtent.minX);
            double scaleY = Height / (aExtent.maxY - aExtent.minY);
            if (_mapFrame.MapView.Projection.IsLonLatMap)
            {
                scaleFactor = _mapFrame.MapView.XYScaleFactor;
            }
            else
            {
                scaleFactor = 1;
            }

            if (scaleX > scaleY)
            {
                scaleX = scaleY / scaleFactor;
                Width = (int)((aExtent.maxX - aExtent.minX) * scaleX);
            }
            else
            {
                scaleY = scaleX * scaleFactor;
                Height = (int)((aExtent.maxY - aExtent.minY) * scaleY);
            }
        }

        #endregion

        #region Events
        void MapFrame_MapViewUpdated(object sender, EventArgs e)
        {
            OnMapViewUpdated();
        }

        /// <summary>
        /// Fire MapViewUpdated event
        /// </summary>
        protected virtual void OnMapViewUpdated()
        {
            if (MapViewUpdated != null) MapViewUpdated(this, new EventArgs());
        }

        /// <summary>
        /// Raise MapViewUpdated event
        /// </summary>
        public void FireMapViewUpdatedEvent()
        {
            OnMapViewUpdated();
        }

        //void MapFrame_BoundsChanged(object sender, EventArgs e)
        //{
        //    this.Left = _mapFrame.LayoutBounds.Left;
        //    this.Top = _mapFrame.LayoutBounds.Top;
        //    this.Width = _mapFrame.LayoutBounds.Width;
        //    this.Height = _mapFrame.LayoutBounds.Height;
        //}

        ///// <summary>
        ///// Override LocationChanged event
        ///// </summary>
        //protected override void OnLocationChanged()
        //{
        //    base.OnLocationChanged();

        //    _mapFrame.SetLayoutBounds(new Rectangle(Left, Top, Width, Height));
        //}

        ///// <summary>
        ///// Override SizeChanged event
        ///// </summary>
        //protected override void OnSizeChanged()
        //{
        //    base.OnSizeChanged();

        //    _mapFrame.SetLayoutBounds(new Rectangle(Left, Top, Width, Height));
        //}

        #endregion
    }
}
