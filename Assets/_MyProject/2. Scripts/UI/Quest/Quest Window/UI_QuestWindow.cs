using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Panel�� �ƴ� ���� ��ܿ� ������ �����ϸ鼭�� ������ Quest â
/// </summary>
public class UI_QuestWindow : MonoBehaviour
{    
    public GameObject QW_ObjectiveList;
    [Header("Prefab")]
    public UI_QW_Objective objectiveElement;    

    private void Start()
    {
        UI_InprogressQuest.OnGuideButton += SetQuestWindow;
        QuestManager.Instance.OnUpdateQuestProgress += SetQuestWindow;
        QuestManager.Instance.OnCompleteQuest += SetQuestWindow;
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
                QuestData quest = QuestManager.Instance.GetQuestData(questProgress.quest_Id);
                objective.Initialize(quest, questProgress);
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
