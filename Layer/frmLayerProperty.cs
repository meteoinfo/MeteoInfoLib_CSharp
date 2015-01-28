using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Legend;
using MeteoInfoC.Shape;
using MeteoInfoC.Global;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// Layer property form
    /// </summary>
    public partial class frmLayerProperty : Form
    {
        #region Variables
        //private object _propObj;
        private MapFrame _mapFrame;
        private MapLayer _mapLayer;
        private LayersLegend _legend;
        private LegendScheme _legendScheme;
        private bool _isLoading = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aLayer">a map layer</param>
        /// <param name="mf">map frame</param>
        public frmLayerProperty(MapLayer aLayer, MapFrame mf)
        {
            InitializeComponent();

            _mapLayer = aLayer;
            _mapFrame = mf;
            _isLoading = true;
            Initialize();
            _isLoading = false;
        }

        private void Initialize()
        {
            PG_General.SelectedObject = _mapLayer.GetPropertyObject();
            TabControl_Prop.TabPages.Clear();
            TabControl_Prop.TabPages.Add(TabPage_General);
            switch (_mapLayer.LayerType)
            {
                case LayerTypes.VectorLayer:
                case LayerTypes.RasterLayer:
                    TabControl_Prop.TabPages.Add(TabPage_Legend);
                    _legendScheme = (LegendScheme)_mapLayer.LegendScheme.Clone();
                    legendSchemeControl1.Update(_mapLayer, _mapLayer.LegendScheme);
                    break;
                case LayerTypes.ImageLayer:
                    
                    break;
            }

            if (_mapLayer.LayerType == LayerTypes.VectorLayer)
            {
                switch (_mapLayer.ShapeType)
                {
                    case ShapeTypes.Point:
                    case ShapeTypes.PointZ:
                    case ShapeTypes.Polygon:
                        TabControl_Prop.TabPages.Add(TabPage_Chart);
                        VectorLayer aLayer = (VectorLayer)_mapLayer;
                        if (_mapLayer.ShapeType == ShapeTypes.Polygon)
                        {
                            CB_ChartType.Items.Clear();
                            CB_ChartType.Items.Add(ChartTypes.BarChart.ToString());
                            CB_ChartType.Items.Add(ChartTypes.PieChart.ToString());
                        }
                        else
                        {
                            CB_ChartType.Items.Clear();
                            CB_ChartType.Items.Add(ChartTypes.BarChart.ToString());
                            CB_ChartType.Items.Add(ChartTypes.PieChart.ToString());
                            //CB_ChartType.Items.Add(ChartTypes.WindVector.ToString());
                            //CB_ChartType.Items.Add(ChartTypes.WindBarb.ToString());
                            //CB_ChartType.Items.Add(ChartTypes.StationModel.ToString());
                        }
                        CB_ChartType.Text = aLayer.ChartSet.ChartType.ToString();

                        //Add fields                       
                        CLB_Fields.Items.Clear();
                        for (int i = 0; i < aLayer.NumFields; i++)
                        {
                            if (MIMath.IsNumeric(aLayer.GetField(i)))
                            {
                                string fn = aLayer.GetFieldName(i);
                                CLB_Fields.Items.Add(fn);
                                if (aLayer.ChartSet.FieldNames.Contains(fn))
                                    CLB_Fields.SetItemChecked(CLB_Fields.Items.Count - 1, true);
                            }
                        }

                        TB_BarWidth.Text = aLayer.ChartSet.BarWidth.ToString();
                        TB_MaxSize.Text = aLayer.ChartSet.MaxSize.ToString();
                        TB_MinSize.Text = aLayer.ChartSet.MinSize.ToString();
                        TB_XShift.Text = aLayer.ChartSet.XShift.ToString();
                        TB_YShift.Text = aLayer.ChartSet.YShift.ToString();
                        CHB_Collision.Checked = aLayer.ChartSet.AvoidCollision;
                        //Set align type
                        CB_Align.Items.Clear();
                        foreach (string align in Enum.GetNames(typeof(AlignType)))
                            CB_Align.Items.Add(align);
                        CB_Align.Text = aLayer.ChartSet.AlignType.ToString();
                        CHB_View3D.Checked = aLayer.ChartSet.View3D;
                        TB_Thickness.Text = aLayer.ChartSet.Thickness.ToString();

                        legendView_Chart.LegendScheme = aLayer.ChartSet.LegendScheme;
                        break;
                }
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set map layer
        /// </summary>
        public MapLayer MapLayer
        {
            get { return _mapLayer; }
            set 
            { 
                _mapLayer = value;
                Initialize();
            }
        }

        /// <summary>
        /// Get or set parent legend
        /// </summary>
        public LayersLegend Legend
        {
            get { return _legend; }
            set { _legend = value; }
        }

        #endregion

        #region Events

        #endregion

        private void B_Apply_Click(object sender, EventArgs e)
        {
            switch (TabControl_Prop.SelectedTab.Text)
            {
                case "Legend":
                    _mapLayer.LegendScheme = legendSchemeControl1.LegendScheme;
                    legendSchemeControl1.legendView1.Refresh();
                    break;
                case "Chart":                    
                    ((VectorLayer)_mapLayer).RemoveCharts();
                    AddCharts();
                    break;
            }
            _mapFrame.UpdateLayerNode(_mapLayer.Handle); 
            _legend.Refresh();
            _mapFrame.MapView.PaintLayers();
        }

        private void AddCharts()
        {
            if (CLB_Fields.CheckedItems.Count < 1)
                return;

            VectorLayer aLayer = (VectorLayer)_mapLayer;
            aLayer.ChartSet.ChartType = (ChartTypes)Enum.Parse(typeof(ChartTypes), CB_ChartType.Text);
            List<string> fieldNames = new List<string>();
            int i, j;
            for (i = 0; i < CLB_Fields.CheckedItems.Count; i++)
                fieldNames.Add(CLB_Fields.CheckedItems[i].ToString());

            aLayer.ChartSet.FieldNames = fieldNames;
            aLayer.ChartSet.LegendScheme = legendView_Chart.LegendScheme;
            aLayer.ChartSet.MinSize = int.Parse(TB_MinSize.Text);
            aLayer.ChartSet.MaxSize = int.Parse(TB_MaxSize.Text);
            aLayer.ChartSet.BarWidth = int.Parse(TB_BarWidth.Text);
            aLayer.ChartSet.XShift = int.Parse(TB_XShift.Text);
            aLayer.ChartSet.YShift = int.Parse(TB_YShift.Text);
            aLayer.ChartSet.AvoidCollision = CHB_Collision.Checked;
            aLayer.ChartSet.AlignType = (AlignType)Enum.Parse(typeof(AlignType), CB_Align.Text);
            aLayer.ChartSet.View3D = CHB_View3D.Checked;
            aLayer.ChartSet.Thickness = int.Parse(TB_Thickness.Text);

            List<List<float>> values = new List<List<float>>();
            List<float> minList = new List<float>();
            List<float> maxList = new List<float>();
            List<float> sumList = new List<float>();
            for (i = 0; i < aLayer.ShapeNum; i++)
            {
                List<float> vList = new List<float>();
                float sum = 0;
                float v;
                for (j = 0; j < fieldNames.Count; j++)
                {
                    v = float.Parse(aLayer.GetCellValue(fieldNames[j], i).ToString());
                    vList.Add(v);
                    sum += v;
                }
                values.Add(vList);
                minList.Add(vList.Min());
                maxList.Add(vList.Max());
                sumList.Add(sum);
            }
            
            switch (aLayer.ChartSet.ChartType)
            {
                case ChartTypes.BarChart:
                    aLayer.ChartSet.MinValue = minList.Min();
                    aLayer.ChartSet.MaxValue = maxList.Max();
                    break;
                case ChartTypes.PieChart:
                    aLayer.ChartSet.MinValue = sumList.Min();
                    aLayer.ChartSet.MaxValue = sumList.Max();
                    break;
            }

            aLayer.AddCharts();
        }

        private void frmLayerProperty_FormClosed(object sender, FormClosedEventArgs e)
        {
            _legend.FrmLayerProp = null;
        }

        private void frmLayerProperty_Load(object sender, EventArgs e)
        {

        }

        private void B_OK_Click(object sender, EventArgs e)
        {
            B_Apply.PerformClick();
            this.Close();
        }

        private void B_Cancel_Click(object sender, EventArgs e)
        {
            switch (TabControl_Prop.SelectedTab.Text)
            {
                case "Legend":
                    _mapLayer.LegendScheme = _legendScheme;
                    _mapFrame.UpdateLayerNode(_mapLayer.Handle);
                    _legend.Refresh();
                    _mapFrame.MapView.PaintLayers();
                    this.Close();
                    break;
                case "General":
                    this.Close();
                    break;
                case "Chart":
                    ((VectorLayer)_mapLayer).RemoveCharts();
                    _mapFrame.UpdateLayerNode(_mapLayer.Handle);
                    _legend.Refresh();
                    _mapFrame.MapView.PaintLayers();
                    break;
            }                                   
        }

        private void CLB_Fields_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!_isLoading)
            {
                if (CLB_Fields.SelectedItem == null)
                    return;

                string selFieldStr = CLB_Fields.SelectedItem.ToString();
                if (CLB_Fields.GetItemChecked(CLB_Fields.SelectedIndex))
                {
                    for (int i = 0; i < legendView_Chart.LegendScheme.BreakNum; i++)
                    {
                        if (legendView_Chart.LegendScheme.LegendBreaks[i].Caption == selFieldStr)
                        {
                            legendView_Chart.LegendScheme.LegendBreaks.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    PolygonBreak aPB = new PolygonBreak();
                    aPB.Caption = selFieldStr;
                    aPB.Color = LegendManage.CreateRandomColors(1)[0];
                    legendView_Chart.LegendScheme.LegendBreaks.Add(aPB);
                }
                legendView_Chart.Refresh();
            }
        }

        private void CHB_Collision_CheckedChanged(object sender, EventArgs e)
        {
            ((VectorLayer)_mapLayer).ChartSet.AvoidCollision = CHB_Collision.Checked;
            if (((VectorLayer)_mapLayer).ChartPoints.Count > 0)
                _mapFrame.MapView.PaintLayers();
        }

        private void TabControl_Prop_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TabControl_Prop.SelectedIndex)
            {
                case 0:   //General
                case 1:    //Legend
                    B_Cancel.Text = "Cancel";
                    break;
                case 2:    //Chart
                    B_Cancel.Text = "Clear";
                    break;
            }
        }

        private void CB_ChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_ChartType.Text == "PieChart")
            {
                Lab_Width.Enabled = false;
                TB_BarWidth.Enabled = false;
            }
            else
            {
                Lab_Width.Enabled = true;
                TB_BarWidth.Enabled = true;
            }
        }

    }
}
