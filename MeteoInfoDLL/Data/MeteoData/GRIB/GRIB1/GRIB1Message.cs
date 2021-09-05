using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 1 message
    /// </summary>
    public class GRIB1Message
    {
        #region Variables
        /// <summary>
        /// Indeicator section
        /// </summary>
        public GRIB1IndicatorSection RecordIS = null;
        /// <summary>
        /// Product definition section
        /// </summary>
        public GRIB1ProductDefineSection RecordPDS = null;
        /// <summary>
        /// Grid definition section
        /// </summary>
        public GRIB1GridDefineSection RecordGDS = null;
        /// <summary>
        /// Bitmap section
        /// </summary>
        public GRIB1BitMapSection RecordBMS = null;
        /// <summary>
        /// Binary data section
        /// </summary>
        public GRIB1BinaryDataSection RecordBDS = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        /// <param name="aDataInfo">GRIB1 data info</param>
        public GRIB1Message(BinaryReader br, GRIB1DataInfo aDataInfo)
        {
            RecordIS = new GRIB1IndicatorSection(br);          // read Indicator Section

            RecordPDS = new GRIB1ProductDefineSection(br);         // read Product Definition Section

            if (RecordPDS.GDSExist)
            {
                RecordGDS = new GRIB1GridDefineSection(br);
            }
            else
            {
                //throw new NoValidGribException("GribRecord: No GDS included.");
            }

            if (RecordPDS.BMSExist)
            {
                RecordBMS = new GRIB1BitMapSection(br);     // read Bitmap Section
            }

            // read Binary Data Section
            if (RecordPDS.GDSExist)
                RecordBDS = new GRIB1BinaryDataSection(br, RecordPDS.DecimalScale, RecordBMS, RecordGDS.ScanMode,
                    RecordGDS.NX, RecordGDS.NY);
            else
                RecordBDS = new GRIB1BinaryDataSection(br, RecordPDS.DecimalScale, RecordBMS, 64, aDataInfo.X.Length,
                    aDataInfo.Y.Length);

        }

        #endregion

        #region Methods
        /// <summary>
        /// Get data 2-D array
        /// </summary>
        /// <returns>data array</returns>
        public double[,] GetDataArray()
        {
            double[,] data = new double[RecordGDS.NY, RecordGDS.NX];
            for (int i = 0; i < RecordGDS.NY; i++)
            {
                for (int j = 0; j < RecordGDS.NX; j++)
                {
                    data[i, j] = RecordBDS.Data[i * RecordGDS.NX + j];
                }
            }

            return data;
        }

        #endregion
    }
}
