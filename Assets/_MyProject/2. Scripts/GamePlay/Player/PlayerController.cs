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
    private PlayerWeapon playerWeapon;

    private Damagable damagable;
    private Attackable attackable;

    private BoxCollider weaponCollider;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();        
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();

        damagable = gameObject.AddComponent<Damagable>();
        attackable = gameObject.AddComponent<Attackable>();

        playerWeapon = GetComponentInChildren<PlayerWeapon>();
        weaponCollider = playerWeapon.GetComponent<BoxCollider>();        
    }
    private void Start()
    {
        damagable.Initialize(maxHp: 100, hp: 100);
        attackable.Initialize(damage: 10, range: 2);

        playerWeapon.damage = attackable.Damage;
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

    public void OnWeaponCollider()
    {
        weaponCollider.enabled = true;
    }
    public void OffWeaponCollider()
    {
        weaponCollider.enabled = false;
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