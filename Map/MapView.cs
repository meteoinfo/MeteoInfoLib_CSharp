using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Data;

using MeteoInfoC.Layer;
using MeteoInfoC.Shape;
using MeteoInfoC.Global;
using MeteoInfoC.Drawing;
using MeteoInfoC.Legend;
using MeteoInfoC.Projections;
using MeteoInfoC.Data.MapData;
using MeteoInfoC.Layout;
using MeteoInfoC.Geoprocess;

namespace MeteoInfoC.Map
{
    /// <summary>
    /// Map view control
    /// </summary>
    public class MapView:UserControl,IMapView
    {
        #region Events definition
        /// <summary>
        /// Occurs after view extent changed.
        /// </summary>
        public event EventHandler ViewExtentChanged;
        /// <summary>
        /// Occurs after layers updated.
        /// </summary>
        public event EventHandler LayersUpdated;
        /// <summary>
        /// Occurs after paint layers
        /// </summary>
        public event EventHandler MapViewRedrawed;
        /// <summary>
        /// Occurs after back color or layer legend render changed
        /// </summary>
        public event EventHandler RenderChanged;
        /// <summary>
        /// Occurs after one of the graphic is selected.
        /// </summary>
        public event EventHandler GraphicSeleted;
        /// <summary>
        /// Occurs after projection changed
        /// </summary>
        public event EventHandler ProjectionChanged;
        /// <summary>
        /// Occurs after some shapes are selected
        /// </summary>
        public event EventHandler ShapeSelected;

        #endregion

        #region Variables

        //const double PI = 3.1415926535;

        /// <summary>
        /// Identifer form
        /// </summary>
        private frmIdentifer _frmIdentifer;
        /// <summary>
        /// Identifer form for grid layer
        /// </summary>
        private frmIdentiferGrid _frmIdentiferGrid = new frmIdentiferGrid();
        private frmMeasurement _frmMeasure = new frmMeasurement();

        //clsLayersTreeView m_LayersTV = new clsLayersTreeView();
        private Extent _extent = new Extent();
        private Extent _viewExtent = new Extent();
        private Extent _drawExtent = new Extent();
        private LayerCollection _layerSet = new LayerCollection();
        private int _selectedLayer;
        private bool _isGeoMap = true;
        private ProjectionSet _projection = new ProjectionSet();
        private MouseTools _mouseTool = new MouseTools();
        //private MapExtentSet m_MapExtentSet;
        private List<string> _xGridStrs = new List<string>();
        private List<string> _yGridStrs = new List<string>();
        private List<object[]> _xGridPosLabel = new List<object[]>();
        private List<object[]> _yGridPosLabel = new List<object[]>();
        private List<GridLabel> _gridLabels = new List<GridLabel>();
        private bool _lockViewUpdate = false;
        private bool _drawNeatLine = false;
        private Color _neatLineColor = Color.Black;
        private int _neatLineSize = 1;
        private bool _drawGridTickLine = false;
        private Color _gridLineColor = Color.Gray;
        private int _gridLineSize = 1;
        private DashStyle _gridLineStyle = DashStyle.Dash;
        private bool _drawGridLine = false;
        private double _gridXDelt = 10;
        private double _gridYDelt = 10;
        private double _gridXOrigin = 0;
        private double _gridYOrigin = -90;
        private bool _gridDeltChanged = false;

        private bool _isLayoutMap = false;
        private Color _selectColor;
        private bool _IsPaint;
        //private Image _animatedImage = null;
        
        Bitmap _mapBitmap = new Bitmap(10, 10, PixelFormat.Format32bppPArgb);
        Bitmap _tempMapBitmap = new Bitmap(10, 10, PixelFormat.Format32bppPArgb);
        Point _mouseDownPoint = new Point(0, 0);
        Point _mouseLastPos = new Point(0, 0);
        Point _mousePos = new Point(0, 0);
        private int _xShift = 0;
        private int _yShift = 0;
                        
        //private MapProperty m_MapProperty;
        private SmoothingMode _smoothingMode = SmoothingMode.Default;
        private SmoothingMode _pointSmoothingMode = SmoothingMode.AntiAlias;
        private MaskOut _maskOut;
        private GraphicsPath _maskOutGraphicsPath = new GraphicsPath();
        private bool _highSpeedWheelZoom = false;               

        private VectorLayer _lonLatLayer = null;
        private VectorLayer _lonLatProjLayer = null;
        
        private double _scaleX;
        private double _scaleY;
        private double _XYScaleFactor;
        private bool _multiGlobalDraw = true;

        private GraphicCollection _graphicCollection = new GraphicCollection();
        private bool _startNewGraphic = true;
        private List<PointF> _graphicPoints = new List<PointF>();
        private GraphicCollection _visibleGraphics = new GraphicCollection();
        private GraphicCollection _selectedGraphics = new GraphicCollection();
        //private bool _isInSelectedGraphics = false;
        private Rectangle _selectedRectangle = new Rectangle();
        private bool m_IsSelectedInLayout;

        //private bool _isEditingVertice = false;
        private Edge _resizeSelectedEdge = Edge.None;        
        private Rectangle _resizeRectangle = new Rectangle();

        private List<PointD> _editingVertices = new List<PointD>();
        private int _editingVerticeIndex;

        private PointBreak _defPointBreak = new PointBreak();
        private LabelBreak _defLabelBreak = new LabelBreak();
        private PolyLineBreak _defPolylineBreak = new PolyLineBreak();
        private PolygonBreak _defPolygonBreak = new PolygonBreak();

        private frmPointSymbolSet _frmPointSymbolSet = new frmPointSymbolSet(true);
        private frmPolylineSymbolSet _frmPolylinSymbolSet = new frmPolylineSymbolSet(true);
        private frmPolygonSymbolSet _frmPolygonSymbolSet = new frmPolygonSymbolSet(true);
        private frmLabelSymbolSet _frmLabelSymbolSet = new frmLabelSymbolSet();
        //private frmProperty _frmSymbolProp = new frmProperty(true);

        private bool _drawIdentiferShape = false;
        private bool _mouseDoubleClicked = false;    //If fired mouse double click event

        private DateTime _lastMouseWheelTime;
        private Timer _mouseWheelDetctionTimer = new Timer();

        #endregion               

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MapView()
        {
            this.SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.OptimizedDoubleBuffer, true);

            _mouseWheelDetctionTimer.Tick += new EventHandler(MouseWheelDetectionTimer_Tick);

            _frmIdentifer = new frmIdentifer(this);
            _frmMeasure.FormClosed += new FormClosedEventHandler(FrmMeasurementClosed);
            _maskOut = new MaskOut(this);
            this.BackColor = Color.White;

            //_projection = new ProjectionSet();
            _mapBitmap = new Bitmap(10, 10, PixelFormat.Format32bppPArgb);
            _mouseTool = MouseTools.None;
            //m_MapProperty = new MapProperty(this);                        
            //m_MapExtentSet = new MapExtentSet(0, 360, -90, 90);
            _viewExtent.minX = 0;
            _viewExtent.maxX = 360;
            _viewExtent.minY = -90;
            _viewExtent.maxY = 90;
            _drawExtent = _viewExtent;

            _scaleX = 1;
            _scaleY = 1;
            _XYScaleFactor = 1.2;
            
            m_IsSelectedInLayout = false;
            _selectColor = Color.Cyan;
            _IsPaint = true;

            _defPointBreak.Size = 10;
            _defLabelBreak.Text = "Text";
            _defLabelBreak.Font = new Font("Arial", 10);
            _defPolylineBreak.Color = Color.Red;
            _defPolylineBreak.Size = 2;
            _defPolygonBreak.Color = Color.FromArgb (125, Color.LightYellow);
            //_defPolygonBreak.TransparencyPercent = 50;
        }

        #endregion

        #region Property

        #region Setting map properties
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
                    if (_smoothingMode == SmoothingMode.AntiAlias || _smoothingMode == SmoothingMode.HighQuality)
                        _pointSmoothingMode = value;
                    PaintLayers();
                }
            }
        }

        /// <summary>
        /// Get or set smoothing mode of GDI+
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set point smoothing mode")]
        public SmoothingMode PointSmoothingMode
        {
            get
            {
                return _pointSmoothingMode;
            }
            set
            {
                if (value != SmoothingMode.Invalid)
                {
                    _pointSmoothingMode = value;
                    PaintLayers();
                }
            }
        }

        /// <summary>
        /// Get or set if using high speed zoom - zoom with image
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set if using high speed zoom option")]
        public bool HighSpeedWheelZoom
        {
            get { return _highSpeedWheelZoom; }
            set { _highSpeedWheelZoom = value; }
        }

        /// <summary>
        /// Get or set fore color of map view
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set map fore color")]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                PaintLayers();
            }
        }

        /// <summary>
        /// Get or set back color of map region
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set map back color")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                if (base.BackColor == Color.Black)
                {
                    base.ForeColor = Color.White;
                    _neatLineColor = Color.White;
                    _gridLineColor = Color.White;
                }
                else if (base.BackColor == Color.White)
                {
                    base.ForeColor = Color.Black;
                    _neatLineColor = Color.Black;
                    _gridLineColor = Color.Black;
                }

                OnRenderChanged();
                PaintLayers();
            }
        }

        /// <summary>
        /// Get or set X/Y scale factor
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("X/Y scale factor")]
        public double XYScaleFactor
        {
            get { return _XYScaleFactor; }
            set 
            { 
                _XYScaleFactor = value;
                if (_XYScaleFactor < 0.5 || _XYScaleFactor > 2)
                    _XYScaleFactor = 1;
                ZoomToExtent(_drawExtent);
            }
        }

        /// <summary>
        /// Get or set if draw muti global extents - only validate with lon/lat projection
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Draw multi global extents")]
        public bool MultiGlobalDraw
        {
            get { return _multiGlobalDraw; }
            set 
            { 
                _multiGlobalDraw = value;
                PaintLayers();
            }
        }

        #endregion        

        #region Others

        /// <summary>
        /// Get map bitmap
        /// </summary>
        public Bitmap MapBitMap
        {
            get { return _mapBitmap; }
        }

        /// <summary>
        /// Get last added layer
        /// </summary>
        public MapLayer LastAddedLayer
        {
            get
            {
                int hnd = 0;
                for (int i = 0; i < _layerSet.Layers.Count; i++)
                {
                    if (_layerSet.Layers[i].Handle > hnd)
                        hnd = _layerSet.Layers[i].Handle;
                }

                return GetLayerFromHandle(hnd);
            }
        }

        /// <summary>
        /// Get or set the whole extent of all layers
        /// </summary>
        public Extent Extent
        {
            get { return _extent; }
            set { _extent = value; }
        }

        /// <summary>
        /// Get or set MapView extent
        /// </summary>
        public Extent ViewExtent
        {
            get { return _viewExtent; }
            set 
            {
                _viewExtent = value;
                _drawExtent = _viewExtent;
                if (_isGeoMap)
                {
                    SetCoordinateGeoMap(ref _drawExtent);
                }
                else
                {
                    SetCoordinateMap(ref _drawExtent);
                }
            }
        }

        /// <summary>
        /// Get draw extent
        /// </summary>
        public Extent DrawExtent
        {
            get { return _drawExtent; }
        }

        /// <summary>
        /// Get x scale
        /// </summary>
        public double XScale
        {
            get { return _scaleX; }
        }

        /// <summary>
        /// Get y scale
        /// </summary>
        public double YScale
        {
            get { return _scaleY; }
        }

        ///// <summary>
        ///// Get or set mask out graphics path
        ///// </summary>
        //public GraphicsPath MaskOutGraphicsPath
        //{
        //    get { return _maskOutGraphicsPath; }
        //    set { _maskOutGraphicsPath = value; }
        //}

        /// <summary>
        /// Get or set if the map view is for geographical map
        /// </summary>
        public bool IsGeoMap
        {
            get { return _isGeoMap; }
            set { _isGeoMap = value; }
        }

        /// <summary>
        /// Get or set projection
        /// </summary>
        public ProjectionSet Projection
        {
            get { return _projection; }
            set { _projection = value; }
        }

        /// <summary>
        /// Get or set layer set
        /// </summary>
        public LayerCollection LayerSet
        {
            get { return _layerSet; }
            set { _layerSet = value; }
        }

        /// <summary>
        /// Get layers
        /// </summary>
        public List<MapLayer> Layers
        {
            get { return _layerSet.Layers; }
        }

        /// <summary>
        /// Selected layer
        /// </summary>
        public int SelectedLayer
        {
            get { return _selectedLayer; }
            set { _selectedLayer = value; }
        }

        /// <summary>
        /// Get or set select color
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("The color of selected shapes")]
        public Color SelectColor
        {
            get { return _selectColor; }
            set { _selectColor = value; }
        }

        /// <summary>
        /// Get or set mouse tool
        /// </summary>
        public MouseTools MouseTool
        {
            get { return _mouseTool; }
            set 
            { 
                _mouseTool = value;
                Cursor myCursor;
                switch (_mouseTool)
                {
                    case MouseTools.None:
                    case MouseTools.SelectElements:
                        this.Cursor = Cursors.Default;
                        break;
                    case MouseTools.Zoom_In:
                        myCursor = new Cursor(this.GetType().Assembly.
                            GetManifestResourceStream("MeteoInfoC.Resources.zoom_in.cur"));
                        this.Cursor = myCursor;
                        break;
                    case MouseTools.Zoom_Out:
                        myCursor = new Cursor(this.GetType().Assembly.
                            GetManifestResourceStream("MeteoInfoC.Resources.zoom_out.cur"));
                        this.Cursor = myCursor;
                        break;
                    case MouseTools.Pan:
                        myCursor = new Cursor(this.GetType().Assembly.
                            GetManifestResourceStream("MeteoInfoC.Resources.Pan_Open.cur"));
                        this.Cursor = myCursor;
                        break;
                    case MouseTools.Identifer:
                        myCursor = new Cursor(this.GetType().Assembly.
                            GetManifestResourceStream("MeteoInfoC.Resources.cursor.cur"));
                        this.Cursor = myCursor;
                        break;
                    case MouseTools.SelectFeatures_Rectangle:
                    case MouseTools.SelectFeatures_Polygon:
                    case MouseTools.SelectFeatures_Lasso:
                    case MouseTools.SelectFeatures_Circle:
                    case MouseTools.New_Label:
                    case MouseTools.New_Point:
                    case MouseTools.New_Polygon:
                    case MouseTools.New_Polyline:
                    case MouseTools.New_Rectangle:
                    case MouseTools.New_Circle:
                    case MouseTools.New_Curve:
                    case MouseTools.New_CurvePolygon:
                    case MouseTools.New_Ellipse:
                    case MouseTools.New_Freehand:
                        this.Cursor = Cursors.Cross;
                        break;
                    case MouseTools.Measurement:
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

        ///// <summary>
        ///// Get or set map extent set
        ///// </summary>
        //public MapExtentSet MapExtentSetV
        //{
        //    get { return m_MapExtentSet; }
        //    set { m_MapExtentSet = value; }
        //}

        /// <summary>
        /// Get or set X grid labels
        /// </summary>
        public List<string> XGridStrs
        {
            get { return _xGridStrs; }
            set { _xGridStrs = value; }
        }

        /// <summary>
        /// Get or set y grid labels
        /// </summary>
        public List<string> YGridStrs
        {
            get { return _yGridStrs; }
            set { _yGridStrs = value; }
        }

        /// <summary>
        /// Get X grid position points and labels
        /// </summary>
        public List<object[]> XGridPosLabel
        {
            get { return _xGridPosLabel; }
        }

        /// <summary>
        /// Get y grid position points and labels
        /// </summary>
        public List<object[]> YGridPosLabel
        {
            get { return _yGridPosLabel; }
        }

        /// <summary>
        /// Get grid line labels
        /// </summary>
        public List<GridLabel> GridLabels
        {
            get { return _gridLabels; }
        }

        ///// <summary>
        ///// Get or set map property
        ///// </summary>
        //public MapProperty MapPropertyV
        //{
        //    get { return m_MapProperty; }
        //    set { m_MapProperty = value; }
        //}

        /// <summary>
        /// Get or set longitude/latitude layer
        /// </summary>
        public VectorLayer LonLatLayer
        {
            get { return _lonLatLayer; }
            set { _lonLatLayer = value; }
        }

        /// <summary>
        /// Get or set projected longitude/latitude layer
        /// </summary>
        public VectorLayer LonLatProjLayer
        {
            get { return _lonLatProjLayer; }
            set { _lonLatProjLayer = value; }
        }

        /// <summary>
        /// Get or set mask out layer
        /// </summary>
        public MaskOut MaskOut
        {
            get { return _maskOut; }
            set { _maskOut = value; }
        }        

        /// <summary>
        /// Get or set if lock view from update
        /// </summary>
        public bool LockViewUpdate
        {
            get { return _lockViewUpdate; }
            set { _lockViewUpdate = value; }
        }

        /// <summary>
        /// Get or set if draw neat line
        /// </summary>
        public bool DrawNeatLine
        {
            get { return _drawNeatLine; }
            set { _drawNeatLine = value; }
        }

        /// <summary>
        /// Get or set neat line color
        /// </summary>
        public Color NeatLineColor
        {
            get { return _neatLineColor; }
            set { _neatLineColor = value; }
        }

        /// <summary>
        /// Get or set neat line size
        /// </summary>
        public int NeatLineSize
        {
            get { return _neatLineSize; }
            set { _neatLineSize = value; }
        }

        /// <summary>
        /// Get or set if draw grid or tick line
        /// </summary>
        public bool DrawGridTickLine
        {
            get { return _drawGridTickLine; }
            set { _drawGridTickLine = value; }
        }

        /// <summary>
        /// Get or set gird line color
        /// </summary>        
        public Color GridLineColor
        {
            get { return _gridLineColor; }
            set { _gridLineColor = value; }
        }

        /// <summary>
        /// Get or set grid line size
        /// </summary>        
        public int GridLineSize
        {
            get { return _gridLineSize; }
            set { _gridLineSize = value; }
        }

        /// <summary>
        /// Get or set grid line style
        /// </summary>        
        public DashStyle GridLineStyle
        {
            get { return _gridLineStyle; }
            set { _gridLineStyle = value; }
        }

        /// <summary>
        /// Get or set if draw grid line
        /// </summary>        
        public bool DrawGridLine
        {
            get { return _drawGridLine; }
            set { _drawGridLine = value; }
        }

        /// <summary>
        /// Get or set grid x/longitude delt 
        /// </summary>
        public double GridXDelt
        {
            get { return _gridXDelt; }
            set 
            {
                _gridXDelt = value;
                _gridDeltChanged = true;
            }
        }

        /// <summary>
        /// Get or set grid y/latitude delt
        /// </summary>
        public double GridYDelt
        {
            get { return _gridYDelt; }
            set 
            { 
                _gridYDelt = value;
                _gridDeltChanged = true;
            }
        }

        /// <summary>
        /// Get or set grid x/longitude origin
        /// </summary>
        public double GridXOrigin
        {
            get { return _gridXOrigin; }
            set 
            {
                _gridXOrigin = value;
                _gridDeltChanged = true;
            }
        }

        /// <summary>
        /// Get or set grid y/latitude origin
        /// </summary>
        public double GridYOrigin
        {
            get { return _gridYOrigin; }
            set 
            {
                _gridYOrigin = value;
                _gridDeltChanged = true;
            }
        }

        /// <summary>
        /// Get or set if the MapView is in a Layout
        /// </summary>
        public bool IsLayoutMap
        {
            get { return _isLayoutMap; }
            set { _isLayoutMap = value; }
        }

        /// <summary>
        /// Get or set if the control is selected in layout
        /// </summary>
        public bool IsSelectedInLayout
        {
            get { return m_IsSelectedInLayout; }
            set { m_IsSelectedInLayout = value; }
        }

        /// <summary>
        /// Get or set if paint itself
        /// </summary>
        public bool IsPaint
        {
            get { return _IsPaint; }
            set { _IsPaint = value; }
        }

        /// <summary>
        /// Get graphic collection
        /// </summary>
        public GraphicCollection GraphicCollection
        {
            get { return _graphicCollection; }            
        }

        /// <summary>
        /// Get selected graphic collection
        /// </summary>
        public GraphicCollection SelectedGraphics
        {
            get { return _selectedGraphics; }
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
        /// Get or set if draw identifer shape
        /// </summary>
        public bool DrawIdentiferShape
        {
            get { return _drawIdentiferShape; }
            set { _drawIdentiferShape = value; }
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

        #region Methods

        #region General
        /// <summary>
        /// Set properties
        /// </summary>
        public void SetProperties()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("SmoothingMode", "SmoothingMode");
            objAttr.Add("PointSmoothingMode", "PointSmoothingMode");
            objAttr.Add("HighSpeedWheelZoom", "HighSpeedWheelZoom");
            objAttr.Add("ForeColor", "ForeColor");
            objAttr.Add("BackColor", "BackColor");
            objAttr.Add("XYScaleFactor", "XYscaleFactor");
            objAttr.Add("SelectColor", "SelectColor");
            objAttr.Add("MultiGlobalDraw", "MultiGlobalDraw");
            CustomProperty cp = new CustomProperty(this, objAttr);
            frmProperty aFrmP = new frmProperty();
            aFrmP.SetObject(cp);
            aFrmP.ShowDialog();
        }

        /// <summary>
        /// Clone map view
        /// </summary>
        /// <returns>map view</returns>
        public MapView Clone()
        {
            MapView aMapView = new MapView();
            aMapView.LayerSet = _layerSet;
            aMapView.Extent = _extent;
            aMapView.ViewExtent = _viewExtent;            
            aMapView.MaskOut = _maskOut;
            //aMapView.MapExtentSetV = m_MapExtentSet;
            //aMapView.MapPropertyV = m_MapProperty;
            aMapView.MouseTool = _mouseTool;
            aMapView.Projection = _projection;
            aMapView.IsGeoMap = _isGeoMap;

            return aMapView;
        }

        /// <summary>
        /// Update map view
        /// </summary>
        /// <param name="aMapView">a map view</param>
        public void UpdateMapView(MapView aMapView)
        {
            BackColor = aMapView.BackColor;
            ForeColor = aMapView.ForeColor;
            LayerSet = aMapView.LayerSet;
            SelectedLayer = aMapView.SelectedLayer;
            Extent = aMapView.Extent;
            ViewExtent = aMapView.ViewExtent;
            XYScaleFactor = aMapView.XYScaleFactor;
            MaskOut = aMapView.MaskOut;
            //MapExtentSetV = aMapView.MapExtentSetV.Clone();
            //MapExtentSetV = aMapView.MapExtentSetV;
            this.ForeColor = aMapView.ForeColor;
            this.BackColor = aMapView.BackColor;
            SmoothingMode = aMapView.SmoothingMode;
            //MapPropertyV = aMapView.MapPropertyV;
            //MapPropertyV.MapControl = this;
            MouseTool = aMapView.MouseTool;
            Projection = aMapView.Projection;
            IsGeoMap = aMapView.IsGeoMap;            
            LonLatLayer = aMapView.LonLatLayer;
            DrawGridTickLine = aMapView.DrawGridTickLine;
            DrawGridLine = aMapView.DrawGridLine;
            GridLineColor = aMapView.GridLineColor;
            GridLineSize = aMapView.GridLineSize;
            GridLineStyle = aMapView.GridLineStyle;
            _graphicCollection = aMapView.GraphicCollection;
            _selectedGraphics = aMapView.SelectedGraphics;

            ZoomToExtent(_viewExtent);
        }

        /// <summary>
        /// Set graphics
        /// </summary>
        /// <param name="aGCollection">graphic collection</param>
        public void SetGraphics(GraphicCollection aGCollection)
        {
            _graphicCollection = aGCollection;
        }

        /// <summary>
        /// Projection layers
        /// </summary>
        /// <param name="toProj">to projection info</param>
        public void ProjectLayers(ProjectionInfo toProj)
        {
            _projection.ProjectLayers(this, toProj);
            OnProjectionChanged();
        }

        /// <summary>
        /// Projection layers
        /// </summary>
        /// <param name="toProj">to projection info</param>
        /// <param name="isUpdateView">if paint mapview</param>
        public void ProjectLayers(ProjectionInfo toProj, Boolean isUpdateView)
        {
            _projection.ProjectLayers(this, toProj, isUpdateView);
            OnProjectionChanged();
        }

        #endregion

        #region Coordinate transfer
        /// <summary>
        /// Convert coordinate from map to screen
        /// </summary>
        /// <param name="projX">projection X</param>
        /// <param name="projY">projection Y</param>
        /// <param name="screenX">screen X</param>
        /// <param name="screenY">screen Y</param>       
        public void ProjToScreen(double projX, double projY, ref double screenX, ref double screenY)
        {
            screenX = (projX - _drawExtent.minX) * _scaleX;
            screenY = (_drawExtent.maxY - projY) * _scaleY;
        }

        /// <summary>
        /// Convert coordinate from map to screen
        /// </summary>
        /// <param name="projX">projection X</param>
        /// <param name="projY">projection Y</param>
        /// <param name="screenX">screen X</param>
        /// <param name="screenY">screen Y</param>
        /// <param name="LonShift">longitude shift</param>        
        public void ProjToScreen(double projX, double projY, ref double screenX, ref double screenY,
            double LonShift)
        {
            screenX = (projX + LonShift - _drawExtent.minX) * _scaleX;
            screenY = (_drawExtent.maxY - projY) * _scaleY;
        }
        
        /// <summary>
        /// Convert coordinate from map to screen
        /// </summary>
        /// <param name="projX">projection X</param>
        /// <param name="projY">projection Y</param>
        /// <param name="screenX">screen X</param>
        /// <param name="screenY">screen Y</param>
        /// <param name="LonShift">longitude shift</param>        
        public void ProjToScreen(double projX, double projY, ref Single screenX, ref Single screenY,
            double LonShift)
        {
            screenX = Convert.ToSingle((projX + LonShift - _drawExtent.minX) * _scaleX);
            screenY = Convert.ToSingle((_drawExtent.maxY - projY) * _scaleY);
        }

        /// <summary>
        /// Convert coordinate from map to screen
        /// </summary>
        /// <param name="projX">projection X</param>
        /// <param name="projY">projection Y</param>
        /// <param name="screenX">screen X</param>
        /// <param name="screenY">screen Y</param>            
        public void ProjToScreen(double projX, double projY, ref Single screenX, ref Single screenY)
        {
            screenX = Convert.ToSingle((projX - _drawExtent.minX) * _scaleX);
            screenY = Convert.ToSingle((_drawExtent.maxY - projY) * _scaleY);
        }

        /// <summary>
        /// Longitude/Latitude convert to screen X/Y
        /// </summary>
        /// <param name="lon">longitude</param>
        /// <param name="lat">latitude</param>
        /// <param name="screenX">screen X</param>
        /// <param name="screenY">screen Y</param>
        public void LonLatToScreen(double lon, double lat, ref Single screenX, ref Single screenY)
        {
            if (_projection.IsLonLatMap)
            {
                double LonShift = GetLonShift(lon);
                ProjToScreen(lon, lat, ref screenX, ref screenY, LonShift);
            }
            else
            {
                ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
                ProjectionInfo toProj = _projection.ProjInfo;
                double[][] points = new double[1][];
                points[0] = new double[] { lon, lat };
                try
                {
                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, 1);
                    double projX = points[0][0];
                    double projY = points[0][1];
                    ProjToScreen(projX, projY, ref screenX, ref screenY);
                }
                catch
                {
                    
                }
            }
        }

        /// <summary>
        /// convert coordinate from screen to map
        /// </summary>
        /// <param name="projX">projection X</param>
        /// <param name="projY">projection Y</param>
        /// <param name="screenX">screen X</param>
        /// <param name="screenY">screen Y</param>        
        public void ScreenToProj(ref double projX, ref double projY, double screenX, double screenY)
        {
            projX = screenX / _scaleX + _drawExtent.minX;
            projY = _drawExtent.maxY - screenY / _scaleY;
        }

        /// <summary>
        /// convert coordinate from screen to map
        /// </summary>
        /// <param name="projX">projection X</param>
        /// <param name="projY">projection Y</param>
        /// <param name="screenX">screen X</param>
        /// <param name="screenY">screen Y</param>    
        /// <param name="zoom">zoom</param>
        public void ScreenToProj(ref double projX, ref double projY, double screenX, double screenY, double zoom)
        {
            projX = screenX / _scaleX * zoom + _drawExtent.minX;
            projY = _drawExtent.maxY - screenY / _scaleY * zoom;
        }

        /// <summary>
        /// convert coordinate from screen to map
        /// </summary>
        /// <param name="projX">projection X</param>
        /// <param name="projY">projection Y</param>
        /// <param name="screenX">screen X</param>
        /// <param name="screenY">screen Y</param>        
        public void ScreenToProj(ref Single projX, ref Single projY, Single screenX, Single screenY)
        {
            projX = (Single)(screenX / _scaleX + _drawExtent.minX);
            projY = (Single)(_drawExtent.maxY - screenY / _scaleY);
        }

        /// <summary>
        /// convert coordinate from screen to map
        /// </summary>
        /// <param name="projX">projection X</param>
        /// <param name="projY">projection Y</param>
        /// <param name="screenX">screen X</param>
        /// <param name="screenY">screen Y</param>
        /// <param name="LonShift">Longitude shift</param>
        public void ScreenToProj(ref Single projX, ref Single projY, Single screenX, Single screenY, double LonShift)
        {
            projX = (Single)(screenX / _scaleX + _drawExtent.minX + LonShift);
            projY = (Single)(_drawExtent.maxY - screenY / _scaleY);
        }

        private void GetProjXYShift(ref double xShift, ref double yShift, Point point1, Point point2)
        {
            double pX1 = 0, pY1 = 0, pX2 = 0, pY2 = 0;
            ScreenToProj(ref pX1, ref pY1, point1.X, point1.Y);
            ScreenToProj(ref pX2, ref pY2, point2.X, point2.Y);
            xShift = pX2 - pX1;
            yShift = pY2 - pY1;
        }

        private void MoveShapeOnScreen(ref Shape.Shape aShape, Point point1, Point point2)
        {
            double xShift = 0, yShift = 0;
            GetProjXYShift(ref xShift, ref yShift, point1, point2);
            MoveShape(ref aShape, xShift, yShift);
        }

        private void MoveShape(ref Shape.Shape aShape, double xShift, double yShift)
        {
            List<PointD> points = aShape.GetPoints();
            for (int i = 0; i < points.Count; i++)
            {
                PointD aPoint = points[i];
                aPoint.X += xShift;
                aPoint.Y += yShift;
                points[i] = aPoint;
            }

            aShape.SetPoints(points);
        }

        private void ResizeShapeOnScreen(ref Shape.Shape aShape, ColorBreak legend, Rectangle newRect)
        {
            double xMin = 0, xMax = 0, yMin = 0, yMax = 0;
            ScreenToProj(ref xMin, ref yMin, newRect.Left, newRect.Bottom);
            ScreenToProj(ref xMax, ref yMax, newRect.Right, newRect.Top);
            Extent newExtent = new Extent(xMin, xMax, yMin, yMax);
            List<PointD> points = aShape.GetPoints();
            Extent aExtent = aShape.Extent;

            switch (aShape.ShapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes.PointM:
                    if (legend.GetType() == typeof(PointBreak))
                    {
                        PointBreak aPB = (PointBreak)legend;
                        aPB.Size = newRect.Width;
                    }
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.CurveLine:
                case ShapeTypes.Polygon:
                case ShapeTypes.PolygonM:
                case ShapeTypes.Circle:   
                case ShapeTypes.CurvePolygon:
                    MoveShape(ref aShape, newExtent.minX - aExtent.minX, newExtent.minY - aExtent.minY);
                    
                    double deltaX = newExtent.Width - aExtent.Width;
                    double deltaY = newExtent.Height - aExtent.Height;
                    for (int i = 0; i < points.Count; i++)
                    {
                        PointD aP = points[i];
                        aP.X = aP.X + deltaX * (aP.X - aExtent.minX) / aExtent.Width;
                        aP.Y = aP.Y + deltaY * (aP.Y - aExtent.minY) / aExtent.Height;
                        points[i] = aP;
                    }
                    aShape.SetPoints(points);
                    break;
                case ShapeTypes.Rectangle:
                case ShapeTypes.Ellipse:
                    points = new List<PointD>();
                    points.Add(new PointD(newExtent.minX, newExtent.minY));
                    points.Add(new PointD(newExtent.minX, newExtent.maxY));
                    points.Add(new PointD(newExtent.maxX, newExtent.maxY));
                    points.Add(new PointD(newExtent.maxX, newExtent.minY));
                    aShape.SetPoints(points);
                    break;
            }
        }

        /// <summary>
        /// Get longitude shift
        /// </summary>
        /// <param name="aExtent">extent</param>
        /// <returns>longitude shift</returns>
        public double GetLonShift(Extent aExtent)
        {
            double LonShift = 0;
            if (_drawExtent.maxX < aExtent.minX)
            {
                LonShift = -360;
            }
            if (_drawExtent.minX > aExtent.maxX)
            {
                LonShift = 360;
            }

            return LonShift;
        }

        /// <summary>
        /// Get longitude shift
        /// </summary>
        /// <param name="lon">Longitude</param>        
        /// <returns>longitude shift</returns>
        public double GetLonShift(double lon)
        {
            double LonShift = 0;
            if (_drawExtent.maxX < lon)
            {
                LonShift = -360;
            }
            if (_drawExtent.minX > lon)
            {
                LonShift = 360;
            }

            return LonShift;
        }        

        #endregion

        #region Zoom
        /// <summary>
        /// Refresh X/Y scale
        /// </summary>
        public void RefreshXYScale()
        {
            RefreshXYScale(this.Width, this.Height);
        }

        /// <summary>
        /// Refresh X/Y scale
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        public void RefreshXYScale(int width, int height)
        {
            Extent aExtent = _viewExtent;            

            if (_isGeoMap)
            {
                SetCoordinateGeoMap(ref aExtent, width, height);
            }
            else
            {
                SetCoordinateMap(ref aExtent, width, height);
            }

            _drawExtent = aExtent;
        }

        /// <summary>
        /// Zoom to extent by screen coordinate
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        public void ZoomToExtentScreen(double minX, double maxX, double minY, double maxY)
        {
            double pMinX = 0, pMinY = 0, pMaxX = 0, pMaxY = 0;
            ScreenToProj(ref pMinX, ref pMinY, minX, maxY);
            ScreenToProj(ref pMaxX, ref pMaxY, maxX, minY);
            ZoomToExtent(pMinX, pMaxX, pMinY, pMaxY);
        }

        /// <summary>
        /// Zoom to extent by screen coordinate
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <param name="zoom">zoom</param>
        public void ZoomToExtentScreen(double minX, double maxX, double minY, double maxY, double zoom)
        {
            double pMinX = 0, pMinY = 0, pMaxX = 0, pMaxY = 0;
            ScreenToProj(ref pMinX, ref pMinY, minX, maxY, zoom);
            ScreenToProj(ref pMaxX, ref pMaxY, maxX, minY, zoom);
            ZoomToExtent(pMinX, pMaxX, pMinY, pMaxY);
        }

        /// <summary>
        /// Zoom to extent
        /// </summary>
        /// <param name="aExtent">extent</param>
        public void ZoomToExtent(Extent aExtent)
        {
            _viewExtent = aExtent;
            RefreshXYScale();

            //if (_isGeoMap)
            //{
            //    SetCoordinateGeoMap(ref aExtent);
            //}
            //else
            //{
            //    SetCoordinateMap(ref aExtent);
            //}

            //_drawExtent = aExtent;

            PaintLayers();

            OnViewExtentChanged();
        }

        /// <summary>
        /// Zoom to lon/lat extent
        /// </summary>
        /// <param name="minX">mininum X</param>
        /// <param name="maxX">maxinum X</param>
        /// <param name="minY">mininum Y</param>
        /// <param name="maxY">maxinum Y</param>
        public void ZoomToExtentLonLat(double minX, double maxX, double minY, double maxY)
        {
            Extent aExtent = new Extent();
            aExtent.minX = minX;
            aExtent.maxX = maxX;
            aExtent.minY = minY;
            aExtent.maxY = maxY;

            ZoomToExtentLonLat(aExtent);
        }

        /// <summary>
        /// Zoom to lon/lat extent
        /// </summary>
        /// <param name="aExtent">extent</param>
        public void ZoomToExtentLonLat(Extent aExtent)
        {
            if (!_projection.IsLonLatMap)
            {
                aExtent = _projection.GetProjectedExtentFromLonLat(aExtent);
            }

            _viewExtent = aExtent;

            if (_isGeoMap)
            {
                SetCoordinateGeoMap(ref aExtent);
            }
            else
            {
                SetCoordinateMap(ref aExtent);
            }

            _drawExtent = aExtent;

            PaintLayers();

            OnViewExtentChanged();
        }

        /// <summary>
        /// Zoom to exactly extent
        /// </summary>
        /// <param name="aExtent">extent</param>
        public void ZoomToExtentEx(Extent aExtent)
        {
            _viewExtent = aExtent;

            if (_isGeoMap)
            {
                SetCoordinateGeoMapEx(ref aExtent);
            }
            else
            {
                SetCoordinateMap(ref aExtent);
            }

            _drawExtent = aExtent;

            PaintLayers();

            OnViewExtentChanged();
        }

        /// <summary>
        /// Zoom to exactly lon/lat extent
        /// </summary>
        /// <param name="aExtent">extent</param>
        public void ZoomToExtentLonLatEx(Extent aExtent)
        {
            if (!_projection.IsLonLatMap)
            {
                aExtent = _projection.GetProjectedExtentFromLonLat(aExtent);
            }

            _viewExtent = aExtent;

            if (_isGeoMap)
            {
                SetCoordinateGeoMapEx(ref aExtent);
            }
            else
            {
                SetCoordinateMap(ref aExtent);
            }

            _drawExtent = aExtent;

            PaintLayers();

            OnViewExtentChanged();
        }

        private void SetCoordinateGeoMap(ref Extent aExtent)
        {
            SetCoordinateGeoMap(ref aExtent, this.Width, this.Height);            
        }

        private void SetCoordinateGeoMap(ref Extent aExtent, int width, int height)
        {
            double scaleFactor, lonRan, latRan, temp;

            _scaleX = width / (aExtent.maxX - aExtent.minX);
            _scaleY = height / (aExtent.maxY - aExtent.minY);
            if (_projection.ProjInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat)
            {
                scaleFactor = _XYScaleFactor;
            }
            else
            {
                scaleFactor = 1;
            }

            if (_scaleX > _scaleY)
            {
                _scaleX = _scaleY / scaleFactor;
                temp = aExtent.minX;
                aExtent.minX = aExtent.maxX - width / _scaleX;
                lonRan = (aExtent.minX - temp) / 2;
                aExtent.minX = aExtent.minX - lonRan;
                aExtent.maxX = aExtent.maxX - lonRan;
            }
            else
            {
                _scaleY = _scaleX * scaleFactor;
                temp = aExtent.minY;
                aExtent.minY = aExtent.maxY - height / _scaleY;
                latRan = (aExtent.minY - temp) / 2;
                aExtent.minY = aExtent.minY - latRan;
                aExtent.maxY = aExtent.maxY - latRan;
            }
        }

        private void SetCoordinateGeoMapEx(ref Extent aExtent)
        {
            SetCoordinateGeoMapEx(ref aExtent, this.Width, this.Height);            
        }

        private void SetCoordinateGeoMapEx(ref Extent aExtent, int width, int height)
        {
            double scaleFactor;

            _scaleX = width / (aExtent.maxX - aExtent.minX);
            _scaleY = height / (aExtent.maxY - aExtent.minY);
            if (_projection.ProjInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat)
            {
                scaleFactor = _XYScaleFactor;
            }
            else
            {
                scaleFactor = 1;
            }

            if (_scaleX < _scaleY)
            {
                _scaleX = _scaleY / scaleFactor;
                width = (int)((aExtent.maxX - aExtent.minX) * _scaleX);
            }
            else
            {
                _scaleY = _scaleX * scaleFactor;
                height = (int)((aExtent.maxY - aExtent.minY) * _scaleY);
            }
        }

        private void SetCoordinateMap(ref Extent aExtent)
        {
            SetCoordinateMap(ref aExtent, this.Width, this.Height);            
        }

        private void SetCoordinateMap(ref Extent aExtent, int width, int height)
        {
            _scaleX = width / (aExtent.maxX - aExtent.minX);
            _scaleY = height / (aExtent.maxY - aExtent.minY);
        }

        /// <summary>
        /// Zoom to extent
        /// </summary>
        /// <param name="minX">mininum X</param>
        /// <param name="maxX">maxinum X</param>
        /// <param name="minY">mininum Y</param>
        /// <param name="maxY">maxinum Y</param>
        public void ZoomToExtent(double minX, double maxX, double minY, double maxY)
        {
            Extent aExtent = new Extent();
            aExtent.minX = minX;
            aExtent.maxX = maxX;
            aExtent.minY = minY;
            aExtent.maxY = maxY;

            ZoomToExtent(aExtent);
        }        

        private double GetGeoWidth(double width)
        {
            double geoWidth = width / _scaleX;
            if (_projection.IsLonLatMap)
                geoWidth = geoWidth * GetLonDistScale();

            return geoWidth;
        }

        private double GetWidth(double geoWidth)
        {
            double width = geoWidth * _scaleX;
            if (_projection.IsLonLatMap)
                width = width / GetLonDistScale();

            return width;
        }

        private double GetLonDistScale()
        {
            //Get meters of one longitude degree
            double pY = (_viewExtent.maxY + _viewExtent.minY) / 2;
            double ProjX = 0, ProjY = pY, pProjX = 1, pProjY = pY;
            double dx = Math.Abs(ProjX - pProjX);
            double dy = Math.Abs(ProjY - pProjY);
            double dist;
            double y = (ProjY + pProjY) / 2;
            double factor = Math.Cos(y * Math.PI / 180);
            dx *= factor;
            dist = Math.Sqrt(dx * dx + dy * dy);
            dist = dist * 111319.5;

            return dist;
        }

        /// <summary>
        /// Get geographic scale
        /// </summary>
        /// <returns>Geographic scale</returns>
        public double GetGeoScale()
        {
            double breakWidth = 1;
            double geoBreakWidth = GetGeoWidth(breakWidth);
            double scale = geoBreakWidth * 100 / (breakWidth / this.CreateGraphics().DpiX * 2.539999918);

            return scale;
        }

        /// <summary>
        /// Get View center point
        /// </summary>
        /// <returns>The view center point</returns>
        public PointD getViewCenter()
        {
            return _viewExtent.GetCenterPoint();
        }

        /// <summary>
        /// Set view center point
        /// </summary>
        /// <param name="center">The view center point</param>
        public void setViewCenter(PointD center)
        {
            PointD oldCenter = this.getViewCenter();
            double dx = center.X - oldCenter.X;
            double dy = center.Y - oldCenter.Y;
            Extent extent = _viewExtent.Shift(dx, dy);
            this.ZoomToExtent(extent);
        }

        #endregion Zoom

        #region Layer
        /// <summary>
        /// Open a layer from layer file
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>map layer</returns>
        public MapLayer OpenLayer(string aFile)
        {
            MapLayer aLayer = MapDataManage.OpenLayer(aFile);
            AddLayer(aLayer);

            return aLayer;
        }

        /// <summary>
        /// Get layer from handle
        /// </summary>
        /// <param name="handle">layer handle</param>
        /// <returns>layer object</returns>
        public MapLayer GetLayerFromHandle(int handle)
        {
            MapLayer aLayer = null;
            for (int i = 0; i < _layerSet.LayerNum; i++)
            {
                if (_layerSet.Layers[i].Handle == handle)
                {
                    aLayer = _layerSet.Layers[i];
                    break;
                }
            }

            return aLayer;
        }

        ///// <summary>
        ///// Get Lon/lat layer from handle
        ///// </summary>
        ///// <param name="handle">layer handle</param>
        ///// <returns>layer object</returns>
        //public MapLayer GetGeoLayerFromHandle(int handle)
        //{
        //    MapLayer aLayer = null;
        //    for (int i = 0; i < _layerSet.GeoLayers.Count; i++)
        //    {
        //        if (_layerSet.GeoLayers[i].Handle == handle)
        //        {
        //            aLayer = _layerSet.GeoLayers[i];
        //            break;
        //        }
        //    }

        //    return aLayer;            
        //}

        /// <summary>
        /// Get layer visible
        /// </summary>
        /// <param name="handle">layer handle</param>
        /// <returns>visible</returns>
        public bool GetLayerVisible(int handle)
        {
            object aLayer = GetLayerFromHandle(handle);
            if (aLayer.GetType() == typeof(VectorLayer))
            {
                return ((VectorLayer)aLayer).Visible;
            }
            else
            {
                return ((ImageLayer)aLayer).Visible;
            }
        }

        /// <summary>
        /// Get layer name
        /// </summary>
        /// <param name="handle">layer handle</param>
        /// <returns>layer name</returns>
        public string GetLayerName(int handle)
        {
            Layer.MapLayer aLayer = GetLayerFromHandle(handle);
            return aLayer.LayerName;
        }

        /// <summary>
        /// Get polygon layer names
        /// </summary>
        /// <returns>polygon layer names</returns>
        public List<string> GetPolygonLayerNames()
        {
            List<string> aList = new List<string>();
            for (int i = 0; i < _layerSet.Layers.Count; i++)
            {
                if (_layerSet.Layers[i].LayerType == LayerTypes.VectorLayer)
                {
                    VectorLayer aLayer = (VectorLayer)_layerSet.Layers[i];
                    if (aLayer.LayerDrawType == LayerDrawType.Map)
                    {
                        switch (aLayer.ShapeType)
                        {
                            case ShapeTypes.Polygon:
                            case ShapeTypes.PolygonM:
                                if (aLayer.ShapeNum < 1000)
                                {
                                    aList.Add(aLayer.LayerName);
                                }
                                break;
                        }
                    }
                }
            }

            return aList;
        }
        
        # region Layer Position Operation

        /// <summary>
        /// Get layers whole extent
        /// </summary>
        /// <returns></returns>
        public Extent GetLayersWholeExtent()
        {
            Extent aExtent = new Extent();
            Extent bExtent = new Extent();            
            for (int i = 0; i < _layerSet.Layers.Count; i++)
            {
                bExtent = _layerSet.Layers[i].Extent;                
                if (i == 0)
                {
                    aExtent = bExtent;
                }
                else
                {
                    aExtent = MIMath.GetLagerExtent(aExtent, bExtent);
                }
            }

            return aExtent;
        }

        /// <summary>
        /// Get new layer handle
        /// </summary>
        /// <returns></returns>
        public int GetNewLayerHandle()
        {
            int handle = 0;
            for (int i = 0; i < _layerSet.Layers.Count; i++)
            {
                if (handle < _layerSet.Layers[i].Handle)
                    handle = _layerSet.Layers[i].Handle;                
            }
            handle += 1;

            return handle;
        }

        /// <summary>
        /// Get layer handle from layer name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetLayerHandleFromName(string name)
        {
            int handle = -1;
            for (int i = 0; i < _layerSet.Layers.Count; i++)
            {
                if (_layerSet.Layers[i].LayerName == name)
                {
                    handle = _layerSet.Layers[i].Handle;
                    break;
                }                
            }

            return handle;
        }

        /// <summary>
        /// Get layer from name
        /// </summary>
        /// <param name="name">layer name</param>
        /// <returns>map layer</returns>
        public MapLayer GetLayerFromName(string name)
        {
            MapLayer aLayer = null;
            foreach (MapLayer ml in _layerSet.Layers)
            {
                if (ml.LayerName == name)
                {
                    aLayer = ml;
                    break;
                }
            }

            return aLayer;
        }

        /// <summary>
        /// Get layer from name
        /// </summary>
        /// <param name="name">layer name</param>
        /// <returns>map layer</returns>
        public MapLayer GetLayer(string name)
        {
            MapLayer aLayer = null;
            foreach (MapLayer ml in _layerSet.Layers)
            {
                if (ml.LayerName == name)
                {
                    aLayer = ml;
                    break;
                }
            }

            return aLayer;
        }

        /// <summary>
        /// Get layer handle from layer index
        /// </summary>
        /// <param name="lIdx"></param>
        /// <returns></returns>
        public int GetLayerHandleFromIdx(int lIdx)
        {          
            return _layerSet.Layers[lIdx].Handle;
        }

        ///// <summary>
        ///// Get layer from layer handle
        ///// </summary>
        ///// <param name="handle"></param>
        ///// <returns></returns>
        //public object GetLayerFromHandle(int handle)
        //{
        //    object aLayer = new object();
        //    for (int i = 0; i < _layerSet.LayerNum; i++)
        //    {
        //        if (_layerSet.Layers[i].GetType() == typeof(VectorLayer))
        //        {
        //            aLayer = (VectorLayer)_layerSet.Layers[i];
        //            if (((VectorLayer)aLayer).Handle == handle)
        //            {
        //                break;
        //            }
        //        }
        //        else
        //        {
        //            aLayer = (ImageLayer)_layerSet.Layers[i];
        //            if (((ImageLayer)aLayer).Handle == handle)
        //            {
        //                break;
        //            }
        //        }
        //    }

        //    return aLayer;
        //}

        /// <summary>
        /// Get layer index from layer handle
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public int GetLayerIdxFromHandle(int handle)
        {
            int lIdx = -1;
            object aLayer = new object();
            for (int i = 0; i < _layerSet.LayerNum; i++)
            {
                if (_layerSet.Layers[i].Handle == handle)
                {
                    lIdx = i;
                    break;
                }                
            }

            return lIdx;
        }

        /// <summary>
        /// Add layer
        /// </summary>
        /// <param name="aLayer">layer object</param>        
        /// <returns>Layer handle</returns>
        public int AddLayer(MapLayer aLayer)
        {
            switch (aLayer.LayerType)
            {
                case LayerTypes.VectorLayer:
                    return AddVectorLayer((VectorLayer)aLayer);
                case LayerTypes.RasterLayer:
                    return AddRasterLayer((RasterLayer)aLayer);
                case LayerTypes.ImageLayer:
                    return AddImageLayer(aLayer);
                default:
                    return -1;
            }
        }

        ///// <summary>
        ///// Add vector layer
        ///// </summary>
        ///// <param name="aLayer">Vector layer</param>        
        ///// <returns>layer handle</returns>
        //public int AddVectorLayer_old(VectorLayer aLayer)
        //{
        //    int lIdx, handle;
        //    handle = GetNewLayerHandle();
        //    aLayer.Handle = handle;
        //    ProjectionInfo aProjInfo = aLayer.ProjInfo;
        //    bool projectLabels = true;
        //    if (aLayer.LabelPoints.Count > 0)
        //        projectLabels = false;

        //    if (aProjInfo.ToProj4String() == Projection.ProjInfo.ToProj4String())
        //    {
        //        if (!Projection.IsLonLatMap)
        //        {
        //            VectorLayer gLayer = Projection.ProjectLayer(aLayer, aProjInfo,
        //                KnownCoordinateSystems.Geographic.World.WGS1984, projectLabels);
        //            _layerSet.GeoLayers.Add(gLayer);
        //        }
        //        _layerSet.Layers.Add(aLayer);
        //    }
        //    else
        //    {
        //        if (aProjInfo.Transform.Proj4Name == "lonlat")
        //        {
        //            _layerSet.GeoLayers.Add(aLayer);
        //        }
        //        else
        //        {
        //            VectorLayer gLayer = Projection.ProjectLayer(aLayer, aProjInfo,
        //                KnownCoordinateSystems.Geographic.World.WGS1984, projectLabels);
        //            _layerSet.GeoLayers.Add(gLayer);
        //        }
        //        VectorLayer pLayer = this.Projection.ProjectLayer(aLayer, aProjInfo, this.Projection.ProjInfo, projectLabels);
        //        _layerSet.Layers.Add(pLayer);
        //    }

        //    lIdx = _layerSet.LayerNum - 1;

        //    _extent = GetLayersWholeExtent();

        //    this.PaintLayers();
        //    OnLayersUpdated();

        //    return handle;
        //}

        /// <summary>
        /// Add vector layer
        /// </summary>
        /// <param name="aLayer">Vector layer</param>        
        /// <returns>layer handle</returns>
        public int AddVectorLayer(VectorLayer aLayer)
        {
            int handle = GetNewLayerHandle();
            aLayer.Handle = handle;
            bool projectLabels = true;
            if (aLayer.LabelPoints.Count > 0)
                projectLabels = false;

            if (aLayer.ProjInfo.ToProj4String() != Projection.ProjInfo.ToProj4String())
                this.Projection.ProjectLayer(aLayer, this.Projection.ProjInfo, projectLabels);

            _layerSet.Layers.Add(aLayer);
            _extent = GetLayersWholeExtent();

            this.PaintLayers();
            OnLayersUpdated();

            return handle;
        }

        ///// <summary>
        ///// Add vector layer
        ///// </summary>
        ///// <param name="aLayer">Vector layer</param>        
        ///// <param name="EarthWind">if wind relative to earth</param>
        ///// <returns>layer handle</returns>
        //public int AddWindLayer(VectorLayer aLayer, bool EarthWind)
        //{
        //    int lIdx, handle;
        //    handle = GetNewLayerHandle();
        //    aLayer.Handle = handle;
        //    ProjectionInfo aProjInfo = aLayer.ProjInfo;
        //    ProjectionInfo GeoProjInfo = KnownCoordinateSystems.Geographic.World.WGS1984;

        //    if (aProjInfo.ToProj4String() == Projection.ProjInfo.ToProj4String())
        //    {
        //        if (!Projection.IsLonLatMap)
        //        {
        //            VectorLayer gLayer = new VectorLayer(aLayer.ShapeType);
        //            if (EarthWind)
        //                gLayer = Projection.ProjectWindLayer(aLayer, aProjInfo,
        //                    GeoProjInfo, false);
        //            else
        //                gLayer = Projection.ProjectLayer(aLayer, aProjInfo,
        //                    GeoProjInfo);

        //            _layerSet.GeoLayers.Add(gLayer);
        //        }
        //        _layerSet.Layers.Add(aLayer);

        //        //if (EarthWind)
        //        //{
        //        //    VectorLayer p1Layer = Projection.ProjectLayerAngle_Proj4(aLayer, GeoProjInfo, Projection.ProjInfo);
        //        //    _layerSet.Layers.Add(p1Layer);
        //        //}
        //        //else
        //        //    _layerSet.Layers.Add(aLayer);
        //    }
        //    else
        //    {
        //        if (aProjInfo.Transform.Proj4Name == "lonlat")
        //        {
        //            if (EarthWind)
        //                _layerSet.GeoLayers.Add(aLayer);
        //            else
        //            {
        //                VectorLayer gLayer = Projection.ProjectLayerAngle(aLayer, aProjInfo, GeoProjInfo);
        //                _layerSet.GeoLayers.Add(gLayer);
        //            }
        //        }
        //        else
        //        {
        //            VectorLayer gLayer = new VectorLayer(aLayer.ShapeType);
        //            if (EarthWind)
        //                gLayer = Projection.ProjectWindLayer(aLayer, aProjInfo, GeoProjInfo, false);
        //            else
        //                gLayer = Projection.ProjectLayer(aLayer, aProjInfo, GeoProjInfo);

        //            _layerSet.GeoLayers.Add(gLayer);
        //        }

        //        VectorLayer pLayer = new VectorLayer(aLayer.ShapeType);
        //        if (EarthWind)
        //        {
        //            if (aProjInfo.Transform.Proj4Name == "lonlat")
        //                pLayer = this.Projection.ProjectLayer(aLayer, aProjInfo, this.Projection.ProjInfo);
        //            else
        //            {
        //                pLayer = this.Projection.ProjectWindLayer(aLayer, aProjInfo, this.Projection.ProjInfo, false);
        //                pLayer = Projection.ProjectLayerAngle(pLayer, GeoProjInfo, Projection.ProjInfo);
        //            }
        //        }
        //        else
        //            pLayer = this.Projection.ProjectLayer(aLayer, aProjInfo, this.Projection.ProjInfo);

        //        _layerSet.Layers.Add(pLayer);
        //    }

        //    lIdx = _layerSet.LayerNum - 1;

        //    _extent = GetLayersWholeExtent();

        //    this.PaintLayers();

        //    OnLayersUpdated();

        //    return handle;
        //}

        /// <summary>
        /// Add vector layer
        /// </summary>
        /// <param name="aLayer">Vector layer</param>        
        /// <param name="EarthWind">if wind relative to earth</param>
        /// <returns>layer handle</returns>
        public int AddWindLayer(VectorLayer aLayer, bool EarthWind)
        {
            int lIdx, handle;
            handle = GetNewLayerHandle();
            aLayer.Handle = handle;
            ProjectionInfo aProjInfo = aLayer.ProjInfo;
            ProjectionInfo GeoProjInfo = KnownCoordinateSystems.Geographic.World.WGS1984;

            if (aProjInfo.ToProj4String() != Projection.ProjInfo.ToProj4String())
            {                
                if (EarthWind)
                {
                    if (aProjInfo.Transform.Proj4Name == "lonlat")
                        this.Projection.ProjectLayer(aLayer, this.Projection.ProjInfo);
                    else
                    {
                        this.Projection.ProjectWindLayer(aLayer, this.Projection.ProjInfo, false);
                        Projection.ProjectLayerAngle(aLayer, GeoProjInfo, Projection.ProjInfo);
                    }
                }
                else
                    this.Projection.ProjectLayer(aLayer, this.Projection.ProjInfo);

                
            }
            _layerSet.Layers.Add(aLayer);

            lIdx = _layerSet.LayerNum - 1;

            _extent = GetLayersWholeExtent();

            this.PaintLayers();

            OnLayersUpdated();

            return handle;
        }

        /// <summary>
        /// Add raster layer
        /// </summary>
        /// <param name="aLayer">raster layer</param>
        /// <returns>layer handle</returns>
        public int AddRasterLayer(RasterLayer aLayer)
        {
            int handle = GetNewLayerHandle();
            aLayer.Handle = handle;

            if (aLayer.ProjInfo.ToProj4String() != Projection.ProjInfo.ToProj4String())
                this.Projection.ProjectLayer(aLayer, Projection.ProjInfo);

            _layerSet.Layers.Add(aLayer);
            _extent = GetLayersWholeExtent();

            this.PaintLayers();
            OnLayersUpdated();

            return handle;
        }

        /// <summary>
        /// Add image layer
        /// </summary>
        /// <param name="aLayer">Image layer</param>
        /// <returns>layer handle</returns>
        public int AddImageLayer(MapLayer aLayer)
        {
            int lIdx, handle;
            handle = GetNewLayerHandle();
            aLayer.Handle = handle;
            _layerSet.Layers.Add(aLayer);
            lIdx = _layerSet.LayerNum - 1;
            _extent = GetLayersWholeExtent();
            //if (!this.Projection.IsLonLatMap)
            //{
            //    _layerSet.GeoLayers.Add(aLayer);
            //}
                        
            this.PaintLayers();
            OnLayersUpdated();

            return handle;
        }

        /// <summary>
        /// Insert polygon layer
        /// </summary>
        /// <param name="aLayer">vector layer</param>        
        /// <returns>layer handle</returns>
        public int InsertPolygonLayer(VectorLayer aLayer)
        {
            LockViewUpdate = true;
            int lIdx = GetPolygonLayerIdx() + 1;
            int handle = AddLayer(aLayer);
            if (lIdx < 0)
            {
                lIdx = 0;
            }
            int pIdx = GetLayerIdxFromHandle(handle);
            MoveLayer(pIdx, lIdx);
            LockViewUpdate = false;
            PaintLayers();

            return handle;
        }

        ///// <summary>
        ///// Insert layer
        ///// </summary>
        ///// <param name="aLayer"></param>
        ///// <param name="aIdx"></param>
        ///// <param name="isDataLonLat"></param>
        ///// <returns></returns>
        //public int InsertLayer(VectorLayer aLayer, int aIdx, bool isDataLonLat)
        //{
        //    int lIdx, handle;
        //    handle = GetNewLayerHandle();
        //    aLayer.Handle = handle;
        //    lIdx = aIdx;
        //    ProjectionInfo geoProj = KnownCoordinateSystems.Geographic.World.WGS1984;
        //    ProjectionInfo theProj = this.Projection.ProjInfo;
        //    if (this.Projection.IsLonLatMap)
        //    {
        //        if (isDataLonLat)
        //        {
        //            _layerSet.Layers.Insert(aIdx, aLayer);
        //        }
        //        else
        //        {
        //            VectorLayer pLayer = this.Projection.ProjectLayer(aLayer, theProj, geoProj);
        //            _layerSet.Layers.Insert(aIdx, pLayer);
        //        }
        //    }
        //    else
        //    {
        //        if (isDataLonLat)
        //        {
        //            _layerSet.GeoLayers.Insert(aIdx, aLayer);
        //            VectorLayer pLayer = this.Projection.ProjectLayer(aLayer, geoProj, theProj);
        //            _layerSet.Layers.Insert(aIdx, pLayer);
        //        }
        //        else
        //        {
        //            _layerSet.Layers.Insert(aIdx, aLayer);
        //            VectorLayer pLayer = this.Projection.ProjectLayer(aLayer, theProj, geoProj);
        //            _layerSet.GeoLayers.Insert(aIdx, pLayer);
        //        }
        //    }
        //    _extent = GetLayersWholeExtent();
                     
        //    this.PaintLayers();

        //    return handle;
        //}

        ///// <summary>
        ///// Insert image layer
        ///// </summary>
        ///// <param name="aLayer"></param>
        ///// <param name="aIdx"></param>
        ///// <returns></returns>
        //public int InsertImageLayer(MapLayer aLayer, int aIdx)
        //{
        //    int lIdx, handle;
        //    handle = GetNewLayerHandle();
        //    aLayer.Handle = handle;
        //    _layerSet.Layers.Insert(aIdx, aLayer);
        //    lIdx = aIdx;
        //    _extent = GetLayersWholeExtent();
                                 
        //    this.PaintLayers();

        //    return handle;
        //}

        /// <summary>
        /// Move layer position
        /// </summary>
        /// <param name="lPreIdx">previous index</param>
        /// <param name="lNewIdx">new index</param>
        public void MoveLayer(int lPreIdx, int lNewIdx)
        {

            if (lNewIdx > lPreIdx)
            {
                if (lNewIdx == _layerSet.LayerNum - 1)
                {
                    _layerSet.Layers.Add(_layerSet.Layers[lPreIdx]);
                }
                else
                {
                    if (lPreIdx == 0)
                    {
                        _layerSet.Layers.Insert(lNewIdx + 1, _layerSet.Layers[lPreIdx]);
                    }
                    else
                    {
                        _layerSet.Layers.Insert(lNewIdx + 1, _layerSet.Layers[lPreIdx]);
                    }
                }
                _layerSet.Layers.RemoveAt(lPreIdx);
            }
            else
            {
                _layerSet.Layers.Insert(lNewIdx, _layerSet.Layers[lPreIdx]);
                _layerSet.Layers.RemoveAt(lPreIdx + 1);
            }
        }

        /// <summary>
        /// Move layer
        /// </summary>
        /// <param name="layer">The layer</param>
        /// <param name="lNewIdx">New layer index</param>
        public void MoveLayer(MapLayer layer, int lNewIdx)
        {
            int lPreIdx = this.GetLayerIdxFromHandle(layer.Handle);
            MoveLayer(lPreIdx, lNewIdx);
        }

        /// <summary>
        /// Move layer to top
        /// </summary>
        /// <param name="layer">The layer</param>
        public void MoveLayerToTop(MapLayer layer)
        {
            MoveLayer(layer, this._layerSet.LayerNum - 1);
        }

        /// <summary>
        /// Move layer to bottom
        /// </summary>
        /// <param name="layer">The layer</param>
        public void MoveLayerToBottom(MapLayer layer)
        {
            MoveLayer(layer, 0);
        }

        ///// <summary>
        ///// Move layer position
        ///// </summary>
        ///// <param name="lPreIdx">previous index</param>
        ///// <param name="lNewIdx">new index</param>
        //public void MoveLayer_old(int lPreIdx, int lNewIdx)
        //{
            
        //    if (lNewIdx > lPreIdx)
        //    {
        //        if (lNewIdx == _layerSet.LayerNum - 1)
        //        {
        //            _layerSet.Layers.Add(_layerSet.Layers[lPreIdx]);
        //            if (!this.Projection.IsLonLatMap)
        //                _layerSet.GeoLayers.Add(_layerSet.GeoLayers[lPreIdx]);
        //        }
        //        else
        //        {
        //            if (lPreIdx == 0)
        //            {
        //                _layerSet.Layers.Insert(lNewIdx + 1, _layerSet.Layers[lPreIdx]);
        //                if (!this.Projection.IsLonLatMap)
        //                {
        //                    _layerSet.GeoLayers.Insert(lNewIdx + 1, _layerSet.GeoLayers[lPreIdx]);
        //                }
        //            }
        //            else
        //            {
        //                _layerSet.Layers.Insert(lNewIdx + 1, _layerSet.Layers[lPreIdx]);
        //                if (!this.Projection.IsLonLatMap)
        //                {
        //                    _layerSet.GeoLayers.Insert(lNewIdx + 1, _layerSet.GeoLayers[lPreIdx]);
        //                }
        //            }
        //        }
        //        _layerSet.Layers.RemoveAt(lPreIdx);
        //        if (!this.Projection.IsLonLatMap)
        //        {
        //            _layerSet.GeoLayers.RemoveAt(lPreIdx);
        //        }
        //    }
        //    else
        //    {
        //        _layerSet.Layers.Insert(lNewIdx, _layerSet.Layers[lPreIdx]);
        //        _layerSet.Layers.RemoveAt(lPreIdx + 1);
        //        if (!this.Projection.IsLonLatMap)
        //        {
        //            _layerSet.GeoLayers.Insert(lNewIdx, _layerSet.GeoLayers[lPreIdx]);
        //            _layerSet.GeoLayers.RemoveAt(lPreIdx + 1);
        //        }
        //    }            
        //}        

        /// <summary>
        /// Remove layer by handle
        /// </summary>
        /// <param name="handle">handle</param>        
        public void RemoveLayerHandle(int handle)
        {
            int lIdx = GetLayerIdxFromHandle(handle);
            if (lIdx >= 0)
            {
                RemoveLayer(lIdx);
            }
        }

        /// <summary>
        /// Remove layer by index
        /// </summary>
        /// <param name="aIdx"></param>        
        public void RemoveLayer(int aIdx)
        {
            MapLayer aLayer = _layerSet.Layers[aIdx];
            _layerSet.Layers.RemoveAt(aIdx);

            if (aLayer.LayerType == LayerTypes.VectorLayer)
                ((VectorLayer)aLayer).Dispose();

            _extent = GetLayersWholeExtent();                   
        }

        /// <summary>
        /// Remove layer object
        /// </summary>
        /// <param name="aLayer"></param>
        public void RemoveLayer(MapLayer aLayer)
        {
            int aIdx = GetLayerIdxFromHandle(aLayer.Handle);
            RemoveLayer(aIdx);
        }

        /// <summary>
        /// Remove meteorological data layers
        /// </summary>
        public void RemoveMeteoLayers()
        {            
            for (int i = 0; i < _layerSet.Layers.Count; i++)
            {
                if (i == _layerSet.Layers.Count)
                {
                    break;
                }
                if (_layerSet.Layers[i].FileName == String.Empty)
                {
                    RemoveLayer(_layerSet.Layers[i]);
                    i -= 1;
                }
            }

        }

        /// <summary>
        /// Remove all layers
        /// </summary>
        public void RemoveAllLayers()
        {
            int aNum = _layerSet.Layers.Count;
            for (int i = 0; i < aNum; i++)
            {
                RemoveLayer(0);
            }
            this.Refresh();
            this.PaintLayers();
        }

        /// <summary>
        /// Remove map layers
        /// </summary>
        public void RemoveMapLayers()
        {            
            for (int i = 0; i < _layerSet.Layers.Count; i++)
            {
                if (i == _layerSet.Layers.Count)
                {
                    break;
                }
                MapLayer aLayer = _layerSet.Layers[i];
                if (aLayer.LayerDrawType == LayerDrawType.Map)
                {
                    RemoveLayer(aLayer);
                    i -= 1;
                }
            }

        }        

        /// <summary>
        /// Get last polyline layer index
        /// </summary>        
        /// <returns>layer index</returns>
        public int GetLineLayerIdx()
        {
            VectorLayer bLayer;
            int lIdx = -1;                        
            for (int i = _layerSet.LayerNum - 1; i >= 0; i--)
            {
                if (_layerSet.Layers[i].GetType() == typeof(VectorLayer))
                {
                    bLayer = (VectorLayer)_layerSet.Layers[i];
                    switch (bLayer.ShapeType)
                    {
                        case ShapeTypes.Polyline:
                        case ShapeTypes.PolylineM:
                        case ShapeTypes.PolylineZ:
                        case ShapeTypes.Polygon:
                        case ShapeTypes.PolygonM:
                            lIdx = i;
                            break;
                    }

                    if (lIdx > -1)
                        break;
                }
                else
                {
                    lIdx = i;
                    break;
                }
            }            

            return lIdx;
        }

        /// <summary>
        /// Get last polygon layer index
        /// </summary>        
        /// <returns>layer index</returns>
        public int GetPolygonLayerIdx()
        {
            VectorLayer bLayer;
            int lIdx = -1;
            for (int i = _layerSet.LayerNum - 1; i >= 0; i--)
            {
                if (_layerSet.Layers[i].GetType() == typeof(VectorLayer))
                {
                    bLayer = (VectorLayer)_layerSet.Layers[i];
                    switch (bLayer.ShapeType)
                    {
                        case ShapeTypes.Polygon:
                        case ShapeTypes.PolygonM:
                            lIdx = i;
                            break;
                    }

                    if (lIdx > -1)
                        break;
                }
                else
                {
                    lIdx = i;
                    break;
                }
            }

            return lIdx;
        }

        /// <summary>
        /// Get last image layer index
        /// </summary>       
        /// <returns>layer index</returns>
        public int GetImageLayerIdx()
        {           
            int lIdx = -1;
            for (int i = _layerSet.LayerNum - 1; i >= 0; i--)
            {
                if (_layerSet.Layers[i].LayerType == LayerTypes.ImageLayer ||
                    _layerSet.Layers[i].LayerType == LayerTypes.RasterLayer)
                {
                    lIdx = i;
                    break;                    
                }
            }

            return lIdx;
        }  

        # endregion Layer Operation

        # region Layer Property Edit

        /// <summary>
        /// Set layer legend scheme
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="aLS"></param>
        public void SetLayerLegendScheme(int handle, LegendScheme aLS)
        {
            int lIdx = GetLayerIdxFromHandle(handle);
            ((VectorLayer)_layerSet.Layers[lIdx]).LegendScheme = aLS;
            this.PaintLayers();
        }

        /// <summary>
        /// Set projected layer legend scheme
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="aLS"></param>
        public void SetLayerLegendSchemeProj(int handle, LegendScheme aLS)
        {
            int lIdx = GetLayerIdxFromHandle(handle);
            ((VectorLayer)_layerSet.Layers[lIdx]).LegendScheme = aLS;
            //if (!this.Projection.IsLonLatMap)
            //{
            //    ((VectorLayer)_layerSet.GeoLayers[lIdx]).LegendScheme = aLS;
            //}
            this.PaintLayers();
        }

        /// <summary>
        /// Set layer name
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        public void SetLayerName(int handle, string name)
        {
            MapLayer aLayer = GetLayerFromHandle(handle);
            aLayer.LayerName = name;                   
        }

        /// <summary>
        /// Set layer transparency
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="transPerc"></param>
        public void SetLayerTransparency(int handle, int transPerc)
        {
            VectorLayer aLayer = (VectorLayer)GetLayerFromHandle(handle);
            aLayer.TransparencyPerc = transPerc;
            this.PaintLayers();
        }

        /// <summary>
        /// Set layer visible
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="visible"></param>
        public void SetLayerVisible(int handle, bool visible)
        {
            MapLayer aLayer = GetLayerFromHandle(handle);
            aLayer.Visible = visible;            
            this.PaintLayers();            
        }
        
        /// <summary>
        /// Set layer avoid collision
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="avoidCollision"></param>
        public void SetLayerAvoidCollision(int handle, bool avoidCollision)
        {
            VectorLayer aLayer = (VectorLayer)GetLayerFromHandle(handle);
            aLayer.AvoidCollision = avoidCollision;
            this.PaintLayers();
        }

        /// <summary>
        /// Set image layer extent
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="aExtent"></param>
        /// <param name="aWFP"></param>
        public void SetImageLayerExtent(int handle, Extent aExtent, WorldFilePara aWFP)
        {
            ImageLayer aILayer = (ImageLayer)GetLayerFromHandle(handle);
            aILayer.Extent = aExtent;
            aILayer.WorldFileParaV = aWFP;
            aILayer.WriteImageWorldFile(aILayer.WorldFileName, aWFP);
            this.PaintLayers();
        }

        # endregion

        #endregion Layer

        #region Select
        /// <summary>
        /// Select shapes
        /// </summary>
        /// <param name="aLayer">vector laer</param>
        /// <param name="aPoint">point</param>
        /// <param name="SelectedShapes">ref selected shapes</param>
        /// <param name="isSel">if the selected shapes will be set as selected</param>
        /// <returns>If selected</returns>
        public bool SelectShapes(VectorLayer aLayer, PointF aPoint, ref List<int> SelectedShapes, bool isSel)
        {
            double ProjX, ProjY;
            ProjX = 0;
            ProjY = 0;
            Single sX = aPoint.X;
            Single sY = aPoint.Y;
            ScreenToProj(ref ProjX, ref ProjY, aPoint.X, aPoint.Y);            
            if (Projection.IsLonLatMap)
            {
                if (ProjX < aLayer.Extent.minX)
                {
                    if (aLayer.Extent.minX > -360 && aLayer.Extent.maxX > 0)
                    {
                        ProjToScreen(ProjX, ProjY, ref sX, ref sY, 360);
                    }
                }
                if (ProjX > aLayer.Extent.maxX)
                {
                    if (aLayer.Extent.maxX < 360 && aLayer.Extent.minX < 0)
                    {
                        ProjToScreen(ProjX, ProjY, ref sX, ref sY, -360);
                    }
                }
            }

            int Buffer = 5;
            Extent aExtent = new Extent();
            ScreenToProj(ref ProjX, ref ProjY, sX - Buffer, sY + Buffer);
            aExtent.minX = ProjX;
            aExtent.minY = ProjY;
            ScreenToProj(ref ProjX, ref ProjY, sX + Buffer, sY - Buffer);
            aExtent.maxX = ProjX;
            aExtent.maxY = ProjY;

            SelectedShapes = aLayer.SelectShapes(aExtent, true);
            bool hasSel = SelectedShapes.Count > 0;
            //bool hasSel = aLayer.SelectShapes(aExtent, ref SelectedShapes);
            if (isSel)
            {
                foreach (int i in SelectedShapes)
                    aLayer.ShapeList[i].Selected = true;
            }

            return hasSel;
        }

        /// <summary>
        /// Select shapes
        /// </summary>
        /// <param name="aLayer">vector laer</param>
        /// <param name="aPoint">point</param>
        /// <param name="SelectedShapes">ref selected shapes</param>
        /// <returns>If selected</returns>
        public bool SelectShapes(VectorLayer aLayer, PointF aPoint, ref List<int> SelectedShapes)
        {
            return SelectShapes(aLayer, aPoint, ref SelectedShapes, false);
        }

        /// <summary>
        /// Select shapes
        /// </summary>
        /// <param name="aLayer">vector laer</param>
        /// <param name="rect">Select rectangle</param>
        /// <param name="isSel">if the selected shapes will be set as selected</param>
        /// <returns>If selected</returns>
        public List<int> SelectShapes(VectorLayer aLayer, RectangleF rect, bool isSel)
        {
            Extent aExtent = new Extent();
            ScreenToProj(ref aExtent.minX, ref aExtent.maxY, rect.Left, rect.Top);
            ScreenToProj(ref aExtent.maxX, ref aExtent.minY, rect.Right, rect.Bottom);
            if (Projection.IsLonLatMap)
            {
                if (aExtent.maxX < aLayer.Extent.minX)
                {
                    if (aLayer.Extent.minX > -360 && aLayer.Extent.maxX > 0)
                    {
                        aExtent = MIMath.ShiftExtentLon(aExtent, 360);
                    }
                }
                if (aExtent.minX > aLayer.Extent.maxX)
                {
                    if (aLayer.Extent.maxX < 360 && aLayer.Extent.minX < 0)
                    {
                        aExtent = MIMath.ShiftExtentLon(aExtent, -360);
                    }
                }
            }

            List<int> selectedShapes = aLayer.SelectShapes(aExtent, false);
            if (isSel)
            {
                foreach (int i in selectedShapes)
                    aLayer.ShapeList[i].Selected = true;
            }

            return selectedShapes;
        }

        /// <summary>
        /// Select shapes
        /// </summary>
        /// <param name="aLayer">vector laer</param>
        /// <param name="rect">Select rectangle</param>
        /// <returns>If selected</returns>
        public List<int> SelectShapes(VectorLayer aLayer, RectangleF rect)
        {
            return SelectShapes(aLayer, rect, false);
        }

        /// <summary>
        /// Select grid cell
        /// </summary>
        /// <param name="aLayer">raster layer</param>
        /// <param name="aPoint">point</param>
        /// <param name="iIdx">i index</param>
        /// <param name="jIdx">j index</param>
        /// <returns>if selected</returns>
        public bool SelectGridCell(RasterLayer aLayer, PointF aPoint, ref int iIdx, ref int jIdx)
        {            
            double LonShift = 0;
            //if (_projection.IsLonLatMap)
            //{
            //    if (_drawExtent.maxX < aLayer.Extent.minX)
            //    {
            //        LonShift = -360;
            //    }
            //    if (_drawExtent.minX > aLayer.Extent.maxX)
            //    {
            //        LonShift = 360;
            //    }
            //}
            Single aX = 0, aY = 0;
            ScreenToProj(ref aX, ref aY, aPoint.X, aPoint.Y);
            if (Projection.IsLonLatMap)
            {
                if (aX < aLayer.Extent.minX)
                {
                    if (aLayer.Extent.minX > -360 && aLayer.Extent.maxX > 0)
                    {
                        LonShift = 360;
                    }
                }
                if (aX > aLayer.Extent.maxX)
                {
                    if (aLayer.Extent.maxX < 360 && aLayer.Extent.minX < 0)
                    {
                        LonShift = -360;
                    }
                }
            }
            aX = aX + (Single)LonShift;

            Extent aExtent = new Extent();
            double XDelt = aLayer.GridData.X[1] - aLayer.GridData.X[0];
            double YDelt = aLayer.GridData.Y[1] - aLayer.GridData.Y[0];
            aExtent.minX = aX - XDelt / 2;
            aExtent.maxX = aX + XDelt / 2;
            aExtent.minY = aY - YDelt / 2;
            aExtent.maxY = aY + YDelt / 2;           

            iIdx = -1;
            jIdx = -1;
            for (int i = 0; i < aLayer.GridData.Y.Length; i++)
            {
                if (aLayer.GridData.Y[i] >= aExtent.minY && aLayer.GridData.Y[i] <= aExtent.maxY)
                {
                    iIdx = i;
                    break;
                }
            }
            for (int j = 0; j < aLayer.GridData.X.Length; j++)
            {
                if (aLayer.GridData.X[j] >= aExtent.minX && aLayer.GridData.X[j] <= aExtent.maxX)
                {
                    jIdx = j;
                    break;
                }
            }

            if (iIdx == -1 || jIdx == -1)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Select labels
        /// </summary>
        /// <param name="LayerHnd">Layer handle</param>
        /// <param name="aPoint">mouse point</param>
        /// <param name="SelectedLabels">ref selected labels</param>
        /// <returns>If selected</returns>
        public bool SelectLabels(int LayerHnd, PointF aPoint, ref List<int> SelectedLabels)
        {
            SelectedLabels.Clear();            
            Graphic aLP = new Graphic();
            Extent aExtent = new Extent();            
            int i;

            VectorLayer aLayer = (VectorLayer)GetLayerFromHandle(LayerHnd);
            if (aLayer == null)
                return false;
            else
                if (aLayer.LabelPoints.Count == 0)
                    return false;

            double LonShift = 0;            
            Single ProjX = 0, ProjY = 0;
            ScreenToProj(ref ProjX, ref ProjY, aPoint.X, aPoint.Y);
            if (Projection.IsLonLatMap)
            {
                if (ProjX < aLayer.Extent.minX)
                {
                    if (aLayer.Extent.minX > -360 && aLayer.Extent.maxX > 0)
                    {
                        LonShift = -360;
                    }
                }
                if (ProjX > aLayer.Extent.maxX)
                {
                    if (aLayer.Extent.maxX < 360 && aLayer.Extent.minX < 0)
                    {
                        LonShift = 360;
                    }
                }
            }

            for (i = 0; i < aLayer.LabelPoints.Count; i++)
            {
                aLP = aLayer.LabelPoints[i];
                PointShape aPS = (PointShape)aLP.Shape;
                LabelBreak aLB = (LabelBreak)aLP.Legend;
                Single aX = 0, aY = 0;
                ProjToScreen(aPS.Point.X, aPS.Point.Y, ref aX, ref aY, LonShift);
                SizeF labSize = this.CreateGraphics().MeasureString(aLB.Text, aLB.Font);
                switch (aLB.AlignType)
                {
                    case AlignType.Center:
                        aX = aX - labSize.Width / 2;
                        break;
                    case AlignType.Left:
                        aX = aX - labSize.Width;
                        break;
                }
                aY -= aLB.YShift;
                aExtent.minX = aX;
                aExtent.maxX = aX + labSize.Width;
                aExtent.minY = aY;
                aExtent.maxY = aY + labSize.Height;

                if (MIMath.PointInExtent(aPoint, aExtent))
                {
                    SelectedLabels.Add(i);
                    break;
                }
            }

            if (SelectedLabels.Count > 0)
                return true;
            else
                return false;
        }

        private GraphicCollection GetVisibleGraphics()
        {
            GraphicCollection graphicCollection = new GraphicCollection();
            foreach (Graphic aGraphic in _graphicCollection.GraphicList)
                graphicCollection.Add(aGraphic);

            foreach (MapLayer aLayer in LayerSet.Layers)
            {                
                if (aLayer.LayerType == LayerTypes.VectorLayer && aLayer.Visible)
                {
                    VectorLayer vLayer = (VectorLayer)aLayer;
                    foreach (Graphic aGraphic in vLayer.LabelPoints)
                    {
                        if (aGraphic.Shape.Visible)
                            graphicCollection.Add(aGraphic);
                    }
                    foreach (Graphic aGraphic in vLayer.ChartPoints)
                    {
                        if (aGraphic.Shape.Visible)
                            graphicCollection.Add(aGraphic);
                    }
                }
            }

            return graphicCollection;
        }

        /// <summary>
        /// Select graphics by point
        /// </summary>
        /// <param name="aPoint">mouse point</param>
        /// <param name="selectedGraphics">ref selected graphics</param>
        /// <param name="lonShift">longitude shift</param>
        /// <returns>if selected</returns>
        public bool SelectGraphics(PointF aPoint, ref GraphicCollection selectedGraphics, ref double lonShift)
        {
            _visibleGraphics = GetVisibleGraphics();
            return SelectGraphics(aPoint, _visibleGraphics, ref selectedGraphics, ref lonShift, 0);
        }

        /// <summary>
        /// Select graphics by point
        /// </summary>
        /// <param name="aPoint">mouse point</param>
        /// <param name="baseGraphics">base graphics</param>
        /// <param name="selectedGraphics">ref selected graphics</param>
        /// <param name="lonShift">longitude shift</param>
        /// <param name="limit">tolerance limit</param>
        /// <returns>if selected</returns>
        public bool SelectGraphics_back(PointF aPoint, GraphicCollection baseGraphics, ref GraphicCollection selectedGraphics,
            ref double lonShift, int limit)
        {
            if (baseGraphics.Count == 0)
                return false;

            selectedGraphics.GraphicList.Clear();            
            int i;
            Graphics g = this.CreateGraphics();
            bool ifSel = true;
                        
            if (Projection.IsLonLatMap)
            {
                bool ifCheckLonShift = true;
                if (baseGraphics.GraphicList[0].Shape.ShapeType == ShapeTypes.Point)
                {
                    for (i = 0; i < baseGraphics.Count; i++)
                    {
                        Graphic aGraphic = baseGraphics.GraphicList[i];
                        Rectangle rect = GetGraphicRectangle(g, aGraphic, lonShift);
                        rect.Inflate(limit, limit);
                        if (MIMath.PointInRectangle(aPoint, rect))
                        {
                            selectedGraphics.Add(aGraphic);
                            break;
                        }
                    }

                    if (selectedGraphics.Count > 0)
                    {
                        ifCheckLonShift = false;
                        ifSel = false;
                    }
                }

                if (ifCheckLonShift)
                {
                    Single ProjX = 0, ProjY = 0;
                    ScreenToProj(ref ProjX, ref ProjY, aPoint.X, aPoint.Y);
                    if (ProjX < baseGraphics.Extent.minX)
                    {
                        if (baseGraphics.Extent.minX > -360 && baseGraphics.Extent.maxX > 0)
                        {
                            lonShift = -360;
                        }
                    }
                    if (ProjX > baseGraphics.Extent.maxX)
                    {
                        if (baseGraphics.Extent.maxX < 360 && baseGraphics.Extent.minX < 0)
                        {
                            lonShift = 360;
                        }
                    }
                }
            }

            if (ifSel)
            {
                for (i = 0; i < baseGraphics.Count; i++)
                {
                    Graphic aGraphic = baseGraphics.GraphicList[i];
                    Rectangle rect = GetGraphicRectangle(g, aGraphic, lonShift);
                    rect.Inflate(limit, limit);
                    if (MIMath.PointInRectangle(aPoint, rect))
                    {
                        selectedGraphics.Add(aGraphic);
                        //break;
                    }
                }
            }

            if (selectedGraphics.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Select graphics by point
        /// </summary>
        /// <param name="aPoint">mouse point</param>
        /// <param name="baseGraphics">base graphics</param>
        /// <param name="selectedGraphics">ref selected graphics</param>
        /// <param name="lonShift">longitude shift</param>
        /// <param name="limit">tolerance limit</param>
        /// <returns>if selected</returns>
        public bool SelectGraphics(PointF aPoint, GraphicCollection baseGraphics, ref GraphicCollection selectedGraphics,
            ref double lonShift, int limit)
        {
            if (baseGraphics.Count == 0)
                return false;

            selectedGraphics.GraphicList.Clear();
            int i;
            Graphics g = this.CreateGraphics();
            bool ifSel = true;
            double projX = 0, projY = 0;
            ScreenToProj(ref projX, ref projY, (double)aPoint.X, (double)aPoint.Y);
            PointD pp = new PointD(projX + lonShift, projY);
            double buffer = 5 / this._scaleX;

            if (Projection.IsLonLatMap)
            {
                bool ifCheckLonShift = true;
                for (i = 0; i < baseGraphics.Count; i++)
                {
                    Graphic aGraphic = baseGraphics.GraphicList[i];
                    switch (aGraphic.Shape.ShapeType)
                    {
                        case ShapeTypes.Polyline:
                        case ShapeTypes.CurveLine:
                            PolylineShape aPLS = (PolylineShape)aGraphic.Shape;
                            if (GeoComputation.SelectPolylineShape(pp, aPLS, buffer))
                            {
                                selectedGraphics.Add(aGraphic);
                            }
                            break;
                        default:
                            Rectangle rect = GetGraphicRectangle(g, aGraphic, lonShift);
                            rect.Inflate(limit, limit);
                            if (MIMath.PointInRectangle(aPoint, rect))
                            {
                                selectedGraphics.Add(aGraphic);
                            }
                            break;
                    }
                }

                if (selectedGraphics.Count > 0)
                {
                    ifCheckLonShift = false;
                    ifSel = false;
                }

                if (ifCheckLonShift)
                {
                    Single ProjX = 0, ProjY = 0;
                    ScreenToProj(ref ProjX, ref ProjY, aPoint.X, aPoint.Y);
                    if (ProjX < baseGraphics.Extent.minX)
                    {
                        if (baseGraphics.Extent.minX > -360 && baseGraphics.Extent.maxX > 0)
                        {
                            lonShift = -360;
                        }
                    }
                    if (ProjX > baseGraphics.Extent.maxX)
                    {
                        if (baseGraphics.Extent.maxX < 360 && baseGraphics.Extent.minX < 0)
                        {
                            lonShift = 360;
                        }
                    }
                }
            }

            if (ifSel)
            {
                pp = new PointD(projX + lonShift, projY);
                for (i = 0; i < baseGraphics.Count; i++)
                {
                    Graphic aGraphic = baseGraphics.GraphicList[i];
                    switch (aGraphic.Shape.ShapeType)
                    {
                        case ShapeTypes.Polyline:
                        case ShapeTypes.CurveLine:
                            PolylineShape aPLS = (PolylineShape)aGraphic.Shape;
                            if (GeoComputation.SelectPolylineShape(pp, aPLS, buffer))
                            {
                                selectedGraphics.Add(aGraphic);
                            }
                            break;
                        default:
                            Rectangle rect = GetGraphicRectangle(g, aGraphic, lonShift);
                            rect.Inflate(limit, limit);
                            if (MIMath.PointInRectangle(aPoint, rect))
                            {
                                selectedGraphics.Add(aGraphic);
                            }
                            break;
                    }
                }
            }

            if (selectedGraphics.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Select graphics by rectangle
        /// </summary>
        /// <param name="aRect">select rectangle</param>
        /// <param name="selectedGraphics">ref selected graphics</param>
        /// <param name="lonShift">longitude shift</param>
        /// <returns>if selected</returns>
        public bool SelectGraphics(Rectangle aRect, ref GraphicCollection selectedGraphics, ref double lonShift)
        {
            _visibleGraphics = GetVisibleGraphics();
            return SelectGraphics(aRect, _visibleGraphics, ref selectedGraphics, ref lonShift);
        }

        /// <summary>
        /// Select graphics by rectangle
        /// </summary>
        /// <param name="aRect">select rectangle</param>
        /// <param name="baseGraphics">base graphics</param>
        /// <param name="selectedGraphics">ref selected graphics</param>
        /// <param name="lonShift">longitude shift</param>
        /// <returns>if selected</returns>
        public bool SelectGraphics(Rectangle aRect, GraphicCollection baseGraphics, ref GraphicCollection selectedGraphics,
            ref double lonShift)
        {
            if (baseGraphics.Count == 0)
                return false;

            selectedGraphics.GraphicList.Clear();
            int i;
            Graphics g = this.CreateGraphics();
            bool ifSel = true;

            if (Projection.IsLonLatMap)
            {
                bool ifCheckLonShift = true;
                if (baseGraphics.GraphicList[0].Shape.ShapeType == ShapeTypes.Point)
                {
                    for (i = 0; i < baseGraphics.Count; i++)
                    {
                        Graphic aGraphic = baseGraphics.GraphicList[i];
                        Rectangle rect = GetGraphicRectangle(g, aGraphic, lonShift);
                        if (MIMath.IsInclude(aRect, rect))
                        {
                            selectedGraphics.Add(aGraphic);
                            break;
                        }
                    }

                    if (selectedGraphics.Count > 0)
                    {
                        ifCheckLonShift = false;
                        ifSel = false;
                    }
                }

                if (ifCheckLonShift)
                {
                    Single ProjX = 0, ProjY = 0;
                    Point aPoint = new Point(aRect.Left + aRect.Width / 2, aRect.Top + aRect.Height / 2);
                    ScreenToProj(ref ProjX, ref ProjY, aPoint.X, aPoint.Y);
                    if (ProjX < baseGraphics.Extent.minX)
                    {
                        if (baseGraphics.Extent.minX > -360 && baseGraphics.Extent.maxX > 0)
                        {
                            lonShift = -360;
                        }
                    }
                    if (ProjX > baseGraphics.Extent.maxX)
                    {
                        if (baseGraphics.Extent.maxX < 360 && baseGraphics.Extent.minX < 0)
                        {
                            lonShift = 360;
                        }
                    }
                }
            }

            if (ifSel)
            {
                for (i = 0; i < baseGraphics.Count; i++)
                {
                    Graphic aGraphic = baseGraphics.GraphicList[i];
                    Rectangle rect = GetGraphicRectangle(g, aGraphic, lonShift);
                    if (MIMath.IsInclude(aRect, rect))
                    {
                        selectedGraphics.Add(aGraphic);
                        //break;
                    }
                }
            }

            if (selectedGraphics.Count > 0)
                return true;
            else
                return false;
        }

        private Rectangle GetGraphicRectangle(Graphics g, Graphic aGraphic, double lonShift)
        {
            Rectangle rect = new Rectangle();
            Single aX = 0, aY = 0;
            switch (aGraphic.Shape.ShapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes.PointM:
                    PointShape aPS = (PointShape)aGraphic.Shape;
                    ProjToScreen(aPS.Point.X, aPS.Point.Y, ref aX, ref aY, lonShift);

                    switch (aGraphic.Legend.BreakType)
                    {
                        case BreakTypes.PointBreak:
                            PointBreak aPB = (PointBreak)aGraphic.Legend;
                            int buffer = (int)aPB.Size + 2;
                            rect.X = (int)aX - buffer / 2;
                            rect.Y = (int)aY - buffer / 2;
                            rect.Width = buffer;
                            rect.Height = buffer;
                            break;
                        case BreakTypes.LabelBreak:
                            LabelBreak aLB = (LabelBreak)aGraphic.Legend;
                            SizeF labSize = this.CreateGraphics().MeasureString(aLB.Text, aLB.Font);
                            switch (aLB.AlignType)
                            {
                                case AlignType.Center:
                                    aX = aX - labSize.Width / 2;
                                    break;
                                case AlignType.Left:
                                    aX = aX - labSize.Width;
                                    break;
                            }
                            aY -= aLB.YShift;
                            rect.X = (int)aX;
                            rect.Y = (int)aY;
                            rect.Width = (int)labSize.Width;
                            rect.Height = (int)labSize.Height;
                            break;
                        case BreakTypes.ChartBreak:
                            ChartBreak aCB = (ChartBreak)aGraphic.Legend;
                            rect = aCB.GetDrawExtent(new PointF(aX, aY)).ConvertToRectangle();
                            break;
                    }                    
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.Polygon:
                case ShapeTypes.Rectangle:
                case ShapeTypes.CurveLine:
                case ShapeTypes.Ellipse:
                case ShapeTypes.Circle:
                case ShapeTypes.CurvePolygon:
                    List<PointD> newPList = aGraphic.Shape.GetPoints();
                    List<PointD> points = new List<PointD>();
                    double X = 0, Y = 0;
                    for (int i = 0; i < newPList.Count; i++)
                    {
                        PointD wPoint = newPList[i];
                        ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y, lonShift);                        
                        points.Add(new PointD(X, Y));
                    }
                    Extent aExtent = MIMath.GetPointsExtent(points);
                    rect.X = (int)aExtent.minX;
                    rect.Y = (int)aExtent.minY;
                    rect.Width = (int)(aExtent.maxX - aExtent.minX);
                    rect.Height = (int)(aExtent.maxY - aExtent.minY);
                    break;
            }

            return rect;
        }

        /// <summary>
        /// Remove a graphic
        /// </summary>
        /// <param name="aGraphic">graphic</param>
        public void RemoveGraphic(Graphic aGraphic)
        {
            if (_graphicCollection.GraphicList.Contains(aGraphic))
                _graphicCollection.Remove(aGraphic);
            else
            {
                foreach (MapLayer aLayer in _layerSet.Layers)
                {
                    if (aLayer.LayerType == LayerTypes.VectorLayer)
                    {
                        VectorLayer aVLayer = (VectorLayer)aLayer;
                        if (aVLayer.LabelPoints.Contains(aGraphic))
                        {
                            aVLayer.LabelPoints.Remove(aGraphic);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Remove selected graphics
        /// </summary>
        public void RemoveSelectedGraphics()
        {
            foreach (Graphic aGraphic in _selectedGraphics.GraphicList)
                RemoveGraphic(aGraphic);

            _selectedGraphics.RemoveAll();            
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

        private bool SelectEditVertices(Point aPoint, Shape.Shape aShape, ref List<PointD> vertices, ref int vIdx)
        {
            List<PointD> points = aShape.GetPoints();
            int buffer = 4;            
            Extent aExtent = new Extent();
            double ProjX = 0, ProjY = 0;
            ScreenToProj(ref ProjX, ref ProjY, aPoint.X - buffer, aPoint.Y + buffer);
            aExtent.minX = ProjX;
            aExtent.minY = ProjY;
            ScreenToProj(ref ProjX, ref ProjY, aPoint.X + buffer, aPoint.Y - buffer);
            aExtent.maxX = ProjX;
            aExtent.maxY = ProjY;
            
            vertices.Clear();
            PointD aPD = new PointD();
            for (int i = 0; i < points.Count; i++)
            {
                if (MIMath.PointInExtent(points[i], aExtent))
                {
                    vIdx = i;
                    vertices.Add(points[i]);
                    switch (aShape.ShapeType)
                    {
                        case ShapeTypes.Polyline:
                        case ShapeTypes.CurveLine:
                            if (i == 0)
                                vertices.Add(points[i + 1]);
                            else if (i == points.Count - 1)
                                vertices.Add(points[i - 1]);
                            else
                            {
                                vertices.Add(points[i - 1]);
                                vertices.Add(points[i + 1]);
                            }
                            break;
                        default:
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
                            break;
                    }

                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Paint layers
        /// </summary>
        public void PaintLayers()
        {
            if (!_lockViewUpdate)
            {
                //RefreshXYScale(this.Width, this.Height);

                //_mapBitmap = new Bitmap(this.ClientRectangle.Width,
                //    this.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
                GC.Collect();
                _mapBitmap = new Bitmap(this.Width,
                    this.Height, PixelFormat.Format32bppPArgb);
                Graphics g = Graphics.FromImage(_mapBitmap);

                GetMaskOutGraphicsPath(g);

                g.SmoothingMode = _smoothingMode;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                //g.InterpolationMode = InterpolationMode.NearestNeighbor;
                //g.CompositingMode = CompositingMode.SourceCopy;
                //g.CompositingQuality = CompositingQuality.HighSpeed;
                //g.PixelOffsetMode = PixelOffsetMode.Half;

                _xGridPosLabel.Clear();
                _yGridPosLabel.Clear();

                if (_isGeoMap)
                {
                    UpdateLonLatLayer();
                    if (_projection.IsLonLatMap)
                    {
                        DrawLonLatMap(g);
                    }
                    else
                    {
                        DrawProjectedMap(g);
                    }
                }
                else
                {
                    Draw2DMap(g);
                }

                //Draw neatline
                if (_drawNeatLine)
                {
                    Rectangle mapRect = new Rectangle(_neatLineSize - 1, _neatLineSize - 1,
                        this.Width - _neatLineSize, this.Height - _neatLineSize);
                    g.DrawRectangle(new Pen(_neatLineColor, _neatLineSize), mapRect);
                }

                g.Dispose();

                this.Refresh();

                //Fire OnMayViewReDrawed event
                //if (_isLayoutMap)                    
                OnMapViewReDrawed();
            }
        }

        /// <summary>
        /// Paint graphics
        /// </summary>
        /// <param name="g">graphics</param>
        public void PaintGraphics(Graphics g)
        {
            if (!_lockViewUpdate)
            {
                //RefreshXYScale(this.Width, this.Height);

                //Rectangle R = this.Bounds;
                //GraphicsPath path = new GraphicsPath();
                //g.SetClip(path);
                GetMaskOutGraphicsPath(g);

                g.SmoothingMode = _smoothingMode;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;

                _xGridPosLabel.Clear();
                _yGridPosLabel.Clear();

                if (_isGeoMap)
                {
                    UpdateLonLatLayer();
                    if (_projection.IsLonLatMap)
                    {
                        DrawLonLatMap(g);
                    }
                    else
                    {
                        DrawProjectedMap(g);
                    }
                }
                else
                {
                    Draw2DMap(g);
                }

                //Draw neatline
                if (_drawNeatLine)
                {
                    Rectangle mapRect = new Rectangle(_neatLineSize - 1, _neatLineSize - 1,
                        this.Width - _neatLineSize, this.Height - _neatLineSize);
                    g.DrawRectangle(new Pen(_neatLineColor, _neatLineSize), mapRect);
                }
            }
        }

        /// <summary>
        /// Paint graphics
        /// </summary>
        /// <param name="g">graphics</param>
        /// <param name="rect">target rectangle</param>
        public void PaintGraphics(Graphics g, Rectangle rect)
        {
            RefreshXYScale(rect.Width, rect.Height);

            Matrix oldMatrix = g.Transform;
            Region oldRegion = g.Clip;
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(rect);
            g.SetClip(path);

            GetMaskOutGraphicsPath(g);            

            g.TranslateTransform(rect.X, rect.Y);
            _maskOutGraphicsPath.Transform(g.Transform);
            
            g.SmoothingMode = _smoothingMode;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            _xGridPosLabel.Clear();
            _yGridPosLabel.Clear();

            if (_isGeoMap)
            {
                UpdateLonLatLayer();
                if (_projection.IsLonLatMap)
                {
                    DrawLonLatMap(g, rect.Width, rect.Height);
                }
                else
                {
                    DrawProjectedMap(g, rect.Width, rect.Height);
                }
                GetLonLatGridLabels();
            }
            else
            {
                Draw2DMap(g);
            }

            g.Transform = oldMatrix;
            g.Clip = oldRegion;
        }

        /// <summary>
        /// Draw layers with legend scheme
        /// </summary>
        /// <param name="aLayer">vector layer</param>
        /// <param name="g">graphic</param>
        /// <param name="LonShift">longitude shift</param>        
        public void DrawLayerWithLegendScheme(VectorLayer aLayer,
            Graphics g, double LonShift)
        {
            Extent lExtent = MIMath.ShiftExtentLon(aLayer.Extent, LonShift);
            if (!MIMath.IsExtentCross(lExtent, _drawExtent))
            {
                return;
            }

            bool hasDrawCharts = false;
            switch (aLayer.ShapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes.PointM:
                    //Draw layer charts
                    if (aLayer.ChartSet.DrawCharts)
                    {
                        DrawLayerCharts(g, aLayer, LonShift);
                        hasDrawCharts = true;
                    }

                    DrawPointLayer(aLayer, g, LonShift);
                    break;
                case ShapeTypes.Polygon:
                case ShapeTypes.PolygonM:
                    DrawPolygonLayer(aLayer, g, LonShift);
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineM:
                case ShapeTypes.PolylineZ:
                    DrawPolylineLayer(aLayer, g, LonShift);
                    break;
            }

            //Draw layer labels
            if (aLayer.LabelSet.DrawLabels)
                DrawLayerLabels(g, aLayer, LonShift);

            //Draw layer charts
            if (!hasDrawCharts)
            {
                if (aLayer.ChartSet.DrawCharts)
                    DrawLayerCharts(g, aLayer, LonShift);
            }
        }

        private void DrawLonLatLayer(VectorLayer aLayer, Graphics g, double LonShift)
        {
            Extent lExtent = MIMath.ShiftExtentLon(aLayer.Extent, LonShift);
            if (!MIMath.IsExtentCross(lExtent, _drawExtent))
            {
                return;
            }

            int shapeIdx = 0;            
            LegendScheme aLS = aLayer.LegendScheme;            
            Font drawFont = new Font("Arial", 7);                       

            foreach (PolylineShape aPLS in aLayer.ShapeList)
            {
                if (!aPLS.Visible)
                    continue;

                PolyLineBreak aPLB = new PolyLineBreak();
                string vStr = aLayer.GetCellValue(0, shapeIdx).ToString();
                aPLB = (PolyLineBreak)aLS.LegendBreaks[0];
                if (aPLB.DrawPolyline)
                {
                    DrawLonLatPolylineShape(g, aPLS, aPLB, LonShift);
                }
                shapeIdx += 1;
            }            
        }

        private void DrawPointLayer(VectorLayer aLayer, Graphics g, double LonShift)
        {
            SmoothingMode aSM = g.SmoothingMode;
            g.SmoothingMode = this._pointSmoothingMode;
            Single X, Y;
            X = 0;
            Y = 0;
            PointF aPoint = new PointF();
            LegendScheme aLS = aLayer.LegendScheme;

            foreach (PointShape aPS in aLayer.ShapeList)
            {
                if (aPS.Point.X + LonShift < _drawExtent.minX || aPS.Point.X + LonShift > _drawExtent.maxX
                    || aPS.Point.Y < _drawExtent.minY || aPS.Point.Y > _drawExtent.maxY)
                {
                    continue;
                }
                if (aPS.LegendIndex < 0)
                    continue;

                PointBreak aPB = (PointBreak)aLS.LegendBreaks[aPS.LegendIndex];
                ProjToScreen(aPS.Point.X, aPS.Point.Y, ref X, ref Y,
                                    LonShift);
                aPoint.X = X;
                aPoint.Y = Y;
                if (aPB.DrawShape)
                {
                    if (aPS.Selected)
                    {
                        PointBreak newPB = (PointBreak)aPB.Clone();
                        newPB.Color = _selectColor;
                        newPB.Size = 10;
                        if (aPB.Size > 10)
                            newPB.Size = aPB.Size;
                        else
                            newPB.DrawOutline = false;
                        Draw.DrawPoint(aPoint, newPB, g);
                    }
                    else
                        Draw.DrawPoint(aPoint, aPB, g);
                }                
            }

            //Draw identifer shape
            if (_drawIdentiferShape)
            {
                PointShape aPS = (PointShape)aLayer.ShapeList[aLayer.IdentiferShape];
                ProjToScreen(aPS.Point.X, aPS.Point.Y, ref X, ref Y,
                                    LonShift);
                aPoint.X = X;
                aPoint.Y = Y;
                PointBreak aPB = new PointBreak();
                aPB.OutlineColor = Color.Red;
                aPB.Size = 10;
                aPB.Style = PointStyle.Square;
                aPB.DrawFill = false;

                Draw.DrawPoint(aPoint, aPB, g);
            }

            g.SmoothingMode = aSM;
        }

        private void DrawPointLayer_back(VectorLayer aLayer, Graphics g, double LonShift)
        {
            SmoothingMode aSM = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int shapeIdx = 0;
            Single X, Y;
            X = 0;
            Y = 0;
            PointF aPoint = new PointF();
            double value;
            int blNum = 0;
            LegendScheme aLS = aLayer.LegendScheme;

            foreach (PointShape aPS in aLayer.ShapeList)
            {
                if (aPS.Point.X + LonShift < _drawExtent.minX || aPS.Point.X + LonShift > _drawExtent.maxX
                    || aPS.Point.Y < _drawExtent.minY || aPS.Point.Y > _drawExtent.maxY)
                {
                    shapeIdx += 1;
                    continue;
                }
                PointBreak aPB = new PointBreak();
                switch (aLS.LegendType)
                {
                    case LegendType.SingleSymbol:
                        aPB = (PointBreak)aLS.LegendBreaks[0];
                        ProjToScreen(aPS.Point.X, aPS.Point.Y, ref X, ref Y,
                                    LonShift);
                        aPoint.X = X;
                        aPoint.Y = Y;
                        if (aPB.DrawShape)
                        {
                            if (aPS.Selected)
                            {
                                PointBreak newPB = (PointBreak)aPB.Clone();
                                newPB.Color = _selectColor;
                                Draw.DrawPoint(aPoint, newPB, g);
                            }
                            else
                                Draw.DrawPoint(aPoint, aPB, g);
                        }
                        break;
                    case LegendType.UniqueValue:
                        string vStr = aLayer.GetCellValue(aLS.FieldName, shapeIdx).ToString();
                        for (int j = 0; j < aLS.LegendBreaks.Count; j++)
                        {
                            aPB = (PointBreak)aLS.LegendBreaks[j];
                            if (vStr == aPB.StartValue.ToString())
                            {
                                ProjToScreen(aPS.Point.X, aPS.Point.Y, ref X, ref Y,
                                    LonShift);
                                aPoint.X = X;
                                aPoint.Y = Y;
                                if (aPB.DrawShape)
                                {
                                    if (aPS.Selected)
                                    {
                                        PointBreak newPB = (PointBreak)aPB.Clone();
                                        newPB.Color = _selectColor;
                                        Draw.DrawPoint(aPoint, newPB, g);
                                    }
                                    else
                                        Draw.DrawPoint(aPoint, aPB, g);
                                }
                            }
                        }
                        break;
                    case LegendType.GraduatedColor:
                        vStr = aLayer.GetCellValue(aLS.FieldName, shapeIdx).ToString().Trim();
                        if (vStr == string.Empty || vStr == null)
                            value = 0;
                        else
                            value = double.Parse(vStr);
                        blNum = 0;
                        for (int j = 0; j < aLS.LegendBreaks.Count; j++)
                        {
                            aPB = (PointBreak)aLS.LegendBreaks[j];
                            blNum += 1;
                            if (MIMath.DoubleEquals(value, double.Parse(aPB.StartValue.ToString())) ||
                                (value > double.Parse(aPB.StartValue.ToString())
                                && value < double.Parse(aPB.EndValue.ToString())) ||
                                (blNum == aLS.LegendBreaks.Count && value == double.Parse(aPB.EndValue.ToString())))
                            {
                                ProjToScreen(aPS.Point.X, aPS.Point.Y, ref X, ref Y,
                                    LonShift);
                                aPoint.X = X;
                                aPoint.Y = Y;
                                if (aPB.DrawShape)
                                {
                                    if (aPS.Selected)
                                    {
                                        PointBreak newPB = (PointBreak)aPB.Clone();
                                        newPB.Color = _selectColor;
                                        Draw.DrawPoint(aPoint, newPB, g);
                                    }
                                    else
                                        Draw.DrawPoint(aPoint, aPB, g);
                                }
                            }
                        }
                        break;
                }
                shapeIdx += 1;
            }

            //Draw identifer shape
            if (_drawIdentiferShape)
            {
                PointShape aPS = (PointShape)aLayer.ShapeList[aLayer.IdentiferShape];
                ProjToScreen(aPS.Point.X, aPS.Point.Y, ref X, ref Y,
                                    LonShift);
                aPoint.X = X;
                aPoint.Y = Y;
                PointBreak aPB = new PointBreak();
                aPB.OutlineColor = Color.Red;
                aPB.Size = 10;
                aPB.Style = PointStyle.Square;
                aPB.DrawFill = false;

                Draw.DrawPoint(aPoint, aPB, g);
            }

            g.SmoothingMode = aSM;
        }


        private void DrawPolygonLayer(VectorLayer aLayer, Graphics g, double LonShift)
        {            
            LegendScheme aLS = aLayer.LegendScheme;

            foreach (PolygonShape aPGS in aLayer.ShapeList)
            {
                if (aPGS.LegendIndex < 0)
                    continue;

                PolygonBreak aPGB = (PolygonBreak)aLS.LegendBreaks[aPGS.LegendIndex];
                if (aPGB.DrawShape)
                {
                    DrawPolygonShape(g, aPGS, aPGB, LonShift, aLayer.TransparencyPerc);
                }                
            }

            //Draw identifer shape
            if (_drawIdentiferShape)
            {
                PolygonShape aPGS = (PolygonShape)aLayer.ShapeList[aLayer.IdentiferShape];
                PolygonBreak aPGB = new PolygonBreak();
                aPGB.OutlineColor = Color.Red;
                aPGB.OutlineSize = 2;
                aPGB.Color = Color.Red;
                DrawPolygonShape(g, aPGS, aPGB, LonShift, 50);
            }
        }

        private void DrawPolygonLayer_bak(VectorLayer aLayer, Graphics g, double LonShift)
        {
            int shapeIdx = 0;
            double value;
            int blNum = 0;
            LegendScheme aLS = aLayer.LegendScheme;

            foreach (PolygonShape aPGS in aLayer.ShapeList)
            {
                PolygonBreak aPGB = new PolygonBreak();
                switch (aLS.LegendType)
                {
                    case LegendType.SingleSymbol:
                        aPGB = (PolygonBreak)aLS.LegendBreaks[0];
                        if (aPGB.DrawShape)
                        {
                            DrawPolygonShape(g, aPGS, aPGB, LonShift, aLayer.TransparencyPerc);
                        }
                        break;
                    case LegendType.UniqueValue:
                        string vStr = aLayer.GetCellValue(aLS.FieldName, shapeIdx).ToString();
                        for (int j = 0; j < aLS.LegendBreaks.Count; j++)
                        {
                            aPGB = (PolygonBreak)aLS.LegendBreaks[j];
                            if (vStr == aPGB.StartValue.ToString())
                            {
                                if (aPGB.DrawShape)
                                {
                                    DrawPolygonShape(g, aPGS, aPGB, LonShift, aLayer.TransparencyPerc);
                                }
                            }
                        }
                        break;
                    case LegendType.GraduatedColor:
                        vStr = aLayer.GetCellValue(aLS.FieldName, shapeIdx).ToString();
                        value = double.Parse(vStr);
                        blNum = 0;
                        for (int j = 0; j < aLS.LegendBreaks.Count; j++)
                        {
                            aPGB = (PolygonBreak)aLS.LegendBreaks[j];
                            blNum += 1;
                            if (MIMath.DoubleEquals(value, double.Parse(aPGB.StartValue.ToString())) ||
                                (value > double.Parse(aPGB.StartValue.ToString())
                                && value < double.Parse(aPGB.EndValue.ToString())) ||
                                (blNum == aLS.LegendBreaks.Count && value == double.Parse(aPGB.EndValue.ToString())))
                            {
                                if (aPGB.DrawShape)
                                {
                                    DrawPolygonShape(g, aPGS, aPGB, LonShift, aLayer.TransparencyPerc);
                                }
                            }
                        }
                        break;
                }
                shapeIdx += 1;
            }

            //Draw identifer shape
            if (_drawIdentiferShape)
            {
                PolygonShape aPGS = (PolygonShape)aLayer.ShapeList[aLayer.IdentiferShape];
                PolygonBreak aPGB = new PolygonBreak();
                aPGB.OutlineColor = Color.Red;
                aPGB.OutlineSize = 2;
                aPGB.Color = Color.Red;
                DrawPolygonShape(g, aPGS, aPGB, LonShift, 50);
            }
        }

        private void DrawPolylineLayer(VectorLayer aLayer, Graphics g, double LonShift)
        {            
            //double value;
            //int blNum = 0;
            LegendScheme aLS = aLayer.LegendScheme;
            //PointD wPoint;
            //Single X, Y;
            //X = 0;
            //Y = 0;
            //Font drawFont = new Font("Arial", 7);
            bool isStreamline = false;

            switch (aLayer.LayerDrawType)
            {
                //case LayerDrawType.TrajLine:
                //    //Draw start point symbol                                     
                //    PointF aPF = new PointF();
                //    foreach (PolylineShape aPLS in aLayer.ShapeList)
                //    {
                //        wPoint = (PointD)aPLS.Points[0];
                //        aPF.X = (Single)(wPoint.X + LonShift);
                //        aPF.Y = (Single)(wPoint.Y);
                //        if (MIMath.PointInExtent(aPF, _drawExtent))
                //        {
                //            ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
                //                LonShift);
                //            aPF.X = X;
                //            aPF.Y = Y;
                //            Draw.DrawPoint(PointStyle.UpTriangle, aPF, this.ForeColor,
                //                this.ForeColor, 10, true, true, g);
                //        }
                //    }
                //    break;
                case LayerDrawType.Streamline:
                    isStreamline = true;
                    break;
            }

            foreach (PolylineShape aPLS in aLayer.ShapeList)
            {
                if (!aPLS.Visible)
                    continue;
                if (aPLS.LegendIndex < 0)
                    continue;

                PolyLineBreak aPLB = (PolyLineBreak)aLS.LegendBreaks[aPLS.LegendIndex];
                if (aPLB.DrawPolyline || aPLB.DrawSymbol)
                {
                    DrawPolylineShape(g, aPLS, aPLB, LonShift, isStreamline);
                }
            }

            //Draw identifer shape
            if (_drawIdentiferShape)
            {
                PolylineShape aPLS = (PolylineShape)aLayer.ShapeList[aLayer.IdentiferShape];
                PolyLineBreak aPLB = new PolyLineBreak();
                aPLB.Color = Color.Red;
                aPLB.Size = 2;
                DrawPolylineShape(g, aPLS, aPLB, LonShift, isStreamline);
            }
        }

        private void DrawPolylineLayer_back(VectorLayer aLayer, Graphics g, double LonShift)
        {
            int shapeIdx = 0;
            double value;
            int blNum = 0;
            LegendScheme aLS = aLayer.LegendScheme;
            PointD wPoint;
            Single X, Y;
            X = 0;
            Y = 0;
            //Font drawFont = new Font("Arial", 7);
            bool isStreamline = false;

            switch (aLayer.LayerDrawType)
            {
                case LayerDrawType.TrajLine:
                    //Draw start point symbol                                     
                    PointF aPF = new PointF();
                    foreach (PolylineShape aPLS in aLayer.ShapeList)
                    {
                        wPoint = (PointD)aPLS.Points[0];
                        aPF.X = (Single)(wPoint.X + LonShift);
                        aPF.Y = (Single)(wPoint.Y);
                        if (MIMath.PointInExtent(aPF, _drawExtent))
                        {
                            ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
                                LonShift);
                            aPF.X = X;
                            aPF.Y = Y;
                            Draw.DrawPoint(PointStyle.UpTriangle, aPF, this.ForeColor,
                                this.ForeColor, 10, true, true, g);
                        }
                    }
                    break;
                case LayerDrawType.Streamline:
                    isStreamline = true;
                    break;
            }

            foreach (PolylineShape aPLS in aLayer.ShapeList)
            {
                if (!aPLS.Visible)
                    continue;

                PolyLineBreak aPLB = new PolyLineBreak();
                switch (aLS.LegendType)
                {
                    case LegendType.SingleSymbol:
                        string vStr = aLayer.GetCellValue(0, shapeIdx).ToString();
                        aPLB = (PolyLineBreak)aLS.LegendBreaks[0];
                        if (aPLB.DrawPolyline || aPLB.DrawSymbol)
                        {
                            DrawPolylineShape(g, aPLS, aPLB, LonShift, isStreamline);
                        }
                        break;
                    case LegendType.UniqueValue:
                        vStr = aLayer.GetCellValue(aLS.FieldName, shapeIdx).ToString();
                        for (int j = 0; j < aLS.LegendBreaks.Count; j++)
                        {
                            aPLB = (PolyLineBreak)aLS.LegendBreaks[j];
                            if (vStr == aPLB.StartValue.ToString())
                            {
                                if (aPLB.DrawPolyline || aPLB.DrawSymbol)
                                {
                                    DrawPolylineShape(g, aPLS, aPLB, LonShift, isStreamline);
                                }
                            }
                        }
                        break;
                    case LegendType.GraduatedColor:
                        vStr = aLayer.GetCellValue(aLS.FieldName, shapeIdx).ToString();
                        value = double.Parse(vStr);
                        blNum = 0;
                        for (int j = 0; j < aLS.LegendBreaks.Count; j++)
                        {
                            aPLB = (PolyLineBreak)aLS.LegendBreaks[j];
                            blNum += 1;
                            if (MIMath.DoubleEquals(value, double.Parse(aPLB.StartValue.ToString())) ||
                                (value > double.Parse(aPLB.StartValue.ToString()) && value < double.Parse(aPLB.EndValue.ToString())) ||
                                (blNum == aLS.LegendBreaks.Count && value == double.Parse(aPLB.EndValue.ToString())))
                            {
                                if (aPLB.DrawPolyline || aPLB.DrawSymbol)
                                {
                                    DrawPolylineShape(g, aPLS, aPLB, LonShift, isStreamline);
                                }
                            }
                        }
                        break;
                }
                shapeIdx += 1;
            }

            //Draw identifer shape
            if (_drawIdentiferShape)
            {
                PolylineShape aPLS = (PolylineShape)aLayer.ShapeList[aLayer.IdentiferShape];
                PolyLineBreak aPLB = new PolyLineBreak();
                aPLB.Color = Color.Red;
                aPLB.Size = 2;
                DrawPolylineShape(g, aPLS, aPLB, LonShift, isStreamline);
            }
        }

        //private void DrawPolylineZLayer(VectorLayer aLayer, Graphics g, double LonShift)
        //{
        //    int shapeIdx = 0;
        //    double value;
        //    int blNum = 0;
        //    LegendScheme aLS = aLayer.LegendScheme;
        //    PointD wPoint;
        //    Single X, Y;
        //    X = 0;
        //    Y = 0;
        //    Font drawFont = new Font("Arial", 7);
        //    bool isStreamline = false;

        //    switch (aLayer.LayerDrawType)
        //    {
        //        case LayerDrawType.TrajLine:
        //            //Draw start point symbol                                     
        //            PointF aPF = new PointF();
        //            foreach (PolylineZShape aPLS in aLayer.ShapeList)
        //            {
        //                wPoint = aPLS.Points[0].ToPointD();
        //                aPF.X = (Single)(wPoint.X + LonShift);
        //                aPF.Y = (Single)(wPoint.Y);
        //                if (MIMath.PointInExtent(aPF, _drawExtent))
        //                {
        //                    ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
        //                        LonShift);
        //                    aPF.X = X;
        //                    aPF.Y = Y;
        //                    Draw.DrawPoint(PointStyle.UpTriangle, aPF, this.ForeColor,
        //                        this.ForeColor, 10, true, true, g);
        //                }
        //            }
        //            break;
        //        case LayerDrawType.Streamline:
        //            isStreamline = true;
        //            break;
        //    }

        //    foreach (PolylineZShape aPLS in aLayer.ShapeList)
        //    {
        //        if (!aPLS.Visible)
        //            continue;

        //        PolyLineBreak aPLB = new PolyLineBreak();
        //        switch (aLS.LegendType)
        //        {
        //            case LegendType.SingleSymbol:
        //                string vStr = aLayer.GetCellValue(0, shapeIdx).ToString();
        //                aPLB = (PolyLineBreak)aLS.LegendBreaks[0];
        //                if (aPLB.DrawPolyline || aPLB.DrawSymbol)
        //                {
        //                    DrawPolylineShape(g, aPLS, aPLB, LonShift, isStreamline);
        //                }
        //                break;
        //            case LegendType.UniqueValue:
        //                vStr = aLayer.GetCellValue(aLS.FieldName, shapeIdx).ToString();
        //                for (int j = 0; j < aLS.LegendBreaks.Count; j++)
        //                {
        //                    aPLB = (PolyLineBreak)aLS.LegendBreaks[j];
        //                    if (vStr == aPLB.StartValue.ToString())
        //                    {
        //                        if (aPLB.DrawPolyline || aPLB.DrawSymbol)
        //                        {
        //                            DrawPolylineShape(g, aPLS, aPLB, LonShift, isStreamline);
        //                        }
        //                    }
        //                }
        //                break;
        //            case LegendType.GraduatedColor:
        //                vStr = aLayer.GetCellValue(aLS.FieldName, shapeIdx).ToString();
        //                value = double.Parse(vStr);
        //                blNum = 0;
        //                for (int j = 0; j < aLS.LegendBreaks.Count; j++)
        //                {
        //                    aPLB = (PolyLineBreak)aLS.LegendBreaks[j];
        //                    blNum += 1;
        //                    if (value == double.Parse(aPLB.StartValue.ToString()) || (value > double.Parse(aPLB.StartValue.ToString()) && value < double.Parse(aPLB.EndValue.ToString())) ||
        //                        (blNum == aLS.LegendBreaks.Count && value == double.Parse(aPLB.EndValue.ToString())))
        //                    {
        //                        if (aPLB.DrawPolyline || aPLB.DrawSymbol)
        //                        {
        //                            DrawPolylineShape(g, aPLS, aPLB, LonShift, isStreamline);
        //                        }
        //                    }
        //                }
        //                break;
        //        }
        //        shapeIdx += 1;
        //    }

        //    //Draw identifer shape
        //    if (_drawIdentiferShape)
        //    {
        //        PolylineShape aPLS = (PolylineShape)aLayer.ShapeList[aLayer.IdentiferShape];
        //        PolyLineBreak aPLB = new PolyLineBreak();
        //        aPLB.Color = Color.Red;
        //        aPLB.Size = 2;
        //        DrawPolylineShape(g, aPLS, aPLB, LonShift, isStreamline);
        //    }
        //}

        private void DrawPolylineShape(Graphics g, PolylineShape aPLS, PolyLineBreak aPLB, double LonShift,
            bool isStreamline)
        {
            DrawPolylineShape(g, aPLS, aPLB, LonShift, isStreamline, false);
        }

        private void DrawPolylineShape(Graphics g, PolylineZShape aPLS, PolyLineBreak aPLB, double LonShift,
            bool isStreamline)
        {
            DrawPolylineShape(g, aPLS, aPLB, LonShift, isStreamline, false);
        }

        //private void DrawPolylineShape_Old(Graphics g, PolylineShape aPLS, PolyLineBreak aPLB, double LonShift,
        //    bool isStreamline, bool isSelected)
        //{
        //    Extent shapeExtent = MIMath.ShiftExtentLon(aPLS.Extent, LonShift);
        //    if (!MIMath.IsExtentCross(shapeExtent, _drawExtent))
        //    {
        //        return;
        //    }

        //    PolylineShape dPLS = aPLS;
        //    if (aPLS.Extent.Width / _drawExtent.Width > 1000 || aPLS.Extent.Height / _drawExtent.Height > 1000)
        //    {
        //        dPLS = GeoComputation.ClipPolylineShape(aPLS, MIMath.ShiftExtentLon(_drawExtent, -LonShift));
        //        if (dPLS == null)
        //            return;
        //    }

        //    List<PointD> newPList = dPLS.Points;
        //    PointF[] Points = new PointF[newPList.Count];
        //    Single X = 0;
        //    Single Y = 0;
        //    PointF aPoint = new PointF(0, 0);
        //    int p, pp;
        //    PointF[] Pointps;
        //    for (int i = 0; i < newPList.Count; i++)
        //    {
        //        PointD wPoint = (PointD)newPList[i];
        //        ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
        //                LonShift);
        //        aPoint.X = X;
        //        aPoint.Y = Y;
        //        Points[i] = aPoint;
        //    }

        //    Pen aPen = new Pen(aPLB.Color);
        //    aPen.DashStyle = aPLB.DashStyle;
        //    if (aPLS.Selected)
        //        aPen.Color = _selectColor;
        //    aPen.Width = aPLB.Size;
        //    if (aPLB.DrawPolyline)
        //    {
        //        if (!(aPLS.PartNum > 1))
        //        {
        //            g.DrawLines(aPen, Points);
        //            if (isStreamline)
        //            {
        //                int len = (int)(aPLS.value * 3);
        //                for (int i = 0; i < Points.Length; i++)
        //                {
        //                    if (i > 0 && i < Points.Length - 2 && i % len == 0)
        //                    {
        //                        //Draw arraw
        //                        aPoint = Points[i];
        //                        PointF bPoint = Points[i + 1];
        //                        double U = bPoint.X - aPoint.X;
        //                        double V = bPoint.Y - aPoint.Y;
        //                        double angle = Math.Atan((V) / (U)) * 180 / Math.PI;
        //                        angle = angle + 90;
        //                        if (U < 0)
        //                            angle = angle + 180;

        //                        if (angle >= 360)
        //                            angle = angle - 360;

        //                        PointF eP1 = new PointF();
        //                        double aSize = 8;
        //                        eP1.X = (int)(aPoint.X - aSize * Math.Sin((angle + 20.0) * Math.PI / 180));
        //                        eP1.Y = (int)(aPoint.Y + aSize * Math.Cos((angle + 20.0) * Math.PI / 180));
        //                        g.DrawLine(aPen, aPoint, eP1);

        //                        eP1.X = (int)(aPoint.X - aSize * Math.Sin((angle - 20.0) * Math.PI / 180));
        //                        eP1.Y = (int)(aPoint.Y + aSize * Math.Cos((angle - 20.0) * Math.PI / 180));
        //                        g.DrawLine(aPen, aPoint, eP1);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            for (p = 0; p < dPLS.PartNum; p++)
        //            {
        //                if (p == dPLS.PartNum - 1)
        //                {
        //                    Pointps = new PointF[dPLS.PointNum - dPLS.parts[p]];
        //                    for (pp = dPLS.parts[p]; pp < aPLS.PointNum; pp++)
        //                    {
        //                        Pointps[pp - dPLS.parts[p]] = Points[pp];
        //                    }
        //                }
        //                else
        //                {
        //                    Pointps = new PointF[dPLS.parts[p + 1] - dPLS.parts[p]];
        //                    for (pp = dPLS.parts[p]; pp < dPLS.parts[p + 1]; pp++)
        //                    {
        //                        Pointps[pp - dPLS.parts[p]] = Points[pp];
        //                    }
        //                }
        //                g.DrawLines(aPen, Pointps);
        //            }
        //        }
        //    }

        //    //Draw symbol            
        //    if (aPLB.DrawSymbol)
        //    {
        //        SmoothingMode oldSMode = g.SmoothingMode;
        //        g.SmoothingMode = SmoothingMode.AntiAlias;
        //        for (int i = 0; i < Points.Length; i++)
        //        {
        //            if (i % aPLB.SymbolInterval == 0)
        //                Draw.DrawPoint(aPLB.SymbolStyle, Points[i], aPLB.SymbolColor, aPLB.SymbolColor, aPLB.SymbolSize, true, false, g);
        //        }
        //        g.SmoothingMode = oldSMode;
        //    }

        //    //Draw selected rectangle
        //    if (isSelected)
        //    {
        //        Extent aExtent = MIMath.GetPointFsExtent(Points);
        //        Rectangle rect = new Rectangle();
        //        rect.X = (int)aExtent.minX;
        //        rect.Y = (int)aExtent.minY;
        //        rect.Width = (int)(aExtent.maxX - aExtent.minX);
        //        rect.Height = (int)(aExtent.maxY - aExtent.minY);

        //        aPen = new Pen(Color.Cyan);
        //        aPen.DashStyle = DashStyle.Dash;
        //        g.DrawRectangle(aPen, rect);
        //    }
        //}

        private void DrawPolylineShape(Graphics g, PolylineShape aPLS, PolyLineBreak aPLB, double LonShift,
            bool isStreamline, bool isSelected)
        {
            Extent shapeExtent = MIMath.ShiftExtentLon(aPLS.Extent, LonShift);
            if (!MIMath.IsExtentCross(shapeExtent, _drawExtent))
            {
                return;
            }

            List<PointD> newPList = aPLS.Points;
            PointF[] Points = new PointF[newPList.Count];
            Single X = 0;
            Single Y = 0;
            PointF aPoint = new PointF(0, 0);
            int p, pp;
            PointF[] Pointps;
            for (int i = 0; i < newPList.Count; i++)
            {
                PointD wPoint = (PointD)newPList[i];
                ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
                        LonShift);
                aPoint.X = X;
                aPoint.Y = Y;
                Points[i] = aPoint;
            }

            Pen aPen = new Pen(aPLB.Color);
            aPen.DashStyle = aPLB.DashStyle;
            if (aPLS.Selected)
                aPen.Color = _selectColor;
            aPen.Width = aPLB.Size;
            if (aPLB.DrawPolyline)
            {
                if (!(aPLS.PartNum > 1))
                {
                    g.DrawLines(aPen, Points);
                    if (isStreamline)
                    {
                        int len = (int)(aPLS.value * 3);
                        for (int i = 0; i < Points.Length; i++)
                        {
                            if (i > 0 && i < Points.Length - 2 && i % len == 0)
                            {
                                //Draw arraw
                                aPoint = Points[i];
                                PointF bPoint = Points[i + 1];
                                double U = bPoint.X - aPoint.X;
                                double V = bPoint.Y - aPoint.Y;
                                double angle = Math.Atan((V) / (U)) * 180 / Math.PI;
                                if (Double.IsNaN(angle))
                                    continue;

                                angle = angle + 90;
                                if (U < 0)
                                    angle = angle + 180;

                                if (angle >= 360)
                                    angle = angle - 360;

                                PointF eP1 = new PointF();
                                double aSize = 8;
                                eP1.X = (int)(aPoint.X - aSize * Math.Sin((angle + 20.0) * Math.PI / 180));
                                eP1.Y = (int)(aPoint.Y + aSize * Math.Cos((angle + 20.0) * Math.PI / 180));
                                g.DrawLine(aPen, aPoint, eP1);

                                eP1.X = (int)(aPoint.X - aSize * Math.Sin((angle - 20.0) * Math.PI / 180));
                                eP1.Y = (int)(aPoint.Y + aSize * Math.Cos((angle - 20.0) * Math.PI / 180));
                                g.DrawLine(aPen, aPoint, eP1);
                            }
                        }
                    }
                }
                else
                {
                    for (p = 0; p < aPLS.PartNum; p++)
                    {
                        if (p == aPLS.PartNum - 1)
                        {
                            Pointps = new PointF[aPLS.PointNum - aPLS.parts[p]];
                            for (pp = aPLS.parts[p]; pp < aPLS.PointNum; pp++)
                            {
                                Pointps[pp - aPLS.parts[p]] = Points[pp];
                            }
                        }
                        else
                        {
                            Pointps = new PointF[aPLS.parts[p + 1] - aPLS.parts[p]];
                            for (pp = aPLS.parts[p]; pp < aPLS.parts[p + 1]; pp++)
                            {
                                Pointps[pp - aPLS.parts[p]] = Points[pp];
                            }
                        }
                        g.DrawLines(aPen, Pointps);
                    }
                }
            }

            //Draw symbol            
            if (aPLB.DrawSymbol)
            {
                SmoothingMode oldSMode = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                for (int i = 0; i < Points.Length; i++)
                {
                    if (i % aPLB.SymbolInterval == 0)
                        Draw.DrawPoint(aPLB.SymbolStyle, Points[i], aPLB.SymbolColor, aPLB.SymbolColor, aPLB.SymbolSize, true, false, g);
                }
                g.SmoothingMode = oldSMode;
            }

            //Draw selected rectangle
            if (isSelected)
            {
                Extent aExtent = MIMath.GetPointFsExtent(Points);
                Rectangle rect = new Rectangle();
                rect.X = (int)aExtent.minX;
                rect.Y = (int)aExtent.minY;
                rect.Width = (int)(aExtent.maxX - aExtent.minX);
                rect.Height = (int)(aExtent.maxY - aExtent.minY);

                aPen = new Pen(Color.Cyan);
                aPen.DashStyle = DashStyle.Dash;
                g.DrawRectangle(aPen, rect);                
            }

            aPen.Dispose();
        }

        //private void DrawPolylineShape(Graphics g, PolylineZShape aPLS, PolyLineBreak aPLB, double LonShift,
        //    bool isStreamline, bool isSelected)
        //{
        //    Extent shapeExtent = MIMath.ShiftExtentLon(aPLS.Extent, LonShift);
        //    if (!MIMath.IsExtentCross(shapeExtent, _drawExtent))
        //    {
        //        return;
        //    }

        //    List<PointZ> newPList = aPLS.Points;
        //    PointF[] Points = new PointF[newPList.Count];
        //    Single X = 0;
        //    Single Y = 0;
        //    PointF aPoint = new PointF(0, 0);
        //    int p, pp;
        //    PointF[] Pointps;
        //    for (int i = 0; i < newPList.Count; i++)
        //    {
        //        PointD wPoint = newPList[i].ToPointD();
        //        ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
        //                LonShift);
        //        aPoint.X = X;
        //        aPoint.Y = Y;
        //        Points[i] = aPoint;
        //    }

        //    Pen aPen = new Pen(aPLB.Color);
        //    aPen.DashStyle = aPLB.DashStyle;
        //    if (aPLS.Selected)
        //        aPen.Color = _selectColor;
        //    aPen.Width = aPLB.Size;
        //    if (aPLB.DrawPolyline)
        //    {
        //        if (!(aPLS.PartNum > 1))
        //        {
        //            g.DrawLines(aPen, Points);
        //            if (isStreamline)
        //            {
        //                int len = (int)(aPLS.value * 3);
        //                for (int i = 0; i < Points.Length; i++)
        //                {
        //                    if (i > 0 && i < Points.Length - 2 && i % len == 0)
        //                    {
        //                        //Draw arraw
        //                        aPoint = Points[i];
        //                        PointF bPoint = Points[i + 1];
        //                        double U = bPoint.X - aPoint.X;
        //                        double V = bPoint.Y - aPoint.Y;
        //                        double angle = Math.Atan((V) / (U)) * 180 / Math.PI;
        //                        if (Double.IsNaN(angle))
        //                            continue;

        //                        angle = angle + 90;
        //                        if (U < 0)
        //                            angle = angle + 180;

        //                        if (angle >= 360)
        //                            angle = angle - 360;

        //                        PointF eP1 = new PointF();
        //                        double aSize = 8;
        //                        eP1.X = (int)(aPoint.X - aSize * Math.Sin((angle + 20.0) * Math.PI / 180));
        //                        eP1.Y = (int)(aPoint.Y + aSize * Math.Cos((angle + 20.0) * Math.PI / 180));
        //                        g.DrawLine(aPen, aPoint, eP1);

        //                        eP1.X = (int)(aPoint.X - aSize * Math.Sin((angle - 20.0) * Math.PI / 180));
        //                        eP1.Y = (int)(aPoint.Y + aSize * Math.Cos((angle - 20.0) * Math.PI / 180));
        //                        g.DrawLine(aPen, aPoint, eP1);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            for (p = 0; p < aPLS.PartNum; p++)
        //            {
        //                if (p == aPLS.PartNum - 1)
        //                {
        //                    Pointps = new PointF[aPLS.PointNum - aPLS.parts[p]];
        //                    for (pp = aPLS.parts[p]; pp < aPLS.PointNum; pp++)
        //                    {
        //                        Pointps[pp - aPLS.parts[p]] = Points[pp];
        //                    }
        //                }
        //                else
        //                {
        //                    Pointps = new PointF[aPLS.parts[p + 1] - aPLS.parts[p]];
        //                    for (pp = aPLS.parts[p]; pp < aPLS.parts[p + 1]; pp++)
        //                    {
        //                        Pointps[pp - aPLS.parts[p]] = Points[pp];
        //                    }
        //                }
        //                g.DrawLines(aPen, Pointps);
        //            }
        //        }
        //    }

        //    //Draw symbol            
        //    if (aPLB.DrawSymbol)
        //    {
        //        SmoothingMode oldSMode = g.SmoothingMode;
        //        g.SmoothingMode = SmoothingMode.AntiAlias;
        //        for (int i = 0; i < Points.Length; i++)
        //        {
        //            if (i % aPLB.SymbolInterval == 0)
        //                Draw.DrawPoint(aPLB.SymbolStyle, Points[i], aPLB.SymbolColor, aPLB.SymbolColor, aPLB.SymbolSize, true, false, g);
        //        }
        //        g.SmoothingMode = oldSMode;
        //    }

        //    //Draw selected rectangle
        //    if (isSelected)
        //    {
        //        Extent aExtent = MIMath.GetPointFsExtent(Points);
        //        Rectangle rect = new Rectangle();
        //        rect.X = (int)aExtent.minX;
        //        rect.Y = (int)aExtent.minY;
        //        rect.Width = (int)(aExtent.maxX - aExtent.minX);
        //        rect.Height = (int)(aExtent.maxY - aExtent.minY);

        //        aPen = new Pen(Color.Cyan);
        //        aPen.DashStyle = DashStyle.Dash;
        //        g.DrawRectangle(aPen, rect);
        //    }
        //}

        private void DrawLonLatPolylineShape(Graphics g, PolylineShape aPLS, PolyLineBreak aPLB, double LonShift)
        {
            Extent shapeExtent = MIMath.ShiftExtentLon(aPLS.Extent, LonShift);
            if (!MIMath.IsExtentCross(shapeExtent, _drawExtent))
            {
                return;
            }

            List<PointD> newPList = aPLS.Points;
            PointF[] Points = new PointF[newPList.Count];
            Single X = 0;
            Single Y = 0;
            PointF aPoint = new PointF(0, 0);            
            for (int i = 0; i < newPList.Count; i++)
            {
                PointD wPoint = newPList[i];
                ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
                        LonShift);
                aPoint.X = X;
                aPoint.Y = Y;
                Points[i] = aPoint;
            }

            Pen aPen = new Pen(aPLB.Color);
            aPen.DashStyle = aPLB.DashStyle;
            if (aPLS.Selected)
                aPen.Color = _selectColor;
            aPen.Width = aPLB.Size;

            if (!(aPLS.PartNum > 1))
            {
                int p = 0;
                for (int i = 1; i < Points.Length; i++)
                {
                    //if (!MIMath.PointInExtent(Points[i], _drawExtent) && !MIMath.PointInExtent(Points[p], _drawExtent))
                    //{
                    //    p = i;
                    //    continue;
                    //}

                    g.DrawLines(aPen, new PointF[] { Points[p], Points[i] });
                    p = i;
                }
            }
            else
            {
                int p, pp;
                PointF[] Pointps;
                for (p = 0; p < aPLS.PartNum; p++)
                {
                    if (p == aPLS.PartNum - 1)
                    {
                        Pointps = new PointF[aPLS.PointNum - aPLS.parts[p]];
                        for (pp = aPLS.parts[p]; pp < aPLS.PointNum; pp++)
                        {
                            Pointps[pp - aPLS.parts[p]] = Points[pp];
                        }
                    }
                    else
                    {
                        Pointps = new PointF[aPLS.parts[p + 1] - aPLS.parts[p]];
                        for (pp = aPLS.parts[p]; pp < aPLS.parts[p + 1]; pp++)
                        {
                            Pointps[pp - aPLS.parts[p]] = Points[pp];
                        }
                    }

                    int f = 0;
                    for (int i = 1; i < Pointps.Length; i++)
                    {
                        //if (!MIMath.PointInExtent(Points[i], _drawExtent) && !MIMath.PointInExtent(Points[p], _drawExtent))
                        //{
                        //    p = i;
                        //    continue;
                        //}

                        g.DrawLines(aPen, new PointF[] { Pointps[f], Pointps[i] });
                        f = i;
                    }
                }
            }

            aPen.Dispose();
        }

        //private void DrawPolylineShape_Back(Graphics g, PolylineShape aPLS, PolyLineBreak aPLB, double LonShift, Extent sExtent,
        //    MapExtentSet aLLSS, bool showValue, string labStr, Font drawFont)
        //{
        //    Extent shapeExtent = MIMath.ShiftExtentLon(aPLS.Extent, LonShift);
        //    if (!MIMath.IsExtentCross(shapeExtent, sExtent))
        //    {
        //        return;
        //    }

        //    List<PointD> newPList = aPLS.Points;
        //    PointF[] Points = new PointF[newPList.Count];
        //    int labIdx = newPList.Count / 2;
        //    Single maxX = 0;
        //    Single minX = 1000;
        //    Single X = 0;
        //    Single Y = 0;
        //    PointF aPoint = new PointF(0, 0);
        //    PointF labPoint = new PointF(0, 0);
        //    int p, pp;
        //    PointF[] Pointps;
        //    for (int i = 0; i < newPList.Count; i++)
        //    {
        //        PointD wPoint = (PointD)newPList[i];
        //        aLLSS.ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
        //                LonShift, aLLSS);
        //        aPoint.X = X;
        //        aPoint.Y = Y;
        //        Points[i] = aPoint;
        //        if (i == labIdx)
        //        {
        //            labPoint.X = aPoint.X;
        //            labPoint.Y = aPoint.Y;
        //        }
        //        if (aPoint.X < minX)
        //        {
        //            minX = aPoint.X;
        //        }
        //        if (aPoint.X > maxX)
        //        {
        //            maxX = aPoint.X;
        //        }
        //    }
        //    Single xLength = maxX - minX;

        //    Pen aPen = new Pen(aPLB.Color);
        //    aPen.DashStyle = aPLB.DashStyle;
        //    aPen.Color = aPLB.Color;
        //    aPen.Width = aPLB.Size;
        //    if (aPLB.DrawPolyline)
        //    {
        //        if (!(aPLS.PartNum > 1))
        //        {
        //            g.DrawLines(aPen, Points);
        //        }
        //        else
        //        {
        //            for (p = 0; p < aPLS.PartNum; p++)
        //            {
        //                if (p == aPLS.PartNum - 1)
        //                {
        //                    Pointps = new PointF[aPLS.PointNum - aPLS.parts[p]];
        //                    for (pp = aPLS.parts[p]; pp < aPLS.PointNum; pp++)
        //                    {
        //                        Pointps[pp - aPLS.parts[p]] = Points[pp];
        //                    }
        //                }
        //                else
        //                {
        //                    Pointps = new PointF[aPLS.parts[p + 1] - aPLS.parts[p]];
        //                    for (pp = aPLS.parts[p]; pp < aPLS.parts[p + 1]; pp++)
        //                    {
        //                        Pointps[pp - aPLS.parts[p]] = Points[pp];
        //                    }
        //                }
        //                g.DrawLines(aPen, Pointps);
        //            }
        //        }

        //        if (showValue)
        //        {
        //            SolidBrush aBrush = new SolidBrush(aPLB.Color);
        //            SizeF labSize = g.MeasureString(labStr, drawFont);
        //            labPoint.X = labPoint.X - (int)labSize.Width / 2;
        //            labPoint.Y = labPoint.Y - (int)labSize.Height / 2;
        //            if (xLength > this.Width / 10)
        //            {
        //                g.FillRectangle(new SolidBrush(this.BackColor), labPoint.X, labPoint.Y,
        //                    (int)labSize.Width, (int)labSize.Height);
        //                g.DrawString(labStr, drawFont, aBrush, labPoint);
        //            }
        //        }
        //    }
        //}

        private void DrawPolygonShape(Graphics g, PolygonShape aPGS, PolygonBreak aPGB, double LonShift,
            int transparencyPerc)
        {
            DrawPolygonShape(g, aPGS, aPGB, LonShift, false);
        }

        //private void DrawPolygonShape(Graphics g, PolygonShape aPGS, PolygonBreak aPGB, double LonShift,
        //    bool isSelected)
        //{
        //    DrawPolygonShape(g, aPGS, aPGB, LonShift, isSelected);
        //}

        private void DrawPolygonShape_Back(Graphics g, PolygonShape aPGS, PolygonBreak aPGB, double LonShift,
            int transparencyPerc, bool isSelected)
        {
            Extent shapeExtent = MIMath.ShiftExtentLon(aPGS.Extent, LonShift);
            if (!MIMath.IsExtentCross(shapeExtent, _drawExtent))
            {
                return;
            }

            List<PointD> newPList = aPGS.Points;
            PointF[] Points = new PointF[newPList.Count];
            PointD wPoint = new PointD();
            Single X = 0;
            Single Y = 0;
            PointF aPoint = new PointF(0, 0);
            int p, pp;
            PointF[] Pointps;
            for (int i = 0; i < newPList.Count; i++)
            {
                wPoint = (PointD)newPList[i];
                ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
                        LonShift);
                aPoint.X = X;
                aPoint.Y = Y;
                Points[i] = aPoint;
            }

            if (aPGB.DrawFill)
            {
                int alpha = (int)((1 - (double)transparencyPerc / 100.0) * 255);
                Color aColor = Color.FromArgb(alpha, aPGB.Color);
                Brush aBrush;
                if (aPGB.UsingHatchStyle)
                    aBrush = new HatchBrush(aPGB.Style, aColor, aPGB.BackColor);
                else
                    aBrush = new SolidBrush(aColor);

                if (!(aPGS.PartNum > 1))
                {
                    g.FillPolygon(aBrush, Points);
                }
                else
                {
                    for (p = 0; p < aPGS.PartNum; p++)
                    {
                        if (p == aPGS.PartNum - 1)
                        {
                            Pointps = new PointF[aPGS.PointNum - aPGS.parts[p]];
                            for (pp = aPGS.parts[p]; pp < aPGS.PointNum; pp++)
                            {
                                Pointps[pp - aPGS.parts[p]] = Points[pp];
                            }
                        }
                        else
                        {
                            Pointps = new PointF[aPGS.parts[p + 1] - aPGS.parts[p]];
                            for (pp = aPGS.parts[p]; pp < aPGS.parts[p + 1]; pp++)
                            {
                                Pointps[pp - aPGS.parts[p]] = Points[pp];
                            }
                        }
                        g.FillPolygon(aBrush, Pointps);
                    }
                }
            }
            if (aPGB.DrawOutline)
            {
                Pen aPen = new Pen(aPGB.OutlineColor);
                aPen.Width = aPGB.OutlineSize;
                if (!(aPGS.PartNum > 1))
                {
                    g.DrawPolygon(aPen, Points);
                }
                else
                {
                    for (p = 0; p < aPGS.PartNum; p++)
                    {
                        if (p == aPGS.PartNum - 1)
                        {
                            Pointps = new PointF[aPGS.PointNum - aPGS.parts[p]];
                            for (pp = aPGS.parts[p]; pp < aPGS.PointNum; pp++)
                            {
                                Pointps[pp - aPGS.parts[p]] = Points[pp];
                            }
                        }
                        else
                        {
                            Pointps = new PointF[aPGS.parts[p + 1] - aPGS.parts[p]];
                            for (pp = aPGS.parts[p]; pp < aPGS.parts[p + 1]; pp++)
                            {
                                Pointps[pp - aPGS.parts[p]] = Points[pp];
                            }
                        }
                        g.DrawPolygon(aPen, Pointps);
                    }
                }
            }

            //Draw selected rectangle
            if (isSelected)
            {
                Extent aExtent = MIMath.GetPointFsExtent(Points);
                Rectangle rect = new Rectangle();
                rect.X = (int)aExtent.minX;
                rect.Y = (int)aExtent.minY;
                rect.Width = (int)(aExtent.maxX - aExtent.minX);
                rect.Height = (int)(aExtent.maxY - aExtent.minY);

                Pen aPen = new Pen(Color.Red);
                aPen.DashStyle = DashStyle.Dash;
                g.DrawRectangle(aPen, rect);
            }
        }

        private void DrawPolygonShape(Graphics g, PolygonShape aPGS, PolygonBreak aPGB, double LonShift,
            bool isSelected)
        {
            Extent shapeExtent = MIMath.ShiftExtentLon(aPGS.Extent, LonShift);
            if (!MIMath.IsExtentCross(shapeExtent, _drawExtent))
            {
                return;
            }

            List<PointF> pointList = new List<PointF>();
            foreach (Polygon aPolygon in aPGS.Polygons)
            {
                pointList.AddRange(DrawPolygon(g, aPolygon, aPGB, LonShift, aPGS.Selected));
            }

            //Draw selected rectangle
            if (isSelected)
            {
                Extent aExtent = MIMath.GetPointFsExtent(pointList);
                Rectangle rect = new Rectangle();
                rect.X = (int)aExtent.minX;
                rect.Y = (int)aExtent.minY;
                rect.Width = (int)(aExtent.maxX - aExtent.minX);
                rect.Height = (int)(aExtent.maxY - aExtent.minY);

                Pen aPen = new Pen(Color.Red);
                aPen.DashStyle = DashStyle.Dash;
                g.DrawRectangle(aPen, rect);
                aPen.Dispose();
            }
        }

        private List<PointF> DrawPolygon_Back(Graphics g, Polygon aPG, PolygonBreak aPGB, double LonShift,
            int transparencyPerc, bool isSelected)
        {            
            PointF[] Points = new PointF[aPG.OutLine.Count];
            PointD wPoint = new PointD();
            float X = 0;
            float Y = 0;
            for (int i = 0; i < aPG.OutLine.Count; i++)
            {
                wPoint = aPG.OutLine[i];
                ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y, LonShift);                
                Points[i] = new PointF(X, Y);
            }
            List<PointF> rPoints = new List<PointF>(Points);
                        
            GraphicsPath bPath = new GraphicsPath();
            GraphicsPath aPath = new GraphicsPath();
            aPath.AddPolygon(Points);
            bPath.AddLines(Points);
            Region aRegion = new Region(aPath);
            List<PointD> newPList = new List<PointD>();
            if (aPG.HasHole)
            {
                for (int h = 0; h < aPG.HoleLines.Count; h++)
                {
                    newPList = aPG.HoleLines[h];
                    Points = new PointF[newPList.Count];
                    for (int j = 0; j < newPList.Count; j++)
                    {
                        wPoint = newPList[j];
                        ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y, LonShift);
                        Points[j] = new PointF(X, Y);
                    }
                    aPath = new GraphicsPath();
                    aPath.AddPolygon(Points);
                    GraphicsPath cPath = new GraphicsPath();
                    cPath.AddLines(Points);
                    bPath.AddPath(cPath, false);
                    aRegion.Xor(aPath);
                }
            }

            if (aPGB.DrawFill)
            {
                int alpha = (int)((1 - (double)transparencyPerc / 100.0) * 255);
                Color aColor = Color.FromArgb(alpha, aPGB.Color);
                Brush aBrush;
                if (aPGB.UsingHatchStyle)
                    aBrush = new HatchBrush(aPGB.Style, aColor, aPGB.BackColor);
                else
                    aBrush = new SolidBrush(aColor);

                g.FillRegion(aBrush, aRegion);
            }

            if (aPGB.DrawOutline)
            {
                Pen aPen = new Pen(aPGB.OutlineColor);
                aPen.Width = aPGB.OutlineSize;
                g.DrawPath(aPen, bPath);
            }

            return rPoints;
        }

        private List<PointF> DrawPolygon(Graphics g, Polygon aPG, PolygonBreak aPGB, double LonShift,
            bool isSelected)
        {
            PointF[] Points = new PointF[aPG.OutLine.Count];
            PointD wPoint = new PointD();
            float X = 0;
            float Y = 0;
            for (int i = 0; i < aPG.OutLine.Count; i++)
            {
                wPoint = aPG.OutLine[i];
                ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y, LonShift);
                Points[i] = new PointF(X, Y);
            }
            List<PointF> rPoints = new List<PointF>(Points);

            GraphicsPath bPath = new GraphicsPath();
            GraphicsPath aPath = new GraphicsPath();
            aPath.AddPolygon(Points);
            bPath.AddLines(Points);
            //Region aRegion = new Region(aPath);
            List<PointD> newPList = new List<PointD>();
            if (aPG.HasHole)
            {
                for (int h = 0; h < aPG.HoleLines.Count; h++)
                {
                    newPList = aPG.HoleLines[h];
                    Points = new PointF[newPList.Count];
                    for (int j = 0; j < newPList.Count; j++)
                    {
                        wPoint = newPList[j];
                        ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y, LonShift);
                        Points[j] = new PointF(X, Y);
                    }
                    //aPath = new GraphicsPath();
                    aPath.AddPolygon(Points);
                    GraphicsPath cPath = new GraphicsPath();
                    cPath.AddLines(Points);
                    bPath.AddPath(cPath, false);
                    //aRegion.Xor(aPath);
                }
            }

            if (aPGB.DrawFill)
            {
                //int alpha = (int)((1 - (double)transparencyPerc / 100.0) * 255);
                //Color aColor = Color.FromArgb(alpha, aPGB.Color);
                Color aColor = aPGB.Color;
                if (isSelected)
                    aColor = _selectColor;
                Brush aBrush;
                if (aPGB.UsingHatchStyle)
                    aBrush = new HatchBrush(aPGB.Style, aColor, aPGB.BackColor);
                else
                    aBrush = new SolidBrush(aColor);

                //g.FillRegion(aBrush, aRegion);
                g.FillPath(aBrush, aPath);
                aBrush.Dispose();
            }
            else
            {
                if (isSelected)
                    g.FillPath(new SolidBrush(_selectColor), aPath);
            }

            if (aPGB.DrawOutline)
            {
                Pen aPen = new Pen(aPGB.OutlineColor);
                aPen.Width = aPGB.OutlineSize;
                g.DrawPath(aPen, bPath);
                aPen.Dispose();
            }

            aPath.Dispose();
            bPath.Dispose();            

            return rPoints;
        }

        /// <summary>
        /// Draw identifer shape
        /// </summary>
        /// <param name="g">graphis</param>
        /// <param name="aShape">a shape</param>
        public void DrawIdShape(Graphics g, Shape.Shape aShape)
        {
            List<double> lonShifts = new List<double>();
            if (MIMath.IsExtentCross(this.ViewExtent, aShape.Extent))
                lonShifts.Add(0);
            if (MIMath.IsExtentCross(this.ViewExtent, MIMath.ShiftExtentLon(aShape.Extent, 360)))
                lonShifts.Add(360);
            if (MIMath.IsExtentCross(this.ViewExtent, MIMath.ShiftExtentLon(aShape.Extent, -360)))
                lonShifts.Add(-360);

            foreach (double LonShift in lonShifts)
            {
                switch (aShape.ShapeType)
                {
                    case ShapeTypes.Point:
                    case ShapeTypes.PointM:
                    case ShapeTypes.PointZ:
                        float X = 0;
                        float Y = 0;
                        PointShape aPS = (PointShape)aShape;
                        ProjToScreen(aPS.Point.X, aPS.Point.Y, ref X, ref Y,
                                            LonShift);
                        PointF aPoint = new PointF();
                        aPoint.X = X;
                        aPoint.Y = Y;
                        PointBreak aPB = new PointBreak();
                        aPB.OutlineColor = Color.Red;
                        aPB.Size = 10;
                        aPB.Style = PointStyle.Square;
                        aPB.DrawFill = false;

                        Draw.DrawPoint(aPoint, aPB, g);
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineM:
                        PolylineShape aPLS = (PolylineShape)aShape;
                        PolyLineBreak aPLB = new PolyLineBreak();
                        aPLB.Color = Color.Red;
                        aPLB.Size = 2;
                        DrawPolylineShape(g, aPLS, aPLB, LonShift, false);
                        break;
                    case ShapeTypes.PolylineZ:
                        PolylineZShape aPLZS = (PolylineZShape)aShape;
                        aPLB = new PolyLineBreak();
                        aPLB.Color = Color.Red;
                        aPLB.Size = 2;
                        DrawPolylineShape(g, aPLZS, aPLB, LonShift, false);
                        break;
                    case ShapeTypes.Polygon:
                    case ShapeTypes.PolygonM:
                        PolygonShape aPGS = (PolygonShape)aShape;
                        PolygonBreak aPGB = new PolygonBreak();
                        aPGB.OutlineColor = Color.Red;
                        aPGB.OutlineSize = 2;
                        aPGB.Color = Color.Red;
                        DrawPolygonShape(g, aPGS, aPGB, LonShift, 50);
                        break;
                }
            }
        }

        /// <summary>
        /// Draw identifer shape
        /// </summary>
        /// <param name="g">graphis</param>
        /// <param name="aShape">a shape</param>
        /// <param name="rect">rectangle extent</param>
        public void DrawIdShape(Graphics g, Shape.Shape aShape, Rectangle rect)
        {
            List<double> lonShifts = new List<double>();
            if (MIMath.IsExtentCross(this.ViewExtent, aShape.Extent))
                lonShifts.Add(0);
            if (MIMath.IsExtentCross(this.ViewExtent, MIMath.ShiftExtentLon(aShape.Extent, 360)))
                lonShifts.Add(360);
            if (MIMath.IsExtentCross(this.ViewExtent, MIMath.ShiftExtentLon(aShape.Extent, -360)))
                lonShifts.Add(-360);

            //RefreshXYScale(rect.Width, rect.Height);

            Matrix oldMatrix = g.Transform;
            Region oldRegion = g.Clip;
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(rect);
            g.SetClip(path);

            GetMaskOutGraphicsPath(g);

            g.TranslateTransform(rect.X, rect.Y);

            foreach (double LonShift in lonShifts)
            {
                switch (aShape.ShapeType)
                {
                    case ShapeTypes.Point:
                    case ShapeTypes.PointM:
                    case ShapeTypes.PointZ:
                        float X = 0;
                        float Y = 0;
                        PointShape aPS = (PointShape)aShape;
                        ProjToScreen(aPS.Point.X, aPS.Point.Y, ref X, ref Y,
                                            LonShift);
                        PointF aPoint = new PointF();
                        aPoint.X = X;
                        aPoint.Y = Y;
                        PointBreak aPB = new PointBreak();
                        aPB.OutlineColor = Color.Red;
                        aPB.Size = 10;
                        aPB.Style = PointStyle.Square;
                        aPB.DrawFill = false;

                        Draw.DrawPoint(aPoint, aPB, g);
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineM:
                    case ShapeTypes.PolylineZ:
                        PolylineShape aPLS = (PolylineShape)aShape;
                        PolyLineBreak aPLB = new PolyLineBreak();
                        aPLB.Color = Color.Red;
                        aPLB.Size = 2;
                        DrawPolylineShape(g, aPLS, aPLB, LonShift, false);
                        break;
                    case ShapeTypes.Polygon:
                    case ShapeTypes.PolygonM:
                        PolygonShape aPGS = (PolygonShape)aShape;
                        PolygonBreak aPGB = new PolygonBreak();
                        aPGB.OutlineColor = Color.Red;
                        aPGB.OutlineSize = 2;
                        aPGB.Color = Color.Red;
                        DrawPolygonShape(g, aPGS, aPGB, LonShift, 50);
                        break;
                }
            }

            g.Transform = oldMatrix;
            g.Clip = oldRegion;
            path.Dispose();
            oldMatrix.Dispose();
            oldRegion.Dispose();
        }

        private void DrawLayerLabels(Graphics g, VectorLayer aLayer, double LonShift)
        {
            Extent lExtent = MIMath.ShiftExtentLon(aLayer.Extent, LonShift);
            if (!MIMath.IsExtentCross(lExtent, _drawExtent))
            {
                return;
            }

            LegendScheme aLS = aLayer.LegendScheme;
            List<Shape.Shape> shapeList = new List<MeteoInfoC.Shape.Shape>(aLayer.ShapeList);
            
            Font drawFont = aLayer.LabelSet.LabelFont;
            SolidBrush labelBrush = new SolidBrush(aLayer.LabelSet.LabelColor);

            List<Extent> extentList = new List<Extent>();
            Extent maxExtent = new Extent();
            Extent aExtent = new Extent();
            int i, j;
            List<Graphic> LabelPoints = aLayer.GetLabelPoints();
            string LabelStr;
            PointF aPoint = new PointF();
            Single X, Y;
            X = 0;
            Y = 0;
            Single angle;
            Matrix myMatrix = new Matrix();

            for (i = 0; i < LabelPoints.Count; i++)
            {
                Graphic aLP = LabelPoints[i];
                PointShape aPS = (PointShape)aLP.Shape;
                LabelBreak aLB = (LabelBreak)aLP.Legend;
                aPS.Visible = true;
                LabelStr = aLB.Text;
                aPoint.X = (float)aPS.Point.X;
                aPoint.Y = (float)aPS.Point.Y;
                angle = aLB.Angle;
                labelBrush.Color = aLB.Color;
                drawFont = aLB.Font;
                if (aPoint.X + LonShift < _drawExtent.minX || aPoint.X + LonShift > _drawExtent.maxX
                            || aPoint.Y < _drawExtent.minY || aPoint.Y > _drawExtent.maxY)
                {
                    continue;
                }
                ProjToScreen(aPoint.X, aPoint.Y, ref X, ref Y, LonShift);
                aPoint.X = X;
                aPoint.Y = Y;
                SizeF labSize = g.MeasureString(LabelStr, drawFont);
                switch (aLB.AlignType)
                {
                    case AlignType.Center:
                        aPoint.X = X - labSize.Width / 2;
                        break;
                    case AlignType.Left:
                        aPoint.X = X - labSize.Width;
                        break;
                }
                aPoint.Y -= aLB.YShift;
                aPoint.X += aLB.XShift;
                
                if (angle != 0)
                {
                    //g.RotateTransform(angle);                    
                    myMatrix.RotateAt(angle, aPoint, MatrixOrder.Append);
                    g.Transform = myMatrix;
                }

                bool ifDraw = true;
                Single aSize = labSize.Width / 2;
                Single bSize = labSize.Height / 2;
                aExtent.minX = aPoint.X;
                aExtent.maxX = aPoint.X + labSize.Width;
                aExtent.minY = aPoint.Y - labSize.Height;
                aExtent.maxY = aPoint.Y;
                if (aLayer.LabelSet.AvoidCollision)
                {
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
                            for (j = 0; j < extentList.Count; j++)
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
                            else
                                aPS.Visible = false;
                        }
                    }
                }                

                if (ifDraw)
                {                    
                    if (aLayer.LabelSet.DrawShadow)
                    {
                        g.FillRectangle(new SolidBrush(aLayer.LabelSet.ShadowColor), aPoint.X, aPoint.Y,
                            labSize.Width, labSize.Height);
                    }
                    g.DrawString(LabelStr, drawFont, labelBrush, aPoint);

                    //Draw selected rectangle
                    if (aPS.Selected)
                    {
                        Pen aPen = new Pen(Color.Cyan);
                        aPen.DashStyle = DashStyle.Dash;
                        g.DrawRectangle(aPen, (float)aExtent.minX, (float)aExtent.maxY, labSize.Width, labSize.Height);
                        aPen.Dispose();
                    }
                }

                if (angle != 0)
                {
                    //g.RotateTransform(-angle);
                    g.Transform = new Matrix(); 
                }
            }

            myMatrix.Dispose();
            //drawFont.Dispose();

        }

        private void DrawLayerCharts(Graphics g, VectorLayer aLayer, double LonShift)
        {
            Extent lExtent = MIMath.ShiftExtentLon(aLayer.Extent, LonShift);
            if (!MIMath.IsExtentCross(lExtent, _drawExtent))
            {
                return;
            }

            LegendScheme aLS = aLayer.LegendScheme;
            List<Shape.Shape> shapeList = new List<MeteoInfoC.Shape.Shape>(aLayer.ShapeList);

            //Font drawFont = aLayer.LabelSet.LabelFont;
            //SolidBrush labelBrush = new SolidBrush(aLayer.LabelSet.LabelColor);

            List<Extent> extentList = new List<Extent>();
            Extent maxExtent = new Extent();
            Extent aExtent = new Extent();
            int i, j;
            List<Graphic> chartPoints = aLayer.ChartPoints;
            PointF aPoint = new PointF();
            Single X, Y;
            X = 0;
            Y = 0;

            for (i = 0; i < chartPoints.Count; i++)
            {
                Graphic aCP = chartPoints[i];
                PointShape aPS = (PointShape)aCP.Shape;
                ChartBreak aCB = (ChartBreak)aCP.Legend;
                aPS.Visible = true;
                aPoint.X = (float)aPS.Point.X;
                aPoint.Y = (float)aPS.Point.Y;
                if (aPoint.X + LonShift < _drawExtent.minX || aPoint.X + LonShift > _drawExtent.maxX
                            || aPoint.Y < _drawExtent.minY || aPoint.Y > _drawExtent.maxY)
                {
                    continue;
                }
                ProjToScreen(aPoint.X, aPoint.Y, ref X, ref Y, LonShift);
                aPoint.X = X;
                aPoint.Y = Y;

                aExtent = aCB.GetDrawExtent(aPoint);
                aPoint.X = (float)aExtent.minX;
                aPoint.Y = (float)aExtent.maxY;

                bool ifDraw = true;                
                if (aLayer.ChartSet.AvoidCollision)
                {
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
                            for (j = 0; j < extentList.Count; j++)
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
                            else
                                aPS.Visible = false;
                        }
                    }
                }

                if (ifDraw)
                {
                    Draw.DrawChartPoint(aPoint, aCB, g);

                    //Draw selected rectangle
                    if (aPS.Selected)
                    {
                        Pen aPen = new Pen(Color.Cyan);
                        aPen.DashStyle = DashStyle.Dash;
                        g.DrawRectangle(aPen, (float)aExtent.minX, (float)aExtent.minY, (float)aExtent.Width, (float)aExtent.Height);
                        aPen.Dispose();
                    }
                }
            }

        }

        /// <summary>
        /// Draw vect layer with legend scheme
        /// </summary>
        /// <param name="aLayer"></param>
        /// <param name="g"></param>
        /// <param name="LonShift"></param>      
        public void DrawVectLayerWithLegendScheme(VectorLayer aLayer,
            Graphics g, double LonShift)
        {
            Extent lExtent = MIMath.ShiftExtentLon(aLayer.Extent, LonShift);
            if (!MIMath.IsExtentCross(lExtent, _drawExtent))
            {
                return;
            }

            PointD aPoint = new PointD();
            PointF sPoint = new PointF(0, 0);
            //Pen aPen = new Pen(Color.Black);
            double zoom = 1;
            Single max;           
            max = ((PointBreak)aLayer.LegendScheme.LegendBreaks[0]).Size * 3;
            List<WindArraw> windArraws = new List<WindArraw>();
            Single X, Y;
            X = 0;
            Y = 0;

            //WindArraws = (ArrayList)aLayer.shapeList.Clone();
            int shapeIdx = 0;
            List<int> idxList = new List<int>();
            foreach (WindArraw aArraw in aLayer.ShapeList)
            {
                aPoint = aArraw.Point;
                if (!(aPoint.X + LonShift < _drawExtent.minX || aPoint.X + LonShift > _drawExtent.maxX
                        || aPoint.Y < _drawExtent.minY || aPoint.Y > _drawExtent.maxY))
                {
                    windArraws.Add(aArraw);
                    idxList.Add(shapeIdx);
                }
                shapeIdx += 1;
            }
            //Draw.GetMaxMinWindSpeed(windArraws, ref min, ref max);
            //zoom = 30.0 / (double)max;
            zoom = (double)max / 30.0;
            aLayer.DrawingZoom = (float)zoom;
            //zoom = zoom * 360 / (aLLSS.maxLon - aLLSS.minLon);
            LegendScheme aLS = aLayer.LegendScheme;
            Color aColor = new Color();            
            double value;
            switch (aLS.LegendType)
            {
                case LegendType.SingleSymbol:
                    PointBreak aPB = (PointBreak)aLS.LegendBreaks[0];
                    aColor = aPB.Color;
                    foreach (WindArraw aArraw in windArraws)
                    {
                        aPoint = aArraw.Point;
                        ProjToScreen(aPoint.X, aPoint.Y, ref X, ref Y, LonShift);
                        sPoint.X = X;
                        sPoint.Y = Y;
                        Draw.DrawArraw(aColor, sPoint, aArraw, g, zoom);
                    }
                    break;
                case LegendType.UniqueValue:

                    break;
                case LegendType.GraduatedColor:
                    for (int w = 0; w < windArraws.Count; w++)
                    {
                        WindArraw aArraw = windArraws[w];
                        shapeIdx = idxList[w];
                        //value = aArraw.Value;
                        aPoint = aArraw.Point;
                        ProjToScreen(aPoint.X, aPoint.Y, ref X, ref Y, LonShift);
                        sPoint.X = X;
                        sPoint.Y = Y;

                        string vStr = aLayer.GetCellValue(aLS.FieldName, shapeIdx).ToString().Trim();
                        if (vStr == string.Empty || vStr == null)
                            value = 0;
                        else
                            value = double.Parse(vStr);
                        int blNum = 0;
                        for (int i = 0; i < aLS.LegendBreaks.Count; i++)
                        {
                            aPB = (PointBreak)aLS.LegendBreaks[i];
                            if (value == double.Parse(aPB.StartValue.ToString()) || (value > double.Parse(aPB.StartValue.ToString())
                                && value < double.Parse(aPB.EndValue.ToString())) ||
                                (blNum == aLS.LegendBreaks.Count && value == double.Parse(aPB.EndValue.ToString())))
                            {
                                aColor = aPB.Color;
                                Draw.DrawArraw(aColor, sPoint, aArraw, g, zoom);
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Draw vect layer with legend scheme
        /// </summary>
        /// <param name="aLayer"></param>
        /// <param name="g"></param>
        /// <param name="LonShift"></param>      
        public void DrawVectLayerWithLegendScheme_Dynamic(VectorLayer aLayer,
            Graphics g, double LonShift)
        {
            Extent lExtent = MIMath.ShiftExtentLon(aLayer.Extent, LonShift);
            if (!MIMath.IsExtentCross(lExtent, _drawExtent))
            {
                return;
            }

            PointD aPoint = new PointD();
            PointF sPoint = new PointF(0, 0);
            //Pen aPen = new Pen(Color.Black);
            double zoom;
            Single min, max;
            min = 0;
            max = 30;
            List<WindArraw> windArraws = new List<WindArraw>();
            Single X, Y;
            X = 0;
            Y = 0;

            //WindArraws = (ArrayList)aLayer.shapeList.Clone();
            int shapeIdx = 0;
            List<int> idxList = new List<int>();
            foreach (WindArraw aArraw in aLayer.ShapeList)
            {
                aPoint = aArraw.Point;
                if (!(aPoint.X + LonShift < _drawExtent.minX || aPoint.X + LonShift > _drawExtent.maxX
                        || aPoint.Y < _drawExtent.minY || aPoint.Y > _drawExtent.maxY))
                {
                    windArraws.Add(aArraw);
                    idxList.Add(shapeIdx);
                }
                shapeIdx += 1;
            }
            Draw.GetMaxMinWindSpeed(windArraws, ref min, ref max);
            zoom = 30.0 / (double)max;
            aLayer.DrawingZoom = (float)zoom;
            //zoom = zoom * 360 / (aLLSS.maxLon - aLLSS.minLon);
            LegendScheme aLS = aLayer.LegendScheme;
            Color aColor = new Color();
            double value;
            switch (aLS.LegendType)
            {
                case LegendType.SingleSymbol:
                    PointBreak aPB = (PointBreak)aLS.LegendBreaks[0];
                    aColor = aPB.Color;
                    foreach (WindArraw aArraw in windArraws)
                    {
                        aPoint = aArraw.Point;
                        ProjToScreen(aPoint.X, aPoint.Y, ref X, ref Y, LonShift);
                        sPoint.X = X;
                        sPoint.Y = Y;
                        Draw.DrawArraw(aColor, sPoint, aArraw, g, zoom);
                    }
                    break;
                case LegendType.UniqueValue:

                    break;
                case LegendType.GraduatedColor:
                    for (int w = 0; w < windArraws.Count; w++)
                    {
                        WindArraw aArraw = windArraws[w];
                        shapeIdx = idxList[w];
                        //value = aArraw.Value;
                        aPoint = aArraw.Point;
                        ProjToScreen(aPoint.X, aPoint.Y, ref X, ref Y, LonShift);
                        sPoint.X = X;
                        sPoint.Y = Y;

                        string vStr = aLayer.GetCellValue(aLS.FieldName, shapeIdx).ToString().Trim();
                        if (vStr == string.Empty || vStr == null)
                            value = 0;
                        else
                            value = double.Parse(vStr);
                        int blNum = 0;
                        for (int i = 0; i < aLS.LegendBreaks.Count; i++)
                        {
                            aPB = (PointBreak)aLS.LegendBreaks[i];
                            if (value == double.Parse(aPB.StartValue.ToString()) || (value > double.Parse(aPB.StartValue.ToString())
                                && value < double.Parse(aPB.EndValue.ToString())) ||
                                (blNum == aLS.LegendBreaks.Count && value == double.Parse(aPB.EndValue.ToString())))
                            {
                                aColor = aPB.Color;
                                Draw.DrawArraw(aColor, sPoint, aArraw, g, zoom);
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Draw wind barb layer with legendscheme
        /// </summary>
        /// <param name="aLayer"></param>
        /// <param name="g"></param>
        /// <param name="LonShift"></param>      
        public void DrawBarbLayerWithLegendScheme(VectorLayer aLayer,
            Graphics g, double LonShift)
        {
            Extent lExtent = MIMath.ShiftExtentLon(aLayer.Extent, LonShift);
            if (!MIMath.IsExtentCross(lExtent, _drawExtent))
            {
                return;
            }

            PointD aPoint = new PointD();
            PointF sPoint = new PointF(0, 0);
            //Pen aPen = new Pen(Color.Black);
            Single X, Y;
            X = 0;
            Y = 0;
            LegendScheme aLS = aLayer.LegendScheme;
            Color aColor = new Color();
            double value;
            List<Shape.Shape> WindBarbs = new List<MeteoInfoC.Shape.Shape>(aLayer.ShapeList);            
            List<Extent> extentList = new List<Extent>();
            Extent maxExtent = new Extent();
            Extent aExtent = new Extent();
            if (aLS.LegendType == LegendType.SingleSymbol)
            {
                PointBreak aPB = (PointBreak)aLS.LegendBreaks[0];
                aColor = aPB.Color;
                foreach (WindBarb aWB in WindBarbs)
                {
                    aPoint = aWB.Point;
                    if (aPoint.X + LonShift < _drawExtent.minX || aPoint.X + LonShift > _drawExtent.maxX
                            || aPoint.Y < _drawExtent.minY || aPoint.Y > _drawExtent.maxY)
                    {
                        continue;
                    }
                    ProjToScreen(aPoint.X, aPoint.Y, ref X, ref Y, LonShift);
                    sPoint.X = X;
                    sPoint.Y = Y;

                    if (aLayer.AvoidCollision)
                    {
                        //Judge extent
                        Single aSize = aPB.Size / 2;
                        aExtent.minX = X - aSize;
                        aExtent.maxX = X + aSize;
                        aExtent.minY = Y - aSize;
                        aExtent.maxY = Y + aSize;
                        if (extentList.Count == 0)
                        {
                            maxExtent = aExtent;
                            extentList.Add(aExtent);
                            Draw.DrawWindBarb(aColor, sPoint, aWB, g, aPB.Size);
                        }
                        else
                        {
                            if (!MIMath.IsExtentCross(aExtent, maxExtent))
                            {
                                extentList.Add(aExtent);
                                maxExtent = MIMath.GetLagerExtent(maxExtent, aExtent);
                                Draw.DrawWindBarb(aColor, sPoint, aWB, g, aPB.Size);
                            }
                            else
                            {
                                bool ifDraw = true;
                                for (int i = 0; i < extentList.Count; i++)
                                {
                                    if (MIMath.IsExtentCross(aExtent, extentList[i]))
                                    {
                                        ifDraw = false;
                                        break;
                                    }
                                }
                                if (ifDraw)
                                {
                                    extentList.Add(aExtent);
                                    maxExtent = MIMath.GetLagerExtent(maxExtent, aExtent);
                                    Draw.DrawWindBarb(aColor, sPoint, aWB, g, aPB.Size);
                                }
                            }
                        }
                    }
                    else
                    {
                        Draw.DrawWindBarb(aColor, sPoint, aWB, g, aPB.Size);
                    }

                }
            }
            else
            {
                int shapeIdx = 0;
                foreach (WindBarb aWB in WindBarbs)
                {
                    //value = aWB.Value;
                    string vStr = aLayer.GetCellValue(aLS.FieldName, shapeIdx).ToString().Trim();
                    if (vStr == string.Empty || vStr == null)
                        value = 0;
                    else
                        value = double.Parse(vStr);

                    aPoint = aWB.Point;
                    if (aPoint.X + LonShift < _drawExtent.minX || aPoint.X + LonShift > _drawExtent.maxX
                            || aPoint.Y < _drawExtent.minY || aPoint.Y > _drawExtent.maxY)
                    {
                        shapeIdx += 1;
                        continue;
                    }
                    ProjToScreen(aPoint.X, aPoint.Y, ref X, ref Y, LonShift);
                    sPoint.X = X;
                    sPoint.Y = Y;

                    if (aLayer.AvoidCollision)
                    {
                        //Judge extent
                        Single aSize = ((PointBreak)aLS.LegendBreaks[0]).Size / 2;
                        aExtent.minX = X - aSize;
                        aExtent.maxX = X + aSize;
                        aExtent.minY = Y - aSize;
                        aExtent.maxY = Y + aSize;
                        if (extentList.Count == 0)
                        {
                            maxExtent = aExtent;
                            extentList.Add(aExtent);
                            Single bSize = ((PointBreak)aLS.LegendBreaks[0]).Size;
                            foreach (PointBreak aPB in aLS.LegendBreaks)
                            {
                                if (value == double.Parse(aPB.StartValue.ToString()) || (value > double.Parse(aPB.StartValue.ToString())
                                    && value < double.Parse(aPB.EndValue.ToString())))
                                {
                                    aColor = aPB.Color;
                                    Draw.DrawWindBarb(aColor, sPoint, aWB, g, bSize);
                                }
                            }
                        }
                        else
                        {
                            if (!MIMath.IsExtentCross(aExtent, maxExtent))
                            {
                                extentList.Add(aExtent);
                                maxExtent = MIMath.GetLagerExtent(maxExtent, aExtent);
                                Single bSize = ((PointBreak)aLS.LegendBreaks[0]).Size;
                                foreach (PointBreak aPB in aLS.LegendBreaks)
                                {
                                    if (value == double.Parse(aPB.StartValue.ToString()) || (value > double.Parse(aPB.StartValue.ToString())
                                    && value < double.Parse(aPB.EndValue.ToString())))
                                    {
                                        aColor = aPB.Color;
                                        Draw.DrawWindBarb(aColor, sPoint, aWB, g, bSize);
                                    }
                                }
                            }
                            else
                            {
                                bool ifDraw = true;
                                for (int i = 0; i < extentList.Count; i++)
                                {
                                    if (MIMath.IsExtentCross(aExtent, extentList[i]))
                                    {
                                        ifDraw = false;
                                        break;
                                    }
                                }
                                if (ifDraw)
                                {
                                    extentList.Add(aExtent);
                                    maxExtent = MIMath.GetLagerExtent(maxExtent, aExtent);
                                    Single bSize = ((PointBreak)aLS.LegendBreaks[0]).Size;
                                    foreach (PointBreak aPB in aLS.LegendBreaks)
                                    {
                                        if (value == double.Parse(aPB.StartValue.ToString()) || (value > double.Parse(aPB.StartValue.ToString()) &&
                                            value < double.Parse(aPB.EndValue.ToString())))
                                        {
                                            aColor = aPB.Color;
                                            Draw.DrawWindBarb(aColor, sPoint, aWB, g, bSize);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Single bSize = ((PointBreak)aLS.LegendBreaks[0]).Size;
                        foreach (PointBreak aPB in aLS.LegendBreaks)
                        {
                            if (value == (double)aPB.StartValue || (value > (double)aPB.StartValue && value < (double)aPB.EndValue))
                            {
                                aColor = aPB.Color;
                                Draw.DrawWindBarb(aColor, sPoint, aWB, g, bSize);
                            }
                        }
                    }

                    shapeIdx += 1;
                }
            }
        }

        /// <summary>
        /// Draw weather layer with legendscheme
        /// </summary>
        /// <param name="aLayer"></param>
        /// <param name="g"></param>
        /// <param name="LonShift"></param>        
        public void DrawWeatherLayerWithLegendScheme(VectorLayer aLayer,
            Graphics g, double LonShift)
        {
            Extent lExtent = MIMath.ShiftExtentLon(aLayer.Extent, LonShift);
            if (!MIMath.IsExtentCross(lExtent, _drawExtent))
            {
                return;
            }

            PointD aPoint = new PointD();
            PointF sPoint = new PointF(0, 0);
            //Pen aPen = new Pen(Color.Black);
            Single X, Y;
            X = 0;
            Y = 0;
            LegendScheme aLS = aLayer.LegendScheme;
            Color aColor = new Color();
            double value;
            List<Shape.Shape> Weathers = new List<MeteoInfoC.Shape.Shape>(aLayer.ShapeList);            
            //Font wFont = new Font("Weather", 6);
            //Font wFont = new Font("WeatherSymbols", 6);
            //Font aFont = new Font("Arial", 6);
            //SolidBrush aBrush = new SolidBrush(this.ForeColor);
            List<Extent> extentList = new List<Extent>();
            Extent maxExtent = new Extent();
            Extent aExtent = new Extent();
            if (aLS.LegendType == LegendType.SingleSymbol)
            {
                PointBreak aPB = (PointBreak)aLS.LegendBreaks[0];
                aColor = aPB.Color;
                foreach (WeatherSymbol aWS in Weathers)
                {
                    if (aWS.weather == 0)
                    {
                        continue;
                    }
                    aPoint = aWS.Point;
                    if (aPoint.X + LonShift < _drawExtent.minX || aPoint.X + LonShift > _drawExtent.maxX
                            || aPoint.Y < _drawExtent.minY || aPoint.Y > _drawExtent.maxY)
                    {
                        continue;
                    }
                    ProjToScreen(aPoint.X, aPoint.Y, ref X, ref Y, LonShift);
                    sPoint.X = X;
                    sPoint.Y = Y;

                    if (aLayer.AvoidCollision)
                    {
                        //Judge extent
                        Single aSize = aPB.Size / 2;
                        aExtent.minX = X - aSize;
                        aExtent.maxX = X + aSize;
                        aExtent.minY = Y - aSize;
                        aExtent.maxY = Y + aSize;
                        if (extentList.Count == 0)
                        {
                            maxExtent = aExtent;
                            extentList.Add(aExtent);
                            Draw.DrawWeatherSymbol(aColor, sPoint, aWS, g, aPB.Size);
                        }
                        else
                        {
                            if (!MIMath.IsExtentCross(aExtent, maxExtent))
                            {
                                extentList.Add(aExtent);
                                maxExtent = MIMath.GetLagerExtent(maxExtent, aExtent);
                                Draw.DrawWeatherSymbol(aColor, sPoint, aWS, g, aPB.Size);
                            }
                            else
                            {
                                bool ifDraw = true;
                                for (int i = 0; i < extentList.Count; i++)
                                {
                                    if (MIMath.IsExtentCross(aExtent, extentList[i]))
                                    {
                                        ifDraw = false;
                                        break;
                                    }
                                }
                                if (ifDraw)
                                {
                                    extentList.Add(aExtent);
                                    maxExtent = MIMath.GetLagerExtent(maxExtent, aExtent);
                                    Draw.DrawWeatherSymbol(aColor, sPoint, aWS, g, aPB.Size);
                                }
                            }
                        }
                    }
                    else
                    {
                        Draw.DrawWeatherSymbol(aColor, sPoint, aWS, g, aPB.Size);
                    }

                }
            }
            else
            {
                foreach (WeatherSymbol aWS in Weathers)
                {
                    if (aWS.weather == 0)
                    {
                        continue;
                    }
                    value = aWS.Value;
                    aPoint = aWS.Point;
                    if (aPoint.X + LonShift < _drawExtent.minX || aPoint.X + LonShift > _drawExtent.maxX
                            || aPoint.Y < _drawExtent.minY || aPoint.Y > _drawExtent.maxY)
                    {
                        continue;
                    }
                    ProjToScreen(aPoint.X, aPoint.Y, ref X, ref Y, LonShift);
                    sPoint.X = X;
                    sPoint.Y = Y;
                    foreach (PointBreak aPB in aLS.LegendBreaks)
                    {
                        if (value == (double)aPB.StartValue || (value > (double)aPB.StartValue && value < (double)aPB.EndValue))
                        {
                            aColor = aPB.Color;
                            Draw.DrawWeatherSymbol(aColor, sPoint, aWS, g, aPB.Size);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draw station model layer with legendscheme
        /// </summary>
        /// <param name="aLayer"></param>
        /// <param name="g"></param>
        /// <param name="LonShift"></param>        
        public void DrawStationModelLayerWithLegendScheme(VectorLayer aLayer,
            Graphics g, double LonShift)
        {
            Extent lExtent = MIMath.ShiftExtentLon(aLayer.Extent, LonShift);
            if (!MIMath.IsExtentCross(lExtent, _drawExtent))
            {
                return;
            }

            PointD aPoint = new PointD();
            PointF sPoint = new PointF(0, 0);
            //Pen aPen = new Pen(Color.Black);
            Single X, Y;
            X = 0;
            Y = 0;
            LegendScheme aLS = aLayer.LegendScheme;
            Color aColor = new Color();
            double value;
            List<Shape.Shape> stationModels = new List<MeteoInfoC.Shape.Shape>(aLayer.ShapeList);            
            List<Extent> extentList = new List<Extent>();
            Extent maxExtent = new Extent();
            Extent aExtent = new Extent();
            if (aLS.LegendType == LegendType.SingleSymbol)
            {
                PointBreak aPB = (PointBreak)aLS.LegendBreaks[0];
                aColor = aPB.Color;
                foreach (StationModelShape aSM in stationModels)
                {
                    aPoint = aSM.Point;
                    if (aPoint.X + LonShift < _drawExtent.minX || aPoint.X + LonShift > _drawExtent.maxX
                            || aPoint.Y < _drawExtent.minY || aPoint.Y > _drawExtent.maxY)
                    {
                        continue;
                    }
                    ProjToScreen(aPoint.X, aPoint.Y, ref X, ref Y, LonShift);
                    sPoint.X = X;
                    sPoint.Y = Y;

                    if (aLayer.AvoidCollision)
                    {
                        //Judge extent
                        aExtent.minX = X - aPB.Size;
                        aExtent.maxX = X + aPB.Size;
                        aExtent.minY = Y - aPB.Size;
                        aExtent.maxY = Y + aPB.Size;
                        bool ifDraw = true;
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
                                for (int i = 0; i < extentList.Count; i++)
                                {
                                    if (MIMath.IsExtentCross(aExtent, extentList[i]))
                                    {
                                        ifDraw = false;
                                        break;
                                    }
                                }
                            }
                        }
                        if (ifDraw)
                        {
                            extentList.Add(aExtent);
                            maxExtent = MIMath.GetLagerExtent(maxExtent, aExtent);
                            Draw.DrawStationModel(aColor, this.ForeColor, sPoint, aSM, g, aPB.Size, aPB.Size / 8 * 3);
                        }
                    }
                    else
                    {
                        Draw.DrawStationModel(aColor, this.ForeColor, sPoint, aSM, g, aPB.Size, aPB.Size / 8 * 3);
                    }
                }
            }
            else
            {
                foreach (StationModelShape aSM in stationModels)
                {
                    value = aSM.Value;
                    aPoint = aSM.Point;
                    if (aPoint.X + LonShift < _drawExtent.minX || aPoint.X + LonShift > _drawExtent.maxX
                            || aPoint.Y < _drawExtent.minY || aPoint.Y > _drawExtent.maxY)
                    {
                        continue;
                    }
                    ProjToScreen(aPoint.X, aPoint.Y, ref X, ref Y, LonShift);
                    sPoint.X = X;
                    sPoint.Y = Y;
                    foreach (PointBreak aPB in aLS.LegendBreaks)
                    {
                        if (value == (double)aPB.StartValue || (value > (double)aPB.StartValue && value < (double)aPB.EndValue))
                        {
                            aColor = aPB.Color;
                            Draw.DrawStationModel(aColor, this.ForeColor, sPoint, aSM, g, aPB.Size, aPB.Size / 8 * 3);
                        }
                    }
                }
            }

            //Draw layer labels
            if (aLayer.LabelSet.DrawLabels)
                DrawLayerLabels(g, aLayer, LonShift);
        }

        /// <summary>
        /// Export to a picture file
        /// </summary>
        /// <param name="aFile">file path</param>
        public void ExportToPicture(string aFile)
        {
            if (Path.GetExtension(aFile).ToLower() == ".wmf")
            {
                Bitmap bmp = new Bitmap(this.Width, this.Height);
                Graphics gs = Graphics.FromImage(bmp);
                Rectangle rect = this.Bounds;
                Metafile mf = new Metafile(aFile, gs.GetHdc(), rect, MetafileFrameUnit.Pixel);

                Graphics g = Graphics.FromImage(mf);

                PaintGraphics(g);

                //g.Save();
                g.Dispose();
                mf.Dispose();
                gs.Dispose();
                bmp.Dispose();
            }
            else
            {
                Bitmap bitmap = new Bitmap(this.Width, this.Height);
                DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));
                bitmap.MakeTransparent(Color.Transparent);
                switch (Path.GetExtension(aFile).ToLower())
                {
                    case ".gif":
                        OctreeQuantizer quantizer = new OctreeQuantizer(255, 8);
                        Bitmap quantizered = quantizer.Quantize(bitmap);
                        quantizered.Save(aFile, ImageFormat.Gif);
                        quantizered.Dispose();                        
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

                bitmap.Dispose();
            }
        }

        private void PaintOnlyLayers(int aX, int aY)
        {
            _tempMapBitmap = new Bitmap(this.ClientRectangle.Width,
                this.ClientRectangle.Height, PixelFormat.Format32bppPArgb);
            Graphics g = Graphics.FromImage(_tempMapBitmap);

            Rectangle mapRect = new Rectangle(1, 1, this.Width - 2, this.Height - 2);
            g.Clip = new Region(mapRect);
            //Fill backgroud
            //Rectangle mapRect = this.Bounds;
            Color aColor = this.BackColor;
            if (aColor == Color.Transparent)
                aColor = Color.White;
            g.FillRectangle(new SolidBrush(aColor), mapRect);
            
            g.DrawImageUnscaled(_mapBitmap, aX, aY);

            g.Dispose();

        }

        private void GetMaskOutGraphicsPath(Graphics g)
        {
            if (_maskOut.SetMaskLayer)
            {                
                if (_maskOut.Shapes.Count > 0)
                {                    
                    GraphicsPath tPath = new GraphicsPath();
                    int sNum = 0;
                    double[] lonShiftList = new double[]{ 0};
                    if (_projection.IsLonLatMap)
                        lonShiftList = new double[]{ 0, 360, -360 };

                    foreach (double lonShift in lonShiftList)
                    {
                        foreach (PolygonShape aPGS in _maskOut.Shapes)
                        {
                            GraphicsPath aPath = new GraphicsPath();
                            PointF[] points = new PointF[aPGS.Points.Count];
                            PointF pt = new PointF(0, 0);
                            Single X = 0, Y = 0;
                            PointD wPoint = new PointD();

                            for (int i = 0; i < aPGS.Points.Count; i++)
                            {
                                wPoint = (PointD)aPGS.Points[i];
                                ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
                                        lonShift);
                                pt.X = X;
                                pt.Y = Y;
                                points[i] = pt;
                            }

                            if (aPGS.PartNum == 1)
                            {
                                aPath.AddPolygon(points);                                                             
                            }
                            else
                            {
                                PointF[] Pointps;
                                for (int p = 0; p < aPGS.PartNum; p++)
                                {
                                    if (p == aPGS.PartNum - 1)
                                    {
                                        Pointps = new PointF[aPGS.PointNum - aPGS.parts[p]];
                                        for (int pp = aPGS.parts[p]; pp < aPGS.PointNum; pp++)
                                        {
                                            Pointps[pp - aPGS.parts[p]] = points[pp];
                                        }
                                    }
                                    else
                                    {
                                        Pointps = new PointF[aPGS.parts[p + 1] - aPGS.parts[p]];
                                        for (int pp = aPGS.parts[p]; pp < aPGS.parts[p + 1]; pp++)
                                        {
                                            Pointps[pp - aPGS.parts[p]] = points[pp];
                                        }
                                    }
                                    aPath.AddPolygon(Pointps);
                                }                                
                            }

                            tPath.AddPath(aPath, false);
                            sNum += 1;
                        }
                    }

                    _maskOutGraphicsPath = tPath;
                }
            }
        }

        private void SetClipRegion(ref Graphics g)
        {

            if (_maskOut.SetMaskLayer)
            {
                if (_maskOut.Shapes.Count > 0)
                {
                    Matrix oldMatrix = g.Transform;
                    g.ResetTransform();
                    g.SetClip(_maskOutGraphicsPath, CombineMode.Intersect);
                    //g.Clip.Transform(oldMatrix);
                    g.Transform = oldMatrix;
                }
            }


        }

        private void SetClipRegion_Bak(ref Graphics g, int lonShift)
        {

            if (_maskOut.SetMaskLayer)
            {
                int aLayerHandle = GetLayerHandleFromName(_maskOut.MaskLayer);
                if (aLayerHandle > 0)
                {
                    Region oldRegion = g.Clip;
                    Region aRegion = new Region();
                    VectorLayer aLayer = (VectorLayer)GetLayerFromHandle(aLayerHandle);
                    int sNum = 0;
                    foreach (PolygonShape aPGS in aLayer.ShapeList)
                    {
                        GraphicsPath aPath = new GraphicsPath();
                        PointF[] points = new PointF[aPGS.Points.Count];
                        PointF pt = new PointF(0, 0);
                        Single X = 0, Y = 0;
                        PointD wPoint = new PointD();

                        for (int i = 0; i < aPGS.Points.Count; i++)
                        {
                            wPoint = (PointD)aPGS.Points[i];
                            ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
                                    lonShift);
                            pt.X = X;
                            pt.Y = Y;
                            points[i] = pt;
                        }

                        if (aPGS.PartNum == 1)
                        {
                            aPath.AddPolygon(points);
                            if (_projection.IsLonLatMap)
                            {
                                if (aLayer.Extent.minX < 0)    //Polygon layer not from 0 to 360 degree
                                {
                                    for (int i = 0; i < aPGS.Points.Count; i++)
                                    {
                                        wPoint = (PointD)aPGS.Points[i];
                                        ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
                                                lonShift + 360);
                                        pt.X = X;
                                        pt.Y = Y;
                                        points[i] = pt;
                                    }
                                    aPath.AddPolygon(points);
                                }
                            }
                            if (sNum == 0)
                            {
                                aRegion = new Region(aPath);
                            }
                            else
                            {
                                aRegion.Union(aPath);
                            }
                            sNum += 1;
                        }
                        else
                        {
                            PointF[] Pointps;
                            for (int p = 0; p < aPGS.PartNum; p++)
                            {
                                if (p == aPGS.PartNum - 1)
                                {
                                    Pointps = new PointF[aPGS.PointNum - aPGS.parts[p]];
                                    for (int pp = aPGS.parts[p]; pp < aPGS.PointNum; pp++)
                                    {
                                        Pointps[pp - aPGS.parts[p]] = points[pp];
                                    }
                                }
                                else
                                {
                                    Pointps = new PointF[aPGS.parts[p + 1] - aPGS.parts[p]];
                                    for (int pp = aPGS.parts[p]; pp < aPGS.parts[p + 1]; pp++)
                                    {
                                        Pointps[pp - aPGS.parts[p]] = points[pp];
                                    }
                                }
                                aPath.AddPolygon(Pointps);
                            }

                            if (_projection.IsLonLatMap)
                            {
                                if (aLayer.Extent.minX < 0)    //Polygon layer not from 0 to 360 degree
                                {
                                    for (int i = 0; i < aPGS.Points.Count; i++)
                                    {
                                        wPoint = (PointD)aPGS.Points[i];
                                        ProjToScreen(wPoint.X, wPoint.Y, ref X, ref Y,
                                                lonShift + 360);
                                        pt.X = X;
                                        pt.Y = Y;
                                        points[i] = pt;
                                    }

                                    for (int p = 0; p < aPGS.PartNum; p++)
                                    {
                                        if (p == aPGS.PartNum - 1)
                                        {
                                            Pointps = new PointF[aPGS.PointNum - aPGS.parts[p]];
                                            for (int pp = aPGS.parts[p]; pp < aPGS.PointNum; pp++)
                                            {
                                                Pointps[pp - aPGS.parts[p]] = points[pp];
                                            }
                                        }
                                        else
                                        {
                                            Pointps = new PointF[aPGS.parts[p + 1] - aPGS.parts[p]];
                                            for (int pp = aPGS.parts[p]; pp < aPGS.parts[p + 1]; pp++)
                                            {
                                                Pointps[pp - aPGS.parts[p]] = points[pp];
                                            }
                                        }
                                        aPath.AddPolygon(Pointps);
                                    }
                                }
                            }
                        }

                        if (sNum == 0)
                        {
                            aRegion = new Region(aPath);
                        }
                        else
                        {
                            aRegion.Union(aPath);
                        }
                        sNum += 1;
                    }

                    aRegion.Intersect(oldRegion);
                    //g.Clip = aRegion;
                    g.SetClip(aRegion, CombineMode.Replace);
                }
            }


        }

        private void DrawLonLatMap(Graphics g)
        {
            DrawLonLatMap(g, this.Width, this.Height);
        }

        private void DrawLonLatMap(Graphics g, int width, int heigth)
        {           
            //Draw layers
            DrawLayers(g, width, heigth);
            
            //Draw logo
            //DrawLogo(e);

            //Draw lon lat
            if (_drawGridLine)
            {
                //DrawLonLat(g);

                //if (_lonLatLayer == null)
                //    _lonLatLayer = GenerateLonLatLayer();

                LegendScheme aLS = _lonLatLayer.LegendScheme;
                PolyLineBreak aPLB = (PolyLineBreak)aLS.LegendBreaks[0];
                aPLB.Color = _gridLineColor;
                aPLB.Size = _gridLineSize;
                aPLB.DashStyle = _gridLineStyle;

                DrawLonLatLayer(_lonLatLayer, g, 0);
                if (_multiGlobalDraw)
                {
                    if (_lonLatLayer.Extent.minX > -360 && _lonLatLayer.Extent.maxX > 0)
                        DrawLonLatLayer(_lonLatLayer, g, -360);
                    if (_lonLatLayer.Extent.maxX < 360 && _lonLatLayer.Extent.minX < 0)
                        DrawLonLatLayer(_lonLatLayer, g, 360);
                }
            }  
            
            //Draw graphics
            DrawGraphicList(g, 0);
            if (_multiGlobalDraw)
            {
                if (_graphicCollection.Extent.minX > -360 && _graphicCollection.Extent.maxX > 0)
                    DrawGraphicList(g, -360);
                if (_graphicCollection.Extent.maxX < 360 && _graphicCollection.Extent.minX < 0)
                    DrawGraphicList(g, 360);
            }
        }

        private void DrawProjectedMap(Graphics g)
        {
            DrawProjectedMap(g, this.Width, this.Height);
        }

        private void DrawProjectedMap(Graphics g, int width, int heigth)
        {
            //Draw layers
            DrawProjectedLayers(g, width, heigth);

            //Draw lon/lat
            if (_drawGridLine)
                DrawProjectedLonLat(g);

            //Draw graphics
            DrawGraphicList(g, 0);            
        }

        private void Draw2DMap(Graphics g)
        {            

            //Draw layers
            DrawLayers(g);

            //Draw X/Y grid
            DrawXYGrid(g, _xGridStrs, _yGridStrs);

        }

        private void DrawLayers(Graphics g)
        {
            DrawLayers(g, this.Width, this.Height);
        }

        private void DrawLayers(Graphics g, int width, int height)
        {
            Region oldRegion = g.Clip;
            double geoScale = this.GetGeoScale();
            for (int i = 0; i < _layerSet.Layers.Count; i++)
            {
                MapLayer layer = _layerSet.Layers[i];
                if (layer.VisibleScale.EnableMinVisScale)
                {
                    if (geoScale > layer.VisibleScale.MinVisScale)
                        continue;
                }
                if (layer.VisibleScale.EnableMaxVisScale)
                {
                    if (geoScale < layer.VisibleScale.MaxVisScale)
                        continue;
                }

                switch (layer.LayerType)
                {
                    case LayerTypes.ImageLayer:
                        ImageLayer aImageLayer = (ImageLayer)_layerSet.Layers[i];
                        if (aImageLayer.Visible)
                        {
                            if (aImageLayer.IsMaskout)
                                SetClipRegion(ref g);
                            DrawImage(g, aImageLayer, 0, width, height);
                            if (aImageLayer.IsMaskout)
                                g.Clip = oldRegion;

                            if (_multiGlobalDraw)
                            {
                                if (aImageLayer.Extent.minX > -360 && aImageLayer.Extent.maxX > 0)
                                {
                                    if (aImageLayer.IsMaskout)
                                        SetClipRegion(ref g);
                                    DrawImage(g, aImageLayer, -360, width, height);
                                    if (aImageLayer.IsMaskout)
                                        g.Clip = oldRegion;
                                }
                                if (aImageLayer.Extent.maxX < 360 && aImageLayer.Extent.minX < 0)
                                {
                                    if (aImageLayer.IsMaskout)
                                        SetClipRegion(ref g);
                                    DrawImage(g, aImageLayer, 360, width, height);
                                    if (aImageLayer.IsMaskout)
                                        g.Clip = oldRegion;
                                }
                            }
                        }
                        break;
                    case LayerTypes.RasterLayer:
                        RasterLayer aRLayer = (RasterLayer)_layerSet.Layers[i];
                        if (aRLayer.Visible)
                        {
                            if (aRLayer.IsMaskout)
                                SetClipRegion(ref g);
                            DrawRasterLayer(g, aRLayer, 0);
                            if (aRLayer.IsMaskout)
                                g.Clip = oldRegion;

                            if (_multiGlobalDraw)
                            {
                                if (aRLayer.Extent.minX > -360 && aRLayer.Extent.maxX > 0)
                                {
                                    if (aRLayer.IsMaskout)
                                        SetClipRegion(ref g);
                                    DrawRasterLayer(g, aRLayer, -360);
                                    if (aRLayer.IsMaskout)
                                        g.Clip = oldRegion;
                                }
                                if (aRLayer.Extent.maxX < 360 && aRLayer.Extent.minX < 0)
                                {
                                    if (aRLayer.IsMaskout)
                                        SetClipRegion(ref g);
                                    DrawRasterLayer(g, aRLayer, 360);
                                    if (aRLayer.IsMaskout)
                                        g.Clip = oldRegion;
                                }
                            }
                        }
                        break;
                    case LayerTypes.VectorLayer:
                        VectorLayer aLayer = (VectorLayer)_layerSet.Layers[i];
                        if (aLayer.Visible)
                        {
                            switch (aLayer.LayerDrawType)
                            {
                                case LayerDrawType.Vector:
                                    if (aLayer.IsMaskout)
                                        SetClipRegion(ref g);
                                    DrawVectLayerWithLegendScheme(aLayer, g, 0);
                                    if (aLayer.IsMaskout)
                                        g.Clip = oldRegion;

                                    if (_multiGlobalDraw)
                                    {
                                        if (aLayer.Extent.minX > -360 && aLayer.Extent.maxX > 0)
                                        {
                                            if (aLayer.IsMaskout)
                                                SetClipRegion(ref g);
                                            DrawVectLayerWithLegendScheme(aLayer, g, -360);
                                            if (aLayer.IsMaskout)
                                                g.Clip = oldRegion;
                                        }
                                        if (aLayer.Extent.maxX < 360 && aLayer.Extent.minX < 0)
                                        {
                                            if (aLayer.IsMaskout)
                                                SetClipRegion(ref g);
                                            DrawVectLayerWithLegendScheme(aLayer, g, 360);
                                            if (aLayer.IsMaskout)
                                                g.Clip = oldRegion;
                                        }
                                    }
                                    break;
                                case LayerDrawType.Barb:
                                    if (aLayer.IsMaskout)
                                        SetClipRegion(ref g);
                                    DrawBarbLayerWithLegendScheme(aLayer, g, 0);
                                    if (aLayer.IsMaskout)
                                        g.Clip = oldRegion;

                                    if (_multiGlobalDraw)
                                    {
                                        if (aLayer.Extent.minX > -360 && aLayer.Extent.maxX > 0)
                                        {
                                            if (aLayer.IsMaskout)
                                                SetClipRegion(ref g);
                                            DrawBarbLayerWithLegendScheme(aLayer, g, -360);
                                            if (aLayer.IsMaskout)
                                                g.Clip = oldRegion;
                                        }
                                        if (aLayer.Extent.maxX < 360 && aLayer.Extent.minX < 0)
                                        {
                                            if (aLayer.IsMaskout)
                                                SetClipRegion(ref g);
                                            DrawBarbLayerWithLegendScheme(aLayer, g, 360);
                                            if (aLayer.IsMaskout)
                                                g.Clip = oldRegion;
                                        }
                                    }
                                    break;
                                case LayerDrawType.StationModel:
                                    if (aLayer.IsMaskout)
                                        SetClipRegion(ref g);
                                    DrawStationModelLayerWithLegendScheme(aLayer, g, 0);
                                    if (aLayer.IsMaskout)
                                        g.Clip = oldRegion;

                                    if (_multiGlobalDraw)
                                    {
                                        if (aLayer.Extent.minX > -360 && aLayer.Extent.maxX > 0)
                                        {
                                            if (aLayer.IsMaskout)
                                                SetClipRegion(ref g);
                                            DrawStationModelLayerWithLegendScheme(aLayer, g, -360);
                                            if (aLayer.IsMaskout)
                                                g.Clip = oldRegion;
                                        }
                                        if (aLayer.Extent.maxX < 360 && aLayer.Extent.minX < 0)
                                        {
                                            if (aLayer.IsMaskout)
                                                SetClipRegion(ref g);
                                            DrawStationModelLayerWithLegendScheme(aLayer, g, 360);
                                            if (aLayer.IsMaskout)
                                                g.Clip = oldRegion;
                                        }
                                    }
                                    break;
                                case LayerDrawType.WeatherSymbol:
                                    if (aLayer.IsMaskout)
                                        SetClipRegion(ref g);
                                    DrawWeatherLayerWithLegendScheme(aLayer, g, 0);
                                    if (aLayer.IsMaskout)
                                        g.Clip = oldRegion;

                                    if (_multiGlobalDraw)
                                    {
                                        if (aLayer.Extent.minX > -360 && aLayer.Extent.maxX > 0)
                                        {
                                            if (aLayer.IsMaskout)
                                                SetClipRegion(ref g);
                                            DrawWeatherLayerWithLegendScheme(aLayer, g, -360);
                                            if (aLayer.IsMaskout)
                                                g.Clip = oldRegion;
                                        }
                                        if (aLayer.Extent.maxX < 360 && aLayer.Extent.minX < 0)
                                        {
                                            if (aLayer.IsMaskout)
                                                SetClipRegion(ref g);
                                            DrawWeatherLayerWithLegendScheme(aLayer, g, 360);
                                            if (aLayer.IsMaskout)
                                                g.Clip = oldRegion;
                                        }
                                    }
                                    break;
                                default:
                                    if (aLayer.IsMaskout)
                                        SetClipRegion(ref g);
                                    DrawLayerWithLegendScheme(aLayer, g, 0);
                                    if (aLayer.IsMaskout)
                                        g.Clip = oldRegion;

                                    if (_multiGlobalDraw)
                                    {
                                        if (aLayer.Extent.minX > -360 && aLayer.Extent.maxX > 0)
                                        {
                                            if (aLayer.IsMaskout)
                                                SetClipRegion(ref g);
                                            DrawLayerWithLegendScheme(aLayer, g, -360);
                                            if (aLayer.IsMaskout)
                                                g.Clip = oldRegion;
                                        }
                                        if (aLayer.Extent.maxX < 360 && aLayer.Extent.minX < 0)
                                        {
                                            if (aLayer.IsMaskout)
                                                SetClipRegion(ref g);
                                            DrawLayerWithLegendScheme(aLayer, g, 360);
                                            if (aLayer.IsMaskout)
                                                g.Clip = oldRegion;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        private void DrawProjectedLayers(Graphics g)
        {
            DrawProjectedLayers(g, this.Width, this.Height);
        }

        private void DrawProjectedLayers(Graphics g, int width, int height)
        {
            Region oldRegion = g.Clip;
            for (int i = 0; i < _layerSet.Layers.Count; i++)
            {
                switch (_layerSet.Layers[i].LayerType)
                {
                    case LayerTypes.ImageLayer:
                        ImageLayer aImageLayer = (ImageLayer)_layerSet.Layers[i];
                        if (aImageLayer.Visible)
                        {
                            if (aImageLayer.IsMaskout)
                                SetClipRegion(ref g);
                            DrawImage(g, aImageLayer, 0, width, height);
                            if (aImageLayer.IsMaskout)
                                g.Clip = oldRegion;
                        }
                        break;
                    case LayerTypes.RasterLayer:
                        RasterLayer aRLayer = (RasterLayer)_layerSet.Layers[i];
                        if (aRLayer.Visible)
                        {
                            if (aRLayer.IsMaskout)
                                SetClipRegion(ref g);
                            DrawRasterLayer(g, aRLayer, 0);
                            if (aRLayer.IsMaskout)
                                g.Clip = oldRegion;
                        }
                        break;
                    case LayerTypes.VectorLayer:
                        VectorLayer aLayer = (VectorLayer)_layerSet.Layers[i];
                        if (aLayer.Visible)
                        {
                            if (aLayer.IsMaskout)
                                SetClipRegion(ref g);
                            switch (aLayer.LayerDrawType)
                            {                                
                                case LayerDrawType.Vector:                                    
                                    DrawVectLayerWithLegendScheme(aLayer, g, 0);                                    
                                    break;
                                case LayerDrawType.Barb:                                    
                                    DrawBarbLayerWithLegendScheme(aLayer, g, 0);                                    
                                    break;
                                case LayerDrawType.WeatherSymbol:                                    
                                    DrawWeatherLayerWithLegendScheme(aLayer, g, 0);                                    
                                    break;
                                case LayerDrawType.StationModel:                                    
                                    DrawStationModelLayerWithLegendScheme(aLayer, g, 0);                                    
                                    break;
                                default:                                    
                                    DrawLayerWithLegendScheme(aLayer, g, 0);                                    
                                    break;
                            }
                            if (aLayer.IsMaskout)
                                g.Clip = oldRegion;
                        }
                        break;
                }                
            }
        }

        private void DrawImage(Graphics g, ImageLayer aILayer, double LonShift, int width, int height)
        {                        
            Extent lExtent = MIMath.ShiftExtentLon(aILayer.Extent, LonShift);
            if (MIMath.IsExtentCross(lExtent, _drawExtent))
            {
                double X, Y;
                X = 0;
                Y = 0;
                double XUL, YUL, XBR, YBR;
                XUL = aILayer.Extent.minX;
                YUL = aILayer.Extent.maxY;
                XBR = aILayer.Extent.maxX;
                YBR = aILayer.Extent.minY;
                ProjToScreen(XUL - aILayer.WorldFileParaV.XScale / 2, YUL - aILayer.WorldFileParaV.YScale / 2,
                    ref X, ref Y, LonShift);
                double sX = X;
                double sY = Y;
                ProjToScreen(XBR, YBR, ref X, ref Y, LonShift);
                double aWidth = X - sX;
                double aHeight = Y - sY;

                if (aWidth < 5 || aHeight < 5)
                {
                    return;
                } 
               
                //Draw image
                Image dImage = new Bitmap(width, height);
                Graphics dg = Graphics.FromImage(dImage);
                dg.PixelOffsetMode = PixelOffsetMode.Half;
                //dg.InterpolationMode = InterpolationMode.NearestNeighbor;
                float iWidth = aILayer.Image.Width;
                float iHeight = aILayer.Image.Height;
                float cw = (float)(aWidth / iWidth);
                float ch = (float)(aHeight / iHeight);
                Matrix mx = new Matrix();
                float shx = (float)aILayer.WorldFileParaV.XRotate;
                float shy = (float)aILayer.WorldFileParaV.YRotate;
                if (shx == 0 && shy == 0)
                {
                    mx.Scale(cw, ch);
                    mx.Translate((float)sX, (float)sY, MatrixOrder.Append); 
                }
                else
                {
                    shx = -(float)(cw / aILayer.WorldFileParaV.XScale * shx);
                    shy = -(float)(ch / aILayer.WorldFileParaV.YScale * shy);
                    mx = new Matrix(cw, shx, shy, ch, (float)sX, (float)sY);
                }
                dg.Transform = mx;

                if (aILayer.TransparencyPerc > 0)
                {
                    //Set transparency
                    int transPerc = 100 - aILayer.TransparencyPerc;
                    ImageAttributes imageAttributes =
                         new ImageAttributes();
                    float[][] colorMatrixElements = {    
                                                     new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},   
                                                     new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},   
                                                     new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},   
                                                     new float[] {0.0f,  0.0f,  0.0f,  transPerc /100.0f, 0.0f},   
                                                     new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}   
                                                };
                    ColorMatrix wmColorMatrix = new
                         ColorMatrix(colorMatrixElements);
                    imageAttributes.SetColorMatrix(wmColorMatrix,
                         ColorMatrixFlag.Default,
                         ColorAdjustType.Bitmap);
                                                          
                    dg.DrawImage(aILayer.Image, new Rectangle(0, 0, aILayer.Image.Width, aILayer.Image.Height), 0, 0, aILayer.Image.Width, aILayer.Image.Height,
                        GraphicsUnit.Pixel, imageAttributes);                    
                }
                else
                {
                    Bitmap vImage = (Bitmap)aILayer.Image;
                    if (vImage.HorizontalResolution != dg.DpiX || vImage.VerticalResolution != dg.DpiY)
                        vImage.SetResolution(dg.DpiX, dg.DpiY);

                    dg.DrawImage(vImage, 0, 0);                   
                }
                dg.Dispose();

                g.DrawImageUnscaled(dImage, 0, 0);
                dImage.Dispose();                
            }
        }        

        private void DrawImage_back(Graphics g, ImageLayer aILayer, double LonShift)
        {
            Extent lExtent = MIMath.ShiftExtentLon(aILayer.Extent, LonShift);
            if (MIMath.IsExtentCross(lExtent, _drawExtent))
            {
                double X, Y;
                X = 0;
                Y = 0;
                double XUL, YUL, XBR, YBR;
                XUL = aILayer.Extent.minX;
                YUL = aILayer.Extent.maxY;
                XBR = aILayer.Extent.maxX;
                YBR = aILayer.Extent.minY;
                ProjToScreen(XUL - aILayer.WorldFileParaV.XScale / 2, YUL - aILayer.WorldFileParaV.YScale / 2,
                    ref X, ref Y, LonShift);
                double sX = X;
                double sY = Y;
                ProjToScreen(XBR, YBR, ref X, ref Y, LonShift);
                double aWidth = X - sX;
                double aHeight = Y - sY;

                if (aWidth < 5 || aHeight < 5)
                {
                    return;
                }
                int transPerc = 100 - aILayer.TransparencyPerc;

                //Set transparency
                ImageAttributes imageAttributes =
                     new ImageAttributes();
                float[][] colorMatrixElements = {    
                                                     new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},   
                                                     new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},   
                                                     new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},   
                                                     new float[] {0.0f,  0.0f,  0.0f,  transPerc /100.0f, 0.0f},   
                                                     new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}   
                                                };
                ColorMatrix wmColorMatrix = new
                     ColorMatrix(colorMatrixElements);
                imageAttributes.SetColorMatrix(wmColorMatrix,
                     ColorMatrixFlag.Default,
                     ColorAdjustType.Bitmap);

                //if (aILayer.SetTransColor)
                //{
                //    //((Bitmap)aILayer.Image).MakeTransparent(aILayer.TransparencyColor);
                //    ((Bitmap)aILayer.Image).MakeTransparent();
                //    for (int i = 0; i < aILayer.Image.Width; i++)
                //    {
                //        for (int j = 0; j < aILayer.Image.Height; j++)
                //        {
                //            if (((Bitmap)aILayer.Image).GetPixel(i, j) == aILayer.TransparencyColor)
                //                ((Bitmap)aILayer.Image).SetPixel(i, j, Color.FromArgb(0, aILayer.TransparencyColor)); 
                //        }
                //    }
                //}

                //Draw image
                //g.DrawImage(aILayer.Image, sX, sY, aWidth, aHeight);
                g.DrawImage(aILayer.Image, new Rectangle((int)sX, (int)sY, (int)aWidth, (int)aHeight), 0, 0, aILayer.Image.Width, aILayer.Image.Height, GraphicsUnit.Pixel, imageAttributes);
                //g.DrawImage(aILayer.Bitmap, sX, sY, aWidth, aHeight);
                //g.DrawImage(aILayer.Bitmap, new Rectangle((int)sX, (int)sY, (int)aWidth, (int)aHeight), 0, 0, aWidth, aHeight, GraphicsUnit.Pixel, imageAttributes);
                //g.DrawImage(_bitmap, new Rectangle((int)p0.X, (int)p1.Y, destWidth, destHeight), 0, 0, _bitmap.Width, _bitmap.Height, GraphicsUnit.Pixel, imageAttributes);
            }
        }

        private void DrawRasterLayer(Graphics g, RasterLayer aRLayer, double LonShift)
        {
            Extent lExtent = MIMath.ShiftExtentLon(aRLayer.Extent, LonShift);
            if (MIMath.IsExtentCross(lExtent, _drawExtent))
            {
                Single X, Y;
                X = 0;
                Y = 0;
                double XUL, YUL, XBR, YBR;
                XUL = aRLayer.Extent.minX;
                YUL = aRLayer.Extent.maxY;
                XBR = aRLayer.Extent.maxX;
                YBR = aRLayer.Extent.minY;
                //ProjToScreen(XUL - aRLayer.WorldFileParaV.XScale / 2, YUL - aRLayer.WorldFileParaV.YScale / 2,
                //    ref X, ref Y, LonShift);
                ProjToScreen(XUL, YUL, ref X, ref Y, LonShift);
                Single sX = X;
                Single sY = Y;
                ProjToScreen(XBR, YBR, ref X, ref Y, LonShift);
                Single aWidth = X - sX;
                Single aHeigh = Y - sY;

                if (aWidth < 5 || aHeigh < 5)
                {
                    return;
                }
                //g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.InterpolationMode = aRLayer.InterpMode;
                g.DrawImage(aRLayer.Image, sX, sY, aWidth, aHeigh);
            }
        }

        private void DrawMaps(Graphics g)
        {
            Region oldRegion = g.Clip;
            for (int i = 0; i < _layerSet.Layers.Count; i++)
            {
                if (_layerSet.Layers[i].GetType() == typeof(VectorLayer))
                {
                    VectorLayer aLayer = (VectorLayer)_layerSet.Layers[i];
                    if (aLayer.Visible)
                    {
                        switch (aLayer.LayerDrawType)
                        {
                            case LayerDrawType.Map:
                                //DrawLayerWithLegendScheme(aLayer, g, 0, aLSS);
                                //if (aLayer.Extent.minX > -360 && aLayer.Extent.maxX > 0)
                                //{
                                //    DrawLayerWithLegendScheme(aLayer, g, -360, aLSS);
                                //}
                                //if (aLayer.Extent.maxX < 360 && aLayer.Extent.minX < 0)
                                //{
                                //    DrawLayerWithLegendScheme(aLayer, g, 360, aLSS);
                                //}
                                if (aLayer.IsMaskout)
                                    SetClipRegion(ref g);
                                DrawLayerWithLegendScheme(aLayer, g, 0);
                                if (aLayer.IsMaskout)
                                    g.Clip = oldRegion;
                                if (aLayer.Extent.minX > -360 && aLayer.Extent.maxX > 0)
                                {
                                    if (aLayer.IsMaskout)
                                        SetClipRegion(ref g);
                                    DrawLayerWithLegendScheme(aLayer, g, -360);
                                    if (aLayer.IsMaskout)
                                        g.Clip = oldRegion;
                                }
                                if (aLayer.Extent.maxX < 360 && aLayer.Extent.minX < 0)
                                {
                                    if (aLayer.IsMaskout)
                                        SetClipRegion(ref g);
                                    DrawLayerWithLegendScheme(aLayer, g, 360);
                                    if (aLayer.IsMaskout)
                                        g.Clip = oldRegion;
                                }
                                break;
                        }
                    }
                }
            }
        }                          
        
        private void DrawLogo(Graphics g)
        {
            int size = 8;
            Font drawFont = new Font("Arial", size);
            SolidBrush aBrush = new SolidBrush(this.ForeColor);

            g.DrawString("MeteoInfo/CMA", drawFont, Brushes.Black, 10, this.Height - 20);

            drawFont.Dispose();
            aBrush.Dispose();
        }        

        private void DrawLonLat(Graphics g)
        {
            Single lonRan, lonDelta, latRan, latDelta, MinX, MinY;

            lonRan = (Single)(_drawExtent.maxX - _drawExtent.minX);
            latRan = (Single)(_drawExtent.maxY - _drawExtent.minY);
            lonDelta = lonRan / 10;
            latDelta = latRan / 10;

            string eStr;
            int aD, aE;
            //Longitude
            eStr = lonDelta.ToString("e1");
            aD = int.Parse(eStr.Substring(0, 1));
            aE = int.Parse(eStr.Substring(eStr.IndexOf("e") + 1, eStr.Length - eStr.IndexOf("e") - 1));
            if (aD > 4)
            {
                lonDelta = (Single)(Math.Pow(10, aE + 1));
                //MinX = Convert.ToInt32((_drawExtent.minX + lonDelta) / Math.Pow(10, aE + 1)) * Math.Pow(10, aE + 1);
                MinX = (Single)((int)(_drawExtent.minX / lonDelta + 1) * Math.Pow(10, aE + 1));
            }
            else if ((aD == 4) || aD == 2)
            {
                lonDelta = (Single)(5 * Math.Pow(10, aE));
                //MinX = Convert.ToInt32((_drawExtent.minX + lonDelta) / Math.Pow(10, aE)) * Math.Pow(10, aE);
                MinX = (Single)((int)(_drawExtent.minX / lonDelta + 1) * 5 * Math.Pow(10, aE));
            }
            else if (aD == 3)
            {
                lonDelta = (Single)(6 * Math.Pow(10, aE));
                //MinX = Convert.ToInt32((_drawExtent.minX + lonDelta) / Math.Pow(10, aE)) * Math.Pow(10, aE);
                MinX = (Single)((int)(_drawExtent.minX / lonDelta + 1) * 6 * Math.Pow(10, aE));
            }
            else
            {
                lonDelta = (Single)(2 * Math.Pow(10, aE));
                //MinX = Convert.ToInt32((_drawExtent.minX + lonDelta) / Math.Pow(10, aE)) * Math.Pow(10, aE);
                MinX = (Single)((int)(_drawExtent.minX / lonDelta + 1) * 2 * Math.Pow(10, aE));
            }
            if (MinX - lonDelta == _drawExtent.minX)
            {
                MinX = (Single)(_drawExtent.minX);
            }
            else if (MinX - lonDelta > _drawExtent.minX)
            {
                MinX = MinX - lonDelta;
            }
            string aStr = lonDelta.ToString();
            int lonValidNum;
            if (aStr.IndexOf(".") < 0)
            {
                lonValidNum = 0;
            }
            else
            {
                lonValidNum = aStr.Substring(aStr.IndexOf(".") + 1).TrimEnd('0').Length;
            }
            string lonVF = "f" + lonValidNum.ToString();

            //Latitude
            eStr = latDelta.ToString("e1");
            aD = int.Parse(eStr.Substring(0, 1));
            aE = int.Parse(eStr.Substring(eStr.IndexOf("e") + 1, eStr.Length - eStr.IndexOf("e") - 1));
            if (aD > 4)
            {
                latDelta = (Single)(Math.Pow(10, aE + 1));
                //MinY = Convert.ToInt32((_drawExtent.minY + latDelta) / Math.Pow(10, aE + 1)) * Math.Pow(10, aE + 1);
                MinY = (int)(_drawExtent.minY / latDelta + 1) * latDelta;
            }
            else if (aD <= 4 && aD >= 3)
            {
                latDelta = (Single)(5 * Math.Pow(10, aE));
                //MinY = Convert.ToInt32((_drawExtent.minY + latDelta) / Math.Pow(10, aE)) * Math.Pow(10, aE);
                MinY = (int)(_drawExtent.minY / latDelta + 1) * latDelta;
            }
            else if (aD <= 2 && aD >= 1)
            {
                latDelta = (Single)(3 * Math.Pow(10, aE));
                //MinY = Convert.ToInt32((_drawExtent.minY + latDelta) / Math.Pow(10, aE)) * Math.Pow(10, aE);
                MinY = (int)(_drawExtent.minY / latDelta + 1) * latDelta;
            }
            else
            {
                latDelta = (Single)(Math.Pow(10, aE));
                //MinY = Convert.ToInt32((_drawExtent.minY + latDelta) / Math.Pow(10, aE)) * Math.Pow(10, aE);
                MinY = (int)(_drawExtent.minY / latDelta + 1) * latDelta;
            }
            if (MinY - latDelta == _drawExtent.minY)
            {
                MinY = (Single)(_drawExtent.minY);
            }
            else if (MinY - latDelta > _drawExtent.minY)
            {
                MinY = MinY - latDelta;
            }
            aStr = latDelta.ToString();
            int latValidNum;
            if (aStr.IndexOf(".") < 0)
            {
                latValidNum = 0;
            }
            else
            {
                latValidNum = aStr.Substring(aStr.IndexOf(".") + 1).TrimEnd('0').Length;
            }
            string latVF = "f" + latValidNum.ToString();

            int i = 0;
            double lon = MinX;
            PointF sP, eP;
            sP = new PointF(0, 0);
            eP = new PointF(0, 0);
            Single X = 0, Y = 0;
            Pen aPen = new Pen(_gridLineColor);
            aPen.DashStyle = _gridLineStyle;
            aPen.Width = _gridLineSize;
            //Font drawFont = new Font("Arial", 7);
            string drawStr;
            //SizeF aSF;
            //SolidBrush aBrush = new SolidBrush(this.ForeColor);

            //Draw longitude
            _xGridPosLabel.Clear();
            while (true)
            {
                lon = MinX + i * lonDelta;
                if (lon > _drawExtent.maxX || lon < _drawExtent.minX)
                {
                    break;
                }
                ProjToScreen(lon, _drawExtent.minY, ref X, ref Y, 0);                
                sP.X = X;
                sP.Y = Y;
                if (_drawGridTickLine)
                {
                    eP.X = sP.X;
                    eP.Y = sP.Y - 5;
                    aPen.Color = this.ForeColor;
                    aPen.DashStyle = DashStyle.Solid;
                    aPen.Width = 1;                    
                }
                else
                {
                    ProjToScreen(lon, _drawExtent.maxY, ref X, ref Y, 0);
                    eP.X = X;
                    eP.Y = Y;
                }
                if (lon > _drawExtent.minX || lon < _drawExtent.maxX)
                {
                    g.DrawLine(aPen, sP, eP);
                }
                if (lon < 0)
                {
                    lon = lon + 360;
                }
                if (lon > 0 && lon < 180)
                {
                    drawStr = lon.ToString(lonVF) + "E";
                }
                else if (lon == 0 || lon == 360)
                {
                    drawStr = "0";
                }
                else if (lon == 180)
                {
                    drawStr = "180";
                }
                else
                {
                    drawStr = (360 - lon).ToString(lonVF) + "W";
                }

                //aSF = g.MeasureString(drawStr, drawFont);
                //g.DrawString(drawStr, drawFont, aBrush, sP.X - aSF.Width / 2, sP.Y + 2);
                object[] aPosLab = new object[2];
                aPosLab[0] = sP;
                aPosLab[1] = drawStr;
                _xGridPosLabel.Add(aPosLab);

                i++;
            }

            //Draw latitude
            _yGridPosLabel.Clear();
            i = 0;
            double lat = MinY;
            while (true)
            {
                lat = MinY + i * latDelta;
                if (lat > _drawExtent.maxY)
                {
                    break;
                }
                ProjToScreen(_drawExtent.minX, lat, ref X, ref Y, 0);
                sP.X = X;
                sP.Y = Y;
                if (_drawGridTickLine)
                {
                    eP.X = sP.X + 5;
                    eP.Y = sP.Y;
                }
                else
                {                    
                    ProjToScreen(_drawExtent.maxX, lat, ref X, ref Y, 0);
                    eP.X = X;
                    eP.Y = Y;
                }
                if (lat > _drawExtent.minY || lat < _drawExtent.maxY)
                {
                    g.DrawLine(aPen, sP, eP);
                }

                if (lat > 0)
                {
                    drawStr = lat.ToString(latVF) + "N";
                }
                else if (lat < 0)
                {
                    drawStr = (-lat).ToString(latVF) + "S";
                }
                else
                {
                    drawStr = "EQ";
                }

                //aSF = g.MeasureString(drawStr, drawFont);
                //g.DrawString(drawStr, drawFont, aBrush, sP.X - aSF.Width - 5, sP.Y - aSF.Height / 2);
                object[] aPosLab = new object[2];
                aPosLab[0] = sP;
                aPosLab[1] = drawStr;
                _yGridPosLabel.Add(aPosLab);

                i++;
            }

            aPen.Dispose();
        }

        private void DrawXYGrid(Graphics g, List<string> XGridStrs, List<string> YGridStrs)
        {
            if (LayerSet.LayerNum == 0)
            {
                return;
            }

            int XDelt, YDelt, vXNum, vYNum;
            vXNum = (int)(_drawExtent.maxX - _drawExtent.minX);
            vYNum = (int)(_drawExtent.maxY - _drawExtent.minY);
            XDelt = (int)vXNum / 10 + 1;
            YDelt = (int)vYNum / 10 + 1;

            int i;
            PointF sP, eP;
            sP = new PointF(0, 0);
            eP = new PointF(0, 0);
            float X = 0, Y = 0;
            Pen aPen = new Pen(_gridLineColor);
            aPen.DashStyle = _gridLineStyle;
            aPen.Width = _gridLineSize;
            //Font drawFont = new Font("Arial", 7);
            if (!_drawGridLine)
            {
                aPen.Color = this.ForeColor;
                aPen.DashStyle = DashStyle.Solid;
                aPen.Width = 1;
            }
            //SolidBrush aBrush = new SolidBrush(this.ForeColor);
            string drawStr;
            //SizeF aSF;
            int XGridNum = XGridStrs.Count;
            int YGridNum = YGridStrs.Count;

            //Draw X grid
            //_xGridPosLabel.Clear();
            _gridLabels = new List<GridLabel>();
            for (i = 0; i < XGridNum; i += XDelt)
            {
                if (i >= _drawExtent.minX && i <= _drawExtent.maxX)
                {
                    ProjToScreen(i, _drawExtent.minY, ref X, ref Y, 0);
                    sP.X = X;
                    sP.Y = Y;

                    if (_drawGridLine)
                    {
                        ProjToScreen(i, _drawExtent.maxY, ref X, ref Y, 0);
                        eP.X = X;
                        eP.Y = Y;
                        if (i > _drawExtent.minX && i < _drawExtent.maxX)
                        {
                            g.DrawLine(aPen, sP, eP);
                        }
                    }

                    drawStr = XGridStrs[i];
                    
                    //object[] aPosLab = new object[2];
                    //aPosLab[0] = sP;
                    //aPosLab[1] = drawStr;
                    //_xGridPosLabel.Add(aPosLab);

                    GridLabel aGL = new GridLabel();
                    aGL.IsBorder = true;
                    aGL.LabPoint = new PointD(sP.X, sP.Y);
                    aGL.LabDirection = Direction.South;
                    aGL.LabString = drawStr;
                    _gridLabels.Add(aGL);
                }
            }

            //Draw Y grid   
            _yGridPosLabel.Clear();         
            for (i = 0; i < YGridNum; i += YDelt)
            {
                if (i > _drawExtent.minY && i < _drawExtent.maxY)
                {
                    ProjToScreen(_drawExtent.minX, i, ref X, ref Y, 0);
                    sP.X = X;
                    sP.Y = Y;

                    if (_drawGridLine)
                    {
                        ProjToScreen(_drawExtent.maxX, i, ref X, ref Y, 0);
                        eP.X = X;
                        eP.Y = Y;
                        if (i > _drawExtent.minY && i < _drawExtent.maxY)
                        {
                            g.DrawLine(aPen, sP, eP);
                        }
                    }

                    drawStr = YGridStrs[i];         
           
                    //object[] aPosLab = new object[2];
                    //aPosLab[0] = sP;
                    //aPosLab[1] = drawStr;
                    //_yGridPosLabel.Add(aPosLab);

                    GridLabel aGL = new GridLabel();
                    aGL.IsBorder = true;
                    aGL.LabPoint = new PointD(sP.X, sP.Y);
                    aGL.LabDirection = Direction.Weast;
                    aGL.LabString = drawStr;
                    _gridLabels.Add(aGL);
                }
            }

            aPen.Dispose();
        }        

        private void DrawProjectedLonLat(Graphics g)
        {
            if (_lonLatLayer != null)
            {
                LegendScheme aLS = _lonLatLayer.LegendScheme;
                PolyLineBreak aPLB = (PolyLineBreak)aLS.LegendBreaks[0];
                aPLB.Color = _gridLineColor;
                aPLB.Size = _gridLineSize;
                aPLB.DashStyle = _gridLineStyle;
                DrawLonLatLayer(_lonLatLayer, g, 0);
            }

            if (_gridLabels.Count == 0)
            {
                GetLonLatGridLabels();
            }
        }

        private void GetLonLatGridLabels()
        {
            if (_isGeoMap)
            {
                _gridLabels = new List<GridLabel>();
                if (_projection.IsLonLatMap)
                {
                    if (_lonLatLayer == null)
                        return;

                    for (int i = 0; i < _lonLatLayer.ShapeList.Count; i++)
                    {
                        PolylineShape aPLS = (PolylineShape)_lonLatLayer.ShapeList[i];
                        string labStr = _lonLatLayer.GetCellValue(0, i).ToString().Trim();
                        float value = float.Parse(labStr);
                        string isLonStr = _lonLatLayer.GetCellValue(1, i).ToString();
                        bool isLon = (isLonStr == "Y");
                        if (isLon)
                        {
                            if (value == -180)
                                labStr = "180";
                            else if (!(value == 0 || value == 180))
                            {
                                if (labStr.Substring(0, 1) == "-")
                                    labStr = labStr.Substring(1) + "W";
                                else
                                    labStr = labStr + "E";
                            }
                        }
                        else
                        {
                            if (!(value == 0))
                            {
                                if (labStr.Substring(0, 1) == "-")
                                    labStr = labStr.Substring(1) + "S";
                                else
                                    labStr = labStr + "N";
                            }
                        }
                        List<GridLabel> gLabels = new List<GridLabel>();
                        for (int l = 0; l < aPLS.PolyLines.Count; l++)
                        {
                            PolyLine aPL = aPLS.PolyLines[l];
                            gLabels.AddRange(GeoComputation.GetGridLabels_StraightLine(aPL, _drawExtent, isLon));

                            if (isLon)
                            {
                                List<PointD> aPList = new List<PointD>(aPL.PointList);
                                for (int j = 0; j < aPList.Count; j++)
                                {
                                    PointD aP = (PointD)aPList[j].Clone();
                                    aP.X = aP.X + 360;
                                    aPList[j] = aP;
                                }
                                aPL = new PolyLine();
                                aPL.PointList = aPList;
                                gLabels.AddRange(GeoComputation.GetGridLabels_StraightLine(aPL, _drawExtent, isLon));
                                for (int j = 0; j < aPList.Count; j++)
                                {
                                    PointD aP = (PointD)aPList[j].Clone();
                                    aP.X = aP.X - 720;
                                    aPList[j] = aP;
                                }
                                aPL = new PolyLine();
                                aPL.PointList = aPList;
                                gLabels.AddRange(GeoComputation.GetGridLabels_StraightLine(aPL, _drawExtent, isLon));
                            }
                        }

                        for (int j = 0; j < gLabels.Count; j++)
                            gLabels[j].LabString = labStr;

                        _gridLabels.AddRange(gLabels);
                    }
                }
                else
                {
                    if (_lonLatLayer == null)
                        return;

                    List<GridLabel> gridLabels = new List<GridLabel>();
                    for (int i = 0; i < _lonLatLayer.ShapeList.Count; i++)
                    {
                        PolylineShape aPLS = (PolylineShape)_lonLatLayer.ShapeList[i];
                        string labStr = _lonLatLayer.GetCellValue(0, i).ToString().Trim();
                        float value = float.Parse(labStr);
                        if (value == -9999.0)
                            continue;

                        string isLonStr = _lonLatLayer.GetCellValue(1, i).ToString();
                        bool isLon = (isLonStr == "Y");
                        if (isLon)
                        {
                            if (value == -180)
                                labStr = "180";
                            else if (!(value == 0 || value == 180))
                            {
                                if (labStr.Substring(0, 1) == "-")
                                    labStr = labStr.Substring(1) + "W";
                                else
                                    labStr = labStr + "E";
                            }
                        }
                        else
                        {
                            if (value == 90 || value == -90)
                                continue;

                            if (!(value == 0))
                            {
                                if (labStr.Substring(0, 1) == "-")
                                    labStr = labStr.Substring(1) + "S";
                                else
                                    labStr = labStr + "N";
                            }
                        }
                        List<GridLabel> gLabels = new List<GridLabel>();
                        foreach (PolyLine aPL in aPLS.PolyLines)
                            gLabels.AddRange(GeoComputation.GetGridLabels(aPL, _drawExtent, isLon));

                        for (int j = 0; j < gLabels.Count; j++)
                        {
                            gLabels[j].LabString = labStr;
                            gLabels[j].Value = value;
                        }

                        gridLabels.AddRange(gLabels);
                    }

                    //Adjust for diferent projections
                    float refLon;
                    switch (_projection.ProjInfo.Transform.ProjectionName)
                    {
                        case ProjectionNames.Lambert_Conformal:
                            foreach (GridLabel aGL in gridLabels)
                            {
                                if (!aGL.IsBorder)
                                {
                                    if (!aGL.IsLon)
                                        aGL.LabDirection = Direction.North;
                                    else
                                    {
                                        if (aGL.LabPoint.Y > 0 && Math.Abs(aGL.LabPoint.X) < 1000)
                                            continue;

                                        if (MIMath.LonDistance(aGL.Value, (float)_projection.ProjInfo.CentralMeridian) > 60)
                                        {
                                            if (aGL.LabPoint.X < 0)
                                                aGL.LabDirection = Direction.Weast;
                                            else
                                                aGL.LabDirection = Direction.East;
                                        }
                                        else
                                            aGL.LabDirection = Direction.South;
                                    }
                                }
                                _gridLabels.Add(aGL);
                            }                            
                            break;
                        case ProjectionNames.Albers_Conic_Equal_Area:
                            foreach (GridLabel aGL in gridLabels)
                            {
                                if (!aGL.IsBorder)
                                {
                                    if (!aGL.IsLon)
                                        aGL.LabDirection = Direction.North;
                                    else
                                    {
                                        if (aGL.LabPoint.Y > 7000000 && Math.Abs(aGL.LabPoint.X) < 5000000)
                                            continue;

                                        if (MIMath.LonDistance(aGL.Value, (float)_projection.ProjInfo.CentralMeridian) > 60)
                                        {
                                            if (aGL.LabPoint.X < 0)
                                                aGL.LabDirection = Direction.Weast;
                                            else
                                                aGL.LabDirection = Direction.East;
                                        }
                                        else
                                            aGL.LabDirection = Direction.South;
                                    }
                                }
                                _gridLabels.Add(aGL);
                            }  
                            break;
                        case ProjectionNames.North_Polar_Stereographic:
                        case ProjectionNames.South_Polar_Stereographic:
                            foreach (GridLabel aGL in gridLabels)
                            {
                                if (!aGL.IsBorder)
                                {
                                    if (aGL.IsLon)
                                    {
                                        if (Math.Abs(aGL.LabPoint.X) < 1000 && Math.Abs(aGL.LabPoint.Y) < 1000)
                                            continue;

                                        refLon = (float)_projection.ProjInfo.CentralMeridian;
                                        if (MIMath.LonDistance(aGL.Value, refLon) < 45)
                                        {
                                            if (_projection.ProjInfo.Transform.ProjectionName == ProjectionNames.North_Polar_Stereographic)
                                                aGL.LabDirection = Direction.South;
                                            else
                                                aGL.LabDirection = Direction.North;
                                        }
                                        else
                                        {
                                            refLon = MIMath.LonAdd(refLon, 180);
                                            if (MIMath.LonDistance(aGL.Value, refLon) < 45)
                                            {
                                                if (_projection.ProjInfo.Transform.ProjectionName == ProjectionNames.North_Polar_Stereographic)
                                                    aGL.LabDirection = Direction.North;
                                                else
                                                    aGL.LabDirection = Direction.South;
                                            }
                                            else
                                            {
                                                if (aGL.LabPoint.X < 0)
                                                    aGL.LabDirection = Direction.Weast;
                                                else
                                                    aGL.LabDirection = Direction.East;
                                            }
                                        }
                                    }
                                    else
                                        continue;                                   
                                }

                                _gridLabels.Add(aGL);
                            }
                            break;
                        case ProjectionNames.Robinson:
                            foreach (GridLabel aGL in gridLabels)
                            {
                                if (!aGL.IsBorder)
                                {
                                    if (aGL.IsLon)
                                    {
                                        if (aGL.LabPoint.Y < 0)
                                            aGL.LabDirection = Direction.South;
                                        else
                                            aGL.LabDirection = Direction.North;
                                    }
                                    else
                                    {
                                        if (aGL.LabPoint.X < 0)
                                            aGL.LabDirection = Direction.Weast;
                                        else
                                            aGL.LabDirection = Direction.East;
                                    }
                                }

                                _gridLabels.Add(aGL);
                            }
                            break;
                        case ProjectionNames.Mollweide:
                            foreach (GridLabel aGL in gridLabels)
                            {
                                if (!aGL.IsBorder)
                                {
                                    if (aGL.IsLon)
                                        continue;
                                    else
                                    {
                                        if (aGL.LabPoint.X < 0)
                                            aGL.LabDirection = Direction.Weast;
                                        else
                                            aGL.LabDirection = Direction.East;
                                    }
                                }

                                _gridLabels.Add(aGL);
                            }
                            break;
                        case ProjectionNames.Orthographic:
                        case ProjectionNames.Geostationary:
                            foreach (GridLabel aGL in gridLabels)
                            {
                                if (!aGL.IsBorder)
                                {
                                    if (aGL.IsLon)
                                        continue;
                                    else
                                    {
                                        if (aGL.LabPoint.X < 0)
                                            aGL.LabDirection = Direction.Weast;
                                        else
                                            aGL.LabDirection = Direction.East;
                                    }
                                }

                                _gridLabels.Add(aGL);
                            }
                            break;
                        case ProjectionNames.Oblique_Stereographic:
                        case ProjectionNames.Transverse_Mercator:
                            foreach (GridLabel aGL in gridLabels)
                            {
                                if (!aGL.IsBorder)
                                    continue;

                                _gridLabels.Add(aGL);
                            }
                            break;
                        default:
                            _gridLabels = gridLabels;
                            break;
                    }
                }

                for (int i = 0; i < _gridLabels.Count; i++)
                {
                    GridLabel aGL = _gridLabels[i];
                    double X = 0, Y = 0;
                    ProjToScreen(aGL.LabPoint.X, aGL.LabPoint.Y, ref X, ref Y);
                    aGL.LabPoint = new PointD(X, Y);
                    _gridLabels[i] = aGL;
                }
            }
        }

        private void UpdateLonLatLayer()
        {
            if (_lonLatLayer == null || _gridDeltChanged)
            {
                _lonLatLayer = GenerateLonLatLayer();
                if (!_projection.IsLonLatMap)
                {
                    ProjectionInfo toProj = Projection.ProjInfo;
                    Projection.ProjectLayer(_lonLatLayer, toProj);
                }                

                _gridDeltChanged = false;
            }
        }

        /// <summary>
        /// Generate longitude/latitude grid line layer
        /// </summary>
        /// <returns>lon/lat layer</returns>
        public VectorLayer GenerateLonLatLayer()
        {
            return GenerateLonLatLayer(_gridXOrigin, _gridYOrigin, _gridXDelt, _gridYDelt);
        }

        /// <summary>
        /// Generate Lon/Lat layer
        /// </summary>
        /// <returns>vector layer</returns>
        public VectorLayer GenerateLonLatLayer_Back(double Delt_Lon, double Delt_Lat)
        {
            //Create lon/lat layer                        
            PolylineShape aPLS = new PolylineShape();
            int lineNum;
            double lon, lat;
            //PointD wPoint = new PointD();
            List<PointD> PList = new List<PointD>();                

            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
            string columnName = "Value";
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = columnName;
            aDC.DataType = typeof(Single);
            aLayer.EditAddField(aDC);
            int shapeNum;

            //Longitude
            lineNum = 0;
            Extent extent = new Extent();
            lon = -180;
            while (lon <= 180)
            {                                                
                lat = -90;
                while (lat < 90)
                {
                    aPLS = new PolylineShape();
                    aPLS.value = lon;
                    extent.minX = lon;
                    extent.maxX = lon;
                    extent.minY = lat;                                        
                    PList = new List<PointD>();                    
                    PList.Add(new PointD(lon, lat));
                    lat += 1;
                    PList.Add(new PointD(lon, lat));                                        
                    extent.maxY = lat;
                    aPLS.Extent = extent;

                    aPLS.Points = PList;

                    shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aPLS, shapeNum))
                        aLayer.EditCellValue(columnName, shapeNum, lon);
                }
                                
                lineNum += 1;
                lon += Delt_Lon;
            }            


            //Latitue
            lat = -90;
            while (lat <=90 )
            {                                
                lon = -180;
                while(lon < 180)
                {
                    aPLS = new PolylineShape();
                    aPLS.value = lat;
                    extent.minX = lon;                    
                    extent.minY = lat;
                    extent.maxY = lat;                    
                    PList = new List<PointD>();                    
                    PList.Add(new PointD(lon, lat));
                    lon += 1;
                    PList.Add(new PointD(lon, lat));
                    extent.maxX = lon;
                    aPLS.Extent = extent;

                    aPLS.Points = PList;

                    shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aPLS, shapeNum))
                        aLayer.EditCellValue(columnName, shapeNum, lat);
                }               

                lineNum += 1;
                lat += Delt_Lat;
            }            

            //Generate layer
            Extent lExt = new Extent();
            lExt.minX = -180;
            lExt.maxX = 180;
            lExt.minY = -90;
            lExt.maxY = 90;

            aLayer.Extent = lExt;
            aLayer.LayerName = "Map_LonLat";
            aLayer.FileName = "";
            aLayer.LayerDrawType = LayerDrawType.Map;
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
            PolyLineBreak aPLB = (PolyLineBreak)aLayer.LegendScheme.LegendBreaks[0];
            aPLB.Style = LineStyles.Dash;
            aLayer.LegendScheme.LegendBreaks[0] = aPLB;
            aLayer.Visible = true;

            //Get projected lon/lat layer   
            return aLayer;
        }

        private VectorLayer GenerateLonLatLayer(double origin_Lon, double origin_Lat, double Delt_Lon, double Delt_Lat)
        {
            //Create lon/lat layer                        
            PolylineShape aPLS = new PolylineShape();
            int lineNum;
            double lon, lat;
            //PointD wPoint = new PointD();
            List<PointD> PList = new List<PointD>();

            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
            string columnName = "Value";
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = columnName;
            aDC.DataType = typeof(Single);
            aLayer.EditAddField(aDC);
            aDC = new DataColumn("Longitude", typeof(string));
            aLayer.EditAddField(aDC);
            int shapeNum;

            double refLon = _projection.RefCutLon;

            //Longitude
            lineNum = 0;
            Extent extent = new Extent();
            bool isLabelLon = false;
            lon = origin_Lon;
            while (true)
            {
                if (lon >= origin_Lon && lineNum > 0 && lon - Delt_Lon < origin_Lon)
                    break;

                if (lon > 180)
                    lon = lon - 360;

                if (!_projection.IsLonLatMap)
                {
                    if (refLon == 180 || refLon == -180)
                    {
                        if (lon == 180 || lon == -180)
                        {
                            isLabelLon = true;
                            lon += Delt_Lon;
                            continue;
                        }
                    }
                    else
                    {
                        if (MIMath.DoubleEquals(lon, refLon))
                        {
                            isLabelLon = true;
                            lon += Delt_Lon;
                            continue;
                        }
                    }
                }

                aPLS = new PolylineShape();
                aPLS.value = lon;
                extent.minX = lon;
                extent.maxX = lon;
                extent.minY = -90;
                extent.maxY = 90;
                aPLS.Extent = extent;
                PList = new List<PointD>();

                lat = -90;
                while (lat <= 90)
                {

                    PList.Add(new PointD(lon, lat));
                    lat += 1;
                }
                aPLS.Points = PList;

                shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPLS, shapeNum))
                {
                    aLayer.EditCellValue(0, shapeNum, lon);
                    aLayer.EditCellValue(1, shapeNum, "Y");
                }

                lineNum += 1;
                lon += Delt_Lon;
            }

            //Add longitudes around reference longitude
            switch (_projection.ProjInfo.Transform.ProjectionName)
            {
                case ProjectionNames.Lon_Lat:
                case ProjectionNames.Oblique_Stereographic:
                    break;
                default:
                    double value;
                    lon = refLon - 0.0001;
                    if (lon < -180)
                        lon += 360;
                    aPLS = new PolylineShape();
                    aPLS.value = lon;
                    extent.minX = lon;
                    extent.maxX = lon;
                    extent.minY = -90;
                    extent.maxY = 90;
                    aPLS.Extent = extent;
                    PList = new List<PointD>();

                    lat = -90;
                    while (lat <= 90)
                    {

                        PList.Add(new PointD(lon, lat));
                        lat += 1;
                    }
                    aPLS.Points = PList;

                    if (isLabelLon)
                        value = refLon;
                    else
                        value = -9999.0;
                    shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aPLS, shapeNum))
                    {
                        aLayer.EditCellValue(0, shapeNum, value);
                        aLayer.EditCellValue(1, shapeNum, "Y");
                    }

                    lon = refLon + 0.0001;
                    if (lon > 180)
                        lon -= 360;
                    aPLS = new PolylineShape();
                    aPLS.value = lon;
                    extent.minX = lon;
                    extent.maxX = lon;
                    extent.minY = -90;
                    extent.maxY = 90;
                    aPLS.Extent = extent;
                    PList = new List<PointD>();

                    lat = -90;
                    while (lat <= 90)
                    {

                        PList.Add(new PointD(lon, lat));
                        lat += 1;
                    }
                    aPLS.Points = PList;

                    if (isLabelLon)
                        value = refLon;
                    else
                        value = -9999.0;
                    shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aPLS, shapeNum))
                    {
                        aLayer.EditCellValue(0, shapeNum, value);
                        aLayer.EditCellValue(1, shapeNum, "Y");
                    }
                    break;
            }

            //Latitue
            //lat = -90;
            lat = origin_Lat;
            while (lat <= 90)
            {
                aPLS = new PolylineShape();
                aPLS.value = lat;
                extent.minX = -180;
                extent.minY = lat;
                extent.maxY = lat;
                extent.maxX = 180;
                aPLS.Extent = extent;
                PList = new List<PointD>();

                lon = -180;
                while (lon <= 180)
                {
                    PList.Add(new PointD(lon, lat));
                    lon += 1;
                }
                aPLS.Points = PList;

                shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPLS, shapeNum))
                {
                    aLayer.EditCellValue(0, shapeNum, lat);
                    aLayer.EditCellValue(1, shapeNum, "N");
                }

                lineNum += 1;
                lat += Delt_Lat;
            }

            //Generate layer
            Extent lExt = new Extent();
            lExt.minX = -180;
            lExt.maxX = 180;
            lExt.minY = -90;
            lExt.maxY = 90;

            aLayer.Extent = lExt;
            aLayer.LayerName = "Map_LonLat";
            aLayer.FileName = "";
            aLayer.LayerDrawType = LayerDrawType.Map;
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
            PolyLineBreak aPLB = (PolyLineBreak)aLayer.LegendScheme.LegendBreaks[0];
            aPLB.Style = LineStyles.Dash;
            aLayer.LegendScheme.LegendBreaks[0] = aPLB;
            aLayer.Visible = true;

            //Get projected lon/lat layer   
            return aLayer;
        }

        /// <summary>
        /// Generate Lon/Lat layer
        /// </summary>
        /// <returns>vector layer</returns>
        public VectorLayer GenerateLonLatLayer_Old(double Delt_Lon, double Delt_Lat)
        {
            //Create lon/lat layer                        
            PolylineShape aPLS = new PolylineShape();
            int lineNum;
            double lon, lat;
            //PointD wPoint = new PointD();
            List<PointD> PList = new List<PointD>();

            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
            string columnName = "Value";
            DataColumn aDC = new DataColumn();
            aDC.ColumnName = columnName;
            aDC.DataType = typeof(Single);
            aLayer.EditAddField(aDC);
            aDC = new DataColumn("Longitude", typeof(string));
            aLayer.EditAddField(aDC);
            int shapeNum;

            //Longitude
            lineNum = 0;
            Extent extent = new Extent();
            lon = -180;
            while (lon <= 180)
            {
                aPLS = new PolylineShape();
                aPLS.value = lon;
                extent.minX = lon;
                extent.maxX = lon;
                extent.minY = -90;
                extent.maxY = 90;
                aPLS.Extent = extent;
                PList = new List<PointD>();

                lat = -90;
                while (lat <= 90)
                {
                    
                    PList.Add(new PointD(lon, lat));
                    lat += 1;                    
                }
                aPLS.Points = PList;

                shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPLS, shapeNum))
                {
                    aLayer.EditCellValue(0, shapeNum, lon);
                    aLayer.EditCellValue(1, shapeNum, "Y");
                }

                lineNum += 1;
                lon += Delt_Lon;
            }


            //Latitue
            lat = -90;
            while (lat <= 90)
            {
                aPLS = new PolylineShape();
                aPLS.value = lat;
                extent.minX = -180;
                extent.minY = lat;
                extent.maxY = lat;
                extent.maxX = 180;
                aPLS.Extent = extent;
                PList = new List<PointD>();

                lon = -180;
                while (lon <= 180)
                {                    
                    PList.Add(new PointD(lon, lat));
                    lon += 1;  
                }
                aPLS.Points = PList;

                shapeNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPLS, shapeNum))
                {
                    aLayer.EditCellValue(0, shapeNum, lat);
                    aLayer.EditCellValue(1, shapeNum, "N");
                }

                lineNum += 1;
                lat += Delt_Lat;
            }

            //Generate layer
            Extent lExt = new Extent();
            lExt.minX = -180;
            lExt.maxX = 180;
            lExt.minY = -90;
            lExt.maxY = 90;

            aLayer.Extent = lExt;
            aLayer.LayerName = "Map_LonLat";
            aLayer.FileName = "";
            aLayer.LayerDrawType = LayerDrawType.Map;
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
            PolyLineBreak aPLB = (PolyLineBreak)aLayer.LegendScheme.LegendBreaks[0];
            aPLB.Style = LineStyles.Dash;
            aLayer.LegendScheme.LegendBreaks[0] = aPLB;
            aLayer.Visible = true;

            //Get projected lon/lat layer   
            return aLayer;
        }

        ///// <summary>
        ///// Generate Lon/Lat layer
        ///// </summary>
        ///// <returns>vector layer</returns>
        //public VectorLayer GenerateLonLatLayer_Back()
        //{
        //    //Create lon/lat layer                        
        //    PolylineShape aPLS = new PolylineShape();
        //    int i, j, lineNum;
        //    double lon, lat;
        //    PointD wPoint = new PointD();
        //    List<PointD> PList = new List<PointD>();
        //    double refLon;
        //    int refIntLon;

        //    //Define coordinate transform            
        //    ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
        //    ProjectionInfo toProj = _projection.ProjInfo;

        //    refLon = _projection.RefCutLon;
        //    refIntLon = (int)refLon;

        //    VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
        //    string columnName = "Value";
        //    DataColumn aDC = new DataColumn();
        //    aDC.ColumnName = columnName;
        //    aDC.DataType = typeof(Single);
        //    aLayer.EditAddField(aDC);
        //    int shapeNum;

        //    //Longitude
        //    lineNum = 0;
        //    Extent extent;
        //    for (i = 0; i <= 36; i++)
        //    {
        //        lon = -180 + i * 10;
        //        if (lon == refIntLon)
        //        {
        //            continue;
        //        }
        //        aPLS = new PolylineShape();
        //        aPLS.value = lon;
        //        extent.minX = lon;
        //        extent.maxX = lon;
        //        extent.minY = -90;
        //        extent.maxY = 90;
        //        aPLS.Extent = extent;
        //        PList = new List<PointD>();
        //        for (j = -90; j <= 90; j++)
        //        {
        //            wPoint.X = lon;
        //            wPoint.Y = j;
        //            PList.Add(wPoint);
        //        }
        //        switch (toProj.Transform.ProjectionName)
        //        {
        //            case ProjectionNames.Lambert_Conformal:
        //            case ProjectionNames.North_Polar_Stereographic:
        //                PList.RemoveRange(0, 10);
        //                extent.minY = -80;
        //                aPLS.Extent = extent;
        //                break;
        //            case ProjectionNames.South_Polar_Stereographic:
        //                PList.RemoveRange(PList.Count - 10, 10);
        //                extent.maxY = 80;
        //                aPLS.Extent = extent;
        //                break;
        //        }
        //        aPLS.Points = PList;
        //        aPLS.PointNum = PList.Count;

        //        shapeNum = aLayer.ShapeNum;
        //        if (aLayer.EditInsertShape(aPLS, shapeNum))
        //            aLayer.EditCellValue(columnName, shapeNum, lon);

        //        lineNum += 1;
        //    }

        //    //Add longitudes around reference longitude
        //    lon = refLon - 0.0001;
        //    aPLS = new PolylineShape();
        //    aPLS.value = lon;
        //    extent.minX = lon;
        //    extent.maxX = lon;
        //    extent.minY = -80;
        //    extent.maxY = 90;
        //    aPLS.Extent = extent;
        //    PList = new List<PointD>();
        //    for (j = -80; j <= 90; j++)
        //    {
        //        wPoint.X = lon;
        //        wPoint.Y = j;
        //        PList.Add(wPoint);
        //    }
        //    aPLS.Points = PList;
        //    aPLS.PointNum = PList.Count;
        //    shapeNum = aLayer.ShapeNum;
        //    if (aLayer.EditInsertShape(aPLS, shapeNum))
        //        aLayer.EditCellValue(columnName, shapeNum, lon);

        //    lineNum += 1;

        //    lon = refLon + 0.0001;
        //    aPLS = new PolylineShape();
        //    aPLS.value = lon;
        //    extent.minX = lon;
        //    extent.maxX = lon;
        //    extent.minY = -80;
        //    extent.maxY = 90;
        //    aPLS.Extent = extent;
        //    PList = new List<PointD>();
        //    for (j = -80; j <= 90; j++)
        //    {
        //        wPoint.X = lon;
        //        wPoint.Y = j;
        //        PList.Add(wPoint);
        //    }
        //    aPLS.Points = PList;
        //    aPLS.PointNum = PList.Count;

        //    shapeNum = aLayer.ShapeNum;
        //    if (aLayer.EditInsertShape(aPLS, shapeNum))
        //        aLayer.EditCellValue(columnName, shapeNum, lon);

        //    lineNum += 1;


        //    //Latitue
        //    for (i = 0; i <= 18; i++)
        //    {
        //        lat = -90 + i * 10;
        //        aPLS = new PolylineShape();
        //        aPLS.value = lat;
        //        extent.minX = -180;
        //        extent.maxX = 180;
        //        extent.minY = lat;
        //        extent.maxY = lat;
        //        aPLS.Extent = extent;
        //        PList = new List<PointD>();
        //        switch (toProj.Transform.ProjectionName)
        //        {
        //            case ProjectionNames.Lambert_Conformal:
        //                if (i == 0)
        //                {
        //                    continue;
        //                }
        //                extent.minX = -180;
        //                extent.maxX = refIntLon;
        //                extent.minY = lat;
        //                extent.maxY = lat;
        //                aPLS.Extent = extent;
        //                PList = new List<PointD>();
        //                for (j = -180; j < refIntLon; j++)
        //                {
        //                    wPoint.X = j;
        //                    wPoint.Y = lat;
        //                    PList.Add(wPoint);
        //                }
        //                wPoint.X = refLon - 0.0001;
        //                wPoint.Y = lat;
        //                PList.Add(wPoint);
        //                if (PList.Count > 1)
        //                {
        //                    aPLS.Points = PList;
        //                    aPLS.PointNum = PList.Count;

        //                    shapeNum = aLayer.ShapeNum;
        //                    if (aLayer.EditInsertShape(aPLS, shapeNum))
        //                        aLayer.EditCellValue(columnName, shapeNum, lon);

        //                    lineNum += 1;
        //                }

        //                aPLS = new PolylineShape();
        //                extent.minX = refIntLon;
        //                extent.maxX = 180;
        //                extent.minY = lat;
        //                extent.maxY = lat;
        //                aPLS.Extent = extent;
        //                PList = new List<PointD>();
        //                wPoint.X = refLon + 0.0001;
        //                wPoint.Y = lat;
        //                PList.Add(wPoint);
        //                for (j = refIntLon + 1; j <= 180; j++)
        //                {
        //                    wPoint.X = j;
        //                    wPoint.Y = lat;
        //                    PList.Add(wPoint);
        //                }
        //                if (PList.Count > 1)
        //                {
        //                    aPLS.Points = PList;
        //                    aPLS.PointNum = PList.Count;

        //                    shapeNum = aLayer.ShapeNum;
        //                    if (aLayer.EditInsertShape(aPLS, shapeNum))
        //                        aLayer.EditCellValue(columnName, shapeNum, lon);

        //                    lineNum += 1;
        //                }
        //                break;
        //            case ProjectionNames.North_Polar_Stereographic:
        //                if (i == 0)
        //                {
        //                    continue;
        //                }
        //                for (j = -180; j <= 180; j++)
        //                {
        //                    wPoint.X = j;
        //                    wPoint.Y = lat;
        //                    PList.Add(wPoint);
        //                }
        //                break;
        //            case ProjectionNames.South_Polar_Stereographic:
        //                if (i == 18)
        //                {
        //                    continue;
        //                }
        //                for (j = -180; j <= 180; j++)
        //                {
        //                    wPoint.X = j;
        //                    wPoint.Y = lat;
        //                    PList.Add(wPoint);
        //                }
        //                break;
        //            default:
        //                for (j = -180; j <= 180; j++)
        //                {
        //                    wPoint.X = j;
        //                    wPoint.Y = lat;
        //                    PList.Add(wPoint);
        //                }
        //                break;
        //        }

        //        aPLS.Points = PList;
        //        aPLS.PointNum = PList.Count;

        //        shapeNum = aLayer.ShapeNum;
        //        if (aLayer.EditInsertShape(aPLS, shapeNum))
        //            aLayer.EditCellValue(columnName, shapeNum, lon);

        //        lineNum += 1;
        //    }

        //    //if (m_projectionName == ProjectionName.Orthographic ||
        //    //    m_projectionName == ProjectionName.Transverse_Mercator)
        //    //{                
        //    //    ArrayList NPolylines = new ArrayList();
        //    //    NPolylines = (ArrayList)polylines.Clone();
        //    //    polylines.Clear();
        //    //    for (i = 0; i < NPolylines.Count; i++)
        //    //    {
        //    //        aPLS = (PolylineShape)NPolylines[i];
        //    //        PList = (ArrayList)aPLS.points.Clone();
        //    //        ArrayList NPList = new ArrayList();
        //    //        for (j = 0; j < PList.Count - 10; j += 10)
        //    //        {
        //    //            NPList.Clear();
        //    //            for (int k = 0; k < 11; k++)
        //    //            {
        //    //                NPList.Add(PList[j + k]);
        //    //            }
        //    //            aPLS.points = (ArrayList)NPList.Clone();
        //    //            polylines.Add(aPLS);

        //    //            aDR = aLayer.AttributeTable.Table.NewRow();
        //    //            aDR[aDC] = aPLS.value;
        //    //            aLayer.AttributeTable.Table.Rows.Add(aDR);
        //    //            lineNum += 1;
        //    //        }
        //    //    }
        //    //}            

        //    //Generate layer
        //    Extent lExt = new Extent();
        //    lExt.minX = -180;
        //    lExt.maxX = 180;
        //    lExt.minY = -90;
        //    lExt.maxY = 90;

        //    aLayer.Extent = lExt;
        //    aLayer.LayerName = "Map_LonLat";
        //    aLayer.FileName = "";
        //    aLayer.LayerDrawType = LayerDrawType.Map;
        //    aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
        //    PolyLineBreak aPLB = (PolyLineBreak)aLayer.LegendScheme.LegendBreaks[0];
        //    aPLB.Style = LineStyles.Dash;
        //    aLayer.LegendScheme.LegendBreaks[0] = aPLB;
        //    aLayer.Visible = true;

        //    //Get projected lon/lat layer   
        //    if (!(fromProj.ToProj4String() == toProj.ToProj4String()))
        //        return aLayer;
        //    else
        //    {
        //        VectorLayer lonLatLayer = _projection.ProjectLayer_Proj4(aLayer, fromProj, toProj);
        //        return lonLatLayer;
        //    }
        //}

        /// <summary>
        /// Draw graphic list
        /// </summary>
        /// <param name="g">graphics</param>
        /// <param name="lonShift">longitude shift</param>
        public void DrawGraphicList(Graphics g, double lonShift)
        {
            if (_graphicCollection.Count > 0)
            {
                Extent aExtent = MIMath.ShiftExtentLon(_graphicCollection.Extent, lonShift);
                if (!MIMath.IsExtentCross(aExtent, _drawExtent))
                    return;

                SmoothingMode aSM = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                foreach (Graphic aGraphic in _graphicCollection.GraphicList)
                {
                    DrawGraphic(g, aGraphic, lonShift);                    
                }
                g.SmoothingMode = aSM;
            }
        }

        /// <summary>
        /// Draw a graphic
        /// </summary>
        /// <param name="g">graphics</param>
        /// <param name="aGraphic">graphic</param>
        /// <param name="lonShift">longitude shift</param>
        public void DrawGraphic(Graphics g, Graphic aGraphic, double lonShift)
        {
            Extent aExtent = MIMath.ShiftExtentLon(aGraphic.Shape.Extent, lonShift);            
            if (MIMath.IsExtentCross(aExtent, _drawExtent))
            {
                SmoothingMode sMode = g.SmoothingMode;                
                float X = 0, Y = 0;                             

                //Get screen points
                List<PointD> points = aGraphic.Shape.GetPoints();
                PointF[] screenPoints = new PointF[points.Count];
                for (int i = 0; i < points.Count; i++)
                {
                    ProjToScreen(points[i].X, points[i].Y, ref X, ref Y, lonShift);
                    screenPoints[i] = new PointF(X, Y);
                }

                Region oldRegion = g.Clip;
                switch (aGraphic.Shape.ShapeType)
                {
                    case ShapeTypes.Polygon:                        
                    case ShapeTypes.Rectangle:
                    case ShapeTypes.Circle:
                    case ShapeTypes.CurvePolygon:
                    case ShapeTypes.Ellipse:
                        if (((PolygonBreak)aGraphic.Legend).IsMaskout)
                            SetClipRegion(ref g);
                        break;
                }                
                
                Draw.DrawGrahpic(screenPoints, aGraphic, g, _mouseTool == MouseTools.EditVertices);
                
                g.SmoothingMode = sMode;
                g.Clip = oldRegion;
            }
        }

        #endregion

        #region Graphics
        /// <summary>
        /// Add a graphic
        /// </summary>
        /// <param name="aGraphic">graphic</param>
        public void AddGraphic(Graphic aGraphic)
        {
            _graphicCollection.Add(aGraphic);
        }

        /// <summary>
        /// Add a text label element
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="x">center x</param>
        /// <param name="y">center y</param>
        /// <returns>text layout graphic</returns>
        public Graphic AddText(string text, int x, int y)
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
        public Graphic AddText(string text, int x, int y, float fontSize)
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
        public Graphic AddText(string text, int x, int y, string fontName, float fontSize)
        {
            PointShape aPS = new PointShape();
            aPS.Point = new MeteoInfoC.PointD(x, y);
            LabelBreak aLB = (LabelBreak)_defLabelBreak.Clone();
            aLB.Text = text;
            aLB.Font = new Font(fontName, fontSize);
            Graphic aGraphic = new Graphic(aPS, aLB);
            _graphicCollection.Add(aGraphic);

            return aGraphic;
        }

        #endregion

        #region XML import and export

        #region Export
        /// <summary>
        /// Export project XML content
        /// </summary>
        /// <param name="m_Doc">ref XML document</param>
        /// <param name="parent">parent XML element</param>
        /// <param name="projectFilePath">project file path</param>
        public void ExportProjectXML(ref XmlDocument m_Doc, XmlElement parent, string projectFilePath)
        {
            ExportExtentsElement(ref m_Doc, parent);
            ExportMapPropElement(ref m_Doc, parent);
            ExportGridLineElement(ref m_Doc, parent);
            ExportMaskOutElement(ref m_Doc, parent);
            ExportProjectionElement(ref m_Doc, parent);
            ExportLayers(ref m_Doc, parent, projectFilePath);
            ExportGraphics(ref m_Doc, parent, _graphicCollection.GraphicList);
        }

        /// <summary>
        /// Add extent element
        /// </summary>
        /// <param name="m_Doc">XmlDocument</param>
        /// <param name="parent">parent XmlElement</param>
        public void ExportExtentsElement(ref XmlDocument m_Doc, XmlElement parent)
        {
            XmlElement Extents = m_Doc.CreateElement("Extents");
            XmlAttribute xMin = m_Doc.CreateAttribute("xMin");
            XmlAttribute xMax = m_Doc.CreateAttribute("xMax");
            XmlAttribute yMin = m_Doc.CreateAttribute("yMin");
            XmlAttribute yMax = m_Doc.CreateAttribute("yMax");

            xMin.InnerText = _viewExtent.minX.ToString();
            xMax.InnerText = _viewExtent.maxX.ToString();
            yMin.InnerText = _viewExtent.minY.ToString();
            yMax.InnerText = _viewExtent.maxY.ToString();

            Extents.Attributes.Append(xMin);
            Extents.Attributes.Append(xMax);
            Extents.Attributes.Append(yMin);
            Extents.Attributes.Append(yMax);

            parent.AppendChild(Extents);
        }        

        /// <summary>
        /// Add map property element
        /// </summary>
        /// <param name="m_Doc">XmlDocument</param>
        /// <param name="parent">parent XmlElement</param>
        public void ExportMapPropElement(ref XmlDocument m_Doc, XmlElement parent)
        {
            XmlElement MapProperty = m_Doc.CreateElement("MapProperty");
            XmlAttribute BackColor = m_Doc.CreateAttribute("BackColor");
            XmlAttribute ForeColor = m_Doc.CreateAttribute("ForeColor");
            XmlAttribute SmoothingMode = m_Doc.CreateAttribute("SmoothingMode");
            XmlAttribute pointSmoothingMode = m_Doc.CreateAttribute("PointSmoothingMode");
            XmlAttribute xyScaleFactor = m_Doc.CreateAttribute("XYScaleFactor");
            XmlAttribute multiGlobalDraw = m_Doc.CreateAttribute("MultiGlobalDraw");
            XmlAttribute selectColor = m_Doc.CreateAttribute("SelectColor");
            XmlAttribute highSpeedWheelZoom = m_Doc.CreateAttribute("HighSpeedWheelZoom");

            BackColor.InnerText = ColorTranslator.ToHtml(this.BackColor);
            ForeColor.InnerText = ColorTranslator.ToHtml(this.ForeColor);
            SmoothingMode.InnerText = Enum.GetName(typeof(SmoothingMode), _smoothingMode);
            pointSmoothingMode.InnerText = Enum.GetName(typeof(SmoothingMode), _pointSmoothingMode);
            xyScaleFactor.InnerText = _XYScaleFactor.ToString();
            multiGlobalDraw.InnerText = _multiGlobalDraw.ToString();
            selectColor.InnerText = ColorTranslator.ToHtml(_selectColor);
            highSpeedWheelZoom.InnerText = _highSpeedWheelZoom.ToString();

            MapProperty.Attributes.Append(BackColor);
            MapProperty.Attributes.Append(ForeColor);
            MapProperty.Attributes.Append(SmoothingMode);
            MapProperty.Attributes.Append(pointSmoothingMode);
            MapProperty.Attributes.Append(xyScaleFactor);
            MapProperty.Attributes.Append(multiGlobalDraw);
            MapProperty.Attributes.Append(selectColor);
            MapProperty.Attributes.Append(highSpeedWheelZoom);

            parent.AppendChild(MapProperty);
        }

        /// <summary>
        /// Add grid line element
        /// </summary>
        /// <param name="m_Doc">XmlDocument</param>
        /// <param name="parent">parent XmlElement</param>
        public void ExportGridLineElement(ref XmlDocument m_Doc, XmlElement parent)
        {
            XmlElement GridLine = m_Doc.CreateElement("GridLine");
            //XmlAttribute DrawNeatLine = m_Doc.CreateAttribute("DrawNeatLine");
            //XmlAttribute NeatLineColor = m_Doc.CreateAttribute("NeatLineColor");
            //XmlAttribute NeatLineSize = m_Doc.CreateAttribute("NeatLineSize");
            XmlAttribute GridLineColor = m_Doc.CreateAttribute("GridLineColor");
            XmlAttribute GridLineSize = m_Doc.CreateAttribute("GridLineSize");
            XmlAttribute GridLineStyle = m_Doc.CreateAttribute("GridLineStyle");
            XmlAttribute DrawGridLine = m_Doc.CreateAttribute("DrawGridLine");
            XmlAttribute DrawGridTickLine = m_Doc.CreateAttribute("DrawGridTickLine");

            GridLineColor.InnerText = ColorTranslator.ToHtml(_gridLineColor);
            GridLineSize.InnerText = _gridLineSize.ToString();
            GridLineStyle.InnerText = Enum.GetName(typeof(DashStyle), _gridLineStyle);
            DrawGridLine.InnerText = _drawGridLine.ToString();
            DrawGridTickLine.InnerText = _drawGridTickLine.ToString();

            GridLine.Attributes.Append(GridLineColor);
            GridLine.Attributes.Append(GridLineSize);
            GridLine.Attributes.Append(GridLineStyle);
            GridLine.Attributes.Append(DrawGridLine);
            GridLine.Attributes.Append(DrawGridTickLine);

            parent.AppendChild(GridLine);
        }

        /// <summary>
        /// Add maskout element
        /// </summary>
        /// <param name="m_Doc">XmlDocument</param>
        /// <param name="parent">parent XmlElement</param>
        public void ExportMaskOutElement(ref XmlDocument m_Doc, XmlElement parent)
        {
            XmlElement MaskOut = m_Doc.CreateElement("MaskOut");
            XmlAttribute SetMaskLayer = m_Doc.CreateAttribute("SetMaskLayer");
            XmlAttribute MaskLayer = m_Doc.CreateAttribute("MaskLayer");

            SetMaskLayer.InnerText = _maskOut.SetMaskLayer.ToString();
            MaskLayer.InnerText = _maskOut.MaskLayer.ToString();

            MaskOut.Attributes.Append(SetMaskLayer);
            MaskOut.Attributes.Append(MaskLayer);

            parent.AppendChild(MaskOut);
        }

        /// <summary>
        /// Add prjection element
        /// </summary>
        /// <param name="m_Doc">XmlDocument</param>
        /// <param name="parent">parent XmlElement</param>
        public void ExportProjectionElement(ref XmlDocument m_Doc, XmlElement parent)
        {
            XmlElement Projection = m_Doc.CreateElement("Projection");
            XmlAttribute IsLonLatMap = m_Doc.CreateAttribute("IsLonLatMap");            
            XmlAttribute ProjStr = m_Doc.CreateAttribute("ProjStr");
            XmlAttribute RefLon = m_Doc.CreateAttribute("RefLon");
            XmlAttribute RefCutLon = m_Doc.CreateAttribute("RefCutLon");

            IsLonLatMap.InnerText = _projection.IsLonLatMap.ToString();            
            ProjStr.InnerText = _projection.ProjInfo.ToProj4String();
            RefLon.InnerText = _projection.RefLon.ToString();
            RefCutLon.InnerText = _projection.RefCutLon.ToString();

            Projection.Attributes.Append(IsLonLatMap);            
            Projection.Attributes.Append(ProjStr);
            Projection.Attributes.Append(RefLon);
            Projection.Attributes.Append(RefCutLon);

            parent.AppendChild(Projection);
        }

        private void ExportLayers(ref XmlDocument m_Doc, XmlElement parent, string projectFilePath)
        {
            XmlElement Layers = m_Doc.CreateElement("Layers");

            //Add Layers
            for (int i = 0; i < _layerSet.Layers.Count; i++)
            {
                if (_layerSet.Layers[i].LayerType == LayerTypes.VectorLayer)
                {
                    VectorLayer aVLayer = (VectorLayer)_layerSet.Layers[i];
                    if (File.Exists(aVLayer.FileName))
                        ExportVectorLayerElement(ref m_Doc, Layers, aVLayer, projectFilePath);
                }
                else
                {
                    ImageLayer aILayer = (ImageLayer)_layerSet.Layers[i];
                    if (File.Exists(aILayer.FileName))
                        ExportImageLayer(ref m_Doc, Layers, aILayer, projectFilePath);
                }
            }

            parent.AppendChild(Layers);
        }

        /// <summary>
        /// Add vector layer element
        /// </summary>
        /// <param name="m_Doc"></param>
        /// <param name="parent"></param>
        /// <param name="aVLayer"></param>
        /// <param name="projectFilePath"></param>
        public void ExportVectorLayerElement(ref XmlDocument m_Doc, XmlElement parent, VectorLayer aVLayer,
            string projectFilePath)
        {
            Global.GlobalUtil CGlobal = new MeteoInfoC.Global.GlobalUtil();

            XmlElement Layer = m_Doc.CreateElement("Layer");
            XmlAttribute Handle = m_Doc.CreateAttribute("Handle");
            XmlAttribute LayerName = m_Doc.CreateAttribute("LayerName");
            XmlAttribute FileName = m_Doc.CreateAttribute("FileName");
            XmlAttribute Visible = m_Doc.CreateAttribute("Visible");
            XmlAttribute IsMaskout = m_Doc.CreateAttribute("IsMaskout");
            XmlAttribute LayerType = m_Doc.CreateAttribute("LayerType");
            XmlAttribute LayerDrawType = m_Doc.CreateAttribute("LayerDrawType");
            XmlAttribute ShapeType = m_Doc.CreateAttribute("ShapeType");
            XmlAttribute AvoidCollision = m_Doc.CreateAttribute("AvoidCollision");
            XmlAttribute TransparencyPerc = m_Doc.CreateAttribute("TransparencyPerc");
            XmlAttribute Expanded = m_Doc.CreateAttribute("Expanded");
            //XmlAttribute HasLabels = m_Doc.CreateAttribute("HasLabels");

            Handle.InnerText = aVLayer.Handle.ToString();
            LayerName.InnerText = aVLayer.LayerName;
            FileName.InnerText = CGlobal.GetRelativePath(aVLayer.FileName, projectFilePath);
            Visible.InnerText = aVLayer.Visible.ToString();
            IsMaskout.InnerText = aVLayer.IsMaskout.ToString();
            LayerType.InnerText = Enum.GetName(typeof(LayerTypes), aVLayer.LayerType);
            LayerDrawType.InnerText = Enum.GetName(typeof(LayerDrawType), aVLayer.LayerDrawType);
            ShapeType.InnerText = Enum.GetName(typeof(ShapeTypes), aVLayer.ShapeType);
            AvoidCollision.InnerText = aVLayer.AvoidCollision.ToString();
            TransparencyPerc.InnerText = aVLayer.TransparencyPerc.ToString();
            Expanded.InnerText = aVLayer.Expanded.ToString();
            //HasLabels.InnerText = aVLayer.HasLabels.ToString();

            Layer.Attributes.Append(Handle);
            Layer.Attributes.Append(LayerName);
            Layer.Attributes.Append(FileName);
            Layer.Attributes.Append(Visible);
            Layer.Attributes.Append(IsMaskout);
            Layer.Attributes.Append(LayerType);
            Layer.Attributes.Append(LayerDrawType);
            Layer.Attributes.Append(ShapeType);
            Layer.Attributes.Append(AvoidCollision);
            Layer.Attributes.Append(TransparencyPerc);
            Layer.Attributes.Append(Expanded);
            //Layer.Attributes.Append(HasLabels);

            //Add legend scheme            
            //AddLegendScheme(ref m_Doc, Layer, aVLayer.LegendScheme);
            aVLayer.LegendScheme.ExportToXML(ref m_Doc, Layer);

            //Add label set
            ExportLabelSet(ref m_Doc, Layer, aVLayer.LabelSet);

            //Add graphics
            ExportGraphics(ref m_Doc, Layer, aVLayer.LabelPoints);

            //Add chart set
            ExportChartSet(ref m_Doc, Layer, aVLayer.ChartSet);

            //Add charts
            ExportChartGraphics(ref m_Doc, Layer, aVLayer.ChartPoints);

            //Add visible scale
            ExportVisibleScale(ref m_Doc, Layer, aVLayer.VisibleScale);

            parent.AppendChild(Layer);
        }        

        private void ExportLegendScheme(ref XmlDocument m_Doc, XmlElement parent, LegendScheme aLS)
        {
            XmlElement root = m_Doc.CreateElement("LegendScheme");
            XmlAttribute fieldName = m_Doc.CreateAttribute("FieldName");
            XmlAttribute legendType = m_Doc.CreateAttribute("LegendType");
            XmlAttribute shapeType = m_Doc.CreateAttribute("ShapeType");
            XmlAttribute breakNum = m_Doc.CreateAttribute("BreakNum");
            XmlAttribute hasNoData = m_Doc.CreateAttribute("HasNoData");
            XmlAttribute minValue = m_Doc.CreateAttribute("MinValue");
            XmlAttribute maxValue = m_Doc.CreateAttribute("MaxValue");
            XmlAttribute MissingValue = m_Doc.CreateAttribute("MissingValue");

            fieldName.InnerText = aLS.FieldName;
            legendType.InnerText = Enum.GetName(typeof(LegendType), aLS.LegendType);
            shapeType.InnerText = Enum.GetName(typeof(ShapeTypes), aLS.ShapeType);
            breakNum.InnerText = aLS.BreakNum.ToString();
            hasNoData.InnerText = aLS.HasNoData.ToString();
            minValue.InnerText = aLS.MinValue.ToString();
            maxValue.InnerText = aLS.MaxValue.ToString();
            MissingValue.InnerText = aLS.MissingValue.ToString();

            root.Attributes.Append(fieldName);
            root.Attributes.Append(legendType);
            root.Attributes.Append(shapeType);
            root.Attributes.Append(breakNum);
            root.Attributes.Append(hasNoData);
            root.Attributes.Append(minValue);
            root.Attributes.Append(maxValue);
            root.Attributes.Append(MissingValue);

            XmlElement breaks = m_Doc.CreateElement("Breaks");
            XmlElement brk;
            XmlAttribute caption;
            XmlAttribute startValue;
            XmlAttribute endValue;
            XmlAttribute color;
            XmlAttribute drawShape;
            XmlAttribute size;
            XmlAttribute style;
            XmlAttribute outlineColor;
            XmlAttribute drawOutline;
            XmlAttribute drawFill;
            switch (aLS.ShapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes.PointM:
                    XmlAttribute isNoData;
                    foreach (PointBreak aPB in aLS.LegendBreaks)
                    {
                        brk = m_Doc.CreateElement("Break");
                        caption = m_Doc.CreateAttribute("Caption");
                        startValue = m_Doc.CreateAttribute("StartValue");
                        endValue = m_Doc.CreateAttribute("EndValue");
                        color = m_Doc.CreateAttribute("Color");
                        drawShape = m_Doc.CreateAttribute("DrawShape");
                        outlineColor = m_Doc.CreateAttribute("OutlineColor");
                        size = m_Doc.CreateAttribute("Size");
                        style = m_Doc.CreateAttribute("Style");
                        drawOutline = m_Doc.CreateAttribute("DrawOutline");
                        drawFill = m_Doc.CreateAttribute("DrawFill");
                        isNoData = m_Doc.CreateAttribute("IsNoData");

                        caption.InnerText = aPB.Caption;
                        startValue.InnerText = aPB.StartValue.ToString();
                        endValue.InnerText = aPB.EndValue.ToString();
                        color.InnerText = ColorTranslator.ToHtml(aPB.Color);
                        drawShape.InnerText = aPB.DrawShape.ToString();
                        outlineColor.InnerText = ColorTranslator.ToHtml(aPB.OutlineColor);
                        size.InnerText = aPB.Size.ToString();
                        style.InnerText = Enum.GetName(typeof(PointStyle), aPB.Style);
                        drawOutline.InnerText = aPB.DrawOutline.ToString();
                        drawFill.InnerText = aPB.DrawFill.ToString();
                        isNoData.InnerText = aPB.IsNoData.ToString();

                        brk.Attributes.Append(caption);
                        brk.Attributes.Append(startValue);
                        brk.Attributes.Append(endValue);
                        brk.Attributes.Append(color);
                        brk.Attributes.Append(drawShape);
                        brk.Attributes.Append(outlineColor);
                        brk.Attributes.Append(size);
                        brk.Attributes.Append(style);
                        brk.Attributes.Append(drawOutline);
                        brk.Attributes.Append(drawFill);
                        brk.Attributes.Append(isNoData);

                        breaks.AppendChild(brk);
                    }
                    break;
                case ShapeTypes.Polyline:
                    foreach (PolyLineBreak aPLB in aLS.LegendBreaks)
                    {
                        brk = m_Doc.CreateElement("Break");
                        caption = m_Doc.CreateAttribute("Caption");
                        startValue = m_Doc.CreateAttribute("StartValue");
                        endValue = m_Doc.CreateAttribute("EndValue");
                        color = m_Doc.CreateAttribute("Color");
                        drawShape = m_Doc.CreateAttribute("DrawShape");
                        size = m_Doc.CreateAttribute("Size");
                        style = m_Doc.CreateAttribute("Style");

                        caption.InnerText = aPLB.Caption;
                        startValue.InnerText = aPLB.StartValue.ToString();
                        endValue.InnerText = aPLB.EndValue.ToString();
                        color.InnerText = ColorTranslator.ToHtml(aPLB.Color);
                        drawShape.InnerText = aPLB.DrawPolyline.ToString();
                        size.InnerText = aPLB.Size.ToString();
                        style.InnerText = Enum.GetName(typeof(DashStyle), aPLB.Style);

                        brk.Attributes.Append(caption);
                        brk.Attributes.Append(startValue);
                        brk.Attributes.Append(endValue);
                        brk.Attributes.Append(color);
                        brk.Attributes.Append(drawShape);
                        brk.Attributes.Append(size);
                        brk.Attributes.Append(style);

                        breaks.AppendChild(brk);
                    }
                    break;
                case ShapeTypes.Polygon:
                case ShapeTypes.PolygonM:
                    XmlAttribute outlineSize;
                    foreach (PolygonBreak aPGB in aLS.LegendBreaks)
                    {
                        brk = m_Doc.CreateElement("Break");
                        caption = m_Doc.CreateAttribute("Caption");
                        startValue = m_Doc.CreateAttribute("StartValue");
                        endValue = m_Doc.CreateAttribute("EndValue");
                        color = m_Doc.CreateAttribute("Color");
                        drawShape = m_Doc.CreateAttribute("DrawShape");
                        outlineColor = m_Doc.CreateAttribute("OutlineColor");
                        drawOutline = m_Doc.CreateAttribute("DrawOutline");
                        drawFill = m_Doc.CreateAttribute("DrawFill");
                        outlineSize = m_Doc.CreateAttribute("OutlineSize");

                        caption.InnerText = aPGB.Caption;
                        startValue.InnerText = aPGB.StartValue.ToString();
                        endValue.InnerText = aPGB.EndValue.ToString();
                        color.InnerText = ColorTranslator.ToHtml(aPGB.Color);
                        drawShape.InnerText = aPGB.DrawShape.ToString();
                        outlineColor.InnerText = ColorTranslator.ToHtml(aPGB.OutlineColor);
                        drawOutline.InnerText = aPGB.DrawOutline.ToString();
                        drawFill.InnerText = aPGB.DrawFill.ToString();
                        outlineSize.InnerText = aPGB.OutlineSize.ToString();

                        brk.Attributes.Append(caption);
                        brk.Attributes.Append(startValue);
                        brk.Attributes.Append(endValue);
                        brk.Attributes.Append(color);
                        brk.Attributes.Append(drawShape);
                        brk.Attributes.Append(outlineColor);
                        brk.Attributes.Append(drawOutline);
                        brk.Attributes.Append(drawFill);
                        brk.Attributes.Append(outlineSize);

                        breaks.AppendChild(brk);
                    }
                    break;
            }

            root.AppendChild(breaks);
            parent.AppendChild(root);

        }

        private void ExportLabelSet(ref XmlDocument m_Doc, XmlElement parent, LabelSet aLabelSet)
        {
            XmlElement LabelSet = m_Doc.CreateElement("LabelSet");
            XmlAttribute DrawLabels = m_Doc.CreateAttribute("DrawLabels");
            XmlAttribute FieldName = m_Doc.CreateAttribute("FieldName");
            XmlAttribute FontName = m_Doc.CreateAttribute("FontName");
            XmlAttribute FontSize = m_Doc.CreateAttribute("FontSize");
            XmlAttribute LabelColor = m_Doc.CreateAttribute("LabelColor");
            XmlAttribute DrawShadow = m_Doc.CreateAttribute("DrawShadow");
            XmlAttribute ShadowColor = m_Doc.CreateAttribute("ShadowColor");
            XmlAttribute AlignType = m_Doc.CreateAttribute("AlignType");
            XmlAttribute Offset = m_Doc.CreateAttribute("Offset");
            XmlAttribute AvoidCollision = m_Doc.CreateAttribute("AvoidCollision");
            XmlAttribute autoDecimal = m_Doc.CreateAttribute("AutoDecimal");
            XmlAttribute decimalDigits = m_Doc.CreateAttribute("DecimalDigits");

            DrawLabels.InnerText = aLabelSet.DrawLabels.ToString();
            FieldName.InnerText = aLabelSet.FieldName;
            FontName.InnerText = aLabelSet.LabelFont.Name;
            FontSize.InnerText = aLabelSet.LabelFont.Size.ToString();
            LabelColor.InnerText = ColorTranslator.ToHtml(aLabelSet.LabelColor);
            DrawShadow.InnerText = aLabelSet.DrawShadow.ToString();
            ShadowColor.InnerText = ColorTranslator.ToHtml(aLabelSet.ShadowColor);
            AlignType.InnerText = Enum.GetName(typeof(AlignType), aLabelSet.LabelAlignType);
            Offset.InnerText = aLabelSet.YOffset.ToString();
            AvoidCollision.InnerText = aLabelSet.AvoidCollision.ToString();
            autoDecimal.InnerText = aLabelSet.AutoDecimal.ToString();
            decimalDigits.InnerText = aLabelSet.DecimalDigits.ToString();

            LabelSet.Attributes.Append(DrawLabels);
            LabelSet.Attributes.Append(FieldName);
            LabelSet.Attributes.Append(FontName);
            LabelSet.Attributes.Append(FontSize);
            LabelSet.Attributes.Append(LabelColor);
            LabelSet.Attributes.Append(DrawShadow);
            LabelSet.Attributes.Append(ShadowColor);
            LabelSet.Attributes.Append(AlignType);
            LabelSet.Attributes.Append(Offset);
            LabelSet.Attributes.Append(AvoidCollision);
            LabelSet.Attributes.Append(autoDecimal);
            LabelSet.Attributes.Append(decimalDigits);

            parent.AppendChild(LabelSet);
        }

        private void ExportChartSet(ref XmlDocument m_Doc, XmlElement parent, ChartSet aChartSet)
        {
            XmlElement chartSet = m_Doc.CreateElement("ChartSet");
            XmlAttribute drawCharts = m_Doc.CreateAttribute("DrawCharts");
            XmlAttribute chartType = m_Doc.CreateAttribute("ChartType");
            XmlAttribute fieldNames = m_Doc.CreateAttribute("FieldNames");
            XmlAttribute xShift = m_Doc.CreateAttribute("XShift");
            XmlAttribute yShift = m_Doc.CreateAttribute("YShift");
            XmlAttribute maxSize = m_Doc.CreateAttribute("MaxSize");
            XmlAttribute minSize = m_Doc.CreateAttribute("MinSize");
            XmlAttribute maxValue = m_Doc.CreateAttribute("MaxValue");
            XmlAttribute minValue = m_Doc.CreateAttribute("MinValue");
            XmlAttribute barWidth = m_Doc.CreateAttribute("BarWidth");
            XmlAttribute avoidCollision = m_Doc.CreateAttribute("AvoidCollision");
            XmlAttribute alignType = m_Doc.CreateAttribute("AlignType");
            XmlAttribute view3D = m_Doc.CreateAttribute("View3D");
            XmlAttribute thickness = m_Doc.CreateAttribute("Thickness");

            drawCharts.InnerText = aChartSet.DrawCharts.ToString();
            chartType.InnerText = aChartSet.ChartType.ToString();
            string fns = "";
            for (int i = 0; i < aChartSet.FieldNames.Count; i++)
            {
                if (i == 0)
                    fns = aChartSet.FieldNames[i];
                else
                    fns = fns + "," + aChartSet.FieldNames[i];
            }
            fieldNames.InnerText = fns;
            xShift.InnerText = aChartSet.XShift.ToString();
            yShift.InnerText = aChartSet.YShift.ToString();
            maxSize.InnerText = aChartSet.MaxSize.ToString();
            minSize.InnerText = aChartSet.MinSize.ToString();
            maxValue.InnerText = aChartSet.MaxValue.ToString();
            minValue.InnerText = aChartSet.MinValue.ToString();
            barWidth.InnerText = aChartSet.BarWidth.ToString();
            avoidCollision.InnerText = aChartSet.AvoidCollision.ToString();
            alignType.InnerText = Enum.GetName(typeof(AlignType), aChartSet.AlignType);
            view3D.InnerText = aChartSet.View3D.ToString();
            thickness.InnerText = aChartSet.Thickness.ToString();

            chartSet.Attributes.Append(drawCharts);
            chartSet.Attributes.Append(chartType);
            chartSet.Attributes.Append(fieldNames);
            chartSet.Attributes.Append(xShift);
            chartSet.Attributes.Append(yShift);
            chartSet.Attributes.Append(maxSize);
            chartSet.Attributes.Append(minSize);
            chartSet.Attributes.Append(maxValue);
            chartSet.Attributes.Append(minValue);
            chartSet.Attributes.Append(barWidth);
            chartSet.Attributes.Append(avoidCollision);
            chartSet.Attributes.Append(alignType);
            chartSet.Attributes.Append(view3D);
            chartSet.Attributes.Append(thickness);

            //Export legend scheme
            aChartSet.LegendScheme.ExportToXML(ref m_Doc, chartSet);

            parent.AppendChild(chartSet);
        }

        private void ExportVisibleScale(ref XmlDocument m_Doc, XmlElement parent, VisibleScale visibleScale)
        {
            XmlElement visScaleElem = m_Doc.CreateElement("VisibleScale");
            XmlAttribute enableMinVisScale = m_Doc.CreateAttribute("EnableMinVisScale");
            XmlAttribute enableMaxVisScale = m_Doc.CreateAttribute("EnableMaxVisScale");
            XmlAttribute minVisScale = m_Doc.CreateAttribute("MinVisScale");
            XmlAttribute maxVisScale = m_Doc.CreateAttribute("MaxVisScale");            

            enableMinVisScale.InnerText = visibleScale.EnableMinVisScale.ToString();
            enableMaxVisScale.InnerText = visibleScale.EnableMaxVisScale.ToString();            
            minVisScale.InnerText = visibleScale.MinVisScale.ToString();
            maxVisScale.InnerText = visibleScale.MaxVisScale.ToString();            

            visScaleElem.Attributes.Append(enableMinVisScale);
            visScaleElem.Attributes.Append(enableMaxVisScale);
            visScaleElem.Attributes.Append(minVisScale);
            visScaleElem.Attributes.Append(maxVisScale);

            parent.AppendChild(visScaleElem);
        }

        /// <summary>
        /// Add image layer element
        /// </summary>
        /// <param name="m_Doc"></param>
        /// <param name="parent"></param>
        /// <param name="aILayer"></param>
        /// <param name="projectFilePath"></param>
        public void ExportImageLayer(ref XmlDocument m_Doc, XmlElement parent, ImageLayer aILayer, string projectFilePath)
        {
            Global.GlobalUtil CGlobal = new MeteoInfoC.Global.GlobalUtil();

            XmlElement Layer = m_Doc.CreateElement("Layer");
            XmlAttribute Handle = m_Doc.CreateAttribute("Handle");
            XmlAttribute LayerName = m_Doc.CreateAttribute("LayerName");
            XmlAttribute FileName = m_Doc.CreateAttribute("FileName");
            XmlAttribute Visible = m_Doc.CreateAttribute("Visible");
            XmlAttribute IsMaskout = m_Doc.CreateAttribute("IsMaskout");
            XmlAttribute LayerType = m_Doc.CreateAttribute("LayerType");
            XmlAttribute LayerDrawType = m_Doc.CreateAttribute("LayerDrawType");
            XmlAttribute transparencyPerc = m_Doc.CreateAttribute("TransparencyPerc");
            XmlAttribute transparencyColor = m_Doc.CreateAttribute("TransparencyColor");
            XmlAttribute setTransColor = m_Doc.CreateAttribute("SetTransColor");

            Handle.InnerText = aILayer.Handle.ToString();
            LayerName.InnerText = aILayer.LayerName;
            FileName.InnerText = CGlobal.GetRelativePath(aILayer.FileName, projectFilePath);
            Visible.InnerText = aILayer.Visible.ToString();
            IsMaskout.InnerText = aILayer.IsMaskout.ToString();
            LayerType.InnerText = Enum.GetName(typeof(LayerTypes), aILayer.LayerType);
            LayerDrawType.InnerText = Enum.GetName(typeof(LayerDrawType), aILayer.LayerDrawType);
            transparencyPerc.InnerText = aILayer.TransparencyPerc.ToString();
            transparencyColor.InnerText = ColorTranslator.ToHtml(aILayer.TransparencyColor);
            setTransColor.InnerText = aILayer.SetTransColor.ToString();

            Layer.Attributes.Append(Handle);
            Layer.Attributes.Append(LayerName);
            Layer.Attributes.Append(FileName);
            Layer.Attributes.Append(Visible);
            Layer.Attributes.Append(IsMaskout);
            Layer.Attributes.Append(LayerType);
            Layer.Attributes.Append(LayerDrawType);
            Layer.Attributes.Append(transparencyPerc);
            Layer.Attributes.Append(transparencyColor);
            Layer.Attributes.Append(setTransColor);

            //Add visible scale
            ExportVisibleScale(ref m_Doc, Layer, aILayer.VisibleScale);

            parent.AppendChild(Layer);
        }

        private void ExportChartGraphics(ref XmlDocument m_Doc, XmlElement parent, List<Graphic> graphicList)
        {
            XmlElement graphics = m_Doc.CreateElement("ChartGraphics");

            //Add graphics
            foreach (Graphic aGraphic in graphicList)
            {
                //AddGraphic(ref m_Doc, graphics, aGraphic);
                aGraphic.ExportToXML(ref m_Doc, graphics);
            }

            parent.AppendChild(graphics);
        }

        /// <summary>
        /// Add graphics
        /// </summary>
        /// <param name="m_Doc">xml document</param>
        /// <param name="parent">parent xml element</param>
        /// <param name="graphicList">graphic list</param>
        public void ExportGraphics(ref XmlDocument m_Doc, XmlElement parent, List<Graphic> graphicList)
        {
            XmlElement graphics = m_Doc.CreateElement("Graphics");

            //Add graphics
            foreach (Graphic aGraphic in graphicList)
            {
                //AddGraphic(ref m_Doc, graphics, aGraphic);
                aGraphic.ExportToXML(ref m_Doc, graphics);
            }

            parent.AppendChild(graphics);
        }

        //private void ExportGraphic(ref XmlDocument m_Doc, XmlElement parent, Graphic aGraphic)
        //{
        //    XmlElement graphic = m_Doc.CreateElement("Graphic");
        //    ExportShape(ref m_Doc, graphic, aGraphic.Shape);
        //    ExportLegend(ref m_Doc, graphic, aGraphic.Legend, aGraphic.Shape.ShapeType);

        //    parent.AppendChild(graphic);
        //}

        //private void ExportShape(ref XmlDocument m_Doc, XmlElement parent, Shape.Shape aShape)
        //{
        //    XmlElement shape = m_Doc.CreateElement("Shape");

        //    //Add general attribute
        //    XmlAttribute shapeType = m_Doc.CreateAttribute("ShapeType");
        //    XmlAttribute visible = m_Doc.CreateAttribute("Visible");
        //    XmlAttribute selected = m_Doc.CreateAttribute("Selected");

        //    shapeType.InnerText = Enum.GetName(typeof(ShapeTypes), aShape.ShapeType);
        //    visible.InnerText = aShape.Visible.ToString();
        //    selected.InnerText = aShape.Selected.ToString();

        //    shape.Attributes.Append(shapeType);
        //    shape.Attributes.Append(visible);
        //    shape.Attributes.Append(selected);

        //    //Add points
        //    XmlElement points = m_Doc.CreateElement("Points");
        //    List<PointD> pointList = aShape.GetPoints();
        //    foreach (PointD aPoint in pointList)
        //    {
        //        XmlElement point = m_Doc.CreateElement("Point");
        //        XmlAttribute x = m_Doc.CreateAttribute("X");
        //        XmlAttribute y = m_Doc.CreateAttribute("Y");
        //        x.InnerText = aPoint.X.ToString();
        //        y.InnerText = aPoint.Y.ToString();
        //        point.Attributes.Append(x);
        //        point.Attributes.Append(y);

        //        points.AppendChild(point);
        //    }

        //    shape.AppendChild(points);

        //    parent.AppendChild(shape);
        //}

        //private void ExportLegend(ref XmlDocument doc, XmlElement parent, ColorBreak aLegend, ShapeTypes shapeType)
        //{
        //    XmlElement legend = doc.CreateElement("Legend");            
        //    XmlAttribute color = doc.CreateAttribute("Color");            
        //    color.InnerText = ColorTranslator.ToHtml(aLegend.Color);
        //    legend.Attributes.Append(color);

        //    XmlAttribute legendType = doc.CreateAttribute("LegendType");
        //    XmlAttribute size;
        //    XmlAttribute style;
        //    XmlAttribute outlineColor;
        //    XmlAttribute drawOutline;
        //    XmlAttribute drawFill;
        //    switch (shapeType)
        //    {
        //        case ShapeTypes.Point:
        //            if (aLegend.GetType() == typeof(PointBreak))
        //            {
        //                PointBreak aPB = (PointBreak)aLegend;                        
        //                outlineColor = doc.CreateAttribute("OutlineColor");
        //                size = doc.CreateAttribute("Size");
        //                style = doc.CreateAttribute("Style");
        //                drawOutline = doc.CreateAttribute("DrawOutline");
        //                drawFill = doc.CreateAttribute("DrawFill");

        //                legendType.InnerText = "PointBreak";
        //                outlineColor.InnerText = ColorTranslator.ToHtml(aPB.OutlineColor);
        //                size.InnerText = aPB.Size.ToString();
        //                style.InnerText = Enum.GetName(typeof(PointStyle), aPB.Style);
        //                drawOutline.InnerText = aPB.DrawOutline.ToString();
        //                drawFill.InnerText = aPB.DrawFill.ToString();

        //                legend.Attributes.Append(legendType);
        //                legend.Attributes.Append(outlineColor);
        //                legend.Attributes.Append(size);
        //                legend.Attributes.Append(style);
        //                legend.Attributes.Append(drawOutline);
        //                legend.Attributes.Append(drawFill);
        //            }
        //            else if (aLegend.GetType() == typeof(LabelBreak))
        //            {
        //                LabelBreak aLB = (LabelBreak)aLegend;
        //                XmlAttribute text = doc.CreateAttribute("Text");
        //                XmlAttribute angle = doc.CreateAttribute("Angle");
        //                XmlAttribute fontName = doc.CreateAttribute("FontName");
        //                XmlAttribute fontSize = doc.CreateAttribute("FontSize");
        //                XmlAttribute fontBold = doc.CreateAttribute("FontBold");
        //                XmlAttribute yShift = doc.CreateAttribute("YShift");

        //                legendType.InnerText = "LabelBreak";
        //                text.InnerText = aLB.Text;
        //                angle.InnerText = aLB.Angle.ToString();
        //                fontName.InnerText = aLB.Font.Name;
        //                fontSize.InnerText = aLB.Font.Size.ToString();
        //                fontBold.InnerText = aLB.Font.Bold.ToString();
        //                yShift.InnerText = aLB.YShift.ToString();                        

        //                legend.Attributes.Append(legendType);
        //                legend.Attributes.Append(text);
        //                legend.Attributes.Append(angle);
        //                legend.Attributes.Append(fontName);
        //                legend.Attributes.Append(fontSize);
        //                legend.Attributes.Append(fontBold);
        //                legend.Attributes.Append(yShift);
        //            }
        //            break;
        //        case ShapeTypes.Polyline:
        //            PolyLineBreak aPLB = (PolyLineBreak)aLegend;
        //            size = doc.CreateAttribute("Size");
        //            style = doc.CreateAttribute("Style");
        //            XmlAttribute drawSymbol = doc.CreateAttribute("DrawSymbol");
        //            XmlAttribute symbolSize = doc.CreateAttribute("SymbolSize");
        //            XmlAttribute symbolStyle = doc.CreateAttribute("SymbolStyle");
        //            XmlAttribute symbolColor = doc.CreateAttribute("SymbolColor");
        //            XmlAttribute symbolInterval = doc.CreateAttribute("SymbolInterval");

        //            legendType.InnerText = "PolylineBreak";
        //            size.InnerText = aPLB.Size.ToString();
        //            style.InnerText = Enum.GetName(typeof(DashStyle), aPLB.Style);
        //            drawSymbol.InnerText = aPLB.DrawSymbol.ToString();
        //            symbolSize.InnerText = aPLB.SymbolSize.ToString();
        //            symbolStyle.InnerText = aPLB.SymbolStyle.ToString();
        //            symbolColor.InnerText = ColorTranslator.ToHtml(aPLB.SymbolColor);
        //            symbolInterval.InnerText = aPLB.SymbolInterval.ToString();

        //            legend.Attributes.Append(legendType);
        //            legend.Attributes.Append(size);
        //            legend.Attributes.Append(style);
        //            legend.Attributes.Append(drawSymbol);
        //            legend.Attributes.Append(symbolSize);
        //            legend.Attributes.Append(symbolStyle);
        //            legend.Attributes.Append(symbolColor);
        //            legend.Attributes.Append(symbolInterval);
        //            break;
        //        case ShapeTypes.Polygon:
        //        case ShapeTypes.Rectangle:
        //            PolygonBreak aPGB = (PolygonBreak)aLegend;
        //            outlineColor = doc.CreateAttribute("OutlineColor");
        //            drawOutline = doc.CreateAttribute("DrawOutline");
        //            drawFill = doc.CreateAttribute("DrawFill");
        //            XmlAttribute outlineSize = doc.CreateAttribute("OutlineSize");
        //            XmlAttribute usingHatchStyle = doc.CreateAttribute("UsingHatchStyle");
        //            style = doc.CreateAttribute("Style");
        //            XmlAttribute backColor = doc.CreateAttribute("BackColor");
        //            XmlAttribute transparencyPer = doc.CreateAttribute("TransparencyPercent");

        //            legendType.InnerText = "PolygonBreak";
        //            outlineColor.InnerText = ColorTranslator.ToHtml(aPGB.OutlineColor);
        //            drawOutline.InnerText = aPGB.DrawOutline.ToString();
        //            drawFill.InnerText = aPGB.DrawFill.ToString();
        //            outlineSize.InnerText = aPGB.OutlineSize.ToString();
        //            usingHatchStyle.InnerText = aPGB.UsingHatchStyle.ToString();
        //            style.InnerText = aPGB.Style.ToString();
        //            backColor.InnerText = ColorTranslator.ToHtml(aPGB.BackColor);
        //            transparencyPer.InnerText = aPGB.TransparencyPercent.ToString();

        //            legend.Attributes.Append(legendType);
        //            legend.Attributes.Append(outlineColor);
        //            legend.Attributes.Append(drawOutline);
        //            legend.Attributes.Append(drawFill);
        //            legend.Attributes.Append(outlineSize);
        //            legend.Attributes.Append(usingHatchStyle);
        //            legend.Attributes.Append(style);
        //            legend.Attributes.Append(backColor);
        //            legend.Attributes.Append(transparencyPer);
        //            break;
        //    }

        //    parent.AppendChild(legend);
        //}

        #endregion

        #region Import

        /// <summary>
        /// Import project XML content
        /// </summary>
        /// <param name="parent">parent XML element</param>
        public void ImportProjectXML(XmlElement parent)
        {            
            LoadMapPropElement(parent);
            LoadGridLineElement(parent);
            LoadMaskOutElement(parent);
            LoadProjectionElement(parent);
            LoadLayers(parent);
            LoadGraphics(parent);
            LoadExtentsElement(parent);
        }

        /// <summary>
        /// Load extent element
        /// </summary>
        /// <param name="parent">parent xml element</param>
        public void LoadExtentsElement(XmlElement parent)
        {
            XmlNode Extents = parent.GetElementsByTagName("Extents")[0];
            MeteoInfoC.Global.Extent aExtent = new MeteoInfoC.Global.Extent();
            aExtent.minX = double.Parse(Extents.Attributes["xMin"].InnerText);
            aExtent.maxX = double.Parse(Extents.Attributes["xMax"].InnerText);
            aExtent.minY = double.Parse(Extents.Attributes["yMin"].InnerText);
            aExtent.maxY = double.Parse(Extents.Attributes["yMax"].InnerText);
            
            ViewExtent = aExtent;
        }

        /// <summary>
        /// Load map property element
        /// </summary>
        /// <param name="parent">parent xml element</param>
        public void LoadMapPropElement(XmlElement parent)
        {
            XmlNode MapProperty = parent.GetElementsByTagName("MapProperty")[0];
            try
            {
                this.BackColor = ColorTranslator.FromHtml(MapProperty.Attributes["BackColor"].InnerText);
                this.ForeColor = ColorTranslator.FromHtml(MapProperty.Attributes["ForeColor"].InnerText);
                _smoothingMode = (SmoothingMode)Enum.Parse(typeof(SmoothingMode),
                    MapProperty.Attributes["SmoothingMode"].InnerText, true);
                _pointSmoothingMode = (SmoothingMode)Enum.Parse(typeof(SmoothingMode),
                    MapProperty.Attributes["PointSmoothingMode"].InnerText, true);
                _XYScaleFactor = float.Parse(MapProperty.Attributes["XYScaleFactor"].InnerText);
                _multiGlobalDraw = bool.Parse(MapProperty.Attributes["MultiGlobalDraw"].InnerText);
                _selectColor = ColorTranslator.FromHtml(MapProperty.Attributes["SelectColor"].InnerText);
                _highSpeedWheelZoom = bool.Parse(MapProperty.Attributes["HighSpeedWheelZoom"].InnerText);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Load grid line element
        /// </summary>
        /// <param name="parent">parent xml element</param>
        public void LoadGridLineElement(XmlElement parent)
        {
            XmlNode GridLine = parent.GetElementsByTagName("GridLine")[0];
            try
            {
                _gridLineColor = ColorTranslator.FromHtml(GridLine.Attributes["GridLineColor"].InnerText);
                _gridLineSize = int.Parse(GridLine.Attributes["GridLineSize"].InnerText);
                _gridLineStyle = (DashStyle)Enum.Parse(typeof(DashStyle),
                    GridLine.Attributes["GridLineStyle"].InnerText, true);
                _drawGridLine = bool.Parse(GridLine.Attributes["DrawGridLine"].InnerText);
                _drawGridTickLine = bool.Parse(GridLine.Attributes["DrawGridTickLine"].InnerText);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Load mask out element
        /// </summary>
        /// <param name="parent">parent xml element</param>
        public void LoadMaskOutElement(XmlElement parent)
        {
            XmlNode MaskOut = parent.GetElementsByTagName("MaskOut")[0];
            try
            {
                _maskOut.SetMaskLayer = bool.Parse(MaskOut.Attributes["SetMaskLayer"].InnerText);
                _maskOut.MaskLayer = MaskOut.Attributes["MaskLayer"].InnerText;
            }
            catch
            {

            }
        }

        /// <summary>
        /// Load projection element
        /// </summary>
        /// <param name="parent">parent xml element</param>
        public void LoadProjectionElement(XmlElement parent)
        {
            XmlNode Projection = parent.GetElementsByTagName("Projection")[0];
            try
            {
                //_projection.IsLonLatMap = bool.Parse(Projection.Attributes["IsLonLatMap"].InnerText);                  
                _projection.ProjStr = Projection.Attributes["ProjStr"].InnerText;
                //if (!_projection.IsLonLatMap)
                //    _projection.ProjInfo = new ProjectionInfo(_projection.ProjStr);
                _projection.ProjInfo = new ProjectionInfo(_projection.ProjStr);
                _projection.RefLon = double.Parse(Projection.Attributes["RefLon"].InnerText);
                _projection.RefCutLon = double.Parse(Projection.Attributes["RefCutLon"].InnerText);
                if (!(_projection.ProjInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat))
                {
                    ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
                    ProjectionInfo toProj = new ProjectionInfo();
                    toProj = new ProjectionInfo(_projection.ProjStr);
                    ProjectLayers(toProj);
                    //_projection.ProjInfo = toProj;
                    //_projection.ProjectLayers_Proj4(this, fromProj, toProj);
                    //_lonLatLayer = _projection.GenerateLonLatLayer();
                }
            }
            catch
            {

            }
        }

        private void LoadLayers(XmlElement parent)
        {
            XmlNode Layers = parent.GetElementsByTagName("Layers")[0];
            foreach (XmlNode aLayer in Layers.ChildNodes)
            {
                LayerDrawType aLayerType;
                if (aLayer.Attributes["LayerDrawType"] == null)
                    aLayerType = (LayerDrawType)Enum.Parse(typeof(LayerDrawType),
                        aLayer.Attributes["LayerType"].InnerText, true);
                else
                    aLayerType = (LayerDrawType)Enum.Parse(typeof(LayerDrawType),
                        aLayer.Attributes["LayerDrawType"].InnerText, true);

                if (aLayerType == LayerDrawType.Image)
                {
                    ImageLayer aILayer = LoadImageLayer(aLayer);

                    AddLayer(aILayer);
                }
                else
                {
                    VectorLayer aVLayer = LoadVectorLayer(aLayer);
                    
                    AddLayer(aVLayer);
                }
            }
        }

        /// <summary>
        /// Load vector layer
        /// </summary>
        /// <param name="aVLayer">vector layer xml node</param>
        /// <returns>vector layer</returns>
        public VectorLayer LoadVectorLayer(XmlNode aVLayer)
        {
            string aFile = aVLayer.Attributes["FileName"].InnerText;
            aFile = Path.GetFullPath(aFile);            
            VectorLayer aLayer = null;

            if (File.Exists(aFile))
            {
                switch (Path.GetExtension(aFile).ToLower())
                {
                    case ".dat":
                        aLayer = MapDataManage.ReadMapFile_MICAPS(aFile);
                        break;
                    case ".shp":
                        aLayer = MapDataManage.ReadMapFile_ShapeFile(aFile);
                        break;
                    case ".wmp":
                        aLayer = MapDataManage.ReadMapFile_WMP(aFile);
                        break;
                    default:
                        aLayer = MapDataManage.ReadMapFile_GrADS(aFile);
                        break;
                }

                try
                {
                    aLayer.Handle = int.Parse(aVLayer.Attributes["Handle"].InnerText);
                    aLayer.LayerName = aVLayer.Attributes["LayerName"].InnerText;
                    aLayer.Visible = bool.Parse(aVLayer.Attributes["Visible"].InnerText);
                    aLayer.IsMaskout = bool.Parse(aVLayer.Attributes["IsMaskout"].InnerText);
                    aLayer.TransparencyPerc = int.Parse(aVLayer.Attributes["TransparencyPerc"].InnerText);
                    aLayer.AvoidCollision = bool.Parse(aVLayer.Attributes["AvoidCollision"].InnerText);
                    aLayer.Expanded = bool.Parse(aVLayer.Attributes["Expanded"].InnerText);
                    aLayer.LayerType = (LayerTypes)Enum.Parse(typeof(LayerTypes), 
                        aVLayer.Attributes["LayerType"].InnerText, true);
                    aLayer.LayerDrawType = (LayerDrawType)Enum.Parse(typeof(LayerDrawType),
                        aVLayer.Attributes["LayerDrawType"].InnerText, true);
                }
                catch
                {

                }

                //Load legend scheme
                XmlNode LS = aVLayer.ChildNodes[0];
                //LegendScheme aLS = new LegendScheme();
                aLayer.LegendScheme.ImportFromXML(LS);
                aLayer.UpdateLegendIndexes();
                //if (LoadLegendScheme(LS, ref aLS, aLayer.ShapeType))
                //{
                //    aLayer.LegendScheme = aLS;
                //}

                //Load label set
                XmlNode labelNode = aVLayer.ChildNodes[1];
                LabelSet aLabelSet = new LabelSet();
                LoadLabelSet(labelNode, ref aLabelSet);
                aLayer.LabelSet = aLabelSet;                               

                //Load label graphics
                GraphicCollection gc = LoadGraphicCollection((XmlElement)aVLayer);
                aLayer.LabelPoints = gc.GraphicList;

                //Load chart set 
                XmlNode chartNode = aVLayer.SelectSingleNode("ChartSet");
                if (chartNode != null)
                {
                    ChartSet aChartSet = new ChartSet();
                    LoadChartSet(chartNode, ref aChartSet);
                    aLayer.ChartSet = aChartSet;

                    //Load chart graphics
                    gc = LoadChartGraphicCollection((XmlElement)aVLayer);
                    aLayer.ChartPoints = gc.GraphicList;
                    aLayer.UpdateChartsProp();
                }    
            
                //Load visible scale
                XmlNode visScaleNode = aVLayer.SelectSingleNode("VisibleScale");
                if (visScaleNode != null)
                {
                    VisibleScale visScale = aLayer.VisibleScale;
                    LoadVisibleScale(visScaleNode, ref visScale);
                }
            }

            return aLayer;
        }

        //private bool LoadLegendScheme(XmlNode LSNode, ref LegendScheme aLS, ShapeTypes aST)
        //{
        //    aLS.LegendBreaks = new List<ColorBreak>();

        //    if (!(LSNode.Attributes["FieldName"] == null))
        //    {
        //        aLS.FieldName = LSNode.Attributes["FieldName"].InnerText;
        //    }
        //    aLS.LegendType = (LegendType)Enum.Parse(typeof(LegendType),
        //        LSNode.Attributes["LegendType"].InnerText, true);
        //    ShapeTypes aShapeType = (ShapeTypes)Enum.Parse(typeof(ShapeTypes),
        //        LSNode.Attributes["ShapeType"].InnerText, true);
        //    if (aShapeType != aST)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        aLS.ShapeType = aST;
        //        //aLS.breakNum = Convert.ToInt32(LSNode.Attributes["BreakNum"].InnerText);
        //        aLS.HasNoData = Convert.ToBoolean(LSNode.Attributes["HasNoData"].InnerText);
        //        aLS.MinValue = Convert.ToDouble(LSNode.Attributes["MinValue"].InnerText);
        //        aLS.MaxValue = Convert.ToDouble(LSNode.Attributes["MaxValue"].InnerText);
        //        aLS.MissingValue = Convert.ToDouble(LSNode.Attributes["MissingValue"].InnerText);

        //        XmlNode breaks = LSNode.ChildNodes[0];
        //        switch (aLS.ShapeType)
        //        {
        //            case ShapeTypes.Point:
        //                PointBreak aPB = new PointBreak();
        //                foreach (XmlNode brk in breaks.ChildNodes)
        //                {
        //                    aPB.Caption = brk.Attributes["Caption"].InnerText;
        //                    aPB.StartValue = Convert.ToDouble(brk.Attributes["StartValue"].InnerText);
        //                    aPB.EndValue = Convert.ToDouble(brk.Attributes["EndValue"].InnerText);
        //                    aPB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                    aPB.DrawShape = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
        //                    aPB.DrawFill = Convert.ToBoolean(brk.Attributes["DrawFill"].InnerText);
        //                    aPB.DrawOutline = Convert.ToBoolean(brk.Attributes["DrawOutline"].InnerText);
        //                    aPB.IsNoData = Convert.ToBoolean(brk.Attributes["IsNoData"].InnerText);
        //                    aPB.OutlineColor = ColorTranslator.FromHtml(brk.Attributes["OutlineColor"].InnerText);
        //                    aPB.Size = Convert.ToSingle(brk.Attributes["Size"].InnerText);
        //                    aPB.Style = (PointStyle)Enum.Parse(typeof(PointStyle),
        //                        brk.Attributes["Style"].InnerText, true);
        //                    aLS.LegendBreaks.Add(aPB);
        //                }
        //                break;
        //            case ShapeTypes.Polyline:
        //                PolyLineBreak aPLB = new PolyLineBreak();
        //                foreach (XmlNode brk in breaks.ChildNodes)
        //                {
        //                    aPLB.Caption = brk.Attributes["Caption"].InnerText;
        //                    aPLB.StartValue = Convert.ToDouble(brk.Attributes["StartValue"].InnerText);
        //                    aPLB.EndValue = Convert.ToDouble(brk.Attributes["EndValue"].InnerText);
        //                    aPLB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                    aPLB.DrawPolyline = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
        //                    aPLB.Size = Convert.ToSingle(brk.Attributes["Size"].InnerText);
        //                    aPLB.Style = (DashStyle)Enum.Parse(typeof(DashStyle),
        //                        brk.Attributes["Style"].InnerText, true);
        //                    aLS.LegendBreaks.Add(aPLB);
        //                }
        //                break;
        //            case ShapeTypes.Polygon:
        //                PolygonBreak aPGB = new PolygonBreak();
        //                foreach (XmlNode brk in breaks.ChildNodes)
        //                {
        //                    aPGB.Caption = brk.Attributes["Caption"].InnerText;
        //                    aPGB.StartValue = Convert.ToDouble(brk.Attributes["StartValue"].InnerText);
        //                    aPGB.EndValue = Convert.ToDouble(brk.Attributes["EndValue"].InnerText);
        //                    aPGB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                    aPGB.DrawShape = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
        //                    aPGB.DrawFill = Convert.ToBoolean(brk.Attributes["DrawFill"].InnerText);
        //                    aPGB.DrawOutline = Convert.ToBoolean(brk.Attributes["DrawOutline"].InnerText);
        //                    aPGB.OutlineSize = Convert.ToSingle(brk.Attributes["OutlineSize"].InnerText);
        //                    aPGB.OutlineColor = ColorTranslator.FromHtml(brk.Attributes["OutlineColor"].InnerText);
        //                    aLS.LegendBreaks.Add(aPGB);
        //                }
        //                break;
        //        }

        //        return true;
        //    }
        //}

        private void LoadLabelSet(XmlNode LabelNode, ref LabelSet aLabelSet)
        {
            try
            {
                aLabelSet.DrawLabels = bool.Parse(LabelNode.Attributes["DrawLabels"].InnerText);
                aLabelSet.FieldName = LabelNode.Attributes["FieldName"].InnerText;
                string fontName = LabelNode.Attributes["FontName"].InnerText;
                Single fontSize = Single.Parse(LabelNode.Attributes["FontSize"].InnerText);
                aLabelSet.LabelFont = new Font(fontName, fontSize);
                aLabelSet.LabelColor = ColorTranslator.FromHtml(LabelNode.Attributes["LabelColor"].InnerText);
                aLabelSet.DrawShadow = bool.Parse(LabelNode.Attributes["DrawShadow"].InnerText);
                aLabelSet.ShadowColor = ColorTranslator.FromHtml(LabelNode.Attributes["ShadowColor"].InnerText);
                aLabelSet.LabelAlignType = (AlignType)Enum.Parse(typeof(AlignType),
                    LabelNode.Attributes["AlignType"].InnerText, true);
                aLabelSet.YOffset = int.Parse(LabelNode.Attributes["Offset"].InnerText);
                aLabelSet.AvoidCollision = bool.Parse(LabelNode.Attributes["AvoidCollision"].InnerText);
                aLabelSet.AutoDecimal = bool.Parse(LabelNode.Attributes["AutoDecimal"].InnerText);
                aLabelSet.DecimalDigits = int.Parse(LabelNode.Attributes["DecimalDigits"].InnerText);
            }
            catch
            {

            }
        }

        private void LoadChartSet(XmlNode chartNode, ref ChartSet aChartSet)
        {
            try
            {
                aChartSet.DrawCharts = bool.Parse(chartNode.Attributes["DrawCharts"].InnerText);
                aChartSet.ChartType = (ChartTypes)Enum.Parse(typeof(ChartTypes), chartNode.Attributes["ChartType"].InnerText);
                aChartSet.FieldNames = new List<string>(chartNode.Attributes["FieldNames"].InnerText.Split(','));
                aChartSet.XShift = int.Parse(chartNode.Attributes["XShift"].InnerText);
                aChartSet.YShift = int.Parse(chartNode.Attributes["YShift"].InnerText);
                aChartSet.MaxSize = int.Parse(chartNode.Attributes["MaxSize"].InnerText);
                aChartSet.MinSize = int.Parse(chartNode.Attributes["MinSize"].InnerText);
                aChartSet.MaxValue = float.Parse(chartNode.Attributes["MaxValue"].InnerText);
                aChartSet.MinValue = float.Parse(chartNode.Attributes["MinValue"].InnerText);
                aChartSet.BarWidth = int.Parse(chartNode.Attributes["BarWidth"].InnerText);
                aChartSet.AvoidCollision = bool.Parse(chartNode.Attributes["AvoidCollision"].InnerText);
                aChartSet.AlignType = (AlignType)Enum.Parse(typeof(AlignType), chartNode.Attributes["AlignType"].InnerText);
                aChartSet.View3D = bool.Parse(chartNode.Attributes["View3D"].InnerText);
                aChartSet.Thickness = int.Parse(chartNode.Attributes["Thickness"].InnerText);
            }
            catch
            {

            }

            //Load legend scheme
            XmlNode lsNode = chartNode.ChildNodes[0];
            aChartSet.LegendScheme.ImportFromXML(lsNode);
        }

        private void LoadVisibleScale(XmlNode visScaleNode, ref VisibleScale visScale)
        {
            try
            {
                visScale.EnableMinVisScale = bool.Parse(visScaleNode.Attributes["EnableMinVisScale"].InnerText);
                visScale.EnableMaxVisScale = bool.Parse(visScaleNode.Attributes["EnableMaxVisScale"].InnerText);
                visScale.MinVisScale = double.Parse(visScaleNode.Attributes["MinVisScale"].InnerText);
                visScale.MaxVisScale = double.Parse(visScaleNode.Attributes["MaxVisScale"].InnerText);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Load image layer
        /// </summary>
        /// <param name="aILayer">image layer xml node</param>
        /// <returns>image layer</returns>
        public ImageLayer LoadImageLayer(XmlNode aILayer)
        {
            string aFile = aILayer.Attributes["FileName"].InnerText;
            aFile = Path.GetFullPath(aFile);
            ImageLayer aLayer = null;

            if (File.Exists(aFile))
            {
                aLayer = MapDataManage.ReadImageFile(aFile);
                try
                {
                    aLayer.Handle = int.Parse(aILayer.Attributes["Handle"].InnerText);
                    aLayer.LayerName = aILayer.Attributes["LayerName"].InnerText;
                    aLayer.Visible = bool.Parse(aILayer.Attributes["Visible"].InnerText);
                    aLayer.IsMaskout = bool.Parse(aILayer.Attributes["IsMaskout"].InnerText);
                    aLayer.LayerType = (LayerTypes)Enum.Parse(typeof(LayerTypes),
                        aILayer.Attributes["LayerType"].InnerText, true);
                    aLayer.LayerDrawType = (LayerDrawType)Enum.Parse(typeof(LayerDrawType),
                        aILayer.Attributes["LayerDrawType"].InnerText, true);
                    aLayer.TransparencyPerc = int.Parse(aILayer.Attributes["TransparencyPerc"].InnerText);
                    aLayer.TransparencyColor = ColorTranslator.FromHtml(aILayer.Attributes["TransparencyColor"].InnerText);
                    aLayer.SetTransColor = bool.Parse(aILayer.Attributes["SetTransColor"].InnerText);

                    //Load visible scale
                    XmlNode visScaleNode = aILayer.SelectSingleNode("VisibleScale");
                    if (visScaleNode != null)
                    {
                        VisibleScale visScale = aLayer.VisibleScale;
                        LoadVisibleScale(visScaleNode, ref visScale);
                    }
                }
                catch
                {

                }                
            }

            return aLayer;
        }

        /// <summary>
        /// Load graphics
        /// </summary>
        /// <param name="parent"></param>
        public void LoadGraphics(XmlElement parent)
        {
            _graphicCollection = LoadGraphicCollection(parent);
        }

        private GraphicCollection LoadChartGraphicCollection(XmlElement parent)
        {
            GraphicCollection gc = new GraphicCollection();
            XmlNode graphics = null;
            foreach (XmlNode aNode in parent.ChildNodes)
            {
                if (aNode.Name == "ChartGraphics")
                {
                    graphics = aNode;
                    break;
                }
            }
            if (graphics != null)
            {
                foreach (XmlNode graphicNode in graphics.ChildNodes)
                {
                    //Graphic aGraphic = LoadGraphic(graphic);
                    Graphic aGraphic = new Graphic();
                    aGraphic.ImportFromXML(graphicNode);
                    gc.Add(aGraphic);
                }
            }

            return gc;
        }

        /// <summary>
        /// Load graphic collection
        /// </summary>
        /// <param name="parent">graphics node</param>
        private GraphicCollection LoadGraphicCollection(XmlElement parent)
        {
            GraphicCollection gc = new GraphicCollection();
            XmlNode graphics = null;
            foreach (XmlNode aNode in parent.ChildNodes)
            {
                if (aNode.Name == "Graphics")
                {
                    graphics = aNode;
                    break;
                }
            }
            if (graphics != null)
            {                                
                foreach (XmlNode graphicNode in graphics.ChildNodes)
                {
                    //Graphic aGraphic = LoadGraphic(graphic);
                    Graphic aGraphic = new Graphic();
                    aGraphic.ImportFromXML(graphicNode);
                    gc.Add(aGraphic);
                }
            }

            return gc;
        }

        //private Graphic LoadGraphic(XmlNode graphicNode)
        //{
        //    Graphic aGraphic = new Graphic();

        //    XmlNode shape = graphicNode.ChildNodes[0];
        //    aGraphic.Shape = LoadShape(shape);

        //    XmlNode legend = graphicNode.ChildNodes[1];
        //    aGraphic.Legend = LoadLegend(legend, aGraphic.Shape.ShapeType);

        //    return aGraphic;
        //}

        //private Shape.Shape LoadShape(XmlNode shapeNode)
        //{
        //    Shape.Shape aShape = new MeteoInfoC.Shape.Shape();
        //    try
        //    {
        //        ShapeTypes shapeType = (ShapeTypes)Enum.Parse(typeof(ShapeTypes), shapeNode.Attributes["ShapeType"].InnerText, true);
        //        switch (shapeType)
        //        {
        //            case ShapeTypes.Point:
        //                aShape = new PointShape();
        //                break;
        //            case ShapeTypes.Polyline:
        //                aShape = new PolylineShape();
        //                break;
        //            case ShapeTypes.Polygon:
        //            case ShapeTypes.Rectangle:
        //                aShape = new PolygonShape();
        //                break;
        //        }

        //        aShape.Visible = bool.Parse(shapeNode.Attributes["Visible"].InnerText);
        //        aShape.Selected = bool.Parse(shapeNode.Attributes["Selected"].InnerText);

        //        List<PointD> pointList = new List<PointD>();
        //        XmlNode pointsNode = shapeNode.ChildNodes[0];
        //        foreach (XmlNode pNode in pointsNode.ChildNodes)
        //        {
        //            PointD aPoint = new PointD(double.Parse(pNode.Attributes["X"].InnerText), 
        //                double.Parse(pNode.Attributes["Y"].InnerText));
        //            pointList.Add(aPoint);
        //        }
        //        aShape.SetPoints(pointList);
        //    }
        //    catch
        //    {

        //    }

        //    return aShape;
        //}

        //private ColorBreak LoadLegend(XmlNode legendNode, ShapeTypes shapeType)
        //{            
        //    try
        //    {
        //        Color color = ColorTranslator.FromHtml(legendNode.Attributes["Color"].InnerText);
        //        string legendType = legendNode.Attributes["LegendType"].InnerText;
        //        switch (shapeType)
        //        {
        //            case ShapeTypes.Point:
        //                if (legendType == "PointBreak")
        //                {
        //                    PointBreak aPB = new PointBreak();
        //                    aPB.Color = color;
        //                    aPB.DrawFill = bool.Parse(legendNode.Attributes["DrawFill"].InnerText);
        //                    aPB.DrawOutline = bool.Parse(legendNode.Attributes["DrawOutline"].InnerText);
        //                    aPB.OutlineColor = ColorTranslator.FromHtml(legendNode.Attributes["OutlineColor"].InnerText);
        //                    aPB.Size = float.Parse(legendNode.Attributes["Size"].InnerText);
        //                    aPB.Style = (PointStyle)Enum.Parse(typeof(PointStyle), legendNode.Attributes["Style"].InnerText);

        //                    return aPB;
        //                }
        //                else if (legendType == "LabelBreak")
        //                {
        //                    LabelBreak aLB = new LabelBreak();
        //                    aLB.Color = color;
        //                    aLB.Angle = float.Parse(legendNode.Attributes["Angle"].InnerText);
        //                    aLB.Text = legendNode.Attributes["Text"].InnerText;
        //                    string fontName = legendNode.Attributes["FontName"].InnerText;
        //                    float fontSize = float.Parse(legendNode.Attributes["FontSize"].InnerText);
        //                    bool fontBold = bool.Parse(legendNode.Attributes["FontBold"].InnerText);
        //                    if (fontBold)
        //                        aLB.Font = new Font(fontName, fontSize, FontStyle.Bold);
        //                    else
        //                        aLB.Font = new Font(fontName, fontSize);
        //                    aLB.YShift = float.Parse(legendNode.Attributes["YShift"].InnerText);

        //                    return aLB;
        //                }
        //                else
        //                    return null;
                        
        //            case ShapeTypes.Polyline:
        //                PolyLineBreak aPLB = new PolyLineBreak();
        //                aPLB.Color = color;
        //                aPLB.Size = Convert.ToSingle(legendNode.Attributes["Size"].InnerText);
        //                aPLB.Style = (DashStyle)Enum.Parse(typeof(DashStyle),
        //                    legendNode.Attributes["Style"].InnerText, true);
        //                aPLB.DrawSymbol = bool.Parse(legendNode.Attributes["DrawSymbol"].InnerText);
        //                aPLB.SymbolSize = Single.Parse(legendNode.Attributes["SymbolSize"].InnerText);
        //                aPLB.SymbolStyle = (PointStyle)Enum.Parse(typeof(PointStyle),
        //                    legendNode.Attributes["SymbolStyle"].InnerText, true);
        //                aPLB.SymbolColor = ColorTranslator.FromHtml(legendNode.Attributes["SymbolColor"].InnerText);
        //                aPLB.SymbolInterval = int.Parse(legendNode.Attributes["SymbolInterval"].InnerText);

        //                return aPLB;
                        
        //            case ShapeTypes.Polygon:
        //            case ShapeTypes.Rectangle:
        //                PolygonBreak aPGB = new PolygonBreak();
        //                aPGB.Color = color;
        //                aPGB.DrawFill = Convert.ToBoolean(legendNode.Attributes["DrawFill"].InnerText);
        //                aPGB.DrawOutline = Convert.ToBoolean(legendNode.Attributes["DrawOutline"].InnerText);
        //                aPGB.OutlineSize = Convert.ToSingle(legendNode.Attributes["OutlineSize"].InnerText);
        //                aPGB.OutlineColor = ColorTranslator.FromHtml(legendNode.Attributes["OutlineColor"].InnerText);
        //                aPGB.UsingHatchStyle = bool.Parse(legendNode.Attributes["UsingHatchStyle"].InnerText);
        //                aPGB.Style = (HatchStyle)Enum.Parse(typeof(HatchStyle), legendNode.Attributes["Style"].InnerText, true);
        //                aPGB.BackColor = ColorTranslator.FromHtml(legendNode.Attributes["BackColor"].InnerText);
        //                aPGB.TransparencyPercent = int.Parse(legendNode.Attributes["TransparencyPercent"].InnerText);

        //                return aPGB;
                        
        //            default:
        //                return null;
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        #endregion

        #endregion

        #endregion Methods

        #region Events

        /// <summary>
        /// MapView paint event
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Graphics g = pe.Graphics;
            //g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImageUnscaled(_mapBitmap, _xShift, _yShift);

            ////Draws the selection rectangle around each selected item
            //Pen selectionPen = new Pen(Color.Red, 1F);
            //selectionPen.DashPattern = new[] { 2.0F, 1.0F };
            //selectionPen.DashCap = DashCap.Round;
            //if (m_IsSelectedInLayout)
            //{
            //    Rectangle aRect = this.Bounds;
            //    pe.Graphics.DrawRectangle(selectionPen, 0, 0, this.Width - 1, this.Height - 1);
            //}

        }

        /// <summary>
        /// Override OnMouseClick evnet
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Left)
            {
                switch (_mouseTool)
                {
                    case MouseTools.Identifer:
                        if (SelectedLayer < 0)
                            return;
                        MapLayer aMLayer = GetLayerFromHandle(SelectedLayer);
                        if (aMLayer == null)
                            return;
                        if (aMLayer.LayerType == LayerTypes.ImageLayer)
                            return;

                        PointF aPoint = new PointF(e.X, e.Y);
                        if (aMLayer.LayerType == LayerTypes.VectorLayer)
                        {
                            VectorLayer aLayer = (VectorLayer)aMLayer;
                            List<int> SelectedShapes = new List<int>();
                            if (SelectShapes(aLayer, aPoint, ref SelectedShapes))
                            {
                                if (!_frmIdentifer.Visible)
                                {
                                    _frmIdentifer = new frmIdentifer(this);
                                    _frmIdentifer.Show(this);
                                }

                                _frmIdentifer.ListView1.Items.Clear();
                                string fieldStr, valueStr;
                                int shapeIdx = SelectedShapes[0];
                                aLayer.IdentiferShape = shapeIdx;
                                _drawIdentiferShape = true;

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
                                DrawIdShape(this.CreateGraphics(), aLayer.ShapeList[shapeIdx]);
                                //this.PaintLayers();
                            }
                        }
                        else if (aMLayer.LayerType == LayerTypes.RasterLayer)
                        {
                            RasterLayer aRLayer = (RasterLayer)aMLayer;
                            int iIdx = 0;
                            int jIdx = 0;
                            if (SelectGridCell(aRLayer, aPoint, ref iIdx, ref jIdx))
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
                    case MouseTools.SelectFeatures_Rectangle:
                        if (SelectedLayer < 0)
                            return;
                        aMLayer = GetLayerFromHandle(SelectedLayer);
                        if (aMLayer == null)
                            return;
                        if (aMLayer.LayerType != LayerTypes.VectorLayer)
                            return;

                        if (!((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                        {
                            ((VectorLayer)aMLayer).ClearSelectedShapes();
                            this.PaintLayers();
                        }

                        aPoint = new PointF(e.X, e.Y);
                        if (aMLayer.LayerType == LayerTypes.VectorLayer)
                        {
                            VectorLayer aLayer = (VectorLayer)aMLayer;
                            List<int> SelectedShapes = new List<int>();
                            if (SelectShapes(aLayer, aPoint, ref SelectedShapes, false))
                            {
                                int shapeIdx = SelectedShapes[0];
                                if (!((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                                {
                                    aLayer.ShapeList[shapeIdx].Selected = true;
                                    //this.Refresh();
                                    //DrawIdShape(this.CreateGraphics(), aLayer.ShapeList[shapeIdx]);
                                }
                                else
                                {
                                    aLayer.ShapeList[shapeIdx].Selected = !aLayer.ShapeList[shapeIdx].Selected;
                                    //if (!aLayer.ShapeList[shapeIdx].Selected)
                                    //{
                                    //    this.Refresh();
                                    //    foreach (int sIdx in aLayer.GetSelectedShapeIndexes())
                                    //        DrawIdShape(this.CreateGraphics(), aLayer.ShapeList[sIdx]);
                                    //}
                                    //else
                                    //    DrawIdShape(this.CreateGraphics(), aLayer.ShapeList[shapeIdx]);
                                }

                                OnShapeSelected();
                            }
                        }
                        break;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                switch (_mouseTool)
                {
                    case MouseTools.SelectElements:
                    case MouseTools.MoveSelection:
                    case MouseTools.ResizeSelection:
                        if (_selectedGraphics.Count > 0)
                        {
                            Graphic aGraphic = _selectedGraphics.GraphicList[0];
                            ContextMenuStrip mnuGraphic = new ContextMenuStrip();                           
                            ToolStripMenuItem orderItem = new ToolStripMenuItem("Order");
                            orderItem.DropDownItems.Add("Bring to Front");
                            orderItem.DropDownItems[orderItem.DropDownItems.Count - 1].Click += new EventHandler(OnBringToFrontClick);
                            orderItem.DropDownItems.Add("Send to Back");
                            orderItem.DropDownItems[orderItem.DropDownItems.Count - 1].Click += new EventHandler(OnSendToBackClick);
                            orderItem.DropDownItems.Add("Bring Forward");
                            orderItem.DropDownItems[orderItem.DropDownItems.Count - 1].Click += new EventHandler(OnBringForwardClick);
                            orderItem.DropDownItems.Add("Send Backward");
                            orderItem.DropDownItems[orderItem.DropDownItems.Count - 1].Click += new EventHandler(OnSendBackwardClick);
                            mnuGraphic.Items.Add(orderItem);
                            mnuGraphic.Items.Add(new ToolStripSeparator());
                            mnuGraphic.Items.Add("Remove");
                            mnuGraphic.Items[mnuGraphic.Items.Count - 1].Click += new EventHandler(OnRemoveGraphicClick);

                            if (aGraphic.Legend.BreakType == BreakTypes.PolylineBreak || aGraphic.Legend.BreakType == BreakTypes.PolygonBreak)
                            {
                                mnuGraphic.Items.Add("Reverse");
                                mnuGraphic.Items[mnuGraphic.Items.Count - 1].Click += new EventHandler(OnReverseGraphicClick);
                                if (aGraphic.Shape.ShapeType == ShapeTypes.Polyline || aGraphic.Shape.ShapeType == ShapeTypes.Polygon)
                                {
                                    mnuGraphic.Items.Add(new ToolStripSeparator());
                                    mnuGraphic.Items.Add("Smooth Graphic");
                                    mnuGraphic.Items[mnuGraphic.Items.Count - 1].Click += new EventHandler(OnGrahpicSmoothClick);
                                }
                                if (aGraphic.Legend.BreakType == BreakTypes.PolygonBreak)
                                {
                                    mnuGraphic.Items.Add(new ToolStripSeparator());
                                    mnuGraphic.Items.Add("Set Maskout");
                                    if (((PolygonBreak)aGraphic.Legend).IsMaskout)
                                        mnuGraphic.Items[mnuGraphic.Items.Count - 1].Text = "Not Maskout";
                                    mnuGraphic.Items[mnuGraphic.Items.Count - 1].Click += new EventHandler(OnGraphicMaskoutClick);
                                }                                
                            }

                            Point aPoint = new Point(0, 0);
                            aPoint.X = e.X;
                            aPoint.Y = e.Y;
                            mnuGraphic.Show(this, aPoint);
                        }
                        break;
                }
            }
        }

        private void OnRemoveGraphicClick(object sender, EventArgs e)
        {
            RemoveSelectedGraphics();
            _startNewGraphic = true;
            PaintLayers();
        }

        private void OnBringToFrontClick(object sender, EventArgs e)
        {
            Graphic aG = _selectedGraphics.GraphicList[0];
            int idx = _graphicCollection.GraphicList.IndexOf(aG);
            if (idx < _graphicCollection.Count - 1)
            {
                _graphicCollection.Remove(aG);
                _graphicCollection.Add(aG);
                this.PaintLayers();
            }
        }

        private void OnSendToBackClick(object sender, EventArgs e)
        {
            Graphic aG = _selectedGraphics.GraphicList[0];
            int idx = _graphicCollection.GraphicList.IndexOf(aG);
            if (idx > 0)
            {
                _graphicCollection.Remove(aG);
                _graphicCollection.GraphicList.Insert(0, aG);
                this.PaintLayers();
            }            
        }

        private void OnBringForwardClick(object sender, EventArgs e)
        {
            Graphic aG = _selectedGraphics.GraphicList[0];
            int idx = _graphicCollection.GraphicList.IndexOf(aG);
            if (idx < _graphicCollection.Count - 1)
            {
                _graphicCollection.Remove(aG);
                _graphicCollection.GraphicList.Insert(idx + 1, aG);
                this.PaintLayers();
            }
        }

        private void OnSendBackwardClick(object sender, EventArgs e)
        {
            Graphic aG = _selectedGraphics.GraphicList[0];
            int idx = _graphicCollection.GraphicList.IndexOf(aG);
            if (idx > 0)
            {
                _graphicCollection.Remove(aG);
                _graphicCollection.GraphicList.Insert(idx - 1, aG);
                this.PaintLayers();
            }   
        }

        private void OnReverseGraphicClick(object sender, EventArgs e)
        {
            Graphic aGraphic = _selectedGraphics.GraphicList[0];
            List<PointD> points = aGraphic.Shape.GetPoints();
            points.Reverse();
            aGraphic.Shape.SetPoints(points);
            this.PaintLayers();
        }

        private void OnGrahpicSmoothClick(object sender, EventArgs e)
        {
            Graphic aGraphic = _selectedGraphics.GraphicList[0];
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
            this.PaintLayers();
        }

        private void OnGraphicMaskoutClick(object sender, EventArgs e)
        {
            Graphic aGraphic = _selectedGraphics.GraphicList[0];
            ((PolygonBreak)aGraphic.Legend).IsMaskout = !((PolygonBreak)aGraphic.Legend).IsMaskout;
            this.PaintLayers();
        }

        /// <summary>
        /// MapView mouse double click event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);            

            switch (_mouseTool)
            {
                case MouseTools.SelectElements:
                case MouseTools.MoveSelection:
                case MouseTools.ResizeSelection:
                    if (_mouseTool == MouseTools.MoveSelection)
                        _mouseDoubleClicked = true;

                    if (_selectedGraphics.GraphicList.Count == 0)
                        return;

                    Graphic aGraphic = _selectedGraphics.GraphicList[0];   

                    //Remove selected graphics
                    foreach (Graphic aG in _selectedGraphics.GraphicList)
                    {
                        aG.Shape.Selected = false;
                    }
                    _selectedGraphics.GraphicList.Clear();                                             

                    //Select graphics                    
                    PointF mousePoint = new PointF(_mouseDownPoint.X, _mouseDownPoint.Y);
                    double lonShift = 0;
                    if (SelectGraphics(mousePoint, ref _selectedGraphics, ref lonShift))
                    {
                        if (_selectedGraphics.Count > 1)
                        {
                            aGraphic.Shape.Selected = false;
                            int idx = _selectedGraphics.GraphicList.IndexOf(aGraphic);
                            idx += 1;
                            if (idx > _selectedGraphics.Count - 1)
                                idx = 0;
                            aGraphic = _selectedGraphics.GraphicList[idx];
                            _selectedGraphics.GraphicList.Clear();
                            _selectedGraphics.Add(aGraphic);
                        }
                        //aGraphic = _selectedGraphics.GraphicList[0];
                        aGraphic.Shape.Selected = true;
                        this.PaintLayers();

                        switch (aGraphic.Legend.BreakType)
                        {
                            case BreakTypes.PointBreak:
                                if (!_frmPointSymbolSet.Visible)
                                {
                                    _frmPointSymbolSet = new frmPointSymbolSet(this);
                                    _frmPointSymbolSet.Show(this);
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
                                if (!_frmPolylinSymbolSet.Visible)
                                {
                                    _frmPolylinSymbolSet = new frmPolylineSymbolSet(this);
                                    _frmPolylinSymbolSet.Show(this);
                                }
                                _frmPolylinSymbolSet.PolylineBreak = (PolyLineBreak)aGraphic.Legend;
                                break;
                            case BreakTypes.PolygonBreak:
                                if (!_frmPolygonSymbolSet.Visible)
                                {
                                    _frmPolygonSymbolSet = new frmPolygonSymbolSet(this);
                                    _frmPolygonSymbolSet.Show(this);
                                }
                                _frmPolygonSymbolSet.PolygonBreak = (PolygonBreak)aGraphic.Legend;
                                break;
                        }                     
                    }
                    break;
                case MouseTools.New_Polyline:
                case MouseTools.New_Polygon:
                case MouseTools.New_Curve:
                case MouseTools.New_CurvePolygon:
                //case MouseTools.New_Freehand:
                case MouseTools.SelectFeatures_Polygon:
                    if (!_startNewGraphic)
                    {
                        _startNewGraphic = true;
                        //_graphicPoints.Add(new PointF(e.X, e.Y));
                        _graphicPoints.RemoveAt(_graphicPoints.Count - 1);
                        List<PointD> points = new List<PointD>();
                        double projX = 0, projY = 0;
                        foreach (PointF aPoint in _graphicPoints)
                        {
                            ScreenToProj(ref projX, ref projY, aPoint.X, aPoint.Y);
                            points.Add(new PointD(projX, projY));
                        }

                        aGraphic = null;
                        switch (_mouseTool)
                        {
                            case MouseTools.New_Polyline:
                            //case MouseTools.New_Freehand:
                                if (points.Count > 1)
                                {
                                    PolylineShape aPLS = new PolylineShape();
                                    aPLS.Points = points;
                                    aGraphic = new Graphic(aPLS, (PolyLineBreak)_defPolylineBreak.Clone());
                                }
                                break;
                            case MouseTools.New_Polygon:
                                if (points.Count > 2)
                                {
                                    PolygonShape aPGS = new PolygonShape();
                                    points.Add((PointD)points[0].Clone());
                                    aPGS.Points = points;
                                    aGraphic = new Graphic(aPGS, (PolygonBreak)_defPolygonBreak.Clone());
                                }
                                break;
                            case MouseTools.New_Curve:
                                if (points.Count > 1)
                                {
                                    CurveLineShape aCLS = new CurveLineShape();
                                    aCLS.Points = points;
                                    aGraphic = new Graphic(aCLS, (PolyLineBreak)_defPolylineBreak.Clone());
                                }
                                break;
                            case MouseTools.New_CurvePolygon:
                                if (points.Count > 2)
                                {
                                    CurvePolygonShape aCPS = new CurvePolygonShape();
                                    aCPS.Points = points;
                                    aGraphic = new Graphic(aCPS, (PolygonBreak)_defPolygonBreak.Clone());
                                }
                                break;
                        }

                        if (_mouseTool == MouseTools.SelectFeatures_Polygon)
                        {
                            MapLayer layer = this.GetLayerFromHandle(_selectedLayer);
                            if (layer == null)
                                return;
                            if (layer.LayerType != LayerTypes.VectorLayer)
                                return;

                            PolygonShape aPGS = new PolygonShape();
                            points.Add((PointD)points[0].Clone());
                            aPGS.Points = points;
                            VectorLayer aLayer = (VectorLayer)layer;
                            if (!((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                            {
                                aLayer.ClearSelectedShapes();
                            }
                            aLayer.SelectShapes(aPGS);
                            OnShapeSelected();
                        }
                        else
                        {
                            if (aGraphic != null)
                            {
                                _graphicCollection.Add(aGraphic);
                                PaintLayers();
                            }
                            else
                                this.Refresh();
                        }
                    }
                    break;                    
            }
        }
          
        /// <summary>
        /// MapView mouse down event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                Graphics g = this.CreateGraphics();
                switch (_mouseTool)
                {
                    case MouseTools.Zoom_In:
                        break;
                    case MouseTools.Pan:
                        //PaintOnlyLayers(0, 0);
                        //_mapBitmap = new Bitmap(10, 10, PixelFormat.Format32bppPArgb);
                        break;
                    case MouseTools.SelectElements:
                        PointF mousePoint = new PointF();
                        mousePoint.X = e.X;
                        mousePoint.Y = e.Y;
                        double lonShift = 0;

                        GraphicCollection tempGraphics = new GraphicCollection();
                        if (SelectGraphics(mousePoint, _selectedGraphics, ref tempGraphics, ref lonShift, 3))
                        {
                            //_isInSelectedGraphics = true;
                            _selectedRectangle = GetGraphicRectangle(g, _selectedGraphics.GraphicList[0], lonShift);
                            _resizeRectangle = _selectedRectangle;
                            if (_resizeSelectedEdge == Edge.None)
                                _mouseTool = MouseTools.MoveSelection;
                            else
                            {
                                _mouseTool = MouseTools.ResizeSelection;
                            }
                        }
                        else
                            _mouseTool = MouseTools.CreateSelection;

                        break;
                    case MouseTools.New_Point:
                        PointShape aPS = new PointShape();
                        double projX = 0, projY = 0;
                        ScreenToProj(ref projX, ref projY, e.X, e.Y);
                        aPS.Point = new MeteoInfoC.PointD(projX, projY);
                        //PointBreak aPB = new PointBreak();
                        //aPB.Size = 10;
                        Graphic aGraphic = new Graphic();
                        aGraphic.Shape = aPS;
                        aGraphic.Legend = (PointBreak)_defPointBreak.Clone();
                        _graphicCollection.Add(aGraphic);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        DrawGraphic(g, aGraphic, 0);
                        break;
                    case MouseTools.New_Label:
                        //LabelBreak aLB = new LabelBreak();
                        //aLB.Text = "Text";
                        //aLB.Font = new Font("Arial", 12);
                        projX = 0;
                        projY = 0;
                        ScreenToProj(ref projX, ref projY, e.X, e.Y);
                        aPS = new PointShape();
                        aPS.Point = new MeteoInfoC.PointD(projX, projY);
                        aGraphic = new Graphic(aPS, (LabelBreak)_defLabelBreak.Clone());
                        _graphicCollection.Add(aGraphic);
                        DrawGraphic(g, aGraphic, 0);
                        break;
                    case MouseTools.New_Polyline:
                    case MouseTools.New_Polygon:
                    case MouseTools.New_Curve:
                    case MouseTools.New_CurvePolygon:
                    case MouseTools.New_Freehand:
                    case MouseTools.SelectFeatures_Polygon:
                    case MouseTools.SelectFeatures_Lasso:
                        if (_startNewGraphic)
                        {
                            _graphicPoints = new List<PointF>();
                            _startNewGraphic = false;
                        }
                        _graphicPoints.Add(new PointF(e.X, e.Y));
                        break;
                    case MouseTools.EditVertices:
                        if (_selectedGraphics.Count > 0)
                        {
                            if (SelectEditVertices(new Point(e.X, e.Y), _selectedGraphics.GraphicList[0].Shape,
                                ref _editingVertices, ref _editingVerticeIndex))
                            {
                                _mouseTool = MouseTools.InEditingVertices;
                            }
                        }
                        break;
                    case MouseTools.Measurement:
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
                                    MapLayer aMLayer = GetLayerFromHandle(SelectedLayer);
                                    if (aMLayer != null)
                                    {
                                        if (aMLayer.LayerType == LayerTypes.VectorLayer)
                                        {
                                            VectorLayer aLayer = (VectorLayer)aMLayer;
                                            if (aLayer.ShapeType != ShapeTypes.Point)
                                            {
                                                List<int> SelectedShapes = new List<int>();
                                                PointF aPoint = new PointF(e.X, e.Y);
                                                if (SelectShapes(aLayer, aPoint, ref SelectedShapes))
                                                {
                                                    Shape.Shape aShape = aLayer.ShapeList[SelectedShapes[0]];
                                                    this.Refresh();
                                                    DrawIdShape(g, aShape);
                                                    double value = 0.0;
                                                    switch (aShape.ShapeType)
                                                    {
                                                        case ShapeTypes.Polyline:
                                                        case ShapeTypes.PolylineZ:
                                                            _frmMeasure.IsArea = false;
                                                            double areaValue = 0.0;
                                                            if (_projection.IsLonLatMap)
                                                            {
                                                                value = GeoComputation.GetDistance(((PolylineShape)aShape).Points, true);
                                                                if (((PolylineShape)aShape).IsClosed)
                                                                {
                                                                    areaValue = GeoComputation.SphericalPolygonArea(aShape.GetPoints());
                                                                }
                                                            }
                                                            else
                                                            {
                                                                value = ((PolylineShape)aShape).Length;
                                                                value *= _projection.ProjInfo.Unit.Meters;
                                                                if (((PolylineShape)aShape).IsClosed)
                                                                {
                                                                    areaValue = GeoComputation.GetArea(aShape.GetPoints());
                                                                }
                                                            }
                                                            _frmMeasure.CurrentValue = value;
                                                            if (((PolylineShape)aShape).IsClosed)
                                                            {
                                                                _frmMeasure.AreaValue = areaValue;
                                                            }
                                                            break;
                                                        case ShapeTypes.Polygon:
                                                        case ShapeTypes.PolygonM:
                                                            _frmMeasure.IsArea = true;
                                                            if (_projection.IsLonLatMap)
                                                            {
                                                                //ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
                                                                //string toProjStr = "+proj=aea +lat_1=25 +lat_2=47 +lat_0=40 +lon_0=105";
                                                                //ProjectionInfo toProj = new ProjectionInfo(toProjStr);
                                                                //PolygonShape aPGS = (PolygonShape)((PolygonShape)aShape).Clone();
                                                                //aPGS = _projection.ProjectPolygonShape(aPGS, fromProj, toProj);
                                                                //value = aPGS.Area;
                                                                value = ((PolygonShape)aShape).SphericalArea;
                                                            }
                                                            else
                                                                value = ((PolygonShape)aShape).Area;
                                                            value *= _projection.ProjInfo.Unit.Meters * _projection.ProjInfo.Unit.Meters;
                                                            _frmMeasure.CurrentValue = value;
                                                            break;
                                                    }                                                    
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
                switch (_mouseTool)
                {
                    case MouseTools.Measurement:
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
        /// MapView mouse move event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int deltaX = e.X - _mouseLastPos.X;
            int deltaY = e.Y - _mouseLastPos.Y;
            _mouseLastPos.X = e.X;
            _mouseLastPos.Y = e.Y;

            Graphics g;
            g = this.CreateGraphics();
            Single aWidth, aHeight, aX, aY;
            Pen aPen = new Pen(this.ForeColor);
            switch (_mouseTool)
            {
                case MouseTools.Zoom_In:
                    if (e.Button == MouseButtons.Left)
                    {
                        this.Refresh();
                        aWidth = Math.Abs(e.X - _mouseDownPoint.X);
                        aHeight = Math.Abs(e.Y - _mouseDownPoint.Y);
                        aX = Math.Min(e.X, _mouseDownPoint.X);
                        aY = Math.Min(e.Y, _mouseDownPoint.Y);
                        aPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        g.DrawRectangle(aPen, aX, aY, aWidth, aHeight);
                    }
                    break;
                case MouseTools.Pan:
                    if (e.Button == MouseButtons.Left)
                    {
                        _xShift = e.X - _mouseDownPoint.X;
                        _yShift = e.Y - _mouseDownPoint.Y;
                        //Image dImage = new Bitmap(this.Width, this.Height);
                        //Graphics dg = Graphics.FromImage(dImage);
                        //dg.DrawImage(_mapBitmap, new RectangleF(_xShift, _yShift, this.Width, this.Height));
                        //_mapBitmap = (Bitmap)dImage;
                        //dg.Dispose();
                        this.Refresh();
                    }
                    //if (e.Button == MouseButtons.Left)
                    //{
                    //    //g.Clear(this.MapBackColor);
                    //    //PicBox_Layout.Refresh();

                    //    Color bColor = this.BackColor;
                    //    if (bColor == Color.Transparent)
                    //        bColor = Color.White;
                    //    SolidBrush aBrush = new SolidBrush(bColor);
                    //    aX = e.X - _mouseDownPoint.X;
                    //    aY = e.Y - _mouseDownPoint.Y;
                    //    Rectangle mapRect = new Rectangle(0, 0, this.Width, this.Height);
                    //    //g.Clip = new Region(mapRect);
                    //    //PaintOnlyLayers(aX, aY);                        
                    //    if (aX > 0)
                    //    {
                    //        g.FillRectangle(aBrush, mapRect.X, mapRect.Y, aX, mapRect.Height);
                    //    }
                    //    else
                    //    {
                    //        g.FillRectangle(aBrush, mapRect.Right + aX, mapRect.Y, Math.Abs(aX), mapRect.Height);
                    //    }
                    //    if (aY > 0)
                    //    {
                    //        g.FillRectangle(aBrush, mapRect.X, mapRect.Y, mapRect.Width, aY);
                    //    }
                    //    else
                    //    {
                    //        g.FillRectangle(aBrush, mapRect.X, mapRect.Bottom + aY, mapRect.Width, Math.Abs(aY));
                    //    }
                    //    //g.FillRectangle(Brushes.White, mapRect);
                    //    g.DrawImageUnscaled(_tempMapBitmap, (int)aX, (int)aY);
                    //    //g.DrawImageUnscaled(_tempMapBitmap, 0, 0);
                    //    //g.ResetClip();     

                    //    //Draw neatline                        
                    //    g.DrawRectangle(new Pen(Color.Black, 2), mapRect);
                    //}
                    break;
                case MouseTools.SelectElements:
                    if (_selectedGraphics.Count > 0)
                    {

                        GraphicCollection tempGraphics = new GraphicCollection();
                        double lonShift = 0;
                        if (SelectGraphics(new PointF(e.X, e.Y), _selectedGraphics, ref tempGraphics, ref lonShift, 3))
                        {
                            //Change mouse cursor
                            Rectangle aRect = GetGraphicRectangle(g, _selectedGraphics.GraphicList[0], lonShift);
                            _resizeSelectedEdge = IntersectElementEdge(aRect, new PointF(e.X, e.Y), 3F);
                            switch (_selectedGraphics.GraphicList[0].ResizeAbility)
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
                case MouseTools.CreateSelection:
                case MouseTools.SelectFeatures_Rectangle:
                    if (e.Button == MouseButtons.Left)
                    {
                        this.Refresh();
                        int sx = Math.Min(_mouseDownPoint.X, e.X);
                        int sy = Math.Min(_mouseDownPoint.Y, e.Y);
                        g.DrawRectangle(new Pen(ForeColor),
                            new Rectangle(new Point(sx, sy), new Size(Math.Abs(e.X - _mouseDownPoint.X),
                            Math.Abs(e.Y - _mouseDownPoint.Y))));
                    }
                    break;
                case MouseTools.MoveSelection:
                    //Move selected graphics
                    if (e.Button == MouseButtons.Left)
                    {
                        this.Cursor = Cursors.SizeAll;
                        this.Refresh();

                        Rectangle rect = new Rectangle();
                        rect.X = _selectedRectangle.X + e.X - _mouseDownPoint.X;
                        rect.Y = _selectedRectangle.Y + e.Y - _mouseDownPoint.Y;
                        rect.Width = _selectedRectangle.Width;
                        rect.Height = _selectedRectangle.Height;

                        aPen.Color = Color.Red;
                        aPen.DashStyle = DashStyle.Dash;
                        g.DrawRectangle(aPen, rect);
                    }
                    break;
                case MouseTools.ResizeSelection:
                    Graphic aGraphic = _selectedGraphics.GraphicList[0];
                    if (_selectedRectangle.Width > 2 && _selectedRectangle.Height > 2)
                    {
                        switch (aGraphic.ResizeAbility)
                        {
                            case ResizeAbility.SameWidthHeight:
                                //deltaY = deltaX;
                                switch (_resizeSelectedEdge)
                                {
                                    case Edge.TopLeft:
                                        _resizeRectangle.X += deltaX;
                                        _resizeRectangle.Y += deltaX;
                                        _resizeRectangle.Width -= deltaX;
                                        _resizeRectangle.Height -= deltaX;
                                        break;
                                    case Edge.BottomRight:
                                        _resizeRectangle.Width += deltaX;
                                        _resizeRectangle.Height += deltaX;
                                        break;
                                    case Edge.TopRight:
                                        _resizeRectangle.Y += deltaY;
                                        _resizeRectangle.Width -= deltaY;
                                        _resizeRectangle.Height -= deltaY;
                                        break;
                                    case Edge.BottomLeft:
                                        _resizeRectangle.X += deltaX;
                                        _resizeRectangle.Width -= deltaX;
                                        _resizeRectangle.Height -= deltaX;
                                        break;
                                }
                                break;
                            case ResizeAbility.ResizeAll:
                                switch (_resizeSelectedEdge)
                                {
                                    case Edge.TopLeft:
                                        _resizeRectangle.X += deltaX;
                                        _resizeRectangle.Y += deltaY;
                                        _resizeRectangle.Width -= deltaX;
                                        _resizeRectangle.Height -= deltaY;
                                        break;
                                    case Edge.BottomRight:
                                        _resizeRectangle.Width += deltaX;
                                        _resizeRectangle.Height += deltaY;
                                        break;
                                    case Edge.Top:
                                        _resizeRectangle.Y += deltaY;
                                        _resizeRectangle.Height -= deltaY;
                                        break;
                                    case Edge.Bottom:
                                        _resizeRectangle.Height += deltaY;
                                        break;
                                    case Edge.TopRight:
                                        _resizeRectangle.Y += deltaY;
                                        _resizeRectangle.Width += deltaX;
                                        _resizeRectangle.Height -= deltaY;
                                        break;
                                    case Edge.BottomLeft:
                                        _resizeRectangle.X += deltaX;
                                        _resizeRectangle.Width -= deltaX;
                                        _resizeRectangle.Height += deltaY;
                                        break;
                                    case Edge.Left:
                                        _resizeRectangle.X += deltaX;
                                        _resizeRectangle.Width -= deltaX;
                                        break;
                                    case Edge.Right:
                                        _resizeRectangle.Width += deltaX;
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        _resizeRectangle.Width = 3;
                        _resizeRectangle.Height = 3;
                    }
                    //this.PaintGraphics();
                    this.Refresh();
                    aPen.Color = Color.Red;
                    aPen.DashStyle = DashStyle.Dash;
                    g.DrawRectangle(aPen, _resizeRectangle);
                    break;
                case MouseTools.New_Polyline:
                case MouseTools.New_Polygon:
                case MouseTools.New_Curve:
                case MouseTools.New_CurvePolygon:
                case MouseTools.New_Freehand:
                case MouseTools.SelectFeatures_Polygon:
                case MouseTools.SelectFeatures_Lasso:
                    if (!_startNewGraphic)
                    {
                        this.Refresh();
                        PointF[] points = _graphicPoints.ToArray();
                        Array.Resize(ref points, _graphicPoints.Count + 1);
                        points[_graphicPoints.Count] = new PointF(e.X, e.Y);

                        switch (_mouseTool)
                        {
                            case MouseTools.New_Polyline:
                                g.DrawLines(new Pen(ForeColor), points);
                                break;
                            case MouseTools.New_Polygon:
                            case MouseTools.SelectFeatures_Polygon:
                                g.DrawPolygon(new Pen(ForeColor), points);
                                break;
                            case MouseTools.SelectFeatures_Lasso:
                                _graphicPoints.Add(new PointF(e.X, e.Y));
                                g.DrawPolygon(new Pen(ForeColor), points);
                                break;
                            case MouseTools.New_Curve:
                                g.DrawCurve(new Pen(ForeColor), points);
                                break;
                            case MouseTools.New_CurvePolygon:
                                g.DrawCurve(new Pen(ForeColor), points);
                                break;
                            case MouseTools.New_Freehand:                                
                                _graphicPoints.Add(new PointF(e.X, e.Y));
                                g.DrawLines(new Pen(ForeColor), points);
                                break;
                        }
                    }
                    break;
                case MouseTools.New_Rectangle:
                case MouseTools.New_Ellipse:
                    if (e.Button == MouseButtons.Left)
                    {
                        this.Refresh();
                        int sx = Math.Min(_mouseDownPoint.X, e.X);
                        int sy = Math.Min(_mouseDownPoint.Y, e.Y);
                        g.DrawRectangle(new Pen(ForeColor),
                            new Rectangle(new Point(sx, sy), new Size(Math.Abs(e.X - _mouseDownPoint.X),
                            Math.Abs(e.Y - _mouseDownPoint.Y))));
                    }
                    break;
                case MouseTools.New_Circle:
                case MouseTools.SelectFeatures_Circle:
                    if (e.Button == MouseButtons.Left)
                    {
                        this.Refresh();
                        int radius = (int)Math.Sqrt(Math.Pow(e.X - _mouseDownPoint.X, 2) + Math.Pow(e.Y - _mouseDownPoint.Y, 2));
                        g.DrawLine(new Pen(ForeColor), _mouseDownPoint.X, _mouseDownPoint.Y, e.X, e.Y);
                        g.DrawEllipse(new Pen(ForeColor), _mouseDownPoint.X - radius, _mouseDownPoint.Y - radius,
                            radius * 2, radius * 2);
                    }
                    break;
                case MouseTools.EditVertices:
                    if (_selectedGraphics.Count > 0)
                    {
                        if (SelectEditVertices(new Point(e.X, e.Y), _selectedGraphics.GraphicList[0].Shape,
                            ref _editingVertices, ref _editingVerticeIndex))
                        {
                            this.Cursor = new Cursor(this.GetType().Assembly.
                                GetManifestResourceStream("MeteoInfoC.Resources.MoveVertex.ico"));
                        }
                        else
                            this.Cursor = Cursors.Default;
                    }
                    break;
                case MouseTools.InEditingVertices:
                    this.Refresh();
                    float x1 = 0, x2 = 0;
                    ProjToScreen(_editingVertices[1].X, _editingVertices[1].Y, ref x1, ref x2);
                    g.DrawLine(new Pen(Color.Black), (int)x1, (int)x2, e.X, e.Y);
                    if (_editingVertices.Count == 3)
                    {
                        ProjToScreen(_editingVertices[2].X, _editingVertices[2].Y, ref x1, ref x2);
                        g.DrawLine(new Pen(ForeColor), (int)x1, (int)x2, e.X, e.Y);
                    }

                    Rectangle nRect = new Rectangle(e.X - 3, e.Y - 3, 6, 6);
                    g.FillRectangle(new SolidBrush(Color.Cyan), nRect);
                    g.DrawRectangle(new Pen(Color.Black), nRect);
                    break;
                case MouseTools.Measurement:
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
                                    double ProjX = 0, ProjY = 0;
                                    ScreenToProj(ref ProjX, ref ProjY, e.X, e.Y);
                                    if (_frmMeasure.MeasureType == MeasureType.Length)
                                    {                                        
                                        double pProjX = 0, pProjY = 0;
                                        ScreenToProj(ref pProjX, ref pProjY,
                                            _mouseDownPoint.X, _mouseDownPoint.Y);
                                        double dx = Math.Abs(ProjX - pProjX);
                                        double dy = Math.Abs(ProjY - pProjY);
                                        double dist;
                                        if (_projection.IsLonLatMap)
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
                                            dist *= _projection.ProjInfo.Unit.Meters;
                                        }

                                        _frmMeasure.CurrentValue = dist;
                                    }
                                    else
                                    {
                                        List<PointD> mPoints = new List<PointD>();
                                        for (int i = 0; i < points.Length; i++)
                                        {
                                            ScreenToProj(ref ProjX, ref ProjY,
                                                points[i].X, points[i].Y);
                                            mPoints.Add(new PointD(ProjX, ProjY));
                                        }
                                        double area = GeoComputation.GetArea(mPoints);
                                        if (_projection.IsLonLatMap)
                                        {
                                            area = area * 111319.5 * 111319.5;
                                        }
                                        else
                                        {
                                            area *= _projection.ProjInfo.Unit.Meters * _projection.ProjInfo.Unit.Meters;
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
        /// MapView mouse up event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            double MinX, MaxX, MinY, MaxY, lonRan, latRan, ZoomF, ZoomFY;
            double mouseLon, mouseLat, lon, lat;
            lonRan = _drawExtent.maxX - _drawExtent.minX;
            latRan = _drawExtent.maxY - _drawExtent.minY;
            mouseLon = _drawExtent.minX + e.X / _scaleX;
            mouseLat = _drawExtent.maxY - e.Y / _scaleY;

            _mousePos.X = e.X;
            _mousePos.Y = e.Y;
            switch (_mouseTool)
            {
                case MouseTools.Zoom_In:
                    if (Math.Abs(_mousePos.X - _mouseDownPoint.X) > 5)
                    {
                        ZoomF = Math.Abs(_mousePos.X - _mouseDownPoint.X) / Convert.ToDouble(this.Width );
                        ZoomFY = Math.Abs(_mousePos.Y - _mouseDownPoint.Y) / Convert.ToDouble(this.Height);
                        if (_isGeoMap)
                        {
                            if (ZoomF < ZoomFY)
                            {
                                ZoomF = ZoomFY;
                            }
                            else
                            {
                                ZoomFY = ZoomF;
                            }
                        }
                        mouseLon = _drawExtent.minX + ((_mouseDownPoint.X + (_mousePos.X - _mouseDownPoint.X) / 2)) / _scaleX;
                        mouseLat = _drawExtent.maxY - ((_mouseDownPoint.Y + (_mousePos.Y - _mouseDownPoint.Y) / 2)) / _scaleY;
                        MinX = mouseLon - (lonRan / 2 * ZoomF);
                        MaxX = mouseLon + (lonRan / 2 * ZoomF);
                        MinY = mouseLat - (latRan / 2 * ZoomFY);
                        MaxY = mouseLat + (latRan / 2 * ZoomFY);
                    }
                    else
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            ZoomF = 0.75;
                        }
                        else
                        {
                            ZoomF = 1.5;
                        }
                        MinX = mouseLon - (lonRan / 2 * ZoomF);
                        MaxX = mouseLon + (lonRan / 2 * ZoomF);
                        MinY = mouseLat - (latRan / 2 * ZoomF);
                        MaxY = mouseLat + (latRan / 2 * ZoomF);
                    }

                    if (MaxX - MinX > 0.001)
                    {
                        //m_MapExtentSet.SetCoordinateGeoMap(MinX, MaxX, MinY, MaxY, this.Width, this.Height, _projection.IsLonLatMap);
                        ZoomToExtent(MinX, MaxX, MinY, MaxY); 
                    }
                    break;
                case MouseTools.Zoom_Out:
                    if (e.Button == MouseButtons.Left)
                    {
                        ZoomF = 1.5;
                    }
                    else
                    {
                        ZoomF = 0.75;
                    }
                    MinX = mouseLon - (lonRan / 2 * ZoomF);
                    MaxX = mouseLon + (lonRan / 2 * ZoomF);
                    MinY = mouseLat - (latRan / 2 * ZoomF);
                    MaxY = mouseLat + (latRan / 2 * ZoomF);

                    ZoomToExtent(MinX, MaxX, MinY, MaxY);
                    //m_MapExtentSet.SetCoordinateGeoMap(MinX, MaxX, MinY, MaxY, this.Width, this.Height, _projection.IsLonLatMap);
                    break;
                case MouseTools.Pan:
                    if (e.Button == MouseButtons.Left)
                    {
                        _xShift = 0;
                        _yShift = 0;

                        lon = _drawExtent.minX + _mouseDownPoint.X / _scaleX;
                        lat = _drawExtent.maxY - _mouseDownPoint.Y / _scaleY;
                        MinX = _drawExtent.minX - (mouseLon - lon);
                        MaxX = _drawExtent.maxX - (mouseLon - lon);
                        MinY = _drawExtent.minY - (mouseLat - lat);
                        MaxY = _drawExtent.maxY - (mouseLat - lat);

                        ZoomToExtent(MinX, MaxX, MinY, MaxY);
                        //m_MapExtentSet.SetCoordinateGeoMap(MinX, MaxX, MinY, MaxY, this.Width, this.Height, _projection.IsLonLatMap);                        
                    }
                    break;
                case MouseTools.SelectFeatures_Rectangle:
                    if (e.Button == MouseButtons.Left)
                    {
                        if (SelectedLayer < 0)
                            return;
                        MapLayer aMLayer = GetLayerFromHandle(SelectedLayer);
                        if (aMLayer == null)
                            return;
                        if (aMLayer.LayerType != LayerTypes.VectorLayer)
                            return;

                        VectorLayer aLayer = (VectorLayer)aMLayer;
                        int minx = Math.Min(_mouseDownPoint.X, e.X);
                        int miny = Math.Min(_mouseDownPoint.Y, e.Y);
                        int width = Math.Abs(e.X - _mouseDownPoint.X);
                        int height = Math.Abs(e.Y - _mouseDownPoint.Y);
                        Rectangle rect = new Rectangle(minx, miny, width, height); 
                        List<int> selectedShapes = SelectShapes(aLayer, rect);
                        if (selectedShapes.Count > 0)
                        {
                            if (!((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                            {
                                aLayer.ClearSelectedShapes();
                            }
                            this.Refresh();
                            foreach (int shapeIdx in selectedShapes)
                            {
                                aLayer.ShapeList[shapeIdx].Selected = true;
                                //DrawIdShape(this.CreateGraphics(), aLayer.ShapeList[shapeIdx]);                                
                            }

                            OnShapeSelected();
                        }
                    }
                    break;
                case MouseTools.CreateSelection:
                    if (e.Button == MouseButtons.Left)
                    {
                        if (!((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                        {
                            //Remove selected graphics
                            foreach (Graphic aGraphic in _selectedGraphics.GraphicList)
                            {
                                aGraphic.Shape.Selected = false;
                            }
                            _selectedGraphics.GraphicList.Clear();
                        }

                        //Select graphics
                        if (Math.Abs(e.X - _mouseDownPoint.X) > 5 || Math.Abs(e.Y - _mouseDownPoint.Y) > 5)
                        {
                            int minx = Math.Min(_mouseDownPoint.X, e.X);
                            int miny = Math.Min(_mouseDownPoint.Y, e.Y);
                            int width = Math.Abs(e.X - _mouseDownPoint.X);
                            int height = Math.Abs(e.Y - _mouseDownPoint.Y);
                            Rectangle rect = new Rectangle(minx, miny, width, height);
                            double lonShift = 0;
                            GraphicCollection tempGraphics = new GraphicCollection();
                            if (SelectGraphics(rect, ref tempGraphics, ref lonShift))
                            {                               
                                if (!((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                                {
                                    foreach (Graphic aGraphic in tempGraphics.GraphicList)
                                    {
                                        aGraphic.Shape.Selected = true;
                                        _selectedGraphics.Add(aGraphic);
                                    }
                                }
                                else
                                {
                                    foreach (Graphic aGraphic in tempGraphics.GraphicList)
                                    {
                                        aGraphic.Shape.Selected = !aGraphic.Shape.Selected;
                                        if (aGraphic.Shape.Selected)
                                            _selectedGraphics.Add(aGraphic);
                                        else
                                            _selectedGraphics.Remove(aGraphic);
                                    }
                                }
                            }

                            PaintLayers();
                            return;
                        }
                        else
                        {
                            PointF mousePoint = new PointF(_mouseDownPoint.X, _mouseDownPoint.Y);
                            double lonShift = 0;
                            GraphicCollection tempGraphics = new GraphicCollection();
                            if (SelectGraphics(mousePoint, ref tempGraphics, ref lonShift))
                            {
                                Graphic aGraphic = tempGraphics.GraphicList[0];
                                if (!((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                                {
                                    aGraphic.Shape.Selected = true;
                                    _selectedGraphics.Add(aGraphic);
                                }
                                else
                                {
                                    aGraphic.Shape.Selected = !aGraphic.Shape.Selected;
                                    if (aGraphic.Shape.Selected)
                                        _selectedGraphics.Add(aGraphic);
                                    else
                                        _selectedGraphics.Remove(aGraphic);
                                }

                                //Show symbol form
                                switch (aGraphic.Legend.BreakType)
                                {
                                    case BreakTypes.PointBreak:
                                        if (_frmPointSymbolSet.Visible)
                                            _frmPointSymbolSet.PointBreak = (PointBreak)aGraphic.Legend;
                                        break ;
                                    case BreakTypes.LabelBreak:
                                        if (_frmLabelSymbolSet.Visible)
                                            _frmLabelSymbolSet.LabelBreak = (LabelBreak)aGraphic.Legend;
                                        break;
                                    case BreakTypes.PolylineBreak:
                                        if (_frmPolylinSymbolSet.Visible)
                                            _frmPolylinSymbolSet.PolylineBreak = (PolyLineBreak)aGraphic.Legend;
                                        break;
                                    case BreakTypes.PolygonBreak:
                                        if (_frmPolygonSymbolSet.Visible)
                                            _frmPolygonSymbolSet.PolygonBreak = (PolygonBreak)aGraphic.Legend;
                                        break;
                                }

                                OnGraphicSelected();
                            }
                        }

                        PaintLayers();
                    }
                    _mouseTool = MouseTools.SelectElements;
                    break;
                case MouseTools.MoveSelection:
                    if (_mouseDoubleClicked)
                    {
                        _mouseDoubleClicked = false;
                    }
                    else
                    {
                        if (_selectedGraphics.Count > 0)
                        {
                            if (Math.Abs(e.X - _mouseDownPoint.X) < 2 && Math.Abs(e.Y - _mouseDownPoint.Y) < 2)
                            {
                                Graphic aGraphic = _selectedGraphics.GraphicList[0];
                                PointF mousePoint = new PointF(_mouseDownPoint.X, _mouseDownPoint.Y);
                                double lonShift = 0;
                                GraphicCollection tempGraphics = new GraphicCollection();
                                SelectGraphics(mousePoint, ref tempGraphics, ref lonShift);
                                if ((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                                {
                                    if (tempGraphics.Count > 0)
                                    {
                                        aGraphic = tempGraphics.GraphicList[0];
                                        aGraphic.Shape.Selected = !aGraphic.Shape.Selected;
                                        if (aGraphic.Shape.Selected)
                                            _selectedGraphics.Add(aGraphic);
                                        else
                                            _selectedGraphics.Remove(aGraphic);
                                    }
                                }
                                else
                                {
                                    if (tempGraphics.Count > 1)
                                    {
                                        aGraphic.Shape.Selected = false;
                                        int idx = tempGraphics.GraphicList.IndexOf(aGraphic);
                                        if (idx == 0)
                                            idx = tempGraphics.Count - 1;
                                        else
                                            idx -= 1;
                                        aGraphic = tempGraphics.GraphicList[idx];
                                        _selectedGraphics.GraphicList.Clear();
                                        _selectedGraphics.Add(aGraphic);
                                        _selectedGraphics.GraphicList[0].Shape.Selected = true;

                                        //Show symbol form
                                        switch (aGraphic.Legend.BreakType)
                                        {
                                            case BreakTypes.PointBreak:
                                                if (_frmPointSymbolSet.Visible)
                                                    _frmPointSymbolSet.PointBreak = (PointBreak)aGraphic.Legend;
                                                break;
                                            case BreakTypes.LabelBreak:
                                                if (_frmLabelSymbolSet.Visible)
                                                    _frmLabelSymbolSet.LabelBreak = (LabelBreak)aGraphic.Legend;
                                                break;
                                            case BreakTypes.PolylineBreak:
                                                if (_frmPolylinSymbolSet.Visible)
                                                    _frmPolylinSymbolSet.PolylineBreak = (PolyLineBreak)aGraphic.Legend;
                                                break;
                                            case BreakTypes.PolygonBreak:
                                                if (_frmPolygonSymbolSet.Visible)
                                                    _frmPolygonSymbolSet.PolygonBreak = (PolygonBreak)aGraphic.Legend;
                                                break;
                                        }

                                        OnGraphicSelected();
                                    }
                                }
                            }
                            else
                            {
                                Graphic aGraphic = _selectedGraphics.GraphicList[0];
                                Shape.Shape aShape = aGraphic.Shape;
                                MoveShapeOnScreen(ref aShape, _mouseDownPoint, new Point(e.X, e.Y));
                                aGraphic.Shape = aShape;

                                _selectedGraphics.Remove(aGraphic);
                                _selectedGraphics.Insert(0, aGraphic);
                            }

                            PaintLayers();
                        }
                    }
                    _mouseTool = MouseTools.SelectElements;
                    break;
                case MouseTools.ResizeSelection:
                    Graphic aG = _selectedGraphics.GraphicList[0];
                    Shape.Shape shape = aG.Shape;
                    ResizeShapeOnScreen(ref shape, aG.Legend, _resizeRectangle);
                    aG.Shape = shape;

                    _selectedGraphics.Remove(aG);
                    _selectedGraphics.Insert(0, aG);

                    PaintLayers();

                    _mouseTool = MouseTools.SelectElements;
                    break;
                case MouseTools.New_Rectangle:
                case MouseTools.New_Ellipse:
                    if (e.Button == MouseButtons.Left)
                    {
                        if (Math.Abs(e.X - _mouseDownPoint.X) < 2 || Math.Abs(e.Y - _mouseDownPoint.Y) < 2)
                            return;

                        _startNewGraphic = true;
                        _graphicPoints = new List<PointF>();
                        _graphicPoints.Add(new PointF(_mouseDownPoint.X, _mouseDownPoint.Y));
                        _graphicPoints.Add(new PointF(_mouseDownPoint.X, e.Y));
                        _graphicPoints.Add(new PointF(e.X, e.Y));
                        _graphicPoints.Add(new PointF(e.X, _mouseDownPoint.Y));
                        List<PointD> points = new List<PointD>();
                        double projX = 0, projY = 0;
                        foreach (PointF aPoint in _graphicPoints)
                        {
                            ScreenToProj(ref projX, ref projY, aPoint.X, aPoint.Y);
                            points.Add(new PointD(projX, projY));
                        }

                        Graphic aGraphic = null;
                        switch (_mouseTool)
                        {
                            case MouseTools.New_Rectangle:
                                RectangleShape aPGS = new RectangleShape();
                                aPGS.Points = points;
                                aGraphic = new Graphic(aPGS, (PolygonBreak)_defPolygonBreak.Clone());
                                break;
                            case MouseTools.New_Ellipse:
                                EllipseShape aES = new EllipseShape();
                                aES.Points = points;
                                aGraphic = new Graphic(aES, (PolygonBreak)_defPolygonBreak.Clone());
                                break;
                        }                        

                        if (aGraphic != null)
                        {
                            _graphicCollection.Add(aGraphic);
                            PaintLayers();
                        }
                        else
                            this.Refresh();
                    }
                    break;
                case MouseTools.New_Freehand:
                case MouseTools.SelectFeatures_Lasso:
                    if (e.Button == MouseButtons.Left)
                    {
                        _startNewGraphic = true;
                        if (_graphicPoints.Count < 2)
                            break;

                        List<PointD> points = new List<PointD>();
                        double projX = 0, projY = 0;
                        foreach (PointF aPoint in _graphicPoints)
                        {
                            ScreenToProj(ref projX, ref projY, aPoint.X, aPoint.Y);
                            points.Add(new PointD(projX, projY));
                        }

                        if (_mouseTool == MouseTools.New_Freehand)
                        {
                            PolylineShape aPLS = new PolylineShape();
                            aPLS.Points = points;
                            Graphic aGraphic = new Graphic(aPLS, (PolyLineBreak)_defPolylineBreak.Clone());

                            if (aGraphic != null)
                            {
                                _graphicCollection.Add(aGraphic);
                                PaintLayers();
                            }
                            else
                                this.Refresh();
                        }
                        else
                        {
                            MapLayer aMLayer = GetLayerFromHandle(SelectedLayer);
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
                            OnShapeSelected();
                        }
                    }
                    break;
                case MouseTools.New_Circle:
                case MouseTools.SelectFeatures_Circle:
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
                        List<PointD> points = new List<PointD>();
                        double projX = 0, projY = 0;
                        foreach (PointF aPoint in _graphicPoints)
                        {
                            ScreenToProj(ref projX, ref projY, aPoint.X, aPoint.Y);
                            points.Add(new PointD(projX, projY));
                        }

                        CircleShape aPGS = new CircleShape();
                        aPGS.SetPoints(points);
                        if (_mouseTool == MouseTools.New_Circle)
                        {
                            Graphic aGraphic = new Graphic(aPGS, (PolygonBreak)_defPolygonBreak.Clone());

                            if (aGraphic != null)
                            {
                                _graphicCollection.Add(aGraphic);
                                PaintLayers();
                            }
                            else
                                this.Refresh();
                        }
                        else
                        {
                            MapLayer layer = this.GetLayerFromHandle(_selectedLayer);
                            if (layer == null)
                                return;
                            if (layer.LayerType != LayerTypes.VectorLayer)
                                return;

                            VectorLayer aLayer = (VectorLayer)layer;
                            if (!((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                            {
                                aLayer.ClearSelectedShapes();
                            }
                            aLayer.SelectShapes(aPGS);
                            OnShapeSelected();
                        }
                    }
                    break;                
                case MouseTools.InEditingVertices:
                    double vX = 0, vY = 0;
                    ScreenToProj(ref vX, ref vY, e.X, e.Y);
                    _selectedGraphics.GraphicList[0].VerticeEditUpdate(_editingVerticeIndex, vX, vY);
                    
                    _mouseTool = MouseTools.EditVertices;
                    this.PaintLayers();
                    break;
            }
            //PaintLayers();
        }

        /// <summary>
        /// Override OnMouseWheel event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            double MinX, MaxX, MinY, MaxY, lonRan, latRan, ZoomF;
            double mouseLon, mouseLat;
            lonRan = _drawExtent.maxX - _drawExtent.minX;
            latRan = _drawExtent.maxY - _drawExtent.minY;
            //mouseLon = _drawExtent.minX + e.X / _scaleX;
            //mouseLat = _drawExtent.maxY - e.Y / _scaleY;
            mouseLon = _drawExtent.minX + lonRan / 2;
            mouseLat = _drawExtent.minY + latRan / 2;

            ZoomF = 1 - e.Delta / 1000.0;

            MinX = mouseLon - (lonRan / 2 * ZoomF);
            MaxX = mouseLon + (lonRan / 2 * ZoomF);
            MinY = mouseLat - (latRan / 2 * ZoomF);
            MaxY = mouseLat + (latRan / 2 * ZoomF);

            if (!_highSpeedWheelZoom)
                ZoomToExtent(MinX, MaxX, MinY, MaxY);
            else
            {
                Image dImage = new Bitmap(this.Width, this.Height);
                Graphics dg = Graphics.FromImage(dImage);
                //Graphics g = this.CreateGraphics();
                //g.Clear(Color.White);
                float nWidth = this.Width / (float)ZoomF;
                float nHeight = this.Height / (float)ZoomF;
                float nx = (this.Width - nWidth) / 2;
                float ny = (this.Height - nHeight) / 2;
                dg.DrawImage(_mapBitmap, new RectangleF(nx, ny, nWidth, nHeight));
                //g.DrawImage(_mapBitmap, new RectangleF(nx, ny, nWidth, nHeight));
                //g.DrawImageUnscaled(dImage, 0, 0);
                _mapBitmap = (Bitmap)dImage;
                //g.Dispose();
                dg.Dispose();
                this.Refresh();
                _viewExtent = new Extent(MinX, MaxX, MinY, MaxY);
                RefreshXYScale();

                this._lastMouseWheelTime = DateTime.Now;
                if (!this._mouseWheelDetctionTimer.Enabled)
                    this._mouseWheelDetctionTimer.Start();
            }
        }

        private void MouseWheelDetectionTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Subtract(_lastMouseWheelTime).TotalSeconds > 0.2)
            {
                this.PaintLayers();
                this._mouseWheelDetctionTimer.Stop();
            }
        }

        /// <summary>
        /// Override OnKeyDown event
        /// </summary>
        /// <param name="e">KeyEventArgs</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (_mouseTool == MouseTools.SelectElements || _mouseTool == MouseTools.CreateSelection)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    RemoveSelectedGraphics();
                    _startNewGraphic = true;
                    PaintLayers();
                }
            }
        }

        /// <summary>
        /// MapView resize event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //Set coordinate
            if (!_lockViewUpdate)
                ZoomToExtent(_viewExtent);
        }

        /// <summary>
        /// Fires the ViewExtentChanged event
        /// </summary>
        protected virtual void OnViewExtentChanged()
        {
            if (ViewExtentChanged != null) ViewExtentChanged(this, new EventArgs());

            if (!_lockViewUpdate)
            {
                //Add dynamic contour layer's labels
                for (int i = 0; i < _layerSet.Layers.Count; i++)
                {
                    if (_layerSet.Layers[i].LayerType == LayerTypes.VectorLayer)
                    {
                        VectorLayer aLayer = (VectorLayer)_layerSet.Layers[i];
                        if (aLayer.LabelSet.DynamicContourLabel)
                        {
                            aLayer.RemoveLabels();
                            aLayer.AddLabelsContourDynamic(_viewExtent);
                        }
                    }
                }                
            }            
        }

        /// <summary>
        /// Fires the LayersUpdated event
        /// </summary>
        protected virtual void OnLayersUpdated()
        {
            if (LayersUpdated != null) LayersUpdated(this, new EventArgs());
        }

        /// <summary>
        /// Fires the MapViewRedrawed event
        /// </summary>
        protected virtual void OnMapViewReDrawed()
        {
            if (MapViewRedrawed != null) MapViewRedrawed(this, new EventArgs());
        }

        /// <summary>
        /// Fires the RenderChanged event
        /// </summary>
        protected virtual void OnRenderChanged()
        {
            if (RenderChanged != null) RenderChanged(this, new EventArgs());
        }

        /// <summary>
        /// Fires the GraphicSelected event
        /// </summary>
        protected virtual void OnGraphicSelected()
        {
            if (GraphicSeleted != null) GraphicSeleted(this, new EventArgs());
        }

        /// <summary>
        /// Fires the ShapeSelected event
        /// </summary>
        protected virtual void OnShapeSelected()
        {
            this.PaintLayers();
            if (ShapeSelected != null) ShapeSelected(this, new EventArgs());            
        }

        /// <summary>
        /// Raise ShapeSelected event
        /// </summary>
        public void RaiseShapeSelectedEvent()
        {
            OnShapeSelected();
        }

        /// <summary>
        /// Raise LayersUpdated event
        /// </summary>
        public void RaiseLayersUndateEvent()
        {
            OnLayersUpdated();
        }

        /// <summary>
        /// Raise view extent changed event
        /// </summary>
        public void RaiseViewExtentChangedEvent()
        {
            if (ViewExtentChanged != null) ViewExtentChanged(this, new EventArgs());
        }

        /// <summary>
        /// Fires the ProjectionChanged event
        /// </summary>
        protected virtual void OnProjectionChanged()
        {
            if (ProjectionChanged != null) ProjectionChanged(this, new EventArgs());
        }

        /// <summary>
        /// Raise ProjectionChanged event
        /// </summary>
        public void RaiseProjectionChangedEvent()
        {
            OnProjectionChanged();
        }

        private void FrmMeasurementClosed(object sender, FormClosedEventArgs e)
        {
            this.Refresh();
        }

        #endregion

    }
}
