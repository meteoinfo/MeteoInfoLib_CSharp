using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MeteoInfoC.Projections;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 2 grid definition section
    /// </summary>
    public class GRIB2GridDefinitionSection
    {
        #region Variables
        /// <summary>
        /// scale factor for Lat/Lon variables in degrees.
        /// </summary>
        private static float tenToNegSix = (float)(1 / 1000000.0);
        /// <summary>
        /// scale factor for dx and dy variables plus others
        /// </summary>
        private static float tenToNegThree = (float)(1 / 1000.0);
        /// <summary>
        /// Section length in bytes
        /// </summary>
        public int Length;
        /// <summary>
        /// Number of section
        /// </summary>
        public int SectionNum;
        /// <summary>
        /// Source of grid definition
        /// </summary>
        public int SourceGridDef;
        /// <summary>
        /// Number of data points
        /// </summary>
        public int NumPoints;
        /// <summary>
        /// olon > 0 is a quasi regular grid.
        /// </summary>
        public int Olon;
        /// <summary>
        /// are extreme points in the quasi regular grid.
        /// </summary>
        public int Iolon;
        /// <summary>
        /// number of points in each parallel for quasi grids.
        /// </summary>
        private int[] olonPts;
        /// <summary>
        /// Max number of points in parallel for quasi grids.
        /// </summary>
        private int maxPts;
        /// <summary>
        /// Grid definition template number
        /// </summary>
        public int TemplateNum;
        /// <summary>
        /// Grid name
        /// </summary>
        public string GridName;
        /// <summary>
        /// Grid definitions from template 3
        /// </summary>
        private int Shape;
        /// <summary>
        /// Earth radius
        /// </summary>
        private float earthRadius;
        /// <summary>
        /// Major axis
        /// </summary>
        private float majorAxis;
        /// <summary>
        /// Minor axis
        /// </summary>
        private float minorAxis;
        /// <summary>
        /// Numbe of grid columns
        /// </summary>
        public int NX;
        /// <summary>
        /// Number of grid rows
        /// </summary>
        public int NY;
        /// <summary>
        /// Angle
        /// </summary>
        private int Angle;
        private int SubdivisionsAngle;
        /// <summary>
        /// First latitude
        /// </summary>
        private float la1;
        /// <summary>
        /// First longitude
        /// </summary>
        private float lo1;
        /// <summary>
        /// resolution
        /// </summary>
        private int resolution;
        /// <summary>
        /// 2nd latitude
        /// </summary>
        private float la2;
        /// <summary>
        /// 2nd longitude
        /// </summary>
        private float lo2;
        /// <summary>
        /// lad
        /// </summary>
        private float lad;
        /// <summary>
        /// lov
        /// </summary>
        private float lov;
        /// <summary>
        /// x-distance between two grid points. can be delta-Lon or delta x.
        /// </summary>
        private float dx;
        /// <summary>
        ///  y-distance of two grid points. can be delta-Lat or delta y.
        /// </summary>
        private float dy;
        /// <summary>
        /// units of the dx and dy variables
        /// </summary>
        private String grid_units;
        /// <summary>
        /// Projection center
        /// </summary>
        private int projectionCenter;
        /// <summary>
        /// Scan mode
        /// </summary>
        public int ScanMode;
        /// <summary>
        /// latin1
        /// </summary>
        private float latin1;
        /// <summary>
        /// latin2
        /// </summary>
        private float latin2;
        /// <summary>
        /// spLat
        /// </summary>
        private float spLat;
        /// <summary>
        /// spLon
        /// </summary>
        private float spLon;
        /// <summary>
        /// Rotatoin angle
        /// </summary>
        private float rotationangle;
        /// <summary>
        /// poleLat
        /// </summary>
        private float poleLat;
        /// <summary>
        /// poleLon
        /// </summary>
        private float poleLon;
        /// <summary>
        /// lon of center
        /// </summary>
        private int lonofcenter;
        /// <summary>
        /// factor
        /// </summary>
        private int factor;
        /// <summary>
        /// n
        /// </summary>
        private int n;
        /// <summary>
        /// j
        /// </summary>
        private float j;
        /// <summary>
        /// k
        /// </summary>
        private float k;
        /// <summary>
        /// m
        /// </summary>
        private float m;
        /// <summary>
        /// method
        /// </summary>
        private int method;
        /// <summary>
        /// mode
        /// </summary>
        private int mode;
        /// <summary>
        /// xp
        /// </summary>
        private float xp;
        /// <summary>
        /// yp
        /// </summary>
        private float yp;
        /// <summary>
        /// lap
        /// </summary>
        private int lap;
        /// <summary>
        /// lop
        /// </summary>
        private int lop;
        /// <summary>
        /// xo
        /// </summary>
        private int xo;
        /// <summary>
        /// yo
        /// </summary>
        private int yo;
        /// <summary>
        /// Altitude
        /// </summary>
        private int altitude;
        /// <summary>
        /// Is orthographic projection
        /// </summary>
        private bool isOrtho = false;
        /// <summary>
        /// n2
        /// </summary>
        private int n2;
        /// <summary>
        /// n3
        /// </summary>
        private int n3;
        /// <summary>
        /// ni
        /// </summary>
        private int ni;
        /// <summary>
        /// nd
        /// </summary>
        private int nd;
        /// <summary>
        /// position
        /// </summary>
        private int position;
        /// <summary>
        /// order
        /// </summary>
        private int order;
        /// <summary>
        /// nb
        /// </summary>
        private float nb;
        /// <summary>
        /// nr
        /// </summary>
        private float nr;
        /// <summary>
        /// dstart
        /// </summary>
        private float dstart;
        ///// <summary>
        ///// check sum
        ///// </summary>
        //private String checksum = "";
        ///// <summary>
        ///// 4.0 indexes use this for the GDS key
        ///// </summary>
        //private int gdskey;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        public GRIB2GridDefinitionSection(BinaryReader br)
        {
            float ratio;
            double checkSum;
            int scalefactorradius = 0;
            int scaledvalueradius = 0;
            int scalefactormajor = 0;
            int scaledvaluemajor = 0;
            int scalefactorminor = 0;
            int scaledvalueminor = 0;

            long sectionEnd = br.BaseStream.Position;

            Length = Bytes2Number.Int4(br);
            sectionEnd += Length;

            SectionNum = br.ReadByte();
            SourceGridDef = br.ReadByte();
            NumPoints = Bytes2Number.Int4(br);
            checkSum = NumPoints;
            Olon = br.ReadByte();
            Iolon = br.ReadByte();
            TemplateNum = Bytes2Number.Int2(br);
            GridName = GetGridName(TemplateNum);            

            switch (TemplateNum)
            {  // Grid Definition Template Number

                case 0:
                case 1:
                case 2:
                case 3:       // Latitude/Longitude Grid
                    Shape = br.ReadByte();
                    //System.out.println( "shape=" + shape );
                    scalefactorradius = br.ReadByte();
                    scaledvalueradius = Bytes2Number.Int4(br);
                    scalefactormajor = br.ReadByte();
                    scaledvaluemajor = Bytes2Number.Int4(br);
                    scalefactorminor = br.ReadByte();
                    scaledvalueminor = Bytes2Number.Int4(br);
                    NX = Bytes2Number.Int4(br);
                    NY = Bytes2Number.Int4(br);
                    Angle = Bytes2Number.Int4(br);
                    SubdivisionsAngle = Bytes2Number.Int4(br);
                    if (Angle == 0)
                    {
                        ratio = tenToNegSix;
                    }
                    else
                    {
                        ratio = Angle / SubdivisionsAngle;
                    }
                    la1 = (float)(Bytes2Number.Int4(br) * ratio);
                    checkSum = 7 * checkSum + la1;
                    lo1 = (float)(Bytes2Number.Int4(br) * ratio);
                    checkSum = 7 * checkSum + lo1;
                    resolution = br.ReadByte();
                    la2 = (float)(Bytes2Number.Int4(br) * ratio);
                    checkSum = 7 * checkSum + la2;
                    lo2 = (float)(Bytes2Number.Int4(br) * ratio);
                    checkSum = 7 * checkSum + lo2;
                    dx = (float)(Bytes2Number.Int4(br) * ratio);
                    //checkSum = 7 * checkSum + dx;
                    dy = (float)(Bytes2Number.Int4(br) * ratio);

                    grid_units = "degrees";

                    //checkSum = 7 * checkSum + dy;
                    ScanMode = br.ReadByte();

                    //  1, 2, and 3 needs checked
                    if (TemplateNum == 1)
                    {         //Rotated Latitude/longitude
                        spLat = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + spLat;
                        spLon = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + spLon;
                        rotationangle = br.ReadSingle();

                    }
                    else if (TemplateNum == 2)
                    {  //Stretched Latitude/longitude
                        poleLat = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + poleLat;
                        poleLon = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + poleLon;
                        factor = Bytes2Number.Int4(br);

                    }
                    else if (TemplateNum == 3)
                    {  //Stretched and Rotated
                        // Latitude/longitude
                        spLat = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + spLat;
                        spLon = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + spLon;
                        rotationangle = br.ReadSingle();
                        poleLat = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + poleLat;
                        poleLon = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + poleLon;
                        factor = Bytes2Number.Int4(br);
                    }
                    break;

                case 10:  // Mercator
                    // la1, lo1, lad, la2, and lo2 need checked
                    Shape = br.ReadByte();
                    //System.out.println( "shape=" + shape );
                    scalefactorradius = br.ReadByte();
                    scaledvalueradius = Bytes2Number.Int4(br);
                    scalefactormajor = br.ReadByte();
                    scaledvaluemajor = Bytes2Number.Int4(br);
                    scalefactorminor = br.ReadByte();
                    scaledvalueminor = Bytes2Number.Int4(br);
                    NX = Bytes2Number.Int4(br);
                    //System.out.println( "nx=" + nx);
                    NY = Bytes2Number.Int4(br);
                    //System.out.println( "ny=" + ny);
                    la1 = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + la1;
                    lo1 = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + lo1;
                    resolution = br.ReadByte();
                    lad = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + lad;
                    la2 = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + la2;
                    lo2 = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + lo2;
                    ScanMode = br.ReadByte();
                    Angle = Bytes2Number.Int4(br);
                    dx = (float)(Bytes2Number.Int4(br) * tenToNegThree);
                    //checkSum = 7 * checkSum + dx;
                    dy = (float)(Bytes2Number.Int4(br) * tenToNegThree);
                    //checkSum = 7 * checkSum + dy;
                    grid_units = "m";

                    break;

                case 20:  // Polar stereographic projection
                    // la1, lo1, lad, and lov need checked
                    Shape = br.ReadByte();
                    //System.out.println( "shape=" + shape );
                    scalefactorradius = br.ReadByte();
                    scaledvalueradius = Bytes2Number.Int4(br);
                    scalefactormajor = br.ReadByte();
                    scaledvaluemajor = Bytes2Number.Int4(br);
                    scalefactorminor = br.ReadByte();
                    scaledvalueminor = Bytes2Number.Int4(br);
                    NX = Bytes2Number.Int4(br);
                    //System.out.println( "nx=" + nx);
                    NY = Bytes2Number.Int4(br);
                    //System.out.println( "ny=" + ny);
                    la1 = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + la1;
                    lo1 = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + lo1;
                    resolution = br.ReadByte();
                    lad = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + lad;
                    lov = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + lov;
                    dx = (float)(Bytes2Number.Int4(br) * tenToNegThree);
                    //checkSum = 7 * checkSum + dx;
                    dy = (float)(Bytes2Number.Int4(br) * tenToNegThree);
                    grid_units = "m";
                    //checkSum = 7 * checkSum + dy;
                    projectionCenter = br.ReadByte();
                    ScanMode = br.ReadByte();

                    break;

                case 30:  // Lambert Conformal
                    Shape = br.ReadByte();
                    //System.out.println( "shape=" + shape );
                    scalefactorradius = br.ReadByte();
                    scaledvalueradius = Bytes2Number.Int4(br);
                    scalefactormajor = br.ReadByte();
                    scaledvaluemajor = Bytes2Number.Int4(br);
                    scalefactorminor = br.ReadByte();
                    scaledvalueminor = Bytes2Number.Int4(br);
                    NX = Bytes2Number.Int4(br);
                    //System.out.println( "nx=" + nx);
                    NY = Bytes2Number.Int4(br);
                    //System.out.println( "ny=" + ny);
                    la1 = (float)(Bytes2Number.Int4(br) * tenToNegSix);
                    checkSum = 7 * checkSum + la1;
                    //System.out.println( "la1=" + la1 );
                    lo1 = (float)(Bytes2Number.Int4(br) * tenToNegSix);
                    checkSum = 7 * checkSum + lo1;
                    //System.out.println( "lo1=" + lo1);
                    resolution = br.ReadByte();
                    lad = (float)(Bytes2Number.Int4(br)
                        * tenToNegSix);
                    checkSum = 7 * checkSum + lad;
                    lov = (float)(Bytes2Number.Int4(br)
                        * tenToNegSix);
                    checkSum = 7 * checkSum + lov;
                    dx = (float)(Bytes2Number.Int4(br) * tenToNegThree);
                    //checkSum = 7 * checkSum + dx;
                    dy = (float)(Bytes2Number.Int4(br) * tenToNegThree);
                    //checkSum = 7 * checkSum + dy;
                    grid_units = "m";

                    projectionCenter = br.ReadByte();
                    ScanMode = br.ReadByte();
                    latin1 = (float)(Bytes2Number.Int4(br)
                        * tenToNegSix);
                    checkSum = 7 * checkSum + latin1;
                    latin2 = (float)(Bytes2Number.Int4(br)
                        * tenToNegSix);
                    checkSum = 7 * checkSum + latin2;
                    //System.out.println( "latin1=" + latin1);
                    //System.out.println( "latin2=" + latin2);
                    spLat = (float)(Bytes2Number.Int4(br) * tenToNegSix);
                    checkSum = 7 * checkSum + spLat;
                    spLon = (float)(Bytes2Number.Int4(br) * tenToNegSix);
                    checkSum = 7 * checkSum + spLon;
                    //System.out.println( "spLat=" + spLat);
                    //System.out.println( "spLon=" + spLon);

                    break;

                case 31:  // Albers Equal Area
                    // la1, lo1, lad, and lov need checked
                    Shape = br.ReadByte();
                    //System.out.println( "shape=" + shape );
                    scalefactorradius = br.ReadByte();
                    scaledvalueradius = Bytes2Number.Int4(br);
                    scalefactormajor = br.ReadByte();
                    scaledvaluemajor = Bytes2Number.Int4(br);
                    scalefactorminor = br.ReadByte();
                    scaledvalueminor = Bytes2Number.Int4(br);
                    NX = Bytes2Number.Int4(br);
                    //System.out.println( "nx=" + nx);
                    NY = Bytes2Number.Int4(br);
                    //System.out.println( "ny=" + ny);
                    la1 = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + la1;
                    //System.out.println( "la1=" + la1 );
                    lo1 = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + lo1;
                    //System.out.println( "lo1=" + lo1);
                    resolution = br.ReadByte();
                    lad = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + lad;
                    lov = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + lov;
                    dx = (float)(Bytes2Number.Int4(br) * tenToNegThree);
                    //checkSum = 7 * checkSum + dx;
                    dy = (float)(Bytes2Number.Int4(br) * tenToNegThree);
                    //checkSum = 7 * checkSum + dy;
                    grid_units = "m";

                    projectionCenter = br.ReadByte();
                    ScanMode = br.ReadByte();
                    latin1 = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + latin1;
                    latin2 = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + latin2;
                    //System.out.println( "latin1=" + latin1);
                    //System.out.println( "latin2=" + latin2);
                    spLat = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + spLat;
                    spLon = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + spLon;
                    //System.out.println( "spLat=" + spLat);
                    //System.out.println( "spLon=" + spLon);

                    break;

                case 40:
                case 41:
                case 42:
                case 43:  // Gaussian latitude/longitude
                    // octet 15
                    Shape = br.ReadByte();
                    //System.out.println( "shape=" + shape );
                    // octet 16
                    scalefactorradius = br.ReadByte();
                    // octets 17-20
                    scaledvalueradius = Bytes2Number.Int4(br);
                    // octet 21
                    scalefactormajor = br.ReadByte();
                    // octets 22-25
                    scaledvaluemajor = Bytes2Number.Int4(br);
                    // octet 26
                    scalefactorminor = br.ReadByte();
                    // octets 27-30
                    scaledvalueminor = Bytes2Number.Int4(br);
                    // octets 31-34
                    NX = Bytes2Number.Int4(br);
                    //System.out.println( "nx=" + nx);
                    // octets 35-38
                    NY = Bytes2Number.Int4(br);
                    //System.out.println( "ny=" + ny);
                    // octets 39-42
                    Angle = Bytes2Number.Int4(br);
                    // octets 43-46
                    SubdivisionsAngle = Bytes2Number.Int4(br);
                    if (Angle == 0)
                    {
                        ratio = tenToNegSix;
                    }
                    else
                    {
                        ratio = Angle / SubdivisionsAngle;
                    }
                    //System.out.println( "ratio =" + ratio );
                    // octets 47-50
                    la1 = (float)(Bytes2Number.Int4(br) * ratio);
                    checkSum = 7 * checkSum + la1;
                    // octets 51-54
                    lo1 = (float)(Bytes2Number.Int4(br) * ratio);
                    checkSum = 7 * checkSum + lo1;
                    // octet 55
                    resolution = br.ReadByte();
                    // octets 56-59
                    la2 = (float)(Bytes2Number.Int4(br) * ratio);
                    checkSum = 7 * checkSum + la2;
                    // octets 60-63
                    lo2 = (float)(Bytes2Number.Int4(br) * ratio);
                    checkSum = 7 * checkSum + lo2;
                    // octets 64-67
                    dx = (float)(Bytes2Number.Int4(br) * ratio);
                    //checkSum = 7 * checkSum + dx;
                    grid_units = "degrees";

                    // octet 68-71
                    n = Bytes2Number.Int4(br);
                    // octet 72
                    ScanMode = br.ReadByte();

                    if (TemplateNum == 41)
                    {  //Rotated Gaussian Latitude/longitude
                        // octets 73-76
                        spLat = Bytes2Number.Int4(br) * ratio;
                        checkSum = 7 * checkSum + spLat;
                        // octets 77-80
                        spLon = Bytes2Number.Int4(br) * ratio;
                        checkSum = 7 * checkSum + spLon;
                        // octets 81-84
                        rotationangle = br.ReadSingle();

                    }
                    else if (TemplateNum == 42)
                    {  //Stretched Gaussian
                        // Latitude/longitude
                        // octets 73-76
                        poleLat = Bytes2Number.Int4(br) * ratio;
                        checkSum = 7 * checkSum + poleLat;
                        // octets 77-80
                        poleLon = Bytes2Number.Int4(br) * ratio;
                        checkSum = 7 * checkSum + poleLon;
                        // octets 81-84
                        factor = Bytes2Number.Int4(br);

                    }
                    else if (TemplateNum == 43)
                    {  //Stretched and Rotated Gaussian
                        // Latitude/longitude
                        // octets 73-76
                        spLat = Bytes2Number.Int4(br) * ratio;
                        checkSum = 7 * checkSum + spLat;
                        // octets 77-80
                        spLon = Bytes2Number.Int4(br) * ratio;
                        checkSum = 7 * checkSum + spLon;
                        // octets 81-84
                        rotationangle = br.ReadSingle();
                        // octets 85-88
                        poleLat = Bytes2Number.Int4(br) * ratio;
                        checkSum = 7 * checkSum + poleLat;
                        // octets 89-92
                        poleLon = Bytes2Number.Int4(br) * ratio;
                        checkSum = 7 * checkSum + poleLon;
                        // octets 93-96
                        factor = Bytes2Number.Int4(br);
                    }
                    break;

                case 50:
                case 51:
                case 52:
                case 53:                     // Spherical harmonic coefficients

                    j = br.ReadSingle();
                    k = br.ReadSingle();
                    m = br.ReadSingle();
                    method = br.ReadByte();
                    mode = br.ReadByte();
                    grid_units = "";
                    if (TemplateNum == 51)
                    {         //Rotated Spherical harmonic coefficients

                        spLat = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + spLat;
                        spLon = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + spLon;
                        rotationangle = br.ReadSingle();

                    }
                    else if (TemplateNum == 52)
                    {  //Stretched Spherical
                        // harmonic coefficients

                        poleLat = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + poleLat;
                        poleLon = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + poleLon;
                        factor = Bytes2Number.Int4(br);

                    }
                    else if (TemplateNum == 53)
                    {  //Stretched and Rotated
                        // Spherical harmonic coefficients

                        spLat = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + spLat;
                        spLon = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + spLon;
                        rotationangle = br.ReadSingle();
                        poleLat = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + poleLat;
                        poleLon = Bytes2Number.Int4(br) * tenToNegSix;
                        checkSum = 7 * checkSum + poleLon;
                        factor = Bytes2Number.Int4(br);
                    }
                    break;

                case 90:  // Space view perspective or orthographic
                    Shape = br.ReadByte();
                    //System.out.println( "shape=" + shape );
                    scalefactorradius = br.ReadByte();
                    scaledvalueradius = Bytes2Number.Int4(br);
                    scalefactormajor = br.ReadByte();
                    scaledvaluemajor = Bytes2Number.Int4(br);
                    scalefactorminor = br.ReadByte();
                    scaledvalueminor = Bytes2Number.Int4(br);
                    NX = Bytes2Number.Int4(br);
                    //System.out.println( "nx=" + nx);
                    NY = Bytes2Number.Int4(br);
                    //System.out.println( "ny=" + ny);
                    lap = Bytes2Number.Int4(br);
                    checkSum = 7 * checkSum + lap;
                    lop = Bytes2Number.Int4(br);
                    checkSum = 7 * checkSum + lop;
                    resolution = br.ReadByte();
                    dx = Bytes2Number.Int4(br);
                    //checkSum = 7 * checkSum + dx;
                    dy = Bytes2Number.Int4(br);
                    //checkSum = 7 * checkSum + dy;
                    grid_units = "";
                    xp = (float)(Bytes2Number.Int4(br) * tenToNegThree);
                    checkSum = 7 * checkSum + xp;
                    yp = (float)(Bytes2Number.Int4(br) * tenToNegThree);
                    checkSum = 7 * checkSum + yp;
                    ScanMode = br.ReadByte();
                    Angle = Bytes2Number.Int4(br);
                    //altitude = Bytes2Number.int4( raf ) * 1000000;
                    byte[] bytes = br.ReadBytes(4);
                    altitude = Bytes2Number.Int4(bytes[0], bytes[1], bytes[2], bytes[3]);
                    altitude = (int)(altitude * 6.378169 - 6378169);
                    if ((bytes[0] & 256) == 256 && (bytes[1] & 256) == 256 && (bytes[2] & 256) == 256 && (bytes[3] & 256) == 256)
                        isOrtho = true;
                    //altitude = Bytes2Number.Int4(br);
                    checkSum = 7 * checkSum + altitude;
                    xo = Bytes2Number.Int4(br);
                    checkSum = 7 * checkSum + xo;
                    yo = Bytes2Number.Int4(br);
                    checkSum = 7 * checkSum + yo;

                    break;

                case 100:  // Triangular grid based on an icosahedron

                    n2 = br.ReadByte();
                    checkSum = 7 * checkSum + n2;
                    n3 = br.ReadByte();
                    checkSum = 7 * checkSum + n3;
                    ni = Bytes2Number.Int2(br);
                    checkSum = 7 * checkSum + ni;
                    nd = br.ReadByte();
                    checkSum = 7 * checkSum + nd;
                    poleLat = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + poleLat;
                    poleLon = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + poleLon;
                    lonofcenter = Bytes2Number.Int4(br);
                    position = br.ReadByte();
                    order = br.ReadByte();
                    ScanMode = br.ReadByte();
                    n = Bytes2Number.Int4(br);
                    grid_units = "";
                    break;

                case 110:  // Equatorial azimuthal equidistant projection
                    Shape = br.ReadByte();
                    scalefactorradius = br.ReadByte();
                    scaledvalueradius = Bytes2Number.Int4(br);
                    scalefactormajor = br.ReadByte();
                    scaledvaluemajor = Bytes2Number.Int4(br);
                    scalefactorminor = br.ReadByte();
                    scaledvalueminor = Bytes2Number.Int4(br);
                    NX = Bytes2Number.Int4(br);
                    NY = Bytes2Number.Int4(br);
                    la1 = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + la1;
                    lo1 = Bytes2Number.Int4(br) * tenToNegSix;
                    checkSum = 7 * checkSum + lo1;
                    resolution = br.ReadByte();
                    dx = (float)(Bytes2Number.Int4(br) * tenToNegThree);
                    dy = (float)(Bytes2Number.Int4(br) * tenToNegThree);
                    grid_units = "";
                    projectionCenter = br.ReadByte();
                    ScanMode = br.ReadByte();

                    break;

                case 120:  // Azimuth-range Projection
                    nb = Bytes2Number.Int4(br);
                    checkSum = 7 * checkSum + nb;
                    nr = Bytes2Number.Int4(br);
                    checkSum = 7 * checkSum + nr;
                    la1 = Bytes2Number.Int4(br);
                    checkSum = 7 * checkSum + la1;
                    lo1 = Bytes2Number.Int4(br);
                    checkSum = 7 * checkSum + lo1;
                    dx = Bytes2Number.Int4(br);
                    //checkSum = 7 * checkSum + dx;
                    grid_units = "";
                    dstart = br.ReadSingle();
                    ScanMode = br.ReadByte();
                    for (int i = 0; i < nr; i++)
                    {
                        // get azi (33+4(Nr-1))-(34+4(Nr-1))
                        // get adelta (35+4(Nr-1))-(36+4(Nr-1))
                    }

                    break;

                case 204:  // Curvilinear orthographic
                    Shape = br.ReadByte();
                    //System.out.println( "shape=" + shape );
                    scalefactorradius = br.ReadByte();
                    scaledvalueradius = Bytes2Number.Int4(br);
                    scalefactormajor = br.ReadByte();
                    scaledvaluemajor = Bytes2Number.Int4(br);
                    scalefactorminor = br.ReadByte();
                    scaledvalueminor = Bytes2Number.Int4(br);
                    NX = Bytes2Number.Int4(br);
                    NY = Bytes2Number.Int4(br);
                    // octets 39 - 54 not used, set to 0        
                    br.ReadBytes(16);
                    resolution = br.ReadByte();
                    // octets 56 - 71 not used        
                    br.ReadBytes(16);
                    ScanMode = br.ReadByte();
                    grid_units = "";
                    break;
                default:
                    break;

            }  // end switch

            // calculate earth radius
            if (((TemplateNum < 50) || (TemplateNum > 53)) && (TemplateNum != 100) && (TemplateNum != 120))
            {
                if (Shape == 0)
                {
                    earthRadius = 6367470;
                }
                else if (Shape == 1)
                {
                    earthRadius = scaledvalueradius;
                    if (scalefactorradius != 0)
                    {
                        earthRadius /= (float)Math.Pow(10, scalefactorradius);
                    }
                }
                else if (Shape == 2)
                {
                    majorAxis = (float)6378160.0;
                    minorAxis = (float)6356775.0;
                }
                else if (Shape == 3)
                {
                    majorAxis = scaledvaluemajor;
                    //System.out.println( "majorAxisScale =" + scalefactormajor );
                    //System.out.println( "majorAxisiValue =" + scaledvaluemajor );
                    majorAxis /= (float)Math.Pow(10, scalefactormajor);

                    minorAxis = scaledvalueminor;
                    //System.out.println( "minorAxisScale =" + scalefactorminor );
                    //System.out.println( "minorAxisValue =" + scaledvalueminor );
                    minorAxis /= (float)Math.Pow(10, scalefactorminor);
                }
                else if (Shape == 4)
                {
                    majorAxis = (float)6378137.0;
                    minorAxis = (float)6356752.314;
                }
                else if (Shape == 6)
                {
                    earthRadius = 6371229;
                }
            }
            // This is a quasi-regular grid, save the number of pts in each parallel
            if (Olon != 0)
            {
                //System.out.println( "olon ="+ olon +" iolon ="+ iolon );
                int numPts;
                if ((ScanMode & 32) == 0)
                {
                    numPts = NY;
                }
                else
                {
                    numPts = NX;
                }
                olonPts = new int[numPts];
                //int count = 0;
                maxPts = 0;
                if (Olon == 1)
                {
                    for (int i = 0; i < numPts; i++)
                    {
                        olonPts[i] = br.ReadByte();
                        if (maxPts < olonPts[i])
                        {
                            maxPts = olonPts[i];
                        }
                        //count += olonPts[ i ];
                        //System.out.println( "parallel =" + i +" number pts ="+ latPts );
                    }
                }
                else if (Olon == 2)
                {
                    for (int i = 0; i < numPts; i++)
                    {
                        olonPts[i] = br.ReadUInt16();
                        if (maxPts < olonPts[i])
                        {
                            maxPts = olonPts[i];
                        }
                        //count += olonPts[ i ];
                        //System.out.println( "parallel =" + i +" number pts ="+ latPts );
                    }
                }
                if ((ScanMode & 32) == 0)
                {
                    NX = maxPts;
                }
                else
                {
                    NY = maxPts;
                }
                //double lodiff = gds.getLo2() - gds.getLo1();
                dx = (float)(lo2 - lo1) / (NX - 0);
                //System.out.println( "total number pts ="+ count );
            }

            //gdskey = Double.ToString(checkSum).HashCode();
            //checksum = Integer.toString(gdskey);

            br.BaseStream.Position = sectionEnd;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get projection info
        /// </summary>
        /// <returns>projection info</returns>
        public ProjectionInfo GetProjectionInfo()
        {
            ProjectionInfo aProjInfo = new ProjectionInfo();
            string projStr = "+proj=longlat +ellps=WGS84 +datum=WGS84 +no_defs";
            switch (TemplateNum)
            {  // Grid Definition Template Number

                case 0:
                case 1:
                case 2:
                case 3:       // Latitude/Longitude Grid
                    projStr = KnownCoordinateSystems.Geographic.World.WGS1984.ToProj4String();
                    break;
                case 10:  // Mercator
                    if (lad == 0)
                        projStr = "+proj=merc+lon_0=" + GRIBData.GetMeanLongitude(lo1, lo2).ToString();
                    else    //Transverse_Mercator
                        projStr = "+proj=tmerc+lon_0=" + GRIBData.GetMeanLongitude(lo1, lo2).ToString() +
                            "+lat_0=" + lad.ToString();
                    break;
                case 20:  // Polar stereographic projection
                    double lat0 = 90;
                    if ((projectionCenter & 128) != 0)
                        lat0 = -90;
                    projStr = "+proj=stere+lon_0=" + lov.ToString() +
                        "+lat_0=" + lat0.ToString();
                    break;
                case 30:  // Lambert Conformal
                    projStr = "+proj=lcc+lon_0=" + lov.ToString() +
                        "+lat_0=" + lad.ToString() +
                        "+lat_1=" + latin1.ToString() +
                        "+lat_2=" + latin2.ToString();
                    break;
                case 31:  // Albers Equal Area
                    

                    break;
                case 40:
                case 41:
                case 42:
                case 43:  // Gaussian latitude/longitude
                    projStr = KnownCoordinateSystems.Geographic.World.WGS1984.ToProj4String();
                    break;
                case 50:
                case 51:
                case 52:
                case 53:                     // Spherical harmonic coefficients

                    
                    break;
                case 90:  // Space view perspective or orthographic
                    projStr = "+proj=ortho+lon_0=" + lop.ToString() +
                             "+lat_0=" + lap.ToString();
                    
                    //if (isOrtho)
                    //    projStr = "+proj=ortho+lon_0=" + lop.ToString() +
                    //        "+lat_0=" + lap.ToString();
                    //else
                    //    projStr = "+proj=geos+lon_0=" + lop.ToString() +
                    //        "+h=" + altitude.ToString();
                    break;
                case 100:  // Triangular grid based on an icosahedron

                   
                    break;
                case 110:  // Equatorial azimuthal equidistant projection
                    

                    break;
                case 120:  // Azimuth-range Projection
                    

                    break;
                case 204:  // Curvilinear orthographic
                    
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
        public double[] GetXArray()
        {
            double[] X = new double[NX];
            double ddx = Math.Abs(dx);
            double ddx1 = (lo2 - lo1) / (NX - 1);
            if (!Global.MIMath.DoubleEquals(ddx, ddx1) && Math.Abs(ddx - ddx1) < 0.001)
                ddx = ddx1;

            for (int i = 0; i < NX; i++)
            {
                X[i] = lo1 + ddx * i;
            }

            return X;
        }

        /// <summary>
        /// Get Y coordinate array
        /// </summary>
        /// <returns>Y array</returns>
        public double[] GetYArray()
        {
            double[] Y = new double[NY];
            double ddy = Math.Abs(dy);
            double sLat = Math.Min(la1, la2); 
            for (int i = 0; i < NY; i++)
            {
                Y[i] = sLat + ddy * i;
            }

            return Y;
        }

        /// <summary>
        /// Get Y coordinate array of Gaussian grid
        /// </summary>
        /// <returns>Y coordinate array</returns>
        public double[] GetGaussYArray()
        {
            double[] Y = new double[NY];
            Y = (double[])DataMath.Gauss2Lats(NY)[0];

            //double ymin = Y[0];
            //double ymax = Y[Y.Length - 1];
            //double delta = (ymax - ymin) / (NY - 1);
            //for (int i = 0; i < NY; i++)
            //    Y[i] = ymin + i * delta;

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
            switch (aProjInfo.Transform.Proj4Name)
            {
                case "lonlat":
                    switch (TemplateNum)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            X = GetXArray();
                            Y = GetYArray();
                            break;
                        case 40:
                        case 41:
                        case 42:
                        case 43:  // Gaussian latitude/longitude
                            X = GetXArray();
                            Y = GetGaussYArray();
                            break;
                    }                    
                    break;
                case "ortho":
                case "geos":
                    //Get under satellite point X/Y
                    ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
                    double s_X, s_Y;
                    double[][] points = new double[1][];
                    points[0] = new double[] { lo1, la1 };
                    Reproject.ReprojectPoints(points, fromProj, aProjInfo, 0, 1);
                    s_X = points[0][0];
                    s_Y = points[0][1];

                    //Get integer sync X/Y            
                    int i_XP, i_YP;
                    double i_X, i_Y;
                    i_XP = (int)xp;
                    if (xp == i_XP)
                    {
                        i_X = s_X;
                    }
                    else
                    {
                        i_X = s_X - (xp - i_XP) * dx;
                    }
                    i_YP = (int)yp;
                    if (yp == i_YP)
                    {
                        i_Y = s_Y;
                    }
                    else
                    {
                        i_Y = s_Y - (yp - i_YP) * dy;
                    }

                    //Get left bottom X/Y
                    int nx, ny;
                    nx = X.Length;
                    ny = Y.Length;
                    double xlb, ylb;
                    xlb = i_X - (i_XP - 1) * dx;
                    ylb = i_Y - (i_YP - 1) * dy;

                    //Get X Y with orient 0
                    int i;
                    X = new double[NX];
                    Y = new double[NY];
                    for (i = 0; i < NX; i++)
                    {
                        X[i] = xlb + i * dx;
                    }
                    for (i = 0; i < NY; i++)
                    {
                        Y[i] = ylb + i * dy;
                    }
                    break;
                default:
                    //Get start X/Y
                    fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;                    
                    points = new double[1][];
                    points[0] = new double[] { lo1, la1 };
                    Reproject.ReprojectPoints(points, fromProj, aProjInfo, 0, 1);
                    s_X = points[0][0];
                    s_Y = points[0][1];

                    //Get X/Y
                    X = new double[NX];
                    Y = new double[NY];                    
                    for (i = 0; i < NX; i++)
                        X[i] = s_X + dx * i;

                    for (i = 0; i < NY; i++)
                        Y[i] = s_Y + dy * i;
                    break;
            }            
        }

        /// <summary>
        /// Get grid name
        /// </summary>
        /// <param name="gdtn">grid difinition template number</param>
        /// <returns>grid name</returns>
        public static string GetGridName(int gdtn)
        {
            switch (gdtn)
            {  // code table 3.2

                case 0:
                    return "Latitude/Longitude";

                case 1:
                    return "Rotated Latitude/Longitude";

                case 2:
                    return "Stretched Latitude/Longitude";

                case 3:
                    return "iStretched and Rotated Latitude/Longitude";

                case 10:
                    return "Mercator";

                case 20:
                    return "Polar stereographic";

                case 30:
                    return "Lambert Conformal";

                case 31:
                    return "Albers Equal Area";

                case 40:
                    return "Gaussian latitude/longitude";

                case 41:
                    return "Rotated Gaussian Latitude/longitude";

                case 42:
                    return "Stretched Gaussian Latitude/longitude";

                case 43:
                    return "Stretched and Rotated Gaussian Latitude/longitude";

                case 50:
                    return "Spherical harmonic coefficients";

                case 51:
                    return "Rotated Spherical harmonic coefficients";

                case 52:
                    return "Stretched Spherical harmonic coefficients";

                case 53:
                    return "Stretched and Rotated Spherical harmonic coefficients";

                case 90:
                    return "Space View Perspective or Orthographic";

                case 100:
                    return "Triangular Grid Based on an Icosahedron";

                case 110:
                    return "Equatorial Azimuthal Equidistant";

                case 120:
                    return "Azimuth-Range";

                case 204:
                    return "Curvilinear Orthogonal Grid";

                default:
                    return "Unknown projection" + gdtn;
            }
        }

        /// <summary>
        /// Get shape name
        /// </summary>
        /// <param name="shape">shape integer</param>
        /// <returns>shape name</returns>
        public static string GetShapeName(int shape)
        {
            switch (shape)
            {  // code table 3.2

                case 0:
                    return "Earth spherical with radius = 6367470 m";

                case 1:
                    return "Earth spherical with radius specified by producer";

                case 2:
                    return "Earth oblate spheroid with major axis = 6378160.0 m and minor axis = 6356775.0 m";

                case 3:
                    return "Earth oblate spheroid with axes specified by producer";

                case 4:
                    return "Earth oblate spheroid with major axis = 6378137.0 m and minor axis = 6356752.314 m";

                case 5:
                    return "Earth represent by WGS84";

                case 6:
                    return "Earth spherical with radius of 6371229.0 m";

                default:
                    return "Unknown Earth Shape";
            }
        }

        #endregion
    }
}
