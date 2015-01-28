using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 2 data representation section
    /// </summary>
    public class GRIB2DataRepresentationSection
    {
        #region Vairables
        /// <summary>
        /// Section length in bytes
        /// </summary>
        public int Length;
        /// <summary>
        /// Section number: 5
        /// </summary>
        public int SectionNum;
        /// <summary>
        /// Number of data points
        /// </summary>
        public int DataPoints;
        /// <summary>
        /// Data representation template number
        /// </summary>
        public int DataTemplateNum;
        /// <summary>
        /// Reference value (R) (IEEE 32-bit floating-point value).
        /// </summary>       
        public float ReferenceValue;
        /// <summary>
        /// Binary scale factor (E).
        /// </summary>
        public int BinaryScaleFactor;
        /// <summary>
        /// Decimal scale factor (D).
        /// </summary>
        public int DecimalScaleFactor;
        /// <summary>
        /// Number of bits used for each packed value.
        /// </summary>
        public int NumberOfBits;
        /// <summary>
        /// data type of original field values.
        /// </summary>
        private int originalType;
        /// <summary>
        /// Group splitting method used (see Code Table 5.4).
        /// </summary>
        private int splittingMethod;
        /// <summary>
        /// Type compression method used (see Code Table 5.40000).
        /// </summary>
        private int compressionMethod;
        /// <summary>
        /// Compression ratio used.
        /// </summary>
        private int compressionRatio;
        /// <summary>
        /// Missing value management used (see Code Table 5.5).
        /// </summary>
        public int MissingValueManagement;
        /// <summary>
        /// Primary missing value substitute.
        /// </summary>
        public float PrimaryMissingValue = (float)Bytes2Number.UNDEF;
        /// <summary>
        /// Secondary missing value substitute.
        /// </summary>
        public float SecondaryMissingValue = (float)Bytes2Number.UNDEF;
        /// <summary>
        /// NG - Number of groups of data values into which field is split.
        /// </summary>
        public int NumberOfGroups;
        /// <summary>
        /// Reference for group widths (see Note 12).
        /// </summary>
        public int ReferenceGroupWidths;
        /// <summary>
        /// Number of bits used for the group widths (after the reference value.
        /// in octet 36 has been removed)
        /// </summary>
        public int BitsGroupWidths;
        /// <summary>
        /// Reference for group lengths (see Note 13).
        /// </summary>
        public int ReferenceGroupLength;
        /// <summary>
        /// Length increment for the group lengths (see Note 14).
        /// </summary>
        public int LengthIncrement;
        /// <summary>
        /// Length for the last group (see Note 14).
        /// </summary>
        public int LengthLastGroup;
        /// <summary>
        /// Number of bits used for the scaled group lengths (after subtraction of
        /// the reference value given in octets 38-41 and division by the length
        /// increment given in octet 42).
        /// </summary>
        public int BitsScaledGroupLength;
        /// <summary>
        /// Order of spatial differencing (see Code Table 5.6).
        /// </summary>
        public int OrderSpatial;
        /// <summary>
        /// Number of octets required in the Data Section to specify extra
        /// descriptors needed for spatial differencing (octets 6-ww in Data
        /// Template 7.3) .
        /// </summary>
        public int DescriptorSpatial;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        public GRIB2DataRepresentationSection(BinaryReader br)
        {
            long sectionEnd = br.BaseStream.Position;

            Length = Bytes2Number.Int4(br);
            sectionEnd += Length;

            SectionNum = br.ReadByte();
            DataPoints = Bytes2Number.Int4(br);
            DataTemplateNum = Bytes2Number.Uint2(br);

            switch (DataTemplateNum)
            {  // Data Template Number

                case 0:
                case 1:               // 0 - Grid point data - simple packing 
                    // 1 - Matrix values - simple packing              
                    //ReferenceValue = br.ReadSingle();
                    ReferenceValue = Bytes2Number.Float(br);
                    BinaryScaleFactor = Bytes2Number.Int2(br);
                    DecimalScaleFactor = Bytes2Number.Int2(br);
                    NumberOfBits = br.ReadByte();
                    //System.out.println( "DRS numberOfBits=" + numberOfBits );
                    originalType = br.ReadByte();
                    //System.out.println( "DRS originalType=" + originalType );

                    if (DataTemplateNum == 0)
                    {
                        break;
                    }
                    // case 1 not implememted
                    //System.out.println("DRS dataTemplate=1 not implemented yet");
                    break;

                case 2:
                case 3:  // Grid point data - complex packing
                    //System.out.println( "DRS dataTemplate=" + dataTemplate );
                    // octet 12 - 15
                    //ReferenceValue = br.ReadSingle();
                    ReferenceValue = Bytes2Number.Float(br);
                    // octet 16 - 17
                    BinaryScaleFactor = Bytes2Number.Int2(br);
                    // octet 18 - 19
                    DecimalScaleFactor = Bytes2Number.Int2(br);
                    // octet 20
                    NumberOfBits = br.ReadByte();
                    //System.out.println( "DRS numberOfBits=" + numberOfBits );
                    // octet 21
                    originalType = br.ReadByte();
                    //System.out.println( "DRS originalType=" + originalType );
                    // octet 22
                    splittingMethod = br.ReadByte();
                    //System.out.println( "DRS splittingMethod=" + 
                    //     splittingMethod );
                    // octet 23
                    MissingValueManagement = br.ReadByte();
                    //System.out.println( "DRS MissingValueManagement=" + 
                    //     MissingValueManagement );
                    // octet 24 - 27
                    PrimaryMissingValue = br.ReadSingle();
                    // octet 28 - 31
                    SecondaryMissingValue = br.ReadSingle();
                    // octet 32 - 35
                    NumberOfGroups = Bytes2Number.Int4(br);
                    //System.out.println( "DRS numberOfGroups=" + 
                    //     numberOfGroups );
                    // octet 36
                    ReferenceGroupWidths = br.ReadByte();
                    //System.out.println( "DRS referenceGroupWidths=" + 
                    //     referenceGroupWidths );
                    // octet 37
                    BitsGroupWidths = br.ReadByte();
                    // according to documentation subtract referenceGroupWidths
                    // TODO: check again and delete
                    //bitsGroupWidths = bitsGroupWidths - referenceGroupWidths;
                    //System.out.println( "DRS bitsGroupWidths=" + 
                    //     bitsGroupWidths );
                    // octet 38 - 41
                    ReferenceGroupLength = Bytes2Number.Int4(br);
                    //System.out.println( "DRS referenceGroupLength=" + 
                    //     referenceGroupLength );
                    // octet 42
                    LengthIncrement = br.ReadByte();
                    //System.out.println( "DRS lengthIncrement=" + 
                    //     lengthIncrement );
                    // octet 43 - 46
                    LengthLastGroup = Bytes2Number.Int4(br);
                    //System.out.println( "DRS lengthLastGroup=" + 
                    //     lengthLastGroup );
                    // octet 47
                    BitsScaledGroupLength = br.ReadByte();
                    //System.out.println( "DRS bitsScaledGroupLength=" + 
                    //     bitsScaledGroupLength );
                    if (DataTemplateNum == 2)
                    {
                        break;
                    }

                    // case 3 // complex packing & spatial differencing
                    // octet 48
                    OrderSpatial = br.ReadByte();
                    //System.out.println( "DRS orderSpatial=" + orderSpatial);
                    // octet 49
                    DescriptorSpatial = br.ReadByte();
                    //System.out.println( "DRS descriptorSpatial=" + descriptorSpatial);
                    break;

                case 40:
                case 40000:  // Grid point data - JPEG 2000 Code Stream Format
                    //System.out.println( "DRS dataTemplate=" + dataTemplate );
                    //ReferenceValue = br.ReadSingle();
                    ReferenceValue = Bytes2Number.Float(br);
                    BinaryScaleFactor = Bytes2Number.Int2(br);
                    DecimalScaleFactor = Bytes2Number.Int2(br);
                    NumberOfBits = br.ReadByte();
                    //System.out.println( "DRS numberOfBits=" + numberOfBits );
                    originalType = br.ReadByte();
                    //System.out.println( "DRS originalType=" + originalType );
                    compressionMethod = br.ReadByte();
                    //System.out.println( "DRS compressionMethod=" + compressionMethod );
                    compressionRatio = br.ReadByte();
                    //System.out.println( "DRS compressionRatio=" + compressionRatio );
                    break;

                default:
                    break;
            }

            //Set stream position to the section end
            br.BaseStream.Position = sectionEnd;
        }

        #endregion

        #region Mehods


        #endregion
    }
}
