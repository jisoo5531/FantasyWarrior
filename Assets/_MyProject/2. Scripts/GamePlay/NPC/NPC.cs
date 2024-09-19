using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("NPC ID")]
    public int NPC_ID;
    [Header("����Ʈ ����")]
    public GameObject startQuestIcon;
    public GameObject completeQuestIcon;
    [Header("NPC ��ȭ")]
    public UI_NPCDialogue nPCDialogue;

    /// <summary>
    /// �� npc�� �� ����Ʈ���� ID��
    /// </summary>
    private List<int> quests_ID = new List<int>();
    /// <summary>
    /// �� npc�� �� ����Ʈ��
    /// </summary>
    private List<QuestsData> npcQuestList = new List<QuestsData>();

    private void Awake()
    {
        EventHandler.managerEvent.RegisterNPCManagerInitInit(Initialize);
    }

    private void OnDisable()
    {
        EventHandler.managerEvent.UnRegisterNPCManagerInitInit(Initialize);
    }
    private void Initialize()
    {
        if (QuestManager.Instance == null)
        {
            Debug.Log("NULL�̳�?");
        }
        else
        {
            QuestManager.Instance.OnUpdateQuestProgress += AvailableCompleteQuest;  // ����Ʈ �����Ȳ ������Ʈ���� �̺�Ʈ�� ȣ���Ѵ�.
        }
        
        quests_ID = NPCManager.Instance.GetQuestIDFromNPC(this.NPC_ID);

        List<QuestsData> allQuestList = QuestManager.Instance.questsDataList;

        if (quests_ID != null)
        {
            foreach (int questID in quests_ID)
            {
                QuestsData quest = allQuestList.Find(x => x.Quest_ID.Equals(questID));
                npcQuestList.Add(quest);
            }
        }        
        if (npcQuestList != null)
        {
            // npc�� �� ����Ʈ�� �ִٸ�
            startQuestIcon.SetActive(true);
            completeQuestIcon.SetActive(false);
        }
        //�� npc�� ��ȭâ �ʱ�ȭ
        nPCDialogue.Initialize();
    }
    /// <summary>
    /// ���� �� NPC ����Ʈ �߿��� �Ϸ� ������ ���� �ִٸ�
    /// </summary>
    private void AvailableCompleteQuest()
    {
        startQuestIcon.SetActive(false);
        completeQuestIcon.SetActive(true);
    }
}
