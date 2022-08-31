using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using System;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Bullet _bulletPrefab;
    
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameUI _gameUI;

    [SerializeField] private int _amountCreateBulletInPool;
    [SerializeField] private int _countLevel;

    [SerializeField] private List<Level> _levels;
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
        _gameUI.Initialize(this);
        _gameUI.StartPLayButton.SetActive(true);
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
        _gameUI.WinButton.SetActive(false);
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
        player.Initialize(this, _camera);
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
}
