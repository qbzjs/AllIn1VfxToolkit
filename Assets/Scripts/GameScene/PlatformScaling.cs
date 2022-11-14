using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class PlatformScaling : MonoBehaviour
    {
        [SerializeField] GameManager _gameManager;

        void Start()
        {
            float position = (_gameManager.GameSize - 1f) / 2f; // TODO : still not correct
            float scale = _gameManager.GameSize / 10f;

            transform.localPosition = new Vector3(position, transform.localPosition.y, position);
            transform.localScale = new Vector3(scale, transform.localScale.y, scale);
        }
    }
}

