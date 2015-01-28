using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB parameter
    /// </summary>
    public class Parameter
    {
        #region Variables
        /// <summary>
        /// Parameter number
        /// </summary>
        public int Number;
        /// <summary>
        /// Variable name
        /// </summary>
        public string Name;
        /// <summary>
        /// Level type
        /// </summary>
        public int LevelType;
        /// <summary>
        /// Level number
        /// </summary>
        public int LevelNum;
        /// <summary>
        /// Level list
        /// </summary>
        public List<double> Levels;
        /// <summary>
        /// Units
        /// </summary>
        public string Units;
        /// <summary>
        /// Description
        /// </summary>
        public string Description;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Parameter()
        {            
            Name = "Undef";            
            Levels = new List<double>();
            Units = "Undef";
            Description = "Undef";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aNum">Parameter number</param>
        /// <param name="aName">name</param>
        /// <param name="aDesc">description</param>
        /// <param name="aUnit">units</param>
        public Parameter(int aNum, String aName, String aDesc, String aUnit)
        {
            Number = aNum;
            Name = aName;
            Description = aDesc;
            Units = aUnit;
            LevelNum = 0;
            Levels = new List<double>();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>parameter object</returns>
        public object Clone()
        {
            Parameter aPar = new Parameter();
            aPar.Number = Number;
            aPar.Name = Name;
            aPar.Units = Units;
            aPar.Description = Description;
            //aPar.LevelNum = LevelNum;
            aPar.LevelType = LevelType;
            //aPar.Levels = Levels;

            return aPar;
        }

        /// <summary>
        /// Judge if two parameter are equal
        /// </summary>
        /// <param name="aVar">variable</param>
        /// <returns>if equal</returns>
        public bool Equals(Parameter aVar)
        {
            if (Name != aVar.Name) return false;
            if (Number != aVar.Number) return false;
            if (Description != aVar.Description) return false;
            if (Units != aVar.Units) return false;

            return true;
        }

        /// <summary>
        /// Judge if two parameter are totally equal
        /// </summary>
        /// <param name="aVar">variable</param>
        /// <returns>if equal</returns>
        public bool TEquals(Parameter aVar)
        {
            if (Name != aVar.Name) return false;
            if (Number != aVar.Number) return false;
            if (Description != aVar.Description) return false;
            if (Units != aVar.Units) return false;
            if (LevelType != aVar.LevelType) return false;

            return true;
        }

        /// <summary>
        /// Add a level
        /// </summary>
        /// <param name="levelValue">level value</param>
        public void AddLevel(float levelValue)
        {
            if (!Levels.Contains(levelValue))
            {
                Levels.Add(levelValue);
                LevelNum = Levels.Count;
            }
        }

        /// <summary>
        /// Get true level number
        /// </summary>
        /// <returns>true level number</returns>
        public int GetTrueLevelNumber()
        {
            if (LevelNum == 0)
                return 1;
            else
                return LevelNum;
        }

        #endregion

    }
}
