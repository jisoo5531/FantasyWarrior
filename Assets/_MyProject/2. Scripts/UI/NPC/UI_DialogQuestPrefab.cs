using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_DialogQuestPrefab : MonoBehaviour
{
    public TMP_Text dialogNameLabel;

    /// <summary>
    /// 어떤 npc와 대화 중인지
    /// </summary>
    private int npc_ID;
    /// <summary>
    /// 이 스크립트가 실행되는 퀘스트의 ID
    /// </summary>
    private QuestData quest;
    /// <summary>
    /// 이 오브젝트에 부착된 버튼을 누르면 실행할 콜백 함수
    /// </summary>
    private Action<DialogStatus, int> clickStartDialog;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickQuestDialog);
    }
    public void Initialize(int npcID, QuestData quest, Action<DialogStatus, int> SelectDialog)
    {
        this.npc_ID = npcID;
        this.quest = quest;
        dialogNameLabel.text = quest.Quest_Name;
        clickStartDialog = SelectDialog;
    }

    private void OnClickQuestDialog()
    {
        Debug.Log($"클릭했다. {this.quest.Quest_Name}");        
        List<QuestProgress> userQuestList = QuestManager.Instance.questProgressList;
        QuestProgress userQuest = userQuestList.Find(x => x.quest_Id == this.quest.Quest_ID);
        if (userQuest == null)
        {
            Debug.Log("혹시 null?");
            // 현재 유저가 해당 퀘스트를 받지 않았다면
            clickStartDialog(DialogStatus.QuestStart, this.quest.Quest_ID);
        }
        else if (userQuest.IsQuestComplete())
        {
            // 현재 유저가 해당 퀘스트를 수행 중이라면
            // 해당 퀘스트가 완료 가능하다면
            clickStartDialog(DialogStatus.QuestEnd, this.quest.Quest_ID);
        }
        CheckTalkQuest();
    }
    /// <summary>
    /// 이 NPC한테 온 대화 퀘스트 있는지 확인
    /// </summary>
    private void CheckTalkQuest()
    {
        List<NPCTalkQuestData> talkList = NPCManager.Instance.talkNpcQuestList;
        // 이 NPC한테 온 대화 퀘스트 있는지 확인
        NPCTalkQuestData talkQuest = talkList.Find(x => x.NPC_ID == this.npc_ID);

        if (talkQuest != null)
        {
            // 해당되는 퀘스트가 있으면            
            // 해당 퀘스트의 완료 대화 진행.
            clickStartDialog(DialogStatus.QuestEnd, talkQuest.Quest_ID);
        }
    }
}
