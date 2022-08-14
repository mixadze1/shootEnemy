using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Game : MonoBehaviour
{ 
    [SerializeField] private Bullet _bulletPrefab;
    
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private List<Level> _levels;

    [SerializeField] private GameObject _startPlayButton;
    [SerializeField] private GameObject _winButton;

    [SerializeField] private int _amountCreateBulletInPool;
    [SerializeField] private int _countLevel;

    private Player _player;
    private Level _level;
    public NavMeshSurface NavMesh;
    public PoolBehaviour<Bullet> Bullets;

    public bool IsPlay;

    private void Start()
    {
        CreatePoolBullet();
        StartNewGame();
    }

    private void StartNewGame()
    {
        _startPlayButton.SetActive(true);
        CreateLevel(_levels[_countLevel]);
        InitializePlayer();
        InitializeEnemy();
        NavMesh.BuildNavMesh();
        InitializeCamera();
    }

    public void RestartGame()
    {
        IsPlay = false;
        Bullets.DisablePool();
        _winButton.SetActive(false);
        Destroy(_level.gameObject);
        StartNewGame();
    }

    private void InitializeCamera()
    {
        _virtualCamera.Follow = _player.transform;
        _virtualCamera.LookAt = _player.transform;
    }

    private void CreateLevel(Level level)
    {
       _level = Instantiate(level);
    }

    private void InitializePlayer()
    {
        Player player = _player = _level.GetComponentInChildren<Player>();
        player.Initialize(this);
    }

    private void InitializeEnemy()
    {
        var positions = _level.GetComponentsInChildren<NavMeshPosition>();
        foreach (var position in positions)
        {
            position.Initialize();
        }
    }

    private void CreatePoolBullet()
    {
        Bullets = new PoolBehaviour<Bullet>(_bulletPrefab, _amountCreateBulletInPool);
    }

    #region UI
    public void StartPlay()
    {
        IsPlay = true;
        _startPlayButton.SetActive(false);
    }

    public void Win()
    {
        _winButton.SetActive(true);
    }
    #endregion
}
