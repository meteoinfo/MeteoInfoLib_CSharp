using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Drawing;
using MeteoInfoC.Map;
using MeteoInfoC.Layout;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Polyline symbol set form
    /// </summary>
    public partial class frmPolylineSymbolSet : Form
    {
        #region Variables
        private PolyLineBreak _polylineBreak;
        private object _parent;
        private bool _isLoading = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">parent object</param>
        public frmPolylineSymbolSet(object parent)
        {
            InitializeComponent();

            _parent = parent;
            if (_parent.GetType() == typeof(LegendView))
            {
                this.Height = GB_PointSymbol.Bottom - this.Top + 40;
                B_Apply.Visible = false;
                B_OK.Visible = false;
            }
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public frmPolylineSymbolSet()
        //{
        //    InitializeComponent();

        //    this.Height = GB_PointSymbol.Bottom - this.Top + 40;
        //    B_Apply.Visible = false;
        //    B_OK.Visible = false;
        //}

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="isApply"></param>
        //public frmPolylineSymbolSet(bool isApply)
        //{
        //    InitializeComponent ();
        //}

        #endregion

        #region Properties
        /// <summary>
        /// Get or set polyline break
        /// </summary>
        public PolyLineBreak PolylineBreak
        {
            get { return _polylineBreak; }
            set 
            { 
                _polylineBreak = value;
                UpdateProperties();
            }
        }

        #endregion

        #region Methods
        private void frmPolylineSymbolSet_Load(object sender, EventArgs e)
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

            B_FillColor.BackColor = _polylineBreak.Color;
            NUD_Size.Value = (decimal)_polylineBreak.Size;            
            ChB_DrawShape.Checked = _polylineBreak.DrawShape;
            ChB_DrawPointSymbol.Checked = _polylineBreak.DrawSymbol;
            NUD_SymbolSize.Value = (decimal)_polylineBreak.SymbolSize;
            B_SymbolColor.BackColor = _polylineBreak.SymbolColor;
            NUD_SymbolInterval.Value = (decimal)_polylineBreak.SymbolInterval;
            CB_SymbolStyle.Items.Clear();
            foreach (string sName in Enum.GetNames(typeof(PointStyle)))
                CB_SymbolStyle.Items.Add(sName);

            CB_SymbolStyle.Text = _polylineBreak.SymbolStyle.ToString();

            if (_parent.GetType() == typeof(LegendView))
                symbolControl1.SymbolNumber = 5;
            else
                symbolControl1.SymbolNumber = Enum.GetNames(typeof(LineStyles)).Length;

            symbolControl1.SelectedCell = Array.IndexOf(Enum.GetNames(typeof(LineStyles)), _polylineBreak.Style.ToString());

            _isLoading = false;
        }

        #endregion

        #region Events

        private void ChB_DrawPointSymbol_CheckedChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            GB_PointSymbol.Enabled = ChB_DrawPointSymbol.Checked;
            _polylineBreak.DrawSymbol = ChB_DrawPointSymbol.Checked;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_DrawSymbol(ChB_DrawPointSymbol.Checked);            
        }

        private void B_FillColor_Click(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            ColorDialog colorDlg = new ColorDialog();
            colorDlg.Color = _polylineBreak.Color;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                B_FillColor.BackColor = colorDlg.Color;
                _polylineBreak.Color = colorDlg.Color;
                if (_parent.GetType() == typeof(LegendView))
                    ((LegendView)_parent).SetLegendBreak_Color(colorDlg.Color);                
            }
        }

        private void NUD_Size_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _polylineBreak.Size = (float)NUD_Size.Value;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_Size((float)NUD_Size.Value);            
        }

        private void ChB_DrawShape_CheckedChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _polylineBreak.DrawShape = ChB_DrawShape.Checked;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_DrawShape(ChB_DrawShape.Checked);            
        }

        private void NUD_SymbolSize_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _polylineBreak.SymbolSize = (float)NUD_SymbolSize.Value;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_SymbolSize((float)NUD_SymbolSize.Value);            
        }

        private void B_SymbolColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.Color = _polylineBreak.SymbolColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                B_SymbolColor.BackColor = colorDlg.Color;
                _polylineBreak.SymbolColor = colorDlg.Color;
                if (_parent.GetType() == typeof(LegendView))
                    ((LegendView)_parent).SetLegendBreak_SymbolColor(colorDlg.Color);                
            }
        }

        private void CB_SymbolStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _polylineBreak.SymbolStyle = (PointStyle)Enum.Parse(typeof(PointStyle), CB_SymbolStyle.Text);
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_SymbolStyle(_polylineBreak.SymbolStyle);            
        }

        private void NUD_SymbolInterval_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _polylineBreak.SymbolInterval = (int)NUD_SymbolInterval.Value;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_SymbolInterval(_polylineBreak.SymbolInterval);            
        }

        private void symbolControl1_SelectedCellChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _polylineBreak.Style = (LineStyles)(Enum.GetValues(typeof(LineStyles)).GetValue(symbolControl1.SelectedCell));
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_PolylineStyle(_polylineBreak.Style);            
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
                ((MapView)_parent).DefPolylineBreak = _polylineBreak;
            }
            else if (_parent.GetType() == typeof(MapLayout))
                ((MapLayout)_parent).DefPolylineBreak = _polylineBreak;

            ParentRedraw();
            this.Close();
        }

        #endregion
    }
}
