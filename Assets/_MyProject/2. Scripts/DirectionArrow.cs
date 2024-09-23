using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DirectionArrow : MonoBehaviour
{
    public Transform player;       // �÷��̾��� Transform
    public Transform target;       // ��ǥ ������ Transform
    public RectTransform arrowUI;  // ȭ��ǥ �̹����� RectTransform

    private void Awake()
    {
        EventHandler.questNavEvent.RegisterQuestNav(OnSetQuestNavTarget);
    }
    private void Start()
    {
        
    }

    void Update()
    {
        // ��ǥ ������ �÷��̾��� ��ġ ���̸� ���ͷ� ���
        Vector3 direction = target.position - player.position;
        direction.y = 0; // ���� ���̸� �����ϰ� ��� ���� ���⸸ ���

        // ���� ���͸� ������ ��ȯ
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // ȭ��ǥ UI�� ������ ���� ȸ��
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
        Debug.Log(quest_ID);
        int index = questProgress.FindIndex(x => x.quest_Id == quest_ID);
        Debug.Log(questProgress[index].NPC_Id);
        if (questProgress[index].monster_Id != 0)
        {
            
        }
        else if (questProgress[index].NPC_Id != 0)
        {
            NPC npc = LocationManger.Instance.NPC_List.Find(x => x.NPC_ID == questProgress[index].NPC_Id);
            Debug.Log(npc.name);
            if (npc != null)
            {
                target = npc.transform;
            }            
        }
    }
}
