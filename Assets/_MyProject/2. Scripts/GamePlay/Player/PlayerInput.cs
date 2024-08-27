using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool IsRun { get; private set; }
    public bool IsAttack { get; private set; }

    public event Action OnAttack;

    public void Keyinput()
    {
        // 이동 입력
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        IsRun = Input.GetButton("Run");
        IsAttack = Input.GetButton("Fire1");

        InputAction();
    }

    private void InputAction()
    {
        if (IsAttack)
        {
            OnAttack?.Invoke();
        }
    }
}