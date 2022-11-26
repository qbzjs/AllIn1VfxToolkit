using UnityEngine;

namespace GameScene
{
    public class CameraHolder : CustomMonoBehaviour
    {
        [SerializeField] GameManager _gameManager;
        [SerializeField] MeshRenderer _platformMeshRenderer;

        protected override void SubscribeToMessageHubEvents()
        {
            SubscribeToMessageHubEvent<SetPositionEvent>((e) => SetPosition());
        }

        private void SetPosition()
        {
            Vector3 setPosition = Vector3.zero;
            setPosition.x = _platformMeshRenderer.bounds.center.x; // Center the X
            setPosition.y = transform.position.y; // Dont change the Y
            setPosition.z = -5 + _gameManager.GameSize; // Z scales with game size, keep at the edge

            transform.position = setPosition;
        }

        public class SetPositionEvent { }
    }
}
