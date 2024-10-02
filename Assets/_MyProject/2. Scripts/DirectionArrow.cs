using UnityEngine;
using System.Collections.Generic;

public class DirectionArrow : MonoBehaviour
{
    public Transform player;       // 플레이어의 Transform
    public Transform target;       // 목표 지점의 Transform
    public RectTransform arrowUI;  // 화살표 이미지의 RectTransform

    private void OnEnable()
    {
        EventHandler.questNavEvent.RegisterQuestNav(OnSetQuestNavTarget);
    }
    private void OnDisable()
    {
        EventHandler.questNavEvent.UnRegisterQuestNav(OnSetQuestNavTarget);
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }
                
        // 목표 지점과 플레이어의 위치 차이를 벡터로 계산
        Vector3 direction = target.position - player.position;
        direction.y = 0; // 높이 차이를 무시하고 평면 상의 방향만 계산

        // 방향 벡터를 각도로 변환
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // 화살표 UI를 각도에 맞춰 회전하되, y축 회전은 0으로 설정
        arrowUI.rotation = Quaternion.Euler(0, 0, -angle);
    }

    private void OnSetQuestNavTarget(QuestData quest)
    {
        int quest_ID = quest.Quest_ID;
        SetTarget(quest_ID);
    }

    private void SetTarget(int quest_ID)
    {
        List<QuestProgress> questProgress = QuestManager.Instance.questProgressList;
        int index = questProgress.FindIndex(x => x.quest_Id == quest_ID);
        if (questProgress[index].NPC_Id != 0)
        {
            NPC npc = LocationManger.Instance.NPC_List.Find(x => x.NPC_ID == questProgress[index].NPC_Id);
            if (npc != null)
            {
                target = npc.transform;
            }
        }
    }
}
