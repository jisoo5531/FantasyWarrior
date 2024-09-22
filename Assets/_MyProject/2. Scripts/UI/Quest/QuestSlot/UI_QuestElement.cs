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

    protected QuestData quest;
    protected bool isInfoOpen;

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickToOpenQuestInfoWindow);
    }

    public virtual void Initialize(int questID, UI_QuestInfo questInfoWindow)
    {
        this.quest = QuestManager.Instance.GetQuestData(questID);
        this.questInfoWindow = questInfoWindow;
        questNameText.text = quest.Quest_Name;
        Q_Status? questStatus = QuestManager.Instance.GetQuestStatus(quest.Quest_ID);
        if (questStatus == null)
        {
            Debug.Log("����Ʈ ���̵� �߸� ��");
        }
        completeText.text = questStatus.ToString();
    }

    /// <summary>
    /// ����Ʈ�� Ŭ���Ͽ� ����Ʈ ������ ����
    /// </summary>
    protected virtual void OnClickToOpenQuestInfoWindow()
    {
        isInfoOpen = !questInfoWindow.gameObject.activeSelf;
        questInfoWindow.gameObject.SetActive(isInfoOpen);                
    }
}
