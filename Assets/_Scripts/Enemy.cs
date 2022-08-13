using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody[] _rigidbody;

    private NavMeshPosition _navMeshPosition;
    private Animator _animator;
  
    public float Health = 10;

    public bool IsDie;

    public void Initialize(NavMeshPosition navMeshPosition)
    {
        _navMeshPosition = navMeshPosition;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in _rigidbody)
        {
            rb.isKinematic = true;
        }
    }

    public void GetDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _animator.enabled = false;
        foreach (Rigidbody rb in _rigidbody)
        {
            rb.isKinematic = false;
        }
        IsDie = true;
        _navMeshPosition.Enemies.Remove(this);
       Debug.Log(_navMeshPosition.Enemies.Count) ;
    }

}

