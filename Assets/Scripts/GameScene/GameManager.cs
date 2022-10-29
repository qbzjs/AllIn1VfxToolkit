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
        [SerializeField] List<GameObject> _snakeBodyCubes;
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

        List<Vector3Int> _snakeBodyPreviousPositions = new List<Vector3Int>();
        List<Vector3Int> _snakeBodyCurrentPositions = new List<Vector3Int>();

        List<GameObject> _snakePartClones = new List<GameObject>();
        List<Vector3Int> _snakePartClonePreviousPositions = new List<Vector3Int>();
        List<Vector3Int> _snakePartCloneCurrentPositions = new List<Vector3Int>();

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
            for (int i = 0; i < _snakeBodyCurrentPositions.Count; i++)
            {
                Vector3 interpolatedPosition = Vector3.Lerp(_snakeBodyPreviousPositions[i], _snakeBodyCurrentPositions[i], interpolationRatio);
                _snakeBodyCubes[i].transform.localPosition = interpolatedPosition;

                if (_snakePartClones[i] != null)
                {
                    Vector3 cloneInterpolatedPosition = Vector3.Lerp(_snakePartClonePreviousPositions[i], _snakePartCloneCurrentPositions[i], interpolationRatio);
                    _snakePartClones[i].transform.localPosition = cloneInterpolatedPosition;
                }
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
            for (int i = 0; i < _snakeBodyCurrentPositions.Count; i++)
            {
                _snakeBodyCubes[i].transform.localPosition = _snakeBodyCurrentPositions[i];
            }
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
            UpdateSnakeBodyVisuals();
            UpdateSnakeFoodVisuals();
        }

        private void UpdateSnakeBodyVisuals()
        {
            for (int i = 0; i < _snakePartClones.Count; i++)
            {
                if (_snakePartClones[i] != null)
                {
                    Destroy(_snakePartClones[i]);
                    _snakePartClones[i] = null;
                }
            }

            _snakeBodyPreviousPositions = _snakeGame.GetPreviousSnakeBodyWorldSpacePositions();
            _snakeBodyCurrentPositions = _snakeGame.GetCurrentSnakeBodyWorldSpacePositions();

            // TODO : Checking if the count of _snakeBodyCubes and _snakePartClones matches _snakeBodyCurrentPositions
            // TODO : If less than, then instantiate and add to the _snakeBodyCubes, with position set
            // TODO : Add null to the list of _snakePartClones

            if (_snakePartClones.Count < _snakeBodyCurrentPositions.Count)
            {
                _snakePartClones.Add(null);
                _snakePartClonePreviousPositions.Add(new Vector3Int());
                _snakePartCloneCurrentPositions.Add(new Vector3Int());
            }

            for (int i = 0; i < _snakeBodyCurrentPositions.Count; i++)
            {
                // Handling when warping
                // TODO : Handling when more than one snake body part is warping at the same time!
                if (Vector3.Distance(_snakeBodyPreviousPositions[i], _snakeBodyCurrentPositions[i]) > 1 && _enableLerp)
                {
                    Vector3Int pastDirection = _snakeGame.GetPreviousSnakeBodyWorldSpaceDirections()[i];
                    _snakeBodyPreviousPositions[i] = _snakeBodyCurrentPositions[i] - pastDirection;

                    _snakePartClones[i] = Instantiate(_snakeBodyCubes[i]);
                    _snakePartClones[i].transform.SetParent(_snakeBodyCubes[i].transform.parent);
                    _snakePartClones[i].transform.localPosition = _snakeBodyCubes[i].transform.localPosition;

                    _snakePartClonePreviousPositions[i] = _snakeGame.GetPreviousSnakeBodyWorldSpacePositions()[i];
                    _snakePartCloneCurrentPositions[i] = _snakePartClonePreviousPositions[i] + pastDirection;
                }
            }
        }

        private void UpdateSnakeFoodVisuals()
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

                if (_enableLerp)
                {
                    _snakeFoodClone = Instantiate(_foodCube);
                    _snakeFoodClone.transform.SetParent(_foodCube.transform.parent);
                    _snakeFoodClone.transform.localPosition = _snakeFoodPreviousPosition;
                }
            }
        }
    }
}
