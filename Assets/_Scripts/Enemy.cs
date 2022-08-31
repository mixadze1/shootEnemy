using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Anim;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyRagdoll))]
public class Enemy : MonoBehaviour
{
    private EnemyUI _enemyUI;
    private EnemyRagdoll _enemyRagdoll;

    private NavMeshPosition _navMeshPosition;
    private Animator _animator;

    public float MaxHealth;

    public float Health;

    public bool IsDie;

    public void Initialize(NavMeshPosition navMeshPosition)
    {
        GetNeedComponent();
       
        MaxHealth = Health;
        _navMeshPosition = navMeshPosition;
        _enemyRagdoll.Initialize();
        _enemyUI?.Initialize(this);
    }

    private void GetNeedComponent()
    {
        _enemyRagdoll = GetComponent<EnemyRagdoll>();
        _enemyUI = GetComponent<EnemyUI>();
        _animator = GetComponent<Animator>();
    }

    public void GetDamage(float damage)
    {
        _animator.Play(Anim.Enemy.IS_DAMAGE);

        Health -= damage;
        _enemyUI?.ChangeHealth();
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _enemyUI?.DisableUI();
        _animator.enabled = false;
        _enemyRagdoll.ActivateRagdoll();
        IsDie = true;
        _navMeshPosition.Enemies.Remove(this);
    }
}

