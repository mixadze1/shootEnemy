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
        GetNeedComponent();
        _textHealth.text = Health.ToString();
        _maxHealth = Health;
        _navMeshPosition = navMeshPosition;
        foreach (Rigidbody rb in _rigidbody)
        {
            rb.isKinematic = true;
        }
        RotationCanvas();
    }

    private void GetNeedComponent()
    {
        _canvas = GetComponentInChildren<Canvas>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponentsInChildren<Rigidbody>();
    }

    public void GetDamage(float damage)
    {
        _animator.Play(IS_DAMAGE);

        Health -= damage;
        _imageHealth.fillAmount = Health / _maxHealth;
        _textHealth.text = Health.ToString();

        if (Health <= 0)
        {
            Die();
        }
    }

    private void RotationCanvas()
    {
        _canvas.transform.localRotation = Quaternion.Euler(0, (_offsetAngle - transform.rotation.eulerAngles.y + _offsetAngle), 0);
    }

    private void Die()
    {
        _canvas.gameObject.SetActive(false);
        _animator.enabled = false;
        foreach (Rigidbody rb in _rigidbody)
        {
            rb.isKinematic = false;
        }
        IsDie = true;
        _navMeshPosition.Enemies.Remove(this);
    }
}

