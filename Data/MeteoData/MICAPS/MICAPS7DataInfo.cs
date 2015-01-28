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
    /// MICAPS 7 data info (Typhoon)
    /// </summary>
    public class MICAPS7DataInfo : DataInfo,ITrajDataInfo
    {
        #region Variables
        /// <summary>
        /// File name
        /// </summary>
        public List<string> FileNames;        
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
        public MICAPS7DataInfo()
        {
            InitVariables();
        }

        private void InitVariables()
        {
            FileNames = new List<string>();            
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
        /// Read data info
        /// </summary>
        /// <param name="fileName">File name</param>
        public override void ReadDataInfo(string fileName)
        {
            string[] trajFiles = new string[1];
            trajFiles[0] = fileName;
            ReadDataInfo(trajFiles);
        }

        /// <summary>
        /// Read MICAPS 7 data info
        /// </summary>
        /// <param name="TrajFiles">File paths</param>        
        public void ReadDataInfo(string[] TrajFiles)
        {
            this.FileName = TrajFiles[0];
            string aLine;
            string[] dataArray;
            List<string> dataList = new List<string>();
            int j, t, LastNonEmpty;
            DateTime aDateTime = new DateTime();

            InitVariables();
            List<double> times = new List<double>();

            for (t = 0; t < TrajFiles.Length; t++)
            {
                string aFile = TrajFiles[t];
                FileNames.Add(aFile);

                FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);                                

                //  
                TrajectoryInfo aTrajInfo = new TrajectoryInfo();
                List<TrajectoryInfo> trajInfoList = new List<TrajectoryInfo>();
                aLine = sr.ReadLine();
                aLine = sr.ReadLine();
                int trajIdx = -1;
                int trajNum = 0;
                while(aLine != null)
                {
                    if (aLine.Trim() == String.Empty)
                    {
                        aLine = sr.ReadLine();
                        continue;
                    }

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
                    switch (dataList.Count)
                    {
                        case 4:
                            aTrajInfo = new TrajectoryInfo();
                            aTrajInfo.TrajName = dataList[0];
                            aTrajInfo.TrajID = dataList[1];
                            aTrajInfo.TrajCenter = dataList[2];
                            trajIdx = -1;
                            trajNum += 1;
                            break;
                        case 13:
                            trajIdx += 1;
                            if (trajIdx == 0)
                            {
                                //aDateTime = DateTime.Parse(dataList[0] + "-" + dataList[1] + "-" +
                                //    dataList[2] + " " + dataList[3] + ":00");
                                //aDateTime = DateTime.ParseExact(dataList[0] + "-" + dataList[1] + "-" + dataList[2] +
                                //    " " + dataList[3] + ":00", "yy-MM-dd HH:mm", null);
                                int year = int.Parse(dataList[0]);
                                if (year < 100)
                                {
                                    if (year < 50)
                                        year = 2000 + year;
                                    else
                                        year = 1900 + year;
                                }
                                aDateTime = new DateTime(year, int.Parse(dataList[1]), int.Parse(dataList[2]), int.Parse(dataList[3]), 0, 0);
                                if (times.Count == 0)
                                    times.Add(DataConvert.ToDouble(aDateTime));

                                aTrajInfo.StartTime = aDateTime;
                                aTrajInfo.StartLat = Single.Parse(dataList[6]);
                                aTrajInfo.StartLon = Single.Parse(dataList[5]);
                                //aTrajInfo.StartHeight = Single.Parse(dataList[6]);
                                trajInfoList.Add(aTrajInfo);
                            }
                            break;
                    }

                    aLine = sr.ReadLine();
                }
                TrajeoryNums.Add(trajNum);
                TrajeoryNumber += TrajeoryNums[t];
                TrajInfos.Add(trajInfoList);

                Dimension tdim = new Dimension(DimensionType.T);
                tdim.SetValues(times);
                this.TimeDimension = tdim;

                sr.Close();
                fs.Close();

                Variable var = new Variable();
                var.Name = "Traj";
                var.IsStation = true;
                var.SetDimension(tdim);
                List<Variable> variables = new List<Variable>();
                variables.Add(var);
                this.Variables = variables;
            }
        }

        /// <summary>
        /// Read MICAPS 7 data and create a polyline layer
        /// </summary>        
        /// <returns>vector layer</returns>
        public VectorLayer CreateTrajLineLayer()
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Polyline);            
            aLayer.EditAddField(new System.Data.DataColumn("TrajIndex", typeof(int)));
            aLayer.EditAddField(new System.Data.DataColumn("TrajName", typeof(string)));
            aLayer.EditAddField(new System.Data.DataColumn("TrajID", typeof(string)));
            aLayer.EditAddField(new System.Data.DataColumn("TrajCenter", typeof(string)));
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

                int TrajIdx = -1;
                List<PointD> pList = new List<PointD>();
                List<List<PointD>> PointList = new List<List<PointD>>();
                for (i = 0; i < TrajeoryNums[t]; i++)
                {
                    pList = new List<PointD>();
                    PointList.Add(pList);
                }
                PointD aPoint = new PointD();
                ArrayList polylines = new ArrayList();                
                aLine = sr.ReadLine();
                aLine = sr.ReadLine();
                while (aLine != null)
                {                    
                    if (aLine.Trim() == String.Empty)
                    {
                        aLine = sr.ReadLine();
                        continue;
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
                    switch (dataList.Count)
                    {
                        case 4:
                            TrajIdx += 1;
                            break;
                        case 13:
                            aPoint = new PointD();
                            aPoint.X = double.Parse(dataList[5]);
                            aPoint.Y = double.Parse(dataList[6]);
                            PointList[TrajIdx].Add(aPoint);
                            break;
                    }                    

                    aLine = sr.ReadLine();
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
                        aLayer.EditCellValue("TrajIndex", shapeNum, TrajNum);
                        aLayer.EditCellValue("TrajName", shapeNum, TrajInfos[t][i].TrajName);
                        aLayer.EditCellValue("TrajID", shapeNum, TrajInfos[t][i].TrajID);
                        aLayer.EditCellValue("TrajCenter", shapeNum, TrajInfos[t][i].TrajCenter);
                        aLayer.EditCellValue("StartDate", shapeNum, TrajInfos[t][i].StartTime.ToString("yyyyMMddHH"));
                        aLayer.EditCellValue("StartLat", shapeNum, TrajInfos[t][i].StartLat);
                        aLayer.EditCellValue("StartLon", shapeNum, TrajInfos[t][i].StartLon);
                        aLayer.EditCellValue("StartHeight", shapeNum, TrajInfos[t][i].StartHeight);
                    }
                }

                sr.Close();
                fs.Close();
            }

            aLayer.LayerName = "Typhoon_Lines";
            aLayer.LayerDrawType = LayerDrawType.TrajLine;
            //aLayer.LegendScheme = m_Legend.CreateSingleSymbolLegendScheme(Shape.ShapeType.Polyline, Color.Blue, 1.0F, 1, aDataInfo.TrajeoryNum);            
            aLayer.Visible = true;            
            LegendScheme aLS = LegendManage.CreateUniqValueLegendScheme(aLayer, 1, TrajeoryNumber);           
            aLS.FieldName = "TrajIndex";
            aLayer.LegendScheme = aLS;

            return aLayer;
        }

        /// <summary>
        /// Read MICAPS 7 data and create a trajectory start point layer
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

                //
                int TrajIdx = -1;
                List<object> pList = new List<object>();
                List<PointD> PointList = new List<PointD>();
                PointD aPoint = new PointD();
                for (i = 0; i < TrajeoryNums[t]; i++)
                {                    
                    PointList.Add(aPoint);
                }
                
                ArrayList polylines = new ArrayList();                
                aLine = sr.ReadLine();
                aLine = sr.ReadLine();
                bool IsFirstTraj = false;
                while (aLine != null)
                {
                    if (aLine.Trim() == String.Empty)
                    {
                        aLine = sr.ReadLine();
                        continue;
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
                    switch (dataList.Count)
                    {
                        case 4:
                            TrajIdx += 1;
                            IsFirstTraj = true;
                            break;
                        case 13:
                            if (IsFirstTraj)
                            {
                                aPoint = new PointD();
                                aPoint.X = double.Parse(dataList[5]);
                                aPoint.Y = double.Parse(dataList[6]);
                                PointList[TrajIdx] = aPoint;
                                IsFirstTraj = false;
                            }
                            break;
                    }                         

                    aLine = sr.ReadLine();
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

            aLayer.LayerName = "Typhoon_Start_Points";
            aLayer.LayerDrawType = LayerDrawType.TrajPoint;             
            aLayer.Visible = true;            
            LegendScheme aLS = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Black, 8.0F);
            aLS.FieldName = "TrajID";
            aLayer.LegendScheme = aLS;

            return aLayer;
        }

        /// <summary>
        /// Read MICAPS 7 data and create a trajectory point layer
        /// </summary>        
        /// <returns>vector layer</returns>
        public VectorLayer CreateTrajPointLayer()
        {
            VectorLayer aLayer = new VectorLayer(ShapeTypes.Point);
            aLayer.EditAddField(new System.Data.DataColumn("TrajID", typeof(int)));
            aLayer.EditAddField(new System.Data.DataColumn("Date", typeof(string)));
            aLayer.EditAddField(new System.Data.DataColumn("PreHour", typeof(int)));
            aLayer.EditAddField(new System.Data.DataColumn("Lon", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("Lat", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("WindSpeed", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("Radius_W7", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("Radius_W10", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("MoveDir", typeof(double)));
            aLayer.EditAddField(new System.Data.DataColumn("MoveSpeed", typeof(double)));

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

                //
                int TrajIdx = -1;
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
                aLine = sr.ReadLine();
                aLine = sr.ReadLine();
                while (aLine != null)
                {
                    if (aLine.Trim() == String.Empty)
                    {
                        aLine = sr.ReadLine();
                        continue;
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
                    switch (dataList.Count)
                    {
                        case 4:
                            TrajIdx += 1;
                            break;
                        case 13:
                            List<object> dList = new List<object>();                            
                            aDate = DateTime.Parse(dataList[0] + "-" + dataList[1] + "-" +
                                dataList[2] + " " + dataList[3] + ":00");
                            aPoint = new PointD();
                            aPoint.X = double.Parse(dataList[5]);
                            aPoint.Y = double.Parse(dataList[6]);                            
                            dList.Add(aPoint);
                            dList.Add(aDate);
                            dList.Add(int.Parse(dataList[4]));
                            for (int d = 0; d < 5; d++)
                            {
                                dList.Add(double.Parse(dataList[d + 7]));
                            }
                            PointList[TrajIdx].Add(dList);
                            break;
                    }

                    aLine = sr.ReadLine();
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
                            aLayer.EditCellValue("PreHour", shapeNum, (int)PointList[i][j][2]);
                            aLayer.EditCellValue("Lat", shapeNum, aPS.Point.Y);
                            aLayer.EditCellValue("Lon", shapeNum, aPS.Point.X);
                            aLayer.EditCellValue("WindSpeed", shapeNum, (double)PointList[i][j][3]);
                            aLayer.EditCellValue("Radius_W7", shapeNum, (double)PointList[i][j][4]);
                            aLayer.EditCellValue("Radius_W10", shapeNum, (double)PointList[i][j][5]);
                            aLayer.EditCellValue("MoveDir", shapeNum, (double)PointList[i][j][6]);
                            aLayer.EditCellValue("MoveSpeed", shapeNum, (double)PointList[i][j][7]);
                        }
                    }
                }

                sr.Close();
                fs.Close();
            }

            aLayer.LayerName = "Typhoon_Points";
            aLayer.LayerDrawType = LayerDrawType.TrajLine;
            //aLayer.LegendScheme = m_Legend.CreateSingleSymbolLegendScheme(Shape.ShapeType.Polyline, Color.Blue, 1.0F, 1, aDataInfo.TrajeoryNum);            
            aLayer.Visible = true;            
            LegendScheme aLS = LegendManage.CreateSingleSymbolLegendScheme(ShapeTypes.Point, Color.Red, 5.0F);
            aLS.FieldName = "TrajID";
            aLayer.LegendScheme = aLS;

            return aLayer;
        }

        /// <summary>
        /// Read one typhoon points data
        /// </summary>        
        /// <returns>A typhoon points data</returns>
        public List<List<object>> GetATrajData(int aTrajIdx)
        {
            List<List<object>> trajPointsData = new List<List<object>>();
            
            bool ifExit = false;
            for (int t = 0; t < FileNames.Count; t++)
            {
                string aFile = FileNames[t];
                FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string aLine;
                string[] dataArray;
                List<string> dataList = new List<string>();
                int i, LastNonEmpty;

                //
                int TrajIdx = -1;
                List<List<object>> pList = new List<List<object>>();                
                PointD aPoint = new PointD();
                ArrayList polylines = new ArrayList();                
                DateTime aDate;
                aLine = sr.ReadLine();
                aLine = sr.ReadLine();
                while (aLine != null)
                {
                    if (aLine.Trim() == String.Empty)
                    {
                        aLine = sr.ReadLine();
                        continue;
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
                    switch (dataList.Count)
                    {
                        case 4:
                            TrajIdx += 1;
                            if (TrajIdx > aTrajIdx)
                                ifExit = true;

                            break;
                        case 13:
                            if (TrajIdx == aTrajIdx)
                            {
                                List<object> dList = new List<object>();
                                aDate = DateTime.Parse(dataList[0] + "-" + dataList[1] + "-" +
                                    dataList[2] + " " + dataList[3] + ":00");
                                aPoint = new PointD();
                                aPoint.X = double.Parse(dataList[5]);
                                aPoint.Y = double.Parse(dataList[6]);
                                dList.Add(aPoint);
                                dList.Add(aDate);
                                dList.Add(double.Parse(dataList[7]));
                                
                                trajPointsData.Add(dList);                                
                            }
                            break;
                    }
                    if (ifExit)
                        break;

                    aLine = sr.ReadLine();
                }
                
                sr.Close();
                fs.Close();

                if (ifExit)
                    break;
            }

            return trajPointsData;
        }

        /// <summary>
        /// Get MICAPS 7 data info text
        /// </summary>        
        /// <returns></returns>
        public override string GenerateInfoText()
        {
            string dataInfo = "";
            for (int t = 0; t < FileNames.Count; t++)
            {                
                dataInfo += "File Name: " + FileNames[t];
                dataInfo += Environment.NewLine + "Typhoon number = " + TrajeoryNums[t].ToString();
                dataInfo += Environment.NewLine + Environment.NewLine + "Typhoons:";
                foreach (TrajectoryInfo aTrajInfo in TrajInfos[t])
                {
                    dataInfo += Environment.NewLine + "  " + aTrajInfo.TrajName + " " +
                        aTrajInfo.TrajID + " " + aTrajInfo.TrajCenter + " " + aTrajInfo.StartTime.ToString("yyyy-MM-dd HH:00") +
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
