using UnityEngine;

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

        GameManager _gameManager => GameManager.Instance;
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
            transform.position = CalculateFlatCameraWorldPosition(_platformMeshRenderer, _cameraHeight, _gameManager.GameSize);

            if (_mainCamera != null)
                _mainCamera.orthographicSize = CalculateFlatCameraOrthographicize(_gameManager.GameSize, _orthographicSizeFactor);
        }

        public static Vector3 CalculateFlatCameraWorldPosition(MeshRenderer platformMeshRenderer, float cameraHeight, int gameSize)
        {
            Vector3 cameraPosition = platformMeshRenderer.bounds.center;
            cameraPosition.y = cameraHeight * gameSize / GameManager.SizeReference; // TODO : This should scale with game size
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
