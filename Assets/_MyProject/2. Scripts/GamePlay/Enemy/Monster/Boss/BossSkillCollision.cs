using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillCollision : MonoBehaviour
{
    public int damage;
    public int skill_Order;
    public LayerMask targetLayer;
    public Status_Effect skillCC;

    private void OnTriggerEnter(Collider other)
    {        
        Debug.Log("��� ����");
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            return;
        }
        if (skillCC == Status_Effect.Airbone)
        {
            Debug.Log("��� �ȴ�.");
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
        else if (skillCC == Status_Effect.NuckBack)
        {
            other.GetComponent<Rigidbody>().AddForce(Vector3.back * 10f, ForceMode.Impulse);
        }
        // ����Ʈ �����Ȳ ������Ʈ�� ���� ���� �ƴ� ���� ���� ����
        if (other.TryGetComponent(out Damagable damagable))
        {
            Debug.Log("��ƼŬ �¾Ҵ�.");
            damagable.GetDamage(damage);
            //transform.root.GetComponentInChildren<PlayerSkill>().skillList[skill_Order].SkillSendDamage(damagable);
            //damagable.GetDamage(damage);
            Debug.Log("���� �ǳ���?");
        }
    }
}
