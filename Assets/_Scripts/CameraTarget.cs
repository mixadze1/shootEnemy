using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Vector3 _distanceForNotShoot;
    [SerializeField] private float _timeToPosNotShoot;
    [SerializeField] private Vector3 _distanceForShoot;
    [SerializeField] private float _timeToPosShoot;

    public bool IsShotPos;

    public void Initialize(Player player)
    {
        _player = player;
    }

    private void LateUpdate()
    {
        if (IsShotPos)
        {
            CameraPosition(_distanceForShoot, _timeToPosShoot);
        }
        else
        {
            CameraPosition(_distanceForNotShoot, _timeToPosNotShoot);
        }

    }
    private void CameraPosition(Vector3 distance, float timeShoot)
    {
        Vector3 positionToGo = _player.transform.position + distance;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, positionToGo, timeShoot);
        transform.position = smoothPosition;
    }
}
