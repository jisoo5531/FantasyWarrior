using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ItemManager : MonoBehaviour
{        
    public static ItemManager Instance { get; private set; }

    /// <summary>
    /// ������ �����͵��� ��Ƴ��� ��ųʸ�
    /// <para>key�� ������ ID</para>
    /// </summary>
    public Dictionary<int, ItemData> Item_Dict { get; private set; }
    /// <summary>
    /// ��� ������ �����͵��� ��Ƴ��� ��ųʸ�
    /// <para>key�� ������ ID</para>
    /// </summary>
    private Dictionary<int, EquipItemData> EquipItem_Dict;
    /// <summary>
    /// �Һ� ������ �����͵��� ��Ƴ��� ��ųʸ�
    /// <para>key�� ������ ID</para>
    /// </summary>
    private Dictionary<int, ConsumpItemData> ConsumpItem_Dict;
    /// <summary>
    /// ��Ÿ ������ �����͵��� ��Ƴ��� ��ųʸ�
    /// <para>key�� ������ ID</para>
    /// </summary>
    private Dictionary<int, OtherItemData> OtherItem_Dict;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        
        //EventHandler.managerEvent.TriggerItemManagerInit();
    }
    public void Initialize()
    {
        GetItemFromDB();
        GetEquipItemFromDB();
        GetConsumpItemFromDB();
        GetOtherItemFromDB();
    }

    /// <summary>
    /// �ش� ��� �������� ���� ��������
    /// </summary>
    /// <param name="item_ID"></param>
    /// <returns></returns>
    public EquipItemData GetEquipItemData(int item_ID)
    {
        if (EquipItem_Dict.TryGetValue(item_ID, out EquipItemData equipItem))
        {
            return equipItem;
        }
        return null;
    }
    /// <summary>
    /// �Һ� ������ ���� ��������
    /// </summary>
    /// <param name="item_ID"></param>
    /// <returns></returns>
    public ConsumpItemData GetConsumpItemData(int item_ID)
    {
        if (ConsumpItem_Dict.TryGetValue(item_ID, out ConsumpItemData consumpItem))
        {
            return consumpItem;
        }
        return null;
    }
    /// <summary>
    /// ��Ÿ ������ ���� ��������
    /// </summary>
    /// <param name="item_ID"></param>
    /// <returns></returns>
    public OtherItemData GetOtherItemData(int item_ID)
    {
        if (OtherItem_Dict.TryGetValue(item_ID, out OtherItemData otherItem))
        {
            return otherItem;
        }
        return null;
    }

    /// <summary>
    /// Ư�� ������ �̸� ��������
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public string GetInventoryItemNameFromDB(int itemID)
    {
        Debug.Log($"���� : {itemID}");
        Debug.Log(Item_Dict == null);
        if (Item_Dict.TryGetValue(itemID, out ItemData item))
        {
            return item.Item_Name;
        }
        return string.Empty;
    }
    /// <summary>
    /// Ư�� ������ Ÿ��(����) ��������
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public Item_Type? GetInventoryItemTypeFromDB(int itemID)
    {
        if (Item_Dict.TryGetValue(itemID, out ItemData item))
        {
            return item.Item_Type;
        }
        return null;
    }





    #region DB
    /// <summary>
    /// �����۵��� ����Ʈ�� ��������
    /// </summary>
    /// <returns></returns>
    private void GetItemFromDB()
    {        
        Item_Dict = new Dictionary<int, ItemData>();
        string query =
            $"SELECT *\n" +
            $"FROM items;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int id = int.Parse(row["Item_ID"].ToString());
                Item_Dict.Add(id, new ItemData(row));
            }
        }
        else
        {
            //  ����            
        }
    }
    /// <summary>
    /// ��� ������ ������ ��������
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    private void GetEquipItemFromDB()
    {        
        EquipItem_Dict = new Dictionary<int, EquipItemData>();

        string query =
            $"SELECT *\n" +
            $"FROM equipmentitems;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int id = int.Parse(row["Item_ID"].ToString());
                EquipItem_Dict.Add(id, new EquipItemData(row));                
            }
        }
        else
        {
            //����
        }
    }
    private void GetConsumpItemFromDB()
    {        
        ConsumpItem_Dict = new Dictionary<int, ConsumpItemData>();
        string query =
            $"SELECT *\n" +
            $"FROM consumitems;";        
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int id = int.Parse(row["Item_ID"].ToString());
                ConsumpItem_Dict.Add(id, new ConsumpItemData(row));                
            }
        }
        else
        {
            //  ����
        }
    }
    private void GetOtherItemFromDB()
    {        
        OtherItem_Dict = new Dictionary<int, OtherItemData>();
        string query =
            $"SELECT *\n" +
            $"FROM otheritems;";    
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int id = int.Parse(row["Item_ID"].ToString());
                OtherItem_Dict.Add(id, new OtherItemData(row));                
            }
        }
        else
        {
            //  ����
        }
    }
    #endregion
}
