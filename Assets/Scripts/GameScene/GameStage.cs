using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Snake4D;
using DG.Tweening;

namespace GameScene
{
    public class GameStage : CustomMonoBehaviour
    {
        [Header("Game Stage Settings")]
        [SerializeField] RenderPlane _renderPlane = RenderPlane.All;

        [Header("Tween Position Settings")]
        [SerializeField] bool _enableTweenPosition;
        [SerializeField] Ease _tweenPositionEase = Ease.Linear;
        [SerializeField] bool _enableTweenSizeWhenWarp;
        [SerializeField] float _warpSizeFactor = 0.5f;
        [SerializeField] Ease _tweenWarpSizeEaseIn = Ease.InQuad;
        [SerializeField] Ease _tweenWarpSizeEaseOut = Ease.OutQuad;

        [Header("Tween Direction Settings")]
        [SerializeField] bool _enableTweenDirection;
        [SerializeField] Ease _tweenDirectionEase = Ease.Linear;
        [SerializeField] bool _onlyTweenHeadDirection;

        [Header("Tween Food Settings")]
        [SerializeField] bool _enableTweenFoodSize;
        [SerializeField] float _spawnFoodsizeFactor = 0.5f;
        [SerializeField] Ease _tweenFoodEaseIn = Ease.InQuad;
        [SerializeField] Ease _tweenFoodEaseOut = Ease.OutQuad;


        [Header("Corner Settings")]
        [SerializeField] bool _enableCornerClones;

        SnakeGame _snakeGame => GameManager.Instance.SnakeGameInfo;
        float _updateInterval => GameManager.Instance.UpdateInterval;

        List<GameObject> _snakeBodyCubes = new List<GameObject>();

        List<Vector3Int> _snakeBodyPreviousPositions = new List<Vector3Int>();
        List<Vector3Int> _snakeBodyCurrentPositions = new List<Vector3Int>();

        List<Vector3Int> _snakeBodyPreviousDirections = new List<Vector3Int>();
        List<Vector3Int> _snakeBodyCurrentDirections = new List<Vector3Int>();

        List<GameObject> _snakePartClones = new List<GameObject>();
        List<Vector3Int> _snakePartClonePreviousPositions = new List<Vector3Int>();
        List<Vector3Int> _snakePartCloneCurrentPositions = new List<Vector3Int>();

        List<GameObject> _snakePartCornerClones = new List<GameObject>();

        Vector3 _snakeFoodPreviousPosition;
        Vector3 _snakeFoodCurrentPosition;

        GameObject _snakeFood;
        GameObject _snakeFoodClone;

        bool _isInit = false;

        protected override void SubscribeToMessageHubEvents()
        {
            SubscribeToMessageHubEvent<RequestInitEvent>((e) => Init());
            SubscribeToMessageHubEvent<RequestUpdateVisualsEvent>((e) => UpdateVisuals());
        }

        private void Init()
        {
            PublishMessageHubEvent<PlatformScaling.SetPositionAndScaleEvent>(null);

            GameObject snakeHead = Instantiate(GameManager.Instance.HeadPrefab, Vector3.zero, Quaternion.identity);
            snakeHead.transform.parent = this.transform;
            snakeHead.transform.localPosition = Vector3.zero;
            SetupLayer(snakeHead);
            SetupMaterialClipping(snakeHead);

            _snakeBodyCubes.Add(snakeHead);

            _snakeFood = Instantiate(GameManager.Instance.FoodPrefab, Vector3.zero, Quaternion.identity);
            _snakeFood.transform.parent = this.transform;
            _snakeFood.transform.localPosition = Vector3.one; // Arbitrary position
            SetupLayer(_snakeFood);

            _snakeFoodClone = Instantiate(GameManager.Instance.FoodPrefab, Vector3.zero, Quaternion.identity);
            _snakeFoodClone.transform.parent = this.transform;
            _snakeFoodClone.transform.localPosition = Vector3.one; // Arbitrary position
            SetupLayer(_snakeFoodClone);

            _isInit = true;
        }

        private void SetSnakeBodyPositions()
        {
            for (int i = 0; i < _snakeBodyCurrentPositions.Count; i++)
            {
                _snakeBodyCubes[i].transform.localPosition = _snakeBodyCurrentPositions[i];
            }
        }

        private void SetSnakeBodyDirections()
        {
            for (int i = 0; i < _snakeBodyCurrentPositions.Count; i++)
            {
                _snakeBodyCubes[i].transform.LookAt(_snakeBodyCubes[i].transform.position + _snakeBodyCurrentDirections[i]);
            }
        }

        private void UpdateVisuals()
        {
            UpdateSnakeBodyVisuals();
            UpdateSnakeFoodVisuals();
        }

