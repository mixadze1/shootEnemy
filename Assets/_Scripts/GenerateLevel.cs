using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    private Level _level;
    private Camera _camera;
    private Game _game;
    private Player _player;

    public Player Player => _player;
    public Level Level => _level;

    public void Initialize(Level level, Camera camera, Game game)
    {
        _game = game;
        _camera = camera;
        CreateLevel(level);
        InitializePlayer();
        InitializeEnemy();
    }

    private void CreateLevel(Level level)
    {
        _level = Instantiate(level);
    }

    private void InitializePlayer()
    {
        Player player = _player = _level.GetComponentInChildren<Player>();
        player.Initialize(_game, _camera);
    }

    private void InitializeEnemy()
    {
        var positions = _level.GetComponentsInChildren<NavMeshPosition>();
        foreach (var position in positions)
        {
            position.Initialize();
        }
    }
}
