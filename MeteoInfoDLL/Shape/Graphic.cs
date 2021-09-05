using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using MeteoInfoC.Legend;
using MeteoInfoC.Drawing;
using MeteoInfoC.Layout;
using MeteoInfoC.Global;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Graphic
    /// </summary>
    public class Graphic
    {
        #region Variables
        private Shape _shape = null;
        private ColorBreak _legend = null;
        private ResizeAbility _resizeAbility = ResizeAbility.ResizeAll;
        private string _tag = "";

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Graphic()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shape">shape</param>
        /// <param name="legend">legend</param>
        public Graphic(Shape shape, ColorBreak legend)
        {
            _shape = shape;
            _legend = legend;
            UpdateResizeAbility();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set shape
        /// </summary>
        public Shape Shape
        {
            get { return _shape; }
            set 
            {
                _shape = value;
                UpdateResizeAbility();
            }
        }

        /// <summary>
        /// Get or set legend
        /// </summary>
        public ColorBreak Legend
        {
            get { return _legend; }
            set 
            { 
                _legend = value;
                UpdateResizeAbility();
            }
        }

        /// <summary>
        /// Get resize ability
        /// </summary>
        public ResizeAbility ResizeAbility
        {
            get { return _resizeAbility; }
        }

        /// <summary>
        /// Get or set tag
        /// </summary>
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        #endregion

        #region Methods
        private void UpdateResizeAbility()
        {
            if (_shape != null && _legend != null)
            {
                switch (_shape.ShapeType)
                {
                    case ShapeTypes.Point:
                        switch (_legend.BreakType)
                        {
                            case BreakTypes.PointBreak:
                                _resizeAbility = ResizeAbility.SameWidthHeight;
                                break;
                            case BreakTypes.LabelBreak:
                            case BreakTypes.ChartBreak:
                                _resizeAbility = ResizeAbility.None;
                                break;
                        }  
                        break;
                    case ShapeTypes.Circle:
                        _resizeAbility = ResizeAbility.SameWidthHeight;
                        break;
                    default:
                        _resizeAbility = ResizeAbility.ResizeAll;
                        break;
                }                
            }
        }

        /// <summary>
        /// Vertice edited update
        /// </summary>
        /// <param name="vIdx">vertice index</param>
        /// <param name="newX">new X</param>
        /// <param name="newY">new Y</param>
        public void VerticeEditUpdate(int vIdx, double newX, double newY)
        {
            List<PointD> points = _shape.GetPoints();
            PointD aP = points[vIdx];
            aP.X = newX;
            aP.Y = newY;
            points[vIdx] = aP;
            _shape.SetPoints(points);            
        }

        /// <summary>
        /// Export to XML document
        /// </summary>
        /// <param name="doc">xml document</param>
        /// <param name="parent">parent xml element</param>
        public void ExportToXML(ref XmlDocument doc, XmlElement parent)
        {
            XmlElement graphic = doc.CreateElement("Graphic");

            XmlAttribute tagAttr = doc.CreateAttribute("Tag");
            tagAttr.InnerText = this._tag;
            graphic.Attributes.Append(tagAttr);

            AddShape(ref doc, graphic, _shape);
            AddLegend(ref doc, graphic, _legend, _shape.ShapeType);

            parent.AppendChild(graphic);
        }

        private void AddShape(ref XmlDocument doc, XmlElement parent, Shape aShape)
        {
            XmlElement shape = doc.CreateElement("Shape");

            //Add general attribute
            XmlAttribute shapeType = doc.CreateAttribute("ShapeType");
            XmlAttribute visible = doc.CreateAttribute("Visible");
            XmlAttribute selected = doc.CreateAttribute("Selected");

            //shapeType.InnerText = Enum.GetName(typeof(ShapeTypes), aShape.ShapeType);
            shapeType.InnerText = aShape.ShapeType.ToString();
            visible.InnerText = aShape.Visible.ToString();
            selected.InnerText = aShape.Selected.ToString();

            shape.Attributes.Append(shapeType);
            shape.Attributes.Append(visible);
            shape.Attributes.Append(selected);

            //Add points
            XmlElement points = doc.CreateElement("Points");
            List<PointD> pointList = aShape.GetPoints();
            foreach (PointD aPoint in pointList)
            {
                XmlElement point = doc.CreateElement("Point");
                XmlAttribute x = doc.CreateAttribute("X");
                XmlAttribute y = doc.CreateAttribute("Y");
                x.InnerText = aPoint.X.ToString();
                y.InnerText = aPoint.Y.ToString();
                point.Attributes.Append(x);
                point.Attributes.Append(y);

                points.AppendChild(point);
            }

            shape.AppendChild(points);

            parent.AppendChild(shape);
        }

        private void AddLegend(ref XmlDocument doc, XmlElement parent, ColorBreak aLegend, ShapeTypes shapeType)
        {
            XmlElement legend = doc.CreateElement("Legend");
            XmlAttribute color = doc.CreateAttribute("Color");
            color.InnerText = ColorTranslator.ToHtml(aLegend.Color);
            legend.Attributes.Append(color);

            XmlAttribute legendType = doc.CreateAttribute("LegendType");
            XmlAttribute size;
            XmlAttribute style;
            XmlAttribute outlineColor;
            XmlAttribute drawOutline;
            XmlAttribute drawFill;
            legendType.InnerText = aLegend.BreakType.ToString();
            switch (aLegend.BreakType)
            {
                case BreakTypes.PointBreak:
                    PointBreak aPB = (PointBreak)aLegend;
                    outlineColor = doc.CreateAttribute("OutlineColor");
                    size = doc.CreateAttribute("Size");
                    style = doc.CreateAttribute("Style");
                    drawOutline = doc.CreateAttribute("DrawOutline");
                    drawFill = doc.CreateAttribute("DrawFill");
                    XmlAttribute markerType = doc.CreateAttribute("MarkerType");
                    XmlAttribute fontName = doc.CreateAttribute("FontName");
                    XmlAttribute charIndex = doc.CreateAttribute("CharIndex");
                    XmlAttribute imagePath = doc.CreateAttribute("ImagePath");
                    XmlAttribute angle = doc.CreateAttribute("Angle");

                    //legendType.InnerText = "PointBreak";
                    outlineColor.InnerText = ColorTranslator.ToHtml(aPB.OutlineColor);
                    size.InnerText = aPB.Size.ToString();
                    style.InnerText = Enum.GetName(typeof(PointStyle), aPB.Style);
                    drawOutline.InnerText = aPB.DrawOutline.ToString();
                    drawFill.InnerText = aPB.DrawFill.ToString();
                    markerType.InnerText = aPB.MarkerType.ToString();
                    fontName.InnerText = aPB.FontName;
                    charIndex.InnerText = aPB.CharIndex.ToString();
                    imagePath.InnerText = aPB.ImagePath;
                    angle.InnerText = aPB.Angle.ToString();

                    legend.Attributes.Append(legendType);
                    legend.Attributes.Append(outlineColor);
                    legend.Attributes.Append(size);
                    legend.Attributes.Append(style);
                    legend.Attributes.Append(drawOutline);
                    legend.Attributes.Append(drawFill);
                    legend.Attributes.Append(markerType);
                    legend.Attributes.Append(fontName);
                    legend.Attributes.Append(charIndex);
                    legend.Attributes.Append(imagePath);
                    legend.Attributes.Append(angle);
                    break;
                case BreakTypes.LabelBreak:
                    LabelBreak aLB = (LabelBreak)aLegend;
                    XmlAttribute text = doc.CreateAttribute("Text");
                    angle = doc.CreateAttribute("Angle");
                    fontName = doc.CreateAttribute("FontName");
                    XmlAttribute fontSize = doc.CreateAttribute("FontSize");
                    XmlAttribute fontBold = doc.CreateAttribute("FontBold");
                    XmlAttribute yShift = doc.CreateAttribute("YShift");

                    //legendType.InnerText = "LabelBreak";
                    text.InnerText = aLB.Text;
                    angle.InnerText = aLB.Angle.ToString();
                    fontName.InnerText = aLB.Font.Name;
                    fontSize.InnerText = aLB.Font.Size.ToString();
                    fontBold.InnerText = aLB.Font.Bold.ToString();
                    yShift.InnerText = aLB.YShift.ToString();

                    legend.Attributes.Append(legendType);
                    legend.Attributes.Append(text);
                    legend.Attributes.Append(angle);
                    legend.Attributes.Append(fontName);
                    legend.Attributes.Append(fontSize);
                    legend.Attributes.Append(fontBold);
                    legend.Attributes.Append(yShift);
                    break;
                case BreakTypes.ChartBreak:
                    ChartBreak aChB = (ChartBreak)aLegend;
                    XmlAttribute shapeIndex = doc.CreateAttribute("ShapeIndex");
                    XmlAttribute chartType = doc.CreateAttribute("ChartType");
                    XmlAttribute chartData = doc.CreateAttribute("ChartData");
                    XmlAttribute xShift = doc.CreateAttribute("XShift");
                    yShift = doc.CreateAttribute("YShift");

                    shapeIndex.InnerText = aChB.ShapeIndex.ToString();
                    //legendType.InnerText = "ChartBreak";
                    chartType.InnerText = aChB.ChartType.ToString();
                    string cdata = "";
                    for (int i = 0; i < aChB.ItemNum; i++)
                    {
                        if (i == 0)
                            cdata = aChB.ChartData[i].ToString();
                        else
                            cdata += "," + aChB.ChartData[i].ToString();
                    }
                    chartData.InnerText = cdata;
                    xShift.InnerText = aChB.XShift.ToString();
                    yShift.InnerText = aChB.YShift.ToString();

                    legend.Attributes.Append(legendType);
                    legend.Attributes.Append(shapeIndex);
                    legend.Attributes.Append(chartType);
                    legend.Attributes.Append(chartData);
                    legend.Attributes.Append(xShift);
                    legend.Attributes.Append(yShift);
                    break;
                case BreakTypes.VectorBreak:
                    //legendType.InnerText = "VectorBreak";
                    legend.Attributes.Append(legendType);
                    break;
                case BreakTypes.PolylineBreak:
                    PolyLineBreak aPLB = (PolyLineBreak)aLegend;
                    size = doc.CreateAttribute("Size");
                    style = doc.CreateAttribute("Style");
                    XmlAttribute drawSymbol = doc.CreateAttribute("DrawSymbol");
                    XmlAttribute symbolSize = doc.CreateAttribute("SymbolSize");
                    XmlAttribute symbolStyle = doc.CreateAttribute("SymbolStyle");
                    XmlAttribute symbolColor = doc.CreateAttribute("SymbolColor");
                    XmlAttribute symbolInterval = doc.CreateAttribute("SymbolInterval");

                    //legendType.InnerText = "PolylineBreak";
                    size.InnerText = aPLB.Size.ToString();
                    style.InnerText = Enum.GetName(typeof(LineStyles), aPLB.Style);
                    drawSymbol.InnerText = aPLB.DrawSymbol.ToString();
                    symbolSize.InnerText = aPLB.SymbolSize.ToString();
                    symbolStyle.InnerText = aPLB.SymbolStyle.ToString();
                    symbolColor.InnerText = ColorTranslator.ToHtml(aPLB.SymbolColor);
                    symbolInterval.InnerText = aPLB.SymbolInterval.ToString();

                    legend.Attributes.Append(legendType);
                    legend.Attributes.Append(size);
                    legend.Attributes.Append(style);
                    legend.Attributes.Append(drawSymbol);
                    legend.Attributes.Append(symbolSize);
                    legend.Attributes.Append(symbolStyle);
                    legend.Attributes.Append(symbolColor);
                    legend.Attributes.Append(symbolInterval);
                    break;
                case BreakTypes.PolygonBreak:
                    PolygonBreak aPGB = (PolygonBreak)aLegend;
                    outlineColor = doc.CreateAttribute("OutlineColor");
                    drawOutline = doc.CreateAttribute("DrawOutline");
                    drawFill = doc.CreateAttribute("DrawFill");
                    XmlAttribute outlineSize = doc.CreateAttribute("OutlineSize");
                    XmlAttribute usingHatchStyle = doc.CreateAttribute("UsingHatchStyle");
                    style = doc.CreateAttribute("Style");
                    XmlAttribute backColor = doc.CreateAttribute("BackColor");
                    //XmlAttribute transparencyPer = doc.CreateAttribute("TransparencyPercent");
                    XmlAttribute isMaskout = doc.CreateAttribute("IsMaskout");

                    //legendType.InnerText = "PolygonBreak";
                    outlineColor.InnerText = ColorTranslator.ToHtml(aPGB.OutlineColor);
                    drawOutline.InnerText = aPGB.DrawOutline.ToString();
                    drawFill.InnerText = aPGB.DrawFill.ToString();
                    outlineSize.InnerText = aPGB.OutlineSize.ToString();
                    usingHatchStyle.InnerText = aPGB.UsingHatchStyle.ToString();
                    style.InnerText = aPGB.Style.ToString();
                    backColor.InnerText = ColorTranslator.ToHtml(aPGB.BackColor);
                    //transparencyPer.InnerText = aPGB.TransparencyPercent.ToString();
                    isMaskout.InnerText = aPGB.IsMaskout.ToString();

                    legend.Attributes.Append(legendType);
                    legend.Attributes.Append(outlineColor);
                    legend.Attributes.Append(drawOutline);
                    legend.Attributes.Append(drawFill);
                    legend.Attributes.Append(outlineSize);
                    legend.Attributes.Append(usingHatchStyle);
                    legend.Attributes.Append(style);
                    legend.Attributes.Append(backColor);
                    //legend.Attributes.Append(transparencyPer);
                    legend.Attributes.Append(isMaskout);
                    break;
            }  

            parent.AppendChild(legend);
        }

        /// <summary>
        /// Import from XML node
        /// </summary>
        /// <param name="graphicNode">graphic xml node</param>
        public void ImportFromXML(XmlNode graphicNode)
        {
            XmlNode shape = graphicNode.ChildNodes[0];
            _shape = LoadShape(shape);

            XmlNode legend = graphicNode.ChildNodes[1];
            _legend = LoadLegend(legend, _shape.ShapeType);

            UpdateResizeAbility();

            try
            {
                this._tag = graphicNode.Attributes["Tag"].InnerText;
            }
            catch
            {

            }

        }

        private Shape LoadShape(XmlNode shapeNode)
        {
            Shape aShape = new Shape();
            try
            {
                ShapeTypes shapeType = (ShapeTypes)Enum.Parse(typeof(ShapeTypes), shapeNode.Attributes["ShapeType"].InnerText, true);
                switch (shapeType)
                {
                    case ShapeTypes.Point:
                        aShape = new PointShape();
                        break;
                    case ShapeTypes.WindArraw:
                        aShape = new WindArraw();
                        break;
                    case ShapeTypes.Polyline:
                        aShape = new PolylineShape();
                        break;
                    case ShapeTypes.CurveLine:
                        aShape = new CurveLineShape();
                        break;
                    case ShapeTypes.Circle:
                        aShape = new CircleShape();
                        break;
                    case ShapeTypes.Polygon:
                    case ShapeTypes.Rectangle:
                        aShape = new PolygonShape();
                        break;
                    case ShapeTypes.CurvePolygon:
                        aShape = new CurvePolygonShape();
                        break;
                    case ShapeTypes.Ellipse:
                        aShape = new EllipseShape();
                        break;
                }

                aShape.Visible = bool.Parse(shapeNode.Attributes["Visible"].InnerText);
                aShape.Selected = bool.Parse(shapeNode.Attributes["Selected"].InnerText);

                List<PointD> pointList = new List<PointD>();
                XmlNode pointsNode = shapeNode.ChildNodes[0];
                foreach (XmlNode pNode in pointsNode.ChildNodes)
                {
                    PointD aPoint = new PointD(double.Parse(pNode.Attributes["X"].InnerText),
                        double.Parse(pNode.Attributes["Y"].InnerText));
                    pointList.Add(aPoint);
                }
                aShape.SetPoints(pointList);
            }
            catch
            {

            }

            return aShape;
        }

        private ColorBreak LoadLegend(XmlNode legendNode, ShapeTypes shapeType)
        {
            ColorBreak legend = new ColorBreak();
            try
            {
                Color color = ColorTranslator.FromHtml(legendNode.Attributes["Color"].InnerText);
                string legendType = legendNode.Attributes["LegendType"].InnerText;
                BreakTypes breakType = (BreakTypes)Enum.Parse(typeof(BreakTypes), legendType);
                switch (breakType)
                {
                    case BreakTypes.PointBreak:
                        PointBreak aPB = new PointBreak();
                        try
                        {
                            aPB.Color = color;
                            aPB.DrawFill = bool.Parse(legendNode.Attributes["DrawFill"].InnerText);
                            aPB.DrawOutline = bool.Parse(legendNode.Attributes["DrawOutline"].InnerText);
                            aPB.OutlineColor = ColorTranslator.FromHtml(legendNode.Attributes["OutlineColor"].InnerText);
                            aPB.Size = float.Parse(legendNode.Attributes["Size"].InnerText);
                            aPB.Style = (PointStyle)Enum.Parse(typeof(PointStyle), legendNode.Attributes["Style"].InnerText);
                            aPB.MarkerType = (MarkerType)Enum.Parse(typeof(MarkerType), legendNode.Attributes["MarkerType"].InnerText, true);
                            aPB.FontName = legendNode.Attributes["FontName"].InnerText;
                            aPB.CharIndex = int.Parse(legendNode.Attributes["CharIndex"].InnerText);
                            aPB.ImagePath = legendNode.Attributes["ImagePath"].InnerText;
                            aPB.Angle = float.Parse(legendNode.Attributes["Angle"].InnerText);
                        }
                        catch { }
                        finally
                        {
                            legend = aPB;
                        }
                        break;
                    case BreakTypes.LabelBreak:
                        LabelBreak aLB = new LabelBreak();
                        try
                        {
                            aLB.Color = color;
                            aLB.Angle = float.Parse(legendNode.Attributes["Angle"].InnerText);
                            aLB.Text = legendNode.Attributes["Text"].InnerText;
                            string fontName = legendNode.Attributes["FontName"].InnerText;
                            float fontSize = float.Parse(legendNode.Attributes["FontSize"].InnerText);
                            bool fontBold = bool.Parse(legendNode.Attributes["FontBold"].InnerText);
                            if (fontBold)
                                aLB.Font = new Font(fontName, fontSize, FontStyle.Bold);
                            else
                                aLB.Font = new Font(fontName, fontSize);

                            aLB.YShift = float.Parse(legendNode.Attributes["YShift"].InnerText);
                        }
                        catch { }
                        finally
                        {
                            legend = aLB;
                        }
                        break;
                    case BreakTypes.ChartBreak:
                        ChartBreak aChB = new ChartBreak(ChartTypes.BarChart);
                        try
                        {
                            ChartTypes chartType = (ChartTypes)Enum.Parse(typeof(ChartTypes), legendNode.Attributes["ChartType"].InnerText);
                            aChB = new ChartBreak(chartType);
                            aChB.ShapeIndex = int.Parse(legendNode.Attributes["ShapeIndex"].InnerText);
                            List<float> cData = new List<float>();
                            string[] cDataStr = legendNode.Attributes["ChartData"].InnerText.Split(',');
                            for (int i = 0; i < cDataStr.Length; i++)
                                cData.Add(float.Parse(cDataStr[i]));

                            aChB.ChartData = cData;
                            aChB.XShift = int.Parse(legendNode.Attributes["XShift"].InnerText);
                            aChB.YShift = int.Parse(legendNode.Attributes["YShift"].InnerText);
                        }
                        catch { }
                        finally
                        {
                            legend = aChB;
                        }
                        break;
                    case BreakTypes.VectorBreak:
                        VectorBreak aVB = new VectorBreak();
                        try
                        {
                            aVB.Color = color;
                        }
                        catch { }
                        finally
                        {
                            legend = aVB;
                        }
                        break;
                    case BreakTypes.PolylineBreak:
                        PolyLineBreak aPLB = new PolyLineBreak();
                        try
                        {
                            aPLB.Color = color;
                            aPLB.Size = Convert.ToSingle(legendNode.Attributes["Size"].InnerText);
                            aPLB.Style = (LineStyles)Enum.Parse(typeof(LineStyles),
                                legendNode.Attributes["Style"].InnerText, true);
                            aPLB.DrawSymbol = bool.Parse(legendNode.Attributes["DrawSymbol"].InnerText);
                            aPLB.SymbolSize = Single.Parse(legendNode.Attributes["SymbolSize"].InnerText);
                            aPLB.SymbolStyle = (PointStyle)Enum.Parse(typeof(PointStyle),
                                legendNode.Attributes["SymbolStyle"].InnerText, true);
                            aPLB.SymbolColor = ColorTranslator.FromHtml(legendNode.Attributes["SymbolColor"].InnerText);
                            aPLB.SymbolInterval = int.Parse(legendNode.Attributes["SymbolInterval"].InnerText);
                        }
                        catch { }
                        finally
                        {
                            legend = aPLB;
                        }
                        break;
                    case BreakTypes.PolygonBreak:
                        PolygonBreak aPGB = new PolygonBreak();
                        try
                        {
                            aPGB.Color = color;
                            aPGB.DrawFill = Convert.ToBoolean(legendNode.Attributes["DrawFill"].InnerText);
                            aPGB.DrawOutline = Convert.ToBoolean(legendNode.Attributes["DrawOutline"].InnerText);
                            aPGB.OutlineSize = Convert.ToSingle(legendNode.Attributes["OutlineSize"].InnerText);
                            aPGB.OutlineColor = ColorTranslator.FromHtml(legendNode.Attributes["OutlineColor"].InnerText);
                            aPGB.UsingHatchStyle = bool.Parse(legendNode.Attributes["UsingHatchStyle"].InnerText);
                            aPGB.Style = (HatchStyle)Enum.Parse(typeof(HatchStyle), legendNode.Attributes["Style"].InnerText, true);
                            aPGB.BackColor = ColorTranslator.FromHtml(legendNode.Attributes["BackColor"].InnerText);
                            //aPGB.TransparencyPercent = int.Parse(legendNode.Attributes["TransparencyPercent"].InnerText);
                            aPGB.IsMaskout = bool.Parse(legendNode.Attributes["IsMaskout"].InnerText);
                        }
                        catch { }
                        {
                            legend = aPGB;
                        }
                        break;
                }
            }
            catch { }
            return legend;
        }

        #endregion
    }
}
