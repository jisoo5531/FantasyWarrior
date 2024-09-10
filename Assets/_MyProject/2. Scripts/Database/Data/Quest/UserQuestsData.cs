using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// 유저의 퀘스트 진행상태
/// <para>유저가 수행중인 퀘스의 상태(수락, 진행 중, 완료)를 관리</para>
/// </summary>
public class UserQuestsData
{
    public int User_ID { get; set; }
    public int Quest_ID { get; set; }
    public Q_Status questStatus { get; set; }

    public UserQuestsData(DataRow row) : this
        (
            int.Parse(row["User_ID"].ToString()),
            int.Parse(row["Quest_ID"].ToString()),
            (Q_Status)int.Parse(row["Status"].ToString())
        )
    { }

    public UserQuestsData(int user_ID, int quest_ID, Q_Status questStatus)
    {
        this.User_ID = user_ID;
        this.Quest_ID = quest_ID;
        this.questStatus = questStatus;
    }
}
/// <summary>
/// 현재 퀘스트가 어떤 상태인지 (수락, 진행, 완료)
/// </summary>
public enum Q_Status
{    
    InProgress,
    Completed
}
