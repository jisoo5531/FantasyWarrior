using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestPanel : MonoBehaviour
{
    // TODO : 0912. ����Ʈ ���� ��Ȳ ���� �ذ��ϱ�. ����Ʈ�� �Ϸ�� �� ���� �ǿ��� �Ϸ� ������ �Ѿ�� �Ѵ�.
    // �� ��, �Ϸ������� �Ѿ������ ���� �ǿ��� ������� �ʴ� ���� �߻�
    // TODO : ����Ʈ ������ â �����

    [Header("��� ����Ʈ")]
    public Button allQuestTabButton;
    public GameObject AllQuestContent;
    [Header("������ ����Ʈ")]
    public Button InProgressQuestButton;
    public GameObject InProgressQuestContent;
    [Header("�Ϸ� ����Ʈ")]
    public Button CompleteQuestButton;
    public GameObject CompleteQuestContent;

    [Header("QuestInfo")]    
    public UI_AllQuestInfo ALLQuestInfoWindow;
    public UI_InProgressQuestInfo InProgressQuestInfoWindow;
    [Header("QuestInfo Prefab")]
    public GameObject QuestInfoPrefab;
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
    public void QuestPanelInit()
    {
        Debug.Log("�ʱ�ȭ �Ƴ�?");
        QuestSet();
        QuestManager.Instance.OnAcceptQuest += QuestSet;
        QuestManager.Instance.OnUpdateQuestProgress += QuestSet;
        QuestManager.Instance.OnCompleteQuest += QuestSet;
    }

    private void QuestSet()
    {
        Debug.Log("����?");
        AllQuestSetting();
        InProgressQuestSetting();
        CompleteQuestSetting();
    }
    /// <summary>
    /// ����Ʈ ��� ����
    /// </summary>
    private void AllQuestSetting()
    {
        ContentClear(AllQuestContent);
        List<QuestsData> questsDataList = QuestManager.Instance.questsDataList;        
        if (questsDataList == null)
        {
            // ����Ʈ�� ���ٸ�
            return;
        }
        
        foreach (QuestsData quest in questsDataList)        
        {            
            Q_Status? questStatus = QuestManager.Instance.GetQuestStatus(quest.Quest_ID);
            if (questStatus == Q_Status.Completed || questStatus == Q_Status.InProgress)
            {
                // ���� ���̰ų� �Ϸ��� ����Ʈ�� ��Ͽ��� ����.
                continue;
            }
            UI_QuestElement questElement = Instantiate(QuestInfoPrefab, AllQuestContent.transform).GetComponent<UI_QuestElement>();
            questElement.Initialize(quest.Quest_ID, ALLQuestInfoWindow);
        }
    }
    /// <summary>
    /// �������� ����Ʈ ����
    /// </summary>
    private void InProgressQuestSetting()
    {
        ContentClear(InProgressQuestContent);
        List<UserQuestsData> InprogressList = QuestManager.Instance.GetInProgressQuest();        
        if (InprogressList == null)
        {
            // �������� ����Ʈ ����
            Debug.Log("�������� ����Ʈ ����.");
            return;
        }
        
        foreach (UserQuestsData questData in InprogressList)
        {
            if (questData.questStatus == Q_Status.InProgress)
            {
                UI_QuestElement questElement = Instantiate(InProgressQuestInfo, InProgressQuestContent.transform).GetComponent<UI_QuestElement>();
                questElement.Initialize(questData.Quest_ID, InProgressQuestInfoWindow);
            }
        }
    }
    /// <summary>
    /// �Ϸ��� ����Ʈ ����
    /// </summary>
    private void CompleteQuestSetting()
    {
        ContentClear(CompleteQuestContent);
        List<UserQuestsData> CompletedList = QuestManager.Instance.GetCompletedQuest();
        if (CompletedList == null)
        {
            // �Ϸ��� ����Ʈ ����
            return;
        }
        
        foreach (UserQuestsData questData in CompletedList)
        {
            if (questData.questStatus == Q_Status.Completed)
            {
                UI_QuestElement questElement = Instantiate(QuestInfoPrefab, CompleteQuestContent.transform).GetComponent<UI_QuestElement>();
                questElement.Initialize(questData.Quest_ID, ALLQuestInfoWindow);
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
