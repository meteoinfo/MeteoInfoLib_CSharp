using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Meteorological data type
    /// </summary>
    public enum MeteoDataType
    {
        /// <summary>
        /// GrADS grid
        /// </summary>
        GrADS_Grid,
        /// <summary>
        /// GrADS station
        /// </summary>
        GrADS_Station,
        /// <summary>
        /// MICAPS 1
        /// </summary>
        MICAPS_1,
        /// <summary>
        /// MICAPS 2
        /// </summary>
        MICAPS_2,
        /// <summary>
        /// MICAPS 3
        /// </summary>
        MICAPS_3,
        /// <summary>
        /// MICAPS 4
        /// </summary>
        MICAPS_4,
        /// <summary>
        /// MICAPS 7
        /// </summary>
        MICAPS_7,
        /// <summary>
        /// MICAPS 11
        /// </summary>
        MICAPS_11,
        /// <summary>
        /// MICAPS 13
        /// </summary>
        MICAPS_13,
        /// <summary>
        /// HYSPLIT concentration
        /// </summary>
        HYSPLIT_Conc,
        /// <summary>
        /// HYSPLIT particle
        /// </summary>
        HYSPLIT_Particle,
        /// <summary>
        /// HYSPLIT trajectory
        /// </summary>
        HYSPLIT_Traj,
        /// <summary>
        /// ARL grid
        /// </summary>
        ARL_Grid,
        /// <summary>
        /// NetCDF
        /// </summary>
        NetCDF,
        /// <summary>
        /// HDF
        /// </summary>
        HDF,
        /// <summary>
        /// ASCII grid
        /// </summary>
        ASCII_Grid,
        /// <summary>
        /// Sufer grid
        /// </summary>
        Sufer_Grid,
        /// <summary>
        /// SYNOP
        /// </summary>
        SYNOP,
        /// <summary>
        /// METAR
        /// </summary>
        METAR,
        /// <summary>
        /// NOAA ISH dataset
        /// </summary>
        ISH,
        /// <summary>
        /// Lon/Lat stations
        /// </summary>
        LonLatStation,
        /// <summary>
        /// GRIB edition 1
        /// </summary>
        GRIB1,
        /// <summary>
        /// GRIB edition 2
        /// </summary>
        GRIB2,
        /// <summary>
        /// AWX - FY satellite data format
        /// </summary>
        AWX,
        /// <summary>
        /// HRIT/LRIT satellite data format
        /// </summary>
        HRIT,
        /// <summary>
        /// GeoTiff
        /// </summary>
        GeoTiff
    }
}
