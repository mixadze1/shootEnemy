using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private float _maxDistance;

    private Vector3 _positionBullet;

    public Vector3 Direction;

    public void Initialize()
    {
        _positionBullet = transform.position;
    }

    private void Update()
    {
        transform.Translate(_speed * Direction * Time.deltaTime);
        CheckDistance();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Enemy>())
        {
            TakeDamage((collision.gameObject.GetComponentInParent<Enemy>()));
        }
        Direction = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void TakeDamage(Enemy enemy)
    {
        enemy.GetDamage(_damage);
    }
    
    private void CheckDistance()
    {
        if (Vector3.Distance(transform.position, _positionBullet) > _maxDistance)
        {
            Direction = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}