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

    public ItemData(DataRow row) : this
        (
            int.Parse(row["item_id"].ToString()),
            row["item_name"].ToString(),
            (Item_Type)int.Parse(row["item_type"].ToString()),
            row["item_description"].ToString(),
            (Rarity)int.Parse(row["rarity"].ToString())
        )
    { }
    public ItemData(int item_ID, string item_Name, Item_Type item_Type, string item_Description, Rarity rarity)
    {
        this.Item_ID = item_ID;
        this.Item_Name = item_Name;
        this.Item_Type = item_Type;
        this.Item_Description = item_Description;
        this.Rarity = rarity;
    }
}
