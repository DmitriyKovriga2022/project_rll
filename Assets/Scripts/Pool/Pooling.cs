using System.Collections.Generic;
using UnityEngine;

public class Pooling
{
    public GameObject Prefab { get; }

    public Transform Container { get; }

    private List<GameObject> _pool;

    public Pooling(GameObject prefab, int count, Transform container)
    {
        Prefab = prefab;
        Container = container;

        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        _pool = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private GameObject CreateObject(bool isActive = false)
    {
        var createdObject = Object.Instantiate(Prefab, Container);
        createdObject.gameObject.SetActive(isActive);
        _pool.Add(createdObject);
        return createdObject;
    }

    public bool HasFreeElement(out GameObject element)
    {
        foreach (var mono in _pool)
        {
            if (!mono.gameObject.activeInHierarchy)
            {
                element = mono;
                return true;
            }
        }
        element = null;
        return false;
    }

    public GameObject GetFreeElement()
    {
        if (HasFreeElement(out var element))
        {
            return element;
        }
        
        return CreateObject();
    }
}