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
    public int? required_Amount { get; private set; }
    public int? current_Amount { get; private set; }
    public bool isGuide { get; private set; }

    // ������
    public QuestProgress(int questId, int? currentAmount, int? requiredAmount)
    {
        this.quest_Id = questId;
        this.required_Amount = requiredAmount;
        this.current_Amount = currentAmount;
        QuestObjectivesData questObjective = QuestManager.Instance.GetObjectiveData(quest_ID: quest_Id);

        this.monster_Id = questObjective.Monster_ID;
        this.Item_Id = questObjective.Item_ID;
        this.isGuide = false;
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