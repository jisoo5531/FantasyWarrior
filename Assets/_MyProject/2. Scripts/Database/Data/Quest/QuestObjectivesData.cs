using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// 퀘스트 목표 정보
/// </summary>
public class QuestObjectivesData
{
    /// <summary>
    /// 목표 고유 ID
    /// </summary>
    public int ObjectiveID { get; set; }
    public int Quest_ID { get; set; }
    public Q_ObjectiveType ObjectiveType { get; set; }
    /// <summary>
    /// 대상이 되는 몬스터 ID
    /// </summary>
    public int Monster_ID { get; set; }
    /// <summary>
    /// 대상이 되는 아이템 ID
    /// </summary>
    public int Item_ID { get; set; }
    /// <summary>
    /// 얼만큼의 수량 또는 얼마나 해야 하는지
    /// </summary>
    public int ReqAmount { get; set; }
    /// <summary>
    /// 목표 설명
    /// </summary>
    public string DESC { get; set; }

    public QuestObjectivesData(DataRow row) : this
        (
            int.Parse(row["Objective_ID"].ToString()),
            int.Parse(row["Quest_ID"].ToString()),
            (Q_ObjectiveType)int.Parse(row["Objective_Type"].ToString()),
            int.TryParse(row["Monster_ID"]?.ToString(), out int monsterid) ? monsterid : 0,
            int.TryParse(row["Item_ID"]?.ToString(), out int itemid) ? itemid : 0,
            int.Parse(row["ReqAmount"].ToString()),
            row["Description"].ToString()
        )
    { }

    public QuestObjectivesData(int objectiveID, int quest_ID, Q_ObjectiveType objectiveType, int monster_ID, int item_ID, int reqAmount, string dESC)
    {
        this.ObjectiveID = objectiveID;
        this.Quest_ID = quest_ID;
        this.ObjectiveType = objectiveType;
        this.Monster_ID = monster_ID;
        this.Item_ID = item_ID;
        this.ReqAmount = reqAmount;
        this.DESC = dESC;
    }
}
/// <summary>
/// 퀘스트의 목표 유형
/// </summary>
public enum Q_ObjectiveType
{
    Kill,
    Collect,
    Visit
}