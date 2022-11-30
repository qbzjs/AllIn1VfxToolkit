using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using FairyGUIArchitecture;
using Game;

public class GameViewModel : ViewModel
{
    UI_MainPage _mainPage;

    public GameViewModel(GComponent view, Dictionary<string, object> viewParameters)
    : base(view, viewParameters)
    {
        UI_GameView gameView = view as UI_GameView;
        _mainPage = gameView.m_MainPage;

        Texture leftMapTexture = (Texture)viewParameters["leftMapTexture"];
        Texture rightMapTexture = (Texture)viewParameters["rightMapTexture"];
        Texture mainMapTexture = (Texture)viewParameters["mainMapTexture"];

        _mainPage.m_MapView.m_LeftMap.texture = new NTexture(leftMapTexture);
        _mainPage.m_MapView.m_RightMap.texture = new NTexture(rightMapTexture);
        _mainPage.m_MainMap.texture = new NTexture(mainMapTexture);

        _mainPage.m_LeftDPad.m_up.onRollOver.Add(() =>
            MessageHubSingleton.Instance.Publish<UserInputEvent>(new(
                Snake4D.UserInputType.Y_Positive
            ))
        );

        _mainPage.m_LeftDPad.m_down.onRollOver.Add(() =>
            MessageHubSingleton.Instance.Publish<UserInputEvent>(new(
                Snake4D.UserInputType.Y_Negative
            ))
        );

        _mainPage.m_LeftDPad.m_left.onRollOver.Add(() =>
            MessageHubSingleton.Instance.Publish<UserInputEvent>(new(
                Snake4D.UserInputType.X_Negative
            ))
        );

        _mainPage.m_LeftDPad.m_right.onRollOver.Add(() =>
            MessageHubSingleton.Instance.Publish<UserInputEvent>(new(
                Snake4D.UserInputType.X_Positive
            ))
        );

        _mainPage.m_RightDPad.m_up.onRollOver.Add(() =>
            MessageHubSingleton.Instance.Publish<UserInputEvent>(new(
                Snake4D.UserInputType.Z_Positive
            ))
        );

        _mainPage.m_RightDPad.m_down.onRollOver.Add(() =>
            MessageHubSingleton.Instance.Publish<UserInputEvent>(new(
                Snake4D.UserInputType.Z_Negative
            ))
        );

        _mainPage.m_RightDPad.m_left.onRollOver.Add(() =>
            MessageHubSingleton.Instance.Publish<UserInputEvent>(new(
                Snake4D.UserInputType.W_Negative
            ))
        );

        _mainPage.m_RightDPad.m_right.onRollOver.Add(() =>
            MessageHubSingleton.Instance.Publish<UserInputEvent>(new(
                Snake4D.UserInputType.W_Positive
            ))
        );
    }
}