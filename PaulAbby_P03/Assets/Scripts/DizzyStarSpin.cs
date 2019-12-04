using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DizzyStarSpin : MonoBehaviour
{
    [SerializeField] float spinSpeed = 1;

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up * spinSpeed);
    }
}
