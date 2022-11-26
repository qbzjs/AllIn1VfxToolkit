using UnityEngine;

namespace GameScene
{
    /// <summary>
    /// Sets the position and orthographic size of the orthographic camera based on the platform and the game size.
    /// </summary>
    public class OrthoCameraHelper : CustomMonoBehaviour
    {
        [SerializeField] GameManager _gameManager;
        [SerializeField] MeshRenderer _platformMeshRenderer;
        [SerializeField] float _cameraHeight = 20f; // The intended fixed y position
        [SerializeField] float _orthographicSizeFactor = 1; // Converts game size to orthographic size

        Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = GetComponent<Camera>();
        }

        protected override void SubscribeToMessageHubEvents()
        {
            SubscribeToMessageHubEvent<InitOrthoCameraEvent>((e) => InitOrthoCamera());
        }

        private void InitOrthoCamera()
        {
            transform.position = CalculateFlatCameraWorldPosition(_platformMeshRenderer, _cameraHeight);

            if (_mainCamera != null)
                _mainCamera.orthographicSize = CalculateFlatCameraOrthographicize(_gameManager.GameSize, _orthographicSizeFactor);

            DebugLog(transform.position);
        }

        public static Vector3 CalculateFlatCameraWorldPosition(MeshRenderer platformMeshRenderer, float cameraHeight)
        {
            Vector3 cameraPosition = platformMeshRenderer.bounds.center;
            cameraPosition.y = cameraHeight;
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
    }
}
