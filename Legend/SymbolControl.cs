using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using MeteoInfoC.Drawing;
using MeteoInfoC.Shape;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Marker Control
    /// </summary>
    public partial class SymbolControl : UserControl
    {
        #region Events definitation
        /// <summary>
        /// Occurs after selected cell changed.
        /// </summary>
        public event EventHandler SelectedCellChanged;

        #endregion

        #region private variables
        private ShapeTypes _shapeType;
        private MarkerType _markerType;
        private Size _cellSize;
        private int _symbolNumber;
        private int _colNumber;
        private int _rowNumber;
        private int _selectedCell;
        private List<Image> _imageList;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public SymbolControl()
        {
            InitializeComponent();

            this.AllowDrop = true;
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);

            _shapeType = ShapeTypes.Point;
            _markerType = MarkerType.Simple;
            _cellSize = new Size(25, 25);
            _symbolNumber = 256;
            _colNumber = 10;
            _rowNumber = 26;
            _selectedCell = -1;
            _imageList = new List<Image>();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set shape type
        /// </summary>
        public ShapeTypes ShapeType
        {
            get { return _shapeType; }
            set 
            {
                _shapeType = value;
                switch (_shapeType)
                {
                    case ShapeTypes.Point:
                        SymbolNumber = 256;
                        break;
                    case ShapeTypes.Polyline:
                        SymbolNumber = Enum.GetNames(typeof(LineStyles)).Length;
                        break;
                    case ShapeTypes.Polygon:
                        SymbolNumber = Enum.GetNames(typeof(HatchStyle)).Length;
                        break;
                }
            }
        }

        /// <summary>
        /// Get or set marker type
        /// </summary>
        public MarkerType MarkerType
        {
            get { return _markerType; }
            set 
            { 
                _markerType = value;
                if (_selectedCell < 0 || _selectedCell >= _symbolNumber)
                    _selectedCell = 0;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Get or set cell size
        /// </summary>
        public Size CellSize
        {
            get { return _cellSize; }
            set { _cellSize = value; }
        }

        /// <summary>
        /// Get or set selected cell
        /// </summary>
        public int SelectedCell
        {
            get { return _selectedCell; }
            set { _selectedCell = value; }
        }

        /// <summary>
        /// Get or set symbol number
        /// </summary>
        public int SymbolNumber
        {
            get { return _symbolNumber; }
            set
            {
                _symbolNumber = value;
                _rowNumber = (int)Math.Ceiling((float)_symbolNumber / _colNumber);
            }
        }

        /// <summary>
        /// Get or set column number
        /// </summary>
        public int ColumnNumber
        {
            get { return _colNumber; }
            set { _colNumber = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Set image list
        /// </summary>
        /// <param name="imageList">image list</param>
        public void SetIamgeList(List<Image> imageList)
        {
            _imageList = new List<Image>(imageList);
            SymbolNumber = imageList.Count;
        }

        #endregion

        #region Events
        /// <summary>
        /// Override OnPaint event
        /// </summary>
        /// <param name="e">paint event args</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            PaintGraphics(g);
            //g.Dispose();
        }

        /// <summary>
        /// Override OnPaintBackground event
        /// </summary>
        /// <param name="e">PaintEventArgs</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        private void PaintGraphics(Graphics g)
        {
            int TotalHeight = CalcTotalDrawHeight();
            Rectangle rect;
            if (TotalHeight > this.Height)
            {
                _vScrollBar.Minimum = 0;
                _vScrollBar.SmallChange = _cellSize.Height;
                _vScrollBar.LargeChange = this.Height;
                _vScrollBar.Maximum = TotalHeight;

                if (_vScrollBar.Visible == false)
                {
                    _vScrollBar.Value = 0;
                    _vScrollBar.Visible = true;
                }
                rect = new Rectangle(0, -_vScrollBar.Value, this.Width - _vScrollBar.Width, TotalHeight);
            }
            else
            {
                _vScrollBar.Visible = false;
                rect = new Rectangle(0, 0, this.Width, this.Height);
            }           

            DrawCells(g, rect.Y);
        }

        private void DrawCells(Graphics g, int sHeight)
        {
            int hideRows = 0;
            switch (_shapeType)
            {
                case ShapeTypes.Point:
                    switch (_markerType)
                    {
                        case MarkerType.Character:
                            Font smallFont = new Font(Font.FontFamily, _cellSize.Width * .8F, GraphicsUnit.Pixel);
                            for (int i = 0; i < _symbolNumber; i++)
                            {
                                int row = i / _colNumber;
                                if (row > hideRows)
                                {
                                    sHeight += _cellSize.Height;
                                    hideRows = row;
                                }
                                if (sHeight + _cellSize.Height < this.ClientRectangle.Top)
                                    continue;

                                int col = i % _colNumber;
                                if (i == _selectedCell)
                                    g.FillRectangle(Brushes.LightSkyBlue, col * _cellSize.Width, sHeight, _cellSize.Width, _cellSize.Height);

                                string text = ((char)i).ToString();
                                g.DrawString(text, smallFont, Brushes.Black, new System.Drawing.PointF((float)(col * _cellSize.Width), (float)(sHeight)));
                            }
                            break;
                        case MarkerType.Simple:
                            PointBreak aPB = new PointBreak();
                            aPB.Color = Color.Black;
                            aPB.DrawOutline = false;
                            aPB.Size = _cellSize.Width * 0.8f;
                            for (int i = 0; i < _symbolNumber; i++)
                            {
                                int row = i / _colNumber;
                                if (row > hideRows)
                                {
                                    sHeight += _cellSize.Height;
                                    hideRows = row;
                                }
                                if (sHeight + _cellSize.Height < this.ClientRectangle.Top)
                                    continue;

                                int col = i % _colNumber;
                                if (i == _selectedCell)
                                    g.FillRectangle(Brushes.LightSkyBlue, col * _cellSize.Width, sHeight, _cellSize.Width, _cellSize.Height);

                                PointF sP = new PointF(col * _cellSize.Width + _cellSize.Width / 2,
                                    sHeight + _cellSize.Height / 2);
                                aPB.Style = (PointStyle)i;
                                Draw.DrawPoint(sP, aPB, g);
                            }
                            break;
                        case MarkerType.Image:
                            float size = _cellSize.Width * 0.8f;
                            for (int i = 0; i < _symbolNumber; i++)
                            {
                                int row = i / _colNumber;
                                if (row > hideRows)
                                {
                                    sHeight += _cellSize.Height;
                                    hideRows = row;
                                }
                                if (sHeight + _cellSize.Height < this.ClientRectangle.Top)
                                    continue;

                                int col = i % _colNumber;
                                if (i == _selectedCell)
                                    g.FillRectangle(Brushes.LightSkyBlue, col * _cellSize.Width, sHeight, _cellSize.Width, _cellSize.Height);

                                ((Bitmap)_imageList[i]).MakeTransparent(Color.White);
                                g.DrawImage(_imageList[i], col * _cellSize.Width, sHeight, size, size);
                            }
                            break;
                    }
                    break;
                case ShapeTypes.Polyline:
                    PolyLineBreak aPLB = new PolyLineBreak();
                    aPLB.Size = 2;
                    aPLB.Color = Color.Black;
                    for (int i = 0; i < _symbolNumber; i++)
                    {
                        int row = i / _colNumber;
                        if (row > hideRows)
                        {
                            sHeight += _cellSize.Height;
                            hideRows = row;
                        }
                        if (sHeight + _cellSize.Height < this.ClientRectangle.Top)
                            continue;

                        int col = i % _colNumber;
                        Rectangle rect = new Rectangle(col * _cellSize.Width, sHeight + _cellSize.Height / 4, _cellSize.Width, _cellSize.Height / 2);
                        if (i == _selectedCell)
                            g.FillRectangle(Brushes.LightSkyBlue, rect);

                        aPLB.Style = (LineStyles)(Enum.GetValues(typeof(LineStyles)).GetValue(i));
                        Draw.DrawPolyLineSymbol(new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2),
                            rect.Width * 0.8f, rect.Height * 0.8f, aPLB, g);
                    }
                    break;                    
                case ShapeTypes.Polygon:
                    PolygonBreak aPGB = new PolygonBreak();
                    aPGB.Color = Color.Red;
                    aPGB.OutlineColor = Color.Black;                    
                    for (int i = 0; i < _symbolNumber; i++)
                    {
                        int row = i / _colNumber;
                        if (row > hideRows)
                        {
                            sHeight += _cellSize.Height;
                            hideRows = row;
                        }
                        if (sHeight + _cellSize.Height < this.ClientRectangle.Top)
                            continue;

                        int col = i % _colNumber;
                        Rectangle rect = new Rectangle(col * _cellSize.Width, sHeight, _cellSize.Width, _cellSize.Height);
                        if (i == _selectedCell)
                            g.FillRectangle(Brushes.LightSkyBlue, rect);

                        if (i == 0)
                            aPGB.UsingHatchStyle = false;
                        else
                        {
                            aPGB.UsingHatchStyle = true;
                            aPGB.Style = (HatchStyle)(Enum.GetValues(typeof(HatchStyle)).GetValue(i - 1));
                        }
                        Draw.DrawPolygonSymbol(new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2),
                            rect.Width * 0.8f, rect.Height * 0.8f, aPGB, 0, g);
                    }
                    break;
            }
        }

        private int CalcTotalDrawHeight()
        {            
            return _cellSize.Height * _rowNumber;
        }               

        /// <summary>
        /// Override OnResize event
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.Width > 0 && this.Height > 0)
            {
                //m_BackBuffer = new Bitmap(this.Width, this.Height);
                //m_Draw = Graphics.FromImage(m_BackBuffer);
                _vScrollBar.Top = 0;
                _vScrollBar.Height = this.Height;
                _vScrollBar.Left = this.Width - _vScrollBar.Width;
            }
            this.Invalidate();
        }       

        private void _vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            VScrollBar sbar = (VScrollBar)sender;
            sbar.Value = e.NewValue;
            this.Invalidate();
        }

        /// <summary>
        /// Override OnMouseClick event
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            int col = e.X / _cellSize.Width;
            int row = (e.Y + _vScrollBar.Value) / _cellSize.Height;
            int sel = row * _colNumber + col;
            if (sel < _symbolNumber)
            {
                _selectedCell = row * _colNumber + col;
                //((frmPointSymbolSet)this.ParentForm).PointBreak.CharIndex = _selectedCell;
                this.Invalidate();

                OnSelectedCellChanged();
            }
        }

        /// <summary>
        /// Fires the SelectedCellChanged event
        /// </summary>
        protected virtual void OnSelectedCellChanged()
        {
            if (SelectedCellChanged != null) SelectedCellChanged(this, new EventArgs());           
        }

        #endregion
    }
}
