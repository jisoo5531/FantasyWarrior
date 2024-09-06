using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class OtherItemData
{
    public int Others_ID { get; set; }
    public int Item_ID { get; set; }
    public string Desc { get; set; }

    public OtherItemData(DataRow row) : this
        (
            int.Parse(row["Others_ID"].ToString()),
            int.Parse(row["Item_ID"].ToString()),
            row["Description"].ToString()
        )
    { }
    public OtherItemData(int others_ID, int item_ID, string desc)
    {
        Others_ID = others_ID;
        Item_ID = item_ID;
        Desc = desc;
    }
}
