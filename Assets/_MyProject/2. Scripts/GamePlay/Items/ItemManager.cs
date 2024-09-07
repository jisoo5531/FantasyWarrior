using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ItemManager : MonoBehaviour
{    
    // TODO : ���, �Һ�, ��Ÿ ������ ������ �޾Ƽ� Ư�� �������� ���� ��, ĳ���� ���� �ݿ�
    public static ItemManager Instance { get; private set; }

    public List<ItemData> itemDataList = new List<ItemData>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GetItemDataFromDatabase();        
    }


    #region ������ ���� ��������
    private void GetItemDataFromDatabase()
    {
        string query =
            $"SELECT *\n" +
            $"FROM items";

        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {            
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                itemDataList.Add(new ItemData(row));
            }
        }
        else
        {
            //  ����
        }
    }

    /// <summary>
    /// Ư�� ������ �̸� ��������
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
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
            //  ����
            return string.Empty;
        }
    }
    /// <summary>
    /// Ư�� ������ Ÿ��(����) ��������
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public Item_Type GetInventoryItemTypeFromDB(int itemID)
    {
        string query =
            $"SELECT items.Item_Type\n" +
            $"FROM items\n" +
            $"WHERE items.item_ID={itemID};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return (Item_Type)int.Parse(row["Item_Type"].ToString());
        }
        else
        {
            //  ����
            return (Item_Type)int.MaxValue;
        }
    }
    #endregion

    #region ���, �Һ�, ��Ÿ ������ ��������
    public EquipItemData GetEquipItemFromDB(int itemID)
    {
        string query =
            $"SELECT *\n" +
            $"FROM equipmentitems\n" +
            $"WHERE equipmentitems.item_id={itemID};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return new EquipItemData(row);
        }
        else
        {
            //  ����
            return null;
        }
    }
    public ConsumpItemData GetConsumpItemFromDB(int itemID)
    {
        string query =
            $"SELECT *\n" +
            $"FROM consumitems\n" +
            $"WHERE consumitems.item_id={itemID};";        
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return new ConsumpItemData(row);
        }
        else
        {
            //  ����
            return null;
        }
    }
    public OtherItemData GetOtherItemFromDB(int itemID)
    {
        string query =
            $"SELECT *\n" +
            $"FROM otheritems\n" +
            $"WHERE otheritems.item_id={itemID};";        
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return new OtherItemData(row);
        }
        else
        {
            //  ����
            return null;
        }
    }
    #endregion
}
