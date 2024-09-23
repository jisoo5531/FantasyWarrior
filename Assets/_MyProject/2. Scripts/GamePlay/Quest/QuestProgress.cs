using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ŭ���̾�Ʈ�� �ӽ÷� ������ ���� ����Ʈ ���൵
/// <para>����Ʈ�� �����ϸ� �ν��Ͻ� ����</para>
/// </summary>
public class QuestProgress
{
    public int quest_Id { get; private set; }         // ����Ʈ ID
    public int monster_Id { get; private set; }       // Ÿ���� �Ǵ� ���� ID (������ 0)
    public int Item_Id { get; private set; }         // Ÿ���� �Ǵ� ������ ID (������ 0)
    public int NPC_Id { get; private set; }         // Ÿ���� �Ǵ� NPC ID
    public int? required_Amount { get; private set; }
    public int? current_Amount { get; private set; }
    public bool isGuide { get; private set; }
    public Q_ObjectiveType questType { get; private set; }

    // ������
    public QuestProgress(int questId, int? currentAmount = null)
    {
        this.quest_Id = questId;
        this.current_Amount = currentAmount;
        Initialize();
        SetTargetID();
    }
    private void ResetValue()
    {
        this.monster_Id = 0;
        this.Item_Id = 0;
        this.NPC_Id = 0;
    }
    private void Initialize()
    {
        ResetValue();
        QuestObjectiveData questObjective = QuestManager.Instance.GetObjectiveData(quest_Id);
        this.questType = questObjective.ObjectiveType;        
        this.isGuide = false;
    }
    private void SetTargetID()
    {        
        QuestObj_KillData killQuest = QuestManager.Instance.GetKillQuestInfo(this.quest_Id);
        if (killQuest != null)
        {
            this.monster_Id = killQuest.Monster_ID;
            this.required_Amount = killQuest.KillAmount;
        }
        Questobj_CollectData collectQuest = QuestManager.Instance.GetCollectQuestInfo(this.quest_Id);
        if (collectQuest != null)
        {
            this.Item_Id = collectQuest.Item_ID;
            this.required_Amount = collectQuest.CollectAmount;
        }
        QuestObj_TalkData talkQuest = QuestManager.Instance.GetTalkQuestInfo(this.quest_Id);        
        if (talkQuest != null)
        {
            this.NPC_Id = talkQuest.NPC_ID;
        }        
    }

    /// <summary>
    /// ���൵ ������Ʈ
    /// </summary>
    public void UpdateProgress()
    {
        current_Amount++;        
    }
    /// <summary>
    /// �˸��̷� ����� ������
    /// </summary>
    public void UpdateGuideOnOff(bool OnOff)
    {
        isGuide = OnOff;
    }

    /// <summary>
    /// ����Ʈ �Ϸ������� �޼��Ͽ��ٸ�
    /// </summary>
    /// <returns></returns>
    public bool IsQuestComplete()
    {        
        return current_Amount >= required_Amount;
    }
}