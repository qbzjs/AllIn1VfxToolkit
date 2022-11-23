using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class PerspectiveCameraZScaling : MonoBehaviour
    {
        [SerializeField] GameManager _gameManager;

        const int SIZE_REFERENCE = 10;
        const float Z_REFERENCE = -32f;

        void Start()
        {
            float z = Z_REFERENCE * _gameManager.GameSize / SIZE_REFERENCE;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }
    }
}

