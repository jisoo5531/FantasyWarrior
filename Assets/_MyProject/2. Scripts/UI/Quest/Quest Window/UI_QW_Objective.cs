using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QW_Objective : MonoBehaviour
{
    public TMP_Text questObjectiveText;

    public void Initialize(QuestsData quest, QuestProgress questProgress)
    {
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
}
