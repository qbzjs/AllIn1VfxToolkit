using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    [Header("Object Pool")]
    [SerializeField] GameObject _objectToPool;
    [SerializeField] int _initialAmountToPool;
    [SerializeField] bool _allowExceedInitialAmount = true;

    List<GameObject> _pooledObjects = new List<GameObject>();

    private void Start()
    {
        GameObject temp;
        for (int i = 0; i < _initialAmountToPool; i++)
        {
            temp = Instantiate(_objectToPool);
            temp.transform.SetParent(this.transform);
            temp.SetActive(false);
            _pooledObjects.Add(temp);
        }
    }

    public GameObject TakeFromPool()
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            GameObject pooledObject = _pooledObjects[i];
            if (!pooledObject.activeInHierarchy)
            {
                pooledObject.SetActive(true);
                return pooledObject;
            }
        }

        if (_allowExceedInitialAmount)
        {
            GameObject newPooledObject = Instantiate(_objectToPool);
            newPooledObject.transform.SetParent(this.transform);
            newPooledObject.SetActive(true);
            _pooledObjects.Add(newPooledObject);
            return newPooledObject;
        }

        return null;
    }

    public void ReturnToPool(GameObject objectToReturnToPool)
    {
        objectToReturnToPool.transform.SetParent(this.transform);
        objectToReturnToPool.SetActive(false);
        if (!_pooledObjects.Contains(objectToReturnToPool))
            _pooledObjects.Add(objectToReturnToPool);
    }
}