using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private NavMeshPosition _navMeshPosition;
    [SerializeField] private Level _level;
    [SerializeField] private Bullet _bulletPrefab;

    public List<Bullet> Bullets = new List<Bullet>();

    private void Start()
    {
        
    }

    private void StartNewGame()
    {
       // CreatePull
    }
}
