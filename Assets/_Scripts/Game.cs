using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Game : MonoBehaviour
{ 
    [SerializeField] private Bullet _bulletPrefab;
    
    [SerializeField] private CameraTarget _cameraTarget;
    [SerializeField] public NavMeshSurface _navMesh;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private List<Level> _levels;

    [SerializeField] private GameObject _startPlayButton;
    [SerializeField] private GameObject _winButton;

    [SerializeField] private int _countLevel;

    private Player _player;
    private Level _level;
    public PoolBehaviour<Bullet> Bullets;

    public bool IsPlay;

    private void Start()
    {
        CreatePoolBullet();
        StartNewGame();

    }

    public void StartPlay()
    {
        IsPlay = true;
        _startPlayButton.SetActive(false);
    }

    public void Win()
    {
        _winButton.SetActive(true);
    }

    private void InitializeCamera()
    {
        _virtualCamera.Follow = _player.transform;
        _virtualCamera.LookAt = _player.transform;
    }

    private void StartNewGame()
    {
        _startPlayButton.SetActive(true);
        CreateLevel(_levels[_countLevel]);
        InitializePlayer();
        InitializeEnemy();
        _navMesh.BuildNavMesh();
        InitializeCamera();
    }

    private void CreateLevel(Level level)
    {
       _level = Instantiate(level);
    }

    private void InitializePlayer()
    {
        Player player = _player = _level.GetComponentInChildren<Player>();
        player.Initialize(_cameraTarget, this);
        _cameraTarget.Initialize(player);
    }

    private void InitializeEnemy()
    {
        var positions = _level.GetComponentsInChildren<NavMeshPosition>();
        foreach (var position in positions)
        {
            position.Initialize();
        }
    }

   

    public void RestartGame()
    {
        IsPlay = false;
        _winButton.SetActive(false);
        Destroy(_level.gameObject);
        StartNewGame();
    }

    private void CreatePoolBullet()
    {
        Bullets = new PoolBehaviour<Bullet>(_bulletPrefab, 10);
    }
}
