using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuestPanel : MonoBehaviour
{
    // TODO : 0911. ����Ʈ �гΰ� ������. �����Ȳ�� ������Ʈ�� ������ UPDATE, UI�� ���� Update
    // TODO : � ���͸� ����� ��, �� ���Ϳ� ���� ����Ʈ�� �ִ��� �Ǻ��ϰ� / ������ ����Ʈ ��Ȳ ������Ʈ �ϵ���

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
