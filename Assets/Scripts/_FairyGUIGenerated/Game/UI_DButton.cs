/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace Game
{
    public partial class UI_DButton : GButton
    {
        public GImage m_up;
        public GImage m_down;
        public const string URL = "ui://wdu27ktis9uy6";

        public static UI_DButton CreateInstance()
        {
            return (UI_DButton)UIPackage.CreateObject("Game", "DButton");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_up = (GImage)GetChildAt(0);
            m_down = (GImage)GetChildAt(1);
        }
    }
}