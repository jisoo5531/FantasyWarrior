using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    /// <summary>
    /// ��� ����Ʈ�� ������ ��ųʸ�
    /// <para>ù��° ����� key�� ����Ʈ�� id�� �ȴ�.</para>
    /// </summary>
    public Dictionary<int, QuestData> questDict { get; private set; }
    /// <summary>
    /// ����Ʈ ��ǥ�� �������� ������ ��ųʸ�
    /// <para>Key�� ������Ʈ�� ID</para>
    /// </summary>
    public Dictionary<int, QuestObjectiveData> questObjectDict { get; private set; }
    /// <summary>
    /// ������ �������� ����Ʈ���� ���� ���µ��� ������ ����Ʈ (���� �� ������Ʈ�ϸ鼭 �����ȴ�.)
    /// <para>���� ���� ��, �������� �� ��</para>
    /// </summary>
    public List<UserQuestsData> userQuestsList { get; private set; }
    /// <summary>
    /// ������ �������� ����Ʈ���� ���൵�� ������ ����Ʈ (���� �� ������Ʈ�ϸ鼭 �����ȴ�.)
    /// <para>���� ���� ��, �������� �� ��</para>
    /// </summary>
    public List<UserQuestObjectivesData> userQuestObjList { get; private set; }

    /// <summary>
    /// ���� ������ ���� ���� ����Ʈ ���� ��Ȳ (Ŭ���̾�Ʈ)
    /// </summary>
    public List<QuestProgress> questProgressList { get; private set; }


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
        GetQuestListFromDB();
        GetObjectivesDataFromDB();
        GetUserQuestObjectivesFromDB();
        GetUserQuestsFromDB();

        questProgressList = new List<QuestProgress>();
        List<UserQuestObjectivesData> userQOList = GetUserQuestObjectivesFromDB();
        if (userQOList != null)
        {
            foreach (var QO in userQOList)
            {
                QuestObjectiveData objective = GetObjectiveData(QO.ObjectiveID);
                questProgressList.Add(new QuestProgress(objective.Quest_ID, QO.CurrentAmount, objective.ReqAmount));
            }
        }
        EventHandler.managerEvent.TriggerQuestManagerInit();
    }

    #region ����Ʈ ���� ��������

    /// <summary>
    /// �ش� ID �� ����Ʈ ������ ��������
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public QuestData GetQuestData(int quest_ID)
    {
        if (questDict.TryGetValue(quest_ID, out QuestData quest))
        {
            return quest;
        }
        return null;
    }
    /// <summary>
    /// ���� ������ ���� ���� (Inprogress) ����Ʈ�� ������ ��������
    /// </summary>
    /// <returns></returns>
    public List<UserQuestsData> GetInProgressQuest()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        if (userQuestsList == null)
        {
            return null;
        }
        return userQuestsList.FindAll((x) => x.User_ID.Equals(user_ID) && x.questStatus.Equals(Q_Status.InProgress));
    }
    /// <summary>
    /// ������ �Ϸ��� ����Ʈ ��� ��������
    /// </summary>
    /// <returns></returns>
    public List<UserQuestsData> GetCompletedQuest()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        if (userQuestsList == null)
        {
            return null;
        }
        return userQuestsList.FindAll((x) => x.User_ID.Equals(user_ID) && x.questStatus.Equals(Q_Status.Completed));
    }
    /// <summary>
    /// ���� ����Ʈ�� ����(����, ����, �Ϸ�)�� �������� �޼���
    /// </summary>
    /// <returns></returns>
    public Q_Status? GetQuestStatus(int quest_ID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        if (userQuestsList == null)
        {
            return null;
        }
        int index = userQuestsList.FindIndex((x) => x.User_ID.Equals(user_ID) && x.Quest_ID.Equals(quest_ID));
        if (index >= 0)
        {
            return userQuestsList[index].questStatus;
        }
        return null;
    }
    /// <summary>
    /// �ش� ����Ʈ ID �Ǵ� ��ǥ ID�� ��ǥ ����Ʈ ���� ��������
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public QuestObjectiveData GetObjectiveData(int objectiveID)
    {
        if (questObjectDict.TryGetValue(objectiveID, out QuestObjectiveData objectiveData))
        {
            return objectiveData;
        }
        return null;
    }
    /// <summary>
    /// ���� ����Ʈ ���൵
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public int? GetCurrentQuestProgress(int quest_ID)
    {
        int index = questProgressList.FindIndex((x) => x.quest_Id.Equals(quest_ID));
        if (index >= 0)
        {
            return questProgressList[index].current_Amount;
        }
        return null;
    }
    /// <summary>
    /// ����Ʈ�� �Ϸ� ���� ��������
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public int? GetRequireCompleteQuest(int quest_ID)
    {        
        if (questObjectDict.TryGetValue(quest_ID, out QuestObjectiveData objectiveData))
        {
            return objectiveData.ReqAmount;
        }
        return null;
    }
    /// <summary>
    /// ����Ʈ ��ǥ�� � Ÿ������ �������� (���� ���, ������ ����, �� �ɱ� ��)
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public Q_ObjectiveType? GetObjectiveType(int quest_ID)
    {
        if (questObjectDict.TryGetValue(quest_ID, out QuestObjectiveData objectiveData))
        {
            return objectiveData.ObjectiveType;
        }
        return null;
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
        QuestObjectiveData objectiveData = GetObjectiveData(quest_ID);

        string query =
            $"INSERT INTO userquests (userquests.User_ID, userquests.Quest_ID, userquests.`Status`)\n" +
            $"VALUES ({user_ID}, {quest_ID}, 0);";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        query =
            $"INSERT INTO userquestobjectives (userquestobjectives.User_ID, userquestobjectives.Objective_ID)\n" +
            $"VALUES ({user_ID}, {objectiveData.ObjectiveID});";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        userQuestsList.Add(new UserQuestsData(user_ID, quest_ID, Q_Status.InProgress));
        userQuestObjList.Add(new UserQuestObjectivesData(user_ID, objectiveData.ObjectiveID, 0, false));

        int? reqAmount = GetRequireCompleteQuest(quest_ID);
        questProgressList.Add(new QuestProgress(quest_ID, 0, reqAmount));

        if (objectiveData.ObjectiveType == Q_ObjectiveType.Talk)
        {
            // ���� �� ����Ʈ�� �ܼ��� �� ���� ���� ����Ʈ �������
            int npcID = objectiveData.NPC_ID;

            NPCManager.Instance.AddTalkQuest(new NPCTalkQuestData(npcID, quest_ID));
        }


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

            OnUpdateQuestProgress?.Invoke();
        }
        if (itemID != 0)
        {

            OnUpdateQuestProgress?.Invoke();
        }

    }
    /// <summary>
    /// ����Ʈ �Ϸ� �ÿ� ������ �޼���
    /// TODO : �Ϸ� ��ư�� ������ �Ϸ� UI �߰�
    /// </summary>
    /// <param name="quest_ID"></param>
    public void QuestComplete(int quest_ID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        QuestObjectiveData objectiveData = GetObjectiveData(quest_ID);

        // ������ ���� ����Ʈ �Ϸ� ���·� ����
        string query =
            $"UPDATE userquests\n" +
            $"SET userquests.`Status`={(int)Q_Status.Completed}\n" +
            $"WHERE userquests.User_ID={user_ID} AND userquests.Quest_ID={quest_ID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        // ���� ����Ʈ ��ǥ ����
        query =
            $"DELETE FROM userquestobjectives\n" +
            $"WHERE userquestobjectives.User_ID={user_ID} " +
            $"AND userquestobjectives.Objective_ID={objectiveData.ObjectiveID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        NPCManager.Instance.NPCQuestComplete(quest_ID);

        // ����Ʈ �Ϸ� ���·� �ٲٱ�
        int userQuestsindex = userQuestsList.FindIndex(x => x.User_ID.Equals(user_ID) && x.Quest_ID.Equals(quest_ID));
        userQuestsList[userQuestsindex].questStatus = Q_Status.Completed;

        // ������ ���� ���� ���� ����Ʈ���� ���൵�� ��Ÿ���� ����Ʈ���� �Ϸ��� ����Ʈ ����
        int userQuestObjIndex = userQuestObjList.FindIndex(x => x.User_ID.Equals(user_ID) && x.ObjectiveID.Equals(objectiveData.ObjectiveID));
        userQuestObjList.RemoveAt(userQuestObjIndex);

        // Ŭ���̾�Ʈ�� ����� ����Ʈ���� ���൵�� ��Ÿ���� ����Ʈ���� �Ϸ��� ����Ʈ ����
        int index = questProgressList.FindIndex(x => x.quest_Id.Equals(quest_ID));
        questProgressList.RemoveAt(index);

        // Ŭ���̾�Ʈ�� ������ �ӽ� ��ȭ ����Ʈ ��Ͽ��� ����
        NPCManager.Instance.RemoveTalkQuest(quest_ID);


        OnCompleteQuest?.Invoke();
    }
    #endregion

    #region ���� ������ �� ���� ��������
    /// <summary>
    /// ����Ʈ ����Ʈ ��������
    /// </summary>
    /// <returns></returns>
    private void GetQuestListFromDB()
    {
        questDict = new Dictionary<int, QuestData>();
        string query =
            $"SELECT *\n" +
            $"FROM quests;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int id = int.Parse(row["Quest_ID"].ToString());
                questDict.Add(id, new QuestData(row));
            }
        }
        else
        {
            //  ����
        }
    }
    /// <param name="quest_ID"></param>
    /// /// <summary>
    /// ����Ʈ���� ��ǥ ���� ��������
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    private void GetObjectivesDataFromDB()
    {
        questObjectDict = new Dictionary<int, QuestObjectiveData>();

        string query =
            $"SELECT *\n" +
            $"FROM questobjectives;";

        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int id = int.Parse(row["Objective_ID"].ToString());
                questObjectDict.Add(id, new QuestObjectiveData(row));
            }
        }
        else
        {
            //  ����            
        }
    }
    /// <summary>
    /// ������ ���� ���� ����Ʈ���� ���൵ ��������
    /// </summary>
    /// <returns></returns>
    private List<UserQuestObjectivesData> GetUserQuestObjectivesFromDB()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        userQuestObjList = new List<UserQuestObjectivesData>();

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
                userQuestObjList.Add(new UserQuestObjectivesData(row));
            }
            return userQuestObjList;
        }
        else
        {
            //  ����
            return null;
        }
    }
    /// <summary>
    /// ������ ���� ���� ����Ʈ���� ���� ���� ��������
    /// </summary>
    /// <returns></returns>
    private List<UserQuestsData> GetUserQuestsFromDB()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        userQuestsList = new List<UserQuestsData>();

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
    #endregion

    #region ����Ʈ ����
    /// <summary>
    /// DB�� ���� ���� �ʰ� Ŭ���̾�Ʈ�� ���Ƿ� �����س��� �����͵��� DB�� ���� (userquestList, userquestOBJList)
    /// <para>(���� ���� �� �Ǵ� ���� �ð�����)</para>
    /// </summary>
    public void SaveQuestProgress()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        foreach (QuestProgress questProgress in questProgressList)
        {
            QuestObjectiveData objectiveData = GetObjectiveData(questProgress.quest_Id);
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
