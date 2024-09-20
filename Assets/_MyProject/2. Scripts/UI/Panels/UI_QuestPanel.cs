using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestPanel : MonoBehaviour
{
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
    public UI_QuestInfo QuestInfoWindow;    
    [Header("QuestInfo Prefab")]
    public GameObject QuestInfoPrefab;
    public GameObject InProgressQuestInfo;
    public GameObject CompleteQuestInfo;


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
        QuestSet();
        QuestManager.Instance.OnAcceptQuest += QuestSet;
        QuestManager.Instance.OnUpdateQuestProgress += QuestSet;
        QuestManager.Instance.OnCompleteQuest += QuestSet;
    }

    private void QuestSet()
    {        
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
            UI_AllQuest questElement = Instantiate(QuestInfoPrefab, AllQuestContent.transform).GetComponent<UI_AllQuest>();
            questElement.Initialize(quest.Quest_ID, QuestInfoWindow);
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
                UI_InprogressQuest questElement = Instantiate(InProgressQuestInfo, InProgressQuestContent.transform).GetComponent<UI_InprogressQuest>();
                questElement.Initialize(questData.Quest_ID, QuestInfoWindow);
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
                UI_CompletedQuest questElement = Instantiate(CompleteQuestInfo, CompleteQuestContent.transform).GetComponent<UI_CompletedQuest>();
                questElement.Initialize(questData.Quest_ID, QuestInfoWindow);
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
