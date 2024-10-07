using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillCollision : MonoBehaviour
{
    public int skill_Order;
    public LayerMask targetLayer;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("��� ����");
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            return;
        }

        // ����Ʈ �����Ȳ ������Ʈ�� ���� ���� �ƴ� ���� ���� ����
        if (other.TryGetComponent(out Damagable damagable))
        {
            Debug.Log("��ƼŬ �¾Ҵ�.");
            //transform.root.GetComponentInChildren<PlayerSkill>().skillList[skill_Order].SkillSendDamage(damagable);
            //damagable.GetDamage(damage);
            Debug.Log("���� �ǳ���?");
        }
    }
}
