using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using MeteoInfoC.Shape;
using MeteoInfoC.Legend;
using MeteoInfoC.Global;
using MeteoInfoC.Drawing;
using MeteoInfoC.Layer;

namespace MeteoInfoC.Layout
{
    /// <summary>
    /// Layout graphic element
    /// </summary>
    public class LayoutGraphic:LayoutElement
    {
        #region Variables    
        private MapLayout _mapLayout;
        private LayoutMap _layoutMap = null;
        private bool _IsPaint;
        private Graphic _graphic;
        private SmoothingMode _smoothingMode = SmoothingMode.Default;
        private bool _isTitle = false;
        private bool _updatingSize = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>        
        public LayoutGraphic(Graphic aGraphic, MapLayout aMapLayout):base()
        {
            ElementType = ElementType.LayoutGraphic;
            ResizeAbility = ResizeAbility.ResizeAll;

            _mapLayout = aMapLayout;                                                 
            _IsPaint = true;
            Graphic = aGraphic;
            if (_graphic.Legend != null)
            {
                if (_graphic.Legend.GetType() == typeof(LabelBreak))
                    ((LabelBreak)_graphic.Legend).SizeChanged += new EventHandler(this.LabelSizeChange);
            }
            //UpdateControlSize();
        }

        /// <summary>
        /// Constructor
        /// </summary>        
        public LayoutGraphic(Graphic aGraphic, MapLayout aMapLayout, LayoutMap aLayoutMap)
            : base()
        {
            ElementType = ElementType.LayoutGraphic;
            ResizeAbility = ResizeAbility.ResizeAll;

            _mapLayout = aMapLayout;            
            _IsPaint = true;
            Graphic = aGraphic;
            if (_graphic.Legend != null)
            {
                if (_graphic.Legend.GetType() == typeof(LabelBreak))
                    ((LabelBreak)_graphic.Legend).SizeChanged += new EventHandler(this.LabelSizeChange);
            }
            //UpdateControlSize();

            _layoutMap = aLayoutMap;
            _layoutMap.MapViewUpdated += new EventHandler(LayoutMap_MapViewUpdated);
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set if this is title
        /// </summary>
        public bool IsTitle
        {
            get { return _isTitle; }
            set { _isTitle = value; }
        }

        /// <summary>
        /// Get or set if paint itself
        /// </summary>
        public bool IsPaint
        {
            get { return _IsPaint; }
            set { _IsPaint = value; }
        }

        /// <summary>
        /// Get or set graphic
        /// </summary>
        public Graphic Graphic
        {
            get { return _graphic; }
            set 
            {
                _graphic = value;
                if (_graphic.Shape != null)
                {
                    switch (_graphic.Shape.ShapeType)
                    {
                        case ShapeTypes.Point:
                            if (_graphic.Legend.GetType() == typeof(PointBreak))
                                ResizeAbility = ResizeAbility.SameWidthHeight;
                            else if (_graphic.Legend.GetType() == typeof(LabelBreak))
                                ResizeAbility = ResizeAbility.None;
                            break;
                        case ShapeTypes.Circle:
                            ResizeAbility = ResizeAbility.SameWidthHeight;
                            break;
                        default:
                            ResizeAbility = ResizeAbility.ResizeAll;
                            break;
                    }                    
                    UpdateControlSize();
                }
            }
        }

        /// <summary>
        /// Get or set smoothing mode
        /// </summary>
        public SmoothingMode SmoothingMode
        {
            get { return _smoothingMode; }
            set { _smoothingMode = value; }
        }

        

        #endregion

        #region Methods
        /// <summary>
        /// Set properties
        /// </summary>
        public void SetProperties()
        {
            Dictionary<string, string> objAttr = new Dictionary<string, string>();
            objAttr.Add("Text", "Text");
            objAttr.Add("Font", "Font");
            objAttr.Add("ForeColor", "Fore Color");
            objAttr.Add("BackColor", "Back Color");
            CustomProperty cp = new CustomProperty(this, objAttr);
            frmProperty aFrmP = new frmProperty();
            aFrmP.SetObject(cp);
            aFrmP.ShowDialog();
        }

        /// <summary>
        /// Set label text if it is a label break
        /// </summary>
        /// <param name="text"></param>
        public void SetLabelText(string text)
        {
            switch (_graphic.Shape.ShapeType)
            {
                case ShapeTypes.Point:
                    if (_graphic.Legend.GetType() == typeof(LabelBreak))
                    {
                        ((LabelBreak)_graphic.Legend).Text = text;
                        UpdateControlSize();
                    }
                    break;
            }
        }

        /// <summary>
        /// Set font if it is a label break
        /// </summary>
        /// <param name="aFont">font</param>
        public void SetFont(Font aFont)
        {
            switch (_graphic.Shape.ShapeType)
            {
                case ShapeTypes.Point:
                    if (_graphic.Legend.GetType() == typeof(LabelBreak))
                    {
                        ((LabelBreak)_graphic.Legend).Font = aFont;
                        UpdateControlSize();
                    }
                    break;
            }
        }

        /// <summary>
        /// Set font if it is a label break
        /// </summary>
        /// <param name="fontName">font name</param>
        /// <param name="fontSize">font size</param>
        public void SetFont(string fontName, int fontSize)
        {
            Font aFont = new Font(fontName, fontSize);
            SetFont(aFont);
        }

        /// <summary>
        /// Update control size
        /// </summary>
        public void UpdateControlSize()
        {
            if (_graphic.Shape == null)
                return;

            _updatingSize = true;

            switch (_graphic.Shape.ShapeType)
            {
                case ShapeTypes.Point:
                    PointShape aPS = (PointShape)_graphic.Shape;
                    this.Left = (int)aPS.Point.X;
                    this.Top = (int)aPS.Point.Y;
                    if (_graphic.Legend.GetType() == typeof(PointBreak))
                    {
                        PointBreak aPB = (PointBreak)_graphic.Legend;
                        this.Left -= (int)(aPB.Size / 2);
                        this.Top -= (int)(aPB.Size / 2);
                        this.Width = (int)Math.Ceiling(aPB.Size);
                        this.Height = (int)Math.Ceiling(aPB.Size);
                    }
                    else if (_graphic.Legend.GetType() == typeof(LabelBreak))
                    {
                        LabelBreak aLB = (LabelBreak)_graphic.Legend;
                        SizeF aSF = _mapLayout.CreateGraphics().MeasureString(aLB.Text, aLB.Font);
                        this.Left -= (int)(aSF.Width / 2);
                        this.Top -= (int)(aSF.Height / 2);
                        this.Width = (int)Math.Ceiling(aSF.Width);
                        this.Height = (int)Math.Ceiling(aSF.Height);
                    }
                    break;
                case ShapeTypes.WindArraw:
                    WindArraw aWA = (WindArraw)_graphic.Shape;
                    this.Left = (int)aWA.Point.X;
                    this.Top = (int)aWA.Point.Y;
                    this.Width = (int)(aWA.length * ((VectorBreak)_graphic.Legend).Zoom);
                    this.Height = 20;
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.Polygon:
                case ShapeTypes.Rectangle:
                case ShapeTypes.Circle:
                case ShapeTypes.CurveLine:
                case ShapeTypes.CurvePolygon:
                case ShapeTypes.Ellipse:
                    Extent extent = _graphic.Shape.Extent;
                    this.Left = (int)Math.Ceiling(extent.minX);
                    this.Top = (int)Math.Ceiling(extent.minY);
                    this.Width = (int)Math.Ceiling((extent.Width));
                    this.Height = (int)Math.Ceiling((extent.Height));
                    break;
            }

            _updatingSize = false;
        }

        /// <summary>
        /// Vertice edited update
        /// </summary>
        /// <param name="vIdx">vertice index</param>
        /// <param name="newX">new X</param>
        /// <param name="newY">new Y</param>
        public void VerticeEditUpdate(int vIdx, double newX, double newY)
        {
            List<PointD> points = _graphic.Shape.GetPoints();
            PointD aP = points[vIdx];
            aP.X = newX;
            aP.Y = newY;
            points[vIdx] = aP;
            _graphic.Shape.SetPoints(points);
            UpdateControlSize();
        }

        /// <summary>
        /// Override move update
        /// </summary>
        public override void MoveUpdate()
        {
            if (_graphic.Shape != null)
            {
                List<PointD> points = _graphic.Shape.GetPoints();
                Extent aExtent = _graphic.Shape.Extent;
                double minX = aExtent.minX;
                double minY = aExtent.minY;
                if (_graphic.Shape.ShapeType == ShapeTypes.Point)
                {
                    //if (_graphic.Legend.GetType() == typeof(LabelBreak))
                    //{
                    minX -= this.Width / 2;
                    minY -= this.Height / 2;
                    //}
                }
                int shiftX = this.Left - (int)minX;
                int shiftY = this.Top - (int)minY;
                for (int i = 0; i < points.Count; i++)
                {
                    PointD aP = points[i];
                    aP.X += shiftX;
                    aP.Y += shiftY;
                    points[i] = aP;
                }
                _graphic.Shape.SetPoints(points);
            }
        }

        /// <summary>
        /// Resize shape
        /// </summary>
        public override void  ResizeUpdate()
        {
            if (_graphic.Shape != null)
            {
                switch (_graphic.Shape.ShapeType)
                {
                    case ShapeTypes.Point:
                        if (_graphic.Legend.GetType() == typeof(PointBreak))
                        {
                            PointBreak aPB = (PointBreak)_graphic.Legend;
                            aPB.Size = Width;
                        }
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.Polygon:
                    case ShapeTypes.CurveLine:
                    case ShapeTypes.CurvePolygon:
                        MoveUpdate();
                        List<PointD> points = _graphic.Shape.GetPoints();
                        Extent aExtent = _graphic.Shape.Extent;                        
                        int deltaX = this.Width - (int)aExtent.Width;
                        int deltaY = this.Height - (int)aExtent.Height;

                        for (int i = 0; i < points.Count; i++)
                        {
                            PointD aP = points[i];
                            aP.X = aP.X + deltaX * (aP.X - aExtent.minX) / aExtent.Width;
                            aP.Y = aP.Y + deltaY * (aP.Y - aExtent.minY) / aExtent.Height;
                            points[i] = aP;
                        }
                        _graphic.Shape.SetPoints(points);
                        break;
                    case ShapeTypes.Rectangle:
                    case ShapeTypes.Ellipse:
                        points = new List<PointD>();
                        points.Add(new PointD(this.Left, this.Top));
                        points.Add(new PointD(this.Left, this.Bottom));
                        points.Add(new PointD(this.Right, this.Bottom));
                        points.Add(new PointD(this.Right, this.Top));
                        _graphic.Shape.SetPoints(points);
                        break;
                    case ShapeTypes.Circle:
                        points = new List<PointD>();
                        points.Add(new PointD(this.Left, this.Top + this.Width / 2));
                        points.Add(new PointD(this.Left + this.Width / 2, this.Top));
                        points.Add(new PointD(this.Left + this.Width, this.Top + this.Width / 2));
                        points.Add(new PointD(this.Left + this.Width / 2, this.Top + this.Width));
                        _graphic.Shape.SetPoints(points);
                        break;
                }
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// Override Paint event
        /// </summary>
        /// <param name="g">Graphics</param>
        public override void Paint(Graphics g)
        {
            //Matrix oldMatrix = g.Transform;
            //g.TranslateTransform(this.Left, this.Top);

            //Draw background color            
            g.FillRectangle(new SolidBrush(this.BackColor), this.Left, this.Top, this.Width, this.Height);

            //Draw text
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.SmoothingMode = _smoothingMode;
            PaintGraphics(g);

            ////Draws the selection rectangle around each selected item            
            //if (this.Selected)
            //{
            //    Pen selectionPen = new Pen(Color.Red, 1F);
            //    selectionPen.DashPattern = new[] { 2.0F, 1.0F };
            //    selectionPen.DashCap = DashCap.Round;
            //    Rectangle aRect = this.Bounds;
            //    g.DrawRectangle(selectionPen, 0, 0, this.Width - 1, this.Height - 1);                
            //}

            //g.Transform = oldMatrix;
        }        

        /// <summary>
        /// Paint graphics
        /// </summary>
        /// <param name="g">graphics</param>
        public void PaintGraphics(Graphics g)
        {
            switch (_graphic.Shape.ShapeType)
            {
                case ShapeTypes.Point:
                    PointD dPoint = _graphic.Shape.GetPoints()[0];
                    PointF aPoint = new PointF((float)dPoint.X, (float)dPoint.Y);
                    if (_graphic.Legend.GetType() == typeof(PointBreak))
                    {
                        PointBreak aPB = (PointBreak)_graphic.Legend;
                        //PointF aPoint = new PointF(this.Left + (float)this.Width / 2, this.Top + (float)this.Height / 2);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        Draw.DrawPoint(aPoint, aPB, g);
                        g.SmoothingMode = _smoothingMode;
                    }
                    else if (_graphic.Legend.GetType() == typeof(LabelBreak))
                    {
                        LabelBreak aLB = (LabelBreak)_graphic.Legend;
                        Rectangle rect = new Rectangle();
                        //Draw.DrawLabelPoint(new PointF(this.Left + (float)this.Width / 2, this.Top + (float)this.Height / 2), aLB, g, ref rect);
                        Draw.DrawLabelPoint(aPoint, aLB, g, ref rect);
                        //g.DrawString(aLB.Text, aLB.Font, new SolidBrush(aLB.Color), new PointF(0, 0));
                    }
                    break;
                case ShapeTypes.Polyline:                    
                case ShapeTypes.Polygon:
                case ShapeTypes.Rectangle:
                    List<PointD> pList = _graphic.Shape.GetPoints();
                    PointF[] points = new PointF[pList.Count];
                    for (int i = 0; i < pList.Count; i++)
                    {
                        points[i].X = (float)pList[i].X;
                        points[i].Y = (float)pList[i].Y;
                    }

                    if (_graphic.Shape.ShapeType == ShapeTypes.Polyline)
                        Draw.DrawPolyline(points, (PolyLineBreak)_graphic.Legend, g);
                    else
                        Draw.DrawPolygon(points, (PolygonBreak)_graphic.Legend, g);

                    break;
            }              
        }

        /// <summary>
        /// Override PaintOnLayout method
        /// </summary>
        /// <param name="g">graphics</param>
        /// <param name="pageLocation">page location</param>
        /// <param name="zoom">zoom</param>
        public override void PaintOnLayout(Graphics g, PointF pageLocation, float zoom)
        {
            PointF aP = PageToScreen(this.Left, this.Top, pageLocation, zoom);
            Rectangle rect = new Rectangle((int)aP.X, (int)aP.Y, (int)(Width * zoom), (int)(Height * zoom));
            g.FillRectangle(new SolidBrush(this.BackColor), rect);

            //Draw text
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.SmoothingMode = _smoothingMode;
            PaintGraphics(g, pageLocation, zoom);
        }

        /// <summary>
        /// Paint graphics
        /// </summary>
        /// <param name="g">graphics</param>
        /// <param name="pageLocation">page location</param>
        /// <param name="zoom">zoom</param>
        public void PaintGraphics(Graphics g, PointF pageLocation, float zoom)
        {
            switch (_graphic.Shape.ShapeType)
            {
                case ShapeTypes.Point:
                    PointD dPoint = _graphic.Shape.GetPoints()[0];
                    PointF aPoint = PageToScreen((float)dPoint.X, (float)dPoint.Y, pageLocation, zoom);                    
                    if (_graphic.Legend.GetType() == typeof(PointBreak))
                    {
                        PointBreak aPB = (PointBreak)((PointBreak)_graphic.Legend).Clone();
                        float size = aPB.Size;
                        aPB.Size = aPB.Size * zoom;
                        //PointF aPoint = new PointF(this.Left + (float)this.Width / 2, this.Top + (float)this.Height / 2);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        Draw.DrawPoint(aPoint, aPB, g);
                        aPB.Size = size;
                        g.SmoothingMode = _smoothingMode;
                    }
                    else if (_graphic.Legend.GetType() == typeof(LabelBreak))
                    {
                        LabelBreak aLB = (LabelBreak)((LabelBreak)_graphic.Legend).Clone();
                        Font font = (Font)aLB.Font.Clone();                        
                        aLB.Font = new Font(font.Name, font.Size * zoom, font.Style);
                        Rectangle rect = new Rectangle();
                        //Draw.DrawLabelPoint(new PointF(this.Left + (float)this.Width / 2, this.Top + (float)this.Height / 2), aLB, g, ref rect);
                        Draw.DrawLabelPoint(aPoint, aLB, g, ref rect);
                        aLB.Font = font;
                        //g.DrawString(aLB.Text, aLB.Font, new SolidBrush(aLB.Color), new PointF(0, 0));
                    }
                    break;
                case ShapeTypes.WindArraw:
                    dPoint = _graphic.Shape.GetPoints()[0];
                    aPoint = PageToScreen((float)dPoint.X, (float)dPoint.Y, pageLocation, zoom);
                    WindArraw aArraw = (WindArraw)_graphic.Shape;
                    VectorBreak aVB = (VectorBreak)_graphic.Legend;
                    Draw.DrawArraw(aVB.Color, aPoint, aArraw, g, aVB.Zoom * zoom);
                    Font drawFont = new Font("Arial", 8 * zoom);
                    g.DrawString(aArraw.length.ToString(), drawFont, new SolidBrush(aVB.Color), aPoint.X, aPoint.Y);
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.Polygon:
                case ShapeTypes.Rectangle:
                case ShapeTypes.Circle:
                case ShapeTypes.CurveLine:
                case ShapeTypes.CurvePolygon:
                case ShapeTypes.Ellipse:
                    List<PointD> pList = _graphic.Shape.GetPoints();
                    PointF[] points = new PointF[pList.Count];
                    for (int i = 0; i < pList.Count; i++)
                    {                        
                        points[i] = PageToScreen((float)pList[i].X, (float)pList[i].Y, pageLocation, zoom);
                    }

                    switch (_graphic.Shape.ShapeType)
                    {
                        case ShapeTypes.Polyline:
                            PolyLineBreak aPLB = (PolyLineBreak)((PolyLineBreak)_graphic.Legend).Clone();
                            float size = aPLB.Size;
                            aPLB.Size = size * zoom;
                            Draw.DrawPolyline(points, (PolyLineBreak)_graphic.Legend, g);
                            aPLB.Size = size;
                            break;
                        case ShapeTypes.Polygon:
                        case ShapeTypes.Rectangle:
                            PolygonBreak aPGB = (PolygonBreak)((PolygonBreak)_graphic.Legend).Clone();
                            size = aPGB.OutlineSize;
                            aPGB.OutlineSize = size * zoom;
                            Draw.DrawPolygon(points, (PolygonBreak)_graphic.Legend, g);
                            aPGB.OutlineSize = size;
                            break;
                        case ShapeTypes.Circle:
                            aPGB = (PolygonBreak)((PolygonBreak)_graphic.Legend).Clone();
                            size = aPGB.OutlineSize;
                            aPGB.OutlineSize = size * zoom;
                            Draw.DrawCircle(points, (PolygonBreak)_graphic.Legend, g);
                            aPGB.OutlineSize = size;
                            break;
                        case ShapeTypes.CurveLine:
                            aPLB = (PolyLineBreak)((PolyLineBreak)_graphic.Legend).Clone();
                            size = aPLB.Size;
                            aPLB.Size = size * zoom;
                            Draw.DrawCurveLine(points, (PolyLineBreak)_graphic.Legend, g);
                            aPLB.Size = size;
                            break;
                        case ShapeTypes.CurvePolygon:
                            aPGB = (PolygonBreak)((PolygonBreak)_graphic.Legend).Clone();
                            size = aPGB.OutlineSize;
                            aPGB.OutlineSize = size * zoom;
                            Draw.DrawCurvePolygon(points, (PolygonBreak)_graphic.Legend, g);
                            aPGB.OutlineSize = size;
                            break;
                        case ShapeTypes.Ellipse:
                            aPGB = (PolygonBreak)((PolygonBreak)_graphic.Legend).Clone();
                            size = aPGB.OutlineSize;
                            aPGB.OutlineSize = size * zoom;
                            Draw.DrawEllipse(points, (PolygonBreak)_graphic.Legend, g);
                            aPGB.OutlineSize = size;
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Override OnLocationChanged event
        /// </summary>
        protected override void OnLocationChanged()
        {
            base.OnLocationChanged();

            if (!_updatingSize)
                MoveUpdate();

            //if (_graphic.Shape != null)
            //{
            //    switch (_graphic.Shape.ShapeType)
            //    {
            //        case ShapeTypes.Polyline:
            //        case ShapeTypes.Polygon:
            //        case ShapeTypes.Rectangle:
            //            List<PointD> pList = _graphic.Shape.GetPoints();
            //            for (int i = 0; i < pList.Count; i++)
            //            {
            //                pList
            //            }
            //            break;
            //    }
            //}
        }

        /// <summary>
        /// Override OnSizeChaged
        /// </summary>
        protected override void OnSizeChanged()
        {
            //base.OnSizeChanged();

            //if (_graphic.Shape != null)
            //{
            //    switch (_graphic.Shape.ShapeType)
            //    {
            //        case ShapeTypes.Point:
            //            if (_graphic.Legend.GetType() == typeof(PointBreak))
            //            {
            //                PointBreak aPB = (PointBreak)_graphic.Legend;
            //                aPB.Size = Width;
            //            }
            //            break;
            //        case ShapeTypes.Polyline:
            //        case ShapeTypes.Polygon:

            //            break;
            //    }
            //}
        }

        void LabelSizeChange(object sender, EventArgs e)
        {
            UpdateControlSize();
        }

        /// <summary>
        /// Override get property object methods
        /// </summary>
        /// <returns>property object</returns>
        public override object GetPropertyObject()
        {
            return _graphic.Legend.GetPropertyObject();
        }

        private void LayoutMap_MapViewUpdated(object sender, EventArgs e)
        {
            if (_graphic.Legend.GetType() == typeof(VectorBreak))
            {
                for (int i = 0; i < _layoutMap.MapFrame.MapView.LayerSet.Layers.Count; i++)
                {
                    MapLayer aLayer = _layoutMap.MapFrame.MapView.LayerSet.Layers[_layoutMap.MapFrame.MapView.LayerSet.Layers.Count - 1 - i];
                    if (aLayer.LayerType == LayerTypes.VectorLayer)
                    {
                        if (aLayer.Visible && aLayer.LayerDrawType == LayerDrawType.Vector)
                        {
                            this.Visible = true;
                            float zoom = ((VectorLayer)aLayer).DrawingZoom;
                            ((VectorBreak)_graphic.Legend).Zoom = zoom;
                            float max = 30.0f / zoom;
                            WindArraw aWA = (WindArraw)_graphic.Shape;
                            int llen = 5;
                            for (i = 10; i <= 100; i += 5)
                            {
                                if (max < i)
                                {
                                    llen = i - 5;
                                    break;
                                }
                            }
                            aWA.length = llen;
                            aWA.size = 5;
                            aWA.Value = 0;
                            UpdateControlSize();
                            //if (_defaultLegend.Visible)
                            //    _defaultWindArraw.Top = _defaultLegend.Bottom + 10;
                            //else
                            //    _defaultWindArraw.Top = _defaultLayoutMap.Bottom;
                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
