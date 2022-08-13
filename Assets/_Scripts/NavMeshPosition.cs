using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshPosition : MonoBehaviour
{
    public List<Enemy> Enemies = new List<Enemy>();
    public bool IsFinish;

    public void Initialize()
    {
       Enemy[] enemies = GetComponentsInChildren<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Enemies.Add(enemy);
        }
        foreach(Enemy enemy in Enemies)
        {
            enemy.Initialize(this);
        }
    }
}
