/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Game
{
    public partial class UI_DPad : GComponent
    {
        public GGraph m_bg;
        public UI_DButton m_up;
        public UI_DButton m_down;
        public UI_DButton m_left;
        public UI_DButton m_right;
        public const string URL = "ui://wdu27ktis9uy5";

        public static UI_DPad CreateInstance()
        {
            return (UI_DPad)UIPackage.CreateObject("Game", "DPad");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_bg = (GGraph)GetChildAt(0);
            m_up = (UI_DButton)GetChildAt(1);
            m_down = (UI_DButton)GetChildAt(2);
            m_left = (UI_DButton)GetChildAt(3);
            m_right = (UI_DButton)GetChildAt(4);
        }
    }
}