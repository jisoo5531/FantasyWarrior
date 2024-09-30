using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QW_Objective : MonoBehaviour
{
    public TMP_Text questObjectiveText;
    public Toggle navButton;
    private QuestData quest;
    private void Awake()
    {
        Debug.Log("됐나");
        navButton.group = transform.parent.GetComponent<ToggleGroup>();
        navButton.onValueChanged.AddListener(OnClickNavButton);
    }

    public void Initialize(QuestData quest, QuestProgress questProgress)
    {
        this.quest = quest;
        int? currentProgress = questProgress.current_Amount;
        int? require = questProgress.required_Amount;
        if (require == null)
        {
            return;
        }
        if (questProgress.IsQuestComplete())
        {
            questObjectiveText.fontStyle = FontStyles.Strikethrough;
        }        
        questObjectiveText.text = $"{quest.DESC} {currentProgress}/{require}";
    }
    /// <summary>
    /// 퀘스트 빠른 등록 창에서 길찾기 버튼을 클릭하면
    /// </summary>
    private void OnClickNavButton(bool isOn)
    {
        Debug.Log("버튼 클릭");
        if (isOn)
        {
            EventHandler.questNavEvent.TriggerQuestNav(this.quest);
        }        
    }
}
