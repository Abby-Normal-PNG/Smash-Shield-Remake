using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BulletMotor : MonoBehaviour
{
    [SerializeField] float _bulletSpeed = 10f;
    [SerializeField] float _countdownTime = 10f;
    [SerializeField] float _effectTime = 0.2f;
    //[SerializeField] Light _light = null;  

    public Vector3 _velocity = Vector3.zero;
    private Rigidbody _rb = null;

    Coroutine _destroyCountdown, _destroyEffects;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _destroyCountdown = StartCoroutine(DestroyIfNotCollided(_countdownTime));
    }

    private void FixedUpdate()
    {
        MoveForward();
        _velocity = _rb.velocity;
    }

    private void MoveForward()
    {
        _rb.AddRelativeForce(Vector3.forward * _bulletSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _destroyEffects = StartCoroutine(DestroySelf(_effectTime));
    }

    IEnumerator DestroyIfNotCollided(float secondsTillDestroy)
    {
        yield return new WaitForSeconds(secondsTillDestroy);
        Destroy(gameObject);
    }

    IEnumerator DestroySelf(float secondsToWait)
    {
        //_light.intensity *= 2;
        yield return new WaitForSeconds(secondsToWait);
        Destroy(gameObject);
    }
}
