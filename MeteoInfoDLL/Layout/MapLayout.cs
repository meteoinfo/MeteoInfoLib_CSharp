using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.ComponentModel;
using System.IO;

using MeteoInfoC.Map;
using MeteoInfoC.Shape;
using MeteoInfoC.Legend;
using MeteoInfoC.Global;
using MeteoInfoC.Layer;
using MeteoInfoC.Geoprocess;
using MeteoInfoC.Projections;

namespace MeteoInfoC.Layout
{
    /// <summary>
    /// Map layout control
    /// </summary>
    public class MapLayout:UserControl
    {
        private IContainer components;

        #region Events definition
        /// <summary>
        /// Occurs after one of the elements is selected.
        /// </summary>
        public event EventHandler ElementSeleted;
        /// <summary>
        /// Occurs after zoom is changed
        /// </summary>
        public event EventHandler ZoomChanged;
        /// <summary>
        /// Occurs after active map frame is changed
        /// </summary>
        public event EventHandler ActiveMapFrameChanged;
        /// <summary>
        /// Occurs after map frames updated
        /// </summary>
        public event EventHandler MapFramesUpdated;

        #endregion

        #region Variables
        ///// <summary>
        ///// WS_CLITCHILDREN
        ///// </summary>
        //public static int WS_CLIPCHILDREN = 0x02000000;

        private frmIdentifer _frmIdentifer;
        private frmIdentiferGrid _frmIdentiferGrid = new frmIdentiferGrid();
        private frmMeasurement _frmMeasure = new frmMeasurement();

        private List<MapFrame> _mapFrames = new List<MapFrame>();
        private List<LayoutElement> _layoutElements = new List<LayoutElement>();
        private LayoutMap _currentLayoutMap;
        //private LayoutMap _defaultLayoutMap;
        //private LayoutLegend _defaultLegend;
        //private LayoutIllustrationMap _illusMap;
        //private LayoutGraphic _defaultTitle;
        //private LayoutGraphic _defaultWindArraw;

        private Bitmap _layoutBitmap = new Bitmap(10, 10, PixelFormat.Format32bppPArgb);
        private Bitmap _tempMapBitmap = new Bitmap(10, 10, PixelFormat.Format32bppPArgb);

        //private MapView _mapView = new MapView();                         
        private Rectangle _pageBounds;
        private Color _PageForeColor = Color.Black;
        private Color _PageBackColor = Color.White;
        private PrinterSettings _printerSetting;
        private float _zoom;
        private PaperSize _paperSize = new PaperSize();
        private List<PaperSize> _paperSizeList = new List<PaperSize>();
        private bool _isLandscape;
        //private List<Control> m_SelectedControls = new List<Control>();
        private MouseMode _mouseMode = MouseMode.Default;        
        private Point _mouseDownPos = Point.Empty;
        private Point _mouseLastPos = Point.Empty;
        private Edge _resizeSelectedEdge = Edge.None;
        private bool _AutoResize = false;
        //private Font _GridFont = new Font("Arial", 8);
        private SmoothingMode _smoothingMode = SmoothingMode.Default;
        private PointF _pageLocation = new PointF(0, 0);

        private Point _mouseDownPoint = new Point(0, 0);
        private bool _startNewGraphic = true;
        private List<PointF> _graphicPoints = new List<PointF>();        
        private List<LayoutElement> _selectedElements = new List<LayoutElement>();
        private Rectangle _selectedRectangle = new Rectangle();
        //private List<LayoutElement> _defaultElements = new List<LayoutElement>();

        private List<PointD> _editingVertices = new List<PointD>();
        private int _editingVerticeIndex;

        private PointBreak _defPointBreak = new PointBreak();
        private LabelBreak _defLabelBreak = new LabelBreak();
        private PolyLineBreak _defPolylineBreak = new PolyLineBreak();
        private PolygonBreak _defPolygonBreak = new PolygonBreak();

        private frmLabelSymbolSet _frmLabelSymbolSet = new frmLabelSymbolSet();
        private frmPointSymbolSet _frmPointSymbolSet;
        private frmPolylineSymbolSet _frmPolylineSymbolSet;
        private frmPolygonSymbolSet _frmPolygonSymbolSet;

        private VScrollBar _vScrollBar;
        private HScrollBar _hScrollBar;

        #endregion        

        #region Constructor
        /// <summary>
        /// Map layout constructor
        /// </summary>
        public MapLayout()
        {
            InitializeComponent();

            this.SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer, true);

            //_frmIdentifer = new frmIdentifer(_mapView);
            _frmPointSymbolSet = new frmPointSymbolSet(this);
            _frmPolylineSymbolSet = new frmPolylineSymbolSet(this);
            _frmPolygonSymbolSet = new frmPolygonSymbolSet(this);
            _frmMeasure.FormClosed += new FormClosedEventHandler(this.FrmMeasurementClosed);

            BackColor = Color.DarkGray;
            //Set page        
            PaperSize aPS = new PaperSize("A4", 827, 1169);
            _paperSizeList.Add(aPS);
            aPS = new PaperSize("Letter", 850, 1100);
            _paperSizeList.Add(aPS);
            aPS = new PaperSize("A5", 583, 827);
            _paperSizeList.Add(aPS);
            aPS = new PaperSize("Custom", 500, 750);
            _paperSizeList.Add(aPS);
            _isLandscape = true;

            //Set default size
            _pageBounds = new Rectangle();
            _pageBounds.X = 0;
            _pageBounds.Y = 0;
            _pageBounds.Width = 730;
            _pageBounds.Height = 480;
            _zoom = 1.0F;
            PaperSize = aPS;

            //Add a default map frame
            MapFrame aMF = new MapFrame();
            aMF.Active = true;
            _mapFrames.Add(aMF);
            _frmIdentifer = new frmIdentifer(aMF.MapView);

            ////Add a layout elements  
            //_defaultLayoutMap = new LayoutMap(_mapView); 
            //_defaultLayoutMap.Left = 40;
            //_defaultLayoutMap.Top = 36;
            //_defaultLayoutMap.Width = 606;
            //_defaultLayoutMap.Height = 425;
            //AddElement(_defaultLayoutMap);

            //_illusMap = new LayoutIllustrationMap(_mapView);
            //_illusMap.Visible = false;
            //_illusMap.Left = 528;
            //_illusMap.Top = 318;
            //_illusMap.Width = 94;
            //_illusMap.Height = 119;
            //AddElement(_illusMap);

            //_defaultLegend = new LayoutLegend(this);
            //_defaultLegend.Visible = false;
            //_defaultLegend.Left = _defaultLayoutMap.Right + 10;
            //_defaultLegend.Top = _pageBounds.Top + _pageBounds.Height / 4;            
            //AddElement(_defaultLegend);

            //WindArraw aWindArraw = new WindArraw();
            //aWindArraw.angle = 270;
            //VectorBreak aVB = new VectorBreak();
            //aVB.Color = Color.Black;
            //_defaultWindArraw = new LayoutGraphic(new Graphic(aWindArraw, aVB), this);
            //_defaultWindArraw.Left = 10;
            //_defaultWindArraw.Top = 10;
            //_defaultWindArraw.Visible = false;
            //AddElement(_defaultWindArraw);

            //PointShape aShape = new PointShape();            
            //LabelBreak aLB = new LabelBreak();
            //Font aFont = new Font("Arial", 8, FontStyle.Bold);
            //aLB.Font = aFont;
            //aLB.Text = "MeteoInfo: Meteological Data Information System";
            //_defaultTitle = new LayoutGraphic(new Graphic(aShape, aLB), this);
            //_defaultTitle.IsTitle = true;
            //_defaultTitle.Left = 20;
            //_defaultTitle.Top = 10;
            //AddElement(_defaultTitle);

            //_defaultElements.Add(_defaultLayoutMap);
            //_defaultElements.Add(_defaultLegend);
            //_defaultElements.Add(_illusMap);
            //_defaultElements.Add(_defaultWindArraw);
            //_defaultElements.Add(_defaultTitle);
            
            ////Set map view      
            //_mapView.LockViewUpdate = true;
            //_mapView.DrawNeatLine = true;
            //_mapView.DrawGridLine = true;
            //_mapView.DrawGridTickLine = true;
            //_mapView.IsLayoutMap = true;
            //_mapView.ViewExtentChanged += MapViewViewExtentChanged;
            //_mapView.LayersUpdated += MapViewLayersUpdated;
            //_mapView.Resize += MapViewResize;
            //_mapView.MapViewRedrawed += MapViewRedrawed;
            //_mapView.Visible = false;                                                 

            //_mapView.Left = 40;
            //_mapView.Top = 40;
            //_mapView.Width = 620;
            //_mapView.Height = 400;

