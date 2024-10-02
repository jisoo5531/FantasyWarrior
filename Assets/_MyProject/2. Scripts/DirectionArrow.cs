using UnityEngine;
using System.Collections.Generic;

public class DirectionArrow : MonoBehaviour
{
    public Transform player;       // �÷��̾��� Transform
    public Transform target;       // ��ǥ ������ Transform
    public RectTransform arrowUI;  // ȭ��ǥ �̹����� RectTransform

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
                
        // ��ǥ ������ �÷��̾��� ��ġ ���̸� ���ͷ� ���
        Vector3 direction = target.position - player.position;
        direction.y = 0; // ���� ���̸� �����ϰ� ��� ���� ���⸸ ���

        // ���� ���͸� ������ ��ȯ
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // ȭ��ǥ UI�� ������ ���� ȸ���ϵ�, y�� ȸ���� 0���� ����
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
