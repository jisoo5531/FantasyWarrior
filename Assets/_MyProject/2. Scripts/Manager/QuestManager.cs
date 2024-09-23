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
    /// 모든 퀘스트를 저장할 딕셔너리
    /// <para>첫번째 요소의 key는 퀘스트의 id가 된다.</para>
    /// </summary>
    public Dictionary<int, QuestData> questDict { get; private set; }
    /// <summary>
    /// 퀘스트 목표의 정보들을 저장할 딕셔너리
    /// <para>Key는 오브젝트의 ID</para>
    /// </summary>
    public Dictionary<int, QuestObjectiveData> questObjectDict { get; private set; }
    public Dictionary<int, Questobj_CollectData> questObjCollect_Dict { get; private set; }
    public Dictionary<int, QuestObj_KillData> questObjKill_Dict { get; private set; }
    public Dictionary<int, QuestObj_TalkData> questObjTalk_Dict { get; private set; }
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
    public List<QuestProgress> questProgressList { get; private set; }


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
    /// <summary>
    /// 퀘스트 완료 후, 보상 분배
    /// </summary>
    public event Action<QuestData> OnGetQuestReward;

    #endregion

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

        // 5분마다 자동 저장
        InvokeRepeating("AutoSave", 300f, 300f);
    }
    /// <summary>
    /// 게임이 시작하면 저장되어 있던 데이터 가져오기
    /// </summary>
    public void Initialize()
    {
        GetQuestListFromDB();
        GetObjectivesDataFromDB();
        GetUserQuestObjectivesFromDB();
        GetUserQuestsFromDB();
        GetQuestObjKill();
        GetQuestObjTalk();
        GetQuestObjCollect();

        questProgressList = new List<QuestProgress>();
        List<UserQuestObjectivesData> userQOList = GetUserQuestObjectivesFromDB();
        if (userQOList != null)
        {
            Debug.Log("여긴 되냐?");
            foreach (var QO in userQOList)
            {
                QuestObjectiveData objective = GetObjectiveData(QO.ObjectiveID);
                questProgressList.Add(new QuestProgress(objective.Quest_ID));
            }
        }
        //EventHandler.managerEvent.TriggerQuestManagerInit();
    }

    /// <summary>
    /// 해당 ID 의 퀘스트 데이터 가져오기
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


    #region 퀘스트의 목표 정보 (obj)

    /// <summary>
    /// 해당 퀘스트 ID 또는 목표 ID의 목표 퀘스트 정보 가져오기
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
    /// 해당 ID의 처치 퀘스트 정보 가져오기
    /// </summary>
    /// <param name="quest_Id"></param>
    /// <returns></returns>
    public QuestObj_KillData GetKillQuestInfo(int quest_Id)
    {
        if (questObjKill_Dict.TryGetValue(quest_Id, out QuestObj_KillData killQuest))
        {
            return killQuest;
        }
        return null;
    }
    /// <summary>
    /// 해당 ID의 수집 퀘스트 정보 가져오기
    /// </summary>
    /// <param name="quest_Id"></param>
    /// <returns></returns>
    public Questobj_CollectData GetCollectQuestInfo(int quest_Id)
    {
        if (questObjCollect_Dict.TryGetValue(quest_Id, out Questobj_CollectData collectQuest))
        {
            return collectQuest;
        }
        return null;
    }
    /// <summary>
    /// 해당 ID의 대화 퀘스트 정보 가져오기
    /// </summary>
    /// <param name="quest_Id"></param>
    /// <returns></returns>
    public QuestObj_TalkData GetTalkQuestInfo(int quest_Id)
    {
        if (questObjTalk_Dict.TryGetValue(quest_Id, out QuestObj_TalkData talkQuest))
        {            
            return talkQuest;
        }
        return null;
    }
    /// <summary>
    /// 퀘스트 목표가 어떤 타입인지 가져오기 (몬스터 잡기, 아이템 수집, 말 걸기 등)
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
    /// <summary>
    /// 해당 퀘스트의 완료 조건 수량 가져오기
    /// </summary>
    /// <param name="quest_ID"></param>
    /// <returns></returns>
    public int? GetRequireComplete(int quest_ID)
    {
        Q_ObjectiveType questType = GetObjectiveData(quest_ID).ObjectiveType;
        if (questType == Q_ObjectiveType.Kill)
        {
            return GetKillQuestInfo(quest_ID).KillAmount;
        }
        else if (questType == Q_ObjectiveType.Collect)
        {
            return GetCollectQuestInfo(quest_ID).CollectAmount;
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region 유저의 퀘스트 진행상황

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

    #endregion


    #region 퀘스트 진행상황 업데이트 (수락, 업데이트, 완료)
    /// <summary>
    /// 퀘스트를 수락하면 userquests 테이블 (현재 유저가 수행 중인 퀘스트 상태)에 저장
    /// <para>userquestobjectives 테이블에 저장 (현재 유저가 수행 중인 퀘스트 진행도)</para>
    /// </summary>
    public void AcceptQuest(int quest_ID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        QuestObjectiveData objectiveData = GetObjectiveData(quest_ID);

        string query =
            $"INSERT INTO userquests (userquests.User_ID, userquests.Quest_ID, userquests.`Status`)\n" +
            $"VALUES ({user_ID}, {quest_ID}, '{Q_Status.InProgress.ToString()}');";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        query =
            $"INSERT INTO userquestobjectives (userquestobjectives.User_ID, userquestobjectives.Objective_ID)\n" +
            $"VALUES ({user_ID}, {objectiveData.ObjectiveID});";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        userQuestsList.Add(new UserQuestsData(user_ID, quest_ID, Q_Status.InProgress));
        userQuestObjList.Add(new UserQuestObjectivesData(user_ID, objectiveData.ObjectiveID, 0, false));

        questProgressList.Add(new QuestProgress(quest_ID, 0));

        if (objectiveData.ObjectiveType == Q_ObjectiveType.Talk)
        {
            // 만약 이 퀘스트가 단순히 말 전달 연계 퀘스트 종류라면

            int npcID = GetTalkQuestInfo(objectiveData.Quest_ID).NPC_ID;

            NPCManager.Instance.AddTalkQuest(new NPCTalkQuestData(npcID, quest_ID));
        }


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

            OnUpdateQuestProgress?.Invoke();
        }
        if (itemID != 0)
        {
            int index = questProgressList.FindIndex((x) => { return x.Item_Id == itemID; });
            if (index < 0)
            {
                // 퀘스트 대상이 아니다.
                Debug.Log("얘 아니다..");
                return;
            }
            Debug.Log("퀘스트 대상이다.");
            questProgressList[index].UpdateProgress();
            OnUpdateQuestProgress?.Invoke();
        }

    }
    #region 퀘스트 완료
    /// <summary>
    /// 퀘스트 완료 시에 실행할 메서드
    /// TODO : 완료 버튼을 누르면 완료 UI 뜨게
    /// </summary>
    /// <param name="quest_ID"></param>
    public void QuestComplete(int quest_ID)
    {
        QuestCompleteQuery(quest_ID);
        SetUserQuestData(quest_ID);
        GetReward(quest_ID);

        // 클라이언트에 저장한 임시 대화 퀘스트 목록에서 삭제
        NPCManager.Instance.RemoveTalkQuest(quest_ID);

        OnCompleteQuest?.Invoke();
    }
    /// <summary>
    /// DB에 퀘스트 완료문 쿼리문 날리기
    /// </summary>
    /// <param name="quest_ID"></param>
    private void QuestCompleteQuery(int quest_ID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        QuestObjectiveData objectiveData = GetObjectiveData(quest_ID);

        // 유저가 받은 퀘스트 완료 상태로 변경
        string query =
            $"UPDATE userquests\n" +
            $"SET userquests.`Status`='{Q_Status.Completed}'\n" +
            $"WHERE userquests.User_ID={user_ID} AND userquests.Quest_ID={quest_ID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        // 유저 퀘스트 목표 삭제
        query =
            $"DELETE FROM userquestobjectives\n" +
            $"WHERE userquestobjectives.User_ID={user_ID} " +
            $"AND userquestobjectives.Objective_ID={objectiveData.ObjectiveID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        NPCManager.Instance.NPCQuestComplete(quest_ID);
    }
    /// <summary>
    /// 유저의 퀘스트 진행 상황 업데이트
    /// </summary>
    /// <param name="quest_ID"></param>
    private void SetUserQuestData(int quest_ID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        QuestObjectiveData objectiveData = GetObjectiveData(quest_ID);
        // 퀘스트 완료 상태로 바꾸기
        int userQuestsindex = userQuestsList.FindIndex(x => x.User_ID.Equals(user_ID) && x.Quest_ID.Equals(quest_ID));
        userQuestsList[userQuestsindex].questStatus = Q_Status.Completed;

        // 유저가 현재 수행 중인 퀘스트들의 진행도를 나타내는 리스트에서 완료한 퀘스트 삭제
        int userQuestObjIndex = userQuestObjList.FindIndex(x => x.User_ID.Equals(user_ID) && x.ObjectiveID.Equals(objectiveData.ObjectiveID));
        userQuestObjList.RemoveAt(userQuestObjIndex);

        // 클라이언트에 저장된 퀘스트들의 진행도를 나타내는 리스트에서 완료한 퀘스트 삭제
        int index = questProgressList.FindIndex(x => x.quest_Id.Equals(quest_ID));
        questProgressList.RemoveAt(index);
    }
    /// <summary>
    /// 퀘스트 완료 후, 퀘스트 보상 수령
    /// </summary>
    /// <param name="quest_ID"></param>
    private void GetReward(int quest_ID)
    {
        QuestData quest = GetQuestData(quest_ID);

        if (quest.RewardItemID != 0)
        {
            ItemData reward_Item = ItemManager.Instance.GetItemData(quest.RewardItemID);
            InventoryManager.Instance.GetItem(reward_Item, quest.RewardItem_Amount);
        }
    }
    #endregion

    #endregion

    #region 게임 시작할 때 정보 가져오기 (DB)
    /// <summary>
    /// 퀘스트 리스트 가져오기
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
            //  실패
        }
    }
    /// <param name="quest_ID"></param>
    /// /// <summary>
    /// 퀘스트들의 목표 정보 가져오기
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
            //  실패            
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
    /// <summary>
    /// 퀘스트 목표 정보 가져오기 (몬스터 처치)
    /// </summary>
    private void GetQuestObjKill()
    {
        questObjKill_Dict = new Dictionary<int, QuestObj_KillData>();
        string query =
            $"SELECT *\n" +
            $"FROM quest_kill;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int id = int.Parse(row["Quest_ID"].ToString());
                questObjKill_Dict.Add(id, new QuestObj_KillData(row));
            }
        }
        else
        {
            //  실패
        }
    }
    /// <summary>
    /// 퀘스트 목표 정보 가져오기 (아이템 수집)
    /// </summary>
    private void GetQuestObjCollect()
    {
        questObjCollect_Dict = new Dictionary<int, Questobj_CollectData>();
        string query =
            $"SELECT *\n" +
            $"FROM quest_collect;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int id = int.Parse(row["Quest_ID"].ToString());
                questObjCollect_Dict.Add(id, new Questobj_CollectData(row));
            }
        }
        else
        {
            //  실패
        }
    }
    /// <summary>
    /// 퀘스트 목표 정보 가져오기 (대화)
    /// </summary>
    private void GetQuestObjTalk()
    {
        questObjTalk_Dict = new Dictionary<int, QuestObj_TalkData>();
        string query =
            $"SELECT *\n" +
            $"FROM quest_talk;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int id = int.Parse(row["Quest_ID"].ToString());
                questObjTalk_Dict.Add(id, new QuestObj_TalkData(row));
            }
        }
        else
        {
            //  실패
        }
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
        Debug.Log("게임 종료.");
        SaveQuestProgress();
    }
    #endregion
}
