/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace GameInit
{
    public partial class UI_StartPage : GComponent
    {
        public GTextField m_Title;
        public GButton m_PlayButton;
        public GButton m_ThemeIcon;
        public GButton m_SettingsButton;
        public GImage m_IconWireframe;
        public const string URL = "ui://qha20o7xm9z92";

        public static UI_StartPage CreateInstance()
        {
            return (UI_StartPage)UIPackage.CreateObject("GameInit", "StartPage");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_Title = (GTextField)GetChildAt(0);
            m_PlayButton = (GButton)GetChildAt(1);
            m_ThemeIcon = (GButton)GetChildAt(2);
            m_SettingsButton = (GButton)GetChildAt(3);
            m_IconWireframe = (GImage)GetChildAt(4);
        }
    }
}