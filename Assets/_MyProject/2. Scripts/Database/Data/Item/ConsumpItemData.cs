using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ConsumpItemData
{
    public int Consum_ID { get; set; }
    public int Item_ID { get; set; }
    public string Effect { get; set; }
    public int Recovery_Amount { get; set; }
    public int Duration { get; set; }

    public ConsumpItemData(DataRow row) : this
        (
            int.Parse(row["Consum_ID"].ToString()),
            int.Parse(row["Item_ID"].ToString()),
            row["Effect"].ToString(),
            int.Parse(row["Recovery_Amount"].ToString()),
            int.Parse(row["Duration"].ToString())
        )
    { }

    public ConsumpItemData(int consum_ID, int item_ID, string effect, int recovery_Amount, int duration)
    {
        this.Consum_ID = consum_ID;
        this.Item_ID = item_ID;
        this.Effect = effect;
        this.Recovery_Amount = recovery_Amount;
        this.Duration = duration;
    }
}
