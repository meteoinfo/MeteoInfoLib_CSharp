using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MeteoInfoC.Data.MapData
{
    /// <summary>
    /// A class for controling the attribute Table related information for a shapefile.
    /// </summary>    
    public class AttributeTable 
    {
        /// <summary>
        /// Occurs after content has been loaded into the attribute data.
        /// </summary>
        public event EventHandler AttributesFilled;

        #region Variables

        private int _numRecords;
        private DateTime _updateDate;
        private int _headerLength;
        private int _recordLength;
        private int _numFields;
        private List<Field> _columns;
        private byte _fileType;
        private BinaryWriter _writer;
        private string _filename;
        private DataTable _dataTable;        

        // Constant for the size of a record
        private const int FileDescriptorSize = 32;
        private bool _attributesPopulated;
        private char[] _characterContent;
        private byte[] _byteContent;
        private long[] _offsets;
        private bool _hasDeletedRecords;
        private Stopwatch _dataRowWatch;
        private bool _loaded;
        //private bool _virtualMode;
        private List<int> _deletedRows;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an attribute Table with no file reference
        /// </summary>
        public AttributeTable()
        {
            Configure();
        }

        /// <summary>
        /// Creates a new AttributeTable with the specified filename, or opens
        /// an existing file with that name.
        /// </summary>
        /// <param name="filename"></param>
        public AttributeTable(string filename)
        {
            _dataRowWatch = new Stopwatch();
            _filename = filename;
            Configure();
            if (File.Exists(filename))
                Open(filename);
        }

        private void Configure()
        {
            _fileType = 0x03;            
            _dataTable = new DataTable();
            _columns = new List<Field>();
            _attributesPopulated = true; // only turn this false during an "open" method
            _deletedRows = new List<int>();
            _characterContent=new char[1];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads all the information from the file, including the vector shapes and the database component.
        /// </summary>
        public void Open(string filename)
        {
            _attributesPopulated = false; // we had a file, but have not read the dbf content into memory yet.
            _filename = Path.ChangeExtension(filename, ".dbf");
            _dataTable = new DataTable();
            if (!File.Exists(_filename))
            {
                _filename = Path.ChangeExtension(filename, ".DBF");
                if (!File.Exists(_filename))
                {
                    MessageBox.Show("The dbf file for this shapefile was not found.");
                    return;
                }
            }
            FileStream myStream = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read, 100000);
            BinaryReader myReader = new BinaryReader(myStream,Encoding.Default);
            ReadTableHeader(myReader); // based on the header, set up the fields information etc.

            //_hasDeletedRecords = false;

            FileInfo fi = new FileInfo(_filename);
            if (fi.Length == (_headerLength + 1) + _numRecords * _recordLength)
            {
                _hasDeletedRecords = false;
                // No deleted rows detected
                return;
            }
            _hasDeletedRecords = true;
            int count = 0;
            int row = 0;
            while (count < _numRecords)
            {
                if (myStream.ReadByte() == (byte)' ')
                {
                    count++;
                }
                else
                {
                    _deletedRows.Add(row);
                }
                row++;
                myStream.Seek(_recordLength - 1, SeekOrigin.Current);
            }
            myReader.Close();
        }

        private void Load()
        {
            FileStream myStream = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read, 100000);
            BinaryReader myReader = new BinaryReader(myStream, Encoding.Default);

            FileInfo fi = new FileInfo(_filename);

            // Encoding appears to be ASCII, not Unicode
            myStream.Seek(_headerLength + 1, SeekOrigin.Begin);
            if ((int)fi.Length == _headerLength)
            {
                // The file is empty, so we are done here
                return;
            }
            int length = (int)fi.Length - (_headerLength) - 1;
            _byteContent = myReader.ReadBytes(length);
            myReader.Close();
            //_characterContent = new char[length];            
            //Encoding.Default.GetChars(_byteContent, 0, length, _characterContent, 0);
            if (_hasDeletedRecords)
            {
                int recordCount = length / _recordLength;
                _offsets = new long[_numRecords];
                int j = 0; // undeleted index
                for (int i = 0; i <= recordCount; i++)
                {
                    //if (_characterContent[i * _recordLength] != '*')
                    //    _offsets[j] = i * _recordLength;
                    if ((char)(_byteContent[i * _recordLength]) != '*')
                        _offsets[j] = i * _recordLength;
                    j++;
                    if (j == _numRecords) break;
                }
            }
            _loaded = true;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>attribute table</returns>
        public AttributeTable Clone()
        {
            AttributeTable newAT = new AttributeTable();
            newAT = (AttributeTable)this.MemberwiseClone();

            //newAT.Table = new DataTable();
            //foreach (DataColumn aDC in _dataTable.Columns)
            //{
            //    DataColumn bDC = new DataColumn();
            //    bDC.ColumnName = aDC.ColumnName;
            //    bDC.DataType = aDC.DataType;
            //    newAT.Table.Columns.Add(bDC);
            //}

            return newAT;
        }

        /// <summary>
        /// This populates the Table with data from the file.
        /// </summary>
        /// <param name="numRows">In the event that the dbf file is not found, this indicates how many blank rows should exist in the attribute Table.</param>
        public void Fill(int numRows)
        {
            if (!_loaded) Load();
            _dataRowWatch = new Stopwatch();

            _dataTable.Rows.Clear(); // if we have already loaded data, clear the data.

            if (File.Exists(_filename) == false)
            {
                _numRecords = numRows;
                _dataTable.BeginLoadData();
                _dataTable.Columns.Add("FID", typeof (int));
                for (int row = 0; row < numRows; row++)
                {
                    DataRow dr = _dataTable.NewRow();
                    dr["FID"] = row;
                    _dataTable.Rows.Add(dr);
                }
                _dataTable.EndLoadData();
                return;
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            _dataTable.BeginLoadData();
            // Reading the Table elements as well as the shapes in a single progress loop.
            for (int row = 0; row < _numRecords; row++)
            {
                // --------- DATABASE --------- CurrentFeature = ReadTableRow(myReader);
                try
                {
                    //_dataTable.Rows.Add(ReadTableRowFromChars(row));
                    _dataTable.Rows.Add(ReadTableRowFromBytes(row));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    _dataTable.Rows.Add(_dataTable.NewRow());
                }                
            }            
            _dataTable.EndLoadData();
            sw.Stop();

            Debug.WriteLine("Load Time:" + sw.ElapsedMilliseconds + " Milliseconds");
            Debug.WriteLine("Conversion:" + _dataRowWatch.ElapsedMilliseconds + " Milliseconds");
            _attributesPopulated = true;
            OnAttributesFilled();
        }


        /// <summary>
        /// Attempts to save the file to the path specified by the Filename property.
        /// This should be the .shp extension.
        /// </summary>
        public void Save()
        {
            if (File.Exists(Filename)) File.Delete(Filename);
            UpdateSchema();
            string dbfFile = Path.ChangeExtension(Filename, ".dbf");
            _writer = new BinaryWriter(File.Open(dbfFile, FileMode.Create, FileAccess.Write, FileShare.None), Encoding.Default);
            WriteHeader(_writer);
            WriteTable();
            _writer.Close();
        }

        /// <summary>
        /// Saves this Table to the specified filename
        /// </summary>
        /// <param name="filename">The string filename to save to</param>
        /// <param name="overwrite">A boolean indicating whether or not to write over the file if it exists.</param>
        public void SaveAs(string filename, bool overwrite)
        {
            if (Filename == filename)
            {
                Save();
                return;
            }
            if (File.Exists(filename))
            {
                if (overwrite == false)
                {
                    if (
                        MessageBox.Show("The file " + filename + " already exists.  Do you wish to overwrite it?",
                                        "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                        DialogResult.No)
                        return;
                }
            }
            Filename = filename;
            Save();
        }


        private void UpdateSchema()
        {
            List<Field> tempColumns = new List<Field>();
            _recordLength = 1; // delete character
            _numRecords = Table.Rows.Count;
            _updateDate = DateTime.Now;
            _headerLength = FileDescriptorSize + FileDescriptorSize*Table.Columns.Count + 1;
            if (_columns == null) _columns = new List<Field>();
            // Delete any fields from the columns list that are no 
            // longer in the data Table.
            List<Field> removeFields = new List<Field>();
            foreach (Field fld in _columns)
            {
                if (Table.Columns.Contains(fld.ColumnName) == false)
                    removeFields.Add(fld);
                else
                    tempColumns.Add(fld);
            }
            foreach (Field field in removeFields)
            {
                _columns.Remove(field);
            }

            // Add new columns that exist in the data Table, but don't have a matching field yet.
            if (Table.Columns != null)
            {
                foreach (DataColumn dc in Table.Columns)
                {
                    if (ColumnNameExists(dc.ColumnName)) continue;
                    Field fld = dc as Field;
                    if (fld == null) fld = new Field(dc);
                            
                    tempColumns.Add(fld);
                }
            }

            _columns = tempColumns;

            // Recalculate the recordlength
            foreach (Field fld in Columns)
            {
                //_recordLength = _recordLength + fld.Length + 1;
                _recordLength = _recordLength + fld.Length;
            }
        }

        // Tests to see if the list of columns contains the specified name or not.
        private bool ColumnNameExists(string name)
        {
            foreach (Field fld in _columns)
            {
                if (fld.ColumnName == name) return true;
            }
            return false;
        }


        /// <summary>
        /// This appends the content of one datarow to a dBase file.
        /// </summary>
        /// <exception cref="ArgumentNullException">The columnValues parameter was null</exception>
        /// <exception cref="InvalidOperationException">Header records need to be written first.</exception>
        /// <exception cref="InvalidDataException">Table property of columnValues parameter cannot be null.</exception>
        public void WriteTable()
        {
            if (_dataTable == null) return;

            // _writer.Write((byte)0x20); // the deleted flag
            NumberConverter[] ncs = new NumberConverter[_columns.Count];
            for (int i = 0; i < _columns.Count; i++)
            {
                Field fld = _columns[i];
                ncs[i] = new NumberConverter(fld.Length, fld.DecimalCount);
            }
            for (int row = 0; row < _dataTable.Rows.Count; row++)
            {
                _writer.Write((byte) 0x20); // the deleted flag
                int len = _recordLength - 1;
                for (int fld = 0; fld < _columns.Count; fld++)
                {
                    string name = _columns[fld].ColumnName;
                    object columnValue = _dataTable.Rows[row][name];
                    if (columnValue == null || columnValue is DBNull)
                        WriteSpaces(_columns[fld].Length);
                    else if (columnValue is decimal)
                        _writer.Write(ncs[fld].ToChar((decimal) columnValue));
                    else if (columnValue is double)
                    {
                        //Write((double)columnValue, _columns[fld].Length, _columns[fld].DecimalCount);
                        char[] test = ncs[fld].ToChar((double) columnValue);
                        _writer.Write(test);
                    }
                    else if (columnValue is float)
                    {
                        //Write((float)columnValue, _columns[fld].Length, _columns[fld].DecimalCount);
                        Field currentField = _columns[fld];
                        if (currentField.TypeCharacter == 'F')
                        {
                            string val = ((float) columnValue).ToString();
                            Write(val, currentField.Length);
                        }
                        else
                        {
                            char[] test = ncs[fld].ToChar((float) columnValue);
                            _writer.Write(test);
                        }
                    }
                    else if (columnValue is int || columnValue is short || columnValue is long || columnValue is byte)
                        Write(Convert.ToInt64(columnValue), _columns[fld].Length, _columns[fld].DecimalCount);
                    else if (columnValue is bool)
                        Write((bool) columnValue);
                    else if (columnValue is string)
                    {
                        int length = _columns[fld].Length;
                        Write((string) columnValue, length);
                    }
                    else if (columnValue is DateTime)
                        WriteDate((DateTime) columnValue);
                    else
                        Write((string) columnValue, _columns[fld].Length);
                    len -= _columns[fld].Length;
                }
                // If, for some reason the column lengths don't add up to the total record length, fill with spaces.
                if (len > 0) WriteSpaces(len);
            }
        }

        /// <summary>
        /// Writes a number of spaces equal to numspaces
        /// </summary>
        /// <param name="numspaces">The integer number of spaces to write</param>
        public void WriteSpaces(int numspaces)
        {
            for (int I = 0; I < numspaces; I++)
            {
                _writer.Write(' ');
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length"></param>
        /// <param name="decimalCount"></param>
        public void Write(double number, int length, int decimalCount)
        {
            // write with 19 chars.
            string format = "{0:";
            for (int i = 0; i < decimalCount; i++)
            {
                if (i == 0)
                    format = format + "0.";
                format = format + "0";
            }
            format = format + "}";
            string str = String.Format(format, number);
            for (int i = 0; i < length - str.Length; i++)
                _writer.Write((byte) 0x20);
            foreach (char c in str)
                _writer.Write(c);

            // Attempting to store double as binary
            //_writer.Write(number);
        }

        /// <summary>
        /// Writes an integer so that it is formatted for dbf.  This is still buggy since it is possible to lose info here.
        /// </summary>
        /// <param name="number">The long value</param>
        /// <param name="length">The length of the field.</param>
        /// <param name="decimalCount">The number of digits after the decimal</param>
        public void Write(long number, int length, int decimalCount)
        {
            string str = number.ToString();
            if (str.Length > length)
                str = str.Substring(str.Length - length, length);
            for (int i = 0; i < length - str.Length; i++)
                _writer.Write((byte) 0x20);
            foreach (char c in str)
                _writer.Write(c);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length"></param>
        /// <param name="decimalCount"></param>
        public void Write(float number, int length, int decimalCount)
        {
			//string test = number.ToString(String.Format("{0:000000000.000000000}"));
            //_writer.Write(String.Format("{0:000000000.000000000}", number));
            //_writer.Write(number);

            string format = "{0:";
            for (int i = 0; i < decimalCount; i++)
            {
                if (i == 0)
                    format = format + "0.";
                format = format + "0";
            }
            format = format + "}";
            string str = String.Format(format, number);
            for (int i = 0; i < length - str.Length; i++)
                _writer.Write((byte)0x20);
            foreach (char c in str)
                _writer.Write(c);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        public void Write(string text, int length)
        {
            // ensure string is not too big
            text = text.PadRight(length, ' ');
            string dbaseString = text.Substring(0, length);

            // will extra chars get written??
            byte[] bytes = Encoding.Default.GetBytes(dbaseString.ToCharArray());
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i < length)
                {
                    _writer.Write(bytes[i]);
                }
            }
            //_writer.Write(bytes);
            //_writer.Write(Encoding.Default.GetBytes(text.ToCharArray()));
            //foreach (char c in dbaseString)
            //{                
            //    _writer.Write(c);
            //}

            int extraPadding = length - dbaseString.Length;
            for (int i = 0; i < extraPadding; i++)
                _writer.Write((byte) 0x20);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        public void WriteDate(DateTime date)
        {
            // YYYYMMDD format
            string test = date.ToString("yyyyMMdd");
            Write(test, 8);
            //_writer.Write(date.Year - 1900);
            //_writer.Write(date.Month);
            //_writer.Write(date.Day);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        public void Write(bool flag)
        {
            if (flag)
                _writer.Write("T");
            else _writer.Write("F");
        }


        /// <summary>
        /// Write the header data to the DBF file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteHeader(BinaryWriter writer)
        {
            // write the output file type.
            writer.Write(_fileType);

            writer.Write((byte) (_updateDate.Year - 1900));
            writer.Write((byte) _updateDate.Month);
            writer.Write((byte) _updateDate.Day);

            // write the number of records in the datafile.
            writer.Write(_numRecords);

            // write the length of the header structure.
            writer.Write((short) _headerLength); // 32 + 30 * numColumns

            // write the length of a record
            writer.Write((short) _recordLength);

            // write the reserved bytes in the header
            for (int i = 0; i < 20; i++)
                writer.Write((byte) 0);

            // write all of the header records
            Field CurrentField;
            for (int i = 0; i < _columns.Count; i++)
            {
                CurrentField = _columns[i];
                // write the field name
                byte[] bytes = Encoding.Default.GetBytes(CurrentField.ColumnName.ToCharArray());
                for (int j = 0; j < 11; j++)
                {
                    if (bytes.Length > j)
                        writer.Write(bytes[j]);
                    else
                        writer.Write((byte)0);                    
                }
                //for (int j = 0; j < 11; j++)
                //{
                //    if (CurrentField.ColumnName.Length > j)
                //        writer.Write((byte) CurrentField.ColumnName[j]);
                //    else writer.Write((byte) 0);
                //}

                // write the field type
                writer.Write(CurrentField.TypeCharacter);

                // write the field data address, offset from the start of the record.
                writer.Write(0);

                // write the length of the field.
                writer.Write(CurrentField.Length);

                // write the decimal count.
                writer.Write(CurrentField.DecimalCount);

                // write the reserved bytes.
                for (int j = 0; j < 14; j++) writer.Write((byte) 0);
            }
            // write the end of the field definitions marker
            writer.Write((byte) 0x0D);
        }

        private static bool IsNull(char[] CharArray)
        {
            for (int I = 0; I < CharArray.Length; I++)
            {
                if (CharArray[I] != ' ' && CharArray[I] != '\0') return false;
            }
            return true;
        }

        /// <summary>
        /// Read a single dbase record
        /// </summary>
        /// <returns>Returns an IFeature with information appropriate for the current row in the Table</returns>
        private DataRow ReadTableRowFromChars(int currentRow)
        {
            DataRow result = _dataTable.NewRow();

            long start;
            if (_hasDeletedRecords == false)
                start = currentRow*_recordLength;
            else
                start = _offsets[currentRow];

            for (int col = 0; col < _dataTable.Columns.Count; col++)
            {
                // find the length of the field.
                Field CurrentField = _columns[col];


                // find the field type
                char tempFieldType = CurrentField.TypeCharacter;

                // read the data.

                char[] cBuffer = new char[CurrentField.Length];
                //byte[] bBuffer = new byte[CurrentField.Length];
                Array.Copy(_characterContent, start, cBuffer, 0, CurrentField.Length);
                //Array.Copy(_characterContent, start, bBuffer, 0, CurrentField.Length);
                start += CurrentField.Length;

                object tempObject = DBNull.Value;
                if (IsNull(cBuffer)) continue;


                switch (tempFieldType)
                {
                    case 'L': // logical data type, one character (T,t,F,f,Y,y,N,n)

                        char tempChar = cBuffer[0];
                        if ((tempChar == 'T') || (tempChar == 't') || (tempChar == 'Y') || (tempChar == 'y'))
                            tempObject = true;
                        else tempObject = false;
                        break;

                    case 'C': // character record.

                        tempObject = new string(cBuffer).Trim().Replace("\0", ""); //.ToCharArray();
                        //tempObject = System.Text.Encoding.Default.GetString(bBuffer); 
                        break;
                    case 'T':
                        throw new NotSupportedException();

                    case 'D': // date data type.

                        //char[] ebuffer = new char[8];
                        //ebuffer = _reader.ReadChars(8);

                        //else
                        //{

                        string tempString = new string(cBuffer, 0, 4);
                        int year;
                        if (int.TryParse(tempString, out year) == false) break;
                        int month;
                        tempString = new string(cBuffer, 4, 2);
                        if (int.TryParse(tempString, out month) == false) break;
                        int day;
                        tempString = new string(cBuffer, 6, 2);
                        if (int.TryParse(tempString, out day) == false) break;


                        tempObject = new DateTime(year, month, day);

                        // }
                        break;
                    case 'F':
                    case 'B':
                    case 'N': // number - ESRI uses N for doubles and floats

                        string tempStr = new string(cBuffer);
                        tempObject = DBNull.Value;
                        Type t = CurrentField.DataType;
                        if (t == typeof (byte))
                        {
                            byte temp;
                            if (byte.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                // It is possible to store values larger than 255 with
                                // three characters.  Therefore, we may have to upgrade the 
                                // numeric type for the entire field to short.
                                short upTest;
                                if (short.TryParse(tempStr.Trim(), out upTest))
                                {
                                    // Since we were successful, we should upgrade the field to storing short values instead of byte values.
                                    UpgradeColumn(CurrentField, typeof (short), currentRow, col, _dataTable);
                                    tempObject = upTest;
                                }
                                else
                                {
                                    UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                                    tempObject = tempStr;
                                }
                            }
                        }
                        else if (t == typeof (short))
                        {
                            short temp;
                            if (short.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                int upTest;
                                if (int.TryParse(tempStr.Trim(), out upTest))
                                {
                                    UpgradeColumn(CurrentField, typeof(int), currentRow, col, _dataTable);
                                    tempObject = upTest;
                                }
                                else
                                {
                                    UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                                    tempObject = tempStr;
                                }
                            }
                        }
                        else if (t == typeof (int))
                        {
                            int temp;
                            if (int.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                long upTest;
                                if (long.TryParse(tempStr.Trim(), out upTest))
                                {
                                    UpgradeColumn(CurrentField, typeof(long), currentRow, col, _dataTable);
                                    tempObject = upTest;
                                }
                                else
                                {
                                    UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                                    tempObject = tempStr;
                                }
                            }
                        }
                        else if (t == typeof (long))
                        {
                            long temp;
                            if (long.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                                tempObject = tempStr;
                            }
                        }
                        else if (t == typeof (float))
                        {
                            float temp;
                            if (float.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                double upTest;
                                if (double.TryParse(tempStr.Trim(), out upTest))
                                {
                                    UpgradeColumn(CurrentField, typeof(double), currentRow, col, _dataTable);
                                    tempObject = upTest;
                                }
                                else
                                {
                                    UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                                    tempObject = tempStr;
                                }
                            }
                        }
                        else if (t == typeof (double))
                        {
                            double temp;
                            if (double.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                decimal upTest;
                                if (decimal.TryParse(tempStr.Trim(), out upTest))
                                {
                                    UpgradeColumn(CurrentField, typeof(decimal), currentRow, col, _dataTable);
                                    tempObject = upTest;
                                }
                                else
                                {
                                    UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                                    tempObject = tempStr;
                                }
                            }
                        }
                        else if (t == typeof (decimal))
                        {
                            decimal temp;
                            if (decimal.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                                tempObject = tempStr;
                            }
                        }


                        break;

                    default:
                        throw new NotSupportedException("Do not know how to parse Field type " + tempFieldType);
                }
                // j++;

                result[CurrentField.ColumnName] = tempObject;
            }

            return result;
        }

        private DataRow ReadTableRowFromBytes(int currentRow)
        {
            DataRow result = _dataTable.NewRow();

            long start;
            if (_hasDeletedRecords == false)
                start = currentRow * _recordLength;
            else
                start = _offsets[currentRow];

            for (int col = 0; col < _dataTable.Columns.Count; col++)
            {
                // find the length of the field.
                Field CurrentField = _columns[col];


                // find the field type
                char tempFieldType = CurrentField.TypeCharacter;

                // read the data.
                //char[] cBuffer = new char[CurrentField.Length];
                byte[] cBuffer = new byte[CurrentField.Length];
                Array.Copy(_byteContent, start, cBuffer, 0, CurrentField.Length);
                //Array.Copy(_characterContent, start, bBuffer, 0, CurrentField.Length);
                start += CurrentField.Length;

                object tempObject = DBNull.Value;
                //if (IsNull(cBuffer)) continue;


                switch (tempFieldType)
                {
                    case 'L': // logical data type, one character (T,t,F,f,Y,y,N,n)

                        char tempChar = (char)cBuffer[0];
                        if ((tempChar == 'T') || (tempChar == 't') || (tempChar == 'Y') || (tempChar == 'y'))
                            tempObject = true;
                        else tempObject = false;
                        break;

                    case 'C': // character record.

                        //tempObject = new string(cBuffer).Trim().Replace("\0", ""); //.ToCharArray();
                        tempObject = System.Text.Encoding.Default.GetString(cBuffer).Trim(); 
                        break;
                    case 'T':
                        throw new NotSupportedException();

                    case 'D': // date data type.

                        //char[] ebuffer = new char[8];
                        //ebuffer = _reader.ReadChars(8);

                        //else
                        //{

                        string tempString = Encoding.Default.GetString(cBuffer, 0, 4);
                        int year;
                        if (int.TryParse(tempString, out year) == false) break;
                        int month;
                        tempString = Encoding.Default.GetString(cBuffer, 4, 2);
                        if (int.TryParse(tempString, out month) == false) break;
                        int day;
                        tempString = Encoding.Default.GetString(cBuffer, 6, 2);
                        if (int.TryParse(tempString, out day) == false) break;


                        tempObject = new DateTime(year, month, day);

                        // }
                        break;
                    case 'F':
                    case 'B':
                    case 'N': // number - ESRI uses N for doubles and floats

                        string tempStr = Encoding.Default.GetString(cBuffer);
                        tempObject = DBNull.Value;
                        Type t = CurrentField.DataType;
                        if (t == typeof(byte))
                        {
                            byte temp;
                            if (byte.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                // It is possible to store values larger than 255 with
                                // three characters.  Therefore, we may have to upgrade the 
                                // numeric type for the entire field to short.
                                short upTest;
                                if (short.TryParse(tempStr.Trim(), out upTest))
                                {
                                    // Since we were successful, we should upgrade the field to storing short values instead of byte values.
                                    UpgradeColumn(CurrentField, typeof(short), currentRow, col, _dataTable);
                                    tempObject = upTest;
                                }
                                else
                                {
                                    UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                                    tempObject = tempStr;
                                }
                            }
                        }
                        else if (t == typeof(short))
                        {
                            short temp;
                            if (short.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            //else
                            //{
                            //    int upTest;
                            //    if (int.TryParse(tempStr.Trim(), out upTest))
                            //    {
                            //        UpgradeColumn(CurrentField, typeof(int), currentRow, col, _dataTable);
                            //        tempObject = upTest;
                            //    }
                            //    else
                            //    {
                            //        UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                            //        tempObject = tempStr;
                            //    }
                            //}
                        }
                        else if (t == typeof(int))
                        {
                            int temp;
                            if (int.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                long upTest;
                                if (long.TryParse(tempStr.Trim(), out upTest))
                                {
                                    UpgradeColumn(CurrentField, typeof(long), currentRow, col, _dataTable);
                                    tempObject = upTest;
                                }
                                //else
                                //{
                                //    UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                                //    tempObject = tempStr;
                                //}
                            }
                        }
                        else if (t == typeof(long))
                        {
                            long temp;
                            if (long.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            //else
                            //{
                            //    UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                            //    tempObject = tempStr;
                            //}
                        }
                        else if (t == typeof(float))
                        {
                            float temp;
                            if (float.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                double upTest;
                                if (double.TryParse(tempStr.Trim(), out upTest))
                                {
                                    UpgradeColumn(CurrentField, typeof(double), currentRow, col, _dataTable);
                                    tempObject = upTest;
                                }
                                //else
                                //{
                                //    UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                                //    tempObject = tempStr;
                                //}
                            }
                        }
                        else if (t == typeof(double))
                        {
                            double temp;
                            if (double.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            //else
                            //{
                            //    decimal upTest;
                            //    if (decimal.TryParse(tempStr.Trim(), out upTest))
                            //    {
                            //        UpgradeColumn(CurrentField, typeof(decimal), currentRow, col, _dataTable);
                            //        tempObject = upTest;
                            //    }
                            //    else
                            //    {
                            //        UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                            //        tempObject = tempStr;
                            //    }
                            //}
                        }
                        else if (t == typeof(decimal))
                        {
                            decimal temp;
                            if (decimal.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            //else
                            //{
                            //    UpgradeColumn(CurrentField, typeof(string), currentRow, col, _dataTable);
                            //    tempObject = tempStr;
                            //}
                        }


                        break;

                    default:
                        throw new NotSupportedException("Do not know how to parse Field type " + tempFieldType);
                }
                // j++;

                result[CurrentField.ColumnName] = tempObject;
            }

            return result;
        }


        /// <summary>
        /// Read the header data from the DBF file.
        /// </summary>
        /// <param name="reader">BinaryReader containing the header.</param>
        private void ReadTableHeader(BinaryReader reader)
        {
            // type of reader.
            _fileType = reader.ReadByte();
            if (_fileType != 0x03)
                throw new NotSupportedException("Unsupported DBF reader Type " + _fileType);

            // parse the update date information.
            int year = reader.ReadByte();
            int month = reader.ReadByte();
            int day = reader.ReadByte();
            _updateDate = new DateTime(year + 1900, month, day);

            // read the number of records.
            _numRecords = reader.ReadInt32();

            // read the length of the header structure.
            _headerLength = reader.ReadInt16();

            // read the length of a record
            _recordLength = reader.ReadInt16();

            // skip the reserved bytes in the header.
            //in.skipBytes(20);
            reader.ReadBytes(20);
            // calculate the number of Fields in the header

            _numFields = (_headerLength - FileDescriptorSize - 1)/FileDescriptorSize;

            // _numFields = (_headerLength - FileDescriptorSize) / FileDescriptorSize;

            _columns = new List<Field>();

            for (int i = 0; i < _numFields; i++)
            {
                // read the field name				
                //char[] buffer = reader.ReadChars(11);
                //string name = new string(buffer);

                byte[] buffer = new byte[11];
                buffer = reader.ReadBytes(11);//如果不这样做，就会在后面的读取中，造成读取移位，出错。 
                string name = System.Text.Encoding.Default.GetString(buffer); 


                int nullPoint = name.IndexOf((char) 0);
                if (nullPoint != -1)
                    name = name.Substring(0, nullPoint);


                // read the field type
                char Code = (char) reader.ReadByte();

                // read the field data address, offset from the start of the record.
                int dataAddress = reader.ReadInt32();

                // read the field length in bytes
                byte tempLength = reader.ReadByte();


                // read the field decimal count in bytes
                byte decimalcount = reader.ReadByte();

                // read the reserved bytes.
                //reader.skipBytes(14);
                reader.ReadBytes(14);
                int j = 1;
                string tempName = name;
                while (_dataTable.Columns.Contains(tempName))
                {
                    tempName = name + j;
                    j++;
                }
                name = tempName;
                Field myField = new Field(name, Code, tempLength, decimalcount);
                myField.DataAddress = dataAddress; // not sure what this does yet

                _columns.Add(myField); // Store fields accessible by an index
                _dataTable.Columns.Add(myField);
            }

            // Last byte is a marker for the end of the field definitions.
            reader.ReadBytes(1);
        }



        /// <summary>
        /// This systematically copies all the existing values to a new data column with the same properties,
        /// but with a new data type.  Values that cannot convert will be set to null.
        /// </summary>
        /// <param name="oldDataColumn">The old data column to update</param>
        /// <param name="newDataType">The new data type that the column should become</param>
        /// <param name="currentRow">The row up to which values should be changed for</param>
        /// <param name="columnIndex">The column index of the field being changed</param>
        /// <param name="table"> The Table to apply this strategy to.</param>
        /// <returns>An integer list showing the index values of the rows where the conversion failed.</returns>
        public List<int> UpgradeColumn(Field oldDataColumn, Type newDataType, int currentRow, int columnIndex, DataTable table)
        {
            List<int> failureList = new List<int>();
            object[] newValues = new object[table.Rows.Count];
            string name = oldDataColumn.ColumnName;
            Field dc = new Field(oldDataColumn.ColumnName, newDataType);
            dc.Length = oldDataColumn.Length;
            dc.DecimalCount = oldDataColumn.DecimalCount;
            for (int row = 0; row < currentRow; row++)
            {
                try
                {
                    if (table.Rows[row][name] is DBNull)
                        newValues[row] = null;
                    else
                    {
                        object obj = _dataTable.Rows[row][name];

                        object newObj = Convert.ChangeType(obj, newDataType);
                        newValues[row] = newObj;
                    }
                }
                catch
                {
                    failureList.Add(row);
                }
            }

            int ord = oldDataColumn.Ordinal;            
            table.Columns.Remove(oldDataColumn);            
            table.Columns.Add(dc);
            dc.SetOrdinal(ord);
            _columns[columnIndex] = dc;
            for (int row = 0; row < currentRow; row++)
            {
                if (newValues[row] == null)
                    table.Rows[row][name] = DBNull.Value;
                else
                    table.Rows[row][name] = newValues[row];
            }
            return failureList;
        }

        /// <summary>
        /// Get data table
        /// </summary>
        /// <returns>data table</returns>
        public DataTable GetDataTable()
        {
            return _dataTable;
        }
        #endregion

        #region Properties

        /// <summary>
        /// gets or sets whether the Attributes have been populated.  If data was "opened" from a file,
        /// and a query is made to the DataTable while _attributesPopulated is false, then
        /// a Fill method will be called automatically
        /// </summary>
        public bool AttributesPopulated
        {
            get { return _attributesPopulated; }
            set { _attributesPopulated = value; }
        }


        /// <summary>
        /// The byte length of the header
        /// </summary>
        public int HeaderLength
        {
            get { return _headerLength; }
        }


        /// <summary>
        /// The columns
        /// </summary>
        public List<Field> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        /// <summary>
        /// The file type
        /// </summary>
        public byte FileType
        {
            get { return _fileType; }
        }

        /// <summary>
        /// The filename of the dbf file
        /// </summary>
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// Number of records
        /// </summary>
        public int NumRecords
        {
            get { return _numRecords; }
        }                

        /// <summary>
        /// The byte length of each record
        /// </summary>
        public int RecordLength
        {
            get { return _recordLength; }
        }

        /// <summary>
        /// DataSet
        /// </summary>
        public DataTable Table
        {
            get
            {
                if (_attributesPopulated == false)
                    Fill(_numRecords);
                return _dataTable;
            }
            set { _dataTable = value; }
        }


        /// <summary>
        /// Last date written to 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return _updateDate; }
        }

        /// <summary>
        /// Fires the AttributesFilled event
        /// </summary>
        protected virtual void OnAttributesFilled()
        {
            if (AttributesFilled != null) AttributesFilled(this, new EventArgs());
        }

        #endregion

        #region IDataPageRetriever Members

        /// <summary>
        /// Reads just the content requested in order to satisfy the paging ability of VirtualMode for the DataGridView
        /// </summary>
        /// <param name="lowerPageBondary"></param>
        /// <param name="rowsPerPage"></param>
        /// <returns></returns>
        public DataTable SupplyPageOfData(int lowerPageBondary, int rowsPerPage)
        {
            FileStream myStream = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read, 100000);
            BinaryReader myReader = new BinaryReader(myStream);

            FileInfo fi = new FileInfo(_filename);

            // Encoding appears to be ASCII, not Unicode
            myStream.Seek(_headerLength + 1, SeekOrigin.Begin);
            if ((int)fi.Length == _headerLength)
            {
                // The file is empty, so we are done here
                return null;
            }
            int maxRawRow = (int)((fi.Length - (HeaderLength + 1))/_recordLength);
            int strt = GetFileIndex(lowerPageBondary);
            int end = GetFileIndex(lowerPageBondary + rowsPerPage);
            int rawRows = end - strt;
            int length = rawRows*_recordLength;
            long offset = strt*_recordLength;

            
            myStream.Seek(offset, SeekOrigin.Current);
            byte[] byteContent = myReader.ReadBytes(length);
            if (byteContent.Length < length)
            {
                length = byteContent.Length;
                rawRows = length / _recordLength;
            }
            myReader.Close();
            char[] characterContent = new char[length];
            Encoding.Default.GetChars(byteContent, 0, length, characterContent, 0);
            DataTable result = new DataTable();

            foreach (Field field in _columns)
            {
                result.Columns.Add(new Field(field.ColumnName, field.TypeCharacter, field.Length, field.DecimalCount));
            }

            int start = 0;
            for (int row = lowerPageBondary; row < lowerPageBondary + rowsPerPage; row++)
            {
                if(row > maxRawRow) break;
                result.Rows.Add(ReadTableRow(GetFileIndex(row)-strt, start, characterContent, result));
                start += _recordLength;
            }
            return result;
        }

        /// <summary>
        /// Accounts for deleted rows and returns the index as it appears in the file
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private int GetFileIndex(int rowIndex)
        {
            int count = 0;
            if (_deletedRows == null) return rowIndex;
            foreach (int row in _deletedRows)
            {
                if (row <= rowIndex + count)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return rowIndex + count;
        }
       

        /// <summary>
        /// saves a single row to the data source.
        /// </summary>
        /// <param name="index">the integer row (or FID) index</param>
        /// <param name="values">The object array holding the new values to store.</param>
        public virtual void Edit(int index, Dictionary<string, object> values)
        {
            int rawRow = GetFileIndex(index);
            NumberConverter[] ncs = new NumberConverter[_columns.Count];
            for (int i = 0; i < _columns.Count; i++)
            {
                Field fld = _columns[i];
                ncs[i] = new NumberConverter(fld.Length, fld.DecimalCount);
            }
            //overridden in sub-classes
            FileStream myStream = new FileStream(_filename, FileMode.Open, FileAccess.Write, FileShare.Write, 100000);
            _writer = new BinaryWriter(myStream);
            myStream.Seek(_headerLength + _recordLength * rawRow, SeekOrigin.Begin);
            _writer.Write((byte)0x20); // the deleted flag
            int len = _recordLength - 1;
            for (int fld = 0; fld < _columns.Count; fld++)
            {
                string name = _columns[fld].ColumnName;
                object columnValue = values[name];
                if (columnValue == null || columnValue is DBNull)
                    WriteSpaces(_columns[fld].Length);
                else if (columnValue is decimal)
                    _writer.Write(ncs[fld].ToChar((decimal)columnValue));
                else if (columnValue is double)
                {
                    //Write((double)columnValue, _columns[fld].Length, _columns[fld].DecimalCount);
                    char[] test = ncs[fld].ToChar((double)columnValue);
                    _writer.Write(test);
                }
                else if (columnValue is float)
                {
                    //Write((float)columnValue, _columns[fld].Length, _columns[fld].DecimalCount);
                    Field currentField = _columns[fld];
                    if (currentField.TypeCharacter == 'F')
                    {
                        string val = ((float)columnValue).ToString();
                        Write(val, currentField.Length);
                    }
                    else
                    {
                        char[] test = ncs[fld].ToChar((float)columnValue);
                        _writer.Write(test);
                    }
                }
                else if (columnValue is int || columnValue is short || columnValue is long || columnValue is byte)
                    Write(Convert.ToInt64(columnValue), _columns[fld].Length, _columns[fld].DecimalCount);
                else if (columnValue is bool)
                    Write((bool)columnValue);
                else if (columnValue is string)
                {
                    int length = _columns[fld].Length;
                    Write((string)columnValue, length);
                }
                else if (columnValue is DateTime)
                    WriteDate((DateTime)columnValue);
                else
                    Write((string)columnValue, _columns[fld].Length);
                len -= _columns[fld].Length;
            }
            // If, for some reason the column lengths don't add up to the total record length, fill with spaces.
            if (len > 0) WriteSpaces(len);
            _writer.Flush();
            _writer.Close();
        }


       
        /// <summary>
        /// Read a single dbase record
        /// </summary>
        /// <returns>Returns an IFeature with information appropriate for the current row in the Table</returns>
        private DataRow ReadTableRow(int currentRow, long start, char[] characterContent, DataTable table)
        {
            DataRow result = table.NewRow();
            for (int col = 0; col < table.Columns.Count; col++)
            {
                // find the length of the field.
                Field currentField = table.Columns[col] as Field;
                if (currentField == null)
                {
                    // somehow the field is not a valid Field
                    return result;
                }

                // find the field type
                char tempFieldType = currentField.TypeCharacter;

                // read the data.

                char[] cBuffer = new char[currentField.Length];
                long len;
                if(start + currentField.Length > characterContent.Length)
                {
                    len = characterContent.Length - start;
                }
                else
                {
                    len = currentField.Length;
                }
                if (len < 0) return result;
                Array.Copy(characterContent, start, cBuffer, 0, len);
                start += currentField.Length;

                object tempObject = DBNull.Value;
                if (IsNull(cBuffer)) continue;


                switch (tempFieldType)
                {
                    case 'L': // logical data type, one character (T,t,F,f,Y,y,N,n)

                        char tempChar = cBuffer[0];
                        if ((tempChar == 'T') || (tempChar == 't') || (tempChar == 'Y') || (tempChar == 'y'))
                            tempObject = true;
                        else tempObject = false;
                        break;

                    case 'C': // character record.

                        tempObject = new string(cBuffer).Trim().Replace("\0", ""); //.ToCharArray();
                        break;
                    case 'T':
                        throw new NotSupportedException();

                    case 'D': // date data type.
                        string tempString = new string(cBuffer, 0, 4);
                        int year;
                        if (int.TryParse(tempString, out year) == false) break;
                        int month;
                        tempString = new string(cBuffer, 4, 2);
                        if (int.TryParse(tempString, out month) == false) break;
                        int day;
                        tempString = new string(cBuffer, 6, 2);
                        if (int.TryParse(tempString, out day) == false) break;
                        tempObject = new DateTime(year, month, day);

                        // }
                        break;
                    case 'F':
                    case 'B':
                    case 'N': // number - ESRI uses N for doubles and floats

                        string tempStr = new string(cBuffer);
                        tempObject = DBNull.Value;
                        Type t = currentField.DataType;
                        if (t == typeof (byte))
                        {
                            byte temp;
                            if (byte.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                // It is possible to store values larger than 255 with
                                // three characters.  Therefore, we may have to upgrade the 
                                // numeric type for the entire field to short.
                                short upTest;
                                if (short.TryParse(tempStr.Trim(), out upTest))
                                {
                                    // Since we were successful, we should upgrade the field to storing short values instead of byte values.
                                    UpgradeColumn(currentField, typeof (short), currentRow, col, table);
                                    tempObject = upTest;
                                }
                                else
                                {
                                    UpgradeColumn(currentField, typeof (string), currentRow, col, table);
                                    tempObject = tempStr;
                                }
                            }
                        }
                        else if (t == typeof (short))
                        {
                            short temp;
                            if (short.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                int upTest;
                                if (int.TryParse(tempStr.Trim(), out upTest))
                                {
                                    UpgradeColumn(currentField, typeof (int), currentRow, col, table);
                                    tempObject = upTest;
                                }
                                else
                                {
                                    UpgradeColumn(currentField, typeof (string), currentRow, col, table);
                                    tempObject = tempStr;
                                }
                            }
                        }
                        else if (t == typeof (int))
                        {
                            int temp;
                            if (int.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                long upTest;
                                if (long.TryParse(tempStr.Trim(), out upTest))
                                {
                                    UpgradeColumn(currentField, typeof (long), currentRow, col, table);
                                    tempObject = upTest;
                                }
                                else
                                {
                                    UpgradeColumn(currentField, typeof (string), currentRow, col, table);
                                    tempObject = tempStr;
                                }
                            }
                        }
                        else if (t == typeof (long))
                        {
                            long temp;
                            if (long.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                UpgradeColumn(currentField, typeof (string), currentRow, col, table);
                                tempObject = tempStr;
                            }
                        }
                        else if (t == typeof (float))
                        {
                            float temp;
                            if (float.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                double upTest;
                                if (double.TryParse(tempStr.Trim(), out upTest))
                                {
                                    UpgradeColumn(currentField, typeof (double), currentRow, col, table);
                                    tempObject = upTest;
                                }
                                else
                                {
                                    UpgradeColumn(currentField, typeof (string), currentRow, col, table);
                                    tempObject = tempStr;
                                }
                            }
                        }
                        else if (t == typeof (double))
                        {
                            double temp;
                            if (double.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            //else
                            //{
                            //    decimal upTest;
                            //    if (decimal.TryParse(tempStr.Trim(), out upTest))
                            //    {
                            //        UpgradeColumn(currentField, typeof (decimal), currentRow, col, table);
                            //        tempObject = upTest;
                            //    }
                            //    else
                            //    {
                            //        UpgradeColumn(currentField, typeof (string), currentRow, col, table);
                            //        tempObject = tempStr;
                            //    }
                            //}
                        }
                        else if (t == typeof (decimal))
                        {
                            decimal temp;
                            if (decimal.TryParse(tempStr.Trim(), out temp))
                                tempObject = temp;
                            else
                            {
                                UpgradeColumn(currentField, typeof (string), currentRow, col, table);
                                tempObject = tempStr;
                            }
                        }


                        break;

                    default:
                        throw new NotSupportedException("Do not know how to parse Field type " + tempFieldType);
                }
                // j++;

                result[currentField.ColumnName] = tempObject;
            }

            return result;
        }


        #endregion
    }
}