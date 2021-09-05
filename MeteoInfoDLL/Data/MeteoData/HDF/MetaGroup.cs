using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// HDF4 MetaGroup
    /// </summary>
    public class MetaGroup
    {
        #region Variables
        private List<string> _lines = new List<string>();
        private List<string> _paraLines = new List<string>();
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lines">lines</param>
        /// <param name="groupName">group name</param>
        public MetaGroup(List<string> lines, string groupName)
        {
            _lines = new List<string>();
            for (int i = 0; i < lines.Count; i++)
            {
                string aLine = lines[i].Trim();
                if (aLine == "GROUP=" + groupName)
                {
                    _lines.Add(aLine);
                    while (aLine != "END_GROUP=" + groupName)
                    {
                        i += 1;
                        if (i == lines.Count)
                            break;

                        aLine = lines[i].Trim();
                        _lines.Add(aLine);                        
                    }
                    break;
                }
            }

            _paraLines = new List<string>();
            for (int i = 1; i < _lines.Count; i++)
            {
                string aLine = _lines[i].Trim();                
                if (aLine.Substring(0, 5) == "GROUP")
                    break;

                _paraLines.Add(aLine);
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get parameter lines
        /// </summary>
        public List<string> ParaLines
        {
            get { return _paraLines; }
        }

        /// <summary>
        /// Get lines
        /// </summary>
        public List<string> Lines
        {
            get { return _lines; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get meta group
        /// </summary>
        /// <param name="groupName">group name</param>
        /// <returns>meta group</returns>
        public MetaGroup GetGroup(string groupName)
        {
            MetaGroup aMGroup = new MetaGroup(_lines, groupName);

            return aMGroup;
        }

        /// <summary>
        /// Get meta object
        /// </summary>
        /// <param name="objectName">object name</param>
        /// <returns>meta object</returns>
        public MetaObject GetObject(string objectName)
        {
            MetaObject aMObj = new MetaObject(_lines, objectName);

            return aMObj;
        }

        /// <summary>
        /// Get parameter string value
        /// </summary>
        /// <param name="paraName">parameter name</param>
        /// <returns>string value</returns>
        public string GetParaStr(string paraName)
        {
            string str = String.Empty;
            for (int i = 0; i < _paraLines.Count; i++)
            {
                string[] paras = _paraLines[i].Split('=');
                if (paras[0] == paraName)
                {
                    str = paras[1];
                    break;
                }
            }

            return str;
        }

        #endregion
    }
}
