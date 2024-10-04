using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AllQuest : UI_QuestElement
{
    protected override void OnClickToOpenQuestInfoWindow()
    {
        base.OnClickToOpenQuestInfoWindow();
        if (isInfoOpen)
        {
            questInfoWindow.Initialize(userId, this.quest, q_Status: null);
        }        
    }
}
