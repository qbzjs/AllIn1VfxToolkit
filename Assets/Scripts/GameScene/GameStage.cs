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
        [SerializeField] bool _tweenSizeWhenRotating;
        [SerializeField] float _rotatingSizeFactor = 0.85f;

        [Header("Tween Food Settings")]
        [SerializeField] bool _enableTweenFoodSize;
        [SerializeField] float _spawnFoodsizeFactor = 0.5f;
        [SerializeField] Ease _tweenFoodEaseIn = Ease.InQuad;
        [SerializeField] Ease _tweenFoodEaseOut = Ease.OutQuad;

        [Header("Override Height Settings")]
        [SerializeField] bool _overrideSnakeHeadHeight = true;
        [SerializeField] int _overrideSnakeHeadYPosition = 2;
        [SerializeField] bool _overrideFoodHeight = true;
        [SerializeField] int _overrideFoodYPosition = 1;

        [Header("Onion Settings")]
        [SerializeField] bool _enableOnion;
        [SerializeField][Range(0, 1)] float _onionOpacity = 0.3f;
        [SerializeField][Range(0, 1)] float _foodOnionOpacity = 0.7f;

        [Header("Corner Settings")]
        [SerializeField] bool _enableCornerClones;

        SnakeGame _snakeGame => GameManager.Instance.SnakeGameInfo;
        float _updateInterval => GameManager.Instance.UpdateInterval;

        List<GameObject> _snakeBodyCubes = new List<GameObject>();

        List<Vector4Int> _snakeBodyCurrentGameSpacePositions = new List<Vector4Int>();
        List<Vector3Int> _snakeBodyPreviousPositions = new List<Vector3Int>();
        List<Vector3Int> _snakeBodyCurrentPositions = new List<Vector3Int>();

        List<Vector3Int> _snakeBodyPreviousDirections = new List<Vector3Int>();
        List<Vector3Int> _snakeBodyCurrentDirections = new List<Vector3Int>();

        List<GameObject> _snakePartClones = new List<GameObject>();
        List<Vector3Int> _snakePartClonePreviousPositions = new List<Vector3Int>();
        List<Vector3Int> _snakePartCloneCurrentPositions = new List<Vector3Int>();

        List<GameObject> _snakePartCornerClones = new List<GameObject>();

        Vector4Int _snakeFoodCurrentGamePosition;
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

            if (_renderPlane != RenderPlane.All)
            {
                _snakeBodyCubes[0].transform.localPosition = new Vector3(_snakeBodyCurrentPositions[0].x, 2, _snakeBodyCurrentPositions[0].z);
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
                HandleSnakePartPosition(i);
                HandleSnakePartDirection(i);
                HandleCornerClone(i);
                HandleOnionSkin(i);
            }
        }

        private void HandleOnionSkin(int i)
        {
            if (!_enableOnion) return;

            // The snake head does not change color
            if (i == 0) return;

            // Assumes that the target child with the desired MeshRenderer is higher in the hierarchy
            Vector4Int snakePartGameSpacePosition = _snakeBodyCurrentGameSpacePositions[i];
            float distance = GameDistanceWithSnakeHead(snakePartGameSpacePosition);

            Material materialInstance = _snakeBodyCubes[i].GetComponentInChildren<MeshRenderer>().material;
            Material cloneMaterialInstance = _snakePartClones[i].GetComponentInChildren<MeshRenderer>().material;

            if (distance == 0)
            {
                materialInstance.DOFloat(1f, "_opacity", _updateInterval);
                cloneMaterialInstance.DOFloat(1f, "_opacity", _updateInterval);

                materialInstance.DOColor(GameManager.Instance.BodyColor, "_mainColor", _updateInterval);
                cloneMaterialInstance.DOColor(GameManager.Instance.BodyColor, "_mainColor", _updateInterval);
            }
            else if (distance != 0)
            {
                materialInstance.DOFloat(_onionOpacity, "_opacity", _updateInterval);
                cloneMaterialInstance.DOFloat(_onionOpacity, "_opacity", _updateInterval);

                materialInstance.DOColor(GameManager.Instance.OnionColor, "_mainColor", _updateInterval);
                cloneMaterialInstance.DOColor(GameManager.Instance.OnionColor, "_mainColor", _updateInterval);
            }
        }

        private float GameDistanceWithSnakeHead(Vector4Int gamePositionToCompare)
        {
            float distance = 0;
            Vector4Int headGameSpacePosition = _snakeBodyCurrentGameSpacePositions[0];
            if (_renderPlane == RenderPlane.XY)
            {
                float wDistance = Mathf.Abs(headGameSpacePosition.w - gamePositionToCompare.w);
                float zDistance = Mathf.Abs(headGameSpacePosition.z - gamePositionToCompare.z);
                distance = wDistance + zDistance;
            }
            else if (_renderPlane == RenderPlane.ZW)
            {
                float xDistance = Mathf.Abs(headGameSpacePosition.x - gamePositionToCompare.x);
                float yDistance = Mathf.Abs(headGameSpacePosition.y - gamePositionToCompare.y);
                distance = xDistance + yDistance;
            }
            else if (_renderPlane == RenderPlane.All)
            {
                float wDistance = Mathf.Abs(headGameSpacePosition.w - gamePositionToCompare.w);
                distance = wDistance;
            }

            return distance;
        }

        private void HandleCornerClone(int i)
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

        private void HandleSnakePartDirection(int i)
        {
            if (!_enableTweenDirection)
            {
                SetSnakeBodyDirections();
            }
            else
            {
                if (_enableTweenDirection)
                {
                    // currentDirection is defined as (currentPosition - previousPosition)
                    // And all snake body parts are initially in the previous position, tweening towards the current position

                    Vector3 previousDirection = _snakeBodyPreviousDirections[i];
                    Vector3 currentDirection = _snakeBodyCurrentDirections[i];
                    Vector3 currentCrossProduct = Vector3.Cross(previousDirection, currentDirection);

                    // Head direction handling
                    if (i == 0 && currentCrossProduct != Vector3.zero) // Non-zero cross product => Not the same direction, have rotation
                    {
                        TweenRotateSnakeBodyPart(i, currentCrossProduct, 90f, _updateInterval);
                    }

                    // Body direction handling
                    else if (i > 0 && !_onlyTweenHeadDirection)
                    {
                        Vector3 futureDirection = _snakeBodyCurrentDirections[i - 1];
                        Vector3 futureCrossProduct = Vector3.Cross(currentDirection, futureDirection);

                        // Finish pre-rotated rotation
                        if (currentCrossProduct != Vector3.zero && futureCrossProduct == Vector3.zero)
                        {
                            if (_tweenSizeWhenRotating)
                                TweenResizeSnakeBodyPart(i, 1f, _updateInterval);

                            TweenRotateSnakeBodyPart(i, currentCrossProduct, 45f, _updateInterval);
                        }

                        // Finish pre-rotated rotation + Pre-rotate before actually rotating
                        else if (currentCrossProduct != Vector3.zero && futureCrossProduct != Vector3.zero)
                        {
                            TweenRotateSnakeBodyPart(i, currentCrossProduct, 45f, _updateInterval / 2)
                                .OnComplete(() => TweenRotateSnakeBodyPart(i, futureCrossProduct, 45f, _updateInterval / 2));
                        }

                        // Pre-rotate before actually rotating
                        else if (currentCrossProduct == Vector3.zero && futureCrossProduct != Vector3.zero)
                        {
                            if (_tweenSizeWhenRotating)
                                TweenResizeSnakeBodyPart(i, _rotatingSizeFactor, _updateInterval);

                            TweenRotateSnakeBodyPart(i, futureCrossProduct, 45f, _updateInterval);
                        }
                    }
                }
            }

            Tween TweenRotateSnakeBodyPart(int i, Vector3 crossProduct, float degrees, float duration)
            {
                Sequence parallelRotationSequence = DOTween.Sequence();
                parallelRotationSequence
                    // Rotate the snake parts
                    .Append(_snakeBodyCubes[i].transform.DOBlendableRotateBy(crossProduct * degrees, duration).SetEase(_tweenDirectionEase))

                    // Rotate the clones even if they are not active, so when they are turned active there will not be any visual glitches
                    .Join(_snakePartClones[i].transform.DOBlendableRotateBy(crossProduct * degrees, duration).SetEase(_tweenDirectionEase));

                return parallelRotationSequence;
            }

            Tween TweenResizeSnakeBodyPart(int i, float scaleFactor, float duration)
            {
                Vector3 originalScale = GameManager.Instance.BodyPrefab.transform.localScale;

                Sequence resizeSequence = DOTween.Sequence();
                resizeSequence
                    .Append(_snakeBodyCubes[i].transform.DOScale(originalScale * scaleFactor, duration))
                    .Join(_snakePartClones[i].transform.DOScale(originalScale * scaleFactor, duration));

                return resizeSequence;
            }
        }

        private void HandleSnakePartPosition(int i)
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

                    // Tween Move Clone
                    if (_overrideSnakeHeadHeight && _renderPlane != RenderPlane.All && i == 0)
                    {
                        Vector3 previousPosition = new Vector3(_snakePartClonePreviousPositions[i].x, _overrideSnakeHeadYPosition, _snakePartClonePreviousPositions[i].z);
                        Vector3 currentPosition = new Vector3(_snakePartCloneCurrentPositions[i].x, _overrideSnakeHeadYPosition, _snakePartCloneCurrentPositions[i].z);
                        _snakePartClones[i].transform.localPosition = previousPosition;
                        _snakePartClones[i].transform.DOLocalMove(currentPosition, _updateInterval).SetEase(_tweenPositionEase);
                    }
                    else
                    {
                        _snakePartClones[i].transform.localPosition = _snakePartClonePreviousPositions[i];
                        _snakePartClones[i].transform.DOLocalMove(_snakePartCloneCurrentPositions[i], _updateInterval).SetEase(_tweenPositionEase);
                    }

                    HandlePortalSpawnWhenWarp(i);
                    HandleTweenSizeWhenWarp(i);
                }

                // Tween Move
                if (_overrideSnakeHeadHeight && _renderPlane != RenderPlane.All && i == 0)
                {
                    Vector3 previousPosition = new Vector3(_snakeBodyPreviousPositions[i].x, _overrideSnakeHeadYPosition, _snakeBodyPreviousPositions[i].z);
                    Vector3 currentPosition = new Vector3(_snakeBodyCurrentPositions[i].x, _overrideSnakeHeadYPosition, _snakeBodyCurrentPositions[i].z);
                    _snakeBodyCubes[i].transform.localPosition = previousPosition;
                    _snakeBodyCubes[i].transform.DOLocalMove(currentPosition, _updateInterval).SetEase(_tweenPositionEase);
                }
                else
                {
                    _snakeBodyCubes[i].transform.localPosition = _snakeBodyPreviousPositions[i];
                    _snakeBodyCubes[i].transform.DOLocalMove(_snakeBodyCurrentPositions[i], _updateInterval).SetEase(_tweenPositionEase);
                }
            }
        }

        private void HandleTweenSizeWhenWarp(int i)
        {
            if (_enableTweenSizeWhenWarp)
            {
                Vector3 originalScale = GameManager.Instance.BodyPrefab.transform.localScale;
                if (i == 0) originalScale = GameManager.Instance.HeadPrefab.transform.localScale;

                _snakePartClones[i].transform.localScale = originalScale;
                _snakePartClones[i].transform.DOScale(originalScale * _warpSizeFactor, _updateInterval).SetEase(_tweenWarpSizeEaseOut);

                _snakeBodyCubes[i].transform.localScale = originalScale * _warpSizeFactor;
                _snakeBodyCubes[i].transform.DOScale(originalScale, _updateInterval).SetEase(_tweenWarpSizeEaseIn);
            }
        }

        private void HandlePortalSpawnWhenWarp(int i)
        {
            const float DISTANCE_FACTOR = 0.7f; // Make sure platform gives sufficient space for prtal to spawn

            float gameSize = GameManager.Instance.GameSize;
            // Map the position to a color, so that the portal at that position will always have the same color
            Color color = new Color(_snakeBodyCurrentPositions[i].x / gameSize, _snakeBodyCurrentPositions[i].y / gameSize, _snakeBodyCurrentPositions[i].z / gameSize);

            Vector3 spawnLocalPosition_In = _snakePartClonePreviousPositions[i] + DISTANCE_FACTOR * (Vector3)_snakeBodyCurrentDirections[i];
            SpawnPortal(spawnLocalPosition_In, _snakeBodyCurrentDirections[i], color);

            Vector3 spawnLocalPosition_Out = _snakeBodyCurrentPositions[i] - DISTANCE_FACTOR * (Vector3)_snakeBodyCurrentDirections[i];
            SpawnPortal(spawnLocalPosition_Out, _snakeBodyCurrentDirections[i], color);
        }

        private void SpawnPortal(Vector3 spawnLocalPosition, Vector3 currentDirection, Color portalColor)
        {
            Vector3 rotation = Vector3.zero;
            if (currentDirection.y != 0) rotation.x = 90;
            if (currentDirection.x != 0) rotation.y = 90;

            GameObject portal = GameManager.Instance.PortalObjectPool.TakeFromPool();
            portal.transform.SetParent(this.transform);
            portal.transform.localPosition = spawnLocalPosition;
            portal.transform.localEulerAngles = rotation;

            SpriteRenderer spriteRenderer = portal.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null) spriteRenderer.color = portalColor;

            Sequence portalSequence = DOTween.Sequence();
            portalSequence
                .Append(portal.transform.DOScale(Vector3.one, _updateInterval).SetEase(Ease.InOutQuad))
                .AppendInterval(_updateInterval * 2)
                .Append(portal.transform.DOScale(Vector3.zero, _updateInterval).SetEase(Ease.InOutQuad))
                .AppendCallback(() => GameManager.Instance.PortalObjectPool.ReturnToPool(portal));
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
            _snakeBodyCurrentGameSpacePositions = _snakeGame.GetCurrentSnakeBodyGameSpacePositions();
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
            const float BUFFER = 0.08f;

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

            int foodYPosition = 0;
            if (_overrideFoodHeight) foodYPosition = _overrideFoodYPosition;

            _snakeFoodCurrentGamePosition = _snakeGame.GetCurrentSnakeFoodGameSpacePosition();
            _snakeFoodCurrentPosition = _snakeGame.GetCurrentSnakeFoodWorldSpacePosition(_renderPlane, foodYPosition);
            _snakeFoodPreviousPosition = _snakeGame.GetPreviousSnakeFoodWorldSpacePosition(_renderPlane, foodYPosition);

            if (_enableOnion)
            {
                float distance = GameDistanceWithSnakeHead(_snakeFoodCurrentGamePosition);
                if (distance == 0)
                {
                    _snakeFood.GetComponent<Renderer>().material.DOColor(GameManager.Instance.FoodColor, "_BaseColor", _updateInterval);
                    _snakeFoodClone.GetComponent<Renderer>().material.DOColor(GameManager.Instance.FoodColor, "_BaseColor", _updateInterval);
                }
                else
                {
                    Color color = new Color(GameManager.Instance.FoodColor.r, GameManager.Instance.FoodColor.g, GameManager.Instance.FoodColor.b, _foodOnionOpacity);
                    _snakeFood.GetComponent<Renderer>().material.DOColor(color, "_BaseColor", _updateInterval);
                    _snakeFoodClone.GetComponent<Renderer>().material.DOColor(color, "_BaseColor", _updateInterval);
                }
            }

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
