using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Shape;
using MeteoInfoC.Legend;
using MeteoInfoC.Map;
using MeteoInfoC.Layout;

namespace MeteoInfoC
{
    /// <summary>
    /// Property form
    /// </summary>
    public partial class frmProperty : Form
    {
        #region Variables
        private ShapeTypes m_ShapeType = new ShapeTypes();
        //LayersLegend m_LayersLegend = new LayersLegend();
        private object _parent = null;
        private bool _isApply = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public frmProperty()
        {
            InitializeComponent();

            B_Apply.Visible = false;
            B_OK.Visible = false;
        }

        /// <summary>
        /// Constructro
        /// </summary>
        /// <param name="apply"></param>
        public frmProperty(bool apply)
        {
            InitializeComponent();

            _isApply = true;
            propertyGrid1.Dock = DockStyle.None;
            //propertyGrid1.Width = this.Width - 5;
            propertyGrid1.Height = this.Height - (this.Bottom - B_OK.Top) - 10;
        }

        #endregion

        #region Methods

        private void frmProperty_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Set object
        /// </summary>
        /// <param name="aObj"></param>
        public void SetObject(object aObj)
        {
            propertyGrid1.SelectedObject = aObj;
        }

        /// <summary>
        /// Set parent
        /// </summary>
        /// <param name="parent">parent object</param>
        public void SetParent(object parent)
        {
            _parent = parent;
        }

        ///// <summary>
        ///// Set layersLegend
        ///// </summary>
        ///// <param name="aLL"></param>
        //public void SetLayersLegend(LayersLegend aLL)
        //{
        //    m_LayersLegend = aLL;
        //}

        /// <summary>
        /// Set ShapeSet
        /// </summary>
        /// <param name="aST"></param>
        public void SetShapeSet(ShapeTypes aST)
        {
            m_ShapeType = aST;
        }

        private void frmProperty_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_parent != null)
            {
                if (_parent.GetType() == typeof(LayersLegend))
                    ((LayersLegend)_parent).FrmLayerProp = null;
                //else if (_parent.GetType() == typeof(frmLegendSet))
                //    ((frmLegendSet)_parent).m_FrmSS = null;
            }

            //switch (Text)
            //{
            //    case "Symbol Set":
            //        frmLegendSet.m_FrmSS = null;
            //        break;
            //    case "Layer Set":                    
            //        //m_LayersLegend.FrmLayerProp = null;
            //        break;
            //}
        }

        #endregion

        private void B_Apply_Click(object sender, EventArgs e)
        {
            ParentRedraw();
        }

        private void B_OK_Click(object sender, EventArgs e)
        {
            ParentRedraw();

            this.Close();
        }

        private void ParentRedraw()
        {
            if (_parent != null)
            {
                if (_parent.GetType() == typeof(LayersLegend))
                    ((LayersLegend)_parent).Refresh();
                else if (_parent.GetType() == typeof(frmLegendSet))
                    ((frmLegendSet)_parent).Refresh();
                else if (_parent.GetType() == typeof(MapView))
                    ((MapView)_parent).PaintLayers();
                else if (_parent.GetType() == typeof(MapLayout))
                    ((MapLayout)_parent).PaintGraphics();
            }
        }

        private void frmProperty_Resize(object sender, EventArgs e)
        {
            if (_isApply)
            {
                propertyGrid1.Width = this.Width;
                propertyGrid1.Height = this.Height - (this.Bottom - B_OK.Top) - 10;
            }
        }
    }
}
