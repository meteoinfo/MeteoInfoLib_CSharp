using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace MeteoInfoC.Layout
{
    /// <summary>
    /// Page setup form
    /// </summary>
    public partial class frmPageSetup : Form
    {
        #region Variables
        private PrinterSettings m_PrinterSetting = new PrinterSettings();
        private PaperSize m_PaperSize = new PaperSize();
        private bool m_Landscape = true;
        private bool m_IsCustom = false;
        private List<PaperSize> m_PaperSizeList = new List<PaperSize>();

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public frmPageSetup(PrinterSettings aPrinterSetting)
        {
            InitializeComponent();

            m_PrinterSetting = aPrinterSetting;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public frmPageSetup(PaperSize aPS, bool isLandscape)
        {
            InitializeComponent();

            m_PaperSize = aPS;              
            m_Landscape = isLandscape;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get paper size
        /// </summary>
        public PaperSize PaperSizeV
        {
            get { return m_PaperSize; }
        }

        /// <summary>
        /// Get landscape
        /// </summary>
        public bool Landscape
        {
            get { return m_Landscape; }
        }

        #endregion

        #region Methods
        private void frmPageSetup_Load(object sender, EventArgs e)
        {
            //Gets the list of available paper sizes
            this.CB_PageSize.SuspendLayout();
            //PrinterSettings.PaperSizeCollection paperSizes = m_PrinterSetting.PaperSizes;
            //foreach (PaperSize ps in paperSizes)
            //    CB_PageSize.Items.Add(ps.PaperName);
            //CB_PageSize.SelectedItem = m_PrinterSetting.DefaultPageSettings.PaperSize.PaperName;
            //if (CB_PageSize.SelectedIndex == -1) CB_PageSize.SelectedIndex = 1;

            PaperSize aPS = new PaperSize("A4", 827, 1169);
            m_PaperSizeList.Add(aPS);
            aPS = new PaperSize("Letter", 850, 1100);
            m_PaperSizeList.Add(aPS);
            aPS = new PaperSize("A5", 583, 827);
            m_PaperSizeList.Add(aPS);
            aPS = new PaperSize("Custom", 500, 800);
            if (m_PaperSize.PaperName == "Custom")
            {
                m_IsCustom = true;
                aPS.Width = m_PaperSize.Width;
                aPS.Height = m_PaperSize.Height;
            }
            m_PaperSizeList.Add(aPS);

            foreach (PaperSize ps in m_PaperSizeList)
                CB_PageSize.Items.Add(ps.PaperName);
            CB_PageSize.SelectedItem = m_PaperSize.PaperName;
            if (CB_PageSize.SelectedIndex == -1) CB_PageSize.SelectedIndex = 1;
            
            CB_PageSize.ResumeLayout();

            //Gets the paper orientation
            if (m_Landscape)
                RB_Landscape.Checked = true;
            else
                RB_Portrait.Checked = true;

            //Set units
            CB_WidthUnit.Items.Clear();
            CB_WidthUnit.Items.Add("Inches");
            CB_WidthUnit.SelectedIndex = 0;
            CB_HeightUnit.Items.Clear();
            CB_HeightUnit.Items.Add("Inches");
            CB_HeightUnit.SelectedIndex = 0;
        }

        /// <summary>
        /// Get printer setting
        /// </summary>
        /// <returns>printer setting</returns>
        public PrinterSettings GetPrinterSetting()
        {
            return m_PrinterSetting;
        }       

        private void UpdatePaperSize()
        {
            m_IsCustom = false;

            if (CB_PageSize.SelectedIndex == -1)
                CB_PageSize.SelectedIndex = 0;            
            if (m_Landscape)
            {
                TB_Width.Text = (m_PaperSize.Height / 100.0).ToString();
                TB_Height.Text = (m_PaperSize.Width / 100.0).ToString();
            }
            else
            {
                TB_Width.Text = (m_PaperSize.Width / 100.0).ToString();
                TB_Height.Text = (m_PaperSize.Height / 100.0).ToString();
            }

            m_IsCustom = true;
        }

        private void UpdatePaperSize_Old()
        {
            m_IsCustom = false;

            if (CB_PageSize.SelectedIndex == -1)
                CB_PageSize.SelectedIndex = 0;
            PaperSize aPS = m_PrinterSetting.DefaultPageSettings.PaperSize;
            if (m_PrinterSetting.DefaultPageSettings.Landscape)
            {
                TB_Width.Text = (aPS.Height / 100.0).ToString();
                TB_Height.Text = (aPS.Width / 100.0).ToString();
            }
            else
            {
                TB_Width.Text = (aPS.Width / 100.0).ToString();
                TB_Height.Text = (aPS.Height / 100.0).ToString();
            }

            m_IsCustom = true;
        }

        private void CB_PageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //m_PrinterSetting.DefaultPageSettings.PaperSize = m_PrinterSetting.PaperSizes[CB_PageSize.SelectedIndex];
            m_PaperSize = m_PaperSizeList[CB_PageSize.SelectedIndex];
            UpdatePaperSize();
        }

        private void RB_Portrait_CheckedChanged(object sender, EventArgs e)
        {
            //m_PrinterSetting.DefaultPageSettings.Landscape = !RB_Portrait.Checked;
            m_Landscape = !RB_Portrait.Checked;
            UpdatePaperSize();
        }

        private void RB_Landscape_CheckedChanged(object sender, EventArgs e)
        {
            //m_PrinterSetting.DefaultPageSettings.Landscape = RB_Landscape.Checked;
            m_Landscape = RB_Landscape.Checked;
            UpdatePaperSize();
        }

        private void B_OK_Click(object sender, EventArgs e)
        {
            if (m_PaperSize.PaperName == "Custom")
            {
                if (m_Landscape)
                {
                    m_PaperSize.Width = (int)(Single.Parse(TB_Height.Text) * 100);
                    m_PaperSize.Height = (int)(Single.Parse(TB_Width.Text) * 100);
                }
                else
                {
                    m_PaperSize.Width = (int)(Single.Parse(TB_Width.Text) * 100);
                    m_PaperSize.Height = (int)(Single.Parse(TB_Height.Text) * 100);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void TB_Width_TextChanged(object sender, EventArgs e)
        {
            if (m_IsCustom)
                CB_PageSize.SelectedIndex = CB_PageSize.Items.Count - 1;
        }

        private void TB_Height_TextChanged(object sender, EventArgs e)
        {
            if (m_IsCustom)
                CB_PageSize.SelectedIndex = CB_PageSize.Items.Count - 1;
        }

        private void B_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}
