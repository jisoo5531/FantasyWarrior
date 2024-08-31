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
    public static PlayerInputAction inputActions;

    protected CharacterController controller;
    //protected PlayerInput playerInput;
    protected PlayerMovement playerMovement;
    protected PlayerAnimation playerAnimation;
    protected UIComponent playerUI;
    protected PlayerSkill playerSkill;
    
    protected Damagable damagable;
    protected Attackable attackable;    

    private void Awake()
    {
        inputActions = new();

        controller = GetComponent<CharacterController>();        
        //playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();        
        playerUI = FindObjectOfType<PlayerUI>();

        damagable = gameObject.AddComponent<Damagable>();
        attackable = gameObject.AddComponent<Attackable>();                
        
        PlayerInit();
    }
    protected virtual void PlayerInit()
    {

    }
    private void Start()
    {                
        damagable.Initialize(maxHp: 1000, hp: 1000);
        attackable.Initialize(damage: 10, range: 2);

        playerUI?.Initialize(damagable);        
        damagable.OnDeath += () => { Debug.Log("플레이어 죽었다."); };        
    }        

    private void OnEnable()
    {       
        inputActions.PlayerActions.Enable();        

        damagable.OnHpChange += OnHpChange;
        damagable.OnDeath += OnDeath;        
    }
    
    private void FixedUpdate()
    {
        playerMovement?.Move(controller);
    }    

    private void OnDisable()
    {        

        inputActions.PlayerActions.Move.performed -= playerMovement.OnMovePerformed;
        inputActions.PlayerActions.Run.performed -= playerMovement.OnRunPerformed;

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