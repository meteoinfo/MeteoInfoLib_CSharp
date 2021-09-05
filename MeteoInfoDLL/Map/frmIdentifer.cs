using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MeteoInfoC.Map
{
    /// <summary>
    /// Identifer form for vector layer
    /// </summary>
    public partial class frmIdentifer : Form
    {
        #region Variables
        private MapView _mapView;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>     
        /// <param name="mapView">parent MapView</param>
        public frmIdentifer(MapView mapView)
        {
            InitializeComponent();

            _mapView = mapView;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set map view
        /// </summary>
        public MapView MapView
        {
            get { return _mapView; }
            set { _mapView = value; }
        }

        #endregion

        #region Methods
        private void frmIdentifer_Load(object sender, EventArgs e)
        {
            ListView1.View = View.Details;
            ListView1.Columns.Add(new ColumnHeader());
            ListView1.Columns[0].Width = 100;
            ListView1.Columns[0].Text = "Field";
            ListView1.Columns.Add(new ColumnHeader());
            ListView1.Columns[1].Width = ListView1.Width - ListView1.Columns[0].Width - 5;
            ListView1.Columns[1].Text = "Value";
        }

        private void frmIdentifer_FormClosed(object sender, FormClosedEventArgs e)
        {
            _mapView.DrawIdentiferShape = false;
            _mapView.PaintLayers();
        }

        #endregion

        private void frmIdentifer_Resize(object sender, EventArgs e)
        {
            if (ListView1.Columns.Count > 0)
                ListView1.Columns[1].Width = ListView1.Width - ListView1.Columns[0].Width - 5;
        }
    }
}
