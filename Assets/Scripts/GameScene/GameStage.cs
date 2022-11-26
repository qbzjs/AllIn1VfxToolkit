using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Snake4D;

namespace GameScene
{
    public class GameStage : CustomMonoBehaviour
    {
        [Header("Visual Settings")]
        [SerializeField] bool _enableLerpPosition;
        [SerializeField] bool _enableLerpDirection;
        [SerializeField] bool _onlyLerpHeadDirection;
        [SerializeField] bool _enableCornerClones;

        SnakeGame _snakeGame => GameManager.Instance.SnakeGameInfo;
        float _updateInterval => GameManager.Instance.UpdateInterval;
        float _elapsedTimeBetweenUpdateInterval = 0;

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

        private void Update()
        {
            if (!_isInit) return;

            _elapsedTimeBetweenUpdateInterval += Time.deltaTime;
            float interpolationRatio = _elapsedTimeBetweenUpdateInterval / _updateInterval;

            if (_enableLerpPosition)
            {
                LerpSnakeBodyPositions(interpolationRatio);
                LerpSnakeFoodSize(interpolationRatio);
            }
            else
            {
                SetSnakeBodyPositions();
            }

            if (_enableLerpDirection)
            {
                LerpSnakeBodyDirections(interpolationRatio);
            }
            else
            {
                SetSnakeBodyDirections();
            }
        }

        private void LerpSnakeBodyPositions(float interpolationRatio)
        {
            for (int i = 0; i < _snakeBodyCurrentPositions.Count; i++)
            {
                Vector3 interpolatedPosition = Vector3.Lerp(_snakeBodyPreviousPositions[i], _snakeBodyCurrentPositions[i], interpolationRatio);
                _snakeBodyCubes[i].transform.localPosition = interpolatedPosition;

                if (_snakePartClones[i].activeSelf)
                {
                    Vector3 cloneInterpolatedPosition = Vector3.Lerp(_snakePartClonePreviousPositions[i], _snakePartCloneCurrentPositions[i], interpolationRatio);
                    _snakePartClones[i].transform.localPosition = cloneInterpolatedPosition;
                }
            }
        }

        private void LerpSnakeBodyDirections(float interpolationRatio)
        {
            for (int i = 0; i < _snakeBodyCurrentPositions.Count; i++)
            {
                if (i != 0 && _onlyLerpHeadDirection) break;

                Vector3 previousPoint = _snakeBodyCubes[i].transform.position + _snakeBodyPreviousDirections[i];
                Vector3 currentPoint = _snakeBodyCubes[i].transform.position + _snakeBodyCurrentDirections[i];

                Vector3 interpolatedPoint = Vector3.Lerp(previousPoint, currentPoint, interpolationRatio);
                _snakeBodyCubes[i].transform.LookAt(interpolatedPoint);

                // Rotate the clones even if they are not active, so when they are turned active there will not be any visual glitches
                Vector3 previousClonePoint = _snakePartClones[i].transform.position + _snakeBodyPreviousDirections[i];
                Vector3 currentClonePoint = _snakePartClones[i].transform.position + _snakeBodyCurrentDirections[i];

                Vector3 interpolatedClonePoint = Vector3.Lerp(previousClonePoint, currentClonePoint, interpolationRatio);
                _snakePartClones[i].transform.LookAt(interpolatedClonePoint);
            }
        }

        private void LerpSnakeFoodSize(float interpolationRatio)
        {
            if (!_snakeFoodClone.activeSelf) return;

            Vector3 originalScale = GameManager.Instance.FoodPrefab.transform.localScale;

            Vector3 interpolatedSize = Vector3.Lerp(originalScale / 2, originalScale, interpolationRatio);
            _snakeFood.transform.localScale = interpolatedSize;

            Vector3 cloneInterpolatedSize = Vector3.Lerp(originalScale, originalScale / 2, interpolationRatio);
            _snakeFoodClone.transform.localScale = cloneInterpolatedSize;
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
            _elapsedTimeBetweenUpdateInterval = 0;
            UpdateSnakeBodyVisuals();
            UpdateSnakeFoodVisuals();
        }

        private void UpdateSnakeBodyVisuals()
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

            _snakeBodyPreviousPositions = _snakeGame.GetPreviousSnakeBodyWorldSpacePositions();
            _snakeBodyCurrentPositions = _snakeGame.GetCurrentSnakeBodyWorldSpacePositions();

