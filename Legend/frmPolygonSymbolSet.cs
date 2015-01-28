using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Map;
using MeteoInfoC.Layout;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Polygon symbol set form
    /// </summary>
    public partial class frmPolygonSymbolSet : Form
    {
        #region Variables
        private PolygonBreak _polygonBreak;
        private object _parent;
        private bool _isLoading = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">parent object</param>
        public frmPolygonSymbolSet(object parent)
        {
            InitializeComponent();

            _parent = parent;
            if (_parent.GetType() == typeof(LegendView))
            {
                this.Height = GB_Outline.Bottom - this.Top + 40;
                //Lab_Trans.Enabled = false;
                //NUD_TransparencyPerc.Enabled = false;
                B_Apply.Visible = false;
                B_OK.Visible = false;
            }
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public frmPolygonSymbolSet()
        //{
        //    InitializeComponent();

        //    this.Height = GB_Outline.Bottom - this.Top + 40;
        //    Lab_Trans.Enabled = false;
        //    NUD_TransparencyPerc.Enabled = false;
        //    B_Apply.Visible = false;
        //    B_OK.Visible = false;
        //}

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public frmPolygonSymbolSet(bool isApply)
        //{
        //    InitializeComponent();
        //}

        #endregion

        #region Properties
        /// <summary>
        /// Get or set polygon break
        /// </summary>
        public PolygonBreak PolygonBreak
        {
            get { return _polygonBreak; }
            set
            {
                _polygonBreak = value;
                UpdateProperties();
            }
        }

        #endregion

        #region Methods

        private void frmPolygonSymbolSet_Load(object sender, EventArgs e)
        {

        }

        ///// <summary>
        ///// Set parent
        ///// </summary>
        ///// <param name="parent">parent object</param>
        //public void SetParent(object parent)
        //{
        //    _parent = parent;
        //}

        private void UpdateProperties()
        {            
            _isLoading = true;

            B_FillColor.BackColor = _polygonBreak.Color;
            ChB_DrawShape.Checked = _polygonBreak.DrawShape;
            int trans = (int)((1 - (double)_polygonBreak.Color.A / 255) * 100);
            NUD_TransparencyPerc.Value = (decimal)trans;
            ChB_DrawFill.Checked = _polygonBreak.DrawFill;
            ChB_DrawOutline.Checked = _polygonBreak.DrawOutline;
            NUD_OutlineSize.Value = (decimal)_polygonBreak.OutlineSize;
            B_OutlineColor.BackColor = _polygonBreak.OutlineColor;
            B_BackColor.BackColor = _polygonBreak.BackColor;

            symbolControl1.SymbolNumber = Enum.GetNames(typeof(HatchStyle)).Length + 1;
            if (_polygonBreak.UsingHatchStyle)
                symbolControl1.SelectedCell = Array.IndexOf(Enum.GetNames(typeof(HatchStyle)), _polygonBreak.Style.ToString()) + 1;
            else
                symbolControl1.SelectedCell = 0;

            if (symbolControl1.SelectedCell == 0)
            {
                _polygonBreak.UsingHatchStyle = false;
                B_BackColor.Enabled = false;
            }
            else
            {
                _polygonBreak.UsingHatchStyle = true;
                _polygonBreak.Style = (HatchStyle)(Enum.GetValues(typeof(HatchStyle)).GetValue(symbolControl1.SelectedCell - 1));
                B_BackColor.Enabled = true;
            }

            _isLoading = false;
        }

        #endregion

        #region Events

        private void symbolControl1_SelectedCellChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            if (symbolControl1.SelectedCell == 0)
            {
                _polygonBreak.UsingHatchStyle = false;
                B_BackColor.Enabled = false;
            }
            else
            {
                _polygonBreak.UsingHatchStyle = true;
                _polygonBreak.Style = (HatchStyle)(Enum.GetValues(typeof(HatchStyle)).GetValue(symbolControl1.SelectedCell - 1));
                B_BackColor.Enabled = true;
            }
            if (_parent.GetType() == typeof(LegendView))
            {
                ((LegendView)_parent).SetLegendBreak_UsingHatchStyle(_polygonBreak.UsingHatchStyle);
                if (_polygonBreak.UsingHatchStyle)
                    ((LegendView)_parent).SetLegendBreak_HatchStyle(_polygonBreak.Style);

            }
        }

        private void ChB_DrawShape_CheckedChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _polygonBreak.DrawShape = ChB_DrawShape.Checked;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_DrawShape(ChB_DrawShape.Checked);            
        }

        private void NUD_TransparencyPerc_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            int alpha = (int)((1 - (double)NUD_TransparencyPerc.Value / 100.0) * 255);
            _polygonBreak.Color = Color.FromArgb(alpha, _polygonBreak.Color);
            if (_polygonBreak.UsingHatchStyle)
                _polygonBreak.BackColor = Color.FromArgb(alpha, _polygonBreak.BackColor);

            if (_parent.GetType() == typeof(LegendView))
            {
                ((LegendView)_parent).SetLegendBreak_Alpha(alpha);
                if (_polygonBreak.UsingHatchStyle)
                    ((LegendView)_parent).SetLegendBreak_BackColorAlpha(alpha); 
            }
        }

        private void ChB_DrawFill_CheckedChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _polygonBreak.DrawFill = ChB_DrawFill.Checked;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_DrawFill(ChB_DrawFill.Checked);            
        }

        private void ChB_DrawOutline_CheckedChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _polygonBreak.DrawOutline = ChB_DrawOutline.Checked;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_DrawOutline(ChB_DrawOutline.Checked);            
        }

        private void B_FillColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.Color = _polygonBreak.Color;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                B_FillColor.BackColor = colorDlg.Color;
                int alpha = (int)((1 - (double)NUD_TransparencyPerc.Value / 100.0) * 255);
                _polygonBreak.Color = Color.FromArgb(alpha, colorDlg.Color);
                if (_parent.GetType() == typeof(LegendView))
                    ((LegendView)_parent).SetLegendBreak_Color(_polygonBreak.Color);                
            }
        }

        private void NUD_OutlineSize_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _polygonBreak.OutlineSize = (float)NUD_OutlineSize.Value;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_OutlineSize(_polygonBreak.OutlineSize);            
        }

        private void B_OutlineColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.Color = _polygonBreak.OutlineColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                B_OutlineColor.BackColor = colorDlg.Color;
                _polygonBreak.OutlineColor = colorDlg.Color;
                if (_parent.GetType() == typeof(LegendView))
                    ((LegendView)_parent).SetLegendBreak_OutlineColor(colorDlg.Color);                
            }
        }

        private void B_BackColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.Color = _polygonBreak.BackColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                B_BackColor.BackColor = colorDlg.Color;
                int alpha = (int)((1 - (double)NUD_TransparencyPerc.Value / 100.0) * 255);
                _polygonBreak.BackColor = Color.FromArgb(alpha, colorDlg.Color);
                if (_parent.GetType() == typeof(LegendView))
                    ((LegendView)_parent).SetLegendBreak_BackColor(_polygonBreak.BackColor);                
            }
        }

        private void ParentRedraw()
        {
            if (_parent.GetType() == typeof(MapView))
            {
                ((MapView)_parent).PaintLayers();
            }
            else if (_parent.GetType() == typeof(MapLayout))
                ((MapLayout)_parent).PaintGraphics();
        }

        private void B_Apply_Click(object sender, EventArgs e)
        {
            ParentRedraw();
        }

        private void B_OK_Click(object sender, EventArgs e)
        {
            if (_parent.GetType() == typeof(MapView))
            {
                ((MapView)_parent).DefPolygonBreak = _polygonBreak;
            }
            else if (_parent.GetType() == typeof(MapLayout))
                ((MapLayout)_parent).DefPolygonBreak = _polygonBreak;

            ParentRedraw();
            this.Close();
        }

        #endregion
    }
}
