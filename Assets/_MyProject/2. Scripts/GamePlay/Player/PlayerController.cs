using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[
    RequireComponent(typeof(CharacterController)), 
    RequireComponent(typeof(PlayerInput)), 
    RequireComponent(typeof(PlayerMovement)), 
    RequireComponent(typeof(PlayerAnimation)),
    RequireComponent(typeof(PlayerStat))
]
public class PlayerController : MonoBehaviour
{    
    protected CharacterController controller;
    //protected PlayerInput playerInput;
    protected PlayerMovement playerMovement;
    protected PlayerAnimation playerAnimation;
    protected UIComponent playerUI;
    protected PlayerSkill playerSkill;
    protected PlayerStat playerStat;
    
    protected Damagable damagable;
    protected Attackable attackable;

    public Button LevelUpButton;

    private void Awake()
    {        
        controller = GetComponent<CharacterController>();        
        //playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerStat = GetComponent<PlayerStat>();
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
        int MaxHp = UserStatManager.Instance.userStatData.MaxHp;
        int Hp = UserStatManager.Instance.userStatData.Hp;
        int damage = UserStatManager.Instance.userStatData.STR;
        Debug.Log($"{MaxHp}, {Hp}, {damage}");
        damagable.Initialize(maxHp: MaxHp, hp: Hp);
        attackable.Initialize(damage: damage, range: 2);


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

    // TODO : �ӽ� ��ư Ŭ�� �÷��̾� ���� ��    
    public void OnLevelUp()
    {
        //DatabaseManager.Instance.LevelUP(LevelUpSuccess);        
    }
    private void LevelUpSuccess()
    {
        playerStat.OnLevelUpStatChange();
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