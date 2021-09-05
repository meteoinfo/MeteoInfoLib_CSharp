using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Plugin
{
    /// <summary>
    /// Plugin manager interface
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Plugin name
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Plugin host application
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set plugin author
        /// </summary>
        string Author
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set plugin version
        /// </summary>
        string Version
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set plugin description
        /// </summary>
        string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Plugin load
        /// </summary>        
        void Load();

        /// <summary>
        /// Plugin unload
        /// </summary>
        void UnLoad();
    }
}
