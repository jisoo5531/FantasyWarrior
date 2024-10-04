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
    [Header("����Ʈ ����")]
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
        // ���� �÷��̾� üũ
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // ���� �÷��̾ �ƴ� ��� UI�� �������� ����
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
        QuestObjectiveData questOBJ = questObjDict[this.questData.Quest_ID];   // ���� ����Ʈ�� ��ǥ ����

        int? reqAmount = QuestManager.Instance.GetRequireComplete(this.questData.Quest_ID);
        string reqAmountText = reqAmount != null ? reqAmount.ToString() : string.Empty;

        questNameLabel.text = this.questData.Quest_Name;        
        questReqText.text = reqAmountText;
        questDescText.text = this.questData.DESC;
        questRewardText.text = $"Gold : {this.questData.Reward_Gold}, EXP : {this.questData.Reward_Exp}";
    }
    /// <summary>
    /// ����Ʈ�� ���� �ľ�
    /// </summary>
    private void CheckQuestStatus()
    {
        // TODO : �ؽ�Ʈ �ѱ۷� �ٲٱ�
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
    /// ����Ʈ ���۹�ư�� ������ ����Ʈ ����
    /// </summary>
    private void OnClickQuestStartButton()
    {
        QuestManager.Instance.AcceptQuest(questData.Quest_ID);
    }
    /// <summary>
    /// ����Ʈ �Ϸ��ư�� ������ ����Ʈ �Ϸ�ó��
    /// </summary>
    private void OnClickCompleteButton()
    {
        QuestManager.Instance.QuestComplete(userId, questData.Quest_ID);
    }
}
