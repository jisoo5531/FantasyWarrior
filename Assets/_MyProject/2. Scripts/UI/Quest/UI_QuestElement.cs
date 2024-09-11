using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_QuestElement : MonoBehaviour
{
    // TODO : 0911. ����Ʈ ����. 
    // Ư�� ����Ʈ�� ���൵ �Ǵ�. �Ϸ� ����. ��ǥ�� ���缭 �Ϸ� �Ǵ���
    /// <summary>
    /// ���̵忡 ���� ���� ��ư
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
            Debug.Log("����Ʈ ���̵� �߸� ��");
        }
        completeText.text = questStatus.ToString();
    }
    
}
