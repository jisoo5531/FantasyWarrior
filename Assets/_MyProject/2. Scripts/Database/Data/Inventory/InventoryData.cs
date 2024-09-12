using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryData
{
    public int User_ID { get; set; }
    public int Inventory_ID { get; set; }
    public int Item_ID { get; set; }
    public int Quantity { get; set; }

    public InventoryData(DataRow row) : this
        (
            int.Parse(row["user_id"].ToString()),
            int.Parse(row["item_id"].ToString()),
            int.Parse(row["quantity"].ToString())
        )
    { }

    public InventoryData(int user_ID, int item_ID, int quantity)
    {
        this.User_ID = user_ID;
        this.Item_ID = item_ID;
        this.Quantity = quantity;
    }
}
