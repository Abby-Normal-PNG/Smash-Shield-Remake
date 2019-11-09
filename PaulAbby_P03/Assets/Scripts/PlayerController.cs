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

    enum PlayerState {Idle, Walking, Jumping, Shielding, Stunned};
    [SerializeField] PlayerState _currentState = PlayerState.Idle;

    private void Awake()
    {
        _input = GetComponent<SmashInput>();
        _motor = GetComponent<SmashMotor>();
        _shield = GetComponent<SheildShrink>();
        _hp = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _input.MoveInput += OnMove;
        _input.JumpInput += OnJump;
        _input.SheildInput += OnShield;
        _input.SheildRelease += OnShieldRelease;
        _shield.ShieldBreak += OnShieldBreak;
        _shield.ShieldFix += OnShieldFix;
        _motor.Land += OnLand;
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
        _motor.Land += OnLand;
        _input.QuitInput -= OnQuit;
    }

    private void Update()
    {
        CheckState();
    }

    private void CheckState()
    {
        if (_currentState == PlayerState.Stunned ||
            _currentState == PlayerState.Shielding ||
            _currentState == PlayerState.Jumping)
        {
            return;
        }
        if (_motor.IsGrounded == false)
        {
            _currentState = PlayerState.Jumping;
            return;
        }
        _currentState = PlayerState.Idle;
    }

    void OnMove(Vector3 movement)
    {
        if (_currentState != PlayerState.Stunned)
        {
            _motor.Move(movement * _currentSpeed);
            if(movement == Vector3.zero)
            {
                _currentState = PlayerState.Idle;
            }else if(_currentState != PlayerState.Jumping)
            {
                _currentState = PlayerState.Walking;
            }
        }
    }

    void OnJump()
    {
        if (_currentState != PlayerState.Stunned)
        {
            _motor.Jump(_jumpVelocity);
            OnShieldRelease();
        }
    }

    void OnShield()
    {
        if (_currentState != PlayerState.Stunned &&
            _motor.IsGrounded == true)
        {
            _shield._shieldActive = true;
            _currentState = PlayerState.Shielding;
        }
    }

    void OnShieldRelease()
    {
        _shield._shieldActive = false;
        if (_currentState != PlayerState.Stunned)
        {
            _currentState = PlayerState.Idle;
        }
    }

    void OnShieldBreak()
    {
        _currentState = PlayerState.Stunned;
        _motor.Jump(_shield._breakLaunchSpeed);
    }

    void OnShieldFix()
    {
        _currentState = PlayerState.Idle;
    }

    void OnLand()
    {
        if(_currentState != PlayerState.Stunned)
        {
            _currentState = PlayerState.Idle;
        }
    }

    void OnQuit()
    {
        Application.Quit();
    }
}
