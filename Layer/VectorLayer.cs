using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml;
using MeteoInfoC.Global;
using MeteoInfoC.Shape;
using MeteoInfoC.Legend;
using MeteoInfoC.Drawing;
using MeteoInfoC.Data.MapData;
using MeteoInfoC.Map;
using MeteoInfoC.Projections;
using MeteoInfoC.Geoprocess;

namespace MeteoInfoC.Layer
{
    /// <summary>
    /// Vector Layer
    /// </summary>
    public class VectorLayer:MapLayer, IDisposable
    {
        #region Private Variables
        private bool _isEditing;
        private bool _avoidCollision;        
        //private int m_shapeNum;
        private List<Shape.Shape> _shapeList;            
        private AttributeTable _attributeTable;
        private LabelSet _labelSet;        
        private List<Graphic> _labelPoints;
        private ChartSet _chartSet;
        private List<Graphic> _chartPoints;

        private int _numFields;
        private int _identiferShape;

        private float _drawingZoom = 1.0f;

        private List<Shape.Shape> _originShapes = null;
        private AttributeTable _originAttributeTable = null;
        private List<Graphic> _originLabelPoints = null;
        private List<Graphic> _originChartPoints = null;
        private bool _isProjected = false;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public VectorLayer(ShapeTypes shapeType):base()
        {
            LayerType = LayerTypes.VectorLayer;
            ShapeType = shapeType;                                 
            _avoidCollision = true;                      
            _attributeTable = new AttributeTable();
            _labelSet = new LabelSet();           
            _labelPoints = new List<Graphic>();
            _chartSet = new ChartSet();
            _chartPoints = new List<Graphic>();
            _shapeList = new List<MeteoInfoC.Shape.Shape>();
            //m_shapeNum = 0;
            _isEditing = false;          
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or set if enable avoid collision
        /// </summary>
        [CategoryAttribute("Layer Set Edit"), DescriptionAttribute("Set layer if enable avoid collision")]
        public bool AvoidCollision
        {
            get { return _avoidCollision; }
            set { _avoidCollision = value; }
        }

        /// <summary>
        /// Get shape number
        /// </summary>
        [CategoryAttribute("Layer Property"), DescriptionAttribute("layer shape number")]
        public int ShapeNum
        {
            get { return _shapeList.Count; }           
        }

        /// <summary>
        /// Get or set shapes
        /// </summary>
        public List<Shape.Shape> ShapeList
        {
            get { return _shapeList; }
            set { _shapeList = value; }
        }      

        /// <summary>
        /// Get or set AttributeTable
        /// </summary>
        public AttributeTable AttributeTable
        {
            get { return _attributeTable; }
            set { _attributeTable = value; }
        }
        
        /// <summary>
        /// Get or set label set
        /// </summary>
        public LabelSet LabelSet
        {
            get { return _labelSet; }
            set { _labelSet = value; }
        }

        /// <summary>
        /// Get or set label points
        /// </summary>
        public List<Graphic> LabelPoints
        {
            get { return _labelPoints; }
            set { _labelPoints = value; }
        }

        /// <summary>
        /// Get or set chart set
        /// </summary>
        public ChartSet ChartSet
        {
            get { return _chartSet; }
            set { _chartSet = value; }
        }

        /// <summary>
        /// Get or set chart points
        /// </summary>
        public List<Graphic> ChartPoints
        {
            get { return _chartPoints; }
            set { _chartPoints = value; }
        }

        /// <summary>
        /// Get or set fields number
        /// </summary>
        public int NumFields
        {
            get { return _numFields; }
            set { _numFields = value; }
        }

        /// <summary>
        /// Get fields
        /// </summary>
        public DataColumnCollection Fields
        {
            get { return GetFields(); }
        }

        /// <summary>
        /// Get or set if the layer is editing
        /// </summary>
        public bool IsEditing
        {
            get { return _isEditing; }
            set { _isEditing = value; }
        }

        /// <summary>
        /// Override TransparencyPerc property
        /// </summary>
        public override int TransparencyPerc
        {
            get
            {
                return base.TransparencyPerc;
            }
            set
            {
                base.TransparencyPerc = value;
                if (ShapeType == ShapeTypes.Polygon)
                {
                    foreach (PolygonBreak aPGB in LegendScheme.LegendBreaks)
                    {
                        //aPGB.TransparencyPercent = value;
                        int alpha = (int)((1 - (double)value / 100.0) * 255);
                        aPGB.Color = Color.FromArgb(alpha, aPGB.Color);
                    }
                }
            }
        }

        /// <summary>
        /// Override LegendScheme property
        /// </summary>
        public override LegendScheme LegendScheme
        {
            get
            {
                return base.LegendScheme;
            }
            set
            {
                base.LegendScheme = value;
                UpdateLegendIndexes();
            }
        }

        /// <summary>
        /// Get or set identifer shape
        /// </summary>
        public int IdentiferShape
        {
            get { return _identiferShape; }
            set { _identiferShape = value; }
        }

        /// <summary>
        /// Get or set drawing zoom
        /// </summary>
        public float DrawingZoom
        {
            get { return _drawingZoom; }
            set { _drawingZoom = value; }
        }

        /// <summary>
        /// Get or set if is projected
        /// </summary>
        public bool IsProjected
        {
            get { return _isProjected; }
            set { _isProjected = value; }
        }

        #endregion

        #region Methods

        #region DataTable
        /// <summary>
        /// Start editing table
        /// </summary>
        public void StartEditingTable()
        {
            _attributeTable.Table.BeginLoadData();
        }

        /// <summary>
        /// Stop editing table
        /// </summary>
        /// <param name="applyChanges"></param>
        public void StopEditingTable(bool applyChanges)
        {
            _attributeTable.Table.EndLoadData();
            if (applyChanges)
                _attributeTable.Save();
        }

        /// <summary>
        /// Get field name by index
        /// </summary>
        /// <param name="FieldIndex">field index</param>
        /// <returns>field name</returns>
        public string GetFieldName(int FieldIndex)
        {
            return _attributeTable.Table.Columns[FieldIndex].ColumnName;
        }

        /// <summary>
        /// Get field index by name
        /// </summary>
        /// <param name="fieldName">field name</param>
        /// <returns>field index</returns>
        public int GetFieldIdxByName(string fieldName)
        {
            int fieldIdx = -1;
            for (int i = 0; i < NumFields; i++)
            {
                if (_attributeTable.Table.Columns[i].ColumnName == fieldName)
                {
                    fieldIdx = i;
                    break;
                }
            }

            return fieldIdx;
        }

        /// <summary>
        /// Get field name list
        /// </summary>
        /// <returns>field name list</returns>
        public List<string> GetFieldNameList()
        {
            List<string> FNList = new List<string>();
            for (int i = 0; i < _numFields; i++)
            {
                FNList.Add(GetFieldName(i));
            }
            return FNList;
        }

        /// <summary>
        /// Get field by index
        /// </summary>
        /// <param name="FieldIndex">field index</param>
        /// <returns>field</returns>
        public DataColumn GetField(int FieldIndex)
        {
            return _attributeTable.Table.Columns[FieldIndex];
        }

        /// <summary>
        /// Get fields
        /// </summary>
        /// <returns>fields</returns>
        public DataColumnCollection GetFields()
        {
            return _attributeTable.Table.Columns;
        }

        /// <summary>
        /// Get cell value
        /// </summary>
        /// <param name="FieldIndex">Field index</param>
        /// <param name="ShapeIndex">Shape index</param>
        /// <returns>Cell value</returns>
        public object GetCellValue(int FieldIndex, int ShapeIndex)
        {
            //return m_DataTable.Rows[ShapeIndex][FieldIndex];
            return _attributeTable.Table.Rows[ShapeIndex][FieldIndex];
        }

        /// <summary>
        /// Get cell value
        /// </summary>
        /// <param name="FieldName">Field name</param>
        /// <param name="ShapeIndex">Shape index</param>
        /// <returns>Cell value</returns>
        public object GetCellValue(string FieldName, int ShapeIndex)
        {
            //return m_DataTable.Rows[ShapeIndex][FieldName];
            return _attributeTable.Table.Rows[ShapeIndex][FieldName];
        }        

        /// <summary>
        /// Edit cell value
        /// </summary>
        /// <param name="FieldName">field name</param>
        /// <param name="ShapeIndex">shape index</param>
        /// <param name="value">value</param>
        public void EditCellValue(string FieldName, int ShapeIndex, object value)
        {
            //m_DataTable.Rows[ShapeIndex][FieldName] = value;
            _attributeTable.Table.Rows[ShapeIndex][FieldName] = value;
        }

        /// <summary>
        /// Edit cell value
        /// </summary>
        /// <param name="FieldIndex">field index</param>
        /// <param name="ShapeIndex">shape index</param>
        /// <param name="value">value</param>
        public void EditCellValue(int FieldIndex, int ShapeIndex, object value)
        {            
            _attributeTable.Table.Rows[ShapeIndex][FieldIndex] = value;
        }

        /// <summary>
        /// Get minimum data value of a field
        /// </summary>
        /// <param name="fieldName">field name</param>
        /// <returns>minimum data</returns>
        public double GetMinValue(string fieldName)
        {
            if (MIMath.IsNumeric(_attributeTable.Table.Columns[fieldName]))
            {
                double min = 0;
                int dNum = 0;
                for (int i = 0; i < ShapeNum; i++)
                {
                    double aValue = double.Parse(GetCellValue(fieldName, i).ToString());
                    if (Math.Abs(aValue / LegendScheme.MissingValue - 1) < 0.01)
                        continue;

                    if (dNum == 0)
                        min = aValue;
                    else
                    {
                        if (min > aValue)
                            min = aValue;
                    }
                    dNum += 1;
                }
                return min;
            }
            else
                return 0;
        }
        #endregion

        #region Label
        /// <summary>
        /// Get label points
        /// </summary>
        /// <returns>Label points</returns>
        public List<Graphic> GetLabelPoints()
        {
            return _labelPoints;
        }

        /// <summary>
        /// Add label point
        /// </summary>
        /// <param name="aLP">Label point</param>
        public void AddLabel(Graphic aLP)
        {
            _labelPoints.Add(aLP);            
        }

        /// <summary>
        /// Remove all labels
        /// </summary>
        public void RemoveLabels()
        {
            _labelPoints.Clear();
            _labelSet.DrawLabels = false;
        }

        /// <summary>
        /// Add labels
        /// </summary>
        public void AddLabels()
        {
            AddLabelsByColor();

            //if (_labelSet.ColorByLegend)
            //    AddLabelsByLegend();
            //else
            //    AddLabelsByColor();

            _labelSet.DrawLabels = true;
        }

        private ColorBreak GetColorBreak(string vStr)
        {
            ColorBreak aCB = null;
            switch (LegendScheme.LegendType)
            {
                case LegendType.SingleSymbol:
                    aCB = LegendScheme.LegendBreaks[0];
                    break;
                case LegendType.UniqueValue:                    
                    for (int j = 0; j < LegendScheme.LegendBreaks.Count; j++)
                    {
                        if (vStr == LegendScheme.LegendBreaks[j].StartValue.ToString())
                        {
                            aCB = LegendScheme.LegendBreaks[j];
                            break;
                        }
                    }
                    break;
                case LegendType.GraduatedColor:
                    double value;                    
                    if (vStr == string.Empty || vStr == null)
                        value = 0;
                    else
                        value = double.Parse(vStr);
                    int blNum = 0;
                    for (int j = 0; j < LegendScheme.LegendBreaks.Count; j++)
                    {
                        ColorBreak aPB = LegendScheme.LegendBreaks[j];
                        blNum += 1;
                        if (value == double.Parse(aPB.StartValue.ToString()) || (value > double.Parse(aPB.StartValue.ToString())
                            && value < double.Parse(aPB.EndValue.ToString())) ||
                            (blNum == LegendScheme.LegendBreaks.Count && value == double.Parse(aPB.EndValue.ToString())))
                        {
                            aCB = aPB;
                            break;
                        }
                    }
                    break;
            }

            return aCB;
        }

        private bool IsShapeVisible(int shapeIdx)
        {
            string vStr = GetCellValue(LegendScheme.FieldName, shapeIdx).ToString().Trim();
            ColorBreak aCB = GetColorBreak(vStr);            

            if (aCB == null)
                return false;
            else
                return aCB.DrawShape;
        }

        /// <summary>
        /// Add labels by legend scheme colors
        /// </summary>        
        private void AddLabelsByLegend()
        {
            List<Shape.Shape> shapeList = new List<MeteoInfoC.Shape.Shape>(ShapeList);
            int shapeIdx = -1;           

            switch (ShapeType)
            {                
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineZ:
                    foreach (PolylineShape aPLS in shapeList)
                    {
                        shapeIdx += 1;
                        if (!IsShapeVisible(shapeIdx))
                            continue;

                        int pIdx = aPLS.Points.Count / 2;  
                        PointShape aPS = new PointShape();
                        aPS.Point = (PointD)aPLS.Points[pIdx - 1]; 

                        LabelBreak aLP = new LabelBreak();                                                                                                                 
                        aLP.Text = GetCellValue(_labelSet.FieldName, shapeIdx).ToString();                        
                        aLP.Font = _labelSet.LabelFont;
                        aLP.AlignType = _labelSet.LabelAlignType;
                        aLP.YShift = _labelSet.YOffset;
                        aLP.XShift = _labelSet.XOffset;

                        string vStr;
                        PolyLineBreak aPLB = new PolyLineBreak();
                        switch (LegendScheme.LegendType)
                        {            
                            case LegendType.SingleSymbol:
                                aPLB = (PolyLineBreak)LegendScheme.LegendBreaks[0];
                                aLP.Color = aPLB.Color;
                                break;
                            case LegendType.UniqueValue:
                                vStr = GetCellValue(_labelSet.FieldName, shapeIdx).ToString();
                                for (int j = 0; j < LegendScheme.LegendBreaks.Count; j++)
                                {
                                    aPLB = (PolyLineBreak)LegendScheme.LegendBreaks[j];
                                    if (vStr == aPLB.StartValue.ToString())
                                    {
                                        aLP.Color = aPLB.Color;
                                    }
                                }
                                break;
                            case LegendType.GraduatedColor:
                                vStr = GetCellValue(_labelSet.FieldName, shapeIdx).ToString();
                                double value = double.Parse(vStr);
                                int blNum = 0;
                                for (int j = 0; j < LegendScheme.LegendBreaks.Count; j++)
                                {
                                    aPLB = (PolyLineBreak)LegendScheme.LegendBreaks[j];
                                    blNum += 1;
                                    if (value == double.Parse(aPLB.StartValue.ToString()) ||
                                        (value > double.Parse(aPLB.StartValue.ToString()) &&
                                        value < double.Parse(aPLB.EndValue.ToString())) ||
                                        (blNum == LegendScheme.LegendBreaks.Count && value == double.Parse(aPLB.EndValue.ToString())))
                                    {
                                        aLP.Color = aPLB.Color;
                                    }
                                }
                                break;
                        }

                        Graphic aGraphic = new Graphic();
                        aGraphic.Shape = aPS;
                        aGraphic.Legend = aLP;
                        AddLabel(aGraphic);                        
                    }
                    break;
            }
        }

        /// <summary>
        /// Add labels
        /// </summary>        
        private void AddLabelsByColor()
        {
            List<Shape.Shape> shapeList = new List<MeteoInfoC.Shape.Shape>(ShapeList);
            int shapeIdx = -1;
            PointD aPoint = new PointD();

            string dFormat = "f1";
            bool isData = false;
            if (MIMath.IsNumeric(_attributeTable.Table.Columns[_labelSet.FieldName]))
            {
                if (_labelSet.AutoDecimal)
                {
                    double min = GetMinValue(_labelSet.FieldName);
                    _labelSet.DecimalDigits = MIMath.GetDecimalNum(min);
                }

                dFormat = "f" + _labelSet.DecimalDigits.ToString();
                isData = true;
            }

            List<int> selShapeIdx = GetSelectedShapeIndexes();
            bool isShapeSel = true;
            if (selShapeIdx.Count == 0)
                isShapeSel = false;
            foreach (Shape.Shape aShape in shapeList)
            {
                shapeIdx += 1;
                if (isShapeSel)
                {
                    if (!aShape.Selected)
                        continue;
                }

                ColorBreak aCB = null;
                if (LegendScheme.LegendType == LegendType.SingleSymbol)
                    aCB = LegendScheme.LegendBreaks[0];
                else
                {
                    if (LegendScheme.FieldName != null)
                    {
                        string vStr = GetCellValue(LegendScheme.FieldName, shapeIdx).ToString().Trim();
                        aCB = GetColorBreak(vStr);
                    }
                }
                if (aCB == null)
                    continue;
                if (!aCB.DrawShape)
                    continue;

                PointShape aPS = new PointShape();
                switch (ShapeType)
                {
                    case ShapeTypes.Point:
                    case ShapeTypes.PointM:
                    case ShapeTypes.PointZ:
                    case ShapeTypes.WindArraw:
                    case ShapeTypes.WindBarb:
                    case ShapeTypes.WeatherSymbol:
                    case ShapeTypes.StationModel:
                        aPS.Point = (PointD)((PointShape)aShape).Point.Clone();
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineM:
                    case ShapeTypes.PolylineZ:
                        int pIdx = ((PolylineShape)aShape).Points.Count / 2;
                        aPS.Point = (PointD)((PolylineShape)aShape).Points[pIdx - 1].Clone();
                        break;
                    case ShapeTypes.Polygon:
                    case ShapeTypes.PolygonM:
                        Extent aExtent = aShape.Extent;
                        aPoint = new PointD();
                        aPoint.X = ((aExtent.minX + aExtent.maxX) / 2);
                        aPoint.Y = ((aExtent.minY + aExtent.maxY) / 2);                        
                        aPS.Point = aPoint;
                        break;
                }

                LabelBreak aLP = new LabelBreak();
                if (isData)
                    aLP.Text = double.Parse(GetCellValue(_labelSet.FieldName, shapeIdx).ToString()).ToString(dFormat);
                else
                    aLP.Text = GetCellValue(_labelSet.FieldName, shapeIdx).ToString();

                if (_labelSet.ColorByLegend)
                    aLP.Color = aCB.Color;
                else
                    aLP.Color = _labelSet.LabelColor;
                aLP.Font = _labelSet.LabelFont;
                aLP.AlignType = _labelSet.LabelAlignType;
                aLP.YShift = _labelSet.YOffset;
                aLP.XShift = _labelSet.XOffset;
                Graphic aGraphic = new Graphic(aPS, aLP);
                AddLabel(aGraphic);
            }            
        }

        ///// <summary>
        ///// Add labels
        ///// </summary>        
        //private void AddLabelsByColor_Old()
        //{
        //    List<Shape.Shape> shapeList = new List<MeteoInfoC.Shape.Shape>(ShapeList);
        //    int shapeIdx = -1;
        //    PointD aPoint = new PointD();

        //    string dFormat = "f1";
        //    bool isData = false;
        //    if (MIMath.IsNumeric(_attributeTable.Table.Columns[_labelSet.FieldName]))
        //    {
        //        double min = GetMinValue(_labelSet.FieldName);
        //        int dNum = MIMath.GetDecimalNum(min);
        //        dFormat = "f" + dNum.ToString();
        //        isData = true;
        //    }

        //    switch (ShapeType)
        //    {
        //        case ShapeTypes.Point:
        //            foreach (PointShape aPS in shapeList)
        //            {
        //                shapeIdx += 1;
        //                if (!IsShapeVisible(shapeIdx))
        //                    continue;

        //                PointShape newPS = new PointShape();
        //                newPS.Point = aPS.Point;
        //                LabelBreak aLP = new LabelBreak();                                                
        //                if (isData)
        //                    aLP.Text = double.Parse(GetCellValue(_labelSet.FieldName, shapeIdx).ToString()).ToString(dFormat);
        //                else
        //                    aLP.Text = GetCellValue(_labelSet.FieldName, shapeIdx).ToString();
        //                aLP.Color = _labelSet.LabelColor;
        //                aLP.Font = _labelSet.LabelFont;
        //                aLP.AlignType = _labelSet.LabelAlignType;
        //                aLP.YShift = _labelSet.YOffset;
        //                Graphic aGraphic = new Graphic(newPS, aLP);
        //                AddLabel(aGraphic);                        
        //            }
        //            break;
        //        case ShapeTypes.Polygon:
        //            foreach (PolygonShape aPGS in shapeList)
        //            {
        //                shapeIdx += 1;
        //                if (!IsShapeVisible(shapeIdx))
        //                    continue;

        //                LabelBreak aLP = new LabelBreak();
        //                Extent aExtent = aPGS.Extent;
        //                aPoint.X = (Single)((aExtent.minX + aExtent.maxX) / 2);
        //                aPoint.Y = (Single)((aExtent.minY + aExtent.maxY) / 2);
        //                PointShape aPS = new PointShape();
        //                aPS.Point = aPoint;
        //                if (isData)
        //                    aLP.Text = double.Parse(GetCellValue(_labelSet.FieldName, shapeIdx).ToString()).ToString(dFormat);
        //                else
        //                    aLP.Text = GetCellValue(_labelSet.FieldName, shapeIdx).ToString();
        //                aLP.Color = _labelSet.LabelColor;
        //                aLP.Font = _labelSet.LabelFont;
        //                aLP.AlignType = _labelSet.LabelAlignType;
        //                aLP.YShift = _labelSet.YOffset;
        //                Graphic aGraphic = new Graphic(aPS, aLP);
        //                AddLabel(aGraphic);                        
        //            }
        //            break;
        //        case ShapeTypes.Polyline:
        //        case ShapeTypes.PolylineZ:
        //            foreach (PolylineShape aPLS in shapeList)
        //            {
        //                shapeIdx += 1;
        //                if (!IsShapeVisible(shapeIdx))
        //                    continue;

        //                LabelBreak aLP = new LabelBreak();
        //                int pIdx = aPLS.Points.Count / 2;
        //                //Single angle = (Single)(Math.Atan((((PointD)aPLS.points[pIdx]).y - ((PointD)aPLS.points[pIdx - 1]).y) / 
        //                //(((PointD)aPLS.points[pIdx]).x - ((PointD)aPLS.points[pIdx - 1]).x)) * 180 / Math.PI);   
        //                PointShape aPS = new PointShape();
        //                aPS.Point = (PointD)aPLS.Points[pIdx - 1];
        //                //aLP.Angle = angle;
        //                aLP.Text = GetCellValue(_labelSet.FieldName, shapeIdx).ToString();
        //                aLP.Color = _labelSet.LabelColor;
        //                aLP.Font = _labelSet.LabelFont;
        //                aLP.AlignType = _labelSet.LabelAlignType;
        //                aLP.YShift = _labelSet.YOffset;
        //                Graphic aGraphic = new Graphic(aPS, aLP);
        //                AddLabel(aGraphic);                        
        //            }
        //            break;
        //    }            
        //}

        /// <summary>
        /// Add lables of contour layer dynamicly
        /// </summary>                
        /// <param name="sExtent">View extent of MayView</param>        
        public void AddLabelsContourDynamic(Extent sExtent)
        {                    
            int shapeIdx = 0;
            foreach (PolylineShape aPLS in _shapeList)
            {
                Extent IExtent = aPLS.Extent;
                if (IExtent.maxX - IExtent.minX > (sExtent.maxX - sExtent.minX) / 10 ||
                    IExtent.maxY - IExtent.minY > (sExtent.maxY - sExtent.minY) / 10)
                {
                    LabelBreak aLP = new LabelBreak();
                    int pIdx = aPLS.Points.Count / 2;
                    PointF aPoint = new PointF(0, 0);
                    PointShape aPS = new PointShape();
                    aPS.Point = (PointD)aPLS.Points[pIdx - 1];
                    aLP.Text = GetCellValue(_labelSet.FieldName, shapeIdx).ToString();
                    aLP.Font = _labelSet.LabelFont;
                    aLP.AlignType = _labelSet.LabelAlignType;
                    aLP.YShift = _labelSet.YOffset;

                    string vStr;
                    PolyLineBreak aPLB = new PolyLineBreak();
                    switch (LegendScheme.LegendType)
                    {
                        case LegendType.SingleSymbol:
                            aPLB = (PolyLineBreak)LegendScheme.LegendBreaks[0];
                            aLP.Color = aPLB.Color;
                            break;
                        case LegendType.UniqueValue:
                            vStr = GetCellValue(_labelSet.FieldName, shapeIdx).ToString();
                            for (int j = 0; j < LegendScheme.LegendBreaks.Count; j++)
                            {
                                aPLB = (PolyLineBreak)LegendScheme.LegendBreaks[j];
                                if (vStr == aPLB.StartValue.ToString())
                                {
                                    aLP.Color = aPLB.Color;
                                }
                            }
                            break;
                        case LegendType.GraduatedColor:
                            vStr = GetCellValue(_labelSet.FieldName, shapeIdx).ToString();
                            double value = double.Parse(vStr);
                            int blNum = 0;
                            for (int j = 0; j < LegendScheme.LegendBreaks.Count; j++)
                            {
                                aPLB = (PolyLineBreak)LegendScheme.LegendBreaks[j];
                                blNum += 1;
                                if (value == double.Parse(aPLB.StartValue.ToString()) ||
                                    (value > double.Parse(aPLB.StartValue.ToString()) &&
                                    value < double.Parse(aPLB.EndValue.ToString())) ||
                                    (blNum == LegendScheme.LegendBreaks.Count && value == double.Parse(aPLB.EndValue.ToString())))
                                {
                                    aLP.Color = aPLB.Color;
                                }
                            }
                            break;
                    }

                    Graphic aGraphic = new Graphic(aPS, aLP);
                    AddLabel(aGraphic);
                }
                shapeIdx += 1;
            }

            _labelSet.DrawLabels = true;
        }

        /// <summary>
        /// Select labels
        /// </summary>
        /// <param name="aExtent">select extent</param>
        /// <param name="SelectedLabels">ref selected labels</param>
        /// <returns>if selected</returns>
        public bool SelectLabels(Extent aExtent, ref List<int> SelectedLabels)
        {
            SelectedLabels.Clear();            
            Graphic aPoint = new Graphic();
            int i;            

            for (i = 0; i < _labelPoints.Count; i++)
            {
                aPoint = _labelPoints[i];
                if (MIMath.PointInExtent(((PointShape)aPoint.Shape).Point, aExtent))                
                    SelectedLabels.Add(i);                    
            }

            if (SelectedLabels.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Move label to a new position
        /// </summary>
        /// <param name="labStr">label text</param>
        /// <param name="xShift">x shift</param>
        /// <param name="yShift">y shift</param>
        public void MoveLabel(string labStr, float xShift, float yShift)
        {
            for (int i = 0; i < _labelPoints.Count; i++)
            {
                if (((LabelBreak)_labelPoints[i].Legend).Text == labStr)
                {
                    ((LabelBreak)_labelPoints[i].Legend).YShift = yShift;
                    break;
                }
            }
        }

        #endregion

        #region Chart
        /// <summary>
        /// Add a chart point
        /// </summary>
        /// <param name="aCP"></param>
        public void AddChart(Graphic aCP)
        {
            _chartPoints.Add(aCP);
        }

        /// <summary>
        /// Remove all charts
        /// </summary>
        public void RemoveCharts()
        {
            _chartPoints.Clear();
            _chartSet.DrawCharts = false;
        }

        /// <summary>
        /// Add charts
        /// </summary>
        public void AddCharts()
        {
            List<Shape.Shape> shapeList = new List<MeteoInfoC.Shape.Shape>(ShapeList);
            int shapeIdx = -1;
            PointD aPoint = new PointD();           

            List<int> selShapeIdx = GetSelectedShapeIndexes();
            bool isShapeSel = true;
            if (selShapeIdx.Count == 0)
                isShapeSel = false;
            foreach (Shape.Shape aShape in shapeList)
            {
                shapeIdx += 1;
                if (isShapeSel)
                {
                    if (!aShape.Selected)
                        continue;
                }                

                PointShape aPS = new PointShape();
                switch (ShapeType)
                {
                    case ShapeTypes.Point:
                    case ShapeTypes.PointM:
                    case ShapeTypes.PointZ:
                        aPS.Point = ((PointShape)aShape).Point;
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineM:
                    case ShapeTypes.PolylineZ:
                        int pIdx = ((PolylineShape)aShape).Points.Count / 2;
                        aPS.Point = ((PolylineShape)aShape).Points[pIdx - 1];
                        break;
                    case ShapeTypes.Polygon:
                    case ShapeTypes.PolygonM:
                        Extent aExtent = aShape.Extent;
                        aPoint.X = (Single)((aExtent.minX + aExtent.maxX) / 2);
                        aPoint.Y = (Single)((aExtent.minY + aExtent.maxY) / 2);
                        aPS.Point = aPoint;
                        break;
                }

                ChartBreak aCP = new ChartBreak(_chartSet.ChartType);                
                foreach (string fn in _chartSet.FieldNames)
                    aCP.ChartData.Add(float.Parse(GetCellValue(fn, shapeIdx).ToString()));
                aCP.XShift = _chartSet.XShift;
                aCP.YShift = _chartSet.YShift;
                aCP.LegendScheme = _chartSet.LegendScheme;
                aCP.MinSize = _chartSet.MinSize;
                aCP.MaxSize = _chartSet.MaxSize;
                aCP.MinValue = _chartSet.MinValue;
                aCP.MaxValue = _chartSet.MaxValue;
                aCP.BarWidth = _chartSet.BarWidth;
                aCP.AlignType = _chartSet.AlignType;
                aCP.View3D = _chartSet.View3D;
                aCP.Thickness = _chartSet.Thickness;
                aCP.ShapeIndex = shapeIdx;
                
                Graphic aGraphic = new Graphic(aPS, aCP);
                AddChart(aGraphic);
            }
            _chartSet.DrawCharts = true;
        }

        /// <summary>
        /// Update charts properties
        /// </summary>
        public void UpdateChartsProp()
        {
            List<Shape.Shape> shapeList = new List<MeteoInfoC.Shape.Shape>(ShapeList);

            foreach (Graphic chartG in _chartPoints)
            {
                ChartBreak aCP = (ChartBreak)chartG.Legend;
                aCP.LegendScheme = _chartSet.LegendScheme;
                aCP.MinSize = _chartSet.MinSize;
                aCP.MaxSize = _chartSet.MaxSize;
                aCP.MinValue = _chartSet.MinValue;
                aCP.MaxValue = _chartSet.MaxValue;
                aCP.BarWidth = _chartSet.BarWidth;
                aCP.AlignType = _chartSet.AlignType;
                aCP.View3D = _chartSet.View3D;
                aCP.Thickness = _chartSet.Thickness;
            }
        }

        #endregion

        #region Shape

        ///// <summary>
        ///// Select shapes
        ///// </summary>
        ///// <param name="aExtent">Extent</param>
        ///// <param name="SelectedShapes">Selected shapes</param>
        ///// <returns>If selected</returns>
        //public bool SelectShapes(Extent aExtent, ref List<int> SelectedShapes)
        //{
        //    SelectedShapes.Clear();            
        //    int i, j;
        //    PointD aPoint = new PointD();
        //    aPoint.X = (aExtent.minX + aExtent.maxX) / 2;
        //    aPoint.Y = (aExtent.minY + aExtent.maxY) / 2;

        //    switch (LayerDrawType)
        //    {
        //        case LayerDrawType.Barb:
        //            for (i = 0; i < ShapeList.Count; i++)
        //            {
        //                WindBarb aWB = (WindBarb)ShapeList[i];
        //                if (MIMath.PointInExtent(aWB.Point, aExtent))
        //                {
        //                    SelectedShapes.Add(i);
        //                }
        //            }
        //            break;
        //        case LayerDrawType.StationModel:
        //            for (i = 0; i < ShapeList.Count; i++)
        //            {
        //                StationModelShape aSM = (StationModelShape)ShapeList[i];
        //                if (MIMath.PointInExtent(aSM.Point, aExtent))
        //                {
        //                    SelectedShapes.Add(i);
        //                }
        //            }
        //            break;
        //        case LayerDrawType.Vector:
        //            for (i = 0; i < ShapeList.Count; i++)
        //            {
        //                WindArraw aArraw = (WindArraw)ShapeList[i];
        //                if (MIMath.PointInExtent(aArraw.Point, aExtent))
        //                {
        //                    SelectedShapes.Add(i);
        //                }
        //            }
        //            break;
        //        case LayerDrawType.WeatherSymbol:
        //            for (i = 0; i < ShapeList.Count; i++)
        //            {
        //                WeatherSymbol aWS = (WeatherSymbol)ShapeList[i];
        //                if (MIMath.PointInExtent(aWS.Point, aExtent))
        //                {
        //                    SelectedShapes.Add(i);
        //                }
        //            }
        //            break;
        //        default:
        //            switch (ShapeType)
        //            {
        //                case ShapeTypes.Point:
        //                case ShapeTypes.PointM:
        //                case ShapeTypes.PointZ:
        //                    for (i = 0; i < ShapeList.Count; i++)
        //                    {
        //                        PointShape aPS = (PointShape)ShapeList[i];
        //                        if (MIMath.PointInExtent(aPS.Point, aExtent))
        //                        {
        //                            SelectedShapes.Add(i);
        //                        }
        //                    }
        //                    break;
        //                case ShapeTypes.Polyline:
        //                case ShapeTypes.PolylineM:
        //                case ShapeTypes.PolylineZ:
        //                    for (i = 0; i < ShapeList.Count; i++)
        //                    {
        //                        PolylineShape aPLS = (PolylineShape)ShapeList[i];
        //                        if (MIMath.IsExtentCross(aExtent, aPLS.Extent))
        //                        {
        //                            for (j = 0; j < aPLS.Points.Count; j++)
        //                            {
        //                                aPoint = aPLS.Points[j];
        //                                if (MIMath.PointInExtent(aPoint, aExtent))
        //                                {
        //                                    SelectedShapes.Add(i);
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    break;
        //                case ShapeTypes.Polygon:    
        //                case ShapeTypes.PolygonM:                       
        //                    for (i = ShapeList.Count - 1; i >= 0; i--)
        //                    {
        //                        PolygonShape aPGS = (PolygonShape)ShapeList[i];
        //                        if (GeoComputation.PointInPolygon(aPGS, aPoint))
        //                            SelectedShapes.Add(i);

        //                        //if (!(aPGS.numParts > 1))
        //                        //{
        //                        //    if (MIMath.PointInPolygon(aPGS.Points, aPoint))
        //                        //    {
        //                        //        SelectedShapes.Add(i);
        //                        //    }
        //                        //}
        //                        //else
        //                        //{
        //                        //    for (int p = 0; p < aPGS.numParts; p++)
        //                        //    {
        //                        //        ArrayList pList = new ArrayList();
        //                        //        if (p == aPGS.numParts - 1)
        //                        //        {
        //                        //            for (int pp = aPGS.parts[p]; pp < aPGS.numPoints; pp++)
        //                        //            {
        //                        //                pList.Add(aPGS.Points[pp]);
        //                        //            }
        //                        //        }
        //                        //        else
        //                        //        {
        //                        //            for (int pp = aPGS.parts[p]; pp < aPGS.parts[p + 1]; pp++)
        //                        //            {
        //                        //                pList.Add(aPGS.Points[pp]);
        //                        //            }
        //                        //        }
        //                        //        if (MIMath.PointInPolygon(pList, aPoint))
        //                        //        {
        //                        //            SelectedShapes.Add(i);
        //                        //            break;
        //                        //        }
        //                        //    }
        //                        //}
        //                    }
        //                    break;
        //            }
        //        break;
        //    }            

        //    if (SelectedShapes.Count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// Select shapes
        /// </summary>
        /// <param name="aExtent">Extent</param>
        /// <param name="isSingleSel">If only select one shape</param>
        /// <returns>Slected shapes</returns>
        public List<int> SelectShapes(Extent aExtent, bool isSingleSel)
        {
            int i, j;
            List<int> selectedShapes = new List<int>();
            PointD sp = aExtent.GetCenterPoint();

            switch (ShapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes.PointM:
                case ShapeTypes.PointZ:
                case ShapeTypes.WindArraw:
                case ShapeTypes.WindBarb:
                case ShapeTypes.WeatherSymbol:
                case ShapeTypes.StationModel:
                    for (i = 0; i < ShapeList.Count; i++)
                    {
                        PointShape aPS = (PointShape)ShapeList[i];
                        if (MIMath.PointInExtent(aPS.Point, aExtent))
                        {
                            selectedShapes.Add(i);
                            if (isSingleSel)
                                break;
                        }
                    }
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineM:
                case ShapeTypes.PolylineZ:
                    for (i = 0; i < ShapeList.Count; i++)
                    {
                        PolylineShape aPLS = (PolylineShape)ShapeList[i];
                        bool isSelected = false;
                        if (MIMath.IsExtentCross(aExtent, aPLS.Extent))
                        {
                            for (j = 0; j < aPLS.Points.Count; j++)
                            {
                                PointD aPoint = aPLS.Points[j];
                                if (MIMath.PointInExtent(aPoint, aExtent))
                                {
                                    selectedShapes.Add(i);
                                    isSelected = true;
                                    break;
                                }

                                if (j < aPLS.PointNum - 1)
                                {
                                    PointD bPoint = aPLS.Points[j + 1];
                                    if (Math.Abs(sp.Y - aPoint.Y) <= Math.Abs(bPoint.Y - aPoint.Y)
                                            && Math.Abs(sp.X - aPoint.X) <= Math.Abs(bPoint.X - aPoint.X))
                                    {
                                        if (GeoComputation.dis_PointToLine(sp, aPoint, bPoint) < aExtent.Width)
                                        {
                                            selectedShapes.Add(i);
                                            isSelected = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (isSingleSel && isSelected)
                            break;
                    }
                    break;
                case ShapeTypes.Polygon:
                case ShapeTypes.PolygonM:
                    for (i = ShapeList.Count - 1; i >= 0; i--)
                    {
                        PolygonShape aPGS = (PolygonShape)ShapeList[i];
                        if (isSingleSel)
                        {
                            if (GeoComputation.PointInPolygon(aPGS, sp))
                            {
                                selectedShapes.Add(i);
                                break;
                            }
                        }
                        else
                        {
                            if (MIMath.IsExtentCross(aExtent, aPGS.Extent))
                            {
                                for (j = 0; j < aPGS.Polygons[0].OutLine.Count; j++)
                                {
                                    if (MIMath.PointInExtent(aPGS.Polygons[0].OutLine[j], aExtent))
                                    {
                                        selectedShapes.Add(i);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

            return selectedShapes;
        }

        /// <summary>
        /// Select shapes by a polygon shape
        /// </summary>
        /// <param name="polygonShape">The polygon shape</param>
        /// <returns>Selected shape indexes</returns>
        public List<int> SelectShapes(PolygonShape polygonShape)
        {
            List<int> selIdxs = new List<int>();
            for (int i = 0; i < _shapeList.Count; i++)
            {
                bool isIn = false;
                List<PointD> points = _shapeList[i].GetPoints();
                foreach (PointD aPoint in points)
                {
                    if (GeoComputation.PointInPolygon(polygonShape, aPoint))
                    {
                        isIn = true;
                        break;
                    }
                }

                if (isIn)
                {
                    _shapeList[i].Selected = true;
                    selIdxs.Add(i);
                }
            }

            return selIdxs;
        }

        /// <summary>
        /// Get if has selected shape
        /// </summary>
        /// <returns>Boolean</returns>
        public bool HasSelectedShapes()
        {
            foreach (Shape.Shape shape in _shapeList)
            {
                if (shape.Selected)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Get selected shape indexes
        /// </summary>
        /// <returns>indexes</returns>
        public List<int> GetSelectedShapeIndexes()
        {
            List<int> selIndexes = new List<int>();
            for (int i = 0; i < ShapeNum; i++)
            {
                if (_shapeList[i].Selected)
                    selIndexes.Add(i);
            }

            return selIndexes;
        }

        /// <summary>
        /// Clear selected shapes
        /// </summary>
        public void ClearSelectedShapes()
        {
            foreach (Shape.Shape aShape in _shapeList)
            {
                if (aShape.Selected)
                    aShape.Selected = false;
            }
        }

        #endregion

        #region Edit
        /// <summary>
        /// Edit: Add shape
        /// </summary>
        /// <param name="aShape">shape</param>        
        /// <returns>ifsuccess</returns>
        public bool EditAddShape(Shape.Shape aShape)
        {
            //if (aShape.GetType() == ShapeType.GetType())
            //{
                _shapeList.Add(aShape);                
                AddRecord();
                UpdateLayerExtent(aShape);

                return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        private void UpdateLayerExtent(object aShape)
        {
            if (ShapeNum == 1)
            {
                switch (ShapeType)
                {
                    case ShapeTypes.Point:
                    case ShapeTypes.PointM:
                    case ShapeTypes.PointZ:
                    case ShapeTypes.WindArraw:
                    case ShapeTypes.WindBarb:
                    case ShapeTypes.WeatherSymbol:
                    case ShapeTypes.StationModel:
                        Extent aExtent = new Extent();
                        aExtent.minX = ((PointShape)aShape).Point.X;
                        aExtent.maxX = ((PointShape)aShape).Point.X;
                        aExtent.minY = ((PointShape)aShape).Point.Y;
                        aExtent.maxY = ((PointShape)aShape).Point.Y;
                        Extent = aExtent;
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineM:
                        Extent = ((PolylineShape)aShape).Extent;
                        break;
                    case ShapeTypes.PolylineZ :
                        Extent = ((PolylineZShape)aShape).Extent;
                        break;
                    case ShapeTypes.Polygon:
                    case ShapeTypes.PolygonM:
                        Extent = ((PolygonShape)aShape).Extent;
                        break;
                }
            }
            else
            {                
                switch (ShapeType)
                {
                    case ShapeTypes.Point:
                    case ShapeTypes.PointM:
                    case ShapeTypes.PointZ:
                    case ShapeTypes.WindArraw:
                    case ShapeTypes.WindBarb:
                    case ShapeTypes.WeatherSymbol:
                    case ShapeTypes.StationModel:
                        Extent aExtent = new Extent();
                        aExtent.minX = ((PointShape)aShape).Point.X;
                        aExtent.maxX = ((PointShape)aShape).Point.X;
                        aExtent.minY = ((PointShape)aShape).Point.Y;
                        aExtent.maxY = ((PointShape)aShape).Point.Y;
                        Extent = MIMath.GetLagerExtent(Extent, aExtent);
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineM:
                        Extent = MIMath.GetLagerExtent(Extent, ((PolylineShape)aShape).Extent);
                        break;
                    case ShapeTypes.PolylineZ:
                        Extent = MIMath.GetLagerExtent(Extent, ((PolylineZShape)aShape).Extent);
                        break;
                    case ShapeTypes.Polygon:
                    case ShapeTypes.PolygonM:
                        Extent = MIMath.GetLagerExtent(Extent, ((PolygonShape)aShape).Extent);
                        break;
                }
            }
        }

        private void UpdateLayerExtent()
        {
            if (ShapeNum == 1)
            {
                switch (ShapeType)
                {
                    case ShapeTypes.Point:
                    case ShapeTypes.PointM:
                    case ShapeTypes.PointZ:
                    case ShapeTypes.WindArraw:
                    case ShapeTypes.WindBarb:
                    case ShapeTypes.WeatherSymbol:
                    case ShapeTypes.StationModel:
                        Extent aExtent = new Extent();
                        aExtent.minX = ((PointShape)_shapeList[0]).Point.X;
                        aExtent.maxX = ((PointShape)_shapeList[0]).Point.X;
                        aExtent.minY = ((PointShape)_shapeList[0]).Point.Y;
                        aExtent.maxY = ((PointShape)_shapeList[0]).Point.Y;
                        Extent = aExtent;
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineM:
                    case ShapeTypes.PolylineZ:
                        Extent = ((PolylineShape)_shapeList[0]).Extent;
                        break;
                    case ShapeTypes.Polygon:
                    case ShapeTypes.PolygonM:
                        Extent = ((PolygonShape)_shapeList[0]).Extent;
                        break;
                }
            }
            else
            {                
                int i;
                switch (ShapeType)
                {
                    case ShapeTypes.Point:
                    case ShapeTypes.PointM:
                    case ShapeTypes.PointZ:
                        ArrayList pList = new ArrayList();
                        for (i = 0; i < ShapeNum; i++)
                        {
                            pList.Add(((PointShape)_shapeList[i]).Point);
                        }
                        Extent aExtent = new Extent();
                        aExtent = MIMath.GetPointsExtent(pList);
                        Extent = aExtent;
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineM:
                    case ShapeTypes.PolylineZ:
                        for (i = 0; i < ShapeNum; i++)
                        {
                            if (i == 0)
                            {
                                Extent = ((PolylineShape)_shapeList[i]).Extent;
                            }
                            else
                            {
                                Extent = MIMath.GetLagerExtent(Extent, ((PolylineShape)_shapeList[i]).Extent);
                            }
                        }                        
                        break;
                    case ShapeTypes.Polygon:
                    case ShapeTypes.PolygonM:
                        for (i = 0; i < ShapeNum; i++)
                        {
                            if (i == 0)
                            {
                                Extent = ((PolygonShape)_shapeList[i]).Extent;
                            }
                            else
                            {
                                Extent = MIMath.GetLagerExtent(Extent, ((PolygonShape)_shapeList[i]).Extent);
                            }
                        }  
                        break;
                }
            }
        }


        /// <summary>
        /// Edit: Insert shape
        /// </summary>
        /// <param name="aShape">shape</param>
        /// <param name="position">position</param>
        /// <returns>ifsuccess</returns>
        public bool EditInsertShape(Shape.Shape aShape, int position)
        {
            //if (aShape.GetType() == ShapeType.GetType())
            //{
                _shapeList.Insert(position, aShape);                
                InsertRecord(position);
                UpdateLayerExtent(aShape);

                return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        /// <summary>
        /// Edit: Delete shape
        /// </summary>
        /// <param name="position">position</param>
        public void EditDeleteShape(int position)
        {
            _shapeList.RemoveAt(position);            
            DeleteRecord(position);
            UpdateLayerExtent();
        }

        /// <summary>
        /// Edit: Replace a shape
        /// </summary>
        /// <param name="position">postion</param>
        /// <param name="aShape">shape</param>
        public void EditReplaceShape(int position, Shape.Shape aShape)
        {
            _shapeList[position] = aShape;
        }

        private void AddRecord()
        {
            DataRow aDR = _attributeTable.Table.NewRow();
            _attributeTable.Table.Rows.Add(aDR);
        }
        
        private void InsertRecord(int position)
        {
            DataRow aDR = _attributeTable.Table.NewRow();
            _attributeTable.Table.Rows.InsertAt(aDR, position);
        }

        private void DeleteRecord(int position)
        {
            _attributeTable.Table.Rows.RemoveAt(position);
        }

        /// <summary>
        /// Add field
        /// </summary>
        /// <param name="aField">field</param>
        public void EditAddField(DataColumn aField)
        {
            for (int i = 0; i < _numFields; i++)
            {
                if (aField.ColumnName == _attributeTable.Table.Columns[i].ColumnName)
                    aField.ColumnName = aField.ColumnName + "_1";
            }
            _attributeTable.Table.Columns.Add(aField);
            _numFields = _attributeTable.Table.Columns.Count;            
        }

        /// <summary>
        /// Edit: Add field
        /// </summary>
        /// <param name="fieldName">field name</param>
        /// <param name="fieldType">field type</param>
        public void EditAddField(string fieldName, Type fieldType)
        {
            DataColumn aField = new DataColumn(fieldName, fieldType);
            EditAddField(aField);
        }

        /// <summary>
        /// Edit: Insert field
        /// </summary>
        /// <param name="aField">field</param>
        /// <param name="position">position</param>
        public void EditInsertField(DataColumn aField, int position)
        {
            for (int i = 0; i < _numFields; i++)
            {
                if (aField.ColumnName == _attributeTable.Table.Columns[i].ColumnName)
                    aField.ColumnName = aField.ColumnName + "_1";
            }
            _attributeTable.Table.Columns.Add(aField);
            _numFields = _attributeTable.Table.Columns.Count;
            if (position < _numFields)
                _attributeTable.Table.Columns[aField.ColumnName].SetOrdinal(position);
        }

        /// <summary>
        /// Edit: Insert field
        /// </summary>
        /// <param name="fieldName">field name</param>
        /// <param name="fieldType">field type</param>
        /// <param name="position">position</param>
        public void EditInsertField(string fieldName, Type fieldType, int position)
        {
            DataColumn aField = new DataColumn(fieldName, fieldType);
            EditInsertField(aField, position);
        }

        /// <summary>
        /// Edit: Delete a field by index
        /// </summary>
        /// <param name="fieldIndex">field index</param>
        public void EditDeleteField(int fieldIndex)
        {
            _attributeTable.Table.Columns.RemoveAt(fieldIndex);
            _numFields = _attributeTable.Table.Columns.Count;
        }

        /// <summary>
        /// Edit: Delete a field by name
        /// </summary>
        /// <param name="fieldName">field name</param>
        public void EditDeleteField(string fieldName)
        {
            _attributeTable.Table.Columns.Remove(fieldName);
            _numFields = _attributeTable.Table.Columns.Count;
        }

        /// <summary>
        /// Edit: Rename field
        /// </summary>
        /// <param name="oldName">old field name</param>
        /// <param name="newName">new field name</param>
        public void EditRenameField(string oldName, string newName)
        {
            _attributeTable.Table.Columns[oldName].ColumnName = newName;
        }

        /// <summary>
        /// Save attribute table
        /// </summary>
        public void SaveAttributeTable()
        {
            _attributeTable.Save();
        }

        /// <summary>
        /// Save layer as a shape file
        /// </summary>
        public void SaveFile()
        {           
            if (File.Exists(FileName))
            {
                ShapeFileManage.SaveShapeFile(FileName, this);
            }
            else
            {
                SaveFileDialog saveDlg = new SaveFileDialog();                
                saveDlg.Filter = "shp file (*.shp)|*.shp";
                if (saveDlg.ShowDialog() == DialogResult.OK)
                {
                    FileName = saveDlg.FileName;
                    if (System.IO.File.Exists(FileName))
                    {
                        string lPathNoExt = System.IO.Path.GetDirectoryName(FileName) + @"\" + System.IO.Path.GetFileNameWithoutExtension(FileName);
                        System.IO.File.Delete(lPathNoExt + ".shp");
                        System.IO.File.Delete(lPathNoExt + ".shx");
                        System.IO.File.Delete(lPathNoExt + ".dbf");
                        System.IO.File.Delete(lPathNoExt + ".prj");
                    }
                    ShapeFileManage.SaveShapeFile(FileName, this);
                }
            }
        }

        /// <summary>
        /// Save layer as a shape file
        /// </summary>
        public void SaveFile(string aFile)
        {
            FileName = aFile;           
            if (System.IO.File.Exists(FileName))
            {
                string lPathNoExt = System.IO.Path.GetDirectoryName(FileName) + @"\" + 
                    System.IO.Path.GetFileNameWithoutExtension(FileName);
                System.IO.File.Delete(lPathNoExt + ".shp");
                System.IO.File.Delete(lPathNoExt + ".shx");
                System.IO.File.Delete(lPathNoExt + ".dbf");
                System.IO.File.Delete(lPathNoExt + ".prj");
            }
            ShapeFileManage.SaveShapeFile(FileName, this);
        }

        /// <summary>
        /// Clip the layer by a clipping layer
        /// </summary>
        /// <param name="clipLayer">Clipping layer</param>
        /// <param name="onlySel">If only using selected shapes in clipping layer</param>
        /// <returns>Clipped result layer</returns>
        public VectorLayer Clip(VectorLayer clipLayer, bool onlySel)
        {
            List<Shape.Shape> shapes = new List<Shape.Shape>();
            if (onlySel)
            {
                foreach (Shape.Shape aShape in clipLayer.ShapeList)
                {
                    if (aShape.Selected)
                        shapes.Add(aShape);
                }
            }
            else
                shapes = clipLayer.ShapeList;

            VectorLayer newLayer = (VectorLayer)this.Clone();
            newLayer.AttributeTable.Table = new DataTable();
            foreach (DataColumn aDC in this.AttributeTable.Table.Columns)
            {
                DataColumn bDC = new DataColumn();
                bDC.ColumnName = aDC.ColumnName;
                bDC.DataType = aDC.DataType;
                newLayer.EditAddField(bDC);
            }

            newLayer.ShapeList = new List<Shape.Shape>();
            foreach (Shape.Shape aShape in shapes)
            {
                PolygonShape aPGS = (PolygonShape)aShape;
                int i = 0;
                foreach (Shape.Shape bShape in this.ShapeList)
                {
                    DataRow aDR = this.AttributeTable.Table.Rows[i];
                    foreach (Polygon aPolygon in aPGS.Polygons)
                    {
                        Shape.Shape clipShape = GeoComputation.ClipShape(bShape, aPolygon.OutLine);
                        if (clipShape != null)
                        {
                            newLayer.ShapeList.Add(clipShape);
                            newLayer.AttributeTable.Table.Rows.Add(aDR.ItemArray);
                        }
                    }

                    i++;
                }
            }

            return newLayer;
        }

        #endregion

        #region Projection
        /// <summary>
        /// Update data to origion set
        /// </summary>
        public void UpdateOriginData()
        {
            _originAttributeTable = _attributeTable.Clone();
            //_originShapes = new List<MeteoInfoC.Shape.Shape>(_shapeList);
            _originShapes = new List<MeteoInfoC.Shape.Shape>();
            foreach (Shape.Shape aShape in _shapeList)
                _originShapes.Add((Shape.Shape)aShape.Clone());

            _originLabelPoints = new List<Graphic>(_labelPoints);
            _originChartPoints = new List<Graphic>(_chartPoints);
            _isProjected = true;
        }

        /// <summary>
        /// Get origin data
        /// </summary>
        public void GetOriginData()
        {
            _attributeTable = _originAttributeTable.Clone();
            //_shapeList = _originShapes;
            _shapeList = new List<MeteoInfoC.Shape.Shape>();
            foreach (Shape.Shape aShape in _originShapes)
                _shapeList.Add((Shape.Shape)aShape.Clone());

            _labelPoints = _originLabelPoints;
            _chartPoints = _originChartPoints;
            UpdateExtent();
        }

        /// <summary>
        /// Get origin shapes
        /// </summary>
        /// <returns>Origin shapes</returns>
        public List<Shape.Shape> GetOriginShapes()
        {
            return _originShapes;
        }

        /// <summary>
        /// Get origin attribute table
        /// </summary>
        /// <returns>Origin attribute table</returns>
        public AttributeTable GetOriginAttTable()
        {
            return _originAttributeTable;
        }

        #endregion

        #region Other
        /// <summary>
        /// Update legend scheme
        /// </summary>
        /// <param name="aLT">legend type</param>
        /// <param name="fieldName">field name</param>
        public void UpdateLegendScheme(LegendType aLT, string fieldName)
        {
            this.LegendScheme = CreateLegendScheme(aLT, fieldName);
        }
        
        /// <summary>
        /// Update legend scheme -> update 
        /// </summary>
        public void UpdateLegendIndexes()
        {
            switch (LegendScheme.LegendType)
            {
                case LegendType.UniqueValue:
                    int shapeIdx = 0;
                    foreach (Shape.Shape aShape in ((VectorLayer)this).ShapeList)
                    {
                        aShape.LegendIndex = -1;
                        string vStr = ((VectorLayer)this).GetCellValue(LegendScheme.FieldName, shapeIdx).ToString();
                        for (int i = 0; i < LegendScheme.LegendBreaks.Count; i++)
                        {
                            if (vStr == LegendScheme.LegendBreaks[i].StartValue.ToString())
                                aShape.LegendIndex = i;
                        }
                        shapeIdx += 1;
                    }
                    break;
                case LegendType.GraduatedColor:
                    shapeIdx = 0;
                    foreach (Shape.Shape aShape in ((VectorLayer)this).ShapeList)
                    {
                        aShape.LegendIndex = -1;
                        string vStr = ((VectorLayer)this).GetCellValue(LegendScheme.FieldName, shapeIdx).ToString();
                        double v = double.Parse(vStr);
                        int blNum = 0;
                        for (int i = 0; i < LegendScheme.LegendBreaks.Count; i++)
                        {
                            ColorBreak cb = LegendScheme.LegendBreaks[i];
                            blNum += 1;
                            if (MIMath.DoubleEquals(v, double.Parse(cb.StartValue.ToString())) ||
                                (v > double.Parse(cb.StartValue.ToString())
                                && v < double.Parse(cb.EndValue.ToString())) ||
                                (blNum == LegendScheme.LegendBreaks.Count && v == double.Parse(cb.EndValue.ToString())))
                            {
                                aShape.LegendIndex = i;
                            }
                        }
                        shapeIdx += 1;
                    }
                    break;
                default:
                    foreach (Shape.Shape aShape in ((VectorLayer)this).ShapeList)
                        aShape.LegendIndex = 0;
                    break;
            }
        }

        /// <summary>
        /// Create legend scheme
        /// </summary>
        /// <param name="aLT">legend type</param>
        /// <param name="fieldName">field name</param>
        /// <returns>legend scheme</returns>
        public LegendScheme CreateLegendScheme(LegendType aLT, string fieldName)
        {
            double min, max;
            ShapeTypes aST = this.ShapeType;
            LegendScheme aLS = new LegendScheme(this.ShapeType);

            min = aLS.MinValue;
            max = aLS.MaxValue;
            switch (aLT)
            {
                case LegendType.SingleSymbol:
                    Color aColor = Color.Black;
                    Single size = 0.1F;
                    switch (aST)
                    {
                        case ShapeTypes.Point:
                        case ShapeTypes.PointM:
                        case ShapeTypes.PointZ:
                            aColor = Color.Black;
                            size = 5;
                            break;
                        case ShapeTypes.Polyline:
                        case ShapeTypes.PolylineM:
                        case ShapeTypes.PolylineZ:
                            aColor = Color.Black;
                            break;
                        case ShapeTypes.Polygon:
                        case ShapeTypes.PolygonM:
                        case ShapeTypes.Image:
                            aColor = Color.FromArgb(255, 251, 195);
                            break;
                    }

                    aLS = LegendManage.CreateSingleSymbolLegendScheme(aST, aColor, size);
                    break;
                case LegendType.UniqueValue:
                    Color[] colors;
                    List<string> valueList = new List<string>();
                    bool isDateField = false;
                    Type colType = this.AttributeTable.Table.Columns[fieldName].DataType;
                    if (colType == typeof(DateTime))
                        isDateField = true;

                    List<string> captions = new List<string>();

                    for (int i = 0; i < this.AttributeTable.Table.Rows.Count; i++)
                    {
                        if (!valueList.Contains(this.AttributeTable.Table.Rows[i][fieldName].ToString()))
                        {
                            valueList.Add(this.AttributeTable.Table.Rows[i][fieldName].ToString());
                            if (isDateField)
                                captions.Add(((DateTime)this.AttributeTable.Table.Rows[i][fieldName]).ToString("yyyy/M/d"));
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
                        aLS = LegendManage.CreateUniqValueLegendScheme(valueList, captions, colors, aST, min,
                            max, aLS.HasNoData, aLS.MissingValue);
                    else
                        aLS = LegendManage.CreateUniqValueLegendScheme(valueList, colors,
                            aST, min, max, aLS.HasNoData, aLS.MissingValue);

                    aLS.FieldName = fieldName;
                    break;
                case LegendType.GraduatedColor:
                    double[] S = new double[this.AttributeTable.Table.Rows.Count];
                    for (int i = 0; i < S.Length; i++)
                    {
                        S[i] = double.Parse(this.AttributeTable.Table.Rows[i][fieldName].ToString());
                    }
                    MIMath.GetMaxMinValue(S, aLS.MissingValue, ref min, ref max);


                    if (min == max)
                    {
                        MessageBox.Show("The values of all shapes are same!", "Alarm");
                        break;
                    }

                    double[] CValues;
                    CValues = LegendManage.CreateContourValues(min, max);
                    colors = LegendManage.CreateRainBowColors(CValues.Length + 1);

                    aLS = LegendManage.CreateGraduatedLegendScheme(CValues, colors,
                        aST, min, max, aLS.HasNoData, aLS.MissingValue);
                    aLS.FieldName = fieldName;
                    break;
            }

            return aLS;
        }

        /// <summary>
        /// Update extent
        /// </summary>
        public void UpdateExtent()
        {
            for (int i = 0; i < _shapeList.Count; i++)
            {
                if (i == 0)
                    Extent = _shapeList[i].Extent;
                else
                    Extent = MIMath.GetLagerExtent(Extent, _shapeList[i].Extent);
            }
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>object</returns>
        public override object Clone()
        {
            VectorLayer aLayer = new VectorLayer(ShapeType);            
            aLayer.Extent = Extent;
            aLayer.FileName = FileName;
            aLayer.Handle = Handle;
            aLayer.LayerName = LayerName;
            aLayer.ProjInfo = ProjInfo;
            aLayer.LegendScheme = LegendScheme;
            if (_isProjected)
            {
                for (int i = 0; i < _originShapes.Count; i++)
                    aLayer.ShapeList.Add((Shape.Shape)_originShapes[i].Clone());
            }
            else
            {
                for (int i = 0; i < _shapeList.Count; i++)
                    aLayer.ShapeList.Add((Shape.Shape)_shapeList[i].Clone());
            }
            aLayer.TransparencyPerc = TransparencyPerc;
            aLayer.LayerDrawType = LayerDrawType;
            aLayer.Visible = Visible;
            aLayer.LabelSet = _labelSet;
            aLayer.Expanded = Expanded;
            aLayer.AvoidCollision = AvoidCollision;
            aLayer.IsMaskout = IsMaskout;
            aLayer.Tag = Tag;

            if (_isProjected)
                aLayer.AttributeTable = _originAttributeTable.Clone();
            else
                aLayer.AttributeTable = _attributeTable.Clone();

            return aLayer;
        }

        /// <summary>
        /// Override get custom property method
        /// </summary>
        /// <returns>property object</returns>
        public override object GetPropertyObject()
        {
            CustomProperty cp = (CustomProperty)base.GetPropertyObject();

            Dictionary<string, string> objAtt = cp.ObjectAttribs;
            objAtt.Add("AvoidCollision", "AvoidCollision");
            objAtt.Add("ShapeNum", "ShapeNum");

            return new CustomProperty(this, objAtt);
        }

        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (_shapeList != null)
            {
                _shapeList.Clear();
                _shapeList = null;
            }
            if (_originShapes != null)
            {
                _originShapes.Clear();
                _originShapes = null;
            }
            if (_attributeTable != null)
            {
                _attributeTable.Table.Dispose();
                _attributeTable = null;
            }
            if (_originAttributeTable != null)
            {
                _originAttributeTable.Table.Dispose();
                _originAttributeTable = null;
            }
            if (_chartPoints != null)
            {
                _chartPoints.Clear();
                _chartPoints = null;
            }
            if (_labelPoints != null)
            {
                _labelPoints.Clear();
                _labelPoints = null;
            }
            if (_originChartPoints != null)
            {
                _originChartPoints.Clear();
                _originChartPoints = null;
            }
            if (_originLabelPoints != null)
            {
                _originLabelPoints.Clear();
                _originLabelPoints = null;
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Convert
        /// <summary>
        /// Save as KML (Google Earth data format) file
        /// <param name="fileName">KML file name</param>
        /// </summary>
        public void SaveAsKMLFile_XML(string fileName)
        {
            // Create document
            XmlDocument xDoc = new XmlDocument();
            XmlDeclaration xDec = xDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            XmlElement rootNode = xDoc.CreateElement("kml");
            rootNode.SetAttribute("xmlns", @"http://www.opengis.net/kml/2.2");
            xDoc.InsertBefore(xDec, xDoc.DocumentElement);
            xDoc.AppendChild(rootNode);
            XmlElement docNode = xDoc.CreateElement("Document");
            rootNode.AppendChild(docNode);

            XmlElement nameNodeMain = xDoc.CreateElement("Name");
            XmlText nameTextMain = xDoc.CreateTextNode(fileName);
            docNode.AppendChild(nameNodeMain);
            nameNodeMain.AppendChild(nameTextMain);

            // Write styles
            int styleNum = 0;
            foreach (ColorBreak cb in this.LegendScheme.LegendBreaks)
            {
                // StyleMap
                XmlElement styleMapElement = xDoc.CreateElement("StyleMap");
                styleMapElement.SetAttribute("id", @"pg" + styleNum.ToString());
                docNode.AppendChild(styleMapElement);
                XmlElement pairElement = xDoc.CreateElement("Pair");
                styleMapElement.AppendChild(pairElement);
                XmlElement keyElement = xDoc.CreateElement("key");
                XmlText keyText = xDoc.CreateTextNode("normal");
                keyElement.AppendChild(keyText);
                pairElement.AppendChild(keyElement);
                XmlElement styleUrlElement = xDoc.CreateElement("styleUrl");
                XmlText styleUrlText = xDoc.CreateTextNode("#pgn" + styleNum.ToString());
                styleUrlElement.AppendChild(styleUrlText);
                pairElement.AppendChild(styleUrlElement);

                pairElement = xDoc.CreateElement("Pair");
                styleMapElement.AppendChild(pairElement);
                keyElement = xDoc.CreateElement("key");
                keyText = xDoc.CreateTextNode("highlight");
                keyElement.AppendChild(keyText);
                pairElement.AppendChild(keyElement);
                styleUrlElement = xDoc.CreateElement("styleUrl");
                styleUrlText = xDoc.CreateTextNode("#pgn");
                styleUrlElement.AppendChild(styleUrlText);
                pairElement.AppendChild(styleUrlElement);

                // Normal style
                PolygonBreak pgb = (PolygonBreak)cb;
                XmlElement styleElement = xDoc.CreateElement("Style");
                styleElement.SetAttribute("id", @"pgn" + styleNum.ToString());
                docNode.AppendChild(styleElement);
                XmlElement lineStyleElement = xDoc.CreateElement("LineStyle");
                styleElement.AppendChild(lineStyleElement);
                XmlElement colorElement = xDoc.CreateElement("color");
                XmlText colorText = xDoc.CreateTextNode(ColorUtils.ToKMLColor(pgb.OutlineColor));
                colorElement.AppendChild(colorText);
                lineStyleElement.AppendChild(colorElement);
                XmlElement polyStyleElement = xDoc.CreateElement("PolyStyle");
                styleElement.AppendChild(polyStyleElement);
                colorElement = xDoc.CreateElement("color");
                colorText = xDoc.CreateTextNode(ColorUtils.ToKMLColor(pgb.Color));
                colorElement.AppendChild(colorText);
                polyStyleElement.AppendChild(colorElement);
                XmlElement fillElement = xDoc.CreateElement("fill");
                string fillstr = "1";
                if (!pgb.DrawFill)
                    fillstr = "0";
                XmlText fillText = xDoc.CreateTextNode(fillstr);
                fillElement.AppendChild(fillText);
                polyStyleElement.AppendChild(fillElement);

                styleNum += 1;
            }

            // Highlight style - shared by all elements
            XmlElement styleElement1 = xDoc.CreateElement("Style");
            styleElement1.SetAttribute("id", @"pgh");
            docNode.AppendChild(styleElement1);
            XmlElement lineStyleElement1 = xDoc.CreateElement("LineStyle");
            styleElement1.AppendChild(lineStyleElement1);
            XmlElement colorElement1 = xDoc.CreateElement("color");
            XmlText colorText1 = xDoc.CreateTextNode("00000000");
            colorElement1.AppendChild(colorText1);
            lineStyleElement1.AppendChild(colorElement1);
            XmlElement polyStyleElement1 = xDoc.CreateElement("PolyStyle");
            styleElement1.AppendChild(polyStyleElement1);
            colorElement1 = xDoc.CreateElement("color");
            colorText1 = xDoc.CreateTextNode("a0ff00ff");
            colorElement1.AppendChild(colorText1);
            polyStyleElement1.AppendChild(colorElement1);
            XmlElement fillElement1 = xDoc.CreateElement("fill");
            string fillstr1 = "1";
            XmlText fillText1 = xDoc.CreateTextNode(fillstr1);
            fillElement1.AppendChild(fillText1);
            polyStyleElement1.AppendChild(fillElement1);

            // Generate contours
            XmlElement folderElement = xDoc.CreateElement("Folder");
            docNode.AppendChild(folderElement);
            XmlElement nameElement = xDoc.CreateElement("name");
            XmlText nameText = xDoc.CreateTextNode(fileName);
            nameElement.AppendChild(nameText);
            folderElement.AppendChild(nameElement);
            XmlElement desElment = xDoc.CreateElement("description");
            XmlText desText = xDoc.CreateTextNode(@"Generated using MeteoInfo by Yaqiang Wang");
            desElment.AppendChild(desText);
            folderElement.AppendChild(desElment);

            foreach (PolygonShape pgs in this._shapeList)
            {
                Double currentLevel = pgs.lowValue;
                int levelNum = pgs.LegendIndex;

                foreach (Polygon polygon in pgs.Polygons)
                {
                    XmlElement placemarkElement = xDoc.CreateElement("Placemark");
                    folderElement.AppendChild(placemarkElement);
                    nameElement = xDoc.CreateElement("name");
                    nameText = xDoc.CreateTextNode("Level " + currentLevel.ToString());
                    nameElement.AppendChild(nameText);
                    placemarkElement.AppendChild(nameElement);
                    desElment = xDoc.CreateElement("description");
                    desText = xDoc.CreateTextNode("Level " + currentLevel.ToString());
                    desElment.AppendChild(desText);
                    placemarkElement.AppendChild(desElment);
                    XmlElement styleUrlElement = xDoc.CreateElement("styleUrl");
                    XmlText styleUrlText = xDoc.CreateTextNode("#pg" + levelNum.ToString());
                    styleUrlElement.AppendChild(styleUrlText);
                    placemarkElement.AppendChild(styleUrlElement);
                    XmlElement polygonElement = xDoc.CreateElement("Polygon");
                    placemarkElement.AppendChild(polygonElement);
                    XmlElement outerBIsElement = xDoc.CreateElement("outerBoundaryIs");
                    polygonElement.AppendChild(outerBIsElement);
                    XmlElement lrElement = xDoc.CreateElement("LinearRing");
                    outerBIsElement.AppendChild(lrElement);
                    XmlElement coorElement = xDoc.CreateElement("coordinates");
                    lrElement.AppendChild(coorElement);

                    int n = 0;
                    foreach (PointD point in polygon.OutLine)
                    {
                        XmlText coordText = xDoc.CreateTextNode(point.X.ToString() + "," + point.Y.ToString());
                        coorElement.AppendChild(coordText);
                        if (n < polygon.OutLine.Count - 1)
                            coorElement.InnerText = coorElement.InnerText + " ";
                        n += 1;
                    }

                    // If Fill=true then add innerBoundaryIs for the contour 'holes'
                    if (((PolygonBreak)LegendScheme.LegendBreaks[levelNum]).DrawFill)
                    {
                        if (polygon.HasHole)
                        {
                            foreach (List<PointD> hole in polygon.HoleLines)
                            {
                                XmlElement innerBIsElement = xDoc.CreateElement("innerBoundaryIs");
                                polygonElement.AppendChild(innerBIsElement);
                                lrElement = xDoc.CreateElement("LinearRing");
                                innerBIsElement.AppendChild(lrElement);
                                coorElement = xDoc.CreateElement("coordinates");
                                lrElement.AppendChild(coorElement);

                                n = 0;
                                foreach (PointD point in hole)
                                {
                                    XmlText coordText = xDoc.CreateTextNode(point.X.ToString() + "," + point.Y.ToString());
                                    coorElement.AppendChild(coordText);
                                    if (n < hole.Count - 1)
                                        coorElement.InnerText = coorElement.InnerText + " ";
                                    n += 1;
                                }
                            }
                        }
                    }
                }
            }

            // Save kml file
            xDoc.Save(fileName);
        }

        /// <summary>
        /// Save as KML (Google Earth data format) file
        /// <param name="fileName">KML file name</param>
        /// </summary>
        public void SaveAsKMLFile(string fileName)
        {
            switch (ShapeType)
            {
                case ShapeTypes.Polygon:
                case ShapeTypes.PolygonM:
                    SaveAsKMLFile_Polygon(fileName);
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineM:
                case ShapeTypes.PolylineZ:
                    SaveAsKMLFile_Polyline(fileName);
                    break;
                case ShapeTypes.Point:
                case ShapeTypes.PointM:
                case ShapeTypes.PointZ:
                    SaveAsKMLFile_Point(fileName);
                    break;
            }
        }

        /// <summary>
        /// Save as KML (Google Earth data format) file
        /// <param name="fileName">KML file name</param>
        /// </summary>
        private void SaveAsKMLFile_Polygon(string fileName)
        {
            // Create XML text file
            FileStream fs = new FileStream(fileName, FileMode.Create);
            XmlTextWriter writer = new XmlTextWriter(fs, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;

            // Open the document
            writer.WriteStartDocument(true);

            // Write KML
            writer.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            writer.WriteStartElement("Document");
            writer.WriteStartElement("Name");
            writer.WriteString(fileName);
            writer.WriteEndElement();    //Name
            // Write styles
            int styleNum = 0;
            foreach (ColorBreak cb in this.LegendScheme.LegendBreaks)
            {
                //StyleMap
                writer.WriteStartElement("StyleMap");
                writer.WriteAttributeString("id", @"pg" + styleNum.ToString());
                writer.WriteStartElement("Pair");
                writer.WriteStartElement("key");
                writer.WriteString("normal");
                writer.WriteEndElement();    //key
                writer.WriteStartElement("styleUrl");
                writer.WriteString("#pgn" + styleNum.ToString());
                writer.WriteEndElement();    //styleUrl
                writer.WriteEndElement();    //Pair

                writer.WriteStartElement("Pair");
                writer.WriteStartElement("key");
                writer.WriteString("highlight");
                writer.WriteEndElement();    //key
                writer.WriteStartElement("styleUrl");
                writer.WriteString("#pgn");
                writer.WriteEndElement();    //styleUrl
                writer.WriteEndElement();    //Pair
                writer.WriteEndElement();    //StyleMap

                //Normal style
                PolygonBreak pgb = (PolygonBreak)cb;
                writer.WriteStartElement("Style");
                writer.WriteAttributeString("id", @"pgn" + styleNum.ToString());
                writer.WriteStartElement("LineStyle");
                writer.WriteStartElement("color");
                writer.WriteString(ColorUtils.ToKMLColor(pgb.OutlineColor));
                writer.WriteEndElement();    //color
                writer.WriteEndElement();    //LineStyle
                writer.WriteStartElement("PolyStyle");
                writer.WriteStartElement("color");
                writer.WriteString(ColorUtils.ToKMLColor(pgb.Color));
                writer.WriteEndElement();    //color
                writer.WriteStartElement("fill");
                string fillstr = "1";
                if (!pgb.DrawFill)
                    fillstr = "0";
                writer.WriteString(fillstr);
                writer.WriteEndElement();    //fill
                writer.WriteEndElement();    //PolySyle
                writer.WriteEndElement();    //Style

                styleNum += 1;
            }

            // Highlight style - shared by all elements
            writer.WriteStartElement("Style");
            writer.WriteAttributeString("id", @"pgh");
            writer.WriteStartElement("LineStyle");
            writer.WriteStartElement("color");
            writer.WriteString("00000000");
            writer.WriteEndElement();    //color
            writer.WriteEndElement();    //LineStyle
            writer.WriteStartElement("PolyStyle");
            writer.WriteStartElement("color");
            writer.WriteString("a0ff00ff");
            writer.WriteEndElement();    //color
            writer.WriteStartElement("fill");
            writer.WriteString("1");
            writer.WriteEndElement();    //fill
            writer.WriteEndElement();    //PolyStyle
            writer.WriteEndElement();    //Style

            // Write shape coordinates
            writer.WriteStartElement("Folder");
            writer.WriteStartElement("name");
            writer.WriteString(fileName);
            writer.WriteEndElement();    //name
            writer.WriteStartElement("description");
            writer.WriteString(@"Generated using MeteoInfo");
            writer.WriteEndElement();    //description

            bool hasSelShape = this.HasSelectedShapes();
            foreach (PolygonShape pgs in this._shapeList)
            {
                if (hasSelShape)
                {
                    if (!pgs.Selected)
                        continue;
                }
                Double currentLevel = pgs.lowValue;
                int levelNum = pgs.LegendIndex;

                foreach (Polygon polygon in pgs.Polygons)
                {
                    writer.WriteStartElement("Placemark");
                    writer.WriteStartElement("name");
                    writer.WriteString("Level " + currentLevel.ToString());
                    writer.WriteEndElement();    //name
                    writer.WriteStartElement("description");
                    writer.WriteString("Level " + currentLevel.ToString());
                    writer.WriteEndElement();    //description
                    writer.WriteStartElement("styleUrl");
                    writer.WriteString("#pg" + levelNum.ToString());
                    writer.WriteEndElement();    //syleUrl
                    writer.WriteStartElement("Polygon");
                    writer.WriteStartElement("outerBoundaryIs");
                    writer.WriteStartElement("LinearRing");
                    writer.WriteStartElement("coordinates");                    
                    foreach (PointD point in polygon.OutLine)
                    {
                        writer.WriteString(point.X.ToString() + "," + point.Y.ToString() + " ");                                             
                    }
                    writer.WriteEndElement();    //Coordinates
                    writer.WriteEndElement();    //LinearRing
                    writer.WriteEndElement();    //OuterBoundaryIs

                    // If Fill=true then add innerBoundaryIs for the contour 'holes'
                    if (((PolygonBreak)LegendScheme.LegendBreaks[levelNum]).DrawFill)
                    {
                        if (polygon.HasHole)
                        {
                            foreach (List<PointD> hole in polygon.HoleLines)
                            {
                                writer.WriteStartElement("innerBoundaryIs");
                                writer.WriteStartElement("LinearRing");
                                writer.WriteStartElement("coordinates");                               
                                foreach (PointD point in hole)
                                {
                                    writer.WriteString(point.X.ToString() + "," + point.Y.ToString() + " ");                                    
                                }
                                writer.WriteEndElement();    //Coordinates
                                writer.WriteEndElement();    //LinearRing
                                writer.WriteEndElement();    //innerBoundaryIs
                            }
                        }
                    }
                    writer.WriteEndElement();    //Polygon
                    writer.WriteEndElement();    //Placemark
                }
            }

            writer.WriteEndElement();    //Folder
            writer.WriteEndElement();    //Document
            writer.WriteEndElement();    //kml

            // End the document
            writer.WriteEndDocument();

            // Close writer
            writer.Close();                 
        }

        /// <summary>
        /// Save as KML (Google Earth data format) file
        /// <param name="fileName">KML file name</param>
        /// </summary>
        private void SaveAsKMLFile_Polyline(string fileName)
        {
            // Create XML text file
            FileStream fs = new FileStream(fileName, FileMode.Create);
            XmlTextWriter writer = new XmlTextWriter(fs, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;

            // Open the document
            writer.WriteStartDocument(true);

            // Write KML
            writer.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            writer.WriteStartElement("Document");
            writer.WriteStartElement("Name");
            writer.WriteString(fileName);
            writer.WriteEndElement();    //Name
            // Write styles
            int styleNum = 0;
            foreach (ColorBreak cb in this.LegendScheme.LegendBreaks)
            {
                //StyleMap
                writer.WriteStartElement("StyleMap");
                writer.WriteAttributeString("id", @"pg" + styleNum.ToString());
                writer.WriteStartElement("Pair");
                writer.WriteStartElement("key");
                writer.WriteString("normal");
                writer.WriteEndElement();    //key
                writer.WriteStartElement("styleUrl");
                writer.WriteString("#pgn" + styleNum.ToString());
                writer.WriteEndElement();    //styleUrl
                writer.WriteEndElement();    //Pair

                writer.WriteStartElement("Pair");
                writer.WriteStartElement("key");
                writer.WriteString("highlight");
                writer.WriteEndElement();    //key
                writer.WriteStartElement("styleUrl");
                writer.WriteString("#pgn");
                writer.WriteEndElement();    //styleUrl
                writer.WriteEndElement();    //Pair
                writer.WriteEndElement();    //StyleMap

                //Normal style
                PolyLineBreak pgb = (PolyLineBreak)cb;
                writer.WriteStartElement("Style");
                writer.WriteAttributeString("id", @"pgn" + styleNum.ToString());
                writer.WriteStartElement("LineStyle");
                writer.WriteStartElement("color");
                writer.WriteString(ColorUtils.ToKMLColor(pgb.Color));
                writer.WriteEndElement();    //color
                writer.WriteEndElement();    //LineStyle                
                writer.WriteEndElement();    //Style

                styleNum += 1;
            }

            // Highlight style - shared by all elements
            writer.WriteStartElement("Style");
            writer.WriteAttributeString("id", @"pgh");
            writer.WriteStartElement("LineStyle");
            writer.WriteStartElement("color");
            writer.WriteString("00000000");
            writer.WriteEndElement();    //color
            writer.WriteEndElement();    //LineStyle            
            writer.WriteEndElement();    //Style

            // Write shape coordinates
            writer.WriteStartElement("Folder");
            writer.WriteStartElement("name");
            writer.WriteString(fileName);
            writer.WriteEndElement();    //name
            writer.WriteStartElement("description");
            writer.WriteString(@"Generated using MeteoInfo");
            writer.WriteEndElement();    //description

            bool hasSelShape = this.HasSelectedShapes();
            foreach (PolylineShape pgs in this._shapeList)
            {
                if (hasSelShape)
                {
                    if (!pgs.Selected)
                        continue;
                }
                Double currentLevel = pgs.value;
                int levelNum = pgs.LegendIndex;
                int i = 0;
                foreach (PolyLine line in pgs.PolyLines)
                {
                    writer.WriteStartElement("Placemark");
                    writer.WriteStartElement("name");
                    writer.WriteString("Level " + currentLevel.ToString());
                    writer.WriteEndElement();    //name
                    writer.WriteStartElement("description");
                    writer.WriteString("Level " + currentLevel.ToString());
                    writer.WriteEndElement();    //description
                    writer.WriteStartElement("styleUrl");
                    writer.WriteString("#pg" + levelNum.ToString());
                    writer.WriteEndElement();    //syleUrl
                    writer.WriteStartElement("LineString");                    
                    writer.WriteStartElement("coordinates");
                    string coorStr;
                    foreach (PointD point in line.PointList)
                    {
                        coorStr = point.X.ToString() + "," + point.Y.ToString();
                        if (ShapeType == ShapeTypes.PolylineZ)
                            coorStr = coorStr + "," + ((PolylineZShape)pgs).ZArray[i].ToString();                        
                        writer.WriteString(coorStr + " ");
                        i += 1;
                    }
                    writer.WriteEndElement();    //Coordinates                    
                    writer.WriteEndElement();    //LineString
                    writer.WriteEndElement();    //Placemark
                }
            }

            writer.WriteEndElement();    //Folder
            writer.WriteEndElement();    //Document
            writer.WriteEndElement();    //kml

            // End the document
            writer.WriteEndDocument();

            // Close writer
            writer.Close();
        }

        /// <summary>
        /// Save as KML (Google Earth data format) file
        /// <param name="fileName">KML file name</param>
        /// </summary>
        private void SaveAsKMLFile_Point(string fileName)
        {
            // Create XML text file
            FileStream fs = new FileStream(fileName, FileMode.Create);
            XmlTextWriter writer = new XmlTextWriter(fs, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;

            // Open the document
            writer.WriteStartDocument(true);

            // Write KML
            writer.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            writer.WriteStartElement("Document");
            writer.WriteStartElement("Name");
            writer.WriteString(fileName);
            writer.WriteEndElement();    //Name

            // Write styles
            int styleNum = 0;
            foreach (ColorBreak cb in this.LegendScheme.LegendBreaks)
            {
                //StyleMap
                writer.WriteStartElement("StyleMap");
                writer.WriteAttributeString("id", @"pg" + styleNum.ToString());
                writer.WriteStartElement("Pair");
                writer.WriteStartElement("key");
                writer.WriteString("normal");
                writer.WriteEndElement();    //key
                writer.WriteStartElement("styleUrl");
                writer.WriteString("#pgn" + styleNum.ToString());
                writer.WriteEndElement();    //styleUrl
                writer.WriteEndElement();    //Pair

                writer.WriteStartElement("Pair");
                writer.WriteStartElement("key");
                writer.WriteString("highlight");
                writer.WriteEndElement();    //key
                writer.WriteStartElement("styleUrl");
                writer.WriteString("#pgn");
                writer.WriteEndElement();    //styleUrl
                writer.WriteEndElement();    //Pair
                writer.WriteEndElement();    //StyleMap

                //Normal style
                PointBreak pgb = (PointBreak)cb;
                writer.WriteStartElement("Style");
                writer.WriteAttributeString("id", @"pgn" + styleNum.ToString());
                writer.WriteStartElement("BalloonStyle");
                writer.WriteStartElement("bgColor");
                writer.WriteString(ColorUtils.ToKMLColor(pgb.Color));
                writer.WriteEndElement();    //color
                writer.WriteEndElement();    //BalloonStyle                
                writer.WriteEndElement();    //Style

                styleNum += 1;
            }

            // Highlight style - shared by all elements
            writer.WriteStartElement("Style");
            writer.WriteAttributeString("id", @"pgh");
            writer.WriteStartElement("BalloonStyle");
            writer.WriteStartElement("color");
            writer.WriteString("00000000");
            writer.WriteEndElement();    //color
            writer.WriteEndElement();    //BalloonStyle            
            writer.WriteEndElement();    //Style

            // Write shape coordinates
            writer.WriteStartElement("Folder");
            writer.WriteStartElement("name");
            writer.WriteString(fileName);
            writer.WriteEndElement();    //name
            writer.WriteStartElement("description");
            writer.WriteString(@"Generated using MeteoInfo");
            writer.WriteEndElement();    //description

            bool hasSelShape = this.HasSelectedShapes();
            int shpIdx = 0;
            foreach (PointShape ps in this._shapeList)
            {
                if (hasSelShape)
                {
                    if (!ps.Selected)
                    {
                        shpIdx += 1;
                        continue;
                    }
                }
                Double currentLevel = ps.Value;
                int levelNum = ps.LegendIndex;

                writer.WriteStartElement("Placemark");
                if (LabelSet.DrawLabels)
                {
                    string label = this.GetCellValue(this.LabelSet.FieldName, shpIdx).ToString();
                    writer.WriteStartElement("name");
                    writer.WriteString(label);
                    writer.WriteEndElement();    //name
                }
                writer.WriteStartElement("description");
                writer.WriteString("Level " + currentLevel.ToString());
                writer.WriteEndElement();    //description
                writer.WriteStartElement("styleUrl");
                writer.WriteString("#pg" + levelNum.ToString());
                writer.WriteEndElement();    //syleUrl
                writer.WriteStartElement("Point");                
                writer.WriteStartElement("coordinates");
                string coorStr = ps.Point.X.ToString() + "," + ps.Point.Y.ToString();
                if (ShapeType == ShapeTypes.PointZ)
                    coorStr = coorStr + "," + ((PointZShape)ps).Z.ToString();
                writer.WriteString(coorStr);
                writer.WriteEndElement();    //Coordinates                
                writer.WriteEndElement();    //Point
                writer.WriteEndElement();    //Placemark

                shpIdx += 1;
            }

            writer.WriteEndElement();    //Folder
            writer.WriteEndElement();    //Document
            writer.WriteEndElement();    //kml

            // End the document
            writer.WriteEndDocument();

            // Close writer
            writer.Close();
        }

        #endregion

        #endregion
    }
}
