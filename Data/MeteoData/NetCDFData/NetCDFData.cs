using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// NetCDF data
    /// </summary>
    public class NetCDFData
    {
        #region Methods
        /// <summary>
        /// Join data files
        /// </summary>
        /// <param name="inFiles">input nc files</param>
        /// <param name="outFile">joined output nc file</param>
        public static void JoinDataFiles(List<string> inFiles, string outFile)
        {
            JoinDataFiles(inFiles, outFile, "time");
        }

        /// <summary>
        /// Join data files
        /// </summary>
        /// <param name="inFiles">input nc files</param>
        /// <param name="outFile">joined output nc file</param>
        /// <param name="tDimName">time dimension name</param>
        public static void JoinDataFiles(List<string> inFiles, string outFile, string tDimName)
        {
            //Check number of selected files
            int fNum = inFiles.Count;
            if (fNum < 2)
            {
                MessageBox.Show("There should be at least two files!", "Error");
                return;
            }

            //Check top two files to decide joining time or variables
            string aFile = inFiles[0];
            string bFile = inFiles[1];

            NetCDFDataInfo aDataInfo = new NetCDFDataInfo();
            NetCDFDataInfo bDataInfo = new NetCDFDataInfo();

            aDataInfo.ReadDataInfo(aFile);
            bDataInfo.ReadDataInfo(bFile);

            //If can be joined
            int dataJoinType = GetDataJoinType(aDataInfo, bDataInfo, tDimName);
            if (dataJoinType == 0)
            {
                MessageBox.Show("Data dimensions are not same!", "Error");
                return;
            }

            //Join data
            if (dataJoinType == 2)    //Join variables
            {
                JoinDataFiles_Variable(inFiles, outFile);
            }
            else    //Join time
            {
                JoinDataFiles_Time(inFiles, outFile, tDimName);
            }
        }

        /// <summary>
        /// Join data files
        /// </summary>
        /// <param name="inFiles">input nc files</param>
        /// <param name="outFile">joined output nc file</param>
        /// <param name="timeDimStr">time dimension name string</param>
        public static void JoinDataFiles_Time(List<string> inFiles, string outFile, string timeDimStr)
        {
            string timeVarStr = timeDimStr;           

            //Check number of selected files
            int fNum = inFiles.Count;
            if (fNum < 2)
            {
                MessageBox.Show("There should be at least two files!", "Error");
                return;
            }

            //Open the first file
            int i, j;
            string aFile = inFiles[0];
            NetCDFDataInfo aDataInfo = new NetCDFDataInfo();
            aDataInfo.ReadDataInfo(aFile);

            switch (aDataInfo.Convention)
            {
                case Conventions.WRFOUT:
                    timeDimStr = "Time";
                    timeVarStr = "Times";
                    break;
            }

            //Change time dimension as unlimit
            for (i = 0; i < aDataInfo.Dimensions.Count; i++)
            {
                Dimension aDimS = aDataInfo.Dimensions[i];
                if (aDimS.DimName == timeDimStr)
                {
                    //aDimS.dimLen = -1;
                    //aDataInfo.dimList[i] = aDimS;
                    aDataInfo.unlimdimid = aDimS.DimId;
                    break;
                }
            }

            for (i = 0; i < aDataInfo.nvars; i++)
            {
                if (Global.MIMath.IsNumeric(aDataInfo.Variables[i].Name.Substring(0, 1)))
                    aDataInfo.Variables[i].Name = 'V' + aDataInfo.Variables[i].Name;
            }

            int tDimid = 0;
            NetCDF4.nc_inq_dimid(aDataInfo.ncid, timeDimStr, out tDimid);

            //object[] sttu = aDataInfo.GetStartTimeInfo();
            //DateTime sTime = (DateTime)sttu[0];
            //TimeUnit aTU = (TimeUnit)sttu[1];

            //Create output nc file and write the data of the first file
            aDataInfo.CreateNCFile(outFile);

            //Write the data of first data file
            NetCDFDataInfo bDataInfo = new NetCDFDataInfo();
            bDataInfo.ReadDataInfo(inFiles[0]);
            for (i = 0; i < bDataInfo.nvars; i++)
            {
                Variable aVarS = aDataInfo.Variables[i];
                int dimNum = aVarS.DimNumber;
                int[] start = new int[dimNum];
                int[] count = new int[dimNum];
                if (dimNum == 4)
                {
                    start[2] = 0;
                    count[2] = aVarS.Dimensions[2].DimLength;
                    start[3] = 0;
                    count[3] = aVarS.Dimensions[3].DimLength; 
                    for (int d1 = 0; d1 < aVarS.Dimensions[0].DimLength; d1++)
                    {
                        start[0] = d1;
                        count[0] = 1;
                        for (int d2 = 0; d2 < aVarS.Dimensions[1].DimLength; d2++)
                        {
                            start[1] = d2;
                            count[1] = 1;

                            object[] varaData = new object[1];
                            bDataInfo.GetVaraData(aVarS, start, count, ref varaData);
                            aDataInfo.WriteVara(aVarS, start, count, varaData);
                        }
                    }
                }
                else if (dimNum == 3)
                {
                    start[1] = 0;
                    count[1] = aVarS.Dimensions[1].DimLength;
                    start[2] = 0;
                    count[2] = aVarS.Dimensions[2].DimLength;
                    for (int d1 = 0; d1 < aVarS.Dimensions[0].DimLength; d1++)
                    {
                        start[0] = d1;
                        count[0] = 1;

                        object[] varaData = new object[1];
                        bDataInfo.GetVaraData(aVarS, start, count, ref varaData);
                        aDataInfo.WriteVara(aVarS, start, count, varaData);
                    }
                }
                else
                {
                    for (int v = 0; v < aVarS.DimNumber; v++)
                    {
                        start[v] = 0;
                        count[v] = bDataInfo.Dimensions[aVarS.Dimensions[v].DimId].DimLength;

                        object[] varData = new object[1];
                        bDataInfo.GetVarData(aVarS, ref varData);
                        aDataInfo.WriteVara(aVarS, start, count, varData);
                    }
                }                                
            }

            //Join data                        
            //Add data
            for (i = 1; i < fNum; i++)
            {
                aFile = inFiles[i];
                bDataInfo = new NetCDFDataInfo();
                bDataInfo.ReadDataInfo(aFile);

                int tDimNum = 1;
                NetCDF4.nc_inq_dimlen(aDataInfo.ncid, tDimid, out tDimNum);
                for (j = 0; j < bDataInfo.nvars; j++)
                {
                    Variable aVarS = bDataInfo.Variables[j];
                    int vIdx = aDataInfo.VariableNames.IndexOf(aVarS.Name);
                    if (vIdx < 0)
                        continue;

                    if (Array.IndexOf(aVarS.DimIds, tDimid) < 0)
                        continue;

                    int dimNum = aVarS.DimNumber;
                    int[] start = new int[dimNum];
                    int[] count = new int[dimNum];
                    if (dimNum == 4)
                    {
                        start[2] = 0;
                        count[2] = aVarS.Dimensions[2].DimLength;
                        start[3] = 0;
                        count[3] = aVarS.Dimensions[3].DimLength;
                        for (int d1 = 0; d1 < aVarS.Dimensions[0].DimLength; d1++)
                        {
                            start[0] = d1;
                            count[0] = 1;
                            for (int d2 = 0; d2 < aVarS.Dimensions[1].DimLength; d2++)
                            {
                                start[0] = d1;
                                start[1] = d2;
                                count[1] = 1;

                                object[] varaData = new object[1];
                                bDataInfo.GetVaraData(aVarS, start, count, ref varaData);
                                start[Array.IndexOf(aVarS.DimIds, tDimid)] += tDimNum;
                                bDataInfo.WriteVaraData(aDataInfo.ncid, aDataInfo.Variables[vIdx].VarId, aVarS.NCType, start, count, varaData);
                            }
                        }
                    }
                    else if (dimNum == 3)
                    {
                        start[1] = 0;
                        count[1] = aVarS.Dimensions[1].DimLength;
                        start[2] = 0;
                        count[2] = aVarS.Dimensions[2].DimLength;
                        for (int d1 = 0; d1 < aVarS.Dimensions[0].DimLength; d1++)
                        {
                            start[0] = d1;
                            count[0] = 1;

                            object[] varaData = new object[1];
                            bDataInfo.GetVaraData(aVarS, start, count, ref varaData);
                            start[Array.IndexOf(aVarS.DimIds, tDimid)] += tDimNum;
                            bDataInfo.WriteVaraData(aDataInfo.ncid, aDataInfo.Variables[vIdx].VarId, aVarS.NCType, start, count, varaData);
                        }
                    }
                    else
                    {
                        for (int v = 0; v < aVarS.DimNumber; v++)
                        {
                            start[v] = 0;
                            count[v] = bDataInfo.Dimensions[aVarS.Dimensions[v].DimId].DimLength;

                            object[] varData = new object[1];
                            bDataInfo.GetVarData(aVarS, ref varData);
                            start[Array.IndexOf(aVarS.DimIds, tDimid)] = tDimNum;
                            bDataInfo.WriteVaraData(aDataInfo.ncid, aDataInfo.Variables[vIdx].VarId, aVarS.NCType, start, count, varData);
                        }
                    }                  
                }
            }

            //Close data file
            aDataInfo.CloseNCFile();
        }

        /// <summary>
        /// Join data files
        /// </summary>
        /// <param name="inFiles">input nc files</param>
        /// <param name="outFile">joined output nc file</param>
        /// <param name="timeDimStr">time dimension name string</param>
        public static void JoinDataFiles_Time_bak2(List<string> inFiles, string outFile, string timeDimStr)
        {
            //Check number of selected files
            int fNum = inFiles.Count;
            if (fNum < 2)
            {
                MessageBox.Show("There should be at least two files!", "Error");
                return;
            }

            //Open the first file
            int i, j;
            string aFile = inFiles[0];
            NetCDFDataInfo aDataInfo = new NetCDFDataInfo();
            aDataInfo.ReadDataInfo(aFile);
            //Change time dimension as unlimit
            for (i = 0; i < aDataInfo.Dimensions.Count; i++)
            {
                Dimension aDimS = aDataInfo.Dimensions[i];
                if (aDimS.DimName == timeDimStr)
                {
                    //aDimS.dimLen = -1;
                    //aDataInfo.dimList[i] = aDimS;
                    aDataInfo.unlimdimid = aDimS.DimId;
                    break;
                }
            }

            for (i = 0; i < aDataInfo.nvars; i++)
            {
                if (Global.MIMath.IsNumeric(aDataInfo.Variables[i].Name.Substring(0, 1)))
                    aDataInfo.Variables[i].Name = 'V' + aDataInfo.Variables[i].Name;
            }

            int tDimid = 0;
            NetCDF4.nc_inq_dimid(aDataInfo.ncid, timeDimStr, out tDimid);

            object[] sttu = aDataInfo.GetStartTimeInfo();
            DateTime sTime = (DateTime)sttu[0];
            TimeUnit aTU = (TimeUnit)sttu[1];

            //Create output nc file and write the data of the first file
            aDataInfo.CreateNCFile(outFile);

            //Write the data of first data file
            NetCDFDataInfo bDataInfo = new NetCDFDataInfo();
            bDataInfo.ReadDataInfo(inFiles[0]);
            for (i = 0; i < bDataInfo.nvars; i++)
            {
                Variable aVarS = aDataInfo.Variables[i];
                int dimNum = aVarS.DimNumber;
                int[] start = new int[dimNum];
                int[] count = new int[dimNum];
                if (dimNum == 4)
                {
                    start[2] = 0;
                    count[2] = aVarS.Dimensions[2].DimLength;
                    start[3] = 0;
                    count[3] = aVarS.Dimensions[3].DimLength;
                    for (int d1 = 0; d1 < aVarS.Dimensions[0].DimLength; d1++)
                    {
                        start[0] = d1;
                        count[0] = 1;
                        for (int d2 = 0; d2 < aVarS.Dimensions[1].DimLength; d2++)
                        {
                            start[1] = d2;
                            count[1] = 1;

                            object[] varaData = new object[1];
                            bDataInfo.GetVaraData(aVarS, start, count, ref varaData);
                            aDataInfo.WriteVara(aVarS, start, count, varaData);
                        }
                    }
                }
                else if (dimNum == 3)
                {
                    start[1] = 0;
                    count[1] = aVarS.Dimensions[1].DimLength;
                    start[2] = 0;
                    count[2] = aVarS.Dimensions[2].DimLength;
                    for (int d1 = 0; d1 < aVarS.Dimensions[0].DimLength; d1++)
                    {
                        start[0] = d1;
                        count[0] = 1;

                        object[] varaData = new object[1];
                        bDataInfo.GetVaraData(aVarS, start, count, ref varaData);
                        aDataInfo.WriteVara(aVarS, start, count, varaData);
                    }
                }
                else
                {
                    for (int v = 0; v < aVarS.DimNumber; v++)
                    {
                        start[v] = 0;
                        count[v] = bDataInfo.Dimensions[aVarS.Dimensions[v].DimId].DimLength;

                        object[] varData = new object[1];
                        bDataInfo.GetVarData(aVarS, ref varData);
                        aDataInfo.WriteVara(aVarS, start, count, varData);
                    }
                }
            }

            //Join data                        
            //Add data
            for (i = 1; i < fNum; i++)
            {
                aFile = inFiles[i];
                bDataInfo = new NetCDFDataInfo();
                bDataInfo.ReadDataInfo(aFile);

                int tDimNum = 1;
                NetCDF4.nc_inq_dimlen(aDataInfo.ncid, tDimid, out tDimNum);
                for (j = 0; j < bDataInfo.nvars; j++)
                {
                    Variable aVarS = bDataInfo.Variables[j];
                    int vIdx = aDataInfo.VariableNames.IndexOf(aVarS.Name);
                    if (vIdx < 0)
                        continue;

                    if (aVarS.DimNumber > 1)
                    {
                        if (Array.IndexOf(aVarS.DimIds, tDimid) < 0)
                            continue;

                        int dimNum = aVarS.DimNumber;
                        int[] start = new int[dimNum];
                        int[] count = new int[dimNum];
                        if (dimNum == 4)
                        {
                            start[2] = 0;
                            count[2] = aVarS.Dimensions[2].DimLength;
                            start[3] = 0;
                            count[3] = aVarS.Dimensions[3].DimLength;
                            for (int d1 = 0; d1 < aVarS.Dimensions[0].DimLength; d1++)
                            {
                                start[0] = d1;
                                count[0] = 1;
                                for (int d2 = 0; d2 < aVarS.Dimensions[1].DimLength; d2++)
                                {
                                    start[1] = d2;
                                    count[1] = 1;

                                    object[] varaData = new object[1];
                                    bDataInfo.GetVaraData(aVarS, start, count, ref varaData);
                                    start[Array.IndexOf(aVarS.DimIds, tDimid)] = tDimNum;
                                    bDataInfo.WriteVaraData(aDataInfo.ncid, aDataInfo.Variables[vIdx].VarId, aVarS.NCType, start, count, varaData);
                                }
                            }
                        }
                        else if (dimNum == 3)
                        {
                            start[1] = 0;
                            count[1] = aVarS.Dimensions[1].DimLength;
                            start[2] = 0;
                            count[2] = aVarS.Dimensions[2].DimLength;
                            for (int d1 = 0; d1 < aVarS.Dimensions[0].DimLength; d1++)
                            {
                                start[0] = d1;
                                count[0] = 1;

                                object[] varaData = new object[1];
                                bDataInfo.GetVaraData(aVarS, start, count, ref varaData);
                                start[Array.IndexOf(aVarS.DimIds, tDimid)] = tDimNum;
                                bDataInfo.WriteVaraData(aDataInfo.ncid, aDataInfo.Variables[vIdx].VarId, aVarS.NCType, start, count, varaData);
                            }
                        }
                        else
                        {
                            for (int v = 0; v < aVarS.DimNumber; v++)
                            {
                                start[v] = 0;
                                count[v] = bDataInfo.Dimensions[aVarS.Dimensions[v].DimId].DimLength;

                                object[] varData = new object[1];
                                bDataInfo.GetVarData(aVarS, ref varData);
                                start[Array.IndexOf(aVarS.DimIds, tDimid)] = tDimNum;
                                bDataInfo.WriteVaraData(aDataInfo.ncid, aDataInfo.Variables[vIdx].VarId, aVarS.NCType, start, count, varData);
                            }
                        }
                    }

                    if (aVarS.Name == timeDimStr)
                    {
                        object[] varData = new object[1];
                        bDataInfo.GetVarData(aVarS, ref varData);
                        sttu = bDataInfo.GetStartTimeInfo();
                        DateTime nTime = (DateTime)sttu[0];
                        TimeUnit nTU = (TimeUnit)sttu[1];
                        if (DateTime.Compare(sTime, nTime) != 0)
                        {
                            double shift = 0.0;
                            switch (aTU)
                            {
                                case TimeUnit.Day:
                                    shift = nTime.Subtract(sTime).TotalDays;
                                    break;
                                case TimeUnit.Hour:
                                    shift = nTime.Subtract(sTime).TotalHours;
                                    break;
                                case TimeUnit.Minute:
                                    shift = nTime.Subtract(sTime).TotalMinutes;
                                    break;
                                case TimeUnit.Second:
                                    shift = nTime.Subtract(sTime).TotalSeconds;
                                    break;
                            }
                            for (int d = 0; d < varData.Length; d++)
                            {
                                varData[d] = (double)varData[d] + shift;
                            }
                        }

                        int[] start = new int[aVarS.DimNumber];
                        int[] count = new int[aVarS.DimNumber];
                        for (int v = 0; v < aVarS.DimNumber; v++)
                        {
                            start[v] = 0;
                            count[v] = bDataInfo.Dimensions[aVarS.Dimensions[v].DimId].DimLength;
                        }
                        start[Array.IndexOf(aVarS.DimIds, tDimid)] = tDimNum;
                        bDataInfo.WriteVaraData(aDataInfo.ncid, aDataInfo.Variables[vIdx].VarId, aVarS.NCType, start, count, varData);
                    }
                }
            }

            //Close data file
            aDataInfo.CloseNCFile();
        }

        /// <summary>
        /// Join data files
        /// </summary>
        /// <param name="inFiles">input nc files</param>
        /// <param name="outFile">joined output nc file</param>
        /// <param name="timeDimStr">time dimension name string</param>
        public static void JoinDataFiles_Time_bak1(List<string> inFiles, string outFile, string timeDimStr)
        {
            //Check number of selected files
            int fNum = inFiles.Count;
            if (fNum < 2)
            {
                MessageBox.Show("There should be at least two files!", "Error");
                return;
            }

            //Open the first file
            int i, j;
            string aFile = inFiles[0];
            NetCDFDataInfo aDataInfo = new NetCDFDataInfo();
            aDataInfo.ReadDataInfo(aFile);
            //Change time dimension as unlimit
            for (i = 0; i < aDataInfo.Dimensions.Count; i++)
            {
                Dimension aDimS = aDataInfo.Dimensions[i];
                if (aDimS.DimName == timeDimStr)
                {
                    //aDimS.dimLen = -1;
                    //aDataInfo.dimList[i] = aDimS;
                    aDataInfo.unlimdimid = aDimS.DimId;
                    break;
                }
            }

            for (i = 0; i < aDataInfo.nvars; i++)
            {
                if (Global.MIMath.IsNumeric(aDataInfo.Variables[i].Name.Substring(0, 1)))
                    aDataInfo.Variables[i].Name = 'V' + aDataInfo.Variables[i].Name;
            }

            int tDimid = 0;
            NetCDF4.nc_inq_dimid(aDataInfo.ncid, timeDimStr, out tDimid);
                        
            object[] sttu = aDataInfo.GetStartTimeInfo();
            DateTime sTime = (DateTime)sttu[0];
            TimeUnit aTU = (TimeUnit)sttu[1];

            //Create output nc file and write the data of the first file
            aDataInfo.CreateNCFile(outFile);

            //Write the data of first data file
            NetCDFDataInfo bDataInfo = new NetCDFDataInfo();
            bDataInfo.ReadDataInfo(inFiles[0]);
            for (i = 0; i < bDataInfo.nvars; i++)
            {
                Variable aVarS = aDataInfo.Variables[i];
                object[] varData = new object[1];
                bDataInfo.GetVarData(aVarS, ref varData);

                int[] start = new int[aVarS.DimNumber];
                int[] count = new int[aVarS.DimNumber];
                for (int v = 0; v < aVarS.DimNumber; v++)
                {
                    start[v] = 0;
                    count[v] = bDataInfo.Dimensions[aVarS.Dimensions[v].DimId].DimLength;
                }
                aDataInfo.WriteVara(aVarS, start, count, varData);
            }

            //Join data                        
            //Add data
            for (i = 1; i < fNum; i++)
            {
                aFile = inFiles[i];
                bDataInfo = new NetCDFDataInfo();
                bDataInfo.ReadDataInfo(aFile);

                int tDimNum = 1;
                NetCDF4.nc_inq_dimlen(aDataInfo.ncid, tDimid, out tDimNum);
                for (j = 0; j < bDataInfo.nvars; j++)
                {
                    Variable aVarS = bDataInfo.Variables[j];
                    int vIdx = aDataInfo.VariableNames.IndexOf(aVarS.Name);
                    if (vIdx < 0)
                        continue;
                    
                    if (aVarS.DimNumber > 1)
                    {
                        if (Array.IndexOf(aVarS.DimIds, tDimid) < 0)
                            continue;

                        object[] varData = new object[1];
                        bDataInfo.GetVarData(aVarS, ref varData);

                        int[] start = new int[aVarS.DimNumber];
                        int[] count = new int[aVarS.DimNumber];
                        for (int v = 0; v < aVarS.DimNumber; v++)
                        {
                            start[v] = 0;
                            count[v] = bDataInfo.Dimensions[aVarS.Dimensions[v].DimId].DimLength;
                        }
                        start[Array.IndexOf(aVarS.DimIds, tDimid)] = tDimNum;
                        bDataInfo.WriteVaraData(aDataInfo.ncid, aDataInfo.Variables[vIdx].VarId, aVarS.NCType, start, count, varData);
                    }

                    if (aVarS.Name == timeDimStr)
                    {
                        object[] varData = new object[1];
                        bDataInfo.GetVarData(aVarS, ref varData);
                        sttu = bDataInfo.GetStartTimeInfo();
                        DateTime nTime = (DateTime)sttu[0];
                        TimeUnit nTU = (TimeUnit)sttu[1];
                        if (DateTime.Compare(sTime, nTime) != 0)
                        {
                            double shift = 0.0;
                            switch (aTU)
                            {
                                case TimeUnit.Day:
                                    shift = nTime.Subtract(sTime).TotalDays;
                                    break;
                                case TimeUnit.Hour:
                                    shift = nTime.Subtract(sTime).TotalHours;
                                    break;
                                case TimeUnit.Minute:
                                    shift = nTime.Subtract(sTime).TotalMinutes;
                                    break;
                                case TimeUnit.Second:
                                    shift = nTime.Subtract(sTime).TotalSeconds;
                                    break;
                            }
                            for (int d = 0; d < varData.Length; d++)
                            {
                                varData[d] = (double)varData[d] + shift;
                            }
                        }

                        int[] start = new int[aVarS.DimNumber];
                        int[] count = new int[aVarS.DimNumber];
                        for (int v = 0; v < aVarS.DimNumber; v++)
                        {
                            start[v] = 0;
                            count[v] = bDataInfo.Dimensions[aVarS.Dimensions[v].DimId].DimLength;
                        }
                        start[Array.IndexOf(aVarS.DimIds, tDimid)] = tDimNum;
                        bDataInfo.WriteVaraData(aDataInfo.ncid, aDataInfo.Variables[vIdx].VarId, aVarS.NCType, start, count, varData);
                    }
                }
            }

            //Close data file
            aDataInfo.CloseNCFile();
        }

        /// <summary>
        /// Join data files
        /// </summary>
        /// <param name="inFiles">input nc files</param>
        /// <param name="outFile">joined output nc file</param>
        /// <param name="timeDimStr">time dimension name string</param>
        public static void JoinDataFiles_Time_bak(List<string> inFiles, string outFile, string timeDimStr)
        {
            //Check number of selected files
            int fNum = inFiles.Count;
            if (fNum < 2)
            {
                MessageBox.Show("There should be at least two files!", "Error");
                return;
            }

            //Check top two files to decide joining time or variables
            string aFile = inFiles[0];

            NetCDFDataInfo aDataInfo = new NetCDFDataInfo();
            aDataInfo.ReadDataInfo(aFile);

            //Join data
            int i, j, res = 0;

            //Copy first file to output file                   
            File.Copy(aDataInfo.FileName, outFile, true);

            //Open output file
            int ncid = 0;
            res = NetCDF4.nc_open(outFile, (int)NetCDF4.CreateMode.NC_WRITE, out ncid);
            if (res != 0) { goto ERROR; }
            int tDimid = 0;
            NetCDF4.nc_inq_dimid(ncid, timeDimStr, out tDimid);

            //Add data
            for (i = 1; i < fNum; i++)
            {
                aFile = inFiles[i];
                NetCDFDataInfo bDataInfo = new NetCDFDataInfo();
                bDataInfo.ReadDataInfo(aFile);
                if (GetDataJoinType(aDataInfo, bDataInfo, timeDimStr) != 1)
                    continue;

                int tDimNum = 1;
                NetCDF4.nc_inq_dimlen(ncid, tDimid, out tDimNum);
                for (j = 0; j < bDataInfo.nvars; j++)
                {
                    Variable aVarS = bDataInfo.Variables[j];
                    if (aVarS.DimNumber > 1 || aVarS.Name == timeDimStr)
                    {
                        if (Array.IndexOf(aVarS.DimIds, tDimid) < 0)
                            continue;

                        object[] varData = new object[1];
                        bDataInfo.GetVarData(bDataInfo.Variables[j], ref varData);

                        int[] start = new int[aVarS.DimNumber];
                        int[] count = new int[aVarS.DimNumber];
                        for (int v = 0; v < aVarS.DimNumber; v++)
                        {
                            start[v] = 0;
                            count[v] = bDataInfo.Dimensions[aVarS.DimIds[v]].DimLength;
                        }
                        start[Array.IndexOf(aVarS.DimIds, tDimid)] = tDimNum;
                        bDataInfo.WriteVaraData(ncid, aVarS.VarId, aVarS.NCType, start, count, varData);
                    }
                }
            }

            //Close data file
            res = NetCDF4.nc_close(ncid);
            if (res != 0) { goto ERROR; }

            return;
        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");

            return;
        }

        /// <summary>
        /// Join data files
        /// </summary>
        /// <param name="inFiles">input nc files</param>
        /// <param name="outFile">joined output nc file</param>
        public static void JoinDataFiles_Variable(List<string> inFiles, string outFile)
        {
            //Check number of selected files
            int fNum = inFiles.Count;
            if (fNum < 2)
            {
                MessageBox.Show("There should be at least two files!", "Error");
                return;
            }

            //Check top two files to decide joining time or variables
            string aFile = inFiles[0];

            NetCDFDataInfo aDataInfo = new NetCDFDataInfo();
            aDataInfo.ReadDataInfo(aFile);

            //Join data
            int i, j, res = 0;
            //Copy first file to output file                   
            File.Copy(aDataInfo.FileName, outFile, true);

            //Open output file
            int ncid = 0;
            res = NetCDF4.nc_open(outFile, (int)NetCDF4.CreateMode.NC_WRITE, out ncid);
            if (res != 0) { goto ERROR; }

            //Add data
            for (i = 1; i < fNum; i++)
            {
                aFile = inFiles[i];
                NetCDFDataInfo bDataInfo = new NetCDFDataInfo();
                bDataInfo.ReadDataInfo(aFile);
                //if (GetDataJoinType(aDataInfo, bDataInfo, "time") != 2)
                //    continue;

                for (j = 0; j < bDataInfo.nvars; j++)
                {
                    Variable aVarS = (Variable)bDataInfo.Variables[j].Clone();
                    if (aVarS.DimNumber > 1 && (!aDataInfo.VariableNames.Contains(aVarS.Name)))
                    {
                        aDataInfo.AddNewVariable(aVarS, ncid);

                        object[] varData = new object[1];
                        if (aVarS.DimNumber > 1)
                        {
                            int[] start = new int[aVarS.DimNumber];
                            int[] count = new int[aVarS.DimNumber];
                            for (int v = 1; v < aVarS.DimNumber; v++)
                            {
                                start[v] = 0;
                                count[v] = aVarS.Dimensions[v].DimLength;
                            }
                            for (int d = 0; d < aVarS.Dimensions[0].DimLength; d++)
                            {
                                start[0] = d;
                                count[0] = 1;

                                bDataInfo.GetVaraData(bDataInfo.Variables[j], start, count, ref varData);
                                aDataInfo.WriteVaraData(ncid, aVarS.VarId, aVarS.NCType, start, count, varData);
                            }
                        }
                        else
                        {
                            bDataInfo.GetVarData(bDataInfo.Variables[j], ref varData);
                            aDataInfo.WriteVarData(ncid, aVarS.VarId, aVarS.NCType, varData);
                        }
                    }
                }
            }

            //Close data file
            res = NetCDF4.nc_close(ncid);
            if (res != 0) { goto ERROR; }

            return;
        ERROR:
            MessageBox.Show("Error: " + NetCDF4.nc_strerror(res), "Error");

            return;
        }

        private static int GetDataJoinType(NetCDFDataInfo aDataInfo, NetCDFDataInfo bDataInfo, string tDimName)
        {
            //If same dimension number
            if (aDataInfo.ndims != bDataInfo.ndims)
            {
                return 0;  //Can't be joined
            }

            //If same dimensions
            int i;
            bool IsSame = true;
            bool IsJoinVar = true;
            for (i = 0; i < aDataInfo.ndims; i++)
            {
                Dimension aDim = aDataInfo.Dimensions[i];
                Dimension bDim = bDataInfo.Dimensions[i];
                if (aDim.DimName != bDim.DimName)
                {
                    IsSame = false;
                    break;
                }
                if (aDim.DimName.ToLower() == tDimName)
                {
                    if (aDim.DimLength != bDim.DimLength)
                        IsJoinVar = false;

                    for (int j = 0; j < aDataInfo.times.Count; j++)
                    {
                        if (aDataInfo.times[j] != bDataInfo.times[j])
                        {
                            IsJoinVar = false;
                            break;
                        }
                    }
                }
                else
                {
                    if (aDim.DimLength != bDim.DimLength)
                    {
                        IsSame = false;
                        break;
                    }
                }
            }
            if (!IsSame)
            {
                return 0;    //Can't be joined
            }

            if (IsJoinVar)
                return 2;    //Can join variable
            else
            {
                if (aDataInfo.nvars != bDataInfo.nvars)
                    return 0;

                IsSame = true;
                for (i = 0; i < aDataInfo.nvars; i++)
                {
                    Variable aVarS = aDataInfo.Variables[i];
                    Variable bVarS = bDataInfo.Variables[i];
                    if (aVarS.Name != bVarS.Name || aVarS.DimNumber != bVarS.DimNumber)
                        IsSame = false;
                }
                if (IsSame)
                    return 1;    //Can join time
                else
                    return 0;
            }
        }

        /// <summary>
        /// Add time dimension
        /// </summary>
        /// <param name="inFile">input nc file</param>
        /// <param name="outFile">output nc file</param>
        /// <param name="aTime">time</param>
        public static void AddTimeDimension(string inFile, string outFile, DateTime aTime)
        {
            AddTimeDimension(inFile, outFile, aTime, "days");
        }

        /// <summary>
        /// Add time dimension
        /// </summary>
        /// <param name="inFile">input nc file</param>
        /// <param name="outFile">output nc file</param>
        /// <param name="aTime">time</param>
        /// <param name="timeUnit">time unit (days, minutes, seconds)</param>
        public static void AddTimeDimension(string inFile, string outFile, DateTime aTime, string timeUnit)
        {            
            //Set data info
            NetCDFDataInfo aDataInfo = new NetCDFDataInfo();
            aDataInfo.ReadDataInfo(inFile);            
            NetCDFDataInfo bDataInfo = (NetCDFDataInfo)aDataInfo.Clone();

            //Check variables if time included
            List<string> varList = new List<string>();
            int j;
            for (j = 0; j < aDataInfo.Variables.Count; j++)
            {
                varList.Add(aDataInfo.Variables[j].Name.ToLower());
            }
            if (varList.Contains("time"))
            {
                return;
            }

            //set start time of the data
            DateTime sTime = new DateTime();
            double tValue = 0;
            switch (timeUnit.ToLower())
            {
                case "minutes":
                    sTime = aTime.AddYears(-1);
                    tValue = (aTime - sTime).TotalMinutes;
                    break;
                case "seconds":
                    sTime = aTime.AddYears(-1);
                    tValue = (aTime - sTime).TotalSeconds;
                    break;   
                default:
                    sTime = DateTime.Parse("1800-1-1 00:00:00");
                    tValue = (aTime - sTime).TotalDays;
                    break;
            }            

            //Set data info, Add time dimension and variable
            Dimension tDim = new Dimension();
            tDim.DimName = "time";
            tDim.DimLength = 1;
            tDim.DimId = bDataInfo.ndims;
            bDataInfo.AddDimension(tDim);

            Variable tVar = new Variable();
            List<AttStruct> attList = new List<AttStruct>();
            AttStruct aAtts = new AttStruct();
            aAtts.attName = "units";
            aAtts.NCType = NetCDF4.NcType.NC_CHAR;
            aAtts.attValue = timeUnit.ToLower() +  " since " + sTime.ToString("yyyy-M-d HH:mm:ss");
            aAtts.attLen = ((string)aAtts.attValue).Length;
            attList.Add(aAtts);
            aAtts = new AttStruct();
            aAtts.attName = "long_name";
            aAtts.NCType = NetCDF4.NcType.NC_CHAR;
            aAtts.attValue = "Time";
            aAtts.attLen = ((string)aAtts.attValue).Length;
            attList.Add(aAtts);
            //aAtts = new AttStruct();
            //aAtts.attName = "delta_t";
            //aAtts.ncType = NetCDF4.NcType.NC_CHAR;
            //aAtts.attValue = "0000-00-01 00:00:00";
            //aAtts.attLen = ((string)aAtts.attValue).Length;
            //attList.Add(aAtts);
            tVar.Attributes = attList;
            tVar.Dimensions.Add(tDim);            
            tVar.AttNumber = attList.Count;
            tVar.NCType = NetCDF4.NcType.NC_DOUBLE;
            //tVar.nDims = 1;
            tVar.VarId = bDataInfo.nvars;
            tVar.Name = "time";
            //tVar.isDataVar = false;
            bDataInfo.AddVariable(tVar);
            for (j = 0; j < bDataInfo.Variables.Count; j++)
            {
                Variable aVarS = bDataInfo.Variables[j];
                if (aVarS.DimNumber > 1)
                {
                    aVarS.Dimensions.Insert(0, tDim);
                    //for (int d = 0; d < aVarS.DimNumber; d++)
                    //    aVarS.Dimensions.Add(aVarS.Dimensions[d]);
                    //aVarS.nDims += 1;
                    bDataInfo.Variables[j] = aVarS;
                }
            }

            //Get and set data array
            object[] dataArray = new object[0];
            int cLen = 0;
            for (j = 0; j < aDataInfo.nvars; j++)
            {
                object[] varData = new object[1];
                if (aDataInfo.GetVarData(aDataInfo.Variables[j], ref varData))
                {
                    Array.Resize(ref dataArray, cLen + varData.Length);
                    Array.Copy(varData, 0, dataArray, cLen, varData.Length);
                    cLen = dataArray.Length;
                }
            }
            Array.Resize(ref dataArray, cLen + 1);
            dataArray[cLen] = tValue;

            //Write NetCDF data file                                
            bDataInfo.WriteNetCDFData(outFile, dataArray);
        }

        #endregion        
    }
}
