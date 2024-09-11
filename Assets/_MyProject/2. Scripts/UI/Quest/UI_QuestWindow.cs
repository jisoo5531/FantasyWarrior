using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Panel�� �ƴ� ���� ��ܿ� ������ �����ϸ鼭�� ������ Quest â
/// </summary>
public class UI_QuestWindow : MonoBehaviour
{
    // TODO : ������ ������ ���̵忡 üũ�� �͵鸸 �����쿡 �����Բ�
    public GameObject QW_ObjectiveList;
    public UI_QW_Objective objectiveElement;    

    private void Start()
    {
        UI_InprogressQuest.OnGuideButton += SetQuestWindow;
        SetQuestWindow();
    }
    private void SetQuestWindow()
    {        
        List<QuestProgress> questProgressList = QuestManager.Instance.questProgressList;
        if (questProgressList == null)
        {
            // ������ ���� ���� ����Ʈ�� ���ٸ�
            return;
        }
        ObjectiveListClear(QW_ObjectiveList);
        foreach (QuestProgress questProgress in questProgressList)
        {            
            // �˸��̷� ����� �Ǿ��ִٸ�
            if (questProgress.isGuide)
            {
                UI_QW_Objective objective = Instantiate(objectiveElement.gameObject, QW_ObjectiveList.transform).GetComponent<UI_QW_Objective>();
                QuestsData quest = QuestManager.Instance.GetQuestData(questProgress.quest_Id);
                objective.Initialize(quest);
            }            
        }
    }
    /// <summary>
    /// ����Ʈ ������ �˸��� Ŭ����
    /// </summary>
    private void ObjectiveListClear(GameObject content)
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
}
