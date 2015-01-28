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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/13/2009 10:26:47 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
//using MapWindow;
//using MapWindow.Main;
//using MapWindow.Data;
//using MapWindow.Drawing;
//using MapWindow.Geometries;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// EsriString
    /// </summary>
    public interface IEsriString
    {
     
        #region Methods

        /// <summary>
        /// Writes the value in the format that it would appear in within a prj file
        /// </summary>
        /// <returns>The a nested portion of the total esri string.</returns>
        string ToEsriString();

        /// <summary>
        /// This reads the string and attempts to parse the relavent values.
        /// </summary>
        /// <param name="esriString">The string to read</param>
        void ReadEsriString(string esriString);
        



        #endregion

        #region Properties



        #endregion



    }
}
