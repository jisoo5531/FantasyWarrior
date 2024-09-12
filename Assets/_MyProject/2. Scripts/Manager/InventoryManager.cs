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
    /// ������ �������� �����ϰų� ����� �κ��丮�� �������� �߰��Ǵ� ��Ȳ���� ���� ����Ʈ
    /// </summary>
    public List<int> addWhichItemList = new List<int>();
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
    /// �κ��丮�� � �������� �߰��Ǿ����� Ȯ���� �޼���
    /// </summary>
    /// <param name="itemID"></param>
    public void AddWhichItem(int itemID)
    {
        addWhichItemList.Add(itemID);
    }
    public void ClearAddWhichItemList()
    {
        addWhichItemList = new List<int>();
    }

    /// <summary>
    /// <para>�κ��丮 �����͸� �������� �޼���</para>
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
    /// �������� ȹ�� ��, ������ �޼���
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
            // ������ ȹ�� ��, �κ��丮�� �ִ� �������̶��
            inventoryDataList[index].Quantity += amount;
        }
        else
        {
            // ������ ȹ�� ��, �κ��丮�� ���� �������̶��
            inventoryDataList.Add(new InventoryData(user_Id, itemData.Item_ID, amount));
            AddWhichItem(itemData.Item_ID);
        }
        OnGetItem?.Invoke();
    }
    public void GetItem(int item_ID, int amount)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        inventoryDataList.Add(new InventoryData(user_ID, item_ID, amount));
    }
    /// <summary>
    /// ��� ���� ��, �κ��丮���� �ش� ������ ������� �� �޼���
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
    public int GetItemQuantity(int itemID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;

        int index = inventoryDataList.FindIndex(x => x.User_ID.Equals(user_ID) && x.Item_ID.Equals(itemID));
        if (index >= 0)
        {
            return inventoryDataList[index].Quantity;
        }
        return 0;
    }
    #region �κ��丮 ����
    /// <summary>
    /// DB�� ���� ���� �ʰ� Ŭ���̾�Ʈ�� ���Ƿ� �����س��� �����͵��� DB�� ���� (userquestList, userquestOBJList)
    /// <para>(���� ���� �� �Ǵ� ���� �ð�����)</para>
    /// </summary>
    public void SaveQuestProgress()
    {
        Debug.Log("���� ��� ����.");
        int user_ID = DatabaseManager.Instance.userData.UID;
        var (added, removed, modified) = originInventoryList.GetDifferences(inventoryDataList);

        foreach (var item in added)
        {
            string query =
                $"INSERT INTO inventory (inventory.User_ID, inventory.Item_ID, inventory.Quantity)\n" +
                $"VALUES ({user_ID}, {item.Item_ID}, {item.Quantity});";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
        foreach (var item in removed)
        {
            string query =
                $"DELETE FROM inventory\n" +
                $"WHERE inventory.User_ID={user_ID} AND inventory.Item_ID={item.Item_ID};";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
        foreach (var item in modified)
        {
            string query =
                $"UPDATE inventory\n" +
                $"SET inventory.Quantity={item.Quantity}\n" +
                $"WHERE inventory.User_ID={user_ID} AND inventory.Item_ID={item.Item_ID};";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
    }
    private void AutoSave()
    {
        SaveQuestProgress();
    }
    private void OnApplicationQuit()
    {
        SaveQuestProgress();
    }
    #endregion
}
