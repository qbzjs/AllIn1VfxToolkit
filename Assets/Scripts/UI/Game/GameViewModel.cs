using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using FairyGUIArchitecture;
using Game;

public class GameViewModel : ViewModel
{
    UI_GameView _gameView;

    public GameViewModel(GComponent view, Dictionary<string, object> viewParameters)
    : base(view, viewParameters)
    {
        _gameView = view as UI_GameView;
    }

    public override void OnDestroy()
    {
        // TODO : Unsubscribe from message hub subscriptions
    }
}