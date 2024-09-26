using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum Item_Type
{
    Equipment,
    Consump,
    Other
}
public enum Rarity
{
    Normal,
    Rare,
    Epic
}
public class ItemData
{
    public int Item_ID { get; set; }
    public string Item_Name { get; set; }
    public Item_Type Item_Type { get; set; }
    public string Item_Description { get; set; }
    public Rarity Rarity { get; set; }
    public int SellPrice { get; set; }

    public ItemData(DataRow row) : this
        (
            int.Parse(row["item_id"].ToString()),
            row["item_name"].ToString(),
            (Item_Type)System.Enum.Parse(typeof(Item_Type), row["item_type"].ToString()),            
            row["item_description"].ToString(),            
            (Rarity)System.Enum.Parse(typeof(Rarity), row["rarity"].ToString()),
            int.Parse(row["Sell_Price"].ToString())
        )
    { }

    public ItemData(int item_ID, string item_Name, Item_Type item_Type, string item_Description, Rarity rarity, int sellPrice)
    {
        Item_ID = item_ID;
        Item_Name = item_Name;
        Item_Type = item_Type;
        Item_Description = item_Description;
        Rarity = rarity;
        SellPrice = sellPrice;
    }
}
