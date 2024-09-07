using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ItemManager : MonoBehaviour
{    
    // TODO : 장비, 소비, 기타 아이템 정보를 받아서 특정 아이템을 장착 시, 캐릭터 스탯 반영
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


    #region 아이템 정보 가져오기
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
            //  실패
        }
    }

    /// <summary>
    /// 특정 아이템 이름 가져오기
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
            //  실패
            return string.Empty;
        }
    }
    /// <summary>
    /// 특정 아이템 타입(종류) 가져오기
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
            //  실패
            return (Item_Type)int.MaxValue;
        }
    }
    #endregion

    #region 장비, 소비, 기타 아이템 가져오기
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
            //  실패
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
            //  실패
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
            //  실패
            return null;
        }
    }
    #endregion
}
