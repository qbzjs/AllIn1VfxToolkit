/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace GameInit
{
    public partial class UI_GameInitView : GComponent
    {
        public GTextField m_Title;
        public const string URL = "ui://qha20o7xt2dr0";

        public static UI_GameInitView CreateInstance()
        {
            return (UI_GameInitView)UIPackage.CreateObject("GameInit", "GameInitView");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_Title = (GTextField)GetChildAt(0);
        }
    }
}