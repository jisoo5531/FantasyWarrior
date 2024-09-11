using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QuestElement : MonoBehaviour
{
    // TODO : 0911. 퀘스트 관리. 
    // 특정 퀘스트의 진행도 판단. 완료 여부. 목표에 맞춰서 완료 되는지
    /// <summary>
    /// 가이드에 띄우기 위한 버튼
    /// </summary>     
    
    public TMP_Text questNameText;
    public TMP_Text completeText;    

    protected QuestsData quest;    

    public void Initialize(int questID)
    {
        this.quest = QuestManager.Instance.GetQuestData(questID);        
        questNameText.text = quest.Quest_Name;
        Q_Status? questStatus = QuestManager.Instance.GetQuestStatus(quest.Quest_ID);
        if (questStatus == null)
        {
            Debug.Log("퀘스트 아이디 잘못 됨");
        }
        completeText.text = questStatus.ToString();
    }
    
}