            _snakeBodyPreviousDirections = _snakeGame.GetPreviousSnakeBodyWorldSpaceDirections();
            _snakeBodyCurrentDirections = _snakeGame.GetCurrentSnakeBodyWorldSpaceDirections();

            if (_snakeBodyCubes.Count < _snakeBodyCurrentPositions.Count)
            {
                GameObject newBodyCube = Instantiate(GameManager.Instance.BodyPrefab);
                newBodyCube.transform.SetParent(_snakeBodyCubes[0].transform.parent);
                newBodyCube.transform.localPosition = _snakeBodyPreviousPositions[_snakeBodyPreviousPositions.Count - 1];
                SetupLayer(newBodyCube);
                SetupMaterialClipping(newBodyCube);

                _snakeBodyCubes.Add(newBodyCube);
            }

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

            for (int i = 0; i < _snakeBodyCurrentPositions.Count; i++)
            {
                Vector3Int previousDirection = _snakeBodyPreviousDirections[i];
                Vector3Int currentDirection = _snakeBodyCurrentDirections[i];

                // Handling when warping
                if (Vector3.Distance(_snakeBodyPreviousPositions[i], _snakeBodyCurrentPositions[i]) > 1 && _enableLerpPosition)
                {
                    _snakeBodyPreviousPositions[i] = _snakeBodyCurrentPositions[i] - currentDirection;

                    _snakePartClones[i].SetActive(true);
                    _snakePartClones[i].transform.localPosition = _snakeBodyCubes[i].transform.localPosition;

                    _snakePartClonePreviousPositions[i] = _snakeGame.GetPreviousSnakeBodyWorldSpacePositions()[i];
                    _snakePartCloneCurrentPositions[i] = _snakePartClonePreviousPositions[i] + currentDirection;
                }

                // Handling when turning corners
                if (currentDirection != previousDirection && _enableCornerClones)
                {
                    // Corner clones for all except the tail
                    if (i != _snakeBodyCurrentPositions.Count - 1)
                    {
                        _snakePartCornerClones[i].SetActive(true);
                        _snakePartCornerClones[i].transform.localPosition = _snakeGame.GetPreviousSnakeBodyWorldSpacePositions()[i];
                    }
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
            Renderer[] renderers = gameObjectToSetup.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                Material materialInstance = renderer.material;

                // Assumed a material that has "_X_Max_Plane_Position" will also have the rest of the parameters
                if (!materialInstance.HasVector("_X_Max_Plane_Position")) continue;

                materialInstance.SetVector("_X_Max_Plane_Position", new Vector3(transform.position.x - 0.5f, 0, 0));
                materialInstance.SetVector("_X_Min_Plane_Position", new Vector3(transform.position.x - 0.5f + GameManager.Instance.GameSize, 0, 0));

                materialInstance.SetVector("_Y_Max_Plane_Position", new Vector3(0, -0.5f + GameManager.Instance.GameSize, 0));

                materialInstance.SetVector("_Z_Max_Plane_Position", new Vector3(0, 0, transform.position.z - 0.5f));
                materialInstance.SetVector("_Z_Min_Plane_Position", new Vector3(0, 0, transform.position.z - 0.5f + GameManager.Instance.GameSize));
            }
        }

        private void UpdateSnakeFoodVisuals()
        {
            if (_snakeFoodClone.activeSelf)
            {
                _snakeFoodClone.SetActive(false);
            }

            // TODO : If game over, still make clone but also destroy the food
            _snakeFoodCurrentPosition = _snakeGame.GetCurrentSnakeFoodWorldSpacePosition();
            _snakeFoodPreviousPosition = _snakeGame.GetPreviousSnakeFoodWorldSpacePosition();

            if (_snakeFood.transform.localPosition != _snakeFoodCurrentPosition)
            {
                _snakeFood.transform.localPosition = _snakeFoodCurrentPosition;

                if (_enableLerpPosition)
                {
                    _snakeFoodClone.SetActive(true);
                    _snakeFoodClone.transform.SetParent(_snakeFood.transform.parent);
                    _snakeFoodClone.transform.localPosition = _snakeFoodPreviousPosition;
                }
            }
        }

        public class RequestUpdateVisualsEvent { }
        public class RequestInitEvent { }
    }
}
