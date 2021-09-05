using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Layer;
using MeteoInfoC.Shape;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Layer node
    /// </summary>
    public class LayerNode:ItemNode
    {
        #region Variables
        private MapLayer _mapLayer = null;        
        private int _GroupHandle = -1;               
        private List<LegendNode> _legendNodes = new List<LegendNode>();
        private MapFrame _mapFrame = null; 
        
        #endregion

        #region Constructor
        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public LayerNode(string name):base()
        //{            
        //    Text = name;                      
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aLayer">map layer</param>
        public LayerNode(MapLayer aLayer):base()
        {
            _mapLayer = aLayer;            
            Text = aLayer.LayerName;            
            Checked = aLayer.Visible;
            NodeType = NodeTypes.LayerNode;
            //UpdateLegendScheme(aLayer.LegendScheme);
        }
        
        #endregion

        #region Properties
        /// <summary>
        /// Get or set map frame
        /// </summary>
        public MapFrame MapFrame
        {
            get { return _mapFrame; }
            set { _mapFrame = value; }
        }

        /// <summary>
        /// Get or set map layer
        /// </summary>
        public MapLayer MapLayer
        {
            get { return _mapLayer; }
            set { _mapLayer = value; }
        }

        /// <summary>
        /// Get layer handle
        /// </summary>
        public int LayerHandle
        {
            get { return _mapLayer.Handle; }            
        }        

        /// <summary>
        /// Get or set group handle
        /// </summary>
        public int GroupHandle
        {
            get { return _GroupHandle; }
            set { _GroupHandle = value; }
        }

        ///// <summary>
        ///// Get or set layer visible
        ///// </summary>
        //public bool LayerVisible
        //{
        //    get { return _LayerVisible; }
        //    set { _LayerVisible = value; }
        //}       

        /// <summary>
        /// Get layer type
        /// </summary>
        public LayerTypes LayerType
        {
            get { return _mapLayer.LayerType; }            
        }

        /// <summary>
        /// Get legend nodes
        /// </summary>
        public List<LegendNode> LegendNodes
        {
            get { return _legendNodes; }     
        }

        /// <summary>
        /// Get shape type
        /// </summary>
        public ShapeTypes ShapeType
        {
            get { return _mapLayer.ShapeType; }            
        }

        /// <summary>
        /// Get legend scheme
        /// </summary>
        public LegendScheme LegendScheme
        {
            get { return _mapLayer.LegendScheme; }
        }

        /// <summary>
        /// Set layer visible
        /// </summary>
        public override bool Checked
        {
            get { return _mapLayer.Visible; }
            set 
            {
                base.Checked = value;
                _mapLayer.Visible = value; 
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Update properties
        /// </summary>
        public void Update()
        {
            Text = _mapLayer.LayerName;
            Checked = _mapLayer.Visible;
            UpdateLegendScheme(_mapLayer.LegendScheme);
        }

        /// <summary>
        /// Update legend nodes using a legend scheme
        /// </summary>
        /// <param name="aLS">legend scheme</param>
        public void UpdateLegendScheme(LegendScheme aLS)
        {            
            if (_mapLayer.LayerType != LayerTypes.ImageLayer)
            {
                _legendNodes.Clear();
                LegendNode aTN = new LegendNode();
                for (int i = 0; i < aLS.BreakNum; i++)
                {
                    if (aLS.LegendBreaks[i].DrawShape)
                    {
                        aTN = new LegendNode();
                        aTN.ShapeType = ShapeType;
                        aTN.LegendBreak = aLS.LegendBreaks[i];
                        _legendNodes.Add(aTN);
                    }
                }

                if (_mapLayer.LayerType == LayerTypes.VectorLayer)
                {
                    VectorLayer aLayer = (VectorLayer)_mapLayer;
                    if (aLayer.ChartSet.DrawCharts && aLayer.ChartPoints.Count > 0)
                    {
                        LegendNode aLN = new LegendNode();
                        aLN.ShapeType = ShapeTypes.Polygon;
                        ChartBreak aCB = ((ChartBreak)aLayer.ChartPoints[0].Legend).GetSampleChartBreak();                        
                        aLN.LegendBreak = aCB;
                        aLN.Height = ((ChartBreak)aLN.LegendBreak).GetHeight() + 10;
                        _legendNodes.Add(aLN);
                        for (int i = 0; i < aLayer.ChartSet.LegendScheme.BreakNum; i++)
                        {
                            aLN = new LegendNode();
                            aLN.ShapeType = ShapeTypes.Polygon;
                            aLN.LegendBreak = aLayer.ChartSet.LegendScheme.LegendBreaks[i];
                            _legendNodes.Add(aLN);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Override GetExpandedHeight method
        /// </summary>
        /// <returns>expanded height</returns>
        public override int GetExpandedHeight()
        {
            int height = Height;
            foreach (LegendNode legNode in _legendNodes)
            {
                height += legNode.Height + Constants.ITEM_PAD;
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
        /// Clone LayerNode
        /// </summary>
        /// <returns>LayerNode</returns>
        public override object Clone()
        {
            LayerNode aLN = new LayerNode((MapLayer)_mapLayer.Clone());            
            if (this.IsExpanded)
                aLN.Expand();

            if (_legendNodes.Count > 0)
            {
                foreach (LegendNode aLegNode in _legendNodes)
                    aLN.LegendNodes.Add(aLegNode.Clone() as LegendNode);
            }

            return aLN;
        }

        #endregion
    }
}
