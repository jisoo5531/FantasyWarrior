using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    public List<ItemData> itemDataList = new List<ItemData>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GetItemDataFromDatabase();
        Debug.Log(itemDataList.Count);
    }

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
            //  ½ÇÆÐ
        }
    }
}
