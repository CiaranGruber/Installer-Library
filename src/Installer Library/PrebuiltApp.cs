using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerLibrary
{
    /// <summary>
    /// Contains a set of code used to create a prebuilt installer
    /// </summary>
    public class PrebuiltApp
    {
        /// <summary>
        /// Contains the list of executables to be installed
        /// </summary>
        public List<ExecutableData> Executables { get; set; }
        /// <summary>
        /// The location where the application is installed or is designed to be installed
        /// </summary>
        public string InstallLocation { get; set; }
        /// <summary>
        /// Whether the application has been installed for all users or not
        /// </summary>
        public bool IsGlobalInstall { get; set; }
        /// <summary>
        /// Whether the application is located in an app fodler or is a standalone executable
        /// </summary>
        public bool InAppFolder { get; set; }

        /// <summary>
        /// Creates an instance of a prebuilt installer with executables and an install location
        /// </summary>
        /// <param name="executables">The executables located within the installation</param>
        /// <param name="installLocation">The location where the program is installed</param>
        public PrebuiltApp(List<ExecutableData> executables, string installLocation) : this(executables, installLocation, false)
        {

        }

        /// <summary>
        /// Creates an instance of a prebuilt installer with executables, an install location and an option for all users
        /// </summary>
        /// <param name="executables">The executables located within the installation</param>
        /// <param name="installLocation">The location where the program is installed</param>
        /// <param name="allUsers">Whether the program has been installed for all users or not</param>
        public PrebuiltApp(List<ExecutableData> executables, string installLocation, bool allUsers)
        {
            Executables = executables;
            InstallLocation = installLocation;
            IsGlobalInstall = allUsers;
        }

        /// <summary>
        /// Installs the prebuilt application
        /// </summary>
        /// <param name="programLocation">The location where the program is currently located</param>
        public void InstallApp(string programLocation)
        {
            Installer.InstallApp(Executables, programLocation, InstallLocation, InAppFolder, IsGlobalInstall);
        }

        /// <summary>
        /// Uninstalls the prebuilt application removing all data
        /// </summary>
        public void UninstallApp()
        {
            Uninstaller.UninstallApp(Executables.Select(x => x.ShortcutName).ToList(), InstallLocation);
        }

        /// <summary>
        /// Uninstalls the prebuilt application saving data to a specified location
        /// </summary>
        /// <param name="saveLocation">The lcoation to save the application to</param>
        public void UninstallApp(string saveLocation)
        {
            Uninstaller.MoveApp(Executables.Select(x => x.ShortcutName).ToList(), InstallLocation, saveLocation);
        }
    }
}
