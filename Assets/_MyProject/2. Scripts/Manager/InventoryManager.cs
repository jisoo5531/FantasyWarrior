using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    /// <summary>
    /// 게임 중에 쓰일 인벤토리 리스트
    /// </summary>
    public List<InventoryData> inventoryDataList { get; private set; }
    /// <summary>
    /// 장착한 아이템을 해제하거나 등등의 인벤토리로 아이템이 추가되는 상황에서 쓰일 리스트
    /// </summary>
    public List<AddItemClassfiy> addWhichItemList = new List<AddItemClassfiy>();
    /// <summary>
    /// 저장할 때 비교를 위한 원본 인벤토리 리스트
    /// </summary>
    private List<InventoryData> originInventoryList;
    /// <summary>
    /// 현재 유저가 어떤 제작 도구 데이터들을 가지고 있는지에 대한 리스트
    /// </summary>
    private List<UserCraftToolData> userCraftToolList;
    /// <summary>
    /// 현재 유저가 어떤 제작 도구 데이터들을 가지고 있는지에 대한 리스트를 클라이언트에 임시로 저장
    /// <para>인게임에서 사용할</para>
    /// </summary>
    public List<UserCraftToolData> userCraftToolClient { get; private set; }

    private ItemData itemData;

    /// <summary>
    /// 아이템을 획득 시, 실행할 이벤트
    /// </summary>
    public event Action OnGetItem;
    /// <summary>
    /// 아이템 획득 시, 아이템 데이터와 같이 넘겨주기
    /// </summary>
    public event Action<ItemData> OnGetItemData;
    /// <summary>
    /// 제작도구를 획득 시, 실행할 이벤트
    /// </summary>
    public event Action OnGetCraftItem;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        GetDataFromDatabase();
        GetUserCraftItemFromDB();        
        //EventHandler.managerEvent.TriggerInventoryManagerInit();
    }
    
    /// <summary>
    /// 인벤토리로 어떤 아이템이 추가되었는지 확인할 메서드
    /// </summary>
    /// <param name="itemID"></param>
    public void AddWhichItem(AddItemClassfiy addClassfiy)
    {
        addWhichItemList.Add(addClassfiy);
    }
    public void ClearAddWhichItemList()
    {
        addWhichItemList = new();
    }
    
    /// <summary>
    /// 아이템을 획득 시, 실행할 메서드
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="amount"></param>
    public void GetItem(ItemData itemData, int amount)
    {
        this.itemData = itemData;
        int user_Id = DatabaseManager.Instance.userData.UID;

        int index = inventoryDataList.FindIndex(x => x.User_ID.Equals(user_Id) && x.Item_ID.Equals(itemData.Item_ID));
        if (index >= 0)
        {
            // 아이템 획득 시, 인벤토리에 있는 아이템이라면
            inventoryDataList[index].Quantity += amount;
            AddWhichItem(new AddItemClassfiy(itemData.Item_ID, amount, true));
        }
        else
        {
            // 아이템 획득 시, 인벤토리에 없는 아이템이라면
            inventoryDataList.Add(new InventoryData(user_Id, itemData.Item_ID, amount));
            AddWhichItem(new AddItemClassfiy(itemData.Item_ID, amount, false));
        }
        CheckGetCraftTool(itemData.Item_ID);
        OnGetItemData?.Invoke(itemData);
        OnGetItem?.Invoke();
    }
    /// <summary>
    /// 얻은 아이템이 제작도구인지 확인
    /// </summary>
    /// <param name="item_ID"></param>
    public void CheckGetCraftTool(int item_ID)
    {
        CraftToolData craftTool = ItemManager.Instance.GetCraftToolData(item_ID);
        if (craftTool == null) return;

        AddCraftItem(craftTool);
    }
    /// <summary>
    /// 제작도구를 얻었을 때 유저가 가지고 있는 제작도구 리스트(usercraftList)에 추가
    /// </summary>
    private void AddCraftItem(CraftToolData craftTool)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        int index = userCraftToolClient.FindIndex(x => x.CreftType.Equals(craftTool.CreftType));
        userCraftToolClient[index].Item_ID = craftTool.Item_ID;

        OnGetCraftItem?.Invoke();
    }
    /// <summary>
    /// 장비를 벗을 때, 인벤토리로 추가
    /// </summary>
    /// <param name="item_ID"></param>
    /// <param name="amount"></param>
    public void GetItemUnEquip(int item_ID, int amount)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        inventoryDataList.Add(new InventoryData(user_ID, item_ID, amount));
    }
    /// <summary>
    /// 장비를 장착 시, 인벤토리에서 해당 아이템 사라지게 할 메서드
    /// </summary>
    public void EquipItemUpdateInventory(int itemID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        int index = inventoryDataList.FindIndex(x => x.User_ID.Equals(user_ID) && x.Item_ID.Equals(itemID));
        if (index >= 0)
        {
            inventoryDataList.RemoveAt(index);
        }
    }
    public int? GetItemQuantity(int itemID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;

        int index = inventoryDataList.FindIndex(x => x.User_ID.Equals(user_ID) && x.Item_ID.Equals(itemID));
        if (index >= 0)
        {
            return inventoryDataList[index].Quantity;
        }
        return null;
    }

    #region DB
    /// <summary>
    /// <para>인벤토리 데이터를 가져오는 메서드</para>
    /// </summary>
    private List<InventoryData> GetDataFromDatabase()
    {
        originInventoryList = new List<InventoryData>();
        inventoryDataList = new List<InventoryData>();
        string query =
            $"SELECT *\n" +
            $"FROM inventory;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                originInventoryList.Add(new InventoryData(row));
                inventoryDataList.Add(new InventoryData(row));
            }
            return inventoryDataList;
        }
        else
        {
            return null;
        }
        //OnGetItem?.Invoke();
    }
    /// <summary>
    /// 유저가 보유한 제작도구들을 가져오기
    /// <para>동시에 클라이언트에 임시로 저장하는 것도 같이</para>
    /// </summary>
    private void GetUserCraftItemFromDB()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        userCraftToolList = new List<UserCraftToolData>();
        userCraftToolClient = new List<UserCraftToolData>();
        string query =
            $"SELECT *\n" +
            $"FROM usercrafttools\n" +
            $"WHERE usercrafttools.User_ID={user_ID}";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                userCraftToolList.Add(new UserCraftToolData(row));
                userCraftToolClient.Add(new UserCraftToolData(row));
            }
        }
        else
        {
            //  실패
        }
    }
    #endregion

    #region 인벤토리 저장
    /// <summary>
    /// DB에 아직 넣지 않고 클라이언트에 임의로 저장해놓은 데이터들을 DB로 저장 (userquestList, userquestOBJList)
    /// <para>(게임 종료 전 또는 일정 시간마다)</para>
    /// </summary>
    public void SaveInventory()
    {        
        int user_ID = DatabaseManager.Instance.userData.UID;

        var differences = Extensions.GetDifferences(
            originInventoryList,
            inventoryDataList,
            (original, updated) => original.Item_ID == updated.Item_ID,
            (original, updated) => original.Quantity != updated.Quantity
        );
                

        foreach (var item in differences.Modified)
        {                        
            string query =
                $"UPDATE inventory\n" +
                $"SET inventory.Quantity={item.Quantity}\n" +
                $"WHERE inventory.User_ID={user_ID} AND inventory.Item_ID={item.Item_ID};";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
        foreach (var item in differences.Added)
        {            
            string query =
                $"INSERT INTO inventory (inventory.User_ID, inventory.Item_ID, inventory.Quantity)\n" +
                $"VALUES ({user_ID}, {item.Item_ID}, {item.Quantity});";            
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
        foreach (var item in differences.Removed)
        {            
            string query =
                $"DELETE FROM inventory\n" +
                $"WHERE inventory.User_ID={user_ID} AND inventory.Item_ID={item.Item_ID};";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }        
    }
    public void SaveUserCraftTool()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;

        var differences = Extensions.GetDifferences(
            userCraftToolList,
            userCraftToolClient,
            (original, updated) => original.User_ID == updated.User_ID,
            (original, updated) => original.Item_ID != updated.Item_ID
        );
        foreach (var craft in differences.Modified)
        {
            string query =
                $"UPDATE usercrafttools\n" +
                $"SET usercrafttools.Item_ID={craft.Item_ID}\n" +
                $"WHERE usercrafttools.User_ID={user_ID};";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
    }
    private void AutoSave()
    {
        SaveInventory();
        SaveUserCraftTool();
    }
    private void OnApplicationQuit()
    {
        SaveInventory();
        SaveUserCraftTool();
    }
    #endregion
}
/// <summary>
/// 얻은 아이템의 분류를 위한 클래스
/// </summary>
public class AddItemClassfiy 
{
    public int item_ID { get; private set; }
    public int Amount { get; private set; }
    public bool isExist { get; private set; }

    public AddItemClassfiy(int item_ID, int amount, bool isExist)
    {
        this.item_ID = item_ID;
        this.Amount = amount;
        this.isExist = isExist;
    }
}
