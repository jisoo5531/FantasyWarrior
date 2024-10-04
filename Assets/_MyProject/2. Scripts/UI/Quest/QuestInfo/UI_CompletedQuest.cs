using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CompletedQuest : UI_QuestElement
{
    protected override void OnClickToOpenQuestInfoWindow()
    {
        base.OnClickToOpenQuestInfoWindow();
        if (isInfoOpen)
        {
            questInfoWindow.Initialize(this.userId, this.quest, Q_Status.Completed);
        }
        
    }
}