        private void UpdateSnakeBodyVisuals()
        {
            SetAllClonesAsInactive();

            GetSnakeBodyPositions();
            GetSnakeBodyDirections();

            InstantiateSnakePart();
            InstantiateSnakePartClone();
            InstantiateSnakeCorners();

            for (int i = 0; i < _snakeBodyCurrentPositions.Count; i++)
            {
                HandleSnakePartPositions(i);
                HandleSnakePartDirections(i);
                HandleCornerClones(i);
            }
        }

        private void HandleCornerClones(int i)
        {
            // Handling when turning corners
            if (_snakeBodyCurrentDirections[i] != _snakeBodyPreviousDirections[i] && _enableCornerClones)
            {
                // Corner clones for all except the tail
                if (i != _snakeBodyCurrentPositions.Count - 1)
                {
                    _snakePartCornerClones[i].SetActive(true);
                    _snakePartCornerClones[i].transform.localPosition = _snakeGame.GetPreviousSnakeBodyWorldSpacePositions(_renderPlane)[i];
                }
            }
        }

        private void HandleSnakePartDirections(int i)
        {
            if (!_enableTweenDirection)
            {
                SetSnakeBodyDirections();
            }
            else
            {
                Vector3 crossProduct = Vector3.Cross(_snakeBodyPreviousDirections[i], _snakeBodyCurrentDirections[i]);
                if (_enableTweenDirection && crossProduct != Vector3.zero && !(i != 0 && _onlyTweenHeadDirection))
                {
                    _snakeBodyCubes[i].transform.DOBlendableRotateBy(crossProduct * 90, _updateInterval).SetEase(_tweenDirectionEase);

                    // Rotate the clones even if they are not active, so when they are turned active there will not be any visual glitches
                    _snakePartClones[i].transform.DOBlendableRotateBy(crossProduct * 90, _updateInterval).SetEase(_tweenDirectionEase);
                }
            }
        }

        private void HandleSnakePartPositions(int i)
        {
            if (!_enableTweenPosition)
            {
                SetSnakeBodyPositions();
            }
            else
            {
                // Handling when warping
                if ((_snakeBodyPreviousPositions[i] - _snakeBodyCurrentPositions[i]).sqrMagnitude > 1) // Square magnitude is more performant than calculating actual distance
                {
                    _snakeBodyPreviousPositions[i] = _snakeBodyCurrentPositions[i] - _snakeBodyCurrentDirections[i];

                    _snakePartClonePreviousPositions[i] = _snakeGame.GetPreviousSnakeBodyWorldSpacePositions(_renderPlane)[i];
                    _snakePartCloneCurrentPositions[i] = _snakePartClonePreviousPositions[i] + _snakeBodyCurrentDirections[i];

                    _snakePartClones[i].SetActive(true);
                    _snakePartClones[i].transform.localPosition = _snakePartClonePreviousPositions[i];
                    _snakePartClones[i].transform.DOLocalMove(_snakePartCloneCurrentPositions[i], _updateInterval).SetEase(_tweenPositionEase);

                    if (_enableTweenSizeWhenWarp)
                    {
                        Vector3 originalScale = GameManager.Instance.BodyPrefab.transform.localScale;
                        if (i == 0) originalScale = GameManager.Instance.HeadPrefab.transform.localScale;

                        _snakePartClones[i].transform.localScale = originalScale;
                        _snakePartClones[i].transform.DOScale(originalScale * _warpSizeFactor, _updateInterval).SetEase(_tweenWarpSizeEaseOut);

                        _snakeBodyCubes[i].transform.localScale = originalScale * _warpSizeFactor;
                        _snakeBodyCubes[i].transform.DOScale(originalScale, _updateInterval).SetEase(_tweenWarpSizeEaseIn); ;
                    }
                }

                // Tween Move
                _snakeBodyCubes[i].transform.localPosition = _snakeBodyPreviousPositions[i];
                _snakeBodyCubes[i].transform.DOLocalMove(_snakeBodyCurrentPositions[i], _updateInterval).SetEase(_tweenPositionEase);
            }
        }

        private void InstantiateSnakeCorners()
        {
            if (_snakePartCornerClones.Count < _snakeBodyCurrentPositions.Count)
            {
                GameObject newCorner = Instantiate(GameManager.Instance.BodyPrefab);
                newCorner.transform.SetParent(_snakeBodyCubes[_snakeBodyCubes.Count - 1].transform.parent);
                SetupLayer(newCorner);
                SetupMaterialClipping(newCorner);

                newCorner.SetActive(false);
                newCorner.name = "Corner";
                _snakePartCornerClones.Add(newCorner);
            }
        }

        private void InstantiateSnakePartClone()
        {
            if (_snakePartClones.Count < _snakeBodyCurrentPositions.Count)
            {
                GameObject newClone = Instantiate(_snakeBodyCubes[_snakeBodyCubes.Count - 1]);
                newClone.transform.SetParent(_snakeBodyCubes[_snakeBodyCubes.Count - 1].transform.parent);
                SetupLayer(newClone);
                SetupMaterialClipping(newClone);

                newClone.SetActive(false);
                _snakePartClones.Add(newClone);
                _snakePartClonePreviousPositions.Add(new Vector3Int());
                _snakePartCloneCurrentPositions.Add(new Vector3Int());
            }
        }

