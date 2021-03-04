using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shell32;
using IWshRuntimeLibrary;

namespace InstallerLibrary
{
    /// <summary>
    /// Contains helper methods used to install to select paths
    /// </summary>
    public static class Installer
    {
        /// <summary>
        /// Installs an app using the full process
        /// </summary>
        /// <param name="executables">The list of executables installed in the program</param>
        /// <param name="programFolder">The folder where the program is situated</param>
        /// <param name="installFolder">The folder where the program is to be installed</param>
        /// <param name="containedInAnAppFolder">Whether the executable is contained in an app folder or is standalone</param>
        /// <param name="isAdmin">Whether the user wishes to act as admin or not</param>
        public static void InstallApp(List<ExecutableData> executables, string programFolder, string installFolder, bool containedInAnAppFolder, bool isAdmin)
        {
            // Change shortcut name to include standard .lnk extension
            executables.ForEach(x => x.ShortcutName = Path.GetFileNameWithoutExtension(x.ShortcutName) + ".lnk");

            // Move files to project folder
            Directory.CreateDirectory(installFolder);
            if (containedInAnAppFolder)
            {
                SharedApp.MoveDirectory(programFolder, installFolder);
            }
            else
            {
                foreach (ExecutableData executable in executables)
                {
                    System.IO.File.Move(Path.Combine(programFolder, executable.ExecutablePath), Path.Combine(installFolder, executable.ExecutablePath));
                }
            }

            // Create shortcuts
            IWshShell_Class wsh = new IWshShell_Class();
            foreach (ExecutableData executable in executables)
            {
                IWshShortcut shortcut, desktopShortcut = null;
                if (isAdmin)
                {
                    // Create standard shortcut
                    shortcut = wsh.CreateShortcut(Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.CommonPrograms), executable.ShortcutName)) as IWshShortcut;

                    // Create desktop shortcut
                    if (executable.OnDesktop)
                    {
                        desktopShortcut = wsh.CreateShortcut(Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.CommonDesktopDirectory), executable.ShortcutName)) as IWshShortcut;
                    }
                }
                else
                {
                    // Create standard shortcuts
                    shortcut = wsh.CreateShortcut(Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.Programs), executable.ShortcutName)) as IWshShortcut;

                    // Create desktop shortcut
                    if (executable.OnDesktop)
                    {
                        desktopShortcut = wsh.CreateShortcut(Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.Desktop), executable.ShortcutName)) as IWshShortcut;
                    }
                }
                shortcut.WorkingDirectory = installFolder;
                shortcut.TargetPath = Path.Combine(installFolder, Path.Combine(installFolder, executable.ExecutablePath));
                shortcut.Save();

                // Desktop shortcuts will be created if they are required
                if (executable.OnDesktop)
                {
                    desktopShortcut.WorkingDirectory = installFolder;
                    desktopShortcut.TargetPath = Path.Combine(installFolder, Path.Combine(installFolder, executable.ExecutablePath));
                    desktopShortcut.Save();
                }
            }
        }
    }
}
