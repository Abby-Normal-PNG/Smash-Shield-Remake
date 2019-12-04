using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class DamageVolume : MonoBehaviour
{
    [SerializeField] int _damageValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        Health targetHealth = other.gameObject.GetComponent<Health>();
        targetHealth?.TakeDamage(_damageValue);
        Destroy(gameObject);
    }
}
