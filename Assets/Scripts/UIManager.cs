using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SceneInit
{
    [SerializeField] Texture _leftMapTexture;
    [SerializeField] Texture _rightMapTexture;

    private void Awake()
    {
        _viewParameters = new Dictionary<string, object>()
        {
            {"leftMapTexture", _leftMapTexture},
            {"rightMapTexture", _rightMapTexture}
        };
    }
}
