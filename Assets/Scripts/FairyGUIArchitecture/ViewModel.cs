using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

namespace FairyGUIArchitecture
{
    public abstract class ViewModel : IViewModel
    {
        public ViewModel(GComponent view, Dictionary<string, object> viewParameters) { }

        public static ViewModel InitViewModel(ViewID viewID, GComponent view, Dictionary<string, object> viewParameters)
        {
            switch (viewID)
            {
                case ViewID.GameInit: return new GameInitViewModel(view, viewParameters);
                case ViewID.Game: return new GameViewModel(view, viewParameters);
            }
            return null;
        }

        public abstract void OnDestroy();
    }
}