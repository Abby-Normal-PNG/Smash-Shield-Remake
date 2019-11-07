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
    [SerializeField] float _turnSpeed = 6f;
    [SerializeField] float _jumpStrength = 10f;

    private float _currentSpeed = .1f;

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
        _input.QuitInput += OnQuit;
    }

    private void OnDisable()
    {
        _input.MoveInput -= OnMove;
        _input.JumpInput -= OnJump;
        _input.SheildInput -= OnShield;
        _input.SheildRelease -= OnShieldRelease;
        _input.QuitInput -= OnQuit;
    }

    void OnMove(Vector3 movement)
    {
        _motor.Move(movement * _currentSpeed);
    }

    void OnJump()
    {
        _motor.Jump(_jumpStrength);
    }

    void OnShield()
    {
        _shield._shieldActive = true;
    }

    void OnShieldRelease()
    {
        _shield._shieldActive = false;
    }

    void OnQuit()
    {
        Application.Quit();
    }
}
