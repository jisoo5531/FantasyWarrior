using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class NPCQuestData
{
    public int NPC_ID { get; set; }
    public int Quest_ID { get; set; }
    public bool IsComplete { get; set; }

    public NPCQuestData(DataRow row) : this
        (
            int.Parse(row["NPC_ID"].ToString()),
            int.Parse(row["Quest_ID"].ToString()),
            bool.Parse(row["IsComplete"].ToString())
        )
    { }

    public NPCQuestData(int nPC_ID, int quest_ID, bool isComplete)
    {
        this.NPC_ID = nPC_ID;
        this.Quest_ID = quest_ID;
        this.IsComplete = isComplete;
    }
}
