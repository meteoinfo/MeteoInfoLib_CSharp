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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:36:37 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

#pragma warning disable 1591
using MeteoInfoC.Projections.GeographicCategories;
using System.Reflection;
namespace MeteoInfoC.Projections
{


    /// <summary>
    /// GeographicSystems
    /// </summary>
    public class GeographicSystems
    {
        #region Private Variables

        private string[] _names;

        public readonly Africa Africa;
        public readonly Antarctica Antarctica;
        public readonly Asia Asia;
        public readonly Australia Australia;
        public readonly CountySystems CountySystems;
        public readonly Europe Europe;
        public readonly NorthAmerica NorthAmerica;
        public readonly Oceans Oceans;
        public readonly SolarSystem SolarSystem;
        public readonly SouthAmerica SouthAmerica;
        public readonly SpheroidBased SpheroidBased;
        public readonly World World;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeographicSystems
        /// </summary>
        public GeographicSystems()
        {
            Africa = new Africa();
            Antarctica = new Antarctica();
            Asia = new Asia();
            Australia = new Australia();
            CountySystems = new CountySystems();
            Europe = new Europe();
            NorthAmerica = new NorthAmerica();
            Oceans = new Oceans();
            SolarSystem = new SolarSystem();
            SouthAmerica = new SouthAmerica();
            SpheroidBased = new SpheroidBased();
            World = new World();
        }


        private void AddNames()
        {
            FieldInfo[] flds = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            _names = new string[flds.Length];
            for (int i = 0; i < flds.Length; i++)
            {
                _names[i] = flds[i].Name;
            }
        }

        /// <summary>
        /// Gets an array of all the names of the coordinate system categories 
        /// in this collection of systems.
        /// </summary>
        public string[] Names
        {
            get
            {
                if (_names == null)
                {
                    AddNames();
                }
                return _names;
            }
        }

        /// <summary>
        /// Given the string name, this will return the specified coordinate category
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CoordinateSystemCategory GetCategory(string name)
        {
            FieldInfo[] flds = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < flds.Length; i++)
            {
                if (flds[i].Name == name)
                {
                    return flds[i].GetValue(this) as CoordinateSystemCategory;
                }
            }
            return null;
        }


        #endregion

      


    }
}
#pragma warning restore 1591