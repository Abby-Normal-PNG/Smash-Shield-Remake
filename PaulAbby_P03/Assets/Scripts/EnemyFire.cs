using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyFire : MonoBehaviour
{
    [Header("Shooting Variables")]
    [SerializeField] GameObject _projectile = null;
    [SerializeField] Transform _shotOrigin = null;
    [SerializeField] float _firingDelaySeconds = 3f;

    [Header("Feedback")]
    //[SerializeField] ParticleSystem _shotParticle = null;
    [SerializeField] AudioSource _shotAudio = null;

    Coroutine _firingCoroutine = null;

    private void Update()
    {
        if (_firingCoroutine == null)
        {
            _firingCoroutine = StartCoroutine(FiringDelay(_firingDelaySeconds));
        }
    }

    IEnumerator FiringDelay(float delayInSeconds)
    {
        FireShot();
        yield return new WaitForSeconds(delayInSeconds);
        _firingCoroutine = null;
    }

    private void FireShot()
    {
        Instantiate(_projectile, _shotOrigin.transform);
        PlayShotSound();
        PlayShotParticle();
    }

    public void PlayShotSound()
    {
        _shotAudio.Play();
    }

    public void PlayShotParticle()
    {
        //_shotParticle.Play();
    }
}
