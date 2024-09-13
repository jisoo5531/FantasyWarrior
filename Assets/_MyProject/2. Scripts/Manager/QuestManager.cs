using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // TODO : DB 와 관련된 것들. 클라이언트에 임시로 저장시키기 -> 매번 DB에 접속하면 부하
    public static QuestManager Instance { get; private set; }
    /// <summary>
    /// 모든 퀘스트들을 저장할 리스트
    /// </summary>
    public List<QuestsData> questsDataList { get; private set; }
    /// <summary>
    /// 퀘스트 목표의 정보들을 저장할 리스트
    /// </summary>
    public List<QuestObjectivesData> questObjectList { get; private set; }
    /// <summary>
    /// 유저가 수행중인 퀘스트들의 진행 상태들을 저장할 리스트 (게임 중 업데이트하면서 수정된다.)
    /// <para>게임 종료 시, 저장해햐 할 것</para>
    /// </summary>
    public List<UserQuestsData> userQuestsList { get; private set; }
    /// <summary>
    /// 유저가 수행중인 퀘스트들의 진행도를 저장할 리스트 (게임 중 업데이트하면서 수정된다.)
    /// <para>게임 종료 시, 저장해햐 할 것</para>
    /// </summary>
    public List<UserQuestObjectivesData> userQuestObjList { get; private set; }

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
                QuestObjectivesData objective = GetObjectiveData(objectiveID: QO.ObjectiveID);
                questProgressList.Add(new QuestProgress(objective.Quest_ID, QO.CurrentAmount, objective.ReqAmount));
            }
        }
        EventHandler.managerEvent.TriggerQuestManagerInit();
    }
    #region 게임 시작할 때 정보 가져오기
    /// <summary>
    /// 퀘스트 리스트 가져오기
    /// </summary>
    /// <returns></returns>
    private List<QuestsData> GetQuestListFromDB()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        questsDataList = new List<QuestsData>();
        string query =
            $"SELECT *\n" +
            $"FROM quests\n" +
            $"WHERE quests.Quest_ID={user_ID};";
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
    /// 퀘스트들의 목표 정보 가져오기
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    private List<QuestObjectivesData> GetObjectivesDataFromDB()
    {
        questObjectList = new List<QuestObjectivesData>();

        string query =
            $"SELECT *\n" +
            $"FROM questobjectives;";

        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                questObjectList.Add(new QuestObjectivesData(row));
            }
            return questObjectList;
        }
        else
        {
            //  실패
            return null;
        }
    }
    /// <summary>
    /// 유저가 수행 중인 퀘스트들의 진행도 가져오기
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
            //  실패
            return null;
        }
    }
    /// <summary>
    /// 유저가 수행 중인 퀘스트들의 진행 상태 가져오기
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
            //  실패
            return null;
        }
    }
    #endregion

    #region 퀘스트 정보 가져오기

    /// <summary>
    /// 해당 ID 의 퀘스트 데이터 가져오기
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public QuestsData GetQuestData(int quest_ID)
    {
        if (questsDataList == null)
        {
            return null;
        }

        int index = questsDataList.FindIndex((x) => x.Quest_ID.Equals(quest_ID));
        if (index >= 0)
        {
            return questsDataList[index];
        }
        return null;
    }
    /// <summary>
    /// 현재 유저가 수행 중인 (Inprogress) 퀘스트들 데이터 가져오기
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
    /// 유저가 완료한 퀘스트 목록 가져오기
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
    /// 현재 퀘스트의 상태(수락, 진행, 완료)를 가져오는 메서드
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
    /// 해당 퀘스트 ID 또는 목표 ID의 목표 퀘스트 정보 가져오기
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public QuestObjectivesData GetObjectiveData(int? quest_ID = null, int? objectiveID = null)
    {  
        if (quest_ID > 0)
        {
            int index = questObjectList.FindIndex((x) => x.Quest_ID.Equals(quest_ID));
            if (index >= 0)
            {
                return questObjectList[index];
            }
        }
        if (objectiveID > 0)
        {
            int index = questObjectList.FindIndex((x) => x.Quest_ID.Equals(objectiveID));
            if (index >= 0)
            {
                return questObjectList[index];
            }            
        }
        return null;
    }
    /// <summary>
    /// 현재 퀘스트 진행도
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
    /// 퀘스트의 완료 조건 가져오기
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public int? GetRequireCompleteQuest(int quest_ID)
    {
        QuestObjectivesData questObj = GetObjectiveData(quest_ID: quest_ID);
        int index = questObjectList.FindIndex((x) => x.ObjectiveID.Equals(questObj.ObjectiveID));
        if (index >= 0)
        {            
            return questObjectList[index].ReqAmount;
        }
        return null;
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

        userQuestsList.Add(new UserQuestsData(user_ID, quest_ID, Q_Status.InProgress, false));
        userQuestObjList.Add(new UserQuestObjectivesData(user_ID, objectiveData.ObjectiveID, 0, false));

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
        QuestObjectivesData objectiveData = GetObjectiveData(quest_ID: quest_ID);

        string query =
            $"UPDATE userquests\n" +
            $"SET userquests.`Status`={(int)Q_Status.Completed}\n" +
            $"WHERE userquests.User_ID={user_ID} AND userquests.Quest_ID={quest_ID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        query =
            $"DELETE FROM userquestobjectives\n" +
            $"WHERE userquestobjectives.User_ID={user_ID} " +
            $"AND userquestobjectives.Objective_ID={objectiveData.ObjectiveID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        // 퀘스트 완료 상태로 바꾸기
        int userQuestsindex = userQuestsList.FindIndex(x => x.User_ID.Equals(user_ID) && x.Quest_ID.Equals(quest_ID));
        userQuestsList[userQuestsindex].questStatus = Q_Status.Completed;

        // 유저가 현재 수행 중인 퀘스트들의 진행도를 나타내는 리스트에서 완료한 퀘스트 삭제
        int userQuestObjIndex = userQuestObjList.FindIndex(x => x.User_ID.Equals(user_ID) && x.ObjectiveID.Equals(objectiveData.ObjectiveID));
        questObjectList.RemoveAt(userQuestObjIndex);

        // 클라이언트에 저장된 퀘스트들의 진행도를 나타내는 리스트에서 완료한 퀘스트 삭제
        int index = questProgressList.FindIndex(x => x.quest_Id.Equals(quest_ID));
        questProgressList.RemoveAt(index);

        OnCompleteQuest?.Invoke();
    }
    #endregion

    #region 퀘스트 저장
    /// <summary>
    /// DB에 아직 넣지 않고 클라이언트에 임의로 저장해놓은 데이터들을 DB로 저장 (userquestList, userquestOBJList)
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
