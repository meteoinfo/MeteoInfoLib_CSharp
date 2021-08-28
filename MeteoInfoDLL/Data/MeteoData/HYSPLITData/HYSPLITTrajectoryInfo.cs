using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Drawing;
using MeteoInfoC.Layer;
using MeteoInfoC.Shape;
using MeteoInfoC.Legend;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{   
    /// <summary>
    /// HYSPLIT trajectory data info
    /// </summary>
    public class HYSPLITTrajectoryInfo : DataInfo,ITrajDataInfo
    {
        #region Variables
        /// <summary>
        /// File name
        /// </summary>
        public List<string> FileNames;
        /// <summary>
        /// Number of meteorological files
        /// </summary>
        public List<int> MeteoFileNums;
        /// <summary>
        /// Number of trajectories
        /// </summary>
        public int TrajeoryNumber;
        /// <summary>
        /// Number of trajectories
        /// </summary>
        public List<int> TrajeoryNums;
        /// <summary>
        /// Trajectory direction - foreward or backward
        /// </summary>
        public List<string> TrajDirections;
        /// <summary>
        /// Vertical motion
        /// </summary>
        public List<string> VerticalMotions;
        /// <summary>
        /// Information list of trajectories
        /// </summary>
        public List<List<TrajectoryInfo>> TrajInfos;
        /// <summary>
        /// Number of variables
        /// </summary>
        public List<int> VarNums;
        /// <summary>
        /// Variable name list
        /// </summary>
        public List<List<string>> VarNames;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public HYSPLITTrajectoryInfo()
        {
            InitVariables();
        }

        private void InitVariables()
        {
            FileNames = new List<string>();
            MeteoFileNums = new List<int>();
            TrajeoryNums = new List<int>();
            TrajDirections = new List<string>();
            VerticalMotions = new List<string>();
            TrajInfos = new List<List<TrajectoryInfo>>();
            VarNums = new List<int>();
            VarNames = new List<List<string>>();
            TrajeoryNumber = 0;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Read data info - one file
        /// </summary>
        /// <param name="aFile">file path</param>
        public override void ReadDataInfo(string aFile)
        {
            string[] trajFiles = new string[1];
            trajFiles[0] = aFile;
            ReadDataInfo(trajFiles);
        }

        /// <summary>
        /// Read HYSPLIT trajectory data info
        /// </summary>
        /// <param name="TrajFiles">File paths</param>        
        public void ReadDataInfo(string[] TrajFiles)
        {
            this.FileName = TrajFiles[0];
            string aLine;
            string[] dataArray;
            List<string> dataList = new List<string>();
            int i, j, t, LastNonEmpty;
            DateTime aDateTime = new DateTime();

            InitVariables();
            List<double> times = new List<double>();

            for (t = 0; t < TrajFiles.Length; t++)
            {
                string aFile = TrajFiles[t];
                FileNames.Add(aFile);

                FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                //Record #1
                aLine = sr.ReadLine();
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
                MeteoFileNums.Add(int.Parse(dataList[0]));

                //Record #2
                for (i = 0; i < MeteoFileNums[t]; i++)
                {
                    aLine = sr.ReadLine();
                }

                //Record #3
                aLine = sr.ReadLine();
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
                TrajeoryNums.Add(int.Parse(dataList[0]));
                TrajeoryNumber += TrajeoryNums[t];
                TrajDirections.Add (dataList[1]);
                VerticalMotions.Add ( dataList[2]);

                //Record #4  
                TrajectoryInfo aTrajInfo = new TrajectoryInfo();
                List<TrajectoryInfo> trajInfoList = new List<TrajectoryInfo>();
                for (i = 0; i < TrajeoryNums[t]; i++)
                {
                    aLine = sr.ReadLine();
                    dataArray = aLine.Split();
                    LastNonEmpty = -1;
                    dataList.Clear();
                    for (j = 0; j < dataArray.Length; j++)
                    {
                        if (dataArray[j] != string.Empty)
                        {
                            LastNonEmpty++;
                            dataList.Add(dataArray[j]);
                        }
                    }
                    int y = int.Parse(dataList[0]);
                    if (y < 100)
                    {
                        if (y > 50)
                            y = 1900 + y;
                        else
                            y = 2000 + y;
                    }
                    aDateTime = new DateTime(y, int.Parse(dataList[1]), 
                        int.Parse(dataList[2]), int.Parse(dataList[3]), 0, 0);
                    //aDateTime = DateTime.Parse(dataList[0] + "-" + dataList[1] + "-" +
                    //    dataList[2] + " " + dataList[3] + ":00");
                    if (times.Count == 0)
                        times.Add(DataConvert.ToDouble(aDateTime));

                    aTrajInfo = new TrajectoryInfo();
                    aTrajInfo.StartTime = aDateTime;
                    aTrajInfo.StartLat = Single.Parse(dataList[4]);
                    aTrajInfo.StartLon = Single.Parse(dataList[5]);
                    aTrajInfo.StartHeight = Single.Parse(dataList[6]);
                    trajInfoList.Add(aTrajInfo);                   
                }
                TrajInfos.Add(trajInfoList);
                Dimension tdim = new Dimension(DimensionType.T);
                tdim.SetValues(times);

                //Record #5
                aLine = sr.ReadLine();
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
                VarNums.Add (int.Parse(dataList[0]));
                List<string> varNameList = new List<string>();                
                for (i = 0; i < VarNums[t]; i++)
                {
                    varNameList.Add(dataList[i + 1]);                    
                }
                VarNames.Add(varNameList);

                Variable var = new Variable();
                var.Name = "Traj";
                var.IsStation = true;
                var.SetDimension(tdim);
                List<Variable> variables = new List<Variable>();
                variables.Add(var);
                this.Variables = variables;

                sr.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// Read HYSPLIT trajectory data and create a polyline layer
        /// </summary>        
        /// <returns>vector layer</returns>
        public VectorLayer CreateTrajLineLayer()
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);
            aLayer.EditAddField(new System.Data.DataColumn("TrajID", typeof(int)));
            aLayer.EditAddField(new System.Data.DataColumn("StartDate", typeof(string)));
            aLayer.EditAddField(new System.Data.DataColumn("StartLon", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("StartLat", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("StartHeight", typeof(double)));

            int TrajNum = 0;
            for (int t = 0; t < FileNames.Count; t++)
            {
                string aFile = FileNames[t];
                FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string aLine;
                string[] dataArray;
                List<string> dataList = new List<string>();
                int i, LastNonEmpty;                

                //Record #1
                aLine = sr.ReadLine();

                //Record #2
                for (i = 0; i < MeteoFileNums[t]; i++)
                {
                    aLine = sr.ReadLine();
                }

                //Record #3
                aLine = sr.ReadLine();

                //Record #4             
                for (i = 0; i < TrajeoryNums[t]; i++)
                {
                    aLine = sr.ReadLine();
                }

                //Record #5
                aLine = sr.ReadLine();

                //Record #6
                int TrajIdx;
                List<PointD> pList = new List<PointD>();
                List<List<PointD>> PointList = new List<List<PointD>>();
                for (i = 0; i < TrajeoryNums[t]; i++)
                {
                    pList = new List<PointD>();
                    PointList.Add(pList);
                }
                PointD aPoint = new PointD();
                ArrayList polylines = new ArrayList();                    
                while (true)
                {
                    aLine = sr.ReadLine();
                    if (aLine == null || aLine == "")
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
                    TrajIdx = int.Parse(dataList[0]) - 1;
                    aPoint = new PointD();
                    aPoint.X = double.Parse(dataList[10]);
                    aPoint.Y = double.Parse(dataList[9]);

                    if (PointList[TrajIdx].Count > 1)
                    {
                        PointD oldPoint = PointList[TrajIdx][PointList[TrajIdx].Count - 1];
                        if (Math.Abs(aPoint.X - oldPoint.X) > 100)
                        {
                            if (aPoint.X > oldPoint.X)
                                aPoint.X -= 360;
                            else
                                aPoint.X += 360;
                        }
                    }
                    PointList[TrajIdx].Add(aPoint);                        
                }

                for (i = 0; i < TrajeoryNums[t]; i++)
                {
                    Shape.PolylineShape aPolyline = new Shape.PolylineShape();
                    //aPolyline.value = aDataInfo.TrajInfos[i].StartTime.ToBinary();
                    TrajNum += 1;
                    aPolyline.value = TrajNum;
                    aPolyline.Points = PointList[i];
                    aPolyline.Extent = MIMath.GetPointsExtent(aPolyline.Points);

                    int shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aPolyline, shapeNum))
                    {
                        aLayer.EditCellValue("TrajID", shapeNum, TrajNum);
                        aLayer.EditCellValue("StartDate", shapeNum, TrajInfos[t][i].StartTime.ToString("yyyyMMddHH"));
                        aLayer.EditCellValue("StartLat", shapeNum, TrajInfos[t][i].StartLat);
                        aLayer.EditCellValue("StartLon", shapeNum, TrajInfos[t][i].StartLon);
                        aLayer.EditCellValue("StartHeight", shapeNum, TrajInfos[t][i].StartHeight);
                    }
                }

                sr.Close();
                fs.Close();
            }

            aLayer.LayerName = "Trajectory_Lines";
            aLayer.LayerDrawType = LayerDrawType.TrajLine;
            //aLayer.LegendScheme = m_Legend.CreateSingleSymbolLegendScheme(Shape.ShapeType.Polyline, Color.Blue, 1.0F, 1, aDataInfo.TrajeoryNum);            
            aLayer.Visible = true;            
            LegendScheme aLS = LegendManage.CreateUniqValueLegendScheme(aLayer, 1, TrajeoryNumber);           
            aLS.FieldName = "TrajID";
            aLayer.LegendScheme = aLS;

            return aLayer;
        }

        /// <summary>
        /// Read HYSPLIT trajectory data and create a trajectory start point layer
        /// </summary>        
        /// <returns>vector layer</returns>
        public VectorLayer CreateTrajStartPointLayer()
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            aLayer.EditAddField(new System.Data.DataColumn("TrajID", typeof(int)));
            aLayer.EditAddField(new System.Data.DataColumn("StartDate", typeof(string)));
            aLayer.EditAddField(new System.Data.DataColumn("StartLon", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("StartLat", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("StartHeight", typeof(double)));

            int TrajNum = 0;
            for (int t = 0; t < FileNames.Count; t++)
            {
                string aFile = FileNames[t];
                FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string aLine;
                string[] dataArray;
                List<string> dataList = new List<string>();
                int i, LastNonEmpty;

                //Record #1
                aLine = sr.ReadLine();

                //Record #2
                for (i = 0; i < MeteoFileNums[t]; i++)
                {
                    aLine = sr.ReadLine();
                }

                //Record #3
                aLine = sr.ReadLine();

                //Record #4             
                for (i = 0; i < TrajeoryNums[t]; i++)
                {
                    aLine = sr.ReadLine();
                }

                //Record #5
                aLine = sr.ReadLine();

                //Record #6
                int TrajIdx;
                List<object> pList = new List<object>();
                List<PointD> PointList = new List<PointD>();
                PointD aPoint = new PointD();
                for (i = 0; i < TrajeoryNums[t]; i++)
                {                    
                    PointList.Add(aPoint);
                }
                
                ArrayList polylines = new ArrayList();                             
                while (true)
                {
                    aLine = sr.ReadLine();
                    if (aLine == null || aLine == "")
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
                    if (float.Parse(dataList[8]) == 0)
                    {
                        TrajIdx = int.Parse(dataList[0]) - 1;
                        aPoint = new PointD();
                        aPoint.X = double.Parse(dataList[10]);
                        aPoint.Y = double.Parse(dataList[9]);
                        PointList[TrajIdx] = aPoint;
                    }
                }

                for (i = 0; i < TrajeoryNums[t]; i++)
                {
                    PointShape aPS = new PointShape();                    
                    TrajNum += 1;
                    aPS.Value = TrajNum;
                    aPS.Point = PointList[i];
                    
                    int shapeNum = aLayer.ShapeNum;
                    if (aLayer.EditInsertShape(aPS, shapeNum))
                    {
                        aLayer.EditCellValue("TrajID", shapeNum, TrajNum);
                        aLayer.EditCellValue("StartDate", shapeNum, TrajInfos[t][i].StartTime.ToString("yyyyMMddHH"));
                        aLayer.EditCellValue("StartLat", shapeNum, TrajInfos[t][i].StartLat);
                        aLayer.EditCellValue("StartLon", shapeNum, TrajInfos[t][i].StartLon);
                        aLayer.EditCellValue("StartHeight", shapeNum, TrajInfos[t][i].StartHeight);
                    }
                }

                sr.Close();
                fs.Close();
            }

            aLayer.LayerName = "Trajectory_Start_Points";
            aLayer.LayerDrawType = LayerDrawType.TrajPoint;             
            aLayer.Visible = true;            
            LegendScheme aLS = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Black, 8.0F);
            aLS.FieldName = "TrajID";
            aLayer.LegendScheme = aLS;

            return aLayer;
        }

        /// <summary>
        /// Read HYSPLIT trajectory data and create a trajectory point layer
        /// </summary>        
        /// <returns>vector layer</returns>
        public VectorLayer CreateTrajPointLayer()
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            aLayer.EditAddField(new System.Data.DataColumn("TrajID", typeof(int)));
            aLayer.EditAddField(new System.Data.DataColumn("Date", typeof(string)));
            aLayer.EditAddField(new System.Data.DataColumn("Lon", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("Lat", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("Height", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("Pressure", typeof(double)));
            bool isMultiVar = false;
            if (VarNums[0] > 1)
            {
                isMultiVar = true;
                for (int v = 1; v < VarNums[0]; v++)
                    aLayer.EditAddField(new System.Data.DataColumn(VarNames[0][v], typeof(double)));
            }

            int TrajNum = 0;
            for (int t = 0; t < FileNames.Count; t++)
            {
                string aFile = FileNames[t];
                FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string aLine;
                string[] dataArray;
                List<string> dataList = new List<string>();
                int i, LastNonEmpty;

                //Record #1
                aLine = sr.ReadLine();

                //Record #2
                for (i = 0; i < MeteoFileNums[t]; i++)
                {
                    aLine = sr.ReadLine();
                }

                //Record #3
                aLine = sr.ReadLine();

                //Record #4             
                for (i = 0; i < TrajeoryNums[t]; i++)
                {
                    aLine = sr.ReadLine();
                }

                //Record #5
                aLine = sr.ReadLine();

                //Record #6
                int TrajIdx;
                List<List<object>> pList = new List<List<object>>();
                List<List<List<object>>> PointList = new List<List<List<object>>>();
                for (i = 0; i < TrajeoryNums[t]; i++)
                {
                    pList = new List<List<object>>();
                    PointList.Add(pList);
                }
                PointD aPoint = new PointD();
                ArrayList polylines = new ArrayList();               
                DateTime aDate;
                double Height, Press;
                while (true)
                {
                    aLine = sr.ReadLine();
                    if (aLine == null || aLine == "")
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
                    List<object> dList = new List<object>();
                    TrajIdx = int.Parse(dataList[0]) - 1;
                    aDate = DateTime.Parse(dataList[2] + "-" + dataList[3] + "-" +
                        dataList[4] + " " + dataList[5] + ":00");
                    aPoint = new PointD();
                    aPoint.X = double.Parse(dataList[10]);
                    aPoint.Y = double.Parse(dataList[9]);
                    Height = double.Parse(dataList[11]);
                    Press = double.Parse(dataList[12]);
                    dList.Add(aPoint);
                    dList.Add(aDate);
                    dList.Add(Height);
                    dList.Add(Press);
                    if (isMultiVar)
                    {
                        for (i = 13; i < dataList.Count; i++)
                            dList.Add(double.Parse(dataList[i]));
                    }
                    PointList[TrajIdx].Add(dList);
                }

                for (i = 0; i < TrajeoryNums[t]; i++)
                {
                    TrajNum += 1;
                    for (int j = 0; j < PointList[i].Count; j++)
                    {                        
                        PointShape aPS = new PointShape();
                        aPS.Value = TrajNum;
                        aPS.Point = (PointD)PointList[i][j][0];
                        int shapeNum = aLayer.ShapeNum;
                        if (aLayer.EditInsertShape(aPS, shapeNum))
                        {
                            aLayer.EditCellValue("TrajID", shapeNum, TrajNum);
                            aLayer.EditCellValue("Date", shapeNum, ((DateTime)PointList[i][j][1]).ToString("yyyyMMddHH"));
                            aLayer.EditCellValue("Lat", shapeNum, aPS.Point.Y);
                            aLayer.EditCellValue("Lon", shapeNum, aPS.Point.X);
                            aLayer.EditCellValue("Height", shapeNum, (double)PointList[i][j][2]);
                            aLayer.EditCellValue("Pressure", shapeNum, (double)PointList[i][j][3]);
                            if (isMultiVar)
                            {
                                for (int v = 1; v < VarNums[0]; v++)
                                    aLayer.EditCellValue(VarNames[0][v], shapeNum, (double)PointList[i][j][3 + v]);
                            }
                        }
                    }
                }

                sr.Close();
                fs.Close();
            }

            aLayer.LayerName = "Trajectory_Points";
            aLayer.LayerDrawType = LayerDrawType.TrajLine;
            //aLayer.LegendScheme = m_Legend.CreateSingleSymbolLegendScheme(Shape.ShapeType.Polyline, Color.Blue, 1.0F, 1, aDataInfo.TrajeoryNum);            
            aLayer.Visible = true;            
            LegendScheme aLS = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Red, 5.0F);
            aLS.FieldName = "TrajID";
            aLayer.LegendScheme = aLS;

            return aLayer;
        }

        /// <summary>
        /// Read one trajectory points data
        /// </summary>        
        /// <returns>A trajectory points data</returns>
        public List<List<object>> GetATrajData(int aTrajIdx)
        {
            List<List<object>> trajPointsData = new List<List<object>>();
            
            for (int t = 0; t < FileNames.Count; t++)
            {
                string aFile = FileNames[t];
                FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string aLine;
                string[] dataArray;
                List<string> dataList = new List<string>();
                int i, LastNonEmpty;

                //Record #1
                aLine = sr.ReadLine();

                //Record #2
                for (i = 0; i < MeteoFileNums[t]; i++)
                {
                    aLine = sr.ReadLine();
                }

                //Record #3
                aLine = sr.ReadLine();

                //Record #4             
                for (i = 0; i < TrajeoryNums[t]; i++)
                {
                    aLine = sr.ReadLine();
                }

                //Record #5
                aLine = sr.ReadLine();

                //Record #6
                int TrajIdx;
                List<List<object>> pList = new List<List<object>>();                
                PointD aPoint = new PointD();
                ArrayList polylines = new ArrayList();                
                DateTime aDate;
                double Height, Press;
                while (true)
                {
                    aLine = sr.ReadLine();
                    if (aLine == null || aLine == "")
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
                    List<object> dList = new List<object>();
                    TrajIdx = int.Parse(dataList[0]) - 1;
                    if (TrajIdx == aTrajIdx)
                    {
                        aDate = DateTime.Parse(dataList[2] + "-" + dataList[3] + "-" +
                            dataList[4] + " " + dataList[5] + ":00");
                        aPoint = new PointD();
                        aPoint.X = double.Parse(dataList[10]);
                        aPoint.Y = double.Parse(dataList[9]);
                        Height = double.Parse(dataList[11]);
                        Press = double.Parse(dataList[12]);
                        dList.Add(aPoint);
                        dList.Add(aDate);
                        dList.Add(Height);
                        dList.Add(Press);

                        trajPointsData.Add(dList);
                    }
                }                

                sr.Close();
                fs.Close();
            }            

            return trajPointsData;
        }

        /// <summary>
        /// Get HYSPLIT trajectory data info text
        /// </summary>        
        /// <returns></returns>
        public override string GenerateInfoText()
        {
            string dataInfo = "";
            for (int t = 0; t < FileNames.Count; t++)
            {
                int i;
                dataInfo += "File Name: " + FileNames[t];
                dataInfo += Environment.NewLine + "Trajectory number = " + TrajeoryNums[t].ToString();
                dataInfo += Environment.NewLine + "Trajectory direction = " + TrajDirections[t];
                dataInfo += Environment.NewLine + "Vertical motion =" + VerticalMotions[t];
                dataInfo += Environment.NewLine + "Number of diagnostic output variables = " + VarNums[t].ToString();
                dataInfo += Environment.NewLine + "Variables:";
                for (i = 0; i < VarNums[t]; i++)
                {
                    dataInfo += " " + VarNames[t][i];
                }
                dataInfo += Environment.NewLine + Environment.NewLine + "Trajectories:";
                foreach (TrajectoryInfo aTrajInfo in TrajInfos[t])
                {
                    dataInfo += Environment.NewLine + "  " + aTrajInfo.StartTime.ToString("yyyy-MM-dd HH:00") +
                        "  " + aTrajInfo.StartLat.ToString() + "  " + aTrajInfo.StartLon.ToString() +
                        "  " + aTrajInfo.StartHeight.ToString();
                }

                if (t < FileNames.Count - 1)
                    dataInfo += Environment.NewLine + Environment.NewLine +
                        "******************************" + Environment.NewLine;
            }

            return dataInfo;
        }

        #endregion
    }
}
