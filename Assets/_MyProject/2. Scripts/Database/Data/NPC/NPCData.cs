using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class NPCData 
{
    public int NPC_ID { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }

    public NPCData(DataRow row) : this
        (
            int.Parse(row["NPC_ID"].ToString()),
            row["Name"].ToString(),
            row["Description"].ToString()
        )
    { }

    public NPCData(int nPC_ID, string name, string desc)
    {
        this.NPC_ID = nPC_ID;
        this.Name = name;
        this.Desc = desc;
    }
}
