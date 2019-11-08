using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SmashInput))]
[RequireComponent(typeof(SmashMotor))]
[RequireComponent(typeof(SheildShrink))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    SmashInput _input = null;
    SmashMotor _motor = null;
    SheildShrink _shield = null;
    Health _hp = null;

    [SerializeField] float _walkSpeed = .1f;
    [SerializeField] float _runSpeed = .2f;
    [SerializeField] float _jumpVelocity = 10f;

    private float _currentSpeed = .1f;
    private bool _stunned = false;

    private void Awake()
    {
        _input = GetComponent<SmashInput>();
        _motor = GetComponent<SmashMotor>();
        _shield = GetComponent<SheildShrink>();
        _hp = GetComponent<Health>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        _input.MoveInput += OnMove;
        _input.JumpInput += OnJump;
        _input.SheildInput += OnShield;
        _input.SheildRelease += OnShieldRelease;
        _shield.ShieldBreak += OnShieldBreak;
        _shield.ShieldFix += OnShieldFix;
        _input.QuitInput += OnQuit;
    }

    private void OnDisable()
    {
        _input.MoveInput -= OnMove;
        _input.JumpInput -= OnJump;
        _input.SheildInput -= OnShield;
        _input.SheildRelease -= OnShieldRelease;
        _shield.ShieldBreak -= OnShieldBreak;
        _shield.ShieldFix -= OnShieldFix;
        _input.QuitInput -= OnQuit;
    }

    void OnMove(Vector3 movement)
    {
        if (!_stunned)
        {
            _motor.Move(movement * _currentSpeed);
        }
    }

    void OnJump()
    {
        if (!_stunned)
        {
            _motor.Jump(_jumpVelocity);
        }
    }

    void OnShield()
    {
        if (!_stunned)
        {
            _shield._shieldActive = true;
        }
    }

    void OnShieldRelease()
    {
        _shield._shieldActive = false;
    }

    void OnShieldBreak()
    {
        _stunned = true;
        _motor.Jump(_shield._breakLaunchSpeed);
    }

    void OnShieldFix()
    {
        _stunned = false;
    }

    void OnQuit()
    {
        Application.Quit();
    }
}
