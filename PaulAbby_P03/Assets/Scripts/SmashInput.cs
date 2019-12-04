using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SmashInput : MonoBehaviour
{
    [SerializeField] bool _invertVertical = false;

    public event Action<Vector3> MoveInput = delegate { };
    public event Action MoveRelease = delegate { };
    public event Action JumpInput = delegate { };
    public event Action SheildInput = delegate { };
    public event Action SheildRelease = delegate { };
    public event Action QuitInput = delegate { };

    private void Start()
    {
        MoveInput?.Invoke(Vector3.zero);
        //SheildInput?.Invoke();
        //SheildRelease?.Invoke();
    }

    private void Update()
    {
        DetectMoveInput();
        DetectJumpInput();
        DetectSheildInput();
        DetectQuitInput();
    }

    private void DetectQuitInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitInput?.Invoke();
        }
    }

    private void DetectSheildInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SheildInput?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            SheildRelease?.Invoke();
        }
    }

    private void DetectJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpInput?.Invoke();
        }
    }

    private void DetectMoveInput()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (xInput != 0 || yInput != 0)
        {
            Vector3 _horizontalMovement = transform.right * xInput;
            Vector3 _forwardMovement = transform.up * yInput;

            Vector3 movement = (_horizontalMovement + _forwardMovement).normalized;
            MoveInput?.Invoke(movement);
        }
        else
        {
            MoveRelease?.Invoke();
        }
    }
}
