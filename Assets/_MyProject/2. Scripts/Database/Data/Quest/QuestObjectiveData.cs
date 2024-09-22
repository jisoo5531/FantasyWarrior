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
    /// ����� �Ǵ� ���� ID
    /// </summary>
    public int Monster_ID { get; set; }
    /// <summary>
    /// ����� �Ǵ� ������ ID
    /// </summary>
    public int Item_ID { get; set; }
    /// <summary>
    /// ����� �Ǵ� NPC ID
    /// </summary>
    public int NPC_ID { get; set; }
    /// <summary>
    /// ��ŭ�� ���� �Ǵ� �󸶳� �ؾ� �ϴ���
    /// </summary>
    public int ReqAmount { get; set; }
    /// <summary>
    /// ��ǥ ����
    /// </summary>
    public string DESC { get; set; }

    public QuestObjectiveData(DataRow row) : this
        (
            int.Parse(row["Objective_ID"].ToString()),
            int.Parse(row["Quest_ID"].ToString()),
            (Q_ObjectiveType)int.Parse(row["Objective_Type"].ToString()),
            int.TryParse(row["Monster_ID"]?.ToString(), out int monsterid) ? monsterid : 0,
            int.TryParse(row["Item_ID"]?.ToString(), out int itemid) ? itemid : 0,
            int.TryParse(row["NPC_ID"]?.ToString(), out int npcid) ? npcid : 0,
            int.Parse(row["Required_Amount"].ToString()),
            row["Description"].ToString()
        )
    { }

    public QuestObjectiveData(int objectiveID, int quest_ID, Q_ObjectiveType objectiveType, int monster_ID, int item_ID, int nPC_ID, int reqAmount, string dESC)
    {
        this.ObjectiveID = objectiveID;
        this.Quest_ID = quest_ID;
        this.ObjectiveType = objectiveType;
        this.Monster_ID = monster_ID;
        this.Item_ID = item_ID;
        this.NPC_ID = nPC_ID;
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
    Talk
}