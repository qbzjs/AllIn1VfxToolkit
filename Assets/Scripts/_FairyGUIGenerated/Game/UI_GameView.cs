/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Game
{
    public partial class UI_GameView : GComponent
    {
        public UI_MainPage m_MainPage;
        public const string URL = "ui://wdu27ktim9z90";

        public static UI_GameView CreateInstance()
        {
            return (UI_GameView)UIPackage.CreateObject("Game", "GameView");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_MainPage = (UI_MainPage)GetChildAt(0);
        }
    }
}