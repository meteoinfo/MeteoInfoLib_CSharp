using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Layout;
using MeteoInfoC.Map;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Label symbol set form
    /// </summary>
    public partial class frmLabelSymbolSet : Form
    {
        #region Variables
        private LabelBreak _labelBreak;
        private object _parent;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public frmLabelSymbolSet()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set label break
        /// </summary>
        public LabelBreak LabelBreak
        {
            get { return _labelBreak; }
            set 
            { 
                _labelBreak = value;
                UpdateProperties();
            }
        }

        #endregion

        private void frmLabelSymbolSet_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Set parent
        /// </summary>
        /// <param name="parent">parent object</param>
        public void SetParent(object parent)
        {
            _parent = parent;
        }

        private void UpdateProperties()
        {
            TB_Text.Text = _labelBreak.Text;
            B_Color.ForeColor = _labelBreak.Color;
            NUD_Angle.Value = (decimal)_labelBreak.Angle;
        }

        private void B_Font_Click(object sender, EventArgs e)
        {
            FontDialog aFDlg = new FontDialog();
            aFDlg.Font = _labelBreak.Font;
            if (aFDlg.ShowDialog() == DialogResult.OK)
            {
                _labelBreak.Font = aFDlg.Font;
            }
        }

        private void B_Color_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                B_Color.ForeColor = colorDlg.Color;
                _labelBreak.Color = colorDlg.Color;
            }
        }

        private void NUD_Angle_ValueChanged(object sender, EventArgs e)
        {
            _labelBreak.Angle = (float)NUD_Angle.Value;
        }

        private void TB_Text_TextChanged(object sender, EventArgs e)
        {
            _labelBreak.Text = TB_Text.Text;
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
            ParentRedraw();
            if (_parent.GetType() == typeof(MapView))
            {
                ((MapView)_parent).DefLabelBreak = _labelBreak;
            }
            else if (_parent.GetType() == typeof(MapLayout))
                ((MapLayout)_parent).DefLabelBreak = _labelBreak;

            this.Close();
        }
    }
}
