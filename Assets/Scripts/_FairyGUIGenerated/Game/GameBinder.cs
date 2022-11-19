/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;

namespace Game
{
    public class GameBinder
    {
        public static void BindAll()
        {
            UIObjectFactory.SetPackageItemExtension(UI_GameView.URL, typeof(UI_GameView));
            UIObjectFactory.SetPackageItemExtension(UI_MapView.URL, typeof(UI_MapView));
            UIObjectFactory.SetPackageItemExtension(UI_MainPage.URL, typeof(UI_MainPage));
        }
    }
}