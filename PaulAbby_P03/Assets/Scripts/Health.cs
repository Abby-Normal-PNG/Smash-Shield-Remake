using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public event Action TookDamage = delegate { };
    public event Action Healed = delegate { };
    public event Action Died = delegate { };

    public int _healthMax = 10;
    public int _healthCurrent = 10;

    private void Start()
    {
        _healthCurrent = _healthMax;
    }

    public void TakeDamage(int damageAmount)
    {
        _healthCurrent -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage!");
        TookDamage?.Invoke();
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (_healthCurrent <= 0)
        {
            Died.Invoke();
        }
    }

    public void HealDamage(int healAmount)
    {
        _healthCurrent += healAmount;
        if(_healthCurrent > _healthMax)
        {
            _healthCurrent = _healthMax;
        }
        Healed.Invoke();
    }
}
