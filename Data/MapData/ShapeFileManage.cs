using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
//using System.Data.Odbc;
//using System.Data.OleDb;
using MeteoInfoC.Layer;
using MeteoInfoC.Global;
using MeteoInfoC.Shape;
using MeteoInfoC.Legend;
using MeteoInfoC.Projections;

namespace MeteoInfoC.Data.MapData
{
    /// <summary>
    /// Shape file manage
    /// </summary>
    public class ShapeFileManage
    {
        #region Methods
        ///// <summary>
        ///// Load shape file
        ///// </summary>
        ///// <param name="shpfilepath"></param>
        ///// <param name="aSF"></param>
        ///// <returns></returns>
        //public static Boolean LoadShapeFile(string shpfilepath, ref ShapeFile aSF)
        //{                 
        //    string dataDir = Path.GetDirectoryName(shpfilepath);
        //    string shpfilename = Path.GetFileName(shpfilepath);
        //    string shxfilepath = shpfilepath.Replace(Path.GetExtension(shpfilepath), ".shx");
        //    string dbffilepath = shpfilepath.Replace(Path.GetExtension(shpfilepath), ".dbf");
        //    if (!File.Exists(shxfilepath))
        //    {
        //        shxfilepath = shxfilepath.Replace(".shx", ".SHX");
        //    }
        //    if (!File.Exists(dbffilepath))
        //    {
        //        dbffilepath = dbffilepath.Replace(".dbf", ".DBF");
        //    }

        //    ////read out the layer attribute infomation 
        //    //string connectionString;
        //    //connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dataDir + @";Extended Properties=""dBASE IV;HDR=Yes;"";";
        //    //OleDbConnection conn = new OleDbConnection(connectionString);
        //    //conn.Open();
        //    //OleDbDataAdapter adpt = new OleDbDataAdapter("Select * From [" + Path.GetFileName(dbffilepath) + "]", connectionString);
        //    ////OleDbDataAdapter adpt = new OleDbDataAdapter("Select * From [" + dbffilepath + "]", connectionString);
        //    //DataTable dt = new DataTable();
        //    //adpt.Fill(dt);
        //    //conn.Close();

        //    //aSF.dataTable = dt;

        //    AttributeTable attrTable = new AttributeTable();
        //    attrTable.Open(shpfilepath);
        //    attrTable.Fill(attrTable.NumRecords);
        //    aSF.dataTable = attrTable.GetDataTable();

        //    aSF.shapes = new ArrayList();
        //    try
        //    {
        //        //先读取.shx文件,得到文件的总字节长度 
        //        if (shxfilepath == "")
        //        {
        //            //  MessageBox.Show("索引文件打开出错"); 
        //            return false;
        //        }
        //        FileStream fs = new FileStream(shxfilepath, FileMode.Open, FileAccess.Read);   //文件流形式   
        //        long BytesSum = fs.Length;  //得到文件的字节总长   
        //        aSF.shapeNum = (int)(BytesSum - 100) / 8;  //得以总记录数目              

        //        fs.Close();

        //        //打开.shp文件,读取x,y坐标的信息 
        //        fs = new FileStream(shpfilepath, FileMode.Open, FileAccess.Read);   //文件流形式 
        //        BinaryReader br = new BinaryReader(fs);     //打开二进制文件    

        //        br.ReadBytes(32);  //先读出36个字节,紧接着是Box边界合 
        //        int aShapeType = br.ReadInt32();

        //        aSF.extent.minX = br.ReadDouble();   //读出整个shp图层的边界合 
        //        aSF.extent.minY = br.ReadDouble();
        //        aSF.extent.maxX = br.ReadDouble();
        //        aSF.extent.maxY = br.ReadDouble();


        //        br.ReadBytes(32);  //   shp中尚未使用的边界盒     


        //        //Get Shape Data From Here On                 
        //        double x, y;
        //        PointF aPF = new PointF(0, 0);
        //        switch (aShapeType)
        //        {
        //            case 1://single point 
        //                aSF.shapeFileType = ShapeFileType.Point;
        //                Shape_Point aP = new Shape_Point();
        //                for (int i = 0; i < aSF.shapeNum; i++)
        //                {

        //                    br.ReadBytes(12); //记录头8个字节和一个int(4个字节)的shapetype 

        //                    /* stype = BinaryFile.ReadInt32(); 
                                   
        //                     if (stype != shapetype) 
        //                         continue; 
 
        //                     */
        //                    x = br.ReadDouble();
        //                    y = br.ReadDouble();

        //                    aP.X = x;
        //                    aP.Y = y;
        //                    aSF.shapes.Add(aP);
        //                }
        //                break;

