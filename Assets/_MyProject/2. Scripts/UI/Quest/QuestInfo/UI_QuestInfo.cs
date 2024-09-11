using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestInfo : MonoBehaviour
{
    
    protected QuestsData questData;
    
    public virtual void Initialize(QuestsData questData)
    {
        this.questData = questData;
        //QuestProgress que
    }
}
