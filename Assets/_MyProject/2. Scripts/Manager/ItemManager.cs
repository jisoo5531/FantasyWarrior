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
    public List<ItemData> itemDataList { get; private set; }
    public List<EquipItemData> equipItemList { get; private set; }
    public List<ConsumpItemData> consumpItemList { get; private set; }
    public List<OtherItemData> otherItemList { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GetItemFromDB();
        GetEquipItemFromDB();
        GetConsumpItemFromDB();
        GetOtherItemFromDB();
        EventHandler.managerEvent.TriggerItemManagerInit();
    }    

    /// <summary>
    /// 아이템들의 리스트를 가져오기
    /// </summary>
    /// <returns></returns>
    private List<ItemData> GetItemFromDB()
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
        int index = itemDataList.FindIndex(x => x.Item_ID.Equals(itemID));
        if (index >= 0)
        {
            return itemDataList[index].Item_Name;
        }
        return string.Empty;
    }
    /// <summary>
    /// 특정 아이템 타입(종류) 가져오기
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public Item_Type? GetInventoryItemTypeFromDB(int itemID)
    {
        int index = itemDataList.FindIndex(x => x.Item_ID.Equals(itemID));
        if (index >= 0)
        {
            return itemDataList[index].Item_Type;
        }
        return null;
    }

    #region 장비, 소비, 기타 아이템 가져오기
    /// <summary>
    /// 장비 아이템 데이터 가져오기
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    private void GetEquipItemFromDB()
    {
        equipItemList = new List<EquipItemData>();

        string query =
            $"SELECT *\n" +
            $"FROM equipmentitems;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                equipItemList.Add(new EquipItemData(row));
            }
        }
        else
        {
            //실패
        }

    }
    private void GetConsumpItemFromDB()
    {
        consumpItemList = new List<ConsumpItemData>();
        string query =
            $"SELECT *\n" +
            $"FROM consumitems;";        
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                consumpItemList.Add(new ConsumpItemData(row));
            }
        }
        else
        {
            //  실패
        }
    }
    private void GetOtherItemFromDB()
    {
        otherItemList = new List<OtherItemData>();
        string query =
            $"SELECT *\n" +
            $"FROM otheritems;";    
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                otherItemList.Add(new OtherItemData(row));
            }
        }
        else
        {
            //  실패
        }
    }
    #endregion
}
