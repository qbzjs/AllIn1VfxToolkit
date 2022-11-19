using FairyGUI;

namespace FairyGUIArchitecture
{
    public static class PackageManager
    {
        /// <summary>
        /// Binds all the packages published from FairyGUI.
        /// </summary>
        public static void BindAllPackages()
        {
            Common.CommonBinder.BindAll();
            GameInit.GameInitBinder.BindAll();
            Game.GameBinder.BindAll();
        }

        /// <summary>
        /// Add a UI package from a path relative to Unity Resources path.
        /// </summary>
        /// <param name="packageName">The package name.</param>
        /// <param name="path">The path relative to Unity Resources path..</param>
        public static void AddPackage(string packageName, string path = "FairyGUI/")
        {
            UIPackage.AddPackage($"{path}{packageName}");
        }

        /// <summary>
        /// Removes a package from the Unity project.
        /// </summary>
        /// <param name="packageName">The package name.</param>
        public static void RemovePackage(string packageName)
        {
            UIPackage.RemovePackage(packageName);
        }
    }
}