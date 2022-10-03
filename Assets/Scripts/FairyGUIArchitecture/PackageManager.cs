using FairyGUI;

namespace FairyGUIArchitecture
{
    public static class PackageManager
    {
        public static void BindAllPackages()
        {
            GameInit.GameInitBinder.BindAll();
        }

        /// <summary>
        /// Adds the published package in the Resources folder from FairyGUI into the Unity project.
        /// </summary>
        /// <param name="packageName">The package name.</param>
        /// <param name="path">The path from the Resource folder.</param>
        public static void AddPackage(string packageName, string path = "FairyGUI/")
        {
            UIPackage.AddPackage($"{path}{packageName}");
        }

        public static void RemovePackage(string packageName)
        {
            UIPackage.RemovePackage(packageName);
        }
    }
}