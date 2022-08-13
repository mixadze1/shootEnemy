using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Image _imageHealth;
    [SerializeField] private TextMeshProUGUI _textHealth;
    
    private Rigidbody[] _rigidbody;

    private NavMeshPosition _navMeshPosition;
    private Animator _animator;
    private Canvas _canvas;


    private float _offsetAngle = 180;
    private float _maxHealth;

    public float Health = 10;

    public bool IsDie;

    public const string IS_DAMAGE = "Damage";

    public void Initialize(NavMeshPosition navMeshPosition)
    {
        _textHealth.text = Health.ToString();
        _maxHealth = Health;
        _navMeshPosition = navMeshPosition;
        _canvas = GetComponentInChildren<Canvas>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in _rigidbody)
        {
            rb.isKinematic = true;
        }
        RotationCanvas();
    }

    public void GetDamage(float damage)
    {
        Health -= damage;
        _animator.Play(IS_DAMAGE);
        _imageHealth.fillAmount = Health / _maxHealth;
        _textHealth.text = Health.ToString();
        if (Health <= 0)
        {
            
            _canvas.gameObject.SetActive(false);
            Die();
        }
    }

    private void RotationCanvas()
    {
        Debug.Log(180f - transform.rotation.eulerAngles.y);
        _canvas.transform.localRotation = Quaternion.Euler(0, (_offsetAngle - transform.rotation.eulerAngles.y + _offsetAngle), 0);
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
    }

}

