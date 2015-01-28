using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Xml;
using System.IO;
using MeteoInfoC.Shape;
using MeteoInfoC.Drawing;
using MeteoInfoC.Data.MeteoData;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Legend scheme
    /// </summary>
    public class LegendScheme
    {
        #region Variables
        /// <summary>
        /// Field name
        /// </summary>
        public string FieldName;
        /// <summary>
        /// Legend type
        /// </summary>
        public LegendType LegendType;
        /// <summary>
        /// Shape type
        /// </summary>
        public ShapeTypes ShapeType;        
        ///// <summary>
        ///// Break number
        ///// </summary>
        //public int breakNum;
        /// <summary>
        /// Break list
        /// </summary>
        public List<ColorBreak> LegendBreaks;
        /// <summary>
        /// If including undefine data
        /// </summary>
        public Boolean HasNoData;
        /// <summary>
        /// Minimum value
        /// </summary>
        public double MinValue;
        /// <summary>
        /// Maximum value
        /// </summary>
        public double MaxValue;
        /// <summary>
        /// Undefine data
        /// </summary>
        public double MissingValue;

        #endregion     
   
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aShapeType">shape type</param>
        public LegendScheme(ShapeTypes aShapeType)
        {
            ShapeType = aShapeType;
            LegendBreaks = new List<ColorBreak>();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get break number
        /// </summary>
        public int BreakNum
        {
            get { return LegendBreaks.Count; }
        }

        /// <summary>
        /// Get visible break number
        /// </summary>
        public int VisibleBreakNum
        {
            get
            {
                int n = 0;
                foreach (ColorBreak aCB in LegendBreaks)
                {
                    if (aCB.DrawShape)
                        n += 1;
                }

                return n;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get color list
        /// </summary>
        /// <returns>color list</returns>
        public List<Color> GetColors()
        {
            List<Color> colors = new List<Color>();
            for (int i = 0; i < LegendBreaks.Count; i++)
            {
                colors.Add(LegendBreaks[i].Color);
            }

            return colors;
        }

        /// <summary>
        /// Get values
        /// </summary>
        /// <returns>Values</returns>
        public List<double> GetValues()
        {
            List<double> values = new List<double>();
            foreach (ColorBreak cb in LegendBreaks)
                values.Add(double.Parse(cb.StartValue.ToString()));

            return values;
        }

        /// <summary>
        /// Judge if shape type is consistent with draw type
        /// </summary>
        /// <param name="drawTyp">draw type</param>
        /// <returns>if consistent</returns>
        public bool IsConsistent(DrawType2D drawTyp)
        {
            switch (ShapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes.PointZ:
                case ShapeTypes.StationModel:
                case ShapeTypes.WeatherSymbol:
                case ShapeTypes.WindArraw:
                case ShapeTypes.WindBarb:
                    switch (drawTyp)
                    {
                        case DrawType2D.Grid_Point:
                        case DrawType2D.Station_Info:
                        case DrawType2D.Station_Model:
                        case DrawType2D.Station_Point:
                        case DrawType2D.Traj_Point:
                        case DrawType2D.Traj_StartPoint:
                        case DrawType2D.Weather_Symbol:
                        case DrawType2D.Barb:
                            return true;
                        default:
                            return false;
                    }
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineZ:
                    switch (drawTyp)
                    {
                        case DrawType2D.Contour:
                        case DrawType2D.Streamline:
                        case DrawType2D.Traj_Line:
                            return true;
                        default:
                            return false;
                    }
                case ShapeTypes.Polygon:
                    switch (drawTyp)
                    {
                        case DrawType2D.Shaded:
                            return true;
                        default:
                            return false;
                    }
                case ShapeTypes.Image:
                    switch (drawTyp)
                    {
                        case DrawType2D.Raster:
                            return true;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }

        /// <summary>
        /// Export to XML document
        /// </summary>
        /// <param name="doc">xml document</param>
        /// <param name="parent">parent xml element</param>
        public void ExportToXML(ref XmlDocument doc, XmlElement parent)
        {            
            XmlElement root = doc.CreateElement("LegendScheme");
            XmlAttribute FieldName = doc.CreateAttribute("FieldName");
            XmlAttribute LegendType = doc.CreateAttribute("LegendType");
            XmlAttribute ShapeType = doc.CreateAttribute("ShapeType");
            XmlAttribute breakNum = doc.CreateAttribute("BreakNum");
            XmlAttribute HasNoData = doc.CreateAttribute("HasNoData");
            XmlAttribute MinValue = doc.CreateAttribute("MinValue");
            XmlAttribute MaxValue = doc.CreateAttribute("MaxValue");
            XmlAttribute MissingValue = doc.CreateAttribute("UNDEF");

            FieldName.InnerText = this.FieldName;
            LegendType.InnerText = Enum.GetName(typeof(LegendType), this.LegendType);
            ShapeType.InnerText = Enum.GetName(typeof(ShapeTypes), this.ShapeType);
            breakNum.InnerText = this.BreakNum.ToString();
            HasNoData.InnerText = this.HasNoData.ToString();
            MinValue.InnerText = this.MinValue.ToString();
            MaxValue.InnerText = this.MaxValue.ToString();
            MissingValue.InnerText = this.MissingValue.ToString();

            root.Attributes.Append(FieldName);
            root.Attributes.Append(LegendType);
            root.Attributes.Append(ShapeType);
            root.Attributes.Append(breakNum);
            root.Attributes.Append(HasNoData);
            root.Attributes.Append(MinValue);
            root.Attributes.Append(MaxValue);
            root.Attributes.Append(MissingValue);

            XmlElement breaks = doc.CreateElement("Breaks");
            XmlElement brk;
            XmlAttribute caption;
            XmlAttribute startValue;
            XmlAttribute endValue;
            XmlAttribute color;
            XmlAttribute drawShape;
            XmlAttribute size;
            XmlAttribute style;
            XmlAttribute outlineColor;
            XmlAttribute drawOutline;
            XmlAttribute drawFill;
            switch (this.ShapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes.PointZ:
                    XmlAttribute isNoData;
                    foreach (PointBreak aPB in this.LegendBreaks)
                    {
                        brk = doc.CreateElement("Break");
                        caption = doc.CreateAttribute("Caption");
                        startValue = doc.CreateAttribute("StartValue");
                        endValue = doc.CreateAttribute("EndValue");
                        color = doc.CreateAttribute("Color");
                        drawShape = doc.CreateAttribute("DrawShape");
                        outlineColor = doc.CreateAttribute("OutlineColor");
                        size = doc.CreateAttribute("Size");
                        style = doc.CreateAttribute("Style");
                        drawOutline = doc.CreateAttribute("DrawOutline");
                        drawFill = doc.CreateAttribute("DrawFill");
                        isNoData = doc.CreateAttribute("IsNoData");
                        XmlAttribute markerType = doc.CreateAttribute("MarkerType");
                        XmlAttribute fontName = doc.CreateAttribute("FontName");
                        XmlAttribute charIndex = doc.CreateAttribute("CharIndex");
                        XmlAttribute imagePath = doc.CreateAttribute("ImagePath");
                        XmlAttribute angle = doc.CreateAttribute("Angle");

                        caption.InnerText = aPB.Caption;
                        startValue.InnerText = aPB.StartValue.ToString();
                        endValue.InnerText = aPB.EndValue.ToString();
                        color.InnerText = ColorTranslator.ToHtml(aPB.Color);
                        drawShape.InnerText = aPB.DrawShape.ToString();
                        outlineColor.InnerText = ColorTranslator.ToHtml(aPB.OutlineColor);
                        size.InnerText = aPB.Size.ToString();
                        style.InnerText = Enum.GetName(typeof(PointStyle), aPB.Style);
                        drawOutline.InnerText = aPB.DrawOutline.ToString();
                        drawFill.InnerText = aPB.DrawFill.ToString();
                        isNoData.InnerText = aPB.IsNoData.ToString();
                        markerType.InnerText = aPB.MarkerType.ToString();
                        fontName.InnerText = aPB.FontName;
                        charIndex.InnerText = aPB.CharIndex.ToString();
                        imagePath.InnerText = aPB.ImagePath;
                        angle.InnerText = aPB.Angle.ToString();

                        brk.Attributes.Append(caption);
                        brk.Attributes.Append(startValue);
                        brk.Attributes.Append(endValue);
                        brk.Attributes.Append(color);
                        brk.Attributes.Append(drawShape);
                        brk.Attributes.Append(outlineColor);
                        brk.Attributes.Append(size);
                        brk.Attributes.Append(style);
                        brk.Attributes.Append(drawOutline);
                        brk.Attributes.Append(drawFill);
                        brk.Attributes.Append(isNoData);
                        brk.Attributes.Append(markerType);
                        brk.Attributes.Append(fontName);
                        brk.Attributes.Append(charIndex);
                        brk.Attributes.Append(imagePath);
                        brk.Attributes.Append(angle);

                        breaks.AppendChild(brk);
                    }
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineZ:
                    foreach (PolyLineBreak aPLB in this.LegendBreaks)
                    {
                        brk = doc.CreateElement("Break");
                        caption = doc.CreateAttribute("Caption");
                        startValue = doc.CreateAttribute("StartValue");
                        endValue = doc.CreateAttribute("EndValue");
                        color = doc.CreateAttribute("Color");
                        drawShape = doc.CreateAttribute("DrawShape");
                        size = doc.CreateAttribute("Size");
                        style = doc.CreateAttribute("Style");
                        XmlAttribute drawSymbol = doc.CreateAttribute("DrawSymbol");
                        XmlAttribute symbolSize = doc.CreateAttribute("SymbolSize");
                        XmlAttribute symbolStyle = doc.CreateAttribute("SymbolStyle");
                        XmlAttribute symbolColor = doc.CreateAttribute("SymbolColor");
                        XmlAttribute symbolInterval = doc.CreateAttribute("SymbolInterval");

                        caption.InnerText = aPLB.Caption;
                        startValue.InnerText = aPLB.StartValue.ToString();
                        endValue.InnerText = aPLB.EndValue.ToString();
                        color.InnerText = ColorTranslator.ToHtml(aPLB.Color);
                        drawShape.InnerText = aPLB.DrawPolyline.ToString();
                        size.InnerText = aPLB.Size.ToString();
                        style.InnerText = Enum.GetName(typeof(DashStyle), aPLB.Style);
                        drawSymbol.InnerText = aPLB.DrawSymbol.ToString();
                        symbolSize.InnerText = aPLB.SymbolSize.ToString();
                        symbolStyle.InnerText = aPLB.SymbolStyle.ToString();
                        symbolColor.InnerText = ColorTranslator.ToHtml(aPLB.SymbolColor);
                        symbolInterval.InnerText = aPLB.SymbolInterval.ToString();

                        brk.Attributes.Append(caption);
                        brk.Attributes.Append(startValue);
                        brk.Attributes.Append(endValue);
                        brk.Attributes.Append(color);
                        brk.Attributes.Append(drawShape);
                        brk.Attributes.Append(size);
                        brk.Attributes.Append(style);
                        brk.Attributes.Append(drawSymbol);
                        brk.Attributes.Append(symbolSize);
                        brk.Attributes.Append(symbolStyle);
                        brk.Attributes.Append(symbolColor);
                        brk.Attributes.Append(symbolInterval);

                        breaks.AppendChild(brk);
                    }
                    break;
                case ShapeTypes.Polygon:
                    XmlAttribute outlineSize;
                    foreach (PolygonBreak aPGB in this.LegendBreaks)
                    {
                        brk = doc.CreateElement("Break");
                        caption = doc.CreateAttribute("Caption");
                        startValue = doc.CreateAttribute("StartValue");
                        endValue = doc.CreateAttribute("EndValue");
                        color = doc.CreateAttribute("Color");
                        drawShape = doc.CreateAttribute("DrawShape");
                        outlineColor = doc.CreateAttribute("OutlineColor");
                        drawOutline = doc.CreateAttribute("DrawOutline");
                        drawFill = doc.CreateAttribute("DrawFill");
                        outlineSize = doc.CreateAttribute("OutlineSize");
                        XmlAttribute usingHatchStyle = doc.CreateAttribute("UsingHatchStyle");
                        style = doc.CreateAttribute("Style");
                        XmlAttribute backColor = doc.CreateAttribute("BackColor");

                        caption.InnerText = aPGB.Caption;
                        startValue.InnerText = aPGB.StartValue.ToString();
                        endValue.InnerText = aPGB.EndValue.ToString();
                        color.InnerText = ColorTranslator.ToHtml(aPGB.Color);
                        drawShape.InnerText = aPGB.DrawShape.ToString();
                        outlineColor.InnerText = ColorTranslator.ToHtml(aPGB.OutlineColor);
                        drawOutline.InnerText = aPGB.DrawOutline.ToString();
                        drawFill.InnerText = aPGB.DrawFill.ToString();
                        outlineSize.InnerText = aPGB.OutlineSize.ToString();
                        usingHatchStyle.InnerText = aPGB.UsingHatchStyle.ToString();
                        style.InnerText = aPGB.Style.ToString();
                        backColor.InnerText = ColorTranslator.ToHtml(aPGB.BackColor);

                        brk.Attributes.Append(caption);
                        brk.Attributes.Append(startValue);
                        brk.Attributes.Append(endValue);
                        brk.Attributes.Append(color);
                        brk.Attributes.Append(drawShape);
                        brk.Attributes.Append(outlineColor);
                        brk.Attributes.Append(drawOutline);
                        brk.Attributes.Append(drawFill);
                        brk.Attributes.Append(outlineSize);
                        brk.Attributes.Append(usingHatchStyle);
                        brk.Attributes.Append(style);
                        brk.Attributes.Append(backColor);

                        breaks.AppendChild(brk);
                    }
                    break;
                case ShapeTypes.Image:
                    foreach (ColorBreak aCB in this.LegendBreaks)
                    {
                        brk = doc.CreateElement("Break");
                        caption = doc.CreateAttribute("Caption");
                        startValue = doc.CreateAttribute("StartValue");
                        endValue = doc.CreateAttribute("EndValue");
                        color = doc.CreateAttribute("Color");
                        isNoData = doc.CreateAttribute("IsNoData");

                        caption.InnerText = aCB.Caption;
                        startValue.InnerText = aCB.StartValue.ToString();
                        endValue.InnerText = aCB.EndValue.ToString();
                        color.InnerText = ColorTranslator.ToHtml(aCB.Color);
                        isNoData.InnerText = aCB.IsNoData.ToString();

                        brk.Attributes.Append(caption);
                        brk.Attributes.Append(startValue);
                        brk.Attributes.Append(endValue);
                        brk.Attributes.Append(color);
                        brk.Attributes.Append(isNoData);

                        breaks.AppendChild(brk);
                    }
                    break;
            }

            root.AppendChild(breaks);
            parent.AppendChild(root);
        }

        /// <summary>
        /// Export to XML file
        /// </summary>
        /// <param name="aFile">file path</param>
        public void ExportToXMLFile(string aFile)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", "gb2312", null);
            doc.AppendChild(xmldecl);
            doc.LoadXml("<MeteoInfo file='" + Path.GetFileNameWithoutExtension(aFile) + "' type='LegendScheme'></MeteoInfo>");
            XmlElement root = doc.DocumentElement;

            ExportToXML(ref doc, root);
            
            doc.Save(aFile);
        }

        ///// <summary>
        ///// Import legend scheme from XML node
        ///// </summary>
        ///// <param name="LSNode">xml node</param>
        ///// <param name="aST">shape type</param>
        //public void ImportFromXML(XmlNode LSNode, ShapeTypes aST)
        //{
        //    LegendBreaks = new List<ColorBreak>();
            
        //    if (!(LSNode.Attributes["FieldName"] == null))
        //    {
        //        FieldName = LSNode.Attributes["FieldName"].InnerText;
        //    }
        //    LegendType = (LegendType)Enum.Parse(typeof(LegendType),
        //        LSNode.Attributes["LegendType"].InnerText, true);
        //    ShapeTypes aShapeType = (ShapeTypes)Enum.Parse(typeof(ShapeTypes),
        //        LSNode.Attributes["ShapeType"].InnerText, true);
        //    if (aShapeType != aST)
        //    {
        //        ShapeType = aST;
        //        breakNum = Convert.ToInt32(LSNode.Attributes["BreakNum"].InnerText);
        //        HasNoData = false;
        //        MinValue = Convert.ToDouble(LSNode.Attributes["MinValue"].InnerText);
        //        MaxValue = Convert.ToDouble(LSNode.Attributes["MaxValue"].InnerText);
        //        MissingValue = Convert.ToDouble(LSNode.Attributes["MissingValue"].InnerText);

        //        XmlNode breaks = LSNode.ChildNodes[0];
        //        switch (ShapeType)
        //        {
        //            case ShapeTypes.Point:
        //                foreach (XmlNode brk in breaks.ChildNodes)
        //                {
        //                    PointBreak aPB = new PointBreak();
        //                    aPB.Caption = brk.Attributes["Caption"].InnerText;
        //                    aPB.StartValue = brk.Attributes["StartValue"].InnerText;
        //                    aPB.EndValue = brk.Attributes["EndValue"].InnerText;
        //                    aPB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                    aPB.DrawPoint = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);                            
        //                    LegendBreaks.Add(aPB);
        //                }
        //                break;
        //            case ShapeTypes.Polyline:
        //            case ShapeTypes.PolylineZ:
        //                foreach (XmlNode brk in breaks.ChildNodes)
        //                {
        //                    if (brk.Attributes["Caption"].InnerText != "NoData")
        //                    {
        //                        PolyLineBreak aPLB = new PolyLineBreak();
        //                        aPLB.Caption = brk.Attributes["Caption"].InnerText;
        //                        aPLB.StartValue = brk.Attributes["StartValue"].InnerText;
        //                        aPLB.EndValue = brk.Attributes["EndValue"].InnerText;
        //                        aPLB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                        aPLB.DrawPolyline = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
        //                        LegendBreaks.Add(aPLB);
        //                    }
        //                }
        //                break;
        //            case ShapeTypes.Polygon:
        //                foreach (XmlNode brk in breaks.ChildNodes)
        //                {
        //                    if (brk.Attributes["Caption"].InnerText != "NoData")
        //                    {
        //                        PolygonBreak aPGB = new PolygonBreak();
        //                        aPGB.Caption = brk.Attributes["Caption"].InnerText;
        //                        aPGB.StartValue = Convert.ToDouble(brk.Attributes["StartValue"].InnerText);
        //                        aPGB.EndValue = Convert.ToDouble(brk.Attributes["EndValue"].InnerText);
        //                        aPGB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                        aPGB.DrawPolygon = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
        //                        aPGB.DrawFill = true;
        //                        LegendBreaks.Add(aPGB);
        //                    }
        //                }
        //                break;
        //            case ShapeTypes.Image:
        //                foreach (XmlNode brk in breaks.ChildNodes)
        //                {
        //                    ColorBreak aCB = new ColorBreak();
        //                    aCB.Caption = brk.Attributes["Caption"].InnerText;
        //                    aCB.StartValue = double.Parse(brk.Attributes["StartValue"].InnerText);
        //                    aCB.EndValue = double.Parse(brk.Attributes["EndValue"].InnerText);
        //                    aCB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                    LegendBreaks.Add(aCB);
        //                }
        //                break;
        //        }
        //        breakNum = LegendBreaks.Count;
        //    }
        //    else
        //    {
        //        ShapeType = aST;
        //        breakNum = Convert.ToInt32(LSNode.Attributes["BreakNum"].InnerText);
        //        HasNoData = Convert.ToBoolean(LSNode.Attributes["HasNoData"].InnerText);
        //        MinValue = Convert.ToDouble(LSNode.Attributes["MinValue"].InnerText);
        //        MaxValue = Convert.ToDouble(LSNode.Attributes["MaxValue"].InnerText);
        //        MissingValue = Convert.ToDouble(LSNode.Attributes["MissingValue"].InnerText);

        //        XmlNode breaks = LSNode.ChildNodes[0];
        //        switch (ShapeType)
        //        {
        //            case ShapeTypes.Point:
        //                foreach (XmlNode brk in breaks.ChildNodes)
        //                {
        //                    PointBreak aPB = new PointBreak();
        //                    aPB.Caption = brk.Attributes["Caption"].InnerText;
        //                    aPB.StartValue = brk.Attributes["StartValue"].InnerText;
        //                    aPB.EndValue = brk.Attributes["EndValue"].InnerText;
        //                    aPB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                    aPB.DrawPoint = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
        //                    aPB.DrawFill = Convert.ToBoolean(brk.Attributes["DrawFill"].InnerText);
        //                    aPB.DrawOutline = Convert.ToBoolean(brk.Attributes["DrawOutline"].InnerText);
        //                    aPB.IsNoData = Convert.ToBoolean(brk.Attributes["IsNoData"].InnerText);
        //                    aPB.OutlineColor = ColorTranslator.FromHtml(brk.Attributes["OutlineColor"].InnerText);
        //                    aPB.Size = Convert.ToSingle(brk.Attributes["Size"].InnerText);
        //                    aPB.Style = (PointStyle)Enum.Parse(typeof(PointStyle),
        //                        brk.Attributes["Style"].InnerText, true);
        //                    LegendBreaks.Add(aPB);
        //                }
        //                break;
        //            case ShapeTypes.Polyline:
        //            case ShapeTypes.PolylineZ:
        //                foreach (XmlNode brk in breaks.ChildNodes)
        //                {
        //                    PolyLineBreak aPLB = new PolyLineBreak();
        //                    try
        //                    {
        //                        aPLB.Caption = brk.Attributes["Caption"].InnerText;
        //                        aPLB.StartValue = brk.Attributes["StartValue"].InnerText;
        //                        aPLB.EndValue = brk.Attributes["EndValue"].InnerText;
        //                        aPLB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                        aPLB.DrawPolyline = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
        //                        aPLB.Size = Convert.ToSingle(brk.Attributes["Size"].InnerText);
        //                        aPLB.Style = (DashStyle)Enum.Parse(typeof(DashStyle),
        //                            brk.Attributes["Style"].InnerText, true);
        //                        aPLB.DrawSymbol = bool.Parse(brk.Attributes["DrawSymbol"].InnerText);
        //                        aPLB.SymbolSize = Single.Parse(brk.Attributes["SymbolSize"].InnerText);
        //                        aPLB.SymbolStyle = (PointStyle)Enum.Parse(typeof(PointStyle),
        //                            brk.Attributes["SymbolStyle"].InnerText, true);
        //                        aPLB.SymbolColor = ColorTranslator.FromHtml(brk.Attributes["SymbolColor"].InnerText);
        //                        aPLB.SymbolInterval = int.Parse(brk.Attributes["SymbolInterval"].InnerText);
        //                    }
        //                    catch
        //                    {

        //                    }
        //                    finally
        //                    {
        //                        LegendBreaks.Add(aPLB);
        //                    }
        //                }
        //                break;
        //            case ShapeTypes.Polygon:
        //                foreach (XmlNode brk in breaks.ChildNodes)
        //                {
        //                    PolygonBreak aPGB = new PolygonBreak();
        //                    try
        //                    {
        //                        aPGB.Caption = brk.Attributes["Caption"].InnerText;
        //                        aPGB.StartValue = brk.Attributes["StartValue"].InnerText;
        //                        aPGB.EndValue = brk.Attributes["EndValue"].InnerText;
        //                        aPGB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                        aPGB.DrawPolygon = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
        //                        aPGB.DrawFill = Convert.ToBoolean(brk.Attributes["DrawFill"].InnerText);
        //                        aPGB.DrawOutline = Convert.ToBoolean(brk.Attributes["DrawOutline"].InnerText);
        //                        aPGB.OutlineSize = Convert.ToSingle(brk.Attributes["OutlineSize"].InnerText);
        //                        aPGB.OutlineColor = ColorTranslator.FromHtml(brk.Attributes["OutlineColor"].InnerText);
        //                        aPGB.UsingHatchStyle = bool.Parse(brk.Attributes["UsingHatchStyle"].InnerText);
        //                        aPGB.Style = (HatchStyle)Enum.Parse(typeof(HatchStyle), brk.Attributes["Style"].InnerText, true);
        //                        aPGB.BackColor = ColorTranslator.FromHtml(brk.Attributes["BackColor"].InnerText);
        //                    }
        //                    catch
        //                    {

        //                    }
        //                    finally
        //                    {
        //                        LegendBreaks.Add(aPGB);
        //                    }
        //                }
        //                break;
        //            case ShapeTypes.Image:
        //                foreach (XmlNode brk in breaks.ChildNodes)
        //                {
        //                    ColorBreak aCB = new ColorBreak();
        //                    aCB.Caption = brk.Attributes["Caption"].InnerText;
        //                    aCB.StartValue = double.Parse(brk.Attributes["StartValue"].InnerText);
        //                    aCB.EndValue = double.Parse(brk.Attributes["EndValue"].InnerText);
        //                    aCB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
        //                    aCB.IsNoData = bool.Parse(brk.Attributes["IsNoData"].InnerText);
        //                    LegendBreaks.Add(aCB);
        //                }
        //                break;
        //        }
        //    }
        //}

        /// <summary>
        /// Import legend scheme from XML node
        /// </summary>
        /// <param name="LSNode">xml node</param>
        public void ImportFromXML(XmlNode LSNode)
        {
            ImportFromXML(LSNode, true);     
        }

        /// <summary>
        /// Import legend scheme from XML node
        /// </summary>
        /// <param name="LSNode">xml node</param>
        /// <param name="keepShape">if keep the legend shape type</param>
        public void ImportFromXML(XmlNode LSNode, bool keepShape)
        {
            LegendBreaks = new List<ColorBreak>();

            if (!(LSNode.Attributes["FieldName"] == null))
            {
                FieldName = LSNode.Attributes["FieldName"].InnerText;
            }
            LegendType = (LegendType)Enum.Parse(typeof(LegendType),
                LSNode.Attributes["LegendType"].InnerText, true);
            ShapeTypes aShapeType = (ShapeTypes)Enum.Parse(typeof(ShapeTypes),
                LSNode.Attributes["ShapeType"].InnerText, true);

            //BreakNum = Convert.ToInt32(LSNode.Attributes["BreakNum"].InnerText);
            HasNoData = Convert.ToBoolean(LSNode.Attributes["HasNoData"].InnerText);
            MinValue = Convert.ToDouble(LSNode.Attributes["MinValue"].InnerText);
            MaxValue = Convert.ToDouble(LSNode.Attributes["MaxValue"].InnerText);
            MissingValue = Convert.ToDouble(LSNode.Attributes["UNDEF"].InnerText);

            if (!keepShape)
                ShapeType = aShapeType;
            bool sameShapeType = (ShapeType == aShapeType);
            ImportBreaks(LSNode, sameShapeType);
        }

        private void ImportBreaks(XmlNode parent, bool sameShapeType)
        {
            XmlNode breaks = parent.ChildNodes[0];

            if (sameShapeType)
            {
                switch (ShapeType)
                {
                    case ShapeTypes.Point:
                        foreach (XmlNode brk in breaks.ChildNodes)
                        {
                            PointBreak aPB = new PointBreak();
                            try
                            {
                                aPB.Caption = brk.Attributes["Caption"].InnerText;
                                aPB.StartValue = brk.Attributes["StartValue"].InnerText;
                                aPB.EndValue = brk.Attributes["EndValue"].InnerText;
                                aPB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
                                aPB.DrawShape = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
                                aPB.DrawFill = Convert.ToBoolean(brk.Attributes["DrawFill"].InnerText);
                                aPB.DrawOutline = Convert.ToBoolean(brk.Attributes["DrawOutline"].InnerText);
                                aPB.IsNoData = Convert.ToBoolean(brk.Attributes["IsNoData"].InnerText);
                                aPB.OutlineColor = ColorTranslator.FromHtml(brk.Attributes["OutlineColor"].InnerText);
                                aPB.Size = Convert.ToSingle(brk.Attributes["Size"].InnerText);
                                aPB.Style = (PointStyle)Enum.Parse(typeof(PointStyle),
                                    brk.Attributes["Style"].InnerText, true);
                                aPB.MarkerType = (MarkerType)Enum.Parse(typeof(MarkerType), brk.Attributes["MarkerType"].InnerText, true);
                                aPB.FontName = brk.Attributes["FontName"].InnerText;
                                aPB.CharIndex = int.Parse(brk.Attributes["CharIndex"].InnerText);
                                aPB.ImagePath = brk.Attributes["ImagePath"].InnerText;
                                aPB.Angle = float.Parse(brk.Attributes["Angle"].InnerText);
                            }
                            catch
                            {

                            }
                            finally
                            {
                                LegendBreaks.Add(aPB);
                            }
                        }
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineZ:
                        foreach (XmlNode brk in breaks.ChildNodes)
                        {
                            PolyLineBreak aPLB = new PolyLineBreak();
                            try
                            {
                                aPLB.Caption = brk.Attributes["Caption"].InnerText;
                                aPLB.StartValue = brk.Attributes["StartValue"].InnerText;
                                aPLB.EndValue = brk.Attributes["EndValue"].InnerText;
                                aPLB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
                                aPLB.DrawPolyline = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
                                aPLB.Size = Convert.ToSingle(brk.Attributes["Size"].InnerText);
                                aPLB.Style = (LineStyles)Enum.Parse(typeof(LineStyles),
                                    brk.Attributes["Style"].InnerText, true);
                                aPLB.DrawSymbol = bool.Parse(brk.Attributes["DrawSymbol"].InnerText);
                                aPLB.SymbolSize = Single.Parse(brk.Attributes["SymbolSize"].InnerText);
                                aPLB.SymbolStyle = (PointStyle)Enum.Parse(typeof(PointStyle),
                                    brk.Attributes["SymbolStyle"].InnerText, true);
                                aPLB.SymbolColor = ColorTranslator.FromHtml(brk.Attributes["SymbolColor"].InnerText);
                                aPLB.SymbolInterval = int.Parse(brk.Attributes["SymbolInterval"].InnerText);
                            }
                            catch
                            {

                            }
                            finally
                            {
                                LegendBreaks.Add(aPLB);
                            }
                        }
                        break;
                    case ShapeTypes.Polygon:
                        foreach (XmlNode brk in breaks.ChildNodes)
                        {
                            PolygonBreak aPGB = new PolygonBreak();
                            try
                            {
                                aPGB.Caption = brk.Attributes["Caption"].InnerText;
                                aPGB.StartValue = brk.Attributes["StartValue"].InnerText;
                                aPGB.EndValue = brk.Attributes["EndValue"].InnerText;
                                aPGB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
                                aPGB.DrawShape = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
                                aPGB.DrawFill = Convert.ToBoolean(brk.Attributes["DrawFill"].InnerText);
                                aPGB.DrawOutline = Convert.ToBoolean(brk.Attributes["DrawOutline"].InnerText);
                                aPGB.OutlineSize = Convert.ToSingle(brk.Attributes["OutlineSize"].InnerText);
                                aPGB.OutlineColor = ColorTranslator.FromHtml(brk.Attributes["OutlineColor"].InnerText);
                                aPGB.UsingHatchStyle = bool.Parse(brk.Attributes["UsingHatchStyle"].InnerText);
                                aPGB.Style = (HatchStyle)Enum.Parse(typeof(HatchStyle), brk.Attributes["Style"].InnerText, true);
                                aPGB.BackColor = ColorTranslator.FromHtml(brk.Attributes["BackColor"].InnerText);
                            }
                            catch
                            {

                            }
                            finally
                            {
                                LegendBreaks.Add(aPGB);
                            }
                        }
                        break;
                    case ShapeTypes.Image:
                        foreach (XmlNode brk in breaks.ChildNodes)
                        {
                            ColorBreak aCB = new ColorBreak();
                            aCB.Caption = brk.Attributes["Caption"].InnerText;
                            aCB.StartValue = double.Parse(brk.Attributes["StartValue"].InnerText);
                            aCB.EndValue = double.Parse(brk.Attributes["EndValue"].InnerText);
                            aCB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
                            aCB.IsNoData = bool.Parse(brk.Attributes["IsNoData"].InnerText);
                            LegendBreaks.Add(aCB);
                        }
                        break;
                }
            }
            else
            {
                switch (ShapeType)
                {
                    case ShapeTypes.Point:
                        foreach (XmlNode brk in breaks.ChildNodes)
                        {
                            PointBreak aPB = new PointBreak();
                            aPB.Caption = brk.Attributes["Caption"].InnerText;
                            aPB.StartValue = brk.Attributes["StartValue"].InnerText;
                            aPB.EndValue = brk.Attributes["EndValue"].InnerText;
                            aPB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
                            aPB.DrawShape = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
                            LegendBreaks.Add(aPB);
                        }
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineZ:
                        foreach (XmlNode brk in breaks.ChildNodes)
                        {
                            if (brk.Attributes["Caption"].InnerText != "NoData")
                            {
                                PolyLineBreak aPLB = new PolyLineBreak();
                                aPLB.Caption = brk.Attributes["Caption"].InnerText;
                                aPLB.StartValue = brk.Attributes["StartValue"].InnerText;
                                aPLB.EndValue = brk.Attributes["EndValue"].InnerText;
                                aPLB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
                                aPLB.DrawPolyline = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
                                LegendBreaks.Add(aPLB);
                            }
                        }
                        break;
                    case ShapeTypes.Polygon:
                        foreach (XmlNode brk in breaks.ChildNodes)
                        {
                            if (brk.Attributes["Caption"].InnerText != "NoData")
                            {
                                PolygonBreak aPGB = new PolygonBreak();
                                aPGB.Caption = brk.Attributes["Caption"].InnerText;
                                aPGB.StartValue = Convert.ToDouble(brk.Attributes["StartValue"].InnerText);
                                aPGB.EndValue = Convert.ToDouble(brk.Attributes["EndValue"].InnerText);
                                aPGB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
                                aPGB.DrawShape = Convert.ToBoolean(brk.Attributes["DrawShape"].InnerText);
                                aPGB.DrawFill = true;
                                LegendBreaks.Add(aPGB);
                            }
                        }
                        break;
                    case ShapeTypes.Image:
                        foreach (XmlNode brk in breaks.ChildNodes)
                        {
                            ColorBreak aCB = new ColorBreak();
                            aCB.Caption = brk.Attributes["Caption"].InnerText;
                            aCB.StartValue = double.Parse(brk.Attributes["StartValue"].InnerText);
                            aCB.EndValue = double.Parse(brk.Attributes["EndValue"].InnerText);
                            aCB.Color = ColorTranslator.FromHtml(brk.Attributes["Color"].InnerText);
                            LegendBreaks.Add(aCB);
                        }
                        break;
                }
                //breakNum = LegendBreaks.Count;
            }
        }

        ///// <summary>
        ///// Import legend scheme from XML file
        ///// </summary>
        ///// <param name="aFile">file path</param>
        ///// <param name="aST">shape type</param>
        //public void ImportFromXMLFile(string aFile, ShapeTypes aST)
        //{            
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(aFile);

        //    XmlElement root = doc.DocumentElement;
        //    XmlNode LSNode;
        //    if (root.InnerText == "MeteoInfo")
        //        LSNode = root.GetElementsByTagName("LegendScheme")[0];
        //    else
        //        LSNode = root;

        //    ImportFromXML(LSNode, aST);
        //}

        /// <summary>
        /// Import legend scheme from XML file
        /// </summary>
        /// <param name="aFile">file path</param>        
        public void ImportFromXMLFile(string aFile)
        {
            ImportFromXMLFile(aFile, true);
        }

        /// <summary>
        /// Import legend scheme from XML file
        /// </summary>
        /// <param name="aFile">file path</param>  
        /// <param name="keepShape">if keep shape type</param>
        public void ImportFromXMLFile(string aFile, bool keepShape)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(aFile);

            XmlElement root = doc.DocumentElement;
            XmlNode LSNode;
            if (root.Name == "MeteoInfo")
                LSNode = root.GetElementsByTagName("LegendScheme")[0];
            else
                LSNode = root;

            ImportFromXML(LSNode, keepShape);
        }

        /// <summary>
        /// Import legend scheme from an image color palette file
        /// </summary>
        /// <param name="aFile">file path</param>
        public void ImportFromPaletteFile_Unique(string aFile)
        {
            StreamReader sr = new StreamReader(aFile);
            this.ShapeType = ShapeTypes.Image;
            this.LegendType = LegendType.UniqueValue;
            this.LegendBreaks = new List<ColorBreak>();
            ColorBreak aCB = new ColorBreak();

            string[] dataArray;
            int lastNonEmpty, i;
            sr.ReadLine();
            string aLine = sr.ReadLine();
            while (aLine != null)
            {
                dataArray = aLine.Split();
                lastNonEmpty = -1;
                List<int> dataList = new List<int>();
                for (i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] != string.Empty)
                    {
                        lastNonEmpty++;
                        dataList.Add(int.Parse(dataArray[i]));
                    }
                }
                Color aColor = Color.FromArgb(255, dataList[3], dataList[2], dataList[1]);
                aCB = new ColorBreak();
                aCB.Color = aColor;
                aCB.StartValue = dataList[0];
                aCB.EndValue = dataList[0];
                aCB.Caption = aCB.StartValue.ToString();
                this.LegendBreaks.Add(aCB);

                aLine = sr.ReadLine();
            }
            sr.Close();
        }

        /// <summary>
        /// Import legend scheme from an image color palette file
        /// </summary>
        /// <param name="aFile">file path</param>
        public void ImportFromPaletteFile_Graduated(string aFile)
        {
            StreamReader sr = new StreamReader(aFile);
            this.ShapeType = ShapeTypes.Image;
            this.LegendType = LegendType.GraduatedColor;
            this.LegendBreaks = new List<ColorBreak>();
            List<Color> colorList = new List<Color>();
            List<int> values = new List<int>();
            ColorBreak aCB = new ColorBreak();

            string[] dataArray;
            int lastNonEmpty, i;
            sr.ReadLine();
            string aLine = sr.ReadLine();
            while (aLine != null)
            {
                dataArray = aLine.Split();
                lastNonEmpty = -1;
                List<int> dataList = new List<int>();
                for (i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] != string.Empty)
                    {
                        lastNonEmpty++;
                        dataList.Add(int.Parse(dataArray[i]));
                    }
                }
                Color aColor = Color.FromArgb(255, dataList[3], dataList[2], dataList[1]);
                if (colorList.Count == 0)
                    colorList.Add(aColor);
                else
                {
                    if (!colorList.Contains(aColor))
                    {
                        aCB = new ColorBreak();
                        aCB.Color = aColor;
                        aCB.StartValue = values.Min();
                        aCB.EndValue = values.Max();
                        if (aCB.StartValue.ToString() == aCB.EndValue.ToString())
                        {
                            aCB.Caption = aCB.StartValue.ToString();
                        }
                        else
                        {
                            if (this.LegendBreaks.Count == 0)
                                aCB.Caption = "< " + aCB.EndValue.ToString();
                            else
                                aCB.Caption = aCB.StartValue.ToString() + " - " + aCB.EndValue.ToString();
                        }
                        this.LegendBreaks.Add(aCB);

                        values.Clear();
                        colorList.Add(aColor);
                    }
                }
                values.Add(dataList[0]);

                aLine = sr.ReadLine();
            }
            sr.Close();

            aCB = new ColorBreak();
            aCB.Color = colorList[colorList.Count - 1];
            aCB.StartValue = values.Min();
            aCB.EndValue = values.Max();
            aCB.Caption = "> " + aCB.StartValue.ToString();
            this.LegendBreaks.Add(aCB);
        }

        /// <summary>
        /// Clone legend scheme
        /// </summary>
        /// <returns>legend scheme</returns>
        public object Clone()
        {
            LegendScheme bLS = new LegendScheme(ShapeType);
            bLS.FieldName = FieldName;
            //bLS.breakNum = breakNum;
            bLS.HasNoData = HasNoData;
            bLS.LegendType = LegendType;            
            bLS.MinValue = MinValue;
            bLS.MaxValue = MaxValue;
            bLS.MissingValue = MissingValue;
            foreach (ColorBreak aCB in LegendBreaks)
                bLS.LegendBreaks.Add((ColorBreak)aCB.Clone());                      

            return bLS;
        }        

        #endregion
    }
}
