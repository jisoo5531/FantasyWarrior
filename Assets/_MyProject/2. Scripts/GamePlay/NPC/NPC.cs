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
    public GameObject completeOrInprogressQuestIcon;
    [Header("NPC ��ȭ")]
    public UI_NPCDialogue nPCDialogue;

    /// <summary>
    /// �� npc�� �� ����Ʈ���� ID��
    /// </summary>
    private List<int> questID_List = new List<int>();
    /// <summary>
    /// �� npc�� �� ����Ʈ��
    /// </summary>
    private List<QuestData> npcQuestList = new List<QuestData>();

    private void Awake()
    {
        //EventHandler.managerEvent.RegisterNPCManagerInit(Initialize);
    }
    private void Start()
    {
        Initialize();
    }
    private void OnDisable()
    {
        EventHandler.managerEvent.UnRegisterNPCManagerInit(Initialize);
        QuestManager.Instance.OnAcceptQuest -= CheckQuestStatus;
        QuestManager.Instance.OnUpdateQuestProgress -= CheckQuestStatus;  // ����Ʈ �����Ȳ ������Ʈ���� �̺�Ʈ�� ȣ���Ѵ�.
        QuestManager.Instance.OnCompleteQuest -= CheckQuestStatus;
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
        bool isAnyCompleteOrInprogressQuest = false;

        if (questID_List == null)
        {
            UpdateQuestIcons(false, false);
            return;
        }
        foreach (int questID in questID_List)
        {
            QuestProgress userQuest = userQuestList.Find(x => x.quest_Id == questID);
            if (userQuest == null)
            {
                // ������ ����Ʈ�� ���� ���� ���
                isAnyUserQuest = true;
                isAnyCompleteOrInprogressQuest = false;
                break; // �� �̻� Ȯ���� �ʿ䰡 �����Ƿ� �ݺ��� Ż��
            }
            else if (userQuest.IsQuestComplete())
            {
                // ����Ʈ�� �Ϸ� ������ ���
                isAnyCompleteOrInprogressQuest = true;
                completeOrInprogressQuestIcon.GetComponent<Image>().ImageTransparent(1);
                break; // �Ϸ� ���� ����Ʈ�� ã�����Ƿ� �ݺ��� Ż��
            }
            else
            {
                // ����Ʈ ���� ��
                isAnyCompleteOrInprogressQuest = true;
                completeOrInprogressQuestIcon.GetComponent<Image>().ImageTransparent(0.25f);
            }
        }
        // ��ȭ ���� ����Ʈ ����Ʈ Ȯ��
        foreach (var talkList in NPCManager.Instance.talkNpcQuestList)
        {
            if (talkList.NPC_ID == this.NPC_ID)
            {
                isAnyCompleteOrInprogressQuest = true;
                isAnyUserQuest = false;
                break;
            }
        }
        // ������ ���� ������Ʈ
        UpdateQuestIcons(isAnyUserQuest, isAnyCompleteOrInprogressQuest);
    }
    /// <summary>
    /// ����Ʈ ���º��� �������� ������Ʈ�ϴ� �Լ�
    /// </summary>
    private void UpdateQuestIcons(bool hasNewQuest, bool hasCompleteQuest)
    {
        startQuestIcon.SetActive(hasNewQuest);
        completeOrInprogressQuestIcon.SetActive(hasCompleteQuest);
    }
}
