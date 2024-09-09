using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<InventoryData> inventoryDataList = new List<InventoryData>();
    public List<int> addWhichItemList = new List<int>();
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
        GetDataFromDatabase();
    }

    public void AddWhichItem(int itemID)
    {
        addWhichItemList.Add(itemID);
    }
    public void ClearAddWhichItemList()
    {
        addWhichItemList = new List<int>();
    }

    /// <summary>
    /// <para>인벤토리 데이터를 가져오는 메서드</para>
    /// </summary>
    public List<InventoryData> GetDataFromDatabase()
    {
        string query =
            $"SELECT *\n" +
            $"FROM inventory;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            inventoryDataList.Clear();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
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
    /// 아이템을 획득 시, 실행할 메서드
    /// </summary>
    /// <param name="itemData"></param>
    /// <param name="amount"></param>
    public void GetItem(ItemData itemData, int amount)
    {
        this.itemData = itemData;
        int user_Id = DatabaseManager.Instance.userData.UID;
        int index = FindItemIndexInventory(itemData);                
        if (index >= 0)
        {
            // 아이템 획득 시, 인벤토리에 있는 아이템이라면
            string query =
                $"UPDATE inventory\n" +
                $"SET quantity={inventoryDataList[index].Quantity + amount}\n" +
                $"WHERE user_id={user_Id} AND item_id={itemData.Item_ID};";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);            
        }
        else
        {
            // 아이템 획득 시, 인벤토리에 없는 아이템이라면
            string query =
                $"INSERT INTO Inventory (user_id, item_id, quantity)\n" +
                $"VALUES (1, {itemData.Item_ID}, {amount});";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
            AddWhichItem(itemData.Item_ID);
        }
        OnGetItem?.Invoke();
    }
    /// <summary>
    /// 장비를 장착 시, 인벤토리에서 해당 아이템 사라지게 할 메서드
    /// </summary>
    public void EquipItemUpdateInventory(int itemID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"DELETE FROM inventory\n" +
            $"WHERE user_id={user_ID} AND item_ID={itemID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);        
    }
    private int FindItemIndexInventory(ItemData itemData)
    {        
        return inventoryDataList.FindIndex((x) => { return x.Item_ID.Equals(itemData.Item_ID); });
    }        
    public int GetItemQuantity(int itemID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"SELECT inventory.Quantity\n" +
            $"FROM inventory\n" +
            $"WHERE inventory.User_ID={user_ID} AND inventory.Item_ID={itemID};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return int.Parse(row["Quantity"].ToString());
        }
        else
        {
            return 0;
        }

    }
}
