using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private int itemQuantity;

    private void Awake()
    {
        Instance = this;
    }

    public void GetItem_InsertDatabase(ItemData itemData, int amount)
    {
        int user_Id = DatabaseManager.Instance.userData.UID;
        if (IsExistInventory(itemData))
        {
            string query =
                $"UPDATE inventory\n" +
                $"SET quantity={itemQuantity + amount}\n" +
                $"WHERE user_id={user_Id} AND item_id={itemData.Item_ID};";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
        else
        {
            string query =
                $"INSERT INTO Inventory (user_id, item_id, quantity)\n" +
                $"VALUES (1, {itemData.Item_ID}, 1);";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
    }
    private bool IsExistInventory(ItemData itemData)
    {
        int user_Id = DatabaseManager.Instance.userData.UID;

        string query =
            $"SELECT *\n" +
            $"FROM inventory\n" +
            $"WHERE user_id={user_Id} AND item_id={itemData.Item_ID};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            itemQuantity = int.Parse(row["quantity"].ToString());
            return true;
        }
        else
        {
            return false;
        }
    }
}
