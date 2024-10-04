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
        // 서버에서 실행되지 않도록
        if (NetworkServer.active)
        {
            return; // 서버에서는 UI를 실행하지 않음
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
        userID = id; // 서버에서 userID 설정
        Debug.Log($"User ID set to: {userID}");
    }


    /// <summary>
    /// 플레이어의 레벨 등의 스탯 (UserStatManager), 플레이어가 착용하고 있는 장비 (PlayerEquipManager) 를 받아온 다음에 초기화
    /// </summary>
    private void StatInit()
    {        
        damagable.OnTakeDamage += OnHpChange;
        damagable.OnDeath += OnDeath;
        // TODO : 플레이어 스탯에 반영 (제대로)        
        UserStatClient userStatClient = UserStatManager.Instance.GetUserStatClient(this.userID);
        int MaxHp = userStatClient.MaxHP;
        int Hp = userStatClient.HP;
        int damage = userStatClient.STR;        
        damagable.Initialize(unitID: this.userID, maxHp: MaxHp, hp: Hp);
        attackable.Initialize(damage: damage, range: 2);        

        playerUI?.Initialize(damagable);
        damagable.OnDeath += () => { Debug.Log("플레이어 죽었다."); };
    }
    [Command]
    private void cmdOnJoin(int userId)
    {
        Debug.Log($"{userId} 들어옴");
    }
    private void Start()
    {
        // 로컬 플레이어만 초기화하는 로직
        if (!isLocalPlayer) return;
        Debug.LogError("여기;!!!");

        GameObject playerUI = Instantiate(playerUIPrefab, transform);
        this.playerUI = playerUI.GetComponentInChildren<UIComponent>();

        DatabaseManager.Instance.InitializePlayer(this.gameObject, DatabaseManager.Instance.userData.UID, DatabaseManager.Instance.userData.Name);
        this.userID = DatabaseManager.Instance.GetPlayerData(this.gameObject).UserId;
        StatInit();
        CmdSetUserID(userID);
        cmdOnJoin(userID);

        Debug.LogError($"얘 아이디 : {this.userID}");
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
        // TODO : 유닛마다 들어가는 데미지 다르게끔 (방어도? 따라)
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