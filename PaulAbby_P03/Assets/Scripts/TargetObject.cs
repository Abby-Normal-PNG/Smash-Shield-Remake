using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetObject : MonoBehaviour
{
    [SerializeField] Transform _target = null;
    [SerializeField] float _targetingDistance = 20f;

    public event Action TargetFound = delegate { };
    public event Action TargetLost = delegate { };

    private void FixedUpdate()
    {
        if (TargetIsClose())
        {
            transform.LookAt(_target);
            TargetFound?.Invoke();
        }
        else
        {
            TargetLost?.Invoke();
        }
    }

    private bool TargetIsClose()
    {
        float currentDistance = Vector3.Distance(_target.position, transform.position);
        if (currentDistance < _targetingDistance)
        {
            return true;
        }
        return false;
    }
}
