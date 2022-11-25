using UnityEngine;

namespace GameScene
{
    public class CameraManager : CustomMonoBehaviour
    {
        [SerializeField] Camera _mainCamera;
        [SerializeField] GameManager _gameManager;
        [SerializeField] MeshRenderer _platformMeshRenderer;
        [SerializeField] float _cameraHeight = 20f; // The intended fixed y position
        [SerializeField] float _orthographicSizeFactor = 1; // Converts game size to orthographic size

        bool _isCameraSet = false;

        private void LateUpdate()
        {
            if (!_isCameraSet)
            {
                _mainCamera.transform.position = CalculateFlatCameraWorldPosition(_platformMeshRenderer, _cameraHeight);
                _mainCamera.orthographicSize = CalculateFlatCameraOrthographicize(_gameManager.GameSize, _orthographicSizeFactor);

                _isCameraSet = true;
            }
        }

        public static Vector3 CalculateFlatCameraWorldPosition(MeshRenderer platformMeshRenderer, float cameraHeight)
        {
            Vector3 cameraPosition = platformMeshRenderer.bounds.center;
            cameraPosition.y = cameraHeight;
            // cameraPosition.y = _mainCamera.transform.position.y;
            return cameraPosition;
        }

        public static float CalculateFlatCameraOrthographicize(int gameSize, float orthographicSizeFactor)
        {
            return gameSize * orthographicSizeFactor;
        }
    }
}
