using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Global.Colors;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Color symbol set form
    /// </summary>
    public partial class frmColorSymbolSet : Form
    {
        #region Variables
        private object _parent;
        private ColorBreak _colorBreak;
        private bool _isLoading = false;

        #endregion

        #region Constructor
        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public frmColorSymbolSet()
        //{
        //    InitializeComponent();
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">paraent object</param>
        public frmColorSymbolSet(object parent)
        {
            InitializeComponent();

            _parent = parent;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set color break
        /// </summary>
        public ColorBreak ColorBreak
        {
            get { return _colorBreak; }
            set 
            { 
                _colorBreak = value;
                UpdateProperties();
            }
        }

        #endregion

        private void frmColorSymbolSet_Load(object sender, EventArgs e)
        {

        }

        private void UpdateProperties()
        {
            _isLoading = true;

            B_FillColor.BackColor = _colorBreak.Color;
            int trans = (int)((1 - (double)_colorBreak.Color.A / 255) * 100);
            NUD_TransparencyPerc.Value = (decimal)trans;

            _isLoading = false;
        }

        private void B_FillColor_Click(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            //frmColorPicker frmCP = new frmColorPicker(_colorBreak.Color);
            //if (frmCP.ShowDialog() == DialogResult.OK)
            //{
            //    B_FillColor.BackColor = frmCP.PrimaryColor;
            //    _colorBreak.Color = frmCP.PrimaryColor;
            //    if (_parent.GetType() == typeof(LegendView))
            //        ((LegendView)_parent).SetLegendBreak_Color(frmCP.PrimaryColor);
            //}

            ColorDialog colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                B_FillColor.BackColor = colorDlg.Color;
                int alpha = (int)((1 - (double)NUD_TransparencyPerc.Value / 100.0) * 255);
                _colorBreak.Color = Color.FromArgb(alpha, colorDlg.Color);
                if (_parent.GetType() == typeof(LegendView))
                    ((LegendView)_parent).SetLegendBreak_Color(_colorBreak.Color);
            }
        }

        private void NUD_TransparencyPerc_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            int alpha = (int)((1 - (double)NUD_TransparencyPerc.Value / 100.0) * 255);
            _colorBreak.Color = Color.FromArgb(alpha, _colorBreak.Color);
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_Color(_colorBreak.Color);
        }
    }
}
