using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MeteoInfoC.Analysis
{
    /// <summary>
    /// Clustering
    /// </summary>
    public class Clustering
    {
        
        /// <summary>
        /// Clustering calculation
        /// </summary>
        /// <param name="inFile">input file</param>
        /// <param name="outFile">output file</param>
        /// <param name="N">row number</param>
        /// <param name="M">column number</param>
        /// <param name="LN">Level number</param>
        /// <param name="DISTYPE">Distant define type: Euclidean or Angle</param>
        public static void Clustering_Cal(string inFile, string outFile, int N, int M, int LN, string DISTYPE)
        {
            double[,] DATA = new double[N, M];

            //---- Open input File            
            string aLine = null;
            string[] aDataArray = null;
            int i = 0;
            int j = 0;
            int row = 0;
            int col = 0;
            List<string> flags = new List<string>();    //Date time and height

            StreamReader sr = new StreamReader(inFile);
            row = 0;
            while (sr.Peek() >= 0)
            {
                aLine = sr.ReadLine();
                if (string.IsNullOrEmpty(aLine))
                {
                    continue;
                }
                aDataArray = aLine.Split(',');

                //Ignor the data line with not normal point number
                if ((aDataArray.Length - 2) / 3 != M / 2)
                    continue;

                flags.Add(aDataArray[0] + "," + aDataArray[1]);

                col = 0;
                for (i = 0; i <= M / 2 - 1; i++)
                {
                    for (j = 0; j <= 2; j++)
                    {
                        if (j != 2)
                        {
                            DATA[row, col] = double.Parse(aDataArray[i * 3 + j + 2]);
                            col += 1;
                        }
                    }
                }
                row += 1;
            }
            sr.Close();

            //Clustering calculation
            int[,] ICLASS = Clustering_Cal(DATA, LN, DISTYPE);

            //Write clustering result to output file
            StreamWriter sw = new StreamWriter(outFile, false);
            aLine = "Time,Height";
            for (i = 2; i <= LN; i++)
            {
                aLine = aLine + "," + i.ToString() + "CL";
            }
            sw.WriteLine(aLine);
            for (i = 0; i <= N - 1; i++)
            {
                aLine = flags[i];
                for (j = 0; j <= LN - 2; j++)
                {
                    aLine = aLine + "," + ICLASS[i, j].ToString();
                }
                sw.WriteLine(aLine);
            }
            sw.Close();
        }

        /// <summary>
        /// Clustering calculation
        /// </summary>
        /// <param name="DATA">input data array</param>
        /// <param name="outFile">output file</param>
        /// <param name="LN">Level number</param>
        /// <param name="DISTYPE">Distant define type: Euclidean or Angle</param>
        public static void Clustering_Cal(double[,] DATA, string outFile, int LN, string DISTYPE)
        {
            //double[,] DATA = new double[N, M];
            int N = DATA.GetLength(0);
            int M = DATA.GetLength(1);
            double[] CRIT = new double[N];
            double[] MEMBR = new double[N];
            double[] CRITVAL = new double[LN];
            int[] IA = new int[N];
            int[] IB = new int[N];
            int[,] ICLASS = new int[N, LN];
            int[] HVALS = new int[LN];
            int[] IORDER = new int[LN];
            int[] HEIGHT = new int[LN];
            int[] NN = new int[N];
            double[] DISNN = new double[N];
            double[] D = new double[N * (N - 1) / 2];
            bool[] FLAG = new bool[N];

            //---- IN ABOVE, 18=N, 16=M, 9=LEV, 153=N(N-1)/2
           

            //---- Call HC
            int LEN = (N * (N - 1)) / 2;
            int IOPT = 1;
            HC(N, M, IOPT, DATA, IA, IB, CRIT, MEMBR, NN, DISNN, FLAG, D, DISTYPE);

            //---- Call HCASS
            HCASS(N, IA, IB, CRIT, LN, ICLASS, HVALS, IORDER, CRITVAL, HEIGHT);

            //---- Write output file
            StreamWriter sw = new StreamWriter(outFile, false);
            int i, j;
            string aLine = "NO";
            for (i = 2; i <= LN; i++)
            {
                aLine = aLine + "," + i.ToString() + "CL";
            }
            sw.WriteLine(aLine);
            for (i = 0; i <= N - 1; i++)
            {
                aLine = (i + 1).ToString();
                for (j = 0; j <= LN - 2; j++)
                {
                    aLine = aLine + "," + ICLASS[i, j].ToString();
                }
                sw.WriteLine(aLine);
            }
            sw.Close();

            //---- Call HCDEN
            //Call HCDEN(LEV, IORDER, HEIGHT, CRITVAL)

        }

        /// <summary>
        /// Clustering calculation
        /// </summary>
        /// <param name="DATA">input data array</param>
        /// <param name="LN">Level number</param>
        /// <param name="DISTYPE">Distant define type: Euclidean or Angle</param>
        /// <returns>clustering result array</returns>
        public static int[,] Clustering_Cal(double[,] DATA, int LN, string DISTYPE)
        {
            //double[,] DATA = new double[N, M];
            int N = DATA.GetLength(0);
            int M = DATA.GetLength(1);
            double[] CRIT = new double[N];
            double[] MEMBR = new double[N];
            double[] CRITVAL = new double[LN];
            int[] IA = new int[N];
            int[] IB = new int[N];
            int[,] ICLASS = new int[N, LN];
            int[] HVALS = new int[LN];
            int[] IORDER = new int[LN];
            int[] HEIGHT = new int[LN];
            int[] NN = new int[N];
            double[] DISNN = new double[N];
            double[] D = new double[N * (N - 1) / 2];
            bool[] FLAG = new bool[N];

            //---- IN ABOVE, 18=N, 16=M, 9=LEV, 153=N(N-1)/2


            //---- Call HC
            int LEN = (N * (N - 1)) / 2;
            int IOPT = 1;
            HC(N, M, IOPT, DATA, IA, IB, CRIT, MEMBR, NN, DISNN, FLAG, D, DISTYPE);

            //---- Call HCASS
            HCASS(N, IA, IB, CRIT, LN, ICLASS, HVALS, IORDER, CRITVAL, HEIGHT);           

            //---- Call HCDEN
            //Call HCDEN(LEV, IORDER, HEIGHT, CRITVAL)

            return ICLASS;
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //                                                            C
        //  HIERARCHICAL CLUSTERING using (user-specified) criterion. C
        //                                                            C
        //  Parameters:                                               C
        //                                                            C
        //  DATA(N,M)         input data matrix,                      C
        //  DISS(LEN)         dissimilarities in lower half diagonal  C
        //                    storage; LEN = N.N-1/2,                 C
        //  IOPT              clustering criterion to be used,        C
        //  IA, IB, CRIT      history of agglomerations; dimensions   C
        //                    N, first N-1 locations only used,       C
        //  MEMBR, NN, DISNN  vectors of length N, used to store      C 
        //                    cluster cardinalities, current nearest  C
        //                    neighbour, and the dissimilarity assoc. C
        //                    with the latter.                        C
        //  FLAG              boolean indicator of agglomerable obj./ C
        //                    clusters.                               C
        //                                                            C
        //  F. Murtagh, ESA/ESO/STECF, Garching, February 1986.       C
        //                                                            C
        //------------------------------------------------------------C
        private static void HC(int N, int M, int IOPT, double[,] DATA, int[] IA, int[] IB, double[] CRIT, double[] MEMBR, 
            int[] NN, double[] DISNN, bool[] FLAG, double[] DISS, string DISTYPE)
        {
            double INF = 1.0 * (Math.Pow(10, 20));
            int i = 0;
            int j = 0;
            int k = 0;

            //---- Initializations
            for (i = 0; i <= N - 1; i++)
            {
                MEMBR[i] = 1.0;
                FLAG[i] = true;
            }
            int NCL = N;

            //---- Construct dissimilarity matrix
            int IND = 0;

            if (DISTYPE == "Angle Distance")
            {
                double X0 = 0;
                double Y0 = 0;
                double ANGLE = 0;
                double A = 0;
                double B = 0;
                double C = 0;
                X0 = DATA[0, 0];
                Y0 = DATA[0, 1];
                for (i = 0; i <= N - 2; i++)
                {
                    for (j = i + 1; j <= N - 1; j++)
                    {
                        IND = IOFFSET(N, i + 1, j + 1);
                        DISS[IND] = 0.0;
                        for (k = 1; k <= M / 2 - 1; k++)
                        {
                            A = Math.Pow((DATA[i, 2 * k] - X0), 2) + Math.Pow((DATA[i, 2 * k + 1] - Y0), 2);
                            B = Math.Pow((DATA[j, 2 * k] - X0), 2) + Math.Pow((DATA[j, 2 * k + 1] - Y0), 2);
                            C = Math.Pow((DATA[j, 2 * k] - DATA[i, 2 * k]), 2) + Math.Pow((DATA[j, 2 * k + 1] - DATA[i, 2 * k + 1]), 2);
                            if (A == 0 | B == 0)
                            {
                                ANGLE = 0;
                            }
                            else
                            {
                                ANGLE = 0.5 * (A + B - C) / Math.Sqrt(A * B);
                            }
                            if ((Math.Abs(ANGLE) > 1.0))
                            {
                                ANGLE = 1.0;
                            }
                            DISS[IND] = DISS[IND] + Math.Acos(ANGLE);
                        }
                        DISS[IND] = DISS[IND] / (M / 2);
                        if (IOPT == 1)
                            DISS[IND] = DISS[IND] / 2.0;
                        //           (Above is done for the case of the min. var. method
                        //            where merging criteria are defined in terms of variances
                        //            rather than distances.)
                    }
                }
            }
            else
            {
                for (i = 0; i <= N - 2; i++)
                {
                    for (j = i + 1; j <= N - 1; j++)
                    {
                        IND = IOFFSET(N, i + 1, j + 1);
                        DISS[IND] = 0.0;
                        for (k = 0; k <= M / 2 - 1; k++)
                        {
                            DISS[IND] = DISS[IND] + Math.Pow((DATA[i, 2 * k] - DATA[j, 2 * k]), 2) + Math.Pow((DATA[i, 2 * k + 1] - DATA[j, 2 * k + 1]), 2);
                        }
                        DISS[IND] = Math.Sqrt(DISS[IND]);
                        if (IOPT == 1)
                        {
                            DISS[IND] = DISS[IND] / 2;
                        }
                        //---- (Above is done for the case of the min. var. method
                        //---- where merging criteria are defined in terms of variances
                        //---- rather than distances.)
                    }
                }

                //For i = 0 To N - 2
                //For j = i + 1 To N - 1
                //IND = IOFFSET(N, i + 1, j + 1)
                //DISS(IND) = 0.0
                //For k = 0 To M - 1
                //DISS(IND) = DISS(IND) + (DATA(i, k) - DATA(j, k)) ^ 2
                //Next
                //If IOPT = 1 Then
                //DISS(IND) = DISS(IND) / 2
                //End If
                //---- (Above is done for the case of the min. var. method
                //---- where merging criteria are defined in terms of variances
                //---- rather than distances.)
                //Next
                //Next
            }

            //---- Carry out an agglomeration - first create list of NNs
            double DMIN = 0;
            int JM = 0;
            for (i = 0; i <= N - 2; i++)
            {
                DMIN = INF;
                for (j = i + 1; j <= N - 1; j++)
                {
                    IND = IOFFSET(N, i + 1, j + 1);
                    if (DISS[IND] >= DMIN)
                        continue;
                    DMIN = DISS[IND];
                    JM = j;
                }
                NN[i] = JM;
                DISNN[i] = DMIN;
            }

            //---- Loop 
            do
            {
                //---- Next, determine least diss. using list of NNs
                int IM = 0;
                DMIN = INF;
                for (i = 0; i <= N - 2; i++)
                {
                    if (!FLAG[i])
                        continue;
                    if (DISNN[i] >= DMIN)
                        continue;
                    DMIN = DISNN[i];
                    IM = i;
                    JM = NN[i];
                }
                NCL = NCL - 1;

                //---- This allows an agglomeration to be carried out.
                int I2 = 0;
                int J2 = 0;

                I2 = Math.Min(IM, JM);
                J2 = Math.Max(IM, JM);
                IA[N - NCL - 1] = I2 + 1;
                IB[N - NCL - 1] = J2 + 1;
                CRIT[N - NCL - 1] = DMIN;

                //---- Update dissimilarities from new cluster.
                double X = 0;
                double XX = 0;
                int IND1 = 0;
                int IND2 = 0;
                int IND3 = 0;
                int JJ = 0;
                FLAG[J2] = false;
                DMIN = INF;
                for (k = 0; k <= N - 1; k++)
                {
                    if (!FLAG[k])
                        continue;
                    if (k == I2)
                        continue;
                    X = MEMBR[I2] + MEMBR[J2] + MEMBR[k];
                    if (I2 < k)
                    {
                        IND1 = IOFFSET(N, I2 + 1, k + 1);
                    }
                    else
                    {
                        IND1 = IOFFSET(N, k + 1, I2 + 1);
                    }
                    if (J2 < k)
                    {
                        IND2 = IOFFSET(N, J2 + 1, k + 1);
                    }
                    else
                    {
                        IND2 = IOFFSET(N, k + 1, J2 + 1);
                    }
                    IND3 = IOFFSET(N, I2 + 1, J2 + 1);
                    XX = DISS[IND3];

                    //---- Ward's minimum variance method - IOPT=1.
                    if (IOPT == 1)
                    {
                        DISS[IND1] = (MEMBR[I2] + MEMBR[k]) * DISS[IND1] + (MEMBR[J2] + MEMBR[k]) * DISS[IND2] - MEMBR[k] * XX;
                        DISS[IND1] = DISS[IND1] / X;
                    }

                    //---- Single link method - IOPT=2.
                    if (IOPT == 2)
                    {
                        DISS[IND1] = Math.Min(DISS[IND1], DISS[IND2]);
                    }

                    //---- Complete link method - IOPT=3.
                    if (IOPT == 3)
                    {
                        DISS[IND1] = Math.Max(DISS[IND1], DISS[IND2]);
                    }

                    //---- Average link (or group average) method - IOPT=4.
                    if (IOPT == 4)
                    {
                        DISS[IND1] = (MEMBR[I2] * DISS[IND1] + MEMBR[J2] * DISS[IND2]) / (MEMBR[I2] + MEMBR[J2]);
                    }

                    //---- Mcquitty's method - IOPT=5.
                    if (IOPT == 5)
                    {
                        DISS[IND1] = 0.5 * DISS[IND1] + 0.5 * DISS[IND2];
                    }

                    //----  MEDIAN (GOWER'S) METHOD - IOPT=6.
                    if (IOPT == 6)
                    {
                        DISS[IND1] = 0.5 * DISS[IND1] + 0.5 * DISS[IND2] - 0.25 * XX;
                    }

                    //  CENTROID METHOD - IOPT=7.
                    if (IOPT == 7)
                    {
                        DISS[IND1] = (MEMBR[I2] * DISS[IND1] + MEMBR[J2] * DISS[IND2] - MEMBR[I2] * MEMBR[J2] * XX / (MEMBR[I2] + MEMBR[J2])) / (MEMBR[I2] + MEMBR[J2]);
                    }

                    if (I2 > k)
                        continue;
                    if (DISS[IND1] >= DMIN)
                        continue;
                    DMIN = DISS[IND1];
                    JJ = k;
                }
                MEMBR[I2] = MEMBR[I2] + MEMBR[J2];
                DISNN[I2] = DMIN;
                NN[I2] = JJ;

                //---- Update list of NNs insofar as this is required.
                for (i = 0; i <= N - 2; i++)
                {
                    if (!FLAG[i])
                        continue;
                    if (NN[i] == I2 | NN[i] == J2)
                    {
                        //---- Redetermine NN of I
                        DMIN = INF;
                        for (j = i + 1; j <= N - 1; j++)
                        {
                            IND = IOFFSET(N, i + 1, j + 1);
                            if (!FLAG[j])
                                continue;
                            if (i == j)
                                continue;
                            if (DISS[IND] >= DMIN)
                                continue;
                            DMIN = DISS[IND];
                            JJ = j;
                        }
                        NN[i] = JJ;
                        DISNN[i] = DMIN;
                    }
                }
                //frmMain.CurrentWin.TSPB_Main.Value = (N - NCL) / N * 100;
                //Application.DoEvents();
            } while (!(NCL == 1));

        }

        private static int IOFFSET(int N, int I, int J)
        {
            //  Map row I and column J of upper half diagonal symmetric matrix 
            //  onto vector.
            return J + (I - 1) * N - (I * (I + 1)) / 2 - 1;
        }

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++C
        //                                                               C
        //  Given a HIERARCHIC CLUSTERING, described as a sequence of    C
        //  agglomerations, derive the assignments into clusters for the C
        //  top LEV-1 levels of the hierarchy.                           C
        //  Prepare also the required data for representing the          C
        //  dendrogram of this top part of the hierarchy.                C
        //                                                               C
        //  Parameters:                                                  C
        //                                                               C
        //  IA, IB, CRIT: vectors of dimension N defining the agglomer-  C
        //                 ations.                                       C
        //  LEV:          number of clusters in largest partition.       C
        //  HVALS:        vector of dim. LEV, used internally only.      C
        //  ICLASS:       array of cluster assignments; dim. N by LEV.   C
        //  IORDER, CRITVAL, HEIGHT: vectors describing the dendrogram,  C
        //                all of dim. LEV.                               C
        //                                                               C
        //  F. Murtagh, ESA/ESO/STECF, Garching, February 1986.          C
        //                                                               C
        //---------------------------------------------------------------C
        private static void HCASS(int N, int[] IA, int[] IB, double[] CRIT, int LEV, int[,] ICLASS, int[] HVALS, int[] IORDER, 
            double[] CRITVAL, int[] HEIGHT)
        {
            //  Pick out the clusters which the N objects belong to,
            //  at levels N-2, N-3, ... N-LEV+1 of the hierarchy.
            //  The clusters are identified by the lowest seq. no. of
            //  their members.
            //  There are 2, 3, ... LEV clusters, respectively, for the
            //  above levels of the hierarchy.

            HVALS[0] = 1;
            HVALS[1] = IB[N - 2];
            int LOC = 2;
            int i = 0;
            int j = 0;
            bool ifGo = false;
            for (i = N - 3; i >= N - LEV; i += -1)
            {
                ifGo = true;
                for (j = 0; j <= LOC - 1; j++)
                {
                    if (IA[i] == HVALS[j])
                        ifGo = false;
                }
                if (ifGo)
                {
                    HVALS[LOC] = IA[i];
                    LOC += 1;
                }
                ifGo = true;
                for (j = 0; j <= LOC - 1; j++)
                {
                    if (IB[i] == HVALS[j])
                        ifGo = false;
                }
                if (ifGo)
                {
                    HVALS[LOC] = IB[i];
                    LOC += 1;
                }
            }

            int LEVEL = 0;
            int ICL = 0;
            int ILEV = 0;
            int NCL = 0;
            for (LEVEL = N - LEV; LEVEL <= N - 2; LEVEL++)
            {
                for (i = 0; i <= N - 1; i++)
                {
                    ICL = i + 1;
                    for (ILEV = 0; ILEV <= LEVEL - 1; ILEV++)
                    {
                        if (IB[ILEV] == ICL)
                            ICL = IA[ILEV];
                    }
                    NCL = N - LEVEL - 1;
                    ICLASS[i, NCL - 1] = ICL;
                }
            }

            int k = 0;
            for (i = 0; i <= N - 1; i++)
            {
                for (j = 0; j <= LEV - 2; j++)
                {
                    for (k = 1; k <= LEV - 1; k++)
                    {
                        if (ICLASS[i, j] != HVALS[k])
                            continue;
                        ICLASS[i, j] = k + 1;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }

        }

        //+++++++++++++++++++++++++++++++++++++++++++++++++C
        //                                                 C
        //  Construct a DENDROGRAM of the top 8 levels of  C
        //  a HIERARCHIC CLUSTERING.                       C
        //                                                 C
        //  Parameters:                                    C
        //                                                 C
        //  IORDER, HEIGHT, CRITVAL: vectors of length LEV C
        //          defining the dendrogram.               C
        //          These are: the ordering of objects     C
        //          along the bottom of the dendrogram     C
        //          (IORDER); the height of the vertical   C
        //          above each object, in ordinal values   C
        //          (HEIGHT); and in real values (CRITVAL).C
        //                                                 C
        //  NOTE: these vectors MUST have been set up with C
        //        LEV = 9 in the prior call to routine     C
        //        HCASS.
        //                                                 C
        //  F. Murtagh, ESA/ESO/STECF, Garching, Feb. 1986.C
        //                                                 C 
        //-------------------------------------------------C
        private static void HCDEN(int LEV, int[] IORDER, int[] HEIGHT, double[] CRITVAL)
        {

        }
    }
}
