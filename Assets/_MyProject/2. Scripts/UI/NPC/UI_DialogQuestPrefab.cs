using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_DialogQuestPrefab : MonoBehaviour
{
    public TMP_Text dialogNameLabel;

    /// <summary>
    /// � npc�� ��ȭ ������
    /// </summary>
    private int npc_ID;
    /// <summary>
    /// �� ��ũ��Ʈ�� ����Ǵ� ����Ʈ�� ID
    /// </summary>
    private QuestData quest;
    /// <summary>
    /// �� ������Ʈ�� ������ ��ư�� ������ ������ �ݹ� �Լ�
    /// </summary>
    private Action<DialogStatus, int> clickStartDialog;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickQuestDialog);
    }
    public void Initialize(int npcID, QuestData quest, Action<DialogStatus, int> SelectDialog)
    {
        this.npc_ID = npcID;
        this.quest = quest;
        dialogNameLabel.text = quest.Quest_Name;
        clickStartDialog = SelectDialog;
    }

    private void OnClickQuestDialog()
    {
        Debug.Log($"Ŭ���ߴ�. {this.quest.Quest_Name}");        
        List<QuestProgress> userQuestList = QuestManager.Instance.questProgressList;
        QuestProgress userQuest = userQuestList.Find(x => x.quest_Id == this.quest.Quest_ID);
        if (userQuest == null)
        {
            Debug.Log("Ȥ�� null?");
            // ���� ������ �ش� ����Ʈ�� ���� �ʾҴٸ�
            clickStartDialog(DialogStatus.QuestStart, this.quest.Quest_ID);
        }
        else if (userQuest.IsQuestComplete())
        {
            // ���� ������ �ش� ����Ʈ�� ���� ���̶��
            // �ش� ����Ʈ�� �Ϸ� �����ϴٸ�
            clickStartDialog(DialogStatus.QuestEnd, this.quest.Quest_ID);
        }
        CheckTalkQuest();
    }
    /// <summary>
    /// �� NPC���� �� ��ȭ ����Ʈ �ִ��� Ȯ��
    /// </summary>
    private void CheckTalkQuest()
    {
        List<NPCTalkQuestData> talkList = NPCManager.Instance.talkNpcQuestList;
        // �� NPC���� �� ��ȭ ����Ʈ �ִ��� Ȯ��
        NPCTalkQuestData talkQuest = talkList.Find(x => x.NPC_ID == this.npc_ID);

        if (talkQuest != null)
        {
            // �ش�Ǵ� ����Ʈ�� ������            
            // �ش� ����Ʈ�� �Ϸ� ��ȭ ����.
            clickStartDialog(DialogStatus.QuestEnd, talkQuest.Quest_ID);
        }
    }
}
