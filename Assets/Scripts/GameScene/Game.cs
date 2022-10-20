using System.Collections;
using UnityEngine;
using FairyGUIArchitecture;
using Snake4D;

namespace GameScene
{
    public class Game : SceneInit
    {
        SnakeGame _snakeGame = null;

        private void Start()
        {
            CreateView();

            _snakeGame = new SnakeGame(new SnakeGameParameters
            {
                GameMode = GameMode.DimensionTwo,
                Size = new Vector4Int(5, 5)
            });
        }
    }
}
