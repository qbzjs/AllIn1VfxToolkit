using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUIArchitecture;
using Snake4D;

namespace GameScene
{
    public class GameManager : CustomMonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static int SizeReference => SIZE_REFERENCE;
        public const int SIZE_REFERENCE = 10;

        public SnakeGame SnakeGameInfo => _snakeGame;
        public int GameSize => SnakeGameInfo != null ? SnakeGameInfo.Size.x : _snakeGameSize; // Assumes x == y == z == w
        public Dimension GameDimension => SnakeGameInfo != null ? SnakeGameInfo.GameDimension : _snakeGameDimension;
        public float UpdateInterval => _updateInterval;
        public GameObject HeadPrefab => _headCubePrefab;
        public GameObject BodyPrefab => _bodyCubePrefab;
        public GameObject FoodPrefab => _foodCubePrefab;
        public ObjectPool PortalObjectPool => _portalObjectPool;
        public Color FoodColor => FoodPrefab.GetComponent<Renderer>().sharedMaterial.GetColor("_BaseColor");

        [Header("Game Manager")]
        [SerializeField] float _updateInterval;
        [SerializeField] int _bufferCapacity;
        [SerializeField] GameObject _headCubePrefab;
        [SerializeField] GameObject _bodyCubePrefab;
        [SerializeField] GameObject _foodCubePrefab;
        [SerializeField] ObjectPool _portalObjectPool;

        [Header("Snake Game Parameters")]
        [SerializeField] Dimension _snakeGameDimension;
        [SerializeField] int _snakeGameSize;
        [SerializeField] bool _passThroughWalls;

        [Header("Debug")]
        [SerializeField] bool _debugMode;
        [SerializeField] bool _useCustomSeed;
        [SerializeField] int _customSeed;

        SnakeGame _snakeGame = null;
        WaitForSeconds _waitForUpdateInterval;
        IEnumerator _updateCoroutine = null;

        InputBuffer _inputBuffer;

        private void Awake()
        {
            // Singleton Pattern
            if (GameManager.Instance != null && GameManager.Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;

            int randomSeed = new System.Random().Next(0, 1000);
            if (_useCustomSeed) randomSeed = _customSeed;
            else _customSeed = randomSeed;

            Debug.Log($"Using Random Seed '{randomSeed}'");
            Random.InitState(randomSeed);
        }

        protected override void Start()
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            yield return null; // Give one frame time for other components to subscribe to message hub

            _snakeGame = new SnakeGame(new SnakeGameParameters
            {
                Dimension = _snakeGameDimension,
                Size = new Vector4Int(_snakeGameSize, _snakeGameSize, _snakeGameSize, _snakeGameSize),
                PassThroughWalls = _passThroughWalls
            });

            InitGameStage();
            InitCameras(); // Need to InitGameStage first as camera positions depend on game stage.
            UpdateGameStageVisuals();

            _inputBuffer = new InputBuffer(_bufferCapacity);

            _waitForUpdateInterval = new WaitForSeconds(_updateInterval);
            _updateCoroutine = UpdateCoroutine();
            StartCoroutine(_updateCoroutine);

            base.Start();
        }

        protected override void SubscribeToMessageHubEvents()
        {
            SubscribeToMessageHubEvent<UserInputEvent>((e) =>
            {
                bool validInput = _inputBuffer.AddInput(e.InputType, _snakeGame.GameDimension);

                if (validInput)
                {
                    // DebugLog($"Input received! Input: {e.InputType}");

#if (UNITY_ANDROID || UNITY_IOS)
                    Handheld.Vibrate();
#endif
                }
            });
        }

        private IEnumerator UpdateCoroutine()
        {
            while (true)
            {
                if (_debugMode) yield return new WaitForSeconds(_updateInterval);
                else yield return _waitForUpdateInterval;

                // DebugLog(_snakeGame.DebugState());

                _snakeGame.OnUserInput(_inputBuffer.GetInput());
                _snakeGame.UpdateState();

                UpdateGameStageVisuals();

                // Current handling when bite its own tail
                if (_snakeGame.GameOver)
                {
                    if (_debugMode) yield return new WaitForSeconds(_updateInterval);
                    else yield return _waitForUpdateInterval;

                    break;
                }

                //! Testing transition
                if (GameDimension == Dimension.DimensionTwo && _snakeGame.GetSnakeTailLength() == 2)
                {
                    _snakeGame.UpdateDimension(Dimension.DimensionThree);
                    PublishMessageHubEvent<DynamicCameraHelper.RequestTransitionEvent>(new(DynamicCameraHelper.TransitionType.ToPerspective));
                }

                if (GameDimension == Dimension.DimensionThree && _snakeGame.GetSnakeTailLength() == 4)
                {
                    _snakeGame.UpdateDimension(Dimension.DimensionFour);
                    PublishMessageHubEvent<OrthoCameraHelper.TransitionTo4DViewEvent>(null);
                }
                //!
            }

            DebugLog("Game Over!");
        }

        private void InitCameras()
        {
            PublishMessageHubEvent<CameraHolder.SetPositionEvent>(null);
            PublishMessageHubEvent<OrthoCameraHelper.InitOrthoCameraEvent>(null);
            PublishMessageHubEvent<PerspectiveCameraHelper.SetLocalZEvent>(null);
            PublishMessageHubEvent<DynamicCameraHelper.SetTransformEvent>(null);
        }

        private void InitGameStage()
        {
            PublishMessageHubEvent<GameStage.RequestInitEvent>(null);
        }

        private void UpdateGameStageVisuals()
        {
            PublishMessageHubEvent<GameStage.RequestUpdateVisualsEvent>(null);
        }
    }
}
