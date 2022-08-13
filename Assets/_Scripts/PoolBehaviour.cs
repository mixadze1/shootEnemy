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
        this.Prefab = prefab;
        this.Container = null;
        this.CreatePool(count);
    }

    private void CreatePool(int count)
    {
        this._pool = new List<T>();
        for (int i = 0; i < count; i++)
        {
            this.CreateObject();
        }
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createdObject = Object.Instantiate(this.Prefab, this.Container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        this._pool.Add(createdObject);
        return createdObject;
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
        if (this.HasFreeElement(out var element))
        {
                return element;
        }
            return this.CreateObject(true);
    }
}