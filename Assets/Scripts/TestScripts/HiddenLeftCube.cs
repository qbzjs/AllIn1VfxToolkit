using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenLeftCube : MonoBehaviour
{
    GameScene.GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameScene.GameManager>();
        transform.position = new Vector3(transform.position.x, transform.position.y, -5 + _gameManager.GameSize);
    }
}
