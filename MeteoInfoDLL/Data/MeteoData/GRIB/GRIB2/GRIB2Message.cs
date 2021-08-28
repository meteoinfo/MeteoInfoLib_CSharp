using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 2 message
    /// </summary>
    public class GRIB2Message
    {
        #region Variables
        /// <summary>
        /// Indicator Section
        /// </summary>
        public GRIB2IndicatorSection GribINS = null;
        /// <summary>
        /// Identification Section
        /// </summary>
        public GRIB2IdentificationSection GribIDS = null;
        /// <summary>
        /// Local Use Section
        /// </summary>
        public GRIB2LocalUseSection GribLUS = null;
        /// <summary>
        /// Grid Definition Section
        /// </summary>
        public GRIB2GridDefinitionSection GribGDS = null;
        /// <summary>
        /// Product Definition Section
        /// </summary>
        public GRIB2ProductDefinitionSection GribPDS = null;
        /// <summary>
        /// Data Representation Section
        /// </summary>
        public GRIB2DataRepresentationSection GribDRS = null;
        /// <summary>
        /// Bit Map Section
        /// </summary>
        public GRIB2BitMapSection GribBMS = null;
        /// <summary>
        /// Data Section
        /// </summary>
        public GRIB2DataSection GribDS = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        public GRIB2Message(BinaryReader br)
        {
            GribINS = new GRIB2IndicatorSection(br);
            GribIDS = new GRIB2IdentificationSection(br);
            int sectionNum = GRIBData.ReadSectionNumber(br);
            if (sectionNum == 2)
                GribLUS = new GRIB2LocalUseSection(br);

            GribGDS = new GRIB2GridDefinitionSection(br);
            GribPDS = new GRIB2ProductDefinitionSection(br);
            GribDRS = new GRIB2DataRepresentationSection(br);
            GribBMS = new GRIB2BitMapSection(br);
            GribDS = new GRIB2DataSection(br, GribGDS, GribDRS, GribBMS);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        /// <param name="startSection">start section</param>
        /// <param name="startPos">start position</param>
        public GRIB2Message(BinaryReader br, int startSection, long startPos)
        {
            GribINS = new GRIB2IndicatorSection(br);
            GribIDS = new GRIB2IdentificationSection(br);
            if (startSection == 2)
            {
                br.BaseStream.Position = startPos;
                GribLUS = new GRIB2LocalUseSection(br);
                GribGDS = new GRIB2GridDefinitionSection(br);
                GribPDS = new GRIB2ProductDefinitionSection(br);
                GribDRS = new GRIB2DataRepresentationSection(br);
                GribBMS = new GRIB2BitMapSection(br);
                GribDS = new GRIB2DataSection(br, GribGDS, GribDRS, GribBMS);

            }
            else
            {
                int sectionNum = GRIBData.ReadSectionNumber(br);
                if (sectionNum == 2)
                    GribLUS = new GRIB2LocalUseSection(br);

                switch (startSection)
                {
                    case 3:
                        br.BaseStream.Position = startPos;                        
                        GribGDS = new GRIB2GridDefinitionSection(br);
                        GribPDS = new GRIB2ProductDefinitionSection(br);
                        GribDRS = new GRIB2DataRepresentationSection(br);
                        GribBMS = new GRIB2BitMapSection(br);
                        GribDS = new GRIB2DataSection(br, GribGDS, GribDRS, GribBMS);
                        break;
                    case 4:
                        GribGDS = new GRIB2GridDefinitionSection(br);
                        br.BaseStream.Position = startPos;                        
                        GribPDS = new GRIB2ProductDefinitionSection(br);
                        GribDRS = new GRIB2DataRepresentationSection(br);
                        GribBMS = new GRIB2BitMapSection(br);
                        GribDS = new GRIB2DataSection(br, GribGDS, GribDRS, GribBMS);
                        break;
                }
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get data 2-D array
        /// </summary>
        /// <returns>data array</returns>
        public double[,] GetDataArray()
        {
            double[,] data = new double[GribGDS.NY, GribGDS.NX];
            for (int i = 0; i < GribGDS.NY; i++)
            {
                for (int j = 0; j < GribGDS.NX; j++)
                {
                    data[i, j] = GribDS.Data[i * GribGDS.NX + j];
                }
            }

            return data;
        }

        #endregion
    }
}
