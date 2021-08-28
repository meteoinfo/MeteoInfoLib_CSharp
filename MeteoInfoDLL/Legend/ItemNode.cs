using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Item node
    /// </summary>
    public class ItemNode:ICloneable
    {
        #region Variables
        private LayersLegend _parentLegend = null;
        private int _top;
        private int _height;        
        private bool _isExpanded;
        private bool _checked;
        private string _text;
        private Color _backColor;
        private Color _foreColor;
        private NodeTypes _nodeType;
        private bool _selected;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemNode()
        {
            _height = Constants.ITEM_HEIGHT;
            _isExpanded = false;
            _checked = true;
            _backColor = Color.White;
            _foreColor = Color.Black;
            _selected = false;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get parent layers legend
        /// </summary>
        public LayersLegend ParentLegend
        {
            get { return _parentLegend; }
        }

        /// <summary>
        /// Get or set top
        /// </summary>
        public int Top
        {
            get { return _top; }
            set { _top = value; }
        }

        /// <summary>
        /// Get or set height
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Get is expanded
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
        }

        /// <summary>
        /// Get or set is checked
        /// </summary>
        public virtual bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

        /// <summary>
        /// Get or set text
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Get or set back color
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// Get or set fore color
        /// </summary>
        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        /// <summary>
        /// Get or set node type
        /// </summary>
        public NodeTypes NodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }

        /// <summary>
        /// Get or set if is selected
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Set parentLegend
        /// </summary>
        /// <param name="aLegend">layers legend</param>
        public void SetParentLegend( LayersLegend aLegend)
        {
            _parentLegend = aLegend;
        }

        /// <summary>
        /// Expand
        /// </summary>
        public void Expand()
        {
            _isExpanded = true;
        }

        /// <summary>
        /// Collapse
        /// </summary>
        public void Collapse()
        {
            _isExpanded = false;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>object</returns>
        public virtual object Clone()
        {
            ItemNode aNode = new ItemNode();
            aNode.Height = _height;
            aNode.Checked = _checked;
            aNode.BackColor = _backColor;
            aNode.ForeColor = _foreColor;
            if (_isExpanded)
                aNode.Expand();
            else
                aNode.Collapse();
            
            aNode.Text = _text;
            aNode.Top = _top;

            return aNode;
        }

        /// <summary>
        /// Get expanded height
        /// </summary>
        /// <returns>expanded height</returns>
        public virtual int GetExpandedHeight()
        {
            return Height;
        }

        /// <summary>
        /// Get drawing height
        /// </summary>
        /// <returns>draw height</returns>
        public virtual int GetDrawHeight()
        {
            return Height;
        }

        #endregion
    }
}
