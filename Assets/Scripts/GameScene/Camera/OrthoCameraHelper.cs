using UnityEngine;
using DG.Tweening;

namespace GameScene
{
    /// <summary>
    /// Sets the position and orthographic size of the orthographic camera based on the platform and the game size.
    /// </summary>
    public class OrthoCameraHelper : CustomMonoBehaviour
    {
        [SerializeField] MeshRenderer _platformMeshRenderer;
        [SerializeField] float _cameraHeight = 20f; // The intended fixed y position
        [SerializeField] float _orthographicSizeFactor = 1; // Converts game size to orthographic size

        [Header("ZW Plane Settings")]
        [SerializeField] bool _isZWPlane = false;
        [SerializeField] MeshRenderer _originReference;
        [SerializeField] float _transitionDuration;
        [SerializeField] GameObject _platformExtension;

        GameManager _gameManager => GameManager.Instance;
        Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = GetComponent<Camera>();
        }

        protected override void SubscribeToMessageHubEvents()
        {
            SubscribeToMessageHubEvent<InitOrthoCameraEvent>((e) => InitOrthoCamera());
            SubscribeToMessageHubEvent<TransitionTo4DViewEvent>((e) => { if (_isZWPlane) TransitionTo4DView(); });
        }

        private void InitOrthoCamera()
        {
            if (!_isZWPlane || _gameManager.GameDimension == Snake4D.Dimension.DimensionFour)
                transform.position = CalculateOrthoCameraWorldPosition(_platformMeshRenderer, _cameraHeight, _gameManager.GameSize);
            else if (_gameManager.GameDimension == Snake4D.Dimension.DimensionThree)
                Set3DView();

            // TODO : 2D view?
            else if (_gameManager.GameDimension == Snake4D.Dimension.DimensionTwo)
                Set3DView();

            if (_mainCamera != null)
                _mainCamera.orthographicSize = CalculateFlatCameraOrthographicize(_gameManager.GameSize, _orthographicSizeFactor);
        }

        private void Set3DView()
        {
            _platformExtension.SetActive(true);

            Vector3 platformCenter = _platformMeshRenderer.bounds.center;
            Vector3 originCenter = _originReference.bounds.center;
            Vector3 orthoPosition = CalculateOrthoCameraWorldPosition(_platformMeshRenderer, _cameraHeight, _gameManager.GameSize);

            Vector3 targetPosition = new Vector3(originCenter.x, orthoPosition.y, platformCenter.z);
            transform.position = targetPosition;
        }

        private void TransitionTo4DView()
        {
            DebugLog("TransitionTo4DView...");
            Vector3 targetPosition = CalculateOrthoCameraWorldPosition(_platformMeshRenderer, _cameraHeight, _gameManager.GameSize);
            transform.DOMove(targetPosition, _transitionDuration).SetEase(Ease.InOutQuad);
        }

        public static Vector3 CalculateOrthoCameraWorldPosition(MeshRenderer referenceMeshRenderer, float cameraHeight, int gameSize)
        {
            Vector3 cameraPosition = referenceMeshRenderer.bounds.center;
            cameraPosition.y = cameraHeight * gameSize / GameManager.SizeReference;
            return cameraPosition;
        }

        public static float CalculateFlatCameraOrthographicize(int gameSize, float orthographicSizeFactor)
        {
            return gameSize * orthographicSizeFactor;
        }

        /// <summary>
        /// Event to initialize orthographic camera world position and orthographic size.
        /// </summary>
        public class InitOrthoCameraEvent { }

        public class TransitionTo4DViewEvent { }
    }
}
