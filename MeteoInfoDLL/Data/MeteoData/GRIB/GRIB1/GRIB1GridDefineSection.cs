using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Projections;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 1 grid define section
    /// </summary>
    public class GRIB1GridDefineSection
    {
        #region Variables
        /// <summary>
        /// Length in octets of the Grid Description Section
        /// </summary>
        public int Length;
        /// <summary>
        /// The number of vertical coordinate parameters
        /// </summary>
        public int NV;
        /// <summary>
        /// PV, the location (octet number) of the list of vertical coordinate parameters, if present
        /// or
        /// PL, the location (octet number) of the list of
        /// numbers of points in each row (when no vertical
        /// parameters are present), if present
        /// or
        /// 255 (all bits set to 1) if neither are present
        /// </summary>
        public int P_VorL;
        /// <summary>
        /// Date representation type
        /// </summary>
        public int GridType;
        /// <summary>
        /// Number of grid columns
        /// </summary>
        public int NX;
        /// <summary>
        /// If x reverse
        /// </summary>
        public bool XReverse = false;
        /// <summary>
        /// If y reverse
        /// </summary>
        public bool YReverse = false;
        /// <summary>
        /// Number of grid rows
        /// </summary>
        public int NY;
        /// <summary>
        /// Latitude of grid start point
        /// </summary>
        public double Lat1;
        /// <summary>
        /// Longitude of grid start point
        /// </summary>
        public double Lon1;
        /// <summary>
        /// Resolution of grid
        /// </summary>
        public int Resolution;
        /// <summary>
        /// Latitude of grid last point
        /// </summary>
        public double Lat2;
        /// <summary>
        /// Longitude of grid last point
        /// </summary>
        public double Lon2;
        /// <summary>
        /// X-distance of two grid points
        /// </summary>
        public double DX;
        /// <summary>
        /// Y-distance of two grid points
        /// </summary>
        public double DY;
        /// <summary>
        /// Number of parallels between a pole and the equator.
        /// </summary>
        public int NP;
        /// <summary>
        /// Orientation of the grid
        /// </summary>
        public double Lov;
        /// <summary>
        /// Projection center flag
        /// </summary>
        public int Proj_Center;
        /// <summary>
        /// The first latitude from pole at which secant cone cuts the sperical earth
        /// </summary>
        public double Latin1;
        /// <summary>
        /// The second latitude from pole at which secant cone cuts the sperical earth
        /// </summary>
        public double Latin2;
        /// <summary>
        /// Scanning mode
        /// </summary>
        public int ScanMode;
        /// <summary>
        /// Latitude of south pole
        /// </summary>
        public double Latsp;
        /// <summary>
        /// Longitude of south pole
        /// </summary>
        public double Lonsp;
        /// <summary>
        /// Angle of rotation
        /// </summary>
        public double Rotang;
        /// <summary>
        /// If is thinned grid
        /// </summary>
        public bool ThinnedGrid = false;
        /// <summary>
        /// X number on each latitude of thinned grid
        /// </summary>
        public int[] ThinnedXNums = null;
        /// <summary>
        /// Total thinned grid data number
        /// </summary>
        public int ThinnedGridNum;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        public GRIB1GridDefineSection(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(3);
            Length = Bytes2Number.Uint3(bytes[0], bytes[1], bytes[2]);

            br.BaseStream.Seek(-3, SeekOrigin.Current);
            bytes = br.ReadBytes(Length);

            NV = Convert.ToInt32(bytes[3]);
            P_VorL = Convert.ToInt32(bytes[4]);
            GridType = Convert.ToInt32(bytes[5]);
            double checkSum = GridType;

            if (GridType != 50) //Values same up to resolution
            {
                NX = Bytes2Number.Uint2(bytes[6], bytes[7]);
                if (NX >= 65535)
                    ThinnedGrid = true;

                checkSum = 7 * checkSum + NX;
                NY = Bytes2Number.Uint2(bytes[8], bytes[9]);
                checkSum = 7 * checkSum + NY;
                Lat1 = Bytes2Number.Int3(bytes[10], bytes[11], bytes[12]) / 1000.0;
                checkSum = 7 * checkSum + Lat1;
                Lon1 = Bytes2Number.Int3(bytes[13], bytes[14], bytes[15]) / 1000.0;
                checkSum = 7 * checkSum + Lon1;
                Resolution = Convert.ToInt32(bytes[16]);
            }

            switch (GridType)
            {
                case 0:    // Standard Lat/Lon grid, no rotation
                case 4:    //Gaussian Lat/Lon grid
                case 10:    // Rotated Lat/Lon grid   
                case 201:    //Arkawa semi-staggered E-grid on rotated lat/lon grid
                case 202:    //Arakawa filled E -grid on rotated lat/lon grid                               
                    Lat2 = Bytes2Number.Int3(bytes[17], bytes[18], bytes[19]) / 1000.0;
                    checkSum = 7 * checkSum + Lat2;
                    Lon2 = Bytes2Number.Int3(bytes[20], bytes[21], bytes[22]) / 1000.0;
                    checkSum = 7 * checkSum + Lon2;

                    //Increments given
                    if (Resolution == 128)
                    {
                        DX = Bytes2Number.Uint2(bytes[23], bytes[24]) / 1000.0;
                        //if (ThinnedGrid)
                        //{
                        //    NX = 73;
                        //    DX = 1.25;
                        //}

                        if (GridType == 4)
                        {
                            NP = Bytes2Number.Uint2(bytes[25], bytes[26]);
                            DY = NP / 1000.0;    //try for error data
                        }
                        else
                            DY = Bytes2Number.Uint2(bytes[25], bytes[26]) / 1000.0;

                        ScanMode = Convert.ToInt32(bytes[27]);
                        if ((ScanMode & 128) != 0)
                            DX = -DX;
                        //if ((ScanMode & 64) != 64 && Lat2 < Lat1)
                        //    DY = -DY;
                        //if ((ScanMode & 64) != 64 || Lat2 < Lat1)
                        //    DY = -DY;
                        if (Lat2 < Lat1)
                            DY = -DY;
                    }
                    else
                    {
                        //Calculate increments
                        DX = (Lon2 - Lon1) / (NX - 1);
                        DY = (Lat2 - Lat1) / (NY - 1);
                    }
                    if (DX < 0)
                        XReverse = true;
                    if (DY < 0)
                        YReverse = true;

                    if (ThinnedGrid)
                    {
                        ThinnedXNums = new int[NY];
                        ThinnedGridNum = 0;                        
                        for (int i = 0; i < NY; i++)
                        {
                            ThinnedXNums[i] = Bytes2Number.Int2(bytes[32 + i * 2], bytes[33 + i * 2]);
                            ThinnedGridNum += ThinnedXNums[i];
                            if (i == 0)
                                NX = ThinnedXNums[i];
                            else
                            {
                                if (NX < ThinnedXNums[i])
                                    NX = ThinnedXNums[i];
                            }
                        }
                        DX = Math.Abs(Lon2 - Lon1) / (NX - 1);
                    }

                    if (GridType == 10)
                    {
                        // Rotated Lat/Lon grid, Lat (octets 33-35), Lon (octets 36-38), rotang (octets 39-42)                                        
                        Latsp = Bytes2Number.Int3(bytes[32], bytes[33], bytes[34]) / 1000.0;
                        Lonsp = Bytes2Number.Int3(bytes[35], bytes[36], bytes[37]) / 1000.0;
                        Rotang = Bytes2Number.Int4(bytes[38], bytes[39], bytes[40], bytes[41]) / 1000.0;
                    }
                    break;
                case 1:    //Mercator projection
                    Lat2 = Bytes2Number.Int3(bytes[17], bytes[18], bytes[19]) / 1000.0;
                    checkSum = 7 * checkSum + Lat2;
                    Lon2 = Bytes2Number.Int3(bytes[20], bytes[21], bytes[22]) / 1000.0;
                    checkSum = 7 * checkSum + Lon2;
                    Latin1 = Bytes2Number.Int3(bytes[23], bytes[24], bytes[25]) / 1000.0;
                    checkSum = 7 * checkSum + Latin1;
                    ScanMode = bytes[27];
                    DX = Bytes2Number.Int3(bytes[28], bytes[29], bytes[30]);
                    DY = Bytes2Number.Int3(bytes[31], bytes[32], bytes[33]);
                    break;
                case 3:    //Lambert projection                    
                    Lov = Bytes2Number.Int3(bytes[17], bytes[18], bytes[19]) / 1000.0;
                    checkSum = 7 * checkSum + Lov;
                    DX = Bytes2Number.Int3(bytes[20], bytes[21], bytes[22]);
                    DY = Bytes2Number.Int3(bytes[23], bytes[24], bytes[25]);
                    Proj_Center = bytes[26];
                    ScanMode = bytes[27];
                    Latin1 = Bytes2Number.Int3(bytes[28], bytes[29], bytes[30]) / 1000.0;
                    checkSum = 7 * checkSum + Latin1;
                    Latin2 = Bytes2Number.Int3(bytes[31], bytes[32], bytes[33]) / 1000.0;
                    checkSum = 7 * checkSum + Latin2;
                    Latsp = Bytes2Number.Int3(bytes[34], bytes[35], bytes[36]) / 1000.0;
                    checkSum = 7 * checkSum + Latsp;
                    Lonsp = Bytes2Number.Int3(bytes[37], bytes[38], bytes[39]) / 1000.0;
                    checkSum = 7 * checkSum + Lonsp;
                    break;
                case 5:    //Polar stereographic projection
                case 87:
                    Lov = Bytes2Number.Int3(bytes[17], bytes[18], bytes[19]) / 1000.0;
                    checkSum = 7 * checkSum + Lov;

                    if (GridType == 87)
                    {
                        Lon2 = Bytes2Number.Int3(bytes[20], bytes[21], bytes[22]) / 1000.0;
                        checkSum = 7 * checkSum + Lon2;
                    }

                    DX = Bytes2Number.Int3(bytes[20], bytes[21], bytes[22]);
                    DY = Bytes2Number.Int3(bytes[23], bytes[24], bytes[25]);
                    Proj_Center = bytes[26];
                    ScanMode = bytes[27];
                    break;
                default:

                    break;
            }
                                 

            // This switch uses the grid_type to define how to handle the
            // southpole information.              
            switch (GridType)
            {
                case 0:
                    // Standard Lat/Lon grid, no rotation
                    Latsp = -90.0;
                    Lonsp = 0.0;
                    Rotang = 0.0;
                    break;                
                default:
                    // No knowledge yet
                    // NEED to fix this later, if supporting other grid types
                    Latsp = Double.NaN;
                    Lonsp = Double.NaN;
                    Rotang = Double.NaN;
                    break;
            }            
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get grid name
        /// </summary>
        /// <param name="type">grid type</param>
        /// <returns>grid name</returns>
        public static string GetGridName(int type)
        {
            switch (type)
            {

                case 0:
                    return "Latitude/Longitude Grid";

                case 1:
                    return "Mercator Projection Grid";

                case 2:
                    return "Gnomonic Projection Grid";

                case 3:
                    return "Lambert Conformal";

                case 4:
                    return "Gaussian Latitude/Longitude";

                case 5:
                case 87:
                    return "Polar Stereographic projection Grid";

                case 6:
                    return "Universal Transverse Mercator";

                case 7:
                    return "Simple polyconic projection";

                case 8:
                    return "Albers equal-area, secant or tangent, conic or bi-polar, projection";

                case 9:
                    return "Miller's cylindrical projection";

                case 10:
                    return "Rotated latitude/longitude grid";

                case 13:
                    return "Oblique Lambert conformal, secant or tangent, conical or bipolar, projection";

                case 14:
                    return "Rotated Gaussian latitude/longitude grid";

                case 20:
                    return "Stretched latitude/longitude grid";

                case 24:
                    return "Stretched Gaussian latitude/longitude grid";

                case 30:
                    return "Stretched and rotated latitude/longitude grids";

                case 34:
                    return "Stretched and rotated Gaussian latitude/longitude grids";

                case 50:
                    return "Spherical Harmonic Coefficients";

                case 60:
                    return "Rotated spherical harmonic coefficients";

                case 70:
                    return "Stretched spherical harmonics";

                case 80:
                    return "Stretched and rotated spherical harmonic coefficients";

                case 90:
                    return "Space view perspective or orthographic";

                case 201:
                    return "Arakawa semi-staggered E-grid on rotated latitude/longitude grid-point array";

                case 202:
                    return "Arakawa filled E-grid on rotated latitude/longitude grid-point array";
            }

            return "Unknown";

        }

        /// <summary>
        /// Get projection info
        /// </summary>
        /// <returns>projection info</returns>
        public ProjectionInfo GetProjectionInfo()
        {
            ProjectionInfo aProjInfo = new ProjectionInfo();
            string projStr = "+proj=longlat +ellps=WGS84 +datum=WGS84 +no_defs";
            switch (GridType)
            {
                case 0:    // Standard Lat/Lon grid, no rotation
                case 4:    //Gaussian Lat/Lon grid
                case 10:    // Rotated Lat/Lon grid   
                case 201:    //Arkawa semi-staggered E-grid on rotated lat/lon grid
                case 202:    //Arakawa filled E -grid on rotated lat/lon grid
                    projStr = KnownCoordinateSystems.Geographic.World.WGS1984.ToProj4String();
                    break;
                case 1:    //Mercator projection
                    if (Latin1 == 0)
                        projStr = "+proj=merc+lon_0=" + GRIBData.GetMeanLongitude(Lon1, Lon2).ToString();
                    else    //Transverse_Mercator
                        projStr = "+proj=tmerc+lon_0=" + GRIBData.GetMeanLongitude(Lon1, Lon2).ToString() +
                            "+lat_0=" + Latin1.ToString();
                    break;
                case 3:    //Lambert projection                    
                    projStr = "+proj=lcc+lon_0=" + Lov.ToString() +
                        "+lat_1=" + Latin1.ToString() +
                        "+lat_2=" + Latin2.ToString();
                    break;
                case 5:    //Polar stereographic projection
                case 87:
                    double lat0 = 90;
                    if ((Proj_Center & 128) != 0)
                        lat0 = -90;
                    projStr = "+proj=stere+lon_0=" + Lov.ToString() +
                        "+lat_0=" + lat0.ToString();
                    break;
                default:
                    return null;                   
            }

            aProjInfo = new ProjectionInfo(projStr);
            return aProjInfo;
        }

        /// <summary>
        /// Get X coordinate array
        /// </summary>
        /// <returns>X array</returns>
        private double[] GetXArray()
        {
            double[] X = new double[NX];
            double dx = Math.Abs(DX);
            for (int i = 0; i < NX; i++)
            {
                X[i] = Lon1 + dx * i;
            }

            return X;
        }

        /// <summary>
        /// Get Y coordinate array
        /// </summary>
        /// <returns>Y array</returns>
        private double[] GetYArray()
        {
            double[] Y = new double[NY];

            switch (GridType)
            {
                case 4:    //Gaussian Lat/Lon grid
                    Y = (double[])DataMath.Gauss2Lats(NY)[0];                    
                    break;
                //default:
                //    double dy = Math.Abs(DY);
                //    //double sLat = Math.Min(Lat1, Lat2);
                //    double sLat = Lat1;

                //    for (int i = 0; i < NY; i++)
                //    {
                //        Y[i] = sLat + dy * i;
                //    }
                //    break;
                default:
                    for (int i = 0; i < NY; i++)
                    {
                        Y[i] = Lat1 + DY * i;
                    }
                    if (DY < 0)
                        Array.Reverse(Y);
                    break;
            }

            return Y;
        }

        /// <summary>
        /// Get X/Y coordinate array
        /// </summary>
        /// <param name="X">ref X array</param>
        /// <param name="Y">ref Y array</param>
        public void GetXYArray(ref double[] X, ref double[] Y)
        {
            ProjectionInfo aProjInfo = GetProjectionInfo();
            if (aProjInfo.Transform.Proj4Name == "lonlat")
            {
                X = GetXArray();
                Y = GetYArray();
            }
            else
            {
                //Get start X/Y
                ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
                double s_X, s_Y;
                double[][] points = new double[1][];
                points[0] = new double[] { Lon1, Lat1 };
                Reproject.ReprojectPoints(points, fromProj, aProjInfo, 0, 1);
                s_X = points[0][0];
                s_Y = points[0][1];

                //Get X/Y
                X = new double[NX];
                Y = new double[NY];
                int i;
                for (i = 0; i < NX; i++)
                    X[i] = s_X + DX * i;

                for (i = 0; i < NY; i++)
                    Y[i] = s_Y + DY * i;

            }
        }

        #endregion
    }
}
