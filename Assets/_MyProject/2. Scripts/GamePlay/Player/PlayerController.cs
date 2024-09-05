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
        damagable.OnDeath += () => { Debug.Log("�÷��̾� �׾���."); };        
    }        

    private void OnEnable()
    {        
        damagable.OnHpChange += OnHpChange;
        damagable.OnDeath += OnDeath;        
    }
    
    private void FixedUpdate()
    {
        playerMovement?.Move(controller);
    }    

    private void OnDisable()
    {        
        damagable.OnHpChange -= OnHpChange;
        damagable.OnDeath -= OnDeath;        
    }
    //private void OnDestroy()
    //{
    //    playerInput.OnAttack -= playerAnimation.AttackAnimation;
    //    EventHandler.actionEvent.UnRegisterHpChange(OnHpChange);
    //    EventHandler.actionEvent.UnRegisterDeath(OnDeath);
    //}

    public static void OnLevelUp()
    {
        DatabaseManager.Instance.LevelUP(LevelUpSuccess);
    }
    private static void LevelUpSuccess()
    {
        EventHandler.playerEvent.TriggerPlayerLevelUp();
    }
    private void OnHpChange(int damage)
    {
        // TODO : ���ָ��� ���� ������ �ٸ��Բ� (��? ����)
        damagable.Hp -= damage;
        Debug.Log($"������ ���� {damage} ��ŭ");
    }
    private void OnDeath()
    {
        playerAnimation.DeathAnimation();

        Destroy(gameObject, 3f);
    }
}