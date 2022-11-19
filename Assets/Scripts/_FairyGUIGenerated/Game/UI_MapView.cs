/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Game
{
    public partial class UI_MapView : GComponent
    {
        public GGraph m_bg;
        public GLoader m_LeftMap;
        public GLoader m_RightMap;
        public const string URL = "ui://wdu27ktim9z91";

        public static UI_MapView CreateInstance()
        {
            return (UI_MapView)UIPackage.CreateObject("Game", "MapView");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_LeftMap = (GLoader)GetChildAt(1);
            m_RightMap = (GLoader)GetChildAt(2);
        }
    }
}