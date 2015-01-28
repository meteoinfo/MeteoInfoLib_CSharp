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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/11/2009 4:50:16 PM
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
    /// IProjectionCategory
    /// </summary>
    public interface IProjectionCategory
    {
     
       
        #region Properties

        /// <summary>
        /// Gets or sets the main category for this projection
        /// </summary>
        string MainCategory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the category for this projection
        /// </summary>
        string Category
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string name
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the proj4 string that defines this projection
        /// </summary>
        string Proj4String
        {
            get;
            set;
        }

        #endregion



    }
}
