using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// NetCDF 4
    /// </summary>
    public static partial class NetCDF4
    {
        #region Const values
        /// <summary>
        /// Create mode
        /// </summary>
        public enum CreateMode
        {
            /// <summary>
            /// NC_NOWRITE
            /// </summary>
            NC_NOWRITE = 0,
            /// <summary>
            /// NC_WRITE
            /// </summary>
            NC_WRITE = 0x1,      // read & write
            /// <summary>
            /// NC_CLOBBER
            /// </summary>
            NC_CLOBBER = 0,
            /// <summary>
            /// NC_NOCLOBBER
            /// </summary>
            NC_NOCLOBBER = 0x4,  // Don't destroy existing file on create
            /// <summary>
            /// NC_FILL
            /// </summary>
            NC_FILL = 0,         // argument to ncsetfill to clear NC_NOFILL
            /// <summary>
            /// NC_NOFILL
            /// </summary>
            NC_NOFILL = 0x100,   // Don't fill data section an records
            /// <summary>
            /// NC_LOCK
            /// </summary>
            NC_LOCK = 0x400,     // Use locking if available
            /// <summary>
            /// NC_SHARE
            /// </summary>
            NC_SHARE = 0x800,   // Share updates, limit cacheing
        }

        /// <summary>
        /// NetCDF data type
        /// </summary>
        public enum NcType
        {
            /// <summary>
            /// NC_BYTE
            /// </summary>
            NC_BYTE = 1, // signed 1 byte integer
            /// <summary>
            /// NC_CHAR
            /// </summary>
            NC_CHAR = 2, // ISO/ASCII character
            /// <summary>
            /// NC_SHORT
            /// </summary>
            NC_SHORT = 3, // signed 2 byte integer
            /// <summary>
            /// NC_INT
            /// </summary>
            NC_INT = 4, // signed 4 byte integer
            /// <summary>
            /// NC_FLOAT
            /// </summary>
            NC_FLOAT = 5, // single precision floating point number
            /// <summary>
            /// NC_DOUBLE
            /// </summary>
            NC_DOUBLE = 6, // double precision floating point number            
        }

        /// <summary>
        /// NetCDF limits
        /// </summary>
        public enum NetCDF_limits
        {
            /// <summary>
            /// Maximum dimensions
            /// </summary>
            NC_MAX_DIMS = 10,  // max dimensions per file
            /// <summary>
            /// Maximum attribute number
            /// </summary>
            NC_MAX_ATTRS = 2000, // max global or per variable attributes
            /// <summary>
            /// Maximum variables per file
            /// </summary>
            NC_MAX_VARS = 2000, // max variables per file
            /// <summary>
            /// Maximum length of a name
            /// </summary>
            NC_MAX_NAME = 128, // max length of a name
            /// <summary>
            /// Maximum per variable dimensions
            /// </summary>
            NC_MAX_VAR_DIMS = 10, // max per variable dimensions
        }

        // 'size' argument to ncdimdef for an unlimited dimension
        /// <summary>
        /// Unlimited dimension identifer
        /// </summary>
        public const int NC_UNLIMITED = 0;

        // attribute id to put/get a global attribute
        /// <summary>
        /// Global attribute id
        /// </summary>
        public const int NC_GLOBAL = -1;

        #endregion

        //Declare extern functions
        #region File operation
        /// <summary>
        /// Open NetCDF data file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <param name="ncidp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_open(string path, int mode, out int ncidp);

        /// <summary>
        /// Create NetCDF data file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <param name="ncidp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_create(string path, int mode, out int ncidp);

        /// <summary>
        /// Close NetCDF data file
        /// </summary>
        /// <param name="ncidp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_close(int ncidp);

        /// <summary>
        /// nc_sync
        /// </summary>
        /// <param name="ncid"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_sync(int ncid);

        /// <summary>
        /// End defination
        /// </summary>
        /// <param name="ncid"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_enddef(int ncid);

        /// <summary>
        /// Redefination
        /// </summary>
        /// <param name="ncid"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_redef(int ncid);

        /// <summary>
        /// Error text
        /// </summary>
        /// <param name="ncerror"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern string nc_strerror(int ncerror);

        /// <summary>
        /// Define dimension
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="name"></param>
        /// <param name="len"></param>
        /// <param name="dimidp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_def_dim(int ncid, string name, int len, out int dimidp);

        /// <summary>
        /// Define variable
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="name"></param>
        /// <param name="xtype"></param>
        /// <param name="ndims"></param>
        /// <param name="dimids"></param>
        /// <param name="varidp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_def_var(int ncid, string name, int xtype, int ndims, int[] dimids, out int varidp);        

        #endregion

        #region Inquire
        
        /// <summary>
        /// Inquire
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="ndims"></param>
        /// <param name="nvars"></param>
        /// <param name="ngatts"></param>
        /// <param name="unlimdimid"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq(int ncid, out int ndims, out int nvars, out int ngatts, out int unlimdimid);
        
        /// <summary>
        /// Inquire variable
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="ndims"></param>
        /// <param name="dimids"></param>
        /// <param name="natts"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_var(int ncid, int varid, StringBuilder name, out int type, out int ndims, int[] dimids, out int natts);
       
        /// <summary>
        /// Inquire variable ids
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="nvars"></param>
        /// <param name="varids"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_varids(int ncid, out int nvars, int[] varids);

        /// <summary>
        /// Inquire variable type
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="xtypep"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_vartype(int ncid, int varid, out int xtypep);

        /// <summary>
        /// Inquire variable attributes
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="nattsp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_varnatts(int ncid, int varid, out int nattsp);

        /// <summary>
        /// Inquire variable id
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="name"></param>
        /// <param name="varidp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_varid(int ncid, string name, out int varidp);

        /// <summary>
        /// Inquire dimension number
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="ndims"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_ndims(int ncid, out int ndims);

        /// <summary>
        /// Inquire variable number
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="nvars"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_nvars(int ncid, out int nvars);

        /// <summary>
        /// Inquire variable name
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_varname(int ncid, int varid, StringBuilder name);

        /// <summary>
        /// Inquire variable dimension number
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="ndims"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_varndims(int ncid, int varid, out int ndims);

        /// <summary>
        /// Inquire variable dimension ids
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="dimids"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_vardimid(int ncid, int varid, int[] dimids);

        /// <summary>
        /// Inquire variable fill
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="no_fill"></param>
        /// <param name="fill_value"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_var_fill(int ncid, int varid, out int no_fill, out object fill_value);

        /// <summary>
        /// Inquire global attribute number
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="ngatts"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_natts(int ncid, out int ngatts);

        /// <summary>
        /// Inquire unlimited dimension id
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="unlimdimid"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_unlimdim(int ncid, out int unlimdimid);

        /// <summary>
        /// Inquire format
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_format(int ncid, out int format);

        /// <summary>
        /// Inquire attribute name
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="attnum"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_attname(int ncid, int varid, int attnum, StringBuilder name);

        /// <summary>
        /// Inquire attribute name
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_atttype(int ncid, int varid, StringBuilder name, out int type);

        /// <summary>
        /// Inquire attribute
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_att(int ncid, int varid, StringBuilder name, out int type, out int length);

        /// <summary>
        /// Inquire dimension
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="dimid"></param>
        /// <param name="name"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_dim(int ncid, int dimid, StringBuilder name, out int length);

        /// <summary>
        /// Inquire dimension name
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="dimid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_dimname(int ncid, int dimid, StringBuilder name);

        /// <summary>
        /// Inquire dimension id
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="name"></param>
        /// <param name="dimid"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_dimid(int ncid, string name, out int dimid);

        /// <summary>
        /// Inquire dimension length
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="dimid"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_inq_dimlen(int ncid, int dimid, out int length);   
        #endregion

        #region Read data
        /// <summary>
        /// Get attribute text
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_att_text(int ncid, int varid, string name, StringBuilder value);

        /// <summary>
        /// Get attribute schar
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_att_schar(int ncid, int varid, string name, sbyte[] data);

        /// <summary>
        /// Get attribute uchar
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_att_uchar(int ncid, int varid, string name, byte[] data);

        /// <summary>
        /// Get attribute short
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_att_short(int ncid, int varid, string name, short[] data);

        /// <summary>
        /// Get attribute int
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_att_int(int ncid, int varid, string name, int[] data);

        /// <summary>
        /// Get attribute float
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_att_float(int ncid, int varid, string name, float[] data);

        /// <summary>
        /// Get attribute double
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_att_double(int ncid, int varid, string name, double[] data);

        /// <summary>
        /// Get vara text
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_vara_text(int ncid, int varid, int[] start, int[] count, byte[] data);
        
        /// <summary>
        /// Get vara schar
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_vara_schar(int ncid, int varid, int[] start, int[] count, sbyte[] data);

        /// <summary>
        /// Get vara uchar
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_vara_uchar(int ncid, int varid, int[] start, int[] count, byte[] data); 
        
        /// <summary>
        /// Get vara short
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_vara_short(int ncid, int varid, int[] start, int[] count, short[] data);
        
        /// <summary>
        /// Get vara ubyte
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_vara_ubyte(int ncid, int varid, int[] start, int[] count, byte[] data);
        
        /// <summary>
        /// Get vara long
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_vara_long(int ncid, int varid, int[] start, int[] count, long[] data);
        
        /// <summary>
        /// Get vara int
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_vara_int(int ncid, int varid, int[] start, int[] count, int[] data);
        
        /// <summary>
        /// Get vara float
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_vara_float(int ncid, int varid, int[] start, int[] count, float[] data);
        
        /// <summary>
        /// Get vara double
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_vara_double(int ncid, int varid, int[] start, int[] count, double[] data);
        
        /// <summary>
        /// Get vara string
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_vara_string(int ncid, int varid, int[] start, int[] count, string[] data);

        /// <summary>
        /// Get variable string
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_var_string(int ncid, int varid, string[] data);

        /// <summary>
        /// Get variable text
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_var_text(int ncid, int varid, byte[] data);
        
        /// <summary>
        /// Get variable schar
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_var_schar(int ncid, int varid, sbyte[] data);

        /// <summary>
        /// Get variable short
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_var_short(int ncid, int varid, short[] data);

        /// <summary>
        /// Get variable int
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_var_int(int ncid, int varid, int[] data);

        /// <summary>
        /// Get variable long
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_var_long(int ncid, int varid, long[] data);

        /// <summary>
        /// Get variable float
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_var_float(int ncid, int varid, float[] data);

        /// <summary>
        /// Get variable double
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_var_double(int ncid, int varid, double[] data);

        /// <summary>
        /// Get variable byte
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_get_var_ubyte(int ncid, int varid, byte[] data);

        #endregion

        #region Write data
        /// <summary>
        /// Put attribute text
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="len"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_att_text(int ncid, int varid, string name, int len, string tp);
        
        /// <summary>
        /// Put attribute double
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="len"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_att_double(int ncid, int varid, string name, int type, int len, double[] tp);
        
        /// <summary>
        /// Put attribute int
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="len"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_att_int(int ncid, int varid, string name, int type, int len, int[] tp);
        
        /// <summary>
        /// Put attribute short
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="len"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_att_short(int ncid, int varid, string name, int type, int len, short[] tp);
       
        /// <summary>
        /// Put attribute float
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="len"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_att_float(int ncid, int varid, string name, int type, int len, float[] tp);
        
        /// <summary>
        /// Put attribute byte
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="len"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_att_byte(int ncid, int varid, string name, int type, int len, sbyte[] tp);                    

        /// <summary>
        /// Put variable double
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_var_double(int ncid, int varid, double[] dp);
        
        /// <summary>
        /// Put variable float
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_var_float(int ncid, int varid, float[] dp);
        
        /// <summary>
        /// Put variable short
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_var_short(int ncid, int varid, short[] dp);
        
        /// <summary>
        /// Put variable int
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_var_int(int ncid, int varid, int[] dp);
        
        /// <summary>
        /// Put variable long
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_var_long(int ncid, int varid, long[] dp);
        
        /// <summary>
        /// Put variable ubyte
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_var_ubyte(int ncid, int varid, byte[] dp);
        
        /// <summary>
        /// Put variable schar
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_var_schar(int ncid, int varid, sbyte[] dp);
        
        /// <summary>
        /// Put variable string
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_var_string(int ncid, int varid, string[] dp);

        /// <summary>
        /// Put vara double
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_vara_double(int ncid, int varid, int[] start, int[] count, double[] dp);
        
        /// <summary>
        /// Put vara float
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="fp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_vara_float(int ncid, int varid, int[] start, int[] count, float[] fp);
        
        /// <summary>
        /// Put vara short
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_vara_short(int ncid, int varid, int[] start, int[] count, short[] sp);
        
        /// <summary>
        /// Put vara int
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_vara_int(int ncid, int varid, int[] start, int[] count, int[] ip);
        
        /// <summary>
        /// Put vara long
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="lp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_vara_long(int ncid, int varid, int[] start, int[] count, long[] lp);
        
        /// <summary>
        /// Put vara ubyte
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="bp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_vara_ubyte(int ncid, int varid, int[] start, int[] count, byte[] bp);
        
        /// <summary>
        /// Put vara schar
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="cp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_vara_schar(int ncid, int varid, int[] start, int[] count, sbyte[] cp);
        
        /// <summary>
        /// Put vara string
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_vara_string(int ncid, int varid, int[] start, int[] count, string[] sp);

        /// <summary>
        /// put vara text
        /// </summary>
        /// <param name="ncid"></param>
        /// <param name="varid"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="cp"></param>
        /// <returns></returns>
        [DllImport("netcdf4.dll")]
        public static extern int nc_put_vara_text(int ncid, int varid, int[] start, int[] count, byte[] cp);
                
        #endregion
    }
}
