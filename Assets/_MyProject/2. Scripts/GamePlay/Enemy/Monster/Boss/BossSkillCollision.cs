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
        Debug.Log("어떤거 맞음");
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {
            return;
        }
        if (skillCC == Status_Effect.Airbone)
        {
            Debug.Log("에어본 된다.");
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
        else if (skillCC == Status_Effect.NuckBack)
        {
            other.GetComponent<Rigidbody>().AddForce(Vector3.back * 10f, ForceMode.Impulse);
        }
        // 퀘스트 진행상황 업데이트는 맞은 놈이 아닌 때린 놈이 판정
        if (other.TryGetComponent(out Damagable damagable))
        {
            Debug.Log("파티클 맞았다.");
            damagable.GetDamage(damage);
            //transform.root.GetComponentInChildren<PlayerSkill>().skillList[skill_Order].SkillSendDamage(damagable);
            //damagable.GetDamage(damage);
            Debug.Log("여기 되나요?");
        }
    }
}
