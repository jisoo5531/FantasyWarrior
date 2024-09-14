using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QuestInfo : MonoBehaviour
{
    [Header("Button")]
    public Button questStartButton;
    public Button questCompleteButton;
    [Header("퀘스트 정보")]
    public TMP_Text questNameLabel;
    public TMP_Text questStatusText;
    public TMP_Text questReqText;
    public TMP_Text questRewardText;
    public TMP_Text questDescText;

    private QuestsData questData;
    private Q_Status? questStatus;

    private void Awake()
    {
        questStartButton.onClick.AddListener(OnClickQuestStartButton);
        questCompleteButton.onClick.AddListener(OnClickCompleteButton);
    }

    public void Initialize(QuestsData questData, Q_Status? q_Status = null)
    {
        this.questData = questData;
        this.questStatus = q_Status;

        QuestInfoInit();
        CheckQuestStatus();


    }
    private void QuestInfoInit()
    {
        List<QuestObjectivesData> questObjList = QuestManager.Instance.questObjectList;
        QuestObjectivesData questOBJ = questObjList.Find(x => x.Quest_ID.Equals(this.questData.Quest_ID));  // 현재 퀘스트의 목표 정보
        questNameLabel.text = this.questData.Quest_Name;        
        questReqText.text = questOBJ.ReqAmount.ToString();
        questDescText.text = this.questData.DESC;
        questRewardText.text = $"Gold : {this.questData.Reward_Gold}, EXP : {this.questData.Reward_Exp}";
    }
    /// <summary>
    /// 퀘스트의 상태 파악
    /// </summary>
    private void CheckQuestStatus()
    {
        // TODO : 텍스트 한글로 바꾸기
        List<QuestProgress> questProgressList = QuestManager.Instance.questProgressList;

        if (questProgressList == null)
        {
            return;
        }
        if (this.questStatus == Q_Status.Completed)
        {
            questStatusText.text = "Completed";
            questStartButton.interactable = false;
            questCompleteButton.interactable = false;
            return;
        }
        int index = questProgressList.FindIndex((x) => { return x.quest_Id.Equals(questData.Quest_ID); });
        if (index >= 0)
        {
            questStartButton.interactable = false;
            if (questProgressList[index].IsQuestComplete())
            {
                questStatusText.text = "Available Complete";
                questCompleteButton.interactable = true;
            }
            else
            {
                questStatusText.text = "InProgress Quest";
            }
        }
        else
        {
            questStatusText.text = "Available QuestStart";
            questStartButton.interactable = true;
            questCompleteButton.interactable = false;
        }
    }
    /// <summary>
    /// 퀘스트 시작버튼을 누르면 퀘스트 시작
    /// </summary>
    private void OnClickQuestStartButton()
    {
        QuestManager.Instance.AcceptQuest(questData.Quest_ID);
    }
    /// <summary>
    /// 퀘스트 완료버튼을 누르면 퀘스트 완료처리
    /// </summary>
    private void OnClickCompleteButton()
    {
        QuestManager.Instance.QuestComplete(questData.Quest_ID);
    }
}
