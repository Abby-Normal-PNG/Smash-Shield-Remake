﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ShieldShrink))]
public class SmashMotor : MonoBehaviour
{
    public event Action Land = delegate { };
    public event Action Jumped = delegate { };
    
    [SerializeField] GroundDetector _groundDetector = null;
    [SerializeField] int _jumpMax = 2;

    [SerializeField] GameObject _visuals = null;
    [SerializeField] float _rightYRot = 90, _leftYRot = -90;
    [SerializeField] GameObject _shieldVisuals = null;
    [SerializeField] float _shieldRadius = 10;
    [SerializeField] Transform _shieldOriginalPosition = null;

    Rigidbody _rigidbody = null;
    Vector3 _movementThisFrame = Vector3.zero;
    ShieldShrink _shield = null;

    bool _isGrounded = false;
    public bool IsGrounded { get => _isGrounded; private set { } }
    int _jumpIndex = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _shield = GetComponent<ShieldShrink>();
    }

    private void OnEnable()
    {
        _groundDetector.GroundDetected += OnGroundDetected;
        _groundDetector.GroundVanished += OnGroundVanished;
    }

    private void FixedUpdate()
    {
        ApplyMovement(_movementThisFrame);
    }

    private void OnDisable()
    {
        _groundDetector.GroundDetected -= OnGroundDetected;
        _groundDetector.GroundVanished -= OnGroundVanished;
    }

    public void Move(Vector3 requestedMovement)
    {
        _movementThisFrame = requestedMovement;
    }

    public void Jump(float jumpForce)
    {
        if (_isGrounded || _jumpIndex < _jumpMax)
        {
            _rigidbody.velocity = Vector3.up * jumpForce;
            _jumpIndex++;
            Jumped.Invoke();
        }
    }

    private void ApplyMovement(Vector3 moveVector)
    {
        if (moveVector == Vector3.zero)
        {
            return;
        }
        if (_shield._shieldActive)
        {
            _shieldVisuals.transform.position = _shieldOriginalPosition.position + moveVector * _shieldRadius;
        }
        else
        {
            //Limit y and z movement
            _movementThisFrame.z = 0;
            if(_movementThisFrame.y > 0)
            {
                _movementThisFrame.y = 0;
            }
            _rigidbody.MovePosition(_rigidbody.position + _movementThisFrame);
            FaceDirection();
            _movementThisFrame = Vector3.zero;
        }
    }

    private void FaceDirection()
    {
        if(_movementThisFrame.x >= 0)
        {
            _visuals.transform.rotation = Quaternion.Euler(0, _rightYRot, 0);
        }
        else
        {
            _visuals.transform.rotation = Quaternion.Euler(0, _leftYRot, 0);
        }
    }

    private void OnGroundDetected()
    {
        _isGrounded = true;
        _jumpIndex = 0;
        Land?.Invoke();
    }

    private void OnGroundVanished()
    {
        _isGrounded = false;
    }

    internal void ReturnShield()
    {
        _shieldVisuals.transform.position = _shieldOriginalPosition.position;
    }
}
