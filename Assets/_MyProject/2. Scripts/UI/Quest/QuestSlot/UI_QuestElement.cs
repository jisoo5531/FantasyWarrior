using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QuestElement : MonoBehaviour
{        
        
    public TMP_Text questNameText;
    public TMP_Text completeText;

    protected UI_QuestInfo questInfoWindow;

    /// <summary>
    /// 해당 항목에 있는 퀘스트 정보
    /// </summary>
    protected QuestData quest;
    protected bool isInfoOpen;

    protected int userId;

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickToOpenQuestInfoWindow);
    }

    public virtual void Initialize(int userId, int questID, UI_QuestInfo questInfoWindow)
    {
        this.userId = userId;
        this.quest = QuestManager.Instance.GetQuestData(questID);
        this.questInfoWindow = questInfoWindow;
        questNameText.text = quest.Quest_Name;
        Q_Status? questStatus = QuestManager.Instance.GetQuestStatus(quest.Quest_ID);
        if (questStatus == null)
        {
            Debug.Log("퀘스트 아이디 잘못 됨");
        }
        completeText.text = questStatus.ToString();
    }

    /// <summary>
    /// 퀘스트를 클릭하여 퀘스트 상세정보 열기
    /// </summary>
    protected virtual void OnClickToOpenQuestInfoWindow()
    {
        isInfoOpen = !questInfoWindow.gameObject.activeSelf;
        questInfoWindow.gameObject.SetActive(isInfoOpen);                
    }
}
