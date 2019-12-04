using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(SmashMotor))]

public class SmashUIManager : MonoBehaviour
{
    /*[Header("Damage")]
    [SerializeField] CanvasGroup _damagedEffectCG = null;
    [SerializeField] AudioClip _damagedClip = null;
    [SerializeField] float _damageEffectSeconds = 0.3f;

    [Header("Heal")]
    [SerializeField] CanvasGroup _healEffectCG = null;
    [SerializeField] AudioClip _healedClip = null;
    [SerializeField] float _healEffectSeconds = 0.3f;

    [Header("Dead")]
    [SerializeField] CanvasGroup _deadEffectCG = null;
    [SerializeField] AudioClip _deadClip = null;
    [SerializeField] float _deadEffectTime = 1.5f;*/

    [Header("UI")]
    [SerializeField] Text _hpText = null;

    /*[Header("Misc")]
    [SerializeField] AudioSource _playerAudio = null;
    [SerializeField] AudioClip _jumpClip = null;*/

    private Health _health;
    private SmashMotor _motor;
    private Scene _currentScene;

    Coroutine _damagedCoroutine, _healedCoroutine, _deathCoroutine = null;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _motor = GetComponent<SmashMotor>();
        _currentScene = SceneManager.GetActiveScene();
    }

    private void OnEnable()
    {
        _health.TookDamage += OnTookDamage;
        _health.Healed += OnHealed;
        _health.Died += OnDied;
    }

    private void OnDisable()
    {
        _health.TookDamage -= OnTookDamage;
        _health.Healed -= OnHealed;
        _health.Died -= OnDied;
    }

    private void Start()
    {
        //hide damaged/healed effect panel at start
        //_damagedEffectCG.alpha = 0;
        //_healEffectCG.alpha = 0;
        PrepHPSlider(_health._healthMax);
        UpdateHPVisual(_health._healthCurrent);
    }

    private void OnTookDamage()
    {
        //_damagedCoroutine = StartCoroutine(ScreenFlash(_damagedEffectCG, _damageEffectSeconds));
        //_playerAudio.clip = _damagedClip;
        //_playerAudio.Play();
        UpdateHPVisual(_health._healthCurrent);
    }

    private void OnHealed()
    {
        //_healedCoroutine = StartCoroutine(ScreenFlash(_healEffectCG, _healEffectSeconds));
        //_playerAudio.clip = _healedClip;
        //_playerAudio.Play();
        UpdateHPVisual(_health._healthCurrent);
    }

    IEnumerator ScreenFlash(CanvasGroup cg, float secondsToWait)
    {
        cg.alpha = 1;
        yield return new WaitForSeconds(secondsToWait);
        cg.alpha = 0;
    }

    void PrepHPSlider(int hpMax)
    {
        _hpText.text = hpMax + " HP";
    }

    private void UpdateHPVisual(int hpValue)
    {
        _hpText.text = hpValue + " HP";
    }

    void OnDied()
    {
        //_deathCoroutine = StartCoroutine(deathScreenDelay(_deadEffectTime));
    }

    IEnumerator deathScreenDelay(float secondsTillReset)
    {
        //_deadEffectCG.alpha = 1;
        //_playerAudio.clip = _deadClip;
        //_playerAudio.Play();
        yield return new WaitForSeconds(secondsTillReset);
        //_deadEffectCG.alpha = 0;
        SceneManager.LoadScene(_currentScene.buildIndex);
    }
}
