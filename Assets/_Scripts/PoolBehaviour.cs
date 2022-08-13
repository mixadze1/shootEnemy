using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBehaviour<T> where T : MonoBehaviour
{
    private List<T> _pool;

    public T Prefab { get; private set; }

    public Transform Container { get; }

    public PoolBehaviour(T prefab, int count)
    {
        Prefab = prefab;
        Container = null;
        CreatePool(count);
    }

    private void CreatePool(int count)
    {
       _pool = new List<T>();
        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createdObject = Object.Instantiate(Prefab, Container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        _pool.Add(createdObject);
        return createdObject;
    }

    public void DisablePoolBullet()
    {
        foreach (var pool in _pool)
        {
            pool.GetComponent<Bullet>().Direction = Vector3.zero;
            pool.gameObject.SetActive(false);
        }
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var pool in _pool)
        {
            if (!pool.gameObject.activeInHierarchy)
            {
                element = pool;
                pool.gameObject.SetActive(true);
                return true;
            }
        }
        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out var element))
        {
            return element;
        }
        return CreateObject(true);
    }
}