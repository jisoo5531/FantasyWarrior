using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InprogressQuest : UI_QuestElement
{
    /// <summary>
    /// 인게임 퀘스트 윈도우에 빠른 알림으로 등록하기 위한 버튼
    /// </summary>
    public Button GuideButton;
    /// <summary>
    /// 네비 화살표를 설정할 것인지에 대한 버튼
    /// </summary>
    public Button NavGuideButton;
    public Image GuideImage;
    /// <summary>
    /// 게임 상의 가이드로 띄울 것인지 여부를 확인할 변수
    /// </summary>
    public bool isGuided = false;

    public static event Action<int> OnGuideButton;

    public override void Initialize(int userId, int questID, UI_QuestInfo questInfoWindow)
    {
        base.Initialize(userId, questID, questInfoWindow);
        GuideButton.onClick.AddListener(OnClickGuide);        
    }
    
    protected override void OnClickToOpenQuestInfoWindow()
    {
        base.OnClickToOpenQuestInfoWindow();
        if (isInfoOpen)
        {
            questInfoWindow.Initialize(this.userId, this.quest, Q_Status.InProgress);
        }        
    }
    /// <summary>
    /// 알림이 버튼을 클릭할 시
    /// </summary>
    private void OnClickGuide()
    {
        isGuided = !isGuided;
        if (isGuided)
        {
            GuideImage.ImageTransparent(1);
        }
        else
        {
            GuideImage.ImageTransparent(0);
        }
        SetIsGuide();
        OnGuideButton?.Invoke(DatabaseManager.Instance.userData.UID);
    }
    /// <summary>
    /// 알림이 버튼을 클릭할 시 IsGuide변수 세팅
    /// </summary>
    private void SetIsGuide()
    {
        List<QuestProgress> questProgressList = QuestManager.Instance.questProgressList;
        int index = questProgressList.FindIndex((x) => x.quest_Id.Equals(quest.Quest_ID));
        if (index < 0)
        {
            return;
        }
        questProgressList[index].UpdateGuideOnOff(isGuided);
    }
    /// <summary>
    /// 길찾기 서비스 버튼을 클릭할 때
    /// </summary>
    private void OnClickNavQuestButton()
    {
        QuestObjectiveData objectiveData = QuestManager.Instance.GetObjectiveData(this.quest.Quest_ID);
        //if (objectiveData.)
        //{

        //}
    }
}
