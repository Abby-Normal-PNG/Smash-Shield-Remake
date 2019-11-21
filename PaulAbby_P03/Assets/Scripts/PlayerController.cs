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

    [SerializeField] AudioSource _audioSource = null;
    [SerializeField] AudioClip _jumpingClip, _dizzyClip = null;
    [SerializeField] Animator _animator = null;
    [SerializeField] string _parameterName = "PlayerState";

    private float _currentSpeed = .1f;
    public enum PlayerState {Idle = 0, Walking = 1, Jumping = 2, Shielding = 3, Stunned = 4};
    private PlayerState _currentState = PlayerState.Idle;
    

    public PlayerState CurrentState { get => _currentState; }

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
        _input.JumpInput += OnJumpInput;
        _input.SheildInput += OnShield;
        _input.SheildRelease += OnShieldRelease;
        _shield.ShieldBreak += OnShieldBreak;
        _shield.ShieldFix += OnShieldFix;
        _motor.Land += OnLand;
        _motor.Jumped += OnJumped;
        _input.QuitInput += OnQuit;
    }

    private void OnDisable()
    {
        _input.MoveInput -= OnMove;
        _input.JumpInput -= OnJumpInput;
        _input.SheildInput -= OnShield;
        _input.SheildRelease -= OnShieldRelease;
        _shield.ShieldBreak -= OnShieldBreak;
        _shield.ShieldFix -= OnShieldFix;
        _motor.Land -= OnLand;
        _motor.Jumped -= OnJumped;
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
            UpdateAnimState();
            return;
        }
        _currentState = PlayerState.Idle;
        UpdateAnimState();
        _audioSource.Stop();
        _audioSource.loop = false;
    }

    private void UpdateAnimState()
    {
        int stateInt = (int)_currentState;
        _animator.SetInteger(_parameterName, stateInt);
    }

    void OnMove(Vector3 movement)
    {
        if (_currentState != PlayerState.Stunned)
        {
            _motor.Move(movement * _currentSpeed);
            if(movement == Vector3.zero)
            {
                _currentState = PlayerState.Idle;
                //UpdateAnimState();
            }
            else if(_currentState != PlayerState.Jumping)
            {
                _currentState = PlayerState.Walking;
                UpdateAnimState();
            }
        }
    }

    void OnJumpInput()
    {
        if (_currentState != PlayerState.Stunned)
        {
            if(_currentState == PlayerState.Shielding)
            {
                OnShieldRelease();
            }
            _motor.Jump(_jumpVelocity);
        }
    }

    void OnShield()
    {
        if (_currentState != PlayerState.Stunned &&
            _motor.IsGrounded == true)
        {
            _shield._shieldActive = true;
            _currentState = PlayerState.Shielding;
            UpdateAnimState();
            _shield.PlaySound(_shield._shieldOnClip);
        }
    }

    void OnShieldRelease()
    {
        _shield._shieldActive = false;
        if (_currentState != PlayerState.Stunned)
        {
            _currentState = PlayerState.Idle;
            UpdateAnimState();
            _shield.PlaySound(_shield._shieldOffClip);
        }
    }

    void OnShieldBreak()
    {
        _currentState = PlayerState.Stunned;
        UpdateAnimState();
        _motor.Jump(_shield._breakLaunchSpeed);
        _shield.PlaySound(_shield._shieldBreakClip);
        _audioSource.loop = true;
        PlaySound(_dizzyClip);
    }

    void OnShieldFix()
    {
        _currentState = PlayerState.Idle;
        UpdateAnimState();
    }

    void OnLand()
    {
        if(_currentState != PlayerState.Stunned)
        {
            _currentState = PlayerState.Idle;
            UpdateAnimState();
        }
    }

    void OnJumped()
    {
        PlaySound(_jumpingClip);
    }

    public void PlaySound(AudioClip audioClip)
    {
        _audioSource.Stop();
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    void OnQuit()
    {
        Application.Quit();
    }
}
