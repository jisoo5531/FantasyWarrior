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
    public GameObject completeQuestIcon;
    [Header("NPC 대화")]
    public UI_NPCDialogue nPCDialogue;

    /// <summary>
    /// 이 npc가 줄 퀘스트들의 ID들
    /// </summary>
    private List<int> questID_List = new List<int>();
    /// <summary>
    /// 이 npc가 줄 퀘스트들
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
                // 현재 유저가 해당 퀘스트를 받지 않았다면
                isAnyUserQuest = true;
                isAnyCompleteQuest = false;
            }
            else
            {
                isAnyUserQuest = false;
                isAnyCompleteQuest = true;
                // 현재 유저가 npc의 퀘스트를 하나라도 완료 가능하다면
                if (userQuest.IsQuestComplete())
                {                    
                    completeQuestIcon.GetComponent<Image>().ImageTransparent(1);
                    break;
                }                
                // 완료 가능한 것이 없고 수행 중이라면
                completeQuestIcon.GetComponent<Image>().ImageTransparent(0.25f);
            }
        }

        startQuestIcon.SetActive(isAnyUserQuest);
        completeQuestIcon.SetActive(isAnyCompleteQuest);
    }
}
