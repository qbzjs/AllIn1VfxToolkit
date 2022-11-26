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

        Texture leftMapTexture = (Texture)viewParameters["leftMapTexture"];
        Texture rightMapTexture = (Texture)viewParameters["rightMapTexture"];
        Texture mainMapTexture = (Texture)viewParameters["mainMapTexture"];

        _gameView.m_MainPage.m_MapView.m_LeftMap.texture = new NTexture(leftMapTexture);
        _gameView.m_MainPage.m_MapView.m_RightMap.texture = new NTexture(rightMapTexture);
        _gameView.m_MainPage.m_MainMap.texture = new NTexture(mainMapTexture);
    }
}