using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SheildShrink : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] GameObject _shieldObject = null;
    [SerializeField] float _maxScale = 1f;
    [SerializeField] float _minScale = 0.05f;
    [SerializeField] float _shieldShrinkSpeed = 0.01f;
    [SerializeField] float _shieldRestoreSpeed = 0.01f;
    public bool _shieldActive = false;
    public float _breakLaunchSpeed = 10f;
    [Header("Feedback")]
    [SerializeField] AudioSource _audioSource = null;
    public AudioClip _shieldOnClip = null;
    public AudioClip _shieldOffClip = null;
    public AudioClip _shieldBreakClip = null;
    [SerializeField] ParticleSystem _shieldBreakParticle = null;

    public event Action ShieldBreak = delegate { };
    public event Action ShieldFix = delegate { };

    private float _currentScale;
    private bool _sheildBroken = false;

    public bool ShieldIsFull { get; private set; }

    private void Start()
    {
        _currentScale = _maxScale;
        _shieldActive = false;
        SetShieldSize(_currentScale);
    }

    private void Update()
    {
        if (!_sheildBroken)
        {
            if (_shieldActive)
            {
                ShieldOn();
                ShrinkShield(_shieldShrinkSpeed);
                CheckShieldBreak();
                Debug.Log("Shield Scale: " + _currentScale);
            }
            else
            {
                ShieldOff();
                RestoreShield(_shieldRestoreSpeed);
                Debug.Log("Shield Scale: " + _currentScale);
            }
        }
        else
        {
            RestoreShield(_shieldRestoreSpeed);
            CheckShieldFix();
            Debug.Log("Shield Scale: " + _currentScale);
        }
    }

    private void CheckShieldBreak()
    {
        if (_currentScale <= _minScale)
        {
            ShieldBreak?.Invoke();
            BreakShield();
        }
    }

    private void BreakShield()
    {
        _sheildBroken = true;
        _shieldObject.SetActive(false);
        PlaySound(_shieldBreakClip);
        _shieldBreakParticle.Play();
    }

    private void CheckShieldFix()
    {
        if (_currentScale >= _maxScale)
        {
            ShieldFix?.Invoke();
            _sheildBroken = false;
        }
    }

    private void ShrinkShield(float shrinkAmount)
    {
        _currentScale -= shrinkAmount;
        SetShieldSize(_currentScale);
    }

    private void RestoreShield(float restoreAmount)
    {
        _currentScale += restoreAmount;
        if(_currentScale > _maxScale)
        {
            _currentScale = _maxScale;
        }
        SetShieldSize(_currentScale);
        Debug.Log("Shield Scale: " + _currentScale);
    }

    private void SetShieldSize(float size)
    {
        _shieldObject.transform.localScale = Vector3.one * size;
    }

    private void ShieldOn()
    {
        _shieldObject.SetActive(true);
    }

    private void ShieldOff()
    {
        _shieldObject.SetActive(false);
    }

    public void PlaySound(AudioClip audioClip)
    {
        _audioSource.Stop();
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
