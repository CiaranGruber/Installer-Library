using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerLibrary
{
    /// <summary>
    /// Contains code used for the uninstaller
    /// </summary>
    public static class Uninstaller
    {
        /// <summary>
        /// Uninstalls an app and removes all data
        /// </summary>
        /// <param name="shortcuts">The shortcuts that are to be removed</param>
        /// <param name="installationLocation">The location of the installation</param>
        public static void UninstallApp(List<string> shortcuts, string installationLocation)
        {
            // Use move app method to reduce errors - delete created directory
            MoveApp(shortcuts, installationLocation, Path.Combine(Path.GetTempPath(), "Directory Deletion"));
            Directory.Delete(Path.Combine(Path.GetTempPath(), "Directory Deletion"));
        }

        /// <summary>
        /// Uninstalls shortcuts and moves program data to a specified location
        /// </summary>
        /// <param name="shortcuts">The name of the shortcuts to be removed</param>
        /// <param name="installationLocation">The location of the installation</param>
        /// <param name="saveLocation">The save location to save to - if null, program will be uninstalled completely</param>
        public static void MoveApp(List<string> shortcuts, string installationLocation, string saveLocation)
        {
            string[] shortcut_directories = { Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory),
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms),
                Environment.GetFolderPath(Environment.SpecialFolder.Programs),
            };

            for (int i = 0; i < shortcuts.Count; ++i)
            {
                if (Path.GetExtension(shortcuts[i]) != ".lnk")
                {
                    shortcuts[i] += ".lnk";
                }
            }

            // Create save directory
            Directory.CreateDirectory(saveLocation);

            // Move file
            if (File.GetAttributes(installationLocation).HasFlag(FileAttributes.Directory))
            {
                SharedApp.MoveDirectory(installationLocation, saveLocation);
            }
            else
            {
                File.Move(installationLocation, saveLocation);
            }

            // Remove all existing shortcuts 
            foreach (string shortcut in shortcuts)
            {
                foreach (string directory in shortcut_directories)
                {
                    if (File.Exists(Path.Combine(directory, shortcut)))
                    {
                        File.Delete(Path.Combine(directory, shortcut));
                    }
                }
            }
        }
    }
}
