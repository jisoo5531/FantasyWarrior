using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Panel이 아닌 우측 상단에 보여질 게임하면서도 보여질 Quest 창
/// </summary>
public class UI_QuestWindow : MonoBehaviour
{
    // TODO : 유저가 윈도우 가이드에 체크한 것들만 윈도우에 나오게끔
    public GameObject QW_ObjectiveList;
    public UI_QW_Objective objectiveElement;

    private void Start()
    {
        List<UserQuestsData> userQuests = QuestManager.Instance.GetUserQuest();
        if (userQuests == null)
        {
            // 유저가 수행 중인 퀘스트가 없다면
            return;
        }        
        foreach (UserQuestsData userQuest in userQuests)
        {
            UI_QW_Objective objective = Instantiate(objectiveElement.gameObject, QW_ObjectiveList.transform).GetComponent<UI_QW_Objective>();
            QuestsData quest = QuestManager.Instance.GetQuestData(userQuest.Quest_ID);
            objective.Initialize(quest);
        }
    }
}
