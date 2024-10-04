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
    public GameObject playerUIPrefab;
    private HumanScene scene;
    
    [SyncVar]
    public int userID;
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
        // �������� ������� �ʵ���
        if (NetworkServer.active)
        {
            return; // ���������� UI�� �������� ����
        }        

        
        //controller = GetComponent<CharacterController>();        
        //playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerStat = GetComponent<PlayerStat>();
        //playerUI = FindObjectOfType<PlayerUI>();

        damagable = gameObject.AddComponent<Damagable>();
        attackable = gameObject.AddComponent<Attackable>();        
        PlayerInit();        
    }
    protected virtual void PlayerInit()
    {
        // override
    }

    [Command]
    private void CmdSetUserID(int id)
    {
        userID = id; // �������� userID ����
        Debug.Log($"User ID set to: {userID}");
    }


    /// <summary>
    /// �÷��̾��� ���� ���� ���� (UserStatManager), �÷��̾ �����ϰ� �ִ� ��� (PlayerEquipManager) �� �޾ƿ� ������ �ʱ�ȭ
    /// </summary>
    private void StatInit()
    {        
        damagable.OnTakeDamage += OnHpChange;
        damagable.OnDeath += OnDeath;
        // TODO : �÷��̾� ���ȿ� �ݿ� (�����)        
        UserStatClient userStatClient = UserStatManager.Instance.GetUserStatClient(this.userID);
        int MaxHp = userStatClient.MaxHP;
        int Hp = userStatClient.HP;
        int damage = userStatClient.STR;        
        damagable.Initialize(unitID: this.userID, maxHp: MaxHp, hp: Hp);
        attackable.Initialize(damage: damage, range: 2);        

        playerUI?.Initialize(damagable);
        damagable.OnDeath += () => { Debug.Log("�÷��̾� �׾���."); };
    }
    [Command]
    private void cmdOnJoin(int userId)
    {
        Debug.Log($"{userId} ����");
    }
    private void Start()
    {
        // ���� �÷��̾ �ʱ�ȭ�ϴ� ����
        if (!isLocalPlayer) return;
        Debug.LogError("����;!!!");

        GameObject playerUI = Instantiate(playerUIPrefab, transform);
        this.playerUI = playerUI.GetComponentInChildren<UIComponent>();

        DatabaseManager.Instance.InitializePlayer(this.gameObject, DatabaseManager.Instance.userData.UID, DatabaseManager.Instance.userData.Name);
        this.userID = DatabaseManager.Instance.GetPlayerData(this.gameObject).UserId;
        StatInit();
        CmdSetUserID(userID);
        cmdOnJoin(userID);

        Debug.LogError($"�� ���̵� : {this.userID}");
        GameObject.Find("_Scene").GetComponent<HumanScene>().player = this;
        GameObject.Find("ClearShot Camera").GetComponent<CinemachineClearShot>().Follow = this.transform;
        GameObject.Find("ClearShot Camera").GetComponent<CinemachineClearShot>().LookAt = this.transform;
        foreach (var dialogue in FindObjectsOfType<UI_NPCDialogue>(true))
        {
            dialogue.playerUI = playerUI.GetComponentInChildren<PlayerUI>();
        }
        foreach (var craft in FindObjectsOfType<Craft>(true))
        {
            craft.playerAnim = GetComponent<Animator>();
        }
        GetComponentInChildren<UI_EquipPanel>(true).Initialize();
        GetComponentInChildren<UI_QuestPanel>(true).Initialize();
        GetComponentInChildren<UI_InventoryPanel>(true).Initialize();
        GetComponentInChildren<UI_StatPanel>(true).Initialize();
    }
    public override void OnStartLocalPlayer()
    {
          
    }
    private void FixedUpdate()
    {
        //playerMovement?.Move(controller);
    }    

    //private void OnDisable()
    //{        
    //    damagable.OnTakeDamage -= OnHpChange;
    //    damagable.OnDeath -= OnDeath;                
    //}
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

    private void OnApplicationQuit()
    {
        GameManager.Instance.SaveData(this.gameObject);
    }
}