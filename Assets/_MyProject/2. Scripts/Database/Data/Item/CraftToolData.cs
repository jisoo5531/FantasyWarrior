using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CraftToolData
{
    public int Tool_ID { get; set; }
    public int Item_ID { get; set; }
    public CraftType CreftType { get; set; }

    public CraftToolData(DataRow row) : this
        (
            int.Parse(row["Tool_ID"].ToString()),
            int.Parse(row["Item_ID"].ToString()),
            (CraftType)System.Enum.Parse(typeof(CraftType), row["Craft_Type"].ToString())
        )
    { }
    public CraftToolData(int tool_ID, int item_ID, CraftType creftType)
    {
        Tool_ID = tool_ID;
        Item_ID = item_ID;
        CreftType = creftType;
    }
}
public enum CraftType
{
    Hammer,
    Fishing,
    CutDownTree,
    Harvest
}