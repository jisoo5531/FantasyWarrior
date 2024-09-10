using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Panel이 아닌 우측 상단에 보여질 게임하면서도 보여질 Quest 창
/// </summary>
public class UI_QuestWindow : MonoBehaviour
{
    public GameObject QW_ObjectiveList;
    public UI_QW_Objective objectiveElement;

    private void Start()
    {
        List<UserQuestsData> userQuests = QuestManager.Instance.GetUserQuest();
        foreach (UserQuestsData userQuest in userQuests)
        {
            UI_QW_Objective objective = Instantiate(objectiveElement.gameObject, QW_ObjectiveList.transform).GetComponent<UI_QW_Objective>();
            QuestsData quest = QuestManager.Instance.GetQuestData(userQuest.Quest_ID);
            objective.Initialize(quest);
        }
    }
}
