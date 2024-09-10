using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// ����Ʈ ��ǥ ����
/// </summary>
public class QuestObjectivesData
{
    /// <summary>
    /// ��ǥ ���� ID
    /// </summary>
    public int ObjectiveID { get; set; }
    public int Quest_ID { get; set; }
    public Q_ObjectiveType ObjectiveType { get; set; }
    /// <summary>
    /// ����� �Ǵ� ID (��, ������ ��)
    /// </summary>
    public int TargetID { get; set; }
    /// <summary>
    /// ��ŭ�� ���� �Ǵ� �󸶳� �ؾ� �ϴ���
    /// </summary>
    public int ReqAmount { get; set; }
    /// <summary>
    /// ��ǥ ����
    /// </summary>
    public string DESC { get; set; }

    public QuestObjectivesData(DataRow row) : this
        (
            int.Parse(row["Objective_ID"].ToString()),
            int.Parse(row["Quest_ID"].ToString()),
            (Q_ObjectiveType)int.Parse(row["Objective_Type"].ToString()),
            int.Parse(row["Target_ID"].ToString()),
            int.Parse(row["ReqAmount"].ToString()),
            row["Description"].ToString()
        )
    { }

    public QuestObjectivesData(int objectiveID, int quest_ID, Q_ObjectiveType objectiveType, int targetID, int reqAmount, string dESC)
    {
        this.ObjectiveID = objectiveID;
        this.Quest_ID = quest_ID;
        this.ObjectiveType = objectiveType;
        this.TargetID = targetID;
        this.ReqAmount = reqAmount;
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
    Visit
}