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
        Debug.Log("�Ƴ�");
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
    /// ����Ʈ ���� ��� â���� ��ã�� ��ư�� Ŭ���ϸ�
    /// </summary>
    private void OnClickNavButton(bool isOn)
    {
        Debug.Log("��ư Ŭ��");
        if (isOn)
        {
            EventHandler.questNavEvent.TriggerQuestNav(this.quest);
        }        
    }
}
