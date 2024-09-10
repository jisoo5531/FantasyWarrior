using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuestPanel : MonoBehaviour
{
    // TODO : 0911. 퀘스트 패널과 윈도우. 진행상황이 업데이트될 때마다 UPDATE, UI도 같이 Update
    // TODO : 어떤 몬스터를 잡았을 때, 그 몬스터에 관한 퀘스트가 있는지 판별하고 / 있으면 퀘스트 상황 업데이트 하도록

    public GameObject QuestContent;
    public GameObject QuestInfo;

    private void Start()
    {
        List<QuestsData> questsDataList = QuestManager.Instance.GetQuestListFromDB();        
        foreach (QuestsData quest in questsDataList)
        {
            UI_QuestElement questElement =  Instantiate(QuestInfo, QuestContent.transform).GetComponent<UI_QuestElement>();
            questElement.Initialize(quest);
        }
    }
}
