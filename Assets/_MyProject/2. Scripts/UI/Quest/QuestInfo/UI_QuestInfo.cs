using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QuestInfo : MonoBehaviour
{
    private QuestsData questData;
    [Header("Button")]
    public Button questStartButton;
    public Button questCompleteButton;
    [Header("����Ʈ ����")]
    public TMP_Text questNameLabel;
    public TMP_Text questStatusText;
    public TMP_Text questReqText;
    public TMP_Text questRewardText;
    public TMP_Text questDescText;


    private void Awake()
    {
        questStartButton.onClick.AddListener(OnClickQuestStartButton);
        questCompleteButton.onClick.AddListener(OnClickCompleteButton);
    }

    public void Initialize(QuestsData questData)
    {
        this.questData = questData;

        QuestInfoInit();
        CheckQuestStatus();


    }
    private void QuestInfoInit()
    {
        List<QuestObjectivesData> questObjList = QuestManager.Instance.questObjectList;
        QuestObjectivesData questOBJ = questObjList.Find(x => x.Quest_ID.Equals(this.questData.Quest_ID));  // ���� ����Ʈ�� ��ǥ ����
        questNameLabel.text = this.questData.Quest_Name;        
        questReqText.text = questOBJ.ReqAmount.ToString();
        questDescText.text = this.questData.DESC;
        questRewardText.text = $"Gold : {this.questData.Reward_Gold}, EXP : {this.questData.Reward_Exp}";
    }
    private void CheckQuestStatus()
    {
        List<QuestProgress> questProgressList = QuestManager.Instance.questProgressList;
        if (questProgressList == null)
        {
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
        QuestManager.Instance.QuestComplete(questData.Quest_ID);
    }
}
