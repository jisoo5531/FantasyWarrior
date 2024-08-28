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
    private PlayerUI playerUI;

    private Damagable damagable;
    private Attackable attackable;    

    private void Awake()
    {
        controller = GetComponent<CharacterController>();        
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerUI = GetComponentInChildren<PlayerUI>();

        damagable = gameObject.AddComponent<Damagable>();
        attackable = gameObject.AddComponent<Attackable>();               
    }
    private void Start()
    {        
        damagable.Initialize(maxHp: 100, hp: 100);
        attackable.Initialize(damage: 10, range: 2);

        playerUI?.Initialize(damagable);

        damagable.OnDeath += () => { Debug.Log("플레이어 죽었다."); };        
    }        

    private void OnEnable()
    {
        playerInput.OnAttack += playerAnimation.AttackAnimation;

        damagable.OnHpChange += OnHpChange;
        damagable.OnDeath += OnDeath;        
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

        damagable.OnHpChange -= OnHpChange;
        damagable.OnDeath -= OnDeath;        
    }
    //private void OnDestroy()
    //{
    //    playerInput.OnAttack -= playerAnimation.AttackAnimation;
    //    EventHandler.actionEvent.UnRegisterHpChange(OnHpChange);
    //    EventHandler.actionEvent.UnRegisterDeath(OnDeath);
    //}
    private void OnHpChange(int damage)
    {
        // TODO : 유닛마다 들어가는 데미지 다르게끔 (방어도? 따라)
        damagable.Hp -= damage;
        Debug.Log($"데미지 받음 {damage} 만큼");
    }
    private void OnDeath()
    {
        playerAnimation.DeathAnimation();

        Destroy(gameObject, 3f);
    }
}