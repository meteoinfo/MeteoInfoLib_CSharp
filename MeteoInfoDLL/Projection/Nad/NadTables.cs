//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/17/2009 4:52:04 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Collections.Generic;
using System.IO;
using System.Reflection;
namespace MeteoInfoC.Projections
{


    /// <summary>
    /// NadTables
    /// </summary>
    public class NadTables
    {
        #region Private Variables

        private readonly Dictionary<string, NadTable> _tables;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NadTables
        /// </summary>
        public NadTables()
        {
            _tables = new Dictionary<string, NadTable>();
            Assembly a = Assembly.GetExecutingAssembly();
            string[] names = a.GetManifestResourceNames();
           
            foreach (string s in names)
            {

                if(s.EndsWith(".lla"))
                {
                    Stream text = a.GetManifestResourceStream(s);
                    NadTable nt = new NadTable(text);
                    if(_tables.ContainsKey(nt.Name))continue;
                    _tables.Add(nt.Name, nt);                   
                }
            }

        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets an array of the lla tables that have been added as a resource
        /// </summary>
        public Dictionary<string, NadTable> Tables
        {
            get { return _tables; }
        }

        #endregion



    }
}
