using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUIArchitecture;
using Snake4D;

namespace GameScene
{
    public class GameManager : SceneInit
    {
        [Header("Game Manager")]
        [SerializeField] float _updateInterval;
        [SerializeField] bool _enableLerp;
        [SerializeField] int _bufferCapacity = 2;
        [SerializeField] GameObject _cube;
        [SerializeField] GameObject _foodCube;

        [Header("Snake Game Parameters")]
        [SerializeField] bool _passThroughWalls;

        [Header("Debug")]
        [SerializeField] bool _debugMode;
        [SerializeField] bool _useCustomSeed;
        [SerializeField] int _customSeed;

        SnakeGame _snakeGame = null;
        WaitForSeconds _waitForUpdateInterval;
        IEnumerator _updateCoroutine = null;

        float _elapsedTimeBetweenUpdateInterval;

        Vector3 _snakeHeadPreviousPosition;
        Vector3 _snakeHeadCurrentPosition;

        GameObject _snakeHeadClone;
        Vector3 _snakeHeadClonePreviousPosition;
        Vector3 _snakeHeadCloneCurrentPosition;

        Vector3 _snakeFoodPreviousPosition;
        Vector3 _snakeFoodCurrentPosition;

        GameObject _snakeFoodClone;

        InputBuffer _inputBuffer;

        private void Start()
        {
            int randomSeed = new System.Random().Next(0, 1000);
            if (_useCustomSeed) randomSeed = _customSeed;

            DebugLog($"Using Random Seed '{randomSeed}'");
            Random.InitState(randomSeed);

            // TODO : Design UI
            // CreateView();

            // TODO : The below should be in GameModel : Model, Model : IModel

            _snakeGame = new SnakeGame(new SnakeGameParameters
            {
                Dimension = Dimension.DimensionTwo,
                Size = new Vector4Int(10, 10, 10, 10),
                PassThroughWalls = _passThroughWalls
            });

            _inputBuffer = new InputBuffer(_bufferCapacity);

            // TODO : create a space based on _snakeGame.Size
            // TODO : - need to zoom camera appropriately
            // TODO : - 1D and 2D needa be orthographic, 3D and 4D needa be perspective

            // ==========
            UpdateVisuals();

            _waitForUpdateInterval = new WaitForSeconds(_updateInterval);
            _updateCoroutine = UpdateCoroutine();
            StartCoroutine(_updateCoroutine);

            SubscribeToMessageHubEvents();
        }

        protected override void SubscribeToMessageHubEvents()
        {
            SubscribeToMessageHubEvent<UserInputEvent>((e) =>
            {
                // DebugLog($"Received input of type '{e.InputType}'");
                _inputBuffer.AddInput(e.InputType);
            });
        }

        private void Update()
        {
            _elapsedTimeBetweenUpdateInterval += Time.deltaTime;
            float interpolationRatio = _elapsedTimeBetweenUpdateInterval / _updateInterval;

            if (_enableLerp)
            {
                LerpSnakeHeadPosition(interpolationRatio);
                LerpSnakeFoodSize(interpolationRatio);
            }
            else
            {
                SetSnakeHeadPosition();
            }
        }

        private void LerpSnakeHeadPosition(float interpolationRatio)
        {
            Vector3 interpolatedPosition = Vector3.Lerp(_snakeHeadPreviousPosition, _snakeHeadCurrentPosition, interpolationRatio);
            _cube.transform.localPosition = interpolatedPosition;

            if (_snakeHeadClone != null)
            {
                Vector3 cloneInterpolatedPosition = Vector3.Lerp(_snakeHeadClonePreviousPosition, _snakeHeadCloneCurrentPosition, interpolationRatio);
                _snakeHeadClone.transform.localPosition = cloneInterpolatedPosition;
            }
        }

        private void LerpSnakeFoodSize(float interpolationRatio)
        {
            if (_snakeFoodClone == null) return;

            Vector3 interpolatedSize = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), Vector3.one, interpolationRatio);
            _foodCube.transform.localScale = interpolatedSize;

            interpolatedSize = Vector3.Lerp(Vector3.one, new Vector3(0.5f, 0.5f, 0.5f), interpolationRatio);
            _snakeFoodClone.transform.localScale = interpolatedSize;
        }

        private void SetSnakeHeadPosition()
        {
            _cube.transform.localPosition = _snakeHeadCurrentPosition;
        }

        private IEnumerator UpdateCoroutine()
        {
            while (!_snakeGame.GameOver)
            {
                if (_debugMode) yield return new WaitForSeconds(_updateInterval);
                else yield return _waitForUpdateInterval;

                _snakeGame.OnUserInput(_inputBuffer.GetInput());
                _snakeGame.UpdateState();
                UpdateVisuals();
            }
        }

        private void UpdateVisuals()
        {
            _elapsedTimeBetweenUpdateInterval = 0;
            UpdateSnakeHead();
            UpdateSnakeFood();
        }

        private void UpdateSnakeHead()
        {
            if (_snakeHeadClone != null)
            {
                Destroy(_snakeHeadClone);
                _snakeHeadClone = null;
            }

            _snakeHeadPreviousPosition = _snakeGame.GetPreviousSnakeHeadWorldSpacePosition();
            _snakeHeadCurrentPosition = _snakeGame.GetCurrentSnakeHeadWorldSpacePosition();

            // Handling when warping
            if (Vector3.Distance(_snakeHeadPreviousPosition, _snakeHeadCurrentPosition) != 1 && _enableLerp)
            {
                Vector3 pastDirection = _snakeGame.GetPreviousSnakeHeadWorldSpaceDirection();
                _snakeHeadPreviousPosition = _snakeHeadCurrentPosition - pastDirection;

                _snakeHeadClone = Instantiate(_cube);
                _snakeHeadClone.transform.SetParent(_cube.transform.parent);
                _snakeHeadClone.transform.localPosition = _cube.transform.localPosition;

                _snakeHeadClonePreviousPosition = _snakeGame.GetPreviousSnakeHeadWorldSpacePosition();
                _snakeHeadCloneCurrentPosition = _snakeHeadClonePreviousPosition + pastDirection;
            }
        }

        private void UpdateSnakeFood()
        {
            if (_snakeFoodClone != null)
            {
                Destroy(_snakeFoodClone);
                _snakeFoodClone = null;
            }

            // TODO : When updating food, make food clone for the food being eaten, comparing past position of the food
            // TODO : If game over, still make clone but also destroy the food
            _snakeFoodCurrentPosition = _snakeGame.GetCurrentSnakeFoodWorldSpacePosition();
            _snakeFoodPreviousPosition = _snakeGame.GetPreviousSnakeFoodWorldSpacePosition();

            if (_foodCube.transform.localPosition != _snakeFoodCurrentPosition)
            {
                _foodCube.transform.localPosition = _snakeFoodCurrentPosition;

                // TODO : Clone and lerp the size to show being eaten
                // TODO : Lerp size of food small to big to show it spawned

                _snakeFoodClone = Instantiate(_foodCube);
                _snakeFoodClone.transform.SetParent(_foodCube.transform.parent);
                _snakeFoodClone.transform.localPosition = _snakeFoodPreviousPosition;
            }
        }
    }
}
