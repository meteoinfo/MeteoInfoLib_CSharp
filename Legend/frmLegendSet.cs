using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using MeteoInfoC.Shape;
using MeteoInfoC.Drawing;
using MeteoInfoC.Layer;
using MeteoInfoC.Map;
using MeteoInfoC.Global;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Legend set form
    /// </summary>
    public partial class frmLegendSet : Form
    {
        #region Variables
        /// <summary>
        /// Legend set form
        /// </summary>
        public static frmLegendSet pCurrenWin = null;
        ///// <summary>
        ///// Property form
        ///// </summary>
        //public frmProperty m_FrmSS = null;

        private frmPointSymbolSet _frmPointSymbolSet;
        private frmPolylineSymbolSet _frmPolylineSymbolSet;
        private frmPolygonSymbolSet _frmPolygonSymbolSet;
        private frmColorSymbolSet _frmColorSymbolSet;

        bool m_IsApplied;
        //int m_LayerHandle;
        MapLayer _mapLayer;
        Form m_FrmMeteoData = new Form();
        LegendScheme _legendScheme = null;
        LayersLegend m_LayersTV = new LayersLegend();                
        Boolean m_IfFrmMeteoData = true;        
        Boolean m_IfCreateLegendscheme = false;

        //private bool _IsLoading = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ifFrmMeteoData"></param>
        /// <param name="aLayer"></param>
        /// <param name="aLayersTV"></param>
        public frmLegendSet(Boolean ifFrmMeteoData, MapLayer aLayer, LayersLegend aLayersTV)
        {
            InitializeComponent();
            pCurrenWin = this;

            _frmPointSymbolSet = new frmPointSymbolSet(this.legendView1);
            _frmPolylineSymbolSet = new frmPolylineSymbolSet(this.legendView1);
            _frmPolygonSymbolSet = new frmPolygonSymbolSet(this.legendView1);
            _frmColorSymbolSet = new frmColorSymbolSet(this.legendView1);

            m_IfFrmMeteoData = ifFrmMeteoData;
            if (m_IfFrmMeteoData)
            {
                this.B_Apply.Visible = false;
            }
            _mapLayer = aLayer;
            //m_LayerHandle = handle;
            m_LayersTV = aLayersTV;
        }

        #endregion

        #region Methods

        private void frmLegendSet_Load(object sender, EventArgs e)
        {
            //_IsLoading = true;
            m_IsApplied = false;

            //MapLayer _mapLayer = m_LayersTV.MapView.GetLayerFromHandle(m_LayerHandle); 
            //VectorLayer aVLayer = (VectorLayer)_mapLayer;            

            if (_mapLayer.LayerType == LayerTypes.VectorLayer)
            {
                //Set legend type
                CB_LegendType.Items.Add(Enum.GetName(typeof(LegendType), LegendType.SingleSymbol));
                CB_LegendType.Items.Add(Enum.GetName(typeof(LegendType), LegendType.UniqueValue));
                CB_LegendType.Items.Add(Enum.GetName(typeof(LegendType), LegendType.GraduatedColor));
                CB_LegendType.Text = Enum.GetName(typeof(LegendType), _legendScheme.LegendType);
                if (CB_LegendType.Text == Enum.GetName(typeof(LegendType), LegendType.SingleSymbol))
                {
                    TSB_Add.Enabled = false;
                    TSB_Del.Enabled = false;
                    TSB_DelAll.Enabled = false;
                    TSB_Down.Enabled = false;
                    TSB_Up.Enabled = false;
                    TSB_MakeBreaks.Enabled = false;
                }

                //Set field text
                CB_Field.Text = _legendScheme.FieldName;

                if (m_IfFrmMeteoData)
                {
                    CB_LegendType.Enabled = false;
                    CB_Field.Enabled = false;
                }
            }
            else
            {
                CB_LegendType.Enabled = false;
                CB_Field.Enabled = false;
            }
            
            //if (_legendScheme.BreakNum > 0)
            //{
            //    ShowLegendScheme(_legendScheme);
            //}
            
            m_IfCreateLegendscheme = true;
            //_IsLoading = false;
        }        

        /// <summary>
        /// Set legend scheme
        /// </summary>
        /// <param name="aLS"></param>
        public void SetLegendScheme(LegendScheme aLS)
        {
            if (aLS != null)
            {
                _legendScheme = (LegendScheme)aLS.Clone();
                legendView1.LegendScheme = _legendScheme;
            }
        }

        /// <summary>
        /// Set meteorological data form
        /// </summary>
        /// <param name="aFrmMD"></param>
        public void SetFrmMeteoData(Form aFrmMD)
        {
            m_FrmMeteoData = aFrmMD;
        }

        /// <summary>
        /// Get legend scheme
        /// </summary>
        /// <returns></returns>
        public LegendScheme GetLegendScheme()
        {
            return _legendScheme;
        }

        /// <summary>
        /// Get if applied
        /// </summary>
        /// <returns></returns>
        public bool GetIsApplied()
        {
            return m_IsApplied;
        }

        ///// <summary>
        ///// Show legend scheme
        ///// </summary>
        ///// <param name="legendScheme"></param>
        //public void ShowLegendScheme(LegendScheme legendScheme)
        //{
        //    int i;
        //    switch (legendScheme.ShapeType)
        //    {
        //        case ShapeTypes.Point:
        //            PointBreak aPB;
        //            DataGridView1.RowCount = legendScheme.BreakNum;
        //            if (legendScheme.LegendType == LegendType.SingleSymbol)
        //            {
        //                aPB = (PointBreak)LegendScheme.LegendBreaks[0];
        //                DataGridView1.Columns[1].Visible = false;
        //                DataGridView1[2, 0].Value = aPB.Caption;
        //            }
        //            else
        //            {
        //                DataGridView1.Columns[1].Visible = true;
        //                for (i = 0; i < legendScheme.BreakNum; i++)
        //                {
        //                    aPB = (PointBreak)LegendScheme.LegendBreaks[i];

        //                    if (aPB.StartValue.ToString() == aPB.EndValue.ToString())
        //                    {
        //                        DataGridView1[1, i].Value = aPB.StartValue.ToString();
        //                    }
        //                    else
        //                    {
        //                        DataGridView1[1, i].Value = aPB.StartValue.ToString() +
        //                            " - " + aPB.EndValue.ToString();
        //                    }
        //                    DataGridView1[2, i].Value = aPB.Caption;
        //                }
        //            }
        //            break;
        //        case ShapeTypes.Polyline:
        //        case ShapeTypes.PolylineZ:
        //            PolyLineBreak aPLB;
        //            DataGridView1.RowCount = legendScheme.BreakNum;
        //            if (legendScheme.LegendType == LegendType.SingleSymbol)
        //            {
        //                aPLB = (PolyLineBreak)LegendScheme.LegendBreaks[0];
        //                DataGridView1.Columns[1].Visible = false;
        //                DataGridView1[2, 0].Value = aPLB.Caption;
        //            }
        //            else
        //            {
        //                DataGridView1.Columns[1].Visible = true;
        //                for (i = 0; i < legendScheme.BreakNum; i++)
        //                {
        //                    aPLB = (PolyLineBreak)LegendScheme.LegendBreaks[i];

        //                    if (aPLB.StartValue == aPLB.EndValue)
        //                    {
        //                        DataGridView1[1, i].Value = aPLB.StartValue.ToString();
        //                    }
        //                    else
        //                    {
        //                        DataGridView1[1, i].Value = aPLB.StartValue.ToString() +
        //                            " - " + aPLB.EndValue.ToString();
        //                    }
        //                    DataGridView1[2, i].Value = aPLB.Caption;
        //                }
        //            }
        //            break;
        //        case ShapeTypes.Polygon:
        //            PolygonBreak aPGB;
        //            DataGridView1.RowCount = legendScheme.BreakNum;
        //            if (legendScheme.LegendType == LegendType.SingleSymbol)
        //            {
        //                aPGB = (PolygonBreak)LegendScheme.LegendBreaks[0];
        //                DataGridView1.Columns[1].Visible = false;
        //                DataGridView1[2, 0].Value = aPGB.Caption;
        //            }
        //            else
        //            {
        //                DataGridView1.Columns[1].Visible = true;
        //                for (i = 0; i < legendScheme.BreakNum; i++)
        //                {
        //                    aPGB = (PolygonBreak)LegendScheme.LegendBreaks[i];

        //                    if (aPGB.StartValue == aPGB.EndValue)
        //                    {
        //                        DataGridView1[1, i].Value = aPGB.StartValue.ToString();
        //                    }
        //                    else
        //                    {
        //                        DataGridView1[1, i].Value = aPGB.StartValue.ToString() +
        //                            " - " + aPGB.EndValue.ToString();
        //                    }
        //                    DataGridView1[2, i].Value = aPGB.Caption;
        //                }
        //            }
        //            break;
        //        case ShapeTypes.Image:
        //            ColorBreak aCB;
        //            DataGridView1.RowCount = legendScheme.BreakNum;
        //            if (legendScheme.LegendType == LegendType.SingleSymbol)
        //            {
        //                aCB = LegendScheme.LegendBreaks[0];
        //                DataGridView1.Columns[1].Visible = false;
        //                DataGridView1[2, 0].Value = aCB.Caption;
        //            }
        //            else
        //            {
        //                DataGridView1.Columns[1].Visible = true;
        //                for (i = 0; i < legendScheme.BreakNum; i++)
        //                {
        //                    aCB = LegendScheme.LegendBreaks[i];

        //                    if (aCB.StartValue == aCB.EndValue)
        //                    {
        //                        DataGridView1[1, i].Value = aCB.StartValue.ToString();
        //                    }
        //                    else
        //                    {
        //                        DataGridView1[1, i].Value = aCB.StartValue.ToString() +
        //                            " - " + aCB.EndValue.ToString();
        //                    }
        //                    DataGridView1[2, i].Value = aCB.Caption;
        //                }
        //            }
        //            break;
        //    }
        //}

        //private void DrawShape(LegendScheme legendScheme,
        //    Graphics g)
        //{
        //    int i;            
        //    Single aSize;
        //    Point aP = new Point(0, 0);
        //    Rectangle rect;
        //    Single width, height;

        //    switch (legendScheme.ShapeType)
        //    {
        //        case ShapeTypes.Point:
        //            PointBreak aPB = new PointBreak();
        //            for (i = 0; i < legendScheme.BreakNum; i++)
        //            {
        //                aPB = (PointBreak)LegendScheme.LegendBreaks[i];                        
        //                rect = DataGridView1.GetCellDisplayRectangle(0, i, true);
        //                aP.X = rect.Left + rect.Width / 2;
        //                aP.Y = rect.Top + rect.Height / 2;                                                
        //                aSize = aPB.Size;
        //                if (aPB.DrawShape)
        //                {
        //                    //Draw.DrawPoint(aPB.Style, aP, aPB.Color, aPB.OutlineColor, 
        //                    //    aPB.Size, aPB.DrawOutline, aPB.DrawFill, g);
        //                    Draw.DrawPoint(aP, aPB, g);
        //                }
        //            }
        //            break;
        //        case ShapeTypes.Polyline:
        //        case ShapeTypes.PolylineZ:
        //            PolyLineBreak aPLB = new PolyLineBreak();
        //            for (i = 0; i < legendScheme.BreakNum; i++)
        //            {
        //                aPLB = (PolyLineBreak)LegendScheme.LegendBreaks[i];
        //                rect = DataGridView1.GetCellDisplayRectangle(0, i, true);
        //                aP.X = rect.Left + rect.Width / 2;
        //                aP.Y = rect.Top + rect.Height / 2;
        //                aSize = aPLB.Size;
        //                width = rect.Width / 2;
        //                height = rect.Height / 2;
        //                if (aPLB.DrawPolyline)
        //                {
        //                    //Draw.DrawPolyLineSymbol(aPLB.Style, aP, aPLB.Color, aSize, width, height, g);
        //                    Draw.DrawPolyLineSymbol(aP, width, height, aPLB, g);
        //                }
        //            }
        //            break;
        //        case ShapeTypes.Polygon:
        //            PolygonBreak aPGB = new PolygonBreak();
        //            for (i = 0; i < legendScheme.BreakNum; i++)
        //            {
        //                aPGB = (PolygonBreak)LegendScheme.LegendBreaks[i];
        //                rect = DataGridView1.GetCellDisplayRectangle(0, i, true);
        //                aP.X = rect.Left + rect.Width / 2;
        //                aP.Y = rect.Top + rect.Height / 2;                        
        //                width = rect.Width / 2;
        //                height = rect.Height / 2;
        //                if (aPGB.DrawShape)
        //                {
        //                    //Draw.DrawPolygonSymbol(aP, aPGB.Color, aPGB.OutlineColor, width,
        //                    //    height, aPGB.DrawFill, aPGB.DrawOutline, g);
        //                    Draw.DrawPolygonSymbol(aP, width, height, aPGB, 0, g);
        //                }
        //            }
        //            break;
        //        case ShapeTypes.Image:
        //            ColorBreak aCB = new ColorBreak();
        //            for (i = 0; i < legendScheme.BreakNum; i++)
        //            {
        //                aCB = LegendScheme.LegendBreaks[i];
        //                rect = DataGridView1.GetCellDisplayRectangle(0, i, true);
        //                aP.X = rect.Left + rect.Width / 2;
        //                aP.Y = rect.Top + rect.Height / 2;
        //                width = rect.Width / 2;
        //                height = rect.Height / 2;
        //                Draw.DrawPolygonSymbol(aP, aCB.Color, Color.Black, width,
        //                        height, true, true, g);
        //            }
        //            break;
        //    }
        //}

        #endregion

        #region Edit legend break
        /// <summary>
        /// Set legend break color
        /// </summary>
        /// <param name="aColor"></param>
        public void SetLegendBreak_Color(Color aColor)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                _legendScheme.LegendBreaks[rowIdx].Color = aColor;                
            }

            legendView1.Update(_legendScheme);            
        }

        /// <summary>
        /// Set legend break outline color
        /// </summary>
        /// <param name="aColor">color</param>
        public void SetLegendBreak_OutlineColor(Color aColor)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                switch (_legendScheme.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPB.OutlineColor = aColor;
                        _legendScheme.LegendBreaks[rowIdx] = aPB;
                        break;                    
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPGB.OutlineColor = aColor;
                        _legendScheme.LegendBreaks[rowIdx] = aPGB;
                        break;
                }
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break outline size
        /// </summary>
        /// <param name="outlineSize">color</param>
        public void SetLegendBreak_OutlineSize(float outlineSize)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                switch (_legendScheme.ShapeType)
                {
                    //case ShapeTypes.Point:
                    //    PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                    //    aPB.OutlineColor = aColor;
                    //    _legendScheme.LegendBreaks[rowIdx] = aPB;
                    //    break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPGB.OutlineSize = outlineSize;
                        _legendScheme.LegendBreaks[rowIdx] = aPGB;
                        break;
                }
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break angle
        /// </summary>
        /// <param name="angle">angle</param>
        public void SetLegendBreak_Angle(float angle)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                aPB.Angle = angle;
                _legendScheme.LegendBreaks[rowIdx] = aPB;
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break size
        /// </summary>
        /// <param name="aSize">size</param>
        public void SetLegendBreak_Size(Single aSize)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                switch (_legendScheme.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPB.Size = aSize;
                        _legendScheme.LegendBreaks[rowIdx] = aPB;                        
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineZ:
                        PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPLB.Size = aSize;
                        _legendScheme.LegendBreaks[rowIdx] = aPLB;                        
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPGB.OutlineSize = aSize;
                        _legendScheme.LegendBreaks[rowIdx] = aPGB;                        
                        break;
                }                             
            }
            //legendView1.CurrentCell = null;
            legendView1.Update(_legendScheme);            
        }

        /// <summary>
        /// Set legend break point style
        /// </summary>
        /// <param name="aPS"></param>
        public void SetLegendBreak_PointStyle(PointStyle aPS)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                aPB.Style = aPS;
                _legendScheme.LegendBreaks[rowIdx] = aPB;                                                    
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break polyline style
        /// </summary>
        /// <param name="style"></param>
        public void SetLegendBreak_PolylineStyle(LineStyles style)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                aPLB.Style = style;
                _legendScheme.LegendBreaks[rowIdx] = aPLB;
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break if draw outline
        /// </summary>
        /// <param name="drawOutLine"></param>
        public void SetLegendBreak_DrawOutline(Boolean drawOutLine)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                switch (_legendScheme.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPB.DrawOutline = drawOutLine;
                        _legendScheme.LegendBreaks[rowIdx] = aPB;
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPGB.DrawOutline = drawOutLine;
                        _legendScheme.LegendBreaks[rowIdx] = aPGB;
                        break;
                }
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break if draw fill
        /// </summary>
        /// <param name="drawFill"></param>
        public void SetLegendBreak_DrawFill(Boolean drawFill)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                switch (_legendScheme.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPB.DrawFill = drawFill;
                        _legendScheme.LegendBreaks[rowIdx] = aPB;
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPGB.DrawFill = drawFill;
                        _legendScheme.LegendBreaks[rowIdx] = aPGB;
                        break;
                }
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break if draw shape
        /// </summary>
        /// <param name="drawShape"></param>
        public void SetLegendBreak_DrawShape(Boolean drawShape)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                switch (_legendScheme.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPB.DrawShape = drawShape;
                        _legendScheme.LegendBreaks[rowIdx] = aPB;
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineZ:
                        PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPLB.DrawPolyline = drawShape;
                        _legendScheme.LegendBreaks[rowIdx] = aPLB;
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        aPGB.DrawShape = drawShape;
                        _legendScheme.LegendBreaks[rowIdx] = aPGB;
                        break;
                }                
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break if using hatch style
        /// </summary>
        /// <param name="usginHatchStyle"></param>
        public void SetLegendBreak_UsingHatchStyle(Boolean usginHatchStyle)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                aPGB.UsingHatchStyle = usginHatchStyle;
                _legendScheme.LegendBreaks[rowIdx] = aPGB;
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break hatch style
        /// </summary>
        /// <param name="hatchStyle"></param>
        public void SetLegendBreak_HatchStyle(HatchStyle hatchStyle)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                aPGB.Style = hatchStyle;
                _legendScheme.LegendBreaks[rowIdx] = aPGB;
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break marder type
        /// </summary>
        /// <param name="markerType">marker type</param>
        public void SetLegendBreak_MarkerType(MarkerType markerType)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                aPB.MarkerType = markerType;
                _legendScheme.LegendBreaks[rowIdx] = aPB;
            }
        }

        /// <summary>
        /// Set legend break font name
        /// </summary>
        /// <param name="fontName">font name</param>
        public void SetLegendBreak_FontName(string fontName)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                aPB.FontName = fontName;
                _legendScheme.LegendBreaks[rowIdx] = aPB;
            }
        }

        /// <summary>
        /// Set legend break image path
        /// </summary>
        /// <param name="imagePath">image path</param>
        public void SetLegendBreak_Image(string imagePath)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                aPB.ImagePath = imagePath;
                _legendScheme.LegendBreaks[rowIdx] = aPB;
            }
        }

        /// <summary>
        /// Set legend break marker index
        /// </summary>
        /// <param name="markerIdx">marker index</param>
        public void SetLegendBreak_MarkerIndex(int markerIdx)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                switch (aPB.MarkerType)
                {
                    case MarkerType.Character:
                        aPB.CharIndex = markerIdx;
                        break;
                    case MarkerType.Simple:
                        aPB.Style = (PointStyle)markerIdx;
                        break;
                    case MarkerType.Image:

                        break;
                }                
                _legendScheme.LegendBreaks[rowIdx] = aPB;
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break if draw fill
        /// </summary>
        /// <param name="backColor"></param>
        public void SetLegendBreak_BackColor(Color backColor)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                aPGB.BackColor = backColor;
                _legendScheme.LegendBreaks[rowIdx] = aPGB;
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break if draw symbol
        /// </summary>
        /// <param name="drawSymbol"></param>
        public void SetLegendBreak_DrawSymbol(bool drawSymbol)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                aPLB.DrawSymbol = drawSymbol;
                _legendScheme.LegendBreaks[rowIdx] = aPLB;
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break symbol size
        /// </summary>
        /// <param name="symbolSize"></param>
        public void SetLegendBreak_SymbolSize(Single symbolSize)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                aPLB.SymbolSize = symbolSize;
                _legendScheme.LegendBreaks[rowIdx] = aPLB;
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break symbol size
        /// </summary>
        /// <param name="symbolStyle"></param>
        public void SetLegendBreak_SymbolStyle(PointStyle symbolStyle)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                aPLB.SymbolStyle = symbolStyle;
                _legendScheme.LegendBreaks[rowIdx] = aPLB;
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break symbol color
        /// </summary>
        /// <param name="symbolColor">symbol color</param>
        public void SetLegendBreak_SymbolColor(Color symbolColor)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                aPLB.SymbolColor = symbolColor;
                _legendScheme.LegendBreaks[rowIdx] = aPLB;
            }

            legendView1.Update(_legendScheme);
        }

        /// <summary>
        /// Set legend break symbol interval
        /// </summary>
        /// <param name="symbolInterval">symbol interval</param>
        public void SetLegendBreak_SymbolInterval(int symbolInterval)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                aPLB.SymbolInterval = symbolInterval;
                _legendScheme.LegendBreaks[rowIdx] = aPLB;
            }

            legendView1.Update(_legendScheme);
        }

        #endregion

        #region Events        
                
        private void TSB_Add_Click(object sender, EventArgs e)
        {
            switch (_legendScheme.ShapeType)
            {
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineZ:
                    PolyLineBreak aPLB = new PolyLineBreak();
                    //if (_legendScheme.breakNum > 0)
                    //{
                    //    aPLB = ((PolyLineBreak)_legendScheme.LegendBreaks[0];
                    //}
                    aPLB.DrawPolyline = true;
                    aPLB.Size = 0.1F;
                    aPLB.Color = Color.Red;
                    aPLB.StartValue = 0;
                    aPLB.EndValue = 0;
                    aPLB.Caption = "";
                    _legendScheme.LegendBreaks.Add(aPLB);
                    //_legendScheme.BreakNum = _legendScheme.LegendBreaks.Count;
                    break;
                case ShapeTypes.Point:
                    PointBreak aPB = new PointBreak();
                    //if (_legendScheme.breakNum > 0)
                    //{
                    //    aPB = (PointBreak)_legendScheme.LegendBreaks[0];
                    //}
                    aPB.DrawShape = true;
                    aPB.DrawFill = true;
                    aPB.Size = 5;
                    aPB.Color = Color.Red;
                    aPB.StartValue = 0;
                    aPB.EndValue = 0;
                    aPB.Caption = "";
                    _legendScheme.LegendBreaks.Add(aPB);
                    //_legendScheme.BreakNum = _legendScheme.LegendBreaks.Count;
                    break;
                case ShapeTypes.Polygon:
                    PolygonBreak aPGB = new PolygonBreak();
                    //if (_legendScheme.breakNum > 0)
                    //{
                    //    aPGB = (PolygonBreak)_legendScheme.LegendBreaks[0];
                    //}
                    aPGB.DrawShape = true;
                    aPGB.DrawFill = true;
                    aPGB.Color = Color.Red;
                    aPGB.StartValue = 0;
                    aPGB.EndValue = 0;
                    aPGB.Caption = "";
                    _legendScheme.LegendBreaks.Add(aPGB);
                    //_legendScheme.breakNum = _legendScheme.LegendBreaks.Count;
                    break;
                case ShapeTypes.Image:
                    ColorBreak aCB = new ColorBreak();
                    aCB.Color = Color.Red;
                    aCB.StartValue = 0;
                    aCB.EndValue = 0;
                    aCB.Caption = "";
                    _legendScheme.LegendBreaks.Add(aCB);
                    //_legendScheme.breakNum = _legendScheme.LegendBreaks.Count;
                    break;
            }
            legendView1.Update(_legendScheme);
            B_Apply.Enabled = true;
            B_OK.Enabled = true;
        }

        private void TSB_Del_Click(object sender, EventArgs e)
        {
            int i, rowIdx;
            for (i = 0; i < legendView1.SelectedRows.Count; i++)
            {
                rowIdx = legendView1.SelectedRows[i];
                if (rowIdx < _legendScheme.LegendBreaks.Count)
                    _legendScheme.LegendBreaks.RemoveAt(rowIdx);
            }
            legendView1.Update(_legendScheme);
            if (_legendScheme.BreakNum == 0)
            {
                B_Apply.Enabled = false;
                B_OK.Enabled = false;
            }
        }

        private void TSB_DelAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("If remove all breaks?", "Alarm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _legendScheme.LegendBreaks.Clear();
                legendView1.Update(_legendScheme);
                B_Apply.Enabled = false;
                B_OK.Enabled = false;
            }
        }

        private void TSB_Up_Click(object sender, EventArgs e)
        {
            int oldIdx, newIdx;
            oldIdx = legendView1.SelectedRows[0];
            if (oldIdx > 0)
            {
                if (_legendScheme.ShapeType == ShapeTypes.Point)
                {
                    PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[oldIdx];
                    if (aPB.IsNoData)
                    {
                        return;
                    }
                }
                newIdx = oldIdx - 1;
                _legendScheme.LegendBreaks.Insert(newIdx, _legendScheme.LegendBreaks[oldIdx]);
                _legendScheme.LegendBreaks.RemoveAt(oldIdx + 1);
                legendView1.SelectedRows.Clear();
                legendView1.SelectedRows.Add(newIdx);
                legendView1.Update(_legendScheme);
            }
        }

        private void TSB_Down_Click(object sender, EventArgs e)
        {
            int oldIdx, newIdx, endIdx;
            oldIdx = legendView1.SelectedRows[0];
            endIdx = legendView1.LegendScheme.BreakNum - 1;
            if (_legendScheme.ShapeType == ShapeTypes.Point)
            {
                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[oldIdx];
                if (aPB.IsNoData)
                {
                    endIdx = endIdx - 1;
                }
            }
            if (oldIdx < endIdx)
            {
                newIdx = oldIdx + 2;
                _legendScheme.LegendBreaks.Insert(newIdx, _legendScheme.LegendBreaks[oldIdx]);
                _legendScheme.LegendBreaks.RemoveAt(oldIdx);
                legendView1.SelectedRows.Clear();
                legendView1.SelectedRows.Add(newIdx - 1);
                legendView1.Update(_legendScheme);
            }
        }               

        private void TSB_MakeBreaks_Click(object sender, EventArgs e)
        {
            bool isUniqueValue = false;
            if (_legendScheme.LegendType == LegendType.UniqueValue)
                isUniqueValue = true;

            frmLegendBreaks frmLB = new frmLegendBreaks(this, isUniqueValue);
            frmLB.SetLegendScheme(_legendScheme);
            frmLB.Show(this);
        }

        //private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    int rowIdx = e.RowIndex;
        //    string caption;
        //    if (e.ColumnIndex == 1)
        //    {
        //        string aValue = legendView1[e.ColumnIndex, e.RowIndex].Value.ToString().Trim();                
        //        double sValue, eValue;
        //        int aIdx;
                
        //        aIdx = aValue.IndexOf("-");
        //        if (aIdx > 0)
        //        {
        //            if (aValue.Substring(aIdx - 1, 1) == "E")
        //                aIdx = aValue.IndexOf("-", aIdx + 1);
        //            sValue = Convert.ToDouble(aValue.Substring(0, aIdx).Trim());
        //            eValue = Convert.ToDouble(aValue.Substring(aIdx + 1).Trim());
        //            aValue = aValue.Substring(0, aIdx).Trim() + " - " + aValue.Substring(aIdx + 1).Trim();
        //            DataGridView1[e.ColumnIndex, e.RowIndex].Value = aValue;
        //        }
        //        else if (aIdx == 0)
        //        {
        //            aIdx = aValue.Substring(1).IndexOf("-");
        //            if (aIdx > 0)
        //            {
        //                aIdx += 1;
        //                sValue = Convert.ToDouble(aValue.Substring(0, aIdx).Trim());
        //                eValue = Convert.ToDouble(aValue.Substring(aIdx + 1).Trim());
        //                aValue = aValue.Substring(0, aIdx).Trim() + " - " + aValue.Substring(aIdx + 1).Trim();
        //                DataGridView1[e.ColumnIndex, e.RowIndex].Value = aValue;
        //            }
        //            else
        //            {
        //                sValue = Convert.ToDouble(aValue);
        //                eValue = sValue;
        //            }
        //        }
        //        else
        //        {
        //            sValue = Convert.ToDouble(aValue);
        //            eValue = sValue;
        //        }
        //        DataGridView1[2, e.RowIndex].Value = aValue;

        //        caption = aValue;
        //        switch (_legendScheme.ShapeType)
        //        {
        //            case ShapeTypes.Point:
        //                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
        //                aPB.Caption = caption;
        //                aPB.StartValue = sValue;
        //                aPB.EndValue = eValue;
        //                _legendScheme.LegendBreaks[rowIdx] = aPB;
        //                break;
        //            case ShapeTypes.Polyline:
        //            case ShapeTypes.PolylineZ:
        //                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
        //                aPLB.Caption = caption;
        //                aPLB.StartValue = sValue;
        //                aPLB.EndValue = eValue;
        //                _legendScheme.LegendBreaks[rowIdx] = aPLB;
        //                break;
        //            case ShapeTypes.Polygon:
        //                PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
        //                aPGB.Caption = caption;
        //                aPGB.StartValue = sValue;
        //                aPGB.EndValue = eValue;
        //                _legendScheme.LegendBreaks[rowIdx] = aPGB;
        //                break;
        //            case ShapeTypes.Image:
        //                ColorBreak aCB = (ColorBreak)_legendScheme.LegendBreaks[rowIdx];
        //                aCB.Caption = caption;
        //                aCB.StartValue = sValue;
        //                aCB.EndValue = eValue;
        //                _legendScheme.LegendBreaks[rowIdx] = aCB;
        //                break;
        //        }
        //    }
        //    if (e.ColumnIndex == 2)
        //    {
        //        caption = DataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
        //        switch (_legendScheme.ShapeType)
        //        {
        //            case ShapeTypes.Point:
        //                PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
        //                aPB.Caption = caption;
        //                _legendScheme.LegendBreaks[rowIdx] = aPB;
        //                break;
        //            case ShapeTypes.Polyline:
        //            case ShapeTypes.PolylineZ:
        //                PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
        //                aPLB.Caption = caption;
        //                _legendScheme.LegendBreaks[rowIdx] = aPLB;
        //                break;
        //            case ShapeTypes.Polygon:
        //                PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
        //                aPGB.Caption = caption;
        //                _legendScheme.LegendBreaks[rowIdx] = aPGB;
        //                break;
        //            case ShapeTypes.Image:
        //                ColorBreak aCB = (ColorBreak)_legendScheme.LegendBreaks[rowIdx];
        //                aCB.Caption = caption;                        
        //                _legendScheme.LegendBreaks[rowIdx] = aCB;
        //                break;
        //        }
        //    }
        //}

        private void B_Apply_Click(object sender, EventArgs e)
        {
            //if (m_IfFrmMeteoData)
            //{                
            //    m_LayersTV.RemoveLayerHandle(m_LayerHandle, false);
            //    MeteoInfoC.Data.MeteoData.DrawMeteoData CDrawMD = new MeteoInfoC.Data.MeteoData.DrawMeteoData();
            //    VectorLayer aLayer=CDrawMD .
            //    if (m_LayersTV.FindForm().GetType() == typeof(frmMain))
            //    {
            //        //frmMeteoData.pCurrenWin.DrawMeteoMap(true, _legendScheme);
            //        m_FrmMeteoData.DrawMeteoMap(true, _legendScheme);
            //    }
            //    else
            //    {
            //        ((frmSectionPlot)m_LayersTV.FindForm()).DrawMeteoMap(true, _legendScheme);

            //    }

            //    //frmMain.pCurrenWin.PicBox_Layout.Refresh();
            //    //m_LayersTV.m_MapPicture.Refresh();
            //}
            //else
            //{
            //    m_LayersTV.SetLayerLegendScheme(m_LayerHandle, _legendScheme);
            //}

            //m_LayersTV.SetLayerLegendScheme(m_LayerHandle, _legendScheme);

            m_IsApplied = true;            
        }

        private void B_Cancel_Click(object sender, EventArgs e)
        {
            //frmMeteoData.pCurrenWin.DrawMeteoMap(false, frmMeteoData.pCurrenWin._legendScheme);
            //frmMain.pCurrenWin.pictureBox1.Refresh();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void B_OK_Click(object sender, EventArgs e)
        {
            //frmMeteoData.pCurrenWin.SetLegendScheme(_legendScheme);
            //frmMeteoData.pCurrenWin.DrawMeteoMap(false, frmMeteoData.pCurrenWin._legendScheme);
            //frmMain.pCurrenWin.pictureBox1.Refresh();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void TSB_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "LegendScheme file (*.lgs)|*.lgs";
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {                
                _legendScheme.ExportToXMLFile(saveDlg.FileName);
            }
        }

        private void TSB_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog oDlg = new OpenFileDialog();
            oDlg.Filter = "LegendScheme file (*.lgs,*.pal)|*.lgs;*.pal";
            if (oDlg.ShowDialog() == DialogResult.OK)
            {                
                string aFile = oDlg.FileName;
                switch (Path.GetExtension(aFile).ToLower())
                {
                    case ".pal":
                        _legendScheme.ImportFromPaletteFile_Unique(aFile);
                        break;
                    case ".lgs":
                        _legendScheme.ImportFromXMLFile(aFile);                        
                        break;
                }

                m_IfCreateLegendscheme = false;
                CB_LegendType.Text = _legendScheme.LegendType.ToString();
                if (_legendScheme.FieldName == null || _legendScheme.FieldName == "")
                {
                    _legendScheme.FieldName = CB_Field.Text;
                }
                CB_Field.Text = _legendScheme.FieldName;
                legendView1.Update(_legendScheme);
                m_IfCreateLegendscheme = true;            
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIdx = e.RowIndex;
            switch (_legendScheme.ShapeType)
            {
                case ShapeTypes.Point:
                    if (_frmPointSymbolSet.Visible)
                    {
                        PointBreak aPB = (PointBreak)_legendScheme.LegendBreaks[rowIdx];
                        _frmPointSymbolSet.PointBreak = aPB;
                    }
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineZ:
                    if (_frmPolylineSymbolSet.Visible)
                    {
                        PolyLineBreak aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[rowIdx];
                        _frmPolylineSymbolSet.PolylineBreak = aPLB;
                    }
                    break;
                case ShapeTypes.Polygon:
                    if (_frmPolygonSymbolSet.Visible)
                    {
                        PolygonBreak aPGB = (PolygonBreak)_legendScheme.LegendBreaks[rowIdx];
                        _frmPolygonSymbolSet.PolygonBreak = aPGB;
                    }
                    break;
                case ShapeTypes.Image:
                    if (_frmColorSymbolSet.Visible)
                    {
                        ColorBreak aCB = _legendScheme.LegendBreaks[rowIdx];
                        _frmColorSymbolSet.ColorBreak = aCB;
                    }
                    break;
            }            
        }

        private void frmLegendSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (m_FrmSS != null)
            //{ m_FrmSS = null; }
        }

        private void CB_LegendType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LegendType aLT = (LegendType)Enum.Parse(typeof(LegendType), CB_LegendType.Text, true);
            SetFieldByLegendType(aLT);            
        }

        private void SetFieldByLegendType(LegendType aLT)
        {
            //MapLayer aMLayer = m_LayersTV.MapView.GetLayerFromHandle(m_LayerHandle);
            if (_mapLayer.LayerType == LayerTypes.VectorLayer)
            {
                VectorLayer aLayer = (VectorLayer)_mapLayer;
                switch (aLT)
                {
                    case LegendType.SingleSymbol:
                        CB_Field.Enabled = false;
                        CB_Field.Text = "<None>";
                        CreateLegendScheme(aLT, "");
                        break;
                    case LegendType.UniqueValue:
                        CB_Field.Enabled = true;
                        CB_Field.Items.Clear();
                        foreach (string fn in aLayer.GetFieldNameList())
                        {
                            CB_Field.Items.Add(fn);
                        }
                        CB_Field.Text = "<None>";
                        break;
                    case LegendType.GraduatedColor:
                        CB_Field.Enabled = true;
                        CB_Field.Items.Clear();                        
                        for (int i = 0; i < aLayer.NumFields; i++)
                        {
                            if (MIMath.IsNumeric(aLayer.GetField(i)))
                            {
                                CB_Field.Items.Add(aLayer.GetFieldName(i));
                            }
                        }
                        CB_Field.Text = "<None>";
                        break;
                }
            }
        }

        private void CreateLegendScheme(LegendType aLT, string fieldName)
        {
            if (m_IfCreateLegendscheme)
            {                
                double min, max;
                ShapeTypes aST = _legendScheme.ShapeType;

                min = _legendScheme.MinValue;
                max = _legendScheme.MaxValue;
                switch (aLT)
                {
                    case LegendType.SingleSymbol:
                        Color aColor = Color.Black;
                        switch (aST)
                        {
                            case ShapeTypes.Point:
                                aColor = Color.Black;
                                break;
                            case ShapeTypes.Polyline:
                            case ShapeTypes.PolylineZ:
                                aColor = Color.Black;
                                break;
                            case ShapeTypes.Polygon:
                            case ShapeTypes.Image:
                                aColor = Color.FromArgb(255, 251, 195);
                                break;
                        }
                        Single size = 0.1F;
                        if (_legendScheme.ShapeType == ShapeTypes.Point)
                        {
                            size = 5;
                        }
                        _legendScheme = LegendManage.CreateSingleSymbolLegendScheme(_legendScheme.ShapeType, aColor,
                            size);

                        TSB_Add.Enabled = false;
                        TSB_Del.Enabled = false;
                        TSB_DelAll.Enabled = false;
                        TSB_Down.Enabled = false;
                        TSB_Up.Enabled = false;
                        TSB_MakeBreaks.Enabled = false;
                        TSB_Reverse.Enabled = false;
                        break;
                    case LegendType.UniqueValue:                                                
                        Color[] colors;                        
                        List<string> valueList = new List<string>();

                        VectorLayer aLayer = (VectorLayer)_mapLayer;                        
                        bool isDateField = false;
                        Type colType = aLayer.AttributeTable.Table.Columns[fieldName].DataType;
                        if (colType == typeof(DateTime))
                            isDateField = true;

                        List<string> captions = new List<string>();

                        for (int i = 0; i < aLayer.AttributeTable.Table.Rows.Count; i++)
                        {
                            if (!valueList.Contains(aLayer.AttributeTable.Table.Rows[i][fieldName].ToString()))
                            {                                
                                valueList.Add(aLayer.AttributeTable.Table.Rows[i][fieldName].ToString());
                                if (isDateField)
                                    captions.Add(((DateTime)aLayer.AttributeTable.Table.Rows[i][fieldName]).ToString("yyyy/M/d"));
                            }
                        }

                        if (valueList.Count == 1)
                        {
                            MessageBox.Show("The values of all shapes are same!", "Alarm");
                            break;
                        }

                        if (valueList.Count <= 13)
                        {
                            colors = LegendManage.CreateRainBowColors(valueList.Count);
                        }
                        else
                        {
                            colors = LegendManage.CreateRandomColors(valueList.Count);
                        }
                        List<Color> CList = new List<Color>(colors);
                        CList.Insert(0, Color.White);
                        colors = CList.ToArray();

                        if (isDateField)
                            _legendScheme = LegendManage.CreateUniqValueLegendScheme(valueList, captions, colors, aST, min,
                                max, _legendScheme.HasNoData, _legendScheme.MissingValue);
                        else
                            _legendScheme = LegendManage.CreateUniqValueLegendScheme(valueList, colors,
                                aST, min, max, _legendScheme.HasNoData, _legendScheme.MissingValue);

                        _legendScheme.FieldName = fieldName;

                        TSB_Add.Enabled = true;
                        TSB_Del.Enabled = true;
                        TSB_DelAll.Enabled = true;
                        TSB_Down.Enabled = true;
                        TSB_Up.Enabled = true;
                        TSB_MakeBreaks.Enabled = true;
                        TSB_Reverse.Enabled = true;
                        break;
                    case LegendType.GraduatedColor:
                        aLayer = (VectorLayer)_mapLayer;
                        double[] S = new double[aLayer.AttributeTable.Table.Rows.Count];
                        for (int i = 0; i < S.Length; i++)
                        {
                            S[i] = double.Parse(aLayer.AttributeTable.Table.Rows[i][fieldName].ToString());
                        }
                        MIMath.GetMaxMinValue(S, _legendScheme.MissingValue, ref min, ref max);
                        
                        
                        if (min == max)
                        {
                            MessageBox.Show("The values of all shapes are same!", "Alarm");
                            break;
                        }

                        double[] CValues;
                        CValues = LegendManage.CreateContourValues(min, max);
                        colors = LegendManage.CreateRainBowColors(CValues.Length + 1);

                        _legendScheme = LegendManage.CreateGraduatedLegendScheme(CValues, colors,
                            aST, min, max, _legendScheme.HasNoData, _legendScheme.MissingValue);
                        _legendScheme.FieldName = fieldName;

                        TSB_Add.Enabled = true;
                        TSB_Del.Enabled = true;
                        TSB_DelAll.Enabled = true;
                        TSB_Down.Enabled = true;
                        TSB_Up.Enabled = true;
                        TSB_MakeBreaks.Enabled = true;
                        TSB_Reverse.Enabled = true;
                        break;
                }

                //ShowLegendScheme(_legendScheme);
                //DataGridView1.Refresh();
                legendView1.LegendScheme = _legendScheme;
                legendView1.Invalidate();
            }
        }

        private void TSB_Reverse_Click(object sender, EventArgs e)
        {
            LegendScheme aLS = (LegendScheme)_legendScheme.Clone();
            int aNum;
            if (_legendScheme.HasNoData)
            {
                aNum = _legendScheme.BreakNum - 1;
            }
            else
            {
                aNum = _legendScheme.BreakNum;
            }
            _legendScheme.LegendBreaks.Reverse(0, aNum);

            //switch (_legendScheme.ShapeType)
            //{
            //    case ShapeType.Point:
            //        PointBreak aPB = new PointBreak();
            //        PointBreak bPB = new PointBreak();
            //        for (int i = 0; i < aNum; i++)
            //        {
            //            if (i == aNum - i - 1)
            //            {
            //                continue;
            //            }
            //            else
            //            {
            //                aPB = (PointBreak)_legendScheme.LegendBreaks[i];
            //                bPB = (PointBreak)aLS.LegendBreaks[aNum - i - 1];
            //                aPB.color = bPB.color;
            //                aPB.drawFill = bPB.drawFill;
            //                aPB.drawOutline = bPB.drawOutline;
            //                aPB.drawPoint = bPB.drawPoint;
            //                aPB.isNoData = bPB.isNoData;
            //                aPB.outlineColor = bPB.outlineColor;
            //                aPB.size = bPB.size;
            //                aPB.style = bPB.style;
            //                _legendScheme.LegendBreaks[i] = aPB;
            //            }
            //        }
            //        break;
            //    case ShapeType.Polyline:
            //        PolyLineBreak aPLB = new PolyLineBreak();
            //        PolyLineBreak bPLB = new PolyLineBreak();
            //        for (int i = 0; i < aNum; i++)
            //        {
            //            if (i == aNum - i - 1)
            //            {
            //                continue;
            //            }
            //            else
            //            {
            //                aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[i];
            //                bPLB = (PolyLineBreak)aLS.LegendBreaks[aNum - i - 1];
            //                aPLB.color = bPLB.color;                            
            //                aPLB.size = bPLB.size;
            //                aPLB.style = bPLB.style;
            //                aPLB.drawLine = bPLB.drawLine;                            
            //                _legendScheme.LegendBreaks[i] = aPLB;
            //            }
            //        }
            //        break;
            //    case ShapeType.Polygon:
            //        PolygonBreak aPGB = new PolygonBreak();
            //        PolygonBreak bPGB = new PolygonBreak();
            //        for (int i = 0; i < aNum; i++)
            //        {
            //            if (i == aNum - i - 1)
            //            {
            //                continue;
            //            }
            //            else
            //            {
            //                aPGB = (PolygonBreak)_legendScheme.LegendBreaks[i];
            //                bPGB = (PolygonBreak)aLS.LegendBreaks[aNum - i - 1];
            //                aPGB.color = bPGB.color;
            //                aPGB.drawFill = bPGB.drawFill;
            //                aPGB.drawOutline = bPGB.drawOutline;
            //                aPGB.drawPolygon = bPGB.drawPolygon;
            //                aPGB.outlineColor = bPGB.outlineColor;
            //                aPGB.outlineSize = bPGB.outlineSize;
            //                _legendScheme.LegendBreaks[i] = aPGB;
            //            }
            //        }
            //        break;
            //}


            legendView1.Update(_legendScheme);
        }

        private void CB_Field_SelectedIndexChanged(object sender, EventArgs e)
        {
            LegendType aLT = (LegendType)Enum.Parse(typeof(LegendType), CB_LegendType.Text, true);
            string fieldName = CB_Field.Text;
            if (fieldName != "<None>")
            {
                CreateLegendScheme(aLT, fieldName);                
            }
        }

        #endregion

        private void legendView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
        
    }
}
