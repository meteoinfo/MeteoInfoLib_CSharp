using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using MeteoInfoC.Layer;
using MeteoInfoC.Projections;
using MeteoInfoC.Global;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// MICAPS 13 data info
    /// </summary>
    public class MICAPS13DataInfo : DataInfo,IGridDataInfo
    {
        #region Variables
        ///// <summary>
        ///// File name
        ///// </summary>
        //public string FileName;
        /// <summary>
        /// Description
        /// </summary>
        public string Description;
        /// <summary>
        /// DateTime
        /// </summary>
        public DateTime Time;        
        /// <summary>
        /// Y number
        /// </summary>
        public int YNum;
        /// <summary>
        /// X number
        /// </summary>
        public int XNum;
        /// <summary>
        /// Left bottom longitude
        /// </summary>
        public double Lon_LB;
        /// <summary>
        /// Left bottom latitude
        /// </summary>
        public double Lat_LB;
        /// <summary>
        /// Projection: 1-lambert  2-mecator  3-北半球  4-南半球
        /// </summary>
        public int ProjOption;
        /// <summary>
        /// Zoom factor
        /// </summary>
        public double ZoomFactor;
        /// <summary>
        /// Image type: 1—红外云图 2—雷达拼图 3—地形图 4—可见光云图 5—水汽图
        /// </summary>
        public int ImageType;
        /// <summary>
        /// Table name: 象素值与相应物理量对照表文件名
        /// </summary>
        public string TableName;
        /// <summary>
        /// Center longitude
        /// </summary>
        public double Lon_Center;
        /// <summary>
        /// Center latitude
        /// </summary>
        public double Lat_Center;
        /// <summary>
        /// Image bytes
        /// </summary>
        public byte[] ImageBytes;                
        /// <summary>
        /// World file parameter
        /// </summary>
        public WorldFilePara WorldFileP;
        /// <summary>
        /// X array
        /// </summary>
        public double[] X;
        /// <summary>
        /// Y array
        /// </summary>
        public double[] Y;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MICAPS13DataInfo()
        {

        }

        #endregion

        #region Methods
        /// <summary>
        /// Read data info
        /// </summary>
        /// <param name="aFile">file path</param>
        public override void ReadDataInfo(string aFile)
        {
            FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs, Encoding.Default);

            //Read file head
            //string header = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(128));
            string header = System.Text.Encoding.Default.GetString(br.ReadBytes(128));
            string[] dataArray = header.Split();
            int LastNonEmpty = -1;
            List<string> dataList = new List<string>();
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (dataArray[i] != string.Empty)
                {
                    LastNonEmpty++;
                    dataList.Add(dataArray[i]);
                }
            }
            FileName = aFile;
            Description = dataList[2];
            //Time = DateTime.Parse(dataList[3] + "-" + dataList[4] + "-" + dataList[5] + " " +
            //    dataList[6] + ":00");
            int aYear = int.Parse(dataList[3]);
            if (aYear < 100)
            {
                if (aYear > 50)
                    aYear = 1900 + aYear;
                else
                    aYear = 2000 + aYear;
            }
            Time = new DateTime(aYear, int.Parse(dataList[4]), 
                int.Parse(dataList[5]), int.Parse(dataList[6]), 0, 0); 
           
            XNum = int.Parse(dataList[7]);
            YNum = int.Parse(dataList[8]);
            Lon_LB = double.Parse(dataList[9]);
            Lat_LB = double.Parse(dataList[10]);
            ProjOption = int.Parse(dataList[11]);
            ZoomFactor = double.Parse(dataList[12]);
            ImageType = int.Parse(dataList[13]);
            TableName = dataList[14];
            if (MIMath.IsNumeric(dataList[15]) && MIMath.IsNumeric(dataList[16]))
            {
                Lon_Center = double.Parse(dataList[15]);
                Lat_Center = double.Parse(dataList[16]);
                if (Lon_Center > 180)
                    Lon_Center = Lon_Center / 100;
                if (Lat_Center > 90)
                    Lat_Center = Lat_Center / 100;
            }
            else
            {
                Lon_Center = 110.0;
                Lat_Center = 30.0;
            }

            //Read image data    
            int length = (int)fs.Length - 128;
            ImageBytes = br.ReadBytes(length);

            //Set projection parameters
            GetProjectionInfo();
            CalCoordinate();

            br.Close();
            fs.Close();

            Dimension tdim = new Dimension(DimensionType.T);
            tdim.DimValue.Add(DataConvert.ToDouble(Time));
            tdim.DimLength = 1;
            this.TimeDimension = tdim;
            Dimension xdim = new Dimension(DimensionType.X);
            xdim.SetValues(X);
            Dimension ydim = new Dimension(DimensionType.Y);
            ydim.SetValues(Y);
            Variable var = new Variable();
            var.Name = "var";
            var.SetDimension(tdim);
            var.SetDimension(ydim);
            var.SetDimension(xdim);
            List<Variable> vars = new List<Variable>();
            vars.Add(var);
            this.Variables = vars;
        }

        private void GetProjectionInfo()
        {
            string ProjStr = "+proj=lcc" +
                            "+lat_1=" + Lat_Center.ToString() +
                            "+lat_2=60" +
                            "+lat_0=0" +
                            "+lon_0=" + Lon_Center.ToString() +
                            "+x_0=0" +
                            "+y_0=0";
            switch (ProjOption)
            {
                case 1:
                    ProjStr = "+proj=lcc" +
                            "+lat_1=" + Lat_Center.ToString() +
                            "+lat_2=60" +
                            "+lat_0=0" +
                            "+lon_0=" + Lon_Center.ToString() +
                            "+x_0=0" +
                            "+y_0=0";
                    break;
                case 2:
                    ProjStr = "+proj=merc" +
                            "+lon_0=" + Lon_Center.ToString();
                    break;
                case 3:
                    ProjStr = "+proj=stere" +
                                "+lat_0=90" +
                                "+lon_0=" + Lon_Center.ToString();
                    break;
                case 4:
                    ProjStr = "+proj=stere" +
                                "+lat_0=-90" +
                                "+lon_0=" + Lon_Center.ToString();
                    break;
            }

            this.ProjectionInfo = new ProjectionInfo(ProjStr);
        }

        private void CalCoordinate()
        {
            ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
            double X_LB, Y_LB;
            double[][] points = new double[1][];
            points[0] = new double[] { Lon_LB, Lat_LB };
            Reproject.ReprojectPoints(points, fromProj, this.ProjectionInfo, 0, 1);
            X_LB = points[0][0];
            Y_LB = points[0][1];

            double X_Center, Y_Center;
            points = new double[1][];
            points[0] = new double[] { Lon_Center, Lat_Center };
            Reproject.ReprojectPoints(points, fromProj, this.ProjectionInfo, 0, 1);
            X_Center = points[0][0];
            Y_Center = points[0][1];
            
            //double width = (X_Center - X_LB) * 2;
            //double height = (Y_Center - Y_LB) * 2;                       

            //WorldFileP.XUL = X_LB;
            //WorldFileP.YUL = Y_Center + (Y_Center - Y_LB);
            //WorldFileP.XScale = width / XNum;
            //WorldFileP.YScale = -height / YNum;

            X = new double[XNum];
            Y = new double[YNum];
            double xMax = X_Center + (X_Center - X_LB);
            double yMax = Y_Center + (Y_Center - Y_LB);
            double width = xMax - X_LB;
            double height = yMax - Y_LB;
            double xDelt = width / (XNum - 1);
            double yDelt = height / (YNum - 1);
            int i;
            for (i = 0; i < XNum; i++)
                X[i] = X_LB + i * xDelt;
            for (i = 0; i < YNum; i++)
                Y[i] = Y_LB + i * yDelt;
        }

        /// <summary>
        /// Get MICAPS 13 data info text
        /// </summary>        
        /// <returns>Info text</returns>
        public override string GenerateInfoText()
        {
            string dataInfo = "";
            dataInfo += "File Name: " + FileName;
            dataInfo += Environment.NewLine + "Description: " + Description;
            dataInfo += Environment.NewLine + "Time: " + Time.ToString("yyyy-MM-dd HH:mm");
            dataInfo += Environment.NewLine + "X number: " + XNum.ToString();
            dataInfo += Environment.NewLine + "Y number: " + YNum.ToString();
            dataInfo += Environment.NewLine + "Left-Bottom longitude: " + Lon_LB.ToString();
            dataInfo += Environment.NewLine + "Left-Bottom latitude: " + Lat_LB.ToString();
            dataInfo += Environment.NewLine + "Center longitude: " + Lon_Center.ToString();
            dataInfo += Environment.NewLine + "Center latitude: " + Lat_Center.ToString();
            dataInfo += Environment.NewLine + "Projection: " + GetProjectionString(ProjOption);
            dataInfo += Environment.NewLine + "Zoom factor: " + ZoomFactor.ToString();
            dataInfo += Environment.NewLine + "Image type: " + GetImageType(ImageType);
            dataInfo += Environment.NewLine + "Table name: " + TableName;

            return dataInfo;
        }

        private string GetProjectionString(int proj)
        {
            string projStr = "Lon/Lat";
            switch (proj)
            {
                case 1:
                    projStr = "Lambert";
                    break;
                case 2:
                    projStr = "Mecator";
                    break;
                case 3:
                    projStr = "NorthPolar";
                    break;
                case 4:
                    projStr = "SourthPolar";
                    break;
            }

            return projStr;
        }

        private string GetImageType(int iType)
        {
            string imageType = "1—红外云图 2—雷达拼图 3—地形图 4—可见光云图 5—水汽图";
            switch (iType)
            {
                case 1:
                    imageType = "红外云图";
                    break;
                case 2:
                    imageType = "雷达拼图";
                    break;
                case 3:
                    imageType = "地形图";
                    break;
                case 4:
                    imageType = "可见光云图";
                    break;
                case 5:
                    imageType = "水汽图";
                    break;
            }

            return imageType;
        }

        /// <summary>
        /// Read grid data - LonLat
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_LonLat(int timeIdx, int varIdx, int levelIdx)
        {
            GridData gridData = new GridData();
            double[,] gData = new double[YNum, XNum];
            for (int i = 0; i < YNum; i++)
            {
                for (int j = 0; j < XNum; j++)
                    gData[i, j] = ImageBytes[i * XNum + j];
            }
            gridData.Data = gData;
            gridData.X = X;
            gridData.Y = Y;

            return gridData;
        }

        /// <summary>
        /// Read grid data - TimeLat
        /// </summary>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_TimeLat(int lonIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - TimeLon
        /// </summary>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_TimeLon(int latIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - LevelLat
        /// </summary>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="timeIdx">Time index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_LevelLat(int lonIdx, int varIdx, int timeIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - LevelLon
        /// </summary>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="timeIdx">Time index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_LevelLon(int latIdx, int varIdx, int timeIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - LevelTime
        /// </summary>
        /// <param name="latIdx">Laititude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="lonIdx">Longitude index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_LevelTime(int latIdx, int varIdx, int lonIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - Time
        /// </summary>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_Time(int lonIdx, int latIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Read grid data - Level
        /// </summary>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="timeIdx">Time index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_Level(int lonIdx, int latIdx, int varIdx, int timeIdx)
        {
            return null;
        }

        /// <summary>
        /// Get grid data - Lon
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="latIdx">Latitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level Index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_Lon(int timeIdx, int latIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        /// <summary>
        /// Get grid data - Lat
        /// </summary>
        /// <param name="timeIdx">Time index</param>
        /// <param name="lonIdx">Longitude index</param>
        /// <param name="varIdx">Variable index</param>
        /// <param name="levelIdx">Level index</param>
        /// <returns>Grid data</returns>
        public GridData GetGridData_Lat(int timeIdx, int lonIdx, int varIdx, int levelIdx)
        {
            return null;
        }

        #endregion
    }
}
