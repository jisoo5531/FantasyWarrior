using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    /// <summary>
    /// ���� �߿� ���� �κ��丮 ����Ʈ
    /// </summary>
    public List<InventoryData> inventoryDataList { get; private set; }
    /// <summary>
    /// ������ �������� �����ϰų� ����� �κ��丮�� �������� �߰��Ǵ� ��Ȳ���� ���� ����Ʈ
    /// </summary>
    public List<AddItemClassfiy> addWhichItemList = new List<AddItemClassfiy>();
    /// <summary>
    /// ������ �� �񱳸� ���� ���� �κ��丮 ����Ʈ
    /// </summary>
    private List<InventoryData> originInventoryList;
    /// <summary>
    /// ���� ������ � ���� ���� �����͵��� ������ �ִ����� ���� ����Ʈ
    /// </summary>
    private List<UserCraftToolData> userCraftToolList;
    /// <summary>
    /// ���� ������ � ���� ���� �����͵��� ������ �ִ����� ���� ����Ʈ�� Ŭ���̾�Ʈ�� �ӽ÷� ����
    /// <para>�ΰ��ӿ��� �����</para>
    /// </summary>
    public List<UserCraftToolData> userCraftToolClient { get; private set; }

    private ItemData itemData;

    /// <summary>
    /// �������� ȹ�� ��, ������ �̺�Ʈ
    /// </summary>
    public event Action OnGetItem;
    /// <summary>
    /// ������ ȹ�� ��, ������ �����Ϳ� ���� �Ѱ��ֱ�
    /// </summary>
    public event Action<ItemData> OnGetItemData;
    /// <summary>
    /// ���۵����� ȹ�� ��, ������ �̺�Ʈ
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
    /// �κ��丮�� � �������� �߰��Ǿ����� Ȯ���� �޼���
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
            AddWhichItem(new AddItemClassfiy(itemData.Item_ID, amount, true));
        }
        else
        {
            // ������ ȹ�� ��, �κ��丮�� ���� �������̶��
            inventoryDataList.Add(new InventoryData(user_Id, itemData.Item_ID, amount));
            AddWhichItem(new AddItemClassfiy(itemData.Item_ID, amount, false));
        }
        CheckGetCraftTool(itemData.Item_ID);
        OnGetItemData?.Invoke(itemData);
        OnGetItem?.Invoke();
    }
    /// <summary>
    /// ���� �������� ���۵������� Ȯ��
    /// </summary>
    /// <param name="item_ID"></param>
    public void CheckGetCraftTool(int item_ID)
    {
        CraftToolData craftTool = ItemManager.Instance.GetCraftToolData(item_ID);
        if (craftTool == null) return;

        AddCraftItem(craftTool);
    }
    /// <summary>
    /// ���۵����� ����� �� ������ ������ �ִ� ���۵��� ����Ʈ(usercraftList)�� �߰�
    /// </summary>
    private void AddCraftItem(CraftToolData craftTool)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        int index = userCraftToolClient.FindIndex(x => x.CreftType.Equals(craftTool.CreftType));
        userCraftToolClient[index].Item_ID = craftTool.Item_ID;

        OnGetCraftItem?.Invoke();
    }
    /// <summary>
    /// ��� ���� ��, �κ��丮�� �߰�
    /// </summary>
    /// <param name="item_ID"></param>
    /// <param name="amount"></param>
    public void GetItemUnEquip(int item_ID, int amount)
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
    /// ������ ������ ���۵������� ��������
    /// <para>���ÿ� Ŭ���̾�Ʈ�� �ӽ÷� �����ϴ� �͵� ����</para>
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
            //  ����
        }
    }
    #endregion

    #region �κ��丮 ����
    /// <summary>
    /// DB�� ���� ���� �ʰ� Ŭ���̾�Ʈ�� ���Ƿ� �����س��� �����͵��� DB�� ���� (userquestList, userquestOBJList)
    /// <para>(���� ���� �� �Ǵ� ���� �ð�����)</para>
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
/// ���� �������� �з��� ���� Ŭ����
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