        //            case 8://multi points layer 
        //                break;

        //            case 3://Polyline layer 
        //                aSF.shapeFileType = ShapeFileType.PolyLine;
        //                Shape_PolyLine aPL = new Shape_PolyLine();
        //                for (int i = 0; i < aSF.shapeNum; i++)
        //                {
        //                    //geolayer.getAttributeContainer().Add(ds.Tables[0].Rows[i][0]);      //read out the attribute step by step 

        //                    br.ReadBytes(12);

        //                    //	 int pos = indexRecs[i].Offset+8; 
        //                    //	 bb0.position(pos); 
        //                    //	 stype = bb0.getInt(); 
        //                    //	 if (stype!=nshapetype){ 
        //                    //		 continue; 
        //                    //	 } 

        //                    aPL.extent.minX = br.ReadDouble();
        //                    aPL.extent.minY = br.ReadDouble();
        //                    aPL.extent.maxX = br.ReadDouble();
        //                    aPL.extent.maxY = br.ReadDouble();

        //                    aPL.PartNum = br.ReadInt32();
        //                    aPL.PointNum = br.ReadInt32();
        //                    aPL.parts = new int[aPL.PartNum];
        //                    aPL.points = new PointF[aPL.PointNum];

        //                    //firstly read out parts begin pos in file 
        //                    for (int j = 0; j < aPL.PartNum; j++)
        //                    {
        //                        aPL.parts[j] = br.ReadInt32();
        //                    }

        //                    //read out coordinates 
        //                    for (int j = 0; j < aPL.PointNum; j++)
        //                    {
        //                        x = br.ReadDouble();
        //                        y = br.ReadDouble();
        //                        aPF.X = (float)x;
        //                        aPF.Y = (float)y;
        //                        aPL.points[j] = aPF;
        //                    }
        //                    aSF.shapes.Add(aPL);
        //                }
        //                break;
        //            case 5://Polygon layer 
        //                aSF.shapeFileType = ShapeFileType.Polygon;
        //                Shape_Polygon aSPG = new Shape_Polygon();
        //                for (int i = 0; i < aSF.shapeNum; i++)
        //                {
        //                    //geolayer.getAttributeContainer().Add(ds.Tables[0].Rows[i][0]);

        //                    /*  bb0.rewind(); 
        //                      bb0.position(indexRecs[i].Offset + 8); 
        //                      stype = BinaryFile.ReadInt32(); 
 
        //                      if (stype != shapetype) 
        //                      { 
        //                          continue; 
        //                      }*/

        //                    br.ReadBytes(12);

        //                    aSPG.extent.minX = br.ReadDouble();
        //                    aSPG.extent.minY = br.ReadDouble();
        //                    aSPG.extent.maxX = br.ReadDouble();
        //                    aSPG.extent.maxY = br.ReadDouble();
        //                    aSPG.PartNum = br.ReadInt32();
        //                    aSPG.PointNum = br.ReadInt32();
        //                    aSPG.parts = new int[aSPG.PartNum];
        //                    aSPG.points = new PointF[aSPG.PointNum];

        //                    //firstly read out parts begin pos in file 
        //                    for (int j = 0; j < aSPG.PartNum; j++)
        //                    {
        //                        aSPG.parts[j] = br.ReadInt32();
        //                    }

        //                    //read out coordinates 
        //                    for (int j = 0; j < aSPG.PointNum; j++)
        //                    {
        //                        x = br.ReadDouble();
        //                        y = br.ReadDouble();
        //                        aPF.X = (float)x;
        //                        aPF.Y = (float)y;
        //                        aSPG.points[j] = aPF;
        //                    }
        //                    aSF.shapes.Add(aSPG);
        //                }
        //                break;
        //            default:
        //                return false;
        //        }

        //        fs.Close();
        //        br.Close();
        //    }
        //    catch (FileNotFoundException e)
        //    {
        //        e.ToString();

        //    }

        //    return true;
        //}

