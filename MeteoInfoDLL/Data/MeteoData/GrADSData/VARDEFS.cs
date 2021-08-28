using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GrADS variable dimension set
    /// </summary>
    public class VARDEFS
    {
        #region Variables
        private List<Variable> _vars = new List<Variable>();

        #endregion

        #region Properties
        /// <summary>
        /// Get Number of variables
        /// </summary>
        public int VNum
        {
            get { return _vars.Count; }
        }

        /// <summary>
        /// Get or set Variables
        /// </summary>
        public List<Variable> Vars
        {
            get { return _vars; }
            set { _vars = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Add var
        /// </summary>
        /// <param name="aVar"></param>
        public void AddVar(Variable aVar)
        {
            _vars.Add(aVar);
        }

        #endregion
    }
}
