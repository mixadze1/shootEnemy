using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private LayerMask _aimLayerMask;
    [SerializeField] private float _distanceShoot;
    [SerializeField] private Transform _bulletTransform;
    [SerializeField, Range(0.2f, 0.3f)] private float _prepareShoot = 0.25f;
    [SerializeField, Range(0.45f, 1f)] private float _timeToNextShoot = 0.45f;

    private Player _player;
    private Bullet _bulletSave;
    private Coroutine _coroutine;
    private Game _game;
    private Camera _camera;

    private bool _canShoot;
    private bool _isShoot;

    public bool CanShoot { get => _canShoot; set => _canShoot = value; }

    public void Initialize(Player player, Camera camera, Game game)
    {
        _game = game;
        _player = player;
        _camera = camera;
    }

    private void Update()
    {
        if (IsCanShoot())
        {
            Shoot(GetRay());
        }
    }

    private Ray GetRay()
    {
        return _camera.ScreenPointToRay(Input.mousePosition);
    }

    private void Shoot(Ray ray)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(PrepareShoot(ray));
    }

    private bool IsCanShoot()
    {
        if (Input.GetMouseButtonDown(0) && _canShoot && !_isShoot && _player.Position.
            Enemies.Count > 0)
            return true;
        return false;
    }


    private IEnumerator PrepareShoot(Ray ray)
    {
        _player.AnimationChange.ChangeAnimationState(Anim.Player.SHOOT);
        _isShoot = true;

        yield return new WaitForSeconds(_prepareShoot);
        InitializeBullet();

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _distanceShoot, _aimLayerMask))
        {
            BulletTrajectory(hitInfo);
            RotationPlayerToShoot(hitInfo);
        }
        yield return new WaitForSeconds(_timeToNextShoot);
        _isShoot = false;
    }

    private void InitializeBullet()
    {
        Bullet bullet = _bulletSave = _game.Bullets.GetFreeElement();

        ResetBullet(bullet);
        ResetBulletRigidbody(bullet);
        bullet.GetComponentInChildren<TrailRenderer>().Clear();
        bullet.gameObject.SetActive(true);
        bullet.Initialize();
    }

    private void ResetBullet(Bullet bullet)
    {
        bullet.transform.position = _bulletTransform.position;
        bullet.transform.rotation = new Quaternion(0, 0, 0, 0);
        bullet.Direction = Vector3.zero;
    }

    private void ResetBulletRigidbody(Bullet bullet)
    {
        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    private void BulletTrajectory(RaycastHit hitInfo)
    {
        var destination = hitInfo.point;
        var direction = (destination - _bulletSave.transform.position).normalized;
        _bulletSave.Direction = direction;
    }

    private void RotationPlayerToShoot(RaycastHit hitInfo)
    {
        var destination = hitInfo.point;

        destination.y = transform.position.y;
        var direction = destination - transform.position;
        direction.Normalize();

        Quaternion rotation = Quaternion.LookRotation(direction, transform.position);
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = rotation;
    }
}
