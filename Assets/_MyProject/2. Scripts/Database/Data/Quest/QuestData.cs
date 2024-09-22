using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// 퀘스트를 관리하는 정보
/// </summary>
public class QuestData
{
    public int Quest_ID { get; set; }
    public string Quest_Name { get; set; }
    public QuestType questType { get; set; }
    public string DESC { get; set; }
    public int Reward_Exp { get; set; }    
    public int Reward_Gold { get; set; }    
    public int RewardItemID { get; set; }    
    public bool IsRepeatable { get; set; }

    public QuestData(DataRow row) : this
        (  
            int.Parse(row["Quest_ID"].ToString()),
            row["Quest_Name"].ToString(),
            (QuestType)Enum.Parse(typeof(QuestType), row["Quest_Type"].ToString()),            
            row["Description"].ToString(),
            int.Parse(row["Reward_Exp"].ToString()),
            int.Parse(row["Reward_Gold"].ToString()),
            int.TryParse(row["RewardItem_ID"].ToString(), out int rewarditem) ? rewarditem : 0,
            bool.Parse(row["IsRepeatable"].ToString())
        )
    { }
    public QuestData(int quest_ID, string quest_Name, QuestType questType, string dESC, int reward_Exp, int reward_Gold, int rewardItemID, bool isRepeatable)
    {
        this.Quest_ID = quest_ID;
        this.Quest_Name = quest_Name;
        this.questType = questType;
        this.DESC = dESC;
        this.Reward_Exp = reward_Exp;
        this.Reward_Gold = reward_Gold;
        this.RewardItemID = rewardItemID;
        this.IsRepeatable = isRepeatable;
    }
}
public enum QuestType
{
    Main,
    Side,
    Daily
}