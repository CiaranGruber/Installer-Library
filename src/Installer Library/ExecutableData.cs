using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerLibrary
{
    /// <summary>
    /// Contains the data used for the executables
    /// </summary>
    public class ExecutableData
    {
        /// <summary>Gets or sets the shortcut name for the executable</summary>
        public string ShortcutName { get; set; }
        /// <summary>Gets or sets the executable path for the executable</summary>
        public string ExecutablePath { get; set; }
        /// <summary>Gets or sets whether the executable will be installed on the desktop</summary>
        public bool OnDesktop { get; set; }

        /// <summary>
        /// Initialises a new <c>ExecutableData</c> instance with default values
        /// </summary>
        public ExecutableData() : this("", "", false) { }

        /// <summary>
        /// Initialises a new <c>ExecutableData</c> instance with the specified parameters
        /// </summary>
        /// <param name="shortcutName">The name of the shortcut</param>
        /// <param name="executablePath">The relative path to the executable from the program path</param>
        /// <param name="onDesktop">Whether the shortcut will be found on the desktop</param>
        public ExecutableData(string shortcutName, string executablePath, bool onDesktop)
        {
            ShortcutName = shortcutName;
            ExecutablePath = executablePath;
            OnDesktop = onDesktop;
        }

        /// <summary>
        /// Initialises a new <c>ExecutableData</c> instance using an existing instance
        /// </summary>
        /// <param name="executable">The executable to copy the data from</param>
        public ExecutableData(ExecutableData executable) : this(executable.ShortcutName, executable.ExecutablePath, executable.OnDesktop) { }
    }
}
