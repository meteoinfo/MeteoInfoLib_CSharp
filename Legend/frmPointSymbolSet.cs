using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MeteoInfoC.Drawing;
using MeteoInfoC.Map;
using MeteoInfoC.Layout;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Point symbol set form
    /// </summary>
    public partial class frmPointSymbolSet : Form
    {
        #region Variables
        private PointBreak _pointBreak;
        private string[] _imagePaths = null;
        //private List<Image> _imageList = null;
        private object _parent = null ;
        private MarkerType _markerType = MarkerType.Simple;
        private bool _isLoading = false;

        #endregion

        #region Constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="parent">parent object</param>
        public frmPointSymbolSet(object parent)
        {
            InitializeComponent();

            _parent = parent;
            if (_parent.GetType() == typeof(LegendView))
            {
                this.Height = GB_Outline.Bottom - this.Top + 40;
                B_Apply.Visible = false;
                B_OK.Visible = false;
            }
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public frmPointSymbolSet()
        //{
        //    InitializeComponent();

        //    this.Height = GB_Outline.Bottom - this.Top + 40;
        //    B_Apply.Visible = false;
        //    B_OK.Visible = false;
        //}

        ///// <summary>
        ///// constructor
        ///// </summary>
        ///// <param name="isApply"></param>
        //public frmPointSymbolSet(bool isApply)
        //{
        //    InitializeComponent();
        //}

        #endregion

        #region Properties
        /// <summary>
        /// Get or set point break
        /// </summary>
        public PointBreak PointBreak
        {
            get { return _pointBreak; }
            set 
            { 
                _pointBreak = value;
                CB_MarkerType.Items.Clear();
                CB_MarkerType.Items.AddRange(Enum.GetNames(typeof(MarkerType)));
                CB_MarkerType.Text = _pointBreak.MarkerType.ToString();
                UpdateProperties();
            }
        }

        #endregion

        #region Methods
        ///// <summary>
        ///// Set parent
        ///// </summary>
        ///// <param name="parent">parent object</param>
        //public void SetParent(object parent)
        //{
        //    _parent = parent;
        //}

        private void frmPointSymbolSet_Load(object sender, EventArgs e)
        {                       
            
        }
          
        private void UpdateProperties()
        {
            _isLoading = true;
            B_FillColor.BackColor = _pointBreak.Color;
            NUD_Size.Value = (decimal)_pointBreak.Size;
            ChB_DrawOutline.Checked = _pointBreak.DrawOutline;
            B_OutlineColor.BackColor = _pointBreak.OutlineColor;
            ChB_DrawShape.Checked = _pointBreak.DrawShape;
            ChB_DrawFill.Checked = _pointBreak.DrawFill;
            NUD_Angle.Value = (decimal)_pointBreak.Angle;
            _isLoading = false;
        }

        private void UpdateSimpleTab()
        {
            markerControl1.MarkerType = MarkerType.Simple;
            markerControl1.SymbolNumber = Enum.GetNames(typeof(PointStyle)).Length;
            Lab_FontFamily.Enabled = false;
            CB_FontFamily.Enabled = false;
            GB_Outline.Enabled = true;
            Lab_FillColor.Enabled = true;
            B_FillColor.Enabled = true;
            ChB_DrawFill.Enabled = true;
        }

        private void UpdateCharacterTab()
        {
            markerControl1.MarkerType = MarkerType.Character;
            markerControl1.SymbolNumber = 256;
            Lab_FontFamily.Enabled = true;
            CB_FontFamily.Enabled = true;
            GB_Outline.Enabled = false;
            Lab_FillColor.Enabled = true;
            B_FillColor.Enabled = true;
            ChB_DrawFill.Enabled = false;

            InstalledFontCollection fonts = new InstalledFontCollection();
            this.CB_FontFamily.Items.Clear();
            foreach (FontFamily ff in fonts.Families)
            {
                this.CB_FontFamily.Items.Add(ff.Name);
            }
            this.CB_FontFamily.Text = _pointBreak.FontName;                   
        }

        private void UpdateImageTab()
        {
            markerControl1.MarkerType = MarkerType.Image;
            Lab_FontFamily.Enabled = false;
            CB_FontFamily.Enabled = false;
            GB_Outline.Enabled = false;
            Lab_FillColor.Enabled = false;
            B_FillColor.Enabled = false;
            ChB_DrawFill.Enabled = false;

            if (_imagePaths == null)
            {
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location.ToString());
                path = Path.Combine(path, "Image");
                _imagePaths = Directory.GetFiles(path);
                List<Image> imageList = new List<Image>();
                foreach (string aFile in _imagePaths)
                {
                    if (Path.GetExtension(aFile).ToLower() == ".ico")
                        imageList.Add(Bitmap.FromHicon(new Icon(aFile).Handle));
                    else
                        imageList.Add(new Bitmap(aFile));
                }

                markerControl1.SetIamgeList(imageList);
            }
            else
            {
                markerControl1.SymbolNumber = _imagePaths.Length;                
            }
        }

        #endregion

        #region Events

        private void CB_MarkerType_SelectedIndexChanged(object sender, EventArgs e)
        {                        
            switch ((MarkerType)Enum.Parse(typeof(MarkerType), CB_MarkerType.Text, true))
            {
                case MarkerType.Simple:                    
                    UpdateSimpleTab();
                    _markerType = MarkerType.Simple;
                    break;
                case MarkerType.Character:                    
                    UpdateCharacterTab();
                    _markerType = MarkerType.Character;
                    break;
                case MarkerType.Image:
                    UpdateImageTab();
                    _markerType = MarkerType.Image;                    
                    break;                    
            }

            markerControl1._vScrollBar.Value = 0;
            //_pointBreak.MarkerType = markerType;            
            //if (this.Owner != null)
            //{
            //    if (this.Owner.GetType() == typeof(frmLegendSet))
            //        ((frmLegendSet)this.Owner).SetLegendBreak_MarkerType(_markerType);
            //}
        }       

        private void CB_FontFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            Font aFont = new Font(CB_FontFamily.Text, 10, FontStyle.Regular);
            //_pointBreak.FontName = aFont.Name;
            markerControl1.Font = aFont;
            markerControl1.SelectedCell = _pointBreak.CharIndex;
            markerControl1.Invalidate();

            _pointBreak.FontName = aFont.Name;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_FontName(aFont.Name);            
        }

        private void B_FillColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.Color = _pointBreak.Color;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                B_FillColor.BackColor = colorDlg.Color;
                _pointBreak.Color = colorDlg.Color;
                if (_parent.GetType() == typeof(LegendView))
                    ((LegendView)_parent).SetLegendBreak_Color(colorDlg.Color);             
            }
        }

        private void B_OutlineColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.Color = _pointBreak.OutlineColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                B_OutlineColor.BackColor = colorDlg.Color;
                _pointBreak.OutlineColor = colorDlg.Color;
                if (_parent.GetType() == typeof(LegendView))
                    ((LegendView)_parent).SetLegendBreak_OutlineColor(colorDlg.Color);                
            }
        }

        private void NUD_Size_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _pointBreak.Size = (float)NUD_Size.Value;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_Size((float)NUD_Size.Value);            
        }

        private void ChB_DrawOutline_CheckedChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _pointBreak.DrawOutline = ChB_DrawOutline.Checked;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_DrawOutline(ChB_DrawOutline.Checked);
        }

        private void markerControl1_SelectedCellChanged(object sender, EventArgs e)
        {
            _pointBreak.MarkerType = _markerType;
            switch (_pointBreak.MarkerType)
            {
                case MarkerType.Character:
                    _pointBreak.CharIndex = markerControl1.SelectedCell;
                    break;
                case MarkerType.Image:
                    _pointBreak.ImagePath = _imagePaths[markerControl1.SelectedCell];
                    break;
                case MarkerType.Simple:
                    _pointBreak.Style = (PointStyle)markerControl1.SelectedCell;
                    break;
            }
            if (_parent.GetType() == typeof(LegendView))
            {
                ((LegendView)_parent).SetLegendBreak_MarkerType(_markerType);
                if (_pointBreak.MarkerType == MarkerType.Image)
                    ((LegendView)_parent).SetLegendBreak_Image(_imagePaths[markerControl1.SelectedCell]);
                else
                    ((LegendView)_parent).SetLegendBreak_MarkerIndex(markerControl1.SelectedCell);

            }
        }        

        private void ChB_DrawShape_CheckedChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _pointBreak.DrawShape = ChB_DrawShape.Checked;
            if (_parent.GetType() == typeof(LegendView))
                ((LegendView)_parent).SetLegendBreak_DrawShape(ChB_DrawShape.Checked);            
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

        #endregion

        private void B_Apply_Click(object sender, EventArgs e)
        {
            ParentRedraw();
        }

        private void B_OK_Click(object sender, EventArgs e)
        {
            ParentRedraw();
            if (_parent.GetType() == typeof(MapView))
            {
                ((MapView)_parent).DefPointBreak = _pointBreak;
            }
            else if (_parent.GetType() == typeof(MapLayout))
                ((MapLayout)_parent).DefPointBreak = _pointBreak;

            this.Close();
        }

        private void NUD_Angle_ValueChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _pointBreak.Angle = (float)NUD_Angle.Value;
            if (this.Owner != null)
            {
                if (this.Owner.GetType() == typeof(frmLegendSet))
                    ((frmLegendSet)this.Owner).SetLegendBreak_Angle((float)NUD_Angle.Value);
            }
        }

        private void ChB_DrawFill_CheckedChanged(object sender, EventArgs e)
        {
            if (_isLoading)
                return;

            _pointBreak.DrawFill = ChB_DrawFill.Checked;
            if (this.Owner != null)
            {
                if (this.Owner.GetType() == typeof(frmLegendSet))
                    ((frmLegendSet)this.Owner).SetLegendBreak_DrawFill(_pointBreak.DrawFill);
            }
        }
    }
}
