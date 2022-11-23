using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperRightCube : MonoBehaviour
{
    GameScene.GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameScene.GameManager>();
        transform.position = new Vector3(-5 + _gameManager.GameSize, -0.5f + _gameManager.GameSize, transform.position.z);
    }
}