            _defPointBreak.Size = 10;
            _defLabelBreak.Text = "Text";
            _defLabelBreak.Font = new Font("Arial", 10);
            _defPolylineBreak.Color = Color.Red;
            _defPolylineBreak.Size = 2;
            _defPolygonBreak.Color = Color.FromArgb(125, Color.LightYellow);
            //_defPolygonBreak.TransparencyPercent = 50;
        }
        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LayersLegend));
            this._vScrollBar = new VScrollBar();
            this._hScrollBar = new HScrollBar();
            //this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // _vScrollBar
            // 
            this._vScrollBar.Location = new System.Drawing.Point(120, 44);
            this._vScrollBar.Name = "_vScrollBar";
            this._vScrollBar.Size = new System.Drawing.Size(17, 232);
            this._vScrollBar.TabIndex = 0;
            this._vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
            //
            //_hScrollBar
            //
            this._hScrollBar.Location = new System.Drawing.Point(10, 120);
            this._hScrollBar.Name = "_hScrollBar";
            this._hScrollBar.Size = new Size(232, 17);
            this._hScrollBar.TabIndex = 1;
            this._hScrollBar.Scroll += new ScrollEventHandler(this.hScrollBar_Scroll);
            //// 
            //// Icons
            //// 
            //this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            //this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            //this.Icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Icons.ImageStream")));
            //this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Legend
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.AddRange(new System.Windows.Forms.Control[] {this._vScrollBar,
                this._hScrollBar});
            this.Name = "Legend";
            this.Size = new System.Drawing.Size(156, 336);
            //this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Legend_MouseUp);
            //this.DoubleClick += new System.EventHandler(this.Legend_DoubleClick);
            //this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Legend_MouseMove);
            //this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Legend_MouseDown);
            this.ResumeLayout(false);

        }
        #endregion

        #region Properties

        #region Setting
        /// <summary>
        /// Get or set smoothing mode of GDI+
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set graph smoothing mode")]
        public SmoothingMode SmoothingMode
        {
            get
            {
                return _smoothingMode;
            }
            set
            {
                if (value != SmoothingMode.Invalid)
                {
                    _smoothingMode = value;
                    //MapView.SmoothingMode = _smoothingMode;                    
                    //UpdateControls();
                }
            }
        }

        /// <summary>
        /// Get or set fore color of map view
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set page fore color")]
        public Color PageForeColor
        {
            get
            {
                return _PageForeColor;
            }
            set
            {
                _PageForeColor = value;
                //UpdateControls();
            }
        }

        /// <summary>
        /// Get or set back color of map layout
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set page back color")]
        public Color PageBackColor
        {
            get
            {
                return _PageBackColor;
            }
            set
            {                
                _PageBackColor = value;

                if (value == Color.Black)
                {
                    _PageForeColor = Color.White;
                    //_defaultLayoutMap.ForeColor = Color.White;
                    //_defaultLayoutMap.BackColor = Color.Black;
                    //_defaultLayoutMap.MapFrame.GridLineColor = Color.DarkGray;
                    //_defaultLayoutMap.MapFrame.NeatLineColor = Color.White;                    
                    //_defaultLegend.NeatLineColor = Color.White;
                    //_defaultLegend.ForeColor = Color.White;
                    //((LabelBreak)_defaultTitle.Graphic.Legend).Color = Color.White;                    
                    //UpdateControls();
                }
                else if (value == Color.White)
                {
                    _PageForeColor = Color.Black;
                    //_defaultLayoutMap.ForeColor = Color.Black;
                    //_defaultLayoutMap.BackColor = Color.White;
                    //_defaultLayoutMap.MapFrame.GridLineColor = Color.DarkGray;
                    //_defaultLayoutMap.MapFrame.NeatLineColor = Color.Black;
                    //_defaultLegend.NeatLineColor = Color.Black;
                    //_defaultLegend.ForeColor = Color.Black;
                    //((LabelBreak)_defaultTitle.Graphic.Legend).Color = Color.Black;
                    //UpdateControls();
                }
                else
                {
                    Refresh();
                }
            }
        }                

        ///// <summary>
        ///// Get or set draw font
        ///// </summary>
        //[CategoryAttribute("Grid"), DescriptionAttribute("Grid line string font")]
        //public Font GridFont
        //{
        //    get { return _GridFont; }
        //    set
        //    {
        //        _GridFont = value;
        //        UpdateControls();
        //    }
        //}

        #endregion

        #region Others  
        /// <summary>
        /// Get or set map frames
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<MapFrame> MapFrames
        {
            get { return _mapFrames; }
            set
            {
                List<MapFrame> mfs = value;
                _mapFrames = new List<MapFrame>();
                foreach (MapFrame mf in mfs)
                {
                    bool isInsert = false;
                    for (int i = 0; i < _mapFrames.Count; i++)
                    {
                        MapFrame amf = _mapFrames[i];
                        if (mf.Order < amf.Order)
                        {
                            _mapFrames.Insert(i, mf);
                            isInsert = true;
                            break;
                        }
                    }

                    if (!isInsert)
                        _mapFrames.Add(mf);
                }
                //UpdateMapFrames();
            }
        }

        /// <summary>
        /// Get or set layout elements
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<LayoutElement> LayoutElements
        {
            get { return _layoutElements; }
            set { _layoutElements = value; }
        }

        /// <summary>
        /// Get active map frame
        /// </summary>
        public MapFrame ActiveMapFrame
        {
            get
            {
                foreach (MapFrame mf in _mapFrames)
                    if (mf.Active)
                        return mf;

                return null;
            }
        }

        /// <summary>
        /// Get if has legend element
        /// </summary>
        public bool HasLegendElement
        {
            get
            {
                foreach (LayoutElement aLE in _layoutElements)
                {
                    if (aLE.ElementType == ElementType.LayoutLegend)
                        return true;
                }
                return false;
            }
        }

        ///// <summary>
        ///// Get active MapView
        ///// </summary>
        //public MapView MapView
        //{
        //    get { return DefaultLayoutMap.MapFrame.MapView; }
        //}

        ///// <summary>
        ///// Get or set map view control
        ///// </summary>
        //public MapView MapView
        //{
        //    get { return _mapView; }
        //    set 
        //    { 
        //        _mapView = value;
        //        _defaultLayoutMap.MapView = _mapView;
        //        _illusMap.LinkedMapView = _mapView;
        //        _mapView.ViewExtentChanged += MapViewViewExtentChanged;
        //        _mapView.LayersUpdated += MapViewLayersUpdated;
        //        _mapView.Resize += MapViewResize;
        //        _mapView.MapViewRedrawed += MapViewRedrawed;
        //        _mapView.RenderChanged += MapViewRenderChanged;
        //    }
        //}

        /// <summary>
        /// Get layout maps
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<LayoutMap> LayoutMaps
        {
            get
            {
                List<LayoutMap> layoutMaps = new List<LayoutMap>();
                foreach (LayoutElement aLE in _layoutElements)
                {
                    if (aLE.ElementType == ElementType.LayoutMap)
                        layoutMaps.Add((LayoutMap)aLE);
                }
                return layoutMaps;
            }
        }

        /// <summary>
        /// Get active layout map
        /// </summary>
        public LayoutMap ActiveLayoutMap
        {
            get 
            {
                LayoutMap aLM = null;
                foreach (LayoutMap lm in LayoutMaps)
                {
                    if (lm.MapFrame.Active)
                    {
                        aLM = lm;
                        break;
                    }
                }
                return aLM; 
            }
        }

        ///// <summary>
        ///// Get default illustration map
        ///// </summary>
        //public LayoutIllustrationMap DefaultIllustration
        //{
        //    get { return _illusMap; }
        //}

        ///// <summary>
        ///// Get default title
        ///// </summary>
        //public LayoutGraphic DefaultTitle
        //{
        //    get { return _defaultTitle; }            
        //}

        ///// <summary>
        ///// Get default legend
        ///// </summary>
        //public LayoutLegend DefaultLegend
        //{
        //    get { return _defaultLegend; }
        //}

        /// <summary>
        /// Get selected elements
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<LayoutElement> SelectedElements
        {
            get { return _selectedElements; }
        }

        /// <summary>
        /// Get or set page bounds
        /// </summary>
        public Rectangle PageBounds
        {
            get { return _pageBounds; }
            set { _pageBounds = value; }
        }

        /// <summary>
        /// Get or set page location
        /// </summary>
        public PointF PageLocation
        {
            get { return _pageLocation; }
            set { _pageLocation = value; }
        }

        /// <summary>
        /// Get or set printer setting
        /// </summary>
        public PrinterSettings PrinterSetting
        {
            get { return _printerSetting; }
            set 
            { 
                _printerSetting = value;
                RectangleF aRect = PaperToScreen(new RectangleF(0, 0, PaperWidth, PaperHeight));
                _pageBounds.Width = (int)aRect.Width;
                _pageBounds.Height = (int)aRect.Height;
            }
        }

        /// <summary>
        /// Get or set paper size
        /// </summary>
        public PaperSize PaperSize
        {
            get { return _paperSize; }
            set
            {
                _paperSize = value;
                RectangleF aRect = PaperToScreen(new RectangleF(0, 0, PaperWidth, PaperHeight));
                _pageBounds.Width = (int)aRect.Width;
                _pageBounds.Height = (int)aRect.Height;
            }
        }

        /// <summary>
        /// Get or set landscape
        /// </summary>
        public bool Landscape
        {
            get { return _isLandscape; }
            set 
            {
                _isLandscape = value;
                RectangleF aRect = PaperToScreen(new RectangleF(0, 0, PaperWidth, PaperHeight));
                _pageBounds.Width = (int)aRect.Width;
                _pageBounds.Height = (int)aRect.Height;
            }
        }

        /// <summary>
        /// Get or set mouse mode
        /// </summary>
        public MouseMode MouseMode
        {
            get { return _mouseMode; }
            set 
            { 
                _mouseMode = value;
                switch (_mouseMode)
                {                    
                    case MouseMode.New_Label:
                    case MouseMode.New_Point:
                    case MouseMode.New_Polyline:
                    case MouseMode.New_Polygon:
                    case MouseMode.New_Rectangle:
                    case MouseMode.New_Circle:
                    case MouseMode.New_Curve:
                    case MouseMode.New_CurvePolygon:
                    case MouseMode.New_Ellipse:
                    case MouseMode.New_Freehand:
                    //case MouseMode.Map_SelectFeatures_Rectangle:
                    //case MouseMode.Map_SelectFeatures_Polygon:
                    //case MouseMode.Map_SelectFeatures_Lasso:
                    //case MouseMode.Map_SelectFeatures_Circle:
                        this.Cursor = Cursors.Cross;
                        break;
                    case MouseMode.Map_Measurement:
                        this.Cursor = Cursors.Cross;
                        if (!_frmMeasure.IsHandleCreated)
                        {
                            _frmMeasure = new frmMeasurement();
                            _frmMeasure.Show(this);
                        }
                        _frmMeasure.FormClosed += new FormClosedEventHandler(FrmMeasurementClosed);
                        break;
                }
            }
        }

        /// <summary>
        /// Get or set if autometic resize
        /// </summary>
        public bool AutoResize
        {
            get { return _AutoResize; }
            set { _AutoResize = value; }
        }

        /// <summary>
        /// Get or set zoom
        /// </summary>
        public float Zoom
        {
            get { return _zoom; }
            set 
            { 
                _zoom = value;
                OnZoomChanged();
            }
        }

        /// <summary>
        /// Get or set default point break
        /// </summary>
        public PointBreak DefPointBreak
        {
            get { return _defPointBreak; }
            set { _defPointBreak = value; }
        }

        /// <summary>
        /// Get or set default lable break
        /// </summary>
        public LabelBreak DefLabelBreak
        {
            get { return _defLabelBreak; }
            set { _defLabelBreak = value; }
        }

        /// <summary>
        /// Get or set default polyline break
        /// </summary>
        public PolyLineBreak DefPolylineBreak
        {
            get { return _defPolylineBreak; }
            set { _defPolylineBreak = value; }
        }

        /// <summary>
        /// Get or set default polygon break
        /// </summary>
        public PolygonBreak DefPolygonBreak
        {
            get { return _defPolygonBreak; }
            set { _defPolygonBreak = value; }
        }

        /// <summary>
        /// Get or set measurement form
        /// </summary>
        public frmMeasurement MeasurementForm
        {
            get { return _frmMeasure; }
            set { _frmMeasure = value; }
        } 

        #endregion

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the width of the paper in 1/100 of an inch
        /// </summary>
        private int PaperWidth
        {
            get
            {

                if (_isLandscape)
                    return _paperSize.Height;
                return _paperSize.Width;
            }
        }

        /// <summary>
        /// Gets the heigh of the paper in 1/100 of an inch
        /// </summary>
        private int PaperHeight
        {
            get
            {
                if (_isLandscape)
                    return _paperSize.Width;
                return _paperSize.Height;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Update map frames
        /// </summary>
        /// <param name="mapFrames">Map frames</param>
        public void UpdateMapFrames(List<MapFrame> mapFrames)
        {            
            foreach (MapFrame mf in mapFrames)
            {
                bool isNew = true;
                foreach (MapFrame aMF in _mapFrames)
                {
                    if (mf == aMF)
                    {
                        isNew = false;
                        break;
                    }
                }
                if (isNew)
                {
                    LayoutMap aLM = new LayoutMap(mf);                    
                    AddElement(aLM);
                }
            }

            for (int i = 0; i < _mapFrames.Count; i++)
            {
                MapFrame mf = _mapFrames[i];
                bool isNew = true;
                foreach (MapFrame aMF in mapFrames)
                {
                    if (mf == aMF)
                    {
                        isNew = false;
                        break;
                    }
                }
                if (isNew)
                {
                    LayoutMap aLM = GetLayoutMap(mf);
                    if (aLM != null)
                    {
                        RemoveElement(aLM);
                        i -= 1;
                    }
                }
            }

            this.MapFrames = mapFrames;
        }

        /// <summary>
        /// Update the order of the map frames
        /// </summary>
        public void UpdateMapFrameOrder()
        {
            List<LayoutMap> lms = LayoutMaps;
            for (int i = 0; i < lms.Count; i++)
            {
                lms[i].MapFrame.Order = i;
            }
        }

        /// <summary>
        /// Add a layout element
        /// </summary>
        /// <param name="aElement">layout element</param>
        public void AddElement(LayoutElement aElement)
        {
            _layoutElements.Add(aElement);
            if (aElement.ElementType == ElementType.LayoutMap)
            {
                LayoutMap aLM = (LayoutMap)aElement;
                aLM.MapViewUpdated += new EventHandler(MapViewUpdated);
                if (aLM.MapFrame.Active)
                    _currentLayoutMap = aLM;
            }
        }

        /// <summary>
        /// Remove a layout element
        /// </summary>
        /// <param name="aElement">layout element</param>
        public void RemoveElement(LayoutElement aElement)
        {
            switch (aElement.ElementType)
            {
                case ElementType.LayoutMap:
                    if (LayoutMaps.Count == 1)
                    {
                        MessageBox.Show("There is at least one layout map!", "Alarm");
                        return;
                    }

                    LayoutMap aLM = (LayoutMap)aElement;
                    for (int i = 0; i < _layoutElements.Count; i++)
                    {
                        LayoutElement aLE = _layoutElements[i];
                        switch (aLE.ElementType)
                        {
                            case ElementType.LayoutLegend:
                                if (((LayoutLegend)aLE).LayoutMap == aLM)
                                {
                                    _layoutElements.Remove(aLE);
                                    i -= 1;
                                }
                                break;
                            case ElementType.LayoutScaleBar:
                                if (((LayoutScaleBar)aLE).LayoutMap == aLM)
                                {
                                    _layoutElements.Remove(aLE);
                                    i -= 1;
                                }
                                break;
                            case ElementType.LayoutNorthArraw:
                                if (((LayoutNorthArrow)aLE).LayoutMap == aLM)
                                {
                                    _layoutElements.Remove(aLE);
                                    i -= 1;
                                }
                                break;
                        }                        
                    }
                    _mapFrames.Remove(aLM.MapFrame);
                    _layoutElements.Remove(aElement);
                    SetActiveMapFrame(_mapFrames[0]);
                    OnMapFramesUpdated();
                    break;
                default:
                    _layoutElements.Remove(aElement);
                    break;
            }            
        }

        private void RemoveElements_NotMap()
        {
            foreach (LayoutElement aLE in _layoutElements)
            {
                if (aLE.ElementType != ElementType.LayoutMap)
                    _layoutElements.Remove(aLE);
            }
        }

        /// <summary>
        /// Add a graphic
        /// </summary>
        /// <param name="aGraphic">graphic</param>
        /// <returns>layout graphic</returns>
        public LayoutGraphic AddGraphic(Graphic aGraphic)
        {
            LayoutGraphic aLayoutGraphic = new LayoutGraphic(aGraphic, this);
            AddElement(aLayoutGraphic);

            return aLayoutGraphic;
        }

        /// <summary>
        /// Add a text label element
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="x">center x</param>
        /// <param name="y">center y</param>
        /// <returns>text layout graphic</returns>
        public LayoutGraphic AddText(string text, int x, int y)
        {
            return AddText(text, x, y, _defLabelBreak.Font.Name, _defLabelBreak.Font.Size);
        }

        /// <summary>
        /// Add a text label element
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="x">center x</param>
        /// <param name="y">center y</param>
        /// <param name="fontSize">font size</param>
        /// <returns>text layout graphic</returns>
        public LayoutGraphic AddText(string text, int x, int y, float fontSize)
        {
            return AddText(text, x, y, _defLabelBreak.Font.Name, fontSize);
        }

        /// <summary>
        /// Add a text label element
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="x">center x</param>
        /// <param name="y">center y</param>
        /// <param name="fontName">font name</param>
        /// <param name="fontSize">font size</param>
        /// <returns>text layout graphic</returns>
        public LayoutGraphic AddText(string text, int x, int y, string fontName, float fontSize)
        {
            PointShape aPS = new PointShape();
            aPS.Point = new MeteoInfoC.PointD(x, y);
            LabelBreak aLB = (LabelBreak)_defLabelBreak.Clone();
            aLB.Text = text;
            aLB.Font = new Font(fontName, fontSize);
            Graphic aGraphic = new Graphic(aPS, aLB);
            LayoutGraphic aLayoutGraphic = new LayoutGraphic(aGraphic, this);
            AddElement(aLayoutGraphic);

            return aLayoutGraphic;
        }

        /// <summary>
        /// Add a point graphic
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>point layout graphic</returns>
        public LayoutGraphic AddPoint(int x, int y)
        {
            return AddPoint(x, y, (PointBreak)_defPointBreak.Clone());            
        }

        /// <summary>
        /// Add a point graphic
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="aPB">point break</param>
        /// <returns>point layout graphic</returns>
        public LayoutGraphic AddPoint(int x, int y, PointBreak aPB)
        {
            PointShape aPS = new PointShape();
            aPS.Point = new MeteoInfoC.PointD(x, y);
            Graphic aGraphic = new Graphic(aPS, aPB);
            LayoutGraphic aLayoutGraphic = new LayoutGraphic(aGraphic, this);
            AddElement(aLayoutGraphic);

            return aLayoutGraphic;
        }

        /// <summary>
        /// Add a wind arrow graphic
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Wind arrow layout graphic</returns>
        public LayoutGraphic AddWindArrow(int x, int y)
        {
            WindArraw aWindArraw = new WindArraw();
            aWindArraw.angle = 270;
            aWindArraw.length = 20;
            VectorBreak aVB = new VectorBreak();
            aVB.Color = Color.Black;
            LayoutGraphic wag = new LayoutGraphic(new Graphic(aWindArraw, aVB), this, this.ActiveLayoutMap);
            wag.Left = x;
            wag.Top = y;
            AddElement(wag);

            return wag;
        }

        /// <summary>
        /// Get layout graphic list
        /// </summary>
        /// <returns>layout graphic list</returns>
        public List<LayoutGraphic> GetGraphics()
        {
            List<LayoutGraphic> graphics = new List<LayoutGraphic>();
            foreach (LayoutElement aLE in _layoutElements)
            {
                if (aLE.ElementType == ElementType.LayoutGraphic)
                    graphics.Add((LayoutGraphic)aLE);
            }

            return graphics;
        }

        /// <summary>
        /// Get text graphic list
        /// </summary>
        /// <returns>text graphic list</returns>
        public List<LayoutGraphic> GetTexts()
        {
            List<LayoutGraphic> texts = new List<LayoutGraphic>();
            List<LayoutGraphic> graphics = GetGraphics();
            foreach (LayoutGraphic aLG in graphics)
            {
                if (aLG.Graphic.Legend.GetType() == typeof(LabelBreak))
                    texts.Add(aLG);
            }

            return texts;
        }

        /// <summary>
        /// Get a text graphic by text string
        /// </summary>
        /// <param name="text">text string</param>
        /// <returns>text graphic</returns>
        public LayoutGraphic GetText(string text)
        {
            List<LayoutGraphic> texts = GetTexts();
            foreach (LayoutGraphic aLG in texts)
            {
                if (((LabelBreak)aLG.Graphic.Legend).Text == text)
                    return aLG;
            }

            return null;
        }

        /// <summary>
        /// Add a layout legend
        /// </summary>
        /// <param name="left">left</param>
        /// <param name="top">top</param>
        /// <returns>layout legend</returns>
        public LayoutLegend AddLegend(int left, int top)
        {
            LayoutMap aLM = ActiveLayoutMap;
            LayoutLegend aLL = new LayoutLegend(this, aLM);
            aLL.Left = left;
            aLL.Top = top;
            if (aLM.MapFrame.MapView.LayerSet.LayerNum > 0)
            {
                foreach (MapLayer aLayer in aLM.MapFrame.MapView.LayerSet.Layers)
                {
                    if (aLayer.LayerType != LayerTypes.ImageLayer)
                        aLL.LegendLayer = aLayer;
                }
            }

            AddElement(aLL);

            return aLL;
        }

        /// <summary>
        /// Get layout legend list
        /// </summary>
        /// <returns>layout legend list</returns>
        public List<LayoutLegend> GetLegends()
        {
            List<LayoutLegend> legends = new List<LayoutLegend>();
            foreach (LayoutElement aLE in _layoutElements)
            {
                if (aLE.ElementType == ElementType.LayoutLegend)
                    legends.Add((LayoutLegend)aLE);
            }

            return legends;
        }

        /// <summary>
        /// Add a layout scale bar
        /// </summary>
        /// <param name="left">left</param>
        /// <param name="top">top</param>
        /// <returns>layout scale bar</returns>
        public LayoutScaleBar AddScaleBar(int left, int top)
        {
            LayoutMap aLM = ActiveLayoutMap;
            LayoutScaleBar aLSB = new LayoutScaleBar(aLM);
            aLSB.Left = left;
            aLSB.Top = top;
            AddElement(aLSB);

            return aLSB;
        }

        /// <summary>
        /// Add a layout north arrow
        /// </summary>
        /// <param name="left">left</param>
        /// <param name="top">top</param>
        /// <returns>layout north arrow</returns>
        public LayoutNorthArrow AddNorthArrow(int left, int top)
        {
            LayoutMap aLM = ActiveLayoutMap;
            LayoutNorthArrow aLNA = new LayoutNorthArrow(aLM);
            aLNA.Left = left;
            aLNA.Top = top;
            AddElement(aLNA);

            return aLNA;
        }

        /// <summary>
        /// Add a map frame
        /// </summary>
        /// <param name="mapFrame">The map frame</param>
        /// <returns>The layout map</returns>
        public LayoutMap AddMapFrame(MapFrame mapFrame)
        {
            LayoutMap aLM = new LayoutMap(mapFrame);
            //aLM.MapViewUpdated += new EventHandler(MapViewUpdated);
            AddElement(aLM);
            _mapFrames.Add(mapFrame);

            return aLM;
        }

        /// <summary>
        /// Remove a map frame
        /// </summary>
        /// <param name="mapFrame">The map frame</param>
        public void RemoveMapFrame(MapFrame mapFrame)
        {
            LayoutMap aLM = GetLayoutMap(mapFrame);
            if (aLM != null)
            {
                _layoutElements.Remove(aLM);
                _mapFrames.Remove(mapFrame);

                SetActiveMapFrame(_mapFrames[0]);
                OnMapFramesUpdated();
            }
        }

        /// <summary>
        /// Get a layout map by map frame
        /// </summary>
        /// <param name="mapFrame">The map frame</param>
        /// <returns>The layout map</returns>
        public LayoutMap GetLayoutMap(MapFrame mapFrame)
        {
            LayoutMap aLM = null;
            foreach (LayoutMap lm in LayoutMaps)
            {
                if (lm.MapFrame == mapFrame)
                {
                    aLM = lm;
                    break;
                }
            }

            return aLM;
        }

        /// <summary>
        /// Get layout map by name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>The layout map</returns>
        public LayoutMap GetLayoutMap(string name)
        {
            LayoutMap aLM = null;
            foreach (LayoutMap lm in LayoutMaps)
            {
                if (lm.MapFrame.Text == name)
                {
                    aLM = lm;
                    break;
                }
            }

            return aLM;
        }

        /// <summary>
        /// Get layout map by a point
        /// </summary>
        /// <param name="aPoint">The point</param>
        /// <returns>The layout map</returns>
        public LayoutMap GetLayoutMap(Point aPoint)
        {
            //LayoutMap lm = null;
            for (int i = LayoutMaps.Count - 1; i >= 0; i--)
            {
                LayoutMap aLM = LayoutMaps[i];
                if (MIMath.PointInRectangle(aPoint, aLM.Bounds))
                    return aLM;
            }

            return null;

            //foreach (LayoutMap aLM in LayoutMaps)
            //{
            //    if (MIMath.PointInRectangle(aPoint, aLM.Bounds))
            //    {
            //        lm = aLM;
            //        break;
            //    }
            //}

            //return lm;
        }

        private int GetLayoutMapIndex(LayoutMap aLM)
        {
            return LayoutMaps.IndexOf(aLM);
        }

        private bool IsInLayoutMaps(Point aPoint)
        {
            foreach (LayoutMap aLM in LayoutMaps)
            {
                if (MIMath.PointInRectangle(aPoint, aLM.Bounds))
                    return true;
            }

            return false;
        }

        private void RemoveAllLayoutMaps()
        {
            List<LayoutElement> aList = new List<LayoutElement>();
            foreach (LayoutElement aLE in _layoutElements)
            {
                switch (aLE.ElementType)
                {
                    case ElementType.LayoutGraphic:
                        aList.Add(aLE);
                        break;
                }
            }

            _layoutElements = aList;
        }

        /// <summary>
        /// Set a map frame as active
        /// </summary>
        /// <param name="mapFrame">map frame</param>
        public void SetActiveMapFrame(MapFrame mapFrame)
        {
            foreach (MapFrame mf in _mapFrames)
                mf.Active = false;

            mapFrame.Active = true;
            OnActiveMapFrameChanged();
        }

        private Rectangle GetElementViewExtent(LayoutElement aLE)
        {
            PointF aP = aLE.PageToScreen(aLE.Left, aLE.Top, _pageLocation, _zoom);
            Rectangle rect = new Rectangle((int)aP.X, (int)aP.Y, (int)(aLE.Width * _zoom), (int)(aLE.Height * _zoom));

            return rect;
        }

        /// <summary>
        /// Set properties
        /// </summary>
        public void SetProperties()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("SmoothingMode", "SmoothingMode");
            objAttr.Add("PageForeColor", "ForeColor");
            objAttr.Add("PageBackColor", "BackColor");                        
            objAttr.Add("GridFont", "GridFont");
            CustomProperty cp = new CustomProperty(this, objAttr);
            frmProperty aFrmP = new frmProperty();
            aFrmP.SetObject(cp);
            aFrmP.ShowDialog();
        }       

        ///// <summary>
        ///// Update illustration position
        ///// </summary>
        //public void SetIllustrationPos()
        //{
        //    int width, height, left, top;            
        //    width = (int)(_mapView.Width * m_IllustrationSet.WidthRatio);
        //    height = (int)(_mapView.Height * m_IllustrationSet.HeightRatio);
        //    switch (m_IllustrationSet.PositionOption)
        //    {
        //        case IllusPos.RightBottom:
        //            left = _mapView.Right - width - m_IllustrationSet.XShift;
        //            top = _mapView.Bottom - height - m_IllustrationSet.YShift;
        //            break;
        //        default:
        //            left = _mapView.Left + m_IllustrationSet.XShift;
        //            top = _mapView.Bottom - height - m_IllustrationSet.YShift;
        //            break;
        //    }

        //    m_IllustrationMap.Width = width;
        //    m_IllustrationMap.Height = height;
        //    m_IllustrationMap.Left = left;
        //    m_IllustrationMap.Top = top;
        //}

        ///// <summary>
        ///// Update map view
        ///// </summary>
        ///// <param name="aMapView">a map view</param>
        //public void UpdateIllustration(MapView aMapView)
        //{
        //    if (_illusMap.Visible)
        //    {
        //        _illusMap.LinkedMapView = aMapView;
        //    }            
        //}        

        ///// <summary>
        ///// Set main tilte position
        ///// </summary>
        //public void SetMainTitlePos()
        //{
        //    //_mainTitle.Left = _mapView.Left + (_mapView.Width - _mainTitle.Width) / 2;
        //    //_mainTitle.Top = _mapView.Top - _mainTitle.Height - 10;
        //    //_mainTitle.UpdateControlSize();
        //    //_mainTitle.Refresh();
        //}

        ///// <summary>
        ///// Update controls
        ///// </summary>
        //public void UpdateControls()
        //{
        //    _mapView.PaintLayers();
        //    //m_IllustrationMap.PaintLayers();
        //    //m_Legend.Refresh();
        //    //_mainTitle.Refresh();
        //    this.Refresh();
        //}

        ///// <summary>
        ///// Update controls size
        ///// </summary>
        //public void UpdateControlSize()
        //{
        //    if (!_AutoResize)
        //        return;

        //    //Set legend
        //    if (_defaultLegend.Visible)
        //    {
        //        switch (_defaultLegend.LegendStyle)
        //        {
        //            case LegendStyles.Bar_Horizontal:
        //                _defaultLegend.Left = _pageBounds.Left + _pageBounds.Width / 4;
        //                _defaultLegend.Width = _pageBounds.Width / 2;
        //                _defaultLegend.Top = _pageBounds.Bottom - 40;
        //                _defaultLegend.Height = 30;
        //                break;
        //            case LegendStyles.Bar_Vertical:
        //                _defaultLegend.Left = _pageBounds.Right - _defaultLegend.Width - 5;
        //                //m_Legend.Width = 30;
        //                _defaultLegend.Top = _pageBounds.Top + _pageBounds.Height / 4;
        //                _defaultLegend.Height = _pageBounds.Height / 2;
        //                break;
        //            case LegendStyles.Normal:
        //                _defaultLegend.Left = _pageBounds.Right - _defaultLegend.Width - 5;
        //                _defaultLegend.Top = _pageBounds.Top + 60;                       
        //                break;
        //        }
        //    }            

        //    ////Set MapView              
        //    //int shift;
        //    //if (m_Legend.DrawLegend)
        //    //{
        //    //    switch (m_Legend.LegendStyle)
        //    //    {
        //    //        case LegendStyles.Bar_Horizontal:                    
        //    //            shift = _mapView.Bottom - m_Legend.Top;
        //    //            _mapView.Height = _mapView.Height - shift - 30;
        //    //            _mapView.Width = _pageBounds.Width - _mapView.Left - 15;
        //    //            break;
        //    //        case LegendStyles.Bar_Vertical:
        //    //        case LegendStyles.Normal:
        //    //            shift = _mapView.Right - m_Legend.Left;
        //    //            _mapView.Width = _mapView.Width - shift - 10;
        //    //            _mapView.Height = _pageBounds.Height - _mapView.Top - 30;
        //    //            break;                    
        //    //    }
        //    //}            
        //    //_mapView.PaintLayers();            

        //    ////Set Illustration
        //    //if (m_IllustrationSet.DrawIllustration)
        //    //    SetIllustrationPos();

        //    ////Set Title
        //    //SetMainTitlePos();

        //    this.Refresh();
        //}        

        ///// <summary>
        ///// Update legend by layers
        ///// </summary>
        //public void UpdateLegendByLayers()
        //{
        //    //Update legend
        //    bool oldDrawLegend = _defaultLegend.Visible;

        //    _defaultLegend.Visible = false;
        //    if (_defaultLegend.FirstMeteoLayer)
        //    {
        //        for (int i = 0; i < _currentLayoutMap.MapFrame.MapView.LayerSet.Layers.Count; i++)
        //        {
        //            MapLayer aLayer = _currentLayoutMap.MapFrame.MapView.LayerSet.Layers[_currentLayoutMap.MapFrame.MapView.LayerSet.Layers.Count - 1 - i];
        //            if (aLayer.LayerType != LayerTypes.ImageLayer)
        //            {
        //                if (aLayer.Visible && aLayer.LayerDrawType != LayerDrawType.Map &&
        //                    aLayer.legendScheme.LegendType != LegendType.SingleSymbol)
        //                {
        //                    _defaultLegend.Visible = true;
        //                    _defaultLegend.LegendLayer = aLayer;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < _currentLayoutMap.MapFrame.MapView.LayerSet.Layers.Count; i++)
        //        {
        //            MapLayer aLayer = _currentLayoutMap.MapFrame.MapView.LayerSet.Layers[_currentLayoutMap.MapFrame.MapView.LayerSet.Layers.Count - 1 - i];
        //            if (aLayer.LayerType != LayerTypes.ImageLayer)
        //            {
        //                if (aLayer.Visible && aLayer.Expanded && aLayer.legendScheme.LegendType != LegendType.SingleSymbol)
        //                {
        //                    _defaultLegend.Visible = true;
        //                    _defaultLegend.LegendLayer = aLayer;
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    if (_defaultLegend.Visible != oldDrawLegend)
        //    {
        //        UpdateControlSize();
        //    }

        //}

        //private void UpdateWindArrawLegend()
        //{
        //    _defaultWindArraw.Visible = false;
        //    for (int i = 0; i < _currentLayoutMap.MapFrame.MapView.LayerSet.Layers.Count; i++)
        //    {
        //        MapLayer aLayer = _currentLayoutMap.MapFrame.MapView.LayerSet.Layers[_currentLayoutMap.MapFrame.MapView.LayerSet.Layers.Count - 1 - i];
        //        if (aLayer.LayerType == LayerTypes.VectorLayer)
        //        {
        //            if (aLayer.Visible && aLayer.LayerDrawType == LayerDrawType.Vector)
        //            {
        //                _defaultWindArraw.Visible = true;
        //                float zoom = ((VectorLayer)aLayer).DrawingZoom;
        //                ((VectorBreak)_defaultWindArraw.Graphic.Legend).Zoom = zoom;
        //                float max = 30.0f / zoom;
        //                WindArraw aWA = (WindArraw)_defaultWindArraw.Graphic.Shape;
        //                int llen = 5;
        //                for (i = 10; i <= 100; i += 5)
        //                {
        //                    if (max < i)
        //                    {
        //                        llen = i - 5;
        //                        break;
        //                    }
        //                }
        //                aWA.length = llen;                        
        //                aWA.size = 5;
        //                aWA.Value = 0;
        //                _defaultWindArraw.UpdateControlSize();
        //                //if (_defaultLegend.Visible)
        //                //    _defaultWindArraw.Top = _defaultLegend.Bottom + 10;
        //                //else
        //                //    _defaultWindArraw.Top = _defaultLayoutMap.Bottom;
        //                break;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Set paper size
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        public void SetPaperSize(int width, int height)
        {
            this.PaperSize = new PaperSize("Custom", width, height);
        }

        private void UpdatePageSet()
        {
            RectangleF aRect = PaperToScreen(new RectangleF(0, 0, PaperWidth, PaperHeight));
            _pageBounds.Width = (int)aRect.Width;
            _pageBounds.Height = (int)aRect.Height;
        }        

        /// <summary>
        /// Export to picture
        /// </summary>
        /// <param name="aFile">file path</param>
        public void ExportToPicture(string aFile)
        {
            Bitmap bitmap = new Bitmap(_pageBounds.Width, _pageBounds.Height, PixelFormat.Format32bppPArgb);
            //bitmap.SetResolution(300, 300);            
            Rectangle rect = new Rectangle(0, 0, _pageBounds.Width + _pageBounds.Left,
                    _pageBounds.Height + _pageBounds.Top);

            if (Path.GetExtension(aFile).ToLower() == ".wmf")
            {                
                Graphics gs = Graphics.FromImage(bitmap);                
                Metafile mf = new Metafile(aFile, gs.GetHdc(), rect, MetafileFrameUnit.Pixel);

                Graphics g = Graphics.FromImage(mf);
                PaintGraphics(g);
                g.Dispose();
                mf.Dispose();
                gs.Dispose();
            }
            else
            {
                //PaintGraphicsWithoutBound();
                //DrawToBitmap(bitmap, rect);               
                //bitmap = _layoutBitmap;
                Graphics gb = Graphics.FromImage(bitmap);
                Rectangle aRect = new Rectangle();
                aRect.X = _pageBounds.X - 2;
                aRect.Y = _pageBounds.Y - 2;
                aRect.Width = _pageBounds.Width + 3;
                aRect.Height = _pageBounds.Height + 3;
                gb.FillRectangle(new SolidBrush(_PageBackColor), aRect); 
                PaintGraphics(gb);

                //Bitmap newbitmap = new Bitmap(_pageBounds.Width, _pageBounds.Height, PixelFormat.Format32bppPArgb);
                //newbitmap.SetResolution(300, 300);
                //Graphics newg = Graphics.FromImage(newbitmap);
                //newg.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //newg.DrawImage(bitmap, new Rectangle(0, 0, newbitmap.Width, newbitmap.Height), new Rectangle(0, 0, bitmap.Width,
                //    bitmap.Height), GraphicsUnit.Pixel);
                //newg.Dispose();
                ////bitmap.SetResolution(300, 300);
                switch (Path.GetExtension(aFile).ToLower())
                {
                    case ".gif":
                        OctreeQuantizer quantizer = new OctreeQuantizer(255, 8);
                        Bitmap quantizered = quantizer.Quantize(bitmap);
                        quantizered.Save(aFile, ImageFormat.Gif);
                        quantizered.Dispose();                        
                        //bitmap.Save(aFile, ImageFormat.Gif);
                        break;
                    case ".bmp":
                        bitmap.Save(aFile, ImageFormat.Bmp);
                        break;
                    case ".jpg":
                        bitmap.Save(aFile, ImageFormat.Jpeg);
                        break;
                    case ".tif":
                        bitmap.Save(aFile, ImageFormat.Tiff);
                        break;
                    case ".png":
                        bitmap.Save(aFile, ImageFormat.Png);
                        //newbitmap.Save(aFile, ImageFormat.Png);
                        break;
                }

                gb.Dispose();
                //PaintGraphics();
            }

            bitmap.Dispose();
        }

        /// <summary>
        /// Export to picture
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <param name="resolution">Resolution with DPI</param>
        public void ExportToPicture(string aFile, float resolution)
        {
            Bitmap bitmap = new Bitmap(_pageBounds.Width, _pageBounds.Height, PixelFormat.Format32bppPArgb);
            //bitmap.SetResolution(resolution, resolution);            
            Rectangle rect = new Rectangle(0, 0, _pageBounds.Width + _pageBounds.Left,
                    _pageBounds.Height + _pageBounds.Top);

            if (Path.GetExtension(aFile).ToLower() == ".wmf")
            {
                Graphics gs = Graphics.FromImage(bitmap);
                Metafile mf = new Metafile(aFile, gs.GetHdc(), rect, MetafileFrameUnit.Pixel);

                Graphics g = Graphics.FromImage(mf);
                PaintGraphics(g);
                g.Dispose();
                mf.Dispose();
            }
            else
            {               
                Graphics gb = Graphics.FromImage(bitmap);
                Rectangle aRect = new Rectangle();
                aRect.X = _pageBounds.X - 2;
                aRect.Y = _pageBounds.Y - 2;
                aRect.Width = _pageBounds.Width + 3;
                aRect.Height = _pageBounds.Height + 3;
                gb.FillRectangle(new SolidBrush(_PageBackColor), aRect);
                PaintGraphics(gb);
                bitmap.SetResolution(resolution, resolution);
                switch (Path.GetExtension(aFile).ToLower())
                {
                    case ".gif":
                        OctreeQuantizer quantizer = new OctreeQuantizer(255, 8);
                        Bitmap quantizered = quantizer.Quantize(bitmap);
                        quantizered.SetResolution(resolution, resolution);
                        quantizered.Save(aFile, ImageFormat.Gif);
                        quantizered.Dispose();
                        //bitmap.Save(aFile, ImageFormat.Gif);
                        break;
                    case ".bmp":                        
                        bitmap.Save(aFile, ImageFormat.Bmp);
                        break;
                    case ".jpg":
                        bitmap.Save(aFile, ImageFormat.Jpeg);
                        break;
                    case ".tif":
                        bitmap.Save(aFile, ImageFormat.Tiff);
                        break;
                    case ".png":
                        bitmap.Save(aFile, ImageFormat.Png);                        
                        break;
                }

                gb.Dispose();             
            }

            bitmap.Dispose();
        }        

        /// <summary>
        /// Calculates which edge of a rectangle the point intersects with, within a certain limit
        /// </summary>
        private static Edge IntersectElementEdge(RectangleF screen, PointF pt, float limit)
        {
            RectangleF ptRect = new RectangleF(pt.X - limit, pt.Y - limit, 2F * limit, 2F * limit);
            if ((pt.X >= screen.X - limit && pt.X <= screen.X + limit) && (pt.Y >= screen.Y - limit && pt.Y <= screen.Y + limit))
                return Edge.TopLeft;
            if ((pt.X >= screen.X + screen.Width - limit && pt.X <= screen.X + screen.Width + limit) && (pt.Y >= screen.Y - limit && pt.Y <= screen.Y + limit))
                return Edge.TopRight;
            if ((pt.X >= screen.X + screen.Width - limit && pt.X <= screen.X + screen.Width + limit) && (pt.Y >= screen.Y + screen.Height - limit && pt.Y <= screen.Y + screen.Height + limit))
                return Edge.BottomRight;
            if ((pt.X >= screen.X - limit && pt.X <= screen.X + limit) && (pt.Y >= screen.Y + screen.Height - limit && pt.Y <= screen.Y + screen.Height + limit))
                return Edge.BottomLeft;
            if (ptRect.IntersectsWith(new RectangleF(screen.X, screen.Y, screen.Width, 1F)))
                return Edge.Top;
            if (ptRect.IntersectsWith(new RectangleF(screen.X, screen.Y, 1F, screen.Height)))
                return Edge.Left;
            if (ptRect.IntersectsWith(new RectangleF(screen.X, screen.Y + screen.Height, screen.Width, 1F)))
                return Edge.Bottom;
            if (ptRect.IntersectsWith(new RectangleF(screen.X + screen.Width, screen.Y, 1F, screen.Height)))
                return Edge.Right;
            return Edge.None;
        }

        private List<LayoutElement> SelectElements(Point aPoint, List<LayoutElement> baseElements, int limit)
        {
            List<LayoutElement> selectedElements = new List<LayoutElement>();
            for (int i = baseElements.Count - 1; i >= 0; i --)
            {
                LayoutElement element = baseElements[i];
                if (element.Visible)
                {
                    Rectangle rect = element.Bounds;
                    if (rect.Width < 5)
                        rect.Width = 5;
                    if (rect.Height < 5)
                        rect.Height = 5;
                    rect.Inflate(limit, limit);
                    //Rectangle rect = PageToScreen(element.Bounds);
                    if (MIMath.PointInRectangle(aPoint, rect))
                    {
                        selectedElements.Add(element);
                    }
                }
            }

            return selectedElements;
        }

        private bool SelectEditVertices(Point aPoint, Shape.Shape aShape, ref List<PointD> vertices, ref int vIdx)
        {
            List<PointD> points = aShape.GetPoints();
            int buffer = 4;
            Rectangle rect = new Rectangle(aPoint.X - buffer / 2, aPoint.Y - buffer / 2, buffer, buffer);
            vertices.Clear();
            PointD aPD = new PointD();
            for (int i = 0; i < points.Count; i++)
            {
                if (MIMath.PointInRectangle(points[i], rect))
                {
                    vIdx = i;
                    vertices.Add(points[i]);
                    if (aShape.ShapeType == ShapeTypes.Polyline)
                    {                        
                        if (i == 0)
                            vertices.Add(points[i + 1]);
                        else if (i == points.Count - 1)
                            vertices.Add(points[i - 1]);
                        else
                        {                            
                            vertices.Add(points[i - 1]);
                            vertices.Add(points[i + 1]);
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            vertices.Add(points[i + 1]);
                            aPD = points[points.Count - 1];
                            if (aPD.X == points[i].X && aPD.Y == points[i].Y)
                                vertices.Add(points[points.Count - 2]);
                            else
                                vertices.Add(aPD);
                        }
                        else if (i == points.Count - 1)
                        {
                            vertices.Add(points[i - 1]);
                            aPD = points[0];
                            if (aPD.X == points[i].X && aPD.Y == points[i].Y)
                                vertices.Add(points[1]);
                            else
                                vertices.Add(points[0]);
                        }
                        else
                        {
                            vertices.Add(points[i - 1]);
                            vertices.Add(points[i + 1]);
                        }
                    }
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Coordinate Convert
        /// <summary>
        /// Converts a point in screen coordinants to paper coordinants in 1/100 of an inch
        /// </summary>
        /// <returns></returns>
        private PointF ScreenToPaper(PointF screen)
        {
            return ScreenToPaper(screen.X, screen.Y);
        }

        /// <summary>
        /// Converts a point in screen coordinants to paper coordinants in 1/100 of an inch
        /// </summary>
        /// <returns></returns>
        private PointF ScreenToPaper(float screenX, float screenY)
        {
            float paperX = (screenX - _pageLocation.X) / _zoom / 96F * 100F;
            float paperY = (screenY - _pageLocation.Y) / _zoom / 96F * 100F;
            return (new PointF(paperX, paperY));
        }

        /// <summary>
        /// Converts a rectangle in screen coordinants to paper coordiants in 1/100 of an inch
        /// </summary>
        /// <returns></returns>
        private RectangleF ScreenToPaper(RectangleF screen)
        {
            return ScreenToPaper(screen.X, screen.Y, screen.Width, screen.Height);
        }

        /// <summary>
        /// Converts a rectangle in screen coordinants to paper coordiants in 1/100 of an inch
        /// </summary>
        /// <returns></returns>
        private RectangleF ScreenToPaper(float screenX, float screenY, float screenW, float screenH)
        {
            PointF paperTL = ScreenToPaper(screenX, screenY);
            PointF paperBR = ScreenToPaper(screenX + screenW, screenY + screenH);
            return new RectangleF(paperTL.X, paperTL.Y, paperBR.X - paperTL.X, paperBR.Y - paperTL.Y);
        }

        /// <summary>
        /// Converts between a point in paper coordinants in 1/100th of an inch to screen coordinants
        /// </summary>
        /// <returns></returns>
        private PointF PaperToScreen(PointF paper)
        {
            return PaperToScreen(paper.X, paper.Y);
        }

        /// <summary>
        /// Converts between a point in paper coordinants in 1/100th of an inch to screen coordinants
        /// </summary>
        /// <returns></returns>
        private PointF PaperToScreen(float paperX, float paperY)
        {
            float screenX = (paperX / 100F * 96F * _zoom) + _pageLocation.X;
            float screenY = (paperY / 100F * 96F * _zoom) + _pageLocation.Y;
            return (new PointF(screenX, screenY));
        }

        /// <summary>
        /// Converts between a rectangle in paper coordinants in 1/100th of an inch to screen coordinants
        /// </summary>
        /// <returns></returns>
        private RectangleF PaperToScreen(RectangleF paper)
        {
            return PaperToScreen(paper.X, paper.Y, paper.Width, paper.Height);
        }

        /// <summary>
        /// Converts a rectangle in paper coordiants in 1/100 of an inch to screen coordinants
        /// </summary>
        /// <returns></returns>
        private RectangleF PaperToScreen(float paperX, float paperY, float paperW, float paperH)
        {
            PointF screenTL = PaperToScreen(paperX, paperY);
            PointF screenBR = PaperToScreen(paperX + paperW, paperY + paperH);
            return new RectangleF(screenTL.X, screenTL.Y, screenBR.X - screenTL.X, screenBR.Y - screenTL.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageX"></param>
        /// <param name="pageY"></param>
        /// <returns></returns>
        private PointF PageToScreen(float pageX, float pageY)
        {
            float x = pageX * _zoom + _pageLocation.X;
            float y = pageY * _zoom + _pageLocation.Y;
            return (new PointF(x, y));
        }

        private RectangleF PageToScreen(float pageX, float pageY, float pageW, float pageH)
        {
            PointF screenTL = PageToScreen(pageX, pageY);
            PointF screenBR = PageToScreen(pageX + pageW, pageY + pageH);
            return new RectangleF(screenTL.X, screenTL.Y, screenBR.X - screenTL.X, screenBR.Y - screenTL.Y);
        }

        private Rectangle PageToScreen(Rectangle rect)
        {
            PointF screenTL = PageToScreen(rect.X, rect.Y);
            PointF screenBR = PageToScreen(rect.Right, rect.Bottom);
            return new Rectangle((int)screenTL.X, (int)screenTL.Y, (int)(screenBR.X - screenTL.X), (int)(screenBR.Y - screenTL.Y));
        }

        private PointF ScreenToPage(float screenX, float screenY)
        {
            float x = (screenX - _pageLocation.X) / _zoom;
            float y = (screenY - _pageLocation.Y) / _zoom;
            return (new PointF(x, y));
        }

        /// <summary>
        /// Screen to page coordinates
        /// </summary>
        /// <param name="screenX">screen x</param>
        /// <param name="screenY">screen y</param>
        /// <returns>page position</returns>
        public Point ScreenToPage(int screenX, int screenY)
        {
            float x = (screenX - _pageLocation.X) / _zoom;
            float y = (screenY - _pageLocation.Y) / _zoom;
            return (new Point((int)x, (int)y));
        }

        private bool PointInDrawingRegion(int x, int y)
        {
            int width = this.Width;
            int height = this.Height;
            if (_vScrollBar.Visible)
                width = width - _vScrollBar.Width;
            if (_hScrollBar.Visible)
                height = height - _hScrollBar.Height;

            if (x <= width && y <= height)
                return true;
            else
                return false;
        }

        #endregion

        #region XML import and export

        /// <summary>
        /// Save XML file
        /// </summary>
        /// <param name="aFile">file path</param>
        public void SaveXMLFile(string aFile)
        {           
            XmlDocument doc = new XmlDocument();
            //doc.LoadXml("<MeteoInfo name='" + Path.GetFileNameWithoutExtension(aFile) + "' type='projectfile'></MeteoInfo>");            
            //XmlElement root = doc.DocumentElement;

            XmlDeclaration xmldecl;
            xmldecl = doc.CreateXmlDeclaration("1.0", "gb2312", null);
            doc.AppendChild(xmldecl);
            XmlElement root = doc.CreateElement("Layout");
            doc.AppendChild(root);            

            //Add MapLayout content
            ExportProjectXML(ref doc, root);

            //Save project file            
            doc.Save(aFile);
        }

        /// <summary>
        /// Load XML file
        /// </summary>
        /// <param name="aFile">file path</param>
        public void LoadXMLFile(string aFile)
        {
            if (File.Exists(aFile))
            {
                for (int i = 0; i < _layoutElements.Count; i++)
                {
                    if (i == _layoutElements.Count)
                        break;

                    LayoutElement aLE = _layoutElements[i];
                    if (aLE.ElementType == ElementType.LayoutGraphic)
                    {
                        if (!((LayoutGraphic)aLE).IsTitle)
                            _layoutElements.Remove(aLE);
                    }
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(aFile);
                XmlElement root = doc.DocumentElement;

                //Load MapLayout content
                ImportProjectXML(root);

                this.Refresh();
            }
        }

        /// <summary>
        /// Export project XML content
        /// </summary>
        /// <param name="m_Doc">XML document</param>
        /// <param name="parent">parent XML element</param>
        public void ExportProjectXML(ref XmlDocument m_Doc, XmlElement parent)
        {
            ExportLayout(ref m_Doc, parent);            
        }        

        private void ExportLayout(ref XmlDocument m_Doc, XmlElement parent)
        {
            XmlElement layout = m_Doc.CreateElement("Layout");

            //Add attribute
            XmlAttribute BackColor = m_Doc.CreateAttribute("BackColor");
            XmlAttribute ForeColor = m_Doc.CreateAttribute("ForeColor");
            XmlAttribute SmoothingMode = m_Doc.CreateAttribute("SmoothingMode");                                          
            XmlAttribute PaperSizeName = m_Doc.CreateAttribute("PaperSizeName");
            XmlAttribute PaperSizeWidth = m_Doc.CreateAttribute("PaperSizeWidth");
            XmlAttribute PaperSizeHeight = m_Doc.CreateAttribute("PaperSizeHeight");
            XmlAttribute Landscape = m_Doc.CreateAttribute("Landscape");

            BackColor.InnerText = ColorTranslator.ToHtml(_PageBackColor);
            ForeColor.InnerText = ColorTranslator.ToHtml(_PageForeColor);
            SmoothingMode.InnerText = Enum.GetName(typeof(SmoothingMode), _smoothingMode);                                             
            PaperSizeName.InnerText = _paperSize.PaperName;
            PaperSizeWidth.InnerText = _paperSize.Width.ToString();
            PaperSizeHeight.InnerText = _paperSize.Height.ToString();
            Landscape.InnerText = _isLandscape.ToString();

            layout.Attributes.Append(BackColor);
            layout.Attributes.Append(ForeColor);
            layout.Attributes.Append(SmoothingMode);                                             
            layout.Attributes.Append(PaperSizeName);
            layout.Attributes.Append(PaperSizeWidth);
            layout.Attributes.Append(PaperSizeHeight);
            layout.Attributes.Append(Landscape);

            parent.AppendChild(layout);

            //Add layout elements
            AddLayoutElements(ref m_Doc, layout);
        }

        private void AddLayoutElements(ref XmlDocument doc, XmlElement parent)
        {
            XmlElement layoutElements = doc.CreateElement("LayoutElements");
            foreach (LayoutElement aElement in _layoutElements)
            {
                switch (aElement.ElementType)
                {
                    case ElementType.LayoutMap:
                        AddLayoutMapElement(ref doc, layoutElements, (LayoutMap)aElement);
                        break;
                    case ElementType.LayoutIllustration:
                        //AddIllustrationElement(ref doc, layoutElements, (LayoutIllustrationMap)aElement);
                        break;
                    case ElementType.LayoutLegend:
                        AddLayoutLegendElement(ref doc, layoutElements, (LayoutLegend)aElement);
                        break;
                    case ElementType.LayoutGraphic:
                        AddLayoutGraphicElement(ref doc, layoutElements, (LayoutGraphic)aElement);
                        break;
                    case ElementType.LayoutScaleBar:
                        AddLayoutScaleBarElement(ref doc, layoutElements, (LayoutScaleBar)aElement);
                        break;
                    case ElementType.LayoutNorthArraw:
                        AddLayoutNorthArrowElement(ref doc, layoutElements, (LayoutNorthArrow)aElement);
                        break;
                }
            }
            parent.AppendChild(layoutElements);
        }

        private void AddLayoutMapElement(ref XmlDocument m_Doc, XmlElement parent, LayoutMap aMap)
        {
            XmlElement layoutMap = m_Doc.CreateElement("LayoutMap");
            XmlAttribute elementType = m_Doc.CreateAttribute("ElementType");
            XmlAttribute left = m_Doc.CreateAttribute("Left");
            XmlAttribute top = m_Doc.CreateAttribute("Top");
            XmlAttribute width = m_Doc.CreateAttribute("Width");
            XmlAttribute height = m_Doc.CreateAttribute("Height");
            XmlAttribute DrawMapNeatLine = m_Doc.CreateAttribute("DrawNeatLine");
            XmlAttribute MapNeatLineColor = m_Doc.CreateAttribute("NeatLineColor");
            XmlAttribute MapNeatLineSize = m_Doc.CreateAttribute("NeatLineSize");
            XmlAttribute GridLineColor = m_Doc.CreateAttribute("GridLineColor");
            XmlAttribute GridLineSize = m_Doc.CreateAttribute("GridLineSize");
            XmlAttribute GridLineStyle = m_Doc.CreateAttribute("GridLineStyle");
            XmlAttribute DrawGridLine = m_Doc.CreateAttribute("DrawGridLine");
            XmlAttribute DrawGridLabel = m_Doc.CreateAttribute("DrawGridLabel");
            XmlAttribute GridFontName = m_Doc.CreateAttribute("GridFontName");
            XmlAttribute GridFontSize = m_Doc.CreateAttribute("GridFontSize");
            XmlAttribute GridXDelt = m_Doc.CreateAttribute("GridXDelt");
            XmlAttribute GridYDelt = m_Doc.CreateAttribute("GridYDelt");
            XmlAttribute GridXOrigin = m_Doc.CreateAttribute("GridXOrigin");
            XmlAttribute GridYOrigin = m_Doc.CreateAttribute("GridYOrigin");
            XmlAttribute gridLabelPosition = m_Doc.CreateAttribute("GridLabelPosition");

            elementType.InnerText = aMap.ElementType.ToString();
            left.InnerText = aMap.Left.ToString();
            top.InnerText = aMap.Top.ToString();
            width.InnerText = aMap.Width.ToString();
            height.InnerText = aMap.Height.ToString();
            DrawMapNeatLine.InnerText = aMap.DrawNeatLine.ToString();
            MapNeatLineColor.InnerText = ColorTranslator.ToHtml(aMap.NeatLineColor);
            MapNeatLineSize.InnerText = aMap.NeatLineSize.ToString();
            GridLineColor.InnerText = ColorTranslator.ToHtml(aMap.GridLineColor);
            GridLineSize.InnerText = aMap.GridLineSize.ToString();
            GridLineStyle.InnerText = Enum.GetName(typeof(DashStyle), aMap.GridLineStyle);
            DrawGridLine.InnerText = aMap.DrawGridLine.ToString();
            DrawGridLabel.InnerText = aMap.DrawGridLabel.ToString();
            GridFontName.InnerText = aMap.GridFont.Name;
            GridFontSize.InnerText = aMap.GridFont.Size.ToString();
            GridXDelt.InnerText = aMap.GridXDelt.ToString();
            GridYDelt.InnerText = aMap.GridYDelt.ToString();
            GridXOrigin.InnerText = aMap.GridXOrigin.ToString();
            GridYOrigin.InnerText = aMap.GridYOrigin.ToString();
            gridLabelPosition.InnerText = Enum.GetName(typeof(GridLabelPosition), aMap.GridLabelPosition);
          
            layoutMap.Attributes.Append(elementType);
            layoutMap.Attributes.Append(left);
            layoutMap.Attributes.Append(top);
            layoutMap.Attributes.Append(width);
            layoutMap.Attributes.Append(height);
            layoutMap.Attributes.Append(DrawMapNeatLine);
            layoutMap.Attributes.Append(MapNeatLineColor);
            layoutMap.Attributes.Append(MapNeatLineSize);
            layoutMap.Attributes.Append(GridLineColor);
            layoutMap.Attributes.Append(GridLineSize);
            layoutMap.Attributes.Append(GridLineStyle);
            layoutMap.Attributes.Append(DrawGridLine);
            layoutMap.Attributes.Append(DrawGridLabel);
            layoutMap.Attributes.Append(GridFontName);
            layoutMap.Attributes.Append(GridFontSize);
            layoutMap.Attributes.Append(GridXDelt);
            layoutMap.Attributes.Append(GridYDelt);
            layoutMap.Attributes.Append(GridXOrigin);
            layoutMap.Attributes.Append(GridYOrigin);
            layoutMap.Attributes.Append(gridLabelPosition);

            parent.AppendChild(layoutMap);
        }

        //private void AddIllustrationElement(ref XmlDocument m_Doc, XmlElement parent, LayoutIllustrationMap aIllustration)
        //{
        //    XmlElement LayoutMap = m_Doc.CreateElement("LayoutIllustration");
        //    XmlAttribute elementType = m_Doc.CreateAttribute("ElementType");
        //    XmlAttribute Left = m_Doc.CreateAttribute("Left");
        //    XmlAttribute Top = m_Doc.CreateAttribute("Top");
        //    XmlAttribute Width = m_Doc.CreateAttribute("Width");
        //    XmlAttribute Height = m_Doc.CreateAttribute("Height");
        //    XmlAttribute DrawMapNeatLine = m_Doc.CreateAttribute("DrawNeatLine");
        //    XmlAttribute MapNeatLineColor = m_Doc.CreateAttribute("NeatLineColor");
        //    XmlAttribute MapNeatLineSize = m_Doc.CreateAttribute("NeatLineSize");
        //    XmlAttribute GridLineColor = m_Doc.CreateAttribute("GridLineColor");
        //    XmlAttribute GridLineSize = m_Doc.CreateAttribute("GridLineSize");
        //    XmlAttribute GridLineStyle = m_Doc.CreateAttribute("GridLineStyle");
        //    XmlAttribute DrawGridLine = m_Doc.CreateAttribute("DrawGridLine");
        //    XmlAttribute DrawGridTickLine = m_Doc.CreateAttribute("DrawGridTickLine");
        //    XmlAttribute visible = m_Doc.CreateAttribute("Visible");
        //    XmlAttribute minLon = m_Doc.CreateAttribute("MinLon");
        //    XmlAttribute maxLon = m_Doc.CreateAttribute("MaxLon");
        //    XmlAttribute minLat = m_Doc.CreateAttribute("MinLat");
        //    XmlAttribute maxLat = m_Doc.CreateAttribute("MaxLat");

        //    elementType.InnerText = aIllustration.ElementType.ToString();
        //    Left.InnerText = aIllustration.Left.ToString();
        //    Top.InnerText = aIllustration.Top.ToString();
        //    Width.InnerText = aIllustration.Width.ToString();
        //    Height.InnerText = aIllustration.Height.ToString();
        //    DrawMapNeatLine.InnerText = aIllustration.DrawNeatLine.ToString();
        //    MapNeatLineColor.InnerText = ColorTranslator.ToHtml(aIllustration.NeatLineColor);
        //    MapNeatLineSize.InnerText = aIllustration.NeatLineSize.ToString();
        //    GridLineColor.InnerText = ColorTranslator.ToHtml(aIllustration.GridLineColor);
        //    GridLineSize.InnerText = aIllustration.GridLineSize.ToString();
        //    GridLineStyle.InnerText = Enum.GetName(typeof(DashStyle), aIllustration.GridLineStyle);
        //    DrawGridLine.InnerText = aIllustration.DrawGridLine.ToString();
        //    DrawGridTickLine.InnerText = aIllustration.DrawGridTickLine.ToString();
        //    visible.InnerText = aIllustration.Visible.ToString();
        //    minLon.InnerText = aIllustration.MinLon.ToString();
        //    maxLon.InnerText = aIllustration.MaxLon.ToString();
        //    minLat.InnerText = aIllustration.MinLat.ToString();
        //    maxLat.InnerText = aIllustration.MaxLat.ToString();

        //    LayoutMap.Attributes.Append(elementType);
        //    LayoutMap.Attributes.Append(Left);
        //    LayoutMap.Attributes.Append(Top);
        //    LayoutMap.Attributes.Append(Width);
        //    LayoutMap.Attributes.Append(Height);
        //    LayoutMap.Attributes.Append(DrawMapNeatLine);
        //    LayoutMap.Attributes.Append(MapNeatLineColor);
        //    LayoutMap.Attributes.Append(MapNeatLineSize);
        //    LayoutMap.Attributes.Append(GridLineColor);
        //    LayoutMap.Attributes.Append(GridLineSize);
        //    LayoutMap.Attributes.Append(GridLineStyle);
        //    LayoutMap.Attributes.Append(DrawGridLine);
        //    LayoutMap.Attributes.Append(DrawGridTickLine);
        //    LayoutMap.Attributes.Append(visible);
        //    LayoutMap.Attributes.Append(minLon);
        //    LayoutMap.Attributes.Append(maxLon);
        //    LayoutMap.Attributes.Append(minLat);
        //    LayoutMap.Attributes.Append(maxLat);

        //    parent.AppendChild(LayoutMap);
        //}

        private void AddLayoutLegendElement(ref XmlDocument doc, XmlElement parent, LayoutLegend aLegend)
        {
            XmlElement Legend = doc.CreateElement("LayoutLegend");
            XmlAttribute elementType = doc.CreateAttribute("ElementType");
            XmlAttribute layoutMapIndex = doc.CreateAttribute("LayoutMapIndex");
            XmlAttribute legendLayer = doc.CreateAttribute("LegendLayer");
            XmlAttribute LegendStyle = doc.CreateAttribute("LegendStyle");            
            XmlAttribute layerUpdateType = doc.CreateAttribute("LayerUpdateType");
            XmlAttribute BackColor = doc.CreateAttribute("BackColor");
            XmlAttribute DrawNeatLine = doc.CreateAttribute("DrawNeatLine");
            XmlAttribute NeatLineColor = doc.CreateAttribute("NeatLineColor");
            XmlAttribute NeatLineSize = doc.CreateAttribute("NeatLineSize");
            XmlAttribute Left = doc.CreateAttribute("Left");
            XmlAttribute Top = doc.CreateAttribute("Top");
            XmlAttribute Width = doc.CreateAttribute("Width");
            XmlAttribute Height = doc.CreateAttribute("Height");
            XmlAttribute FontName = doc.CreateAttribute("FontName");
            XmlAttribute FontSize = doc.CreateAttribute("FontSize");
            XmlAttribute colNum = doc.CreateAttribute("ColumnNumber");

            elementType.InnerText = aLegend.ElementType.ToString();
            layoutMapIndex.InnerText = GetLayoutMapIndex(aLegend.LayoutMap).ToString();
            legendLayer.InnerText = aLegend.LegendLayer.LayerName;
            LegendStyle.InnerText = Enum.GetName(typeof(LegendStyles), aLegend.LegendStyle);            
            layerUpdateType.InnerText = aLegend.LayerUpdateType.ToString();
            BackColor.InnerText = ColorTranslator.ToHtml(aLegend.BackColor);
            DrawNeatLine.InnerText = aLegend.DrawNeatLine.ToString();
            NeatLineColor.InnerText = ColorTranslator.ToHtml(aLegend.NeatLineColor);
            NeatLineSize.InnerText = aLegend.NeatLineSize.ToString();
            Left.InnerText = aLegend.Left.ToString();
            Top.InnerText = aLegend.Top.ToString();
            Width.InnerText = aLegend.Width.ToString();
            Height.InnerText = aLegend.Height.ToString();
            FontName.InnerText = aLegend.Font.Name;
            FontSize.InnerText = aLegend.Font.Size.ToString();
            colNum.InnerText = aLegend.ColumnNumber.ToString();

            Legend.Attributes.Append(elementType);
            Legend.Attributes.Append(layoutMapIndex);
            Legend.Attributes.Append(legendLayer);
            Legend.Attributes.Append(LegendStyle);            
            Legend.Attributes.Append(layerUpdateType);
            Legend.Attributes.Append(BackColor);
            Legend.Attributes.Append(DrawNeatLine);
            Legend.Attributes.Append(NeatLineColor);
            Legend.Attributes.Append(NeatLineSize);
            Legend.Attributes.Append(Left);
            Legend.Attributes.Append(Top);
            Legend.Attributes.Append(Width);
            Legend.Attributes.Append(Height);
            Legend.Attributes.Append(FontName);
            Legend.Attributes.Append(FontSize);
            Legend.Attributes.Append(colNum);

            parent.AppendChild(Legend);
        }

        private void AddLayoutScaleBarElement(ref XmlDocument doc, XmlElement parent, LayoutScaleBar aScaleBar)
        {
            XmlElement Legend = doc.CreateElement("LayoutScaleBar");
            XmlAttribute elementType = doc.CreateAttribute("ElementType");
            XmlAttribute layoutMapIndex = doc.CreateAttribute("LayoutMapIndex");
            XmlAttribute scaleBarType = doc.CreateAttribute("ScaleBarType");
            XmlAttribute BackColor = doc.CreateAttribute("BackColor");
            XmlAttribute foreColor = doc.CreateAttribute("ForeColor");
            XmlAttribute DrawNeatLine = doc.CreateAttribute("DrawNeatLine");
            XmlAttribute NeatLineColor = doc.CreateAttribute("NeatLineColor");
            XmlAttribute NeatLineSize = doc.CreateAttribute("NeatLineSize");
            XmlAttribute Left = doc.CreateAttribute("Left");
            XmlAttribute Top = doc.CreateAttribute("Top");
            XmlAttribute Width = doc.CreateAttribute("Width");
            XmlAttribute Height = doc.CreateAttribute("Height");
            XmlAttribute FontName = doc.CreateAttribute("FontName");
            XmlAttribute FontSize = doc.CreateAttribute("FontSize");
            XmlAttribute drawScaleText = doc.CreateAttribute("DrawScaleText");

            elementType.InnerText = aScaleBar.ElementType.ToString();
            layoutMapIndex.InnerText = GetLayoutMapIndex(aScaleBar.LayoutMap).ToString();
            scaleBarType.InnerText = Enum.GetName(typeof(ScaleBarTypes), aScaleBar.ScaleBarType);
            BackColor.InnerText = ColorTranslator.ToHtml(aScaleBar.BackColor);
            foreColor.InnerText = ColorTranslator.ToHtml(aScaleBar.ForeColor);
            DrawNeatLine.InnerText = aScaleBar.DrawNeatLine.ToString();
            NeatLineColor.InnerText = ColorTranslator.ToHtml(aScaleBar.NeatLineColor);
            NeatLineSize.InnerText = aScaleBar.NeatLineSize.ToString();
            Left.InnerText = aScaleBar.Left.ToString();
            Top.InnerText = aScaleBar.Top.ToString();
            Width.InnerText = aScaleBar.Width.ToString();
            Height.InnerText = aScaleBar.Height.ToString();
            FontName.InnerText = aScaleBar.Font.Name;
            FontSize.InnerText = aScaleBar.Font.Size.ToString();
            drawScaleText.InnerText = aScaleBar.DrawScaleText.ToString();

            Legend.Attributes.Append(elementType);
            Legend.Attributes.Append(layoutMapIndex);
            Legend.Attributes.Append(scaleBarType);
            Legend.Attributes.Append(BackColor);
            Legend.Attributes.Append(foreColor);
            Legend.Attributes.Append(DrawNeatLine);
            Legend.Attributes.Append(NeatLineColor);
            Legend.Attributes.Append(NeatLineSize);
            Legend.Attributes.Append(Left);
            Legend.Attributes.Append(Top);
            Legend.Attributes.Append(Width);
            Legend.Attributes.Append(Height);
            Legend.Attributes.Append(FontName);
            Legend.Attributes.Append(FontSize);
            Legend.Attributes.Append(drawScaleText);

            parent.AppendChild(Legend);
        }

        private void AddLayoutNorthArrowElement(ref XmlDocument doc, XmlElement parent, LayoutNorthArrow aNorthArrow)
        {
            XmlElement Legend = doc.CreateElement("LayoutScaleBar");
            XmlAttribute elementType = doc.CreateAttribute("ElementType");
            XmlAttribute layoutMapIndex = doc.CreateAttribute("LayoutMapIndex");
            XmlAttribute BackColor = doc.CreateAttribute("BackColor");
            XmlAttribute foreColor = doc.CreateAttribute("ForeColor");
            XmlAttribute DrawNeatLine = doc.CreateAttribute("DrawNeatLine");
            XmlAttribute NeatLineColor = doc.CreateAttribute("NeatLineColor");
            XmlAttribute NeatLineSize = doc.CreateAttribute("NeatLineSize");
            XmlAttribute Left = doc.CreateAttribute("Left");
            XmlAttribute Top = doc.CreateAttribute("Top");
            XmlAttribute Width = doc.CreateAttribute("Width");
            XmlAttribute Height = doc.CreateAttribute("Height");
            XmlAttribute angle = doc.CreateAttribute("Angle");

            elementType.InnerText = aNorthArrow.ElementType.ToString();
            layoutMapIndex.InnerText = GetLayoutMapIndex(aNorthArrow.LayoutMap).ToString();
            BackColor.InnerText = ColorTranslator.ToHtml(aNorthArrow.BackColor);
            foreColor.InnerText = ColorTranslator.ToHtml(aNorthArrow.ForeColor);
            DrawNeatLine.InnerText = aNorthArrow.DrawNeatLine.ToString();
            NeatLineColor.InnerText = ColorTranslator.ToHtml(aNorthArrow.NeatLineColor);
            NeatLineSize.InnerText = aNorthArrow.NeatLineSize.ToString();
            Left.InnerText = aNorthArrow.Left.ToString();
            Top.InnerText = aNorthArrow.Top.ToString();
            Width.InnerText = aNorthArrow.Width.ToString();
            Height.InnerText = aNorthArrow.Height.ToString();
            angle.InnerText = aNorthArrow.Angle.ToString();

            Legend.Attributes.Append(elementType);
            Legend.Attributes.Append(layoutMapIndex);
            Legend.Attributes.Append(BackColor);
            Legend.Attributes.Append(foreColor);
            Legend.Attributes.Append(DrawNeatLine);
            Legend.Attributes.Append(NeatLineColor);
            Legend.Attributes.Append(NeatLineSize);
            Legend.Attributes.Append(Left);
            Legend.Attributes.Append(Top);
            Legend.Attributes.Append(Width);
            Legend.Attributes.Append(Height);
            Legend.Attributes.Append(angle);

            parent.AppendChild(Legend);
        }


        private void AddLayoutGraphicElement(ref XmlDocument doc, XmlElement parent, LayoutGraphic aLayoutGraphic)
        {
            XmlElement layoutGraphic = doc.CreateElement("LayoutGraphic");
            XmlAttribute elementType = doc.CreateAttribute("ElementType");
            XmlAttribute isTitle = doc.CreateAttribute("IsTitle");
            XmlAttribute left = doc.CreateAttribute("Left");
            XmlAttribute top = doc.CreateAttribute("Top");
            XmlAttribute width = doc.CreateAttribute("Width");
            XmlAttribute height = doc.CreateAttribute("Height");

            elementType.InnerText = aLayoutGraphic.ElementType.ToString();
            isTitle.InnerText = aLayoutGraphic.IsTitle.ToString();
            left.InnerText = aLayoutGraphic.Left.ToString();
            top.InnerText = aLayoutGraphic.Top.ToString();
            width.InnerText = aLayoutGraphic.Width.ToString();
            height.InnerText = aLayoutGraphic.Height.ToString();

            layoutGraphic.Attributes.Append(elementType);
            layoutGraphic.Attributes.Append(isTitle);
            layoutGraphic.Attributes.Append(left);
            layoutGraphic.Attributes.Append(top);
            layoutGraphic.Attributes.Append(width);
            layoutGraphic.Attributes.Append(height);

            //Add graphic
            Graphic aGraphic = aLayoutGraphic.Graphic;
            aGraphic.ExportToXML(ref doc, layoutGraphic);

            //Append in parent
            parent.AppendChild(layoutGraphic);
        }        

        /// <summary>
        /// Import project XML content
        /// </summary>
        /// <param name="parent">parent XML element</param>
        public void ImportProjectXML(XmlElement parent)
        {
            LoadLayout(parent);            
        }        

        private void LoadLayout(XmlElement parent)
        {
            XmlNode layout = parent.GetElementsByTagName("Layout")[0];
            try
            {
                _PageBackColor = ColorTranslator.FromHtml(layout.Attributes["BackColor"].InnerText);
                _PageForeColor = ColorTranslator.FromHtml(layout.Attributes["ForeColor"].InnerText);
                string smStr = layout.Attributes["SmoothingMode"].InnerText;
                if (smStr.ToLower() == "false")
                    this.SmoothingMode = SmoothingMode.Default;
                else if (smStr.ToLower() == "true")
                    this.SmoothingMode = SmoothingMode.AntiAlias;
                else
                    this.SmoothingMode = (SmoothingMode)Enum.Parse(typeof(SmoothingMode), smStr, true);                          
                _paperSize.PaperName = layout.Attributes["PaperSizeName"].InnerText;
                _paperSize.Width = int.Parse(layout.Attributes["PaperSizeWidth"].InnerText);
                _paperSize.Height = int.Parse(layout.Attributes["PaperSizeHeight"].InnerText);
                _isLandscape = bool.Parse(layout.Attributes["Landscape"].InnerText);                

                UpdatePageSet();

                LoadLayoutElements((XmlElement)layout);
            }
            catch
            {

            }
        }

        private void LoadLayoutElements(XmlElement parent)
        {
            //RemoveElements_NotMap();
            _layoutElements.Clear();
            _selectedElements.Clear();

            XmlNode layoutElements = parent.GetElementsByTagName("LayoutElements")[0];
            //Load layout maps
            int i = 0;
            foreach (XmlNode elementNode in layoutElements.ChildNodes)
            {
                ElementType aType = (ElementType)Enum.Parse(typeof(ElementType),
                    elementNode.Attributes["ElementType"].InnerText, true);
                switch (aType)
                {
                    case ElementType.LayoutMap:
                        MapFrame aMF = null;
                        if (_mapFrames.Count > i)
                            aMF = _mapFrames[i];
                        else
                            aMF = new MapFrame();

                        LayoutMap aLM = new LayoutMap(aMF);
                        LoadLayoutMapElement(elementNode, ref aLM);
                        AddElement(aLM);
                        i += 1;
                        break;
                }
            }

            //Load other elements
            foreach (XmlNode elementNode in layoutElements.ChildNodes)
            {
                ElementType aType = (ElementType)Enum.Parse(typeof(ElementType), 
                    elementNode.Attributes["ElementType"].InnerText, true);
                switch (aType)
                {
                    case ElementType.LayoutIllustration:
                        //LoadLayoutIllustrationElement(elementNode);
                        break;
                    case ElementType.LayoutLegend:
                        LayoutLegend aLL = LoadLayoutLegendElement(elementNode);
                        AddElement(aLL);
                        break;
                    case ElementType.LayoutGraphic:
                        LayoutGraphic aLG = LoadLayoutGraphicElement(elementNode);
                        if (aLG.Graphic.Shape.ShapeType == ShapeTypes.WindArraw)
                        {
                            ((WindArraw)aLG.Graphic.Shape).angle = 270;
                        }
                        
                        AddElement(aLG);
                        break; 
                    case ElementType.LayoutScaleBar:
                        LayoutScaleBar aLSB = LoadLayoutScaleBarElement(elementNode);
                        if (aLSB != null)
                            AddElement(aLSB);
                        break;
                    case ElementType.LayoutNorthArraw:
                        LayoutNorthArrow aLNA = LoadLayoutNorthArrowElement(elementNode);
                        if (aLNA != null)
                            AddElement(aLNA);
                        break;
                }                
            } 
        }

        private void LoadLayoutMapElement(XmlNode LayoutMap, ref LayoutMap aLM)
        {
            try
            {
                aLM.Left = int.Parse(LayoutMap.Attributes["Left"].InnerText);
                aLM.Top = int.Parse(LayoutMap.Attributes["Top"].InnerText);
                aLM.Width = int.Parse(LayoutMap.Attributes["Width"].InnerText);
                aLM.Height = int.Parse(LayoutMap.Attributes["Height"].InnerText);
                aLM.DrawNeatLine = bool.Parse(LayoutMap.Attributes["DrawNeatLine"].InnerText);
                aLM.NeatLineColor = ColorTranslator.FromHtml(LayoutMap.Attributes["NeatLineColor"].InnerText);
                aLM.NeatLineSize = int.Parse(LayoutMap.Attributes["NeatLineSize"].InnerText);
                aLM.GridLineColor = ColorTranslator.FromHtml(LayoutMap.Attributes["GridLineColor"].InnerText);
                aLM.GridLineSize = int.Parse(LayoutMap.Attributes["GridLineSize"].InnerText);
                aLM.GridLineStyle = (DashStyle)Enum.Parse(typeof(DashStyle),
                    LayoutMap.Attributes["GridLineStyle"].InnerText, true);
                aLM.DrawGridLine = bool.Parse(LayoutMap.Attributes["DrawGridLine"].InnerText);
                //aLM.DrawGridTickLine = bool.Parse(LayoutMap.Attributes["DrawGridTickLine"].InnerText);
                aLM.DrawGridLabel = bool.Parse(LayoutMap.Attributes["DrawGridLabel"].InnerText);
                string fontName = LayoutMap.Attributes["GridFontName"].InnerText;
                float fontSize = float.Parse(LayoutMap.Attributes["GridFontSize"].InnerText);
                aLM.GridFont = new Font(fontName, fontSize);
                aLM.GridXDelt = double.Parse(LayoutMap.Attributes["GridXDelt"].InnerText);
                aLM.GridYDelt = double.Parse(LayoutMap.Attributes["GridYDelt"].InnerText);
                aLM.GridXOrigin = double.Parse(LayoutMap.Attributes["GridXOrigin"].InnerText);
                aLM.GridYOrigin = double.Parse(LayoutMap.Attributes["GridYOrigin"].InnerText);
                aLM.GridLabelPosition = (GridLabelPosition)Enum.Parse(typeof(GridLabelPosition),
                    LayoutMap.Attributes["GridLabelPosition"].InnerText, true);
            }
            catch
            {

            }
        }

        //private void LoadLayoutIllustrationElement(XmlNode LayoutIllustration)
        //{
        //    try
        //    {
        //        _illusMap.Left = int.Parse(LayoutIllustration.Attributes["Left"].InnerText);
        //        _illusMap.Top = int.Parse(LayoutIllustration.Attributes["Top"].InnerText);
        //        _illusMap.Width = int.Parse(LayoutIllustration.Attributes["Width"].InnerText);
        //        _illusMap.Height = int.Parse(LayoutIllustration.Attributes["Height"].InnerText);
        //        _illusMap.DrawNeatLine = bool.Parse(LayoutIllustration.Attributes["DrawNeatLine"].InnerText);
        //        _illusMap.NeatLineColor = ColorTranslator.FromHtml(LayoutIllustration.Attributes["NeatLineColor"].InnerText);
        //        _illusMap.NeatLineSize = int.Parse(LayoutIllustration.Attributes["NeatLineSize"].InnerText);
        //        _illusMap.GridLineColor = ColorTranslator.FromHtml(LayoutIllustration.Attributes["GridLineColor"].InnerText);
        //        _illusMap.GridLineSize = int.Parse(LayoutIllustration.Attributes["GridLineSize"].InnerText);
        //        _illusMap.GridLineStyle = (DashStyle)Enum.Parse(typeof(DashStyle),
        //            LayoutIllustration.Attributes["GridLineStyle"].InnerText, true);
        //        _illusMap.DrawGridLine = bool.Parse(LayoutIllustration.Attributes["DrawGridLine"].InnerText);
        //        _illusMap.DrawGridTickLine = bool.Parse(LayoutIllustration.Attributes["DrawGridTickLine"].InnerText);
        //        _illusMap.Visible = bool.Parse(LayoutIllustration.Attributes["Visible"].InnerText);
        //        _illusMap.MinLon = double.Parse(LayoutIllustration.Attributes["MinLon"].InnerText);
        //        _illusMap.MaxLon = double.Parse(LayoutIllustration.Attributes["MaxLon"].InnerText);
        //        _illusMap.MinLat = double.Parse(LayoutIllustration.Attributes["MinLat"].InnerText);
        //        _illusMap.MaxLat = double.Parse(LayoutIllustration.Attributes["MaxLat"].InnerText);
        //    }
        //    catch
        //    {

        //    }
        //}

        private LayoutLegend LoadLayoutLegendElement(XmlNode layoutLegend)
        {
            LayoutLegend aLL = null;
            try
            {
                int layoutMapIdx = int.Parse(layoutLegend.Attributes["LayoutMapIndex"].InnerText);
                string legendLayerName = layoutLegend.Attributes["LegendLayer"].InnerText;
                LayoutMap aLM = LayoutMaps[layoutMapIdx];
                MapLayer legendLayer = aLM.MapFrame.MapView.GetLayerFromName(legendLayerName);
                aLL = new LayoutLegend(this, aLM);
                aLL.LegendLayer = legendLayer;
            }
            catch 
            {
                aLL = new LayoutLegend(this, LayoutMaps[0]);
                aLL.LegendLayer = aLL.LayoutMap.MapFrame.MapView.LayerSet.Layers[0];
            }

            try
            {
                aLL.LegendStyle = (LegendStyles)Enum.Parse(typeof(LegendStyles), 
                    layoutLegend.Attributes["LegendStyle"].InnerText, true);               
                aLL.BackColor = ColorTranslator.FromHtml(layoutLegend.Attributes["BackColor"].InnerText);
                aLL.DrawNeatLine = bool.Parse(layoutLegend.Attributes["DrawNeatLine"].InnerText);
                aLL.NeatLineColor = ColorTranslator.FromHtml(layoutLegend.Attributes["NeatLineColor"].InnerText);
                aLL.NeatLineSize = (int)(float.Parse(layoutLegend.Attributes["NeatLineSize"].InnerText));
                aLL.Left = int.Parse(layoutLegend.Attributes["Left"].InnerText);
                aLL.Top = int.Parse(layoutLegend.Attributes["Top"].InnerText);
                aLL.Width = int.Parse(layoutLegend.Attributes["Width"].InnerText);
                aLL.Height = int.Parse(layoutLegend.Attributes["Height"].InnerText);
                string fontName = layoutLegend.Attributes["FontName"].InnerText;
                float fontSize = float.Parse(layoutLegend.Attributes["FontSize"].InnerText);
                aLL.Font = new Font(fontName, fontSize);
                aLL.LayerUpdateType = (LayerUpdateTypes)Enum.Parse(typeof(LayerUpdateTypes),
                    layoutLegend.Attributes["LayerUpdateType"].InnerText, true);
                aLL.ColumnNumber = int.Parse(layoutLegend.Attributes["ColumnNumber"].InnerText);
            }
            catch
            {
                
            }

            return aLL;
        }

        private LayoutScaleBar LoadLayoutScaleBarElement(XmlNode layoutScaleBar)
        {
            LayoutScaleBar aLSB = null;
            try
            {
                int layoutMapIdx = int.Parse(layoutScaleBar.Attributes["LayoutMapIndex"].InnerText);                
                LayoutMap aLM = LayoutMaps[layoutMapIdx];
                aLSB = new LayoutScaleBar(aLM);
            }
            catch
            {
                aLSB = new LayoutScaleBar(LayoutMaps[0]);                
            }

            try
            {
                aLSB.ScaleBarType = (ScaleBarTypes)Enum.Parse(typeof(ScaleBarTypes),
                    layoutScaleBar.Attributes["ScaleBarType"].InnerText, true);
                aLSB.BackColor = ColorTranslator.FromHtml(layoutScaleBar.Attributes["BackColor"].InnerText);
                aLSB.ForeColor = ColorTranslator.FromHtml(layoutScaleBar.Attributes["ForeColor"].InnerText);
                aLSB.DrawNeatLine = bool.Parse(layoutScaleBar.Attributes["DrawNeatLine"].InnerText);
                aLSB.NeatLineColor = ColorTranslator.FromHtml(layoutScaleBar.Attributes["NeatLineColor"].InnerText);
                aLSB.NeatLineSize = int.Parse(layoutScaleBar.Attributes["NeatLineSize"].InnerText);
                aLSB.Left = int.Parse(layoutScaleBar.Attributes["Left"].InnerText);
                aLSB.Top = int.Parse(layoutScaleBar.Attributes["Top"].InnerText);
                aLSB.Width = int.Parse(layoutScaleBar.Attributes["Width"].InnerText);
                aLSB.Height = int.Parse(layoutScaleBar.Attributes["Height"].InnerText);
                string fontName = layoutScaleBar.Attributes["FontName"].InnerText;
                float fontSize = float.Parse(layoutScaleBar.Attributes["FontSize"].InnerText);
                aLSB.Font = new Font(fontName, fontSize);
                aLSB.DrawScaleText = bool.Parse(layoutScaleBar.Attributes["DrawScaleText"].InnerText);
            }
            catch
            {

            }

            return aLSB;
        }

        private LayoutNorthArrow LoadLayoutNorthArrowElement(XmlNode layoutNorthArrow)
        {
            LayoutNorthArrow aLNA = null;
            try
            {
                int layoutMapIdx = int.Parse(layoutNorthArrow.Attributes["LayoutMapIndex"].InnerText);
                LayoutMap aLM = LayoutMaps[layoutMapIdx];
                aLNA = new LayoutNorthArrow(aLM);
            }
            catch
            {
                aLNA = new LayoutNorthArrow(LayoutMaps[0]);
            }

            try
            {
                aLNA.BackColor = ColorTranslator.FromHtml(layoutNorthArrow.Attributes["BackColor"].InnerText);
                aLNA.ForeColor = ColorTranslator.FromHtml(layoutNorthArrow.Attributes["ForeColor"].InnerText);
                aLNA.DrawNeatLine = bool.Parse(layoutNorthArrow.Attributes["DrawNeatLine"].InnerText);
                aLNA.NeatLineColor = ColorTranslator.FromHtml(layoutNorthArrow.Attributes["NeatLineColor"].InnerText);
                aLNA.NeatLineSize = int.Parse(layoutNorthArrow.Attributes["NeatLineSize"].InnerText);
                aLNA.Left = int.Parse(layoutNorthArrow.Attributes["Left"].InnerText);
                aLNA.Top = int.Parse(layoutNorthArrow.Attributes["Top"].InnerText);
                aLNA.Width = int.Parse(layoutNorthArrow.Attributes["Width"].InnerText);
                aLNA.Height = int.Parse(layoutNorthArrow.Attributes["Height"].InnerText);
                aLNA.Angle = float.Parse(layoutNorthArrow.Attributes["Angle"].InnerText);
            }
            catch
            {

            }

            return aLNA;
        }

        private LayoutGraphic LoadLayoutGraphicElement(XmlNode layoutGraphic)
        {
            Graphic aGraphic = new Graphic();
            XmlNode graphicNode = layoutGraphic.ChildNodes[0];
            aGraphic.ImportFromXML(graphicNode);

            LayoutGraphic aLG;
            if (aGraphic.Shape.ShapeType == ShapeTypes.WindArraw)
                aLG = new LayoutGraphic(aGraphic, this, this.ActiveLayoutMap);
            else
                aLG = new LayoutGraphic(aGraphic, this);
            //aLG.Graphic = aGraphic;

            aLG.IsTitle = bool.Parse(layoutGraphic.Attributes["IsTitle"].InnerText);
            //aLG.Left = int.Parse(layoutGraphic.Attributes["Left"].InnerText);
            //aLG.Top = int.Parse(layoutGraphic.Attributes["Top"].InnerText);
            //aLG.Width = int.Parse(layoutGraphic.Attributes["Width"].InnerText);
            //aLG.Height = int.Parse(layoutGraphic.Attributes["Height"].InnerText);
                                     
            return aLG;
        }
        
        #endregion

        #region Events
        /// <summary>
        /// OnPaint
        /// </summary>
        /// <param name="e">PaintEventArgs</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            
            //g.DrawImageUnscaled(_layoutBitmap, (int)_pageLocation.X, (int)_pageLocation.Y);
            g.DrawImageUnscaled(_layoutBitmap, 0, 0);
            //Graphics g = e.Graphics;
            //PaintGraphics(g);

            ////Draws the selection rectangle around each selected item
            //Pen selectionPen = new Pen(Color.Red, 1F);
            //selectionPen.DashPattern = new[] { 2.0F, 1.0F };
            //selectionPen.DashCap = DashCap.Round;
            //foreach (Control aCtl in m_SelectedControls)
            //{
            //    Rectangle aRect = aCtl.Bounds;
            //    g.DrawRectangle(selectionPen, aRect.X - 1, aRect.Y - 1, aRect.Width + 1, aRect.Height + 1);
            //    // Draw the reversible frame.
            //    //MyDrawReversibleRectangle(aRect);

            //}
        }

        // Convert and normalize the points and draw the reversible frame.
        private void MyDrawReversibleRectangle(Rectangle aRect)
        {
            Rectangle rc = new Rectangle();

            // Convert the points to screen coordinates.
            Point p1 = aRect.Location;
            Point p2 = new Point(aRect.Right, aRect.Bottom);
            p1 = PointToScreen(p1);
            p2 = PointToScreen(p2);
            // Normalize the rectangle.
            if (p1.X < p2.X)
            {
                rc.X = p1.X;
                rc.Width = p2.X - p1.X;
            }
            else
            {
                rc.X = p2.X;
                rc.Width = p1.X - p2.X;
            }
            if (p1.Y < p2.Y)
            {
                rc.Y = p1.Y;
                rc.Height = p2.Y - p1.Y;
            }
            else
            {
                rc.Y = p2.Y;
                rc.Height = p1.Y - p2.Y;
            }
            // Draw the reversible frame.
            ControlPaint.DrawReversibleFrame(rc,
                            Color.Red, FrameStyle.Dashed);
        }

        ///// <summary>
        ///// Paint graphics
        ///// </summary>
        ///// <param name="g">graphics</param>
        //public void PaintGraphics_Old(Graphics g)
        //{
        //    g.SmoothingMode = _smoothingMode;

        //    //Draw grid labels
        //    SizeF aSF = new SizeF();
        //    SolidBrush aBrush = new SolidBrush(_PageForeColor);
        //    string drawStr;
        //    PointF sP = new PointF(0, 0);
        //    for (int i = 0; i < _mapView.XGridPosLabel.Count; i++)
        //    {
        //        sP = (PointF)_mapView.XGridPosLabel[i][0];
        //        sP.X = sP.X + _mapView.Bounds.Left;
        //        sP.Y = sP.Y + _mapView.Bounds.Top;
        //        drawStr = (string)_mapView.XGridPosLabel[i][1];
        //        aSF = g.MeasureString(drawStr, _GridFont);
        //        g.DrawString(drawStr, _GridFont, aBrush, sP.X - aSF.Width / 2, sP.Y + 2);
        //    }

        //    for (int i = 0; i < _mapView.YGridPosLabel.Count; i++)
        //    {
        //        sP = (PointF)_mapView.YGridPosLabel[i][0];
        //        sP.X = sP.X + _mapView.Bounds.Left;
        //        sP.Y = sP.Y + _mapView.Bounds.Top;
        //        drawStr = (string)_mapView.YGridPosLabel[i][1];
        //        aSF = g.MeasureString(drawStr, _GridFont);
        //        g.DrawString(drawStr, _GridFont, aBrush, sP.X - aSF.Width - 5, sP.Y - aSF.Height / 2);
        //    }

        //    //ControlPaint.DrawReversibleLine(new Point(0, 0), new Point(this.Right, this.Bottom), Color.Red);
        //}

        /// <summary>
        /// Paint graphics
        /// </summary>        
        public void PaintGraphics()
        {
            if ((this.ClientRectangle.Width == 0) || (this.ClientRectangle.Height == 0))
                return;

            if ((this._pageBounds.Width == 0) || (this._pageBounds.Height == 0))
                return;

            GC.Collect();
            _layoutBitmap = new Bitmap(this.ClientRectangle.Width,
                    this.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
            //_layoutBitmap = new Bitmap(_pageBounds.Width, _pageBounds.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(_layoutBitmap);            

            //Draw bound rectangle
            RectangleF aRect = new RectangleF();
            aRect = PageToScreen(_pageBounds.X, _pageBounds.Y, _pageBounds.Width, _pageBounds.Height);
            //aRect.X = _pageBounds.X - 2;
            //aRect.Y = _pageBounds.Y - 2;
            //aRect.Width = _pageBounds.Width + 3;
            //aRect.Height = _pageBounds.Height + 3;
            g.FillRectangle(new SolidBrush(_PageBackColor), aRect);
            //g.FillRectangle(new SolidBrush(_PageBackColor), _pageBounds);
            //g.DrawRectangle(new Pen(Color.Black), aRect.X, aRect.Y, aRect.Width, aRect.Height);

            //Judge if show scroll bar
            if (aRect.Height > this.Height)
            {
                _vScrollBar.Minimum = 0;
                _vScrollBar.SmallChange = 5;
                _vScrollBar.LargeChange = this.ClientRectangle.Height;
                _vScrollBar.Maximum = (int)aRect.Height + 100;

                if (_vScrollBar.Visible == false)
                {
                    _vScrollBar.Value = 0;
                    _vScrollBar.Visible = true;
                }
            }
            else
            {
                if (aRect.Y >= 0)
                    _vScrollBar.Visible = false;
            }

            if (aRect.Width > this.Width)
            {
                _hScrollBar.Minimum = 0;
                _hScrollBar.SmallChange = 5;
                _hScrollBar.LargeChange = this.ClientRectangle.Width;
                _hScrollBar.Maximum = (int)aRect.Width + 100;

                if (_hScrollBar.Visible == false)
                {
                    _hScrollBar.Value = 0;
                    _hScrollBar.Visible = true;
                }
            }
            else
            {
                if (aRect.X >= 0)
                    _hScrollBar.Visible = false;
            }


            //Draw layout elements
            PaintGraphicsOnLayout(g);

            g.Dispose();
            this.Refresh ();
        }

        /// <summary>
        /// Paint graphics
        /// </summary>        
        public void PaintGraphicsWithoutBound()
        {
            if ((this.ClientRectangle.Width == 0) || (this.ClientRectangle.Height == 0))
                return;

            _layoutBitmap = new Bitmap(this.ClientRectangle.Width,
                    this.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
            Graphics g = Graphics.FromImage(_layoutBitmap);

            //Draw bound rectangle
            Rectangle aRect = new Rectangle();
            aRect.X = _pageBounds.X - 2;
            aRect.Y = _pageBounds.Y - 2;
            aRect.Width = _pageBounds.Width + 3;
            aRect.Height = _pageBounds.Height + 3;
            g.FillRectangle(new SolidBrush(_PageBackColor), aRect);

            //Draw layout elements
            PaintGraphics(g);

            g.Dispose();
            this.Refresh();
        }

        /// <summary>
        /// Paint graphics
        /// </summary>
        /// <param name="g">graphics</param>
        public void PaintGraphicsOnLayout(Graphics g)
        {
            g.SmoothingMode = _smoothingMode;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            foreach (LayoutElement aElement in _layoutElements)
            {
                if (!aElement.Visible)
                    continue;

                aElement.PaintOnLayout(g, _pageLocation, _zoom);

                ////Draws the selection rectangle around each selected item            
                //if (aElement.Selected)
                //{
                //    if (_mouseMode == MouseMode.EditVertices)
                //    {
                //        LayoutGraphic aLG = (LayoutGraphic)aElement;
                //        List<PointD> points = aLG.Graphic.Shape.GetPoints();
                //        DrawSelectedVertices(g, points);
                //    }
                //    else
                //    {
                //        Pen selectionPen = new Pen(Color.Cyan, 1F);
                //        selectionPen.DashPattern = new[] { 2.0F, 1.0F };
                //        selectionPen.DashCap = DashCap.Round;
                //        Rectangle aRect = PageToScreen(aElement.Bounds);
                //        g.DrawRectangle(selectionPen, aRect);

                //        switch (aElement.ResizeAbility)
                //        {
                //            case ResizeAbility.SameWidthHeight:
                //                DrawSelectedConers(g, aElement);
                //                break;
                //            case ResizeAbility.ResizeAll:
                //                DrawSelectedConers(g, aElement);
                //                DrawSelectedEdgeCenters(g, aElement);
                //                break;
                //        }
                //    }
                //}
            }

            //Draws the selection rectangle around each selected item 
            foreach (LayoutElement aElement in _layoutElements)
            {
                if (!aElement.Visible)
                    continue;
            
                if (aElement.Selected)
                {
                    if (_mouseMode == MouseMode.EditVertices)
                    {
                        LayoutGraphic aLG = (LayoutGraphic)aElement;
                        List<PointD> points = aLG.Graphic.Shape.GetPoints();
                        DrawSelectedVertices(g, points);
                    }
                    else
                    {
                        Pen selectionPen = new Pen(Color.Cyan, 1F);
                        selectionPen.DashPattern = new[] { 2.0F, 1.0F };
                        selectionPen.DashCap = DashCap.Round;
                        Rectangle aRect = PageToScreen(aElement.Bounds);
                        g.DrawRectangle(selectionPen, aRect);

                        switch (aElement.ResizeAbility)
                        {
                            case ResizeAbility.SameWidthHeight:
                                DrawSelectedConers(g, aElement);
                                break;
                            case ResizeAbility.ResizeAll:
                                DrawSelectedConers(g, aElement);
                                DrawSelectedEdgeCenters(g, aElement);
                                break;
                        }
                    }
                }
            }

            ////Draw grid labels
            //SizeF aSF = new SizeF();
            //SolidBrush aBrush = new SolidBrush(_PageForeColor);
            //string drawStr;
            //PointF sP = new PointF(0, 0);
            //Font font = new Font(_GridFont.Name, _GridFont.Size * _zoom, _GridFont.Style);
            //for (int i = 0; i < _mapView.XGridPosLabel.Count; i++)
            //{
            //    sP = (PointF)_mapView.XGridPosLabel[i][0];                
            //    sP.X = sP.X + _defaultLayoutMap.Left * _zoom + _pageLocation.X;
            //    sP.Y = sP.Y + _defaultLayoutMap.Top * _zoom + _pageLocation.Y;
            //    //sP = PageToScreen(sP.X, sP.Y);
            //    drawStr = (string)_mapView.XGridPosLabel[i][1];
            //    aSF = g.MeasureString(drawStr, font);
            //    g.DrawString(drawStr, font, aBrush, sP.X - aSF.Width / 2, sP.Y + 2);
            //}

            //for (int i = 0; i < _mapView.YGridPosLabel.Count; i++)
            //{
            //    sP = (PointF)_mapView.YGridPosLabel[i][0];
            //    sP.X = sP.X + _defaultLayoutMap.Left * _zoom + _pageLocation.X;
            //    sP.Y = sP.Y + _defaultLayoutMap.Top * _zoom + _pageLocation.Y;
            //    //sP = PageToScreen(sP.X, sP.Y);
            //    drawStr = (string)_mapView.YGridPosLabel[i][1];
            //    aSF = g.MeasureString(drawStr, font);
            //    g.DrawString(drawStr, font, aBrush, sP.X - aSF.Width - 5, sP.Y - aSF.Height / 2);
            //}
        }

        /// <summary>
        /// Paint graphics
        /// </summary>
        /// <param name="g">graphics</param>
        public void PaintGraphics(Graphics g)
        {            
            g.SmoothingMode = _smoothingMode;

            foreach (LayoutElement aElement in _layoutElements)
            {
                if (!aElement.Visible)
                    continue;

                //aElement.Paint(g);
                aElement.PaintOnLayout(g, new PointF(0, 0), 1);

                ////Draws the selection rectangle around each selected item            
                //if (aElement.Selected)
                //{
                //    if (_mouseMode == MouseMode.EditVertices)
                //    {
                //        LayoutGraphic aLG = (LayoutGraphic)aElement;
                //        List<PointD> points = aLG.Graphic.Shape.GetPoints();
                //        DrawSelectedVertices(g, points);
                //    }
                //    else
                //    {
                //        Pen selectionPen = new Pen(Color.Cyan, 1F);
                //        selectionPen.DashPattern = new[] { 2.0F, 1.0F };
                //        selectionPen.DashCap = DashCap.Round;
                //        Rectangle aRect = aElement.Bounds;
                //        g.DrawRectangle(selectionPen, aElement.Left, aElement.Top, aElement.Width, aElement.Height);

                //        switch (aElement.ResizeAbility)
                //        {
                //            case ResizeAbility.SameWidthHeight:
                //                DrawSelectedConers(g, aElement);
                //                break;
                //            case ResizeAbility.ResizeAll:
                //                DrawSelectedConers(g, aElement);
                //                DrawSelectedEdgeCenters(g, aElement);
                //                break;
                //        }
                //    }                                    
                //}
            }

            ////Draw grid labels
            //SizeF aSF = new SizeF();
            //SolidBrush aBrush = new SolidBrush(_PageForeColor);
            //string drawStr;
            //PointF sP = new PointF(0, 0);
            //for (int i = 0; i < _mapView.XGridPosLabel.Count; i++)
            //{
            //    sP = (PointF)_mapView.XGridPosLabel[i][0];
            //    sP.X = sP.X + _defaultLayoutMap.Left;
            //    sP.Y = sP.Y + _defaultLayoutMap.Top;
            //    drawStr = (string)_mapView.XGridPosLabel[i][1];
            //    aSF = g.MeasureString(drawStr, _GridFont);
            //    g.DrawString(drawStr, _GridFont, aBrush, sP.X - aSF.Width / 2, sP.Y + 2);
            //}

            //for (int i = 0; i < _mapView.YGridPosLabel.Count; i++)
            //{
            //    sP = (PointF)_mapView.YGridPosLabel[i][0];
            //    sP.X = sP.X + _defaultLayoutMap.Left;
            //    sP.Y = sP.Y + _defaultLayoutMap.Top;
            //    drawStr = (string)_mapView.YGridPosLabel[i][1];
            //    aSF = g.MeasureString(drawStr, _GridFont);
            //    g.DrawString(drawStr, _GridFont, aBrush, sP.X - aSF.Width - 5, sP.Y - aSF.Height / 2);
            //}                   
        }

        private void DrawSelectedConers(Graphics g, LayoutElement aElement)
        {
            Rectangle elementRect = PageToScreen(aElement.Bounds);
            int size = 6;
            SolidBrush sBrush = new SolidBrush(Color.Cyan);
            Pen aPen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(elementRect.Left - size / 2, elementRect.Top - size / 2, size, size);
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
            rect.Y = elementRect.Bottom - size / 2;
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
            rect.X = elementRect.Right - size / 2;
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
            rect.Y = elementRect.Top - size / 2;
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
        }

        private void DrawSelectedEdgeCenters(Graphics g, LayoutElement aElement)
        {
            Rectangle elementRect = PageToScreen(aElement.Bounds);
            int size = 6;
            SolidBrush sBrush = new SolidBrush(Color.Cyan);
            Pen aPen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(elementRect.Left + elementRect.Width / 2 - size / 2, elementRect.Top - size / 2, size, size);            
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
            rect.Y = elementRect.Bottom - size / 2;
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
            rect.X = elementRect.Left - size / 2;
            rect.Y = elementRect.Top + elementRect.Height / 2 - size / 2;
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
            rect.X = elementRect.Right - size / 2;
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
        }

        private void DrawSelectedVertices(Graphics g, List<PointD> points)
        {
            int size = 6;
            SolidBrush sBrush = new SolidBrush(Color.Cyan);
            Pen aPen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(0, 0, size, size);

            foreach (PointD aPoint in points)
            {
                PointF aP = PageToScreen((float)aPoint.X, (float)aPoint.Y);
                rect.X = (int)aP.X - size / 2;
                rect.Y = (int)aP.Y - size / 2;
                g.FillRectangle(sBrush, rect);
                g.DrawRectangle(aPen, rect);
            }
        }

        /// <summary>
        /// Override OnPaintBackground
        /// </summary>
        /// <param name="e">PaintEventArgs</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            //Draw page
            //Graphics g = e.Graphics;
            //g.FillRectangle(new SolidBrush(Color.LightGray), this.Bounds);
            //g.FillRectangle(new SolidBrush(Color.White), _pageBounds);

            //Rectangle aRect = new Rectangle();
            //aRect.X = _pageBounds.X - 2;
            //aRect.Y = _pageBounds.Y - 2;
            //aRect.Width = _pageBounds.Width + 3;
            //aRect.Height = _pageBounds.Height + 3;           
            //g.FillRectangle(new SolidBrush(_PageBackColor), aRect);
            //g.DrawRectangle(new Pen(Color.Black), aRect);
        }

        ///// <summary>
        ///// CreateParams
        ///// </summary>
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        //return base.CreateParams;
        //        CreateParams cp = base.CreateParams;
        //        //cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
        //        //cp.Style &= (~WS_CLIPCHILDREN);
        //        return cp;
        //    }
        //}

        /// <summary>
        /// On resize
        /// </summary>
        /// <param name="e">eventArgs</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.Width > 0 && this.Height > 0)
            {
                _vScrollBar.Top = 0;
                _vScrollBar.Height = this.Height;
                _vScrollBar.Left = this.Width - _vScrollBar.Width;

                _hScrollBar.Left = 0;
                _hScrollBar.Width = this.Width;
                _hScrollBar.Top = this.Height - _hScrollBar.Height;
            }

            this.PaintGraphics();
            //this.Invalidate();
        }

        void MapViewUpdated(object sender, EventArgs e)
        {
            PaintGraphics();
        }

        //void MapViewViewExtentChanged(object sender, EventArgs e)
        //{
        //    if (_mapView.IsLayoutMap)
        //    {
        //        UpdateWindArrawLegend();
        //        PaintGraphics();
        //    }
        //}

        //void MapViewLayersUpdated(object sender, EventArgs e)
        //{
        //    if (_mapView.IsLayoutMap)
        //    {
        //        //Update legend
        //        UpdateLegendByLayers();
        //        UpdateWindArrawLegend();

        //        //Update illustration
        //        if (_illusMap.Visible)
        //        {
        //            UpdateIllustration(_mapView);
        //            //SetIllustrationViewExtent();
        //        }

        //        if (!_mapView.LockViewUpdate)
        //        {                    
        //            //_mapView.SendToBack();
        //            //_mapView.LockViewUpdate = false;
        //            this.PaintGraphics();
        //        }
        //    }
        //}

        //void MapViewRenderChanged(object sender, EventArgs e)
        //{
        //    //Update illustration
        //    if (_illusMap.Visible)
        //    {
        //        UpdateIllustration(_mapView);                
        //    }
        //}

        //void MapViewResize(object sender, EventArgs e)
        //{
        //    if (_mapView.IsLayoutMap)
        //    {
        //        this.Refresh();
        //    }
        //}

        //void MapViewRedrawed(object sender, EventArgs e)
        //{
        //    //UpdateIllustration(_mapView);
        //    //SetIllustrationViewExtent();
        //    //m_IllustrationMap.PaintLayers();
        //    if (_mapView.IsLayoutMap)
        //        this.PaintGraphics();
        //}       

        /// <summary>
        /// Override OnMouseClick event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            Point pageP = ScreenToPage(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                switch (_mouseMode)
                {
                    case MouseMode.Map_Identifer:
                        MapLayer aMLayer = _currentLayoutMap.MapFrame.MapView.GetLayerFromHandle(_currentLayoutMap.MapFrame.MapView.SelectedLayer);
                        if (aMLayer == null)
                            return;
                        if (aMLayer.LayerType == LayerTypes.ImageLayer)
                            return;

                        PointF mapP = PageToScreen(_currentLayoutMap.Left, _currentLayoutMap.Top);
                        PointF aPoint = new PointF(e.X - mapP.X, e.Y - mapP.Y);
                        //PointF aPoint = new PointF(e.X - _currentLayoutMap.Left, e.Y - _defaultLayoutMap.Top);
                        if (aMLayer.LayerType == LayerTypes.VectorLayer)
                        {
                            VectorLayer aLayer = (VectorLayer)aMLayer;
                            List<int> SelectedShapes = new List<int>();
                            if (_currentLayoutMap.MapFrame.MapView.SelectShapes(aLayer, aPoint, ref SelectedShapes))
                            {
                                _frmIdentifer.MapView = _currentLayoutMap.MapFrame.MapView;
                                if (!_frmIdentifer.Visible)
                                {
                                    _frmIdentifer = new frmIdentifer(_currentLayoutMap.MapFrame.MapView);
                                    _frmIdentifer.Show(this);
                                }

                                _frmIdentifer.ListView1.Items.Clear();
                                string fieldStr, valueStr;
                                int shapeIdx = SelectedShapes[0];
                                aLayer.IdentiferShape = shapeIdx;
                                _currentLayoutMap.MapFrame.MapView.DrawIdentiferShape = true;

                                fieldStr = "Index";
                                valueStr = shapeIdx.ToString();
                                _frmIdentifer.ListView1.Items.Add(fieldStr).SubItems.Add(valueStr);

                                if (aLayer.ShapeNum > 0)
                                {
                                    for (int i = 0; i < aLayer.NumFields; i++)
                                    {
                                        fieldStr = aLayer.GetFieldName(i);
                                        valueStr = aLayer.GetCellValue(i, shapeIdx).ToString();
                                        _frmIdentifer.ListView1.Items.Add(fieldStr).SubItems.Add(valueStr);
                                    }
                                }

                                this.Refresh();
                                Rectangle rect = GetElementViewExtent(_currentLayoutMap);
                                _currentLayoutMap.MapFrame.MapView.DrawIdShape(this.CreateGraphics(), aLayer.ShapeList[shapeIdx], rect);
                                //_currentLayoutMap.MapFrame.MapView.PaintLayers();
                            }
                        }
                        else if (aMLayer.LayerType == LayerTypes.RasterLayer)
                        {
                            RasterLayer aRLayer = (RasterLayer)aMLayer;
                            int iIdx = 0;
                            int jIdx = 0;
                            if (_currentLayoutMap.MapFrame.MapView.SelectGridCell(aRLayer, aPoint, ref iIdx, ref jIdx))
                            {
                                double aValue = aRLayer.GetCellValue(iIdx, jIdx);
                                if (!_frmIdentiferGrid.Visible)
                                {
                                    _frmIdentiferGrid = new frmIdentiferGrid();
                                    _frmIdentiferGrid.Show(this);
                                }

                                _frmIdentiferGrid.Lab_I.Text = "I = " + iIdx.ToString();
                                _frmIdentiferGrid.Lab_J.Text = "J = " + jIdx.ToString();
                                _frmIdentiferGrid.Lab_CellValue.Text = "Cell Value: " + aValue.ToString();
                            }
                        }
                        break;
                    case MouseMode.Map_SelectFeatures_Rectangle:
                        aMLayer = _currentLayoutMap.MapFrame.MapView.GetLayerFromHandle(_currentLayoutMap.MapFrame.MapView.SelectedLayer);
                        if (aMLayer == null)
                            return;

                        if (aMLayer.LayerType != LayerTypes.VectorLayer)
                            return;

                        if (!((Control.ModifierKeys & Keys.Control) == Keys.Control))
                        {
                            ((VectorLayer)aMLayer).ClearSelectedShapes();
                            _currentLayoutMap.MapFrame.MapView.PaintLayers();
                        }

                        mapP = PageToScreen(_currentLayoutMap.Left, _currentLayoutMap.Top);
                        aPoint = new PointF(e.X - mapP.X, e.Y - mapP.Y);
                        if (aMLayer.LayerType == LayerTypes.VectorLayer)
                        {
                            VectorLayer aLayer = (VectorLayer)aMLayer;
                            List<int> SelectedShapes = new List<int>();
                            if (_currentLayoutMap.MapFrame.MapView.SelectShapes(aLayer, aPoint, ref SelectedShapes, true))
                            {
                                int shapeIdx = SelectedShapes[0];
                                if (!((Control.ModifierKeys & Keys.Control) == Keys.Control))
                                    this.Refresh();
                                Rectangle rect = GetElementViewExtent(_currentLayoutMap);
                                _currentLayoutMap.MapFrame.MapView.DrawIdShape(this.CreateGraphics(), aLayer.ShapeList[shapeIdx], rect);
                                //_currentLayoutMap.MapFrame.MapView.PaintLayers();
                            }
                        }
                        break;
                }
            }
            else if (e.Button == MouseButtons.Right && _mouseMode == MouseMode.Select)
            {
                if (_selectedElements.Count == 0)
                    return;

                ContextMenuStrip mnuElement = new ContextMenuStrip();
                mnuElement.Items.Add("Remove");
                mnuElement.Items[mnuElement.Items.Count - 1].Click += new EventHandler(OnRemoveElementClick);
                mnuElement.Items.Add(new ToolStripSeparator());
                ToolStripMenuItem orderItem = new ToolStripMenuItem("Order");
                orderItem.DropDownItems.Add("Bring to Front");
                orderItem.DropDownItems[orderItem.DropDownItems.Count - 1].Click += new EventHandler(OnBringToFrontClick);
                orderItem.DropDownItems.Add("Send to Back");
                orderItem.DropDownItems[orderItem.DropDownItems.Count - 1].Click += new EventHandler(OnSendToBackClick);
                orderItem.DropDownItems.Add("Bring Forward");
                orderItem.DropDownItems[orderItem.DropDownItems.Count - 1].Click += new EventHandler(OnBringForwardClick);
                orderItem.DropDownItems.Add("Send Backward");
                orderItem.DropDownItems[orderItem.DropDownItems.Count - 1].Click += new EventHandler(OnSendBackwardClick);
                mnuElement.Items.Add(orderItem);
                
                switch (_mouseMode)
                {
                    case MouseMode.Select:
                    case MouseMode.MoveSelection:
                    case MouseMode.ResizeSelected:                        
                        if (_selectedElements.Count > 0)
                        {
                            LayoutElement aElement = _selectedElements[0];
                            if (MIMath.PointInRectangle(pageP, aElement.Bounds))
                            {
                                if (aElement.ElementType == ElementType.LayoutGraphic)
                                {
                                    Graphic aGraphic = ((LayoutGraphic)aElement).Graphic;
                                    if (aGraphic.Legend.BreakType == BreakTypes.PolylineBreak || aGraphic.Legend.BreakType == BreakTypes.PolygonBreak)
                                    {
                                        mnuElement.Items.Add(new ToolStripSeparator());
                                        mnuElement.Items.Add("Reverse");
                                        mnuElement.Items[mnuElement.Items.Count - 1].Click += new EventHandler(OnReverseGraphicClick);
                                        if (aGraphic.Shape.ShapeType == ShapeTypes.Polyline || aGraphic.Shape.ShapeType == ShapeTypes.Polygon)
                                        {                                            
                                            mnuElement.Items.Add("Smooth Graphic");
                                            mnuElement.Items[mnuElement.Items.Count - 1].Click += new EventHandler(OnGrahpicSmoothClick);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }

                Point aPoint = new Point(0, 0);
                aPoint.X = e.X;
                aPoint.Y = e.Y;
                mnuElement.Show(this, aPoint);
            }
        }

        private void OnRemoveElementClick(object sender, EventArgs e)
        {
            for (int i = 0; i < _layoutElements.Count; i++)
            {
                LayoutElement aElement = _layoutElements[i];
                if (aElement.Selected)
                {
                    RemoveElement(aElement);
                    _selectedElements.Remove(aElement);
                }
            }
            //_selectedElements.Clear();

            _startNewGraphic = true;
            PaintGraphics();
        }

        private void OnBringToFrontClick(object sender, EventArgs e)
        {
            LayoutElement aLE = _selectedElements[0];
            int idx = _layoutElements.IndexOf(aLE);
            if (idx < _layoutElements.Count - 1)
            {
                _layoutElements.Remove(aLE);
                _layoutElements.Add(aLE);
                this.PaintGraphics();
            }
        }

        private void OnSendToBackClick(object sender, EventArgs e)
        {
            LayoutElement aLE = _selectedElements[0];
            int idx = _layoutElements.IndexOf(aLE);
            if (idx > 0)
            {
                _layoutElements.Remove(aLE);
                _layoutElements.Insert(0, aLE);
                this.PaintGraphics();
            }
        }

        private void OnBringForwardClick(object sender, EventArgs e)
        {
            LayoutElement aLE = _selectedElements[0];
            int idx = _layoutElements.IndexOf(aLE);
            if (idx < _layoutElements.Count - 1)
            {
                _layoutElements.Remove(aLE);
                _layoutElements.Insert(idx + 1, aLE);
                this.PaintGraphics();
            }
        }

        private void OnSendBackwardClick(object sender, EventArgs e)
        {
            LayoutElement aLE = _selectedElements[0];
            int idx = _layoutElements.IndexOf(aLE);
            if (idx > 0)
            {
                _layoutElements.Remove(aLE);
                _layoutElements.Insert(idx - 1, aLE);
                this.PaintGraphics();
            }
        }

        private void OnReverseGraphicClick(object sender, EventArgs e)
        {
            LayoutElement aElement = _selectedElements[0];
            Graphic aGraphic = ((LayoutGraphic)aElement).Graphic;
            List<PointD> points = aGraphic.Shape.GetPoints();
            points.Reverse();
            aGraphic.Shape.SetPoints(points);
            //((LayoutGraphic)aElement).UpdateControlSize();
            this.PaintGraphics();
        }

        private void OnGrahpicSmoothClick(object sender, EventArgs e)
        {
            LayoutElement aElement = _selectedElements[0];
            Graphic aGraphic = ((LayoutGraphic)aElement).Graphic;
            List<wContour.PointD> pointList = new List<wContour.PointD>();
            List<PointD> newPoints = new List<PointD>();

            foreach (PointD aP in aGraphic.Shape.GetPoints())
                pointList.Add(new wContour.PointD(aP.X, aP.Y));

            if (aGraphic.Shape.ShapeType == ShapeTypes.Polygon)
                pointList.Add(pointList[0]);

            pointList = wContour.Contour.SmoothPoints(pointList);
            foreach (wContour.PointD aP in pointList)
            {
                newPoints.Add(new PointD(aP.X, aP.Y));
            }
            aGraphic.Shape.SetPoints(newPoints);
            ((LayoutGraphic)aElement).UpdateControlSize();
            this.PaintGraphics();
        }        

        /// <summary>
        /// Override OnMouseDown event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                int left = e.X;
                int top = e.Y;
                Point pageP = ScreenToPage(e.X, e.Y);
                Graphics g = this.CreateGraphics();
                LayoutMap aLM = GetLayoutMap(pageP);
                if (aLM != null)
                    _currentLayoutMap = aLM;

                switch (_mouseMode)
                {
                    case MouseMode.Map_Pan:
                        Rectangle mapRect = PageToScreen(_currentLayoutMap.Bounds);
                        _tempMapBitmap = new Bitmap(mapRect.Width - 2,
                            mapRect.Height - 2, PixelFormat.Format32bppPArgb);
                        Graphics tg = Graphics.FromImage(_tempMapBitmap);
                        
                        //tg.Clip = new Region(mapRect);
                        //Fill backgroud                        
                        tg.FillRectangle(new SolidBrush(_currentLayoutMap.MapFrame.MapView.BackColor), mapRect);

                        tg.DrawImageUnscaled(_layoutBitmap, -mapRect.Left - 1, -mapRect.Top - 1);

                        tg.Dispose();
                        break;
                    case MouseMode.Select:
                        //Point mousePoint = new Point(e.X, e.Y);                                                
                        List<LayoutElement> tempGraphics = SelectElements(pageP, _selectedElements, 3);
                        if (tempGraphics.Count > 0)
                        {                            
                            _selectedRectangle = _selectedElements[0].Bounds;
                            _selectedRectangle = PageToScreen(_selectedRectangle);
                            if (_resizeSelectedEdge == Edge.None)
                                _mouseMode = MouseMode.MoveSelection;
                            else
                                _mouseMode = MouseMode.ResizeSelected;
                        }
                        else
                        {                            
                            _mouseMode = MouseMode.CreateSelection;
                        }
                        
                        break;
                    case MouseMode.New_Point:
                        PointShape aPS = new PointShape();
                        aPS.Point = new MeteoInfoC.PointD(pageP.X, pageP.Y);
                        //PointBreak aPB = new PointBreak();
                        //aPB.Size = 10;
                        Graphic aGraphic = new Graphic(aPS, (PointBreak)_defPointBreak.Clone());
                        LayoutGraphic aLayoutGraphic = new LayoutGraphic(aGraphic, this);
                        //aLayoutGraphic.Left = pageP.X;
                        //aLayoutGraphic.Top = pageP.Y;
                        AddElement(aLayoutGraphic);
                        this.PaintGraphics();
                        break;
                    case MouseMode.New_Label:
                        //LabelBreak aLB = new LabelBreak();
                        //aLB.Text = "Text";
                        //aLB.Font = new Font("Arial", 12);
                        aPS = new PointShape();
                        aPS.Point = new MeteoInfoC.PointD(pageP.X, pageP.Y);
                        aGraphic = new Graphic(aPS, (LabelBreak)_defLabelBreak.Clone());
                        aLayoutGraphic = new LayoutGraphic(aGraphic, this);
                        //aLayoutGraphic.Left = pageP.X;
                        //aLayoutGraphic.Top = pageP.Y;
                        AddElement(aLayoutGraphic);
                        this.PaintGraphics();
                        break;
                    case MouseMode.New_Polyline:
                    case MouseMode.New_Polygon:
                    case MouseMode.New_Curve:
                    case MouseMode.New_CurvePolygon:
                    case MouseMode.New_Freehand:
                    case MouseMode.Map_SelectFeatures_Polygon:
                    case MouseMode.Map_SelectFeatures_Lasso:
                        if (_startNewGraphic)
                        {
                            _graphicPoints = new List<PointF>();
                            _startNewGraphic = false;
                        }
                        _graphicPoints.Add(new PointF(e.X, e.Y));
                        break;
                    case MouseMode.EditVertices:
                        if (_selectedElements.Count > 0)
                        {                            
                            if (SelectEditVertices(pageP, ((LayoutGraphic)_selectedElements[0]).Graphic.Shape,
                                ref _editingVertices, ref _editingVerticeIndex))
                            {
                                _mouseMode = MouseMode.InEditingVertices;                                
                            }
                        }
                        break;
                    case MouseMode.Map_Measurement:
                        if (_frmMeasure.IsHandleCreated)
                        {
                            switch (_frmMeasure.MeasureType)
                            {
                                case MeasureType.Length:
                                case MeasureType.Area:
                                    if (_startNewGraphic)
                                    {
                                        _graphicPoints = new List<PointF>();
                                        _startNewGraphic = false;
                                    }
                                    _frmMeasure.PreviousValue = _frmMeasure.TotalValue;
                                    _graphicPoints.Add(new PointF(e.X, e.Y));
                                    break;
                                case MeasureType.Feature:
                                    MapLayer aMLayer = _currentLayoutMap.MapFrame.MapView.GetLayerFromHandle(_currentLayoutMap.MapFrame.MapView.SelectedLayer);
                                    if (aMLayer != null)
                                    {
                                        if (aMLayer.LayerType == LayerTypes.VectorLayer)
                                        {
                                            VectorLayer aLayer = (VectorLayer)aMLayer;
                                            if (aLayer.ShapeType != ShapeTypes.Point)
                                            {
                                                List<int> SelectedShapes = new List<int>();
                                                PointF mapP = PageToScreen(_currentLayoutMap.Left, _currentLayoutMap.Top);
                                                PointF aPoint = new PointF(e.X - mapP.X, e.Y - mapP.Y);
                                                if (_currentLayoutMap.MapFrame.MapView.SelectShapes(aLayer, aPoint, ref SelectedShapes))
                                                {
                                                    int shapeIdx = SelectedShapes[0];
                                                    Shape.Shape aShape = aLayer.ShapeList[shapeIdx];
                                                    this.Refresh();
                                                    Rectangle rect = GetElementViewExtent(_currentLayoutMap);
                                                    _currentLayoutMap.MapFrame.MapView.DrawIdShape(this.CreateGraphics(), aLayer.ShapeList[shapeIdx], rect);
                                                    double value = 0.0;
                                                    switch (aShape.ShapeType)
                                                    {
                                                        case ShapeTypes.Polyline:
                                                        case ShapeTypes.PolylineZ:
                                                            _frmMeasure.IsArea = false;
                                                            if (_currentLayoutMap.MapFrame.MapView.Projection.IsLonLatMap)
                                                                value = GeoComputation.GetDistance(((PolylineShape)aShape).Points, true);
                                                            else
                                                            {
                                                                value = ((PolylineShape)aShape).Length;
                                                                value *= _currentLayoutMap.MapFrame.MapView.Projection.ProjInfo.Unit.Meters;
                                                            }
                                                            break;
                                                        case ShapeTypes.Polygon:
                                                            _frmMeasure.IsArea = true;
                                                            if (_currentLayoutMap.MapFrame.MapView.Projection.IsLonLatMap)
                                                            {
                                                                //ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
                                                                //string toProjStr = "+proj=aea +lat_1=25 +lat_2=47 +lat_0=40 +lon_0=105";
                                                                //ProjectionInfo toProj = new ProjectionInfo(toProjStr);
                                                                //PolygonShape aPGS = (PolygonShape)((PolygonShape)aShape).Clone();
                                                                //aPGS = _currentLayoutMap.MapFrame.MapView.Projection.ProjectPolygonShape(aPGS, fromProj, toProj);
                                                                //value = aPGS.Area;
                                                                value = ((PolygonShape)aShape).SphericalArea;
                                                            }
                                                            else
                                                                value = ((PolygonShape)aShape).Area;
                                                            value *= _currentLayoutMap.MapFrame.MapView.Projection.ProjInfo.Unit.Meters * _currentLayoutMap.MapFrame.MapView.Projection.ProjInfo.Unit.Meters;
                                                            break;
                                                    }
                                                    _frmMeasure.CurrentValue = value;
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                switch (_mouseMode)
                {
                    case MouseMode.Map_Measurement:
                        if (_frmMeasure.IsHandleCreated)
                        {
                            switch (_frmMeasure.MeasureType)
                            {
                                case MeasureType.Length:
                                case MeasureType.Area:
                                    _startNewGraphic = true;
                                    _frmMeasure.TotalValue = 0;
                                    break;
                            }
                        }
                        break;
                }
            }

            _mouseDownPoint.X = e.X;
            _mouseDownPoint.Y = e.Y;
            _mouseLastPos = _mouseDownPoint;
        }

        /// <summary>
        /// Override OnMouseMove event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int deltaX = e.X - _mouseLastPos.X;
            int deltaY = e.Y - _mouseLastPos.Y;
            //deltaX = (int)(deltaX / _zoom);
            //deltaY = (int)(deltaY / _zoom);
            _mouseLastPos.X = e.X;
            _mouseLastPos.Y = e.Y;

            Point pageP = ScreenToPage(e.X, e.Y);

            Graphics g = this.CreateGraphics();
            Pen aPen = new Pen(Color.Red);
            aPen.DashStyle = DashStyle.Dash;
            Rectangle rect = new Rectangle();
            _vScrollBar.Cursor = Cursors.Default;
            _hScrollBar.Cursor = Cursors.Default;
            this.Cursor = Cursors.Default;            
            
            switch (_mouseMode)
            {
                case MouseMode.Map_ZoomIn:
                    if (IsInLayoutMaps(pageP))
                    {
                        this.Cursor = new Cursor(this.GetType().Assembly.
                            GetManifestResourceStream("MeteoInfoC.Resources.zoom_in.cur"));

                        if (e.Button == MouseButtons.Left)
                        {
                            rect.X = Math.Min(e.X, _mouseDownPoint.X);
                            rect.Y = Math.Min(e.Y, _mouseDownPoint.Y);
                            rect.Width = Math.Abs(e.X - _mouseDownPoint.X);
                            rect.Height = Math.Abs(e.Y - _mouseDownPoint.Y);
                            this.Refresh();
                            aPen.Color = this.ForeColor;
                            aPen.DashStyle = DashStyle.Dot;
                            g.DrawRectangle(aPen, rect);
                        }
                    }
                    break;
                case MouseMode.Map_ZoomOut:
                    if (IsInLayoutMaps(pageP))
                    {
                        this.Cursor = new Cursor(this.GetType().Assembly.
                            GetManifestResourceStream("MeteoInfoC.Resources.zoom_out.cur"));
                    }
                    break;
                case MouseMode.Map_Pan:
                    if (IsInLayoutMaps(pageP))
                    {
                        this.Cursor = new Cursor(this.GetType().Assembly.
                            GetManifestResourceStream("MeteoInfoC.Resources.Pan_Open.cur"));

                        if (e.Button == MouseButtons.Left)
                        {
                            Rectangle mapRect = PageToScreen(_currentLayoutMap.Bounds); 
                            g.Clip = new Region(mapRect);
                            //g.Clear(_mapView.BackColor);
                            Color aColor = _currentLayoutMap.MapFrame.BackColor;
                            if (aColor == Color.Transparent)
                                aColor = Color.White;
                            SolidBrush aBrush = new SolidBrush(aColor);
                            //g.FillRectangle(aBrush, mapRect);
                            int aX = e.X - _mouseDownPoint.X;
                            int aY = e.Y - _mouseDownPoint.Y;
                            aX = (int)(aX / _zoom);
                            aY = (int)(aY / _zoom);           
                            if (aX > 0)
                            {
                                if (mapRect.X >= 0)
                                    g.FillRectangle(aBrush, mapRect.X, mapRect.Y, aX, mapRect.Height);
                                else
                                    g.FillRectangle(aBrush, 0, mapRect.Y, aX, mapRect.Height);
                            }
                            else
                            {
                                if (mapRect.X <= this.Width)
                                    g.FillRectangle(aBrush, mapRect.Right + aX, mapRect.Y, Math.Abs(aX), mapRect.Height);
                                else
                                    g.FillRectangle(aBrush, this.Width + aX, mapRect.Y, Math.Abs(aX), mapRect.Height);
                            }
                            if (aY > 0)
                            {
                                if (mapRect.Y >= 0)
                                    g.FillRectangle(aBrush, mapRect.X, mapRect.Y, mapRect.Width, aY);
                                else
                                    g.FillRectangle(aBrush, mapRect.X, 0, mapRect.Width, aY);
                            }
                            else
                            {
                                if (mapRect.Bottom <= this.Bottom)
                                    g.FillRectangle(aBrush, mapRect.X, mapRect.Bottom + aY, mapRect.Width, Math.Abs(aY));
                                else
                                    g.FillRectangle(aBrush, mapRect.X, this.Bottom + aY, mapRect.Width, Math.Abs(aY));
                            }
                            int startX = mapRect.X + aX;
                            int startY = mapRect.Y + aY;
                            g.DrawImageUnscaled(_tempMapBitmap, startX, startY);
                            g.DrawRectangle(new Pen(this.ForeColor, 2), mapRect);
                        }
                    }
                    break;
                case MouseMode.Map_Identifer:
                    if (IsInLayoutMaps(pageP))
                    {
                        this.Cursor = new Cursor(this.GetType().Assembly.
                            GetManifestResourceStream("MeteoInfoC.Resources.cursor.cur"));
                    }
                    break;
                case MouseMode.Map_SelectFeatures_Rectangle:
                    if (IsInLayoutMaps(pageP))
                    {
                        this.Cursor = Cursors.Cross;
                        if (e.Button == MouseButtons.Left)
                        {
                            this.Refresh();
                            int sx = Math.Min(_mouseDownPoint.X, e.X);
                            int sy = Math.Min(_mouseDownPoint.Y, e.Y);
                            g.DrawRectangle(new Pen(ForeColor),
                                new Rectangle(new Point(sx, sy), new Size(Math.Abs(e.X - _mouseDownPoint.X),
                                Math.Abs(e.Y - _mouseDownPoint.Y))));
                        }
                    }
                    break;
                case MouseMode.Select:
                    if (_selectedElements.Count > 0)
                    {
                        List<LayoutElement> tempElements = SelectElements(pageP, _selectedElements, 3);
                        if (tempElements.Count > 0)
                        {
                            //Change mouse cursor    
                            Rectangle aRect = _selectedElements[0].Bounds;
                            //_resizeSelectedEdge = IntersectElementEdge(aRect, new PointF(e.X, e.Y), 3F);
                            _resizeSelectedEdge = IntersectElementEdge(aRect, pageP, 3F);
                            switch (_selectedElements[0].ResizeAbility)
                            {
                                case ResizeAbility.SameWidthHeight:
                                    switch (_resizeSelectedEdge)
                                    {
                                        case Edge.TopLeft:
                                        case Edge.BottomRight:
                                            this.Cursor = Cursors.SizeNWSE;
                                            break;                                        
                                        case Edge.TopRight:
                                        case Edge.BottomLeft:
                                            this.Cursor = Cursors.SizeNESW;
                                            break;                                        
                                        default:
                                            this.Cursor = Cursors.SizeAll;
                                            break;
                                    }
                                    break;
                                case ResizeAbility.ResizeAll:                                    
                                    switch (_resizeSelectedEdge)
                                    {
                                        case Edge.TopLeft:
                                        case Edge.BottomRight:
                                            this.Cursor = Cursors.SizeNWSE;
                                            break;
                                        case Edge.Top:
                                        case Edge.Bottom:
                                            this.Cursor = Cursors.SizeNS;
                                            break;
                                        case Edge.TopRight:
                                        case Edge.BottomLeft:
                                            this.Cursor = Cursors.SizeNESW;
                                            break;
                                        case Edge.Left:
                                        case Edge.Right:
                                            this.Cursor = Cursors.SizeWE;
                                            break;
                                        case Edge.None:
                                            this.Cursor = Cursors.SizeAll;
                                            break;
                                    }
                                    break;
                                default:
                                    this.Cursor = Cursors.SizeAll;
                                    break;
                            }                            
                        }
                        else
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }
                    else
                        this.Cursor = Cursors.Default;

                    break;
                case MouseMode.MoveSelection:
                    //Move selected graphics
                    if (e.Button == MouseButtons.Left)
                    {
                        this.Cursor = Cursors.SizeAll;
                        this.Refresh();
                        
                        rect.X = _selectedRectangle.X + e.X - _mouseDownPoint.X;
                        rect.Y = _selectedRectangle.Y + e.Y - _mouseDownPoint.Y;
                        rect.Width = _selectedRectangle.Width;
                        rect.Height = _selectedRectangle.Height;
                        
                        g.DrawRectangle(aPen, rect);
                    }
                    break;
                case MouseMode.ResizeSelected:
                    LayoutElement oElement = _selectedElements[0];
                    //_selectedRectangle = PageToScreen(oElement.Bounds);
                    if (_selectedRectangle.Width > 2 && _selectedRectangle.Height > 2)
                    {
                        switch (oElement.ResizeAbility)
                        {
                            case ResizeAbility.SameWidthHeight:
                                //deltaY = deltaX;
                                switch (_resizeSelectedEdge)
                                {
                                    case Edge.TopLeft:
                                        _selectedRectangle.X += deltaX;
                                        _selectedRectangle.Y += deltaX;
                                        _selectedRectangle.Width -= deltaX;
                                        _selectedRectangle.Height -= deltaX;
                                        break;
                                    case Edge.BottomRight:
                                        _selectedRectangle.Width += deltaX;
                                        _selectedRectangle.Height += deltaX;
                                        break;                                    
                                    case Edge.TopRight:
                                        _selectedRectangle.Y += deltaY;
                                        _selectedRectangle.Width -= deltaY;
                                        _selectedRectangle.Height -= deltaY;
                                        break;
                                    case Edge.BottomLeft:
                                        _selectedRectangle.X += deltaX;
                                        _selectedRectangle.Width -= deltaX;
                                        _selectedRectangle.Height -= deltaX;
                                        break;                                    
                                }
                                break;
                            case ResizeAbility.ResizeAll:
                                switch (_resizeSelectedEdge)
                                {
                                    case Edge.TopLeft:
                                        _selectedRectangle.X += deltaX;
                                        _selectedRectangle.Y += deltaY;
                                        _selectedRectangle.Width -= deltaX;
                                        _selectedRectangle.Height -= deltaY;
                                        break;
                                    case Edge.BottomRight:
                                        _selectedRectangle.Width += deltaX;
                                        _selectedRectangle.Height += deltaY;
                                        break;
                                    case Edge.Top:
                                        _selectedRectangle.Y += deltaY;
                                        _selectedRectangle.Height -= deltaY;
                                        break;
                                    case Edge.Bottom:
                                        _selectedRectangle.Height += deltaY;
                                        break;
                                    case Edge.TopRight:
                                        _selectedRectangle.Y += deltaY;
                                        _selectedRectangle.Width += deltaX;
                                        _selectedRectangle.Height -= deltaY;
                                        break;
                                    case Edge.BottomLeft:
                                        _selectedRectangle.X += deltaX;
                                        _selectedRectangle.Width -= deltaX;
                                        _selectedRectangle.Height += deltaY;
                                        break;
                                    case Edge.Left:
                                        _selectedRectangle.X += deltaX;
                                        _selectedRectangle.Width -= deltaX;
                                        break;
                                    case Edge.Right:
                                        _selectedRectangle.Width += deltaX;
                                        break;
                                }
                                break;
                        }

                        PointF minP = ScreenToPage(_selectedRectangle.X, _selectedRectangle.Y);
                        PointF maxP = ScreenToPage(_selectedRectangle.Right, _selectedRectangle.Bottom);
                        oElement.Left = (int)minP.X;
                        oElement.Top = (int)minP.Y;
                        oElement.Width = (int)(maxP.X - minP.X);
                        oElement.Height = (int)(maxP.Y - minP.Y);
                    }
                    else
                    {
                        oElement.Width = 3;
                        oElement.Height = 3;
                    }
                    
                    //this.PaintGraphics();
                    this.Refresh();
                    g.DrawRectangle(aPen, _selectedRectangle);
                    break;
                case MouseMode.New_Polyline:
                case MouseMode.New_Polygon:
                case MouseMode.New_Curve:
                case MouseMode.New_CurvePolygon:
                case MouseMode.New_Freehand:
                case MouseMode.Map_SelectFeatures_Polygon:
                case MouseMode.Map_SelectFeatures_Lasso:
                    if (!_startNewGraphic)
                    {
                        this.Refresh();
                        PointF[] points = _graphicPoints.ToArray();
                        Array.Resize(ref points, _graphicPoints.Count + 1);
                        points[_graphicPoints.Count] = new PointF(e.X, e.Y);

                        switch (_mouseMode)
                        {
                            case MouseMode.New_Polyline:
                                g.DrawLines(new Pen(Color.Black), points);
                                break;
                            case MouseMode.New_Polygon:
                            case MouseMode.Map_SelectFeatures_Polygon:
                                g.DrawPolygon(new Pen(Color.Black), points);
                                break;
                            case MouseMode.New_Curve:
                                g.DrawCurve(new Pen(ForeColor), points);
                                break;
                            case MouseMode.New_CurvePolygon:
                                g.DrawCurve(new Pen(ForeColor), points);
                                break;
                            case MouseMode.New_Freehand:                            
                                _graphicPoints.Add(new PointF(e.X, e.Y));
                                g.DrawLines(new Pen(ForeColor), points);
                                break;
                            case MouseMode.Map_SelectFeatures_Lasso:
                                _graphicPoints.Add(new PointF(e.X, e.Y));
                                g.DrawPolygon(new Pen(Color.Black), points);
                                break;
                        }
                    }
                    break;
                case MouseMode.New_Rectangle:
                case MouseMode.New_Ellipse:
                    if (e.Button == MouseButtons.Left)
                    {
                        this.Refresh();
                        g.DrawRectangle(new Pen(Color.Black),
                            new Rectangle(_mouseDownPoint, new Size(Math.Abs(e.X - _mouseDownPoint.X),
                            Math.Abs(e.Y - _mouseDownPoint.Y))));
                    }
                    break;
                case MouseMode.New_Circle:
                case MouseMode.Map_SelectFeatures_Circle:
                    if (e.Button == MouseButtons.Left)
                    {
                        this.Refresh();
                        int radius = (int)Math.Sqrt(Math.Pow(e.X - _mouseDownPoint.X, 2) + Math.Pow(e.Y - _mouseDownPoint.Y, 2));
                        g.DrawLine(new Pen(ForeColor), _mouseDownPoint.X, _mouseDownPoint.Y, e.X, e.Y);
                        g.DrawEllipse(new Pen(ForeColor), _mouseDownPoint.X - radius, _mouseDownPoint.Y - radius,
                            radius * 2, radius * 2);
                    }
                    break;
                case MouseMode.EditVertices:
                    if (_selectedElements.Count > 0)
                    {                        
                        if (SelectEditVertices(pageP, ((LayoutGraphic)_selectedElements[0]).Graphic.Shape,
                            ref _editingVertices, ref _editingVerticeIndex))
                        {
                            this.Cursor = new Cursor(this.GetType().Assembly.
                                GetManifestResourceStream("MeteoInfoC.Resources.MoveVertex.ico"));
                        }
                    }
                    break;
                case MouseMode.InEditingVertices:
                    this.Refresh();
                    PointF p1 = PageToScreen((float)_editingVertices[1].X, (float)_editingVertices[1].Y);
                    PointF p2 = PageToScreen((float)_editingVertices[2].X, (float)_editingVertices[2].Y);
                    g.DrawLine(new Pen(Color.Black), (int)p1.X, (int)p1.Y, e.X, e.Y);
                    if (_editingVertices.Count == 3)
                        g.DrawLine(new Pen(Color.Black), (int)p2.X, (int)p2.Y, e.X, e.Y);

                    rect = new Rectangle(e.X - 3, e.Y - 3, 6, 6);
                    g.FillRectangle(new SolidBrush(Color.Cyan), rect);
                    g.DrawRectangle(new Pen(Color.Black), rect);
                    break;
                case MouseMode.Map_Measurement:
                    if (IsInLayoutMaps(pageP))
                        this.Cursor = Cursors.Cross;

                    if (_frmMeasure.IsHandleCreated)
                    {
                        switch (_frmMeasure.MeasureType)
                        {
                            case MeasureType.Length:
                            case MeasureType.Area:
                                if (!_startNewGraphic)
                                {
                                    //Draw graphic                                    
                                    g.SmoothingMode = SmoothingMode.AntiAlias;
                                    this.Refresh();
                                    PointF[] points = _graphicPoints.ToArray();
                                    Array.Resize(ref points, _graphicPoints.Count + 1);
                                    points[_graphicPoints.Count] = new PointF(e.X, e.Y);

                                    if (_frmMeasure.MeasureType == MeasureType.Length)
                                        g.DrawLines(new Pen(Color.Red, 2), points);
                                    else
                                    {
                                        Color aColor = Color.FromArgb(100, Color.Blue);
                                        g.FillPolygon(new SolidBrush(aColor), points);
                                        g.DrawPolygon(new Pen(Color.Red, 2), points);
                                    }

                                    g.Dispose();

                                    //Calculate             
                                    PointF mapP = PageToScreen(_currentLayoutMap.Left, _currentLayoutMap.Top);
                                    PointF aPoint = new PointF(e.X - mapP.X, e.Y - mapP.Y);
                                    double ProjX = 0, ProjY = 0;
                                    _currentLayoutMap.MapFrame.MapView.ScreenToProj(ref ProjX, ref ProjY, aPoint.X, aPoint.Y);
                                    if (_frmMeasure.MeasureType == MeasureType.Length)
                                    {
                                        double pProjX = 0, pProjY = 0;
                                        aPoint = new PointF(_mouseDownPoint.X - mapP.X, _mouseDownPoint.Y - mapP.Y);
                                        _currentLayoutMap.MapFrame.MapView.ScreenToProj(ref pProjX, ref pProjY, aPoint.X, aPoint.Y);
                                        double dx = Math.Abs(ProjX - pProjX);
                                        double dy = Math.Abs(ProjY - pProjY);
                                        double dist;
                                        if (_currentLayoutMap.MapFrame.MapView.Projection.IsLonLatMap)
                                        {
                                            double y = (ProjY + pProjY) / 2;
                                            double factor = Math.Cos(y * Math.PI / 180);
                                            dx *= factor;
                                            dist = Math.Sqrt(dx * dx + dy * dy);
                                            dist = dist * 111319.5;
                                        }
                                        else
                                        {
                                            dist = Math.Sqrt(dx * dx + dy * dy);
                                            dist *= _currentLayoutMap.MapFrame.MapView.Projection.ProjInfo.Unit.Meters;
                                        }

                                        _frmMeasure.CurrentValue = dist;
                                    }
                                    else
                                    {
                                        List<PointD> mPoints = new List<PointD>();
                                        for (int i = 0; i < points.Length; i++)
                                        {
                                            aPoint = new PointF(points[i].X - mapP.X, points[i].Y - mapP.Y);
                                            _currentLayoutMap.MapFrame.MapView.ScreenToProj(ref ProjX, ref ProjY,
                                                aPoint.X, aPoint.Y);
                                            mPoints.Add(new PointD(ProjX, ProjY));
                                        }
                                        double area = GeoComputation.GetArea(mPoints);
                                        if (_currentLayoutMap.MapFrame.MapView.Projection.IsLonLatMap)
                                        {
                                            area = area * 111319.5 * 111319.5;
                                        }
                                        else
                                        {
                                            area *= _currentLayoutMap.MapFrame.MapView.Projection.ProjInfo.Unit.Meters * _currentLayoutMap.MapFrame.MapView.Projection.ProjInfo.Unit.Meters;
                                        }
                                        _frmMeasure.CurrentValue = area;
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Override OnMouseUp event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            double MinX, MaxX, MinY, MaxY, ZoomF;
            Point pageP = ScreenToPage(e.X, e.Y);
            switch (_mouseMode)
            {
                case MouseMode.Map_ZoomIn:                    
                    MinX = Math.Min(e.X, _mouseDownPoint.X);
                    MinY = Math.Min(e.Y, _mouseDownPoint.Y);
                    MaxX = Math.Max(e.X, _mouseDownPoint.X);
                    MaxY = Math.Max(e.Y, _mouseDownPoint.Y);
                    if (MaxX - MinX < 5)
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            ZoomF = 0.75;
                        }
                        else
                        {
                            ZoomF = 1.5;
                        }
                        MinX = pageP.X - (_currentLayoutMap.Width / 2 * ZoomF);
                        MaxX = pageP.X + (_currentLayoutMap.Width / 2 * ZoomF);
                        MinY = pageP.Y - (_currentLayoutMap.Height / 2 * ZoomF);
                        MaxY = pageP.Y + (_currentLayoutMap.Height / 2 * ZoomF);
                    }
                    else
                    {
                        PointF minP = ScreenToPage((float)MinX, (float)MinY);
                        PointF maxP = ScreenToPage((float)MaxX, (float)MaxY);
                        MinX = minP.X;
                        MinY = minP.Y;
                        MaxX = maxP.X;
                        MaxY = maxP.Y;
                    }

                    MinX -= _currentLayoutMap.Left;
                    MinY -= _currentLayoutMap.Top;
                    MaxX -= _currentLayoutMap.Left;
                    MaxY -= _currentLayoutMap.Top;
                    if (MaxX - MinX > 0.001)
                        _currentLayoutMap.MapFrame.MapView.ZoomToExtentScreen(MinX, MaxX, MinY, MaxY, _zoom);
                    break;
                case MouseMode.Map_ZoomOut:
                    if (e.Button == MouseButtons.Left)
                    {
                        ZoomF = 1.5;
                    }
                    else
                    {
                        ZoomF = 0.75;
                    }
                    MinX = pageP.X - (_currentLayoutMap.Width / 2 * ZoomF);
                    MaxX = pageP.X + (_currentLayoutMap.Width / 2 * ZoomF);
                    MinY = pageP.Y - (_currentLayoutMap.Height / 2 * ZoomF);
                    MaxY = pageP.Y + (_currentLayoutMap.Height / 2 * ZoomF);

                    MinX -= _currentLayoutMap.Left;
                    MinY -= _currentLayoutMap.Top;
                    MaxX -= _currentLayoutMap.Left;
                    MaxY -= _currentLayoutMap.Top;
                    if (MaxX - MinX > 0.001)
                        _currentLayoutMap.MapFrame.MapView.ZoomToExtentScreen(MinX, MaxX, MinY, MaxY, _zoom);
                    break;         
                case MouseMode.Map_Pan:
                    if (e.Button == MouseButtons.Left)
                    {
                        int deltaX = e.X - _mouseDownPoint.X;
                        int deltaY = e.Y - _mouseDownPoint.Y;
                        deltaX = (int)(deltaX / _zoom);
                        deltaY = (int)(deltaY / _zoom);
                        MinX = - deltaX;
                        MinY = - deltaY;
                        MaxX = _currentLayoutMap.Width - deltaX;
                        MaxY = _currentLayoutMap.Height - deltaY;
                        _currentLayoutMap.MapFrame.MapView.ZoomToExtentScreen(MinX, MaxX, MinY, MaxY, _zoom);
                    }
                    break;
            }

            if (e.Button == MouseButtons.Left)
            {
                switch (_mouseMode)
                {
                    case MouseMode.Map_SelectFeatures_Rectangle:
                        if (e.Button == MouseButtons.Left)
                        {
                            if (_currentLayoutMap.MapFrame.MapView.SelectedLayer < 0)
                                return;
                            MapLayer aMLayer = _currentLayoutMap.MapFrame.MapView.GetLayerFromHandle(_currentLayoutMap.MapFrame.MapView.SelectedLayer);
                            if (aMLayer == null)
                                return;
                            if (aMLayer.LayerType != LayerTypes.VectorLayer)
                                return;

                            VectorLayer aLayer = (VectorLayer)aMLayer;
                            PointF mapP = PageToScreen(_currentLayoutMap.Left, _currentLayoutMap.Top);
                            Point aPoint = new Point(e.X - (int)mapP.X, e.Y - (int)mapP.Y);
                            Point bPoint = new Point(_mouseDownPoint.X - (int)mapP.X, _mouseDownPoint.Y - (int)mapP.Y);
                            int minx = Math.Min(bPoint.X, aPoint.X);
                            int miny = Math.Min(bPoint.Y, aPoint.Y);
                            int width = Math.Abs(aPoint.X - bPoint.X);
                            int height = Math.Abs(aPoint.Y - bPoint.Y);
                            Rectangle rect = new Rectangle(minx, miny, width, height);
                            List<int> selectedShapes = _currentLayoutMap.MapFrame.MapView.SelectShapes(aLayer, rect);
                            if (selectedShapes.Count > 0)
                            {
                                if (!((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                                {
                                    aLayer.ClearSelectedShapes();
                                }
                                this.Refresh();
                                rect = GetElementViewExtent(_currentLayoutMap);
                                foreach (int shapeIdx in selectedShapes)
                                {
                                    aLayer.ShapeList[shapeIdx].Selected = true;                                    
                                    _currentLayoutMap.MapFrame.MapView.DrawIdShape(this.CreateGraphics(), aLayer.ShapeList[shapeIdx], rect);
                                    //_currentLayoutMap.MapFrame.MapView.DrawIdShape(this.CreateGraphics(), aLayer.ShapeList[shapeIdx]);
                                }

                                _currentLayoutMap.MapFrame.MapView.RaiseShapeSelectedEvent();
                            }
                        }
                        break;
                    case MouseMode.CreateSelection:
                        //Remove selected graphics
                        foreach (LayoutElement aElement in _selectedElements)
                        {
                            aElement.Selected = false;
                        }
                        _selectedElements.Clear();

                        //Select elements
                        if (Math.Abs(e.X - _mouseDownPoint.X) > 2 || Math.Abs(e.Y - _mouseDownPoint.Y) > 2)
                        {
                            this.PaintGraphics();
                            return;
                        }

                        //Point mousePoint = new Point(_mouseDownPoint.X, _mouseDownPoint.Y);
                        _selectedElements = SelectElements(pageP, _layoutElements, 0);
                        if (_selectedElements.Count > 0)
                        {
                            for (int i = 0; i < _selectedElements.Count - 1; i++)
                            {
                                _selectedElements.RemoveAt(_selectedElements.Count - 1);
                            }
                            _selectedElements[0].Selected = true;
                            if (_selectedElements[0].ElementType == ElementType.LayoutMap)
                                SetActiveMapFrame(((LayoutMap)_selectedElements[0]).MapFrame);

                            OnElementSeleted();
                        }

                        _mouseMode = MouseMode.Select;
                        this.PaintGraphics();                        
                        break;
                    case MouseMode.MoveSelection:
                        //Select elements
                        if (Math.Abs(e.X - _mouseDownPoint.X) < 2 && Math.Abs(e.Y - _mouseDownPoint.Y) < 2)
                        {
                            LayoutElement aElement = _selectedElements[0];
                            _selectedElements = SelectElements(pageP, _layoutElements, 0);
                            if (_selectedElements.Count > 1)
                            {
                                aElement.Selected = false;
                                int idx = _selectedElements.IndexOf(aElement);
                                if (idx == 0)
                                    idx = _selectedElements.Count - 1;
                                else
                                    idx -= 1;
                                if (idx < 0)
                                    idx = 0;
                                aElement = _selectedElements[idx];
                                _selectedElements.Clear();
                                _selectedElements.Add(aElement);
                                _selectedElements[0].Selected = true;
                                if (_selectedElements[0].ElementType == ElementType.LayoutMap)
                                    SetActiveMapFrame(((LayoutMap)_selectedElements[0]).MapFrame);

                                OnElementSeleted();
                            }
                        }
                        else
                        {
                            foreach (LayoutElement aElement in _selectedElements)
                            {
                                aElement.Left += (int)((e.X - _mouseDownPoint.X) / _zoom);
                                aElement.Top += (int)((e.Y - _mouseDownPoint.Y) / _zoom);
                                aElement.MoveUpdate();
                            }
                        }

                        _mouseMode = MouseMode.Select;
                        this.PaintGraphics();
                        break;    
                    case MouseMode.ResizeSelected:
                        _mouseMode = MouseMode.Select;
                        _selectedElements[0].ResizeUpdate();
                        this.PaintGraphics();
                        break;
                    case MouseMode.New_Rectangle:
                    case MouseMode.New_Ellipse:
                        if (e.Button == MouseButtons.Left)
                        {
                            if (e.X - _mouseDownPoint.X < 2 || e.Y - _mouseDownPoint.Y < 2)
                                return;

                            _startNewGraphic = true;
                            _graphicPoints = new List<PointF>();
                            _graphicPoints.Add(new PointF(_mouseDownPoint.X, _mouseDownPoint.Y));
                            _graphicPoints.Add(new PointF(_mouseDownPoint.X, e.Y));
                            _graphicPoints.Add(new PointF(e.X, e.Y));
                            _graphicPoints.Add(new PointF(e.X, _mouseDownPoint.Y));
                            List<PointD> points = new List<PointD>();
                            foreach (PointF aPoint in _graphicPoints)
                            {
                                PointF bPoint = ScreenToPage(aPoint.X, aPoint.Y);
                                points.Add(new PointD(bPoint.X, bPoint.Y));
                            }

                            Graphic aGraphic = null;
                            switch (_mouseMode)
                            {
                                case MouseMode.New_Rectangle:
                                    RectangleShape aPGS = new RectangleShape();
                                    aPGS.Points = points;
                                    aGraphic = new Graphic(aPGS, (PolygonBreak)_defPolygonBreak.Clone());
                                    break;
                                case MouseMode.New_Ellipse:
                                    EllipseShape aES = new EllipseShape();
                                    aES.Points = points;
                                    aGraphic = new Graphic(aES, (PolygonBreak)_defPolygonBreak.Clone());
                                    break;
                            }   
                            
                            if (aGraphic != null)
                            {
                                AddElement(new LayoutGraphic(aGraphic, this));
                                this.PaintGraphics();
                            }
                            else
                                this.Refresh();
                        }
                        break;
                    case MouseMode.New_Freehand:
                    case MouseMode.Map_SelectFeatures_Lasso:
                        if (e.Button == MouseButtons.Left)
                        {
                            _startNewGraphic = true;
                            if (_graphicPoints.Count < 2)
                                break;
                            
                            if (_mouseMode == MouseMode.New_Freehand)
                            {
                                List<PointD> points = new List<PointD>();
                                foreach (PointF aPoint in _graphicPoints)
                                {
                                    PointF bPoint = ScreenToPage(aPoint.X, aPoint.Y);
                                    points.Add(new PointD(bPoint.X, bPoint.Y));
                                }

                                Graphic aGraphic = null;
                                PolylineShape aPLS = new PolylineShape();
                                aPLS.Points = points;
                                aGraphic = new Graphic(aPLS, (PolyLineBreak)_defPolylineBreak.Clone());

                                if (aGraphic != null)
                                {
                                    AddElement(new LayoutGraphic(aGraphic, this));
                                    this.PaintGraphics();
                                }
                                else
                                    this.Refresh();
                            }
                            else
                            {
                                PointF mapP = PageToScreen(_currentLayoutMap.Left, _currentLayoutMap.Top);
                                List<PointD> points = new List<PointD>();
                                double projX = 0, projY = 0;
                                MapView currentMapView = _currentLayoutMap.MapFrame.MapView;
                                foreach (PointF aPoint in _graphicPoints)
                                {
                                    currentMapView.ScreenToProj(ref projX, ref projY, aPoint.X - mapP.X, aPoint.Y - mapP.Y);
                                    points.Add(new PointD(projX, projY));
                                }

                                MapLayer aMLayer = _currentLayoutMap.MapFrame.MapView.GetLayerFromHandle(_currentLayoutMap.MapFrame.MapView.SelectedLayer);
                                if (aMLayer == null)
                                    return;
                                if (aMLayer.LayerType != LayerTypes.VectorLayer)
                                    return;

                                PolygonShape aPGS = new PolygonShape();
                                points.Add((PointD)points[0].Clone());
                                aPGS.Points = points;
                                VectorLayer aLayer = (VectorLayer)aMLayer;
                                if (!((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                                {
                                    aLayer.ClearSelectedShapes();
                                }
                                aLayer.SelectShapes(aPGS);
                                _currentLayoutMap.MapFrame.MapView.RaiseShapeSelectedEvent();
                            }
                        }
                        break;
                    case MouseMode.New_Circle:
                    case MouseMode.Map_SelectFeatures_Circle:
                        if (e.Button == MouseButtons.Left)
                        {
                            if (e.X - _mouseDownPoint.X < 2 || e.Y - _mouseDownPoint.Y < 2)
                                return;

                            float radius = (float)Math.Sqrt(Math.Pow(e.X - _mouseDownPoint.X, 2) +
                                Math.Pow(e.Y - _mouseDownPoint.Y, 2));
                            _startNewGraphic = true;
                            _graphicPoints = new List<PointF>();
                            _graphicPoints.Add(new PointF(_mouseDownPoint.X - radius, _mouseDownPoint.Y));
                            _graphicPoints.Add(new PointF(_mouseDownPoint.X, _mouseDownPoint.Y - radius));
                            _graphicPoints.Add(new PointF(_mouseDownPoint.X + radius, _mouseDownPoint.Y));
                            _graphicPoints.Add(new PointF(_mouseDownPoint.X, _mouseDownPoint.Y + radius));
                            if (_mouseMode == MouseMode.New_Circle)
                            {
                                List<PointD> points = new List<PointD>();
                                foreach (PointF aPoint in _graphicPoints)
                                {
                                    PointF bPoint = ScreenToPage(aPoint.X, aPoint.Y);
                                    points.Add(new PointD(bPoint.X, bPoint.Y));
                                }

                                Graphic aGraphic = null;
                                CircleShape aPGS = new CircleShape();
                                aPGS.Points = points;
                                aGraphic = new Graphic(aPGS, (PolygonBreak)_defPolygonBreak.Clone());

                                if (aGraphic != null)
                                {
                                    AddElement(new LayoutGraphic(aGraphic, this));
                                    this.PaintGraphics();
                                }
                                else
                                    this.Refresh();
                            }
                            else
                            {
                                PointF mapP = PageToScreen(_currentLayoutMap.Left, _currentLayoutMap.Top);
                                List<PointD> points = new List<PointD>();
                                double projX = 0, projY = 0;
                                MapView currentMapView = _currentLayoutMap.MapFrame.MapView;
                                foreach (PointF aPoint in _graphicPoints)
                                {
                                    currentMapView.ScreenToProj(ref projX, ref projY, aPoint.X - mapP.X, aPoint.Y - mapP.Y);
                                    points.Add(new PointD(projX, projY));
                                }

                                MapLayer aMLayer = _currentLayoutMap.MapFrame.MapView.GetLayerFromHandle(_currentLayoutMap.MapFrame.MapView.SelectedLayer);
                                if (aMLayer == null)
                                    return;
                                if (aMLayer.LayerType != LayerTypes.VectorLayer)
                                    return;

                                CircleShape aPGS = new CircleShape();
                                points.Add((PointD)points[0].Clone());
                                aPGS.SetPoints(points);
                                VectorLayer aLayer = (VectorLayer)aMLayer;
                                if (!((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                                {
                                    aLayer.ClearSelectedShapes();
                                }
                                aLayer.SelectShapes(aPGS);
                                _currentLayoutMap.MapFrame.MapView.RaiseShapeSelectedEvent();
                            }
                        }
                        break; 
                    case MouseMode.InEditingVertices:
                        ((LayoutGraphic)_selectedElements[0]).VerticeEditUpdate(_editingVerticeIndex, pageP.X, pageP.Y);
                        _mouseMode = MouseMode.EditVertices;
                        this.PaintGraphics();                        
                        break;
                }
            }
        }

        /// <summary>
        /// Override OnMouseDoubleClick event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {            
            base.OnMouseDoubleClick(e);

            Point pageP = ScreenToPage(e.X, e.Y);
            switch (_mouseMode)
            {
                //case MouseMode.Select:
                //case MouseMode.CreateSelection:
                case MouseMode.MoveSelection:
                case MouseMode.ResizeSelected:
                    LayoutElement aElement = _selectedElements[0];
                    _selectedElements = SelectElements(pageP, _layoutElements, 0);
                    if (_selectedElements.Count > 1)
                    {
                        aElement.Selected = false;
                        int idx = _selectedElements.IndexOf(aElement);
                        if (idx == _selectedElements.Count - 1)
                            idx = 0;
                        else
                            idx += 1;
                        aElement = _selectedElements[idx];
                        _selectedElements.Clear();
                        _selectedElements.Add(aElement);
                        _selectedElements[0].Selected = true;
                    }
                    this.PaintGraphics();

                    if (aElement.ElementType == ElementType.LayoutGraphic)
                    {
                        Graphic aGraphic = ((LayoutGraphic)aElement).Graphic;

                        switch (aGraphic.Legend.BreakType)
                        {
                            case BreakTypes.PointBreak:
                                if (!_frmPointSymbolSet.Visible)
                                    {
                                        _frmPointSymbolSet = new frmPointSymbolSet(this);
                                        //_frmPointSymbolSet.SetParent(this);
                                        _frmPointSymbolSet.Show(this);
                                        //_frmPointSymbolSet.ShowDialog();
                                    }
                                    _frmPointSymbolSet.PointBreak = (PointBreak)aGraphic.Legend;
                                    break;
                            case BreakTypes.LabelBreak:
                                if (!_frmLabelSymbolSet.Visible)
                                {
                                    _frmLabelSymbolSet = new frmLabelSymbolSet();
                                    _frmLabelSymbolSet.SetParent(this);
                                    _frmLabelSymbolSet.Show(this);
                                }
                                _frmLabelSymbolSet.LabelBreak = (LabelBreak)aGraphic.Legend;
                                break;
                            case BreakTypes.PolylineBreak:
                                if (!_frmPolylineSymbolSet.Visible)
                                {
                                    _frmPolylineSymbolSet = new frmPolylineSymbolSet(this);
                                    //_frmPolylineSymbolSet.SetParent(this);
                                    _frmPolylineSymbolSet.Show(this);
                                }
                                _frmPolylineSymbolSet.PolylineBreak = (PolyLineBreak)aGraphic.Legend;
                                break;
                            case BreakTypes.PolygonBreak:
                                if (!_frmPolygonSymbolSet.Visible)
                                {
                                    _frmPolygonSymbolSet = new frmPolygonSymbolSet(this);
                                    //_frmPolygonSymbolSet.SetParent(this);
                                    _frmPolygonSymbolSet.Show(this);
                                }
                                _frmPolygonSymbolSet.PolygonBreak = (PolygonBreak)aGraphic.Legend;
                                break;
                        }   

                        //if (aGraphic.Shape.ShapeType == ShapeTypes.Point)
                        //{
                        //    frmPointSymbolSet aFrmPSS = new frmPointSymbolSet(true);
                        //    aFrmPSS.PointBreak = (PointBreak)aGraphic.Legend;
                        //    aFrmPSS.SetParent(this);
                        //    aFrmPSS.ShowDialog();
                        //}
                        //else
                        //{
                        //    object propertyObj = aElement.GetPropertyObject();
                        //    if (propertyObj != null)
                        //    {
                        //        frmProperty aFrmProperty = new frmProperty(true);
                        //        aFrmProperty.SetObject(propertyObj);
                        //        aFrmProperty.SetParent(this);
                        //        aFrmProperty.ShowDialog();
                        //    }
                        //}
                    }
                    else
                    {
                        object propertyObj = aElement.GetPropertyObject();
                        if (propertyObj != null)
                        {
                            frmProperty aFrmProperty = new frmProperty(true);
                            aFrmProperty.SetObject(propertyObj);
                            aFrmProperty.SetParent(this);
                            aFrmProperty.ShowDialog();
                        }
                    }
                    MouseMode = MouseMode.Select;
                    OnElementSeleted();
                    break;
                case MouseMode.New_Polyline:
                case MouseMode.New_Polygon:
                case MouseMode.New_Curve:
                case MouseMode.New_CurvePolygon:
                case MouseMode.New_Freehand:
                case MouseMode.Map_SelectFeatures_Polygon:
                    if (!_startNewGraphic)
                    {
                        _startNewGraphic = true;
                        //_graphicPoints.Add(new PointF(e.X, e.Y));
                        _graphicPoints.RemoveAt(_graphicPoints.Count - 1);

                        if (_mouseMode == MouseMode.Map_SelectFeatures_Polygon)
                        {
                            PointF mapP = PageToScreen(_currentLayoutMap.Left, _currentLayoutMap.Top);
                            List<PointD> points = new List<PointD>();
                            double projX = 0, projY = 0;
                            MapView currentMapView = _currentLayoutMap.MapFrame.MapView;
                            foreach (PointF aPoint in _graphicPoints)
                            {
                                currentMapView.ScreenToProj(ref projX, ref projY, aPoint.X - mapP.X, aPoint.Y - mapP.Y);
                                points.Add(new PointD(projX, projY));
                            }

                            MapLayer aMLayer = _currentLayoutMap.MapFrame.MapView.GetLayerFromHandle(_currentLayoutMap.MapFrame.MapView.SelectedLayer);
                            if (aMLayer == null)
                                return;
                            if (aMLayer.LayerType != LayerTypes.VectorLayer)
                                return;

                            PolygonShape aPGS = new PolygonShape();
                            points.Add((PointD)points[0].Clone());
                            aPGS.Points = points;
                            VectorLayer aLayer = (VectorLayer)aMLayer;
                            if (!((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                            {
                                aLayer.ClearSelectedShapes();
                            }
                            aLayer.SelectShapes(aPGS);
                            _currentLayoutMap.MapFrame.MapView.RaiseShapeSelectedEvent();
                        }
                        else
                        {
                            List<PointD> points = new List<PointD>();
                            foreach (PointF aPoint in _graphicPoints)
                            {
                                PointF bPoint = ScreenToPage(aPoint.X, aPoint.Y);
                                points.Add(new PointD(bPoint.X, bPoint.Y));
                            }

                            Graphic aGraphic = null;
                            switch (_mouseMode)
                            {
                                case MouseMode.New_Polyline:
                                case MouseMode.New_Freehand:
                                    PolylineShape aPLS = new PolylineShape();
                                    aPLS.Points = points;
                                    aGraphic = new Graphic(aPLS, (PolyLineBreak)_defPolylineBreak.Clone());
                                    break;
                                case MouseMode.New_Polygon:
                                    if (points.Count > 2)
                                    {
                                        PolygonShape aPGS = new PolygonShape();
                                        aPGS.Points = points;
                                        aGraphic = new Graphic(aPGS, (PolygonBreak)_defPolygonBreak.Clone());
                                    }
                                    break;
                                case MouseMode.New_Curve:
                                    CurveLineShape aCLS = new CurveLineShape();
                                    aCLS.Points = points;
                                    aGraphic = new Graphic(aCLS, (PolyLineBreak)_defPolylineBreak.Clone());
                                    break;
                                case MouseMode.New_CurvePolygon:
                                    if (points.Count > 2)
                                    {
                                        CurvePolygonShape aCPS = new CurvePolygonShape();
                                        aCPS.Points = points;
                                        aGraphic = new Graphic(aCPS, (PolygonBreak)_defPolygonBreak.Clone());
                                    }
                                    break;
                            }

                            if (aGraphic != null)
                            {
                                AddElement(new LayoutGraphic(aGraphic, this));
                                this.PaintGraphics();
                            }
                            else
                                this.Refresh();
                        }
                    }
                    break;
            }            
        }

        /// <summary>
        /// Override OnKeyDown event
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            
            if (_mouseMode == MouseMode.Select)
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        OnRemoveElementClick(this, e);                        
                        break;
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Up:
                    case Keys.Down:
                        int x = 0;
                        int y = 0;
                        int d = 5;
                        switch (e.KeyCode)
                        {
                            case Keys.Left:
                                x = -d;
                                break;
                            case Keys.Right:
                                x = d;
                                break; 
                            case Keys.Up:
                                y = -d;
                                break;
                            case Keys.Down:
                                y = d;
                                break;
                        }
                        for (int i = 0; i < _layoutElements.Count; i++)
                        {
                            LayoutElement aElement = _layoutElements[i];
                            if (aElement.Selected)
                            {
                                if (x != 0)
                                    aElement.Left += x;
                                if (y != 0)
                                    aElement.Top += y;
                                aElement.MoveUpdate();
                            }
                        }
                        break;
                }                
            }            
        }

        /// <summary>
        /// This fires when the vscrollbar is moved
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">ScrollEventArgs</param>
        void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            _pageLocation.Y = _pageLocation.Y + (e.OldValue - e.NewValue);
            this.PaintGraphics();
            //this.Invalidate();
        }

        /// <summary>
        /// This fires when the hscrollbar is moved
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">ScrollEventArgs</param>
        void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            _pageLocation.X = _pageLocation.X + (e.OldValue - e.NewValue);
            this.PaintGraphics();
            //this.Invalidate();
        }

        /// <summary>
        /// Fire the ElementSelected event
        /// </summary>
        protected virtual void OnElementSeleted()
        {
            if (ElementSeleted != null) ElementSeleted(this, new EventArgs());
        }

        /// <summary>
        /// Fire the ZoomChanged event
        /// </summary>
        protected virtual void OnZoomChanged()
        {
            if (ZoomChanged != null) ZoomChanged(this, new EventArgs());
        }

        /// <summary>
        /// Fires the ActiveMapFrameChanged event
        /// </summary>
        protected virtual void OnActiveMapFrameChanged()
        {
            if (ActiveMapFrameChanged != null) ActiveMapFrameChanged(this, new EventArgs());
        }

        /// <summary>
        /// Fire the MapFramesUpdated event
        /// </summary>
        protected virtual void OnMapFramesUpdated()
        {
            if (MapFramesUpdated != null) MapFramesUpdated(this, new EventArgs());
        }

        private void FrmMeasurementClosed(object sender, FormClosedEventArgs e)
        {
            this.Refresh();
        }

        #endregion
    }
}
