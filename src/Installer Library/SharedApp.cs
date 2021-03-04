using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace InstallerLibrary
{
    /// <summary>
    /// Contains code used in the general application
    /// </summary>
    static class SharedApp
    {
        /// <summary>
        /// Tests whether the application is running as an admin
        /// </summary>
        /// <returns>Whether the application is running as an admin or not</returns>
        public static bool RunningAsAdmin()
        {
            var currentIdentity = WindowsIdentity.GetCurrent();
            if (currentIdentity == null)
                return false;

            return new WindowsPrincipal(currentIdentity).IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Test if path is admin protected
        /// </summary>
        /// <param name="path">The path to check whether the program has create/delete permissions</param>
        /// <returns>A boolean value as to whether the path is admin protected or not</returns>
        public static bool IsAdminProtected(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            FileSystemRights[] AccessRights = { FileSystemRights.CreateFiles, FileSystemRights.Delete };
            try
            {
                AuthorizationRuleCollection rules = Directory.GetAccessControl(path).GetAccessRules(true, true, typeof(SecurityIdentifier));
                WindowsIdentity identity = WindowsIdentity.GetCurrent();

                foreach (FileSystemAccessRule rule in rules)
                {
                    if (identity.Groups.Contains(rule.IdentityReference))
                    {
                        foreach (FileSystemRights AccessRight in AccessRights)
                        {
                            if ((AccessRight & rule.FileSystemRights) == AccessRight)
                            {
                                if (rule.AccessControlType == AccessControlType.Allow)
                                    return true;
                            }
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Moves a directory to the specified location
        /// </summary>
        /// <param name="sourceDirectory">The source directory to copy from</param>
        /// <param name="finalDirectory">The directory to copy to</param>
        public static void MoveDirectory(string sourceDirectory, string finalDirectory)
        {
            // Create new location
            Directory.CreateDirectory(finalDirectory);

            // Move contents to new location
            string[] folderContents = Directory.EnumerateFileSystemEntries(sourceDirectory).ToArray();
            foreach (string content in folderContents)
            {
                // If content is a directory
                if (File.GetAttributes(content).HasFlag(FileAttributes.Directory))
                {
                    MoveDirectory(content, Path.Combine(finalDirectory, Path.GetFileName(content)));
                }
                // If content is a file
                else
                {
                    File.Move(content, Path.Combine(finalDirectory, Path.GetFileName(content)));
                }
            }

            // Delete old location
            Directory.Delete(sourceDirectory);
        }
    }
}
