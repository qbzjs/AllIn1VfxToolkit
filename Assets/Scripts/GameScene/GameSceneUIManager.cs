using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameSceneUIManager : SceneInit
    {
        [Header("GameScene UI Settings")]
        [SerializeField] Texture _leftMapTexture;
        [SerializeField] Texture _rightMapTexture;
        [SerializeField] Texture _mainMapTexture;

        private void Awake()
        {
            _viewParameters = new Dictionary<string, object>()
            {
                {"leftMapTexture", _leftMapTexture},
                {"rightMapTexture", _rightMapTexture},
                {"mainMapTexture", _mainMapTexture}
            };
        }
    }
}

