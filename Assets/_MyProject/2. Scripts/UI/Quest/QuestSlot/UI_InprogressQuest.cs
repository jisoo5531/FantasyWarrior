using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InprogressQuest : UI_QuestElement
{
    public Button GuideButton;
    public Image GuideImage;
    /// <summary>
    /// 게임 상의 가이드로 띄울 것인지 여부를 확인할 변수
    /// </summary>
    public bool isGuided = false;

    public static event Action OnGuideButton;

    public override void Initialize(int questID, UI_QuestInfo questInfoWindow)
    {
        base.Initialize(questID, questInfoWindow);
        GuideButton.onClick.AddListener(OnClickGuide);
        
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
        OnGuideButton?.Invoke();
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
}
