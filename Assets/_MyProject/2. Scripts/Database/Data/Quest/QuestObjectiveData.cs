using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// ����Ʈ ��ǥ ����
/// </summary>
public class QuestObjectiveData
{
    /// <summary>
    /// ��ǥ ���� ID
    /// </summary>
    public int ObjectiveID { get; set; }
    public int Quest_ID { get; set; }
    public Q_ObjectiveType ObjectiveType { get; set; }
    /// <summary>
    /// ��ǥ ����
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
/// ����Ʈ�� ��ǥ ����
/// </summary>
public enum Q_ObjectiveType
{
    Kill,
    Collect,
    Talk,
    Visit
}