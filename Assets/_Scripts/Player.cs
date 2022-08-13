using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(AnimationChange))]
public class Player : MonoBehaviour
{    
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _bulletTransform;
    [SerializeField] private LayerMask _aimLayerMask;
    [SerializeField] private List<NavMeshPosition> _position;
    private Game _game;
    private CameraTarget _camera;
    private Bullet _bulletSave;
    private Coroutine _coroutineRotate;
    private Coroutine _coroutineAnimShoot;
    private AnimationChange _animationChange;
   
    private int _countClick;

    public const string WALKING = "Walking";
    public const string SHOOT_IDLE = "ShootIdle";
    public const string SHOOT = "Shoot";
    public const string TO_IDLE = "ToIdle";

    private bool _canShoot;

    private bool _isShoot;

    public bool IsMoving;

    public void Initialize(CameraTarget cameraTarget, Game game)
    {
        _game = game;
        _camera = cameraTarget;
        _animationChange = GetComponent<AnimationChange>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _agent.velocity.sqrMagnitude == 0f && 
            _position[_countClick].gameObject.GetComponent<NavMeshPosition>().
                        Enemies.Count <= 0)
        {
            if (_position[_countClick].IsFinish)
                return;

            _camera.IsShotPos = false;
            _countClick++;
            StartMovingPos();
            
        }
        if (Input.GetMouseButtonDown(0) && _canShoot && !_isShoot && _position[_countClick].gameObject.GetComponent<NavMeshPosition>().
                        Enemies.Count > 0)
        {
            Shoot();
        }
            CheckDestination();
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
                    _camera.IsShotPos = true;
                    _animationChange.Animator.SetBool(TO_IDLE, true);
                    if (_position[_countClick].gameObject.GetComponent<NavMeshPosition>().
                        Enemies.Count > 0)
                        _canShoot = true;
                    else
                    {
                        _canShoot = false;
                    }
                }
            }
        }
    }

    private void Shoot()
    {
        if (_coroutineAnimShoot != null)
            StopCoroutine(_coroutineAnimShoot);
        _coroutineAnimShoot = StartCoroutine(PrepareShoot());
    }

    private IEnumerator PrepareShoot()
    {
        _animationChange.ChangeAnimationState(SHOOT); 
        _isShoot = true;
        
        yield return new WaitForSeconds(0.25f);
        Bullet bullet = _bulletSave = _game.Bullets.GetFreeElement();
        bullet.transform.position = _bulletTransform.position;
        bullet.Initialize();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _aimLayerMask))
        {
            RotationPlayer(hitInfo);
            BulletFly(hitInfo);
        }
        yield return new WaitForSeconds(0.45f);
        _isShoot = false;
    }

    private void BulletFly(RaycastHit hitInfo)
    {
        var destination = hitInfo.point;
        var direction = (destination - _bulletTransform.position).normalized;
        _bulletSave.Direction = direction;
    }

    private void RotationPlayer(RaycastHit hitInfo)
    {
        var destination = hitInfo.point;

        destination.y = transform.position.y;
        var direction = destination - transform.position; 
         direction.Normalize();

        Quaternion rotation =  Quaternion.LookRotation(direction, transform.position);
        rotation.x = 0;
        rotation.z = 0;
        if (_coroutineRotate != null)
            StopCoroutine(_coroutineRotate);
      _coroutineRotate =   StartCoroutine(RotationPlayer( rotation));
    }

    private IEnumerator RotationPlayer(Quaternion rotation)
    {
        while( _position[_countClick].gameObject.GetComponent<NavMeshPosition>().
                        Enemies.Count > 0)
        {
            yield return new WaitForSeconds(0.02f);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.5f);
        }
       
    }
}