        private void InstantiateSnakePart()
        {
            if (_snakeBodyCubes.Count < _snakeBodyCurrentPositions.Count)
            {
                GameObject newBodyCube = Instantiate(GameManager.Instance.BodyPrefab);
                newBodyCube.transform.SetParent(_snakeBodyCubes[0].transform.parent);
                newBodyCube.transform.localPosition = _snakeBodyPreviousPositions[_snakeBodyPreviousPositions.Count - 1];
                SetupLayer(newBodyCube);
                SetupMaterialClipping(newBodyCube);

                _snakeBodyCubes.Add(newBodyCube);
            }
        }

        private void GetSnakeBodyDirections()
        {
            _snakeBodyPreviousDirections = _snakeGame.GetPreviousSnakeBodyWorldSpaceDirections(_renderPlane);
            _snakeBodyCurrentDirections = _snakeGame.GetCurrentSnakeBodyWorldSpaceDirections(_renderPlane);
        }

        private void GetSnakeBodyPositions()
        {
            _snakeBodyPreviousPositions = _snakeGame.GetPreviousSnakeBodyWorldSpacePositions(_renderPlane);
            _snakeBodyCurrentPositions = _snakeGame.GetCurrentSnakeBodyWorldSpacePositions(_renderPlane);
        }

        private void SetAllClonesAsInactive()
        {
            for (int i = 0; i < _snakeBodyCurrentPositions.Count; i++)
            {
                if (_snakePartClones[i].activeSelf)
                {
                    _snakePartClones[i].SetActive(false);
                }

                if (_snakePartCornerClones[i].activeSelf)
                {
                    _snakePartCornerClones[i].SetActive(false);
                }
            }
        }

        private void SetupLayer(GameObject gameObjectToSetup)
        {
            foreach (Transform transformComponent in gameObjectToSetup.GetComponentsInChildren<Transform>())
            {
                transformComponent.gameObject.layer = gameObjectToSetup.transform.parent.gameObject.layer;
            }
        }

        private void SetupMaterialClipping(GameObject gameObjectToSetup)
        {
            const float BUFFER = 0.1f;

            Renderer[] renderers = gameObjectToSetup.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                Material materialInstance = renderer.material;

                // Assumed a material that has "_X_Max_Plane_Position" will also have the rest of the parameters
                if (!materialInstance.HasVector("_X_Max_Plane_Position")) continue;

                materialInstance.SetVector("_X_Max_Plane_Position", new Vector3(transform.position.x - 0.5f - BUFFER, 0, 0));
                materialInstance.SetVector("_X_Min_Plane_Position", new Vector3(transform.position.x - 0.5f + GameManager.Instance.GameSize + BUFFER, 0, 0));

                materialInstance.SetVector("_Y_Max_Plane_Position", new Vector3(0, -0.5f + GameManager.Instance.GameSize + BUFFER, 0));

                materialInstance.SetVector("_Z_Max_Plane_Position", new Vector3(0, 0, transform.position.z - 0.5f - BUFFER));
                materialInstance.SetVector("_Z_Min_Plane_Position", new Vector3(0, 0, transform.position.z - 0.5f + GameManager.Instance.GameSize + BUFFER));
            }
        }

        private void UpdateSnakeFoodVisuals()
        {
            if (_snakeFoodClone.activeSelf)
            {
                _snakeFoodClone.SetActive(false);
            }

            // TODO : If game over, still make clone but also destroy the food
            _snakeFoodCurrentPosition = _snakeGame.GetCurrentSnakeFoodWorldSpacePosition(_renderPlane);
            _snakeFoodPreviousPosition = _snakeGame.GetPreviousSnakeFoodWorldSpacePosition(_renderPlane);

            if (_snakeFood.transform.localPosition != _snakeFoodCurrentPosition)
            {
                _snakeFood.transform.localPosition = _snakeFoodCurrentPosition;

                if (_enableTweenFoodSize)
                {
                    Vector3 originalScale = GameManager.Instance.FoodPrefab.transform.localScale;
                    _snakeFood.transform.localScale = originalScale * _spawnFoodsizeFactor;
                    _snakeFood.transform.DOScale(originalScale, _updateInterval).SetEase(_tweenFoodEaseOut);

                    _snakeFoodClone.SetActive(true);
                    _snakeFoodClone.transform.SetParent(_snakeFood.transform.parent);
                    _snakeFoodClone.transform.localPosition = _snakeFoodPreviousPosition;
                    _snakeFoodClone.transform.localScale = originalScale;
                    _snakeFoodClone.transform.DOScale(originalScale * _spawnFoodsizeFactor, _updateInterval).SetEase(_tweenFoodEaseIn); ;
                }
            }
        }

        public class RequestUpdateVisualsEvent { }
        public class RequestInitEvent { }
    }
}
