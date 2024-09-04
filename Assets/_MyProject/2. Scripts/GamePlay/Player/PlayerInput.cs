using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{    
    public static PlayerInput Instance { get; private set; }
    public PlayerInputAction inputActions;

    private void Awake()
    {
        Instance = this;
        inputActions = new PlayerInputAction();

    }
    private void OnEnable()
    {
        inputActions.PlayerActions.Enable();
    }
    private void OnDisable()
    {
        
    }
}