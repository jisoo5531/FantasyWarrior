using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuestPanel : MonoBehaviour
{
    public GameObject QuestContent;
    public GameObject QuestInfo;

    private void Start()
    {
        List<QuestsData> questsDataList = QuestManager.Instance.GetQuestDataFromDB();        
        foreach (QuestsData quest in questsDataList)
        {
            UI_QuestElement questElement =  Instantiate(QuestInfo, QuestContent.transform).GetComponent<UI_QuestElement>();
            questElement.Initialize(quest);
        }
    }
}
