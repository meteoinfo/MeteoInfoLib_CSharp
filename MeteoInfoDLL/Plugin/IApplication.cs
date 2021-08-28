using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Map;
using MeteoInfoC.Legend;

namespace MeteoInfoC.Plugin
{
    /// <summary>
    /// Plugin host application
    /// </summary>
    public interface IApplication
    {
        #region Properties
        /// <summary>
        /// Get the MapView
        /// </summary>
        IMapView MapView
        {
            get;
        }

        /// <summary>
        /// Get the MapDocument
        /// </summary>
        LayersLegend MapDocument
        {
            get;
        }

        /// <summary>
        /// Get main menu strip
        /// </summary>
        MenuStrip MainMenuStrip
        {
            get;
        }

        /// <summary>
        /// Get main tool strip container
        /// </summary>
        ToolStripContainer MainToolStripContainer
        {
            get;
        }

        /// <summary>
        /// Get or set current tool
        /// </summary>
        ToolStripItem CurrentTool
        {
            get;
            set;
        }

        /// <summary>
        /// Get main ToolStripProgressBar
        /// </summary>
        ToolStripProgressBar MainToolStripProgressBar
        {
            get;
        }

        /// <summary>
        /// Get main ToolStripStatusLabel
        /// </summary>
        ToolStripStatusLabel MainToolStripStatusLabel
        {
            get;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Open project file
        /// </summary>
        /// <param name="fileName">Project file name</param>
        void OpenProjectFile(String fileName);

        #endregion
    }
}
