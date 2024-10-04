using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InprogressQuest : UI_QuestElement
{
    /// <summary>
    /// �ΰ��� ����Ʈ �����쿡 ���� �˸����� ����ϱ� ���� ��ư
    /// </summary>
    public Button GuideButton;
    /// <summary>
    /// �׺� ȭ��ǥ�� ������ �������� ���� ��ư
    /// </summary>
    public Button NavGuideButton;
    public Image GuideImage;
    /// <summary>
    /// ���� ���� ���̵�� ��� ������ ���θ� Ȯ���� ����
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
    /// �˸��� ��ư�� Ŭ���� ��
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
    /// �˸��� ��ư�� Ŭ���� �� IsGuide���� ����
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
    /// ��ã�� ���� ��ư�� Ŭ���� ��
    /// </summary>
    private void OnClickNavQuestButton()
    {
        QuestObjectiveData objectiveData = QuestManager.Instance.GetObjectiveData(this.quest.Quest_ID);
        //if (objectiveData.)
        //{

        //}
    }
}
