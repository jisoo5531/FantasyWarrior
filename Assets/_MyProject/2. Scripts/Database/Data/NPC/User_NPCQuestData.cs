using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class User_NPCQuestData
{
    public int NPC_ID { get; set; }
    public int Quest_ID { get; set; }
    public bool IsComplete { get; set; }
    public int User_Id { get; set; }

    public User_NPCQuestData(DataRow row) : this
        (
            int.Parse(row["NPC_ID"].ToString()),
            int.Parse(row["Quest_ID"].ToString()),
            bool.Parse(row["IsComplete"].ToString()),
            int.Parse(row["user_id"].ToString())
        )
    { }

    public User_NPCQuestData(int nPC_ID, int quest_ID, bool isComplete, int user_Id)
    {
        this.NPC_ID = nPC_ID;
        this.Quest_ID = quest_ID;
        this.IsComplete = isComplete;
        this.User_Id = user_Id;
    }
}
