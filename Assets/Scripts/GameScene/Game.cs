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
            // TODO : Design UI
            // CreateView();

            _snakeGame = new SnakeGame(new SnakeGameParameters
            {
                Dimension = Dimension.DimensionTwo,
                Size = new Vector4Int(5, 5)
            });

            // TODO : create a space based on _snakeGame.Size
            // TODO : - need to zoom camera appropriately
            // TODO : - 1D and 2D needa be orthographic, 3D and 4D needa be perspective

            // TODO : show snake head based on current state
            // TODO : lerp snake head based on predicted state

            // TODO : Get user input (keyboard publish event / touch input publish event)
        }
    }
}
