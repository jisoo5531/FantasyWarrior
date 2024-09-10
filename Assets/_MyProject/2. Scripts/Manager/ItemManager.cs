using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ItemManager : MonoBehaviour
{        
    public static ItemManager Instance { get; private set; }

    /// <summary>
    /// 아이템 테이블의 데이터들을 담아놓은 리스트
    /// </summary>
    private List<ItemData> itemDataList;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GetItemDataFromDatabase();        
    }


    #region 아이템 정보 가져오기
    /// <summary>
    /// 아이템들의 리스트를 가져오기
    /// </summary>
    /// <returns></returns>
    public List<ItemData> GetItemDataFromDatabase()
    {
        itemDataList = new List<ItemData>();
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
            return itemDataList;
        }
        else
        {
            //  실패
            return null;
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
    /// <summary>
    /// 장비 아이템 데이터 가져오기
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public EquipItemData GetEquipItemFromDB(int itemID)
    {
        if (itemID == 0)
        {
            return null;
        }
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
