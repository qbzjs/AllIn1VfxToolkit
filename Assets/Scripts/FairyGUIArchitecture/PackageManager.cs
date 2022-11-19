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
    }
}