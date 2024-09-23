using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class UserCraftToolData
{
    public int Tool_ID { get; set; }
    public int User_ID { get; set; }
    public int Item_ID { get; set; }
    public CraftType CreftType { get; set; }

    public UserCraftToolData(DataRow row) : this
        (
            int.Parse(row["Tool_ID"].ToString()),
            int.Parse(row["User_ID"].ToString()),
            int.Parse(row["Item_ID"].ToString()),
            (CraftType)System.Enum.Parse(typeof(CraftType), row["Craft_Type"].ToString())
        )
    { }

    public UserCraftToolData(int tool_ID, int user_ID, int item_ID, CraftType creftType)
    {
        this.Tool_ID = tool_ID;
        this.User_ID = user_ID;
        this.Item_ID = item_ID;
        this.CreftType = creftType;
    }
}

