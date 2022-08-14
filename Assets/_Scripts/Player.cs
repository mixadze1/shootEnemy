using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(AnimationChange))]
public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _aimLayerMask;

    [SerializeField] private Transform _bulletTransform; 

    [SerializeField] private List<NavMeshPosition> _position;

    [SerializeField] private float _distanceShoot;

    [SerializeField, Range(0.2f,0.3f)] private float _prepareShoot = 0.25f;
    [SerializeField,Range (0.45f,1f)] private float _timeToNextShoot = 0.45f;

    private NavMeshAgent _agent;
    private Game _game;
    private Bullet _bulletSave;
    private Coroutine _coroutine;
    private AnimationChange _animationChange;
   
    private int _countClick;

    private bool _canShoot;
    private bool _isShoot;

    public bool IsMoving;

    public const string WALKING = "Walking";
    public const string SHOOT_IDLE = "ShootIdle";
    public const string SHOOT = "Shoot";
    public const string TO_IDLE = "ToIdle";

    public void Initialize(Game game)
    {
        _game = game;
        _animationChange = GetComponent<AnimationChange>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (IsNextPosition())
        {
            if (_position[_countClick].IsFinish)
                return;

            _countClick++;
            StartMovingPos();
        }

        if (IsCanShoot())
        {
            Shoot(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
        CheckDestination();
    }

    private bool IsCanShoot()
    {
        if (Input.GetMouseButtonDown(0) && _canShoot && !_isShoot && _position[_countClick].
            Enemies.Count > 0)
            return true;
        return false;
    }

    private bool IsNextPosition()
    {
        if (_game.IsPlay && _agent.velocity.sqrMagnitude == 0f &&
                _position[_countClick].Enemies.Count <= 0)
            return true;
        return false;
    }

    private void StartMovingPos()
    {  
        _animationChange.Animator.SetBool(TO_IDLE, false);
        _animationChange.ChangeAnimationState(WALKING);
        _agent.SetDestination(_position[_countClick].transform.position); 
    }

    private void CheckDestination()
    {
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    _animationChange.Animator.SetBool(TO_IDLE, true);
                    CheckFinish();
                    CheckEnemies();
                }
            }
            else
            {
                _canShoot = false;
            }
        }
    }

    private void CheckEnemies()
    {
        if (_position[_countClick]. Enemies.Count > 0)
            _canShoot = true;
    }

    private void CheckFinish()
    {
        if (_position[_countClick].IsFinish)
            _game.RestartGame();
    }

    private void Shoot(Ray ray)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(PrepareShoot(ray));
    }

    private IEnumerator PrepareShoot(Ray ray)
    {
        _animationChange.ChangeAnimationState(SHOOT); 
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

        Quaternion rotation =  Quaternion.LookRotation(direction, transform.position);
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = rotation;
    }
}
