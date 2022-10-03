using UnityEngine;
using FairyGUI;
using FairyGUIArchitecture;
using GameInit;

public class GameInitViewModel : ViewModel
{
    UI_GameInitView _gameInitView;

    public GameInitViewModel(GComponent view)
    {
        _gameInitView = view as UI_GameInitView;
    }

    public override void OnDestroy()
    {
        // TODO : Unsubscribe from message hub subscriptions
    }
}