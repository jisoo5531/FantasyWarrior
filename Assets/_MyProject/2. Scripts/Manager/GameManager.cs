using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static PlayerInputAction inputActions;

    public Dictionary<int, GameObject> userManager_Dict { get; private set; }
    public Dictionary<int, InventoryManager> invenManager { get; private set; }
    public Dictionary<int, UserStatManager> statManager { get; private set; }
    public Dictionary<int, PlayerEquipManager> equipManager { get; private set; }
    public Dictionary<int, SkillManager> skillManager { get; private set; }
    public Dictionary<int, QuestManager> questManager { get; private set; }
    public Dictionary<int, NPCManager> npcManager { get; private set; }

    public int currentLocationID;

    public GameObject goblin;
    public GameObject mummy;
    public GameObject Item;
    public GameObject UI;

    public Transform teleportTarget;
    public Transform player;

    [Header("매니저")]
    public GameObject ManagerObj;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        inputActions = new PlayerInputAction();

        userManager_Dict = new Dictionary<int, GameObject>();
        invenManager = new Dictionary<int, InventoryManager>();
        statManager = new Dictionary<int, UserStatManager>();
        equipManager = new Dictionary<int, PlayerEquipManager>();
        skillManager = new Dictionary<int, SkillManager>();
        questManager = new Dictionary<int, QuestManager>();
        npcManager = new Dictionary<int, NPCManager>();
    }

    private void OnEnable()
    {
        inputActions.PlayerActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.PlayerActions.Disable();
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
        // ManagerObj 프리팹을 인스턴스화하여 각 플레이어에게 매니저 부착
        GameObject managerObjInstance = Instantiate(ManagerObj);
        userManager_Dict.Add(userID, managerObjInstance);
        userManager_Dict[userID].GetComponent<InventoryManager>();

        // 각 매니저 인스턴스 생성
        InventoryManager inventoryManager = userManager_Dict[userID].GetComponentInChildren<InventoryManager>();
        UserStatManager userStatManager = userManager_Dict[userID].GetComponentInChildren<UserStatManager>();
        PlayerEquipManager playerEquipManager = userManager_Dict[userID].GetComponentInChildren<PlayerEquipManager>();
        SkillManager skillManagerInstance = userManager_Dict[userID].GetComponentInChildren<SkillManager>();
        QuestManager questManagerInstance = userManager_Dict[userID].GetComponentInChildren<QuestManager>();
        NPCManager npcManagerInstance = userManager_Dict[userID].GetComponentInChildren<NPCManager>();

        // 딕셔너리에 추가
        invenManager.Add(userID, inventoryManager);
        statManager.Add(userID, userStatManager);
        equipManager.Add(userID, playerEquipManager);
        skillManager.Add(userID, skillManagerInstance);
        questManager.Add(userID, questManagerInstance);
        npcManager.Add(userID, npcManagerInstance);

        // 각 매니저 초기화
        inventoryManager.Initialize(userID);
        userStatManager.Initialize(userID);
        playerEquipManager.Initialize(userID);
        skillManagerInstance.Initialize(userID);
        questManagerInstance.Initialize(userID);
        npcManagerInstance.Initialize(userID);

        // 글로벌 매니저 초기화
        ItemManager.Instance.Initialize();
        PlayerUIManager.Instance.Initialize();
        //MonsterManager.Instance.Initialize();
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

        // 각 매니저 저장 메서드 호출
        if (invenManager.ContainsKey(userId)) invenManager[userId].Save(userId);
        if (statManager.ContainsKey(userId)) statManager[userId].Save(userId);
        if (equipManager.ContainsKey(userId)) equipManager[userId].Save(userId);
        if (skillManager.ContainsKey(userId)) skillManager[userId].Save(userId);
        if (questManager.ContainsKey(userId)) questManager[userId].Save(userId);
        if (npcManager.ContainsKey(userId)) npcManager[userId].Save(userId);
    }
}
