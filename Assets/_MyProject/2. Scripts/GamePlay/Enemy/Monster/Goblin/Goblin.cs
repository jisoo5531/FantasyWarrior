using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Goblin : MonsterUnit
{    
    int rewardExp = 1000;
    protected override void Initialize()
    {
        monsterID = 1;
        M_StateMachine = new MonsterStateMachine(this);
        M_StateMachine.Initialize(M_StateMachine.idleState);

        base.Initialize();                          
    }

    private void Update()
    {
        if (damagable.isStunned)
        {
            Debug.Log("얘 스턴이다. 못 움직여");
            M_StateMachine.StateTransition(M_StateMachine.idleState);
            return;
        }
        if (damagable.Hp > 0)
        {
            M_StateMachine.Excute();
        }
    }
    protected override void OnHpChange(int damage)
    {
        base.OnHpChange(damage);
    }

    // TODO : 몬스터가 죽었을 때 아이템, 경험치, 골드 등등 보상 떨어지게
    // TODO : 아이템 랜덤하게?
    protected override void OnDeath()
    {
        nav.isStopped = true;
        ItemData dropItem = ItemManager.Instance.GetItemData(8);

        GameObject rewardItemOBJ = Resources.Load<GameObject>($"Items/{dropItem.Item_Name}");
        GameObject itemObj = Instantiate(rewardItemOBJ, transform.position + new Vector3(0, 0.5f, 0), rewardItemOBJ.transform.rotation);
        Item rewardItem = itemObj.GetComponent<Item>();
        rewardItem?.Initialize(ItemManager.Instance.GetItemData(monsterData.Item_Reward));
        UserStatManager.Instance.UpdateExp(rewardExp);
        //item.GetComponent<Rigidbody>().AddForce(Vector3.up * 20f, ForceMode.Impulse);

        base.OnDeath();
    }
}