using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InProgressQuestInfo : UI_QuestInfo
{
    public Button CompleteButton;

    private void Awake()
    {
        CompleteButton.onClick.AddListener(OnClickCompleteButton);
    }
    public override void Initialize(QuestsData questData)
    {
        base.Initialize(questData);
        List<QuestProgress> questProgressList = QuestManager.Instance.questProgressList;
        int index = questProgressList.FindIndex((x) => { return x.quest_Id.Equals(questData.Quest_ID); });
        CompleteButton.interactable = questProgressList[index].IsQuestComplete();
    }

    /// <summary>
    /// 퀘스트 완료버튼을 누르면 퀘스트 완료처리
    /// </summary>
    private void OnClickCompleteButton()
    {
        QuestManager.Instance.QuestComplete(questData.Quest_ID);
    }
}
