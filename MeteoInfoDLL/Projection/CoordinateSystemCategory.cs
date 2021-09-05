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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:43:46 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
namespace MeteoInfoC.Projections
{


    /// <summary>
    /// CoordinateSystem
    /// </summary>
    public class CoordinateSystemCategory
    {
        #region Private Variables

        private readonly string[] _names;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CoordinateSystem
        /// </summary>
        public CoordinateSystemCategory()
        {
           
            Type t = GetType();
            FieldInfo[] fields = t.GetFields();
            _names = new string[fields.Length];
            for(int i = 0; i < fields.Length; i++)
            {
                _names[i] = fields[i].Name; 
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the specified projection given the specified name.
        /// </summary>
        /// <param name="name">The string name of the projection to obtain information for</param>
        /// <returns></returns>
        public ProjectionInfo GetProjection(string name)
        {
            Type t = GetType();
            FieldInfo[] fields = t.GetFields();
            foreach (FieldInfo info in fields)
            {
                if(info.Name == name)
                {
                    return info.GetValue(this) as ProjectionInfo;
                }
            }
            return null;
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of names of all the members on this object
        /// </summary>
        public string[] Names
        {
            get { return _names; }
        }

        /// <summary>
        /// Obtains all the members of this category, building a single
        /// array of the projection info classes.  This returns the
        /// original classes, not a copy.
        /// </summary>
        /// <returns>The array of projection info classes</returns>
        public ProjectionInfo[] ToArray()
        {
            Type t = GetType();
            FieldInfo[] fields = t.GetFields();
            ProjectionInfo[] result = new ProjectionInfo[fields.Length];
            for (int i = 0; i < fields.Length; i++ )
            {
                result[i] = fields[i].GetValue(this) as ProjectionInfo;
            }
            return null;
        }

        #endregion



    }
}
