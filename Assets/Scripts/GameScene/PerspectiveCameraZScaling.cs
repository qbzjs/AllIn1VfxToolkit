using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class PerspectiveCameraZScaling : MonoBehaviour
    {
        [SerializeField] GameManager _gameManager;
        [SerializeField] float _zReference = -32f;

        const int SIZE_REFERENCE = 10;

        void Start()
        {
            float z = _zReference * _gameManager.GameSize / SIZE_REFERENCE;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }
    }
}

