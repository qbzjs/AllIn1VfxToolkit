using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    /// <summary>
    /// Sets the local z position of the perspective camera reference, 
    /// assuming the parent game object is positioned at the lower far end vertex of the game platform.
    /// </summary>
    public class PerspectiveCameraHelper : CustomMonoBehaviour
    {
        public static int SizeReference => SIZE_REFERENCE;
        public const int SIZE_REFERENCE = 10;

        [SerializeField] GameManager _gameManager;

        [Header("Perspective Camera Settings")]
        [SerializeField] float _zReference = -32f;

        protected override void Start()
        {
            base.Start();

            float z = CalculateLocalZScaling(_zReference, _gameManager.GameSize);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }

        public static float CalculateLocalZScaling(float zReference, int gameSize)
        {
            return zReference * gameSize / SizeReference;
        }
    }
}

