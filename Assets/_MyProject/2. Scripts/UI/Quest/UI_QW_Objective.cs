using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QW_Objective : MonoBehaviour
{
    public TMP_Text questObjectiveText;

    public void Initialize(QuestsData quest)
    {
        int? currentProgress = QuestManager.Instance.GetCurrentQuestProgress(quest.Quest_ID);
        int? require = QuestManager.Instance.GetRequireCompleteQuest(quest.Quest_ID);
        if (currentProgress == null || require == null)
        {
            return;
        }

        questObjectiveText.text = $"{quest.DESC} {currentProgress}/{require}";
    }
}
