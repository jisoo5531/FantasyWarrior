using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Cinemachine;

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
    public CinemachineClearShot clearShot;

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
        ManagerInit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            player = FindObjectOfType<PlayerController>().gameObject.transform;
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
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            clearShot.Follow = GameObject.Find("Ancient Golem").transform;
            clearShot.LookAt = GameObject.Find("Ancient Golem").transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            clearShot.Follow = player.transform;
            clearShot.LookAt = player.transform;
        }
    }

    
    private void ManagerInit()
    {
        Debug.Log(ItemManager.Instance == null);
        ItemManager.Instance.Initialize();
        InventoryManager.Instance.Initialize();
        UserStatManager.Instance.Initialize();
        PlayerEquipManager.Instance.Initialize();
        SkillManager.Instance.Initialize();
        QuestManager.Instance.Initialize();
        PlayerUIManager.Instance.Initialize();
        NPCManager.Instance.Initialize();
        MonsterManager.Instance.Initialize();
        LocationManger.Instance.Initialize();
        ShopManager.Instance.Initialize();
        CraftManager.Instance.Initialize();
        CraftRecipeManager.Instance.Initialize();
        BlacksmithManager.Instance.Initialize();
    }
    public void Save()
    {
        InventoryManager.Instance.Save();
        UserStatManager.Instance.SaveStat();
        PlayerEquipManager.Instance.SaveEquipments();
        SkillManager.Instance.SaveSkill();
        SkillManager.Instance.SaveSkillKeyBind();
        QuestManager.Instance.SaveQuestProgress();        
    }
}
