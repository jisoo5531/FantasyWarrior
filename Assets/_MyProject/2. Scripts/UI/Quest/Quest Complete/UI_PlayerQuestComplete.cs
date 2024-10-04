using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class UI_PlayerQuestComplete : MonoBehaviour
{
    public GameObject go_ComleteQuest;

    public TMP_Text questNameLabel;
    public TMP_Text reward_ExpText;
    public TMP_Text reward_GoldText;
    private int userID;

    private QuestData quest;

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

        QuestManager.Instance.OnCompleteQuestData += OnCompleteQuestData;
    }

    private void OnCompleteQuestData(QuestData quest, int userID)
    {
        this.userID = userID;
        this.quest = quest;
        questNameLabel.text = quest.Quest_Name;
        reward_ExpText.text = quest.Reward_Exp.ToString();
        reward_GoldText.text = quest.Reward_Gold.ToString();
        OnCompleteQuest_GO();
    }    
    private void OnCompleteQuest_GO()
    {
        go_ComleteQuest.SetActive(true);
        Invoke("OffCompleteQuest_Go", 3.5f);
    }
    private void OffCompleteQuest_Go()
    {
        go_ComleteQuest.SetActive(false);
        QuestManager.Instance.TriggerGetReward(quest.Quest_ID, userID);
    }
}
