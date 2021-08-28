using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Layer;
using MeteoInfoC.Shape;
using MeteoInfoC.Global;
using MeteoInfoC.Map;
using MeteoInfoC.Legend;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// frmLabelSet class
    /// </summary>
    public partial class frmLabelSet : Form
    {
        #region Variables
        private VectorLayer _layer;
        private Font _font = new Font("Arial", 7);
        private Color _color;
        private Color _shadowColor;
        private MapView _mapView;
        private bool _isLoading = false;

        #endregion

        #region Constructor
        //public frmLabelSet()
        //{
        //    InitializeComponent();

        //    pCurrenWin = this;
        //    _mapView = null;
        //}


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aMapView"></param>
        public frmLabelSet(MapView aMapView)
        {
            InitializeComponent();

            _mapView = aMapView;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set layer
        /// </summary>
        public VectorLayer Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }

        #endregion

        #region Methods

        private void frmLabelSet_Load(object sender, EventArgs e)
        {
            _isLoading = true;

            //_layer = (VectorLayer)frmMain.G_LayersLegend.GetLayerFromHandle(TheLayerHandle);
            _font = _layer.LabelSet.LabelFont;
            _color = _layer.LabelSet.LabelColor;
            _shadowColor = _layer.LabelSet.ShadowColor;
            
            int i;

            //Set fields
            CB_Field.Items.Clear();
            for (i = 0; i < _layer.NumFields; i++)
            {
                CB_Field.Items.Add(_layer.GetFieldName(i));
            }
            if (CB_Field.Items.Count > 0)
            {
                if (_layer.LabelSet.FieldName != "")
                {
                    CB_Field.Text = _layer.LabelSet.FieldName;
                }
                else
                {
                    CB_Field.SelectedIndex = 0;
                }
            }

            //Set align type
            CB_Align.Items.Clear();
            foreach (string align in Enum.GetNames(typeof(AlignType)))
            {
                CB_Align.Items.Add(align);
            }
            CB_Align.Text = _layer.LabelSet.LabelAlignType.ToString();

            //Set offset
            TB_XOffset.Text = _layer.LabelSet.XOffset.ToString();
            TB_YOffset.Text = _layer.LabelSet.YOffset.ToString();

            //Set avoid collision
            CHB_Collision.Checked = _layer.LabelSet.AvoidCollision;

            //Set draw shadow
            CHB_DrawShadow.Checked = _layer.LabelSet.DrawShadow;

            //Set color by legend
            CHB_ColorByLegend.Checked = _layer.LabelSet.ColorByLegend;

            //Set contour dynamic label
            CHB_ContourDynamic.Checked = _layer.LabelSet.DynamicContourLabel;
            if (_layer.ShapeType == ShapeTypes.Polyline)
                CHB_ContourDynamic.Enabled = true;
            else
                CHB_ContourDynamic.Enabled = false;

            //Set auto decimal digits
            CHB_AutoDecimal.Checked = _layer.LabelSet.AutoDecimal;
            AutoDecimal_CheckedChanged();

            _isLoading = false;
        }
        
        //public void UpdateLabels(VectorLayer aLayer)
        //{
        //    aLayer.ClearLabels();
        //    ArrayList shapeList = new ArrayList();
        //    shapeList = (ArrayList)aLayer.ShapeList.Clone();
        //    int shapeIdx = 0;
        //    PointF aPoint = new PointF();

        //    switch (aLayer.ShapeType)
        //    {
        //        case ShapeTypes.Point:                    
        //            foreach (PointShape aPS in shapeList)
        //            {
        //                LabelPoint aLP = new LabelPoint();
        //                aPoint.X = (Single)aPS.Point.X;
        //                aPoint.Y = (Single)aPS.Point.Y;
        //                aLP.PointPos = aPoint;
        //                aLP.Text = aLayer.GetCellValue(aLayer.LabelSetV.FieldName, shapeIdx).ToString();
        //                aLayer.AddLabel(aLP);
        //                shapeIdx += 1;
        //            }                    
        //            break;
        //        case ShapeTypes.Polygon:
        //            foreach (PolygonShape aPGS in shapeList)
        //            {
        //                LabelPoint aLP = new LabelPoint();
        //                Extent aExtent = aPGS.extent;
        //                aPoint.X = (Single)((aExtent.minX + aExtent.maxX) / 2);
        //                aPoint.Y = (Single)((aExtent.minY + aExtent.maxY) / 2);
        //                aLP.PointPos = aPoint;
        //                aLP.Text = aLayer.GetCellValue(aLayer.LabelSetV.FieldName, (int)aPGS.lowValue).ToString();
        //                aLayer.AddLabel(aLP);
        //                shapeIdx += 1;
        //            }
        //            break;
        //        case ShapeTypes.Polyline:                    
        //            foreach (PolylineShape aPLS in shapeList)
        //            {
        //                LabelPoint aLP = new LabelPoint();
        //                int pIdx = aPLS.points.Count / 2;
        //                //Single angle = (Single)(Math.Atan((((wContour.Contour.Point)aPLS.points[pIdx]).y - ((wContour.Contour.Point)aPLS.points[pIdx - 1]).y) / 
        //                    //(((wContour.Contour.Point)aPLS.points[pIdx]).x - ((wContour.Contour.Point)aPLS.points[pIdx - 1]).x)) * 180 / Math.PI);
        //                aPoint.X = (Single)((wContour.Point)aPLS.points[pIdx - 1]).X;
        //                aPoint.Y = (Single)((wContour.Point)aPLS.points[pIdx - 1]).Y;
        //                aLP.PointPos = aPoint;
        //                //aLP.Angle = angle;
        //                aLP.Text = aLayer.GetCellValue(aLayer.LabelSetV.FieldName, (int)aPLS.value).ToString();
        //                aLayer.AddLabel(aLP);
        //                shapeIdx += 1;
        //            }
        //            break;
        //    }

        //    frmMain.G_LayersLegend.ActiveMapFrame.MapView.PaintLayers();
        //}

        private void B_DrawLabels_Click(object sender, EventArgs e)
        {
            _layer.RemoveLabels();
            UpdateLabelSet();
            AddLabels();

            _mapView.PaintLayers();
        }

        private void B_ClearLabels_Click(object sender, EventArgs e)
        {
            _layer.RemoveLabels();
            if (!_layer.LabelSet.DynamicContourLabel)
                _layer.LabelSet.DrawLabels = false;

            _mapView.PaintLayers();
        }

        private void B_Font_Click(object sender, EventArgs e)
        {
            FontDialog aFDlg = new FontDialog();
            aFDlg.Font = _font;
            if (aFDlg.ShowDialog() == DialogResult.OK)
            {
                _font = aFDlg.Font;
                UpdateLabelSet();
                UpdateLabelsFontColor();
                _mapView.PaintLayers();
            }
        }

        private void B_Color_Click(object sender, EventArgs e)
        {
            ColorDialog aCDlg = new ColorDialog();
            aCDlg.Color = _color;
            if (aCDlg.ShowDialog() == DialogResult.OK)
            {
                _color = aCDlg.Color;
                UpdateLabelSet();
                UpdateLabelsFontColor();
                _mapView.PaintLayers();
            }
        }

        private void B_ShadowColor_Click(object sender, EventArgs e)
        {
            ColorDialog aCDlg = new ColorDialog();
            aCDlg.Color = _shadowColor;
            if (aCDlg.ShowDialog() == DialogResult.OK)
            {
                _shadowColor = aCDlg.Color;
                UpdateLabelSet();
                _mapView.PaintLayers();
            }
        }

        private void CHB_ColorByLegend_CheckedChanged(object sender, EventArgs e)
        {
            if (CHB_ColorByLegend.Checked)
                B_Color.Enabled = false;
            else
                B_Color.Enabled = true;
        }

        private void B_AddLabels_Click(object sender, EventArgs e)
        {
            UpdateLabelSet();
            AddLabels();
            
            _mapView.PaintLayers();
        }

        private void UpdateLabelSet()
        {
            _layer.LabelSet.FieldName = CB_Field.Text;
            _layer.LabelSet.AvoidCollision = CHB_Collision.Checked;
            _layer.LabelSet.LabelAlignType = (AlignType)Enum.Parse(typeof(AlignType), CB_Align.Text, true);
            _layer.LabelSet.XOffset = int.Parse(TB_XOffset.Text);
            _layer.LabelSet.YOffset = int.Parse(TB_YOffset.Text);
            _layer.LabelSet.LabelFont = _font;
            _layer.LabelSet.LabelColor = _color;
            _layer.LabelSet.DrawShadow = CHB_DrawShadow.Checked;
            _layer.LabelSet.ShadowColor = _shadowColor;
            _layer.LabelSet.DrawLabels = true;
            _layer.LabelSet.ColorByLegend = CHB_ColorByLegend.Checked;
            _layer.LabelSet.DynamicContourLabel = CHB_ContourDynamic.Checked;
            _layer.LabelSet.AutoDecimal = CHB_AutoDecimal.Checked;
            if (TB_DecimalDigits.Text != string.Empty)
                _layer.LabelSet.DecimalDigits = int.Parse(TB_DecimalDigits.Text);
        }

        private void AddLabels()
        {                        
            //Add Labels
            if (_layer.LabelSet.DynamicContourLabel)
                _layer.AddLabelsContourDynamic(_mapView.ViewExtent);
            else
                _layer.AddLabels();
        }

        private void UpdateLabelsFontColor()
        {
            foreach (Graphic lp in _layer.GetLabelPoints())
            {
                LabelBreak lb = (LabelBreak)lp.Legend;
                LabelSet labelSet = _layer.LabelSet;
                if (!labelSet.ColorByLegend)
                {
                    lb.Color = labelSet.LabelColor;
                }
                lb.Font = labelSet.LabelFont;
            }
        }

        private void CHB_ContourDynamic_CheckedChanged(object sender, EventArgs e)
        {
            //groupBox1.Enabled = !CHB_ContourDynamic.Checked;
            //_layer.LabelSetV.DynamicContourLabel = CHB_ContourDynamic.Checked;
        }

        private void CHB_Collision_CheckedChanged(object sender, EventArgs e)
        {
            _layer.LabelSet.AvoidCollision = CHB_Collision.Checked;
            if (_layer.GetLabelPoints().Count > 0)
                _mapView.PaintLayers();
        }

        private void CB_Field_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MIMath.IsNumeric(_layer.AttributeTable.Table.Columns[CB_Field.Text]))
                CHB_AutoDecimal.Enabled = true;
            else
                CHB_AutoDecimal.Enabled = false;
        }

        private void CHB_AutoDecimal_CheckedChanged(object sender, EventArgs e)
        {
            AutoDecimal_CheckedChanged();
        }

        private void AutoDecimal_CheckedChanged()
        {
            if (CHB_AutoDecimal.Checked)
            {
                Lab_DecimalDigits.Enabled = false;
                TB_DecimalDigits.Enabled = false;                
                TB_DecimalDigits.Text = "";
            }
            else
            {
                Lab_DecimalDigits.Enabled = true;
                TB_DecimalDigits.Enabled = true;
                TB_DecimalDigits.Text = _layer.LabelSet.DecimalDigits.ToString();
            }
        }

        private void CHB_DrawShadow_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isLoading)
            {
                UpdateLabelSet();
                _mapView.PaintLayers();
            }
        }

        #endregion
    }
}
