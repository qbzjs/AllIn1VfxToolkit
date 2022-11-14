using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerRightCube : MonoBehaviour
{
    [SerializeField] GameScene.GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-5 + _gameManager.GameSize, transform.position.y, transform.position.z);
    }
}
