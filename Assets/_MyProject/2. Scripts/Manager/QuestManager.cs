using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // TODO : 현재 클라이언트에 임시로 퀘스트 진행상황을 업데이트 하는 상황.
    // TODO : 만약 데이터베이스에 현재 상황을 저장하지 않고 종료 시, 데이터가 날라간다. 해결책 구현    
    public static QuestManager Instance { get; private set; }
    private List<QuestsData> questsDataList = new List<QuestsData>();
    private List<UserQuestsData> userQuestsList = new List<UserQuestsData>();
    /// <summary>
    /// 현재 유저가 수행 중인 퀘스트 진행 상황 (클라이언트)
    /// </summary>
    public List<QuestProgress> questProgressList = new List<QuestProgress>();

    #region 이벤트

    public event Action OnQuestManagerInit;
    /// <summary>
    /// 퀘스트를 수락하면 발생하는 이벤트
    /// </summary>
    public event Action OnAcceptQuest;
    /// <summary>
    /// 퀘스트가 업데이트될 때마다 발생하는 이벤트
    /// </summary>
    public event Action OnUpdateQuestProgress;
    /// <summary>
    /// 퀘스트가 완료되면 발생하는 이벤트
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

        // 5분마다 자동 저장
        InvokeRepeating("AutoSave", 300f, 300f);
    }
    /// <summary>
    /// 게임이 시작하면 저장되어 있던 데이터 가져오기
    /// </summary>
    private void Initialize()
    {
        questProgressList = new List<QuestProgress>();
        List<UserQuestObjectivesData> userQOList = GetUserQuestProgress();
        if (userQOList == null)
        {
            // 아직 DB에 유저가 받은 퀘스트가 없다.
            return;
        }
        foreach (var QO in userQOList)
        {
            QuestObjectivesData objective = GetObjectiveData(objectiveID: QO.ObjectiveID);
            questProgressList.Add(new QuestProgress(objective.Quest_ID, QO.CurrentAmount, objective.ReqAmount));
        }
    }

    #region 퀘스트 정보 가져오기
    /// <summary>
    /// 퀘스트 리스트 가져오기
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
            //  실패
            return null;
        }
    }
    /// <param name="quest_ID"></param>
    /// /// <summary>
    /// 특정 ID에 맞는 퀘스트 데이터 가져오기
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
            //  실패
            return null;
        }
    }
    /// <summary>
    /// 현재 유저가 수행 중인 (Inprogress) 퀘스트들 데이터 가져오기
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
            //  실패
            return null;
        }
    }
    /// <summary>
    /// 유저가 완료한 퀘스트 목록 가져오기
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
            //  실패
            return null;
        }
    }
    /// <summary>
    /// 현재 퀘스트의 상태(수락, 진행, 완료)를 가져오는 메서드
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
            //  실패
            return null;
        }
    }
    /// <summary>
    /// 목표 퀘스트 정보 가져오기 (퀘스트 ID)
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
            //  실패
            return null;
        }
    }
    /// <summary>
    /// 현재 퀘스트 진행도
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
            //  실패
            return null;
        }
    }
    /// <summary>
    /// 퀘스트의 완료 조건 가져오기
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
            //  실패
            return null;
        }
    }
    /// <summary>
    /// 유저의 퀘스트 진행상황도 가져오기
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
            //  실패
            return null;
        }
    }
    #endregion

    #region 퀘스트 진행상황 업데이트 (수락, 업데이트, 완료)
    /// <summary>
    /// 퀘스트를 수락하면 userquests 테이블 (현재 유저가 수행 중인 퀘스트 상태)에 저장
    /// <para>userquestobjectives 테이블에 저장 (현재 유저가 수행 중인 퀘스트 진행도)</para>
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
    /// 퀘스트 진척도 업데이트를 위한 메서드
    /// <para>몬스터를 잡거나 특정 아이템을 얻었다면 이 메서드를 활용하여 업데이트</para>
    /// <para>퀘스트 대상이 아니면 ID 가 0이다.</para>
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
                // 퀘스트 대상이 아니다.
                Debug.Log("얘 아니다..");
                return;
            }
            Debug.Log("퀘스트 대상이다.");
            questProgressList[index].UpdateProgress();

        }
        if (itemID != 0)
        {

        }
        OnUpdateQuestProgress?.Invoke();
    }
    /// <summary>
    /// 퀘스트 완료 시에 실행할 메서드
    /// TODO : 완료 버튼을 누르면 완료 UI 뜨게
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

    #region 퀘스트 저장
    /// <summary>
    /// DB에 아직 넣지 않고 클라이언트에 임의로 저장해놓은 데이터들을 DB로 저장 
    /// <para>(게임 종료 전 또는 일정 시간마다)</para>
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
        Debug.Log("게임 종료.");
        SaveQuestProgress();
    }
    #endregion
}
