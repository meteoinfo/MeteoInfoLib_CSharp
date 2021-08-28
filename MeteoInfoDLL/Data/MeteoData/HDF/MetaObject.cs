using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Meta object
    /// </summary>
    public class MetaObject
    {
        #region Variables        
        private List<string> _paraLines = new List<string>();
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lines">lines</param>
        /// <param name="objectName">object name</param>
        public MetaObject(List<string> lines, string objectName)
        {
            _paraLines = new List<string>();
            for (int i = 0; i < lines.Count; i++)
            {
                string aLine = lines[i].Trim();
                if (aLine == "OBJECT=" + objectName)
                {                    
                    while (aLine != "END_OBJECT=" + objectName)
                    {
                        i += 1;
                        if (i == lines.Count)
                            break;

                        aLine = lines[i].Trim();
                        _paraLines.Add(aLine);                        
                    }
                    break;
                }
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

        #endregion

        #region Methods       
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

