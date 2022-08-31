using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    private Rigidbody[] _rigidbodyRagdoll;

    public void Initialize()
    {
        _rigidbodyRagdoll = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in _rigidbodyRagdoll)
        {
            rb.isKinematic = true;
        }
    }

    public void ActivateRagdoll()
    {
        foreach (Rigidbody rb in _rigidbodyRagdoll)
        {
            rb.isKinematic = false;
        }
    }
}
