using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Anim;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(AnimationChange))]
public class Player : MonoBehaviour
{
    [SerializeField] private List<NavMeshPosition> _position;

    private NavMeshAgent _agent;
    private Game _game;
    private AnimationChange _animationChange;
    private PlayerShoot _playerShoot;
    private Finish _finish;

    private int _countClick;

    public bool IsMoving;

    public NavMeshPosition Position => _position[_countClick];

    public AnimationChange AnimationChange => _animationChange;

    public void Initialize(Game game, Camera camera)
    {
        _finish = new Finish();
        _game = game;
        GetNeedComponent();
        _playerShoot.Initialize(this,camera,_game);
    }

    private void GetNeedComponent()
    {
        _animationChange = GetComponent<AnimationChange>();
        _agent = GetComponent<NavMeshAgent>();
        _playerShoot = GetComponent<PlayerShoot>();
    }

    private void Update()
    {
        if (IsNextPosition())
        {
            if (Position.IsFinish)
                return;

            _countClick++;
            StartMovingPos();
        }
        CheckDestination();
    }

    private bool IsNextPosition()
    {
        if (_game.IsPlay && _agent.velocity.sqrMagnitude == 0f &&
                Position.Enemies.Count <= 0)
            return true;
        return false;
    }

    private void StartMovingPos()
    {  
        _animationChange.Animator.SetBool(Anim.Player.TO_IDLE, false);
        _animationChange.ChangeAnimationState(Anim.Player.WALKING);
        _agent.SetDestination(Position.transform.position); 
    }

    private void CheckDestination()
    {
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    _animationChange.Animator.SetBool(Anim.Player.TO_IDLE, true);
                    CheckFinish();
                    CheckEnemies();
                }
            }
            else
            {
                _playerShoot.CanShoot = false;
            }
        }
    }

    private void CheckEnemies()
    {
        if (Position.Enemies.Count > 0)
            _playerShoot.CanShoot = true;
    }

    private void CheckFinish()
    {
        if (Position.IsFinish)
            _finish.EndGame(_game);
    }
}
