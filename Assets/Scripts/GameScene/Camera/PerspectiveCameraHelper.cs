using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    /// <summary>
    /// Sets the local z position of the perspective camera position reference, 
    /// assuming the parent game object is positioned at the lower far end vertex of the game platform.
    /// </summary>
    public class PerspectiveCameraHelper : CustomMonoBehaviour
    {
        [Header("Perspective Camera Settings")]
        [SerializeField] float _zReference = -32f;

        GameManager _gameManager => GameManager.Instance;

        protected override void SubscribeToMessageHubEvents()
        {
            SubscribeToMessageHubEvent<SetLocalZEvent>((e) => SetLocalZ());
        }

        private void SetLocalZ()
        {
            float z = CalculateLocalZScaling(_zReference, _gameManager.GameSize);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }

        public static float CalculateLocalZScaling(float zReference, int gameSize)
        {
            return zReference * gameSize / GameManager.SizeReference;
        }

        /// <summary>
        /// Event to set the local Z position of the perspective camera position reference.
        /// </summary>
        public class SetLocalZEvent { }
    }
}

