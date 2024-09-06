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
