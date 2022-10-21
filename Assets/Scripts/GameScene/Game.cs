using System.Collections;
using UnityEngine;
using FairyGUIArchitecture;
using Snake4D;

namespace GameScene
{
    public class Game : SceneInit
    {
        [SerializeField] float _updateInterval;
        [SerializeField] GameObject _cube;

        SnakeGame _snakeGame = null;
        WaitForSeconds _waitForUpdateInterval;
        IEnumerator _updateCoroutine = null;

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
            UpdateVisuals();

            _waitForUpdateInterval = new WaitForSeconds(_updateInterval);
            _updateCoroutine = UpdateCoroutine();
            StartCoroutine(_updateCoroutine);
        }

        private IEnumerator UpdateCoroutine()
        {
            while (true) // TODO : use a better boolean
            {
                yield return _waitForUpdateInterval;

                _snakeGame.UpdateState();
                UpdateVisuals();
            }
        }

        private void UpdateVisuals()
        {
            UpdateSnakeHead();
        }

        private void UpdateSnakeHead()
        {
            _cube.transform.localPosition = _snakeGame.GetSnakeHeadWorldSpacePosition();
        }
    }
}
