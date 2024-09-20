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
    private List<int> questID_List = new List<int>();
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
        QuestManager.Instance.OnAcceptQuest += CheckQuestStatus;
        QuestManager.Instance.OnUpdateQuestProgress += CheckQuestStatus;  // ����Ʈ �����Ȳ ������Ʈ���� �̺�Ʈ�� ȣ���Ѵ�.
        QuestManager.Instance.OnCompleteQuest += CheckQuestStatus;

        
        CheckQuestStatus();

        //�� npc�� ��ȭâ �ʱ�ȭ
        nPCDialogue.Initialize();
    }
    /// <summary>
    /// �� NPC�� ����Ʈ���� ���� üũ (�Ϸᰡ������, �ƴ���)
    /// </summary>
    private void CheckQuestStatus()
    {        
        questID_List = NPCManager.Instance.GetQuestIDFromNPC(this.NPC_ID);
        List<QuestProgress> userQuestList = QuestManager.Instance.questProgressList;
        bool isAnyUserQuest = false;
        bool isAnyCompleteQuest = false;

        if (questID_List == null)
        {            
            startQuestIcon.SetActive(false);
            completeQuestIcon.SetActive(false);
            return;
        }
        foreach (int questID in questID_List)
        {                        
            QuestProgress userQuest = userQuestList.Find(x => x.quest_Id == questID);
            if (userQuest == null)
            {                
                // ���� ������ �ش� ����Ʈ�� ���� �ʾҴٸ�
                isAnyUserQuest = true;
                isAnyCompleteQuest = false;
            }
            else
            {
                isAnyUserQuest = false;
                isAnyCompleteQuest = true;
                // ���� ������ npc�� ����Ʈ�� �ϳ��� �Ϸ� �����ϴٸ�
                if (userQuest.IsQuestComplete())
                {                    
                    completeQuestIcon.GetComponent<Image>().ImageTransparent(1);
                    break;
                }                
                // �Ϸ� ������ ���� ���� ���� ���̶��
                completeQuestIcon.GetComponent<Image>().ImageTransparent(0.25f);
            }
        }

        startQuestIcon.SetActive(isAnyUserQuest);
        completeQuestIcon.SetActive(isAnyCompleteQuest);
    }
}
