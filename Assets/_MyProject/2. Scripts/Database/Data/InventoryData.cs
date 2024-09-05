using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryData
{
    public int Inventory_ID { get; set; }
    public int Item_ID { get; set; }
    public int Quantity { get; set; }

    public InventoryData(DataRow row) : this
        (
            int.Parse(row["inventory_id"].ToString()),
            int.Parse(row["item_id"].ToString()),
            int.Parse(row["quantity"].ToString())
        )
    { }

    public InventoryData(int inventory_ID, int item_ID, int quantity)
    {
        Inventory_ID = inventory_ID;
        Item_ID = item_ID;
        Quantity = quantity;
    }
}
