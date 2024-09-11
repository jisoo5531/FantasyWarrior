using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // TODO : ���� Ŭ���̾�Ʈ�� �ӽ÷� ����Ʈ �����Ȳ�� ������Ʈ �ϴ� ��Ȳ.
    // TODO : ���� �����ͺ��̽��� ���� ��Ȳ�� �������� �ʰ� ���� ��, �����Ͱ� ���󰣴�. �ذ�å ����    
    public static QuestManager Instance { get; private set; }
    private List<QuestsData> questsDataList = new List<QuestsData>();
    private List<UserQuestsData> userQuestsList = new List<UserQuestsData>();
    /// <summary>
    /// ���� ������ ���� ���� ����Ʈ ���� ��Ȳ (Ŭ���̾�Ʈ)
    /// </summary>
    public List<QuestProgress> questProgressList = new List<QuestProgress>();

    #region �̺�Ʈ

    public event Action OnQuestManagerInit;
    /// <summary>
    /// ����Ʈ�� �����ϸ� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action OnAcceptQuest;
    /// <summary>
    /// ����Ʈ�� ������Ʈ�� ������ �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action OnUpdateQuestProgress;
    /// <summary>
    /// ����Ʈ�� �Ϸ�Ǹ� �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action OnCompleteQuest;

    #endregion

    private void Awake()
    {
        Instance = this;
        OnQuestManagerInit?.Invoke();

    }
    private void Start()
    {
        Initialize();

        // 5�и��� �ڵ� ����
        InvokeRepeating("AutoSave", 300f, 300f);
    }
    /// <summary>
    /// ������ �����ϸ� ����Ǿ� �ִ� ������ ��������
    /// </summary>
    private void Initialize()
    {
        questProgressList = new List<QuestProgress>();
        List<UserQuestObjectivesData> userQOList = GetUserQuestProgress();
        if (userQOList == null)
        {
            // ���� DB�� ������ ���� ����Ʈ�� ����.
            return;
        }
        foreach (var QO in userQOList)
        {
            QuestObjectivesData objective = GetObjectiveData(objectiveID: QO.ObjectiveID);
            questProgressList.Add(new QuestProgress(objective.Quest_ID, QO.CurrentAmount, objective.ReqAmount));
        }
    }

    #region ����Ʈ ���� ��������
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
    /// <param name="quest_ID"></param>
    /// /// <summary>
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
    /// ���� ������ ���� ���� (Inprogress) ����Ʈ�� ������ ��������
    /// </summary>
    /// <returns></returns>
    public List<UserQuestsData> GetInProgressQuest()
    {
        List<UserQuestsData> inProgressQuestList = new List<UserQuestsData>();
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"SELECT *\n" +
            $"FROM userquests\n" +
            $"WHERE userquests.User_ID={user_ID} AND userquests.status={(int)Q_Status.InProgress};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                inProgressQuestList.Add(new UserQuestsData(row));
            }
            return inProgressQuestList;
        }
        else
        {
            //  ����
            return null;
        }
    }
    /// <summary>
    /// ������ �Ϸ��� ����Ʈ ��� ��������
    /// </summary>
    /// <returns></returns>
    public List<UserQuestsData> GetCompletedQuest()
    {
        List<UserQuestsData> completedQuestList = new List<UserQuestsData>();
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"SELECT *\n" +
            $"FROM userquests\n" +
            $"WHERE userquests.User_ID={user_ID} AND userquests.status={(int)Q_Status.Completed};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                completedQuestList.Add(new UserQuestsData(row));
            }
            return completedQuestList;
        }
        else
        {
            //  ����
            return null;
        }
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
    /// ��ǥ ����Ʈ ���� �������� (����Ʈ ID)
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public QuestObjectivesData GetObjectiveData(int? quest_ID = null, int? objectiveID = null)
    {
        string query = string.Empty;
        if (quest_ID == null && objectiveID == null)
        {
            return null;
        }
        if (quest_ID > 0)
        {
            query =
            $"SELECT *\n" +
            $"FROM questobjectives\n" +
            $"WHERE questobjectives.Quest_ID={quest_ID};";
        }
        if (objectiveID > 0)
        {
            query =
            $"SELECT *\n" +
            $"FROM questobjectives\n" +
            $"WHERE questobjectives.Objective_ID={objectiveID};";
        }
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return new QuestObjectivesData(row);
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
        QuestObjectivesData questObjective = GetObjectiveData(quest_ID: quest_ID);
        int questObjID = questObjective.ObjectiveID;

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
        QuestObjectivesData questObjective = GetObjectiveData(quest_ID: quest_ID);
        int questObjID = questObjective.ObjectiveID;

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
    /// ������ ����Ʈ �����Ȳ�� ��������
    /// </summary>
    /// <returns></returns>
    public List<UserQuestObjectivesData> GetUserQuestProgress()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        List<UserQuestObjectivesData> userQO_List = new List<UserQuestObjectivesData>();
        string query =
            $"SELECT *\n" +
            $"FROM userquestobjectives\n" +
            $"WHERE userquestobjectives.User_ID={user_ID};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                userQO_List.Add(new UserQuestObjectivesData(row));
            }
            return userQO_List;
        }
        else
        {
            //  ����
            return null;
        }
    }
    #endregion

    #region ����Ʈ �����Ȳ ������Ʈ (����, ������Ʈ, �Ϸ�)
    /// <summary>
    /// ����Ʈ�� �����ϸ� userquests ���̺� (���� ������ ���� ���� ����Ʈ ����)�� ����
    /// <para>userquestobjectives ���̺� ���� (���� ������ ���� ���� ����Ʈ ���൵)</para>
    /// </summary>
    public void AcceptQuest(int quest_ID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        QuestObjectivesData objectiveData = GetObjectiveData(quest_ID: quest_ID);
        string query =
            $"INSERT INTO userquests (userquests.User_ID, userquests.Quest_ID, userquests.`Status`)\n" +
            $"VALUES ({user_ID}, {quest_ID}, 0);";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        query =
            $"INSERT INTO userquestobjectives (userquestobjectives.User_ID, userquestobjectives.Objective_ID)\n" +
            $"VALUES ({user_ID}, {objectiveData.ObjectiveID});";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        int? reqAmount = GetRequireCompleteQuest(quest_ID);

        questProgressList.Add(new QuestProgress(quest_ID, 0, reqAmount));
        OnAcceptQuest?.Invoke();
    }
    /// <summary>
    /// ����Ʈ ��ô�� ������Ʈ�� ���� �޼���
    /// <para>���͸� ��ų� Ư�� �������� ����ٸ� �� �޼��带 Ȱ���Ͽ� ������Ʈ</para>
    /// <para>����Ʈ ����� �ƴϸ� ID �� 0�̴�.</para>
    /// </summary>
    /// <param name="monsterID"></param>
    /// <param name="item_ID"></param>
    public void UpdateQuestProgress(int unitID = 0, int itemID = 0)
    {
        if (unitID != 0)
        {
            int index = questProgressList.FindIndex((x) => { return x.monster_Id == unitID; });
            if (index < 0)
            {
                // ����Ʈ ����� �ƴϴ�.
                Debug.Log("�� �ƴϴ�..");
                return;
            }
            Debug.Log("����Ʈ ����̴�.");
            questProgressList[index].UpdateProgress();

        }
        if (itemID != 0)
        {

        }
        OnUpdateQuestProgress?.Invoke();
    }
    /// <summary>
    /// ����Ʈ �Ϸ� �ÿ� ������ �޼���
    /// TODO : �Ϸ� ��ư�� ������ �Ϸ� UI �߰�
    /// </summary>
    /// <param name="quest_ID"></param>
    public void QuestComplete(int quest_ID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"UPDATE userquests\n" +
            $"SET userquests.`Status`={(int)Q_Status.Completed}\n" +
            $"WHERE userquests.User_ID={user_ID} AND userquests.Quest_ID={quest_ID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        QuestObjectivesData objectiveData = GetObjectiveData(quest_ID: quest_ID);
        query =
                $"DELETE FROM userquestobjectives\n" +
                $"WHERE userquestobjectives.User_ID={user_ID} " +
                $"AND userquestobjectives.Objective_ID={objectiveData.ObjectiveID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        int index = questProgressList.FindIndex((x) => { return x.quest_Id.Equals(quest_ID); });
        questProgressList.RemoveAt(index);

        OnCompleteQuest?.Invoke();
    }
    #endregion

    #region ����Ʈ ����
    /// <summary>
    /// DB�� ���� ���� �ʰ� Ŭ���̾�Ʈ�� ���Ƿ� �����س��� �����͵��� DB�� ���� 
    /// <para>(���� ���� �� �Ǵ� ���� �ð�����)</para>
    /// </summary>
    public void SaveQuestProgress()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        foreach (QuestProgress questProgress in questProgressList)
        {
            QuestObjectivesData objectiveData = GetObjectiveData(quest_ID: questProgress.quest_Id);
            string query =
                $"UPDATE userquestobjectives\n" +
                $"SET userquestobjectives.Current_Amount={questProgress.current_Amount}\n" +
                $"WHERE userquestobjectives.User_ID={user_ID} " +
                $"AND userquestobjectives.Objective_ID={objectiveData.ObjectiveID};";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
    }
    private void AutoSave()
    {
        SaveQuestProgress();
    }
    private void OnApplicationQuit()
    {
        Debug.Log("���� ����.");
        SaveQuestProgress();
    }
    #endregion
}
