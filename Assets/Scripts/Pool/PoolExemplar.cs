using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolExemplar : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _poolCount;
    [SerializeField] private Transform _container;

    private Pooling _pool;

    void Awake()
    {
        _pool = new(_prefab, _poolCount, _container);
    }

    public GameObject CreateObject(Vector2 position, Quaternion rotation)
    {
        GameObject element = _pool.GetFreeElement();
        element.transform.position = position;
        element.transform.rotation = rotation;
        element.SetActive(true);
        return element;
    }
}
