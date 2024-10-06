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
    public bool IsLocalPlayer => isLocalPlayer;
    
    [SyncVar]
    public int userID;
    [SyncVar]
    public int MaxHp;
    [SyncVar]
    public int Hp;
    [SyncVar]
    public int Damage;    
    //protected CharacterController controller;
    //protected PlayerInput playerInput;
    protected PlayerMovement playerMovement;
    protected PlayerAnimation playerAnimation;
    protected PlayerUI playerUI;
    protected PlayerSkill playerSkill;
    protected PlayerStat playerStat;
    
    protected PlayerDamagable damagable;
    protected PlayerAttackable attackable;

    private void Awake()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        
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
    [Command]
    private void cmdOnJoin(int userId)
    {
        Debug.Log($"{userId} ����");
    }
    [Command]
    public void CmdRequestSendDamage(NetworkIdentity damagableIdentity, int damage)
    {
        Debug.Log("������ ó������ ");
        // NetworkIdentity�� ���� �ش� ������Ʈ�� ã��
        if (damagableIdentity.TryGetComponent<MonsterDamagable>(out var damagable))
        {
            Debug.Log("�������� ������ ó��.");
            damagable.GetDamage(damage);
        }
        else
        {
            Debug.LogError("��� ���͸� ã�� �� �����ϴ�.");
        }
    }

    /// <summary>
    /// �÷��̾��� ���� ���� ���� (UserStatManager), �÷��̾ �����ϰ� �ִ� ��� (PlayerEquipManager) �� �޾ƿ� ������ �ʱ�ȭ
    /// </summary>
    private void StatInit()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Debug.Log(damagable == null);
        damagable.OnTakeDamage += OnHpChange;
        damagable.OnDeath += OnDeath;
        // TODO : �÷��̾� ���ȿ� �ݿ� (�����)        
        UserStatClient userStatClient = UserStatManager.Instance.GetUserStatClient(this.userID);
        this.MaxHp = userStatClient.MaxHP;
        this.Hp = userStatClient.HP;
        this.Damage = userStatClient.STR;        
        damagable.Initialize(unitID: this.userID, maxHp: MaxHp, hp: Hp, isMonster: false);        
        attackable.Initialize(damage: Damage, range: 2);        

        playerUI?.Initialize(damagable);
        damagable.OnDeath += () => { Debug.Log("�÷��̾� �׾���."); };
    }
    
    private void Start()
    {        
        // ���� �÷��̾ �ʱ�ȭ�ϴ� ����
        if (!isLocalPlayer) return;
        Debug.Log("���� �� ��?����������������");
        //controller = GetComponent<CharacterController>();        
        //playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerStat = GetComponent<PlayerStat>();
        //playerUI = FindObjectOfType<PlayerUI>();

        damagable = gameObject.AddComponent<PlayerDamagable>();
        attackable = gameObject.AddComponent<PlayerAttackable>();
        PlayerInit();
        //EventHandler.monsterEvent.RegisterMonsterCreate(RequestInit);

        DatabaseManager.Instance.InitializePlayer(this.gameObject, DatabaseManager.Instance.userData.UID, DatabaseManager.Instance.userData.Name);
        this.userID = DatabaseManager.Instance.GetPlayerData(this.gameObject).UserId;

        GameManager.Instance.userManager_Dict[this.userID].transform.SetParent(this.transform);

        
        this.playerUI = Instantiate(playerUIPrefab, transform).GetComponentInChildren<PlayerUI>();

        StatInit();
        CmdSetUserID(userID);
        cmdOnJoin(userID);

        
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
        if (!isLocalPlayer)
        {
            return;
        }
        //if (!isServer)
        //{
        //    return;
        //}
        // TODO : ���ָ��� ���� ������ �ٸ��Բ� (��? ����)
        damagable.Hp -= damage;        
    }
    private void OnDeath()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        //if (!isServer)
        //{
        //    return;
        //}
        playerAnimation.DeathAnimation();

        Destroy(gameObject, 3f);
    }


    private void OnApplicationQuit()
    {
        GameManager.Instance.SaveData(this.gameObject);
    }
}