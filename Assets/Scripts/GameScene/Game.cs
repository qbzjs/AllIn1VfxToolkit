using System.Collections;
using UnityEngine;
using FairyGUIArchitecture;
using Snake4D;

namespace GameScene
{
    public class Game : SceneInit
    {
        [Header("Game")]
        [SerializeField] float _updateInterval;
        [SerializeField] bool _enableLerp;
        [SerializeField] GameObject _cube;

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
        Vector3 _snakeHeadPredictedPosition;

        GameObject _clone;
        Vector3 _clonePreviousPosition;
        Vector3 _cloneCurrentPosition;
        Vector3 _clonePredictedPosition;

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

            // TODO : create a space based on _snakeGame.Size
            // TODO : - need to zoom camera appropriately
            // TODO : - 1D and 2D needa be orthographic, 3D and 4D needa be perspective

            // TODO : Get user input (keyboard publish event / touch input publish event)

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
                DebugLog($"Received input of type '{e.InputType}'");
                _snakeGame.OnUserInput(e.InputType);
            });
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
            Vector3 interpolatedPosition = Vector3.Lerp(_snakeHeadPreviousPosition, _snakeHeadCurrentPosition, interpolationRatio);
            _cube.transform.localPosition = interpolatedPosition;

            if (_clone != null)
            {
                Vector3 cloneInterpolatedPosition = Vector3.Lerp(_clonePreviousPosition, _cloneCurrentPosition, interpolationRatio);
                _clone.transform.localPosition = cloneInterpolatedPosition;
            }
        }

        private void SetSnakeHeadPosition()
        {
            _cube.transform.localPosition = _snakeHeadCurrentPosition;
        }

        private IEnumerator UpdateCoroutine()
        {
            while (true) // TODO : use a better boolean
            {
                if (_debugMode) yield return new WaitForSeconds(_updateInterval);
                else yield return _waitForUpdateInterval;

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
            if (_clone != null)
            {
                Destroy(_clone);
                _clone = null;
            }

            _snakeHeadPreviousPosition = _snakeGame.GetPreviousSnakeHeadWorldSpacePosition();
            _snakeHeadCurrentPosition = _snakeGame.GetCurrentSnakeHeadWorldSpacePosition();

            // Handling when warping
            if (Vector3.Distance(_snakeHeadPreviousPosition, _snakeHeadCurrentPosition) != 1 && _enableLerp)
            {
                Vector3 pastDirection = _snakeGame.GetPreviousSnakeHeadWorldSpaceDirection();
                _snakeHeadPreviousPosition = _snakeHeadCurrentPosition - pastDirection;

                _clone = Instantiate(_cube);
                _clone.transform.SetParent(_cube.transform.parent);
                _clone.transform.localPosition = _cube.transform.localPosition;

                _clonePreviousPosition = _snakeGame.GetPreviousSnakeHeadWorldSpacePosition();
                _cloneCurrentPosition = _clonePreviousPosition + pastDirection;
            }
        }
    }
}