        /// <summary>
        /// Load shape file
        /// </summary>
        /// <param name="shpfilepath">shape file path</param>        
        /// <returns>vectorlayer</returns>
        public static VectorLayer LoadShapeFile(string shpfilepath)
        {
            //Set file names
            string dataDir = Path.GetDirectoryName(shpfilepath);
            string shpfilename = Path.GetFileName(shpfilepath);
            string shxfilepath = shpfilepath.Replace(Path.GetExtension(shpfilepath), ".shx");
            string dbffilepath = shpfilepath.Replace(Path.GetExtension(shpfilepath), ".dbf");
            string projfilepath = shpfilepath.Replace(Path.GetExtension(shpfilepath), ".prj");
            if (!File.Exists(shxfilepath))
                shxfilepath = shxfilepath.Replace(".shx", ".SHX");
            if (!File.Exists(dbffilepath))
                dbffilepath = dbffilepath.Replace(".dbf", ".DBF");
            if (!File.Exists(projfilepath))
                projfilepath = projfilepath.Replace(".prj", ".PRJ");
                        
            //Read shx file 
            if (shxfilepath == "")
            {
                //  MessageBox.Show("Open shx file error"); 
                return null;
            }
            FileStream fs = new FileStream(shxfilepath, FileMode.Open, FileAccess.Read);     
            long BytesSum = fs.Length;  //Get file byte length   
            int shapeNum = (int)(BytesSum - 100) / 8;  //Get total number of records   
            LoadShxFile(shxfilepath);

            fs.Close();

            //Open shp file 
            fs = new FileStream(shpfilepath, FileMode.Open, FileAccess.Read);   
            BinaryReader br = new BinaryReader(fs);      

            br.ReadBytes(32);  //先读出36个字节,紧接着是Box边界合 
            int aShapeType = br.ReadInt32();
            ShapeTypes aST = (ShapeTypes)aShapeType;
            VectorLayer aLayer = null;
            
            Extent aExtent = new Extent();
            aExtent.minX = br.ReadDouble();   //读出整个shp图层的边界合 
            aExtent.minY = br.ReadDouble();
            aExtent.maxX = br.ReadDouble();
            aExtent.maxY = br.ReadDouble();            


            br.ReadBytes(32);  //   shp中尚未使用的边界盒     


            //Get Shape Data             
            switch (aST)
            {
                case ShapeTypes.Point://single point                                                              
                    aLayer = ReadPointShapes(br, shapeNum);
                    break;
                case ShapeTypes.PointM:
                    aLayer = ReadPointMShapes(br, shapeNum);
                    break;
                case ShapeTypes.Polyline:    //Polyline layer       
                    aLayer = ReadPolylineShapes(br, shapeNum);                            
                    break;
                case ShapeTypes.PolylineM:
                    aLayer = ReadPolylineMShapes(br, shapeNum);
                    break;
                case ShapeTypes.PolylineZ:
                    aLayer = ReadPolylineZShapes(br, shapeNum);
                    break;
                case ShapeTypes.Polygon:    //Polygon layer                                                               
                    aLayer = ReadPolygonShapes(br, shapeNum);
                    break;   
                case ShapeTypes.PolygonM:
                    aLayer = ReadPolygonMShapes(br, shapeNum);
                    break;
            }

            fs.Close();
            br.Close();

            if (aLayer != null)
            {
                aLayer.Extent = aExtent;
                //Layer property
                aLayer.LayerDrawType = LayerDrawType.Map;
                aLayer.FileName = shpfilepath;
                aLayer.LayerName = shpfilename;
                aLayer.Visible = true;

                //read out the layer attribute information             
                AttributeTable attrTable = new AttributeTable();
                attrTable.Open(shpfilepath);
                attrTable.Fill(attrTable.NumRecords);
                aLayer.AttributeTable = attrTable;
                aLayer.NumFields = attrTable.Table.Columns.Count;

                //Get projection information
                if (File.Exists(projfilepath))
                {
                    LoadProjFile(projfilepath, ref aLayer);
                }
            }

            return aLayer;
                        
        }

        private static void ReadHeader(BinaryReader br)
        {
            int i;                                                                        
            
            int FileCode = SwapByteOrder(br.ReadInt32());
            for (i = 0; i < 5; i++)
            {
                br.ReadInt32();
            }
            int FileLength = SwapByteOrder(br.ReadInt32());
            int Version = br.ReadInt32();
            int aShapeType = br.ReadInt32();
            Extent aExtent = new Extent();
            aExtent.minX = br.ReadDouble();
            aExtent.minY = br.ReadDouble();
            aExtent.maxX = br.ReadDouble();
            aExtent.maxY = br.ReadDouble();
            for (i = 0; i < 4; i++)
            {
                br.ReadDouble();
            }

            //MessageBox.Show(FileLength.ToString() + Environment.NewLine + FileCode.ToString());
        }

        private static VectorLayer ReadPointShapes(BinaryReader br, int shapeNum)
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            int RecordNum, ContentLength, aShapeType;
            double x, y;
            //PointD aPoint;

