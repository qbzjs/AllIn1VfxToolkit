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

        bool _isCameraSet = false;

        protected override void Start()
        {
            _mainCamera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            if (!_isCameraSet)
            {
                transform.position = CalculateFlatCameraWorldPosition(_platformMeshRenderer, _cameraHeight);

                if (_mainCamera != null)
                    _mainCamera.orthographicSize = CalculateFlatCameraOrthographicize(_gameManager.GameSize, _orthographicSizeFactor);

                _isCameraSet = true;
            }
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
    }
}
