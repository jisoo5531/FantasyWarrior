using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : Weapon
{

    protected override void OnTriggerEnter(Collider other)
    {
        if ((targetLayer | (1 << other.gameObject.layer)) != targetLayer)
        {            
            return;
        }
        // 퀘스트 진행상황 업데이트는 맞은 놈이 아닌 때린 놈이 판정
        if (other.TryGetComponent(out Damagable damagable))
        {
            damagable.GetDamage(damage);
            if (damagable.Hp <= 0)
            {
                Debug.Log("여기 안되나?");
                QuestManager.Instance.UpdateQuestProgress(unitID: damagable.Unit_ID);
            }
        }
    }

}
