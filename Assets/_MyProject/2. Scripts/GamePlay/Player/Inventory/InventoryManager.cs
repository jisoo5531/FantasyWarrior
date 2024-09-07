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

    /// <summary>
    /// �������� ȹ�� ��, ������ �̺�Ʈ
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

    /// <summary>
    /// <para>�κ��丮 �����͸� �������� �޼���</para>
    /// �� �޼��带 �����Ͽ� �κ��丮 UI�� �̺�Ʈ�� Ȱ���Ͽ� ������Ʈ�� �����Ѵ�.
    /// </summary>
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
        OnGetItem?.Invoke();
    }

    /// <summary>
    /// �������� ȹ�� ��, ������ �޼���
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
            // ������ ȹ�� ��, �κ��丮�� �ִ� �������̶��
            string query =
                $"UPDATE inventory\n" +
                $"SET quantity={inventoryDataList[index].Quantity + amount}\n" +
                $"WHERE user_id={user_Id} AND item_id={itemData.Item_ID};";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
            GetDataFromDatabase();
        }
        else
        {
            // ������ ȹ�� ��, �κ��丮�� ���� �������̶��
            string query =
                $"INSERT INTO Inventory (user_id, item_id, quantity)\n" +
                $"VALUES (1, {itemData.Item_ID}, {amount});";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
            GetDataFromDatabase();
        }
    }
    /// <summary>
    /// ��� ���� ��, �κ��丮���� �ش� ������ ������� �� �޼���
    /// </summary>
    public void EquipItemUpdateInventory(int itemID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"DELETE FROM inventory\n" +
            $"WHERE user_id={user_ID} AND item_ID={itemID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        GetDataFromDatabase();
    }
    private int FindItemIndexInventory(ItemData itemData)
    {        
        return inventoryDataList.FindIndex((x) => { return x.Item_ID.Equals(itemData.Item_ID); });
    }        
}
