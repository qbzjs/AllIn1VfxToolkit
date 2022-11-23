using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperLeftCube : MonoBehaviour
{
    GameScene.GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameScene.GameManager>();
        transform.position = new Vector3(transform.position.x, -0.5f + _gameManager.GameSize, transform.position.z);
    }
}
