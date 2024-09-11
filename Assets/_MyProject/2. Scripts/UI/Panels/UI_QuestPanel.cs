using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestPanel : MonoBehaviour
{
    // TODO : 0911. ����Ʈ �гΰ� ������. �����Ȳ�� ������Ʈ�� ������ UPDATE, UI�� ���� Update
    // TODO : � ���͸� ����� ��, �� ���Ϳ� ���� ����Ʈ�� �ִ��� �Ǻ��ϰ� / ������ ����Ʈ ��Ȳ ������Ʈ �ϵ���

    [Header("��� ����Ʈ")]
    public Button allQuestTabButton;
    public GameObject AllQuestContent;
    [Header("������ ����Ʈ")]
    public Button InProgressQuestButton;
    public GameObject InProgressQuestContent;
    [Header("�Ϸ� ����Ʈ")]
    public Button CompleteQuestButton;
    public GameObject CompleteQuestContent;

    [Header("QuestInfo Prefab")]
    public GameObject QuestInfo;
    public GameObject InProgressQuestInfo;

    private void Awake()
    {
        //allQuestTabButton.onClick.AddListener()
        allQuestTabButton.onClick.AddListener(() =>
        {
            AllQuestContent.transform.parent.parent.parent.gameObject.SetActive(true);
            InProgressQuestContent.transform.parent.parent.parent.gameObject.SetActive(false);
            CompleteQuestContent.transform.parent.parent.parent.gameObject.SetActive(false);
        });
        InProgressQuestButton.onClick.AddListener(() =>
        {
            InProgressQuestContent.transform.parent.parent.parent.gameObject.SetActive(true);
            AllQuestContent.transform.parent.parent.parent.gameObject.SetActive(false);
            CompleteQuestContent.transform.parent.parent.parent.gameObject.SetActive(false);
        });
        CompleteQuestButton.onClick.AddListener(() =>
        {
            CompleteQuestContent.transform.parent.parent.parent.gameObject.SetActive(true);
            AllQuestContent.transform.parent.parent.parent.gameObject.SetActive(false);
            InProgressQuestContent.transform.parent.parent.parent.gameObject.SetActive(false);
        });        
    }
    private void Start()
    {        
        QuestManager.Instance.OnAcceptQuest += QuestSet;

        QuestSet();
    }

    private void QuestSet()
    {
        AllQuestSetting();
        InProgressQuestSetting();
        CompleteQuestSetting();
    }

    private void AllQuestSetting()
    {
        List<QuestsData> questsDataList = QuestManager.Instance.GetQuestListFromDB();
        if (questsDataList == null)
        {
            // ����Ʈ�� ���ٸ�
            return;
        }
        ContentClear(AllQuestContent);
        foreach (QuestsData quest in questsDataList)        
        {            
            Q_Status? questStatus = QuestManager.Instance.GetQuestStatus(quest.Quest_ID);
            if (questStatus == Q_Status.Completed || questStatus == Q_Status.InProgress)
            {
                // ���� ���̰ų� �Ϸ��� ����Ʈ�� ��Ͽ��� ����.
                continue;
            }
            UI_QuestElement questElement = Instantiate(QuestInfo, AllQuestContent.transform).GetComponent<UI_QuestElement>();
            questElement.Initialize(quest.Quest_ID);
        }
    }
    private void InProgressQuestSetting()
    {
        List<UserQuestsData> InprogressList = QuestManager.Instance.GetInProgressQuest();
        if (InprogressList == null)
        {
            // �������� ����Ʈ ����
            return;
        }
        ContentClear(InProgressQuestContent);
        foreach (UserQuestsData questData in InprogressList)
        {
            if (questData.questStatus == Q_Status.InProgress)
            {
                UI_QuestElement questElement = Instantiate(InProgressQuestInfo, InProgressQuestContent.transform).GetComponent<UI_QuestElement>();
                questElement.Initialize(questData.Quest_ID);
            }
        }
    }
    private void CompleteQuestSetting()
    {
        List<UserQuestsData> CompletedList = QuestManager.Instance.GetCompletedQuest();
        if (CompletedList == null)
        {
            // �Ϸ��� ����Ʈ ����
            return;
        }
        ContentClear(CompleteQuestContent);
        foreach (UserQuestsData questData in CompletedList)
        {
            if (questData.questStatus == Q_Status.Completed)
            {
                UI_QuestElement questElement = Instantiate(QuestInfo, CompleteQuestContent.transform).GetComponent<UI_QuestElement>();
                questElement.Initialize(questData.Quest_ID);
            }
        }
    }
    private void ContentClear(GameObject content)
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
}
