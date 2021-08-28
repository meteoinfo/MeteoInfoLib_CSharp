using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Drawing.Text;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Resources;

using MeteoInfoC.Map;
using MeteoInfoC.Layer;
using MeteoInfoC.Global;
using MeteoInfoC.Shape;
using MeteoInfoC.Projections;
using MeteoInfoC.Drawing;
using MeteoInfoC.Data.MapData;
using MeteoInfoC.Layout;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Layers legend control
    /// </summary>
    public class LayersLegend : UserControl
    {
        private IContainer components;

        #region Events definitation
        ///// <summary>
        ///// Occurs after layers updated. Including expended status changed.
        ///// </summary>
        //public event EventHandler LayersUpdated;
        /// <summary>
        /// Occurs after mouse click on the group
        /// </summary>
        public event EventHandler GroupMouseClick;
        /// <summary>
        /// Occurs after mouse click on the layer
        /// </summary>
        public event EventHandler LayerMouseClick;
        /// <summary>
        /// Occurs after mouse click on the map frame
        /// </summary>
        public event EventHandler MapFrameMouseClick;
        /// <summary>
        /// Occurs after active map frame is changed
        /// </summary>
        public event EventHandler ActiveMapFrameChanged;
        /// <summary>
        /// Occurs after map frames updated
        /// </summary>
        public event EventHandler MapFramesUpdated;

        #endregion

        #region variables

        /// <summary>
        /// Layer property form
        /// </summary>
        //public frmProperty FrmLayerProp;
        public frmLayerProperty FrmLayerProp;

        private MapLayout _mapLayout;
        private ItemNode _selectedNode;
        //private MapView _MapView;
        //private int _SelectedLayerHandle;
        //private int _SelectedGroup;
        //private List<GroupNode> _Groups;
        //private List<ItemNode> _nodes;
        //private bool _RaiseGroupCheckEvent;
        //private bool _RaiseLayerCheckEvent;
        private Color _selectedBackColor = Color.WhiteSmoke;
        private Color _selectedForeColor = Color.White;

        private Point _mouseDownPos = new Point(0, 0);
        private ItemNode _dragNode = null;
        //private MapFrame _activeMapFrame;
        private MapFrame _currentMapFrame;
        private List<MapFrame> _mapFrames = new List<MapFrame>();
        private bool _isLayoutView = false;

        private VScrollBar _vScrollBar;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LayersLegend()
        {
            InitializeComponent();

            this.AllowDrop = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.Font = new Font("Arial", 8);

            _mapLayout = null;
            //_MapView = new MapView();
            FrmLayerProp = null;
            //_Groups = new List<GroupNode>();
            //_RaiseGroupCheckEvent = true;
            //_RaiseLayerCheckEvent = true;            
            BackColor = Color.White;
            ForeColor = Color.Black;

            //if (_mapFrames.Count == 0)
            //    AddMapFrame(new MapFrame());
            //SetActiveMapFrame(_mapFrames[0]);            
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
            this._vScrollBar = new System.Windows.Forms.VScrollBar();
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
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this._vScrollBar});
            ////
            //// MapFrame
            ////
            //MapFrame mf = new MapFrame();
            //mf.LayersUpdated += MapFrameLayerUpdated;
            //_mapFrames.Add(mf);

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
        /// <summary>
        /// Get or set selected node
        /// </summary>
        public ItemNode SelectedNode
        {
            get { return _selectedNode; }
            set { _selectedNode = value; }
        }

        /// <summary>
        /// Get current map frame
        /// </summary>
        public MapFrame CurrentMapFrame
        {
            get { return GetMapFrame(_selectedNode); }
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
        /// Get or set map frames
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<MapFrame> MapFrames
        {
            get { return _mapFrames; }
            set { _mapFrames = value; }
        }

        /// <summary>
        /// Get or set map layout
        /// </summary>
        public MapLayout MapLayout
        {
            get { return _mapLayout; }
            set
            {
                _mapLayout = value; ;
                _mapLayout.UpdateMapFrames(_mapFrames);
                _mapLayout.ActiveMapFrameChanged += new EventHandler(Layout_ActiveMapFrameChanged);
                _mapLayout.MapFramesUpdated += new EventHandler(Layout_MapFramesUpdated);
            }
        }

        /// <summary>
        /// Get or set if is layout view
        /// </summary>
        public bool IsLayoutView
        {
            get { return _isLayoutView; }
            set
            {
                _isLayoutView = value;
                if (_isLayoutView)
                {
                    if (_mapLayout != null)
                    {
                        foreach (MapFrame aMF in _mapFrames)
                            aMF.IsFireMapViewUpdate = true;

                        if (_mapLayout.HasLegendElement)
                            _mapLayout.ActiveLayoutMap.FireMapViewUpdatedEvent();
                    }
                }
                else
                {
                    if (_mapLayout != null)
                    {
                        foreach (MapFrame aMF in _mapFrames)
                            aMF.IsFireMapViewUpdate = false;
                    }
                }
            }
        }

        ///// <summary>
        ///// Get or set selected layer handle
        ///// </summary>
        //public int SelectedLayer
        //{
        //    get { return _SelectedLayerHandle; }
        //    set { _SelectedLayerHandle = value; }
        //}

        ///// <summary>
        ///// Get or set selected group handle
        ///// </summary>
        //public int SelectedGroup
        //{
        //    get { return _SelectedGroup; }
        //    set { _SelectedGroup = value; }
        //}

        ///// <summary>
        ///// Get or set MapView of the legend
        ///// </summary>
        //public MapView MapView
        //{
        //    get { return _MapView; }
        //    set { _MapView = value; }
        //}

        ///// <summary>
        ///// Get or set node collection
        ///// </summary>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] 
        //public List<ItemNode> Nodes
        //{
        //    get { return _nodes; }
        //    //set { _nodes = value; }
        //}

        ///// <summary>
        ///// Get or set groups
        ///// </summary>
        //public List<GroupNode> Groups
        //{
        //    get { return _Groups; }
        //    set { _Groups = value; }
        //}        

        #endregion

        #region Methods
        /// <summary>
        /// Add a map frame
        /// </summary>
        /// <param name="mf">map frame</param>
        public void AddMapFrame(MapFrame mf)
        {
            mf.LayersUpdated += MapFrameLayerUpdated;
            _mapFrames.Add(mf);
            OnMapFramesUpdated();
        }

        /// <summary>
        /// Remove a map frame
        /// </summary>
        /// <param name="mapFrame">map frame</param>
        public void RemoveMapFrame(MapFrame mapFrame)
        {
            _mapFrames.Remove(mapFrame);
            if (mapFrame.Active)
            {
                _mapFrames[0].Active = true;
                OnActiveMapFrameChanged();
            }
            this.Refresh();
            OnMapFramesUpdated();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            _mapFrames[0].Active = true;
            OnActiveMapFrameChanged();
            while (_mapFrames.Count > 1)
            {
                _mapFrames.RemoveAt(1);
            }
            _mapFrames[0].RemoveAllLayers();

            this.Refresh();
            OnMapFramesUpdated();
        }

        /// <summary>
        /// Set a map frame as active map frame
        /// </summary>
        /// <param name="mapFrame">map frame</param>
        public void SetActiveMapFrame(MapFrame mapFrame)
        {
            foreach (MapFrame mf in _mapFrames)
                mf.Active = false;

            mapFrame.Active = true;
            OnActiveMapFrameChanged();
        }

        /// <summary>
        /// Get new map frame name
        /// </summary>
        /// <returns>name</returns>
        public string GetNewMapFrameName()
        {
            List<string> names = new List<string>();
            foreach (MapFrame mf in _mapFrames)
                if (mf.Text.Contains("New Map Frame"))
                    names.Add(mf.Text);

            string name = "New Map Frame";
            if (names.Count > 0)
            {
                for (int i = 1; i <= 100; i++)
                {
                    name = "New Map Frame " + i.ToString();
                    if (!names.Contains(name))
                        break;
                }
            }

            return name;
        }

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
            XmlElement mapFrames = m_Doc.CreateElement("MapFrames");
            foreach (MapFrame mf in _mapFrames)
            {
                mf.ExportProjectXML(ref m_Doc, mapFrames, projectFilePath);
            }
            parent.AppendChild(mapFrames);
        }

        /// <summary>
        /// Import project XML content
        /// </summary>
        /// <param name="parent">parent XML element</param>
        public void ImportProjectXML(XmlElement parent)
        {
            _mapFrames.Clear();
            XmlNode mapFrames = parent.GetElementsByTagName("MapFrames")[0];
            if (mapFrames == null)
            {
                MapFrame mf = new MapFrame();
                mf.ImportProjectXML(parent);
                //AddMapFrame(mf);
                mf.LayersUpdated += MapFrameLayerUpdated;
                mf.Active = true;
                _mapFrames.Add(mf);
            }
            else
            {
                foreach (XmlNode mapFrame in mapFrames.ChildNodes)
                {
                    MapFrame mf = new MapFrame();
                    mf.ImportProjectXML((XmlElement)mapFrame);
                    //AddMapFrame(mf);
                    mf.LayersUpdated += MapFrameLayerUpdated;
                    _mapFrames.Add(mf);
                }
            }
            //OnActiveMapFrameChanged();
            //_mapFrames[0].ImportProjectXML(parent);
        }

        #endregion

        # region Events

        /// <summary>
        /// Override OnPaint event
        /// </summary>
        /// <param name="e">paint event args</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            DrawMapFrames(g);
        }

        /// <summary>
        /// Override OnPaintBackground event
        /// </summary>
        /// <param name="e">paint event args</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        private void DrawMapFrames(Graphics g)
        {
            int TotalHeight = CalcTotalDrawHeight();
            Rectangle rect;
            if (TotalHeight > this.Height)
            {
                _vScrollBar.Minimum = 0;
                _vScrollBar.SmallChange = Constants.ITEM_HEIGHT;
                _vScrollBar.LargeChange = this.Height;
                _vScrollBar.Maximum = TotalHeight;

                if (_vScrollBar.Visible == false)
                {
                    _vScrollBar.Value = 0;
                    _vScrollBar.Visible = true;
                }

                //RecalcItemPositions();
                rect = new Rectangle(0, -_vScrollBar.Value, this.Width - _vScrollBar.Width, TotalHeight);
            }
            else
            {
                _vScrollBar.Visible = false;
                rect = new Rectangle(0, 0, this.Width, this.Height);
            }
            rect.Y += Constants.ITEM_PAD;

            //Draw map frame
            g.Clear(BackColor);
            foreach (MapFrame mapFrame in _mapFrames)
            {
                DrawMapFrame(g, mapFrame, new Point(Constants.MAPFRAME_LEFT_PAD, rect.Y));
                rect.Y += mapFrame.GetDrawHeight() + Constants.ITEM_PAD * 2;
            }
        }

        private int CalcTotalDrawHeight()
        {
            int height = 0;
            foreach (MapFrame mapFrame in _mapFrames)
                height += mapFrame.GetDrawHeight() + Constants.ITEM_PAD * 2;

            height -= Constants.ITEM_PAD * 2;

            return height;
        }

        //private void RecalcItemPositions()
        //{
        //    //this function calculates the top of each group and layer.
        //    //this is important because the click events use the stored top as
        //    //the way of figuring out if the item was clicked
        //    //and if the checkbox or expansion box was clicked

        //    int CurTop = 0;

        //    if (_vScrollBar.Visible == true)
        //        CurTop = -_vScrollBar.Value;

        //    for (int i = _nodes.Count - 1; i >= 0; i--)
        //    {
        //        ItemNode aIN = _nodes[i];
        //        aIN.Top = CurTop;
        //        CurTop += aIN.GetDrawHeight() + Constants.ITEM_PAD;
        //    }
        //}

        private ItemNode GetNodeByPosition(int x, int y, ref bool inItem, ref int curTop)
        {
            ItemNode aIN = null;
            inItem = false;
            curTop = 0;
            if (_vScrollBar.Visible == true)
                curTop = -_vScrollBar.Value;

            foreach (MapFrame mapFrame in _mapFrames)
            {
                if (y > curTop && y < curTop + mapFrame.Height)
                    return mapFrame;

                curTop += mapFrame.Height + Constants.ITEM_PAD;
                for (int i = mapFrame.Nodes.Count - 1; i >= 0; i--)
                {
                    if (mapFrame.Nodes[i].GetType() == typeof(LayerNode))
                    {
                        if (y > curTop && y < curTop + mapFrame.Nodes[i].GetDrawHeight())
                        {
                            if (y < curTop + mapFrame.Nodes[i].Height)
                                inItem = true;

                            return mapFrame.Nodes[i];
                        }
                        curTop += mapFrame.Nodes[i].GetDrawHeight() + Constants.ITEM_PAD;
                    }
                    else
                    {
                        GroupNode gNode = (GroupNode)mapFrame.Nodes[i];
                        if (y > curTop && y < curTop + gNode.Height)
                            return gNode;

                        curTop += gNode.Height + Constants.ITEM_PAD;
                        if (gNode.IsExpanded)
                        {
                            for (int j = gNode.Layers.Count - 1; j >= 0; j--)
                            {
                                LayerNode aLN = (LayerNode)gNode.Layers[j];
                                if (y > curTop && y < curTop + aLN.GetDrawHeight())
                                {
                                    if (y < curTop + mapFrame.Nodes[i].Height)
                                        inItem = true;

                                    return aLN;
                                }
                                curTop += aLN.GetDrawHeight() + Constants.ITEM_PAD;
                            }
                        }
                    }
                }
            }

            return aIN;
        }

        private ItemNode GetNodeByPosition(int x, int y, ref bool inCheckBox, ref bool inExpansionBox)
        {
            int curTop = 0;
            bool inItem = false;
            ItemNode aIN = GetNodeByPosition(x, y, ref inItem, ref curTop);
            if (aIN != null)
            {
                int leftPad = Constants.MAPFRAME_LEFT_PAD;
                if (aIN.GetType() == typeof(MapFrame))
                {
                    if (x > leftPad && x < leftPad + Constants.EXPAND_BOX_SIZE)
                        inExpansionBox = true;
                    else
                        inExpansionBox = false;

                    if (x > leftPad + Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD &&
                        x < leftPad + Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD + Constants.CHECK_BOX_SIZE)
                        inCheckBox = true;
                    else
                        inCheckBox = false;
                }
                else if (aIN.GetType() == typeof(GroupNode))
                {
                    leftPad += Constants.ITEM_LEFT_PAD;
                    if (x > leftPad && x < leftPad + Constants.EXPAND_BOX_SIZE)
                        inExpansionBox = true;
                    else
                        inExpansionBox = false;

                    if (x > leftPad + Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD &&
                        x < leftPad + Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD + Constants.CHECK_BOX_SIZE)
                        inCheckBox = true;
                    else
                        inCheckBox = false;
                }
                else if (aIN.GetType() == typeof(LayerNode))
                {
                    if (inItem)
                    {
                        leftPad += Constants.ITEM_LEFT_PAD;
                        if (((LayerNode)aIN).GroupHandle >= 0)
                        {
                            leftPad += Constants.ITEM_LEFT_PAD;
                        }
                        if (x > leftPad && x < leftPad + Constants.EXPAND_BOX_SIZE)
                            inExpansionBox = true;
                        else
                            inExpansionBox = false;

                        if (x > leftPad + Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD &&
                            x < leftPad + Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD + Constants.CHECK_BOX_SIZE)
                            inCheckBox = true;
                        else
                            inCheckBox = false;
                    }
                }
            }

            return aIN;
        }

        private void DrawMapFrame(Graphics g, MapFrame aMapFrame, Point sP)
        {
            //Draw map frame
            //aMapFrame.Nodes = _nodes;
            if (aMapFrame.Selected)
            {
                Rectangle rect = new Rectangle(3, sP.Y, this.ClientRectangle.Width - 10, aMapFrame.Height);
                g.FillRectangle(new SolidBrush(_selectedBackColor), rect);
                g.DrawRectangle(new Pen(Color.LightGray), rect);
            }

            SolidBrush aBrush = new SolidBrush(ForeColor);
            if (aMapFrame.Nodes.Count > 0)
                DrawExpansionBox(g, new Point(sP.X, sP.Y + Constants.EXPAND_BOX_TOP_PAD), aMapFrame.IsExpanded);

            Icon icon = new Icon(this.GetType().Assembly.GetManifestResourceStream("MeteoInfoC.Resources.Layers.ico"));
            if (icon != null)
                g.DrawIcon(icon, sP.X + Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD, sP.Y);

            Font newFont = this.Font;
            if (aMapFrame.Active)
                newFont = new Font(this.Font, FontStyle.Bold);
            g.DrawString(aMapFrame.Text, newFont, aBrush, sP.X + Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD +
                Constants.CHECK_BOX_SIZE + Constants.TEXT_LEFT_PAD * 2, sP.Y);

            //Draw nodes
            if (aMapFrame.IsExpanded)
            {
                sP.X += Constants.ITEM_LEFT_PAD;
                sP.Y += aMapFrame.Height + Constants.ITEM_PAD;
                for (int i = aMapFrame.Nodes.Count - 1; i >= 0; i--)
                {
                    ItemNode aTN = aMapFrame.Nodes[i];
                    if (aTN.GetType() == typeof(GroupNode))
                    {
                        if (sP.Y + aTN.GetDrawHeight() < this.ClientRectangle.Top)
                        {
                            sP.Y += aTN.GetDrawHeight() + Constants.ITEM_PAD;
                            continue;
                        }
                        DrawGroupNode(g, (GroupNode)aTN, sP);
                    }
                    else
                    {
                        LayerNode aLN = (LayerNode)aTN;
                        if (sP.Y + aLN.GetDrawHeight() < this.ClientRectangle.Top)
                        {
                            sP.Y += aLN.GetDrawHeight() + Constants.ITEM_PAD;
                            continue;
                        }
                        DrawLayerNode(g, (LayerNode)aTN, sP);
                    }

                    sP.Y += aTN.GetDrawHeight() + Constants.ITEM_PAD;
                    if (sP.Y >= this.ClientRectangle.Bottom)
                        break;
                }
            }
        }

        private void DrawGroupNode(Graphics g, GroupNode groupNode, Point sP)
        {
            //Draw group
            if (groupNode.Selected)
            {
                Rectangle rect = new Rectangle(3, sP.Y, this.ClientRectangle.Width - 10, groupNode.Height);
                g.FillRectangle(new SolidBrush(_selectedBackColor), rect);
                g.DrawRectangle(new Pen(Color.LightGray), rect);
            }

            SolidBrush aBrush = new SolidBrush(ForeColor);
            if (groupNode.Layers.Count > 0)
                DrawExpansionBox(g, new Point(sP.X, sP.Y + Constants.EXPAND_BOX_TOP_PAD), groupNode.IsExpanded);

            groupNode.UpdateCheckStatus();
            DrawCheckBox(g, new Point(sP.X + Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD,
                sP.Y + Constants.CHECK_TOP_PAD), groupNode.CheckStatus);
            //Font newFont = new Font(this.Font, FontStyle.Bold);
            g.DrawString(groupNode.Text, Font, aBrush, sP.X + Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD +
                Constants.CHECK_BOX_SIZE + Constants.TEXT_LEFT_PAD, sP.Y);

            //Draw layer nodes
            if (groupNode.IsExpanded)
            {
                sP.Y += Constants.ITEM_HEIGHT + Constants.ITEM_PAD;
                sP.X += Constants.ITEM_LEFT_PAD;
                for (int j = groupNode.Layers.Count - 1; j >= 0; j--)
                {
                    LayerNode layerNode = groupNode.Layers[j];
                    DrawLayerNode(g, layerNode, sP);
                    sP.Y += layerNode.GetDrawHeight() + Constants.ITEM_PAD;
                }
            }
        }

        private void DrawLayerNode(Graphics g, LayerNode layerNode, Point sP)
        {
            //Draw Layer
            SolidBrush aBrush = new SolidBrush(ForeColor);
            if (layerNode.Selected)
            {
                Rectangle rect = new Rectangle(3, sP.Y, this.ClientRectangle.Width - 10, layerNode.Height);
                g.FillRectangle(new SolidBrush(_selectedBackColor), rect);
                g.DrawRectangle(new Pen(Color.LightGray), rect);
            }

            if (layerNode.LegendNodes.Count > 0)
                DrawExpansionBox(g, new Point(sP.X, sP.Y + Constants.EXPAND_BOX_TOP_PAD), layerNode.IsExpanded);

            int checkStatus = 0;
            if (layerNode.Checked)
                checkStatus = 1;

            DrawCheckBox(g, new Point(sP.X + Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD,
                sP.Y + Constants.CHECK_TOP_PAD), checkStatus);
            g.DrawString(layerNode.Text, Font, aBrush, sP.X + Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD +
                Constants.CHECK_BOX_SIZE + Constants.TEXT_LEFT_PAD, sP.Y);

            //Draw legend nodes
            if (layerNode.IsExpanded)
            {
                sP.X += Constants.ITEM_LEFT_PAD;
                //sP.X += Constants.EXPAND_BOX_SIZE + Constants.CHECK_LEFT_PAD + Constants.CHECK_BOX_SIZE;
                sP.Y = sP.Y + layerNode.Height + Constants.ITEM_PAD;
                foreach (LegendNode aLN in layerNode.LegendNodes)
                {
                    Rectangle rect = new Rectangle(sP.X, sP.Y, 40, aLN.Height);
                    DrawLegendNode(aLN, rect, g);
                    sP.Y = sP.Y + aLN.Height + Constants.ITEM_PAD;
                }
            }
        }

        private void DrawLegendNode(LegendNode aLN, Rectangle rect, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Single aSize;
            Point aP = new Point(0, 0);
            Single width, height;
            string caption = "";

            switch (aLN.LegendBreak.BreakType)
            {
                case BreakTypes.PointBreak:
                    PointBreak aPB = (PointBreak)aLN.LegendBreak;
                    caption = aPB.Caption;
                    aP.X = rect.Left + rect.Width / 2;
                    aP.Y = rect.Top + rect.Height / 2;
                    aSize = aPB.Size;
                    if (aPB.DrawShape)
                    {
                        //Draw.DrawPoint(aPB.Style, aP, aPB.Color, aPB.OutlineColor,
                        //    aPB.Size, aPB.DrawOutline, aPB.DrawFill, g);
                        if (aPB.MarkerType == MarkerType.Character)
                        {
                            TextRenderingHint aTextRendering = g.TextRenderingHint;
                            g.TextRenderingHint = TextRenderingHint.AntiAlias;
                            Draw.DrawPoint(aP, aPB, g);
                            g.TextRenderingHint = aTextRendering;
                        }
                        else
                            Draw.DrawPoint(aP, aPB, g);
                    }
                    break;
                case BreakTypes.PolylineBreak:
                    PolyLineBreak aPLB = (PolyLineBreak)aLN.LegendBreak;
                    caption = aPLB.Caption;
                    aP.X = rect.Left + rect.Width / 2;
                    aP.Y = rect.Top + rect.Height / 2;
                    aSize = aPLB.Size;
                    width = rect.Width / 3 * 2;
                    height = rect.Height / 3 * 2;
                    Draw.DrawPolyLineSymbol(aP, width, height, aPLB, g);
                    break;
                case BreakTypes.PolygonBreak:
                    PolygonBreak aPGB = (PolygonBreak)aLN.LegendBreak;
                    caption = aPGB.Caption;
                    aP.X = rect.Left + rect.Width / 2;
                    aP.Y = rect.Top + rect.Height / 2;
                    width = rect.Width / 3 * 2;
                    height = rect.Height / 5 * 4;
                    if (aPGB.DrawShape)
                    {
                        //Draw.DrawPolygonSymbol(aP, aPGB.Color, aPGB.OutlineColor, width,
                        //    height, aPGB.DrawFill, aPGB.DrawOutline, g);
                        Draw.DrawPolygonSymbol(aP, width, height, aPGB, 0, g);
                    }
                    break;
                case BreakTypes.ColorBreak:
                    ColorBreak aCB = aLN.LegendBreak;
                    caption = aCB.Caption;
                    aP.X = rect.Left + rect.Width / 2;
                    aP.Y = rect.Top + rect.Height / 2;
                    width = rect.Width / 3 * 2;
                    height = rect.Height / 3 * 2;
                    Draw.DrawPolygonSymbol(aP, aCB.Color, Color.Black, width,
                            height, true, true, g);
                    break;
                case BreakTypes.ChartBreak:
                    ChartBreak aChB = (ChartBreak)aLN.LegendBreak;
                    aP.X = rect.Left;
                    aP.Y = rect.Bottom - 5;
                    switch (aChB.ChartType)
                    {
                        case ChartTypes.BarChart:
                            Draw.DrawBarChartSymbol(aP, aChB, g, true, this.Font);
                            break;
                        case ChartTypes.PieChart:
                            Draw.DrawPieChartSymbol(aP, aChB, g);
                            break;
                    }
                    break;
            }

            rect.X = rect.X + rect.Width + 5;
            g.DrawString(caption, this.Font, Brushes.Black, rect.X, rect.Y);
        }

        private void DrawLegendNode_Old(LegendNode aLN, Rectangle rect, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Single aSize;
            Point aP = new Point(0, 0);
            Single width, height;
            string caption = "";

            switch (aLN.ShapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes.PointZ:
                case ShapeTypes.WindArraw:
                case ShapeTypes.WindBarb:
                case ShapeTypes.WeatherSymbol:
                case ShapeTypes.StationModel:
                    PointBreak aPB = (PointBreak)aLN.LegendBreak;
                    caption = aPB.Caption;
                    aP.X = rect.Left + rect.Width / 2;
                    aP.Y = rect.Top + rect.Height / 2;
                    aSize = aPB.Size;
                    if (aPB.DrawShape)
                    {
                        //Draw.DrawPoint(aPB.Style, aP, aPB.Color, aPB.OutlineColor,
                        //    aPB.Size, aPB.DrawOutline, aPB.DrawFill, g);
                        if (aPB.MarkerType == MarkerType.Character)
                        {
                            TextRenderingHint aTextRendering = g.TextRenderingHint;
                            g.TextRenderingHint = TextRenderingHint.AntiAlias;
                            Draw.DrawPoint(aP, aPB, g);
                            g.TextRenderingHint = aTextRendering;
                        }
                        else
                            Draw.DrawPoint(aP, aPB, g);
                    }
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineZ:
                    PolyLineBreak aPLB = (PolyLineBreak)aLN.LegendBreak;
                    caption = aPLB.Caption;
                    aP.X = rect.Left + rect.Width / 2;
                    aP.Y = rect.Top + rect.Height / 2;
                    aSize = aPLB.Size;
                    width = rect.Width / 3 * 2;
                    height = rect.Height / 3 * 2;
                    Draw.DrawPolyLineSymbol(aP, width, height, aPLB, g);
                    break;
                case ShapeTypes.Polygon:
                    PolygonBreak aPGB = (PolygonBreak)aLN.LegendBreak;
                    caption = aPGB.Caption;
                    aP.X = rect.Left + rect.Width / 2;
                    aP.Y = rect.Top + rect.Height / 2;
                    width = rect.Width / 3 * 2;
                    height = rect.Height / 5 * 4;
                    if (aPGB.DrawShape)
                    {
                        //Draw.DrawPolygonSymbol(aP, aPGB.Color, aPGB.OutlineColor, width,
                        //    height, aPGB.DrawFill, aPGB.DrawOutline, g);
                        Draw.DrawPolygonSymbol(aP, width, height, aPGB, 0, g);
                    }
                    break;
                case ShapeTypes.Image:
                    ColorBreak aCB = aLN.LegendBreak;
                    caption = aCB.Caption;
                    aP.X = rect.Left + rect.Width / 2;
                    aP.Y = rect.Top + rect.Height / 2;
                    width = rect.Width / 3 * 2;
                    height = rect.Height / 3 * 2;
                    Draw.DrawPolygonSymbol(aP, aCB.Color, Color.Black, width,
                            height, true, true, g);
                    break;
            }

            rect.X = rect.X + rect.Width + 5;
            g.DrawString(caption, this.Font, Brushes.Black, rect.X, rect.Y);
        }

        private void DrawExpansionBox(Graphics g, Point sP, bool expanded)
        {
            int size = 8;
            int gap = 2;
            Rectangle rect = new Rectangle(sP, new Size(size, size));
            g.DrawRectangle(new Pen(Color.Gray), rect);

            GraphicsPath path = new GraphicsPath();
            path.AddLine(sP.X + gap, sP.Y + size / 2, sP.X + size - gap, sP.Y + size / 2);
            if (!expanded)
            {
                path.StartFigure();
                path.AddLine(sP.X + size / 2, sP.Y + gap, sP.X + size / 2, sP.Y + size - gap);
            }

            g.DrawPath(new Pen(Color.Black), path);
        }

        private void DrawCheckBox(Graphics g, Point sP, int checkStatus)
        {
            int size = 10;
            Rectangle rect = new Rectangle(sP, new Size(size, size));
            g.DrawRectangle(new Pen(Color.Gray), rect);

            if (checkStatus == 2)
                g.FillRectangle(new SolidBrush(Color.LightGray), rect);

            switch (checkStatus)
            {
                case 1:    //Checked
                case 2:    //Partly checked
                    GraphicsPath path = new GraphicsPath();
                    path.AddLine(sP.X + 2, sP.Y + 6, sP.X + 5, sP.Y + 8);
                    path.AddLine(sP.X + 5, sP.Y + 8, sP.X + 8, sP.Y + 2);
                    g.DrawPath(new Pen(Color.Black), path);
                    break;
            }
        }

        /// <summary>
        /// Drag enter event
        /// </summary>
        /// <param name="drgevent"></param>
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);

            drgevent.Effect = DragDropEffects.Move;
        }

        ///// <summary>
        ///// Item drag event
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnItemDrag(ItemDragEventArgs e)
        //{
        //    base.OnItemDrag(e);

        //    DoDragDrop(e.Item, DragDropEffects.Move);
        //}

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            VScrollBar sbar = (VScrollBar)sender;
            sbar.Value = e.NewValue;
            Redraw();
        }

        /// <summary>
        /// Redraw the Legend
        /// </summary>
        protected internal void Redraw()
        {
            this.Invalidate();
        }

        /// <summary>
        /// Override OnResize event
        /// </summary>
        /// <param name="e">envent args</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.Width > 0 && this.Height > 0)
            {
                //m_BackBuffer = new Bitmap(this.Width, this.Height);
                //m_Draw = Graphics.FromImage(m_BackBuffer);
                _vScrollBar.Top = 0;
                _vScrollBar.Height = this.Height;
                _vScrollBar.Left = this.Width - _vScrollBar.Width;
            }
            this.Invalidate();
        }

        /// <summary>
        /// Mouse click event
        /// </summary>
        /// <param name="e">mouse envent args</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            bool inItem = false;
            int curTop = 0;
            ItemNode aNode = GetNodeByPosition(e.X, e.Y, ref inItem, ref curTop);
            if (aNode == null)
                return;

            SelectNode(aNode);
            if (aNode.GetType() == typeof(GroupNode))
                OnGroupMouseClick(e);
            else if (aNode.GetType() == typeof(LayerNode))
                OnLayerMouseClick(e);
            else if (aNode.GetType() == typeof(MapFrame))
                OnMapFrameMouseClick(e);

            this.Invalidate();
        }

        private MapFrame GetMapFrame(ItemNode aNode)
        {
            MapFrame mf = null;
            switch (aNode.NodeType)
            {
                case NodeTypes.MapFrameNode:
                    mf = (MapFrame)aNode;
                    break;
                case NodeTypes.LayerNode:
                    mf = ((LayerNode)aNode).MapFrame;
                    break;
                case NodeTypes.GroupNode:
                    mf = ((GroupNode)aNode).MapFrame;
                    break;
            }
            return mf;
        }

        private void SelectNode(ItemNode aNode)
        {
            _selectedNode = aNode;
            foreach (MapFrame mf in _mapFrames)
            {
                mf.Selected = false;
                mf.UnSelectNodes();
            }

            aNode.Selected = true;
            MapFrame aMF = GetMapFrame(aNode);
            if (aNode.NodeType == NodeTypes.LayerNode)
                aMF.MapView.SelectedLayer = ((LayerNode)aNode).LayerHandle;
            else
                aMF.MapView.SelectedLayer = -1;
        }

        /// <summary>
        /// Override OnMouseDoubelClick event
        /// </summary>
        /// <param name="e">mouse event args</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            bool inItem = false;
            int curTop = 0;
            ItemNode aNode = GetNodeByPosition(e.X, e.Y, ref inItem, ref curTop);
            if (aNode != null)
            {
                if (aNode.GetType() == typeof(LayerNode))
                {
                    LayerNode aLN = (LayerNode)aNode;
                    //MapLayer aLayerObj = aLN.MapFrame.MapView.GetLayerFromHandle(aLN.LayerHandle);
                    MapLayer aLayerObj = aLN.MapLayer;
                    //object aLayerSet = new object();
                    //switch (aLayerObj.LayerType)
                    //{
                    //    case LayerTypes.VectorLayer:                            
                    //        aLayerSet = new VectorLayerSet((VectorLayer)aLayerObj, this);
                    //        break;
                    //    case LayerTypes.ImageLayer:                            
                    //        aLayerSet = new ImageLayerSet((ImageLayer)aLayerObj, this);
                    //        break;
                    //    case LayerTypes.RasterLayer:                            
                    //        aLayerSet = new RasterLayerSet((RasterLayer)aLayerObj, this);
                    //        break;
                    //}

                    if (FrmLayerProp == null)
                    {
                        FrmLayerProp = new frmLayerProperty(aLayerObj, aLN.MapFrame);
                        FrmLayerProp.Legend = this;
                        //FrmLayerProp.Text = "Layer Set";
                        //FrmLayerProp.Height = 400;
                        //FrmLayerProp.SetLayersLegend(this);
                        //FrmLayerProp.SetParent(this);
                        FrmLayerProp.Show(this);
                    }
                    else
                    {
                        FrmLayerProp.MapLayer = aLayerObj;
                        FrmLayerProp.Legend = this;
                        FrmLayerProp.Activate();
                    }

                    //FrmLayerProp.TabPage_General.SetObject(aLayerSet);
                }
                else if (aNode.GetType() == typeof(GroupNode))
                {
                    ((GroupNode)aNode).SetProperties();
                }
                else if (aNode.NodeType == NodeTypes.MapFrameNode)
                {
                    object propertyObj = ((MapFrame)aNode).GetNameObject();
                    if (propertyObj != null)
                    {
                        frmProperty aFrmProperty = new frmProperty(true);
                        aFrmProperty.SetObject(propertyObj);
                        aFrmProperty.SetParent(this);
                        aFrmProperty.ShowDialog();
                    }
                }
            }
        }

        ///// <summary>
        ///// Override OnMouseDoubelClick event
        ///// </summary>
        ///// <param name="e">mouse event args</param>
        //protected override void OnMouseDoubleClick_Old(MouseEventArgs e)
        //{
        //    base.OnMouseDoubleClick(e);

        //    bool inItem = false;
        //    int curTop = 0;
        //    ItemNode aNode = GetNodeByPosition(e.X, e.Y, ref inItem, ref curTop);
        //    if (aNode != null)
        //    {
        //        if (aNode.GetType() == typeof(LayerNode))
        //        {
        //            LayerNode aLN = (LayerNode)aNode;
        //            MapLayer aLayerObj = _MapView.GetLayerFromHandle(aLN.LayerHandle);
        //            object aLayerSet = new object();
        //            switch (aLayerObj.LayerType)
        //            {
        //                case LayerTypes.VectorLayer:
        //                    aLayerSet = new VectorLayerSet((VectorLayer)aLayerObj, this);
        //                    break;
        //                case LayerTypes.ImageLayer:
        //                    aLayerSet = new ImageLayerSet((ImageLayer)aLayerObj, this);
        //                    break;
        //                case LayerTypes.RasterLayer:
        //                    aLayerSet = new RasterLayerSet((RasterLayer)aLayerObj, this);
        //                    break;
        //            }

        //            if (FrmLayerProp == null)
        //            {
        //                FrmLayerProp = new frmProperty();
        //                FrmLayerProp.Text = "Layer Set";
        //                FrmLayerProp.Height = 400;
        //                //FrmLayerProp.SetLayersLegend(this);
        //                FrmLayerProp.SetParent(this);
        //                FrmLayerProp.Show(this);
        //            }
        //            else
        //            {
        //                FrmLayerProp.Activate();
        //            }

        //            FrmLayerProp.SetObject(aLayerSet);
        //        }
        //        else if (aNode.GetType() == typeof(GroupNode))
        //        {
        //            ((GroupNode)aNode).SetProperties();
        //        }
        //    }
        //}

        /// <summary>
        /// Override OnMouseDown event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            _mouseDownPos.X = e.X;
            _mouseDownPos.Y = e.Y;

            bool inItem = false;
            int curTop = 0;
            _dragNode = GetNodeByPosition(e.X, e.Y, ref inItem, ref curTop);
        }

        /// <summary>
        /// Override OnMouseMove event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left)
            {
                if (_dragNode != null)
                {
                    bool inItem = false;
                    int curTop = 0;
                    ItemNode aNode = GetNodeByPosition(e.X, e.Y, ref inItem, ref curTop);
                    this.Refresh();
                    if (aNode != null && aNode != _dragNode)
                    {
                        if (_dragNode.GetType() == typeof(GroupNode) && aNode.GetType() == typeof(LayerNode))
                        {
                            if (((LayerNode)aNode).GroupHandle != -1)
                                return;
                        }

                        //Draw drag line                        
                        Pen aPen = new Pen(Color.Gray);
                        aPen.Width = 3;
                        Graphics g = this.CreateGraphics();
                        int y = curTop + aNode.Height;
                        if (aNode.GetType() == typeof(LayerNode))
                            y = curTop + aNode.GetDrawHeight();

                        g.DrawLine(new Pen(Color.Black), Constants.ITEM_LEFT_PAD, y,
                            this.ClientRectangle.Width - Constants.ITEM_RIGHT_PAD, y);
                    }
                }
            }
        }

        /// <summary>
        /// Override OnMouseUp event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
            {
                if (_dragNode != null)
                {
                    bool inItem = false;
                    int curTop = 0;
                    ItemNode aNode = GetNodeByPosition(e.X, e.Y, ref inItem, ref curTop);

                    if (aNode != null && aNode != _dragNode)
                    {
                        MapFrame fromMF = GetMapFrame(_dragNode);
                        MapFrame mapFrame = GetMapFrame(aNode);
                        if (object.ReferenceEquals(fromMF, mapFrame))
                        {
                            if (_dragNode.NodeType == NodeTypes.GroupNode)
                            {
                                //Remove drag node
                                ((GroupNode)_dragNode).MapFrame.RemoveNode(_dragNode);

                                //Add the node to new position
                                if (aNode.NodeType == NodeTypes.MapFrameNode)
                                {
                                    mapFrame.AddNode(_dragNode);
                                }
                                else
                                {
                                    int idx = mapFrame.Nodes.IndexOf(aNode);
                                    mapFrame.InsertNode(idx, _dragNode);
                                }
                            }
                            else if (_dragNode.NodeType == NodeTypes.LayerNode)
                            {
                                //Remove drag node
                                if (((LayerNode)_dragNode).GroupHandle >= 0)
                                {
                                    GroupNode sGroup = mapFrame.GetGroupByHandle(((LayerNode)_dragNode).GroupHandle);
                                    sGroup.RemoveLayer((LayerNode)_dragNode);
                                }
                                else
                                {
                                    mapFrame.RemoveNode(_dragNode);
                                }

                                //Add to new position
                                if (aNode.NodeType == NodeTypes.MapFrameNode)
                                {
                                    mapFrame.AddNode(_dragNode);
                                }
                                else if (aNode.NodeType == NodeTypes.GroupNode)
                                {
                                    ((GroupNode)aNode).AddLayer((LayerNode)_dragNode);
                                }
                                else if (aNode.NodeType == NodeTypes.LayerNode)
                                {
                                    if (((LayerNode)aNode).GroupHandle >= 0)
                                    {
                                        GroupNode dGroup = mapFrame.GetGroupByHandle(((LayerNode)aNode).GroupHandle);
                                        dGroup.InsertLayer((LayerNode)_dragNode, dGroup.GetLayerIndex((LayerNode)aNode));
                                    }
                                    else
                                    {
                                        int idx = mapFrame.Nodes.IndexOf(aNode);
                                        mapFrame.InsertNode(idx, _dragNode);
                                    }
                                }
                            }

                            mapFrame.ReOrderMapViewLayers();
                        }
                        else
                        {
                            if (_dragNode.NodeType == NodeTypes.GroupNode)
                            {
                                //Add the node to new position
                                GroupNode newGN = new GroupNode(_dragNode.Text);
                                foreach (LayerNode aLN in ((GroupNode)_dragNode).Layers)
                                {
                                    LayerNode bLN = (LayerNode)aLN.Clone();
                                    //if (!fromMF.MapView.Projection.IsLonLatMap)
                                    //    bLN.MapLayer = (MapLayer)fromMF.MapView.GetGeoLayerFromHandle(bLN.LayerHandle).Clone();
                                    newGN.AddLayer(bLN);
                                }
                                if (aNode.NodeType == NodeTypes.MapFrameNode)
                                {
                                    mapFrame.AddGroupNode(newGN);
                                }
                                else
                                {
                                    int idx = mapFrame.Nodes.IndexOf(aNode);
                                    mapFrame.InsertGroupNode(idx, newGN);
                                }
                            }
                            else if (_dragNode.NodeType == NodeTypes.LayerNode)
                            {
                                //Add to new position
                                LayerNode aLN = (LayerNode)((_dragNode)).Clone();
                                //if (!fromMF.MapView.Projection.IsLonLatMap)
                                //    aLN.MapLayer = (MapLayer)fromMF.MapView.GetGeoLayerFromHandle(aLN.LayerHandle).Clone();
                                if (aNode.NodeType == NodeTypes.MapFrameNode)
                                {
                                    mapFrame.AddLayerNode(aLN);
                                }
                                else if (aNode.NodeType == NodeTypes.GroupNode)
                                {
                                    mapFrame.AddLayerNode(aLN, (GroupNode)aNode);
                                }
                                else if (aNode.NodeType == NodeTypes.LayerNode)
                                {
                                    if (((LayerNode)aNode).GroupHandle >= 0)
                                    {
                                        GroupNode dGroup = mapFrame.GetGroupByHandle(((LayerNode)aNode).GroupHandle);
                                        //dGroup.InsertLayer((LayerNode)_dragNode, dGroup.GetLayerIndex((LayerNode)aNode));
                                        mapFrame.InsertLayerNode(dGroup.GetLayerIndex((LayerNode)aNode), aLN, dGroup);
                                    }
                                    else
                                    {
                                        int idx = mapFrame.Nodes.IndexOf(aNode);
                                        mapFrame.InsertLayerNode(idx, aLN);
                                    }
                                }
                            }
                        }

                        Redraw();
                    }
                }
            }
        }

        private void OnRemoveLayerClick(object sender, EventArgs e)
        {
            if (_selectedNode.NodeType == NodeTypes.LayerNode)
            {
                LayerNode aLN = (LayerNode)_selectedNode;
                aLN.MapFrame.RemoveLayer(aLN);
            }

            this.Invalidate();
        }

        private void OnMinVisScaleClick(object sender, EventArgs e)
        {
            LayerNode aLN = (LayerNode)_selectedNode;
            MapLayer aLayer = aLN.MapFrame.MapView.GetLayerFromHandle(aLN.LayerHandle);
            aLayer.VisibleScale.EnableMinVisScale = true;
            aLayer.VisibleScale.MinVisScale = aLN.MapFrame.MapView.GetGeoScale();

            this.Invalidate();
            aLN.MapFrame.MapView.PaintLayers();
        }

        private void OnMaxVisScaleClick(object sender, EventArgs e)
        {
            LayerNode aLN = (LayerNode)_selectedNode;
            MapLayer aLayer = aLN.MapFrame.MapView.GetLayerFromHandle(aLN.LayerHandle);
            aLayer.VisibleScale.EnableMaxVisScale = true;
            aLayer.VisibleScale.MaxVisScale = aLN.MapFrame.MapView.GetGeoScale();

            this.Invalidate();
            aLN.MapFrame.MapView.PaintLayers();
        }

        private void OnRemoveVisScaleClick(object sender, EventArgs e)
        {
            LayerNode aLN = (LayerNode)_selectedNode;
            MapLayer aLayer = aLN.MapFrame.MapView.GetLayerFromHandle(aLN.LayerHandle);
            aLayer.VisibleScale.EnableMinVisScale = false;
            aLayer.VisibleScale.EnableMaxVisScale = false;

            this.Invalidate();
            aLN.MapFrame.MapView.PaintLayers();
        }

        private void OnAttrTableMenuClick(Object sender, EventArgs e)
        {
            LayerNode aLN = (LayerNode)_selectedNode;
            MapLayer aLayer = aLN.MapFrame.MapView.GetLayerFromHandle(aLN.LayerHandle);
            if (aLayer.LayerType == LayerTypes.VectorLayer)
            {
                frmAttriData frm = new frmAttriData((VectorLayer)aLayer);
                frm.Show();
            }
        }

        private void OnZoomLayerMenuClick(object sender, EventArgs e)
        {
            LayerNode aLN = (LayerNode)_selectedNode;
            MapLayer aLayer = aLN.MapFrame.MapView.GetLayerFromHandle(aLN.LayerHandle);
            aLN.MapFrame.MapView.ZoomToExtent(aLayer.Extent);
        }

        private void OnLabelMenuClick(object sender, EventArgs e)
        {
            LayerNode aLN = (LayerNode)_selectedNode;
            MapLayer aLayer = aLN.MapFrame.MapView.GetLayerFromHandle(aLN.LayerHandle);
            if (aLayer.LayerType == LayerTypes.VectorLayer)
            {
                frmLabelSet frm = new frmLabelSet(aLN.MapFrame.MapView);
                frm.Layer = (VectorLayer)aLayer;
                frm.Show();
            }
        }

        private void OnPropertiesMenuClick(object sender, EventArgs e)
        {
            LayerNode aLN = (LayerNode)_selectedNode;
            MapLayer aLayerObj = aLN.MapFrame.MapView.GetLayerFromHandle(aLN.LayerHandle);
            if (FrmLayerProp == null)
            {
                FrmLayerProp = new frmLayerProperty(aLayerObj, aLN.MapFrame);
                FrmLayerProp.Legend = this;
                FrmLayerProp.Show(this);
            }
            else
            {
                FrmLayerProp.MapLayer = aLayerObj;
                FrmLayerProp.Legend = this;
                FrmLayerProp.Activate();
            }
        }

        private void OnAddGroupClick(object sender, EventArgs e)
        {
            _currentMapFrame.AddNewGroup("New Group");
            this.Invalidate();
        }

        private void OnRemoveGroupClick(object sender, EventArgs e)
        {
            if (_selectedNode.NodeType == NodeTypes.GroupNode)
            {
                GroupNode aGN = (GroupNode)_selectedNode;
                aGN.MapFrame.RemoveGroup(aGN);
            }
            //GroupNode aGN = GetGroupByHandle(_SelectedGroup);
            //RemoveGroup(aGN);
            this.Invalidate();
        }

        private void OnAddLayerClick(object sender, EventArgs e)
        {
            OpenFileDialog aDlg = new OpenFileDialog();
            aDlg.Filter = "Supported Formats|*.shp;*.wmp;*.bln;*.bmp;*.gif;*.jpg;*.tif;*.png|Shape File (*.shp)|*.shp|WMP File (*.wmp)|*.wmp|BLN File (*.bln)|*.bln|" +
                "Bitmap Image (*.bmp)|*.bmp|Gif Image (*.gif)|*.gif|Jpeg Image (*.jpg)|*.jpg|Tif Image (*.tif)|*.tif|Png Iamge (*.png)|*.png|All Files (*.*)|*.*";

            if (aDlg.ShowDialog() == DialogResult.OK)
            {
                string aFile = aDlg.FileName;
                //Add layer
                MapLayer aLayer = _currentMapFrame.OpenLayer(aFile);
            }
        }

        private void OnSaveLayerClick(object sender, EventArgs e)
        {
            if (_selectedNode.NodeType == NodeTypes.LayerNode)
            {
                LayerNode aLN = (LayerNode)_selectedNode;
                VectorLayer aLayer = (VectorLayer)aLN.MapFrame.MapView.GetLayerFromHandle(aLN.LayerHandle);
                if (aLN.MapFrame.MapView.Projection.ProjInfo.ToProj4String() == aLayer.ProjInfo.ToProj4String())
                {
                    aLayer.SaveFile();
                }
                else
                {
                    //VectorLayer bLayer = (VectorLayer)aLN.MapFrame.MapView.GetGeoLayerFromHandle(aLN.LayerHandle);
                    VectorLayer bLayer = (VectorLayer)aLayer.Clone();
                    bLayer.SaveFile();
                    aLayer.FileName = bLayer.FileName;
                }
            }
        }

        private void OnAddMapFrameClick(object sender, EventArgs e)
        {
            MapFrame aMF = new MapFrame();
            aMF.Order = _mapFrames.Count;
            aMF.Text = GetNewMapFrameName();
            AddMapFrame(aMF);
            this.Invalidate();
        }

        private void OnRemoveMapFrameClick(object sender, EventArgs e)
        {
            RemoveMapFrame(_currentMapFrame);
        }

        private void OnMapFrameActiveClick(object sender, EventArgs e)
        {
            SetActiveMapFrame(_currentMapFrame);
        }

        /// <summary>
        /// Fires the MapFrameMouseClick event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected void OnMapFrameMouseClick(MouseEventArgs e)
        {
            if (MapFrameMouseClick != null) MapFrameMouseClick(this, e);

            bool inExpansionBox = false;
            bool inCheckBox = false;
            MapFrame aNode = (MapFrame)GetNodeByPosition(e.X, e.Y, ref inCheckBox, ref inExpansionBox);
            _currentMapFrame = GetMapFrame(aNode);
            if (inExpansionBox)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (aNode.IsExpanded)
                        aNode.Collapse();
                    else
                        aNode.Expand();

                    this.Refresh();
                }
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenuStrip mnuLayer = new ContextMenuStrip();
                    //mnuLayer.Items.Add("New Map Frame");
                    //mnuLayer.Items[mnuLayer.Items.Count - 1].Click += new EventHandler(OnAddMapFrameClick);
                    mnuLayer.Items.Add("New Group");
                    mnuLayer.Items[mnuLayer.Items.Count - 1].Click += new EventHandler(OnAddGroupClick);
                    mnuLayer.Items.Add("Add Layer");
                    mnuLayer.Items[mnuLayer.Items.Count - 1].Click += new EventHandler(OnAddLayerClick);
                    mnuLayer.Items.Add(new ToolStripSeparator());
                    mnuLayer.Items.Add("Active");
                    mnuLayer.Items[mnuLayer.Items.Count - 1].Click += new EventHandler(OnMapFrameActiveClick);
                    if (_mapFrames.Count > 1)
                    {
                        mnuLayer.Items.Add(new ToolStripSeparator());
                        mnuLayer.Items.Add("Remove Map Frame");
                        mnuLayer.Items[mnuLayer.Items.Count - 1].Click += new EventHandler(OnRemoveMapFrameClick);
                    }

                    Point aPoint = new Point(0, 0);
                    aPoint.X = e.X;
                    aPoint.Y = e.Y;
                    mnuLayer.Show(this, aPoint);
                }
            }
        }

        /// <summary>
        /// Fires the GroupMouseClick event
        /// </summary>
        protected void OnGroupMouseClick(MouseEventArgs e)
        {
            if (GroupMouseClick != null) GroupMouseClick(this, e);

            bool inExpansionBox = false;
            bool inCheckBox = false;
            GroupNode aNode = (GroupNode)GetNodeByPosition(e.X, e.Y, ref inCheckBox, ref inExpansionBox);
            if (inExpansionBox)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (aNode.IsExpanded)
                        aNode.Collapse();
                    else
                        aNode.Expand();

                    this.Invalidate();
                }
            }
            else if (inCheckBox)
            {
                if (e.Button == MouseButtons.Left)
                {
                    switch (aNode.CheckStatus)
                    {
                        case 0:
                        case 2:
                            aNode.CheckStatus = 1;
                            aNode.Checked = true;
                            break;
                        default:
                            aNode.CheckStatus = 0;
                            aNode.Checked = false;
                            break;
                    }

                    foreach (LayerNode aLN in aNode.Layers)
                    {
                        aLN.Checked = aNode.Checked;
                        MapLayer aLayer = aLN.MapFrame.MapView.GetLayerFromHandle(aLN.LayerHandle);
                        aLayer.Visible = aNode.Checked;
                    }

                    this.Invalidate();
                    aNode.MapFrame.RaiseLayersUndateEvent();
                    aNode.MapFrame.MapView.PaintLayers();
                    //OnLayersUpdated();
                    //_MapView.PaintLayers();
                }
            }
            else
            {
                //_SelectedGroup = ((GroupNode)aNode).GroupHandle;

                if (e.Button == MouseButtons.Right)
                {
                    _currentMapFrame = GetMapFrame(aNode);
                    ContextMenuStrip mnuLayer = new ContextMenuStrip();
                    mnuLayer.Items.Add("New Group");
                    mnuLayer.Items[mnuLayer.Items.Count - 1].Click += new EventHandler(OnAddGroupClick);
                    mnuLayer.Items.Add("Remove Group");
                    mnuLayer.Items[mnuLayer.Items.Count - 1].Click += new EventHandler(OnRemoveGroupClick);

                    Point aPoint = new Point(0, 0);
                    aPoint.X = e.X;
                    aPoint.Y = e.Y;
                    mnuLayer.Show(this, aPoint);
                }
            }
        }

        /// <summary>
        /// Fires LayerMouseClick event
        /// </summary>
        /// <param name="e">mouse event args</param>
        protected void OnLayerMouseClick(MouseEventArgs e)
        {
            if (LayerMouseClick != null) LayerMouseClick(this, e);

            bool inExpansionBox = false;
            bool inCheckBox = false;
            ItemNode aNode = GetNodeByPosition(e.X, e.Y, ref inCheckBox, ref inExpansionBox);
            LayerNode aLN = (LayerNode)aNode;
            //int handle = aLN.LayerHandle;
            //MapLayer aLayerObj = aLN.MapFrame.MapView.GetLayerFromHandle(handle);
            MapLayer aLayerObj = aLN.MapLayer;
            if (inExpansionBox)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (aNode.IsExpanded)
                        aNode.Collapse();
                    else
                        aNode.Expand();

                    aLayerObj.Expanded = aNode.IsExpanded;
                    this.Invalidate();

                    aLN.MapFrame.RaiseLayersUndateEvent();
                }
            }
            else if (inCheckBox)
            {
                if (e.Button == MouseButtons.Left)
                {
                    aNode.Checked = !aNode.Checked;
                    aLayerObj.Visible = aNode.Checked;
                    this.Invalidate();

                    aLN.MapFrame.RaiseLayersUndateEvent();
                    aLN.MapFrame.MapView.PaintLayers();
                    //_MapView.PaintLayers();
                }
            }
            else
            {
                //SelectLayer(aNode as LayerNode);

                if (e.Button == MouseButtons.Right)
                {
                    ContextMenuStrip mnuLayer = new ContextMenuStrip();
                    Stream myStream;
                    Bitmap image;

                    //Remove and save layer
                    mnuLayer.Items.Add("Remove Layer");
                    mnuLayer.Items[mnuLayer.Items.Count - 1].Click += new EventHandler(OnRemoveLayerClick);
                    if (aLayerObj.LayerType == LayerTypes.VectorLayer)
                    {
                        VectorLayer aLayer = (VectorLayer)aLayerObj;
                        if (!System.IO.File.Exists(aLayer.FileName))
                        {
                            mnuLayer.Items.Add("Save Layer");
                            mnuLayer.Items[mnuLayer.Items.Count - 1].Click += new EventHandler(OnSaveLayerClick);
                        }
                    }

                    mnuLayer.Items.Add(new ToolStripSeparator());

                    //Attribute table
                    if (aLayerObj.LayerType == LayerTypes.VectorLayer)
                    {
                        ToolStripMenuItem attrTableMenu = new ToolStripMenuItem("Attribute Table");
                        myStream = this.GetType().Assembly.GetManifestResourceStream("MeteoInfoC.Resources.TSB_ViewData.png");
                        if (myStream != null)
                        {
                            image = new Bitmap(myStream);
                            attrTableMenu.Image = image;
                        }
                        attrTableMenu.Click += new EventHandler(OnAttrTableMenuClick);
                        mnuLayer.Items.Add(attrTableMenu);

                        mnuLayer.Items.Add(new ToolStripSeparator());
                    }

                    //Zoom to layer
                    ToolStripMenuItem zoomLayerMenu = new ToolStripMenuItem("Zoom to Layer");
                    myStream = this.GetType().Assembly.GetManifestResourceStream("MeteoInfoC.Resources.TSB_ZoomToLayer.png");
                    if (myStream != null)
                    {
                        image = new Bitmap(myStream);
                        zoomLayerMenu.Image = image;
                    }
                    zoomLayerMenu.Click += new EventHandler(OnZoomLayerMenuClick);
                    mnuLayer.Items.Add(zoomLayerMenu);

                    //Visible scale
                    ToolStripMenuItem visScaleMenu = new ToolStripMenuItem("Visible Scale");
                    mnuLayer.Items.Add(visScaleMenu);
                    ToolStripMenuItem minVisScaleMenu = new ToolStripMenuItem("Set Minimum Scale");
                    minVisScaleMenu.Click += new EventHandler(OnMinVisScaleClick);
                    visScaleMenu.DropDownItems.Add(minVisScaleMenu);
                    ToolStripMenuItem maxVisScaleMenu = new ToolStripMenuItem("Set Maximum Scale");
                    maxVisScaleMenu.Click += new EventHandler(OnMaxVisScaleClick);
                    visScaleMenu.DropDownItems.Add(maxVisScaleMenu);
                    ToolStripMenuItem removeVisScaleMenu = new ToolStripMenuItem("Remove Visible Scale");
                    removeVisScaleMenu.Click += new EventHandler(OnRemoveVisScaleClick);
                    visScaleMenu.DropDownItems.Add(removeVisScaleMenu);
                    if (aLayerObj.VisibleScale.IsVisibleScaleEnabled())
                        removeVisScaleMenu.Enabled = true;
                    else
                        removeVisScaleMenu.Enabled = false;

                    mnuLayer.Items.Add(new ToolStripSeparator());

                    //Label
                    if (aLayerObj.LayerType == LayerTypes.VectorLayer)
                    {
                        ToolStripMenuItem labelMenu = new ToolStripMenuItem("Label");
                        myStream = this.GetType().Assembly.GetManifestResourceStream("MeteoInfoC.Resources.TSB_LabelSet.png");
                        if (myStream != null)
                        {
                            image = new Bitmap(myStream);
                            labelMenu.Image = image;
                        }
                        labelMenu.Click += new EventHandler(OnLabelMenuClick);
                        mnuLayer.Items.Add(labelMenu);

                        mnuLayer.Items.Add(new ToolStripSeparator());
                    }

                    //Properties
                    ToolStripMenuItem propertiesMenu = new ToolStripMenuItem("Properties");
                    myStream = this.GetType().Assembly.GetManifestResourceStream("MeteoInfoC.Resources.Properties.png");
                    if (myStream != null)
                    {
                        image = new Bitmap(myStream);
                        propertiesMenu.Image = image;
                    }
                    propertiesMenu.Click += new EventHandler(OnPropertiesMenuClick);
                    mnuLayer.Items.Add(propertiesMenu);

                    //Show context menu
                    Point aPoint = new Point(0, 0);
                    aPoint.X = e.X;
                    aPoint.Y = e.Y;
                    mnuLayer.Show(this, aPoint);
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (FrmLayerProp != null)
                    {
                        //LayerNode aLN = (LayerNode)aNode;                        
                        object aLayerSet = new object();
                        switch (aLayerObj.LayerType)
                        {
                            case LayerTypes.VectorLayer:
                                aLayerSet = new VectorLayerSet((VectorLayer)aLayerObj, this);
                                break;
                            case LayerTypes.ImageLayer:
                                aLayerSet = new ImageLayerSet((ImageLayer)aLayerObj, this);
                                break;
                            case LayerTypes.RasterLayer:
                                aLayerSet = new RasterLayerSet((RasterLayer)aLayerObj, this);
                                break;
                        }

                        FrmLayerProp.Activate();
                        //FrmLayerProp.SetObject(aLayerSet);
                        FrmLayerProp.MapLayer = aLayerObj;
                    }
                }
            }
        }

        /// <summary>
        /// Fires the ActiveMapFrameChanged event
        /// </summary>
        protected virtual void OnActiveMapFrameChanged()
        {
            if (ActiveMapFrameChanged != null) ActiveMapFrameChanged(this, new EventArgs());
            this.Invalidate();
        }

        /// <summary>
        /// Fires the MapFramesUpdated event
        /// </summary>
        protected virtual void OnMapFramesUpdated()
        {
            if (MapFramesUpdated != null) MapFramesUpdated(this, new EventArgs());

            if (_mapLayout != null)
            {
                _mapLayout.UpdateMapFrames(_mapFrames);
                if (_isLayoutView)
                {
                    foreach (MapFrame aMF in _mapFrames)
                        aMF.IsFireMapViewUpdate = true;

                    _mapLayout.PaintGraphics();
                }
            }
        }

        /// <summary>
        /// Set if layer node checked
        /// </summary>
        /// <param name="layerNode">layer node</param>
        /// <param name="isCheck">is check</param>
        public void CheckLayerNode(LayerNode layerNode, bool isCheck)
        {
            MapLayer aLayer = layerNode.MapFrame.MapView.GetLayerFromHandle(layerNode.LayerHandle);
            layerNode.Checked = isCheck;
            aLayer.Visible = isCheck;

            layerNode.MapFrame.RaiseLayersUndateEvent();
        }

        void MapFrameLayerUpdated(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Layout_ActiveMapFrameChanged(object sender, EventArgs e)
        {
            if (_mapLayout != null)
                this.SetActiveMapFrame(_mapLayout.ActiveMapFrame);
        }

        private void Layout_MapFramesUpdated(object sender, EventArgs e)
        {
            if (_mapLayout != null)
            {
                _mapFrames = _mapLayout.MapFrames;
                this.Refresh();
            }
        }

        # endregion
    }

    internal class Constants
    {
        //Item
        public static int ITEM_HEIGHT = 15;
        public static int ITEM_PAD = 2;
        public static int ITEM_RIGHT_PAD = 10;
        public static int ITEM_LEFT_PAD = 15;
        //Map frame
        public static int MAPFRAME_LEFT_PAD = 5;
        //  TEXT
        public static int TEXT_HEIGHT = 14;
        public static int TEXT_TOP_PAD = 3;
        public static int TEXT_LEFT_PAD = 6;
        public static int TEXT_RIGHT_PAD = 25;
        public static int TEXT_RIGHT_PAD_NO_ICON = 8;
        //  CHECK BOX
        public static int CHECK_TOP_PAD = 2;
        public static int CHECK_LEFT_PAD = 4;
        public static int CHECK_BOX_SIZE = 10;
        //  EXPANSION BOX
        public static int EXPAND_BOX_TOP_PAD = 4;
        public static int EXPAND_BOX_LEFT_PAD = 3;
        public static int EXPAND_BOX_SIZE = 8;
        //  GROUP
        public static int GRP_INDENT = 3;
        //	LIST ITEMS
        public static int LIST_ITEM_INDENT = 18;
        public static int ICON_RIGHT_PAD = 25;
        public static int ICON_TOP_PAD = 3;
        public static int ICON_SIZE = 13;
        //	CONNECTION LINES FROM GROUPS TO SUB ITEMS
        public static int VERT_LINE_INDENT = (GRP_INDENT + 7);
        public static int VERT_LINE_GRP_TOP_OFFSET = 14;
        //	COLOR SCHEME CONSTANTS
        public static int CS_ITEM_HEIGHT = 14;
        public static int CS_TOP_PAD = 1;
        public static int CS_PATCH_WIDTH = 15;
        public static int CS_PATCH_HEIGHT = 12;
        public static int CS_PATCH_LEFT_INDENT = (CHECK_LEFT_PAD);
        public static int CS_TEXT_LEFT_INDENT = (CS_PATCH_LEFT_INDENT + CS_PATCH_WIDTH + 3);
        public static int CS_TEXT_TOP_PAD = 3;
        //	SCROLLBAR
        public static int SCROLL_WIDTH = 15;
    }
}