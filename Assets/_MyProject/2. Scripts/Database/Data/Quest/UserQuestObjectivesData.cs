using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// ������ ����Ʈ ��ǥ�� �󸶳� �޼��ߴ���
/// </summary>
public class UserQuestObjectivesData
{
    public int User_ID { get; set; }
    /// <summary>
    /// ��ǥ ����Ʈ ID
    /// </summary>
    public int ObjectiveID { get; set; }
    /// <summary>
    /// ���� �޼��� ��ǥ�� ����
    /// </summary>
    public int CurrentAmount { get; set; }
    /// <summary>
    /// �޼��ߴ��� ����
    /// </summary>
    public bool IsCompleted { get; set; }

    public UserQuestObjectivesData(DataRow row) : this
        (
            int.Parse(row["User_ID"].ToString()),
            int.Parse(row["Objective_ID"].ToString()),
            int.Parse(row["Current_Amount"].ToString()),
            bool.Parse(row["IsCompleted"].ToString())
        )
    { }
    public UserQuestObjectivesData(int user_ID, int objectiveID, int currentAmount, bool isCompleted)
    {
        this.User_ID = user_ID;
        this.ObjectiveID = objectiveID;
        this.CurrentAmount = currentAmount;
        this.IsCompleted = isCompleted;
    }
}
