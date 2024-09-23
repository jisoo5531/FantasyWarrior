using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

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
    }

    
    private void ManagerInit()
    {
        ItemManager.Instance.Initialize();
        InventoryManager.Instance.Initialize();
        UserStatManager.Instance.Initialize();
        PlayerEquipManager.Instance.Initialize();
        SkillManager.Instance.Initialize();
        QuestManager.Instance.Initialize();
        PlayerUIManager.Instance.Initialize();
        NPCManager.Instance.Initialize();
        MonsterManager.Instance.Initialize();
    }
}
