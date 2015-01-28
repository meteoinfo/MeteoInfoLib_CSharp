using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Projections;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// ARL meteo data
    /// </summary>
    public class ARLMeteoData
    {
        ///// <summary>
        ///// Read ARL data info
        ///// </summary>
        ///// <param name="aFile"></param>
        ///// <param name="aDataInfo"></param>
        ///// <param name="ErrorStr"></param>
        ///// <returns></returns>
        //public Boolean ReadARLDataInfo(string aFile, ref ARLDataInfo aDataInfo, ref string ErrorStr)
        //{
        //    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    DataLabel aDL = new DataLabel();
        //    DataHead aDH = new DataHead();
        //    int i, j, vNum;
        //    string vName;
        //    List<string> vList = new List<string>();

        //    //open file to decode the standard label (50) plus the 
        //    //fixed portion (108) of the extended header            
        //    aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //    aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //    aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //    aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //    aDH.MODEL = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //    aDH.ICX = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(3)));
        //    aDH.MN = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDH.POLE_LAT = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
        //    aDH.POLE_LON = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
        //    aDH.REF_LAT = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
        //    aDH.REF_LON = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
        //    aDH.SIZE = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
        //    aDH.ORIENT = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
        //    aDH.TANG_LAT = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
        //    aDH.SYNC_XP = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
        //    aDH.SYNC_YP = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
        //    aDH.SYNC_LAT = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
        //    aDH.SYNC_LON = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
        //    aDH.DUMMY = Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(7)));
        //    aDH.NX = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(3)));
        //    aDH.NY = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(3)));
        //    aDH.NZ = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(3)));
        //    aDH.K_FLAG = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDH.LENH = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //    for (i = 0; i < aDH.NZ; i++)
        //    {
        //        aDataInfo.levels.Add(Single.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(6))));
        //        vNum = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        for (j = 0; j < vNum; j++)
        //        {
        //            vName = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //            vList.Add(vName);
        //            br.ReadBytes(4);
        //        }
        //        aDataInfo.varList.Add(new List<string>(vList));
        //        vList.Clear();
        //    }
        //    aDataInfo.levelNum = aDH.NZ;

        //    //Close file reader
        //    br.Close();
        //    fs.Close();

        //    if (aDL.Variable != "INDX")
        //    {
        //        ErrorStr = "WARNING Old format meteo data grid!" + Environment.NewLine + aDL.Variable;
        //        return false;
        //    }

        //    //Decide projection
        //    aDataInfo.fileName = aFile;
        //    aDataInfo.dataHead = aDH;
        //    if (aDH.SIZE == 0)
        //    {
        //        aDataInfo.isLatLon = true;                
        //        aDataInfo.X = new double[aDH.NX];
        //        aDataInfo.Y = new double[aDH.NY];
        //        for (i = 0; i < aDH.NX; i++)
        //        {
        //            aDataInfo.X[i] = aDH.SYNC_LON + i * aDH.REF_LON;
        //        }
        //        if (aDataInfo.X[aDH.NX - 1] + aDH.REF_LON - aDataInfo.X[0] == 360)
        //        {
        //            aDataInfo.isGlobal = true;
        //        }
        //        for (i = 0; i < aDH.NY; i++)
        //        {
        //            aDataInfo.Y[i] = aDH.SYNC_LAT + i * aDH.REF_LAT;
        //        }
        //    }
        //    else
        //    {                
        //        //Identify projection
        //        aDataInfo.isLatLon = false;
        //        string ProjStr;
        //        ProjectionInfo theProj = new ProjectionInfo();
        //        if (aDH.POLE_LAT == 90 || aDH.POLE_LAT == -90)
        //        {
        //            if (aDH.TANG_LAT == 90 || aDH.TANG_LAT == -90)
        //            {                        
        //                ProjStr = "+proj=stere" +
        //                        "+lat_0=" + aDH.TANG_LAT.ToString() +
        //                        "+lon_0=" + (aDH.REF_LON + aDH.ORIENT).ToString();
        //            }
        //            else if (aDH.TANG_LAT == 0)
        //            {                        
        //                ProjStr = "+proj=merc" +
        //                    "+lon_0=" + (aDH.REF_LON + aDH.ORIENT).ToString();
        //            }
        //            else
        //            {                        
        //                ProjStr = "+proj=lcc" +
        //                        "+lat_1=" + aDH.TANG_LAT.ToString() +
        //                        "+lon_0=" + (aDH.REF_LON + aDH.ORIENT).ToString();
        //            }
        //        }
        //        else
        //        {
        //            if (aDH.TANG_LAT == 0)
        //            {                        
        //                ProjStr = "+proj=tmerc" +
        //                    "+lat_0=" + aDH.POLE_LAT.ToString() +
        //                    "+lon_0=" + (aDH.REF_LON + aDH.ORIENT).ToString();
        //            }
        //            else
        //            {                        
        //                ProjStr = "+proj=stere" +
        //                            "+lat_0=" + aDH.POLE_LAT.ToString() +
        //                            "+lon_0=" + (aDH.REF_LON + aDH.ORIENT).ToString();
        //            }
        //        }

        //        theProj = new ProjectionInfo(ProjStr);
        //        aDataInfo.projInfo = theProj;

        //        //Set X Y
        //        aDataInfo.X = new double[aDH.NX];
        //        aDataInfo.Y = new double[aDH.NY];
        //        GetProjectedXY(theProj, aDH.SIZE * 1000, aDH.SYNC_XP, aDH.SYNC_YP, aDH.SYNC_LON,
        //            aDH.SYNC_LAT, ref aDataInfo.X, ref aDataInfo.Y);
        //    }

        //    //Reopen
        //    int NXY = aDH.NX * aDH.NY;
        //    int LEN = NXY + 50;
        //    aDataInfo.recLen = LEN;
        //    byte[] dataBytes = new byte[NXY];
        //    DateTime aTime, oldTime;
        //    int recNum, timeNum;
        //    fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
        //    br = new BinaryReader(fs);
        //    recNum = 0;
        //    timeNum = 0;
        //    string dStr;
        //    dStr = aDL.Year.ToString("00") + "-" + aDL.Month.ToString("00") + "-" +
        //        aDL.Day.ToString("00") + " " + aDL.Hour.ToString("00");
        //    oldTime = DateTime.ParseExact(dStr, "yy-MM-dd HH", System.Globalization.CultureInfo.InvariantCulture);
        //    aDataInfo.times.Add(oldTime);

        //    do
        //    {
        //        if (br.BaseStream.Position >= br.BaseStream.Length)
        //        {
        //            break;
        //        }

        //        //Read label
        //        aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //        aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //        aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //        aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //        //Read Data
        //        dataBytes = br.ReadBytes(NXY);

        //        if (aDL.Variable != "INDX")
        //        {
        //            dStr = aDL.Year.ToString("00") + "-" + aDL.Month.ToString("00") + "-" +
        //                aDL.Day.ToString("00") + " " + aDL.Hour.ToString("00");
        //            aTime = DateTime.ParseExact(dStr, "yy-MM-dd HH", System.Globalization.CultureInfo.InvariantCulture);
        //            if (aTime != oldTime)
        //            {
        //                aDataInfo.times.Add(aTime);
        //                oldTime = aTime;
        //                timeNum += 1;
        //            }
        //            if (timeNum == 0)
        //            {
        //                recNum += 1;
        //            }
        //        }

        //    } while (true);

        //    br.Close();
        //    fs.Close();

        //    aDataInfo.recsPerTime = recNum + 1;
        //    ARLVAR aVar = new ARLVAR();
        //    vList.Clear();
        //    int varIdx;
        //    if (aDataInfo.levelNum > 1)
        //    {
        //        for (i = 1; i < aDataInfo.varList.Count; i++)
        //        {
        //            for (j = 0; j < aDataInfo.varList[i].Count; j++)
        //            {
        //                vName = aDataInfo.varList[i][j];
        //                if (!vList.Contains(vName))
        //                {
        //                    vList.Add(vName);
        //                    aVar = new ARLVAR();
        //                    aVar.VName = vName;
        //                    aVar.LevelIdxs.Add(i);
        //                    aVar.VarInLevelIdxs.Add(j);
        //                    aDataInfo.varLevList.Add(aVar);
        //                }
        //                else
        //                {
        //                    varIdx = vList.IndexOf(vName);
        //                    aVar = aDataInfo.varLevList[varIdx];
        //                    aVar.LevelIdxs.Add(i);
        //                    aVar.VarInLevelIdxs.Add(j);
        //                    aDataInfo.varLevList[varIdx] = aVar;
        //                }
        //            }
        //        }
        //    }
        //    for (j = 0; j < aDataInfo.varList[0].Count; j++)
        //    {
        //        vName = aDataInfo.varList[0][j];
        //        vList.Add(vName);
        //        aVar = new ARLVAR();
        //        aVar.VName = vName;
        //        aVar.LevelIdxs.Add(0);
        //        aVar.VarInLevelIdxs.Add(j);
        //        aDataInfo.varLevList.Add(aVar);
        //    }
        //    for (i = 0; i < aDataInfo.varLevList.Count; i++)
        //    {
        //        aDataInfo.varLevList[i].LevelNum = aDataInfo.varLevList[i].LevelIdxs.Count;
        //    }

        //    return true;
        //}

        //private void GetProjectedXY(ProjectionInfo projInfo, Single size,
        //    Single sync_XP, Single sync_YP, Single sync_Lon, Single sync_Lat,
        //    ref double[] X, ref double[] Y)
        //{
        //    //Get sync X/Y
        //    ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
        //    double sync_X, sync_Y;
        //    double[][] points = new double[1][];
        //    points[0] = new double[] { sync_Lon, sync_Lat };
        //    Reproject.ReprojectPoints(points, fromProj, projInfo, 0, 1);
        //    sync_X = points[0][0];
        //    sync_Y = points[0][1];

        //    //Get integer sync X/Y            
        //    int i_XP, i_YP;
        //    double i_X, i_Y;
        //    i_XP = (int)sync_XP;
        //    if (sync_XP == i_XP)
        //    {
        //        i_X = sync_X;
        //    }
        //    else
        //    {
        //        i_X = sync_X - (sync_XP - i_XP) * size;
        //    }
        //    i_YP = (int)sync_YP;
        //    if (sync_YP == i_YP)
        //    {
        //        i_Y = sync_Y;
        //    }
        //    else
        //    {
        //        i_Y = sync_Y - (sync_YP - i_YP) * size;
        //    }

        //    //Get left bottom X/Y
        //    int nx, ny;
        //    nx = X.Length;
        //    ny = Y.Length;
        //    double xlb, ylb;
        //    xlb = i_X - (i_XP - 1) * size;
        //    ylb = i_Y - (i_YP - 1) * size;

        //    //Get X Y with orient 0
        //    int i;
        //    for (i = 0; i < nx; i++)
        //    {
        //        X[i] = xlb + i * size;
        //    }
        //    for (i = 0; i < ny; i++)
        //    {
        //        Y[i] = ylb + i * size;
        //    }
        //}

        ///// <summary>
        ///// Get ARL data - Lon/Lat
        ///// </summary>
        ///// <param name="timeIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="levelIdx"></param>
        ///// <param name="aDataInfo"></param>
        ///// <returns></returns>
        //public double[,] GetARLGridData(int timeIdx, int varIdx, int levelIdx, ARLDataInfo aDataInfo)
        //{
        //    int xNum, yNum;
        //    xNum = aDataInfo.dataHead.NX;
        //    yNum = aDataInfo.dataHead.NY;
        //    double[,] gridData = new double[yNum, xNum];
        //    FileStream fs = new FileStream(aDataInfo.fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();

        //    br.BaseStream.Position = timeIdx * aDataInfo.recsPerTime * aDataInfo.recLen;
        //    br.BaseStream.Position += aDataInfo.recLen;
        //    for (int i = 0; i < levelIdx; i++)
        //    {
        //        br.BaseStream.Position += aDataInfo.varList[i].Count * aDataInfo.recLen;
        //    }
        //    br.BaseStream.Position += varIdx * aDataInfo.recLen;

        //    //Read label
        //    aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //    aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //    aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //    aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //    //Read Data

        //    dataBytes = br.ReadBytes(aDataInfo.recLen - 50);

        //    br.Close();
        //    fs.Close();

        //    gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

        //    if (aDataInfo.isGlobal)
        //    {
        //        double[,] newGridData;
        //        newGridData = new double[yNum, xNum + 1];
        //        for (int i = 0; i < yNum; i++)
        //        {
        //            for (int j = 0; j < xNum; j++)
        //            {
        //                newGridData[i, j] = gridData[i, j];
        //            }
        //            newGridData[i, xNum] = newGridData[i, 0];
        //        }
        //        return newGridData;
        //    }
        //    else
        //    {
        //        return gridData;
        //    }
        //}

        ///// <summary>
        ///// Get ARL data - Time/Lon
        ///// </summary>
        ///// <param name="latIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="levelIdx"></param>
        ///// <param name="aDataInfo"></param>
        ///// <returns></returns>
        //public double[,] GetARLGridData_TimeLon(int latIdx, int varIdx, int levelIdx, ARLDataInfo aDataInfo)
        //{
        //    int xNum, yNum, tNum, t;
        //    xNum = aDataInfo.dataHead.NX;
        //    yNum = aDataInfo.dataHead.NY;
        //    tNum = aDataInfo.times.Count;
        //    double[,] gridData = new double[yNum, xNum];
        //    FileStream fs = new FileStream(aDataInfo.fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    double[,] newGridData = new double[tNum, xNum];

        //    for (t = 0; t < aDataInfo.times.Count; t++)
        //    {
        //        br.BaseStream.Position = t * aDataInfo.recsPerTime * aDataInfo.recLen;
        //        br.BaseStream.Position += aDataInfo.recLen;
        //        for (int i = 0; i < levelIdx; i++)
        //        {
        //            br.BaseStream.Position += aDataInfo.varList[i].Count * aDataInfo.recLen;
        //        }
        //        br.BaseStream.Position += varIdx * aDataInfo.recLen;

        //        //Read label
        //        aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //        aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //        aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //        aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //        //Read Data
        //        dataBytes = br.ReadBytes(aDataInfo.recLen - 50);
        //        gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);
        //        for (int j = 0; j < xNum; j++)
        //        {
        //            newGridData[t, j] = gridData[latIdx, j];
        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return newGridData;
        //}

        ///// <summary>
        ///// Get ARL data - Time/Lat
        ///// </summary>
        ///// <param name="lonIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="levelIdx"></param>
        ///// <param name="aDataInfo"></param>
        ///// <returns></returns>
        //public double[,] GetARLGridData_TimeLat(int lonIdx, int varIdx, int levelIdx, ARLDataInfo aDataInfo)
        //{
        //    int xNum, yNum, tNum, t;
        //    xNum = aDataInfo.dataHead.NX;
        //    yNum = aDataInfo.dataHead.NY;
        //    tNum = aDataInfo.times.Count;
        //    double[,] gridData = new double[yNum, xNum];
        //    FileStream fs = new FileStream(aDataInfo.fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    double[,] newGridData = new double[tNum, yNum];

        //    for (t = 0; t < aDataInfo.times.Count; t++)
        //    {
        //        br.BaseStream.Position = t * aDataInfo.recsPerTime * aDataInfo.recLen;
        //        br.BaseStream.Position += aDataInfo.recLen;
        //        for (int i = 0; i < levelIdx; i++)
        //        {
        //            br.BaseStream.Position += aDataInfo.varList[i].Count * aDataInfo.recLen;
        //        }
        //        br.BaseStream.Position += varIdx * aDataInfo.recLen;

        //        //Read label
        //        aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //        aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //        aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //        aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //        //Read Data
        //        dataBytes = br.ReadBytes(aDataInfo.recLen - 50);
        //        gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);
        //        for (int i = 0; i < yNum; i++)
        //        {
        //            newGridData[t, i] = gridData[i, lonIdx];
        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return newGridData;
        //}

        ///// <summary>
        ///// Get ARL data - Level/Lon
        ///// </summary>
        ///// <param name="latIdx"></param>
        ///// <param name="cvarIdx"></param>
        ///// <param name="tIdx"></param>
        ///// <param name="aDataInfo"></param>
        ///// <returns></returns>
        //public double[,] GetARLGridData_LevelLon(int latIdx, int cvarIdx, int tIdx, ARLDataInfo aDataInfo)
        //{
        //    int xNum, yNum, lNum, varIdx, levIdx;
        //    xNum = aDataInfo.dataHead.NX;
        //    yNum = aDataInfo.dataHead.NY;
        //    lNum = aDataInfo.varLevList[cvarIdx].LevelNum;
        //    double[,] gridData = new double[yNum, xNum];
        //    FileStream fs = new FileStream(aDataInfo.fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    double[,] newGridData = new double[lNum, xNum];
        //    long aLevPosition;

        //    br.BaseStream.Position = tIdx * aDataInfo.recsPerTime * aDataInfo.recLen;
        //    br.BaseStream.Position += aDataInfo.recLen;
        //    aLevPosition = br.BaseStream.Position;
        //    levIdx = aDataInfo.varLevList[cvarIdx].LevelIdxs[0];
        //    for (int i = 0; i < lNum; i++)
        //    {
        //        varIdx = aDataInfo.varLevList[cvarIdx].VarInLevelIdxs[i];
        //        levIdx = aDataInfo.varLevList[cvarIdx].LevelIdxs[i];
        //        br.BaseStream.Position = aLevPosition;
        //        for (int j = 0; j < levIdx; j++)
        //        {
        //            br.BaseStream.Position += aDataInfo.varList[j].Count * aDataInfo.recLen;
        //        }
        //        br.BaseStream.Position += varIdx * aDataInfo.recLen;

        //        //Read label
        //        aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //        aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //        aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //        aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //        //Read Data
        //        dataBytes = br.ReadBytes(aDataInfo.recLen - 50);
        //        gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);
        //        for (int j = 0; j < xNum; j++)
        //        {
        //            newGridData[i, j] = gridData[latIdx, j];
        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return newGridData;
        //}

        ///// <summary>
        ///// Get ARL data - Level/Lat
        ///// </summary>
        ///// <param name="lonIdx"></param>
        ///// <param name="cvarIdx"></param>
        ///// <param name="tIdx"></param>
        ///// <param name="aDataInfo"></param>
        ///// <returns></returns>
        //public double[,] GetARLGridData_LevelLat(int lonIdx, int cvarIdx, int tIdx, ARLDataInfo aDataInfo)
        //{
        //    int xNum, yNum, lNum, varIdx, levIdx;
        //    xNum = aDataInfo.dataHead.NX;
        //    yNum = aDataInfo.dataHead.NY;
        //    lNum = aDataInfo.varLevList[cvarIdx].LevelNum;
        //    double[,] gridData = new double[yNum, xNum];
        //    FileStream fs = new FileStream(aDataInfo.fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    double[,] newGridData = new double[lNum, yNum];
        //    long aLevPosition;

        //    br.BaseStream.Position = tIdx * aDataInfo.recsPerTime * aDataInfo.recLen;
        //    br.BaseStream.Position += aDataInfo.recLen;
        //    aLevPosition = br.BaseStream.Position;
        //    levIdx = aDataInfo.varLevList[cvarIdx].LevelIdxs[0];
        //    for (int i = 0; i < lNum; i++)
        //    {
        //        varIdx = aDataInfo.varLevList[cvarIdx].VarInLevelIdxs[i];
        //        levIdx = aDataInfo.varLevList[cvarIdx].LevelIdxs[i];
        //        br.BaseStream.Position = aLevPosition;
        //        for (int j = 0; j < levIdx; j++)
        //        {
        //            br.BaseStream.Position += aDataInfo.varList[j].Count * aDataInfo.recLen;
        //        }
        //        br.BaseStream.Position += varIdx * aDataInfo.recLen;

        //        //Read label
        //        aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //        aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //        aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //        aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //        //Read Data
        //        dataBytes = br.ReadBytes(aDataInfo.recLen - 50);
        //        gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);
        //        for (int j = 0; j < yNum; j++)
        //        {
        //            newGridData[i, j] = gridData[j, lonIdx];
        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return newGridData;
        //}

        ///// <summary>
        ///// Get ARL data - LevelTime
        ///// </summary>
        ///// <param name="latIdx"></param>
        ///// <param name="cvarIdx"></param>
        ///// <param name="lonIdx"></param>
        ///// <param name="aDataInfo"></param>
        ///// <returns></returns>
        //public double[,] GetARLGridData_LevelTime(int latIdx, int cvarIdx, int lonIdx, ARLDataInfo aDataInfo)
        //{
        //    int xNum, yNum, lNum, varIdx, levIdx, t, tNum;
        //    xNum = aDataInfo.dataHead.NX;
        //    yNum = aDataInfo.dataHead.NY;
        //    lNum = aDataInfo.varLevList[cvarIdx].LevelNum;
        //    tNum = aDataInfo.times.Count;
        //    double[,] gridData = new double[yNum, xNum];
        //    FileStream fs = new FileStream(aDataInfo.fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    double[,] newGridData = new double[lNum, tNum];
        //    long aLevPosition;

        //    for (t = 0; t < aDataInfo.times.Count; t++)
        //    {
        //        br.BaseStream.Position = t * aDataInfo.recsPerTime * aDataInfo.recLen;
        //        br.BaseStream.Position += aDataInfo.recLen;
        //        aLevPosition = br.BaseStream.Position;
        //        levIdx = aDataInfo.varLevList[cvarIdx].LevelIdxs[0];
        //        for (int i = 0; i < lNum; i++)
        //        {
        //            varIdx = aDataInfo.varLevList[cvarIdx].VarInLevelIdxs[i];
        //            levIdx = aDataInfo.varLevList[cvarIdx].LevelIdxs[i];
        //            br.BaseStream.Position = aLevPosition;
        //            for (int j = 0; j < levIdx; j++)
        //            {
        //                br.BaseStream.Position += aDataInfo.varList[j].Count * aDataInfo.recLen;
        //            }
        //            br.BaseStream.Position += varIdx * aDataInfo.recLen;

        //            //Read label
        //            aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //            aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //            aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //            aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //            aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //            aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //            aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //            aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //            aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //            aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //            aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //            //Read Data
        //            dataBytes = br.ReadBytes(aDataInfo.recLen - 50);
        //            gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

        //            newGridData[i, t] = gridData[latIdx, lonIdx];

        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return newGridData;
        //}
        
        //private double[,] UnpackARLGridData(byte[] dataBytes, int xNum, int yNum, DataLabel aDL)
        //{
        //    double[,] gridData = new double[yNum, xNum];
        //    double SCALE = Math.Pow(2.0, (7 - aDL.Exponent));
        //    double VOLD = aDL.Value;
        //    int INDX = 0;
        //    int i, j;
        //    for (j = 0; j < yNum; j++)
        //    {
        //        for (i = 0; i < xNum; i++)
        //        {
        //            gridData[j, i] = ((int)(dataBytes[INDX]) - 127) / SCALE + VOLD;
        //            INDX += 1;
        //            VOLD = gridData[j, i];
        //        }
        //        VOLD = gridData[j, 0];
        //    }


        //    return gridData;
        //}

        ///// <summary>
        ///// Get ARL data - Time
        ///// </summary>
        ///// <param name="lonIdx"></param>
        ///// <param name="latIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="levelIdx"></param>
        ///// <param name="aDataInfo"></param>
        ///// <returns></returns>
        //public List<PointD> GetARLGridData_Time(int lonIdx, int latIdx, int varIdx, int levelIdx, ARLDataInfo aDataInfo)
        //{
        //    PointD aPoint = new PointD();
        //    List<PointD> pointList = new List<PointD>();

        //    FileStream fs = new FileStream(aDataInfo.fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    int xNum, yNum, t;
        //    xNum = aDataInfo.dataHead.NX;
        //    yNum = aDataInfo.dataHead.NY;
        //    double[,] gridData = new double[yNum, xNum];
        //    double aValue;

        //    for (t = 0; t < aDataInfo.times.Count; t++)
        //    {
        //        br.BaseStream.Position = t * aDataInfo.recsPerTime * aDataInfo.recLen;
        //        br.BaseStream.Position += aDataInfo.recLen;
        //        for (int i = 0; i < levelIdx; i++)
        //        {
        //            br.BaseStream.Position += aDataInfo.varList[i].Count * aDataInfo.recLen;
        //        }
        //        br.BaseStream.Position += varIdx * aDataInfo.recLen;

        //        //Read label
        //        aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //        aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //        aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //        aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //        //Read Data
        //        dataBytes = br.ReadBytes(aDataInfo.recLen - 50);
        //        gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

        //        aValue = gridData[latIdx, lonIdx];
        //        if (!(Math.Abs(aValue / aDataInfo.UNDEF - 1) < 0.01))
        //        {
        //            aPoint.X = aDataInfo.times[t].ToBinary();
        //            aPoint.Y = aValue;
        //            pointList.Add(aPoint);
        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return pointList;
        //}

        ///// <summary>
        ///// Get ARL data - Level
        ///// </summary>
        ///// <param name="lonIdx"></param>
        ///// <param name="latIdx"></param>
        ///// <param name="cvarIdx"></param>
        ///// <param name="timeIdx"></param>
        ///// <param name="aDataInfo"></param>
        ///// <returns></returns>
        //public List<PointD> GetARLGridData_Level(int lonIdx, int latIdx, int cvarIdx, int timeIdx, ARLDataInfo aDataInfo)
        //{
        //    PointD aPoint = new PointD();
        //    List<PointD> pointList = new List<PointD>();

        //    FileStream fs = new FileStream(aDataInfo.fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    int xNum, yNum, varIdx, levIdx, lNum;
        //    xNum = aDataInfo.dataHead.NX;
        //    yNum = aDataInfo.dataHead.NY;
        //    lNum = aDataInfo.varLevList[cvarIdx].LevelNum;
        //    double[,] gridData = new double[yNum, xNum];
        //    double aValue;

        //    br.BaseStream.Position += aDataInfo.recLen;
        //    long aLevPosition = br.BaseStream.Position;
        //    levIdx = aDataInfo.varLevList[cvarIdx].LevelIdxs[0];
        //    for (int i = 0; i < lNum; i++)
        //    {
        //        varIdx = aDataInfo.varLevList[cvarIdx].VarInLevelIdxs[i];
        //        levIdx = aDataInfo.varLevList[cvarIdx].LevelIdxs[i];
        //        br.BaseStream.Position = aLevPosition;
        //        for (int j = 0; j < levIdx; j++)
        //        {
        //            br.BaseStream.Position += aDataInfo.varList[j].Count * aDataInfo.recLen;
        //        }
        //        br.BaseStream.Position += varIdx * aDataInfo.recLen;

        //        //Read label
        //        aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //        aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //        aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //        aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //        aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //        //Read Data
        //        dataBytes = br.ReadBytes(aDataInfo.recLen - 50);
        //        gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);
        //        aValue = gridData[latIdx, lonIdx];
        //        if (!(Math.Abs(aValue / aDataInfo.UNDEF - 1) < 0.01))
        //        {
        //            aPoint.X = aValue;
        //            aPoint.Y = aDataInfo.levels[levIdx];
        //            pointList.Add(aPoint);
        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return pointList;
        //}

        ///// <summary>
        ///// Get ARL data - Longitude
        ///// </summary>
        ///// <param name="timeIdx"></param>
        ///// <param name="latIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="levelIdx"></param>
        ///// <param name="aDataInfo"></param>
        ///// <returns></returns>
        //public List<PointD> GetARLGridData_Lon(int timeIdx, int latIdx, int varIdx, int levelIdx, ARLDataInfo aDataInfo)
        //{
        //    PointD aPoint = new PointD();
        //    List<PointD> pointList = new List<PointD>();

        //    FileStream fs = new FileStream(aDataInfo.fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    int xNum, yNum, i;
        //    xNum = aDataInfo.dataHead.NX;
        //    yNum = aDataInfo.dataHead.NY;
        //    double[,] gridData = new double[yNum, xNum];
        //    double aValue;

        //    br.BaseStream.Position = timeIdx * aDataInfo.recsPerTime * aDataInfo.recLen;
        //    br.BaseStream.Position += aDataInfo.recLen;
        //    for (i = 0; i < levelIdx; i++)
        //    {
        //        br.BaseStream.Position += aDataInfo.varList[i].Count * aDataInfo.recLen;
        //    }
        //    br.BaseStream.Position += varIdx * aDataInfo.recLen;

        //    //Read label
        //    aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //    aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //    aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //    aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //    //Read Data
        //    dataBytes = br.ReadBytes(aDataInfo.recLen - 50);
        //    gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

        //    for (i = 0; i < xNum; i++)
        //    {
        //        aValue = gridData[latIdx, i];
        //        if (!(Math.Abs(aValue / aDataInfo.UNDEF - 1) < 0.01))
        //        {
        //            aPoint.X = aDataInfo.X[i];
        //            aPoint.Y = aValue;
        //            pointList.Add(aPoint);
        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return pointList;
        //}

        ///// <summary>
        ///// Get ARL data - Latitude
        ///// </summary>
        ///// <param name="timeIdx"></param>
        ///// <param name="lonIdx"></param>
        ///// <param name="varIdx"></param>
        ///// <param name="levelIdx"></param>
        ///// <param name="aDataInfo"></param>
        ///// <returns></returns>
        //public List<PointD> GetARLGridData_Lat(int timeIdx, int lonIdx, int varIdx, int levelIdx, ARLDataInfo aDataInfo)
        //{
        //    PointD aPoint = new PointD();
        //    List<PointD> pointList = new List<PointD>();

        //    FileStream fs = new FileStream(aDataInfo.fileName, FileMode.Open, FileAccess.Read);
        //    BinaryReader br = new BinaryReader(fs);
        //    byte[] dataBytes;
        //    DataLabel aDL = new DataLabel();
        //    int xNum, yNum, i;
        //    xNum = aDataInfo.dataHead.NX;
        //    yNum = aDataInfo.dataHead.NY;
        //    double[,] gridData = new double[yNum, xNum];
        //    double aValue;

        //    br.BaseStream.Position = timeIdx * aDataInfo.recsPerTime * aDataInfo.recLen;
        //    br.BaseStream.Position += aDataInfo.recLen;
        //    for (i = 0; i < levelIdx; i++)
        //    {
        //        br.BaseStream.Position += aDataInfo.varList[i].Count * aDataInfo.recLen;
        //    }
        //    br.BaseStream.Position += varIdx * aDataInfo.recLen;

        //    //Read label
        //    aDL.Year = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Month = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Day = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Hour = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Forecast = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Level = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Grid = Int16.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(2)));
        //    aDL.Variable = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
        //    aDL.Exponent = int.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)));
        //    aDL.Precision = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));
        //    aDL.Value = double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(14)));

        //    //Read Data
        //    dataBytes = br.ReadBytes(aDataInfo.recLen - 50);
        //    gridData = UnpackARLGridData(dataBytes, xNum, yNum, aDL);

        //    for (i = 0; i < yNum; i++)
        //    {
        //        aValue = gridData[i, lonIdx];
        //        if (!(Math.Abs(aValue / aDataInfo.UNDEF - 1) < 0.01))
        //        {
        //            aPoint.X = aDataInfo.Y[i];
        //            aPoint.Y = aValue;
        //            pointList.Add(aPoint);
        //        }
        //    }

        //    br.Close();
        //    fs.Close();

        //    return pointList;
        //}

        ///// <summary>
        ///// Generate data info text
        ///// </summary>
        ///// <param name="aDataInfo"></param>
        ///// <returns></returns>
        //public string GenerateInfoText(ARLDataInfo aDataInfo)
        //{
        //    string dataInfo;
        //    dataInfo = "File Name: " + aDataInfo.fileName;
        //    dataInfo += Environment.NewLine + "File Start Time: " + aDataInfo.times[0].ToString("yyyy-MM-dd HH:00");
        //    dataInfo += Environment.NewLine + "File End Time: " + aDataInfo.times[aDataInfo.times.Count - 1].ToString("yyyy-MM-dd HH:00");
        //    dataInfo += Environment.NewLine + "Record Length Bytes: " + aDataInfo.recLen.ToString();
        //    dataInfo += Environment.NewLine + "Meteo Data Model: " + aDataInfo.dataHead.MODEL;
        //    dataInfo += Environment.NewLine + "Xsize = " + aDataInfo.dataHead.NX.ToString() +
        //            "  Ysize = " + aDataInfo.dataHead.NY.ToString() + "  Zsize = " + aDataInfo.dataHead.NZ.ToString() +
        //            "  Tsize = " + aDataInfo.times.Count.ToString();
        //    dataInfo += Environment.NewLine + "Record Per Time: " + aDataInfo.recsPerTime.ToString();
        //    dataInfo += Environment.NewLine + "Number of Surface Variables = " + aDataInfo.varList[0].Count.ToString();
        //    foreach (string v in aDataInfo.varList[0])
        //    {
        //        dataInfo += Environment.NewLine + "  " + v;
        //    }
        //    if (aDataInfo.varList.Count > 1)
        //    {
        //        dataInfo += Environment.NewLine + "Number of Upper Variables = " + aDataInfo.varList[1].Count.ToString();
        //        foreach (string v in aDataInfo.varList[1])
        //        {
        //            dataInfo += Environment.NewLine + "  " + v;
        //        }
        //    }
        //    dataInfo += Environment.NewLine + "Pole pnt lat/lon: " +
        //        aDataInfo.dataHead.POLE_LAT.ToString() + "  " + aDataInfo.dataHead.POLE_LON.ToString();
        //    dataInfo += Environment.NewLine + "Reference pnt lat/lon: " +
        //                    aDataInfo.dataHead.REF_LAT.ToString() + "  " + aDataInfo.dataHead.REF_LON.ToString();
        //    dataInfo += Environment.NewLine + "Grid Size: " + aDataInfo.dataHead.SIZE.ToString();
        //    dataInfo += Environment.NewLine + "Orientation: " + aDataInfo.dataHead.ORIENT.ToString();
        //    dataInfo += Environment.NewLine + "Tan lat/cone: " + aDataInfo.dataHead.TANG_LAT.ToString();
        //    dataInfo += Environment.NewLine + "Syn pnt x/y: " + aDataInfo.dataHead.SYNC_XP.ToString() +
        //        "  " + aDataInfo.dataHead.SYNC_YP.ToString();
        //    dataInfo += Environment.NewLine + "Syn pnt lat/lon: " + aDataInfo.dataHead.SYNC_LAT.ToString() +
        //        "  " + aDataInfo.dataHead.SYNC_LON.ToString();


        //    return dataInfo;
        //}
    }
}
