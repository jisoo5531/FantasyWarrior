using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private List<InventoryData> originInventoryList;
    public List<InventoryData> inventoryDataList { get; private set; }
    /// <summary>
    /// 장착한 아이템을 해제하거나 등등의 인벤토리로 아이템이 추가되는 상황에서 쓰일 리스트
    /// </summary>
    public List<AddItemClassfiy> addWhichItemList = new List<AddItemClassfiy>();
    
    private ItemData itemData;

    /// <summary>
    /// 아이템을 획득 시, 실행할 이벤트
    /// </summary>
    public event Action OnGetItem;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InventoryManagerInit();
    }
    private void InventoryManagerInit()
    {
        GetDataFromDatabase();
        EventHandler.managerEvent.TriggerInventoryManagerInit();
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
    /// 몬스터를 잡고 아이템을 획득 시, 실행할 메서드
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
        OnGetItem?.Invoke();
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
    private void AutoSave()
    {
        SaveInventory();
    }
    private void OnApplicationQuit()
    {
        SaveInventory();
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
