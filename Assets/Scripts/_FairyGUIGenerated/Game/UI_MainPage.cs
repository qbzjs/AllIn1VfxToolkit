/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Game
{
    public partial class UI_MainPage : GComponent
    {
        public UI_MapView m_MapView;
        public GGraph m_MainMapContainer;
        public GLoader m_MainMap;
        public UI_DPad m_LeftDPad;
        public UI_DPad m_RightDPad;
        public const string URL = "ui://wdu27ktim9z92";

        public static UI_MainPage CreateInstance()
        {
            return (UI_MainPage)UIPackage.CreateObject("Game", "MainPage");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_MapView = (UI_MapView)GetChildAt(0);
            m_MainMapContainer = (GGraph)GetChildAt(1);
            m_MainMap = (GLoader)GetChildAt(2);
            m_LeftDPad = (UI_DPad)GetChildAt(3);
            m_RightDPad = (UI_DPad)GetChildAt(4);
        }
    }
}