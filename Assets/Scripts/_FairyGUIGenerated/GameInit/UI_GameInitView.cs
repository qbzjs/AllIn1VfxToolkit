/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace GameInit
{
    public partial class UI_GameInitView : GComponent
    {
        public Controller m_Page;
        public GGraph m_bg;
        public UI_StartPage m_StartPage;
        public Transition m_StartPage_Hide;
        public Transition m_StartPage_Show;
        public const string URL = "ui://qha20o7xt2dr0";

        public static UI_GameInitView CreateInstance()
        {
            return (UI_GameInitView)UIPackage.CreateObject("GameInit", "GameInitView");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_Page = GetControllerAt(0);
            m_bg = (GGraph)GetChildAt(0);
            m_StartPage = (UI_StartPage)GetChildAt(1);
            m_StartPage_Hide = GetTransitionAt(0);
            m_StartPage_Show = GetTransitionAt(1);
        }
    }
}