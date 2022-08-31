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

    private void FixedUpdate()
    {
        transform.Translate(_speed * Direction * Time.deltaTime);
        CheckDistance();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        if (enemy)
        {
            TakeDamage(enemy);
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