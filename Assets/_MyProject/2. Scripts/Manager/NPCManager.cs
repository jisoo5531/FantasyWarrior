using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }
    /// <summary>
    /// NPC���� ������ ����ִ� ����Ʈ
    /// </summary>
    public List<NPCData> NPC_List { get; set; }
    /// <summary>
    /// NPC���� ��ȭ ������ ��� �ִ� ����Ʈ
    /// </summary>
    public List<NPCDialogData> NPCDialog_List { get; set; }
    /// <summary>
    /// NPC���� � ����Ʈ�� ���� �ִ����� ���� ����Ʈ
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
    /// Ư�� npc�� ������ �ִ� ����Ʈ ID�� �������� �޼���
    /// </summary>
    /// <param name="NPC_ID"></param>
    /// <returns>NPC_ID�� ���� ID�� QuestID�� 0�� ����.</returns>
    public int GetQuestIDFromNPC(int NPC_ID)
    {
        int index = NPCQuest_List.FindIndex(x => x.NPC_ID.Equals(NPC_ID));
        if (index >= 0)
        {
            return NPCQuest_List[index].Quest_ID;
        }
        return 0;
    }    

    #region NPC ���� DB���� ��������

    /// <summary>
    /// NPC ����Ʈ ��������
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
            //  ����
            return null;
        }
    }    
    /// <summary>
    /// NPC ��ȭ ���� ����Ʈ ��������
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
            //  ����
            return null;
        }
    }
    /// <summary>
    /// NPC �� � ����Ʈ�� �� �������� ���� ������ ��������
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
            //  ����
            return null;
        }
    }

    #endregion
}
