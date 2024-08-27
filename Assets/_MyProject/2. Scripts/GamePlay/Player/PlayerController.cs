using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    RequireComponent(typeof(CharacterController)), 
    RequireComponent(typeof(PlayerInput)), 
    RequireComponent(typeof(PlayerMovement)), 
    RequireComponent(typeof(PlayerAnimation))
]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;

    private Damagable damagable;
    private Attackable attackable;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();        
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();

        damagable = gameObject.AddComponent<Damagable>();
        attackable = new Attackable(damage: 10, range: 2);

        GetComponentInChildren<PlayerWeapon>().damage = attackable.Damage;
    }
    private void Start()
    {
        damagable.Initialize(maxHp: 100, hp: 100);
    }

    private void OnEnable()
    {
        playerInput.OnAttack += playerAnimation.AttackAnimation;
    }

    private void Update()
    {
        playerInput?.Keyinput();
        
        float moveValue = Mathf.Max(Mathf.Abs(playerInput.Vertical), Mathf.Abs(playerInput.Horizontal));
        playerAnimation?.MoveAnimation(moveValue, playerInput.IsRun);
    }
    
    private void FixedUpdate()
    {
        playerMovement?.Move(transform, controller, playerInput.Horizontal, playerInput.Vertical, playerInput.IsRun);
    }

    private void OnDisable()
    {        
        playerInput.OnAttack -= playerAnimation.AttackAnimation;
    }
    private void OnDestroy()
    {
        playerInput.OnAttack -= playerAnimation.AttackAnimation;
    }
}