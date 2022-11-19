using UnityEngine;

namespace GameScene
{
    public class CameraManager : CustomMonoBehaviour
    {
        [SerializeField] Camera _mainCamera;
        [SerializeField] GameManager _gameManager;
        [SerializeField] MeshRenderer _platformMeshRenderer;
        [SerializeField] float _orthographicSizeFactor = 1;

        bool _isCameraSet = false;

        private void LateUpdate()
        {
            if (!_isCameraSet)
            {
                // Calculate camera position
                Vector3 cameraPosition = _platformMeshRenderer.bounds.center;
                cameraPosition.y = _mainCamera.transform.position.y;
                _mainCamera.transform.position = cameraPosition;

                // Calculate camera orthgraphic size
                _mainCamera.orthographicSize = _gameManager.GameSize * _orthographicSizeFactor;

                _isCameraSet = true;
            }
        }
    }
}
