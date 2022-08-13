using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBehaviour<T> where T : MonoBehaviour
{
    public T prefab { get; private set; }
    public bool autoExpand { get; set; }
    public Transform container { get; }

    private List<T> _pool;

    public PoolBehaviour(T prefab, int count)
    {
        this.prefab = prefab;
        this.container = null;
        this.CreatePool(count);
    }

/*    public PoolBehaviour(T prefab, Transform container)
    {
        this.prefab = prefab;
        this.container = container;
        //this.CreatePool(count);
    }*/

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
        var createdObject = Object.Instantiate(this.prefab, this.container);
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
        if (this.autoExpand)
            return this.CreateObject(true);

        throw new System.Exception($"Element null");
    }
}