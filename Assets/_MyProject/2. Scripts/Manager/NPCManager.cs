using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }    
    /// <summary>
    /// npc���� ������ ����ִ� ��ųʸ�
    /// <para>key�� npc�� ID</para>
    /// </summary>
    public Dictionary<int, NPCData> NPC_Dict { get; private set; }
    /// <summary>
    /// NPC���� ��ȭ ������ ��� �ִ� ����Ʈ
    /// </summary>
    public List<NPCDialogData> NPCDialog_List { get; private set; }
    /// <summary>
    /// NPC���� � ����Ʈ�� ���� �ִ����� ���� ����Ʈ
    /// </summary>
    public List<NPCQuestData> NPCQuest_List { get; private set; }    
    /// <summary>
    /// ���� ����Ʈ ���� �ӽ÷� �߰��ϰų� �׷� �͵��� ���� ����Ʈ
    /// </summary>
    public List<NPCTalkQuestData> talkNpcQuestList { get; private set; }

    private List<NPCTalkQuestData> orgTalkQuestList;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GetNPCDataFromDB();
        GetNPCDialogFromDB();
        GetNPCQuestFromDB();
        GetTalkQuestDataFromDB();

        EventHandler.managerEvent.TriggerNPCManagerInitInit();
    }

    /// <summary>
    /// npc �̸� ��������
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
    /// Ư�� npc�� ������ �ִ� ����Ʈ���� ID�� �������� �޼���
    /// <para>�̹� �Ϸ�� ����Ʈ�� �������� �ʴ´�.</para>
    /// </summary>
    /// <param name="NPC_ID"></param>
    /// <returns>NPC_ID�� ���� ID�� QuestID�� 0�� ����.</returns>
    public List<int> GetQuestIDFromNPC(int NPC_ID)
    {        
        List<int> questsID = new List<int>();        
        List<NPCQuestData> NPCQuestData = NPCQuest_List.FindAll(x => false == x.IsComplete && x.NPC_ID == NPC_ID);
        foreach (NPCQuestData nPCQuest in NPCQuestData)
        {
            questsID.Add(nPCQuest.Quest_ID);
        }
        return questsID;
    }        
    /// <summary>
    /// �ش� NPC�� ��� ��ȭ���� ��������
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
    /// ����Ʈ ���� �� �Ǵ� �� �� ��Ȳ���� ��ȭ ���� ����Ʈ ������ ����Ʈ�� �߰�
    /// </summary>
    /// <param name="talkQuest"></param>
    public void AddTalkQuest(NPCTalkQuestData talkQuest)
    {
        talkNpcQuestList.Add(talkQuest);
    }
    /// <summary>
    /// ����Ʈ �Ϸ� �� �Ǵ� �� �� ��Ȳ���� ���� �ʿ������ ����
    /// </summary>
    /// <param name="quest_ID"></param>
    public void RemoveTalkQuest(int quest_ID)
    {
        int talkQuestIndex = talkNpcQuestList.FindIndex(x => x.Quest_ID == quest_ID);
        if (talkQuestIndex >= 0)
        {
            talkNpcQuestList.RemoveAt(talkQuestIndex);
        }        
    }

    #region NPC ���� DB���� ��������

    /// <summary>
    /// NPC ����Ʈ ��������
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
            //  ����
        }
    }    
    /// <summary>
    /// NPC ��ȭ ���� ����Ʈ ��������
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
            //  ����
        }
    }
    /// <summary>
    /// NPC �� � ����Ʈ�� �� �������� ���� ������ ��������
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
            //  ����
        }
    }
    /// <summary>
    /// ��ȭ ���� ����Ʈ ������ ��������
    /// </summary>
    /// <returns></returns>
    private void GetTalkQuestDataFromDB()
    {
        orgTalkQuestList = new List<NPCTalkQuestData>();
        talkNpcQuestList = new List<NPCTalkQuestData>();
        string query =
            $"SELECT *" +
            $"FROM npc_talkquests;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                orgTalkQuestList.Add(new NPCTalkQuestData(row));
                talkNpcQuestList.Add(new NPCTalkQuestData(row));
            }
        }
        else
        {
            //  ����
        }
    }

    #endregion

    /// <summary>
    /// npc�� ����Ʈ�� Ŭ���� �� �� ȣ��
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
    /// DB�� ���� ���� �ʰ� Ŭ���̾�Ʈ�� ���Ƿ� �����س��� �����͵��� DB�� ���� (npcQuestList)
    /// <para>(���� ���� �� �Ǵ� ���� �ð�����)</para>
    /// </summary>
    public void SaveQuestProgress()
    {        
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

        // �ӽ� ��ȭ ���� ����Ʈ�� �����ϱ� ����
        // ���� ���Ϸ� �����ϴ� ����Ʈ�� �ް� ������ ���� ������ �ȵǱ� ������
        var differences = Extensions.GetDifferences(
            orgTalkQuestList,
            talkNpcQuestList,
            (original, updated) => original.NPC_ID == updated.NPC_ID && original.Quest_ID == updated.Quest_ID
        );
        foreach (var talkQuest in differences.Added)
        {
            string query =
                $"INSERT INTO npc_talkquests(npc_talkquests.NPC_ID, npc_talkquests.Quest_ID)\n" +
                $"VALUES ({talkQuest.NPC_ID}, {talkQuest.Quest_ID});";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
        foreach (var talkQuest in differences.Removed)
        {
            string query =
                $"DELETE FROM npc_talkquests\n" +
                $"WHERE npc_talkquests.NPC_ID={talkQuest.NPC_ID} AND " +
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
