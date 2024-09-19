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
    private List<int> quests_ID = new List<int>();
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
        if (QuestManager.Instance == null)
        {
            Debug.Log("NULL이냐?");
        }
        else
        {
            QuestManager.Instance.OnUpdateQuestProgress += AvailableCompleteQuest;  // 퀘스트 진행상황 업데이트마다 이벤트를 호출한다.
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
            // npc가 줄 퀘스트가 있다면
            startQuestIcon.SetActive(true);
            completeQuestIcon.SetActive(false);
        }
        //이 npc의 대화창 초기화
        nPCDialogue.Initialize();
    }
    /// <summary>
    /// 만약 이 NPC 퀘스트 중에서 완료 가능한 것이 있다면
    /// </summary>
    private void AvailableCompleteQuest()
    {
        startQuestIcon.SetActive(false);
        completeQuestIcon.SetActive(true);
    }
}
