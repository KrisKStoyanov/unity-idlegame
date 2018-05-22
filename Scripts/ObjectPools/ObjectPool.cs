using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public GameObject pooledObject;
    public int pooledAmount;
    public List<GameObject> pooledObjects;
    public Transform pooledObjectHost;

    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int objectIndex = 0; objectIndex < pooledAmount; objectIndex++)
        {
            CreateObject();
        }
    }

    public GameObject GetPooledObject()
    {
        for (int objectIndex = 0; objectIndex < pooledObjects.Count; objectIndex++)
        {
            if (!pooledObjects[objectIndex].activeInHierarchy)
            {
                return pooledObjects[objectIndex];
            }
        }

        return CreateObject();
    }

    GameObject CreateObject()
    {
        GameObject _gameObject = (GameObject)Instantiate(pooledObject);
        _gameObject.transform.SetParent(pooledObjectHost, false);
        _gameObject.SetActive(false);
        pooledObjects.Add(_gameObject);
        return _gameObject;
    }
}
