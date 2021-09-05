using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Group node
    /// </summary>
    public class GroupNode:ItemNode
    {
        #region Variables
        private int _GroupHandel;        
        private List<LayerNode> _Layers;
        private int _checkStatus = 0;
        private MapFrame _mapFrame = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public GroupNode(string name):base()
        {
            _GroupHandel = -1;
            Text = name;            
            _Layers = new List<LayerNode>();
            NodeType = NodeTypes.GroupNode;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get or set group handle
        /// </summary>
        [CategoryAttribute("Group Property"), DescriptionAttribute("Group handle")]
        [ReadOnly(true)]
        public int GroupHandle
        {
            get { return _GroupHandel; }
            set { _GroupHandel = value; }
        }

        /// <summary>
        /// Get or set group handle
        /// </summary>
        [CategoryAttribute("Group Property"), DescriptionAttribute("Group name")]        
        public string GroupName
        {
            get { return Text; }
            set 
            {
                Text = value;
                if (ParentLegend != null)
                    ParentLegend.Redraw();
            }
        }   

        /// <summary>
        /// Get or set layer name
        /// </summary>
        public List<LayerNode> Layers
        {
            get { return _Layers; }
            set { _Layers = value; }
        }

        /// <summary>
        /// Get or set check status
        /// </summary>
        public int CheckStatus
        {
            get { return _checkStatus; }
            set { _checkStatus = value; }
        }

        /// <summary>
        /// Get or set map frame
        /// </summary>
        public MapFrame MapFrame
        {
            get { return _mapFrame; }
            set { _mapFrame = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Add layer node
        /// </summary>
        /// <param name="aLayer">layer node</param>
        public void AddLayer(LayerNode aLayer)
        {            
            _Layers.Add(aLayer);
            aLayer.GroupHandle = _GroupHandel;
        }

        /// <summary>
        /// Remove a layer node
        /// </summary>
        /// <param name="aLayer">layer node</param>
        public void RemoveLayer(LayerNode aLayer)
        {
            _Layers.Remove(aLayer);
            aLayer.GroupHandle = -1;
        }

        /// <summary>
        /// Insert layer node
        /// </summary>
        /// <param name="aLayer">layer node</param>
        /// <param name="index">index</param>
        public void InsertLayer(LayerNode aLayer, int index)
        {
            _Layers.Insert(index, aLayer);
            aLayer.GroupHandle = _GroupHandel;
        }

        /// <summary>
        /// Get layer node index
        /// </summary>
        /// <param name="aLayer">layer node</param>
        /// <returns>index</returns>
        public int GetLayerIndex(LayerNode aLayer)
        {
            return _Layers.IndexOf(aLayer);
        }

        /// <summary>
        /// Update check status
        /// </summary>
        public void UpdateCheckStatus()
        {
            bool allChecked = true;
            bool hasChecked = false;
            foreach (LayerNode aLN in _Layers)
            {
                if (aLN.Checked)
                    hasChecked = true;
                else
                    allChecked = false;
            }

            if (allChecked)
                _checkStatus = 1;
            else if (hasChecked)
                _checkStatus = 2;
            else
                _checkStatus = 0;
        }

        /// <summary>
        /// Override get property object methods
        /// </summary>
        /// <returns>property object</returns>
        public object GetPropertyObject()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("GroupName", "GroupName");

            CustomProperty cp = new CustomProperty(this, objAttr);
            return cp;
        }

        /// <summary>
        /// Set properties
        /// </summary>
        public void SetProperties()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            //objAttr.Add("GroupHandle", "GroupHandle");
            objAttr.Add("GroupName", "GroupName");            
            CustomProperty cp = new CustomProperty(this, objAttr);
            frmProperty aFrmP = new frmProperty();
            aFrmP.SetObject(cp);
            aFrmP.ShowDialog();
        }

        /// <summary>
        /// Override GetExpandedHeight method
        /// </summary>
        /// <returns>expanded height</returns>
        public override int GetExpandedHeight()
        {
            int height = Height;
            foreach (LayerNode layerNode in _Layers)
            {
                int lnHeight;
                if (layerNode.IsExpanded)
                    lnHeight = layerNode.GetExpandedHeight();
                else
                    lnHeight = layerNode.Height;

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
        /// Clone GroupNode
        /// </summary>
        /// <returns>GroupNode</returns>
        public override object Clone()
        {
            GroupNode aGN = new GroupNode(Text);
            aGN.GroupHandle = _GroupHandel;            
            if (this.IsExpanded)
                aGN.Expand();

            if (_Layers.Count > 0)
            {
                foreach (LayerNode aLayerNode in _Layers)
                    _Layers.Add(aLayerNode.Clone() as LayerNode);
            }

            return aGN;
        }

        #endregion
    }
}
