/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Game
{
    public partial class UI_MainPage : GComponent
    {
        public UI_MapView m_MapView;
        public const string URL = "ui://wdu27ktim9z92";

        public static UI_MainPage CreateInstance()
        {
            return (UI_MainPage)UIPackage.CreateObject("Game", "MainPage");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_MapView = (UI_MapView)GetChildAt(0);
        }
    }
}