using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Mirror;

[
    RequireComponent(typeof(PlayerInput)), 
    RequireComponent(typeof(PlayerMovement)), 
    RequireComponent(typeof(PlayerAnimation)),
    RequireComponent(typeof(PlayerStat))
]
public class PlayerController : NetworkBehaviour
{
    private HumanScene scene;

    //protected CharacterController controller;
    //protected PlayerInput playerInput;
    protected PlayerMovement playerMovement;
    protected PlayerAnimation playerAnimation;
    protected UIComponent playerUI;
    protected PlayerSkill playerSkill;
    protected PlayerStat playerStat;
    
    protected Damagable damagable;
    protected Attackable attackable;

    private void Awake()
    {        
        //controller = GetComponent<CharacterController>();        
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
        damagable.OnTakeDamage += OnHpChange;
        damagable.OnDeath += OnDeath;
    }
    private void Start()
    {
        StatInit();
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
        damagable.Initialize(unitID: user_ID, maxHp: MaxHp, hp: Hp);
        attackable.Initialize(damage: damage, range: 2);


        playerUI?.Initialize(damagable);
        damagable.OnDeath += () => { Debug.Log("�÷��̾� �׾���."); };
    }

    public override void OnStartLocalPlayer()
    {
        GameObject.Find("_Scene").GetComponent<HumanScene>().player = this;
        GameObject.Find("ClearShot Camera").GetComponent<CinemachineClearShot>().Follow = this.transform;
        GameObject.Find("ClearShot Camera").GetComponent<CinemachineClearShot>().LookAt = this.transform;
        FindObjectOfType<Craft>().playerAnim = this.GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //playerMovement?.Move(controller);
    }    

    private void OnDisable()
    {        
        damagable.OnTakeDamage -= OnHpChange;
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
        // TODO : ���ָ��� ���� ������ �ٸ��Բ� (��? ����)
        damagable.Hp -= damage;        
    }
    private void OnDeath()
    {
        playerAnimation.DeathAnimation();

        Destroy(gameObject, 3f);
    }
}