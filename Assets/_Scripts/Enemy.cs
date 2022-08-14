using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Image _imageHealth;
    [SerializeField] private TextMeshProUGUI _textHealth;
    [SerializeField] private Canvas _canvasForHealth;

    private Rigidbody[] _rigidbodyRagdoll;

    private NavMeshPosition _navMeshPosition;
    private Animator _animator;


    private float _offsetAngleForCanvas = 180;
    private float _maxHealth;

    public float Health;

    public bool IsDie;

    public const string IS_DAMAGE = "Damage";

    public void Initialize(NavMeshPosition navMeshPosition)
    {
        GetNeedComponent();
        _textHealth.text = Health.ToString();
        _maxHealth = Health;
        _navMeshPosition = navMeshPosition;
        foreach (Rigidbody rb in _rigidbodyRagdoll)
        {
            rb.isKinematic = true;
        }
        RotationCanvas();
    }

    private void GetNeedComponent()
    {
        _animator = GetComponent<Animator>();
        _rigidbodyRagdoll = GetComponentsInChildren<Rigidbody>();
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
        _canvasForHealth.transform.localRotation = Quaternion.Euler(0, (_offsetAngleForCanvas - transform.rotation.eulerAngles.y + _offsetAngleForCanvas), 0);
    }

    private void Die()
    {
        _canvasForHealth.gameObject.SetActive(false);
        _animator.enabled = false;
        foreach (Rigidbody rb in _rigidbodyRagdoll)
        {
            rb.isKinematic = false;
        }
        IsDie = true;
        _navMeshPosition.Enemies.Remove(this);
    }
}

