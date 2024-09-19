using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }
    /// <summary>
    /// NPC들의 정보를 담고있는 리스트
    /// </summary>
    public List<NPCData> NPC_List { get; set; }
    /// <summary>
    /// NPC들의 대화 내용을 담고 있는 리스트
    /// </summary>
    public List<NPCDialogData> NPCDialog_List { get; set; }
    /// <summary>
    /// NPC들이 어떤 퀘스트를 갖고 있는지에 대한 리스트
    /// </summary>
    public List<NPCQuestData> NPCQuest_List { get; set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _ = GetNPCDataFromDB();
        _ = GetNPCDialogFromDB();
        _ = GetNPCQuestFromDB();
    }

    /// <summary>
    /// 특정 npc가 가지고 있는 퀘스트 ID를 가져오는 메서드
    /// </summary>
    /// <param name="NPC_ID"></param>
    /// <returns>NPC_ID가 없는 ID면 QuestID는 0이 리턴.</returns>
    public int GetQuestIDFromNPC(int NPC_ID)
    {
        int index = NPCQuest_List.FindIndex(x => x.NPC_ID.Equals(NPC_ID));
        if (index >= 0)
        {
            return NPCQuest_List[index].Quest_ID;
        }
        return 0;
    }    

    #region NPC 정보 DB에서 가져오기

    /// <summary>
    /// NPC 리스트 가져오기
    /// </summary>
    /// <returns></returns>
    private List<NPCData> GetNPCDataFromDB()
    {
        NPC_List = new List<NPCData>();
        string query =
            $"SELECT *\n" +
            $"FROM npcs;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                NPC_List.Add(new NPCData(row));
            }
            return NPC_List;
        }
        else
        {
            //  실패
            return null;
        }
    }    
    /// <summary>
    /// NPC 대화 내용 리스트 가져오기
    /// </summary>
    /// <returns></returns>
    private List<NPCDialogData> GetNPCDialogFromDB()
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
            return NPCDialog_List;
        }
        else
        {
            //  실패
            return null;
        }
    }
    /// <summary>
    /// NPC 가 어떤 퀘스트를 줄 것인지에 대한 데이터 가져오기
    /// </summary>
    /// <returns></returns>
    private List<NPCQuestData> GetNPCQuestFromDB()
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
            return NPCQuest_List;
        }
        else
        {
            //  실패
            return null;
        }
    }

    #endregion
}
