using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damage = 15;

    public Vector3 Direction;

    private void Update()
    {
        transform.Translate(_speed * Direction * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Enemy>())
        {
            TakeDamage((collision.gameObject.GetComponentInParent<Enemy>()));
        }
        Destroy(gameObject);
    }

    private void TakeDamage(Enemy enemy)
    {
        enemy.GetDamage(_damage);
    }
}