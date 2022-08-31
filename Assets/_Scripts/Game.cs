using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Game : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private GenerateLevel _generateLevel;
    
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GameUI _gameUI;

    [SerializeField] private int _amountCreateBulletInPool;
    [SerializeField] private int _countLevel;

    [SerializeField] private List<Level> _levels;

    private Transform _playerTransform => _generateLevel.Player.transform;
    private GameObject _level => _generateLevel.Level.gameObject;


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
        InitializeUI();
        GenerateLevel();
        NavMesh.BuildNavMesh();
        InitializeCamera();
    }

    private void InitializeUI()
    {
        _gameUI.Initialize(this);
        _gameUI.StartPLayButton.SetActive(true);
    }

    private void GenerateLevel()
    {
        _generateLevel.Initialize(_levels[_countLevel], _camera, this);
    }

    public void RestartGame()
    {
        IsPlay = false;
        Bullets.DisablePool();
        _gameUI.WinButton.SetActive(false);
        Destroy(_level);
        StartNewGame();
    }

    private void InitializeCamera()
    {
        _virtualCamera.Follow = _playerTransform;
        _virtualCamera.LookAt = _playerTransform;
    }

    private void CreatePoolBullet()
    {
        Bullets = new PoolBehaviour<Bullet>(_bulletPrefab, _amountCreateBulletInPool);
    }
}
