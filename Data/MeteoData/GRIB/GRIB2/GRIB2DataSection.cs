using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ucar.jpeg.jj2000.j2k.decoder;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 2 data section
    /// </summary>
    public class GRIB2DataSection
    {
        #region Variables
        /// <summary>
        /// Section length in bytes
        /// </summary>
        public int Length;
        /// <summary>
        /// Number of section
        /// </summary>
        public int SectionNum;
        /// <summary>
        /// Unpacked data
        /// </summary>
        public float[] Data;
        /// <summary>
        /// Scan mode
        /// </summary>
        public int scanMode;
        private int bitBuf = 0;
        private int bitPos = 0;
        private int Xlength;
        private int count;
        /// <summary>
        ///  flag to signifly if a static Missing Value is used. Since it's possible to have different missing values
        /// in a Grib file, the first record's missing value might not be the correct missing value for the current
        /// record. If a static missing value is used (float.NaN) then there will be no conflict of missing value
        /// processing.
        /// </summary>
        private static bool staticMissingValueInUse = true;

        private static int[] bitsmv1 = new int[31];      

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        ///<param name="br">binary reader</param>
        ///<param name="gds">grid definition section</param>
        ///<param name="drs">data representation section</param>
        ///<param name="bms">bit map section</param>
        public GRIB2DataSection(BinaryReader br, GRIB2GridDefinitionSection gds,
            GRIB2DataRepresentationSection drs, GRIB2BitMapSection bms)
        {
            for (int i = 0; i < 31; i++)
            {
                bitsmv1[i] = (int)Math.Pow((double)2, (double)i) - 1;
            }
  
            Length = Bytes2Number.Int4(br);
            SectionNum = br.ReadByte();

            int dtn = drs.DataTemplateNum;
            if (dtn == 0)
            {  // 0: Grid point data - simple packing
                SimpleUnpacking(br, gds, drs, bms);
            }
            else if (dtn == 1)
            {
                // 1: Matrix values - simple packing
                Data = null;
                //logger.error("Matrix values - simple packing not implemented");
            }
            else if (dtn == 2)
            {                       // 2:Grid point data - complex packing
                ComplexUnpacking(br, gds, drs, bms);
            }
            else if (dtn == 3)
            {    // 3: complex packing with spatial differencing               
                ComplexUnpackingWithSpatial(br, gds, drs, bms);
            }
            else if ((dtn == 40) || (dtn == 40000))
            {  // JPEG 2000 Stream Format
                jpeg2000Unpacking(br, gds, drs, bms);
                //MessageBox.Show("The packing method is not supported at present!", "Error");
            }
        }

        #endregion

        #region Methods
        private void SimpleUnpacking(BinaryReader br, GRIB2GridDefinitionSection gds,
            GRIB2DataRepresentationSection drs, GRIB2BitMapSection bms)
        {
            int dtn = drs.DataTemplateNum;
            int nb = drs.NumberOfBits;
            int D = drs.DecimalScaleFactor;
            float DD = (float)Math.Pow(10, D);
            float R = drs.ReferenceValue;
            int E = drs.BinaryScaleFactor;
            float EE = (float)Math.Pow(2.0, E);

            int numberPoints = gds.NumPoints;
            Data = new float[numberPoints];

            bool[] bitmap = bms.Bitmap;

            if (bitmap == null)
            {
                for (int i = 0; i < numberPoints; i++)
                {                    
                    Data[i] = (R + Bits2UInt(nb, br) * EE) / DD;
                }
            }
            else
            {
                bitPos = 0;
                bitBuf = 0;
                for (int i = 0; i < bitmap.Length; i++)
                {
                    if (bitmap[i])
                    {
                        Data[i] = (R + Bits2UInt(nb, br) * EE) / DD;
                    }
                    else
                    {                        
                        Data[i] = R / DD;
                    }
                }
            }

            scanMode = gds.ScanMode;
            Xlength = gds.NX;  // needs some smarts for different type Grids
            ScanningModeCheck();
        }

        private void ComplexUnpacking(BinaryReader br,
                                GRIB2GridDefinitionSection gds,
                                GRIB2DataRepresentationSection drs,
                                GRIB2BitMapSection bms)
        {

            int mvm = drs.MissingValueManagement;

            float mv = float.NaN;
            if (staticMissingValueInUse)
            {
                mv = float.NaN;
            }
            else if (mvm == 0)
            {
                mv = float.NaN;
            }
            else if (mvm == 1)
            {
                mv = drs.PrimaryMissingValue;
            }
            else if (mvm == 2)
            {
                mv = drs.SecondaryMissingValue;
            }

            int numberPoints = gds.NumPoints;
            int NG = drs.NumberOfGroups;

            if (NG == 0)
            {
                //logger.debug("Grib2DataSection.complexUnpacking : NG = 0 for file"+
                //raf.getLocation());
                //if( debug )
                //  System.out.println("Grib2DataSection.complexUnpacking : NG = 0 for file "+
                //    raf.getLocation());
                Data = new float[numberPoints];
                if (mvm == 0)
                {
                    for (int i = 0; i < numberPoints; i++)
                        Data[i] = mv;
                    //data[i] = X1[ i ];   should be equal to X1 but there's no X1 data
                }
                else
                { //if (mvm == 1) || (mvm == 2 )
                    for (int i = 0; i < numberPoints; i++)
                        Data[i] = mv;
                }
                return;
            }
            // 6-xx  Get reference values for groups (X1's)
            int[] X1 = new int[NG];
            int nb = drs.NumberOfBits;

            bitPos = 0;
            bitBuf = 0;
            for (int i = 0; i < NG; i++)
            {
                X1[i] = Bits2UInt(nb, br);
            }

            // [xx +1 ]-yy Get number of bits used to encode each group
            int[] NB = new int[NG];
            nb = drs.BitsGroupWidths;

            if (nb != 0)
            {
                bitPos = 0;
                bitBuf = 0;
                for (int i = 0; i < NG; i++)
                {
                    NB[i] = Bits2UInt(nb, br);
                }
            }
            else
            {
                for (int i = 0; i < NG; i++)
                    NB[i] = 0;
            }

            // [yy +1 ]-zz Get the scaled group lengths using formula
            //     Ln = ref + Kn * len_inc, where n = 1-NG,
            //          ref = referenceGroupLength, and  len_inc = lengthIncrement

            int[] L = new int[NG];
            int refgl = drs.ReferenceGroupLength;

            int len_inc = drs.LengthIncrement;

            nb = drs.BitsScaledGroupLength;

            bitPos = 0;
            bitBuf = 0;
            for (int i = 0; i < NG; i++)
            {  // NG
                L[i] = refgl + (Bits2UInt(nb, br) * len_inc);
                //System.out.println( "DS L[ i ]=" + L[ i ] );
            }
            //enter Length of Last Group
            L[NG - 1] = drs.LengthLastGroup;

            int D = drs.DecimalScaleFactor;
            //System.out.println( "DS D=" + D );
            float DD = (float)Math.Pow((double)10, (double)D);
            //System.out.println( "DS DD=" + DD );

            float R = drs.ReferenceValue;
            //System.out.println( "DS R=" + R );

            int E = drs.BinaryScaleFactor;
            //System.out.println( "DS E=" + E );
            float EE = (float)Math.Pow((double)2.0, (double)E);
            //System.out.println( "DS EE=" + EE );

            Data = new float[numberPoints];
            //System.out.println( "DS dataPoints="+ gds.getNumberPoints() );
            Xlength = gds.NX;  // needs some smarts for different type Grids

            // [zz +1 ]-nn get X2 values and calculate the results Y using formula

            //              Y = R + [(X1 + X2) * (2 ** E) * (10 ** D)]
            //               WHERE:
            //                     Y = THE VALUE WE ARE UNPACKING
            //                     R = THE REFERENCE VALUE (FIRST ORDER MINIMA)
            //                    X1 = THE PACKED VALUE
            //                    X2 = THE SECOND ORDER MINIMA
            //                     E = THE BINARY SCALE FACTOR
            //                     D = THE DECIMAL SCALE FACTOR
            count = 0;
            int X2;
            bitPos = 0;
            bitBuf = 0;
            for (int i = 0; i < NG; i++)
            {
                //System.out.println( "DS NB[ i ]=" + NB[ i ] );
                //System.out.println( "DS L[ i ]=" + L[ i ] );
                //System.out.println( "DS X1[ i ]=" + X1[ i ] );
                for (int j = 0; j < L[i]; j++)
                {
                    if (NB[i] == 0)
                    {
                        if (mvm == 0)
                        {  // X2 = 0
                            Data[count++] = (R + X1[i] * EE) / DD;
                        }
                        else
                        { //if (mvm == 1) || (mvm == 2 )
                            Data[count++] = mv;
                        }
                    }
                    else
                    {
                        X2 = Bits2UInt(NB[i], br);
                        if (mvm == 0)
                        {
                            Data[count++] = (R + (X1[i] + X2) * EE) / DD;
                        }
                        else
                        { //if (mvm == 1) || (mvm == 2 )
                            // X2 is also set to missing value if all bits set to 1's
                            if (X2 == bitsmv1[NB[i]])
                            {
                                Data[count++] = mv;
                            }
                            else
                            {
                                Data[count++] = (R + (X1[i] + X2) * EE) / DD;
                            }
                        }
                        //System.out.println( "DS count=" + count );
                        //System.out.println( "DS NB[ "+ i +" ]=" + NB[ i ] );
                        //System.out.println( "DS X1[ "+ i +" ]=" + X1[ i ] );
                        //System.out.println( "DS X2 =" +X2 );
                        //System.out.println( "DS X1[ i ] + X2 ="+(X1[ i ]+X2) );
                    }
                }  // end for j
            }      // end for i

            bool[] bitmap = bms.Bitmap;
            // bit map is used
            if (bitmap != null)
            {
                int idx = 0;
                float[] tmp = new float[numberPoints];
                for (int i = 0; i < numberPoints; i++)
                {
                    if (bitmap[i])
                    {
                        tmp[i] = Data[idx++];
                    }
                    else
                    {
                        tmp[i] = mv;
                    }
                    //System.out.println( "tmp[ "+ i +"] ="+ data[ i]);
                }
                Data = tmp;
            } //end bitmap

            scanMode = gds.ScanMode;
            ScanningModeCheck();
        }

        private void ComplexUnpackingWithSpatial(BinaryReader br,
                                           GRIB2GridDefinitionSection gds,
                                           GRIB2DataRepresentationSection drs,
                                           GRIB2BitMapSection bms)
        {
            // check first if missing values
            int mvm = drs.MissingValueManagement;
            //System.out.println( "DS mvm=" + mvm );

            float mv = float.NaN;
            if (staticMissingValueInUse)
            {
                mv = float.NaN;
            }
            else if (mvm == 0)
            {
                mv = float.NaN;
            }
            else if (mvm == 1)
            {
                mv = drs.PrimaryMissingValue;
            }
            else if (mvm == 2)
            {
                mv = drs.SecondaryMissingValue;
            }

            int ival1 = 0,
            ival2 = 0,
            minsd = 0;

            // [6-ww]   1st values of undifferenced scaled values and minimums
            int os = drs.OrderSpatial;
            int nbitsd = drs.DescriptorSpatial;
            //System.out.println( "DS os=" + os +" ds =" + ds );
            bitPos = 0;
            bitBuf = 0;
            int sign;
            // ds is number of bytes, convert to bits -1 for sign bit
            nbitsd = nbitsd * 8;
            if (nbitsd != 0)
            {         // first order spatial differencing g1 and gMin
                sign = Bits2UInt(1, br);
                ival1 = Bits2UInt(nbitsd - 1, br);
                if (sign == 1)
                {
                    ival1 = -ival1;
                }
                if (os == 2)
                {  //second order spatial differencing h1, h2, hMin
                    sign = Bits2UInt(1, br);
                    ival2 = Bits2UInt(nbitsd - 1, br);
                    if (sign == 1)
                    {
                        ival2 = -ival2;
                    }
                }
                sign = Bits2UInt(1, br);
                minsd = Bits2UInt(nbitsd - 1, br);
                if (sign == 1)
                {
                    minsd = -minsd;
                }
                //System.out.println( "DS nbitsd ="+ nbitsd +" ival1=" + ival1 +" ival2 =" + ival2 + " minsd=" + minsd );
            }
            else
            {
                return;
            }

            int numberPoints = gds.NumPoints;
            int NG = drs.NumberOfGroups;
            //System.out.println( "DS NG=" + NG );
            if (NG == 0)
            {
                //logger.error("Grib2DataSection.complexUnpackingWithSpatial: NG = 0 for file"+ raf.getLocation());
                //if( debug )
                //  System.out.println("Grib2DataSection.complexUnpackingWithSpatial: NG = 0 for file "+
                //    raf.getLocation());
                Data = new float[numberPoints];
                if (mvm == 0)
                {
                    for (int i = 0; i < numberPoints; i++)
                        Data[i] = mv;
                }
                else
                { //if (mvm == 1) || (mvm == 2 )
                    for (int i = 0; i < numberPoints; i++)
                        Data[i] = mv;
                }
                return;
            }

            // [ww +1]-xx  Get reference values for groups (X1's)
            // X1 == gref
            int[] X1 = new int[NG];
            int nb = drs.NumberOfBits;
            //System.out.println( "DS nb=" + nb );
            if (nb != 0)
            {
                bitPos = 0;
                bitBuf = 0;
                for (int i = 0; i < NG; i++)
                {
                    X1[i] = Bits2UInt(nb, br);
                    //System.out.println( "DS X1[ i ]=" + X1[ i ] );
                }
            }
            else
            {
                for (int i = 0; i < NG; i++)
                {
                    X1[i] = 0;
                }
            }

            // [xx +1 ]-yy Get number of bits used to encode each group
            // NB == gwidth
            int[] NB = new int[NG];
            nb = drs.BitsGroupWidths;
            //System.out.println( "DS nb=" + nb );
            if (nb != 0)
            {
                bitPos = 0;
                bitBuf = 0;
                for (int i = 0; i < NG; i++)
                {
                    NB[i] = Bits2UInt(nb, br);
                    //System.out.println( "DS X1[ i ]=" + X1[ i ] );
                }
            }
            else
            {
                for (int i = 0; i < NG; i++)
                {
                    NB[i] = 0;
                }
            }

            int referenceGroupWidths = drs.ReferenceGroupWidths;
            //System.out.println( "DS len_inc=" + len_inc );
            for (int i = 0; i < NG; i++)
            {
                NB[i] += referenceGroupWidths;
            }

            // [yy +1 ]-zz Get the scaled group lengths using formula
            //     Ln = ref + Kn * len_inc, where n = 1-NG,
            //          ref = referenceGroupLength, and  len_inc = lengthIncrement

            int[] L = new int[NG];
            // L == glen
            int referenceGroupLength = drs.ReferenceGroupLength;
            //System.out.println( "DS ref=" + ref );

            nb = drs.BitsScaledGroupLength;
            //System.out.println( "DS nb=" + nb );
            int len_inc = drs.LengthIncrement;

            bitPos = 0;
            bitBuf = 0;

            if (nb != 0)
            {
                for (int i = 0; i < NG; i++)
                {
                    L[i] = Bits2UInt(nb, br);

                }
            }
            else
            {
                for (int i = 0; i < NG; i++)
                    L[i] = 0;
            }
            int totalL = 0;
            //System.out.println( "DS NG=" + NG );
            for (int i = 0; i < NG; i++)
            {
                L[i] = L[i] * len_inc + referenceGroupLength;
                //System.out.print( "DS L[ i ]=" + L[ i ] );
                totalL += L[i];
                //System.out.println( " totalL=" + totalL +" "+ i);
            }
            totalL -= L[NG - 1];
            totalL += drs.LengthLastGroup;

            //enter Length of Last Group
            L[NG - 1] = drs.LengthLastGroup;

            // test
            if (mvm != 0)
            {
                if (totalL != numberPoints)
                {
                    for (int i = 0; i < numberPoints; i++)
                        Data[i] = mv;
                    return;
                }
            }
            else
            {
                if (totalL != drs.DataPoints)
                {
                    for (int i = 0; i < numberPoints; i++)
                        Data[i] = mv;
                    return;
                }
            }



            int D = drs.DecimalScaleFactor;
            //System.out.println( "DS D=" + D );
            float DD = (float)Math.Pow((double)10, (double)D);
            //System.out.println( "DS DD=" + DD );

            float R = drs.ReferenceValue;
            //System.out.println( "DS R=" + R );

            int E = drs.BinaryScaleFactor;
            //System.out.println( "DS E=" + E );
            float EE = (float)Math.Pow((double)2.0, (double)E);
            //System.out.println( "DS EE=" + EE );

            Data = new float[numberPoints];
            //System.out.println( "DS dataPoints="+ gds.getNumberPoints() );
            Xlength = gds.NX;  // needs some smarts for different type Grids

            // [zz +1 ]-nn get X2 values and calculate the results Y using formula
            //      formula used to create values,  Y * 10**D = R + (X1 + X2) * 2**E

            //               Y = (R + (X1 + X2) * (2 ** E) ) / (10 ** D)]
            //               WHERE:
            //                     Y = THE VALUE WE ARE UNPACKING
            //                     R = THE REFERENCE VALUE (FIRST ORDER MINIMA)
            //                    X1 = THE PACKED VALUE
            //                    X2 = THE SECOND ORDER MINIMA
            //                     E = THE BINARY SCALE FACTOR
            //                     D = THE DECIMAL SCALE FACTOR
            count = 0;
            bitPos = 0;
            bitBuf = 0;
            int dataSize = 0;
            bool[] dataBitMap = null;
            if (mvm == 0)
            {
                for (int i = 0; i < NG; i++)
                {
                    //System.out.println( "DS NB[ i ]=" + NB[ i ] );
                    //System.out.println( "DS L[ i ]=" + L[ i ] );
                    //System.out.println( "DS X1[ i ]=" + X1[ i ] );
                    if (NB[i] != 0)
                    {
                        for (int j = 0; j < L[i]; j++)
                            Data[count++] = Bits2UInt(NB[i], br) + X1[i];
                    }
                    else
                    {
                        for (int j = 0; j < L[i]; j++)
                            Data[count++] = X1[i];
                        //System.out.println( "DS count=" + count );
                        //System.out.println( "DS NB[ "+ i +" ]=" + NB[ i ] );
                        //System.out.println( "DS X1[ "+ i +" ]=" + X1[ i ] );
                        //System.out.println( "Data[ "+ (count -1) +"] ="+ Data[ count -1 ]);
                    }
                }  // end for i
            }
            else if (mvm == 1 || mvm == 2)
            {
                // don't add missing values into Data but keep track of them in dataBitMap
                dataBitMap = new bool[numberPoints];
                dataSize = 0;
                for (int i = 0; i < NG; i++)
                {
                    //System.out.println( "DS NB[ i ]=" + NB[ i ] );
                    //System.out.println( "DS L[ i ]=" + L[ i ] );
                    //System.out.println( "DS X1[ i ]=" + X1[ i ] );
                    if (NB[i] != 0)
                    {
                        int msng1 = bitsmv1[NB[i]];
                        int msng2 = msng1 - 1;
                        for (int j = 0; j < L[i]; j++)
                        {
                            Data[count] = Bits2UInt(NB[i], br);
                            if (Data[count] == msng1)
                            {
                                dataBitMap[count] = false;
                            }
                            else if (mvm == 2 && Data[count] == msng2)
                            {
                                dataBitMap[count] = false;
                            }
                            else
                            {
                                dataBitMap[count] = true;
                                Data[dataSize++] = Data[count] + X1[i];
                            }
                            count++;
                        }
                    }
                    else
                    {
                        int msng1 = bitsmv1[drs.NumberOfBits];
                        int msng2 = msng1 - 1;
                        if (X1[i] == msng1)
                        {
                            for (int j = 0; j < L[i]; j++)
                                dataBitMap[count++] = false;
                            //Data[count++] = X1[i];
                        }
                        else if (mvm == 2 && X1[i] == msng2)
                        {
                            for (int j = 0; j < L[i]; j++)
                                dataBitMap[count++] = false;
                        }
                        else
                        {
                            for (int j = 0; j < L[i]; j++)
                            {
                                dataBitMap[count] = true;
                                Data[dataSize++] = X1[i];
                                count++;
                            }
                        }
                        //System.out.println( "DS count=" + count );
                        //System.out.println( "DS NB[ "+ i +" ]=" + NB[ i ] );
                        //System.out.println( "DS X1[ "+ i +" ]=" + X1[ i ] );
                        //System.out.println( "DS X2 =" +X2 );
                        //System.out.println( "DS X1[ i ] + X2 ="+(X1[ i ]+X2) );
                        //System.out.println( "Data[ "+ (count -1) +"] ="+ Data[ count -1 ]);
                    }
                }  // end for i
            }

            // first order spatial differencing
            if (os == 1)
            {   // g1 and gMin
                // encoded by G(n) = F(n) - F(n -1 )
                // decoded by F(n) = G(n) + F(n -1 )
                // Data[] at this point contains G0, G1, G2, ....
                Data[0] = ival1;
                int itemp;
                if (mvm == 0)
                {           // no missing values
                    itemp = numberPoints;
                }
                else
                {
                    itemp = dataSize;
                }
                for (int i = 1; i < itemp; i++)
                {
                    Data[i] += minsd;
                    Data[i] = Data[i] + Data[i - 1];
                    //System.out.println( "Data[ "+ i +"] ="+ Data[ i ]);
                }
            }
            else if (os == 2)
            { // 2nd order
                Data[0] = ival1;
                Data[1] = ival2;
                int itemp;
                if (mvm == 0)
                {           // no missing values
                    itemp = numberPoints;
                }
                else
                {
                    itemp = dataSize;
                }
                for (int i = 2; i < itemp; i++)
                {
                    Data[i] += minsd;
                    Data[i] = Data[i] + (2 * Data[i - 1]) - Data[i - 2];
                    //System.out.println( "Data[ "+ i +"] ="+ Data[ i ]);
                }
            }

            // formula used to create values,  Y * 10**D = R + (X1 + X2) * 2**E

            //               Y = (R + (X1 + X2) * (2 ** E) ) / (10 ** D)]
            //               WHERE:
            //                     Y = THE VALUE WE ARE UNPACKING
            //                     R = THE REFERENCE VALUE (FIRST ORDER MINIMA)
            //                    X1 = THE PACKED VALUE
            //                    X2 = THE SECOND ORDER MINIMA
            //                     E = THE BINARY SCALE FACTOR
            //                     D = THE DECIMAL SCALE FACTOR

            if (mvm == 0)
            {  // no missing values
                for (int i = 0; i < Data.Length; i++)
                {
                    Data[i] = (R + (Data[i] * EE)) / DD;
                    //System.out.println( "Data[ "+ i +"] ="+ Data[ i ]);
                }
            }
            else
            {         // missing value == 1  || missing value == 2
                dataSize = 0;
                float[] tmp = new float[numberPoints];
                for (int i = 0; i < Data.Length; i++)
                {
                    if (dataBitMap[i])
                    {
                        tmp[i] = (R + (Data[dataSize++] * EE)) / DD;
                    }
                    else
                    { // mvm = 1 or 2
                        tmp[i] = mv;
                    }
                }
                Data = tmp;
            }

            bool[] bitmap = bms.Bitmap;
            // bit map is used
            if (bitmap != null)
            {
                int idx = 0;
                float[] tmp = new float[numberPoints];
                for (int i = 0; i < numberPoints; i++)
                {
                    if (bitmap[i])
                    {
                        tmp[i] = Data[idx++];
                    }
                    else
                    {
                        tmp[i] = mv;
                    }
                    //System.out.println( "tmp[ "+ i +"] ="+ Data[ i]);
                }
                Data = tmp;
            }

            scanMode = gds.ScanMode;
            ScanningModeCheck();
            //System.out.println( "DS true end =" + raf.position() );

        }

        private void jpeg2000Unpacking(BinaryReader br,
                                 GRIB2GridDefinitionSection gds,
                                 GRIB2DataRepresentationSection drs,
                                 GRIB2BitMapSection bms)
        {
            // 6-xx  jpeg2000 data block to decode

            // dataPoints are number of points encoded, it could be less than the
            // numberPoints in the grid record if bitMap is used, otherwise equal
            //int dataPoints = drs.getDataPoints();
            //System.out.println( "DS DRS dataPoints=" + drs.getDataPoints() );
            //System.out.println( "DS length=" + length );

            int mvm = drs.MissingValueManagement;
            //System.out.println( "DS mvm=" + mvm );

            float mv = float.NaN;
            if (staticMissingValueInUse)
            {
                mv = float.NaN;
            }
            else if (mvm == 0)
            {
                mv = float.NaN;
            }
            else if (mvm == 1)
            {
                mv = drs.PrimaryMissingValue;
            }
            else if (mvm == 2)
            {
                mv = drs.SecondaryMissingValue;
            }

            int nb = drs.NumberOfBits;
            //System.out.println( "DS nb = " + nb );

            int D = drs.DecimalScaleFactor;
            //System.out.println( "DS D=" + D );
            float DD = (float)Math.Pow((double)10, (double)D);
            //System.out.println( "DS DD=" + DD );

            float R = drs.ReferenceValue;
            //System.out.println( "DS R=" + R );

            int E = drs.BinaryScaleFactor;
            //System.out.println( "DS E=" + E );
            float EE = (float)Math.Pow((double)2.0, (double)E);
            //System.out.println( "DS EE=" + EE );

            Grib2JpegDecoder g2j = null;
            int numberPoints = 0;
            try
            {
                if (nb != 0)
                {  // there's data to decode
                    String[] argv = new String[4];
                    argv[0] = "-rate";
                    argv[1] = nb.ToString();
                    argv[2] = "-verbose";
                    argv[3] = "off";
                    //argv[ 4 ] = "-debug" ;
                    //argv[ 2 ] = "-nocolorspace" ;
                    //argv[ 3 ] = "-Rno_roi" ;
                    //argv[ 4 ] = "-cdstr_info" ;
                    //argv[ 5 ] = "-verbose" ;
                    g2j = new Grib2JpegDecoder(argv);
                    // how jpeg2000.jar use to decode, used raf
                    //g2j.decode(raf, length - 5);
                    // jpeg-1.0.jar added method to have the data read first
                    byte[] buf = new byte[Length - 5];
                    br.Read(buf, 0, buf.Length);
                    g2j.decode(buf);
                }
                numberPoints = gds.NumPoints;
                //System.out.println( "DS GDS NumberPoints=" +  gds.getNumberPoints() );
                Data = new float[numberPoints];
                bool[] bitmap = bms.Bitmap;

                if (nb == 0)
                {  // no data to decoded, set to reference or  MissingValue
                    if (mvm == 0)
                    {
                        for (int i = 0; i < numberPoints; i++)
                            Data[i] = R;
                    }
                    else
                    { //if (mvm == 1) || (mvm == 2 )
                        for (int i = 0; i < numberPoints; i++)
                            Data[i] = mv;
                    }
                }
                else if (bitmap == null)
                {
                    //System.out.println( "DS jpeg data length ="+ g2j.data.length );
                    if (g2j.data.Length != numberPoints)
                    {
                        Data = null;
                        return;
                    }
                    for (int i = 0; i < numberPoints; i++)
                    {
                        //Y = (R + ( 0 + X2) * EE)/DD ;
                        Data[i] = (R + g2j.data[i] * EE) / DD;
                        //System.out.println( "DS data[ " + i +"  ]=" + data[ i ] );
                    }
                }
                else
                {  // use bitmap
                    for (int i = 0, j = 0; i < bitmap.Length; i++)
                    {
                        if (i == Data.Length)
                            break;

                        if (bitmap[i] && j < g2j.data.Length)
                        {
                            Data[i] = (R + g2j.data[j++] * EE) / DD;
                        }
                        else
                        {
                            Data[i] = mv;
                        }
                    }
                }
            }
            catch
            {
                for (int i = 0; i < numberPoints; i++)
                {
                    Data[i] = mv;
                }
                return;
            }
            scanMode = gds.ScanMode;
            ScanningModeCheck();
        }

        private int Bits2UInt(int nb, BinaryReader br)
        {
            int bitsLeft = nb;
            int result = 0;

            if (bitPos == 0)
            {
                //bitBuf = br.Read();
                bitBuf = br.ReadByte();
                bitPos = 8;
            }

            while (true)
            {
                int shift = bitsLeft - bitPos;
                if (shift > 0)
                {
                    // Consume the entire buffer
                    result |= bitBuf << shift;
                    bitsLeft -= bitPos;

                    // Get the next byte from the RandomAccessFile
                    //bitBuf = br.Read();
                    bitBuf = br.ReadByte();
                    bitPos = 8;
                }
                else
                {
                    // Consume a portion of the buffer
                    result |= bitBuf >> -shift;
                    bitPos -= bitsLeft;
                    bitBuf &= 0xff >> (8 - bitPos);  // mask off consumed bits

                    return result;
                }
            }
        }

        private void ScanningModeCheck()
        {
            float tmp;
            int mid = (int)Xlength / 2;

            // Mode  0 +x, -y, adjacent x, adjacent rows same dir
            // Mode  64 +x, +y, adjacent x, adjacent rows same dir
            if ((scanMode == 0) || (scanMode == 64))
            {
                return;                
            }
            // Mode  128 -x, -y, adjacent x, adjacent rows same dir
            // Mode  192 -x, +y, adjacent x, adjacent rows same dir
            // change -x to +x ie east to west -> west to east
            else if ((scanMode == 128) || (scanMode == 192))
            {
                mid = (int)Xlength / 2;
                //System.out.println( "Xlength =" +Xlength +" mid ="+ mid );
                for (int index = 0; index < Data.Length; index += Xlength)
                {
                    for (int idx = 0; idx < mid; idx++)
                    {
                        tmp = Data[index + idx];
                        Data[index + idx] = Data[index + Xlength - idx - 1];
                        Data[index + Xlength - idx - 1] = tmp;
                        //System.out.println( "switch " + (index + idx) + " " +
                        //(index + Xlength -idx -1) );
                    }
                }
                return;
            }
            // else
            // scanMode == 16, 80, 144, 208 adjacent rows scan opposite dir            
            //System.out.println( "Xlength =" +Xlength +" mid ="+ mid );
            for (int index = 0; index < Data.Length; index += Xlength)
            {
                int row = (int)index / Xlength;
                if (row % 2 == 1)
                {  // odd numbered row, calculate reverse index
                    for (int idx = 0; idx < mid; idx++)
                    {
                        tmp = Data[index + idx];
                        Data[index + idx] = Data[index + Xlength - idx - 1];
                        Data[index + Xlength - idx - 1] = tmp;
                        //System.out.println( "switch " + (index + idx) + " " +
                        //(index + Xlength -idx -1) );
                    }
                }
            }
        }

        #endregion

    }
}
