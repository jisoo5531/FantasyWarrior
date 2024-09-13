using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestPanel : MonoBehaviour
{
    // TODO : 0912. 퀘스트 진행 상황 오류 해결하기. 퀘스트가 완료될 때 진행 탭에서 완료 탭으로 넘어가야 한다.
    // 이 때, 완료탭으로 넘어가지지만 진행 탭에서 사라지지 않는 버그 발생
    // TODO : 퀘스트 상세정보 창 만들기

    [Header("모든 퀘스트")]
    public Button allQuestTabButton;
    public GameObject AllQuestContent;
    [Header("진행중 퀘스트")]
    public Button InProgressQuestButton;
    public GameObject InProgressQuestContent;
    [Header("완료 퀘스트")]
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
        Debug.Log("초기화 됐나?");
        QuestSet();
        QuestManager.Instance.OnAcceptQuest += QuestSet;
        QuestManager.Instance.OnUpdateQuestProgress += QuestSet;
        QuestManager.Instance.OnCompleteQuest += QuestSet;
    }

    private void QuestSet()
    {
        Debug.Log("여기?");
        AllQuestSetting();
        InProgressQuestSetting();
        CompleteQuestSetting();
    }
    /// <summary>
    /// 퀘스트 목록 세팅
    /// </summary>
    private void AllQuestSetting()
    {
        ContentClear(AllQuestContent);
        List<QuestsData> questsDataList = QuestManager.Instance.questsDataList;        
        if (questsDataList == null)
        {
            // 퀘스트가 없다면
            return;
        }
        
        foreach (QuestsData quest in questsDataList)        
        {            
            Q_Status? questStatus = QuestManager.Instance.GetQuestStatus(quest.Quest_ID);
            if (questStatus == Q_Status.Completed || questStatus == Q_Status.InProgress)
            {
                // 진행 중이거나 완료한 퀘스트는 목록에서 제외.
                continue;
            }
            UI_QuestElement questElement = Instantiate(QuestInfoPrefab, AllQuestContent.transform).GetComponent<UI_QuestElement>();
            questElement.Initialize(quest.Quest_ID, ALLQuestInfoWindow);
        }
    }
    /// <summary>
    /// 진행중인 퀘스트 세팅
    /// </summary>
    private void InProgressQuestSetting()
    {
        ContentClear(InProgressQuestContent);
        List<UserQuestsData> InprogressList = QuestManager.Instance.GetInProgressQuest();        
        if (InprogressList == null)
        {
            // 진행중인 퀘스트 없음
            Debug.Log("진행중인 퀘스트 없다.");
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
    /// 완료한 퀘스트 세팅
    /// </summary>
    private void CompleteQuestSetting()
    {
        ContentClear(CompleteQuestContent);
        List<UserQuestsData> CompletedList = QuestManager.Instance.GetCompletedQuest();
        if (CompletedList == null)
        {
            // 완료한 퀘스트 없음
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
