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

    // TODO : �ӽ� ������ �׽�Ʈ ��ư
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
        // override
    }
    private void OnEnable()
    {
        UserStatManager.Instance.OnInitStatManager += StatInit;
        PlayerEquipManager.Instance.OnEquipManagerInit += StatInit;
        damagable.OnHpChange += OnHpChange;
        damagable.OnDeath += OnDeath;
    }
     
    /// <summary>
    /// �÷��̾��� ���� ���� ���� (UserStatManager), �÷��̾ �����ϰ� �ִ� ��� (PlayerEquipManager) �� �޾ƿ� ������ �ʱ�ȭ
    /// </summary>
    private void StatInit()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        // TODO : �÷��̾� ���ȿ� �ݿ� (�����)        
        UserStatClient userStatClient = UserStatManager.Instance.userStatClient;
        int MaxHp = userStatClient.MaxHP;
        int Hp = userStatClient.HP;
        int damage = userStatClient.STR;
        Debug.Log($"MaxHP : {MaxHp}, HP : {Hp}");
        damagable.Initialize(unitID: user_ID, maxHp: MaxHp, hp: Hp);
        attackable.Initialize(damage: damage, range: 2);


        playerUI?.Initialize(damagable);
        damagable.OnDeath += () => { Debug.Log("�÷��̾� �׾���."); };
    }
    
    
    private void FixedUpdate()
    {
        playerMovement?.Move(controller);
    }    

    private void OnDisable()
    {        
        damagable.OnHpChange -= OnHpChange;
        damagable.OnDeath -= OnDeath;
        UserStatManager.Instance.OnInitStatManager -= StatInit;
        PlayerEquipManager.Instance.OnEquipManagerInit -= StatInit;
    }
    //private void OnDestroy()
    //{
    //    playerInput.OnAttack -= playerAnimation.AttackAnimation;
    //    EventHandler.actionEvent.UnRegisterHpChange(OnHpChange);
    //    EventHandler.actionEvent.UnRegisterDeath(OnDeath);
    //}
    
    private void OnHpChange(int damage)
    {
        // TODO : ���ָ��� ���� ������ �ٸ��Բ� (��? ����)
        damagable.Hp -= damage;        
    }
    private void OnDeath()
    {
        playerAnimation.DeathAnimation();

        Destroy(gameObject, 3f);
    }
}