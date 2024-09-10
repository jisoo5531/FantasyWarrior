using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

/// <summary>
/// ������ ����Ʈ �������
/// <para>������ �������� ������ ����(����, ���� ��, �Ϸ�)�� ����</para>
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
/// ���� ����Ʈ�� � �������� (����, ����, �Ϸ�)
/// </summary>
public enum Q_Status
{    
    InProgress,
    Completed
}
