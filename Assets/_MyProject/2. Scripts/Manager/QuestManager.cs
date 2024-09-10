using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    private List<QuestsData> questsDataList = new List<QuestsData>();
    private List<UserQuestsData> userQuestsList = new List<UserQuestsData>();

    private void Awake()
    {
        Instance = this;        
    }

    /// <summary>
    /// ����Ʈ ����Ʈ ��������
    /// </summary>
    /// <returns></returns>
    public List<QuestsData> GetQuestListFromDB()
    {        
        questsDataList = new List<QuestsData>();
        string query =
            $"SELECT *\n" +
            $"FROM quests\n" +
            $"WHERE quests.Quest_ID=1;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {            
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                questsDataList.Add(new QuestsData(row));
            }
            return questsDataList;
        }
        else
        {
            //  ����
            return null;
        }
    }
    /// <summary>
    /// Ư�� ID�� �´� ����Ʈ ������ ��������
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public QuestsData GetQuestData(int quest_ID)
    {
        string query =
            $"SELECT *\n" +
            $"FROM quests\n" +
            $"WHERE quests.Quest_ID=1;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return new QuestsData(row);
        }
        else
        {
            //  ����
            return null;
        }
    }
    /// <summary>
    /// ���� ������ ���� ���� ����Ʈ�� ������ ��������
    /// </summary>
    /// <returns></returns>
    public List<UserQuestsData> GetUserQuest()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"SELECT *\n" +
            $"FROM userquests\n" +
            $"WHERE userquests.User_ID={user_ID};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {            
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                userQuestsList.Add(new UserQuestsData(row));
            }
            return userQuestsList;
        }
        else
        {
            //  ����
            return null;
        }
    }
    /// <summary>
    /// ����Ʈ�� �����ϸ� userquests ���̺� (���� ������ ���� ���� ����Ʈ ����)�� ����
    /// </summary>
    /// <param name="quest_ID"></param>
    public void AcceptQuest(int quest_ID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"INSERT INTO userquests (userquests.User_ID, userquests.Quest_ID, userquests.`Status`)\n" +
            $"VALUES ({user_ID}, {quest_ID}, 0);";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
    }    

    /// <summary>
    /// ���� ����Ʈ�� ����(����, ����, �Ϸ�)�� �������� �޼���
    /// </summary>
    /// <returns></returns>
    public Q_Status? GetQuestStatus(int quest_ID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"SELECT userquests.`Status`\n" +
            $"FROM userquests\n" +
            $"WHERE userquests.User_ID={user_ID} AND userquests.Quest_ID={quest_ID};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return (Q_Status)int.Parse(row["Status"].ToString());
        }
        else
        {
            //  ����
            return null;
        }
    }
    /// <summary>
    /// ��ǥ ����Ʈ�� ID ��������
    /// </summary>
    /// <param name="quest_ID">�˰��� �ϴ� ����Ʈ ID</param>
    /// <returns></returns>
    public int? GetObjectiveID(int quest_ID)
    {        
        string query =
            $"SELECT questobjectives.Objective_ID\n" +
            $"FROM questobjectives\n" +
            $"WHERE questobjectives.Quest_ID={quest_ID};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return int.Parse(row["Objective_ID"].ToString());
        }
        else
        {
            //  ����
            return null;
        }
    }

    /// <summary>
    /// ���� ����Ʈ ���൵
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public int? GetCurrentQuestProgress(int quest_ID)
    {
        int? questObjID = GetObjectiveID(quest_ID);
        if (questObjID == null)
        {
            Debug.LogError("�߸��� ����Ʈ ���̵�");
            return null;
        }
        int user_ID = DatabaseManager.Instance.userData.UID;

        string query =
            $"SELECT userquestobjectives.Current_Amount\n" +
            $"FROM userquestobjectives\n" +
            $"WHERE userquestobjectives.User_ID={user_ID} " +
            $"AND userquestobjectives.Objective_ID={questObjID};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return int.Parse(row["Current_Amount"].ToString());
        }
        else
        {
            //  ����
            return null;
        }
    }
    /// <summary>
    /// ����Ʈ�� �Ϸ� ���� ��������
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public int? GetRequireCompleteQuest(int quest_ID)
    {
        int? questObjID = GetObjectiveID(quest_ID);
        if (questObjID == null)
        {
            Debug.LogError("�߸��� ����Ʈ ���̵�");
            return null;
        }        

        string query =
            $"SELECT questobjectives.Required_Amount\n" +
            $"FROM questobjectives\n" +
            $"WHERE questobjectives.Objective_ID={questObjID};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return int.Parse(row["Required_Amount"].ToString());
        }
        else
        {
            //  ����
            return null;
        }
    }

    /// <summary>
    /// ����Ʈ ��ô�� ������Ʈ�� ���� �޼���
    /// <para>���͸� ��ų� Ư�� �������� ����ٸ� �� �޼��带 Ȱ���Ͽ� ������Ʈ</para>
    /// </summary>
    /// <param name="monsterID"></param>
    /// <param name="item_ID"></param>
    public void UpdateQuestProgress(int? monsterID = null, int? item_ID = null)
    {
        
        if (monsterID == null)
        {

        }
        if (item_ID == null)
        {

        }
    }
}
