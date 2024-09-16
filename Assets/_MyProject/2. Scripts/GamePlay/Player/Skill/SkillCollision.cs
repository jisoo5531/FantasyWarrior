using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCollision : MonoBehaviour
{    
    public int damage;
    public LayerMask targetLayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            return;
        }
        
        // ����Ʈ �����Ȳ ������Ʈ�� ���� ���� �ƴ� ���� ���� ����
        if (other.TryGetComponent(out Damagable damagable))
        {
            Debug.Log("��ƼŬ �¾Ҵ�.");
            transform.root.GetComponentInChildren<PlayerSkill>().skillList[0].SkillSendDamage(damagable);
            //damagable.GetDamage(damage);
            if (damagable.Hp <= 0)
            {
                Debug.Log("���� �ȵǳ�?");
                QuestManager.Instance.UpdateQuestProgress(unitID: damagable.Unit_ID);
            }
        }
    }
}
