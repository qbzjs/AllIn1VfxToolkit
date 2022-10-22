using System.Collections;
using UnityEngine;
using FairyGUIArchitecture;
using Snake4D;

namespace GameScene
{
    public class Game : SceneInit
    {
        [SerializeField] float _updateInterval;
        [SerializeField] bool _enableLerp;
        [SerializeField] GameObject _cube;

        [Header("Snake Game Parameters")]
        [SerializeField] bool _passThroughWalls;

        SnakeGame _snakeGame = null;
        WaitForSeconds _waitForUpdateInterval;
        IEnumerator _updateCoroutine = null;

        float _elapsedTimeBetweenUpdateInterval;
        Vector3 _snakeHeadCurrentPosition;
        Vector3 _snakeHeadPredictedPosition;

        private void Start()
        {
            // TODO : Design UI
            // CreateView();

            // TODO : The below should be in GameModel : Model, Model : IModel

            _snakeGame = new SnakeGame(new SnakeGameParameters
            {
                Dimension = Dimension.DimensionTwo,
                Size = new Vector4Int(10, 10, 10, 10),
                PassThroughWalls = _passThroughWalls
            });

            // TODO : create a space based on _snakeGame.Size
            // TODO : - need to zoom camera appropriately
            // TODO : - 1D and 2D needa be orthographic, 3D and 4D needa be perspective

            // TODO : Get user input (keyboard publish event / touch input publish event)

            // ==========
            UpdateVisuals();

            _waitForUpdateInterval = new WaitForSeconds(_updateInterval);
            _updateCoroutine = UpdateCoroutine();
            StartCoroutine(_updateCoroutine);
        }

        private void Update()
        {
            _elapsedTimeBetweenUpdateInterval += Time.deltaTime;
            float interpolationRatio = _elapsedTimeBetweenUpdateInterval / _updateInterval;

            if (_enableLerp) LerpSnakeHeadPosition(interpolationRatio);
            else SetSnakeHeadPosition();
        }

        private void LerpSnakeHeadPosition(float interpolationRatio)
        {
            Vector3 interpolatedPosition = Vector3.Lerp(_snakeHeadCurrentPosition, _snakeHeadPredictedPosition, interpolationRatio);
            _cube.transform.localPosition = interpolatedPosition;
        }

        private void SetSnakeHeadPosition()
        {
            _cube.transform.localPosition = _snakeHeadCurrentPosition;
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
            _elapsedTimeBetweenUpdateInterval = 0;
            UpdateSnakeHead();
        }

        private void UpdateSnakeHead()
        {
            _snakeHeadCurrentPosition = _snakeGame.GetSnakeHeadWorldSpacePosition();
            _snakeHeadPredictedPosition = _snakeGame.GetPredictedSnakeHeadWorldSpacePosition();
        }
    }
}
