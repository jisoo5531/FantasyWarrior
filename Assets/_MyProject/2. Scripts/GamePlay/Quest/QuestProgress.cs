using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 클라이언트에 임시로 저장할 현재 퀘스트 진행도
/// <para>퀘스트를 시작하면 인스턴스 생성</para>
/// </summary>
public class QuestProgress : MonoBehaviour
{
    public int quest_Id { get; private set; }         // 퀘스트 ID
    public int monster_Id { get; private set; }       // 타겟이 되는 몬스터 ID (없으면 0)
    public int Item_Id { get; private set; }         // 타겟이 되는 아이템 ID (없으면 0)
    public int? required_Amount { get; private set; }
    public int current_Amount { get; private set; }

    // 생성자
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
    /// 진행도 업데이트
    /// </summary>
    public void UpdateProgress()
    {
        current_Amount++;        
    }

    // 퀘스트 완료 여부 확인
    public bool IsQuestComplete()
    {        
        return current_Amount >= required_Amount;
    }
}
