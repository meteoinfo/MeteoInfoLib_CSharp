using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using MeteoInfoC.Map;
using MeteoInfoC.Layer;
using MeteoInfoC.Global;
using MeteoInfoC.Shape;
using MeteoInfoC.Projections;
using MeteoInfoC.Drawing;
using MeteoInfoC.Data.MapData;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Grid lable position enum
    /// </summary>
    public enum GridLabelPosition
    {
        /// <summary>
        /// Left and bottom
        /// </summary>
        LeftBottom,
        /// <summary>
        /// Left and up
        /// </summary>
        LeftUp,
        /// <summary>
        /// Right and bottom
        /// </summary>
        RightBottom,
        /// <summary>
        /// Right and up
        /// </summary>
        RightUp,
        /// <summary>
        /// All of four directions
        /// </summary>
        All
    }

    /// <summary>
    /// Map frame
    /// </summary>
    public class MapFrame:ItemNode
    {
        #region Events definitation
        /// <summary>
        /// Occurs after layers updated. Including expended status changed.
        /// </summary>
        public event EventHandler LayersUpdated;
        /// <summary>
        /// Occurs after layout bounds changed
        /// </summary>
        public event EventHandler LayoutBoundsChanged;
        /// <summary>
        /// Occurs after map view updated
        /// </summary>
        public event EventHandler MapViewUpdated;

        #endregion

        #region Variables
        private MapView _mapView = new MapView();
        private List<ItemNode> _nodes = new List<ItemNode>();
        //private int _selectedLayerHandle;
        private LayersLegend _legend;
        private bool _active = false;
        private int _order;

        private bool _drawNeatLine = true;
        private Color _neatLineColor = Color.Black;
        private int _neatLineSize = 1;
        private bool _drawGridLabel = true;
        private bool _drawGridTickLine = true;
        private bool _drawDegreeSymbol = false;
        private bool _insideTickLine = false;
        private int _tickLineLength = 5;
        private int _gridLabelShift = 2;
        private GridLabelPosition _gridLabelPosition = GridLabelPosition.LeftBottom;
        private Font _gridFont = new Font("Arial", 8);
        private Rectangle _layoutBounds;
        private bool _isFireMapViewUpdate = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MapFrame()
        {
            Text = "New Map Frame";
            NodeType = NodeTypes.MapFrameNode;
            Expand();
            LayoutBounds = new Rectangle(100, 100, 300, 200);

            _mapView.ViewExtentChanged += MapViewViewExtentChanged;
            _mapView.LayersUpdated += MapViewLayersUpdated;
            _mapView.MapViewRedrawed += MapViewRedrawed;
            _mapView.ProjectionChanged += new EventHandler(MapViewProjectionChanged);
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set map view
        /// </summary>
        public MapView MapView
        {
            get { return _mapView; }
            set 
            { 
                _mapView = value;
                _mapView.ViewExtentChanged += MapViewViewExtentChanged;
                _mapView.LayersUpdated += MapViewLayersUpdated;
                _mapView.MapViewRedrawed += MapViewRedrawed;
                _mapView.ProjectionChanged += new EventHandler(MapViewProjectionChanged);
                if (_mapView.Projection.IsLonLatMap)
                    _gridLabelPosition = GridLabelPosition.LeftBottom;
                else
                    _gridLabelPosition = GridLabelPosition.All;
            }
        }

        /// <summary>
        /// Get or set nodes
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<ItemNode> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        /// <summary>
        /// Get or set selected layer handle
        /// </summary>
        public int SelectedLayer
        {
            get { return _mapView.SelectedLayer; }
            set { _mapView.SelectedLayer = value; }
        }

        /// <summary>
        /// Get or set layers legend
        /// </summary>
        public LayersLegend Legend
        {
            get { return _legend; }
            set { _legend = value; }
        }

        /// <summary>
        /// Get or set active
        /// </summary>
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        /// <summary>
        /// Get or set z order
        /// </summary>
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        /// <summary>
        /// Get or set map view neat line color
        /// </summary>
        [CategoryAttribute("NeatLine"), DescriptionAttribute("Set if draw map view neat line")]
        public bool DrawNeatLine
        {
            get { return _drawNeatLine; }
            set { _drawNeatLine = value; }
        }

        /// <summary>
        /// Get or set map view neat line color
        /// </summary>
        [CategoryAttribute("NeatLine"), DescriptionAttribute("Set map view neat line color")]
        public Color NeatLineColor
        {
            get { return _neatLineColor; }
            set { _neatLineColor = value; }
        }

        /// <summary>
        /// Get or set map view neat line size
        /// </summary>
        [CategoryAttribute("NeatLine"), DescriptionAttribute("Set map view neat line size")]
        public int NeatLineSize
        {
            get { return _neatLineSize; }
            set { _neatLineSize = value; }
        }

        /// <summary>
        /// Get or set if draw grid labels
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set if draw grid labels")]
        public bool DrawGridLabel
        {
            get { return _drawGridLabel; }
            set { _drawGridLabel = value; }
        }

        /// <summary>
        /// Get or set if draw tick line inside
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set if draw grid tick line inside")]
        public bool InsideTickLine
        {
            get { return _insideTickLine; }
            set { _insideTickLine = value ;}
        }

        /// <summary>
        /// Get or set grid tick line length
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid tick line length")]
        public int TickLineLength
        {
            get { return _tickLineLength; }
            set { _tickLineLength = value; }
        }

        /// <summary>
        /// Get or set grid label shift
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid label shift")]
        public int GridLabelShift
        {
            get { return _gridLabelShift; }
            set { _gridLabelShift = value; }
        }

        /// <summary>
        /// Get or set grid label position
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid label position")]
        public GridLabelPosition GridLabelPosition
        {
            get { return _gridLabelPosition; }
            set { _gridLabelPosition = value; }
        }

        /// <summary>
        /// Get or set grid lable font
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid lable font")]
        public Font GridFont
        {
            get { return _gridFont; }
            set { _gridFont = value; }
        }

        /// <summary>
        /// Get or set map frame name
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set map frame name")]
        public string MapFrameName
        {
            get { return Text; }
            set { Text = value; }
        }

        /// <summary>
        /// Get or set map view back color
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set map view back color")]
        public new Color BackColor
        {
            get { return _mapView.BackColor; }
            set { _mapView.BackColor = value; }
        }

        /// <summary>
        /// Get or set map view fore color
        /// </summary>
        [CategoryAttribute("General"), DescriptionAttribute("Set map view fore color")]
        public new Color ForeColor
        {
            get { return _mapView.ForeColor; }
            set { _mapView.ForeColor = value; }
        }

        /// <summary>
        /// Get or set gird line color
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid line color")]
        public Color GridLineColor
        {
            get { return _mapView.GridLineColor; }
            set { _mapView.GridLineColor = value; }
        }

        /// <summary>
        /// Get or set grid line size
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid line size")]
        public int GridLineSize
        {
            get { return _mapView.GridLineSize; }
            set { _mapView.GridLineSize = value; }
        }

        /// <summary>
        /// Get or set grid line style
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid line style")]
        public DashStyle GridLineStyle
        {
            get { return _mapView.GridLineStyle; }
            set { _mapView.GridLineStyle = value; }
        }

        /// <summary>
        /// Get or set if draw grid line
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set if draw grid line")]
        public bool DrawGridLine
        {
            get { return _mapView.DrawGridLine; }
            set { _mapView.DrawGridLine = value; }
        }

        /// <summary>
        /// Get or set if draw grid line
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set if draw grid tick line")]
        public bool DrawGridTickLine
        {
            get { return _drawGridTickLine; }
            set { _drawGridTickLine = value; }
        }

        /// <summary>
        /// Get or set if draw degree symbol
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set if draw degree symbol")]
        public bool DrawDegreeSymbol
        {
            get { return _drawDegreeSymbol; }
            set { _drawDegreeSymbol = value; }
        }

        /// <summary>
        /// Get or set grid x/longitude delt
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid x/longitude delt")]
        public double GridXDelt
        {
            get { return _mapView.GridXDelt; }
            set { _mapView.GridXDelt = value; }
        }

        /// <summary>
        /// Get or set grid y/latitude delt
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid y/latitude delt")]
        public double GridYDelt
        {
            get { return _mapView.GridYDelt; }
            set { _mapView.GridYDelt = value; }
        }

        /// <summary>
        /// Get or set grid x/longitude delt
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid x/longitude origin")]
        public double GridXOrigin
        {
            get { return _mapView.GridXOrigin; }
            set { _mapView.GridXOrigin = value; }
        }

        /// <summary>
        /// Get or set grid y/latitude delt
        /// </summary>
        [CategoryAttribute("Grid"), DescriptionAttribute("Set grid y/latitude origin")]
        public double GridYOrigin
        {
            get { return _mapView.GridYOrigin; }
            set { _mapView.GridYOrigin = value; }
        }

        /// <summary>
        /// Get or set layout bounds
        /// </summary>
        public Rectangle LayoutBounds
        {
            get { return _layoutBounds; }
            set 
            { 
                _layoutBounds = value;
                OnLayoutBoundsChanged();
            }
        }

        /// <summary>
        /// Get or set if fire MapViewUpdate event
        /// </summary>
        public bool IsFireMapViewUpdate
        {
            get { return _isFireMapViewUpdate; }
            set { _isFireMapViewUpdate = value; }
        }

        #endregion

        #region Methods
        #region Group
        /// <summary>
        /// Add a new group
        /// </summary>
        /// <param name="name">group name</param>
        /// <returns>group handle</returns>
        public int AddNewGroup(string name)
        {
            GroupNode aGroup = new GroupNode(name);
            return (AddGroup(aGroup));
        }

        /// <summary>
        /// Add group
        /// </summary>
        /// <param name="aGroup">group node</param>
        /// <returns>group handle</returns>
        public int AddGroup(GroupNode aGroup)
        {
            //aGroup.SetParentLegend(this);
            AddNode(aGroup);
            //_Nodes.Add(aGroup);

            return aGroup.GroupHandle;
        }

        /// <summary>
        /// Delete group
        /// </summary>
        /// <param name="aGroup">group node</param>
        public void RemoveGroup(GroupNode aGroup)
        {
            MapFrame mapFrame = aGroup.MapFrame;
            if (aGroup.Layers.Count > 0)
            {
                if (MessageBox.Show("All layers of the group will be removed! Will you continue?", "Confirm",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int lNum = aGroup.Layers.Count;
                    for (int i = 0; i < lNum; i++)
                    {
                        LayerNode aLN = aGroup.Layers[0];
                        //RemoveLayer(aLN.LayerHandle);
                        RemoveLayer(aLN);
                        //_Nodes.Remove(aLN);
                    }
                    mapFrame.RemoveNode(aGroup);
                }
            }
            else
            {
                mapFrame.RemoveNode(aGroup);
                //_Groups.Remove(aGroup);
            }
        }

        #endregion

        #region Layer Position Operation
        /// <summary>
        /// Open a layer from layer file
        /// </summary>
        /// <param name="aFile">file path</param>
        /// <returns>map layer</returns>
        public MapLayer OpenLayer(string aFile)
        {
            MapLayer aLayer = MapDataManage.OpenLayer(aFile);
            if (aLayer.LayerType == LayerTypes.VectorLayer)
            {
                if (aLayer.ShapeType == ShapeTypes.Polygon)
                {
                    InsertPolygonLayer((VectorLayer)aLayer);
                }
                else if (aLayer.ShapeType == ShapeTypes.Polyline)
                {
                    InsertPolylineLayer((VectorLayer)aLayer);
                }
                else
                {
                    AddLayer(aLayer);
                }
            }
            else
                InsertImageLayer((ImageLayer)aLayer);

            return aLayer;
        }

        /// <summary>
        /// Add layer node
        /// </summary>
        /// <param name="aLN">a layer node</param>
        /// <returns>layer handle</returns>
        public int AddLayerNode(LayerNode aLN)
        {
            aLN.MapFrame = this;
            int handle = _mapView.AddLayer(aLN.MapLayer);

            //aLN.MapLayer = _mapView.GetLayerFromHandle(handle);
            AddNode(aLN);
            if (aLN.MapLayer.Visible)
            {
                aLN.Checked = true;
            }
            aLN.UpdateLegendScheme(aLN.LegendScheme);
            SelectLayer(aLN);
            if (aLN.MapLayer.Expanded)
                aLN.Expand();
            
            OnLayersUpdated();

            return handle;
        }

        /// <summary>
        /// Add layer node
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="aLN">a layer node</param>
        /// <returns>layer handle</returns>
        public int InsertLayerNode(int index, LayerNode aLN)
        {
            aLN.MapFrame = this;
            int handle = _mapView.AddLayer(aLN.MapLayer);

            //aLN.MapLayer = _mapView.GetLayerFromHandle(handle);
            InsertNode(index, aLN);
            if (aLN.MapLayer.Visible)
            {
                aLN.Checked = true;
            }
            aLN.UpdateLegendScheme(aLN.LegendScheme);
            SelectLayer(aLN);
            if (aLN.MapLayer.Expanded)
                aLN.Expand();

            ReOrderMapViewLayers();
            OnLayersUpdated();

            return handle;
        }

        /// <summary>
        /// Add a layer node in a group node
        /// </summary>
        /// <param name="aLN">a layer node</param>
        /// <param name="aGN">a group node</param>
        /// <returns>layer handle</returns>
        public int AddLayerNode(LayerNode aLN, GroupNode aGN)
        {
            aLN.MapFrame = this;
            int handle = _mapView.AddLayer(aLN.MapLayer);

            //aLN.MapLayer = _mapView.GetLayerFromHandle(handle);
            aGN.AddLayer(aLN);
            if (aLN.MapLayer.Visible)
            {
                aLN.Checked = true;
            }
            aLN.UpdateLegendScheme(aLN.LegendScheme);
            SelectLayer(aLN);
            if (aLN.MapLayer.Expanded)
                aLN.Expand();

            ReOrderMapViewLayers();
            OnLayersUpdated();

            return handle;
        }

        /// <summary>
        /// Add a layer node in a group node
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="aLN">a layer node</param>
        /// <param name="aGN">a group node</param>
        /// <returns>layer handle</returns>
        public int InsertLayerNode(int index, LayerNode aLN, GroupNode aGN)
        {
            aLN.MapFrame = this;
            int handle = _mapView.AddLayer(aLN.MapLayer);

            aLN.MapLayer = _mapView.GetLayerFromHandle(handle);
            aGN.InsertLayer(aLN, index);
            if (aLN.MapLayer.Visible)
            {
                aLN.Checked = true;
            }
            aLN.UpdateLegendScheme(aLN.LegendScheme);
            SelectLayer(aLN);
            if (aLN.MapLayer.Expanded)
                aLN.Expand();

            ReOrderMapViewLayers();
            OnLayersUpdated();

            return handle;
        }

        /// <summary>
        /// Add a group node
        /// </summary>
        /// <param name="aGN">a group node</param>
        public void AddGroupNode(GroupNode aGN)
        {
            AddNode(aGN);
            foreach (LayerNode aLN in aGN.Layers)
            {
                _mapView.AddLayer(aLN.MapLayer);
            }

            ReOrderMapViewLayers();
            OnLayersUpdated();
        }

        /// <summary>
        /// Insert a group node
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="aGN">a group node</param>
        public void InsertGroupNode(int index, GroupNode aGN)
        {
            InsertNode(index, aGN);
            foreach (LayerNode aLN in aGN.Layers)
            {
                _mapView.AddLayer(aLN.MapLayer);
            }

            ReOrderMapViewLayers();
            OnLayersUpdated();
        }

        private int AddLayerNode(LayerNode aLN, bool earthWind)
        {
            aLN.MapFrame = this;
            int handle = _mapView.AddWindLayer((VectorLayer)aLN.MapLayer, earthWind);

            AddNode(aLN);
            if (aLN.MapLayer.Visible)
            {
                aLN.Checked = true;
            }
            aLN.UpdateLegendScheme(aLN.LegendScheme);
            SelectLayer(aLN);
            if (aLN.MapLayer.Expanded)
                aLN.Expand();

            OnLayersUpdated();

            return handle;
        }

        /// <summary>
        /// Add vector layer
        /// </summary>
        /// <param name="aLayer">Vector layer</param>        
        /// <returns>layer handle</returns>
        public int AddLayer(MapLayer aLayer)
        {
            LayerNode aLN = new LayerNode(aLayer);
            return AddLayerNode(aLN);
        }

        /// <summary>
        /// Add vector layer
        /// </summary>
        /// <param name="aLayer">Vector layer</param>       
        /// <param name="earthWind">if wind relative to earth</param>
        /// <returns>layer handle</returns>
        public int AddWindLayer(VectorLayer aLayer, bool earthWind)
        {
            LayerNode aLN = new LayerNode(aLayer);
            return AddLayerNode(aLN, earthWind);
        }

        /// <summary>
        /// Add vector layer
        /// </summary>
        /// <param name="aLayer">Vector layer</param>
        /// <param name="groupHandle">group handle</param>
        /// <returns>layer handle</returns>
        public int AddLayer(MapLayer aLayer, int groupHandle)
        {
            LayerNode aLN = new LayerNode(aLayer);
            GroupNode aGroup = GetGroupByHandle(groupHandle);

            if (aGroup == null)
            {
                return AddLayerNode(aLN);
            }
            else
            {
                return AddLayerNode(aLN, aGroup);
            }
        }

        /// <summary>
        /// Re order map view layers
        /// </summary>
        public void ReOrderMapViewLayers()
        {
            for (int i = _nodes.Count - 1; i >= 0; i--)
            {
                ItemNode aTN = _nodes[i];
                if (aTN.GetType() == typeof(LayerNode))
                {
                    for (int j = 0; j < _mapView.LayerSet.LayerNum; j++)
                    {
                        if (_mapView.LayerSet.Layers[j].Handle == ((LayerNode)aTN).LayerHandle)
                        {
                            if (j > 0)
                                _mapView.MoveLayer(j, 0);
                            break;
                        }
                    }
                }
                else
                {
                    for (int l = ((GroupNode)aTN).Layers.Count - 1; l >= 0; l--)
                    {
                        LayerNode aLN = ((GroupNode)aTN).Layers[l];
                        for (int j = 0; j < _mapView.LayerSet.LayerNum; j++)
                        {
                            if (_mapView.LayerSet.Layers[j].Handle == aLN.LayerHandle)
                            {
                                if (j > 0)
                                    _mapView.MoveLayer(j, 0);
                                break;
                            }
                        }
                    }
                }
            }

            //SelectLayerByHandle(SelectedLayer);
            //_mapView.ZoomToExtent(_mapView.ViewExtent);
            _mapView.PaintLayers();
        }

        /// <summary>
        /// Move layer position
        /// </summary>
        /// <param name="handle">Layer handle</param>
        /// <param name="lNewIdx">Move to index</param>
        public void MoveLayer(int handle, int lNewIdx)
        {
            int lPreIdx = _mapView.GetLayerIdxFromHandle(handle);

            if (lPreIdx == lNewIdx)
            {
                return;
            }
            MoveLayerNode(lPreIdx, lNewIdx);
            _mapView.MoveLayer(lPreIdx, lNewIdx);

            UpdateMapView();

            OnLayersUpdated();
        }

        /// <summary>
        /// Move layer position
        /// </summary>
        /// <param name="aLayer">a layer</param>
        /// <param name="lNewIdx">Move to index</param>
        public void MoveLayer(MapLayer aLayer, int lNewIdx)
        {
            MoveLayer(aLayer.Handle, lNewIdx);
        }

        /// <summary>
        /// Move layer to top
        /// </summary>
        /// <param name="layer">The layer</param>
        public void MoveLayerToTop(MapLayer layer)
        {
            MoveLayer(layer, _mapView.LayerSet.LayerNum - 1);
        }

        /// <summary>
        /// Move layer to bottom
        /// </summary>
        /// <param name="layer">The layer</param>
        public void MoveLayerToBottom(MapLayer layer)
        {
            MoveLayer(layer, 0);
        }

        private void UpdateMapView()
        {
            _mapView.PaintLayers();
        }

        /// <summary>
        /// Remove layer by index
        /// </summary>
        /// <param name="lIdx">layer index</param>
        public void RemoveLayer(int lIdx)
        {
            MapLayer aLayer = _mapView.LayerSet.Layers[lIdx];
            RemoveLayer(aLayer);            
        }

        /// <summary>
        /// Remove layer by handle
        /// </summary>
        /// <param name="handle">handle</param>
        public void RemoveLayerByHandle(int handle)
        {
            int lIdx = _mapView.GetLayerIdxFromHandle(handle);
            if (lIdx > -1)
            {
                LayerNode aLN = GetLayerNodeByHandle(handle);
                if (aLN == null)
                {
                    _mapView.RemoveLayer(lIdx);
                    return;
                }

                if (aLN.GroupHandle >= 0)
                {
                    GroupNode gNode = GetGroupByHandle(aLN.GroupHandle);
                    gNode.RemoveLayer(aLN);
                }
                else
                    RemoveNode(aLN);

                _mapView.RemoveLayer(lIdx);
                if (lIdx > 0)
                {
                    int newHandle = _mapView.GetLayerHandleFromIdx(lIdx - 1);
                    SelectLayerByHandle(newHandle);
                }

                //this.Invalidate();
                _mapView.PaintLayers();

                OnLayersUpdated();
            }
        }

        /// <summary>
        /// Remove layer by handle
        /// </summary>
        /// <param name="aLayer">a layer</param>
        public void RemoveLayer(MapLayer aLayer)
        {
            int handle = aLayer.Handle;
            RemoveLayerByHandle(handle);            
        }

        /// <summary>
        /// Remove a layer node
        /// </summary>
        /// <param name="aLN">a layer node</param>
        public void RemoveLayer(LayerNode aLN)
        {
            _mapView.RemoveLayerHandle(aLN.LayerHandle);
            if (aLN.GroupHandle >= 0)
            {
                GroupNode gNode = GetGroupByHandle(aLN.GroupHandle);
                gNode.RemoveLayer(aLN);
            }
            else
                RemoveNode(aLN);

            //this.Invalidate();
            _mapView.PaintLayers();
            OnLayersUpdated();
        }

        /// <summary>
        /// Remove meteorological data layers
        /// </summary>
        public void RemoveMeteoLayers()
        {
            for (int i = 0; i < _mapView.LayerSet.Layers.Count; i++)
            {
                if (i == _mapView.LayerSet.Layers.Count)
                {
                    break;
                }
                MapLayer aLayer = _mapView.LayerSet.Layers[i];
                if (aLayer.FileName == string.Empty)
                {
                    RemoveLayer(aLayer);
                    i -= 1;
                }
            }
        }

        /// <summary>
        /// Remove all layers
        /// </summary>
        public void RemoveAllLayers()
        {
            int lNum = _mapView.LayerSet.LayerNum;
            for (int i = 0; i < lNum; i++)
            {
                RemoveLayer(0);
            }
        }

        private int GetNodeIdx(string aStr)
        {
            int aIdx = -1;
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].Text == aStr)
                {
                    aIdx = i;
                    break;
                }
            }

            return aIdx;
        }

        /// <summary>
        /// Insert polygon layer
        /// </summary>
        /// <param name="aLayer">vector layer</param>        
        /// <returns>layer handle</returns>
        public int InsertPolygonLayer(MapLayer aLayer)
        {
            _mapView.LockViewUpdate = true;
            int lIdx = _mapView.GetPolygonLayerIdx() + 1;
            int handle = AddLayer(aLayer);
            if (lIdx < 0)
            {
                lIdx = 0;
            }
            MoveLayer(handle, lIdx);
            _mapView.LockViewUpdate = false;
            _mapView.PaintLayers();

            return handle;
        }

        /// <summary>
        /// Insert polyline layer
        /// </summary>
        /// <param name="aLayer">vector layer</param>
        /// <returns>layer handle</returns>
        public int InsertPolylineLayer(VectorLayer aLayer)
        {
            _mapView.LockViewUpdate = true;
            int lIdx = _mapView.GetLineLayerIdx() + 1;
            int handle = AddLayer(aLayer);
            if (lIdx < 0)
            {
                lIdx = 0;
            }
            MoveLayer(handle, lIdx);
            _mapView.LockViewUpdate = false;
            _mapView.PaintLayers();

            return handle;
        }

        /// <summary>
        /// Insert image layer
        /// </summary>
        /// <param name="aLayer">vector layer</param>        
        /// <returns>layer handle</returns>
        public int InsertImageLayer(MapLayer aLayer)
        {
            _mapView.LockViewUpdate = true;
            int lIdx = _mapView.GetImageLayerIdx() + 1;
            int handle = AddLayer(aLayer);
            if (lIdx < 0)
            {
                lIdx = 0;
            }
            MoveLayer(handle, lIdx);
            _mapView.LockViewUpdate = false;
            _mapView.PaintLayers();

            return handle;
        }

        private int IndexInverse(int nIdx)
        {
            int lIdx;
            lIdx = Math.Max(_mapView.LayerSet.LayerNum, Nodes.Count) - nIdx - 1;

            return lIdx;
        }

        /// <summary>
        /// Select layer by handle
        /// </summary>
        /// <param name="handle">layer handle</param>
        public void SelectLayerByHandle(int handle)
        {
            LayerNode aLN = GetLayerNodeByHandle(handle);

            SelectLayer(aLN);
        }

        private void SelectLayer(LayerNode aLN)
        {
            if (Nodes.Count > 1)
            {
                foreach (ItemNode aNode in Nodes)
                {
                    aNode.BackColor = Color.White;
                    aNode.ForeColor = Color.Black;
                    if (aNode.GetType() == typeof(GroupNode))
                    {
                        foreach (LayerNode bNode in ((GroupNode)aNode).Layers)
                        {
                            bNode.BackColor = Color.White;
                            bNode.ForeColor = Color.Black;
                        }
                    }
                }
            }

            SelectedLayer = aLN.LayerHandle;
            _mapView.SelectedLayer = SelectedLayer;

            //aLN.BackColor = _selectedBackColor;
            //aLN.ForeColor = _selectedForeColor;

            //this.Refresh();
        }

        /// <summary>
        /// Unselect all nodes
        /// </summary>
        public void UnSelectNodes()
        {
            foreach (ItemNode aNode in _nodes)
            {
                aNode.Selected = false;
                if (aNode.NodeType == NodeTypes.GroupNode)
                {
                    foreach (LayerNode lNode in ((GroupNode)aNode).Layers)
                        lNode.Selected = false;
                }
            }
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
            int lIdx = _mapView.GetLayerIdxFromHandle(handle);
            _mapView.LayerSet.Layers[lIdx].LegendScheme = aLS;
            //if (!_mapView.Projection.IsLonLatMap)
            //{
            //    _mapView.LayerSet.GeoLayers[lIdx].LegendScheme = aLS;
            //}
            LayerNode aLN = GetLayerNodeByHandle(handle);
            aLN.UpdateLegendScheme(aLS);
            //this.Refresh();
            this._mapView.PaintLayers();
        }

        /// <summary>
        /// Set projected layer legend scheme
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="aLS"></param>
        public void SetLayerLegendSchemeProj(int handle, LegendScheme aLS)
        {
            int lIdx = _mapView.GetLayerIdxFromHandle(handle);
            _mapView.LayerSet.Layers[lIdx].LegendScheme = aLS;
            //if (!_mapView.Projection.IsLonLatMap)
            //{
            //    _mapView.LayerSet.GeoLayers[lIdx].LegendScheme = aLS;
            //}
            LayerNode aLN = GetLayerNodeByHandle(handle);
            aLN.UpdateLegendScheme(aLS);
            //this.Refresh();
            this._mapView.PaintLayers();
        }

        /// <summary>
        /// Set layer name
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        public void SetLayerName(int handle, string name)
        {
            MapLayer aLayer = _mapView.GetLayerFromHandle(handle);
            aLayer.LayerName = name;

            LayerNode aLN = GetLayerNodeByHandle(handle);
            aLN.Text = name;
            //this.Refresh();
        }

        /// <summary>
        /// Set layer transparency
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="transPerc"></param>
        public void SetLayerTransparency(int handle, int transPerc)
        {
            MapLayer aLayer = _mapView.GetLayerFromHandle(handle);

            aLayer.TransparencyPerc = transPerc;
            //this.Refresh();
            this._mapView.PaintLayers();
        }

        /// <summary>
        /// Set image layer if set a transparency color
        /// </summary>
        /// <param name="handle">layer handle</param>
        /// <param name="setTransColor">if set transparency color</param>
        public void SetImageLayerSetTransparencyColor(int handle, bool setTransColor)
        {
            ImageLayer aLayer = (ImageLayer)_mapView.GetLayerFromHandle(handle);
            aLayer.SetTransColor = setTransColor;
            //this.Refresh();
            this._mapView.PaintLayers();
        }

        /// <summary>
        /// Set image layer transparency color
        /// </summary>
        /// <param name="handle">layer handle</param>
        /// <param name="transColor">transparency color</param>
        public void SetImageLayerTransparencyColor(int handle, Color transColor)
        {
            ImageLayer aLayer = (ImageLayer)_mapView.GetLayerFromHandle(handle);
            aLayer.TransparencyColor = transColor;
            //this.Refresh();
            this._mapView.PaintLayers();
        }

        /// <summary>
        /// Set layer visible
        /// </summary>
        /// <param name="handle">layer handle</param>
        /// <param name="visible">is visible</param>
        public void SetLayerVisible(int handle, Boolean visible)
        {
            MapLayer aLayer = _mapView.GetLayerFromHandle(handle);
            aLayer.Visible = visible;

            this._mapView.PaintLayers();
            LayerNode aLN = GetLayerNodeByHandle(handle);
            aLN.Checked = visible;
            //this.Refresh();
        }

        /// <summary>
        /// Set layer visible
        /// </summary>
        /// <param name="aLayer">a layer</param>
        /// <param name="visible">is visible</param>
        public void SetLayerVisible(MapLayer aLayer, Boolean visible)
        {
            int handle;
            aLayer.Visible = visible;
            handle = aLayer.Handle;

            this._mapView.PaintLayers();
            LayerNode aLN = GetLayerNodeByHandle(handle);
            aLN.Checked = visible;
            //this.Refresh();
        }

        /// <summary>
        /// Set layer expanded
        /// </summary>
        /// <param name="aLayer">a layer</param>
        /// <param name="expanded">is expanded</param>
        public void SetLayerExpanded(VectorLayer aLayer, bool expanded)
        {
            int handle = aLayer.Handle;
            LayerNode aLN = GetLayerNodeByHandle(handle);
            if (aLN.IsExpanded && !expanded)
                aLN.Collapse();
            else if (!aLN.IsExpanded && expanded)
                aLN.Expand();
            //this.Refresh();
        }

        /// <summary>
        /// Set layer maskout
        /// </summary>
        /// <param name="handle">layer handle</param>
        /// <param name="isMaskout">is maskout</param>
        public void SetLayerIsMaskout(int handle, Boolean isMaskout)
        {
            MapLayer aLayer = _mapView.GetLayerFromHandle(handle);
            aLayer.IsMaskout = isMaskout;

            this._mapView.PaintLayers();
        }

        /// <summary>
        /// Set layer if show value
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="showValue"></param>
        public void SetLayerShowValue(int handle, Boolean showValue)
        {
            VectorLayer aLayer = (VectorLayer)_mapView.GetLayerFromHandle(handle);
            this._mapView.PaintLayers();
        }

        /// <summary>
        /// Set layer avoid collision
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="avoidCollision"></param>
        public void SetLayerAvoidCollision(int handle, bool avoidCollision)
        {
            VectorLayer aLayer = (VectorLayer)_mapView.GetLayerFromHandle(handle);
            aLayer.AvoidCollision = avoidCollision;
            this._mapView.PaintLayers();
        }

        /// <summary>
        /// Set image layer extent
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="aExtent"></param>
        /// <param name="aWFP"></param>
        public void SetImageLayerExtent(int handle, Extent aExtent, WorldFilePara aWFP)
        {
            ImageLayer aILayer = (ImageLayer)_mapView.GetLayerFromHandle(handle);
            aILayer.Extent = aExtent;
            aILayer.WorldFileParaV = aWFP;
            if (File.Exists(aILayer.WorldFileName))
                aILayer.WriteImageWorldFile(aILayer.WorldFileName, aWFP);
            this._mapView.PaintLayers();
        }

        ///// <summary>
        ///// Re draw
        ///// </summary>
        //public void ReDraw()
        //{
        //    _mapView.LockViewUpdate = true;

        //    foreach (ItemNode aTN in this.Nodes)
        //    {
        //        if (aTN.GetType() == typeof(LayerNode))
        //        {
        //            int aHnd = ((LayerNode)aTN).LayerHandle;
        //            MapLayer aLayer = _mapView.GetLayerFromHandle(aHnd);
        //            aTN.Checked = aLayer.Visible;
        //            if (aTN.IsExpanded && !aLayer.Expanded)
        //                aTN.Collapse();
        //            else if (!aTN.IsExpanded && aLayer.Expanded)
        //                aTN.Expand();
        //        }
        //    }
        //    this.Refresh();

        //    _mapView.LockViewUpdate = false;
        //    _mapView.PaintLayers();
        //}

        # endregion

        #region Nodes

        /// <summary>
        /// Add a node
        /// </summary>
        /// <param name="aNode">node</param>
        public void AddNode(ItemNode aNode)
        {
            if (aNode.GetType() == typeof(GroupNode))
            {
                ((GroupNode)aNode).GroupHandle = GetNewGroupHandle();
                ((GroupNode)aNode).MapFrame = this;
            }
            else if (aNode.GetType() == typeof(LayerNode))
                ((LayerNode)aNode).MapFrame = this;

            _nodes.Add(aNode);
        }

        /// <summary>
        /// Insert a node
        /// </summary>
        /// <param name="idx">index</param>
        /// <param name="aNode">node</param>
        public void InsertNode(int idx, ItemNode aNode)
        {
            if (idx == -1)
                return;

            if (aNode.GetType() == typeof(GroupNode))
            {
                ((GroupNode)aNode).GroupHandle = GetNewGroupHandle();
                ((GroupNode)aNode).MapFrame = this;
            }
            else if (aNode.GetType() == typeof(LayerNode))
                ((LayerNode)aNode).MapFrame = this;

            _nodes.Insert(idx, aNode);
        }

        /// <summary>
        /// Remove a node
        /// </summary>
        /// <param name="aNode">node</param>
        public void RemoveNode(ItemNode aNode)
        {
            _nodes.Remove(aNode);
        }

        /// <summary>
        /// Override GetExpandedHeight method
        /// </summary>
        /// <returns>expanded height</returns>
        public override int GetExpandedHeight()
        {
            int height = Height;            
            foreach (ItemNode aNode in _nodes)
            {
                int lnHeight;
                if (aNode.IsExpanded)
                    lnHeight = aNode.GetExpandedHeight();
                else
                    lnHeight = aNode.Height;

                height += lnHeight + Constants.ITEM_PAD;
            }

            return height;
        }

        /// <summary>
        /// Override GetDrawHeight methods
        /// </summary>
        /// <returns>draw height</returns>
        public override int GetDrawHeight()
        {
            if (IsExpanded)
                return GetExpandedHeight();
            else
                return Height;
        }

        /// <summary>
        /// Get group node by handle
        /// </summary>
        /// <param name="handle">handle</param>
        /// <returns>group node</returns>
        public GroupNode GetGroupByHandle(int handle)
        {
            GroupNode aGroup = null;
            foreach (ItemNode aNode in _nodes)
            {
                if (aNode.GetType() == typeof(GroupNode))
                {
                    if (((GroupNode)aNode).GroupHandle == handle)
                    {
                        aGroup = (GroupNode)aNode;
                        break;
                    }
                }
            }

            return aGroup;
        }

        /// <summary>
        /// Get group node by name
        /// </summary>
        /// <param name="name">group name</param>
        /// <returns>group node</returns>
        public GroupNode GetGroupByName(string name)
        {
            GroupNode aGroup = null;
            foreach (ItemNode aTN in _nodes)
            {
                if (aTN.GetType() == typeof(GroupNode))
                {
                    if (((GroupNode)aTN).Text == name)
                    {
                        aGroup = (GroupNode)aTN;
                        break;
                    }
                }
            }

            return aGroup;
        }

        /// <summary>
        /// Get group list
        /// </summary>
        /// <returns>group list</returns>
        public List<GroupNode> GetGroups()
        {
            List<GroupNode> groupNodeList = new List<GroupNode>();
            foreach (ItemNode aTN in _nodes)
            {
                if (aTN.GetType() == typeof(GroupNode))
                    groupNodeList.Add((GroupNode)aTN);
            }

            return groupNodeList;
        }

        /// <summary>
        /// Get new group handle
        /// </summary>
        /// <returns>handle</returns>
        private int GetNewGroupHandle()
        {
            int handle = 0;
            foreach (ItemNode aTN in _nodes)
            {
                if (aTN.GetType() == typeof(GroupNode))
                {
                    if (((GroupNode)aTN).GroupHandle > handle)
                        handle = ((GroupNode)aTN).GroupHandle;
                }
            }
            handle += 1;

            return handle;
        }

        /// <summary>
        /// Get all layer nodes
        /// </summary>
        /// <returns>layer nodes</returns>
        public List<LayerNode> GetLayerNodes()
        {
            List<LayerNode> layerNodes = new List<LayerNode>();
            foreach (ItemNode aIN in _nodes)
            {
                if (aIN.GetType() == typeof(GroupNode))
                {
                    foreach (LayerNode aLN in ((GroupNode)aIN).Layers)
                        layerNodes.Add(aLN);
                }
                else
                {
                    layerNodes.Add((LayerNode)aIN);
                }
            }

            return layerNodes;
        }

        /// <summary>
        /// Move layer node
        /// </summary>
        /// <param name="lPreIdx">previous index</param>
        /// <param name="lNewIdx">new index</param>
        public void MoveLayerNode(int lPreIdx, int lNewIdx)
        {
            List<LayerNode> layerNodes = GetLayerNodes();
            LayerNode aTN = layerNodes[lPreIdx];

            _nodes.Remove(aTN);
            LayerNode toLN = layerNodes[lNewIdx];
            if (toLN.GroupHandle >= 0)
            {
                GroupNode gNode = GetGroupByHandle(toLN.GroupHandle);
                gNode.InsertLayer(aTN, gNode.GetLayerIndex(toLN));
            }
            else
            {
                _nodes.Insert(_nodes.IndexOf(toLN), aTN);
            }

            //this.Refresh();
        }        

        /// <summary>
        /// Get layer node by handle
        /// </summary>
        /// <param name="handle">handle</param>
        /// <returns>layer node</returns>
        public LayerNode GetLayerNodeByHandle(int handle)
        {
            LayerNode aLN = null;
            foreach (ItemNode aTN in _nodes)
            {
                if (aTN.GetType() == typeof(LayerNode))
                {
                    if (((LayerNode)aTN).LayerHandle == handle)
                    {
                        aLN = (LayerNode)aTN;
                        break;
                    }
                }
                else    //Group node
                {
                    bool find = false;
                    foreach (LayerNode bLN in ((GroupNode)aTN).Layers)
                    {
                        if (bLN.LayerHandle == handle)
                        {
                            aLN = bLN;
                            find = true;
                            break;
                        }
                    }
                    if (find)
                        break;
                }
            }

            return aLN;
        }

        /// <summary>
        /// Get layer node by name
        /// </summary>
        /// <param name="lName">layer name</param>
        /// <returns>layer node</returns>
        public LayerNode GetLayerNodeByName(string lName)
        {
            LayerNode aLN = null;
            foreach (ItemNode aTN in _nodes)
            {
                if (aTN.GetType() == typeof(LayerNode))
                {
                    if (((LayerNode)aTN).Text == lName)
                    {
                        aLN = (LayerNode)aTN;
                        break;
                    }
                }
                else    //Group node
                {
                    bool find = false;
                    foreach (LayerNode bLN in ((GroupNode)aTN).Layers)
                    {
                        if (bLN.Text == lName)
                        {
                            aLN = bLN;
                            find = true;
                            break;
                        }
                    }
                    if (find)
                        break;
                }
            }

            return aLN;
        }

        /// <summary>
        /// Update layer node
        /// </summary>
        /// <param name="aLayer">layer</param>
        public void UpdateLayerNode(MapLayer aLayer)
        {
            UpdateLayerNode(aLayer.Handle);           
        }

        /// <summary>
        /// Update layer node by handle
        /// </summary>
        /// <param name="handle">layer handle</param>
        public void UpdateLayerNode(int handle)
        {
            LayerNode aLN = GetLayerNodeByHandle(handle);
            aLN.Update();

            OnLayersUpdated();
        }

        /// <summary>
        /// Update layer node legend scheme
        /// </summary>
        /// <param name="handle">layer handle</param>
        /// <param name="aLS">legend scheme</param>
        public void UpdateLayerNodeLegendScheme(int handle, LegendScheme aLS)
        {
            LayerNode aLN = GetLayerNodeByHandle(handle);
            aLN.UpdateLegendScheme(aLS);
        }

        #endregion

        #region Other
        /// <summary>
        /// Set layout bounds
        /// </summary>
        /// <param name="rect">rectangle</param>
        public void SetLayoutBounds(Rectangle rect)
        {
            _layoutBounds = rect;
        }

        /// <summary>
        /// Override get property object methods
        /// </summary>
        /// <returns>property object</returns>
        public object GetPropertyObject()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("MapFrameName", "MapFrameName");
            objAttr.Add("BackColor", "BackColor");
            objAttr.Add("ForeColor", "ForeColor");
            objAttr.Add("DrawNeatLine", "DrawNeatLine");
            objAttr.Add("NeatLineColor", "NeatLineColor");
            objAttr.Add("NeatLineSize", "NeatLineSize");
            objAttr.Add("DrawGridLine", "DrawGridLine");
            objAttr.Add("DrawGridTickLine", "DrawGridTickLine");
            objAttr.Add("DrawGridLabel", "DrawGridLabel");
            objAttr.Add("GridLineColor", "GridLineColor");
            objAttr.Add("GridLineSize", "GridLineSize");
            objAttr.Add("GridLineStyle", "GridLineStyle");
            objAttr.Add("GridFont", "GridFont");
            if (_mapView.IsGeoMap)
            {
                objAttr.Add("DrawDegreeSymbol", "DrawDegreeSymbol");
                objAttr.Add("GridXDelt", "GridXDelt");
                objAttr.Add("GridYDelt", "GridYDelt");
                objAttr.Add("GridXOrigin", "GridXOrigin");
                objAttr.Add("GridYOrigin", "GridYOrigin");
            }
            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;
        }

        /// <summary>
        /// Get name object
        /// </summary>
        /// <returns></returns>
        public object GetNameObject()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("MapFrameName", "MapFrameName");
   
            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;
        }

        #endregion

        #endregion

        #region XML import and export
        /// <summary>
        /// Export project XML content
        /// </summary>
        /// <param name="m_Doc">ref XML document</param>
        /// <param name="parent">parent XML element</param>
        /// <param name="projectFilePath">project file path</param>
        public void ExportProjectXML(ref XmlDocument m_Doc, XmlElement parent, string projectFilePath)
        {
            AddMapFrameElement(ref m_Doc, parent, projectFilePath);
        }

        private void AddMapFrameElement(ref XmlDocument m_Doc, XmlElement parent, string projectFilePath)
        {
            XmlElement mapFrame = m_Doc.CreateElement("MapFrame");
            XmlAttribute name = m_Doc.CreateAttribute("Name");
            XmlAttribute active = m_Doc.CreateAttribute("Active");
            XmlAttribute expanded = m_Doc.CreateAttribute("Expanded");
            XmlAttribute order = m_Doc.CreateAttribute("Order");
            XmlAttribute Left = m_Doc.CreateAttribute("Left");
            XmlAttribute Top = m_Doc.CreateAttribute("Top");
            XmlAttribute Width = m_Doc.CreateAttribute("Width");
            XmlAttribute Height = m_Doc.CreateAttribute("Height");
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
            XmlAttribute drawGridTickLine = m_Doc.CreateAttribute("DrawGridTickLine");
            XmlAttribute drawDegreeSymbol = m_Doc.CreateAttribute("DrawDegreeSymbol");

            name.InnerText = Text;
            active.InnerText = Active.ToString();
            expanded.InnerText = IsExpanded.ToString();
            order.InnerText = _order.ToString();
            Left.InnerText = _layoutBounds.Left.ToString();
            Top.InnerText = _layoutBounds.Top.ToString();
            Width.InnerText = _layoutBounds.Width.ToString();
            Height.InnerText = _layoutBounds.Height.ToString();
            DrawMapNeatLine.InnerText = _drawNeatLine.ToString();
            MapNeatLineColor.InnerText = ColorTranslator.ToHtml(_neatLineColor);
            MapNeatLineSize.InnerText = _neatLineSize.ToString();
            GridLineColor.InnerText = ColorTranslator.ToHtml(this.GridLineColor);
            GridLineSize.InnerText = this.GridLineSize.ToString();
            GridLineStyle.InnerText = Enum.GetName(typeof(DashStyle), this.GridLineStyle);
            DrawGridLine.InnerText = this.DrawGridLine.ToString();
            DrawGridLabel.InnerText = this.DrawGridLabel.ToString();
            GridFontName.InnerText = GridFont.Name;
            GridFontSize.InnerText = GridFont.Size.ToString();
            GridXDelt.InnerText = this.GridXDelt.ToString();
            GridYDelt.InnerText = this.GridYDelt.ToString();
            GridXOrigin.InnerText = this.GridXOrigin.ToString();
            GridYOrigin.InnerText = this.GridYOrigin.ToString();
            drawGridTickLine.InnerText = _drawGridTickLine.ToString();
            drawDegreeSymbol.InnerText = _drawDegreeSymbol.ToString();

            mapFrame.Attributes.Append(name);
            mapFrame.Attributes.Append(active);
            mapFrame.Attributes.Append(expanded);
            mapFrame.Attributes.Append(order);
            mapFrame.Attributes.Append(Left);
            mapFrame.Attributes.Append(Top);
            mapFrame.Attributes.Append(Width);
            mapFrame.Attributes.Append(Height);
            mapFrame.Attributes.Append(DrawMapNeatLine);
            mapFrame.Attributes.Append(MapNeatLineColor);
            mapFrame.Attributes.Append(MapNeatLineSize);
            mapFrame.Attributes.Append(GridLineColor);
            mapFrame.Attributes.Append(GridLineSize);
            mapFrame.Attributes.Append(GridLineStyle);
            mapFrame.Attributes.Append(DrawGridLine);
            mapFrame.Attributes.Append(DrawGridLabel);
            mapFrame.Attributes.Append(GridFontName);
            mapFrame.Attributes.Append(GridFontSize);
            mapFrame.Attributes.Append(GridXDelt);
            mapFrame.Attributes.Append(GridYDelt);
            mapFrame.Attributes.Append(GridXOrigin);
            mapFrame.Attributes.Append(GridYOrigin);
            mapFrame.Attributes.Append(drawGridTickLine);
            mapFrame.Attributes.Append(drawDegreeSymbol);

            _mapView.ExportExtentsElement(ref m_Doc, mapFrame);
            _mapView.ExportMapPropElement(ref m_Doc, mapFrame);
            _mapView.ExportGridLineElement(ref m_Doc, mapFrame);
            _mapView.ExportMaskOutElement(ref m_Doc, mapFrame);
            _mapView.ExportProjectionElement(ref m_Doc, mapFrame);
            AddGroupLayerElement(ref m_Doc, mapFrame, projectFilePath);
            _mapView.ExportGraphics(ref m_Doc, mapFrame, _mapView.GraphicCollection.GraphicList);

            parent.AppendChild(mapFrame);
        }

        private void AddGroupLayerElement(ref XmlDocument m_Doc, XmlElement parent, string projectFilePath)
        {
            XmlElement GroupLayer = m_Doc.CreateElement("GroupLayer");
            for (int i = 0; i < _nodes.Count; i++)
            {
                ItemNode aTN = this.Nodes[i];
                if (aTN.GetType() == typeof(LayerNode))
                {
                    MapLayer aLayer = _mapView.GetLayerFromHandle(((LayerNode)aTN).LayerHandle);
                    AddLayerElement(ref m_Doc, GroupLayer, aLayer, projectFilePath);
                }
                else
                    AddGroupElement(ref m_Doc, GroupLayer, (GroupNode)aTN, projectFilePath);
            }

            parent.AppendChild(GroupLayer);
        }

        private void AddGroupElement(ref XmlDocument m_Doc, XmlElement parent, GroupNode aGN, string projectFilePath)
        {
            XmlElement Group = m_Doc.CreateElement("Group");
            XmlAttribute GroupHandle = m_Doc.CreateAttribute("GroupHandle");
            XmlAttribute GroupName = m_Doc.CreateAttribute("GroupName");
            XmlAttribute Expanded = m_Doc.CreateAttribute("Expanded");

            GroupHandle.InnerText = aGN.GroupHandle.ToString();
            GroupName.InnerText = aGN.Text;
            Expanded.InnerText = aGN.IsExpanded.ToString();

            Group.Attributes.Append(GroupHandle);
            Group.Attributes.Append(GroupName);
            Group.Attributes.Append(Expanded);

            foreach (LayerNode aLN in aGN.Layers)
            {
                MapLayer aLayer = _mapView.GetLayerFromHandle(aLN.LayerHandle);
                AddLayerElement(ref m_Doc, Group, aLayer, projectFilePath);
            }

            parent.AppendChild(Group);
        }

        private void AddLayerElement(ref XmlDocument m_Doc, XmlElement parent, MapLayer aLayer, string projectFilePath)
        {
            if (aLayer.LayerType == LayerTypes.VectorLayer)
            {
                VectorLayer aVLayer = (VectorLayer)aLayer;
                if (File.Exists(aVLayer.FileName))
                    _mapView.ExportVectorLayerElement(ref m_Doc, parent, aVLayer, projectFilePath);
            }
            else
            {
                ImageLayer aILayer = (ImageLayer)aLayer;
                if (File.Exists(aILayer.FileName))
                    _mapView.ExportImageLayer(ref m_Doc, parent, aILayer, projectFilePath);
            }
        }

        private void AddVectorLayerElement(ref XmlDocument m_Doc, XmlElement parent, VectorLayer aVLayer,
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
            AddLabelSet(ref m_Doc, Layer, aVLayer.LabelSet);

            parent.AppendChild(Layer);
        }

        private void AddLegendScheme(ref XmlDocument m_Doc, XmlElement parent, LegendScheme aLS)
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
                case ShapeTypes.PolylineZ:
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

        private void AddLabelSet(ref XmlDocument m_Doc, XmlElement parent, LabelSet aLabelSet)
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

            parent.AppendChild(LabelSet);
        }

        private void AddImageLayer(ref XmlDocument m_Doc, XmlElement parent, ImageLayer aILayer, string projectFilePath)
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

            Handle.InnerText = aILayer.Handle.ToString();
            LayerName.InnerText = aILayer.LayerName;
            FileName.InnerText = CGlobal.GetRelativePath(aILayer.FileName, projectFilePath);
            Visible.InnerText = aILayer.Visible.ToString();
            IsMaskout.InnerText = aILayer.IsMaskout.ToString();
            LayerType.InnerText = Enum.GetName(typeof(LayerTypes), aILayer.LayerType);
            LayerDrawType.InnerText = Enum.GetName(typeof(LayerDrawType), aILayer.LayerDrawType);

            Layer.Attributes.Append(Handle);
            Layer.Attributes.Append(LayerName);
            Layer.Attributes.Append(FileName);
            Layer.Attributes.Append(Visible);
            Layer.Attributes.Append(IsMaskout);
            Layer.Attributes.Append(LayerType);
            Layer.Attributes.Append(LayerDrawType);

            parent.AppendChild(Layer);
        }


        /// <summary>
        /// Import project XML content
        /// </summary>
        /// <param name="parent">parent XML element</param>
        public void ImportProjectXML(XmlElement parent)
        {
            this.MapView.LockViewUpdate = true;
            this.Nodes.Clear();
            this.MapView.RemoveAllLayers();

            try
            {
                Text = parent.Attributes["Name"].InnerText;
                Active = bool.Parse(parent.Attributes["Active"].InnerText);

                bool expanded = bool.Parse(parent.Attributes["Expanded"].InnerText);
                if (expanded)
                    this.Expand();
                else
                    this.Collapse();
            }
            catch { }

            try
            {
                _order = int.Parse(parent.Attributes["Order"].InnerText);
                int left = int.Parse(parent.Attributes["Left"].InnerText);
                int top = int.Parse(parent.Attributes["Top"].InnerText);
                int width = int.Parse(parent.Attributes["Width"].InnerText);
                int height = int.Parse(parent.Attributes["Height"].InnerText);
                _layoutBounds = new Rectangle(left, top, width, height);
                _drawNeatLine = bool.Parse(parent.Attributes["DrawNeatLine"].InnerText);
                _neatLineColor = ColorTranslator.FromHtml(parent.Attributes["NeatLineColor"].InnerText);
                _neatLineSize = int.Parse(parent.Attributes["NeatLineSize"].InnerText);
                GridLineColor = ColorTranslator.FromHtml(parent.Attributes["GridLineColor"].InnerText);
                GridLineSize = int.Parse(parent.Attributes["GridLineSize"].InnerText);
                GridLineStyle = (DashStyle)Enum.Parse(typeof(DashStyle),
                    parent.Attributes["GridLineStyle"].InnerText, true);
                DrawGridLine = bool.Parse(parent.Attributes["DrawGridLine"].InnerText);
                //_defaultLayoutMap.DrawGridTickLine = bool.Parse(parent.Attributes["DrawGridTickLine"].InnerText);
                DrawGridLabel = bool.Parse(parent.Attributes["DrawGridLabel"].InnerText);
                string fontName = parent.Attributes["GridFontName"].InnerText;
                float fontSize = float.Parse(parent.Attributes["GridFontSize"].InnerText);
                GridFont = new Font(fontName, fontSize);
                GridXDelt = double.Parse(parent.Attributes["GridXDelt"].InnerText);
                GridYDelt = double.Parse(parent.Attributes["GridYDelt"].InnerText);
                GridXOrigin = double.Parse(parent.Attributes["GridXOrigin"].InnerText);
                GridYOrigin = double.Parse(parent.Attributes["GridYOrigin"].InnerText);
                _drawGridTickLine = bool.Parse(parent.Attributes["DrawGridTickLine"].InnerText);
                _drawDegreeSymbol = bool.Parse(parent.Attributes["DrawDegreeSymbol"].InnerText);
            }
            catch
            {

            }

            _mapView.LoadMapPropElement(parent);
            _mapView.LoadGridLineElement(parent);
            _mapView.LoadMaskOutElement(parent);
            _mapView.LoadProjectionElement(parent);
            LoadGroupLayer(parent);
            _mapView.LoadGraphics(parent);
            _mapView.LoadExtentsElement(parent);
            this.MapView.LockViewUpdate = false;
        }

        private void LoadExtentsElement(XmlElement parent)
        {
            XmlNode Extents = parent.GetElementsByTagName("Extents")[0];
            MeteoInfoC.Global.Extent aExtent = new MeteoInfoC.Global.Extent();
            aExtent.minX = double.Parse(Extents.Attributes["xMin"].InnerText);
            aExtent.maxX = double.Parse(Extents.Attributes["xMax"].InnerText);
            aExtent.minY = double.Parse(Extents.Attributes["yMin"].InnerText);
            aExtent.maxY = double.Parse(Extents.Attributes["yMax"].InnerText);

            _mapView.ViewExtent = aExtent;
        }

        //private void LoadMapPropElement(XmlElement parent)
        //{
        //    XmlNode MapProperty = parent.GetElementsByTagName("MapProperty")[0];
        //    try
        //    {
        //        _mapView.MapPropertyV.BackColor = ColorTranslator.FromHtml(MapProperty.Attributes["BackColor"].InnerText);
        //        _mapView.MapPropertyV.ForeColor = ColorTranslator.FromHtml(MapProperty.Attributes["ForeColor"].InnerText);
        //        _mapView.MapPropertyV.SmoothingMode = (SmoothingMode)Enum.Parse(typeof(SmoothingMode),
        //            MapProperty.Attributes["SmoothingMode"].InnerText, true);
        //    }
        //    catch
        //    {

        //    }
        //}

        //private void LoadGridLineElement(XmlElement parent)
        //{
        //    XmlNode GridLine = parent.GetElementsByTagName("GridLine")[0];
        //    try
        //    {
        //        _mapView.GridLineColor = ColorTranslator.FromHtml(GridLine.Attributes["GridLineColor"].InnerText);
        //        _mapView.GridLineSize = int.Parse(GridLine.Attributes["GridLineSize"].InnerText);
        //        _mapView.GridLineStyle = (DashStyle)Enum.Parse(typeof(DashStyle),
        //            GridLine.Attributes["GridLineStyle"].InnerText, true);
        //        _mapView.DrawGridLine = bool.Parse(GridLine.Attributes["DrawGridLine"].InnerText);
        //        _mapView.DrawGridTickLine = bool.Parse(GridLine.Attributes["DrawGridTickLine"].InnerText);
        //    }
        //    catch
        //    {

        //    }
        //}

        //private void LoadMaskOutElement(XmlElement parent)
        //{
        //    XmlNode MaskOut = parent.GetElementsByTagName("MaskOut")[0];
        //    try
        //    {
        //        _mapView.MaskOut.SetMaskLayer = bool.Parse(MaskOut.Attributes["SetMaskLayer"].InnerText);
        //        _mapView.MaskOut.MaskLayer = MaskOut.Attributes["MaskLayer"].InnerText;
        //    }
        //    catch
        //    {

        //    }
        //}

        //private void LoadProjectionElement(XmlElement parent)
        //{
        //    XmlNode Projection = parent.GetElementsByTagName("Projection")[0];
        //    try
        //    {
        //        //_mapView.Projection.IsLonLatMap = bool.Parse(Projection.Attributes["IsLonLatMap"].InnerText);
        //        _mapView.Projection.ProjStr = Projection.Attributes["ProjStr"].InnerText;
        //        _mapView.Projection.ProjInfo = new ProjectionInfo(_mapView.Projection.ProjStr);
        //        _mapView.Projection.RefLon = double.Parse(Projection.Attributes["RefLon"].InnerText);
        //        _mapView.Projection.RefCutLon = double.Parse(Projection.Attributes["RefCutLon"].InnerText);
        //        if (!(_mapView.Projection.ProjInfo.Transform.ProjectionName == ProjectionNames.Lon_Lat))
        //        {
        //            ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
        //            _mapView.Projection.ProjectLayers_Proj4(_mapView, fromProj, _mapView.Projection.ProjInfo);
        //            _mapView.LonLatLayer = _mapView.Projection.GenerateLonLatLayer();
        //        }
        //    }
        //    catch
        //    {

        //    }
        //}

        private void LoadGroupLayer(XmlElement parent)
        {
            XmlNode theNode;
            if (parent.GetElementsByTagName("GroupLayer").Count > 0)
                theNode = parent.GetElementsByTagName("GroupLayer")[0];
            else
                theNode = parent.GetElementsByTagName("Layers")[0];
            foreach (XmlNode aGL in theNode.ChildNodes)
            {
                if (aGL.Name == "Group")
                    LoadGroup(aGL);
                else
                    LoadLayer(aGL, -1);
            }
        }

        private void LoadGroup(XmlNode aGroup)
        {
            GroupNode aGN = new GroupNode(aGroup.Attributes["GroupName"].InnerText);
            try
            {
                bool expanded = bool.Parse(aGroup.Attributes["Expanded"].InnerText);
                if (expanded)
                    aGN.Expand();
                else
                    aGN.Collapse();
            }
            catch
            {

            }
            finally
            {
                AddGroup(aGN);
                foreach (XmlNode aLayerNode in aGroup.ChildNodes)
                {
                    LoadLayer(aLayerNode, aGN.GroupHandle);
                }
                aGN.UpdateCheckStatus();
            }
        }

        private void LoadLayer(XmlNode aLayer, int groupHnd)
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
                ImageLayer aILayer = _mapView.LoadImageLayer(aLayer);
                if (aILayer != null)
                    AddLayer(aILayer, groupHnd);
            }
            else
            {
                VectorLayer aVLayer = _mapView.LoadVectorLayer(aLayer);
                if (aVLayer != null)
                {
                    AddLayer(aVLayer, groupHnd);
                }
            }
        }

        //private void LoadVectorLayer(XmlNode aVLayer, int groupHnd)
        //{
        //    string aFile = aVLayer.Attributes["FileName"].InnerText;
        //    aFile = Path.GetFullPath(aFile);
        //    VectorLayer aLayer;

        //    if (File.Exists(aFile))
        //    {
        //        switch (Path.GetExtension(aFile).ToLower())
        //        {
        //            case ".dat":
        //                aLayer = MapDataManage.ReadMapFile_MICAPS(aFile);
        //                break;
        //            case ".shp":
        //                aLayer = MapDataManage.ReadMapFile_ShapeFile(aFile);
        //                break;
        //            case ".wmp":
        //                aLayer = MapDataManage.ReadMapFile_WMP(aFile);
        //                break;
        //            default:
        //                aLayer = MapDataManage.ReadMapFile_GrADS(aFile);
        //                break;
        //        }

        //        try
        //        {
        //            aLayer.Handle = int.Parse(aVLayer.Attributes["Handle"].InnerText);
        //            aLayer.LayerName = aVLayer.Attributes["LayerName"].InnerText;
        //            aLayer.Visible = bool.Parse(aVLayer.Attributes["Visible"].InnerText);
        //            aLayer.IsMaskout = bool.Parse(aVLayer.Attributes["IsMaskout"].InnerText);
        //            aLayer.TransparencyPerc = int.Parse(aVLayer.Attributes["TransparencyPerc"].InnerText);
        //            aLayer.AvoidCollision = bool.Parse(aVLayer.Attributes["AvoidCollision"].InnerText);
        //            aLayer.Expanded = bool.Parse(aVLayer.Attributes["Expanded"].InnerText);
        //            aLayer.LayerType = (LayerTypes)Enum.Parse(typeof(LayerTypes),
        //                aVLayer.Attributes["LayerType"].InnerText, true);
        //            aLayer.LayerDrawType = (LayerDrawType)Enum.Parse(typeof(LayerDrawType),
        //                aVLayer.Attributes["LayerDrawType"].InnerText, true);
        //        }
        //        catch
        //        {

        //        }

        //        //Load legend scheme
        //        XmlNode LS = aVLayer.ChildNodes[0];
        //        aLayer.LegendScheme.ImportFromXML(LS);
        //        //LegendScheme aLS = new LegendScheme();
        //        //if (LoadLegendScheme(LS, ref aLS, aLayer.ShapeType))
        //        //{
        //        //    aLayer.LegendScheme = aLS;
        //        //}

        //        //Load label set
        //        XmlNode labelNode = aVLayer.ChildNodes[1];
        //        LabelSet aLabelSet = new LabelSet();
        //        LoadLabelSet(labelNode, ref aLabelSet);
        //        aLayer.LabelSet = aLabelSet;
        //        if (aLayer.LabelSet.DrawLabels)
        //        {
        //            aLayer.AddLabels();
        //        }

        //        AddLayer(aLayer, groupHnd);
        //    }
        //}

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

        //    //if (aShapeType != aST)
        //    //    return false;
        //    aLS.ShapeType = aST;
        //    //aLS.breakNum = Convert.ToInt32(LSNode.Attributes["BreakNum"].InnerText);
        //    aLS.HasNoData = Convert.ToBoolean(LSNode.Attributes["HasNoData"].InnerText);
        //    aLS.MinValue = Convert.ToDouble(LSNode.Attributes["MinValue"].InnerText);
        //    aLS.MaxValue = Convert.ToDouble(LSNode.Attributes["MaxValue"].InnerText);
        //    aLS.MissingValue = Convert.ToDouble(LSNode.Attributes["MissingValue"].InnerText);

        //    XmlNode breaks = LSNode.ChildNodes[0];
        //    switch (aLS.ShapeType)
        //    {
        //        case ShapeTypes.Point:
        //            foreach (XmlNode brk in breaks.ChildNodes)
        //            {
        //                PointBreak aPB = new PointBreak();
        //                aPB.Caption = brk.Attributes["Caption"].InnerText;
        //                aPB.StartValue = brk.Attributes["StartValue"].InnerText;
        //                aPB.EndValue = brk.Attributes["EndValue"].InnerText;
        //                aPB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                aPB.DrawShape = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
        //                aPB.DrawFill = Convert.ToBoolean(brk.Attributes["DrawFill"].InnerText);
        //                aPB.DrawOutline = Convert.ToBoolean(brk.Attributes["DrawOutline"].InnerText);
        //                aPB.IsNoData = Convert.ToBoolean(brk.Attributes["IsNoData"].InnerText);
        //                aPB.OutlineColor = ColorTranslator.FromHtml(brk.Attributes["OutlineColor"].InnerText);
        //                aPB.Size = Convert.ToSingle(brk.Attributes["Size"].InnerText);
        //                aPB.Style = (PointStyle)Enum.Parse(typeof(PointStyle),
        //                    brk.Attributes["Style"].InnerText, true);
        //                aLS.LegendBreaks.Add(aPB);
        //            }
        //            break;
        //        case ShapeTypes.Polyline:
        //        case ShapeTypes.PolylineZ:
        //            foreach (XmlNode brk in breaks.ChildNodes)
        //            {
        //                PolyLineBreak aPLB = new PolyLineBreak();
        //                aPLB.Caption = brk.Attributes["Caption"].InnerText;
        //                aPLB.StartValue = brk.Attributes["StartValue"].InnerText;
        //                aPLB.EndValue = brk.Attributes["EndValue"].InnerText;
        //                aPLB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                aPLB.DrawPolyline = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
        //                aPLB.Size = Convert.ToSingle(brk.Attributes["Size"].InnerText);
        //                aPLB.Style = (DashStyle)Enum.Parse(typeof(DashStyle),
        //                    brk.Attributes["Style"].InnerText, true);
        //                aLS.LegendBreaks.Add(aPLB);
        //            }
        //            break;
        //        case ShapeTypes.Polygon:
        //            foreach (XmlNode brk in breaks.ChildNodes)
        //            {
        //                PolygonBreak aPGB = new PolygonBreak();
        //                aPGB.Caption = brk.Attributes["Caption"].InnerText;
        //                aPGB.StartValue = brk.Attributes["StartValue"].InnerText;
        //                aPGB.EndValue = brk.Attributes["EndValue"].InnerText;
        //                aPGB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                aPGB.DrawShape = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
        //                aPGB.DrawFill = Convert.ToBoolean(brk.Attributes["DrawFill"].InnerText);
        //                aPGB.DrawOutline = Convert.ToBoolean(brk.Attributes["DrawOutline"].InnerText);
        //                aPGB.OutlineSize = Convert.ToSingle(brk.Attributes["OutlineSize"].InnerText);
        //                aPGB.OutlineColor = ColorTranslator.FromHtml(brk.Attributes["OutlineColor"].InnerText);
        //                aLS.LegendBreaks.Add(aPGB);
        //            }
        //            break;
        //    }

        //    return true;
        //}

        //private void LoadLabelSet(XmlNode LabelNode, ref LabelSet aLabelSet)
        //{
        //    try
        //    {
        //        aLabelSet.DrawLabels = bool.Parse(LabelNode.Attributes["DrawLabels"].InnerText);
        //        aLabelSet.FieldName = LabelNode.Attributes["FieldName"].InnerText;
        //        string fontName = LabelNode.Attributes["FontName"].InnerText;
        //        Single fontSize = Single.Parse(LabelNode.Attributes["FontSize"].InnerText);
        //        aLabelSet.LabelFont = new Font(fontName, fontSize);
        //        aLabelSet.LabelColor = ColorTranslator.FromHtml(LabelNode.Attributes["LabelColor"].InnerText);
        //        aLabelSet.DrawShadow = bool.Parse(LabelNode.Attributes["DrawShadow"].InnerText);
        //        aLabelSet.ShadowColor = ColorTranslator.FromHtml(LabelNode.Attributes["ShadowColor"].InnerText);
        //        aLabelSet.LabelAlignType = (AlignType)Enum.Parse(typeof(AlignType),
        //            LabelNode.Attributes["AlignType"].InnerText, true);
        //        aLabelSet.YOffset = int.Parse(LabelNode.Attributes["Offset"].InnerText);
        //        aLabelSet.AvoidCollision = bool.Parse(LabelNode.Attributes["AvoidCollision"].InnerText);
        //    }
        //    catch
        //    {

        //    }
        //}

        //private void LoadImageLayer(XmlNode aILayer, int groupHnd)
        //{
        //    string aFile = aILayer.Attributes["FileName"].InnerText;
        //    aFile = Path.GetFullPath(aFile);

        //    if (File.Exists(aFile))
        //    {
        //        ImageLayer aLayer = MapDataManage.ReadImageFile(aFile);
        //        try
        //        {
        //            aLayer.Handle = int.Parse(aILayer.Attributes["Handle"].InnerText);
        //            aLayer.LayerName = aILayer.Attributes["LayerName"].InnerText;
        //            aLayer.Visible = bool.Parse(aILayer.Attributes["Visible"].InnerText);
        //            aLayer.IsMaskout = bool.Parse(aILayer.Attributes["IsMaskout"].InnerText);
        //            aLayer.LayerType = (LayerTypes)Enum.Parse(typeof(LayerTypes),
        //                aILayer.Attributes["LayerType"].InnerText, true);
        //            aLayer.LayerDrawType = (LayerDrawType)Enum.Parse(typeof(LayerDrawType),
        //                aILayer.Attributes["LayerDrawType"].InnerText, true);
        //        }
        //        catch
        //        {

        //        }

        //        AddLayer(aLayer, groupHnd);
        //    }
        //}

        #endregion

        #region Events
        /// <summary>
        /// Fires the LayersUpdated event
        /// </summary>
        protected virtual void OnLayersUpdated()
        {
            if (LayersUpdated != null) LayersUpdated(this, new EventArgs());
            _mapView.RaiseLayersUndateEvent();
        }

        /// <summary>
        /// Raise LayersUpdated event
        /// </summary>
        public void RaiseLayersUndateEvent()
        {
            OnLayersUpdated();
        }

        /// <summary>
        /// Fires the LayoutBoundsChanged event
        /// </summary>
        protected virtual void OnLayoutBoundsChanged()
        {
            if (LayoutBoundsChanged != null) LayoutBoundsChanged(this, new EventArgs());
        }

        /// <summary>
        /// Raise LayoutBoundsChanged event
        /// </summary>
        public void RaiseLayoutBoundsChangedEvent()
        {
            OnLayoutBoundsChanged();
        }

        /// <summary>
        /// Fires the MapViewUpdated event
        /// </summary>
        protected virtual void OnMapViewUpdated()
        {
            if (MapViewUpdated != null) MapViewUpdated(this, new EventArgs());
        }

        void MapViewViewExtentChanged(object sender, EventArgs e)
        {
            if (_isFireMapViewUpdate)
                OnMapViewUpdated();
        }

        void MapViewLayersUpdated(object sender, EventArgs e)
        {
            if (_isFireMapViewUpdate)
                OnMapViewUpdated();
        }

        void MapViewRedrawed(object sender, EventArgs e)
        {
            if (_isFireMapViewUpdate)
                OnMapViewUpdated();
        }

        void MapViewProjectionChanged(object sender, EventArgs e)
        {
            if (_mapView.Projection.IsLonLatMap)
                _gridLabelPosition = GridLabelPosition.LeftBottom;
            else
                _gridLabelPosition = GridLabelPosition.All;

            foreach (ItemNode aIN in _nodes)
            {
                if (aIN.NodeType == NodeTypes.LayerNode)
                {
                    LayerNode aLN = (LayerNode)aIN;
                    MapLayer aLayer = _mapView.GetLayerFromHandle(aLN.LayerHandle);
                    aLN.MapLayer = aLayer;
                }
                else if (aIN.NodeType == NodeTypes.GroupNode)
                {
                    foreach (LayerNode aLN in ((GroupNode)aIN).Layers)
                    {
                        MapLayer aLayer = _mapView.GetLayerFromHandle(aLN.LayerHandle);
                        aLN.MapLayer = aLayer;
                    }
                }
            }
        }

        #endregion
    }
}
