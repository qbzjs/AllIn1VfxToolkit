using UnityEngine;

namespace GameScene
{
    public class CameraManager : CustomMonoBehaviour
    {
        [SerializeField] Camera _mainCamera;

        private void Start()
        {
            Vector3 lowerLeftPoint = _mainCamera.WorldToScreenPoint(new Vector3(-5, -0.5f, -5));
            Vector3 lowerRightPoint = _mainCamera.WorldToScreenPoint(new Vector3(5, -0.5f, -5));

            DebugLog(lowerLeftPoint);
            DebugLog(lowerRightPoint);
        }

        protected override void SubscribeToMessageHubEvents()
        {

        }
    }
}
