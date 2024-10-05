using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
/// <summary>
/// Panel이 아닌 우측 상단에 보여질 게임하면서도 보여질 Quest 창
/// </summary>
public class UI_QuestWindow : MonoBehaviour
{    
    public GameObject QW_ObjectiveList;
    [Header("Prefab")]
    public UI_QW_Objective objectiveElement;

    private int userId;

    private void Start()
    {        
        // 서버에서 실행되지 않도록
        if (NetworkServer.active)
        {
            return; // 서버에서는 UI를 실행하지 않음
        }
        // 로컬 플레이어 체크
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // 로컬 플레이어가 아닐 경우 UI를 실행하지 않음
        }

        
        this.userId = DatabaseManager.Instance.GetPlayerData(transform.root.gameObject).UserId;

        UI_InprogressQuest.OnGuideButton += SetQuestWindow;
        QuestManager.Instance.OnUpdateQuestProgress += SetQuestWindow;
        QuestManager.Instance.OnCompleteQuest += SetQuestWindow;
        SetQuestWindow(userId);
    }

    private void SetQuestWindow(int userId)
    {
        if (this.userId != userId)
        {
            return;
        }

        List<QuestProgress> questProgressList = QuestManager.Instance.questProgressList;
        if (questProgressList == null)
        {
            // 유저가 수행 중인 퀘스트가 없다면
            return;
        }
        ObjectiveListClear(QW_ObjectiveList);
        foreach (QuestProgress questProgress in questProgressList)
        {            
            // 알림이로 등록이 되어있다면
            if (questProgress.isGuide)
            {
                UI_QW_Objective objective = Instantiate(objectiveElement.gameObject, QW_ObjectiveList.transform).GetComponent<UI_QW_Objective>();
                QuestData quest = QuestManager.Instance.GetQuestData(questProgress.quest_Id);
                objective.Initialize(quest, questProgress);
            }            
        }
    }
    /// <summary>
    /// 퀘스트 윈도우 알림이 클리어
    /// </summary>
    private void ObjectiveListClear(GameObject content)
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
}
