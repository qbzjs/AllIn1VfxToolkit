using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using FairyGUIArchitecture;
using GameInit;

public class GameInitViewModel : ViewModel
{
    UI_GameInitView _gameInitView;

    public GameInitViewModel(GComponent view, Dictionary<string, object> viewParameters)
    : base(view, viewParameters)
    {
        _gameInitView = view as UI_GameInitView;
    }
}