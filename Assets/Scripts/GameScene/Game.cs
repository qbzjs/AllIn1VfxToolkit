using System.Collections;
using UnityEngine;
using FairyGUIArchitecture;
using Snake4D;

namespace GameScene
{
    public class Game : SceneInit
    {
        [SerializeField] GameObject _cube;

        SnakeGame _snakeGame = null;

        private void Start()
        {
            // TODO : Design UI
            // CreateView();

            // TODO : The below should be in GameModel : Model, Model : IModel

            _snakeGame = new SnakeGame(new SnakeGameParameters
            {
                Dimension = Dimension.DimensionTwo,
                Size = new Vector4Int(10, 10, 10, 10)
            });

            // TODO : create a space based on _snakeGame.Size
            // TODO : - need to zoom camera appropriately
            // TODO : - 1D and 2D needa be orthographic, 3D and 4D needa be perspective

            // TODO : show snake head based on current state
            // TODO : lerp snake head based on predicted state

            // TODO : Get user input (keyboard publish event / touch input publish event)

            // ==========
            _cube.transform.localPosition = _snakeGame.GetSnakeHeadWorldSpacePosition();
        }
    }
}
