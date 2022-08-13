using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Game : MonoBehaviour
{ 
    [SerializeField] private Bullet _bulletPrefab;
 
    [SerializeField] private CameraTarget _cameraTarget;
    [SerializeField] public NavMeshSurface _navMesh;

    [SerializeField] private List<Level> _levels;

    [SerializeField] private int _countLevel;

    private Level _level;
    public PoolBehaviour<Bullet> Bullets;

    private void Start()
    {
        CreatePoolBullet();
        StartNewGame();
    }

    private void StartNewGame()
    {
        CreateLevel(_levels[_countLevel]);
        InitializePlayer();
        InitializeEnemy();
        _navMesh.BuildNavMesh();
    }

    private void CreateLevel(Level level)
    {
       _level = Instantiate(level);
    }

    private void InitializePlayer()
    {
        Player player = _level.GetComponentInChildren<Player>();
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

            Destroy(_level.gameObject);


        StartNewGame();
    }

    private void CreatePoolBullet()
    {
        Bullets = new PoolBehaviour<Bullet>(_bulletPrefab, 10);
    }
}
