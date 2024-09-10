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
    /// <summary>
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
    /// 현재 유저가 수행 중인 퀘스트들 데이터 가져오기
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
            //  실패
            return null;
        }
    }
    /// <summary>
    /// 퀘스트를 수락하면 userquests 테이블 (현재 유저가 수행 중인 퀘스트 정보)에 저장
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
    /// 목표 퀘스트의 ID 가져오기
    /// </summary>
    /// <param name="quest_ID">알고자 하는 퀘스트 ID</param>
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
        int? questObjID = GetObjectiveID(quest_ID);
        if (questObjID == null)
        {
            Debug.LogError("잘못된 퀘스트 아이디");
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
        int? questObjID = GetObjectiveID(quest_ID);
        if (questObjID == null)
        {
            Debug.LogError("잘못된 퀘스트 아이디");
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
            //  실패
            return null;
        }
    }

    /// <summary>
    /// 퀘스트 진척도 업데이트를 위한 메서드
    /// <para>몬스터를 잡거나 특정 아이템을 얻었다면 이 메서드를 활용하여 업데이트</para>
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
