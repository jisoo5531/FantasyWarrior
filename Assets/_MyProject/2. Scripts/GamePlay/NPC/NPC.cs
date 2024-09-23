using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("NPC ID")]
    public int NPC_ID;
    [Header("퀘스트 상태")]
    public GameObject startQuestIcon;
    public GameObject completeOrInprogressQuestIcon;
    [Header("NPC 대화")]
    public UI_NPCDialogue nPCDialogue;

    /// <summary>
    /// 이 npc가 줄 퀘스트들의 ID들
    /// </summary>
    private List<int> questID_List = new List<int>();
    /// <summary>
    /// 이 npc가 줄 퀘스트들
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
        QuestManager.Instance.OnUpdateQuestProgress -= CheckQuestStatus;  // 퀘스트 진행상황 업데이트마다 이벤트를 호출한다.
        QuestManager.Instance.OnCompleteQuest -= CheckQuestStatus;
    }
    private void Initialize()
    {        
        QuestManager.Instance.OnAcceptQuest += CheckQuestStatus;
        QuestManager.Instance.OnUpdateQuestProgress += CheckQuestStatus;  // 퀘스트 진행상황 업데이트마다 이벤트를 호출한다.
        QuestManager.Instance.OnCompleteQuest += CheckQuestStatus;

        CheckQuestStatus();

        //이 npc의 대화창 초기화
        nPCDialogue.Initialize();
    }

    /// <summary>
    /// 이 NPC의 퀘스트들의 상태 체크 (완료가능한지, 아닌지)
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
                // 유저가 퀘스트를 받지 않은 경우
                isAnyUserQuest = true;
                isAnyCompleteOrInprogressQuest = false;
                break; // 더 이상 확인할 필요가 없으므로 반복문 탈출
            }
            else if (userQuest.IsQuestComplete())
            {
                // 퀘스트가 완료 가능한 경우
                isAnyCompleteOrInprogressQuest = true;
                completeOrInprogressQuestIcon.GetComponent<Image>().ImageTransparent(1);
                break; // 완료 가능 퀘스트를 찾았으므로 반복문 탈출
            }
            else
            {
                // 퀘스트 진행 중
                isAnyCompleteOrInprogressQuest = true;
                completeOrInprogressQuestIcon.GetComponent<Image>().ImageTransparent(0.25f);
            }
        }
        // 대화 연계 퀘스트 리스트 확인
        foreach (var talkList in NPCManager.Instance.talkNpcQuestList)
        {
            if (talkList.NPC_ID == this.NPC_ID)
            {
                isAnyCompleteOrInprogressQuest = true;
                isAnyUserQuest = false;
                break;
            }
        }
        // 아이콘 상태 업데이트
        UpdateQuestIcons(isAnyUserQuest, isAnyCompleteOrInprogressQuest);
    }
    /// <summary>
    /// 퀘스트 상태별로 아이콘을 업데이트하는 함수
    /// </summary>
    private void UpdateQuestIcons(bool hasNewQuest, bool hasCompleteQuest)
    {
        startQuestIcon.SetActive(hasNewQuest);
        completeOrInprogressQuestIcon.SetActive(hasCompleteQuest);
    }
}
