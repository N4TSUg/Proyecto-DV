using System;
using UnityEngine;

public class limitesController : MonoBehaviour
{
    private Transform _playerTransform;

    void Update()
    {
        if (_playerTransform == null)
        {
            _playerTransform = GameObject.FindGameObjectWithTag(tagsClass.PLAYER).transform;
            if (_playerTransform == null)
            {
                return;
            }
        }

        Vector3 newPosition = _playerTransform.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

    }
}
