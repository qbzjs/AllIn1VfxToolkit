using UnityEngine;
using FairyGUI;

namespace FairyGUIArchitecture
{
    public abstract class ViewModel : IViewModel
    {
        public static ViewModel GetViewModel(ViewManager.ViewID viewID, GComponent view)
        {
            switch (viewID)
            {
                case ViewManager.ViewID.GameInit: return new GameInitViewModel(view);
            }
            return null;
        }

        public abstract void OnDestroy();
    }
}