            for (int i = 0; i < shapeNum; i++)
            {

                //br.ReadBytes(12); //记录头8个字节和一个int(4个字节)的shapetype 
                RecordNum = SwapByteOrder(br.ReadInt32());
                ContentLength = SwapByteOrder(br.ReadInt32());
                aShapeType = br.ReadInt32();

                x = br.ReadDouble();
                y = br.ReadDouble();

                PointShape aP = new PointShape();
                PointD aPoint = new PointD();
                aPoint.X = x;
                aPoint.Y = y;
                aP.Point = aPoint;
                aLayer.ShapeList.Add(aP);
            }

            //Create legend scheme            
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Black, 5);
            return aLayer;
        }

        private static VectorLayer ReadPointMShapes(BinaryReader br, int shapeNum)
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.PointM);
            int RecordNum, ContentLength, aShapeType;
            double x, y, m;
            //PointD aPoint;

            for (int i = 0; i < shapeNum; i++)
            {

                //br.ReadBytes(12); //记录头8个字节和一个int(4个字节)的shapetype 
                RecordNum = SwapByteOrder(br.ReadInt32());
                ContentLength = SwapByteOrder(br.ReadInt32());
                aShapeType = br.ReadInt32();

                x = br.ReadDouble();
                y = br.ReadDouble();
                m = br.ReadDouble();

                PointMShape aP = new PointMShape();
                PointM aPoint = new PointM();
                aPoint.X = x;
                aPoint.Y = y;
                aPoint.M = m;
                aP.Point = aPoint;
                aLayer.ShapeList.Add(aP);
            }

            //Create legend scheme            
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Black, 5);
            return aLayer;
        }

        private static VectorLayer ReadPolylineShapes(BinaryReader br, int shapeNum)
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
            double x, y;
            //PointD aPoint;
            for (int i = 0; i < shapeNum; i++)
            {
                //geolayer.getAttributeContainer().Add(ds.Tables[0].Rows[i][0]);      //read out the attribute step by step 

                br.ReadBytes(12);

                //	 int pos = indexRecs[i].Offset+8; 
                //	 bb0.position(pos); 
                //	 stype = bb0.getInt(); 
                //	 if (stype!=nshapetype){ 
                //		 continue; 
                //	 } 

                PolylineShape aPL = new PolylineShape();
                Extent extent;
                extent.minX = br.ReadDouble();
                extent.minY = br.ReadDouble();
                extent.maxX = br.ReadDouble();
                extent.maxY = br.ReadDouble();
                aPL.Extent = extent;

                aPL.PartNum = br.ReadInt32();
                int numPoints = br.ReadInt32();
                aPL.parts = new int[aPL.PartNum];
                List<PointD> points = new List<PointD>();

                //firstly read out parts begin pos in file 
                for (int j = 0; j < aPL.PartNum; j++)
                {
                    aPL.parts[j] = br.ReadInt32();
                }

                //read out coordinates 
                for (int j = 0; j < numPoints; j++)
                {
                    x = br.ReadDouble();
                    y = br.ReadDouble();
                    PointD aPoint = new PointD();
                    aPoint.X = x;
                    aPoint.Y = y;
                    points.Add(aPoint);
                }
                aPL.Points = points;
                aLayer.ShapeList.Add(aPL);
            }

            //Create legend scheme            
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
            return aLayer;
        }

        private static VectorLayer ReadPolylineMShapes(BinaryReader br, int shapeNum)
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.PolylineM);
            double x, y;
            //PointD aPoint;
            for (int i = 0; i < shapeNum; i++)
            {
                br.ReadBytes(12);

                //Read bounding box
                PolylineMShape aPL = new PolylineMShape();
                Extent extent;
                extent.minX = br.ReadDouble();
                extent.minY = br.ReadDouble();
                extent.maxX = br.ReadDouble();
                extent.maxY = br.ReadDouble();
                aPL.Extent = extent;

                aPL.PartNum = br.ReadInt32();
                int numPoints = br.ReadInt32();
                aPL.parts = new int[aPL.PartNum];
                List<PointD> points = new List<PointD>();

                //firstly read out parts begin position in file 
                for (int j = 0; j < aPL.PartNum; j++)
                {
                    aPL.parts[j] = br.ReadInt32();
                }

                //read out coordinates 
                for (int j = 0; j < numPoints; j++)
                {
                    x = br.ReadDouble();
                    y = br.ReadDouble();
                    PointD aPoint = new PointD();
                    aPoint.X = x;
                    aPoint.Y = y;
                    points.Add(aPoint);
                }

                //Read measure
                double mmin = br.ReadDouble();
                double mmax = br.ReadDouble();
                double[] mArray = new double[numPoints];
                for (int j = 0; j < numPoints; j++)
                    mArray[j] = br.ReadDouble();

                //Get pointM list
                List<PointD> pointMs = new List<PointD>();
                for (int j = 0; j < numPoints; j++)
                    pointMs.Add(new PointM(points[j].X, points[j].Y, mArray[j]));

                aPL.Points = pointMs;
                aLayer.ShapeList.Add(aPL);
            }

            //Create legend scheme            
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
            return aLayer;
        }

        private static VectorLayer ReadPolylineZShapes(BinaryReader br, int shapeNum)
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.PolylineZ);
            double x, y;
            //PointD aPoint;
            for (int i = 0; i < shapeNum; i++)
            {                
                br.ReadBytes(12);                

                //Read bounding box
                PolylineZShape aPL = new PolylineZShape();
                Extent extent;
                extent.minX = br.ReadDouble();
                extent.minY = br.ReadDouble();
                extent.maxX = br.ReadDouble();
                extent.maxY = br.ReadDouble();
                aPL.Extent = extent;

                aPL.PartNum = br.ReadInt32();
                int numPoints = br.ReadInt32();
                aPL.parts = new int[aPL.PartNum];
                List<PointD> points = new List<PointD>();

                //firstly read out parts begin position in file 
                for (int j = 0; j < aPL.PartNum; j++)
                {
                    aPL.parts[j] = br.ReadInt32();
                }

                //read out coordinates 
                for (int j = 0; j < numPoints; j++)
                {
                    x = br.ReadDouble();
                    y = br.ReadDouble();
                    PointD aPoint = new PointD();
                    aPoint.X = x;
                    aPoint.Y = y;
                    points.Add(aPoint);
                }
                //aPL.Points = points;

                ////Read Z
                //aPL.ZRange[0] = br.ReadDouble();
                //aPL.ZRange[1] = br.ReadDouble();
                //aPL.ZArray = new double[aPL.numPoints];
                //for (int j = 0; j < aPL.numPoints; j++)
                //    aPL.ZArray[j] = br.ReadDouble();

                ////Read measure
                //aPL.MRange[0] = br.ReadDouble();
                //aPL.MRange[1] = br.ReadDouble();
                //aPL.MArray = new double[aPL.numPoints];
                //for (int j = 0; j < aPL.numPoints; j++)
                //    aPL.MArray[j] = br.ReadDouble();

                //Read Z
                double zmin = br.ReadDouble();
                double zmax = br.ReadDouble();
                double[] zArray = new double[numPoints];
                for (int j = 0; j < numPoints; j++)
                    zArray[j] = br.ReadDouble();

                //Read measure
                double mmin = br.ReadDouble();
                double mmax = br.ReadDouble();
                double[] mArray = new double[numPoints];
                for (int j = 0; j < numPoints; j++)
                    mArray[j] = br.ReadDouble();

                //Get pointZ list
                List<PointD> pointZs = new List<PointD>();
                for (int j = 0; j < numPoints; j++)
                    pointZs.Add(new PointZ(points[j].X, points[j].Y, zArray[j], mArray[j]));

                aPL.Points = pointZs;
                aLayer.ShapeList.Add(aPL);
            }

            //Create legend scheme            
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polyline, Color.DarkGray, 1.0F);
            return aLayer;
        }

        private static VectorLayer ReadPolygonShapes(BinaryReader br, int shapeNum)
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polygon);
            double x, y;
            //PointD aPoint;

            for (int i = 0; i < shapeNum; i++)
            {
                //geolayer.getAttributeContainer().Add(ds.Tables[0].Rows[i][0]);

                /*  bb0.rewind(); 
                  bb0.position(indexRecs[i].Offset + 8); 
                  stype = BinaryFile.ReadInt32(); 
 
                  if (stype != shapetype) 
                  { 
                      continue; 
                  }*/

                br.ReadBytes(12);

                PolygonShape aSPG = new PolygonShape();
                Extent extent;
                extent.minX = br.ReadDouble();
                extent.minY = br.ReadDouble();
                extent.maxX = br.ReadDouble();
                extent.maxY = br.ReadDouble();
                aSPG.Extent = extent;
                aSPG.PartNum = br.ReadInt32();
                int numPoints = br.ReadInt32();
                aSPG.parts = new int[aSPG.PartNum];
                List<PointD> points = new List<PointD>();

                //firstly read out parts begin pos in file 
                for (int j = 0; j < aSPG.PartNum; j++)
                {
                    aSPG.parts[j] = br.ReadInt32();
                }

                //read out coordinates 
                for (int j = 0; j < numPoints; j++)
                {
                    x = br.ReadDouble();
                    y = br.ReadDouble();
                    PointD aPoint = new PointD();
                    aPoint.X = x;
                    aPoint.Y = y;
                    points.Add(aPoint);
                }
                aSPG.Points = points;
                aLayer.ShapeList.Add(aSPG);
            }

            //Create legend scheme            
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polygon, Color.FromArgb(255, 251, 195), 1.0F);
            return aLayer;
        }

        private static VectorLayer ReadPolygonMShapes(BinaryReader br, int shapeNum)
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.PolygonM);
            double x, y;
            //PointD aPoint;

            for (int i = 0; i < shapeNum; i++)
            {
                //geolayer.getAttributeContainer().Add(ds.Tables[0].Rows[i][0]);

                /*  bb0.rewind(); 
                  bb0.position(indexRecs[i].Offset + 8); 
                  stype = BinaryFile.ReadInt32(); 
 
                  if (stype != shapetype) 
                  { 
                      continue; 
                  }*/

                br.ReadBytes(12);

                PolygonMShape aSPG = new PolygonMShape();
                Extent extent;
                extent.minX = br.ReadDouble();
                extent.minY = br.ReadDouble();
                extent.maxX = br.ReadDouble();
                extent.maxY = br.ReadDouble();
                aSPG.Extent = extent;
                aSPG.PartNum = br.ReadInt32();
                int numPoints = br.ReadInt32();
                aSPG.parts = new int[aSPG.PartNum];
                List<PointD> points = new List<PointD>();

                //firstly read out parts begin pos in file 
                for (int j = 0; j < aSPG.PartNum; j++)
                {
                    aSPG.parts[j] = br.ReadInt32();
                }

                //read out coordinates 
                for (int j = 0; j < numPoints; j++)
                {
                    x = br.ReadDouble();
                    y = br.ReadDouble();
                    PointD aPoint = new PointD();
                    aPoint.X = x;
                    aPoint.Y = y;
                    points.Add(aPoint);
                }

                //Read measure
                double mmin = br.ReadDouble();
                double mmax = br.ReadDouble();
                double[] mArray = new double[numPoints];
                for (int j = 0; j < numPoints; j++)
                    mArray[j] = br.ReadDouble();

                //Get pointM list
                List<PointD> pointMs = new List<PointD>();
                for (int j = 0; j < numPoints; j++)
                    pointMs.Add(new PointM(points[j].X, points[j].Y, mArray[j]));

                aSPG.Points = pointMs;
                aLayer.ShapeList.Add(aSPG);
            }

            //Create legend scheme            
            aLayer.LegendScheme = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Polygon, Color.FromArgb(255, 251, 195), 1.0F);
            return aLayer;
        }

        private static void LoadShxFile(string shxfilepath)
        {
            FileStream fs = new FileStream(shxfilepath, FileMode.Open, FileAccess.Read);
            long BytesSum = fs.Length;  //Get file byte length   
            int shapeNum = (int)(BytesSum - 100) / 8;  //Get total number of records   
            BinaryReader bridx = new BinaryReader(fs);
            ReadHeader(bridx);

            int OffSet = 0, ContentLength = 0;
            for (int i = 0; i < shapeNum; i++)
            {
                OffSet = SwapByteOrder(bridx.ReadInt32());
                ContentLength = SwapByteOrder(bridx.ReadInt32());
            }

            bridx.Close();
            fs.Close();
        }

        private static void LoadProjFile(string projFilePath, ref VectorLayer aLayer)
        {
            ProjectionInfo aProjInfo = new ProjectionInfo();
            StreamReader sr = new StreamReader(projFilePath);
            string esriString = sr.ReadToEnd();
            sr.Close();

            aProjInfo.ReadEsriString(esriString);
            if (aProjInfo.Transform.Proj4Name == "lonlat")
                aProjInfo = KnownCoordinateSystems.Geographic.World.WGS1984;
            aLayer.ProjInfo = aProjInfo;
        }

        /// <summary>
        /// Save shape file
        /// </summary>
        /// <param name="shpfilepath">shape file path</param>
        /// <param name="aLayer">vectorlayer</param>  
        /// <returns>if saved</returns>      
        public static bool SaveShapeFile(string shpfilepath, VectorLayer aLayer)
        {
            string dataDir = Path.GetDirectoryName(shpfilepath);
            string shpfilename = Path.GetFileName(shpfilepath);
            string shxfilepath = shpfilepath.Replace(Path.GetExtension(shpfilepath), ".shx");
            string dbffilepath = shpfilepath.Replace(Path.GetExtension(shpfilepath), ".dbf");
            string projFilePath = shpfilepath.Replace(Path.GetExtension(shpfilepath), ".prj");
            //if (!File.Exists(shxfilepath))
            //{
            //    shxfilepath = shxfilepath.Replace(".shx", ".SHX");
            //}
            //if (!File.Exists(dbffilepath))
            //{
            //    dbffilepath = dbffilepath.Replace(".dbf", ".DBF");
            //}

            switch (aLayer.ShapeType)
            {
                case ShapeTypes.Point:
                case ShapeTypes .PointZ :
                case ShapeTypes.Polyline:
                case ShapeTypes .PolylineZ :
                case ShapeTypes.Polygon:
                    WriteShxFile(shxfilepath, aLayer);
                    WriteShpFile(shpfilepath, aLayer);
                    WriteDbfFile(dbffilepath, aLayer);
                    WriteProjFile(projFilePath, aLayer);
                    return true;                

                default:
                    return false;                    
            }            
        }

        private static void WriteShpFile(string shpfilepath, VectorLayer aLayer)
        {
            FileStream fs = new FileStream(shpfilepath, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            //Write header
            int FileLength = GetShpFileLength(aLayer);
            WriteHeader(bw, aLayer, FileLength);

            //Write records
            int RecordNumber;

            for (int i = 0; i < aLayer.ShapeNum; i++)
            {
                object aShape = aLayer.ShapeList[i];
                RecordNumber = i + 1;
                WriteRecord(bw, RecordNumber, aShape, aLayer.ShapeType);
            }                        

            //Close
            bw.Close();
            fs.Close();
        }

        private static int GetShpFileLength(VectorLayer aLayer)
        {
            int fileLength = 50;

            for (int i = 0; i < aLayer.ShapeNum; i++)
            {
                object aShape = aLayer.ShapeList[i];
                int cLen = GetContentLength(aShape, aLayer.ShapeType);
                fileLength += 4 + cLen;
            }            

            return fileLength;
        }

        private static int GetContentLength(object aShape, ShapeTypes aST)
        {
            int contentLength = 0;
            switch (aST)
            {
                case ShapeTypes.Point:
                    contentLength = 2 + 4 * 2;
                    break;
                case ShapeTypes.Polyline:
                    PolylineShape aPLS = (PolylineShape)aShape;
                    contentLength = 2 + 4 * 4 + 2 + 2 + 2 * aPLS.PartNum + 4 * 2 * aPLS.PointNum;
                    break;
                case ShapeTypes.PolylineZ :
                    PolylineZShape aPLZS = (PolylineZShape)aShape;
                    contentLength = 2 + 4 * 4 + 2 + 2 + 2 * aPLZS.PartNum + 4 * 2 * aPLZS.PointNum +
                        4 + 4 + 4 * aPLZS.PointNum + 4 + 4 + 4 * aPLZS.PointNum;
                    break;
                case ShapeTypes.Polygon:
                    PolygonShape aPGS = (PolygonShape)aShape;
                    contentLength = 2 + 4 * 4 + 2 + 2 + 2 * aPGS.PartNum + 4 * 2 * aPGS.PointNum;
                    break;
            }

            return contentLength;
        }

        private static void WriteRecord(BinaryWriter bw, int RecordNumber, object aShape, ShapeTypes aST)
        {
            int ContentLength, i;

            ContentLength = GetContentLength(aShape, aST);
            bw.Write(SwapByteOrder(RecordNumber));
            bw.Write(SwapByteOrder(ContentLength));
            bw.Write((int)aST);
            switch (aST)
            {
                case ShapeTypes.Point:
                    PointShape aPS = (PointShape)aShape;                    
                    bw.Write(aPS.Point.X);
                    bw.Write(aPS.Point.Y);
                    break;
                case ShapeTypes.Polyline:
                    PolylineShape aPLS = (PolylineShape)aShape;
                    bw.Write(aPLS.Extent.minX);
                    bw.Write(aPLS.Extent.minY);
                    bw.Write(aPLS.Extent.maxX);
                    bw.Write(aPLS.Extent.maxY);
                    bw.Write(aPLS.PartNum);
                    bw.Write(aPLS.PointNum);
                    for (i = 0; i < aPLS.PartNum; i++)
                        bw.Write(aPLS.parts[i]);
                    for (i = 0; i < aPLS.PointNum; i++)
                    {
                        bw.Write(((PointD)aPLS.Points[i]).X);
                        bw.Write(((PointD)aPLS.Points[i]).Y);
                    }
                    break;
                case ShapeTypes.PolylineZ :
                    PolylineZShape aPLZS = (PolylineZShape)aShape;
                    bw.Write(aPLZS.Extent.minX);
                    bw.Write(aPLZS.Extent.minY);
                    bw.Write(aPLZS.Extent.maxX);
                    bw.Write(aPLZS.Extent.maxY);
                    bw.Write(aPLZS.PartNum);
                    bw.Write(aPLZS.PointNum);
                    for (i = 0; i < aPLZS.PartNum; i++)
                        bw.Write(aPLZS.parts[i]);
                    for (i = 0; i < aPLZS.PointNum; i++)
                    {
                        bw.Write(((PointZ)aPLZS.Points[i]).X);
                        bw.Write(((PointZ)aPLZS.Points[i]).Y);
                    }
                    bw.Write(aPLZS.ZRange[0]);
                    bw.Write(aPLZS.ZRange[1]);
                    for (i = 0; i < aPLZS.PointNum; i++)
                        bw.Write(aPLZS.ZArray[i]);
                    bw.Write(aPLZS.MRange[0]);
                    bw.Write(aPLZS.MRange[1]);
                    for (i = 0; i < aPLZS.PointNum; i++)
                        bw.Write(aPLZS.MArray[i]);

                    break;
                case ShapeTypes.Polygon:
                    PolygonShape aPGS = (PolygonShape)aShape;
                    bw.Write(aPGS.Extent.minX);
                    bw.Write(aPGS.Extent.minY);
                    bw.Write(aPGS.Extent.maxX);
                    bw.Write(aPGS.Extent.maxY);
                    bw.Write(aPGS.PartNum);
                    bw.Write(aPGS.PointNum);
                    for (i = 0; i < aPGS.PartNum; i++)
                        bw.Write(aPGS.parts[i]);
                    for (i = 0; i < aPGS.PointNum; i++)
                    {
                        bw.Write(((PointD)aPGS.Points[i]).X);
                        bw.Write(((PointD)aPGS.Points[i]).Y);
                    }
                    break;
            }
        }

        private static void WriteHeader(BinaryWriter bw, VectorLayer aLayer, int FileLength)
        {
            int i;
            int FileCode = 9994;
            FileCode = SwapByteOrder(FileCode);
            int Unused = 0;
            Unused = SwapByteOrder(Unused);            
            FileLength = SwapByteOrder(FileLength);
            int Version = 1000;
            int aShapeType = (int)aLayer.ShapeType;

            bw.Write(FileCode);
            for (i = 0; i < 5; i++)
            {
                bw.Write(Unused);
            }
            bw.Write(FileLength);
            bw.Write(Version);
            bw.Write(aShapeType);
            bw.Write(aLayer.Extent.minX);
            bw.Write(aLayer.Extent.minY);
            bw.Write(aLayer.Extent.maxX);
            bw.Write(aLayer.Extent.maxY);
            for (i = 0; i < 4; i++)
            {
                bw.Write(0.0);
            }
        }

        private static void WriteShxFile(string shxfilepath, VectorLayer aLayer)
        {
            FileStream fs = new FileStream(shxfilepath, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            //Write header
            int FileLength = aLayer.ShapeNum * 4 + 50;
            WriteHeader(bw, aLayer, FileLength);

            //Write content
            int OffSet, ContentLength;
            OffSet = 50;

            for (int i = 0; i < aLayer.ShapeNum; i++)
            {
                object aShape = aLayer.ShapeList[i];
                ContentLength = GetContentLength(aShape, aLayer.ShapeType);
                
                bw.Write(SwapByteOrder(OffSet));
                bw.Write(SwapByteOrder(ContentLength));

                OffSet = OffSet + 4 + ContentLength;
            }            

            //Close
            bw.Close();
            fs.Close();
        }

        private static void WriteDbfFile(string dbffilepath, VectorLayer aLayer)
        {
            aLayer.AttributeTable.SaveAs(dbffilepath, true);
        }

        private static void WriteProjFile(string projFilePath, VectorLayer aLayer)
        {
            string esriString = aLayer.ProjInfo.ToEsriString();
            StreamWriter sw = new StreamWriter(projFilePath);
            sw.Write(esriString);
            sw.Close();
        }
        
        ///<summary>
        ///Swaps the byte order of an int32
        ///</summary>
        /// <param name="i">Integer to swap</param>
        /// <returns>Byte Order swapped int32</returns>
        private static int SwapByteOrder(int i)
        {
            byte[] buffer = BitConverter.GetBytes(i);
            Array.Reverse(buffer, 0, buffer.Length);
            return BitConverter.ToInt32(buffer, 0);
        }

        #endregion
    }
}