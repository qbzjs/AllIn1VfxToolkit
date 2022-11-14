using UnityEngine;

namespace GameScene
{
    public class CameraManager : CustomMonoBehaviour
    {
        [SerializeField] Camera _mainCamera;
        [SerializeField] GameManager _gameManager;
        [SerializeField] MeshRenderer _platformMeshRenderer;

        bool _isCameraSet = false;

        private void Start()
        {
            Vector3 lowerLeftPoint = _mainCamera.WorldToScreenPoint(new Vector3(-5, -0.5f, -5));
            Vector3 lowerRightPoint = _mainCamera.WorldToScreenPoint(new Vector3(5, -0.5f, -5));

            DebugLog(lowerLeftPoint);
            DebugLog(lowerRightPoint);
        }

        private void LateUpdate()
        {
            if (!_isCameraSet)
            {
                // Calculate camera position
                Vector3 cameraPosition = _platformMeshRenderer.bounds.center;
                cameraPosition.y = _mainCamera.transform.position.y;
                _mainCamera.transform.position = cameraPosition;

                // Calculate camera orthgraphic size
                // TODO: temporarily arbitrarily set to the game size
                _mainCamera.orthographicSize = _gameManager.GameSize;

                _isCameraSet = true;
            }
        }

        protected override void SubscribeToMessageHubEvents()
        {

        }
    }
}
