using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// 퀘스트 목표 정보
/// </summary>
public class QuestObjectiveData
{
    /// <summary>
    /// 목표 고유 ID
    /// </summary>
    public int ObjectiveID { get; set; }
    public int Quest_ID { get; set; }
    public Q_ObjectiveType ObjectiveType { get; set; }
    /// <summary>
    /// 목표 설명
    /// </summary>
    public string DESC { get; set; }

    public QuestObjectiveData(DataRow row) : this
        (
            int.Parse(row["Objective_ID"].ToString()),
            int.Parse(row["Quest_ID"].ToString()),
            (Q_ObjectiveType)System.Enum.Parse(typeof(Q_ObjectiveType), row["Objective_Type"].ToString()),            
            row["Description"].ToString()
        )
    { }

    public QuestObjectiveData(int objectiveID, int quest_ID, Q_ObjectiveType objectiveType, string dESC)
    {
        this.ObjectiveID = objectiveID;
        this.Quest_ID = quest_ID;
        this.ObjectiveType = objectiveType;
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
    Talk,
    Visit
}