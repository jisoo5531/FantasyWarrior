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
        // ����Ʈ �����Ȳ ������Ʈ�� ���� ���� �ƴ� ���� ���� ����
        if (other.TryGetComponent(out Damagable damagable))
        {
            damagable.GetDamage(damage);
            if (damagable.Hp <= 0)
            {
                Debug.Log("���� �ȵǳ�?");
                QuestManager.Instance.UpdateQuestProgress(unitID: damagable.Unit_ID);
            }
        }
    }

}
