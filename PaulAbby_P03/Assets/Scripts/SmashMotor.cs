﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class SmashMotor : MonoBehaviour
{
    public event Action Land = delegate { };
    public event Action Jumped = delegate { };
    
    [SerializeField] GroundDetector _groundDetector = null;
    [SerializeField] int _jumpMax = 2;

    Rigidbody _rigidbody = null;
    Vector3 _movementThisFrame = Vector3.zero;

    bool _isGrounded = false;
    int _jumpIndex = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
            _rigidbody.AddForce(Vector3.up * jumpForce);
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
        _rigidbody.MovePosition(_rigidbody.position + _movementThisFrame);
        _movementThisFrame = Vector3.zero;
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
}