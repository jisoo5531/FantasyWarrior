using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestPanel : MonoBehaviour
{
    // TODO : 0911. 퀘스트 패널과 윈도우. 진행상황이 업데이트될 때마다 UPDATE, UI도 같이 Update
    // TODO : 어떤 몬스터를 잡았을 때, 그 몬스터에 관한 퀘스트가 있는지 판별하고 / 있으면 퀘스트 상황 업데이트 하도록

    [Header("모든 퀘스트")]
    public Button allQuestTabButton;
    public GameObject AllQuestContent;
    [Header("진행중 퀘스트")]
    public Button InProgressQuestButton;
    public GameObject InProgressQuestContent;
    [Header("완료 퀘스트")]
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
            // 퀘스트가 없다면
            return;
        }
        ContentClear(AllQuestContent);
        foreach (QuestsData quest in questsDataList)        
        {            
            Q_Status? questStatus = QuestManager.Instance.GetQuestStatus(quest.Quest_ID);
            if (questStatus == Q_Status.Completed || questStatus == Q_Status.InProgress)
            {
                // 진행 중이거나 완료한 퀘스트는 목록에서 제외.
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
            // 진행중인 퀘스트 없음
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
            // 완료한 퀘스트 없음
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
