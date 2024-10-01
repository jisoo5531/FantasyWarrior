using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }

    public List<GameObject> NPCList;

    /// <summary>
    /// npc들의 정보를 담고있는 딕셔너리
    /// <para>key는 npc의 ID</para>
    /// </summary>
    public Dictionary<int, NPCData> NPC_Dict { get; private set; }
    /// <summary>
    /// NPC들의 대화 내용을 담고 있는 리스트
    /// </summary>
    public List<NPCDialogData> NPCDialog_List { get; private set; }
    /// <summary>
    /// NPC들이 어떤 퀘스트를 갖고 있는지에 대한 리스트
    /// </summary>
    public List<NPCQuestData> NPCQuest_List { get; private set; }    
    /// <summary>
    /// 연계 퀘스트 등의 임시로 추가하거나 그런 것들을 위한 리스트
    /// </summary>
    public List<User_NPCTalkQuestData> talkNpcQuestList { get; private set; }

    private List<User_NPCTalkQuestData> orgTalkQuestList;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        
        //EventHandler.managerEvent.TriggerNPCManagerInit();
    }
    public void Initialize()
    {
        GetNPCDataFromDB();
        GetNPCDialogFromDB();
        GetNPCQuestFromDB();
        GetTalkQuestDataFromDB();
    }

    /// <summary>
    /// npc 이름 가져오기
    /// </summary>
    /// <param name="NPC_ID"></param>
    /// <returns></returns>
    public string GetNPCName(int NPC_ID)
    {
        if (NPC_Dict.TryGetValue(NPC_ID, out NPCData nPCData))
        {
            return nPCData.Name;
        }
        return string.Empty;
    }
    /// <summary>
    /// 특정 npc가 가지고 있는 퀘스트들의 ID를 가져오는 메서드
    /// <para>현재 레벨에서 수행 가능한 퀘스트만</para>
    /// <para>이미 완료된 퀘스트는 가져오지 않는다.</para>
    /// </summary>
    /// <param name="NPC_ID"></param>
    /// <returns>NPC_ID가 없는 ID면 QuestID는 0이 리턴.</returns>
    public List<int> GetQuestIDFromNPC(int NPC_ID)
    {
        int currentLevel = UserStatManager.Instance.userStatClient.Level;
        List<int> questsID = new List<int>();        
        
        List<NPCQuestData> NPCQuestData = NPCQuest_List.FindAll(x => false == x.IsComplete && x.NPC_ID == NPC_ID);
        foreach (NPCQuestData nPCQuest in NPCQuestData)
        {
            QuestData questData = QuestManager.Instance.GetQuestData(nPCQuest.Quest_ID);
            if (currentLevel >= questData.ReqLv)
            {
                questsID.Add(nPCQuest.Quest_ID);
            }            
        }
        return questsID;
    }        
    /// <summary>
    /// 해당 NPC의 모든 대화내용 가져오기
    /// </summary>
    /// <param name="npc_ID"></param>
    /// <returns></returns>
    public List<NPCDialogData> GetDialogList(int npc_ID)
    {        
        List<NPCDialogData> NPCDialogList = NPCDialog_List.FindAll(x => x.NPC_ID.Equals(npc_ID));
        NPCDialogList.Sort(compareDialogOrder);
        return NPCDialogList;
    }
    private int compareDialogOrder(NPCDialogData a, NPCDialogData b)
    {
        return a.Order < b.Order ? -1 : 1;
    }    
    /// <summary>
    /// 퀘스트 수락 시 또는 그 외 상황에서 대화 연계 퀘스트 데이터 리스트에 추가
    /// </summary>
    /// <param name="talkQuest"></param>
    public void AddTalkQuest(User_NPCTalkQuestData talkQuest)
    {
        talkNpcQuestList.Add(talkQuest);
    }
    /// <summary>
    /// 퀘스트 완료 시 또는 그 외 상황에서 이제 필요없으면 제거
    /// </summary>
    /// <param name="quest_ID"></param>
    public void RemoveTalkQuest(int quest_ID)
    {
        int userID = DatabaseManager.Instance.userData.UID;
        int talkQuestIndex = talkNpcQuestList.FindIndex(x => x.User_ID == userID && x.Quest_ID == quest_ID);
        if (talkQuestIndex >= 0)
        {
            talkNpcQuestList.RemoveAt(talkQuestIndex);
        }        
    }

    #region NPC 정보 DB에서 가져오기

    /// <summary>
    /// NPC 리스트 가져오기
    /// </summary>
    /// <returns></returns>
    private void GetNPCDataFromDB()
    {        
        NPC_Dict = new Dictionary<int, NPCData>();
        string query =
            $"SELECT *\n" +
            $"FROM npcs;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {                
                int id = int.Parse(row["NPC_ID"].ToString());
                NPC_Dict.Add(id, new NPCData(row));
            }
        }
        else
        {
            //  실패
        }
    }    
    /// <summary>
    /// NPC 대화 내용 리스트 가져오기
    /// </summary>
    /// <returns></returns>
    private void GetNPCDialogFromDB()
    {
        NPCDialog_List = new List<NPCDialogData>();

        string query =
            $"SELECT *\n" +
            $"FROM npcdialogue;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                NPCDialog_List.Add(new NPCDialogData(row));
            }
        }
        else
        {
            //  실패
        }
    }
    /// <summary>
    /// NPC 가 어떤 퀘스트를 줄 것인지에 대한 데이터 가져오기
    /// </summary>
    /// <returns></returns>
    private void GetNPCQuestFromDB()
    {
        NPCQuest_List = new List<NPCQuestData>();

        string query =
            $"SELECT *\n" +
            $"FROM npc_quests;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                NPCQuest_List.Add(new NPCQuestData(row));
            }
        }
        else
        {
            //  실패
        }
    }
    /// <summary>
    /// 대화 연계 퀘스트 데이터 가져오기
    /// </summary>
    /// <returns></returns>
    private void GetTalkQuestDataFromDB()
    {
        int userID = DatabaseManager.Instance.userData.UID;

        orgTalkQuestList = new List<User_NPCTalkQuestData>();
        talkNpcQuestList = new List<User_NPCTalkQuestData>();
        string query =
            $"SELECT *" +
            $"FROM user_npc_talkquests\n" +
            $"WHERE user_npc_talkquests.User_ID={userID};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                orgTalkQuestList.Add(new User_NPCTalkQuestData(row));
                talkNpcQuestList.Add(new User_NPCTalkQuestData(row));
            }
        }
        else
        {
            //  실패
        }
    }

    #endregion

    /// <summary>
    /// npc의 퀘스트를 클리어 할 때 호출
    /// </summary>
    /// <param name="quest_ID"></param>
    public void NPCQuestComplete(int quest_ID)
    {
        int index = NPCQuest_List.FindIndex(x => x.Quest_ID == quest_ID);
        if (index >= 0)
        {
            NPCQuest_List[index].IsComplete = true;
        }

    }

    /// <summary>
    /// DB에 아직 넣지 않고 클라이언트에 임의로 저장해놓은 데이터들을 DB로 저장 (npcQuestList)
    /// <para>(게임 종료 전 또는 일정 시간마다)</para>
    /// </summary>
    private void SaveQuestProgress()
    {
        int userID = DatabaseManager.Instance.userData.UID;
        foreach (NPCQuestData npcQuest in NPCQuest_List)
        {
            string checkComplete = "false";
            if (npcQuest.IsComplete)
            {
                checkComplete = "true";
            }            
            string query =
                $"UPDATE npc_quests\n" +
                $"SET npc_quests.IsComplete='{checkComplete}'\n" +
                $"WHERE npc_quests.Quest_ID={npcQuest.Quest_ID};";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }

        // 임시 대화 연계 퀘스트를 저장하기 위한
        // 말을 전하러 가야하는 퀘스트를 받고 저장을 하지 않으면 안되기 때문에
        var differences = Extensions.GetDifferences(
            orgTalkQuestList,
            talkNpcQuestList,
            (original, updated) => original.NPC_ID == updated.NPC_ID && original.Quest_ID == updated.Quest_ID
        );
        foreach (var talkQuest in differences.Added)
        {
            string query =
                $"INSERT INTO user_npc_talkquests(user_npc_talkquests.NPC_ID, user_npc_talkquests.Quest_ID, user_npc_talkquests.User_ID)\n" +
                $"VALUES ({talkQuest.NPC_ID}, {talkQuest.Quest_ID}, {userID});";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
        foreach (var talkQuest in differences.Removed)
        {
            string query =
                $"DELETE FROM user_npc_talkquests\n" +
                $"WHERE user_npc_talkquests.NPC_ID={talkQuest.NPC_ID} AND " +
                $"npc_talkquests.Quest_ID={talkQuest.Quest_ID};";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
    }
    private void AutoSave()
    {
        SaveQuestProgress();
    }
    private void OnApplicationQuit()
    {        
        SaveQuestProgress();
    }
}
