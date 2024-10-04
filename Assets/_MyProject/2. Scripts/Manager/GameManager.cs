using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mirror;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static PlayerInputAction inputActions;

    /// <summary>
    /// 현재 플레이어가 있는 장소의 ID
    /// </summary>
    public int currentLocationID;

    public GameObject goblin;
    public GameObject mummy;
    public GameObject Item;
    public GameObject UI;

    public Transform teleportTarget;

    public Transform player;
    private void Awake()
    {
        Instance = this;
        inputActions = new();
        
    }
    private void OnEnable()
    {
        inputActions.PlayerActions.Enable();
    }
    private void OnDisable()
    {

    }
    private void Start()
    {
        EventHandler.gameStartEvent.TriggerGameStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Instantiate(goblin, player.position + new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10)), goblin.transform.rotation);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Instantiate(mummy, new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10)), mummy.transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            QuestManager.Instance.AcceptQuest(1);
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            UI.SetActive(!UI.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            player.transform.position = teleportTarget.position;
        }
    }
    public void OnUserLoginManagerInit(int userID)
    {        

        ItemManager.Instance.Initialize();
        InventoryManager.Instance.Initialize(userID);
        UserStatManager.Instance.Initialize(userID);
        PlayerEquipManager.Instance.Initialize(userID);
        SkillManager.Instance.Initialize(userID);
        QuestManager.Instance.Initialize(userID);
        PlayerUIManager.Instance.Initialize();
        NPCManager.Instance.Initialize(userID);
        MonsterManager.Instance.Initialize();
        LocationManger.Instance.Initialize();
        ShopManager.Instance.Initialize();
        CraftManager.Instance.Initialize();
        CraftRecipeManager.Instance.Initialize();
        BlacksmithManager.Instance.Initialize();

        Debug.Log($"id : {userID}");        
        EventHandler.playerEvent.TriggerPlayerEnter(userID);
    }
    
    public void SaveData(GameObject player)
    {
        int userId = DatabaseManager.Instance.GetPlayerData(player).UserId;
        InventoryManager.Instance.Save(userId);
        UserStatManager.Instance.Save(userId);
        PlayerEquipManager.Instance.Save(userId);
        SkillManager.Instance.Save(userId);
        QuestManager.Instance.Save(userId);
        NPCManager.Instance.Save(userId);
    }
}

