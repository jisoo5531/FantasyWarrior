using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<InventoryData> inventoryDataList = new List<InventoryData>();
    private ItemData itemData;

    public event Action OnGetItem;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GetDataFromDatabase();
    }

    private void GetDataFromDatabase()
    {
        string query =
            $"SELECT *\n" +
            $"FROM inventory";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            inventoryDataList.Clear();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                inventoryDataList.Add(new InventoryData(row));
            }
        }
        else
        {            
        }
    }

    public void GetItem(ItemData itemData, int amount)
    {
        this.itemData = itemData;
        int user_Id = DatabaseManager.Instance.userData.UID;
        int index = FindItemIndexInventory(itemData);
        Debug.Log($"번호 : {index}");
        if (index >= 0)
        {
            Debug.Log("여기?");
            string query =
                $"UPDATE inventory\n" +
                $"SET quantity={inventoryDataList[index].Quantity + amount}\n" +
                $"WHERE user_id={user_Id} AND item_id={itemData.Item_ID};";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
            GetDataFromDatabase();
        }
        else
        {
            Debug.Log("여기!");
            string query =
                $"INSERT INTO Inventory (user_id, item_id, quantity)\n" +
                $"VALUES (1, {itemData.Item_ID}, {amount});";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
            GetDataFromDatabase();
        }

        OnGetItem?.Invoke();
    }
    private int FindItemIndexInventory(ItemData itemData)
    {        
        return inventoryDataList.FindIndex((x) => { return x.Item_ID.Equals(itemData.Item_ID); });
    }

    public string GetInventoryItemNameFromDB(int itemID)
    {
        string query =
            $"SELECT items.item_name\n" +
            $"FROM items\n" +
            $"WHERE items.Item_ID={itemID};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return row["item_name"].ToString();
        }
        else
        {
            //  실패
            return string.Empty;
        }
    }    
}
