using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;
using MeteoInfoC.Layer;
using MeteoInfoC.Shape;
using MeteoInfoC.Legend;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MapData
{
    /// <summary>
    /// Map data
    /// </summary>
    public class MapDataManage
    {
        #region Methods

        /// <summary>
        /// Open a layer
        /// </summary>
        /// <param name="aFile">layer file path</param>
        /// <returns>map layer</returns>
        public static MapLayer OpenLayer(string aFile)
        {
            MapLayer aLayer = null;
            if (File.Exists(aFile))
            {
                switch (Path.GetExtension(aFile).ToLower())
                {
                    case ".dat":
                        aLayer = MapDataManage.ReadMapFile_MICAPS(aFile);
                        break;
                    case ".shp":
                        aLayer = MapDataManage.ReadMapFile_ShapeFile(aFile);
                        break;
                    case ".wmp":
                        aLayer = MapDataManage.ReadMapFile_WMP(aFile);
                        break;
                    case ".bln":
                        aLayer = MapDataManage.ReadMapFile_BLN(aFile);
                        break;
                    case ".bmp":
                    case ".gif":
                    case ".jpg":
                    case ".tif":
                    case ".png":
                        aLayer = MapDataManage.ReadImageFile(aFile);
                        break;
                    default:
                        aLayer = MapDataManage.ReadMapFile_GrADS(aFile);
                        break;
                }
            }

            return aLayer;
        }

        /// <summary>
        /// Read shapefile as map
        /// </summary>
        /// <param name="aFile">file name</param>       
        /// <returns>vectorlayer</returns>
        public static VectorLayer ReadMapFile_ShapeFile(string aFile)
        {            
            VectorLayer aLayer = ShapeFileManage.LoadShapeFile(aFile);

            return aLayer;
        }                

        /// <summary>
        /// Read MICAPS map
        /// </summary>
        /// <param name="aFile">file name</param>        
        public static VectorLayer ReadMapFile_MICAPS(string aFile)
        {
            StreamReader sr = new StreamReader(aFile);
            string aLine;
            string[] dataArray;
            int i, LastNonEmpty, lineNum, pNum;
            int lType, isClose;
            List<PointD> pList = new List<PointD>();
            //PointD aPoint;
            ArrayList dataList = new ArrayList();            
            PolylineShape aPolyline;                                   

            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
            string columnName = "Value";
            DataColumn aDC = new DataColumn(columnName, typeof(int));            
            aLayer.EditAddField(aDC);            

            lineNum = 0;
            lType = 0;
            isClose = 0;
            aLine = sr.ReadLine().Trim();
            if (aLine.Substring(0, 7) != "March 9")
            {
                MessageBox.Show("Data format is wrong!" + Environment.NewLine +
                    "Need MICAPS (March 9) data!", "Error");
                return null;
            }
            aLine = sr.ReadLine();
            while (aLine != null)
            {
                aLine = sr.ReadLine();
                if (aLine == null)
                {
                    break;
                }
                dataArray = aLine.Split();
                LastNonEmpty = -1;
                dataList.Clear();
                for (i = 0; i < dataArray.Length; i++)
                {
                    if (dataArray[i] != string.Empty)
                    {
                        LastNonEmpty++;
                        dataList.Add(dataArray[i]);
                    }
                }

                if (dataList.Count != 2)
                {
                    pNum = Convert.ToInt32(dataList[0]);
                    lType = Convert.ToInt32(dataList[1]);
                    isClose = Convert.ToInt32(dataList[2]);
                    if (pList.Count > 0)
                    {
                        //if (isClose == 1)
                        //{
                        aPolyline = new PolylineShape();
                        aPolyline.value = lineNum;
                        aPolyline.Points = pList;
                        aPolyline.Extent = MIMath.GetPointsExtent(pList);
                        aPolyline.PartNum = 1;
                        aPolyline.parts = new int[1];
                        aPolyline.parts[0] = 0;                        

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aPolyline, shapeNum))
                            aLayer.EditCellValue(columnName, shapeNum, lineNum);
                        
                        lineNum++;
                        //}
                        pList.Clear();                        
                    }
                }
                else
                {
                    PointD aPoint = new PointD();
                    aPoint.X = Convert.ToDouble(dataList[0]);
                    aPoint.Y = Convert.ToDouble(dataList[1]);
                    pList.Add(aPoint);
                }
            }
            //if (isClose == 1)
            //{
            aPolyline = new PolylineShape();
            aPolyline.value = lineNum;
            aPolyline.Extent = MIMath.GetPointsExtent(pList);
            aPolyline.Points = pList;            

            int sNum = aLayer.ShapeNum;
            if (aLayer.EditInsertShape(aPolyline, sNum))
                aLayer.EditCellValue(columnName, sNum, lineNum);
           

            sr.Close();
            
            aLayer.LayerName = Path.GetFileName(aFile);
            aLayer.FileName = aFile;
            aLayer.LayerDrawType = LayerDrawType.Map;
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
            aLayer.Visible = true;                 

            return aLayer;
        }

        /// <summary>
        /// Read WMP map
        /// </summary>
        /// <param name="aFile"></param>        
        /// <returns></returns>
        public static VectorLayer ReadMapFile_WMP(string aFile)
        {
            StreamReader sr = new StreamReader(aFile);
            string aLine;
            string shapeType;
            string[] dataArray;
            int shapeNum;
            int i, j, pNum;
            List<PointD> pList = new List<PointD>();
            PointD aPoint;
            ArrayList dataList = new ArrayList();                                            
            bool IsTrue = false;
            string columnName = "Value";
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);

            //Read shape type
            shapeType = sr.ReadLine().Trim();
            //Read shape number
            shapeNum = Convert.ToInt32(sr.ReadLine());
            switch (shapeType.ToLower())
            {
                case "point":
                    aLayer = new VectorLayer(ShapeTypes.Point);
                    aLayer.EditAddField(new DataColumn(columnName, typeof(int)));
                                    
                    for (i = 0; i < shapeNum; i++)
                    {
                        aLine = sr.ReadLine();
                        dataArray = aLine.Split(',');
                        aPoint = new PointD();
                        aPoint.X = Convert.ToDouble(dataArray[0]);
                        aPoint.Y = Convert.ToDouble(dataArray[1]);
                        pList.Add(aPoint);
                        PointShape aPS = new PointShape();
                        aPS.Value = i;
                        aPS.Point = aPoint;

                        int sNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aPS, sNum))
                        {                            
                            aLayer.EditCellValue(columnName, sNum, i);
                        }
                    }
                                        
                    aLayer.LayerName = Path.GetFileName(aFile);
                    aLayer.FileName = aFile;
                    aLayer.LayerDrawType = LayerDrawType.Map;
                    aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Black, 5);
                    aLayer.Visible = true;                                   
                    IsTrue = true;
                    break;
                case "polyline":
                    aLayer = new VectorLayer(ShapeTypes.Polyline);
                    aLayer.EditAddField(new DataColumn(columnName, typeof(int)));
                                     
                    for (i = 0; i < shapeNum; i++)
                    {
                        pNum = Convert.ToInt32(sr.ReadLine());
                        pList = new List<PointD>();
                        for (j = 0; j < pNum; j++)
                        {
                            aLine = sr.ReadLine();
                            dataArray = aLine.Split(',');
                            aPoint = new PointD();
                            aPoint.X = Convert.ToDouble(dataArray[0]);
                            aPoint.Y = Convert.ToDouble(dataArray[1]);
                            pList.Add(aPoint);
                        }
                        PolylineShape aPLS = new PolylineShape();   
                        aPLS.value = i;
                        aPLS.Extent = MIMath.GetPointsExtent(pList);
                        aPLS.Points = pList;                                                

                        int sNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aPLS, sNum))
                            aLayer.EditCellValue(columnName, sNum, i);
                    }
                    
                    aLayer.LayerName = Path.GetFileName(aFile);
                    aLayer.FileName = aFile;
                    aLayer.LayerDrawType = LayerDrawType.Map;
                    aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
                    aLayer.Visible = true;                                  
                    IsTrue = true;
                    break;
                case "polygon":
                    aLayer = new VectorLayer(ShapeTypes.Polygon);
                    aLayer.EditAddField(new DataColumn(columnName, typeof(int)));
                    
                    ArrayList polygons = new ArrayList();
                    for (i = 0; i < shapeNum; i++)
                    {
                        pNum = Convert.ToInt32(sr.ReadLine());
                        pList = new List<PointD>();
                        for (j = 0; j < pNum; j++)
                        {
                            aLine = sr.ReadLine();
                            dataArray = aLine.Split(',');
                            aPoint = new PointD();
                            aPoint.X = Convert.ToDouble(dataArray[0]);
                            aPoint.Y = Convert.ToDouble(dataArray[1]);
                            pList.Add(aPoint);
                        }
                        PolygonShape aPGS = new PolygonShape();
                        aPGS.lowValue = i;
                        aPGS.highValue = i;
                        aPGS.Extent = MIMath.GetPointsExtent(pList);
                        aPGS.Points = pList;                        
                        
                        int sNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aPGS, sNum))
                            aLayer.EditCellValue(columnName, sNum, i);
                    }
                    
                    aLayer.LayerName = Path.GetFileName(aFile);
                    aLayer.FileName = aFile;
                    aLayer.LayerDrawType = LayerDrawType.Map;
                    aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polygon, Color.FromArgb(255, 251, 195), 1.0F);
                    aLayer.Visible = true;                                    
                    IsTrue = true;
                    break;
                default:
                    MessageBox.Show("Shape type is invalid!" + Environment.NewLine +
                        shapeType, "Error");
                    IsTrue = false;
                    break;
            }

            sr.Close();

            if (IsTrue)
                return aLayer;
            else
                return null;
        }

        /// <summary>
        /// Write WMP map file
        /// </summary>
        /// <param name="aFile">output file</param>
        /// <param name="shapes">shape list</param>
        public static void WriteMapFile_WMP(string aFile, List<Shape.Shape> shapes)
        {
            StreamWriter sw = new StreamWriter(aFile);
            int shpNum = shapes.Count;
            int i;
            switch (shapes[0].ShapeType)
            {
                case ShapeTypes.Polyline:
                    sw.WriteLine("Polyline");
                    int shapeNum = 0;
                    PolylineShape aPLS = new PolylineShape();
                    for (i = 0; i < shpNum; i++)
                    {
                        aPLS = (PolylineShape)shapes[i];
                        shapeNum += aPLS.PartNum;
                    }
                    sw.WriteLine(shapeNum.ToString());

                    shapeNum = 0;
                    for (i = 0; i < shpNum; i++)
                    {
                        aPLS = (PolylineShape)shapes[i];
                        MeteoInfoC.PointD[] Pointps;
                        for (int p = 0; p < aPLS.PartNum; p++)
                        {
                            if (p == aPLS.PartNum - 1)
                            {
                                Pointps = new MeteoInfoC.PointD[aPLS.PointNum - aPLS.parts[p]];
                                for (int pp = aPLS.parts[p]; pp < aPLS.PointNum; pp++)
                                {
                                    Pointps[pp - aPLS.parts[p]] = (MeteoInfoC.PointD)aPLS.Points[pp];
                                }
                            }
                            else
                            {
                                Pointps = new MeteoInfoC.PointD[aPLS.parts[p + 1] - aPLS.parts[p]];
                                for (int pp = aPLS.parts[p]; pp < aPLS.parts[p + 1]; pp++)
                                {
                                    Pointps[pp - aPLS.parts[p]] = (MeteoInfoC.PointD)aPLS.Points[pp];
                                }
                            }
                            sw.WriteLine(Pointps.Length.ToString());
                            foreach (MeteoInfoC.PointD aPoint in Pointps)
                            {
                                sw.WriteLine(aPoint.X.ToString() + "," + aPoint.Y.ToString());
                            }
                            shapeNum += 1;
                        }                        
                    }
                    break;
                case ShapeTypes.Polygon:
                    sw.WriteLine("Polygon");
                    shapeNum = 0;
                    PolygonShape aPGS = new PolygonShape();
                    for (i = 0; i < shpNum; i++)
                    {
                        aPGS = (PolygonShape)shapes[i];
                        shapeNum += aPGS.PartNum;
                    }
                    sw.WriteLine(shapeNum.ToString());

                    for (i = 0; i < shpNum; i++)
                    {
                        aPGS = (PolygonShape)shapes[i];
                        MeteoInfoC.PointD[] Pointps;
                        for (int p = 0; p < aPGS.PartNum; p++)
                        {
                            if (p == aPGS.PartNum - 1)
                            {
                                Pointps = new MeteoInfoC.PointD[aPGS.PointNum - aPGS.parts[p]];
                                for (int pp = aPGS.parts[p]; pp < aPGS.PointNum; pp++)
                                {
                                    Pointps[pp - aPGS.parts[p]] = (MeteoInfoC.PointD)aPGS.Points[pp];
                                }
                            }
                            else
                            {
                                Pointps = new MeteoInfoC.PointD[aPGS.parts[p + 1] - aPGS.parts[p]];
                                for (int pp = aPGS.parts[p]; pp < aPGS.parts[p + 1]; pp++)
                                {
                                    Pointps[pp - aPGS.parts[p]] = (MeteoInfoC.PointD)aPGS.Points[pp];
                                }
                            }
                            sw.WriteLine(Pointps.Length.ToString());
                            foreach (MeteoInfoC.PointD aPoint in Pointps)
                            {
                                sw.WriteLine(aPoint.X.ToString() + "," + aPoint.Y.ToString());
                            }
                            shapeNum += 1;
                        }                        
                    }
                    break;
            }

            sw.Close();
        }

        /// <summary>
        /// Read Surfer BLN map
        /// </summary>
        /// <param name="aFile">file path</param>        
        /// <returns>vector layer</returns>
        public static VectorLayer ReadMapFile_BLN(string aFile)
        {
            StreamReader sr = new StreamReader(aFile);
            string aLine;
            string[] dataArray;
            int i, j, pNum;
            List<PointD> pList = new List<PointD>();
            PointD aPoint;
            ArrayList dataList = new ArrayList();
            bool IsTrue = false;
            string columnName = "Value";
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);

            //Read data
            aLayer.EditAddField(new DataColumn(columnName, typeof(int)));
            aLine = sr.ReadLine();
            bool isComma = true;
            if (!aLine.Contains(","))
                isComma = false;

            i = 0;
            while(aLine != null)
            {
                if (isComma)
                    pNum = int.Parse(aLine.Split(',')[0]);
                else
                    pNum = int.Parse(MIMath.SplitBySpace(aLine)[0]);
                pList = new List<PointD>();
                for (j = 0; j < pNum; j++)
                {
                    aLine = sr.ReadLine();
                    if (isComma)
                        dataArray = aLine.Split(',');
                    else
                        dataArray = MIMath.SplitBySpace(aLine);
                    aPoint = new PointD();
                    aPoint.X = Convert.ToDouble(dataArray[0]);
                    aPoint.Y = Convert.ToDouble(dataArray[1]);
                    pList.Add(aPoint);
                }
                PolylineShape aPLS = new PolylineShape();
                aPLS.value = i;
                aPLS.Extent = MIMath.GetPointsExtent(pList);
                aPLS.Points = pList;
                i += 1;

                int sNum = aLayer.ShapeNum;
                if (aLayer.EditInsertShape(aPLS, sNum))
                    aLayer.EditCellValue(columnName, sNum, i);

                aLine = sr.ReadLine();
            }

            aLayer.LayerName = Path.GetFileName(aFile);
            aLayer.FileName = aFile;
            aLayer.LayerDrawType = LayerDrawType.Map;
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
            aLayer.Visible = true;
            IsTrue = true;

            sr.Close();

            if (IsTrue)
                return aLayer;
            else
                return null;
        }

        /// <summary>
        /// Write Sufer BLN map file
        /// </summary>
        /// <param name="aFile">output file</param>
        /// <param name="shapes">shape list</param>
        public static void WriteMapFile_BLN(string aFile, List<Shape.Shape> shapes)
        {
            StreamWriter sw = new StreamWriter(aFile);
            int shpNum = shapes.Count;
            int i;
            switch (shapes[0].ShapeType)
            {
                case ShapeTypes.Polyline:
                    int shapeNum = 0;
                    PolylineShape aPLS = new PolylineShape();
                    for (i = 0; i < shpNum; i++)
                    {
                        aPLS = (PolylineShape)shapes[i];
                        MeteoInfoC.PointD[] Pointps;
                        for (int p = 0; p < aPLS.PartNum; p++)
                        {
                            if (p == aPLS.PartNum - 1)
                            {
                                Pointps = new MeteoInfoC.PointD[aPLS.PointNum - aPLS.parts[p]];
                                for (int pp = aPLS.parts[p]; pp < aPLS.PointNum; pp++)
                                {
                                    Pointps[pp - aPLS.parts[p]] = (MeteoInfoC.PointD)aPLS.Points[pp];
                                }
                            }
                            else
                            {
                                Pointps = new MeteoInfoC.PointD[aPLS.parts[p + 1] - aPLS.parts[p]];
                                for (int pp = aPLS.parts[p]; pp < aPLS.parts[p + 1]; pp++)
                                {
                                    Pointps[pp - aPLS.parts[p]] = (MeteoInfoC.PointD)aPLS.Points[pp];
                                }
                            }
                            sw.WriteLine(Pointps.Length.ToString() + ",1");
                            foreach (MeteoInfoC.PointD aPoint in Pointps)
                            {
                                sw.WriteLine(aPoint.X.ToString() + "," + aPoint.Y.ToString());
                            }
                            shapeNum += 1;
                        }
                    }
                    break;
                case ShapeTypes.Polygon:                    
                    shapeNum = 0;
                    PolygonShape aPGS = new PolygonShape();                    
                    for (i = 0; i < shpNum; i++)
                    {
                        aPGS = (PolygonShape)shapes[i];
                        MeteoInfoC.PointD[] Pointps;
                        for (int p = 0; p < aPGS.PartNum; p++)
                        {
                            if (p == aPGS.PartNum - 1)
                            {
                                Pointps = new MeteoInfoC.PointD[aPGS.PointNum - aPGS.parts[p]];
                                for (int pp = aPGS.parts[p]; pp < aPGS.PointNum; pp++)
                                {
                                    Pointps[pp - aPGS.parts[p]] = (MeteoInfoC.PointD)aPGS.Points[pp];
                                }
                            }
                            else
                            {
                                Pointps = new MeteoInfoC.PointD[aPGS.parts[p + 1] - aPGS.parts[p]];
                                for (int pp = aPGS.parts[p]; pp < aPGS.parts[p + 1]; pp++)
                                {
                                    Pointps[pp - aPGS.parts[p]] = (MeteoInfoC.PointD)aPGS.Points[pp];
                                }
                            }
                            sw.WriteLine(Pointps.Length.ToString() + ",1");
                            foreach (MeteoInfoC.PointD aPoint in Pointps)
                            {
                                sw.WriteLine(aPoint.X.ToString() + "," + aPoint.Y.ToString());
                            }
                            shapeNum += 1;
                        }
                    }
                    break;
            }

            sw.Close();
        }

        /// <summary>
        /// Read GrADS map
        /// </summary>
        /// <param name="aFile"></param>   
        /// <returns></returns>
        public static VectorLayer ReadMapFile_GrADS(string aFile)
        {
            FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int i, lineNum;
            byte b;
            Int16 N, lType;
            double lon, lat;
            byte[] bytes;
            
            PointD aPoint;
            List<PointD> pList = new List<PointD>();                                               

            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
            string columnName = "Value";
            DataColumn aDC = new DataColumn(columnName, typeof(int));            
            aLayer.EditAddField(aDC);
            
            lineNum = 0;
            do
            {
                b = br.ReadByte();    // 1-data, 2-skip
                if (Convert.ToString(b) == "2")
                {
                    br.ReadBytes(18);
                    continue;
                }
                b = br.ReadByte();    // Line type: country, river ...
                lType = Convert.ToInt16(b);
                b = br.ReadByte();   // Point number
                N = Convert.ToInt16(b);
                for (i = 0; i < N; i++)
                {
                    bytes = br.ReadBytes(3);    //Longitude
                    int val = bytes[0] << 16 | bytes[1] << 8 | bytes[2];
                    lon = val / 10000.0;

                    bytes = br.ReadBytes(3);    //Latitude
                    val = bytes[0] << 16 | bytes[1] << 8 | bytes[2];
                    lat = val / 10000.0 - 90.0;

                    aPoint = new PointD();
                    aPoint.X = lon;
                    aPoint.Y = lat;
                    pList.Add(aPoint);
                }
                if (pList.Count > 1)
                {
                    //if (N < 255)
                    //{
                    PolylineShape aPolyline = new PolylineShape();
                        aPolyline.value = lineNum;
                        aPolyline.Points = pList;
                        aPolyline.Extent = MIMath.GetPointsExtent(pList);
                        aPolyline.PartNum = 1;
                        aPolyline.parts = new int[1];
                        aPolyline.parts[0] = 0;                        

                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aPolyline, shapeNum))
                        {                            
                            aLayer.EditCellValue(columnName, shapeNum, lineNum);
                        }
                       
                        lineNum++;
                    //}
                }
                pList = new List<PointD>();

            } while (br.BaseStream.Position < br.BaseStream.Length);

            br.Close();
            fs.Close();
            
            aLayer.LayerName = Path.GetFileName(aFile);
            aLayer.FileName = aFile;
            aLayer.LayerDrawType = LayerDrawType.Map;
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
            aLayer.Visible = true;            

            return aLayer;
        }

        /// <summary>
        /// Write GrADS map file
        /// </summary>
        /// <param name="aFile">file name</param>
        /// <param name="Polylines">PolylineShape list</param>
        public static void WriteMapFile_GrADS(string aFile, List<PolylineShape> Polylines)
        {
            FileStream fs = new FileStream(aFile, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            int i;
            ArrayList points = new ArrayList();            
            for (i = 0; i < Polylines.Count; i++)
            {
                PolylineShape aPLS = Polylines[i];
                for (int p = 0; p < aPLS.PartNum; p++)
                {
                    points = new ArrayList();
                    if (p == aPLS.PartNum - 1)
                    {
                        for (int pp = aPLS.parts[p]; pp < aPLS.PointNum; pp++)
                        {
                            points.Add((MeteoInfoC.PointD)aPLS.Points[pp]);
                        }
                    }
                    else
                    {
                        for (int pp = aPLS.parts[p]; pp < aPLS.parts[p + 1]; pp++)
                        {
                            points.Add((MeteoInfoC.PointD)aPLS.Points[pp]);
                        }
                    }
                    WriteGrADSMapBW(bw, points);
                }                      
            }

            bw.Close();
            fs.Close();
        }

        /// <summary>
        /// Write GrADS map file
        /// </summary>
        /// <param name="aFile">file name</param>
        /// <param name="Polygons">PolygonShape list</param>
        public static void WriteMapFile_GrADS(string aFile, List<PolygonShape> Polygons)
        {
            FileStream fs = new FileStream(aFile, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            int i;
            ArrayList points = new ArrayList();
            for (i = 0; i < Polygons.Count; i++)
            {
                PolygonShape aPLS = Polygons[i];
                for (int p = 0; p < aPLS.PartNum; p++)
                {
                    points = new ArrayList();
                    if (p == aPLS.PartNum - 1)
                    {
                        for (int pp = aPLS.parts[p]; pp < aPLS.PointNum; pp++)
                        {
                            points.Add((MeteoInfoC.PointD)aPLS.Points[pp]);
                        }
                    }
                    else
                    {
                        for (int pp = aPLS.parts[p]; pp < aPLS.parts[p + 1]; pp++)
                        {
                            points.Add((MeteoInfoC.PointD)aPLS.Points[pp]);
                        }
                    }
                    WriteGrADSMapBW(bw, points);
                }
            }

            bw.Close();
            fs.Close();
        }

        private static void WriteGrADSMapBW(BinaryWriter bw, ArrayList points)
        {
            int j;
            PointD aPoint = new PointD();
            aPoint = (PointD)points[0];
            int xShift = 0;
            if (aPoint.X < 0)
            {
                xShift = 360;
            }
            byte aByte;
            byte[] bytes = new byte[3];
            ulong data;
            if (points.Count < 255)
            {
                aByte = (byte)1;
                bw.Write(aByte);    //Record type
                aByte = (byte)0;
                bw.Write(aByte);    //Line type
                aByte = (byte)points.Count;
                bw.Write(aByte);    //Point number                                        
                for (j = 0; j < points.Count; j++)
                {
                    aPoint = (PointD)points[j];
                    //if (aPoint.X < 0)
                    //{
                    //    aPoint.X += 360;
                    //}
                    data = (ulong)((aPoint.X + xShift) * 10000);
                    bytes[0] = (byte)(data >> 16);
                    bytes[1] = (byte)(data >> 8);
                    bytes[2] = (byte)(data);
                    bw.Write(bytes);
                    data = (ulong)((aPoint.Y + 90) * 10000);
                    bytes[0] = (byte)(data >> 16);
                    bytes[1] = (byte)(data >> 8);
                    bytes[2] = (byte)(data);
                    bw.Write(bytes);
                }
            }
            else
            {
                do
                {
                    aByte = (byte)1;
                    bw.Write(aByte);    //Record type
                    aByte = (byte)0;
                    bw.Write(aByte);    //Line type
                    aByte = (byte)254;
                    bw.Write(aByte);    //Point number                        
                    for (j = 0; j < 254; j++)
                    {
                        aPoint = (PointD)points[j];
                        //if (aPoint.X < 0)
                        //{
                        //    aPoint.X += 360;
                        //}
                        data = (ulong)((aPoint.X + xShift) * 10000);
                        bytes[0] = (byte)(data >> 16);
                        bytes[1] = (byte)(data >> 8);
                        bytes[2] = (byte)(data);
                        bw.Write(bytes);
                        data = (ulong)((aPoint.Y + 90) * 10000);
                        bytes[0] = (byte)(data >> 16);
                        bytes[1] = (byte)(data >> 8);
                        bytes[2] = (byte)(data);
                        bw.Write(bytes);
                    }
                    points.RemoveRange(0, 254);
                    if (points.Count < 255)
                    {
                        break;
                    }
                }
                while (points.Count > 254);

                if (points.Count > 1)
                {
                    aByte = (byte)1;
                    bw.Write(aByte);    //Record type
                    aByte = (byte)0;
                    bw.Write(aByte);    //Line type
                    aByte = (byte)points.Count;
                    bw.Write(aByte);    //Point number                   
                    for (j = 0; j < points.Count; j++)
                    {
                        aPoint = (PointD)points[j];
                        //if (aPoint.X < 0)
                        //{
                        //    aPoint.X += 360;
                        //}
                        data = (ulong)((aPoint.X + xShift) * 10000);
                        bytes[0] = (byte)(data >> 16);
                        bytes[1] = (byte)(data >> 8);
                        bytes[2] = (byte)(data);
                        bw.Write(bytes);
                        data = (ulong)((aPoint.Y + 90) * 10000);
                        bytes[0] = (byte)(data >> 16);
                        bytes[1] = (byte)(data >> 8);
                        bytes[2] = (byte)(data);
                        bw.Write(bytes);
                    }
                }
            }
        }

        /// <summary>
        /// Read image file
        /// </summary>
        /// <param name="aFile">File name</param>
        ///<returns>image layer</returns>
        public static ImageLayer ReadImageFile(string aFile)
        {            
            string oEx = Path.GetExtension(aFile);
            string sEx = oEx.Remove(2, 1);
            sEx = sEx + "w";
            string wFile = aFile.Replace(oEx, sEx);
            Image aImage = new Bitmap(aFile);            
            ImageLayer aImageLayer = new ImageLayer();
            aImageLayer.FileName = aFile;
            aImageLayer.WorldFileName = wFile;
            aImageLayer.Image = aImage;            
            aImageLayer.LayerName = Path.GetFileName(aFile);
            aImageLayer.Visible = true;
            if (File.Exists(wFile))
            {
                aImageLayer.ReadImageWorldFile(wFile);
            }
            else
            {
                WorldFilePara aWFP = new WorldFilePara();
                aWFP.XUL = 0;
                aWFP.YUL = 90;
                aWFP.XScale = 0.05;
                aWFP.YScale = -0.05;
                aWFP.XRotate = 0;
                aWFP.YRotate = 0;
                aImageLayer.WorldFileParaV = aWFP;
                aImageLayer.WriteImageWorldFile(wFile, aImageLayer.WorldFileParaV);
            }

            double XBR, YBR;
            XBR = aImageLayer.Image.Width * aImageLayer.WorldFileParaV.XScale + aImageLayer.WorldFileParaV.XUL;
            YBR = aImageLayer.Image.Height * aImageLayer.WorldFileParaV.YScale + aImageLayer.WorldFileParaV.YUL;
            Extent aExtent = new Extent();
            aExtent.minX = aImageLayer.WorldFileParaV.XUL;
            aExtent.minY = YBR;
            aExtent.maxX = XBR;
            aExtent.maxY = aImageLayer.WorldFileParaV.YUL;
            aImageLayer.Extent = aExtent;
            aImageLayer.LayerDrawType = LayerDrawType.Image;
            aImageLayer.IsMaskout = true;

            return aImageLayer;
        }
        #endregion
    }
}
