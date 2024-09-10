using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// 유저가 퀘스트 목표를 얼마나 달성했는지
/// </summary>
public class UserQuestObjectivesData
{
    public int User_ID { get; set; }
    /// <summary>
    /// 목표 퀘스트 ID
    /// </summary>
    public int ObjectiveID { get; set; }
    /// <summary>
    /// 현재 달성된 목표의 수량
    /// </summary>
    public int CurrentAmount { get; set; }
    /// <summary>
    /// 달성했는지 여부
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
