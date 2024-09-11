using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ŭ���̾�Ʈ�� �ӽ÷� ������ ���� ����Ʈ ���൵
/// <para>����Ʈ�� �����ϸ� �ν��Ͻ� ����</para>
/// </summary>
public class QuestProgress : MonoBehaviour
{
    public int quest_Id { get; private set; }         // ����Ʈ ID
    public int monster_Id { get; private set; }       // Ÿ���� �Ǵ� ���� ID (������ 0)
    public int Item_Id { get; private set; }         // Ÿ���� �Ǵ� ������ ID (������ 0)
    public int? required_Amount { get; private set; }
    public int current_Amount { get; private set; }

    // ������
    public QuestProgress(int questId, int? requiredAmount)
    {
        this.quest_Id = questId;
        this.required_Amount = requiredAmount;
        this.current_Amount = 0;
        QuestObjectivesData questObjective = QuestManager.Instance.GetObjectiveData(quest_Id);

        this.monster_Id = questObjective.Monster_ID;
        this.Item_Id = questObjective.Item_ID;
    }

    /// <summary>
    /// ���൵ ������Ʈ
    /// </summary>
    public void UpdateProgress()
    {
        current_Amount++;        
    }

    // ����Ʈ �Ϸ� ���� Ȯ��
    public bool IsQuestComplete()
    {        
        return current_Amount >= required_Amount;
    }
}
