using UnityEngine;
using FairyGUI;

namespace FairyGUIArchitecture
{
    public abstract class ViewModel : IViewModel
    {
        public static ViewModel GetViewModel(ViewID viewID, GComponent view)
        {
            switch (viewID)
            {
                case ViewID.GameInit: return new GameInitViewModel(view);
            }
            return null;
        }

        public abstract void OnDestroy();
    }
}