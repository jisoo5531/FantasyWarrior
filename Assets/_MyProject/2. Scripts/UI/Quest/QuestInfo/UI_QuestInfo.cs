using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

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

    private QuestData questData;
    private Q_Status? questStatus;

    private int userId;

    private void Awake()
    {
        // 로컬 플레이어 체크
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // 로컬 플레이어가 아닐 경우 UI를 실행하지 않음
        }
        questStartButton.onClick.AddListener(OnClickQuestStartButton);
        questCompleteButton.onClick.AddListener(OnClickCompleteButton);
    }

    public void Initialize(int userId, QuestData questData, Q_Status? q_Status = null)
    {
        this.userId = userId;
        this.questData = questData;
        this.questStatus = q_Status;

        QuestInfoInit();
        CheckQuestStatus();


    }
    private void QuestInfoInit()
    {
        Dictionary<int, QuestObjectiveData> questObjDict = QuestManager.Instance.questObjectDict;        
        QuestObjectiveData questOBJ = questObjDict[this.questData.Quest_ID];   // 현재 퀘스트의 목표 정보

        int? reqAmount = QuestManager.Instance.GetRequireComplete(this.questData.Quest_ID);
        string reqAmountText = reqAmount != null ? reqAmount.ToString() : string.Empty;

        questNameLabel.text = this.questData.Quest_Name;        
        questReqText.text = reqAmountText;
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
        int index = questProgressList.FindIndex(x => x.quest_Id.Equals(questData.Quest_ID));
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
                questCompleteButton.interactable = false;
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
        QuestManager.Instance.QuestComplete(userId, questData.Quest_ID);
    }
}
