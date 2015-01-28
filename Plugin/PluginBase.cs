using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Plugin
{
    /// <summary>
    /// Plugin base class
    /// </summary>
    public class PluginBase : IPlugin
    {
        #region Variables
        private IApplication _application = null;
        private String _name;
        private String _author;
        private String _version;
        private String _description;
        #endregion

        #region Constructor

        #endregion

        #region Properties
        /// <summary>
        /// Get or set application object
        /// </summary>
        public IApplication Application
        {
            get { return _application; }
            set { _application = value; }
        }

        /// <summary>
        /// Get or set plugin name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Get or set plugin author
        /// </summary>
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        /// <summary>
        /// Get or set plugin version
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// Get or set plugin description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load plugin
        /// </summary>
        public virtual void Load()
        {

        }

        /// <summary>
        /// Unload plugin
        /// </summary>
        public virtual void UnLoad()
        {

        }
        #endregion
    }
}
